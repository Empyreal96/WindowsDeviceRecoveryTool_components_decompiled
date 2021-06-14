using System;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using MS.Internal;

namespace System.Windows.Controls
{
	// Token: 0x020004A3 RID: 1187
	[TemplatePart(Name = "PART_VisualBrushCanvas", Type = typeof(Canvas))]
	internal class DataGridColumnFloatingHeader : Control
	{
		// Token: 0x0600487A RID: 18554 RVA: 0x00149A7C File Offset: 0x00147C7C
		static DataGridColumnFloatingHeader()
		{
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(DataGridColumnFloatingHeader), new FrameworkPropertyMetadata(DataGridColumnHeader.ColumnFloatingHeaderStyleKey));
			FrameworkElement.WidthProperty.OverrideMetadata(typeof(DataGridColumnFloatingHeader), new FrameworkPropertyMetadata(new PropertyChangedCallback(DataGridColumnFloatingHeader.OnWidthChanged), new CoerceValueCallback(DataGridColumnFloatingHeader.OnCoerceWidth)));
			FrameworkElement.HeightProperty.OverrideMetadata(typeof(DataGridColumnFloatingHeader), new FrameworkPropertyMetadata(new PropertyChangedCallback(DataGridColumnFloatingHeader.OnHeightChanged), new CoerceValueCallback(DataGridColumnFloatingHeader.OnCoerceHeight)));
		}

		// Token: 0x0600487B RID: 18555 RVA: 0x00149B0C File Offset: 0x00147D0C
		private static void OnWidthChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			DataGridColumnFloatingHeader dataGridColumnFloatingHeader = (DataGridColumnFloatingHeader)d;
			double num = (double)e.NewValue;
			if (dataGridColumnFloatingHeader._visualBrushCanvas != null && !DoubleUtil.IsNaN(num))
			{
				VisualBrush visualBrush = dataGridColumnFloatingHeader._visualBrushCanvas.Background as VisualBrush;
				if (visualBrush != null)
				{
					Rect viewbox = visualBrush.Viewbox;
					visualBrush.Viewbox = new Rect(viewbox.X, viewbox.Y, num - dataGridColumnFloatingHeader.GetVisualCanvasMarginX(), viewbox.Height);
				}
			}
		}

		// Token: 0x0600487C RID: 18556 RVA: 0x00149B80 File Offset: 0x00147D80
		private static object OnCoerceWidth(DependencyObject d, object baseValue)
		{
			double value = (double)baseValue;
			DataGridColumnFloatingHeader dataGridColumnFloatingHeader = (DataGridColumnFloatingHeader)d;
			if (dataGridColumnFloatingHeader._referenceHeader != null && DoubleUtil.IsNaN(value))
			{
				return dataGridColumnFloatingHeader._referenceHeader.ActualWidth + dataGridColumnFloatingHeader.GetVisualCanvasMarginX();
			}
			return baseValue;
		}

		// Token: 0x0600487D RID: 18557 RVA: 0x00149BC4 File Offset: 0x00147DC4
		private static void OnHeightChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			DataGridColumnFloatingHeader dataGridColumnFloatingHeader = (DataGridColumnFloatingHeader)d;
			double num = (double)e.NewValue;
			if (dataGridColumnFloatingHeader._visualBrushCanvas != null && !DoubleUtil.IsNaN(num))
			{
				VisualBrush visualBrush = dataGridColumnFloatingHeader._visualBrushCanvas.Background as VisualBrush;
				if (visualBrush != null)
				{
					Rect viewbox = visualBrush.Viewbox;
					visualBrush.Viewbox = new Rect(viewbox.X, viewbox.Y, viewbox.Width, num - dataGridColumnFloatingHeader.GetVisualCanvasMarginY());
				}
			}
		}

		// Token: 0x0600487E RID: 18558 RVA: 0x00149C38 File Offset: 0x00147E38
		private static object OnCoerceHeight(DependencyObject d, object baseValue)
		{
			double value = (double)baseValue;
			DataGridColumnFloatingHeader dataGridColumnFloatingHeader = (DataGridColumnFloatingHeader)d;
			if (dataGridColumnFloatingHeader._referenceHeader != null && DoubleUtil.IsNaN(value))
			{
				return dataGridColumnFloatingHeader._referenceHeader.ActualHeight + dataGridColumnFloatingHeader.GetVisualCanvasMarginY();
			}
			return baseValue;
		}

		// Token: 0x0600487F RID: 18559 RVA: 0x00149C7C File Offset: 0x00147E7C
		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			this._visualBrushCanvas = (base.GetTemplateChild("PART_VisualBrushCanvas") as Canvas);
			this.UpdateVisualBrush();
		}

		// Token: 0x170011B3 RID: 4531
		// (get) Token: 0x06004880 RID: 18560 RVA: 0x00149CA0 File Offset: 0x00147EA0
		// (set) Token: 0x06004881 RID: 18561 RVA: 0x00149CA8 File Offset: 0x00147EA8
		internal DataGridColumnHeader ReferenceHeader
		{
			get
			{
				return this._referenceHeader;
			}
			set
			{
				this._referenceHeader = value;
			}
		}

		// Token: 0x06004882 RID: 18562 RVA: 0x00149CB4 File Offset: 0x00147EB4
		private void UpdateVisualBrush()
		{
			if (this._referenceHeader != null && this._visualBrushCanvas != null)
			{
				VisualBrush visualBrush = new VisualBrush(this._referenceHeader);
				visualBrush.ViewboxUnits = BrushMappingMode.Absolute;
				double num = base.Width;
				if (DoubleUtil.IsNaN(num))
				{
					num = this._referenceHeader.ActualWidth;
				}
				else
				{
					num -= this.GetVisualCanvasMarginX();
				}
				double num2 = base.Height;
				if (DoubleUtil.IsNaN(num2))
				{
					num2 = this._referenceHeader.ActualHeight;
				}
				else
				{
					num2 -= this.GetVisualCanvasMarginY();
				}
				Vector offset = VisualTreeHelper.GetOffset(this._referenceHeader);
				visualBrush.Viewbox = new Rect(offset.X, offset.Y, num, num2);
				this._visualBrushCanvas.Background = visualBrush;
			}
		}

		// Token: 0x06004883 RID: 18563 RVA: 0x00149D69 File Offset: 0x00147F69
		internal void ClearHeader()
		{
			this._referenceHeader = null;
			if (this._visualBrushCanvas != null)
			{
				this._visualBrushCanvas.Background = null;
			}
		}

		// Token: 0x06004884 RID: 18564 RVA: 0x00149D88 File Offset: 0x00147F88
		private double GetVisualCanvasMarginX()
		{
			double num = 0.0;
			if (this._visualBrushCanvas != null)
			{
				Thickness margin = this._visualBrushCanvas.Margin;
				num += margin.Left;
				num += margin.Right;
			}
			return num;
		}

		// Token: 0x06004885 RID: 18565 RVA: 0x00149DC8 File Offset: 0x00147FC8
		private double GetVisualCanvasMarginY()
		{
			double num = 0.0;
			if (this._visualBrushCanvas != null)
			{
				Thickness margin = this._visualBrushCanvas.Margin;
				num += margin.Top;
				num += margin.Bottom;
			}
			return num;
		}

		// Token: 0x0400299B RID: 10651
		private DataGridColumnHeader _referenceHeader;

		// Token: 0x0400299C RID: 10652
		private const string VisualBrushCanvasTemplateName = "PART_VisualBrushCanvas";

		// Token: 0x0400299D RID: 10653
		private Canvas _visualBrushCanvas;
	}
}
