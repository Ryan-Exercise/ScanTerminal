//  PointOfSaleTerminal.cs
//
// Description: 
//       <Describe here>
//  Author:
//       Ryan Xu(XuChunlei) <hitxcl@gmail.com>
//  Create at:
//       0:21:25 2/7/2021
//
//  Copyright (c) 2021 XuChunlei
using System;
using System.Collections.Generic;

namespace ScanTerminal
{
    public class PointOfSaleTerminal
    {
        public PricingList Pricing { get; set; }
        private IDictionary<string, int> _scannedWithoutBulk = new Dictionary<string, int>();
        public decimal Total { get; private set; }
        public PointOfSaleTerminal(PricingList list)
        {
            Pricing = list;
        }


        public void ScanProduct(string code)
        {
            if (Pricing == null)
            {
                throw new NullReferenceException("Please initialize Pricing first");
            }

            var price = Pricing.GetPrice(code);
            
            var bulkPrice = Pricing.GetBulkPrice(code);

            int count;
            if(_scannedWithoutBulk.TryGetValue(code, out count))
            {
                count++;
                if(bulkPrice != null)
                {
                    if(count == bulkPrice.GetBulkCount())
                    {
                        Total = Total + bulkPrice.GetBulkPrice() - bulkPrice.GetBulkCount() * price;
                        _scannedWithoutBulk.Remove(code);
                       
                    }
                }
                _scannedWithoutBulk[code] = count;
                Total += price;
            }
            else
            {
                _scannedWithoutBulk.Add(code, 1);
                Total += price;
            }
            
        }

    }
}
