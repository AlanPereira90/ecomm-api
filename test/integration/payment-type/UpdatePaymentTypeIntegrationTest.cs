using Xunit;
using Moq;
using Bogus;

using System;
using System.Net;
using System.Threading.Tasks;

using src.domain.payment_type.entities;
using test.builders.payment_type;

namespace test.integration;

public class UpdatePaymentTypeIntegrationTest
{
  private TestClient _testClient;
  private Faker _faker;

  public UpdatePaymentTypeIntegrationTest()
  {
    _testClient = new TestClient();
    _faker = new Faker();
  }

  [Fact(DisplayName = "should return 202 ACCEPTED when payment type is sucessfully updated")]
  public async void UpdatePaymentTypeSuccess()
  {
    PaymentTypeEntity paymentType = PaymentTypeEntityBuilder.Build();
    var body = new
    {
      code = paymentType.Code,
      name = paymentType.Name,
      description = paymentType.Description
    };

    var expectedLocation = $"/payment-type/{paymentType.Id}";

    _testClient.PaymentTypeDao.Setup(x => x.ReplaceOneAsync(
      paymentType.Id.ToString(), It.IsAny<PaymentTypeEntity>()
    )).Returns(Task.FromResult(paymentType));

    var response = await _testClient.Put($"/payment-type/{paymentType.Id}", null, body);

    Assert.Equal(HttpStatusCode.Accepted, response.StatusCode);
    Assert.Equal(expectedLocation, response.Headers.Location!.ToString());
  }
}
