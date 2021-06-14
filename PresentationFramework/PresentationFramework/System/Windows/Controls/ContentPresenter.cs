using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Markup;
using System.Windows.Media;
using MS.Internal;
using MS.Internal.KnownBoxes;
using MS.Internal.PresentationFramework;
using MS.Utility;

namespace System.Windows.Controls
{
	/// <summary>Displays the content of a <see cref="T:System.Windows.Controls.ContentControl" />.</summary>
	// Token: 0x02000489 RID: 1161
	[Localizability(LocalizationCategory.None, Readability = Readability.Unreadable)]
	public class ContentPresenter : FrameworkElement
	{
		// Token: 0x060043F4 RID: 17396 RVA: 0x00136218 File Offset: 0x00134418
		static ContentPresenter()
		{
			DataTemplate dataTemplate = new DataTemplate();
			FrameworkElementFactory frameworkElementFactory = ContentPresenter.CreateAccessTextFactory();
			frameworkElementFactory.SetValue(AccessText.TextProperty, new TemplateBindingExtension(ContentPresenter.ContentProperty));
			dataTemplate.VisualTree = frameworkElementFactory;
			dataTemplate.Seal();
			ContentPresenter.s_AccessTextTemplate = dataTemplate;
			dataTemplate = new DataTemplate();
			frameworkElementFactory = ContentPresenter.CreateTextBlockFactory();
			frameworkElementFactory.SetValue(TextBlock.TextProperty, new TemplateBindingExtension(ContentPresenter.ContentProperty));
			dataTemplate.VisualTree = frameworkElementFactory;
			dataTemplate.Seal();
			ContentPresenter.s_StringTemplate = dataTemplate;
			dataTemplate = new DataTemplate();
			frameworkElementFactory = ContentPresenter.CreateTextBlockFactory();
			Binding binding = new Binding();
			binding.XPath = ".";
			frameworkElementFactory.SetBinding(TextBlock.TextProperty, binding);
			dataTemplate.VisualTree = frameworkElementFactory;
			dataTemplate.Seal();
			ContentPresenter.s_XmlNodeTemplate = dataTemplate;
			dataTemplate = new ContentPresenter.UseContentTemplate();
			dataTemplate.Seal();
			ContentPresenter.s_UIElementTemplate = dataTemplate;
			dataTemplate = new ContentPresenter.DefaultTemplate();
			dataTemplate.Seal();
			ContentPresenter.s_DefaultTemplate = dataTemplate;
			ContentPresenter.s_DefaultTemplateSelector = new ContentPresenter.DefaultSelector();
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Controls.ContentPresenter" /> class.</summary>
		// Token: 0x060043F5 RID: 17397 RVA: 0x0013645E File Offset: 0x0013465E
		public ContentPresenter()
		{
			this.Initialize();
		}

		// Token: 0x060043F6 RID: 17398 RVA: 0x0013646C File Offset: 0x0013466C
		private void Initialize()
		{
			PropertyMetadata metadata = ContentPresenter.TemplateProperty.GetMetadata(base.DependencyObjectType);
			DataTemplate dataTemplate = (DataTemplate)metadata.DefaultValue;
			if (dataTemplate != null)
			{
				ContentPresenter.OnTemplateChanged(this, new DependencyPropertyChangedEventArgs(ContentPresenter.TemplateProperty, metadata, null, dataTemplate));
			}
			base.DataContext = null;
		}

		/// <summary>Gets or sets a value that indicates whether the <see cref="T:System.Windows.Controls.ContentPresenter" /> should use <see cref="T:System.Windows.Controls.AccessText" /> in its style.   </summary>
		/// <returns>
		///     <see langword="true" /> if the <see cref="T:System.Windows.Controls.ContentPresenter" /> should use <see cref="T:System.Windows.Controls.AccessText" /> in its style; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
		// Token: 0x170010AC RID: 4268
		// (get) Token: 0x060043F7 RID: 17399 RVA: 0x001364B3 File Offset: 0x001346B3
		// (set) Token: 0x060043F8 RID: 17400 RVA: 0x001364C5 File Offset: 0x001346C5
		public bool RecognizesAccessKey
		{
			get
			{
				return (bool)base.GetValue(ContentPresenter.RecognizesAccessKeyProperty);
			}
			set
			{
				base.SetValue(ContentPresenter.RecognizesAccessKeyProperty, BooleanBoxes.Box(value));
			}
		}

		/// <summary>Gets or sets the data used to generate the child elements of a <see cref="T:System.Windows.Controls.ContentPresenter" />.  </summary>
		/// <returns>The data used to generate the child elements. The default is <see langword="null" />.</returns>
		// Token: 0x170010AD RID: 4269
		// (get) Token: 0x060043F9 RID: 17401 RVA: 0x0013600C File Offset: 0x0013420C
		// (set) Token: 0x060043FA RID: 17402 RVA: 0x00136019 File Offset: 0x00134219
		public object Content
		{
			get
			{
				return base.GetValue(ContentControl.ContentProperty);
			}
			set
			{
				base.SetValue(ContentControl.ContentProperty, value);
			}
		}

		// Token: 0x060043FB RID: 17403 RVA: 0x001364D8 File Offset: 0x001346D8
		private static void OnContentChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			ContentPresenter contentPresenter = (ContentPresenter)d;
			if (!contentPresenter._templateIsCurrent)
			{
				return;
			}
			bool flag;
			if (e.NewValue == BindingExpressionBase.DisconnectedItem)
			{
				flag = false;
			}
			else if (contentPresenter.ContentTemplate != null)
			{
				flag = false;
			}
			else if (contentPresenter.ContentTemplateSelector != null)
			{
				flag = true;
			}
			else if (contentPresenter.Template == ContentPresenter.UIElementContentTemplate)
			{
				flag = true;
				contentPresenter.Template = null;
			}
			else if (contentPresenter.Template == ContentPresenter.DefaultContentTemplate)
			{
				flag = true;
			}
			else
			{
				Type type;
				object obj = ContentPresenter.DataTypeForItem(e.OldValue, contentPresenter, out type);
				object obj2 = ContentPresenter.DataTypeForItem(e.NewValue, contentPresenter, out type);
				flag = (obj != obj2);
				if (!flag && contentPresenter.RecognizesAccessKey && typeof(string) == obj2 && contentPresenter.IsUsingDefaultStringTemplate)
				{
					string text = (string)e.OldValue;
					string text2 = (string)e.NewValue;
					bool flag2 = text.IndexOf(AccessText.AccessKeyMarker) > -1;
					bool flag3 = text2.IndexOf(AccessText.AccessKeyMarker) > -1;
					if (flag2 != flag3)
					{
						flag = true;
					}
				}
			}
			if (flag)
			{
				contentPresenter._templateIsCurrent = false;
			}
			if (contentPresenter._templateIsCurrent && contentPresenter.Template != ContentPresenter.UIElementContentTemplate)
			{
				contentPresenter.DataContext = e.NewValue;
			}
		}

		/// <summary>Gets or sets the template used to display the content of the control.   </summary>
		/// <returns>A <see cref="T:System.Windows.DataTemplate" /> that defines the visualization of the content. The default is <see langword="null" />.</returns>
		// Token: 0x170010AE RID: 4270
		// (get) Token: 0x060043FC RID: 17404 RVA: 0x001360D3 File Offset: 0x001342D3
		// (set) Token: 0x060043FD RID: 17405 RVA: 0x001360E5 File Offset: 0x001342E5
		public DataTemplate ContentTemplate
		{
			get
			{
				return (DataTemplate)base.GetValue(ContentControl.ContentTemplateProperty);
			}
			set
			{
				base.SetValue(ContentControl.ContentTemplateProperty, value);
			}
		}

		// Token: 0x060043FE RID: 17406 RVA: 0x00136614 File Offset: 0x00134814
		private static void OnContentTemplateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			ContentPresenter contentPresenter = (ContentPresenter)d;
			contentPresenter._templateIsCurrent = false;
			contentPresenter.OnContentTemplateChanged((DataTemplate)e.OldValue, (DataTemplate)e.NewValue);
		}

		/// <summary>Invoked when the <see cref="P:System.Windows.Controls.ContentPresenter.ContentTemplate" /> changes. </summary>
		/// <param name="oldContentTemplate">The old value of the <see cref="P:System.Windows.Controls.ContentPresenter.ContentTemplate" /> property.</param>
		/// <param name="newContentTemplate">The new value of the <see cref="P:System.Windows.Controls.ContentPresenter.ContentTemplate" /> property.</param>
		// Token: 0x060043FF RID: 17407 RVA: 0x0013664D File Offset: 0x0013484D
		protected virtual void OnContentTemplateChanged(DataTemplate oldContentTemplate, DataTemplate newContentTemplate)
		{
			Helper.CheckTemplateAndTemplateSelector("Content", ContentPresenter.ContentTemplateProperty, ContentPresenter.ContentTemplateSelectorProperty, this);
			this.Template = null;
		}

		/// <summary>Gets or sets the <see cref="T:System.Windows.Controls.DataTemplateSelector" />, which allows the application writer to provide custom logic for choosing the template that is used to display the content of the control.  </summary>
		/// <returns>A <see cref="T:System.Windows.Controls.DataTemplateSelector" /> object that supplies logic to return a <see cref="T:System.Windows.DataTemplate" /> to apply. The default is <see langword="null" />.</returns>
		// Token: 0x170010AF RID: 4271
		// (get) Token: 0x06004400 RID: 17408 RVA: 0x0013613D File Offset: 0x0013433D
		// (set) Token: 0x06004401 RID: 17409 RVA: 0x0013614F File Offset: 0x0013434F
		public DataTemplateSelector ContentTemplateSelector
		{
			get
			{
				return (DataTemplateSelector)base.GetValue(ContentControl.ContentTemplateSelectorProperty);
			}
			set
			{
				base.SetValue(ContentControl.ContentTemplateSelectorProperty, value);
			}
		}

		/// <summary>Returns a value that indicates whether serialization processes should serialize the effective value of the <see cref="P:System.Windows.Controls.ContentPresenter.ContentTemplateSelector" /> property on instances of this class.</summary>
		/// <returns>
		///     <see langword="true" /> if the <see cref="P:System.Windows.Controls.ContentPresenter.ContentTemplateSelector" /> property value should be serialized; otherwise, <see langword="false" />.</returns>
		// Token: 0x06004402 RID: 17410 RVA: 0x0000B02A File Offset: 0x0000922A
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool ShouldSerializeContentTemplateSelector()
		{
			return false;
		}

		// Token: 0x06004403 RID: 17411 RVA: 0x0013666C File Offset: 0x0013486C
		private static void OnContentTemplateSelectorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			ContentPresenter contentPresenter = (ContentPresenter)d;
			contentPresenter._templateIsCurrent = false;
			contentPresenter.OnContentTemplateSelectorChanged((DataTemplateSelector)e.OldValue, (DataTemplateSelector)e.NewValue);
		}

		/// <summary>Invoked when the <see cref="P:System.Windows.Controls.ContentPresenter.ContentTemplateSelector" /> property changes. </summary>
		/// <param name="oldContentTemplateSelector">The old value of the <see cref="P:System.Windows.Controls.ContentPresenter.ContentTemplateSelector" /> property.</param>
		/// <param name="newContentTemplateSelector">The new value of the <see cref="P:System.Windows.Controls.ContentPresenter.ContentTemplateSelector" /> property.</param>
		// Token: 0x06004404 RID: 17412 RVA: 0x0013664D File Offset: 0x0013484D
		protected virtual void OnContentTemplateSelectorChanged(DataTemplateSelector oldContentTemplateSelector, DataTemplateSelector newContentTemplateSelector)
		{
			Helper.CheckTemplateAndTemplateSelector("Content", ContentPresenter.ContentTemplateProperty, ContentPresenter.ContentTemplateSelectorProperty, this);
			this.Template = null;
		}

		/// <summary>Gets or sets a composite string that specifies how to format the <see cref="P:System.Windows.Controls.ContentPresenter.Content" /> property if it is displayed as a string.</summary>
		/// <returns>A composite string that specifies how to format the <see cref="P:System.Windows.Controls.ContentPresenter.Content" /> property if it is displayed as a string. The default is <see langword="null" />.</returns>
		// Token: 0x170010B0 RID: 4272
		// (get) Token: 0x06004405 RID: 17413 RVA: 0x001366A5 File Offset: 0x001348A5
		// (set) Token: 0x06004406 RID: 17414 RVA: 0x001366B7 File Offset: 0x001348B7
		[Bindable(true)]
		[CustomCategory("Content")]
		public string ContentStringFormat
		{
			get
			{
				return (string)base.GetValue(ContentPresenter.ContentStringFormatProperty);
			}
			set
			{
				base.SetValue(ContentPresenter.ContentStringFormatProperty, value);
			}
		}

		// Token: 0x06004407 RID: 17415 RVA: 0x001366C8 File Offset: 0x001348C8
		private static void OnContentStringFormatChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			ContentPresenter contentPresenter = (ContentPresenter)d;
			contentPresenter.OnContentStringFormatChanged((string)e.OldValue, (string)e.NewValue);
		}

		/// <summary>Invoked when the <see cref="P:System.Windows.Controls.ContentPresenter.ContentStringFormat" /> property changes.</summary>
		/// <param name="oldContentStringFormat">The old value of the <see cref="P:System.Windows.Controls.ContentPresenter.ContentStringFormat" /> property.</param>
		/// <param name="newContentStringFormat">The new value of the <see cref="P:System.Windows.Controls.ContentPresenter.ContentStringFormat" /> property.</param>
		// Token: 0x06004408 RID: 17416 RVA: 0x001366FA File Offset: 0x001348FA
		protected virtual void OnContentStringFormatChanged(string oldContentStringFormat, string newContentStringFormat)
		{
			ContentPresenter.XMLFormattingTemplateField.ClearValue(this);
			ContentPresenter.StringFormattingTemplateField.ClearValue(this);
			ContentPresenter.AccessTextFormattingTemplateField.ClearValue(this);
		}

		/// <summary>Gets or sets the base name to use during automatic aliasing.  </summary>
		/// <returns>The base name to use during automatic aliasing. The default is "Content".</returns>
		// Token: 0x170010B1 RID: 4273
		// (get) Token: 0x06004409 RID: 17417 RVA: 0x0013671D File Offset: 0x0013491D
		// (set) Token: 0x0600440A RID: 17418 RVA: 0x0013672F File Offset: 0x0013492F
		public string ContentSource
		{
			get
			{
				return base.GetValue(ContentPresenter.ContentSourceProperty) as string;
			}
			set
			{
				base.SetValue(ContentPresenter.ContentSourceProperty, value);
			}
		}

		// Token: 0x0600440B RID: 17419 RVA: 0x00136740 File Offset: 0x00134940
		internal override void OnPreApplyTemplate()
		{
			base.OnPreApplyTemplate();
			if (base.TemplatedParent == null)
			{
				base.InvalidateProperty(ContentPresenter.ContentProperty);
			}
			if (this._language != null && this._language != base.Language)
			{
				this._templateIsCurrent = false;
			}
			if (!this._templateIsCurrent)
			{
				this.EnsureTemplate();
				this._templateIsCurrent = true;
			}
		}

		/// <summary>Determines the size of the <see cref="T:System.Windows.Controls.ContentPresenter" /> object based on the sizing properties, margin, and requested size of the child content.</summary>
		/// <param name="constraint">An upper limit value that the return value should not exceed.</param>
		/// <returns>The size that is required to arrange child content.</returns>
		// Token: 0x0600440C RID: 17420 RVA: 0x00136798 File Offset: 0x00134998
		protected override Size MeasureOverride(Size constraint)
		{
			return Helper.MeasureElementWithSingleChild(this, constraint);
		}

		/// <summary>Positions the single child element and determines the content of a <see cref="T:System.Windows.Controls.ContentPresenter" /> object. </summary>
		/// <param name="arrangeSize">The size that this <see cref="T:System.Windows.Controls.ContentPresenter" /> object should use to arrange its child element.</param>
		/// <returns>The actual size needed by the element.</returns>
		// Token: 0x0600440D RID: 17421 RVA: 0x001367A1 File Offset: 0x001349A1
		protected override Size ArrangeOverride(Size arrangeSize)
		{
			return Helper.ArrangeElementWithSingleChild(this, arrangeSize);
		}

		/// <summary>Returns the template to use. This may depend on the content or other properties.</summary>
		/// <returns>The <see cref="T:System.Windows.DataTemplate" /> to use.</returns>
		// Token: 0x0600440E RID: 17422 RVA: 0x001367AC File Offset: 0x001349AC
		protected virtual DataTemplate ChooseTemplate()
		{
			object content = this.Content;
			DataTemplate dataTemplate = this.ContentTemplate;
			if (dataTemplate == null && this.ContentTemplateSelector != null)
			{
				dataTemplate = this.ContentTemplateSelector.SelectTemplate(content, this);
			}
			if (dataTemplate == null)
			{
				dataTemplate = ContentPresenter.DefaultTemplateSelector.SelectTemplate(content, this);
			}
			return dataTemplate;
		}

		// Token: 0x170010B2 RID: 4274
		// (get) Token: 0x0600440F RID: 17423 RVA: 0x001367F3 File Offset: 0x001349F3
		internal static DataTemplate AccessTextContentTemplate
		{
			get
			{
				return ContentPresenter.s_AccessTextTemplate;
			}
		}

		// Token: 0x170010B3 RID: 4275
		// (get) Token: 0x06004410 RID: 17424 RVA: 0x001367FA File Offset: 0x001349FA
		internal static DataTemplate StringContentTemplate
		{
			get
			{
				return ContentPresenter.s_StringTemplate;
			}
		}

		// Token: 0x170010B4 RID: 4276
		// (get) Token: 0x06004411 RID: 17425 RVA: 0x00136801 File Offset: 0x00134A01
		internal override FrameworkTemplate TemplateInternal
		{
			get
			{
				return this.Template;
			}
		}

		// Token: 0x170010B5 RID: 4277
		// (get) Token: 0x06004412 RID: 17426 RVA: 0x00136809 File Offset: 0x00134A09
		// (set) Token: 0x06004413 RID: 17427 RVA: 0x00136811 File Offset: 0x00134A11
		internal override FrameworkTemplate TemplateCache
		{
			get
			{
				return this._templateCache;
			}
			set
			{
				this._templateCache = (DataTemplate)value;
			}
		}

		// Token: 0x170010B6 RID: 4278
		// (get) Token: 0x06004414 RID: 17428 RVA: 0x0013681F File Offset: 0x00134A1F
		internal bool TemplateIsCurrent
		{
			get
			{
				return this._templateIsCurrent;
			}
		}

		// Token: 0x06004415 RID: 17429 RVA: 0x00136828 File Offset: 0x00134A28
		internal void PrepareContentPresenter(object item, DataTemplate itemTemplate, DataTemplateSelector itemTemplateSelector, string stringFormat)
		{
			if (item != this)
			{
				if (this._contentIsItem || !base.HasNonDefaultValue(ContentPresenter.ContentProperty))
				{
					this.Content = item;
					this._contentIsItem = true;
				}
				if (itemTemplate != null)
				{
					base.SetValue(ContentPresenter.ContentTemplateProperty, itemTemplate);
				}
				if (itemTemplateSelector != null)
				{
					base.SetValue(ContentPresenter.ContentTemplateSelectorProperty, itemTemplateSelector);
				}
				if (stringFormat != null)
				{
					base.SetValue(ContentPresenter.ContentStringFormatProperty, stringFormat);
				}
			}
		}

		// Token: 0x06004416 RID: 17430 RVA: 0x0013688B File Offset: 0x00134A8B
		internal void ClearContentPresenter(object item)
		{
			if (item != this && this._contentIsItem)
			{
				this.Content = BindingExpressionBase.DisconnectedItem;
			}
		}

		// Token: 0x06004417 RID: 17431 RVA: 0x001368A4 File Offset: 0x00134AA4
		internal static object DataTypeForItem(object item, DependencyObject target, out Type type)
		{
			if (item == null)
			{
				type = null;
				return null;
			}
			type = ReflectionHelper.GetReflectionType(item);
			object result;
			if (SystemXmlLinqHelper.IsXElement(item))
			{
				result = SystemXmlLinqHelper.GetXElementTagName(item);
				type = null;
			}
			else if (SystemXmlHelper.IsXmlNode(item))
			{
				result = SystemXmlHelper.GetXmlTagName(item, target);
				type = null;
			}
			else if (type == typeof(object))
			{
				result = null;
			}
			else
			{
				result = type;
			}
			return result;
		}

		// Token: 0x06004418 RID: 17432 RVA: 0x00136905 File Offset: 0x00134B05
		internal void ReevaluateTemplate()
		{
			if (this.Template != this.ChooseTemplate())
			{
				this._templateIsCurrent = false;
				base.InvalidateMeasure();
			}
		}

		// Token: 0x170010B7 RID: 4279
		// (get) Token: 0x06004419 RID: 17433 RVA: 0x00136922 File Offset: 0x00134B22
		private static DataTemplate XmlNodeContentTemplate
		{
			get
			{
				return ContentPresenter.s_XmlNodeTemplate;
			}
		}

		// Token: 0x170010B8 RID: 4280
		// (get) Token: 0x0600441A RID: 17434 RVA: 0x00136929 File Offset: 0x00134B29
		private static DataTemplate UIElementContentTemplate
		{
			get
			{
				return ContentPresenter.s_UIElementTemplate;
			}
		}

		// Token: 0x170010B9 RID: 4281
		// (get) Token: 0x0600441B RID: 17435 RVA: 0x00136930 File Offset: 0x00134B30
		private static DataTemplate DefaultContentTemplate
		{
			get
			{
				return ContentPresenter.s_DefaultTemplate;
			}
		}

		// Token: 0x170010BA RID: 4282
		// (get) Token: 0x0600441C RID: 17436 RVA: 0x00136937 File Offset: 0x00134B37
		private static ContentPresenter.DefaultSelector DefaultTemplateSelector
		{
			get
			{
				return ContentPresenter.s_DefaultTemplateSelector;
			}
		}

		// Token: 0x170010BB RID: 4283
		// (get) Token: 0x0600441D RID: 17437 RVA: 0x00136940 File Offset: 0x00134B40
		private DataTemplate FormattingAccessTextContentTemplate
		{
			get
			{
				DataTemplate dataTemplate = ContentPresenter.AccessTextFormattingTemplateField.GetValue(this);
				if (dataTemplate == null)
				{
					Binding binding = new Binding();
					binding.StringFormat = this.ContentStringFormat;
					FrameworkElementFactory frameworkElementFactory = ContentPresenter.CreateAccessTextFactory();
					frameworkElementFactory.SetBinding(AccessText.TextProperty, binding);
					dataTemplate = new DataTemplate();
					dataTemplate.VisualTree = frameworkElementFactory;
					dataTemplate.Seal();
					ContentPresenter.AccessTextFormattingTemplateField.SetValue(this, dataTemplate);
				}
				return dataTemplate;
			}
		}

		// Token: 0x170010BC RID: 4284
		// (get) Token: 0x0600441E RID: 17438 RVA: 0x001369A0 File Offset: 0x00134BA0
		private DataTemplate FormattingStringContentTemplate
		{
			get
			{
				DataTemplate dataTemplate = ContentPresenter.StringFormattingTemplateField.GetValue(this);
				if (dataTemplate == null)
				{
					Binding binding = new Binding();
					binding.StringFormat = this.ContentStringFormat;
					FrameworkElementFactory frameworkElementFactory = ContentPresenter.CreateTextBlockFactory();
					frameworkElementFactory.SetBinding(TextBlock.TextProperty, binding);
					dataTemplate = new DataTemplate();
					dataTemplate.VisualTree = frameworkElementFactory;
					dataTemplate.Seal();
					ContentPresenter.StringFormattingTemplateField.SetValue(this, dataTemplate);
				}
				return dataTemplate;
			}
		}

		// Token: 0x170010BD RID: 4285
		// (get) Token: 0x0600441F RID: 17439 RVA: 0x00136A00 File Offset: 0x00134C00
		private DataTemplate FormattingXmlNodeContentTemplate
		{
			get
			{
				DataTemplate dataTemplate = ContentPresenter.XMLFormattingTemplateField.GetValue(this);
				if (dataTemplate == null)
				{
					Binding binding = new Binding();
					binding.XPath = ".";
					binding.StringFormat = this.ContentStringFormat;
					FrameworkElementFactory frameworkElementFactory = ContentPresenter.CreateTextBlockFactory();
					frameworkElementFactory.SetBinding(TextBlock.TextProperty, binding);
					dataTemplate = new DataTemplate();
					dataTemplate.VisualTree = frameworkElementFactory;
					dataTemplate.Seal();
					ContentPresenter.XMLFormattingTemplateField.SetValue(this, dataTemplate);
				}
				return dataTemplate;
			}
		}

		// Token: 0x170010BE RID: 4286
		// (get) Token: 0x06004420 RID: 17440 RVA: 0x00136809 File Offset: 0x00134A09
		// (set) Token: 0x06004421 RID: 17441 RVA: 0x00136A6B File Offset: 0x00134C6B
		private DataTemplate Template
		{
			get
			{
				return this._templateCache;
			}
			set
			{
				base.SetValue(ContentPresenter.TemplateProperty, value);
			}
		}

		// Token: 0x06004422 RID: 17442 RVA: 0x00136A79 File Offset: 0x00134C79
		internal override void OnTemplateChangedInternal(FrameworkTemplate oldTemplate, FrameworkTemplate newTemplate)
		{
			this.OnTemplateChanged((DataTemplate)oldTemplate, (DataTemplate)newTemplate);
		}

		// Token: 0x06004423 RID: 17443 RVA: 0x00136A90 File Offset: 0x00134C90
		private static void OnTemplateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			ContentPresenter fe = (ContentPresenter)d;
			StyleHelper.UpdateTemplateCache(fe, (FrameworkTemplate)e.OldValue, (FrameworkTemplate)e.NewValue, ContentPresenter.TemplateProperty);
		}

		/// <summary>Invoked when the <see cref="P:System.Windows.Controls.ContentPresenter.ContentTemplate" /> changes </summary>
		/// <param name="oldTemplate">The old <see cref="T:System.Windows.DataTemplate" /> object value.</param>
		/// <param name="newTemplate">The new <see cref="T:System.Windows.DataTemplate" /> object value.</param>
		// Token: 0x06004424 RID: 17444 RVA: 0x00002137 File Offset: 0x00000337
		protected virtual void OnTemplateChanged(DataTemplate oldTemplate, DataTemplate newTemplate)
		{
		}

		// Token: 0x06004425 RID: 17445 RVA: 0x00136AC8 File Offset: 0x00134CC8
		private void EnsureTemplate()
		{
			DataTemplate template = this.Template;
			DataTemplate dataTemplate = null;
			this._templateIsCurrent = false;
			while (!this._templateIsCurrent)
			{
				this._templateIsCurrent = true;
				dataTemplate = this.ChooseTemplate();
				if (template != dataTemplate)
				{
					this.Template = null;
				}
				if (dataTemplate != ContentPresenter.UIElementContentTemplate)
				{
					base.DataContext = this.Content;
				}
				else
				{
					base.ClearValue(FrameworkElement.DataContextProperty);
				}
			}
			this.Template = dataTemplate;
			if (template == dataTemplate)
			{
				StyleHelper.DoTemplateInvalidations(this, template);
			}
		}

		// Token: 0x06004426 RID: 17446 RVA: 0x00136B3C File Offset: 0x00134D3C
		private DataTemplate SelectTemplateForString(string s)
		{
			string contentStringFormat = this.ContentStringFormat;
			DataTemplate result;
			if (this.RecognizesAccessKey && s.IndexOf(AccessText.AccessKeyMarker) > -1)
			{
				result = (string.IsNullOrEmpty(contentStringFormat) ? ContentPresenter.AccessTextContentTemplate : this.FormattingAccessTextContentTemplate);
			}
			else
			{
				result = (string.IsNullOrEmpty(contentStringFormat) ? ContentPresenter.StringContentTemplate : this.FormattingStringContentTemplate);
			}
			return result;
		}

		// Token: 0x170010BF RID: 4287
		// (get) Token: 0x06004427 RID: 17447 RVA: 0x00136B98 File Offset: 0x00134D98
		private bool IsUsingDefaultStringTemplate
		{
			get
			{
				if (this.Template == ContentPresenter.StringContentTemplate || this.Template == ContentPresenter.AccessTextContentTemplate)
				{
					return true;
				}
				DataTemplate value = ContentPresenter.StringFormattingTemplateField.GetValue(this);
				if (value != null && value == this.Template)
				{
					return true;
				}
				value = ContentPresenter.AccessTextFormattingTemplateField.GetValue(this);
				return value != null && value == this.Template;
			}
		}

		// Token: 0x06004428 RID: 17448 RVA: 0x00136BF6 File Offset: 0x00134DF6
		private DataTemplate SelectTemplateForXML()
		{
			if (!string.IsNullOrEmpty(this.ContentStringFormat))
			{
				return this.FormattingXmlNodeContentTemplate;
			}
			return ContentPresenter.XmlNodeContentTemplate;
		}

		// Token: 0x06004429 RID: 17449 RVA: 0x00136C14 File Offset: 0x00134E14
		internal static FrameworkElementFactory CreateAccessTextFactory()
		{
			return new FrameworkElementFactory(typeof(AccessText));
		}

		// Token: 0x0600442A RID: 17450 RVA: 0x00136C34 File Offset: 0x00134E34
		internal static FrameworkElementFactory CreateTextBlockFactory()
		{
			return new FrameworkElementFactory(typeof(TextBlock));
		}

		// Token: 0x0600442B RID: 17451 RVA: 0x00136C54 File Offset: 0x00134E54
		private static TextBlock CreateTextBlock(ContentPresenter container)
		{
			return new TextBlock();
		}

		// Token: 0x0600442C RID: 17452 RVA: 0x00136C68 File Offset: 0x00134E68
		private void CacheLanguage(XmlLanguage language)
		{
			this._language = language;
		}

		// Token: 0x170010C0 RID: 4288
		// (get) Token: 0x0600442D RID: 17453 RVA: 0x000962DF File Offset: 0x000944DF
		internal override int EffectiveValuesInitialSize
		{
			get
			{
				return 28;
			}
		}

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.ContentPresenter.RecognizesAccessKey" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.ContentPresenter.RecognizesAccessKey" /> dependency property.</returns>
		// Token: 0x0400286D RID: 10349
		[CommonDependencyProperty]
		public static readonly DependencyProperty RecognizesAccessKeyProperty = DependencyProperty.Register("RecognizesAccessKey", typeof(bool), typeof(ContentPresenter), new FrameworkPropertyMetadata(BooleanBoxes.FalseBox));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.ContentPresenter.Content" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.ContentPresenter.Content" /> dependency property.</returns>
		// Token: 0x0400286E RID: 10350
		[CommonDependencyProperty]
		public static readonly DependencyProperty ContentProperty = ContentControl.ContentProperty.AddOwner(typeof(ContentPresenter), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsMeasure, new PropertyChangedCallback(ContentPresenter.OnContentChanged)));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.ContentPresenter.ContentTemplate" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.ContentPresenter.ContentTemplate" /> dependency property.</returns>
		// Token: 0x0400286F RID: 10351
		[CommonDependencyProperty]
		public static readonly DependencyProperty ContentTemplateProperty = ContentControl.ContentTemplateProperty.AddOwner(typeof(ContentPresenter), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsMeasure, new PropertyChangedCallback(ContentPresenter.OnContentTemplateChanged)));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.ContentPresenter.ContentTemplateSelector" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.ContentPresenter.ContentTemplateSelector" /> dependency property.</returns>
		// Token: 0x04002870 RID: 10352
		[CommonDependencyProperty]
		public static readonly DependencyProperty ContentTemplateSelectorProperty = ContentControl.ContentTemplateSelectorProperty.AddOwner(typeof(ContentPresenter), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsMeasure, new PropertyChangedCallback(ContentPresenter.OnContentTemplateSelectorChanged)));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.ContentPresenter.ContentStringFormat" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.ContentPresenter.ContentStringFormat" /> dependency property.</returns>
		// Token: 0x04002871 RID: 10353
		[CommonDependencyProperty]
		public static readonly DependencyProperty ContentStringFormatProperty = DependencyProperty.Register("ContentStringFormat", typeof(string), typeof(ContentPresenter), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(ContentPresenter.OnContentStringFormatChanged)));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.ContentPresenter.ContentSource" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.ContentPresenter.ContentSource" /> dependency property.</returns>
		// Token: 0x04002872 RID: 10354
		[CommonDependencyProperty]
		public static readonly DependencyProperty ContentSourceProperty = DependencyProperty.Register("ContentSource", typeof(string), typeof(ContentPresenter), new FrameworkPropertyMetadata("Content"));

		// Token: 0x04002873 RID: 10355
		internal static readonly DependencyProperty TemplateProperty = DependencyProperty.Register("Template", typeof(DataTemplate), typeof(ContentPresenter), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsMeasure, new PropertyChangedCallback(ContentPresenter.OnTemplateChanged)));

		// Token: 0x04002874 RID: 10356
		private DataTemplate _templateCache;

		// Token: 0x04002875 RID: 10357
		private bool _templateIsCurrent;

		// Token: 0x04002876 RID: 10358
		private bool _contentIsItem;

		// Token: 0x04002877 RID: 10359
		private XmlLanguage _language;

		// Token: 0x04002878 RID: 10360
		private static DataTemplate s_AccessTextTemplate;

		// Token: 0x04002879 RID: 10361
		private static DataTemplate s_StringTemplate;

		// Token: 0x0400287A RID: 10362
		private static DataTemplate s_XmlNodeTemplate;

		// Token: 0x0400287B RID: 10363
		private static DataTemplate s_UIElementTemplate;

		// Token: 0x0400287C RID: 10364
		private static DataTemplate s_DefaultTemplate;

		// Token: 0x0400287D RID: 10365
		private static ContentPresenter.DefaultSelector s_DefaultTemplateSelector;

		// Token: 0x0400287E RID: 10366
		private static readonly UncommonField<DataTemplate> XMLFormattingTemplateField = new UncommonField<DataTemplate>();

		// Token: 0x0400287F RID: 10367
		private static readonly UncommonField<DataTemplate> StringFormattingTemplateField = new UncommonField<DataTemplate>();

		// Token: 0x04002880 RID: 10368
		private static readonly UncommonField<DataTemplate> AccessTextFormattingTemplateField = new UncommonField<DataTemplate>();

		// Token: 0x02000960 RID: 2400
		private class UseContentTemplate : DataTemplate
		{
			// Token: 0x06008735 RID: 34613 RVA: 0x0024F333 File Offset: 0x0024D533
			public UseContentTemplate()
			{
				base.CanBuildVisualTree = true;
			}

			// Token: 0x06008736 RID: 34614 RVA: 0x0024F344 File Offset: 0x0024D544
			internal override bool BuildVisualTree(FrameworkElement container)
			{
				object content = ((ContentPresenter)container).Content;
				UIElement uielement = content as UIElement;
				if (uielement == null)
				{
					TypeConverter converter = TypeDescriptor.GetConverter(ReflectionHelper.GetReflectionType(content));
					uielement = (UIElement)converter.ConvertTo(content, typeof(UIElement));
				}
				StyleHelper.AddCustomTemplateRoot(container, uielement);
				return true;
			}
		}

		// Token: 0x02000961 RID: 2401
		private class DefaultTemplate : DataTemplate
		{
			// Token: 0x06008737 RID: 34615 RVA: 0x0024F333 File Offset: 0x0024D533
			public DefaultTemplate()
			{
				base.CanBuildVisualTree = true;
			}

			// Token: 0x06008738 RID: 34616 RVA: 0x0024F394 File Offset: 0x0024D594
			internal override bool BuildVisualTree(FrameworkElement container)
			{
				bool flag = EventTrace.IsEnabled(EventTrace.Keyword.KeywordGeneral, EventTrace.Level.Info);
				if (flag)
				{
					EventTrace.EventProvider.TraceEvent(EventTrace.Event.WClientStringBegin, EventTrace.Keyword.KeywordGeneral, EventTrace.Level.Info, "ContentPresenter.BuildVisualTree");
				}
				bool result;
				try
				{
					ContentPresenter contentPresenter = (ContentPresenter)container;
					Visual visual = this.DefaultExpansion(contentPresenter.Content, contentPresenter);
					result = (visual != null);
				}
				finally
				{
					if (flag)
					{
						EventTrace.EventProvider.TraceEvent(EventTrace.Event.WClientStringEnd, EventTrace.Keyword.KeywordGeneral, EventTrace.Level.Info, string.Format(CultureInfo.InvariantCulture, "ContentPresenter.BuildVisualTree for CP {0}", new object[]
						{
							container.GetHashCode()
						}));
					}
				}
				return result;
			}

			// Token: 0x06008739 RID: 34617 RVA: 0x0024F424 File Offset: 0x0024D624
			private UIElement DefaultExpansion(object content, ContentPresenter container)
			{
				if (content == null)
				{
					return null;
				}
				TextBlock textBlock = ContentPresenter.CreateTextBlock(container);
				textBlock.IsContentPresenterContainer = true;
				if (container != null)
				{
					StyleHelper.AddCustomTemplateRoot(container, textBlock, false, true);
				}
				this.DoDefaultExpansion(textBlock, content, container);
				return textBlock;
			}

			// Token: 0x0600873A RID: 34618 RVA: 0x0024F45C File Offset: 0x0024D65C
			private void DoDefaultExpansion(TextBlock textBlock, object content, ContentPresenter container)
			{
				Inline item;
				if ((item = (content as Inline)) != null)
				{
					textBlock.Inlines.Add(item);
					return;
				}
				bool flag = false;
				XmlLanguage language = container.Language;
				CultureInfo specificCulture = language.GetSpecificCulture();
				container.CacheLanguage(language);
				string text;
				if ((text = container.ContentStringFormat) != null)
				{
					try
					{
						text = Helper.GetEffectiveStringFormat(text);
						textBlock.Text = string.Format(specificCulture, text, new object[]
						{
							content
						});
						flag = true;
					}
					catch (FormatException)
					{
					}
				}
				if (!flag)
				{
					TypeConverter converter = TypeDescriptor.GetConverter(ReflectionHelper.GetReflectionType(content));
					ContentPresenter.DefaultTemplate.TypeContext context = new ContentPresenter.DefaultTemplate.TypeContext(content);
					if (converter != null && converter.CanConvertTo(context, typeof(string)))
					{
						textBlock.Text = (string)converter.ConvertTo(context, specificCulture, content, typeof(string));
						return;
					}
					textBlock.Text = string.Format(specificCulture, "{0}", new object[]
					{
						content
					});
				}
			}

			// Token: 0x02000BAD RID: 2989
			private class TypeContext : ITypeDescriptorContext, IServiceProvider
			{
				// Token: 0x060091DC RID: 37340 RVA: 0x0025F4BC File Offset: 0x0025D6BC
				public TypeContext(object instance)
				{
					this._instance = instance;
				}

				// Token: 0x17001FD6 RID: 8150
				// (get) Token: 0x060091DD RID: 37341 RVA: 0x0000C238 File Offset: 0x0000A438
				IContainer ITypeDescriptorContext.Container
				{
					get
					{
						return null;
					}
				}

				// Token: 0x17001FD7 RID: 8151
				// (get) Token: 0x060091DE RID: 37342 RVA: 0x0025F4CB File Offset: 0x0025D6CB
				object ITypeDescriptorContext.Instance
				{
					get
					{
						return this._instance;
					}
				}

				// Token: 0x17001FD8 RID: 8152
				// (get) Token: 0x060091DF RID: 37343 RVA: 0x0000C238 File Offset: 0x0000A438
				PropertyDescriptor ITypeDescriptorContext.PropertyDescriptor
				{
					get
					{
						return null;
					}
				}

				// Token: 0x060091E0 RID: 37344 RVA: 0x00002137 File Offset: 0x00000337
				void ITypeDescriptorContext.OnComponentChanged()
				{
				}

				// Token: 0x060091E1 RID: 37345 RVA: 0x0000B02A File Offset: 0x0000922A
				bool ITypeDescriptorContext.OnComponentChanging()
				{
					return false;
				}

				// Token: 0x060091E2 RID: 37346 RVA: 0x0000C238 File Offset: 0x0000A438
				object IServiceProvider.GetService(Type serviceType)
				{
					return null;
				}

				// Token: 0x04004EDE RID: 20190
				private object _instance;
			}
		}

		// Token: 0x02000962 RID: 2402
		private class DefaultSelector : DataTemplateSelector
		{
			// Token: 0x0600873B RID: 34619 RVA: 0x0024F548 File Offset: 0x0024D748
			public override DataTemplate SelectTemplate(object item, DependencyObject container)
			{
				DataTemplate dataTemplate = null;
				if (item != null)
				{
					dataTemplate = (DataTemplate)FrameworkElement.FindTemplateResourceInternal(container, item, typeof(DataTemplate));
				}
				if (dataTemplate == null)
				{
					string s;
					TypeConverter converter;
					if ((s = (item as string)) != null)
					{
						dataTemplate = ((ContentPresenter)container).SelectTemplateForString(s);
					}
					else if (item is UIElement)
					{
						dataTemplate = ContentPresenter.UIElementContentTemplate;
					}
					else if (SystemXmlHelper.IsXmlNode(item))
					{
						dataTemplate = ((ContentPresenter)container).SelectTemplateForXML();
					}
					else if (item is Inline)
					{
						dataTemplate = ContentPresenter.DefaultContentTemplate;
					}
					else if (item != null && (converter = TypeDescriptor.GetConverter(ReflectionHelper.GetReflectionType(item))) != null && converter.CanConvertTo(typeof(UIElement)))
					{
						dataTemplate = ContentPresenter.UIElementContentTemplate;
					}
					else
					{
						dataTemplate = ContentPresenter.DefaultContentTemplate;
					}
				}
				return dataTemplate;
			}
		}
	}
}
