using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;

namespace SourceBit.Inject
{
    public partial class Container
    {
        public delegate object Activator(params object[] args);

        public Activator CreateActivator(Type type, out List<Type> dependencies)
        {
            dependencies = new List<Type>();

            ConstructorInfo constructorInfo = type.GetConstructors()[0];

            ParameterInfo[] parameters = constructorInfo.GetParameters();

            var method = new DynamicMethod("CreateInstance", typeof(object), new[] { typeof(object[]) });

            ILGenerator generator = method.GetILGenerator();

            for (int index = 0; index < parameters.Length; index++)
            {
                Type parameterType = parameters[index].ParameterType;

                dependencies.Add(parameterType);

                generator.Emit(OpCodes.Ldarg_0);
                generator.Emit(OpCodes.Ldc_I4, index);
                generator.Emit(OpCodes.Ldelem_Ref);
                generator.Emit(parameterType.IsValueType ? OpCodes.Unbox_Any : OpCodes.Castclass, parameterType);
            }

            generator.Emit(OpCodes.Newobj, constructorInfo);
            generator.Emit(OpCodes.Ret);

            var activator = method.CreateDelegate(typeof(Activator)) as Activator;

            return activator;
        }
    }
}
