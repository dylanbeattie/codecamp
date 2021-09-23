using Autobarn.Messages;
using EasyNetQ;
using System;
using System.Threading.Tasks;

namespace Autobarn.AuditLog {
	class Program {
		private const string AMQP =
			"amqps://uzvpuvak:r4qZrXuLImUVgkhthDNq46QPbwcT4Fo7@rattlesnake.rmq.cloudamqp.com/uzvpuvak";

		static async Task Main(string[] args) {
			var subscriberId = $"Autobarn.AuditLog@{Environment.MachineName}";
			using var bus = RabbitHutch.CreateBus(AMQP);
			Console.WriteLine("Connected! Listening for NewVehicleMessage messages.");
			await bus.PubSub.SubscribeAsync<NewVehicleMessage>(subscriberId, HandleNewVehicleMessage);
			Console.ReadKey(true);
		}

		private static void HandleNewVehicleMessage(NewVehicleMessage message) {
			var csv =
				$"{message.Registration},{message.Manufacturer},{message.ModelName},{message.Color},{message.Year},{message.ListedAtUtc:O}";
			Console.WriteLine(csv);
		}
	}
}
