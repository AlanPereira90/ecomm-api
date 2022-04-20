using Xunit;
using Moq;
using Bogus;

using System;
using System.Net;
using System.Threading.Tasks;

using src.domain.payment_type.entities;
using test.builders.payment_type;

namespace test.integration;

public class CreatePaymentTypeIntegrationTest
{
  private TestClient _testClient;
  private Faker _faker;

  public CreatePaymentTypeIntegrationTest()
  {
    _testClient = new TestClient();
    _faker = new Faker();
  }

  [Fact(DisplayName = "should return 201 CREATED when payment type is sucessfully created")]
  public async void CreatePaymentTypeSuccess()
  {
    PaymentTypeEntity paymentType = PaymentTypeEntityBuilder.Build();
    var body = new
    {
      code = paymentType.Code,
      name = paymentType.Name,
      description = paymentType.Description
    };

    var expectedLocation = $"/payment-type/{paymentType.Id}";

    _testClient.PaymentTypeDao.Setup(x => x.InsertOneAsync(
      It.IsAny<PaymentTypeEntity>()
    )).Returns(Task.FromResult(paymentType));

    var response = await _testClient.Post("/payment-type", null, body);

    Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    Assert.Equal(expectedLocation, response.Headers.Location!.ToString());
  }

  [Fact(DisplayName = "should return 400 BAD_REQUEST given an invalid body")]
  public async void CreatePaymentTypeInvalidBody()
  {
    PaymentTypeEntity paymentType = PaymentTypeEntityBuilder.Build();
    var body = new
    {
      code = paymentType.Code,
      description = paymentType.Description
    };

    var response = await _testClient.Post("/payment-type", null, body);

    Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
  }

  [Fact(DisplayName = "should return 500 internal server error when database is down")]
  public async void CreatePaymentTypeServerError()
  {
    PaymentTypeEntity paymentType = PaymentTypeEntityBuilder.Build();
    var body = new
    {
      code = paymentType.Code,
      name = paymentType.Name,
      description = paymentType.Description
    };

    _testClient.PaymentTypeDao.Setup(x => x.InsertOneAsync(
      It.IsAny<PaymentTypeEntity>()
    )).Throws(new Exception());

    var response = await _testClient.Post("/payment-type", null, body);

    Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
  }
}
