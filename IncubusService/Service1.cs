using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management;
using System.Net;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Reflection.Emit;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace IncubusService
{
	public partial class Service1 : ServiceBase
	{
		private static readonly HttpClient client = new HttpClient();
		private Incubus thisIncubus;
		private readonly BackgroundWorker worker = new BackgroundWorker();
		private HardwareMonitor monitor = new HardwareMonitor();
		private bool ServerOnline = false;
		private bool IsConnect = false;

		public Service1()
		{
			InitializeComponent();
			this.CanStop = true;
			this.CanPauseAndContinue = true;
			this.AutoLog = true;

			worker.DoWork += worker_DoWork;
			worker.RunWorkerCompleted += worker_RunWorkerCompleted;
			worker.WorkerSupportsCancellation = true;

			thisIncubus = new Incubus();
			thisIncubus.Name = Dns.GetHostName();
			thisIncubus.MAC = monitor.GetMACAddress();
		}

		protected override void OnStart(string[] args)
		{
			worker.RunWorkerAsync();
			Connect();
		}

		protected override void OnStop()
		{
			Disconnect();
			worker.CancelAsync();
			Thread.Sleep(1000);
		}

		protected override void OnContinue()
		{
			worker.RunWorkerAsync();
			Connect();
		}

		protected override void OnShutdown()
		{
			Disconnect();
			worker.CancelAsync();
			Thread.Sleep(1000);
		}

		private void worker_DoWork(object sender, DoWorkEventArgs e)
		{
			while (true)
			{
				if (ServerOnline)
				{
					if (IsConnect)
					{
						Update();
						Thread.Sleep(3000);
					}
					else
					{
						Connect();
						Thread.Sleep(3000);
					}
					
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
				var result = await client.GetAsync("https://succubuswizard.azurewebsites.net/api/Incubus/GetServerStatus");
				if (result.StatusCode == HttpStatusCode.OK)
					ServerOnline = true;
			}
			catch (HttpRequestException ex)
			{
				ServerOnline = false;
				IsConnect = false;
			}
		}

		private async void Connect()
		{
			try
			{
				IncubusData incubusData = new IncubusData();
				incubusData = DataConvertor.ConvertToIncubusData(thisIncubus);
				var response = await client.PostAsJsonAsync("https://succubuswizard.azurewebsites.net/api/Incubus/ConnectIncubus", incubusData);
				ServerOnline = true;
				IsConnect = true;
				thisIncubus = await response.Content.ReadAsAsync<Incubus>();
			}
			catch (HttpRequestException ex)
			{
				ServerOnline = false;
				IsConnect = false;
			}
		}

		private async void Disconnect()
		{
			try
			{
				IncubusData incubusData = new IncubusData();
				incubusData = DataConvertor.ConvertToIncubusData(thisIncubus);
				var response = await client.PostAsJsonAsync("https://succubuswizard.azurewebsites.net/api/Incubus/DisconnectIncubus", incubusData);
			}
			catch (HttpRequestException ex)
			{
				ServerOnline = false;
				IsConnect = false;
			}
		}

		private async void Update()
		{
			try
			{
				UpdateStatePC();
				IncubusData incubusData = new IncubusData();
				incubusData = DataConvertor.ConvertToIncubusData(thisIncubus);
				var response = await client.PostAsJsonAsync("https://succubuswizard.azurewebsites.net/api/Incubus/UpdateIncubus", incubusData);
				var responseString = await response.Content.ReadAsStringAsync();
			}
			catch (HttpRequestException ex)
			{
				ServerOnline = false;
				IsConnect = false;
			}
		}

		private void UpdateStatePC()
		{
			thisIncubus.Name = Dns.GetHostName();
			thisIncubus.cpu = monitor.GetCPU();
			thisIncubus.gpu = monitor.GetGPU();
			thisIncubus.disks = monitor.GetDisks();
		}
	}
}
