using Autofac;
using Autofac.Extensions.DependencyInjection;
using HomeAut.YeeLight.Module;
using HomeAut.YeeLight.Service.Interface;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

var configuration = new ConfigurationBuilder()
	.AddEnvironmentVariables();

var serviceCollection = new ServiceCollection();
serviceCollection.AddHttpClient();
serviceCollection.AddLogging(loggingBuilder =>
{
	loggingBuilder.AddConsole();
});

var serviceProviderFactory = new AutofacServiceProviderFactory();
var containerBuilder = serviceProviderFactory.CreateBuilder(serviceCollection);
containerBuilder.RegisterInstance(configuration.Build())
	.As<IConfiguration>()
	.SingleInstance();
containerBuilder.RegisterModule<YeeLightModule>();

var container = containerBuilder.Build();
await using var scope = container.BeginLifetimeScope();

await scope.Resolve<IYeeLightEventService>().Initialize();

await Task.Delay(-1);
scope.Resolve<IYeeLightDeviceProvider>().Disconnect();