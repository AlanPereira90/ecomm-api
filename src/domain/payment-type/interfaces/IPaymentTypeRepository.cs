using src.domain.payment_type.entities;

namespace src.domain.payment_type.interfaces;

public interface IPaymentTypeRepository
{
  Task<PaymentType> Create(PaymentType paymentType);
}
