using Hr.Ozluk.Application.DTOs;
using Hr.Ozluk.Application.Interfaces;
using Hr.Ozluk.Domain.Entities;
using Hr.Ozluk.Domain.Enums;
using Hr.Ozluk.Infrastructure.Repositories;

namespace Hr.Ozluk.Application.Services;

public class PersonService : IPersonService
{
    private readonly IPersonRepository _repository;

    public PersonService(IPersonRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<PersonDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var list = await _repository.GetAllAsync(cancellationToken);
        return list.Select(MapToDto).ToList();
    }

    public async Task<PersonDto?> GetByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        var person = await _repository.GetByIdAsync(id, cancellationToken);
        return person == null ? null : MapToDto(person);
    }

    public async Task<PersonDto> CreateAsync(PersonCreateDto dto, string createdBy, CancellationToken cancellationToken = default)
    {
        var person = new Person
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            IdentityNumber = dto.IdentityNumber,
            BirthDate = dto.BirthDate,
            BirthPlace = dto.BirthPlace,
            Gender = dto.Gender.HasValue ? (Gender)dto.Gender.Value : null,
            MaritalStatus = dto.MaritalStatus.HasValue ? (MaritalStatus)dto.MaritalStatus.Value : null,
            FatherName = dto.FatherName,
            MotherName = dto.MotherName,
            Nationality = dto.Nationality,
            Note = dto.Note,
            CreatedBy = createdBy
        };

        var created = await _repository.CreateAsync(person, cancellationToken);
        return MapToDto(created);
    }

    public async Task<PersonDto?> UpdateAsync(string id, PersonUpdateDto dto, string updatedBy, CancellationToken cancellationToken = default)
    {
        var existing = await _repository.GetByIdAsync(id, cancellationToken);
        if (existing == null) return null;

        existing.FirstName = dto.FirstName;
        existing.LastName = dto.LastName;
        existing.IdentityNumber = dto.IdentityNumber;
        existing.BirthDate = dto.BirthDate;
        existing.BirthPlace = dto.BirthPlace;
        existing.Gender = dto.Gender.HasValue ? (Gender)dto.Gender.Value : null;
        existing.MaritalStatus = dto.MaritalStatus.HasValue ? (MaritalStatus)dto.MaritalStatus.Value : null;
        existing.FatherName = dto.FatherName;
        existing.MotherName = dto.MotherName;
        existing.Nationality = dto.Nationality;
        existing.Note = dto.Note;
        existing.UpdatedBy = updatedBy;

        var updated = await _repository.UpdateAsync(existing, cancellationToken);
        return updated == null ? null : MapToDto(updated);
    }

    public async Task<bool> DeleteAsync(string id, CancellationToken cancellationToken = default)
    {
        return await _repository.DeleteAsync(id, cancellationToken);
    }

    private static PersonDto MapToDto(Person person)
    {
        return new PersonDto(
            person.Id,
            person.FirstName,
            person.LastName,
            person.IdentityNumber,
            person.BirthDate,
            person.BirthPlace,
            person.Gender.HasValue ? (int)person.Gender.Value : null,
            person.MaritalStatus.HasValue ? (int)person.MaritalStatus.Value : null,
            person.FatherName,
            person.MotherName,
            person.Nationality,
            person.Note,
            person.CreatedAt
        );
    }
}
