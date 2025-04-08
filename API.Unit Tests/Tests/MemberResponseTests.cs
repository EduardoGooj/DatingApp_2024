using System;
using System.Collections.Generic;
using API.DTOs;
using Xunit;

namespace API.UnitTests.Tests
{
    public class MemberResponseTests
    {
        [Fact]
        public void MemberResponse_ShouldInitializePropertiesCorrectly()
        {
            // Arrange
            var expectedId = 1;
            var expectedUserName = "johndoe";
            var expectedAge = 30;
            var expectedPhotoUrl = "http://example.com/photo.jpg";
            var expectedKnownAs = "John";
            var expectedCreated = DateTime.Now.AddDays(-30);
            var expectedLastActive = DateTime.Now;
            var expectedGender = "Male";
            var expectedIntroduction = "Hello, I'm John.";
            var expectedInterests = "Coding, Music";
            var expectedLookingFor = "Friendship";
            var expectedCity = "New York";
            var expectedCountry = "USA";
            var expectedPhotos = new List<PhotoResponse>
            {
                new PhotoResponse { Url = "http://example.com/photo1.jpg" },
                new PhotoResponse { Url = "http://example.com/photo2.jpg" }
            };

            // Act
            var member = new MemberResponse
            {
                Id = expectedId,
                UserName = expectedUserName,
                Age = expectedAge,
                PhotoUrl = expectedPhotoUrl,
                KnownAs = expectedKnownAs,
                Created = expectedCreated,
                LastActive = expectedLastActive,
                Gender = expectedGender,
                Introduction = expectedIntroduction,
                Interests = expectedInterests,
                LookingFor = expectedLookingFor,
                City = expectedCity,
                Country = expectedCountry,
                Photos = expectedPhotos
            };

            // Assert
            Assert.Equal(expectedId, member.Id);
            Assert.Equal(expectedUserName, member.UserName);
            Assert.Equal(expectedAge, member.Age);
            Assert.Equal(expectedPhotoUrl, member.PhotoUrl);
            Assert.Equal(expectedKnownAs, member.KnownAs);
            Assert.Equal(expectedCreated, member.Created);
            Assert.Equal(expectedLastActive, member.LastActive);
            Assert.Equal(expectedGender, member.Gender);
            Assert.Equal(expectedIntroduction, member.Introduction);
            Assert.Equal(expectedInterests, member.Interests);
            Assert.Equal(expectedLookingFor, member.LookingFor);
            Assert.Equal(expectedCity, member.City);
            Assert.Equal(expectedCountry, member.Country);
            Assert.Equal(expectedPhotos, member.Photos);
        }

        [Fact]
        public void MemberResponse_ShouldAllowNullProperties()
        {
            // Act
            var member = new MemberResponse
            {
                UserName = null,
                PhotoUrl = null,
                KnownAs = null,
                Gender = null,
                Introduction = null,
                Interests = null,
                LookingFor = null,
                City = null,
                Country = null,
                Photos = null
            };

            // Assert
            Assert.Null(member.UserName);
            Assert.Null(member.PhotoUrl);
            Assert.Null(member.KnownAs);
            Assert.Null(member.Gender);
            Assert.Null(member.Introduction);
            Assert.Null(member.Interests);
            Assert.Null(member.LookingFor);
            Assert.Null(member.City);
            Assert.Null(member.Country);
            Assert.Null(member.Photos);
        }
    }
}
