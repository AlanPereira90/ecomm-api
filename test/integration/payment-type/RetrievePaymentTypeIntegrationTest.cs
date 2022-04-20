using Xunit;
using Moq;
using Bogus;

using System;
using System.Net;
using System.Threading.Tasks;

using src.domain.payment_type.entities;
using test.builders.payment_type;

namespace test.integration;

public class RetrieveSimulationIntegrationTest
{
  private TestClient _testClient;
  private Faker _faker;

  public RetrieveSimulationIntegrationTest()
  {
    _testClient = new TestClient();
    _faker = new Faker();
  }

  [Fact(DisplayName = "should return 200 OK CREATED when payment type is found")]
  public async void RetrievePaymentTypeSuccess()
  {
    PaymentTypeEntity paymentType = PaymentTypeEntityBuilder.Build();

    _testClient.PaymentTypeDao.Setup(x => x.FindAsync(
      paymentType.Id.ToString()
    )).Returns(Task.FromResult(paymentType));

    var response = await _testClient.Get($"/payment-type/{paymentType.Id}");

    Assert.Equal(HttpStatusCode.OK, response.StatusCode);
  }

  [Fact(DisplayName = "should return 404 NOT_FOUND when payment type is not found")]
  public async void RetrievePaymentTypeNotFound()
  {
    var response = await _testClient.Get($"/payment-type/{Guid.NewGuid()}");

    Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
  }

  [Fact(DisplayName = "should return 500 internal server error when database is down")]
  public async void RetrievePaymentTypeServerError()
  {
    Guid id = Guid.NewGuid();
    _testClient.PaymentTypeDao.Setup(x => x.FindAsync(id.ToString())).Throws(new Exception());

    var response = await _testClient.Get($"/payment-type/{id}");

    Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
  }
}
