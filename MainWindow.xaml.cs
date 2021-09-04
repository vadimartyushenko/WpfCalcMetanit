using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Controls;

namespace WpfCalcMetanit
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		#region Private fields

		private string _leftOper = "0";

		private string _rightOper = "";

		private string _operation = "";

		#endregion

		public MainWindow()
		{

			var gradientBrush = new LinearGradientBrush();
			gradientBrush.GradientStops.Add(new GradientStop(Colors.LightGray, 0));
			gradientBrush.GradientStops.Add(new GradientStop(Colors.Blue, 1));

			this.Resources.Add("buttonGradientBrush", gradientBrush);
			
			InitializeComponent();

			swhButton.Background = (Brush)TryFindResource("buttonGradientBrush");
			escButton.Background = (Brush)TryFindResource("buttonGradientBrush");

			txtBlock.Text += _leftOper;

			foreach (UIElement childElement in LayoutRoot.Children)
			{
				if (childElement is Button)
					((Button) childElement).Click += Button_Click;
			}

			escButton.Click += EscButton_Click;
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			var s = (string) ((Button) e.OriginalSource).Content;

			//TODO: доработать для случая расчета, например, 1+3*5 
			if (s.NotIn(Constants.ClearBtnSynonim, Constants.SwitchBtnSynonim))
				txtBlock.Text = txtBlock.Text == "0" ? s : txtBlock.Text + s;

			int numberBut;
			var isNumber = Int32.TryParse(s, out numberBut);

			if (isNumber)
			{
				if (string.IsNullOrWhiteSpace(_operation))
					_leftOper = _leftOper == "0" ? s : _leftOper + s;
				else
					_rightOper += s;
			}
			else
			{
				if (s == "=")
				{
					Update_RightOper();
					txtBlock.Text += _rightOper;
					_operation = "";
				}
				else if (s == Constants.ClearBtnSynonim)
				{
					_leftOper = "";
					_rightOper = "";
					_operation = "";
					txtBlock.Text = "";
				}
				else
				{
					if (!string.IsNullOrWhiteSpace(_rightOper))
					{
						Update_RightOper();
						_leftOper = _rightOper;
						_rightOper = "";
					}

					_operation = s;
				}

			}

			//Trace.WriteLine(s);
		}

		private void Update_RightOper()
		{
			var num1 = Int32.Parse(_leftOper);
			var num2 = Int32.Parse(_rightOper);

			switch (_operation)
			{
				case "+":
					_rightOper = (num1 + num2).ToString();
					break;

				case "-":
					_rightOper = (num1 - num2).ToString();
					break;

				case "/":
					_rightOper = ((double) num1 / num2).ToString();
					break;

				case "*":
					_rightOper = (num1 * num2).ToString();
					break;
			}
		}

		private void SwhButton_OnClick(object sender, RoutedEventArgs e)
		{
			if (_operation != null)
			{
				txtBlock.Text = "";
				_operation = "";
				_rightOper = "";
			}

			_leftOper = _leftOper != null ? "-" + _leftOper : "-";
			txtBlock.Text = _leftOper;

			swhButton.Background = new SolidColorBrush(Colors.Blue);
		}

		private void EscButton_Click(object sender, RoutedEventArgs e)
		{
			Close();
		}
	}

	public static class Tools
	{
		public static bool In(this string str, params string[] items)
		{
			foreach (var i in items)
				if (str == i)
					return true;

			return false;
		}

		public static bool NotIn(this string str, params string[] items)
		{
			foreach (var i in items)
				if (str == i)
					return false;

			return true;
		}
	}

	public static class Constants
	{
		#region Названия кнопок

		public const string ClearBtnSynonim = "<CLEAR>";

		public const string SwitchBtnSynonim = "Switch";

		#endregion
	}

}
