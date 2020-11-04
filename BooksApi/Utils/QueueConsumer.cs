using BooksApi.Models;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

namespace BooksApi.Utils
{
    public class QueueConsumer
    {
        public static void Consume(IModel channel)
        {
            // Define a fila do canal
            channel.QueueDeclare(Settings.getRabbitSetting("Channel"), durable: true, exclusive: false, autoDelete: false, arguments: null);

            // Trata os dados do canal
            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (sender, e) =>
            {
                try
                {
                    var body = e.Body.ToArray();
                    var jsonString = Encoding.UTF8.GetString(body);
                    var message = Settings.JSONToObject<BookRabbitMessage>(jsonString); 

                    Connector.getBookController().Create(message.data);

                    channel.BasicAck(e.DeliveryTag, false);
                }
                catch (Exception ex)
                {
                    channel.BasicNack(e.DeliveryTag, false, true);

                    Console.WriteLine(ex);
                }

            };

            // Consome a mensagem da fila
            channel.BasicConsume(Settings.getRabbitSetting("Channel"), false, consumer);
            Console.WriteLine("Consumer listering.");
            Console.ReadLine();
        }
    }
}
