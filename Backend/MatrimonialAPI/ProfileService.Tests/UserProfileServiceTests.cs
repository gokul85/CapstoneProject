using Azure.Storage.Blobs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using NUnit.Framework;
using ProfileService.AsyncDataServices;
using ProfileService.Data;
using ProfileService.Exceptions;
using ProfileService.Interfaces;
using ProfileService.Models;
using ProfileService.Models.DTOs;
using ProfileService.Repositories;
using ProfileService.Services;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace ProfileService.Tests
{
    [TestFixture]
    public class UserProfileServiceTests
    {
        private UserProfileService _service;
        private IRepository<int, BasicInfo> _basicInfoRepo;
        private IRepository<int, UserProfile> _userProfileRepo;
        private IRepository<int, Address> _addressRepo;
        private IRepository<int, Careers> _careersRepo;
        private IRepository<int, Educations> _educationsRepo;
        private IRepository<int, FamilyInfo> _familyInfoRepo;
        private IRepository<int, Lifestyle> _lifestyleRepo;
        private IRepository<int, PhysicalAttributes> _physicalAttRepo;
        private IRepository<int, PartnerPreference> _partnerPrefRepo;
        private IRepository<int, ProfileImages> _galleryImagesRepo;
        private FileUploadService _fileUploadService;
        private RabbitMQPublisher _rabbitMQPublisher;
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
            _physicalAttRepo = new PhysicalAttributesRepository(_dbContext);
            _partnerPrefRepo = new PartnerPreferenceRepository(_dbContext);
            _galleryImagesRepo = new ProfileImagesRepository(_dbContext);

            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string>
                {
                    { "RabbitMQ:Host", "localhost" },
                    { "RabbitMQ:ProfileQueueName", "test_profile_queue" }
                })
                .Build();
            _rabbitMQPublisher = new RabbitMQPublisher(configuration);

            _fileUploadService = new FileUploadService(new BlobServiceClient("BlobEndpoint=https://matrimonialapi.blob.core.windows.net/;QueueEndpoint=https://matrimonialapi.queue.core.windows.net/;FileEndpoint=https://matrimonialapi.file.core.windows.net/;TableEndpoint=https://matrimonialapi.table.core.windows.net/;SharedAccessSignature=sv=2022-11-02&ss=bfqt&srt=sco&sp=rwdlacupiytfx&se=2024-08-30T15:14:44Z&st=2024-07-30T07:14:44Z&spr=https,http&sig=BY5nDjGhKZMV3%2B871c25XfhcdYfPeAxixelniw9PtsQ%3D")); // Assuming it has an empty constructor


            _service = new UserProfileService(
                _basicInfoRepo,
                _userProfileRepo,
                _addressRepo,
                _careersRepo,
                _educationsRepo,
                _familyInfoRepo,
                _lifestyleRepo,
                _physicalAttRepo,
                _partnerPrefRepo,
                _fileUploadService,
                _rabbitMQPublisher,
                _galleryImagesRepo);
        }

        [TearDown]
        public void TearDown()
        {
            _dbContext.Database.EnsureDeleted();
            _dbContext.Dispose();
        }

        [Test]
        public async Task AddUserProfile_UserProfileNotFound_ThrowsException()
        {
            var addProfileDTO = new AddUserProfileDTO { UserId = 1 };
            Assert.ThrowsAsync<UserProfileNotFoundException>(async () => await _service.AddUserProfile(addProfileDTO));
        }

        [Test]
        public async Task AddUserProfile_ValidData_AddsProfile()
        {
            var basicInfo = new BasicInfo { FirstName = "John", LastName = "Doe",Email="john.doe@gmail.com",Phone="9873216540",Gender="Male" };
            await _basicInfoRepo.Add(basicInfo);

            var userProfile = new UserProfile { UserId = 1, BasicInfoId = basicInfo.Id, ProfileFor="Self" };
            await _userProfileRepo.Add(userProfile);

            var addProfileDTO = new AddUserProfileDTO
            {
                UserId = 1,
                Bio = "Test Bio",
                NativeLanguage = "English",
                MaritalStatus = "Single",
                Religion = "None",
                Caste = "None",
                AddressLine = "Test",
                City = "Test city",
                State = "Test State",
                HighestQualification = "Bachelor's",
                BloodGroup = "A+",
                EyeColor = "Black",
                Complexion = "Fair",
                HairColor = "Black",
                LivingWith = "With Family",
                PartnerPreferences = new PartnerPreferencesDTO
                {
                    HeightMin = 150,
                    HeightMax = 180,
                    Complexion = "Fair",
                    Language = "Tamil",
                    MaritalStatus = "Single",
                    Education = "None",
                    Religion = "None",
                    State = "None",
                },
                Education = new List<EducationDTO>(),
                Career = new List<CareerDTO>()
            };

            addProfileDTO.Education.Add(new EducationDTO { Degree = "BE", Specialization = "Something", StartYear = 2012, EndYear = 2016, Status = "Completed" });

            addProfileDTO.Career.Add(new CareerDTO { Company = "Test Company", Designation = "Test Role", StartYear = 2016, EndYear = null });

            var result = await _service.AddUserProfile(addProfileDTO);

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.result);
        }

        [Test]
        public async Task GetUserContactDetails_UserProfileNotFound_ThrowsException()
        {
            int userProfileId = 1;
            Assert.ThrowsAsync<UserProfileNotFoundException>(async () => await _service.GetUserContactDetails(userProfileId));
        }

        [Test]
        public async Task GetUserContactDetails_ValidData_ReturnsContactDetails()
        {
            var basicInfo = new BasicInfo { FirstName = "Test", LastName = "Test", Gender = "Male",Email = "test@example.com", Phone = "1234567890" };
            basicInfo = await _basicInfoRepo.Add(basicInfo);

            var userProfile = new UserProfile { UserId = 1, BasicInfoId = basicInfo.Id, ProfileFor="Self"};
            await _userProfileRepo.Add(userProfile);

            var result = await _service.GetUserContactDetails(userProfile.Id);

            Assert.IsNotNull(result);
            var contactDetails = result.result as ContactDetailsDTO;
            Assert.IsNotNull(contactDetails);
            Assert.AreEqual("test@example.com", contactDetails.Email);
            Assert.AreEqual("1234567890", contactDetails.Mobile);
        }

        [Test]
        public async Task VerifyUserProfileStatus_ProfileCompleted_ReturnsTrue()
        {
            var basicInfo = new BasicInfo { FirstName = "Test", LastName = "Test", Gender = "Male", Email = "test@example.com", Phone = "1234567890" };
            basicInfo = await _basicInfoRepo.Add(basicInfo);
            var userProfile = new UserProfile { UserId = 1, ProfileCompleted = true, BasicInfoId=1,ProfileFor="Self" };
            await _userProfileRepo.Add(userProfile);

            var result = await _service.VerifyUserProfileStatus(userProfile.UserId);

            Assert.IsTrue(result);
        }

        [Test]
        public async Task VerifyUserProfileStatus_ProfileNotCompleted_ReturnsFalse()
        {
            var basicInfo = new BasicInfo { FirstName = "Test", LastName = "Test", Gender = "Male", Email = "test@example.com", Phone = "1234567890" };
            basicInfo = await _basicInfoRepo.Add(basicInfo);
            var userProfile = new UserProfile { UserId = 1, ProfileCompleted = false, BasicInfoId = 1, ProfileFor = "Self" };
            await _userProfileRepo.Add(userProfile);

            var result = await _service.VerifyUserProfileStatus(userProfile.UserId);

            Assert.IsFalse(result);
        }

        [Test]
        public async Task CreateUserProfile_ValidData_CreatesUserProfile()
        {
            var registerUserProfileDTO = new RegisterUserProfileDTO
            {
                UserId = 1,
                FirstName = "John",
                LastName = "Doe",
                Gender = "Male",
                DOB = new DateTime(1990, 1, 1),
                Email = "johndoe@example.com",
                Phone = "1234567890",
                ProfileFor = "Self"
            };

            var result = await _service.CreateUserProfile(registerUserProfileDTO);

            Assert.IsNotNull(result);
            var userProfile = result.result as UserProfile;
            Assert.IsNotNull(userProfile);
            Assert.AreEqual(registerUserProfileDTO.UserId, userProfile.UserId);
        }

    }
}
