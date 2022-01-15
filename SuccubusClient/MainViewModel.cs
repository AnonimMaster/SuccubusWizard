using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuccubusClient
{
	class MainViewModel
	{
		public MainViewModel(List<Incubus> IncubusList)
		{

			if (IncubusList.Count > 0)
			{
				Collection = new ObservableCollection<Data>();

				foreach (Incubus incubus in IncubusList)
				{
					Data PC = new Data();
					PC.Name = incubus.Name;
					PC.Children = new List<Data>();

					Data MAC = new Data();
					MAC.Name = "MAC";
					MAC.Value = incubus.MAC;

					Data CPU = new Data();
					CPU.Name = "Модель";
					CPU.Value = incubus.cpu.Model;
					CPU.Children = new List<Data>();

					Data CPUTemp = new Data();
					CPUTemp.Name = "Температура";
					CPUTemp.Children = new List<Data>();

					if (incubus.cpu.Temperature != null)
					{
						for (int i = 0; i < incubus.cpu.Temperature.Values.Count; i++)
						{
							Data temp = new Data();
							temp.Name = "Core#" + i;
							temp.Value = incubus.cpu.Temperature.Values[i];
							temp.MaxValue = incubus.cpu.Temperature.MaxValues[i];
							CPUTemp.Children.Add(temp);
						}
					}
					else
					{
						Data temp = new Data();
						temp.Name = "Пусто";
						temp.Value = "";
						temp.MaxValue = "";
						CPUTemp.Children.Add(temp);
					}
						

					Data CPUClocks = new Data();
					CPUClocks.Name = "Clocks";
					CPUClocks.Children = new List<Data>();

					if(incubus.cpu.Clocks != null)
					{
						for (int i = 0; i < incubus.cpu.Clocks.Values.Count; i++)
						{
							Data temp = new Data();
							temp.Name = "Core#" + i;
							temp.Value = incubus.cpu.Clocks.Values[i];
							temp.MaxValue = incubus.cpu.Clocks.MaxValues[i];
							CPUTemp.Children.Add(temp);
						}
					}
					else
					{
						Data temp = new Data();
						temp.Name = "Пусто";
						temp.Value = "";
						temp.MaxValue = "";
						CPUTemp.Children.Add(temp);
					}

					Data CPULoad = new Data();
					CPULoad.Name = "Нагрузка";
					CPULoad.Children = new List<Data>();

					if(incubus.cpu.Load != null)
					{
						for (int i = 0; i < incubus.cpu.Load.Values.Count; i++)
						{
							Data temp = new Data();
							temp.Name = "Core#" + i;
							temp.Value = incubus.cpu.Load.Values[i];
							temp.MaxValue = incubus.cpu.Load.MaxValues[i];
							CPUTemp.Children.Add(temp);
						}
					}
					else
					{
						Data temp = new Data();
						temp.Name = "Пусто";
						temp.Value = "";
						temp.MaxValue = "";
						CPUTemp.Children.Add(temp);
					}
					

					Data CPUPowers = new Data();
					CPUPowers.Name = "Powers";
					CPUPowers.Children = new List<Data>();

					if(incubus.cpu.Powers != null)
					{
						for (int i = 0; i < incubus.cpu.Powers.Values.Count; i++)
						{
							Data temp = new Data();
							temp.Name = "Core#" + i;
							temp.Value = incubus.cpu.Powers.Values[i];
							temp.MaxValue = incubus.cpu.Powers.MaxValues[i];
							CPUTemp.Children.Add(temp);
						}
					}
					else
					{
						Data temp = new Data();
						temp.Name = "Пусто";
						temp.Value = "";
						temp.MaxValue = "";
						CPUTemp.Children.Add(temp);
					}
					

					CPU.Children.Add(CPUTemp);
					CPU.Children.Add(CPUClocks);
					CPU.Children.Add(CPULoad);
					CPU.Children.Add(CPUPowers);

					PC.Children.Add(MAC);
					PC.Children.Add(CPU);

					Collection.Add(PC);
				}
			}
		}

		public ObservableCollection<Data> Collection { get; }
	}

	public class Data
	{
		public string Name { get; set; }
		public string Value { get; set; }
		public string MaxValue { get; set; }
		public List<Data> Children { get; set; }
	}
}
