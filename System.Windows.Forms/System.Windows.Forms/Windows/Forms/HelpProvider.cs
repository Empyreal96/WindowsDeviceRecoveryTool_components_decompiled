using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing.Design;

namespace System.Windows.Forms
{
	/// <summary>Provides pop-up or online Help for controls.</summary>
	// Token: 0x02000265 RID: 613
	[ProvideProperty("HelpString", typeof(Control))]
	[ProvideProperty("HelpKeyword", typeof(Control))]
	[ProvideProperty("HelpNavigator", typeof(Control))]
	[ProvideProperty("ShowHelp", typeof(Control))]
	[ToolboxItemFilter("System.Windows.Forms")]
	[SRDescription("DescriptionHelpProvider")]
	public class HelpProvider : Component, IExtenderProvider
	{
		/// <summary>Gets or sets a value specifying the name of the Help file associated with this <see cref="T:System.Windows.Forms.HelpProvider" /> object.</summary>
		/// <returns>The name of the Help file. This can be of the form C:\path\sample.chm or /folder/file.htm.</returns>
		// Token: 0x170008D6 RID: 2262
		// (get) Token: 0x060024CB RID: 9419 RVA: 0x000B2681 File Offset: 0x000B0881
		// (set) Token: 0x060024CC RID: 9420 RVA: 0x000B2689 File Offset: 0x000B0889
		[Localizable(true)]
		[DefaultValue(null)]
		[Editor("System.Windows.Forms.Design.HelpNamespaceEditor, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
		[SRDescription("HelpProviderHelpNamespaceDescr")]
		public virtual string HelpNamespace
		{
			get
			{
				return this.helpNamespace;
			}
			set
			{
				this.helpNamespace = value;
			}
		}

		/// <summary>Gets or sets the object that contains supplemental data about the <see cref="T:System.Windows.Forms.HelpProvider" />.</summary>
		/// <returns>An <see cref="T:System.Object" /> that contains data about the <see cref="T:System.Windows.Forms.HelpProvider" />.</returns>
		// Token: 0x170008D7 RID: 2263
		// (get) Token: 0x060024CD RID: 9421 RVA: 0x000B2692 File Offset: 0x000B0892
		// (set) Token: 0x060024CE RID: 9422 RVA: 0x000B269A File Offset: 0x000B089A
		[SRCategory("CatData")]
		[Localizable(false)]
		[Bindable(true)]
		[SRDescription("ControlTagDescr")]
		[DefaultValue(null)]
		[TypeConverter(typeof(StringConverter))]
		public object Tag
		{
			get
			{
				return this.userData;
			}
			set
			{
				this.userData = value;
			}
		}

		/// <summary>Specifies whether this object can provide its extender properties to the specified object.</summary>
		/// <param name="target">The object that the externder properties are provided to. </param>
		/// <returns>
		///     <see langword="true" /> if this object can provide its extender properties; otherwise, <see langword="false" />.</returns>
		// Token: 0x060024CF RID: 9423 RVA: 0x000B26A3 File Offset: 0x000B08A3
		public virtual bool CanExtend(object target)
		{
			return target is Control;
		}

		/// <summary>Returns the Help keyword for the specified control.</summary>
		/// <param name="ctl">A <see cref="T:System.Windows.Forms.Control" /> from which to retrieve the Help topic. </param>
		/// <returns>The Help keyword associated with this control, or <see langword="null" /> if the <see cref="T:System.Windows.Forms.HelpProvider" /> is currently configured to display the entire Help file or is configured to provide a Help string.</returns>
		// Token: 0x060024D0 RID: 9424 RVA: 0x000B26AE File Offset: 0x000B08AE
		[DefaultValue(null)]
		[Localizable(true)]
		[SRDescription("HelpProviderHelpKeywordDescr")]
		public virtual string GetHelpKeyword(Control ctl)
		{
			return (string)this.keywords[ctl];
		}

		/// <summary>Returns the current <see cref="T:System.Windows.Forms.HelpNavigator" /> setting for the specified control.</summary>
		/// <param name="ctl">A <see cref="T:System.Windows.Forms.Control" /> from which to retrieve the Help navigator. </param>
		/// <returns>The <see cref="T:System.Windows.Forms.HelpNavigator" /> setting for the specified control. The default is <see cref="F:System.Windows.Forms.HelpNavigator.AssociateIndex" />.</returns>
		// Token: 0x060024D1 RID: 9425 RVA: 0x000B26C4 File Offset: 0x000B08C4
		[DefaultValue(HelpNavigator.AssociateIndex)]
		[Localizable(true)]
		[SRDescription("HelpProviderNavigatorDescr")]
		public virtual HelpNavigator GetHelpNavigator(Control ctl)
		{
			object obj = this.navigators[ctl];
			if (obj != null)
			{
				return (HelpNavigator)obj;
			}
			return HelpNavigator.AssociateIndex;
		}

		/// <summary>Returns the contents of the pop-up Help window for the specified control.</summary>
		/// <param name="ctl">A <see cref="T:System.Windows.Forms.Control" /> from which to retrieve the Help string. </param>
		/// <returns>The Help string associated with this control. The default is <see langword="null" />.</returns>
		// Token: 0x060024D2 RID: 9426 RVA: 0x000B26ED File Offset: 0x000B08ED
		[DefaultValue(null)]
		[Localizable(true)]
		[SRDescription("HelpProviderHelpStringDescr")]
		public virtual string GetHelpString(Control ctl)
		{
			return (string)this.helpStrings[ctl];
		}

		/// <summary>Returns a value indicating whether the specified control's Help should be displayed.</summary>
		/// <param name="ctl">A <see cref="T:System.Windows.Forms.Control" /> for which Help will be displayed. </param>
		/// <returns>
		///     <see langword="true" /> if Help will be displayed for the control; otherwise, <see langword="false" />.</returns>
		// Token: 0x060024D3 RID: 9427 RVA: 0x000B2700 File Offset: 0x000B0900
		[Localizable(true)]
		[SRDescription("HelpProviderShowHelpDescr")]
		public virtual bool GetShowHelp(Control ctl)
		{
			object obj = this.showHelp[ctl];
			return obj != null && (bool)obj;
		}

		// Token: 0x060024D4 RID: 9428 RVA: 0x000B2728 File Offset: 0x000B0928
		private void OnControlHelp(object sender, HelpEventArgs hevent)
		{
			Control control = (Control)sender;
			string helpString = this.GetHelpString(control);
			string helpKeyword = this.GetHelpKeyword(control);
			HelpNavigator helpNavigator = this.GetHelpNavigator(control);
			if (!this.GetShowHelp(control))
			{
				return;
			}
			if (Control.MouseButtons != MouseButtons.None && helpString != null && helpString.Length > 0)
			{
				Help.ShowPopup(control, helpString, hevent.MousePos);
				hevent.Handled = true;
			}
			if (!hevent.Handled && this.helpNamespace != null)
			{
				if (helpKeyword != null && helpKeyword.Length > 0)
				{
					Help.ShowHelp(control, this.helpNamespace, helpNavigator, helpKeyword);
					hevent.Handled = true;
				}
				if (!hevent.Handled)
				{
					Help.ShowHelp(control, this.helpNamespace, helpNavigator);
					hevent.Handled = true;
				}
			}
			if (!hevent.Handled && helpString != null && helpString.Length > 0)
			{
				Help.ShowPopup(control, helpString, hevent.MousePos);
				hevent.Handled = true;
			}
			if (!hevent.Handled && this.helpNamespace != null)
			{
				Help.ShowHelp(control, this.helpNamespace);
				hevent.Handled = true;
			}
		}

		// Token: 0x060024D5 RID: 9429 RVA: 0x000B2824 File Offset: 0x000B0A24
		private void OnQueryAccessibilityHelp(object sender, QueryAccessibilityHelpEventArgs e)
		{
			Control ctl = (Control)sender;
			e.HelpString = this.GetHelpString(ctl);
			e.HelpKeyword = this.GetHelpKeyword(ctl);
			e.HelpNamespace = this.HelpNamespace;
		}

		/// <summary>Specifies the Help string associated with the specified control.</summary>
		/// <param name="ctl">A <see cref="T:System.Windows.Forms.Control" /> with which to associate the Help string. </param>
		/// <param name="helpString">The Help string associated with the control. </param>
		// Token: 0x060024D6 RID: 9430 RVA: 0x000B285E File Offset: 0x000B0A5E
		public virtual void SetHelpString(Control ctl, string helpString)
		{
			this.helpStrings[ctl] = helpString;
			if (helpString != null && helpString.Length > 0)
			{
				this.SetShowHelp(ctl, true);
			}
			this.UpdateEventBinding(ctl);
		}

		/// <summary>Specifies the keyword used to retrieve Help when the user invokes Help for the specified control.</summary>
		/// <param name="ctl">A <see cref="T:System.Windows.Forms.Control" /> that specifies the control for which to set the Help topic. </param>
		/// <param name="keyword">The Help keyword to associate with the control. </param>
		// Token: 0x060024D7 RID: 9431 RVA: 0x000B2888 File Offset: 0x000B0A88
		public virtual void SetHelpKeyword(Control ctl, string keyword)
		{
			this.keywords[ctl] = keyword;
			if (keyword != null && keyword.Length > 0)
			{
				this.SetShowHelp(ctl, true);
			}
			this.UpdateEventBinding(ctl);
		}

		/// <summary>Specifies the Help command to use when retrieving Help from the Help file for the specified control.</summary>
		/// <param name="ctl">A <see cref="T:System.Windows.Forms.Control" /> for which to set the Help keyword. </param>
		/// <param name="navigator">One of the <see cref="T:System.Windows.Forms.HelpNavigator" /> values. </param>
		/// <exception cref="T:System.ComponentModel.InvalidEnumArgumentException">The value of <paramref name="navigator" /> is not one of the <see cref="T:System.Windows.Forms.HelpNavigator" /> values. </exception>
		// Token: 0x060024D8 RID: 9432 RVA: 0x000B28B4 File Offset: 0x000B0AB4
		public virtual void SetHelpNavigator(Control ctl, HelpNavigator navigator)
		{
			if (!ClientUtils.IsEnumValid(navigator, (int)navigator, -2147483647, -2147483641))
			{
				throw new InvalidEnumArgumentException("navigator", (int)navigator, typeof(HelpNavigator));
			}
			this.navigators[ctl] = navigator;
			this.SetShowHelp(ctl, true);
			this.UpdateEventBinding(ctl);
		}

		/// <summary>Specifies whether Help is displayed for the specified control.</summary>
		/// <param name="ctl">A <see cref="T:System.Windows.Forms.Control" /> for which Help is turned on or off. </param>
		/// <param name="value">
		///       <see langword="true" /> if Help displays for the control; otherwise, <see langword="false" />. </param>
		// Token: 0x060024D9 RID: 9433 RVA: 0x000B2910 File Offset: 0x000B0B10
		public virtual void SetShowHelp(Control ctl, bool value)
		{
			this.showHelp[ctl] = value;
			this.UpdateEventBinding(ctl);
		}

		// Token: 0x060024DA RID: 9434 RVA: 0x000B292B File Offset: 0x000B0B2B
		internal virtual bool ShouldSerializeShowHelp(Control ctl)
		{
			return this.showHelp.ContainsKey(ctl);
		}

		/// <summary>Removes the Help associated with the specified control.</summary>
		/// <param name="ctl">The control to remove Help from.</param>
		// Token: 0x060024DB RID: 9435 RVA: 0x000B2939 File Offset: 0x000B0B39
		public virtual void ResetShowHelp(Control ctl)
		{
			this.showHelp.Remove(ctl);
		}

		// Token: 0x060024DC RID: 9436 RVA: 0x000B2948 File Offset: 0x000B0B48
		private void UpdateEventBinding(Control ctl)
		{
			if (this.GetShowHelp(ctl) && !this.boundControls.ContainsKey(ctl))
			{
				ctl.HelpRequested += this.OnControlHelp;
				ctl.QueryAccessibilityHelp += this.OnQueryAccessibilityHelp;
				this.boundControls[ctl] = ctl;
				return;
			}
			if (!this.GetShowHelp(ctl) && this.boundControls.ContainsKey(ctl))
			{
				ctl.HelpRequested -= this.OnControlHelp;
				ctl.QueryAccessibilityHelp -= this.OnQueryAccessibilityHelp;
				this.boundControls.Remove(ctl);
			}
		}

		/// <summary>Returns a string that represents the current <see cref="T:System.Windows.Forms.HelpProvider" />.</summary>
		/// <returns>A string that represents the current <see cref="T:System.Windows.Forms.HelpProvider" />.</returns>
		// Token: 0x060024DD RID: 9437 RVA: 0x000B29E8 File Offset: 0x000B0BE8
		public override string ToString()
		{
			string str = base.ToString();
			return str + ", HelpNamespace: " + this.HelpNamespace;
		}

		// Token: 0x04000FE0 RID: 4064
		private string helpNamespace;

		// Token: 0x04000FE1 RID: 4065
		private Hashtable helpStrings = new Hashtable();

		// Token: 0x04000FE2 RID: 4066
		private Hashtable showHelp = new Hashtable();

		// Token: 0x04000FE3 RID: 4067
		private Hashtable boundControls = new Hashtable();

		// Token: 0x04000FE4 RID: 4068
		private Hashtable keywords = new Hashtable();

		// Token: 0x04000FE5 RID: 4069
		private Hashtable navigators = new Hashtable();

		// Token: 0x04000FE6 RID: 4070
		private object userData;
	}
}
