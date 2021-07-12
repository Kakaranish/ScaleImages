using FluentAssertions;
using NUnit.Framework;
using System;
using System.Drawing;

namespace ScaleImages.Tests
{
    [TestFixture]
    public class ImageResizerTests
    {
        [Test]
        public void when_image_is_null_then_exception_is_thrown()
        {
            // Arrange:
            using var image = (Image)new Bitmap(2000, 3000);

            // Act & Assert:
            var imageResizer = new ImageResizer();
            Assert.Throws<ArgumentNullException>(() => imageResizer.DownscaleImage(null, 1));
        }

        [Test]
        public void when_downscaleTimes_is_out_of_range_then_exception_is_thrown()
        {
            // Arrange:
            using var image = (Image)new Bitmap(2000, 3000);
            var invalidDownscaleTimes = 0.5m;

            // Act:
            var imageResizer = new ImageResizer();
            Assert.Throws<ArgumentOutOfRangeException>(() => imageResizer.DownscaleImage(image, invalidDownscaleTimes));
        }

        [Test]
        public void when_downscaleTimes_equals_to_1_then_the_same_object_is_returned()
        {
            // Arrange:
            using var image = (Image)new Bitmap(2000, 3000);

            // Act:
            var imageResizer = new ImageResizer();
            var result = imageResizer.DownscaleImage(image, 1);

            // Assert:
            ReferenceEquals(result, image).Should().BeTrue();
        }

        [Test]
        public void when_downscaleTimes_has_valid_value_then_image_is_resized()
        {
            // Arrange:
            using var image = (Image)new Bitmap(2000, 3000);
            var downscaleTimes = 2;

            // Act:
            var imageResizer = new ImageResizer();
            var result = imageResizer.DownscaleImage(image, downscaleTimes);

            // Assert:
            ReferenceEquals(result, image).Should().BeFalse();
            result.Size.Width.Should().Be(1000);
            result.Size.Height.Should().Be(1500);
        }
    }
}