using HomeAut.YeeLight.Service.Interface;
using HomeAuth.Common.Async;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using YeelightAPI;

namespace HomeAut.YeeLight.Service
{
    public class YeeLightDeviceProvider : IYeeLightDeviceProvider
    {
	    public AsyncLazy<Device> Device { get; }

	    private readonly ILogger<YeeLightDeviceProvider> _logger;

		private readonly CancellationTokenSource _cancellationTokenSource;
		
		public YeeLightDeviceProvider(
		    ILogger<YeeLightDeviceProvider> logger,
			IConfiguration configuration)
	    {
		    _logger = logger;
			_cancellationTokenSource = new CancellationTokenSource();
		    Device = new AsyncLazy<Device>(async () =>
		    {
				var device = new Device(configuration["YeeLightIp"]);

				await device.Connect();

				_ = RunReconnectLoop(device, _cancellationTokenSource.Token);

				return device;
		    });
		}
		public void Disconnect()
		{
			_cancellationTokenSource.Cancel();
		}

		private async Task RunReconnectLoop(Device device, CancellationToken cancellationToken)
		{
			while (!cancellationToken.IsCancellationRequested)
			{
				try
				{
					if (!device.IsConnected)
					{
						await device.Connect();
					}
				}
				catch (Exception ex)
				{
					_logger.LogError(ex, "Error in the YeeLight reconnecting loop");
				}

				await Task.Delay(500, cancellationToken);
			}
		}
    }
}
