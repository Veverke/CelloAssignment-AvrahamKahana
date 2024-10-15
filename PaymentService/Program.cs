using Microsoft.EntityFrameworkCore;
using PaymentService;
using PaymentService.DbContexts;
using PaymentService.Settings;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<PaymentContext>();
builder.Services.AddOptions<TariffOverrideParseSettings>()
    .Bind(builder.Configuration.GetSection(TariffOverrideParseSettings.SectionName));
builder.Services.AddSingleton<ParkingPaymentCalculator>();

var app = builder.Build();

// Apply migrations at startup
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<PaymentContext>();
        context.Database.Migrate();  // This will apply any pending migrations
    }
    catch (Exception ex)
    {
        // Log errors or handle them as appropriate
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while migrating the database.");
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

//app.UseAuthorization();

app.MapControllers();

app.Run();
