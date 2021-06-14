using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using MS.Internal;
using MS.Internal.Controls;
using MS.Utility;

namespace System.Windows.Controls
{
	/// <summary>Arranges and virtualizes content on a single line that is oriented either horizontally or vertically.</summary>
	// Token: 0x0200055E RID: 1374
	public class VirtualizingStackPanel : VirtualizingPanel, IScrollInfo, IStackMeasure
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Controls.VirtualizingStackPanel" /> class.</summary>
		// Token: 0x06005A3C RID: 23100 RVA: 0x0018DC60 File Offset: 0x0018BE60
		public VirtualizingStackPanel()
		{
			base.IsVisibleChanged += this.OnIsVisibleChanged;
		}

		// Token: 0x06005A3D RID: 23101 RVA: 0x0018DC7C File Offset: 0x0018BE7C
		static VirtualizingStackPanel()
		{
			object synchronized = DependencyProperty.Synchronized;
			lock (synchronized)
			{
				VirtualizingStackPanel._indicesStoredInItemValueStorage = new int[]
				{
					VirtualizingStackPanel.ContainerSizeProperty.GlobalIndex,
					VirtualizingStackPanel.ContainerSizeDualProperty.GlobalIndex,
					VirtualizingStackPanel.AreContainersUniformlySizedProperty.GlobalIndex,
					VirtualizingStackPanel.UniformOrAverageContainerSizeProperty.GlobalIndex,
					VirtualizingStackPanel.UniformOrAverageContainerSizeDualProperty.GlobalIndex,
					VirtualizingStackPanel.ItemsHostInsetProperty.GlobalIndex
				};
			}
		}

		/// <summary>Scrolls content upward by one logical unit.</summary>
		// Token: 0x06005A3E RID: 23102 RVA: 0x0018DEBC File Offset: 0x0018C0BC
		public virtual void LineUp()
		{
			if (VirtualizingStackPanel.ScrollTracer.IsEnabled && VirtualizingStackPanel.ScrollTracer.IsTracing(this))
			{
				VirtualizingStackPanel.ScrollTracer.Trace(this, VirtualizingStackPanel.ScrollTraceOp.LineUp, new object[0]);
			}
			bool flag = this.Orientation == Orientation.Horizontal;
			double offset = (this.IsPixelBased || flag) ? (this.VerticalOffset - 16.0) : this.NewItemOffset(flag, -1.0, true);
			this.SetVerticalOffsetImpl(offset, true);
		}

		/// <summary>Scrolls content downward by one logical unit.</summary>
		// Token: 0x06005A3F RID: 23103 RVA: 0x0018DF28 File Offset: 0x0018C128
		public virtual void LineDown()
		{
			if (VirtualizingStackPanel.ScrollTracer.IsEnabled && VirtualizingStackPanel.ScrollTracer.IsTracing(this))
			{
				VirtualizingStackPanel.ScrollTracer.Trace(this, VirtualizingStackPanel.ScrollTraceOp.LineDown, new object[0]);
			}
			bool flag = this.Orientation == Orientation.Horizontal;
			double offset = (this.IsPixelBased || flag) ? (this.VerticalOffset + 16.0) : this.NewItemOffset(flag, 1.0, false);
			this.SetVerticalOffsetImpl(offset, true);
		}

		/// <summary>Scrolls content to the left by one logical unit.</summary>
		// Token: 0x06005A40 RID: 23104 RVA: 0x0018DF94 File Offset: 0x0018C194
		public virtual void LineLeft()
		{
			if (VirtualizingStackPanel.ScrollTracer.IsEnabled && VirtualizingStackPanel.ScrollTracer.IsTracing(this))
			{
				VirtualizingStackPanel.ScrollTracer.Trace(this, VirtualizingStackPanel.ScrollTraceOp.LineLeft, new object[0]);
			}
			bool flag = this.Orientation == Orientation.Horizontal;
			double offset = (this.IsPixelBased || !flag) ? (this.HorizontalOffset - 16.0) : this.NewItemOffset(flag, -1.0, true);
			this.SetHorizontalOffsetImpl(offset, true);
		}

		/// <summary>Scrolls content to the right by one logical unit.</summary>
		// Token: 0x06005A41 RID: 23105 RVA: 0x0018E000 File Offset: 0x0018C200
		public virtual void LineRight()
		{
			if (VirtualizingStackPanel.ScrollTracer.IsEnabled && VirtualizingStackPanel.ScrollTracer.IsTracing(this))
			{
				VirtualizingStackPanel.ScrollTracer.Trace(this, VirtualizingStackPanel.ScrollTraceOp.LineRight, new object[0]);
			}
			bool flag = this.Orientation == Orientation.Horizontal;
			double offset = (this.IsPixelBased || !flag) ? (this.HorizontalOffset + 16.0) : this.NewItemOffset(flag, 1.0, false);
			this.SetHorizontalOffsetImpl(offset, true);
		}

		/// <summary>Scrolls content upward by one page.</summary>
		// Token: 0x06005A42 RID: 23106 RVA: 0x0018E06C File Offset: 0x0018C26C
		public virtual void PageUp()
		{
			if (VirtualizingStackPanel.ScrollTracer.IsEnabled && VirtualizingStackPanel.ScrollTracer.IsTracing(this))
			{
				VirtualizingStackPanel.ScrollTracer.Trace(this, VirtualizingStackPanel.ScrollTraceOp.PageUp, new object[0]);
			}
			bool flag = this.Orientation == Orientation.Horizontal;
			double offset = (this.IsPixelBased || flag) ? (this.VerticalOffset - this.ViewportHeight) : this.NewItemOffset(flag, -this.ViewportHeight, true);
			this.SetVerticalOffsetImpl(offset, true);
		}

		/// <summary>Scrolls content downward by one page.</summary>
		// Token: 0x06005A43 RID: 23107 RVA: 0x0018E0D0 File Offset: 0x0018C2D0
		public virtual void PageDown()
		{
			if (VirtualizingStackPanel.ScrollTracer.IsEnabled && VirtualizingStackPanel.ScrollTracer.IsTracing(this))
			{
				VirtualizingStackPanel.ScrollTracer.Trace(this, VirtualizingStackPanel.ScrollTraceOp.PageDown, new object[0]);
			}
			bool flag = this.Orientation == Orientation.Horizontal;
			double offset = (this.IsPixelBased || flag) ? (this.VerticalOffset + this.ViewportHeight) : this.NewItemOffset(flag, this.ViewportHeight, true);
			this.SetVerticalOffsetImpl(offset, true);
		}

		/// <summary>Scrolls content to the left by one page.</summary>
		// Token: 0x06005A44 RID: 23108 RVA: 0x0018E134 File Offset: 0x0018C334
		public virtual void PageLeft()
		{
			if (VirtualizingStackPanel.ScrollTracer.IsEnabled && VirtualizingStackPanel.ScrollTracer.IsTracing(this))
			{
				VirtualizingStackPanel.ScrollTracer.Trace(this, VirtualizingStackPanel.ScrollTraceOp.PageLeft, new object[0]);
			}
			bool flag = this.Orientation == Orientation.Horizontal;
			double offset = (this.IsPixelBased || !flag) ? (this.HorizontalOffset - this.ViewportWidth) : this.NewItemOffset(flag, -this.ViewportWidth, true);
			this.SetHorizontalOffsetImpl(offset, true);
		}

		/// <summary>Scrolls content to the right by one page.</summary>
		// Token: 0x06005A45 RID: 23109 RVA: 0x0018E19C File Offset: 0x0018C39C
		public virtual void PageRight()
		{
			if (VirtualizingStackPanel.ScrollTracer.IsEnabled && VirtualizingStackPanel.ScrollTracer.IsTracing(this))
			{
				VirtualizingStackPanel.ScrollTracer.Trace(this, VirtualizingStackPanel.ScrollTraceOp.PageRight, new object[0]);
			}
			bool flag = this.Orientation == Orientation.Horizontal;
			double offset = (this.IsPixelBased || !flag) ? (this.HorizontalOffset + this.ViewportWidth) : this.NewItemOffset(flag, this.ViewportWidth, true);
			this.SetHorizontalOffsetImpl(offset, true);
		}

		/// <summary>Scrolls content logically upward in response to an upward click of the mouse wheel button.</summary>
		// Token: 0x06005A46 RID: 23110 RVA: 0x0018E204 File Offset: 0x0018C404
		public virtual void MouseWheelUp()
		{
			if (this.CanMouseWheelVerticallyScroll)
			{
				if (VirtualizingStackPanel.ScrollTracer.IsEnabled && VirtualizingStackPanel.ScrollTracer.IsTracing(this))
				{
					VirtualizingStackPanel.ScrollTracer.Trace(this, VirtualizingStackPanel.ScrollTraceOp.MouseWheelUp, new object[0]);
				}
				int wheelScrollLines = SystemParameters.WheelScrollLines;
				bool flag = this.Orientation == Orientation.Horizontal;
				double offset = (this.IsPixelBased || flag) ? (this.VerticalOffset - (double)wheelScrollLines * 16.0) : this.NewItemOffset(flag, (double)(-(double)wheelScrollLines), true);
				this.SetVerticalOffsetImpl(offset, true);
				return;
			}
			this.PageUp();
		}

		/// <summary>Scrolls content logically downward in response to a downward click of the mouse wheel button.</summary>
		// Token: 0x06005A47 RID: 23111 RVA: 0x0018E280 File Offset: 0x0018C480
		public virtual void MouseWheelDown()
		{
			if (this.CanMouseWheelVerticallyScroll)
			{
				if (VirtualizingStackPanel.ScrollTracer.IsEnabled && VirtualizingStackPanel.ScrollTracer.IsTracing(this))
				{
					VirtualizingStackPanel.ScrollTracer.Trace(this, VirtualizingStackPanel.ScrollTraceOp.MouseWheelDown, new object[0]);
				}
				int wheelScrollLines = SystemParameters.WheelScrollLines;
				bool flag = this.Orientation == Orientation.Horizontal;
				double offset = (this.IsPixelBased || flag) ? (this.VerticalOffset + (double)wheelScrollLines * 16.0) : this.NewItemOffset(flag, (double)wheelScrollLines, false);
				this.SetVerticalOffsetImpl(offset, true);
				return;
			}
			this.PageDown();
		}

		/// <summary>Scrolls content logically to the left in response to a left click of the mouse wheel button.</summary>
		// Token: 0x06005A48 RID: 23112 RVA: 0x0018E2FC File Offset: 0x0018C4FC
		public virtual void MouseWheelLeft()
		{
			if (VirtualizingStackPanel.ScrollTracer.IsEnabled && VirtualizingStackPanel.ScrollTracer.IsTracing(this))
			{
				VirtualizingStackPanel.ScrollTracer.Trace(this, VirtualizingStackPanel.ScrollTraceOp.MouseWheelLeft, new object[0]);
			}
			bool flag = this.Orientation == Orientation.Horizontal;
			double offset = (this.IsPixelBased || !flag) ? (this.HorizontalOffset - 48.0) : this.NewItemOffset(flag, -3.0, true);
			this.SetHorizontalOffsetImpl(offset, true);
		}

		/// <summary>Scrolls content logically to the right in response to a right click of the mouse wheel button.</summary>
		// Token: 0x06005A49 RID: 23113 RVA: 0x0018E368 File Offset: 0x0018C568
		public virtual void MouseWheelRight()
		{
			if (VirtualizingStackPanel.ScrollTracer.IsEnabled && VirtualizingStackPanel.ScrollTracer.IsTracing(this))
			{
				VirtualizingStackPanel.ScrollTracer.Trace(this, VirtualizingStackPanel.ScrollTraceOp.MouseWheelRight, new object[0]);
			}
			bool flag = this.Orientation == Orientation.Horizontal;
			double offset = (this.IsPixelBased || !flag) ? (this.HorizontalOffset + 48.0) : this.NewItemOffset(flag, 3.0, false);
			this.SetHorizontalOffsetImpl(offset, true);
		}

		// Token: 0x06005A4A RID: 23114 RVA: 0x0018E3D4 File Offset: 0x0018C5D4
		private double NewItemOffset(bool isHorizontal, double delta, bool fromFirst)
		{
			if (DoubleUtil.IsZero(delta))
			{
				delta = 1.0;
			}
			if (VirtualizingStackPanel.IsVSP45Compat)
			{
				return (isHorizontal ? this.HorizontalOffset : this.VerticalOffset) + delta;
			}
			double num;
			FrameworkElement frameworkElement = this.ComputeFirstContainerInViewport(this, isHorizontal ? FocusNavigationDirection.Right : FocusNavigationDirection.Down, this, null, true, out num);
			if (frameworkElement == null || DoubleUtil.IsZero(num))
			{
				return (isHorizontal ? this.HorizontalOffset : this.VerticalOffset) + delta;
			}
			double num2 = this.FindScrollOffset(frameworkElement);
			if (fromFirst)
			{
				num2 -= num;
			}
			if (isHorizontal)
			{
				this._scrollData._computedOffset.X = num2;
			}
			else
			{
				this._scrollData._computedOffset.Y = num2;
			}
			return num2 + delta;
		}

		/// <summary>Sets the value of the <see cref="P:System.Windows.Controls.VirtualizingStackPanel.HorizontalOffset" /> property.</summary>
		/// <param name="offset">The value of the <see cref="P:System.Windows.Controls.VirtualizingStackPanel.HorizontalOffset" /> property.</param>
		// Token: 0x06005A4B RID: 23115 RVA: 0x0018E47C File Offset: 0x0018C67C
		public void SetHorizontalOffset(double offset)
		{
			if (VirtualizingStackPanel.ScrollTracer.IsEnabled && VirtualizingStackPanel.ScrollTracer.IsTracing(this))
			{
				VirtualizingStackPanel.ScrollTracer.Trace(this, VirtualizingStackPanel.ScrollTraceOp.SetHorizontalOffset, new object[]
				{
					offset,
					"delta:",
					offset - this.HorizontalOffset
				});
			}
			this.ClearAnchorInformation(true);
			this.SetHorizontalOffsetImpl(offset, false);
		}

		// Token: 0x06005A4C RID: 23116 RVA: 0x0018E4D8 File Offset: 0x0018C6D8
		private void SetHorizontalOffsetImpl(double offset, bool setAnchorInformation)
		{
			if (!this.IsScrolling)
			{
				return;
			}
			double num = ScrollContentPresenter.ValidateInputOffset(offset, "HorizontalOffset");
			if (!DoubleUtil.AreClose(num, this._scrollData._offset.X))
			{
				Vector offset2 = this._scrollData._offset;
				this._scrollData._offset.X = num;
				this.OnViewportOffsetChanged(offset2, this._scrollData._offset);
				if (this.IsVirtualizing)
				{
					this.IsScrollActive = true;
					this._scrollData.SetHorizontalScrollType(offset2.X, num);
					base.InvalidateMeasure();
					if (!VirtualizingStackPanel.IsVSP45Compat && this.Orientation == Orientation.Horizontal)
					{
						this.IncrementScrollGeneration();
						double value = Math.Abs(num - offset2.X);
						if (DoubleUtil.LessThanOrClose(value, this.ViewportWidth))
						{
							if (!this.IsPixelBased)
							{
								this._scrollData._offset.X = Math.Floor(this._scrollData._offset.X);
								this._scrollData._computedOffset.X = Math.Floor(this._scrollData._computedOffset.X);
							}
							else if (base.UseLayoutRounding)
							{
								DpiScale dpi = base.GetDpi();
								this._scrollData._offset.X = UIElement.RoundLayoutValue(this._scrollData._offset.X, dpi.DpiScaleX);
								this._scrollData._computedOffset.X = UIElement.RoundLayoutValue(this._scrollData._computedOffset.X, dpi.DpiScaleX);
							}
							if (!setAnchorInformation && !this.IsPixelBased)
							{
								double num2;
								FrameworkElement v = this.ComputeFirstContainerInViewport(this, FocusNavigationDirection.Right, this, null, true, out num2);
								if (num2 > 0.0)
								{
									double x = this.FindScrollOffset(v);
									this._scrollData._computedOffset.X = x;
								}
							}
							setAnchorInformation = true;
						}
					}
				}
				else if (!this.IsPixelBased)
				{
					base.InvalidateMeasure();
				}
				else
				{
					this._scrollData._offset.X = ScrollContentPresenter.CoerceOffset(num, this._scrollData._extent.Width, this._scrollData._viewport.Width);
					this._scrollData._computedOffset.X = this._scrollData._offset.X;
					base.InvalidateArrange();
					this.OnScrollChange();
				}
				if (VirtualizingStackPanel.ScrollTracer.IsEnabled && VirtualizingStackPanel.ScrollTracer.IsTracing(this))
				{
					VirtualizingStackPanel.ScrollTracer.Trace(this, VirtualizingStackPanel.ScrollTraceOp.SetHOff, new object[]
					{
						this._scrollData._offset,
						this._scrollData._extent,
						this._scrollData._computedOffset
					});
				}
			}
			if (setAnchorInformation)
			{
				this.SetAnchorInformation(true);
			}
		}

		/// <summary>Sets the value of the <see cref="P:System.Windows.Controls.VirtualizingStackPanel.VerticalOffset" /> property.</summary>
		/// <param name="offset">The value of the <see cref="P:System.Windows.Controls.VirtualizingStackPanel.VerticalOffset" /> property.</param>
		// Token: 0x06005A4D RID: 23117 RVA: 0x0018E780 File Offset: 0x0018C980
		public void SetVerticalOffset(double offset)
		{
			if (VirtualizingStackPanel.ScrollTracer.IsEnabled && VirtualizingStackPanel.ScrollTracer.IsTracing(this))
			{
				VirtualizingStackPanel.ScrollTracer.Trace(this, VirtualizingStackPanel.ScrollTraceOp.SetVerticalOffset, new object[]
				{
					offset,
					"delta:",
					offset - this.VerticalOffset
				});
			}
			this.ClearAnchorInformation(true);
			this.SetVerticalOffsetImpl(offset, false);
		}

		// Token: 0x06005A4E RID: 23118 RVA: 0x0018E7DC File Offset: 0x0018C9DC
		private void SetVerticalOffsetImpl(double offset, bool setAnchorInformation)
		{
			if (!this.IsScrolling)
			{
				return;
			}
			double num = ScrollContentPresenter.ValidateInputOffset(offset, "VerticalOffset");
			if (!DoubleUtil.AreClose(num, this._scrollData._offset.Y))
			{
				Vector offset2 = this._scrollData._offset;
				this._scrollData._offset.Y = num;
				this.OnViewportOffsetChanged(offset2, this._scrollData._offset);
				if (this.IsVirtualizing)
				{
					base.InvalidateMeasure();
					this.IsScrollActive = true;
					this._scrollData.SetVerticalScrollType(offset2.Y, num);
					if (!VirtualizingStackPanel.IsVSP45Compat && this.Orientation == Orientation.Vertical)
					{
						this.IncrementScrollGeneration();
						double value = Math.Abs(num - offset2.Y);
						if (DoubleUtil.LessThanOrClose(value, this.ViewportHeight))
						{
							if (!this.IsPixelBased)
							{
								this._scrollData._offset.Y = Math.Floor(this._scrollData._offset.Y);
								this._scrollData._computedOffset.Y = Math.Floor(this._scrollData._computedOffset.Y);
							}
							else if (base.UseLayoutRounding)
							{
								DpiScale dpi = base.GetDpi();
								this._scrollData._offset.Y = UIElement.RoundLayoutValue(this._scrollData._offset.Y, dpi.DpiScaleY);
								this._scrollData._computedOffset.Y = UIElement.RoundLayoutValue(this._scrollData._computedOffset.Y, dpi.DpiScaleY);
							}
							if (!setAnchorInformation && !this.IsPixelBased)
							{
								double num2;
								FrameworkElement v = this.ComputeFirstContainerInViewport(this, FocusNavigationDirection.Down, this, null, true, out num2);
								if (num2 > 0.0)
								{
									double y = this.FindScrollOffset(v);
									this._scrollData._computedOffset.Y = y;
								}
							}
							setAnchorInformation = true;
						}
					}
				}
				else if (!this.IsPixelBased)
				{
					base.InvalidateMeasure();
				}
				else
				{
					this._scrollData._offset.Y = ScrollContentPresenter.CoerceOffset(num, this._scrollData._extent.Height, this._scrollData._viewport.Height);
					this._scrollData._computedOffset.Y = this._scrollData._offset.Y;
					base.InvalidateArrange();
					this.OnScrollChange();
				}
			}
			if (setAnchorInformation)
			{
				this.SetAnchorInformation(false);
			}
			if (VirtualizingStackPanel.ScrollTracer.IsEnabled && VirtualizingStackPanel.ScrollTracer.IsTracing(this))
			{
				VirtualizingStackPanel.ScrollTracer.Trace(this, VirtualizingStackPanel.ScrollTraceOp.SetVOff, new object[]
				{
					this._scrollData._offset,
					this._scrollData._extent,
					this._scrollData._computedOffset
				});
			}
		}

		// Token: 0x06005A4F RID: 23119 RVA: 0x0018EA84 File Offset: 0x0018CC84
		private void SetAnchorInformation(bool isHorizontalOffset)
		{
			if (this.IsScrolling && this.IsVirtualizing)
			{
				bool flag = this.Orientation == Orientation.Horizontal;
				if (flag == isHorizontalOffset)
				{
					bool areContainersUniformlySized = this.GetAreContainersUniformlySized(null, this);
					if (!areContainersUniformlySized || this.HasVirtualizingChildren)
					{
						ItemsControl itemsControl;
						ItemsControl.GetItemsOwnerInternal(this, out itemsControl);
						if (itemsControl != null)
						{
							bool flag2 = VirtualizingStackPanel.ScrollTracer.IsEnabled && VirtualizingStackPanel.ScrollTracer.IsTracing(this);
							double num = flag ? (this._scrollData._offset.X - this._scrollData._computedOffset.X) : (this._scrollData._offset.Y - this._scrollData._computedOffset.Y);
							if (flag2)
							{
								VirtualizingStackPanel.ScrollTracer.Trace(this, VirtualizingStackPanel.ScrollTraceOp.BSetAnchor, new object[]
								{
									num
								});
							}
							if (this._scrollData._firstContainerInViewport != null)
							{
								this.OnAnchorOperation(true);
								if (flag)
								{
									VirtualizingStackPanel.ScrollData scrollData = this._scrollData;
									scrollData._offset.X = scrollData._offset.X + num;
								}
								else
								{
									VirtualizingStackPanel.ScrollData scrollData2 = this._scrollData;
									scrollData2._offset.Y = scrollData2._offset.Y + num;
								}
							}
							if (this._scrollData._firstContainerInViewport == null)
							{
								this._scrollData._firstContainerInViewport = this.ComputeFirstContainerInViewport(itemsControl.GetViewportElement(), flag ? FocusNavigationDirection.Right : FocusNavigationDirection.Down, this, delegate(DependencyObject d)
								{
									d.SetCurrentValue(VirtualizingPanel.IsContainerVirtualizableProperty, false);
								}, false, out this._scrollData._firstContainerOffsetFromViewport);
								if (this._scrollData._firstContainerInViewport != null)
								{
									this._scrollData._expectedDistanceBetweenViewports = num;
									DispatcherOperation value = base.Dispatcher.BeginInvoke(DispatcherPriority.Render, new Action(this.OnAnchorOperation));
									VirtualizingStackPanel.AnchorOperationField.SetValue(this, value);
								}
							}
							else
							{
								this._scrollData._expectedDistanceBetweenViewports += num;
							}
							if (flag2)
							{
								VirtualizingStackPanel.ScrollTracer.Trace(this, VirtualizingStackPanel.ScrollTraceOp.ESetAnchor, new object[]
								{
									this._scrollData._expectedDistanceBetweenViewports,
									this._scrollData._firstContainerInViewport,
									this._scrollData._firstContainerOffsetFromViewport
								});
							}
						}
					}
				}
			}
		}

		// Token: 0x06005A50 RID: 23120 RVA: 0x0018EC94 File Offset: 0x0018CE94
		private void OnAnchorOperation()
		{
			bool isAnchorOperationPending = false;
			this.OnAnchorOperation(isAnchorOperationPending);
		}

		// Token: 0x06005A51 RID: 23121 RVA: 0x0018ECAC File Offset: 0x0018CEAC
		private void OnAnchorOperation(bool isAnchorOperationPending)
		{
			ItemsControl itemsControl;
			ItemsControl.GetItemsOwnerInternal(this, out itemsControl);
			if (itemsControl == null || !VisualTreeHelper.IsAncestorOf(this, this._scrollData._firstContainerInViewport))
			{
				this.ClearAnchorInformation(isAnchorOperationPending);
				return;
			}
			bool flag = VirtualizingStackPanel.ScrollTracer.IsEnabled && VirtualizingStackPanel.ScrollTracer.IsTracing(this);
			if (flag)
			{
				VirtualizingStackPanel.ScrollTracer.Trace(this, VirtualizingStackPanel.ScrollTraceOp.BOnAnchor, new object[]
				{
					isAnchorOperationPending,
					this._scrollData._expectedDistanceBetweenViewports,
					this._scrollData._firstContainerInViewport
				});
			}
			bool isVSP45Compat = VirtualizingStackPanel.IsVSP45Compat;
			if (!isVSP45Compat && !isAnchorOperationPending && (base.MeasureDirty || base.ArrangeDirty))
			{
				if (flag)
				{
					VirtualizingStackPanel.ScrollTracer.Trace(this, VirtualizingStackPanel.ScrollTraceOp.ROnAnchor, new object[0]);
				}
				this.CancelPendingAnchoredInvalidateMeasure();
				DispatcherOperation value = base.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(this.OnAnchorOperation));
				VirtualizingStackPanel.AnchorOperationField.SetValue(this, value);
				return;
			}
			bool flag2 = this.Orientation == Orientation.Horizontal;
			FrameworkElement firstContainerInViewport = this._scrollData._firstContainerInViewport;
			double firstContainerOffsetFromViewport = this._scrollData._firstContainerOffsetFromViewport;
			double num = this.FindScrollOffset(this._scrollData._firstContainerInViewport);
			double num2;
			FrameworkElement v = this.ComputeFirstContainerInViewport(itemsControl.GetViewportElement(), flag2 ? FocusNavigationDirection.Right : FocusNavigationDirection.Down, this, null, false, out num2);
			double num3 = this.FindScrollOffset(v);
			double num4 = num3 - num2 - (num - firstContainerOffsetFromViewport);
			bool flag3 = LayoutDoubleUtil.AreClose(this._scrollData._expectedDistanceBetweenViewports, num4);
			if (!flag3 && !isVSP45Compat && !this.IsPixelBased)
			{
				double num5;
				FrameworkElement frameworkElement = this.ComputeFirstContainerInViewport(this, flag2 ? FocusNavigationDirection.Right : FocusNavigationDirection.Down, this, null, true, out num5);
				double num6 = num4 - this._scrollData._expectedDistanceBetweenViewports;
				flag3 = (!LayoutDoubleUtil.LessThan(num6, 0.0) && !LayoutDoubleUtil.LessThan(num5, num6));
				if (flag3)
				{
					num2 += num5;
				}
				if (!flag3)
				{
					double value2;
					double value3;
					if (flag2)
					{
						value2 = this._scrollData._computedOffset.X;
						value3 = this._scrollData._extent.Width - this._scrollData._viewport.Width;
					}
					else
					{
						value2 = this._scrollData._computedOffset.Y;
						value3 = this._scrollData._extent.Height - this._scrollData._viewport.Height;
					}
					flag3 = (LayoutDoubleUtil.LessThan(value3, value2) || LayoutDoubleUtil.AreClose(value3, value2));
				}
			}
			if (flag3)
			{
				if (flag2)
				{
					this._scrollData._computedOffset.X = num3 - num2;
					this._scrollData._offset.X = this._scrollData._computedOffset.X;
				}
				else
				{
					this._scrollData._computedOffset.Y = num3 - num2;
					this._scrollData._offset.Y = this._scrollData._computedOffset.Y;
				}
				this.ClearAnchorInformation(isAnchorOperationPending);
				if (flag)
				{
					VirtualizingStackPanel.ScrollTracer.Trace(this, VirtualizingStackPanel.ScrollTraceOp.SOnAnchor, new object[]
					{
						this._scrollData._offset
					});
					return;
				}
			}
			else
			{
				bool flag4 = false;
				double num7;
				double num8;
				if (flag2)
				{
					this._scrollData._computedOffset.X = num - firstContainerOffsetFromViewport;
					num7 = this._scrollData._computedOffset.X + num4;
					num8 = this._scrollData._computedOffset.X + this._scrollData._expectedDistanceBetweenViewports;
					double num9 = this._scrollData._extent.Width - this._scrollData._viewport.Width;
					if (LayoutDoubleUtil.LessThan(num8, 0.0) || LayoutDoubleUtil.LessThan(num9, num8))
					{
						if (FrameworkAppContextSwitches.OptOutOfEffectiveOffsetHangFix)
						{
							this._scrollData._computedOffset.X = num7;
							this._scrollData._offset.X = num7;
						}
						else if (LayoutDoubleUtil.AreClose(num7, 0.0) || LayoutDoubleUtil.AreClose(num7, num9))
						{
							this._scrollData._computedOffset.X = num7;
							this._scrollData._offset.X = num7;
						}
						else
						{
							flag4 = true;
							this._scrollData._offset.X = num8;
						}
					}
					else
					{
						flag4 = true;
						this._scrollData._offset.X = num8;
					}
				}
				else
				{
					this._scrollData._computedOffset.Y = num - firstContainerOffsetFromViewport;
					num7 = this._scrollData._computedOffset.Y + num4;
					num8 = this._scrollData._computedOffset.Y + this._scrollData._expectedDistanceBetweenViewports;
					double num9 = this._scrollData._extent.Height - this._scrollData._viewport.Height;
					if (LayoutDoubleUtil.LessThan(num8, 0.0) || LayoutDoubleUtil.LessThan(num9, num8))
					{
						if (FrameworkAppContextSwitches.OptOutOfEffectiveOffsetHangFix)
						{
							this._scrollData._computedOffset.Y = num7;
							this._scrollData._offset.Y = num7;
						}
						else if (LayoutDoubleUtil.AreClose(num7, 0.0) || LayoutDoubleUtil.AreClose(num7, num9))
						{
							this._scrollData._computedOffset.Y = num7;
							this._scrollData._offset.Y = num7;
						}
						else
						{
							flag4 = true;
							this._scrollData._offset.Y = num8;
						}
					}
					else
					{
						flag4 = true;
						this._scrollData._offset.Y = num8;
					}
				}
				if (flag)
				{
					VirtualizingStackPanel.ScrollTracer.Trace(this, VirtualizingStackPanel.ScrollTraceOp.EOnAnchor, new object[]
					{
						flag4,
						num8,
						num7,
						this._scrollData._offset,
						this._scrollData._computedOffset
					});
				}
				if (flag4)
				{
					this.OnScrollChange();
					base.InvalidateMeasure();
					if (!isVSP45Compat)
					{
						this.CancelPendingAnchoredInvalidateMeasure();
						this.IncrementScrollGeneration();
					}
					if (!isAnchorOperationPending)
					{
						DispatcherOperation value4 = base.Dispatcher.BeginInvoke(DispatcherPriority.Render, new Action(this.OnAnchorOperation));
						VirtualizingStackPanel.AnchorOperationField.SetValue(this, value4);
					}
					if (!isVSP45Compat && this.IsScrollActive)
					{
						DispatcherOperation value5 = VirtualizingStackPanel.ClearIsScrollActiveOperationField.GetValue(this);
						if (value5 != null)
						{
							value5.Abort();
							VirtualizingStackPanel.ClearIsScrollActiveOperationField.SetValue(this, null);
							return;
						}
					}
				}
				else
				{
					this.ClearAnchorInformation(isAnchorOperationPending);
				}
			}
		}

		// Token: 0x06005A52 RID: 23122 RVA: 0x0018F2E0 File Offset: 0x0018D4E0
		private void ClearAnchorInformation(bool shouldAbort)
		{
			if (this._scrollData == null)
			{
				return;
			}
			if (this._scrollData._firstContainerInViewport != null)
			{
				DependencyObject dependencyObject = this._scrollData._firstContainerInViewport;
				do
				{
					DependencyObject parent = VisualTreeHelper.GetParent(dependencyObject);
					Panel panel = parent as Panel;
					if (panel != null && panel.IsItemsHost)
					{
						dependencyObject.InvalidateProperty(VirtualizingPanel.IsContainerVirtualizableProperty);
					}
					dependencyObject = parent;
				}
				while (dependencyObject != null && dependencyObject != this);
				this._scrollData._firstContainerInViewport = null;
				this._scrollData._firstContainerOffsetFromViewport = 0.0;
				this._scrollData._expectedDistanceBetweenViewports = 0.0;
				if (shouldAbort)
				{
					DispatcherOperation value = VirtualizingStackPanel.AnchorOperationField.GetValue(this);
					value.Abort();
				}
				VirtualizingStackPanel.AnchorOperationField.ClearValue(this);
			}
		}

		// Token: 0x06005A53 RID: 23123 RVA: 0x0018F394 File Offset: 0x0018D594
		private FrameworkElement ComputeFirstContainerInViewport(FrameworkElement viewportElement, FocusNavigationDirection direction, Panel itemsHost, Action<DependencyObject> action, bool findTopContainer, out double firstContainerOffsetFromViewport)
		{
			bool flag;
			return this.ComputeFirstContainerInViewport(viewportElement, direction, itemsHost, action, findTopContainer, out firstContainerOffsetFromViewport, out flag);
		}

		// Token: 0x06005A54 RID: 23124 RVA: 0x0018F3B4 File Offset: 0x0018D5B4
		private FrameworkElement ComputeFirstContainerInViewport(FrameworkElement viewportElement, FocusNavigationDirection direction, Panel itemsHost, Action<DependencyObject> action, bool findTopContainer, out double firstContainerOffsetFromViewport, out bool foundTopContainer)
		{
			firstContainerOffsetFromViewport = 0.0;
			foundTopContainer = false;
			if (itemsHost == null)
			{
				return null;
			}
			bool isVSP45Compat = VirtualizingStackPanel.IsVSP45Compat;
			if (!isVSP45Compat)
			{
				viewportElement = this;
			}
			FrameworkElement frameworkElement = null;
			UIElementCollection children = itemsHost.Children;
			if (children != null)
			{
				int count = children.Count;
				int num = 0;
				for (int i = (itemsHost is VirtualizingStackPanel) ? ((VirtualizingStackPanel)itemsHost)._firstItemInExtendedViewportChildIndex : 0; i < count; i++)
				{
					FrameworkElement frameworkElement2 = children[i] as FrameworkElement;
					if (frameworkElement2 != null)
					{
						if (frameworkElement2.IsVisible)
						{
							Rect rect;
							ElementViewportPosition elementViewportPosition = ItemsControl.GetElementViewportPosition(viewportElement, frameworkElement2, direction, false, !isVSP45Compat, out rect);
							if (elementViewportPosition == ElementViewportPosition.PartiallyInViewport || elementViewportPosition == ElementViewportPosition.CompletelyInViewport)
							{
								bool flag = false;
								if (!this.IsPixelBased)
								{
									double num2 = (direction == FocusNavigationDirection.Down) ? rect.Y : rect.X;
									if (findTopContainer && DoubleUtil.GreaterThan(num2, 0.0))
									{
										break;
									}
									flag = DoubleUtil.IsZero(num2);
								}
								if (action != null)
								{
									action(frameworkElement2);
								}
								if (isVSP45Compat)
								{
									ItemsControl itemsControl = frameworkElement2 as ItemsControl;
									if (itemsControl != null)
									{
										if (itemsControl.ItemsHost != null && itemsControl.ItemsHost.IsVisible)
										{
											frameworkElement = this.ComputeFirstContainerInViewport(viewportElement, direction, itemsControl.ItemsHost, action, findTopContainer, out firstContainerOffsetFromViewport);
										}
									}
									else
									{
										GroupItem groupItem = frameworkElement2 as GroupItem;
										if (groupItem != null && groupItem.ItemsHost != null && groupItem.ItemsHost.IsVisible)
										{
											frameworkElement = this.ComputeFirstContainerInViewport(viewportElement, direction, groupItem.ItemsHost, action, findTopContainer, out firstContainerOffsetFromViewport);
										}
									}
								}
								else
								{
									Panel panel = null;
									ItemsControl itemsControl2;
									GroupItem groupItem2;
									if ((itemsControl2 = (frameworkElement2 as ItemsControl)) != null)
									{
										panel = itemsControl2.ItemsHost;
									}
									else if ((groupItem2 = (frameworkElement2 as GroupItem)) != null)
									{
										panel = groupItem2.ItemsHost;
									}
									panel = (panel as VirtualizingStackPanel);
									if (panel != null && panel.IsVisible)
									{
										frameworkElement = this.ComputeFirstContainerInViewport(viewportElement, direction, panel, action, findTopContainer, out firstContainerOffsetFromViewport, out foundTopContainer);
									}
								}
								if (frameworkElement == null)
								{
									frameworkElement = frameworkElement2;
									foundTopContainer = flag;
									if (this.IsPixelBased)
									{
										if (direction == FocusNavigationDirection.Down)
										{
											firstContainerOffsetFromViewport = rect.Y;
											if (!isVSP45Compat)
											{
												firstContainerOffsetFromViewport -= frameworkElement2.Margin.Top;
												break;
											}
											break;
										}
										else
										{
											firstContainerOffsetFromViewport = rect.X;
											if (!isVSP45Compat)
											{
												firstContainerOffsetFromViewport -= frameworkElement2.Margin.Left;
												break;
											}
											break;
										}
									}
									else
									{
										if (findTopContainer && flag)
										{
											firstContainerOffsetFromViewport += (double)num;
											break;
										}
										break;
									}
								}
								else
								{
									if (this.IsPixelBased)
									{
										break;
									}
									IHierarchicalVirtualizationAndScrollInfo hierarchicalVirtualizationAndScrollInfo = frameworkElement2 as IHierarchicalVirtualizationAndScrollInfo;
									if (hierarchicalVirtualizationAndScrollInfo == null)
									{
										break;
									}
									if (isVSP45Compat)
									{
										if (direction == FocusNavigationDirection.Down)
										{
											if (DoubleUtil.GreaterThanOrClose(rect.Y, 0.0))
											{
												firstContainerOffsetFromViewport += hierarchicalVirtualizationAndScrollInfo.HeaderDesiredSizes.LogicalSize.Height;
												break;
											}
											break;
										}
										else
										{
											if (DoubleUtil.GreaterThanOrClose(rect.X, 0.0))
											{
												firstContainerOffsetFromViewport += hierarchicalVirtualizationAndScrollInfo.HeaderDesiredSizes.LogicalSize.Width;
												break;
											}
											break;
										}
									}
									else
									{
										Thickness itemsHostInsetForChild = this.GetItemsHostInsetForChild(hierarchicalVirtualizationAndScrollInfo, null, null);
										if (direction == FocusNavigationDirection.Down)
										{
											if (this.IsHeaderBeforeItems(false, frameworkElement2, ref itemsHostInsetForChild) && DoubleUtil.GreaterThanOrClose(rect.Y, 0.0) && (findTopContainer || !foundTopContainer || DoubleUtil.GreaterThan(rect.Y, 0.0)))
											{
												firstContainerOffsetFromViewport += 1.0;
												break;
											}
											break;
										}
										else
										{
											if (this.IsHeaderBeforeItems(true, frameworkElement2, ref itemsHostInsetForChild) && DoubleUtil.GreaterThanOrClose(rect.X, 0.0) && (findTopContainer || !foundTopContainer || DoubleUtil.GreaterThan(rect.X, 0.0)))
											{
												firstContainerOffsetFromViewport += 1.0;
												break;
											}
											break;
										}
									}
								}
							}
							else
							{
								if (elementViewportPosition == ElementViewportPosition.AfterViewport)
								{
									break;
								}
								num = 0;
							}
						}
						else
						{
							num++;
						}
					}
				}
			}
			if (VirtualizingStackPanel.ScrollTracer.IsEnabled && VirtualizingStackPanel.ScrollTracer.IsTracing(this))
			{
				VirtualizingStackPanel.ScrollTracer.Trace(this, VirtualizingStackPanel.ScrollTraceOp.CFCIV, new object[]
				{
					this.ContainerPath(frameworkElement),
					firstContainerOffsetFromViewport
				});
			}
			return frameworkElement;
		}

		// Token: 0x06005A55 RID: 23125 RVA: 0x0018F7D0 File Offset: 0x0018D9D0
		internal void AnchoredInvalidateMeasure()
		{
			this.WasLastMeasurePassAnchored = (this.FirstContainerInViewport != null || this.BringIntoViewLeafContainer != null);
			if (VirtualizingStackPanel.AnchoredInvalidateMeasureOperationField.GetValue(this) == null)
			{
				DispatcherOperation value = base.Dispatcher.BeginInvoke(DispatcherPriority.Render, new Action(delegate()
				{
					if (VirtualizingStackPanel.IsVSP45Compat)
					{
						VirtualizingStackPanel.AnchoredInvalidateMeasureOperationField.ClearValue(this);
						if (this.WasLastMeasurePassAnchored)
						{
							this.SetAnchorInformation(this.Orientation == Orientation.Horizontal);
						}
						base.InvalidateMeasure();
						return;
					}
					base.InvalidateMeasure();
					VirtualizingStackPanel.AnchoredInvalidateMeasureOperationField.ClearValue(this);
					if (this.WasLastMeasurePassAnchored)
					{
						this.SetAnchorInformation(this.Orientation == Orientation.Horizontal);
					}
				}));
				VirtualizingStackPanel.AnchoredInvalidateMeasureOperationField.SetValue(this, value);
			}
		}

		// Token: 0x06005A56 RID: 23126 RVA: 0x0018F82C File Offset: 0x0018DA2C
		private void CancelPendingAnchoredInvalidateMeasure()
		{
			DispatcherOperation value = VirtualizingStackPanel.AnchoredInvalidateMeasureOperationField.GetValue(this);
			if (value != null)
			{
				value.Abort();
				VirtualizingStackPanel.AnchoredInvalidateMeasureOperationField.ClearValue(this);
			}
		}

		/// <summary>Scrolls to the specified coordinates and makes that portion of a <see cref="T:System.Windows.Media.Visual" /> visible.</summary>
		/// <param name="visual">The <see cref="T:System.Windows.Media.Visual" /> that becomes visible.</param>
		/// <param name="rectangle">A <see cref="T:System.Windows.Rect" /> that represents the coordinate space within a <see cref="T:System.Windows.Media.Visual" />.</param>
		/// <returns>A <see cref="T:System.Windows.Rect" /> that is visible.</returns>
		// Token: 0x06005A57 RID: 23127 RVA: 0x0018F85C File Offset: 0x0018DA5C
		public Rect MakeVisible(Visual visual, Rect rectangle)
		{
			this.ClearAnchorInformation(true);
			Vector vector = default(Vector);
			Rect result = default(Rect);
			Rect rect = rectangle;
			bool flag = this.Orientation == Orientation.Horizontal;
			if (rectangle.IsEmpty || visual == null || visual == this || !base.IsAncestorOf(visual))
			{
				return Rect.Empty;
			}
			GeneralTransform generalTransform = visual.TransformToAncestor(this);
			rectangle = generalTransform.TransformBounds(rectangle);
			if (!this.IsScrolling)
			{
				return rectangle;
			}
			bool isVSP45Compat = VirtualizingStackPanel.IsVSP45Compat;
			bool alignTopOfBringIntoViewContainer = false;
			bool alignBottomOfBringIntoViewContainer = false;
			this.MakeVisiblePhysicalHelper(rectangle, ref vector, ref result, !flag, ref alignTopOfBringIntoViewContainer, ref alignBottomOfBringIntoViewContainer);
			alignTopOfBringIntoViewContainer = (this._scrollData._bringIntoViewLeafContainer == visual && this.AlignTopOfBringIntoViewContainer);
			alignBottomOfBringIntoViewContainer = (this._scrollData._bringIntoViewLeafContainer == visual && (isVSP45Compat ? (!this.AlignTopOfBringIntoViewContainer) : this.AlignBottomOfBringIntoViewContainer));
			if (this.IsPixelBased)
			{
				this.MakeVisiblePhysicalHelper(rectangle, ref vector, ref result, flag, ref alignTopOfBringIntoViewContainer, ref alignBottomOfBringIntoViewContainer);
			}
			else
			{
				int childIndex = (int)this.FindScrollOffset(visual);
				this.MakeVisibleLogicalHelper(childIndex, rectangle, ref vector, ref result, ref alignTopOfBringIntoViewContainer, ref alignBottomOfBringIntoViewContainer);
			}
			vector.X = ScrollContentPresenter.CoerceOffset(vector.X, this._scrollData._extent.Width, this._scrollData._viewport.Width);
			vector.Y = ScrollContentPresenter.CoerceOffset(vector.Y, this._scrollData._extent.Height, this._scrollData._viewport.Height);
			if (!LayoutDoubleUtil.AreClose(vector.X, this._scrollData._offset.X) || !LayoutDoubleUtil.AreClose(vector.Y, this._scrollData._offset.Y))
			{
				if (visual != this._scrollData._bringIntoViewLeafContainer)
				{
					this._scrollData._bringIntoViewLeafContainer = visual;
					this.AlignTopOfBringIntoViewContainer = alignTopOfBringIntoViewContainer;
					this.AlignBottomOfBringIntoViewContainer = alignBottomOfBringIntoViewContainer;
				}
				Vector offset = this._scrollData._offset;
				this._scrollData._offset = vector;
				if (VirtualizingStackPanel.ScrollTracer.IsEnabled && VirtualizingStackPanel.ScrollTracer.IsTracing(this))
				{
					VirtualizingStackPanel.ScrollTracer.Trace(this, VirtualizingStackPanel.ScrollTraceOp.MakeVisible, new object[]
					{
						this._scrollData._offset,
						rectangle,
						this._scrollData._bringIntoViewLeafContainer
					});
				}
				this.OnViewportOffsetChanged(offset, vector);
				if (this.IsVirtualizing)
				{
					this.IsScrollActive = true;
					this._scrollData.SetHorizontalScrollType(offset.X, vector.X);
					this._scrollData.SetVerticalScrollType(offset.Y, vector.Y);
					base.InvalidateMeasure();
				}
				else if (!this.IsPixelBased)
				{
					base.InvalidateMeasure();
				}
				else
				{
					this._scrollData._computedOffset = vector;
					base.InvalidateArrange();
				}
				this.OnScrollChange();
				if (this.ScrollOwner != null)
				{
					this.ScrollOwner.MakeVisible(visual, rect);
				}
			}
			else
			{
				if (isVSP45Compat)
				{
					this._scrollData._bringIntoViewLeafContainer = null;
				}
				this.AlignTopOfBringIntoViewContainer = false;
				this.AlignBottomOfBringIntoViewContainer = false;
			}
			return result;
		}

		/// <summary>Generates the item at the specified index position and brings it into view.</summary>
		/// <param name="index">The position of the item to generate and make visible.</param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The <paramref name="index" /> position does not exist in the child collection.</exception>
		// Token: 0x06005A58 RID: 23128 RVA: 0x0018FB40 File Offset: 0x0018DD40
		protected internal override void BringIndexIntoView(int index)
		{
			ItemsControl itemsOwner = ItemsControl.GetItemsOwner(this);
			if (!itemsOwner.IsGrouping)
			{
				this.BringContainerIntoView(itemsOwner, index);
				return;
			}
			base.EnsureGenerator();
			ItemContainerGenerator itemContainerGenerator = (ItemContainerGenerator)base.Generator;
			IList itemsInternal = itemContainerGenerator.ItemsInternal;
			for (int i = 0; i < itemsInternal.Count; i++)
			{
				CollectionViewGroup collectionViewGroup = itemsInternal[i] as CollectionViewGroup;
				if (collectionViewGroup != null)
				{
					if (index >= collectionViewGroup.ItemCount)
					{
						index -= collectionViewGroup.ItemCount;
					}
					else
					{
						GroupItem groupItem = itemContainerGenerator.ContainerFromItem(collectionViewGroup) as GroupItem;
						if (groupItem == null)
						{
							this.BringContainerIntoView(itemsOwner, i);
							groupItem = (itemContainerGenerator.ContainerFromItem(collectionViewGroup) as GroupItem);
						}
						if (groupItem == null)
						{
							break;
						}
						groupItem.UpdateLayout();
						VirtualizingPanel virtualizingPanel = groupItem.ItemsHost as VirtualizingPanel;
						if (virtualizingPanel != null)
						{
							virtualizingPanel.BringIndexIntoViewPublic(index);
							return;
						}
						break;
					}
				}
				else if (i == index)
				{
					this.BringContainerIntoView(itemsOwner, i);
				}
			}
		}

		// Token: 0x06005A59 RID: 23129 RVA: 0x0018FC1C File Offset: 0x0018DE1C
		private void BringContainerIntoView(ItemsControl itemsControl, int itemIndex)
		{
			if (itemIndex < 0 || itemIndex >= this.ItemCount)
			{
				throw new ArgumentOutOfRangeException("itemIndex");
			}
			IItemContainerGenerator generator = base.Generator;
			int childIndex;
			GeneratorPosition position = this.IndexToGeneratorPositionForStart(itemIndex, out childIndex);
			UIElement uielement;
			using (generator.StartAt(position, GeneratorDirection.Forward, true))
			{
				bool newlyRealized;
				uielement = (generator.GenerateNext(out newlyRealized) as UIElement);
				if (uielement != null)
				{
					bool flag = this.AddContainerFromGenerator(childIndex, uielement, newlyRealized, false);
					if (flag)
					{
						base.InvalidateZState();
					}
				}
			}
			if (uielement != null)
			{
				FrameworkElement frameworkElement = uielement as FrameworkElement;
				if (frameworkElement != null)
				{
					this._bringIntoViewContainer = frameworkElement;
					base.Dispatcher.BeginInvoke(DispatcherPriority.Loaded, new Action(delegate()
					{
						this._bringIntoViewContainer = null;
					}));
					if (!itemsControl.IsGrouping && VirtualizingPanel.GetScrollUnit(itemsControl) == ScrollUnit.Item)
					{
						frameworkElement.BringIntoView();
						return;
					}
					if (!(frameworkElement is GroupItem))
					{
						base.UpdateLayout();
						frameworkElement.BringIntoView();
					}
				}
			}
		}

		/// <summary>Gets or sets a value that describes the horizontal or vertical orientation of stacked content.  </summary>
		/// <returns>The <see cref="T:System.Windows.Controls.Orientation" /> of child content. The default is <see cref="F:System.Windows.Controls.Orientation.Vertical" />.</returns>
		// Token: 0x170015E5 RID: 5605
		// (get) Token: 0x06005A5A RID: 23130 RVA: 0x0018FD04 File Offset: 0x0018DF04
		// (set) Token: 0x06005A5B RID: 23131 RVA: 0x0018FD16 File Offset: 0x0018DF16
		public Orientation Orientation
		{
			get
			{
				return (Orientation)base.GetValue(VirtualizingStackPanel.OrientationProperty);
			}
			set
			{
				base.SetValue(VirtualizingStackPanel.OrientationProperty, value);
			}
		}

		/// <summary>Gets a value that indicates if this <see cref="T:System.Windows.Controls.VirtualizingStackPanel" /> has a vertical or horizontal orientation.</summary>
		/// <returns>This property always returns <see langword="true" />.</returns>
		// Token: 0x170015E6 RID: 5606
		// (get) Token: 0x06005A5C RID: 23132 RVA: 0x00016748 File Offset: 0x00014948
		protected internal override bool HasLogicalOrientation
		{
			get
			{
				return true;
			}
		}

		/// <summary>Gets a value that represents the <see cref="T:System.Windows.Controls.Orientation" /> of the <see cref="T:System.Windows.Controls.VirtualizingStackPanel" />.</summary>
		/// <returns>An <see cref="T:System.Windows.Controls.Orientation" /> value.</returns>
		// Token: 0x170015E7 RID: 5607
		// (get) Token: 0x06005A5D RID: 23133 RVA: 0x0018FD29 File Offset: 0x0018DF29
		protected internal override Orientation LogicalOrientation
		{
			get
			{
				return this.Orientation;
			}
		}

		/// <summary>Gets or sets a value that indicates whether a <see cref="T:System.Windows.Controls.VirtualizingStackPanel" /> can scroll in the horizontal dimension.</summary>
		/// <returns>
		///     <see langword="true" /> if content can scroll in the horizontal dimension; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
		// Token: 0x170015E8 RID: 5608
		// (get) Token: 0x06005A5E RID: 23134 RVA: 0x0018FD31 File Offset: 0x0018DF31
		// (set) Token: 0x06005A5F RID: 23135 RVA: 0x0018FD48 File Offset: 0x0018DF48
		[DefaultValue(false)]
		public bool CanHorizontallyScroll
		{
			get
			{
				return this._scrollData != null && this._scrollData._allowHorizontal;
			}
			set
			{
				this.EnsureScrollData();
				if (this._scrollData._allowHorizontal != value)
				{
					this._scrollData._allowHorizontal = value;
					base.InvalidateMeasure();
				}
			}
		}

		/// <summary>Gets or sets a value that indicates whether content can scroll in the vertical dimension.</summary>
		/// <returns>
		///     <see langword="true" /> if content can scroll in the vertical dimension; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
		// Token: 0x170015E9 RID: 5609
		// (get) Token: 0x06005A60 RID: 23136 RVA: 0x0018FD70 File Offset: 0x0018DF70
		// (set) Token: 0x06005A61 RID: 23137 RVA: 0x0018FD87 File Offset: 0x0018DF87
		[DefaultValue(false)]
		public bool CanVerticallyScroll
		{
			get
			{
				return this._scrollData != null && this._scrollData._allowVertical;
			}
			set
			{
				this.EnsureScrollData();
				if (this._scrollData._allowVertical != value)
				{
					this._scrollData._allowVertical = value;
					base.InvalidateMeasure();
				}
			}
		}

		/// <summary>Gets a value that contains the horizontal size of the extent.</summary>
		/// <returns>A <see cref="T:System.Double" /> that represents the horizontal size of the extent. The default is 0.0.</returns>
		// Token: 0x170015EA RID: 5610
		// (get) Token: 0x06005A62 RID: 23138 RVA: 0x0018FDAF File Offset: 0x0018DFAF
		public double ExtentWidth
		{
			get
			{
				if (this._scrollData == null)
				{
					return 0.0;
				}
				return this._scrollData._extent.Width;
			}
		}

		/// <summary>Gets a value that contains the vertical size of the extent.</summary>
		/// <returns>A <see cref="T:System.Double" /> that represents the vertical size of the extent. The default is 0.0.</returns>
		// Token: 0x170015EB RID: 5611
		// (get) Token: 0x06005A63 RID: 23139 RVA: 0x0018FDD3 File Offset: 0x0018DFD3
		public double ExtentHeight
		{
			get
			{
				if (this._scrollData == null)
				{
					return 0.0;
				}
				return this._scrollData._extent.Height;
			}
		}

		/// <summary>Gets a value that contains the horizontal size of the viewport of the content.</summary>
		/// <returns>A <see cref="T:System.Double" /> that represents the vertical size of the viewport of the content. The default is 0.0.</returns>
		// Token: 0x170015EC RID: 5612
		// (get) Token: 0x06005A64 RID: 23140 RVA: 0x0018FDF7 File Offset: 0x0018DFF7
		public double ViewportWidth
		{
			get
			{
				if (this._scrollData == null)
				{
					return 0.0;
				}
				return this._scrollData._viewport.Width;
			}
		}

		/// <summary>Gets a value that contains the vertical size of the viewport of the content. </summary>
		/// <returns>A <see cref="T:System.Double" /> that represents the vertical size of the viewport of the content. The default is 0.0.</returns>
		// Token: 0x170015ED RID: 5613
		// (get) Token: 0x06005A65 RID: 23141 RVA: 0x0018FE1B File Offset: 0x0018E01B
		public double ViewportHeight
		{
			get
			{
				if (this._scrollData == null)
				{
					return 0.0;
				}
				return this._scrollData._viewport.Height;
			}
		}

		/// <summary>Gets a value that contains the horizontal offset of the scrolled content. </summary>
		/// <returns>
		///     <see cref="T:System.Double" /> that represents the horizontal offset of the scrolled content. The default is 0.0.</returns>
		// Token: 0x170015EE RID: 5614
		// (get) Token: 0x06005A66 RID: 23142 RVA: 0x0018FE3F File Offset: 0x0018E03F
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public double HorizontalOffset
		{
			get
			{
				if (this._scrollData == null)
				{
					return 0.0;
				}
				return this._scrollData._computedOffset.X;
			}
		}

		/// <summary>Gets a value that contains the vertical offset of the scrolled content. </summary>
		/// <returns>A <see cref="T:System.Double" /> that represents the vertical offset of the scrolled content. The default is 0.0.</returns>
		// Token: 0x170015EF RID: 5615
		// (get) Token: 0x06005A67 RID: 23143 RVA: 0x0018FE63 File Offset: 0x0018E063
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public double VerticalOffset
		{
			get
			{
				if (this._scrollData == null)
				{
					return 0.0;
				}
				return this._scrollData._computedOffset.Y;
			}
		}

		/// <summary>Gets or sets a value that identifies the container that controls scrolling behavior in this <see cref="T:System.Windows.Controls.VirtualizingStackPanel" />. </summary>
		/// <returns>The <see cref="T:System.Windows.Controls.ScrollViewer" /> that owns scrolling for this <see cref="T:System.Windows.Controls.VirtualizingStackPanel" />. </returns>
		// Token: 0x170015F0 RID: 5616
		// (get) Token: 0x06005A68 RID: 23144 RVA: 0x0018FE87 File Offset: 0x0018E087
		// (set) Token: 0x06005A69 RID: 23145 RVA: 0x0018FE9E File Offset: 0x0018E09E
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public ScrollViewer ScrollOwner
		{
			get
			{
				if (this._scrollData == null)
				{
					return null;
				}
				return this._scrollData._scrollOwner;
			}
			set
			{
				if (this._scrollData == null)
				{
					this.EnsureScrollData();
				}
				if (value != this._scrollData._scrollOwner)
				{
					VirtualizingStackPanel.ResetScrolling(this);
					this._scrollData._scrollOwner = value;
				}
			}
		}

		/// <summary>Adds an event handler for the <see cref="E:System.Windows.Controls.VirtualizingStackPanel.CleanUpVirtualizedItem" /> attached event.</summary>
		/// <param name="element">The <see cref="T:System.Windows.DependencyObject" /> that is listening for this event.</param>
		/// <param name="handler">The event handler that is to be added.</param>
		// Token: 0x06005A6A RID: 23146 RVA: 0x0018FECE File Offset: 0x0018E0CE
		public static void AddCleanUpVirtualizedItemHandler(DependencyObject element, CleanUpVirtualizedItemEventHandler handler)
		{
			UIElement.AddHandler(element, VirtualizingStackPanel.CleanUpVirtualizedItemEvent, handler);
		}

		/// <summary>Removes an event handler for the <see cref="E:System.Windows.Controls.VirtualizingStackPanel.CleanUpVirtualizedItem" /> attached event. </summary>
		/// <param name="element">The <see cref="T:System.Windows.DependencyObject" /> from which the handler is being removed.</param>
		/// <param name="handler">Specifies the event handler that is to be removed.</param>
		// Token: 0x06005A6B RID: 23147 RVA: 0x0018FEDC File Offset: 0x0018E0DC
		public static void RemoveCleanUpVirtualizedItemHandler(DependencyObject element, CleanUpVirtualizedItemEventHandler handler)
		{
			UIElement.RemoveHandler(element, VirtualizingStackPanel.CleanUpVirtualizedItemEvent, handler);
		}

		/// <summary>Called when an item that is hosted by the <see cref="T:System.Windows.Controls.VirtualizingStackPanel" /> is re-virtualized. </summary>
		/// <param name="e">Data about the event.</param>
		// Token: 0x06005A6C RID: 23148 RVA: 0x0018FEEC File Offset: 0x0018E0EC
		protected virtual void OnCleanUpVirtualizedItem(CleanUpVirtualizedItemEventArgs e)
		{
			ItemsControl itemsOwner = ItemsControl.GetItemsOwner(this);
			if (itemsOwner != null)
			{
				itemsOwner.RaiseEvent(e);
			}
		}

		/// <summary>Gets value that indicates whether the <see cref="T:System.Windows.Controls.VirtualizingStackPanel" /> can virtualize items that are grouped or organized in a hierarchy.</summary>
		/// <returns>
		///     <see langword="true" /> in all cases.</returns>
		// Token: 0x170015F1 RID: 5617
		// (get) Token: 0x06005A6D RID: 23149 RVA: 0x00016748 File Offset: 0x00014948
		protected override bool CanHierarchicallyScrollAndVirtualizeCore
		{
			get
			{
				return true;
			}
		}

		/// <summary>Measures the child elements of a <see cref="T:System.Windows.Controls.VirtualizingStackPanel" /> in anticipation of arranging them during the <see cref="M:System.Windows.Controls.VirtualizingStackPanel.ArrangeOverride(System.Windows.Size)" /> pass.</summary>
		/// <param name="constraint">An upper limit <see cref="T:System.Windows.Size" /> that should not be exceeded.</param>
		/// <returns>The <see cref="T:System.Windows.Size" /> that represents the desired size of the element.</returns>
		// Token: 0x06005A6E RID: 23150 RVA: 0x0018FF0C File Offset: 0x0018E10C
		protected override Size MeasureOverride(Size constraint)
		{
			List<double> previouslyMeasuredOffsets = null;
			double? lastPageSafeOffset = null;
			double? lastPagePixelSize = null;
			if (VirtualizingStackPanel.IsVSP45Compat)
			{
				return this.MeasureOverrideImpl(constraint, ref lastPageSafeOffset, ref previouslyMeasuredOffsets, ref lastPagePixelSize, false);
			}
			VirtualizingStackPanel.OffsetInformation offsetInformation = VirtualizingStackPanel.OffsetInformationField.GetValue(this);
			if (offsetInformation != null)
			{
				previouslyMeasuredOffsets = offsetInformation.previouslyMeasuredOffsets;
				lastPageSafeOffset = offsetInformation.lastPageSafeOffset;
				lastPagePixelSize = offsetInformation.lastPagePixelSize;
			}
			Size result = this.MeasureOverrideImpl(constraint, ref lastPageSafeOffset, ref previouslyMeasuredOffsets, ref lastPagePixelSize, false);
			if (this.IsScrollActive)
			{
				offsetInformation = new VirtualizingStackPanel.OffsetInformation();
				offsetInformation.previouslyMeasuredOffsets = previouslyMeasuredOffsets;
				offsetInformation.lastPageSafeOffset = lastPageSafeOffset;
				offsetInformation.lastPagePixelSize = lastPagePixelSize;
				VirtualizingStackPanel.OffsetInformationField.SetValue(this, offsetInformation);
			}
			return result;
		}

		// Token: 0x06005A6F RID: 23151 RVA: 0x0018FFA8 File Offset: 0x0018E1A8
		private Size MeasureOverrideImpl(Size constraint, ref double? lastPageSafeOffset, ref List<double> previouslyMeasuredOffsets, ref double? lastPagePixelSize, bool remeasure)
		{
			bool flag = this.IsScrolling && EventTrace.IsEnabled(EventTrace.Keyword.KeywordGeneral, EventTrace.Level.Info);
			if (flag)
			{
				EventTrace.EventProvider.TraceEvent(EventTrace.Event.WClientStringBegin, EventTrace.Keyword.KeywordGeneral, EventTrace.Level.Info, "VirtualizingStackPanel :MeasureOverride");
			}
			Size size = default(Size);
			Size size2 = default(Size);
			Size size3 = default(Size);
			Size size4 = default(Size);
			Size size5 = default(Size);
			Size size6 = default(Size);
			Size size7 = default(Size);
			Size size8 = default(Size);
			bool flag2 = false;
			this.ItemsChangedDuringMeasure = false;
			try
			{
				if (!base.IsItemsHost)
				{
					size = this.MeasureNonItemsHost(constraint);
				}
				else
				{
					bool isVSP45Compat = VirtualizingStackPanel.IsVSP45Compat;
					ItemsControl itemsControl = null;
					GroupItem groupItem = null;
					IHierarchicalVirtualizationAndScrollInfo hierarchicalVirtualizationAndScrollInfo = null;
					IContainItemStorage containItemStorage = null;
					object obj = null;
					bool flag3 = this.Orientation == Orientation.Horizontal;
					bool flag4 = false;
					IContainItemStorage containItemStorage2;
					this.GetOwners(true, flag3, out itemsControl, out groupItem, out containItemStorage, out hierarchicalVirtualizationAndScrollInfo, out obj, out containItemStorage2, out flag4);
					Rect empty = Rect.Empty;
					Rect empty2 = Rect.Empty;
					VirtualizationCacheLength virtualizationCacheLength = new VirtualizationCacheLength(0.0);
					VirtualizationCacheLengthUnit cacheUnit = VirtualizationCacheLengthUnit.Pixel;
					long scrollGeneration;
					this.InitializeViewport(obj, containItemStorage2, hierarchicalVirtualizationAndScrollInfo, flag3, constraint, ref empty, ref virtualizationCacheLength, ref cacheUnit, out empty2, out scrollGeneration);
					int minValue = int.MinValue;
					int num = int.MaxValue;
					int minValue2 = int.MinValue;
					UIElement uielement = null;
					double num2 = 0.0;
					double num3 = 0.0;
					bool flag5 = false;
					bool flag6 = false;
					bool flag7 = false;
					base.EnsureGenerator();
					IList realizedChildren = this.RealizedChildren;
					IItemContainerGenerator generator = base.Generator;
					IList itemsInternal = ((ItemContainerGenerator)generator).ItemsInternal;
					int count = itemsInternal.Count;
					IContainItemStorage itemStorageProvider = isVSP45Compat ? containItemStorage : containItemStorage2;
					bool areContainersUniformlySized = this.GetAreContainersUniformlySized(itemStorageProvider, obj);
					bool computedAreContainersUniformlySized = areContainersUniformlySized;
					double num4;
					double num5;
					bool flag8;
					this.GetUniformOrAverageContainerSize(itemStorageProvider, obj, this.IsPixelBased || isVSP45Compat, out num4, out num5, out flag8);
					double computedUniformOrAverageContainerSize = num4;
					double computedUniformOrAverageContainerPixelSize = num5;
					if (VirtualizingStackPanel.ScrollTracer.IsEnabled && VirtualizingStackPanel.ScrollTracer.IsTracing(this))
					{
						VirtualizingStackPanel.ScrollTracer.Trace(this, VirtualizingStackPanel.ScrollTraceOp.BeginMeasure, new object[]
						{
							constraint,
							"MC:",
							this.MeasureCaches,
							"reM:",
							remeasure,
							"acs:",
							num4,
							areContainersUniformlySized,
							flag8
						});
					}
					double num6;
					this.ComputeFirstItemInViewportIndexAndOffset(itemsInternal, count, containItemStorage, empty, virtualizationCacheLength, flag3, areContainersUniformlySized, num4, out num2, out num6, out minValue, out flag5);
					double num7;
					this.ComputeFirstItemInViewportIndexAndOffset(itemsInternal, count, containItemStorage, empty2, new VirtualizationCacheLength(0.0), flag3, areContainersUniformlySized, num4, out num3, out num7, out minValue2, out flag7);
					if (this.IsVirtualizing && !remeasure && this.InRecyclingMode)
					{
						int num8 = this._itemsInExtendedViewportCount;
						if (!isVSP45Compat)
						{
							double num9 = Math.Min(1.0, flag3 ? (empty.Width / empty2.Width) : (empty.Height / empty2.Height));
							int num10 = (int)Math.Ceiling(num9 * (double)this._itemsInExtendedViewportCount);
							num8 = Math.Max(num8, minValue + num10 - minValue2);
						}
						this.CleanupContainers(minValue2, num8, itemsControl);
					}
					Size size9 = constraint;
					if (flag3)
					{
						size9.Width = double.PositiveInfinity;
						if (this.IsScrolling && this.CanVerticallyScroll)
						{
							size9.Height = double.PositiveInfinity;
						}
					}
					else
					{
						size9.Height = double.PositiveInfinity;
						if (this.IsScrolling && this.CanHorizontallyScroll)
						{
							size9.Width = double.PositiveInfinity;
						}
					}
					remeasure = false;
					this._actualItemsInExtendedViewportCount = 0;
					this._firstItemInExtendedViewportIndex = 0;
					this._firstItemInExtendedViewportOffset = 0.0;
					this._firstItemInExtendedViewportChildIndex = 0;
					bool flag9 = false;
					int num11 = 0;
					bool flag10 = false;
					bool flag11 = false;
					bool flag12 = false;
					if (count > 0)
					{
						using (((ItemContainerGenerator)generator).GenerateBatches())
						{
							if (!flag5 || !this.IsEndOfCache(flag3, virtualizationCacheLength.CacheBeforeViewport, cacheUnit, size5, size6) || !this.IsEndOfViewport(flag3, empty, size3))
							{
								bool flag13 = false;
								do
								{
									flag13 = false;
									bool flag14 = false;
									bool isAfterFirstItem = false;
									bool isAfterLastItem = false;
									if (this.IsViewportEmpty(flag3, empty) && DoubleUtil.GreaterThan(virtualizationCacheLength.CacheBeforeViewport, 0.0))
									{
										flag14 = true;
									}
									int num12 = minValue;
									GeneratorPosition position = this.IndexToGeneratorPositionForStart(minValue, out num11);
									int num13 = num11;
									this._firstItemInExtendedViewportIndex = minValue;
									this._firstItemInExtendedViewportOffset = num2;
									this._firstItemInExtendedViewportChildIndex = num11;
									using (generator.StartAt(position, GeneratorDirection.Backward, true))
									{
										for (int i = num12; i >= 0; i--)
										{
											object item = itemsInternal[i];
											this.MeasureChild(ref generator, ref containItemStorage, ref containItemStorage2, ref obj, ref flag8, ref computedUniformOrAverageContainerSize, ref computedUniformOrAverageContainerPixelSize, ref computedAreContainersUniformlySized, ref flag12, ref itemsInternal, ref item, ref realizedChildren, ref this._firstItemInExtendedViewportChildIndex, ref flag9, ref flag3, ref size9, ref empty, ref virtualizationCacheLength, ref cacheUnit, ref scrollGeneration, ref flag5, ref num2, ref size, ref size3, ref size5, ref size7, ref size2, ref size4, ref size6, ref size8, ref flag4, i < minValue || flag14, isAfterFirstItem, isAfterLastItem, false, false, ref flag10, ref flag2);
											if (this.ItemsChangedDuringMeasure)
											{
												remeasure = true;
												goto IL_DEA;
											}
											this._actualItemsInExtendedViewportCount++;
											if (!flag5)
											{
												if (isVSP45Compat)
												{
													this.SyncUniformSizeFlags(obj, containItemStorage2, realizedChildren, itemsInternal, containItemStorage, count, computedAreContainersUniformlySized, computedUniformOrAverageContainerSize, ref areContainersUniformlySized, ref num4, ref flag11, flag3, false);
												}
												else
												{
													this.SyncUniformSizeFlags(obj, containItemStorage2, realizedChildren, itemsInternal, containItemStorage, count, computedAreContainersUniformlySized, computedUniformOrAverageContainerSize, computedUniformOrAverageContainerPixelSize, ref areContainersUniformlySized, ref num4, ref num5, ref flag11, flag3, false);
												}
												this.ComputeFirstItemInViewportIndexAndOffset(itemsInternal, count, containItemStorage, empty, virtualizationCacheLength, flag3, areContainersUniformlySized, num4, out num2, out num6, out minValue, out flag5);
												if (!flag5)
												{
													break;
												}
												if (i != minValue)
												{
													size = default(Size);
													size2 = default(Size);
													this._actualItemsInExtendedViewportCount--;
													flag13 = true;
													break;
												}
												this.MeasureChild(ref generator, ref containItemStorage, ref containItemStorage2, ref obj, ref flag8, ref computedUniformOrAverageContainerSize, ref computedUniformOrAverageContainerPixelSize, ref computedAreContainersUniformlySized, ref flag12, ref itemsInternal, ref item, ref realizedChildren, ref this._firstItemInExtendedViewportChildIndex, ref flag9, ref flag3, ref size9, ref empty, ref virtualizationCacheLength, ref cacheUnit, ref scrollGeneration, ref flag5, ref num2, ref size, ref size3, ref size5, ref size7, ref size2, ref size4, ref size6, ref size8, ref flag4, false, false, false, true, true, ref flag10, ref flag2);
												if (this.ItemsChangedDuringMeasure)
												{
													remeasure = true;
													goto IL_DEA;
												}
											}
											if (!isVSP45Compat && uielement == null && flag5 && i == num12 && 0 <= num13 && num13 < realizedChildren.Count)
											{
												uielement = (realizedChildren[num13] as UIElement);
												if (this.IsScrolling && this._scrollData._firstContainerInViewport != null && !areContainersUniformlySized)
												{
													Size size10;
													this.GetContainerSizeForItem(containItemStorage, item, flag3, areContainersUniformlySized, num4, out size10);
													double num14 = Math.Max(flag3 ? empty.X : empty.Y, 0.0);
													double num15 = flag3 ? size10.Width : size10.Height;
													if (!DoubleUtil.AreClose(num14, 0.0) && !LayoutDoubleUtil.LessThan(num14, num2 + num15))
													{
														double num16 = num15 - num6;
														if (!LayoutDoubleUtil.AreClose(num16, 0.0))
														{
															if (flag3)
															{
																VirtualizingStackPanel.ScrollData scrollData = this._scrollData;
																scrollData._offset.X = scrollData._offset.X + num16;
															}
															else
															{
																VirtualizingStackPanel.ScrollData scrollData2 = this._scrollData;
																scrollData2._offset.Y = scrollData2._offset.Y + num16;
															}
															if (VirtualizingStackPanel.ScrollTracer.IsEnabled && VirtualizingStackPanel.ScrollTracer.IsTracing(this))
															{
																VirtualizingStackPanel.ScrollTracer.Trace(this, VirtualizingStackPanel.ScrollTraceOp.SizeChangeDuringAnchorScroll, new object[]
																{
																	"fivOffset:",
																	num2,
																	"vpSpan:",
																	num14,
																	"oldCSpan:",
																	num6,
																	"newCSpan:",
																	num15,
																	"delta:",
																	num16,
																	"newVpOff:",
																	this._scrollData._offset
																});
															}
															remeasure = true;
															goto IL_DEA;
														}
													}
												}
											}
											if (this.IsEndOfCache(flag3, virtualizationCacheLength.CacheBeforeViewport, cacheUnit, size5, size6))
											{
												break;
											}
											this._firstItemInExtendedViewportIndex = Math.Max(this._firstItemInExtendedViewportIndex - 1, 0);
											this.IndexToGeneratorPositionForStart(this._firstItemInExtendedViewportIndex, out this._firstItemInExtendedViewportChildIndex);
											this._firstItemInExtendedViewportChildIndex = Math.Max(this._firstItemInExtendedViewportChildIndex, 0);
										}
									}
								}
								while (flag13);
								this.ComputeDistance(itemsInternal, containItemStorage, flag3, areContainersUniformlySized, num4, 0, this._firstItemInExtendedViewportIndex, out this._firstItemInExtendedViewportOffset);
							}
							if (flag5 && (!this.IsEndOfCache(flag3, virtualizationCacheLength.CacheAfterViewport, cacheUnit, size7, size8) || !this.IsEndOfViewport(flag3, empty, size3)))
							{
								bool isBeforeFirstItem = false;
								bool flag15 = false;
								int num17;
								bool flag16;
								if (this.IsViewportEmpty(flag3, empty))
								{
									num17 = 0;
									flag16 = true;
									flag15 = true;
								}
								else
								{
									num17 = minValue + 1;
									flag16 = true;
								}
								GeneratorPosition position = this.IndexToGeneratorPositionForStart(num17, out num11);
								using (generator.StartAt(position, GeneratorDirection.Forward, true))
								{
									int j = num17;
									while (j < count)
									{
										object obj2 = itemsInternal[j];
										this.MeasureChild(ref generator, ref containItemStorage, ref containItemStorage2, ref obj, ref flag8, ref computedUniformOrAverageContainerSize, ref computedUniformOrAverageContainerPixelSize, ref computedAreContainersUniformlySized, ref flag12, ref itemsInternal, ref obj2, ref realizedChildren, ref num11, ref flag9, ref flag3, ref size9, ref empty, ref virtualizationCacheLength, ref cacheUnit, ref scrollGeneration, ref flag5, ref num2, ref size, ref size3, ref size5, ref size7, ref size2, ref size4, ref size6, ref size8, ref flag4, isBeforeFirstItem, j > minValue || flag16, j > num || flag15, false, false, ref flag10, ref flag2);
										if (this.ItemsChangedDuringMeasure)
										{
											remeasure = true;
											goto IL_DEA;
										}
										this._actualItemsInExtendedViewportCount++;
										if (this.IsEndOfViewport(flag3, empty, size3))
										{
											if (!flag6)
											{
												flag6 = true;
												num = j;
											}
											if (this.IsEndOfCache(flag3, virtualizationCacheLength.CacheAfterViewport, cacheUnit, size7, size8))
											{
												break;
											}
										}
										j++;
										num11++;
									}
								}
							}
						}
					}
					if (this.IsVirtualizing && !this.IsPixelBased && (flag2 || hierarchicalVirtualizationAndScrollInfo != null) && (this.MeasureCaches || (DoubleUtil.AreClose(virtualizationCacheLength.CacheBeforeViewport, 0.0) && DoubleUtil.AreClose(virtualizationCacheLength.CacheAfterViewport, 0.0))))
					{
						int num18 = this._firstItemInExtendedViewportChildIndex + this._actualItemsInExtendedViewportCount;
						int count2 = realizedChildren.Count;
						for (int k = num18; k < count2; k++)
						{
							this.MeasureExistingChildBeyondExtendedViewport(ref generator, ref containItemStorage, ref containItemStorage2, ref obj, ref flag8, ref computedUniformOrAverageContainerSize, ref computedUniformOrAverageContainerPixelSize, ref computedAreContainersUniformlySized, ref flag12, ref itemsInternal, ref realizedChildren, ref k, ref flag9, ref flag3, ref size9, ref flag5, ref num2, ref flag4, ref flag2, ref flag10, ref scrollGeneration);
							if (this.ItemsChangedDuringMeasure)
							{
								remeasure = true;
								goto IL_DEA;
							}
						}
					}
					if (this._bringIntoViewContainer != null && !flag10)
					{
						num11 = realizedChildren.IndexOf(this._bringIntoViewContainer);
						if (num11 < 0)
						{
							this._bringIntoViewContainer = null;
						}
						else
						{
							this.MeasureExistingChildBeyondExtendedViewport(ref generator, ref containItemStorage, ref containItemStorage2, ref obj, ref flag8, ref computedUniformOrAverageContainerSize, ref computedUniformOrAverageContainerPixelSize, ref computedAreContainersUniformlySized, ref flag12, ref itemsInternal, ref realizedChildren, ref num11, ref flag9, ref flag3, ref size9, ref flag5, ref num2, ref flag4, ref flag2, ref flag10, ref scrollGeneration);
							if (this.ItemsChangedDuringMeasure)
							{
								remeasure = true;
								goto IL_DEA;
							}
						}
					}
					if (isVSP45Compat)
					{
						this.SyncUniformSizeFlags(obj, containItemStorage2, realizedChildren, itemsInternal, containItemStorage, count, computedAreContainersUniformlySized, computedUniformOrAverageContainerSize, ref areContainersUniformlySized, ref num4, ref flag11, flag3, false);
					}
					else
					{
						this.SyncUniformSizeFlags(obj, containItemStorage2, realizedChildren, itemsInternal, containItemStorage, count, computedAreContainersUniformlySized, computedUniformOrAverageContainerSize, computedUniformOrAverageContainerPixelSize, ref areContainersUniformlySized, ref num4, ref num5, ref flag11, flag3, false);
					}
					if (this.IsVirtualizing)
					{
						this.ExtendPixelAndLogicalSizes(realizedChildren, itemsInternal, count, containItemStorage, areContainersUniformlySized, num4, num5, ref size, ref size2, flag3, this._firstItemInExtendedViewportIndex, this._firstItemInExtendedViewportChildIndex, minValue, true);
						this.ExtendPixelAndLogicalSizes(realizedChildren, itemsInternal, count, containItemStorage, areContainersUniformlySized, num4, num5, ref size, ref size2, flag3, this._firstItemInExtendedViewportIndex + this._actualItemsInExtendedViewportCount, this._firstItemInExtendedViewportChildIndex + this._actualItemsInExtendedViewportCount, -1, false);
					}
					this._previousStackPixelSizeInViewport = size3;
					this._previousStackLogicalSizeInViewport = size4;
					this._previousStackPixelSizeInCacheBeforeViewport = size5;
					if (!this.IsPixelBased && DoubleUtil.GreaterThan(flag3 ? empty.Left : empty.Top, num2))
					{
						IHierarchicalVirtualizationAndScrollInfo virtualizingChild = VirtualizingStackPanel.GetVirtualizingChild(uielement);
						if (virtualizingChild != null)
						{
							Thickness itemsHostInsetForChild = this.GetItemsHostInsetForChild(virtualizingChild, null, null);
							this._pixelDistanceToViewport += (flag3 ? itemsHostInsetForChild.Left : itemsHostInsetForChild.Top);
							VirtualizingStackPanel virtualizingStackPanel = virtualizingChild.ItemsHost as VirtualizingStackPanel;
							if (virtualizingStackPanel != null)
							{
								this._pixelDistanceToViewport += virtualizingStackPanel._pixelDistanceToViewport;
							}
						}
					}
					if (double.IsInfinity(empty.Width))
					{
						empty.Width = size.Width;
					}
					if (double.IsInfinity(empty.Height))
					{
						empty.Height = size.Height;
					}
					this._extendedViewport = this.ExtendViewport(hierarchicalVirtualizationAndScrollInfo, flag3, empty, virtualizationCacheLength, cacheUnit, size5, size6, size7, size8, size, size2, ref this._itemsInExtendedViewportCount);
					this._viewport = empty;
					if (hierarchicalVirtualizationAndScrollInfo != null && base.IsVisible)
					{
						hierarchicalVirtualizationAndScrollInfo.ItemDesiredSizes = new HierarchicalVirtualizationItemDesiredSizes(size2, size4, size6, size8, size, size3, size5, size7);
						hierarchicalVirtualizationAndScrollInfo.MustDisableVirtualization = flag4;
					}
					if (this.MustDisableVirtualization != flag4)
					{
						this.MustDisableVirtualization = flag4;
						remeasure |= this.IsScrolling;
					}
					double newOffset = 0.0;
					if (!isVSP45Compat)
					{
						if (flag11 || flag12)
						{
							newOffset = this.ComputeEffectiveOffset(ref empty, uielement, minValue, num2, itemsInternal, containItemStorage, hierarchicalVirtualizationAndScrollInfo, flag3, areContainersUniformlySized, num4, scrollGeneration);
							if (uielement != null)
							{
								double num19;
								this.ComputeDistance(itemsInternal, containItemStorage, flag3, areContainersUniformlySized, num4, 0, this._firstItemInExtendedViewportIndex, out num19);
								if (VirtualizingStackPanel.ScrollTracer.IsEnabled && VirtualizingStackPanel.ScrollTracer.IsTracing(this))
								{
									VirtualizingStackPanel.ScrollTracer.Trace(this, VirtualizingStackPanel.ScrollTraceOp.ReviseArrangeOffset, new object[]
									{
										this._firstItemInExtendedViewportOffset,
										num19
									});
								}
								this._firstItemInExtendedViewportOffset = num19;
							}
							if (!this.IsScrolling)
							{
								DependencyObject dependencyObject = containItemStorage as DependencyObject;
								Panel panel = (dependencyObject != null) ? (VisualTreeHelper.GetParent(dependencyObject) as Panel) : null;
								if (panel != null)
								{
									panel.InvalidateMeasure();
								}
							}
						}
						if (this.HasVirtualizingChildren)
						{
							VirtualizingStackPanel.FirstContainerInformation value = new VirtualizingStackPanel.FirstContainerInformation(ref empty, uielement, minValue, num2, scrollGeneration);
							VirtualizingStackPanel.FirstContainerInformationField.SetValue(this, value);
						}
					}
					if (this.IsScrolling)
					{
						if (isVSP45Compat)
						{
							this.SetAndVerifyScrollingData(flag3, empty, constraint, ref size, ref size2, ref size3, ref size4, ref size5, ref size6, ref remeasure, ref lastPageSafeOffset, ref previouslyMeasuredOffsets);
						}
						else
						{
							this.SetAndVerifyScrollingData(flag3, empty, constraint, uielement, num2, flag11, newOffset, ref size, ref size2, ref size3, ref size4, ref size5, ref size6, ref remeasure, ref lastPageSafeOffset, ref lastPagePixelSize, ref previouslyMeasuredOffsets);
						}
					}
					IL_DEA:
					if (!remeasure)
					{
						if (this.IsVirtualizing)
						{
							if (this.InRecyclingMode)
							{
								this.DisconnectRecycledContainers();
								if (flag9)
								{
									base.InvalidateZState();
								}
							}
							else
							{
								this.EnsureCleanupOperation(false);
							}
						}
						this.HasVirtualizingChildren = flag2;
					}
					if (VirtualizingStackPanel.ScrollTracer.IsEnabled && VirtualizingStackPanel.ScrollTracer.IsTracing(this))
					{
						DependencyObject dependencyObject2 = hierarchicalVirtualizationAndScrollInfo as DependencyObject;
						VirtualizingStackPanel.EffectiveOffsetInformation effectiveOffsetInformation = (dependencyObject2 != null) ? VirtualizingStackPanel.EffectiveOffsetInformationField.GetValue(dependencyObject2) : null;
						VirtualizingStackPanel.SnapshotData value2 = new VirtualizingStackPanel.SnapshotData
						{
							UniformOrAverageContainerSize = num5,
							UniformOrAverageContainerPixelSize = num5,
							EffectiveOffsets = ((effectiveOffsetInformation != null) ? effectiveOffsetInformation.OffsetList : null)
						};
						VirtualizingStackPanel.SnapshotDataField.SetValue(this, value2);
					}
				}
			}
			finally
			{
				if (flag)
				{
					EventTrace.EventProvider.TraceEvent(EventTrace.Event.WClientStringEnd, EventTrace.Keyword.KeywordGeneral, EventTrace.Level.Info, "VirtualizingStackPanel :MeasureOverride");
				}
			}
			if (VirtualizingStackPanel.ScrollTracer.IsEnabled && VirtualizingStackPanel.ScrollTracer.IsTracing(this))
			{
				VirtualizingStackPanel.ScrollTracer.Trace(this, VirtualizingStackPanel.ScrollTraceOp.EndMeasure, new object[]
				{
					size,
					remeasure
				});
			}
			if (remeasure)
			{
				if (!VirtualizingStackPanel.IsVSP45Compat && this.IsScrolling)
				{
					this.IncrementScrollGeneration();
				}
				return this.MeasureOverrideImpl(constraint, ref lastPageSafeOffset, ref previouslyMeasuredOffsets, ref lastPagePixelSize, remeasure);
			}
			return size;
		}

		// Token: 0x06005A70 RID: 23152 RVA: 0x00190F10 File Offset: 0x0018F110
		private Size MeasureNonItemsHost(Size constraint)
		{
			return StackPanel.StackMeasureHelper(this, this._scrollData, constraint);
		}

		// Token: 0x06005A71 RID: 23153 RVA: 0x00190F1F File Offset: 0x0018F11F
		private Size ArrangeNonItemsHost(Size arrangeSize)
		{
			return StackPanel.StackArrangeHelper(this, this._scrollData, arrangeSize);
		}

		/// <summary>Arranges the content of a <see cref="T:System.Windows.Controls.VirtualizingStackPanel" /> element.</summary>
		/// <param name="arrangeSize">The <see cref="T:System.Windows.Size" /> that this element should use to arrange its child elements.</param>
		/// <returns>The <see cref="T:System.Windows.Size" /> that represents the arranged size of this <see cref="T:System.Windows.Controls.VirtualizingStackPanel" /> element and its child elements.</returns>
		// Token: 0x06005A72 RID: 23154 RVA: 0x00190F30 File Offset: 0x0018F130
		protected override Size ArrangeOverride(Size arrangeSize)
		{
			bool flag = this.IsScrolling && EventTrace.IsEnabled(EventTrace.Keyword.KeywordGeneral, EventTrace.Level.Info);
			if (flag)
			{
				EventTrace.EventProvider.TraceEvent(EventTrace.Event.WClientStringBegin, EventTrace.Keyword.KeywordGeneral, EventTrace.Level.Info, "VirtualizingStackPanel :ArrangeOverride");
			}
			try
			{
				if (!base.IsItemsHost)
				{
					this.ArrangeNonItemsHost(arrangeSize);
				}
				else
				{
					ItemsControl itemsControl = null;
					GroupItem groupItem = null;
					IHierarchicalVirtualizationAndScrollInfo hierarchicalVirtualizationAndScrollInfo = null;
					IContainItemStorage containItemStorage = null;
					object item = null;
					bool flag2 = this.Orientation == Orientation.Horizontal;
					bool flag3 = false;
					IContainItemStorage containItemStorage2;
					this.GetOwners(false, flag2, out itemsControl, out groupItem, out containItemStorage, out hierarchicalVirtualizationAndScrollInfo, out item, out containItemStorage2, out flag3);
					if (VirtualizingStackPanel.ScrollTracer.IsEnabled && VirtualizingStackPanel.ScrollTracer.IsTracing(this))
					{
						VirtualizingStackPanel.ScrollTracer.Trace(this, VirtualizingStackPanel.ScrollTraceOp.BeginArrange, new object[]
						{
							arrangeSize,
							"ptv:",
							this._pixelDistanceToViewport,
							"ptfc:",
							this._pixelDistanceToFirstContainerInExtendedViewport
						});
					}
					base.EnsureGenerator();
					IList realizedChildren = this.RealizedChildren;
					IItemContainerGenerator generator = base.Generator;
					IList itemsInternal = ((ItemContainerGenerator)generator).ItemsInternal;
					int count = itemsInternal.Count;
					IContainItemStorage itemStorageProvider = VirtualizingStackPanel.IsVSP45Compat ? containItemStorage : containItemStorage2;
					bool areContainersUniformlySized = this.GetAreContainersUniformlySized(itemStorageProvider, item);
					double uniformOrAverageContainerSize;
					double num;
					this.GetUniformOrAverageContainerSize(itemStorageProvider, item, this.IsPixelBased || VirtualizingStackPanel.IsVSP45Compat, out uniformOrAverageContainerSize, out num);
					ScrollViewer scrollOwner = this.ScrollOwner;
					double num2 = 0.0;
					if (scrollOwner != null && scrollOwner.CanContentScroll)
					{
						num2 = this.GetMaxChildArrangeLength(realizedChildren, flag2);
					}
					num2 = Math.Max(flag2 ? arrangeSize.Height : arrangeSize.Width, num2);
					Size childDesiredSize = Size.Empty;
					Rect rect = new Rect(arrangeSize);
					Size size = default(Size);
					int num3 = -1;
					Point point = default(Point);
					bool isVSP45Compat = VirtualizingStackPanel.IsVSP45Compat;
					for (int i = this._firstItemInExtendedViewportChildIndex; i < realizedChildren.Count; i++)
					{
						UIElement uielement = (UIElement)realizedChildren[i];
						childDesiredSize = uielement.DesiredSize;
						if (i >= this._firstItemInExtendedViewportChildIndex && i < this._firstItemInExtendedViewportChildIndex + this._actualItemsInExtendedViewportCount)
						{
							if (i == this._firstItemInExtendedViewportChildIndex)
							{
								this.ArrangeFirstItemInExtendedViewport(flag2, uielement, childDesiredSize, num2, ref rect, ref size, ref point, ref num3);
								Size childDesiredSize2 = Size.Empty;
								Rect rect2 = rect;
								Size desiredSize = uielement.DesiredSize;
								int num4 = num3;
								Point point2 = point;
								for (int j = this._firstItemInExtendedViewportChildIndex - 1; j >= 0; j--)
								{
									UIElement uielement2 = (UIElement)realizedChildren[j];
									childDesiredSize2 = uielement2.DesiredSize;
									this.ArrangeItemsBeyondTheExtendedViewport(flag2, uielement2, childDesiredSize2, num2, itemsInternal, generator, containItemStorage, areContainersUniformlySized, uniformOrAverageContainerSize, true, ref rect2, ref desiredSize, ref point2, ref num4);
									if (!isVSP45Compat)
									{
										this.SetItemsHostInsetForChild(j, uielement2, containItemStorage, flag2);
									}
								}
							}
							else
							{
								this.ArrangeOtherItemsInExtendedViewport(flag2, uielement, childDesiredSize, num2, i, ref rect, ref size, ref point, ref num3);
							}
						}
						else
						{
							this.ArrangeItemsBeyondTheExtendedViewport(flag2, uielement, childDesiredSize, num2, itemsInternal, generator, containItemStorage, areContainersUniformlySized, uniformOrAverageContainerSize, false, ref rect, ref size, ref point, ref num3);
						}
						if (!isVSP45Compat)
						{
							this.SetItemsHostInsetForChild(i, uielement, containItemStorage, flag2);
						}
					}
					if (VirtualizingStackPanel.ScrollTracer.IsEnabled && VirtualizingStackPanel.ScrollTracer.IsTracing(this))
					{
						DependencyObject dependencyObject = hierarchicalVirtualizationAndScrollInfo as DependencyObject;
						VirtualizingStackPanel.EffectiveOffsetInformation effectiveOffsetInformation = (dependencyObject != null) ? VirtualizingStackPanel.EffectiveOffsetInformationField.GetValue(dependencyObject) : null;
						VirtualizingStackPanel.SnapshotData value = new VirtualizingStackPanel.SnapshotData
						{
							UniformOrAverageContainerSize = num,
							UniformOrAverageContainerPixelSize = num,
							EffectiveOffsets = ((effectiveOffsetInformation != null) ? effectiveOffsetInformation.OffsetList : null)
						};
						VirtualizingStackPanel.SnapshotDataField.SetValue(this, value);
						VirtualizingStackPanel.ScrollTracer.Trace(this, VirtualizingStackPanel.ScrollTraceOp.EndArrange, new object[]
						{
							arrangeSize,
							this._firstItemInExtendedViewportIndex,
							this._firstItemInExtendedViewportOffset
						});
					}
				}
			}
			finally
			{
				if (flag)
				{
					EventTrace.EventProvider.TraceEvent(EventTrace.Event.WClientStringEnd, EventTrace.Keyword.KeywordGeneral, EventTrace.Level.Info, "VirtualizingStackPanel :ArrangeOverride");
				}
			}
			return arrangeSize;
		}

		/// <summary>Called when the <see cref="P:System.Windows.Controls.ItemsControl.Items" /> collection that is associated with the <see cref="T:System.Windows.Controls.ItemsControl" /> for this <see cref="T:System.Windows.Controls.Panel" /> changes.</summary>
		/// <param name="sender">The <see cref="T:System.Object" /> that raised the event.</param>
		/// <param name="args">Provides data for the <see cref="E:System.Windows.Controls.ItemContainerGenerator.ItemsChanged" /> event.</param>
		// Token: 0x06005A73 RID: 23155 RVA: 0x001912FC File Offset: 0x0018F4FC
		protected override void OnItemsChanged(object sender, ItemsChangedEventArgs args)
		{
			if (VirtualizingStackPanel.ScrollTracer.IsEnabled && VirtualizingStackPanel.ScrollTracer.IsTracing(this))
			{
				VirtualizingStackPanel.ScrollTracer.Trace(this, VirtualizingStackPanel.ScrollTraceOp.ItemsChanged, new object[]
				{
					args.Action,
					"pos:",
					args.OldPosition,
					args.Position,
					"count:",
					args.ItemCount,
					args.ItemUICount,
					base.MeasureInProgress ? "MeasureInProgress" : string.Empty
				});
			}
			if (base.MeasureInProgress)
			{
				this.ItemsChangedDuringMeasure = true;
			}
			base.OnItemsChanged(sender, args);
			bool flag = false;
			switch (args.Action)
			{
			case NotifyCollectionChangedAction.Remove:
				this.OnItemsRemove(args);
				flag = true;
				break;
			case NotifyCollectionChangedAction.Replace:
				this.OnItemsReplace(args);
				flag = true;
				break;
			case NotifyCollectionChangedAction.Move:
				this.OnItemsMove(args);
				break;
			case NotifyCollectionChangedAction.Reset:
			{
				flag = true;
				IContainItemStorage itemStorageProvider = VirtualizingStackPanel.GetItemStorageProvider(this);
				itemStorageProvider.Clear();
				this.ClearAsyncOperations();
				break;
			}
			}
			if (flag && this.IsScrolling)
			{
				this.ResetMaximumDesiredSize();
			}
		}

		// Token: 0x06005A74 RID: 23156 RVA: 0x00191415 File Offset: 0x0018F615
		internal void ResetMaximumDesiredSize()
		{
			if (this.IsScrolling)
			{
				this._scrollData._maxDesiredSize = default(Size);
			}
		}

		/// <summary>Returns a value that indicates whether a changed item in an <see cref="T:System.Windows.Controls.ItemsControl" /> affects the layout for this panel.</summary>
		/// <param name="areItemChangesLocal">
		///       <see langword="true" /> if the changed item is a direct child of this <see cref="T:System.Windows.Controls.VirtualizingPanel" />; <see langword="false" /> if the changed item is an indirect descendant of the <see cref="T:System.Windows.Controls.VirtualizingPanel" />.</param>
		/// <param name="args">Contains data regarding the changed item.</param>
		/// <returns>
		///     <see langword="true" /> if the changed item in an <see cref="T:System.Windows.Controls.ItemsControl" /> affects the layout for this panel; otherwise, <see langword="false" />.</returns>
		// Token: 0x06005A75 RID: 23157 RVA: 0x00191430 File Offset: 0x0018F630
		protected override bool ShouldItemsChangeAffectLayoutCore(bool areItemChangesLocal, ItemsChangedEventArgs args)
		{
			bool flag = true;
			if (this.IsVirtualizing)
			{
				if (areItemChangesLocal)
				{
					switch (args.Action)
					{
					case NotifyCollectionChangedAction.Add:
					{
						int num = base.Generator.IndexFromGeneratorPosition(args.Position);
						flag = (num < this._firstItemInExtendedViewportIndex + this._itemsInExtendedViewportCount);
						break;
					}
					case NotifyCollectionChangedAction.Remove:
					{
						int num2 = base.Generator.IndexFromGeneratorPosition(args.OldPosition);
						flag = (args.ItemUICount > 0 || num2 < this._firstItemInExtendedViewportIndex + this._itemsInExtendedViewportCount);
						break;
					}
					case NotifyCollectionChangedAction.Replace:
						flag = (args.ItemUICount > 0);
						break;
					case NotifyCollectionChangedAction.Move:
					{
						int num3 = base.Generator.IndexFromGeneratorPosition(args.Position);
						int num4 = base.Generator.IndexFromGeneratorPosition(args.OldPosition);
						flag = (num3 < this._firstItemInExtendedViewportIndex + this._itemsInExtendedViewportCount || num4 < this._firstItemInExtendedViewportIndex + this._itemsInExtendedViewportCount);
						break;
					}
					}
				}
				else
				{
					int num5 = base.Generator.IndexFromGeneratorPosition(args.Position);
					flag = (num5 != this._firstItemInExtendedViewportIndex + this._itemsInExtendedViewportCount - 1);
				}
				if (!flag)
				{
					if (this.IsScrolling)
					{
						flag = !this.IsExtendedViewportFull();
						if (!flag)
						{
							this.UpdateExtent(areItemChangesLocal);
						}
					}
					else
					{
						DependencyObject itemsOwnerInternal = ItemsControl.GetItemsOwnerInternal(this);
						VirtualizingPanel virtualizingPanel = VisualTreeHelper.GetParent(itemsOwnerInternal) as VirtualizingPanel;
						if (virtualizingPanel != null)
						{
							this.UpdateExtent(areItemChangesLocal);
							IItemContainerGenerator itemContainerGenerator = virtualizingPanel.ItemContainerGenerator;
							int itemIndex = ((ItemContainerGenerator)itemContainerGenerator).IndexFromContainer(itemsOwnerInternal, true);
							ItemsChangedEventArgs args2 = new ItemsChangedEventArgs(NotifyCollectionChangedAction.Reset, itemContainerGenerator.GeneratorPositionFromIndex(itemIndex), 1, 1);
							flag = virtualizingPanel.ShouldItemsChangeAffectLayout(false, args2);
						}
						else
						{
							flag = true;
						}
					}
				}
			}
			return flag;
		}

		// Token: 0x06005A76 RID: 23158 RVA: 0x001915D4 File Offset: 0x0018F7D4
		private void UpdateExtent(bool areItemChangesLocal)
		{
			bool flag = this.Orientation == Orientation.Horizontal;
			bool isVSP45Compat = VirtualizingStackPanel.IsVSP45Compat;
			ItemsControl itemsControl;
			GroupItem groupItem;
			IContainItemStorage containItemStorage;
			IHierarchicalVirtualizationAndScrollInfo hierarchicalVirtualizationAndScrollInfo;
			object obj;
			IContainItemStorage containItemStorage2;
			bool flag2;
			this.GetOwners(false, flag, out itemsControl, out groupItem, out containItemStorage, out hierarchicalVirtualizationAndScrollInfo, out obj, out containItemStorage2, out flag2);
			IContainItemStorage itemStorageProvider = isVSP45Compat ? containItemStorage : containItemStorage2;
			bool areContainersUniformlySized = this.GetAreContainersUniformlySized(itemStorageProvider, obj);
			double num;
			double num2;
			this.GetUniformOrAverageContainerSize(itemStorageProvider, obj, isVSP45Compat || this.IsPixelBased, out num, out num2);
			IList realizedChildren = this.RealizedChildren;
			IItemContainerGenerator generator = base.Generator;
			IList itemsInternal = ((ItemContainerGenerator)generator).ItemsInternal;
			int count = itemsInternal.Count;
			if (!areItemChangesLocal)
			{
				double computedUniformOrAverageContainerSize = num;
				double computedUniformOrAverageContainerPixelSize = num2;
				bool computedAreContainersUniformlySized = areContainersUniformlySized;
				bool flag3 = false;
				if (isVSP45Compat)
				{
					this.SyncUniformSizeFlags(obj, containItemStorage2, realizedChildren, itemsInternal, containItemStorage, count, computedAreContainersUniformlySized, computedUniformOrAverageContainerSize, ref areContainersUniformlySized, ref num, ref flag3, flag, true);
				}
				else
				{
					this.SyncUniformSizeFlags(obj, containItemStorage2, realizedChildren, itemsInternal, containItemStorage, count, computedAreContainersUniformlySized, computedUniformOrAverageContainerSize, computedUniformOrAverageContainerPixelSize, ref areContainersUniformlySized, ref num, ref num2, ref flag3, flag, true);
				}
				if (flag3 && !VirtualizingStackPanel.IsVSP45Compat)
				{
					VirtualizingStackPanel.FirstContainerInformation value = VirtualizingStackPanel.FirstContainerInformationField.GetValue(this);
					if (value != null)
					{
						this.ComputeEffectiveOffset(ref value.Viewport, value.FirstContainer, value.FirstItemIndex, value.FirstItemOffset, itemsInternal, containItemStorage, hierarchicalVirtualizationAndScrollInfo, flag, areContainersUniformlySized, num, value.ScrollGeneration);
					}
				}
			}
			double num3 = 0.0;
			this.ComputeDistance(itemsInternal, containItemStorage, flag, areContainersUniformlySized, num, 0, itemsInternal.Count, out num3);
			if (this.IsScrolling)
			{
				if (flag)
				{
					this._scrollData._extent.Width = num3;
				}
				else
				{
					this._scrollData._extent.Height = num3;
				}
				this.ScrollOwner.InvalidateScrollInfo();
				return;
			}
			if (hierarchicalVirtualizationAndScrollInfo != null)
			{
				HierarchicalVirtualizationItemDesiredSizes itemDesiredSizes = hierarchicalVirtualizationAndScrollInfo.ItemDesiredSizes;
				if (this.IsPixelBased)
				{
					Size pixelSize = itemDesiredSizes.PixelSize;
					if (flag)
					{
						pixelSize.Width = num3;
					}
					else
					{
						pixelSize.Height = num3;
					}
					itemDesiredSizes = new HierarchicalVirtualizationItemDesiredSizes(itemDesiredSizes.LogicalSize, itemDesiredSizes.LogicalSizeInViewport, itemDesiredSizes.LogicalSizeBeforeViewport, itemDesiredSizes.LogicalSizeAfterViewport, pixelSize, itemDesiredSizes.PixelSizeInViewport, itemDesiredSizes.PixelSizeBeforeViewport, itemDesiredSizes.PixelSizeAfterViewport);
				}
				else
				{
					Size logicalSize = itemDesiredSizes.LogicalSize;
					if (flag)
					{
						logicalSize.Width = num3;
					}
					else
					{
						logicalSize.Height = num3;
					}
					itemDesiredSizes = new HierarchicalVirtualizationItemDesiredSizes(logicalSize, itemDesiredSizes.LogicalSizeInViewport, itemDesiredSizes.LogicalSizeBeforeViewport, itemDesiredSizes.LogicalSizeAfterViewport, itemDesiredSizes.PixelSize, itemDesiredSizes.PixelSizeInViewport, itemDesiredSizes.PixelSizeBeforeViewport, itemDesiredSizes.PixelSizeAfterViewport);
				}
				hierarchicalVirtualizationAndScrollInfo.ItemDesiredSizes = itemDesiredSizes;
			}
		}

		// Token: 0x06005A77 RID: 23159 RVA: 0x00191844 File Offset: 0x0018FA44
		private bool IsExtendedViewportFull()
		{
			bool flag = this.Orientation == Orientation.Horizontal;
			bool flag2 = (flag && DoubleUtil.GreaterThanOrClose(base.DesiredSize.Width, base.PreviousConstraint.Width)) || (!flag && DoubleUtil.GreaterThanOrClose(base.DesiredSize.Height, base.PreviousConstraint.Height));
			if (flag2)
			{
				IHierarchicalVirtualizationAndScrollInfo virtualizationInfoProvider = null;
				Rect viewport = this._viewport;
				Rect extendedViewport = this._extendedViewport;
				Rect rect = Rect.Empty;
				VirtualizationCacheLength cacheLength = VirtualizingPanel.GetCacheLength(this);
				VirtualizationCacheLengthUnit cacheLengthUnit = VirtualizingPanel.GetCacheLengthUnit(this);
				int itemsInExtendedViewportCount = this._itemsInExtendedViewportCount;
				this.NormalizeCacheLength(flag, viewport, ref cacheLength, ref cacheLengthUnit);
				rect = this.ExtendViewport(virtualizationInfoProvider, flag, viewport, cacheLength, cacheLengthUnit, Size.Empty, Size.Empty, Size.Empty, Size.Empty, Size.Empty, Size.Empty, ref itemsInExtendedViewportCount);
				return (flag && DoubleUtil.GreaterThanOrClose(extendedViewport.Width, rect.Width)) || (!flag && DoubleUtil.GreaterThanOrClose(extendedViewport.Height, rect.Height));
			}
			return false;
		}

		/// <summary>Called when the collection of child elements is cleared by the base <see cref="T:System.Windows.Controls.Panel" /> class.</summary>
		// Token: 0x06005A78 RID: 23160 RVA: 0x00191954 File Offset: 0x0018FB54
		protected override void OnClearChildren()
		{
			base.OnClearChildren();
			if (this.IsVirtualizing && base.IsItemsHost)
			{
				ItemsControl itemsControl;
				ItemsControl.GetItemsOwnerInternal(this, out itemsControl);
				this.CleanupContainers(int.MaxValue, int.MaxValue, itemsControl);
			}
			if (this._realizedChildren != null)
			{
				this._realizedChildren.Clear();
			}
			base.InternalChildren.ClearInternal();
		}

		// Token: 0x06005A79 RID: 23161 RVA: 0x001919B0 File Offset: 0x0018FBB0
		private void OnIsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
		{
			if (!(bool)e.NewValue)
			{
				IHierarchicalVirtualizationAndScrollInfo virtualizingProvider = this.GetVirtualizingProvider();
				if (virtualizingProvider != null)
				{
					Helper.ClearVirtualizingElement(virtualizingProvider);
				}
				this.ClearAsyncOperations();
				return;
			}
			base.InvalidateMeasure();
		}

		// Token: 0x06005A7A RID: 23162 RVA: 0x001919E8 File Offset: 0x0018FBE8
		internal void ClearAllContainers()
		{
			IItemContainerGenerator generator = base.Generator;
			if (generator != null)
			{
				generator.RemoveAll();
			}
		}

		// Token: 0x06005A7B RID: 23163 RVA: 0x00191A08 File Offset: 0x0018FC08
		private IHierarchicalVirtualizationAndScrollInfo GetVirtualizingProvider()
		{
			ItemsControl element = null;
			DependencyObject itemsOwnerInternal = ItemsControl.GetItemsOwnerInternal(this, out element);
			if (itemsOwnerInternal is GroupItem)
			{
				return VirtualizingStackPanel.GetVirtualizingProvider(itemsOwnerInternal);
			}
			return VirtualizingStackPanel.GetVirtualizingProvider(element);
		}

		// Token: 0x06005A7C RID: 23164 RVA: 0x00191A38 File Offset: 0x0018FC38
		private static IHierarchicalVirtualizationAndScrollInfo GetVirtualizingProvider(DependencyObject element)
		{
			IHierarchicalVirtualizationAndScrollInfo hierarchicalVirtualizationAndScrollInfo = element as IHierarchicalVirtualizationAndScrollInfo;
			if (hierarchicalVirtualizationAndScrollInfo != null)
			{
				VirtualizingPanel virtualizingPanel = VisualTreeHelper.GetParent(element) as VirtualizingPanel;
				if (virtualizingPanel == null || !virtualizingPanel.CanHierarchicallyScrollAndVirtualize)
				{
					hierarchicalVirtualizationAndScrollInfo = null;
				}
			}
			return hierarchicalVirtualizationAndScrollInfo;
		}

		// Token: 0x06005A7D RID: 23165 RVA: 0x00191A6C File Offset: 0x0018FC6C
		private static IHierarchicalVirtualizationAndScrollInfo GetVirtualizingChild(DependencyObject element)
		{
			bool flag = false;
			return VirtualizingStackPanel.GetVirtualizingChild(element, ref flag);
		}

		// Token: 0x06005A7E RID: 23166 RVA: 0x00191A84 File Offset: 0x0018FC84
		private static IHierarchicalVirtualizationAndScrollInfo GetVirtualizingChild(DependencyObject element, ref bool isChildHorizontal)
		{
			IHierarchicalVirtualizationAndScrollInfo hierarchicalVirtualizationAndScrollInfo = element as IHierarchicalVirtualizationAndScrollInfo;
			if (hierarchicalVirtualizationAndScrollInfo != null && hierarchicalVirtualizationAndScrollInfo.ItemsHost != null)
			{
				isChildHorizontal = (hierarchicalVirtualizationAndScrollInfo.ItemsHost.LogicalOrientationPublic == Orientation.Horizontal);
				VirtualizingPanel virtualizingPanel = hierarchicalVirtualizationAndScrollInfo.ItemsHost as VirtualizingPanel;
				if (virtualizingPanel == null || !virtualizingPanel.CanHierarchicallyScrollAndVirtualize)
				{
					hierarchicalVirtualizationAndScrollInfo = null;
				}
			}
			return hierarchicalVirtualizationAndScrollInfo;
		}

		// Token: 0x06005A7F RID: 23167 RVA: 0x00191AD0 File Offset: 0x0018FCD0
		private static IContainItemStorage GetItemStorageProvider(Panel itemsHost)
		{
			ItemsControl itemsControl = null;
			DependencyObject itemsOwnerInternal = ItemsControl.GetItemsOwnerInternal(itemsHost, out itemsControl);
			if (itemsOwnerInternal != itemsControl)
			{
				GroupItem groupItem = itemsOwnerInternal as GroupItem;
			}
			return itemsOwnerInternal as IContainItemStorage;
		}

		// Token: 0x06005A80 RID: 23168 RVA: 0x00191AFC File Offset: 0x0018FCFC
		private void GetOwners(bool shouldSetVirtualizationState, bool isHorizontal, out ItemsControl itemsControl, out GroupItem groupItem, out IContainItemStorage itemStorageProvider, out IHierarchicalVirtualizationAndScrollInfo virtualizationInfoProvider, out object parentItem, out IContainItemStorage parentItemStorageProvider, out bool mustDisableVirtualization)
		{
			groupItem = null;
			parentItem = null;
			parentItemStorageProvider = null;
			bool isScrolling = this.IsScrolling;
			mustDisableVirtualization = (isScrolling && this.MustDisableVirtualization);
			DependencyObject itemsOwnerInternal = ItemsControl.GetItemsOwnerInternal(this, out itemsControl);
			if (itemsOwnerInternal != itemsControl)
			{
				groupItem = (itemsOwnerInternal as GroupItem);
				parentItem = itemsControl.ItemContainerGenerator.ItemFromContainer(groupItem);
			}
			else if (!isScrolling)
			{
				ItemsControl itemsControl2 = ItemsControl.GetItemsOwnerInternal(VisualTreeHelper.GetParent(itemsControl)) as ItemsControl;
				if (itemsControl2 != null)
				{
					parentItem = itemsControl2.ItemContainerGenerator.ItemFromContainer(itemsControl);
				}
				else
				{
					parentItem = this;
				}
			}
			else
			{
				parentItem = this;
			}
			itemStorageProvider = (itemsOwnerInternal as IContainItemStorage);
			virtualizationInfoProvider = null;
			parentItemStorageProvider = ((VirtualizingStackPanel.IsVSP45Compat || isScrolling || itemsOwnerInternal == null) ? null : (ItemsControl.GetItemsOwnerInternal(VisualTreeHelper.GetParent(itemsOwnerInternal)) as IContainItemStorage));
			if (groupItem != null)
			{
				virtualizationInfoProvider = VirtualizingStackPanel.GetVirtualizingProvider(groupItem);
				mustDisableVirtualization = (virtualizationInfoProvider != null && virtualizationInfoProvider.MustDisableVirtualization);
			}
			else if (!isScrolling)
			{
				virtualizationInfoProvider = VirtualizingStackPanel.GetVirtualizingProvider(itemsControl);
				mustDisableVirtualization = (virtualizationInfoProvider != null && virtualizationInfoProvider.MustDisableVirtualization);
			}
			if (shouldSetVirtualizationState)
			{
				if (VirtualizingStackPanel.ScrollTracer.IsEnabled)
				{
					VirtualizingStackPanel.ScrollTracer.ConfigureTracing(this, itemsOwnerInternal, parentItem, itemsControl);
				}
				this.SetVirtualizationState(itemStorageProvider, itemsControl, mustDisableVirtualization);
			}
		}

		// Token: 0x06005A81 RID: 23169 RVA: 0x00191C24 File Offset: 0x0018FE24
		private void SetVirtualizationState(IContainItemStorage itemStorageProvider, ItemsControl itemsControl, bool mustDisableVirtualization)
		{
			if (itemsControl != null)
			{
				bool isVirtualizing = VirtualizingPanel.GetIsVirtualizing(itemsControl);
				bool isVirtualizingWhenGrouping = VirtualizingPanel.GetIsVirtualizingWhenGrouping(itemsControl);
				VirtualizationMode virtualizationMode = VirtualizingPanel.GetVirtualizationMode(itemsControl);
				bool isGrouping = itemsControl.IsGrouping;
				this.IsVirtualizing = (!mustDisableVirtualization && ((!isGrouping && isVirtualizing) || (isGrouping && isVirtualizing && isVirtualizingWhenGrouping)));
				ScrollUnit scrollUnit = VirtualizingPanel.GetScrollUnit(itemsControl);
				bool isPixelBased = this.IsPixelBased;
				this.IsPixelBased = (mustDisableVirtualization || scrollUnit == ScrollUnit.Pixel);
				if (this.IsScrolling)
				{
					if (!this.HasMeasured || isPixelBased != this.IsPixelBased)
					{
						VirtualizingStackPanel.ClearItemValueStorageRecursive(itemStorageProvider, this);
					}
					VirtualizingPanel.SetCacheLength(this, VirtualizingPanel.GetCacheLength(itemsControl));
					VirtualizingPanel.SetCacheLengthUnit(this, VirtualizingPanel.GetCacheLengthUnit(itemsControl));
				}
				if (this.HasMeasured)
				{
					VirtualizationMode virtualizationMode2 = this.InRecyclingMode ? VirtualizationMode.Recycling : VirtualizationMode.Standard;
					if (virtualizationMode2 != virtualizationMode)
					{
						throw new InvalidOperationException(SR.Get("CantSwitchVirtualizationModePostMeasure"));
					}
				}
				else
				{
					this.HasMeasured = true;
				}
				this.InRecyclingMode = (virtualizationMode == VirtualizationMode.Recycling);
			}
		}

		// Token: 0x06005A82 RID: 23170 RVA: 0x00191D08 File Offset: 0x0018FF08
		private static void ClearItemValueStorageRecursive(IContainItemStorage itemStorageProvider, Panel itemsHost)
		{
			Helper.ClearItemValueStorage((DependencyObject)itemStorageProvider, VirtualizingStackPanel._indicesStoredInItemValueStorage);
			UIElementCollection internalChildren = itemsHost.InternalChildren;
			int count = internalChildren.Count;
			for (int i = 0; i < count; i++)
			{
				IHierarchicalVirtualizationAndScrollInfo hierarchicalVirtualizationAndScrollInfo = internalChildren[i] as IHierarchicalVirtualizationAndScrollInfo;
				if (hierarchicalVirtualizationAndScrollInfo != null)
				{
					Panel itemsHost2 = hierarchicalVirtualizationAndScrollInfo.ItemsHost;
					if (itemsHost2 != null)
					{
						IContainItemStorage itemStorageProvider2 = VirtualizingStackPanel.GetItemStorageProvider(itemsHost2);
						if (itemStorageProvider2 != null)
						{
							VirtualizingStackPanel.ClearItemValueStorageRecursive(itemStorageProvider2, itemsHost2);
						}
					}
				}
			}
		}

		// Token: 0x06005A83 RID: 23171 RVA: 0x00191D74 File Offset: 0x0018FF74
		private void InitializeViewport(object parentItem, IContainItemStorage parentItemStorageProvider, IHierarchicalVirtualizationAndScrollInfo virtualizationInfoProvider, bool isHorizontal, Size constraint, ref Rect viewport, ref VirtualizationCacheLength cacheSize, ref VirtualizationCacheLengthUnit cacheUnit, out Rect extendedViewport, out long scrollGeneration)
		{
			Size extent = default(Size);
			bool isVSP45Compat = VirtualizingStackPanel.IsVSP45Compat;
			if (this.IsScrolling)
			{
				Size size = constraint;
				double x = this._scrollData._offset.X;
				double y = this._scrollData._offset.Y;
				extent = this._scrollData._extent;
				Size viewport2 = this._scrollData._viewport;
				scrollGeneration = this._scrollData._scrollGeneration;
				if (!this.IsScrollActive || this.IgnoreMaxDesiredSize)
				{
					this._scrollData._maxDesiredSize = default(Size);
				}
				if (this.IsPixelBased)
				{
					viewport = new Rect(x, y, size.Width, size.Height);
					this.CoerceScrollingViewportOffset(ref viewport, extent, isHorizontal);
				}
				else
				{
					viewport = new Rect(x, y, viewport2.Width, viewport2.Height);
					this.CoerceScrollingViewportOffset(ref viewport, extent, isHorizontal);
					viewport.Size = size;
				}
				if (this.IsVirtualizing)
				{
					cacheSize = VirtualizingPanel.GetCacheLength(this);
					cacheUnit = VirtualizingPanel.GetCacheLengthUnit(this);
					if (DoubleUtil.GreaterThan(cacheSize.CacheBeforeViewport, 0.0) || DoubleUtil.GreaterThan(cacheSize.CacheAfterViewport, 0.0))
					{
						if (!this.MeasureCaches)
						{
							this.WasLastMeasurePassAnchored = (this._scrollData._firstContainerInViewport != null || this._scrollData._bringIntoViewLeafContainer != null);
							if (VirtualizingStackPanel.MeasureCachesOperationField.GetValue(this) == null)
							{
								Action measureCachesAction = null;
								int retryCount = 3;
								measureCachesAction = delegate()
								{
									int num2 = 0;
									int retryCount = retryCount;
									retryCount--;
									bool flag = num2 < retryCount && (this.MeasureDirty || this.ArrangeDirty);
									try
									{
										if (isVSP45Compat || !flag)
										{
											VirtualizingStackPanel.MeasureCachesOperationField.ClearValue(this);
											this.MeasureCaches = true;
											if (this.WasLastMeasurePassAnchored)
											{
												this.SetAnchorInformation(isHorizontal);
											}
											this.InvalidateMeasure();
											this.UpdateLayout();
										}
									}
									finally
									{
										flag = (flag || (0 < retryCount && (this.MeasureDirty || this.ArrangeDirty)));
										if (!isVSP45Compat && flag)
										{
											VirtualizingStackPanel.MeasureCachesOperationField.SetValue(this, this.Dispatcher.BeginInvoke(DispatcherPriority.Background, measureCachesAction));
										}
										this.MeasureCaches = false;
										if (VirtualizingStackPanel.AnchoredInvalidateMeasureOperationField.GetValue(this) == null && (isVSP45Compat || !flag))
										{
											if (isVSP45Compat)
											{
												this.IsScrollActive = false;
											}
											else if (this.IsScrollActive)
											{
												DispatcherOperation dispatcherOperation = VirtualizingStackPanel.ClearIsScrollActiveOperationField.GetValue(this);
												if (dispatcherOperation != null)
												{
													dispatcherOperation.Abort();
												}
												dispatcherOperation = this.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(this.ClearIsScrollActive));
												VirtualizingStackPanel.ClearIsScrollActiveOperationField.SetValue(this, dispatcherOperation);
											}
										}
									}
								};
								DispatcherOperation value = base.Dispatcher.BeginInvoke(DispatcherPriority.Background, measureCachesAction);
								VirtualizingStackPanel.MeasureCachesOperationField.SetValue(this, value);
							}
						}
					}
					else if (this.IsScrollActive && VirtualizingStackPanel.ClearIsScrollActiveOperationField.GetValue(this) == null)
					{
						DispatcherOperation value2 = base.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(this.ClearIsScrollActive));
						VirtualizingStackPanel.ClearIsScrollActiveOperationField.SetValue(this, value2);
					}
					this.NormalizeCacheLength(isHorizontal, viewport, ref cacheSize, ref cacheUnit);
				}
				else
				{
					cacheSize = new VirtualizationCacheLength(double.PositiveInfinity, this.IsViewportEmpty(isHorizontal, viewport) ? 0.0 : double.PositiveInfinity);
					cacheUnit = VirtualizationCacheLengthUnit.Pixel;
					this.ClearAsyncOperations();
				}
			}
			else if (virtualizationInfoProvider != null)
			{
				HierarchicalVirtualizationConstraints constraints = virtualizationInfoProvider.Constraints;
				viewport = constraints.Viewport;
				cacheSize = constraints.CacheLength;
				cacheUnit = constraints.CacheLengthUnit;
				scrollGeneration = constraints.ScrollGeneration;
				this.MeasureCaches = virtualizationInfoProvider.InBackgroundLayout;
				if (isVSP45Compat)
				{
					this.AdjustNonScrollingViewportForHeader(virtualizationInfoProvider, ref viewport, ref cacheSize, ref cacheUnit);
				}
				else
				{
					this.AdjustNonScrollingViewportForInset(isHorizontal, parentItem, parentItemStorageProvider, virtualizationInfoProvider, ref viewport, ref cacheSize, ref cacheUnit);
					DependencyObject instance = virtualizationInfoProvider as DependencyObject;
					VirtualizingStackPanel.EffectiveOffsetInformation value3 = VirtualizingStackPanel.EffectiveOffsetInformationField.GetValue(instance);
					if (value3 != null)
					{
						List<double> offsetList = value3.OffsetList;
						int num = -1;
						if (value3.ScrollGeneration >= scrollGeneration)
						{
							double value4 = isHorizontal ? viewport.X : viewport.Y;
							int i = 0;
							int count = offsetList.Count;
							while (i < count)
							{
								if (LayoutDoubleUtil.AreClose(value4, offsetList[i]))
								{
									num = i;
									break;
								}
								i++;
							}
						}
						if (VirtualizingStackPanel.ScrollTracer.IsEnabled && VirtualizingStackPanel.ScrollTracer.IsTracing(this))
						{
							object[] array = new object[offsetList.Count + 7];
							array[0] = "gen";
							array[1] = value3.ScrollGeneration;
							array[2] = constraints.ScrollGeneration;
							array[3] = viewport.Location;
							array[4] = "at";
							array[5] = num;
							array[6] = "in";
							for (int j = 0; j < offsetList.Count; j++)
							{
								array[j + 7] = offsetList[j];
							}
							VirtualizingStackPanel.ScrollTracer.Trace(this, VirtualizingStackPanel.ScrollTraceOp.UseSubstOffset, array);
						}
						if (num >= 0)
						{
							if (isHorizontal)
							{
								viewport.X = offsetList[offsetList.Count - 1];
							}
							else
							{
								viewport.Y = offsetList[offsetList.Count - 1];
							}
							offsetList.RemoveRange(0, num);
						}
						if (num < 0 || offsetList.Count <= 1)
						{
							VirtualizingStackPanel.EffectiveOffsetInformationField.ClearValue(instance);
						}
					}
				}
			}
			else
			{
				scrollGeneration = 0L;
				viewport = new Rect(0.0, 0.0, constraint.Width, constraint.Height);
				if (isHorizontal)
				{
					viewport.Width = double.PositiveInfinity;
				}
				else
				{
					viewport.Height = double.PositiveInfinity;
				}
			}
			extendedViewport = this._extendedViewport;
			if (isHorizontal)
			{
				extendedViewport.X += viewport.X - this._viewport.X;
			}
			else
			{
				extendedViewport.Y += viewport.Y - this._viewport.Y;
			}
			if (this.IsVirtualizing)
			{
				if (this.MeasureCaches)
				{
					this.IsMeasureCachesPending = false;
					return;
				}
				if (DoubleUtil.GreaterThan(cacheSize.CacheBeforeViewport, 0.0) || DoubleUtil.GreaterThan(cacheSize.CacheAfterViewport, 0.0))
				{
					this.IsMeasureCachesPending = true;
				}
			}
		}

		// Token: 0x06005A84 RID: 23172 RVA: 0x00192330 File Offset: 0x00190530
		private void ClearMeasureCachesState()
		{
			DispatcherOperation value = VirtualizingStackPanel.MeasureCachesOperationField.GetValue(this);
			if (value != null)
			{
				value.Abort();
				VirtualizingStackPanel.MeasureCachesOperationField.ClearValue(this);
			}
			this.IsMeasureCachesPending = false;
			if (this._cleanupOperation != null && this._cleanupOperation.Abort())
			{
				this._cleanupOperation = null;
			}
			if (this._cleanupDelay != null)
			{
				this._cleanupDelay.Stop();
				this._cleanupDelay = null;
			}
		}

		// Token: 0x06005A85 RID: 23173 RVA: 0x0019239C File Offset: 0x0019059C
		private void ClearIsScrollActive()
		{
			VirtualizingStackPanel.ClearIsScrollActiveOperationField.ClearValue(this);
			VirtualizingStackPanel.OffsetInformationField.ClearValue(this);
			this._scrollData._bringIntoViewLeafContainer = null;
			this.IsScrollActive = false;
			if (!VirtualizingStackPanel.IsVSP45Compat)
			{
				this._scrollData._offset = this._scrollData._computedOffset;
			}
		}

		// Token: 0x06005A86 RID: 23174 RVA: 0x001923F0 File Offset: 0x001905F0
		private void NormalizeCacheLength(bool isHorizontal, Rect viewport, ref VirtualizationCacheLength cacheLength, ref VirtualizationCacheLengthUnit cacheUnit)
		{
			if (cacheUnit == VirtualizationCacheLengthUnit.Page)
			{
				double num = isHorizontal ? viewport.Width : viewport.Height;
				if (double.IsPositiveInfinity(num))
				{
					cacheLength = new VirtualizationCacheLength(0.0, 0.0);
				}
				else
				{
					cacheLength = new VirtualizationCacheLength(cacheLength.CacheBeforeViewport * num, cacheLength.CacheAfterViewport * num);
				}
				cacheUnit = VirtualizationCacheLengthUnit.Pixel;
			}
			if (this.IsViewportEmpty(isHorizontal, viewport))
			{
				cacheLength = new VirtualizationCacheLength(0.0, 0.0);
			}
		}

		// Token: 0x06005A87 RID: 23175 RVA: 0x00192484 File Offset: 0x00190684
		private Rect ExtendViewport(IHierarchicalVirtualizationAndScrollInfo virtualizationInfoProvider, bool isHorizontal, Rect viewport, VirtualizationCacheLength cacheLength, VirtualizationCacheLengthUnit cacheUnit, Size stackPixelSizeInCacheBeforeViewport, Size stackLogicalSizeInCacheBeforeViewport, Size stackPixelSizeInCacheAfterViewport, Size stackLogicalSizeInCacheAfterViewport, Size stackPixelSize, Size stackLogicalSize, ref int itemsInExtendedViewportCount)
		{
			Rect rect = viewport;
			if (isHorizontal)
			{
				double num = (DoubleUtil.GreaterThan(this._previousStackPixelSizeInViewport.Width, 0.0) && DoubleUtil.GreaterThan(this._previousStackLogicalSizeInViewport.Width, 0.0)) ? (this._previousStackPixelSizeInViewport.Width / this._previousStackLogicalSizeInViewport.Width) : 16.0;
				double num2 = stackPixelSize.Width;
				double num3 = stackLogicalSize.Width;
				double num4;
				double num5;
				double num6;
				if (this.MeasureCaches)
				{
					num4 = stackPixelSizeInCacheBeforeViewport.Width;
					num5 = stackPixelSizeInCacheAfterViewport.Width;
					num6 = stackLogicalSizeInCacheBeforeViewport.Width;
					double num7 = stackLogicalSizeInCacheAfterViewport.Width;
				}
				else
				{
					num4 = ((cacheUnit == VirtualizationCacheLengthUnit.Item) ? (cacheLength.CacheBeforeViewport * num) : cacheLength.CacheBeforeViewport);
					num5 = ((cacheUnit == VirtualizationCacheLengthUnit.Item) ? (cacheLength.CacheAfterViewport * num) : cacheLength.CacheAfterViewport);
					num6 = ((cacheUnit == VirtualizationCacheLengthUnit.Item) ? cacheLength.CacheBeforeViewport : (cacheLength.CacheBeforeViewport / num));
					double num8 = (cacheUnit == VirtualizationCacheLengthUnit.Item) ? cacheLength.CacheAfterViewport : (cacheLength.CacheAfterViewport / num);
					if (this.IsPixelBased)
					{
						num4 = Math.Max(num4, Math.Abs(this._viewport.X - this._extendedViewport.X));
					}
					else
					{
						num6 = Math.Max(num6, Math.Abs(this._viewport.X - this._extendedViewport.X));
					}
				}
				if (this.IsPixelBased)
				{
					if (!this.IsScrolling && virtualizationInfoProvider != null && this.IsViewportEmpty(isHorizontal, rect) && DoubleUtil.GreaterThan(num4, 0.0))
					{
						rect.X = num2 - num4;
					}
					else
					{
						rect.X -= num4;
					}
					rect.Width += num4 + num5;
					if (this.IsScrolling)
					{
						if (DoubleUtil.LessThan(rect.X, 0.0))
						{
							rect.Width = Math.Max(rect.Width + rect.X, 0.0);
							rect.X = 0.0;
						}
						if (DoubleUtil.GreaterThan(rect.X + rect.Width, this._scrollData._extent.Width))
						{
							rect.Width = this._scrollData._extent.Width - rect.X;
						}
					}
				}
				else
				{
					if (!this.IsScrolling && virtualizationInfoProvider != null && this.IsViewportEmpty(isHorizontal, rect) && DoubleUtil.GreaterThan(num4, 0.0))
					{
						rect.X = num3 - num6;
					}
					else
					{
						rect.X -= num6;
					}
					rect.Width += num4 + num5;
					if (this.IsScrolling)
					{
						if (DoubleUtil.LessThan(rect.X, 0.0))
						{
							rect.Width = Math.Max(rect.Width / num + rect.X, 0.0) * num;
							rect.X = 0.0;
						}
						if (DoubleUtil.GreaterThan(rect.X + rect.Width / num, this._scrollData._extent.Width))
						{
							rect.Width = (this._scrollData._extent.Width - rect.X) * num;
						}
					}
				}
			}
			else
			{
				double num9 = (DoubleUtil.GreaterThan(this._previousStackPixelSizeInViewport.Height, 0.0) && DoubleUtil.GreaterThan(this._previousStackLogicalSizeInViewport.Height, 0.0)) ? (this._previousStackPixelSizeInViewport.Height / this._previousStackLogicalSizeInViewport.Height) : 16.0;
				double num2 = stackPixelSize.Height;
				double num3 = stackLogicalSize.Height;
				double num4;
				double num5;
				double num6;
				if (this.MeasureCaches)
				{
					num4 = stackPixelSizeInCacheBeforeViewport.Height;
					num5 = stackPixelSizeInCacheAfterViewport.Height;
					num6 = stackLogicalSizeInCacheBeforeViewport.Height;
					double num7 = stackLogicalSizeInCacheAfterViewport.Height;
				}
				else
				{
					num4 = ((cacheUnit == VirtualizationCacheLengthUnit.Item) ? (cacheLength.CacheBeforeViewport * num9) : cacheLength.CacheBeforeViewport);
					num5 = ((cacheUnit == VirtualizationCacheLengthUnit.Item) ? (cacheLength.CacheAfterViewport * num9) : cacheLength.CacheAfterViewport);
					num6 = ((cacheUnit == VirtualizationCacheLengthUnit.Item) ? cacheLength.CacheBeforeViewport : (cacheLength.CacheBeforeViewport / num9));
					double num10 = (cacheUnit == VirtualizationCacheLengthUnit.Item) ? cacheLength.CacheAfterViewport : (cacheLength.CacheAfterViewport / num9);
					if (this.IsPixelBased)
					{
						num4 = Math.Max(num4, Math.Abs(this._viewport.Y - this._extendedViewport.Y));
					}
					else
					{
						num6 = Math.Max(num6, Math.Abs(this._viewport.Y - this._extendedViewport.Y));
					}
				}
				if (this.IsPixelBased)
				{
					if (!this.IsScrolling && virtualizationInfoProvider != null && this.IsViewportEmpty(isHorizontal, rect) && DoubleUtil.GreaterThan(num4, 0.0))
					{
						rect.Y = num2 - num4;
					}
					else
					{
						rect.Y -= num4;
					}
					rect.Height += num4 + num5;
					if (this.IsScrolling)
					{
						if (DoubleUtil.LessThan(rect.Y, 0.0))
						{
							rect.Height = Math.Max(rect.Height + rect.Y, 0.0);
							rect.Y = 0.0;
						}
						if (DoubleUtil.GreaterThan(rect.Y + rect.Height, this._scrollData._extent.Height))
						{
							rect.Height = this._scrollData._extent.Height - rect.Y;
						}
					}
				}
				else
				{
					if (!this.IsScrolling && virtualizationInfoProvider != null && this.IsViewportEmpty(isHorizontal, rect) && DoubleUtil.GreaterThan(num4, 0.0))
					{
						rect.Y = num3 - num6;
					}
					else
					{
						rect.Y -= num6;
					}
					rect.Height += num4 + num5;
					if (this.IsScrolling)
					{
						if (DoubleUtil.LessThan(rect.Y, 0.0))
						{
							rect.Height = Math.Max(rect.Height / num9 + rect.Y, 0.0) * num9;
							rect.Y = 0.0;
						}
						if (DoubleUtil.GreaterThan(rect.Y + rect.Height / num9, this._scrollData._extent.Height))
						{
							rect.Height = (this._scrollData._extent.Height - rect.Y) * num9;
						}
					}
				}
			}
			if (this.MeasureCaches)
			{
				itemsInExtendedViewportCount = this._actualItemsInExtendedViewportCount;
			}
			else
			{
				double num11 = Math.Max(1.0, isHorizontal ? (rect.Width / viewport.Width) : (rect.Height / viewport.Height));
				int val = (int)Math.Ceiling(num11 * (double)this._actualItemsInExtendedViewportCount);
				itemsInExtendedViewportCount = Math.Max(val, itemsInExtendedViewportCount);
			}
			return rect;
		}

		// Token: 0x06005A88 RID: 23176 RVA: 0x00192BA8 File Offset: 0x00190DA8
		private void CoerceScrollingViewportOffset(ref Rect viewport, Size extent, bool isHorizontal)
		{
			if (!this._scrollData.IsEmpty)
			{
				viewport.X = ScrollContentPresenter.CoerceOffset(viewport.X, extent.Width, viewport.Width);
				if (!this.IsPixelBased && isHorizontal && DoubleUtil.IsZero(viewport.Width) && DoubleUtil.AreClose(viewport.X, extent.Width))
				{
					viewport.X = ScrollContentPresenter.CoerceOffset(viewport.X - 1.0, extent.Width, viewport.Width);
				}
			}
			if (!this._scrollData.IsEmpty)
			{
				viewport.Y = ScrollContentPresenter.CoerceOffset(viewport.Y, extent.Height, viewport.Height);
				if (!this.IsPixelBased && !isHorizontal && DoubleUtil.IsZero(viewport.Height) && DoubleUtil.AreClose(viewport.Y, extent.Height))
				{
					viewport.Y = ScrollContentPresenter.CoerceOffset(viewport.Y - 1.0, extent.Height, viewport.Height);
				}
			}
		}

		// Token: 0x06005A89 RID: 23177 RVA: 0x00192CB8 File Offset: 0x00190EB8
		private void AdjustNonScrollingViewportForHeader(IHierarchicalVirtualizationAndScrollInfo virtualizationInfoProvider, ref Rect viewport, ref VirtualizationCacheLength cacheLength, ref VirtualizationCacheLengthUnit cacheLengthUnit)
		{
			bool forHeader = true;
			this.AdjustNonScrollingViewport(virtualizationInfoProvider, ref viewport, ref cacheLength, ref cacheLengthUnit, forHeader);
		}

		// Token: 0x06005A8A RID: 23178 RVA: 0x00192CD4 File Offset: 0x00190ED4
		private void AdjustNonScrollingViewportForItems(IHierarchicalVirtualizationAndScrollInfo virtualizationInfoProvider, ref Rect viewport, ref VirtualizationCacheLength cacheLength, ref VirtualizationCacheLengthUnit cacheLengthUnit)
		{
			bool forHeader = false;
			this.AdjustNonScrollingViewport(virtualizationInfoProvider, ref viewport, ref cacheLength, ref cacheLengthUnit, forHeader);
		}

		// Token: 0x06005A8B RID: 23179 RVA: 0x00192CF0 File Offset: 0x00190EF0
		private void AdjustNonScrollingViewport(IHierarchicalVirtualizationAndScrollInfo virtualizationInfoProvider, ref Rect viewport, ref VirtualizationCacheLength cacheLength, ref VirtualizationCacheLengthUnit cacheUnit, bool forHeader)
		{
			Rect rect = viewport;
			double num = cacheLength.CacheBeforeViewport;
			double num2 = cacheLength.CacheAfterViewport;
			HierarchicalVirtualizationHeaderDesiredSizes headerDesiredSizes = virtualizationInfoProvider.HeaderDesiredSizes;
			HierarchicalVirtualizationItemDesiredSizes itemDesiredSizes = virtualizationInfoProvider.ItemDesiredSizes;
			Size size = forHeader ? headerDesiredSizes.PixelSize : itemDesiredSizes.PixelSize;
			Size size2 = forHeader ? headerDesiredSizes.LogicalSize : itemDesiredSizes.LogicalSize;
			RelativeHeaderPosition relativeHeaderPosition = RelativeHeaderPosition.Top;
			if ((forHeader && relativeHeaderPosition == RelativeHeaderPosition.Left) || (!forHeader && relativeHeaderPosition == RelativeHeaderPosition.Right))
			{
				viewport.X -= (this.IsPixelBased ? size.Width : size2.Width);
				if (DoubleUtil.GreaterThan(rect.X, 0.0))
				{
					if (this.IsPixelBased && DoubleUtil.GreaterThan(size.Width, rect.X))
					{
						double num3 = size.Width - rect.X;
						double num4 = size.Width - num3;
						viewport.Width = Math.Max(viewport.Width - num3, 0.0);
						if (cacheUnit == VirtualizationCacheLengthUnit.Pixel)
						{
							num = Math.Max(num - num4, 0.0);
						}
						else
						{
							num = Math.Max(num - Math.Floor(size2.Width * num4 / size.Width), 0.0);
						}
					}
				}
				else if (DoubleUtil.GreaterThan(rect.Width, 0.0))
				{
					if (DoubleUtil.GreaterThanOrClose(rect.Width, size.Width))
					{
						viewport.Width = Math.Max(0.0, rect.Width - size.Width);
					}
					else
					{
						double num5 = rect.Width;
						double num6 = size.Width - num5;
						viewport.Width = 0.0;
						if (cacheUnit == VirtualizationCacheLengthUnit.Pixel)
						{
							num2 = Math.Max(num2 - num6, 0.0);
						}
						else
						{
							num2 = Math.Max(num2 - Math.Floor(size2.Width * num6 / size.Width), 0.0);
						}
					}
				}
				else if (cacheUnit == VirtualizationCacheLengthUnit.Pixel)
				{
					num2 = Math.Max(num2 - size.Width, 0.0);
				}
				else
				{
					num2 = Math.Max(num2 - size2.Width, 0.0);
				}
			}
			else if ((forHeader && relativeHeaderPosition == RelativeHeaderPosition.Top) || (!forHeader && relativeHeaderPosition == RelativeHeaderPosition.Bottom))
			{
				viewport.Y -= (this.IsPixelBased ? size.Height : size2.Height);
				if (DoubleUtil.GreaterThan(rect.Y, 0.0))
				{
					if (this.IsPixelBased && DoubleUtil.GreaterThan(size.Height, rect.Y))
					{
						double num3 = size.Height - rect.Y;
						double num4 = size.Height - num3;
						viewport.Height = Math.Max(viewport.Height - num3, 0.0);
						if (cacheUnit == VirtualizationCacheLengthUnit.Pixel)
						{
							num = Math.Max(num - num4, 0.0);
						}
						else
						{
							num = Math.Max(num - Math.Floor(size2.Height * num4 / size.Height), 0.0);
						}
					}
				}
				else if (DoubleUtil.GreaterThan(rect.Height, 0.0))
				{
					if (DoubleUtil.GreaterThanOrClose(rect.Height, size.Height))
					{
						viewport.Height = Math.Max(0.0, rect.Height - size.Height);
					}
					else
					{
						double num5 = rect.Height;
						double num6 = size.Height - num5;
						viewport.Height = 0.0;
						if (cacheUnit == VirtualizationCacheLengthUnit.Pixel)
						{
							num2 = Math.Max(num2 - num6, 0.0);
						}
						else
						{
							num2 = Math.Max(num2 - Math.Floor(size2.Height * num6 / size.Height), 0.0);
						}
					}
				}
				else if (cacheUnit == VirtualizationCacheLengthUnit.Pixel)
				{
					num2 = Math.Max(num2 - size.Height, 0.0);
				}
				else
				{
					num2 = Math.Max(num2 - size2.Height, 0.0);
				}
			}
			cacheLength = new VirtualizationCacheLength(num, num2);
		}

		// Token: 0x06005A8C RID: 23180 RVA: 0x00193188 File Offset: 0x00191388
		private void AdjustNonScrollingViewportForInset(bool isHorizontal, object parentItem, IContainItemStorage parentItemStorageProvider, IHierarchicalVirtualizationAndScrollInfo virtualizationInfoProvider, ref Rect viewport, ref VirtualizationCacheLength cacheLength, ref VirtualizationCacheLengthUnit cacheUnit)
		{
			Rect rect = viewport;
			FrameworkElement container = virtualizationInfoProvider as FrameworkElement;
			Thickness itemsHostInsetForChild = this.GetItemsHostInsetForChild(virtualizationInfoProvider, parentItemStorageProvider, parentItem);
			bool flag = this.IsHeaderBeforeItems(isHorizontal, container, ref itemsHostInsetForChild);
			double num = cacheLength.CacheBeforeViewport;
			double num2 = cacheLength.CacheAfterViewport;
			if (isHorizontal)
			{
				viewport.X -= (this.IsPixelBased ? itemsHostInsetForChild.Left : ((double)(flag ? 1 : 0)));
			}
			else
			{
				viewport.Y -= (this.IsPixelBased ? itemsHostInsetForChild.Top : ((double)(flag ? 1 : 0)));
			}
			if (isHorizontal)
			{
				if (DoubleUtil.GreaterThan(rect.X, 0.0))
				{
					if (DoubleUtil.GreaterThan(viewport.Width, 0.0))
					{
						if (this.IsPixelBased && DoubleUtil.GreaterThan(0.0, viewport.X))
						{
							if (cacheUnit == VirtualizationCacheLengthUnit.Pixel)
							{
								num = Math.Max(0.0, num - rect.X);
							}
							viewport.Width = Math.Max(0.0, viewport.Width + viewport.X);
						}
					}
					else if (cacheUnit == VirtualizationCacheLengthUnit.Pixel)
					{
						num = Math.Max(0.0, num - itemsHostInsetForChild.Right);
					}
					else if (!flag)
					{
						num = Math.Max(0.0, num - 1.0);
					}
				}
				else if (DoubleUtil.GreaterThan(viewport.Width, 0.0))
				{
					if (DoubleUtil.GreaterThanOrClose(viewport.Width, itemsHostInsetForChild.Left))
					{
						viewport.Width = Math.Max(0.0, viewport.Width - itemsHostInsetForChild.Left);
					}
					else
					{
						if (cacheUnit == VirtualizationCacheLengthUnit.Pixel)
						{
							num2 = Math.Max(0.0, num2 - (itemsHostInsetForChild.Left - viewport.Width));
						}
						viewport.Width = 0.0;
					}
				}
				else if (cacheUnit == VirtualizationCacheLengthUnit.Pixel)
				{
					num2 = Math.Max(0.0, num2 - itemsHostInsetForChild.Left);
				}
				else if (flag)
				{
					num2 = Math.Max(0.0, num2 - 1.0);
				}
			}
			else if (DoubleUtil.GreaterThan(rect.Y, 0.0))
			{
				if (DoubleUtil.GreaterThan(viewport.Height, 0.0))
				{
					if (this.IsPixelBased && DoubleUtil.GreaterThan(0.0, viewport.Y))
					{
						if (cacheUnit == VirtualizationCacheLengthUnit.Pixel)
						{
							num = Math.Max(0.0, num - rect.Y);
						}
						viewport.Height = Math.Max(0.0, viewport.Height + viewport.Y);
					}
				}
				else if (cacheUnit == VirtualizationCacheLengthUnit.Pixel)
				{
					num = Math.Max(0.0, num - itemsHostInsetForChild.Bottom);
				}
				else if (!flag)
				{
					num = Math.Max(0.0, num - 1.0);
				}
			}
			else if (DoubleUtil.GreaterThan(viewport.Height, 0.0))
			{
				if (DoubleUtil.GreaterThanOrClose(viewport.Height, itemsHostInsetForChild.Top))
				{
					viewport.Height = Math.Max(0.0, viewport.Height - itemsHostInsetForChild.Top);
				}
				else
				{
					if (cacheUnit == VirtualizationCacheLengthUnit.Pixel)
					{
						num2 = Math.Max(0.0, num2 - (itemsHostInsetForChild.Top - viewport.Height));
					}
					viewport.Height = 0.0;
				}
			}
			else if (cacheUnit == VirtualizationCacheLengthUnit.Pixel)
			{
				num2 = Math.Max(0.0, num2 - itemsHostInsetForChild.Top);
			}
			else if (flag)
			{
				num2 = Math.Max(0.0, num2 - 1.0);
			}
			cacheLength = new VirtualizationCacheLength(num, num2);
		}

		// Token: 0x06005A8D RID: 23181 RVA: 0x001935B0 File Offset: 0x001917B0
		private void ComputeFirstItemInViewportIndexAndOffset(IList items, int itemCount, IContainItemStorage itemStorageProvider, Rect viewport, VirtualizationCacheLength cacheSize, bool isHorizontal, bool areContainersUniformlySized, double uniformOrAverageContainerSize, out double firstItemInViewportOffset, out double firstItemInViewportContainerSpan, out int firstItemInViewportIndex, out bool foundFirstItemInViewport)
		{
			firstItemInViewportOffset = 0.0;
			firstItemInViewportContainerSpan = 0.0;
			firstItemInViewportIndex = 0;
			foundFirstItemInViewport = false;
			if (this.IsViewportEmpty(isHorizontal, viewport))
			{
				if (DoubleUtil.GreaterThan(cacheSize.CacheBeforeViewport, 0.0))
				{
					firstItemInViewportIndex = itemCount - 1;
					this.ComputeDistance(items, itemStorageProvider, isHorizontal, areContainersUniformlySized, uniformOrAverageContainerSize, 0, itemCount - 1, out firstItemInViewportOffset);
					foundFirstItemInViewport = true;
				}
				else
				{
					firstItemInViewportIndex = 0;
					firstItemInViewportOffset = 0.0;
					foundFirstItemInViewport = DoubleUtil.GreaterThan(cacheSize.CacheAfterViewport, 0.0);
				}
			}
			else
			{
				double num = Math.Max(isHorizontal ? viewport.X : viewport.Y, 0.0);
				if (areContainersUniformlySized)
				{
					if (DoubleUtil.GreaterThan(uniformOrAverageContainerSize, 0.0))
					{
						firstItemInViewportIndex = (int)Math.Floor(num / uniformOrAverageContainerSize);
						firstItemInViewportOffset = (double)firstItemInViewportIndex * uniformOrAverageContainerSize;
					}
					firstItemInViewportContainerSpan = uniformOrAverageContainerSize;
					foundFirstItemInViewport = (firstItemInViewportIndex < itemCount);
					if (!foundFirstItemInViewport)
					{
						firstItemInViewportOffset = 0.0;
						firstItemInViewportIndex = 0;
					}
				}
				else if (DoubleUtil.AreClose(num, 0.0))
				{
					foundFirstItemInViewport = true;
					firstItemInViewportOffset = 0.0;
					firstItemInViewportIndex = 0;
				}
				else
				{
					double num2 = 0.0;
					bool isVSP45Compat = VirtualizingStackPanel.IsVSP45Compat;
					for (int i = 0; i < itemCount; i++)
					{
						object item = items[i];
						Size size;
						this.GetContainerSizeForItem(itemStorageProvider, item, isHorizontal, areContainersUniformlySized, uniformOrAverageContainerSize, out size);
						double num3 = isHorizontal ? size.Width : size.Height;
						num2 += num3;
						bool flag = isVSP45Compat ? DoubleUtil.GreaterThan(num2, num) : LayoutDoubleUtil.LessThan(num, num2);
						if (flag)
						{
							firstItemInViewportIndex = i;
							firstItemInViewportOffset = num2 - num3;
							firstItemInViewportContainerSpan = num3;
							break;
						}
					}
					foundFirstItemInViewport = (isVSP45Compat ? DoubleUtil.GreaterThan(num2, num) : LayoutDoubleUtil.LessThan(num, num2));
					if (!foundFirstItemInViewport)
					{
						firstItemInViewportOffset = 0.0;
						firstItemInViewportIndex = 0;
					}
				}
			}
			if (VirtualizingStackPanel.ScrollTracer.IsEnabled && VirtualizingStackPanel.ScrollTracer.IsTracing(this))
			{
				VirtualizingStackPanel.ScrollTracer.Trace(this, VirtualizingStackPanel.ScrollTraceOp.CFIVIO, new object[]
				{
					viewport,
					foundFirstItemInViewport,
					firstItemInViewportIndex,
					firstItemInViewportOffset
				});
			}
		}

		// Token: 0x06005A8E RID: 23182 RVA: 0x001937F8 File Offset: 0x001919F8
		private double ComputeEffectiveOffset(ref Rect viewport, DependencyObject firstContainer, int itemIndex, double firstItemOffset, IList items, IContainItemStorage itemStorageProvider, IHierarchicalVirtualizationAndScrollInfo virtualizationInfoProvider, bool isHorizontal, bool areContainersUniformlySized, double uniformOrAverageContainerSize, long scrollGeneration)
		{
			if (firstContainer == null || this.IsViewportEmpty(isHorizontal, viewport))
			{
				return -1.0;
			}
			double num = isHorizontal ? viewport.X : viewport.Y;
			double num2;
			this.ComputeDistance(items, itemStorageProvider, isHorizontal, areContainersUniformlySized, uniformOrAverageContainerSize, 0, itemIndex, out num2);
			num2 += num - firstItemOffset;
			VirtualizingStackPanel.EffectiveOffsetInformation effectiveOffsetInformation = VirtualizingStackPanel.EffectiveOffsetInformationField.GetValue(firstContainer);
			List<double> list = (effectiveOffsetInformation != null) ? effectiveOffsetInformation.OffsetList : null;
			if (list != null)
			{
				int count = list.Count;
				num2 += list[count - 1] - list[0];
			}
			DependencyObject dependencyObject = virtualizationInfoProvider as DependencyObject;
			if (dependencyObject != null && !LayoutDoubleUtil.AreClose(num, num2))
			{
				effectiveOffsetInformation = VirtualizingStackPanel.EffectiveOffsetInformationField.GetValue(dependencyObject);
				if (effectiveOffsetInformation == null || effectiveOffsetInformation.ScrollGeneration != scrollGeneration)
				{
					effectiveOffsetInformation = new VirtualizingStackPanel.EffectiveOffsetInformation(scrollGeneration);
					effectiveOffsetInformation.OffsetList.Add(num);
				}
				effectiveOffsetInformation.OffsetList.Add(num2);
				if (VirtualizingStackPanel.ScrollTracer.IsEnabled && VirtualizingStackPanel.ScrollTracer.IsTracing(this))
				{
					List<double> offsetList = effectiveOffsetInformation.OffsetList;
					object[] array = new object[offsetList.Count + 2];
					array[0] = scrollGeneration;
					array[1] = ":";
					for (int i = 0; i < offsetList.Count; i++)
					{
						array[i + 2] = offsetList[i];
					}
					VirtualizingStackPanel.ScrollTracer.Trace(this, VirtualizingStackPanel.ScrollTraceOp.StoreSubstOffset, array);
				}
				VirtualizingStackPanel.EffectiveOffsetInformationField.SetValue(dependencyObject, effectiveOffsetInformation);
			}
			return num2;
		}

		// Token: 0x06005A8F RID: 23183 RVA: 0x0019395D File Offset: 0x00191B5D
		private void IncrementScrollGeneration()
		{
			if (!FrameworkAppContextSwitches.OptOutOfEffectiveOffsetHangFix)
			{
				this._scrollData._scrollGeneration += 1L;
			}
		}

		// Token: 0x06005A90 RID: 23184 RVA: 0x0019397C File Offset: 0x00191B7C
		private void ExtendPixelAndLogicalSizes(IList children, IList items, int itemCount, IContainItemStorage itemStorageProvider, bool areContainersUniformlySized, double uniformOrAverageContainerSize, double uniformOrAverageContainerPixelSize, ref Size stackPixelSize, ref Size stackLogicalSize, bool isHorizontal, int pivotIndex, int pivotChildIndex, int firstContainerInViewportIndex, bool before)
		{
			bool isVSP45Compat = VirtualizingStackPanel.IsVSP45Compat;
			double num = 0.0;
			double num2;
			if (before)
			{
				if (isVSP45Compat)
				{
					this.ComputeDistance(items, itemStorageProvider, isHorizontal, areContainersUniformlySized, uniformOrAverageContainerSize, 0, pivotIndex, out num2);
				}
				else
				{
					this.ComputeDistance(items, itemStorageProvider, isHorizontal, areContainersUniformlySized, uniformOrAverageContainerSize, uniformOrAverageContainerPixelSize, 0, pivotIndex, out num2, out num);
					if (!this.IsPixelBased)
					{
						double num3;
						double num4;
						this.ComputeDistance(items, itemStorageProvider, isHorizontal, areContainersUniformlySized, uniformOrAverageContainerSize, uniformOrAverageContainerPixelSize, pivotIndex, firstContainerInViewportIndex - pivotIndex, out num3, out num4);
						this._pixelDistanceToViewport = num + num4;
						this._pixelDistanceToFirstContainerInExtendedViewport = num;
					}
				}
			}
			else if (isVSP45Compat)
			{
				this.ComputeDistance(items, itemStorageProvider, isHorizontal, areContainersUniformlySized, uniformOrAverageContainerSize, pivotIndex, itemCount - pivotIndex, out num2);
			}
			else
			{
				this.ComputeDistance(items, itemStorageProvider, isHorizontal, areContainersUniformlySized, uniformOrAverageContainerSize, uniformOrAverageContainerPixelSize, pivotIndex, itemCount - pivotIndex, out num2, out num);
			}
			if (!this.IsPixelBased)
			{
				if (isHorizontal)
				{
					stackLogicalSize.Width += num2;
				}
				else
				{
					stackLogicalSize.Height += num2;
				}
				if (isVSP45Compat)
				{
					if (!this.IsScrolling)
					{
						int num5;
						int num6;
						if (before)
						{
							num5 = 0;
							num6 = pivotChildIndex;
						}
						else
						{
							num5 = pivotChildIndex;
							num6 = children.Count;
						}
						for (int i = num5; i < num6; i++)
						{
							Size desiredSize = ((UIElement)children[i]).DesiredSize;
							if (isHorizontal)
							{
								stackPixelSize.Width += desiredSize.Width;
							}
							else
							{
								stackPixelSize.Height += desiredSize.Height;
							}
						}
						return;
					}
				}
				else if (!this.IsScrolling)
				{
					if (isHorizontal)
					{
						stackPixelSize.Width += num;
						return;
					}
					stackPixelSize.Height += num;
				}
				return;
			}
			if (isHorizontal)
			{
				stackPixelSize.Width += num2;
				return;
			}
			stackPixelSize.Height += num2;
		}

		// Token: 0x06005A91 RID: 23185 RVA: 0x00193B38 File Offset: 0x00191D38
		private void ComputeDistance(IList items, IContainItemStorage itemStorageProvider, bool isHorizontal, bool areContainersUniformlySized, double uniformOrAverageContainerSize, int startIndex, int itemCount, out double distance)
		{
			if (!this.IsPixelBased && !VirtualizingStackPanel.IsVSP45Compat)
			{
				double num;
				this.ComputeDistance(items, itemStorageProvider, isHorizontal, areContainersUniformlySized, uniformOrAverageContainerSize, 1.0, startIndex, itemCount, out distance, out num);
				return;
			}
			distance = 0.0;
			if (!areContainersUniformlySized)
			{
				for (int i = startIndex; i < startIndex + itemCount; i++)
				{
					object item = items[i];
					Size size;
					this.GetContainerSizeForItem(itemStorageProvider, item, isHorizontal, areContainersUniformlySized, uniformOrAverageContainerSize, out size);
					if (isHorizontal)
					{
						distance += size.Width;
					}
					else
					{
						distance += size.Height;
					}
				}
				return;
			}
			if (isHorizontal)
			{
				distance += uniformOrAverageContainerSize * (double)itemCount;
				return;
			}
			distance += uniformOrAverageContainerSize * (double)itemCount;
		}

		// Token: 0x06005A92 RID: 23186 RVA: 0x00193BEC File Offset: 0x00191DEC
		private void ComputeDistance(IList items, IContainItemStorage itemStorageProvider, bool isHorizontal, bool areContainersUniformlySized, double uniformOrAverageContainerSize, double uniformOrAverageContainerPixelSize, int startIndex, int itemCount, out double distance, out double pixelDistance)
		{
			distance = 0.0;
			pixelDistance = 0.0;
			if (areContainersUniformlySized)
			{
				distance += uniformOrAverageContainerSize * (double)itemCount;
				pixelDistance += uniformOrAverageContainerPixelSize * (double)itemCount;
				return;
			}
			for (int i = startIndex; i < startIndex + itemCount; i++)
			{
				object item = items[i];
				Size size;
				Size size2;
				this.GetContainerSizeForItem(itemStorageProvider, item, isHorizontal, areContainersUniformlySized, uniformOrAverageContainerSize, uniformOrAverageContainerPixelSize, out size, out size2);
				if (isHorizontal)
				{
					distance += size.Width;
					pixelDistance += size2.Width;
				}
				else
				{
					distance += size.Height;
					pixelDistance += size2.Height;
				}
			}
		}

		// Token: 0x06005A93 RID: 23187 RVA: 0x00193C9C File Offset: 0x00191E9C
		private void GetContainerSizeForItem(IContainItemStorage itemStorageProvider, object item, bool isHorizontal, bool areContainersUniformlySized, double uniformOrAverageContainerSize, out Size containerSize)
		{
			if (!VirtualizingStackPanel.IsVSP45Compat)
			{
				Size size;
				this.GetContainerSizeForItem(itemStorageProvider, item, isHorizontal, areContainersUniformlySized, uniformOrAverageContainerSize, 1.0, out containerSize, out size);
				return;
			}
			containerSize = Size.Empty;
			if (areContainersUniformlySized)
			{
				containerSize = default(Size);
				if (isHorizontal)
				{
					containerSize.Width = uniformOrAverageContainerSize;
					containerSize.Height = (this.IsPixelBased ? base.DesiredSize.Height : 1.0);
					return;
				}
				containerSize.Height = uniformOrAverageContainerSize;
				containerSize.Width = (this.IsPixelBased ? base.DesiredSize.Width : 1.0);
				return;
			}
			else
			{
				object obj = itemStorageProvider.ReadItemValue(item, VirtualizingStackPanel.ContainerSizeProperty);
				if (obj != null)
				{
					containerSize = (Size)obj;
					return;
				}
				containerSize = default(Size);
				if (isHorizontal)
				{
					containerSize.Width = uniformOrAverageContainerSize;
					containerSize.Height = (this.IsPixelBased ? base.DesiredSize.Height : 1.0);
					return;
				}
				containerSize.Height = uniformOrAverageContainerSize;
				containerSize.Width = (this.IsPixelBased ? base.DesiredSize.Width : 1.0);
				return;
			}
		}

		// Token: 0x06005A94 RID: 23188 RVA: 0x00193DDC File Offset: 0x00191FDC
		private void GetContainerSizeForItem(IContainItemStorage itemStorageProvider, object item, bool isHorizontal, bool areContainersUniformlySized, double uniformOrAverageContainerSize, double uniformOrAverageContainerPixelSize, out Size containerSize, out Size containerPixelSize)
		{
			containerSize = default(Size);
			containerPixelSize = default(Size);
			bool flag = areContainersUniformlySized;
			if (!areContainersUniformlySized)
			{
				if (this.IsPixelBased)
				{
					object obj = itemStorageProvider.ReadItemValue(item, VirtualizingStackPanel.ContainerSizeProperty);
					if (obj != null)
					{
						containerSize = (Size)obj;
						containerPixelSize = containerSize;
					}
					else
					{
						flag = true;
					}
				}
				else
				{
					object obj2 = itemStorageProvider.ReadItemValue(item, VirtualizingStackPanel.ContainerSizeDualProperty);
					if (obj2 != null)
					{
						VirtualizingStackPanel.ContainerSizeDual containerSizeDual = (VirtualizingStackPanel.ContainerSizeDual)obj2;
						containerSize = containerSizeDual.ItemSize;
						containerPixelSize = containerSizeDual.PixelSize;
					}
					else
					{
						flag = true;
					}
				}
			}
			if (flag)
			{
				if (isHorizontal)
				{
					double height = base.DesiredSize.Height;
					containerSize.Width = uniformOrAverageContainerSize;
					containerSize.Height = (this.IsPixelBased ? height : 1.0);
					containerPixelSize.Width = uniformOrAverageContainerPixelSize;
					containerPixelSize.Height = height;
					return;
				}
				double width = base.DesiredSize.Width;
				containerSize.Height = uniformOrAverageContainerSize;
				containerSize.Width = (this.IsPixelBased ? width : 1.0);
				containerPixelSize.Height = uniformOrAverageContainerPixelSize;
				containerPixelSize.Width = width;
			}
		}

		// Token: 0x06005A95 RID: 23189 RVA: 0x00193F08 File Offset: 0x00192108
		private void SetContainerSizeForItem(IContainItemStorage itemStorageProvider, IContainItemStorage parentItemStorageProvider, object parentItem, object item, Size containerSize, bool isHorizontal, ref bool hasUniformOrAverageContainerSizeBeenSet, ref double uniformOrAverageContainerSize, ref bool areContainersUniformlySized)
		{
			if (!hasUniformOrAverageContainerSizeBeenSet)
			{
				if (VirtualizingStackPanel.IsVSP45Compat)
				{
					parentItemStorageProvider = itemStorageProvider;
				}
				hasUniformOrAverageContainerSizeBeenSet = true;
				uniformOrAverageContainerSize = (isHorizontal ? containerSize.Width : containerSize.Height);
				this.SetUniformOrAverageContainerSize(parentItemStorageProvider, parentItem, uniformOrAverageContainerSize, 1.0);
			}
			else if (areContainersUniformlySized)
			{
				if (isHorizontal)
				{
					areContainersUniformlySized = DoubleUtil.AreClose(containerSize.Width, uniformOrAverageContainerSize);
				}
				else
				{
					areContainersUniformlySized = DoubleUtil.AreClose(containerSize.Height, uniformOrAverageContainerSize);
				}
			}
			if (!areContainersUniformlySized)
			{
				itemStorageProvider.StoreItemValue(item, VirtualizingStackPanel.ContainerSizeProperty, containerSize);
			}
		}

		// Token: 0x06005A96 RID: 23190 RVA: 0x00193FA0 File Offset: 0x001921A0
		private void SetContainerSizeForItem(IContainItemStorage itemStorageProvider, IContainItemStorage parentItemStorageProvider, object parentItem, object item, Size containerSize, Size containerPixelSize, bool isHorizontal, bool hasVirtualizingChildren, ref bool hasUniformOrAverageContainerSizeBeenSet, ref double uniformOrAverageContainerSize, ref double uniformOrAverageContainerPixelSize, ref bool areContainersUniformlySized, ref bool hasAnyContainerSpanChanged)
		{
			if (!hasUniformOrAverageContainerSizeBeenSet)
			{
				hasUniformOrAverageContainerSizeBeenSet = true;
				uniformOrAverageContainerSize = (isHorizontal ? containerSize.Width : containerSize.Height);
				uniformOrAverageContainerPixelSize = (isHorizontal ? containerPixelSize.Width : containerPixelSize.Height);
				this.SetUniformOrAverageContainerSize(parentItemStorageProvider, parentItem, uniformOrAverageContainerSize, uniformOrAverageContainerPixelSize);
			}
			else if (areContainersUniformlySized)
			{
				bool flag = this.IsPixelBased || (this.IsScrolling && !hasVirtualizingChildren);
				if (isHorizontal)
				{
					areContainersUniformlySized = (DoubleUtil.AreClose(containerSize.Width, uniformOrAverageContainerSize) && (flag || DoubleUtil.AreClose(containerPixelSize.Width, uniformOrAverageContainerPixelSize)));
				}
				else
				{
					areContainersUniformlySized = (DoubleUtil.AreClose(containerSize.Height, uniformOrAverageContainerSize) && (flag || DoubleUtil.AreClose(containerPixelSize.Height, uniformOrAverageContainerPixelSize)));
				}
			}
			if (!areContainersUniformlySized)
			{
				double value = 0.0;
				double value2 = 0.0;
				bool flag2 = VirtualizingStackPanel.ScrollTracer.IsEnabled && VirtualizingStackPanel.ScrollTracer.IsTracing(this);
				if (this.IsPixelBased)
				{
					object obj = itemStorageProvider.ReadItemValue(item, VirtualizingStackPanel.ContainerSizeProperty);
					Size size = (obj != null) ? ((Size)obj) : Size.Empty;
					if (obj == null || containerSize != size)
					{
						if (flag2)
						{
							ItemContainerGenerator itemContainerGenerator = (ItemContainerGenerator)base.Generator;
							VirtualizingStackPanel.ScrollTracer.Trace(this, VirtualizingStackPanel.ScrollTraceOp.SetContainerSize, new object[]
							{
								itemContainerGenerator.IndexFromContainer(itemContainerGenerator.ContainerFromItem(item)),
								size,
								containerSize
							});
						}
						if (isHorizontal)
						{
							value = ((obj != null) ? size.Width : uniformOrAverageContainerSize);
							value2 = containerSize.Width;
						}
						else
						{
							value = ((obj != null) ? size.Height : uniformOrAverageContainerSize);
							value2 = containerSize.Height;
						}
					}
					itemStorageProvider.StoreItemValue(item, VirtualizingStackPanel.ContainerSizeProperty, containerSize);
				}
				else
				{
					object obj2 = itemStorageProvider.ReadItemValue(item, VirtualizingStackPanel.ContainerSizeDualProperty);
					VirtualizingStackPanel.ContainerSizeDual containerSizeDual = (obj2 != null) ? ((VirtualizingStackPanel.ContainerSizeDual)obj2) : new VirtualizingStackPanel.ContainerSizeDual(Size.Empty, Size.Empty);
					if (obj2 == null || containerSize != containerSizeDual.ItemSize || containerPixelSize != containerSizeDual.PixelSize)
					{
						if (flag2)
						{
							ItemContainerGenerator itemContainerGenerator2 = (ItemContainerGenerator)base.Generator;
							VirtualizingStackPanel.ScrollTracer.Trace(this, VirtualizingStackPanel.ScrollTraceOp.SetContainerSize, new object[]
							{
								itemContainerGenerator2.IndexFromContainer(itemContainerGenerator2.ContainerFromItem(item)),
								containerSizeDual.ItemSize,
								containerSize,
								containerSizeDual.PixelSize,
								containerPixelSize
							});
						}
						if (isHorizontal)
						{
							value = ((obj2 != null) ? containerSizeDual.ItemSize.Width : uniformOrAverageContainerSize);
							value2 = containerSize.Width;
						}
						else
						{
							value = ((obj2 != null) ? containerSizeDual.ItemSize.Height : uniformOrAverageContainerSize);
							value2 = containerSize.Height;
						}
					}
					VirtualizingStackPanel.ContainerSizeDual value3 = new VirtualizingStackPanel.ContainerSizeDual(containerPixelSize, containerSize);
					itemStorageProvider.StoreItemValue(item, VirtualizingStackPanel.ContainerSizeDualProperty, value3);
				}
				if (!LayoutDoubleUtil.AreClose(value, value2))
				{
					hasAnyContainerSpanChanged = true;
				}
			}
		}

		// Token: 0x06005A97 RID: 23191 RVA: 0x001942AC File Offset: 0x001924AC
		private Thickness GetItemsHostInsetForChild(IHierarchicalVirtualizationAndScrollInfo virtualizationInfoProvider, IContainItemStorage parentItemStorageProvider = null, object parentItem = null)
		{
			FrameworkElement frameworkElement = virtualizationInfoProvider as FrameworkElement;
			if (parentItemStorageProvider == null)
			{
				return (Thickness)frameworkElement.GetValue(VirtualizingStackPanel.ItemsHostInsetProperty);
			}
			Thickness thickness = default(Thickness);
			object obj = parentItemStorageProvider.ReadItemValue(parentItem, VirtualizingStackPanel.ItemsHostInsetProperty);
			if (obj != null)
			{
				thickness = (Thickness)obj;
			}
			else if ((obj = frameworkElement.ReadLocalValue(VirtualizingStackPanel.ItemsHostInsetProperty)) != DependencyProperty.UnsetValue)
			{
				thickness = (Thickness)obj;
			}
			else
			{
				HierarchicalVirtualizationHeaderDesiredSizes headerDesiredSizes = virtualizationInfoProvider.HeaderDesiredSizes;
				Thickness margin = frameworkElement.Margin;
				thickness.Top = headerDesiredSizes.PixelSize.Height + margin.Top;
				thickness.Left = headerDesiredSizes.PixelSize.Width + margin.Left;
				parentItemStorageProvider.StoreItemValue(parentItem, VirtualizingStackPanel.ItemsHostInsetProperty, thickness);
			}
			frameworkElement.SetValue(VirtualizingStackPanel.ItemsHostInsetProperty, thickness);
			return thickness;
		}

		// Token: 0x06005A98 RID: 23192 RVA: 0x00194384 File Offset: 0x00192584
		private void SetItemsHostInsetForChild(int index, UIElement child, IContainItemStorage itemStorageProvider, bool isHorizontal)
		{
			bool flag = isHorizontal;
			IHierarchicalVirtualizationAndScrollInfo virtualizingChild = VirtualizingStackPanel.GetVirtualizingChild(child, ref flag);
			Panel panel = (virtualizingChild == null) ? null : virtualizingChild.ItemsHost;
			if (panel == null || !panel.IsVisible)
			{
				return;
			}
			GeneralTransform generalTransform = child.TransformToDescendant(panel);
			if (generalTransform == null)
			{
				return;
			}
			FrameworkElement frameworkElement = virtualizingChild as FrameworkElement;
			Thickness thickness = (frameworkElement == null) ? default(Thickness) : frameworkElement.Margin;
			Rect rect = new Rect(default(Point), child.DesiredSize);
			rect.Offset(-thickness.Left, -thickness.Top);
			Rect rect2 = generalTransform.TransformBounds(rect);
			Size desiredSize = panel.DesiredSize;
			double left = DoubleUtil.AreClose(0.0, rect2.Left) ? 0.0 : (-rect2.Left);
			double top = DoubleUtil.AreClose(0.0, rect2.Top) ? 0.0 : (-rect2.Top);
			double right = DoubleUtil.AreClose(desiredSize.Width, rect2.Right) ? 0.0 : (rect2.Right - desiredSize.Width);
			double bottom = DoubleUtil.AreClose(desiredSize.Height, rect2.Bottom) ? 0.0 : (rect2.Bottom - desiredSize.Height);
			Thickness thickness2 = new Thickness(left, top, right, bottom);
			object itemFromContainer = this.GetItemFromContainer(child);
			if (itemFromContainer == DependencyProperty.UnsetValue)
			{
				return;
			}
			object obj = itemStorageProvider.ReadItemValue(itemFromContainer, VirtualizingStackPanel.ItemsHostInsetProperty);
			bool flag2 = obj == null;
			bool flag3 = flag2;
			if (!flag2)
			{
				Thickness thickness3 = (Thickness)obj;
				flag2 = (!DoubleUtil.AreClose(thickness3.Left, thickness2.Left) || !DoubleUtil.AreClose(thickness3.Top, thickness2.Top) || !DoubleUtil.AreClose(thickness3.Right, thickness2.Right) || !DoubleUtil.AreClose(thickness3.Bottom, thickness2.Bottom));
				flag3 = (flag2 && ((isHorizontal && (!VirtualizingStackPanel.AreInsetsClose(thickness3.Left, thickness2.Left) || !VirtualizingStackPanel.AreInsetsClose(thickness3.Right, thickness2.Right))) || (!isHorizontal && (!VirtualizingStackPanel.AreInsetsClose(thickness3.Top, thickness2.Top) || !VirtualizingStackPanel.AreInsetsClose(thickness3.Bottom, thickness2.Bottom)))));
			}
			if (flag2)
			{
				itemStorageProvider.StoreItemValue(itemFromContainer, VirtualizingStackPanel.ItemsHostInsetProperty, thickness2);
				child.SetValue(VirtualizingStackPanel.ItemsHostInsetProperty, thickness2);
			}
			if (flag3)
			{
				ItemsControl scrollingItemsControl = this.GetScrollingItemsControl(child);
				Panel panel2 = (scrollingItemsControl == null) ? null : scrollingItemsControl.ItemsHost;
				if (panel2 != null)
				{
					VirtualizingStackPanel virtualizingStackPanel = panel2 as VirtualizingStackPanel;
					if (virtualizingStackPanel != null)
					{
						virtualizingStackPanel.AnchoredInvalidateMeasure();
						return;
					}
					panel2.InvalidateMeasure();
				}
			}
		}

		// Token: 0x06005A99 RID: 23193 RVA: 0x0019465C File Offset: 0x0019285C
		private static bool AreInsetsClose(double value1, double value2)
		{
			if (value1 == value2)
			{
				return true;
			}
			double num = (Math.Abs(value1) + Math.Abs(value2)) * 0.001;
			double num2 = value1 - value2;
			return -num <= num2 && num >= num2;
		}

		// Token: 0x06005A9A RID: 23194 RVA: 0x0019469C File Offset: 0x0019289C
		private ItemsControl GetScrollingItemsControl(UIElement container)
		{
			if (container is TreeViewItem)
			{
				for (ItemsControl itemsControl = ItemsControl.ItemsControlFromItemContainer(container); itemsControl != null; itemsControl = ItemsControl.ItemsControlFromItemContainer(itemsControl))
				{
					TreeView treeView = itemsControl as TreeView;
					if (treeView != null)
					{
						return treeView;
					}
				}
			}
			else if (container is GroupItem)
			{
				DependencyObject dependencyObject = container;
				ItemsControl itemsControl2;
				for (;;)
				{
					dependencyObject = VisualTreeHelper.GetParent(dependencyObject);
					itemsControl2 = (dependencyObject as ItemsControl);
					if (itemsControl2 != null)
					{
						break;
					}
					if (dependencyObject == null)
					{
						goto Block_6;
					}
				}
				return itemsControl2;
				Block_6:;
			}
			else
			{
				string text = (container == null) ? "null" : container.GetType().Name;
			}
			return null;
		}

		// Token: 0x06005A9B RID: 23195 RVA: 0x0019470C File Offset: 0x0019290C
		private object GetItemFromContainer(DependencyObject container)
		{
			return container.ReadLocalValue(System.Windows.Controls.ItemContainerGenerator.ItemForItemContainerProperty);
		}

		// Token: 0x06005A9C RID: 23196 RVA: 0x0019471C File Offset: 0x0019291C
		private bool IsHeaderBeforeItems(bool isHorizontal, FrameworkElement container, ref Thickness inset)
		{
			Thickness thickness = (container == null) ? default(Thickness) : container.Margin;
			if (isHorizontal)
			{
				return DoubleUtil.GreaterThanOrClose(inset.Left - thickness.Left, inset.Right - thickness.Right);
			}
			return DoubleUtil.GreaterThanOrClose(inset.Top - thickness.Top, inset.Bottom - thickness.Bottom);
		}

		// Token: 0x06005A9D RID: 23197 RVA: 0x00194784 File Offset: 0x00192984
		private bool IsEndOfCache(bool isHorizontal, double cacheSize, VirtualizationCacheLengthUnit cacheUnit, Size stackPixelSizeInCache, Size stackLogicalSizeInCache)
		{
			if (!this.MeasureCaches)
			{
				return true;
			}
			if (cacheUnit == VirtualizationCacheLengthUnit.Item)
			{
				if (isHorizontal)
				{
					return DoubleUtil.GreaterThanOrClose(stackLogicalSizeInCache.Width, cacheSize);
				}
				return DoubleUtil.GreaterThanOrClose(stackLogicalSizeInCache.Height, cacheSize);
			}
			else
			{
				if (cacheUnit != VirtualizationCacheLengthUnit.Pixel)
				{
					return false;
				}
				if (isHorizontal)
				{
					return DoubleUtil.GreaterThanOrClose(stackPixelSizeInCache.Width, cacheSize);
				}
				return DoubleUtil.GreaterThanOrClose(stackPixelSizeInCache.Height, cacheSize);
			}
		}

		// Token: 0x06005A9E RID: 23198 RVA: 0x001947E1 File Offset: 0x001929E1
		private bool IsEndOfViewport(bool isHorizontal, Rect viewport, Size stackPixelSizeInViewport)
		{
			if (isHorizontal)
			{
				return DoubleUtil.GreaterThanOrClose(stackPixelSizeInViewport.Width, viewport.Width);
			}
			return DoubleUtil.GreaterThanOrClose(stackPixelSizeInViewport.Height, viewport.Height);
		}

		// Token: 0x06005A9F RID: 23199 RVA: 0x0019480D File Offset: 0x00192A0D
		private bool IsViewportEmpty(bool isHorizontal, Rect viewport)
		{
			if (isHorizontal)
			{
				return DoubleUtil.AreClose(viewport.Width, 0.0);
			}
			return DoubleUtil.AreClose(viewport.Height, 0.0);
		}

		// Token: 0x06005AA0 RID: 23200 RVA: 0x00194840 File Offset: 0x00192A40
		private void SetViewportForChild(bool isHorizontal, IContainItemStorage itemStorageProvider, bool areContainersUniformlySized, double uniformOrAverageContainerSize, bool mustDisableVirtualization, UIElement child, IHierarchicalVirtualizationAndScrollInfo virtualizingChild, object item, bool isBeforeFirstItem, bool isAfterFirstItem, double firstItemInViewportOffset, Rect parentViewport, VirtualizationCacheLength parentCacheSize, VirtualizationCacheLengthUnit parentCacheUnit, long scrollGeneration, Size stackPixelSize, Size stackPixelSizeInViewport, Size stackPixelSizeInCacheBeforeViewport, Size stackPixelSizeInCacheAfterViewport, Size stackLogicalSize, Size stackLogicalSizeInViewport, Size stackLogicalSizeInCacheBeforeViewport, Size stackLogicalSizeInCacheAfterViewport, out Rect childViewport, ref VirtualizationCacheLength childCacheSize, ref VirtualizationCacheLengthUnit childCacheUnit)
		{
			childViewport = parentViewport;
			if (isHorizontal)
			{
				if (isBeforeFirstItem)
				{
					Size size;
					this.GetContainerSizeForItem(itemStorageProvider, item, isHorizontal, areContainersUniformlySized, uniformOrAverageContainerSize, out size);
					childViewport.X = (this.IsPixelBased ? stackPixelSizeInCacheBeforeViewport.Width : stackLogicalSizeInCacheBeforeViewport.Width) + size.Width;
					childViewport.Width = 0.0;
				}
				else if (isAfterFirstItem)
				{
					childViewport.X = Math.Min(childViewport.X, 0.0) - (this.IsPixelBased ? (stackPixelSizeInViewport.Width + stackPixelSizeInCacheAfterViewport.Width) : (stackLogicalSizeInViewport.Width + stackLogicalSizeInCacheAfterViewport.Width));
					childViewport.Width = Math.Max(childViewport.Width - stackPixelSizeInViewport.Width, 0.0);
				}
				else
				{
					childViewport.X -= firstItemInViewportOffset;
					childViewport.Width = Math.Max(childViewport.Width - stackPixelSizeInViewport.Width, 0.0);
				}
				if (parentCacheUnit == VirtualizationCacheLengthUnit.Item)
				{
					childCacheSize = new VirtualizationCacheLength((isAfterFirstItem || DoubleUtil.LessThanOrClose(childViewport.X, 0.0)) ? 0.0 : Math.Max(parentCacheSize.CacheBeforeViewport - stackLogicalSizeInCacheBeforeViewport.Width, 0.0), isBeforeFirstItem ? 0.0 : Math.Max(parentCacheSize.CacheAfterViewport - stackLogicalSizeInCacheAfterViewport.Width, 0.0));
					childCacheUnit = VirtualizationCacheLengthUnit.Item;
				}
				else if (parentCacheUnit == VirtualizationCacheLengthUnit.Pixel)
				{
					childCacheSize = new VirtualizationCacheLength((isAfterFirstItem || DoubleUtil.LessThanOrClose(childViewport.X, 0.0)) ? 0.0 : Math.Max(parentCacheSize.CacheBeforeViewport - stackPixelSizeInCacheBeforeViewport.Width, 0.0), isBeforeFirstItem ? 0.0 : Math.Max(parentCacheSize.CacheAfterViewport - stackPixelSizeInCacheAfterViewport.Width, 0.0));
					childCacheUnit = VirtualizationCacheLengthUnit.Pixel;
				}
			}
			else
			{
				if (isBeforeFirstItem)
				{
					Size size2;
					this.GetContainerSizeForItem(itemStorageProvider, item, isHorizontal, areContainersUniformlySized, uniformOrAverageContainerSize, out size2);
					childViewport.Y = (this.IsPixelBased ? stackPixelSizeInCacheBeforeViewport.Height : stackLogicalSizeInCacheBeforeViewport.Height) + size2.Height;
					childViewport.Height = 0.0;
				}
				else if (isAfterFirstItem)
				{
					childViewport.Y = Math.Min(childViewport.Y, 0.0) - (this.IsPixelBased ? (stackPixelSizeInViewport.Height + stackPixelSizeInCacheAfterViewport.Height) : (stackLogicalSizeInViewport.Height + stackLogicalSizeInCacheAfterViewport.Height));
					childViewport.Height = Math.Max(childViewport.Height - stackPixelSizeInViewport.Height, 0.0);
				}
				else
				{
					childViewport.Y -= firstItemInViewportOffset;
					childViewport.Height = Math.Max(childViewport.Height - stackPixelSizeInViewport.Height, 0.0);
				}
				if (parentCacheUnit == VirtualizationCacheLengthUnit.Item)
				{
					childCacheSize = new VirtualizationCacheLength((isAfterFirstItem || DoubleUtil.LessThanOrClose(childViewport.Y, 0.0)) ? 0.0 : Math.Max(parentCacheSize.CacheBeforeViewport - stackLogicalSizeInCacheBeforeViewport.Height, 0.0), isBeforeFirstItem ? 0.0 : Math.Max(parentCacheSize.CacheAfterViewport - stackLogicalSizeInCacheAfterViewport.Height, 0.0));
					childCacheUnit = VirtualizationCacheLengthUnit.Item;
				}
				else if (parentCacheUnit == VirtualizationCacheLengthUnit.Pixel)
				{
					childCacheSize = new VirtualizationCacheLength((isAfterFirstItem || DoubleUtil.LessThanOrClose(childViewport.Y, 0.0)) ? 0.0 : Math.Max(parentCacheSize.CacheBeforeViewport - stackPixelSizeInCacheBeforeViewport.Height, 0.0), isBeforeFirstItem ? 0.0 : Math.Max(parentCacheSize.CacheAfterViewport - stackPixelSizeInCacheAfterViewport.Height, 0.0));
					childCacheUnit = VirtualizationCacheLengthUnit.Pixel;
				}
			}
			if (virtualizingChild != null)
			{
				virtualizingChild.Constraints = new HierarchicalVirtualizationConstraints(childCacheSize, childCacheUnit, childViewport)
				{
					ScrollGeneration = scrollGeneration
				};
				virtualizingChild.InBackgroundLayout = this.MeasureCaches;
				virtualizingChild.MustDisableVirtualization = mustDisableVirtualization;
			}
			if (child is IHierarchicalVirtualizationAndScrollInfo)
			{
				this.InvalidateMeasureOnItemsHost((IHierarchicalVirtualizationAndScrollInfo)child);
			}
		}

		// Token: 0x06005AA1 RID: 23201 RVA: 0x00194CC0 File Offset: 0x00192EC0
		private void InvalidateMeasureOnItemsHost(IHierarchicalVirtualizationAndScrollInfo virtualizingChild)
		{
			Panel itemsHost = virtualizingChild.ItemsHost;
			if (itemsHost != null)
			{
				Helper.InvalidateMeasureOnPath(itemsHost, this, true);
				if (!(itemsHost is VirtualizingStackPanel))
				{
					IList internalChildren = itemsHost.InternalChildren;
					for (int i = 0; i < internalChildren.Count; i++)
					{
						IHierarchicalVirtualizationAndScrollInfo hierarchicalVirtualizationAndScrollInfo = internalChildren[i] as IHierarchicalVirtualizationAndScrollInfo;
						if (hierarchicalVirtualizationAndScrollInfo != null)
						{
							this.InvalidateMeasureOnItemsHost(hierarchicalVirtualizationAndScrollInfo);
						}
					}
				}
			}
		}

		// Token: 0x06005AA2 RID: 23202 RVA: 0x00194D18 File Offset: 0x00192F18
		private void GetSizesForChild(bool isHorizontal, bool isChildHorizontal, bool isBeforeFirstItem, bool isAfterLastItem, IHierarchicalVirtualizationAndScrollInfo virtualizingChild, Size childDesiredSize, Rect childViewport, VirtualizationCacheLength childCacheSize, VirtualizationCacheLengthUnit childCacheUnit, out Size childPixelSize, out Size childPixelSizeInViewport, out Size childPixelSizeInCacheBeforeViewport, out Size childPixelSizeInCacheAfterViewport, out Size childLogicalSize, out Size childLogicalSizeInViewport, out Size childLogicalSizeInCacheBeforeViewport, out Size childLogicalSizeInCacheAfterViewport)
		{
			childPixelSize = default(Size);
			childPixelSizeInViewport = default(Size);
			childPixelSizeInCacheBeforeViewport = default(Size);
			childPixelSizeInCacheAfterViewport = default(Size);
			childLogicalSize = default(Size);
			childLogicalSizeInViewport = default(Size);
			childLogicalSizeInCacheBeforeViewport = default(Size);
			childLogicalSizeInCacheAfterViewport = default(Size);
			if (virtualizingChild != null)
			{
				RelativeHeaderPosition relativeHeaderPosition = RelativeHeaderPosition.Top;
				HierarchicalVirtualizationHeaderDesiredSizes headerDesiredSizes = virtualizingChild.HeaderDesiredSizes;
				HierarchicalVirtualizationItemDesiredSizes itemDesiredSizes = virtualizingChild.ItemDesiredSizes;
				Size pixelSize = headerDesiredSizes.PixelSize;
				Size logicalSize = headerDesiredSizes.LogicalSize;
				childPixelSize = childDesiredSize;
				if (relativeHeaderPosition == RelativeHeaderPosition.Top || relativeHeaderPosition == RelativeHeaderPosition.Bottom)
				{
					childLogicalSize.Height = itemDesiredSizes.LogicalSize.Height + logicalSize.Height;
					childLogicalSize.Width = Math.Max(itemDesiredSizes.LogicalSize.Width, logicalSize.Width);
				}
				else
				{
					childLogicalSize.Width = itemDesiredSizes.LogicalSize.Width + logicalSize.Width;
					childLogicalSize.Height = Math.Max(itemDesiredSizes.LogicalSize.Height, logicalSize.Height);
				}
				if (this.IsPixelBased && ((isHorizontal && DoubleUtil.AreClose(itemDesiredSizes.PixelSize.Width, itemDesiredSizes.PixelSizeInViewport.Width)) || (!isHorizontal && DoubleUtil.AreClose(itemDesiredSizes.PixelSize.Height, itemDesiredSizes.PixelSizeInViewport.Height))))
				{
					Rect childViewport2 = childViewport;
					if (relativeHeaderPosition == RelativeHeaderPosition.Top || relativeHeaderPosition == RelativeHeaderPosition.Left)
					{
						VirtualizationCacheLength virtualizationCacheLength = childCacheSize;
						VirtualizationCacheLengthUnit virtualizationCacheLengthUnit = childCacheUnit;
						this.AdjustNonScrollingViewportForHeader(virtualizingChild, ref childViewport2, ref virtualizationCacheLength, ref virtualizationCacheLengthUnit);
					}
					this.GetSizesForChildIntersectingTheViewport(isHorizontal, isChildHorizontal, itemDesiredSizes.PixelSizeInViewport, itemDesiredSizes.LogicalSizeInViewport, childViewport2, ref childPixelSizeInViewport, ref childLogicalSizeInViewport, ref childPixelSizeInCacheBeforeViewport, ref childLogicalSizeInCacheBeforeViewport, ref childPixelSizeInCacheAfterViewport, ref childLogicalSizeInCacheAfterViewport);
				}
				else
				{
					VirtualizingStackPanel.StackSizes(isHorizontal, ref childPixelSizeInViewport, itemDesiredSizes.PixelSizeInViewport);
					VirtualizingStackPanel.StackSizes(isHorizontal, ref childLogicalSizeInViewport, itemDesiredSizes.LogicalSizeInViewport);
				}
				if (isChildHorizontal == isHorizontal)
				{
					VirtualizingStackPanel.StackSizes(isHorizontal, ref childPixelSizeInCacheBeforeViewport, itemDesiredSizes.PixelSizeBeforeViewport);
					VirtualizingStackPanel.StackSizes(isHorizontal, ref childLogicalSizeInCacheBeforeViewport, itemDesiredSizes.LogicalSizeBeforeViewport);
					VirtualizingStackPanel.StackSizes(isHorizontal, ref childPixelSizeInCacheAfterViewport, itemDesiredSizes.PixelSizeAfterViewport);
					VirtualizingStackPanel.StackSizes(isHorizontal, ref childLogicalSizeInCacheAfterViewport, itemDesiredSizes.LogicalSizeAfterViewport);
				}
				Rect childViewport3 = childViewport;
				Size sz = default(Size);
				Size sz2 = default(Size);
				Size sz3 = default(Size);
				Size sz4 = default(Size);
				Size sz5 = default(Size);
				Size sz6 = default(Size);
				bool isHorizontal2 = relativeHeaderPosition == RelativeHeaderPosition.Left || relativeHeaderPosition == RelativeHeaderPosition.Right;
				if (relativeHeaderPosition == RelativeHeaderPosition.Bottom || relativeHeaderPosition == RelativeHeaderPosition.Right)
				{
					VirtualizationCacheLength virtualizationCacheLength2 = childCacheSize;
					VirtualizationCacheLengthUnit virtualizationCacheLengthUnit2 = childCacheUnit;
					this.AdjustNonScrollingViewportForItems(virtualizingChild, ref childViewport3, ref virtualizationCacheLength2, ref virtualizationCacheLengthUnit2);
				}
				if (isBeforeFirstItem)
				{
					sz3 = pixelSize;
					sz4 = logicalSize;
				}
				else if (isAfterLastItem)
				{
					sz5 = pixelSize;
					sz6 = logicalSize;
				}
				else
				{
					this.GetSizesForChildIntersectingTheViewport(isHorizontal, isChildHorizontal, pixelSize, logicalSize, childViewport3, ref sz, ref sz2, ref sz3, ref sz4, ref sz5, ref sz6);
				}
				VirtualizingStackPanel.StackSizes(isHorizontal2, ref childPixelSizeInViewport, sz);
				VirtualizingStackPanel.StackSizes(isHorizontal2, ref childLogicalSizeInViewport, sz2);
				VirtualizingStackPanel.StackSizes(isHorizontal2, ref childPixelSizeInCacheBeforeViewport, sz3);
				VirtualizingStackPanel.StackSizes(isHorizontal2, ref childLogicalSizeInCacheBeforeViewport, sz4);
				VirtualizingStackPanel.StackSizes(isHorizontal2, ref childPixelSizeInCacheAfterViewport, sz5);
				VirtualizingStackPanel.StackSizes(isHorizontal2, ref childLogicalSizeInCacheAfterViewport, sz6);
				return;
			}
			childPixelSize = childDesiredSize;
			childLogicalSize = new Size((double)(DoubleUtil.GreaterThan(childPixelSize.Width, 0.0) ? 1 : 0), (double)(DoubleUtil.GreaterThan(childPixelSize.Height, 0.0) ? 1 : 0));
			if (isBeforeFirstItem)
			{
				childPixelSizeInCacheBeforeViewport = childDesiredSize;
				childLogicalSizeInCacheBeforeViewport = new Size((double)(DoubleUtil.GreaterThan(childPixelSizeInCacheBeforeViewport.Width, 0.0) ? 1 : 0), (double)(DoubleUtil.GreaterThan(childPixelSizeInCacheBeforeViewport.Height, 0.0) ? 1 : 0));
				return;
			}
			if (isAfterLastItem)
			{
				childPixelSizeInCacheAfterViewport = childDesiredSize;
				childLogicalSizeInCacheAfterViewport = new Size((double)(DoubleUtil.GreaterThan(childPixelSizeInCacheAfterViewport.Width, 0.0) ? 1 : 0), (double)(DoubleUtil.GreaterThan(childPixelSizeInCacheAfterViewport.Height, 0.0) ? 1 : 0));
				return;
			}
			this.GetSizesForChildIntersectingTheViewport(isHorizontal, isHorizontal, childPixelSize, childLogicalSize, childViewport, ref childPixelSizeInViewport, ref childLogicalSizeInViewport, ref childPixelSizeInCacheBeforeViewport, ref childLogicalSizeInCacheBeforeViewport, ref childPixelSizeInCacheAfterViewport, ref childLogicalSizeInCacheAfterViewport);
		}

		// Token: 0x06005AA3 RID: 23203 RVA: 0x00195114 File Offset: 0x00193314
		private void GetSizesForChildWithInset(bool isHorizontal, bool isChildHorizontal, bool isBeforeFirstItem, bool isAfterLastItem, IHierarchicalVirtualizationAndScrollInfo virtualizingChild, Size childDesiredSize, Rect childViewport, VirtualizationCacheLength childCacheSize, VirtualizationCacheLengthUnit childCacheUnit, out Size childPixelSize, out Size childPixelSizeInViewport, out Size childPixelSizeInCacheBeforeViewport, out Size childPixelSizeInCacheAfterViewport, out Size childLogicalSize, out Size childLogicalSizeInViewport, out Size childLogicalSizeInCacheBeforeViewport, out Size childLogicalSizeInCacheAfterViewport)
		{
			childPixelSize = childDesiredSize;
			childPixelSizeInViewport = default(Size);
			childPixelSizeInCacheBeforeViewport = default(Size);
			childPixelSizeInCacheAfterViewport = default(Size);
			childLogicalSize = default(Size);
			childLogicalSizeInViewport = default(Size);
			childLogicalSizeInCacheBeforeViewport = default(Size);
			childLogicalSizeInCacheAfterViewport = default(Size);
			HierarchicalVirtualizationItemDesiredSizes hierarchicalVirtualizationItemDesiredSizes = (virtualizingChild != null) ? virtualizingChild.ItemDesiredSizes : default(HierarchicalVirtualizationItemDesiredSizes);
			if ((!isHorizontal && (hierarchicalVirtualizationItemDesiredSizes.PixelSize.Height > 0.0 || hierarchicalVirtualizationItemDesiredSizes.LogicalSize.Height > 0.0)) || (isHorizontal && (hierarchicalVirtualizationItemDesiredSizes.PixelSize.Width > 0.0 || hierarchicalVirtualizationItemDesiredSizes.LogicalSize.Width > 0.0)))
			{
				VirtualizingStackPanel.StackSizes(isHorizontal, ref childPixelSizeInCacheBeforeViewport, hierarchicalVirtualizationItemDesiredSizes.PixelSizeBeforeViewport);
				VirtualizingStackPanel.StackSizes(isHorizontal, ref childPixelSizeInViewport, hierarchicalVirtualizationItemDesiredSizes.PixelSizeInViewport);
				VirtualizingStackPanel.StackSizes(isHorizontal, ref childPixelSizeInCacheAfterViewport, hierarchicalVirtualizationItemDesiredSizes.PixelSizeAfterViewport);
				VirtualizingStackPanel.StackSizes(isHorizontal, ref childLogicalSize, hierarchicalVirtualizationItemDesiredSizes.LogicalSize);
				VirtualizingStackPanel.StackSizes(isHorizontal, ref childLogicalSizeInCacheBeforeViewport, hierarchicalVirtualizationItemDesiredSizes.LogicalSizeBeforeViewport);
				VirtualizingStackPanel.StackSizes(isHorizontal, ref childLogicalSizeInViewport, hierarchicalVirtualizationItemDesiredSizes.LogicalSizeInViewport);
				VirtualizingStackPanel.StackSizes(isHorizontal, ref childLogicalSizeInCacheAfterViewport, hierarchicalVirtualizationItemDesiredSizes.LogicalSizeAfterViewport);
				Thickness itemsHostInsetForChild = this.GetItemsHostInsetForChild(virtualizingChild, null, null);
				bool flag = this.IsHeaderBeforeItems(isHorizontal, virtualizingChild as FrameworkElement, ref itemsHostInsetForChild);
				Size childPixelSize2 = isHorizontal ? new Size(Math.Max(itemsHostInsetForChild.Left, 0.0), childDesiredSize.Height) : new Size(childDesiredSize.Width, Math.Max(itemsHostInsetForChild.Top, 0.0));
				Size size = flag ? new Size(1.0, 1.0) : new Size(0.0, 0.0);
				VirtualizingStackPanel.StackSizes(isHorizontal, ref childLogicalSize, size);
				this.GetSizesForChildIntersectingTheViewport(isHorizontal, isChildHorizontal, childPixelSize2, size, childViewport, ref childPixelSizeInViewport, ref childLogicalSizeInViewport, ref childPixelSizeInCacheBeforeViewport, ref childLogicalSizeInCacheBeforeViewport, ref childPixelSizeInCacheAfterViewport, ref childLogicalSizeInCacheAfterViewport);
				Size childPixelSize3 = isHorizontal ? new Size(Math.Max(itemsHostInsetForChild.Right, 0.0), childDesiredSize.Height) : new Size(childDesiredSize.Width, Math.Max(itemsHostInsetForChild.Bottom, 0.0));
				Size size2 = flag ? new Size(0.0, 0.0) : new Size(1.0, 1.0);
				VirtualizingStackPanel.StackSizes(isHorizontal, ref childLogicalSize, size2);
				Rect childViewport2 = childViewport;
				if (isHorizontal)
				{
					childViewport2.X -= (this.IsPixelBased ? (childPixelSize2.Width + hierarchicalVirtualizationItemDesiredSizes.PixelSize.Width) : (size.Width + hierarchicalVirtualizationItemDesiredSizes.LogicalSize.Width));
					childViewport2.Width = Math.Max(0.0, childViewport2.Width - childPixelSizeInViewport.Width);
				}
				else
				{
					childViewport2.Y -= (this.IsPixelBased ? (childPixelSize2.Height + hierarchicalVirtualizationItemDesiredSizes.PixelSize.Height) : (size.Height + hierarchicalVirtualizationItemDesiredSizes.LogicalSize.Height));
					childViewport2.Height = Math.Max(0.0, childViewport2.Height - childPixelSizeInViewport.Height);
				}
				this.GetSizesForChildIntersectingTheViewport(isHorizontal, isChildHorizontal, childPixelSize3, size2, childViewport2, ref childPixelSizeInViewport, ref childLogicalSizeInViewport, ref childPixelSizeInCacheBeforeViewport, ref childLogicalSizeInCacheBeforeViewport, ref childPixelSizeInCacheAfterViewport, ref childLogicalSizeInCacheAfterViewport);
				return;
			}
			childLogicalSize = new Size(1.0, 1.0);
			if (isBeforeFirstItem)
			{
				childPixelSizeInCacheBeforeViewport = childDesiredSize;
				childLogicalSizeInCacheBeforeViewport = new Size((double)(DoubleUtil.GreaterThan(childPixelSizeInCacheBeforeViewport.Width, 0.0) ? 1 : 0), (double)(DoubleUtil.GreaterThan(childPixelSizeInCacheBeforeViewport.Height, 0.0) ? 1 : 0));
				return;
			}
			if (isAfterLastItem)
			{
				childPixelSizeInCacheAfterViewport = childDesiredSize;
				childLogicalSizeInCacheAfterViewport = new Size((double)(DoubleUtil.GreaterThan(childPixelSizeInCacheAfterViewport.Width, 0.0) ? 1 : 0), (double)(DoubleUtil.GreaterThan(childPixelSizeInCacheAfterViewport.Height, 0.0) ? 1 : 0));
				return;
			}
			this.GetSizesForChildIntersectingTheViewport(isHorizontal, isChildHorizontal, childPixelSize, childLogicalSize, childViewport, ref childPixelSizeInViewport, ref childLogicalSizeInViewport, ref childPixelSizeInCacheBeforeViewport, ref childLogicalSizeInCacheBeforeViewport, ref childPixelSizeInCacheAfterViewport, ref childLogicalSizeInCacheAfterViewport);
		}

		// Token: 0x06005AA4 RID: 23204 RVA: 0x0019557C File Offset: 0x0019377C
		private void GetSizesForChildIntersectingTheViewport(bool isHorizontal, bool childIsHorizontal, Size childPixelSize, Size childLogicalSize, Rect childViewport, ref Size childPixelSizeInViewport, ref Size childLogicalSizeInViewport, ref Size childPixelSizeInCacheBeforeViewport, ref Size childLogicalSizeInCacheBeforeViewport, ref Size childPixelSizeInCacheAfterViewport, ref Size childLogicalSizeInCacheAfterViewport)
		{
			bool isVSP45Compat = VirtualizingStackPanel.IsVSP45Compat;
			double num = 0.0;
			double num2 = 0.0;
			double num3 = 0.0;
			double num4 = 0.0;
			double num5 = 0.0;
			double num6 = 0.0;
			if (isHorizontal)
			{
				if (this.IsPixelBased)
				{
					if (childIsHorizontal != isHorizontal && (DoubleUtil.GreaterThanOrClose(childViewport.Y, childPixelSize.Height) || DoubleUtil.AreClose(childViewport.Height, 0.0)))
					{
						return;
					}
					num3 = (DoubleUtil.LessThan(childViewport.X, childPixelSize.Width) ? Math.Max(childViewport.X, 0.0) : childPixelSize.Width);
					num = Math.Min(childViewport.Width, childPixelSize.Width - num3);
					num5 = Math.Max(childPixelSize.Width - num - num3, 0.0);
				}
				else
				{
					if (childIsHorizontal != isHorizontal && (DoubleUtil.GreaterThanOrClose(childViewport.Y, childLogicalSize.Height) || DoubleUtil.AreClose(childViewport.Height, 0.0)))
					{
						return;
					}
					if (DoubleUtil.GreaterThanOrClose(childViewport.X, childLogicalSize.Width))
					{
						num3 = childPixelSize.Width;
						if (!isVSP45Compat)
						{
							num4 = childLogicalSize.Width;
						}
					}
					else if (DoubleUtil.GreaterThan(childViewport.Width, 0.0))
					{
						num = childPixelSize.Width;
					}
					else
					{
						num5 = childPixelSize.Width;
						if (!isVSP45Compat)
						{
							num6 = childLogicalSize.Width;
						}
					}
				}
				if (DoubleUtil.GreaterThan(childPixelSize.Width, 0.0))
				{
					num4 = Math.Floor(childLogicalSize.Width * num3 / childPixelSize.Width);
					num6 = Math.Floor(childLogicalSize.Width * num5 / childPixelSize.Width);
					num2 = childLogicalSize.Width - num4 - num6;
				}
				else if (!isVSP45Compat)
				{
					num2 = childLogicalSize.Width - num4 - num6;
				}
				double val = Math.Min(childViewport.Height, childPixelSize.Height - Math.Max(childViewport.Y, 0.0));
				childPixelSizeInViewport.Width += num;
				childPixelSizeInViewport.Height = Math.Max(childPixelSizeInViewport.Height, val);
				childPixelSizeInCacheBeforeViewport.Width += num3;
				childPixelSizeInCacheBeforeViewport.Height = Math.Max(childPixelSizeInCacheBeforeViewport.Height, val);
				childPixelSizeInCacheAfterViewport.Width += num5;
				childPixelSizeInCacheAfterViewport.Height = Math.Max(childPixelSizeInCacheAfterViewport.Height, val);
				childLogicalSizeInViewport.Width += num2;
				childLogicalSizeInViewport.Height = Math.Max(childLogicalSizeInViewport.Height, childLogicalSize.Height);
				childLogicalSizeInCacheBeforeViewport.Width += num4;
				childLogicalSizeInCacheBeforeViewport.Height = Math.Max(childLogicalSizeInCacheBeforeViewport.Height, childLogicalSize.Height);
				childLogicalSizeInCacheAfterViewport.Width += num6;
				childLogicalSizeInCacheAfterViewport.Height = Math.Max(childLogicalSizeInCacheAfterViewport.Height, childLogicalSize.Height);
				return;
			}
			if (this.IsPixelBased)
			{
				if (childIsHorizontal != isHorizontal && (DoubleUtil.GreaterThanOrClose(childViewport.X, childPixelSize.Width) || DoubleUtil.AreClose(childViewport.Width, 0.0)))
				{
					return;
				}
				num3 = (DoubleUtil.LessThan(childViewport.Y, childPixelSize.Height) ? Math.Max(childViewport.Y, 0.0) : childPixelSize.Height);
				num = Math.Min(childViewport.Height, childPixelSize.Height - num3);
				num5 = Math.Max(childPixelSize.Height - num - num3, 0.0);
			}
			else
			{
				if (childIsHorizontal != isHorizontal && (DoubleUtil.GreaterThanOrClose(childViewport.X, childLogicalSize.Width) || DoubleUtil.AreClose(childViewport.Width, 0.0)))
				{
					return;
				}
				if (DoubleUtil.GreaterThanOrClose(childViewport.Y, childLogicalSize.Height))
				{
					num3 = childPixelSize.Height;
					if (!isVSP45Compat)
					{
						num4 = childLogicalSize.Height;
					}
				}
				else if (DoubleUtil.GreaterThan(childViewport.Height, 0.0))
				{
					num = childPixelSize.Height;
				}
				else
				{
					num5 = childPixelSize.Height;
					if (!isVSP45Compat)
					{
						num6 = childLogicalSize.Height;
					}
				}
			}
			if (DoubleUtil.GreaterThan(childPixelSize.Height, 0.0))
			{
				num4 = Math.Floor(childLogicalSize.Height * num3 / childPixelSize.Height);
				num6 = Math.Floor(childLogicalSize.Height * num5 / childPixelSize.Height);
				num2 = childLogicalSize.Height - num4 - num6;
			}
			else if (!VirtualizingStackPanel.IsVSP45Compat)
			{
				num2 = childLogicalSize.Height - num4 - num6;
			}
			double val2 = Math.Min(childViewport.Width, childPixelSize.Width - Math.Max(childViewport.X, 0.0));
			childPixelSizeInViewport.Height += num;
			childPixelSizeInViewport.Width = Math.Max(childPixelSizeInViewport.Width, val2);
			childPixelSizeInCacheBeforeViewport.Height += num3;
			childPixelSizeInCacheBeforeViewport.Width = Math.Max(childPixelSizeInCacheBeforeViewport.Width, val2);
			childPixelSizeInCacheAfterViewport.Height += num5;
			childPixelSizeInCacheAfterViewport.Width = Math.Max(childPixelSizeInCacheAfterViewport.Width, val2);
			childLogicalSizeInViewport.Height += num2;
			childLogicalSizeInViewport.Width = Math.Max(childLogicalSizeInViewport.Width, childLogicalSize.Width);
			childLogicalSizeInCacheBeforeViewport.Height += num4;
			childLogicalSizeInCacheBeforeViewport.Width = Math.Max(childLogicalSizeInCacheBeforeViewport.Width, childLogicalSize.Width);
			childLogicalSizeInCacheAfterViewport.Height += num6;
			childLogicalSizeInCacheAfterViewport.Width = Math.Max(childLogicalSizeInCacheAfterViewport.Width, childLogicalSize.Width);
		}

		// Token: 0x06005AA5 RID: 23205 RVA: 0x00195B48 File Offset: 0x00193D48
		private void UpdateStackSizes(bool isHorizontal, bool foundFirstItemInViewport, Size childPixelSize, Size childPixelSizeInViewport, Size childPixelSizeInCacheBeforeViewport, Size childPixelSizeInCacheAfterViewport, Size childLogicalSize, Size childLogicalSizeInViewport, Size childLogicalSizeInCacheBeforeViewport, Size childLogicalSizeInCacheAfterViewport, ref Size stackPixelSize, ref Size stackPixelSizeInViewport, ref Size stackPixelSizeInCacheBeforeViewport, ref Size stackPixelSizeInCacheAfterViewport, ref Size stackLogicalSize, ref Size stackLogicalSizeInViewport, ref Size stackLogicalSizeInCacheBeforeViewport, ref Size stackLogicalSizeInCacheAfterViewport)
		{
			VirtualizingStackPanel.StackSizes(isHorizontal, ref stackPixelSize, childPixelSize);
			VirtualizingStackPanel.StackSizes(isHorizontal, ref stackLogicalSize, childLogicalSize);
			if (foundFirstItemInViewport)
			{
				VirtualizingStackPanel.StackSizes(isHorizontal, ref stackPixelSizeInViewport, childPixelSizeInViewport);
				VirtualizingStackPanel.StackSizes(isHorizontal, ref stackLogicalSizeInViewport, childLogicalSizeInViewport);
				VirtualizingStackPanel.StackSizes(isHorizontal, ref stackPixelSizeInCacheBeforeViewport, childPixelSizeInCacheBeforeViewport);
				VirtualizingStackPanel.StackSizes(isHorizontal, ref stackLogicalSizeInCacheBeforeViewport, childLogicalSizeInCacheBeforeViewport);
				VirtualizingStackPanel.StackSizes(isHorizontal, ref stackPixelSizeInCacheAfterViewport, childPixelSizeInCacheAfterViewport);
				VirtualizingStackPanel.StackSizes(isHorizontal, ref stackLogicalSizeInCacheAfterViewport, childLogicalSizeInCacheAfterViewport);
			}
		}

		// Token: 0x06005AA6 RID: 23206 RVA: 0x00195BA8 File Offset: 0x00193DA8
		private static void StackSizes(bool isHorizontal, ref Size sz1, Size sz2)
		{
			if (isHorizontal)
			{
				sz1.Width += sz2.Width;
				sz1.Height = Math.Max(sz1.Height, sz2.Height);
				return;
			}
			sz1.Height += sz2.Height;
			sz1.Width = Math.Max(sz1.Width, sz2.Width);
		}

		// Token: 0x06005AA7 RID: 23207 RVA: 0x00195C14 File Offset: 0x00193E14
		private void SyncUniformSizeFlags(object parentItem, IContainItemStorage parentItemStorageProvider, IList children, IList items, IContainItemStorage itemStorageProvider, int itemCount, bool computedAreContainersUniformlySized, double computedUniformOrAverageContainerSize, ref bool areContainersUniformlySized, ref double uniformOrAverageContainerSize, ref bool hasAverageContainerSizeChanged, bool isHorizontal, bool evaluateAreContainersUniformlySized)
		{
			parentItemStorageProvider = itemStorageProvider;
			if (evaluateAreContainersUniformlySized || areContainersUniformlySized != computedAreContainersUniformlySized)
			{
				if (!evaluateAreContainersUniformlySized)
				{
					areContainersUniformlySized = computedAreContainersUniformlySized;
					this.SetAreContainersUniformlySized(parentItemStorageProvider, parentItem, areContainersUniformlySized);
				}
				for (int i = 0; i < children.Count; i++)
				{
					UIElement uielement = children[i] as UIElement;
					if (uielement != null && VirtualizingPanel.GetShouldCacheContainerSize(uielement))
					{
						IHierarchicalVirtualizationAndScrollInfo virtualizingChild = VirtualizingStackPanel.GetVirtualizingChild(uielement);
						Size desiredSize;
						if (virtualizingChild != null)
						{
							HierarchicalVirtualizationHeaderDesiredSizes headerDesiredSizes = virtualizingChild.HeaderDesiredSizes;
							HierarchicalVirtualizationItemDesiredSizes itemDesiredSizes = virtualizingChild.ItemDesiredSizes;
							if (this.IsPixelBased)
							{
								desiredSize = new Size(Math.Max(headerDesiredSizes.PixelSize.Width, itemDesiredSizes.PixelSize.Width), headerDesiredSizes.PixelSize.Height + itemDesiredSizes.PixelSize.Height);
							}
							else
							{
								desiredSize = new Size(Math.Max(headerDesiredSizes.LogicalSize.Width, itemDesiredSizes.LogicalSize.Width), headerDesiredSizes.LogicalSize.Height + itemDesiredSizes.LogicalSize.Height);
							}
						}
						else if (this.IsPixelBased)
						{
							desiredSize = uielement.DesiredSize;
						}
						else
						{
							desiredSize = new Size((double)(DoubleUtil.GreaterThan(uielement.DesiredSize.Width, 0.0) ? 1 : 0), (double)(DoubleUtil.GreaterThan(uielement.DesiredSize.Height, 0.0) ? 1 : 0));
						}
						if (evaluateAreContainersUniformlySized && computedAreContainersUniformlySized)
						{
							if (isHorizontal)
							{
								computedAreContainersUniformlySized = DoubleUtil.AreClose(desiredSize.Width, uniformOrAverageContainerSize);
							}
							else
							{
								computedAreContainersUniformlySized = DoubleUtil.AreClose(desiredSize.Height, uniformOrAverageContainerSize);
							}
							if (!computedAreContainersUniformlySized)
							{
								i = -1;
							}
						}
						else
						{
							itemStorageProvider.StoreItemValue(((ItemContainerGenerator)base.Generator).ItemFromContainer(uielement), VirtualizingStackPanel.ContainerSizeProperty, desiredSize);
						}
					}
				}
				if (evaluateAreContainersUniformlySized)
				{
					areContainersUniformlySized = computedAreContainersUniformlySized;
					this.SetAreContainersUniformlySized(parentItemStorageProvider, parentItem, areContainersUniformlySized);
				}
			}
			if (!computedAreContainersUniformlySized)
			{
				double num = 0.0;
				int num2 = 0;
				for (int j = 0; j < itemCount; j++)
				{
					object obj = itemStorageProvider.ReadItemValue(items[j], VirtualizingStackPanel.ContainerSizeProperty);
					if (obj != null)
					{
						Size size = (Size)obj;
						if (isHorizontal)
						{
							num += size.Width;
							num2++;
						}
						else
						{
							num += size.Height;
							num2++;
						}
					}
				}
				if (num2 > 0)
				{
					if (this.IsPixelBased)
					{
						uniformOrAverageContainerSize = num / (double)num2;
					}
					else
					{
						uniformOrAverageContainerSize = Math.Round(num / (double)num2);
					}
				}
			}
			else
			{
				uniformOrAverageContainerSize = computedUniformOrAverageContainerSize;
			}
			if (VirtualizingStackPanel.ScrollTracer.IsEnabled && VirtualizingStackPanel.ScrollTracer.IsTracing(this))
			{
				VirtualizingStackPanel.ScrollTracer.Trace(this, VirtualizingStackPanel.ScrollTraceOp.SyncAveSize, new object[]
				{
					uniformOrAverageContainerSize,
					areContainersUniformlySized,
					hasAverageContainerSizeChanged
				});
			}
		}

		// Token: 0x06005AA8 RID: 23208 RVA: 0x00195EF0 File Offset: 0x001940F0
		private void SyncUniformSizeFlags(object parentItem, IContainItemStorage parentItemStorageProvider, IList children, IList items, IContainItemStorage itemStorageProvider, int itemCount, bool computedAreContainersUniformlySized, double computedUniformOrAverageContainerSize, double computedUniformOrAverageContainerPixelSize, ref bool areContainersUniformlySized, ref double uniformOrAverageContainerSize, ref double uniformOrAverageContainerPixelSize, ref bool hasAverageContainerSizeChanged, bool isHorizontal, bool evaluateAreContainersUniformlySized)
		{
			bool flag = VirtualizingStackPanel.ScrollTracer.IsEnabled && VirtualizingStackPanel.ScrollTracer.IsTracing(this);
			ItemContainerGenerator itemContainerGenerator = (ItemContainerGenerator)base.Generator;
			if (evaluateAreContainersUniformlySized || areContainersUniformlySized != computedAreContainersUniformlySized)
			{
				if (!evaluateAreContainersUniformlySized)
				{
					areContainersUniformlySized = computedAreContainersUniformlySized;
					this.SetAreContainersUniformlySized(parentItemStorageProvider, parentItem, areContainersUniformlySized);
				}
				for (int i = 0; i < children.Count; i++)
				{
					UIElement uielement = children[i] as UIElement;
					if (uielement != null && VirtualizingPanel.GetShouldCacheContainerSize(uielement))
					{
						IHierarchicalVirtualizationAndScrollInfo virtualizingChild = VirtualizingStackPanel.GetVirtualizingChild(uielement);
						Size desiredSize;
						Size size;
						if (virtualizingChild != null)
						{
							HierarchicalVirtualizationItemDesiredSizes itemDesiredSizes = virtualizingChild.ItemDesiredSizes;
							object obj = uielement.ReadLocalValue(VirtualizingStackPanel.ItemsHostInsetProperty);
							if (obj != DependencyProperty.UnsetValue)
							{
								Thickness thickness = (Thickness)obj;
								desiredSize = new Size(thickness.Left + itemDesiredSizes.PixelSize.Width + thickness.Right, thickness.Top + itemDesiredSizes.PixelSize.Height + thickness.Bottom);
							}
							else
							{
								desiredSize = uielement.DesiredSize;
							}
							if (this.IsPixelBased)
							{
								size = desiredSize;
							}
							else
							{
								size = (isHorizontal ? new Size(1.0 + itemDesiredSizes.LogicalSize.Width, Math.Max(1.0, itemDesiredSizes.LogicalSize.Height)) : new Size(Math.Max(1.0, itemDesiredSizes.LogicalSize.Width), 1.0 + itemDesiredSizes.LogicalSize.Height));
							}
						}
						else
						{
							desiredSize = uielement.DesiredSize;
							if (this.IsPixelBased)
							{
								size = desiredSize;
							}
							else
							{
								size = new Size((double)(DoubleUtil.GreaterThan(uielement.DesiredSize.Width, 0.0) ? 1 : 0), (double)(DoubleUtil.GreaterThan(uielement.DesiredSize.Height, 0.0) ? 1 : 0));
							}
						}
						if (evaluateAreContainersUniformlySized && computedAreContainersUniformlySized)
						{
							if (isHorizontal)
							{
								computedAreContainersUniformlySized = (DoubleUtil.AreClose(size.Width, uniformOrAverageContainerSize) && (this.IsPixelBased || DoubleUtil.AreClose(desiredSize.Width, uniformOrAverageContainerPixelSize)));
							}
							else
							{
								computedAreContainersUniformlySized = (DoubleUtil.AreClose(size.Height, uniformOrAverageContainerSize) && (this.IsPixelBased || DoubleUtil.AreClose(desiredSize.Height, uniformOrAverageContainerPixelSize)));
							}
							if (!computedAreContainersUniformlySized)
							{
								i = -1;
							}
						}
						else if (this.IsPixelBased)
						{
							if (flag)
							{
								VirtualizingStackPanel.ScrollTracer.Trace(this, VirtualizingStackPanel.ScrollTraceOp.SetContainerSize, new object[]
								{
									itemContainerGenerator.IndexFromContainer(uielement),
									size
								});
							}
							itemStorageProvider.StoreItemValue(itemContainerGenerator.ItemFromContainer(uielement), VirtualizingStackPanel.ContainerSizeProperty, size);
						}
						else
						{
							if (flag)
							{
								VirtualizingStackPanel.ScrollTracer.Trace(this, VirtualizingStackPanel.ScrollTraceOp.SetContainerSize, new object[]
								{
									itemContainerGenerator.IndexFromContainer(uielement),
									size,
									desiredSize
								});
							}
							VirtualizingStackPanel.ContainerSizeDual value = new VirtualizingStackPanel.ContainerSizeDual(desiredSize, size);
							itemStorageProvider.StoreItemValue(itemContainerGenerator.ItemFromContainer(uielement), VirtualizingStackPanel.ContainerSizeDualProperty, value);
						}
					}
				}
				if (evaluateAreContainersUniformlySized)
				{
					areContainersUniformlySized = computedAreContainersUniformlySized;
					this.SetAreContainersUniformlySized(parentItemStorageProvider, parentItem, areContainersUniformlySized);
				}
			}
			if (!computedAreContainersUniformlySized)
			{
				Size size2 = default(Size);
				Size size3 = default(Size);
				double num = 0.0;
				double num2 = 0.0;
				int num3 = 0;
				for (int j = 0; j < itemCount; j++)
				{
					object obj2;
					if (this.IsPixelBased)
					{
						obj2 = itemStorageProvider.ReadItemValue(items[j], VirtualizingStackPanel.ContainerSizeProperty);
						if (obj2 != null)
						{
							size2 = (Size)obj2;
							size3 = size2;
						}
					}
					else
					{
						obj2 = itemStorageProvider.ReadItemValue(items[j], VirtualizingStackPanel.ContainerSizeDualProperty);
						if (obj2 != null)
						{
							VirtualizingStackPanel.ContainerSizeDual containerSizeDual = (VirtualizingStackPanel.ContainerSizeDual)obj2;
							size2 = containerSizeDual.ItemSize;
							size3 = containerSizeDual.PixelSize;
						}
					}
					if (obj2 != null)
					{
						if (isHorizontal)
						{
							num += size2.Width;
							num2 += size3.Width;
							num3++;
						}
						else
						{
							num += size2.Height;
							num2 += size3.Height;
							num3++;
						}
					}
				}
				if (num3 > 0)
				{
					uniformOrAverageContainerPixelSize = num2 / (double)num3;
					if (base.UseLayoutRounding && !FrameworkAppContextSwitches.OptOutOfEffectiveOffsetHangFix)
					{
						DpiScale dpi = base.GetDpi();
						double num4 = isHorizontal ? dpi.DpiScaleX : dpi.DpiScaleY;
						uniformOrAverageContainerPixelSize = UIElement.RoundLayoutValue(Math.Max(uniformOrAverageContainerPixelSize, num4), num4);
					}
					if (this.IsPixelBased)
					{
						uniformOrAverageContainerSize = uniformOrAverageContainerPixelSize;
					}
					else
					{
						uniformOrAverageContainerSize = Math.Round(num / (double)num3);
					}
					if (this.SetUniformOrAverageContainerSize(parentItemStorageProvider, parentItem, uniformOrAverageContainerSize, uniformOrAverageContainerPixelSize))
					{
						hasAverageContainerSizeChanged = true;
					}
				}
			}
			else
			{
				uniformOrAverageContainerSize = computedUniformOrAverageContainerSize;
				uniformOrAverageContainerPixelSize = computedUniformOrAverageContainerPixelSize;
			}
			if (flag)
			{
				VirtualizingStackPanel.ScrollTracer.Trace(this, VirtualizingStackPanel.ScrollTraceOp.SyncAveSize, new object[]
				{
					uniformOrAverageContainerSize,
					uniformOrAverageContainerPixelSize,
					areContainersUniformlySized,
					hasAverageContainerSizeChanged
				});
			}
		}

		// Token: 0x06005AA9 RID: 23209 RVA: 0x001963EC File Offset: 0x001945EC
		private void ClearAsyncOperations()
		{
			bool isVSP45Compat = VirtualizingStackPanel.IsVSP45Compat;
			if (isVSP45Compat)
			{
				DispatcherOperation value = VirtualizingStackPanel.MeasureCachesOperationField.GetValue(this);
				if (value != null)
				{
					value.Abort();
					VirtualizingStackPanel.MeasureCachesOperationField.ClearValue(this);
				}
			}
			else
			{
				this.ClearMeasureCachesState();
			}
			DispatcherOperation value2 = VirtualizingStackPanel.AnchorOperationField.GetValue(this);
			if (value2 != null)
			{
				if (isVSP45Compat)
				{
					value2.Abort();
					VirtualizingStackPanel.AnchorOperationField.ClearValue(this);
				}
				else
				{
					this.ClearAnchorInformation(true);
				}
			}
			DispatcherOperation value3 = VirtualizingStackPanel.AnchoredInvalidateMeasureOperationField.GetValue(this);
			if (value3 != null)
			{
				value3.Abort();
				VirtualizingStackPanel.AnchoredInvalidateMeasureOperationField.ClearValue(this);
			}
			DispatcherOperation value4 = VirtualizingStackPanel.ClearIsScrollActiveOperationField.GetValue(this);
			if (value4 != null)
			{
				if (isVSP45Compat)
				{
					value4.Abort();
					VirtualizingStackPanel.ClearIsScrollActiveOperationField.ClearValue(this);
					return;
				}
				value4.Abort();
				this.ClearIsScrollActive();
			}
		}

		// Token: 0x06005AAA RID: 23210 RVA: 0x001964B0 File Offset: 0x001946B0
		private bool GetAreContainersUniformlySized(IContainItemStorage itemStorageProvider, object item)
		{
			if (item == this)
			{
				if (this.AreContainersUniformlySized != null)
				{
					return this.AreContainersUniformlySized.Value;
				}
			}
			else
			{
				object obj = itemStorageProvider.ReadItemValue(item, VirtualizingStackPanel.AreContainersUniformlySizedProperty);
				if (obj != null)
				{
					return (bool)obj;
				}
			}
			return true;
		}

		// Token: 0x06005AAB RID: 23211 RVA: 0x001964F8 File Offset: 0x001946F8
		private void SetAreContainersUniformlySized(IContainItemStorage itemStorageProvider, object item, bool value)
		{
			if (item == this)
			{
				this.AreContainersUniformlySized = new bool?(value);
				return;
			}
			itemStorageProvider.StoreItemValue(item, VirtualizingStackPanel.AreContainersUniformlySizedProperty, value);
		}

		// Token: 0x06005AAC RID: 23212 RVA: 0x00196520 File Offset: 0x00194720
		private double GetUniformOrAverageContainerSize(IContainItemStorage itemStorageProvider, object item)
		{
			double result;
			double num;
			this.GetUniformOrAverageContainerSize(itemStorageProvider, item, this.IsPixelBased || VirtualizingStackPanel.IsVSP45Compat, out result, out num);
			return result;
		}

		// Token: 0x06005AAD RID: 23213 RVA: 0x0019654C File Offset: 0x0019474C
		private void GetUniformOrAverageContainerSize(IContainItemStorage itemStorageProvider, object item, bool isSingleValue, out double uniformOrAverageContainerSize, out double uniformOrAverageContainerPixelSize)
		{
			bool flag;
			this.GetUniformOrAverageContainerSize(itemStorageProvider, item, isSingleValue, out uniformOrAverageContainerSize, out uniformOrAverageContainerPixelSize, out flag);
		}

		// Token: 0x06005AAE RID: 23214 RVA: 0x00196568 File Offset: 0x00194768
		private void GetUniformOrAverageContainerSize(IContainItemStorage itemStorageProvider, object item, bool isSingleValue, out double uniformOrAverageContainerSize, out double uniformOrAverageContainerPixelSize, out bool hasUniformOrAverageContainerSizeBeenSet)
		{
			if (item == this)
			{
				if (this.UniformOrAverageContainerSize != null)
				{
					hasUniformOrAverageContainerSizeBeenSet = true;
					uniformOrAverageContainerSize = this.UniformOrAverageContainerSize.Value;
					if (isSingleValue)
					{
						uniformOrAverageContainerPixelSize = uniformOrAverageContainerSize;
						return;
					}
					uniformOrAverageContainerPixelSize = this.UniformOrAverageContainerPixelSize.Value;
					return;
				}
			}
			else if (isSingleValue)
			{
				object obj = itemStorageProvider.ReadItemValue(item, VirtualizingStackPanel.UniformOrAverageContainerSizeProperty);
				if (obj != null)
				{
					hasUniformOrAverageContainerSizeBeenSet = true;
					uniformOrAverageContainerSize = (double)obj;
					uniformOrAverageContainerPixelSize = uniformOrAverageContainerSize;
					return;
				}
			}
			else
			{
				object obj2 = itemStorageProvider.ReadItemValue(item, VirtualizingStackPanel.UniformOrAverageContainerSizeDualProperty);
				if (obj2 != null)
				{
					VirtualizingStackPanel.UniformOrAverageContainerSizeDual uniformOrAverageContainerSizeDual = (VirtualizingStackPanel.UniformOrAverageContainerSizeDual)obj2;
					hasUniformOrAverageContainerSizeBeenSet = true;
					uniformOrAverageContainerSize = uniformOrAverageContainerSizeDual.ItemSize;
					uniformOrAverageContainerPixelSize = uniformOrAverageContainerSizeDual.PixelSize;
					return;
				}
			}
			hasUniformOrAverageContainerSizeBeenSet = false;
			uniformOrAverageContainerPixelSize = 16.0;
			uniformOrAverageContainerSize = (this.IsPixelBased ? uniformOrAverageContainerPixelSize : 1.0);
		}

		// Token: 0x06005AAF RID: 23215 RVA: 0x00196640 File Offset: 0x00194840
		private bool SetUniformOrAverageContainerSize(IContainItemStorage itemStorageProvider, object item, double value, double pixelValue)
		{
			bool result = false;
			if (DoubleUtil.GreaterThan(value, 0.0))
			{
				if (item == this)
				{
					if (this.UniformOrAverageContainerSize != value)
					{
						this.UniformOrAverageContainerSize = new double?(value);
						this.UniformOrAverageContainerPixelSize = new double?(pixelValue);
						result = true;
					}
				}
				else if (this.IsPixelBased || VirtualizingStackPanel.IsVSP45Compat)
				{
					object objA = itemStorageProvider.ReadItemValue(item, VirtualizingStackPanel.UniformOrAverageContainerSizeProperty);
					itemStorageProvider.StoreItemValue(item, VirtualizingStackPanel.UniformOrAverageContainerSizeProperty, value);
					result = !object.Equals(objA, value);
				}
				else
				{
					VirtualizingStackPanel.UniformOrAverageContainerSizeDual uniformOrAverageContainerSizeDual = itemStorageProvider.ReadItemValue(item, VirtualizingStackPanel.UniformOrAverageContainerSizeDualProperty) as VirtualizingStackPanel.UniformOrAverageContainerSizeDual;
					VirtualizingStackPanel.UniformOrAverageContainerSizeDual value2 = new VirtualizingStackPanel.UniformOrAverageContainerSizeDual(pixelValue, value);
					itemStorageProvider.StoreItemValue(item, VirtualizingStackPanel.UniformOrAverageContainerSizeDualProperty, value2);
					result = (uniformOrAverageContainerSizeDual == null || uniformOrAverageContainerSizeDual.ItemSize != value);
				}
			}
			return result;
		}

		// Token: 0x06005AB0 RID: 23216 RVA: 0x00196730 File Offset: 0x00194930
		private void MeasureExistingChildBeyondExtendedViewport(ref IItemContainerGenerator generator, ref IContainItemStorage itemStorageProvider, ref IContainItemStorage parentItemStorageProvider, ref object parentItem, ref bool hasUniformOrAverageContainerSizeBeenSet, ref double computedUniformOrAverageContainerSize, ref double computedUniformOrAverageContainerPixelSize, ref bool computedAreContainersUniformlySized, ref bool hasAnyContainerSpanChanged, ref IList items, ref IList children, ref int childIndex, ref bool visualOrderChanged, ref bool isHorizontal, ref Size childConstraint, ref bool foundFirstItemInViewport, ref double firstItemInViewportOffset, ref bool mustDisableVirtualization, ref bool hasVirtualizingChildren, ref bool hasBringIntoViewContainerBeenMeasured, ref long scrollGeneration)
		{
			object obj = ((ItemContainerGenerator)generator).ItemFromContainer((UIElement)children[childIndex]);
			Rect rect = default(Rect);
			VirtualizationCacheLength virtualizationCacheLength = default(VirtualizationCacheLength);
			VirtualizationCacheLengthUnit virtualizationCacheLengthUnit = VirtualizationCacheLengthUnit.Pixel;
			Size size = default(Size);
			Size size2 = default(Size);
			Size size3 = default(Size);
			Size size4 = default(Size);
			Size size5 = default(Size);
			Size size6 = default(Size);
			Size size7 = default(Size);
			Size size8 = default(Size);
			bool isBeforeFirstItem = childIndex < this._firstItemInExtendedViewportChildIndex;
			bool isAfterFirstItem = childIndex > this._firstItemInExtendedViewportChildIndex;
			bool isAfterLastItem = childIndex > this._firstItemInExtendedViewportChildIndex + this._actualItemsInExtendedViewportCount;
			bool skipActualMeasure = false;
			bool skipGeneration = true;
			this.MeasureChild(ref generator, ref itemStorageProvider, ref parentItemStorageProvider, ref parentItem, ref hasUniformOrAverageContainerSizeBeenSet, ref computedUniformOrAverageContainerSize, ref computedUniformOrAverageContainerPixelSize, ref computedAreContainersUniformlySized, ref hasAnyContainerSpanChanged, ref items, ref obj, ref children, ref childIndex, ref visualOrderChanged, ref isHorizontal, ref childConstraint, ref rect, ref virtualizationCacheLength, ref virtualizationCacheLengthUnit, ref scrollGeneration, ref foundFirstItemInViewport, ref firstItemInViewportOffset, ref size, ref size2, ref size3, ref size4, ref size5, ref size6, ref size7, ref size8, ref mustDisableVirtualization, isBeforeFirstItem, isAfterFirstItem, isAfterLastItem, skipActualMeasure, skipGeneration, ref hasBringIntoViewContainerBeenMeasured, ref hasVirtualizingChildren);
		}

		// Token: 0x06005AB1 RID: 23217 RVA: 0x00196830 File Offset: 0x00194A30
		private void MeasureChild(ref IItemContainerGenerator generator, ref IContainItemStorage itemStorageProvider, ref IContainItemStorage parentItemStorageProvider, ref object parentItem, ref bool hasUniformOrAverageContainerSizeBeenSet, ref double computedUniformOrAverageContainerSize, ref double computedUniformOrAverageContainerPixelSize, ref bool computedAreContainersUniformlySized, ref bool hasAnyContainerSpanChanged, ref IList items, ref object item, ref IList children, ref int childIndex, ref bool visualOrderChanged, ref bool isHorizontal, ref Size childConstraint, ref Rect viewport, ref VirtualizationCacheLength cacheSize, ref VirtualizationCacheLengthUnit cacheUnit, ref long scrollGeneration, ref bool foundFirstItemInViewport, ref double firstItemInViewportOffset, ref Size stackPixelSize, ref Size stackPixelSizeInViewport, ref Size stackPixelSizeInCacheBeforeViewport, ref Size stackPixelSizeInCacheAfterViewport, ref Size stackLogicalSize, ref Size stackLogicalSizeInViewport, ref Size stackLogicalSizeInCacheBeforeViewport, ref Size stackLogicalSizeInCacheAfterViewport, ref bool mustDisableVirtualization, bool isBeforeFirstItem, bool isAfterFirstItem, bool isAfterLastItem, bool skipActualMeasure, bool skipGeneration, ref bool hasBringIntoViewContainerBeenMeasured, ref bool hasVirtualizingChildren)
		{
			Rect empty = Rect.Empty;
			VirtualizationCacheLength childCacheSize = new VirtualizationCacheLength(0.0);
			VirtualizationCacheLengthUnit childCacheUnit = VirtualizationCacheLengthUnit.Pixel;
			Size childDesiredSize = default(Size);
			UIElement uielement;
			if (!skipActualMeasure && !skipGeneration)
			{
				bool newlyRealized;
				uielement = (generator.GenerateNext(out newlyRealized) as UIElement);
				ItemContainerGenerator itemContainerGenerator;
				if (uielement == null && (itemContainerGenerator = (generator as ItemContainerGenerator)) != null)
				{
					itemContainerGenerator.Verify();
				}
				visualOrderChanged |= this.AddContainerFromGenerator(childIndex, uielement, newlyRealized, isBeforeFirstItem);
			}
			else
			{
				uielement = (UIElement)children[childIndex];
			}
			hasBringIntoViewContainerBeenMeasured |= (uielement == this._bringIntoViewContainer);
			bool flag = isHorizontal;
			IHierarchicalVirtualizationAndScrollInfo virtualizingChild = VirtualizingStackPanel.GetVirtualizingChild(uielement, ref flag);
			this.SetViewportForChild(isHorizontal, itemStorageProvider, computedAreContainersUniformlySized, computedUniformOrAverageContainerSize, mustDisableVirtualization, uielement, virtualizingChild, item, isBeforeFirstItem, isAfterFirstItem, firstItemInViewportOffset, viewport, cacheSize, cacheUnit, scrollGeneration, stackPixelSize, stackPixelSizeInViewport, stackPixelSizeInCacheBeforeViewport, stackPixelSizeInCacheAfterViewport, stackLogicalSize, stackLogicalSizeInViewport, stackLogicalSizeInCacheBeforeViewport, stackLogicalSizeInCacheAfterViewport, out empty, ref childCacheSize, ref childCacheUnit);
			if (!skipActualMeasure)
			{
				uielement.Measure(childConstraint);
			}
			childDesiredSize = uielement.DesiredSize;
			if (virtualizingChild != null)
			{
				virtualizingChild = VirtualizingStackPanel.GetVirtualizingChild(uielement, ref flag);
				mustDisableVirtualization |= ((virtualizingChild != null && virtualizingChild.MustDisableVirtualization) || flag != isHorizontal);
			}
			Size size;
			Size childPixelSizeInViewport;
			Size childPixelSizeInCacheBeforeViewport;
			Size childPixelSizeInCacheAfterViewport;
			Size size2;
			Size childLogicalSizeInViewport;
			Size childLogicalSizeInCacheBeforeViewport;
			Size childLogicalSizeInCacheAfterViewport;
			if (VirtualizingStackPanel.IsVSP45Compat)
			{
				this.GetSizesForChild(isHorizontal, flag, isBeforeFirstItem, isAfterLastItem, virtualizingChild, childDesiredSize, empty, childCacheSize, childCacheUnit, out size, out childPixelSizeInViewport, out childPixelSizeInCacheBeforeViewport, out childPixelSizeInCacheAfterViewport, out size2, out childLogicalSizeInViewport, out childLogicalSizeInCacheBeforeViewport, out childLogicalSizeInCacheAfterViewport);
			}
			else
			{
				this.GetSizesForChildWithInset(isHorizontal, flag, isBeforeFirstItem, isAfterLastItem, virtualizingChild, childDesiredSize, empty, childCacheSize, childCacheUnit, out size, out childPixelSizeInViewport, out childPixelSizeInCacheBeforeViewport, out childPixelSizeInCacheAfterViewport, out size2, out childLogicalSizeInViewport, out childLogicalSizeInCacheBeforeViewport, out childLogicalSizeInCacheAfterViewport);
			}
			this.UpdateStackSizes(isHorizontal, foundFirstItemInViewport, size, childPixelSizeInViewport, childPixelSizeInCacheBeforeViewport, childPixelSizeInCacheAfterViewport, size2, childLogicalSizeInViewport, childLogicalSizeInCacheBeforeViewport, childLogicalSizeInCacheAfterViewport, ref stackPixelSize, ref stackPixelSizeInViewport, ref stackPixelSizeInCacheBeforeViewport, ref stackPixelSizeInCacheAfterViewport, ref stackLogicalSize, ref stackLogicalSizeInViewport, ref stackLogicalSizeInCacheBeforeViewport, ref stackLogicalSizeInCacheAfterViewport);
			if (virtualizingChild != null)
			{
				hasVirtualizingChildren = true;
			}
			if (VirtualizingPanel.GetShouldCacheContainerSize(uielement))
			{
				if (VirtualizingStackPanel.IsVSP45Compat)
				{
					this.SetContainerSizeForItem(itemStorageProvider, parentItemStorageProvider, parentItem, item, this.IsPixelBased ? size : size2, isHorizontal, ref hasUniformOrAverageContainerSizeBeenSet, ref computedUniformOrAverageContainerSize, ref computedAreContainersUniformlySized);
					return;
				}
				this.SetContainerSizeForItem(itemStorageProvider, parentItemStorageProvider, parentItem, item, this.IsPixelBased ? size : size2, size, isHorizontal, hasVirtualizingChildren, ref hasUniformOrAverageContainerSizeBeenSet, ref computedUniformOrAverageContainerSize, ref computedUniformOrAverageContainerPixelSize, ref computedAreContainersUniformlySized, ref hasAnyContainerSpanChanged);
			}
		}

		// Token: 0x06005AB2 RID: 23218 RVA: 0x00196A7C File Offset: 0x00194C7C
		private void ArrangeFirstItemInExtendedViewport(bool isHorizontal, UIElement child, Size childDesiredSize, double arrangeLength, ref Rect rcChild, ref Size previousChildSize, ref Point previousChildOffset, ref int previousChildItemIndex)
		{
			rcChild.X = 0.0;
			rcChild.Y = 0.0;
			if (this.IsScrolling)
			{
				if (!this.IsPixelBased)
				{
					if (isHorizontal)
					{
						rcChild.X = -1.0 * ((VirtualizingStackPanel.IsVSP45Compat || !this.IsVirtualizing || !this.HasVirtualizingChildren) ? this._previousStackPixelSizeInCacheBeforeViewport.Width : this._pixelDistanceToViewport);
						rcChild.Y = -1.0 * this._scrollData._computedOffset.Y;
					}
					else
					{
						rcChild.Y = -1.0 * ((VirtualizingStackPanel.IsVSP45Compat || !this.IsVirtualizing || !this.HasVirtualizingChildren) ? this._previousStackPixelSizeInCacheBeforeViewport.Height : this._pixelDistanceToViewport);
						rcChild.X = -1.0 * this._scrollData._computedOffset.X;
					}
				}
				else
				{
					rcChild.X = -1.0 * this._scrollData._computedOffset.X;
					rcChild.Y = -1.0 * this._scrollData._computedOffset.Y;
				}
			}
			if (this.IsVirtualizing)
			{
				if (this.IsPixelBased)
				{
					if (isHorizontal)
					{
						rcChild.X += this._firstItemInExtendedViewportOffset;
					}
					else
					{
						rcChild.Y += this._firstItemInExtendedViewportOffset;
					}
				}
				else if (!VirtualizingStackPanel.IsVSP45Compat && (!this.IsScrolling || this.HasVirtualizingChildren))
				{
					if (isHorizontal)
					{
						rcChild.X += this._pixelDistanceToFirstContainerInExtendedViewport;
					}
					else
					{
						rcChild.Y += this._pixelDistanceToFirstContainerInExtendedViewport;
					}
				}
			}
			bool flag = isHorizontal;
			IHierarchicalVirtualizationAndScrollInfo virtualizingChild = VirtualizingStackPanel.GetVirtualizingChild(child, ref flag);
			if (isHorizontal)
			{
				rcChild.Width = childDesiredSize.Width;
				rcChild.Height = Math.Max(arrangeLength, childDesiredSize.Height);
				previousChildSize = childDesiredSize;
				if (!this.IsPixelBased && virtualizingChild != null && VirtualizingStackPanel.IsVSP45Compat)
				{
					HierarchicalVirtualizationItemDesiredSizes itemDesiredSizes = virtualizingChild.ItemDesiredSizes;
					previousChildSize.Width = itemDesiredSizes.PixelSizeInViewport.Width;
					if (flag == isHorizontal)
					{
						previousChildSize.Width += itemDesiredSizes.PixelSizeBeforeViewport.Width + itemDesiredSizes.PixelSizeAfterViewport.Width;
					}
					RelativeHeaderPosition relativeHeaderPosition = RelativeHeaderPosition.Top;
					Size pixelSize = virtualizingChild.HeaderDesiredSizes.PixelSize;
					if (relativeHeaderPosition == RelativeHeaderPosition.Left || relativeHeaderPosition == RelativeHeaderPosition.Right)
					{
						previousChildSize.Width += pixelSize.Width;
					}
					else
					{
						previousChildSize.Width = Math.Max(previousChildSize.Width, pixelSize.Width);
					}
				}
			}
			else
			{
				rcChild.Height = childDesiredSize.Height;
				rcChild.Width = Math.Max(arrangeLength, childDesiredSize.Width);
				previousChildSize = childDesiredSize;
				if (!this.IsPixelBased && virtualizingChild != null && VirtualizingStackPanel.IsVSP45Compat)
				{
					HierarchicalVirtualizationItemDesiredSizes itemDesiredSizes2 = virtualizingChild.ItemDesiredSizes;
					previousChildSize.Height = itemDesiredSizes2.PixelSizeInViewport.Height;
					if (flag == isHorizontal)
					{
						previousChildSize.Height += itemDesiredSizes2.PixelSizeBeforeViewport.Height + itemDesiredSizes2.PixelSizeAfterViewport.Height;
					}
					RelativeHeaderPosition relativeHeaderPosition2 = RelativeHeaderPosition.Top;
					Size pixelSize2 = virtualizingChild.HeaderDesiredSizes.PixelSize;
					if (relativeHeaderPosition2 == RelativeHeaderPosition.Top || relativeHeaderPosition2 == RelativeHeaderPosition.Bottom)
					{
						previousChildSize.Height += pixelSize2.Height;
					}
					else
					{
						previousChildSize.Height = Math.Max(previousChildSize.Height, pixelSize2.Height);
					}
				}
			}
			previousChildItemIndex = this._firstItemInExtendedViewportIndex;
			previousChildOffset = rcChild.Location;
			child.Arrange(rcChild);
		}

		// Token: 0x06005AB3 RID: 23219 RVA: 0x00196E4C File Offset: 0x0019504C
		private void ArrangeOtherItemsInExtendedViewport(bool isHorizontal, UIElement child, Size childDesiredSize, double arrangeLength, int index, ref Rect rcChild, ref Size previousChildSize, ref Point previousChildOffset, ref int previousChildItemIndex)
		{
			if (isHorizontal)
			{
				rcChild.X += previousChildSize.Width;
				rcChild.Width = childDesiredSize.Width;
				rcChild.Height = Math.Max(arrangeLength, childDesiredSize.Height);
			}
			else
			{
				rcChild.Y += previousChildSize.Height;
				rcChild.Height = childDesiredSize.Height;
				rcChild.Width = Math.Max(arrangeLength, childDesiredSize.Width);
			}
			previousChildSize = childDesiredSize;
			previousChildItemIndex = this._firstItemInExtendedViewportIndex + (index - this._firstItemInExtendedViewportChildIndex);
			previousChildOffset = rcChild.Location;
			child.Arrange(rcChild);
		}

		// Token: 0x06005AB4 RID: 23220 RVA: 0x00196F04 File Offset: 0x00195104
		private void ArrangeItemsBeyondTheExtendedViewport(bool isHorizontal, UIElement child, Size childDesiredSize, double arrangeLength, IList items, IItemContainerGenerator generator, IContainItemStorage itemStorageProvider, bool areContainersUniformlySized, double uniformOrAverageContainerSize, bool beforeExtendedViewport, ref Rect rcChild, ref Size previousChildSize, ref Point previousChildOffset, ref int previousChildItemIndex)
		{
			if (isHorizontal)
			{
				rcChild.Width = childDesiredSize.Width;
				rcChild.Height = Math.Max(arrangeLength, childDesiredSize.Height);
				if (this.IsPixelBased)
				{
					int num = ((ItemContainerGenerator)generator).IndexFromContainer(child, true);
					if (beforeExtendedViewport)
					{
						double num2;
						if (previousChildItemIndex == -1)
						{
							this.ComputeDistance(items, itemStorageProvider, isHorizontal, areContainersUniformlySized, uniformOrAverageContainerSize, 0, num, out num2);
						}
						else
						{
							this.ComputeDistance(items, itemStorageProvider, isHorizontal, areContainersUniformlySized, uniformOrAverageContainerSize, num, previousChildItemIndex - num, out num2);
						}
						rcChild.X = previousChildOffset.X - num2;
						rcChild.Y = previousChildOffset.Y;
					}
					else
					{
						double num2;
						if (previousChildItemIndex == -1)
						{
							this.ComputeDistance(items, itemStorageProvider, isHorizontal, areContainersUniformlySized, uniformOrAverageContainerSize, 0, num, out num2);
						}
						else
						{
							this.ComputeDistance(items, itemStorageProvider, isHorizontal, areContainersUniformlySized, uniformOrAverageContainerSize, previousChildItemIndex, num - previousChildItemIndex, out num2);
						}
						rcChild.X = previousChildOffset.X + num2;
						rcChild.Y = previousChildOffset.Y;
					}
					previousChildItemIndex = num;
				}
				else if (beforeExtendedViewport)
				{
					rcChild.X -= childDesiredSize.Width;
				}
				else
				{
					rcChild.X += previousChildSize.Width;
				}
			}
			else
			{
				rcChild.Height = childDesiredSize.Height;
				rcChild.Width = Math.Max(arrangeLength, childDesiredSize.Width);
				if (this.IsPixelBased)
				{
					int num3 = ((ItemContainerGenerator)generator).IndexFromContainer(child, true);
					if (beforeExtendedViewport)
					{
						double num4;
						if (previousChildItemIndex == -1)
						{
							this.ComputeDistance(items, itemStorageProvider, isHorizontal, areContainersUniformlySized, uniformOrAverageContainerSize, 0, num3, out num4);
						}
						else
						{
							this.ComputeDistance(items, itemStorageProvider, isHorizontal, areContainersUniformlySized, uniformOrAverageContainerSize, num3, previousChildItemIndex - num3, out num4);
						}
						rcChild.Y = previousChildOffset.Y - num4;
						rcChild.X = previousChildOffset.X;
					}
					else
					{
						double num4;
						if (previousChildItemIndex == -1)
						{
							this.ComputeDistance(items, itemStorageProvider, isHorizontal, areContainersUniformlySized, uniformOrAverageContainerSize, 0, num3, out num4);
						}
						else
						{
							this.ComputeDistance(items, itemStorageProvider, isHorizontal, areContainersUniformlySized, uniformOrAverageContainerSize, previousChildItemIndex, num3 - previousChildItemIndex, out num4);
						}
						rcChild.Y = previousChildOffset.Y + num4;
						rcChild.X = previousChildOffset.X;
					}
					previousChildItemIndex = num3;
				}
				else if (beforeExtendedViewport)
				{
					rcChild.Y -= childDesiredSize.Height;
				}
				else
				{
					rcChild.Y += previousChildSize.Height;
				}
			}
			previousChildSize = childDesiredSize;
			previousChildOffset = rcChild.Location;
			child.Arrange(rcChild);
		}

		// Token: 0x06005AB5 RID: 23221 RVA: 0x0019717B File Offset: 0x0019537B
		private void InsertNewContainer(int childIndex, UIElement container)
		{
			this.InsertContainer(childIndex, container, false);
		}

		// Token: 0x06005AB6 RID: 23222 RVA: 0x00197187 File Offset: 0x00195387
		private bool InsertRecycledContainer(int childIndex, UIElement container)
		{
			return this.InsertContainer(childIndex, container, true);
		}

		// Token: 0x06005AB7 RID: 23223 RVA: 0x00197194 File Offset: 0x00195394
		private bool InsertContainer(int childIndex, UIElement container, bool isRecycled)
		{
			bool result = false;
			UIElementCollection internalChildren = base.InternalChildren;
			int num;
			if (childIndex > 0)
			{
				num = this.ChildIndexFromRealizedIndex(childIndex - 1);
				num++;
			}
			else
			{
				num = this.ChildIndexFromRealizedIndex(childIndex);
			}
			if (!isRecycled || num >= internalChildren.Count || internalChildren[num] != container)
			{
				if (num < internalChildren.Count)
				{
					int index = num;
					if (isRecycled && container.InternalVisualParent != null)
					{
						internalChildren.MoveVisualChild(container, internalChildren[num]);
						result = true;
					}
					else
					{
						VirtualizingPanel.InsertInternalChild(internalChildren, index, container);
					}
				}
				else if (isRecycled && container.InternalVisualParent != null)
				{
					internalChildren.MoveVisualChild(container, null);
					result = true;
				}
				else
				{
					VirtualizingPanel.AddInternalChild(internalChildren, container);
				}
			}
			if (this.IsVirtualizing && this.InRecyclingMode)
			{
				if (this.ItemsChangedDuringMeasure)
				{
					this._realizedChildren = null;
				}
				if (this._realizedChildren != null)
				{
					this._realizedChildren.Insert(childIndex, container);
				}
				else
				{
					this.EnsureRealizedChildren();
				}
			}
			base.Generator.PrepareItemContainer(container);
			return result;
		}

		// Token: 0x06005AB8 RID: 23224 RVA: 0x00197278 File Offset: 0x00195478
		private void EnsureCleanupOperation(bool delay)
		{
			if (delay)
			{
				bool flag = true;
				if (this._cleanupOperation != null)
				{
					flag = this._cleanupOperation.Abort();
					if (flag)
					{
						this._cleanupOperation = null;
					}
				}
				if (flag && this._cleanupDelay == null)
				{
					this._cleanupDelay = new DispatcherTimer();
					this._cleanupDelay.Tick += this.OnDelayCleanup;
					this._cleanupDelay.Interval = TimeSpan.FromMilliseconds(500.0);
					this._cleanupDelay.Start();
					return;
				}
			}
			else if (this._cleanupOperation == null && this._cleanupDelay == null)
			{
				this._cleanupOperation = base.Dispatcher.BeginInvoke(DispatcherPriority.Background, new DispatcherOperationCallback(this.OnCleanUp), null);
			}
		}

		// Token: 0x06005AB9 RID: 23225 RVA: 0x0019732C File Offset: 0x0019552C
		private bool PreviousChildIsGenerated(int childIndex)
		{
			GeneratorPosition position = new GeneratorPosition(childIndex, 0);
			position = base.Generator.GeneratorPositionFromIndex(base.Generator.IndexFromGeneratorPosition(position) - 1);
			return position.Offset == 0 && position.Index >= 0;
		}

		// Token: 0x06005ABA RID: 23226 RVA: 0x00197374 File Offset: 0x00195574
		private bool AddContainerFromGenerator(int childIndex, UIElement child, bool newlyRealized, bool isBeforeViewport)
		{
			bool result = false;
			if (!newlyRealized)
			{
				if (this.InRecyclingMode)
				{
					IList realizedChildren = this.RealizedChildren;
					if (childIndex < 0 || childIndex >= realizedChildren.Count || realizedChildren[childIndex] != child)
					{
						result = this.InsertRecycledContainer(childIndex, child);
					}
				}
			}
			else
			{
				this.InsertNewContainer(childIndex, child);
			}
			return result;
		}

		// Token: 0x06005ABB RID: 23227 RVA: 0x001973C0 File Offset: 0x001955C0
		private void OnItemsRemove(ItemsChangedEventArgs args)
		{
			this.RemoveChildRange(args.Position, args.ItemCount, args.ItemUICount);
		}

		// Token: 0x06005ABC RID: 23228 RVA: 0x001973DC File Offset: 0x001955DC
		private void OnItemsReplace(ItemsChangedEventArgs args)
		{
			if (args.ItemUICount > 0)
			{
				UIElementCollection internalChildren = base.InternalChildren;
				using (base.Generator.StartAt(args.Position, GeneratorDirection.Forward, true))
				{
					for (int i = 0; i < args.ItemUICount; i++)
					{
						int index = args.Position.Index + i;
						bool flag;
						UIElement uielement = base.Generator.GenerateNext(out flag) as UIElement;
						internalChildren.SetInternal(index, uielement);
						base.Generator.PrepareItemContainer(uielement);
					}
				}
			}
		}

		// Token: 0x06005ABD RID: 23229 RVA: 0x00197478 File Offset: 0x00195678
		private void OnItemsMove(ItemsChangedEventArgs args)
		{
			this.RemoveChildRange(args.OldPosition, args.ItemCount, args.ItemUICount);
		}

		// Token: 0x06005ABE RID: 23230 RVA: 0x00197494 File Offset: 0x00195694
		private void RemoveChildRange(GeneratorPosition position, int itemCount, int itemUICount)
		{
			if (base.IsItemsHost)
			{
				UIElementCollection internalChildren = base.InternalChildren;
				int num = position.Index;
				if (position.Offset > 0)
				{
					num++;
				}
				if (num < internalChildren.Count && itemUICount > 0)
				{
					VirtualizingPanel.RemoveInternalChildRange(internalChildren, num, itemUICount);
					if (this.IsVirtualizing && this.InRecyclingMode)
					{
						this._realizedChildren.RemoveRange(num, itemUICount);
					}
				}
			}
		}

		// Token: 0x06005ABF RID: 23231 RVA: 0x001974FA File Offset: 0x001956FA
		private void CleanupContainers(int firstItemInExtendedViewportIndex, int itemsInExtendedViewportCount, ItemsControl itemsControl)
		{
			this.CleanupContainers(firstItemInExtendedViewportIndex, itemsInExtendedViewportCount, itemsControl, false, 0);
		}

		// Token: 0x06005AC0 RID: 23232 RVA: 0x00197508 File Offset: 0x00195708
		private bool CleanupContainers(int firstItemInExtendedViewportIndex, int itemsInExtendedViewportCount, ItemsControl itemsControl, bool timeBound, int startTickCount)
		{
			IList realizedChildren = this.RealizedChildren;
			if (realizedChildren.Count == 0)
			{
				return false;
			}
			int num = -1;
			int num2 = 0;
			int num3 = -1;
			bool flag = false;
			bool isVirtualizing = this.IsVirtualizing;
			bool result = false;
			for (int i = 0; i < realizedChildren.Count; i++)
			{
				if (timeBound)
				{
					int num4 = Environment.TickCount - startTickCount;
					if (num4 > 50 && num2 > 0)
					{
						result = true;
						break;
					}
				}
				UIElement uielement = (UIElement)realizedChildren[i];
				int num5 = num3;
				num3 = this.GetGeneratedIndex(i);
				object item = itemsControl.ItemContainerGenerator.ItemFromContainer(uielement);
				if (num3 - num5 != 1)
				{
					flag = true;
				}
				if (flag)
				{
					if (num >= 0 && num2 > 0)
					{
						this.CleanupRange(realizedChildren, base.Generator, num, num2);
						i -= num2;
						num2 = 0;
						num = -1;
					}
					flag = false;
				}
				if ((num3 < firstItemInExtendedViewportIndex || num3 >= firstItemInExtendedViewportIndex + itemsInExtendedViewportCount) && num3 >= 0 && !((IGeneratorHost)itemsControl).IsItemItsOwnContainer(item) && !uielement.IsKeyboardFocusWithin && uielement != this._bringIntoViewContainer && this.NotifyCleanupItem(uielement, itemsControl) && VirtualizingPanel.GetIsContainerVirtualizable(uielement))
				{
					if (num == -1)
					{
						num = i;
					}
					num2++;
				}
				else
				{
					flag = true;
				}
			}
			if (num >= 0 && num2 > 0)
			{
				this.CleanupRange(realizedChildren, base.Generator, num, num2);
			}
			return result;
		}

		// Token: 0x06005AC1 RID: 23233 RVA: 0x0019763C File Offset: 0x0019583C
		private void EnsureRealizedChildren()
		{
			if (this._realizedChildren == null)
			{
				UIElementCollection internalChildren = base.InternalChildren;
				this._realizedChildren = new List<UIElement>(internalChildren.Count);
				for (int i = 0; i < internalChildren.Count; i++)
				{
					this._realizedChildren.Add(internalChildren[i]);
				}
			}
		}

		// Token: 0x06005AC2 RID: 23234 RVA: 0x0019768C File Offset: 0x0019588C
		[Conditional("DEBUG")]
		private void debug_VerifyRealizedChildren()
		{
			ItemContainerGenerator itemContainerGenerator = base.Generator as ItemContainerGenerator;
			ItemsControl itemsOwner = ItemsControl.GetItemsOwner(this);
			if (itemContainerGenerator != null && itemsOwner != null && !itemsOwner.IsGrouping)
			{
				foreach (object obj in base.InternalChildren)
				{
					UIElement container = (UIElement)obj;
					int num = itemContainerGenerator.IndexFromContainer(container);
					if (num != -1)
					{
						GeneratorPosition generatorPosition = base.Generator.GeneratorPositionFromIndex(num);
					}
				}
			}
		}

		// Token: 0x06005AC3 RID: 23235 RVA: 0x00197720 File Offset: 0x00195920
		[Conditional("DEBUG")]
		private void debug_AssertRealizedChildrenEqualVisualChildren()
		{
			if (this.IsVirtualizing && this.InRecyclingMode)
			{
				UIElementCollection internalChildren = base.InternalChildren;
				for (int i = 0; i < internalChildren.Count; i++)
				{
				}
			}
		}

		// Token: 0x06005AC4 RID: 23236 RVA: 0x00197758 File Offset: 0x00195958
		private int ChildIndexFromRealizedIndex(int realizedChildIndex)
		{
			if (this.IsVirtualizing && this.InRecyclingMode && realizedChildIndex < this._realizedChildren.Count)
			{
				UIElement uielement = this._realizedChildren[realizedChildIndex];
				UIElementCollection internalChildren = base.InternalChildren;
				for (int i = realizedChildIndex; i < internalChildren.Count; i++)
				{
					if (internalChildren[i] == uielement)
					{
						return i;
					}
				}
			}
			return realizedChildIndex;
		}

		// Token: 0x06005AC5 RID: 23237 RVA: 0x001977B8 File Offset: 0x001959B8
		private void DisconnectRecycledContainers()
		{
			int num = 0;
			UIElement uielement = (this._realizedChildren.Count > 0) ? this._realizedChildren[0] : null;
			UIElementCollection internalChildren = base.InternalChildren;
			for (int i = 0; i < internalChildren.Count; i++)
			{
				UIElement uielement2 = internalChildren[i];
				if (uielement2 == uielement)
				{
					num++;
					if (num < this._realizedChildren.Count)
					{
						uielement = this._realizedChildren[num];
					}
					else
					{
						uielement = null;
					}
				}
				else
				{
					internalChildren.RemoveNoVerify(uielement2);
					i--;
				}
			}
		}

		// Token: 0x06005AC6 RID: 23238 RVA: 0x00197840 File Offset: 0x00195A40
		private GeneratorPosition IndexToGeneratorPositionForStart(int index, out int childIndex)
		{
			IItemContainerGenerator generator = base.Generator;
			GeneratorPosition result = (generator != null) ? generator.GeneratorPositionFromIndex(index) : new GeneratorPosition(-1, index + 1);
			childIndex = ((result.Offset == 0) ? result.Index : (result.Index + 1));
			return result;
		}

		// Token: 0x06005AC7 RID: 23239 RVA: 0x00197888 File Offset: 0x00195A88
		private void OnDelayCleanup(object sender, EventArgs e)
		{
			bool flag = false;
			try
			{
				flag = this.CleanUp();
			}
			finally
			{
				if (!flag)
				{
					this._cleanupDelay.Stop();
					this._cleanupDelay = null;
				}
			}
		}

		// Token: 0x06005AC8 RID: 23240 RVA: 0x001978C8 File Offset: 0x00195AC8
		private object OnCleanUp(object args)
		{
			bool flag = false;
			try
			{
				flag = this.CleanUp();
			}
			finally
			{
				this._cleanupOperation = null;
			}
			if (flag)
			{
				this.EnsureCleanupOperation(true);
			}
			return null;
		}

		// Token: 0x06005AC9 RID: 23241 RVA: 0x00197904 File Offset: 0x00195B04
		private bool CleanUp()
		{
			ItemsControl itemsControl = null;
			ItemsControl.GetItemsOwnerInternal(this, out itemsControl);
			if (itemsControl == null || !this.IsVirtualizing || !base.IsItemsHost)
			{
				return false;
			}
			if (!VirtualizingStackPanel.IsVSP45Compat && this.IsMeasureCachesPending)
			{
				return true;
			}
			int tickCount = Environment.TickCount;
			bool result = false;
			UIElementCollection internalChildren = base.InternalChildren;
			int minDesiredGenerated = this.MinDesiredGenerated;
			int maxDesiredGenerated = this.MaxDesiredGenerated;
			int num = maxDesiredGenerated - minDesiredGenerated;
			int num2 = internalChildren.Count - num;
			if (this.HasVirtualizingChildren || num2 > num * 2)
			{
				result = ((Mouse.LeftButton == MouseButtonState.Pressed && num2 < 1000) || this.CleanupContainers(this._firstItemInExtendedViewportIndex, this._actualItemsInExtendedViewportCount, itemsControl, true, tickCount));
			}
			return result;
		}

		// Token: 0x06005ACA RID: 23242 RVA: 0x001979AF File Offset: 0x00195BAF
		private bool NotifyCleanupItem(int childIndex, UIElementCollection children, ItemsControl itemsControl)
		{
			return this.NotifyCleanupItem(children[childIndex], itemsControl);
		}

		// Token: 0x06005ACB RID: 23243 RVA: 0x001979C0 File Offset: 0x00195BC0
		private bool NotifyCleanupItem(UIElement child, ItemsControl itemsControl)
		{
			CleanUpVirtualizedItemEventArgs cleanUpVirtualizedItemEventArgs = new CleanUpVirtualizedItemEventArgs(itemsControl.ItemContainerGenerator.ItemFromContainer(child), child);
			cleanUpVirtualizedItemEventArgs.Source = this;
			this.OnCleanUpVirtualizedItem(cleanUpVirtualizedItemEventArgs);
			return !cleanUpVirtualizedItemEventArgs.Cancel;
		}

		// Token: 0x06005ACC RID: 23244 RVA: 0x001979F8 File Offset: 0x00195BF8
		private void CleanupRange(IList children, IItemContainerGenerator generator, int startIndex, int count)
		{
			if (this.InRecyclingMode)
			{
				if (VirtualizingStackPanel.ScrollTracer.IsEnabled && VirtualizingStackPanel.ScrollTracer.IsTracing(this))
				{
					List<string> list = new List<string>(count);
					for (int i = 0; i < count; i++)
					{
						list.Add(this.ContainerPath((DependencyObject)children[startIndex + i]));
					}
					VirtualizingStackPanel.ScrollTracer.Trace(this, VirtualizingStackPanel.ScrollTraceOp.RecycleChildren, new object[]
					{
						startIndex,
						count,
						list
					});
				}
				((IRecyclingItemContainerGenerator)generator).Recycle(new GeneratorPosition(startIndex, 0), count);
				this._realizedChildren.RemoveRange(startIndex, count);
				return;
			}
			if (VirtualizingStackPanel.ScrollTracer.IsEnabled && VirtualizingStackPanel.ScrollTracer.IsTracing(this))
			{
				List<string> list2 = new List<string>(count);
				for (int j = 0; j < count; j++)
				{
					list2.Add(this.ContainerPath((DependencyObject)children[startIndex + j]));
				}
				VirtualizingStackPanel.ScrollTracer.Trace(this, VirtualizingStackPanel.ScrollTraceOp.RemoveChildren, new object[]
				{
					startIndex,
					count,
					list2
				});
			}
			VirtualizingPanel.RemoveInternalChildRange((UIElementCollection)children, startIndex, count);
			generator.Remove(new GeneratorPosition(startIndex, 0), count);
			this.AdjustFirstVisibleChildIndex(startIndex, count);
		}

		// Token: 0x06005ACD RID: 23245 RVA: 0x00197B20 File Offset: 0x00195D20
		private void AdjustFirstVisibleChildIndex(int startIndex, int count)
		{
			if (startIndex < this._firstItemInExtendedViewportChildIndex)
			{
				int num = startIndex + count - 1;
				if (num < this._firstItemInExtendedViewportChildIndex)
				{
					this._firstItemInExtendedViewportChildIndex -= count;
					return;
				}
				this._firstItemInExtendedViewportChildIndex = startIndex;
			}
		}

		// Token: 0x170015F2 RID: 5618
		// (get) Token: 0x06005ACE RID: 23246 RVA: 0x00197B5B File Offset: 0x00195D5B
		private int MinDesiredGenerated
		{
			get
			{
				return Math.Max(0, this._firstItemInExtendedViewportIndex);
			}
		}

		// Token: 0x170015F3 RID: 5619
		// (get) Token: 0x06005ACF RID: 23247 RVA: 0x00197B69 File Offset: 0x00195D69
		private int MaxDesiredGenerated
		{
			get
			{
				return Math.Min(this.ItemCount, this._firstItemInExtendedViewportIndex + this._actualItemsInExtendedViewportCount);
			}
		}

		// Token: 0x170015F4 RID: 5620
		// (get) Token: 0x06005AD0 RID: 23248 RVA: 0x00197B83 File Offset: 0x00195D83
		private int ItemCount
		{
			get
			{
				base.EnsureGenerator();
				return ((ItemContainerGenerator)base.Generator).ItemsInternal.Count;
			}
		}

		// Token: 0x06005AD1 RID: 23249 RVA: 0x00197BA0 File Offset: 0x00195DA0
		private void EnsureScrollData()
		{
			if (this._scrollData == null)
			{
				this._scrollData = new VirtualizingStackPanel.ScrollData();
			}
		}

		// Token: 0x06005AD2 RID: 23250 RVA: 0x00197BB5 File Offset: 0x00195DB5
		private static void ResetScrolling(VirtualizingStackPanel element)
		{
			element.InvalidateMeasure();
			if (element.IsScrolling)
			{
				element._scrollData.ClearLayout();
			}
		}

		// Token: 0x06005AD3 RID: 23251 RVA: 0x00197BD0 File Offset: 0x00195DD0
		private void OnScrollChange()
		{
			if (this.ScrollOwner != null)
			{
				this.ScrollOwner.InvalidateScrollInfo();
			}
		}

		// Token: 0x06005AD4 RID: 23252 RVA: 0x00197BE8 File Offset: 0x00195DE8
		private void SetAndVerifyScrollingData(bool isHorizontal, Rect viewport, Size constraint, UIElement firstContainerInViewport, double firstContainerOffsetFromViewport, bool hasAverageContainerSizeChanged, double newOffset, ref Size stackPixelSize, ref Size stackLogicalSize, ref Size stackPixelSizeInViewport, ref Size stackLogicalSizeInViewport, ref Size stackPixelSizeInCacheBeforeViewport, ref Size stackLogicalSizeInCacheBeforeViewport, ref bool remeasure, ref double? lastPageSafeOffset, ref double? lastPagePixelSize, ref List<double> previouslyMeasuredOffsets)
		{
			Vector vector = new Vector(viewport.Location.X, viewport.Location.Y);
			Vector offset = this._scrollData._offset;
			Size size;
			Size size2;
			if (this.IsPixelBased)
			{
				size = stackPixelSize;
				size2 = viewport.Size;
			}
			else
			{
				size = stackLogicalSize;
				size2 = stackLogicalSizeInViewport;
				if (isHorizontal)
				{
					if (DoubleUtil.GreaterThan(stackPixelSizeInViewport.Width, constraint.Width) && size2.Width > 1.0)
					{
						double num = size2.Width;
						size2.Width = num - 1.0;
					}
					size2.Height = viewport.Height;
				}
				else
				{
					if (DoubleUtil.GreaterThan(stackPixelSizeInViewport.Height, constraint.Height) && size2.Height > 1.0)
					{
						double num = size2.Height;
						size2.Height = num - 1.0;
					}
					size2.Width = viewport.Width;
				}
			}
			if (isHorizontal)
			{
				if (this.MeasureCaches && this.IsVirtualizing)
				{
					stackPixelSize.Height = this._scrollData._extent.Height;
				}
				this._scrollData._maxDesiredSize.Height = Math.Max(this._scrollData._maxDesiredSize.Height, stackPixelSize.Height);
				stackPixelSize.Height = this._scrollData._maxDesiredSize.Height;
				size.Height = stackPixelSize.Height;
				if (double.IsPositiveInfinity(constraint.Height))
				{
					size2.Height = stackPixelSize.Height;
				}
			}
			else
			{
				if (this.MeasureCaches && this.IsVirtualizing)
				{
					stackPixelSize.Width = this._scrollData._extent.Width;
				}
				this._scrollData._maxDesiredSize.Width = Math.Max(this._scrollData._maxDesiredSize.Width, stackPixelSize.Width);
				stackPixelSize.Width = this._scrollData._maxDesiredSize.Width;
				size.Width = stackPixelSize.Width;
				if (double.IsPositiveInfinity(constraint.Width))
				{
					size2.Width = stackPixelSize.Width;
				}
			}
			if (!double.IsPositiveInfinity(constraint.Width))
			{
				stackPixelSize.Width = ((this.IsPixelBased || DoubleUtil.AreClose(vector.X, 0.0)) ? Math.Min(stackPixelSize.Width, constraint.Width) : constraint.Width);
			}
			if (!double.IsPositiveInfinity(constraint.Height))
			{
				stackPixelSize.Height = ((this.IsPixelBased || DoubleUtil.AreClose(vector.Y, 0.0)) ? Math.Min(stackPixelSize.Height, constraint.Height) : constraint.Height);
			}
			if (VirtualizingStackPanel.ScrollTracer.IsEnabled && VirtualizingStackPanel.ScrollTracer.IsTracing(this))
			{
				VirtualizingStackPanel.ScrollTracer.Trace(this, VirtualizingStackPanel.ScrollTraceOp.SVSDBegin, new object[]
				{
					"isa:",
					this.IsScrollActive,
					"mc:",
					this.MeasureCaches,
					"o:",
					this._scrollData._offset,
					"co:",
					vector,
					"ex:",
					size,
					"vs:",
					size2,
					"pxInV:",
					stackPixelSizeInViewport
				});
				if (hasAverageContainerSizeChanged)
				{
					VirtualizingStackPanel.ScrollTracer.Trace(this, VirtualizingStackPanel.ScrollTraceOp.SVSDBegin, new object[]
					{
						"acs:",
						this.UniformOrAverageContainerSize,
						this.UniformOrAverageContainerPixelSize
					});
				}
			}
			bool flag = isHorizontal ? (!DoubleUtil.AreClose(vector.X, this._scrollData._offset.X) || (this.IsScrollActive && vector.X > 0.0 && DoubleUtil.GreaterThanOrClose(vector.X, this._scrollData.Extent.Width - this._scrollData.Viewport.Width))) : (!DoubleUtil.AreClose(vector.Y, this._scrollData._offset.Y) || (this.IsScrollActive && vector.Y > 0.0 && DoubleUtil.GreaterThanOrClose(vector.Y, this._scrollData.Extent.Height - this._scrollData.Viewport.Height)));
			bool flag2 = isHorizontal ? (!DoubleUtil.AreClose(vector.Y, this._scrollData._offset.Y) || (this.IsScrollActive && vector.Y > 0.0 && DoubleUtil.GreaterThanOrClose(vector.Y, this._scrollData.Extent.Height - this._scrollData.Viewport.Height))) : (!DoubleUtil.AreClose(vector.X, this._scrollData._offset.X) || (this.IsScrollActive && vector.X > 0.0 && DoubleUtil.GreaterThanOrClose(vector.X, this._scrollData.Extent.Width - this._scrollData.Viewport.Width)));
			bool flag3 = false;
			if (hasAverageContainerSizeChanged && newOffset >= 0.0)
			{
				if (VirtualizingStackPanel.ScrollTracer.IsEnabled && VirtualizingStackPanel.ScrollTracer.IsTracing(this))
				{
					VirtualizingStackPanel.ScrollTracer.Trace(this, VirtualizingStackPanel.ScrollTraceOp.AdjustOffset, new object[]
					{
						newOffset,
						vector
					});
				}
				if (isHorizontal)
				{
					if (!LayoutDoubleUtil.AreClose(vector.X, newOffset))
					{
						double num2 = newOffset - vector.X;
						vector.X = newOffset;
						offset.X = newOffset;
						this._viewport.X = newOffset;
						this._extendedViewport.X = this._extendedViewport.X + num2;
						flag3 = true;
						if (DoubleUtil.GreaterThan(newOffset + size2.Width, size.Width))
						{
							flag = true;
							this.IsScrollActive = true;
							this._scrollData.HorizontalScrollType = VirtualizingStackPanel.ScrollType.ToEnd;
						}
					}
				}
				else if (!LayoutDoubleUtil.AreClose(vector.Y, newOffset))
				{
					double num3 = newOffset - vector.Y;
					vector.Y = newOffset;
					offset.Y = newOffset;
					this._viewport.Y = newOffset;
					this._extendedViewport.Y = this._extendedViewport.Y + num3;
					if (DoubleUtil.GreaterThan(newOffset + size2.Height, size.Height))
					{
						flag = true;
						this.IsScrollActive = true;
						this._scrollData.VerticalScrollType = VirtualizingStackPanel.ScrollType.ToEnd;
					}
				}
			}
			if (lastPagePixelSize != null && lastPageSafeOffset == null && !DoubleUtil.AreClose(isHorizontal ? stackPixelSizeInViewport.Width : stackPixelSizeInViewport.Height, lastPagePixelSize.Value))
			{
				flag3 = true;
				flag = true;
				if (VirtualizingStackPanel.ScrollTracer.IsEnabled && VirtualizingStackPanel.ScrollTracer.IsTracing(this))
				{
					VirtualizingStackPanel.ScrollTracer.Trace(this, VirtualizingStackPanel.ScrollTraceOp.LastPageSizeChange, new object[]
					{
						vector,
						stackPixelSizeInViewport,
						lastPagePixelSize
					});
				}
			}
			if (flag3)
			{
				if (previouslyMeasuredOffsets != null)
				{
					previouslyMeasuredOffsets.Clear();
				}
				lastPageSafeOffset = null;
				lastPagePixelSize = null;
			}
			bool flag4 = !DoubleUtil.AreClose(size2, this._scrollData._viewport);
			bool flag5 = !DoubleUtil.AreClose(size, this._scrollData._extent);
			bool flag6 = !DoubleUtil.AreClose(vector, this._scrollData._computedOffset);
			bool flag7;
			bool flag8;
			if (flag5)
			{
				flag7 = !DoubleUtil.AreClose(size.Width, this._scrollData._extent.Width);
				flag8 = !DoubleUtil.AreClose(size.Height, this._scrollData._extent.Height);
			}
			else
			{
				flag8 = (flag7 = false);
			}
			Vector vector2 = vector;
			bool flag9 = false;
			ScrollViewer scrollOwner = this.ScrollOwner;
			if (scrollOwner.InChildMeasurePass1 || scrollOwner.InChildMeasurePass2)
			{
				ScrollBarVisibility verticalScrollBarVisibility = scrollOwner.VerticalScrollBarVisibility;
				bool flag10 = verticalScrollBarVisibility == ScrollBarVisibility.Auto;
				if (flag10)
				{
					Visibility computedVerticalScrollBarVisibility = scrollOwner.ComputedVerticalScrollBarVisibility;
					Visibility visibility = DoubleUtil.LessThanOrClose(size.Height, size2.Height) ? Visibility.Collapsed : Visibility.Visible;
					if (computedVerticalScrollBarVisibility != visibility)
					{
						vector2 = offset;
						flag9 = true;
					}
				}
				if (!flag9)
				{
					ScrollBarVisibility horizontalScrollBarVisibility = scrollOwner.HorizontalScrollBarVisibility;
					bool flag11 = horizontalScrollBarVisibility == ScrollBarVisibility.Auto;
					if (flag11)
					{
						Visibility computedHorizontalScrollBarVisibility = scrollOwner.ComputedHorizontalScrollBarVisibility;
						Visibility visibility2 = DoubleUtil.LessThanOrClose(size.Width, size2.Width) ? Visibility.Collapsed : Visibility.Visible;
						if (computedHorizontalScrollBarVisibility != visibility2)
						{
							vector2 = offset;
							flag9 = true;
						}
					}
				}
				if (flag9 && VirtualizingStackPanel.ScrollTracer.IsEnabled && VirtualizingStackPanel.ScrollTracer.IsTracing(this))
				{
					VirtualizingStackPanel.ScrollTracer.Trace(this, VirtualizingStackPanel.ScrollTraceOp.ScrollBarChangeVisibility, new object[]
					{
						vector2
					});
				}
			}
			if (isHorizontal)
			{
				if (!flag9)
				{
					if (this.WasOffsetPreviouslyMeasured(previouslyMeasuredOffsets, vector.X))
					{
						if (!this.IsPixelBased && lastPageSafeOffset != null && !DoubleUtil.AreClose(lastPageSafeOffset.Value, vector.X))
						{
							if (VirtualizingStackPanel.ScrollTracer.IsEnabled && VirtualizingStackPanel.ScrollTracer.IsTracing(this))
							{
								VirtualizingStackPanel.ScrollTracer.Trace(this, VirtualizingStackPanel.ScrollTraceOp.RemeasureCycle, new object[]
								{
									vector2.X,
									lastPageSafeOffset
								});
							}
							vector2.X = lastPageSafeOffset.Value;
							lastPageSafeOffset = null;
							remeasure = true;
						}
					}
					else if (!remeasure)
					{
						if (!this.IsPixelBased)
						{
							if (!remeasure && !this.IsEndOfViewport(isHorizontal, viewport, stackPixelSizeInViewport) && DoubleUtil.GreaterThan(stackLogicalSize.Width, stackLogicalSizeInViewport.Width))
							{
								if (lastPageSafeOffset == null || vector.X < lastPageSafeOffset.Value)
								{
									lastPageSafeOffset = new double?(vector.X);
									lastPagePixelSize = new double?(stackPixelSizeInViewport.Width);
								}
								double num4 = stackPixelSizeInViewport.Width / stackLogicalSizeInViewport.Width;
								double num5 = Math.Floor(viewport.Width / num4);
								if (DoubleUtil.GreaterThan(num5, size2.Width))
								{
									if (VirtualizingStackPanel.ScrollTracer.IsEnabled && VirtualizingStackPanel.ScrollTracer.IsTracing(this))
									{
										VirtualizingStackPanel.ScrollTracer.Trace(this, VirtualizingStackPanel.ScrollTraceOp.RemeasureEndExpandViewport, new object[]
										{
											"off:",
											vector.X,
											lastPageSafeOffset,
											"pxSz:",
											stackPixelSizeInViewport.Width,
											viewport.Width,
											"itSz:",
											stackLogicalSizeInViewport.Width,
											size2.Width,
											"newVpSz:",
											num5
										});
									}
									vector2.X = double.PositiveInfinity;
									size2.Width = num5;
									remeasure = true;
									this.StorePreviouslyMeasuredOffset(ref previouslyMeasuredOffsets, vector.X);
								}
							}
							if (!remeasure && flag && flag4 && !DoubleUtil.AreClose(this._scrollData._viewport.Width, size2.Width))
							{
								if (VirtualizingStackPanel.ScrollTracer.IsEnabled && VirtualizingStackPanel.ScrollTracer.IsTracing(this))
								{
									VirtualizingStackPanel.ScrollTracer.Trace(this, VirtualizingStackPanel.ScrollTraceOp.RemeasureEndChangeOffset, new object[]
									{
										"off:",
										vector.X,
										"vpSz:",
										this._scrollData._viewport.Width,
										size2.Width,
										"newOff:",
										this._scrollData._offset
									});
								}
								remeasure = true;
								vector2.X = double.PositiveInfinity;
								this.StorePreviouslyMeasuredOffset(ref previouslyMeasuredOffsets, vector.X);
								if (DoubleUtil.AreClose(size2.Width, 0.0))
								{
									size2.Width = this._scrollData._viewport.Width;
								}
							}
						}
						if (!remeasure && flag7)
						{
							if (this._scrollData.HorizontalScrollType == VirtualizingStackPanel.ScrollType.ToEnd || (DoubleUtil.GreaterThan(vector.X, 0.0) && DoubleUtil.GreaterThan(vector.X, size.Width - size2.Width)))
							{
								if (VirtualizingStackPanel.ScrollTracer.IsEnabled && VirtualizingStackPanel.ScrollTracer.IsTracing(this))
								{
									VirtualizingStackPanel.ScrollTracer.Trace(this, VirtualizingStackPanel.ScrollTraceOp.RemeasureEndExtentChanged, new object[]
									{
										"off:",
										vector.X,
										"ext:",
										this._scrollData._extent.Width,
										size.Width,
										"vpSz:",
										size2.Width
									});
								}
								remeasure = true;
								vector2.X = double.PositiveInfinity;
								this._scrollData.HorizontalScrollType = VirtualizingStackPanel.ScrollType.ToEnd;
								this.StorePreviouslyMeasuredOffset(ref previouslyMeasuredOffsets, vector.X);
							}
							else if (this._scrollData.HorizontalScrollType == VirtualizingStackPanel.ScrollType.Absolute && !DoubleUtil.AreClose(this._scrollData._extent.Width, 0.0) && !DoubleUtil.AreClose(size.Width, 0.0))
							{
								if (this.IsPixelBased)
								{
									if (!LayoutDoubleUtil.AreClose(vector.X / size.Width, this._scrollData._offset.X / this._scrollData._extent.Width))
									{
										remeasure = true;
										vector2.X = size.Width * this._scrollData._offset.X / this._scrollData._extent.Width;
										this.StorePreviouslyMeasuredOffset(ref previouslyMeasuredOffsets, vector.X);
									}
								}
								else if (!LayoutDoubleUtil.AreClose(Math.Floor(vector.X) / size.Width, Math.Floor(this._scrollData._offset.X) / this._scrollData._extent.Width))
								{
									remeasure = true;
									vector2.X = Math.Floor(size.Width * Math.Floor(this._scrollData._offset.X) / this._scrollData._extent.Width);
									this.StorePreviouslyMeasuredOffset(ref previouslyMeasuredOffsets, vector.X);
								}
								if ((VirtualizingStackPanel.ScrollTracer.IsEnabled && VirtualizingStackPanel.ScrollTracer.IsTracing(this)) & remeasure)
								{
									VirtualizingStackPanel.ScrollTracer.Trace(this, VirtualizingStackPanel.ScrollTraceOp.RemeasureRatio, new object[]
									{
										"expRat:",
										this._scrollData._offset.X,
										this._scrollData._extent.Width,
										this._scrollData._offset.X / this._scrollData._extent.Width,
										"actRat:",
										vector.X,
										size.Width,
										vector.X / size.Width,
										"newOff:",
										vector2.X
									});
								}
							}
						}
						if (!remeasure && flag8)
						{
							if (this._scrollData.VerticalScrollType == VirtualizingStackPanel.ScrollType.ToEnd || (DoubleUtil.GreaterThan(vector.Y, 0.0) && DoubleUtil.GreaterThan(vector.Y, size.Height - size2.Height)))
							{
								if (VirtualizingStackPanel.ScrollTracer.IsEnabled && VirtualizingStackPanel.ScrollTracer.IsTracing(this))
								{
									VirtualizingStackPanel.ScrollTracer.Trace(this, VirtualizingStackPanel.ScrollTraceOp.RemeasureEndExtentChanged, new object[]
									{
										"perp",
										"off:",
										vector.Y,
										"ext:",
										this._scrollData._extent.Height,
										size.Height,
										"vpSz:",
										size2.Height
									});
								}
								remeasure = true;
								vector2.Y = double.PositiveInfinity;
								this._scrollData.VerticalScrollType = VirtualizingStackPanel.ScrollType.ToEnd;
							}
							else if (this._scrollData.VerticalScrollType == VirtualizingStackPanel.ScrollType.Absolute && !DoubleUtil.AreClose(this._scrollData._extent.Height, 0.0) && !DoubleUtil.AreClose(size.Height, 0.0))
							{
								if (!LayoutDoubleUtil.AreClose(vector.Y / size.Height, this._scrollData._offset.Y / this._scrollData._extent.Height))
								{
									remeasure = true;
									vector2.Y = size.Height * this._scrollData._offset.Y / this._scrollData._extent.Height;
								}
								if ((VirtualizingStackPanel.ScrollTracer.IsEnabled && VirtualizingStackPanel.ScrollTracer.IsTracing(this)) & remeasure)
								{
									VirtualizingStackPanel.ScrollTracer.Trace(this, VirtualizingStackPanel.ScrollTraceOp.RemeasureRatio, new object[]
									{
										"perp",
										"expRat:",
										this._scrollData._offset.Y,
										this._scrollData._extent.Height,
										this._scrollData._offset.Y / this._scrollData._extent.Height,
										"actRat:",
										vector.Y,
										size.Height,
										vector.Y / size.Height,
										"newOff:",
										vector2.Y
									});
								}
							}
						}
					}
				}
			}
			else if (!flag9)
			{
				if (this.WasOffsetPreviouslyMeasured(previouslyMeasuredOffsets, vector.Y))
				{
					if (!this.IsPixelBased && lastPageSafeOffset != null && !DoubleUtil.AreClose(lastPageSafeOffset.Value, vector.Y))
					{
						if (VirtualizingStackPanel.ScrollTracer.IsEnabled && VirtualizingStackPanel.ScrollTracer.IsTracing(this))
						{
							VirtualizingStackPanel.ScrollTracer.Trace(this, VirtualizingStackPanel.ScrollTraceOp.RemeasureCycle, new object[]
							{
								vector2.Y,
								lastPageSafeOffset
							});
						}
						vector2.Y = lastPageSafeOffset.Value;
						lastPageSafeOffset = null;
						remeasure = true;
					}
				}
				else if (!remeasure)
				{
					if (!this.IsPixelBased)
					{
						if (!remeasure && !this.IsEndOfViewport(isHorizontal, viewport, stackPixelSizeInViewport) && DoubleUtil.GreaterThan(stackLogicalSize.Height, stackLogicalSizeInViewport.Height))
						{
							if (lastPageSafeOffset == null || vector.Y < lastPageSafeOffset.Value)
							{
								lastPageSafeOffset = new double?(vector.Y);
								lastPagePixelSize = new double?(stackPixelSizeInViewport.Height);
							}
							double num6 = stackPixelSizeInViewport.Height / stackLogicalSizeInViewport.Height;
							double num7 = Math.Floor(viewport.Height / num6);
							if (DoubleUtil.GreaterThan(num7, size2.Height))
							{
								if (VirtualizingStackPanel.ScrollTracer.IsEnabled && VirtualizingStackPanel.ScrollTracer.IsTracing(this))
								{
									VirtualizingStackPanel.ScrollTracer.Trace(this, VirtualizingStackPanel.ScrollTraceOp.RemeasureEndExpandViewport, new object[]
									{
										"off:",
										vector.Y,
										lastPageSafeOffset,
										"pxSz:",
										stackPixelSizeInViewport.Height,
										viewport.Height,
										"itSz:",
										stackLogicalSizeInViewport.Height,
										size2.Height,
										"newVpSz:",
										num7
									});
								}
								vector2.Y = double.PositiveInfinity;
								size2.Height = num7;
								remeasure = true;
								this.StorePreviouslyMeasuredOffset(ref previouslyMeasuredOffsets, vector.Y);
							}
						}
						if (!remeasure && flag && flag4 && !DoubleUtil.AreClose(this._scrollData._viewport.Height, size2.Height))
						{
							if (VirtualizingStackPanel.ScrollTracer.IsEnabled && VirtualizingStackPanel.ScrollTracer.IsTracing(this))
							{
								VirtualizingStackPanel.ScrollTracer.Trace(this, VirtualizingStackPanel.ScrollTraceOp.RemeasureEndChangeOffset, new object[]
								{
									"off:",
									vector.Y,
									"vpSz:",
									this._scrollData._viewport.Height,
									size2.Height,
									"newOff:",
									this._scrollData._offset
								});
							}
							remeasure = true;
							vector2.Y = double.PositiveInfinity;
							this.StorePreviouslyMeasuredOffset(ref previouslyMeasuredOffsets, vector.Y);
							if (DoubleUtil.AreClose(size2.Height, 0.0))
							{
								size2.Height = this._scrollData._viewport.Height;
							}
						}
					}
					if (!remeasure && flag8)
					{
						if (this._scrollData.VerticalScrollType == VirtualizingStackPanel.ScrollType.ToEnd || (DoubleUtil.GreaterThan(vector.Y, 0.0) && DoubleUtil.GreaterThan(vector.Y, size.Height - size2.Height)))
						{
							if (VirtualizingStackPanel.ScrollTracer.IsEnabled && VirtualizingStackPanel.ScrollTracer.IsTracing(this))
							{
								VirtualizingStackPanel.ScrollTracer.Trace(this, VirtualizingStackPanel.ScrollTraceOp.RemeasureEndExtentChanged, new object[]
								{
									"off:",
									vector.Y,
									"ext:",
									this._scrollData._extent.Height,
									size.Height,
									"vpSz:",
									size2.Height
								});
							}
							remeasure = true;
							vector2.Y = double.PositiveInfinity;
							this._scrollData.VerticalScrollType = VirtualizingStackPanel.ScrollType.ToEnd;
							this.StorePreviouslyMeasuredOffset(ref previouslyMeasuredOffsets, vector.Y);
						}
						else if (this._scrollData.VerticalScrollType == VirtualizingStackPanel.ScrollType.Absolute && !DoubleUtil.AreClose(this._scrollData._extent.Height, 0.0) && !DoubleUtil.AreClose(size.Height, 0.0))
						{
							if (this.IsPixelBased)
							{
								if (!LayoutDoubleUtil.AreClose(vector.Y / size.Height, this._scrollData._offset.Y / this._scrollData._extent.Height))
								{
									remeasure = true;
									vector2.Y = size.Height * this._scrollData._offset.Y / this._scrollData._extent.Height;
									this.StorePreviouslyMeasuredOffset(ref previouslyMeasuredOffsets, vector.Y);
								}
							}
							else if (!LayoutDoubleUtil.AreClose(Math.Floor(vector.Y) / size.Height, Math.Floor(this._scrollData._offset.Y) / this._scrollData._extent.Height))
							{
								remeasure = true;
								vector2.Y = Math.Floor(size.Height * Math.Floor(this._scrollData._offset.Y) / this._scrollData._extent.Height);
							}
							if ((VirtualizingStackPanel.ScrollTracer.IsEnabled && VirtualizingStackPanel.ScrollTracer.IsTracing(this)) & remeasure)
							{
								VirtualizingStackPanel.ScrollTracer.Trace(this, VirtualizingStackPanel.ScrollTraceOp.RemeasureRatio, new object[]
								{
									"expRat:",
									this._scrollData._offset.Y,
									this._scrollData._extent.Height,
									this._scrollData._offset.Y / this._scrollData._extent.Height,
									"actRat:",
									vector.Y,
									size.Height,
									vector.Y / size.Height,
									"newOff:",
									vector2.Y
								});
							}
						}
					}
					if (!remeasure && flag7)
					{
						if (this._scrollData.HorizontalScrollType == VirtualizingStackPanel.ScrollType.ToEnd || (DoubleUtil.GreaterThan(vector.X, 0.0) && DoubleUtil.GreaterThan(vector.X, size.Width - size2.Width)))
						{
							if (VirtualizingStackPanel.ScrollTracer.IsEnabled && VirtualizingStackPanel.ScrollTracer.IsTracing(this))
							{
								VirtualizingStackPanel.ScrollTracer.Trace(this, VirtualizingStackPanel.ScrollTraceOp.RemeasureEndExtentChanged, new object[]
								{
									"perp",
									"off:",
									vector.X,
									"ext:",
									this._scrollData._extent.Width,
									size.Width,
									"vpSz:",
									size2.Width
								});
							}
							remeasure = true;
							vector2.X = double.PositiveInfinity;
							this._scrollData.HorizontalScrollType = VirtualizingStackPanel.ScrollType.ToEnd;
						}
						else if (this._scrollData.HorizontalScrollType == VirtualizingStackPanel.ScrollType.Absolute && !DoubleUtil.AreClose(this._scrollData._extent.Width, 0.0) && !DoubleUtil.AreClose(size.Width, 0.0))
						{
							if (!LayoutDoubleUtil.AreClose(vector.X / size.Width, this._scrollData._offset.X / this._scrollData._extent.Width))
							{
								remeasure = true;
								vector2.X = size.Width * this._scrollData._offset.X / this._scrollData._extent.Width;
							}
							if ((VirtualizingStackPanel.ScrollTracer.IsEnabled && VirtualizingStackPanel.ScrollTracer.IsTracing(this)) & remeasure)
							{
								VirtualizingStackPanel.ScrollTracer.Trace(this, VirtualizingStackPanel.ScrollTraceOp.RemeasureRatio, new object[]
								{
									"perp",
									"expRat:",
									this._scrollData._offset.X,
									this._scrollData._extent.Width,
									this._scrollData._offset.X / this._scrollData._extent.Width,
									"actRat:",
									vector.X,
									size.Width,
									vector.X / size.Width,
									"newOff:",
									vector2.X
								});
							}
						}
					}
				}
			}
			if (remeasure && this.IsVirtualizing && !this.IsScrollActive)
			{
				if (isHorizontal && this._scrollData.HorizontalScrollType == VirtualizingStackPanel.ScrollType.ToEnd)
				{
					this.IsScrollActive = true;
				}
				if (!isHorizontal && this._scrollData.VerticalScrollType == VirtualizingStackPanel.ScrollType.ToEnd)
				{
					this.IsScrollActive = true;
				}
			}
			if (!this.IsVirtualizing && !remeasure)
			{
				this.ClearIsScrollActive();
			}
			flag4 = !DoubleUtil.AreClose(size2, this._scrollData._viewport);
			if (VirtualizingStackPanel.ScrollTracer.IsEnabled && VirtualizingStackPanel.ScrollTracer.IsTracing(this))
			{
				VirtualizingStackPanel.ScrollTracer.Trace(this, VirtualizingStackPanel.ScrollTraceOp.SVSDEnd, new object[]
				{
					"off:",
					this._scrollData._offset,
					vector2,
					"ext:",
					this._scrollData._extent,
					size,
					"co:",
					this._scrollData._computedOffset,
					vector,
					"vp:",
					this._scrollData._viewport,
					size2
				});
			}
			if (flag4 || flag5 || flag6)
			{
				Vector computedOffset = this._scrollData._computedOffset;
				Size viewport2 = this._scrollData._viewport;
				this._scrollData._viewport = size2;
				this._scrollData._extent = size;
				this._scrollData._computedOffset = vector;
				if (flag4)
				{
					this.OnViewportSizeChanged(viewport2, size2);
				}
				if (flag6)
				{
					this.OnViewportOffsetChanged(computedOffset, vector);
				}
				this.OnScrollChange();
			}
			this._scrollData._offset = vector2;
		}

		// Token: 0x06005AD5 RID: 23253 RVA: 0x001998CC File Offset: 0x00197ACC
		private void SetAndVerifyScrollingData(bool isHorizontal, Rect viewport, Size constraint, ref Size stackPixelSize, ref Size stackLogicalSize, ref Size stackPixelSizeInViewport, ref Size stackLogicalSizeInViewport, ref Size stackPixelSizeInCacheBeforeViewport, ref Size stackLogicalSizeInCacheBeforeViewport, ref bool remeasure, ref double? lastPageSafeOffset, ref List<double> previouslyMeasuredOffsets)
		{
			Vector vector = new Vector(viewport.Location.X, viewport.Location.Y);
			Size size;
			Size size2;
			if (this.IsPixelBased)
			{
				size = stackPixelSize;
				size2 = viewport.Size;
			}
			else
			{
				size = stackLogicalSize;
				size2 = stackLogicalSizeInViewport;
				if (isHorizontal)
				{
					if (DoubleUtil.GreaterThan(stackPixelSizeInViewport.Width, constraint.Width) && size2.Width > 1.0)
					{
						double num = size2.Width;
						size2.Width = num - 1.0;
					}
					size2.Height = viewport.Height;
				}
				else
				{
					if (DoubleUtil.GreaterThan(stackPixelSizeInViewport.Height, constraint.Height) && size2.Height > 1.0)
					{
						double num = size2.Height;
						size2.Height = num - 1.0;
					}
					size2.Width = viewport.Width;
				}
			}
			if (isHorizontal)
			{
				if (this.MeasureCaches && this.IsVirtualizing)
				{
					stackPixelSize.Height = this._scrollData._extent.Height;
				}
				this._scrollData._maxDesiredSize.Height = Math.Max(this._scrollData._maxDesiredSize.Height, stackPixelSize.Height);
				stackPixelSize.Height = this._scrollData._maxDesiredSize.Height;
				size.Height = stackPixelSize.Height;
				if (double.IsPositiveInfinity(constraint.Height))
				{
					size2.Height = stackPixelSize.Height;
				}
			}
			else
			{
				if (this.MeasureCaches && this.IsVirtualizing)
				{
					stackPixelSize.Width = this._scrollData._extent.Width;
				}
				this._scrollData._maxDesiredSize.Width = Math.Max(this._scrollData._maxDesiredSize.Width, stackPixelSize.Width);
				stackPixelSize.Width = this._scrollData._maxDesiredSize.Width;
				size.Width = stackPixelSize.Width;
				if (double.IsPositiveInfinity(constraint.Width))
				{
					size2.Width = stackPixelSize.Width;
				}
			}
			if (!double.IsPositiveInfinity(constraint.Width))
			{
				stackPixelSize.Width = ((this.IsPixelBased || DoubleUtil.AreClose(vector.X, 0.0)) ? Math.Min(stackPixelSize.Width, constraint.Width) : constraint.Width);
			}
			if (!double.IsPositiveInfinity(constraint.Height))
			{
				stackPixelSize.Height = ((this.IsPixelBased || DoubleUtil.AreClose(vector.Y, 0.0)) ? Math.Min(stackPixelSize.Height, constraint.Height) : constraint.Height);
			}
			bool flag = !DoubleUtil.AreClose(size2, this._scrollData._viewport);
			bool flag2 = !DoubleUtil.AreClose(size, this._scrollData._extent);
			bool flag3 = !DoubleUtil.AreClose(vector, this._scrollData._computedOffset);
			Vector offset = vector;
			bool flag4 = true;
			ScrollViewer scrollOwner = this.ScrollOwner;
			if (scrollOwner.InChildMeasurePass1 || scrollOwner.InChildMeasurePass2)
			{
				ScrollBarVisibility verticalScrollBarVisibility = scrollOwner.VerticalScrollBarVisibility;
				bool flag5 = verticalScrollBarVisibility == ScrollBarVisibility.Auto;
				if (flag5)
				{
					Visibility computedVerticalScrollBarVisibility = scrollOwner.ComputedVerticalScrollBarVisibility;
					Visibility visibility = DoubleUtil.LessThanOrClose(size.Height, size2.Height) ? Visibility.Collapsed : Visibility.Visible;
					if (computedVerticalScrollBarVisibility != visibility)
					{
						offset = this._scrollData._offset;
						flag4 = false;
					}
				}
				if (flag4)
				{
					ScrollBarVisibility horizontalScrollBarVisibility = scrollOwner.HorizontalScrollBarVisibility;
					bool flag6 = horizontalScrollBarVisibility == ScrollBarVisibility.Auto;
					if (flag6)
					{
						Visibility computedHorizontalScrollBarVisibility = scrollOwner.ComputedHorizontalScrollBarVisibility;
						Visibility visibility2 = DoubleUtil.LessThanOrClose(size.Width, size2.Width) ? Visibility.Collapsed : Visibility.Visible;
						if (computedHorizontalScrollBarVisibility != visibility2)
						{
							offset = this._scrollData._offset;
						}
					}
				}
			}
			if (isHorizontal)
			{
				flag4 = !this.WasOffsetPreviouslyMeasured(previouslyMeasuredOffsets, vector.X);
				if (flag4)
				{
					bool flag7 = !DoubleUtil.AreClose(vector.X, this._scrollData._offset.X);
					if (!this.IsPixelBased)
					{
						if (!this.IsEndOfViewport(isHorizontal, viewport, stackPixelSizeInViewport) && DoubleUtil.GreaterThan(stackLogicalSize.Width, stackLogicalSizeInViewport.Width))
						{
							lastPageSafeOffset = new double?((lastPageSafeOffset != null) ? Math.Min(vector.X, lastPageSafeOffset.Value) : vector.X);
							double num2 = stackPixelSizeInViewport.Width / stackLogicalSizeInViewport.Width;
							double num3 = Math.Floor(viewport.Width / num2);
							if (DoubleUtil.GreaterThan(num3, size2.Width))
							{
								offset.X = double.PositiveInfinity;
								size2.Width = num3;
								remeasure = true;
								this.StorePreviouslyMeasuredOffset(ref previouslyMeasuredOffsets, vector.X);
							}
						}
						if (!remeasure && flag7 && flag && !DoubleUtil.AreClose(this._scrollData._viewport.Width, size2.Width))
						{
							remeasure = true;
							offset.X = this._scrollData._offset.X;
							this.StorePreviouslyMeasuredOffset(ref previouslyMeasuredOffsets, vector.X);
							if (DoubleUtil.AreClose(size2.Width, 0.0))
							{
								size2.Width = this._scrollData._viewport.Width;
							}
						}
					}
					if (!remeasure && flag2 && !DoubleUtil.AreClose(this._scrollData._extent.Width, size.Width))
					{
						if (DoubleUtil.GreaterThan(vector.X, 0.0) && DoubleUtil.GreaterThan(vector.X, size.Width - size2.Width))
						{
							remeasure = true;
							offset.X = double.PositiveInfinity;
							this.StorePreviouslyMeasuredOffset(ref previouslyMeasuredOffsets, vector.X);
						}
						if (!remeasure && flag7)
						{
							remeasure = true;
							offset.X = this._scrollData._offset.X;
							this.StorePreviouslyMeasuredOffset(ref previouslyMeasuredOffsets, vector.X);
						}
						if (!remeasure)
						{
							bool flag8 = (this.MeasureCaches && !this.WasLastMeasurePassAnchored) || (this._scrollData._firstContainerInViewport == null && flag3 && !LayoutDoubleUtil.AreClose(vector.X, this._scrollData._computedOffset.X));
							if (flag8 && !DoubleUtil.AreClose(this._scrollData._extent.Width, 0.0) && !DoubleUtil.AreClose(size.Width, 0.0))
							{
								if (this.IsPixelBased)
								{
									if (!LayoutDoubleUtil.AreClose(vector.X / size.Width, this._scrollData._offset.X / this._scrollData._extent.Width))
									{
										remeasure = true;
										offset.X = size.Width * this._scrollData._offset.X / this._scrollData._extent.Width;
										this.StorePreviouslyMeasuredOffset(ref previouslyMeasuredOffsets, vector.X);
									}
								}
								else if (!LayoutDoubleUtil.AreClose(Math.Floor(vector.X) / size.Width, Math.Floor(this._scrollData._offset.X) / this._scrollData._extent.Width))
								{
									remeasure = true;
									offset.X = Math.Floor(size.Width * Math.Floor(this._scrollData._offset.X) / this._scrollData._extent.Width);
									this.StorePreviouslyMeasuredOffset(ref previouslyMeasuredOffsets, vector.X);
								}
							}
						}
					}
				}
				else if (!this.IsPixelBased && lastPageSafeOffset != null && !DoubleUtil.AreClose(lastPageSafeOffset.Value, vector.X))
				{
					offset.X = lastPageSafeOffset.Value;
					lastPageSafeOffset = null;
					remeasure = true;
				}
			}
			else
			{
				flag4 = !this.WasOffsetPreviouslyMeasured(previouslyMeasuredOffsets, vector.Y);
				if (flag4)
				{
					bool flag9 = !DoubleUtil.AreClose(vector.Y, this._scrollData._offset.Y);
					if (!this.IsPixelBased)
					{
						if (!this.IsEndOfViewport(isHorizontal, viewport, stackPixelSizeInViewport) && DoubleUtil.GreaterThan(stackLogicalSize.Height, stackLogicalSizeInViewport.Height))
						{
							lastPageSafeOffset = new double?((lastPageSafeOffset != null) ? Math.Min(vector.Y, lastPageSafeOffset.Value) : vector.Y);
							double num4 = stackPixelSizeInViewport.Height / stackLogicalSizeInViewport.Height;
							double num5 = Math.Floor(viewport.Height / num4);
							if (DoubleUtil.GreaterThan(num5, size2.Height))
							{
								offset.Y = double.PositiveInfinity;
								size2.Height = num5;
								remeasure = true;
								this.StorePreviouslyMeasuredOffset(ref previouslyMeasuredOffsets, vector.Y);
							}
						}
						if (!remeasure && flag9 && flag && !DoubleUtil.AreClose(this._scrollData._viewport.Height, size2.Height))
						{
							remeasure = true;
							offset.Y = this._scrollData._offset.Y;
							this.StorePreviouslyMeasuredOffset(ref previouslyMeasuredOffsets, vector.Y);
							if (DoubleUtil.AreClose(size2.Height, 0.0))
							{
								size2.Height = this._scrollData._viewport.Height;
							}
						}
					}
					if (!remeasure && flag2 && !DoubleUtil.AreClose(this._scrollData._extent.Height, size.Height))
					{
						if (DoubleUtil.GreaterThan(vector.Y, 0.0) && DoubleUtil.GreaterThan(vector.Y, size.Height - size2.Height))
						{
							remeasure = true;
							offset.Y = double.PositiveInfinity;
							this.StorePreviouslyMeasuredOffset(ref previouslyMeasuredOffsets, vector.Y);
						}
						if (!remeasure && flag9)
						{
							remeasure = true;
							offset.Y = this._scrollData._offset.Y;
							this.StorePreviouslyMeasuredOffset(ref previouslyMeasuredOffsets, vector.Y);
						}
						if (!remeasure)
						{
							bool flag10 = (this.MeasureCaches && !this.WasLastMeasurePassAnchored) || (this._scrollData._firstContainerInViewport == null && flag3 && !LayoutDoubleUtil.AreClose(vector.Y, this._scrollData._computedOffset.Y));
							if (flag10 && !DoubleUtil.AreClose(this._scrollData._extent.Height, 0.0) && !DoubleUtil.AreClose(size.Height, 0.0))
							{
								if (this.IsPixelBased)
								{
									if (!LayoutDoubleUtil.AreClose(vector.Y / size.Height, this._scrollData._offset.Y / this._scrollData._extent.Height))
									{
										remeasure = true;
										offset.Y = size.Height * this._scrollData._offset.Y / this._scrollData._extent.Height;
										this.StorePreviouslyMeasuredOffset(ref previouslyMeasuredOffsets, vector.Y);
									}
								}
								else if (!LayoutDoubleUtil.AreClose(Math.Floor(vector.Y) / size.Height, Math.Floor(this._scrollData._offset.Y) / this._scrollData._extent.Height))
								{
									remeasure = true;
									offset.Y = Math.Floor(size.Height * Math.Floor(this._scrollData._offset.Y) / this._scrollData._extent.Height);
									this.StorePreviouslyMeasuredOffset(ref previouslyMeasuredOffsets, vector.Y);
								}
							}
						}
					}
				}
				else if (!this.IsPixelBased && lastPageSafeOffset != null && !DoubleUtil.AreClose(lastPageSafeOffset.Value, vector.Y))
				{
					offset.Y = lastPageSafeOffset.Value;
					lastPageSafeOffset = null;
					remeasure = true;
				}
			}
			flag = !DoubleUtil.AreClose(size2, this._scrollData._viewport);
			if (flag || flag2 || flag3)
			{
				Vector computedOffset = this._scrollData._computedOffset;
				Size viewport2 = this._scrollData._viewport;
				this._scrollData._viewport = size2;
				this._scrollData._extent = size;
				this._scrollData._computedOffset = vector;
				if (flag)
				{
					this.OnViewportSizeChanged(viewport2, size2);
				}
				if (flag3)
				{
					this.OnViewportOffsetChanged(computedOffset, vector);
				}
				this.OnScrollChange();
			}
			this._scrollData._offset = offset;
		}

		// Token: 0x06005AD6 RID: 23254 RVA: 0x0019A5BD File Offset: 0x001987BD
		private void StorePreviouslyMeasuredOffset(ref List<double> previouslyMeasuredOffsets, double offset)
		{
			if (previouslyMeasuredOffsets == null)
			{
				previouslyMeasuredOffsets = new List<double>();
			}
			previouslyMeasuredOffsets.Add(offset);
		}

		// Token: 0x06005AD7 RID: 23255 RVA: 0x0019A5D4 File Offset: 0x001987D4
		private bool WasOffsetPreviouslyMeasured(List<double> previouslyMeasuredOffsets, double offset)
		{
			if (previouslyMeasuredOffsets != null)
			{
				for (int i = 0; i < previouslyMeasuredOffsets.Count; i++)
				{
					if (DoubleUtil.AreClose(previouslyMeasuredOffsets[i], offset))
					{
						return true;
					}
				}
			}
			return false;
		}

		/// <summary>Called when the size of the viewport changes.</summary>
		/// <param name="oldViewportSize">The old size of the viewport.</param>
		/// <param name="newViewportSize">The new size of the viewport.</param>
		// Token: 0x06005AD8 RID: 23256 RVA: 0x00002137 File Offset: 0x00000337
		protected virtual void OnViewportSizeChanged(Size oldViewportSize, Size newViewportSize)
		{
		}

		/// <summary>Called when the offset of the viewport changes as a user scrolls through content.</summary>
		/// <param name="oldViewportOffset">The old offset of the viewport.</param>
		/// <param name="newViewportOffset">The new offset of the viewport</param>
		// Token: 0x06005AD9 RID: 23257 RVA: 0x00002137 File Offset: 0x00000337
		protected virtual void OnViewportOffsetChanged(Vector oldViewportOffset, Vector newViewportOffset)
		{
		}

		/// <summary>Returns the position of the specified item, relative to the <see cref="T:System.Windows.Controls.VirtualizingStackPanel" />.</summary>
		/// <param name="child">The element whose position to find.</param>
		/// <returns>The position of the specified item, relative to the <see cref="T:System.Windows.Controls.VirtualizingStackPanel" />.</returns>
		// Token: 0x06005ADA RID: 23258 RVA: 0x0019A608 File Offset: 0x00198808
		protected override double GetItemOffsetCore(UIElement child)
		{
			if (child == null)
			{
				throw new ArgumentNullException("child");
			}
			bool isHorizontal = this.Orientation == Orientation.Horizontal;
			ItemsControl itemsControl;
			GroupItem groupItem;
			IContainItemStorage containItemStorage;
			IHierarchicalVirtualizationAndScrollInfo hierarchicalVirtualizationAndScrollInfo;
			object item;
			IContainItemStorage containItemStorage2;
			bool flag;
			this.GetOwners(false, isHorizontal, out itemsControl, out groupItem, out containItemStorage, out hierarchicalVirtualizationAndScrollInfo, out item, out containItemStorage2, out flag);
			ItemContainerGenerator itemContainerGenerator = (ItemContainerGenerator)base.Generator;
			IList itemsInternal = itemContainerGenerator.ItemsInternal;
			int num = itemContainerGenerator.IndexFromContainer(child, true);
			double result = 0.0;
			if (num >= 0)
			{
				IContainItemStorage itemStorageProvider = VirtualizingStackPanel.IsVSP45Compat ? containItemStorage : containItemStorage2;
				this.ComputeDistance(itemsInternal, containItemStorage, isHorizontal, this.GetAreContainersUniformlySized(itemStorageProvider, item), this.GetUniformOrAverageContainerSize(itemStorageProvider, item), 0, num, out result);
			}
			return result;
		}

		// Token: 0x06005ADB RID: 23259 RVA: 0x0019A6A8 File Offset: 0x001988A8
		private double FindScrollOffset(Visual v)
		{
			ItemsControl itemsOwner = ItemsControl.GetItemsOwner(this);
			DependencyObject dependencyObject = v;
			DependencyObject parent = VisualTreeHelper.GetParent(dependencyObject);
			double num = 0.0;
			bool flag = this.Orientation == Orientation.Horizontal;
			bool returnLocalIndex = true;
			for (;;)
			{
				IHierarchicalVirtualizationAndScrollInfo virtualizingChild = VirtualizingStackPanel.GetVirtualizingChild(parent);
				if (virtualizingChild != null)
				{
					Panel itemsHost = virtualizingChild.ItemsHost;
					dependencyObject = this.FindDirectDescendentOfItemsHost(itemsHost, dependencyObject);
					if (dependencyObject != null)
					{
						VirtualizingPanel virtualizingPanel = itemsHost as VirtualizingPanel;
						if (virtualizingPanel != null && virtualizingPanel.CanHierarchicallyScrollAndVirtualize)
						{
							double itemOffset = virtualizingPanel.GetItemOffset((UIElement)dependencyObject);
							num += itemOffset;
							if (this.IsPixelBased)
							{
								if (VirtualizingStackPanel.IsVSP45Compat)
								{
									Size pixelSize = virtualizingChild.HeaderDesiredSizes.PixelSize;
									num += (flag ? pixelSize.Width : pixelSize.Height);
								}
								else
								{
									Thickness itemsHostInsetForChild = this.GetItemsHostInsetForChild(virtualizingChild, null, null);
									num += (flag ? itemsHostInsetForChild.Left : itemsHostInsetForChild.Top);
								}
							}
							else if (VirtualizingStackPanel.IsVSP45Compat)
							{
								Size logicalSize = virtualizingChild.HeaderDesiredSizes.LogicalSize;
								num += (flag ? logicalSize.Width : logicalSize.Height);
							}
							else
							{
								Thickness itemsHostInsetForChild2 = this.GetItemsHostInsetForChild(virtualizingChild, null, null);
								bool flag2 = this.IsHeaderBeforeItems(flag, virtualizingChild as FrameworkElement, ref itemsHostInsetForChild2);
								num += (double)(flag2 ? 1 : 0);
							}
						}
					}
					dependencyObject = (DependencyObject)virtualizingChild;
				}
				else if (parent == this)
				{
					break;
				}
				parent = VisualTreeHelper.GetParent(parent);
			}
			dependencyObject = this.FindDirectDescendentOfItemsHost(this, dependencyObject);
			if (dependencyObject != null)
			{
				IContainItemStorage itemStorageProvider = VirtualizingStackPanel.GetItemStorageProvider(this);
				IContainItemStorage itemStorageProvider2 = VirtualizingStackPanel.IsVSP45Compat ? itemStorageProvider : (ItemsControl.GetItemsOwnerInternal(VisualTreeHelper.GetParent((Visual)itemStorageProvider)) as IContainItemStorage);
				IList itemsInternal = ((ItemContainerGenerator)this.Generator).ItemsInternal;
				int itemCount = ((ItemContainerGenerator)this.Generator).IndexFromContainer(dependencyObject, returnLocalIndex);
				double num2;
				this.ComputeDistance(itemsInternal, itemStorageProvider, flag, this.GetAreContainersUniformlySized(itemStorageProvider2, this), this.GetUniformOrAverageContainerSize(itemStorageProvider2, this), 0, itemCount, out num2);
				num += num2;
			}
			return num;
		}

		// Token: 0x06005ADC RID: 23260 RVA: 0x0019A8B8 File Offset: 0x00198AB8
		private DependencyObject FindDirectDescendentOfItemsHost(Panel itemsHost, DependencyObject child)
		{
			if (itemsHost == null || !itemsHost.IsVisible)
			{
				return null;
			}
			for (DependencyObject parent = VisualTreeHelper.GetParent(child); parent != itemsHost; parent = VisualTreeHelper.GetParent(child))
			{
				child = parent;
				if (child == null)
				{
					break;
				}
			}
			return child;
		}

		// Token: 0x06005ADD RID: 23261 RVA: 0x0019A8F0 File Offset: 0x00198AF0
		private void MakeVisiblePhysicalHelper(Rect r, ref Vector newOffset, ref Rect newRect, bool isHorizontal, ref bool alignTop, ref bool alignBottom)
		{
			double num;
			double num2;
			double num3;
			double num4;
			if (isHorizontal)
			{
				num = this._scrollData._computedOffset.X;
				num2 = this.ViewportWidth;
				num3 = r.X;
				num4 = r.Width;
			}
			else
			{
				num = this._scrollData._computedOffset.Y;
				num2 = this.ViewportHeight;
				num3 = r.Y;
				num4 = r.Height;
			}
			num3 += num;
			double num5 = ScrollContentPresenter.ComputeScrollOffsetWithMinimalScroll(num, num + num2, num3, num3 + num4, ref alignTop, ref alignBottom);
			if (alignTop)
			{
				num3 = num;
			}
			else if (alignBottom)
			{
				num3 = num + num2 - num4;
			}
			double num6 = Math.Max(num3, num5);
			num4 = Math.Max(Math.Min(num4 + num3, num5 + num2) - num6, 0.0);
			num3 = num6;
			num3 -= num;
			if (isHorizontal)
			{
				newOffset.X = num5;
				newRect.X = num3;
				newRect.Width = num4;
				return;
			}
			newOffset.Y = num5;
			newRect.Y = num3;
			newRect.Height = num4;
		}

		// Token: 0x06005ADE RID: 23262 RVA: 0x0019A9E0 File Offset: 0x00198BE0
		private void MakeVisibleLogicalHelper(int childIndex, Rect r, ref Vector newOffset, ref Rect newRect, ref bool alignTop, ref bool alignBottom)
		{
			bool flag = this.Orientation == Orientation.Horizontal;
			double num = r.Y;
			int num2;
			int num3;
			if (flag)
			{
				num2 = (int)this._scrollData._computedOffset.X;
				num3 = (int)this._scrollData._viewport.Width;
			}
			else
			{
				num2 = (int)this._scrollData._computedOffset.Y;
				num3 = (int)this._scrollData._viewport.Height;
			}
			int num4 = num2;
			if (childIndex < num2)
			{
				alignTop = true;
				num = 0.0;
				num4 = childIndex;
			}
			else if (childIndex > num2 + Math.Max(num3 - 1, 0))
			{
				alignBottom = true;
				num4 = childIndex - num3 + 1;
				double num5 = flag ? base.ActualWidth : base.ActualHeight;
				num = num5 * (1.0 - 1.0 / (double)num3);
			}
			if (flag)
			{
				newOffset.X = (double)num4;
				newRect.X = num;
				newRect.Width = r.Width;
				return;
			}
			newOffset.Y = (double)num4;
			newRect.Y = num;
			newRect.Height = r.Height;
		}

		// Token: 0x06005ADF RID: 23263 RVA: 0x0019AAEE File Offset: 0x00198CEE
		private int GetGeneratedIndex(int childIndex)
		{
			return base.Generator.IndexFromGeneratorPosition(new GeneratorPosition(childIndex, 0));
		}

		// Token: 0x06005AE0 RID: 23264 RVA: 0x0019AB04 File Offset: 0x00198D04
		private double GetMaxChildArrangeLength(IList children, bool isHorizontal)
		{
			double num = 0.0;
			int i = 0;
			int count = children.Count;
			while (i < count)
			{
				UIElement uielement = (UIElement)children[i];
				Size desiredSize = uielement.DesiredSize;
				if (isHorizontal)
				{
					num = Math.Max(num, desiredSize.Height);
				}
				else
				{
					num = Math.Max(num, desiredSize.Width);
				}
				i++;
			}
			return num;
		}

		// Token: 0x06005AE1 RID: 23265 RVA: 0x0019AB67 File Offset: 0x00198D67
		private static void OnOrientationChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			VirtualizingStackPanel.ResetScrolling(d as VirtualizingStackPanel);
		}

		// Token: 0x170015F5 RID: 5621
		// (get) Token: 0x06005AE2 RID: 23266 RVA: 0x0019AB74 File Offset: 0x00198D74
		// (set) Token: 0x06005AE3 RID: 23267 RVA: 0x0019AB7C File Offset: 0x00198D7C
		private bool HasMeasured
		{
			get
			{
				return base.VSP_HasMeasured;
			}
			set
			{
				base.VSP_HasMeasured = value;
			}
		}

		// Token: 0x170015F6 RID: 5622
		// (get) Token: 0x06005AE4 RID: 23268 RVA: 0x0019AB85 File Offset: 0x00198D85
		// (set) Token: 0x06005AE5 RID: 23269 RVA: 0x0019AB8D File Offset: 0x00198D8D
		private bool InRecyclingMode
		{
			get
			{
				return base.VSP_InRecyclingMode;
			}
			set
			{
				base.VSP_InRecyclingMode = value;
			}
		}

		// Token: 0x170015F7 RID: 5623
		// (get) Token: 0x06005AE6 RID: 23270 RVA: 0x0019AB96 File Offset: 0x00198D96
		internal bool IsScrolling
		{
			get
			{
				return this._scrollData != null && this._scrollData._scrollOwner != null;
			}
		}

		// Token: 0x170015F8 RID: 5624
		// (get) Token: 0x06005AE7 RID: 23271 RVA: 0x0019ABB0 File Offset: 0x00198DB0
		// (set) Token: 0x06005AE8 RID: 23272 RVA: 0x0019ABB8 File Offset: 0x00198DB8
		internal bool IsPixelBased
		{
			get
			{
				return base.VSP_IsPixelBased;
			}
			set
			{
				base.VSP_IsPixelBased = value;
			}
		}

		// Token: 0x170015F9 RID: 5625
		// (get) Token: 0x06005AE9 RID: 23273 RVA: 0x0019ABC1 File Offset: 0x00198DC1
		// (set) Token: 0x06005AEA RID: 23274 RVA: 0x0019ABC9 File Offset: 0x00198DC9
		internal bool MustDisableVirtualization
		{
			get
			{
				return base.VSP_MustDisableVirtualization;
			}
			set
			{
				base.VSP_MustDisableVirtualization = value;
			}
		}

		// Token: 0x170015FA RID: 5626
		// (get) Token: 0x06005AEB RID: 23275 RVA: 0x0019ABD2 File Offset: 0x00198DD2
		// (set) Token: 0x06005AEC RID: 23276 RVA: 0x0019ABE7 File Offset: 0x00198DE7
		internal bool MeasureCaches
		{
			get
			{
				return base.VSP_MeasureCaches || !this.IsVirtualizing;
			}
			set
			{
				base.VSP_MeasureCaches = value;
			}
		}

		/// <summary>Gets or sets a value that indicates that this <see cref="T:System.Windows.Controls.VirtualizingStackPanel" /> is virtualizing its child collection.</summary>
		/// <returns>
		///   <see langword="true" /> if this instance of <see cref="T:System.Windows.Controls.VirtualizingStackPanel" /> is virtualizing its child collection; otherwise <see langword="false" />. The default is <see langword="true" />.</returns>
		// Token: 0x170015FB RID: 5627
		// (get) Token: 0x06005AED RID: 23277 RVA: 0x0019ABF0 File Offset: 0x00198DF0
		// (set) Token: 0x06005AEE RID: 23278 RVA: 0x0019ABF8 File Offset: 0x00198DF8
		private bool IsVirtualizing
		{
			get
			{
				return base.VSP_IsVirtualizing;
			}
			set
			{
				if (!base.IsItemsHost || !value)
				{
					this._realizedChildren = null;
				}
				base.VSP_IsVirtualizing = value;
			}
		}

		// Token: 0x170015FC RID: 5628
		// (get) Token: 0x06005AEF RID: 23279 RVA: 0x0019AC1F File Offset: 0x00198E1F
		// (set) Token: 0x06005AF0 RID: 23280 RVA: 0x0019AC28 File Offset: 0x00198E28
		private bool HasVirtualizingChildren
		{
			get
			{
				return this.GetBoolField(VirtualizingStackPanel.BoolField.HasVirtualizingChildren);
			}
			set
			{
				this.SetBoolField(VirtualizingStackPanel.BoolField.HasVirtualizingChildren, value);
			}
		}

		// Token: 0x170015FD RID: 5629
		// (get) Token: 0x06005AF1 RID: 23281 RVA: 0x0019AC32 File Offset: 0x00198E32
		// (set) Token: 0x06005AF2 RID: 23282 RVA: 0x0019AC3B File Offset: 0x00198E3B
		private bool AlignTopOfBringIntoViewContainer
		{
			get
			{
				return this.GetBoolField(VirtualizingStackPanel.BoolField.AlignTopOfBringIntoViewContainer);
			}
			set
			{
				this.SetBoolField(VirtualizingStackPanel.BoolField.AlignTopOfBringIntoViewContainer, value);
			}
		}

		// Token: 0x170015FE RID: 5630
		// (get) Token: 0x06005AF3 RID: 23283 RVA: 0x0019AC45 File Offset: 0x00198E45
		// (set) Token: 0x06005AF4 RID: 23284 RVA: 0x0019AC4F File Offset: 0x00198E4F
		private bool AlignBottomOfBringIntoViewContainer
		{
			get
			{
				return this.GetBoolField(VirtualizingStackPanel.BoolField.AlignBottomOfBringIntoViewContainer);
			}
			set
			{
				this.SetBoolField(VirtualizingStackPanel.BoolField.AlignBottomOfBringIntoViewContainer, value);
			}
		}

		// Token: 0x170015FF RID: 5631
		// (get) Token: 0x06005AF5 RID: 23285 RVA: 0x0019AC5A File Offset: 0x00198E5A
		// (set) Token: 0x06005AF6 RID: 23286 RVA: 0x0019AC63 File Offset: 0x00198E63
		private bool WasLastMeasurePassAnchored
		{
			get
			{
				return this.GetBoolField(VirtualizingStackPanel.BoolField.WasLastMeasurePassAnchored);
			}
			set
			{
				this.SetBoolField(VirtualizingStackPanel.BoolField.WasLastMeasurePassAnchored, value);
			}
		}

		// Token: 0x17001600 RID: 5632
		// (get) Token: 0x06005AF7 RID: 23287 RVA: 0x0019AC6D File Offset: 0x00198E6D
		// (set) Token: 0x06005AF8 RID: 23288 RVA: 0x0019AC76 File Offset: 0x00198E76
		private bool ItemsChangedDuringMeasure
		{
			get
			{
				return this.GetBoolField(VirtualizingStackPanel.BoolField.ItemsChangedDuringMeasure);
			}
			set
			{
				this.SetBoolField(VirtualizingStackPanel.BoolField.ItemsChangedDuringMeasure, value);
			}
		}

		// Token: 0x17001601 RID: 5633
		// (get) Token: 0x06005AF9 RID: 23289 RVA: 0x0019AC80 File Offset: 0x00198E80
		// (set) Token: 0x06005AFA RID: 23290 RVA: 0x0019AC8C File Offset: 0x00198E8C
		private bool IsScrollActive
		{
			get
			{
				return this.GetBoolField(VirtualizingStackPanel.BoolField.IsScrollActive);
			}
			set
			{
				if (VirtualizingStackPanel.ScrollTracer.IsEnabled && VirtualizingStackPanel.ScrollTracer.IsTracing(this))
				{
					bool boolField = this.GetBoolField(VirtualizingStackPanel.BoolField.IsScrollActive);
					if (value != boolField)
					{
						VirtualizingStackPanel.ScrollTracer.Trace(this, VirtualizingStackPanel.ScrollTraceOp.IsScrollActive, new object[]
						{
							value
						});
					}
				}
				this.SetBoolField(VirtualizingStackPanel.BoolField.IsScrollActive, value);
				if (!value)
				{
					this._scrollData.HorizontalScrollType = VirtualizingStackPanel.ScrollType.None;
					this._scrollData.VerticalScrollType = VirtualizingStackPanel.ScrollType.None;
				}
			}
		}

		// Token: 0x17001602 RID: 5634
		// (get) Token: 0x06005AFB RID: 23291 RVA: 0x0019ACF0 File Offset: 0x00198EF0
		// (set) Token: 0x06005AFC RID: 23292 RVA: 0x0019ACFA File Offset: 0x00198EFA
		internal bool IgnoreMaxDesiredSize
		{
			get
			{
				return this.GetBoolField(VirtualizingStackPanel.BoolField.IgnoreMaxDesiredSize);
			}
			set
			{
				this.SetBoolField(VirtualizingStackPanel.BoolField.IgnoreMaxDesiredSize, value);
			}
		}

		// Token: 0x17001603 RID: 5635
		// (get) Token: 0x06005AFD RID: 23293 RVA: 0x0019AD05 File Offset: 0x00198F05
		// (set) Token: 0x06005AFE RID: 23294 RVA: 0x0019AD12 File Offset: 0x00198F12
		private bool IsMeasureCachesPending
		{
			get
			{
				return this.GetBoolField(VirtualizingStackPanel.BoolField.IsMeasureCachesPending);
			}
			set
			{
				this.SetBoolField(VirtualizingStackPanel.BoolField.IsMeasureCachesPending, value);
			}
		}

		// Token: 0x17001604 RID: 5636
		// (get) Token: 0x06005AFF RID: 23295 RVA: 0x0019AD20 File Offset: 0x00198F20
		// (set) Token: 0x06005B00 RID: 23296 RVA: 0x0019AD28 File Offset: 0x00198F28
		private bool? AreContainersUniformlySized { get; set; }

		// Token: 0x17001605 RID: 5637
		// (get) Token: 0x06005B01 RID: 23297 RVA: 0x0019AD31 File Offset: 0x00198F31
		// (set) Token: 0x06005B02 RID: 23298 RVA: 0x0019AD39 File Offset: 0x00198F39
		private double? UniformOrAverageContainerSize { get; set; }

		// Token: 0x17001606 RID: 5638
		// (get) Token: 0x06005B03 RID: 23299 RVA: 0x0019AD42 File Offset: 0x00198F42
		// (set) Token: 0x06005B04 RID: 23300 RVA: 0x0019AD4A File Offset: 0x00198F4A
		private double? UniformOrAverageContainerPixelSize { get; set; }

		// Token: 0x17001607 RID: 5639
		// (get) Token: 0x06005B05 RID: 23301 RVA: 0x0019AD53 File Offset: 0x00198F53
		private IList RealizedChildren
		{
			get
			{
				if (this.IsVirtualizing && this.InRecyclingMode)
				{
					this.EnsureRealizedChildren();
					return this._realizedChildren;
				}
				return base.InternalChildren;
			}
		}

		// Token: 0x17001608 RID: 5640
		// (get) Token: 0x06005B06 RID: 23302 RVA: 0x0019AD78 File Offset: 0x00198F78
		internal static bool IsVSP45Compat
		{
			get
			{
				return FrameworkCompatibilityPreferences.GetVSP45Compat();
			}
		}

		// Token: 0x17001609 RID: 5641
		// (get) Token: 0x06005B07 RID: 23303 RVA: 0x0019AD7F File Offset: 0x00198F7F
		bool IStackMeasure.IsScrolling
		{
			get
			{
				return this.IsScrolling;
			}
		}

		// Token: 0x1700160A RID: 5642
		// (get) Token: 0x06005B08 RID: 23304 RVA: 0x00171716 File Offset: 0x0016F916
		UIElementCollection IStackMeasure.InternalChildren
		{
			get
			{
				return base.InternalChildren;
			}
		}

		// Token: 0x06005B09 RID: 23305 RVA: 0x0019AD87 File Offset: 0x00198F87
		void IStackMeasure.OnScrollChange()
		{
			this.OnScrollChange();
		}

		// Token: 0x1700160B RID: 5643
		// (get) Token: 0x06005B0A RID: 23306 RVA: 0x0019AD8F File Offset: 0x00198F8F
		private DependencyObject BringIntoViewLeafContainer
		{
			get
			{
				VirtualizingStackPanel.ScrollData scrollData = this._scrollData;
				return ((scrollData != null) ? scrollData._bringIntoViewLeafContainer : null) ?? null;
			}
		}

		// Token: 0x1700160C RID: 5644
		// (get) Token: 0x06005B0B RID: 23307 RVA: 0x0019ADA8 File Offset: 0x00198FA8
		private FrameworkElement FirstContainerInViewport
		{
			get
			{
				VirtualizingStackPanel.ScrollData scrollData = this._scrollData;
				return ((scrollData != null) ? scrollData._firstContainerInViewport : null) ?? null;
			}
		}

		// Token: 0x1700160D RID: 5645
		// (get) Token: 0x06005B0C RID: 23308 RVA: 0x0019ADC1 File Offset: 0x00198FC1
		private double FirstContainerOffsetFromViewport
		{
			get
			{
				VirtualizingStackPanel.ScrollData scrollData = this._scrollData;
				if (scrollData == null)
				{
					return 0.0;
				}
				return scrollData._firstContainerOffsetFromViewport;
			}
		}

		// Token: 0x1700160E RID: 5646
		// (get) Token: 0x06005B0D RID: 23309 RVA: 0x0019ADDC File Offset: 0x00198FDC
		private double ExpectedDistanceBetweenViewports
		{
			get
			{
				VirtualizingStackPanel.ScrollData scrollData = this._scrollData;
				if (scrollData == null)
				{
					return 0.0;
				}
				return scrollData._expectedDistanceBetweenViewports;
			}
		}

		// Token: 0x1700160F RID: 5647
		// (get) Token: 0x06005B0E RID: 23310 RVA: 0x0017FD16 File Offset: 0x0017DF16
		private bool CanMouseWheelVerticallyScroll
		{
			get
			{
				return SystemParameters.WheelScrollLines > 0;
			}
		}

		// Token: 0x06005B0F RID: 23311 RVA: 0x0019ADF7 File Offset: 0x00198FF7
		private bool GetBoolField(VirtualizingStackPanel.BoolField field)
		{
			return (this._boolFieldStore & field) > ~(VirtualizingStackPanel.BoolField.HasVirtualizingChildren | VirtualizingStackPanel.BoolField.AlignTopOfBringIntoViewContainer | VirtualizingStackPanel.BoolField.WasLastMeasurePassAnchored | VirtualizingStackPanel.BoolField.ItemsChangedDuringMeasure | VirtualizingStackPanel.BoolField.IsScrollActive | VirtualizingStackPanel.BoolField.IgnoreMaxDesiredSize | VirtualizingStackPanel.BoolField.AlignBottomOfBringIntoViewContainer | VirtualizingStackPanel.BoolField.IsMeasureCachesPending);
		}

		// Token: 0x06005B10 RID: 23312 RVA: 0x0019AE04 File Offset: 0x00199004
		private void SetBoolField(VirtualizingStackPanel.BoolField field, bool value)
		{
			if (value)
			{
				this._boolFieldStore |= field;
				return;
			}
			this._boolFieldStore &= ~field;
		}

		// Token: 0x06005B11 RID: 23313 RVA: 0x0019AE28 File Offset: 0x00199028
		private VirtualizingStackPanel.Snapshot TakeSnapshot()
		{
			VirtualizingStackPanel.Snapshot snapshot = new VirtualizingStackPanel.Snapshot();
			if (this.IsScrolling)
			{
				snapshot._scrollData = new VirtualizingStackPanel.ScrollData();
				snapshot._scrollData._offset = this._scrollData._offset;
				snapshot._scrollData._extent = this._scrollData._extent;
				snapshot._scrollData._computedOffset = this._scrollData._computedOffset;
				snapshot._scrollData._viewport = this._scrollData._viewport;
			}
			snapshot._boolFieldStore = this._boolFieldStore;
			snapshot._areContainersUniformlySized = this.AreContainersUniformlySized;
			snapshot._firstItemInExtendedViewportChildIndex = this._firstItemInExtendedViewportChildIndex;
			snapshot._firstItemInExtendedViewportIndex = this._firstItemInExtendedViewportIndex;
			snapshot._firstItemInExtendedViewportOffset = this._firstItemInExtendedViewportOffset;
			snapshot._actualItemsInExtendedViewportCount = this._actualItemsInExtendedViewportCount;
			snapshot._viewport = this._viewport;
			snapshot._itemsInExtendedViewportCount = this._itemsInExtendedViewportCount;
			snapshot._extendedViewport = this._extendedViewport;
			snapshot._previousStackPixelSizeInViewport = this._previousStackPixelSizeInViewport;
			snapshot._previousStackLogicalSizeInViewport = this._previousStackLogicalSizeInViewport;
			snapshot._previousStackPixelSizeInCacheBeforeViewport = this._previousStackPixelSizeInCacheBeforeViewport;
			snapshot._firstContainerInViewport = this.FirstContainerInViewport;
			snapshot._firstContainerOffsetFromViewport = this.FirstContainerOffsetFromViewport;
			snapshot._expectedDistanceBetweenViewports = this.ExpectedDistanceBetweenViewports;
			snapshot._bringIntoViewContainer = this._bringIntoViewContainer;
			snapshot._bringIntoViewLeafContainer = this.BringIntoViewLeafContainer;
			VirtualizingStackPanel.SnapshotData value = VirtualizingStackPanel.SnapshotDataField.GetValue(this);
			if (value != null)
			{
				snapshot._uniformOrAverageContainerSize = new double?(value.UniformOrAverageContainerSize);
				snapshot._uniformOrAverageContainerPixelSize = new double?(value.UniformOrAverageContainerPixelSize);
				snapshot._effectiveOffsets = value.EffectiveOffsets;
				VirtualizingStackPanel.SnapshotDataField.ClearValue(this);
			}
			ItemContainerGenerator itemContainerGenerator = base.Generator as ItemContainerGenerator;
			List<VirtualizingStackPanel.ChildInfo> list = new List<VirtualizingStackPanel.ChildInfo>();
			foreach (object obj in this.RealizedChildren)
			{
				UIElement uielement = (UIElement)obj;
				list.Add(new VirtualizingStackPanel.ChildInfo
				{
					_itemIndex = itemContainerGenerator.IndexFromContainer(uielement, true),
					_desiredSize = uielement.DesiredSize,
					_arrangeRect = uielement.PreviousArrangeRect,
					_inset = (Thickness)uielement.GetValue(VirtualizingStackPanel.ItemsHostInsetProperty)
				});
			}
			snapshot._realizedChildren = list;
			return snapshot;
		}

		// Token: 0x06005B12 RID: 23314 RVA: 0x0019B074 File Offset: 0x00199274
		private string ContainerPath(DependencyObject container)
		{
			if (container == null)
			{
				return string.Empty;
			}
			VirtualizingStackPanel virtualizingStackPanel = VisualTreeHelper.GetParent(container) as VirtualizingStackPanel;
			if (virtualizingStackPanel == null)
			{
				return "{Disconnected}";
			}
			if (virtualizingStackPanel == this)
			{
				ItemContainerGenerator itemContainerGenerator = base.Generator as ItemContainerGenerator;
				return string.Format(CultureInfo.InvariantCulture, "{0}", new object[]
				{
					itemContainerGenerator.IndexFromContainer(container, true)
				});
			}
			ItemContainerGenerator itemContainerGenerator2 = virtualizingStackPanel.Generator as ItemContainerGenerator;
			int num = itemContainerGenerator2.IndexFromContainer(container, true);
			DependencyObject dependencyObject = ItemsControl.ContainerFromElement(null, virtualizingStackPanel);
			if (dependencyObject == null)
			{
				return string.Format(CultureInfo.InvariantCulture, "{0}", new object[]
				{
					num
				});
			}
			return string.Format(CultureInfo.InvariantCulture, "{0}.{1}", new object[]
			{
				this.ContainerPath(dependencyObject),
				num
			});
		}

		// Token: 0x04002F38 RID: 12088
		private static readonly DependencyProperty ContainerSizeProperty = DependencyProperty.Register("ContainerSize", typeof(Size), typeof(VirtualizingStackPanel));

		// Token: 0x04002F39 RID: 12089
		private static readonly DependencyProperty ContainerSizeDualProperty = DependencyProperty.Register("ContainerSizeDual", typeof(VirtualizingStackPanel.ContainerSizeDual), typeof(VirtualizingStackPanel));

		// Token: 0x04002F3A RID: 12090
		private static readonly DependencyProperty AreContainersUniformlySizedProperty = DependencyProperty.Register("AreContainersUniformlySized", typeof(bool), typeof(VirtualizingStackPanel));

		// Token: 0x04002F3B RID: 12091
		private static readonly DependencyProperty UniformOrAverageContainerSizeProperty = DependencyProperty.Register("UniformOrAverageContainerSize", typeof(double), typeof(VirtualizingStackPanel));

		// Token: 0x04002F3C RID: 12092
		private static readonly DependencyProperty UniformOrAverageContainerSizeDualProperty = DependencyProperty.Register("UniformOrAverageContainerSizeDual", typeof(VirtualizingStackPanel.UniformOrAverageContainerSizeDual), typeof(VirtualizingStackPanel));

		// Token: 0x04002F3D RID: 12093
		internal static readonly DependencyProperty ItemsHostInsetProperty = DependencyProperty.Register("ItemsHostInset", typeof(Thickness), typeof(VirtualizingStackPanel));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.VirtualizingPanel.IsVirtualizing" /> attached property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.VirtualizingPanel.IsVirtualizing" /> attached property.</returns>
		// Token: 0x04002F3E RID: 12094
		public new static readonly DependencyProperty IsVirtualizingProperty = VirtualizingPanel.IsVirtualizingProperty;

		/// <summary>Identifies the <see langword="VirtualizingStackPanel.VirtualizationMode" /> attached property.</summary>
		/// <returns>The identifier for the <see langword="VirtualizingStackPanel.VirtualizationMode" /> attached property.</returns>
		// Token: 0x04002F3F RID: 12095
		public new static readonly DependencyProperty VirtualizationModeProperty = VirtualizingPanel.VirtualizationModeProperty;

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.VirtualizingStackPanel.Orientation" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.VirtualizingStackPanel.Orientation" /> dependency property.</returns>
		// Token: 0x04002F40 RID: 12096
		public static readonly DependencyProperty OrientationProperty = DependencyProperty.Register("Orientation", typeof(Orientation), typeof(VirtualizingStackPanel), new FrameworkPropertyMetadata(Orientation.Vertical, FrameworkPropertyMetadataOptions.AffectsMeasure, new PropertyChangedCallback(VirtualizingStackPanel.OnOrientationChanged)), new ValidateValueCallback(ScrollBar.IsValidOrientation));

		/// <summary>Identifies the <see cref="E:System.Windows.Controls.VirtualizingStackPanel.CleanUpVirtualizedItem" /> attached event. </summary>
		/// <returns>The identifier for the <see cref="E:System.Windows.Controls.VirtualizingStackPanel.CleanUpVirtualizedItem" /> attached event.</returns>
		// Token: 0x04002F41 RID: 12097
		public static readonly RoutedEvent CleanUpVirtualizedItemEvent = EventManager.RegisterRoutedEvent("CleanUpVirtualizedItemEvent", RoutingStrategy.Direct, typeof(CleanUpVirtualizedItemEventHandler), typeof(VirtualizingStackPanel));

		// Token: 0x04002F45 RID: 12101
		private VirtualizingStackPanel.BoolField _boolFieldStore;

		// Token: 0x04002F46 RID: 12102
		private VirtualizingStackPanel.ScrollData _scrollData;

		// Token: 0x04002F47 RID: 12103
		private int _firstItemInExtendedViewportChildIndex;

		// Token: 0x04002F48 RID: 12104
		private int _firstItemInExtendedViewportIndex;

		// Token: 0x04002F49 RID: 12105
		private double _firstItemInExtendedViewportOffset;

		// Token: 0x04002F4A RID: 12106
		private int _actualItemsInExtendedViewportCount;

		// Token: 0x04002F4B RID: 12107
		private Rect _viewport;

		// Token: 0x04002F4C RID: 12108
		private int _itemsInExtendedViewportCount;

		// Token: 0x04002F4D RID: 12109
		private Rect _extendedViewport;

		// Token: 0x04002F4E RID: 12110
		private Size _previousStackPixelSizeInViewport;

		// Token: 0x04002F4F RID: 12111
		private Size _previousStackLogicalSizeInViewport;

		// Token: 0x04002F50 RID: 12112
		private Size _previousStackPixelSizeInCacheBeforeViewport;

		// Token: 0x04002F51 RID: 12113
		private double _pixelDistanceToFirstContainerInExtendedViewport;

		// Token: 0x04002F52 RID: 12114
		private double _pixelDistanceToViewport;

		// Token: 0x04002F53 RID: 12115
		private List<UIElement> _realizedChildren;

		// Token: 0x04002F54 RID: 12116
		private DispatcherOperation _cleanupOperation;

		// Token: 0x04002F55 RID: 12117
		private DispatcherTimer _cleanupDelay;

		// Token: 0x04002F56 RID: 12118
		private const int FocusTrail = 5;

		// Token: 0x04002F57 RID: 12119
		private DependencyObject _bringIntoViewContainer;

		// Token: 0x04002F58 RID: 12120
		private static int[] _indicesStoredInItemValueStorage;

		// Token: 0x04002F59 RID: 12121
		private static readonly UncommonField<DispatcherOperation> MeasureCachesOperationField = new UncommonField<DispatcherOperation>();

		// Token: 0x04002F5A RID: 12122
		private static readonly UncommonField<DispatcherOperation> AnchorOperationField = new UncommonField<DispatcherOperation>();

		// Token: 0x04002F5B RID: 12123
		private static readonly UncommonField<DispatcherOperation> AnchoredInvalidateMeasureOperationField = new UncommonField<DispatcherOperation>();

		// Token: 0x04002F5C RID: 12124
		private static readonly UncommonField<DispatcherOperation> ClearIsScrollActiveOperationField = new UncommonField<DispatcherOperation>();

		// Token: 0x04002F5D RID: 12125
		private static readonly UncommonField<VirtualizingStackPanel.OffsetInformation> OffsetInformationField = new UncommonField<VirtualizingStackPanel.OffsetInformation>();

		// Token: 0x04002F5E RID: 12126
		private static readonly UncommonField<VirtualizingStackPanel.EffectiveOffsetInformation> EffectiveOffsetInformationField = new UncommonField<VirtualizingStackPanel.EffectiveOffsetInformation>();

		// Token: 0x04002F5F RID: 12127
		private static readonly UncommonField<VirtualizingStackPanel.SnapshotData> SnapshotDataField = new UncommonField<VirtualizingStackPanel.SnapshotData>();

		// Token: 0x04002F60 RID: 12128
		private static UncommonField<VirtualizingStackPanel.FirstContainerInformation> FirstContainerInformationField = new UncommonField<VirtualizingStackPanel.FirstContainerInformation>();

		// Token: 0x04002F61 RID: 12129
		private static readonly UncommonField<VirtualizingStackPanel.ScrollTracingInfo> ScrollTracingInfoField = new UncommonField<VirtualizingStackPanel.ScrollTracingInfo>();

		// Token: 0x020009CB RID: 2507
		[Flags]
		private enum BoolField : byte
		{
			// Token: 0x0400459C RID: 17820
			HasVirtualizingChildren = 1,
			// Token: 0x0400459D RID: 17821
			AlignTopOfBringIntoViewContainer = 2,
			// Token: 0x0400459E RID: 17822
			WasLastMeasurePassAnchored = 4,
			// Token: 0x0400459F RID: 17823
			ItemsChangedDuringMeasure = 8,
			// Token: 0x040045A0 RID: 17824
			IsScrollActive = 16,
			// Token: 0x040045A1 RID: 17825
			IgnoreMaxDesiredSize = 32,
			// Token: 0x040045A2 RID: 17826
			AlignBottomOfBringIntoViewContainer = 64,
			// Token: 0x040045A3 RID: 17827
			IsMeasureCachesPending = 128
		}

		// Token: 0x020009CC RID: 2508
		private enum ScrollType
		{
			// Token: 0x040045A5 RID: 17829
			None,
			// Token: 0x040045A6 RID: 17830
			Relative,
			// Token: 0x040045A7 RID: 17831
			Absolute,
			// Token: 0x040045A8 RID: 17832
			ToEnd
		}

		// Token: 0x020009CD RID: 2509
		private class ScrollData : IStackMeasureScrollData
		{
			// Token: 0x060088BE RID: 35006 RVA: 0x00252EBC File Offset: 0x002510BC
			internal void ClearLayout()
			{
				this._offset = default(Vector);
				this._viewport = (this._extent = (this._maxDesiredSize = default(Size)));
			}

			// Token: 0x17001EE3 RID: 7907
			// (get) Token: 0x060088BF RID: 35007 RVA: 0x00252EF8 File Offset: 0x002510F8
			internal bool IsEmpty
			{
				get
				{
					return this._offset.X == 0.0 && this._offset.Y == 0.0 && this._viewport.Width == 0.0 && this._viewport.Height == 0.0 && this._extent.Width == 0.0 && this._extent.Height == 0.0 && this._maxDesiredSize.Width == 0.0 && this._maxDesiredSize.Height == 0.0;
				}
			}

			// Token: 0x17001EE4 RID: 7908
			// (get) Token: 0x060088C0 RID: 35008 RVA: 0x00252FBD File Offset: 0x002511BD
			// (set) Token: 0x060088C1 RID: 35009 RVA: 0x00252FC5 File Offset: 0x002511C5
			public Vector Offset
			{
				get
				{
					return this._offset;
				}
				set
				{
					this._offset = value;
				}
			}

			// Token: 0x17001EE5 RID: 7909
			// (get) Token: 0x060088C2 RID: 35010 RVA: 0x00252FCE File Offset: 0x002511CE
			// (set) Token: 0x060088C3 RID: 35011 RVA: 0x00252FD6 File Offset: 0x002511D6
			public Size Viewport
			{
				get
				{
					return this._viewport;
				}
				set
				{
					this._viewport = value;
				}
			}

			// Token: 0x17001EE6 RID: 7910
			// (get) Token: 0x060088C4 RID: 35012 RVA: 0x00252FDF File Offset: 0x002511DF
			// (set) Token: 0x060088C5 RID: 35013 RVA: 0x00252FE7 File Offset: 0x002511E7
			public Size Extent
			{
				get
				{
					return this._extent;
				}
				set
				{
					this._extent = value;
				}
			}

			// Token: 0x17001EE7 RID: 7911
			// (get) Token: 0x060088C6 RID: 35014 RVA: 0x00252FF0 File Offset: 0x002511F0
			// (set) Token: 0x060088C7 RID: 35015 RVA: 0x00252FF8 File Offset: 0x002511F8
			public Vector ComputedOffset
			{
				get
				{
					return this._computedOffset;
				}
				set
				{
					this._computedOffset = value;
				}
			}

			// Token: 0x060088C8 RID: 35016 RVA: 0x00002137 File Offset: 0x00000337
			public void SetPhysicalViewport(double value)
			{
			}

			// Token: 0x17001EE8 RID: 7912
			// (get) Token: 0x060088C9 RID: 35017 RVA: 0x00253001 File Offset: 0x00251201
			// (set) Token: 0x060088CA RID: 35018 RVA: 0x00253009 File Offset: 0x00251209
			public VirtualizingStackPanel.ScrollType HorizontalScrollType { get; set; }

			// Token: 0x17001EE9 RID: 7913
			// (get) Token: 0x060088CB RID: 35019 RVA: 0x00253012 File Offset: 0x00251212
			// (set) Token: 0x060088CC RID: 35020 RVA: 0x0025301A File Offset: 0x0025121A
			public VirtualizingStackPanel.ScrollType VerticalScrollType { get; set; }

			// Token: 0x060088CD RID: 35021 RVA: 0x00253024 File Offset: 0x00251224
			public void SetHorizontalScrollType(double oldOffset, double newOffset)
			{
				if (DoubleUtil.GreaterThanOrClose(newOffset, this._extent.Width - this._viewport.Width))
				{
					this.HorizontalScrollType = VirtualizingStackPanel.ScrollType.ToEnd;
					return;
				}
				if (DoubleUtil.GreaterThan(Math.Abs(newOffset - oldOffset), this._viewport.Width))
				{
					this.HorizontalScrollType = VirtualizingStackPanel.ScrollType.Absolute;
					return;
				}
				if (this.HorizontalScrollType == VirtualizingStackPanel.ScrollType.None)
				{
					this.HorizontalScrollType = VirtualizingStackPanel.ScrollType.Relative;
				}
			}

			// Token: 0x060088CE RID: 35022 RVA: 0x0025308C File Offset: 0x0025128C
			public void SetVerticalScrollType(double oldOffset, double newOffset)
			{
				if (DoubleUtil.GreaterThanOrClose(newOffset, this._extent.Height - this._viewport.Height))
				{
					this.VerticalScrollType = VirtualizingStackPanel.ScrollType.ToEnd;
					return;
				}
				if (DoubleUtil.GreaterThan(Math.Abs(newOffset - oldOffset), this._viewport.Height))
				{
					this.VerticalScrollType = VirtualizingStackPanel.ScrollType.Absolute;
					return;
				}
				if (this.VerticalScrollType == VirtualizingStackPanel.ScrollType.None)
				{
					this.VerticalScrollType = VirtualizingStackPanel.ScrollType.Relative;
				}
			}

			// Token: 0x040045A9 RID: 17833
			internal bool _allowHorizontal;

			// Token: 0x040045AA RID: 17834
			internal bool _allowVertical;

			// Token: 0x040045AB RID: 17835
			internal Vector _offset;

			// Token: 0x040045AC RID: 17836
			internal Vector _computedOffset = new Vector(0.0, 0.0);

			// Token: 0x040045AD RID: 17837
			internal Size _viewport;

			// Token: 0x040045AE RID: 17838
			internal Size _extent;

			// Token: 0x040045AF RID: 17839
			internal ScrollViewer _scrollOwner;

			// Token: 0x040045B0 RID: 17840
			internal Size _maxDesiredSize;

			// Token: 0x040045B1 RID: 17841
			internal DependencyObject _bringIntoViewLeafContainer;

			// Token: 0x040045B2 RID: 17842
			internal FrameworkElement _firstContainerInViewport;

			// Token: 0x040045B3 RID: 17843
			internal double _firstContainerOffsetFromViewport;

			// Token: 0x040045B4 RID: 17844
			internal double _expectedDistanceBetweenViewports;

			// Token: 0x040045B5 RID: 17845
			internal long _scrollGeneration;
		}

		// Token: 0x020009CE RID: 2510
		private class OffsetInformation
		{
			// Token: 0x17001EEA RID: 7914
			// (get) Token: 0x060088D0 RID: 35024 RVA: 0x00253116 File Offset: 0x00251316
			// (set) Token: 0x060088D1 RID: 35025 RVA: 0x0025311E File Offset: 0x0025131E
			public List<double> previouslyMeasuredOffsets { get; set; }

			// Token: 0x17001EEB RID: 7915
			// (get) Token: 0x060088D2 RID: 35026 RVA: 0x00253127 File Offset: 0x00251327
			// (set) Token: 0x060088D3 RID: 35027 RVA: 0x0025312F File Offset: 0x0025132F
			public double? lastPageSafeOffset { get; set; }

			// Token: 0x17001EEC RID: 7916
			// (get) Token: 0x060088D4 RID: 35028 RVA: 0x00253138 File Offset: 0x00251338
			// (set) Token: 0x060088D5 RID: 35029 RVA: 0x00253140 File Offset: 0x00251340
			public double? lastPagePixelSize { get; set; }
		}

		// Token: 0x020009CF RID: 2511
		private class FirstContainerInformation
		{
			// Token: 0x060088D7 RID: 35031 RVA: 0x00253149 File Offset: 0x00251349
			public FirstContainerInformation(ref Rect viewport, DependencyObject firstContainer, int firstItemIndex, double firstItemOffset, long scrollGeneration)
			{
				this.Viewport = viewport;
				this.FirstContainer = firstContainer;
				this.FirstItemIndex = firstItemIndex;
				this.FirstItemOffset = firstItemOffset;
				this.ScrollGeneration = scrollGeneration;
			}

			// Token: 0x040045BB RID: 17851
			public Rect Viewport;

			// Token: 0x040045BC RID: 17852
			public DependencyObject FirstContainer;

			// Token: 0x040045BD RID: 17853
			public int FirstItemIndex;

			// Token: 0x040045BE RID: 17854
			public double FirstItemOffset;

			// Token: 0x040045BF RID: 17855
			public long ScrollGeneration;
		}

		// Token: 0x020009D0 RID: 2512
		private class ContainerSizeDual : Tuple<Size, Size>
		{
			// Token: 0x060088D8 RID: 35032 RVA: 0x0025317B File Offset: 0x0025137B
			public ContainerSizeDual(Size pixelSize, Size itemSize) : base(pixelSize, itemSize)
			{
			}

			// Token: 0x17001EED RID: 7917
			// (get) Token: 0x060088D9 RID: 35033 RVA: 0x00253185 File Offset: 0x00251385
			public Size PixelSize
			{
				get
				{
					return base.Item1;
				}
			}

			// Token: 0x17001EEE RID: 7918
			// (get) Token: 0x060088DA RID: 35034 RVA: 0x0025318D File Offset: 0x0025138D
			public Size ItemSize
			{
				get
				{
					return base.Item2;
				}
			}
		}

		// Token: 0x020009D1 RID: 2513
		private class UniformOrAverageContainerSizeDual : Tuple<double, double>
		{
			// Token: 0x060088DB RID: 35035 RVA: 0x00253195 File Offset: 0x00251395
			public UniformOrAverageContainerSizeDual(double pixelSize, double itemSize) : base(pixelSize, itemSize)
			{
			}

			// Token: 0x17001EEF RID: 7919
			// (get) Token: 0x060088DC RID: 35036 RVA: 0x0025319F File Offset: 0x0025139F
			public double PixelSize
			{
				get
				{
					return base.Item1;
				}
			}

			// Token: 0x17001EF0 RID: 7920
			// (get) Token: 0x060088DD RID: 35037 RVA: 0x002531A7 File Offset: 0x002513A7
			public double ItemSize
			{
				get
				{
					return base.Item2;
				}
			}
		}

		// Token: 0x020009D2 RID: 2514
		private class EffectiveOffsetInformation
		{
			// Token: 0x17001EF1 RID: 7921
			// (get) Token: 0x060088DE RID: 35038 RVA: 0x002531AF File Offset: 0x002513AF
			// (set) Token: 0x060088DF RID: 35039 RVA: 0x002531B7 File Offset: 0x002513B7
			public long ScrollGeneration { get; private set; }

			// Token: 0x17001EF2 RID: 7922
			// (get) Token: 0x060088E0 RID: 35040 RVA: 0x002531C0 File Offset: 0x002513C0
			// (set) Token: 0x060088E1 RID: 35041 RVA: 0x002531C8 File Offset: 0x002513C8
			public List<double> OffsetList { get; private set; }

			// Token: 0x060088E2 RID: 35042 RVA: 0x002531D1 File Offset: 0x002513D1
			public EffectiveOffsetInformation(long scrollGeneration)
			{
				this.ScrollGeneration = scrollGeneration;
				this.OffsetList = new List<double>(2);
			}
		}

		// Token: 0x020009D3 RID: 2515
		private class ScrollTracer
		{
			// Token: 0x060088E3 RID: 35043 RVA: 0x002531EC File Offset: 0x002513EC
			static ScrollTracer()
			{
				VirtualizingStackPanel.ScrollTracer._targetName = FrameworkCompatibilityPreferences.GetScrollingTraceTarget();
				VirtualizingStackPanel.ScrollTracer._flushDepth = 0;
				VirtualizingStackPanel.ScrollTracer._luThreshold = 20;
				string scrollingTraceFile = FrameworkCompatibilityPreferences.GetScrollingTraceFile();
				if (!string.IsNullOrEmpty(scrollingTraceFile))
				{
					string[] array = scrollingTraceFile.Split(new char[]
					{
						';'
					});
					VirtualizingStackPanel.ScrollTracer._fileName = array[0];
					int flushDepth;
					if (array.Length > 1 && int.TryParse(array[1], NumberStyles.Integer, CultureInfo.InvariantCulture, out flushDepth))
					{
						VirtualizingStackPanel.ScrollTracer._flushDepth = flushDepth;
					}
					int num;
					if (array.Length > 2 && int.TryParse(array[2], NumberStyles.Integer, CultureInfo.InvariantCulture, out num))
					{
						VirtualizingStackPanel.ScrollTracer._luThreshold = ((num <= 0) ? int.MaxValue : num);
					}
				}
				if (VirtualizingStackPanel.ScrollTracer._targetName != null)
				{
					VirtualizingStackPanel.ScrollTracer.Enable();
				}
			}

			// Token: 0x060088E4 RID: 35044 RVA: 0x00253330 File Offset: 0x00251530
			private static void Enable()
			{
				if (VirtualizingStackPanel.ScrollTracer.IsEnabled)
				{
					return;
				}
				VirtualizingStackPanel.ScrollTracer._isEnabled = true;
				Application application = Application.Current;
				if (application != null)
				{
					application.Exit += VirtualizingStackPanel.ScrollTracer.OnApplicationExit;
					application.DispatcherUnhandledException += VirtualizingStackPanel.ScrollTracer.OnUnhandledException;
				}
			}

			// Token: 0x17001EF3 RID: 7923
			// (get) Token: 0x060088E5 RID: 35045 RVA: 0x00253378 File Offset: 0x00251578
			internal static bool IsEnabled
			{
				get
				{
					return VirtualizingStackPanel.ScrollTracer._isEnabled;
				}
			}

			// Token: 0x060088E6 RID: 35046 RVA: 0x00253380 File Offset: 0x00251580
			internal static bool SetTarget(object o)
			{
				ItemsControl itemsControl = o as ItemsControl;
				if (itemsControl != null || o == null)
				{
					List<Tuple<WeakReference<ItemsControl>, VirtualizingStackPanel.ScrollTracer.TraceList>> obj = VirtualizingStackPanel.ScrollTracer.s_TargetToTraceListMap;
					lock (obj)
					{
						VirtualizingStackPanel.ScrollTracer.CloseAllTraceLists();
						if (itemsControl != null)
						{
							VirtualizingStackPanel.ScrollTracer.Enable();
							VirtualizingStackPanel.ScrollTracer.AddToMap(itemsControl);
							VirtualizingStackPanel.ScrollTracingInfo nullInfo = VirtualizingStackPanel.ScrollTracer._nullInfo;
							int generation = nullInfo.Generation + 1;
							nullInfo.Generation = generation;
						}
					}
				}
				return itemsControl == o;
			}

			// Token: 0x060088E7 RID: 35047 RVA: 0x00041D30 File Offset: 0x0003FF30
			internal static void SetFileAndDepth(string filename, int flushDepth)
			{
				throw new NotSupportedException();
			}

			// Token: 0x060088E8 RID: 35048 RVA: 0x002533F4 File Offset: 0x002515F4
			private static void Flush()
			{
				List<Tuple<WeakReference<ItemsControl>, VirtualizingStackPanel.ScrollTracer.TraceList>> obj = VirtualizingStackPanel.ScrollTracer.s_TargetToTraceListMap;
				lock (obj)
				{
					int i = 0;
					int count = VirtualizingStackPanel.ScrollTracer.s_TargetToTraceListMap.Count;
					while (i < count)
					{
						VirtualizingStackPanel.ScrollTracer.s_TargetToTraceListMap[i].Item2.Flush(-1);
						i++;
					}
				}
			}

			// Token: 0x060088E9 RID: 35049 RVA: 0x0025345C File Offset: 0x0025165C
			private static void Mark(params object[] args)
			{
				VirtualizingStackPanel.ScrollTraceRecord record = new VirtualizingStackPanel.ScrollTraceRecord(VirtualizingStackPanel.ScrollTraceOp.Mark, null, -1, 0, 0, VirtualizingStackPanel.ScrollTracer.BuildDetail(args));
				List<Tuple<WeakReference<ItemsControl>, VirtualizingStackPanel.ScrollTracer.TraceList>> obj = VirtualizingStackPanel.ScrollTracer.s_TargetToTraceListMap;
				lock (obj)
				{
					int i = 0;
					int count = VirtualizingStackPanel.ScrollTracer.s_TargetToTraceListMap.Count;
					while (i < count)
					{
						VirtualizingStackPanel.ScrollTracer.s_TargetToTraceListMap[i].Item2.Add(record);
						i++;
					}
				}
			}

			// Token: 0x060088EA RID: 35050 RVA: 0x002534D8 File Offset: 0x002516D8
			internal static bool IsConfigured(VirtualizingStackPanel vsp)
			{
				VirtualizingStackPanel.ScrollTracingInfo value = VirtualizingStackPanel.ScrollTracingInfoField.GetValue(vsp);
				return value != null;
			}

			// Token: 0x060088EB RID: 35051 RVA: 0x002534F8 File Offset: 0x002516F8
			internal static void ConfigureTracing(VirtualizingStackPanel vsp, DependencyObject itemsOwner, object parentItem, ItemsControl itemsControl)
			{
				VirtualizingStackPanel.ScrollTracer scrollTracer = null;
				VirtualizingStackPanel.ScrollTracingInfo value = VirtualizingStackPanel.ScrollTracer._nullInfo;
				VirtualizingStackPanel.ScrollTracingInfo scrollTracingInfo = VirtualizingStackPanel.ScrollTracingInfoField.GetValue(vsp);
				if (scrollTracingInfo != null && scrollTracingInfo.Generation < VirtualizingStackPanel.ScrollTracer._nullInfo.Generation)
				{
					scrollTracingInfo = null;
				}
				if (parentItem == vsp)
				{
					if (scrollTracingInfo == null)
					{
						if (itemsOwner == itemsControl)
						{
							VirtualizingStackPanel.ScrollTracer.TraceList traceList = VirtualizingStackPanel.ScrollTracer.TraceListForItemsControl(itemsControl);
							if (traceList != null)
							{
								scrollTracer = new VirtualizingStackPanel.ScrollTracer(itemsControl, vsp, traceList);
							}
						}
						if (scrollTracer != null)
						{
							value = new VirtualizingStackPanel.ScrollTracingInfo(scrollTracer, VirtualizingStackPanel.ScrollTracer._nullInfo.Generation, 0, itemsOwner as FrameworkElement, null, null, 0);
						}
					}
				}
				else
				{
					VirtualizingStackPanel virtualizingStackPanel = VisualTreeHelper.GetParent(itemsOwner) as VirtualizingStackPanel;
					if (virtualizingStackPanel != null)
					{
						VirtualizingStackPanel.ScrollTracingInfo value2 = VirtualizingStackPanel.ScrollTracingInfoField.GetValue(virtualizingStackPanel);
						if (value2 != null)
						{
							scrollTracer = value2.ScrollTracer;
							if (scrollTracer != null)
							{
								ItemContainerGenerator itemContainerGenerator = virtualizingStackPanel.ItemContainerGenerator as ItemContainerGenerator;
								int num = (itemContainerGenerator != null) ? itemContainerGenerator.IndexFromContainer(itemsOwner, true) : -1;
								if (scrollTracingInfo == null)
								{
									value = new VirtualizingStackPanel.ScrollTracingInfo(scrollTracer, VirtualizingStackPanel.ScrollTracer._nullInfo.Generation, value2.Depth + 1, itemsOwner as FrameworkElement, virtualizingStackPanel, parentItem, num);
								}
								else if (object.Equals(parentItem, scrollTracingInfo.ParentItem))
								{
									if (num != scrollTracingInfo.ItemIndex)
									{
										VirtualizingStackPanel.ScrollTracer.Trace(vsp, VirtualizingStackPanel.ScrollTraceOp.ID, new object[]
										{
											"Index changed from ",
											scrollTracingInfo.ItemIndex,
											" to ",
											num
										});
										scrollTracingInfo.ChangeIndex(num);
									}
								}
								else
								{
									VirtualizingStackPanel.ScrollTracer.Trace(vsp, VirtualizingStackPanel.ScrollTraceOp.ID, new object[]
									{
										"Container recyled from ",
										scrollTracingInfo.ItemIndex,
										" to ",
										num
									});
									scrollTracingInfo.ChangeItem(parentItem);
									scrollTracingInfo.ChangeIndex(num);
								}
							}
						}
					}
				}
				if (scrollTracingInfo == null)
				{
					VirtualizingStackPanel.ScrollTracingInfoField.SetValue(vsp, value);
				}
			}

			// Token: 0x060088EC RID: 35052 RVA: 0x002536A8 File Offset: 0x002518A8
			internal static bool IsTracing(VirtualizingStackPanel vsp)
			{
				VirtualizingStackPanel.ScrollTracingInfo value = VirtualizingStackPanel.ScrollTracingInfoField.GetValue(vsp);
				return value != null && value.ScrollTracer != null;
			}

			// Token: 0x060088ED RID: 35053 RVA: 0x002536D0 File Offset: 0x002518D0
			internal static void Trace(VirtualizingStackPanel vsp, VirtualizingStackPanel.ScrollTraceOp op, params object[] args)
			{
				VirtualizingStackPanel.ScrollTracingInfo value = VirtualizingStackPanel.ScrollTracingInfoField.GetValue(vsp);
				VirtualizingStackPanel.ScrollTracer scrollTracer = value.ScrollTracer;
				if (VirtualizingStackPanel.ScrollTracer.ShouldIgnore(op, value))
				{
					return;
				}
				scrollTracer.AddTrace(vsp, op, value, args);
			}

			// Token: 0x060088EE RID: 35054 RVA: 0x00253704 File Offset: 0x00251904
			private static bool ShouldIgnore(VirtualizingStackPanel.ScrollTraceOp op, VirtualizingStackPanel.ScrollTracingInfo sti)
			{
				return op == VirtualizingStackPanel.ScrollTraceOp.NoOp;
			}

			// Token: 0x060088EF RID: 35055 RVA: 0x0025370C File Offset: 0x0025190C
			private static string DisplayType(object o)
			{
				StringBuilder stringBuilder = new StringBuilder();
				bool flag = false;
				bool flag2 = false;
				Type type = o.GetType();
				while (!flag2 && type != null)
				{
					if (flag)
					{
						stringBuilder.Append("/");
					}
					string text = type.ToString();
					flag2 = text.StartsWith("System.Windows.Controls.");
					if (flag2)
					{
						text = text.Substring(24);
					}
					stringBuilder.Append(text);
					flag = true;
					type = type.BaseType;
				}
				return stringBuilder.ToString();
			}

			// Token: 0x060088F0 RID: 35056 RVA: 0x00253784 File Offset: 0x00251984
			private static string BuildDetail(object[] args)
			{
				int num = (args != null) ? args.Length : 0;
				if (num == 0)
				{
					return string.Empty;
				}
				return string.Format(CultureInfo.InvariantCulture, VirtualizingStackPanel.ScrollTracer.s_format[num], args);
			}

			// Token: 0x060088F1 RID: 35057 RVA: 0x002537B6 File Offset: 0x002519B6
			private void Push()
			{
				this._depth++;
			}

			// Token: 0x060088F2 RID: 35058 RVA: 0x002537C6 File Offset: 0x002519C6
			private void Pop()
			{
				this._depth--;
			}

			// Token: 0x060088F3 RID: 35059 RVA: 0x002537D6 File Offset: 0x002519D6
			private void Pop(VirtualizingStackPanel.ScrollTraceRecord record)
			{
				this._depth--;
				record.ChangeOpDepth(-1);
			}

			// Token: 0x060088F4 RID: 35060 RVA: 0x002537ED File Offset: 0x002519ED
			private ScrollTracer(ItemsControl itemsControl, VirtualizingStackPanel vsp, VirtualizingStackPanel.ScrollTracer.TraceList traceList)
			{
				this._wrIC = new WeakReference<ItemsControl>(itemsControl);
				this._traceList = traceList;
				this.IdentifyTrace(itemsControl, vsp);
			}

			// Token: 0x060088F5 RID: 35061 RVA: 0x00253818 File Offset: 0x00251A18
			private static void OnApplicationExit(object sender, ExitEventArgs e)
			{
				Application application = sender as Application;
				if (application != null)
				{
					application.Exit -= VirtualizingStackPanel.ScrollTracer.OnApplicationExit;
				}
				VirtualizingStackPanel.ScrollTracer.CloseAllTraceLists();
			}

			// Token: 0x060088F6 RID: 35062 RVA: 0x00253848 File Offset: 0x00251A48
			private static void OnUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
			{
				Application application = sender as Application;
				if (application != null)
				{
					application.DispatcherUnhandledException -= VirtualizingStackPanel.ScrollTracer.OnUnhandledException;
				}
				VirtualizingStackPanel.ScrollTracer.CloseAllTraceLists();
			}

			// Token: 0x060088F7 RID: 35063 RVA: 0x00253878 File Offset: 0x00251A78
			private void IdentifyTrace(ItemsControl ic, VirtualizingStackPanel vsp)
			{
				this.AddTrace(null, VirtualizingStackPanel.ScrollTraceOp.ID, VirtualizingStackPanel.ScrollTracer._nullInfo, new object[]
				{
					VirtualizingStackPanel.ScrollTracer.DisplayType(ic),
					"Items:",
					ic.Items.Count,
					"Panel:",
					VirtualizingStackPanel.ScrollTracer.DisplayType(vsp),
					"Time:",
					DateTime.Now
				});
				this.AddTrace(null, VirtualizingStackPanel.ScrollTraceOp.ID, VirtualizingStackPanel.ScrollTracer._nullInfo, new object[]
				{
					"IsVirt:",
					VirtualizingPanel.GetIsVirtualizing(ic),
					"IsVirtWhenGroup:",
					VirtualizingPanel.GetIsVirtualizingWhenGrouping(ic),
					"VirtMode:",
					VirtualizingPanel.GetVirtualizationMode(ic),
					"ScrollUnit:",
					VirtualizingPanel.GetScrollUnit(ic),
					"CacheLen:",
					VirtualizingPanel.GetCacheLength(ic),
					VirtualizingPanel.GetCacheLengthUnit(ic)
				});
				DpiScale dpi = vsp.GetDpi();
				this.AddTrace(null, VirtualizingStackPanel.ScrollTraceOp.ID, VirtualizingStackPanel.ScrollTracer._nullInfo, new object[]
				{
					"DPIScale:",
					dpi.DpiScaleX,
					dpi.DpiScaleY,
					"UseLayoutRounding:",
					vsp.UseLayoutRounding,
					"Rounding Quantum:",
					1.0 / dpi.DpiScaleY
				});
				this.AddTrace(null, VirtualizingStackPanel.ScrollTraceOp.ID, VirtualizingStackPanel.ScrollTracer._nullInfo, new object[]
				{
					"CanContentScroll:",
					ScrollViewer.GetCanContentScroll(ic),
					"IsDeferredScrolling:",
					ScrollViewer.GetIsDeferredScrollingEnabled(ic),
					"PanningMode:",
					ScrollViewer.GetPanningMode(ic),
					"HSBVisibility:",
					ScrollViewer.GetHorizontalScrollBarVisibility(ic),
					"VSBVisibility:",
					ScrollViewer.GetVerticalScrollBarVisibility(ic)
				});
				DataGrid dataGrid = ic as DataGrid;
				if (dataGrid != null)
				{
					this.AddTrace(null, VirtualizingStackPanel.ScrollTraceOp.ID, VirtualizingStackPanel.ScrollTracer._nullInfo, new object[]
					{
						"EnableRowVirt:",
						dataGrid.EnableRowVirtualization,
						"EnableColVirt:",
						dataGrid.EnableColumnVirtualization,
						"Columns:",
						dataGrid.Columns.Count,
						"FrozenCols:",
						dataGrid.FrozenColumnCount
					});
				}
			}

			// Token: 0x060088F8 RID: 35064 RVA: 0x00253AE8 File Offset: 0x00251CE8
			private void AddTrace(VirtualizingStackPanel vsp, VirtualizingStackPanel.ScrollTraceOp op, VirtualizingStackPanel.ScrollTracingInfo sti, params object[] args)
			{
				if (op == VirtualizingStackPanel.ScrollTraceOp.LayoutUpdated)
				{
					int num = this._luCount + 1;
					this._luCount = num;
					if (num > VirtualizingStackPanel.ScrollTracer._luThreshold)
					{
						this.AddTrace(null, VirtualizingStackPanel.ScrollTraceOp.ID, VirtualizingStackPanel.ScrollTracer._nullInfo, new object[]
						{
							"Inactive at",
							DateTime.Now
						});
						ItemsControl itemsControl;
						if (this._wrIC.TryGetTarget(out itemsControl))
						{
							itemsControl.LayoutUpdated -= this.OnLayoutUpdated;
						}
						this._traceList.FlushAndClear();
						this._luCount = -1;
					}
				}
				else
				{
					int luCount = this._luCount;
					this._luCount = 0;
					if (luCount < 0)
					{
						this.AddTrace(null, VirtualizingStackPanel.ScrollTraceOp.ID, VirtualizingStackPanel.ScrollTracer._nullInfo, new object[]
						{
							"Reactivate at",
							DateTime.Now
						});
						ItemsControl itemsControl2;
						if (this._wrIC.TryGetTarget(out itemsControl2))
						{
							itemsControl2.LayoutUpdated += this.OnLayoutUpdated;
						}
					}
				}
				VirtualizingStackPanel.ScrollTraceRecord scrollTraceRecord = new VirtualizingStackPanel.ScrollTraceRecord(op, vsp, sti.Depth, sti.ItemIndex, this._depth, VirtualizingStackPanel.ScrollTracer.BuildDetail(args));
				this._traceList.Add(scrollTraceRecord);
				switch (op)
				{
				case VirtualizingStackPanel.ScrollTraceOp.BeginMeasure:
					this.Push();
					break;
				case VirtualizingStackPanel.ScrollTraceOp.EndMeasure:
					this.Pop(scrollTraceRecord);
					scrollTraceRecord.Snapshot = vsp.TakeSnapshot();
					this._traceList.Flush(sti.Depth);
					break;
				case VirtualizingStackPanel.ScrollTraceOp.BeginArrange:
					this.Push();
					break;
				case VirtualizingStackPanel.ScrollTraceOp.EndArrange:
					this.Pop(scrollTraceRecord);
					scrollTraceRecord.Snapshot = vsp.TakeSnapshot();
					this._traceList.Flush(sti.Depth);
					break;
				case VirtualizingStackPanel.ScrollTraceOp.BSetAnchor:
					this.Push();
					break;
				case VirtualizingStackPanel.ScrollTraceOp.ESetAnchor:
					this.Pop(scrollTraceRecord);
					break;
				case VirtualizingStackPanel.ScrollTraceOp.BOnAnchor:
					this.Push();
					break;
				case VirtualizingStackPanel.ScrollTraceOp.ROnAnchor:
					this.Pop(scrollTraceRecord);
					break;
				case VirtualizingStackPanel.ScrollTraceOp.SOnAnchor:
					this.Pop();
					break;
				case VirtualizingStackPanel.ScrollTraceOp.EOnAnchor:
					this.Pop(scrollTraceRecord);
					break;
				case VirtualizingStackPanel.ScrollTraceOp.RecycleChildren:
				case VirtualizingStackPanel.ScrollTraceOp.RemoveChildren:
					scrollTraceRecord.RevirtualizedChildren = (args[2] as List<string>);
					break;
				}
				if (VirtualizingStackPanel.ScrollTracer._flushDepth < 0)
				{
					this._traceList.Flush(VirtualizingStackPanel.ScrollTracer._flushDepth);
				}
			}

			// Token: 0x060088F9 RID: 35065 RVA: 0x00253CF6 File Offset: 0x00251EF6
			private void OnLayoutUpdated(object sender, EventArgs e)
			{
				this.AddTrace(null, VirtualizingStackPanel.ScrollTraceOp.LayoutUpdated, VirtualizingStackPanel.ScrollTracer._nullInfo, null);
			}

			// Token: 0x060088FA RID: 35066 RVA: 0x00253D08 File Offset: 0x00251F08
			private static VirtualizingStackPanel.ScrollTracer.TraceList TraceListForItemsControl(ItemsControl target)
			{
				VirtualizingStackPanel.ScrollTracer.TraceList traceList = null;
				List<Tuple<WeakReference<ItemsControl>, VirtualizingStackPanel.ScrollTracer.TraceList>> obj = VirtualizingStackPanel.ScrollTracer.s_TargetToTraceListMap;
				lock (obj)
				{
					int i = 0;
					int count = VirtualizingStackPanel.ScrollTracer.s_TargetToTraceListMap.Count;
					while (i < count)
					{
						WeakReference<ItemsControl> item = VirtualizingStackPanel.ScrollTracer.s_TargetToTraceListMap[i].Item1;
						ItemsControl itemsControl;
						if (item.TryGetTarget(out itemsControl) && itemsControl == target)
						{
							traceList = VirtualizingStackPanel.ScrollTracer.s_TargetToTraceListMap[i].Item2;
							break;
						}
						i++;
					}
					if (traceList == null && target.Name == VirtualizingStackPanel.ScrollTracer._targetName)
					{
						traceList = VirtualizingStackPanel.ScrollTracer.AddToMap(target);
					}
				}
				return traceList;
			}

			// Token: 0x060088FB RID: 35067 RVA: 0x00253DB0 File Offset: 0x00251FB0
			private static VirtualizingStackPanel.ScrollTracer.TraceList AddToMap(ItemsControl target)
			{
				VirtualizingStackPanel.ScrollTracer.TraceList traceList = null;
				List<Tuple<WeakReference<ItemsControl>, VirtualizingStackPanel.ScrollTracer.TraceList>> obj = VirtualizingStackPanel.ScrollTracer.s_TargetToTraceListMap;
				lock (obj)
				{
					VirtualizingStackPanel.ScrollTracer.PurgeMap();
					VirtualizingStackPanel.ScrollTracer.s_seqno++;
					string text = VirtualizingStackPanel.ScrollTracer._fileName;
					if (string.IsNullOrEmpty(text) || text == "default")
					{
						text = "ScrollTrace.stf";
					}
					if (text != "none" && VirtualizingStackPanel.ScrollTracer.s_seqno > 1)
					{
						int num = text.LastIndexOf(".", StringComparison.Ordinal);
						if (num < 0)
						{
							num = text.Length;
						}
						text = text.Substring(0, num) + VirtualizingStackPanel.ScrollTracer.s_seqno.ToString() + text.Substring(num);
					}
					traceList = new VirtualizingStackPanel.ScrollTracer.TraceList(text);
					VirtualizingStackPanel.ScrollTracer.s_TargetToTraceListMap.Add(new Tuple<WeakReference<ItemsControl>, VirtualizingStackPanel.ScrollTracer.TraceList>(new WeakReference<ItemsControl>(target), traceList));
				}
				return traceList;
			}

			// Token: 0x060088FC RID: 35068 RVA: 0x00253E8C File Offset: 0x0025208C
			private static void CloseAllTraceLists()
			{
				int i = 0;
				int count = VirtualizingStackPanel.ScrollTracer.s_TargetToTraceListMap.Count;
				while (i < count)
				{
					VirtualizingStackPanel.ScrollTracer.TraceList item = VirtualizingStackPanel.ScrollTracer.s_TargetToTraceListMap[i].Item2;
					item.FlushAndClose();
					i++;
				}
				VirtualizingStackPanel.ScrollTracer.s_TargetToTraceListMap.Clear();
			}

			// Token: 0x060088FD RID: 35069 RVA: 0x00253ED4 File Offset: 0x002520D4
			private static void PurgeMap()
			{
				for (int i = 0; i < VirtualizingStackPanel.ScrollTracer.s_TargetToTraceListMap.Count; i++)
				{
					WeakReference<ItemsControl> item = VirtualizingStackPanel.ScrollTracer.s_TargetToTraceListMap[i].Item1;
					ItemsControl itemsControl;
					if (!item.TryGetTarget(out itemsControl))
					{
						VirtualizingStackPanel.ScrollTracer.TraceList item2 = VirtualizingStackPanel.ScrollTracer.s_TargetToTraceListMap[i].Item2;
						item2.FlushAndClose();
						VirtualizingStackPanel.ScrollTracer.s_TargetToTraceListMap.RemoveAt(i);
						i--;
					}
				}
			}

			// Token: 0x040045C2 RID: 17858
			private const int s_StfFormatVersion = 2;

			// Token: 0x040045C3 RID: 17859
			private const int s_MaxTraceRecords = 30000;

			// Token: 0x040045C4 RID: 17860
			private const int s_MinTraceRecords = 5000;

			// Token: 0x040045C5 RID: 17861
			private const int s_DefaultLayoutUpdatedThreshold = 20;

			// Token: 0x040045C6 RID: 17862
			private static string _targetName;

			// Token: 0x040045C7 RID: 17863
			private static bool _isEnabled;

			// Token: 0x040045C8 RID: 17864
			private static string _fileName;

			// Token: 0x040045C9 RID: 17865
			private static int _flushDepth;

			// Token: 0x040045CA RID: 17866
			private static int _luThreshold;

			// Token: 0x040045CB RID: 17867
			private static VirtualizingStackPanel.ScrollTracingInfo _nullInfo = new VirtualizingStackPanel.ScrollTracingInfo(null, 0, -1, null, null, null, -1);

			// Token: 0x040045CC RID: 17868
			private static string[] s_format = new string[]
			{
				"",
				"{0}",
				"{0} {1}",
				"{0} {1} {2}",
				"{0} {1} {2} {3}",
				"{0} {1} {2} {3} {4} ",
				"{0} {1} {2} {3} {4} {5}",
				"{0} {1} {2} {3} {4} {5} {6}",
				"{0} {1} {2} {3} {4} {5} {6} {7}",
				"{0} {1} {2} {3} {4} {5} {6} {7} {8}",
				"{0} {1} {2} {3} {4} {5} {6} {7} {8} {9}",
				"{0} {1} {2} {3} {4} {5} {6} {7} {8} {9} {10}",
				"{0} {1} {2} {3} {4} {5} {6} {7} {8} {9} {10} {11}",
				"{0} {1} {2} {3} {4} {5} {6} {7} {8} {9} {10} {11} {12}",
				"{0} {1} {2} {3} {4} {5} {6} {7} {8} {9} {10} {11} {12} {13}"
			};

			// Token: 0x040045CD RID: 17869
			private int _depth;

			// Token: 0x040045CE RID: 17870
			private VirtualizingStackPanel.ScrollTracer.TraceList _traceList;

			// Token: 0x040045CF RID: 17871
			private WeakReference<ItemsControl> _wrIC;

			// Token: 0x040045D0 RID: 17872
			private int _luCount = -1;

			// Token: 0x040045D1 RID: 17873
			private static List<Tuple<WeakReference<ItemsControl>, VirtualizingStackPanel.ScrollTracer.TraceList>> s_TargetToTraceListMap = new List<Tuple<WeakReference<ItemsControl>, VirtualizingStackPanel.ScrollTracer.TraceList>>();

			// Token: 0x040045D2 RID: 17874
			private static int s_seqno;

			// Token: 0x02000BAE RID: 2990
			private class TraceList
			{
				// Token: 0x060091E3 RID: 37347 RVA: 0x0025F4D3 File Offset: 0x0025D6D3
				internal TraceList(string filename)
				{
					if (filename != "none")
					{
						this._writer = new BinaryWriter(File.Open(filename, FileMode.Create));
						this._writer.Write(2);
					}
				}

				// Token: 0x060091E4 RID: 37348 RVA: 0x0025F511 File Offset: 0x0025D711
				internal void Add(VirtualizingStackPanel.ScrollTraceRecord record)
				{
					this._traceList.Add(record);
				}

				// Token: 0x060091E5 RID: 37349 RVA: 0x0025F520 File Offset: 0x0025D720
				internal void Flush(int depth)
				{
					if (this._writer != null && depth <= VirtualizingStackPanel.ScrollTracer._flushDepth)
					{
						while (this._flushIndex < this._traceList.Count)
						{
							this._traceList[this._flushIndex].Write(this._writer);
							this._flushIndex++;
						}
						this._writer.Flush();
						if (this._flushIndex > 30000)
						{
							int count = this._flushIndex - 5000;
							this._traceList.RemoveRange(0, count);
							this._flushIndex = this._traceList.Count;
						}
					}
				}

				// Token: 0x060091E6 RID: 37350 RVA: 0x0025F5C5 File Offset: 0x0025D7C5
				internal void FlushAndClose()
				{
					if (this._writer != null)
					{
						this.Flush(VirtualizingStackPanel.ScrollTracer._flushDepth);
						this._writer.Close();
						this._writer = null;
					}
				}

				// Token: 0x060091E7 RID: 37351 RVA: 0x0025F5EC File Offset: 0x0025D7EC
				internal void FlushAndClear()
				{
					if (this._writer != null)
					{
						this.Flush(VirtualizingStackPanel.ScrollTracer._flushDepth);
						this._traceList.Clear();
						this._flushIndex = 0;
					}
				}

				// Token: 0x04004EDF RID: 20191
				private List<VirtualizingStackPanel.ScrollTraceRecord> _traceList = new List<VirtualizingStackPanel.ScrollTraceRecord>();

				// Token: 0x04004EE0 RID: 20192
				private BinaryWriter _writer;

				// Token: 0x04004EE1 RID: 20193
				private int _flushIndex;
			}
		}

		// Token: 0x020009D4 RID: 2516
		private class ScrollTracingInfo
		{
			// Token: 0x17001EF4 RID: 7924
			// (get) Token: 0x060088FE RID: 35070 RVA: 0x00253F37 File Offset: 0x00252137
			// (set) Token: 0x060088FF RID: 35071 RVA: 0x00253F3F File Offset: 0x0025213F
			internal VirtualizingStackPanel.ScrollTracer ScrollTracer { get; private set; }

			// Token: 0x17001EF5 RID: 7925
			// (get) Token: 0x06008900 RID: 35072 RVA: 0x00253F48 File Offset: 0x00252148
			// (set) Token: 0x06008901 RID: 35073 RVA: 0x00253F50 File Offset: 0x00252150
			internal int Generation { get; set; }

			// Token: 0x17001EF6 RID: 7926
			// (get) Token: 0x06008902 RID: 35074 RVA: 0x00253F59 File Offset: 0x00252159
			// (set) Token: 0x06008903 RID: 35075 RVA: 0x00253F61 File Offset: 0x00252161
			internal int Depth { get; private set; }

			// Token: 0x17001EF7 RID: 7927
			// (get) Token: 0x06008904 RID: 35076 RVA: 0x00253F6A File Offset: 0x0025216A
			// (set) Token: 0x06008905 RID: 35077 RVA: 0x00253F72 File Offset: 0x00252172
			internal FrameworkElement Owner { get; private set; }

			// Token: 0x17001EF8 RID: 7928
			// (get) Token: 0x06008906 RID: 35078 RVA: 0x00253F7B File Offset: 0x0025217B
			// (set) Token: 0x06008907 RID: 35079 RVA: 0x00253F83 File Offset: 0x00252183
			internal VirtualizingStackPanel Parent { get; private set; }

			// Token: 0x17001EF9 RID: 7929
			// (get) Token: 0x06008908 RID: 35080 RVA: 0x00253F8C File Offset: 0x0025218C
			// (set) Token: 0x06008909 RID: 35081 RVA: 0x00253F94 File Offset: 0x00252194
			internal object ParentItem { get; private set; }

			// Token: 0x17001EFA RID: 7930
			// (get) Token: 0x0600890A RID: 35082 RVA: 0x00253F9D File Offset: 0x0025219D
			// (set) Token: 0x0600890B RID: 35083 RVA: 0x00253FA5 File Offset: 0x002521A5
			internal int ItemIndex { get; private set; }

			// Token: 0x0600890C RID: 35084 RVA: 0x00253FAE File Offset: 0x002521AE
			internal ScrollTracingInfo(VirtualizingStackPanel.ScrollTracer tracer, int generation, int depth, FrameworkElement owner, VirtualizingStackPanel parent, object parentItem, int itemIndex)
			{
				this.ScrollTracer = tracer;
				this.Generation = generation;
				this.Depth = depth;
				this.Owner = owner;
				this.Parent = parent;
				this.ParentItem = parentItem;
				this.ItemIndex = itemIndex;
			}

			// Token: 0x0600890D RID: 35085 RVA: 0x00253FEB File Offset: 0x002521EB
			internal void ChangeItem(object newItem)
			{
				this.ParentItem = newItem;
			}

			// Token: 0x0600890E RID: 35086 RVA: 0x00253FF4 File Offset: 0x002521F4
			internal void ChangeIndex(int newIndex)
			{
				this.ItemIndex = newIndex;
			}
		}

		// Token: 0x020009D5 RID: 2517
		private enum ScrollTraceOp : ushort
		{
			// Token: 0x040045DB RID: 17883
			NoOp,
			// Token: 0x040045DC RID: 17884
			ID,
			// Token: 0x040045DD RID: 17885
			Mark,
			// Token: 0x040045DE RID: 17886
			LineUp,
			// Token: 0x040045DF RID: 17887
			LineDown,
			// Token: 0x040045E0 RID: 17888
			LineLeft,
			// Token: 0x040045E1 RID: 17889
			LineRight,
			// Token: 0x040045E2 RID: 17890
			PageUp,
			// Token: 0x040045E3 RID: 17891
			PageDown,
			// Token: 0x040045E4 RID: 17892
			PageLeft,
			// Token: 0x040045E5 RID: 17893
			PageRight,
			// Token: 0x040045E6 RID: 17894
			MouseWheelUp,
			// Token: 0x040045E7 RID: 17895
			MouseWheelDown,
			// Token: 0x040045E8 RID: 17896
			MouseWheelLeft,
			// Token: 0x040045E9 RID: 17897
			MouseWheelRight,
			// Token: 0x040045EA RID: 17898
			SetHorizontalOffset,
			// Token: 0x040045EB RID: 17899
			SetVerticalOffset,
			// Token: 0x040045EC RID: 17900
			SetHOff,
			// Token: 0x040045ED RID: 17901
			SetVOff,
			// Token: 0x040045EE RID: 17902
			MakeVisible,
			// Token: 0x040045EF RID: 17903
			BeginMeasure,
			// Token: 0x040045F0 RID: 17904
			EndMeasure,
			// Token: 0x040045F1 RID: 17905
			BeginArrange,
			// Token: 0x040045F2 RID: 17906
			EndArrange,
			// Token: 0x040045F3 RID: 17907
			LayoutUpdated,
			// Token: 0x040045F4 RID: 17908
			BSetAnchor,
			// Token: 0x040045F5 RID: 17909
			ESetAnchor,
			// Token: 0x040045F6 RID: 17910
			BOnAnchor,
			// Token: 0x040045F7 RID: 17911
			ROnAnchor,
			// Token: 0x040045F8 RID: 17912
			SOnAnchor,
			// Token: 0x040045F9 RID: 17913
			EOnAnchor,
			// Token: 0x040045FA RID: 17914
			RecycleChildren,
			// Token: 0x040045FB RID: 17915
			RemoveChildren,
			// Token: 0x040045FC RID: 17916
			ItemsChanged,
			// Token: 0x040045FD RID: 17917
			IsScrollActive,
			// Token: 0x040045FE RID: 17918
			CFCIV,
			// Token: 0x040045FF RID: 17919
			CFIVIO,
			// Token: 0x04004600 RID: 17920
			SyncAveSize,
			// Token: 0x04004601 RID: 17921
			StoreSubstOffset,
			// Token: 0x04004602 RID: 17922
			UseSubstOffset,
			// Token: 0x04004603 RID: 17923
			ReviseArrangeOffset,
			// Token: 0x04004604 RID: 17924
			SVSDBegin,
			// Token: 0x04004605 RID: 17925
			AdjustOffset,
			// Token: 0x04004606 RID: 17926
			ScrollBarChangeVisibility,
			// Token: 0x04004607 RID: 17927
			RemeasureCycle,
			// Token: 0x04004608 RID: 17928
			RemeasureEndExpandViewport,
			// Token: 0x04004609 RID: 17929
			RemeasureEndChangeOffset,
			// Token: 0x0400460A RID: 17930
			RemeasureEndExtentChanged,
			// Token: 0x0400460B RID: 17931
			RemeasureRatio,
			// Token: 0x0400460C RID: 17932
			RecomputeFirstOffset,
			// Token: 0x0400460D RID: 17933
			LastPageSizeChange,
			// Token: 0x0400460E RID: 17934
			SVSDEnd,
			// Token: 0x0400460F RID: 17935
			SetContainerSize,
			// Token: 0x04004610 RID: 17936
			SizeChangeDuringAnchorScroll
		}

		// Token: 0x020009D6 RID: 2518
		private class ScrollTraceRecord
		{
			// Token: 0x0600890F RID: 35087 RVA: 0x00253FFD File Offset: 0x002521FD
			internal ScrollTraceRecord(VirtualizingStackPanel.ScrollTraceOp op, VirtualizingStackPanel vsp, int vspDepth, int itemIndex, int opDepth, string detail)
			{
				this.Op = op;
				this.VSP = vsp;
				this.VDepth = vspDepth;
				this.ItemIndex = itemIndex;
				this.OpDepth = opDepth;
				this.Detail = detail;
			}

			// Token: 0x17001EFB RID: 7931
			// (get) Token: 0x06008910 RID: 35088 RVA: 0x00254032 File Offset: 0x00252232
			// (set) Token: 0x06008911 RID: 35089 RVA: 0x0025403A File Offset: 0x0025223A
			internal VirtualizingStackPanel.ScrollTraceOp Op { get; private set; }

			// Token: 0x17001EFC RID: 7932
			// (get) Token: 0x06008912 RID: 35090 RVA: 0x00254043 File Offset: 0x00252243
			// (set) Token: 0x06008913 RID: 35091 RVA: 0x0025404B File Offset: 0x0025224B
			internal int OpDepth { get; private set; }

			// Token: 0x17001EFD RID: 7933
			// (get) Token: 0x06008914 RID: 35092 RVA: 0x00254054 File Offset: 0x00252254
			// (set) Token: 0x06008915 RID: 35093 RVA: 0x0025405C File Offset: 0x0025225C
			internal VirtualizingStackPanel VSP { get; private set; }

			// Token: 0x17001EFE RID: 7934
			// (get) Token: 0x06008916 RID: 35094 RVA: 0x00254065 File Offset: 0x00252265
			// (set) Token: 0x06008917 RID: 35095 RVA: 0x0025406D File Offset: 0x0025226D
			internal int VDepth { get; private set; }

			// Token: 0x17001EFF RID: 7935
			// (get) Token: 0x06008918 RID: 35096 RVA: 0x00254076 File Offset: 0x00252276
			// (set) Token: 0x06008919 RID: 35097 RVA: 0x0025407E File Offset: 0x0025227E
			internal int ItemIndex { get; private set; }

			// Token: 0x17001F00 RID: 7936
			// (get) Token: 0x0600891A RID: 35098 RVA: 0x00254087 File Offset: 0x00252287
			// (set) Token: 0x0600891B RID: 35099 RVA: 0x0025408F File Offset: 0x0025228F
			internal string Detail { get; set; }

			// Token: 0x17001F01 RID: 7937
			// (get) Token: 0x0600891C RID: 35100 RVA: 0x00254098 File Offset: 0x00252298
			// (set) Token: 0x0600891D RID: 35101 RVA: 0x002540A5 File Offset: 0x002522A5
			internal VirtualizingStackPanel.Snapshot Snapshot
			{
				get
				{
					return this._extraData as VirtualizingStackPanel.Snapshot;
				}
				set
				{
					this._extraData = value;
				}
			}

			// Token: 0x17001F02 RID: 7938
			// (get) Token: 0x0600891E RID: 35102 RVA: 0x002540AE File Offset: 0x002522AE
			// (set) Token: 0x0600891F RID: 35103 RVA: 0x002540A5 File Offset: 0x002522A5
			internal List<string> RevirtualizedChildren
			{
				get
				{
					return this._extraData as List<string>;
				}
				set
				{
					this._extraData = value;
				}
			}

			// Token: 0x06008920 RID: 35104 RVA: 0x002540BB File Offset: 0x002522BB
			internal void ChangeOpDepth(int delta)
			{
				this.OpDepth += delta;
			}

			// Token: 0x06008921 RID: 35105 RVA: 0x002540CC File Offset: 0x002522CC
			public override string ToString()
			{
				return string.Format(CultureInfo.InvariantCulture, "{0} {1} {2} {3} {4}", new object[]
				{
					this.OpDepth,
					this.VDepth,
					this.ItemIndex,
					this.Op,
					this.Detail
				});
			}

			// Token: 0x06008922 RID: 35106 RVA: 0x00254130 File Offset: 0x00252330
			internal void Write(BinaryWriter writer)
			{
				writer.Write((ushort)this.Op);
				writer.Write(this.OpDepth);
				writer.Write(this.VDepth);
				writer.Write(this.ItemIndex);
				writer.Write(this.Detail);
				if (this.Snapshot != null)
				{
					writer.Write(1);
					this.Snapshot.Write(writer, this.VSP);
					return;
				}
				List<string> revirtualizedChildren;
				if ((revirtualizedChildren = this.RevirtualizedChildren) != null)
				{
					int count = revirtualizedChildren.Count;
					writer.Write(2);
					writer.Write(count);
					for (int i = 0; i < count; i++)
					{
						writer.Write(revirtualizedChildren[i]);
					}
					return;
				}
				writer.Write(0);
			}

			// Token: 0x04004617 RID: 17943
			private object _extraData;
		}

		// Token: 0x020009D7 RID: 2519
		private class Snapshot
		{
			// Token: 0x06008923 RID: 35107 RVA: 0x002541E0 File Offset: 0x002523E0
			internal void Write(BinaryWriter writer, VirtualizingStackPanel vsp)
			{
				if (this._scrollData == null)
				{
					writer.Write(false);
				}
				else
				{
					writer.Write(true);
					VirtualizingStackPanel.Snapshot.WriteVector(writer, ref this._scrollData._offset);
					VirtualizingStackPanel.Snapshot.WriteSize(writer, ref this._scrollData._extent);
					VirtualizingStackPanel.Snapshot.WriteVector(writer, ref this._scrollData._computedOffset);
				}
				writer.Write((byte)this._boolFieldStore);
				writer.Write(!(this._areContainersUniformlySized == false));
				writer.Write((this._uniformOrAverageContainerSize != null) ? this._uniformOrAverageContainerSize.Value : -1.0);
				writer.Write((this._uniformOrAverageContainerPixelSize != null) ? this._uniformOrAverageContainerPixelSize.Value : -1.0);
				writer.Write(this._firstItemInExtendedViewportChildIndex);
				writer.Write(this._firstItemInExtendedViewportIndex);
				writer.Write(this._firstItemInExtendedViewportOffset);
				writer.Write(this._actualItemsInExtendedViewportCount);
				VirtualizingStackPanel.Snapshot.WriteRect(writer, ref this._viewport);
				writer.Write(this._itemsInExtendedViewportCount);
				VirtualizingStackPanel.Snapshot.WriteRect(writer, ref this._extendedViewport);
				VirtualizingStackPanel.Snapshot.WriteSize(writer, ref this._previousStackPixelSizeInViewport);
				VirtualizingStackPanel.Snapshot.WriteSize(writer, ref this._previousStackLogicalSizeInViewport);
				VirtualizingStackPanel.Snapshot.WriteSize(writer, ref this._previousStackPixelSizeInCacheBeforeViewport);
				writer.Write(vsp.ContainerPath(this._firstContainerInViewport));
				writer.Write(this._firstContainerOffsetFromViewport);
				writer.Write(this._expectedDistanceBetweenViewports);
				writer.Write(vsp.ContainerPath(this._bringIntoViewContainer));
				writer.Write(vsp.ContainerPath(this._bringIntoViewLeafContainer));
				writer.Write(this._realizedChildren.Count);
				for (int i = 0; i < this._realizedChildren.Count; i++)
				{
					VirtualizingStackPanel.ChildInfo childInfo = this._realizedChildren[i];
					writer.Write(childInfo._itemIndex);
					VirtualizingStackPanel.Snapshot.WriteSize(writer, ref childInfo._desiredSize);
					VirtualizingStackPanel.Snapshot.WriteRect(writer, ref childInfo._arrangeRect);
					VirtualizingStackPanel.Snapshot.WriteThickness(writer, ref childInfo._inset);
				}
				if (this._effectiveOffsets != null)
				{
					writer.Write(this._effectiveOffsets.Count);
					using (List<double>.Enumerator enumerator = this._effectiveOffsets.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							double value = enumerator.Current;
							writer.Write(value);
						}
						return;
					}
				}
				writer.Write(0);
			}

			// Token: 0x06008924 RID: 35108 RVA: 0x00254454 File Offset: 0x00252654
			private static void WriteRect(BinaryWriter writer, ref Rect rect)
			{
				writer.Write(rect.Left);
				writer.Write(rect.Top);
				writer.Write(rect.Width);
				writer.Write(rect.Height);
			}

			// Token: 0x06008925 RID: 35109 RVA: 0x00254486 File Offset: 0x00252686
			private static void WriteSize(BinaryWriter writer, ref Size size)
			{
				writer.Write(size.Width);
				writer.Write(size.Height);
			}

			// Token: 0x06008926 RID: 35110 RVA: 0x002544A0 File Offset: 0x002526A0
			private static void WriteVector(BinaryWriter writer, ref Vector vector)
			{
				writer.Write(vector.X);
				writer.Write(vector.Y);
			}

			// Token: 0x06008927 RID: 35111 RVA: 0x002544BA File Offset: 0x002526BA
			private static void WriteThickness(BinaryWriter writer, ref Thickness thickness)
			{
				writer.Write(thickness.Left);
				writer.Write(thickness.Top);
				writer.Write(thickness.Right);
				writer.Write(thickness.Bottom);
			}

			// Token: 0x04004618 RID: 17944
			internal VirtualizingStackPanel.ScrollData _scrollData;

			// Token: 0x04004619 RID: 17945
			internal VirtualizingStackPanel.BoolField _boolFieldStore;

			// Token: 0x0400461A RID: 17946
			internal bool? _areContainersUniformlySized;

			// Token: 0x0400461B RID: 17947
			internal double? _uniformOrAverageContainerSize;

			// Token: 0x0400461C RID: 17948
			internal double? _uniformOrAverageContainerPixelSize;

			// Token: 0x0400461D RID: 17949
			internal List<VirtualizingStackPanel.ChildInfo> _realizedChildren;

			// Token: 0x0400461E RID: 17950
			internal int _firstItemInExtendedViewportChildIndex;

			// Token: 0x0400461F RID: 17951
			internal int _firstItemInExtendedViewportIndex;

			// Token: 0x04004620 RID: 17952
			internal double _firstItemInExtendedViewportOffset;

			// Token: 0x04004621 RID: 17953
			internal int _actualItemsInExtendedViewportCount;

			// Token: 0x04004622 RID: 17954
			internal Rect _viewport;

			// Token: 0x04004623 RID: 17955
			internal int _itemsInExtendedViewportCount;

			// Token: 0x04004624 RID: 17956
			internal Rect _extendedViewport;

			// Token: 0x04004625 RID: 17957
			internal Size _previousStackPixelSizeInViewport;

			// Token: 0x04004626 RID: 17958
			internal Size _previousStackLogicalSizeInViewport;

			// Token: 0x04004627 RID: 17959
			internal Size _previousStackPixelSizeInCacheBeforeViewport;

			// Token: 0x04004628 RID: 17960
			internal FrameworkElement _firstContainerInViewport;

			// Token: 0x04004629 RID: 17961
			internal double _firstContainerOffsetFromViewport;

			// Token: 0x0400462A RID: 17962
			internal double _expectedDistanceBetweenViewports;

			// Token: 0x0400462B RID: 17963
			internal DependencyObject _bringIntoViewContainer;

			// Token: 0x0400462C RID: 17964
			internal DependencyObject _bringIntoViewLeafContainer;

			// Token: 0x0400462D RID: 17965
			internal List<double> _effectiveOffsets;
		}

		// Token: 0x020009D8 RID: 2520
		private class ChildInfo
		{
			// Token: 0x06008929 RID: 35113 RVA: 0x002544EC File Offset: 0x002526EC
			public override string ToString()
			{
				return string.Format(CultureInfo.InvariantCulture, "{0} ds: {1} ar: {2} in: {3}", new object[]
				{
					this._itemIndex,
					this._desiredSize,
					this._arrangeRect,
					this._inset
				});
			}

			// Token: 0x0400462E RID: 17966
			internal int _itemIndex;

			// Token: 0x0400462F RID: 17967
			internal Size _desiredSize;

			// Token: 0x04004630 RID: 17968
			internal Rect _arrangeRect;

			// Token: 0x04004631 RID: 17969
			internal Thickness _inset;
		}

		// Token: 0x020009D9 RID: 2521
		private class SnapshotData
		{
			// Token: 0x17001F03 RID: 7939
			// (get) Token: 0x0600892B RID: 35115 RVA: 0x00254546 File Offset: 0x00252746
			// (set) Token: 0x0600892C RID: 35116 RVA: 0x0025454E File Offset: 0x0025274E
			internal double UniformOrAverageContainerSize { get; set; }

			// Token: 0x17001F04 RID: 7940
			// (get) Token: 0x0600892D RID: 35117 RVA: 0x00254557 File Offset: 0x00252757
			// (set) Token: 0x0600892E RID: 35118 RVA: 0x0025455F File Offset: 0x0025275F
			internal double UniformOrAverageContainerPixelSize { get; set; }

			// Token: 0x17001F05 RID: 7941
			// (get) Token: 0x0600892F RID: 35119 RVA: 0x00254568 File Offset: 0x00252768
			// (set) Token: 0x06008930 RID: 35120 RVA: 0x00254570 File Offset: 0x00252770
			internal List<double> EffectiveOffsets { get; set; }
		}
	}
}
