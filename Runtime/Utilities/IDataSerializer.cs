// Perfect Core for Unity
// Copyright © 2020 Bogdan Nikolayev <bodix321@gmail.com>
// All Rights Reserved

namespace PerfectCore
{
	public interface IDataSerializer
	{
		string Extension { get; }
		void Serialize<T>(T data, string filePath);
		T Deserialize<T>(string filePath);
	}
}