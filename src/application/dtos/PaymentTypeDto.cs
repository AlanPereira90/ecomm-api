using System.ComponentModel.DataAnnotations;
using src.domain.payment_type.entities;

namespace src.application.dtos;

public class PaymentTypeDto
{
  public string Id { get; private set; }
  [Required] public string Code { get; set; }
  [Required] public string Name { get; set; }
  [Required] public string Description { get; set; }
  public bool Enabled { get; private set; }

  public PaymentTypeEntity ToDomain() => new PaymentTypeEntity(
    this.Code, this.Name, this.Description
  );
}
