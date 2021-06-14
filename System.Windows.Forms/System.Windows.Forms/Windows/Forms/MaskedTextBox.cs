using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Globalization;
using System.Media;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;
using System.Windows.Forms.VisualStyles;

namespace System.Windows.Forms
{
	/// <summary>Uses a mask to distinguish between proper and improper user input.</summary>
	// Token: 0x020002D6 RID: 726
	[ComVisible(true)]
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[DefaultEvent("MaskInputRejected")]
	[DefaultBindingProperty("Text")]
	[DefaultProperty("Mask")]
	[Designer("System.Windows.Forms.Design.MaskedTextBoxDesigner, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	[SRDescription("DescriptionMaskedTextBox")]
	public class MaskedTextBox : TextBoxBase
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.MaskedTextBox" /> class using defaults.</summary>
		// Token: 0x06002B76 RID: 11126 RVA: 0x000CB16C File Offset: 0x000C936C
		public MaskedTextBox()
		{
			MaskedTextProvider maskedTextProvider = new MaskedTextProvider("<>", CultureInfo.CurrentCulture);
			this.flagState[MaskedTextBox.IS_NULL_MASK] = true;
			this.Initialize(maskedTextProvider);
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.MaskedTextBox" /> class using the specified input mask.</summary>
		/// <param name="mask">A <see cref="T:System.String" /> representing the input mask. The initial value of the <see cref="P:System.Windows.Forms.MaskedTextBox.Mask" /> property.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="mask" /> is <see langword="null" />.</exception>
		// Token: 0x06002B77 RID: 11127 RVA: 0x000CB1A8 File Offset: 0x000C93A8
		public MaskedTextBox(string mask)
		{
			if (mask == null)
			{
				throw new ArgumentNullException();
			}
			MaskedTextProvider maskedTextProvider = new MaskedTextProvider(mask, CultureInfo.CurrentCulture);
			this.flagState[MaskedTextBox.IS_NULL_MASK] = false;
			this.Initialize(maskedTextProvider);
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.MaskedTextBox" /> class using the specified custom mask language provider.</summary>
		/// <param name="maskedTextProvider">A custom mask language provider, derived from the <see cref="T:System.ComponentModel.MaskedTextProvider" /> class. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="maskedTextProvider" /> is <see langword="null" />.</exception>
		// Token: 0x06002B78 RID: 11128 RVA: 0x000CB1E8 File Offset: 0x000C93E8
		public MaskedTextBox(MaskedTextProvider maskedTextProvider)
		{
			if (maskedTextProvider == null)
			{
				throw new ArgumentNullException();
			}
			this.flagState[MaskedTextBox.IS_NULL_MASK] = false;
			this.Initialize(maskedTextProvider);
		}

		// Token: 0x06002B79 RID: 11129 RVA: 0x000CB214 File Offset: 0x000C9414
		private void Initialize(MaskedTextProvider maskedTextProvider)
		{
			this.maskedTextProvider = maskedTextProvider;
			if (!this.flagState[MaskedTextBox.IS_NULL_MASK])
			{
				this.SetWindowText();
			}
			this.passwordChar = this.maskedTextProvider.PasswordChar;
			this.insertMode = InsertKeyMode.Default;
			this.flagState[MaskedTextBox.HIDE_PROMPT_ON_LEAVE] = false;
			this.flagState[MaskedTextBox.BEEP_ON_ERROR] = false;
			this.flagState[MaskedTextBox.USE_SYSTEM_PASSWORD_CHAR] = false;
			this.flagState[MaskedTextBox.REJECT_INPUT_ON_FIRST_FAILURE] = false;
			this.flagState[MaskedTextBox.CUTCOPYINCLUDEPROMPT] = this.maskedTextProvider.IncludePrompt;
			this.flagState[MaskedTextBox.CUTCOPYINCLUDELITERALS] = this.maskedTextProvider.IncludeLiterals;
			this.flagState[MaskedTextBox.HANDLE_KEY_PRESS] = true;
			this.caretTestPos = 0;
		}

		/// <summary>Gets or sets a value determining how TAB keys are handled for multiline configurations. This property is not supported by <see cref="T:System.Windows.Forms.MaskedTextBox" />. </summary>
		/// <returns>
		///     <see langword="false" /> in all cases.</returns>
		// Token: 0x17000A86 RID: 2694
		// (get) Token: 0x06002B7A RID: 11130 RVA: 0x0000E0A4 File Offset: 0x0000C2A4
		// (set) Token: 0x06002B7B RID: 11131 RVA: 0x0000701A File Offset: 0x0000521A
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new bool AcceptsTab
		{
			get
			{
				return false;
			}
			set
			{
			}
		}

		/// <summary>Gets or sets a value indicating whether <see cref="P:System.Windows.Forms.MaskedTextBox.PromptChar" /> can be entered as valid data by the user. </summary>
		/// <returns>
		///     <see langword="true" /> if the user can enter the prompt character into the control; otherwise, <see langword="false" />. The default is <see langword="true" />. </returns>
		// Token: 0x17000A87 RID: 2695
		// (get) Token: 0x06002B7C RID: 11132 RVA: 0x000CB2EA File Offset: 0x000C94EA
		// (set) Token: 0x06002B7D RID: 11133 RVA: 0x000CB2F8 File Offset: 0x000C94F8
		[SRCategory("CatBehavior")]
		[SRDescription("MaskedTextBoxAllowPromptAsInputDescr")]
		[DefaultValue(true)]
		public bool AllowPromptAsInput
		{
			get
			{
				return this.maskedTextProvider.AllowPromptAsInput;
			}
			set
			{
				if (value != this.maskedTextProvider.AllowPromptAsInput)
				{
					MaskedTextProvider maskedTextProvider = new MaskedTextProvider(this.maskedTextProvider.Mask, this.maskedTextProvider.Culture, value, this.maskedTextProvider.PromptChar, this.maskedTextProvider.PasswordChar, this.maskedTextProvider.AsciiOnly);
					this.SetMaskedTextProvider(maskedTextProvider);
				}
			}
		}

		/// <summary>Occurs when the value of the <see cref="P:System.Windows.Forms.MaskedTextBox.AcceptsTab" /> property has changed. This event is not raised by <see cref="T:System.Windows.Forms.MaskedTextBox" />.</summary>
		// Token: 0x14000207 RID: 519
		// (add) Token: 0x06002B7E RID: 11134 RVA: 0x0000701A File Offset: 0x0000521A
		// (remove) Token: 0x06002B7F RID: 11135 RVA: 0x0000701A File Offset: 0x0000521A
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new event EventHandler AcceptsTabChanged
		{
			add
			{
			}
			remove
			{
			}
		}

		/// <summary>Gets or sets a value indicating whether the <see cref="T:System.Windows.Forms.MaskedTextBox" /> control accepts characters outside of the ASCII character set.</summary>
		/// <returns>
		///     <see langword="true" /> if only ASCII is accepted; <see langword="false" /> if the <see cref="T:System.Windows.Forms.MaskedTextBox" /> control can accept any arbitrary Unicode character. The default is <see langword="false" />.</returns>
		// Token: 0x17000A88 RID: 2696
		// (get) Token: 0x06002B80 RID: 11136 RVA: 0x000CB358 File Offset: 0x000C9558
		// (set) Token: 0x06002B81 RID: 11137 RVA: 0x000CB368 File Offset: 0x000C9568
		[SRCategory("CatBehavior")]
		[SRDescription("MaskedTextBoxAsciiOnlyDescr")]
		[RefreshProperties(RefreshProperties.Repaint)]
		[DefaultValue(false)]
		public bool AsciiOnly
		{
			get
			{
				return this.maskedTextProvider.AsciiOnly;
			}
			set
			{
				if (value != this.maskedTextProvider.AsciiOnly)
				{
					MaskedTextProvider maskedTextProvider = new MaskedTextProvider(this.maskedTextProvider.Mask, this.maskedTextProvider.Culture, this.maskedTextProvider.AllowPromptAsInput, this.maskedTextProvider.PromptChar, this.maskedTextProvider.PasswordChar, value);
					this.SetMaskedTextProvider(maskedTextProvider);
				}
			}
		}

		/// <summary>Gets or sets a value indicating whether the masked text box control raises the system beep for each user key stroke that it rejects.</summary>
		/// <returns>
		///     <see langword="true" /> if the <see cref="T:System.Windows.Forms.MaskedTextBox" /> control should beep on invalid input; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
		// Token: 0x17000A89 RID: 2697
		// (get) Token: 0x06002B82 RID: 11138 RVA: 0x000CB3C8 File Offset: 0x000C95C8
		// (set) Token: 0x06002B83 RID: 11139 RVA: 0x000CB3DA File Offset: 0x000C95DA
		[SRCategory("CatBehavior")]
		[SRDescription("MaskedTextBoxBeepOnErrorDescr")]
		[DefaultValue(false)]
		public bool BeepOnError
		{
			get
			{
				return this.flagState[MaskedTextBox.BEEP_ON_ERROR];
			}
			set
			{
				this.flagState[MaskedTextBox.BEEP_ON_ERROR] = value;
			}
		}

		/// <summary>Gets a value indicating whether the user can undo the previous operation. This property is not supported by <see cref="T:System.Windows.Forms.MaskedTextBox" />. </summary>
		/// <returns>
		///     <see langword="false" /> in all cases. </returns>
		// Token: 0x17000A8A RID: 2698
		// (get) Token: 0x06002B84 RID: 11140 RVA: 0x0000E0A4 File Offset: 0x0000C2A4
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new bool CanUndo
		{
			get
			{
				return false;
			}
		}

		/// <summary>Gets the required creation parameters when the control handle is created.</summary>
		/// <returns>A <see cref="T:System.Windows.Forms.CreateParams" /> representing the information needed when creating a control.</returns>
		// Token: 0x17000A8B RID: 2699
		// (get) Token: 0x06002B85 RID: 11141 RVA: 0x000CB3F0 File Offset: 0x000C95F0
		protected override CreateParams CreateParams
		{
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			get
			{
				CreateParams createParams = base.CreateParams;
				HorizontalAlignment horizontalAlignment = base.RtlTranslateHorizontal(this.textAlign);
				createParams.ExStyle &= -4097;
				switch (horizontalAlignment)
				{
				case HorizontalAlignment.Left:
					createParams.Style |= 0;
					break;
				case HorizontalAlignment.Right:
					createParams.Style |= 2;
					break;
				case HorizontalAlignment.Center:
					createParams.Style |= 1;
					break;
				}
				return createParams;
			}
		}

		/// <summary>Gets or sets the culture information associated with the masked text box.</summary>
		/// <returns>A <see cref="T:System.Globalization.CultureInfo" /> representing the culture supported by the <see cref="T:System.Windows.Forms.MaskedTextBox" />.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <see cref="P:System.Windows.Forms.MaskedTextBox.Culture" /> was set to <see langword="null" />.</exception>
		// Token: 0x17000A8C RID: 2700
		// (get) Token: 0x06002B86 RID: 11142 RVA: 0x000CB466 File Offset: 0x000C9666
		// (set) Token: 0x06002B87 RID: 11143 RVA: 0x000CB474 File Offset: 0x000C9674
		[SRCategory("CatBehavior")]
		[SRDescription("MaskedTextBoxCultureDescr")]
		[RefreshProperties(RefreshProperties.Repaint)]
		public CultureInfo Culture
		{
			get
			{
				return this.maskedTextProvider.Culture;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException();
				}
				if (!this.maskedTextProvider.Culture.Equals(value))
				{
					MaskedTextProvider maskedTextProvider = new MaskedTextProvider(this.maskedTextProvider.Mask, value, this.maskedTextProvider.AllowPromptAsInput, this.maskedTextProvider.PromptChar, this.maskedTextProvider.PasswordChar, this.maskedTextProvider.AsciiOnly);
					this.SetMaskedTextProvider(maskedTextProvider);
				}
			}
		}

		/// <summary>Gets or sets a value that determines whether literals and prompt characters are copied to the clipboard.</summary>
		/// <returns>One of the <see cref="T:System.Windows.Forms.MaskFormat" /> values. The default is <see cref="F:System.Windows.Forms.MaskFormat.IncludeLiterals" />.</returns>
		/// <exception cref="T:System.ComponentModel.InvalidEnumArgumentException">Property set with a <see cref="T:System.Windows.Forms.MaskFormat" />  value that is not valid. </exception>
		// Token: 0x17000A8D RID: 2701
		// (get) Token: 0x06002B88 RID: 11144 RVA: 0x000CB4E2 File Offset: 0x000C96E2
		// (set) Token: 0x06002B89 RID: 11145 RVA: 0x000CB524 File Offset: 0x000C9724
		[SRCategory("CatBehavior")]
		[SRDescription("MaskedTextBoxCutCopyMaskFormat")]
		[RefreshProperties(RefreshProperties.Repaint)]
		[DefaultValue(MaskFormat.IncludeLiterals)]
		public MaskFormat CutCopyMaskFormat
		{
			get
			{
				if (this.flagState[MaskedTextBox.CUTCOPYINCLUDEPROMPT])
				{
					if (this.flagState[MaskedTextBox.CUTCOPYINCLUDELITERALS])
					{
						return MaskFormat.IncludePromptAndLiterals;
					}
					return MaskFormat.IncludePrompt;
				}
				else
				{
					if (this.flagState[MaskedTextBox.CUTCOPYINCLUDELITERALS])
					{
						return MaskFormat.IncludeLiterals;
					}
					return MaskFormat.ExcludePromptAndLiterals;
				}
			}
			set
			{
				if (!ClientUtils.IsEnumValid(value, (int)value, 0, 3))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(MaskFormat));
				}
				if (value == MaskFormat.IncludePrompt)
				{
					this.flagState[MaskedTextBox.CUTCOPYINCLUDEPROMPT] = true;
					this.flagState[MaskedTextBox.CUTCOPYINCLUDELITERALS] = false;
					return;
				}
				if (value == MaskFormat.IncludeLiterals)
				{
					this.flagState[MaskedTextBox.CUTCOPYINCLUDEPROMPT] = false;
					this.flagState[MaskedTextBox.CUTCOPYINCLUDELITERALS] = true;
					return;
				}
				bool value2 = value == MaskFormat.IncludePromptAndLiterals;
				this.flagState[MaskedTextBox.CUTCOPYINCLUDEPROMPT] = value2;
				this.flagState[MaskedTextBox.CUTCOPYINCLUDELITERALS] = value2;
			}
		}

		/// <summary>Gets or sets the <see cref="T:System.IFormatProvider" /> to use when performing type validation.</summary>
		/// <returns>An object that implements the <see cref="T:System.IFormatProvider" /> interface. </returns>
		// Token: 0x17000A8E RID: 2702
		// (get) Token: 0x06002B8A RID: 11146 RVA: 0x000CB5CC File Offset: 0x000C97CC
		// (set) Token: 0x06002B8B RID: 11147 RVA: 0x000CB5D4 File Offset: 0x000C97D4
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public IFormatProvider FormatProvider
		{
			get
			{
				return this.formatProvider;
			}
			set
			{
				this.formatProvider = value;
			}
		}

		/// <summary>Gets or sets a value indicating whether the prompt characters in the input mask are hidden when the masked text box loses focus.</summary>
		/// <returns>
		///     <see langword="true" /> if <see cref="P:System.Windows.Forms.MaskedTextBox.PromptChar" /> is hidden when <see cref="T:System.Windows.Forms.MaskedTextBox" /> does not have focus; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
		// Token: 0x17000A8F RID: 2703
		// (get) Token: 0x06002B8C RID: 11148 RVA: 0x000CB5DD File Offset: 0x000C97DD
		// (set) Token: 0x06002B8D RID: 11149 RVA: 0x000CB5F0 File Offset: 0x000C97F0
		[SRCategory("CatBehavior")]
		[SRDescription("MaskedTextBoxHidePromptOnLeaveDescr")]
		[RefreshProperties(RefreshProperties.Repaint)]
		[DefaultValue(false)]
		public bool HidePromptOnLeave
		{
			get
			{
				return this.flagState[MaskedTextBox.HIDE_PROMPT_ON_LEAVE];
			}
			set
			{
				if (this.flagState[MaskedTextBox.HIDE_PROMPT_ON_LEAVE] != value)
				{
					this.flagState[MaskedTextBox.HIDE_PROMPT_ON_LEAVE] = value;
					if (!this.flagState[MaskedTextBox.IS_NULL_MASK] && !this.Focused && !this.MaskFull && !base.DesignMode)
					{
						this.SetWindowText();
					}
				}
			}
		}

		// Token: 0x17000A90 RID: 2704
		// (get) Token: 0x06002B8E RID: 11150 RVA: 0x000CB651 File Offset: 0x000C9851
		// (set) Token: 0x06002B8F RID: 11151 RVA: 0x000CB65E File Offset: 0x000C985E
		private bool IncludeLiterals
		{
			get
			{
				return this.maskedTextProvider.IncludeLiterals;
			}
			set
			{
				this.maskedTextProvider.IncludeLiterals = value;
			}
		}

		// Token: 0x17000A91 RID: 2705
		// (get) Token: 0x06002B90 RID: 11152 RVA: 0x000CB66C File Offset: 0x000C986C
		// (set) Token: 0x06002B91 RID: 11153 RVA: 0x000CB679 File Offset: 0x000C9879
		private bool IncludePrompt
		{
			get
			{
				return this.maskedTextProvider.IncludePrompt;
			}
			set
			{
				this.maskedTextProvider.IncludePrompt = value;
			}
		}

		/// <summary>Gets or sets the text insertion mode of the masked text box control.</summary>
		/// <returns>An <see cref="T:System.Windows.Forms.InsertKeyMode" /> value that indicates the current insertion mode. The default is <see cref="F:System.Windows.Forms.InsertKeyMode.Default" />.</returns>
		/// <exception cref="T:System.ComponentModel.InvalidEnumArgumentException">An invalid <see cref="T:System.Windows.Forms.InsertKeyMode" /> value was supplied when setting this property.</exception>
		// Token: 0x17000A92 RID: 2706
		// (get) Token: 0x06002B92 RID: 11154 RVA: 0x000CB687 File Offset: 0x000C9887
		// (set) Token: 0x06002B93 RID: 11155 RVA: 0x000CB690 File Offset: 0x000C9890
		[SRCategory("CatBehavior")]
		[SRDescription("MaskedTextBoxInsertKeyModeDescr")]
		[DefaultValue(InsertKeyMode.Default)]
		public InsertKeyMode InsertKeyMode
		{
			get
			{
				return this.insertMode;
			}
			set
			{
				if (!ClientUtils.IsEnumValid(value, (int)value, 0, 2))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(InsertKeyMode));
				}
				if (this.insertMode != value)
				{
					bool isOverwriteMode = this.IsOverwriteMode;
					this.insertMode = value;
					if (isOverwriteMode != this.IsOverwriteMode)
					{
						this.OnIsOverwriteModeChanged(EventArgs.Empty);
					}
				}
			}
		}

		/// <summary>Determines whether the specified key is an input key or a special key that requires preprocessing.</summary>
		/// <param name="keyData">One of the <see cref="T:System.Windows.Forms.Keys" /> values. </param>
		// Token: 0x06002B94 RID: 11156 RVA: 0x000CB6EE File Offset: 0x000C98EE
		protected override bool IsInputKey(Keys keyData)
		{
			return (keyData & Keys.KeyCode) != Keys.Return && base.IsInputKey(keyData);
		}

		/// <summary>Gets a value that specifies whether new user input overwrites existing input.</summary>
		/// <returns>
		///     <see langword="true" /> if <see cref="T:System.Windows.Forms.MaskedTextBox" /> will overwrite existing characters as the user enters new ones; <see langword="false" /> if <see cref="T:System.Windows.Forms.MaskedTextBox" /> will shift existing characters forward. The default is <see langword="false" />.</returns>
		// Token: 0x17000A93 RID: 2707
		// (get) Token: 0x06002B95 RID: 11157 RVA: 0x000CB704 File Offset: 0x000C9904
		[Browsable(false)]
		public bool IsOverwriteMode
		{
			get
			{
				if (this.flagState[MaskedTextBox.IS_NULL_MASK])
				{
					return false;
				}
				switch (this.insertMode)
				{
				case InsertKeyMode.Default:
					return this.flagState[MaskedTextBox.INSERT_TOGGLED];
				case InsertKeyMode.Insert:
					return false;
				case InsertKeyMode.Overwrite:
					return true;
				default:
					return false;
				}
			}
		}

		/// <summary>Occurs after the insert mode has changed. </summary>
		// Token: 0x14000208 RID: 520
		// (add) Token: 0x06002B96 RID: 11158 RVA: 0x000CB756 File Offset: 0x000C9956
		// (remove) Token: 0x06002B97 RID: 11159 RVA: 0x000CB769 File Offset: 0x000C9969
		[SRCategory("CatPropertyChanged")]
		[SRDescription("MaskedTextBoxIsOverwriteModeChangedDescr")]
		public event EventHandler IsOverwriteModeChanged
		{
			add
			{
				base.Events.AddHandler(MaskedTextBox.EVENT_ISOVERWRITEMODECHANGED, value);
			}
			remove
			{
				base.Events.RemoveHandler(MaskedTextBox.EVENT_ISOVERWRITEMODECHANGED, value);
			}
		}

		/// <summary>Gets or sets the lines of text in multiline configurations. This property is not supported by <see cref="T:System.Windows.Forms.MaskedTextBox" />.</summary>
		/// <returns>An array of type <see cref="T:System.String" /> that contains a single line. </returns>
		// Token: 0x17000A94 RID: 2708
		// (get) Token: 0x06002B98 RID: 11160 RVA: 0x000CB77C File Offset: 0x000C997C
		// (set) Token: 0x06002B99 RID: 11161 RVA: 0x0000701A File Offset: 0x0000521A
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new string[] Lines
		{
			get
			{
				this.flagState[MaskedTextBox.QUERY_BASE_TEXT] = true;
				string[] lines;
				try
				{
					lines = base.Lines;
				}
				finally
				{
					this.flagState[MaskedTextBox.QUERY_BASE_TEXT] = false;
				}
				return lines;
			}
			set
			{
			}
		}

		/// <summary>Gets or sets the input mask to use at run time. </summary>
		/// <returns>A <see cref="T:System.String" /> representing the current mask. The default value is the empty string which allows any input.</returns>
		/// <exception cref="T:System.ArgumentException">The string supplied to the <see cref="P:System.Windows.Forms.MaskedTextBox.Mask" /> property is not a valid mask. Invalid masks include masks containing non-printable characters.</exception>
		// Token: 0x17000A95 RID: 2709
		// (get) Token: 0x06002B9A RID: 11162 RVA: 0x000CB7C8 File Offset: 0x000C99C8
		// (set) Token: 0x06002B9B RID: 11163 RVA: 0x000CB7F0 File Offset: 0x000C99F0
		[SRCategory("CatBehavior")]
		[SRDescription("MaskedTextBoxMaskDescr")]
		[RefreshProperties(RefreshProperties.Repaint)]
		[DefaultValue("")]
		[MergableProperty(false)]
		[Localizable(true)]
		[Editor("System.Windows.Forms.Design.MaskPropertyEditor, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
		public string Mask
		{
			get
			{
				if (!this.flagState[MaskedTextBox.IS_NULL_MASK])
				{
					return this.maskedTextProvider.Mask;
				}
				return string.Empty;
			}
			set
			{
				if (this.flagState[MaskedTextBox.IS_NULL_MASK] == string.IsNullOrEmpty(value) && (this.flagState[MaskedTextBox.IS_NULL_MASK] || value == this.maskedTextProvider.Mask))
				{
					return;
				}
				string textOnInitializingMask = null;
				string mask = value;
				if (string.IsNullOrEmpty(value))
				{
					string textOutput = this.TextOutput;
					string text = this.maskedTextProvider.ToString(false, false);
					this.flagState[MaskedTextBox.IS_NULL_MASK] = true;
					if (this.maskedTextProvider.IsPassword)
					{
						this.SetEditControlPasswordChar(this.maskedTextProvider.PasswordChar);
					}
					this.SetWindowText(text, false, false);
					EventArgs empty = EventArgs.Empty;
					this.OnMaskChanged(empty);
					if (text != textOutput)
					{
						this.OnTextChanged(empty);
					}
					mask = "<>";
				}
				else
				{
					for (int i = 0; i < value.Length; i++)
					{
						char c = value[i];
						if (!MaskedTextProvider.IsValidMaskChar(c))
						{
							throw new ArgumentException(SR.GetString("MaskedTextBoxMaskInvalidChar"));
						}
					}
					if (this.flagState[MaskedTextBox.IS_NULL_MASK])
					{
						textOnInitializingMask = this.Text;
					}
				}
				MaskedTextProvider newProvider = new MaskedTextProvider(mask, this.maskedTextProvider.Culture, this.maskedTextProvider.AllowPromptAsInput, this.maskedTextProvider.PromptChar, this.maskedTextProvider.PasswordChar, this.maskedTextProvider.AsciiOnly);
				this.SetMaskedTextProvider(newProvider, textOnInitializingMask);
			}
		}

		/// <summary>Occurs after the input mask is changed.</summary>
		// Token: 0x14000209 RID: 521
		// (add) Token: 0x06002B9C RID: 11164 RVA: 0x000CB95B File Offset: 0x000C9B5B
		// (remove) Token: 0x06002B9D RID: 11165 RVA: 0x000CB96E File Offset: 0x000C9B6E
		[SRCategory("CatPropertyChanged")]
		[SRDescription("MaskedTextBoxMaskChangedDescr")]
		public event EventHandler MaskChanged
		{
			add
			{
				base.Events.AddHandler(MaskedTextBox.EVENT_MASKCHANGED, value);
			}
			remove
			{
				base.Events.RemoveHandler(MaskedTextBox.EVENT_MASKCHANGED, value);
			}
		}

		/// <summary>Gets a value indicating whether all required inputs have been entered into the input mask.</summary>
		/// <returns>
		///     <see langword="true" /> if all required input has been entered into the mask; otherwise, <see langword="false" />.</returns>
		// Token: 0x17000A96 RID: 2710
		// (get) Token: 0x06002B9E RID: 11166 RVA: 0x000CB981 File Offset: 0x000C9B81
		[Browsable(false)]
		public bool MaskCompleted
		{
			get
			{
				return this.maskedTextProvider.MaskCompleted;
			}
		}

		/// <summary>Gets a value indicating whether all required and optional inputs have been entered into the input mask. </summary>
		/// <returns>
		///     <see langword="true" /> if all required and optional inputs have been entered; otherwise, <see langword="false" />. </returns>
		// Token: 0x17000A97 RID: 2711
		// (get) Token: 0x06002B9F RID: 11167 RVA: 0x000CB98E File Offset: 0x000C9B8E
		[Browsable(false)]
		public bool MaskFull
		{
			get
			{
				return this.maskedTextProvider.MaskFull;
			}
		}

		/// <summary>Gets a clone of the mask provider associated with this instance of the masked text box control.</summary>
		/// <returns>A masking language provider of type <see cref="P:System.Windows.Forms.MaskedTextBox.MaskedTextProvider" />.</returns>
		// Token: 0x17000A98 RID: 2712
		// (get) Token: 0x06002BA0 RID: 11168 RVA: 0x000CB99B File Offset: 0x000C9B9B
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public MaskedTextProvider MaskedTextProvider
		{
			get
			{
				if (!this.flagState[MaskedTextBox.IS_NULL_MASK])
				{
					return (MaskedTextProvider)this.maskedTextProvider.Clone();
				}
				return null;
			}
		}

		/// <summary>Occurs when the user's input or assigned character does not match the corresponding format element of the input mask.</summary>
		// Token: 0x1400020A RID: 522
		// (add) Token: 0x06002BA1 RID: 11169 RVA: 0x000CB9C1 File Offset: 0x000C9BC1
		// (remove) Token: 0x06002BA2 RID: 11170 RVA: 0x000CB9D4 File Offset: 0x000C9BD4
		[SRCategory("CatBehavior")]
		[SRDescription("MaskedTextBoxMaskInputRejectedDescr")]
		public event MaskInputRejectedEventHandler MaskInputRejected
		{
			add
			{
				base.Events.AddHandler(MaskedTextBox.EVENT_MASKINPUTREJECTED, value);
			}
			remove
			{
				base.Events.RemoveHandler(MaskedTextBox.EVENT_MASKINPUTREJECTED, value);
			}
		}

		/// <summary>Gets or sets the maximum number of characters the user can type or paste into the text box control. This property is not supported by <see cref="T:System.Windows.Forms.MaskedTextBox" />. </summary>
		/// <returns>This property always returns 0. </returns>
		// Token: 0x17000A99 RID: 2713
		// (get) Token: 0x06002BA3 RID: 11171 RVA: 0x000CB9E7 File Offset: 0x000C9BE7
		// (set) Token: 0x06002BA4 RID: 11172 RVA: 0x0000701A File Offset: 0x0000521A
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public override int MaxLength
		{
			get
			{
				return base.MaxLength;
			}
			set
			{
			}
		}

		/// <summary>Gets or sets a value indicating whether this is a multiline text box control. This property is not fully supported by <see cref="T:System.Windows.Forms.MaskedTextBox" />. </summary>
		/// <returns>This property always returns <see langword="false" />.</returns>
		// Token: 0x17000A9A RID: 2714
		// (get) Token: 0x06002BA5 RID: 11173 RVA: 0x0000E0A4 File Offset: 0x0000C2A4
		// (set) Token: 0x06002BA6 RID: 11174 RVA: 0x0000701A File Offset: 0x0000521A
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public override bool Multiline
		{
			get
			{
				return false;
			}
			set
			{
			}
		}

		/// <summary>Typically occurs when the value of the <see cref="P:System.Windows.Forms.MaskedTextBox.Multiline" /> property has changed; however, this event is not raised by <see cref="T:System.Windows.Forms.MaskedTextBox" />.</summary>
		// Token: 0x1400020B RID: 523
		// (add) Token: 0x06002BA7 RID: 11175 RVA: 0x0000701A File Offset: 0x0000521A
		// (remove) Token: 0x06002BA8 RID: 11176 RVA: 0x0000701A File Offset: 0x0000521A
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new event EventHandler MultilineChanged
		{
			add
			{
			}
			remove
			{
			}
		}

		/// <summary>Gets or sets the character to be displayed in substitute for user input.</summary>
		/// <returns>The <see cref="T:System.Char" /> value used as the password character.</returns>
		/// <exception cref="T:System.ArgumentException">The character specified when setting this property is not a valid password character, as determined by the <see cref="M:System.ComponentModel.MaskedTextProvider.IsValidPasswordChar(System.Char)" /> method of the <see cref="T:System.ComponentModel.MaskedTextProvider" /> class.</exception>
		/// <exception cref="T:System.InvalidOperationException">The password character specified is the same as the current prompt character, <see cref="P:System.Windows.Forms.MaskedTextBox.PromptChar" />. The two are required to be different.</exception>
		// Token: 0x17000A9B RID: 2715
		// (get) Token: 0x06002BA9 RID: 11177 RVA: 0x000CB9EF File Offset: 0x000C9BEF
		// (set) Token: 0x06002BAA RID: 11178 RVA: 0x000CB9FC File Offset: 0x000C9BFC
		[SRCategory("CatBehavior")]
		[SRDescription("MaskedTextBoxPasswordCharDescr")]
		[RefreshProperties(RefreshProperties.Repaint)]
		[DefaultValue('\0')]
		public char PasswordChar
		{
			get
			{
				return this.maskedTextProvider.PasswordChar;
			}
			set
			{
				if (!MaskedTextProvider.IsValidPasswordChar(value))
				{
					throw new ArgumentException(SR.GetString("MaskedTextBoxInvalidCharError"));
				}
				if (this.passwordChar != value)
				{
					if (value == this.maskedTextProvider.PromptChar)
					{
						throw new InvalidOperationException(SR.GetString("MaskedTextBoxPasswordAndPromptCharError"));
					}
					this.passwordChar = value;
					if (!this.UseSystemPasswordChar)
					{
						this.maskedTextProvider.PasswordChar = value;
						if (this.flagState[MaskedTextBox.IS_NULL_MASK])
						{
							this.SetEditControlPasswordChar(value);
						}
						else
						{
							this.SetWindowText();
						}
						base.VerifyImeRestrictedModeChanged();
					}
				}
			}
		}

		// Token: 0x17000A9C RID: 2716
		// (get) Token: 0x06002BAB RID: 11179 RVA: 0x000CBA8A File Offset: 0x000C9C8A
		internal override bool PasswordProtect
		{
			get
			{
				if (this.maskedTextProvider != null)
				{
					return this.maskedTextProvider.IsPassword;
				}
				return base.PasswordProtect;
			}
		}

		/// <summary>Gets or sets the character used to represent the absence of user input in <see cref="T:System.Windows.Forms.MaskedTextBox" />.</summary>
		/// <returns>The character used to prompt the user for input. The default is an underscore (_). </returns>
		/// <exception cref="T:System.ArgumentException">The character specified when setting this property is not a valid prompt character, as determined by the <see cref="M:System.ComponentModel.MaskedTextProvider.IsValidPasswordChar(System.Char)" /> method of the <see cref="T:System.ComponentModel.MaskedTextProvider" /> class.</exception>
		/// <exception cref="T:System.InvalidOperationException">The prompt character specified is the same as the current password character, <see cref="P:System.Windows.Forms.MaskedTextBox.PasswordChar" />. The two are required to be different.</exception>
		// Token: 0x17000A9D RID: 2717
		// (get) Token: 0x06002BAC RID: 11180 RVA: 0x000CBAA6 File Offset: 0x000C9CA6
		// (set) Token: 0x06002BAD RID: 11181 RVA: 0x000CBAB4 File Offset: 0x000C9CB4
		[SRCategory("CatAppearance")]
		[SRDescription("MaskedTextBoxPromptCharDescr")]
		[RefreshProperties(RefreshProperties.Repaint)]
		[Localizable(true)]
		[DefaultValue('_')]
		public char PromptChar
		{
			get
			{
				return this.maskedTextProvider.PromptChar;
			}
			set
			{
				if (!MaskedTextProvider.IsValidInputChar(value))
				{
					throw new ArgumentException(SR.GetString("MaskedTextBoxInvalidCharError"));
				}
				if (this.maskedTextProvider.PromptChar != value)
				{
					if (value == this.passwordChar || value == this.maskedTextProvider.PasswordChar)
					{
						throw new InvalidOperationException(SR.GetString("MaskedTextBoxPasswordAndPromptCharError"));
					}
					MaskedTextProvider maskedTextProvider = new MaskedTextProvider(this.maskedTextProvider.Mask, this.maskedTextProvider.Culture, this.maskedTextProvider.AllowPromptAsInput, value, this.maskedTextProvider.PasswordChar, this.maskedTextProvider.AsciiOnly);
					this.SetMaskedTextProvider(maskedTextProvider);
				}
			}
		}

		/// <summary>Gets or sets a value indicating whether text in the text box is read-only. </summary>
		/// <returns>
		///     <see langword="true" /> to indicate the text is read only; otherwise, <see langword="false" />.</returns>
		// Token: 0x17000A9E RID: 2718
		// (get) Token: 0x06002BAE RID: 11182 RVA: 0x000CBB53 File Offset: 0x000C9D53
		// (set) Token: 0x06002BAF RID: 11183 RVA: 0x000CBB5B File Offset: 0x000C9D5B
		public new bool ReadOnly
		{
			get
			{
				return base.ReadOnly;
			}
			set
			{
				if (this.ReadOnly != value)
				{
					base.ReadOnly = value;
					if (!this.flagState[MaskedTextBox.IS_NULL_MASK])
					{
						this.SetWindowText();
					}
				}
			}
		}

		/// <summary>Gets or sets a value indicating whether the parsing of user input should stop after the first invalid character is reached.</summary>
		/// <returns>
		///     <see langword="true" /> if processing of the input string should be terminated at the first parsing error; otherwise, <see langword="false" /> if processing should ignore all errors. The default is <see langword="false" />.</returns>
		// Token: 0x17000A9F RID: 2719
		// (get) Token: 0x06002BB0 RID: 11184 RVA: 0x000CBB85 File Offset: 0x000C9D85
		// (set) Token: 0x06002BB1 RID: 11185 RVA: 0x000CBB97 File Offset: 0x000C9D97
		[SRCategory("CatBehavior")]
		[SRDescription("MaskedTextBoxRejectInputOnFirstFailureDescr")]
		[DefaultValue(false)]
		public bool RejectInputOnFirstFailure
		{
			get
			{
				return this.flagState[MaskedTextBox.REJECT_INPUT_ON_FIRST_FAILURE];
			}
			set
			{
				this.flagState[MaskedTextBox.REJECT_INPUT_ON_FIRST_FAILURE] = value;
			}
		}

		/// <summary>Gets or sets a value that determines how an input character that matches the prompt character should be handled.</summary>
		/// <returns>
		///     <see langword="true" /> if the prompt character entered as input causes the current editable position in the mask to be reset; otherwise, <see langword="false" /> to indicate that the prompt character is to be processed as a normal input character. The default is <see langword="true" />.</returns>
		// Token: 0x17000AA0 RID: 2720
		// (get) Token: 0x06002BB2 RID: 11186 RVA: 0x000CBBAA File Offset: 0x000C9DAA
		// (set) Token: 0x06002BB3 RID: 11187 RVA: 0x000CBBB7 File Offset: 0x000C9DB7
		[SRCategory("CatBehavior")]
		[SRDescription("MaskedTextBoxResetOnPrompt")]
		[DefaultValue(true)]
		public bool ResetOnPrompt
		{
			get
			{
				return this.maskedTextProvider.ResetOnPrompt;
			}
			set
			{
				this.maskedTextProvider.ResetOnPrompt = value;
			}
		}

		/// <summary>Gets or sets a value that determines how a space input character should be handled.</summary>
		/// <returns>
		///     <see langword="true" /> if the space input character causes the current editable position in the mask to be reset; otherwise, <see langword="false" /> to indicate that it is to be processed as a normal input character. The default is <see langword="true" />.</returns>
		// Token: 0x17000AA1 RID: 2721
		// (get) Token: 0x06002BB4 RID: 11188 RVA: 0x000CBBC5 File Offset: 0x000C9DC5
		// (set) Token: 0x06002BB5 RID: 11189 RVA: 0x000CBBD2 File Offset: 0x000C9DD2
		[SRCategory("CatBehavior")]
		[SRDescription("MaskedTextBoxResetOnSpace")]
		[DefaultValue(true)]
		public bool ResetOnSpace
		{
			get
			{
				return this.maskedTextProvider.ResetOnSpace;
			}
			set
			{
				this.maskedTextProvider.ResetOnSpace = value;
			}
		}

		/// <summary>Gets or sets a value indicating whether the user is allowed to reenter literal values.</summary>
		/// <returns>
		///     <see langword="true" /> to allow literals to be reentered; otherwise, <see langword="false" /> to prevent the user from overwriting literal characters. The default is <see langword="true" />.</returns>
		// Token: 0x17000AA2 RID: 2722
		// (get) Token: 0x06002BB6 RID: 11190 RVA: 0x000CBBE0 File Offset: 0x000C9DE0
		// (set) Token: 0x06002BB7 RID: 11191 RVA: 0x000CBBED File Offset: 0x000C9DED
		[SRCategory("CatBehavior")]
		[SRDescription("MaskedTextBoxSkipLiterals")]
		[DefaultValue(true)]
		public bool SkipLiterals
		{
			get
			{
				return this.maskedTextProvider.SkipLiterals;
			}
			set
			{
				this.maskedTextProvider.SkipLiterals = value;
			}
		}

		/// <summary>Gets or sets the current selection in the <see cref="T:System.Windows.Forms.MaskedTextBox" /> control.</summary>
		/// <returns>The currently selected text as a <see cref="T:System.String" />. If no text is currently selected, this property resolves to an empty string.</returns>
		// Token: 0x17000AA3 RID: 2723
		// (get) Token: 0x06002BB8 RID: 11192 RVA: 0x000CBBFB File Offset: 0x000C9DFB
		// (set) Token: 0x06002BB9 RID: 11193 RVA: 0x000CBC1C File Offset: 0x000C9E1C
		public override string SelectedText
		{
			get
			{
				if (this.flagState[MaskedTextBox.IS_NULL_MASK])
				{
					return base.SelectedText;
				}
				return this.GetSelectedText();
			}
			set
			{
				this.SetSelectedTextInternal(value, true);
			}
		}

		// Token: 0x06002BBA RID: 11194 RVA: 0x000CBC26 File Offset: 0x000C9E26
		internal override void SetSelectedTextInternal(string value, bool clearUndo)
		{
			if (this.flagState[MaskedTextBox.IS_NULL_MASK])
			{
				base.SetSelectedTextInternal(value, true);
				return;
			}
			this.PasteInt(value);
		}

		// Token: 0x06002BBB RID: 11195 RVA: 0x000CBC4A File Offset: 0x000C9E4A
		private void ImeComplete()
		{
			this.flagState[MaskedTextBox.IME_COMPLETING] = true;
			this.ImeNotify(1);
		}

		// Token: 0x06002BBC RID: 11196 RVA: 0x000CBC64 File Offset: 0x000C9E64
		private void ImeNotify(int action)
		{
			HandleRef hWnd = new HandleRef(this, base.Handle);
			IntPtr intPtr = UnsafeNativeMethods.ImmGetContext(hWnd);
			if (intPtr != IntPtr.Zero)
			{
				try
				{
					UnsafeNativeMethods.ImmNotifyIME(new HandleRef(null, intPtr), 21, action, 0);
				}
				finally
				{
					UnsafeNativeMethods.ImmReleaseContext(hWnd, new HandleRef(null, intPtr));
				}
			}
		}

		// Token: 0x06002BBD RID: 11197 RVA: 0x000CBCC8 File Offset: 0x000C9EC8
		private void SetEditControlPasswordChar(char pwdChar)
		{
			if (base.IsHandleCreated)
			{
				base.SendMessage(204, (int)pwdChar, 0);
				base.Invalidate();
			}
		}

		// Token: 0x17000AA4 RID: 2724
		// (get) Token: 0x06002BBE RID: 11198 RVA: 0x000CBCE8 File Offset: 0x000C9EE8
		private char SystemPasswordChar
		{
			get
			{
				if (MaskedTextBox.systemPwdChar == '\0')
				{
					TextBox textBox = new TextBox();
					textBox.UseSystemPasswordChar = true;
					MaskedTextBox.systemPwdChar = textBox.PasswordChar;
					textBox.Dispose();
				}
				return MaskedTextBox.systemPwdChar;
			}
		}

		/// <summary>Gets or sets the text as it is currently displayed to the user. </summary>
		/// <returns>A <see cref="T:System.String" /> containing the text currently displayed by the control. The default is an empty string.</returns>
		// Token: 0x17000AA5 RID: 2725
		// (get) Token: 0x06002BBF RID: 11199 RVA: 0x000CBD1F File Offset: 0x000C9F1F
		// (set) Token: 0x06002BC0 RID: 11200 RVA: 0x000CBD54 File Offset: 0x000C9F54
		[Editor("System.Windows.Forms.Design.MaskedTextBoxTextEditor, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
		[SRCategory("CatAppearance")]
		[RefreshProperties(RefreshProperties.Repaint)]
		[Bindable(true)]
		[DefaultValue("")]
		[Localizable(true)]
		public override string Text
		{
			get
			{
				if (this.flagState[MaskedTextBox.IS_NULL_MASK] || this.flagState[MaskedTextBox.QUERY_BASE_TEXT])
				{
					return base.Text;
				}
				return this.TextOutput;
			}
			set
			{
				if (this.flagState[MaskedTextBox.IS_NULL_MASK])
				{
					base.Text = value;
					return;
				}
				if (string.IsNullOrEmpty(value))
				{
					this.Delete(Keys.Delete, 0, this.maskedTextProvider.Length);
					return;
				}
				if (!this.RejectInputOnFirstFailure)
				{
					this.Replace(value, 0, this.maskedTextProvider.Length);
					return;
				}
				string textOutput = this.TextOutput;
				MaskedTextResultHint rejectionHint;
				if (this.maskedTextProvider.Set(value, out this.caretTestPos, out rejectionHint))
				{
					if (this.TextOutput != textOutput)
					{
						this.SetText();
					}
					int selectionStart = this.caretTestPos + 1;
					this.caretTestPos = selectionStart;
					base.SelectionStart = selectionStart;
					return;
				}
				this.OnMaskInputRejected(new MaskInputRejectedEventArgs(this.caretTestPos, rejectionHint));
			}
		}

		/// <summary>Gets the length of the displayed text. </summary>
		/// <returns>An Int32 representing the number of characters in the <see cref="P:System.Windows.Forms.MaskedTextBox.Text" /> property. <see cref="P:System.Windows.Forms.MaskedTextBox.TextLength" /> respects properties such as <see cref="P:System.Windows.Forms.MaskedTextBox.HidePromptOnLeave" />, which means that the return results may be different depending on whether the control has focus.</returns>
		// Token: 0x17000AA6 RID: 2726
		// (get) Token: 0x06002BC1 RID: 11201 RVA: 0x000CBE0F File Offset: 0x000CA00F
		[Browsable(false)]
		public override int TextLength
		{
			get
			{
				if (this.flagState[MaskedTextBox.IS_NULL_MASK])
				{
					return base.TextLength;
				}
				return this.GetFormattedDisplayString().Length;
			}
		}

		// Token: 0x17000AA7 RID: 2727
		// (get) Token: 0x06002BC2 RID: 11202 RVA: 0x000CBE35 File Offset: 0x000CA035
		private string TextOutput
		{
			get
			{
				return this.maskedTextProvider.ToString();
			}
		}

		/// <summary>Gets or sets how text is aligned in a masked text box control.</summary>
		/// <returns>One of the <see cref="T:System.Windows.Forms.HorizontalAlignment" /> enumeration values that specifies how text is aligned relative to the control. The default is <see cref="F:System.Windows.Forms.HorizontalAlignment.Left" />.</returns>
		/// <exception cref="T:System.ComponentModel.InvalidEnumArgumentException">The value assigned to this property is not of type <see cref="T:System.Windows.Forms.HorizontalAlignment" />.</exception>
		// Token: 0x17000AA8 RID: 2728
		// (get) Token: 0x06002BC3 RID: 11203 RVA: 0x000CBE42 File Offset: 0x000CA042
		// (set) Token: 0x06002BC4 RID: 11204 RVA: 0x000CBE4C File Offset: 0x000CA04C
		[Localizable(true)]
		[SRCategory("CatAppearance")]
		[DefaultValue(HorizontalAlignment.Left)]
		[SRDescription("TextBoxTextAlignDescr")]
		public HorizontalAlignment TextAlign
		{
			get
			{
				return this.textAlign;
			}
			set
			{
				if (this.textAlign != value)
				{
					if (!ClientUtils.IsEnumValid(value, (int)value, 0, 2))
					{
						throw new InvalidEnumArgumentException("value", (int)value, typeof(HorizontalAlignment));
					}
					this.textAlign = value;
					base.RecreateHandle();
					this.OnTextAlignChanged(EventArgs.Empty);
				}
			}
		}

		/// <summary>Occurs when the text alignment is changed. </summary>
		// Token: 0x1400020C RID: 524
		// (add) Token: 0x06002BC5 RID: 11205 RVA: 0x000CBEA0 File Offset: 0x000CA0A0
		// (remove) Token: 0x06002BC6 RID: 11206 RVA: 0x000CBEB3 File Offset: 0x000CA0B3
		[SRCategory("CatPropertyChanged")]
		[SRDescription("RadioButtonOnTextAlignChangedDescr")]
		public event EventHandler TextAlignChanged
		{
			add
			{
				base.Events.AddHandler(MaskedTextBox.EVENT_TEXTALIGNCHANGED, value);
			}
			remove
			{
				base.Events.RemoveHandler(MaskedTextBox.EVENT_TEXTALIGNCHANGED, value);
			}
		}

		/// <summary>Gets or sets a value that determines whether literals and prompt characters are included in the formatted string.</summary>
		/// <returns>One of the <see cref="T:System.Windows.Forms.MaskFormat" /> values. The default is <see cref="F:System.Windows.Forms.MaskFormat.IncludeLiterals" />.</returns>
		/// <exception cref="T:System.ComponentModel.InvalidEnumArgumentException">Property set with a <see cref="T:System.Windows.Forms.MaskFormat" /> value that is not valid. </exception>
		// Token: 0x17000AA9 RID: 2729
		// (get) Token: 0x06002BC7 RID: 11207 RVA: 0x000CBEC6 File Offset: 0x000CA0C6
		// (set) Token: 0x06002BC8 RID: 11208 RVA: 0x000CBEE8 File Offset: 0x000CA0E8
		[SRCategory("CatBehavior")]
		[SRDescription("MaskedTextBoxTextMaskFormat")]
		[RefreshProperties(RefreshProperties.Repaint)]
		[DefaultValue(MaskFormat.IncludeLiterals)]
		public MaskFormat TextMaskFormat
		{
			get
			{
				if (this.IncludePrompt)
				{
					if (this.IncludeLiterals)
					{
						return MaskFormat.IncludePromptAndLiterals;
					}
					return MaskFormat.IncludePrompt;
				}
				else
				{
					if (this.IncludeLiterals)
					{
						return MaskFormat.IncludeLiterals;
					}
					return MaskFormat.ExcludePromptAndLiterals;
				}
			}
			set
			{
				if (this.TextMaskFormat == value)
				{
					return;
				}
				if (!ClientUtils.IsEnumValid(value, (int)value, 0, 3))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(MaskFormat));
				}
				string text = this.flagState[MaskedTextBox.IS_NULL_MASK] ? null : this.TextOutput;
				if (value == MaskFormat.IncludePrompt)
				{
					this.IncludePrompt = true;
					this.IncludeLiterals = false;
				}
				else if (value == MaskFormat.IncludeLiterals)
				{
					this.IncludePrompt = false;
					this.IncludeLiterals = true;
				}
				else
				{
					bool flag = value == MaskFormat.IncludePromptAndLiterals;
					this.IncludePrompt = flag;
					this.IncludeLiterals = flag;
				}
				if (text != null && text != this.TextOutput)
				{
					this.OnTextChanged(EventArgs.Empty);
				}
			}
		}

		/// <summary>Returns a string that represents the current masked text box. This method overrides <see cref="M:System.Windows.Forms.TextBoxBase.ToString" />.</summary>
		/// <returns>A <see cref="T:System.String" /> that contains information about the current <see cref="T:System.Windows.Forms.MaskedTextBox" />. The string includes the type, a simplified view of the input string, and the formatted input string.</returns>
		// Token: 0x06002BC9 RID: 11209 RVA: 0x000CBF98 File Offset: 0x000CA198
		public override string ToString()
		{
			if (this.flagState[MaskedTextBox.IS_NULL_MASK])
			{
				return base.ToString();
			}
			bool includePrompt = this.IncludePrompt;
			bool includeLiterals = this.IncludeLiterals;
			string result;
			try
			{
				this.IncludePrompt = (this.IncludeLiterals = true);
				result = base.ToString();
			}
			finally
			{
				this.IncludePrompt = includePrompt;
				this.IncludeLiterals = includeLiterals;
			}
			return result;
		}

		/// <summary>Occurs when <see cref="T:System.Windows.Forms.MaskedTextBox" /> has finished parsing the current value using the <see cref="P:System.Windows.Forms.MaskedTextBox.ValidatingType" /> property.</summary>
		// Token: 0x1400020D RID: 525
		// (add) Token: 0x06002BCA RID: 11210 RVA: 0x000CC008 File Offset: 0x000CA208
		// (remove) Token: 0x06002BCB RID: 11211 RVA: 0x000CC01B File Offset: 0x000CA21B
		[SRCategory("CatFocus")]
		[SRDescription("MaskedTextBoxTypeValidationCompletedDescr")]
		public event TypeValidationEventHandler TypeValidationCompleted
		{
			add
			{
				base.Events.AddHandler(MaskedTextBox.EVENT_VALIDATIONCOMPLETED, value);
			}
			remove
			{
				base.Events.RemoveHandler(MaskedTextBox.EVENT_VALIDATIONCOMPLETED, value);
			}
		}

		/// <summary>Gets or sets a value indicating whether the operating system-supplied password character should be used.</summary>
		/// <returns>
		///     <see langword="true" /> if the system password should be used as the prompt character; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
		/// <exception cref="T:System.InvalidOperationException">The password character specified is the same as the current prompt character, <see cref="P:System.Windows.Forms.MaskedTextBox.PromptChar" />. The two are required to be different.</exception>
		// Token: 0x17000AAA RID: 2730
		// (get) Token: 0x06002BCC RID: 11212 RVA: 0x000CC02E File Offset: 0x000CA22E
		// (set) Token: 0x06002BCD RID: 11213 RVA: 0x000CC040 File Offset: 0x000CA240
		[SRCategory("CatBehavior")]
		[SRDescription("MaskedTextBoxUseSystemPasswordCharDescr")]
		[RefreshProperties(RefreshProperties.Repaint)]
		[DefaultValue(false)]
		public bool UseSystemPasswordChar
		{
			get
			{
				return this.flagState[MaskedTextBox.USE_SYSTEM_PASSWORD_CHAR];
			}
			set
			{
				if (value != this.flagState[MaskedTextBox.USE_SYSTEM_PASSWORD_CHAR])
				{
					if (value)
					{
						if (this.SystemPasswordChar == this.PromptChar)
						{
							throw new InvalidOperationException(SR.GetString("MaskedTextBoxPasswordAndPromptCharError"));
						}
						this.maskedTextProvider.PasswordChar = this.SystemPasswordChar;
					}
					else
					{
						this.maskedTextProvider.PasswordChar = this.passwordChar;
					}
					this.flagState[MaskedTextBox.USE_SYSTEM_PASSWORD_CHAR] = value;
					if (this.flagState[MaskedTextBox.IS_NULL_MASK])
					{
						this.SetEditControlPasswordChar(this.maskedTextProvider.PasswordChar);
					}
					else
					{
						this.SetWindowText();
					}
					base.VerifyImeRestrictedModeChanged();
				}
			}
		}

		/// <summary>Gets or sets the data type used to verify the data input by the user. </summary>
		/// <returns>A <see cref="T:System.Type" /> representing the data type used in validation. The default is <see langword="null" />.</returns>
		// Token: 0x17000AAB RID: 2731
		// (get) Token: 0x06002BCE RID: 11214 RVA: 0x000CC0EA File Offset: 0x000CA2EA
		// (set) Token: 0x06002BCF RID: 11215 RVA: 0x000CC0F2 File Offset: 0x000CA2F2
		[Browsable(false)]
		[DefaultValue(null)]
		public Type ValidatingType
		{
			get
			{
				return this.validatingType;
			}
			set
			{
				if (this.validatingType != value)
				{
					this.validatingType = value;
				}
			}
		}

		/// <summary>Gets or sets a value indicating whether a multiline text box control automatically wraps words to the beginning of the next line when necessary. This property is not supported by <see cref="T:System.Windows.Forms.MaskedTextBox" />. </summary>
		/// <returns>The <see cref="P:System.Windows.Forms.MaskedTextBox.WordWrap" /> property always returns <see langword="false" />. </returns>
		// Token: 0x17000AAC RID: 2732
		// (get) Token: 0x06002BD0 RID: 11216 RVA: 0x0000E0A4 File Offset: 0x0000C2A4
		// (set) Token: 0x06002BD1 RID: 11217 RVA: 0x0000701A File Offset: 0x0000521A
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new bool WordWrap
		{
			get
			{
				return false;
			}
			set
			{
			}
		}

		/// <summary>Clears information about the most recent operation from the undo buffer of the text box. This method is not supported by <see cref="T:System.Windows.Forms.MaskedTextBox" />.</summary>
		// Token: 0x06002BD2 RID: 11218 RVA: 0x0000701A File Offset: 0x0000521A
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new void ClearUndo()
		{
		}

		/// <summary>Creates a handle for the control.</summary>
		// Token: 0x06002BD3 RID: 11219 RVA: 0x000CC109 File Offset: 0x000CA309
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[UIPermission(SecurityAction.InheritanceDemand, Window = UIPermissionWindow.AllWindows)]
		protected override void CreateHandle()
		{
			if (!this.flagState[MaskedTextBox.IS_NULL_MASK] && base.RecreatingHandle)
			{
				this.SetWindowText(this.GetFormattedDisplayString(), false, false);
			}
			base.CreateHandle();
		}

		// Token: 0x06002BD4 RID: 11220 RVA: 0x000CC13C File Offset: 0x000CA33C
		private void Delete(Keys keyCode, int startPosition, int selectionLen)
		{
			this.caretTestPos = startPosition;
			if (selectionLen == 0)
			{
				if (keyCode == Keys.Back)
				{
					if (startPosition == 0)
					{
						return;
					}
					startPosition--;
				}
				else if (startPosition + selectionLen == this.maskedTextProvider.Length)
				{
					return;
				}
			}
			int endPosition = (selectionLen > 0) ? (startPosition + selectionLen - 1) : startPosition;
			string textOutput = this.TextOutput;
			int position;
			MaskedTextResultHint maskedTextResultHint;
			if (this.maskedTextProvider.RemoveAt(startPosition, endPosition, out position, out maskedTextResultHint))
			{
				if (this.TextOutput != textOutput)
				{
					this.SetText();
					this.caretTestPos = startPosition;
				}
				else if (selectionLen > 0)
				{
					this.caretTestPos = startPosition;
				}
				else if (maskedTextResultHint == MaskedTextResultHint.NoEffect)
				{
					if (keyCode == Keys.Delete)
					{
						this.caretTestPos = this.maskedTextProvider.FindEditPositionFrom(startPosition, true);
					}
					else
					{
						if (this.maskedTextProvider.FindAssignedEditPositionFrom(startPosition, true) == MaskedTextProvider.InvalidIndex)
						{
							this.caretTestPos = this.maskedTextProvider.FindAssignedEditPositionFrom(startPosition, false);
						}
						else
						{
							this.caretTestPos = this.maskedTextProvider.FindEditPositionFrom(startPosition, false);
						}
						if (this.caretTestPos != MaskedTextProvider.InvalidIndex)
						{
							this.caretTestPos++;
						}
					}
					if (this.caretTestPos == MaskedTextProvider.InvalidIndex)
					{
						this.caretTestPos = startPosition;
					}
				}
				else if (keyCode == Keys.Back)
				{
					this.caretTestPos = startPosition;
				}
			}
			else
			{
				this.OnMaskInputRejected(new MaskInputRejectedEventArgs(position, maskedTextResultHint));
			}
			base.SelectInternal(this.caretTestPos, 0, this.maskedTextProvider.Length);
		}

		/// <summary>Retrieves the character that is closest to the specified location within the control.</summary>
		/// <param name="pt">The location from which to seek the nearest character.</param>
		/// <returns>The character at the specified location.</returns>
		// Token: 0x06002BD5 RID: 11221 RVA: 0x000CC290 File Offset: 0x000CA490
		public override char GetCharFromPosition(Point pt)
		{
			this.flagState[MaskedTextBox.QUERY_BASE_TEXT] = true;
			char charFromPosition;
			try
			{
				charFromPosition = base.GetCharFromPosition(pt);
			}
			finally
			{
				this.flagState[MaskedTextBox.QUERY_BASE_TEXT] = false;
			}
			return charFromPosition;
		}

		/// <summary>Retrieves the index of the character nearest to the specified location.</summary>
		/// <param name="pt">The location to search.</param>
		/// <returns>The zero-based character index at the specified location.</returns>
		// Token: 0x06002BD6 RID: 11222 RVA: 0x000CC2DC File Offset: 0x000CA4DC
		public override int GetCharIndexFromPosition(Point pt)
		{
			this.flagState[MaskedTextBox.QUERY_BASE_TEXT] = true;
			int charIndexFromPosition;
			try
			{
				charIndexFromPosition = base.GetCharIndexFromPosition(pt);
			}
			finally
			{
				this.flagState[MaskedTextBox.QUERY_BASE_TEXT] = false;
			}
			return charIndexFromPosition;
		}

		// Token: 0x06002BD7 RID: 11223 RVA: 0x000CC328 File Offset: 0x000CA528
		internal override int GetEndPosition()
		{
			if (this.flagState[MaskedTextBox.IS_NULL_MASK])
			{
				return base.GetEndPosition();
			}
			int num = this.maskedTextProvider.FindEditPositionFrom(this.maskedTextProvider.LastAssignedPosition + 1, true);
			if (num == MaskedTextProvider.InvalidIndex)
			{
				num = this.maskedTextProvider.LastAssignedPosition + 1;
			}
			return num;
		}

		/// <summary>Retrieves the index of the first character of the current line. This method is not supported by <see cref="T:System.Windows.Forms.MaskedTextBox" />. </summary>
		/// <returns>This method will always return 0. </returns>
		// Token: 0x06002BD8 RID: 11224 RVA: 0x0000E0A4 File Offset: 0x0000C2A4
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new int GetFirstCharIndexOfCurrentLine()
		{
			return 0;
		}

		/// <summary>Retrieves the index of the first character of a given line. This method is not supported by <see cref="T:System.Windows.Forms.MaskedTextBox" />. </summary>
		/// <param name="lineNumber">This parameter is not used.</param>
		/// <returns>This method will always return 0. </returns>
		// Token: 0x06002BD9 RID: 11225 RVA: 0x0000E0A4 File Offset: 0x0000C2A4
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new int GetFirstCharIndexFromLine(int lineNumber)
		{
			return 0;
		}

		// Token: 0x06002BDA RID: 11226 RVA: 0x000CC380 File Offset: 0x000CA580
		private string GetFormattedDisplayString()
		{
			bool includePrompt = !this.ReadOnly && (base.DesignMode || !this.HidePromptOnLeave || this.Focused);
			return this.maskedTextProvider.ToString(false, includePrompt, true, 0, this.maskedTextProvider.Length);
		}

		/// <summary>Retrieves the line number from the specified character position within the text of the control. This method is not supported by <see cref="T:System.Windows.Forms.MaskedTextBox" />. </summary>
		/// <param name="index">This parameter is not used.</param>
		/// <returns>This method will always return 0.</returns>
		// Token: 0x06002BDB RID: 11227 RVA: 0x0000E0A4 File Offset: 0x0000C2A4
		[EditorBrowsable(EditorBrowsableState.Never)]
		public override int GetLineFromCharIndex(int index)
		{
			return 0;
		}

		/// <summary>Retrieves the location within the control at the specified character index.</summary>
		/// <param name="index">The index of the character for which to retrieve the location.</param>
		/// <returns>The location of the specified character within the client rectangle of the control.</returns>
		// Token: 0x06002BDC RID: 11228 RVA: 0x000CC3D4 File Offset: 0x000CA5D4
		public override Point GetPositionFromCharIndex(int index)
		{
			this.flagState[MaskedTextBox.QUERY_BASE_TEXT] = true;
			Point positionFromCharIndex;
			try
			{
				positionFromCharIndex = base.GetPositionFromCharIndex(index);
			}
			finally
			{
				this.flagState[MaskedTextBox.QUERY_BASE_TEXT] = false;
			}
			return positionFromCharIndex;
		}

		// Token: 0x06002BDD RID: 11229 RVA: 0x000CC420 File Offset: 0x000CA620
		internal override Size GetPreferredSizeCore(Size proposedConstraints)
		{
			this.flagState[MaskedTextBox.QUERY_BASE_TEXT] = true;
			Size preferredSizeCore;
			try
			{
				preferredSizeCore = base.GetPreferredSizeCore(proposedConstraints);
			}
			finally
			{
				this.flagState[MaskedTextBox.QUERY_BASE_TEXT] = false;
			}
			return preferredSizeCore;
		}

		// Token: 0x06002BDE RID: 11230 RVA: 0x000CC46C File Offset: 0x000CA66C
		private string GetSelectedText()
		{
			int startPosition;
			int num;
			base.GetSelectionStartAndLength(out startPosition, out num);
			if (num == 0)
			{
				return string.Empty;
			}
			bool includePrompt = (this.CutCopyMaskFormat & MaskFormat.IncludePrompt) > MaskFormat.ExcludePromptAndLiterals;
			bool includeLiterals = (this.CutCopyMaskFormat & MaskFormat.IncludeLiterals) > MaskFormat.ExcludePromptAndLiterals;
			return this.maskedTextProvider.ToString(true, includePrompt, includeLiterals, startPosition, num);
		}

		/// <summary>Occurs when the value of the <see cref="P:System.Windows.Forms.Control.BackColor" /> property changes.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
		// Token: 0x06002BDF RID: 11231 RVA: 0x000CC4B4 File Offset: 0x000CA6B4
		protected override void OnBackColorChanged(EventArgs e)
		{
			base.OnBackColorChanged(e);
			if (Application.RenderWithVisualStyles && base.IsHandleCreated && base.BorderStyle == BorderStyle.Fixed3D)
			{
				SafeNativeMethods.RedrawWindow(new HandleRef(this, base.Handle), null, NativeMethods.NullHandleRef, 1025);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.HandleCreated" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x06002BE0 RID: 11232 RVA: 0x000CC4F2 File Offset: 0x000CA6F2
		protected override void OnHandleCreated(EventArgs e)
		{
			base.OnHandleCreated(e);
			base.SetSelectionOnHandle();
			if (this.flagState[MaskedTextBox.IS_NULL_MASK] && this.maskedTextProvider.IsPassword)
			{
				this.SetEditControlPasswordChar(this.maskedTextProvider.PasswordChar);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.MaskedTextBox.IsOverwriteModeChanged" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains event data. </param>
		// Token: 0x06002BE1 RID: 11233 RVA: 0x000CC534 File Offset: 0x000CA734
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void OnIsOverwriteModeChanged(EventArgs e)
		{
			EventHandler eventHandler = base.Events[MaskedTextBox.EVENT_ISOVERWRITEMODECHANGED] as EventHandler;
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.KeyDown" /> event.</summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.KeyEventArgs" /> that contains the event data.</param>
		// Token: 0x06002BE2 RID: 11234 RVA: 0x000CC564 File Offset: 0x000CA764
		protected override void OnKeyDown(KeyEventArgs e)
		{
			base.OnKeyDown(e);
			if (this.flagState[MaskedTextBox.IS_NULL_MASK])
			{
				return;
			}
			Keys keys = e.KeyCode;
			if (keys == Keys.Return || keys == Keys.Escape)
			{
				this.flagState[MaskedTextBox.HANDLE_KEY_PRESS] = false;
			}
			if (keys == Keys.Insert && e.Modifiers == Keys.None && this.insertMode == InsertKeyMode.Default)
			{
				this.flagState[MaskedTextBox.INSERT_TOGGLED] = !this.flagState[MaskedTextBox.INSERT_TOGGLED];
				this.OnIsOverwriteModeChanged(EventArgs.Empty);
				return;
			}
			if (e.Control && char.IsLetter((char)keys))
			{
				if (keys != Keys.H)
				{
					this.flagState[MaskedTextBox.HANDLE_KEY_PRESS] = false;
					return;
				}
				keys = Keys.Back;
			}
			if ((keys == Keys.Delete || keys == Keys.Back) && !this.ReadOnly)
			{
				int num;
				int num2;
				base.GetSelectionStartAndLength(out num, out num2);
				Keys modifiers = e.Modifiers;
				if (modifiers != Keys.Shift)
				{
					if (modifiers == Keys.Control)
					{
						if (num2 == 0)
						{
							if (keys == Keys.Delete)
							{
								num2 = this.maskedTextProvider.Length - num;
							}
							else
							{
								num2 = ((num == this.maskedTextProvider.Length) ? num : (num + 1));
								num = 0;
							}
						}
					}
				}
				else if (keys == Keys.Delete)
				{
					keys = Keys.Back;
				}
				if (!this.flagState[MaskedTextBox.HANDLE_KEY_PRESS])
				{
					this.flagState[MaskedTextBox.HANDLE_KEY_PRESS] = true;
				}
				this.Delete(keys, num, num2);
				e.SuppressKeyPress = true;
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.KeyPress" /> event.</summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.KeyPressEventArgs" /> that contains the event data.</param>
		// Token: 0x06002BE3 RID: 11235 RVA: 0x000CC6C4 File Offset: 0x000CA8C4
		protected override void OnKeyPress(KeyPressEventArgs e)
		{
			base.OnKeyPress(e);
			if (this.flagState[MaskedTextBox.IS_NULL_MASK])
			{
				return;
			}
			if (!this.flagState[MaskedTextBox.HANDLE_KEY_PRESS])
			{
				this.flagState[MaskedTextBox.HANDLE_KEY_PRESS] = true;
				if (!char.IsLetter(e.KeyChar))
				{
					return;
				}
			}
			if (!this.ReadOnly)
			{
				int startPosition;
				int num;
				base.GetSelectionStartAndLength(out startPosition, out num);
				string textOutput = this.TextOutput;
				MaskedTextResultHint rejectionHint;
				if (this.PlaceChar(e.KeyChar, startPosition, num, this.IsOverwriteMode, out rejectionHint))
				{
					if (this.TextOutput != textOutput)
					{
						this.SetText();
					}
					int selectionStart = this.caretTestPos + 1;
					this.caretTestPos = selectionStart;
					base.SelectionStart = selectionStart;
					if (ImeModeConversion.InputLanguageTable == ImeModeConversion.KoreanTable)
					{
						int num2 = this.maskedTextProvider.FindUnassignedEditPositionFrom(this.caretTestPos, true);
						if (num2 == MaskedTextProvider.InvalidIndex)
						{
							this.ImeComplete();
						}
					}
				}
				else
				{
					this.OnMaskInputRejected(new MaskInputRejectedEventArgs(this.caretTestPos, rejectionHint));
				}
				if (num > 0)
				{
					this.SelectionLength = 0;
				}
				e.Handled = true;
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.KeyUp" /> event.</summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.KeyEventArgs" /> that contains the event data.</param>
		// Token: 0x06002BE4 RID: 11236 RVA: 0x000CC7D4 File Offset: 0x000CA9D4
		protected override void OnKeyUp(KeyEventArgs e)
		{
			base.OnKeyUp(e);
			if (this.flagState[MaskedTextBox.IME_COMPLETING])
			{
				this.flagState[MaskedTextBox.IME_COMPLETING] = false;
			}
			if (this.flagState[MaskedTextBox.IME_ENDING_COMPOSITION])
			{
				this.flagState[MaskedTextBox.IME_ENDING_COMPOSITION] = false;
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.MaskedTextBox.MaskChanged" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains event data. </param>
		// Token: 0x06002BE5 RID: 11237 RVA: 0x000CC830 File Offset: 0x000CAA30
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void OnMaskChanged(EventArgs e)
		{
			EventHandler eventHandler = base.Events[MaskedTextBox.EVENT_MASKCHANGED] as EventHandler;
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		// Token: 0x06002BE6 RID: 11238 RVA: 0x000CC860 File Offset: 0x000CAA60
		private void OnMaskInputRejected(MaskInputRejectedEventArgs e)
		{
			if (this.BeepOnError)
			{
				SoundPlayer soundPlayer = new SoundPlayer();
				soundPlayer.Play();
			}
			MaskInputRejectedEventHandler maskInputRejectedEventHandler = base.Events[MaskedTextBox.EVENT_MASKINPUTREJECTED] as MaskInputRejectedEventHandler;
			if (maskInputRejectedEventHandler != null)
			{
				maskInputRejectedEventHandler(this, e);
			}
		}

		/// <summary>Typically raises the <see cref="E:System.Windows.Forms.MaskedTextBox.MultilineChanged" /> event, but disabled for <see cref="T:System.Windows.Forms.MaskedTextBox" />.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains event data. </param>
		// Token: 0x06002BE7 RID: 11239 RVA: 0x0000701A File Offset: 0x0000521A
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void OnMultilineChanged(EventArgs e)
		{
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.MaskedTextBox.TextAlignChanged" /> event. </summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains event data. </param>
		// Token: 0x06002BE8 RID: 11240 RVA: 0x000CC8A4 File Offset: 0x000CAAA4
		protected virtual void OnTextAlignChanged(EventArgs e)
		{
			EventHandler eventHandler = base.Events[MaskedTextBox.EVENT_TEXTALIGNCHANGED] as EventHandler;
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		// Token: 0x06002BE9 RID: 11241 RVA: 0x000CC8D4 File Offset: 0x000CAAD4
		private void OnTypeValidationCompleted(TypeValidationEventArgs e)
		{
			TypeValidationEventHandler typeValidationEventHandler = base.Events[MaskedTextBox.EVENT_VALIDATIONCOMPLETED] as TypeValidationEventHandler;
			if (typeValidationEventHandler != null)
			{
				typeValidationEventHandler(this, e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.Validating" /> event.</summary>
		/// <param name="e">A <see cref="T:System.ComponentModel.CancelEventArgs" /> that contains event data. </param>
		/// <exception cref="T:System.Exception">A critical exception occurred during the parsing of the input string.</exception>
		// Token: 0x06002BEA RID: 11242 RVA: 0x000CC902 File Offset: 0x000CAB02
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected override void OnValidating(CancelEventArgs e)
		{
			this.PerformTypeValidation(e);
			base.OnValidating(e);
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.TextChanged" /> event. </summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains event data. </param>
		// Token: 0x06002BEB RID: 11243 RVA: 0x000CC914 File Offset: 0x000CAB14
		protected override void OnTextChanged(EventArgs e)
		{
			bool value = this.flagState[MaskedTextBox.QUERY_BASE_TEXT];
			this.flagState[MaskedTextBox.QUERY_BASE_TEXT] = false;
			try
			{
				base.OnTextChanged(e);
			}
			finally
			{
				this.flagState[MaskedTextBox.QUERY_BASE_TEXT] = value;
			}
		}

		// Token: 0x06002BEC RID: 11244 RVA: 0x000CC970 File Offset: 0x000CAB70
		private void Replace(string text, int startPosition, int selectionLen)
		{
			MaskedTextProvider maskedTextProvider = (MaskedTextProvider)this.maskedTextProvider.Clone();
			int num = this.caretTestPos;
			MaskedTextResultHint maskedTextResultHint = MaskedTextResultHint.NoEffect;
			int num2 = startPosition + selectionLen - 1;
			if (this.RejectInputOnFirstFailure)
			{
				if (!((startPosition > num2) ? maskedTextProvider.InsertAt(text, startPosition, out this.caretTestPos, out maskedTextResultHint) : maskedTextProvider.Replace(text, startPosition, num2, out this.caretTestPos, out maskedTextResultHint)))
				{
					this.OnMaskInputRejected(new MaskInputRejectedEventArgs(this.caretTestPos, maskedTextResultHint));
				}
			}
			else
			{
				MaskedTextResultHint maskedTextResultHint2 = maskedTextResultHint;
				int i = 0;
				while (i < text.Length)
				{
					char c = text[i];
					if (this.maskedTextProvider.VerifyEscapeChar(c, startPosition))
					{
						goto IL_BF;
					}
					int num3 = maskedTextProvider.FindEditPositionFrom(startPosition, true);
					if (num3 != MaskedTextProvider.InvalidIndex)
					{
						startPosition = num3;
						goto IL_BF;
					}
					this.OnMaskInputRejected(new MaskInputRejectedEventArgs(startPosition, MaskedTextResultHint.UnavailableEditPosition));
					IL_109:
					i++;
					continue;
					IL_BF:
					int num4 = (num2 >= startPosition) ? 1 : 0;
					bool overwrite = num4 > 0;
					if (!this.PlaceChar(maskedTextProvider, c, startPosition, num4, overwrite, out maskedTextResultHint2))
					{
						this.OnMaskInputRejected(new MaskInputRejectedEventArgs(startPosition, maskedTextResultHint2));
						goto IL_109;
					}
					startPosition = this.caretTestPos + 1;
					if (maskedTextResultHint2 == MaskedTextResultHint.Success && maskedTextResultHint != maskedTextResultHint2)
					{
						maskedTextResultHint = maskedTextResultHint2;
						goto IL_109;
					}
					goto IL_109;
				}
				if (selectionLen > 0 && startPosition <= num2)
				{
					if (!maskedTextProvider.RemoveAt(startPosition, num2, out this.caretTestPos, out maskedTextResultHint2))
					{
						this.OnMaskInputRejected(new MaskInputRejectedEventArgs(this.caretTestPos, maskedTextResultHint2));
					}
					if (maskedTextResultHint == MaskedTextResultHint.NoEffect && maskedTextResultHint != maskedTextResultHint2)
					{
						maskedTextResultHint = maskedTextResultHint2;
					}
				}
			}
			bool flag = this.TextOutput != maskedTextProvider.ToString();
			this.maskedTextProvider = maskedTextProvider;
			if (flag)
			{
				this.SetText();
				this.caretTestPos = startPosition;
				base.SelectInternal(this.caretTestPos, 0, this.maskedTextProvider.Length);
				return;
			}
			this.caretTestPos = num;
		}

		// Token: 0x06002BED RID: 11245 RVA: 0x000CCB20 File Offset: 0x000CAD20
		private void PasteInt(string text)
		{
			int startPosition;
			int selectionLen;
			base.GetSelectionStartAndLength(out startPosition, out selectionLen);
			if (string.IsNullOrEmpty(text))
			{
				this.Delete(Keys.Delete, startPosition, selectionLen);
				return;
			}
			this.Replace(text, startPosition, selectionLen);
		}

		// Token: 0x06002BEE RID: 11246 RVA: 0x000CCB54 File Offset: 0x000CAD54
		private object PerformTypeValidation(CancelEventArgs e)
		{
			object obj = null;
			if (this.validatingType != null)
			{
				string text = null;
				if (!this.flagState[MaskedTextBox.IS_NULL_MASK] && !this.maskedTextProvider.MaskCompleted)
				{
					text = SR.GetString("MaskedTextBoxIncompleteMsg");
				}
				else
				{
					string value;
					if (!this.flagState[MaskedTextBox.IS_NULL_MASK])
					{
						value = this.maskedTextProvider.ToString(false, this.IncludeLiterals);
					}
					else
					{
						value = base.Text;
					}
					try
					{
						obj = Formatter.ParseObject(value, this.validatingType, typeof(string), null, null, this.formatProvider, null, Formatter.GetDefaultDataSourceNullValue(this.validatingType));
					}
					catch (Exception innerException)
					{
						if (ClientUtils.IsSecurityOrCriticalException(innerException))
						{
							throw;
						}
						if (innerException.InnerException != null)
						{
							innerException = innerException.InnerException;
						}
						text = innerException.GetType().ToString() + ": " + innerException.Message;
					}
				}
				bool isValidInput = false;
				if (text == null)
				{
					isValidInput = true;
					text = SR.GetString("MaskedTextBoxTypeValidationSucceeded");
				}
				TypeValidationEventArgs typeValidationEventArgs = new TypeValidationEventArgs(this.validatingType, isValidInput, obj, text);
				this.OnTypeValidationCompleted(typeValidationEventArgs);
				if (e != null)
				{
					e.Cancel = typeValidationEventArgs.Cancel;
				}
			}
			return obj;
		}

		// Token: 0x06002BEF RID: 11247 RVA: 0x000CCC8C File Offset: 0x000CAE8C
		private bool PlaceChar(char ch, int startPosition, int length, bool overwrite, out MaskedTextResultHint hint)
		{
			return this.PlaceChar(this.maskedTextProvider, ch, startPosition, length, overwrite, out hint);
		}

		// Token: 0x06002BF0 RID: 11248 RVA: 0x000CCCA4 File Offset: 0x000CAEA4
		private bool PlaceChar(MaskedTextProvider provider, char ch, int startPosition, int length, bool overwrite, out MaskedTextResultHint hint)
		{
			this.caretTestPos = startPosition;
			if (startPosition >= this.maskedTextProvider.Length)
			{
				hint = MaskedTextResultHint.UnavailableEditPosition;
				return false;
			}
			if (length > 0)
			{
				int endPosition = startPosition + length - 1;
				return provider.Replace(ch, startPosition, endPosition, out this.caretTestPos, out hint);
			}
			if (overwrite)
			{
				return provider.Replace(ch, startPosition, out this.caretTestPos, out hint);
			}
			return provider.InsertAt(ch, startPosition, out this.caretTestPos, out hint);
		}

		/// <summary>Processes a command key.</summary>
		/// <param name="msg">A <see cref="T:System.Windows.Forms.Message" />, passed by reference that represents the window message to process. </param>
		/// <param name="keyData">One of the <see cref="T:System.Windows.Forms.Keys" /> values that represents the shortcut key to process. </param>
		/// <returns>
		///     <see langword="true" /> if the command key was processed by the control; otherwise, <see langword="false" />.</returns>
		// Token: 0x06002BF1 RID: 11249 RVA: 0x000CCD10 File Offset: 0x000CAF10
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
		{
			bool flag = base.ProcessCmdKey(ref msg, keyData);
			if (!flag && keyData == (Keys)131137)
			{
				base.SelectAll();
				flag = true;
			}
			return flag;
		}

		/// <summary>Overrides the base implementation of this method to handle input language changes.</summary>
		/// <param name="m">A <see cref="T:System.Windows.Forms.Message" />, passed by reference that represents the window message to process.</param>
		/// <returns>
		///     <see langword="true" /> if the message was processed by the control; otherwise, <see langword="false" />.</returns>
		// Token: 0x06002BF2 RID: 11250 RVA: 0x000CCD3C File Offset: 0x000CAF3C
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		protected internal override bool ProcessKeyMessage(ref Message m)
		{
			bool flag = base.ProcessKeyMessage(ref m);
			if (this.flagState[MaskedTextBox.IS_NULL_MASK])
			{
				return flag;
			}
			return (m.Msg == 258 && base.ImeWmCharsToIgnore > 0) || flag;
		}

		// Token: 0x06002BF3 RID: 11251 RVA: 0x000CCD7E File Offset: 0x000CAF7E
		private void ResetCulture()
		{
			this.Culture = CultureInfo.CurrentCulture;
		}

		/// <summary>Scrolls the contents of the control to the current caret position. This method is not supported by <see cref="T:System.Windows.Forms.MaskedTextBox" />.</summary>
		// Token: 0x06002BF4 RID: 11252 RVA: 0x0000701A File Offset: 0x0000521A
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new void ScrollToCaret()
		{
		}

		// Token: 0x06002BF5 RID: 11253 RVA: 0x000CCD8B File Offset: 0x000CAF8B
		private void SetMaskedTextProvider(MaskedTextProvider newProvider)
		{
			this.SetMaskedTextProvider(newProvider, null);
		}

		// Token: 0x06002BF6 RID: 11254 RVA: 0x000CCD98 File Offset: 0x000CAF98
		private void SetMaskedTextProvider(MaskedTextProvider newProvider, string textOnInitializingMask)
		{
			newProvider.IncludePrompt = this.maskedTextProvider.IncludePrompt;
			newProvider.IncludeLiterals = this.maskedTextProvider.IncludeLiterals;
			newProvider.SkipLiterals = this.maskedTextProvider.SkipLiterals;
			newProvider.ResetOnPrompt = this.maskedTextProvider.ResetOnPrompt;
			newProvider.ResetOnSpace = this.maskedTextProvider.ResetOnSpace;
			if (this.flagState[MaskedTextBox.IS_NULL_MASK] && textOnInitializingMask == null)
			{
				this.maskedTextProvider = newProvider;
				return;
			}
			int position = 0;
			MaskedTextResultHint maskedTextResultHint = MaskedTextResultHint.NoEffect;
			MaskedTextProvider maskedTextProvider = this.maskedTextProvider;
			bool flag = maskedTextProvider.Mask == newProvider.Mask;
			string a;
			bool flag2;
			if (textOnInitializingMask != null)
			{
				a = textOnInitializingMask;
				flag2 = !newProvider.Set(textOnInitializingMask, out position, out maskedTextResultHint);
			}
			else
			{
				a = this.TextOutput;
				int i = maskedTextProvider.AssignedEditPositionCount;
				int num = 0;
				int num2 = 0;
				while (i > 0)
				{
					num = maskedTextProvider.FindAssignedEditPositionFrom(num, true);
					if (flag)
					{
						num2 = num;
					}
					else
					{
						num2 = newProvider.FindEditPositionFrom(num2, true);
						if (num2 == MaskedTextProvider.InvalidIndex)
						{
							newProvider.Clear();
							position = newProvider.Length;
							maskedTextResultHint = MaskedTextResultHint.UnavailableEditPosition;
							break;
						}
					}
					if (!newProvider.Replace(maskedTextProvider[num], num2, out position, out maskedTextResultHint))
					{
						flag = false;
						newProvider.Clear();
						break;
					}
					num++;
					num2++;
					i--;
				}
				flag2 = !MaskedTextProvider.GetOperationResultFromHint(maskedTextResultHint);
			}
			this.maskedTextProvider = newProvider;
			if (this.flagState[MaskedTextBox.IS_NULL_MASK])
			{
				this.flagState[MaskedTextBox.IS_NULL_MASK] = false;
			}
			if (flag2)
			{
				this.OnMaskInputRejected(new MaskInputRejectedEventArgs(position, maskedTextResultHint));
			}
			if (newProvider.IsPassword)
			{
				this.SetEditControlPasswordChar('\0');
			}
			EventArgs empty = EventArgs.Empty;
			if (textOnInitializingMask != null || maskedTextProvider.Mask != newProvider.Mask)
			{
				this.OnMaskChanged(empty);
			}
			this.SetWindowText(this.GetFormattedDisplayString(), a != this.TextOutput, flag);
		}

		// Token: 0x06002BF7 RID: 11255 RVA: 0x000CCF6F File Offset: 0x000CB16F
		private void SetText()
		{
			this.SetWindowText(this.GetFormattedDisplayString(), true, false);
		}

		// Token: 0x06002BF8 RID: 11256 RVA: 0x000CCF7F File Offset: 0x000CB17F
		private void SetWindowText()
		{
			this.SetWindowText(this.GetFormattedDisplayString(), false, true);
		}

		// Token: 0x06002BF9 RID: 11257 RVA: 0x000CCF90 File Offset: 0x000CB190
		private void SetWindowText(string text, bool raiseTextChangedEvent, bool preserveCaret)
		{
			this.flagState[MaskedTextBox.QUERY_BASE_TEXT] = true;
			try
			{
				if (preserveCaret)
				{
					this.caretTestPos = base.SelectionStart;
				}
				this.WindowText = text;
				if (raiseTextChangedEvent)
				{
					this.OnTextChanged(EventArgs.Empty);
				}
				if (preserveCaret)
				{
					base.SelectionStart = this.caretTestPos;
				}
			}
			finally
			{
				this.flagState[MaskedTextBox.QUERY_BASE_TEXT] = false;
			}
		}

		// Token: 0x06002BFA RID: 11258 RVA: 0x000CD008 File Offset: 0x000CB208
		private bool ShouldSerializeCulture()
		{
			return !CultureInfo.CurrentCulture.Equals(this.Culture);
		}

		/// <summary>Undoes the last edit operation in the text box. This method is not supported by <see cref="T:System.Windows.Forms.MaskedTextBox" />.</summary>
		// Token: 0x06002BFB RID: 11259 RVA: 0x0000701A File Offset: 0x0000521A
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new void Undo()
		{
		}

		/// <summary>Converts the user input string to an instance of the validating type.</summary>
		/// <returns>If successful, an <see cref="T:System.Object" /> of the type specified by the <see cref="P:System.Windows.Forms.MaskedTextBox.ValidatingType" /> property; otherwise, <see langword="null" /> to indicate conversion failure.</returns>
		/// <exception cref="T:System.Exception">A critical exception occurred during the parsing of the input string.</exception>
		// Token: 0x06002BFC RID: 11260 RVA: 0x000CD01D File Offset: 0x000CB21D
		public object ValidateText()
		{
			return this.PerformTypeValidation(null);
		}

		// Token: 0x06002BFD RID: 11261 RVA: 0x000CD028 File Offset: 0x000CB228
		private bool WmClear()
		{
			if (!this.ReadOnly)
			{
				int startPosition;
				int selectionLen;
				base.GetSelectionStartAndLength(out startPosition, out selectionLen);
				this.Delete(Keys.Delete, startPosition, selectionLen);
				return true;
			}
			return false;
		}

		// Token: 0x06002BFE RID: 11262 RVA: 0x000CD054 File Offset: 0x000CB254
		private bool WmCopy()
		{
			if (this.maskedTextProvider.IsPassword)
			{
				return false;
			}
			string selectedText = this.GetSelectedText();
			try
			{
				IntSecurity.ClipboardWrite.Assert();
				if (selectedText.Length == 0)
				{
					Clipboard.Clear();
				}
				else
				{
					Clipboard.SetText(selectedText);
				}
			}
			catch (Exception ex)
			{
				if (ClientUtils.IsSecurityOrCriticalException(ex))
				{
					throw;
				}
			}
			return true;
		}

		// Token: 0x06002BFF RID: 11263 RVA: 0x000CD0B8 File Offset: 0x000CB2B8
		private bool WmImeComposition(ref Message m)
		{
			if (ImeModeConversion.InputLanguageTable == ImeModeConversion.KoreanTable)
			{
				byte b = 0;
				if ((m.LParam.ToInt32() & 8) != 0)
				{
					b = 1;
				}
				else if ((m.LParam.ToInt32() & 2048) != 0)
				{
					b = 2;
				}
				if (b != 0 && this.flagState[MaskedTextBox.IME_ENDING_COMPOSITION])
				{
					return this.flagState[MaskedTextBox.IME_COMPLETING];
				}
			}
			return false;
		}

		// Token: 0x06002C00 RID: 11264 RVA: 0x000CD128 File Offset: 0x000CB328
		private bool WmImeStartComposition()
		{
			int num;
			int num2;
			base.GetSelectionStartAndLength(out num, out num2);
			int num3 = this.maskedTextProvider.FindEditPositionFrom(num, true);
			if (num3 != MaskedTextProvider.InvalidIndex)
			{
				if (num2 > 0 && ImeModeConversion.InputLanguageTable == ImeModeConversion.KoreanTable)
				{
					int num4 = this.maskedTextProvider.FindEditPositionFrom(num + num2 - 1, false);
					if (num4 < num3)
					{
						this.ImeComplete();
						this.OnMaskInputRejected(new MaskInputRejectedEventArgs(num, MaskedTextResultHint.UnavailableEditPosition));
						return true;
					}
					num2 = num4 - num3 + 1;
					this.Delete(Keys.Delete, num3, num2);
				}
				if (num != num3)
				{
					this.caretTestPos = num3;
					base.SelectionStart = this.caretTestPos;
				}
				this.SelectionLength = 0;
				return false;
			}
			this.ImeComplete();
			this.OnMaskInputRejected(new MaskInputRejectedEventArgs(num, MaskedTextResultHint.UnavailableEditPosition));
			return true;
		}

		// Token: 0x06002C01 RID: 11265 RVA: 0x000CD1DC File Offset: 0x000CB3DC
		private void WmPaste()
		{
			if (this.ReadOnly)
			{
				return;
			}
			string text;
			try
			{
				IntSecurity.ClipboardRead.Assert();
				text = Clipboard.GetText();
			}
			catch (Exception ex)
			{
				if (ClientUtils.IsSecurityOrCriticalException(ex))
				{
					throw;
				}
				return;
			}
			this.PasteInt(text);
		}

		// Token: 0x06002C02 RID: 11266 RVA: 0x000CD22C File Offset: 0x000CB42C
		private void WmPrint(ref Message m)
		{
			base.WndProc(ref m);
			if ((2 & (int)((long)m.LParam)) != 0 && Application.RenderWithVisualStyles && base.BorderStyle == BorderStyle.Fixed3D)
			{
				IntSecurity.UnmanagedCode.Assert();
				try
				{
					using (Graphics graphics = Graphics.FromHdc(m.WParam))
					{
						Rectangle rect = new Rectangle(0, 0, base.Size.Width - 1, base.Size.Height - 1);
						using (Pen pen = new Pen(VisualStyleInformation.TextControlBorder))
						{
							graphics.DrawRectangle(pen, rect);
						}
						rect.Inflate(-1, -1);
						graphics.DrawRectangle(SystemPens.Window, rect);
					}
				}
				finally
				{
					CodeAccessPermission.RevertAssert();
				}
			}
		}

		/// <summary>Processes Windows messages.</summary>
		/// <param name="m">The Windows <see cref="T:System.Windows.Forms.Message" /> to process.</param>
		// Token: 0x06002C03 RID: 11267 RVA: 0x000CD318 File Offset: 0x000CB518
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		protected override void WndProc(ref Message m)
		{
			int msg = m.Msg;
			if (msg <= 183)
			{
				if (msg != 123)
				{
					if (msg != 183)
					{
						goto IL_5D;
					}
					return;
				}
			}
			else
			{
				switch (msg)
				{
				case 197:
				case 199:
					return;
				case 198:
					break;
				default:
					if (msg == 772)
					{
						return;
					}
					if (msg == 791)
					{
						this.WmPrint(ref m);
						return;
					}
					goto IL_5D;
				}
			}
			base.ClearUndo();
			base.WndProc(ref m);
			return;
			IL_5D:
			if (this.flagState[MaskedTextBox.IS_NULL_MASK])
			{
				base.WndProc(ref m);
				return;
			}
			msg = m.Msg;
			if (msg <= 8)
			{
				if (msg == 7)
				{
					this.WmSetFocus();
					base.WndProc(ref m);
					return;
				}
				if (msg == 8)
				{
					base.WndProc(ref m);
					this.WmKillFocus();
					return;
				}
			}
			else
			{
				switch (msg)
				{
				case 269:
					if (this.WmImeStartComposition())
					{
						return;
					}
					break;
				case 270:
					this.flagState[MaskedTextBox.IME_ENDING_COMPOSITION] = true;
					break;
				case 271:
					if (this.WmImeComposition(ref m))
					{
						return;
					}
					break;
				default:
					switch (msg)
					{
					case 768:
						if (!this.ReadOnly && this.WmCopy())
						{
							this.WmClear();
							return;
						}
						return;
					case 769:
						this.WmCopy();
						return;
					case 770:
						this.WmPaste();
						return;
					case 771:
						this.WmClear();
						return;
					}
					break;
				}
			}
			base.WndProc(ref m);
		}

		// Token: 0x06002C04 RID: 11268 RVA: 0x000CD468 File Offset: 0x000CB668
		private void WmKillFocus()
		{
			base.GetSelectionStartAndLength(out this.caretTestPos, out this.lastSelLength);
			if (this.HidePromptOnLeave && !this.MaskFull)
			{
				this.SetWindowText();
				base.SelectInternal(this.caretTestPos, this.lastSelLength, this.maskedTextProvider.Length);
			}
		}

		// Token: 0x06002C05 RID: 11269 RVA: 0x000CD4BA File Offset: 0x000CB6BA
		private void WmSetFocus()
		{
			if (this.HidePromptOnLeave && !this.MaskFull)
			{
				this.SetWindowText();
			}
			base.SelectInternal(this.caretTestPos, this.lastSelLength, this.maskedTextProvider.Length);
		}

		// Token: 0x040012AE RID: 4782
		private const bool forward = true;

		// Token: 0x040012AF RID: 4783
		private const bool backward = false;

		// Token: 0x040012B0 RID: 4784
		private const string nullMask = "<>";

		// Token: 0x040012B1 RID: 4785
		private static readonly object EVENT_MASKINPUTREJECTED = new object();

		// Token: 0x040012B2 RID: 4786
		private static readonly object EVENT_VALIDATIONCOMPLETED = new object();

		// Token: 0x040012B3 RID: 4787
		private static readonly object EVENT_TEXTALIGNCHANGED = new object();

		// Token: 0x040012B4 RID: 4788
		private static readonly object EVENT_ISOVERWRITEMODECHANGED = new object();

		// Token: 0x040012B5 RID: 4789
		private static readonly object EVENT_MASKCHANGED = new object();

		// Token: 0x040012B6 RID: 4790
		private static char systemPwdChar;

		// Token: 0x040012B7 RID: 4791
		private const byte imeConvertionNone = 0;

		// Token: 0x040012B8 RID: 4792
		private const byte imeConvertionUpdate = 1;

		// Token: 0x040012B9 RID: 4793
		private const byte imeConvertionCompleted = 2;

		// Token: 0x040012BA RID: 4794
		private int lastSelLength;

		// Token: 0x040012BB RID: 4795
		private int caretTestPos;

		// Token: 0x040012BC RID: 4796
		private static int IME_ENDING_COMPOSITION = BitVector32.CreateMask();

		// Token: 0x040012BD RID: 4797
		private static int IME_COMPLETING = BitVector32.CreateMask(MaskedTextBox.IME_ENDING_COMPOSITION);

		// Token: 0x040012BE RID: 4798
		private static int HANDLE_KEY_PRESS = BitVector32.CreateMask(MaskedTextBox.IME_COMPLETING);

		// Token: 0x040012BF RID: 4799
		private static int IS_NULL_MASK = BitVector32.CreateMask(MaskedTextBox.HANDLE_KEY_PRESS);

		// Token: 0x040012C0 RID: 4800
		private static int QUERY_BASE_TEXT = BitVector32.CreateMask(MaskedTextBox.IS_NULL_MASK);

		// Token: 0x040012C1 RID: 4801
		private static int REJECT_INPUT_ON_FIRST_FAILURE = BitVector32.CreateMask(MaskedTextBox.QUERY_BASE_TEXT);

		// Token: 0x040012C2 RID: 4802
		private static int HIDE_PROMPT_ON_LEAVE = BitVector32.CreateMask(MaskedTextBox.REJECT_INPUT_ON_FIRST_FAILURE);

		// Token: 0x040012C3 RID: 4803
		private static int BEEP_ON_ERROR = BitVector32.CreateMask(MaskedTextBox.HIDE_PROMPT_ON_LEAVE);

		// Token: 0x040012C4 RID: 4804
		private static int USE_SYSTEM_PASSWORD_CHAR = BitVector32.CreateMask(MaskedTextBox.BEEP_ON_ERROR);

		// Token: 0x040012C5 RID: 4805
		private static int INSERT_TOGGLED = BitVector32.CreateMask(MaskedTextBox.USE_SYSTEM_PASSWORD_CHAR);

		// Token: 0x040012C6 RID: 4806
		private static int CUTCOPYINCLUDEPROMPT = BitVector32.CreateMask(MaskedTextBox.INSERT_TOGGLED);

		// Token: 0x040012C7 RID: 4807
		private static int CUTCOPYINCLUDELITERALS = BitVector32.CreateMask(MaskedTextBox.CUTCOPYINCLUDEPROMPT);

		// Token: 0x040012C8 RID: 4808
		private char passwordChar;

		// Token: 0x040012C9 RID: 4809
		private Type validatingType;

		// Token: 0x040012CA RID: 4810
		private IFormatProvider formatProvider;

		// Token: 0x040012CB RID: 4811
		private MaskedTextProvider maskedTextProvider;

		// Token: 0x040012CC RID: 4812
		private InsertKeyMode insertMode;

		// Token: 0x040012CD RID: 4813
		private HorizontalAlignment textAlign;

		// Token: 0x040012CE RID: 4814
		private BitVector32 flagState;
	}
}
