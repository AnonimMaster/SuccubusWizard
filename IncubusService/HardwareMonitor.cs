using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using OpenHardwareMonitor.Hardware;
using System.IO;

namespace IncubusService
{
	public class HardwareMonitor
	{
		public string GetMACAddress()
		{
			NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
			String sMacAddress = string.Empty;
			foreach (NetworkInterface adapter in nics)
			{
				if (sMacAddress == String.Empty) 
				{
					IPInterfaceProperties properties = adapter.GetIPProperties();
					sMacAddress = adapter.GetPhysicalAddress().ToString();
				}
			}
			return sMacAddress;
		}

		public CPU GetCPU()
		{
			CPU cpu = new CPU();
			string Model = string.Empty;

			ValueContainer Temperature = new ValueContainer();
			Temperature.Values = new List<string>();
			Temperature.MaxValues = new List<string>();

			ValueContainer Clocks = new ValueContainer();
			Clocks.Values = new List<string>();
			Clocks.MaxValues = new List<string>();

			ValueContainer Load = new ValueContainer();
			Load.Values = new List<string>();
			Load.MaxValues = new List<string>();

			ValueContainer Powers = new ValueContainer();
			Powers.Values = new List<string>();
			Powers.MaxValues = new List<string>();

			Visitor visitor = new Visitor();

			Computer computer = new Computer();
			computer.Open();
			computer.CPUEnabled = true;
			computer.FanControllerEnabled = true;
			computer.Accept(visitor);

			for (int i = 0; i < computer.Hardware.Length; i++)
			{
				if (computer.Hardware[i].HardwareType == HardwareType.CPU)
				{
					//Model
					Model = computer.Hardware[i].Name;

					//Temperature
					for (int j = 0; j < computer.Hardware[i].Sensors.Length; j++)
					{
						if (computer.Hardware[i].Sensors[j].SensorType == SensorType.Temperature)
						{
							Temperature.Values.Add(computer.Hardware[i].Sensors[j].Value.ToString());
							Temperature.MaxValues.Add(computer.Hardware[i].Sensors[j].Max.ToString());
						}
					}

					//Clocks
					for (int j = 0; j < computer.Hardware[i].Sensors.Length; j++)
					{
						if (computer.Hardware[i].Sensors[j].SensorType == SensorType.Clock)
						{
							Clocks.Values.Add(computer.Hardware[i].Sensors[j].Value.ToString());
							Clocks.MaxValues.Add(computer.Hardware[i].Sensors[j].Max.ToString());
						}
					}

					//Load
					for (int j = 0; j < computer.Hardware[i].Sensors.Length; j++)
					{
						if (computer.Hardware[i].Sensors[j].SensorType == SensorType.Load)
						{
							Load.Values.Add(computer.Hardware[i].Sensors[j].Value.ToString());
							Load.MaxValues.Add(computer.Hardware[i].Sensors[j].Max.ToString());
						}
					}

					//Powers
					for (int j = 0; j < computer.Hardware[i].Sensors.Length; j++)
					{
						if (computer.Hardware[i].Sensors[j].SensorType == SensorType.Power)
						{
							Powers.Values.Add(computer.Hardware[i].Sensors[j].Value.ToString());
							Powers.MaxValues.Add(computer.Hardware[i].Sensors[j].Max.ToString());
						}
					}
				}
			}

			cpu.Model = Model;
			cpu.Temperature = Temperature;
			cpu.Clocks = Clocks;
			cpu.Load = Load;
			cpu.Powers = Powers;

			return cpu;
		}

		public GPU GetGPU()
		{
			GPU gpu = new GPU();
			string Model = string.Empty;

			ValueContainer Temperature = new ValueContainer();
			Temperature.Values = new List<string>();
			Temperature.MaxValues = new List<string>();

			ValueContainer Clocks = new ValueContainer();
			Clocks.Values = new List<string>();
			Clocks.MaxValues = new List<string>();

			ValueContainer Load = new ValueContainer();
			Load.Values = new List<string>();
			Load.MaxValues = new List<string>();

			ValueContainer Fans = new ValueContainer();
			Fans.Values = new List<string>();
			Fans.MaxValues = new List<string>();

			Visitor visitor = new Visitor();

			Computer computer = new Computer();
			computer.Open();
			computer.GPUEnabled = true;
			computer.FanControllerEnabled = true;
			computer.Accept(visitor);

			for (int i = 0; i < computer.Hardware.Length; i++)
			{
				if (computer.Hardware[i].HardwareType == HardwareType.GpuNvidia)
				{
					//Model
					Model = computer.Hardware[i].Name;

					//Temperature
					for (int j = 0; j < computer.Hardware[i].Sensors.Length; j++)
					{
						if (computer.Hardware[i].Sensors[j].SensorType == SensorType.Temperature)
						{
							Temperature.Values.Add(computer.Hardware[i].Sensors[j].Value.ToString());
							Temperature.MaxValues.Add(computer.Hardware[i].Sensors[j].Max.ToString());
						}
					}

					//Clocks
					for (int j = 0; j < computer.Hardware[i].Sensors.Length; j++)
					{
						if (computer.Hardware[i].Sensors[j].SensorType == SensorType.Clock)
						{
							Clocks.Values.Add(computer.Hardware[i].Sensors[j].Value.ToString());
							Clocks.MaxValues.Add(computer.Hardware[i].Sensors[j].Max.ToString());
						}
					}

					//Load
					for (int j = 0; j < computer.Hardware[i].Sensors.Length; j++)
					{
						if (computer.Hardware[i].Sensors[j].SensorType == SensorType.Load)
						{
							Load.Values.Add(computer.Hardware[i].Sensors[j].Value.ToString());
							Load.MaxValues.Add(computer.Hardware[i].Sensors[j].Max.ToString());
						}
					}

					//Fans
					for (int j = 0; j < computer.Hardware[i].Sensors.Length; j++)
					{
						if (computer.Hardware[i].Sensors[j].SensorType == SensorType.Fan)
						{
							Fans.Values.Add(computer.Hardware[i].Sensors[j].Value.ToString());
							Fans.MaxValues.Add(computer.Hardware[i].Sensors[j].Max.ToString());
						}
					}
				}
			}

			gpu.Model = Model;
			gpu.Temperature = Temperature;
			gpu.Clocks = Clocks;
			gpu.Load = Load;
			gpu.Fans = Fans;

			return gpu;
		}

		public List<HardwareDisk> GetDisks()
		{
			List<HardwareDisk> disks = new List<HardwareDisk>();

			foreach (var drive in DriveInfo.GetDrives())
			{
				try
				{
					HardwareDisk disk = new HardwareDisk();
					disk.Name = drive.Name;
					disk.TotalSize = ""+drive.TotalSize;
					disk.TotalFreeSpace = ""+drive.TotalFreeSpace;
					disks.Add(disk);
				}
				catch { }
			}

			return disks;
		}
	}
}
