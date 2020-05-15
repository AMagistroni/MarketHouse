using MarketHouse.Test.Data;
using System;
using System.Collections.Generic;
using System.Linq.Dynamic;
using System.Security.Cryptography.X509Certificates;
using System.Linq;
using TechTalk.SpecFlow.CommonModels;

namespace MarketHouse.Test.Core
{
    public class OrderCore
    {
        private IDbSessionManager sessionManager;
        public OrderCore(IDbSessionManager sessionManager)
        {
            this.sessionManager = sessionManager;
        }

        public OrderResult AcceptOrder(IEnumerable<Order> orders)
        {
            var orderResult = new OrderResult();            
            foreach (var order in orders)
            {
                var product = sessionManager.Query<Product>().Single(x => x.Code == order.Product);
                product.Quantity = product.Quantity - order.Quantity;

                sessionManager.SaveOrUpdate(product);

                if (product.Quantity < product.Threshold)
                    orderResult.AlertThresholds.Add(
                        new OrderResult.AlertThreshold
                        {
                            product = product.Code,
                            Quantity = product.Quantity,
                            Threshold = product.Threshold
                        });

            }

            return orderResult;
        }
    }
}
