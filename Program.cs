using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using OrderApi.Models;
using OrderApi.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System;

var builder = WebApplication.CreateBuilder(args);

// Configura Kestrel usando la configuración del archivo appsettings.json
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(5040); // HTTP
    options.ListenAnyIP(6001, listenOptions => // HTTPS
    {
        listenOptions.UseHttps();
    });
});

// Agrega el contexto de la base de datos
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Agrega servicios
builder.Services.AddHttpClient<IWhatsAppService, WhatsAppService>(client =>
{
    client.BaseAddress = new Uri("https://your-whatsapp-api-url.com");
    // Configura otros encabezados necesarios si es necesario
});

builder.Services.AddScoped<MessageService>(); // Asegúrate de que MessageService esté disponible

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Order API", Version = "v1" });
});

// Agregar servicios de autorización
builder.Services.AddAuthorization();

var app = builder.Build();

// Configura el pipeline de solicitud HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Order API v1");
    });
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // app.UseHsts(); // Solo en producción
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthorization(); // Asegúrate de usar autorización después de la autenticación

app.MapControllers();

app.Run();
