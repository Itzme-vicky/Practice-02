using Microsoft.EntityFrameworkCore;
using Serilog;
using World.Api.Common;
using World.Api.Data;
using World.Api.Repository;
using World.Api.Repository.IRepository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


// Add CORS Configuration
#region CORS Configration


builder.Services.AddCors(options =>
{
    options.AddPolicy("CustomPolicy", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});


#endregion

#region Database Configuration


var Connectionstring = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(Connectionstring));


#endregion

#region Auto Mapper Configuration

builder.Services.AddAutoMapper(typeof(MappingProfile));


#endregion

#region Repository Service
builder.Services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddTransient<ICountryRepository, CountryRepository>();
builder.Services.AddTransient<IStateRepository, StateRepository>();


#endregion

#region Serilog Configration
builder.Host.UseSerilog((context, config) => {
    config.WriteTo.File("Log/log.txt", rollingInterval: RollingInterval.Day);
    if (context.HostingEnvironment.IsProduction() == false)
    {
        config.WriteTo.Console();
    }
});
#endregion

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    

    //app.UseSwaggerUI(options =>
    //{
    //    options.SwaggerEndpoint("/swagger/v1/swagger.json", "World API V1");
    //    options.RoutePrefix = string.Empty; // Set Swagger UI at the app's root
    //});
}


#region Configure CORS Pipeline
// Add the CORS middleware to the request pipeline
app.UseCors("CustomPolicy");
#endregion
app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
