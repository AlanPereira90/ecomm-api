using src.domain.order.entities;

namespace src.domain.order.interfaces;

public interface IOrderService
{
  Task<Guid> Create(OrderEntity order);
  Task<OrderEntity> FindOne(Guid id, string userId);
  Task<bool> Cancel(Guid id, string userId);
  Task<bool> Confirm(Guid id, string userId);
  Task<bool> Finish(Guid id, string userId);
}
