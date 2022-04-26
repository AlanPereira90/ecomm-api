using src.domain.order.entities;

namespace src.domain.order.interfaces;

public interface IOrderRepository
{
  Task<OrderEntity> Create(OrderEntity order);
  Task<OrderEntity> FindOne(Guid id);
  Task<bool> UpdateOne(OrderEntity order);
}
