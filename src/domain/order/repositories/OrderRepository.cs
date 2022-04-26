using System.Net;

using src.domain.order.interfaces;
using src.domain.order.entities;

namespace src.domain.order.repositories;

public class OrderRepository : IOrderRepository
{
  private readonly IOrderDao _dao;

  public OrderRepository(IOrderDao dao)
  {
    this._dao = dao;
  }

  public async Task<OrderEntity> Create(OrderEntity order)
  {
    try
    {
      return await this._dao.InsertOneAsync(order);
    }
    catch (Exception err)
    {
      Console.WriteLine($"Fail to create order: {err.Message}");
      throw new ResponseError(HttpStatusCode.InternalServerError, err.Message);
    }

  }

  public async Task<OrderEntity> FindOne(Guid id)
  {
    try
    {
      return await this._dao.FindAsync(id.ToString());
    }
    catch (Exception err)
    {
      Console.WriteLine($"Fail to find order: {err.Message}");
      throw new ResponseError(HttpStatusCode.InternalServerError, err.Message);
    }
  }

  public async Task<bool> UpdateOne(OrderEntity order)
  {
    try
    {
      var result = await this._dao.ReplaceOneAsync(order.Id.ToString(), order);
      return result != null;
    }
    catch (Exception err)
    {
      Console.WriteLine($"Fail to update order: {err.Message}");
      throw new ResponseError(HttpStatusCode.InternalServerError, err.Message);
    }
  }
}
