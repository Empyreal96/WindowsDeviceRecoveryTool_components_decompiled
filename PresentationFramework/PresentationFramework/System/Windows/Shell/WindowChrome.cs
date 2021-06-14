using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Data;
using Standard;

namespace System.Windows.Shell
{
	/// <summary>Represents an object that describes the customizations to the non-client area of a window.</summary>
	// Token: 0x02000151 RID: 337
	public class WindowChrome : Freezable
	{
		/// <summary>Gets a uniform thickness of -1.</summary>
		/// <returns>A uniform thickness of -1 in all cases.</returns>
		// Token: 0x1700047E RID: 1150
		// (get) Token: 0x06000ECC RID: 3788 RVA: 0x00038AA7 File Offset: 0x00036CA7
		public static Thickness GlassFrameCompleteThickness
		{
			get
			{
				return new Thickness(-1.0);
			}
		}

		// Token: 0x06000ECD RID: 3789 RVA: 0x00038AB8 File Offset: 0x00036CB8
		private static void _OnChromeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			if (DesignerProperties.GetIsInDesignMode(d))
			{
				return;
			}
			Window window = (Window)d;
			WindowChrome windowChrome = (WindowChrome)e.NewValue;
			WindowChromeWorker windowChromeWorker = WindowChromeWorker.GetWindowChromeWorker(window);
			if (windowChromeWorker == null)
			{
				windowChromeWorker = new WindowChromeWorker();
				WindowChromeWorker.SetWindowChromeWorker(window, windowChromeWorker);
			}
			windowChromeWorker.SetWindowChrome(windowChrome);
		}

		/// <summary>Gets the value of the <see cref="P:System.Windows.Shell.WindowChrome.WindowChrome" /> attached property from the specified <see cref="T:System.Windows.Window" />.</summary>
		/// <param name="window">The <see cref="T:System.Windows.Window" /> from which to read the property value.</param>
		/// <returns>The instance of <see cref="T:System.Windows.Shell.WindowChrome" /> that is attached to the specified <see cref="T:System.Windows.Window" />.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="window" /> is <see langword="null" />.</exception>
		// Token: 0x06000ECE RID: 3790 RVA: 0x00038B00 File Offset: 0x00036D00
		public static WindowChrome GetWindowChrome(Window window)
		{
			Verify.IsNotNull<Window>(window, "window");
			return (WindowChrome)window.GetValue(WindowChrome.WindowChromeProperty);
		}

		/// <summary>Sets the value of the <see cref="P:System.Windows.Shell.WindowChrome.WindowChrome" /> attached property on the specified <see cref="T:System.Windows.Window" />.</summary>
		/// <param name="window">The <see cref="T:System.Windows.Window" /> on which to set the <see cref="P:System.Windows.Shell.WindowChrome.WindowChrome" /> attached property.</param>
		/// <param name="chrome">The instance of <see cref="T:System.Windows.Shell.WindowChrome" /> to set.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="window" /> is <see langword="null" />.</exception>
		// Token: 0x06000ECF RID: 3791 RVA: 0x00038B1D File Offset: 0x00036D1D
		public static void SetWindowChrome(Window window, WindowChrome chrome)
		{
			Verify.IsNotNull<Window>(window, "window");
			window.SetValue(WindowChrome.WindowChromeProperty, chrome);
		}

		/// <summary>Gets the value of the <see cref="P:System.Windows.Shell.WindowChrome.IsHitTestVisibleInChrome" /> attached property from the specified input element.</summary>
		/// <param name="inputElement">The input element from which to read the property value.</param>
		/// <returns>The value of the <see cref="P:System.Windows.Shell.WindowChrome.IsHitTestVisibleInChrome" /> attached property.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="inputElement" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentException">
		///         <paramref name="inputElement" /> is not a <see cref="T:System.Windows.DependencyObject" />.</exception>
		// Token: 0x06000ED0 RID: 3792 RVA: 0x00038B38 File Offset: 0x00036D38
		public static bool GetIsHitTestVisibleInChrome(IInputElement inputElement)
		{
			Verify.IsNotNull<IInputElement>(inputElement, "inputElement");
			DependencyObject dependencyObject = inputElement as DependencyObject;
			if (dependencyObject == null)
			{
				throw new ArgumentException("The element must be a DependencyObject", "inputElement");
			}
			return (bool)dependencyObject.GetValue(WindowChrome.IsHitTestVisibleInChromeProperty);
		}

		/// <summary>Sets the value of the <see cref="P:System.Windows.Shell.WindowChrome.IsHitTestVisibleInChrome" /> attached property on the specified input element.</summary>
		/// <param name="inputElement">The element on which to set the <see cref="P:System.Windows.Shell.WindowChrome.IsHitTestVisibleInChrome" /> attached property.</param>
		/// <param name="hitTestVisible">The property value to set.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="inputElement" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentException">
		///         <paramref name="inputElement" /> is not a <see cref="T:System.Windows.DependencyObject" />.</exception>
		// Token: 0x06000ED1 RID: 3793 RVA: 0x00038B7C File Offset: 0x00036D7C
		public static void SetIsHitTestVisibleInChrome(IInputElement inputElement, bool hitTestVisible)
		{
			Verify.IsNotNull<IInputElement>(inputElement, "inputElement");
			DependencyObject dependencyObject = inputElement as DependencyObject;
			if (dependencyObject == null)
			{
				throw new ArgumentException("The element must be a DependencyObject", "inputElement");
			}
			dependencyObject.SetValue(WindowChrome.IsHitTestVisibleInChromeProperty, hitTestVisible);
		}

		/// <summary>Gets the value of the <see cref="P:System.Windows.Shell.WindowChrome.ResizeGripDirection" /> attached property from the specified input element.</summary>
		/// <param name="inputElement">The input element from which to read the property value.</param>
		/// <returns>The value of the <see cref="P:System.Windows.Shell.WindowChrome.ResizeGripDirection" /> attached property.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="inputElement" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentException">
		///         <paramref name="inputElement" /> is not a <see cref="T:System.Windows.DependencyObject" />.</exception>
		// Token: 0x06000ED2 RID: 3794 RVA: 0x00038BBC File Offset: 0x00036DBC
		public static ResizeGripDirection GetResizeGripDirection(IInputElement inputElement)
		{
			Verify.IsNotNull<IInputElement>(inputElement, "inputElement");
			DependencyObject dependencyObject = inputElement as DependencyObject;
			if (dependencyObject == null)
			{
				throw new ArgumentException("The element must be a DependencyObject", "inputElement");
			}
			return (ResizeGripDirection)dependencyObject.GetValue(WindowChrome.ResizeGripDirectionProperty);
		}

		/// <summary>Sets the value of the <see cref="P:System.Windows.Shell.WindowChrome.ResizeGripDirection" /> attached property on the specified input element.</summary>
		/// <param name="inputElement">The element on which to set the <see cref="P:System.Windows.Shell.WindowChrome.ResizeGripDirection" /> attached property.</param>
		/// <param name="direction">The property value to set.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="inputElement" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentException">
		///         <paramref name="inputElement" /> is not a <see cref="T:System.Windows.DependencyObject" />.</exception>
		// Token: 0x06000ED3 RID: 3795 RVA: 0x00038C00 File Offset: 0x00036E00
		public static void SetResizeGripDirection(IInputElement inputElement, ResizeGripDirection direction)
		{
			Verify.IsNotNull<IInputElement>(inputElement, "inputElement");
			DependencyObject dependencyObject = inputElement as DependencyObject;
			if (dependencyObject == null)
			{
				throw new ArgumentException("The element must be a DependencyObject", "inputElement");
			}
			dependencyObject.SetValue(WindowChrome.ResizeGripDirectionProperty, direction);
		}

		/// <summary>Gets or sets the height of the caption area at the top of a window.</summary>
		/// <returns>The height of the caption area.</returns>
		// Token: 0x1700047F RID: 1151
		// (get) Token: 0x06000ED4 RID: 3796 RVA: 0x00038C43 File Offset: 0x00036E43
		// (set) Token: 0x06000ED5 RID: 3797 RVA: 0x00038C55 File Offset: 0x00036E55
		public double CaptionHeight
		{
			get
			{
				return (double)base.GetValue(WindowChrome.CaptionHeightProperty);
			}
			set
			{
				base.SetValue(WindowChrome.CaptionHeightProperty, value);
			}
		}

		/// <summary>Gets or sets a value that indicates the width of the border that is used to resize a window.</summary>
		/// <returns>The width of the border that is used to resize a window.</returns>
		// Token: 0x17000480 RID: 1152
		// (get) Token: 0x06000ED6 RID: 3798 RVA: 0x00038C68 File Offset: 0x00036E68
		// (set) Token: 0x06000ED7 RID: 3799 RVA: 0x00038C7A File Offset: 0x00036E7A
		public Thickness ResizeBorderThickness
		{
			get
			{
				return (Thickness)base.GetValue(WindowChrome.ResizeBorderThicknessProperty);
			}
			set
			{
				base.SetValue(WindowChrome.ResizeBorderThicknessProperty, value);
			}
		}

		// Token: 0x06000ED8 RID: 3800 RVA: 0x00038C8D File Offset: 0x00036E8D
		private static object _CoerceGlassFrameThickness(Thickness thickness)
		{
			if (!Utility.IsThicknessNonNegative(thickness))
			{
				return WindowChrome.GlassFrameCompleteThickness;
			}
			return thickness;
		}

		/// <summary>Gets or sets a value that indicates the width of the glass border around a window.</summary>
		/// <returns>The width of the glass border around a window.</returns>
		// Token: 0x17000481 RID: 1153
		// (get) Token: 0x06000ED9 RID: 3801 RVA: 0x00038CA8 File Offset: 0x00036EA8
		// (set) Token: 0x06000EDA RID: 3802 RVA: 0x00038CBA File Offset: 0x00036EBA
		public Thickness GlassFrameThickness
		{
			get
			{
				return (Thickness)base.GetValue(WindowChrome.GlassFrameThicknessProperty);
			}
			set
			{
				base.SetValue(WindowChrome.GlassFrameThicknessProperty, value);
			}
		}

		/// <summary>Gets or sets a value that indicates whether hit-testing is enabled on the Windows Aero caption buttons.</summary>
		/// <returns>
		///     <see langword="true" /> if hit-testing is enabled on the caption buttons; otherwise, <see langword="false" />. The registered default is <see langword="true" />. For more information about what can influence the value, see Dependency Property Value Precedence.</returns>
		// Token: 0x17000482 RID: 1154
		// (get) Token: 0x06000EDB RID: 3803 RVA: 0x00038CCD File Offset: 0x00036ECD
		// (set) Token: 0x06000EDC RID: 3804 RVA: 0x00038CDF File Offset: 0x00036EDF
		public bool UseAeroCaptionButtons
		{
			get
			{
				return (bool)base.GetValue(WindowChrome.UseAeroCaptionButtonsProperty);
			}
			set
			{
				base.SetValue(WindowChrome.UseAeroCaptionButtonsProperty, value);
			}
		}

		/// <summary>Gets or sets a value that indicates the amount that the corners of a window are rounded.</summary>
		/// <returns>A value that describes the amount that corners are rounded.</returns>
		// Token: 0x17000483 RID: 1155
		// (get) Token: 0x06000EDD RID: 3805 RVA: 0x00038CED File Offset: 0x00036EED
		// (set) Token: 0x06000EDE RID: 3806 RVA: 0x00038CFF File Offset: 0x00036EFF
		public CornerRadius CornerRadius
		{
			get
			{
				return (CornerRadius)base.GetValue(WindowChrome.CornerRadiusProperty);
			}
			set
			{
				base.SetValue(WindowChrome.CornerRadiusProperty, value);
			}
		}

		// Token: 0x06000EDF RID: 3807 RVA: 0x00038D14 File Offset: 0x00036F14
		private static bool _NonClientFrameEdgesAreValid(object value)
		{
			NonClientFrameEdges nonClientFrameEdges = NonClientFrameEdges.None;
			try
			{
				nonClientFrameEdges = (NonClientFrameEdges)value;
			}
			catch (InvalidCastException)
			{
				return false;
			}
			return nonClientFrameEdges == NonClientFrameEdges.None || ((nonClientFrameEdges | WindowChrome.NonClientFrameEdges_All) == WindowChrome.NonClientFrameEdges_All && nonClientFrameEdges != WindowChrome.NonClientFrameEdges_All);
		}

		/// <summary>
		///     Gets or sets a value that indicates which edges of the window frame are not owned by the client.</summary>
		/// <returns>A bitwise combination of the enumeration values that specify which edges of the frame are not owned by the client.The registered default is <see cref="F:System.Windows.Shell.NonClientFrameEdges.None" />. For more information about what can influence the value, see Dependency Property Value Precedence.</returns>
		/// <exception cref="T:System.ArgumentException">‘Left, Right, Top, Bottom’ is not a valid value. At least one edge must belong to the client.</exception>
		// Token: 0x17000484 RID: 1156
		// (get) Token: 0x06000EE0 RID: 3808 RVA: 0x00038D64 File Offset: 0x00036F64
		// (set) Token: 0x06000EE1 RID: 3809 RVA: 0x00038D76 File Offset: 0x00036F76
		public NonClientFrameEdges NonClientFrameEdges
		{
			get
			{
				return (NonClientFrameEdges)base.GetValue(WindowChrome.NonClientFrameEdgesProperty);
			}
			set
			{
				base.SetValue(WindowChrome.NonClientFrameEdgesProperty, value);
			}
		}

		/// <summary>Creates a new instance of the <see cref="T:System.Windows.Shell.WindowChrome" /> class.</summary>
		/// <returns>The new instance of this class.</returns>
		// Token: 0x06000EE2 RID: 3810 RVA: 0x00038D89 File Offset: 0x00036F89
		protected override Freezable CreateInstanceCore()
		{
			return new WindowChrome();
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Shell.WindowChrome" /> class.</summary>
		// Token: 0x06000EE3 RID: 3811 RVA: 0x00038D90 File Offset: 0x00036F90
		public WindowChrome()
		{
			foreach (WindowChrome._SystemParameterBoundProperty systemParameterBoundProperty in WindowChrome._BoundProperties)
			{
				BindingOperations.SetBinding(this, systemParameterBoundProperty.DependencyProperty, new Binding
				{
					Path = new PropertyPath("(SystemParameters." + systemParameterBoundProperty.SystemParameterPropertyName + ")", new object[0]),
					Mode = BindingMode.OneWay,
					UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
				});
			}
		}

		// Token: 0x06000EE4 RID: 3812 RVA: 0x00038E2C File Offset: 0x0003702C
		private void _OnPropertyChangedThatRequiresRepaint()
		{
			EventHandler propertyChangedThatRequiresRepaint = this.PropertyChangedThatRequiresRepaint;
			if (propertyChangedThatRequiresRepaint != null)
			{
				propertyChangedThatRequiresRepaint(this, EventArgs.Empty);
			}
		}

		// Token: 0x1400003D RID: 61
		// (add) Token: 0x06000EE5 RID: 3813 RVA: 0x00038E50 File Offset: 0x00037050
		// (remove) Token: 0x06000EE6 RID: 3814 RVA: 0x00038E88 File Offset: 0x00037088
		internal event EventHandler PropertyChangedThatRequiresRepaint;

		/// <summary>Identifies the <see cref="P:System.Windows.Shell.WindowChrome.WindowChrome" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Shell.WindowChrome.WindowChrome" /> dependency property.</returns>
		// Token: 0x04001161 RID: 4449
		public static readonly DependencyProperty WindowChromeProperty = DependencyProperty.RegisterAttached("WindowChrome", typeof(WindowChrome), typeof(WindowChrome), new PropertyMetadata(null, new PropertyChangedCallback(WindowChrome._OnChromeChanged)));

		/// <summary>Identifies the <see cref="P:System.Windows.Shell.WindowChrome.IsHitTestVisibleInChrome" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Shell.WindowChrome.IsHitTestVisibleInChrome" /> dependency property.</returns>
		// Token: 0x04001162 RID: 4450
		public static readonly DependencyProperty IsHitTestVisibleInChromeProperty = DependencyProperty.RegisterAttached("IsHitTestVisibleInChrome", typeof(bool), typeof(WindowChrome), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.Inherits));

		/// <summary>Identifies the <see cref="P:System.Windows.Shell.WindowChrome.ResizeGripDirection" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Shell.WindowChrome.ResizeGripDirection" /> dependency property.</returns>
		// Token: 0x04001163 RID: 4451
		public static readonly DependencyProperty ResizeGripDirectionProperty = DependencyProperty.RegisterAttached("ResizeGripDirection", typeof(ResizeGripDirection), typeof(WindowChrome), new FrameworkPropertyMetadata(ResizeGripDirection.None, FrameworkPropertyMetadataOptions.Inherits));

		/// <summary>Identifies the <see cref="P:System.Windows.Shell.WindowChrome.CaptionHeight" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Shell.WindowChrome.CaptionHeight" /> dependency property.</returns>
		// Token: 0x04001164 RID: 4452
		public static readonly DependencyProperty CaptionHeightProperty = DependencyProperty.Register("CaptionHeight", typeof(double), typeof(WindowChrome), new PropertyMetadata(0.0, delegate(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((WindowChrome)d)._OnPropertyChangedThatRequiresRepaint();
		}), (object value) => (double)value >= 0.0);

		/// <summary>Identifies the <see cref="P:System.Windows.Shell.WindowChrome.ResizeBorderThickness" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Shell.WindowChrome.ResizeBorderThickness" /> dependency property.</returns>
		// Token: 0x04001165 RID: 4453
		public static readonly DependencyProperty ResizeBorderThicknessProperty = DependencyProperty.Register("ResizeBorderThickness", typeof(Thickness), typeof(WindowChrome), new PropertyMetadata(default(Thickness)), (object value) => Utility.IsThicknessNonNegative((Thickness)value));

		/// <summary>Identifies the <see cref="P:System.Windows.Shell.WindowChrome.GlassFrameThickness" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Shell.WindowChrome.GlassFrameThickness" /> dependency property.</returns>
		// Token: 0x04001166 RID: 4454
		public static readonly DependencyProperty GlassFrameThicknessProperty = DependencyProperty.Register("GlassFrameThickness", typeof(Thickness), typeof(WindowChrome), new PropertyMetadata(default(Thickness), delegate(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((WindowChrome)d)._OnPropertyChangedThatRequiresRepaint();
		}, (DependencyObject d, object o) => WindowChrome._CoerceGlassFrameThickness((Thickness)o)));

		/// <summary>Identifies the <see cref="P:System.Windows.Shell.WindowChrome.UseAeroCaptionButtons" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Shell.WindowChrome.UseAeroCaptionButtons" /> dependency property.</returns>
		// Token: 0x04001167 RID: 4455
		public static readonly DependencyProperty UseAeroCaptionButtonsProperty = DependencyProperty.Register("UseAeroCaptionButtons", typeof(bool), typeof(WindowChrome), new FrameworkPropertyMetadata(true));

		/// <summary>Identifies the <see cref="P:System.Windows.Shell.WindowChrome.CornerRadius" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Shell.WindowChrome.CornerRadius" /> dependency property.</returns>
		// Token: 0x04001168 RID: 4456
		public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(WindowChrome), new PropertyMetadata(default(CornerRadius), delegate(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((WindowChrome)d)._OnPropertyChangedThatRequiresRepaint();
		}), (object value) => Utility.IsCornerRadiusValid((CornerRadius)value));

		/// <summary>Identifies the <see cref="P:System.Windows.Shell.WindowChrome.NonClientFrameEdges" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Shell.WindowChrome.NonClientFrameEdges" /> dependency property.</returns>
		// Token: 0x04001169 RID: 4457
		public static readonly DependencyProperty NonClientFrameEdgesProperty = DependencyProperty.Register("NonClientFrameEdges", typeof(NonClientFrameEdges), typeof(WindowChrome), new PropertyMetadata(NonClientFrameEdges.None, delegate(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((WindowChrome)d)._OnPropertyChangedThatRequiresRepaint();
		}), new ValidateValueCallback(WindowChrome._NonClientFrameEdgesAreValid));

		// Token: 0x0400116A RID: 4458
		private static readonly NonClientFrameEdges NonClientFrameEdges_All = NonClientFrameEdges.Left | NonClientFrameEdges.Top | NonClientFrameEdges.Right | NonClientFrameEdges.Bottom;

		// Token: 0x0400116B RID: 4459
		private static readonly List<WindowChrome._SystemParameterBoundProperty> _BoundProperties = new List<WindowChrome._SystemParameterBoundProperty>
		{
			new WindowChrome._SystemParameterBoundProperty
			{
				DependencyProperty = WindowChrome.CornerRadiusProperty,
				SystemParameterPropertyName = "WindowCornerRadius"
			},
			new WindowChrome._SystemParameterBoundProperty
			{
				DependencyProperty = WindowChrome.CaptionHeightProperty,
				SystemParameterPropertyName = "WindowCaptionHeight"
			},
			new WindowChrome._SystemParameterBoundProperty
			{
				DependencyProperty = WindowChrome.ResizeBorderThicknessProperty,
				SystemParameterPropertyName = "WindowResizeBorderThickness"
			},
			new WindowChrome._SystemParameterBoundProperty
			{
				DependencyProperty = WindowChrome.GlassFrameThicknessProperty,
				SystemParameterPropertyName = "WindowNonClientFrameThickness"
			}
		};

		// Token: 0x02000840 RID: 2112
		private struct _SystemParameterBoundProperty
		{
			// Token: 0x17001D8B RID: 7563
			// (get) Token: 0x06007F04 RID: 32516 RVA: 0x0023744A File Offset: 0x0023564A
			// (set) Token: 0x06007F05 RID: 32517 RVA: 0x00237452 File Offset: 0x00235652
			public string SystemParameterPropertyName { get; set; }

			// Token: 0x17001D8C RID: 7564
			// (get) Token: 0x06007F06 RID: 32518 RVA: 0x0023745B File Offset: 0x0023565B
			// (set) Token: 0x06007F07 RID: 32519 RVA: 0x00237463 File Offset: 0x00235663
			public DependencyProperty DependencyProperty { get; set; }
		}
	}
}
