using src.domain.payment_type.entities;

namespace src.domain.payment_type.interfaces;

public interface IPaymentTypeService
{
  Task<Guid> Create(PaymentTypeEntity paymentType);
  Task<PaymentTypeEntity> FindOne(Guid id);
  Task<Guid> UpdateOne(PaymentTypeEntity paymentType);
}
