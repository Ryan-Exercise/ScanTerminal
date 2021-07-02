//  BulkPriceProductTests.cs
//
// Description: 
//       <Describe here>
//  Author:
//       Ryan Xu(XuChunlei) <hitxcl@gmail.com>
//  Create at:
//       22:47:17 1/7/2021
//
//  Copyright (c) 2021 XuChunlei
using System;
using AutoFixture;
using Xunit;

namespace ScanTerminal.Tests
{
    public class BulkPriceProductTests
    {
        [Theory]
        [InlineData(1)]
        [InlineData(0)]
        [InlineData(-1)]
        public void Create_WithNegativeOrZeroBulkCount_ThrowsArgumentException(int bulkCount)
        {
            // Arrange
            Fixture fixture = new Fixture();
            var code = fixture.Create<string>();
            var price = fixture.Create<decimal>();
            var bulkPrice = fixture.Create<decimal>();

            // Act
            Action actual = () => BulkPriceProduct.Create(code, price, bulkCount, bulkPrice);

            // Assert
            var exception = Assert.Throws<ArgumentException>(actual);
            Assert.Equal("Bulk Count cannot be less than 1", exception.Message);

        }

        [Theory]
        [InlineData(-1.11)]
        public void Create_WithNegativeBulkPrice_ThrowsArgumentException(decimal bulkPrice)
        {
            // Arrange
            Fixture fixture = new Fixture();
            var code = fixture.Create<string>();
            var price = fixture.Create<decimal>();
            var bulkCount = fixture.Create<int>();

            // Act
            Action actual = () => BulkPriceProduct.Create(code, price, bulkCount, bulkPrice);

            // Assert
            var exception = Assert.Throws<ArgumentException>(actual);
            Assert.Equal("Bulk Price cannot be negative", exception.Message);

        }

        [Theory]
        [InlineData(2, 0)]
        [InlineData(2, 1)]
        [InlineData(2, 1.1)]
        [InlineData(2, 1.111)]
        public void Create_ReturnsInstanceOfBulkPriceProduct(int bulkCount, decimal bulkPrice)
        {
            // Arrange
            Fixture fixture = new Fixture();
            var code = fixture.Create<string>();
            var price = fixture.Create<decimal>();

            // Act
            var product = BulkPriceProduct.Create(code, price, bulkCount, bulkPrice);

            // Assert
            Assert.Equal(Math.Round(bulkPrice, 2), product.GetBulkPrice());
            Assert.Equal(bulkCount, product.GetBulkCount());
        }
    }
}
