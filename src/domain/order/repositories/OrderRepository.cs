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

  public Task<OrderEntity> Create(OrderEntity order)
  {
    try
    {
      return this._dao.InsertOneAsync(order);
    }
    catch (Exception err)
    {
      Console.WriteLine($"Fail to create order: {err.Message}");
      throw new ResponseError(HttpStatusCode.InternalServerError, err.Message);
    }

  }
}
