using Bogus;

using src.domain.payment_type.entities;

namespace test.builders.payment_type;

public static class PaymentTypeEntityBuilder
{
  private static Faker faker = new Faker();
  public static PaymentTypeEntity build() => new PaymentTypeEntity(
    code: faker.Random.AlphaNumeric(10),
    name: faker.Lorem.Word(),
    description: faker.Lorem.Sentence()
  );
}
