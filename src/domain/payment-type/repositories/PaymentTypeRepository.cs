using src.domain.payment_type.interfaces;
using src.domain.payment_type.entities;
using src.domain.common.interfaces;

namespace src.domain.payment_type.repositories;

public class PaymentTypeRepository : IPaymentTypeRepository
{
  private readonly IDatabase<PaymentType> _database;

  public PaymentTypeRepository(IDatabase<PaymentType> database)
  {
    this._database = database;
  }

  async public Task<Guid> Create(PaymentType paymentType)
  {
    var result = await this._database.Save(paymentType);
    return result.Id;
  }
}
