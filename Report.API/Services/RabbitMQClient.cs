﻿using RabbitMQ.Client;

namespace Report.API.Services
{
    public class RabbitMQClient
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;

        public RabbitMQClient(string hostname)
        {
            var factory = new ConnectionFactory() { HostName = hostname };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
        }

        public IModel GetChannel()
        {
            return _channel;
        }
    }
}
