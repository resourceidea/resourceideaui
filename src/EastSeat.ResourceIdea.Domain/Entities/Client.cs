using EastSeat.ResourceIdea.Domain.Common;
using EastSeat.ResourceIdea.Domain.Enums;

namespace EastSeat.ResourceIdea.Domain.Entities;

/// <summary>
/// Represents a client of the audit firm
/// </summary>
public class Client : AuditableEntity
{
    private Client() { } // EF Core

    public string Name { get; private set; } = string.Empty;
    public string? RegistrationNumber { get; private set; }
    public string? Sector { get; private set; }
    public string? ContactEmail { get; private set; }
    public string? ContactPhone { get; private set; }
    public bool IsActive { get; private set; } = true;

    // Navigation properties
    public ICollection<Engagement> Engagements { get; private set; } = new List<Engagement>();

    public static Result<Client> Create(string name, string? registrationNumber = null,
        string? sector = null, string? contactEmail = null, string? contactPhone = null)
    {
        try
        {
            Guard.AgainstNullOrEmpty(name, nameof(name));

            var client = new Client
            {
                Name = name.Trim(),
                RegistrationNumber = registrationNumber?.Trim(),
                Sector = sector?.Trim(),
                ContactEmail = contactEmail?.Trim(),
                ContactPhone = contactPhone?.Trim()
            };

            return Result<Client>.Success(client);
        }
        catch (Exception ex)
        {
            return Result<Client>.Failure(ex.Message);
        }
    }

    public void UpdateContactInfo(string? email, string? phone)
    {
        ContactEmail = email?.Trim();
        ContactPhone = phone?.Trim();
        ModifiedAt = DateTime.UtcNow;
    }

    public void Deactivate()
    {
        IsActive = false;
        ModifiedAt = DateTime.UtcNow;
    }

    public void Reactivate()
    {
        IsActive = true;
        ModifiedAt = DateTime.UtcNow;
    }
}
