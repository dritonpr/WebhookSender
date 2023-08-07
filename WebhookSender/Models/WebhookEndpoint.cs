namespace WebhookSender.Models
{
    public class WebhookEndpoint
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
    }
}
