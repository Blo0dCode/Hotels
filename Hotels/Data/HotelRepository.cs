namespace Hotels.Data;

public class HotelRepository : IHotelRepository
{
    private readonly HotelDbContext _hotelDb;

    public HotelRepository(HotelDbContext hotelDb)
    {
        _hotelDb = hotelDb;
    }

    public Task<List<HotelDto>> GetHotelsAsync() => _hotelDb.Hotels.ToListAsync();

    public Task<List<HotelDto>> GetHotelsAsync(string name) =>
        _hotelDb.Hotels.Where(h => h.Name.Contains(name)).ToListAsync();

    public async Task<HotelDto?> GetHotelAsync(int hotelId) =>
        await _hotelDb.Hotels.FindAsync(hotelId);

    public async Task InsertHotelAsync(HotelDto hotelDto) =>
        await _hotelDb.Hotels.AddAsync(hotelDto);

    public async Task UpdateHotelAsync(HotelDto hotelDto)
    {
        var hotelFromDb = await _hotelDb.Hotels.FindAsync(hotelDto.Id);
        if (hotelFromDb == null)
        {
            return;
        }

        hotelFromDb.Latitude = hotelDto.Latitude;
        hotelFromDb.Longitude = hotelDto.Longitude;
        hotelFromDb.Name = hotelDto.Name;
    }

    public async Task DeleteHotelAsync(int hotelId)
    {
        var hotelFromDb = await _hotelDb.Hotels.FindAsync(hotelId);
        if (hotelFromDb == null)
        {
            return;
        }

        _hotelDb.Hotels.Remove(hotelFromDb);
    }

    public async Task SaveAsync() =>
        await _hotelDb.SaveChangesAsync();


    private bool _disposed = false;

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                _hotelDb.Dispose();
            }
        }

        _disposed = true;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}