using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Newtonsoft.Json;
using System.Net;
using MahApps.Metro.Controls;
using System.Collections.ObjectModel;
using ControlzEx.Standard;

namespace SuccubusClient
{
	/// <summary>
	/// Логика взаимодействия для MainWindow.xaml
	/// </summary>
	public partial class MainWindow : MetroWindow
	{
		private static readonly HttpClient client = new HttpClient();
		public static bool ServerOnline = false;
		private readonly BackgroundWorker worker = new BackgroundWorker();
		private static List<Incubus> IncubusList = new List<Incubus>();
		private static MainViewModel ViewModel = new MainViewModel();

		public MainWindow()
		{
			InitializeComponent();
			worker.DoWork += worker_DoWork;
			worker.RunWorkerCompleted += worker_RunWorkerCompleted;
			worker.WorkerSupportsCancellation = true;

			this.DataContext = ViewModel;
			ViewModel.Collection = new ObservableCollection<Data>();
		}

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			worker.RunWorkerAsync();
		}

		private void Window_Closing(object sender, CancelEventArgs e)
		{
			worker.CancelAsync();
		}


		private void worker_DoWork(object sender, DoWorkEventArgs e)
		{
			while (true)
			{
				if (ServerOnline)
				{
					Update();
					Thread.Sleep(3000);
				}
				else
				{
					Ping();
					Thread.Sleep(5000);
				}
			}
		}

		private void worker_RunWorkerCompleted(object sender,RunWorkerCompletedEventArgs e)
		{

		}

		private async void Ping()
		{
			try
			{
				var result = await client.GetAsync("https://succubuswizard.azurewebsites.net/api/Incubus/GetServerStatus");
				if (result.StatusCode == HttpStatusCode.OK)
				{
					ServerOnline = true;
					//Вызывают методом BeginInvoke в основной поток где был создан ServerLabel, что иметь доступ к нему. Иначе пиздец
					this.ServerLabel.BeginInvoke((Action)(() => this.ServerLabel.Content = "Server Online"));
				}
			}
			catch (HttpRequestException ex)
			{
				ServerOnline = false;
				this.ServerLabel.BeginInvoke((Action)(() => this.ServerLabel.Content = "Server Offline"));
			}

		}

		private async void Update()
		{
			try
			{
				var result = await client.GetAsync("https://succubuswizard.azurewebsites.net/api/Incubus/GetIncubusList");
				var incubusresult = await result.Content.ReadAsAsync<IEnumerable<IncubusData>>();

				IncubusList.Clear();
				foreach(IncubusData data in incubusresult)
				{
					Incubus incubus = new Incubus();
					incubus = DataConvertor.ConvertToIncubus(data);

					IncubusList.Add(incubus);
				}


				this.BeginInvoke((Action)(() => UpdateContext())); 
					
			}
			catch (HttpRequestException ex)
			{
				ServerOnline = false;
			}
		}
		

		private void UpdateContext()
		{
			foreach(Incubus incubus in IncubusList)
			{
				Data FindData = ViewModel.Collection.FirstOrDefault(x => x.Name == incubus.Name);
				if (FindData != null)
				{
					foreach(Data data in FindData.Children)
					{
						if(data.Name == "Процессор")
						{
							data.Value = incubus.cpu.Model;
							foreach(Data cpuData in data.Children)
							{
								if(cpuData.Name == "Температура")
								{
									for(int i = 0; i < cpuData.Children.Count; i++)
									{
										cpuData.Children[i].Value = incubus.cpu.Temperature.Values[i];
										cpuData.Children[i].MaxValue = incubus.cpu.Temperature.MaxValues[i];
									}
								}

								if (cpuData.Name == "Clocks")
								{
									for (int i = 0; i < cpuData.Children.Count; i++)
									{
										cpuData.Children[i].Value = incubus.cpu.Clocks.Values[i];
										cpuData.Children[i].MaxValue = incubus.cpu.Clocks.MaxValues[i];
									}
								}

								if (cpuData.Name == "Load")
								{
									for (int i = 0; i < cpuData.Children.Count; i++)
									{
										cpuData.Children[i].Value = incubus.cpu.Load.Values[i];
										cpuData.Children[i].MaxValue = incubus.cpu.Load.MaxValues[i];
									}
								}

								if (cpuData.Name == "Powers")
								{
									for (int i = 0; i < cpuData.Children.Count; i++)
									{
										cpuData.Children[i].Value = incubus.cpu.Powers.Values[i];
										cpuData.Children[i].MaxValue = incubus.cpu.Powers.MaxValues[i];
									}
								}
							}
						}

						if (data.Name == "Видеокарта")
						{
							data.Value = incubus.gpu.Model;
							foreach (Data gpuData in data.Children)
							{
								if (gpuData.Name == "Температура")
								{
									for (int i = 0; i < gpuData.Children.Count; i++)
									{
										gpuData.Children[i].Value = incubus.gpu.Temperature.Values[i];
										gpuData.Children[i].MaxValue = incubus.gpu.Temperature.MaxValues[i];
									}
								}

								if (gpuData.Name == "Clocks")
								{
									for (int i = 0; i < gpuData.Children.Count; i++)
									{
										gpuData.Children[i].Value = incubus.gpu.Clocks.Values[i];
										gpuData.Children[i].MaxValue = incubus.gpu.Clocks.MaxValues[i];
									}
								}

								if (gpuData.Name == "Load")
								{
									for (int i = 0; i < gpuData.Children.Count; i++)
									{
										gpuData.Children[i].Value = incubus.gpu.Load.Values[i];
										gpuData.Children[i].MaxValue = incubus.gpu.Load.MaxValues[i];
									}
								}

								if (gpuData.Name == "Fans")
								{
									for (int i = 0; i < gpuData.Children.Count; i++)
									{
										gpuData.Children[i].Value = incubus.gpu.Fans.Values[i];
										gpuData.Children[i].MaxValue = incubus.gpu.Fans.MaxValues[i];
									}
								}
							}
						}

						if (data.Name == "Жесткие диски")
						{
							for (int i = 0; i < data.Children.Count; i++)
							{
								data.Children[i].Name = incubus.disks[i].Name;
								data.Children[i].Value = incubus.disks[i].TotalFreeSpace;
								data.Children[i].MaxValue = incubus.disks[i].TotalSize;
							}
						}
					}
				}
				else
				{
					if(incubus.cpu!=null)
						ViewModel.Collection.Add(InitIncubusContext(incubus));
				}
			}

			List<Data> DeleteData = new List<Data>();
			foreach (Data data in ViewModel.Collection)
			{
				Incubus FindIncubus = IncubusList.FirstOrDefault(x => x.Name == data.Name);
				if (FindIncubus==null)
				{
					DeleteData.Add(data);
				}
			}

			foreach(Data data in DeleteData)
			{
				ViewModel.Collection.Remove(data);
			}
		}

		private void InitContext()
		{
			foreach (Incubus incubus in IncubusList)
			{
				ViewModel.Collection.Add(InitIncubusContext(incubus));
			}
		}

		private Data InitIncubusContext(Incubus incubus)
		{
			Data PC = new Data();
			PC.Name = incubus.Name;
			PC.Children = new List<Data>();

			Data MAC = new Data();
			MAC.Name = "MAC";
			MAC.Value = incubus.MAC;

			PC.Children.Add(MAC);

			if (incubus.cpu != null) {
				Data CPU = new Data();
				CPU.Name = "Процессор";
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

				if (incubus.cpu.Clocks != null)
				{
					for (int i = 0; i < incubus.cpu.Clocks.Values.Count; i++)
					{
						Data temp = new Data();
						temp.Name = "Core#" + i;
						temp.Value = incubus.cpu.Clocks.Values[i];
						temp.MaxValue = incubus.cpu.Clocks.MaxValues[i];
						CPUClocks.Children.Add(temp);
					}
				}
				else
				{
					Data temp = new Data();
					temp.Name = "Пусто";
					temp.Value = "";
					temp.MaxValue = "";
					CPUClocks.Children.Add(temp);
				}

				Data CPULoad = new Data();
				CPULoad.Name = "Load";
				CPULoad.Children = new List<Data>();

				if (incubus.cpu.Load != null)
				{
					for (int i = 0; i < incubus.cpu.Load.Values.Count; i++)
					{
						Data temp = new Data();
						temp.Name = "Core#" + i;
						temp.Value = incubus.cpu.Load.Values[i];
						temp.MaxValue = incubus.cpu.Load.MaxValues[i];
						CPULoad.Children.Add(temp);
					}
				}
				else
				{
					Data temp = new Data();
					temp.Name = "Пусто";
					temp.Value = "";
					temp.MaxValue = "";
					CPULoad.Children.Add(temp);
				}


				Data CPUPowers = new Data();
				CPUPowers.Name = "Powers";
				CPUPowers.Children = new List<Data>();

				if (incubus.cpu.Powers != null)
				{
					for (int i = 0; i < incubus.cpu.Powers.Values.Count; i++)
					{
						Data temp = new Data();
						temp.Name = "Core#" + i;
						temp.Value = incubus.cpu.Powers.Values[i];
						temp.MaxValue = incubus.cpu.Powers.MaxValues[i];
						CPUPowers.Children.Add(temp);
					}
				}
				else
				{
					Data temp = new Data();
					temp.Name = "Пусто";
					temp.Value = "";
					temp.MaxValue = "";
					CPUPowers.Children.Add(temp);
				}

				CPU.Children.Add(CPUTemp);
				CPU.Children.Add(CPUClocks);
				CPU.Children.Add(CPULoad);
				CPU.Children.Add(CPUPowers);

				PC.Children.Add(CPU);
			}


			if (incubus.gpu != null)
			{
				Data GPU = new Data();
				GPU.Name = "Видеокарта";
				GPU.Value = incubus.gpu.Model;
				GPU.Children = new List<Data>();

				Data GPUTemp = new Data();
				GPUTemp.Name = "Температура";
				GPUTemp.Children = new List<Data>();

				if (incubus.gpu.Temperature != null)
				{
					for (int i = 0; i < incubus.gpu.Temperature.Values.Count; i++)
					{
						Data temp = new Data();
						temp.Name = "Core#" + i;
						temp.Value = incubus.gpu.Temperature.Values[i];
						temp.MaxValue = incubus.gpu.Temperature.MaxValues[i];
						GPUTemp.Children.Add(temp);
					}
				}
				else
				{
					Data temp = new Data();
					temp.Name = "Пусто";
					temp.Value = "";
					temp.MaxValue = "";
					GPUTemp.Children.Add(temp);
				}

				Data GPUClocks = new Data();
				GPUClocks.Name = "Clocks";
				GPUClocks.Children = new List<Data>();

				if (incubus.gpu.Clocks != null)
				{
					for (int i = 0; i < incubus.gpu.Clocks.Values.Count; i++)
					{
						Data temp = new Data();
						temp.Name = "Core#" + i;
						temp.Value = incubus.gpu.Clocks.Values[i];
						temp.MaxValue = incubus.gpu.Clocks.MaxValues[i];
						GPUClocks.Children.Add(temp);
					}
				}
				else
				{
					Data temp = new Data();
					temp.Name = "Пусто";
					temp.Value = "";
					temp.MaxValue = "";
					GPUClocks.Children.Add(temp);
				}

				Data GPULoad = new Data();
				GPULoad.Name = "Load";
				GPULoad.Children = new List<Data>();

				if (incubus.gpu.Load != null)
				{
					for (int i = 0; i < incubus.gpu.Load.Values.Count; i++)
					{
						Data temp = new Data();
						temp.Name = "Core#" + i;
						temp.Value = incubus.gpu.Load.Values[i];
						temp.MaxValue = incubus.gpu.Load.MaxValues[i];
						GPULoad.Children.Add(temp);
					}
				}
				else
				{
					Data temp = new Data();
					temp.Name = "Пусто";
					temp.Value = "";
					temp.MaxValue = "";
					GPULoad.Children.Add(temp);
				}

				Data GPUFans = new Data();
				GPUFans.Name = "Fans";
				GPUFans.Children = new List<Data>();

				if (incubus.gpu.Fans != null)
				{
					for (int i = 0; i < incubus.gpu.Fans.Values.Count; i++)
					{
						Data temp = new Data();
						temp.Name = "Кулер#" + i;
						temp.Value = incubus.gpu.Fans.Values[i];
						temp.MaxValue = incubus.gpu.Fans.MaxValues[i];
						GPUFans.Children.Add(temp);
					}
				}
				else
				{
					Data temp = new Data();
					temp.Name = "Пусто";
					temp.Value = "";
					temp.MaxValue = "";
					GPUFans.Children.Add(temp);
				}

				GPU.Children.Add(GPUTemp);
				GPU.Children.Add(GPUClocks);
				GPU.Children.Add(GPULoad);
				GPU.Children.Add(GPUFans);

				PC.Children.Add(GPU);
			}

			if (incubus.disks != null)
			{
				Data Disks = new Data();
				Disks.Name = "Жесткие диски";
				Disks.Children = new List<Data>();

				if (incubus.disks != null)
				{
					for (int i = 0; i < incubus.disks.Count; i++)
					{
						Data temp = new Data();
						temp.Name = incubus.disks[i].Name;
						temp.Value = incubus.disks[i].TotalFreeSpace;
						temp.MaxValue = incubus.disks[i].TotalSize;
						Disks.Children.Add(temp);
					}
				}
				else
				{
					Data temp = new Data();
					temp.Name = "Пусто";
					temp.Value = "";
					temp.MaxValue = "";
					Disks.Children.Add(temp);
				}

				PC.Children.Add(Disks);
			}

			return PC;
		}
	}
}
