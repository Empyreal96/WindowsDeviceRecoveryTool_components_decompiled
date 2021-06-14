using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Drawing.Design;
using System.Internal;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Windows.Forms.Internal;

namespace System.Windows.Forms
{
	/// <summary>Provides a user interface for indicating that a control on a form has an error associated with it.</summary>
	// Token: 0x0200023C RID: 572
	[ProvideProperty("IconPadding", typeof(Control))]
	[ProvideProperty("IconAlignment", typeof(Control))]
	[ProvideProperty("Error", typeof(Control))]
	[ToolboxItemFilter("System.Windows.Forms")]
	[ComplexBindingProperties("DataSource", "DataMember")]
	[SRDescription("DescriptionErrorProvider")]
	public class ErrorProvider : Component, IExtenderProvider, ISupportInitialize
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.ErrorProvider" /> class and initializes the default settings for <see cref="P:System.Windows.Forms.ErrorProvider.BlinkRate" />, <see cref="P:System.Windows.Forms.ErrorProvider.BlinkStyle" />, and the <see cref="P:System.Windows.Forms.ErrorProvider.Icon" />.</summary>
		// Token: 0x060021C3 RID: 8643 RVA: 0x000A5310 File Offset: 0x000A3510
		public ErrorProvider()
		{
			this.icon = ErrorProvider.DefaultIcon;
			this.blinkRate = 250;
			this.blinkStyle = ErrorBlinkStyle.BlinkIfDifferentError;
			this.currentChanged = new EventHandler(this.ErrorManager_CurrentChanged);
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.ErrorProvider" /> class attached to a container.</summary>
		/// <param name="parentControl">The container of the control to monitor for errors. </param>
		// Token: 0x060021C4 RID: 8644 RVA: 0x000A537A File Offset: 0x000A357A
		public ErrorProvider(ContainerControl parentControl) : this()
		{
			this.parentControl = parentControl;
			this.propChangedEvent = new EventHandler(this.ParentControl_BindingContextChanged);
			parentControl.BindingContextChanged += this.propChangedEvent;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.ErrorProvider" /> class attached to an <see cref="T:System.ComponentModel.IContainer" /> implementation.</summary>
		/// <param name="container">The <see cref="T:System.ComponentModel.IContainer" /> to monitor for errors.</param>
		// Token: 0x060021C5 RID: 8645 RVA: 0x000A53A7 File Offset: 0x000A35A7
		public ErrorProvider(IContainer container) : this()
		{
			if (container == null)
			{
				throw new ArgumentNullException("container");
			}
			container.Add(this);
		}

		/// <summary>Gets or sets the <see cref="T:System.ComponentModel.ISite" /> of the <see cref="T:System.ComponentModel.Component" />.</summary>
		/// <returns>The <see cref="T:System.ComponentModel.ISite" /> associated with the <see cref="T:System.ComponentModel.Component" />, or null if the <see cref="T:System.ComponentModel.Component" /> is not encapsulated in an <see cref="T:System.ComponentModel.IContainer" />, the <see cref="T:System.ComponentModel.Component" /> does not have an <see cref="T:System.ComponentModel.ISite" /> associated with it, or the <see cref="T:System.ComponentModel.Component" /> is removed from its <see cref="T:System.ComponentModel.IContainer" />.</returns>
		// Token: 0x17000812 RID: 2066
		// (set) Token: 0x060021C6 RID: 8646 RVA: 0x000A53C4 File Offset: 0x000A35C4
		public override ISite Site
		{
			set
			{
				base.Site = value;
				if (value == null)
				{
					return;
				}
				IDesignerHost designerHost = value.GetService(typeof(IDesignerHost)) as IDesignerHost;
				if (designerHost != null)
				{
					IComponent rootComponent = designerHost.RootComponent;
					if (rootComponent is ContainerControl)
					{
						this.ContainerControl = (ContainerControl)rootComponent;
					}
				}
			}
		}

		/// <summary>Gets or sets a value indicating when the error icon flashes.</summary>
		/// <returns>One of the <see cref="T:System.Windows.Forms.ErrorBlinkStyle" /> values. The default is <see cref="F:System.Windows.Forms.ErrorBlinkStyle.BlinkIfDifferentError" />.</returns>
		/// <exception cref="T:System.ComponentModel.InvalidEnumArgumentException">The assigned value is not one of the <see cref="T:System.Windows.Forms.ErrorBlinkStyle" /> values. </exception>
		// Token: 0x17000813 RID: 2067
		// (get) Token: 0x060021C7 RID: 8647 RVA: 0x000A5410 File Offset: 0x000A3610
		// (set) Token: 0x060021C8 RID: 8648 RVA: 0x000A5424 File Offset: 0x000A3624
		[SRCategory("CatBehavior")]
		[DefaultValue(ErrorBlinkStyle.BlinkIfDifferentError)]
		[SRDescription("ErrorProviderBlinkStyleDescr")]
		public ErrorBlinkStyle BlinkStyle
		{
			get
			{
				if (this.blinkRate == 0)
				{
					return ErrorBlinkStyle.NeverBlink;
				}
				return this.blinkStyle;
			}
			set
			{
				if (!ClientUtils.IsEnumValid(value, (int)value, 0, 2))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(ErrorBlinkStyle));
				}
				if (this.blinkRate == 0)
				{
					value = ErrorBlinkStyle.NeverBlink;
				}
				if (this.blinkStyle == value)
				{
					return;
				}
				if (value == ErrorBlinkStyle.AlwaysBlink)
				{
					this.showIcon = true;
					this.blinkStyle = ErrorBlinkStyle.AlwaysBlink;
					using (IEnumerator enumerator = this.windows.Values.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							object obj = enumerator.Current;
							ErrorProvider.ErrorWindow errorWindow = (ErrorProvider.ErrorWindow)obj;
							errorWindow.StartBlinking();
						}
						return;
					}
				}
				if (this.blinkStyle == ErrorBlinkStyle.AlwaysBlink)
				{
					this.blinkStyle = value;
					using (IEnumerator enumerator2 = this.windows.Values.GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							object obj2 = enumerator2.Current;
							ErrorProvider.ErrorWindow errorWindow2 = (ErrorProvider.ErrorWindow)obj2;
							errorWindow2.StopBlinking();
						}
						return;
					}
				}
				this.blinkStyle = value;
			}
		}

		/// <summary>Gets or sets a value indicating the parent control for this <see cref="T:System.Windows.Forms.ErrorProvider" />.</summary>
		/// <returns>The <see cref="T:System.Windows.Forms.ContainerControl" /> that contains the controls that the <see cref="T:System.Windows.Forms.ErrorProvider" /> is attached to.</returns>
		// Token: 0x17000814 RID: 2068
		// (get) Token: 0x060021C9 RID: 8649 RVA: 0x000A5534 File Offset: 0x000A3734
		// (set) Token: 0x060021CA RID: 8650 RVA: 0x000A553C File Offset: 0x000A373C
		[DefaultValue(null)]
		[SRCategory("CatData")]
		[SRDescription("ErrorProviderContainerControlDescr")]
		public ContainerControl ContainerControl
		{
			[UIPermission(SecurityAction.LinkDemand, Window = UIPermissionWindow.AllWindows)]
			[UIPermission(SecurityAction.InheritanceDemand, Window = UIPermissionWindow.AllWindows)]
			get
			{
				return this.parentControl;
			}
			set
			{
				if (this.parentControl != value)
				{
					if (this.parentControl != null)
					{
						this.parentControl.BindingContextChanged -= this.propChangedEvent;
					}
					this.parentControl = value;
					if (this.parentControl != null)
					{
						this.parentControl.BindingContextChanged += this.propChangedEvent;
					}
					this.Set_ErrorManager(this.DataSource, this.DataMember, true);
				}
			}
		}

		/// <summary>Gets or sets a value that indicates whether the component is used in a locale that supports right-to-left fonts.</summary>
		/// <returns>
		///     <see langword="true" /> if the component is used in a right-to-left locale; otherwise, <see langword="false" />.</returns>
		// Token: 0x17000815 RID: 2069
		// (get) Token: 0x060021CB RID: 8651 RVA: 0x000A559E File Offset: 0x000A379E
		// (set) Token: 0x060021CC RID: 8652 RVA: 0x000A55A6 File Offset: 0x000A37A6
		[SRCategory("CatAppearance")]
		[Localizable(true)]
		[DefaultValue(false)]
		[SRDescription("ControlRightToLeftDescr")]
		public virtual bool RightToLeft
		{
			get
			{
				return this.rightToLeft;
			}
			set
			{
				if (value != this.rightToLeft)
				{
					this.rightToLeft = value;
					this.OnRightToLeftChanged(EventArgs.Empty);
				}
			}
		}

		/// <summary>Occurs when the <see cref="P:System.Windows.Forms.ErrorProvider.RightToLeft" /> property changes value. </summary>
		// Token: 0x1400017E RID: 382
		// (add) Token: 0x060021CD RID: 8653 RVA: 0x000A55C3 File Offset: 0x000A37C3
		// (remove) Token: 0x060021CE RID: 8654 RVA: 0x000A55DC File Offset: 0x000A37DC
		[SRCategory("CatPropertyChanged")]
		[SRDescription("ControlOnRightToLeftChangedDescr")]
		public event EventHandler RightToLeftChanged
		{
			add
			{
				this.onRightToLeftChanged = (EventHandler)Delegate.Combine(this.onRightToLeftChanged, value);
			}
			remove
			{
				this.onRightToLeftChanged = (EventHandler)Delegate.Remove(this.onRightToLeftChanged, value);
			}
		}

		/// <summary>Gets or sets an object that contains data about the component.</summary>
		/// <returns>An object that contains data about the control. The default is <see langword="null" />.</returns>
		// Token: 0x17000816 RID: 2070
		// (get) Token: 0x060021CF RID: 8655 RVA: 0x000A55F5 File Offset: 0x000A37F5
		// (set) Token: 0x060021D0 RID: 8656 RVA: 0x000A55FD File Offset: 0x000A37FD
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

		// Token: 0x060021D1 RID: 8657 RVA: 0x000A5608 File Offset: 0x000A3808
		private void Set_ErrorManager(object newDataSource, string newDataMember, bool force)
		{
			if (this.inSetErrorManager)
			{
				return;
			}
			this.inSetErrorManager = true;
			try
			{
				bool flag = this.DataSource != newDataSource;
				bool flag2 = this.DataMember != newDataMember;
				if (flag || flag2 || force)
				{
					this.dataSource = newDataSource;
					this.dataMember = newDataMember;
					if (this.initializing)
					{
						this.setErrorManagerOnEndInit = true;
					}
					else
					{
						this.UnwireEvents(this.errorManager);
						if (this.parentControl != null && this.dataSource != null && this.parentControl.BindingContext != null)
						{
							this.errorManager = this.parentControl.BindingContext[this.dataSource, this.dataMember];
						}
						else
						{
							this.errorManager = null;
						}
						this.WireEvents(this.errorManager);
						if (this.errorManager != null)
						{
							this.UpdateBinding();
						}
					}
				}
			}
			finally
			{
				this.inSetErrorManager = false;
			}
		}

		/// <summary>Gets or sets the data source that the <see cref="T:System.Windows.Forms.ErrorProvider" /> monitors.</summary>
		/// <returns>A data source based on the <see cref="T:System.Collections.IList" /> interface to be monitored for errors. Typically, this is a <see cref="T:System.Data.DataSet" /> to be monitored for errors.</returns>
		// Token: 0x17000817 RID: 2071
		// (get) Token: 0x060021D2 RID: 8658 RVA: 0x000A56F4 File Offset: 0x000A38F4
		// (set) Token: 0x060021D3 RID: 8659 RVA: 0x000A56FC File Offset: 0x000A38FC
		[DefaultValue(null)]
		[SRCategory("CatData")]
		[AttributeProvider(typeof(IListSource))]
		[SRDescription("ErrorProviderDataSourceDescr")]
		public object DataSource
		{
			get
			{
				return this.dataSource;
			}
			set
			{
				if (this.parentControl != null && value != null && !string.IsNullOrEmpty(this.dataMember))
				{
					try
					{
						this.errorManager = this.parentControl.BindingContext[value, this.dataMember];
					}
					catch (ArgumentException)
					{
						this.dataMember = "";
					}
				}
				this.Set_ErrorManager(value, this.DataMember, false);
			}
		}

		// Token: 0x060021D4 RID: 8660 RVA: 0x000A576C File Offset: 0x000A396C
		private bool ShouldSerializeDataSource()
		{
			return this.dataSource != null;
		}

		/// <summary>Gets or sets the list within a data source to monitor.</summary>
		/// <returns>The string that represents a list within the data source specified by the <see cref="P:System.Windows.Forms.ErrorProvider.DataSource" /> to be monitored. Typically, this will be a <see cref="T:System.Data.DataTable" />.</returns>
		// Token: 0x17000818 RID: 2072
		// (get) Token: 0x060021D5 RID: 8661 RVA: 0x000A5777 File Offset: 0x000A3977
		// (set) Token: 0x060021D6 RID: 8662 RVA: 0x000A577F File Offset: 0x000A397F
		[DefaultValue(null)]
		[SRCategory("CatData")]
		[Editor("System.Windows.Forms.Design.DataMemberListEditor, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
		[SRDescription("ErrorProviderDataMemberDescr")]
		public string DataMember
		{
			get
			{
				return this.dataMember;
			}
			set
			{
				if (value == null)
				{
					value = "";
				}
				this.Set_ErrorManager(this.DataSource, value, false);
			}
		}

		// Token: 0x060021D7 RID: 8663 RVA: 0x000A5799 File Offset: 0x000A3999
		private bool ShouldSerializeDataMember()
		{
			return this.dataMember != null && this.dataMember.Length != 0;
		}

		/// <summary>Provides a method to set both the <see cref="P:System.Windows.Forms.ErrorProvider.DataSource" /> and <see cref="P:System.Windows.Forms.ErrorProvider.DataMember" /> at run time.</summary>
		/// <param name="newDataSource">A data set based on the <see cref="T:System.Collections.IList" /> interface to be monitored for errors. Typically, this is a <see cref="T:System.Data.DataSet" /> to be monitored for errors. </param>
		/// <param name="newDataMember">A collection within the <paramref name="newDataSource" /> to monitor for errors. Typically, this will be a <see cref="T:System.Data.DataTable" />. </param>
		// Token: 0x060021D8 RID: 8664 RVA: 0x000A57B3 File Offset: 0x000A39B3
		public void BindToDataAndErrors(object newDataSource, string newDataMember)
		{
			this.Set_ErrorManager(newDataSource, newDataMember, false);
		}

		// Token: 0x060021D9 RID: 8665 RVA: 0x000A57C0 File Offset: 0x000A39C0
		private void WireEvents(BindingManagerBase listManager)
		{
			if (listManager != null)
			{
				listManager.CurrentChanged += this.currentChanged;
				listManager.BindingComplete += this.ErrorManager_BindingComplete;
				CurrencyManager currencyManager = listManager as CurrencyManager;
				if (currencyManager != null)
				{
					currencyManager.ItemChanged += this.ErrorManager_ItemChanged;
					currencyManager.Bindings.CollectionChanged += this.ErrorManager_BindingsChanged;
				}
			}
		}

		// Token: 0x060021DA RID: 8666 RVA: 0x000A5824 File Offset: 0x000A3A24
		private void UnwireEvents(BindingManagerBase listManager)
		{
			if (listManager != null)
			{
				listManager.CurrentChanged -= this.currentChanged;
				listManager.BindingComplete -= this.ErrorManager_BindingComplete;
				CurrencyManager currencyManager = listManager as CurrencyManager;
				if (currencyManager != null)
				{
					currencyManager.ItemChanged -= this.ErrorManager_ItemChanged;
					currencyManager.Bindings.CollectionChanged -= this.ErrorManager_BindingsChanged;
				}
			}
		}

		// Token: 0x060021DB RID: 8667 RVA: 0x000A5888 File Offset: 0x000A3A88
		private void ErrorManager_BindingComplete(object sender, BindingCompleteEventArgs e)
		{
			Binding binding = e.Binding;
			if (binding != null && binding.Control != null)
			{
				this.SetError(binding.Control, (e.ErrorText == null) ? string.Empty : e.ErrorText);
			}
		}

		// Token: 0x060021DC RID: 8668 RVA: 0x000A58C8 File Offset: 0x000A3AC8
		private void ErrorManager_BindingsChanged(object sender, CollectionChangeEventArgs e)
		{
			this.ErrorManager_CurrentChanged(this.errorManager, e);
		}

		// Token: 0x060021DD RID: 8669 RVA: 0x000A58D7 File Offset: 0x000A3AD7
		private void ParentControl_BindingContextChanged(object sender, EventArgs e)
		{
			this.Set_ErrorManager(this.DataSource, this.DataMember, true);
		}

		/// <summary>Provides a method to update the bindings of the <see cref="P:System.Windows.Forms.ErrorProvider.DataSource" />, <see cref="P:System.Windows.Forms.ErrorProvider.DataMember" />, and the error text.</summary>
		// Token: 0x060021DE RID: 8670 RVA: 0x000A58EC File Offset: 0x000A3AEC
		public void UpdateBinding()
		{
			this.ErrorManager_CurrentChanged(this.errorManager, EventArgs.Empty);
		}

		// Token: 0x060021DF RID: 8671 RVA: 0x000A5900 File Offset: 0x000A3B00
		private void ErrorManager_ItemChanged(object sender, ItemChangedEventArgs e)
		{
			BindingsCollection bindings = this.errorManager.Bindings;
			int count = bindings.Count;
			if (e.Index == -1 && this.errorManager.Count == 0)
			{
				for (int i = 0; i < count; i++)
				{
					if (bindings[i].Control != null)
					{
						this.SetError(bindings[i].Control, "");
					}
				}
				return;
			}
			this.ErrorManager_CurrentChanged(sender, e);
		}

		// Token: 0x060021E0 RID: 8672 RVA: 0x000A5970 File Offset: 0x000A3B70
		private void ErrorManager_CurrentChanged(object sender, EventArgs e)
		{
			if (this.errorManager.Count == 0)
			{
				return;
			}
			object obj = this.errorManager.Current;
			if (!(obj is IDataErrorInfo))
			{
				return;
			}
			BindingsCollection bindings = this.errorManager.Bindings;
			int count = bindings.Count;
			foreach (object obj2 in this.items.Values)
			{
				ErrorProvider.ControlItem controlItem = (ErrorProvider.ControlItem)obj2;
				controlItem.BlinkPhase = 0;
			}
			Hashtable hashtable = new Hashtable(count);
			for (int i = 0; i < count; i++)
			{
				if (bindings[i].Control != null)
				{
					BindToObject bindToObject = bindings[i].BindToObject;
					string text = ((IDataErrorInfo)obj)[bindToObject.BindingMemberInfo.BindingField];
					if (text == null)
					{
						text = "";
					}
					string text2 = "";
					if (hashtable.Contains(bindings[i].Control))
					{
						text2 = (string)hashtable[bindings[i].Control];
					}
					if (string.IsNullOrEmpty(text2))
					{
						text2 = text;
					}
					else
					{
						text2 = text2 + "\r\n" + text;
					}
					hashtable[bindings[i].Control] = text2;
				}
			}
			foreach (object obj3 in hashtable)
			{
				DictionaryEntry dictionaryEntry = (DictionaryEntry)obj3;
				this.SetError((Control)dictionaryEntry.Key, (string)dictionaryEntry.Value);
			}
		}

		/// <summary>Gets or sets the rate at which the error icon flashes.</summary>
		/// <returns>The rate, in milliseconds, at which the error icon should flash. The default is 250 milliseconds.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The value is less than zero. </exception>
		// Token: 0x17000819 RID: 2073
		// (get) Token: 0x060021E1 RID: 8673 RVA: 0x000A5B1C File Offset: 0x000A3D1C
		// (set) Token: 0x060021E2 RID: 8674 RVA: 0x000A5B24 File Offset: 0x000A3D24
		[SRCategory("CatBehavior")]
		[DefaultValue(250)]
		[SRDescription("ErrorProviderBlinkRateDescr")]
		[RefreshProperties(RefreshProperties.Repaint)]
		public int BlinkRate
		{
			get
			{
				return this.blinkRate;
			}
			set
			{
				if (value < 0)
				{
					throw new ArgumentOutOfRangeException("BlinkRate", value, SR.GetString("BlinkRateMustBeZeroOrMore"));
				}
				this.blinkRate = value;
				if (this.blinkRate == 0)
				{
					this.BlinkStyle = ErrorBlinkStyle.NeverBlink;
				}
			}
		}

		// Token: 0x1700081A RID: 2074
		// (get) Token: 0x060021E3 RID: 8675 RVA: 0x000A5B5C File Offset: 0x000A3D5C
		private static Icon DefaultIcon
		{
			get
			{
				if (ErrorProvider.defaultIcon == null)
				{
					Type typeFromHandle = typeof(ErrorProvider);
					lock (typeFromHandle)
					{
						if (ErrorProvider.defaultIcon == null)
						{
							ErrorProvider.defaultIcon = new Icon(typeof(ErrorProvider), "Error.ico");
						}
					}
				}
				return ErrorProvider.defaultIcon;
			}
		}

		/// <summary>Gets or sets the <see cref="T:System.Drawing.Icon" /> that is displayed next to a control when an error description string has been set for the control.</summary>
		/// <returns>An <see cref="T:System.Drawing.Icon" /> that signals an error has occurred. The default icon consists of an exclamation point in a circle with a red background.</returns>
		/// <exception cref="T:System.ArgumentNullException">The assigned value of the <see cref="T:System.Drawing.Icon" /> is <see langword="null" />. </exception>
		// Token: 0x1700081B RID: 2075
		// (get) Token: 0x060021E4 RID: 8676 RVA: 0x000A5BC8 File Offset: 0x000A3DC8
		// (set) Token: 0x060021E5 RID: 8677 RVA: 0x000A5BD0 File Offset: 0x000A3DD0
		[Localizable(true)]
		[SRCategory("CatAppearance")]
		[SRDescription("ErrorProviderIconDescr")]
		public Icon Icon
		{
			get
			{
				return this.icon;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				this.icon = value;
				this.DisposeRegion();
				ErrorProvider.ErrorWindow[] array = new ErrorProvider.ErrorWindow[this.windows.Values.Count];
				this.windows.Values.CopyTo(array, 0);
				for (int i = 0; i < array.Length; i++)
				{
					array[i].Update(false);
				}
			}
		}

		// Token: 0x1700081C RID: 2076
		// (get) Token: 0x060021E6 RID: 8678 RVA: 0x000A5C37 File Offset: 0x000A3E37
		internal ErrorProvider.IconRegion Region
		{
			get
			{
				if (this.region == null)
				{
					this.region = new ErrorProvider.IconRegion(this.Icon);
				}
				return this.region;
			}
		}

		/// <summary>Signals the object that initialization is starting.</summary>
		// Token: 0x060021E7 RID: 8679 RVA: 0x000A5C58 File Offset: 0x000A3E58
		void ISupportInitialize.BeginInit()
		{
			this.initializing = true;
		}

		// Token: 0x060021E8 RID: 8680 RVA: 0x000A5C61 File Offset: 0x000A3E61
		private void EndInitCore()
		{
			this.initializing = false;
			if (this.setErrorManagerOnEndInit)
			{
				this.setErrorManagerOnEndInit = false;
				this.Set_ErrorManager(this.DataSource, this.DataMember, true);
			}
		}

		/// <summary>Signals the object that initialization is complete.</summary>
		// Token: 0x060021E9 RID: 8681 RVA: 0x000A5C8C File Offset: 0x000A3E8C
		void ISupportInitialize.EndInit()
		{
			ISupportInitializeNotification supportInitializeNotification = this.DataSource as ISupportInitializeNotification;
			if (supportInitializeNotification != null && !supportInitializeNotification.IsInitialized)
			{
				supportInitializeNotification.Initialized += this.DataSource_Initialized;
				return;
			}
			this.EndInitCore();
		}

		// Token: 0x060021EA RID: 8682 RVA: 0x000A5CCC File Offset: 0x000A3ECC
		private void DataSource_Initialized(object sender, EventArgs e)
		{
			ISupportInitializeNotification supportInitializeNotification = this.DataSource as ISupportInitializeNotification;
			if (supportInitializeNotification != null)
			{
				supportInitializeNotification.Initialized -= this.DataSource_Initialized;
			}
			this.EndInitCore();
		}

		/// <summary>Clears all settings associated with this component.</summary>
		// Token: 0x060021EB RID: 8683 RVA: 0x000A5D00 File Offset: 0x000A3F00
		public void Clear()
		{
			ErrorProvider.ErrorWindow[] array = new ErrorProvider.ErrorWindow[this.windows.Values.Count];
			this.windows.Values.CopyTo(array, 0);
			for (int i = 0; i < array.Length; i++)
			{
				array[i].Dispose();
			}
			this.windows.Clear();
			foreach (object obj in this.items.Values)
			{
				ErrorProvider.ControlItem controlItem = (ErrorProvider.ControlItem)obj;
				if (controlItem != null)
				{
					controlItem.Dispose();
				}
			}
			this.items.Clear();
		}

		/// <summary>Gets a value indicating whether a control can be extended.</summary>
		/// <param name="extendee">The control to be extended. </param>
		/// <returns>
		///     <see langword="true" /> if the control can be extended; otherwise, <see langword="false" />.This property will be <see langword="true" /> if the object is a <see cref="T:System.Windows.Forms.Control" /> and is not a <see cref="T:System.Windows.Forms.Form" /> or <see cref="T:System.Windows.Forms.ToolBar" />.</returns>
		// Token: 0x060021EC RID: 8684 RVA: 0x000A5DB8 File Offset: 0x000A3FB8
		public bool CanExtend(object extendee)
		{
			return extendee is Control && !(extendee is Form) && !(extendee is ToolBar);
		}

		/// <summary>Releases the unmanaged resources used by the <see cref="T:System.ComponentModel.Component" /> and optionally releases the managed resources.</summary>
		/// <param name="disposing">
		///       <see langword="true" /> to release both managed and unmanaged resources; <see langword="false" /> to release only unmanaged resources. </param>
		// Token: 0x060021ED RID: 8685 RVA: 0x000A5DD8 File Offset: 0x000A3FD8
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				this.Clear();
				this.DisposeRegion();
				this.UnwireEvents(this.errorManager);
			}
			base.Dispose(disposing);
		}

		// Token: 0x060021EE RID: 8686 RVA: 0x000A5DFC File Offset: 0x000A3FFC
		private void DisposeRegion()
		{
			if (this.region != null)
			{
				this.region.Dispose();
				this.region = null;
			}
		}

		// Token: 0x060021EF RID: 8687 RVA: 0x000A5E18 File Offset: 0x000A4018
		private ErrorProvider.ControlItem EnsureControlItem(Control control)
		{
			if (control == null)
			{
				throw new ArgumentNullException("control");
			}
			ErrorProvider.ControlItem controlItem = (ErrorProvider.ControlItem)this.items[control];
			if (controlItem == null)
			{
				int value = this.itemIdCounter + 1;
				this.itemIdCounter = value;
				controlItem = new ErrorProvider.ControlItem(this, control, (IntPtr)value);
				this.items[control] = controlItem;
			}
			return controlItem;
		}

		// Token: 0x060021F0 RID: 8688 RVA: 0x000A5E74 File Offset: 0x000A4074
		internal ErrorProvider.ErrorWindow EnsureErrorWindow(Control parent)
		{
			ErrorProvider.ErrorWindow errorWindow = (ErrorProvider.ErrorWindow)this.windows[parent];
			if (errorWindow == null)
			{
				errorWindow = new ErrorProvider.ErrorWindow(this, parent);
				this.windows[parent] = errorWindow;
			}
			return errorWindow;
		}

		/// <summary>Returns the current error description string for the specified control.</summary>
		/// <param name="control">The item to get the error description string for. </param>
		/// <returns>The error description string for the specified control.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="control" /> is <see langword="null" />.</exception>
		// Token: 0x060021F1 RID: 8689 RVA: 0x000A5EAC File Offset: 0x000A40AC
		[DefaultValue("")]
		[Localizable(true)]
		[SRCategory("CatAppearance")]
		[SRDescription("ErrorProviderErrorDescr")]
		public string GetError(Control control)
		{
			return this.EnsureControlItem(control).Error;
		}

		/// <summary>Gets a value indicating where the error icon should be placed in relation to the control.</summary>
		/// <param name="control">The control to get the icon location for. </param>
		/// <returns>One of the <see cref="T:System.Windows.Forms.ErrorIconAlignment" /> values. The default icon alignment is <see cref="F:System.Windows.Forms.ErrorIconAlignment.MiddleRight" />.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="control" /> is <see langword="null" />.</exception>
		// Token: 0x060021F2 RID: 8690 RVA: 0x000A5EBA File Offset: 0x000A40BA
		[DefaultValue(ErrorIconAlignment.MiddleRight)]
		[Localizable(true)]
		[SRCategory("CatAppearance")]
		[SRDescription("ErrorProviderIconAlignmentDescr")]
		public ErrorIconAlignment GetIconAlignment(Control control)
		{
			return this.EnsureControlItem(control).IconAlignment;
		}

		/// <summary>Returns the amount of extra space to leave next to the error icon.</summary>
		/// <param name="control">The control to get the padding for. </param>
		/// <returns>The number of pixels to leave between the icon and the control. </returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="control" /> is <see langword="null" />.</exception>
		// Token: 0x060021F3 RID: 8691 RVA: 0x000A5EC8 File Offset: 0x000A40C8
		[DefaultValue(0)]
		[Localizable(true)]
		[SRCategory("CatAppearance")]
		[SRDescription("ErrorProviderIconPaddingDescr")]
		public int GetIconPadding(Control control)
		{
			return this.EnsureControlItem(control).IconPadding;
		}

		// Token: 0x060021F4 RID: 8692 RVA: 0x000A5ED6 File Offset: 0x000A40D6
		private void ResetIcon()
		{
			this.Icon = ErrorProvider.DefaultIcon;
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.ErrorProvider.RightToLeftChanged" /> event. </summary>
		/// <param name="e">A <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x060021F5 RID: 8693 RVA: 0x000A5EE4 File Offset: 0x000A40E4
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void OnRightToLeftChanged(EventArgs e)
		{
			foreach (object obj in this.windows.Values)
			{
				ErrorProvider.ErrorWindow errorWindow = (ErrorProvider.ErrorWindow)obj;
				errorWindow.Update(false);
			}
			if (this.onRightToLeftChanged != null)
			{
				this.onRightToLeftChanged(this, e);
			}
		}

		/// <summary>Sets the error description string for the specified control.</summary>
		/// <param name="control">The control to set the error description string for. </param>
		/// <param name="value">The error description string, or <see langword="null" /> or <see cref="F:System.String.Empty" /> to remove the error.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="control" /> is <see langword="null" />.</exception>
		// Token: 0x060021F6 RID: 8694 RVA: 0x000A5F58 File Offset: 0x000A4158
		public void SetError(Control control, string value)
		{
			this.EnsureControlItem(control).Error = value;
		}

		/// <summary>Sets the location where the error icon should be placed in relation to the control.</summary>
		/// <param name="control">The control to set the icon location for. </param>
		/// <param name="value">One of the <see cref="T:System.Windows.Forms.ErrorIconAlignment" /> values. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="control" /> is <see langword="null" />.</exception>
		// Token: 0x060021F7 RID: 8695 RVA: 0x000A5F67 File Offset: 0x000A4167
		public void SetIconAlignment(Control control, ErrorIconAlignment value)
		{
			this.EnsureControlItem(control).IconAlignment = value;
		}

		/// <summary>Sets the amount of extra space to leave between the specified control and the error icon.</summary>
		/// <param name="control">The <paramref name="control" /> to set the padding for. </param>
		/// <param name="padding">The number of pixels to add between the icon and the <paramref name="control" />. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="control" /> is <see langword="null" />.</exception>
		// Token: 0x060021F8 RID: 8696 RVA: 0x000A5F76 File Offset: 0x000A4176
		public void SetIconPadding(Control control, int padding)
		{
			this.EnsureControlItem(control).IconPadding = padding;
		}

		// Token: 0x060021F9 RID: 8697 RVA: 0x000A5F85 File Offset: 0x000A4185
		private bool ShouldSerializeIcon()
		{
			return this.Icon != ErrorProvider.DefaultIcon;
		}

		// Token: 0x04000EBD RID: 3773
		private Hashtable items = new Hashtable();

		// Token: 0x04000EBE RID: 3774
		private Hashtable windows = new Hashtable();

		// Token: 0x04000EBF RID: 3775
		private Icon icon = ErrorProvider.DefaultIcon;

		// Token: 0x04000EC0 RID: 3776
		private ErrorProvider.IconRegion region;

		// Token: 0x04000EC1 RID: 3777
		private int itemIdCounter;

		// Token: 0x04000EC2 RID: 3778
		private int blinkRate;

		// Token: 0x04000EC3 RID: 3779
		private ErrorBlinkStyle blinkStyle;

		// Token: 0x04000EC4 RID: 3780
		private bool showIcon = true;

		// Token: 0x04000EC5 RID: 3781
		private bool inSetErrorManager;

		// Token: 0x04000EC6 RID: 3782
		private bool setErrorManagerOnEndInit;

		// Token: 0x04000EC7 RID: 3783
		private bool initializing;

		// Token: 0x04000EC8 RID: 3784
		[ThreadStatic]
		private static Icon defaultIcon;

		// Token: 0x04000EC9 RID: 3785
		private const int defaultBlinkRate = 250;

		// Token: 0x04000ECA RID: 3786
		private const ErrorBlinkStyle defaultBlinkStyle = ErrorBlinkStyle.BlinkIfDifferentError;

		// Token: 0x04000ECB RID: 3787
		private const ErrorIconAlignment defaultIconAlignment = ErrorIconAlignment.MiddleRight;

		// Token: 0x04000ECC RID: 3788
		private ContainerControl parentControl;

		// Token: 0x04000ECD RID: 3789
		private object dataSource;

		// Token: 0x04000ECE RID: 3790
		private string dataMember;

		// Token: 0x04000ECF RID: 3791
		private BindingManagerBase errorManager;

		// Token: 0x04000ED0 RID: 3792
		private EventHandler currentChanged;

		// Token: 0x04000ED1 RID: 3793
		private EventHandler propChangedEvent;

		// Token: 0x04000ED2 RID: 3794
		private EventHandler onRightToLeftChanged;

		// Token: 0x04000ED3 RID: 3795
		private bool rightToLeft;

		// Token: 0x04000ED4 RID: 3796
		private object userData;

		// Token: 0x020005CD RID: 1485
		internal class ErrorWindow : NativeWindow
		{
			// Token: 0x06005A45 RID: 23109 RVA: 0x0017C250 File Offset: 0x0017A450
			public ErrorWindow(ErrorProvider provider, Control parent)
			{
				this.provider = provider;
				this.parent = parent;
			}

			// Token: 0x06005A46 RID: 23110 RVA: 0x0017C2A4 File Offset: 0x0017A4A4
			public void Add(ErrorProvider.ControlItem item)
			{
				this.items.Add(item);
				if (!this.EnsureCreated())
				{
					return;
				}
				NativeMethods.TOOLINFO_T toolinfo_T = new NativeMethods.TOOLINFO_T();
				toolinfo_T.cbSize = Marshal.SizeOf(toolinfo_T);
				toolinfo_T.hwnd = base.Handle;
				toolinfo_T.uId = item.Id;
				toolinfo_T.lpszText = item.Error;
				toolinfo_T.uFlags = 16;
				UnsafeNativeMethods.SendMessage(new HandleRef(this.tipWindow, this.tipWindow.Handle), NativeMethods.TTM_ADDTOOL, 0, toolinfo_T);
				this.Update(false);
			}

			// Token: 0x06005A47 RID: 23111 RVA: 0x0017C32F File Offset: 0x0017A52F
			public void Dispose()
			{
				this.EnsureDestroyed();
			}

			// Token: 0x06005A48 RID: 23112 RVA: 0x0017C338 File Offset: 0x0017A538
			private bool EnsureCreated()
			{
				if (base.Handle == IntPtr.Zero)
				{
					if (!this.parent.IsHandleCreated)
					{
						return false;
					}
					this.CreateHandle(new CreateParams
					{
						Caption = string.Empty,
						Style = 1342177280,
						ClassStyle = 8,
						X = 0,
						Y = 0,
						Width = 0,
						Height = 0,
						Parent = this.parent.Handle
					});
					NativeMethods.INITCOMMONCONTROLSEX initcommoncontrolsex = new NativeMethods.INITCOMMONCONTROLSEX();
					initcommoncontrolsex.dwICC = 8;
					initcommoncontrolsex.dwSize = Marshal.SizeOf(initcommoncontrolsex);
					SafeNativeMethods.InitCommonControlsEx(initcommoncontrolsex);
					CreateParams createParams = new CreateParams();
					createParams.Parent = base.Handle;
					createParams.ClassName = "tooltips_class32";
					createParams.Style = 1;
					this.tipWindow = new NativeWindow();
					this.tipWindow.CreateHandle(createParams);
					UnsafeNativeMethods.SendMessage(new HandleRef(this.tipWindow, this.tipWindow.Handle), 1048, 0, SystemInformation.MaxWindowTrackSize.Width);
					SafeNativeMethods.SetWindowPos(new HandleRef(this.tipWindow, this.tipWindow.Handle), NativeMethods.HWND_TOP, 0, 0, 0, 0, 19);
					UnsafeNativeMethods.SendMessage(new HandleRef(this.tipWindow, this.tipWindow.Handle), 1027, 3, 0);
				}
				return true;
			}

			// Token: 0x06005A49 RID: 23113 RVA: 0x0017C498 File Offset: 0x0017A698
			private void EnsureDestroyed()
			{
				if (this.timer != null)
				{
					this.timer.Dispose();
					this.timer = null;
				}
				if (this.tipWindow != null)
				{
					this.tipWindow.DestroyHandle();
					this.tipWindow = null;
				}
				SafeNativeMethods.SetWindowPos(new HandleRef(this, base.Handle), NativeMethods.HWND_TOP, this.windowBounds.X, this.windowBounds.Y, this.windowBounds.Width, this.windowBounds.Height, 131);
				if (this.parent != null)
				{
					this.parent.Invalidate(true);
				}
				this.DestroyHandle();
				if (this.mirrordc != null)
				{
					this.mirrordc.Dispose();
				}
			}

			// Token: 0x06005A4A RID: 23114 RVA: 0x0017C550 File Offset: 0x0017A750
			private void CreateMirrorDC(IntPtr hdc, int originOffset)
			{
				this.mirrordc = DeviceContext.FromHdc(hdc);
				if (this.parent.IsMirrored && this.mirrordc != null)
				{
					this.mirrordc.SaveHdc();
					this.mirrordcExtent = this.mirrordc.ViewportExtent;
					this.mirrordcOrigin = this.mirrordc.ViewportOrigin;
					this.mirrordcMode = this.mirrordc.SetMapMode(DeviceContextMapMode.Anisotropic);
					this.mirrordc.ViewportExtent = new Size(-this.mirrordcExtent.Width, this.mirrordcExtent.Height);
					this.mirrordc.ViewportOrigin = new Point(this.mirrordcOrigin.X + originOffset, this.mirrordcOrigin.Y);
				}
			}

			// Token: 0x06005A4B RID: 23115 RVA: 0x0017C614 File Offset: 0x0017A814
			private void RestoreMirrorDC()
			{
				if (this.parent.IsMirrored && this.mirrordc != null)
				{
					this.mirrordc.ViewportExtent = this.mirrordcExtent;
					this.mirrordc.ViewportOrigin = this.mirrordcOrigin;
					this.mirrordc.SetMapMode(this.mirrordcMode);
					this.mirrordc.RestoreHdc();
					this.mirrordc.Dispose();
				}
				this.mirrordc = null;
				this.mirrordcExtent = Size.Empty;
				this.mirrordcOrigin = Point.Empty;
				this.mirrordcMode = DeviceContextMapMode.Text;
			}

			// Token: 0x06005A4C RID: 23116 RVA: 0x0017C6A4 File Offset: 0x0017A8A4
			private void OnPaint(ref Message m)
			{
				NativeMethods.PAINTSTRUCT paintstruct = default(NativeMethods.PAINTSTRUCT);
				IntPtr hdc = UnsafeNativeMethods.BeginPaint(new HandleRef(this, base.Handle), ref paintstruct);
				try
				{
					this.CreateMirrorDC(hdc, this.windowBounds.Width - 1);
					try
					{
						for (int i = 0; i < this.items.Count; i++)
						{
							ErrorProvider.ControlItem controlItem = (ErrorProvider.ControlItem)this.items[i];
							Rectangle iconBounds = controlItem.GetIconBounds(this.provider.Region.Size);
							SafeNativeMethods.DrawIconEx(new HandleRef(this, this.mirrordc.Hdc), iconBounds.X - this.windowBounds.X, iconBounds.Y - this.windowBounds.Y, new HandleRef(this.provider.Region, this.provider.Region.IconHandle), iconBounds.Width, iconBounds.Height, 0, NativeMethods.NullHandleRef, 3);
						}
					}
					finally
					{
						this.RestoreMirrorDC();
					}
				}
				finally
				{
					UnsafeNativeMethods.EndPaint(new HandleRef(this, base.Handle), ref paintstruct);
				}
			}

			// Token: 0x06005A4D RID: 23117 RVA: 0x000337A1 File Offset: 0x000319A1
			protected override void OnThreadException(Exception e)
			{
				Application.OnThreadException(e);
			}

			// Token: 0x06005A4E RID: 23118 RVA: 0x0017C7D4 File Offset: 0x0017A9D4
			private void OnTimer(object sender, EventArgs e)
			{
				int num = 0;
				for (int i = 0; i < this.items.Count; i++)
				{
					num += ((ErrorProvider.ControlItem)this.items[i]).BlinkPhase;
				}
				if (num == 0 && this.provider.BlinkStyle != ErrorBlinkStyle.AlwaysBlink)
				{
					this.timer.Stop();
				}
				this.Update(true);
			}

			// Token: 0x06005A4F RID: 23119 RVA: 0x0017C838 File Offset: 0x0017AA38
			private void OnToolTipVisibilityChanging(IntPtr id, bool toolTipShown)
			{
				for (int i = 0; i < this.items.Count; i++)
				{
					if (((ErrorProvider.ControlItem)this.items[i]).Id == id)
					{
						((ErrorProvider.ControlItem)this.items[i]).ToolTipShown = toolTipShown;
					}
				}
			}

			// Token: 0x06005A50 RID: 23120 RVA: 0x0017C890 File Offset: 0x0017AA90
			public void Remove(ErrorProvider.ControlItem item)
			{
				this.items.Remove(item);
				if (this.tipWindow != null)
				{
					NativeMethods.TOOLINFO_T toolinfo_T = new NativeMethods.TOOLINFO_T();
					toolinfo_T.cbSize = Marshal.SizeOf(toolinfo_T);
					toolinfo_T.hwnd = base.Handle;
					toolinfo_T.uId = item.Id;
					UnsafeNativeMethods.SendMessage(new HandleRef(this.tipWindow, this.tipWindow.Handle), NativeMethods.TTM_DELTOOL, 0, toolinfo_T);
				}
				if (this.items.Count == 0)
				{
					this.EnsureDestroyed();
					return;
				}
				this.Update(false);
			}

			// Token: 0x06005A51 RID: 23121 RVA: 0x0017C91C File Offset: 0x0017AB1C
			internal void StartBlinking()
			{
				if (this.timer == null)
				{
					this.timer = new Timer();
					this.timer.Tick += this.OnTimer;
				}
				this.timer.Interval = this.provider.BlinkRate;
				this.timer.Start();
				this.Update(false);
			}

			// Token: 0x06005A52 RID: 23122 RVA: 0x0017C97B File Offset: 0x0017AB7B
			internal void StopBlinking()
			{
				if (this.timer != null)
				{
					this.timer.Stop();
				}
				this.Update(false);
			}

			// Token: 0x06005A53 RID: 23123 RVA: 0x0017C998 File Offset: 0x0017AB98
			public void Update(bool timerCaused)
			{
				ErrorProvider.IconRegion region = this.provider.Region;
				Size size = region.Size;
				this.windowBounds = Rectangle.Empty;
				for (int i = 0; i < this.items.Count; i++)
				{
					ErrorProvider.ControlItem controlItem = (ErrorProvider.ControlItem)this.items[i];
					Rectangle iconBounds = controlItem.GetIconBounds(size);
					if (this.windowBounds.IsEmpty)
					{
						this.windowBounds = iconBounds;
					}
					else
					{
						this.windowBounds = Rectangle.Union(this.windowBounds, iconBounds);
					}
				}
				Region region2 = new Region(new Rectangle(0, 0, 0, 0));
				IntPtr intPtr = IntPtr.Zero;
				try
				{
					for (int j = 0; j < this.items.Count; j++)
					{
						ErrorProvider.ControlItem controlItem2 = (ErrorProvider.ControlItem)this.items[j];
						Rectangle iconBounds2 = controlItem2.GetIconBounds(size);
						iconBounds2.X -= this.windowBounds.X;
						iconBounds2.Y -= this.windowBounds.Y;
						bool flag = true;
						if (!controlItem2.ToolTipShown)
						{
							switch (this.provider.BlinkStyle)
							{
							case ErrorBlinkStyle.BlinkIfDifferentError:
								flag = (controlItem2.BlinkPhase == 0 || (controlItem2.BlinkPhase > 0 && (controlItem2.BlinkPhase & 1) == (j & 1)));
								break;
							case ErrorBlinkStyle.AlwaysBlink:
								flag = ((j & 1) == 0 == this.provider.showIcon);
								break;
							}
						}
						if (flag)
						{
							region.Region.Translate(iconBounds2.X, iconBounds2.Y);
							region2.Union(region.Region);
							region.Region.Translate(-iconBounds2.X, -iconBounds2.Y);
						}
						if (this.tipWindow != null)
						{
							NativeMethods.TOOLINFO_T toolinfo_T = new NativeMethods.TOOLINFO_T();
							toolinfo_T.cbSize = Marshal.SizeOf(toolinfo_T);
							toolinfo_T.hwnd = base.Handle;
							toolinfo_T.uId = controlItem2.Id;
							toolinfo_T.lpszText = controlItem2.Error;
							toolinfo_T.rect = NativeMethods.RECT.FromXYWH(iconBounds2.X, iconBounds2.Y, iconBounds2.Width, iconBounds2.Height);
							toolinfo_T.uFlags = 16;
							if (this.provider.RightToLeft)
							{
								toolinfo_T.uFlags |= 4;
							}
							UnsafeNativeMethods.SendMessage(new HandleRef(this.tipWindow, this.tipWindow.Handle), NativeMethods.TTM_SETTOOLINFO, 0, toolinfo_T);
						}
						if (timerCaused && controlItem2.BlinkPhase > 0)
						{
							ErrorProvider.ControlItem controlItem3 = controlItem2;
							int blinkPhase = controlItem3.BlinkPhase;
							controlItem3.BlinkPhase = blinkPhase - 1;
						}
					}
					if (timerCaused)
					{
						this.provider.showIcon = !this.provider.showIcon;
					}
					DeviceContext deviceContext = null;
					using (DeviceContext deviceContext = DeviceContext.FromHwnd(base.Handle))
					{
						this.CreateMirrorDC(deviceContext.Hdc, this.windowBounds.Width);
						Graphics graphics = Graphics.FromHdcInternal(this.mirrordc.Hdc);
						try
						{
							intPtr = region2.GetHrgn(graphics);
							System.Internal.HandleCollector.Add(intPtr, NativeMethods.CommonHandles.GDI);
						}
						finally
						{
							graphics.Dispose();
							this.RestoreMirrorDC();
						}
						if (UnsafeNativeMethods.SetWindowRgn(new HandleRef(this, base.Handle), new HandleRef(region2, intPtr), true) != 0)
						{
							intPtr = IntPtr.Zero;
						}
					}
				}
				finally
				{
					region2.Dispose();
					if (intPtr != IntPtr.Zero)
					{
						SafeNativeMethods.DeleteObject(new HandleRef(null, intPtr));
					}
				}
				SafeNativeMethods.SetWindowPos(new HandleRef(this, base.Handle), NativeMethods.HWND_TOP, this.windowBounds.X, this.windowBounds.Y, this.windowBounds.Width, this.windowBounds.Height, 16);
				SafeNativeMethods.InvalidateRect(new HandleRef(this, base.Handle), null, false);
			}

			// Token: 0x06005A54 RID: 23124 RVA: 0x0017CDA8 File Offset: 0x0017AFA8
			protected override void WndProc(ref Message m)
			{
				int msg = m.Msg;
				if (msg != 15)
				{
					if (msg != 20)
					{
						if (msg == 78)
						{
							NativeMethods.NMHDR nmhdr = (NativeMethods.NMHDR)m.GetLParam(typeof(NativeMethods.NMHDR));
							if (nmhdr.code == -521 || nmhdr.code == -522)
							{
								this.OnToolTipVisibilityChanging(nmhdr.idFrom, nmhdr.code == -521);
								return;
							}
						}
						else
						{
							base.WndProc(ref m);
						}
					}
					return;
				}
				this.OnPaint(ref m);
			}

			// Token: 0x04003956 RID: 14678
			private ArrayList items = new ArrayList();

			// Token: 0x04003957 RID: 14679
			private Control parent;

			// Token: 0x04003958 RID: 14680
			private ErrorProvider provider;

			// Token: 0x04003959 RID: 14681
			private Rectangle windowBounds = Rectangle.Empty;

			// Token: 0x0400395A RID: 14682
			private Timer timer;

			// Token: 0x0400395B RID: 14683
			private NativeWindow tipWindow;

			// Token: 0x0400395C RID: 14684
			private DeviceContext mirrordc;

			// Token: 0x0400395D RID: 14685
			private Size mirrordcExtent = Size.Empty;

			// Token: 0x0400395E RID: 14686
			private Point mirrordcOrigin = Point.Empty;

			// Token: 0x0400395F RID: 14687
			private DeviceContextMapMode mirrordcMode = DeviceContextMapMode.Text;
		}

		// Token: 0x020005CE RID: 1486
		internal class ControlItem
		{
			// Token: 0x06005A55 RID: 23125 RVA: 0x0017CE24 File Offset: 0x0017B024
			public ControlItem(ErrorProvider provider, Control control, IntPtr id)
			{
				this.toolTipShown = false;
				this.iconAlignment = ErrorIconAlignment.MiddleRight;
				this.error = string.Empty;
				this.id = id;
				this.control = control;
				this.provider = provider;
				this.control.HandleCreated += this.OnCreateHandle;
				this.control.HandleDestroyed += this.OnDestroyHandle;
				this.control.LocationChanged += this.OnBoundsChanged;
				this.control.SizeChanged += this.OnBoundsChanged;
				this.control.VisibleChanged += this.OnParentVisibleChanged;
				this.control.ParentChanged += this.OnParentVisibleChanged;
			}

			// Token: 0x06005A56 RID: 23126 RVA: 0x0017CEF0 File Offset: 0x0017B0F0
			public void Dispose()
			{
				if (this.control != null)
				{
					this.control.HandleCreated -= this.OnCreateHandle;
					this.control.HandleDestroyed -= this.OnDestroyHandle;
					this.control.LocationChanged -= this.OnBoundsChanged;
					this.control.SizeChanged -= this.OnBoundsChanged;
					this.control.VisibleChanged -= this.OnParentVisibleChanged;
					this.control.ParentChanged -= this.OnParentVisibleChanged;
				}
				this.error = string.Empty;
			}

			// Token: 0x170015DB RID: 5595
			// (get) Token: 0x06005A57 RID: 23127 RVA: 0x0017CF9D File Offset: 0x0017B19D
			public IntPtr Id
			{
				get
				{
					return this.id;
				}
			}

			// Token: 0x170015DC RID: 5596
			// (get) Token: 0x06005A58 RID: 23128 RVA: 0x0017CFA5 File Offset: 0x0017B1A5
			// (set) Token: 0x06005A59 RID: 23129 RVA: 0x0017CFAD File Offset: 0x0017B1AD
			public int BlinkPhase
			{
				get
				{
					return this.blinkPhase;
				}
				set
				{
					this.blinkPhase = value;
				}
			}

			// Token: 0x170015DD RID: 5597
			// (get) Token: 0x06005A5A RID: 23130 RVA: 0x0017CFB6 File Offset: 0x0017B1B6
			// (set) Token: 0x06005A5B RID: 23131 RVA: 0x0017CFBE File Offset: 0x0017B1BE
			public int IconPadding
			{
				get
				{
					return this.iconPadding;
				}
				set
				{
					if (this.iconPadding != value)
					{
						this.iconPadding = value;
						this.UpdateWindow();
					}
				}
			}

			// Token: 0x170015DE RID: 5598
			// (get) Token: 0x06005A5C RID: 23132 RVA: 0x0017CFD6 File Offset: 0x0017B1D6
			// (set) Token: 0x06005A5D RID: 23133 RVA: 0x0017CFE0 File Offset: 0x0017B1E0
			public string Error
			{
				get
				{
					return this.error;
				}
				set
				{
					if (value == null)
					{
						value = "";
					}
					if (this.error.Equals(value) && this.provider.BlinkStyle != ErrorBlinkStyle.AlwaysBlink)
					{
						return;
					}
					bool flag = this.error.Length == 0;
					this.error = value;
					if (value.Length == 0)
					{
						this.RemoveFromWindow();
						return;
					}
					if (flag)
					{
						this.AddToWindow();
						return;
					}
					if (this.provider.BlinkStyle != ErrorBlinkStyle.NeverBlink)
					{
						this.StartBlinking();
						return;
					}
					this.UpdateWindow();
				}
			}

			// Token: 0x170015DF RID: 5599
			// (get) Token: 0x06005A5E RID: 23134 RVA: 0x0017D05E File Offset: 0x0017B25E
			// (set) Token: 0x06005A5F RID: 23135 RVA: 0x0017D066 File Offset: 0x0017B266
			public ErrorIconAlignment IconAlignment
			{
				get
				{
					return this.iconAlignment;
				}
				set
				{
					if (this.iconAlignment != value)
					{
						if (!ClientUtils.IsEnumValid(value, (int)value, 0, 5))
						{
							throw new InvalidEnumArgumentException("value", (int)value, typeof(ErrorIconAlignment));
						}
						this.iconAlignment = value;
						this.UpdateWindow();
					}
				}
			}

			// Token: 0x170015E0 RID: 5600
			// (get) Token: 0x06005A60 RID: 23136 RVA: 0x0017D0A4 File Offset: 0x0017B2A4
			// (set) Token: 0x06005A61 RID: 23137 RVA: 0x0017D0AC File Offset: 0x0017B2AC
			public bool ToolTipShown
			{
				get
				{
					return this.toolTipShown;
				}
				set
				{
					this.toolTipShown = value;
				}
			}

			// Token: 0x06005A62 RID: 23138 RVA: 0x0017D0B5 File Offset: 0x0017B2B5
			internal ErrorIconAlignment RTLTranslateIconAlignment(ErrorIconAlignment align)
			{
				if (!this.provider.RightToLeft)
				{
					return align;
				}
				switch (align)
				{
				case ErrorIconAlignment.TopLeft:
					return ErrorIconAlignment.TopRight;
				case ErrorIconAlignment.TopRight:
					return ErrorIconAlignment.TopLeft;
				case ErrorIconAlignment.MiddleLeft:
					return ErrorIconAlignment.MiddleRight;
				case ErrorIconAlignment.MiddleRight:
					return ErrorIconAlignment.MiddleLeft;
				case ErrorIconAlignment.BottomLeft:
					return ErrorIconAlignment.BottomRight;
				case ErrorIconAlignment.BottomRight:
					return ErrorIconAlignment.BottomLeft;
				default:
					return align;
				}
			}

			// Token: 0x06005A63 RID: 23139 RVA: 0x0017D0F4 File Offset: 0x0017B2F4
			internal Rectangle GetIconBounds(Size size)
			{
				int x = 0;
				int y = 0;
				switch (this.RTLTranslateIconAlignment(this.IconAlignment))
				{
				case ErrorIconAlignment.TopLeft:
				case ErrorIconAlignment.MiddleLeft:
				case ErrorIconAlignment.BottomLeft:
					x = this.control.Left - size.Width - this.iconPadding;
					break;
				case ErrorIconAlignment.TopRight:
				case ErrorIconAlignment.MiddleRight:
				case ErrorIconAlignment.BottomRight:
					x = this.control.Right + this.iconPadding;
					break;
				}
				switch (this.IconAlignment)
				{
				case ErrorIconAlignment.TopLeft:
				case ErrorIconAlignment.TopRight:
					y = this.control.Top;
					break;
				case ErrorIconAlignment.MiddleLeft:
				case ErrorIconAlignment.MiddleRight:
					y = this.control.Top + (this.control.Height - size.Height) / 2;
					break;
				case ErrorIconAlignment.BottomLeft:
				case ErrorIconAlignment.BottomRight:
					y = this.control.Bottom - size.Height;
					break;
				}
				return new Rectangle(x, y, size.Width, size.Height);
			}

			// Token: 0x06005A64 RID: 23140 RVA: 0x0017D1E4 File Offset: 0x0017B3E4
			private void UpdateWindow()
			{
				if (this.window != null)
				{
					this.window.Update(false);
				}
			}

			// Token: 0x06005A65 RID: 23141 RVA: 0x0017D1FA File Offset: 0x0017B3FA
			private void StartBlinking()
			{
				if (this.window != null)
				{
					this.BlinkPhase = 10;
					this.window.StartBlinking();
				}
			}

			// Token: 0x06005A66 RID: 23142 RVA: 0x0017D218 File Offset: 0x0017B418
			private void AddToWindow()
			{
				if (this.window == null && (this.control.Created || this.control.RecreatingHandle) && this.control.Visible && this.control.ParentInternal != null && this.error.Length > 0)
				{
					this.window = this.provider.EnsureErrorWindow(this.control.ParentInternal);
					this.window.Add(this);
					if (this.provider.BlinkStyle != ErrorBlinkStyle.NeverBlink)
					{
						this.StartBlinking();
					}
				}
			}

			// Token: 0x06005A67 RID: 23143 RVA: 0x0017D2AB File Offset: 0x0017B4AB
			private void RemoveFromWindow()
			{
				if (this.window != null)
				{
					this.window.Remove(this);
					this.window = null;
				}
			}

			// Token: 0x06005A68 RID: 23144 RVA: 0x0017D2C8 File Offset: 0x0017B4C8
			private void OnBoundsChanged(object sender, EventArgs e)
			{
				this.UpdateWindow();
			}

			// Token: 0x06005A69 RID: 23145 RVA: 0x0017D2D0 File Offset: 0x0017B4D0
			private void OnParentVisibleChanged(object sender, EventArgs e)
			{
				this.BlinkPhase = 0;
				this.RemoveFromWindow();
				this.AddToWindow();
			}

			// Token: 0x06005A6A RID: 23146 RVA: 0x0017D2E5 File Offset: 0x0017B4E5
			private void OnCreateHandle(object sender, EventArgs e)
			{
				this.AddToWindow();
			}

			// Token: 0x06005A6B RID: 23147 RVA: 0x0017D2ED File Offset: 0x0017B4ED
			private void OnDestroyHandle(object sender, EventArgs e)
			{
				this.RemoveFromWindow();
			}

			// Token: 0x04003960 RID: 14688
			private string error;

			// Token: 0x04003961 RID: 14689
			private Control control;

			// Token: 0x04003962 RID: 14690
			private ErrorProvider.ErrorWindow window;

			// Token: 0x04003963 RID: 14691
			private ErrorProvider provider;

			// Token: 0x04003964 RID: 14692
			private int blinkPhase;

			// Token: 0x04003965 RID: 14693
			private IntPtr id;

			// Token: 0x04003966 RID: 14694
			private int iconPadding;

			// Token: 0x04003967 RID: 14695
			private bool toolTipShown;

			// Token: 0x04003968 RID: 14696
			private ErrorIconAlignment iconAlignment;

			// Token: 0x04003969 RID: 14697
			private const int startingBlinkPhase = 10;
		}

		// Token: 0x020005CF RID: 1487
		internal class IconRegion
		{
			// Token: 0x06005A6C RID: 23148 RVA: 0x0017D2F5 File Offset: 0x0017B4F5
			public IconRegion(Icon icon)
			{
				this.icon = new Icon(icon, 16, 16);
			}

			// Token: 0x170015E1 RID: 5601
			// (get) Token: 0x06005A6D RID: 23149 RVA: 0x0017D30D File Offset: 0x0017B50D
			public IntPtr IconHandle
			{
				get
				{
					return this.icon.Handle;
				}
			}

			// Token: 0x170015E2 RID: 5602
			// (get) Token: 0x06005A6E RID: 23150 RVA: 0x0017D31C File Offset: 0x0017B51C
			public Region Region
			{
				get
				{
					if (this.region == null)
					{
						this.region = new Region(new Rectangle(0, 0, 0, 0));
						IntPtr intPtr = IntPtr.Zero;
						try
						{
							Size size = this.icon.Size;
							Bitmap bitmap = this.icon.ToBitmap();
							bitmap.MakeTransparent();
							intPtr = ControlPaint.CreateHBitmapTransparencyMask(bitmap);
							bitmap.Dispose();
							int num = 16;
							int num2 = 2 * ((size.Width + 15) / num);
							byte[] array = new byte[num2 * size.Height];
							SafeNativeMethods.GetBitmapBits(new HandleRef(null, intPtr), array.Length, array);
							for (int i = 0; i < size.Height; i++)
							{
								for (int j = 0; j < size.Width; j++)
								{
									if (((int)array[i * num2 + j / 8] & 1 << 7 - j % 8) == 0)
									{
										this.region.Union(new Rectangle(j, i, 1, 1));
									}
								}
							}
							this.region.Intersect(new Rectangle(0, 0, size.Width, size.Height));
						}
						finally
						{
							if (intPtr != IntPtr.Zero)
							{
								SafeNativeMethods.DeleteObject(new HandleRef(null, intPtr));
							}
						}
					}
					return this.region;
				}
			}

			// Token: 0x170015E3 RID: 5603
			// (get) Token: 0x06005A6F RID: 23151 RVA: 0x0017D460 File Offset: 0x0017B660
			public Size Size
			{
				get
				{
					return this.icon.Size;
				}
			}

			// Token: 0x06005A70 RID: 23152 RVA: 0x0017D46D File Offset: 0x0017B66D
			public void Dispose()
			{
				if (this.region != null)
				{
					this.region.Dispose();
					this.region = null;
				}
				this.icon.Dispose();
			}

			// Token: 0x0400396A RID: 14698
			private Region region;

			// Token: 0x0400396B RID: 14699
			private Icon icon;
		}
	}
}
