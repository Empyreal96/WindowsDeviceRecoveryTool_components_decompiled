using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Security;
using System.Security.Permissions;
using System.Text;
using System.Windows.Forms.Layout;
using Microsoft.Win32;

namespace System.Windows.Forms
{
	/// <summary>Represents a Windows rich text box control.</summary>
	// Token: 0x02000335 RID: 821
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[ComVisible(true)]
	[Docking(DockingBehavior.Ask)]
	[Designer("System.Windows.Forms.Design.RichTextBoxDesigner, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	[SRDescription("DescriptionRichTextBox")]
	public class RichTextBox : TextBoxBase
	{
		// Token: 0x17000C7F RID: 3199
		// (get) Token: 0x06003285 RID: 12933 RVA: 0x000EAF9A File Offset: 0x000E919A
		private static TraceSwitch RichTextDbg
		{
			get
			{
				if (RichTextBox.richTextDbg == null)
				{
					RichTextBox.richTextDbg = new TraceSwitch("RichTextDbg", "Debug info about RichTextBox");
				}
				return RichTextBox.richTextDbg;
			}
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.RichTextBox" /> class.</summary>
		// Token: 0x06003286 RID: 12934 RVA: 0x000EAFBC File Offset: 0x000E91BC
		public RichTextBox()
		{
			this.InConstructor = true;
			this.richTextBoxFlags[RichTextBox.autoWordSelectionSection] = 0;
			this.DetectUrls = true;
			this.ScrollBars = RichTextBoxScrollBars.Both;
			this.RichTextShortcutsEnabled = true;
			this.MaxLength = int.MaxValue;
			this.Multiline = true;
			this.AutoSize = false;
			this.curSelStart = (this.curSelEnd = (int)(this.curSelType = -1));
			this.InConstructor = false;
		}

		/// <summary>Gets or sets a value indicating whether the control will enable drag-and-drop operations.</summary>
		/// <returns>
		///     <see langword="true" /> if drag-and-drop is enabled in the control; otherwise, <see langword="false" />.</returns>
		// Token: 0x17000C80 RID: 3200
		// (get) Token: 0x06003287 RID: 12935 RVA: 0x000EB04B File Offset: 0x000E924B
		// (set) Token: 0x06003288 RID: 12936 RVA: 0x000EB060 File Offset: 0x000E9260
		[Browsable(false)]
		public override bool AllowDrop
		{
			get
			{
				return this.richTextBoxFlags[RichTextBox.allowOleDropSection] != 0;
			}
			set
			{
				if (value)
				{
					try
					{
						IntSecurity.ClipboardRead.Demand();
					}
					catch (Exception innerException)
					{
						throw new InvalidOperationException(SR.GetString("DragDropRegFailed"), innerException);
					}
				}
				this.richTextBoxFlags[RichTextBox.allowOleDropSection] = (value ? 1 : 0);
				this.UpdateOleCallback();
			}
		}

		// Token: 0x17000C81 RID: 3201
		// (get) Token: 0x06003289 RID: 12937 RVA: 0x000EB0BC File Offset: 0x000E92BC
		// (set) Token: 0x0600328A RID: 12938 RVA: 0x000EB0D1 File Offset: 0x000E92D1
		internal bool AllowOleObjects
		{
			get
			{
				return this.richTextBoxFlags[RichTextBox.allowOleObjectsSection] != 0;
			}
			set
			{
				this.richTextBoxFlags[RichTextBox.allowOleObjectsSection] = (value ? 1 : 0);
			}
		}

		/// <summary>This property is not relevant to this class.</summary>
		/// <returns>
		///     <see langword="true" /> if enabled; otherwise, <see langword="false" />.</returns>
		// Token: 0x17000C82 RID: 3202
		// (get) Token: 0x0600328B RID: 12939 RVA: 0x000EB0EA File Offset: 0x000E92EA
		// (set) Token: 0x0600328C RID: 12940 RVA: 0x000EB0F2 File Offset: 0x000E92F2
		[DefaultValue(false)]
		[RefreshProperties(RefreshProperties.Repaint)]
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
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

		/// <summary>Gets or sets a value indicating whether automatic word selection is enabled.</summary>
		/// <returns>
		///     <see langword="true" /> if automatic word selection is enabled; otherwise, <see langword="false" />. The default is <see langword="true" />.</returns>
		// Token: 0x17000C83 RID: 3203
		// (get) Token: 0x0600328D RID: 12941 RVA: 0x000EB0FB File Offset: 0x000E92FB
		// (set) Token: 0x0600328E RID: 12942 RVA: 0x000EB110 File Offset: 0x000E9310
		[SRCategory("CatBehavior")]
		[DefaultValue(false)]
		[SRDescription("RichTextBoxAutoWordSelection")]
		public bool AutoWordSelection
		{
			get
			{
				return this.richTextBoxFlags[RichTextBox.autoWordSelectionSection] != 0;
			}
			set
			{
				this.richTextBoxFlags[RichTextBox.autoWordSelectionSection] = (value ? 1 : 0);
				if (base.IsHandleCreated)
				{
					base.SendMessage(1101, value ? 2 : 4, 1);
				}
			}
		}

		/// <summary>This property is not relevant to this class.</summary>
		/// <returns>The background image displayed in the control.</returns>
		// Token: 0x17000C84 RID: 3204
		// (get) Token: 0x0600328F RID: 12943 RVA: 0x000EB145 File Offset: 0x000E9345
		// (set) Token: 0x06003290 RID: 12944 RVA: 0x000EB14D File Offset: 0x000E934D
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public override Image BackgroundImage
		{
			get
			{
				return base.BackgroundImage;
			}
			set
			{
				base.BackgroundImage = value;
			}
		}

		/// <summary>Occurs when the value of the <see cref="P:System.Windows.Forms.RichTextBox.BackgroundImage" /> property changes.</summary>
		// Token: 0x1400026C RID: 620
		// (add) Token: 0x06003291 RID: 12945 RVA: 0x000EB156 File Offset: 0x000E9356
		// (remove) Token: 0x06003292 RID: 12946 RVA: 0x000EB15F File Offset: 0x000E935F
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event EventHandler BackgroundImageChanged
		{
			add
			{
				base.BackgroundImageChanged += value;
			}
			remove
			{
				base.BackgroundImageChanged -= value;
			}
		}

		/// <summary>This property is not relevant to this class.</summary>
		/// <returns>The layout of the background image displayed in the control.</returns>
		// Token: 0x17000C85 RID: 3205
		// (get) Token: 0x06003293 RID: 12947 RVA: 0x000EB168 File Offset: 0x000E9368
		// (set) Token: 0x06003294 RID: 12948 RVA: 0x000EB170 File Offset: 0x000E9370
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public override ImageLayout BackgroundImageLayout
		{
			get
			{
				return base.BackgroundImageLayout;
			}
			set
			{
				base.BackgroundImageLayout = value;
			}
		}

		/// <summary>Occurs when the value of the <see cref="P:System.Windows.Forms.RichTextBox.BackgroundImageLayout" /> property changes.</summary>
		// Token: 0x1400026D RID: 621
		// (add) Token: 0x06003295 RID: 12949 RVA: 0x000EB179 File Offset: 0x000E9379
		// (remove) Token: 0x06003296 RID: 12950 RVA: 0x000EB182 File Offset: 0x000E9382
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event EventHandler BackgroundImageLayoutChanged
		{
			add
			{
				base.BackgroundImageLayoutChanged += value;
			}
			remove
			{
				base.BackgroundImageLayoutChanged -= value;
			}
		}

		/// <summary>Gets or sets the indentation used in the <see cref="T:System.Windows.Forms.RichTextBox" /> control when the bullet style is applied to the text.</summary>
		/// <returns>The number of pixels inserted as the indentation after a bullet. The default is zero.</returns>
		/// <exception cref="T:System.ArgumentException">The specified indentation was less than zero. </exception>
		// Token: 0x17000C86 RID: 3206
		// (get) Token: 0x06003297 RID: 12951 RVA: 0x000EB18B File Offset: 0x000E938B
		// (set) Token: 0x06003298 RID: 12952 RVA: 0x000EB194 File Offset: 0x000E9394
		[SRCategory("CatBehavior")]
		[DefaultValue(0)]
		[Localizable(true)]
		[SRDescription("RichTextBoxBulletIndent")]
		public int BulletIndent
		{
			get
			{
				return this.bulletIndent;
			}
			set
			{
				if (value < 0)
				{
					throw new ArgumentOutOfRangeException("BulletIndent", SR.GetString("InvalidArgument", new object[]
					{
						"BulletIndent",
						value.ToString(CultureInfo.CurrentCulture)
					}));
				}
				this.bulletIndent = value;
				if (base.IsHandleCreated && this.SelectionBullet)
				{
					this.SelectionBullet = true;
				}
			}
		}

		// Token: 0x17000C87 RID: 3207
		// (get) Token: 0x06003299 RID: 12953 RVA: 0x000EB1F5 File Offset: 0x000E93F5
		// (set) Token: 0x0600329A RID: 12954 RVA: 0x000EB20A File Offset: 0x000E940A
		private bool CallOnContentsResized
		{
			get
			{
				return this.richTextBoxFlags[RichTextBox.callOnContentsResizedSection] != 0;
			}
			set
			{
				this.richTextBoxFlags[RichTextBox.callOnContentsResizedSection] = (value ? 1 : 0);
			}
		}

		// Token: 0x17000C88 RID: 3208
		// (get) Token: 0x0600329B RID: 12955 RVA: 0x000EB223 File Offset: 0x000E9423
		internal override bool CanRaiseTextChangedEvent
		{
			get
			{
				return !this.SuppressTextChangedEvent;
			}
		}

		/// <summary>Gets a value indicating whether there are actions that have occurred within the <see cref="T:System.Windows.Forms.RichTextBox" /> that can be reapplied.</summary>
		/// <returns>
		///     <see langword="true" /> if there are operations that have been undone that can be reapplied to the content of the control; otherwise, <see langword="false" />.</returns>
		// Token: 0x17000C89 RID: 3209
		// (get) Token: 0x0600329C RID: 12956 RVA: 0x000EB230 File Offset: 0x000E9430
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[SRDescription("RichTextBoxCanRedoDescr")]
		public bool CanRedo
		{
			get
			{
				return base.IsHandleCreated && (int)((long)base.SendMessage(1109, 0, 0)) != 0;
			}
		}

		/// <summary>Gets the required creation parameters when the control handle is created.</summary>
		/// <returns>A <see cref="T:System.Windows.Forms.CreateParams" /> that contains the required creation parameters when the handle to the control is created.</returns>
		// Token: 0x17000C8A RID: 3210
		// (get) Token: 0x0600329D RID: 12957 RVA: 0x000EB260 File Offset: 0x000E9460
		protected override CreateParams CreateParams
		{
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			get
			{
				if (RichTextBox.moduleHandle == IntPtr.Zero)
				{
					string text = LocalAppContextSwitches.DoNotLoadLatestRichEditControl ? "RichEd20.DLL" : "MsftEdit.DLL";
					RichTextBox.moduleHandle = UnsafeNativeMethods.LoadLibraryFromSystemPathIfAvailable(text);
					int lastWin32Error = Marshal.GetLastWin32Error();
					if ((long)RichTextBox.moduleHandle < 32L)
					{
						throw new Win32Exception(lastWin32Error, SR.GetString("LoadDLLError", new object[]
						{
							text
						}));
					}
					StringBuilder moduleFileNameLongPath = UnsafeNativeMethods.GetModuleFileNameLongPath(new HandleRef(null, RichTextBox.moduleHandle));
					string text2 = moduleFileNameLongPath.ToString();
					new FileIOPermission(FileIOPermissionAccess.Read, text2).Assert();
					FileVersionInfo versionInfo;
					try
					{
						versionInfo = FileVersionInfo.GetVersionInfo(text2);
					}
					finally
					{
						CodeAccessPermission.RevertAssert();
					}
					int num;
					if (versionInfo != null && !string.IsNullOrEmpty(versionInfo.ProductVersion) && int.TryParse(versionInfo.ProductVersion[0].ToString(), out num))
					{
						RichTextBox.richEditMajorVersion = num;
					}
				}
				CreateParams createParams = base.CreateParams;
				if (Marshal.SystemDefaultCharSize == 1)
				{
					createParams.ClassName = (LocalAppContextSwitches.DoNotLoadLatestRichEditControl ? "RichEdit20A" : "RICHEDIT50A");
				}
				else
				{
					createParams.ClassName = (LocalAppContextSwitches.DoNotLoadLatestRichEditControl ? "RichEdit20W" : "RICHEDIT50W");
				}
				if (this.Multiline)
				{
					if ((this.ScrollBars & RichTextBoxScrollBars.Horizontal) != RichTextBoxScrollBars.None && !base.WordWrap)
					{
						createParams.Style |= 1048576;
						if ((this.ScrollBars & (RichTextBoxScrollBars)16) != RichTextBoxScrollBars.None)
						{
							createParams.Style |= 8192;
						}
					}
					if ((this.ScrollBars & RichTextBoxScrollBars.Vertical) != RichTextBoxScrollBars.None)
					{
						createParams.Style |= 2097152;
						if ((this.ScrollBars & (RichTextBoxScrollBars)16) != RichTextBoxScrollBars.None)
						{
							createParams.Style |= 8192;
						}
					}
				}
				if (BorderStyle.FixedSingle == base.BorderStyle && (createParams.Style & 8388608) != 0)
				{
					createParams.Style &= -8388609;
					createParams.ExStyle |= 512;
				}
				return createParams;
			}
		}

		/// <summary>Gets or sets a value indicating whether or not the <see cref="T:System.Windows.Forms.RichTextBox" /> will automatically format a Uniform Resource Locator (URL) when it is typed into the control.</summary>
		/// <returns>
		///     <see langword="true" /> if the <see cref="T:System.Windows.Forms.RichTextBox" /> will automatically format URLs that are typed into the control as a link; otherwise, <see langword="false" />. The default is <see langword="true" />.</returns>
		// Token: 0x17000C8B RID: 3211
		// (get) Token: 0x0600329E RID: 12958 RVA: 0x000EB450 File Offset: 0x000E9650
		// (set) Token: 0x0600329F RID: 12959 RVA: 0x000EB468 File Offset: 0x000E9668
		[SRCategory("CatBehavior")]
		[DefaultValue(true)]
		[SRDescription("RichTextBoxDetectURLs")]
		public bool DetectUrls
		{
			get
			{
				return this.richTextBoxFlags[RichTextBox.autoUrlDetectSection] != 0;
			}
			set
			{
				if (value != this.DetectUrls)
				{
					this.richTextBoxFlags[RichTextBox.autoUrlDetectSection] = (value ? 1 : 0);
					if (base.IsHandleCreated)
					{
						base.SendMessage(1115, value ? 1 : 0, 0);
						base.RecreateHandle();
					}
				}
			}
		}

		/// <summary>Gets the default size of the control. </summary>
		/// <returns>A <see cref="T:System.Drawing.Size" /> value.</returns>
		// Token: 0x17000C8C RID: 3212
		// (get) Token: 0x060032A0 RID: 12960 RVA: 0x000EB4B7 File Offset: 0x000E96B7
		protected override Size DefaultSize
		{
			get
			{
				return new Size(100, 96);
			}
		}

		/// <summary>Gets or sets a value that enables drag-and-drop operations on text, pictures, and other data.</summary>
		/// <returns>
		///     <see langword="true" /> to enable drag-and-drop operations; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
		// Token: 0x17000C8D RID: 3213
		// (get) Token: 0x060032A1 RID: 12961 RVA: 0x000EB4C2 File Offset: 0x000E96C2
		// (set) Token: 0x060032A2 RID: 12962 RVA: 0x000EB4D8 File Offset: 0x000E96D8
		[SRCategory("CatBehavior")]
		[DefaultValue(false)]
		[SRDescription("RichTextBoxEnableAutoDragDrop")]
		public bool EnableAutoDragDrop
		{
			get
			{
				return this.richTextBoxFlags[RichTextBox.enableAutoDragDropSection] != 0;
			}
			set
			{
				if (value)
				{
					try
					{
						IntSecurity.ClipboardRead.Demand();
					}
					catch (Exception innerException)
					{
						throw new InvalidOperationException(SR.GetString("DragDropRegFailed"), innerException);
					}
				}
				this.richTextBoxFlags[RichTextBox.enableAutoDragDropSection] = (value ? 1 : 0);
				this.UpdateOleCallback();
			}
		}

		/// <summary>Gets or sets the foreground color of the control.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> that represents the control's foreground color.</returns>
		// Token: 0x17000C8E RID: 3214
		// (get) Token: 0x060032A3 RID: 12963 RVA: 0x000EB534 File Offset: 0x000E9734
		// (set) Token: 0x060032A4 RID: 12964 RVA: 0x000EB53C File Offset: 0x000E973C
		public override Color ForeColor
		{
			get
			{
				return base.ForeColor;
			}
			set
			{
				if (base.IsHandleCreated)
				{
					if (this.InternalSetForeColor(value))
					{
						base.ForeColor = value;
						return;
					}
				}
				else
				{
					base.ForeColor = value;
				}
			}
		}

		/// <summary>Gets or sets the font of the text displayed by the control.</summary>
		/// <returns>The <see cref="T:System.Drawing.Font" /> to apply to the text displayed by the control. The default is the value of the <see cref="P:System.Windows.Forms.Control.DefaultFont" /> property.</returns>
		// Token: 0x17000C8F RID: 3215
		// (get) Token: 0x060032A5 RID: 12965 RVA: 0x00012071 File Offset: 0x00010271
		// (set) Token: 0x060032A6 RID: 12966 RVA: 0x000EB560 File Offset: 0x000E9760
		public override Font Font
		{
			get
			{
				return base.Font;
			}
			set
			{
				if (base.IsHandleCreated)
				{
					if (SafeNativeMethods.GetWindowTextLength(new HandleRef(this, base.Handle)) > 0)
					{
						if (value == null)
						{
							base.Font = null;
							this.SetCharFormatFont(false, this.Font);
							return;
						}
						try
						{
							Font charFormatFont = this.GetCharFormatFont(false);
							if (charFormatFont == null || !charFormatFont.Equals(value))
							{
								this.SetCharFormatFont(false, value);
								this.CallOnContentsResized = true;
								base.Font = this.GetCharFormatFont(false);
							}
							return;
						}
						finally
						{
							this.CallOnContentsResized = false;
						}
					}
					base.Font = value;
					return;
				}
				base.Font = value;
			}
		}

		// Token: 0x060032A7 RID: 12967 RVA: 0x000EB5FC File Offset: 0x000E97FC
		internal override Size GetPreferredSizeCore(Size proposedConstraints)
		{
			Size empty = Size.Empty;
			if (!base.WordWrap && this.Multiline && (this.ScrollBars & RichTextBoxScrollBars.Horizontal) != RichTextBoxScrollBars.None)
			{
				empty.Height += SystemInformation.HorizontalScrollBarHeight;
			}
			if (this.Multiline && (this.ScrollBars & RichTextBoxScrollBars.Vertical) != RichTextBoxScrollBars.None)
			{
				empty.Width += SystemInformation.VerticalScrollBarWidth;
			}
			proposedConstraints -= empty;
			Size preferredSizeCore = base.GetPreferredSizeCore(proposedConstraints);
			return preferredSizeCore + empty;
		}

		// Token: 0x17000C90 RID: 3216
		// (get) Token: 0x060032A8 RID: 12968 RVA: 0x000EB679 File Offset: 0x000E9879
		// (set) Token: 0x060032A9 RID: 12969 RVA: 0x000EB68E File Offset: 0x000E988E
		private bool InConstructor
		{
			get
			{
				return this.richTextBoxFlags[RichTextBox.fInCtorSection] != 0;
			}
			set
			{
				this.richTextBoxFlags[RichTextBox.fInCtorSection] = (value ? 1 : 0);
			}
		}

		/// <summary>Gets or sets a value that indicates <see cref="T:System.Windows.Forms.RichTextBox" /> settings for Input Method Editor (IME) and Asian language support.</summary>
		/// <returns>One of the <see cref="T:System.Windows.Forms.RichTextBoxLanguageOptions" /> values. The default is <see cref="F:System.Windows.Forms.RichTextBoxLanguageOptions.AutoFontSizeAdjust" />.</returns>
		// Token: 0x17000C91 RID: 3217
		// (get) Token: 0x060032AA RID: 12970 RVA: 0x000EB6A8 File Offset: 0x000E98A8
		// (set) Token: 0x060032AB RID: 12971 RVA: 0x000EB6E5 File Offset: 0x000E98E5
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public RichTextBoxLanguageOptions LanguageOption
		{
			get
			{
				RichTextBoxLanguageOptions result;
				if (base.IsHandleCreated)
				{
					result = (RichTextBoxLanguageOptions)((int)UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), 1145, 0, 0));
				}
				else
				{
					result = this.languageOption;
				}
				return result;
			}
			set
			{
				if (this.LanguageOption != value)
				{
					this.languageOption = value;
					if (base.IsHandleCreated)
					{
						UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), 1144, 0, (int)value);
					}
				}
			}
		}

		// Token: 0x17000C92 RID: 3218
		// (get) Token: 0x060032AC RID: 12972 RVA: 0x000EB718 File Offset: 0x000E9918
		// (set) Token: 0x060032AD RID: 12973 RVA: 0x000EB72D File Offset: 0x000E992D
		private bool LinkCursor
		{
			get
			{
				return this.richTextBoxFlags[RichTextBox.linkcursorSection] != 0;
			}
			set
			{
				this.richTextBoxFlags[RichTextBox.linkcursorSection] = (value ? 1 : 0);
			}
		}

		/// <summary>Gets or sets the maximum number of characters the user can type or paste into the rich text box control.</summary>
		/// <returns>The number of characters that can be entered into the control. The default is <see cref="F:System.Int32.MaxValue" />.</returns>
		/// <exception cref="T:System.ArgumentException">The value assigned to the property is less than 0. </exception>
		// Token: 0x17000C93 RID: 3219
		// (get) Token: 0x060032AE RID: 12974 RVA: 0x000CB9E7 File Offset: 0x000C9BE7
		// (set) Token: 0x060032AF RID: 12975 RVA: 0x000EB746 File Offset: 0x000E9946
		[DefaultValue(2147483647)]
		public override int MaxLength
		{
			get
			{
				return base.MaxLength;
			}
			set
			{
				base.MaxLength = value;
			}
		}

		/// <summary>Gets or sets a value indicating whether this is a multiline <see cref="T:System.Windows.Forms.RichTextBox" /> control.</summary>
		/// <returns>
		///     <see langword="true" /> if the control is a multiline <see cref="T:System.Windows.Forms.RichTextBox" /> control; otherwise, <see langword="false" />. The default is <see langword="true" />.</returns>
		// Token: 0x17000C94 RID: 3220
		// (get) Token: 0x060032B0 RID: 12976 RVA: 0x000EB74F File Offset: 0x000E994F
		// (set) Token: 0x060032B1 RID: 12977 RVA: 0x000EB757 File Offset: 0x000E9957
		[DefaultValue(true)]
		public override bool Multiline
		{
			get
			{
				return base.Multiline;
			}
			set
			{
				base.Multiline = value;
			}
		}

		// Token: 0x17000C95 RID: 3221
		// (get) Token: 0x060032B2 RID: 12978 RVA: 0x000EB760 File Offset: 0x000E9960
		// (set) Token: 0x060032B3 RID: 12979 RVA: 0x000EB775 File Offset: 0x000E9975
		private bool ProtectedError
		{
			get
			{
				return this.richTextBoxFlags[RichTextBox.protectedErrorSection] != 0;
			}
			set
			{
				this.richTextBoxFlags[RichTextBox.protectedErrorSection] = (value ? 1 : 0);
			}
		}

		/// <summary>Gets the name of the action that can be reapplied to the control when the <see cref="M:System.Windows.Forms.RichTextBox.Redo" /> method is called.</summary>
		/// <returns>A string that represents the name of the action that will be performed when a call to the <see cref="M:System.Windows.Forms.RichTextBox.Redo" /> method is made.</returns>
		// Token: 0x17000C96 RID: 3222
		// (get) Token: 0x060032B4 RID: 12980 RVA: 0x000EB790 File Offset: 0x000E9990
		[SRCategory("CatBehavior")]
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[SRDescription("RichTextBoxRedoActionNameDescr")]
		public string RedoActionName
		{
			get
			{
				if (!this.CanRedo)
				{
					return "";
				}
				int actionID = (int)((long)base.SendMessage(1111, 0, 0));
				return this.GetEditorActionName(actionID);
			}
		}

		/// <summary>This property is not relevant for this class.</summary>
		/// <returns>
		///     <see langword="true" /> if shortcut keys are enabled; otherwise, <see langword="false" />. The default is <see langword="true" />.</returns>
		// Token: 0x17000C97 RID: 3223
		// (get) Token: 0x060032B5 RID: 12981 RVA: 0x000EB7C6 File Offset: 0x000E99C6
		// (set) Token: 0x060032B6 RID: 12982 RVA: 0x000EB7DB File Offset: 0x000E99DB
		[DefaultValue(true)]
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool RichTextShortcutsEnabled
		{
			get
			{
				return this.richTextBoxFlags[RichTextBox.richTextShortcutsEnabledSection] != 0;
			}
			set
			{
				if (RichTextBox.shortcutsToDisable == null)
				{
					RichTextBox.shortcutsToDisable = new int[]
					{
						131148,
						131154,
						131141,
						131146
					};
				}
				this.richTextBoxFlags[RichTextBox.richTextShortcutsEnabledSection] = (value ? 1 : 0);
			}
		}

		/// <summary>Gets or sets the size of a single line of text within the <see cref="T:System.Windows.Forms.RichTextBox" /> control.</summary>
		/// <returns>The size, in pixels, of a single line of text in the control. The default is zero.</returns>
		/// <exception cref="T:System.ArgumentException">The specified value was less than zero. </exception>
		// Token: 0x17000C98 RID: 3224
		// (get) Token: 0x060032B7 RID: 12983 RVA: 0x000EB811 File Offset: 0x000E9A11
		// (set) Token: 0x060032B8 RID: 12984 RVA: 0x000EB81C File Offset: 0x000E9A1C
		[SRCategory("CatBehavior")]
		[DefaultValue(0)]
		[Localizable(true)]
		[SRDescription("RichTextBoxRightMargin")]
		public int RightMargin
		{
			get
			{
				return this.rightMargin;
			}
			set
			{
				if (this.rightMargin != value)
				{
					if (value < 0)
					{
						throw new ArgumentOutOfRangeException("RightMargin", SR.GetString("InvalidLowBoundArgumentEx", new object[]
						{
							"RightMargin",
							value.ToString(CultureInfo.CurrentCulture),
							0.ToString(CultureInfo.CurrentCulture)
						}));
					}
					this.rightMargin = value;
					if (value == 0)
					{
						base.RecreateHandle();
						return;
					}
					if (base.IsHandleCreated)
					{
						IntPtr intPtr = UnsafeNativeMethods.CreateIC("DISPLAY", null, null, new HandleRef(null, IntPtr.Zero));
						try
						{
							base.SendMessage(1096, intPtr, (IntPtr)RichTextBox.Pixel2Twip(intPtr, value, true));
						}
						finally
						{
							if (intPtr != IntPtr.Zero)
							{
								UnsafeNativeMethods.DeleteDC(new HandleRef(null, intPtr));
							}
						}
					}
				}
			}
		}

		/// <summary>Gets or sets the text of the <see cref="T:System.Windows.Forms.RichTextBox" /> control, including all rich text format (RTF) codes.</summary>
		/// <returns>The text of the control in RTF format.</returns>
		// Token: 0x17000C99 RID: 3225
		// (get) Token: 0x060032B9 RID: 12985 RVA: 0x000EB8F4 File Offset: 0x000E9AF4
		// (set) Token: 0x060032BA RID: 12986 RVA: 0x000EB924 File Offset: 0x000E9B24
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[SRDescription("RichTextBoxRTF")]
		[RefreshProperties(RefreshProperties.All)]
		public string Rtf
		{
			get
			{
				if (base.IsHandleCreated)
				{
					return this.StreamOut(2);
				}
				if (this.textPlain != null)
				{
					this.ForceHandleCreate();
					return this.StreamOut(2);
				}
				return this.textRtf;
			}
			set
			{
				if (value == null)
				{
					value = "";
				}
				if (value.Equals(this.Rtf))
				{
					return;
				}
				this.ForceHandleCreate();
				this.textRtf = value;
				this.StreamIn(value, 2);
				if (this.CanRaiseTextChangedEvent)
				{
					this.OnTextChanged(EventArgs.Empty);
				}
			}
		}

		/// <summary>Gets or sets the type of scroll bars to display in the <see cref="T:System.Windows.Forms.RichTextBox" /> control.</summary>
		/// <returns>One of the <see cref="T:System.Windows.Forms.RichTextBoxScrollBars" /> values. The default is <see langword="RichTextBoxScrollBars.Both" />.</returns>
		/// <exception cref="T:System.ComponentModel.InvalidEnumArgumentException">The specified value is not defined in the <see cref="T:System.Windows.Forms.RichTextBoxScrollBars" /> enumeration. </exception>
		// Token: 0x17000C9A RID: 3226
		// (get) Token: 0x060032BB RID: 12987 RVA: 0x000EB972 File Offset: 0x000E9B72
		// (set) Token: 0x060032BC RID: 12988 RVA: 0x000EB984 File Offset: 0x000E9B84
		[SRCategory("CatAppearance")]
		[DefaultValue(RichTextBoxScrollBars.Both)]
		[Localizable(true)]
		[SRDescription("RichTextBoxScrollBars")]
		public RichTextBoxScrollBars ScrollBars
		{
			get
			{
				return (RichTextBoxScrollBars)this.richTextBoxFlags[RichTextBox.scrollBarsSection];
			}
			set
			{
				if (!ClientUtils.IsEnumValid_NotSequential(value, (int)value, new int[]
				{
					3,
					0,
					1,
					2,
					17,
					18,
					19
				}))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(RichTextBoxScrollBars));
				}
				if (value != this.ScrollBars)
				{
					using (LayoutTransaction.CreateTransactionIf(this.AutoSize, this.ParentInternal, this, PropertyNames.ScrollBars))
					{
						this.richTextBoxFlags[RichTextBox.scrollBarsSection] = (int)value;
						base.RecreateHandle();
					}
				}
			}
		}

		/// <summary>Gets or sets the alignment to apply to the current selection or insertion point.</summary>
		/// <returns>One of the <see cref="T:System.Windows.Forms.HorizontalAlignment" /> values.</returns>
		/// <exception cref="T:System.ComponentModel.InvalidEnumArgumentException">The specified value is not one of the values defined in the <see cref="T:System.Windows.Forms.HorizontalAlignment" /> class. </exception>
		// Token: 0x17000C9B RID: 3227
		// (get) Token: 0x060032BD RID: 12989 RVA: 0x000EBA1C File Offset: 0x000E9C1C
		// (set) Token: 0x060032BE RID: 12990 RVA: 0x000EBA90 File Offset: 0x000E9C90
		[DefaultValue(HorizontalAlignment.Left)]
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[SRDescription("RichTextBoxSelAlignment")]
		public HorizontalAlignment SelectionAlignment
		{
			get
			{
				HorizontalAlignment result = HorizontalAlignment.Left;
				this.ForceHandleCreate();
				NativeMethods.PARAFORMAT paraformat = new NativeMethods.PARAFORMAT();
				paraformat.rgxTabs = new int[32];
				UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), 1085, 0, paraformat);
				if ((8 & paraformat.dwMask) != 0)
				{
					switch (paraformat.wAlignment)
					{
					case 1:
						result = HorizontalAlignment.Left;
						break;
					case 2:
						result = HorizontalAlignment.Right;
						break;
					case 3:
						result = HorizontalAlignment.Center;
						break;
					}
				}
				return result;
			}
			set
			{
				if (!ClientUtils.IsEnumValid(value, (int)value, 0, 2))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(HorizontalAlignment));
				}
				this.ForceHandleCreate();
				NativeMethods.PARAFORMAT paraformat = new NativeMethods.PARAFORMAT();
				paraformat.dwMask = 8;
				switch (value)
				{
				case HorizontalAlignment.Left:
					paraformat.wAlignment = 1;
					break;
				case HorizontalAlignment.Right:
					paraformat.wAlignment = 2;
					break;
				case HorizontalAlignment.Center:
					paraformat.wAlignment = 3;
					break;
				}
				UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), 1095, 0, paraformat);
			}
		}

		/// <summary>Gets or sets a value indicating whether the bullet style is applied to the current selection or insertion point.</summary>
		/// <returns>
		///     <see langword="true" /> if the current selection or insertion point has the bullet style applied; otherwise, <see langword="false" />.</returns>
		// Token: 0x17000C9C RID: 3228
		// (get) Token: 0x060032BF RID: 12991 RVA: 0x000EBB1C File Offset: 0x000E9D1C
		// (set) Token: 0x060032C0 RID: 12992 RVA: 0x000EBB7C File Offset: 0x000E9D7C
		[DefaultValue(false)]
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[SRDescription("RichTextBoxSelBullet")]
		public bool SelectionBullet
		{
			get
			{
				RichTextBoxSelectionAttribute richTextBoxSelectionAttribute = RichTextBoxSelectionAttribute.None;
				this.ForceHandleCreate();
				NativeMethods.PARAFORMAT paraformat = new NativeMethods.PARAFORMAT();
				paraformat.rgxTabs = new int[32];
				UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), 1085, 0, paraformat);
				if ((32 & paraformat.dwMask) != 0)
				{
					if (1 == paraformat.wNumbering)
					{
						richTextBoxSelectionAttribute = RichTextBoxSelectionAttribute.All;
					}
					return richTextBoxSelectionAttribute == RichTextBoxSelectionAttribute.All;
				}
				return false;
			}
			set
			{
				this.ForceHandleCreate();
				NativeMethods.PARAFORMAT paraformat = new NativeMethods.PARAFORMAT();
				paraformat.dwMask = 36;
				if (!value)
				{
					paraformat.wNumbering = 0;
					paraformat.dxOffset = 0;
				}
				else
				{
					paraformat.wNumbering = 1;
					paraformat.dxOffset = RichTextBox.Pixel2Twip(IntPtr.Zero, this.bulletIndent, true);
				}
				UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), 1095, 0, paraformat);
			}
		}

		/// <summary>Gets or sets whether text in the control appears on the baseline, as a superscript, or as a subscript below the baseline.</summary>
		/// <returns>A number that specifies the character offset.</returns>
		/// <exception cref="T:System.ArgumentException">The specified value was less than -2000 or greater than 2000. </exception>
		// Token: 0x17000C9D RID: 3229
		// (get) Token: 0x060032C1 RID: 12993 RVA: 0x000EBBE8 File Offset: 0x000E9DE8
		// (set) Token: 0x060032C2 RID: 12994 RVA: 0x000EBC30 File Offset: 0x000E9E30
		[DefaultValue(0)]
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[SRDescription("RichTextBoxSelCharOffset")]
		public int SelectionCharOffset
		{
			get
			{
				this.ForceHandleCreate();
				NativeMethods.CHARFORMATA charFormat = this.GetCharFormat(true);
				int yOffset;
				if ((charFormat.dwMask & 268435456) != 0)
				{
					yOffset = charFormat.yOffset;
				}
				else
				{
					yOffset = charFormat.yOffset;
				}
				return RichTextBox.Twip2Pixel(IntPtr.Zero, yOffset, false);
			}
			set
			{
				if (value > 2000 || value < -2000)
				{
					throw new ArgumentOutOfRangeException("SelectionCharOffset", SR.GetString("InvalidBoundArgument", new object[]
					{
						"SelectionCharOffset",
						value,
						-2000,
						2000
					}));
				}
				this.ForceHandleCreate();
				NativeMethods.CHARFORMATA charformata = new NativeMethods.CHARFORMATA();
				charformata.dwMask = 268435456;
				charformata.yOffset = RichTextBox.Pixel2Twip(IntPtr.Zero, value, false);
				UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), 1092, 1, charformata);
			}
		}

		/// <summary>Gets or sets the text color of the current text selection or insertion point.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> that represents the color to apply to the current text selection or to text entered after the insertion point.</returns>
		// Token: 0x17000C9E RID: 3230
		// (get) Token: 0x060032C3 RID: 12995 RVA: 0x000EBCD8 File Offset: 0x000E9ED8
		// (set) Token: 0x060032C4 RID: 12996 RVA: 0x000EBD14 File Offset: 0x000E9F14
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[SRDescription("RichTextBoxSelColor")]
		public Color SelectionColor
		{
			get
			{
				Color result = Color.Empty;
				this.ForceHandleCreate();
				NativeMethods.CHARFORMATA charFormat = this.GetCharFormat(true);
				if ((charFormat.dwMask & 1073741824) != 0)
				{
					result = ColorTranslator.FromOle(charFormat.crTextColor);
				}
				return result;
			}
			set
			{
				this.ForceHandleCreate();
				NativeMethods.CHARFORMATA charFormat = this.GetCharFormat(true);
				charFormat.dwMask = 1073741824;
				charFormat.dwEffects = 0;
				charFormat.crTextColor = ColorTranslator.ToWin32(value);
				UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), 1092, 1, charFormat);
			}
		}

		/// <summary>Gets or sets the color of text when the text is selected in a <see cref="T:System.Windows.Forms.RichTextBox" /> control.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> that represents the text color when the text is selected. The default is the value of the <see cref="P:System.Windows.Forms.Control.DefaultBackColor" /> property.</returns>
		// Token: 0x17000C9F RID: 3231
		// (get) Token: 0x060032C5 RID: 12997 RVA: 0x000EBD68 File Offset: 0x000E9F68
		// (set) Token: 0x060032C6 RID: 12998 RVA: 0x000EBDC8 File Offset: 0x000E9FC8
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[SRDescription("RichTextBoxSelBackColor")]
		public Color SelectionBackColor
		{
			get
			{
				Color result = Color.Empty;
				if (base.IsHandleCreated)
				{
					NativeMethods.CHARFORMAT2A charFormat = this.GetCharFormat2(true);
					if ((charFormat.dwEffects & 67108864) != 0)
					{
						result = this.BackColor;
					}
					else if ((charFormat.dwMask & 67108864) != 0)
					{
						result = ColorTranslator.FromOle(charFormat.crBackColor);
					}
				}
				else
				{
					result = this.selectionBackColorToSetOnHandleCreated;
				}
				return result;
			}
			set
			{
				this.selectionBackColorToSetOnHandleCreated = value;
				if (base.IsHandleCreated)
				{
					NativeMethods.CHARFORMAT2A charformat2A = new NativeMethods.CHARFORMAT2A();
					if (value == Color.Empty)
					{
						charformat2A.dwEffects = 67108864;
					}
					else
					{
						charformat2A.dwMask = 67108864;
						charformat2A.crBackColor = ColorTranslator.ToWin32(value);
					}
					UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), 1092, 1, charformat2A);
				}
			}
		}

		/// <summary>Gets or sets the font of the current text selection or insertion point.</summary>
		/// <returns>A <see cref="T:System.Drawing.Font" /> that represents the font to apply to the current text selection or to text entered after the insertion point.</returns>
		// Token: 0x17000CA0 RID: 3232
		// (get) Token: 0x060032C7 RID: 12999 RVA: 0x000EBE34 File Offset: 0x000EA034
		// (set) Token: 0x060032C8 RID: 13000 RVA: 0x000EBE3D File Offset: 0x000EA03D
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[SRDescription("RichTextBoxSelFont")]
		public Font SelectionFont
		{
			get
			{
				return this.GetCharFormatFont(true);
			}
			set
			{
				this.SetCharFormatFont(true, value);
			}
		}

		/// <summary>Gets or sets the distance between the left edge of the first line of text in the selected paragraph and the left edge of subsequent lines in the same paragraph.</summary>
		/// <returns>The distance, in pixels, for the hanging indent applied to the current text selection or the insertion point.</returns>
		// Token: 0x17000CA1 RID: 3233
		// (get) Token: 0x060032C9 RID: 13001 RVA: 0x000EBE48 File Offset: 0x000EA048
		// (set) Token: 0x060032CA RID: 13002 RVA: 0x000EBEA8 File Offset: 0x000EA0A8
		[DefaultValue(0)]
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[SRDescription("RichTextBoxSelHangingIndent")]
		public int SelectionHangingIndent
		{
			get
			{
				int v = 0;
				this.ForceHandleCreate();
				NativeMethods.PARAFORMAT paraformat = new NativeMethods.PARAFORMAT();
				paraformat.rgxTabs = new int[32];
				UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), 1085, 0, paraformat);
				if ((4 & paraformat.dwMask) != 0)
				{
					v = paraformat.dxOffset;
				}
				return RichTextBox.Twip2Pixel(IntPtr.Zero, v, true);
			}
			set
			{
				this.ForceHandleCreate();
				NativeMethods.PARAFORMAT paraformat = new NativeMethods.PARAFORMAT();
				paraformat.dwMask = 4;
				paraformat.dxOffset = RichTextBox.Pixel2Twip(IntPtr.Zero, value, true);
				UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), 1095, 0, paraformat);
			}
		}

		/// <summary>Gets or sets the length, in pixels, of the indentation of the line where the selection starts.</summary>
		/// <returns>The current distance, in pixels, of the indentation applied to the left of the current text selection or the insertion point.</returns>
		// Token: 0x17000CA2 RID: 3234
		// (get) Token: 0x060032CB RID: 13003 RVA: 0x000EBEF4 File Offset: 0x000EA0F4
		// (set) Token: 0x060032CC RID: 13004 RVA: 0x000EBF54 File Offset: 0x000EA154
		[DefaultValue(0)]
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[SRDescription("RichTextBoxSelIndent")]
		public int SelectionIndent
		{
			get
			{
				int v = 0;
				this.ForceHandleCreate();
				NativeMethods.PARAFORMAT paraformat = new NativeMethods.PARAFORMAT();
				paraformat.rgxTabs = new int[32];
				UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), 1085, 0, paraformat);
				if ((1 & paraformat.dwMask) != 0)
				{
					v = paraformat.dxStartIndent;
				}
				return RichTextBox.Twip2Pixel(IntPtr.Zero, v, true);
			}
			set
			{
				this.ForceHandleCreate();
				NativeMethods.PARAFORMAT paraformat = new NativeMethods.PARAFORMAT();
				paraformat.dwMask = 1;
				paraformat.dxStartIndent = RichTextBox.Pixel2Twip(IntPtr.Zero, value, true);
				UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), 1095, 0, paraformat);
			}
		}

		/// <summary>Gets or sets the number of characters selected in control.</summary>
		/// <returns>The number of characters selected in the text box.</returns>
		// Token: 0x17000CA3 RID: 3235
		// (get) Token: 0x060032CD RID: 13005 RVA: 0x000EBF9F File Offset: 0x000EA19F
		// (set) Token: 0x060032CE RID: 13006 RVA: 0x000EBFBB File Offset: 0x000EA1BB
		[SRCategory("CatAppearance")]
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[SRDescription("TextBoxSelectionLengthDescr")]
		public override int SelectionLength
		{
			get
			{
				if (!base.IsHandleCreated)
				{
					return base.SelectionLength;
				}
				return this.SelectedText.Length;
			}
			set
			{
				base.SelectionLength = value;
			}
		}

		/// <summary>Gets or sets a value indicating whether the current text selection is protected.</summary>
		/// <returns>
		///     <see langword="true" /> if the current selection prevents any changes to its content; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
		// Token: 0x17000CA4 RID: 3236
		// (get) Token: 0x060032CF RID: 13007 RVA: 0x000EBFC4 File Offset: 0x000EA1C4
		// (set) Token: 0x060032D0 RID: 13008 RVA: 0x000EBFD9 File Offset: 0x000EA1D9
		[DefaultValue(false)]
		[SRDescription("RichTextBoxSelProtected")]
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool SelectionProtected
		{
			get
			{
				this.ForceHandleCreate();
				return this.GetCharFormat(16, 16) == RichTextBoxSelectionAttribute.All;
			}
			set
			{
				this.ForceHandleCreate();
				this.SetCharFormat(16, value ? 16 : 0, RichTextBoxSelectionAttribute.All);
			}
		}

		// Token: 0x17000CA5 RID: 3237
		// (get) Token: 0x060032D1 RID: 13009 RVA: 0x0000E0A4 File Offset: 0x0000C2A4
		internal override bool SelectionUsesDbcsOffsetsInWin9x
		{
			get
			{
				return false;
			}
		}

		/// <summary>Gets or sets the currently selected rich text format (RTF) formatted text in the control.</summary>
		/// <returns>The selected RTF text in the control.</returns>
		// Token: 0x17000CA6 RID: 3238
		// (get) Token: 0x060032D2 RID: 13010 RVA: 0x000EBFF3 File Offset: 0x000EA1F3
		// (set) Token: 0x060032D3 RID: 13011 RVA: 0x000EC006 File Offset: 0x000EA206
		[DefaultValue("")]
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[SRDescription("RichTextBoxSelRTF")]
		public string SelectedRtf
		{
			get
			{
				this.ForceHandleCreate();
				return this.StreamOut(32770);
			}
			set
			{
				this.ForceHandleCreate();
				if (value == null)
				{
					value = "";
				}
				this.StreamIn(value, 32770);
			}
		}

		/// <summary>The distance (in pixels) between the right edge of the <see cref="T:System.Windows.Forms.RichTextBox" /> control and the right edge of the text that is selected or added at the current insertion point.</summary>
		/// <returns>The indentation space, in pixels, at the right of the current selection or insertion point.</returns>
		// Token: 0x17000CA7 RID: 3239
		// (get) Token: 0x060032D4 RID: 13012 RVA: 0x000EC024 File Offset: 0x000EA224
		// (set) Token: 0x060032D5 RID: 13013 RVA: 0x000EC084 File Offset: 0x000EA284
		[DefaultValue(0)]
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[SRDescription("RichTextBoxSelRightIndent")]
		public int SelectionRightIndent
		{
			get
			{
				int v = 0;
				this.ForceHandleCreate();
				NativeMethods.PARAFORMAT paraformat = new NativeMethods.PARAFORMAT();
				paraformat.rgxTabs = new int[32];
				UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), 1085, 0, paraformat);
				if ((2 & paraformat.dwMask) != 0)
				{
					v = paraformat.dxRightIndent;
				}
				return RichTextBox.Twip2Pixel(IntPtr.Zero, v, true);
			}
			set
			{
				if (value < 0)
				{
					throw new ArgumentOutOfRangeException("SelectionRightIndent", SR.GetString("InvalidLowBoundArgumentEx", new object[]
					{
						"SelectionRightIndent",
						value,
						0
					}));
				}
				this.ForceHandleCreate();
				NativeMethods.PARAFORMAT paraformat = new NativeMethods.PARAFORMAT();
				paraformat.dwMask = 2;
				paraformat.dxRightIndent = RichTextBox.Pixel2Twip(IntPtr.Zero, value, true);
				UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), 1095, 0, paraformat);
			}
		}

		/// <summary>Gets or sets the absolute tab stop positions in a <see cref="T:System.Windows.Forms.RichTextBox" /> control.</summary>
		/// <returns>An array in which each member specifies a tab offset, in pixels.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The array has more than the maximum 32 elements. </exception>
		// Token: 0x17000CA8 RID: 3240
		// (get) Token: 0x060032D6 RID: 13014 RVA: 0x000EC108 File Offset: 0x000EA308
		// (set) Token: 0x060032D7 RID: 13015 RVA: 0x000EC190 File Offset: 0x000EA390
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[SRDescription("RichTextBoxSelTabs")]
		public int[] SelectionTabs
		{
			get
			{
				int[] array = new int[0];
				this.ForceHandleCreate();
				NativeMethods.PARAFORMAT paraformat = new NativeMethods.PARAFORMAT();
				paraformat.rgxTabs = new int[32];
				UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), 1085, 0, paraformat);
				if ((16 & paraformat.dwMask) != 0)
				{
					array = new int[(int)paraformat.cTabCount];
					for (int i = 0; i < (int)paraformat.cTabCount; i++)
					{
						array[i] = RichTextBox.Twip2Pixel(IntPtr.Zero, paraformat.rgxTabs[i], true);
					}
				}
				return array;
			}
			set
			{
				if (value != null && value.Length > 32)
				{
					throw new ArgumentOutOfRangeException("SelectionTabs", SR.GetString("SelTabCountRange"));
				}
				this.ForceHandleCreate();
				NativeMethods.PARAFORMAT paraformat = new NativeMethods.PARAFORMAT();
				paraformat.rgxTabs = new int[32];
				UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), 1085, 0, paraformat);
				paraformat.cTabCount = (short)((value == null) ? 0 : value.Length);
				paraformat.dwMask = 16;
				for (int i = 0; i < (int)paraformat.cTabCount; i++)
				{
					paraformat.rgxTabs[i] = RichTextBox.Pixel2Twip(IntPtr.Zero, value[i], true);
				}
				UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), 1095, 0, paraformat);
			}
		}

		/// <summary>Gets or sets the selected text within the <see cref="T:System.Windows.Forms.RichTextBox" />.</summary>
		/// <returns>A string that represents the selected text in the control.</returns>
		// Token: 0x17000CA9 RID: 3241
		// (get) Token: 0x060032D8 RID: 13016 RVA: 0x000EC248 File Offset: 0x000EA448
		// (set) Token: 0x060032D9 RID: 13017 RVA: 0x000EC268 File Offset: 0x000EA468
		[DefaultValue("")]
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[SRDescription("RichTextBoxSelText")]
		public override string SelectedText
		{
			get
			{
				this.ForceHandleCreate();
				return this.StreamOut(32785);
			}
			set
			{
				this.ForceHandleCreate();
				this.StreamIn(value, 32785);
			}
		}

		/// <summary>Gets the selection type within the control.</summary>
		/// <returns>A bitwise combination of the <see cref="T:System.Windows.Forms.RichTextBoxSelectionTypes" /> values.</returns>
		// Token: 0x17000CAA RID: 3242
		// (get) Token: 0x060032DA RID: 13018 RVA: 0x000EC27C File Offset: 0x000EA47C
		[SRCategory("CatBehavior")]
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[SRDescription("RichTextBoxSelTypeDescr")]
		public RichTextBoxSelectionTypes SelectionType
		{
			get
			{
				this.ForceHandleCreate();
				if (this.SelectionLength > 0)
				{
					return (RichTextBoxSelectionTypes)((long)base.SendMessage(1090, 0, 0));
				}
				return RichTextBoxSelectionTypes.Empty;
			}
		}

		/// <summary>Gets or sets a value indicating whether a selection margin is displayed in the <see cref="T:System.Windows.Forms.RichTextBox" />.</summary>
		/// <returns>
		///     <see langword="true" /> if a selection margin is enabled in the control; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
		// Token: 0x17000CAB RID: 3243
		// (get) Token: 0x060032DB RID: 13019 RVA: 0x000EC2AF File Offset: 0x000EA4AF
		// (set) Token: 0x060032DC RID: 13020 RVA: 0x000EC2C4 File Offset: 0x000EA4C4
		[SRCategory("CatBehavior")]
		[DefaultValue(false)]
		[SRDescription("RichTextBoxSelMargin")]
		public bool ShowSelectionMargin
		{
			get
			{
				return this.richTextBoxFlags[RichTextBox.showSelBarSection] != 0;
			}
			set
			{
				if (value != this.ShowSelectionMargin)
				{
					this.richTextBoxFlags[RichTextBox.showSelBarSection] = (value ? 1 : 0);
					if (base.IsHandleCreated)
					{
						base.SendMessage(1101, value ? 2 : 4, 16777216);
					}
				}
			}
		}

		/// <summary>Gets or sets the current text in the rich text box.</summary>
		/// <returns>The text displayed in the control.</returns>
		// Token: 0x17000CAC RID: 3244
		// (get) Token: 0x060032DD RID: 13021 RVA: 0x000EC314 File Offset: 0x000EA514
		// (set) Token: 0x060032DE RID: 13022 RVA: 0x000EC37C File Offset: 0x000EA57C
		[Localizable(true)]
		[RefreshProperties(RefreshProperties.All)]
		public override string Text
		{
			get
			{
				if (base.IsDisposed)
				{
					return base.Text;
				}
				if (base.RecreatingHandle || base.GetAnyDisposingInHierarchy())
				{
					return "";
				}
				if (base.IsHandleCreated || this.textRtf != null)
				{
					this.ForceHandleCreate();
					return this.StreamOut(17);
				}
				if (this.textPlain != null)
				{
					return this.textPlain;
				}
				return base.Text;
			}
			set
			{
				using (LayoutTransaction.CreateTransactionIf(this.AutoSize, this.ParentInternal, this, PropertyNames.Text))
				{
					this.textRtf = null;
					if (!base.IsHandleCreated)
					{
						this.textPlain = value;
					}
					else
					{
						this.textPlain = null;
						if (value == null)
						{
							value = "";
						}
						this.StreamIn(value, 17);
						base.SendMessage(185, 0, 0);
					}
				}
			}
		}

		// Token: 0x17000CAD RID: 3245
		// (get) Token: 0x060032DF RID: 13023 RVA: 0x000EC400 File Offset: 0x000EA600
		// (set) Token: 0x060032E0 RID: 13024 RVA: 0x000EC418 File Offset: 0x000EA618
		private bool SuppressTextChangedEvent
		{
			get
			{
				return this.richTextBoxFlags[RichTextBox.suppressTextChangedEventSection] != 0;
			}
			set
			{
				bool suppressTextChangedEvent = this.SuppressTextChangedEvent;
				if (value != suppressTextChangedEvent)
				{
					this.richTextBoxFlags[RichTextBox.suppressTextChangedEventSection] = (value ? 1 : 0);
					CommonProperties.xClearPreferredSizeCache(this);
				}
			}
		}

		/// <summary>Gets the length of text in the control.</summary>
		/// <returns>The number of characters contained in the text of the control.</returns>
		// Token: 0x17000CAE RID: 3246
		// (get) Token: 0x060032E1 RID: 13025 RVA: 0x000EC450 File Offset: 0x000EA650
		[Browsable(false)]
		public override int TextLength
		{
			get
			{
				NativeMethods.GETTEXTLENGTHEX gettextlengthex = new NativeMethods.GETTEXTLENGTHEX();
				gettextlengthex.flags = 8U;
				if (Marshal.SystemDefaultCharSize == 1)
				{
					gettextlengthex.codepage = 0U;
				}
				else
				{
					gettextlengthex.codepage = 1200U;
				}
				return (int)((long)UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), 1119, gettextlengthex, 0));
			}
		}

		/// <summary>Gets the name of the action that can be undone in the control when the <see cref="M:System.Windows.Forms.TextBoxBase.Undo" /> method is called.</summary>
		/// <returns>The text name of the action that can be undone.</returns>
		// Token: 0x17000CAF RID: 3247
		// (get) Token: 0x060032E2 RID: 13026 RVA: 0x000EC4A4 File Offset: 0x000EA6A4
		[SRCategory("CatBehavior")]
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[SRDescription("RichTextBoxUndoActionNameDescr")]
		public string UndoActionName
		{
			get
			{
				if (!base.CanUndo)
				{
					return "";
				}
				int actionID = (int)((long)base.SendMessage(1110, 0, 0));
				return this.GetEditorActionName(actionID);
			}
		}

		// Token: 0x060032E3 RID: 13027 RVA: 0x000EC4DC File Offset: 0x000EA6DC
		private string GetEditorActionName(int actionID)
		{
			switch (actionID)
			{
			default:
				return SR.GetString("RichTextBox_IDUnknown");
			case 1:
				return SR.GetString("RichTextBox_IDTyping");
			case 2:
				return SR.GetString("RichTextBox_IDDelete");
			case 3:
				return SR.GetString("RichTextBox_IDDragDrop");
			case 4:
				return SR.GetString("RichTextBox_IDCut");
			case 5:
				return SR.GetString("RichTextBox_IDPaste");
			}
		}

		/// <summary>Gets or sets the current zoom level of the <see cref="T:System.Windows.Forms.RichTextBox" />.</summary>
		/// <returns>The factor by which the contents of the control is zoomed.</returns>
		/// <exception cref="T:System.ArgumentException">The specified zoom factor did not fall within the permissible range. </exception>
		// Token: 0x17000CB0 RID: 3248
		// (get) Token: 0x060032E4 RID: 13028 RVA: 0x000EC548 File Offset: 0x000EA748
		// (set) Token: 0x060032E5 RID: 13029 RVA: 0x000EC59C File Offset: 0x000EA79C
		[SRCategory("CatBehavior")]
		[DefaultValue(1f)]
		[Localizable(true)]
		[SRDescription("RichTextBoxZoomFactor")]
		public float ZoomFactor
		{
			get
			{
				if (base.IsHandleCreated)
				{
					int num = 0;
					int num2 = 0;
					base.SendMessage(1248, ref num, ref num2);
					if (num != 0 && num2 != 0)
					{
						this.zoomMultiplier = (float)num / (float)num2;
					}
					else
					{
						this.zoomMultiplier = 1f;
					}
					return this.zoomMultiplier;
				}
				return this.zoomMultiplier;
			}
			set
			{
				if (this.zoomMultiplier == value)
				{
					return;
				}
				if (value <= 0.015625f || value >= 64f)
				{
					throw new ArgumentOutOfRangeException("ZoomFactor", SR.GetString("InvalidExBoundArgument", new object[]
					{
						"ZoomFactor",
						value.ToString(CultureInfo.CurrentCulture),
						0.015625f.ToString(CultureInfo.CurrentCulture),
						64f.ToString(CultureInfo.CurrentCulture)
					}));
				}
				this.SendZoomFactor(value);
			}
		}

		/// <summary>Occurs when contents within the control are resized.</summary>
		// Token: 0x1400026E RID: 622
		// (add) Token: 0x060032E6 RID: 13030 RVA: 0x000EC626 File Offset: 0x000EA826
		// (remove) Token: 0x060032E7 RID: 13031 RVA: 0x000EC639 File Offset: 0x000EA839
		[SRCategory("CatBehavior")]
		[SRDescription("RichTextBoxContentsResized")]
		public event ContentsResizedEventHandler ContentsResized
		{
			add
			{
				base.Events.AddHandler(RichTextBox.EVENT_REQUESTRESIZE, value);
			}
			remove
			{
				base.Events.RemoveHandler(RichTextBox.EVENT_REQUESTRESIZE, value);
			}
		}

		/// <summary>Occurs when the user completes a drag-and-drop </summary>
		// Token: 0x1400026F RID: 623
		// (add) Token: 0x060032E8 RID: 13032 RVA: 0x000EC64C File Offset: 0x000EA84C
		// (remove) Token: 0x060032E9 RID: 13033 RVA: 0x000EC655 File Offset: 0x000EA855
		[Browsable(false)]
		public new event DragEventHandler DragDrop
		{
			add
			{
				base.DragDrop += value;
			}
			remove
			{
				base.DragDrop -= value;
			}
		}

		/// <summary>Occurs when an object is dragged into the control's bounds.</summary>
		// Token: 0x14000270 RID: 624
		// (add) Token: 0x060032EA RID: 13034 RVA: 0x000EC65E File Offset: 0x000EA85E
		// (remove) Token: 0x060032EB RID: 13035 RVA: 0x000EC667 File Offset: 0x000EA867
		[Browsable(false)]
		public new event DragEventHandler DragEnter
		{
			add
			{
				base.DragEnter += value;
			}
			remove
			{
				base.DragEnter -= value;
			}
		}

		/// <summary>Occurs when an object is dragged out of the control's bounds.</summary>
		// Token: 0x14000271 RID: 625
		// (add) Token: 0x060032EC RID: 13036 RVA: 0x000EC670 File Offset: 0x000EA870
		// (remove) Token: 0x060032ED RID: 13037 RVA: 0x000EC679 File Offset: 0x000EA879
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event EventHandler DragLeave
		{
			add
			{
				base.DragLeave += value;
			}
			remove
			{
				base.DragLeave -= value;
			}
		}

		/// <summary>Occurs when an object is dragged over the control's bounds.</summary>
		// Token: 0x14000272 RID: 626
		// (add) Token: 0x060032EE RID: 13038 RVA: 0x000EC682 File Offset: 0x000EA882
		// (remove) Token: 0x060032EF RID: 13039 RVA: 0x000EC68B File Offset: 0x000EA88B
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event DragEventHandler DragOver
		{
			add
			{
				base.DragOver += value;
			}
			remove
			{
				base.DragOver -= value;
			}
		}

		/// <summary>Occurs during a drag operation.</summary>
		// Token: 0x14000273 RID: 627
		// (add) Token: 0x060032F0 RID: 13040 RVA: 0x000EC694 File Offset: 0x000EA894
		// (remove) Token: 0x060032F1 RID: 13041 RVA: 0x000EC69D File Offset: 0x000EA89D
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event GiveFeedbackEventHandler GiveFeedback
		{
			add
			{
				base.GiveFeedback += value;
			}
			remove
			{
				base.GiveFeedback -= value;
			}
		}

		/// <summary>This event is not relevant to this class.</summary>
		// Token: 0x14000274 RID: 628
		// (add) Token: 0x060032F2 RID: 13042 RVA: 0x000EC6A6 File Offset: 0x000EA8A6
		// (remove) Token: 0x060032F3 RID: 13043 RVA: 0x000EC6AF File Offset: 0x000EA8AF
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event QueryContinueDragEventHandler QueryContinueDrag
		{
			add
			{
				base.QueryContinueDrag += value;
			}
			remove
			{
				base.QueryContinueDrag -= value;
			}
		}

		/// <summary>Occurs when the user clicks the horizontal scroll bar of the control.</summary>
		// Token: 0x14000275 RID: 629
		// (add) Token: 0x060032F4 RID: 13044 RVA: 0x000EC6B8 File Offset: 0x000EA8B8
		// (remove) Token: 0x060032F5 RID: 13045 RVA: 0x000EC6CB File Offset: 0x000EA8CB
		[SRCategory("CatBehavior")]
		[SRDescription("RichTextBoxHScroll")]
		public event EventHandler HScroll
		{
			add
			{
				base.Events.AddHandler(RichTextBox.EVENT_HSCROLL, value);
			}
			remove
			{
				base.Events.RemoveHandler(RichTextBox.EVENT_HSCROLL, value);
			}
		}

		/// <summary>Occurs when the user clicks on a link within the text of the control.</summary>
		// Token: 0x14000276 RID: 630
		// (add) Token: 0x060032F6 RID: 13046 RVA: 0x000EC6DE File Offset: 0x000EA8DE
		// (remove) Token: 0x060032F7 RID: 13047 RVA: 0x000EC6F1 File Offset: 0x000EA8F1
		[SRCategory("CatBehavior")]
		[SRDescription("RichTextBoxLinkClick")]
		public event LinkClickedEventHandler LinkClicked
		{
			add
			{
				base.Events.AddHandler(RichTextBox.EVENT_LINKACTIVATE, value);
			}
			remove
			{
				base.Events.RemoveHandler(RichTextBox.EVENT_LINKACTIVATE, value);
			}
		}

		/// <summary>Occurs when the user switches input methods on an Asian version of the Windows operating system.</summary>
		// Token: 0x14000277 RID: 631
		// (add) Token: 0x060032F8 RID: 13048 RVA: 0x000EC704 File Offset: 0x000EA904
		// (remove) Token: 0x060032F9 RID: 13049 RVA: 0x000EC717 File Offset: 0x000EA917
		[SRCategory("CatBehavior")]
		[SRDescription("RichTextBoxIMEChange")]
		public event EventHandler ImeChange
		{
			add
			{
				base.Events.AddHandler(RichTextBox.EVENT_IMECHANGE, value);
			}
			remove
			{
				base.Events.RemoveHandler(RichTextBox.EVENT_IMECHANGE, value);
			}
		}

		/// <summary>Occurs when the user attempts to modify protected text in the control.</summary>
		// Token: 0x14000278 RID: 632
		// (add) Token: 0x060032FA RID: 13050 RVA: 0x000EC72A File Offset: 0x000EA92A
		// (remove) Token: 0x060032FB RID: 13051 RVA: 0x000EC73D File Offset: 0x000EA93D
		[SRCategory("CatBehavior")]
		[SRDescription("RichTextBoxProtected")]
		public event EventHandler Protected
		{
			add
			{
				base.Events.AddHandler(RichTextBox.EVENT_PROTECTED, value);
			}
			remove
			{
				base.Events.RemoveHandler(RichTextBox.EVENT_PROTECTED, value);
			}
		}

		/// <summary>Occurs when the selection of text within the control has changed.</summary>
		// Token: 0x14000279 RID: 633
		// (add) Token: 0x060032FC RID: 13052 RVA: 0x000EC750 File Offset: 0x000EA950
		// (remove) Token: 0x060032FD RID: 13053 RVA: 0x000EC763 File Offset: 0x000EA963
		[SRCategory("CatBehavior")]
		[SRDescription("RichTextBoxSelChange")]
		public event EventHandler SelectionChanged
		{
			add
			{
				base.Events.AddHandler(RichTextBox.EVENT_SELCHANGE, value);
			}
			remove
			{
				base.Events.RemoveHandler(RichTextBox.EVENT_SELCHANGE, value);
			}
		}

		/// <summary>Occurs when the user clicks the vertical scroll bars of the control.</summary>
		// Token: 0x1400027A RID: 634
		// (add) Token: 0x060032FE RID: 13054 RVA: 0x000EC776 File Offset: 0x000EA976
		// (remove) Token: 0x060032FF RID: 13055 RVA: 0x000EC789 File Offset: 0x000EA989
		[SRCategory("CatBehavior")]
		[SRDescription("RichTextBoxVScroll")]
		public event EventHandler VScroll
		{
			add
			{
				base.Events.AddHandler(RichTextBox.EVENT_VSCROLL, value);
			}
			remove
			{
				base.Events.RemoveHandler(RichTextBox.EVENT_VSCROLL, value);
			}
		}

		/// <summary>Determines whether you can paste information from the Clipboard in the specified data format.</summary>
		/// <param name="clipFormat">One of the <see cref="T:System.Windows.Forms.DataFormats.Format" /> values. </param>
		/// <returns>
		///     <see langword="true" /> if you can paste data from the Clipboard in the specified data format; otherwise, <see langword="false" />.</returns>
		// Token: 0x06003300 RID: 13056 RVA: 0x000EC79C File Offset: 0x000EA99C
		public bool CanPaste(DataFormats.Format clipFormat)
		{
			return (int)((long)base.SendMessage(1074, clipFormat.Id, 0)) != 0;
		}

		/// <summary>This method is not relevant for this class.</summary>
		/// <param name="bitmap">A <see cref="T:System.Drawing.Bitmap" />.</param>
		/// <param name="targetBounds">A <see cref="T:System.Drawing.Rectangle" />.</param>
		// Token: 0x06003301 RID: 13057 RVA: 0x00012A9D File Offset: 0x00010C9D
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new void DrawToBitmap(Bitmap bitmap, Rectangle targetBounds)
		{
			base.DrawToBitmap(bitmap, targetBounds);
		}

		// Token: 0x06003302 RID: 13058 RVA: 0x000EC7C8 File Offset: 0x000EA9C8
		private unsafe int EditStreamProc(IntPtr dwCookie, IntPtr buf, int cb, out int transferred)
		{
			int result = 0;
			byte[] array = new byte[cb];
			int num = (int)dwCookie;
			transferred = 0;
			try
			{
				int num2 = num & 3;
				if (num2 != 1)
				{
					if (num2 == 2)
					{
						if (this.editStream == null)
						{
							this.editStream = new MemoryStream();
						}
						num2 = (num & 112);
						if (num2 != 16)
						{
							if (num2 == 32 || num2 == 64)
							{
								Marshal.Copy(buf, array, 0, cb);
								this.editStream.Write(array, 0, cb);
							}
						}
						else if ((num & 8) != 0)
						{
							int num3 = cb / 2;
							int num4 = 0;
							try
							{
								fixed (byte* ptr = array)
								{
									char* ptr2 = (char*)ptr;
									char* ptr3 = (long)buf;
									for (int i = 0; i < num3; i++)
									{
										if (*ptr3 == '\r')
										{
											ptr3++;
										}
										else
										{
											*ptr2 = *ptr3;
											ptr2++;
											ptr3++;
											num4++;
										}
									}
								}
							}
							finally
							{
								byte* ptr = null;
							}
							this.editStream.Write(array, 0, num4 * 2);
						}
						else
						{
							int num5 = 0;
							try
							{
								fixed (byte* ptr4 = array)
								{
									byte* ptr5 = ptr4;
									byte* ptr6 = (long)buf;
									for (int j = 0; j < cb; j++)
									{
										if (*ptr6 == 13)
										{
											ptr6++;
										}
										else
										{
											*ptr5 = *ptr6;
											ptr5++;
											ptr6++;
											num5++;
										}
									}
								}
							}
							finally
							{
								byte* ptr4 = null;
							}
							this.editStream.Write(array, 0, num5);
						}
						transferred = cb;
					}
				}
				else if (this.editStream != null)
				{
					transferred = this.editStream.Read(array, 0, cb);
					Marshal.Copy(array, 0, buf, transferred);
					if (transferred < 0)
					{
						transferred = 0;
					}
				}
				else
				{
					transferred = 0;
				}
			}
			catch (IOException)
			{
				transferred = 0;
				result = 1;
			}
			return result;
		}

		/// <summary>Searches the text in a <see cref="T:System.Windows.Forms.RichTextBox" /> control for a string.</summary>
		/// <param name="str">The text to locate in the control. </param>
		/// <returns>The location within the control where the search text was found or -1 if the search string is not found or an empty search string is specified in the <paramref name="str" /> parameter.</returns>
		// Token: 0x06003303 RID: 13059 RVA: 0x000EC9E0 File Offset: 0x000EABE0
		public int Find(string str)
		{
			return this.Find(str, 0, 0, RichTextBoxFinds.None);
		}

		/// <summary>Searches the text in a <see cref="T:System.Windows.Forms.RichTextBox" /> control for a string with specific options applied to the search.</summary>
		/// <param name="str">The text to locate in the control. </param>
		/// <param name="options">A bitwise combination of the <see cref="T:System.Windows.Forms.RichTextBoxFinds" /> values. </param>
		/// <returns>The location within the control where the search text was found.</returns>
		// Token: 0x06003304 RID: 13060 RVA: 0x000EC9EC File Offset: 0x000EABEC
		public int Find(string str, RichTextBoxFinds options)
		{
			return this.Find(str, 0, 0, options);
		}

		/// <summary>Searches the text in a <see cref="T:System.Windows.Forms.RichTextBox" /> control for a string at a specific location within the control and with specific options applied to the search.</summary>
		/// <param name="str">The text to locate in the control. </param>
		/// <param name="start">The location within the control's text at which to begin searching. </param>
		/// <param name="options">A bitwise combination of the <see cref="T:System.Windows.Forms.RichTextBoxFinds" /> values. </param>
		/// <returns>The location within the control where the search text was found.</returns>
		// Token: 0x06003305 RID: 13061 RVA: 0x000EC9F8 File Offset: 0x000EABF8
		public int Find(string str, int start, RichTextBoxFinds options)
		{
			return this.Find(str, start, -1, options);
		}

		/// <summary>Searches the text in a <see cref="T:System.Windows.Forms.RichTextBox" /> control for a string within a range of text within the control and with specific options applied to the search.</summary>
		/// <param name="str">The text to locate in the control. </param>
		/// <param name="start">The location within the control's text at which to begin searching. </param>
		/// <param name="end">The location within the control's text at which to end searching. This value must be equal to negative one (-1) or greater than or equal to the <paramref name="start" /> parameter. </param>
		/// <param name="options">A bitwise combination of the <see cref="T:System.Windows.Forms.RichTextBoxFinds" /> values. </param>
		/// <returns>The location within the control where the search text was found.</returns>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="str" /> parameter was <see langword="null" />. </exception>
		/// <exception cref="T:System.ArgumentException">The <paramref name="start" /> parameter was less than zero. </exception>
		/// <exception cref="T:System.ArgumentException">The <paramref name="end" /> parameter was less the <paramref name="start" /> parameter. </exception>
		// Token: 0x06003306 RID: 13062 RVA: 0x000ECA04 File Offset: 0x000EAC04
		public int Find(string str, int start, int end, RichTextBoxFinds options)
		{
			int textLength = this.TextLength;
			if (str == null)
			{
				throw new ArgumentNullException("str");
			}
			if (start < 0 || start > textLength)
			{
				throw new ArgumentOutOfRangeException("start", SR.GetString("InvalidBoundArgument", new object[]
				{
					"start",
					start,
					0,
					textLength
				}));
			}
			if (end < -1)
			{
				throw new ArgumentOutOfRangeException("end", SR.GetString("RichTextFindEndInvalid", new object[]
				{
					end
				}));
			}
			bool flag = true;
			NativeMethods.FINDTEXT findtext = new NativeMethods.FINDTEXT();
			findtext.chrg = new NativeMethods.CHARRANGE();
			findtext.lpstrText = str;
			if (end == -1)
			{
				end = textLength;
			}
			if (start > end)
			{
				throw new ArgumentException(SR.GetString("RichTextFindEndInvalid", new object[]
				{
					end
				}));
			}
			if ((options & RichTextBoxFinds.Reverse) != RichTextBoxFinds.Reverse)
			{
				findtext.chrg.cpMin = start;
				findtext.chrg.cpMax = end;
			}
			else
			{
				findtext.chrg.cpMin = end;
				findtext.chrg.cpMax = start;
			}
			if (findtext.chrg.cpMin == findtext.chrg.cpMax)
			{
				if ((options & RichTextBoxFinds.Reverse) != RichTextBoxFinds.Reverse)
				{
					findtext.chrg.cpMin = 0;
					findtext.chrg.cpMax = -1;
				}
				else
				{
					findtext.chrg.cpMin = textLength;
					findtext.chrg.cpMax = 0;
				}
			}
			int num = 0;
			if ((options & RichTextBoxFinds.WholeWord) == RichTextBoxFinds.WholeWord)
			{
				num |= 2;
			}
			if ((options & RichTextBoxFinds.MatchCase) == RichTextBoxFinds.MatchCase)
			{
				num |= 4;
			}
			if ((options & RichTextBoxFinds.NoHighlight) == RichTextBoxFinds.NoHighlight)
			{
				flag = false;
			}
			if ((options & RichTextBoxFinds.Reverse) != RichTextBoxFinds.Reverse)
			{
				num |= 1;
			}
			int num2 = (int)UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), 1080, num, findtext);
			if (num2 != -1 && flag)
			{
				NativeMethods.CHARRANGE charrange = new NativeMethods.CHARRANGE();
				charrange.cpMin = num2;
				char c = 'ـ';
				string text = this.Text;
				string text2 = text.Substring(num2, str.Length);
				int num3 = text2.IndexOf(c);
				if (num3 == -1)
				{
					charrange.cpMax = num2 + str.Length;
				}
				else
				{
					int i = num3;
					int num4 = num2 + num3;
					while (i < str.Length)
					{
						while (text[num4] == c && str[i] != c)
						{
							num4++;
						}
						i++;
						num4++;
					}
					charrange.cpMax = num4;
				}
				UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), 1079, 0, charrange);
				base.SendMessage(183, 0, 0);
			}
			return num2;
		}

		/// <summary>Searches the text of a <see cref="T:System.Windows.Forms.RichTextBox" /> control for the first instance of a character from a list of characters.</summary>
		/// <param name="characterSet">The array of characters to search for. </param>
		/// <returns>The location within the control where the search characters were found or -1 if the search characters are not found or an empty search character set is specified in the <paramref name="char" /> parameter.</returns>
		// Token: 0x06003307 RID: 13063 RVA: 0x000ECC89 File Offset: 0x000EAE89
		public int Find(char[] characterSet)
		{
			return this.Find(characterSet, 0, -1);
		}

		/// <summary>Searches the text of a <see cref="T:System.Windows.Forms.RichTextBox" /> control, at a specific starting point, for the first instance of a character from a list of characters.</summary>
		/// <param name="characterSet">The array of characters to search for. </param>
		/// <param name="start">The location within the control's text at which to begin searching. </param>
		/// <returns>The location within the control where the search characters are found.</returns>
		// Token: 0x06003308 RID: 13064 RVA: 0x000ECC94 File Offset: 0x000EAE94
		public int Find(char[] characterSet, int start)
		{
			return this.Find(characterSet, start, -1);
		}

		/// <summary>Searches a range of text in a <see cref="T:System.Windows.Forms.RichTextBox" /> control for the first instance of a character from a list of characters.</summary>
		/// <param name="characterSet">The array of characters to search for. </param>
		/// <param name="start">The location within the control's text at which to begin searching. </param>
		/// <param name="end">The location within the control's text at which to end searching. </param>
		/// <returns>The location within the control where the search characters are found.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="characterSet" /> is null. </exception>
		/// <exception cref="T:System.ArgumentException">
		///         <paramref name="start" /> is less than 0 or greater than the length of the text in the control. </exception>
		// Token: 0x06003309 RID: 13065 RVA: 0x000ECCA0 File Offset: 0x000EAEA0
		public int Find(char[] characterSet, int start, int end)
		{
			bool flag = true;
			bool negate = false;
			int textLength = this.TextLength;
			if (characterSet == null)
			{
				throw new ArgumentNullException("characterSet");
			}
			if (start < 0 || start > textLength)
			{
				throw new ArgumentOutOfRangeException("start", SR.GetString("InvalidBoundArgument", new object[]
				{
					"start",
					start,
					0,
					textLength
				}));
			}
			if (end < start && end != -1)
			{
				throw new ArgumentOutOfRangeException("end", SR.GetString("InvalidLowBoundArgumentEx", new object[]
				{
					"end",
					end,
					"start"
				}));
			}
			if (characterSet.Length == 0)
			{
				return -1;
			}
			int windowTextLength = SafeNativeMethods.GetWindowTextLength(new HandleRef(this, base.Handle));
			if (start == end)
			{
				start = 0;
				end = windowTextLength;
			}
			if (end == -1)
			{
				end = windowTextLength;
			}
			NativeMethods.CHARRANGE charrange = new NativeMethods.CHARRANGE();
			charrange.cpMax = (charrange.cpMin = start);
			NativeMethods.TEXTRANGE textrange = new NativeMethods.TEXTRANGE();
			textrange.chrg = new NativeMethods.CHARRANGE();
			textrange.chrg.cpMin = charrange.cpMin;
			textrange.chrg.cpMax = charrange.cpMax;
			UnsafeNativeMethods.CharBuffer charBuffer = UnsafeNativeMethods.CharBuffer.CreateBuffer(513);
			textrange.lpstrText = charBuffer.AllocCoTaskMem();
			if (textrange.lpstrText == IntPtr.Zero)
			{
				throw new OutOfMemoryException();
			}
			try
			{
				bool flag2 = false;
				while (!flag2)
				{
					if (flag)
					{
						textrange.chrg.cpMin = charrange.cpMax;
						textrange.chrg.cpMax += 512;
					}
					else
					{
						textrange.chrg.cpMax = charrange.cpMin;
						textrange.chrg.cpMin -= 512;
						if (textrange.chrg.cpMin < 0)
						{
							textrange.chrg.cpMin = 0;
						}
					}
					if (end != -1)
					{
						textrange.chrg.cpMax = Math.Min(textrange.chrg.cpMax, end);
					}
					int num = (int)UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), 1099, 0, textrange);
					if (num == 0)
					{
						charrange.cpMax = (charrange.cpMin = -1);
						break;
					}
					charBuffer.PutCoTaskMem(textrange.lpstrText);
					string @string = charBuffer.GetString();
					if (flag)
					{
						for (int i = 0; i < num; i++)
						{
							bool charInCharSet = this.GetCharInCharSet(@string[i], characterSet, negate);
							if (charInCharSet)
							{
								flag2 = true;
								break;
							}
							charrange.cpMax++;
						}
					}
					else
					{
						int index = num;
						while (index-- != 0)
						{
							bool charInCharSet2 = this.GetCharInCharSet(@string[index], characterSet, negate);
							if (charInCharSet2)
							{
								flag2 = true;
								break;
							}
							charrange.cpMin--;
						}
					}
				}
			}
			finally
			{
				if (textrange.lpstrText != IntPtr.Zero)
				{
					Marshal.FreeCoTaskMem(textrange.lpstrText);
				}
			}
			return flag ? charrange.cpMax : charrange.cpMin;
		}

		// Token: 0x0600330A RID: 13066 RVA: 0x000ECFC8 File Offset: 0x000EB1C8
		private void ForceHandleCreate()
		{
			if (!base.IsHandleCreated)
			{
				this.CreateHandle();
			}
		}

		// Token: 0x0600330B RID: 13067 RVA: 0x000ECFD8 File Offset: 0x000EB1D8
		private bool InternalSetForeColor(Color value)
		{
			NativeMethods.CHARFORMATA charFormat = this.GetCharFormat(false);
			if ((charFormat.dwMask & 1073741824) != 0 && ColorTranslator.ToWin32(value) == charFormat.crTextColor)
			{
				return true;
			}
			charFormat.dwMask = 1073741824;
			charFormat.dwEffects = 0;
			charFormat.crTextColor = ColorTranslator.ToWin32(value);
			return this.SetCharFormat(4, charFormat);
		}

		// Token: 0x0600330C RID: 13068 RVA: 0x000ED034 File Offset: 0x000EB234
		private NativeMethods.CHARFORMATA GetCharFormat(bool fSelection)
		{
			NativeMethods.CHARFORMATA charformata = new NativeMethods.CHARFORMATA();
			UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), 1082, fSelection ? 1 : 0, charformata);
			return charformata;
		}

		// Token: 0x0600330D RID: 13069 RVA: 0x000ED068 File Offset: 0x000EB268
		private NativeMethods.CHARFORMAT2A GetCharFormat2(bool fSelection)
		{
			NativeMethods.CHARFORMAT2A charformat2A = new NativeMethods.CHARFORMAT2A();
			UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), 1082, fSelection ? 1 : 0, charformat2A);
			return charformat2A;
		}

		// Token: 0x0600330E RID: 13070 RVA: 0x000ED09C File Offset: 0x000EB29C
		private RichTextBoxSelectionAttribute GetCharFormat(int mask, int effect)
		{
			RichTextBoxSelectionAttribute result = RichTextBoxSelectionAttribute.None;
			if (base.IsHandleCreated)
			{
				NativeMethods.CHARFORMATA charFormat = this.GetCharFormat(true);
				if ((charFormat.dwMask & mask) != 0 && (charFormat.dwEffects & effect) != 0)
				{
					result = RichTextBoxSelectionAttribute.All;
				}
			}
			return result;
		}

		// Token: 0x0600330F RID: 13071 RVA: 0x000ED0D4 File Offset: 0x000EB2D4
		private Font GetCharFormatFont(bool selectionOnly)
		{
			this.ForceHandleCreate();
			NativeMethods.CHARFORMATA charFormat = this.GetCharFormat(selectionOnly);
			if ((charFormat.dwMask & 536870912) == 0)
			{
				return null;
			}
			string text = Encoding.Default.GetString(charFormat.szFaceName);
			int num = text.IndexOf('\0');
			if (num != -1)
			{
				text = text.Substring(0, num);
			}
			float num2 = 13f;
			if ((charFormat.dwMask & -2147483648) != 0)
			{
				num2 = (float)charFormat.yHeight / 20f;
				if (num2 == 0f && charFormat.yHeight > 0)
				{
					num2 = 1f;
				}
			}
			FontStyle fontStyle = FontStyle.Regular;
			if ((charFormat.dwMask & 1) != 0 && (charFormat.dwEffects & 1) != 0)
			{
				fontStyle |= FontStyle.Bold;
			}
			if ((charFormat.dwMask & 2) != 0 && (charFormat.dwEffects & 2) != 0)
			{
				fontStyle |= FontStyle.Italic;
			}
			if ((charFormat.dwMask & 8) != 0 && (charFormat.dwEffects & 8) != 0)
			{
				fontStyle |= FontStyle.Strikeout;
			}
			if ((charFormat.dwMask & 4) != 0 && (charFormat.dwEffects & 4) != 0)
			{
				fontStyle |= FontStyle.Underline;
			}
			try
			{
				return new Font(text, num2, fontStyle, GraphicsUnit.Point, charFormat.bCharSet);
			}
			catch
			{
			}
			return null;
		}

		/// <summary>Retrieves the index of the character nearest to the specified location.</summary>
		/// <param name="pt">The location to search. </param>
		/// <returns>The zero-based character index at the specified location.</returns>
		// Token: 0x06003310 RID: 13072 RVA: 0x000ED1F8 File Offset: 0x000EB3F8
		public override int GetCharIndexFromPosition(Point pt)
		{
			NativeMethods.POINT lParam = new NativeMethods.POINT(pt.X, pt.Y);
			int num = (int)UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), 215, 0, lParam);
			string text = this.Text;
			if (num >= text.Length)
			{
				num = Math.Max(text.Length - 1, 0);
			}
			return num;
		}

		// Token: 0x06003311 RID: 13073 RVA: 0x000ED258 File Offset: 0x000EB458
		private bool GetCharInCharSet(char c, char[] charSet, bool negate)
		{
			bool flag = false;
			int num = charSet.Length;
			int num2 = 0;
			while (!flag && num2 < num)
			{
				flag = (c == charSet[num2]);
				num2++;
			}
			if (!negate)
			{
				return flag;
			}
			return !flag;
		}

		/// <summary>Retrieves the line number from the specified character position within the text of the <see cref="T:System.Windows.Forms.RichTextBox" /> control.</summary>
		/// <param name="index">The character index position to search. </param>
		/// <returns>The zero-based line number in which the character index is located.</returns>
		// Token: 0x06003312 RID: 13074 RVA: 0x000ED28A File Offset: 0x000EB48A
		public override int GetLineFromCharIndex(int index)
		{
			return (int)((long)base.SendMessage(1078, 0, index));
		}

		/// <summary>Retrieves the location within the control at the specified character index.</summary>
		/// <param name="index">The index of the character for which to retrieve the location. </param>
		/// <returns>The location of the specified character.</returns>
		// Token: 0x06003313 RID: 13075 RVA: 0x000ED2A0 File Offset: 0x000EB4A0
		public override Point GetPositionFromCharIndex(int index)
		{
			if (RichTextBox.richEditMajorVersion == 2)
			{
				return base.GetPositionFromCharIndex(index);
			}
			if (index < 0 || index > this.Text.Length)
			{
				return Point.Empty;
			}
			NativeMethods.POINT point = new NativeMethods.POINT();
			UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), 214, point, index);
			return new Point(point.x, point.y);
		}

		// Token: 0x06003314 RID: 13076 RVA: 0x000ED305 File Offset: 0x000EB505
		private bool GetProtectedError()
		{
			if (this.ProtectedError)
			{
				this.ProtectedError = false;
				return true;
			}
			return false;
		}

		/// <summary>Loads a rich text format (RTF) or standard ASCII text file into the <see cref="T:System.Windows.Forms.RichTextBox" /> control.</summary>
		/// <param name="path">The name and location of the file to load into the control. </param>
		/// <exception cref="T:System.IO.IOException">An error occurred while loading the file into the control. </exception>
		/// <exception cref="T:System.ArgumentException">The file being loaded is not an RTF document. </exception>
		// Token: 0x06003315 RID: 13077 RVA: 0x000ED319 File Offset: 0x000EB519
		public void LoadFile(string path)
		{
			this.LoadFile(path, RichTextBoxStreamType.RichText);
		}

		/// <summary>Loads a specific type of file into the <see cref="T:System.Windows.Forms.RichTextBox" /> control.</summary>
		/// <param name="path">The name and location of the file to load into the control. </param>
		/// <param name="fileType">One of the <see cref="T:System.Windows.Forms.RichTextBoxStreamType" /> values. </param>
		/// <exception cref="T:System.IO.IOException">An error occurred while loading the file into the control. </exception>
		/// <exception cref="T:System.ArgumentException">The file being loaded is not an RTF document. </exception>
		// Token: 0x06003316 RID: 13078 RVA: 0x000ED324 File Offset: 0x000EB524
		public void LoadFile(string path, RichTextBoxStreamType fileType)
		{
			if (!ClientUtils.IsEnumValid(fileType, (int)fileType, 0, 4))
			{
				throw new InvalidEnumArgumentException("fileType", (int)fileType, typeof(RichTextBoxStreamType));
			}
			Stream stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
			try
			{
				this.LoadFile(stream, fileType);
			}
			finally
			{
				stream.Close();
			}
		}

		/// <summary>Loads the contents of an existing data stream into the <see cref="T:System.Windows.Forms.RichTextBox" /> control.</summary>
		/// <param name="data">A stream of data to load into the <see cref="T:System.Windows.Forms.RichTextBox" /> control. </param>
		/// <param name="fileType">One of the <see cref="T:System.Windows.Forms.RichTextBoxStreamType" /> values. </param>
		/// <exception cref="T:System.IO.IOException">An error occurred while loading the file into the control. </exception>
		/// <exception cref="T:System.ArgumentException">The file being loaded is not an RTF document. </exception>
		// Token: 0x06003317 RID: 13079 RVA: 0x000ED384 File Offset: 0x000EB584
		public void LoadFile(Stream data, RichTextBoxStreamType fileType)
		{
			if (!ClientUtils.IsEnumValid(fileType, (int)fileType, 0, 4))
			{
				throw new InvalidEnumArgumentException("fileType", (int)fileType, typeof(RichTextBoxStreamType));
			}
			int flags;
			switch (fileType)
			{
			case RichTextBoxStreamType.RichText:
				flags = 2;
				goto IL_6A;
			case RichTextBoxStreamType.PlainText:
				this.Rtf = "";
				flags = 1;
				goto IL_6A;
			case RichTextBoxStreamType.UnicodePlainText:
				flags = 17;
				goto IL_6A;
			}
			throw new ArgumentException(SR.GetString("InvalidFileType"));
			IL_6A:
			this.StreamIn(data, flags);
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.BackColorChanged" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
		// Token: 0x06003318 RID: 13080 RVA: 0x000ED403 File Offset: 0x000EB603
		protected override void OnBackColorChanged(EventArgs e)
		{
			if (base.IsHandleCreated)
			{
				base.SendMessage(1091, 0, ColorTranslator.ToWin32(this.BackColor));
			}
			base.OnBackColorChanged(e);
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.ContextMenuChanged" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
		// Token: 0x06003319 RID: 13081 RVA: 0x000ED42C File Offset: 0x000EB62C
		protected override void OnContextMenuChanged(EventArgs e)
		{
			base.OnContextMenuChanged(e);
			this.UpdateOleCallback();
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.RightToLeftChanged" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
		// Token: 0x0600331A RID: 13082 RVA: 0x000ED43C File Offset: 0x000EB63C
		protected override void OnRightToLeftChanged(EventArgs e)
		{
			base.OnRightToLeftChanged(e);
			string windowText = this.WindowText;
			base.ForceWindowText(null);
			base.ForceWindowText(windowText);
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.RichTextBox.ContentsResized" /> event.</summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.ContentsResizedEventArgs" /> that contains the event data. </param>
		// Token: 0x0600331B RID: 13083 RVA: 0x000ED468 File Offset: 0x000EB668
		protected virtual void OnContentsResized(ContentsResizedEventArgs e)
		{
			ContentsResizedEventHandler contentsResizedEventHandler = (ContentsResizedEventHandler)base.Events[RichTextBox.EVENT_REQUESTRESIZE];
			if (contentsResizedEventHandler != null)
			{
				contentsResizedEventHandler(this, e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.HandleCreated" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
		// Token: 0x0600331C RID: 13084 RVA: 0x000ED498 File Offset: 0x000EB698
		protected override void OnHandleCreated(EventArgs e)
		{
			this.curSelStart = (this.curSelEnd = (int)(this.curSelType = -1));
			this.UpdateMaxLength();
			base.SendMessage(1093, 0, 79626255);
			int num = this.rightMargin;
			this.rightMargin = 0;
			this.RightMargin = num;
			base.SendMessage(1115, this.DetectUrls ? 1 : 0, 0);
			if (this.selectionBackColorToSetOnHandleCreated != Color.Empty)
			{
				this.SelectionBackColor = this.selectionBackColorToSetOnHandleCreated;
			}
			this.AutoWordSelection = this.AutoWordSelection;
			base.SendMessage(1091, 0, ColorTranslator.ToWin32(this.BackColor));
			this.InternalSetForeColor(this.ForeColor);
			base.OnHandleCreated(e);
			this.UpdateOleCallback();
			try
			{
				this.SuppressTextChangedEvent = true;
				if (this.textRtf != null)
				{
					string rtf = this.textRtf;
					this.textRtf = null;
					this.Rtf = rtf;
				}
				else if (this.textPlain != null)
				{
					string text = this.textPlain;
					this.textPlain = null;
					this.Text = text;
				}
			}
			finally
			{
				this.SuppressTextChangedEvent = false;
			}
			base.SetSelectionOnHandle();
			if (this.ShowSelectionMargin)
			{
				UnsafeNativeMethods.PostMessage(new HandleRef(this, base.Handle), 1101, (IntPtr)2, (IntPtr)16777216);
			}
			if (this.languageOption != this.LanguageOption)
			{
				this.LanguageOption = this.languageOption;
			}
			base.ClearUndo();
			this.SendZoomFactor(this.zoomMultiplier);
			SystemEvents.UserPreferenceChanged += this.UserPreferenceChangedHandler;
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.HandleDestroyed" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
		// Token: 0x0600331D RID: 13085 RVA: 0x000ED634 File Offset: 0x000EB834
		protected override void OnHandleDestroyed(EventArgs e)
		{
			base.OnHandleDestroyed(e);
			if (!this.InConstructor)
			{
				this.textRtf = this.Rtf;
				if (this.textRtf.Length == 0)
				{
					this.textRtf = null;
				}
			}
			this.oleCallback = null;
			SystemEvents.UserPreferenceChanged -= this.UserPreferenceChangedHandler;
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.RichTextBox.HScroll" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x0600331E RID: 13086 RVA: 0x000ED688 File Offset: 0x000EB888
		protected virtual void OnHScroll(EventArgs e)
		{
			EventHandler eventHandler = (EventHandler)base.Events[RichTextBox.EVENT_HSCROLL];
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.RichTextBox.LinkClicked" /> event.</summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.LinkClickedEventArgs" /> that contains the event data. </param>
		// Token: 0x0600331F RID: 13087 RVA: 0x000ED6B8 File Offset: 0x000EB8B8
		protected virtual void OnLinkClicked(LinkClickedEventArgs e)
		{
			LinkClickedEventHandler linkClickedEventHandler = (LinkClickedEventHandler)base.Events[RichTextBox.EVENT_LINKACTIVATE];
			if (linkClickedEventHandler != null)
			{
				linkClickedEventHandler(this, e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.RichTextBox.ImeChange" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x06003320 RID: 13088 RVA: 0x000ED6E8 File Offset: 0x000EB8E8
		protected virtual void OnImeChange(EventArgs e)
		{
			EventHandler eventHandler = (EventHandler)base.Events[RichTextBox.EVENT_IMECHANGE];
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.RichTextBox.Protected" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x06003321 RID: 13089 RVA: 0x000ED718 File Offset: 0x000EB918
		protected virtual void OnProtected(EventArgs e)
		{
			this.ProtectedError = true;
			EventHandler eventHandler = (EventHandler)base.Events[RichTextBox.EVENT_PROTECTED];
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.RichTextBox.SelectionChanged" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x06003322 RID: 13090 RVA: 0x000ED750 File Offset: 0x000EB950
		protected virtual void OnSelectionChanged(EventArgs e)
		{
			EventHandler eventHandler = (EventHandler)base.Events[RichTextBox.EVENT_SELCHANGE];
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.RichTextBox.VScroll" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x06003323 RID: 13091 RVA: 0x000ED780 File Offset: 0x000EB980
		protected virtual void OnVScroll(EventArgs e)
		{
			EventHandler eventHandler = (EventHandler)base.Events[RichTextBox.EVENT_VSCROLL];
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		/// <summary>Pastes the contents of the Clipboard in the specified Clipboard format.</summary>
		/// <param name="clipFormat">The Clipboard format in which the data should be obtained from the Clipboard. </param>
		// Token: 0x06003324 RID: 13092 RVA: 0x000ED7AE File Offset: 0x000EB9AE
		public void Paste(DataFormats.Format clipFormat)
		{
			IntSecurity.ClipboardRead.Demand();
			this.PasteUnsafe(clipFormat, 0);
		}

		// Token: 0x06003325 RID: 13093 RVA: 0x000ED7C4 File Offset: 0x000EB9C4
		private void PasteUnsafe(DataFormats.Format clipFormat, int hIcon)
		{
			NativeMethods.REPASTESPECIAL repastespecial = null;
			if (hIcon != 0)
			{
				repastespecial = new NativeMethods.REPASTESPECIAL();
				repastespecial.dwAspect = 4;
				repastespecial.dwParam = hIcon;
			}
			UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), 1088, clipFormat.Id, repastespecial);
		}

		/// <summary>Processes a command key.</summary>
		/// <param name="m">A <see cref="T:System.Windows.Forms.Message" />, passed by reference, that represents the window message to process. </param>
		/// <param name="keyData">One of the <see cref="T:System.Windows.Forms.Keys" /> values that represents the  key to process.</param>
		/// <returns>
		///   <see langword="true" /> if the character was processed by the control; otherwise, <see langword="false" />.</returns>
		// Token: 0x06003326 RID: 13094 RVA: 0x000ED808 File Offset: 0x000EBA08
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		protected override bool ProcessCmdKey(ref Message m, Keys keyData)
		{
			if (!this.RichTextShortcutsEnabled)
			{
				foreach (int num in RichTextBox.shortcutsToDisable)
				{
					if (keyData == (Keys)num)
					{
						return true;
					}
				}
			}
			return base.ProcessCmdKey(ref m, keyData);
		}

		/// <summary>Reapplies the last operation that was undone in the control.</summary>
		// Token: 0x06003327 RID: 13095 RVA: 0x000ED843 File Offset: 0x000EBA43
		public void Redo()
		{
			base.SendMessage(1108, 0, 0);
		}

		/// <summary>Saves the contents of the <see cref="T:System.Windows.Forms.RichTextBox" /> to a rich text format (RTF) file.</summary>
		/// <param name="path">The name and location of the file to save. </param>
		/// <exception cref="T:System.IO.IOException">An error occurs in saving the contents of the control to a file. </exception>
		// Token: 0x06003328 RID: 13096 RVA: 0x000ED853 File Offset: 0x000EBA53
		public void SaveFile(string path)
		{
			this.SaveFile(path, RichTextBoxStreamType.RichText);
		}

		/// <summary>Saves the contents of the <see cref="T:System.Windows.Forms.RichTextBox" /> to a specific type of file.</summary>
		/// <param name="path">The name and location of the file to save. </param>
		/// <param name="fileType">One of the <see cref="T:System.Windows.Forms.RichTextBoxStreamType" /> values. </param>
		/// <exception cref="T:System.ArgumentException">An invalid file type is specified in the <paramref name="fileType" /> parameter. </exception>
		/// <exception cref="T:System.IO.IOException">An error occurs in saving the contents of the control to a file. </exception>
		// Token: 0x06003329 RID: 13097 RVA: 0x000ED860 File Offset: 0x000EBA60
		public void SaveFile(string path, RichTextBoxStreamType fileType)
		{
			if (!ClientUtils.IsEnumValid(fileType, (int)fileType, 0, 4))
			{
				throw new InvalidEnumArgumentException("fileType", (int)fileType, typeof(RichTextBoxStreamType));
			}
			Stream stream = File.Create(path);
			try
			{
				this.SaveFile(stream, fileType);
			}
			finally
			{
				stream.Close();
			}
		}

		/// <summary>Saves the contents of a <see cref="T:System.Windows.Forms.RichTextBox" /> control to an open data stream.</summary>
		/// <param name="data">The data stream that contains the file to save to. </param>
		/// <param name="fileType">One of the <see cref="T:System.Windows.Forms.RichTextBoxStreamType" /> values. </param>
		/// <exception cref="T:System.ArgumentException">An invalid file type is specified in the <paramref name="fileType" /> parameter. </exception>
		/// <exception cref="T:System.IO.IOException">An error occurs in saving the contents of the control to a file. </exception>
		// Token: 0x0600332A RID: 13098 RVA: 0x000ED8BC File Offset: 0x000EBABC
		public void SaveFile(Stream data, RichTextBoxStreamType fileType)
		{
			int flags;
			switch (fileType)
			{
			case RichTextBoxStreamType.RichText:
				flags = 2;
				break;
			case RichTextBoxStreamType.PlainText:
				flags = 1;
				break;
			case RichTextBoxStreamType.RichNoOleObjs:
				flags = 3;
				break;
			case RichTextBoxStreamType.TextTextOleObjs:
				flags = 4;
				break;
			case RichTextBoxStreamType.UnicodePlainText:
				flags = 17;
				break;
			default:
				throw new InvalidEnumArgumentException("fileType", (int)fileType, typeof(RichTextBoxStreamType));
			}
			this.StreamOut(data, flags, true);
		}

		// Token: 0x0600332B RID: 13099 RVA: 0x000ED91C File Offset: 0x000EBB1C
		private void SendZoomFactor(float zoom)
		{
			int num;
			int num2;
			if (zoom == 1f)
			{
				num = 0;
				num2 = 0;
			}
			else
			{
				num = 1000;
				float num3 = 1000f * zoom;
				num2 = (int)Math.Ceiling((double)num3);
				if (num2 >= 64000)
				{
					num2 = (int)Math.Floor((double)num3);
				}
			}
			if (base.IsHandleCreated)
			{
				base.SendMessage(1249, num2, num);
			}
			if (num2 != 0)
			{
				this.zoomMultiplier = (float)num2 / (float)num;
				return;
			}
			this.zoomMultiplier = 1f;
		}

		// Token: 0x0600332C RID: 13100 RVA: 0x000ED990 File Offset: 0x000EBB90
		private bool SetCharFormat(int mask, int effect, RichTextBoxSelectionAttribute charFormat)
		{
			if (base.IsHandleCreated)
			{
				NativeMethods.CHARFORMATA charformata = new NativeMethods.CHARFORMATA();
				charformata.dwMask = mask;
				if (charFormat != RichTextBoxSelectionAttribute.None)
				{
					if (charFormat != RichTextBoxSelectionAttribute.All)
					{
						throw new ArgumentException(SR.GetString("UnknownAttr"));
					}
					charformata.dwEffects = effect;
				}
				else
				{
					charformata.dwEffects = 0;
				}
				return IntPtr.Zero != UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), 1092, 1, charformata);
			}
			return false;
		}

		// Token: 0x0600332D RID: 13101 RVA: 0x000ED9FF File Offset: 0x000EBBFF
		private bool SetCharFormat(int charRange, NativeMethods.CHARFORMATA cf)
		{
			return IntPtr.Zero != UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), 1092, charRange, cf);
		}

		// Token: 0x0600332E RID: 13102 RVA: 0x000EDA24 File Offset: 0x000EBC24
		private void SetCharFormatFont(bool selectionOnly, Font value)
		{
			this.ForceHandleCreate();
			NativeMethods.LOGFONT logfont = new NativeMethods.LOGFONT();
			RichTextBox.FontToLogFont(value, logfont);
			int dwMask = -1476394993;
			int num = 0;
			if (value.Bold)
			{
				num |= 1;
			}
			if (value.Italic)
			{
				num |= 2;
			}
			if (value.Strikeout)
			{
				num |= 8;
			}
			if (value.Underline)
			{
				num |= 4;
			}
			byte[] bytes;
			if (Marshal.SystemDefaultCharSize == 1)
			{
				bytes = Encoding.Default.GetBytes(logfont.lfFaceName);
				NativeMethods.CHARFORMATA charformata = new NativeMethods.CHARFORMATA();
				for (int i = 0; i < bytes.Length; i++)
				{
					charformata.szFaceName[i] = bytes[i];
				}
				charformata.dwMask = dwMask;
				charformata.dwEffects = num;
				charformata.yHeight = (int)(value.SizeInPoints * 20f);
				charformata.bCharSet = logfont.lfCharSet;
				charformata.bPitchAndFamily = logfont.lfPitchAndFamily;
				UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), 1092, selectionOnly ? 1 : 4, charformata);
				return;
			}
			bytes = Encoding.Unicode.GetBytes(logfont.lfFaceName);
			NativeMethods.CHARFORMATW charformatw = new NativeMethods.CHARFORMATW();
			for (int j = 0; j < bytes.Length; j++)
			{
				charformatw.szFaceName[j] = bytes[j];
			}
			charformatw.dwMask = dwMask;
			charformatw.dwEffects = num;
			charformatw.yHeight = (int)(value.SizeInPoints * 20f);
			charformatw.bCharSet = logfont.lfCharSet;
			charformatw.bPitchAndFamily = logfont.lfPitchAndFamily;
			UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), 1092, selectionOnly ? 1 : 4, charformatw);
		}

		// Token: 0x0600332F RID: 13103 RVA: 0x000EDBB4 File Offset: 0x000EBDB4
		[SecurityPermission(SecurityAction.Assert, Flags = SecurityPermissionFlag.UnmanagedCode)]
		private static void FontToLogFont(Font value, NativeMethods.LOGFONT logfont)
		{
			value.ToLogFont(logfont);
		}

		// Token: 0x06003330 RID: 13104 RVA: 0x000EDBC0 File Offset: 0x000EBDC0
		private static void SetupLogPixels(IntPtr hDC)
		{
			bool flag = false;
			if (hDC == IntPtr.Zero)
			{
				hDC = UnsafeNativeMethods.GetDC(NativeMethods.NullHandleRef);
				flag = true;
			}
			if (hDC == IntPtr.Zero)
			{
				return;
			}
			RichTextBox.logPixelsX = UnsafeNativeMethods.GetDeviceCaps(new HandleRef(null, hDC), 88);
			RichTextBox.logPixelsY = UnsafeNativeMethods.GetDeviceCaps(new HandleRef(null, hDC), 90);
			if (flag)
			{
				UnsafeNativeMethods.ReleaseDC(NativeMethods.NullHandleRef, new HandleRef(null, hDC));
			}
		}

		// Token: 0x06003331 RID: 13105 RVA: 0x000EDC34 File Offset: 0x000EBE34
		private static int Pixel2Twip(IntPtr hDC, int v, bool xDirection)
		{
			RichTextBox.SetupLogPixels(hDC);
			int num = xDirection ? RichTextBox.logPixelsX : RichTextBox.logPixelsY;
			return (int)((double)v / (double)num * 72.0 * 20.0);
		}

		// Token: 0x06003332 RID: 13106 RVA: 0x000EDC74 File Offset: 0x000EBE74
		private static int Twip2Pixel(IntPtr hDC, int v, bool xDirection)
		{
			RichTextBox.SetupLogPixels(hDC);
			int num = xDirection ? RichTextBox.logPixelsX : RichTextBox.logPixelsY;
			return (int)((double)v / 20.0 / 72.0 * (double)num);
		}

		// Token: 0x06003333 RID: 13107 RVA: 0x000EDCB4 File Offset: 0x000EBEB4
		private void StreamIn(string str, int flags)
		{
			if (str.Length != 0)
			{
				int num = str.IndexOf('\0');
				if (num != -1)
				{
					str = str.Substring(0, num);
				}
				byte[] bytes;
				if ((flags & 16) != 0)
				{
					bytes = Encoding.Unicode.GetBytes(str);
				}
				else
				{
					bytes = Encoding.Default.GetBytes(str);
				}
				this.editStream = new MemoryStream(bytes.Length);
				this.editStream.Write(bytes, 0, bytes.Length);
				this.editStream.Position = 0L;
				this.StreamIn(this.editStream, flags);
				return;
			}
			if ((32768 & flags) != 0)
			{
				base.SendMessage(771, 0, 0);
				this.ProtectedError = false;
				return;
			}
			base.SendMessage(12, 0, "");
		}

		// Token: 0x06003334 RID: 13108 RVA: 0x000EDD68 File Offset: 0x000EBF68
		private void StreamIn(Stream data, int flags)
		{
			if ((flags & 32768) == 0)
			{
				NativeMethods.CHARRANGE lParam = new NativeMethods.CHARRANGE();
				UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), 1079, 0, lParam);
			}
			try
			{
				this.editStream = data;
				if ((flags & 2) != 0)
				{
					long position = this.editStream.Position;
					byte[] array = new byte[RichTextBox.SZ_RTF_TAG.Length];
					this.editStream.Read(array, (int)position, RichTextBox.SZ_RTF_TAG.Length);
					string @string = Encoding.Default.GetString(array);
					if (!RichTextBox.SZ_RTF_TAG.Equals(@string))
					{
						throw new ArgumentException(SR.GetString("InvalidFileFormat"));
					}
					this.editStream.Position = position;
				}
				NativeMethods.EDITSTREAM editstream = new NativeMethods.EDITSTREAM();
				int num;
				if ((flags & 16) != 0)
				{
					num = 9;
				}
				else
				{
					num = 5;
				}
				if ((flags & 2) != 0)
				{
					num |= 64;
				}
				else
				{
					num |= 16;
				}
				editstream.dwCookie = (IntPtr)num;
				editstream.pfnCallback = new NativeMethods.EditStreamCallback(this.EditStreamProc);
				base.SendMessage(1077, 0, int.MaxValue);
				if (IntPtr.Size == 8)
				{
					NativeMethods.EDITSTREAM64 editstream2 = this.ConvertToEDITSTREAM64(editstream);
					UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), 1097, flags, editstream2);
					editstream.dwError = this.GetErrorValue64(editstream2);
				}
				else
				{
					UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), 1097, flags, editstream);
				}
				this.UpdateMaxLength();
				if (!this.GetProtectedError())
				{
					if (editstream.dwError != 0)
					{
						throw new InvalidOperationException(SR.GetString("LoadTextError"));
					}
					base.SendMessage(185, -1, 0);
					base.SendMessage(186, 0, 0);
				}
			}
			finally
			{
				this.editStream = null;
			}
		}

		// Token: 0x06003335 RID: 13109 RVA: 0x000EDF30 File Offset: 0x000EC130
		private string StreamOut(int flags)
		{
			Stream stream = new MemoryStream();
			this.StreamOut(stream, flags, false);
			stream.Position = 0L;
			int num = (int)stream.Length;
			string text = string.Empty;
			if (num > 0)
			{
				byte[] array = new byte[num];
				stream.Read(array, 0, num);
				if ((flags & 16) != 0)
				{
					text = Encoding.Unicode.GetString(array, 0, array.Length);
				}
				else
				{
					text = Encoding.Default.GetString(array, 0, array.Length);
				}
				if (!string.IsNullOrEmpty(text) && text[text.Length - 1] == '\0')
				{
					text = text.Substring(0, text.Length - 1);
				}
			}
			return text;
		}

		// Token: 0x06003336 RID: 13110 RVA: 0x000EDFC8 File Offset: 0x000EC1C8
		private void StreamOut(Stream data, int flags, bool includeCrLfs)
		{
			this.editStream = data;
			try
			{
				NativeMethods.EDITSTREAM editstream = new NativeMethods.EDITSTREAM();
				int num;
				if ((flags & 16) != 0)
				{
					num = 10;
				}
				else
				{
					num = 6;
				}
				if ((flags & 2) != 0)
				{
					num |= 64;
				}
				else if (includeCrLfs)
				{
					num |= 32;
				}
				else
				{
					num |= 16;
				}
				editstream.dwCookie = (IntPtr)num;
				editstream.pfnCallback = new NativeMethods.EditStreamCallback(this.EditStreamProc);
				if (IntPtr.Size == 8)
				{
					NativeMethods.EDITSTREAM64 editstream2 = this.ConvertToEDITSTREAM64(editstream);
					UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), 1098, flags, editstream2);
					editstream.dwError = this.GetErrorValue64(editstream2);
				}
				else
				{
					UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), 1098, flags, editstream);
				}
				if (editstream.dwError != 0)
				{
					throw new InvalidOperationException(SR.GetString("SaveTextError"));
				}
			}
			finally
			{
				this.editStream = null;
			}
		}

		// Token: 0x06003337 RID: 13111 RVA: 0x000EE0B0 File Offset: 0x000EC2B0
		private unsafe NativeMethods.EDITSTREAM64 ConvertToEDITSTREAM64(NativeMethods.EDITSTREAM es)
		{
			NativeMethods.EDITSTREAM64 editstream = new NativeMethods.EDITSTREAM64();
			fixed (byte* ptr = &editstream.contents[0])
			{
				*(long*)ptr = (long)es.dwCookie;
				((int*)ptr)[2] = es.dwError;
				long num = (long)Marshal.GetFunctionPointerForDelegate(es.pfnCallback);
				byte* ptr2 = (byte*)(&num);
				for (int i = 0; i < 8; i++)
				{
					editstream.contents[i + 12] = ptr2[i];
				}
			}
			return editstream;
		}

		// Token: 0x06003338 RID: 13112 RVA: 0x000EE124 File Offset: 0x000EC324
		private unsafe int GetErrorValue64(NativeMethods.EDITSTREAM64 es64)
		{
			int result;
			fixed (byte* ptr = &es64.contents[0])
			{
				result = ((int*)ptr)[2];
			}
			return result;
		}

		// Token: 0x06003339 RID: 13113 RVA: 0x000EE148 File Offset: 0x000EC348
		private void UpdateOleCallback()
		{
			if (base.IsHandleCreated)
			{
				if (this.oleCallback == null)
				{
					bool flag = false;
					try
					{
						IntSecurity.UnmanagedCode.Demand();
						flag = true;
					}
					catch (SecurityException)
					{
						flag = false;
					}
					if (flag)
					{
						this.AllowOleObjects = true;
					}
					else
					{
						this.AllowOleObjects = ((int)((long)base.SendMessage(1294, 0, 1)) != 0);
					}
					this.oleCallback = this.CreateRichEditOleCallback();
					IntPtr iunknownForObject = Marshal.GetIUnknownForObject(this.oleCallback);
					try
					{
						Guid guid = typeof(UnsafeNativeMethods.IRichEditOleCallback).GUID;
						IntPtr intPtr;
						Marshal.QueryInterface(iunknownForObject, ref guid, out intPtr);
						try
						{
							UnsafeNativeMethods.SendCallbackMessage(new HandleRef(this, base.Handle), 1094, IntPtr.Zero, intPtr);
						}
						finally
						{
							Marshal.Release(intPtr);
						}
					}
					finally
					{
						Marshal.Release(iunknownForObject);
					}
				}
				UnsafeNativeMethods.DragAcceptFiles(new HandleRef(this, base.Handle), false);
			}
		}

		// Token: 0x0600333A RID: 13114 RVA: 0x000EE244 File Offset: 0x000EC444
		private void UserPreferenceChangedHandler(object o, UserPreferenceChangedEventArgs e)
		{
			if (base.IsHandleCreated)
			{
				if (this.BackColor.IsSystemColor)
				{
					base.SendMessage(1091, 0, ColorTranslator.ToWin32(this.BackColor));
				}
				if (this.ForeColor.IsSystemColor)
				{
					this.InternalSetForeColor(this.ForeColor);
				}
			}
		}

		/// <summary>Creates an <see langword="IRichEditOleCallback" />-compatible object for handling rich-edit callback operations.</summary>
		/// <returns>An object that implements the <see langword="IRichEditOleCallback" /> interface.</returns>
		// Token: 0x0600333B RID: 13115 RVA: 0x000EE29E File Offset: 0x000EC49E
		[SecurityPermission(SecurityAction.InheritanceDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		protected virtual object CreateRichEditOleCallback()
		{
			return new RichTextBox.OleCallback(this);
		}

		// Token: 0x0600333C RID: 13116 RVA: 0x000EE2A8 File Offset: 0x000EC4A8
		private void EnLinkMsgHandler(ref Message m)
		{
			NativeMethods.ENLINK enlink;
			if (IntPtr.Size == 8)
			{
				enlink = RichTextBox.ConvertFromENLINK64((NativeMethods.ENLINK64)m.GetLParam(typeof(NativeMethods.ENLINK64)));
			}
			else
			{
				enlink = (NativeMethods.ENLINK)m.GetLParam(typeof(NativeMethods.ENLINK));
			}
			int msg = enlink.msg;
			if (msg == 32)
			{
				this.LinkCursor = true;
				m.Result = (IntPtr)1;
				return;
			}
			if (msg != 513)
			{
				m.Result = IntPtr.Zero;
				return;
			}
			string text = this.CharRangeToString(enlink.charrange);
			if (!string.IsNullOrEmpty(text))
			{
				this.OnLinkClicked(new LinkClickedEventArgs(text));
			}
			m.Result = (IntPtr)1;
		}

		// Token: 0x0600333D RID: 13117 RVA: 0x000EE354 File Offset: 0x000EC554
		private string CharRangeToString(NativeMethods.CHARRANGE c)
		{
			NativeMethods.TEXTRANGE textrange = new NativeMethods.TEXTRANGE();
			textrange.chrg = c;
			if (c.cpMax > this.Text.Length || c.cpMax - c.cpMin <= 0)
			{
				return string.Empty;
			}
			int size = c.cpMax - c.cpMin + 1;
			UnsafeNativeMethods.CharBuffer charBuffer = UnsafeNativeMethods.CharBuffer.CreateBuffer(size);
			IntPtr intPtr = charBuffer.AllocCoTaskMem();
			if (intPtr == IntPtr.Zero)
			{
				throw new OutOfMemoryException(SR.GetString("OutOfMemory"));
			}
			textrange.lpstrText = intPtr;
			int num = (int)UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), 1099, 0, textrange);
			charBuffer.PutCoTaskMem(intPtr);
			if (textrange.lpstrText != IntPtr.Zero)
			{
				Marshal.FreeCoTaskMem(intPtr);
			}
			return charBuffer.GetString();
		}

		// Token: 0x0600333E RID: 13118 RVA: 0x000EE421 File Offset: 0x000EC621
		internal override void UpdateMaxLength()
		{
			if (base.IsHandleCreated)
			{
				base.SendMessage(1077, 0, this.MaxLength);
			}
		}

		// Token: 0x0600333F RID: 13119 RVA: 0x000EE440 File Offset: 0x000EC640
		private void WmReflectCommand(ref Message m)
		{
			if (!(m.LParam == base.Handle) || base.GetState(262144))
			{
				base.WndProc(ref m);
				return;
			}
			int num = NativeMethods.Util.HIWORD(m.WParam);
			if (num == 1537)
			{
				this.OnHScroll(EventArgs.Empty);
				return;
			}
			if (num != 1538)
			{
				base.WndProc(ref m);
				return;
			}
			this.OnVScroll(EventArgs.Empty);
		}

		// Token: 0x06003340 RID: 13120 RVA: 0x000EE4B4 File Offset: 0x000EC6B4
		internal void WmReflectNotify(ref Message m)
		{
			if (m.HWnd == base.Handle)
			{
				NativeMethods.NMHDR nmhdr = (NativeMethods.NMHDR)m.GetLParam(typeof(NativeMethods.NMHDR));
				int num = nmhdr.code;
				switch (num)
				{
				case 1793:
					if (!this.CallOnContentsResized)
					{
						NativeMethods.REQRESIZE reqresize = (NativeMethods.REQRESIZE)m.GetLParam(typeof(NativeMethods.REQRESIZE));
						if (base.BorderStyle == BorderStyle.Fixed3D)
						{
							NativeMethods.REQRESIZE reqresize2 = reqresize;
							reqresize2.rc.bottom = reqresize2.rc.bottom + 1;
						}
						this.OnContentsResized(new ContentsResizedEventArgs(Rectangle.FromLTRB(reqresize.rc.left, reqresize.rc.top, reqresize.rc.right, reqresize.rc.bottom)));
						return;
					}
					break;
				case 1794:
				{
					NativeMethods.SELCHANGE selChange = (NativeMethods.SELCHANGE)m.GetLParam(typeof(NativeMethods.SELCHANGE));
					this.WmSelectionChange(selChange);
					return;
				}
				case 1795:
				{
					NativeMethods.ENDROPFILES endropfiles = (NativeMethods.ENDROPFILES)m.GetLParam(typeof(NativeMethods.ENDROPFILES));
					StringBuilder stringBuilder = new StringBuilder(260);
					if (UnsafeNativeMethods.DragQueryFileLongPath(new HandleRef(endropfiles, endropfiles.hDrop), 0, stringBuilder) != 0)
					{
						try
						{
							this.LoadFile(stringBuilder.ToString(), RichTextBoxStreamType.RichText);
						}
						catch
						{
							try
							{
								this.LoadFile(stringBuilder.ToString(), RichTextBoxStreamType.PlainText);
							}
							catch
							{
							}
						}
					}
					m.Result = (IntPtr)1;
					return;
				}
				case 1796:
				{
					NativeMethods.ENPROTECTED enprotected;
					if (IntPtr.Size == 8)
					{
						enprotected = this.ConvertFromENPROTECTED64((NativeMethods.ENPROTECTED64)m.GetLParam(typeof(NativeMethods.ENPROTECTED64)));
					}
					else
					{
						enprotected = (NativeMethods.ENPROTECTED)m.GetLParam(typeof(NativeMethods.ENPROTECTED));
					}
					num = enprotected.msg;
					if (num <= 769)
					{
						if (num != 12)
						{
							if (num == 194)
							{
								goto IL_26C;
							}
							if (num != 769)
							{
								goto IL_265;
							}
						}
					}
					else if (num <= 1092)
					{
						if (num != 1077)
						{
							if (num != 1092)
							{
								goto IL_265;
							}
							NativeMethods.CHARFORMATA charformata = (NativeMethods.CHARFORMATA)UnsafeNativeMethods.PtrToStructure(enprotected.lParam, typeof(NativeMethods.CHARFORMATA));
							if ((charformata.dwMask & 16) != 0)
							{
								m.Result = IntPtr.Zero;
								return;
							}
							goto IL_26C;
						}
					}
					else
					{
						if (num == 1095)
						{
							goto IL_26C;
						}
						if (num != 1097)
						{
							goto IL_265;
						}
						if (((int)((long)enprotected.wParam) & 32768) == 0)
						{
							m.Result = IntPtr.Zero;
							return;
						}
						goto IL_26C;
					}
					m.Result = IntPtr.Zero;
					return;
					IL_265:
					SafeNativeMethods.MessageBeep(0);
					IL_26C:
					this.OnProtected(EventArgs.Empty);
					m.Result = (IntPtr)1;
					return;
				}
				default:
					if (num == 1803)
					{
						this.EnLinkMsgHandler(ref m);
						return;
					}
					base.WndProc(ref m);
					return;
				}
			}
			else
			{
				base.WndProc(ref m);
			}
		}

		// Token: 0x06003341 RID: 13121 RVA: 0x000EE770 File Offset: 0x000EC970
		private unsafe NativeMethods.ENPROTECTED ConvertFromENPROTECTED64(NativeMethods.ENPROTECTED64 es64)
		{
			NativeMethods.ENPROTECTED enprotected = new NativeMethods.ENPROTECTED();
			fixed (byte* ptr = &es64.contents[0])
			{
				enprotected.nmhdr = default(NativeMethods.NMHDR);
				enprotected.chrg = new NativeMethods.CHARRANGE();
				enprotected.nmhdr.hwndFrom = Marshal.ReadIntPtr((IntPtr)((void*)ptr));
				enprotected.nmhdr.idFrom = Marshal.ReadIntPtr((IntPtr)((void*)((byte*)ptr + 8)));
				enprotected.nmhdr.code = Marshal.ReadInt32((IntPtr)((void*)((byte*)ptr + 16)));
				enprotected.msg = Marshal.ReadInt32((IntPtr)((void*)((byte*)ptr + 24)));
				enprotected.wParam = Marshal.ReadIntPtr((IntPtr)((void*)((byte*)ptr + 28)));
				enprotected.lParam = Marshal.ReadIntPtr((IntPtr)((void*)((byte*)ptr + 36)));
				enprotected.chrg.cpMin = Marshal.ReadInt32((IntPtr)((void*)((byte*)ptr + 44)));
				enprotected.chrg.cpMax = Marshal.ReadInt32((IntPtr)((void*)((byte*)ptr + 48)));
			}
			return enprotected;
		}

		// Token: 0x06003342 RID: 13122 RVA: 0x000EE868 File Offset: 0x000ECA68
		private unsafe static NativeMethods.ENLINK ConvertFromENLINK64(NativeMethods.ENLINK64 es64)
		{
			NativeMethods.ENLINK enlink = new NativeMethods.ENLINK();
			fixed (byte* ptr = &es64.contents[0])
			{
				enlink.nmhdr = default(NativeMethods.NMHDR);
				enlink.charrange = new NativeMethods.CHARRANGE();
				enlink.nmhdr.hwndFrom = Marshal.ReadIntPtr((IntPtr)((void*)ptr));
				enlink.nmhdr.idFrom = Marshal.ReadIntPtr((IntPtr)((void*)((byte*)ptr + 8)));
				enlink.nmhdr.code = Marshal.ReadInt32((IntPtr)((void*)((byte*)ptr + 16)));
				enlink.msg = Marshal.ReadInt32((IntPtr)((void*)((byte*)ptr + 24)));
				enlink.wParam = Marshal.ReadIntPtr((IntPtr)((void*)((byte*)ptr + 28)));
				enlink.lParam = Marshal.ReadIntPtr((IntPtr)((void*)((byte*)ptr + 36)));
				enlink.charrange.cpMin = Marshal.ReadInt32((IntPtr)((void*)((byte*)ptr + 44)));
				enlink.charrange.cpMax = Marshal.ReadInt32((IntPtr)((void*)((byte*)ptr + 48)));
			}
			return enlink;
		}

		// Token: 0x06003343 RID: 13123 RVA: 0x000EE960 File Offset: 0x000ECB60
		private void WmSelectionChange(NativeMethods.SELCHANGE selChange)
		{
			int cpMin = selChange.chrg.cpMin;
			int cpMax = selChange.chrg.cpMax;
			short num = (short)selChange.seltyp;
			if (base.ImeMode == ImeMode.Hangul || base.ImeMode == ImeMode.HangulFull)
			{
				int num2 = (int)((long)base.SendMessage(1146, 0, 0));
				if (num2 != 0)
				{
					int windowTextLength = SafeNativeMethods.GetWindowTextLength(new HandleRef(this, base.Handle));
					if (cpMin == cpMax && windowTextLength == this.MaxLength)
					{
						base.SendMessage(8, 0, 0);
						base.SendMessage(7, 0, 0);
						UnsafeNativeMethods.PostMessage(new HandleRef(this, base.Handle), 177, cpMax - 1, cpMax);
					}
				}
			}
			if (cpMin != this.curSelStart || cpMax != this.curSelEnd || num != this.curSelType)
			{
				this.curSelStart = cpMin;
				this.curSelEnd = cpMax;
				this.curSelType = num;
				this.OnSelectionChanged(EventArgs.Empty);
			}
		}

		// Token: 0x06003344 RID: 13124 RVA: 0x000EEA44 File Offset: 0x000ECC44
		private void WmSetFont(ref Message m)
		{
			try
			{
				this.SuppressTextChangedEvent = true;
				base.WndProc(ref m);
			}
			finally
			{
				this.SuppressTextChangedEvent = false;
			}
			this.InternalSetForeColor(this.ForeColor);
		}

		/// <summary>Processes Windows messages.</summary>
		/// <param name="m">A Windows Message object.</param>
		// Token: 0x06003345 RID: 13125 RVA: 0x000EEA88 File Offset: 0x000ECC88
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		protected override void WndProc(ref Message m)
		{
			int msg = m.Msg;
			if (msg <= 276)
			{
				if (msg <= 48)
				{
					if (msg != 32)
					{
						if (msg == 48)
						{
							this.WmSetFont(ref m);
							return;
						}
					}
					else
					{
						this.LinkCursor = false;
						this.DefWndProc(ref m);
						if (this.LinkCursor && !this.Cursor.Equals(Cursors.WaitCursor))
						{
							UnsafeNativeMethods.SetCursor(new HandleRef(Cursors.Hand, Cursors.Hand.Handle));
							m.Result = (IntPtr)1;
							return;
						}
						base.WndProc(ref m);
						return;
					}
				}
				else if (msg != 61)
				{
					if (msg == 135)
					{
						base.WndProc(ref m);
						m.Result = (IntPtr)(base.AcceptsTab ? ((int)((long)m.Result) | 2) : ((int)((long)m.Result) & -3));
						return;
					}
					if (msg == 276)
					{
						base.WndProc(ref m);
						int num = NativeMethods.Util.LOWORD(m.WParam);
						if (num == 5)
						{
							this.OnHScroll(EventArgs.Empty);
						}
						if (num == 4)
						{
							this.OnHScroll(EventArgs.Empty);
							return;
						}
						return;
					}
				}
				else
				{
					base.WndProc(ref m);
					if ((int)((long)m.LParam) == -12)
					{
						m.Result = (IntPtr)((Marshal.SystemDefaultCharSize == 1) ? 65565 : 65566);
						return;
					}
					return;
				}
			}
			else if (msg <= 517)
			{
				if (msg != 277)
				{
					if (msg == 517)
					{
						bool style = base.GetStyle(ControlStyles.UserMouse);
						base.SetStyle(ControlStyles.UserMouse, true);
						base.WndProc(ref m);
						base.SetStyle(ControlStyles.UserMouse, style);
						return;
					}
				}
				else
				{
					base.WndProc(ref m);
					int num = NativeMethods.Util.LOWORD(m.WParam);
					if (num == 5)
					{
						this.OnVScroll(EventArgs.Empty);
						return;
					}
					if (num == 4)
					{
						this.OnVScroll(EventArgs.Empty);
						return;
					}
					return;
				}
			}
			else
			{
				if (msg == 642)
				{
					this.OnImeChange(EventArgs.Empty);
					base.WndProc(ref m);
					return;
				}
				if (msg == 8270)
				{
					this.WmReflectNotify(ref m);
					return;
				}
				if (msg == 8465)
				{
					this.WmReflectCommand(ref m);
					return;
				}
			}
			base.WndProc(ref m);
		}

		// Token: 0x04001E50 RID: 7760
		private static TraceSwitch richTextDbg;

		// Token: 0x04001E51 RID: 7761
		private const int DV_E_DVASPECT = -2147221397;

		// Token: 0x04001E52 RID: 7762
		private const int DVASPECT_CONTENT = 1;

		// Token: 0x04001E53 RID: 7763
		private const int DVASPECT_THUMBNAIL = 2;

		// Token: 0x04001E54 RID: 7764
		private const int DVASPECT_ICON = 4;

		// Token: 0x04001E55 RID: 7765
		private const int DVASPECT_DOCPRINT = 8;

		// Token: 0x04001E56 RID: 7766
		internal const int INPUT = 1;

		// Token: 0x04001E57 RID: 7767
		internal const int OUTPUT = 2;

		// Token: 0x04001E58 RID: 7768
		internal const int DIRECTIONMASK = 3;

		// Token: 0x04001E59 RID: 7769
		internal const int ANSI = 4;

		// Token: 0x04001E5A RID: 7770
		internal const int UNICODE = 8;

		// Token: 0x04001E5B RID: 7771
		internal const int FORMATMASK = 12;

		// Token: 0x04001E5C RID: 7772
		internal const int TEXTLF = 16;

		// Token: 0x04001E5D RID: 7773
		internal const int TEXTCRLF = 32;

		// Token: 0x04001E5E RID: 7774
		internal const int RTF = 64;

		// Token: 0x04001E5F RID: 7775
		internal const int KINDMASK = 112;

		// Token: 0x04001E60 RID: 7776
		private static IntPtr moduleHandle;

		// Token: 0x04001E61 RID: 7777
		private static readonly string SZ_RTF_TAG = "{\\rtf";

		// Token: 0x04001E62 RID: 7778
		private const int CHAR_BUFFER_LEN = 512;

		// Token: 0x04001E63 RID: 7779
		private static readonly object EVENT_HSCROLL = new object();

		// Token: 0x04001E64 RID: 7780
		private static readonly object EVENT_LINKACTIVATE = new object();

		// Token: 0x04001E65 RID: 7781
		private static readonly object EVENT_IMECHANGE = new object();

		// Token: 0x04001E66 RID: 7782
		private static readonly object EVENT_PROTECTED = new object();

		// Token: 0x04001E67 RID: 7783
		private static readonly object EVENT_REQUESTRESIZE = new object();

		// Token: 0x04001E68 RID: 7784
		private static readonly object EVENT_SELCHANGE = new object();

		// Token: 0x04001E69 RID: 7785
		private static readonly object EVENT_VSCROLL = new object();

		// Token: 0x04001E6A RID: 7786
		private int bulletIndent;

		// Token: 0x04001E6B RID: 7787
		private int rightMargin;

		// Token: 0x04001E6C RID: 7788
		private string textRtf;

		// Token: 0x04001E6D RID: 7789
		private string textPlain;

		// Token: 0x04001E6E RID: 7790
		private Color selectionBackColorToSetOnHandleCreated;

		// Token: 0x04001E6F RID: 7791
		private RichTextBoxLanguageOptions languageOption = RichTextBoxLanguageOptions.AutoFont | RichTextBoxLanguageOptions.DualFont;

		// Token: 0x04001E70 RID: 7792
		private static int logPixelsX;

		// Token: 0x04001E71 RID: 7793
		private static int logPixelsY;

		// Token: 0x04001E72 RID: 7794
		private Stream editStream;

		// Token: 0x04001E73 RID: 7795
		private float zoomMultiplier = 1f;

		// Token: 0x04001E74 RID: 7796
		private int curSelStart;

		// Token: 0x04001E75 RID: 7797
		private int curSelEnd;

		// Token: 0x04001E76 RID: 7798
		private short curSelType;

		// Token: 0x04001E77 RID: 7799
		private object oleCallback;

		// Token: 0x04001E78 RID: 7800
		private static int[] shortcutsToDisable;

		// Token: 0x04001E79 RID: 7801
		private static int richEditMajorVersion = 3;

		// Token: 0x04001E7A RID: 7802
		private BitVector32 richTextBoxFlags;

		// Token: 0x04001E7B RID: 7803
		private static readonly BitVector32.Section autoWordSelectionSection = BitVector32.CreateSection(1);

		// Token: 0x04001E7C RID: 7804
		private static readonly BitVector32.Section showSelBarSection = BitVector32.CreateSection(1, RichTextBox.autoWordSelectionSection);

		// Token: 0x04001E7D RID: 7805
		private static readonly BitVector32.Section autoUrlDetectSection = BitVector32.CreateSection(1, RichTextBox.showSelBarSection);

		// Token: 0x04001E7E RID: 7806
		private static readonly BitVector32.Section fInCtorSection = BitVector32.CreateSection(1, RichTextBox.autoUrlDetectSection);

		// Token: 0x04001E7F RID: 7807
		private static readonly BitVector32.Section protectedErrorSection = BitVector32.CreateSection(1, RichTextBox.fInCtorSection);

		// Token: 0x04001E80 RID: 7808
		private static readonly BitVector32.Section linkcursorSection = BitVector32.CreateSection(1, RichTextBox.protectedErrorSection);

		// Token: 0x04001E81 RID: 7809
		private static readonly BitVector32.Section allowOleDropSection = BitVector32.CreateSection(1, RichTextBox.linkcursorSection);

		// Token: 0x04001E82 RID: 7810
		private static readonly BitVector32.Section suppressTextChangedEventSection = BitVector32.CreateSection(1, RichTextBox.allowOleDropSection);

		// Token: 0x04001E83 RID: 7811
		private static readonly BitVector32.Section callOnContentsResizedSection = BitVector32.CreateSection(1, RichTextBox.suppressTextChangedEventSection);

		// Token: 0x04001E84 RID: 7812
		private static readonly BitVector32.Section richTextShortcutsEnabledSection = BitVector32.CreateSection(1, RichTextBox.callOnContentsResizedSection);

		// Token: 0x04001E85 RID: 7813
		private static readonly BitVector32.Section allowOleObjectsSection = BitVector32.CreateSection(1, RichTextBox.richTextShortcutsEnabledSection);

		// Token: 0x04001E86 RID: 7814
		private static readonly BitVector32.Section scrollBarsSection = BitVector32.CreateSection(19, RichTextBox.allowOleObjectsSection);

		// Token: 0x04001E87 RID: 7815
		private static readonly BitVector32.Section enableAutoDragDropSection = BitVector32.CreateSection(1, RichTextBox.scrollBarsSection);

		// Token: 0x0200070F RID: 1807
		private class OleCallback : UnsafeNativeMethods.IRichEditOleCallback
		{
			// Token: 0x06005FDD RID: 24541 RVA: 0x00189837 File Offset: 0x00187A37
			internal OleCallback(RichTextBox owner)
			{
				this.owner = owner;
			}

			// Token: 0x06005FDE RID: 24542 RVA: 0x00189848 File Offset: 0x00187A48
			public int GetNewStorage(out UnsafeNativeMethods.IStorage storage)
			{
				if (!this.owner.AllowOleObjects)
				{
					storage = null;
					return -2147467259;
				}
				UnsafeNativeMethods.ILockBytes iLockBytes = UnsafeNativeMethods.CreateILockBytesOnHGlobal(NativeMethods.NullHandleRef, true);
				storage = UnsafeNativeMethods.StgCreateDocfileOnILockBytes(iLockBytes, 4114, 0);
				return 0;
			}

			// Token: 0x06005FDF RID: 24543 RVA: 0x00033B0C File Offset: 0x00031D0C
			public int GetInPlaceContext(IntPtr lplpFrame, IntPtr lplpDoc, IntPtr lpFrameInfo)
			{
				return -2147467263;
			}

			// Token: 0x06005FE0 RID: 24544 RVA: 0x0000E0A4 File Offset: 0x0000C2A4
			public int ShowContainerUI(int fShow)
			{
				return 0;
			}

			// Token: 0x06005FE1 RID: 24545 RVA: 0x00189888 File Offset: 0x00187A88
			public int QueryInsertObject(ref Guid lpclsid, IntPtr lpstg, int cp)
			{
				try
				{
					IntSecurity.UnmanagedCode.Demand();
					return 0;
				}
				catch (SecurityException)
				{
				}
				Guid a = default(Guid);
				int hr = UnsafeNativeMethods.ReadClassStg(new HandleRef(null, lpstg), ref a);
				if (!NativeMethods.Succeeded(hr))
				{
					return 1;
				}
				if (a == Guid.Empty)
				{
					a = lpclsid;
				}
				string a2 = a.ToString().ToUpper(CultureInfo.InvariantCulture);
				if (a2 == "00000315-0000-0000-C000-000000000046" || a2 == "00000316-0000-0000-C000-000000000046" || a2 == "00000319-0000-0000-C000-000000000046" || a2 == "0003000A-0000-0000-C000-000000000046")
				{
					return 0;
				}
				return 1;
			}

			// Token: 0x06005FE2 RID: 24546 RVA: 0x0000E0A4 File Offset: 0x0000C2A4
			public int DeleteObject(IntPtr lpoleobj)
			{
				return 0;
			}

			// Token: 0x06005FE3 RID: 24547 RVA: 0x00189940 File Offset: 0x00187B40
			public int QueryAcceptData(IDataObject lpdataobj, IntPtr lpcfFormat, int reco, int fReally, IntPtr hMetaPict)
			{
				if (reco != 1)
				{
					return -2147467263;
				}
				if (!this.owner.AllowDrop && !this.owner.EnableAutoDragDrop)
				{
					this.lastDataObject = null;
					return -2147467259;
				}
				MouseButtons mouseButtons = Control.MouseButtons;
				Keys modifierKeys = Control.ModifierKeys;
				int num = 0;
				if ((mouseButtons & MouseButtons.Left) == MouseButtons.Left)
				{
					num |= 1;
				}
				if ((mouseButtons & MouseButtons.Right) == MouseButtons.Right)
				{
					num |= 2;
				}
				if ((mouseButtons & MouseButtons.Middle) == MouseButtons.Middle)
				{
					num |= 16;
				}
				if ((modifierKeys & Keys.Control) == Keys.Control)
				{
					num |= 8;
				}
				if ((modifierKeys & Keys.Shift) == Keys.Shift)
				{
					num |= 4;
				}
				this.lastDataObject = new DataObject(lpdataobj);
				if (!this.owner.EnableAutoDragDrop)
				{
					this.lastEffect = DragDropEffects.None;
				}
				DragEventArgs dragEventArgs = new DragEventArgs(this.lastDataObject, num, Control.MousePosition.X, Control.MousePosition.Y, DragDropEffects.All, this.lastEffect);
				if (fReally == 0)
				{
					dragEventArgs.Effect = (((num & 8) == 8) ? DragDropEffects.Copy : DragDropEffects.Move);
					this.owner.OnDragEnter(dragEventArgs);
				}
				else
				{
					this.owner.OnDragDrop(dragEventArgs);
					this.lastDataObject = null;
				}
				this.lastEffect = dragEventArgs.Effect;
				if (dragEventArgs.Effect == DragDropEffects.None)
				{
					return -2147467259;
				}
				return 0;
			}

			// Token: 0x06005FE4 RID: 24548 RVA: 0x00033B0C File Offset: 0x00031D0C
			public int ContextSensitiveHelp(int fEnterMode)
			{
				return -2147467263;
			}

			// Token: 0x06005FE5 RID: 24549 RVA: 0x00033B0C File Offset: 0x00031D0C
			public int GetClipboardData(NativeMethods.CHARRANGE lpchrg, int reco, IntPtr lplpdataobj)
			{
				return -2147467263;
			}

			// Token: 0x06005FE6 RID: 24550 RVA: 0x00189A94 File Offset: 0x00187C94
			public int GetDragDropEffect(bool fDrag, int grfKeyState, ref int pdwEffect)
			{
				if (this.owner.AllowDrop || this.owner.EnableAutoDragDrop)
				{
					if (fDrag && grfKeyState == 0)
					{
						if (this.owner.EnableAutoDragDrop)
						{
							this.lastEffect = DragDropEffects.All;
						}
						else
						{
							this.lastEffect = DragDropEffects.None;
						}
					}
					else if (!fDrag && this.lastDataObject != null && grfKeyState != 0)
					{
						DragEventArgs dragEventArgs = new DragEventArgs(this.lastDataObject, grfKeyState, Control.MousePosition.X, Control.MousePosition.Y, DragDropEffects.All, this.lastEffect);
						if (this.lastEffect != DragDropEffects.None)
						{
							dragEventArgs.Effect = (((grfKeyState & 8) == 8) ? DragDropEffects.Copy : DragDropEffects.Move);
						}
						this.owner.OnDragOver(dragEventArgs);
						this.lastEffect = dragEventArgs.Effect;
					}
					pdwEffect = (int)this.lastEffect;
				}
				else
				{
					pdwEffect = 0;
				}
				return 0;
			}

			// Token: 0x06005FE7 RID: 24551 RVA: 0x00189B68 File Offset: 0x00187D68
			public int GetContextMenu(short seltype, IntPtr lpoleobj, NativeMethods.CHARRANGE lpchrg, out IntPtr hmenu)
			{
				ContextMenu contextMenu = this.owner.ContextMenu;
				if (contextMenu == null || !this.owner.ShortcutsEnabled)
				{
					hmenu = IntPtr.Zero;
				}
				else
				{
					contextMenu.sourceControl = this.owner;
					contextMenu.OnPopup(EventArgs.Empty);
					IntPtr handle = contextMenu.Handle;
					Menu menu = contextMenu;
					for (;;)
					{
						int i = 0;
						int itemCount = menu.ItemCount;
						while (i < itemCount)
						{
							if (menu.items[i].handle != IntPtr.Zero)
							{
								menu = menu.items[i];
								break;
							}
							i++;
						}
						if (i == itemCount)
						{
							menu.handle = IntPtr.Zero;
							menu.created = false;
							if (menu == contextMenu)
							{
								break;
							}
							menu = ((MenuItem)menu).Menu;
						}
					}
					hmenu = handle;
				}
				return 0;
			}

			// Token: 0x04004131 RID: 16689
			private RichTextBox owner;

			// Token: 0x04004132 RID: 16690
			private IDataObject lastDataObject;

			// Token: 0x04004133 RID: 16691
			private DragDropEffects lastEffect;
		}
	}
}
