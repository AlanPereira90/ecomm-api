namespace src.domain.payment_type.entities;

public class PaymentTypeEntity
{
  public PaymentTypeEntity(
    Guid id,
    string code,
    string name,
    string description,
    bool enabled)
  {
    Id = id;
    Code = code;
    Name = name;
    Description = description;
    Enabled = enabled;
  }

  public PaymentTypeEntity(string code, string name, string description)
  {
    this.Id = Guid.NewGuid();
    this.Code = code;
    this.Name = name;
    this.Description = description;
    this.Enabled = true;
  }

  public void Disable()
  {
    this.Enabled = false;
  }

  public void Enable()
  {
    this.Enabled = true;
  }

  public Guid Id { get; private set; }
  public string Code { get; private set; }
  public string Name { get; private set; }
  public string Description { get; private set; }
  public bool Enabled { get; private set; }
}
