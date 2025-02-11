using MassTransit;
using Report.Domain.Enums;
using Report.Infrastructure.Repositories;
using Shared.Messages;

namespace Report.API.Consumers;

public class ReportRequestConsumer : IConsumer<ReportRequestMessage>
{
    private readonly IReportRepository _reportRepository;
    private readonly IPublishEndpoint _publishEndpoint;

    public ReportRequestConsumer(
        IReportRepository reportRepository,
        IPublishEndpoint publishEndpoint)
    {
        _reportRepository = reportRepository;
        _publishEndpoint = publishEndpoint;
    }

    public async Task Consume(ConsumeContext<ReportRequestMessage> context)
    {
        try
        {
           
            var report = await _reportRepository.GetByIdAsync(context.Message.ReportId);

            if (report == null)
                return;

           
            var personCount = 10;
            var phoneNumberCount = 15;

            
            report.ReportStatus = ReportStatus.Completed;
            report.PersonCount = personCount;
            report.PhoneNumberCount = phoneNumberCount;
            report.CompletedDate = DateTime.UtcNow;

            await _reportRepository.UpdateAsync(report);
            await _reportRepository.SaveChangesAsync();

         
            await _publishEndpoint.Publish(new ReportCompletedMessage
            {
                ReportId = report.Id,
                Location = report.Location,
                PersonCount = personCount,
                PhoneNumberCount = phoneNumberCount,
                CompletedDate = DateTime.UtcNow
            });
        }
        catch (Exception ex)
        {
          
            throw;
        }
    }
}
