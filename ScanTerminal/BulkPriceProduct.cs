//  BulkPriceProduct.cs
//
// Description: 
//       <Describe here>
//  Author:
//       Ryan Xu(XuChunlei) <hitxcl@gmail.com>
//  Create at:
//       20:1:21 1/7/2021
//
//  Copyright (c) 2021 XuChunlei
using System;
namespace ScanTerminal
{
    public class BulkPriceProduct : Product, IBulkPriceHolder
    {
        private readonly int _bulkCount;
        private readonly decimal _bulkPrice;
        protected BulkPriceProduct(string code, decimal price, int bulkCount, decimal bulkPrice): base(code, price)
        {
            _bulkCount = bulkCount;
            _bulkPrice = Math.Round(bulkPrice, 2);
        }

        public int GetBulkCount()
        {
            return _bulkCount;
        }

        public decimal GetBulkPrice()
        {
            return _bulkPrice;
        }

        public static BulkPriceProduct Create(string code, decimal price, int bulkCount, decimal bulkPrice)
        {
            // verify bulk count
            if(bulkCount <= 1)
            {
                throw new ArgumentException("Bulk Count cannot be less than 1");
            }

            // verify bulk price
            if(bulkPrice < 0)
            {
                throw new ArgumentException("Bulk Price cannot be negative");
            }


            return new BulkPriceProduct(code, price, bulkCount, bulkPrice);
        }
    }
}
