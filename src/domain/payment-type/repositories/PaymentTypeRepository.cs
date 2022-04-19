using System;
using System.Net;

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

  public Task<PaymentType> Create(PaymentType paymentType)
  {
    return this._database.Save(paymentType);
  }
}
