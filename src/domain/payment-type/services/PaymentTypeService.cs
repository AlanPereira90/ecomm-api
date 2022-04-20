using System.Net;

using src.domain.payment_type.entities;
using src.domain.payment_type.interfaces;

namespace src.domain.payment_type.services;

public class PaymentTypeService : IPaymentTypeService
{
  private readonly IPaymentTypeRepository _paymentTypeRepository;

  public PaymentTypeService(IPaymentTypeRepository paymentTypeRepository)
  {
    _paymentTypeRepository = paymentTypeRepository;
  }

  private async Task<PaymentTypeEntity> GetPaymentType(Guid id)
  {
    var result = await _paymentTypeRepository.FindOne(id);

    if (result == null)
    {
      throw new ResponseError(HttpStatusCode.NotFound, "Payment type not found");
    }

    return result;
  }

  public async Task<Guid> Create(PaymentTypeEntity paymentType)
  {
    var result = await _paymentTypeRepository.Create(paymentType);
    return result.Id;
  }

  public async Task<PaymentTypeEntity> FindOne(Guid id)
  {
    return await GetPaymentType(id);
  }
}
