using Bogus;

using src.domain.order.entities;
using src.domain.order.enums;

namespace test.builders.order;

public static class OrderEntityBuilder
{
  private static Faker faker = new Faker();
  public static OrderEntity Build() => new OrderEntity(
    id: faker.Random.Guid(),
    userId: faker.Random.AlphaNumeric(10),
    paymentTypeId: faker.Random.Guid(),
    code: faker.Random.AlphaNumeric(10),
    status: OrderStatus.CREATED,
    delivery: new
    {
      address = faker.Address.StreetAddress(),
      city = faker.Address.City(),
      state = faker.Address.State(),
      zipCode = faker.Address.ZipCode(),
      country = faker.Address.Country()
    },
    items: new
    {
      productId = faker.Random.Guid(),
      quantity = faker.Random.Int(1, 10),
      price = faker.Random.Double(1, 100)
    }
  );
}
