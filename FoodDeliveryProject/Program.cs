
using Domain.Data;
using Infrastructure.Repositories;
using Infrastructure.Interfaces;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using FoodDeliveryProject.Repositories;

namespace FoodDeliveryProject
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddDbContext<AppDbContext>(options =>
      options.UseSqlServer(
          builder.Configuration.GetConnectionString("DefaultConnection"),
          b => b.MigrationsAssembly("Domain")
      ));





            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddScoped<UserServices>();
            builder.Services.AddScoped<AddressServices>();
            builder.Services.AddScoped<IAdmin,AdminImplementation>();
            builder.Services.AddScoped<IRestaurant,RestaurantImplementation>();
            builder.Services.AddScoped<IUserRepository, UserServices>();
            builder.Services.AddScoped<IDeliveryAgent, DeliveryAgentService>();
            builder.Services.AddScoped<IDelivery, DeliveryServices>();

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
