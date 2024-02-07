using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Puppy_Project;
using Puppy_Project.Dbcontext;
using Puppy_Project.Interfaces;
using Puppy_Project.Models;
using Puppy_Project.Services.Cart;
using Puppy_Project.Services.Category;
using Puppy_Project.Services.Order;
using Puppy_Project.Services.Products;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAutoMapper(typeof(AutoMaper));
builder.Services.AddSingleton<PuppyDb>();
builder.Services.AddSingleton<IUsers,Users>();
builder.Services.AddSingleton<ICategory,Category>();
builder.Services.AddSingleton<IProducts,Products>();
builder.Services.AddSingleton<ICart,Cart>();
builder.Services.AddSingleton<IOrder,Order>();

builder.Services.AddControllers();



//-------------------------Authorization & Token------------------------------
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(o =>
{
    o.TokenValidationParameters = new TokenValidationParameters
    {
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey
        (Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = false,
        ValidateIssuerSigningKey = true
    };
});
builder.Services.AddAuthorization();
//---------------------------------------------------------------------------



//--------------------React------------------------------
builder.Services.AddCors(options =>
{
    options.AddPolicy("ReactPolicy", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});
//-------------------------------------------------------




var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//------------------------------Authorization-----------------------------------------
app.UseAuthentication();
app.UseAuthorization();
//------------------------------------------------------------------------------------


//---------React----------------------------------
app.UseCors("ReactPolicy");
//-----------------------------------------------

app.MapControllers();

app.Run();
