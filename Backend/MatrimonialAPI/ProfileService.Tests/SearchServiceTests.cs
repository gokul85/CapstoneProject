using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using NUnit.Framework;
using ProfileService.Data;
using ProfileService.Exceptions;
using ProfileService.Interfaces;
using ProfileService.Models;
using ProfileService.Models.DTOs;
using ProfileService.Repositories;
using ProfileService.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProfileService.Tests
{
    [TestFixture]
    public class SearchServiceTests
    {
        private SearchService _service;
        private IRepository<int, BasicInfo> _basicInfoRepo;
        private IRepository<int, UserProfile> _userProfileRepo;
        private IRepository<int, Address> _addressRepo;
        private IRepository<int, Careers> _careersRepo;
        private IRepository<int, Educations> _educationsRepo;
        private IRepository<int, FamilyInfo> _familyInfoRepo;
        private IRepository<int, Lifestyle> _lifestyleRepo;
        private IRepository<int, PhysicalAttributes> _physicalAttrRepo;
        private IRepository<int, PartnerPreference> _partnerPrefRepo;
        private IRepository<int, ProfileImages> _galleryImagesRepo;
        private ProfileServiceDBContext _dbContext;

        [SetUp]
        public void SetUp()
        {
            var options = new DbContextOptionsBuilder<ProfileServiceDBContext>()
                .UseInMemoryDatabase(databaseName: "ProfileServiceTestDB")
                .Options;
            _dbContext = new ProfileServiceDBContext(options);
            _dbContext.Database.EnsureCreated();

            _basicInfoRepo = new BasicInfoRepository(_dbContext);
            _userProfileRepo = new UserProfileRepository(_dbContext);
            _addressRepo = new AddressRepository(_dbContext);
            _careersRepo = new CareersRepository(_dbContext);
            _educationsRepo = new EducationsRepository(_dbContext);
            _familyInfoRepo = new FamilyInfoRepository(_dbContext);
            _lifestyleRepo = new LifeStyleRepository(_dbContext);
            _physicalAttrRepo = new PhysicalAttributesRepository(_dbContext);
            _partnerPrefRepo = new PartnerPreferenceRepository(_dbContext);
            _galleryImagesRepo = new ProfileImagesRepository(_dbContext);

            _service = new SearchService(
                _basicInfoRepo,
                _userProfileRepo,
                _addressRepo,
                _careersRepo,
                _educationsRepo,
                _familyInfoRepo,
                _lifestyleRepo,
                _physicalAttrRepo,
                _partnerPrefRepo,
                _galleryImagesRepo);
        }

        [TearDown]
        public void TearDown()
        {
            _dbContext.Database.EnsureDeleted();
            _dbContext.Dispose();
        }

        [Test]
        public async Task SearchProfiles_WithPartnerPreference_ReturnsProfiles()
        {
            int userId = 1;
            var searchCriteria = new SearchCriteriaDTO { PP = true };

            var userProfile = new UserProfile
            {
                UserId = userId,
                BasicInfo = new BasicInfo { Gender = "Male", DOB = new DateTime(1990, 1, 1), Email = "test@gmail.com", Phone = "9876543210", FirstName = "Test", LastName = "Test" },
                ProfileFor = "Self"
            };
            await _userProfileRepo.Add(userProfile);

            var profile = new UserProfile
            {
                BasicInfo = new BasicInfo { Gender = "Female", DOB = new DateTime(1992, 1, 1), Email = "test2@gmail.com", Phone = "9876543201", FirstName = "Test2", LastName = "Test2" },
                ProfileCompleted = true,
                PhysicalAttribute = new PhysicalAttributes { Height = 160, Weight = 60, BloodGroup = "A+", Complextion = "Fair", EyeColor = "Black", HairColor = "Black" },
                Address = new Address { AddressLine = "Test", City = "Test City", State = "State" },
                Careers = new List<Careers> { new Careers { JobTitle = "Engineer", Company = "Test Company" } },
                ProfileImage = "Image",
                ProfileFor = "Self"
            };
            await _userProfileRepo.Add(profile);


            Assert.ThrowsAsync<UserProfileNotFoundException>(async () => await _service.SearchProfiles(searchCriteria, userId));
        }

        [Test]
        public async Task SearchProfiles_WithoutPartnerPreference_ReturnsProfiles()
        {
            int userId = 1;
            var searchCriteria = new SearchCriteriaDTO { PP = false };

            var userProfile = new UserProfile
            {
                UserId = userId,
                BasicInfo = new BasicInfo { Gender = "Male", DOB = new DateTime(1990, 1, 1), Email="test@gmail.com",Phone="9876543210",FirstName="Test",LastName="Test" },
                ProfileFor = "Self"
            };
            await _userProfileRepo.Add(userProfile);

            var profile = new UserProfile
            {
                BasicInfo = new BasicInfo { Gender = "Female", DOB = new DateTime(1992, 1, 1), Email = "test2@gmail.com", Phone = "9876543201", FirstName = "Test2", LastName = "Test2" },
                ProfileCompleted = true,
                PhysicalAttribute = new PhysicalAttributes { Height = 160, Weight = 60,BloodGroup="A+",Complextion="Fair",EyeColor="Black",HairColor="Black" },
                Address = new Address { AddressLine="Test",City="Test City",State = "State" },
                Careers = new List<Careers> { new Careers { JobTitle = "Engineer",Company="Test Company" } },
                ProfileImage = "Image",
                ProfileFor="Self"
            };
            await _userProfileRepo.Add(profile);

            Assert.ThrowsAsync<UserProfileNotFoundException>(async () => await _service.SearchProfiles(searchCriteria, userId));
        }

        [Test]
        public void SearchProfiles_UserProfileNotFound_ThrowsException()
        {
            int userId = 1;
            var searchCriteria = new SearchCriteriaDTO { PP = true };

            Assert.ThrowsAsync<UserProfileNotFoundException>(async () => await _service.SearchProfiles(searchCriteria, userId));
        }

        [Test]
        public async Task ViewProfile_ProfileExists_ReturnsProfile()
        {
            int profileId = 1;

            var profile = new UserProfile
            {
                Id = profileId,
                BasicInfo = new BasicInfo { FirstName = "John", LastName = "Doe", Gender = "Male",Email="tessjohn@gmail.com",Phone="9876543210", DOB = new DateTime(1990, 1, 1), Religion = "Christianity", MaritalStatus = "Single", Caste = "General", NativeLanguage = "English" },
                PhysicalAttribute = new PhysicalAttributes { Height = 180, Weight = 75, Complextion = "Fair",BloodGroup="A+",HairColor="Black",EyeColor="Black" },
                Address = new Address { AddressLine="Test",State = "State", City = "City" },
                FamilyInfo = new FamilyInfo { Father = true, Mother = true, Siblings = 2 },
                ProfileImage = "Image",
                Careers = new List<Careers> { new Careers { JobTitle = "Engineer",Company="Test Company" } },
                Educations = new List<Educations> { new Educations { Degree = "Bachelor's",Specialization="Tets",Status="Completed" } },
                LifeStyle = new Lifestyle { Drink = false, Smoke = false, LivingWith = "Parents" },
                ProfileFor = "Self"
            };
            await _userProfileRepo.Add(profile);

            var result = await _service.ViewProfile(profileId);

            Assert.IsNotNull(result);
            var viewProfileDTO = result.result as ViewProfileReturnDTO;
            Assert.IsNotNull(viewProfileDTO);
            Assert.AreEqual("John", viewProfileDTO.FirstName);
            Assert.AreEqual("Doe", viewProfileDTO.LastName);
            Assert.AreEqual("Male", viewProfileDTO.Gender);
        }

        [Test]
        public void ViewProfile_ProfileNotFound_ThrowsException()
        {
            int profileId = 1;

            Assert.ThrowsAsync<UserProfileNotFoundException>(async () => await _service.ViewProfile(profileId));
        }
    }
}
