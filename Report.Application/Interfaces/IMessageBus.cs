namespace Report.Application.Interfaces;

public interface IMessageBus
{
    Task  PublishReportRequestAsync(Guid reportId);
}
