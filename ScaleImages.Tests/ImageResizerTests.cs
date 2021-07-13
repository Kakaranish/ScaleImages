using FluentAssertions;
using NUnit.Framework;
using System;
using System.Drawing;
using ScaleImages.ImageResizing;

namespace ScaleImages.Tests
{
    [TestFixture]
    public class ImageResizerTests
    {
        #region DownscaleImage tests

        [Test]
        public void when_image_is_null_then_exception_is_thrown()
        {
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

        #endregion

        #region ResizeImageByWidth

        [Test]
        public void when_image_is_null_while_resizing_by_width_then_exception_is_thrown()
        {
            // Act & Assert:
            var imageResizer = new ImageResizer();
            Assert.Throws<ArgumentNullException>(() => imageResizer.ResizeImageByWidth(null, 2000));
        }

        [Test]
        public void when_width_is_out_of_range_while_resizing_by_width_then_exception_is_thrown()
        {
            // Arrange:
            using var image = (Image)new Bitmap(2000, 3000);
            var destinationWidth = 0;

            // Act & Assert:
            var imageResizer = new ImageResizer();
            Assert.Throws<ArgumentOutOfRangeException>(() => 
                imageResizer.ResizeImageByWidth(image, destinationWidth));
        }

        [Test]
        public void when_scale_factor_is_1_while_resizing_by_width_then_the_same_image_is_returned()
        {
            // Arrange:
            using var image = (Image)new Bitmap(2000, 3000);
            var destinationWidth = 2000;

            // Act:
            var imageResizer = new ImageResizer();
            var result = imageResizer.ResizeImageByWidth(image, destinationWidth);

            // Assert:
            ReferenceEquals(result, image).Should().BeTrue();
            result.Size.Width.Should().Be(2000);
            result.Size.Height.Should().Be(3000);
        }

        [Test]
        public void when_scale_factor_is_valid_but_not_1_while_resizing_by_width_then_new_scaled_image_is_returned()
        {
            // Arrange:
            using var image = (Image)new Bitmap(2000, 3000);
            var destinationWidth = 1000;

            // Act:
            var imageResizer = new ImageResizer();
            var result = imageResizer.ResizeImageByWidth(image, destinationWidth);

            // Assert:
            ReferenceEquals(result, image).Should().BeFalse();
            result.Size.Width.Should().Be(1000);
            result.Size.Height.Should().Be(1500);
        }

        #endregion

        #region ResizeByHeight tests

        [Test]
        public void when_image_is_null_while_resizing_by_height_then_exception_is_thrown()
        {
            // Act & Assert:
            var imageResizer = new ImageResizer();
            Assert.Throws<ArgumentNullException>(() => imageResizer.ResizeImageByHeight(null, 2000));
        }

        [Test]
        public void when_height_is_out_of_range_while_resizing_by_height_then_exception_is_thrown()
        {
            // Arrange:
            using var image = (Image)new Bitmap(2000, 3000);
            var destinationHeight = 0;

            // Act & Assert:
            var imageResizer = new ImageResizer();
            Assert.Throws<ArgumentOutOfRangeException>(() =>
                imageResizer.ResizeImageByHeight(image, destinationHeight));
        }

        [Test]
        public void when_scale_factor_is_valid_but_not_1_while_resizing_by_height_then_new_scaled_image_is_returned()
        {
            // Arrange:
            using var image = (Image)new Bitmap(2000, 3000);
            var destinationHeight = 750;

            // Act:
            var imageResizer = new ImageResizer();
            var result = imageResizer.ResizeImageByHeight(image, destinationHeight);

            // Assert:
            ReferenceEquals(result, image).Should().BeFalse();
            result.Size.Width.Should().Be(500);
            result.Size.Height.Should().Be(750);
        }

        #endregion
    }
}