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
    [Scope(Tag = "Ordine")]
    public class MarketHouseSteps
    {
        private SessionTest sessionManager = new SessionTest();
        OrderResult result;
        
        [Given(@"gli utenti registrati")]
        public void GivenGliUtentiRegistrati(Table table)
        {            
            foreach (var row in table.Rows)
            {
                sessionManager.AddRecord(
                    new User
                    {
                        UserId = row["UserId"],
                        Name = row["Nome"],
                        Surname = row["Cognome"],
                        DeliveryCity = row["Indirizzo spedizione"],
                        DeliveryAddress = row["Indirizzo spedizione"],
                        Mail = row["Mail"]
                    });
            }
        }
        
        [Given(@"Il magazzino")]
        public void GivenIlMagazzino(Table table)
        {            
            foreach (var row in table.Rows)
            {
                sessionManager.AddRecord(
                    new Product
                    {
                        Code = row["Codice"],
                        Name = row["Prodotto"],
                        Quantity = Convert.ToDecimal(row["Quantità"]),
                        UnitMeasure = row["Unita di misura"],
                        Threshold = Convert.ToDecimal(row["Soglia di alert"])
                    });
            }
        }
        
        [When(@"Arriva un ordine")]
        public void WhenArrivaUnOrdine(Table table)
        {
            OrderCore core = new OrderCore(sessionManager);
            List<Order> order = new List<Order>();
            foreach (var row in table.Rows)
            {
                order.Add(
                    new Order
                    {
                        User = row["Utente"],
                        Product = row["Prodotto"],
                        Quantity = Convert.ToDecimal(row["Quantità"]),
                    });
            }

            result = core.AcceptOrder(order);
        }
               
        [Then(@"il magazzino contiene questi prodotti")]
        public void ThenIlMagazzinoContieneQuestiProdotti(Table table)
        {
            var products = sessionManager.Query<Product>();
            foreach (var row in table.Rows)
            {
                var product = products.Where(x => x.Code == row["Codice"]).Single();
                product.Quantity.ShouldBe(Convert.ToDecimal(row["Quantità"]));
            }
        }
        
        [Then(@"viene avvertito l'ufficio Acquisti")]
        public void ThenVieneAvvertitoLUfficioAcquisti(Table table)
        {
            if (table.Rows.Count == 0)
                result.AlertThresholds.Count().ShouldBe(0);
            else
            {
                result.AlertThresholds.Count().ShouldBe(table.Rows.Count);
                foreach (var row in table.Rows)
                {
                    var product = result.AlertThresholds.SingleOrDefault(x => x.product == row["Prodotti sotto soglia"]);

                    product.ShouldNotBeNull();
                    product.Quantity.ShouldBe(Convert.ToDecimal(row["Quantità"]));
                    product.Threshold.ShouldBe(Convert.ToDecimal(row["Soglia"]));
                }
            }
        }
    }
}
