using System.Text.Json;

namespace EffectiveMobileTest;

class Program
{
	public static async Task Main(string[] args)
	{
		JsonSerializerOptions options = new()
		{
			WriteIndented = true
		};
		
		options.Converters.Add(new DateTimeConverter()); 
		
		string path = @"./input.json";

		var input = File.ReadAllText(path);
		var orderList = JsonSerializer.Deserialize<List<Order>>(input, options);

		if (orderList == null)
		{
			await Logger.getInstance().Log("orderList is null | input file is incorrect");
			return;
		}

		string? cityDistrict = null;
		DateTime? firstDeliveryDateTime = null;

		for (int argIndex = 0; argIndex < args.Length; argIndex++)
		{
			if (args[argIndex].Equals("_cityDistrict"))
			{
				if (argIndex + 1 < args.Length)
				{
					cityDistrict = args[argIndex + 1];

					if (cityDistrict.Length <= 0 || !long.TryParse(cityDistrict, out var n))
					{
						await Logger.getInstance().Log("Invalid city district in program parameters");
						return;
					}
				} else
				{
					await Logger.getInstance().Log("Value for _cityDistrict is not set");
					return;
				}
			}

			if (args[argIndex].Equals("_firstDeliveryDateTime"))
			{
				if (argIndex + 1 < args.Length)
				{
					if (DateTime.TryParse(args[argIndex + 1], out var time))
					{
						firstDeliveryDateTime = time;
					} else
					{
						await Logger.getInstance().Log("Invalid first delivery date time in program parameters");
						return;
					}
				} else
				{
					await Logger.getInstance().Log("Value for _firstDeliveryDateTime is not set");
					return;
				}
			}
		}

		if (cityDistrict == null || firstDeliveryDateTime == null)
		{
			await Logger.getInstance().Log("cityDistrict or firstDeliveryDateTime is null");
			return;
		}

		var plus30minutes = firstDeliveryDateTime.Value.AddMinutes(30);

		var result = orderList.Where(x =>
			x.order_time >= firstDeliveryDateTime && x.order_time <= plus30minutes &&
			x.district.Equals(cityDistrict)).ToList();

		var json = JsonSerializer.Serialize(result, options);
		await File.WriteAllTextAsync(Path.Combine(Directory.GetCurrentDirectory(), "result.json"), json);

		Logger.getInstance().Log("SUCCESS");
	}
}