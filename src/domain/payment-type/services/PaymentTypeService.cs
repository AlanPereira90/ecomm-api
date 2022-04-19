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

  public async Task<Guid> Create(PaymentTypeEntity paymentType)
  {
    var result = await _paymentTypeRepository.Create(paymentType);
    return result.Id;
  }
}
