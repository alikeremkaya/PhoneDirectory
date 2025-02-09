using MassTransit;
using Shared.Messages;

namespace Report.Application.Interfaces
{
    public class MessageBus
    {
        private readonly IPublishEndpoint _publishEndpoint;

        public MessageBus(IPublishEndpoint publishEndpoint)
        {
            _publishEndpoint = publishEndpoint;
        }

        public async Task PublishReportRequestAsync(Guid reportId)
        {
            await _publishEndpoint.Publish(new ReportRequestMessage
            {
                ReportId = reportId,
                RequestDate = DateTime.UtcNow
            });
        }
    }
}
