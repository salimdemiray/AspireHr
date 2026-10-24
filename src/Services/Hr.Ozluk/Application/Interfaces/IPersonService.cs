using Hr.Ozluk.Application.DTOs;

namespace Hr.Ozluk.Application.Interfaces;

public interface IPersonService
{
    Task<List<PersonDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<PersonDto?> GetByIdAsync(string id, CancellationToken cancellationToken = default);
    Task<PersonDto> CreateAsync(PersonCreateDto dto, string createdBy, CancellationToken cancellationToken = default);
    Task<PersonDto?> UpdateAsync(string id, PersonUpdateDto dto, string updatedBy, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(string id, CancellationToken cancellationToken = default);
}
