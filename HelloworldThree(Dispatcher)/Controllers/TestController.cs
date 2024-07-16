using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HelloworldThree.Request;
using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Client;

namespace HelloworldThree.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class TestController : ControllerBase
    {

        [HttpPost(Name = "SetTest")]
        public void Test(TestRequest request)
        {
            // for rabbit mq
            // Define RabbitMQ server settings
            var factory = new ConnectionFactory() { HostName = "localhost" };
            // Create a connection to RabbitMQ server
            using var connection = factory.CreateConnection();
            // Open a channel
            using var channel = connection.CreateModel();
            // Declare a queue to send and receive messages
            channel.QueueDeclare(queue: "hello",
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);
            // Publish a message to the queue
            string message = request.Message;
            var body = Encoding.UTF8.GetBytes(message);
            channel.BasicPublish(exchange: "",
                                 routingKey: "hello",
                                 basicProperties: null,
                                 body: body);

            Console.WriteLine(" [x] Sent {0}", message);
            // end RabbitMQ
        }

    }
}