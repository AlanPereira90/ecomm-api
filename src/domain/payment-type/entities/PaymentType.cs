namespace src.domain.payment_type.entities;

public class PaymentType
{
  public PaymentType(string code, string name, string description)
  {
    this.Id = Guid.NewGuid();
    this.Code = code;
    this.Name = name;
    this.Description = description;
    this.Enabled = true;
    this.ExtraInfo = new Dictionary<string, dynamic>();
  }

  public void AddExtraInfo(string key, dynamic value)
  {
    this.ExtraInfo.Add(key, value);
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
  public Dictionary<string, dynamic> ExtraInfo { get; private set; }
}
