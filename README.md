SourceBit.Inject
================

This project grows from our internal dependency injection container based on attributes. Now it rewrites to support registration and autowiring. The focus of the container is simplicity and performance.

To register all assemblies types as registered interfaces with single instance life styles:

var container = new Container();
container.Register(Assembly.GetExecutingAssembly()).AsSingleInstance();
var service = container.Resolve<ISimpleService>();

Per dependency registration:

var container = new Container();
container.Register(Assembly.GetExecutingAssembly()).AsPerDependencyInstance();
var service = container.Resolve<ISimpleService>();

Register service:

var container = new Container();
container.Register<Service, IService>().AsSingleInstance();
container.Resolve<IService>();