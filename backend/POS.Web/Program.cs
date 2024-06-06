using POS.Infrastructure;
using POS.Web.Users;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder
    .Services
    .AddSqlServer<POSDbContext>(builder.Configuration.GetConnectionString("POSConnectionString"));

builder.SetupAuthorization();
builder.SetupAuthentication();

builder
    .Services
    .AddCors(options =>
    {
        options.AddPolicy(
            name: "POSLocalFrontEnd",
            policy =>
            {
                policy
                    .WithOrigins(
                        "http://localhost:5076",
                        "http://localhost:5008",
                        "http://127.0.0.1:5008"
                    )
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
            }
        );
    });

builder.Services.RegisterInfrastructureServices();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

app.UseCors("POSLocalFrontEnd");
app.UseAuthentication();
app.UseAuthorization();

app.AddUserEndpoints();

app.Run();
