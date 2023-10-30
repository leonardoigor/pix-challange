using QueueShared.Queues.Base;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Reflection.Metadata.Ecma335;
using System.Text;

namespace QueueShared.Queues
{
    public class ImpactTransaction : BaseQueue
    {
        string queueName = nameof(ImpactTransaction);

        public bool isReceiver { get; private set; }

        public ImpactTransaction(bool isReceiver) : base(nameof(ImpactTransaction))
        {
            this.isReceiver = isReceiver;
        }
        public override void Send(string json, string exchange = "", IBasicProperties basicProperties = null)
        {
            if (Channel != null)
            {
                Channel.BasicPublish(
                    exchange: exchange,
                    routingKey: $"{queueName}_queue",
                    basicProperties: basicProperties,
                    body: Encoding.UTF8.GetBytes(json)
                    );
            }
        }
        public void SendDLQ(string json, string exchange = "", IBasicProperties basicProperties = null)
        {
            if (Channel != null)
            {
                Channel.BasicPublish(
                    exchange: exchange,
                    routingKey: $"{queueName}_dead_letter_queu",
                    basicProperties: basicProperties,
                    body: Encoding.UTF8.GetBytes(json)
                    );
            }
        }
        public override void Run()
        {
            _ = declareQueueAsync(queueName, OnChannelStart, isReceiver);
            base.Run();
        }
        private void OnChannelStart(IModel channel)
        {
            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += WatchChannel;
            channel.BasicConsume(queue: $"{queueName}_queue",
                     autoAck: false,
                     consumer: consumer);
        }
        private void WatchChannel(object model, BasicDeliverEventArgs ea)
        {
            var body = ea.Body.ToArray();
            var data = Encoding.UTF8.GetString(body);
            Console.WriteLine(data);
        }
    }
}
