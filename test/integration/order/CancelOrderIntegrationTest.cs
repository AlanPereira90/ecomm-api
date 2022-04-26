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

public class CancelOrderIntegrationTest
{
  private TestClient _testClient;
  private Faker _faker;

  public CancelOrderIntegrationTest()
  {
    _testClient = new TestClient();
    _faker = new Faker();
  }

  [Fact(DisplayName = "should return 202 ACCEPTED when order is successfully cancelled")]
  public async void CancelOrderSuccess()
  {
    OrderEntity order = OrderEntityBuilder.Build();

    var headers = new Dictionary<string, string>
    {
      { "user-id", order.UserId.ToString() }
    };

    _testClient.OrderDao.Setup(x => x.FindAsync(
      order.Id.ToString()
    )).ReturnsAsync(order);
    _testClient.OrderDao.Setup(x => x.ReplaceOneAsync(
      order.Id.ToString(), It.IsAny<OrderEntity>()
    )).ReturnsAsync(order);

    var expectedLocation = $"/order/{order.Id}";

    var response = await _testClient.Post($"/order/{order.Id}/cancel", headers);

    Assert.Equal(HttpStatusCode.Accepted, response.StatusCode);
    Assert.Equal(expectedLocation, response.Headers.Location!.ToString());
  }

  [Fact(DisplayName = "should return 404 NOT_FOUND when order is not found")]
  public async void CancelOrderNotFound()
  {
    OrderEntity order = OrderEntityBuilder.Build();

    var headers = new Dictionary<string, string>
    {
      { "user-id", order.UserId.ToString() }
    };

    var response = await _testClient.Post($"/order/{order.Id}/cancel", headers);

    Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
  }

  [Fact(DisplayName = "should return 403 FORBIDDEN when order does not belong to user")]
  public async void CancelOrderNotBelongToUser()
  {
    OrderEntity order = OrderEntityBuilder.Build();

    var headers = new Dictionary<string, string>
    {
      { "user-id", _faker.Random.AlphaNumeric(10) }
    };

    _testClient.OrderDao.Setup(x => x.FindAsync(
      order.Id.ToString()
    )).ReturnsAsync(order);

    var response = await _testClient.Post($"/order/{order.Id}/cancel", headers);

    Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
  }

  [Fact(DisplayName = "should return 422 UNPROCESSABLE_ENTITY when order status is not CREATED")]
  public async void CancelOrderInvalidStatus()
  {
    OrderEntity order = OrderEntityBuilder.Build();
    order.Cancel();

    var headers = new Dictionary<string, string>
    {
      { "user-id", order.UserId.ToString() }
    };

    _testClient.OrderDao.Setup(x => x.FindAsync(
      order.Id.ToString()
    )).ReturnsAsync(order);

    var response = await _testClient.Post($"/order/{order.Id}/cancel", headers);

    Assert.Equal(HttpStatusCode.UnprocessableEntity, response.StatusCode);
  }

  [Fact(DisplayName = "should return 400 BAD_REQUEST with invalid headers")]
  public async void CancelOrderInvalidHeaders()
  {
    OrderEntity order = OrderEntityBuilder.Build();

    var headers = new Dictionary<string, string>
    {
      { "user-ud", order.UserId.ToString() }
    };

    var response = await _testClient.Post($"/order/{order.Id}/cancel", headers);

    Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
  }

  [Fact(DisplayName = "should return 500 INTERNAL_SERVER_ERROR when database is down")]
  public async void CancelOrderServerError()
  {
    OrderEntity order = OrderEntityBuilder.Build();

    var headers = new Dictionary<string, string>
    {
      { "user-id", order.UserId.ToString() }
    };

    _testClient.OrderDao.Setup(x => x.FindAsync(
      order.Id.ToString()
    )).ThrowsAsync(new Exception());

    var response = await _testClient.Post($"/order/{order.Id}/cancel", headers);

    Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
  }
}
