using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Backend.Data; // Replace 'YourNamespace' with the actual namespace where YourDbContext is located



var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddDbContext<YourDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add CORS policy to allow frontend access
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        builder =>
        {
            builder.WithOrigins("http://localhost:3000") // Add your frontend URL
                   .AllowAnyHeader()
                   .AllowAnyMethod();
        });
});

// Configure JWT authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = "dev-nb826bcawvcdo07n.us.auth0.com"; 
        options.Audience = "https://$dev-nb826bcawvcdo07n.us.auth0.com/api/v2/"; 
    });
builder.Services.AddAuthorization();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseCors("AllowFrontend"); // Enable the CORS policy here
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();



