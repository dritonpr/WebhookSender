using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using WebhookSender.Models;

namespace WebhookSender.Data
{
    public class WebhookContext : DbContext
    {
        public WebhookContext(DbContextOptions<WebhookContext> options) : base(options)
        {
        }

        public DbSet<WebhookEndpoint> WebhookEndpoints { get; set; }
    }
}
