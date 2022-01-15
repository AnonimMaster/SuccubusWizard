using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;

namespace SuccubusWizard
{

	public class IncubusData
	{
		public int Id { get; set; }
		public string MAC { get; set; }
		public string Data { get; set; }
	}
}
