// Perfect Core for Unity
// Copyright © 2020-2026 Bogdan Nikolayev <contact.perfectcore@gmail.com>
// All Rights Reserved.

#if NEWTONSOFT_JSON
using System.IO;
using Newtonsoft.Json;

namespace PerfectCore.PerfectFoundation.Newtonsoft
{
	public class JsonDataSerializer : IDataSerializer
	{
		private readonly JsonSerializerSettings _settings;

		public JsonDataSerializer(ConfigService configService)
		{
			_settings = new JsonSerializerSettings
			{
				Formatting = Formatting.Indented,
				Converters = { new DataAssetConverter<DataAsset>(configService) }
			};
		}

		public string Extension => ".json";

		public void Serialize<T>(T data, string filePath)
		{
			string json = JsonConvert.SerializeObject(data, _settings);

			File.WriteAllText(filePath, json);
		}

		public T Deserialize<T>(string filePath)
		{
			string json = File.ReadAllText(filePath);

			return JsonConvert.DeserializeObject<T>(json, _settings);
		}
	}
}
#endif