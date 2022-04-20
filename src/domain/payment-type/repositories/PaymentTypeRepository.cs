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
    try
    {
      return this._dao.InsertOneAsync(paymentType);
    }
    catch (Exception err)
    {
      Console.WriteLine($"Fail to create payment type: {err.Message}");

      throw new ResponseError(HttpStatusCode.InternalServerError, err.Message);
    }
  }

  public Task<PaymentTypeEntity> FindOne(Guid id)
  {
    try
    {
      return this._dao.FindAsync(id.ToString());
    }
    catch (Exception err)
    {
      Console.WriteLine($"Fail to find payment type: {err.Message}");

      throw new ResponseError(HttpStatusCode.InternalServerError, err.Message);
    }
  }

  public async Task<bool> UpdateOne(PaymentTypeEntity paymentType)
  {
    try
    {
      var result = await this._dao.ReplaceOneAsync(paymentType.Id.ToString(), paymentType);
      return result != null;
    }
    catch (Exception err)
    {
      Console.WriteLine($"Fail to update payment type: {err.Message}");

      throw new ResponseError(HttpStatusCode.InternalServerError, err.Message);
    }
  }
}
