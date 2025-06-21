namespace EastSeat.ResourceIdea.Domain.Clients.ValueObjects;

/// <summary>
/// Representation of an address.
/// </summary>
public class Address
{
    private readonly string _building;
    private readonly string _street;
    private readonly string _city;

    /// <summary>
    /// Address building.
    /// </summary>
    public string Building => _building;

    /// <summary>
    /// Address street.
    /// </summary>
    public string Street => _street;

    /// <summary>
    /// Address city.
    /// </summary>
    public string City => _city;

    /// <summary>
    /// Empty address.
    /// </summary>
    public static Address Empty => new(building: string.Empty, street: string.Empty, city: string.Empty);

    private Address(string building, string street, string city)
    {
        _building = building;
        _street = street;
        _city = city;
    }

    /// <summary>
    /// Create an instance of <see cref="Address"/>.
    /// </summary>
    /// <param name="building">Name of the building.</param>
    /// <param name="street">Street.</param>
    /// <param name="city">City.</param>
    /// <returns><see cref="Address"/> created.</returns>
    public static Address Create(string building, string street, string city)
    {
        ArgumentException.ThrowIfNullOrEmpty(street);
        ArgumentException.ThrowIfNullOrEmpty(city);

        return new Address(building, street, city);
    }

    /// <inheritdoc />
    public override string ToString() =>
        string.IsNullOrEmpty(_building)
        ? $"{_street}, {_city}"
        : $"{_building}, {_street}, {_city}";

    /// <summary>Address is not empty.</summary>
    public bool IsNotEmpty() => this != Address.Empty;
}
