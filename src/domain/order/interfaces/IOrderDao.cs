using src.domain.order.entities;

namespace src.domain.order.interfaces;

public interface IOrderDao
{
  Task<OrderEntity> InsertOneAsync(OrderEntity entity);
  Task<OrderEntity> FindAsync(string id);
  Task<OrderEntity> ReplaceOneAsync(string id, OrderEntity entity);
}
