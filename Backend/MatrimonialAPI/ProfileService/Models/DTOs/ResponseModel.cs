namespace ProfileService.Models.DTOs
{
    public class ResponseModel
    {
        public object? result { get; set; }
        public bool HasError { get; set; } = false;
        public string ErrorMessage { get; set; } = string.Empty;
    }
}
