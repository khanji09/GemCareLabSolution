using GemCare.API.Interfaces;
using GemCare.Data.Interfaces;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GemCare.API.Helper
{
    public class BackgroundNotificationService : IHostedService, IDisposable
    {
        private readonly IPushNotificationRepository _pushRepository;
        private readonly IPushService _pushService;
        private Timer _timer;
        public BackgroundNotificationService(IPushNotificationRepository pushRepository, 
            IPushService pushService)
        {
            _pushRepository = pushRepository;
            _pushService = pushService;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            if (SiteKeys.Environment.Equals("Production"))
            {
                _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(30));
            }
            return Task.CompletedTask;
        }

        private void DoWork(object state)
        {
            try
            {
                var (status, message, deviceInfo) = _pushRepository.GetAdminDeviceInfoForBookingNotification(1);
                if (deviceInfo != null)
                {
                    var (notificationSent, errMessage) = _pushService.SendBookingNotificationToAdmin(deviceInfo.PushTitle,
                        deviceInfo.PushBody, deviceInfo.PushToken, deviceInfo.DevicePlatform);
                    if (notificationSent)
                    {
                        _ = _pushRepository.UpdateBookingNotificationStatus();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException);
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
