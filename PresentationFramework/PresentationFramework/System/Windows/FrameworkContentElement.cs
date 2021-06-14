using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Security;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Diagnostics;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using MS.Internal;
using MS.Internal.KnownBoxes;
using MS.Internal.PresentationFramework;
using MS.Utility;

namespace System.Windows
{
	/// <summary>
	///     <see cref="T:System.Windows.FrameworkContentElement" /> is the WPF framework-level implementation and expansion of the <see cref="T:System.Windows.ContentElement" /> base class. <see cref="T:System.Windows.FrameworkContentElement" /> adds support for additional input APIs (including tooltips and context menus), storyboards, data context for data binding, styles support, and logical tree helper APIs. </summary>
	// Token: 0x020000BF RID: 191
	[StyleTypedProperty(Property = "FocusVisualStyle", StyleTargetType = typeof(Control))]
	[XmlLangProperty("Language")]
	[UsableDuringInitialization(true)]
	[RuntimeNameProperty("Name")]
	public class FrameworkContentElement : ContentElement, IFrameworkInputElement, IInputElement, ISupportInitialize, IQueryAmbient
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.FrameworkContentElement" /> class. </summary>
		// Token: 0x06000419 RID: 1049 RVA: 0x0000BBB4 File Offset: 0x00009DB4
		public FrameworkContentElement()
		{
			PropertyMetadata metadata = FrameworkContentElement.StyleProperty.GetMetadata(base.DependencyObjectType);
			Style style = (Style)metadata.DefaultValue;
			if (style != null)
			{
				FrameworkContentElement.OnStyleChanged(this, new DependencyPropertyChangedEventArgs(FrameworkContentElement.StyleProperty, metadata, null, style));
			}
			Application application = Application.Current;
			if (application != null && application.HasImplicitStylesInResources)
			{
				this.ShouldLookupImplicitStyles = true;
			}
		}

		// Token: 0x0600041A RID: 1050 RVA: 0x0000BC20 File Offset: 0x00009E20
		static FrameworkContentElement()
		{
			FrameworkContentElement.LoadedEvent = FrameworkElement.LoadedEvent.AddOwner(typeof(FrameworkContentElement));
			FrameworkContentElement.UnloadedEvent = FrameworkElement.UnloadedEvent.AddOwner(typeof(FrameworkContentElement));
			FrameworkContentElement.ToolTipProperty = ToolTipService.ToolTipProperty.AddOwner(typeof(FrameworkContentElement));
			FrameworkContentElement.ContextMenuProperty = ContextMenuService.ContextMenuProperty.AddOwner(typeof(FrameworkContentElement), new FrameworkPropertyMetadata(null));
			FrameworkContentElement.ToolTipOpeningEvent = ToolTipService.ToolTipOpeningEvent.AddOwner(typeof(FrameworkContentElement));
			FrameworkContentElement.ToolTipClosingEvent = ToolTipService.ToolTipClosingEvent.AddOwner(typeof(FrameworkContentElement));
			FrameworkContentElement.ContextMenuOpeningEvent = ContextMenuService.ContextMenuOpeningEvent.AddOwner(typeof(FrameworkContentElement));
			FrameworkContentElement.ContextMenuClosingEvent = ContextMenuService.ContextMenuClosingEvent.AddOwner(typeof(FrameworkContentElement));
			FrameworkContentElement.ResourcesField = FrameworkElement.ResourcesField;
			FrameworkContentElement.DType = DependencyObjectType.FromSystemTypeInternal(typeof(FrameworkContentElement));
			FrameworkContentElement.InheritanceContextField = new UncommonField<DependencyObject>();
			FrameworkContentElement.MentorField = new UncommonField<DependencyObject>();
			PropertyChangedCallback propertyChangedCallback = new PropertyChangedCallback(FrameworkContentElement.NumberSubstitutionChanged);
			NumberSubstitution.CultureSourceProperty.OverrideMetadata(typeof(FrameworkContentElement), new FrameworkPropertyMetadata(NumberCultureSource.Text, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Inherits, propertyChangedCallback));
			NumberSubstitution.CultureOverrideProperty.OverrideMetadata(typeof(FrameworkContentElement), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Inherits, propertyChangedCallback));
			NumberSubstitution.SubstitutionProperty.OverrideMetadata(typeof(FrameworkContentElement), new FrameworkPropertyMetadata(NumberSubstitutionMethod.AsCulture, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Inherits, propertyChangedCallback));
			EventManager.RegisterClassHandler(typeof(FrameworkContentElement), Mouse.QueryCursorEvent, new QueryCursorEventHandler(FrameworkContentElement.OnQueryCursor), true);
			ContentElement.AllowDropProperty.OverrideMetadata(typeof(FrameworkContentElement), new FrameworkPropertyMetadata(BooleanBoxes.FalseBox, FrameworkPropertyMetadataOptions.Inherits));
			Stylus.IsPressAndHoldEnabledProperty.AddOwner(typeof(FrameworkContentElement), new FrameworkPropertyMetadata(BooleanBoxes.TrueBox, FrameworkPropertyMetadataOptions.Inherits));
			Stylus.IsFlicksEnabledProperty.AddOwner(typeof(FrameworkContentElement), new FrameworkPropertyMetadata(BooleanBoxes.TrueBox, FrameworkPropertyMetadataOptions.Inherits));
			Stylus.IsTapFeedbackEnabledProperty.AddOwner(typeof(FrameworkContentElement), new FrameworkPropertyMetadata(BooleanBoxes.TrueBox, FrameworkPropertyMetadataOptions.Inherits));
			Stylus.IsTouchFeedbackEnabledProperty.AddOwner(typeof(FrameworkContentElement), new FrameworkPropertyMetadata(BooleanBoxes.TrueBox, FrameworkPropertyMetadataOptions.Inherits));
			EventManager.RegisterClassHandler(typeof(FrameworkContentElement), FrameworkContentElement.ToolTipOpeningEvent, new ToolTipEventHandler(FrameworkContentElement.OnToolTipOpeningThunk));
			EventManager.RegisterClassHandler(typeof(FrameworkContentElement), FrameworkContentElement.ToolTipClosingEvent, new ToolTipEventHandler(FrameworkContentElement.OnToolTipClosingThunk));
			EventManager.RegisterClassHandler(typeof(FrameworkContentElement), FrameworkContentElement.ContextMenuOpeningEvent, new ContextMenuEventHandler(FrameworkContentElement.OnContextMenuOpeningThunk));
			EventManager.RegisterClassHandler(typeof(FrameworkContentElement), FrameworkContentElement.ContextMenuClosingEvent, new ContextMenuEventHandler(FrameworkContentElement.OnContextMenuClosingThunk));
			EventManager.RegisterClassHandler(typeof(FrameworkContentElement), Keyboard.GotKeyboardFocusEvent, new KeyboardFocusChangedEventHandler(FrameworkContentElement.OnGotKeyboardFocus));
			EventManager.RegisterClassHandler(typeof(FrameworkContentElement), Keyboard.LostKeyboardFocusEvent, new KeyboardFocusChangedEventHandler(FrameworkContentElement.OnLostKeyboardFocus));
		}

		// Token: 0x0600041B RID: 1051 RVA: 0x0000C141 File Offset: 0x0000A341
		private static void NumberSubstitutionChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
		{
			((FrameworkContentElement)o).HasNumberSubstitutionChanged = true;
		}

		/// <summary>Gets or sets the style to be used by this element.  </summary>
		/// <returns>The applied, nondefault style for the element, if present. Otherwise, <see langword="null" />. The default for a default-constructed <see cref="T:System.Windows.FrameworkContentElement" /> is <see langword="null" />.</returns>
		// Token: 0x170000A3 RID: 163
		// (get) Token: 0x0600041C RID: 1052 RVA: 0x0000C14F File Offset: 0x0000A34F
		// (set) Token: 0x0600041D RID: 1053 RVA: 0x0000C157 File Offset: 0x0000A357
		public Style Style
		{
			get
			{
				return this._styleCache;
			}
			set
			{
				base.SetValue(FrameworkContentElement.StyleProperty, value);
			}
		}

		/// <summary>Returns whether serialization processes should serialize the contents of the <see cref="P:System.Windows.FrameworkContentElement.Style" /> property on instances of this class.</summary>
		/// <returns>
		///     <see langword="true" /> if the <see cref="P:System.Windows.FrameworkContentElement.Style" /> property value should be serialized; otherwise, <see langword="false" />.</returns>
		// Token: 0x0600041E RID: 1054 RVA: 0x0000C165 File Offset: 0x0000A365
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool ShouldSerializeStyle()
		{
			return !this.IsStyleSetFromGenerator && base.ReadLocalValue(FrameworkContentElement.StyleProperty) != DependencyProperty.UnsetValue;
		}

		// Token: 0x0600041F RID: 1055 RVA: 0x0000C188 File Offset: 0x0000A388
		private static void OnStyleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			FrameworkContentElement frameworkContentElement = (FrameworkContentElement)d;
			frameworkContentElement.HasLocalStyle = (e.NewEntry.BaseValueSourceInternal == BaseValueSourceInternal.Local);
			StyleHelper.UpdateStyleCache(null, frameworkContentElement, (Style)e.OldValue, (Style)e.NewValue, ref frameworkContentElement._styleCache);
		}

		/// <summary>Invoked when the style that is in use on this element changes. </summary>
		/// <param name="oldStyle">The old style.</param>
		/// <param name="newStyle">The new style.</param>
		// Token: 0x06000420 RID: 1056 RVA: 0x0000C1DA File Offset: 0x0000A3DA
		protected internal virtual void OnStyleChanged(Style oldStyle, Style newStyle)
		{
			this.HasStyleChanged = true;
		}

		/// <summary>Gets or sets a value indicating whether this element incorporates style properties from theme styles. </summary>
		/// <returns>
		///     <see langword="true" /> if this element does not use theme style properties; all style-originating properties come from local application styles, and theme style properties do not apply. <see langword="false" /> if application styles apply first, and then theme styles apply for properties that were not specifically set in application styles.</returns>
		// Token: 0x170000A4 RID: 164
		// (get) Token: 0x06000421 RID: 1057 RVA: 0x0000C1E3 File Offset: 0x0000A3E3
		// (set) Token: 0x06000422 RID: 1058 RVA: 0x0000C1F5 File Offset: 0x0000A3F5
		public bool OverridesDefaultStyle
		{
			get
			{
				return (bool)base.GetValue(FrameworkContentElement.OverridesDefaultStyleProperty);
			}
			set
			{
				base.SetValue(FrameworkContentElement.OverridesDefaultStyleProperty, BooleanBoxes.Box(value));
			}
		}

		/// <summary>Gets or sets the key to use to find the style template for this control in themes. </summary>
		/// <returns>The style key. To work correctly as part of theme style lookup, this value is expected to be the <see cref="T:System.Type" /> of the element being styled. <see langword="null" /> is an accepted value for a certain case; see Remarks.</returns>
		// Token: 0x170000A5 RID: 165
		// (get) Token: 0x06000423 RID: 1059 RVA: 0x0000C208 File Offset: 0x0000A408
		// (set) Token: 0x06000424 RID: 1060 RVA: 0x0000C215 File Offset: 0x0000A415
		protected internal object DefaultStyleKey
		{
			get
			{
				return base.GetValue(FrameworkContentElement.DefaultStyleKeyProperty);
			}
			set
			{
				base.SetValue(FrameworkContentElement.DefaultStyleKeyProperty, value);
			}
		}

		// Token: 0x06000425 RID: 1061 RVA: 0x0000C223 File Offset: 0x0000A423
		private static void OnThemeStyleKeyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((FrameworkContentElement)d).UpdateThemeStyleProperty();
		}

		// Token: 0x170000A6 RID: 166
		// (get) Token: 0x06000426 RID: 1062 RVA: 0x0000C230 File Offset: 0x0000A430
		internal Style ThemeStyle
		{
			get
			{
				return this._themeStyleCache;
			}
		}

		// Token: 0x170000A7 RID: 167
		// (get) Token: 0x06000427 RID: 1063 RVA: 0x0000C238 File Offset: 0x0000A438
		internal virtual DependencyObjectType DTypeThemeStyleKey
		{
			get
			{
				return null;
			}
		}

		// Token: 0x06000428 RID: 1064 RVA: 0x0000C23C File Offset: 0x0000A43C
		internal static void OnThemeStyleChanged(DependencyObject d, object oldValue, object newValue)
		{
			FrameworkContentElement frameworkContentElement = (FrameworkContentElement)d;
			StyleHelper.UpdateThemeStyleCache(null, frameworkContentElement, (Style)oldValue, (Style)newValue, ref frameworkContentElement._themeStyleCache);
		}

		/// <summary> Gets a reference to the template parent of this element. This property is not relevant if the element was not created through a template. </summary>
		/// <returns>The element whose <see cref="T:System.Windows.FrameworkTemplate" /> <see cref="P:System.Windows.FrameworkTemplate.VisualTree" /> caused this element to be created. This value is frequently <see langword="null" />; see Remarks.</returns>
		// Token: 0x170000A8 RID: 168
		// (get) Token: 0x06000429 RID: 1065 RVA: 0x0000C269 File Offset: 0x0000A469
		public DependencyObject TemplatedParent
		{
			get
			{
				return this._templatedParent;
			}
		}

		// Token: 0x170000A9 RID: 169
		// (get) Token: 0x0600042A RID: 1066 RVA: 0x0000C274 File Offset: 0x0000A474
		internal bool HasResources
		{
			get
			{
				ResourceDictionary value = FrameworkContentElement.ResourcesField.GetValue(this);
				return value != null && (value.Count > 0 || value.MergedDictionaries.Count > 0);
			}
		}

		/// <summary>Gets or sets the current locally-defined resource dictionary. </summary>
		/// <returns>The current locally-defined resources. This is a dictionary of resources, where resources within the dictionary are accessed by key.</returns>
		// Token: 0x170000AA RID: 170
		// (get) Token: 0x0600042B RID: 1067 RVA: 0x0000C2AC File Offset: 0x0000A4AC
		// (set) Token: 0x0600042C RID: 1068 RVA: 0x0000C2E4 File Offset: 0x0000A4E4
		[Ambient]
		public ResourceDictionary Resources
		{
			get
			{
				ResourceDictionary resourceDictionary = FrameworkContentElement.ResourcesField.GetValue(this);
				if (resourceDictionary == null)
				{
					resourceDictionary = new ResourceDictionary();
					resourceDictionary.AddOwner(this);
					FrameworkContentElement.ResourcesField.SetValue(this, resourceDictionary);
				}
				return resourceDictionary;
			}
			set
			{
				ResourceDictionary value2 = FrameworkContentElement.ResourcesField.GetValue(this);
				FrameworkContentElement.ResourcesField.SetValue(this, value);
				if (value2 != null)
				{
					value2.RemoveOwner(this);
				}
				if (value != null && !value.ContainsOwner(this))
				{
					value.AddOwner(this);
				}
				if (value2 != value)
				{
					TreeWalkHelper.InvalidateOnResourcesChange(null, this, new ResourcesChangeInfo(value2, value));
				}
			}
		}

		/// <summary>For a description of this member, see the <see cref="M:System.Windows.Markup.IQueryAmbient.IsAmbientPropertyAvailable(System.String)" /> method.</summary>
		/// <param name="propertyName">The name of the requested ambient property.</param>
		/// <returns>
		///     <see langword="true" /> if <paramref name="propertyName" /> is available; otherwise, <see langword="false" />. </returns>
		// Token: 0x0600042D RID: 1069 RVA: 0x0000C338 File Offset: 0x0000A538
		bool IQueryAmbient.IsAmbientPropertyAvailable(string propertyName)
		{
			return propertyName != "Resources" || this.HasResources;
		}

		/// <summary>Returns whether serialization processes should serialize the contents of the <see cref="P:System.Windows.FrameworkContentElement.Resources" /> property on instances of this class.</summary>
		/// <returns>
		///     <see langword="true" /> if the <see cref="P:System.Windows.FrameworkContentElement.Resources" /> property value should be serialized; otherwise, <see langword="false" />.</returns>
		// Token: 0x0600042E RID: 1070 RVA: 0x0000C34F File Offset: 0x0000A54F
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool ShouldSerializeResources()
		{
			return this.Resources != null && this.Resources.Count != 0;
		}

		/// <summary> Searches for a resource with the specified key, and will throw an exception if the requested resource is not found. </summary>
		/// <param name="resourceKey">Key identifier of the resource to be found.</param>
		/// <returns>The found resource, or <see langword="null" /> if no matching resource was found (but will also throw an exception if <see langword="null" />).</returns>
		/// <exception cref="T:System.Windows.ResourceReferenceKeyNotFoundException">The requested resource key was not found.</exception>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="resourceKey" /> is <see langword="null" />.</exception>
		// Token: 0x0600042F RID: 1071 RVA: 0x0000C36C File Offset: 0x0000A56C
		public object FindResource(object resourceKey)
		{
			if (resourceKey == null)
			{
				throw new ArgumentNullException("resourceKey");
			}
			object obj = FrameworkElement.FindResourceInternal(null, this, resourceKey);
			if (obj == DependencyProperty.UnsetValue)
			{
				Helper.ResourceFailureThrow(resourceKey);
			}
			return obj;
		}

		/// <summary>Searches for a resource with the specified key, and returns that resource if found. </summary>
		/// <param name="resourceKey">Key identifier of the resource to be found.</param>
		/// <returns>The found resource. If no resource was found, <see langword="null" /> is returned.</returns>
		// Token: 0x06000430 RID: 1072 RVA: 0x0000C3A0 File Offset: 0x0000A5A0
		public object TryFindResource(object resourceKey)
		{
			if (resourceKey == null)
			{
				throw new ArgumentNullException("resourceKey");
			}
			object obj = FrameworkElement.FindResourceInternal(null, this, resourceKey);
			if (obj == DependencyProperty.UnsetValue)
			{
				obj = null;
			}
			return obj;
		}

		/// <summary>Searches for a resource with the specified name and sets up a resource reference to it for the specified property. </summary>
		/// <param name="dp">The property to which the resource is bound.</param>
		/// <param name="name">The name of the resource.</param>
		// Token: 0x06000431 RID: 1073 RVA: 0x0000C3CF File Offset: 0x0000A5CF
		public void SetResourceReference(DependencyProperty dp, object name)
		{
			base.SetValue(dp, new ResourceReferenceExpression(name));
			this.HasResourceReference = true;
		}

		/// <summary>Begins the sequence of actions that are contained in the provided storyboard. </summary>
		/// <param name="storyboard">The storyboard to begin.</param>
		// Token: 0x06000432 RID: 1074 RVA: 0x0000C3E5 File Offset: 0x0000A5E5
		public void BeginStoryboard(Storyboard storyboard)
		{
			this.BeginStoryboard(storyboard, HandoffBehavior.SnapshotAndReplace, false);
		}

		/// <summary> Begins the sequence of actions that are contained in the provided storyboard, with options specified for what should occur if the property is already animated. </summary>
		/// <param name="storyboard">The storyboard to begin.</param>
		/// <param name="handoffBehavior">A value of the enumeration that describes behavior to use if a property described in the storyboard is already animated.</param>
		// Token: 0x06000433 RID: 1075 RVA: 0x0000C3F0 File Offset: 0x0000A5F0
		public void BeginStoryboard(Storyboard storyboard, HandoffBehavior handoffBehavior)
		{
			this.BeginStoryboard(storyboard, handoffBehavior, false);
		}

		/// <summary> Begins the sequence of actions that are contained in the provided storyboard, with specified state for control of the animation after it is started. </summary>
		/// <param name="storyboard">The storyboard to begin. </param>
		/// <param name="handoffBehavior">A value of the enumeration that describes behavior to use if a  property described in the storyboard is already animated.</param>
		/// <param name="isControllable">Declares whether the animation is controllable (can be paused) after it is started.</param>
		// Token: 0x06000434 RID: 1076 RVA: 0x0000C3FB File Offset: 0x0000A5FB
		public void BeginStoryboard(Storyboard storyboard, HandoffBehavior handoffBehavior, bool isControllable)
		{
			if (storyboard == null)
			{
				throw new ArgumentNullException("storyboard");
			}
			storyboard.Begin(this, handoffBehavior, isControllable);
		}

		// Token: 0x06000435 RID: 1077 RVA: 0x0000C414 File Offset: 0x0000A614
		internal sealed override void EvaluateBaseValueCore(DependencyProperty dp, PropertyMetadata metadata, ref EffectiveValueEntry newEntry)
		{
			if (dp == FrameworkContentElement.StyleProperty)
			{
				this.HasStyleEverBeenFetched = true;
				this.HasImplicitStyleFromResources = false;
				this.IsStyleSetFromGenerator = false;
			}
			this.GetRawValue(dp, metadata, ref newEntry);
			Storyboard.GetComplexPathValue(this, dp, ref newEntry, metadata);
		}

		// Token: 0x06000436 RID: 1078 RVA: 0x0000C448 File Offset: 0x0000A648
		internal void GetRawValue(DependencyProperty dp, PropertyMetadata metadata, ref EffectiveValueEntry entry)
		{
			if (entry.BaseValueSourceInternal == BaseValueSourceInternal.Local && entry.GetFlattenedEntry(RequestFlags.FullyResolved).Value != DependencyProperty.UnsetValue)
			{
				return;
			}
			if (this.TemplateChildIndex != -1 && this.GetValueFromTemplatedParent(dp, ref entry))
			{
				return;
			}
			if (dp != FrameworkContentElement.StyleProperty)
			{
				if (StyleHelper.GetValueFromStyleOrTemplate(new FrameworkObject(null, this), dp, ref entry))
				{
					return;
				}
			}
			else
			{
				object obj2;
				object obj = FrameworkElement.FindImplicitStyleResource(this, base.GetType(), out obj2);
				if (obj != DependencyProperty.UnsetValue)
				{
					this.HasImplicitStyleFromResources = true;
					entry.BaseValueSourceInternal = BaseValueSourceInternal.ImplicitReference;
					entry.Value = obj;
					return;
				}
			}
			FrameworkPropertyMetadata frameworkPropertyMetadata = metadata as FrameworkPropertyMetadata;
			if (frameworkPropertyMetadata != null && frameworkPropertyMetadata.Inherits && (!TreeWalkHelper.SkipNext(this.InheritanceBehavior) || frameworkPropertyMetadata.OverridesInheritanceBehavior))
			{
				FrameworkElement frameworkElement;
				FrameworkContentElement frameworkContentElement;
				bool frameworkParent = FrameworkElement.GetFrameworkParent(this, out frameworkElement, out frameworkContentElement);
				while (frameworkParent)
				{
					InheritanceBehavior inheritanceBehavior;
					bool flag;
					if (frameworkElement != null)
					{
						flag = TreeWalkHelper.IsInheritanceNode(frameworkElement, dp, out inheritanceBehavior);
					}
					else
					{
						flag = TreeWalkHelper.IsInheritanceNode(frameworkContentElement, dp, out inheritanceBehavior);
					}
					if (TreeWalkHelper.SkipNow(inheritanceBehavior))
					{
						break;
					}
					if (flag)
					{
						if (EventTrace.IsEnabled(EventTrace.Keyword.KeywordGeneral, EventTrace.Level.Verbose))
						{
							string text = "[" + base.GetType().Name + "]" + dp.Name;
							EventTrace.EventProvider.TraceEvent(EventTrace.Event.WClientPropParentCheck, EventTrace.Keyword.KeywordGeneral, EventTrace.Level.Verbose, new object[]
							{
								this.GetHashCode(),
								text
							});
						}
						DependencyObject dependencyObject = frameworkElement;
						if (dependencyObject == null)
						{
							dependencyObject = frameworkContentElement;
						}
						EntryIndex entryIndex = dependencyObject.LookupEntry(dp.GlobalIndex);
						entry = dependencyObject.GetValueEntry(entryIndex, dp, frameworkPropertyMetadata, (RequestFlags)12);
						if (entry.Value != DependencyProperty.UnsetValue)
						{
							entry.BaseValueSourceInternal = BaseValueSourceInternal.Inherited;
						}
						return;
					}
					if (TreeWalkHelper.SkipNext(inheritanceBehavior))
					{
						break;
					}
					if (frameworkElement != null)
					{
						frameworkParent = FrameworkElement.GetFrameworkParent(frameworkElement, out frameworkElement, out frameworkContentElement);
					}
					else
					{
						frameworkParent = FrameworkElement.GetFrameworkParent(frameworkContentElement, out frameworkElement, out frameworkContentElement);
					}
				}
			}
		}

		// Token: 0x06000437 RID: 1079 RVA: 0x0000C608 File Offset: 0x0000A808
		private bool GetValueFromTemplatedParent(DependencyProperty dp, ref EffectiveValueEntry entry)
		{
			FrameworkElement frameworkElement = (FrameworkElement)this._templatedParent;
			FrameworkTemplate templateInternal = frameworkElement.TemplateInternal;
			return templateInternal != null && StyleHelper.GetValueFromTemplatedParent(this._templatedParent, this.TemplateChildIndex, new FrameworkObject(null, this), dp, ref templateInternal.ChildRecordFromChildIndex, templateInternal.VisualTree, ref entry);
		}

		// Token: 0x06000438 RID: 1080 RVA: 0x0000C658 File Offset: 0x0000A858
		internal Expression GetExpressionCore(DependencyProperty dp, PropertyMetadata metadata)
		{
			this.IsRequestingExpression = true;
			EffectiveValueEntry effectiveValueEntry = new EffectiveValueEntry(dp);
			effectiveValueEntry.Value = DependencyProperty.UnsetValue;
			this.EvaluateBaseValueCore(dp, metadata, ref effectiveValueEntry);
			this.IsRequestingExpression = false;
			return effectiveValueEntry.Value as Expression;
		}

		/// <summary>Invoked whenever the effective value of any dependency property on this <see cref="T:System.Windows.FrameworkContentElement" /> has been updated. The specific dependency property that changed is reported in the arguments parameter. Overrides <see cref="M:System.Windows.DependencyObject.OnPropertyChanged(System.Windows.DependencyPropertyChangedEventArgs)" />.</summary>
		/// <param name="e">The event data that describes the property that changed, including the old and new values.</param>
		// Token: 0x06000439 RID: 1081 RVA: 0x0000C6A0 File Offset: 0x0000A8A0
		protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
		{
			DependencyProperty property = e.Property;
			VisualDiagnostics.VerifyVisualTreeChange(this);
			base.OnPropertyChanged(e);
			if (e.IsAValueChange || e.IsASubPropertyChange)
			{
				if (property != null && property.OwnerType == typeof(PresentationSource) && property.Name == "RootSource")
				{
					this.TryFireInitialized();
				}
				if (property == FrameworkElement.NameProperty && EventTrace.IsEnabled(EventTrace.Keyword.KeywordGeneral, EventTrace.Level.Verbose))
				{
					EventTrace.EventProvider.TraceEvent(EventTrace.Event.PerfElementIDName, EventTrace.Keyword.KeywordGeneral, EventTrace.Level.Verbose, new object[]
					{
						PerfService.GetPerfElementID(this),
						base.GetType().Name,
						base.GetValue(property)
					});
				}
				if (property != FrameworkContentElement.StyleProperty && property != FrameworkContentElement.DefaultStyleKeyProperty)
				{
					if (this.TemplatedParent != null)
					{
						FrameworkElement frameworkElement = this.TemplatedParent as FrameworkElement;
						FrameworkTemplate templateInternal = frameworkElement.TemplateInternal;
						StyleHelper.OnTriggerSourcePropertyInvalidated(null, templateInternal, this.TemplatedParent, property, e, false, ref templateInternal.TriggerSourceRecordFromChildIndex, ref templateInternal.PropertyTriggersWithActions, this.TemplateChildIndex);
					}
					if (this.Style != null)
					{
						StyleHelper.OnTriggerSourcePropertyInvalidated(this.Style, null, this, property, e, true, ref this.Style.TriggerSourceRecordFromChildIndex, ref this.Style.PropertyTriggersWithActions, 0);
					}
					if (this.ThemeStyle != null && this.Style != this.ThemeStyle)
					{
						StyleHelper.OnTriggerSourcePropertyInvalidated(this.ThemeStyle, null, this, property, e, true, ref this.ThemeStyle.TriggerSourceRecordFromChildIndex, ref this.ThemeStyle.PropertyTriggersWithActions, 0);
					}
				}
			}
			FrameworkPropertyMetadata frameworkPropertyMetadata = e.Metadata as FrameworkPropertyMetadata;
			if (frameworkPropertyMetadata != null && frameworkPropertyMetadata.Inherits && (this.InheritanceBehavior == InheritanceBehavior.Default || frameworkPropertyMetadata.OverridesInheritanceBehavior) && (!DependencyObject.IsTreeWalkOperation(e.OperationType) || this.PotentiallyHasMentees))
			{
				EffectiveValueEntry newEntry = e.NewEntry;
				EffectiveValueEntry oldEntry = e.OldEntry;
				if (oldEntry.BaseValueSourceInternal > newEntry.BaseValueSourceInternal)
				{
					newEntry = new EffectiveValueEntry(property, BaseValueSourceInternal.Inherited);
				}
				else
				{
					newEntry = newEntry.GetFlattenedEntry(RequestFlags.FullyResolved);
					newEntry.BaseValueSourceInternal = BaseValueSourceInternal.Inherited;
				}
				if (oldEntry.BaseValueSourceInternal != BaseValueSourceInternal.Default || oldEntry.HasModifiers)
				{
					oldEntry = oldEntry.GetFlattenedEntry(RequestFlags.FullyResolved);
					oldEntry.BaseValueSourceInternal = BaseValueSourceInternal.Inherited;
				}
				else
				{
					oldEntry = default(EffectiveValueEntry);
				}
				InheritablePropertyChangeInfo info = new InheritablePropertyChangeInfo(this, property, oldEntry, newEntry);
				if (!DependencyObject.IsTreeWalkOperation(e.OperationType))
				{
					TreeWalkHelper.InvalidateOnInheritablePropertyChange(null, this, info, true);
				}
				if (this.PotentiallyHasMentees)
				{
					TreeWalkHelper.OnInheritedPropertyChanged(this, ref info, this.InheritanceBehavior);
				}
			}
		}

		/// <summary>Gets or sets the identifying name of the element. The name provides an instance reference so that programmatic code-behind, such as event handler code, can refer to an element once it is constructed during parsing of XAML. </summary>
		/// <returns>The name of the element.</returns>
		// Token: 0x170000AB RID: 171
		// (get) Token: 0x0600043A RID: 1082 RVA: 0x0000C907 File Offset: 0x0000AB07
		// (set) Token: 0x0600043B RID: 1083 RVA: 0x0000C919 File Offset: 0x0000AB19
		[MergableProperty(false)]
		[Localizability(LocalizationCategory.NeverLocalize)]
		public string Name
		{
			get
			{
				return (string)base.GetValue(FrameworkContentElement.NameProperty);
			}
			set
			{
				base.SetValue(FrameworkContentElement.NameProperty, value);
			}
		}

		/// <summary>Gets or sets an arbitrary object value that can be used to store custom information about this element.  </summary>
		/// <returns>The intended value. This property has no default value.</returns>
		// Token: 0x170000AC RID: 172
		// (get) Token: 0x0600043C RID: 1084 RVA: 0x0000C927 File Offset: 0x0000AB27
		// (set) Token: 0x0600043D RID: 1085 RVA: 0x0000C934 File Offset: 0x0000AB34
		public object Tag
		{
			get
			{
				return base.GetValue(FrameworkContentElement.TagProperty);
			}
			set
			{
				base.SetValue(FrameworkContentElement.TagProperty, value);
			}
		}

		/// <summary>Gets or sets localization/globalization language information that applies to an individual element.</summary>
		/// <returns>The culture information for this element. The default value is an <see cref="T:System.Windows.Markup.XmlLanguage" /> instance with its <see cref="P:System.Windows.Markup.XmlLanguage.IetfLanguageTag" /> value set to the string "en-US".</returns>
		// Token: 0x170000AD RID: 173
		// (get) Token: 0x0600043E RID: 1086 RVA: 0x0000C942 File Offset: 0x0000AB42
		// (set) Token: 0x0600043F RID: 1087 RVA: 0x0000C954 File Offset: 0x0000AB54
		public XmlLanguage Language
		{
			get
			{
				return (XmlLanguage)base.GetValue(FrameworkContentElement.LanguageProperty);
			}
			set
			{
				base.SetValue(FrameworkContentElement.LanguageProperty, value);
			}
		}

		/// <summary>Gets or sets an object that enables customization of appearance, effects, or other style characteristics that will apply to this element when it captures keyboard focus.</summary>
		/// <returns>The desired style to apply on focus. The default value as declared in the dependency property is an empty static <see cref="T:System.Windows.Style" />. However, the effective value at run time is often (but not always) a style as supplied by theme support for controls. </returns>
		// Token: 0x170000AE RID: 174
		// (get) Token: 0x06000440 RID: 1088 RVA: 0x0000C962 File Offset: 0x0000AB62
		// (set) Token: 0x06000441 RID: 1089 RVA: 0x0000C974 File Offset: 0x0000AB74
		public Style FocusVisualStyle
		{
			get
			{
				return (Style)base.GetValue(FrameworkContentElement.FocusVisualStyleProperty);
			}
			set
			{
				base.SetValue(FrameworkContentElement.FocusVisualStyleProperty, value);
			}
		}

		/// <summary>Gets or sets the cursor that displays when the mouse pointer is over this element.</summary>
		/// <returns>The cursor to display. The default value is defined as <see langword="null" /> per this dependency property. However, the practical default at run time will come from a variety of factors.</returns>
		// Token: 0x170000AF RID: 175
		// (get) Token: 0x06000442 RID: 1090 RVA: 0x0000C982 File Offset: 0x0000AB82
		// (set) Token: 0x06000443 RID: 1091 RVA: 0x0000C994 File Offset: 0x0000AB94
		public Cursor Cursor
		{
			get
			{
				return (Cursor)base.GetValue(FrameworkContentElement.CursorProperty);
			}
			set
			{
				base.SetValue(FrameworkContentElement.CursorProperty, value);
			}
		}

		// Token: 0x06000444 RID: 1092 RVA: 0x0000C9A4 File Offset: 0x0000ABA4
		private static void OnCursorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			FrameworkContentElement frameworkContentElement = (FrameworkContentElement)d;
			if (frameworkContentElement.IsMouseOver)
			{
				Mouse.UpdateCursor();
			}
		}

		/// <summary>Gets or sets a value indicating whether this <see cref="T:System.Windows.FrameworkContentElement" /> should force the user interface (UI) to render the cursor as declared by this instance's <see cref="P:System.Windows.FrameworkContentElement.Cursor" /> property.</summary>
		/// <returns>
		///     <see langword="true" /> to force cursor presentation while over this element to use this instance's setting for the cursor (including on all child elements); otherwise <see langword="false" />. The default value is <see langword="false" />.</returns>
		// Token: 0x170000B0 RID: 176
		// (get) Token: 0x06000445 RID: 1093 RVA: 0x0000C9C5 File Offset: 0x0000ABC5
		// (set) Token: 0x06000446 RID: 1094 RVA: 0x0000C9D7 File Offset: 0x0000ABD7
		public bool ForceCursor
		{
			get
			{
				return (bool)base.GetValue(FrameworkContentElement.ForceCursorProperty);
			}
			set
			{
				base.SetValue(FrameworkContentElement.ForceCursorProperty, BooleanBoxes.Box(value));
			}
		}

		// Token: 0x06000447 RID: 1095 RVA: 0x0000C9EC File Offset: 0x0000ABEC
		private static void OnForceCursorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			FrameworkContentElement frameworkContentElement = (FrameworkContentElement)d;
			if (frameworkContentElement.IsMouseOver)
			{
				Mouse.UpdateCursor();
			}
		}

		// Token: 0x06000448 RID: 1096 RVA: 0x0000CA10 File Offset: 0x0000AC10
		private static void OnQueryCursor(object sender, QueryCursorEventArgs e)
		{
			FrameworkContentElement frameworkContentElement = (FrameworkContentElement)sender;
			Cursor cursor = frameworkContentElement.Cursor;
			if (cursor != null && (!e.Handled || frameworkContentElement.ForceCursor))
			{
				e.Cursor = cursor;
				e.Handled = true;
			}
		}

		/// <summary> Moves the keyboard focus from this element to another element. </summary>
		/// <param name="request">The direction that focus is to be moved, as a value of the enumeration.</param>
		/// <returns>Returns <see langword="true" /> if focus is moved successfully; <see langword="false" /> if the target element in direction as specified does not exist.</returns>
		// Token: 0x06000449 RID: 1097 RVA: 0x0000CA4C File Offset: 0x0000AC4C
		public sealed override bool MoveFocus(TraversalRequest request)
		{
			if (request == null)
			{
				throw new ArgumentNullException("request");
			}
			return KeyboardNavigation.Current.Navigate(this, request);
		}

		/// <summary>Determines the next element that would receive focus relative to this element for a provided focus movement direction, but does not actually move the focus. This method is sealed and cannot be overridden.</summary>
		/// <param name="direction">The direction for which a prospective focus change should be determined.</param>
		/// <returns>The next element that focus would move to if focus were actually traversed. May return <see langword="null" /> if focus cannot be moved relative to this element for the provided direction.</returns>
		/// <exception cref="T:System.ComponentModel.InvalidEnumArgumentException">Specified one of the following directions in the <see cref="T:System.Windows.Input.TraversalRequest" />: <see cref="F:System.Windows.Input.FocusNavigationDirection.Next" />, <see cref="F:System.Windows.Input.FocusNavigationDirection.Previous" />, <see cref="F:System.Windows.Input.FocusNavigationDirection.First" />, <see cref="F:System.Windows.Input.FocusNavigationDirection.Last" />. These directions are not legal for <see cref="M:System.Windows.FrameworkContentElement.PredictFocus(System.Windows.Input.FocusNavigationDirection)" /> (but they are legal for <see cref="M:System.Windows.FrameworkContentElement.MoveFocus(System.Windows.Input.TraversalRequest)" />). </exception>
		// Token: 0x0600044A RID: 1098 RVA: 0x0000CA68 File Offset: 0x0000AC68
		public sealed override DependencyObject PredictFocus(FocusNavigationDirection direction)
		{
			return KeyboardNavigation.Current.PredictFocusedElement(this, direction);
		}

		/// <summary>Class handler for the <see cref="E:System.Windows.ContentElement.GotFocus" /> event.</summary>
		/// <param name="e">Event data for the event.</param>
		// Token: 0x0600044B RID: 1099 RVA: 0x0000CA76 File Offset: 0x0000AC76
		protected override void OnGotFocus(RoutedEventArgs e)
		{
			if (base.IsKeyboardFocused)
			{
				this.BringIntoView();
			}
			base.OnGotFocus(e);
		}

		// Token: 0x0600044C RID: 1100 RVA: 0x0000CA90 File Offset: 0x0000AC90
		private static void OnGotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
		{
			if (sender == e.OriginalSource)
			{
				FrameworkContentElement frameworkContentElement = (FrameworkContentElement)sender;
				KeyboardNavigation.UpdateFocusedElement(frameworkContentElement);
				KeyboardNavigation keyboardNavigation = KeyboardNavigation.Current;
				KeyboardNavigation.ShowFocusVisual();
				keyboardNavigation.UpdateActiveElement(frameworkContentElement);
			}
		}

		// Token: 0x0600044D RID: 1101 RVA: 0x0000CAC5 File Offset: 0x0000ACC5
		private static void OnLostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
		{
			if (sender == e.OriginalSource)
			{
				KeyboardNavigation.Current.HideFocusVisual();
				if (e.NewFocus == null)
				{
					KeyboardNavigation.Current.NotifyFocusChanged(sender, e);
				}
			}
		}

		/// <summary>Attempts to bring this element into view, within any scrollable regions it is contained within. </summary>
		// Token: 0x0600044E RID: 1102 RVA: 0x0000CAF0 File Offset: 0x0000ACF0
		public void BringIntoView()
		{
			base.RaiseEvent(new RequestBringIntoViewEventArgs(this, Rect.Empty)
			{
				RoutedEvent = FrameworkElement.RequestBringIntoViewEvent
			});
		}

		/// <summary>Gets or sets the context for input used by this <see cref="T:System.Windows.FrameworkContentElement" />.</summary>
		/// <returns>The input scope, which modifies how input from alternative input methods is interpreted. The default value is <see langword="null" /> (which results in a default handling of commands).</returns>
		// Token: 0x170000B1 RID: 177
		// (get) Token: 0x0600044F RID: 1103 RVA: 0x0000CB1B File Offset: 0x0000AD1B
		// (set) Token: 0x06000450 RID: 1104 RVA: 0x0000CB2D File Offset: 0x0000AD2D
		public InputScope InputScope
		{
			get
			{
				return (InputScope)base.GetValue(FrameworkContentElement.InputScopeProperty);
			}
			set
			{
				base.SetValue(FrameworkContentElement.InputScopeProperty, value);
			}
		}

		/// <summary> Occurs when any associated target property participating in a binding on this element changes. </summary>
		// Token: 0x1400000F RID: 15
		// (add) Token: 0x06000451 RID: 1105 RVA: 0x0000CB3B File Offset: 0x0000AD3B
		// (remove) Token: 0x06000452 RID: 1106 RVA: 0x0000CB49 File Offset: 0x0000AD49
		public event EventHandler<DataTransferEventArgs> TargetUpdated
		{
			add
			{
				base.AddHandler(Binding.TargetUpdatedEvent, value);
			}
			remove
			{
				base.RemoveHandler(Binding.TargetUpdatedEvent, value);
			}
		}

		/// <summary>Occurs when any associated data source participating in a binding on this element changes. </summary>
		// Token: 0x14000010 RID: 16
		// (add) Token: 0x06000453 RID: 1107 RVA: 0x0000CB57 File Offset: 0x0000AD57
		// (remove) Token: 0x06000454 RID: 1108 RVA: 0x0000CB65 File Offset: 0x0000AD65
		public event EventHandler<DataTransferEventArgs> SourceUpdated
		{
			add
			{
				base.AddHandler(Binding.SourceUpdatedEvent, value);
			}
			remove
			{
				base.RemoveHandler(Binding.SourceUpdatedEvent, value);
			}
		}

		/// <summary> Occurs when this element's data context changes. </summary>
		// Token: 0x14000011 RID: 17
		// (add) Token: 0x06000455 RID: 1109 RVA: 0x0000CB73 File Offset: 0x0000AD73
		// (remove) Token: 0x06000456 RID: 1110 RVA: 0x0000CB81 File Offset: 0x0000AD81
		public event DependencyPropertyChangedEventHandler DataContextChanged
		{
			add
			{
				this.EventHandlersStoreAdd(FrameworkElement.DataContextChangedKey, value);
			}
			remove
			{
				this.EventHandlersStoreRemove(FrameworkElement.DataContextChangedKey, value);
			}
		}

		/// <summary>Gets or sets the data context for an element when it participates in data binding. </summary>
		/// <returns>The object to use as data context.</returns>
		// Token: 0x170000B2 RID: 178
		// (get) Token: 0x06000457 RID: 1111 RVA: 0x0000CB8F File Offset: 0x0000AD8F
		// (set) Token: 0x06000458 RID: 1112 RVA: 0x0000CB9C File Offset: 0x0000AD9C
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Localizability(LocalizationCategory.NeverLocalize)]
		public object DataContext
		{
			get
			{
				return base.GetValue(FrameworkContentElement.DataContextProperty);
			}
			set
			{
				base.SetValue(FrameworkContentElement.DataContextProperty, value);
			}
		}

		// Token: 0x06000459 RID: 1113 RVA: 0x0000CBAA File Offset: 0x0000ADAA
		private static void OnDataContextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			if (e.NewValue == BindingExpressionBase.DisconnectedItem)
			{
				return;
			}
			((FrameworkContentElement)d).RaiseDependencyPropertyChanged(FrameworkElement.DataContextChangedKey, e);
		}

		/// <summary> Gets the <see cref="T:System.Windows.Data.BindingExpression" /> for the specified property's binding. </summary>
		/// <param name="dp">The target <see cref="T:System.Windows.DependencyProperty" /> from which to get the binding.</param>
		/// <returns>Returns a <see cref="T:System.Windows.Data.BindingExpression" /> if the target is data bound; otherwise, <see langword="null" />.</returns>
		// Token: 0x0600045A RID: 1114 RVA: 0x0000CBCC File Offset: 0x0000ADCC
		public BindingExpression GetBindingExpression(DependencyProperty dp)
		{
			return BindingOperations.GetBindingExpression(this, dp);
		}

		/// <summary>Attaches a binding to this element, based on the provided binding object. </summary>
		/// <param name="dp">Identifies the bound property.</param>
		/// <param name="binding">Represents a data binding.</param>
		/// <returns>Records the conditions of the binding. This return value can be useful for error checking.</returns>
		// Token: 0x0600045B RID: 1115 RVA: 0x0000CBD5 File Offset: 0x0000ADD5
		public BindingExpressionBase SetBinding(DependencyProperty dp, BindingBase binding)
		{
			return BindingOperations.SetBinding(this, dp, binding);
		}

		/// <summary>Attaches a binding to this element, based on the provided source property name as a path qualification to the data source. </summary>
		/// <param name="dp">Identifies the bound property.</param>
		/// <param name="path">The source property name or the path to the property used for the binding.</param>
		/// <returns>Records the conditions of the binding. This return value can be useful for error checking.</returns>
		// Token: 0x0600045C RID: 1116 RVA: 0x0000CBDF File Offset: 0x0000ADDF
		public BindingExpression SetBinding(DependencyProperty dp, string path)
		{
			return (BindingExpression)this.SetBinding(dp, new Binding(path));
		}

		/// <summary>Gets or sets the <see cref="T:System.Windows.Data.BindingGroup" /> that is used for the element. </summary>
		/// <returns>The <see cref="T:System.Windows.Data.BindingGroup" /> that is used for the element.</returns>
		// Token: 0x170000B3 RID: 179
		// (get) Token: 0x0600045D RID: 1117 RVA: 0x0000CBF3 File Offset: 0x0000ADF3
		// (set) Token: 0x0600045E RID: 1118 RVA: 0x0000CC05 File Offset: 0x0000AE05
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Localizability(LocalizationCategory.NeverLocalize)]
		public BindingGroup BindingGroup
		{
			get
			{
				return (BindingGroup)base.GetValue(FrameworkContentElement.BindingGroupProperty);
			}
			set
			{
				base.SetValue(FrameworkContentElement.BindingGroupProperty, value);
			}
		}

		/// <summary>Returns an alternative logical parent for this element if there is no visual parent. In this case, a <see cref="T:System.Windows.FrameworkContentElement" />  parent is always the same value as the <see cref="P:System.Windows.FrameworkContentElement.Parent" /> property.</summary>
		/// <returns>Returns something other than <see langword="null" /> whenever a WPF framework-level implementation of this method has a non-visual parent connection.</returns>
		// Token: 0x0600045F RID: 1119 RVA: 0x0000CC13 File Offset: 0x0000AE13
		protected internal override DependencyObject GetUIParentCore()
		{
			return this._parent;
		}

		// Token: 0x06000460 RID: 1120 RVA: 0x0000CC1C File Offset: 0x0000AE1C
		internal override object AdjustEventSource(RoutedEventArgs args)
		{
			object result = null;
			if (this._parent != null || this.HasLogicalChildren)
			{
				DependencyObject dependencyObject = args.Source as DependencyObject;
				if (dependencyObject == null || !this.IsLogicalDescendent(dependencyObject))
				{
					args.Source = this;
					result = this;
				}
			}
			return result;
		}

		// Token: 0x06000461 RID: 1121 RVA: 0x00002137 File Offset: 0x00000337
		internal virtual void AdjustBranchSource(RoutedEventArgs args)
		{
		}

		// Token: 0x06000462 RID: 1122 RVA: 0x0000B02A File Offset: 0x0000922A
		internal virtual bool IgnoreModelParentBuildRoute(RoutedEventArgs args)
		{
			return false;
		}

		// Token: 0x06000463 RID: 1123 RVA: 0x0000CC60 File Offset: 0x0000AE60
		internal sealed override bool BuildRouteCore(EventRoute route, RoutedEventArgs args)
		{
			bool result = false;
			DependencyObject parent = ContentOperations.GetParent(this);
			DependencyObject parent2 = this._parent;
			DependencyObject dependencyObject = route.PeekBranchNode() as DependencyObject;
			if (dependencyObject != null && this.IsLogicalDescendent(dependencyObject))
			{
				args.Source = route.PeekBranchSource();
				this.AdjustBranchSource(args);
				route.AddSource(args.Source);
				route.PopBranchNode();
				FrameworkElement.AddIntermediateElementsToRoute(this, route, args, LogicalTreeHelper.GetParent(dependencyObject));
			}
			if (!this.IgnoreModelParentBuildRoute(args))
			{
				if (parent == null)
				{
					result = (parent2 != null);
				}
				else if (parent2 != null)
				{
					route.PushBranchNode(this, args.Source);
					args.Source = parent;
				}
			}
			return result;
		}

		// Token: 0x06000464 RID: 1124 RVA: 0x0000CCF4 File Offset: 0x0000AEF4
		internal override void AddToEventRouteCore(EventRoute route, RoutedEventArgs args)
		{
			FrameworkElement.AddStyleHandlersToEventRoute(null, this, route, args);
		}

		// Token: 0x06000465 RID: 1125 RVA: 0x0000CCFF File Offset: 0x0000AEFF
		private bool IsLogicalDescendent(DependencyObject child)
		{
			while (child != null)
			{
				if (child == this)
				{
					return true;
				}
				child = LogicalTreeHelper.GetParent(child);
			}
			return false;
		}

		// Token: 0x06000466 RID: 1126 RVA: 0x0000CD18 File Offset: 0x0000AF18
		internal override bool InvalidateAutomationAncestorsCore(Stack<DependencyObject> branchNodeStack, out bool continuePastCoreTree)
		{
			bool result = true;
			continuePastCoreTree = false;
			DependencyObject parent = ContentOperations.GetParent(this);
			DependencyObject parent2 = this._parent;
			DependencyObject dependencyObject = (branchNodeStack.Count > 0) ? branchNodeStack.Peek() : null;
			if (dependencyObject != null && this.IsLogicalDescendent(dependencyObject))
			{
				branchNodeStack.Pop();
				result = FrameworkElement.InvalidateAutomationIntermediateElements(this, LogicalTreeHelper.GetParent(dependencyObject));
			}
			if (parent == null)
			{
				continuePastCoreTree = (parent2 != null);
			}
			else if (parent2 != null)
			{
				branchNodeStack.Push(this);
			}
			return result;
		}

		// Token: 0x06000467 RID: 1127 RVA: 0x00002137 File Offset: 0x00000337
		internal virtual void OnAncestorChanged()
		{
		}

		// Token: 0x06000468 RID: 1128 RVA: 0x0000CD84 File Offset: 0x0000AF84
		internal override void OnContentParentChanged(DependencyObject oldParent)
		{
			DependencyObject parent = ContentOperations.GetParent(this);
			this.TryFireInitialized();
			base.OnContentParentChanged(oldParent);
		}

		// Token: 0x170000B4 RID: 180
		// (get) Token: 0x06000469 RID: 1129 RVA: 0x0000CDA8 File Offset: 0x0000AFA8
		// (set) Token: 0x0600046A RID: 1130 RVA: 0x0000CDC4 File Offset: 0x0000AFC4
		internal InheritanceBehavior InheritanceBehavior
		{
			get
			{
				return (InheritanceBehavior)((this._flags & (InternalFlags)56U) >> 3);
			}
			set
			{
				if (this.IsInitialized)
				{
					throw new InvalidOperationException(SR.Get("Illegal_InheritanceBehaviorSettor"));
				}
				if (value < InheritanceBehavior.Default || value > InheritanceBehavior.SkipAllNext)
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(InheritanceBehavior));
				}
				uint num = (uint)((uint)value << 3);
				this._flags = (InternalFlags)((num & 56U) | (uint)(this._flags & (InternalFlags)4294967239U));
				if (this._parent != null)
				{
					TreeWalkHelper.InvalidateOnTreeChange(null, this, this._parent, true);
					return;
				}
			}
		}

		/// <summary>Called before an element is initialized. </summary>
		// Token: 0x0600046B RID: 1131 RVA: 0x0000CE36 File Offset: 0x0000B036
		public virtual void BeginInit()
		{
			if (this.ReadInternalFlag(InternalFlags.InitPending))
			{
				throw new InvalidOperationException(SR.Get("NestedBeginInitNotSupported"));
			}
			this.WriteInternalFlag(InternalFlags.InitPending, true);
		}

		/// <summary> Called immediately after an element is initialized. </summary>
		// Token: 0x0600046C RID: 1132 RVA: 0x0000CE61 File Offset: 0x0000B061
		public virtual void EndInit()
		{
			if (!this.ReadInternalFlag(InternalFlags.InitPending))
			{
				throw new InvalidOperationException(SR.Get("EndInitWithoutBeginInitNotSupported"));
			}
			this.WriteInternalFlag(InternalFlags.InitPending, false);
			this.TryFireInitialized();
		}

		/// <summary>Gets a value indicating whether this element has been initialized, either by being loaded as Extensible Application Markup Language (XAML), or by explicitly having its <see cref="M:System.Windows.FrameworkContentElement.EndInit" /> method called. </summary>
		/// <returns>
		///     <see langword="true" /> if the element is initialized per the aforementioned loading or method calls; otherwise, <see langword="false" />.</returns>
		// Token: 0x170000B5 RID: 181
		// (get) Token: 0x0600046D RID: 1133 RVA: 0x0000CE92 File Offset: 0x0000B092
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public bool IsInitialized
		{
			get
			{
				return this.ReadInternalFlag(InternalFlags.IsInitialized);
			}
		}

		/// <summary> Occurs when this <see cref="T:System.Windows.FrameworkContentElement" /> is initialized. This coincides with cases where the value of the <see cref="P:System.Windows.FrameworkContentElement.IsInitialized" /> property changes from <see langword="false" /> (or undefined) to <see langword="true" />. </summary>
		// Token: 0x14000012 RID: 18
		// (add) Token: 0x0600046E RID: 1134 RVA: 0x0000CE9F File Offset: 0x0000B09F
		// (remove) Token: 0x0600046F RID: 1135 RVA: 0x0000CEAD File Offset: 0x0000B0AD
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public event EventHandler Initialized
		{
			add
			{
				this.EventHandlersStoreAdd(FrameworkElement.InitializedKey, value);
			}
			remove
			{
				this.EventHandlersStoreRemove(FrameworkElement.InitializedKey, value);
			}
		}

		/// <summary> Raises the <see cref="E:System.Windows.FrameworkContentElement.Initialized" /> event. This method is invoked whenever <see cref="P:System.Windows.FrameworkContentElement.IsInitialized" /> is set to <see langword="true" />. </summary>
		/// <param name="e">Event data for the event.</param>
		// Token: 0x06000470 RID: 1136 RVA: 0x0000CEBB File Offset: 0x0000B0BB
		protected virtual void OnInitialized(EventArgs e)
		{
			if (!this.HasStyleEverBeenFetched)
			{
				this.UpdateStyleProperty();
			}
			if (!this.HasThemeStyleEverBeenFetched)
			{
				this.UpdateThemeStyleProperty();
			}
			this.RaiseInitialized(FrameworkElement.InitializedKey, e);
		}

		// Token: 0x06000471 RID: 1137 RVA: 0x0000CEE5 File Offset: 0x0000B0E5
		private void TryFireInitialized()
		{
			if (!this.ReadInternalFlag(InternalFlags.InitPending) && !this.ReadInternalFlag(InternalFlags.IsInitialized))
			{
				this.WriteInternalFlag(InternalFlags.IsInitialized, true);
				this.OnInitialized(EventArgs.Empty);
			}
		}

		// Token: 0x06000472 RID: 1138 RVA: 0x0000CF18 File Offset: 0x0000B118
		private void RaiseInitialized(EventPrivateKey key, EventArgs e)
		{
			EventHandlersStore eventHandlersStore = base.EventHandlersStore;
			if (eventHandlersStore != null)
			{
				Delegate @delegate = eventHandlersStore.Get(key);
				if (@delegate != null)
				{
					((EventHandler)@delegate)(this, e);
				}
			}
		}

		/// <summary>Gets a value indicating whether this element has been loaded for presentation. </summary>
		/// <returns>
		///     <see langword="true" /> if the current element is attached to an element tree and has been rendered; <see langword="false" /> if the element has never been attached to a loaded element tree. </returns>
		// Token: 0x170000B6 RID: 182
		// (get) Token: 0x06000473 RID: 1139 RVA: 0x0000CF48 File Offset: 0x0000B148
		public bool IsLoaded
		{
			get
			{
				object[] loadedPending = this.LoadedPending;
				object[] unloadedPending = this.UnloadedPending;
				if (loadedPending != null || unloadedPending != null)
				{
					return unloadedPending != null;
				}
				if (this.SubtreeHasLoadedChangeHandler)
				{
					return this.IsLoadedCache;
				}
				return BroadcastEventHelper.IsParentLoaded(this);
			}
		}

		/// <summary> Occurs when the element is laid out, rendered, and ready for interaction.</summary>
		// Token: 0x14000013 RID: 19
		// (add) Token: 0x06000474 RID: 1140 RVA: 0x0000CF83 File Offset: 0x0000B183
		// (remove) Token: 0x06000475 RID: 1141 RVA: 0x0000CF92 File Offset: 0x0000B192
		public event RoutedEventHandler Loaded
		{
			add
			{
				base.AddHandler(FrameworkElement.LoadedEvent, value, false);
			}
			remove
			{
				base.RemoveHandler(FrameworkElement.LoadedEvent, value);
			}
		}

		// Token: 0x06000476 RID: 1142 RVA: 0x0000CFA0 File Offset: 0x0000B1A0
		internal void OnLoaded(RoutedEventArgs args)
		{
			base.RaiseEvent(args);
		}

		/// <summary>Occurs when the element is removed from an element tree of loaded elements.</summary>
		// Token: 0x14000014 RID: 20
		// (add) Token: 0x06000477 RID: 1143 RVA: 0x0000CFA9 File Offset: 0x0000B1A9
		// (remove) Token: 0x06000478 RID: 1144 RVA: 0x0000CFB8 File Offset: 0x0000B1B8
		public event RoutedEventHandler Unloaded
		{
			add
			{
				base.AddHandler(FrameworkElement.UnloadedEvent, value, false);
			}
			remove
			{
				base.RemoveHandler(FrameworkElement.UnloadedEvent, value);
			}
		}

		// Token: 0x06000479 RID: 1145 RVA: 0x0000CFA0 File Offset: 0x0000B1A0
		internal void OnUnloaded(RoutedEventArgs args)
		{
			base.RaiseEvent(args);
		}

		// Token: 0x0600047A RID: 1146 RVA: 0x0000CFC6 File Offset: 0x0000B1C6
		internal override void OnAddHandler(RoutedEvent routedEvent, Delegate handler)
		{
			if (routedEvent == FrameworkContentElement.LoadedEvent || routedEvent == FrameworkContentElement.UnloadedEvent)
			{
				BroadcastEventHelper.AddHasLoadedChangeHandlerFlagInAncestry(this);
			}
		}

		// Token: 0x0600047B RID: 1147 RVA: 0x0000CFDE File Offset: 0x0000B1DE
		internal override void OnRemoveHandler(RoutedEvent routedEvent, Delegate handler)
		{
			if (routedEvent != FrameworkContentElement.LoadedEvent && routedEvent != FrameworkContentElement.UnloadedEvent)
			{
				return;
			}
			if (!this.ThisHasLoadedChangeEventHandler)
			{
				BroadcastEventHelper.RemoveHasLoadedChangeHandlerFlagInAncestry(this);
			}
		}

		// Token: 0x0600047C RID: 1148 RVA: 0x0000D000 File Offset: 0x0000B200
		internal void RaiseClrEvent(EventPrivateKey key, EventArgs args)
		{
			EventHandlersStore eventHandlersStore = base.EventHandlersStore;
			if (eventHandlersStore != null)
			{
				Delegate @delegate = eventHandlersStore.Get(key);
				if (@delegate != null)
				{
					((EventHandler)@delegate)(this, args);
				}
			}
		}

		/// <summary>Gets or sets the tool-tip object that is displayed for this element in the user interface (UI). </summary>
		/// <returns>The tooltip object. See Remarks below for details on why this parameter is not strongly typed.</returns>
		// Token: 0x170000B7 RID: 183
		// (get) Token: 0x0600047D RID: 1149 RVA: 0x0000D02F File Offset: 0x0000B22F
		// (set) Token: 0x0600047E RID: 1150 RVA: 0x0000D037 File Offset: 0x0000B237
		[Bindable(true)]
		[Category("Appearance")]
		public object ToolTip
		{
			get
			{
				return ToolTipService.GetToolTip(this);
			}
			set
			{
				ToolTipService.SetToolTip(this, value);
			}
		}

		/// <summary>Gets or sets the context menu element that should appear whenever the context menu is requested via user interface (UI) from within this element. </summary>
		/// <returns>The context menu that this element uses. </returns>
		// Token: 0x170000B8 RID: 184
		// (get) Token: 0x0600047F RID: 1151 RVA: 0x0000D040 File Offset: 0x0000B240
		// (set) Token: 0x06000480 RID: 1152 RVA: 0x0000D052 File Offset: 0x0000B252
		public ContextMenu ContextMenu
		{
			get
			{
				return (ContextMenu)base.GetValue(FrameworkContentElement.ContextMenuProperty);
			}
			set
			{
				base.SetValue(FrameworkContentElement.ContextMenuProperty, value);
			}
		}

		/// <summary> Occurs when any tooltip on the element is opened. </summary>
		// Token: 0x14000015 RID: 21
		// (add) Token: 0x06000481 RID: 1153 RVA: 0x0000D060 File Offset: 0x0000B260
		// (remove) Token: 0x06000482 RID: 1154 RVA: 0x0000D06E File Offset: 0x0000B26E
		public event ToolTipEventHandler ToolTipOpening
		{
			add
			{
				base.AddHandler(FrameworkContentElement.ToolTipOpeningEvent, value);
			}
			remove
			{
				base.RemoveHandler(FrameworkContentElement.ToolTipOpeningEvent, value);
			}
		}

		// Token: 0x06000483 RID: 1155 RVA: 0x0000D07C File Offset: 0x0000B27C
		private static void OnToolTipOpeningThunk(object sender, ToolTipEventArgs e)
		{
			((FrameworkContentElement)sender).OnToolTipOpening(e);
		}

		/// <summary> Invoked whenever the <see cref="E:System.Windows.FrameworkContentElement.ToolTipOpening" /> routed event reaches this class in its route. Implement this method to add class handling for this event. </summary>
		/// <param name="e">Provides data about the event.</param>
		// Token: 0x06000484 RID: 1156 RVA: 0x00002137 File Offset: 0x00000337
		protected virtual void OnToolTipOpening(ToolTipEventArgs e)
		{
		}

		/// <summary> Occurs just before any tooltip on the element is closed. </summary>
		// Token: 0x14000016 RID: 22
		// (add) Token: 0x06000485 RID: 1157 RVA: 0x0000D08A File Offset: 0x0000B28A
		// (remove) Token: 0x06000486 RID: 1158 RVA: 0x0000D098 File Offset: 0x0000B298
		public event ToolTipEventHandler ToolTipClosing
		{
			add
			{
				base.AddHandler(FrameworkContentElement.ToolTipClosingEvent, value);
			}
			remove
			{
				base.RemoveHandler(FrameworkContentElement.ToolTipClosingEvent, value);
			}
		}

		// Token: 0x06000487 RID: 1159 RVA: 0x0000D0A6 File Offset: 0x0000B2A6
		private static void OnToolTipClosingThunk(object sender, ToolTipEventArgs e)
		{
			((FrameworkContentElement)sender).OnToolTipClosing(e);
		}

		/// <summary> Invoked whenever the <see cref="E:System.Windows.FrameworkContentElement.ToolTipClosing" /> routed event reaches this class in its route. Implement this method to add class handling for this event. </summary>
		/// <param name="e">Provides data about the event.</param>
		// Token: 0x06000488 RID: 1160 RVA: 0x00002137 File Offset: 0x00000337
		protected virtual void OnToolTipClosing(ToolTipEventArgs e)
		{
		}

		/// <summary> Occurs when any context menu on the element is opened. </summary>
		// Token: 0x14000017 RID: 23
		// (add) Token: 0x06000489 RID: 1161 RVA: 0x0000D0B4 File Offset: 0x0000B2B4
		// (remove) Token: 0x0600048A RID: 1162 RVA: 0x0000D0C2 File Offset: 0x0000B2C2
		public event ContextMenuEventHandler ContextMenuOpening
		{
			add
			{
				base.AddHandler(FrameworkContentElement.ContextMenuOpeningEvent, value);
			}
			remove
			{
				base.RemoveHandler(FrameworkContentElement.ContextMenuOpeningEvent, value);
			}
		}

		// Token: 0x0600048B RID: 1163 RVA: 0x0000D0D0 File Offset: 0x0000B2D0
		private static void OnContextMenuOpeningThunk(object sender, ContextMenuEventArgs e)
		{
			((FrameworkContentElement)sender).OnContextMenuOpening(e);
		}

		/// <summary> Invoked whenever the <see cref="E:System.Windows.FrameworkContentElement.ContextMenuOpening" /> routed event reaches this class in its route. Implement this method to add class handling for this event. </summary>
		/// <param name="e">Event data for the event.</param>
		// Token: 0x0600048C RID: 1164 RVA: 0x00002137 File Offset: 0x00000337
		protected virtual void OnContextMenuOpening(ContextMenuEventArgs e)
		{
		}

		/// <summary>Occurs just before any context menu on the element is closed. </summary>
		// Token: 0x14000018 RID: 24
		// (add) Token: 0x0600048D RID: 1165 RVA: 0x0000D0DE File Offset: 0x0000B2DE
		// (remove) Token: 0x0600048E RID: 1166 RVA: 0x0000D0EC File Offset: 0x0000B2EC
		public event ContextMenuEventHandler ContextMenuClosing
		{
			add
			{
				base.AddHandler(FrameworkContentElement.ContextMenuClosingEvent, value);
			}
			remove
			{
				base.RemoveHandler(FrameworkContentElement.ContextMenuClosingEvent, value);
			}
		}

		// Token: 0x0600048F RID: 1167 RVA: 0x0000D0FA File Offset: 0x0000B2FA
		private static void OnContextMenuClosingThunk(object sender, ContextMenuEventArgs e)
		{
			((FrameworkContentElement)sender).OnContextMenuClosing(e);
		}

		/// <summary> Invoked whenever the <see cref="E:System.Windows.FrameworkContentElement.ContextMenuClosing" /> routed event reaches this class in its route. Implement this method to add class handling for this event. </summary>
		/// <param name="e">Provides data about the event.</param>
		// Token: 0x06000490 RID: 1168 RVA: 0x00002137 File Offset: 0x00000337
		protected virtual void OnContextMenuClosing(ContextMenuEventArgs e)
		{
		}

		// Token: 0x06000491 RID: 1169 RVA: 0x0000D108 File Offset: 0x0000B308
		internal override void InvalidateForceInheritPropertyOnChildren(DependencyProperty property)
		{
			IEnumerator logicalChildren = this.LogicalChildren;
			if (logicalChildren != null)
			{
				while (logicalChildren.MoveNext())
				{
					object obj = logicalChildren.Current;
					DependencyObject dependencyObject = obj as DependencyObject;
					if (dependencyObject != null)
					{
						dependencyObject.CoerceValue(property);
					}
				}
			}
		}

		// Token: 0x06000492 RID: 1170 RVA: 0x0000D140 File Offset: 0x0000B340
		private void RaiseDependencyPropertyChanged(EventPrivateKey key, DependencyPropertyChangedEventArgs args)
		{
			EventHandlersStore eventHandlersStore = base.EventHandlersStore;
			if (eventHandlersStore != null)
			{
				Delegate @delegate = eventHandlersStore.Get(key);
				if (@delegate != null)
				{
					((DependencyPropertyChangedEventHandler)@delegate)(this, args);
				}
			}
		}

		// Token: 0x06000493 RID: 1171 RVA: 0x0000D16F File Offset: 0x0000B36F
		private void EventHandlersStoreAdd(EventPrivateKey key, Delegate handler)
		{
			base.EnsureEventHandlersStore();
			base.EventHandlersStore.Add(key, handler);
		}

		// Token: 0x06000494 RID: 1172 RVA: 0x0000D184 File Offset: 0x0000B384
		private void EventHandlersStoreRemove(EventPrivateKey key, Delegate handler)
		{
			EventHandlersStore eventHandlersStore = base.EventHandlersStore;
			if (eventHandlersStore != null)
			{
				eventHandlersStore.Remove(key, handler);
			}
		}

		// Token: 0x170000B9 RID: 185
		// (get) Token: 0x06000495 RID: 1173 RVA: 0x0000D1A3 File Offset: 0x0000B3A3
		// (set) Token: 0x06000496 RID: 1174 RVA: 0x0000D1AC File Offset: 0x0000B3AC
		internal bool HasResourceReference
		{
			get
			{
				return this.ReadInternalFlag(InternalFlags.HasResourceReferences);
			}
			set
			{
				this.WriteInternalFlag(InternalFlags.HasResourceReferences, value);
			}
		}

		// Token: 0x170000BA RID: 186
		// (get) Token: 0x06000497 RID: 1175 RVA: 0x0000D1B6 File Offset: 0x0000B3B6
		// (set) Token: 0x06000498 RID: 1176 RVA: 0x0000D1C3 File Offset: 0x0000B3C3
		internal bool IsLogicalChildrenIterationInProgress
		{
			get
			{
				return this.ReadInternalFlag(InternalFlags.IsLogicalChildrenIterationInProgress);
			}
			set
			{
				this.WriteInternalFlag(InternalFlags.IsLogicalChildrenIterationInProgress, value);
			}
		}

		// Token: 0x170000BB RID: 187
		// (get) Token: 0x06000499 RID: 1177 RVA: 0x0000D1D1 File Offset: 0x0000B3D1
		// (set) Token: 0x0600049A RID: 1178 RVA: 0x0000D1DE File Offset: 0x0000B3DE
		internal bool SubtreeHasLoadedChangeHandler
		{
			get
			{
				return this.ReadInternalFlag2(InternalFlags2.TreeHasLoadedChangeHandler);
			}
			set
			{
				this.WriteInternalFlag2(InternalFlags2.TreeHasLoadedChangeHandler, value);
			}
		}

		// Token: 0x170000BC RID: 188
		// (get) Token: 0x0600049B RID: 1179 RVA: 0x0000D1EC File Offset: 0x0000B3EC
		// (set) Token: 0x0600049C RID: 1180 RVA: 0x0000D1F9 File Offset: 0x0000B3F9
		internal bool IsLoadedCache
		{
			get
			{
				return this.ReadInternalFlag2(InternalFlags2.IsLoadedCache);
			}
			set
			{
				this.WriteInternalFlag2(InternalFlags2.IsLoadedCache, value);
			}
		}

		// Token: 0x170000BD RID: 189
		// (get) Token: 0x0600049D RID: 1181 RVA: 0x0000D207 File Offset: 0x0000B407
		// (set) Token: 0x0600049E RID: 1182 RVA: 0x0000D214 File Offset: 0x0000B414
		internal bool IsParentAnFE
		{
			get
			{
				return this.ReadInternalFlag2(InternalFlags2.IsParentAnFE);
			}
			set
			{
				this.WriteInternalFlag2(InternalFlags2.IsParentAnFE, value);
			}
		}

		// Token: 0x170000BE RID: 190
		// (get) Token: 0x0600049F RID: 1183 RVA: 0x0000D222 File Offset: 0x0000B422
		// (set) Token: 0x060004A0 RID: 1184 RVA: 0x0000D22F File Offset: 0x0000B42F
		internal bool IsTemplatedParentAnFE
		{
			get
			{
				return this.ReadInternalFlag2(InternalFlags2.IsTemplatedParentAnFE);
			}
			set
			{
				this.WriteInternalFlag2(InternalFlags2.IsTemplatedParentAnFE, value);
			}
		}

		// Token: 0x170000BF RID: 191
		// (get) Token: 0x060004A1 RID: 1185 RVA: 0x0000D23D File Offset: 0x0000B43D
		// (set) Token: 0x060004A2 RID: 1186 RVA: 0x0000D24A File Offset: 0x0000B44A
		internal bool HasLogicalChildren
		{
			get
			{
				return this.ReadInternalFlag(InternalFlags.HasLogicalChildren);
			}
			set
			{
				this.WriteInternalFlag(InternalFlags.HasLogicalChildren, value);
			}
		}

		// Token: 0x170000C0 RID: 192
		// (get) Token: 0x060004A3 RID: 1187 RVA: 0x0000D258 File Offset: 0x0000B458
		// (set) Token: 0x060004A4 RID: 1188 RVA: 0x0000D280 File Offset: 0x0000B480
		internal int TemplateChildIndex
		{
			get
			{
				uint num = (uint)(this._flags2 & InternalFlags2.Default);
				if (num == 65535U)
				{
					return -1;
				}
				return (int)num;
			}
			set
			{
				if (value < -1 || value >= 65535)
				{
					throw new ArgumentOutOfRangeException("value", SR.Get("TemplateChildIndexOutOfRange"));
				}
				uint num = (uint)((value == -1) ? 65535 : value);
				this._flags2 = (InternalFlags2)(num | (uint)(this._flags2 & ~(InternalFlags2.R0 | InternalFlags2.R1 | InternalFlags2.R2 | InternalFlags2.R3 | InternalFlags2.R4 | InternalFlags2.R5 | InternalFlags2.R6 | InternalFlags2.R7 | InternalFlags2.R8 | InternalFlags2.R9 | InternalFlags2.RA | InternalFlags2.RB | InternalFlags2.RC | InternalFlags2.RD | InternalFlags2.RE | InternalFlags2.RF)));
			}
		}

		// Token: 0x170000C1 RID: 193
		// (get) Token: 0x060004A5 RID: 1189 RVA: 0x0000D2CF File Offset: 0x0000B4CF
		// (set) Token: 0x060004A6 RID: 1190 RVA: 0x0000D2DC File Offset: 0x0000B4DC
		internal bool IsRequestingExpression
		{
			get
			{
				return this.ReadInternalFlag2(InternalFlags2.IsRequestingExpression);
			}
			set
			{
				this.WriteInternalFlag2(InternalFlags2.IsRequestingExpression, value);
			}
		}

		// Token: 0x060004A7 RID: 1191 RVA: 0x0000D2EA File Offset: 0x0000B4EA
		internal bool ReadInternalFlag(InternalFlags reqFlag)
		{
			return (this._flags & reqFlag) > (InternalFlags)0U;
		}

		// Token: 0x060004A8 RID: 1192 RVA: 0x0000D2F7 File Offset: 0x0000B4F7
		internal bool ReadInternalFlag2(InternalFlags2 reqFlag)
		{
			return (this._flags2 & reqFlag) > (InternalFlags2)0U;
		}

		// Token: 0x060004A9 RID: 1193 RVA: 0x0000D304 File Offset: 0x0000B504
		internal void WriteInternalFlag(InternalFlags reqFlag, bool set)
		{
			if (set)
			{
				this._flags |= reqFlag;
				return;
			}
			this._flags &= ~reqFlag;
		}

		// Token: 0x060004AA RID: 1194 RVA: 0x0000D327 File Offset: 0x0000B527
		internal void WriteInternalFlag2(InternalFlags2 reqFlag, bool set)
		{
			if (set)
			{
				this._flags2 |= reqFlag;
				return;
			}
			this._flags2 &= ~reqFlag;
		}

		/// <summary>Gets the parent in the logical tree for this element. </summary>
		/// <returns>The logical parent for this element.</returns>
		// Token: 0x170000C2 RID: 194
		// (get) Token: 0x060004AB RID: 1195 RVA: 0x0000D34A File Offset: 0x0000B54A
		public new DependencyObject Parent
		{
			get
			{
				return this.ContextVerifiedGetParent();
			}
		}

		/// <summary>Provides an accessor that simplifies access to the <see cref="T:System.Windows.NameScope" /> registration method.</summary>
		/// <param name="name">Name to use for the specified name-object mapping.</param>
		/// <param name="scopedElement">Object for the mapping.</param>
		// Token: 0x060004AC RID: 1196 RVA: 0x0000D354 File Offset: 0x0000B554
		public void RegisterName(string name, object scopedElement)
		{
			INameScope nameScope = FrameworkElement.FindScope(this);
			if (nameScope != null)
			{
				nameScope.RegisterName(name, scopedElement);
				return;
			}
			throw new InvalidOperationException(SR.Get("NameScopeNotFound", new object[]
			{
				name,
				"register"
			}));
		}

		/// <summary>Simplifies access to the <see cref="T:System.Windows.NameScope" /> de-registration method.</summary>
		/// <param name="name">Name of the name-object pair to remove from the current scope.</param>
		// Token: 0x060004AD RID: 1197 RVA: 0x0000D398 File Offset: 0x0000B598
		public void UnregisterName(string name)
		{
			INameScope nameScope = FrameworkElement.FindScope(this);
			if (nameScope != null)
			{
				nameScope.UnregisterName(name);
				return;
			}
			throw new InvalidOperationException(SR.Get("NameScopeNotFound", new object[]
			{
				name,
				"unregister"
			}));
		}

		/// <summary> Finds an element that has the provided identifier name. </summary>
		/// <param name="name">Name of the element to search for.</param>
		/// <returns>The requested element. May be <see langword="null" /> if no matching element was found.</returns>
		// Token: 0x060004AE RID: 1198 RVA: 0x0000D3D8 File Offset: 0x0000B5D8
		public object FindName(string name)
		{
			DependencyObject dependencyObject;
			return this.FindName(name, out dependencyObject);
		}

		// Token: 0x060004AF RID: 1199 RVA: 0x0000D3F0 File Offset: 0x0000B5F0
		internal object FindName(string name, out DependencyObject scopeOwner)
		{
			INameScope nameScope = FrameworkElement.FindScope(this, out scopeOwner);
			if (nameScope != null)
			{
				return nameScope.FindName(name);
			}
			return null;
		}

		/// <summary>Reapplies the default style to the current <see cref="T:System.Windows.FrameworkContentElement" />.</summary>
		// Token: 0x060004B0 RID: 1200 RVA: 0x0000D411 File Offset: 0x0000B611
		public void UpdateDefaultStyle()
		{
			TreeWalkHelper.InvalidateOnResourcesChange(null, this, ResourcesChangeInfo.ThemeChangeInfo);
		}

		/// <summary>Gets an enumerator for the logical child elements of this element. </summary>
		/// <returns>An enumerator for logical child elements of this element.</returns>
		// Token: 0x170000C3 RID: 195
		// (get) Token: 0x060004B1 RID: 1201 RVA: 0x0000C238 File Offset: 0x0000A438
		protected internal virtual IEnumerator LogicalChildren
		{
			get
			{
				return null;
			}
		}

		// Token: 0x060004B2 RID: 1202 RVA: 0x0000D420 File Offset: 0x0000B620
		internal object FindResourceOnSelf(object resourceKey, bool allowDeferredResourceReference, bool mustReturnDeferredResourceReference)
		{
			ResourceDictionary value = FrameworkContentElement.ResourcesField.GetValue(this);
			if (value != null && value.Contains(resourceKey))
			{
				bool flag;
				return value.FetchResource(resourceKey, allowDeferredResourceReference, mustReturnDeferredResourceReference, out flag);
			}
			return DependencyProperty.UnsetValue;
		}

		// Token: 0x060004B3 RID: 1203 RVA: 0x0000CC13 File Offset: 0x0000AE13
		internal DependencyObject ContextVerifiedGetParent()
		{
			return this._parent;
		}

		/// <summary>Adds the provided element as a child of this element. </summary>
		/// <param name="child">The child element to be added.</param>
		// Token: 0x060004B4 RID: 1204 RVA: 0x0000D458 File Offset: 0x0000B658
		protected internal void AddLogicalChild(object child)
		{
			if (child != null)
			{
				if (this.IsLogicalChildrenIterationInProgress)
				{
					throw new InvalidOperationException(SR.Get("CannotModifyLogicalChildrenDuringTreeWalk"));
				}
				this.TryFireInitialized();
				try
				{
					this.HasLogicalChildren = true;
					FrameworkObject frameworkObject = new FrameworkObject(child as DependencyObject);
					frameworkObject.ChangeLogicalParent(this);
				}
				finally
				{
				}
			}
		}

		/// <summary>Removes the specified element from the logical tree for this element. </summary>
		/// <param name="child">The element to remove.</param>
		// Token: 0x060004B5 RID: 1205 RVA: 0x0000D4BC File Offset: 0x0000B6BC
		protected internal void RemoveLogicalChild(object child)
		{
			if (child != null)
			{
				if (this.IsLogicalChildrenIterationInProgress)
				{
					throw new InvalidOperationException(SR.Get("CannotModifyLogicalChildrenDuringTreeWalk"));
				}
				FrameworkObject frameworkObject = new FrameworkObject(child as DependencyObject);
				if (frameworkObject.Parent == this)
				{
					frameworkObject.ChangeLogicalParent(null);
				}
				IEnumerator logicalChildren = this.LogicalChildren;
				if (logicalChildren == null)
				{
					this.HasLogicalChildren = false;
					return;
				}
				this.HasLogicalChildren = logicalChildren.MoveNext();
			}
		}

		// Token: 0x060004B6 RID: 1206 RVA: 0x0000D524 File Offset: 0x0000B724
		internal void ChangeLogicalParent(DependencyObject newParent)
		{
			base.VerifyAccess();
			if (newParent != null)
			{
				newParent.VerifyAccess();
			}
			if (this._parent != null && newParent != null && this._parent != newParent)
			{
				throw new InvalidOperationException(SR.Get("HasLogicalParent"));
			}
			if (newParent == this)
			{
				throw new InvalidOperationException(SR.Get("CannotBeSelfParent"));
			}
			VisualDiagnostics.VerifyVisualTreeChange(this);
			if (newParent != null)
			{
				this.ClearInheritanceContext();
			}
			this.IsParentAnFE = (newParent is FrameworkElement);
			DependencyObject parent = this._parent;
			this.OnNewParent(newParent);
			BroadcastEventHelper.AddOrRemoveHasLoadedChangeHandlerFlag(this, parent, newParent);
			BroadcastEventHelper.BroadcastLoadedOrUnloadedEvent(this, parent, newParent);
			DependencyObject parent2 = (newParent != null) ? newParent : parent;
			TreeWalkHelper.InvalidateOnTreeChange(null, this, parent2, newParent != null);
			this.TryFireInitialized();
		}

		// Token: 0x060004B7 RID: 1207 RVA: 0x0000D5D0 File Offset: 0x0000B7D0
		internal virtual void OnNewParent(DependencyObject newParent)
		{
			DependencyObject parent = this._parent;
			this._parent = newParent;
			if (this._parent != null)
			{
				UIElement.SynchronizeForceInheritProperties(null, this, null, this._parent);
			}
			else
			{
				UIElement.SynchronizeForceInheritProperties(null, this, null, parent);
			}
			base.SynchronizeReverseInheritPropertyFlags(parent, false);
		}

		// Token: 0x060004B8 RID: 1208 RVA: 0x0000D614 File Offset: 0x0000B814
		[SecurityCritical]
		[SecurityTreatAsSafe]
		internal void OnAncestorChangedInternal(TreeChangeInfo parentTreeState)
		{
			bool isSelfInheritanceParent = base.IsSelfInheritanceParent;
			if (parentTreeState.Root != this)
			{
				this.HasStyleChanged = false;
				this.HasStyleInvalidated = false;
			}
			if (parentTreeState.IsAddOperation)
			{
				FrameworkObject frameworkObject = new FrameworkObject(null, this);
				frameworkObject.SetShouldLookupImplicitStyles();
			}
			if (this.HasResourceReference)
			{
				TreeWalkHelper.OnResourcesChanged(this, ResourcesChangeInfo.TreeChangeInfo, false);
			}
			FrugalObjectList<DependencyProperty> item = this.InvalidateTreeDependentProperties(parentTreeState, base.IsSelfInheritanceParent, isSelfInheritanceParent);
			parentTreeState.InheritablePropertiesStack.Push(item);
			PresentationSource.OnAncestorChanged(this);
			this.OnAncestorChanged();
			if (this.PotentiallyHasMentees)
			{
				this.RaiseClrEvent(FrameworkElement.ResourcesChangedKey, EventArgs.Empty);
			}
		}

		// Token: 0x060004B9 RID: 1209 RVA: 0x0000D6B0 File Offset: 0x0000B8B0
		internal FrugalObjectList<DependencyProperty> InvalidateTreeDependentProperties(TreeChangeInfo parentTreeState, bool isSelfInheritanceParent, bool wasSelfInheritanceParent)
		{
			this.AncestorChangeInProgress = true;
			FrugalObjectList<DependencyProperty> result;
			try
			{
				if (!this.HasLocalStyle && this != parentTreeState.Root)
				{
					this.UpdateStyleProperty();
				}
				ChildRecord childRecord = default(ChildRecord);
				bool isChildRecordValid = false;
				Style style = this.Style;
				Style themeStyle = this.ThemeStyle;
				DependencyObject templatedParent = this.TemplatedParent;
				int templateChildIndex = this.TemplateChildIndex;
				bool hasStyleChanged = this.HasStyleChanged;
				FrameworkElement.GetTemplatedParentChildRecord(templatedParent, templateChildIndex, out childRecord, out isChildRecordValid);
				FrameworkElement frameworkElement;
				FrameworkContentElement frameworkContentElement;
				bool frameworkParent = FrameworkElement.GetFrameworkParent(this, out frameworkElement, out frameworkContentElement);
				DependencyObject parent = null;
				InheritanceBehavior inheritanceBehavior = InheritanceBehavior.Default;
				if (frameworkParent)
				{
					if (frameworkElement != null)
					{
						parent = frameworkElement;
						inheritanceBehavior = frameworkElement.InheritanceBehavior;
					}
					else
					{
						parent = frameworkContentElement;
						inheritanceBehavior = frameworkContentElement.InheritanceBehavior;
					}
				}
				if (!TreeWalkHelper.SkipNext(this.InheritanceBehavior) && !TreeWalkHelper.SkipNow(inheritanceBehavior))
				{
					base.SynchronizeInheritanceParent(parent);
				}
				else if (!base.IsSelfInheritanceParent)
				{
					base.SetIsSelfInheritanceParent();
				}
				result = TreeWalkHelper.InvalidateTreeDependentProperties(parentTreeState, null, this, style, themeStyle, ref childRecord, isChildRecordValid, hasStyleChanged, isSelfInheritanceParent, wasSelfInheritanceParent);
			}
			finally
			{
				this.AncestorChangeInProgress = false;
			}
			return result;
		}

		// Token: 0x170000C4 RID: 196
		// (get) Token: 0x060004BA RID: 1210 RVA: 0x0000D7B4 File Offset: 0x0000B9B4
		internal bool ThisHasLoadedChangeEventHandler
		{
			get
			{
				return (base.EventHandlersStore != null && (base.EventHandlersStore.Contains(FrameworkContentElement.LoadedEvent) || base.EventHandlersStore.Contains(FrameworkContentElement.UnloadedEvent))) || (this.Style != null && this.Style.HasLoadedChangeHandler) || (this.ThemeStyle != null && this.ThemeStyle.HasLoadedChangeHandler) || this.HasFefLoadedChangeHandler;
			}
		}

		// Token: 0x170000C5 RID: 197
		// (get) Token: 0x060004BB RID: 1211 RVA: 0x0000D828 File Offset: 0x0000BA28
		internal bool HasFefLoadedChangeHandler
		{
			get
			{
				if (this.TemplatedParent == null)
				{
					return false;
				}
				FrameworkElementFactory feftreeRoot = BroadcastEventHelper.GetFEFTreeRoot(this.TemplatedParent);
				if (feftreeRoot == null)
				{
					return false;
				}
				FrameworkElementFactory frameworkElementFactory = StyleHelper.FindFEF(feftreeRoot, this.TemplateChildIndex);
				return frameworkElementFactory != null && frameworkElementFactory.HasLoadedChangeHandler;
			}
		}

		// Token: 0x060004BC RID: 1212 RVA: 0x0000D868 File Offset: 0x0000BA68
		internal void UpdateStyleProperty()
		{
			if (!this.HasStyleInvalidated)
			{
				if (!this.IsStyleUpdateInProgress)
				{
					this.IsStyleUpdateInProgress = true;
					try
					{
						base.InvalidateProperty(FrameworkContentElement.StyleProperty);
						this.HasStyleInvalidated = true;
						return;
					}
					finally
					{
						this.IsStyleUpdateInProgress = false;
					}
				}
				throw new InvalidOperationException(SR.Get("CyclicStyleReferenceDetected", new object[]
				{
					this
				}));
			}
		}

		// Token: 0x060004BD RID: 1213 RVA: 0x0000D8D4 File Offset: 0x0000BAD4
		internal void UpdateThemeStyleProperty()
		{
			if (!this.IsThemeStyleUpdateInProgress)
			{
				this.IsThemeStyleUpdateInProgress = true;
				try
				{
					StyleHelper.GetThemeStyle(null, this);
					ContextMenu contextMenu = base.GetValueEntry(base.LookupEntry(FrameworkContentElement.ContextMenuProperty.GlobalIndex), FrameworkContentElement.ContextMenuProperty, null, RequestFlags.DeferredReferences).Value as ContextMenu;
					if (contextMenu != null)
					{
						TreeWalkHelper.InvalidateOnResourcesChange(contextMenu, null, ResourcesChangeInfo.ThemeChangeInfo);
					}
					DependencyObject dependencyObject = base.GetValueEntry(base.LookupEntry(FrameworkContentElement.ToolTipProperty.GlobalIndex), FrameworkContentElement.ToolTipProperty, null, RequestFlags.DeferredReferences).Value as DependencyObject;
					if (dependencyObject != null)
					{
						FrameworkObject frameworkObject = new FrameworkObject(dependencyObject);
						if (frameworkObject.IsValid)
						{
							TreeWalkHelper.InvalidateOnResourcesChange(frameworkObject.FE, frameworkObject.FCE, ResourcesChangeInfo.ThemeChangeInfo);
						}
					}
					this.OnThemeChanged();
					return;
				}
				finally
				{
					this.IsThemeStyleUpdateInProgress = false;
				}
			}
			throw new InvalidOperationException(SR.Get("CyclicThemeStyleReferenceDetected", new object[]
			{
				this
			}));
		}

		// Token: 0x060004BE RID: 1214 RVA: 0x00002137 File Offset: 0x00000337
		internal virtual void OnThemeChanged()
		{
		}

		// Token: 0x060004BF RID: 1215 RVA: 0x0000D9C8 File Offset: 0x0000BBC8
		internal void FireLoadedOnDescendentsInternal()
		{
			if (this.LoadedPending == null)
			{
				DependencyObject parent = this.Parent;
				object[] unloadedPending = this.UnloadedPending;
				if (unloadedPending == null || unloadedPending[2] != parent)
				{
					BroadcastEventHelper.AddLoadedCallback(this, parent);
					return;
				}
				BroadcastEventHelper.RemoveUnloadedCallback(this, unloadedPending);
			}
		}

		// Token: 0x060004C0 RID: 1216 RVA: 0x0000DA04 File Offset: 0x0000BC04
		internal void FireUnloadedOnDescendentsInternal()
		{
			if (this.UnloadedPending == null)
			{
				DependencyObject parent = this.Parent;
				object[] loadedPending = this.LoadedPending;
				if (loadedPending == null)
				{
					BroadcastEventHelper.AddUnloadedCallback(this, parent);
					return;
				}
				BroadcastEventHelper.RemoveLoadedCallback(this, loadedPending);
			}
		}

		// Token: 0x060004C1 RID: 1217 RVA: 0x0000DA3C File Offset: 0x0000BC3C
		internal override bool ShouldProvideInheritanceContext(DependencyObject target, DependencyProperty property)
		{
			FrameworkObject frameworkObject = new FrameworkObject(target);
			return !frameworkObject.IsValid;
		}

		// Token: 0x170000C6 RID: 198
		// (get) Token: 0x060004C2 RID: 1218 RVA: 0x0000DA5B File Offset: 0x0000BC5B
		internal override DependencyObject InheritanceContext
		{
			get
			{
				return FrameworkContentElement.InheritanceContextField.GetValue(this);
			}
		}

		// Token: 0x060004C3 RID: 1219 RVA: 0x0000DA68 File Offset: 0x0000BC68
		internal override void AddInheritanceContext(DependencyObject context, DependencyProperty property)
		{
			base.AddInheritanceContext(context, property);
			this.TryFireInitialized();
			if ((property == VisualBrush.VisualProperty || property == BitmapCacheBrush.TargetProperty) && FrameworkElement.GetFrameworkParent(this) == null && !FrameworkObject.IsEffectiveAncestor(this, context))
			{
				if (!this.HasMultipleInheritanceContexts && this.InheritanceContext == null)
				{
					FrameworkContentElement.InheritanceContextField.SetValue(this, context);
					base.OnInheritanceContextChanged(EventArgs.Empty);
					return;
				}
				if (this.InheritanceContext != null)
				{
					FrameworkContentElement.InheritanceContextField.ClearValue(this);
					this.WriteInternalFlag2(InternalFlags2.HasMultipleInheritanceContexts, true);
					base.OnInheritanceContextChanged(EventArgs.Empty);
				}
			}
		}

		// Token: 0x060004C4 RID: 1220 RVA: 0x0000DAF6 File Offset: 0x0000BCF6
		internal override void RemoveInheritanceContext(DependencyObject context, DependencyProperty property)
		{
			if (this.InheritanceContext == context)
			{
				FrameworkContentElement.InheritanceContextField.ClearValue(this);
				base.OnInheritanceContextChanged(EventArgs.Empty);
			}
			base.RemoveInheritanceContext(context, property);
		}

		// Token: 0x060004C5 RID: 1221 RVA: 0x0000DB1F File Offset: 0x0000BD1F
		private void ClearInheritanceContext()
		{
			if (this.InheritanceContext != null)
			{
				FrameworkContentElement.InheritanceContextField.ClearValue(this);
				base.OnInheritanceContextChanged(EventArgs.Empty);
			}
		}

		// Token: 0x060004C6 RID: 1222 RVA: 0x0000DB40 File Offset: 0x0000BD40
		internal override void OnInheritanceContextChangedCore(EventArgs args)
		{
			DependencyObject value = FrameworkContentElement.MentorField.GetValue(this);
			DependencyObject dependencyObject = Helper.FindMentor(this.InheritanceContext);
			if (value != dependencyObject)
			{
				FrameworkContentElement.MentorField.SetValue(this, dependencyObject);
				if (value != null)
				{
					this.DisconnectMentor(value);
				}
				if (dependencyObject != null)
				{
					this.ConnectMentor(dependencyObject);
				}
			}
		}

		// Token: 0x060004C7 RID: 1223 RVA: 0x0000DB8C File Offset: 0x0000BD8C
		private void ConnectMentor(DependencyObject mentor)
		{
			FrameworkObject frameworkObject = new FrameworkObject(mentor);
			frameworkObject.InheritedPropertyChanged += this.OnMentorInheritedPropertyChanged;
			frameworkObject.ResourcesChanged += this.OnMentorResourcesChanged;
			TreeWalkHelper.InvalidateOnTreeChange(null, this, frameworkObject.DO, true);
			if (this.SubtreeHasLoadedChangeHandler)
			{
				bool isLoaded = frameworkObject.IsLoaded;
				this.ConnectLoadedEvents(ref frameworkObject, isLoaded);
				if (isLoaded)
				{
					this.FireLoadedOnDescendentsInternal();
				}
			}
		}

		// Token: 0x060004C8 RID: 1224 RVA: 0x0000DBF8 File Offset: 0x0000BDF8
		private void DisconnectMentor(DependencyObject mentor)
		{
			FrameworkObject frameworkObject = new FrameworkObject(mentor);
			frameworkObject.InheritedPropertyChanged -= this.OnMentorInheritedPropertyChanged;
			frameworkObject.ResourcesChanged -= this.OnMentorResourcesChanged;
			TreeWalkHelper.InvalidateOnTreeChange(null, this, frameworkObject.DO, false);
			if (this.SubtreeHasLoadedChangeHandler)
			{
				bool isLoaded = frameworkObject.IsLoaded;
				this.DisconnectLoadedEvents(ref frameworkObject, isLoaded);
				if (frameworkObject.IsLoaded)
				{
					this.FireUnloadedOnDescendentsInternal();
				}
			}
		}

		// Token: 0x060004C9 RID: 1225 RVA: 0x0000DC6C File Offset: 0x0000BE6C
		internal void ChangeSubtreeHasLoadedChangedHandler(DependencyObject mentor)
		{
			FrameworkObject frameworkObject = new FrameworkObject(mentor);
			bool isLoaded = frameworkObject.IsLoaded;
			if (this.SubtreeHasLoadedChangeHandler)
			{
				this.ConnectLoadedEvents(ref frameworkObject, isLoaded);
				return;
			}
			this.DisconnectLoadedEvents(ref frameworkObject, isLoaded);
		}

		// Token: 0x060004CA RID: 1226 RVA: 0x0000DCA4 File Offset: 0x0000BEA4
		private void OnMentorLoaded(object sender, RoutedEventArgs e)
		{
			FrameworkObject frameworkObject = new FrameworkObject((DependencyObject)sender);
			frameworkObject.Loaded -= this.OnMentorLoaded;
			frameworkObject.Unloaded += this.OnMentorUnloaded;
			BroadcastEventHelper.BroadcastLoadedSynchronously(this, this.IsLoaded);
		}

		// Token: 0x060004CB RID: 1227 RVA: 0x0000DCF0 File Offset: 0x0000BEF0
		private void OnMentorUnloaded(object sender, RoutedEventArgs e)
		{
			FrameworkObject frameworkObject = new FrameworkObject((DependencyObject)sender);
			frameworkObject.Unloaded -= this.OnMentorUnloaded;
			frameworkObject.Loaded += this.OnMentorLoaded;
			BroadcastEventHelper.BroadcastUnloadedSynchronously(this, this.IsLoaded);
		}

		// Token: 0x060004CC RID: 1228 RVA: 0x0000DD3C File Offset: 0x0000BF3C
		private void ConnectLoadedEvents(ref FrameworkObject foMentor, bool isLoaded)
		{
			if (foMentor.IsValid)
			{
				if (isLoaded)
				{
					foMentor.Unloaded += this.OnMentorUnloaded;
					return;
				}
				foMentor.Loaded += this.OnMentorLoaded;
			}
		}

		// Token: 0x060004CD RID: 1229 RVA: 0x0000DD6E File Offset: 0x0000BF6E
		private void DisconnectLoadedEvents(ref FrameworkObject foMentor, bool isLoaded)
		{
			if (foMentor.IsValid)
			{
				if (isLoaded)
				{
					foMentor.Unloaded -= this.OnMentorUnloaded;
					return;
				}
				foMentor.Loaded -= this.OnMentorLoaded;
			}
		}

		// Token: 0x060004CE RID: 1230 RVA: 0x0000DDA0 File Offset: 0x0000BFA0
		private void OnMentorInheritedPropertyChanged(object sender, InheritedPropertyChangedEventArgs e)
		{
			TreeWalkHelper.InvalidateOnInheritablePropertyChange(null, this, e.Info, false);
		}

		// Token: 0x060004CF RID: 1231 RVA: 0x0000DDB0 File Offset: 0x0000BFB0
		private void OnMentorResourcesChanged(object sender, EventArgs e)
		{
			TreeWalkHelper.InvalidateOnResourcesChange(null, this, ResourcesChangeInfo.CatastrophicDictionaryChangeInfo);
		}

		// Token: 0x060004D0 RID: 1232 RVA: 0x0000DDC0 File Offset: 0x0000BFC0
		internal void RaiseInheritedPropertyChangedEvent(ref InheritablePropertyChangeInfo info)
		{
			EventHandlersStore eventHandlersStore = base.EventHandlersStore;
			if (eventHandlersStore != null)
			{
				Delegate @delegate = eventHandlersStore.Get(FrameworkElement.InheritedPropertyChangedKey);
				if (@delegate != null)
				{
					InheritedPropertyChangedEventArgs e = new InheritedPropertyChangedEventArgs(ref info);
					((InheritedPropertyChangedEventHandler)@delegate)(this, e);
				}
			}
		}

		// Token: 0x170000C7 RID: 199
		// (get) Token: 0x060004D1 RID: 1233 RVA: 0x0000DDFA File Offset: 0x0000BFFA
		// (set) Token: 0x060004D2 RID: 1234 RVA: 0x0000DE04 File Offset: 0x0000C004
		internal bool IsStyleUpdateInProgress
		{
			get
			{
				return this.ReadInternalFlag(InternalFlags.IsStyleUpdateInProgress);
			}
			set
			{
				this.WriteInternalFlag(InternalFlags.IsStyleUpdateInProgress, value);
			}
		}

		// Token: 0x170000C8 RID: 200
		// (get) Token: 0x060004D3 RID: 1235 RVA: 0x0000DE0F File Offset: 0x0000C00F
		// (set) Token: 0x060004D4 RID: 1236 RVA: 0x0000DE1C File Offset: 0x0000C01C
		internal bool IsThemeStyleUpdateInProgress
		{
			get
			{
				return this.ReadInternalFlag(InternalFlags.IsThemeStyleUpdateInProgress);
			}
			set
			{
				this.WriteInternalFlag(InternalFlags.IsThemeStyleUpdateInProgress, value);
			}
		}

		// Token: 0x170000C9 RID: 201
		// (get) Token: 0x060004D5 RID: 1237 RVA: 0x0000DE2A File Offset: 0x0000C02A
		// (set) Token: 0x060004D6 RID: 1238 RVA: 0x0000DE37 File Offset: 0x0000C037
		internal bool StoresParentTemplateValues
		{
			get
			{
				return this.ReadInternalFlag(InternalFlags.StoresParentTemplateValues);
			}
			set
			{
				this.WriteInternalFlag(InternalFlags.StoresParentTemplateValues, value);
			}
		}

		// Token: 0x170000CA RID: 202
		// (get) Token: 0x060004D7 RID: 1239 RVA: 0x0000DE45 File Offset: 0x0000C045
		// (set) Token: 0x060004D8 RID: 1240 RVA: 0x0000DE4E File Offset: 0x0000C04E
		internal bool HasNumberSubstitutionChanged
		{
			get
			{
				return this.ReadInternalFlag(InternalFlags.HasNumberSubstitutionChanged);
			}
			set
			{
				this.WriteInternalFlag(InternalFlags.HasNumberSubstitutionChanged, value);
			}
		}

		// Token: 0x170000CB RID: 203
		// (get) Token: 0x060004D9 RID: 1241 RVA: 0x0000DE58 File Offset: 0x0000C058
		// (set) Token: 0x060004DA RID: 1242 RVA: 0x0000DE65 File Offset: 0x0000C065
		internal bool HasTemplateGeneratedSubTree
		{
			get
			{
				return this.ReadInternalFlag(InternalFlags.HasTemplateGeneratedSubTree);
			}
			set
			{
				this.WriteInternalFlag(InternalFlags.HasTemplateGeneratedSubTree, value);
			}
		}

		// Token: 0x170000CC RID: 204
		// (get) Token: 0x060004DB RID: 1243 RVA: 0x0000DE73 File Offset: 0x0000C073
		// (set) Token: 0x060004DC RID: 1244 RVA: 0x0000DE7C File Offset: 0x0000C07C
		internal bool HasImplicitStyleFromResources
		{
			get
			{
				return this.ReadInternalFlag(InternalFlags.HasImplicitStyleFromResources);
			}
			set
			{
				this.WriteInternalFlag(InternalFlags.HasImplicitStyleFromResources, value);
			}
		}

		// Token: 0x170000CD RID: 205
		// (get) Token: 0x060004DD RID: 1245 RVA: 0x0000DE86 File Offset: 0x0000C086
		// (set) Token: 0x060004DE RID: 1246 RVA: 0x0000DE93 File Offset: 0x0000C093
		internal bool ShouldLookupImplicitStyles
		{
			get
			{
				return this.ReadInternalFlag(InternalFlags.ShouldLookupImplicitStyles);
			}
			set
			{
				this.WriteInternalFlag(InternalFlags.ShouldLookupImplicitStyles, value);
			}
		}

		// Token: 0x170000CE RID: 206
		// (get) Token: 0x060004DF RID: 1247 RVA: 0x0000DEA1 File Offset: 0x0000C0A1
		// (set) Token: 0x060004E0 RID: 1248 RVA: 0x0000DEAE File Offset: 0x0000C0AE
		internal bool IsStyleSetFromGenerator
		{
			get
			{
				return this.ReadInternalFlag2(InternalFlags2.IsStyleSetFromGenerator);
			}
			set
			{
				this.WriteInternalFlag2(InternalFlags2.IsStyleSetFromGenerator, value);
			}
		}

		// Token: 0x170000CF RID: 207
		// (get) Token: 0x060004E1 RID: 1249 RVA: 0x0000DEBC File Offset: 0x0000C0BC
		// (set) Token: 0x060004E2 RID: 1250 RVA: 0x0000DEC9 File Offset: 0x0000C0C9
		internal bool HasStyleChanged
		{
			get
			{
				return this.ReadInternalFlag2(InternalFlags2.HasStyleChanged);
			}
			set
			{
				this.WriteInternalFlag2(InternalFlags2.HasStyleChanged, value);
			}
		}

		// Token: 0x170000D0 RID: 208
		// (get) Token: 0x060004E3 RID: 1251 RVA: 0x0000DED7 File Offset: 0x0000C0D7
		// (set) Token: 0x060004E4 RID: 1252 RVA: 0x0000DEE4 File Offset: 0x0000C0E4
		internal bool HasStyleInvalidated
		{
			get
			{
				return this.ReadInternalFlag2(InternalFlags2.HasStyleInvalidated);
			}
			set
			{
				this.WriteInternalFlag2(InternalFlags2.HasStyleInvalidated, value);
			}
		}

		// Token: 0x170000D1 RID: 209
		// (get) Token: 0x060004E5 RID: 1253 RVA: 0x0000DEF2 File Offset: 0x0000C0F2
		// (set) Token: 0x060004E6 RID: 1254 RVA: 0x0000DEFF File Offset: 0x0000C0FF
		internal bool HasStyleEverBeenFetched
		{
			get
			{
				return this.ReadInternalFlag(InternalFlags.HasStyleEverBeenFetched);
			}
			set
			{
				this.WriteInternalFlag(InternalFlags.HasStyleEverBeenFetched, value);
			}
		}

		// Token: 0x170000D2 RID: 210
		// (get) Token: 0x060004E7 RID: 1255 RVA: 0x0000DF0D File Offset: 0x0000C10D
		// (set) Token: 0x060004E8 RID: 1256 RVA: 0x0000DF1A File Offset: 0x0000C11A
		internal bool HasLocalStyle
		{
			get
			{
				return this.ReadInternalFlag(InternalFlags.HasLocalStyle);
			}
			set
			{
				this.WriteInternalFlag(InternalFlags.HasLocalStyle, value);
			}
		}

		// Token: 0x170000D3 RID: 211
		// (get) Token: 0x060004E9 RID: 1257 RVA: 0x0000DF28 File Offset: 0x0000C128
		// (set) Token: 0x060004EA RID: 1258 RVA: 0x0000DF35 File Offset: 0x0000C135
		internal bool HasThemeStyleEverBeenFetched
		{
			get
			{
				return this.ReadInternalFlag(InternalFlags.HasThemeStyleEverBeenFetched);
			}
			set
			{
				this.WriteInternalFlag(InternalFlags.HasThemeStyleEverBeenFetched, value);
			}
		}

		// Token: 0x170000D4 RID: 212
		// (get) Token: 0x060004EB RID: 1259 RVA: 0x0000DF43 File Offset: 0x0000C143
		// (set) Token: 0x060004EC RID: 1260 RVA: 0x0000DF50 File Offset: 0x0000C150
		internal bool AncestorChangeInProgress
		{
			get
			{
				return this.ReadInternalFlag(InternalFlags.AncestorChangeInProgress);
			}
			set
			{
				this.WriteInternalFlag(InternalFlags.AncestorChangeInProgress, value);
			}
		}

		// Token: 0x170000D5 RID: 213
		// (get) Token: 0x060004ED RID: 1261 RVA: 0x0000DF5E File Offset: 0x0000C15E
		// (set) Token: 0x060004EE RID: 1262 RVA: 0x0000DF66 File Offset: 0x0000C166
		internal FrugalObjectList<DependencyProperty> InheritableProperties
		{
			get
			{
				return this._inheritableProperties;
			}
			set
			{
				this._inheritableProperties = value;
			}
		}

		// Token: 0x170000D6 RID: 214
		// (get) Token: 0x060004EF RID: 1263 RVA: 0x0000DF6F File Offset: 0x0000C16F
		internal object[] LoadedPending
		{
			get
			{
				return (object[])base.GetValue(FrameworkContentElement.LoadedPendingProperty);
			}
		}

		// Token: 0x170000D7 RID: 215
		// (get) Token: 0x060004F0 RID: 1264 RVA: 0x0000DF81 File Offset: 0x0000C181
		internal object[] UnloadedPending
		{
			get
			{
				return (object[])base.GetValue(FrameworkContentElement.UnloadedPendingProperty);
			}
		}

		// Token: 0x170000D8 RID: 216
		// (get) Token: 0x060004F1 RID: 1265 RVA: 0x0000DF93 File Offset: 0x0000C193
		internal override bool HasMultipleInheritanceContexts
		{
			get
			{
				return this.ReadInternalFlag2(InternalFlags2.HasMultipleInheritanceContexts);
			}
		}

		// Token: 0x170000D9 RID: 217
		// (get) Token: 0x060004F2 RID: 1266 RVA: 0x0000DFA0 File Offset: 0x0000C1A0
		// (set) Token: 0x060004F3 RID: 1267 RVA: 0x0000DFAD File Offset: 0x0000C1AD
		internal bool PotentiallyHasMentees
		{
			get
			{
				return this.ReadInternalFlag((InternalFlags)2147483648U);
			}
			set
			{
				this.WriteInternalFlag((InternalFlags)2147483648U, value);
			}
		}

		// Token: 0x14000019 RID: 25
		// (add) Token: 0x060004F4 RID: 1268 RVA: 0x0000DFBB File Offset: 0x0000C1BB
		// (remove) Token: 0x060004F5 RID: 1269 RVA: 0x0000DFD0 File Offset: 0x0000C1D0
		internal event EventHandler ResourcesChanged
		{
			add
			{
				this.PotentiallyHasMentees = true;
				this.EventHandlersStoreAdd(FrameworkElement.ResourcesChangedKey, value);
			}
			remove
			{
				this.EventHandlersStoreRemove(FrameworkElement.ResourcesChangedKey, value);
			}
		}

		// Token: 0x1400001A RID: 26
		// (add) Token: 0x060004F6 RID: 1270 RVA: 0x0000DFDE File Offset: 0x0000C1DE
		// (remove) Token: 0x060004F7 RID: 1271 RVA: 0x0000DFF3 File Offset: 0x0000C1F3
		internal event InheritedPropertyChangedEventHandler InheritedPropertyChanged
		{
			add
			{
				this.PotentiallyHasMentees = true;
				this.EventHandlersStoreAdd(FrameworkElement.InheritedPropertyChangedKey, value);
			}
			remove
			{
				this.EventHandlersStoreRemove(FrameworkElement.InheritedPropertyChangedKey, value);
			}
		}

		// Token: 0x04000629 RID: 1577
		internal static readonly NumberSubstitution DefaultNumberSubstitution = new NumberSubstitution(NumberCultureSource.Text, null, NumberSubstitutionMethod.AsCulture);

		/// <summary> Identifies the <see cref="P:System.Windows.FrameworkContentElement.Style" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.FrameworkContentElement.Style" /> dependency property.</returns>
		// Token: 0x0400062A RID: 1578
		[CommonDependencyProperty]
		public static readonly DependencyProperty StyleProperty = FrameworkElement.StyleProperty.AddOwner(typeof(FrameworkContentElement), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsMeasure, new PropertyChangedCallback(FrameworkContentElement.OnStyleChanged)));

		/// <summary> Identifies the <see cref="P:System.Windows.FrameworkContentElement.OverridesDefaultStyle" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.FrameworkContentElement.OverridesDefaultStyle" /> dependency property.</returns>
		// Token: 0x0400062B RID: 1579
		public static readonly DependencyProperty OverridesDefaultStyleProperty = FrameworkElement.OverridesDefaultStyleProperty.AddOwner(typeof(FrameworkContentElement), new FrameworkPropertyMetadata(BooleanBoxes.FalseBox, FrameworkPropertyMetadataOptions.AffectsMeasure, new PropertyChangedCallback(FrameworkContentElement.OnThemeStyleKeyChanged)));

		/// <summary> Identifies the <see cref="P:System.Windows.FrameworkContentElement.DefaultStyleKey" /> dependency property. </summary>
		/// <returns>The <see cref="P:System.Windows.FrameworkContentElement.DefaultStyleKey" /> dependency property identifier.</returns>
		// Token: 0x0400062C RID: 1580
		protected internal static readonly DependencyProperty DefaultStyleKeyProperty = FrameworkElement.DefaultStyleKeyProperty.AddOwner(typeof(FrameworkContentElement), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsMeasure, new PropertyChangedCallback(FrameworkContentElement.OnThemeStyleKeyChanged)));

		/// <summary> Identifies the <see cref="P:System.Windows.FrameworkContentElement.Name" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.FrameworkContentElement.Name" /> dependency property.</returns>
		// Token: 0x0400062D RID: 1581
		public static readonly DependencyProperty NameProperty = FrameworkElement.NameProperty.AddOwner(typeof(FrameworkContentElement), new FrameworkPropertyMetadata(string.Empty));

		/// <summary> Identifies the <see cref="P:System.Windows.FrameworkContentElement.Tag" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.FrameworkContentElement.Tag" /> dependency property.</returns>
		// Token: 0x0400062E RID: 1582
		public static readonly DependencyProperty TagProperty = FrameworkElement.TagProperty.AddOwner(typeof(FrameworkContentElement), new FrameworkPropertyMetadata(null));

		/// <summary> Identifies the <see cref="P:System.Windows.FrameworkContentElement.Language" /> dependency property. </summary>
		/// <returns>The <see cref="P:System.Windows.FrameworkContentElement.Language" /> dependency property identifier.</returns>
		// Token: 0x0400062F RID: 1583
		public static readonly DependencyProperty LanguageProperty = FrameworkElement.LanguageProperty.AddOwner(typeof(FrameworkContentElement), new FrameworkPropertyMetadata(XmlLanguage.GetLanguage("en-US"), FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.Inherits));

		/// <summary> Identifies the <see cref="P:System.Windows.FrameworkContentElement.FocusVisualStyle" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.FrameworkContentElement.FocusVisualStyle" /> dependency property.</returns>
		// Token: 0x04000630 RID: 1584
		public static readonly DependencyProperty FocusVisualStyleProperty = FrameworkElement.FocusVisualStyleProperty.AddOwner(typeof(FrameworkContentElement), new FrameworkPropertyMetadata(FrameworkElement.DefaultFocusVisualStyle));

		/// <summary> Identifies the <see cref="P:System.Windows.FrameworkContentElement.Cursor" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.FrameworkContentElement.Cursor" /> dependency property.</returns>
		// Token: 0x04000631 RID: 1585
		public static readonly DependencyProperty CursorProperty = FrameworkElement.CursorProperty.AddOwner(typeof(FrameworkContentElement), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.None, new PropertyChangedCallback(FrameworkContentElement.OnCursorChanged)));

		/// <summary> Identifies the <see cref="P:System.Windows.FrameworkContentElement.ForceCursor" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.FrameworkContentElement.ForceCursor" /> dependency property.</returns>
		// Token: 0x04000632 RID: 1586
		public static readonly DependencyProperty ForceCursorProperty = FrameworkElement.ForceCursorProperty.AddOwner(typeof(FrameworkContentElement), new FrameworkPropertyMetadata(BooleanBoxes.FalseBox, FrameworkPropertyMetadataOptions.None, new PropertyChangedCallback(FrameworkContentElement.OnForceCursorChanged)));

		/// <summary> Identifies the <see cref="P:System.Windows.FrameworkContentElement.InputScope" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.FrameworkContentElement.InputScope" /> dependency property.</returns>
		// Token: 0x04000633 RID: 1587
		public static readonly DependencyProperty InputScopeProperty = InputMethod.InputScopeProperty.AddOwner(typeof(FrameworkContentElement), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.Inherits));

		/// <summary> Identifies the <see cref="P:System.Windows.FrameworkContentElement.DataContext" /> dependency property. </summary>
		/// <returns>The <see cref="P:System.Windows.FrameworkContentElement.DataContext" /> dependency property identifier.</returns>
		// Token: 0x04000634 RID: 1588
		public static readonly DependencyProperty DataContextProperty = FrameworkElement.DataContextProperty.AddOwner(typeof(FrameworkContentElement), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.Inherits, new PropertyChangedCallback(FrameworkContentElement.OnDataContextChanged)));

		/// <summary>Identifies the <see cref="P:System.Windows.FrameworkContentElement.BindingGroup" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.FrameworkContentElement.BindingGroup" /> dependency property.</returns>
		// Token: 0x04000635 RID: 1589
		public static readonly DependencyProperty BindingGroupProperty = FrameworkElement.BindingGroupProperty.AddOwner(typeof(FrameworkContentElement), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.Inherits));

		// Token: 0x04000636 RID: 1590
		private static readonly DependencyProperty LoadedPendingProperty = FrameworkElement.LoadedPendingProperty.AddOwner(typeof(FrameworkContentElement));

		// Token: 0x04000637 RID: 1591
		private static readonly DependencyProperty UnloadedPendingProperty = FrameworkElement.UnloadedPendingProperty.AddOwner(typeof(FrameworkContentElement));

		/// <summary> Identifies the <see cref="P:System.Windows.FrameworkContentElement.ToolTip" /> dependency property. </summary>
		/// <returns>The <see cref="P:System.Windows.FrameworkContentElement.ToolTip" /> dependency property identifier.</returns>
		// Token: 0x0400063A RID: 1594
		public static readonly DependencyProperty ToolTipProperty;

		/// <summary> Identifies the <see cref="P:System.Windows.FrameworkContentElement.ContextMenu" /> dependency property. </summary>
		/// <returns>The <see cref="P:System.Windows.FrameworkContentElement.ContextMenu" /> dependency property identifier.</returns>
		// Token: 0x0400063B RID: 1595
		public static readonly DependencyProperty ContextMenuProperty;

		// Token: 0x04000640 RID: 1600
		private Style _styleCache;

		// Token: 0x04000641 RID: 1601
		private Style _themeStyleCache;

		// Token: 0x04000642 RID: 1602
		internal DependencyObject _templatedParent;

		// Token: 0x04000643 RID: 1603
		private static readonly UncommonField<ResourceDictionary> ResourcesField;

		// Token: 0x04000644 RID: 1604
		private InternalFlags _flags;

		// Token: 0x04000645 RID: 1605
		private InternalFlags2 _flags2 = InternalFlags2.Default;

		// Token: 0x04000646 RID: 1606
		internal new static DependencyObjectType DType;

		// Token: 0x04000647 RID: 1607
		private new DependencyObject _parent;

		// Token: 0x04000648 RID: 1608
		private FrugalObjectList<DependencyProperty> _inheritableProperties;

		// Token: 0x04000649 RID: 1609
		private static readonly UncommonField<DependencyObject> InheritanceContextField;

		// Token: 0x0400064A RID: 1610
		private static readonly UncommonField<DependencyObject> MentorField;
	}
}
