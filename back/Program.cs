using projekt.Services;
using projekt.Services.Interfaces;
using projekt.Db.BankContext;
using projekt.Db.Repository;
using projekt.Db.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;
if (config == null) throw new Exception("Configuration not found");

var connectionString = config.GetConnectionString("DefaultConnection");
if (connectionString == null) throw new Exception("DefaultConnection not found in appsettings.json");
builder.Services.AddDbContext<BankDbContext>(options => options.UseSqlite(connectionString));
// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IBankService, BankService>();
builder.Services.AddScoped<IActivityService, ActivityService>();
builder.Services.AddScoped<IAccessService, AccessService>();
builder.Services.AddScoped<ICryptoService, CryptoService>();
builder.Services.AddScoped<IDebugSerivce, DebugService>();

builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<ITransferRepository, TransferRepository>();
builder.Services.AddScoped<IActivityRepository, ActivityRepository>();
builder.Services.AddScoped<IVerificationRepository, VerificationRepository>();
builder.Services.AddScoped<ITimeOutRepository, TimeOutRepository>();

builder.Services.AddSingleton<IConfiguration>(config);

var myPolicy = "MyCorsePolicy";
builder.Services.AddCors(options =>
{
    options.AddPolicy(myPolicy, builder =>
    builder.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
} else {
    app.UseCors(myPolicy);
    app.UseSwagger();
    app.UseSwaggerUI();
}

var webSocketOptions = new WebSocketOptions
{
    KeepAliveInterval = TimeSpan.FromMinutes(2)
};

app.UseWebSockets(webSocketOptions);
app.UseHttpsRedirection();
app.MapControllers();

app.Run();
