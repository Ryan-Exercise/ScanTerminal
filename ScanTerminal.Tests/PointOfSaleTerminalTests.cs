//  PointOfSaleTerminalTests.cs
//
// Description: 
//       <Describe here>
//  Author:
//       Ryan Xu(XuChunlei) <hitxcl@gmail.com>
//  Create at:
//       12:38:0 2/7/2021
//
//  Copyright (c) 2021 XuChunlei
using System;
using System.Collections.Generic;
using AutoFixture;
using Xunit;

namespace ScanTerminal.Tests
{
    public class PointOfSaleTerminalTests
    {
        private readonly IReadOnlyList<Product> _products = new List<Product>()
        {
            Product.Create("A", 1.00m),
            BulkPriceProduct.Create("B", 1.10m, 2, 2.00m),
            Product.Create("C", 1.20m),
            BulkPriceProduct.Create("D", 1.30m, 3, 3.00m),
        }.AsReadOnly();

        [Theory]
        [InlineData("ABCD", 4.60)]
        [InlineData("AB C  D  ", 4.60)]
        [InlineData("", 0)]
        [InlineData("   ", 0)]
        [InlineData("ABCDD", 5.90)]
        [InlineData("BBBBB", 5.10)]
        [InlineData("BBDDD", 5.00)]
        [InlineData("BBBDDDD", 7.40)]
        [InlineData("ABABABCDCDCDCD", 15.20)]
        public void ScanProduct_ValidProduct_ComputeTotal(string cart, decimal total)
        {
            // Arrange
            var pricing = PricingList.Create();
            foreach(var p in _products)
            {
                pricing.AddProduct(p);
            }
            var terminal = new PointOfSaleTerminal(pricing);

            // Act
            foreach(var p in cart)
            {
                terminal.ScanProduct(p.ToString());
            }

            // Assert
            Assert.Equal(total, terminal.Total);
        }

        [Fact]
        public void ScanProduct_WithoutPricing_ThrowsNullReferenceException()
        {
            // Arrange
            Fixture fixture = new Fixture();
            const PricingList NULL_PRICING = null;
            var terminal = new PointOfSaleTerminal(NULL_PRICING);
            var code = fixture.Create<string>();

            // Act
            void actual() => terminal.ScanProduct(code);

            // Assert
            var exception = Assert.Throws<NullReferenceException>(actual);
            Assert.Equal("Please initialize Pricing first", exception.Message);
        }

        [Fact]
        public void ScanProduct_ProductNotExist_ThrowsInvalidOperationException()
        {
            // Arrange
            Fixture fixture = new Fixture();
            var price = fixture.Create<int>();
            var pricingList = fixture.Create<PricingList>()
                                     .AddProduct(Product.Create("A", price));
            
            var terminal = new PointOfSaleTerminal(pricingList);
            var code = fixture.Create<string>();

            // Act
            void actual() => terminal.ScanProduct(code);

            // Assert
            var exception = Assert.Throws<InvalidOperationException>(actual);
            Assert.Equal($"{code} product do not exist", exception.Message);
        }
    }
}
