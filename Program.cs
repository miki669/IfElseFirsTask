using System.Text;
using IfElseFirsTask.Context;
using IfElseFirsTask.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Snake.API", Version = "v1" });
});


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpLogging(options => // <--- Setup logging
{
    // Specify all that you need here:
    options.LoggingFields = HttpLoggingFields.RequestHeaders |
                            HttpLoggingFields.RequestBody |
                            HttpLoggingFields.ResponseHeaders |
                            HttpLoggingFields.ResponseBody;
});

    

 builder.Services.AddDbContext<PrimaryDataBaseContext>(options =>
 {
     options.UseNpgsql(builder.Configuration.GetSection("DefaultConnection").Value)
         .EnableSensitiveDataLogging()
         .EnableDetailedErrors();

 },ServiceLifetime.Singleton);
builder.Services.AddSingleton<UserService>();


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