using System;
using System.Drawing;
using FluentAssertions;
using NUnit.Framework;
using ScaleImages.ImageScaling;

namespace ScaleImages.Tests;

[TestFixture]
public class ImageScalerTests
{
    [Test]
    public void when_scale_factor_is_out_of_range_then_throw_exception()
    {
        // Arrange:
        using var image = (Image)new Bitmap(2000, 3000);
        const decimal scaleFactor = -1;

        // Act & Assert:
        var imageScaler = new ImageScaler();
        Assert.Throws<ArgumentOutOfRangeException>(() => imageScaler.ScaleBy(image, scaleFactor));
    }

    [Test]
    public void when_scale_factor_equals_1_then_the_same_image_is_returned()
    {
        // Arrange:
        using var image = (Image)new Bitmap(2000, 3000);
        const decimal scaleFactor = 1;

        // Act:
        var imageScaler = new ImageScaler();
        var result = imageScaler.ScaleBy(image, scaleFactor);

        // Assert:
        ReferenceEquals(result, image).Should().BeTrue();
    }

    [Test]
    public void when_scale_factor_is_valid_but_not_1_then_new_scaled_image_is_returned()
    {
        // Arrange:
        using var image = (Image)new Bitmap(2000, 3000);
        const decimal scaleFactor = 0.5m;

        // Act:
        var imageScaler = new ImageScaler();
        var result = imageScaler.ScaleBy(image, scaleFactor);

        // Assert:
        ReferenceEquals(result, image).Should().BeFalse();
        result.Size.Width.Should().Be(1000);
        result.Size.Height.Should().Be(1500);
    }
}