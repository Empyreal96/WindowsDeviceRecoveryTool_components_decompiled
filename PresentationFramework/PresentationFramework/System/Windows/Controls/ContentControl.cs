using System;
using System.Collections;
using System.ComponentModel;
using System.Windows.Data;
using System.Windows.Markup;
using MS.Internal;
using MS.Internal.Controls;
using MS.Internal.KnownBoxes;
using MS.Internal.PresentationFramework;
using MS.Internal.Telemetry.PresentationFramework;

namespace System.Windows.Controls
{
	/// <summary>Represents a control with a single piece of content of any type.  </summary>
	// Token: 0x02000488 RID: 1160
	[DefaultProperty("Content")]
	[ContentProperty("Content")]
	[Localizability(LocalizationCategory.None, Readability = Readability.Unreadable)]
	public class ContentControl : Control, IAddChild
	{
		// Token: 0x060043D2 RID: 17362 RVA: 0x00135D40 File Offset: 0x00133F40
		static ContentControl()
		{
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(ContentControl), new FrameworkPropertyMetadata(typeof(ContentControl)));
			ContentControl._dType = DependencyObjectType.FromSystemTypeInternal(typeof(ContentControl));
			ControlsTraceLogger.AddControl(TelemetryControls.ContentControl);
		}

		/// <summary> Gets an enumerator to the content control's logical child elements. </summary>
		/// <returns>An enumerator. The default value is <see langword="null" />.</returns>
		// Token: 0x170010A2 RID: 4258
		// (get) Token: 0x060043D3 RID: 17363 RVA: 0x00135EA0 File Offset: 0x001340A0
		protected internal override IEnumerator LogicalChildren
		{
			get
			{
				object content = this.Content;
				if (this.ContentIsNotLogical || content == null)
				{
					return EmptyEnumerator.Instance;
				}
				DependencyObject templatedParent = base.TemplatedParent;
				if (templatedParent != null)
				{
					DependencyObject dependencyObject = content as DependencyObject;
					if (dependencyObject != null)
					{
						DependencyObject parent = LogicalTreeHelper.GetParent(dependencyObject);
						if (parent != null && parent != this)
						{
							return EmptyEnumerator.Instance;
						}
					}
				}
				return new ContentModelTreeEnumerator(this, content);
			}
		}

		// Token: 0x060043D4 RID: 17364 RVA: 0x00135EF4 File Offset: 0x001340F4
		internal override string GetPlainText()
		{
			return ContentControl.ContentObjectToString(this.Content);
		}

		// Token: 0x060043D5 RID: 17365 RVA: 0x00135F04 File Offset: 0x00134104
		internal static string ContentObjectToString(object content)
		{
			if (content == null)
			{
				return string.Empty;
			}
			FrameworkElement frameworkElement = content as FrameworkElement;
			if (frameworkElement != null)
			{
				return frameworkElement.GetPlainText();
			}
			return content.ToString();
		}

		// Token: 0x060043D6 RID: 17366 RVA: 0x00135F34 File Offset: 0x00134134
		internal void PrepareContentControl(object item, DataTemplate itemTemplate, DataTemplateSelector itemTemplateSelector, string itemStringFormat)
		{
			if (item != this)
			{
				this.ContentIsNotLogical = true;
				if (this.ContentIsItem || !base.HasNonDefaultValue(ContentControl.ContentProperty))
				{
					this.Content = item;
					this.ContentIsItem = true;
				}
				if (itemTemplate != null)
				{
					base.SetValue(ContentControl.ContentTemplateProperty, itemTemplate);
				}
				if (itemTemplateSelector != null)
				{
					base.SetValue(ContentControl.ContentTemplateSelectorProperty, itemTemplateSelector);
				}
				if (itemStringFormat != null)
				{
					base.SetValue(ContentControl.ContentStringFormatProperty, itemStringFormat);
					return;
				}
			}
			else
			{
				this.ContentIsNotLogical = false;
			}
		}

		// Token: 0x060043D7 RID: 17367 RVA: 0x00135FA6 File Offset: 0x001341A6
		internal void ClearContentControl(object item)
		{
			if (item != this && this.ContentIsItem)
			{
				this.Content = BindingExpressionBase.DisconnectedItem;
			}
		}

		/// <summary>Indicates whether the <see cref="P:System.Windows.Controls.ContentControl.Content" /> property should be persisted.</summary>
		/// <returns>
		///     <see langword="true" /> if the <see cref="P:System.Windows.Controls.ContentControl.Content" /> property should be persisted; otherwise, <see langword="false" />.</returns>
		// Token: 0x060043D8 RID: 17368 RVA: 0x00135FBF File Offset: 0x001341BF
		[EditorBrowsable(EditorBrowsableState.Never)]
		public virtual bool ShouldSerializeContent()
		{
			return base.ReadLocalValue(ContentControl.ContentProperty) != DependencyProperty.UnsetValue;
		}

		/// <summary>This type or member supports the Windows Presentation Foundation (WPF) infrastructure and is not intended to be used directly from your code.</summary>
		/// <param name="value">  An object to add as a child.</param>
		// Token: 0x060043D9 RID: 17369 RVA: 0x00135FD6 File Offset: 0x001341D6
		void IAddChild.AddChild(object value)
		{
			this.AddChild(value);
		}

		/// <summary>Adds a specified object as the child of a <see cref="T:System.Windows.Controls.ContentControl" />. </summary>
		/// <param name="value">The object to add.</param>
		// Token: 0x060043DA RID: 17370 RVA: 0x00135FDF File Offset: 0x001341DF
		protected virtual void AddChild(object value)
		{
			if (this.Content == null || value == null)
			{
				this.Content = value;
				return;
			}
			throw new InvalidOperationException(SR.Get("ContentControlCannotHaveMultipleContent"));
		}

		/// <summary>This type or member supports the Windows Presentation Foundation (WPF) infrastructure and is not intended to be used directly from your code.</summary>
		/// <param name="text">  A string to add to the object.</param>
		// Token: 0x060043DB RID: 17371 RVA: 0x00136003 File Offset: 0x00134203
		void IAddChild.AddText(string text)
		{
			this.AddText(text);
		}

		/// <summary>Adds a specified text string to a <see cref="T:System.Windows.Controls.ContentControl" />. </summary>
		/// <param name="text">The string to add.</param>
		// Token: 0x060043DC RID: 17372 RVA: 0x00135FD6 File Offset: 0x001341D6
		protected virtual void AddText(string text)
		{
			this.AddChild(text);
		}

		/// <summary> Gets or sets the content of a <see cref="T:System.Windows.Controls.ContentControl" />. </summary>
		/// <returns>An object that contains the control's content. The default value is <see langword="null" />.</returns>
		// Token: 0x170010A3 RID: 4259
		// (get) Token: 0x060043DD RID: 17373 RVA: 0x0013600C File Offset: 0x0013420C
		// (set) Token: 0x060043DE RID: 17374 RVA: 0x00136019 File Offset: 0x00134219
		[Bindable(true)]
		[CustomCategory("Content")]
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

		// Token: 0x060043DF RID: 17375 RVA: 0x00136028 File Offset: 0x00134228
		private static void OnContentChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			ContentControl contentControl = (ContentControl)d;
			contentControl.SetValue(ContentControl.HasContentPropertyKey, (e.NewValue != null) ? BooleanBoxes.TrueBox : BooleanBoxes.FalseBox);
			contentControl.OnContentChanged(e.OldValue, e.NewValue);
		}

		/// <summary> Called when the <see cref="P:System.Windows.Controls.ContentControl.Content" /> property changes. </summary>
		/// <param name="oldContent">The old value of the <see cref="P:System.Windows.Controls.ContentControl.Content" /> property.</param>
		/// <param name="newContent">The new value of the <see cref="P:System.Windows.Controls.ContentControl.Content" /> property.</param>
		// Token: 0x060043E0 RID: 17376 RVA: 0x00136070 File Offset: 0x00134270
		protected virtual void OnContentChanged(object oldContent, object newContent)
		{
			base.RemoveLogicalChild(oldContent);
			if (this.ContentIsNotLogical)
			{
				return;
			}
			DependencyObject dependencyObject = newContent as DependencyObject;
			if (dependencyObject != null)
			{
				DependencyObject parent = LogicalTreeHelper.GetParent(dependencyObject);
				if (parent != null)
				{
					if (base.TemplatedParent != null && FrameworkObject.IsEffectiveAncestor(parent, this))
					{
						return;
					}
					LogicalTreeHelper.RemoveLogicalChild(parent, newContent);
				}
			}
			base.AddLogicalChild(newContent);
		}

		/// <summary> Gets a value that indicates whether the <see cref="T:System.Windows.Controls.ContentControl" /> contains content. </summary>
		/// <returns>
		///     <see langword="true" /> if the <see cref="T:System.Windows.Controls.ContentControl" /> has content; otherwise <see langword="false" />. The default value is <see langword="false" />.</returns>
		// Token: 0x170010A4 RID: 4260
		// (get) Token: 0x060043E1 RID: 17377 RVA: 0x001360C1 File Offset: 0x001342C1
		[Browsable(false)]
		[ReadOnly(true)]
		public bool HasContent
		{
			get
			{
				return (bool)base.GetValue(ContentControl.HasContentProperty);
			}
		}

		/// <summary> Gets or sets the data template used to display the content of the <see cref="T:System.Windows.Controls.ContentControl" />. </summary>
		/// <returns>A data template. The default value is <see langword="null" />.</returns>
		// Token: 0x170010A5 RID: 4261
		// (get) Token: 0x060043E2 RID: 17378 RVA: 0x001360D3 File Offset: 0x001342D3
		// (set) Token: 0x060043E3 RID: 17379 RVA: 0x001360E5 File Offset: 0x001342E5
		[Bindable(true)]
		[CustomCategory("Content")]
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

		// Token: 0x060043E4 RID: 17380 RVA: 0x001360F4 File Offset: 0x001342F4
		private static void OnContentTemplateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			ContentControl contentControl = (ContentControl)d;
			contentControl.OnContentTemplateChanged((DataTemplate)e.OldValue, (DataTemplate)e.NewValue);
		}

		/// <summary> Called when the <see cref="P:System.Windows.Controls.ContentControl.ContentTemplate" /> property changes. </summary>
		/// <param name="oldContentTemplate">The old value of the <see cref="P:System.Windows.Controls.ContentControl.ContentTemplate" /> property.</param>
		/// <param name="newContentTemplate">The new value of the <see cref="P:System.Windows.Controls.ContentControl.ContentTemplate" /> property.</param>
		// Token: 0x060043E5 RID: 17381 RVA: 0x00136126 File Offset: 0x00134326
		protected virtual void OnContentTemplateChanged(DataTemplate oldContentTemplate, DataTemplate newContentTemplate)
		{
			Helper.CheckTemplateAndTemplateSelector("Content", ContentControl.ContentTemplateProperty, ContentControl.ContentTemplateSelectorProperty, this);
		}

		/// <summary> Gets or sets a template selector that enables an application writer to provide custom template-selection logic. </summary>
		/// <returns>A data template selector. The default value is <see langword="null" />.</returns>
		// Token: 0x170010A6 RID: 4262
		// (get) Token: 0x060043E6 RID: 17382 RVA: 0x0013613D File Offset: 0x0013433D
		// (set) Token: 0x060043E7 RID: 17383 RVA: 0x0013614F File Offset: 0x0013434F
		[Bindable(true)]
		[CustomCategory("Content")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
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

		// Token: 0x060043E8 RID: 17384 RVA: 0x00136160 File Offset: 0x00134360
		private static void OnContentTemplateSelectorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			ContentControl contentControl = (ContentControl)d;
			contentControl.OnContentTemplateSelectorChanged((DataTemplateSelector)e.NewValue, (DataTemplateSelector)e.NewValue);
		}

		/// <summary> Called when the <see cref="P:System.Windows.Controls.ContentControl.ContentTemplateSelector" /> property changes. </summary>
		/// <param name="oldContentTemplateSelector">The old value of the <see cref="P:System.Windows.Controls.ContentControl.ContentTemplateSelector" /> property.</param>
		/// <param name="newContentTemplateSelector">The new value of the <see cref="P:System.Windows.Controls.ContentControl.ContentTemplateSelector" /> property.</param>
		// Token: 0x060043E9 RID: 17385 RVA: 0x00136126 File Offset: 0x00134326
		protected virtual void OnContentTemplateSelectorChanged(DataTemplateSelector oldContentTemplateSelector, DataTemplateSelector newContentTemplateSelector)
		{
			Helper.CheckTemplateAndTemplateSelector("Content", ContentControl.ContentTemplateProperty, ContentControl.ContentTemplateSelectorProperty, this);
		}

		/// <summary>Gets or sets a composite string that specifies how to format the <see cref="P:System.Windows.Controls.ContentControl.Content" /> property if it is displayed as a string.</summary>
		/// <returns>A composite string that specifies how to format the <see cref="P:System.Windows.Controls.ContentControl.Content" /> property if it is displayed as a string.</returns>
		// Token: 0x170010A7 RID: 4263
		// (get) Token: 0x060043EA RID: 17386 RVA: 0x00136192 File Offset: 0x00134392
		// (set) Token: 0x060043EB RID: 17387 RVA: 0x001361A4 File Offset: 0x001343A4
		[Bindable(true)]
		[CustomCategory("Content")]
		public string ContentStringFormat
		{
			get
			{
				return (string)base.GetValue(ContentControl.ContentStringFormatProperty);
			}
			set
			{
				base.SetValue(ContentControl.ContentStringFormatProperty, value);
			}
		}

		// Token: 0x060043EC RID: 17388 RVA: 0x001361B4 File Offset: 0x001343B4
		private static void OnContentStringFormatChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			ContentControl contentControl = (ContentControl)d;
			contentControl.OnContentStringFormatChanged((string)e.OldValue, (string)e.NewValue);
		}

		/// <summary>Occurs when the <see cref="P:System.Windows.Controls.ContentControl.ContentStringFormat" /> property changes.</summary>
		/// <param name="oldContentStringFormat">The old value of <see cref="P:System.Windows.Controls.ContentControl.ContentStringFormat" />.</param>
		/// <param name="newContentStringFormat">The new value of <see cref="P:System.Windows.Controls.ContentControl.ContentStringFormat" />.</param>
		// Token: 0x060043ED RID: 17389 RVA: 0x00002137 File Offset: 0x00000337
		protected virtual void OnContentStringFormatChanged(string oldContentStringFormat, string newContentStringFormat)
		{
		}

		// Token: 0x170010A8 RID: 4264
		// (get) Token: 0x060043EE RID: 17390 RVA: 0x001361E6 File Offset: 0x001343E6
		// (set) Token: 0x060043EF RID: 17391 RVA: 0x001361EF File Offset: 0x001343EF
		internal bool ContentIsNotLogical
		{
			get
			{
				return base.ReadControlFlag(Control.ControlBoolFlags.ContentIsNotLogical);
			}
			set
			{
				base.WriteControlFlag(Control.ControlBoolFlags.ContentIsNotLogical, value);
			}
		}

		// Token: 0x170010A9 RID: 4265
		// (get) Token: 0x060043F0 RID: 17392 RVA: 0x001361F9 File Offset: 0x001343F9
		// (set) Token: 0x060043F1 RID: 17393 RVA: 0x00136203 File Offset: 0x00134403
		internal bool ContentIsItem
		{
			get
			{
				return base.ReadControlFlag(Control.ControlBoolFlags.ContentIsItem);
			}
			set
			{
				base.WriteControlFlag(Control.ControlBoolFlags.ContentIsItem, value);
			}
		}

		// Token: 0x170010AA RID: 4266
		// (get) Token: 0x060043F2 RID: 17394 RVA: 0x00094CFC File Offset: 0x00092EFC
		internal override int EffectiveValuesInitialSize
		{
			get
			{
				return 4;
			}
		}

		// Token: 0x170010AB RID: 4267
		// (get) Token: 0x060043F3 RID: 17395 RVA: 0x0013620E File Offset: 0x0013440E
		internal override DependencyObjectType DTypeThemeStyleKey
		{
			get
			{
				return ContentControl._dType;
			}
		}

		/// <summary> Identifies the <see cref="P:System.Windows.Controls.ContentControl.Content" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.ContentControl.Content" /> dependency property.</returns>
		// Token: 0x04002866 RID: 10342
		[CommonDependencyProperty]
		public static readonly DependencyProperty ContentProperty = DependencyProperty.Register("Content", typeof(object), typeof(ContentControl), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(ContentControl.OnContentChanged)));

		// Token: 0x04002867 RID: 10343
		private static readonly DependencyPropertyKey HasContentPropertyKey = DependencyProperty.RegisterReadOnly("HasContent", typeof(bool), typeof(ContentControl), new FrameworkPropertyMetadata(BooleanBoxes.FalseBox, FrameworkPropertyMetadataOptions.None));

		/// <summary> Identifies the <see cref="P:System.Windows.Controls.ContentControl.HasContent" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.ContentControl.HasContent" /> dependency property.</returns>
		// Token: 0x04002868 RID: 10344
		[CommonDependencyProperty]
		public static readonly DependencyProperty HasContentProperty = ContentControl.HasContentPropertyKey.DependencyProperty;

		/// <summary> Identifies the <see cref="P:System.Windows.Controls.ContentControl.ContentTemplate" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.ContentControl.ContentTemplate" /> dependency property.</returns>
		// Token: 0x04002869 RID: 10345
		[CommonDependencyProperty]
		public static readonly DependencyProperty ContentTemplateProperty = DependencyProperty.Register("ContentTemplate", typeof(DataTemplate), typeof(ContentControl), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(ContentControl.OnContentTemplateChanged)));

		/// <summary> Identifies the <see cref="P:System.Windows.Controls.ContentControl.ContentTemplateSelector" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.ContentControl.ContentTemplateSelector" /> dependency property.</returns>
		// Token: 0x0400286A RID: 10346
		[CommonDependencyProperty]
		public static readonly DependencyProperty ContentTemplateSelectorProperty = DependencyProperty.Register("ContentTemplateSelector", typeof(DataTemplateSelector), typeof(ContentControl), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(ContentControl.OnContentTemplateSelectorChanged)));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.ContentControl.ContentStringFormat" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.ContentControl.ContentStringFormat" /> dependency property.</returns>
		// Token: 0x0400286B RID: 10347
		[CommonDependencyProperty]
		public static readonly DependencyProperty ContentStringFormatProperty = DependencyProperty.Register("ContentStringFormat", typeof(string), typeof(ContentControl), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(ContentControl.OnContentStringFormatChanged)));

		// Token: 0x0400286C RID: 10348
		private static DependencyObjectType _dType;
	}
}
