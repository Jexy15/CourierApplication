using CourierProject.Domain.ValueObjects;

namespace CourierProject.Domain.Tests.ValueObjects;

public class WeightTests
{
    [Theory]
    [InlineData(0)]
    [InlineData(-10)]
    public void Constructor_WithInvalidKilograms_ThrowsArgumentOutOfRangeException(int kilograms)
    {
        //Arrange
        decimal kilo = kilograms;
        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => { var weight = new Weight(kilo); });
    }
    [Fact]
    public void Constructor_WithSmallestPositiveValue_IsAccepted()
    {
        //Arrange
        decimal kilogram = 0.001m;
        // Act
        var weight = new Weight(kilogram);
        //Assert
        Assert.Equal(0.001m, weight.Kilograms);
    }
    [Fact]
    public void Max_WhenRealIsHeavier_ReturnsReal()
    {
        //Arrange
        decimal realKilogram = 60m;
        decimal volumetricKilogram = 23m;

        var realWeight = new Weight(realKilogram);
        var volumetricWeight = new Weight(volumetricKilogram);
        //Act
        var max = Weight.Max(realWeight, volumetricWeight);
        //Assert
        Assert.Equal(60m, max.Kilograms);
    }

    [Fact]
    public void Max_WhenVolumetricIsHeavier_ReturnsVolumetric()
    {
        //Arrange
        decimal realKilogram = 15m;
        decimal volumetricKilogram = 23m;

        var realWeight = new Weight(realKilogram);
        var volumetricWeight = new Weight(volumetricKilogram);

        //Act
        var max = Weight.Max(realWeight, volumetricWeight);
        //Assert
        Assert.Equal(23m, max.Kilograms);
    }
    [Fact]
    public void Max_WhenWeightsAreEqual_ReturnsSameValue()
    {
        //Arrange
        decimal realKilogram = 15m;
        decimal volumetricKilogram = 15m;

        var realWeight = new Weight(realKilogram);
        var volumetricWeight = new Weight(volumetricKilogram);

        //Act
        var max = Weight.Max(realWeight, volumetricWeight);
        //Assert
        Assert.Equal(15m, max.Kilograms);
    }
}
