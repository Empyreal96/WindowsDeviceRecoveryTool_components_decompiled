using System;
using System.ComponentModel;

namespace System.Windows.Controls
{
	/// <summary>Defines how you want the group to look at each level.</summary>
	// Token: 0x020004E1 RID: 1249
	[Localizability(LocalizationCategory.None, Readability = Readability.Unreadable)]
	public class GroupStyle : INotifyPropertyChanged
	{
		// Token: 0x06004DE8 RID: 19944 RVA: 0x0015FA2C File Offset: 0x0015DC2C
		static GroupStyle()
		{
			ItemsPanelTemplate itemsPanelTemplate = new ItemsPanelTemplate(new FrameworkElementFactory(typeof(StackPanel)));
			itemsPanelTemplate.Seal();
			GroupStyle.DefaultGroupPanel = itemsPanelTemplate;
			GroupStyle.DefaultStackPanel = itemsPanelTemplate;
			itemsPanelTemplate = new ItemsPanelTemplate(new FrameworkElementFactory(typeof(VirtualizingStackPanel)));
			itemsPanelTemplate.Seal();
			GroupStyle.DefaultVirtualizingStackPanel = itemsPanelTemplate;
			GroupStyle.s_DefaultGroupStyle = new GroupStyle();
		}

		/// <summary>Occurs when a property value changes.</summary>
		// Token: 0x140000DD RID: 221
		// (add) Token: 0x06004DE9 RID: 19945 RVA: 0x0015FA8B File Offset: 0x0015DC8B
		// (remove) Token: 0x06004DEA RID: 19946 RVA: 0x0015FA94 File Offset: 0x0015DC94
		event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged
		{
			add
			{
				this.PropertyChanged += value;
			}
			remove
			{
				this.PropertyChanged -= value;
			}
		}

		/// <summary>Occurs when a property value changes.</summary>
		// Token: 0x140000DE RID: 222
		// (add) Token: 0x06004DEB RID: 19947 RVA: 0x0015FAA0 File Offset: 0x0015DCA0
		// (remove) Token: 0x06004DEC RID: 19948 RVA: 0x0015FAD8 File Offset: 0x0015DCD8
		protected virtual event PropertyChangedEventHandler PropertyChanged;

		/// <summary>Raises the <see cref="E:System.Windows.Controls.GroupStyle.PropertyChanged" /> event using the provided arguments.</summary>
		/// <param name="e">Arguments of the event being raised.</param>
		// Token: 0x06004DED RID: 19949 RVA: 0x0015FB0D File Offset: 0x0015DD0D
		protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
		{
			if (this.PropertyChanged != null)
			{
				this.PropertyChanged(this, e);
			}
		}

		/// <summary>Gets or sets a template that creates the panel used to layout the items.</summary>
		/// <returns>An <see cref="T:System.Windows.Controls.ItemsPanelTemplate" /> object that creates the panel used to layout the items.</returns>
		// Token: 0x170012F1 RID: 4849
		// (get) Token: 0x06004DEE RID: 19950 RVA: 0x0015FB24 File Offset: 0x0015DD24
		// (set) Token: 0x06004DEF RID: 19951 RVA: 0x0015FB2C File Offset: 0x0015DD2C
		public ItemsPanelTemplate Panel
		{
			get
			{
				return this._panel;
			}
			set
			{
				this._panel = value;
				this.OnPropertyChanged("Panel");
			}
		}

		/// <summary>Gets or sets the style that is applied to the <see cref="T:System.Windows.Controls.GroupItem" /> generated for each item.</summary>
		/// <returns>The style that is applied to the <see cref="T:System.Windows.Controls.GroupItem" /> generated for each item. The default is <see langword="null" />.</returns>
		// Token: 0x170012F2 RID: 4850
		// (get) Token: 0x06004DF0 RID: 19952 RVA: 0x0015FB40 File Offset: 0x0015DD40
		// (set) Token: 0x06004DF1 RID: 19953 RVA: 0x0015FB48 File Offset: 0x0015DD48
		[DefaultValue(null)]
		public Style ContainerStyle
		{
			get
			{
				return this._containerStyle;
			}
			set
			{
				this._containerStyle = value;
				this.OnPropertyChanged("ContainerStyle");
			}
		}

		/// <summary>Enables the application writer to provide custom selection logic for a style to apply to each generated <see cref="T:System.Windows.Controls.GroupItem" />.</summary>
		/// <returns>An object that derives from <see cref="T:System.Windows.Controls.StyleSelector" />. The default is <see langword="null" />.</returns>
		// Token: 0x170012F3 RID: 4851
		// (get) Token: 0x06004DF2 RID: 19954 RVA: 0x0015FB5C File Offset: 0x0015DD5C
		// (set) Token: 0x06004DF3 RID: 19955 RVA: 0x0015FB64 File Offset: 0x0015DD64
		[DefaultValue(null)]
		public StyleSelector ContainerStyleSelector
		{
			get
			{
				return this._containerStyleSelector;
			}
			set
			{
				this._containerStyleSelector = value;
				this.OnPropertyChanged("ContainerStyleSelector");
			}
		}

		/// <summary>Gets or sets the template that is used to display the group header.</summary>
		/// <returns>A <see cref="T:System.Windows.DataTemplate" /> object that is used to display the group header. The default is <see langword="null" />.</returns>
		// Token: 0x170012F4 RID: 4852
		// (get) Token: 0x06004DF4 RID: 19956 RVA: 0x0015FB78 File Offset: 0x0015DD78
		// (set) Token: 0x06004DF5 RID: 19957 RVA: 0x0015FB80 File Offset: 0x0015DD80
		[DefaultValue(null)]
		public DataTemplate HeaderTemplate
		{
			get
			{
				return this._headerTemplate;
			}
			set
			{
				this._headerTemplate = value;
				this.OnPropertyChanged("HeaderTemplate");
			}
		}

		/// <summary>Enables the application writer to provide custom selection logic for a template that is used to display the group header.</summary>
		/// <returns>An object that derives from <see cref="T:System.Windows.Controls.DataTemplateSelector" />. The default is <see langword="null" />.</returns>
		// Token: 0x170012F5 RID: 4853
		// (get) Token: 0x06004DF6 RID: 19958 RVA: 0x0015FB94 File Offset: 0x0015DD94
		// (set) Token: 0x06004DF7 RID: 19959 RVA: 0x0015FB9C File Offset: 0x0015DD9C
		[DefaultValue(null)]
		public DataTemplateSelector HeaderTemplateSelector
		{
			get
			{
				return this._headerTemplateSelector;
			}
			set
			{
				this._headerTemplateSelector = value;
				this.OnPropertyChanged("HeaderTemplateSelector");
			}
		}

		/// <summary>Gets or sets a composite string that specifies how to format the header if it is displayed as a string.</summary>
		/// <returns>A composite string that specifies how to format the header if it is displayed as a string.</returns>
		// Token: 0x170012F6 RID: 4854
		// (get) Token: 0x06004DF8 RID: 19960 RVA: 0x0015FBB0 File Offset: 0x0015DDB0
		// (set) Token: 0x06004DF9 RID: 19961 RVA: 0x0015FBB8 File Offset: 0x0015DDB8
		[DefaultValue(null)]
		public string HeaderStringFormat
		{
			get
			{
				return this._headerStringFormat;
			}
			set
			{
				this._headerStringFormat = value;
				this.OnPropertyChanged("HeaderStringFormat");
			}
		}

		/// <summary>Gets or sets a value that indicates whether items corresponding to empty groups should be displayed.</summary>
		/// <returns>
		///     <see langword="true" /> to not display empty groups; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
		// Token: 0x170012F7 RID: 4855
		// (get) Token: 0x06004DFA RID: 19962 RVA: 0x0015FBCC File Offset: 0x0015DDCC
		// (set) Token: 0x06004DFB RID: 19963 RVA: 0x0015FBD4 File Offset: 0x0015DDD4
		[DefaultValue(false)]
		public bool HidesIfEmpty
		{
			get
			{
				return this._hidesIfEmpty;
			}
			set
			{
				this._hidesIfEmpty = value;
				this.OnPropertyChanged("HidesIfEmpty");
			}
		}

		/// <summary>Gets or sets the number of alternating <see cref="T:System.Windows.Controls.GroupItem" /> objects.</summary>
		/// <returns>The number of alternating <see cref="T:System.Windows.Controls.GroupItem" /> objects.</returns>
		// Token: 0x170012F8 RID: 4856
		// (get) Token: 0x06004DFC RID: 19964 RVA: 0x0015FBE8 File Offset: 0x0015DDE8
		// (set) Token: 0x06004DFD RID: 19965 RVA: 0x0015FBF0 File Offset: 0x0015DDF0
		[DefaultValue(0)]
		public int AlternationCount
		{
			get
			{
				return this._alternationCount;
			}
			set
			{
				this._alternationCount = value;
				this._isAlternationCountSet = true;
				this.OnPropertyChanged("AlternationCount");
			}
		}

		/// <summary>Gets the default style of the group.</summary>
		/// <returns>The default style of the group.</returns>
		// Token: 0x170012F9 RID: 4857
		// (get) Token: 0x06004DFE RID: 19966 RVA: 0x0015FC0B File Offset: 0x0015DE0B
		public static GroupStyle Default
		{
			get
			{
				return GroupStyle.s_DefaultGroupStyle;
			}
		}

		// Token: 0x06004DFF RID: 19967 RVA: 0x0015FC12 File Offset: 0x0015DE12
		private void OnPropertyChanged(string propertyName)
		{
			this.OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
		}

		// Token: 0x170012FA RID: 4858
		// (get) Token: 0x06004E00 RID: 19968 RVA: 0x0015FC20 File Offset: 0x0015DE20
		internal bool IsAlternationCountSet
		{
			get
			{
				return this._isAlternationCountSet;
			}
		}

		/// <summary>Identifies the default <see cref="T:System.Windows.Controls.ItemsPanelTemplate" /> that creates the panel used to layout the items.</summary>
		// Token: 0x04002B9F RID: 11167
		public static readonly ItemsPanelTemplate DefaultGroupPanel;

		// Token: 0x04002BA0 RID: 11168
		private ItemsPanelTemplate _panel;

		// Token: 0x04002BA1 RID: 11169
		private Style _containerStyle;

		// Token: 0x04002BA2 RID: 11170
		private StyleSelector _containerStyleSelector;

		// Token: 0x04002BA3 RID: 11171
		private DataTemplate _headerTemplate;

		// Token: 0x04002BA4 RID: 11172
		private DataTemplateSelector _headerTemplateSelector;

		// Token: 0x04002BA5 RID: 11173
		private string _headerStringFormat;

		// Token: 0x04002BA6 RID: 11174
		private bool _hidesIfEmpty;

		// Token: 0x04002BA7 RID: 11175
		private bool _isAlternationCountSet;

		// Token: 0x04002BA8 RID: 11176
		private int _alternationCount;

		// Token: 0x04002BA9 RID: 11177
		private static GroupStyle s_DefaultGroupStyle;

		// Token: 0x04002BAA RID: 11178
		internal static ItemsPanelTemplate DefaultStackPanel;

		// Token: 0x04002BAB RID: 11179
		internal static ItemsPanelTemplate DefaultVirtualizingStackPanel;
	}
}
