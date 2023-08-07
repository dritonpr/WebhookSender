namespace WebhookSender.Models
{
    public class WebhookPolicyOptions
    {
        public int RetryCount { get; set; }
        public int BreakDuration { get; set; }
    }
}
