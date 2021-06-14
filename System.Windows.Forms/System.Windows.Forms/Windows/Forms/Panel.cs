using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Windows.Forms.Layout;

namespace System.Windows.Forms
{
	/// <summary>Used to group collections of controls.</summary>
	// Token: 0x02000309 RID: 777
	[ComVisible(true)]
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[DefaultProperty("BorderStyle")]
	[DefaultEvent("Paint")]
	[Docking(DockingBehavior.Ask)]
	[Designer("System.Windows.Forms.Design.PanelDesigner, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	[SRDescription("DescriptionPanel")]
	public class Panel : ScrollableControl
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.Panel" /> class.</summary>
		// Token: 0x06002F03 RID: 12035 RVA: 0x000D9E45 File Offset: 0x000D8045
		public Panel()
		{
			base.SetState2(2048, true);
			this.TabStop = false;
			base.SetStyle(ControlStyles.Selectable | ControlStyles.AllPaintingInWmPaint, false);
			base.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
		}

		/// <summary>Gets or sets a value that indicates whether the control resizes based on its contents.</summary>
		/// <returns>
		///     <see langword="true" /> if the control automatically resizes based on its contents; otherwise, <see langword="false" />. The default is <see langword="true" />.</returns>
		// Token: 0x17000B56 RID: 2902
		// (get) Token: 0x06002F04 RID: 12036 RVA: 0x0001BA13 File Offset: 0x00019C13
		// (set) Token: 0x06002F05 RID: 12037 RVA: 0x000B0BCE File Offset: 0x000AEDCE
		[Browsable(true)]
		[EditorBrowsable(EditorBrowsableState.Always)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		public override bool AutoSize
		{
			get
			{
				return base.AutoSize;
			}
			set
			{
				base.AutoSize = value;
			}
		}

		/// <summary>Occurs when the value of the <see cref="P:System.Windows.Forms.Panel.AutoSize" /> property has changed.</summary>
		// Token: 0x1400022F RID: 559
		// (add) Token: 0x06002F06 RID: 12038 RVA: 0x0001BA2E File Offset: 0x00019C2E
		// (remove) Token: 0x06002F07 RID: 12039 RVA: 0x0001BA37 File Offset: 0x00019C37
		[SRCategory("CatPropertyChanged")]
		[SRDescription("ControlOnAutoSizeChangedDescr")]
		[Browsable(true)]
		[EditorBrowsable(EditorBrowsableState.Always)]
		public new event EventHandler AutoSizeChanged
		{
			add
			{
				base.AutoSizeChanged += value;
			}
			remove
			{
				base.AutoSizeChanged -= value;
			}
		}

		/// <summary>Indicates the automatic sizing behavior of the control.</summary>
		/// <returns>One of the <see cref="T:System.Windows.Forms.AutoSizeMode" /> values.</returns>
		/// <exception cref="T:System.ComponentModel.InvalidEnumArgumentException">The specified value when setting this property is not a valid <see cref="T:System.Windows.Forms.AutoSizeMode" /> values.</exception>
		// Token: 0x17000B57 RID: 2903
		// (get) Token: 0x06002F08 RID: 12040 RVA: 0x0001B4F5 File Offset: 0x000196F5
		// (set) Token: 0x06002F09 RID: 12041 RVA: 0x000D9E78 File Offset: 0x000D8078
		[SRDescription("ControlAutoSizeModeDescr")]
		[SRCategory("CatLayout")]
		[Browsable(true)]
		[DefaultValue(AutoSizeMode.GrowOnly)]
		[Localizable(true)]
		public virtual AutoSizeMode AutoSizeMode
		{
			get
			{
				return base.GetAutoSizeMode();
			}
			set
			{
				if (!ClientUtils.IsEnumValid(value, (int)value, 0, 1))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(AutoSizeMode));
				}
				if (base.GetAutoSizeMode() != value)
				{
					base.SetAutoSizeMode(value);
					if (this.ParentInternal != null)
					{
						if (this.ParentInternal.LayoutEngine == DefaultLayout.Instance)
						{
							this.ParentInternal.LayoutEngine.InitLayout(this, BoundsSpecified.Size);
						}
						LayoutTransaction.DoLayout(this.ParentInternal, this, PropertyNames.AutoSize);
					}
				}
			}
		}

		/// <summary>Indicates the border style for the control.</summary>
		/// <returns>One of the <see cref="T:System.Windows.Forms.BorderStyle" /> values. The default is <see langword="BorderStyle.None" />.</returns>
		/// <exception cref="T:System.ComponentModel.InvalidEnumArgumentException">The specified value when setting this property is not a valid <see cref="T:System.Windows.Forms.BorderStyle" /> value.</exception>
		// Token: 0x17000B58 RID: 2904
		// (get) Token: 0x06002F0A RID: 12042 RVA: 0x000D9EF9 File Offset: 0x000D80F9
		// (set) Token: 0x06002F0B RID: 12043 RVA: 0x000D9F01 File Offset: 0x000D8101
		[SRCategory("CatAppearance")]
		[DefaultValue(BorderStyle.None)]
		[DispId(-504)]
		[SRDescription("PanelBorderStyleDescr")]
		public BorderStyle BorderStyle
		{
			get
			{
				return this.borderStyle;
			}
			set
			{
				if (this.borderStyle != value)
				{
					if (!ClientUtils.IsEnumValid(value, (int)value, 0, 2))
					{
						throw new InvalidEnumArgumentException("value", (int)value, typeof(BorderStyle));
					}
					this.borderStyle = value;
					base.UpdateStyles();
				}
			}
		}

		/// <summary>Gets the required creation parameters when the control handle is created.</summary>
		/// <returns>A <see cref="T:System.Windows.Forms.CreateParams" /> that contains the required creation parameters when the handle to the control is created.</returns>
		// Token: 0x17000B59 RID: 2905
		// (get) Token: 0x06002F0C RID: 12044 RVA: 0x000D9F40 File Offset: 0x000D8140
		protected override CreateParams CreateParams
		{
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			get
			{
				CreateParams createParams = base.CreateParams;
				createParams.ExStyle |= 65536;
				createParams.ExStyle &= -513;
				createParams.Style &= -8388609;
				BorderStyle borderStyle = this.borderStyle;
				if (borderStyle != BorderStyle.FixedSingle)
				{
					if (borderStyle == BorderStyle.Fixed3D)
					{
						createParams.ExStyle |= 512;
					}
				}
				else
				{
					createParams.Style |= 8388608;
				}
				return createParams;
			}
		}

		/// <summary>Gets the default size of the control.</summary>
		/// <returns>The default <see cref="T:System.Drawing.Size" /> of the control.</returns>
		// Token: 0x17000B5A RID: 2906
		// (get) Token: 0x06002F0D RID: 12045 RVA: 0x000B0CC4 File Offset: 0x000AEEC4
		protected override Size DefaultSize
		{
			get
			{
				return new Size(200, 100);
			}
		}

		// Token: 0x06002F0E RID: 12046 RVA: 0x000D9FC0 File Offset: 0x000D81C0
		internal override Size GetPreferredSizeCore(Size proposedSize)
		{
			Size sz = this.SizeFromClientSize(Size.Empty);
			Size sz2 = sz + base.Padding.Size;
			return this.LayoutEngine.GetPreferredSize(this, proposedSize - sz2) + sz2;
		}

		/// <summary>This member is not meaningful for this control.</summary>
		// Token: 0x14000230 RID: 560
		// (add) Token: 0x06002F0F RID: 12047 RVA: 0x000B0E8C File Offset: 0x000AF08C
		// (remove) Token: 0x06002F10 RID: 12048 RVA: 0x000B0E95 File Offset: 0x000AF095
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event KeyEventHandler KeyUp
		{
			add
			{
				base.KeyUp += value;
			}
			remove
			{
				base.KeyUp -= value;
			}
		}

		/// <summary>This member is not meaningful for this control.</summary>
		// Token: 0x14000231 RID: 561
		// (add) Token: 0x06002F11 RID: 12049 RVA: 0x000B0E9E File Offset: 0x000AF09E
		// (remove) Token: 0x06002F12 RID: 12050 RVA: 0x000B0EA7 File Offset: 0x000AF0A7
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event KeyEventHandler KeyDown
		{
			add
			{
				base.KeyDown += value;
			}
			remove
			{
				base.KeyDown -= value;
			}
		}

		/// <summary>This member is not meaningful for this control.</summary>
		// Token: 0x14000232 RID: 562
		// (add) Token: 0x06002F13 RID: 12051 RVA: 0x000B0EB0 File Offset: 0x000AF0B0
		// (remove) Token: 0x06002F14 RID: 12052 RVA: 0x000B0EB9 File Offset: 0x000AF0B9
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event KeyPressEventHandler KeyPress
		{
			add
			{
				base.KeyPress += value;
			}
			remove
			{
				base.KeyPress -= value;
			}
		}

		/// <summary>Gets or sets a value indicating whether the user can give the focus to this control using the TAB key.</summary>
		/// <returns>
		///     <see langword="true" /> if the user can give the focus to the control using the TAB key; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
		// Token: 0x17000B5B RID: 2907
		// (get) Token: 0x06002F15 RID: 12053 RVA: 0x000AA115 File Offset: 0x000A8315
		// (set) Token: 0x06002F16 RID: 12054 RVA: 0x000AA11D File Offset: 0x000A831D
		[DefaultValue(false)]
		public new bool TabStop
		{
			get
			{
				return base.TabStop;
			}
			set
			{
				base.TabStop = value;
			}
		}

		/// <summary>This member is not meaningful for this control.</summary>
		/// <returns>The text associated with this control.</returns>
		// Token: 0x17000B5C RID: 2908
		// (get) Token: 0x06002F17 RID: 12055 RVA: 0x0001BFA5 File Offset: 0x0001A1A5
		// (set) Token: 0x06002F18 RID: 12056 RVA: 0x0001BFAD File Offset: 0x0001A1AD
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Bindable(false)]
		public override string Text
		{
			get
			{
				return base.Text;
			}
			set
			{
				base.Text = value;
			}
		}

		/// <summary>This member is not meaningful for this control.</summary>
		// Token: 0x14000233 RID: 563
		// (add) Token: 0x06002F19 RID: 12057 RVA: 0x0003E435 File Offset: 0x0003C635
		// (remove) Token: 0x06002F1A RID: 12058 RVA: 0x0003E43E File Offset: 0x0003C63E
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event EventHandler TextChanged
		{
			add
			{
				base.TextChanged += value;
			}
			remove
			{
				base.TextChanged -= value;
			}
		}

		/// <summary>Fires the event indicating that the panel has been resized. Inheriting controls should use this in favor of actually listening to the event, but should still call <see langword="base.onResize" /> to ensure that the event is fired for external listeners.</summary>
		/// <param name="eventargs">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x06002F1B RID: 12059 RVA: 0x000DA007 File Offset: 0x000D8207
		protected override void OnResize(EventArgs eventargs)
		{
			if (base.DesignMode && this.borderStyle == BorderStyle.None)
			{
				base.Invalidate();
			}
			base.OnResize(eventargs);
		}

		// Token: 0x06002F1C RID: 12060 RVA: 0x000DA028 File Offset: 0x000D8228
		internal override void PrintToMetaFileRecursive(HandleRef hDC, IntPtr lParam, Rectangle bounds)
		{
			base.PrintToMetaFileRecursive(hDC, lParam, bounds);
			using (new WindowsFormsUtils.DCMapping(hDC, bounds))
			{
				using (Graphics graphics = Graphics.FromHdcInternal(hDC.Handle))
				{
					ControlPaint.PrintBorder(graphics, new Rectangle(Point.Empty, base.Size), this.BorderStyle, Border3DStyle.Sunken);
				}
			}
		}

		// Token: 0x06002F1D RID: 12061 RVA: 0x000DA0A8 File Offset: 0x000D82A8
		private static string StringFromBorderStyle(BorderStyle value)
		{
			Type typeFromHandle = typeof(BorderStyle);
			if (!ClientUtils.IsEnumValid(value, (int)value, 0, 2))
			{
				return "[Invalid BorderStyle]";
			}
			return typeFromHandle.ToString() + "." + value.ToString();
		}

		/// <summary>Returns a string representation for this control.</summary>
		/// <returns>A <see cref="T:System.String" /> representation of the control.</returns>
		// Token: 0x06002F1E RID: 12062 RVA: 0x000DA0F4 File Offset: 0x000D82F4
		public override string ToString()
		{
			string str = base.ToString();
			return str + ", BorderStyle: " + Panel.StringFromBorderStyle(this.borderStyle);
		}

		// Token: 0x04001D55 RID: 7509
		private BorderStyle borderStyle;
	}
}
