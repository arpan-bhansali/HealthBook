using HealthBook.Data;
using HealthBook.Repository.AppointmentRepo;
using HealthBook.Repository.UserRepo;
using HealthBook.Utility;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using System.Net;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddJsonOptions(jsonOptions =>
{
    jsonOptions.JsonSerializerOptions.PropertyNamingPolicy = null; //this is for returning property as pascal case
});

builder.Services.AddMvc().AddControllersAsServices();

builder.Services.AddCors(options =>
{
    options.AddPolicy("CORS", corsPolicyBuilder => corsPolicyBuilder.AllowAnyOrigin()

        .WithOrigins("*")
        // Apply CORS policy for any type of origin  
        .AllowAnyMethod()
        // Apply CORS policy for any type of http methods  
        .AllowAnyHeader()
        // Apply CORS policy for any headers  
        .AllowCredentials().WithExposedHeaders("Content-Disposition"));
    // Apply CORS policy for all users  

});

// Adding DB Context

builder.Services.AddDbContext<HealthBookContext>(options =>
        options.UseMySQL("Server=localhost;Database=healthbook;User=root;Password=qwerty123;"));

// Adding Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})

// Adding Jwt Bearer  
.AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateIssuerSigningKey = true,
        ValidAudience = "https://localhost:7020",
        ValidIssuer = "https://localhost:7020",
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("85e7a35f5e023ef8f8035d36efda15cf")),
        ValidateLifetime = true
    };
});

//Register application services
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IAppointmentRepository, AppointmentRepository>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Health Book API",
        Description = "Collection of API for health book application."
    });


    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme."
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
                                }
                            },
                            new string[] {}
                    }
                });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Health Book API V1");
    });
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseCors("CORS");

app.Use(async (context, next) =>
{
    await next();

    if (context.Response.StatusCode == (int)HttpStatusCode.Unauthorized) // 401
    {
        context.Response.ContentType = "application/json";
        // Write to the response in any way you wish
        context.Response.StatusCode = 200; // for unauthorized/invalid token
        BaseResponseModel baseResponseModel = new BaseResponseModel();
        baseResponseModel.StatusCode = 4;
        baseResponseModel.Message = "You are not authorized!";
        context.Response.ContentType = "application/json";
        string response = JsonConvert.SerializeObject(baseResponseModel);
        await context.Response.WriteAsync(response);
    }
});

app.UseAuthentication();

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();
