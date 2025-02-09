namespace Report.Application.Interfaces.Services;

public record LocationStatistics(int PersonCount, int PhoneNumberCount);

public interface IPhoneDirectoryService
{
    Task<LocationStatistics> GetLocationStatistics(string location);
}
