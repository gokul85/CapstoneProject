using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq.Protected;
using Moq;
using Newtonsoft.Json;
using PremiumService.AsyncDataService;
using PremiumService.Data;
using PremiumService.Exceptions;
using PremiumService.Interfaces;
using PremiumService.Models;
using PremiumService.Models.DTOs;
using PremiumService.Repositories;
using PremiumService.Services;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace PremiumService.Tests
{
    [TestFixture]
    public class PremiumUserServiceTests
    {
        private PremiumUserService _service;
        private IRepository<int, ContactViews> _contactViewRepo;
        private IRepository<int, Payments> _paymentRepo;
        private RabbitMQPublisher _rabbitMQPublisher;
        private HttpClient _httpClient;
        private Mock<HttpMessageHandler> _mockHttpMessageHandler;
        private PremiumServiceDBContext _dbContext;

        [SetUp]
        public void SetUp()
        {
            var options = new DbContextOptionsBuilder<PremiumServiceDBContext>()
                .UseInMemoryDatabase(databaseName: "PremiumServiceTestDB")
                .Options;
            _dbContext = new PremiumServiceDBContext(options);
            _dbContext.Database.EnsureCreated();

            _contactViewRepo = new ContactViewsRepository(_dbContext);
            _paymentRepo = new PaymentsRepository(_dbContext);

            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string>
                {
                    { "RabbitMQ:Host", "localhost" },
                    { "RabbitMQ:Password", "guest" },
                    { "RabbitMQ:UserName", "guest" },
                    { "RabbitMQ:PaymentQueueName", "test_payment_queue" }
                })
                .Build();
            _rabbitMQPublisher = new RabbitMQPublisher(configuration);

            _mockHttpMessageHandler = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            _httpClient = new HttpClient(_mockHttpMessageHandler.Object)
            {
                BaseAddress = new Uri("https://localhost:7000")
            };

            _service = new PremiumUserService(
                _contactViewRepo,
                _paymentRepo,
                _rabbitMQPublisher,
                _httpClient);
        }

        [TearDown]
        public void TearDown()
        {
            _dbContext.Database.EnsureDeleted();
            _dbContext.Dispose();
        }

        [Test]
        public async Task CheckContactView_NoContactView_ReturnsRemainingCount()
        {
            int userId = 1;
            int profileId = 2;

            var result = await _service.CheckContactView(userId, profileId);

            Assert.IsNotNull(result);
            var data = result.result as CheckContactViewReturnDTO;
            Assert.AreEqual(5, data.remainingCount);
        }

        [Test]
        public async Task CheckContactView_ContactViewFound_ReturnsRemainingCount()
        {
            int userId = 1;
            int profileId1 = 2;
            int profileId2 = 3;
            var responseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonConvert.SerializeObject(new ContactResponseModel
                {
                    Result = new ContactReturnDTO { Mobile = "TestContactDetails", Email = "TestContactDetails" }
                }))
            };
            _mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(responseMessage);

            var result1 = await _service.ContactView(userId, profileId1);
            var result2 = await _service.CheckContactView(userId, profileId2);

            Assert.IsNotNull(result1);
            var data = result2.result as CheckContactViewReturnDTO;
            Assert.AreEqual(4, data.remainingCount);
        }

        [Test]
        public async Task CheckContactView_ContactViewFound_ReturnsContacts()
        {
            int userId = 1;
            int profileId1 = 2;
            int profileId2 = 2;
            var responseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonConvert.SerializeObject(new ContactResponseModel
                {
                    Result = new ContactReturnDTO { Mobile = "TestContactDetails", Email = "TestContactDetails" }
                }))
            };
            _mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(responseMessage);

            var result = await _service.ContactView(userId, profileId1);
            var responseMessage2 = new HttpResponseMessage(HttpStatusCode.InternalServerError)
            {
                Content = new StringContent("Internal Server Error")
            };
            _mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(responseMessage2);
            Assert.ThrowsAsync<UnableToRetriveContactDetails>(async () => await _service.CheckContactView(userId, profileId2));
        }

        [Test]
        public async Task ContactView_UnderLimit_ReturnsContactDetails()
        {
            int userId = 1;
            var newcontactview = new NewContactViewDTO()
            {
                ProfileId = 2
            };

            var responseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonConvert.SerializeObject(new ContactResponseModel
                {
                    Result = new ContactReturnDTO { Mobile = "TestContactDetails", Email = "TestContactDetails" }
                }))
            };
            _mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(responseMessage);

            var result = await _service.ContactView(userId, newcontactview.ProfileId);

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.result);

            // Verify the new entry in the repository
            var contactViewEntries = await _contactViewRepo.FindAll(cv => cv.UserId == userId && cv.ViewedUserId == newcontactview.ProfileId);
            Assert.AreEqual(1, contactViewEntries.Count());
        }

        [Test]
        public void ContactView_OverLimit_ThrowsDailyLimitReachedException()
        {
            int userId = 1;
            int profileId = 2;

            for (int i = 0; i < 5; i++)
            {
                _contactViewRepo.Add(new ContactViews { UserId = userId, ViewedUserId = profileId, ViewedTime = DateTime.Today });
            }

            Assert.ThrowsAsync<DailyLimitReachedException>(async () => await _service.ContactView(userId, profileId));
        }

        [Test]
        public async Task SubscribePremium_ValidPayment_ReturnsTrue()
        {
            var subscribePremiumDTO = new SubscribePremiumDTO { UserId = 1, Amount = 100 };

            var responseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonConvert.SerializeObject(new { success = true }))
            };

            _mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(responseMessage);

            var result = await _service.SubscribePremium(subscribePremiumDTO);

            Assert.IsTrue((bool)result.result);

            // Verify the new payment in the repository
            var paymentEntries = await _paymentRepo.FindAll(p => p.UserId == subscribePremiumDTO.UserId && p.Amount == subscribePremiumDTO.Amount);
            Assert.AreEqual(1, paymentEntries.Count());
        }

        [Test]
        public async Task SubscribePremium_FailedPayment_ThrowsException()
        {
            var subscribePremiumDTO = new SubscribePremiumDTO { UserId = 1, Amount = 100 };

            var responseMessage = new HttpResponseMessage(HttpStatusCode.InternalServerError)
            {
                Content = new StringContent("Internal Server Error")
            };

            _mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(responseMessage);

            var result = await _service.SubscribePremium(subscribePremiumDTO);

            Assert.IsFalse((bool)result.result);
            Assert.IsNotEmpty(result.ErrorMessage);
        }
    }
}
