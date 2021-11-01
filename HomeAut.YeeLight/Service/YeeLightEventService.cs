using HomeAut.YeeLight.Service.Interface;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using YeelightAPI;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using YeelightAPI.Models;

namespace HomeAut.YeeLight.Service
{
    public class YeeLightEventService : IYeeLightEventService
    {
	    private readonly ILogger<YeeLightEventService> _logger;

	    private readonly IYeeLightDeviceProvider _deviceProvider;

	    private readonly IConfiguration _configuration;

	    private readonly HttpClient _httpClient;

	    public YeeLightEventService(
		    ILogger<YeeLightEventService> logger, 
		    IYeeLightDeviceProvider deviceProvider,
		    IHttpClientFactory factory,
		    IConfiguration configuration)
	    {
		    _logger = logger;
		    _deviceProvider = deviceProvider;
		    _configuration = configuration;
		    _httpClient = factory.CreateClient();
	    }

		public async Task Initialize()
		{
			var device = await _deviceProvider.Device;

			device.OnNotificationReceived += async (_, args) =>
			{
				try
				{
					var eventParams = args.Result.Params;
					if (eventParams.TryGetValue(PROPERTIES.power, out var val) && val is "on")
					{
						await _httpClient.PostAsync($"{_configuration["DeploymentUri"]}/api/PlayOnAlexa?userToken={_configuration["UserToken"]}&code={_configuration["ApiKey"]}&playList={_configuration["PlayListId"]}", null);
					}
				}
				catch (Exception ex)
				{
					_logger.LogError(ex, "OnNotificationReceived threw an exception");
				}
			};
		}
    }
}
