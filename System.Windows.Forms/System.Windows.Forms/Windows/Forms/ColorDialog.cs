using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.Windows.Forms
{
	/// <summary>Represents a common dialog box that displays available colors along with controls that enable the user to define custom colors.</summary>
	// Token: 0x02000142 RID: 322
	[DefaultProperty("Color")]
	[SRDescription("DescriptionColorDialog")]
	public class ColorDialog : CommonDialog
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.ColorDialog" /> class.</summary>
		// Token: 0x06000A32 RID: 2610 RVA: 0x0001EEC7 File Offset: 0x0001D0C7
		public ColorDialog()
		{
			this.customColors = new int[16];
			this.Reset();
		}

		/// <summary>Gets or sets a value indicating whether the user can use the dialog box to define custom colors.</summary>
		/// <returns>
		///     <see langword="true" /> if the user can define custom colors; otherwise, <see langword="false" />. The default is <see langword="true" />.</returns>
		// Token: 0x170002D7 RID: 727
		// (get) Token: 0x06000A33 RID: 2611 RVA: 0x0001EEE2 File Offset: 0x0001D0E2
		// (set) Token: 0x06000A34 RID: 2612 RVA: 0x0001EEEE File Offset: 0x0001D0EE
		[SRCategory("CatBehavior")]
		[DefaultValue(true)]
		[SRDescription("CDallowFullOpenDescr")]
		public virtual bool AllowFullOpen
		{
			get
			{
				return !this.GetOption(4);
			}
			set
			{
				this.SetOption(4, !value);
			}
		}

		/// <summary>Gets or sets a value indicating whether the dialog box displays all available colors in the set of basic colors.</summary>
		/// <returns>
		///     <see langword="true" /> if the dialog box displays all available colors in the set of basic colors; otherwise, <see langword="false" />. The default value is <see langword="false" />.</returns>
		// Token: 0x170002D8 RID: 728
		// (get) Token: 0x06000A35 RID: 2613 RVA: 0x0001EEFB File Offset: 0x0001D0FB
		// (set) Token: 0x06000A36 RID: 2614 RVA: 0x0001EF08 File Offset: 0x0001D108
		[SRCategory("CatBehavior")]
		[DefaultValue(false)]
		[SRDescription("CDanyColorDescr")]
		public virtual bool AnyColor
		{
			get
			{
				return this.GetOption(256);
			}
			set
			{
				this.SetOption(256, value);
			}
		}

		/// <summary>Gets or sets the color selected by the user.</summary>
		/// <returns>The color selected by the user. If a color is not selected, the default value is black.</returns>
		// Token: 0x170002D9 RID: 729
		// (get) Token: 0x06000A37 RID: 2615 RVA: 0x0001EF16 File Offset: 0x0001D116
		// (set) Token: 0x06000A38 RID: 2616 RVA: 0x0001EF1E File Offset: 0x0001D11E
		[SRCategory("CatData")]
		[SRDescription("CDcolorDescr")]
		public Color Color
		{
			get
			{
				return this.color;
			}
			set
			{
				if (!value.IsEmpty)
				{
					this.color = value;
					return;
				}
				this.color = Color.Black;
			}
		}

		/// <summary>Gets or sets the set of custom colors shown in the dialog box.</summary>
		/// <returns>A set of custom colors shown by the dialog box. The default value is <see langword="null" />.</returns>
		// Token: 0x170002DA RID: 730
		// (get) Token: 0x06000A39 RID: 2617 RVA: 0x0001EF3C File Offset: 0x0001D13C
		// (set) Token: 0x06000A3A RID: 2618 RVA: 0x0001EF50 File Offset: 0x0001D150
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[SRDescription("CDcustomColorsDescr")]
		public int[] CustomColors
		{
			get
			{
				return (int[])this.customColors.Clone();
			}
			set
			{
				int num = (value == null) ? 0 : Math.Min(value.Length, 16);
				if (num > 0)
				{
					Array.Copy(value, 0, this.customColors, 0, num);
				}
				for (int i = num; i < 16; i++)
				{
					this.customColors[i] = 16777215;
				}
			}
		}

		/// <summary>Gets or sets a value indicating whether the controls used to create custom colors are visible when the dialog box is opened </summary>
		/// <returns>
		///     <see langword="true" /> if the custom color controls are available when the dialog box is opened; otherwise, <see langword="false" />. The default value is <see langword="false" />.</returns>
		// Token: 0x170002DB RID: 731
		// (get) Token: 0x06000A3B RID: 2619 RVA: 0x0001EF9B File Offset: 0x0001D19B
		// (set) Token: 0x06000A3C RID: 2620 RVA: 0x0001EFA4 File Offset: 0x0001D1A4
		[SRCategory("CatAppearance")]
		[DefaultValue(false)]
		[SRDescription("CDfullOpenDescr")]
		public virtual bool FullOpen
		{
			get
			{
				return this.GetOption(2);
			}
			set
			{
				this.SetOption(2, value);
			}
		}

		/// <summary>Gets the underlying window instance handle (HINSTANCE).</summary>
		/// <returns>An <see cref="T:System.IntPtr" /> that contains the HINSTANCE value of the window handle.</returns>
		// Token: 0x170002DC RID: 732
		// (get) Token: 0x06000A3D RID: 2621 RVA: 0x0001EFAE File Offset: 0x0001D1AE
		protected virtual IntPtr Instance
		{
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			[SecurityPermission(SecurityAction.InheritanceDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			get
			{
				return UnsafeNativeMethods.GetModuleHandle(null);
			}
		}

		/// <summary>Gets values to initialize the <see cref="T:System.Windows.Forms.ColorDialog" />.</summary>
		/// <returns>A bitwise combination of internal values that initializes the <see cref="T:System.Windows.Forms.ColorDialog" />.</returns>
		// Token: 0x170002DD RID: 733
		// (get) Token: 0x06000A3E RID: 2622 RVA: 0x0001EFB6 File Offset: 0x0001D1B6
		protected virtual int Options
		{
			get
			{
				return this.options;
			}
		}

		/// <summary>Gets or sets a value indicating whether a Help button appears in the color dialog box.</summary>
		/// <returns>
		///     <see langword="true" /> if the Help button is shown in the dialog box; otherwise, <see langword="false" />. The default value is <see langword="false" />.</returns>
		// Token: 0x170002DE RID: 734
		// (get) Token: 0x06000A3F RID: 2623 RVA: 0x0001EFBE File Offset: 0x0001D1BE
		// (set) Token: 0x06000A40 RID: 2624 RVA: 0x0001EFC7 File Offset: 0x0001D1C7
		[SRCategory("CatBehavior")]
		[DefaultValue(false)]
		[SRDescription("CDshowHelpDescr")]
		public virtual bool ShowHelp
		{
			get
			{
				return this.GetOption(8);
			}
			set
			{
				this.SetOption(8, value);
			}
		}

		/// <summary>Gets or sets a value indicating whether the dialog box will restrict users to selecting solid colors only.</summary>
		/// <returns>
		///     <see langword="true" /> if users can select only solid colors; otherwise, <see langword="false" />. The default value is <see langword="false" />.</returns>
		// Token: 0x170002DF RID: 735
		// (get) Token: 0x06000A41 RID: 2625 RVA: 0x0001EFD1 File Offset: 0x0001D1D1
		// (set) Token: 0x06000A42 RID: 2626 RVA: 0x0001EFDE File Offset: 0x0001D1DE
		[SRCategory("CatBehavior")]
		[DefaultValue(false)]
		[SRDescription("CDsolidColorOnlyDescr")]
		public virtual bool SolidColorOnly
		{
			get
			{
				return this.GetOption(128);
			}
			set
			{
				this.SetOption(128, value);
			}
		}

		// Token: 0x06000A43 RID: 2627 RVA: 0x0001EFEC File Offset: 0x0001D1EC
		private bool GetOption(int option)
		{
			return (this.options & option) != 0;
		}

		/// <summary>Resets all options to their default values, the last selected color to black, and the custom colors to their default values.</summary>
		// Token: 0x06000A44 RID: 2628 RVA: 0x0001EFF9 File Offset: 0x0001D1F9
		public override void Reset()
		{
			this.options = 0;
			this.color = Color.Black;
			this.CustomColors = null;
		}

		// Token: 0x06000A45 RID: 2629 RVA: 0x0001F014 File Offset: 0x0001D214
		private void ResetColor()
		{
			this.Color = Color.Black;
		}

		/// <summary>When overridden in a derived class, specifies a common dialog box.</summary>
		/// <param name="hwndOwner">A value that represents the window handle of the owner window for the common dialog box.</param>
		/// <returns>
		///   <see langword="true" /> if the dialog box was successfully run; otherwise, <see langword="false" />.</returns>
		// Token: 0x06000A46 RID: 2630 RVA: 0x0001F024 File Offset: 0x0001D224
		protected override bool RunDialog(IntPtr hwndOwner)
		{
			NativeMethods.WndProc lpfnHook = new NativeMethods.WndProc(this.HookProc);
			NativeMethods.CHOOSECOLOR choosecolor = new NativeMethods.CHOOSECOLOR();
			IntPtr intPtr = Marshal.AllocCoTaskMem(64);
			bool result;
			try
			{
				Marshal.Copy(this.customColors, 0, intPtr, 16);
				choosecolor.hwndOwner = hwndOwner;
				choosecolor.hInstance = this.Instance;
				choosecolor.rgbResult = ColorTranslator.ToWin32(this.color);
				choosecolor.lpCustColors = intPtr;
				int num = this.Options | 17;
				if (!this.AllowFullOpen)
				{
					num &= -3;
				}
				choosecolor.Flags = num;
				choosecolor.lpfnHook = lpfnHook;
				if (!SafeNativeMethods.ChooseColor(choosecolor))
				{
					result = false;
				}
				else
				{
					if (choosecolor.rgbResult != ColorTranslator.ToWin32(this.color))
					{
						this.color = ColorTranslator.FromOle(choosecolor.rgbResult);
					}
					Marshal.Copy(intPtr, this.customColors, 0, 16);
					result = true;
				}
			}
			finally
			{
				Marshal.FreeCoTaskMem(intPtr);
			}
			return result;
		}

		// Token: 0x06000A47 RID: 2631 RVA: 0x0001F10C File Offset: 0x0001D30C
		private void SetOption(int option, bool value)
		{
			if (value)
			{
				this.options |= option;
				return;
			}
			this.options &= ~option;
		}

		// Token: 0x06000A48 RID: 2632 RVA: 0x0001F130 File Offset: 0x0001D330
		private bool ShouldSerializeColor()
		{
			return !this.Color.Equals(Color.Black);
		}

		/// <summary>Returns a string that represents the <see cref="T:System.Windows.Forms.ColorDialog" />.</summary>
		/// <returns>A <see cref="T:System.String" /> that represents the current <see cref="T:System.Windows.Forms.ColorDialog" />. </returns>
		// Token: 0x06000A49 RID: 2633 RVA: 0x0001F160 File Offset: 0x0001D360
		public override string ToString()
		{
			string str = base.ToString();
			return str + ",  Color: " + this.Color.ToString();
		}

		// Token: 0x040006DB RID: 1755
		private int options;

		// Token: 0x040006DC RID: 1756
		private int[] customColors;

		// Token: 0x040006DD RID: 1757
		private Color color;
	}
}
