using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.Features;

namespace HDSS_BACKEND.Services
{
   public static class ServiceExtensions
    {
public static void ConfigureCors(this IServiceCollection services) => services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", builder => builder
        .WithOrigins("http://10.42.0.1:3000", "http://192.168.51.18:3000", "http://192.168.43.18:5000", "http://localhost:3000", "http://192.168.189.18:3000"
        , "http://192.168.19.18:3000"
        ) // Add the origins you want to allow
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials()
        
    );
});

public static void ConfigureIISIntegration(this IServiceCollection services) => services.Configure<IISOptions>(options =>
{
});





}
    
}