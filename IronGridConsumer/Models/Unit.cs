using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IronGridConsumer.Models;

public class Unit
{
    public int Id { get; set; }
    public string UnitName { get; set; } = "Unknown Unit";
    public string Sector { get; set; } = "General";
    public ICollection<Asset> assets = new List<Asset>();
}
