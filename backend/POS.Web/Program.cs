using POS.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSqlServer<POSDBContext>("POSConnectionString");

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.Map("/", (POSDBContext dbc) => {
    return $"dbc type: {dbc.GetType().Name}";
});

app.Run();
