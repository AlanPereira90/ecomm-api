using Xunit;
using Moq;
using Bogus;

using System;
using System.Net;
using System.Collections.Generic;

using src.domain.order.entities;
using src.domain.payment_type.entities;
using test.builders.order;
using test.builders.payment_type;

namespace test.integration;

public class RetrieveOrderIntegrationTest
{
  private TestClient _testClient;
  private Faker _faker;

  public RetrieveOrderIntegrationTest()
  {
    _testClient = new TestClient();
    _faker = new Faker();
  }

  [Fact(DisplayName = "should return 200 OK given a valid order id and user id")]
  public async void CreateOrderSuccess()
  {
    OrderEntity order = OrderEntityBuilder.Build();

    var headers = new Dictionary<string, string>
    {
      { "user-id", order.UserId.ToString() }
    };

    _testClient.OrderDao.Setup(x => x.FindAsync(
      order.Id.ToString()
    )).ReturnsAsync(order);

    var response = await _testClient.Get($"/order/{order.Id}", headers);

    Assert.Equal(HttpStatusCode.OK, response.StatusCode);
  }

  [Fact(DisplayName = "should return 403 FORBIDDEN given an invalid user id")]
  public async void CreateOrderInvalidUserId()
  {
    OrderEntity order = OrderEntityBuilder.Build();

    var headers = new Dictionary<string, string>
    {
      { "user-id", _faker.Random.AlphaNumeric(10) }
    };

    _testClient.OrderDao.Setup(x => x.FindAsync(
      order.Id.ToString()
    )).ReturnsAsync(order);

    var response = await _testClient.Get($"/order/{order.Id}", headers);

    Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
  }

  [Fact(DisplayName = "should return 404 NOT_FOUND given an invalid order id")]
  public async void CreateOrderNotFound()
  {
    OrderEntity order = OrderEntityBuilder.Build();

    var headers = new Dictionary<string, string>
    {
      { "user-id", _faker.Random.AlphaNumeric(10) }
    };

    var response = await _testClient.Get($"/order/{order.Id}", headers);

    Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
  }

  [Fact(DisplayName = "should return 400 BAD_REQUEST given invalid headers")]
  public async void CreateOrderInvalidHeaders()
  {
    OrderEntity order = OrderEntityBuilder.Build();

    var headers = new Dictionary<string, string>
    {
      { "user-ud", order.UserId.ToString() }
    };

    var response = await _testClient.Get($"/order/{order.Id}", headers);

    Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
  }

  [Fact(DisplayName = "should return 500 INTERNAL_SER_ERROR when database is down")]
  public async void CreateOrderServerError()
  {
    OrderEntity order = OrderEntityBuilder.Build();

    var headers = new Dictionary<string, string>
    {
      { "user-id", order.UserId.ToString() }
    };

    _testClient.OrderDao.Setup(x => x.FindAsync(
      order.Id.ToString()
    )).ThrowsAsync(new Exception());

    var response = await _testClient.Get($"/order/{order.Id}", headers);

    Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
  }
}
