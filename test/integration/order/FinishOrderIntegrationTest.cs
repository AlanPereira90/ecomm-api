using Xunit;
using Moq;
using Bogus;

using System;
using System.Net;
using System.Collections.Generic;

using src.domain.order.entities;
using test.builders.order;

namespace test.integration;

public class FinishOrderIntegrationTest
{
  private TestClient _testClient;
  private Faker _faker;

  public FinishOrderIntegrationTest()
  {
    _testClient = new TestClient();
    _faker = new Faker();
  }

  [Fact(DisplayName = "should return 202 ACCEPTED when order is successfully finished")]
  public async void FinishOrderSuccess()
  {
    OrderEntity order = OrderEntityBuilder.Build();
    order.Confirm();

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

    var response = await _testClient.Patch($"/order/{order.Id}/finish", headers);

    Assert.Equal(HttpStatusCode.Accepted, response.StatusCode);
    Assert.Equal(expectedLocation, response.Headers.Location!.ToString());
  }

  [Fact(DisplayName = "should return 404 NOT_FOUND when order is not found")]
  public async void FinishOrderNotFound()
  {
    OrderEntity order = OrderEntityBuilder.Build();

    var headers = new Dictionary<string, string>
    {
      { "user-id", order.UserId.ToString() }
    };

    var response = await _testClient.Patch($"/order/{order.Id}/finish", headers);

    Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
  }

  [Fact(DisplayName = "should return 403 FORBIDDEN when order does not belong to user")]
  public async void FinishOrderNotBelongToUser()
  {
    OrderEntity order = OrderEntityBuilder.Build();

    var headers = new Dictionary<string, string>
    {
      { "user-id", _faker.Random.AlphaNumeric(10) }
    };

    _testClient.OrderDao.Setup(x => x.FindAsync(
      order.Id.ToString()
    )).ReturnsAsync(order);

    var response = await _testClient.Patch($"/order/{order.Id}/finish", headers);

    Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
  }

  [Fact(DisplayName = "should return 422 UNPROCESSABLE_ENTITY when order status is not PAID")]
  public async void FinishOrderInvalidStatus()
  {
    OrderEntity order = OrderEntityBuilder.Build();

    var headers = new Dictionary<string, string>
    {
      { "user-id", order.UserId.ToString() }
    };

    _testClient.OrderDao.Setup(x => x.FindAsync(
      order.Id.ToString()
    )).ReturnsAsync(order);

    var response = await _testClient.Patch($"/order/{order.Id}/finish", headers);

    Assert.Equal(HttpStatusCode.UnprocessableEntity, response.StatusCode);
  }

  [Fact(DisplayName = "should return 400 BAD_REQUEST with invalid headers")]
  public async void FinishOrderInvalidHeaders()
  {
    OrderEntity order = OrderEntityBuilder.Build();

    var headers = new Dictionary<string, string>
    {
      { "user-ud", order.UserId.ToString() }
    };

    var response = await _testClient.Patch($"/order/{order.Id}/finish", headers);

    Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
  }

  [Fact(DisplayName = "should return 500 INTERNAL_SERVER_ERROR when database is down")]
  public async void FinishOrderServerError()
  {
    OrderEntity order = OrderEntityBuilder.Build();

    var headers = new Dictionary<string, string>
    {
      { "user-id", order.UserId.ToString() }
    };

    _testClient.OrderDao.Setup(x => x.FindAsync(
      order.Id.ToString()
    )).ThrowsAsync(new Exception());

    var response = await _testClient.Patch($"/order/{order.Id}/finish", headers);

    Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
  }
}
