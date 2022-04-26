using src.domain.order.entities;
using System.ComponentModel.DataAnnotations;

namespace src.application.dtos;

public class OrderDto
{
  public string Id { get; private set; }
  public string Status { get; private set; }
  [Required] public string PaymentTypeId { get; set; }
  [Required] public string Code { get; set; }
  [Required] public Object Delivery { get; set; }
  [Required] public Object Items { get; set; }

  public OrderEntity ToDomain(string userId) => new OrderEntity(userId,
    Guid.Parse(this.PaymentTypeId), this.Code, this.Delivery, this.Items
  );
}
