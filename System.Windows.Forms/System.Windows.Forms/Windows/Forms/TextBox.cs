using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;
using System.Windows.Forms.VisualStyles;

namespace System.Windows.Forms
{
	/// <summary>Represents a Windows text box control.</summary>
	// Token: 0x0200038F RID: 911
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[ComVisible(true)]
	[Designer("System.Windows.Forms.Design.TextBoxDesigner, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	[SRDescription("DescriptionTextBox")]
	public class TextBox : TextBoxBase
	{
		/// <summary>Gets or sets a value indicating whether pressing ENTER in a multiline <see cref="T:System.Windows.Forms.TextBox" /> control creates a new line of text in the control or activates the default button for the form.</summary>
		/// <returns>
		///     <see langword="true" /> if the ENTER key creates a new line of text in a multiline version of the control; <see langword="false" /> if the ENTER key activates the default button for the form. The default is <see langword="false" />.</returns>
		// Token: 0x17000E4E RID: 3662
		// (get) Token: 0x06003949 RID: 14665 RVA: 0x000FF06A File Offset: 0x000FD26A
		// (set) Token: 0x0600394A RID: 14666 RVA: 0x000FF072 File Offset: 0x000FD272
		[SRCategory("CatBehavior")]
		[DefaultValue(false)]
		[SRDescription("TextBoxAcceptsReturnDescr")]
		public bool AcceptsReturn
		{
			get
			{
				return this.acceptsReturn;
			}
			set
			{
				this.acceptsReturn = value;
			}
		}

		/// <summary>Gets or sets an option that controls how automatic completion works for the <see cref="T:System.Windows.Forms.TextBox" />.</summary>
		/// <returns>One of the values of <see cref="T:System.Windows.Forms.AutoCompleteMode" />. The following are the values.<see cref="F:System.Windows.Forms.AutoCompleteMode.Append" />Appends the remainder of the most likely candidate string to the existing characters, highlighting the appended characters.<see cref="F:System.Windows.Forms.AutoCompleteMode.Suggest" />Displays the auxiliary drop-down list associated with the edit control. This drop-down is populated with one or more suggested completion strings.<see cref="F:System.Windows.Forms.AutoCompleteMode.SuggestAppend" />Appends both <see langword="Suggest" /> and <see langword="Append" /> options.<see cref="F:System.Windows.Forms.AutoCompleteMode.None" />Disables automatic completion. This is the default.</returns>
		/// <exception cref="T:System.ComponentModel.InvalidEnumArgumentException">The specified value is not one of the values of <see cref="T:System.Windows.Forms.AutoCompleteMode" />. </exception>
		// Token: 0x17000E4F RID: 3663
		// (get) Token: 0x0600394B RID: 14667 RVA: 0x000FF07B File Offset: 0x000FD27B
		// (set) Token: 0x0600394C RID: 14668 RVA: 0x000FF084 File Offset: 0x000FD284
		[DefaultValue(AutoCompleteMode.None)]
		[SRDescription("TextBoxAutoCompleteModeDescr")]
		[Browsable(true)]
		[EditorBrowsable(EditorBrowsableState.Always)]
		public AutoCompleteMode AutoCompleteMode
		{
			get
			{
				return this.autoCompleteMode;
			}
			set
			{
				if (!ClientUtils.IsEnumValid(value, (int)value, 0, 3))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(AutoCompleteMode));
				}
				bool autoComplete = false;
				if (this.autoCompleteMode != AutoCompleteMode.None && value == AutoCompleteMode.None)
				{
					autoComplete = true;
				}
				this.autoCompleteMode = value;
				this.SetAutoComplete(autoComplete);
			}
		}

		/// <summary>Gets or sets a value specifying the source of complete strings used for automatic completion.</summary>
		/// <returns>One of the values of <see cref="T:System.Windows.Forms.AutoCompleteSource" />. The options are <see langword="AllSystemSources" />, <see langword="AllUrl" />, <see langword="FileSystem" />, <see langword="HistoryList" />, <see langword="RecentlyUsedList" />, <see langword="CustomSource" />, and <see langword="None" />. The default is <see langword="None" />.</returns>
		/// <exception cref="T:System.ComponentModel.InvalidEnumArgumentException">The specified value is not one of the values of <see cref="T:System.Windows.Forms.AutoCompleteSource" />. </exception>
		// Token: 0x17000E50 RID: 3664
		// (get) Token: 0x0600394D RID: 14669 RVA: 0x000FF0D4 File Offset: 0x000FD2D4
		// (set) Token: 0x0600394E RID: 14670 RVA: 0x000FF0DC File Offset: 0x000FD2DC
		[DefaultValue(AutoCompleteSource.None)]
		[SRDescription("TextBoxAutoCompleteSourceDescr")]
		[TypeConverter(typeof(TextBoxAutoCompleteSourceConverter))]
		[Browsable(true)]
		[EditorBrowsable(EditorBrowsableState.Always)]
		public AutoCompleteSource AutoCompleteSource
		{
			get
			{
				return this.autoCompleteSource;
			}
			set
			{
				if (!ClientUtils.IsEnumValid_NotSequential(value, (int)value, new int[]
				{
					128,
					7,
					6,
					64,
					1,
					32,
					2,
					256,
					4
				}))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(AutoCompleteSource));
				}
				if (value == AutoCompleteSource.ListItems)
				{
					throw new NotSupportedException(SR.GetString("TextBoxAutoCompleteSourceNoItems"));
				}
				if (value != AutoCompleteSource.None && value != AutoCompleteSource.CustomSource)
				{
					new FileIOPermission(PermissionState.Unrestricted)
					{
						AllFiles = FileIOPermissionAccess.PathDiscovery
					}.Demand();
				}
				this.autoCompleteSource = value;
				this.SetAutoComplete(false);
			}
		}

		/// <summary>Gets or sets a custom <see cref="T:System.Collections.Specialized.StringCollection" /> to use when the <see cref="P:System.Windows.Forms.TextBox.AutoCompleteSource" /> property is set to <see langword="CustomSource" />.</summary>
		/// <returns>A <see cref="T:System.Collections.Specialized.StringCollection" /> to use with <see cref="P:System.Windows.Forms.TextBox.AutoCompleteSource" />.</returns>
		// Token: 0x17000E51 RID: 3665
		// (get) Token: 0x0600394F RID: 14671 RVA: 0x000FF166 File Offset: 0x000FD366
		// (set) Token: 0x06003950 RID: 14672 RVA: 0x000FF198 File Offset: 0x000FD398
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[Localizable(true)]
		[SRDescription("TextBoxAutoCompleteCustomSourceDescr")]
		[Editor("System.Windows.Forms.Design.ListControlStringCollectionEditor, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
		[Browsable(true)]
		[EditorBrowsable(EditorBrowsableState.Always)]
		public AutoCompleteStringCollection AutoCompleteCustomSource
		{
			get
			{
				if (this.autoCompleteCustomSource == null)
				{
					this.autoCompleteCustomSource = new AutoCompleteStringCollection();
					this.autoCompleteCustomSource.CollectionChanged += this.OnAutoCompleteCustomSourceChanged;
				}
				return this.autoCompleteCustomSource;
			}
			set
			{
				if (this.autoCompleteCustomSource != value)
				{
					if (this.autoCompleteCustomSource != null)
					{
						this.autoCompleteCustomSource.CollectionChanged -= this.OnAutoCompleteCustomSourceChanged;
					}
					this.autoCompleteCustomSource = value;
					if (value != null)
					{
						this.autoCompleteCustomSource.CollectionChanged += this.OnAutoCompleteCustomSourceChanged;
					}
					this.SetAutoComplete(false);
				}
			}
		}

		/// <summary>Gets or sets whether the <see cref="T:System.Windows.Forms.TextBox" /> control modifies the case of characters as they are typed.</summary>
		/// <returns>One of the <see cref="T:System.Windows.Forms.CharacterCasing" /> enumeration values that specifies whether the <see cref="T:System.Windows.Forms.TextBox" /> control modifies the case of characters. The default is <see langword="CharacterCasing.Normal" />.</returns>
		/// <exception cref="T:System.ComponentModel.InvalidEnumArgumentException">A value that is not within the range of valid values for the enumeration was assigned to the property. </exception>
		// Token: 0x17000E52 RID: 3666
		// (get) Token: 0x06003951 RID: 14673 RVA: 0x000FF1F5 File Offset: 0x000FD3F5
		// (set) Token: 0x06003952 RID: 14674 RVA: 0x000FF1FD File Offset: 0x000FD3FD
		[SRCategory("CatBehavior")]
		[DefaultValue(CharacterCasing.Normal)]
		[SRDescription("TextBoxCharacterCasingDescr")]
		public CharacterCasing CharacterCasing
		{
			get
			{
				return this.characterCasing;
			}
			set
			{
				if (this.characterCasing != value)
				{
					if (!ClientUtils.IsEnumValid(value, (int)value, 0, 2))
					{
						throw new InvalidEnumArgumentException("value", (int)value, typeof(CharacterCasing));
					}
					this.characterCasing = value;
					base.RecreateHandle();
				}
			}
		}

		/// <summary>Gets or sets a value indicating whether this is a multiline <see cref="T:System.Windows.Forms.TextBox" /> control.</summary>
		/// <returns>
		///     <see langword="true" /> if the control is a multiline <see cref="T:System.Windows.Forms.TextBox" /> control; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
		// Token: 0x17000E53 RID: 3667
		// (get) Token: 0x06003953 RID: 14675 RVA: 0x000EB74F File Offset: 0x000E994F
		// (set) Token: 0x06003954 RID: 14676 RVA: 0x000FF23B File Offset: 0x000FD43B
		public override bool Multiline
		{
			get
			{
				return base.Multiline;
			}
			set
			{
				if (this.Multiline != value)
				{
					base.Multiline = value;
					if (value && this.AutoCompleteMode != AutoCompleteMode.None)
					{
						base.RecreateHandle();
					}
				}
			}
		}

		// Token: 0x17000E54 RID: 3668
		// (get) Token: 0x06003955 RID: 14677 RVA: 0x000FF25E File Offset: 0x000FD45E
		internal override bool PasswordProtect
		{
			get
			{
				return this.PasswordChar > '\0';
			}
		}

		/// <summary>Gets the required creation parameters when the control handle is created.</summary>
		/// <returns>A <see cref="T:System.Windows.Forms.CreateParams" /> that contains the required creation parameters when the handle to the control is created.</returns>
		// Token: 0x17000E55 RID: 3669
		// (get) Token: 0x06003956 RID: 14678 RVA: 0x000FF26C File Offset: 0x000FD46C
		protected override CreateParams CreateParams
		{
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			get
			{
				CreateParams createParams = base.CreateParams;
				CharacterCasing characterCasing = this.characterCasing;
				if (characterCasing != CharacterCasing.Upper)
				{
					if (characterCasing == CharacterCasing.Lower)
					{
						createParams.Style |= 16;
					}
				}
				else
				{
					createParams.Style |= 8;
				}
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
				if (this.Multiline)
				{
					if ((this.scrollBars & ScrollBars.Horizontal) == ScrollBars.Horizontal && this.textAlign == HorizontalAlignment.Left && !base.WordWrap)
					{
						createParams.Style |= 1048576;
					}
					if ((this.scrollBars & ScrollBars.Vertical) == ScrollBars.Vertical)
					{
						createParams.Style |= 2097152;
					}
				}
				if (this.useSystemPasswordChar)
				{
					createParams.Style |= 32;
				}
				return createParams;
			}
		}

		/// <summary>Gets or sets the character used to mask characters of a password in a single-line <see cref="T:System.Windows.Forms.TextBox" /> control.</summary>
		/// <returns>The character used to mask characters entered in a single-line <see cref="T:System.Windows.Forms.TextBox" /> control. Set the value of this property to 0 (character value) if you do not want the control to mask characters as they are typed. Equals 0 (character value) by default.</returns>
		// Token: 0x17000E56 RID: 3670
		// (get) Token: 0x06003957 RID: 14679 RVA: 0x000FF379 File Offset: 0x000FD579
		// (set) Token: 0x06003958 RID: 14680 RVA: 0x000FF39C File Offset: 0x000FD59C
		[SRCategory("CatBehavior")]
		[DefaultValue('\0')]
		[Localizable(true)]
		[SRDescription("TextBoxPasswordCharDescr")]
		[RefreshProperties(RefreshProperties.Repaint)]
		public char PasswordChar
		{
			get
			{
				if (!base.IsHandleCreated)
				{
					this.CreateHandle();
				}
				return (char)((int)base.SendMessage(210, 0, 0));
			}
			set
			{
				this.passwordChar = value;
				if (!this.useSystemPasswordChar && base.IsHandleCreated && this.PasswordChar != value)
				{
					base.SendMessage(204, (int)value, 0);
					base.VerifyImeRestrictedModeChanged();
					this.ResetAutoComplete(false);
					base.Invalidate();
				}
			}
		}

		/// <summary>Gets or sets which scroll bars should appear in a multiline <see cref="T:System.Windows.Forms.TextBox" /> control.</summary>
		/// <returns>One of the <see cref="T:System.Windows.Forms.ScrollBars" /> enumeration values that indicates whether a multiline <see cref="T:System.Windows.Forms.TextBox" /> control appears with no scroll bars, a horizontal scroll bar, a vertical scroll bar, or both. The default is <see langword="ScrollBars.None" />.</returns>
		/// <exception cref="T:System.ComponentModel.InvalidEnumArgumentException">A value that is not within the range of valid values for the enumeration was assigned to the property. </exception>
		// Token: 0x17000E57 RID: 3671
		// (get) Token: 0x06003959 RID: 14681 RVA: 0x000FF3EA File Offset: 0x000FD5EA
		// (set) Token: 0x0600395A RID: 14682 RVA: 0x000FF3F2 File Offset: 0x000FD5F2
		[SRCategory("CatAppearance")]
		[Localizable(true)]
		[DefaultValue(ScrollBars.None)]
		[SRDescription("TextBoxScrollBarsDescr")]
		public ScrollBars ScrollBars
		{
			get
			{
				return this.scrollBars;
			}
			set
			{
				if (this.scrollBars != value)
				{
					if (!ClientUtils.IsEnumValid(value, (int)value, 0, 3))
					{
						throw new InvalidEnumArgumentException("value", (int)value, typeof(ScrollBars));
					}
					this.scrollBars = value;
					base.RecreateHandle();
				}
			}
		}

		// Token: 0x0600395B RID: 14683 RVA: 0x000FF430 File Offset: 0x000FD630
		internal override Size GetPreferredSizeCore(Size proposedConstraints)
		{
			Size empty = Size.Empty;
			if (this.Multiline && !base.WordWrap && (this.ScrollBars & ScrollBars.Horizontal) != ScrollBars.None)
			{
				empty.Height += SystemInformation.GetHorizontalScrollBarHeightForDpi(this.deviceDpi);
			}
			if (this.Multiline && (this.ScrollBars & ScrollBars.Vertical) != ScrollBars.None)
			{
				empty.Width += SystemInformation.GetVerticalScrollBarWidthForDpi(this.deviceDpi);
			}
			proposedConstraints -= empty;
			Size preferredSizeCore = base.GetPreferredSizeCore(proposedConstraints);
			return preferredSizeCore + empty;
		}

		/// <summary>Gets or sets the text associated with this control.</summary>
		/// <returns>The text associated with this control.</returns>
		// Token: 0x17000E58 RID: 3672
		// (get) Token: 0x0600395C RID: 14684 RVA: 0x000FF4B9 File Offset: 0x000FD6B9
		// (set) Token: 0x0600395D RID: 14685 RVA: 0x000FF4C1 File Offset: 0x000FD6C1
		public override string Text
		{
			get
			{
				return base.Text;
			}
			set
			{
				base.Text = value;
				this.selectionSet = false;
			}
		}

		/// <summary>Gets or sets how text is aligned in a <see cref="T:System.Windows.Forms.TextBox" /> control.</summary>
		/// <returns>One of the <see cref="T:System.Windows.Forms.HorizontalAlignment" /> enumeration values that specifies how text is aligned in the control. The default is <see langword="HorizontalAlignment.Left" />.</returns>
		/// <exception cref="T:System.ComponentModel.InvalidEnumArgumentException">A value that is not within the range of valid values for the enumeration was assigned to the property. </exception>
		// Token: 0x17000E59 RID: 3673
		// (get) Token: 0x0600395E RID: 14686 RVA: 0x000FF4D1 File Offset: 0x000FD6D1
		// (set) Token: 0x0600395F RID: 14687 RVA: 0x000FF4DC File Offset: 0x000FD6DC
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

		/// <summary>Gets or sets a value indicating whether the text in the <see cref="T:System.Windows.Forms.TextBox" /> control should appear as the default password character.</summary>
		/// <returns>
		///     <see langword="true" /> if the text in the <see cref="T:System.Windows.Forms.TextBox" /> control should appear as the default password character; otherwise, <see langword="false" />.</returns>
		// Token: 0x17000E5A RID: 3674
		// (get) Token: 0x06003960 RID: 14688 RVA: 0x000FF530 File Offset: 0x000FD730
		// (set) Token: 0x06003961 RID: 14689 RVA: 0x000FF538 File Offset: 0x000FD738
		[SRCategory("CatBehavior")]
		[DefaultValue(false)]
		[SRDescription("TextBoxUseSystemPasswordCharDescr")]
		[RefreshProperties(RefreshProperties.Repaint)]
		public bool UseSystemPasswordChar
		{
			get
			{
				return this.useSystemPasswordChar;
			}
			set
			{
				if (value != this.useSystemPasswordChar)
				{
					this.useSystemPasswordChar = value;
					base.RecreateHandle();
					if (value)
					{
						this.ResetAutoComplete(false);
					}
				}
			}
		}

		/// <summary>Occurs when the value of the <see cref="P:System.Windows.Forms.TextBox.TextAlign" /> property has changed.</summary>
		// Token: 0x140002CA RID: 714
		// (add) Token: 0x06003962 RID: 14690 RVA: 0x000FF55A File Offset: 0x000FD75A
		// (remove) Token: 0x06003963 RID: 14691 RVA: 0x000FF56D File Offset: 0x000FD76D
		[SRCategory("CatPropertyChanged")]
		[SRDescription("RadioButtonOnTextAlignChangedDescr")]
		public event EventHandler TextAlignChanged
		{
			add
			{
				base.Events.AddHandler(TextBox.EVENT_TEXTALIGNCHANGED, value);
			}
			remove
			{
				base.Events.RemoveHandler(TextBox.EVENT_TEXTALIGNCHANGED, value);
			}
		}

		/// <summary>Releases the unmanaged resources used by the <see cref="T:System.Windows.Forms.TextBox" /> and optionally releases the managed resources. </summary>
		/// <param name="disposing">
		///       <see langword="true" /> to release both managed and unmanaged resources; <see langword="false" /> to release only unmanaged resources. </param>
		// Token: 0x06003964 RID: 14692 RVA: 0x000FF580 File Offset: 0x000FD780
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				this.ResetAutoComplete(true);
				if (this.autoCompleteCustomSource != null)
				{
					this.autoCompleteCustomSource.CollectionChanged -= this.OnAutoCompleteCustomSourceChanged;
				}
				if (this.stringSource != null)
				{
					this.stringSource.ReleaseAutoComplete();
					this.stringSource = null;
				}
			}
			base.Dispose(disposing);
		}

		/// <summary>Determines whether the specified key is an input key or a special key that requires preprocessing.</summary>
		/// <param name="keyData">One of the key's values.</param>
		/// <returns>
		///     <see langword="true" /> if the specified key is an input key; otherwise, <see langword="false" />.</returns>
		// Token: 0x06003965 RID: 14693 RVA: 0x000FF5D8 File Offset: 0x000FD7D8
		protected override bool IsInputKey(Keys keyData)
		{
			if (this.Multiline && (keyData & Keys.Alt) == Keys.None)
			{
				Keys keys = keyData & Keys.KeyCode;
				if (keys == Keys.Return)
				{
					return this.acceptsReturn;
				}
			}
			return base.IsInputKey(keyData);
		}

		// Token: 0x06003966 RID: 14694 RVA: 0x000FF611 File Offset: 0x000FD811
		private void OnAutoCompleteCustomSourceChanged(object sender, CollectionChangeEventArgs e)
		{
			if (this.AutoCompleteSource == AutoCompleteSource.CustomSource)
			{
				this.SetAutoComplete(true);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.BackColorChanged" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
		// Token: 0x06003967 RID: 14695 RVA: 0x000CC4B4 File Offset: 0x000CA6B4
		protected override void OnBackColorChanged(EventArgs e)
		{
			base.OnBackColorChanged(e);
			if (Application.RenderWithVisualStyles && base.IsHandleCreated && base.BorderStyle == BorderStyle.Fixed3D)
			{
				SafeNativeMethods.RedrawWindow(new HandleRef(this, base.Handle), null, NativeMethods.NullHandleRef, 1025);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.FontChanged" /> event. </summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x06003968 RID: 14696 RVA: 0x000FF624 File Offset: 0x000FD824
		protected override void OnFontChanged(EventArgs e)
		{
			base.OnFontChanged(e);
			if (this.AutoCompleteMode != AutoCompleteMode.None)
			{
				base.RecreateHandle();
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.GotFocus" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
		// Token: 0x06003969 RID: 14697 RVA: 0x000FF63B File Offset: 0x000FD83B
		protected override void OnGotFocus(EventArgs e)
		{
			base.OnGotFocus(e);
			if (!this.selectionSet)
			{
				this.selectionSet = true;
				if (this.SelectionLength == 0 && Control.MouseButtons == MouseButtons.None)
				{
					base.SelectAll();
				}
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.HandleCreated" /> event.</summary>
		/// <param name="e">The event data.</param>
		// Token: 0x0600396A RID: 14698 RVA: 0x000FF668 File Offset: 0x000FD868
		protected override void OnHandleCreated(EventArgs e)
		{
			base.OnHandleCreated(e);
			base.SetSelectionOnHandle();
			if (this.passwordChar != '\0' && !this.useSystemPasswordChar)
			{
				base.SendMessage(204, (int)this.passwordChar, 0);
			}
			base.VerifyImeRestrictedModeChanged();
			if (this.AutoCompleteMode != AutoCompleteMode.None)
			{
				try
				{
					this.fromHandleCreate = true;
					this.SetAutoComplete(false);
				}
				finally
				{
					this.fromHandleCreate = false;
				}
			}
		}

		/// <summary>Raises the <see cref="M:System.Windows.Forms.Control.OnHandleDestroyed(System.EventArgs)" /> event. </summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x0600396B RID: 14699 RVA: 0x000FF6DC File Offset: 0x000FD8DC
		protected override void OnHandleDestroyed(EventArgs e)
		{
			if (this.stringSource != null)
			{
				this.stringSource.ReleaseAutoComplete();
				this.stringSource = null;
			}
			base.OnHandleDestroyed(e);
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.TextBox.TextAlignChanged" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x0600396C RID: 14700 RVA: 0x000FF700 File Offset: 0x000FD900
		protected virtual void OnTextAlignChanged(EventArgs e)
		{
			EventHandler eventHandler = base.Events[TextBox.EVENT_TEXTALIGNCHANGED] as EventHandler;
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		/// <summary>Processes a command key.</summary>
		/// <param name="m">A <see cref="T:System.Windows.Forms.Message" />, passed by reference that represents the window message to process.</param>
		/// <param name="keyData">One of the <see cref="T:System.Windows.Forms.Keys" /> values that represents the shortcut key to process. </param>
		/// <returns>
		///     <see langword="true" /> if the command key was processed by the control; otherwise, <see langword="false" />.</returns>
		// Token: 0x0600396D RID: 14701 RVA: 0x000FF730 File Offset: 0x000FD930
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		protected override bool ProcessCmdKey(ref Message m, Keys keyData)
		{
			bool flag = base.ProcessCmdKey(ref m, keyData);
			if (!flag && this.Multiline && !LocalAppContextSwitches.DoNotSupportSelectAllShortcutInMultilineTextBox && this.ShortcutsEnabled && keyData == (Keys)131137)
			{
				base.SelectAll();
				return true;
			}
			return flag;
		}

		/// <summary>Sets the selected text to the specified text without clearing the undo buffer.</summary>
		/// <param name="text">The text to replace.</param>
		// Token: 0x0600396E RID: 14702 RVA: 0x000FF771 File Offset: 0x000FD971
		public void Paste(string text)
		{
			base.SetSelectedTextInternal(text, false);
		}

		// Token: 0x0600396F RID: 14703 RVA: 0x000FF77B File Offset: 0x000FD97B
		internal override void SelectInternal(int start, int length, int textLen)
		{
			this.selectionSet = true;
			base.SelectInternal(start, length, textLen);
		}

		// Token: 0x06003970 RID: 14704 RVA: 0x000FF790 File Offset: 0x000FD990
		private string[] GetStringsForAutoComplete()
		{
			string[] array = new string[this.AutoCompleteCustomSource.Count];
			for (int i = 0; i < this.AutoCompleteCustomSource.Count; i++)
			{
				array[i] = this.AutoCompleteCustomSource[i];
			}
			return array;
		}

		// Token: 0x06003971 RID: 14705 RVA: 0x000FF7D4 File Offset: 0x000FD9D4
		internal void SetAutoComplete(bool reset)
		{
			if (this.Multiline || this.passwordChar != '\0' || this.useSystemPasswordChar || this.AutoCompleteSource == AutoCompleteSource.None)
			{
				return;
			}
			if (this.AutoCompleteMode != AutoCompleteMode.None)
			{
				if (!this.fromHandleCreate)
				{
					AutoCompleteMode autoCompleteMode = this.AutoCompleteMode;
					this.autoCompleteMode = AutoCompleteMode.None;
					base.RecreateHandle();
					this.autoCompleteMode = autoCompleteMode;
				}
				if (this.AutoCompleteSource == AutoCompleteSource.CustomSource)
				{
					if (!base.IsHandleCreated || this.AutoCompleteCustomSource == null)
					{
						return;
					}
					if (this.AutoCompleteCustomSource.Count == 0)
					{
						this.ResetAutoComplete(true);
						return;
					}
					if (this.stringSource != null)
					{
						this.stringSource.RefreshList(this.GetStringsForAutoComplete());
						return;
					}
					this.stringSource = new StringSource(this.GetStringsForAutoComplete());
					if (!this.stringSource.Bind(new HandleRef(this, base.Handle), (int)this.AutoCompleteMode))
					{
						throw new ArgumentException(SR.GetString("AutoCompleteFailure"));
					}
					return;
				}
				else
				{
					try
					{
						if (base.IsHandleCreated)
						{
							int num = 0;
							if (this.AutoCompleteMode == AutoCompleteMode.Suggest)
							{
								num |= -1879048192;
							}
							if (this.AutoCompleteMode == AutoCompleteMode.Append)
							{
								num |= 1610612736;
							}
							if (this.AutoCompleteMode == AutoCompleteMode.SuggestAppend)
							{
								num |= 268435456;
								num |= 1073741824;
							}
							int num2 = SafeNativeMethods.SHAutoComplete(new HandleRef(this, base.Handle), (int)(this.AutoCompleteSource | (AutoCompleteSource)num));
						}
						return;
					}
					catch (SecurityException)
					{
						return;
					}
				}
			}
			if (reset)
			{
				this.ResetAutoComplete(true);
			}
		}

		// Token: 0x06003972 RID: 14706 RVA: 0x000FF94C File Offset: 0x000FDB4C
		private void ResetAutoComplete(bool force)
		{
			if ((this.AutoCompleteMode > AutoCompleteMode.None || force) && base.IsHandleCreated)
			{
				int flags = -1610612729;
				SafeNativeMethods.SHAutoComplete(new HandleRef(this, base.Handle), flags);
			}
		}

		// Token: 0x06003973 RID: 14707 RVA: 0x000FF987 File Offset: 0x000FDB87
		private void ResetAutoCompleteCustomSource()
		{
			this.AutoCompleteCustomSource = null;
		}

		// Token: 0x06003974 RID: 14708 RVA: 0x000FF990 File Offset: 0x000FDB90
		private void WmPrint(ref Message m)
		{
			base.WndProc(ref m);
			if ((2 & (int)m.LParam) != 0 && Application.RenderWithVisualStyles && base.BorderStyle == BorderStyle.Fixed3D)
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
		/// <param name="m">A Windows Message object. </param>
		// Token: 0x06003975 RID: 14709 RVA: 0x000FFA7C File Offset: 0x000FDC7C
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		protected override void WndProc(ref Message m)
		{
			int msg = m.Msg;
			if (msg != 513)
			{
				if (msg == 514)
				{
					base.WndProc(ref m);
					return;
				}
				if (msg == 791)
				{
					this.WmPrint(ref m);
					return;
				}
				base.WndProc(ref m);
			}
			else
			{
				MouseButtons mouseButtons = Control.MouseButtons;
				bool validationCancelled = base.ValidationCancelled;
				this.FocusInternal();
				if (mouseButtons == Control.MouseButtons && (!base.ValidationCancelled || validationCancelled))
				{
					base.WndProc(ref m);
					return;
				}
			}
		}

		// Token: 0x04002286 RID: 8838
		private static readonly object EVENT_TEXTALIGNCHANGED = new object();

		// Token: 0x04002287 RID: 8839
		private bool acceptsReturn;

		// Token: 0x04002288 RID: 8840
		private char passwordChar;

		// Token: 0x04002289 RID: 8841
		private bool useSystemPasswordChar;

		// Token: 0x0400228A RID: 8842
		private CharacterCasing characterCasing;

		// Token: 0x0400228B RID: 8843
		private ScrollBars scrollBars;

		// Token: 0x0400228C RID: 8844
		private HorizontalAlignment textAlign;

		// Token: 0x0400228D RID: 8845
		private bool selectionSet;

		// Token: 0x0400228E RID: 8846
		private AutoCompleteMode autoCompleteMode;

		// Token: 0x0400228F RID: 8847
		private AutoCompleteSource autoCompleteSource = AutoCompleteSource.None;

		// Token: 0x04002290 RID: 8848
		private AutoCompleteStringCollection autoCompleteCustomSource;

		// Token: 0x04002291 RID: 8849
		private bool fromHandleCreate;

		// Token: 0x04002292 RID: 8850
		private StringSource stringSource;
	}
}
