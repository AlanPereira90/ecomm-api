using System;
using System.Net;

using src.domain.payment_type.interfaces;
using src.domain.payment_type.entities;

namespace src.domain.payment_type.repositories;

public class PaymentTypeRepository : IPaymentTypeRepository
{
  private readonly IPaymentTypeDao _dao;

  public PaymentTypeRepository(IPaymentTypeDao dao)
  {
    this._dao = dao;
  }

  public Task<PaymentTypeEntity> Create(PaymentTypeEntity paymentType)
  {
    return this._dao.InsertOneAsync(paymentType);
  }
}
