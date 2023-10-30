using RabbitMQ.Client;


namespace QueueShared.Queues.Base
{
    public class BaseQueue:IDisposable
    {
        public string NameQueue { get; }
        public IConnection Connection { get; private set; }
        public IModel Channel { get; private set; }
        public BaseQueue(string nameQueue)
        {
            NameQueue = nameQueue;
        }
        public virtual void Send(string json, string exchange = "", IBasicProperties basicProperties = null) { }

        public virtual void Run()
        {
            Console.WriteLine($"Queue {NameQueue} is Started");
        }

        public async Task declareQueueAsync(string myQueue, Action<IModel> OnChannelStart, bool isReceive = true)
        {
            var factory = new ConnectionFactory { HostName = "localhost" };

            Connection = factory.CreateConnection();
            Channel = Connection.CreateModel();

            // Declare a dead-letter exchange and queue
            string deadLetterExchangeName = $"{myQueue}_dead_letter_exchange";
            string deadLetterQueueName = $"{myQueue}_dead_letter_queue";

            Channel.ExchangeDeclare(deadLetterExchangeName, ExchangeType.Fanout);
            Channel.QueueDeclare(deadLetterQueueName, durable: true, exclusive: false, autoDelete: false, arguments: null);
            Channel.QueueBind(deadLetterQueueName, deadLetterExchangeName, routingKey: "");

            // Declare a regular exchange and queue with a dead-letter configuration
            string exchangeName = $"{myQueue}_exchange";
            string queueName = $"{myQueue}_queue";

            var queueArgs = new Dictionary<string, object>
    {
        { "x-dead-letter-exchange", deadLetterExchangeName },
        { "x-dead-letter-routing-key", "" }
    };

            Channel.ExchangeDeclare(exchangeName, ExchangeType.Direct);
            Channel.QueueDeclare(queueName, durable: true, exclusive: false, autoDelete: false, arguments: queueArgs);
            Channel.QueueBind(queueName, exchangeName, routingKey: $"{myQueue}_routing_key");
            if (!isReceive) return;
            OnChannelStart(Channel);
            while (true)
            {
                await Task.Delay(10 * 1000);
            }


        }

        public void Dispose()
        {
            Channel.Close();
            Connection.Close();
        }
    }
}