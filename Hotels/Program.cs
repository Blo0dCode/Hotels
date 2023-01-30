using Hotels;
using Hotels.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<HotelDbContext>(options =>
{
    options.UseSqlite(builder.Configuration.GetConnectionString("Sqlite"));
});
builder.Services.AddScoped<IHotelRepository, HotelRepository>();
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<HotelDbContext>();
    db.Database.EnsureCreated();
}

app.MapGet("/hotels", async (IHotelRepository repository) =>
        Results.Ok(await repository.GetHotelsAsync()))
    .Produces<List<HotelDto>>()
    .WithName("GetAllHotels")
    .WithTags("Getters");

app.MapGet("/hotels/search/name/{query}",
        async (string query, IHotelRepository repository) =>
            await repository.GetHotelsAsync(query) is IEnumerable<HotelDto> hotels
                ? Results.Ok(hotels)
                : Results.NotFound(Array.Empty<HotelDto>()))
    .Produces<List<HotelDto>>()
    .Produces(StatusCodes.Status404NotFound)
    .WithName("SearchHotels")
    .WithTags("Getters")
    .ExcludeFromDescription();

app.MapGet("/hotels/search/location/{coordinate}",
    async (Coordinate coordinate, IHotelRepository repository) =>
        await repository.GetHotelsAsync(coordinate) is IEnumerable<HotelDto> hotels
            ? Results.Ok(hotels)
            : Results.NotFound(Array.Empty<HotelDto>()))
    .ExcludeFromDescription();

app.MapGet("/hotels/{id}", async (int id, IHotelRepository repository) =>
        await repository.GetHotelAsync(id) is HotelDto hotel
            ? Results.Ok(hotel)
            : Results.NotFound())
    .Produces<HotelDto>()
    .WithName("GetHotel")
    .WithTags("Getters");

app.MapPost("/hotels", async (HotelDto? hotel, IHotelRepository repository) =>
    {
        await repository.InsertHotelAsync(hotel);
        await repository.SaveAsync();
        return Results.Created($"/hotels/{hotel.Id}", hotel);
    })
    .Accepts<HotelDto>("application/json")
    .Produces<HotelDto>(StatusCodes.Status201Created)
    .WithName("CreateHotel")
    .WithTags("Creators");

app.MapPut("/hotels", async (HotelDto hotel, IHotelRepository repository) =>
    {
        await repository.UpdateHotelAsync(hotel);
        await repository.SaveAsync();
    })
    .Accepts<HotelDto>("application/json")
    .WithName("UpdateHotel")
    .WithTags("Updaters");

app.MapDelete("/hotels/{id}", async (int id, IHotelRepository repository) =>
    {
        await repository.DeleteHotelAsync(id);
        await repository.SaveAsync();
    })
    .WithName("DeleteHotel")
    .WithTags("Deleters");

app.UseHttpsRedirection();

app.Run();