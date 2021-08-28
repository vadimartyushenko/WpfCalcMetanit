using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

namespace WpfCalcMetanit
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private string _leftOper = "";

		private string _rightOper = "";

		private string _operation = "";

		public MainWindow()
		{
			InitializeComponent();

			foreach (UIElement childElement in LayoutRoot.Children)
			{
				if (childElement is Button)
					((Button)childElement).Click += Button_Click;
			}
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			var s = (string)((Button) e.OriginalSource).Content;

			txtBlock.Text += s;

			int numberBut;
			var isNumber = Int32.TryParse(s, out numberBut);

			if (isNumber)
			{
				if (string.IsNullOrWhiteSpace(_operation))
					_leftOper += s;
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
				else if (s == "CLEAR")
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
					_rightOper = ((double)num1 / num2).ToString();
					break;

				case "*":
					_rightOper = (num1 * num2).ToString();
					break;
			}
		}
	}
}
