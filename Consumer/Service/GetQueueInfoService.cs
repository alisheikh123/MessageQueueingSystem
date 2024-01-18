using ConsumerAPI.Model;
using ConsumerAPI.Model.Mail;
using ConsumerAPI.Service.Interface;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Net.Http.Headers;
using System.Net.Mail;
using System.Text;

namespace ConsumerAPI.Service
{
    public class GetQueueInfoService: IGetQueueInfoService
    {
        private readonly ILogger _logger;
        private readonly IMailService mailService;
        public GetQueueInfoService(ILogger<GetQueueInfoService> logger, IMailService mailService)
        {
            _logger = logger;
            this.mailService = mailService;
        }
        public void GetConsumerInfo()
        {
            
             var factory = new ConnectionFactory
            {
                HostName = "localhost"
            };
            //Create the RabbitMQ connection using connection factory details as i mentioned above
            var connection = factory.CreateConnection();
            //Here we create channel with session and model
            using var channel = connection.CreateModel();
            _logger.LogInformation("Consumer Connection Created");
            //declare the queue after mentioning name and a few property related to that
            channel.QueueDeclare("model", exclusive: false);

            _logger.LogInformation("Consumer Queue Added in Rabbit MQ");
            //Set Event object which listen message from chanel which is sent by producer
            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += async (model, eventArgs) =>
            {
                byte[] body = eventArgs.Body.ToArray();
                var jsonString = Encoding.UTF8.GetString(body);

                RequestParam response = JsonConvert.DeserializeObject<RequestParam>(jsonString);
                _logger.LogInformation("Subscriber Data {0}:",response);
                await CallAPIEndpoint(response);
            };
            //read the message
            channel.BasicConsume(queue: "model", autoAck: true, consumer: consumer);
            Console.ReadKey();
        }
    
      

        public async Task CallAPIEndpoint(RequestParam model)
        {
            using (HttpClient client = new HttpClient())
            {
                // Prepare the content to be sent (JSON payload)
                StringContent? content = null;
                if (!String.IsNullOrEmpty(model.Payload))
                {
                    content = new StringContent(model.Payload, Encoding.UTF8, "application/json");
                }
                if (!String.IsNullOrEmpty(model.Token))
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", model.Token);
                }

                const int maxAttempts = 10;
                for (int attempt = 1; attempt <= maxAttempts; attempt++)
                {
                    try
                    {
                        if (model.RequestMethod.ToUpper() == "GET")
                        {
                            var response = await client.GetAsync(model.RequestUrl);
                            response.EnsureSuccessStatusCode();
                            if (response.IsSuccessStatusCode)
                            {
                                string responseBody = await response.Content.ReadAsStringAsync();
                                // Process the response body as needed
                                Console.WriteLine(responseBody);

                                break;
                            }
                            else
                            {
                                attempt++;
                                if (attempt== maxAttempts)
                                {
                                    ExceedAPIRequestLimit();
                                }

                            }

                        }
                        if (model.RequestMethod.ToUpper() == "POST")
                        {


                            // Send POST request to the specified URL with the content
                            HttpResponseMessage response = await client.PostAsync(model.RequestUrl, content);

                            // Check if the request was successful (status code 2xx)
                            if (response.IsSuccessStatusCode)
                            {
                                string responseBody = await response.Content.ReadAsStringAsync();
                                Console.WriteLine("Response:", responseBody);
                                break;
                            }
                            else
                            {
                                attempt++;
                                if (attempt == maxAttempts)
                                {
                                    ExceedAPIRequestLimit();
                                }

                            }
                        }
                        if (model.RequestMethod.ToUpper() == "DELETE")
                        {
                            // Send DELETE request to the specified URL
                            HttpResponseMessage response = await client.DeleteAsync(model.RequestUrl);

                            // Check if the request was successful (status code 2xx)
                            if (response.IsSuccessStatusCode)
                            {
                                Console.WriteLine("DELETE request successful");
                                break;
                            }
                            else
                            {
                                attempt++;
                                if (attempt == maxAttempts)
                                {
                                    Console.WriteLine("DELETE request Limit is Exceeded");
                                    ExceedAPIRequestLimit();
                                }

                            }

                        }
                        if (model.RequestMethod.ToUpper() == "PUT")
                        {
                            HttpResponseMessage response = await client.PutAsync(model.RequestUrl, content);

                            // Check if the request was successful (status code 2xx)
                            if (response.IsSuccessStatusCode)
                            {
                                string responseBody = await response.Content.ReadAsStringAsync();
                                Console.WriteLine("Response:", responseBody);
                                break;
                            }
                        }



                    }
                    catch (HttpRequestException ex)
                    {
                        Console.WriteLine($"Attempt {attempt} failed. Error: {ex.Message}");

                        if (attempt == maxAttempts)
                        {
                            // Send email on the last attempt
                            ExceedAPIRequestLimit();
                        }
                        else
                        {
                            // Wait for some time before making the next attempt
                            await Task.Delay(1000); // Adjust the delay as needed
                        }
                    }
                }

               

            }

        }

        async void ExceedAPIRequestLimit()
        {
            //SendErrorEmail();
        }

        public async Task SendErrorEmail([FromForm] MailRequest request)
        {
            await mailService.SendEmailAsync(request);


            //try
            //{
            //    // Code to send an email using System.Net.Mail
            //    // Replace placeholders with your email server details and recipient's email address

            //    string smtpServer = "smtp.sendgrid.net";
            //    int smtpPort = 587;
            //    string smtpUsername = "apikey";
            //    string smtpPassword = "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";
            //    string recipientEmail = "No-reply@Cedarsbiz.com";
            //    string subject = "API Error Notification";
            //    string body = "There was an error while hitting the API endpoint.";

            //    using (var client = new SmtpClient(smtpServer, smtpPort))
            //    {
            //        client.Credentials = new System.Net.NetworkCredential(smtpUsername, smtpPassword);
            //        client.EnableSsl = true;

            //        var mailMessage = new MailMessage
            //        {
            //            //From = ,
            //            Subject = subject,
            //            Body = body,
            //            IsBodyHtml = false
            //        };

            //        mailMessage.To.Add(recipientEmail);

            //        client.Send(mailMessage);
            //    }
            //}
            //catch (HttpRequestException ex)
            //{


            //}
        }

        
    }
}





