using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using Microsoft.Win32;

namespace System.Windows.Forms.VisualStyles
{
	/// <summary>Provides methods for drawing and getting information about a <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" />. This class cannot be inherited.</summary>
	// Token: 0x02000448 RID: 1096
	public sealed class VisualStyleRenderer
	{
		// Token: 0x06004CC7 RID: 19655 RVA: 0x0013AFA2 File Offset: 0x001391A2
		static VisualStyleRenderer()
		{
			SystemEvents.UserPreferenceChanging += VisualStyleRenderer.OnUserPreferenceChanging;
		}

		// Token: 0x170012EA RID: 4842
		// (get) Token: 0x06004CC8 RID: 19656 RVA: 0x0013AFD3 File Offset: 0x001391D3
		private static bool AreClientAreaVisualStylesSupported
		{
			get
			{
				return VisualStyleInformation.IsEnabledByUser && (Application.VisualStyleState & VisualStyleState.ClientAreaEnabled) == VisualStyleState.ClientAreaEnabled;
			}
		}

		/// <summary>Gets a value specifying whether the operating system is using visual styles to draw controls.</summary>
		/// <returns>
		///     <see langword="true" /> if the operating system supports visual styles, the user has enabled visual styles in the operating system, and visual styles are applied to the client area of application windows; otherwise, <see langword="false" />.</returns>
		// Token: 0x170012EB RID: 4843
		// (get) Token: 0x06004CC9 RID: 19657 RVA: 0x0013AFE8 File Offset: 0x001391E8
		public static bool IsSupported
		{
			get
			{
				bool flag = VisualStyleRenderer.AreClientAreaVisualStylesSupported;
				if (flag)
				{
					IntPtr handle = VisualStyleRenderer.GetHandle("BUTTON", false);
					flag = (handle != IntPtr.Zero);
				}
				return flag;
			}
		}

		/// <summary>Determines whether the specified visual style element is defined by the current visual style.</summary>
		/// <param name="element">A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> whose class and part combination will be verified.</param>
		/// <returns>
		///     <see langword="true" /> if the combination of the <see cref="P:System.Windows.Forms.VisualStyles.VisualStyleElement.ClassName" /> and <see cref="P:System.Windows.Forms.VisualStyles.VisualStyleElement.Part" /> properties of <paramref name="element" /> are defined; otherwise, <see langword="false" />. </returns>
		/// <exception cref="T:System.InvalidOperationException">The operating system does not support visual styles.-or-Visual styles are disabled by the user in the operating system.-or-Visual styles are not applied to the client area of application windows.</exception>
		// Token: 0x06004CCA RID: 19658 RVA: 0x0013B017 File Offset: 0x00139217
		public static bool IsElementDefined(VisualStyleElement element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return VisualStyleRenderer.IsCombinationDefined(element.ClassName, element.Part);
		}

		// Token: 0x06004CCB RID: 19659 RVA: 0x0013B038 File Offset: 0x00139238
		internal static bool IsCombinationDefined(string className, int part)
		{
			bool flag = false;
			if (!VisualStyleRenderer.IsSupported)
			{
				if (!VisualStyleInformation.IsEnabledByUser)
				{
					throw new InvalidOperationException(SR.GetString("VisualStyleNotActive"));
				}
				throw new InvalidOperationException(SR.GetString("VisualStylesDisabledInClientArea"));
			}
			else
			{
				if (className == null)
				{
					throw new ArgumentNullException("className");
				}
				IntPtr handle = VisualStyleRenderer.GetHandle(className, false);
				if (handle != IntPtr.Zero)
				{
					flag = (part == 0 || SafeNativeMethods.IsThemePartDefined(new HandleRef(null, handle), part, 0));
				}
				if (!flag)
				{
					using (VisualStyleRenderer.ThemeHandle themeHandle = VisualStyleRenderer.ThemeHandle.Create(className, false))
					{
						if (themeHandle != null)
						{
							flag = SafeNativeMethods.IsThemePartDefined(new HandleRef(null, themeHandle.NativeHandle), part, 0);
						}
						if (flag)
						{
							VisualStyleRenderer.RefreshCache();
						}
					}
				}
				return flag;
			}
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleRenderer" /> class using the given <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" />.</summary>
		/// <param name="element">A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that this <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleRenderer" /> will represent.</param>
		/// <exception cref="T:System.InvalidOperationException">The operating system does not support visual styles.-or-Visual styles are disabled by the user in the operating system.-or-Visual styles are not applied to the client area of application windows.</exception>
		/// <exception cref="T:System.ArgumentException">
		///         <paramref name="element" /> is not defined by the current visual style.</exception>
		// Token: 0x06004CCC RID: 19660 RVA: 0x0013B0F8 File Offset: 0x001392F8
		public VisualStyleRenderer(VisualStyleElement element) : this(element.ClassName, element.Part, element.State)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleRenderer" /> class using the given class, part, and state values.</summary>
		/// <param name="className">The class name of the element that this <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleRenderer" /> will represent.</param>
		/// <param name="part">The part of the element that this <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleRenderer" /> will represent.</param>
		/// <param name="state">The state of the element that this <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleRenderer" /> will represent.</param>
		/// <exception cref="T:System.InvalidOperationException">The operating system does not support visual styles.-or-Visual styles are disabled by the user in the operating system.-or-Visual styles are not applied to the client area of application windows.</exception>
		/// <exception cref="T:System.ArgumentException">The combination of <paramref name="className" />, <paramref name="part" />, and <paramref name="state" /> is not defined by the current visual style.</exception>
		// Token: 0x06004CCD RID: 19661 RVA: 0x0013B112 File Offset: 0x00139312
		public VisualStyleRenderer(string className, int part, int state)
		{
			if (!VisualStyleRenderer.IsCombinationDefined(className, part))
			{
				throw new ArgumentException(SR.GetString("VisualStylesInvalidCombination"));
			}
			this._class = className;
			this.part = part;
			this.state = state;
		}

		/// <summary>Gets the class name of the current visual style element.</summary>
		/// <returns>A string that identifies the class of the current visual style element.</returns>
		// Token: 0x170012EC RID: 4844
		// (get) Token: 0x06004CCE RID: 19662 RVA: 0x0013B148 File Offset: 0x00139348
		public string Class
		{
			get
			{
				return this._class;
			}
		}

		/// <summary>Gets the part of the current visual style element.</summary>
		/// <returns>A value that specifies the part of the current visual style element.</returns>
		// Token: 0x170012ED RID: 4845
		// (get) Token: 0x06004CCF RID: 19663 RVA: 0x0013B150 File Offset: 0x00139350
		public int Part
		{
			get
			{
				return this.part;
			}
		}

		/// <summary>Gets the state of the current visual style element.</summary>
		/// <returns>A value that identifies the state of the current visual style element.</returns>
		// Token: 0x170012EE RID: 4846
		// (get) Token: 0x06004CD0 RID: 19664 RVA: 0x0013B158 File Offset: 0x00139358
		public int State
		{
			get
			{
				return this.state;
			}
		}

		/// <summary>Gets a unique identifier for the current class of visual style elements.</summary>
		/// <returns>An <see cref="T:System.IntPtr" /> that identifies a set of data that defines the class of elements specified by <see cref="P:System.Windows.Forms.VisualStyles.VisualStyleRenderer.Class" />. </returns>
		/// <exception cref="T:System.InvalidOperationException">The operating system does not support visual styles.-or-Visual styles are disabled by the user in the operating system.-or-Visual styles are not applied to the client area of application windows.</exception>
		// Token: 0x170012EF RID: 4847
		// (get) Token: 0x06004CD1 RID: 19665 RVA: 0x0013B160 File Offset: 0x00139360
		public IntPtr Handle
		{
			get
			{
				if (VisualStyleRenderer.IsSupported)
				{
					return VisualStyleRenderer.GetHandle(this._class);
				}
				if (!VisualStyleInformation.IsEnabledByUser)
				{
					throw new InvalidOperationException(SR.GetString("VisualStyleNotActive"));
				}
				throw new InvalidOperationException(SR.GetString("VisualStylesDisabledInClientArea"));
			}
		}

		/// <summary>Sets this <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleRenderer" /> to the visual style element represented by the specified <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" />.</summary>
		/// <param name="element">A <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleElement" /> that specifies the new values of the <see cref="P:System.Windows.Forms.VisualStyles.VisualStyleRenderer.Class" />, <see cref="P:System.Windows.Forms.VisualStyles.VisualStyleRenderer.Part" />, and <see cref="P:System.Windows.Forms.VisualStyles.VisualStyleRenderer.State" /> properties.</param>
		/// <exception cref="T:System.ArgumentException">
		///         <paramref name="element" /> is not defined by the current visual style.</exception>
		/// <exception cref="T:System.InvalidOperationException">The operating system does not support visual styles.-or-Visual styles are disabled by the user in the operating system.-or-Visual styles are not applied to the client area of application windows.</exception>
		// Token: 0x06004CD2 RID: 19666 RVA: 0x0013B19B File Offset: 0x0013939B
		public void SetParameters(VisualStyleElement element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			this.SetParameters(element.ClassName, element.Part, element.State);
		}

		/// <summary>Sets this <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleRenderer" /> to the visual style element represented by the specified class, part, and state values.</summary>
		/// <param name="className">The new value of the <see cref="P:System.Windows.Forms.VisualStyles.VisualStyleRenderer.Class" /> property.</param>
		/// <param name="part">The new value of the <see cref="P:System.Windows.Forms.VisualStyles.VisualStyleRenderer.Part" /> property.</param>
		/// <param name="state">The new value of the <see cref="P:System.Windows.Forms.VisualStyles.VisualStyleRenderer.State" /> property.</param>
		/// <exception cref="T:System.ArgumentException">The combination of <paramref name="className" />, <paramref name="part" />, and <paramref name="state" /> is not defined by the current visual style.</exception>
		/// <exception cref="T:System.InvalidOperationException">The operating system does not support visual styles.-or-Visual styles are disabled by the user in the operating system.-or-Visual styles are not applied to the client area of application windows.</exception>
		// Token: 0x06004CD3 RID: 19667 RVA: 0x0013B1C3 File Offset: 0x001393C3
		public void SetParameters(string className, int part, int state)
		{
			if (!VisualStyleRenderer.IsCombinationDefined(className, part))
			{
				throw new ArgumentException(SR.GetString("VisualStylesInvalidCombination"));
			}
			this._class = className;
			this.part = part;
			this.state = state;
		}

		/// <summary>Draws the background image of the current visual style element within the specified bounding rectangle.</summary>
		/// <param name="dc">The <see cref="T:System.Drawing.IDeviceContext" /> used to draw the background image.</param>
		/// <param name="bounds">A <see cref="T:System.Drawing.Rectangle" /> in which the background image is drawn.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="dc" /> is <see langword="null" />.</exception>
		// Token: 0x06004CD4 RID: 19668 RVA: 0x0013B1F3 File Offset: 0x001393F3
		public void DrawBackground(IDeviceContext dc, Rectangle bounds)
		{
			this.DrawBackground(dc, bounds, IntPtr.Zero);
		}

		// Token: 0x06004CD5 RID: 19669 RVA: 0x0013B204 File Offset: 0x00139404
		internal void DrawBackground(IDeviceContext dc, Rectangle bounds, IntPtr hWnd)
		{
			if (dc == null)
			{
				throw new ArgumentNullException("dc");
			}
			if (bounds.Width < 0 || bounds.Height < 0)
			{
				return;
			}
			using (WindowsGraphicsWrapper windowsGraphicsWrapper = new WindowsGraphicsWrapper(dc, TextFormatFlags.PreserveGraphicsClipping | TextFormatFlags.PreserveGraphicsTranslateTransform))
			{
				HandleRef hdc = new HandleRef(windowsGraphicsWrapper, windowsGraphicsWrapper.WindowsGraphics.DeviceContext.Hdc);
				if (IntPtr.Zero != hWnd)
				{
					using (VisualStyleRenderer.ThemeHandle themeHandle = VisualStyleRenderer.ThemeHandle.Create(this._class, true, new HandleRef(null, hWnd)))
					{
						this.lastHResult = SafeNativeMethods.DrawThemeBackground(new HandleRef(this, themeHandle.NativeHandle), hdc, this.part, this.state, new NativeMethods.COMRECT(bounds), null);
						return;
					}
				}
				this.lastHResult = SafeNativeMethods.DrawThemeBackground(new HandleRef(this, this.Handle), hdc, this.part, this.state, new NativeMethods.COMRECT(bounds), null);
			}
		}

		/// <summary>Draws the background image of the current visual style element within the specified bounding rectangle and clipped to the specified clipping rectangle.</summary>
		/// <param name="dc">The <see cref="T:System.Drawing.IDeviceContext" /> used to draw the background image.</param>
		/// <param name="bounds">A <see cref="T:System.Drawing.Rectangle" /> in which the background image is drawn.</param>
		/// <param name="clipRectangle">A <see cref="T:System.Drawing.Rectangle" /> that defines a clipping rectangle for the drawing operation. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="dc" /> is <see langword="null" />.</exception>
		// Token: 0x06004CD6 RID: 19670 RVA: 0x0013B304 File Offset: 0x00139504
		public void DrawBackground(IDeviceContext dc, Rectangle bounds, Rectangle clipRectangle)
		{
			this.DrawBackground(dc, bounds, clipRectangle, IntPtr.Zero);
		}

		// Token: 0x06004CD7 RID: 19671 RVA: 0x0013B314 File Offset: 0x00139514
		internal void DrawBackground(IDeviceContext dc, Rectangle bounds, Rectangle clipRectangle, IntPtr hWnd)
		{
			if (dc == null)
			{
				throw new ArgumentNullException("dc");
			}
			if (bounds.Width < 0 || bounds.Height < 0)
			{
				return;
			}
			if (clipRectangle.Width < 0 || clipRectangle.Height < 0)
			{
				return;
			}
			using (WindowsGraphicsWrapper windowsGraphicsWrapper = new WindowsGraphicsWrapper(dc, TextFormatFlags.PreserveGraphicsClipping | TextFormatFlags.PreserveGraphicsTranslateTransform))
			{
				HandleRef hdc = new HandleRef(windowsGraphicsWrapper, windowsGraphicsWrapper.WindowsGraphics.DeviceContext.Hdc);
				if (IntPtr.Zero != hWnd)
				{
					using (VisualStyleRenderer.ThemeHandle themeHandle = VisualStyleRenderer.ThemeHandle.Create(this._class, true, new HandleRef(null, hWnd)))
					{
						this.lastHResult = SafeNativeMethods.DrawThemeBackground(new HandleRef(this, themeHandle.NativeHandle), hdc, this.part, this.state, new NativeMethods.COMRECT(bounds), new NativeMethods.COMRECT(clipRectangle));
						return;
					}
				}
				this.lastHResult = SafeNativeMethods.DrawThemeBackground(new HandleRef(this, this.Handle), hdc, this.part, this.state, new NativeMethods.COMRECT(bounds), new NativeMethods.COMRECT(clipRectangle));
			}
		}

		/// <summary>Draws one or more edges of the specified bounding rectangle.</summary>
		/// <param name="dc">The <see cref="T:System.Drawing.IDeviceContext" /> used to draw the edges.</param>
		/// <param name="bounds">The <see cref="T:System.Drawing.Rectangle" /> whose bounds define the edges to draw.</param>
		/// <param name="edges">A bitwise combination of the <see cref="T:System.Windows.Forms.VisualStyles.Edges" /> values.</param>
		/// <param name="style">A bitwise combination of the <see cref="T:System.Windows.Forms.VisualStyles.EdgeStyle" /> values.</param>
		/// <param name="effects">A bitwise combination of the <see cref="T:System.Windows.Forms.VisualStyles.EdgeEffects" /> values.</param>
		/// <returns>A <see cref="T:System.Drawing.Rectangle" /> that represents the interior of the <paramref name="bounds" /> parameter, minus the edges that were drawn.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="dc" /> is <see langword="null" />.</exception>
		// Token: 0x06004CD8 RID: 19672 RVA: 0x0013B434 File Offset: 0x00139634
		public Rectangle DrawEdge(IDeviceContext dc, Rectangle bounds, Edges edges, EdgeStyle style, EdgeEffects effects)
		{
			if (dc == null)
			{
				throw new ArgumentNullException("dc");
			}
			if (!ClientUtils.IsEnumValid_Masked(edges, (int)edges, 31U))
			{
				throw new InvalidEnumArgumentException("edges", (int)edges, typeof(Edges));
			}
			if (!ClientUtils.IsEnumValid_NotSequential(style, (int)style, new int[]
			{
				5,
				10,
				6,
				9
			}))
			{
				throw new InvalidEnumArgumentException("style", (int)style, typeof(EdgeStyle));
			}
			if (!ClientUtils.IsEnumValid_Masked(effects, (int)effects, 55296U))
			{
				throw new InvalidEnumArgumentException("effects", (int)effects, typeof(EdgeEffects));
			}
			NativeMethods.COMRECT comrect = new NativeMethods.COMRECT();
			using (WindowsGraphicsWrapper windowsGraphicsWrapper = new WindowsGraphicsWrapper(dc, TextFormatFlags.PreserveGraphicsClipping | TextFormatFlags.PreserveGraphicsTranslateTransform))
			{
				HandleRef hdc = new HandleRef(windowsGraphicsWrapper, windowsGraphicsWrapper.WindowsGraphics.DeviceContext.Hdc);
				this.lastHResult = SafeNativeMethods.DrawThemeEdge(new HandleRef(this, this.Handle), hdc, this.part, this.state, new NativeMethods.COMRECT(bounds), (int)style, (int)(edges | (Edges)effects | (Edges)8192), comrect);
			}
			return Rectangle.FromLTRB(comrect.left, comrect.top, comrect.right, comrect.bottom);
		}

		/// <summary>Draws the specified image within the specified bounds.</summary>
		/// <param name="g">The <see cref="T:System.Drawing.Graphics" /> used to draw the image.</param>
		/// <param name="bounds">A <see cref="T:System.Drawing.Rectangle" /> in which the image is drawn.</param>
		/// <param name="image">The <see cref="T:System.Drawing.Image" /> to draw.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="g" /> or <paramref name="image" /> is <see langword="null" />.</exception>
		// Token: 0x06004CD9 RID: 19673 RVA: 0x0013B574 File Offset: 0x00139774
		public void DrawImage(Graphics g, Rectangle bounds, Image image)
		{
			if (g == null)
			{
				throw new ArgumentNullException("g");
			}
			if (image == null)
			{
				throw new ArgumentNullException("image");
			}
			if (bounds.Width < 0 || bounds.Height < 0)
			{
				return;
			}
			g.DrawImage(image, bounds);
		}

		/// <summary>Draws the image from the specified <see cref="T:System.Windows.Forms.ImageList" /> within the specified bounds.</summary>
		/// <param name="g">The <see cref="T:System.Drawing.Graphics" /> used to draw the image.</param>
		/// <param name="bounds">A <see cref="T:System.Drawing.Rectangle" /> in which the image is drawn.</param>
		/// <param name="imageList">An <see cref="T:System.Windows.Forms.ImageList" /> that contains the <see cref="T:System.Drawing.Image" /> to draw.</param>
		/// <param name="imageIndex">The index of the <see cref="T:System.Drawing.Image" /> within <paramref name="imageList" /> to draw.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="g" /> or <paramref name="image" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///         <paramref name="imageIndex" /> is less than 0, or greater than or equal to the number of images in<paramref name=" imageList" />.</exception>
		// Token: 0x06004CDA RID: 19674 RVA: 0x0013B5B0 File Offset: 0x001397B0
		public void DrawImage(Graphics g, Rectangle bounds, ImageList imageList, int imageIndex)
		{
			if (g == null)
			{
				throw new ArgumentNullException("g");
			}
			if (imageList == null)
			{
				throw new ArgumentNullException("imageList");
			}
			if (imageIndex < 0 || imageIndex >= imageList.Images.Count)
			{
				throw new ArgumentOutOfRangeException("imageIndex", SR.GetString("InvalidArgument", new object[]
				{
					"imageIndex",
					imageIndex.ToString(CultureInfo.CurrentCulture)
				}));
			}
			if (bounds.Width < 0 || bounds.Height < 0)
			{
				return;
			}
			g.DrawImage(imageList.Images[imageIndex], bounds);
		}

		/// <summary>Draws the background of a control's parent in the specified area.</summary>
		/// <param name="dc">The <see cref="T:System.Drawing.IDeviceContext" /> used to draw the background of the parent of <paramref name="childControl" />. This object typically belongs to the child control.</param>
		/// <param name="bounds">A <see cref="T:System.Drawing.Rectangle" /> in which to draw the parent control's background. This rectangle should be inside the child control’s bounds.</param>
		/// <param name="childControl">The control whose parent's background will be drawn.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="dc" /> is <see langword="null" />.</exception>
		// Token: 0x06004CDB RID: 19675 RVA: 0x0013B648 File Offset: 0x00139848
		public void DrawParentBackground(IDeviceContext dc, Rectangle bounds, Control childControl)
		{
			if (dc == null)
			{
				throw new ArgumentNullException("dc");
			}
			if (childControl == null)
			{
				throw new ArgumentNullException("childControl");
			}
			if (bounds.Width < 0 || bounds.Height < 0)
			{
				return;
			}
			if (childControl.Handle != IntPtr.Zero)
			{
				using (WindowsGraphicsWrapper windowsGraphicsWrapper = new WindowsGraphicsWrapper(dc, TextFormatFlags.PreserveGraphicsClipping | TextFormatFlags.PreserveGraphicsTranslateTransform))
				{
					HandleRef hdc = new HandleRef(windowsGraphicsWrapper, windowsGraphicsWrapper.WindowsGraphics.DeviceContext.Hdc);
					this.lastHResult = SafeNativeMethods.DrawThemeParentBackground(new HandleRef(this, childControl.Handle), hdc, new NativeMethods.COMRECT(bounds));
				}
			}
		}

		/// <summary>Draws text in the specified bounds using default formatting.</summary>
		/// <param name="dc">The <see cref="T:System.Drawing.IDeviceContext" /> used to draw the text.</param>
		/// <param name="bounds">A <see cref="T:System.Drawing.Rectangle" /> in which to draw the text.</param>
		/// <param name="textToDraw">The text to draw.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="dc" /> is <see langword="null" />.</exception>
		// Token: 0x06004CDC RID: 19676 RVA: 0x0013B6F8 File Offset: 0x001398F8
		public void DrawText(IDeviceContext dc, Rectangle bounds, string textToDraw)
		{
			this.DrawText(dc, bounds, textToDraw, false);
		}

		/// <summary>Draws text in the specified bounds with the option of displaying disabled text.</summary>
		/// <param name="dc">The <see cref="T:System.Drawing.IDeviceContext" /> used to draw the text.</param>
		/// <param name="bounds">A <see cref="T:System.Drawing.Rectangle" /> in which to draw the text.</param>
		/// <param name="textToDraw">The text to draw.</param>
		/// <param name="drawDisabled">
		///       <see langword="true" /> to draw grayed-out text; otherwise, <see langword="false" />.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="dc" /> is <see langword="null" />.</exception>
		// Token: 0x06004CDD RID: 19677 RVA: 0x0013B704 File Offset: 0x00139904
		public void DrawText(IDeviceContext dc, Rectangle bounds, string textToDraw, bool drawDisabled)
		{
			this.DrawText(dc, bounds, textToDraw, drawDisabled, TextFormatFlags.HorizontalCenter);
		}

		/// <summary>Draws text in the specified bounding rectangle with the option of displaying disabled text and applying other text formatting.</summary>
		/// <param name="dc">The <see cref="T:System.Drawing.IDeviceContext" /> used to draw the text.</param>
		/// <param name="bounds">A <see cref="T:System.Drawing.Rectangle" /> in which to draw the text.</param>
		/// <param name="textToDraw">The text to draw.</param>
		/// <param name="drawDisabled">
		///       <see langword="true" /> to draw grayed-out text; otherwise, <see langword="false" />.</param>
		/// <param name="flags">A bitwise combination of the <see cref="T:System.Windows.Forms.TextFormatFlags" /> values.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="dc" /> is <see langword="null" />.</exception>
		// Token: 0x06004CDE RID: 19678 RVA: 0x0013B714 File Offset: 0x00139914
		public void DrawText(IDeviceContext dc, Rectangle bounds, string textToDraw, bool drawDisabled, TextFormatFlags flags)
		{
			if (dc == null)
			{
				throw new ArgumentNullException("dc");
			}
			if (bounds.Width < 0 || bounds.Height < 0)
			{
				return;
			}
			int dwTextFlags = drawDisabled ? 1 : 0;
			if (!string.IsNullOrEmpty(textToDraw))
			{
				using (WindowsGraphicsWrapper windowsGraphicsWrapper = new WindowsGraphicsWrapper(dc, TextFormatFlags.PreserveGraphicsClipping | TextFormatFlags.PreserveGraphicsTranslateTransform))
				{
					HandleRef hdc = new HandleRef(windowsGraphicsWrapper, windowsGraphicsWrapper.WindowsGraphics.DeviceContext.Hdc);
					this.lastHResult = SafeNativeMethods.DrawThemeText(new HandleRef(this, this.Handle), hdc, this.part, this.state, textToDraw, textToDraw.Length, (int)flags, dwTextFlags, new NativeMethods.COMRECT(bounds));
				}
			}
		}

		/// <summary>Returns the content area for the background of the current visual style element.</summary>
		/// <param name="dc">The <see cref="T:System.Drawing.IDeviceContext" /> this operation will use.</param>
		/// <param name="bounds">A <see cref="T:System.Drawing.Rectangle" /> that contains the entire background area of the current visual style element.</param>
		/// <returns>A <see cref="T:System.Drawing.Rectangle" /> that contains the content area for the background of the current visual style element.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="dc" /> is <see langword="null" />.</exception>
		// Token: 0x06004CDF RID: 19679 RVA: 0x0013B7CC File Offset: 0x001399CC
		public Rectangle GetBackgroundContentRectangle(IDeviceContext dc, Rectangle bounds)
		{
			if (dc == null)
			{
				throw new ArgumentNullException("dc");
			}
			if (bounds.Width < 0 || bounds.Height < 0)
			{
				return Rectangle.Empty;
			}
			NativeMethods.COMRECT comrect = new NativeMethods.COMRECT();
			using (WindowsGraphicsWrapper windowsGraphicsWrapper = new WindowsGraphicsWrapper(dc, TextFormatFlags.PreserveGraphicsClipping | TextFormatFlags.PreserveGraphicsTranslateTransform))
			{
				HandleRef hdc = new HandleRef(windowsGraphicsWrapper, windowsGraphicsWrapper.WindowsGraphics.DeviceContext.Hdc);
				this.lastHResult = SafeNativeMethods.GetThemeBackgroundContentRect(new HandleRef(this, this.Handle), hdc, this.part, this.state, new NativeMethods.COMRECT(bounds), comrect);
			}
			return Rectangle.FromLTRB(comrect.left, comrect.top, comrect.right, comrect.bottom);
		}

		/// <summary>Returns the entire background area for the current visual style element.</summary>
		/// <param name="dc">The <see cref="T:System.Drawing.IDeviceContext" /> this operation will use.</param>
		/// <param name="contentBounds">A <see cref="T:System.Drawing.Rectangle" /> that contains the content area of the current visual style element.</param>
		/// <returns>A <see cref="T:System.Drawing.Rectangle" /> that contains the entire background area of the current visual style element.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="dc" /> is <see langword="null" />.</exception>
		// Token: 0x06004CE0 RID: 19680 RVA: 0x0013B890 File Offset: 0x00139A90
		public Rectangle GetBackgroundExtent(IDeviceContext dc, Rectangle contentBounds)
		{
			if (dc == null)
			{
				throw new ArgumentNullException("dc");
			}
			if (contentBounds.Width < 0 || contentBounds.Height < 0)
			{
				return Rectangle.Empty;
			}
			NativeMethods.COMRECT comrect = new NativeMethods.COMRECT();
			using (WindowsGraphicsWrapper windowsGraphicsWrapper = new WindowsGraphicsWrapper(dc, TextFormatFlags.PreserveGraphicsClipping | TextFormatFlags.PreserveGraphicsTranslateTransform))
			{
				HandleRef hdc = new HandleRef(windowsGraphicsWrapper, windowsGraphicsWrapper.WindowsGraphics.DeviceContext.Hdc);
				this.lastHResult = SafeNativeMethods.GetThemeBackgroundExtent(new HandleRef(this, this.Handle), hdc, this.part, this.state, new NativeMethods.COMRECT(contentBounds), comrect);
			}
			return Rectangle.FromLTRB(comrect.left, comrect.top, comrect.right, comrect.bottom);
		}

		/// <summary>Returns the region for the background of the current visual style element.</summary>
		/// <param name="dc">The <see cref="T:System.Drawing.IDeviceContext" /> this operation will use.</param>
		/// <param name="bounds">A <see cref="T:System.Drawing.Rectangle" /> that contains the entire background area of the current visual style element.</param>
		/// <returns>The <see cref="T:System.Drawing.Region" /> that contains the background of the current visual style element.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="dc" /> is <see langword="null" />.</exception>
		// Token: 0x06004CE1 RID: 19681 RVA: 0x0013B954 File Offset: 0x00139B54
		[SuppressUnmanagedCodeSecurity]
		public Region GetBackgroundRegion(IDeviceContext dc, Rectangle bounds)
		{
			if (dc == null)
			{
				throw new ArgumentNullException("dc");
			}
			if (bounds.Width < 0 || bounds.Height < 0)
			{
				return null;
			}
			IntPtr zero = IntPtr.Zero;
			using (WindowsGraphicsWrapper windowsGraphicsWrapper = new WindowsGraphicsWrapper(dc, TextFormatFlags.PreserveGraphicsClipping | TextFormatFlags.PreserveGraphicsTranslateTransform))
			{
				HandleRef hdc = new HandleRef(windowsGraphicsWrapper, windowsGraphicsWrapper.WindowsGraphics.DeviceContext.Hdc);
				this.lastHResult = SafeNativeMethods.GetThemeBackgroundRegion(new HandleRef(this, this.Handle), hdc, this.part, this.state, new NativeMethods.COMRECT(bounds), ref zero);
			}
			if (zero == IntPtr.Zero)
			{
				return null;
			}
			Region result = Region.FromHrgn(zero);
			SafeNativeMethods.ExternalDeleteObject(new HandleRef(null, zero));
			return result;
		}

		/// <summary>Returns the value of the specified Boolean property for the current visual style element.</summary>
		/// <param name="prop">One of the <see cref="T:System.Windows.Forms.VisualStyles.BooleanProperty" /> values that specifies which property value to retrieve for the current visual style element.</param>
		/// <returns>
		///     <see langword="true" /> if the property specified by the <paramref name="prop" /> parameter is <see langword="true" /> for the current visual style element; otherwise, <see langword="false" />.</returns>
		/// <exception cref="T:System.ComponentModel.InvalidEnumArgumentException">
		///         <paramref name="prop" /> is not one of the <see cref="T:System.Windows.Forms.VisualStyles.BooleanProperty" /> values.</exception>
		// Token: 0x06004CE2 RID: 19682 RVA: 0x0013BA1C File Offset: 0x00139C1C
		public bool GetBoolean(BooleanProperty prop)
		{
			if (!ClientUtils.IsEnumValid(prop, (int)prop, 2201, 2213))
			{
				throw new InvalidEnumArgumentException("prop", (int)prop, typeof(BooleanProperty));
			}
			bool result = false;
			this.lastHResult = SafeNativeMethods.GetThemeBool(new HandleRef(this, this.Handle), this.part, this.state, (int)prop, ref result);
			return result;
		}

		/// <summary>Returns the value of the specified color property for the current visual style element.</summary>
		/// <param name="prop">One of the <see cref="T:System.Windows.Forms.VisualStyles.ColorProperty" /> values that specifies which property value to retrieve for the current visual style element.</param>
		/// <returns>A <see cref="T:System.Drawing.Color" /> that contains the value of the property specified by the <paramref name="prop" /> parameter for the current visual style element.</returns>
		/// <exception cref="T:System.ComponentModel.InvalidEnumArgumentException">
		///         <paramref name="prop" /> is not one of the <see cref="T:System.Windows.Forms.VisualStyles.ColorProperty" /> values.</exception>
		// Token: 0x06004CE3 RID: 19683 RVA: 0x0013BA80 File Offset: 0x00139C80
		public Color GetColor(ColorProperty prop)
		{
			if (!ClientUtils.IsEnumValid(prop, (int)prop, 3801, 3823))
			{
				throw new InvalidEnumArgumentException("prop", (int)prop, typeof(ColorProperty));
			}
			int win32Color = 0;
			this.lastHResult = SafeNativeMethods.GetThemeColor(new HandleRef(this, this.Handle), this.part, this.state, (int)prop, ref win32Color);
			return ColorTranslator.FromWin32(win32Color);
		}

		/// <summary>Returns the value of the specified enumerated type property for the current visual style element.</summary>
		/// <param name="prop">One of the <see cref="T:System.Windows.Forms.VisualStyles.EnumProperty" /> values that specifies which property value to retrieve for the current visual style element.</param>
		/// <returns>The integer value of the property specified by the <paramref name="prop" /> parameter for the current visual style element.</returns>
		/// <exception cref="T:System.ComponentModel.InvalidEnumArgumentException">
		///         <paramref name="prop" /> is not one of the <see cref="T:System.Windows.Forms.VisualStyles.EnumProperty" /> values.</exception>
		// Token: 0x06004CE4 RID: 19684 RVA: 0x0013BAEC File Offset: 0x00139CEC
		public int GetEnumValue(EnumProperty prop)
		{
			if (!ClientUtils.IsEnumValid(prop, (int)prop, 4001, 4015))
			{
				throw new InvalidEnumArgumentException("prop", (int)prop, typeof(EnumProperty));
			}
			int result = 0;
			this.lastHResult = SafeNativeMethods.GetThemeEnumValue(new HandleRef(this, this.Handle), this.part, this.state, (int)prop, ref result);
			return result;
		}

		/// <summary>Returns the value of the specified file name property for the current visual style element.</summary>
		/// <param name="prop">One of the <see cref="T:System.Windows.Forms.VisualStyles.FilenameProperty" /> values that specifies which property value to retrieve for the current visual style element.</param>
		/// <returns>A <see cref="T:System.String" /> that contains the value of the property specified by the <paramref name="prop" /> parameter for the current visual style element.</returns>
		/// <exception cref="T:System.ComponentModel.InvalidEnumArgumentException">
		///         <paramref name="prop" /> is not one of the <see cref="T:System.Windows.Forms.VisualStyles.FilenameProperty" /> values.</exception>
		// Token: 0x06004CE5 RID: 19685 RVA: 0x0013BB50 File Offset: 0x00139D50
		public string GetFilename(FilenameProperty prop)
		{
			if (!ClientUtils.IsEnumValid(prop, (int)prop, 3001, 3008))
			{
				throw new InvalidEnumArgumentException("prop", (int)prop, typeof(FilenameProperty));
			}
			StringBuilder stringBuilder = new StringBuilder(512);
			this.lastHResult = SafeNativeMethods.GetThemeFilename(new HandleRef(this, this.Handle), this.part, this.state, (int)prop, stringBuilder, stringBuilder.Capacity);
			return stringBuilder.ToString();
		}

		/// <summary>Returns the value of the specified font property for the current visual style element.</summary>
		/// <param name="dc">The <see cref="T:System.Drawing.IDeviceContext" /> this operation will use.</param>
		/// <param name="prop">One of the <see cref="T:System.Windows.Forms.VisualStyles.FontProperty" /> values that specifies which property value to retrieve for the current visual style element.</param>
		/// <returns>A <see cref="T:System.Drawing.Font" /> that contains the value of the property specified by the <paramref name="prop" /> parameter for the current visual style element.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="dc" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ComponentModel.InvalidEnumArgumentException">
		///         <paramref name="prop" /> is not one of the <see cref="T:System.Windows.Forms.VisualStyles.FontProperty" /> values.</exception>
		// Token: 0x06004CE6 RID: 19686 RVA: 0x0013BBC8 File Offset: 0x00139DC8
		public Font GetFont(IDeviceContext dc, FontProperty prop)
		{
			if (dc == null)
			{
				throw new ArgumentNullException("dc");
			}
			if (!ClientUtils.IsEnumValid(prop, (int)prop, 2601, 2601))
			{
				throw new InvalidEnumArgumentException("prop", (int)prop, typeof(FontProperty));
			}
			NativeMethods.LOGFONT logfont = new NativeMethods.LOGFONT();
			using (WindowsGraphicsWrapper windowsGraphicsWrapper = new WindowsGraphicsWrapper(dc, TextFormatFlags.PreserveGraphicsClipping | TextFormatFlags.PreserveGraphicsTranslateTransform))
			{
				HandleRef hdc = new HandleRef(windowsGraphicsWrapper, windowsGraphicsWrapper.WindowsGraphics.DeviceContext.Hdc);
				this.lastHResult = SafeNativeMethods.GetThemeFont(new HandleRef(this, this.Handle), hdc, this.part, this.state, (int)prop, logfont);
			}
			Font result = null;
			if (NativeMethods.Succeeded(this.lastHResult))
			{
				IntSecurity.ObjectFromWin32Handle.Assert();
				try
				{
					result = Font.FromLogFont(logfont);
				}
				catch (Exception ex)
				{
					if (ClientUtils.IsSecurityOrCriticalException(ex))
					{
						throw;
					}
					result = null;
				}
			}
			return result;
		}

		/// <summary>Returns the value of the specified integer property for the current visual style element.</summary>
		/// <param name="prop">One of the <see cref="T:System.Windows.Forms.VisualStyles.IntegerProperty" /> values that specifies which property value to retrieve for the current visual style element.</param>
		/// <returns>The integer value of the property specified by the <paramref name="prop" /> parameter for the current visual style element.</returns>
		/// <exception cref="T:System.ComponentModel.InvalidEnumArgumentException">
		///         <paramref name="prop" /> is not one of the <see cref="T:System.Windows.Forms.VisualStyles.IntegerProperty" /> values.</exception>
		// Token: 0x06004CE7 RID: 19687 RVA: 0x0013BCC0 File Offset: 0x00139EC0
		public int GetInteger(IntegerProperty prop)
		{
			if (!ClientUtils.IsEnumValid(prop, (int)prop, 2401, 2424))
			{
				throw new InvalidEnumArgumentException("prop", (int)prop, typeof(IntegerProperty));
			}
			int result = 0;
			this.lastHResult = SafeNativeMethods.GetThemeInt(new HandleRef(this, this.Handle), this.part, this.state, (int)prop, ref result);
			return result;
		}

		/// <summary>Returns the value of the specified size property of the current visual style part.</summary>
		/// <param name="dc">The <see cref="T:System.Drawing.IDeviceContext" /> this operation will use.</param>
		/// <param name="type">One of the <see cref="T:System.Windows.Forms.VisualStyles.ThemeSizeType" /> values that specifies which size value to retrieve for the part.</param>
		/// <returns>A <see cref="T:System.Drawing.Size" /> that contains the size specified by the <paramref name="type" /> parameter for the current visual style part. </returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="dc" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ComponentModel.InvalidEnumArgumentException">
		///         <paramref name="prop" /> is not one of the <see cref="T:System.Windows.Forms.VisualStyles.ThemeSizeType" /> values.</exception>
		// Token: 0x06004CE8 RID: 19688 RVA: 0x0013BD24 File Offset: 0x00139F24
		public Size GetPartSize(IDeviceContext dc, ThemeSizeType type)
		{
			return this.GetPartSize(dc, type, IntPtr.Zero);
		}

		// Token: 0x06004CE9 RID: 19689 RVA: 0x0013BD34 File Offset: 0x00139F34
		internal Size GetPartSize(IDeviceContext dc, ThemeSizeType type, IntPtr hWnd)
		{
			if (dc == null)
			{
				throw new ArgumentNullException("dc");
			}
			if (!ClientUtils.IsEnumValid(type, (int)type, 0, 2))
			{
				throw new InvalidEnumArgumentException("type", (int)type, typeof(ThemeSizeType));
			}
			NativeMethods.SIZE size = new NativeMethods.SIZE();
			using (WindowsGraphicsWrapper windowsGraphicsWrapper = new WindowsGraphicsWrapper(dc, TextFormatFlags.PreserveGraphicsClipping | TextFormatFlags.PreserveGraphicsTranslateTransform))
			{
				HandleRef hdc = new HandleRef(windowsGraphicsWrapper, windowsGraphicsWrapper.WindowsGraphics.DeviceContext.Hdc);
				if (DpiHelper.EnableDpiChangedMessageHandling && IntPtr.Zero != hWnd)
				{
					using (VisualStyleRenderer.ThemeHandle themeHandle = VisualStyleRenderer.ThemeHandle.Create(this._class, true, new HandleRef(null, hWnd)))
					{
						this.lastHResult = SafeNativeMethods.GetThemePartSize(new HandleRef(this, themeHandle.NativeHandle), hdc, this.part, this.state, null, type, size);
						goto IL_EC;
					}
				}
				this.lastHResult = SafeNativeMethods.GetThemePartSize(new HandleRef(this, this.Handle), hdc, this.part, this.state, null, type, size);
			}
			IL_EC:
			return new Size(size.cx, size.cy);
		}

		/// <summary>Returns the value of the specified size property of the current visual style part using the specified drawing bounds.</summary>
		/// <param name="dc">The <see cref="T:System.Drawing.IDeviceContext" /> this operation will use.</param>
		/// <param name="bounds">A <see cref="T:System.Drawing.Rectangle" /> that contains the area in which the part will be drawn.</param>
		/// <param name="type">One of the <see cref="T:System.Windows.Forms.VisualStyles.ThemeSizeType" /> values that specifies which size value to retrieve for the part.</param>
		/// <returns>A <see cref="T:System.Drawing.Size" /> that contains the size specified by the <paramref name="type" /> parameter for the current visual style part.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="dc" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ComponentModel.InvalidEnumArgumentException">
		///         <paramref name="prop" /> is not one of the <see cref="T:System.Windows.Forms.VisualStyles.ThemeSizeType" /> values.</exception>
		// Token: 0x06004CEA RID: 19690 RVA: 0x0013BE5C File Offset: 0x0013A05C
		public Size GetPartSize(IDeviceContext dc, Rectangle bounds, ThemeSizeType type)
		{
			if (dc == null)
			{
				throw new ArgumentNullException("dc");
			}
			if (!ClientUtils.IsEnumValid(type, (int)type, 0, 2))
			{
				throw new InvalidEnumArgumentException("type", (int)type, typeof(ThemeSizeType));
			}
			NativeMethods.SIZE size = new NativeMethods.SIZE();
			using (WindowsGraphicsWrapper windowsGraphicsWrapper = new WindowsGraphicsWrapper(dc, TextFormatFlags.PreserveGraphicsClipping | TextFormatFlags.PreserveGraphicsTranslateTransform))
			{
				HandleRef hdc = new HandleRef(windowsGraphicsWrapper, windowsGraphicsWrapper.WindowsGraphics.DeviceContext.Hdc);
				this.lastHResult = SafeNativeMethods.GetThemePartSize(new HandleRef(this, this.Handle), hdc, this.part, this.state, new NativeMethods.COMRECT(bounds), type, size);
			}
			return new Size(size.cx, size.cy);
		}

		/// <summary>Returns the value of the specified point property for the current visual style element.</summary>
		/// <param name="prop">One of the <see cref="T:System.Windows.Forms.VisualStyles.PointProperty" /> values that specifies which property value to retrieve for the current visual style element.</param>
		/// <returns>A <see cref="T:System.Drawing.Point" /> that contains the value of the property specified by the <paramref name="prop" /> parameter for the current visual style element.</returns>
		/// <exception cref="T:System.ComponentModel.InvalidEnumArgumentException">
		///         <paramref name="prop" /> is not one of the <see cref="T:System.Windows.Forms.VisualStyles.PointProperty" /> values.</exception>
		// Token: 0x06004CEB RID: 19691 RVA: 0x0013BF20 File Offset: 0x0013A120
		public Point GetPoint(PointProperty prop)
		{
			if (!ClientUtils.IsEnumValid(prop, (int)prop, 3401, 3408))
			{
				throw new InvalidEnumArgumentException("prop", (int)prop, typeof(PointProperty));
			}
			NativeMethods.POINT point = new NativeMethods.POINT();
			this.lastHResult = SafeNativeMethods.GetThemePosition(new HandleRef(this, this.Handle), this.part, this.state, (int)prop, point);
			return new Point(point.x, point.y);
		}

		/// <summary>Returns the value of the specified margins property for the current visual style element.</summary>
		/// <param name="dc">The <see cref="T:System.Drawing.IDeviceContext" /> this operation will use.</param>
		/// <param name="prop">One of the <see cref="T:System.Windows.Forms.VisualStyles.MarginProperty" /> values that specifies which property value to retrieve for the current visual style element.</param>
		/// <returns>A <see cref="T:System.Windows.Forms.Padding" /> that contains the value of the property specified by the <paramref name="prop" /> parameter for the current visual style element.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="dc" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ComponentModel.InvalidEnumArgumentException">
		///         <paramref name="prop" /> is not one of the <see cref="T:System.Windows.Forms.VisualStyles.MarginProperty" /> values.</exception>
		// Token: 0x06004CEC RID: 19692 RVA: 0x0013BF98 File Offset: 0x0013A198
		public Padding GetMargins(IDeviceContext dc, MarginProperty prop)
		{
			if (dc == null)
			{
				throw new ArgumentNullException("dc");
			}
			if (!ClientUtils.IsEnumValid(prop, (int)prop, 3601, 3603))
			{
				throw new InvalidEnumArgumentException("prop", (int)prop, typeof(MarginProperty));
			}
			NativeMethods.MARGINS margins = default(NativeMethods.MARGINS);
			using (WindowsGraphicsWrapper windowsGraphicsWrapper = new WindowsGraphicsWrapper(dc, TextFormatFlags.PreserveGraphicsClipping | TextFormatFlags.PreserveGraphicsTranslateTransform))
			{
				HandleRef hDC = new HandleRef(windowsGraphicsWrapper, windowsGraphicsWrapper.WindowsGraphics.DeviceContext.Hdc);
				this.lastHResult = SafeNativeMethods.GetThemeMargins(new HandleRef(this, this.Handle), hDC, this.part, this.state, (int)prop, ref margins);
			}
			return new Padding(margins.cxLeftWidth, margins.cyTopHeight, margins.cxRightWidth, margins.cyBottomHeight);
		}

		/// <summary>Returns the value of the specified string property for the current visual style element.</summary>
		/// <param name="prop">One of the <see cref="T:System.Windows.Forms.VisualStyles.StringProperty" /> values that specifies which property value to retrieve for the current visual style element.</param>
		/// <returns>A <see cref="T:System.String" /> that contains the value of the property specified by the <paramref name="prop" /> parameter for the current visual style element.</returns>
		/// <exception cref="T:System.ComponentModel.InvalidEnumArgumentException">
		///         <paramref name="prop" /> is not one of the <see cref="T:System.Windows.Forms.VisualStyles.StringProperty" /> values.</exception>
		// Token: 0x06004CED RID: 19693 RVA: 0x0013C070 File Offset: 0x0013A270
		public string GetString(StringProperty prop)
		{
			if (!ClientUtils.IsEnumValid(prop, (int)prop, 3201, 3201))
			{
				throw new InvalidEnumArgumentException("prop", (int)prop, typeof(StringProperty));
			}
			StringBuilder stringBuilder = new StringBuilder(512);
			this.lastHResult = SafeNativeMethods.GetThemeString(new HandleRef(this, this.Handle), this.part, this.state, (int)prop, stringBuilder, stringBuilder.Capacity);
			return stringBuilder.ToString();
		}

		/// <summary>Returns the size and location of the specified string when drawn with the font of the current visual style element.</summary>
		/// <param name="dc">The <see cref="T:System.Drawing.IDeviceContext" /> this operation will use.</param>
		/// <param name="textToDraw">The string to measure.</param>
		/// <param name="flags">A bitwise combination of the <see cref="T:System.Windows.Forms.TextFormatFlags" /> values.</param>
		/// <returns>A <see cref="T:System.Drawing.Rectangle" /> that contains the area required to fit the rendered text. </returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="dc" /> is <see langword="null" />.</exception>
		// Token: 0x06004CEE RID: 19694 RVA: 0x0013C0E8 File Offset: 0x0013A2E8
		public Rectangle GetTextExtent(IDeviceContext dc, string textToDraw, TextFormatFlags flags)
		{
			if (dc == null)
			{
				throw new ArgumentNullException("dc");
			}
			if (string.IsNullOrEmpty(textToDraw))
			{
				throw new ArgumentNullException("textToDraw");
			}
			NativeMethods.COMRECT comrect = new NativeMethods.COMRECT();
			using (WindowsGraphicsWrapper windowsGraphicsWrapper = new WindowsGraphicsWrapper(dc, TextFormatFlags.PreserveGraphicsClipping | TextFormatFlags.PreserveGraphicsTranslateTransform))
			{
				HandleRef hdc = new HandleRef(windowsGraphicsWrapper, windowsGraphicsWrapper.WindowsGraphics.DeviceContext.Hdc);
				this.lastHResult = SafeNativeMethods.GetThemeTextExtent(new HandleRef(this, this.Handle), hdc, this.part, this.state, textToDraw, textToDraw.Length, (int)flags, null, comrect);
			}
			return Rectangle.FromLTRB(comrect.left, comrect.top, comrect.right, comrect.bottom);
		}

		/// <summary>Returns the size and location of the specified string when drawn with the font of the current visual style element within the specified initial bounding rectangle.</summary>
		/// <param name="dc">The <see cref="T:System.Drawing.IDeviceContext" /> this operation will use.</param>
		/// <param name="bounds">A <see cref="T:System.Drawing.Rectangle" /> used to control the flow and wrapping of the text.</param>
		/// <param name="textToDraw">The string to measure.</param>
		/// <param name="flags">A bitwise combination of the <see cref="T:System.Windows.Forms.TextFormatFlags" /> values.</param>
		/// <returns>A <see cref="T:System.Drawing.Rectangle" /> that contains the area required to fit the rendered text. </returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="dc" /> is <see langword="null" />.</exception>
		// Token: 0x06004CEF RID: 19695 RVA: 0x0013C1A8 File Offset: 0x0013A3A8
		public Rectangle GetTextExtent(IDeviceContext dc, Rectangle bounds, string textToDraw, TextFormatFlags flags)
		{
			if (dc == null)
			{
				throw new ArgumentNullException("dc");
			}
			if (string.IsNullOrEmpty(textToDraw))
			{
				throw new ArgumentNullException("textToDraw");
			}
			NativeMethods.COMRECT comrect = new NativeMethods.COMRECT();
			using (WindowsGraphicsWrapper windowsGraphicsWrapper = new WindowsGraphicsWrapper(dc, TextFormatFlags.PreserveGraphicsClipping | TextFormatFlags.PreserveGraphicsTranslateTransform))
			{
				HandleRef hdc = new HandleRef(windowsGraphicsWrapper, windowsGraphicsWrapper.WindowsGraphics.DeviceContext.Hdc);
				this.lastHResult = SafeNativeMethods.GetThemeTextExtent(new HandleRef(this, this.Handle), hdc, this.part, this.state, textToDraw, textToDraw.Length, (int)flags, new NativeMethods.COMRECT(bounds), comrect);
			}
			return Rectangle.FromLTRB(comrect.left, comrect.top, comrect.right, comrect.bottom);
		}

		/// <summary>Retrieves information about the font specified by the current visual style element.</summary>
		/// <param name="dc">The <see cref="T:System.Drawing.IDeviceContext" /> this operation will use.</param>
		/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.TextMetrics" /> that provides information about the font specified by the current visual style element. </returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="dc" /> is <see langword="null" />.</exception>
		// Token: 0x06004CF0 RID: 19696 RVA: 0x0013C270 File Offset: 0x0013A470
		public TextMetrics GetTextMetrics(IDeviceContext dc)
		{
			if (dc == null)
			{
				throw new ArgumentNullException("dc");
			}
			TextMetrics result = default(TextMetrics);
			using (WindowsGraphicsWrapper windowsGraphicsWrapper = new WindowsGraphicsWrapper(dc, TextFormatFlags.PreserveGraphicsClipping | TextFormatFlags.PreserveGraphicsTranslateTransform))
			{
				HandleRef hdc = new HandleRef(windowsGraphicsWrapper, windowsGraphicsWrapper.WindowsGraphics.DeviceContext.Hdc);
				this.lastHResult = SafeNativeMethods.GetThemeTextMetrics(new HandleRef(this, this.Handle), hdc, this.part, this.state, ref result);
			}
			return result;
		}

		/// <summary>Returns a hit test code indicating whether a point is contained in the background of the current visual style element.</summary>
		/// <param name="dc">The <see cref="T:System.Drawing.IDeviceContext" /> this operation will use.</param>
		/// <param name="backgroundRectangle">A <see cref="T:System.Drawing.Rectangle" /> that contains the background of the current visual style element.</param>
		/// <param name="pt">The <see cref="T:System.Drawing.Point" /> to test.</param>
		/// <param name="options">A bitwise combination of the <see cref="T:System.Windows.Forms.VisualStyles.HitTestOptions" /> values.</param>
		/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.HitTestCode" /> that describes where <paramref name="pt" /> is located in the background of the current visual style element.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="dc" /> is <see langword="null" />.</exception>
		// Token: 0x06004CF1 RID: 19697 RVA: 0x0013C2FC File Offset: 0x0013A4FC
		public HitTestCode HitTestBackground(IDeviceContext dc, Rectangle backgroundRectangle, Point pt, HitTestOptions options)
		{
			if (dc == null)
			{
				throw new ArgumentNullException("dc");
			}
			int result = 0;
			NativeMethods.POINTSTRUCT ptTest = new NativeMethods.POINTSTRUCT(pt.X, pt.Y);
			using (WindowsGraphicsWrapper windowsGraphicsWrapper = new WindowsGraphicsWrapper(dc, TextFormatFlags.PreserveGraphicsClipping | TextFormatFlags.PreserveGraphicsTranslateTransform))
			{
				HandleRef hdc = new HandleRef(windowsGraphicsWrapper, windowsGraphicsWrapper.WindowsGraphics.DeviceContext.Hdc);
				this.lastHResult = SafeNativeMethods.HitTestThemeBackground(new HandleRef(this, this.Handle), hdc, this.part, this.state, (int)options, new NativeMethods.COMRECT(backgroundRectangle), NativeMethods.NullHandleRef, ptTest, ref result);
			}
			return (HitTestCode)result;
		}

		/// <summary>Returns a hit test code indicating whether the point is contained in the background of the current visual style element and within the specified bounds.</summary>
		/// <param name="g">The <see cref="T:System.Drawing.Graphics" /> this operation will use.</param>
		/// <param name="backgroundRectangle">A <see cref="T:System.Drawing.Rectangle" /> that contains the background of the current visual style element.</param>
		/// <param name="region">A <see cref="T:System.Drawing.Region" /> that specifies the bounds of the hit test area within the background.</param>
		/// <param name="pt">The <see cref="T:System.Drawing.Point" /> to test.</param>
		/// <param name="options">A bitwise combination of the <see cref="T:System.Windows.Forms.VisualStyles.HitTestOptions" /> values.</param>
		/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.HitTestCode" /> that describes where <paramref name="pt" /> is located in the background of the current visual style element, if at all.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="g" /> is <see langword="null" />.</exception>
		// Token: 0x06004CF2 RID: 19698 RVA: 0x0013C3A4 File Offset: 0x0013A5A4
		public HitTestCode HitTestBackground(Graphics g, Rectangle backgroundRectangle, Region region, Point pt, HitTestOptions options)
		{
			if (g == null)
			{
				throw new ArgumentNullException("g");
			}
			IntPtr hrgn = region.GetHrgn(g);
			return this.HitTestBackground(g, backgroundRectangle, hrgn, pt, options);
		}

		/// <summary>Returns a hit test code indicating whether the point is contained in the background of the current visual style element and within the specified region.</summary>
		/// <param name="dc">The <see cref="T:System.Drawing.IDeviceContext" /> this operation will use.</param>
		/// <param name="backgroundRectangle">A <see cref="T:System.Drawing.Rectangle" /> that contains the background of the current visual style element.</param>
		/// <param name="hRgn">A Windows handle to a <see cref="T:System.Drawing.Region" /> that specifies the bounds of the hit test area within the background.</param>
		/// <param name="pt">The <see cref="T:System.Drawing.Point" /> to test.</param>
		/// <param name="options">A bitwise combination of the <see cref="T:System.Windows.Forms.VisualStyles.HitTestOptions" /> values.</param>
		/// <returns>A <see cref="T:System.Windows.Forms.VisualStyles.HitTestCode" /> that describes where <paramref name="pt" /> is located in the background of the current visual style element.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="dc" /> is <see langword="null" />.</exception>
		// Token: 0x06004CF3 RID: 19699 RVA: 0x0013C3D4 File Offset: 0x0013A5D4
		public HitTestCode HitTestBackground(IDeviceContext dc, Rectangle backgroundRectangle, IntPtr hRgn, Point pt, HitTestOptions options)
		{
			if (dc == null)
			{
				throw new ArgumentNullException("dc");
			}
			int result = 0;
			NativeMethods.POINTSTRUCT ptTest = new NativeMethods.POINTSTRUCT(pt.X, pt.Y);
			using (WindowsGraphicsWrapper windowsGraphicsWrapper = new WindowsGraphicsWrapper(dc, TextFormatFlags.PreserveGraphicsClipping | TextFormatFlags.PreserveGraphicsTranslateTransform))
			{
				HandleRef hdc = new HandleRef(windowsGraphicsWrapper, windowsGraphicsWrapper.WindowsGraphics.DeviceContext.Hdc);
				this.lastHResult = SafeNativeMethods.HitTestThemeBackground(new HandleRef(this, this.Handle), hdc, this.part, this.state, (int)options, new NativeMethods.COMRECT(backgroundRectangle), new HandleRef(this, hRgn), ptTest, ref result);
			}
			return (HitTestCode)result;
		}

		/// <summary>Indicates whether the background of the current visual style element has any semitransparent or alpha-blended pieces.</summary>
		/// <returns>
		///     <see langword="true" /> if the background of the current visual style element has any semitransparent or alpha-blended pieces; otherwise, <see langword="false" />.</returns>
		// Token: 0x06004CF4 RID: 19700 RVA: 0x0013C480 File Offset: 0x0013A680
		public bool IsBackgroundPartiallyTransparent()
		{
			return SafeNativeMethods.IsThemeBackgroundPartiallyTransparent(new HandleRef(this, this.Handle), this.part, this.state);
		}

		/// <summary>Gets the last error code returned by the native visual styles (UxTheme) API methods encapsulated by the <see cref="T:System.Windows.Forms.VisualStyles.VisualStyleRenderer" /> class.</summary>
		/// <returns>A value specifying the last error code returned by the native visual styles API methods that this class encapsulates.</returns>
		// Token: 0x170012F0 RID: 4848
		// (get) Token: 0x06004CF5 RID: 19701 RVA: 0x0013C49F File Offset: 0x0013A69F
		public int LastHResult
		{
			get
			{
				return this.lastHResult;
			}
		}

		// Token: 0x06004CF6 RID: 19702 RVA: 0x0013C4A7 File Offset: 0x0013A6A7
		private static void CreateThemeHandleHashtable()
		{
			VisualStyleRenderer.themeHandles = new Hashtable(VisualStyleRenderer.numberOfPossibleClasses);
		}

		// Token: 0x06004CF7 RID: 19703 RVA: 0x0013C4B8 File Offset: 0x0013A6B8
		private static void OnUserPreferenceChanging(object sender, UserPreferenceChangingEventArgs ea)
		{
			if (ea.Category == UserPreferenceCategory.VisualStyle)
			{
				VisualStyleRenderer.globalCacheVersion += 1L;
			}
		}

		// Token: 0x06004CF8 RID: 19704 RVA: 0x0013C4D4 File Offset: 0x0013A6D4
		private static void RefreshCache()
		{
			if (VisualStyleRenderer.themeHandles != null)
			{
				string[] array = new string[VisualStyleRenderer.themeHandles.Keys.Count];
				VisualStyleRenderer.themeHandles.Keys.CopyTo(array, 0);
				foreach (string text in array)
				{
					VisualStyleRenderer.ThemeHandle themeHandle = (VisualStyleRenderer.ThemeHandle)VisualStyleRenderer.themeHandles[text];
					if (themeHandle != null)
					{
						themeHandle.Dispose();
					}
					if (VisualStyleRenderer.AreClientAreaVisualStylesSupported)
					{
						themeHandle = VisualStyleRenderer.ThemeHandle.Create(text, false);
						if (themeHandle != null)
						{
							VisualStyleRenderer.themeHandles[text] = themeHandle;
						}
					}
				}
			}
		}

		// Token: 0x06004CF9 RID: 19705 RVA: 0x0013C560 File Offset: 0x0013A760
		private static IntPtr GetHandle(string className)
		{
			return VisualStyleRenderer.GetHandle(className, true);
		}

		// Token: 0x06004CFA RID: 19706 RVA: 0x0013C56C File Offset: 0x0013A76C
		private static IntPtr GetHandle(string className, bool throwExceptionOnFail)
		{
			if (VisualStyleRenderer.themeHandles == null)
			{
				VisualStyleRenderer.CreateThemeHandleHashtable();
			}
			if (VisualStyleRenderer.threadCacheVersion != VisualStyleRenderer.globalCacheVersion)
			{
				VisualStyleRenderer.RefreshCache();
				VisualStyleRenderer.threadCacheVersion = VisualStyleRenderer.globalCacheVersion;
			}
			VisualStyleRenderer.ThemeHandle themeHandle;
			if (!VisualStyleRenderer.themeHandles.Contains(className))
			{
				themeHandle = VisualStyleRenderer.ThemeHandle.Create(className, throwExceptionOnFail);
				if (themeHandle == null)
				{
					return IntPtr.Zero;
				}
				VisualStyleRenderer.themeHandles.Add(className, themeHandle);
			}
			else
			{
				themeHandle = (VisualStyleRenderer.ThemeHandle)VisualStyleRenderer.themeHandles[className];
			}
			return themeHandle.NativeHandle;
		}

		// Token: 0x04003167 RID: 12647
		private const TextFormatFlags AllGraphicsProperties = TextFormatFlags.PreserveGraphicsClipping | TextFormatFlags.PreserveGraphicsTranslateTransform;

		// Token: 0x04003168 RID: 12648
		internal const int EdgeAdjust = 8192;

		// Token: 0x04003169 RID: 12649
		private string _class;

		// Token: 0x0400316A RID: 12650
		private int part;

		// Token: 0x0400316B RID: 12651
		private int state;

		// Token: 0x0400316C RID: 12652
		private int lastHResult;

		// Token: 0x0400316D RID: 12653
		private static int numberOfPossibleClasses = VisualStyleElement.Count;

		// Token: 0x0400316E RID: 12654
		[ThreadStatic]
		private static Hashtable themeHandles = null;

		// Token: 0x0400316F RID: 12655
		[ThreadStatic]
		private static long threadCacheVersion = 0L;

		// Token: 0x04003170 RID: 12656
		private static long globalCacheVersion = 0L;

		// Token: 0x02000824 RID: 2084
		private class ThemeHandle : IDisposable
		{
			// Token: 0x06006E6C RID: 28268 RVA: 0x001940B2 File Offset: 0x001922B2
			private ThemeHandle(IntPtr hTheme)
			{
				this._hTheme = hTheme;
			}

			// Token: 0x170017D4 RID: 6100
			// (get) Token: 0x06006E6D RID: 28269 RVA: 0x001940CC File Offset: 0x001922CC
			public IntPtr NativeHandle
			{
				get
				{
					return this._hTheme;
				}
			}

			// Token: 0x06006E6E RID: 28270 RVA: 0x001940D4 File Offset: 0x001922D4
			public static VisualStyleRenderer.ThemeHandle Create(string className, bool throwExceptionOnFail)
			{
				return VisualStyleRenderer.ThemeHandle.Create(className, throwExceptionOnFail, new HandleRef(null, IntPtr.Zero));
			}

			// Token: 0x06006E6F RID: 28271 RVA: 0x001940E8 File Offset: 0x001922E8
			internal static VisualStyleRenderer.ThemeHandle Create(string className, bool throwExceptionOnFail, HandleRef hWndRef)
			{
				IntPtr intPtr = IntPtr.Zero;
				try
				{
					intPtr = SafeNativeMethods.OpenThemeData(hWndRef, className);
				}
				catch (Exception ex)
				{
					if (ClientUtils.IsSecurityOrCriticalException(ex))
					{
						throw;
					}
					if (throwExceptionOnFail)
					{
						throw new InvalidOperationException(SR.GetString("VisualStyleHandleCreationFailed"), ex);
					}
					return null;
				}
				if (!(intPtr == IntPtr.Zero))
				{
					return new VisualStyleRenderer.ThemeHandle(intPtr);
				}
				if (throwExceptionOnFail)
				{
					throw new InvalidOperationException(SR.GetString("VisualStyleHandleCreationFailed"));
				}
				return null;
			}

			// Token: 0x06006E70 RID: 28272 RVA: 0x00194164 File Offset: 0x00192364
			public void Dispose()
			{
				if (this._hTheme != IntPtr.Zero)
				{
					SafeNativeMethods.CloseThemeData(new HandleRef(null, this._hTheme));
					this._hTheme = IntPtr.Zero;
				}
				GC.SuppressFinalize(this);
			}

			// Token: 0x06006E71 RID: 28273 RVA: 0x0019419C File Offset: 0x0019239C
			~ThemeHandle()
			{
				this.Dispose();
			}

			// Token: 0x04004266 RID: 16998
			private IntPtr _hTheme = IntPtr.Zero;
		}
	}
}
