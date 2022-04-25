using src.domain.payment_type.entities;

namespace src.application.dtos;

public class UpdatePaymentTypeDto
{
  public string Id { get; private set; }
  public string Code { get; set; }
  public string Name { get; set; }
  public string Description { get; set; }

  public PaymentTypeEntity ToDomain(Guid id) => new PaymentTypeEntity(
    id, this.Code, this.Name, this.Description
  );
}
