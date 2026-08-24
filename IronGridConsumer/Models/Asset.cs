using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
namespace IronGridConsumer.Models;

public class Asset
{
    public int Id { get; set; }
    [Required]
    public int UnitId { get; set; }
    [Required]
    public string AssetSerial { get; set; } = string.Empty;
    [RegularExpression("^(UAV|PerimeterSensor)$")]
    public string AssetType { get; set; } = "GenericAsset";
    public Unit Unit { get; set; } = null!;
    public AssetLiveStatus? assetLive { get; set; }

}
