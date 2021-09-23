using System;
using Autobarn.Data.Entities;
using Autobarn.Messages;
using EasyNetQ;

namespace Autobarn.Website {
	public static class BusExtensions {
		public static void PublishNewVehicleMessage(this IBus bus, Vehicle vehicle) {
			var message = new NewVehicleMessage() {
				Registration = vehicle.Registration,
				Manufacturer = vehicle.VehicleModel?.Manufacturer?.Name,
				ModelName = vehicle.VehicleModel?.Name,
				ModelCode = vehicle.VehicleModel?.Code,
				Color = vehicle.Color,
				Year = vehicle.Year,
				ListedAtUtc = DateTime.UtcNow
			};
			bus.PubSub.Publish(message);
		}
	}
}