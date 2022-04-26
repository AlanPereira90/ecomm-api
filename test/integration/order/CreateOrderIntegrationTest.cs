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

public class CreateOrderIntegrationTest
{
  private TestClient _testClient;
  private Faker _faker;

  public CreateOrderIntegrationTest()
  {
    _testClient = new TestClient();
    _faker = new Faker();
  }

  [Fact(DisplayName = "should return 201 CREATED when order is sucessfully created")]
  public async void CreateOrderSuccess()
  {
    OrderEntity order = OrderEntityBuilder.Build();
    PaymentTypeEntity paymentType = PaymentTypeEntityBuilder.Build();
    var body = new
    {
      paymentTypeId = order.PaymentTypeId.ToString(),
      code = order.Code,
      delivery = order.Delivery,
      items = order.Items
    };

    var headers = new Dictionary<string, string>
    {
      { "user-id", order.UserId.ToString() }
    };

    var expectedLocation = $"/order/{order.Id}";

    _testClient.PaymentTypeDao.Setup(x => x.FindAsync(
      order.PaymentTypeId.ToString()
    )).ReturnsAsync(paymentType);
    _testClient.OrderDao.Setup(x => x.InsertOneAsync(
      It.IsAny<OrderEntity>()
    )).ReturnsAsync(order);

    var response = await _testClient.Post("/order", headers, body);

    Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    Assert.Equal(expectedLocation, response.Headers.Location!.ToString());
  }

  [Fact(DisplayName = "should return 400 BAD_REQUEST given an invalid body")]
  public async void CreateOrderInvalidBody()
  {
    OrderEntity order = OrderEntityBuilder.Build();
    PaymentTypeEntity paymentType = PaymentTypeEntityBuilder.Build();
    var body = new
    {
      code = order.Code,
      delivery = order.Delivery,
      items = order.Items
    };

    var headers = new Dictionary<string, string>
    {
      { "user-id", order.UserId.ToString() }
    };

    var response = await _testClient.Post("/order", headers, body);

    Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
  }

  [Fact(DisplayName = "should return 403 BAD_REQUEST given invalid headers")]
  public async void CreateOrderInvalidHeaders()
  {
    OrderEntity order = OrderEntityBuilder.Build();
    PaymentTypeEntity paymentType = PaymentTypeEntityBuilder.Build();
    var body = new
    {
      paymentTypeId = order.PaymentTypeId.ToString(),
      code = order.Code,
      delivery = order.Delivery,
      items = order.Items
    };

    var headers = new Dictionary<string, string>
    {
      { "user-ud", order.UserId.ToString() }
    };

    var response = await _testClient.Post("/order", headers, body);

    Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
  }

  [Fact(DisplayName = "should return 422 UNPROCESSABLE_ENTITY given invalid payment type")]
  public async void CreateOrderInvalidPaymentType()
  {
    OrderEntity order = OrderEntityBuilder.Build();
    var body = new
    {
      paymentTypeId = order.PaymentTypeId.ToString(),
      code = order.Code,
      delivery = order.Delivery,
      items = order.Items
    };

    var headers = new Dictionary<string, string>
    {
      { "user-id", order.UserId.ToString() }
    };

    var response = await _testClient.Post("/order", headers, body);

    Assert.Equal(HttpStatusCode.UnprocessableEntity, response.StatusCode);
  }

  [Fact(DisplayName = "should return 422 UNPROCESSABLE_ENTITY given a disabled payment type")]
  public async void CreateOrderDisabledPaymentType()
  {
    OrderEntity order = OrderEntityBuilder.Build();
    PaymentTypeEntity paymentType = PaymentTypeEntityBuilder.Build();
    paymentType.Disable();

    var body = new
    {
      paymentTypeId = order.PaymentTypeId.ToString(),
      code = order.Code,
      delivery = order.Delivery,
      items = order.Items
    };

    var headers = new Dictionary<string, string>
    {
      { "user-id", order.UserId.ToString() }
    };

    _testClient.PaymentTypeDao.Setup(x => x.FindAsync(
      order.PaymentTypeId.ToString()
    )).ReturnsAsync(paymentType);

    var response = await _testClient.Post("/order", headers, body);

    Assert.Equal(HttpStatusCode.UnprocessableEntity, response.StatusCode);
  }

  [Fact(DisplayName = "should return 500 INTERNAL_SERVER_ERROR when database is down")]
  public async void CreateOrderServerError()
  {
    OrderEntity order = OrderEntityBuilder.Build();
    var body = new
    {
      paymentTypeId = order.PaymentTypeId.ToString(),
      code = order.Code,
      delivery = order.Delivery,
      items = order.Items
    };

    var headers = new Dictionary<string, string>
    {
      { "user-id", order.UserId.ToString() }
    };

    _testClient.PaymentTypeDao.Setup(x => x.FindAsync(
      order.PaymentTypeId.ToString()
    )).ThrowsAsync(new Exception());

    var response = await _testClient.Post("/order", headers, body);

    Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
  }
}
