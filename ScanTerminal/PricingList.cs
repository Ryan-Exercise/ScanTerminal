//  PricingBuilder.cs
//
// Description: 
//       <Describe here>
//  Author:
//       Ryan Xu(XuChunlei) <hitxcl@gmail.com>
//  Create at:
//       0:30:35 2/7/2021
//
//  Copyright (c) 2021 XuChunlei
using System;
using System.Collections.Generic;

namespace ScanTerminal
{
    public sealed class PricingList
    {
        private readonly IDictionary<string, decimal> _unitPrice = new Dictionary<string, decimal>();
        private readonly IDictionary<string, IBulkPriceHolder> _bulkPrice = new Dictionary<string, IBulkPriceHolder>();
        
        private PricingList()
        {
        }

        public static PricingList Create()
        {
            return new PricingList();
        }

        public PricingList AddProduct(Product product)
        {
            if(product == null)
            {
                throw new ArgumentNullException();
            }
            if (_unitPrice.ContainsKey(product.Code))
            {
                throw new InvalidOperationException($"Product {product.Code} has been added to Unit Price");
            }

            _unitPrice.Add(product.Code, product.Price);

            if (product is IBulkPriceHolder)
            {
                _bulkPrice.Add(product.Code, product as IBulkPriceHolder);
            }
            return this;
        }

        public decimal GetPrice(string code)
        {
            if(string.IsNullOrEmpty(code) || string.IsNullOrWhiteSpace(code))
            {
                throw new ArgumentException("Invalid product code");
            }
            decimal price;
            if(!_unitPrice.TryGetValue(code.Trim(), out price)) {
                throw new InvalidOperationException($"{code} product do not exist");
            }

            return price;
        }

        public IBulkPriceHolder GetBulkPrice(string code)
        {
            if (string.IsNullOrEmpty(code) || string.IsNullOrWhiteSpace(code))
            {
                throw new ArgumentException("Invalid product code");
            }
            IBulkPriceHolder holder = null;
            var validCode = code.Trim();
            if (_bulkPrice.ContainsKey(validCode)) {
                holder = _bulkPrice[validCode];
            }

            return holder;
        }
        
    }

    
}
