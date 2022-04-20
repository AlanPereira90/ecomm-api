using System.Threading.Tasks;

using Xunit;
using Moq;
using KellermanSoftware.CompareNetObjects;

using src.domain.payment_type.interfaces;
using src.domain.payment_type.entities;
using src.domain.payment_type.services;

using test.builders.payment_type;

namespace test.unit.domain.payment_type.services;

public class PaymentTypeServiceTest
{
  private Mock<IPaymentTypeRepository> _mockRepository;
  private CompareLogic _comparer;

  public PaymentTypeServiceTest()
  {
    this._mockRepository = new Mock<IPaymentTypeRepository>();
    this._comparer = new CompareLogic();
  }

  [Fact(DisplayName = "should call repository and create a new payment type successfully")]
  [Trait("Method", "Create")]
  public async void CreateSuccessfull()
  {
    PaymentTypeEntity paymentType = PaymentTypeEntityBuilder.Build();

    _mockRepository.Setup(x => x.Create(paymentType)).ReturnsAsync(paymentType);
    IPaymentTypeService instance = new PaymentTypeService(_mockRepository.Object);

    var result = await instance.Create(paymentType);

    Assert.Equal(paymentType.Id, result);
    _mockRepository.Verify(x => x.Create(paymentType), Times.Once);
  }

  [Fact(DisplayName = "should find one payment type successfully")]
  [Trait("Method", "FindOne")]
  public async void FindOneSuccessfull()
  {
    PaymentTypeEntity paymentType = PaymentTypeEntityBuilder.Build();

    _mockRepository.Setup(x => x.FindOne(paymentType.Id)).ReturnsAsync(paymentType);
    IPaymentTypeService instance = new PaymentTypeService(_mockRepository.Object);

    var result = await instance.FindOne(paymentType.Id);

    Assert.True(_comparer.Compare(paymentType, result).AreEqual);
    _mockRepository.Verify(x => x.FindOne(paymentType.Id), Times.Once);
  }

  [Fact(DisplayName = "should fail when payment type is not found")]
  [Trait("Method", "FindOne")]
  public async void FindOneNotFound()
  {
    PaymentTypeEntity paymentType = PaymentTypeEntityBuilder.Build();

    IPaymentTypeService instance = new PaymentTypeService(_mockRepository.Object);

    var error = await Assert.ThrowsAsync<ResponseError>(() => instance.FindOne(paymentType.Id));

    Assert.Equal("Payment type not found", error.Message);
    _mockRepository.Verify(x => x.FindOne(paymentType.Id), Times.Once);
  }

  [Fact(DisplayName = "should update one payment type successfully")]
  [Trait("Method", "UpdateOne")]
  public async void UpdateOneSuccessfull()
  {
    PaymentTypeEntity paymentType = PaymentTypeEntityBuilder.Build();

    _mockRepository.Setup(x => x.UpdateOne(paymentType)).Returns(Task.FromResult(true));
    IPaymentTypeService instance = new PaymentTypeService(_mockRepository.Object);

    var result = await instance.UpdateOne(paymentType);

    Assert.Equal(paymentType.Id, result);
    _mockRepository.Verify(x => x.UpdateOne(paymentType), Times.Once);
  }

  [Fact(DisplayName = "should fail when payment type is not found")]
  [Trait("Method", "UpdateOne")]
  public async void UpdateOneNotFound()
  {
    PaymentTypeEntity paymentType = PaymentTypeEntityBuilder.Build();

    _mockRepository.Setup(x => x.UpdateOne(paymentType)).Returns(Task.FromResult(false));
    IPaymentTypeService instance = new PaymentTypeService(_mockRepository.Object);

    var error = await Assert.ThrowsAsync<ResponseError>(() => instance.UpdateOne(paymentType));

    Assert.Equal("Payment type not found", error.Message);
    _mockRepository.Verify(x => x.UpdateOne(paymentType), Times.Once);
  }
}
