using System;
using System.Collections.Generic;
using System.Text;

namespace MarketHouse.Test.Data
{
    public class OrderResult
    {
        public OrderResult()
        {
            AlertThresholds = new List<AlertThreshold>();
        }
        public class AlertThreshold 
        {
            public string product { get; set; }
            public decimal Quantity { get; set; }
            public decimal Threshold { get; set; }
        }

        public List<AlertThreshold> AlertThresholds { get; set; }
    }
}
