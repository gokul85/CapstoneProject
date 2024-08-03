using Newtonsoft.Json;
using PremiumService.AsyncDataService;
using PremiumService.Exceptions;
using PremiumService.Interfaces;
using PremiumService.Models;
using PremiumService.Models.DTOs;
using System.Net.Http;

namespace PremiumService.Services
{
    public class PremiumUserService : IPremiumUserService
    {
        private readonly IRepository<int, Payments> _paymentrepo;
        private readonly IRepository<int, ContactViews> _contactviewrepo;
        private readonly RabbitMQPublisher _rabbitMQPublisher;
        private readonly HttpClient _httpClient;
        public PremiumUserService(IRepository<int, ContactViews> contactviewrepo, IRepository<int, Payments> paymentrepo, RabbitMQPublisher rabbitMQPublisher, HttpClient httpClient)
        {
            _paymentrepo = paymentrepo;
            _contactviewrepo = contactviewrepo;
            _rabbitMQPublisher = rabbitMQPublisher;
            _httpClient = httpClient;
        }

        public async Task<ResponseModel> CheckContactView(int userid, int profileid)
        {
            var contactprofile = await _contactviewrepo.FindAll(cv => cv.UserId == userid && cv.ViewedUserId == profileid);
            object result = null;
            if (contactprofile == null)
            {
                var todaycontact = await _contactviewrepo.FindAll(cv => cv.UserId == userid && cv.ViewedTime.Date == DateTime.Today);
                if (todaycontact == null)
                {
                    result = new CheckContactViewReturnDTO() { remainingCount = 5 };
                }
                else
                {
                    result = new CheckContactViewReturnDTO() { remainingCount = 5 - todaycontact.Count() };
                }
            }
            else
            {
                try
                {
                    result = await RetriveUserContactDetails(profileid);
                }
                catch(Exception ex) 
                {
                    throw ex;
                }
            }
            return new ResponseModel() { result = result };
        }

        public async Task<ResponseModel> ContactView(int userid, int profileid)
        {
            var todaycontact = await _contactviewrepo.FindAll(cv => cv.UserId == userid && cv.ViewedTime.Date == DateTime.Today);
            if (todaycontact == null || todaycontact.Count() < 5)
            {
                var contactview = new ContactViews()
                {
                    UserId = userid,
                    ViewedUserId = profileid,
                    ViewedTime = DateTime.Today,
                };

                await _contactviewrepo.Add(contactview);

                var responsedata = await RetriveUserContactDetails(profileid);

                return new ResponseModel() { result = responsedata };
            }
            
            throw new DailyLimitReachedException("Daily Contact View Limit Reached");
        }

        private async Task<object> RetriveUserContactDetails(int profileid)
        {
            var response = await _httpClient.GetAsync("https://localhost:7000/api/profile/getusercontactdetails?id=" + profileid);
            if (!response.IsSuccessStatusCode)
            {
                throw new UnableToRetriveContactDetails("Unable to retrive contact details");
            }
            var content = await response.Content.ReadAsStringAsync();
            var responseData = JsonConvert.DeserializeObject<ContactResponseModel>(content);
            var contactdetails = responseData.Result;
            return contactdetails;
        }

        public async Task<ResponseModel> SubscribePremium(SubscribePremiumDTO subscribePremiumDTO)
        {
            try
            {
                var payment = new Payments() { Amount = subscribePremiumDTO.Amount, UserId = subscribePremiumDTO.UserId, PaymentDate = DateTime.Now };

                await _paymentrepo.Add(payment);

                var response = await _httpClient.PostAsJsonAsync("https://localhost:7000/api/user/updateuserpremiumstatus", new PaymentCompleteMessageDTO { UserId = subscribePremiumDTO.UserId });
                if (!response.IsSuccessStatusCode)
                {
                    _rabbitMQPublisher.PublishPaymentMessage(new PaymentCompleteMessageDTO { UserId = subscribePremiumDTO.UserId });
                    throw new Exception("Payment Completed, Failed to update status now. Please try login again");
                }

                return new ResponseModel { result = true };
            }
            catch (Exception ex)
            {
                return new ResponseModel { result = false, ErrorMessage = ex.Message };
            }
        }
    }
}
