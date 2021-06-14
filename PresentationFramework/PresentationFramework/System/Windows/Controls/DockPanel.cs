using System;
using System.Windows.Media;
using MS.Internal.PresentationFramework;
using MS.Internal.Telemetry.PresentationFramework;

namespace System.Windows.Controls
{
	/// <summary>Defines an area where you can arrange child elements either horizontally or vertically, relative to each other. </summary>
	// Token: 0x020004CB RID: 1227
	public class DockPanel : Panel
	{
		// Token: 0x06004A86 RID: 19078 RVA: 0x0015029C File Offset: 0x0014E49C
		static DockPanel()
		{
			ControlsTraceLogger.AddControl(TelemetryControls.DockPanel);
		}

		/// <summary>Gets the value of the <see cref="P:System.Windows.Controls.DockPanel.Dock" /> attached property for a specified <see cref="T:System.Windows.UIElement" />.</summary>
		/// <param name="element">The element from which the property value is read.</param>
		/// <returns>The <see cref="P:System.Windows.Controls.DockPanel.Dock" /> property value for the element.</returns>
		// Token: 0x06004A88 RID: 19080 RVA: 0x00150329 File Offset: 0x0014E529
		[AttachedPropertyBrowsableForChildren]
		public static Dock GetDock(UIElement element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (Dock)element.GetValue(DockPanel.DockProperty);
		}

		/// <summary>Sets the value of the <see cref="P:System.Windows.Controls.DockPanel.Dock" /> attached property to a specified element. </summary>
		/// <param name="element">The element to which the attached property is written.</param>
		/// <param name="dock">The needed <see cref="T:System.Windows.Controls.Dock" /> value.</param>
		// Token: 0x06004A89 RID: 19081 RVA: 0x00150349 File Offset: 0x0014E549
		public static void SetDock(UIElement element, Dock dock)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(DockPanel.DockProperty, dock);
		}

		// Token: 0x06004A8A RID: 19082 RVA: 0x0015036C File Offset: 0x0014E56C
		private static void OnDockChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			UIElement uielement = d as UIElement;
			if (uielement != null)
			{
				DockPanel dockPanel = VisualTreeHelper.GetParent(uielement) as DockPanel;
				if (dockPanel != null)
				{
					dockPanel.InvalidateMeasure();
				}
			}
		}

		/// <summary>Gets or sets a value that indicates whether the last child element within a <see cref="T:System.Windows.Controls.DockPanel" /> stretches to fill the remaining available space.   </summary>
		/// <returns>
		///     <see langword="true" /> if the last child element stretches to fill the remaining space; otherwise <see langword="false" />. The default value is <see langword="true" />.</returns>
		// Token: 0x17001231 RID: 4657
		// (get) Token: 0x06004A8B RID: 19083 RVA: 0x00150398 File Offset: 0x0014E598
		// (set) Token: 0x06004A8C RID: 19084 RVA: 0x001503AA File Offset: 0x0014E5AA
		public bool LastChildFill
		{
			get
			{
				return (bool)base.GetValue(DockPanel.LastChildFillProperty);
			}
			set
			{
				base.SetValue(DockPanel.LastChildFillProperty, value);
			}
		}

		/// <summary>Measures the child elements of a <see cref="T:System.Windows.Controls.DockPanel" /> prior to arranging them during the <see cref="M:System.Windows.Controls.DockPanel.ArrangeOverride(System.Windows.Size)" /> pass.</summary>
		/// <param name="constraint">A maximum <see cref="T:System.Windows.Size" /> to not exceed.</param>
		/// <returns>A <see cref="T:System.Windows.Size" /> that represents the element size you want.</returns>
		// Token: 0x06004A8D RID: 19085 RVA: 0x001503B8 File Offset: 0x0014E5B8
		protected override Size MeasureOverride(Size constraint)
		{
			UIElementCollection internalChildren = base.InternalChildren;
			double num = 0.0;
			double num2 = 0.0;
			double num3 = 0.0;
			double num4 = 0.0;
			int i = 0;
			int count = internalChildren.Count;
			while (i < count)
			{
				UIElement uielement = internalChildren[i];
				if (uielement != null)
				{
					Size availableSize = new Size(Math.Max(0.0, constraint.Width - num3), Math.Max(0.0, constraint.Height - num4));
					uielement.Measure(availableSize);
					Size desiredSize = uielement.DesiredSize;
					switch (DockPanel.GetDock(uielement))
					{
					case Dock.Left:
					case Dock.Right:
						num2 = Math.Max(num2, num4 + desiredSize.Height);
						num3 += desiredSize.Width;
						break;
					case Dock.Top:
					case Dock.Bottom:
						num = Math.Max(num, num3 + desiredSize.Width);
						num4 += desiredSize.Height;
						break;
					}
				}
				i++;
			}
			num = Math.Max(num, num3);
			num2 = Math.Max(num2, num4);
			return new Size(num, num2);
		}

		/// <summary>Arranges the content (child elements) of a <see cref="T:System.Windows.Controls.DockPanel" /> element.</summary>
		/// <param name="arrangeSize">The <see cref="T:System.Windows.Size" /> this element uses to arrange its child elements.</param>
		/// <returns>The <see cref="T:System.Windows.Size" /> that represents the arranged size of this <see cref="T:System.Windows.Controls.DockPanel" /> element.</returns>
		// Token: 0x06004A8E RID: 19086 RVA: 0x001504E0 File Offset: 0x0014E6E0
		protected override Size ArrangeOverride(Size arrangeSize)
		{
			UIElementCollection internalChildren = base.InternalChildren;
			int count = internalChildren.Count;
			int num = count - (this.LastChildFill ? 1 : 0);
			double num2 = 0.0;
			double num3 = 0.0;
			double num4 = 0.0;
			double num5 = 0.0;
			for (int i = 0; i < count; i++)
			{
				UIElement uielement = internalChildren[i];
				if (uielement != null)
				{
					Size desiredSize = uielement.DesiredSize;
					Rect finalRect = new Rect(num2, num3, Math.Max(0.0, arrangeSize.Width - (num2 + num4)), Math.Max(0.0, arrangeSize.Height - (num3 + num5)));
					if (i < num)
					{
						switch (DockPanel.GetDock(uielement))
						{
						case Dock.Left:
							num2 += desiredSize.Width;
							finalRect.Width = desiredSize.Width;
							break;
						case Dock.Top:
							num3 += desiredSize.Height;
							finalRect.Height = desiredSize.Height;
							break;
						case Dock.Right:
							num4 += desiredSize.Width;
							finalRect.X = Math.Max(0.0, arrangeSize.Width - num4);
							finalRect.Width = desiredSize.Width;
							break;
						case Dock.Bottom:
							num5 += desiredSize.Height;
							finalRect.Y = Math.Max(0.0, arrangeSize.Height - num5);
							finalRect.Height = desiredSize.Height;
							break;
						}
					}
					uielement.Arrange(finalRect);
				}
			}
			return arrangeSize;
		}

		// Token: 0x06004A8F RID: 19087 RVA: 0x00150688 File Offset: 0x0014E888
		internal static bool IsValidDock(object o)
		{
			Dock dock = (Dock)o;
			return dock == Dock.Left || dock == Dock.Top || dock == Dock.Right || dock == Dock.Bottom;
		}

		// Token: 0x17001232 RID: 4658
		// (get) Token: 0x06004A90 RID: 19088 RVA: 0x0009580F File Offset: 0x00093A0F
		internal override int EffectiveValuesInitialSize
		{
			get
			{
				return 9;
			}
		}

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.DockPanel.LastChildFill" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.DockPanel.LastChildFill" /> dependency property.</returns>
		// Token: 0x04002A67 RID: 10855
		[CommonDependencyProperty]
		public static readonly DependencyProperty LastChildFillProperty = DependencyProperty.Register("LastChildFill", typeof(bool), typeof(DockPanel), new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.AffectsArrange));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.DockPanel.Dock" /> attached property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.DockPanel.Dock" /> attached property.</returns>
		// Token: 0x04002A68 RID: 10856
		[CommonDependencyProperty]
		public static readonly DependencyProperty DockProperty = DependencyProperty.RegisterAttached("Dock", typeof(Dock), typeof(DockPanel), new FrameworkPropertyMetadata(Dock.Left, new PropertyChangedCallback(DockPanel.OnDockChanged)), new ValidateValueCallback(DockPanel.IsValidDock));
	}
}
