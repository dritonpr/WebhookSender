namespace WebhookSender.Models
{
    public class WebhookResult
    {
        public string Url { get; set; }
        public int StatusCode { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; }
    }
}
