using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IronGridConsumer.Models;

public class Report
{
    public int AssetId { get; set; }
    public string AssetType { get; set; } = string.Empty;
    public string RawValue { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; }
}
