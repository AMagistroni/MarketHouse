using System;
using System.Collections.Generic;
using System.Text;

namespace MarketHouse.Test.Data
{
    public class Product
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public decimal Quantity { get; set; }
        public string UnitMeasure { get; set; }
        public decimal Threshold { get; set; }
    }
}
