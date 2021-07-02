using System;
using AutoFixture;
using AutoFixture.Xunit2;
using Xunit;

namespace ScanTerminal.Tests
{
    public class ProductTests
    {
        [Theory]
        [InlineData(1)]
        [InlineData(1.11)]
        [InlineData(1.111)]
        public void Create_ReturnsInstanceOfProductWithValidPrice(decimal price)
        {
            // Arrange
            const string CODE = "A";

            // Act
            var product = Product.Create(CODE, price);

            // Assert
            Assert.Equal(Math.Round(price, 2), product.Price);

        }

        [Theory]
        [InlineData("A")]
        [InlineData(" A")]
        [InlineData(" A ")]
        [InlineData(" A\n\n")]
        [InlineData("5")]
        public void Create_ReturnsInstanceOfProductWithValidCode(string code)
        {
            // Arrange
            Fixture fixture = new Fixture();
            var price = fixture.Create<decimal>();

            // Act
            var product = Product.Create(code, price);
            
            // Assert
            Assert.Equal(code.Trim(), product.Code);

        }

        [Fact]
        public void Create_WithNegativePrice_ThrowsArgumentException()
        {
            // Arrange
            const string CODE = "A";
            const decimal NEGATIVE_PRICE = -1.00m;

            // Act
            Action actual = () => Product.Create(CODE, NEGATIVE_PRICE);

            // Assert
            var exception = Assert.Throws<ArgumentException>(actual);
            Assert.Equal("Price cannot be negative", exception.Message);

        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public void Create_WithEmptyOrNullCode_ThrowsArgumentException(string code)
        {
            // Arrange
            Fixture fixture = new Fixture();
            
            var price = fixture.Create<decimal>();

            // Act
            Action actual = () => Product.Create(code, price);

            // Assert
            var exception = Assert.Throws<ArgumentException>(actual);
            Assert.Equal("Code cannot be empty or null", exception.Message);

        }

        [Theory]
        [InlineData("AA")]
        [InlineData("A\nA")]
        [InlineData("00")]
        public void Create_WithCodeMoreThanOneCharacter_ThrowsArgumentException(string code)
        {
            // Arrange
            Fixture fixture = new();

            var price = fixture.Create<decimal>();

            // Act
            Action actual = () => Product.Create(code, price);

            // Assert
            var exception = Assert.Throws<ArgumentException>(actual);
            Assert.Equal("Code can only contain 1 character", exception.Message);

        }

        [Theory]
        [InlineData("#")]
        [InlineData("\n#")]
        [InlineData("\n#\n")]
        public void Create_WithInvalidCode_ThrowsArgumentException(string code)
        {
            // Arrange
            Fixture fixture = new();

            var price = fixture.Create<decimal>();

            // Act
            Action actual = () => Product.Create(code, price);

            // Assert
            var exception = Assert.Throws<ArgumentException>(actual);
            Assert.Equal("Code can only be single number or letter", exception.Message);

        }


        //[Theory]
        //[InlineData("A", "a")]
        //[InlineData("A", "B")]
        //public void Equals_InstanceOfProduct_ReturnsFalse(string code1, string code2)
        //{
        //    // Arrange
        //    Fixture fixture = new Fixture();
        //    var price = fixture.Create<decimal>();
        //    var product1 = Product.Create(code1, price);
        //    var product2 = Product.Create(code2, price);

        //    // Act
        //    var comparison = product1.Equals(product2);

        //    // Assert
        //    Assert.False(comparison);
        //}

        //[Fact]
        //public void Equals_Null_ReturnsFalse()
        //{
        //    // Arrange
        //    Fixture fixture = new Fixture();
        //    var product = fixture.Create<Product>();
        //    Product NULL_PRODUCT = null;

        //    // Act
        //    var comparison = product.Equals(NULL_PRODUCT);

        //    // Assert
        //    Assert.False(comparison);
        //}

        //[Theory]
        //[InlineData("A", "A")]
        //[InlineData("A", " A ")]
        //[InlineData("A", "A ")]
        //[InlineData("A", " A")]
        //public void Equals_InstanceOfProduct_ReturnsTrue(string code1, string code2)
        //{
        //    // Arrange
        //    Fixture fixture = new Fixture();
        //    var price1 = fixture.Create<decimal>();
        //    var product1 = Product.Create(code1, price1);

        //    var price2 = fixture.Create<decimal>();
        //    var product2 = Product.Create(code2, price2);

        //    // Act
        //    var comparison = product1.Equals(product2);

        //    // Assert
        //    Assert.True(comparison);
        //}

        //[Theory]
        //[InlineData("A", "a")]
        //[InlineData("A", "B")]
        //public void Equals_InstanceOfBulkPriceProduct_ReturnsFalse(string code1, string code2)
        //{
        //    // Arrange
        //    Fixture fixture = new Fixture();
        //    var price = fixture.Create<decimal>();

        //    var product1 = Product.Create(code1, price);

        //    var bulkCount = fixture.Create<int>();
        //    var bulkPrice = fixture.Create<decimal>();
        //    var product2 = BulkPriceProduct.Create(code2, price, bulkCount, bulkPrice);

        //    // Act
        //    var comparison = product1.Equals(product2);

        //    // Assert
        //    Assert.False(comparison);
        //}

        //[Theory]
        //[InlineData("A", "A")]
        //[InlineData("A", " A ")]
        //[InlineData("A", "A ")]
        //[InlineData("A", " A")]
        //public void Equals_InstanceOfBulkPriceProduct_ReturnsTrue(string code1, string code2)
        //{
        //    // Arrange
        //    Fixture fixture = new Fixture();

        //    var price1 = fixture.Create<decimal>();
        //    var product1 = Product.Create(code1, price1);

        //    var price2 = fixture.Create<decimal>();
        //    var bulkCount = fixture.Create<int>();
        //    var bulkPrice = fixture.Create<decimal>();
        //    var product2 = BulkPriceProduct.Create(code2, price2, bulkCount, bulkPrice);

        //    // Act
        //    var comparison = product1.Equals(product2);

        //    // Assert
        //    Assert.True(comparison);
        //}
    }
}
