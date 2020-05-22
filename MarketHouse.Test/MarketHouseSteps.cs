using MarketHouse.Test.Core;
using MarketHouse.Test.Data;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using TechTalk.SpecFlow;

namespace MarketHouse.Test
{
    [Binding]
    [Scope(Tag = "Orders")]
    public class MarketHouseSteps
    {
        private SessionTest sessionManager = new SessionTest();
        OrderResult result;

        [Given(@"registered users")]
        public void GivenRegisteredUsers(Table table)
        {          
            foreach (var row in table.Rows)
            {
                sessionManager.AddRecord(
                    new User
                    {
                        UserId = row["UserId"],
                        Name = row["Name"],
                        Surname = row["Surname"],
                        DeliveryCity = row["City"],
                        DeliveryAddress = row["Delivery address"],
                        Mail = row["Mail"]
                    });
            }
        }

        [Given(@"The warehouse")]
        public void GivenTheWarehouse(Table table)
        {           
            foreach (var row in table.Rows)
            {
                sessionManager.AddRecord(
                    new Product
                    {
                        Code = row["Code"],
                        Name = row["Products"],
                        Quantity = Convert.ToDecimal(row["Quantity"]),
                        UnitMeasure = row["Unit of measure"],
                        Threshold = Convert.ToDecimal(row["Alert threshold"])
                    });
            }
        }

        [When(@"An order arrives")]
        public void WhenAnOrderArrives(Table table)
        {          
            OrderCore core = new OrderCore(sessionManager);
            List<Order> order = new List<Order>();
            foreach (var row in table.Rows)
            {
                order.Add(
                    new Order
                    {
                        User = row["User"],
                        Product = row["Products"],
                        Quantity = Convert.ToDecimal(row["Quantity"]),
                    });
            }

            result = core.AcceptOrder(order);
        }

        [Then(@"The warehouse contains these products")]
        public void ThenTheWarehouseContainsTheseProducts(Table table)
        {           
            var products = sessionManager.Query<Product>();
            foreach (var row in table.Rows)
            {
                var product = products.Where(x => x.Code == row["Code"]).Single();
                product.Quantity.ShouldBe(Convert.ToDecimal(row["Quantity"]));
            }
        }

        [Then(@"the Purchasing Office is notified")]
        public void ThenThePurchasingOfficeIsNotified(Table table)
        {           
            if (table.Rows.Count == 0)
                result.AlertThresholds.Count().ShouldBe(0);
            else
            {
                result.AlertThresholds.Count().ShouldBe(table.Rows.Count);
                foreach (var row in table.Rows)
                {
                    var product = result.AlertThresholds.SingleOrDefault(x => x.product == row["Product under threshold"]);

                    product.ShouldNotBeNull();
                    product.Quantity.ShouldBe(Convert.ToDecimal(row["Quantity"]));
                    product.Threshold.ShouldBe(Convert.ToDecimal(row["Threshold"]));
                }
            }
        }
    }
}
