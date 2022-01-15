using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Xml.Linq;

namespace IncubusService
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
	}
}
