using projekt.Serivces;
using projekt.Db.BankContext;
using projekt.Db.Repository;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;

String connectionString = config.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<BankDbContext>(options => options.UseSqlite(connectionString));
// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IBankService, BankService>();
builder.Services.AddScoped<IActivityService, ActivityService>();

builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<ITransferRepository, TransferRepository>();
builder.Services.AddScoped<IActivityRepository, ActivityRepository>();
builder.Services.AddScoped<IVerificationRepository, VerificationRepository>();

builder.Services.AddScoped<IDebugSerivce, DebugService>();

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
