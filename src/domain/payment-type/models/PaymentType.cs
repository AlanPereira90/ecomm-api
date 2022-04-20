using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

using src.domain.payment_type.entities;

namespace src.domain.payment_type.models;
public class PaymentType
{
  public static PaymentType FromEntity(PaymentTypeEntity entity)
  {
    return new PaymentType
    {
      Id = entity.Id.ToString(),
      Code = entity.Code,
      Name = entity.Name,
      Description = entity.Description,
      Enabled = entity.Enabled,
      ExtraInfo = entity.ExtraInfo
    };
  }

  public PaymentTypeEntity ToEntity()
  {
    return new PaymentTypeEntity(
      Guid.Parse(this.Id), this.Code, this.Name, this.Description, this.Enabled, this.ExtraInfo
    );
  }

  [BsonId]
  public string Id { get; set; }
  public string Code { get; set; }
  public string Name { get; set; }
  public string Description { get; set; }
  public bool Enabled { get; set; }
  public Dictionary<string, dynamic> ExtraInfo { get; set; }
}
