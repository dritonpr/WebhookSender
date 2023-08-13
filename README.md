# Webhook Sender API
The Webhook Sender API is a .NET 7.0 Web API project that provides functionality to register webhook endpoints and trigger HTTP POST requests to all registered endpoints.

#Getting Started
To get a local copy up and running, follow these simple steps:

# Prerequisites
This application is built with .NET 7.0. Make sure you have the .NET 7.0 SDK installed on your machine.

# Installation
Clone the repo: git clone https://github.com/dritonpr/WebhookSender

# Usage
The API exposes the following endpoints:
- GET /api/webhook: Returns all registered webhook endpoints.
- POST /api/webhook: Adds a new webhook endpoint. The request body should be a JSON object with a Url property.
- POST /api/webhook/trigger: Sends a POST request to all registered webhook endpoints.
Application is configured to use Swagger OPEN ap, where you can test all endpoints.
You can alsouse an API client like Postman or curl to call these endpoints.

# Testing Webhooks
You can use a service like webhook.site to create a URL for testing the webhooks.

# Logging
The application logs information, warning, and error messages. Logs are written to the console. Check the console output for the log messages when you run the application.

# Policies
The application uses the Polly library to handle transient faults when triggering webhooks. It includes a retry policy with jitter and a circuit breaker policy. The retry count and circuit breaker duration are configurable through the appsettings.json file.

# Built With
- .NET 7.0
- ASP.NET Core 7.0
- Entity Framework Core 7.0
- SQLite
- Polly
