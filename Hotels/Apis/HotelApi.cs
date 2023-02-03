using Hotels.Data;

namespace Hotels.Apis;

public class HotelApi : IApi
{
    public void Register(WebApplication app)
    {
        app.MapGet("/hotels", Get)
            .Produces<List<HotelDto>>()
            .WithName("GetAllHotels")
            .WithTags("Getters");

        app.MapGet("/hotels/search/name/{query}", SearchByName)
            .Produces<List<HotelDto>>()
            .Produces(StatusCodes.Status404NotFound)
            .WithName("SearchHotels")
            .WithTags("Getters")
            .ExcludeFromDescription();

        app.MapGet("/hotels/search/location/{coordinate}", SearchByCoordinate)
            .ExcludeFromDescription();

        app.MapGet("/hotels/{id}", GetById)
            .Produces<HotelDto>()
            .WithName("GetHotel")
            .WithTags("Getters");

        app.MapPost("/hotels", Post)
            .Accepts<HotelDto>("application/json")
            .Produces<HotelDto>(StatusCodes.Status201Created)
            .WithName("CreateHotel")
            .WithTags("Creators");

        app.MapPut("/hotels", Put)
            .Accepts<HotelDto>("application/json")
            .WithName("UpdateHotel")
            .WithTags("Updaters");

        app.MapDelete("/hotels/{id}", Delete)
            .WithName("DeleteHotel")
            .WithTags("Deleters");
    }

    [Authorize]
    private async Task<IResult> Get(IHotelRepository repository) =>
        Results.Extensions.Xml(await repository.GetHotelsAsync());

    [Authorize]
    private async Task<IResult> GetById(int id, IHotelRepository repository)
    {
        var hotel = await repository.GetHotelAsync(id);
        return hotel is not null
            ? Results.Ok(hotel)
            : Results.NotFound();
    }

    [Authorize]
    private async Task<IResult> SearchByName(string query, IHotelRepository repository) =>
        await repository.GetHotelsAsync(query) is IEnumerable<HotelDto> hotels
            ? Results.Ok(hotels)
            : Results.NotFound(Array.Empty<HotelDto>());

    [Authorize]
    private async Task<IResult> SearchByCoordinate(Coordinate coordinate, IHotelRepository repository) =>
        await repository.GetHotelsAsync(coordinate) is IEnumerable<HotelDto> hotels
            ? Results.Ok(hotels)
            : Results.NotFound(Array.Empty<HotelDto>());

    [Authorize]
    private async Task<IResult> Post(HotelDto hotel, IHotelRepository repository)
    {
        await repository.InsertHotelAsync(hotel);
        await repository.SaveAsync();
        return Results.Created($"/hotels/{hotel.Id}", hotel);
    }

    [Authorize]
    private async Task<IResult> Put(HotelDto hotel, IHotelRepository repository)
    {
        await repository.UpdateHotelAsync(hotel);
        await repository.SaveAsync();
        return Results.NoContent();
    }

    [Authorize]
    private async Task<IResult> Delete(int id, IHotelRepository repository)
    {
        await repository.DeleteHotelAsync(id);
        await repository.SaveAsync();
        return Results.NoContent();
    }
}