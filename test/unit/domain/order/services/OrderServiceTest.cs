using Xunit;
using Moq;

using src.domain.order.interfaces;
using src.domain.order.entities;
using src.domain.order.services;
using src.domain.payment_type.interfaces;
using src.domain.payment_type.repositories;
using src.domain.payment_type.entities;

using test.builders.order;
using test.builders.payment_type;

namespace test.unit.domain.order.services;

public class OrderServiceTest
{
  private readonly Mock<IOrderRepository> _mockOrderRepository;
  private readonly Mock<IPaymentTypeRepository> _mockPaymentTypeRepository;

  public OrderServiceTest()
  {
    _mockOrderRepository = new Mock<IOrderRepository>();
    _mockPaymentTypeRepository = new Mock<IPaymentTypeRepository>();
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
}
