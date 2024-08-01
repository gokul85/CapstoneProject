namespace PremiumService.Models.DTOs
{
    public class ContactResponseModel
    {
        public ContactReturnDTO Result { get; set; }
        public bool HasError { get; set; }
        public string ErrorMessage { get; set; }
    }
}
