using Hotels.Data;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<HotelDb>(options =>
{
    options.UseSqlite(builder.Configuration.GetConnectionString("Sqlite"));
});
builder.Services.AddScoped<IHotelRepository, HotelRepository>();
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<HotelDb>();
    db.Database.EnsureCreated();
}

app.MapGet("/hotels", async (IHotelRepository repository) =>
    Results.Ok(await repository.GetHotelsAsync()));

app.MapGet("/hotels/{id}", async (int id, IHotelRepository repository) => 
    Results.Ok(await repository.GetHotelAsync(id)) is Hotel hotel
    ? Results.Ok(hotel)
    : Results.NotFound());

app.MapPost("/hotels", async (Hotel? hotel, IHotelRepository repository) =>
{
    await repository.InsertHotelAsync(hotel);
    await repository.SaveAsync();
    return Results.Created($"/hotels/{hotel.Id}", hotel);
});

app.MapPut("/hotels", async (Hotel hotel, IHotelRepository repository) =>
{
    await repository.UpdateHotelAsync(hotel);
    await repository.SaveAsync();
});
app.MapDelete("/hotels/{id}", async (int id, IHotelRepository repository) =>
{
    await repository.DeleteHotelAsync(id);
    await repository.SaveAsync();
});

app.UseHttpsRedirection();

app.Run();