using Xunit;
using Moq;
using Bogus;
using KellermanSoftware.CompareNetObjects;

using src.domain.common.interfaces;
using src.domain.payment_type.entities;
using src.domain.payment_type.interfaces;
using src.domain.payment_type.repositories;

using test.builders.payment_type;

namespace test.unit.domain.payment_type.repositories;

public class PaymentTypeRepositoryTest
{
  private Mock<IDatabase<PaymentType>> _mockDatabase;
  private Faker _faker = new Faker();
  private CompareLogic _comparer = new CompareLogic();

  public PaymentTypeRepositoryTest()
  {
    _mockDatabase = new Mock<IDatabase<PaymentType>>();
  }

  [Fact(DisplayName = "should create a new payment type successfully")]
  [Trait("Method", "Create")]
  public async void CreateSuccess()
  {
    PaymentType paymentType = PaymentTypeEntityBuilder.build();

    _mockDatabase.Setup(x => x.Save(paymentType)).ReturnsAsync(paymentType);
    IPaymentTypeRepository instance = new PaymentTypeRepository(_mockDatabase.Object);

    var result = await instance.Create(paymentType);

    Assert.True(_comparer.Compare(paymentType, result).AreEqual);
    _mockDatabase.Verify(x => x.Save(paymentType), Times.Once);
  }
}
