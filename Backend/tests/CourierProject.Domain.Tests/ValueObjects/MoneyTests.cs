using CourierProject.Domain.ValueObjects;

namespace CourierProject.Domain.Tests.ValueObjects;

public class MoneyTests
{
    [Fact]
    public void Constructor_WithLowercaseCurrency_NormalizesToUppercase()
    {
        // Arrange
        var amount = 10m;
        var currency = "eur";

        // Act
        var money = new Money(amount, currency);

        // Assert
        Assert.Equal("EUR", money.Currency);
    }


    [Fact]
    public void Constructor_WithNegativeAmount_ThrowsArgumentOutOfRangeException()
    {
        // Arrange
        var amount = -10m;
        var currency = "EUR";

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => { var money = new Money(amount, currency); });
    }

    [Theory]
    [InlineData("EU")]
    [InlineData("EURO")]
    [InlineData("")]
    [InlineData("    ")]
    public void Constructor_WithInvalidCurrency_ThrowsArgumentException(string currency)
    {
        // Arrange
        var amount = 10m;

        // Act & Assert
        Assert.Throws<ArgumentException>(() => { var money = new Money(amount, currency); });
    }

    [Fact]
    public void Constructor_WithZeroAmount_IsAccepted()
    {
        // Arrange
        var amount = 0m;
        var currency = "EUR";

        // Act
        var money = new Money(amount, currency);

        // Assert
        Assert.Equal(0m, money.Amount);

    }

    [Fact]
    public void Constructor_WithSurroundingSpacesInCurrency_TrimsCurrency()
    {
        // Arrange
        var amount = 10m;
        var currency = " EUR ";

        // Act
        var money = new Money(amount, currency);

        // Assert
        Assert.Equal("EUR", money.Currency);

    }

    [Fact]
    public void Equality_WithSameAmountAndCurrency_AreEqual()
    {
        // Arrange
        var amount = 10m;
        var currency = "EUR";

        var amount1 = 10m;
        var currency1 = "EUR";
        // Act
        var money = new Money(amount, currency);
        var money1 = new Money(amount1, currency1);

        // Assert
        Assert.Equal(money, money1);
        Assert.Equal(money1, money);
    }
    [Fact]
    public void Equality_WithDifferentAmount_AreNotEqual()
    {
        // Arrange
        var amount = 10m;
        var currency = "EUR";

        var amount1 = 15m;
        var currency1 = "EUR";
        // Act
        var money = new Money(amount, currency);
        var money1 = new Money(amount1, currency1);

        // Assert
        Assert.NotEqual(money, money1);
        Assert.NotEqual(money1, money);
    }

    [Fact]
    public void Add_WithSameCurrency_ReturnsSum()
    {
        // Arrange
        var amount = 10m;
        var currency = "EUR";

        var amount1 = 15m;
        var currency1 = "EUR";
        var money = new Money(amount, currency);

        var money1 = new Money(amount1, currency1);
        // Act

        var addMoney = money.Add(money1);

        // Assert
        Assert.Equal(25m, addMoney.Amount);
        Assert.Equal("EUR", addMoney.Currency);

    }
    [Fact]
    public void Add_WithDifferentCurrency_ThrowsInvalidOperationException()
    {
        // Arrange
        var amount = 10m;
        var currency = "EUR";

        var amount1 = 15m;
        var currency1 = "USD";

        var money = new Money(amount, currency);
        var money1 = new Money(amount1, currency1);

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() =>
        {

            var sum = money.Add(money1);
        });
    }
}
