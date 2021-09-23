using EasyNetQ;
using System;
using System.Threading.Tasks;

// To connect your own subscriber to our pub/sub system:
// dotnet new console -o Autobarn.Messages
// cd Autobarn.Messages
// dotnet add package EasyNetQ
// dotnet run

namespace Autobarn.Messages {
	internal class Program {
		private const string amqp =
			"amqps://uzvpuvak:r4qZrXuLImUVgkhthDNq46QPbwcT4Fo7@rattlesnake.rmq.cloudamqp.com/uzvpuvak";

		static async Task Main(string[] args) {
			// If your subscriberId is unique, you will receive a copy of every message
			// If multiple subscribers share an ID, they'll get "round robin" load balancing
			var subscriberId = $"Autobarn.AuditLog@{Environment.MachineName}";
			using var bus = RabbitHutch.CreateBus(amqp);
			Console.WriteLine("Connected! Listening for NewVehicleMessage messages.");
			await bus.PubSub.SubscribeAsync<NewVehicleMessage>(subscriberId, HandleNewVehicleMessage);
			Console.ReadKey(true);
		}

		private static void HandleNewVehicleMessage(NewVehicleMessage message) {
			var output = $@"Received a NewVehicleMessage!
Registration: {message.Registration}
Manufacturer: {message.Manufacturer}
Model:        {message.ModelName}
Color:        {message.Color}
Year:         {message.Year}
Listed At:    {message.ListedAtUtc:O}
----------------------------------------------------------
";
			Console.WriteLine(output);
		}
	}

	public class NewVehicleMessage {
		public string Registration { get; set; }
		public string Manufacturer { get; set; }
		public string ModelCode { get; set; }
		public string ModelName { get; set; }
		public string Color { get; set; }
		public int Year { get; set; }
		public DateTime ListedAtUtc { get; set; }
	}

	public class NewVehiclePriceMessage : NewVehicleMessage {
		public int Price { get; set; }
		public string CurrencyCode { get; set; }
	}

	public static class MessageExtensions {
		public static NewVehiclePriceMessage ToNewVehiclePriceMessage(this NewVehicleMessage incomingMessage, int price,
			string currencyCode) {
			return new NewVehiclePriceMessage {
				Manufacturer = incomingMessage.Manufacturer,
				ModelCode = incomingMessage.ModelCode,
				Color = incomingMessage.Color,
				ModelName = incomingMessage.ModelName,
				Registration = incomingMessage.Registration,
				Year = incomingMessage.Year,
				CurrencyCode = currencyCode,
				Price = price
			};
		}
	}
}
