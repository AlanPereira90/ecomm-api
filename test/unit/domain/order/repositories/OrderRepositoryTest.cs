using System;

using Xunit;
using Moq;
using Bogus;
using KellermanSoftware.CompareNetObjects;

using src.domain.order.interfaces;
using src.domain.order.entities;
using src.domain.order.repositories;

using test.builders.order;

namespace test.unit.domain.order.repositories;

public class OrderRepositoryTest
{
  private Mock<IOrderDao> _mockDao;
  private Faker _faker = new Faker();
  private CompareLogic _comparer = new CompareLogic();

  public OrderRepositoryTest()
  {
    _mockDao = new Mock<IOrderDao>();
  }

  [Fact(DisplayName = "should create a new order successfully")]
  [Trait("Method", "Create")]
  public async void CreateOrderSuccess()
  {
    OrderEntity entity = OrderEntityBuilder.Build();

    _mockDao.Setup(x => x.InsertOneAsync(entity)).ReturnsAsync(entity);
    IOrderRepository instance = new OrderRepository(_mockDao.Object);

    var result = await instance.Create(entity);

    Assert.True(_comparer.Compare(entity, result).AreEqual);
    _mockDao.Verify(x => x.InsertOneAsync(entity), Times.Once);
  }

  [Fact(DisplayName = "should fail when dao fails")]
  [Trait("Method", "Create")]
  public async void CreateOrderFail()
  {
    OrderEntity entity = OrderEntityBuilder.Build();
    string errorMessage = _faker.Lorem.Sentence();

    _mockDao.Setup(x => x.InsertOneAsync(entity)).Throws(new Exception(errorMessage));
    IOrderRepository instance = new OrderRepository(_mockDao.Object);

    var error = await Assert.ThrowsAsync<ResponseError>(() => instance.Create(entity));
    Assert.Equal(errorMessage, error.Message);
    _mockDao.Verify(x => x.InsertOneAsync(entity), Times.Once);
  }
}
