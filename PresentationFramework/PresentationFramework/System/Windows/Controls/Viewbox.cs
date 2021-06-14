using System;
using System.Collections;
using System.Windows.Media;
using MS.Internal;
using MS.Internal.Controls;
using MS.Internal.Telemetry.PresentationFramework;

namespace System.Windows.Controls
{
	/// <summary>Defines a content decorator that can stretch and scale a single child to fill the available space.</summary>
	// Token: 0x02000559 RID: 1369
	public class Viewbox : Decorator
	{
		// Token: 0x060059B6 RID: 22966 RVA: 0x0018BBA8 File Offset: 0x00189DA8
		static Viewbox()
		{
			ControlsTraceLogger.AddControl(TelemetryControls.ViewBox);
		}

		// Token: 0x060059B8 RID: 22968 RVA: 0x0018BC3C File Offset: 0x00189E3C
		private static bool ValidateStretchValue(object value)
		{
			Stretch stretch = (Stretch)value;
			return stretch == Stretch.Uniform || stretch == Stretch.None || stretch == Stretch.Fill || stretch == Stretch.UniformToFill;
		}

		// Token: 0x060059B9 RID: 22969 RVA: 0x0018BC64 File Offset: 0x00189E64
		private static bool ValidateStretchDirectionValue(object value)
		{
			StretchDirection stretchDirection = (StretchDirection)value;
			return stretchDirection == StretchDirection.Both || stretchDirection == StretchDirection.DownOnly || stretchDirection == StretchDirection.UpOnly;
		}

		// Token: 0x170015D2 RID: 5586
		// (get) Token: 0x060059BA RID: 22970 RVA: 0x0018BC86 File Offset: 0x00189E86
		private ContainerVisual InternalVisual
		{
			get
			{
				if (this._internalVisual == null)
				{
					this._internalVisual = new ContainerVisual();
					base.AddVisualChild(this._internalVisual);
				}
				return this._internalVisual;
			}
		}

		// Token: 0x170015D3 RID: 5587
		// (get) Token: 0x060059BB RID: 22971 RVA: 0x0018BCB0 File Offset: 0x00189EB0
		// (set) Token: 0x060059BC RID: 22972 RVA: 0x0018BCE0 File Offset: 0x00189EE0
		private UIElement InternalChild
		{
			get
			{
				VisualCollection children = this.InternalVisual.Children;
				if (children.Count != 0)
				{
					return children[0] as UIElement;
				}
				return null;
			}
			set
			{
				VisualCollection children = this.InternalVisual.Children;
				if (children.Count != 0)
				{
					children.Clear();
				}
				children.Add(value);
			}
		}

		// Token: 0x170015D4 RID: 5588
		// (get) Token: 0x060059BD RID: 22973 RVA: 0x0018BD0F File Offset: 0x00189F0F
		// (set) Token: 0x060059BE RID: 22974 RVA: 0x0018BD1C File Offset: 0x00189F1C
		private Transform InternalTransform
		{
			get
			{
				return this.InternalVisual.Transform;
			}
			set
			{
				this.InternalVisual.Transform = value;
			}
		}

		/// <summary>Gets or sets the single child of a <see cref="T:System.Windows.Controls.Viewbox" /> element. </summary>
		/// <returns>The single child of a <see cref="T:System.Windows.Controls.Viewbox" /> element. This property has no default value.</returns>
		// Token: 0x170015D5 RID: 5589
		// (get) Token: 0x060059BF RID: 22975 RVA: 0x0018BD2A File Offset: 0x00189F2A
		// (set) Token: 0x060059C0 RID: 22976 RVA: 0x0018BD34 File Offset: 0x00189F34
		public override UIElement Child
		{
			get
			{
				return this.InternalChild;
			}
			set
			{
				UIElement internalChild = this.InternalChild;
				if (internalChild != value)
				{
					base.RemoveLogicalChild(internalChild);
					if (value != null)
					{
						base.AddLogicalChild(value);
					}
					this.InternalChild = value;
					base.InvalidateMeasure();
				}
			}
		}

		/// <summary>Gets the number of child <see cref="T:System.Windows.Media.Visual" /> objects in this instance of <see cref="T:System.Windows.Controls.Viewbox" />.</summary>
		/// <returns>The number of <see cref="T:System.Windows.Media.Visual" /> children.</returns>
		// Token: 0x170015D6 RID: 5590
		// (get) Token: 0x060059C1 RID: 22977 RVA: 0x00016748 File Offset: 0x00014948
		protected override int VisualChildrenCount
		{
			get
			{
				return 1;
			}
		}

		/// <summary>Gets a <see cref="T:System.Windows.Media.Visual" /> child at the specified <paramref name="index" /> position.</summary>
		/// <param name="index">The index position of the wanted <see cref="T:System.Windows.Media.Visual" /> child.</param>
		/// <returns>A <see cref="T:System.Windows.Media.Visual" /> child of the parent <see cref="T:System.Windows.Controls.Viewbox" /> element.</returns>
		// Token: 0x060059C2 RID: 22978 RVA: 0x0018BD6A File Offset: 0x00189F6A
		protected override Visual GetVisualChild(int index)
		{
			if (index != 0)
			{
				throw new ArgumentOutOfRangeException("index", index, SR.Get("Visual_ArgumentOutOfRange"));
			}
			return this.InternalVisual;
		}

		/// <summary>Gets an enumerator that can iterate the logical children of this <see cref="T:System.Windows.Controls.Viewbox" /> element.</summary>
		/// <returns>An <see cref="T:System.Collections.IEnumerator" />. This property has no default value.</returns>
		// Token: 0x170015D7 RID: 5591
		// (get) Token: 0x060059C3 RID: 22979 RVA: 0x0018BD90 File Offset: 0x00189F90
		protected internal override IEnumerator LogicalChildren
		{
			get
			{
				if (this.InternalChild == null)
				{
					return EmptyEnumerator.Instance;
				}
				return new SingleChildEnumerator(this.InternalChild);
			}
		}

		/// <summary>Gets or sets the <see cref="T:System.Windows.Controls.Viewbox" /> <see cref="T:System.Windows.Media.Stretch" /> mode, which determines how content fits into the available space.  </summary>
		/// <returns>A <see cref="T:System.Windows.Media.Stretch" /> that determines how content fits in the available space. The default is <see cref="F:System.Windows.Media.Stretch.Uniform" />.</returns>
		// Token: 0x170015D8 RID: 5592
		// (get) Token: 0x060059C4 RID: 22980 RVA: 0x0018BDAB File Offset: 0x00189FAB
		// (set) Token: 0x060059C5 RID: 22981 RVA: 0x0018BDBD File Offset: 0x00189FBD
		public Stretch Stretch
		{
			get
			{
				return (Stretch)base.GetValue(Viewbox.StretchProperty);
			}
			set
			{
				base.SetValue(Viewbox.StretchProperty, value);
			}
		}

		/// <summary>Gets or sets the <see cref="T:System.Windows.Controls.StretchDirection" />, which determines how scaling is applied to the contents of a <see cref="T:System.Windows.Controls.Viewbox" />.  </summary>
		/// <returns>A <see cref="T:System.Windows.Controls.StretchDirection" /> that determines how scaling is applied to the contents of a <see cref="T:System.Windows.Controls.Viewbox" />. The default is <see cref="F:System.Windows.Controls.StretchDirection.Both" />.</returns>
		// Token: 0x170015D9 RID: 5593
		// (get) Token: 0x060059C6 RID: 22982 RVA: 0x0018BDD0 File Offset: 0x00189FD0
		// (set) Token: 0x060059C7 RID: 22983 RVA: 0x0018BDE2 File Offset: 0x00189FE2
		public StretchDirection StretchDirection
		{
			get
			{
				return (StretchDirection)base.GetValue(Viewbox.StretchDirectionProperty);
			}
			set
			{
				base.SetValue(Viewbox.StretchDirectionProperty, value);
			}
		}

		/// <summary>Measures the child elements of a <see cref="T:System.Windows.Controls.Viewbox" /> prior to arranging them during the <see cref="M:System.Windows.Controls.WrapPanel.ArrangeOverride(System.Windows.Size)" /> pass.</summary>
		/// <param name="constraint">A <see cref="T:System.Windows.Size" /> limit that <see cref="T:System.Windows.Controls.Viewbox" /> cannot exceed.</param>
		/// <returns>The <see cref="T:System.Windows.Size" /> that represents the element size you want.</returns>
		// Token: 0x060059C8 RID: 22984 RVA: 0x0018BDF8 File Offset: 0x00189FF8
		protected override Size MeasureOverride(Size constraint)
		{
			UIElement internalChild = this.InternalChild;
			Size result = default(Size);
			if (internalChild != null)
			{
				Size availableSize = new Size(double.PositiveInfinity, double.PositiveInfinity);
				internalChild.Measure(availableSize);
				Size desiredSize = internalChild.DesiredSize;
				Size size = Viewbox.ComputeScaleFactor(constraint, desiredSize, this.Stretch, this.StretchDirection);
				result.Width = size.Width * desiredSize.Width;
				result.Height = size.Height * desiredSize.Height;
			}
			return result;
		}

		/// <summary>Arranges the content of a <see cref="T:System.Windows.Controls.Viewbox" /> element.</summary>
		/// <param name="arrangeSize">The <see cref="T:System.Windows.Size" /> this element uses to arrange its child elements.</param>
		/// <returns>
		///     <see cref="T:System.Windows.Size" /> that represents the arranged size of this <see cref="T:System.Windows.Controls.Viewbox" /> element and its child elements.</returns>
		// Token: 0x060059C9 RID: 22985 RVA: 0x0018BE80 File Offset: 0x0018A080
		protected override Size ArrangeOverride(Size arrangeSize)
		{
			UIElement internalChild = this.InternalChild;
			if (internalChild != null)
			{
				Size desiredSize = internalChild.DesiredSize;
				Size size = Viewbox.ComputeScaleFactor(arrangeSize, desiredSize, this.Stretch, this.StretchDirection);
				this.InternalTransform = new ScaleTransform(size.Width, size.Height);
				internalChild.Arrange(new Rect(default(Point), internalChild.DesiredSize));
				arrangeSize.Width = size.Width * desiredSize.Width;
				arrangeSize.Height = size.Height * desiredSize.Height;
			}
			return arrangeSize;
		}

		// Token: 0x060059CA RID: 22986 RVA: 0x0018BF14 File Offset: 0x0018A114
		internal static Size ComputeScaleFactor(Size availableSize, Size contentSize, Stretch stretch, StretchDirection stretchDirection)
		{
			double num = 1.0;
			double num2 = 1.0;
			bool flag = !double.IsPositiveInfinity(availableSize.Width);
			bool flag2 = !double.IsPositiveInfinity(availableSize.Height);
			if ((stretch == Stretch.Uniform || stretch == Stretch.UniformToFill || stretch == Stretch.Fill) && (flag || flag2))
			{
				num = (DoubleUtil.IsZero(contentSize.Width) ? 0.0 : (availableSize.Width / contentSize.Width));
				num2 = (DoubleUtil.IsZero(contentSize.Height) ? 0.0 : (availableSize.Height / contentSize.Height));
				if (!flag)
				{
					num = num2;
				}
				else if (!flag2)
				{
					num2 = num;
				}
				else
				{
					switch (stretch)
					{
					case Stretch.Uniform:
					{
						double num3 = (num < num2) ? num : num2;
						num2 = (num = num3);
						break;
					}
					case Stretch.UniformToFill:
					{
						double num4 = (num > num2) ? num : num2;
						num2 = (num = num4);
						break;
					}
					}
				}
				switch (stretchDirection)
				{
				case StretchDirection.UpOnly:
					if (num < 1.0)
					{
						num = 1.0;
					}
					if (num2 < 1.0)
					{
						num2 = 1.0;
					}
					break;
				case StretchDirection.DownOnly:
					if (num > 1.0)
					{
						num = 1.0;
					}
					if (num2 > 1.0)
					{
						num2 = 1.0;
					}
					break;
				}
			}
			return new Size(num, num2);
		}

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.Viewbox.Stretch" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.Viewbox.Stretch" /> dependency property.</returns>
		// Token: 0x04002F23 RID: 12067
		public static readonly DependencyProperty StretchProperty = DependencyProperty.Register("Stretch", typeof(Stretch), typeof(Viewbox), new FrameworkPropertyMetadata(Stretch.Uniform, FrameworkPropertyMetadataOptions.AffectsMeasure), new ValidateValueCallback(Viewbox.ValidateStretchValue));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.Viewbox.StretchDirection" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.Viewbox.StretchDirection" /> dependency property.</returns>
		// Token: 0x04002F24 RID: 12068
		public static readonly DependencyProperty StretchDirectionProperty = DependencyProperty.Register("StretchDirection", typeof(StretchDirection), typeof(Viewbox), new FrameworkPropertyMetadata(StretchDirection.Both, FrameworkPropertyMetadataOptions.AffectsMeasure), new ValidateValueCallback(Viewbox.ValidateStretchDirectionValue));

		// Token: 0x04002F25 RID: 12069
		private ContainerVisual _internalVisual;
	}
}
