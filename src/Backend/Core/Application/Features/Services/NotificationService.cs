using Domain.Entities;
using Microsoft.Extensions.Logging;

namespace Application.Features.Services
{
    public class NotificationService
    {
        private readonly ILogger<NotificationService> _logger;

        public NotificationService(ILogger<NotificationService> logger)
        {
            _logger = logger;
        }

        public async Task SendNotification(OrderInstruction instruction)
        {
            foreach (var channel in instruction.NotificationChannels)
            {
                await SendHttpNotification(channel);
                LogNotification(instruction, channel);
            }
        }

        private async Task SendHttpNotification(NotificationChannel channel)
        {
            Console.WriteLine($"Bildirim gönderildi: {channel.Type}");
            // HttpClient bildirim gönderme
            _logger.LogInformation($"Bildirim gönderildi: {channel.Type}");
        }

        private void LogNotification(OrderInstruction instruction, NotificationChannel channel)
        {
            Console.WriteLine($"Bildirim gönderildi: {channel.Type}");
            _logger.LogInformation($"Bildirim gönderildi: {channel.Type}");
            // Bildirim loglama
        }
    }
}
