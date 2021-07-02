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
        private IDictionary<string, int> _scannedWithoutBulk = new Dictionary<string, int>();
        private bool _illegal = false;

        public PricingList Pricing { get; set; }
        public decimal Total { get; private set; }
        public PointOfSaleTerminal(PricingList list)
        {
            Pricing = list;
            Total = 0;
        }


        public void ScanProduct(string code)
        {
            if (_illegal)
            {
                throw new InvalidOperationException("Illegal sate: please reset Scan Terminal");
            }
            if(string.IsNullOrEmpty(code) || string.IsNullOrWhiteSpace(code))
            {
                return;
            }
            if (Pricing == null)
            {
                throw new NullReferenceException("Please initialize Pricing first");
            }

            decimal price = -1m;
            try
            {
                price = Pricing.GetPrice(code);
            }
            catch
            {
                _illegal = true;
                Total = 0;
                _scannedWithoutBulk.Clear();
                throw;
            }
            
            
            var bulkPrice = Pricing.GetBulkPrice(code);

            int count;
            if(_scannedWithoutBulk.TryGetValue(code, out count))
            {
                count++;
                _scannedWithoutBulk[code] = count;
                Total += price;
                if (bulkPrice != null)
                {
                    if(count == bulkPrice.GetBulkCount())
                    {
                        Total = Total + bulkPrice.GetBulkPrice() - bulkPrice.GetBulkCount() * price;
                        _scannedWithoutBulk.Remove(code);
                       
                    }
                }
            }
            else
            {
                _scannedWithoutBulk.Add(code, 1);
                Total += price;
            }
            
        }

        public void Reset()
        {
            Total = 0;
            _scannedWithoutBulk.Clear();
            _illegal = false;
        }

    }
}
