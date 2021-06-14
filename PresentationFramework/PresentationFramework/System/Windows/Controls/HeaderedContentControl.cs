using System;
using System.Collections;
using System.ComponentModel;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Threading;
using MS.Internal;
using MS.Internal.Controls;
using MS.Internal.KnownBoxes;
using MS.Internal.PresentationFramework;

namespace System.Windows.Controls
{
	/// <summary>Provides the base implementation for all controls that contain single content and have a header.</summary>
	// Token: 0x020004E5 RID: 1253
	[Localizability(LocalizationCategory.Text)]
	public class HeaderedContentControl : ContentControl
	{
		// Token: 0x06004E2E RID: 20014 RVA: 0x00160380 File Offset: 0x0015E580
		static HeaderedContentControl()
		{
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(HeaderedContentControl), new FrameworkPropertyMetadata(typeof(HeaderedContentControl)));
			HeaderedContentControl._dType = DependencyObjectType.FromSystemTypeInternal(typeof(HeaderedContentControl));
		}

		/// <summary>Gets or sets the data used for the header of each control.  </summary>
		/// <returns>A header object. The default is <see langword="null" />.</returns>
		// Token: 0x1700130A RID: 4874
		// (get) Token: 0x06004E30 RID: 20016 RVA: 0x001604D4 File Offset: 0x0015E6D4
		// (set) Token: 0x06004E31 RID: 20017 RVA: 0x001604E1 File Offset: 0x0015E6E1
		[Bindable(true)]
		[Category("Content")]
		[Localizability(LocalizationCategory.Label)]
		public object Header
		{
			get
			{
				return base.GetValue(HeaderedContentControl.HeaderProperty);
			}
			set
			{
				base.SetValue(HeaderedContentControl.HeaderProperty, value);
			}
		}

		// Token: 0x06004E32 RID: 20018 RVA: 0x001604F0 File Offset: 0x0015E6F0
		private static void OnHeaderChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			HeaderedContentControl headeredContentControl = (HeaderedContentControl)d;
			headeredContentControl.SetValue(HeaderedContentControl.HasHeaderPropertyKey, (e.NewValue != null) ? BooleanBoxes.TrueBox : BooleanBoxes.FalseBox);
			headeredContentControl.OnHeaderChanged(e.OldValue, e.NewValue);
		}

		/// <summary>Called when the <see cref="P:System.Windows.Controls.HeaderedContentControl.Header" /> property of a <see cref="T:System.Windows.Controls.HeaderedContentControl" /> changes. </summary>
		/// <param name="oldHeader">Old value of the <see cref="P:System.Windows.Controls.HeaderedContentControl.Header" /> property.</param>
		/// <param name="newHeader">New value of the <see cref="P:System.Windows.Controls.HeaderedContentControl.Header" /> property.</param>
		// Token: 0x06004E33 RID: 20019 RVA: 0x00160538 File Offset: 0x0015E738
		protected virtual void OnHeaderChanged(object oldHeader, object newHeader)
		{
			base.RemoveLogicalChild(oldHeader);
			base.AddLogicalChild(newHeader);
		}

		/// <summary>Gets a value that indicates whether the header is <see langword="null" />.  </summary>
		/// <returns>
		///     <see langword="true" /> if the <see cref="P:System.Windows.Controls.HeaderedContentControl.Header" /> property is not <see langword="null" />; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
		// Token: 0x1700130B RID: 4875
		// (get) Token: 0x06004E34 RID: 20020 RVA: 0x00160548 File Offset: 0x0015E748
		[Bindable(false)]
		[Browsable(false)]
		public bool HasHeader
		{
			get
			{
				return (bool)base.GetValue(HeaderedContentControl.HasHeaderProperty);
			}
		}

		/// <summary>Gets or sets the template used to display the content of the control's header.  </summary>
		/// <returns>A data template. The default is <see langword="null" />.</returns>
		// Token: 0x1700130C RID: 4876
		// (get) Token: 0x06004E35 RID: 20021 RVA: 0x0016055A File Offset: 0x0015E75A
		// (set) Token: 0x06004E36 RID: 20022 RVA: 0x0016056C File Offset: 0x0015E76C
		[Bindable(true)]
		[Category("Content")]
		public DataTemplate HeaderTemplate
		{
			get
			{
				return (DataTemplate)base.GetValue(HeaderedContentControl.HeaderTemplateProperty);
			}
			set
			{
				base.SetValue(HeaderedContentControl.HeaderTemplateProperty, value);
			}
		}

		// Token: 0x06004E37 RID: 20023 RVA: 0x0016057C File Offset: 0x0015E77C
		private static void OnHeaderTemplateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			HeaderedContentControl headeredContentControl = (HeaderedContentControl)d;
			headeredContentControl.OnHeaderTemplateChanged((DataTemplate)e.OldValue, (DataTemplate)e.NewValue);
		}

		/// <summary>Called when the <see cref="P:System.Windows.Controls.HeaderedContentControl.HeaderTemplate" /> property changes. </summary>
		/// <param name="oldHeaderTemplate">Old value of the <see cref="P:System.Windows.Controls.HeaderedContentControl.HeaderTemplate" /> property.</param>
		/// <param name="newHeaderTemplate">New value of the <see cref="P:System.Windows.Controls.HeaderedContentControl.HeaderTemplate" /> property.</param>
		// Token: 0x06004E38 RID: 20024 RVA: 0x001605AE File Offset: 0x0015E7AE
		protected virtual void OnHeaderTemplateChanged(DataTemplate oldHeaderTemplate, DataTemplate newHeaderTemplate)
		{
			Helper.CheckTemplateAndTemplateSelector("Header", HeaderedContentControl.HeaderTemplateProperty, HeaderedContentControl.HeaderTemplateSelectorProperty, this);
		}

		/// <summary>Gets or sets a data template selector that provides custom logic for choosing the template used to display the header.  </summary>
		/// <returns>A data template selector object. The default is <see langword="null" />.</returns>
		// Token: 0x1700130D RID: 4877
		// (get) Token: 0x06004E39 RID: 20025 RVA: 0x001605C5 File Offset: 0x0015E7C5
		// (set) Token: 0x06004E3A RID: 20026 RVA: 0x001605D7 File Offset: 0x0015E7D7
		[Bindable(true)]
		[Category("Content")]
		public DataTemplateSelector HeaderTemplateSelector
		{
			get
			{
				return (DataTemplateSelector)base.GetValue(HeaderedContentControl.HeaderTemplateSelectorProperty);
			}
			set
			{
				base.SetValue(HeaderedContentControl.HeaderTemplateSelectorProperty, value);
			}
		}

		// Token: 0x06004E3B RID: 20027 RVA: 0x001605E8 File Offset: 0x0015E7E8
		private static void OnHeaderTemplateSelectorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			HeaderedContentControl headeredContentControl = (HeaderedContentControl)d;
			headeredContentControl.OnHeaderTemplateSelectorChanged((DataTemplateSelector)e.OldValue, (DataTemplateSelector)e.NewValue);
		}

		/// <summary>Called when the <see cref="P:System.Windows.Controls.HeaderedContentControl.HeaderTemplateSelector" /> property changes. </summary>
		/// <param name="oldHeaderTemplateSelector">Old value of the <see cref="P:System.Windows.Controls.HeaderedContentControl.HeaderTemplateSelector" /> property.</param>
		/// <param name="newHeaderTemplateSelector">New value of the <see cref="P:System.Windows.Controls.HeaderedContentControl.HeaderTemplateSelector" /> property.</param>
		// Token: 0x06004E3C RID: 20028 RVA: 0x001605AE File Offset: 0x0015E7AE
		protected virtual void OnHeaderTemplateSelectorChanged(DataTemplateSelector oldHeaderTemplateSelector, DataTemplateSelector newHeaderTemplateSelector)
		{
			Helper.CheckTemplateAndTemplateSelector("Header", HeaderedContentControl.HeaderTemplateProperty, HeaderedContentControl.HeaderTemplateSelectorProperty, this);
		}

		/// <summary>Gets or sets a composite string that specifies how to format the <see cref="P:System.Windows.Controls.HeaderedContentControl.Header" /> property if it is displayed as a string.</summary>
		/// <returns>A composite string that specifies how to format the <see cref="P:System.Windows.Controls.HeaderedContentControl.Header" /> property if it is displayed as a string. The default is <see langword="null" />.</returns>
		// Token: 0x1700130E RID: 4878
		// (get) Token: 0x06004E3D RID: 20029 RVA: 0x0016061A File Offset: 0x0015E81A
		// (set) Token: 0x06004E3E RID: 20030 RVA: 0x0016062C File Offset: 0x0015E82C
		[Bindable(true)]
		[CustomCategory("Content")]
		public string HeaderStringFormat
		{
			get
			{
				return (string)base.GetValue(HeaderedContentControl.HeaderStringFormatProperty);
			}
			set
			{
				base.SetValue(HeaderedContentControl.HeaderStringFormatProperty, value);
			}
		}

		// Token: 0x06004E3F RID: 20031 RVA: 0x0016063C File Offset: 0x0015E83C
		private static void OnHeaderStringFormatChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			HeaderedContentControl headeredContentControl = (HeaderedContentControl)d;
			headeredContentControl.OnHeaderStringFormatChanged((string)e.OldValue, (string)e.NewValue);
		}

		/// <summary>Called when the <see cref="P:System.Windows.Controls.HeaderedContentControl.HeaderStringFormat" /> property changes.</summary>
		/// <param name="oldHeaderStringFormat">The old value of the <see cref="P:System.Windows.Controls.HeaderedContentControl.HeaderStringFormat" /> property.</param>
		/// <param name="newHeaderStringFormat">The new value of the <see cref="P:System.Windows.Controls.HeaderedContentControl.HeaderStringFormat" /> property.</param>
		// Token: 0x06004E40 RID: 20032 RVA: 0x00002137 File Offset: 0x00000337
		protected virtual void OnHeaderStringFormatChanged(string oldHeaderStringFormat, string newHeaderStringFormat)
		{
		}

		/// <summary>Gets an enumerator to the logical child elements of the <see cref="T:System.Windows.Controls.ControlTemplate" />. </summary>
		/// <returns>An enumerator. </returns>
		// Token: 0x1700130F RID: 4879
		// (get) Token: 0x06004E41 RID: 20033 RVA: 0x00160670 File Offset: 0x0015E870
		protected internal override IEnumerator LogicalChildren
		{
			get
			{
				object header = this.Header;
				if (this.HeaderIsNotLogical || header == null)
				{
					return base.LogicalChildren;
				}
				return new HeaderedContentModelTreeEnumerator(this, base.ContentIsNotLogical ? null : base.Content, header);
			}
		}

		// Token: 0x06004E42 RID: 20034 RVA: 0x001606AE File Offset: 0x0015E8AE
		internal override string GetPlainText()
		{
			return ContentControl.ContentObjectToString(this.Header);
		}

		// Token: 0x17001310 RID: 4880
		// (get) Token: 0x06004E43 RID: 20035 RVA: 0x001606BB File Offset: 0x0015E8BB
		// (set) Token: 0x06004E44 RID: 20036 RVA: 0x001606C4 File Offset: 0x0015E8C4
		internal bool HeaderIsNotLogical
		{
			get
			{
				return base.ReadControlFlag(Control.ControlBoolFlags.HeaderIsNotLogical);
			}
			set
			{
				base.WriteControlFlag(Control.ControlBoolFlags.HeaderIsNotLogical, value);
			}
		}

		// Token: 0x17001311 RID: 4881
		// (get) Token: 0x06004E45 RID: 20037 RVA: 0x001606CE File Offset: 0x0015E8CE
		// (set) Token: 0x06004E46 RID: 20038 RVA: 0x001606D8 File Offset: 0x0015E8D8
		internal bool HeaderIsItem
		{
			get
			{
				return base.ReadControlFlag(Control.ControlBoolFlags.HeaderIsItem);
			}
			set
			{
				base.WriteControlFlag(Control.ControlBoolFlags.HeaderIsItem, value);
			}
		}

		// Token: 0x06004E47 RID: 20039 RVA: 0x001606E4 File Offset: 0x0015E8E4
		internal void PrepareHeaderedContentControl(object item, DataTemplate itemTemplate, DataTemplateSelector itemTemplateSelector, string stringFormat)
		{
			if (item != this)
			{
				base.ContentIsNotLogical = true;
				this.HeaderIsNotLogical = true;
				if (base.ContentIsItem || !base.HasNonDefaultValue(ContentControl.ContentProperty))
				{
					base.Content = item;
					base.ContentIsItem = true;
				}
				if (!(item is Visual) && (this.HeaderIsItem || !base.HasNonDefaultValue(HeaderedContentControl.HeaderProperty)))
				{
					this.Header = item;
					this.HeaderIsItem = true;
				}
				if (itemTemplate != null)
				{
					base.SetValue(HeaderedContentControl.HeaderTemplateProperty, itemTemplate);
				}
				if (itemTemplateSelector != null)
				{
					base.SetValue(HeaderedContentControl.HeaderTemplateSelectorProperty, itemTemplateSelector);
				}
				if (stringFormat != null)
				{
					base.SetValue(HeaderedContentControl.HeaderStringFormatProperty, stringFormat);
					return;
				}
			}
			else
			{
				base.ContentIsNotLogical = false;
			}
		}

		// Token: 0x06004E48 RID: 20040 RVA: 0x0016078B File Offset: 0x0015E98B
		internal void ClearHeaderedContentControl(object item)
		{
			if (item != this)
			{
				if (base.ContentIsItem)
				{
					base.Content = BindingExpressionBase.DisconnectedItem;
				}
				if (this.HeaderIsItem)
				{
					this.Header = BindingExpressionBase.DisconnectedItem;
				}
			}
		}

		/// <summary>Provides a string representation of a <see cref="T:System.Windows.Controls.HeaderedContentControl" />. </summary>
		/// <returns>A string representation of the object.</returns>
		// Token: 0x06004E49 RID: 20041 RVA: 0x001607B8 File Offset: 0x0015E9B8
		public override string ToString()
		{
			string text = base.GetType().ToString();
			string headerText = string.Empty;
			string contentText = string.Empty;
			bool valuesDefined = false;
			if (base.CheckAccess())
			{
				headerText = ContentControl.ContentObjectToString(this.Header);
				contentText = ContentControl.ContentObjectToString(base.Content);
				valuesDefined = true;
			}
			else
			{
				base.Dispatcher.Invoke(DispatcherPriority.Send, new TimeSpan(0, 0, 0, 0, 20), new DispatcherOperationCallback(delegate(object o)
				{
					headerText = ContentControl.ContentObjectToString(this.Header);
					contentText = ContentControl.ContentObjectToString(this.Content);
					valuesDefined = true;
					return null;
				}), null);
			}
			if (valuesDefined)
			{
				return SR.Get("ToStringFormatString_HeaderedContentControl", new object[]
				{
					text,
					headerText,
					contentText
				});
			}
			return text;
		}

		// Token: 0x17001312 RID: 4882
		// (get) Token: 0x06004E4A RID: 20042 RVA: 0x00160884 File Offset: 0x0015EA84
		internal override DependencyObjectType DTypeThemeStyleKey
		{
			get
			{
				return HeaderedContentControl._dType;
			}
		}

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.HeaderedContentControl.Header" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.HeaderedContentControl.Header" /> dependency property.</returns>
		// Token: 0x04002BB8 RID: 11192
		[CommonDependencyProperty]
		public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register("Header", typeof(object), typeof(HeaderedContentControl), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(HeaderedContentControl.OnHeaderChanged)));

		// Token: 0x04002BB9 RID: 11193
		internal static readonly DependencyPropertyKey HasHeaderPropertyKey = DependencyProperty.RegisterReadOnly("HasHeader", typeof(bool), typeof(HeaderedContentControl), new FrameworkPropertyMetadata(BooleanBoxes.FalseBox));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.HeaderedContentControl.HasHeader" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.HeaderedContentControl.HasHeader" /> dependency property.</returns>
		// Token: 0x04002BBA RID: 11194
		[CommonDependencyProperty]
		public static readonly DependencyProperty HasHeaderProperty = HeaderedContentControl.HasHeaderPropertyKey.DependencyProperty;

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.HeaderedContentControl.HeaderTemplate" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.HeaderedContentControl.HeaderTemplate" /> dependency property.</returns>
		// Token: 0x04002BBB RID: 11195
		[CommonDependencyProperty]
		public static readonly DependencyProperty HeaderTemplateProperty = DependencyProperty.Register("HeaderTemplate", typeof(DataTemplate), typeof(HeaderedContentControl), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(HeaderedContentControl.OnHeaderTemplateChanged)));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.HeaderedContentControl.HeaderTemplateSelector" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.HeaderedContentControl.HeaderTemplateSelector" /> dependency property.</returns>
		// Token: 0x04002BBC RID: 11196
		[CommonDependencyProperty]
		public static readonly DependencyProperty HeaderTemplateSelectorProperty = DependencyProperty.Register("HeaderTemplateSelector", typeof(DataTemplateSelector), typeof(HeaderedContentControl), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(HeaderedContentControl.OnHeaderTemplateSelectorChanged)));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.HeaderedContentControl.HeaderStringFormat" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.HeaderedContentControl.HeaderStringFormat" /> dependency property.</returns>
		// Token: 0x04002BBD RID: 11197
		[CommonDependencyProperty]
		public static readonly DependencyProperty HeaderStringFormatProperty = DependencyProperty.Register("HeaderStringFormat", typeof(string), typeof(HeaderedContentControl), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(HeaderedContentControl.OnHeaderStringFormatChanged)));

		// Token: 0x04002BBE RID: 11198
		private static DependencyObjectType _dType;
	}
}
