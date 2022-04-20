using src.domain.payment_type.entities;

namespace src.domain.payment_type.interfaces;

public interface IPaymentTypeRepository
{
  Task<PaymentTypeEntity> Create(PaymentTypeEntity paymentType);
  Task<PaymentTypeEntity> FindOne(Guid id);
}
