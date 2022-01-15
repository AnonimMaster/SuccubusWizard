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
		private static MainViewModel ViewModel = new MainViewModel(IncubusList);

		public MainWindow()
		{
			InitializeComponent();
			worker.DoWork += worker_DoWork;
			worker.RunWorkerCompleted += worker_RunWorkerCompleted;
			worker.WorkerSupportsCancellation = true;

			this.DataContext = ViewModel;

			Connect();
		}

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			//worker.RunWorkerAsync();
		}

		private void Window_Closing(object sender, CancelEventArgs e)
		{
			//worker.CancelAsync();
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

		private void worker_RunWorkerCompleted(object sender,
												   RunWorkerCompletedEventArgs e)
		{

		}

		private async void Ping()
		{
			try
			{
				var result = await client.GetAsync("https://succubuswizard.azurewebsites.net/api/Incubus/GetIncubusList");
				ServerOnline = true;
				//Вызывают методом BeginInvoke в основной поток где был создан ServerLabel, что иметь доступ к нему. Иначе пиздец
				this.ServerLabel.BeginInvoke((Action)(() => this.ServerLabel.Content = "Server Online"));
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
				var incubusresult = await result.Content.ReadAsAsync<IEnumerable<Incubus>>();

				IncubusList = incubusresult.ToList();

				MainViewModel newMainViewModel = new MainViewModel(IncubusList);
				this.TESTLABEL.BeginInvoke((Action)(() => this.TESTLABEL.Content = IncubusList.Count));
				ViewModel = newMainViewModel;
			}
			catch (HttpRequestException ex)
			{
				ServerOnline = false;
			}
		}

		private async void Connect()
		{
			Incubus incubus = new Incubus();
			incubus.Name = "Test INCUBUS SUKA";
			incubus.MAC = "ekfoekf";
			incubus.gpu = new GPU()
			{
				Temperature = new ValueContainer()
				{
					Values = new List<string>()
					{
						"PIZDEC ЖАРКО"
					},
					MaxValues = new List<string>()
					{
						"REALNO"
					}
				}
			};
			IncubusData data = DataConvertor.ConvertToIncubusData(incubus);
			var response = await client.PostAsJsonAsync("https://succubuswizard.azurewebsites.net/api/Incubus/ConnectIncubus", data);
			IncubusData responeData = await response.Content.ReadAsAsync<IncubusData>();
			Incubus thisIncubus = new Incubus();
			thisIncubus = DataConvertor.ConvertToIncubus(responeData);
			MessageBox.Show(""+thisIncubus.Id);
		}
	}
}
