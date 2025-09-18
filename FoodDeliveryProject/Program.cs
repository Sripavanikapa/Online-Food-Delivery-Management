
using Domain.Data;
using FoodDeliveryProject.Repositories;
using Infrastructure.Interfaces;
using Infrastructure.JWT;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using Domain.Models;
using Microsoft.Extensions.DependencyInjection;

namespace FoodDeliveryProject
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var orsApiKey = builder.Configuration["OpenRouteService:ApiKey"];
            // Add services to the container.
            builder.Services.AddDbContext<AppDbContext>(options =>
      options.UseSqlServer(
          builder.Configuration.GetConnectionString("DefaultConnection"),
          b => b.MigrationsAssembly("Domain")
      ));



            builder.Services.AddSingleton(new OpenRouteServiceConfig
            {
                ApiKey = orsApiKey
            });

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddScoped<IUserRepository,UserServices>();
            builder.Services.AddScoped<IFoodItems, FoodItemServices>();
            builder.Services.AddScoped<IAddress,AddressServices>();
            builder.Services.AddScoped<IAdmin,AdminImplementation>();
            builder.Services.AddScoped<IRestaurant,RestaurantImplementation>();
            builder.Services.AddScoped<IUserRepository, UserServices>();
            builder.Services.AddScoped<IDeliveryAgent, DeliveryAgentService>();
            builder.Services.AddScoped<IDelivery, DeliveryServices>();
            builder.Services.AddScoped<IReview, ReviewService>();
            builder.Services.AddScoped<IOrder, OrderService>();
            builder.Services.AddScoped<IOrderItem, OrderItemService>();
            builder.Services.AddScoped<ISmsService,SmsService>();
            builder.Services.AddScoped<TokenGeneration>();
            builder.Services.AddScoped<CategoryService>();
            builder.Services.AddHttpClient<OpenRouteServiceClient>();
            builder.Services.Configure<TwilioSettings>(builder.Configuration.GetSection("Twilio"));
            builder.Services.AddScoped<IDeliveryAssignment, DeliveryAssignmentService>();
            builder.Services.AddHttpClient<OpenRouteServiceClient>();
            builder.Services.Configure<OpenRouteServiceConfig>(builder.Configuration.GetSection("OpenRouteService"));
            builder.Services.AddSingleton(sp =>
                                     new OpenRouteServiceClient(
                            sp.GetRequiredService<IOptions<OpenRouteServiceConfig>>(),
                             sp.GetRequiredService<HttpClient>()
                                ));

            builder.Services.Configure<OpenRouteServiceConfig>(
               builder.Configuration.GetSection("OpenRouteService"));


            var key = builder.Configuration.GetValue<string>("ApiSettings:Secret");
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;

            })
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.SaveToken = true;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(builder.Configuration["JwtConfig:Key"])),
                        ValidateIssuer = true,
                        ValidateAudience = false,
                        ValidIssuer = builder.Configuration["JwtConfig:Issuer"],
                        ValidAudience = builder.Configuration["JwtConfig:Audience"],
                        ValidateLifetime = true,
                    };
                });
            builder.Services.AddSwaggerGen(
                c =>
                {
                    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                    {
                        Description = "JWT Authorization header using the Bearer scheme (Example: 'Bearer 12345abcdef')",
                        Name = "Authorization",
                        In = ParameterLocation.Header,
                        Type = SecuritySchemeType.ApiKey,
                        Scheme = "Bearer"
                    });

                    c.AddSecurityRequirement(new OpenApiSecurityRequirement
                            {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "bearer",
                            Name = "Authorization",
                            In = ParameterLocation.Header
                        },
                        new List<string>()
                    }
                            });
                });
            builder.Services.AddAuthorization();



            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
