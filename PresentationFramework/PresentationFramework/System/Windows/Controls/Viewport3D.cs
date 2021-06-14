using System;
using System.ComponentModel;
using System.Windows.Automation.Peers;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using MS.Internal;
using MS.Internal.KnownBoxes;

namespace System.Windows.Controls
{
	/// <summary>Renders the contained 3-D content within the 2-D layout bounds of the <see cref="T:System.Windows.Controls.Viewport3D" /> element.  </summary>
	// Token: 0x0200055A RID: 1370
	[ContentProperty("Children")]
	[Localizability(LocalizationCategory.NeverLocalize)]
	public class Viewport3D : FrameworkElement, IAddChild
	{
		// Token: 0x060059CB RID: 22987 RVA: 0x0018C078 File Offset: 0x0018A278
		static Viewport3D()
		{
			UIElement.ClipToBoundsProperty.OverrideMetadata(typeof(Viewport3D), new PropertyMetadata(BooleanBoxes.TrueBox));
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Controls.Viewport3D" /> class.</summary>
		// Token: 0x060059CC RID: 22988 RVA: 0x0018C110 File Offset: 0x0018A310
		public Viewport3D()
		{
			this._viewport3DVisual = new Viewport3DVisual();
			this._viewport3DVisual.CanBeInheritanceContext = false;
			base.AddVisualChild(this._viewport3DVisual);
			base.SetValue(Viewport3D.ChildrenPropertyKey, this._viewport3DVisual.Children);
			this._viewport3DVisual.SetInheritanceContextForChildren(this);
		}

		// Token: 0x060059CD RID: 22989 RVA: 0x0018C168 File Offset: 0x0018A368
		private static void OnCameraChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			Viewport3D viewport3D = (Viewport3D)d;
			if (!e.IsASubPropertyChange)
			{
				viewport3D._viewport3DVisual.Camera = (Camera)e.NewValue;
			}
		}

		/// <summary>Gets or sets a camera object that projects the 3-D contents of the <see cref="T:System.Windows.Controls.Viewport3D" /> to the 2-D surface of the <see cref="T:System.Windows.Controls.Viewport3D" />.  </summary>
		/// <returns>The camera that projects the 3-D contents to the 2-D surface.</returns>
		// Token: 0x170015DA RID: 5594
		// (get) Token: 0x060059CE RID: 22990 RVA: 0x0018C19C File Offset: 0x0018A39C
		// (set) Token: 0x060059CF RID: 22991 RVA: 0x0018C1AE File Offset: 0x0018A3AE
		public Camera Camera
		{
			get
			{
				return (Camera)base.GetValue(Viewport3D.CameraProperty);
			}
			set
			{
				base.SetValue(Viewport3D.CameraProperty, value);
			}
		}

		/// <summary>Gets a collection of the <see cref="T:System.Windows.Media.Media3D.Visual3D" /> children of the <see cref="T:System.Windows.Controls.Viewport3D" />.  </summary>
		/// <returns>A collection of the <see cref="T:System.Windows.Media.Media3D.Visual3D" /> children of the <see cref="T:System.Windows.Controls.Viewport3D" />.</returns>
		// Token: 0x170015DB RID: 5595
		// (get) Token: 0x060059D0 RID: 22992 RVA: 0x0018C1BC File Offset: 0x0018A3BC
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public Visual3DCollection Children
		{
			get
			{
				return (Visual3DCollection)base.GetValue(Viewport3D.ChildrenProperty);
			}
		}

		/// <summary>Creates and returns a <see cref="T:System.Windows.Automation.Peers.Viewport3DAutomationPeer" /> object for this <see cref="T:System.Windows.Controls.Viewport3D" />.</summary>
		/// <returns>
		///     <see cref="T:System.Windows.Automation.Peers.Viewport3DAutomationPeer" /> object for this <see cref="T:System.Windows.Controls.Viewport3D" />.</returns>
		// Token: 0x060059D1 RID: 22993 RVA: 0x0018C1CE File Offset: 0x0018A3CE
		protected override AutomationPeer OnCreateAutomationPeer()
		{
			return new Viewport3DAutomationPeer(this);
		}

		/// <summary>Causes the <see cref="T:System.Windows.Controls.Viewport3D" /> to arrange its visual content to fit a specified size. </summary>
		/// <param name="finalSize">Size that <see cref="T:System.Windows.Controls.Viewport3D" /> will assume.</param>
		/// <returns>The final size of the arranged <see cref="T:System.Windows.Controls.Viewport3D" />.</returns>
		// Token: 0x060059D2 RID: 22994 RVA: 0x0018C1D8 File Offset: 0x0018A3D8
		protected override Size ArrangeOverride(Size finalSize)
		{
			Rect viewport = new Rect(default(Point), finalSize);
			this._viewport3DVisual.Viewport = viewport;
			return finalSize;
		}

		/// <summary>Gets the <see cref="T:System.Windows.Media.Visual" /> at a specified position in the <see cref="P:System.Windows.Controls.Viewport3D.Children" /> collection of the <see cref="T:System.Windows.Controls.Viewport3D" />.</summary>
		/// <param name="index">Position of the element in the collection.</param>
		/// <returns>Visual at the specified position in the <see cref="P:System.Windows.Controls.Viewport3D.Children" /> collection.</returns>
		// Token: 0x060059D3 RID: 22995 RVA: 0x0018C203 File Offset: 0x0018A403
		protected override Visual GetVisualChild(int index)
		{
			if (index == 0)
			{
				return this._viewport3DVisual;
			}
			throw new ArgumentOutOfRangeException("index", index, SR.Get("Visual_ArgumentOutOfRange"));
		}

		/// <summary>Gets an integer that represents the number of <see cref="T:System.Windows.Media.Visual" /> objects in the <see cref="P:System.Windows.Media.Media3D.ModelVisual3D.Children" /> collection of the <see cref="T:System.Windows.Media.Media3D.Visual3D" />.</summary>
		/// <returns>Integer that represents the number of Visuals in the Children collection of the <see cref="T:System.Windows.Media.Media3D.Visual3D" />.</returns>
		// Token: 0x170015DC RID: 5596
		// (get) Token: 0x060059D4 RID: 22996 RVA: 0x00016748 File Offset: 0x00014948
		protected override int VisualChildrenCount
		{
			get
			{
				return 1;
			}
		}

		/// <summary>This member supports the Windows Presentation Foundation (WPF) infrastructure and is not intended to be used directly from your code.</summary>
		/// <param name="value">   An object to add as a child.</param>
		// Token: 0x060059D5 RID: 22997 RVA: 0x0018C22C File Offset: 0x0018A42C
		void IAddChild.AddChild(object value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			Visual3D visual3D = value as Visual3D;
			if (visual3D == null)
			{
				throw new ArgumentException(SR.Get("UnexpectedParameterType", new object[]
				{
					value.GetType(),
					typeof(Visual3D)
				}), "value");
			}
			this.Children.Add(visual3D);
		}

		/// <summary>This member supports the Windows Presentation Foundation (WPF) infrastructure and is not intended to be used directly from your code.</summary>
		/// <param name="text">   A string to add to the object.</param>
		// Token: 0x060059D6 RID: 22998 RVA: 0x0000B31C File Offset: 0x0000951C
		void IAddChild.AddText(string text)
		{
			XamlSerializerUtil.ThrowIfNonWhiteSpaceInAddText(text, this);
		}

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.Viewport3D.Camera" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.Viewport3D.Camera" /> dependency property.</returns>
		// Token: 0x04002F26 RID: 12070
		public static readonly DependencyProperty CameraProperty = Viewport3DVisual.CameraProperty.AddOwner(typeof(Viewport3D), new FrameworkPropertyMetadata(FreezableOperations.GetAsFrozen(new PerspectiveCamera()), new PropertyChangedCallback(Viewport3D.OnCameraChanged)));

		// Token: 0x04002F27 RID: 12071
		private static readonly DependencyPropertyKey ChildrenPropertyKey = DependencyProperty.RegisterReadOnly("Children", typeof(Visual3DCollection), typeof(Viewport3D), new FrameworkPropertyMetadata(null));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.Viewport3D.Children" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.Viewport3D.Children" /> dependency property.</returns>
		// Token: 0x04002F28 RID: 12072
		public static readonly DependencyProperty ChildrenProperty = Viewport3D.ChildrenPropertyKey.DependencyProperty;

		// Token: 0x04002F29 RID: 12073
		private readonly Viewport3DVisual _viewport3DVisual;
	}
}
