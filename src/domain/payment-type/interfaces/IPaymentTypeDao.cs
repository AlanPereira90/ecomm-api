using src.domain.payment_type.entities;

namespace src.domain.payment_type.interfaces;

public interface IPaymentTypeDao
{
  Task<PaymentTypeEntity> InsertOneAsync(PaymentTypeEntity entity);
  Task<PaymentTypeEntity> FindAsync(string id);
  Task<PaymentTypeEntity> ReplaceOneAsync(string id, PaymentTypeEntity entity);
}
