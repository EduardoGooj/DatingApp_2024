using API.DataEntities;
using API.DTOs;
using Xunit;

namespace API.UnitTests.Tests
{
    public class PhotoResponseTests
    {
        [Fact]
        public void Should_SetPropertiesCorrectly_When_AssignedValues()
        {
            // Arrange
            var photo = new PhotoResponse
            {
                Id = 1,
                Url = "https://example.com/photo.jpg",
                IsMain = true
            };

            // Assert
            Assert.Equal(1, photo.Id);
            Assert.Equal("https://example.com/photo.jpg", photo.Url);
            Assert.True(photo.IsMain);
        }

        [Fact]
        public void Should_HaveDefaultValues_When_InstantiatedWithoutParameters()
        {
            // Arrange
            var photo = new PhotoResponse();

            // Assert
            Assert.Equal(0, photo.Id);             // Valor predeterminado para int
            Assert.Null(photo.Url);                // Valor predeterminado para string nullable
            Assert.False(photo.IsMain);            // Valor predeterminado para bool
        }

        [Fact]
        public void Should_AllowNullUrl_When_SetToNull()
        {
            // Arrange
            var photo = new PhotoResponse { Url = null };

            // Assert
            Assert.Null(photo.Url);
        }

        [Fact]
        public void Should_UpdateProperties_When_Modified()
        {
            // Arrange
            var photo = new PhotoResponse
            {
                Id = 1,
                Url = "https://example.com/photo.jpg",
                IsMain = true
            };

            // Act
            photo.Id = 2;
            photo.Url = "https://example.com/new-photo.jpg";
            photo.IsMain = false;

            // Assert
            Assert.Equal(2, photo.Id);
            Assert.Equal("https://example.com/new-photo.jpg", photo.Url);
            Assert.False(photo.IsMain);
        }
        [Fact]
        public void Should_Allow_Optional_PublicId()
        {
            // Arrange
            var photo = new Photo
            {
                Id = 1,
                Url = "http://example.com/photo1.jpg",
                IsMain = false,
                AppUserId = 2,
                AppUser = new AppUser { 
                    Id = 1, 
                    UserName = "arenita",
                    KnownAs = "Arenita",
                    Gender = "female",
                    City = "Aguascalientes",
                    Country = "Mexico"
                    }
            };

            // Assert
            Assert.Equal(1, photo.Id);
            Assert.Equal("http://example.com/photo1.jpg", photo.Url);
            Assert.False(photo.IsMain);
            Assert.Null(photo.PublicId);  // PublicId is optional
            Assert.Equal(2, photo.AppUserId);
            Assert.NotNull(photo.AppUser);
            Assert.Equal("arenita", photo.AppUser.UserName);
        }
    }
}
