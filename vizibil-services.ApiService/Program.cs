using vizibil_api.AutoMapper;
using vizibil_api.Interfaces.Repositories;
using vizibil_api.Interfaces.Services;
using vizibil_api.Repositories;
using vizibil_api.Services.Database;
using Microsoft.EntityFrameworkCore;
using vizibil_api.Models;
using vizibil_services.ApiService.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddCors(opt =>
{
    opt.AddPolicy("ClientPolicy", b =>
    {
        var clientUrls = builder.Configuration.GetSection("ClientUrl").Value;
        var urls = clientUrls?.Split(",");
        b.WithOrigins(urls ?? [])
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

builder.AddNpgsqlDbContext<DatabaseContext>("vizibil");

builder.AddRabbitMQClient(connectionName: "messaging");
        
builder.Services.AddControllers();
        
builder.Services.AddAutoMapper(typeof(AutoMapperConfig));
        
// Add services to the container.
builder.Services.AddAuthorization();
        
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
        
ConfigureServices(builder);

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
    dbContext.InitializeDatabase(); 
    dbContext.Database.Migrate();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
        
app.UseHttpsRedirection();
        
app.UseAuthorization();

app.UseCors("ClientPolicy");

app.MapControllers();
        
app.Run();

return;

void ConfigureServices(WebApplicationBuilder tempBuilder)
{
    // Services
    tempBuilder.Services.AddTransient<IContactService, ContactService>();
    tempBuilder.Services.AddTransient<IRabbitMqProducer, RabbitMqProducer>();
    
    // Repositories
    tempBuilder.Services.AddTransient<IContactRepository, ContactRepository>();
}