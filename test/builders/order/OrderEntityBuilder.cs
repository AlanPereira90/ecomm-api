using System.Collections.Generic;

using Bogus;

using src.domain.order.entities;
using src.domain.order.enums;

namespace test.builders.order;

public static class OrderEntityBuilder
{
  private static Faker faker = new Faker();

  private static List<OrderItemEntity> BuildItemsList()
  {
    return new List<OrderItemEntity> {
      new OrderItemEntity(
        product: faker.Commerce.ProductName(),
        quantity: faker.Random.Number(1, 10),
        price: faker.Random.Double(1, 100)
      ),
      new OrderItemEntity(
        product: faker.Commerce.ProductName(),
        quantity: faker.Random.Number(1, 10),
        price: faker.Random.Double(1, 100)
      )
    };
  }
  public static OrderEntity Build() => new OrderEntity(
    id: faker.Random.Guid(),
    userId: faker.Random.AlphaNumeric(10),
    paymentTypeId: faker.Random.Guid(),
    code: faker.Random.AlphaNumeric(10),
    status: OrderStatus.CREATED,
    delivery: new OrderDeliveryEntity(
      address: faker.Address.StreetAddress(),
      city: faker.Address.City(),
      state: faker.Address.State(),
      zipCode: faker.Address.ZipCode(),
      country: faker.Address.Country()
    ),
    items: BuildItemsList()
  );
}
