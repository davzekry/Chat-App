
using ChatApp.Application;
using ChatApp.Domain.Interfaces;
using ChatApp.Infrastructure.Data;
using ChatApp.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using System;

namespace ChatAppAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Register MediatR
            builder.Services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssembly(typeof(ApplicationAssembly).Assembly));

            // Add SIgnalR
            builder.Services.AddSignalR();

            // Register repositories
            builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));


            // Add DbContext
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("CS")));

            // Add services to the container.
            builder.Services.AddControllers();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
