using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;

namespace SuccubusWizard
{

	public class Incubus
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string MAC { get; set; }
		/*public string cpuData { get; set; }

		[NotMapped]
		public CPU cpu
		{
			get {
				if (cpuData != null)
				{
					return JsonConvert.DeserializeObject<CPU>(cpuData);
				}
				else
				{
					return null;
				}
			}
			set { cpuData = JsonConvert.SerializeObject(value); }
		}

		public string gpuData { get; set; }

		[NotMapped]
		public GPU gpu
		{
			get {
				if(gpuData != null)
				{
					return JsonConvert.DeserializeObject<GPU>(gpuData);
				}
				else
				{
					return null;
				}	
			}
			set { gpuData = JsonConvert.SerializeObject(value); }
		}

		public string disksData { get; set; }

		[NotMapped]
		public List<HardwareDisk> disks
		{
			get {
				if (disksData != null)
				{
					return JsonConvert.DeserializeObject<List<HardwareDisk>>(disksData);
				}
				else
				{
					return null;
				}
				
			}
			set { disksData = JsonConvert.SerializeObject(value); }
		}*/

	}
	[NotMapped]
	public class ValueContainer
	{
		public List<string> Values { get; set; }
		public List<string> MaxValues { get; set; }
	}

	[NotMapped]
	public class CPU
	{
		public string Model { get; set; }
		public ValueContainer TemperatureData { get; set; }
		public ValueContainer ClocksData { get; set; }
		public ValueContainer LoadData { get; set; }
		public ValueContainer PowersData { get; set; }
	}
	[NotMapped]
	public class GPU
	{
		public string Model { get; set; }
		public ValueContainer TemperatureData { get; set; }
		public ValueContainer ClocksData { get; set; }
		public ValueContainer LoadData { get; set; }
		public ValueContainer FansData { get; set; }
	}

	[NotMapped]
	public class HardwareDisk
	{
		public string Name { get; set; }
		public string TotalFreeSpace { get; set; }
		public string TotalSize { get; set; }
	}
}
