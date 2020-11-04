using System;
using System.IO;
using BooksApi.Controllers;
using BooksApi.Models;
using BooksApi.Services;
using BooksApi.Utils;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;

namespace BooksApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var factory = new ConnectionFactory
            {
                Uri = new Uri(Settings.getRabbitSetting("ConnectionString"))
            };

            // Estabelece a conexão
            using var connection = factory.CreateConnection();
            // Cria o canal da conexão
            using var channel = connection.CreateModel();

            QueueConsumer.Consume(channel);
        }
    }
}
