using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Shapes;

namespace System.Windows.Controls
{
	/// <summary>Represents a converter that converts the dimensions of a <see cref="T:System.Windows.Controls.GroupBox" /> control into a <see cref="T:System.Windows.Media.VisualBrush" />.</summary>
	// Token: 0x02000473 RID: 1139
	public class BorderGapMaskConverter : IMultiValueConverter
	{
		/// <summary>Creates a <see cref="T:System.Windows.Media.VisualBrush" /> that draws the border for a <see cref="T:System.Windows.Controls.GroupBox" /> control.</summary>
		/// <param name="values">An array of three numbers that represent the <see cref="T:System.Windows.Controls.GroupBox" /> control parameters.  See Remarks for more information.</param>
		/// <param name="targetType">This parameter is not used.</param>
		/// <param name="parameter">The width of the visible line to the left of the <see cref="P:System.Windows.Controls.HeaderedContentControl.Header" /> in the <see cref="T:System.Windows.Controls.GroupBox" />.</param>
		/// <param name="culture">This parameter is not used.</param>
		/// <returns>A <see cref="T:System.Windows.Media.VisualBrush" /> that draws the border around a <see cref="T:System.Windows.Controls.GroupBox" /> control that includes a gap for the <see cref="P:System.Windows.Controls.HeaderedContentControl.Header" /> content.</returns>
		// Token: 0x0600427D RID: 17021 RVA: 0x00130A48 File Offset: 0x0012EC48
		public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
		{
			Type typeFromHandle = typeof(double);
			if (parameter == null || values == null || values.Length != 3 || values[0] == null || values[1] == null || values[2] == null || !typeFromHandle.IsAssignableFrom(values[0].GetType()) || !typeFromHandle.IsAssignableFrom(values[1].GetType()) || !typeFromHandle.IsAssignableFrom(values[2].GetType()))
			{
				return DependencyProperty.UnsetValue;
			}
			Type type = parameter.GetType();
			if (!typeFromHandle.IsAssignableFrom(type) && !typeof(string).IsAssignableFrom(type))
			{
				return DependencyProperty.UnsetValue;
			}
			double pixels = (double)values[0];
			double num = (double)values[1];
			double num2 = (double)values[2];
			if (num == 0.0 || num2 == 0.0)
			{
				return null;
			}
			double pixels2;
			if (parameter is string)
			{
				pixels2 = double.Parse((string)parameter, NumberFormatInfo.InvariantInfo);
			}
			else
			{
				pixels2 = (double)parameter;
			}
			Grid grid = new Grid();
			grid.Width = num;
			grid.Height = num2;
			ColumnDefinition columnDefinition = new ColumnDefinition();
			ColumnDefinition columnDefinition2 = new ColumnDefinition();
			ColumnDefinition columnDefinition3 = new ColumnDefinition();
			columnDefinition.Width = new GridLength(pixels2);
			columnDefinition2.Width = new GridLength(pixels);
			columnDefinition3.Width = new GridLength(1.0, GridUnitType.Star);
			grid.ColumnDefinitions.Add(columnDefinition);
			grid.ColumnDefinitions.Add(columnDefinition2);
			grid.ColumnDefinitions.Add(columnDefinition3);
			RowDefinition rowDefinition = new RowDefinition();
			RowDefinition rowDefinition2 = new RowDefinition();
			rowDefinition.Height = new GridLength(num2 / 2.0);
			rowDefinition2.Height = new GridLength(1.0, GridUnitType.Star);
			grid.RowDefinitions.Add(rowDefinition);
			grid.RowDefinitions.Add(rowDefinition2);
			Rectangle rectangle = new Rectangle();
			Rectangle rectangle2 = new Rectangle();
			Rectangle rectangle3 = new Rectangle();
			rectangle.Fill = Brushes.Black;
			rectangle2.Fill = Brushes.Black;
			rectangle3.Fill = Brushes.Black;
			Grid.SetRowSpan(rectangle, 2);
			Grid.SetRow(rectangle, 0);
			Grid.SetColumn(rectangle, 0);
			Grid.SetRow(rectangle2, 1);
			Grid.SetColumn(rectangle2, 1);
			Grid.SetRowSpan(rectangle3, 2);
			Grid.SetRow(rectangle3, 0);
			Grid.SetColumn(rectangle3, 2);
			grid.Children.Add(rectangle);
			grid.Children.Add(rectangle2);
			grid.Children.Add(rectangle3);
			return new VisualBrush(grid);
		}

		/// <summary>Not implemented.</summary>
		/// <param name="value">This parameter is not used.</param>
		/// <param name="targetTypes">This parameter is not used.</param>
		/// <param name="parameter">This parameter is not used.</param>
		/// <param name="culture">This parameter is not used.</param>
		/// <returns>
		///     <see cref="F:System.Windows.Data.Binding.DoNothing" /> in all cases.</returns>
		// Token: 0x0600427E RID: 17022 RVA: 0x000BE07B File Offset: 0x000BC27B
		public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
		{
			return new object[]
			{
				Binding.DoNothing
			};
		}
	}
}
