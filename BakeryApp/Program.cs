
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Models.Data;
using Models.ViewModels;
using Models.ViewModels.FoodItem;
using Models.ViewModels.FoodType;
using Models.ViewModels.RawMaterial;
using Models.ViewModels.Recipe;
using Repositories;
using Repositories.FoodItemRepository;
using Repositories.FoodTypeRepository;
using Repositories.RawMarerialRepository;
using Repositories.RecipeRepository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(
builder.Configuration.GetConnectionString("DefaultConnectionString")
));
builder.Services.AddTransient<IFoodItemRepository, FoodItemRepository>();
builder.Services.AddTransient<IFoodTypeRepository, FoodTypeRepository>();
builder.Services.AddTransient<IRepositoryBase<FoodItemVM>, FoodItemRepository>();
builder.Services.AddTransient<IRepositoryBase<FoodTypeVM>, FoodTypeRepository>();
builder.Services.AddTransient<IRepositoryAllBase<AllFoodItemVM>, AllFoodItemRepository>();
builder.Services.AddTransient<IRepositoryBase<RawMaterialVM>, RawMaterialRepository>();
builder.Services.AddTransient<IRepositoryBase<RecipeVM>, RecipeRepository>();
builder.Services.AddTransient<IRecipeRepository, RecipeRepository>();
builder.Services.AddTransient<IRawMaterialRepository, RawMaterialRepository>();
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

app.UseAuthorization();

//exception handling
//app.ConfigureBuildInExceptionHandler();
//app.ConfigurCustomExceptionHandler();

app.MapControllers();
//AppDbInitializer.Seed(app);

app.Run();
