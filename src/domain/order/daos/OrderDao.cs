using System;

using MongoDB.Driver;
using Microsoft.Extensions.Options;

using src.domain.order.interfaces;
using src.domain.order.models;
using src.domain.order.entities;
using src.domain.infra;

namespace src.domain.order.daos;

public class OrderDao : IOrderDao
{
  private readonly string _collectionName = "orders";
  private readonly IMongoCollection<Order> _collection;

  public OrderDao(IOptions<MongoDbConnection> connection)
  {
    var mongoClient = new MongoClient(
            connection.Value.ConnectionString);

    var mongoDatabase = mongoClient.GetDatabase(
        connection.Value.DatabaseName);

    _collection = mongoDatabase.GetCollection<Order>(_collectionName);
  }

  public async Task<OrderEntity> InsertOneAsync(OrderEntity entity)
  {
    await _collection.InsertOneAsync(Order.FromEntity(entity));
    return entity;
  }

  public async Task<OrderEntity> FindAsync(string id)
  {
    var Order = await _collection.Find(x => x.Id == id).FirstOrDefaultAsync();
    return Order != null ? Order.ToEntity() : null;
  }

  public async Task<OrderEntity> ReplaceOneAsync(string id, OrderEntity entity)
  {
    var updated = await _collection.ReplaceOneAsync(x => x.Id == id, Order.FromEntity(entity));

    return updated.ModifiedCount > 0 ? entity : null;
  }
}
