using Hotels.Apis;
using Hotels.Auth;
using Hotels.Data;
using Hotels.Options;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

RegisterServices(builder.Services);

var app = builder.Build();

var apis = app.Services.GetServices<IApi>();
foreach (var api in apis)
{
    if (api is null) throw new InvalidProgramException("Api not found");
    api.Register(app);
}
Configure(app);

app.Run();

void RegisterServices(IServiceCollection services)
{
    var serviceProvider = builder.Services.BuildServiceProvider();
    services.AddEndpointsApiExplorer();
    services.AddSwaggerGen();
    services.AddDbContext<HotelDbContext>(options =>
    {
        options.UseSqlite(builder.Configuration.GetConnectionString("Sqlite"));
    });

    services.Configure<JwtOptions>(builder.Configuration.GetSection(nameof(JwtOptions)));
    
    services.AddScoped<IHotelRepository, HotelRepository>();
    services.AddSingleton<ITokenService, TokenService>();
    services.AddSingleton<IUserRepository, UserRepository>();
    services.AddAuthorization();
    services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            var keyJwtOptions = serviceProvider.GetRequiredService<IOptions<JwtOptions>>().Value;
            options.TokenValidationParameters = new()
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = keyJwtOptions.Issuer,
                ValidAudience = keyJwtOptions.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(keyJwtOptions.Key))
            };
        });
    services.AddTransient<IApi, HotelApi>();
    services.AddTransient<IApi, AuthApi>();
}

void Configure(WebApplication app)
{
    app.UseAuthentication();
    app.UseAuthorization();
    
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
        using var scope = app.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<HotelDbContext>();
        db.Database.EnsureCreated();
    }
    app.UseHttpsRedirection();
}