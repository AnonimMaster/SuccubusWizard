using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace SuccubusClient
{
	class MainViewModel
	{
		public ObservableCollection<Data> Collection { get; set; }
	}

	public class Data : INotifyPropertyChanged
	{
		public string _Name;
		public string Name
		{
			get => _Name;
			set
			{
				_Name = value;
				OnPropertyChanged(nameof(Name));
			}
		}
		public string _Value;
		public string Value
		{
			get => _Value;
			set
			{
				_Value = value;
				OnPropertyChanged(nameof(Value));
			}
		}
		public string _MaxValue;
		public string MaxValue
		{
			get => _MaxValue;
			set
			{
				_MaxValue = value;
				OnPropertyChanged(nameof(MaxValue));
			}
		}
		public List<Data> _Children;
		public List<Data> Children
		{
			get => _Children;
			set
			{
				_Children = value;
				OnPropertyChanged(nameof(Children));
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;

		protected virtual void OnPropertyChanged(string propertyName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
