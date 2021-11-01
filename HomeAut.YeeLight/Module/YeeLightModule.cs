using Autofac;
using HomeAut.YeeLight.Service;
using HomeAut.YeeLight.Service.Interface;

namespace HomeAut.YeeLight.Module
{
    public class YeeLightModule : Autofac.Module
    {
	    protected override void Load(ContainerBuilder builder)
	    {
		    builder.RegisterType<YeeLightDeviceProvider>()
			    .As<IYeeLightDeviceProvider>()
			    .SingleInstance();

		    builder.RegisterType<YeeLightEventService>()
			    .As<IYeeLightEventService>()
			    .SingleInstance();
	    }
    }
}
