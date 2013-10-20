using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace SourceBit.Inject
{
    /// <summary>
    /// Represents the options of the registered type in the IoC container.
    /// </summary>
    public class Registration : IRegistration
    {
        private readonly Type _type;
        private readonly List<Type> _asTypes;
        private readonly Action<Registration> _register;
        private readonly Func<Type, object> _resolveAction;
        private readonly Activator _activator;
        private readonly List<Type> _dependencies;

        public Registration(Action<Registration> register, Func<Type, object> resolveAction, Type type, params Type[] asTypes)
        {
            _register = register;
            _resolveAction = resolveAction;

            _type = type.IsGenericType ? type.GetGenericTypeDefinition() : type;

            _asTypes = asTypes.Length < 1 ? new List<Type> { type } : asTypes.ToList();

            _dependencies = new List<Type>();

            // By default use single instance resolving stratagy type
            ResolvingStrategyType = 0;

            ConstructorInfo constructorInfo = _type.GetConstructors()[0];

            ParameterInfo[] parameters = constructorInfo.GetParameters();

            var method = new DynamicMethod("CreateInstance", typeof(object), new Type[] { typeof(object[]) });

            ILGenerator generator = method.GetILGenerator();

            for (int i = 0; i < parameters.Length; i++)
            {
                Type parameterType = parameters[i].ParameterType;

                _dependencies.Add(parameterType);

                generator.Emit(OpCodes.Ldarg_0); // Push array (method argument)
                generator.Emit(OpCodes.Ldc_I4, i); // Push i
                generator.Emit(OpCodes.Ldelem_Ref); // Pop array and i and push array[i]
                generator.Emit(parameterType.IsValueType ? OpCodes.Unbox_Any : OpCodes.Castclass, parameterType);
            }

            generator.Emit(OpCodes.Newobj, constructorInfo);
            generator.Emit(OpCodes.Ret);

            _activator = method.CreateDelegate(typeof(Activator)) as Activator;
        }

        public Type Type
        {
            get { return _type; }
        }

        public List<Type> AsTypes
        {
            get { return _asTypes; }
        }

        public Action<Registration> Register
        {
            get { return _register; }
        }

        public byte ResolvingStrategyType { get; set; }

        public void AsSingleInstance()
        {
            _register(this);
        }

        public void AsPerDependencyInstance()
        {
            ResolvingStrategyType = 1;

            _register(this);
        }

        public void AsPerThreadInstance()
        {
            ResolvingStrategyType = 2;

            _register(this);
        }

        public object CreateInstance()
        {
            int dependenciesLength = _dependencies.Count;

            var parameters = new object[dependenciesLength];

            for (int index = 0; index < dependenciesLength; index++)
            {
                parameters[index] = _resolveAction(_dependencies[index]);
            }

            return _activator(parameters);
        }
    }
}
