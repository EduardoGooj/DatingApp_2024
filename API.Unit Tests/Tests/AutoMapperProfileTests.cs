using System;
using System.Collections.Generic;
using System.Linq;
using API.DataEntities;
using API.DTOs;
using API.Helpers;
using AutoMapper;
using Xunit;

namespace API.UnitTests.Tests
{
    public class AutoMapperProfilesTests
    {
        private readonly IMapper _mapper;

        public AutoMapperProfilesTests()
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<AutoMapperProfiles>());
            _mapper = config.CreateMapper();
        }

        [Fact]
        public void AutoMapperProfiles_ShouldMap_AppUserToMemberResponse_Correctly()
        {
            // Arrange
            var appUser = new AppUser
            {
                Id = 1,
                UserName = "johndoe",
                Gender ="Male",
                KnownAs="john",
                Country= "USA",
                City="Texas",
                Photos = new List<Photo>
                {
                    new Photo { Id = 1, Url = "http://example.com/photo1.jpg", IsMain = false },
                    new Photo { Id = 2, Url = "http://example.com/photo2.jpg", IsMain = true }
                }
            };
            
            var expectedAge = DateTime.Today.Year - appUser.BirthDay.Year;
            var expectedPhotoUrl = "http://example.com/photo2.jpg"; // URL de la foto principal

            // Act
            var memberResponse = _mapper.Map<MemberResponse>(appUser);

            // Assert
            Assert.Equal(appUser.Id, memberResponse.Id);
            Assert.Equal(appUser.UserName, memberResponse.UserName);
            Assert.Equal(expectedAge, memberResponse.Age);
            Assert.Equal(expectedPhotoUrl, memberResponse.PhotoUrl);
        }

        [Fact]
        public void AutoMapperProfiles_ShouldMap_PhotoToPhotoResponse_Correctly()
        {
            // Arrange
            var photo = new Photo
            {
                Id = 1,
                Url = "http://example.com/photo1.jpg",
                IsMain = true
            };

            // Act
            var photoResponse = _mapper.Map<PhotoResponse>(photo);

            // Assert
            Assert.Equal(photo.Url, photoResponse.Url);
            Assert.Equal(photo.IsMain, photoResponse.IsMain);
        }

        [Fact]
        public void AutoMapperProfiles_ShouldMap_MemberUpdateRequestToAppUser_Correctly()
        {
            // Arrange
            var memberUpdateRequest = new MemberUpdateRequest
            {
                Introduction = "Hello, I'm John.",
                LookingFor = "Friendship",
                Interests = "Coding, Music",
                City = "New York",
                Country = "USA"
            };

            // Act
            var appUser = _mapper.Map<AppUser>(memberUpdateRequest);

            // Assert
            Assert.Equal(memberUpdateRequest.Introduction, appUser.Introduction);
            Assert.Equal(memberUpdateRequest.LookingFor, appUser.LookingFor);
            Assert.Equal(memberUpdateRequest.Interests, appUser.Interests);
            Assert.Equal(memberUpdateRequest.City, appUser.City);
            Assert.Equal(memberUpdateRequest.Country, appUser.Country);
        }
    }
}
