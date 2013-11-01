SourceBit.Inject
================

This project grows from our internal dependency injection container based on attributes. Now it rewrites to support registration and autowiring. The focus of the container is simplicity and performance.

To register all assemblies types as registered interfaces with single instance life styles:

```sh
var container = new Container();
container.Register(Assembly.GetExecutingAssembly()).AsSingleInstance();
var service = container.Resolve<ISimpleService>();
```

Per dependency registration:

```sh
var container = new Container();
container.Register(Assembly.GetExecutingAssembly()).AsPerDependencyInstance();
var service = container.Resolve<ISimpleService>();
```

Register service:

```sh
var container = new Container();
container.Register<Service, IService>().AsSingleInstance();
container.Resolve<IService>();
```

Attribute based registration:

```sh
var container = new Container();
container.RegisterByAttributes(Assembly.GetExecutingAssembly());

[Inject]
public class SimpleService : ISimpleService
{

[Inject(InjectType.AsSelf)]
public class SelfSimpleService
{

[Inject(LifeTypes.PerDependency)]
public class PerDependencySimpleService : IPerDependencySimpleService
{
```
