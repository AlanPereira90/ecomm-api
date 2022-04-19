using Xunit;
using Moq;

using src.domain.payment_type.interfaces;
using src.domain.payment_type.entities;
using src.domain.payment_type.services;

using test.builders.payment_type;

namespace test.unit.domain.payment_type.services;

public class PaymentTypeServiceTest
{
  private Mock<IPaymentTypeRepository> _mockRepository;

  public PaymentTypeServiceTest()
  {
    this._mockRepository = new Mock<IPaymentTypeRepository>();
  }

  [Fact(DisplayName = "should call repository and create a new payment type successfully")]
  [Trait("Method", "Create")]
  public async void CreateSuccessfull()
  {
    PaymentTypeEntity paymentType = PaymentTypeEntityBuilder.build();

    _mockRepository.Setup(x => x.Create(paymentType)).ReturnsAsync(paymentType);
    IPaymentTypeService instance = new PaymentTypeService(_mockRepository.Object);

    var result = await instance.Create(paymentType);

    Assert.Equal(paymentType.Id, result);
    _mockRepository.Verify(x => x.Create(paymentType), Times.Once);
  }
}
