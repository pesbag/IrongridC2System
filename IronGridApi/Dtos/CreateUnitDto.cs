using IronGridApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IronGridApi.Dtos;

public class CreateUnitDto
{
    public int Id { get; set; }
    public string UnitName { get; set; } = "Unknown Unit";
    public string Sector { get; set; } = "General";
}
