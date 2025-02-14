using Report.API.Services;

namespace Report.API
{
    public class Worker:BackgroundService
    {
        private readonly MessageListener _listener;

        public Worker(MessageListener listener)
        {
            _listener = listener;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _listener.Start();
            return Task.CompletedTask;
        }
    }
}
