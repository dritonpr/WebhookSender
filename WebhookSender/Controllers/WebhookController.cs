using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebhookSender.Data;
using WebhookSender.Models;

namespace WebhookSender.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WebhookController : ControllerBase
    {
        private readonly WebhookContext _context;
        private readonly IHttpClientFactory _clientFactory;
        private readonly ILogger<WebhookController> _logger;

        public WebhookController(WebhookContext context, IHttpClientFactory clientFactory, ILogger<WebhookController> logger)
        {
            _context = context;
            _clientFactory = clientFactory;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<WebhookEndpoint>>> GetWebhookEndpoints()
        {
            return await _context.WebhookEndpoints.ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<WebhookEndpoint>> AddWebhookEndpoint(WebhookEndpoint endpoint)
        {
            _context.WebhookEndpoints.Add(endpoint);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetWebhookEndpoints), new { id = endpoint.Id }, endpoint);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var entity = _context.WebhookEndpoints.Find(id);
                if (entity == null)
                    return BadRequest(String.Format("WebhookEndpoint with this Id: <b>{0}</b> not found", id));

                _context.WebhookEndpoints.Remove(entity);
                await _context.SaveChangesAsync();

                return Ok(string.Format("WebhookEndpoint with this Id: {0} is deleted with success", id));
            }
            catch (Exception ex)
            {
                return BadRequest("mesage: " + ex.Message);
            }
        }



        [HttpPost("trigger")]
        public async Task<ActionResult<IEnumerable<WebhookResult>>> TriggerWebhooks()
        {
            var client = _clientFactory.CreateClient("WebhookClient");
            var endpoints = await _context.WebhookEndpoints.ToListAsync();

            var results = new List<WebhookResult>();

            foreach (var endpoint in endpoints)
            {
                var result = new WebhookResult { Url = endpoint.Url };

                try
                {
                    var response = await client.PostAsync(endpoint.Url, null);

                    result.StatusCode = (int)response.StatusCode;
                    result.Success = response.IsSuccessStatusCode;

                    if (!response.IsSuccessStatusCode)
                    {
                        result.Message = $"Failed to trigger webhook. HTTP status code: {response.StatusCode}. Url:{endpoint.Url}";
                        _logger.LogWarning(result.Message);
                    }
                    else
                    {
                        result.Message = $"Webhook has been successfully triggered! Url:{endpoint.Url}";
                        _logger.LogInformation(result.Message);
                    }
                }
                catch (Exception ex)
                {
                    result.Success = false;
                    result.Message = $"Exception occurred: {ex.Message}. Url:{endpoint.Url}";
                    _logger.LogError(ex, result.Message);
                }

                results.Add(result);
            }

            return Ok(results);
        }

    }
}