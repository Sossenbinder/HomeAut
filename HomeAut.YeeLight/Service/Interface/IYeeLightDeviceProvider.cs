using HomeAuth.Common.Async;
using YeelightAPI;

namespace HomeAut.YeeLight.Service.Interface
{
    public interface IYeeLightDeviceProvider
    {
        AsyncLazy<Device> Device { get; }

        void Disconnect();
    }
}
