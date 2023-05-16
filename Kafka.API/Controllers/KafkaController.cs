using Confluent.Kafka;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Kafka.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KafkaController : ControllerBase
    {
        private readonly IProducer<string, string> _producer;
        private readonly IConsumer<string, string> _consumer;

        public KafkaController(IProducer<string, string> producer, IConsumer<string, string> consumer)
        {
            _producer = producer;
            _consumer = consumer;
        }

        [HttpPost]
        [Route("/write")]
        public async Task<IActionResult> ProduceMessage(string message)
        {
            var result = await _producer.ProduceAsync("my-topic", new Message<string, string>
            {
                Key = Guid.NewGuid().ToString(),
                Value = message
            });

            return Ok($"Message '{message}' with key '{result.Key}' produced to topic '{result.TopicPartition.Topic}'");
        }

        [HttpGet]
        [Route("/read")]
        public async Task<IActionResult> ConsumeMessage()
        {
            _consumer.Subscribe("my-topic");

            var result = await Task.Run(() => _consumer.Consume());

            return Ok($"Message '{result.Message.Value}' with key '{result.Message.Key}' consumed from topic '{result.TopicPartition.Topic}'");
        }
    }
}
