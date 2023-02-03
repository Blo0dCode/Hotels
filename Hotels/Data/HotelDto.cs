namespace Hotels.Data;

public class HotelDto
{
    public int Id { get; set; }
    [Required] public string Name { get; set; } = null!;
    public double Latitude { get; set; }
    public double Longitude { get; set; }
}