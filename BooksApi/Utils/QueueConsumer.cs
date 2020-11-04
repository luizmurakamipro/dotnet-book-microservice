using BooksApi.Controllers;
using BooksApi.Models;
using BooksApi.Services;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;

namespace BooksApi.Utils
{
    public class QueueConsumer
    {
        public static T JSONToObject<T>(string jsonString)
        {
            try
            {
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(T));
                MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(jsonString));
                T obj = (T)serializer.ReadObject(ms);
                return obj;
            }
            catch
            {
                throw;
            }
        }
        public static string ObjectToJSON<T>(T obj)
        {
            try
            {
                DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(T));
                MemoryStream ms = new MemoryStream();
                ser.WriteObject(ms, obj);
                string jsonString = Encoding.UTF8.GetString(ms.ToArray());
                ms.Close();
                return jsonString;
            }
            catch
            {
                throw;
            }
        }

        public static void Consume(IModel channel)
        {
            // Define a fila do canal
            channel.QueueDeclare("rmq-nestjs", durable: true, exclusive: false, autoDelete: false, arguments: null);

            // Trata os dados do canal
            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (sender, e) =>
            {
                try
                {
                    var body = e.Body.ToArray();
                    var jsonString = Encoding.UTF8.GetString(body);
                    var message = JSONToObject<BookRabbitMessage>(jsonString); 

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
            channel.BasicConsume("rmq-nestjs", false, consumer);
            Console.WriteLine("Consumer listering.");
            Console.ReadLine();
        }
    }
}
