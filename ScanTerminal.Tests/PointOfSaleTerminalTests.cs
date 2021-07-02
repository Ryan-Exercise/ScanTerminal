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
using AutoFixture;
using Xunit;

namespace ScanTerminal.Tests
{
    public class PointOfSaleTerminalTests
    {
        [Theory]
        [InlineData("ABCD", 4.60)]
        [InlineData("AAAA", 4.00)]
        [InlineData("ABBA", 4.20)]
        public void ScanProduct_WithoutBulkPrice_ComputeTotal(string cart, decimal total)
        {
            // Arrange
            var pricing = PricingList.Create()
                                     .AddProduct(Product.Create("A", 1.00m))
                                     .AddProduct(Product.Create("B", 1.10m))
                                     .AddProduct(Product.Create("C", 1.20m))
                                     .AddProduct(Product.Create("D", 1.30m));
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
