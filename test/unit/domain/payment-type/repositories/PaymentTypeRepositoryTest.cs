using System;
using System.Threading.Tasks;

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

  [Fact(DisplayName = "should fail when dao fails")]
  [Trait("Method", "Create")]
  public async void CreateFail()
  {
    PaymentTypeEntity paymentType = PaymentTypeEntityBuilder.Build();
    string errorMessage = _faker.Lorem.Sentence();

    _mockDao.Setup(x => x.InsertOneAsync(paymentType)).Throws(new Exception(errorMessage));
    IPaymentTypeRepository instance = new PaymentTypeRepository(_mockDao.Object);

    var error = await Assert.ThrowsAsync<ResponseError>(() => instance.Create(paymentType));
    Assert.Equal(errorMessage, error.Message);
    _mockDao.Verify(x => x.InsertOneAsync(paymentType), Times.Once);
  }

  [Fact(DisplayName = "should find a payment type successfully")]
  [Trait("Method", "FindOne")]
  public async void FindOneSuccess()
  {
    PaymentTypeEntity paymentType = PaymentTypeEntityBuilder.Build();

    _mockDao.Setup(x => x.FindAsync(paymentType.Id.ToString())).ReturnsAsync(paymentType);
    IPaymentTypeRepository instance = new PaymentTypeRepository(_mockDao.Object);

    var result = await instance.FindOne(paymentType.Id);

    Assert.True(_comparer.Compare(paymentType, result).AreEqual);
    _mockDao.Verify(x => x.FindAsync(paymentType.Id.ToString()), Times.Once);
  }

  [Fact(DisplayName = "should return empty")]
  [Trait("Method", "FindOne")]
  public async void FindOneEmpty()
  {
    PaymentTypeEntity paymentType = PaymentTypeEntityBuilder.Build();

    IPaymentTypeRepository instance = new PaymentTypeRepository(_mockDao.Object);

    var result = await instance.FindOne(paymentType.Id);

    Assert.Null(result);
    _mockDao.Verify(x => x.FindAsync(paymentType.Id.ToString()), Times.Once);
  }

  [Fact(DisplayName = "should fail when dao fails")]
  [Trait("Method", "FindOne")]
  public async void FindOneFail()
  {
    PaymentTypeEntity paymentType = PaymentTypeEntityBuilder.Build();
    string errorMessage = _faker.Lorem.Sentence();

    _mockDao.Setup(x => x.FindAsync(paymentType.Id.ToString())).Throws(new Exception(errorMessage));
    IPaymentTypeRepository instance = new PaymentTypeRepository(_mockDao.Object);

    var error = await Assert.ThrowsAsync<ResponseError>(() => instance.FindOne(paymentType.Id));
    Assert.Equal(errorMessage, error.Message);
    _mockDao.Verify(x => x.FindAsync(paymentType.Id.ToString()), Times.Once);
  }

  [Fact(DisplayName = "should update a payment type successfully")]
  [Trait("Method", "UpdateOne")]
  public async void UpdateOneSuccess()
  {
    PaymentTypeEntity paymentType = PaymentTypeEntityBuilder.Build();

    _mockDao.Setup(x => x.ReplaceOneAsync(paymentType.Id.ToString(), paymentType)).ReturnsAsync(paymentType);
    IPaymentTypeRepository instance = new PaymentTypeRepository(_mockDao.Object);

    var result = await instance.UpdateOne(paymentType);

    Assert.True(result);
    _mockDao.Verify(x => x.ReplaceOneAsync(paymentType.Id.ToString(), paymentType), Times.Once);
  }

  [Fact(DisplayName = "should return false when payment type is not found")]
  [Trait("Method", "UpdateOne")]
  public async void UpdateOneEmpty()
  {
    PaymentTypeEntity paymentType = PaymentTypeEntityBuilder.Build();

    IPaymentTypeRepository instance = new PaymentTypeRepository(_mockDao.Object);

    var result = await instance.UpdateOne(paymentType);

    Assert.False(result);
    _mockDao.Verify(x => x.ReplaceOneAsync(paymentType.Id.ToString(), paymentType), Times.Once);
  }

  [Fact(DisplayName = "should fail when dao fails")]
  [Trait("Method", "UpdateOne")]
  public async void UpdateOneFail()
  {
    PaymentTypeEntity paymentType = PaymentTypeEntityBuilder.Build();
    string errorMessage = _faker.Lorem.Sentence();

    _mockDao.Setup(x => x.ReplaceOneAsync(paymentType.Id.ToString(), paymentType)).Throws(new Exception(errorMessage));
    IPaymentTypeRepository instance = new PaymentTypeRepository(_mockDao.Object);

    var error = await Assert.ThrowsAsync<ResponseError>(() => instance.UpdateOne(paymentType));
    Assert.Equal(errorMessage, error.Message);
    _mockDao.Verify(x => x.ReplaceOneAsync(paymentType.Id.ToString(), paymentType), Times.Once);
  }
}
