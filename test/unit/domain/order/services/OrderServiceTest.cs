using Xunit;
using Moq;
using Bogus;

using src.domain.order.interfaces;
using src.domain.order.entities;
using src.domain.order.services;
using src.domain.payment_type.interfaces;
using src.domain.payment_type.entities;

using test.builders.order;
using test.builders.payment_type;

namespace test.unit.domain.order.services;

public class OrderServiceTest
{
  private readonly Mock<IOrderRepository> _mockOrderRepository;
  private readonly Mock<IPaymentTypeRepository> _mockPaymentTypeRepository;
  private readonly Faker _faker;

  public OrderServiceTest()
  {
    _mockOrderRepository = new Mock<IOrderRepository>();
    _mockPaymentTypeRepository = new Mock<IPaymentTypeRepository>();
    _faker = new Faker();
  }

  [Fact(DisplayName = "should create an order successfully")]
  [Trait("Method", "Create")]
  public async void CreateSuccess()
  {
    OrderEntity order = OrderEntityBuilder.Build();
    PaymentTypeEntity paymentType = PaymentTypeEntityBuilder.Build();

    _mockPaymentTypeRepository.Setup(x => x.FindOne(order.PaymentTypeId)).ReturnsAsync(paymentType);
    _mockOrderRepository.Setup(x => x.Create(order)).ReturnsAsync(order);
    IOrderService instance = new OrderService(_mockOrderRepository.Object, _mockPaymentTypeRepository.Object);

    var result = await instance.Create(order);

    Assert.Equal(order.Id, result);
    _mockPaymentTypeRepository.Verify(x => x.FindOne(order.PaymentTypeId), Times.Once);
    _mockOrderRepository.Verify(x => x.Create(order), Times.Once);
  }

  [Fact(DisplayName = "should fail with an invalid payment type")]
  [Trait("Method", "Create")]
  public async void CreateInvalidPaymentType()
  {
    OrderEntity order = OrderEntityBuilder.Build();

    IOrderService instance = new OrderService(_mockOrderRepository.Object, _mockPaymentTypeRepository.Object);

    var error = await Assert.ThrowsAsync<ResponseError>(() => instance.Create(order));

    Assert.Equal("Invalid payment type", error.Message);
    _mockPaymentTypeRepository.Verify(x => x.FindOne(order.PaymentTypeId), Times.Once);
    _mockOrderRepository.Verify(x => x.Create(order), Times.Never);
  }

  [Fact(DisplayName = "should fail with disabled payment type")]
  [Trait("Method", "Create")]
  public async void CreateDisabledPaymentType()
  {
    OrderEntity order = OrderEntityBuilder.Build();
    PaymentTypeEntity paymentType = PaymentTypeEntityBuilder.Build();
    paymentType.Disable();

    _mockPaymentTypeRepository.Setup(x => x.FindOne(order.PaymentTypeId)).ReturnsAsync(paymentType);
    IOrderService instance = new OrderService(_mockOrderRepository.Object, _mockPaymentTypeRepository.Object);

    var error = await Assert.ThrowsAsync<ResponseError>(() => instance.Create(order));
    Assert.Equal("Payment type is disabled", error.Message);
    _mockPaymentTypeRepository.Verify(x => x.FindOne(order.PaymentTypeId), Times.Once);
    _mockOrderRepository.Verify(x => x.Create(order), Times.Never);
  }

  [Fact(DisplayName = "should retrieve an order successfully")]
  [Trait("Method", "FindOne")]
  public async void FindOneSuccess()
  {
    OrderEntity order = OrderEntityBuilder.Build();

    _mockOrderRepository.Setup(x => x.FindOne(order.Id)).ReturnsAsync(order);
    IOrderService instance = new OrderService(_mockOrderRepository.Object, _mockPaymentTypeRepository.Object);

    var result = await instance.FindOne(order.Id, order.UserId);

    Assert.Equal(order, result);
    _mockOrderRepository.Verify(x => x.FindOne(order.Id), Times.Once);
  }

  [Fact(DisplayName = "should fail when order is not found")]
  [Trait("Method", "FindOne")]
  public async void FindOneNotFound()
  {
    OrderEntity order = OrderEntityBuilder.Build();

    IOrderService instance = new OrderService(_mockOrderRepository.Object, _mockPaymentTypeRepository.Object);

    var error = await Assert.ThrowsAsync<ResponseError>(() => instance.FindOne(order.Id, order.UserId));
    Assert.Equal("Order not found", error.Message);
    _mockOrderRepository.Verify(x => x.FindOne(order.Id), Times.Once);
  }

  [Fact(DisplayName = "should fail when order does not belong to user")]
  [Trait("Method", "FindOne")]
  public async void FindOneNotBelongToUser()
  {
    OrderEntity order = OrderEntityBuilder.Build();

    _mockOrderRepository.Setup(x => x.FindOne(order.Id)).ReturnsAsync(order);
    IOrderService instance = new OrderService(_mockOrderRepository.Object, _mockPaymentTypeRepository.Object);

    var error = await Assert.ThrowsAsync<ResponseError>(() => instance.FindOne(order.Id, _faker.Random.AlphaNumeric(10)));
    Assert.Equal("Order does not belong to user", error.Message);
    _mockOrderRepository.Verify(x => x.FindOne(order.Id), Times.Once);
  }
}
