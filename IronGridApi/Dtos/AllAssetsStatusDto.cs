using System.ComponentModel.DataAnnotations;

namespace IronGridApi.Dtos;

public class AllAssetsStatusDto
{
    public int Id { get; set; }
    [Required]
    public int UnitId { get; set; }
    [Required]
    public string AssetSerial { get; set; } = string.Empty;
    [RegularExpression("^(UAV|PerimeterSensor)$")]
    public string AssetType { get; set; } = "GenericAsset";
    [Required]
    public string? RawValue { get; set; } = string.Empty;
    [RegularExpression("^(Stable|Warning)$")]
    public string? ProcessedStatus { get; set; } = string.Empty;
    [Required]
    public bool? IsVerified { get; set; }
    [Required]
    public DateTime LastUpdate { get; set; }
}
