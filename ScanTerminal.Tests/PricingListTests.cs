//  PricingListTests.cs
//
// Description: 
//       <Describe here>
//  Author:
//       Ryan Xu(XuChunlei) <hitxcl@gmail.com>
//  Create at:
//       2:10:19 2/7/2021
//
//  Copyright (c) 2021 XuChunlei
using System;
using AutoFixture;
using Xunit;

namespace ScanTerminal.Tests
{
    public class PricingListTests
    {
        [Fact]
        public void Create_ReturnsPricingList()
        {
            // Arrange

            // Act
            var list = PricingList.Create();

            // Assert
            Assert.IsType<PricingList>(list);
        }

        [Fact]
        public void AddProduct_Null_ThrowsArgumentNullException()
        {
            // Arrange
            Product product = null;

            // Act
            Action actual = () => PricingList.Create()
                                  .AddProduct(product);

            // Assert
            Assert.Throws<ArgumentNullException>(actual);
        }

        [Fact]
        public void AddProduct_Duplicate_ThrowsInvalidOperationException()
        {
            // Arrange
            Fixture fixture = new Fixture();

            Product product = fixture.Create<Product>();

            // Act
            Action actual = () => PricingList.Create()
                                  .AddProduct(product)
                                  .AddProduct(product);

            // Assert
            var exception = Assert.Throws<InvalidOperationException>(actual);
            Assert.Equal($"Product {product.Code} has been added to Unit Price", exception.Message);
        }

        [Fact]
        public void AddProduct_Same_ThrowsInvalidOperationException()
        {
            // Arrange
            Fixture fixture = new Fixture();
            string code = fixture.Create<string>();
            decimal price1 = fixture.Create<decimal>();
            var product1 = Product.Create(code, price1);

            decimal price2 = fixture.Create<decimal>();
            int bulkCount = fixture.Create<int>();
            decimal bulkPrice = fixture.Create<decimal>();
            var product2 = BulkPriceProduct.Create(code, price2, bulkCount, bulkPrice);

            // Act
            Action actual = () => PricingList.Create()
                                  .AddProduct(product1)
                                  .AddProduct(product2);

            // Assert
            var exception = Assert.Throws<InvalidOperationException>(actual);
            Assert.Equal($"Product {code} has been added to Unit Price", exception.Message);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void GetPrice_InvalidProductCode_ThrowsArgumentException(string code)
        {
            // Arrange
            Fixture fixture = new();
            var pricingList = fixture.Create<PricingList>();

            // Act
            Action actual = () => pricingList.GetPrice(code);

            // Assert
            var exception = Assert.Throws<ArgumentException>(actual);
            Assert.Equal("Invalid product code", exception.Message);
            
        }

        [Fact]
        public void GetPrice_ProductNotExist_ThrowsInvalidOperationException()
        {
            // Arrange
            Fixture fixture = new();
            var pricingList = fixture.Create<PricingList>()
                                     .AddProduct(fixture.Create<Product>())
                                     .AddProduct(fixture.Create<BulkPriceProduct>());
            var product1 = fixture.Create<Product>();

            // Act
            Action actual = () => pricingList.GetPrice(product1.Code);

            // Assert
            var exception = Assert.Throws<InvalidOperationException>(actual);
            Assert.Equal($"{product1.Code} product do not exist", exception.Message);

        }

        [Theory]
        [InlineData("A")]
        [InlineData("A ")]
        [InlineData(" A")]
        [InlineData(" A ")]
        public void GetPrice_ReturnsUnitPrice(string code)
        {
            // Arrange
            Fixture fixture = new();

            var price = fixture.Create<decimal>();
            var productA = Product.Create(code, price);
            var pricingList = fixture.Create<PricingList>()
                                     .AddProduct(productA)
                                     .AddProduct(fixture.Create<Product>())
                                     .AddProduct(fixture.Create<BulkPriceProduct>());

            // Act
            var unitPrice = pricingList.GetPrice(code);

            // Assert
            Assert.Equal(price, unitPrice);

        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void GetBulkPrice_InvalidProductCode_ThrowsArgumentException(string code)
        {
            // Arrange
            Fixture fixture = new();
            var pricingList = fixture.Create<PricingList>();

            // Act
            Action actual = () => pricingList.GetBulkPrice(code);

            // Assert
            var exception = Assert.Throws<ArgumentException>(actual);
            Assert.Equal("Invalid product code", exception.Message);

        }

        [Theory]
        [InlineData("A")]
        [InlineData("A ")]
        [InlineData(" A")]
        [InlineData(" A ")]
        public void GetBulkPrice_ReturnsInstanceOfIBulkPriceHolder(string code)
        {
            // Arrange
            Fixture fixture = new();

            var price = fixture.Create<decimal>();
            var bulkCount = fixture.Create<int>();
            var bulkPrice = fixture.Create<decimal>();
            var productA = BulkPriceProduct.Create(code, price, bulkCount, bulkPrice);
            var pricingList = fixture.Create<PricingList>()
                                     .AddProduct(productA)
                                     .AddProduct(fixture.Create<Product>())
                                     .AddProduct(fixture.Create<BulkPriceProduct>());

            // Act
            var unitPrice = pricingList.GetBulkPrice(code);

            // Assert
            Assert.Equal(productA, unitPrice);

        }

        [Fact]
        public void GetBulkPrice_ReturnsNull()
        {
            // Arrange
            Fixture fixture = new();

            var product = fixture.Create<BulkPriceProduct>();
            
            var pricingList = fixture.Create<PricingList>()
                                     .AddProduct(fixture.Create<Product>())
                                     .AddProduct(fixture.Create<BulkPriceProduct>());

            // Act
            var unitPrice = pricingList.GetBulkPrice(product.Code);

            // Assert
            Assert.Null(unitPrice);

        }
    }
}
