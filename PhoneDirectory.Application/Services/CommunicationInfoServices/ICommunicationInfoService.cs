using PhoneDirectory.Application.DTOs.CommunicationInfoDTOs;
using PhoneDirectory.Domain.Utilities.Interfaces;

namespace PhoneDirectory.Application.Services.CommunicationInfoServices;

public interface ICommunicationInfoService
{
    /// <summary>
    /// Belirtilen kişiye ait yeni bir iletişim bilgisi oluşturur.
    /// </summary>
    /// <param name="personId">İletişim bilgisi eklenecek kişinin benzersiz kimliği.</param>
    /// <param name="dto">Oluşturulacak iletişim bilgisinin detaylarını içeren DTO.</param>
    /// <returns>Oluşturulan iletişim bilgisiyle birlikte işlem sonucunu döner.</returns>
    Task<IDataResult<CommunicationInfoDTO>> CreateCommunicationInfoAsync(Guid personId, CommunicationInfoCreateDTO dto);

    /// <summary>
    /// Belirtilen kişiye ait belirli bir iletişim bilgisini getirir.
    /// </summary>
    /// <param name="personId">Kişinin benzersiz kimliği.</param>
    /// <param name="communicationInfoId">Getirilecek iletişim bilgisinin benzersiz kimliği.</param>
    /// <returns>Belirtilen iletişim bilgisiyle birlikte işlem sonucunu döner.</returns>
    Task<IDataResult<CommunicationInfoDTO>> GetCommunicationInfoByIdAsync(Guid personId, Guid communicationInfoId);

    /// <summary>
    /// Belirtilen kişiye ait tüm iletişim bilgilerini getirir.
    /// </summary>
    /// <param name="personId">Kişinin benzersiz kimliği.</param>
    /// <returns>Kişiye ait iletişim bilgileri listesini içeren veri sonucu döner.</returns>
    Task<IDataResult<List<CommunicationInfoDTO>>> GetCommunicationInfosByPersonIdAsync(Guid personId);

    /// <summary>
    /// Belirtilen kişiye ait iletişim bilgisini günceller.
    /// </summary>
    /// <param name="personId">İletişim bilgisi güncellenecek kişinin benzersiz kimliği.</param>
    /// <param name="dto">Güncellenecek iletişim bilgisi detaylarını içeren DTO.</param>
    /// <returns>İşlem sonucunu döner.</returns>
    Task<IResult> UpdateCommunicationInfoAsync(Guid personId, CommunicationInfoUpdateDTO dto);

    /// <summary>
    /// Belirtilen kişiye ait belirli bir iletişim bilgisini siler.
    /// </summary>
    /// <param name="personId">İletişim bilgisi silinecek kişinin benzersiz kimliği.</param>
    /// <param name="communicationInfoId">Silinecek iletişim bilgisinin benzersiz kimliği.</param>
    /// <returns>İşlem sonucunu döner.</returns>
    Task<IResult> DeleteCommunicationInfoAsync(Guid personId, Guid communicationInfoId);

}
