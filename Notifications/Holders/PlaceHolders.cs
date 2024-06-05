using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Media;

namespace Notifications.Holders
{
	public static class TextBoxHelper
	{
		public static readonly DependencyProperty PlaceholderProperty =
			DependencyProperty.RegisterAttached("Placeholder", typeof(string), typeof(TextBoxHelper), new PropertyMetadata(null, OnPlaceholderChanged));

		public static string GetPlaceholder(DependencyObject obj)
		{
			return (string)obj.GetValue(PlaceholderProperty);
		}

		public static void SetPlaceholder(DependencyObject obj, string value)
		{
			obj.SetValue(PlaceholderProperty, value);
		}

		private static void OnPlaceholderChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			if (d is TextBox textBox)
			{
				textBox.GotFocus += TextBox_GotFocus;
				textBox.LostFocus += TextBox_LostFocus;
				UpdatePlaceholder(textBox);
			}
		}

		private static void TextBox_GotFocus(object sender, RoutedEventArgs e)
		{
			UpdatePlaceholder(sender as TextBox);
		}

		private static void TextBox_LostFocus(object sender, RoutedEventArgs e)
		{
			UpdatePlaceholder(sender as TextBox);
		}

		private static void UpdatePlaceholder(TextBox textBox)
		{
			if (textBox == null)
				return;

			if (string.IsNullOrEmpty(textBox.Text))
			{
				textBox.Text = GetPlaceholder(textBox);
				textBox.Foreground = System.Windows.Media.Brushes.LightGray;
			}
			else if (textBox.Text == GetPlaceholder(textBox))
			{
				textBox.Text = string.Empty;
				textBox.Foreground = System.Windows.Media.Brushes.Black;
			}
		}
	}
}

