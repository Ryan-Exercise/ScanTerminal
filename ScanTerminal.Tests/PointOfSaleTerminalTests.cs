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
            BulkPriceProduct.Create("A", 1.25m, 3, 3.00m),
            Product.Create("B", 4.25m),
            BulkPriceProduct.Create("C", 1.00m, 6, 5.00m),
            Product.Create("D", 0.75m),
        }.AsReadOnly();

        [Theory]
        [InlineData("ABCD", 7.25)]
        [InlineData("CCCCCCC", 6)]
        [InlineData("ABCDABA", 13.25)]
        [InlineData("AB C  D  ", 7.25)] // including whitespace
        [InlineData("", 0)]             // empty
        [InlineData("   ", 0)]          // all whitespace
        [InlineData("AABCDCCCC", 12.5)] // all codes without bulk
        [InlineData("AABCDACCCCC", 13)] // all codes with bulk
        [InlineData("AAACCCCCC", 8)]    // bulk only
        [InlineData("AABCCDAABCCDCCC", 20.25)] // all codes with unit & bulk
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

        [Theory]
        [InlineData("aBCD")]
        [InlineData("ABCd")]
        public void ScanProduct_ProductNotExist_SetTotalToZero(string cart)
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
                try
                {
                    terminal.ScanProduct(p.ToString());
                }
                catch
                {
                    break;
                }
                
            }

            // Assert
            Assert.Equal(0, terminal.Total);
        }

        [Theory]
        [InlineData("aBCD")]
        [InlineData("ABCd")]
        public void Reset_When_ProductNotExist_BecomeAvilable(string cart)
        {
            // Arrange
            var pricing = PricingList.Create();
            foreach(var p in _products)
            {
                pricing.AddProduct(p);
            }
            var terminal = new PointOfSaleTerminal(pricing);
            const string LEGAL_CART = "ABCD";
            const decimal LEGAL_TOTAL = 7.25m;
            
            // Act
            foreach(var p in cart)
            {
                try
                {
                    terminal.ScanProduct(p.ToString());
                }
                catch
                {
                    terminal.Reset();
                    break;
                }
            }

            foreach(var p in LEGAL_CART)
            {
                terminal.ScanProduct(p.ToString());
            }

            // Assert
            Assert.Equal(LEGAL_TOTAL, terminal.Total);
        }
    }
}
