using System.Security.Cryptography;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

RSAParameters _rsaParameters;
using (RSACryptoServiceProvider provider = new RSACryptoServiceProvider())
{
    _rsaParameters = provider.ExportParameters(true);
}

// make the RSAParameters available throughout the application
builder.Services.AddSingleton(_rsaParameters.GetType(), _rsaParameters);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

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
