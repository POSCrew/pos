using POS.Infrastructure;
using POS.Web.Users;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSqlServer<POSDbContext>(builder.Configuration.GetConnectionString("POSConnectionString"));

builder.SetupAuthorization();
builder.SetupAuthentication();

builder.Services.RegisterInfrastructureServices();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

//app.MapIdentityApi<IdentityUser>();

app.AddUserEndpoints();

app.Run();
