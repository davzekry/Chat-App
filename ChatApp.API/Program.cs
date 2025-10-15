using Chat_Application.Configurations;
using Chat_Application.Hubs;
using EGRideAPI.API.Configuration;

namespace ChatAppAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Configurations
            builder.Services.ConfigureIdentity(builder.Configuration);    // Identity Configuration
            builder.Services.ConfigureJwtToken(builder.Configuration);    // JWT Configuration
            builder.Services.ExternalConfiguration(builder.Configuration); // SignalR, MediatR and Swagger, and CORS Configurations

            // Add services to the container.
            builder.Services.AddControllers();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            //if (app.Environment.IsDevelopment()) // Re-evaluate if you want Swagger only in dev or always
            //{
            //}
            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseHttpsRedirection();

            // Add UseRouting here. This is important for CORS to work correctly.
            // It might be implicitly handled by MapControllers/MapHub, but explicit is safer.
            app.UseRouting();

            // Apply the CORS policy AFTER UseRouting and BEFORE UseAuthentication/UseAuthorization
            app.UseCors("AllowSpecificOrigin"); // Use the new policy name

            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllers();

            //SignalR
            app.MapHub<Chathub>("/Chathub");

            app.Run();
        }
    }
}