namespace Hotels.Data;

public interface IHotelRepository : IDisposable
{
    Task<List<HotelDto>> GetHotelsAsync();
    Task<List<HotelDto>> GetHotelsAsync(string name);
    Task<List<HotelDto>> GetHotelsAsync(Coordinate coordinate);
    Task<HotelDto?> GetHotelAsync(int hotelId);
    Task InsertHotelAsync(HotelDto hotelDto);
    Task UpdateHotelAsync(HotelDto hotelDto);
    Task DeleteHotelAsync(int hotelId);
    Task SaveAsync();
}