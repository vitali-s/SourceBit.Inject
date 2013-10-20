SourceBit.Inject
================
SourceBit.Inject

As usual the work is still in progress, but some basement is already implemented.

To register types use:

var container = new Container();
container.Register<Service, IService>().AsSingleInstance();

container.Resolve<IService>();
