using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;

namespace System.Windows.Forms.Layout
{
	// Token: 0x020004D9 RID: 1241
	internal class FlowLayout : LayoutEngine
	{
		// Token: 0x0600526F RID: 21103 RVA: 0x0015900C File Offset: 0x0015720C
		internal static FlowLayoutSettings CreateSettings(IArrangedElement owner)
		{
			return new FlowLayoutSettings(owner);
		}

		// Token: 0x06005270 RID: 21104 RVA: 0x00159014 File Offset: 0x00157214
		internal override bool LayoutCore(IArrangedElement container, LayoutEventArgs args)
		{
			CommonProperties.SetLayoutBounds(container, this.xLayout(container, container.DisplayRectangle, false));
			return CommonProperties.GetAutoSize(container);
		}

		// Token: 0x06005271 RID: 21105 RVA: 0x00159030 File Offset: 0x00157230
		internal override Size GetPreferredSize(IArrangedElement container, Size proposedConstraints)
		{
			Rectangle displayRect = new Rectangle(new Point(0, 0), proposedConstraints);
			Size size = this.xLayout(container, displayRect, true);
			if (size.Width > proposedConstraints.Width || size.Height > proposedConstraints.Height)
			{
				displayRect.Size = size;
				size = this.xLayout(container, displayRect, true);
			}
			return size;
		}

		// Token: 0x06005272 RID: 21106 RVA: 0x00159089 File Offset: 0x00157289
		private static FlowLayout.ContainerProxy CreateContainerProxy(IArrangedElement container, FlowDirection flowDirection)
		{
			switch (flowDirection)
			{
			case FlowDirection.TopDown:
				return new FlowLayout.TopDownProxy(container);
			case FlowDirection.RightToLeft:
				return new FlowLayout.RightToLeftProxy(container);
			case FlowDirection.BottomUp:
				return new FlowLayout.BottomUpProxy(container);
			}
			return new FlowLayout.ContainerProxy(container);
		}

		// Token: 0x06005273 RID: 21107 RVA: 0x001590C0 File Offset: 0x001572C0
		private Size xLayout(IArrangedElement container, Rectangle displayRect, bool measureOnly)
		{
			FlowDirection flowDirection = FlowLayout.GetFlowDirection(container);
			bool wrapContents = FlowLayout.GetWrapContents(container);
			FlowLayout.ContainerProxy containerProxy = FlowLayout.CreateContainerProxy(container, flowDirection);
			containerProxy.DisplayRect = displayRect;
			displayRect = containerProxy.DisplayRect;
			FlowLayout.ElementProxy elementProxy = containerProxy.ElementProxy;
			Size empty = Size.Empty;
			if (!wrapContents)
			{
				displayRect.Width = int.MaxValue - displayRect.X;
			}
			int num;
			for (int i = 0; i < container.Children.Count; i = num)
			{
				Size size = Size.Empty;
				Rectangle displayRectangle = new Rectangle(displayRect.X, displayRect.Y, displayRect.Width, displayRect.Height - empty.Height);
				size = this.MeasureRow(containerProxy, elementProxy, i, displayRectangle, out num);
				if (!measureOnly)
				{
					Rectangle rowBounds = new Rectangle(displayRect.X, empty.Height + displayRect.Y, size.Width, size.Height);
					this.LayoutRow(containerProxy, elementProxy, i, num, rowBounds);
				}
				empty.Width = Math.Max(empty.Width, size.Width);
				empty.Height += size.Height;
			}
			if (container.Children.Count != 0)
			{
			}
			return LayoutUtils.FlipSizeIf(flowDirection == FlowDirection.TopDown || FlowLayout.GetFlowDirection(container) == FlowDirection.BottomUp, empty);
		}

		// Token: 0x06005274 RID: 21108 RVA: 0x00159208 File Offset: 0x00157408
		private void LayoutRow(FlowLayout.ContainerProxy containerProxy, FlowLayout.ElementProxy elementProxy, int startIndex, int endIndex, Rectangle rowBounds)
		{
			int num;
			Size size = this.xLayoutRow(containerProxy, elementProxy, startIndex, endIndex, rowBounds, out num, false);
		}

		// Token: 0x06005275 RID: 21109 RVA: 0x00159226 File Offset: 0x00157426
		private Size MeasureRow(FlowLayout.ContainerProxy containerProxy, FlowLayout.ElementProxy elementProxy, int startIndex, Rectangle displayRectangle, out int breakIndex)
		{
			return this.xLayoutRow(containerProxy, elementProxy, startIndex, containerProxy.Container.Children.Count, displayRectangle, out breakIndex, true);
		}

		// Token: 0x06005276 RID: 21110 RVA: 0x00159248 File Offset: 0x00157448
		private Size xLayoutRow(FlowLayout.ContainerProxy containerProxy, FlowLayout.ElementProxy elementProxy, int startIndex, int endIndex, Rectangle rowBounds, out int breakIndex, bool measureOnly)
		{
			Point location = rowBounds.Location;
			Size empty = Size.Empty;
			int num = 0;
			breakIndex = startIndex;
			bool wrapContents = FlowLayout.GetWrapContents(containerProxy.Container);
			bool flag = false;
			ArrangedElementCollection children = containerProxy.Container.Children;
			int i = startIndex;
			while (i < endIndex)
			{
				elementProxy.Element = children[i];
				if (elementProxy.ParticipatesInLayout)
				{
					Size size2;
					if (elementProxy.AutoSize)
					{
						Size size = new Size(int.MaxValue, rowBounds.Height - elementProxy.Margin.Size.Height);
						if (i == startIndex)
						{
							size.Width = rowBounds.Width - empty.Width - elementProxy.Margin.Size.Width;
						}
						size = LayoutUtils.UnionSizes(new Size(1, 1), size);
						size2 = elementProxy.GetPreferredSize(size);
					}
					else
					{
						size2 = elementProxy.SpecifiedSize;
						if (elementProxy.Stretches)
						{
							size2.Height = 0;
						}
						if (size2.Height < elementProxy.MinimumSize.Height)
						{
							size2.Height = elementProxy.MinimumSize.Height;
						}
					}
					Size size3 = size2 + elementProxy.Margin.Size;
					if (!measureOnly)
					{
						Rectangle rectangle = new Rectangle(location, new Size(size3.Width, rowBounds.Height));
						rectangle = LayoutUtils.DeflateRect(rectangle, elementProxy.Margin);
						AnchorStyles anchorStyles = elementProxy.AnchorStyles;
						containerProxy.Bounds = LayoutUtils.AlignAndStretch(size2, rectangle, anchorStyles);
					}
					location.X += size3.Width;
					if (num > 0 && location.X > rowBounds.Right)
					{
						break;
					}
					empty.Width = location.X - rowBounds.X;
					empty.Height = Math.Max(empty.Height, size3.Height);
					if (wrapContents)
					{
						if (flag)
						{
							break;
						}
						if (i + 1 < endIndex && CommonProperties.GetFlowBreak(elementProxy.Element))
						{
							if (num != 0)
							{
								breakIndex++;
								break;
							}
							flag = true;
						}
					}
					num++;
				}
				i++;
				breakIndex++;
			}
			return empty;
		}

		// Token: 0x06005277 RID: 21111 RVA: 0x00159474 File Offset: 0x00157674
		public static bool GetWrapContents(IArrangedElement container)
		{
			int integer = container.Properties.GetInteger(FlowLayout._wrapContentsProperty);
			return integer == 0;
		}

		// Token: 0x06005278 RID: 21112 RVA: 0x00159496 File Offset: 0x00157696
		public static void SetWrapContents(IArrangedElement container, bool value)
		{
			container.Properties.SetInteger(FlowLayout._wrapContentsProperty, value ? 0 : 1);
			LayoutTransaction.DoLayout(container, container, PropertyNames.WrapContents);
		}

		// Token: 0x06005279 RID: 21113 RVA: 0x001594BB File Offset: 0x001576BB
		public static FlowDirection GetFlowDirection(IArrangedElement container)
		{
			return (FlowDirection)container.Properties.GetInteger(FlowLayout._flowDirectionProperty);
		}

		// Token: 0x0600527A RID: 21114 RVA: 0x001594D0 File Offset: 0x001576D0
		public static void SetFlowDirection(IArrangedElement container, FlowDirection value)
		{
			if (!ClientUtils.IsEnumValid(value, (int)value, 0, 3))
			{
				throw new InvalidEnumArgumentException("value", (int)value, typeof(FlowDirection));
			}
			container.Properties.SetInteger(FlowLayout._flowDirectionProperty, (int)value);
			LayoutTransaction.DoLayout(container, container, PropertyNames.FlowDirection);
		}

		// Token: 0x0600527B RID: 21115 RVA: 0x0000701A File Offset: 0x0000521A
		[Conditional("DEBUG_VERIFY_ALIGNMENT")]
		private void Debug_VerifyAlignment(IArrangedElement container, FlowDirection flowDirection)
		{
		}

		// Token: 0x0400349A RID: 13466
		internal static readonly FlowLayout Instance = new FlowLayout();

		// Token: 0x0400349B RID: 13467
		private static readonly int _wrapContentsProperty = PropertyStore.CreateKey();

		// Token: 0x0400349C RID: 13468
		private static readonly int _flowDirectionProperty = PropertyStore.CreateKey();

		// Token: 0x0200084E RID: 2126
		private class ContainerProxy
		{
			// Token: 0x06006FA8 RID: 28584 RVA: 0x0019A046 File Offset: 0x00198246
			public ContainerProxy(IArrangedElement container)
			{
				this._container = container;
				this._isContainerRTL = false;
				if (this._container is Control)
				{
					this._isContainerRTL = (((Control)this._container).RightToLeft == RightToLeft.Yes);
				}
			}

			// Token: 0x17001824 RID: 6180
			// (set) Token: 0x06006FA9 RID: 28585 RVA: 0x0019A084 File Offset: 0x00198284
			public virtual Rectangle Bounds
			{
				set
				{
					if (this.IsContainerRTL)
					{
						if (this.IsVertical)
						{
							value.Y = this.DisplayRect.Bottom - value.Bottom;
						}
						else
						{
							value.X = this.DisplayRect.Right - value.Right;
						}
						FlowLayoutPanel flowLayoutPanel = this.Container as FlowLayoutPanel;
						if (flowLayoutPanel != null)
						{
							Point autoScrollPosition = flowLayoutPanel.AutoScrollPosition;
							if (autoScrollPosition != Point.Empty)
							{
								Point location = new Point(value.X, value.Y);
								if (this.IsVertical)
								{
									location.Offset(0, autoScrollPosition.X);
								}
								else
								{
									location.Offset(autoScrollPosition.X, 0);
								}
								value.Location = location;
							}
						}
					}
					this.ElementProxy.Bounds = value;
				}
			}

			// Token: 0x17001825 RID: 6181
			// (get) Token: 0x06006FAA RID: 28586 RVA: 0x0019A154 File Offset: 0x00198354
			public IArrangedElement Container
			{
				get
				{
					return this._container;
				}
			}

			// Token: 0x17001826 RID: 6182
			// (get) Token: 0x06006FAB RID: 28587 RVA: 0x0000E0A4 File Offset: 0x0000C2A4
			protected virtual bool IsVertical
			{
				get
				{
					return false;
				}
			}

			// Token: 0x17001827 RID: 6183
			// (get) Token: 0x06006FAC RID: 28588 RVA: 0x0019A15C File Offset: 0x0019835C
			protected bool IsContainerRTL
			{
				get
				{
					return this._isContainerRTL;
				}
			}

			// Token: 0x17001828 RID: 6184
			// (get) Token: 0x06006FAD RID: 28589 RVA: 0x0019A164 File Offset: 0x00198364
			// (set) Token: 0x06006FAE RID: 28590 RVA: 0x0019A16C File Offset: 0x0019836C
			public Rectangle DisplayRect
			{
				get
				{
					return this._displayRect;
				}
				set
				{
					if (this._displayRect != value)
					{
						this._displayRect = LayoutUtils.FlipRectangleIf(this.IsVertical, value);
					}
				}
			}

			// Token: 0x17001829 RID: 6185
			// (get) Token: 0x06006FAF RID: 28591 RVA: 0x0019A18E File Offset: 0x0019838E
			public FlowLayout.ElementProxy ElementProxy
			{
				get
				{
					if (this._elementProxy == null)
					{
						this._elementProxy = (this.IsVertical ? new FlowLayout.VerticalElementProxy() : new FlowLayout.ElementProxy());
					}
					return this._elementProxy;
				}
			}

			// Token: 0x06006FB0 RID: 28592 RVA: 0x0019A1B8 File Offset: 0x001983B8
			protected Rectangle RTLTranslateNoMarginSwap(Rectangle bounds)
			{
				Rectangle result = bounds;
				result.X = this.DisplayRect.Right - bounds.X - bounds.Width + this.ElementProxy.Margin.Left - this.ElementProxy.Margin.Right;
				FlowLayoutPanel flowLayoutPanel = this.Container as FlowLayoutPanel;
				if (flowLayoutPanel != null)
				{
					Point autoScrollPosition = flowLayoutPanel.AutoScrollPosition;
					if (autoScrollPosition != Point.Empty)
					{
						Point location = new Point(result.X, result.Y);
						if (this.IsVertical)
						{
							location.Offset(autoScrollPosition.Y, 0);
						}
						else
						{
							location.Offset(autoScrollPosition.X, 0);
						}
						result.Location = location;
					}
				}
				return result;
			}

			// Token: 0x0400432E RID: 17198
			private IArrangedElement _container;

			// Token: 0x0400432F RID: 17199
			private FlowLayout.ElementProxy _elementProxy;

			// Token: 0x04004330 RID: 17200
			private Rectangle _displayRect;

			// Token: 0x04004331 RID: 17201
			private bool _isContainerRTL;
		}

		// Token: 0x0200084F RID: 2127
		private class RightToLeftProxy : FlowLayout.ContainerProxy
		{
			// Token: 0x06006FB1 RID: 28593 RVA: 0x0019A280 File Offset: 0x00198480
			public RightToLeftProxy(IArrangedElement container) : base(container)
			{
			}

			// Token: 0x1700182A RID: 6186
			// (set) Token: 0x06006FB2 RID: 28594 RVA: 0x0019A289 File Offset: 0x00198489
			public override Rectangle Bounds
			{
				set
				{
					base.Bounds = base.RTLTranslateNoMarginSwap(value);
				}
			}
		}

		// Token: 0x02000850 RID: 2128
		private class TopDownProxy : FlowLayout.ContainerProxy
		{
			// Token: 0x06006FB3 RID: 28595 RVA: 0x0019A280 File Offset: 0x00198480
			public TopDownProxy(IArrangedElement container) : base(container)
			{
			}

			// Token: 0x1700182B RID: 6187
			// (get) Token: 0x06006FB4 RID: 28596 RVA: 0x0000E214 File Offset: 0x0000C414
			protected override bool IsVertical
			{
				get
				{
					return true;
				}
			}
		}

		// Token: 0x02000851 RID: 2129
		private class BottomUpProxy : FlowLayout.ContainerProxy
		{
			// Token: 0x06006FB5 RID: 28597 RVA: 0x0019A280 File Offset: 0x00198480
			public BottomUpProxy(IArrangedElement container) : base(container)
			{
			}

			// Token: 0x1700182C RID: 6188
			// (get) Token: 0x06006FB6 RID: 28598 RVA: 0x0000E214 File Offset: 0x0000C414
			protected override bool IsVertical
			{
				get
				{
					return true;
				}
			}

			// Token: 0x1700182D RID: 6189
			// (set) Token: 0x06006FB7 RID: 28599 RVA: 0x0019A289 File Offset: 0x00198489
			public override Rectangle Bounds
			{
				set
				{
					base.Bounds = base.RTLTranslateNoMarginSwap(value);
				}
			}
		}

		// Token: 0x02000852 RID: 2130
		private class ElementProxy
		{
			// Token: 0x1700182E RID: 6190
			// (get) Token: 0x06006FB8 RID: 28600 RVA: 0x0019A298 File Offset: 0x00198498
			public virtual AnchorStyles AnchorStyles
			{
				get
				{
					AnchorStyles unifiedAnchor = LayoutUtils.GetUnifiedAnchor(this.Element);
					bool flag = (unifiedAnchor & (AnchorStyles.Top | AnchorStyles.Bottom)) == (AnchorStyles.Top | AnchorStyles.Bottom);
					bool flag2 = (unifiedAnchor & AnchorStyles.Top) > AnchorStyles.None;
					bool flag3 = (unifiedAnchor & AnchorStyles.Bottom) > AnchorStyles.None;
					if (flag)
					{
						return AnchorStyles.Top | AnchorStyles.Bottom;
					}
					if (flag2)
					{
						return AnchorStyles.Top;
					}
					if (flag3)
					{
						return AnchorStyles.Bottom;
					}
					return AnchorStyles.None;
				}
			}

			// Token: 0x1700182F RID: 6191
			// (get) Token: 0x06006FB9 RID: 28601 RVA: 0x0019A2D6 File Offset: 0x001984D6
			public bool AutoSize
			{
				get
				{
					return CommonProperties.GetAutoSize(this._element);
				}
			}

			// Token: 0x17001830 RID: 6192
			// (set) Token: 0x06006FBA RID: 28602 RVA: 0x0019A2E3 File Offset: 0x001984E3
			public virtual Rectangle Bounds
			{
				set
				{
					this._element.SetBounds(value, BoundsSpecified.None);
				}
			}

			// Token: 0x17001831 RID: 6193
			// (get) Token: 0x06006FBB RID: 28603 RVA: 0x0019A2F2 File Offset: 0x001984F2
			// (set) Token: 0x06006FBC RID: 28604 RVA: 0x0019A2FA File Offset: 0x001984FA
			public IArrangedElement Element
			{
				get
				{
					return this._element;
				}
				set
				{
					this._element = value;
				}
			}

			// Token: 0x17001832 RID: 6194
			// (get) Token: 0x06006FBD RID: 28605 RVA: 0x0019A304 File Offset: 0x00198504
			public bool Stretches
			{
				get
				{
					AnchorStyles anchorStyles = this.AnchorStyles;
					return ((AnchorStyles.Top | AnchorStyles.Bottom) & anchorStyles) == (AnchorStyles.Top | AnchorStyles.Bottom);
				}
			}

			// Token: 0x17001833 RID: 6195
			// (get) Token: 0x06006FBE RID: 28606 RVA: 0x0019A321 File Offset: 0x00198521
			public virtual Padding Margin
			{
				get
				{
					return CommonProperties.GetMargin(this.Element);
				}
			}

			// Token: 0x17001834 RID: 6196
			// (get) Token: 0x06006FBF RID: 28607 RVA: 0x0019A32E File Offset: 0x0019852E
			public virtual Size MinimumSize
			{
				get
				{
					return CommonProperties.GetMinimumSize(this.Element, Size.Empty);
				}
			}

			// Token: 0x17001835 RID: 6197
			// (get) Token: 0x06006FC0 RID: 28608 RVA: 0x0019A340 File Offset: 0x00198540
			public bool ParticipatesInLayout
			{
				get
				{
					return this._element.ParticipatesInLayout;
				}
			}

			// Token: 0x17001836 RID: 6198
			// (get) Token: 0x06006FC1 RID: 28609 RVA: 0x0019A350 File Offset: 0x00198550
			public virtual Size SpecifiedSize
			{
				get
				{
					return CommonProperties.GetSpecifiedBounds(this._element).Size;
				}
			}

			// Token: 0x06006FC2 RID: 28610 RVA: 0x0019A370 File Offset: 0x00198570
			public virtual Size GetPreferredSize(Size proposedSize)
			{
				return this._element.GetPreferredSize(proposedSize);
			}

			// Token: 0x04004332 RID: 17202
			private IArrangedElement _element;
		}

		// Token: 0x02000853 RID: 2131
		private class VerticalElementProxy : FlowLayout.ElementProxy
		{
			// Token: 0x17001837 RID: 6199
			// (get) Token: 0x06006FC4 RID: 28612 RVA: 0x0019A380 File Offset: 0x00198580
			public override AnchorStyles AnchorStyles
			{
				get
				{
					AnchorStyles unifiedAnchor = LayoutUtils.GetUnifiedAnchor(base.Element);
					bool flag = (unifiedAnchor & (AnchorStyles.Left | AnchorStyles.Right)) == (AnchorStyles.Left | AnchorStyles.Right);
					bool flag2 = (unifiedAnchor & AnchorStyles.Left) > AnchorStyles.None;
					bool flag3 = (unifiedAnchor & AnchorStyles.Right) > AnchorStyles.None;
					if (flag)
					{
						return AnchorStyles.Top | AnchorStyles.Bottom;
					}
					if (flag2)
					{
						return AnchorStyles.Top;
					}
					if (flag3)
					{
						return AnchorStyles.Bottom;
					}
					return AnchorStyles.None;
				}
			}

			// Token: 0x17001838 RID: 6200
			// (set) Token: 0x06006FC5 RID: 28613 RVA: 0x0019A3C0 File Offset: 0x001985C0
			public override Rectangle Bounds
			{
				set
				{
					base.Bounds = LayoutUtils.FlipRectangle(value);
				}
			}

			// Token: 0x17001839 RID: 6201
			// (get) Token: 0x06006FC6 RID: 28614 RVA: 0x0019A3CE File Offset: 0x001985CE
			public override Padding Margin
			{
				get
				{
					return LayoutUtils.FlipPadding(base.Margin);
				}
			}

			// Token: 0x1700183A RID: 6202
			// (get) Token: 0x06006FC7 RID: 28615 RVA: 0x0019A3DB File Offset: 0x001985DB
			public override Size MinimumSize
			{
				get
				{
					return LayoutUtils.FlipSize(base.MinimumSize);
				}
			}

			// Token: 0x1700183B RID: 6203
			// (get) Token: 0x06006FC8 RID: 28616 RVA: 0x0019A3E8 File Offset: 0x001985E8
			public override Size SpecifiedSize
			{
				get
				{
					return LayoutUtils.FlipSize(base.SpecifiedSize);
				}
			}

			// Token: 0x06006FC9 RID: 28617 RVA: 0x0019A3F5 File Offset: 0x001985F5
			public override Size GetPreferredSize(Size proposedSize)
			{
				return LayoutUtils.FlipSize(base.GetPreferredSize(LayoutUtils.FlipSize(proposedSize)));
			}
		}
	}
}
