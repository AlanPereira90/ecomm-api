using System;
using System.Net;

using Xunit;
using Moq;
using Bogus;

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
      name = paymentType.Name,
    };

    var expectedLocation = $"/payment-type/{paymentType.Id}";

    _testClient.PaymentTypeDao.Setup(x => x.FindAsync(
      paymentType.Id.ToString()
    )).ReturnsAsync(paymentType);
    _testClient.PaymentTypeDao.Setup(x => x.ReplaceOneAsync(
      paymentType.Id.ToString(), It.IsAny<PaymentTypeEntity>()
    )).ReturnsAsync(paymentType);

    var response = await _testClient.Put($"/payment-type/{paymentType.Id}", null, body);

    Assert.Equal(HttpStatusCode.Accepted, response.StatusCode);
    Assert.Equal(expectedLocation, response.Headers.Location!.ToString());
  }

  [Fact(DisplayName = "should return 404 NOT_FOUND when payment type is not found")]
  public async void UpdatePaymentTypeNotFound()
  {
    PaymentTypeEntity paymentType = PaymentTypeEntityBuilder.Build();
    var body = new
    {
      code = paymentType.Code,
    };

    var response = await _testClient.Put($"/payment-type/{paymentType.Id}", null, body);

    Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
  }

  [Fact(DisplayName = "should return 500 INTERNAL_SERVER_ERROR when database is down")]
  public async void UpdatePaymentTypeServerError()
  {
    PaymentTypeEntity paymentType = PaymentTypeEntityBuilder.Build();
    var body = new
    {
      code = paymentType.Code,
    };

    _testClient.PaymentTypeDao.Setup(x => x.FindAsync(
      paymentType.Id.ToString()
    )).ThrowsAsync(new Exception());

    var response = await _testClient.Put($"/payment-type/{paymentType.Id}", null, body);

    Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
  }
}
