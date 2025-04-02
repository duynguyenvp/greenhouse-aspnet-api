using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using NpgsqlTypes;

namespace greenhouse_aspnet_api.db.Models;

public enum DeviceType
{
  Switch,
  Motor
}

public enum DeviceStatus
{
  Forward,
  Reverse,
  Stopped,
  Running,
  Pending
}

[Table("device")]
public class Device
{
  [Key]
  [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
  [Column("id")]
  public int Id { get; set; }

  [Required]
  [Column("name")]
  public string Name { get; set; } = string.Empty;

  [Required]
  [EnumDataType(typeof(DeviceType))]
  [Column("type", TypeName = "device_type_enum")]
  public DeviceType Type { get; set; } = DeviceType.Switch;

  [Required]
  [EnumDataType(typeof(DeviceStatus))]
  [Column("status", TypeName = "device_status_enum")]
  public DeviceStatus Status { get; set; } = DeviceStatus.Stopped;

  [Required]
  [Column("code", TypeName = "text")]
  public string Code { get; set; } = string.Empty;

  [Required]
  [Column("hasRelay")]
  public bool HasRelay { get; set; } = false;

  [Column("createdAt")]
  public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

  [Column("updatedAt")]
  public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

  [Column("search_vector", TypeName = "tsvector")]
  public NpgsqlTsVector SearchVector { get; set; }
}
