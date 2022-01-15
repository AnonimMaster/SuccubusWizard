using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace IncubusService
{

	public class Incubus
	{
		public string Name { get; set; }
		public string MAC { get; set; }
		public CPU cpu { get; set; }
		public GPU gpu { get; set; }
		public List<HardwareDisk> disks { get; set; }
	}
	public class ValueContainer
	{
		public List<string> Values { get; set; }
		public List<string> MaxValues { get; set; }
	}

	public class CPU
	{
		public string Model { get; set; }
		public ValueContainer Temperature { get; set; }
		public ValueContainer Clocks { get; set; }
		public ValueContainer Load { get; set; }
		public ValueContainer Powers { get; set; }
	}
	public class GPU
	{
		public string Model { get; set; }
		public ValueContainer Temperature { get; set; }
		public ValueContainer Clocks { get; set; }
		public ValueContainer Load { get; set; }
		public ValueContainer Fans { get; set; }
	}

	public class HardwareDisk
	{
		public string Name { get; set; }
		public string TotalFreeSpace { get; set; }	//Байты
		public string TotalSize { get; set; }   //Байты
	}
}
