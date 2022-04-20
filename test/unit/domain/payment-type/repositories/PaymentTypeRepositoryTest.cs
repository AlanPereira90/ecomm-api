using Xunit;
using Moq;
using Bogus;
using KellermanSoftware.CompareNetObjects;

using src.domain.payment_type.entities;
using src.domain.payment_type.interfaces;
using src.domain.payment_type.repositories;

using test.builders.payment_type;

namespace test.unit.domain.payment_type.repositories;

public class PaymentTypeRepositoryTest
{
  private Mock<IPaymentTypeDao> _mockDao;
  private Faker _faker = new Faker();
  private CompareLogic _comparer = new CompareLogic();

  public PaymentTypeRepositoryTest()
  {
    _mockDao = new Mock<IPaymentTypeDao>();
  }

  [Fact(DisplayName = "should create a new payment type successfully")]
  [Trait("Method", "Create")]
  public async void CreateSuccess()
  {
    PaymentTypeEntity paymentType = PaymentTypeEntityBuilder.Build();

    _mockDao.Setup(x => x.InsertOneAsync(paymentType)).ReturnsAsync(paymentType);
    IPaymentTypeRepository instance = new PaymentTypeRepository(_mockDao.Object);

    var result = await instance.Create(paymentType);

    Assert.True(_comparer.Compare(paymentType, result).AreEqual);
    _mockDao.Verify(x => x.InsertOneAsync(paymentType), Times.Once);
  }
}
