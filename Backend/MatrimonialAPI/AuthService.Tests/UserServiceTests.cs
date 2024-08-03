using AuthService.AsyncDataService;
using AuthService.Data;
using AuthService.Exceptions;
using AuthService.Interfaces;
using AuthService.Models;
using AuthService.Models.DTOs;
using AuthService.Repositories;
using AuthService.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using NUnit.Framework;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AuthService.Tests
{
    public class UserServiceTests
    {
        private IUserService _userService;
        private Mock<ITokenService> _tokenServiceMock;
        private AuthServiceDBContext _dbContext;
        private IRepository<int, User> _userRepository;
        private IRepository<int, UserDetails> _userDetailsRepository;
        private RabbitMQPublisher _rabbitMQPublisherMock;
        private string _validToken;

        [SetUp]
        public async Task Setup()
        {
            var options = new DbContextOptionsBuilder<AuthServiceDBContext>()
                .UseInMemoryDatabase(databaseName: "AuthServiceTestDB")
                .Options;
            _dbContext = new AuthServiceDBContext(options);
            _dbContext.Database.EnsureCreated();

            _tokenServiceMock = new Mock<ITokenService>();
            _userRepository = new UserRepository(_dbContext);
            _userDetailsRepository = new UserDetailRepository(_dbContext);
            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string>
                {
                    { "RabbitMQ:Host", "localhost" },
                    { "RabbitMQ:AuthQueueName", "test_authprofile_queue" }
                })
                .Build();
            _rabbitMQPublisherMock = new RabbitMQPublisher(configuration);

            _userService = new UserService(_userRepository, _userDetailsRepository, _tokenServiceMock.Object, _rabbitMQPublisherMock);

            var userDTO = new UserRegisterDTO
            {
                OnBehalf = "Self",
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                Phone = "1234567890",
                Password = "password123",
                Gender = "Male",
                DOB = new DateTime(1990, 1, 1)
            };

            var registeredUser = await _userService.Register(userDTO);

            var loginDTO = new UserLoginDTO
            {
                Email = "john.doe@example.com",
                Password = "password123"
            };

            _tokenServiceMock.Setup(tokenService => tokenService.GenerateToken(It.IsAny<User>(), false)).Returns("valid-token");

            var loginResult = await _userService.Login(loginDTO);
            _validToken = loginResult.Token;
        }

        [TearDown]
        public void TearDown()
        {
            _dbContext.Database.EnsureDeleted();
            _dbContext.Dispose();
        }

        [Test]
        public async Task Register_ValidUser_ReturnsUser()
        {
            var userDTO = new UserRegisterDTO
            {
                OnBehalf = "Self",
                FirstName = "Jane",
                LastName = "Doe",
                Email = "jane.doe@example.com",
                Phone = "0987654321",
                Password = "password123",
                Gender = "Female",
                DOB = new DateTime(1992, 2, 2)
            };

            var result = await _userService.Register(userDTO);

            Assert.IsNotNull(result);
            Assert.AreEqual("jane.doe@example.com", result.Email);
        }

        [Test]
        public async Task Register_UserAlreadyRegisteredException()
        {
            var userDTO = new UserRegisterDTO
            {
                OnBehalf = "Self",
                FirstName = "Jane",
                LastName = "Doe",
                Email = "jane.doe@example.com",
                Phone = "0987654321",
                Password = "password123",
                Gender = "Female",
                DOB = new DateTime(1992, 2, 2)
            };

            var result = await _userService.Register(userDTO);

            var exception = Assert.ThrowsAsync<UserAlreadyExistException>(() => _userService.Register(userDTO));

            Assert.AreEqual("User Account Already Exists, Please Login!", exception.Message);
        }

        [Test]
        public async Task Login_ValidCredentials_ReturnsToken()
        {
            var loginDTO = new UserLoginDTO
            {
                Email = "john.doe@example.com",
                Password = "password123"
            };

            _tokenServiceMock.Setup(tokenService => tokenService.GenerateToken(It.IsAny<User>(), false)).Returns("valid-token");

            var result = await _userService.Login(loginDTO);

            Assert.IsNotNull(result);
            Assert.AreEqual("valid-token", result.Token);
        }

        [Test]
        public void Login_InvalidCredentials_ThrowsUnauthorizedUserException()
        {
            var loginDTO = new UserLoginDTO
            {
                Email = "invalid@example.com",
                Password = "invalidpassword"
            };

            var exception = Assert.ThrowsAsync<UnauthorizedUserException>(() => _userService.Login(loginDTO));

            Assert.AreEqual("Invalid Email or password", exception.Message);
        }

        [Test]
        public async Task Login_UserAcountNotActiveException()
        {
            var userDTO = new UserRegisterDTO
            {
                OnBehalf = "Self",
                FirstName = "Jane",
                LastName = "Doe",
                Email = "jane.doe@example.com",
                Phone = "0987654321",
                Password = "password123",
                Gender = "Female",
                DOB = new DateTime(1992, 2, 2)
            };

            var result = await _userService.Register(userDTO);

            var users = await _userRepository.FindAll(u => u.Email == "john.doe@example.com");
            var user = users.First();
            var updateUserStatusDTO = new UpdateUserStatusDTO
            {
                UserId = user.Id,
                Status = "Disabled"
            };

            var result2 = await _userService.UpdateUserStatus(updateUserStatusDTO);

            var loginDTO = new UserLoginDTO
            {
                Email = "john.doe@example.com",
                Password = "password123"
            };

            var exception = Assert.ThrowsAsync<UserNotActiveException>(() => _userService.Login(loginDTO));

            var error = new ErrorModel() { ErrorCode = 400, ErrorMessage = exception.Message };

            Assert.AreEqual("Your account is not activated", error.ErrorMessage);
        }

        [Test]
        public async Task UpdateUserStatus_ValidUser_UpdatesStatus()
        {
            var users = await _userRepository.FindAll(u => u.Email == "john.doe@example.com");
            var user = users.First();
            var updateUserStatusDTO = new UpdateUserStatusDTO
            {
                UserId = user.Id,
                Status = "Active"
            };

            var result = await _userService.UpdateUserStatus(updateUserStatusDTO);

            var updatedUserDetails = await _userDetailsRepository.Get(user.Id);

            Assert.IsNotNull(result);
            Assert.AreEqual("User Status Successfully Updated", result);
            Assert.AreEqual("Active", updatedUserDetails.Status);
        }

        [Test]
        public async Task UpdateUserStatus_InvalidUser_ReturnsErrorMessage()
        {
            var updateUserStatusDTO = new UpdateUserStatusDTO
            {
                UserId = 999,
                Status = "Active"
            };

            var exception = Assert.ThrowsAsync<NoUserFoundException>(() => _userService.UpdateUserStatus(updateUserStatusDTO));

            Assert.AreEqual("No User Found", exception.Message);
        }

        [Test]
        public async Task GetAllUsers_ReturnsUsers()
        {
            var result = await _userService.GetAllUsers();

            Assert.AreEqual(1, result.Count());
        }

        [Test]
        public async Task UpdateUserRole_ValidUser_UpdatesRole()
        {
            var user = (await _userRepository.FindAll(u => u.Email == "john.doe@example.com")).First();
            var result = await _userService.UpdateUserRole(user.Id, "Admin");

            Assert.AreEqual("John is now Admin", result);
            var updatedUser = await _userRepository.Get(user.Id);
            Assert.AreEqual("Admin", updatedUser.Role);
        }

        [Test]
        public async Task UpdateUserRole_InvalidUser_ReturnsErrorMessage()
        {
            var userId = 999;
            var role = "Admin";

            var exception = Assert.ThrowsAsync<NoUserFoundException>(() => _userService.UpdateUserRole(userId, role));

            Assert.AreEqual("No User Found", exception.Message);
        }

        [Test]
        public async Task VerifyUserProfileStatus_ValidUserId_ReturnsProfileStatus()
        {
            var userId = (await _userRepository.FindAll(u => u.Email == "john.doe@example.com")).First().Id;
            var userdetails = await _userDetailsRepository.FindAll(ud => ud.UserId == userId);
            var userdetail = userdetails.FirstOrDefault();
            userdetail.ProfileCompleted = true;
            await _userDetailsRepository.Update(userdetail);

            var result = await _userService.VerifyUserProfileStatus(userId);

            Assert.IsTrue(result);
        }

        [Test]
        public void VerifyUserProfileStatus_InvalidUserId_ThrowsNoUserFoundException()
        {
            var exception = Assert.ThrowsAsync<NoUserFoundException>(() => _userService.VerifyUserProfileStatus(999));

            Assert.AreEqual("No User Found", exception.Message);
        }

        [Test]
        public async Task UpdateUserProfileStatus_ValidUserId_UpdatesProfileStatus()
        {
            var userId = (await _userRepository.FindAll(u => u.Email == "john.doe@example.com")).First().Id;
            var result = await _userService.UpdateUserProfileStatus(userId);

            var userdetails = await _userDetailsRepository.FindAll(ud => ud.UserId == userId);
            var userdetail = userdetails.FirstOrDefault();

            Assert.IsTrue(userdetail.ProfileCompleted);
        }

        [Test]
        public void UpdateUserProfileStatus_InvalidUserId_ThrowsNoUserFoundException()
        {
            var exception = Assert.ThrowsAsync<NoUserFoundException>(() => _userService.UpdateUserProfileStatus(999));

            Assert.AreEqual("No User Found", exception.Message);
        }


        [Test]
        public async Task UpdateUserPremiumStatus_ValidUserId_UpdatesPremiumStatus()
        {
            var userId = (await _userRepository.FindAll(u => u.Email == "john.doe@example.com")).First().Id;
            var result = await _userService.UpdateUserPremiumStatus(userId);

            var userdetails = await _userDetailsRepository.FindAll(ud => ud.UserId == userId);
            var userdetail = userdetails.FirstOrDefault();

            Assert.AreEqual("Updated", result);
            Assert.IsTrue(userdetail.IsPremium);
        }

        [Test]
        public void UpdateUserPremiumStatus_InvalidUserId_ThrowsNoUserFoundException()
        {
            var exception = Assert.ThrowsAsync<NoUserFoundException>(() => _userService.UpdateUserPremiumStatus(999));

            Assert.AreEqual("No User Found", exception.Message);
        }

        [Test]
        public async Task RefreshUserToken_ValidUserId_ReturnsToken()
        {
            var userId = (await _userRepository.FindAll(u => u.Email == "john.doe@example.com")).First().Id;
            var userdetails = await _userDetailsRepository.FindAll(ud => ud.UserId == userId);
            var userdetail = userdetails.FirstOrDefault();
            userdetail.IsPremium = true;
            await _userDetailsRepository.Update(userdetail);

            _tokenServiceMock.Setup(tokenService => tokenService.GenerateToken(It.IsAny<User>(), true)).Returns("valid-token");

            var token = await _userService.RefreshUserToken(userId);

            _tokenServiceMock.Verify(ts => ts.GenerateToken(It.IsAny<User>(), true), Times.Once);
            Assert.AreEqual("valid-token", token);
        }

        [Test]
        public void RefreshUserToken_InvalidUserId_ThrowsNoUserFoundException()
        {
            var exception = Assert.ThrowsAsync<NoUserFoundException>(() => _userService.RefreshUserToken(999));

            Assert.AreEqual("No User Found", exception.Message);
        }
    }
}
