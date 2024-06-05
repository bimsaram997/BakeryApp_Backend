
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Models.Data;
using Models.ViewModels;
using Models.ViewModels.Product;
using Models.ViewModels.FoodType;
using Models.ViewModels.RawMaterial;
using Models.ViewModels.Recipe;
using Repositories;
using Repositories.ProductRepository;
using Repositories.RawMarerialRepository;
using Repositories.RecipeRepository;
using Repositories.UserRepository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Repositories.AddressRepository;
using Models.ViewModels.Address;
using Models.Helpers;
using Models.ViewModels.User;
using Repositories.EnumTypeRepository;
using Models.ViewModels.MasterData;
using Repositories.MasterDataRepository;
using Repositories.RolesRepository;

var builder = WebApplication.CreateBuilder(args);

var jwtIssuer = builder.Configuration.GetSection("Jwt:Issuer").Get<string>();
var jwtKey = builder.Configuration.GetSection("Jwt:Key").Get<string>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
 .AddJwtBearer(options =>
 {
     options.TokenValidationParameters = new TokenValidationParameters
     {
         ValidateIssuer = true,
         ValidateAudience = true,
         ValidateLifetime = true,
         ValidateIssuerSigningKey = true,
         ValidIssuer = jwtIssuer,
         ValidAudience = jwtIssuer,
         IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
     };
 });

// Add services to the container.

builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(
builder.Configuration.GetConnectionString("DefaultConnectionString")));
builder.Services.AddTransient<IRepositoryBase<ProductVM>, ProductRepository>();
builder.Services.AddTransient<IRepositoryBase<RawMaterialVM>, RawMaterialRepository>();
builder.Services.AddTransient<IRepositoryBase<RecipeVM>, RecipeRepository>();
builder.Services.AddTransient<IRecipeRepository, RecipeRepository>();
builder.Services.AddTransient<IRawMaterialRepository, RawMaterialRepository>();
builder.Services.AddTransient<IProductRepository, ProductRepository>();
builder.Services.AddTransient<UserRepository>();
builder.Services.AddTransient<IUserRepository, UserRepository>();
builder.Services.AddTransient<IRepositoryBase<AddressVM>, AddressRepository>();
builder.Services.AddTransient<EnumTypeRepository>();
builder.Services.AddTransient<IEnumTypeRepository, EnumTypeRepository>();
builder.Services.AddTransient<IRepositoryBase<MasterDataVM>, MasterDataRepository>();
builder.Services.AddTransient<IMasterDataRepository, MasterDataRepository>();
builder.Services.AddTransient<IRolesRepository, RolesRepository>();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = null;
    });
builder.Services.AddCors(p => p.AddPolicy("corsapp", builder =>
{
    builder.WithOrigins("*").AllowAnyMethod().AllowAnyHeader();
}));



var app = builder.Build();

app.UseCors(options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

//exception handling
//app.ConfigureBuildInExceptionHandler();
//app.ConfigurCustomExceptionHandler();

app.MapControllers();
//AppDbInitializer.Seed(app);

app.Run();
