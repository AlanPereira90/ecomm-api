using MongoDB.Driver;
using Microsoft.Extensions.Options;

using src.domain.payment_type.interfaces;
using src.domain.payment_type.models;
using src.domain.payment_type.entities;
using src.domain.infra;

namespace src.domain.payment_type.daos;

public class PaymentTypeDao : IPaymentTypeDao
{
  private readonly string _collectionName = "payment_types";
  private readonly IMongoCollection<PaymentType> _collection;

  public PaymentTypeDao(IOptions<MongoDbConnection> connection)
  {
    var mongoClient = new MongoClient(
            connection.Value.ConnectionString);

    var mongoDatabase = mongoClient.GetDatabase(
        connection.Value.DatabaseName);

    _collection = mongoDatabase.GetCollection<PaymentType>(_collectionName);
  }

  public async Task<PaymentTypeEntity> InsertOneAsync(PaymentTypeEntity entity)
  {
    await _collection.InsertOneAsync(PaymentType.FromEntity(entity));
    return entity;
  }
}
