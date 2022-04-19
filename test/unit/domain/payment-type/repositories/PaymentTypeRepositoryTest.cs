using Xunit;
using Moq;
using System;
using Bogus;

using src.domain.common.interfaces;
using src.domain.payment_type.entities;
using src.domain.payment_type.repositories;

using test.builders.payment_type;

namespace test.unit.domain.payment_type.repositories;

public class PaymentTypeRepositoryTest
{
  private Mock<IDatabase<PaymentType>> _mockDatabase;
  private Faker _faker = new Faker();

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
    PaymentTypeRepository instance = new PaymentTypeRepository(_mockDatabase.Object);

    var result = await instance.Create(paymentType);

    Assert.Equal(paymentType.Id, result);
    _mockDatabase.Verify(x => x.Save(paymentType), Times.Once);
  }

  [Fact(DisplayName = "should fail when database fails")]
  [Trait("Method", "Create")]
  public async void CreateFail()
  {
    PaymentType paymentType = PaymentTypeEntityBuilder.build();
    string errorMessage = _faker.Lorem.Sentence();
    string expectedError = $"Error creating payment type: {errorMessage}";

    _mockDatabase.Setup(x => x.Save(paymentType)).Throws(new Exception(errorMessage));
    PaymentTypeRepository instance = new PaymentTypeRepository(_mockDatabase.Object);

    var error = await Assert.ThrowsAsync<ResponseError>(() => instance.Create(paymentType));
    Assert.Equal(expectedError, error.Message);
    _mockDatabase.Verify(x => x.Save(paymentType), Times.Once);
  }
}
