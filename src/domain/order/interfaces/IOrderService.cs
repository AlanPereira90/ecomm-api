using src.domain.order.entities;

namespace src.domain.order.interfaces;

public interface IOrderService
{
  Task<Guid> Create(OrderEntity order);
}
