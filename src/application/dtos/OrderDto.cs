using src.domain.order.entities;
using System.ComponentModel.DataAnnotations;

namespace src.application.dtos;

public class OrderDto
{
  public string Id { get; private set; }
  public string Status { get; private set; }
  [Required] public string PaymentTypeId { get; set; }
  [Required] public string Code { get; set; }
  [Required] public DeliveryDto Delivery { get; set; }
  [Required] public ItemsDto Items { get; set; }

  public OrderEntity ToDomain(string userId) => new OrderEntity(userId,
    Guid.Parse(this.PaymentTypeId), this.Code, this.Delivery.ToDomain(), this.Items.ToDomain()
  );

  public static OrderDto FromDomain(OrderEntity entity) => new OrderDto
  {
    Id = entity.Id.ToString(),
    Status = entity.Status.ToString(),
    PaymentTypeId = entity.PaymentTypeId.ToString(),
    Code = entity.Code,
    Delivery = DeliveryDto.FromDomain(entity.Delivery),
    Items = ItemsDto.FromDomain(entity.Items)
  };
}

public class DeliveryDto
{
  public OrderDeliveryEntity ToDomain() => new OrderDeliveryEntity(
    this.Address, this.City, this.State, this.ZipCode, this.Country
  );

  public static DeliveryDto FromDomain(OrderDeliveryEntity entity) => new DeliveryDto
  {
    Address = entity.Address,
    City = entity.City,
    State = entity.State,
    ZipCode = entity.ZipCode,
    Country = entity.Country
  };

  public string Address { get; set; }
  public string City { get; set; }
  public string State { get; set; }
  public string ZipCode { get; set; }
  public string Country { get; set; }
}

public class ItemDto
{
  public OrderItemEntity ToDomain() => new OrderItemEntity(
    this.Product, this.Quantity, this.Price
  );

  public static ItemDto FromDomain(OrderItemEntity entity) => new ItemDto
  {
    Product = entity.Product,
    Quantity = entity.Quantity,
    Price = entity.Price
  };

  public string Product { get; set; }
  public int Quantity { get; set; }
  public double Price { get; set; }
}

public class ItemsDto : List<ItemDto>
{
  public List<OrderItemEntity> ToDomain() => this.Select(i => i.ToDomain()).ToList();

  public static ItemsDto FromDomain(List<OrderItemEntity> entities)
  {
    var items = new ItemsDto();
    foreach (var entity in entities)
    {
      items.Add(ItemDto.FromDomain(entity));
    }
    return items;
  }
}
