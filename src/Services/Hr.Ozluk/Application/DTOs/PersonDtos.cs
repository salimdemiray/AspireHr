namespace Hr.Ozluk.Application.DTOs;

public record PersonCreateDto(
    string FirstName,
    string LastName,
    string? IdentityNumber,
    DateOnly? BirthDate,
    string? BirthPlace,
    int? Gender,
    int? MaritalStatus,
    string? FatherName,
    string? MotherName,
    string? Nationality,
    string? Note
);

public record PersonUpdateDto(
    string FirstName,
    string LastName,
    string? IdentityNumber,
    DateOnly? BirthDate,
    string? BirthPlace,
    int? Gender,
    int? MaritalStatus,
    string? FatherName,
    string? MotherName,
    string? Nationality,
    string? Note
);

public record PersonDto(
    string Id,
    string FirstName,
    string LastName,
    string? IdentityNumber,
    DateOnly? BirthDate,
    string? BirthPlace,
    int? Gender,
    int? MaritalStatus,
    string? FatherName,
    string? MotherName,
    string? Nationality,
    string? Note,
    DateTime CreatedAt
);
