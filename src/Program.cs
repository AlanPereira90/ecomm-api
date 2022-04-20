using src.domain.payment_type.interfaces;
using src.domain.payment_type.repositories;
using src.domain.payment_type.services;
using src.domain.payment_type.daos;
using src.domain.infra;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers((option) =>
{
  option.Filters.Add(new ResponseExceptionFilter());
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<MongoDbConnection>(
    builder.Configuration.GetSection("MongoDB"));

builder.Services.AddScoped<IPaymentTypeDao, PaymentTypeDao>();
builder.Services.AddScoped<IPaymentTypeRepository, PaymentTypeRepository>();
builder.Services.AddScoped<IPaymentTypeService, PaymentTypeService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program { }
