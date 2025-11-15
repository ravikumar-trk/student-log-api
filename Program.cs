using Microsoft.OpenApi.Models;
using student_log_api.Common;
using student_log_api.Interface;
using student_log_api.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCors(options =>
{

    options.AddDefaultPolicy(
        policy =>
        {
            policy.WithOrigins("http://localhost:5173")
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});
builder.Services.AddControllers().AddJsonOptions(options =>
{
    // Configure System.Text.Json to use PascalCase
    options.JsonSerializerOptions.PropertyNamingPolicy = null;
});
builder.Services.AddOptions();
builder.Services.Configure<AppSettings>(
    builder.Configuration.GetSection("AppSettings")
);
builder.Services.AddSingleton<IAccountInterface, AccountServices>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Student Log API", Version = "v1" });
});

var app = builder.Build();
app.UseCors();
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
