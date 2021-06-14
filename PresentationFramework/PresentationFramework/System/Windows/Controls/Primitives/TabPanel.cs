using System;
using System.Windows.Input;
using System.Windows.Media;
using MS.Internal;

namespace System.Windows.Controls.Primitives
{
	/// <summary>Handles the layout of the <see cref="T:System.Windows.Controls.TabItem" /> objects on a <see cref="T:System.Windows.Controls.TabControl" />. </summary>
	// Token: 0x020005AB RID: 1451
	public class TabPanel : Panel
	{
		// Token: 0x0600602A RID: 24618 RVA: 0x001AF7AB File Offset: 0x001AD9AB
		static TabPanel()
		{
			KeyboardNavigation.TabNavigationProperty.OverrideMetadata(typeof(TabPanel), new FrameworkPropertyMetadata(KeyboardNavigationMode.Once));
			KeyboardNavigation.DirectionalNavigationProperty.OverrideMetadata(typeof(TabPanel), new FrameworkPropertyMetadata(KeyboardNavigationMode.Cycle));
		}

		/// <summary>Called when remeasuring the control is required. </summary>
		/// <param name="constraint">Constraint size is an upper limit. The return value should not exceed this size.</param>
		/// <returns>The desired size.</returns>
		// Token: 0x0600602B RID: 24619 RVA: 0x001AF7EC File Offset: 0x001AD9EC
		protected override Size MeasureOverride(Size constraint)
		{
			Size result = default(Size);
			Dock tabStripPlacement = this.TabStripPlacement;
			this._numRows = 1;
			this._numHeaders = 0;
			this._rowHeight = 0.0;
			if (tabStripPlacement == Dock.Top || tabStripPlacement == Dock.Bottom)
			{
				int num = 0;
				double num2 = 0.0;
				double num3 = 0.0;
				foreach (object obj in base.InternalChildren)
				{
					UIElement uielement = (UIElement)obj;
					if (uielement.Visibility != Visibility.Collapsed)
					{
						this._numHeaders++;
						uielement.Measure(constraint);
						Size desiredSizeWithoutMargin = this.GetDesiredSizeWithoutMargin(uielement);
						if (this._rowHeight < desiredSizeWithoutMargin.Height)
						{
							this._rowHeight = desiredSizeWithoutMargin.Height;
						}
						if (num2 + desiredSizeWithoutMargin.Width > constraint.Width && num > 0)
						{
							if (num3 < num2)
							{
								num3 = num2;
							}
							num2 = desiredSizeWithoutMargin.Width;
							num = 1;
							this._numRows++;
						}
						else
						{
							num2 += desiredSizeWithoutMargin.Width;
							num++;
						}
					}
				}
				if (num3 < num2)
				{
					num3 = num2;
				}
				result.Height = this._rowHeight * (double)this._numRows;
				if (double.IsInfinity(result.Width) || DoubleUtil.IsNaN(result.Width) || num3 < constraint.Width)
				{
					result.Width = num3;
				}
				else
				{
					result.Width = constraint.Width;
				}
			}
			else if (tabStripPlacement == Dock.Left || tabStripPlacement == Dock.Right)
			{
				foreach (object obj2 in base.InternalChildren)
				{
					UIElement uielement2 = (UIElement)obj2;
					if (uielement2.Visibility != Visibility.Collapsed)
					{
						this._numHeaders++;
						uielement2.Measure(constraint);
						Size desiredSizeWithoutMargin2 = this.GetDesiredSizeWithoutMargin(uielement2);
						if (result.Width < desiredSizeWithoutMargin2.Width)
						{
							result.Width = desiredSizeWithoutMargin2.Width;
						}
						result.Height += desiredSizeWithoutMargin2.Height;
					}
				}
			}
			return result;
		}

		/// <summary>Arranges and sizes the content of a <see cref="T:System.Windows.Controls.Primitives.TabPanel" /> object. </summary>
		/// <param name="arrangeSize">The size that a tab panel assumes to position child elements.</param>
		/// <returns>The size of the tab panel.</returns>
		// Token: 0x0600602C RID: 24620 RVA: 0x001AFA40 File Offset: 0x001ADC40
		protected override Size ArrangeOverride(Size arrangeSize)
		{
			Dock tabStripPlacement = this.TabStripPlacement;
			if (tabStripPlacement == Dock.Top || tabStripPlacement == Dock.Bottom)
			{
				this.ArrangeHorizontal(arrangeSize);
			}
			else if (tabStripPlacement == Dock.Left || tabStripPlacement == Dock.Right)
			{
				this.ArrangeVertical(arrangeSize);
			}
			return arrangeSize;
		}

		/// <summary>Used to override default clipping.</summary>
		/// <param name="layoutSlotSize">The size of the panel.</param>
		/// <returns>A size that is the layout size of the <see cref="T:System.Windows.Controls.Primitives.TabPanel" />.</returns>
		// Token: 0x0600602D RID: 24621 RVA: 0x0000C238 File Offset: 0x0000A438
		protected override Geometry GetLayoutClip(Size layoutSlotSize)
		{
			return null;
		}

		// Token: 0x0600602E RID: 24622 RVA: 0x001AFA74 File Offset: 0x001ADC74
		private Size GetDesiredSizeWithoutMargin(UIElement element)
		{
			Thickness thickness = (Thickness)element.GetValue(FrameworkElement.MarginProperty);
			return new Size
			{
				Height = Math.Max(0.0, element.DesiredSize.Height - thickness.Top - thickness.Bottom),
				Width = Math.Max(0.0, element.DesiredSize.Width - thickness.Left - thickness.Right)
			};
		}

		// Token: 0x0600602F RID: 24623 RVA: 0x001AFB04 File Offset: 0x001ADD04
		private double[] GetHeadersSize()
		{
			double[] array = new double[this._numHeaders];
			int num = 0;
			foreach (object obj in base.InternalChildren)
			{
				UIElement uielement = (UIElement)obj;
				if (uielement.Visibility != Visibility.Collapsed)
				{
					array[num] = this.GetDesiredSizeWithoutMargin(uielement).Width;
					num++;
				}
			}
			return array;
		}

		// Token: 0x06006030 RID: 24624 RVA: 0x001AFB88 File Offset: 0x001ADD88
		private void ArrangeHorizontal(Size arrangeSize)
		{
			Dock tabStripPlacement = this.TabStripPlacement;
			bool flag = this._numRows > 1;
			int num = 0;
			int[] array = new int[0];
			Vector vector = default(Vector);
			double[] headersSize = this.GetHeadersSize();
			if (flag)
			{
				array = this.CalculateHeaderDistribution(arrangeSize.Width, headersSize);
				num = this.GetActiveRow(array);
				if (tabStripPlacement == Dock.Top)
				{
					vector.Y = (double)(this._numRows - 1 - num) * this._rowHeight;
				}
				if (tabStripPlacement == Dock.Bottom && num != 0)
				{
					vector.Y = (double)(this._numRows - num) * this._rowHeight;
				}
			}
			int num2 = 0;
			int num3 = 0;
			foreach (object obj in base.InternalChildren)
			{
				UIElement uielement = (UIElement)obj;
				if (uielement.Visibility != Visibility.Collapsed)
				{
					Thickness thickness = (Thickness)uielement.GetValue(FrameworkElement.MarginProperty);
					double left = thickness.Left;
					double right = thickness.Right;
					double top = thickness.Top;
					double bottom = thickness.Bottom;
					bool flag2 = flag && ((num3 < array.Length && array[num3] == num2) || num2 == this._numHeaders - 1);
					Size size = new Size(headersSize[num2], this._rowHeight);
					if (flag2)
					{
						size.Width = arrangeSize.Width - vector.X;
					}
					uielement.Arrange(new Rect(vector.X, vector.Y, size.Width, size.Height));
					Size size2 = size;
					size2.Height = Math.Max(0.0, size2.Height - top - bottom);
					size2.Width = Math.Max(0.0, size2.Width - left - right);
					vector.X += size.Width;
					if (flag2)
					{
						if ((num3 == num && tabStripPlacement == Dock.Top) || (num3 == num - 1 && tabStripPlacement == Dock.Bottom))
						{
							vector.Y = 0.0;
						}
						else
						{
							vector.Y += this._rowHeight;
						}
						vector.X = 0.0;
						num3++;
					}
					num2++;
				}
			}
		}

		// Token: 0x06006031 RID: 24625 RVA: 0x001AFDF0 File Offset: 0x001ADFF0
		private void ArrangeVertical(Size arrangeSize)
		{
			double num = 0.0;
			foreach (object obj in base.InternalChildren)
			{
				UIElement uielement = (UIElement)obj;
				if (uielement.Visibility != Visibility.Collapsed)
				{
					Size desiredSizeWithoutMargin = this.GetDesiredSizeWithoutMargin(uielement);
					uielement.Arrange(new Rect(0.0, num, arrangeSize.Width, desiredSizeWithoutMargin.Height));
					num += desiredSizeWithoutMargin.Height;
				}
			}
		}

		// Token: 0x06006032 RID: 24626 RVA: 0x001AFE90 File Offset: 0x001AE090
		private int GetActiveRow(int[] solution)
		{
			int num = 0;
			int num2 = 0;
			if (solution.Length != 0)
			{
				foreach (object obj in base.InternalChildren)
				{
					UIElement uielement = (UIElement)obj;
					if (uielement.Visibility != Visibility.Collapsed)
					{
						bool flag = (bool)uielement.GetValue(Selector.IsSelectedProperty);
						if (flag)
						{
							return num;
						}
						if (num < solution.Length && solution[num] == num2)
						{
							num++;
						}
						num2++;
					}
				}
			}
			if (this.TabStripPlacement == Dock.Top)
			{
				num = this._numRows - 1;
			}
			return num;
		}

		// Token: 0x06006033 RID: 24627 RVA: 0x001AFF3C File Offset: 0x001AE13C
		private int[] CalculateHeaderDistribution(double rowWidthLimit, double[] headerWidth)
		{
			double num = 0.0;
			int num2 = headerWidth.Length;
			int num3 = this._numRows - 1;
			double num4 = 0.0;
			int num5 = 0;
			int[] array = new int[num3];
			int[] array2 = new int[num3];
			int[] array3 = new int[this._numRows];
			double[] array4 = new double[this._numRows];
			double[] array5 = new double[this._numRows];
			double[] array6 = new double[this._numRows];
			int num6 = 0;
			double num7;
			for (int i = 0; i < num2; i++)
			{
				if (num4 + headerWidth[i] > rowWidthLimit && num5 > 0)
				{
					array4[num6] = num4;
					array3[num6] = num5;
					num7 = Math.Max(0.0, (rowWidthLimit - num4) / (double)num5);
					array5[num6] = num7;
					array[num6] = i - 1;
					if (num < num7)
					{
						num = num7;
					}
					num6++;
					num4 = headerWidth[i];
					num5 = 1;
				}
				else
				{
					num4 += headerWidth[i];
					if (headerWidth[i] != 0.0)
					{
						num5++;
					}
				}
			}
			if (num6 == 0)
			{
				return new int[0];
			}
			array4[num6] = num4;
			array3[num6] = num5;
			num7 = (rowWidthLimit - num4) / (double)num5;
			array5[num6] = num7;
			if (num < num7)
			{
				num = num7;
			}
			array.CopyTo(array2, 0);
			array5.CopyTo(array6, 0);
			for (;;)
			{
				int num8 = 0;
				double num9 = 0.0;
				for (int j = 0; j < this._numRows; j++)
				{
					if (num9 < array5[j])
					{
						num9 = array5[j];
						num8 = j;
					}
				}
				if (num8 == 0)
				{
					break;
				}
				int num10 = num8;
				int num11 = num10 - 1;
				int num12 = array[num11];
				double num13 = headerWidth[num12];
				array4[num10] += num13;
				if (array4[num10] > rowWidthLimit)
				{
					break;
				}
				array[num11]--;
				array3[num10]++;
				array4[num11] -= num13;
				array3[num11]--;
				array5[num11] = (rowWidthLimit - array4[num11]) / (double)array3[num11];
				array5[num10] = (rowWidthLimit - array4[num10]) / (double)array3[num10];
				num9 = 0.0;
				for (int k = 0; k < this._numRows; k++)
				{
					if (num9 < array5[k])
					{
						num9 = array5[k];
					}
				}
				if (num9 < num)
				{
					num = num9;
					array.CopyTo(array2, 0);
					array5.CopyTo(array6, 0);
				}
			}
			num6 = 0;
			for (int l = 0; l < num2; l++)
			{
				headerWidth[l] += array6[num6];
				if (num6 < num3 && array2[num6] == l)
				{
					num6++;
				}
			}
			return array2;
		}

		// Token: 0x17001722 RID: 5922
		// (get) Token: 0x06006034 RID: 24628 RVA: 0x001B01E8 File Offset: 0x001AE3E8
		private Dock TabStripPlacement
		{
			get
			{
				Dock result = Dock.Top;
				TabControl tabControl = base.TemplatedParent as TabControl;
				if (tabControl != null)
				{
					result = tabControl.TabStripPlacement;
				}
				return result;
			}
		}

		// Token: 0x040030F1 RID: 12529
		private int _numRows = 1;

		// Token: 0x040030F2 RID: 12530
		private int _numHeaders;

		// Token: 0x040030F3 RID: 12531
		private double _rowHeight;
	}
}
