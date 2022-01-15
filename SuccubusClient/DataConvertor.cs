using System;
using Newtonsoft.Json;

namespace SuccubusClient
{
	public static class DataConvertor
	{
		public static Incubus ConvertToIncubus(IncubusData data)
		{
			Incubus incubus = new Incubus();
			incubus = JsonConvert.DeserializeObject<Incubus>(data.data);

			return incubus;
		}

		public static IncubusData ConvertToIncubusData(Incubus incubus)
		{
			IncubusData data = new IncubusData();

			data.Id = 0;
			data.MAC = incubus.MAC;
			data.data = JsonConvert.SerializeObject(incubus);

			return data;
		}

		public static string FormatBytes(string bytes)
		{
			string[] Suffix = { "B", "KB", "MB", "GB", "TB" };
			int i;
			double dblSByte = Convert.ToDouble(bytes);
			double DoubleBytes = Convert.ToDouble(bytes);
			for (i = 0; i < Suffix.Length && DoubleBytes >= 1024; i++, DoubleBytes /= 1024)
			{
				dblSByte = DoubleBytes / 1024.0;
			}

			return String.Format("{0:0.##} {1}", dblSByte, Suffix[i]);
		}
	}
}
