using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Security;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Diagnostics;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using MS.Internal;
using MS.Internal.KnownBoxes;
using MS.Internal.PresentationFramework;
using MS.Utility;

namespace System.Windows
{
	/// <summary>Provides a WPF framework-level set of properties, events, and methods for Windows Presentation Foundation (WPF) elements. This class represents the provided WPF framework-level implementation that is built on the WPF core-level APIs that are defined by <see cref="T:System.Windows.UIElement" />.</summary>
	// Token: 0x020000C3 RID: 195
	[StyleTypedProperty(Property = "FocusVisualStyle", StyleTargetType = typeof(Control))]
	[XmlLangProperty("Language")]
	[UsableDuringInitialization(true)]
	[RuntimeNameProperty("Name")]
	public class FrameworkElement : UIElement, IFrameworkInputElement, IInputElement, ISupportInitialize, IHaveResources, IQueryAmbient
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.FrameworkElement" /> class. </summary>
		// Token: 0x060004FD RID: 1277 RVA: 0x0000E0F0 File Offset: 0x0000C2F0
		public FrameworkElement()
		{
			PropertyMetadata metadata = FrameworkElement.StyleProperty.GetMetadata(base.DependencyObjectType);
			Style style = (Style)metadata.DefaultValue;
			if (style != null)
			{
				FrameworkElement.OnStyleChanged(this, new DependencyPropertyChangedEventArgs(FrameworkElement.StyleProperty, metadata, null, style));
			}
			if ((FlowDirection)FrameworkElement.FlowDirectionProperty.GetDefaultValue(base.DependencyObjectType) == FlowDirection.RightToLeft)
			{
				this.IsRightToLeft = true;
			}
			Application application = Application.Current;
			if (application != null && application.HasImplicitStylesInResources)
			{
				this.ShouldLookupImplicitStyles = true;
			}
			FrameworkElement.EnsureFrameworkServices();
		}

		/// <summary>Gets or sets the style used by this element when it is rendered.  </summary>
		/// <returns>The applied, nondefault style for the element, if present. Otherwise, <see langword="null" />. The default for a default-constructed <see cref="T:System.Windows.FrameworkElement" /> is <see langword="null" />.</returns>
		// Token: 0x170000DA RID: 218
		// (get) Token: 0x060004FE RID: 1278 RVA: 0x0000E17E File Offset: 0x0000C37E
		// (set) Token: 0x060004FF RID: 1279 RVA: 0x0000E186 File Offset: 0x0000C386
		public Style Style
		{
			get
			{
				return this._styleCache;
			}
			set
			{
				base.SetValue(FrameworkElement.StyleProperty, value);
			}
		}

		/// <summary>Returns whether serialization processes should serialize the contents of the <see cref="P:System.Windows.FrameworkElement.Style" /> property.</summary>
		/// <returns>
		///     <see langword="true" /> if the <see cref="P:System.Windows.FrameworkElement.Style" /> property value should be serialized; otherwise, <see langword="false" />.</returns>
		// Token: 0x06000500 RID: 1280 RVA: 0x0000E194 File Offset: 0x0000C394
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool ShouldSerializeStyle()
		{
			return !this.IsStyleSetFromGenerator && base.ReadLocalValue(FrameworkElement.StyleProperty) != DependencyProperty.UnsetValue;
		}

		// Token: 0x06000501 RID: 1281 RVA: 0x0000E1B8 File Offset: 0x0000C3B8
		private static void OnStyleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			FrameworkElement frameworkElement = (FrameworkElement)d;
			frameworkElement.HasLocalStyle = (e.NewEntry.BaseValueSourceInternal == BaseValueSourceInternal.Local);
			StyleHelper.UpdateStyleCache(frameworkElement, null, (Style)e.OldValue, (Style)e.NewValue, ref frameworkElement._styleCache);
		}

		/// <summary>Gets or sets a value that indicates whether this element incorporates style properties from theme styles. </summary>
		/// <returns>
		///     <see langword="true" /> if this element does not use theme style properties; all style-originating properties come from local application styles, and theme style properties do not apply. <see langword="false" /> if application styles apply first, and then theme styles apply for properties that were not specifically set in application styles. The default is <see langword="false" />.</returns>
		// Token: 0x170000DB RID: 219
		// (get) Token: 0x06000502 RID: 1282 RVA: 0x0000E20A File Offset: 0x0000C40A
		// (set) Token: 0x06000503 RID: 1283 RVA: 0x0000E21C File Offset: 0x0000C41C
		public bool OverridesDefaultStyle
		{
			get
			{
				return (bool)base.GetValue(FrameworkElement.OverridesDefaultStyleProperty);
			}
			set
			{
				base.SetValue(FrameworkElement.OverridesDefaultStyleProperty, BooleanBoxes.Box(value));
			}
		}

		/// <summary>Gets or sets a value that indicates whether layout rounding should be applied to this element's size and position during layout. </summary>
		/// <returns>
		///     <see langword="true" /> if layout rounding is applied; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
		// Token: 0x170000DC RID: 220
		// (get) Token: 0x06000504 RID: 1284 RVA: 0x0000E22F File Offset: 0x0000C42F
		// (set) Token: 0x06000505 RID: 1285 RVA: 0x0000E241 File Offset: 0x0000C441
		public bool UseLayoutRounding
		{
			get
			{
				return (bool)base.GetValue(FrameworkElement.UseLayoutRoundingProperty);
			}
			set
			{
				base.SetValue(FrameworkElement.UseLayoutRoundingProperty, BooleanBoxes.Box(value));
			}
		}

		// Token: 0x06000506 RID: 1286 RVA: 0x0000E254 File Offset: 0x0000C454
		private static void OnUseLayoutRoundingChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			FrameworkElement frameworkElement = (FrameworkElement)d;
			bool value = (bool)e.NewValue;
			frameworkElement.SetFlags(value, VisualFlags.UseLayoutRounding);
		}

		/// <summary>Gets or sets the key to use to reference the style for this control, when theme styles are used or defined.</summary>
		/// <returns>The style key. To work correctly as part of theme style lookup, this value is expected to be the <see cref="T:System.Type" /> of the control being styled.</returns>
		// Token: 0x170000DD RID: 221
		// (get) Token: 0x06000507 RID: 1287 RVA: 0x0000E281 File Offset: 0x0000C481
		// (set) Token: 0x06000508 RID: 1288 RVA: 0x0000E28E File Offset: 0x0000C48E
		protected internal object DefaultStyleKey
		{
			get
			{
				return base.GetValue(FrameworkElement.DefaultStyleKeyProperty);
			}
			set
			{
				base.SetValue(FrameworkElement.DefaultStyleKeyProperty, value);
			}
		}

		// Token: 0x06000509 RID: 1289 RVA: 0x0000E29C File Offset: 0x0000C49C
		private static void OnThemeStyleKeyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((FrameworkElement)d).UpdateThemeStyleProperty();
		}

		// Token: 0x170000DE RID: 222
		// (get) Token: 0x0600050A RID: 1290 RVA: 0x0000E2A9 File Offset: 0x0000C4A9
		internal Style ThemeStyle
		{
			get
			{
				return this._themeStyleCache;
			}
		}

		// Token: 0x170000DF RID: 223
		// (get) Token: 0x0600050B RID: 1291 RVA: 0x0000C238 File Offset: 0x0000A438
		internal virtual DependencyObjectType DTypeThemeStyleKey
		{
			get
			{
				return null;
			}
		}

		// Token: 0x0600050C RID: 1292 RVA: 0x0000E2B4 File Offset: 0x0000C4B4
		internal static void OnThemeStyleChanged(DependencyObject d, object oldValue, object newValue)
		{
			FrameworkElement frameworkElement = (FrameworkElement)d;
			StyleHelper.UpdateThemeStyleCache(frameworkElement, null, (Style)oldValue, (Style)newValue, ref frameworkElement._themeStyleCache);
		}

		// Token: 0x170000E0 RID: 224
		// (get) Token: 0x0600050D RID: 1293 RVA: 0x0000C238 File Offset: 0x0000A438
		internal virtual FrameworkTemplate TemplateInternal
		{
			get
			{
				return null;
			}
		}

		// Token: 0x170000E1 RID: 225
		// (get) Token: 0x0600050E RID: 1294 RVA: 0x0000C238 File Offset: 0x0000A438
		// (set) Token: 0x0600050F RID: 1295 RVA: 0x00002137 File Offset: 0x00000337
		internal virtual FrameworkTemplate TemplateCache
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		// Token: 0x06000510 RID: 1296 RVA: 0x0000E2E1 File Offset: 0x0000C4E1
		internal virtual void OnTemplateChangedInternal(FrameworkTemplate oldTemplate, FrameworkTemplate newTemplate)
		{
			this.HasTemplateChanged = true;
		}

		/// <summary>Invoked when the style in use on this element changes, which will invalidate the layout. </summary>
		/// <param name="oldStyle">The old style.</param>
		/// <param name="newStyle">The new style.</param>
		// Token: 0x06000511 RID: 1297 RVA: 0x0000E2EA File Offset: 0x0000C4EA
		protected internal virtual void OnStyleChanged(Style oldStyle, Style newStyle)
		{
			this.HasStyleChanged = true;
		}

		/// <summary> Supports incremental layout implementations in specialized subclasses of <see cref="T:System.Windows.FrameworkElement" />. <see cref="M:System.Windows.FrameworkElement.ParentLayoutInvalidated(System.Windows.UIElement)" />  is invoked when a child element has invalidated a property that is marked in metadata as affecting the parent's measure or arrange passes during layout. </summary>
		/// <param name="child">The child element reporting the change.</param>
		// Token: 0x06000512 RID: 1298 RVA: 0x00002137 File Offset: 0x00000337
		protected internal virtual void ParentLayoutInvalidated(UIElement child)
		{
		}

		/// <summary>Builds the current template's visual tree if necessary, and returns a value that indicates whether the visual tree was rebuilt by this call. </summary>
		/// <returns>
		///     <see langword="true" /> if visuals were added to the tree; returns <see langword="false" /> otherwise.</returns>
		// Token: 0x06000513 RID: 1299 RVA: 0x0000E2F4 File Offset: 0x0000C4F4
		public bool ApplyTemplate()
		{
			this.OnPreApplyTemplate();
			bool flag = false;
			UncommonField<HybridDictionary[]> templateDataField = StyleHelper.TemplateDataField;
			FrameworkTemplate templateInternal = this.TemplateInternal;
			int num = 2;
			int num2 = 0;
			while (templateInternal != null && num2 < num && !this.HasTemplateGeneratedSubTree)
			{
				flag = templateInternal.ApplyTemplateContent(templateDataField, this);
				if (flag)
				{
					this.HasTemplateGeneratedSubTree = true;
					StyleHelper.InvokeDeferredActions(this, templateInternal);
					this.OnApplyTemplate();
				}
				if (templateInternal == this.TemplateInternal)
				{
					break;
				}
				templateInternal = this.TemplateInternal;
				num2++;
			}
			this.OnPostApplyTemplate();
			return flag;
		}

		// Token: 0x06000514 RID: 1300 RVA: 0x00002137 File Offset: 0x00000337
		internal virtual void OnPreApplyTemplate()
		{
		}

		/// <summary>When overridden in a derived class, is invoked whenever application code or internal processes call <see cref="M:System.Windows.FrameworkElement.ApplyTemplate" />.</summary>
		// Token: 0x06000515 RID: 1301 RVA: 0x00002137 File Offset: 0x00000337
		public virtual void OnApplyTemplate()
		{
		}

		// Token: 0x06000516 RID: 1302 RVA: 0x00002137 File Offset: 0x00000337
		internal virtual void OnPostApplyTemplate()
		{
		}

		/// <summary>Begins the sequence of actions that are contained in the provided storyboard. </summary>
		/// <param name="storyboard">The storyboard to begin.</param>
		// Token: 0x06000517 RID: 1303 RVA: 0x0000E36A File Offset: 0x0000C56A
		public void BeginStoryboard(Storyboard storyboard)
		{
			this.BeginStoryboard(storyboard, HandoffBehavior.SnapshotAndReplace, false);
		}

		/// <summary>Begins the sequence of actions contained in the provided storyboard, with options specified for what should happen if the property is already animated. </summary>
		/// <param name="storyboard">The storyboard to begin.</param>
		/// <param name="handoffBehavior">A value of the enumeration that describes behavior to use if a property described in the storyboard is already animated.</param>
		// Token: 0x06000518 RID: 1304 RVA: 0x0000E375 File Offset: 0x0000C575
		public void BeginStoryboard(Storyboard storyboard, HandoffBehavior handoffBehavior)
		{
			this.BeginStoryboard(storyboard, handoffBehavior, false);
		}

		/// <summary> Begins the sequence of actions contained in the provided storyboard, with specified state for control of the animation after it is started. </summary>
		/// <param name="storyboard">The storyboard to begin. </param>
		/// <param name="handoffBehavior">A value of the enumeration that describes behavior to use if a property described in the storyboard is already animated.</param>
		/// <param name="isControllable">Declares whether the animation is controllable (can be paused) after it is started.</param>
		// Token: 0x06000519 RID: 1305 RVA: 0x0000E380 File Offset: 0x0000C580
		public void BeginStoryboard(Storyboard storyboard, HandoffBehavior handoffBehavior, bool isControllable)
		{
			if (storyboard == null)
			{
				throw new ArgumentNullException("storyboard");
			}
			storyboard.Begin(this, handoffBehavior, isControllable);
		}

		// Token: 0x0600051A RID: 1306 RVA: 0x0000E39C File Offset: 0x0000C59C
		internal static FrameworkElement FindNamedFrameworkElement(FrameworkElement startElement, string targetName)
		{
			FrameworkElement result;
			if (targetName == null || targetName.Length == 0)
			{
				result = startElement;
			}
			else
			{
				DependencyObject dependencyObject = LogicalTreeHelper.FindLogicalNode(startElement, targetName);
				if (dependencyObject == null)
				{
					throw new ArgumentException(SR.Get("TargetNameNotFound", new object[]
					{
						targetName
					}));
				}
				FrameworkObject frameworkObject = new FrameworkObject(dependencyObject);
				if (!frameworkObject.IsFE)
				{
					throw new InvalidOperationException(SR.Get("NamedObjectMustBeFrameworkElement", new object[]
					{
						targetName
					}));
				}
				result = frameworkObject.FE;
			}
			return result;
		}

		/// <summary>Gets the collection of triggers established directly on this element, or in child elements. </summary>
		/// <returns>A strongly typed collection of <see cref="T:System.Windows.Trigger" /> objects.</returns>
		// Token: 0x170000E2 RID: 226
		// (get) Token: 0x0600051B RID: 1307 RVA: 0x0000E418 File Offset: 0x0000C618
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public TriggerCollection Triggers
		{
			get
			{
				TriggerCollection triggerCollection = EventTrigger.TriggerCollectionField.GetValue(this);
				if (triggerCollection == null)
				{
					triggerCollection = new TriggerCollection(this);
					EventTrigger.TriggerCollectionField.SetValue(this, triggerCollection);
				}
				return triggerCollection;
			}
		}

		/// <summary>Returns whether serialization processes should serialize the contents of the <see cref="P:System.Windows.FrameworkElement.Triggers" /> property.</summary>
		/// <returns>
		///     <see langword="true" /> if the <see cref="P:System.Windows.FrameworkElement.Triggers" /> property value should be serialized; otherwise, <see langword="false" />.</returns>
		// Token: 0x0600051C RID: 1308 RVA: 0x0000E448 File Offset: 0x0000C648
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool ShouldSerializeTriggers()
		{
			TriggerCollection value = EventTrigger.TriggerCollectionField.GetValue(this);
			return value != null && value.Count != 0;
		}

		// Token: 0x0600051D RID: 1309 RVA: 0x0000E46F File Offset: 0x0000C66F
		private void PrivateInitialized()
		{
			EventTrigger.ProcessTriggerCollection(this);
		}

		/// <summary>Gets a reference to the template parent of this element. This property is not relevant if the element was not created through a template.</summary>
		/// <returns>The element whose <see cref="T:System.Windows.FrameworkTemplate" /> <see cref="P:System.Windows.FrameworkTemplate.VisualTree" /> caused this element to be created. This value is frequently <see langword="null" />; see Remarks.</returns>
		// Token: 0x170000E3 RID: 227
		// (get) Token: 0x0600051E RID: 1310 RVA: 0x0000E477 File Offset: 0x0000C677
		public DependencyObject TemplatedParent
		{
			get
			{
				return this._templatedParent;
			}
		}

		// Token: 0x170000E4 RID: 228
		// (get) Token: 0x0600051F RID: 1311 RVA: 0x0000E47F File Offset: 0x0000C67F
		internal bool IsTemplateRoot
		{
			get
			{
				return this.TemplateChildIndex == 1;
			}
		}

		// Token: 0x170000E5 RID: 229
		// (get) Token: 0x06000520 RID: 1312 RVA: 0x0000E48A File Offset: 0x0000C68A
		// (set) Token: 0x06000521 RID: 1313 RVA: 0x0000E492 File Offset: 0x0000C692
		internal virtual UIElement TemplateChild
		{
			get
			{
				return this._templateChild;
			}
			set
			{
				if (value != this._templateChild)
				{
					base.RemoveVisualChild(this._templateChild);
					this._templateChild = value;
					base.AddVisualChild(value);
				}
			}
		}

		// Token: 0x170000E6 RID: 230
		// (get) Token: 0x06000522 RID: 1314 RVA: 0x0000E4B7 File Offset: 0x0000C6B7
		internal virtual FrameworkElement StateGroupsRoot
		{
			get
			{
				return this._templateChild as FrameworkElement;
			}
		}

		/// <summary>Gets the number of visual child elements within this element.</summary>
		/// <returns>The number of visual child elements for this element.</returns>
		// Token: 0x170000E7 RID: 231
		// (get) Token: 0x06000523 RID: 1315 RVA: 0x0000E4C4 File Offset: 0x0000C6C4
		protected override int VisualChildrenCount
		{
			get
			{
				if (this._templateChild != null)
				{
					return 1;
				}
				return 0;
			}
		}

		/// <summary>Overrides <see cref="M:System.Windows.Media.Visual.GetVisualChild(System.Int32)" />, and returns a child at the specified index from a collection of child elements. </summary>
		/// <param name="index">The zero-based index of the requested child element in the collection.</param>
		/// <returns>The requested child element. This should not return <see langword="null" />; if the provided index is out of range, an exception is thrown.</returns>
		// Token: 0x06000524 RID: 1316 RVA: 0x0000E4D4 File Offset: 0x0000C6D4
		protected override Visual GetVisualChild(int index)
		{
			if (this._templateChild == null)
			{
				throw new ArgumentOutOfRangeException("index", index, SR.Get("Visual_ArgumentOutOfRange"));
			}
			if (index != 0)
			{
				throw new ArgumentOutOfRangeException("index", index, SR.Get("Visual_ArgumentOutOfRange"));
			}
			return this._templateChild;
		}

		// Token: 0x170000E8 RID: 232
		// (get) Token: 0x06000525 RID: 1317 RVA: 0x0000E528 File Offset: 0x0000C728
		internal bool HasResources
		{
			get
			{
				ResourceDictionary value = FrameworkElement.ResourcesField.GetValue(this);
				return value != null && (value.Count > 0 || value.MergedDictionaries.Count > 0);
			}
		}

		/// <summary> Gets or sets the locally-defined resource dictionary. </summary>
		/// <returns>The current locally-defined dictionary of resources, where each resource can be accessed by key.</returns>
		// Token: 0x170000E9 RID: 233
		// (get) Token: 0x06000526 RID: 1318 RVA: 0x0000E560 File Offset: 0x0000C760
		// (set) Token: 0x06000527 RID: 1319 RVA: 0x0000E5B0 File Offset: 0x0000C7B0
		[Ambient]
		public ResourceDictionary Resources
		{
			get
			{
				ResourceDictionary resourceDictionary = FrameworkElement.ResourcesField.GetValue(this);
				if (resourceDictionary == null)
				{
					resourceDictionary = new ResourceDictionary();
					resourceDictionary.AddOwner(this);
					FrameworkElement.ResourcesField.SetValue(this, resourceDictionary);
					if (TraceResourceDictionary.IsEnabled)
					{
						TraceResourceDictionary.TraceActivityItem(TraceResourceDictionary.NewResourceDictionary, this, 0, resourceDictionary);
					}
				}
				return resourceDictionary;
			}
			set
			{
				ResourceDictionary value2 = FrameworkElement.ResourcesField.GetValue(this);
				FrameworkElement.ResourcesField.SetValue(this, value);
				if (TraceResourceDictionary.IsEnabled)
				{
					TraceResourceDictionary.Trace(TraceEventType.Start, TraceResourceDictionary.NewResourceDictionary, this, value2, value);
				}
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
					TreeWalkHelper.InvalidateOnResourcesChange(this, null, new ResourcesChangeInfo(value2, value));
				}
				if (TraceResourceDictionary.IsEnabled)
				{
					TraceResourceDictionary.Trace(TraceEventType.Stop, TraceResourceDictionary.NewResourceDictionary, this, value2, value);
				}
			}
		}

		// Token: 0x170000EA RID: 234
		// (get) Token: 0x06000528 RID: 1320 RVA: 0x0000E636 File Offset: 0x0000C836
		// (set) Token: 0x06000529 RID: 1321 RVA: 0x0000E63E File Offset: 0x0000C83E
		ResourceDictionary IHaveResources.Resources
		{
			get
			{
				return this.Resources;
			}
			set
			{
				this.Resources = value;
			}
		}

		/// <summary>For a description of this member, see the <see cref="M:System.Windows.Markup.IQueryAmbient.IsAmbientPropertyAvailable(System.String)" /> method.</summary>
		/// <param name="propertyName">The name of the requested ambient property.</param>
		/// <returns>
		///     <see langword="true" /> if <paramref name="propertyName" /> is available; otherwise, <see langword="false" />. </returns>
		// Token: 0x0600052A RID: 1322 RVA: 0x0000E647 File Offset: 0x0000C847
		bool IQueryAmbient.IsAmbientPropertyAvailable(string propertyName)
		{
			return propertyName != "Resources" || this.HasResources;
		}

		/// <summary>Returns whether serialization processes should serialize the contents of the <see cref="P:System.Windows.FrameworkElement.Resources" /> property. </summary>
		/// <returns>
		///     <see langword="true" /> if the <see cref="P:System.Windows.FrameworkElement.Resources" /> property value should be serialized; otherwise, <see langword="false" />.</returns>
		// Token: 0x0600052B RID: 1323 RVA: 0x0000E65E File Offset: 0x0000C85E
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool ShouldSerializeResources()
		{
			return this.Resources != null && this.Resources.Count != 0;
		}

		/// <summary>Returns the named element in the visual tree of an instantiated <see cref="T:System.Windows.Controls.ControlTemplate" />.</summary>
		/// <param name="childName">Name of the child to find.</param>
		/// <returns>The requested element. May be <see langword="null" /> if no element of the requested name exists.</returns>
		// Token: 0x0600052C RID: 1324 RVA: 0x0000E678 File Offset: 0x0000C878
		protected internal DependencyObject GetTemplateChild(string childName)
		{
			FrameworkTemplate templateInternal = this.TemplateInternal;
			if (templateInternal == null)
			{
				return null;
			}
			return StyleHelper.FindNameInTemplateContent(this, childName, templateInternal) as DependencyObject;
		}

		/// <summary>Searches for a resource with the specified key, and throws an exception if the requested resource is not found. </summary>
		/// <param name="resourceKey">The key identifier for the requested resource.</param>
		/// <returns>The requested resource. If no resource with the provided key was found, an exception is thrown. An <see cref="F:System.Windows.DependencyProperty.UnsetValue" /> value might also be returned in the exception case.</returns>
		/// <exception cref="T:System.Windows.ResourceReferenceKeyNotFoundException">
		///         <paramref name="resourceKey" /> was not found and an event handler does not exist for the <see cref="E:System.Windows.Threading.Dispatcher.UnhandledException" /> event.-or-
		///         <paramref name="resourceKey" /> was not found and the <see cref="P:System.Windows.Threading.DispatcherUnhandledExceptionEventArgs.Handled" /> property is <see langword="false" /> in the <see cref="E:System.Windows.Threading.Dispatcher.UnhandledException" /> event.</exception>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="resourceKey" /> is <see langword="null" />.</exception>
		// Token: 0x0600052D RID: 1325 RVA: 0x0000E6A0 File Offset: 0x0000C8A0
		public object FindResource(object resourceKey)
		{
			if (resourceKey == null)
			{
				throw new ArgumentNullException("resourceKey");
			}
			object obj = FrameworkElement.FindResourceInternal(this, null, resourceKey);
			if (obj == DependencyProperty.UnsetValue)
			{
				Helper.ResourceFailureThrow(resourceKey);
			}
			return obj;
		}

		/// <summary>Searches for a resource with the specified key, and returns that resource if found. </summary>
		/// <param name="resourceKey">The key identifier of the resource to be found.</param>
		/// <returns>The found resource, or <see langword="null" /> if no resource with the provided <paramref name="key" /> is found.</returns>
		// Token: 0x0600052E RID: 1326 RVA: 0x0000E6D4 File Offset: 0x0000C8D4
		public object TryFindResource(object resourceKey)
		{
			if (resourceKey == null)
			{
				throw new ArgumentNullException("resourceKey");
			}
			object obj = FrameworkElement.FindResourceInternal(this, null, resourceKey);
			if (obj == DependencyProperty.UnsetValue)
			{
				obj = null;
			}
			return obj;
		}

		// Token: 0x0600052F RID: 1327 RVA: 0x0000E704 File Offset: 0x0000C904
		internal static object FindImplicitStyleResource(FrameworkElement fe, object resourceKey, out object source)
		{
			if (fe.ShouldLookupImplicitStyles)
			{
				object unlinkedParent = null;
				bool allowDeferredResourceReference = false;
				bool mustReturnDeferredResourceReference = false;
				bool isImplicitStyleLookup = true;
				DependencyObject boundaryElement = null;
				if (!(fe is Control))
				{
					boundaryElement = fe.TemplatedParent;
				}
				return FrameworkElement.FindResourceInternal(fe, null, FrameworkElement.StyleProperty, resourceKey, unlinkedParent, allowDeferredResourceReference, mustReturnDeferredResourceReference, boundaryElement, isImplicitStyleLookup, out source);
			}
			source = null;
			return DependencyProperty.UnsetValue;
		}

		// Token: 0x06000530 RID: 1328 RVA: 0x0000E758 File Offset: 0x0000C958
		internal static object FindImplicitStyleResource(FrameworkContentElement fce, object resourceKey, out object source)
		{
			if (fce.ShouldLookupImplicitStyles)
			{
				object unlinkedParent = null;
				bool allowDeferredResourceReference = false;
				bool mustReturnDeferredResourceReference = false;
				bool isImplicitStyleLookup = true;
				DependencyObject templatedParent = fce.TemplatedParent;
				return FrameworkElement.FindResourceInternal(null, fce, FrameworkContentElement.StyleProperty, resourceKey, unlinkedParent, allowDeferredResourceReference, mustReturnDeferredResourceReference, templatedParent, isImplicitStyleLookup, out source);
			}
			source = null;
			return DependencyProperty.UnsetValue;
		}

		// Token: 0x06000531 RID: 1329 RVA: 0x0000E7A0 File Offset: 0x0000C9A0
		internal static object FindResourceInternal(FrameworkElement fe, FrameworkContentElement fce, object resourceKey)
		{
			object obj;
			return FrameworkElement.FindResourceInternal(fe, fce, null, resourceKey, null, false, false, null, false, out obj);
		}

		// Token: 0x06000532 RID: 1330 RVA: 0x0000E7C0 File Offset: 0x0000C9C0
		internal static object FindResourceFromAppOrSystem(object resourceKey, out object source, bool disableThrowOnResourceNotFound, bool allowDeferredResourceReference, bool mustReturnDeferredResourceReference)
		{
			return FrameworkElement.FindResourceInternal(null, null, null, resourceKey, null, allowDeferredResourceReference, mustReturnDeferredResourceReference, null, disableThrowOnResourceNotFound, out source);
		}

		// Token: 0x06000533 RID: 1331 RVA: 0x0000E7E0 File Offset: 0x0000C9E0
		internal static object FindResourceInternal(FrameworkElement fe, FrameworkContentElement fce, DependencyProperty dp, object resourceKey, object unlinkedParent, bool allowDeferredResourceReference, bool mustReturnDeferredResourceReference, DependencyObject boundaryElement, bool isImplicitStyleLookup, out object source)
		{
			InheritanceBehavior inheritanceBehavior = InheritanceBehavior.Default;
			if (TraceResourceDictionary.IsEnabled)
			{
				FrameworkObject frameworkObject = new FrameworkObject(fe, fce);
				TraceResourceDictionary.Trace(TraceEventType.Start, TraceResourceDictionary.FindResource, frameworkObject.DO, resourceKey);
			}
			try
			{
				if (fe != null || fce != null || unlinkedParent != null)
				{
					object obj = FrameworkElement.FindResourceInTree(fe, fce, dp, resourceKey, unlinkedParent, allowDeferredResourceReference, mustReturnDeferredResourceReference, boundaryElement, out inheritanceBehavior, out source);
					if (obj != DependencyProperty.UnsetValue)
					{
						return obj;
					}
				}
				Application application = Application.Current;
				if (application != null && (inheritanceBehavior == InheritanceBehavior.Default || inheritanceBehavior == InheritanceBehavior.SkipToAppNow || inheritanceBehavior == InheritanceBehavior.SkipToAppNext))
				{
					object obj = application.FindResourceInternal(resourceKey, allowDeferredResourceReference, mustReturnDeferredResourceReference);
					if (obj != null)
					{
						source = application;
						if (TraceResourceDictionary.IsEnabled)
						{
							TraceResourceDictionary.TraceActivityItem(TraceResourceDictionary.FoundResourceInApplication, resourceKey, obj);
						}
						return obj;
					}
				}
				if (!isImplicitStyleLookup && inheritanceBehavior != InheritanceBehavior.SkipAllNow && inheritanceBehavior != InheritanceBehavior.SkipAllNext)
				{
					object obj = SystemResources.FindResourceInternal(resourceKey, allowDeferredResourceReference, mustReturnDeferredResourceReference);
					if (obj != null)
					{
						source = SystemResourceHost.Instance;
						if (TraceResourceDictionary.IsEnabled)
						{
							TraceResourceDictionary.TraceActivityItem(TraceResourceDictionary.FoundResourceInTheme, source, resourceKey, obj);
						}
						return obj;
					}
				}
			}
			finally
			{
				if (TraceResourceDictionary.IsEnabled)
				{
					FrameworkObject frameworkObject2 = new FrameworkObject(fe, fce);
					TraceResourceDictionary.Trace(TraceEventType.Stop, TraceResourceDictionary.FindResource, frameworkObject2.DO, resourceKey);
				}
			}
			if (TraceResourceDictionary.IsEnabledOverride && !isImplicitStyleLookup)
			{
				if ((fe != null && fe.IsLoaded) || (fce != null && fce.IsLoaded))
				{
					TraceResourceDictionary.Trace(TraceEventType.Warning, TraceResourceDictionary.ResourceNotFound, resourceKey);
				}
				else if (TraceResourceDictionary.IsEnabled)
				{
					TraceResourceDictionary.TraceActivityItem(TraceResourceDictionary.ResourceNotFound, resourceKey);
				}
			}
			source = null;
			return DependencyProperty.UnsetValue;
		}

		// Token: 0x06000534 RID: 1332 RVA: 0x0000E950 File Offset: 0x0000CB50
		internal static object FindResourceInTree(FrameworkElement feStart, FrameworkContentElement fceStart, DependencyProperty dp, object resourceKey, object unlinkedParent, bool allowDeferredResourceReference, bool mustReturnDeferredResourceReference, DependencyObject boundaryElement, out InheritanceBehavior inheritanceBehavior, out object source)
		{
			FrameworkObject frameworkObject = new FrameworkObject(feStart, fceStart);
			FrameworkObject frameworkObject2 = frameworkObject;
			int num = 0;
			bool flag = true;
			inheritanceBehavior = InheritanceBehavior.Default;
			while (flag)
			{
				if (num > ContextLayoutManager.s_LayoutRecursionLimit)
				{
					throw new InvalidOperationException(SR.Get("LogicalTreeLoop"));
				}
				num++;
				Style style = null;
				FrameworkTemplate frameworkTemplate = null;
				Style style2 = null;
				if (frameworkObject2.IsFE)
				{
					FrameworkElement fe = frameworkObject2.FE;
					object obj = fe.FindResourceOnSelf(resourceKey, allowDeferredResourceReference, mustReturnDeferredResourceReference);
					if (obj != DependencyProperty.UnsetValue)
					{
						source = fe;
						if (TraceResourceDictionary.IsEnabled)
						{
							TraceResourceDictionary.TraceActivityItem(TraceResourceDictionary.FoundResourceOnElement, source, resourceKey, obj);
						}
						return obj;
					}
					if (fe != frameworkObject.FE || StyleHelper.ShouldGetValueFromStyle(dp))
					{
						style = fe.Style;
					}
					if (fe != frameworkObject.FE || StyleHelper.ShouldGetValueFromTemplate(dp))
					{
						frameworkTemplate = fe.TemplateInternal;
					}
					if (fe != frameworkObject.FE || StyleHelper.ShouldGetValueFromThemeStyle(dp))
					{
						style2 = fe.ThemeStyle;
					}
				}
				else if (frameworkObject2.IsFCE)
				{
					FrameworkContentElement fce = frameworkObject2.FCE;
					object obj = fce.FindResourceOnSelf(resourceKey, allowDeferredResourceReference, mustReturnDeferredResourceReference);
					if (obj != DependencyProperty.UnsetValue)
					{
						source = fce;
						if (TraceResourceDictionary.IsEnabled)
						{
							TraceResourceDictionary.TraceActivityItem(TraceResourceDictionary.FoundResourceOnElement, source, resourceKey, obj);
						}
						return obj;
					}
					if (fce != frameworkObject.FCE || StyleHelper.ShouldGetValueFromStyle(dp))
					{
						style = fce.Style;
					}
					if (fce != frameworkObject.FCE || StyleHelper.ShouldGetValueFromThemeStyle(dp))
					{
						style2 = fce.ThemeStyle;
					}
				}
				if (style != null)
				{
					object obj = style.FindResource(resourceKey, allowDeferredResourceReference, mustReturnDeferredResourceReference);
					if (obj != DependencyProperty.UnsetValue)
					{
						source = style;
						if (TraceResourceDictionary.IsEnabled)
						{
							TraceResourceDictionary.TraceActivityItem(TraceResourceDictionary.FoundResourceInStyle, new object[]
							{
								style.Resources,
								resourceKey,
								style,
								frameworkObject2.DO,
								obj
							});
						}
						return obj;
					}
				}
				if (frameworkTemplate != null)
				{
					object obj = frameworkTemplate.FindResource(resourceKey, allowDeferredResourceReference, mustReturnDeferredResourceReference);
					if (obj != DependencyProperty.UnsetValue)
					{
						source = frameworkTemplate;
						if (TraceResourceDictionary.IsEnabled)
						{
							TraceResourceDictionary.TraceActivityItem(TraceResourceDictionary.FoundResourceInTemplate, new object[]
							{
								frameworkTemplate.Resources,
								resourceKey,
								frameworkTemplate,
								frameworkObject2.DO,
								obj
							});
						}
						return obj;
					}
				}
				if (style2 != null)
				{
					object obj = style2.FindResource(resourceKey, allowDeferredResourceReference, mustReturnDeferredResourceReference);
					if (obj != DependencyProperty.UnsetValue)
					{
						source = style2;
						if (TraceResourceDictionary.IsEnabled)
						{
							TraceResourceDictionary.TraceActivityItem(TraceResourceDictionary.FoundResourceInThemeStyle, new object[]
							{
								style2.Resources,
								resourceKey,
								style2,
								frameworkObject2.DO,
								obj
							});
						}
						return obj;
					}
				}
				if (boundaryElement != null && frameworkObject2.DO == boundaryElement)
				{
					break;
				}
				if (frameworkObject2.IsValid && TreeWalkHelper.SkipNext(frameworkObject2.InheritanceBehavior))
				{
					inheritanceBehavior = frameworkObject2.InheritanceBehavior;
					break;
				}
				if (unlinkedParent != null)
				{
					DependencyObject dependencyObject = unlinkedParent as DependencyObject;
					if (dependencyObject != null)
					{
						frameworkObject2.Reset(dependencyObject);
						if (frameworkObject2.IsValid)
						{
							flag = true;
						}
						else
						{
							DependencyObject frameworkParent = FrameworkElement.GetFrameworkParent(unlinkedParent);
							if (frameworkParent != null)
							{
								frameworkObject2.Reset(frameworkParent);
								flag = true;
							}
							else
							{
								flag = false;
							}
						}
					}
					else
					{
						flag = false;
					}
					unlinkedParent = null;
				}
				else
				{
					frameworkObject2 = frameworkObject2.FrameworkParent;
					flag = frameworkObject2.IsValid;
				}
				if (frameworkObject2.IsValid && TreeWalkHelper.SkipNow(frameworkObject2.InheritanceBehavior))
				{
					inheritanceBehavior = frameworkObject2.InheritanceBehavior;
					break;
				}
			}
			source = null;
			return DependencyProperty.UnsetValue;
		}

		// Token: 0x06000535 RID: 1333 RVA: 0x0000EC88 File Offset: 0x0000CE88
		internal static object FindTemplateResourceInternal(DependencyObject target, object item, Type templateType)
		{
			if (item == null || item is UIElement)
			{
				return null;
			}
			Type type;
			object dataType = ContentPresenter.DataTypeForItem(item, target, out type);
			ArrayList arrayList = new ArrayList();
			int num = -1;
			while (dataType != null)
			{
				object obj = null;
				if (templateType == typeof(ItemContainerTemplate))
				{
					obj = new ItemContainerTemplateKey(dataType);
				}
				else if (templateType == typeof(DataTemplate))
				{
					obj = new DataTemplateKey(dataType);
				}
				if (obj != null)
				{
					arrayList.Add(obj);
				}
				if (num == -1)
				{
					num = arrayList.Count;
				}
				if (type != null)
				{
					type = type.BaseType;
					if (type == typeof(object))
					{
						type = null;
					}
				}
				dataType = type;
			}
			int count = arrayList.Count;
			object result = FrameworkElement.FindTemplateResourceInTree(target, arrayList, num, ref count);
			if (count >= num)
			{
				object obj2 = Helper.FindTemplateResourceFromAppOrSystem(target, arrayList, num, ref count);
				if (obj2 != null)
				{
					result = obj2;
				}
			}
			return result;
		}

		// Token: 0x06000536 RID: 1334 RVA: 0x0000ED60 File Offset: 0x0000CF60
		private static object FindTemplateResourceInTree(DependencyObject target, ArrayList keys, int exactMatch, ref int bestMatch)
		{
			object result = null;
			FrameworkObject frameworkParent = new FrameworkObject(target);
			while (frameworkParent.IsValid)
			{
				ResourceDictionary resourceDictionary = FrameworkElement.GetInstanceResourceDictionary(frameworkParent.FE, frameworkParent.FCE);
				if (resourceDictionary != null)
				{
					object obj = FrameworkElement.FindBestMatchInResourceDictionary(resourceDictionary, keys, exactMatch, ref bestMatch);
					if (obj != null)
					{
						result = obj;
						if (bestMatch < exactMatch)
						{
							return result;
						}
					}
				}
				resourceDictionary = FrameworkElement.GetStyleResourceDictionary(frameworkParent.FE, frameworkParent.FCE);
				if (resourceDictionary != null)
				{
					object obj = FrameworkElement.FindBestMatchInResourceDictionary(resourceDictionary, keys, exactMatch, ref bestMatch);
					if (obj != null)
					{
						result = obj;
						if (bestMatch < exactMatch)
						{
							return result;
						}
					}
				}
				resourceDictionary = FrameworkElement.GetThemeStyleResourceDictionary(frameworkParent.FE, frameworkParent.FCE);
				if (resourceDictionary != null)
				{
					object obj = FrameworkElement.FindBestMatchInResourceDictionary(resourceDictionary, keys, exactMatch, ref bestMatch);
					if (obj != null)
					{
						result = obj;
						if (bestMatch < exactMatch)
						{
							return result;
						}
					}
				}
				resourceDictionary = FrameworkElement.GetTemplateResourceDictionary(frameworkParent.FE, frameworkParent.FCE);
				if (resourceDictionary != null)
				{
					object obj = FrameworkElement.FindBestMatchInResourceDictionary(resourceDictionary, keys, exactMatch, ref bestMatch);
					if (obj != null)
					{
						result = obj;
						if (bestMatch < exactMatch)
						{
							return result;
						}
					}
				}
				if (frameworkParent.IsValid && TreeWalkHelper.SkipNext(frameworkParent.InheritanceBehavior))
				{
					break;
				}
				frameworkParent = frameworkParent.FrameworkParent;
				if (frameworkParent.IsValid && TreeWalkHelper.SkipNext(frameworkParent.InheritanceBehavior))
				{
					break;
				}
			}
			return result;
		}

		// Token: 0x06000537 RID: 1335 RVA: 0x0000EE74 File Offset: 0x0000D074
		private static object FindBestMatchInResourceDictionary(ResourceDictionary table, ArrayList keys, int exactMatch, ref int bestMatch)
		{
			object result = null;
			if (table != null)
			{
				for (int i = 0; i < bestMatch; i++)
				{
					object obj = table[keys[i]];
					if (obj != null)
					{
						result = obj;
						bestMatch = i;
						if (bestMatch < exactMatch)
						{
							return result;
						}
					}
				}
			}
			return result;
		}

		// Token: 0x06000538 RID: 1336 RVA: 0x0000EEB4 File Offset: 0x0000D0B4
		private static ResourceDictionary GetInstanceResourceDictionary(FrameworkElement fe, FrameworkContentElement fce)
		{
			ResourceDictionary result = null;
			if (fe != null)
			{
				if (fe.HasResources)
				{
					result = fe.Resources;
				}
			}
			else if (fce.HasResources)
			{
				result = fce.Resources;
			}
			return result;
		}

		// Token: 0x06000539 RID: 1337 RVA: 0x0000EEE8 File Offset: 0x0000D0E8
		private static ResourceDictionary GetStyleResourceDictionary(FrameworkElement fe, FrameworkContentElement fce)
		{
			ResourceDictionary result = null;
			if (fe != null)
			{
				if (fe.Style != null && fe.Style._resources != null)
				{
					result = fe.Style._resources;
				}
			}
			else if (fce.Style != null && fce.Style._resources != null)
			{
				result = fce.Style._resources;
			}
			return result;
		}

		// Token: 0x0600053A RID: 1338 RVA: 0x0000EF40 File Offset: 0x0000D140
		private static ResourceDictionary GetThemeStyleResourceDictionary(FrameworkElement fe, FrameworkContentElement fce)
		{
			ResourceDictionary result = null;
			if (fe != null)
			{
				if (fe.ThemeStyle != null && fe.ThemeStyle._resources != null)
				{
					result = fe.ThemeStyle._resources;
				}
			}
			else if (fce.ThemeStyle != null && fce.ThemeStyle._resources != null)
			{
				result = fce.ThemeStyle._resources;
			}
			return result;
		}

		// Token: 0x0600053B RID: 1339 RVA: 0x0000EF98 File Offset: 0x0000D198
		private static ResourceDictionary GetTemplateResourceDictionary(FrameworkElement fe, FrameworkContentElement fce)
		{
			ResourceDictionary result = null;
			if (fe != null && fe.TemplateInternal != null && fe.TemplateInternal._resources != null)
			{
				result = fe.TemplateInternal._resources;
			}
			return result;
		}

		// Token: 0x0600053C RID: 1340 RVA: 0x0000EFCC File Offset: 0x0000D1CC
		internal bool HasNonDefaultValue(DependencyProperty dp)
		{
			return !Helper.HasDefaultValue(this, dp);
		}

		// Token: 0x0600053D RID: 1341 RVA: 0x0000EFD8 File Offset: 0x0000D1D8
		internal static INameScope FindScope(DependencyObject d)
		{
			DependencyObject dependencyObject;
			return FrameworkElement.FindScope(d, out dependencyObject);
		}

		// Token: 0x0600053E RID: 1342 RVA: 0x0000EFF0 File Offset: 0x0000D1F0
		internal static INameScope FindScope(DependencyObject d, out DependencyObject scopeOwner)
		{
			while (d != null)
			{
				INameScope nameScope = NameScope.NameScopeFromObject(d);
				if (nameScope != null)
				{
					scopeOwner = d;
					return nameScope;
				}
				DependencyObject parent = LogicalTreeHelper.GetParent(d);
				d = ((parent != null) ? parent : Helper.FindMentor(d.InheritanceContext));
			}
			scopeOwner = null;
			return null;
		}

		/// <summary>Searches for a resource with the specified name and sets up a resource reference to it for the specified property. </summary>
		/// <param name="dp">The property to which the resource is bound.</param>
		/// <param name="name">The name of the resource.</param>
		// Token: 0x0600053F RID: 1343 RVA: 0x0000F02F File Offset: 0x0000D22F
		public void SetResourceReference(DependencyProperty dp, object name)
		{
			base.SetValue(dp, new ResourceReferenceExpression(name));
			this.HasResourceReference = true;
		}

		// Token: 0x06000540 RID: 1344 RVA: 0x0000F045 File Offset: 0x0000D245
		internal sealed override void EvaluateBaseValueCore(DependencyProperty dp, PropertyMetadata metadata, ref EffectiveValueEntry newEntry)
		{
			if (dp == FrameworkElement.StyleProperty)
			{
				this.HasStyleEverBeenFetched = true;
				this.HasImplicitStyleFromResources = false;
				this.IsStyleSetFromGenerator = false;
			}
			this.GetRawValue(dp, metadata, ref newEntry);
			Storyboard.GetComplexPathValue(this, dp, ref newEntry, metadata);
		}

		// Token: 0x06000541 RID: 1345 RVA: 0x0000F078 File Offset: 0x0000D278
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
			if (dp != FrameworkElement.StyleProperty)
			{
				if (StyleHelper.GetValueFromStyleOrTemplate(new FrameworkObject(this, null), dp, ref entry))
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
			if (frameworkPropertyMetadata != null && frameworkPropertyMetadata.Inherits)
			{
				object inheritableValue = this.GetInheritableValue(dp, frameworkPropertyMetadata);
				if (inheritableValue != DependencyProperty.UnsetValue)
				{
					entry.BaseValueSourceInternal = BaseValueSourceInternal.Inherited;
					entry.Value = inheritableValue;
					return;
				}
			}
		}

		// Token: 0x06000542 RID: 1346 RVA: 0x0000F138 File Offset: 0x0000D338
		private bool GetValueFromTemplatedParent(DependencyProperty dp, ref EffectiveValueEntry entry)
		{
			FrameworkElement frameworkElement = (FrameworkElement)this._templatedParent;
			FrameworkTemplate templateInternal = frameworkElement.TemplateInternal;
			return templateInternal != null && StyleHelper.GetValueFromTemplatedParent(this._templatedParent, this.TemplateChildIndex, new FrameworkObject(this, null), dp, ref templateInternal.ChildRecordFromChildIndex, templateInternal.VisualTree, ref entry);
		}

		// Token: 0x06000543 RID: 1347 RVA: 0x0000F188 File Offset: 0x0000D388
		private object GetInheritableValue(DependencyProperty dp, FrameworkPropertyMetadata fmetadata)
		{
			if (!TreeWalkHelper.SkipNext(this.InheritanceBehavior) || fmetadata.OverridesInheritanceBehavior)
			{
				InheritanceBehavior inheritanceBehavior = InheritanceBehavior.Default;
				FrameworkElement frameworkElement;
				FrameworkContentElement frameworkContentElement;
				bool frameworkParent = FrameworkElement.GetFrameworkParent(this, out frameworkElement, out frameworkContentElement);
				while (frameworkParent)
				{
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
							string text = string.Format(CultureInfo.InvariantCulture, "[{0}]{1}({2})", new object[]
							{
								base.GetType().Name,
								dp.Name,
								base.GetHashCode()
							});
							EventTrace.EventProvider.TraceEvent(EventTrace.Event.WClientPropParentCheck, EventTrace.Keyword.KeywordGeneral, EventTrace.Level.Verbose, new object[]
							{
								base.GetHashCode(),
								text
							});
						}
						DependencyObject dependencyObject = frameworkElement;
						if (dependencyObject == null)
						{
							dependencyObject = frameworkContentElement;
						}
						EntryIndex entryIndex = dependencyObject.LookupEntry(dp.GlobalIndex);
						return dependencyObject.GetValueEntry(entryIndex, dp, fmetadata, (RequestFlags)12).Value;
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
			return DependencyProperty.UnsetValue;
		}

		// Token: 0x06000544 RID: 1348 RVA: 0x0000F2B8 File Offset: 0x0000D4B8
		internal Expression GetExpressionCore(DependencyProperty dp, PropertyMetadata metadata)
		{
			this.IsRequestingExpression = true;
			EffectiveValueEntry effectiveValueEntry = new EffectiveValueEntry(dp);
			effectiveValueEntry.Value = DependencyProperty.UnsetValue;
			this.EvaluateBaseValueCore(dp, metadata, ref effectiveValueEntry);
			this.IsRequestingExpression = false;
			return effectiveValueEntry.Value as Expression;
		}

		/// <summary>Invoked whenever the effective value of any dependency property on this <see cref="T:System.Windows.FrameworkElement" /> has been updated. The specific dependency property that changed is reported in the arguments parameter. Overrides <see cref="M:System.Windows.DependencyObject.OnPropertyChanged(System.Windows.DependencyPropertyChangedEventArgs)" />.</summary>
		/// <param name="e">The event data that describes the property that changed, as well as old and new values.</param>
		// Token: 0x06000545 RID: 1349 RVA: 0x0000F300 File Offset: 0x0000D500
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
				if (property != FrameworkElement.StyleProperty && property != Control.TemplateProperty && property != FrameworkElement.DefaultStyleKeyProperty)
				{
					if (this.TemplatedParent != null)
					{
						FrameworkElement frameworkElement = this.TemplatedParent as FrameworkElement;
						FrameworkTemplate templateInternal = frameworkElement.TemplateInternal;
						if (templateInternal != null)
						{
							StyleHelper.OnTriggerSourcePropertyInvalidated(null, templateInternal, this.TemplatedParent, property, e, false, ref templateInternal.TriggerSourceRecordFromChildIndex, ref templateInternal.PropertyTriggersWithActions, this.TemplateChildIndex);
						}
					}
					if (this.Style != null)
					{
						StyleHelper.OnTriggerSourcePropertyInvalidated(this.Style, null, this, property, e, true, ref this.Style.TriggerSourceRecordFromChildIndex, ref this.Style.PropertyTriggersWithActions, 0);
					}
					if (this.TemplateInternal != null)
					{
						StyleHelper.OnTriggerSourcePropertyInvalidated(null, this.TemplateInternal, this, property, e, !this.HasTemplateGeneratedSubTree, ref this.TemplateInternal.TriggerSourceRecordFromChildIndex, ref this.TemplateInternal.PropertyTriggersWithActions, 0);
					}
					if (this.ThemeStyle != null && this.Style != this.ThemeStyle)
					{
						StyleHelper.OnTriggerSourcePropertyInvalidated(this.ThemeStyle, null, this, property, e, true, ref this.ThemeStyle.TriggerSourceRecordFromChildIndex, ref this.ThemeStyle.PropertyTriggersWithActions, 0);
					}
				}
			}
			FrameworkPropertyMetadata frameworkPropertyMetadata = e.Metadata as FrameworkPropertyMetadata;
			if (frameworkPropertyMetadata != null)
			{
				if (frameworkPropertyMetadata.Inherits && (this.InheritanceBehavior == InheritanceBehavior.Default || frameworkPropertyMetadata.OverridesInheritanceBehavior) && (!DependencyObject.IsTreeWalkOperation(e.OperationType) || this.PotentiallyHasMentees))
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
						TreeWalkHelper.InvalidateOnInheritablePropertyChange(this, null, info, true);
					}
					if (this.PotentiallyHasMentees)
					{
						TreeWalkHelper.OnInheritedPropertyChanged(this, ref info, this.InheritanceBehavior);
					}
				}
				if ((e.IsAValueChange || e.IsASubPropertyChange) && (!this.AncestorChangeInProgress || !this.InVisibilityCollapsedTree))
				{
					bool affectsParentMeasure = frameworkPropertyMetadata.AffectsParentMeasure;
					bool affectsParentArrange = frameworkPropertyMetadata.AffectsParentArrange;
					bool affectsMeasure = frameworkPropertyMetadata.AffectsMeasure;
					bool affectsArrange = frameworkPropertyMetadata.AffectsArrange;
					if (affectsMeasure || affectsArrange || affectsParentArrange || affectsParentMeasure)
					{
						Visual visual = VisualTreeHelper.GetParent(this) as Visual;
						while (visual != null)
						{
							UIElement uielement = visual as UIElement;
							if (uielement != null)
							{
								if (FrameworkElement.DType.IsInstanceOfType(uielement))
								{
									((FrameworkElement)uielement).ParentLayoutInvalidated(this);
								}
								if (affectsParentMeasure)
								{
									uielement.InvalidateMeasure();
								}
								if (affectsParentArrange)
								{
									uielement.InvalidateArrange();
									break;
								}
								break;
							}
							else
							{
								visual = (VisualTreeHelper.GetParent(visual) as Visual);
							}
						}
					}
					if (frameworkPropertyMetadata.AffectsMeasure && (!this.BypassLayoutPolicies || (property != FrameworkElement.WidthProperty && property != FrameworkElement.HeightProperty)))
					{
						base.InvalidateMeasure();
					}
					if (frameworkPropertyMetadata.AffectsArrange)
					{
						base.InvalidateArrange();
					}
					if (frameworkPropertyMetadata.AffectsRender && (e.IsAValueChange || !frameworkPropertyMetadata.SubPropertiesDoNotAffectRender))
					{
						base.InvalidateVisual();
					}
				}
			}
		}

		// Token: 0x06000546 RID: 1350 RVA: 0x0000F6B8 File Offset: 0x0000D8B8
		internal static DependencyObject GetFrameworkParent(object current)
		{
			FrameworkObject frameworkParent = new FrameworkObject(current as DependencyObject);
			frameworkParent = frameworkParent.FrameworkParent;
			return frameworkParent.DO;
		}

		// Token: 0x06000547 RID: 1351 RVA: 0x0000F6E4 File Offset: 0x0000D8E4
		internal static bool GetFrameworkParent(FrameworkElement current, out FrameworkElement feParent, out FrameworkContentElement fceParent)
		{
			FrameworkObject frameworkParent = new FrameworkObject(current, null);
			frameworkParent = frameworkParent.FrameworkParent;
			feParent = frameworkParent.FE;
			fceParent = frameworkParent.FCE;
			return frameworkParent.IsValid;
		}

		// Token: 0x06000548 RID: 1352 RVA: 0x0000F71C File Offset: 0x0000D91C
		internal static bool GetFrameworkParent(FrameworkContentElement current, out FrameworkElement feParent, out FrameworkContentElement fceParent)
		{
			FrameworkObject frameworkParent = new FrameworkObject(null, current);
			frameworkParent = frameworkParent.FrameworkParent;
			feParent = frameworkParent.FE;
			fceParent = frameworkParent.FCE;
			return frameworkParent.IsValid;
		}

		// Token: 0x06000549 RID: 1353 RVA: 0x0000F754 File Offset: 0x0000D954
		internal static bool GetContainingFrameworkElement(DependencyObject current, out FrameworkElement fe, out FrameworkContentElement fce)
		{
			FrameworkObject containingFrameworkElement = FrameworkObject.GetContainingFrameworkElement(current);
			if (containingFrameworkElement.IsValid)
			{
				fe = containingFrameworkElement.FE;
				fce = containingFrameworkElement.FCE;
				return true;
			}
			fe = null;
			fce = null;
			return false;
		}

		// Token: 0x0600054A RID: 1354 RVA: 0x0000F78C File Offset: 0x0000D98C
		internal static void GetTemplatedParentChildRecord(DependencyObject templatedParent, int childIndex, out ChildRecord childRecord, out bool isChildRecordValid)
		{
			isChildRecordValid = false;
			childRecord = default(ChildRecord);
			if (templatedParent != null)
			{
				FrameworkObject frameworkObject = new FrameworkObject(templatedParent, true);
				FrameworkTemplate templateInternal = frameworkObject.FE.TemplateInternal;
				if (templateInternal != null && 0 <= childIndex && childIndex < templateInternal.ChildRecordFromChildIndex.Count)
				{
					childRecord = templateInternal.ChildRecordFromChildIndex[childIndex];
					isChildRecordValid = true;
				}
			}
		}

		// Token: 0x0600054B RID: 1355 RVA: 0x0000C238 File Offset: 0x0000A438
		internal virtual string GetPlainText()
		{
			return null;
		}

		// Token: 0x0600054C RID: 1356 RVA: 0x0000F7E8 File Offset: 0x0000D9E8
		static FrameworkElement()
		{
			FrameworkElement.RequestBringIntoViewEvent = EventManager.RegisterRoutedEvent("RequestBringIntoView", RoutingStrategy.Bubble, typeof(RequestBringIntoViewEventHandler), FrameworkElement._typeofThis);
			FrameworkElement.SizeChangedEvent = EventManager.RegisterRoutedEvent("SizeChanged", RoutingStrategy.Direct, typeof(SizeChangedEventHandler), FrameworkElement._typeofThis);
			FrameworkElement._actualWidthMetadata = new ReadOnlyFrameworkPropertyMetadata(0.0, new GetReadOnlyValueCallback(FrameworkElement.GetActualWidth));
			FrameworkElement.ActualWidthPropertyKey = DependencyProperty.RegisterReadOnly("ActualWidth", typeof(double), FrameworkElement._typeofThis, FrameworkElement._actualWidthMetadata);
			FrameworkElement.ActualWidthProperty = FrameworkElement.ActualWidthPropertyKey.DependencyProperty;
			FrameworkElement._actualHeightMetadata = new ReadOnlyFrameworkPropertyMetadata(0.0, new GetReadOnlyValueCallback(FrameworkElement.GetActualHeight));
			FrameworkElement.ActualHeightPropertyKey = DependencyProperty.RegisterReadOnly("ActualHeight", typeof(double), FrameworkElement._typeofThis, FrameworkElement._actualHeightMetadata);
			FrameworkElement.ActualHeightProperty = FrameworkElement.ActualHeightPropertyKey.DependencyProperty;
			FrameworkElement.LayoutTransformProperty = DependencyProperty.Register("LayoutTransform", typeof(Transform), FrameworkElement._typeofThis, new FrameworkPropertyMetadata(Transform.Identity, FrameworkPropertyMetadataOptions.AffectsMeasure, new PropertyChangedCallback(FrameworkElement.OnLayoutTransformChanged)));
			FrameworkElement.WidthProperty = DependencyProperty.Register("Width", typeof(double), FrameworkElement._typeofThis, new FrameworkPropertyMetadata(double.NaN, FrameworkPropertyMetadataOptions.AffectsMeasure, new PropertyChangedCallback(FrameworkElement.OnTransformDirty)), new ValidateValueCallback(FrameworkElement.IsWidthHeightValid));
			FrameworkElement.MinWidthProperty = DependencyProperty.Register("MinWidth", typeof(double), FrameworkElement._typeofThis, new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsMeasure, new PropertyChangedCallback(FrameworkElement.OnTransformDirty)), new ValidateValueCallback(FrameworkElement.IsMinWidthHeightValid));
			FrameworkElement.MaxWidthProperty = DependencyProperty.Register("MaxWidth", typeof(double), FrameworkElement._typeofThis, new FrameworkPropertyMetadata(double.PositiveInfinity, FrameworkPropertyMetadataOptions.AffectsMeasure, new PropertyChangedCallback(FrameworkElement.OnTransformDirty)), new ValidateValueCallback(FrameworkElement.IsMaxWidthHeightValid));
			FrameworkElement.HeightProperty = DependencyProperty.Register("Height", typeof(double), FrameworkElement._typeofThis, new FrameworkPropertyMetadata(double.NaN, FrameworkPropertyMetadataOptions.AffectsMeasure, new PropertyChangedCallback(FrameworkElement.OnTransformDirty)), new ValidateValueCallback(FrameworkElement.IsWidthHeightValid));
			FrameworkElement.MinHeightProperty = DependencyProperty.Register("MinHeight", typeof(double), FrameworkElement._typeofThis, new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsMeasure, new PropertyChangedCallback(FrameworkElement.OnTransformDirty)), new ValidateValueCallback(FrameworkElement.IsMinWidthHeightValid));
			FrameworkElement.MaxHeightProperty = DependencyProperty.Register("MaxHeight", typeof(double), FrameworkElement._typeofThis, new FrameworkPropertyMetadata(double.PositiveInfinity, FrameworkPropertyMetadataOptions.AffectsMeasure, new PropertyChangedCallback(FrameworkElement.OnTransformDirty)), new ValidateValueCallback(FrameworkElement.IsMaxWidthHeightValid));
			FrameworkElement.FlowDirectionProperty = DependencyProperty.RegisterAttached("FlowDirection", typeof(FlowDirection), FrameworkElement._typeofThis, new FrameworkPropertyMetadata(FlowDirection.LeftToRight, FrameworkPropertyMetadataOptions.AffectsParentArrange | FrameworkPropertyMetadataOptions.Inherits, new PropertyChangedCallback(FrameworkElement.OnFlowDirectionChanged), new CoerceValueCallback(FrameworkElement.CoerceFlowDirectionProperty)), new ValidateValueCallback(FrameworkElement.IsValidFlowDirection));
			FrameworkElement.MarginProperty = DependencyProperty.Register("Margin", typeof(Thickness), FrameworkElement._typeofThis, new FrameworkPropertyMetadata(default(Thickness), FrameworkPropertyMetadataOptions.AffectsMeasure), new ValidateValueCallback(FrameworkElement.IsMarginValid));
			FrameworkElement.HorizontalAlignmentProperty = DependencyProperty.Register("HorizontalAlignment", typeof(HorizontalAlignment), FrameworkElement._typeofThis, new FrameworkPropertyMetadata(HorizontalAlignment.Stretch, FrameworkPropertyMetadataOptions.AffectsArrange), new ValidateValueCallback(FrameworkElement.ValidateHorizontalAlignmentValue));
			FrameworkElement.VerticalAlignmentProperty = DependencyProperty.Register("VerticalAlignment", typeof(VerticalAlignment), FrameworkElement._typeofThis, new FrameworkPropertyMetadata(VerticalAlignment.Stretch, FrameworkPropertyMetadataOptions.AffectsArrange), new ValidateValueCallback(FrameworkElement.ValidateVerticalAlignmentValue));
			FrameworkElement._defaultFocusVisualStyle = null;
			FrameworkElement.FocusVisualStyleProperty = DependencyProperty.Register("FocusVisualStyle", typeof(Style), FrameworkElement._typeofThis, new FrameworkPropertyMetadata(FrameworkElement.DefaultFocusVisualStyle));
			FrameworkElement.CursorProperty = DependencyProperty.Register("Cursor", typeof(Cursor), FrameworkElement._typeofThis, new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.None, new PropertyChangedCallback(FrameworkElement.OnCursorChanged)));
			FrameworkElement.ForceCursorProperty = DependencyProperty.Register("ForceCursor", typeof(bool), FrameworkElement._typeofThis, new FrameworkPropertyMetadata(BooleanBoxes.FalseBox, FrameworkPropertyMetadataOptions.None, new PropertyChangedCallback(FrameworkElement.OnForceCursorChanged)));
			FrameworkElement.InitializedKey = new EventPrivateKey();
			FrameworkElement.LoadedPendingPropertyKey = DependencyProperty.RegisterReadOnly("LoadedPending", typeof(object[]), FrameworkElement._typeofThis, new PropertyMetadata(null));
			FrameworkElement.LoadedPendingProperty = FrameworkElement.LoadedPendingPropertyKey.DependencyProperty;
			FrameworkElement.UnloadedPendingPropertyKey = DependencyProperty.RegisterReadOnly("UnloadedPending", typeof(object[]), FrameworkElement._typeofThis, new PropertyMetadata(null));
			FrameworkElement.UnloadedPendingProperty = FrameworkElement.UnloadedPendingPropertyKey.DependencyProperty;
			FrameworkElement.LoadedEvent = EventManager.RegisterRoutedEvent("Loaded", RoutingStrategy.Direct, typeof(RoutedEventHandler), FrameworkElement._typeofThis);
			FrameworkElement.UnloadedEvent = EventManager.RegisterRoutedEvent("Unloaded", RoutingStrategy.Direct, typeof(RoutedEventHandler), FrameworkElement._typeofThis);
			FrameworkElement.ToolTipProperty = ToolTipService.ToolTipProperty.AddOwner(FrameworkElement._typeofThis);
			FrameworkElement.ContextMenuProperty = ContextMenuService.ContextMenuProperty.AddOwner(FrameworkElement._typeofThis, new FrameworkPropertyMetadata(null));
			FrameworkElement.ToolTipOpeningEvent = ToolTipService.ToolTipOpeningEvent.AddOwner(FrameworkElement._typeofThis);
			FrameworkElement.ToolTipClosingEvent = ToolTipService.ToolTipClosingEvent.AddOwner(FrameworkElement._typeofThis);
			FrameworkElement.ContextMenuOpeningEvent = ContextMenuService.ContextMenuOpeningEvent.AddOwner(FrameworkElement._typeofThis);
			FrameworkElement.ContextMenuClosingEvent = ContextMenuService.ContextMenuClosingEvent.AddOwner(FrameworkElement._typeofThis);
			FrameworkElement.UnclippedDesiredSizeField = new UncommonField<SizeBox>();
			FrameworkElement.LayoutTransformDataField = new UncommonField<FrameworkElement.LayoutTransformData>();
			FrameworkElement.ResourcesField = new UncommonField<ResourceDictionary>();
			FrameworkElement.UIElementDType = DependencyObjectType.FromSystemTypeInternal(typeof(UIElement));
			FrameworkElement._controlDType = null;
			FrameworkElement._contentPresenterDType = null;
			FrameworkElement._pageFunctionBaseDType = null;
			FrameworkElement._pageDType = null;
			FrameworkElement.ResourcesChangedKey = new EventPrivateKey();
			FrameworkElement.InheritedPropertyChangedKey = new EventPrivateKey();
			FrameworkElement.DType = DependencyObjectType.FromSystemTypeInternal(typeof(FrameworkElement));
			FrameworkElement.InheritanceContextField = new UncommonField<DependencyObject>();
			FrameworkElement.MentorField = new UncommonField<DependencyObject>();
			UIElement.SnapsToDevicePixelsProperty.OverrideMetadata(FrameworkElement._typeofThis, new FrameworkPropertyMetadata(BooleanBoxes.FalseBox, FrameworkPropertyMetadataOptions.AffectsArrange | FrameworkPropertyMetadataOptions.Inherits));
			EventManager.RegisterClassHandler(FrameworkElement._typeofThis, Mouse.QueryCursorEvent, new QueryCursorEventHandler(FrameworkElement.OnQueryCursorOverride), true);
			EventManager.RegisterClassHandler(FrameworkElement._typeofThis, Keyboard.PreviewGotKeyboardFocusEvent, new KeyboardFocusChangedEventHandler(FrameworkElement.OnPreviewGotKeyboardFocus));
			EventManager.RegisterClassHandler(FrameworkElement._typeofThis, Keyboard.GotKeyboardFocusEvent, new KeyboardFocusChangedEventHandler(FrameworkElement.OnGotKeyboardFocus));
			EventManager.RegisterClassHandler(FrameworkElement._typeofThis, Keyboard.LostKeyboardFocusEvent, new KeyboardFocusChangedEventHandler(FrameworkElement.OnLostKeyboardFocus));
			UIElement.AllowDropProperty.OverrideMetadata(FrameworkElement._typeofThis, new FrameworkPropertyMetadata(BooleanBoxes.FalseBox, FrameworkPropertyMetadataOptions.Inherits));
			Stylus.IsPressAndHoldEnabledProperty.AddOwner(FrameworkElement._typeofThis, new FrameworkPropertyMetadata(BooleanBoxes.TrueBox, FrameworkPropertyMetadataOptions.Inherits));
			Stylus.IsFlicksEnabledProperty.AddOwner(FrameworkElement._typeofThis, new FrameworkPropertyMetadata(BooleanBoxes.TrueBox, FrameworkPropertyMetadataOptions.Inherits));
			Stylus.IsTapFeedbackEnabledProperty.AddOwner(FrameworkElement._typeofThis, new FrameworkPropertyMetadata(BooleanBoxes.TrueBox, FrameworkPropertyMetadataOptions.Inherits));
			Stylus.IsTouchFeedbackEnabledProperty.AddOwner(FrameworkElement._typeofThis, new FrameworkPropertyMetadata(BooleanBoxes.TrueBox, FrameworkPropertyMetadataOptions.Inherits));
			PropertyChangedCallback propertyChangedCallback = new PropertyChangedCallback(FrameworkElement.NumberSubstitutionChanged);
			NumberSubstitution.CultureSourceProperty.OverrideMetadata(FrameworkElement._typeofThis, new FrameworkPropertyMetadata(NumberCultureSource.User, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Inherits, propertyChangedCallback));
			NumberSubstitution.CultureOverrideProperty.OverrideMetadata(FrameworkElement._typeofThis, new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Inherits, propertyChangedCallback));
			NumberSubstitution.SubstitutionProperty.OverrideMetadata(FrameworkElement._typeofThis, new FrameworkPropertyMetadata(NumberSubstitutionMethod.AsCulture, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Inherits, propertyChangedCallback));
			EventManager.RegisterClassHandler(FrameworkElement._typeofThis, FrameworkElement.ToolTipOpeningEvent, new ToolTipEventHandler(FrameworkElement.OnToolTipOpeningThunk));
			EventManager.RegisterClassHandler(FrameworkElement._typeofThis, FrameworkElement.ToolTipClosingEvent, new ToolTipEventHandler(FrameworkElement.OnToolTipClosingThunk));
			EventManager.RegisterClassHandler(FrameworkElement._typeofThis, FrameworkElement.ContextMenuOpeningEvent, new ContextMenuEventHandler(FrameworkElement.OnContextMenuOpeningThunk));
			EventManager.RegisterClassHandler(FrameworkElement._typeofThis, FrameworkElement.ContextMenuClosingEvent, new ContextMenuEventHandler(FrameworkElement.OnContextMenuClosingThunk));
			TextElement.FontFamilyProperty.OverrideMetadata(FrameworkElement._typeofThis, new FrameworkPropertyMetadata(SystemFonts.MessageFontFamily, FrameworkPropertyMetadataOptions.Inherits, null, new CoerceValueCallback(FrameworkElement.CoerceFontFamily)));
			TextElement.FontSizeProperty.OverrideMetadata(FrameworkElement._typeofThis, new FrameworkPropertyMetadata(SystemFonts.MessageFontSize, FrameworkPropertyMetadataOptions.Inherits, null, new CoerceValueCallback(FrameworkElement.CoerceFontSize)));
			TextElement.FontStyleProperty.OverrideMetadata(FrameworkElement._typeofThis, new FrameworkPropertyMetadata(SystemFonts.MessageFontStyle, FrameworkPropertyMetadataOptions.Inherits, null, new CoerceValueCallback(FrameworkElement.CoerceFontStyle)));
			TextElement.FontWeightProperty.OverrideMetadata(FrameworkElement._typeofThis, new FrameworkPropertyMetadata(SystemFonts.MessageFontWeight, FrameworkPropertyMetadataOptions.Inherits, null, new CoerceValueCallback(FrameworkElement.CoerceFontWeight)));
			TextOptions.TextRenderingModeProperty.OverrideMetadata(typeof(FrameworkElement), new FrameworkPropertyMetadata(new PropertyChangedCallback(FrameworkElement.TextRenderingMode_Changed)));
		}

		// Token: 0x0600054D RID: 1357 RVA: 0x000102BC File Offset: 0x0000E4BC
		private static void TextRenderingMode_Changed(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			FrameworkElement frameworkElement = (FrameworkElement)d;
			frameworkElement.pushTextRenderingMode();
		}

		// Token: 0x0600054E RID: 1358 RVA: 0x000102D8 File Offset: 0x0000E4D8
		internal virtual void pushTextRenderingMode()
		{
			if (DependencyPropertyHelper.GetValueSource(this, TextOptions.TextRenderingModeProperty).BaseValueSource > BaseValueSource.Inherited)
			{
				base.VisualTextRenderingMode = TextOptions.GetTextRenderingMode(this);
			}
		}

		// Token: 0x0600054F RID: 1359 RVA: 0x00002137 File Offset: 0x00000337
		internal virtual void OnAncestorChanged()
		{
		}

		/// <summary>Invoked when the parent of this element in the visual tree is changed. Overrides <see cref="M:System.Windows.UIElement.OnVisualParentChanged(System.Windows.DependencyObject)" />.</summary>
		/// <param name="oldParent">The old parent element. May be <see langword="null" /> to indicate that the element did not have a visual parent previously.</param>
		// Token: 0x06000550 RID: 1360 RVA: 0x00010308 File Offset: 0x0000E508
		protected internal override void OnVisualParentChanged(DependencyObject oldParent)
		{
			DependencyObject parentInternal = VisualTreeHelper.GetParentInternal(this);
			if (parentInternal != null)
			{
				this.ClearInheritanceContext();
			}
			BroadcastEventHelper.AddOrRemoveHasLoadedChangeHandlerFlag(this, oldParent, parentInternal);
			BroadcastEventHelper.BroadcastLoadedOrUnloadedEvent(this, oldParent, parentInternal);
			if (parentInternal != null && !(parentInternal is FrameworkElement))
			{
				Visual visual = parentInternal as Visual;
				if (visual != null)
				{
					visual.VisualAncestorChanged += this.OnVisualAncestorChanged;
				}
				else if (parentInternal is Visual3D)
				{
					((Visual3D)parentInternal).VisualAncestorChanged += this.OnVisualAncestorChanged;
				}
			}
			else if (oldParent != null && !(oldParent is FrameworkElement))
			{
				Visual visual2 = oldParent as Visual;
				if (visual2 != null)
				{
					visual2.VisualAncestorChanged -= this.OnVisualAncestorChanged;
				}
				else if (oldParent is Visual3D)
				{
					((Visual3D)oldParent).VisualAncestorChanged -= this.OnVisualAncestorChanged;
				}
			}
			if (this.Parent == null)
			{
				DependencyObject parent = (parentInternal != null) ? parentInternal : oldParent;
				TreeWalkHelper.InvalidateOnTreeChange(this, null, parent, parentInternal != null);
			}
			this.TryFireInitialized();
			base.OnVisualParentChanged(oldParent);
		}

		// Token: 0x06000551 RID: 1361 RVA: 0x000103F0 File Offset: 0x0000E5F0
		internal new void OnVisualAncestorChanged(object sender, AncestorChangedEventArgs e)
		{
			FrameworkElement frameworkElement = null;
			FrameworkContentElement frameworkContentElement = null;
			FrameworkElement.GetContainingFrameworkElement(VisualTreeHelper.GetParent(this), out frameworkElement, out frameworkContentElement);
			if (e.OldParent == null)
			{
				if (frameworkElement == null || !VisualTreeHelper.IsAncestorOf(e.Ancestor, frameworkElement))
				{
					BroadcastEventHelper.AddOrRemoveHasLoadedChangeHandlerFlag(this, null, VisualTreeHelper.GetParent(e.Ancestor));
					BroadcastEventHelper.BroadcastLoadedOrUnloadedEvent(this, null, VisualTreeHelper.GetParent(e.Ancestor));
					return;
				}
			}
			else if (frameworkElement == null)
			{
				FrameworkElement.GetContainingFrameworkElement(e.OldParent, out frameworkElement, out frameworkContentElement);
				if (frameworkElement != null)
				{
					BroadcastEventHelper.AddOrRemoveHasLoadedChangeHandlerFlag(this, frameworkElement, null);
					BroadcastEventHelper.BroadcastLoadedOrUnloadedEvent(this, frameworkElement, null);
				}
			}
		}

		/// <summary>Gets or sets the scope limits for property value inheritance, resource key lookup, and RelativeSource FindAncestor lookup.</summary>
		/// <returns>A value of the enumeration. The default is <see cref="F:System.Windows.InheritanceBehavior.Default" />.</returns>
		// Token: 0x170000EB RID: 235
		// (get) Token: 0x06000552 RID: 1362 RVA: 0x00010478 File Offset: 0x0000E678
		// (set) Token: 0x06000553 RID: 1363 RVA: 0x00010494 File Offset: 0x0000E694
		protected internal InheritanceBehavior InheritanceBehavior
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
					TreeWalkHelper.InvalidateOnTreeChange(this, null, this._parent, true);
					return;
				}
			}
		}

		/// <summary>Occurs when the target value changes for any property binding on this element. </summary>
		// Token: 0x1400001B RID: 27
		// (add) Token: 0x06000554 RID: 1364 RVA: 0x00010506 File Offset: 0x0000E706
		// (remove) Token: 0x06000555 RID: 1365 RVA: 0x00010514 File Offset: 0x0000E714
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

		/// <summary>Occurs when the source value changes for any existing property binding on this element.</summary>
		// Token: 0x1400001C RID: 28
		// (add) Token: 0x06000556 RID: 1366 RVA: 0x00010522 File Offset: 0x0000E722
		// (remove) Token: 0x06000557 RID: 1367 RVA: 0x00010530 File Offset: 0x0000E730
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

		/// <summary>Occurs when the data context for this element changes. </summary>
		// Token: 0x1400001D RID: 29
		// (add) Token: 0x06000558 RID: 1368 RVA: 0x0001053E File Offset: 0x0000E73E
		// (remove) Token: 0x06000559 RID: 1369 RVA: 0x0001054C File Offset: 0x0000E74C
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

		/// <summary> Gets or sets the data context for an element when it participates in data binding.</summary>
		/// <returns>The object to use as data context.</returns>
		// Token: 0x170000EC RID: 236
		// (get) Token: 0x0600055A RID: 1370 RVA: 0x0001055A File Offset: 0x0000E75A
		// (set) Token: 0x0600055B RID: 1371 RVA: 0x00010567 File Offset: 0x0000E767
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Localizability(LocalizationCategory.NeverLocalize)]
		public object DataContext
		{
			get
			{
				return base.GetValue(FrameworkElement.DataContextProperty);
			}
			set
			{
				base.SetValue(FrameworkElement.DataContextProperty, value);
			}
		}

		// Token: 0x0600055C RID: 1372 RVA: 0x00010575 File Offset: 0x0000E775
		private static void OnDataContextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			if (e.NewValue == BindingExpressionBase.DisconnectedItem)
			{
				return;
			}
			((FrameworkElement)d).RaiseDependencyPropertyChanged(FrameworkElement.DataContextChangedKey, e);
		}

		/// <summary>Returns the <see cref="T:System.Windows.Data.BindingExpression" /> that represents the binding on the specified property. </summary>
		/// <param name="dp">The target <see cref="T:System.Windows.DependencyProperty" /> to get the binding from.</param>
		/// <returns>A <see cref="T:System.Windows.Data.BindingExpression" /> if the target property has an active binding; otherwise, returns <see langword="null" />.</returns>
		// Token: 0x0600055D RID: 1373 RVA: 0x0000CBCC File Offset: 0x0000ADCC
		public BindingExpression GetBindingExpression(DependencyProperty dp)
		{
			return BindingOperations.GetBindingExpression(this, dp);
		}

		/// <summary>Attaches a binding to this element, based on the provided binding object. </summary>
		/// <param name="dp">Identifies the property where the binding should be established.</param>
		/// <param name="binding">Represents the specifics of the data binding.</param>
		/// <returns>Records the conditions of the binding. This return value can be useful for error checking.</returns>
		// Token: 0x0600055E RID: 1374 RVA: 0x0000CBD5 File Offset: 0x0000ADD5
		public BindingExpressionBase SetBinding(DependencyProperty dp, BindingBase binding)
		{
			return BindingOperations.SetBinding(this, dp, binding);
		}

		/// <summary>Attaches a binding to this element, based on the provided source property name as a path qualification to the data source. </summary>
		/// <param name="dp">Identifies the destination property where the binding should be established.</param>
		/// <param name="path">The source property name or the path to the property used for the binding.</param>
		/// <returns>Records the conditions of the binding. This return value can be useful for error checking.</returns>
		// Token: 0x0600055F RID: 1375 RVA: 0x00010597 File Offset: 0x0000E797
		public BindingExpression SetBinding(DependencyProperty dp, string path)
		{
			return (BindingExpression)this.SetBinding(dp, new Binding(path));
		}

		/// <summary>Gets or sets the <see cref="T:System.Windows.Data.BindingGroup" /> that is used for the element.</summary>
		/// <returns>The <see cref="T:System.Windows.Data.BindingGroup" /> that is used for the element.</returns>
		// Token: 0x170000ED RID: 237
		// (get) Token: 0x06000560 RID: 1376 RVA: 0x000105AB File Offset: 0x0000E7AB
		// (set) Token: 0x06000561 RID: 1377 RVA: 0x000105BD File Offset: 0x0000E7BD
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Localizability(LocalizationCategory.NeverLocalize)]
		public BindingGroup BindingGroup
		{
			get
			{
				return (BindingGroup)base.GetValue(FrameworkElement.BindingGroupProperty);
			}
			set
			{
				base.SetValue(FrameworkElement.BindingGroupProperty, value);
			}
		}

		/// <summary>Returns an alternative logical parent for this element if there is no visual parent.</summary>
		/// <returns>Returns something other than <see langword="null" /> whenever a WPF framework-level implementation of this method has a non-visual parent connection.</returns>
		// Token: 0x06000562 RID: 1378 RVA: 0x000105CB File Offset: 0x0000E7CB
		protected internal override DependencyObject GetUIParentCore()
		{
			return this._parent;
		}

		// Token: 0x06000563 RID: 1379 RVA: 0x000105D4 File Offset: 0x0000E7D4
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

		// Token: 0x06000564 RID: 1380 RVA: 0x00002137 File Offset: 0x00000337
		internal virtual void AdjustBranchSource(RoutedEventArgs args)
		{
		}

		// Token: 0x06000565 RID: 1381 RVA: 0x00010615 File Offset: 0x0000E815
		internal override bool BuildRouteCore(EventRoute route, RoutedEventArgs args)
		{
			return this.BuildRouteCoreHelper(route, args, true);
		}

		// Token: 0x06000566 RID: 1382 RVA: 0x00010620 File Offset: 0x0000E820
		internal bool BuildRouteCoreHelper(EventRoute route, RoutedEventArgs args, bool shouldAddIntermediateElementsToRoute)
		{
			bool result = false;
			DependencyObject parent = VisualTreeHelper.GetParent(this);
			DependencyObject uiparentCore = this.GetUIParentCore();
			DependencyObject dependencyObject = route.PeekBranchNode() as DependencyObject;
			if (dependencyObject != null && this.IsLogicalDescendent(dependencyObject))
			{
				args.Source = route.PeekBranchSource();
				this.AdjustBranchSource(args);
				route.AddSource(args.Source);
				route.PopBranchNode();
				if (shouldAddIntermediateElementsToRoute)
				{
					FrameworkElement.AddIntermediateElementsToRoute(this, route, args, LogicalTreeHelper.GetParent(dependencyObject));
				}
			}
			if (!this.IgnoreModelParentBuildRoute(args))
			{
				if (parent == null)
				{
					result = (uiparentCore != null);
				}
				else if (uiparentCore != null)
				{
					Visual visual = parent as Visual;
					if (visual != null)
					{
						if (visual.CheckFlagsAnd(VisualFlags.IsLayoutIslandRoot))
						{
							result = true;
						}
					}
					else if (((Visual3D)parent).CheckFlagsAnd(VisualFlags.IsLayoutIslandRoot))
					{
						result = true;
					}
					route.PushBranchNode(this, args.Source);
					args.Source = parent;
				}
			}
			return result;
		}

		// Token: 0x06000567 RID: 1383 RVA: 0x000106E9 File Offset: 0x0000E8E9
		internal override void AddToEventRouteCore(EventRoute route, RoutedEventArgs args)
		{
			FrameworkElement.AddStyleHandlersToEventRoute(this, null, route, args);
		}

		// Token: 0x06000568 RID: 1384 RVA: 0x000106F4 File Offset: 0x0000E8F4
		internal static void AddStyleHandlersToEventRoute(FrameworkElement fe, FrameworkContentElement fce, EventRoute route, RoutedEventArgs args)
		{
			DependencyObject source = (fe != null) ? fe : fce;
			FrameworkTemplate frameworkTemplate = null;
			Style style;
			DependencyObject templatedParent;
			int templateChildIndex;
			if (fe != null)
			{
				style = fe.Style;
				frameworkTemplate = fe.TemplateInternal;
				templatedParent = fe.TemplatedParent;
				templateChildIndex = fe.TemplateChildIndex;
			}
			else
			{
				style = fce.Style;
				templatedParent = fce.TemplatedParent;
				templateChildIndex = fce.TemplateChildIndex;
			}
			if (style != null && style.EventHandlersStore != null)
			{
				RoutedEventHandlerInfo[] handlers = style.EventHandlersStore.GetRoutedEventHandlers(args.RoutedEvent);
				FrameworkElement.AddStyleHandlersToEventRoute(route, source, handlers);
			}
			if (frameworkTemplate != null && frameworkTemplate.EventHandlersStore != null)
			{
				RoutedEventHandlerInfo[] handlers = frameworkTemplate.EventHandlersStore.GetRoutedEventHandlers(args.RoutedEvent);
				FrameworkElement.AddStyleHandlersToEventRoute(route, source, handlers);
			}
			if (templatedParent != null)
			{
				FrameworkElement frameworkElement = templatedParent as FrameworkElement;
				FrameworkTemplate templateInternal = frameworkElement.TemplateInternal;
				RoutedEventHandlerInfo[] handlers = null;
				if (templateInternal != null && templateInternal.HasEventDependents)
				{
					handlers = StyleHelper.GetChildRoutedEventHandlers(templateChildIndex, args.RoutedEvent, ref templateInternal.EventDependents);
				}
				FrameworkElement.AddStyleHandlersToEventRoute(route, source, handlers);
			}
		}

		// Token: 0x06000569 RID: 1385 RVA: 0x000107E4 File Offset: 0x0000E9E4
		private static void AddStyleHandlersToEventRoute(EventRoute route, DependencyObject source, RoutedEventHandlerInfo[] handlers)
		{
			if (handlers != null)
			{
				for (int i = 0; i < handlers.Length; i++)
				{
					route.Add(source, handlers[i].Handler, handlers[i].InvokeHandledEventsToo);
				}
			}
		}

		// Token: 0x0600056A RID: 1386 RVA: 0x0000B02A File Offset: 0x0000922A
		internal virtual bool IgnoreModelParentBuildRoute(RoutedEventArgs args)
		{
			return false;
		}

		// Token: 0x0600056B RID: 1387 RVA: 0x00010824 File Offset: 0x0000EA24
		internal override bool InvalidateAutomationAncestorsCore(Stack<DependencyObject> branchNodeStack, out bool continuePastCoreTree)
		{
			bool shouldInvalidateIntermediateElements = true;
			return this.InvalidateAutomationAncestorsCoreHelper(branchNodeStack, out continuePastCoreTree, shouldInvalidateIntermediateElements);
		}

		// Token: 0x0600056C RID: 1388 RVA: 0x0001083C File Offset: 0x0000EA3C
		internal override void InvalidateForceInheritPropertyOnChildren(DependencyProperty property)
		{
			if (property == UIElement.IsEnabledProperty)
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
			base.InvalidateForceInheritPropertyOnChildren(property);
		}

		// Token: 0x0600056D RID: 1389 RVA: 0x00010884 File Offset: 0x0000EA84
		internal bool InvalidateAutomationAncestorsCoreHelper(Stack<DependencyObject> branchNodeStack, out bool continuePastCoreTree, bool shouldInvalidateIntermediateElements)
		{
			bool result = true;
			continuePastCoreTree = false;
			DependencyObject parent = VisualTreeHelper.GetParent(this);
			DependencyObject uiparentCore = this.GetUIParentCore();
			DependencyObject dependencyObject = (branchNodeStack.Count > 0) ? branchNodeStack.Peek() : null;
			if (dependencyObject != null && this.IsLogicalDescendent(dependencyObject))
			{
				branchNodeStack.Pop();
				if (shouldInvalidateIntermediateElements)
				{
					result = FrameworkElement.InvalidateAutomationIntermediateElements(this, LogicalTreeHelper.GetParent(dependencyObject));
				}
			}
			if (parent == null)
			{
				continuePastCoreTree = (uiparentCore != null);
			}
			else if (uiparentCore != null)
			{
				Visual visual = parent as Visual;
				if (visual != null)
				{
					if (visual.CheckFlagsAnd(VisualFlags.IsLayoutIslandRoot))
					{
						continuePastCoreTree = true;
					}
				}
				else if (((Visual3D)parent).CheckFlagsAnd(VisualFlags.IsLayoutIslandRoot))
				{
					continuePastCoreTree = true;
				}
				branchNodeStack.Push(this);
			}
			return result;
		}

		// Token: 0x0600056E RID: 1390 RVA: 0x00010924 File Offset: 0x0000EB24
		internal static bool InvalidateAutomationIntermediateElements(DependencyObject mergePoint, DependencyObject modelTreeNode)
		{
			UIElement uielement = null;
			ContentElement contentElement = null;
			UIElement3D uielement3D = null;
			while (modelTreeNode != null && modelTreeNode != mergePoint)
			{
				if (!UIElementHelper.InvalidateAutomationPeer(modelTreeNode, out uielement, out contentElement, out uielement3D))
				{
					return false;
				}
				modelTreeNode = LogicalTreeHelper.GetParent(modelTreeNode);
			}
			return true;
		}

		/// <summary>Gets or sets localization/globalization language information that applies to an element.</summary>
		/// <returns>The language information for this element. The default value is an <see cref="T:System.Windows.Markup.XmlLanguage" /> with its <see cref="P:System.Windows.Markup.XmlLanguage.IetfLanguageTag" /> value set to the string "en-US".</returns>
		// Token: 0x170000EE RID: 238
		// (get) Token: 0x0600056F RID: 1391 RVA: 0x00010959 File Offset: 0x0000EB59
		// (set) Token: 0x06000570 RID: 1392 RVA: 0x0001096B File Offset: 0x0000EB6B
		public XmlLanguage Language
		{
			get
			{
				return (XmlLanguage)base.GetValue(FrameworkElement.LanguageProperty);
			}
			set
			{
				base.SetValue(FrameworkElement.LanguageProperty, value);
			}
		}

		/// <summary>Gets or sets the identifying name of the element. The name provides a reference so that code-behind, such as event handler code, can refer to a markup element after it is constructed during processing by a XAML processor.</summary>
		/// <returns>The name of the element. The default is an empty string.</returns>
		// Token: 0x170000EF RID: 239
		// (get) Token: 0x06000571 RID: 1393 RVA: 0x00010979 File Offset: 0x0000EB79
		// (set) Token: 0x06000572 RID: 1394 RVA: 0x0001098B File Offset: 0x0000EB8B
		[Localizability(LocalizationCategory.NeverLocalize)]
		[MergableProperty(false)]
		[DesignerSerializationOptions(DesignerSerializationOptions.SerializeAsAttribute)]
		public string Name
		{
			get
			{
				return (string)base.GetValue(FrameworkElement.NameProperty);
			}
			set
			{
				base.SetValue(FrameworkElement.NameProperty, value);
			}
		}

		/// <summary>Gets or sets an arbitrary object value that can be used to store custom information about this element.</summary>
		/// <returns>The intended value. This property has no default value.</returns>
		// Token: 0x170000F0 RID: 240
		// (get) Token: 0x06000573 RID: 1395 RVA: 0x00010999 File Offset: 0x0000EB99
		// (set) Token: 0x06000574 RID: 1396 RVA: 0x000109A6 File Offset: 0x0000EBA6
		[Localizability(LocalizationCategory.NeverLocalize)]
		public object Tag
		{
			get
			{
				return base.GetValue(FrameworkElement.TagProperty);
			}
			set
			{
				base.SetValue(FrameworkElement.TagProperty, value);
			}
		}

		/// <summary>Gets or sets the context for input used by this <see cref="T:System.Windows.FrameworkElement" />. </summary>
		/// <returns>The input scope, which modifies how input from alternative input methods is interpreted. The default value is <see langword="null" /> (which results in a default handling of commands).</returns>
		// Token: 0x170000F1 RID: 241
		// (get) Token: 0x06000575 RID: 1397 RVA: 0x000109B4 File Offset: 0x0000EBB4
		// (set) Token: 0x06000576 RID: 1398 RVA: 0x000109C6 File Offset: 0x0000EBC6
		public InputScope InputScope
		{
			get
			{
				return (InputScope)base.GetValue(FrameworkElement.InputScopeProperty);
			}
			set
			{
				base.SetValue(FrameworkElement.InputScopeProperty, value);
			}
		}

		/// <summary>Occurs when <see cref="M:System.Windows.FrameworkElement.BringIntoView(System.Windows.Rect)" /> is called on this element. </summary>
		// Token: 0x1400001E RID: 30
		// (add) Token: 0x06000577 RID: 1399 RVA: 0x000109D4 File Offset: 0x0000EBD4
		// (remove) Token: 0x06000578 RID: 1400 RVA: 0x000109E3 File Offset: 0x0000EBE3
		public event RequestBringIntoViewEventHandler RequestBringIntoView
		{
			add
			{
				base.AddHandler(FrameworkElement.RequestBringIntoViewEvent, value, false);
			}
			remove
			{
				base.RemoveHandler(FrameworkElement.RequestBringIntoViewEvent, value);
			}
		}

		/// <summary>Attempts to bring this element into view, within any scrollable regions it is contained within. </summary>
		// Token: 0x06000579 RID: 1401 RVA: 0x000109F1 File Offset: 0x0000EBF1
		public void BringIntoView()
		{
			this.BringIntoView(Rect.Empty);
		}

		/// <summary>Attempts to bring the provided region size of this element into view, within any scrollable regions it is contained within. </summary>
		/// <param name="targetRectangle">Specified size of the element that should also be brought into view. </param>
		// Token: 0x0600057A RID: 1402 RVA: 0x00010A00 File Offset: 0x0000EC00
		public void BringIntoView(Rect targetRectangle)
		{
			base.RaiseEvent(new RequestBringIntoViewEventArgs(this, targetRectangle)
			{
				RoutedEvent = FrameworkElement.RequestBringIntoViewEvent
			});
		}

		/// <summary>Occurs when either the <see cref="P:System.Windows.FrameworkElement.ActualHeight" /> or the <see cref="P:System.Windows.FrameworkElement.ActualWidth" /> properties change value on this element. </summary>
		// Token: 0x1400001F RID: 31
		// (add) Token: 0x0600057B RID: 1403 RVA: 0x00010A27 File Offset: 0x0000EC27
		// (remove) Token: 0x0600057C RID: 1404 RVA: 0x00010A36 File Offset: 0x0000EC36
		public event SizeChangedEventHandler SizeChanged
		{
			add
			{
				base.AddHandler(FrameworkElement.SizeChangedEvent, value, false);
			}
			remove
			{
				base.RemoveHandler(FrameworkElement.SizeChangedEvent, value);
			}
		}

		// Token: 0x0600057D RID: 1405 RVA: 0x00010A44 File Offset: 0x0000EC44
		private static object GetActualWidth(DependencyObject d, out BaseValueSourceInternal source)
		{
			FrameworkElement frameworkElement = (FrameworkElement)d;
			if (frameworkElement.HasWidthEverChanged)
			{
				source = BaseValueSourceInternal.Local;
				return frameworkElement.RenderSize.Width;
			}
			source = BaseValueSourceInternal.Default;
			return 0.0;
		}

		/// <summary>Gets the rendered width of this element.</summary>
		/// <returns>The element's width, as a value in device-independent units (1/96th inch per unit). The default value is 0 (zero).</returns>
		// Token: 0x170000F2 RID: 242
		// (get) Token: 0x0600057E RID: 1406 RVA: 0x00010A8C File Offset: 0x0000EC8C
		public double ActualWidth
		{
			get
			{
				return base.RenderSize.Width;
			}
		}

		// Token: 0x0600057F RID: 1407 RVA: 0x00010AA8 File Offset: 0x0000ECA8
		private static object GetActualHeight(DependencyObject d, out BaseValueSourceInternal source)
		{
			FrameworkElement frameworkElement = (FrameworkElement)d;
			if (frameworkElement.HasHeightEverChanged)
			{
				source = BaseValueSourceInternal.Local;
				return frameworkElement.RenderSize.Height;
			}
			source = BaseValueSourceInternal.Default;
			return 0.0;
		}

		/// <summary>Gets the rendered height of this element.</summary>
		/// <returns>The element's height, as a value in device-independent units (1/96th inch per unit). The default value is 0 (zero).</returns>
		// Token: 0x170000F3 RID: 243
		// (get) Token: 0x06000580 RID: 1408 RVA: 0x00010AF0 File Offset: 0x0000ECF0
		public double ActualHeight
		{
			get
			{
				return base.RenderSize.Height;
			}
		}

		/// <summary> Gets or sets a graphics transformation that should apply to this element when  layout is performed.</summary>
		/// <returns>The transform this element should use. The default is <see cref="P:System.Windows.Media.Transform.Identity" />.</returns>
		// Token: 0x170000F4 RID: 244
		// (get) Token: 0x06000581 RID: 1409 RVA: 0x00010B0B File Offset: 0x0000ED0B
		// (set) Token: 0x06000582 RID: 1410 RVA: 0x00010B1D File Offset: 0x0000ED1D
		public Transform LayoutTransform
		{
			get
			{
				return (Transform)base.GetValue(FrameworkElement.LayoutTransformProperty);
			}
			set
			{
				base.SetValue(FrameworkElement.LayoutTransformProperty, value);
			}
		}

		// Token: 0x06000583 RID: 1411 RVA: 0x00010B2C File Offset: 0x0000ED2C
		private static void OnLayoutTransformChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			FrameworkElement frameworkElement = (FrameworkElement)d;
			frameworkElement.AreTransformsClean = false;
		}

		// Token: 0x06000584 RID: 1412 RVA: 0x00010B48 File Offset: 0x0000ED48
		private static bool IsWidthHeightValid(object value)
		{
			double num = (double)value;
			return DoubleUtil.IsNaN(num) || (num >= 0.0 && !double.IsPositiveInfinity(num));
		}

		// Token: 0x06000585 RID: 1413 RVA: 0x00010B80 File Offset: 0x0000ED80
		private static bool IsMinWidthHeightValid(object value)
		{
			double num = (double)value;
			return !DoubleUtil.IsNaN(num) && num >= 0.0 && !double.IsPositiveInfinity(num);
		}

		// Token: 0x06000586 RID: 1414 RVA: 0x00010BB4 File Offset: 0x0000EDB4
		private static bool IsMaxWidthHeightValid(object value)
		{
			double num = (double)value;
			return !DoubleUtil.IsNaN(num) && num >= 0.0;
		}

		// Token: 0x06000587 RID: 1415 RVA: 0x00010BE4 File Offset: 0x0000EDE4
		private static void OnTransformDirty(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			FrameworkElement frameworkElement = (FrameworkElement)d;
			frameworkElement.AreTransformsClean = false;
		}

		/// <summary> Gets or sets the width of the element.</summary>
		/// <returns>The width of the element, in device-independent units (1/96th inch per unit). The default value is <see cref="F:System.Double.NaN" />. This value must be equal to or greater than 0.0. See Remarks for upper bound information.</returns>
		// Token: 0x170000F5 RID: 245
		// (get) Token: 0x06000588 RID: 1416 RVA: 0x00010BFF File Offset: 0x0000EDFF
		// (set) Token: 0x06000589 RID: 1417 RVA: 0x00010C11 File Offset: 0x0000EE11
		[TypeConverter(typeof(LengthConverter))]
		[Localizability(LocalizationCategory.None, Readability = Readability.Unreadable)]
		public double Width
		{
			get
			{
				return (double)base.GetValue(FrameworkElement.WidthProperty);
			}
			set
			{
				base.SetValue(FrameworkElement.WidthProperty, value);
			}
		}

		/// <summary> Gets or sets the minimum width constraint of the element.</summary>
		/// <returns>The minimum width of the element, in device-independent units (1/96th inch per unit). The default value is 0.0. This value can be any value equal to or greater than 0.0. However, <see cref="F:System.Double.PositiveInfinity" /> is not valid, nor is <see cref="F:System.Double.NaN" />.</returns>
		// Token: 0x170000F6 RID: 246
		// (get) Token: 0x0600058A RID: 1418 RVA: 0x00010C24 File Offset: 0x0000EE24
		// (set) Token: 0x0600058B RID: 1419 RVA: 0x00010C36 File Offset: 0x0000EE36
		[TypeConverter(typeof(LengthConverter))]
		[Localizability(LocalizationCategory.None, Readability = Readability.Unreadable)]
		public double MinWidth
		{
			get
			{
				return (double)base.GetValue(FrameworkElement.MinWidthProperty);
			}
			set
			{
				base.SetValue(FrameworkElement.MinWidthProperty, value);
			}
		}

		/// <summary>Gets or sets the maximum width constraint of the element.</summary>
		/// <returns>The maximum width of the element, in device-independent units (1/96th inch per unit). The default value is <see cref="F:System.Double.PositiveInfinity" />. This value can be any value equal to or greater than 0.0. <see cref="F:System.Double.PositiveInfinity" /> is also valid.</returns>
		// Token: 0x170000F7 RID: 247
		// (get) Token: 0x0600058C RID: 1420 RVA: 0x00010C49 File Offset: 0x0000EE49
		// (set) Token: 0x0600058D RID: 1421 RVA: 0x00010C5B File Offset: 0x0000EE5B
		[TypeConverter(typeof(LengthConverter))]
		[Localizability(LocalizationCategory.None, Readability = Readability.Unreadable)]
		public double MaxWidth
		{
			get
			{
				return (double)base.GetValue(FrameworkElement.MaxWidthProperty);
			}
			set
			{
				base.SetValue(FrameworkElement.MaxWidthProperty, value);
			}
		}

		/// <summary> Gets or sets the suggested height of the element.</summary>
		/// <returns>The height of the element, in device-independent units (1/96th inch per unit). The default value is <see cref="F:System.Double.NaN" />. This value must be equal to or greater than 0.0. See Remarks for upper bound information.</returns>
		// Token: 0x170000F8 RID: 248
		// (get) Token: 0x0600058E RID: 1422 RVA: 0x00010C6E File Offset: 0x0000EE6E
		// (set) Token: 0x0600058F RID: 1423 RVA: 0x00010C80 File Offset: 0x0000EE80
		[TypeConverter(typeof(LengthConverter))]
		[Localizability(LocalizationCategory.None, Readability = Readability.Unreadable)]
		public double Height
		{
			get
			{
				return (double)base.GetValue(FrameworkElement.HeightProperty);
			}
			set
			{
				base.SetValue(FrameworkElement.HeightProperty, value);
			}
		}

		/// <summary>Gets or sets the minimum height constraint of the element.</summary>
		/// <returns>The minimum height of the element, in device-independent units (1/96th inch per unit). The default value is 0.0. This value can be any value equal to or greater than 0.0. However, <see cref="F:System.Double.PositiveInfinity" /> is NOT valid, nor is <see cref="F:System.Double.NaN" />.</returns>
		// Token: 0x170000F9 RID: 249
		// (get) Token: 0x06000590 RID: 1424 RVA: 0x00010C93 File Offset: 0x0000EE93
		// (set) Token: 0x06000591 RID: 1425 RVA: 0x00010CA5 File Offset: 0x0000EEA5
		[TypeConverter(typeof(LengthConverter))]
		[Localizability(LocalizationCategory.None, Readability = Readability.Unreadable)]
		public double MinHeight
		{
			get
			{
				return (double)base.GetValue(FrameworkElement.MinHeightProperty);
			}
			set
			{
				base.SetValue(FrameworkElement.MinHeightProperty, value);
			}
		}

		/// <summary>Gets or sets the maximum height constraint of the element.</summary>
		/// <returns>The maximum height of the element, in device-independent units (1/96th inch per unit). The default value is <see cref="F:System.Double.PositiveInfinity" />. This value can be any value equal to or greater than 0.0. <see cref="F:System.Double.PositiveInfinity" /> is also valid.</returns>
		// Token: 0x170000FA RID: 250
		// (get) Token: 0x06000592 RID: 1426 RVA: 0x00010CB8 File Offset: 0x0000EEB8
		// (set) Token: 0x06000593 RID: 1427 RVA: 0x00010CCA File Offset: 0x0000EECA
		[TypeConverter(typeof(LengthConverter))]
		[Localizability(LocalizationCategory.None, Readability = Readability.Unreadable)]
		public double MaxHeight
		{
			get
			{
				return (double)base.GetValue(FrameworkElement.MaxHeightProperty);
			}
			set
			{
				base.SetValue(FrameworkElement.MaxHeightProperty, value);
			}
		}

		// Token: 0x06000594 RID: 1428 RVA: 0x00010CE0 File Offset: 0x0000EEE0
		private static object CoerceFlowDirectionProperty(DependencyObject d, object value)
		{
			FrameworkElement frameworkElement = d as FrameworkElement;
			if (frameworkElement != null)
			{
				frameworkElement.InvalidateArrange();
				frameworkElement.InvalidateVisual();
				frameworkElement.AreTransformsClean = false;
			}
			return value;
		}

		// Token: 0x06000595 RID: 1429 RVA: 0x00010D0C File Offset: 0x0000EF0C
		private static void OnFlowDirectionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			FrameworkElement frameworkElement = d as FrameworkElement;
			if (frameworkElement != null)
			{
				frameworkElement.IsRightToLeft = ((FlowDirection)e.NewValue == FlowDirection.RightToLeft);
				frameworkElement.AreTransformsClean = false;
			}
		}

		/// <summary>Gets or sets the direction that text and other user interface (UI) elements flow within any parent element that controls their layout.</summary>
		/// <returns>The direction that text and other UI elements flow within their parent element, as a value of the enumeration. The default value is <see cref="F:System.Windows.FlowDirection.LeftToRight" />.</returns>
		// Token: 0x170000FB RID: 251
		// (get) Token: 0x06000596 RID: 1430 RVA: 0x00010D3F File Offset: 0x0000EF3F
		// (set) Token: 0x06000597 RID: 1431 RVA: 0x00010D4C File Offset: 0x0000EF4C
		[Localizability(LocalizationCategory.None)]
		public FlowDirection FlowDirection
		{
			get
			{
				if (!this.IsRightToLeft)
				{
					return FlowDirection.LeftToRight;
				}
				return FlowDirection.RightToLeft;
			}
			set
			{
				base.SetValue(FrameworkElement.FlowDirectionProperty, value);
			}
		}

		/// <summary>Gets the value of the <see cref="P:System.Windows.FrameworkElement.FlowDirection" /> attached property for the specified <see cref="T:System.Windows.DependencyObject" />. </summary>
		/// <param name="element">The element to return a <see cref="P:System.Windows.FrameworkElement.FlowDirection" /> for.</param>
		/// <returns>The requested flow direction, as a value of the enumeration.</returns>
		// Token: 0x06000598 RID: 1432 RVA: 0x00010D5F File Offset: 0x0000EF5F
		public static FlowDirection GetFlowDirection(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (FlowDirection)element.GetValue(FrameworkElement.FlowDirectionProperty);
		}

		/// <summary>Sets the value of the <see cref="P:System.Windows.FrameworkElement.FlowDirection" /> attached property for the provided element. </summary>
		/// <param name="element">The element that specifies a flow direction.</param>
		/// <param name="value">A value of the enumeration, specifying the direction.</param>
		// Token: 0x06000599 RID: 1433 RVA: 0x00010D7F File Offset: 0x0000EF7F
		public static void SetFlowDirection(DependencyObject element, FlowDirection value)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(FrameworkElement.FlowDirectionProperty, value);
		}

		// Token: 0x0600059A RID: 1434 RVA: 0x00010DA0 File Offset: 0x0000EFA0
		private static bool IsValidFlowDirection(object o)
		{
			FlowDirection flowDirection = (FlowDirection)o;
			return flowDirection == FlowDirection.LeftToRight || flowDirection == FlowDirection.RightToLeft;
		}

		// Token: 0x0600059B RID: 1435 RVA: 0x00010DC0 File Offset: 0x0000EFC0
		private static bool IsMarginValid(object value)
		{
			return ((Thickness)value).IsValid(true, false, true, false);
		}

		/// <summary>Gets or sets the outer margin of an element.</summary>
		/// <returns>Provides margin values for the element. The default value is a <see cref="T:System.Windows.Thickness" /> with all properties equal to 0 (zero).</returns>
		// Token: 0x170000FC RID: 252
		// (get) Token: 0x0600059C RID: 1436 RVA: 0x00010DDF File Offset: 0x0000EFDF
		// (set) Token: 0x0600059D RID: 1437 RVA: 0x00010DF1 File Offset: 0x0000EFF1
		public Thickness Margin
		{
			get
			{
				return (Thickness)base.GetValue(FrameworkElement.MarginProperty);
			}
			set
			{
				base.SetValue(FrameworkElement.MarginProperty, value);
			}
		}

		// Token: 0x0600059E RID: 1438 RVA: 0x00010E04 File Offset: 0x0000F004
		internal static bool ValidateHorizontalAlignmentValue(object value)
		{
			HorizontalAlignment horizontalAlignment = (HorizontalAlignment)value;
			return horizontalAlignment == HorizontalAlignment.Left || horizontalAlignment == HorizontalAlignment.Center || horizontalAlignment == HorizontalAlignment.Right || horizontalAlignment == HorizontalAlignment.Stretch;
		}

		/// <summary>Gets or sets the horizontal alignment characteristics applied to this element when it is composed within a parent element, such as a panel or items control.</summary>
		/// <returns>A horizontal alignment setting, as a value of the enumeration. The default is <see cref="F:System.Windows.HorizontalAlignment.Stretch" />.</returns>
		// Token: 0x170000FD RID: 253
		// (get) Token: 0x0600059F RID: 1439 RVA: 0x00010E29 File Offset: 0x0000F029
		// (set) Token: 0x060005A0 RID: 1440 RVA: 0x00010E3B File Offset: 0x0000F03B
		public HorizontalAlignment HorizontalAlignment
		{
			get
			{
				return (HorizontalAlignment)base.GetValue(FrameworkElement.HorizontalAlignmentProperty);
			}
			set
			{
				base.SetValue(FrameworkElement.HorizontalAlignmentProperty, value);
			}
		}

		// Token: 0x060005A1 RID: 1441 RVA: 0x00010E50 File Offset: 0x0000F050
		internal static bool ValidateVerticalAlignmentValue(object value)
		{
			VerticalAlignment verticalAlignment = (VerticalAlignment)value;
			return verticalAlignment == VerticalAlignment.Top || verticalAlignment == VerticalAlignment.Center || verticalAlignment == VerticalAlignment.Bottom || verticalAlignment == VerticalAlignment.Stretch;
		}

		/// <summary>Gets or sets the vertical alignment characteristics applied to this element when it is composed within a parent element such as a panel or items control.</summary>
		/// <returns>A vertical alignment setting. The default is <see cref="F:System.Windows.VerticalAlignment.Stretch" />.</returns>
		// Token: 0x170000FE RID: 254
		// (get) Token: 0x060005A2 RID: 1442 RVA: 0x00010E75 File Offset: 0x0000F075
		// (set) Token: 0x060005A3 RID: 1443 RVA: 0x00010E87 File Offset: 0x0000F087
		public VerticalAlignment VerticalAlignment
		{
			get
			{
				return (VerticalAlignment)base.GetValue(FrameworkElement.VerticalAlignmentProperty);
			}
			set
			{
				base.SetValue(FrameworkElement.VerticalAlignmentProperty, value);
			}
		}

		// Token: 0x170000FF RID: 255
		// (get) Token: 0x060005A4 RID: 1444 RVA: 0x00010E9C File Offset: 0x0000F09C
		internal static Style DefaultFocusVisualStyle
		{
			get
			{
				if (FrameworkElement._defaultFocusVisualStyle == null)
				{
					Style style = new Style();
					style.Seal();
					FrameworkElement._defaultFocusVisualStyle = style;
				}
				return FrameworkElement._defaultFocusVisualStyle;
			}
		}

		/// <summary>Gets or sets a property that enables customization of appearance, effects, or other style characteristics that will apply to this element when it captures keyboard focus.</summary>
		/// <returns>The desired style to apply on focus. The default value as declared in the dependency property is an empty static <see cref="T:System.Windows.Style" />. However, the effective value at run time is often (but not always) a style as supplied by theme support for controls. </returns>
		// Token: 0x17000100 RID: 256
		// (get) Token: 0x060005A5 RID: 1445 RVA: 0x00010EC7 File Offset: 0x0000F0C7
		// (set) Token: 0x060005A6 RID: 1446 RVA: 0x00010ED9 File Offset: 0x0000F0D9
		public Style FocusVisualStyle
		{
			get
			{
				return (Style)base.GetValue(FrameworkElement.FocusVisualStyleProperty);
			}
			set
			{
				base.SetValue(FrameworkElement.FocusVisualStyleProperty, value);
			}
		}

		/// <summary>Gets or sets the cursor that displays when the mouse pointer is over this element.</summary>
		/// <returns>The cursor to display. The default value is defined as <see langword="null" /> per this dependency property. However, the practical default at run time will come from a variety of factors.</returns>
		// Token: 0x17000101 RID: 257
		// (get) Token: 0x060005A7 RID: 1447 RVA: 0x00010EE7 File Offset: 0x0000F0E7
		// (set) Token: 0x060005A8 RID: 1448 RVA: 0x00010EF9 File Offset: 0x0000F0F9
		public Cursor Cursor
		{
			get
			{
				return (Cursor)base.GetValue(FrameworkElement.CursorProperty);
			}
			set
			{
				base.SetValue(FrameworkElement.CursorProperty, value);
			}
		}

		// Token: 0x060005A9 RID: 1449 RVA: 0x00010F08 File Offset: 0x0000F108
		private static void OnCursorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			FrameworkElement frameworkElement = (FrameworkElement)d;
			if (frameworkElement.IsMouseOver)
			{
				Mouse.UpdateCursor();
			}
		}

		/// <summary>Gets or sets a value that indicates whether this <see cref="T:System.Windows.FrameworkElement" /> should force the user interface (UI) to render the cursor as declared by the <see cref="P:System.Windows.FrameworkElement.Cursor" /> property.</summary>
		/// <returns>
		///     <see langword="true" /> if cursor presentation while over this element is forced to use current <see cref="P:System.Windows.FrameworkElement.Cursor" /> settings for the cursor (including on all child elements); otherwise <see langword="false" />. The default value is <see langword="false" />.</returns>
		// Token: 0x17000102 RID: 258
		// (get) Token: 0x060005AA RID: 1450 RVA: 0x00010F29 File Offset: 0x0000F129
		// (set) Token: 0x060005AB RID: 1451 RVA: 0x00010F3B File Offset: 0x0000F13B
		public bool ForceCursor
		{
			get
			{
				return (bool)base.GetValue(FrameworkElement.ForceCursorProperty);
			}
			set
			{
				base.SetValue(FrameworkElement.ForceCursorProperty, BooleanBoxes.Box(value));
			}
		}

		// Token: 0x060005AC RID: 1452 RVA: 0x00010F50 File Offset: 0x0000F150
		private static void OnForceCursorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			FrameworkElement frameworkElement = (FrameworkElement)d;
			if (frameworkElement.IsMouseOver)
			{
				Mouse.UpdateCursor();
			}
		}

		// Token: 0x060005AD RID: 1453 RVA: 0x00010F74 File Offset: 0x0000F174
		private static void OnQueryCursorOverride(object sender, QueryCursorEventArgs e)
		{
			FrameworkElement frameworkElement = (FrameworkElement)sender;
			Cursor cursor = frameworkElement.Cursor;
			if (cursor != null && (!e.Handled || frameworkElement.ForceCursor))
			{
				e.Cursor = cursor;
				e.Handled = true;
			}
		}

		// Token: 0x060005AE RID: 1454 RVA: 0x00010FB0 File Offset: 0x0000F1B0
		private Transform GetFlowDirectionTransform()
		{
			if (!this.BypassLayoutPolicies && FrameworkElement.ShouldApplyMirrorTransform(this))
			{
				return new MatrixTransform(-1.0, 0.0, 0.0, 1.0, base.RenderSize.Width, 0.0);
			}
			return null;
		}

		// Token: 0x060005AF RID: 1455 RVA: 0x00011010 File Offset: 0x0000F210
		internal static bool ShouldApplyMirrorTransform(FrameworkElement fe)
		{
			FlowDirection flowDirection = fe.FlowDirection;
			FlowDirection parentFD = FlowDirection.LeftToRight;
			DependencyObject parent = VisualTreeHelper.GetParent(fe);
			if (parent != null)
			{
				parentFD = FrameworkElement.GetFlowDirectionFromVisual(parent);
			}
			else
			{
				FrameworkElement frameworkElement;
				FrameworkContentElement frameworkContentElement;
				bool frameworkParent = FrameworkElement.GetFrameworkParent(fe, out frameworkElement, out frameworkContentElement);
				if (frameworkParent)
				{
					if (frameworkElement != null && frameworkElement is IContentHost)
					{
						parentFD = frameworkElement.FlowDirection;
					}
					else if (frameworkContentElement != null)
					{
						parentFD = (FlowDirection)frameworkContentElement.GetValue(FrameworkElement.FlowDirectionProperty);
					}
				}
			}
			return FrameworkElement.ApplyMirrorTransform(parentFD, flowDirection);
		}

		// Token: 0x060005B0 RID: 1456 RVA: 0x0001107C File Offset: 0x0000F27C
		private static FlowDirection GetFlowDirectionFromVisual(DependencyObject visual)
		{
			FlowDirection result = FlowDirection.LeftToRight;
			for (DependencyObject dependencyObject = visual; dependencyObject != null; dependencyObject = VisualTreeHelper.GetParent(dependencyObject))
			{
				FrameworkElement frameworkElement = dependencyObject as FrameworkElement;
				if (frameworkElement != null)
				{
					result = frameworkElement.FlowDirection;
					break;
				}
				object obj = dependencyObject.ReadLocalValue(FrameworkElement.FlowDirectionProperty);
				if (obj != DependencyProperty.UnsetValue)
				{
					result = (FlowDirection)obj;
					break;
				}
			}
			return result;
		}

		// Token: 0x060005B1 RID: 1457 RVA: 0x000110CA File Offset: 0x0000F2CA
		internal static bool ApplyMirrorTransform(FlowDirection parentFD, FlowDirection thisFD)
		{
			return (parentFD == FlowDirection.LeftToRight && thisFD == FlowDirection.RightToLeft) || (parentFD == FlowDirection.RightToLeft && thisFD == FlowDirection.LeftToRight);
		}

		// Token: 0x060005B2 RID: 1458 RVA: 0x000110E0 File Offset: 0x0000F2E0
		private Size FindMaximalAreaLocalSpaceRect(Transform layoutTransform, Size transformSpaceBounds)
		{
			double num = transformSpaceBounds.Width;
			double num2 = transformSpaceBounds.Height;
			if (DoubleUtil.IsZero(num) || DoubleUtil.IsZero(num2))
			{
				return new Size(0.0, 0.0);
			}
			bool flag = double.IsInfinity(num);
			bool flag2 = double.IsInfinity(num2);
			if (flag && flag2)
			{
				return new Size(double.PositiveInfinity, double.PositiveInfinity);
			}
			if (flag)
			{
				num = num2;
			}
			else if (flag2)
			{
				num2 = num;
			}
			Matrix value = layoutTransform.Value;
			if (!value.HasInverse)
			{
				return new Size(0.0, 0.0);
			}
			double m = value.M11;
			double m2 = value.M12;
			double m3 = value.M21;
			double m4 = value.M22;
			double num5;
			double num6;
			if (DoubleUtil.IsZero(m2) || DoubleUtil.IsZero(m3))
			{
				double num3 = flag2 ? double.PositiveInfinity : Math.Abs(num2 / m4);
				double num4 = flag ? double.PositiveInfinity : Math.Abs(num / m);
				if (DoubleUtil.IsZero(m2))
				{
					if (DoubleUtil.IsZero(m3))
					{
						num5 = num3;
						num6 = num4;
					}
					else
					{
						num5 = Math.Min(0.5 * Math.Abs(num / m3), num3);
						num6 = num4 - m3 * num5 / m;
					}
				}
				else
				{
					num6 = Math.Min(0.5 * Math.Abs(num2 / m2), num4);
					num5 = num3 - m2 * num6 / m4;
				}
			}
			else if (DoubleUtil.IsZero(m) || DoubleUtil.IsZero(m4))
			{
				double num7 = Math.Abs(num2 / m2);
				double num8 = Math.Abs(num / m3);
				if (DoubleUtil.IsZero(m))
				{
					if (DoubleUtil.IsZero(m4))
					{
						num5 = num8;
						num6 = num7;
					}
					else
					{
						num5 = Math.Min(0.5 * Math.Abs(num2 / m4), num8);
						num6 = num7 - m4 * num5 / m2;
					}
				}
				else
				{
					num6 = Math.Min(0.5 * Math.Abs(num / m), num7);
					num5 = num8 - m * num6 / m3;
				}
			}
			else
			{
				double num9 = Math.Abs(num / m);
				double num10 = Math.Abs(num / m3);
				double num11 = Math.Abs(num2 / m2);
				double num12 = Math.Abs(num2 / m4);
				num6 = Math.Min(num11, num9) * 0.5;
				num5 = Math.Min(num10, num12) * 0.5;
				if ((DoubleUtil.GreaterThanOrClose(num9, num11) && DoubleUtil.LessThanOrClose(num10, num12)) || (DoubleUtil.LessThanOrClose(num9, num11) && DoubleUtil.GreaterThanOrClose(num10, num12)))
				{
					Rect rect = Rect.Transform(new Rect(0.0, 0.0, num6, num5), layoutTransform.Value);
					double num13 = Math.Min(num / rect.Width, num2 / rect.Height);
					if (!double.IsNaN(num13) && !double.IsInfinity(num13))
					{
						num6 *= num13;
						num5 *= num13;
					}
				}
			}
			return new Size(num6, num5);
		}

		/// <summary>Implements basic measure-pass layout system behavior for <see cref="T:System.Windows.FrameworkElement" />. </summary>
		/// <param name="availableSize">The available size that the parent element can give to the child elements.</param>
		/// <returns>The desired size of this element in layout.</returns>
		// Token: 0x060005B3 RID: 1459 RVA: 0x00011414 File Offset: 0x0000F614
		protected sealed override Size MeasureCore(Size availableSize)
		{
			bool useLayoutRounding = this.UseLayoutRounding;
			DpiScale dpi = base.GetDpi();
			if (useLayoutRounding && !base.CheckFlagsAnd(VisualFlags.UseLayoutRounding))
			{
				base.SetFlags(true, VisualFlags.UseLayoutRounding);
			}
			this.ApplyTemplate();
			if (this.BypassLayoutPolicies)
			{
				return this.MeasureOverride(availableSize);
			}
			Thickness margin = this.Margin;
			double num = margin.Left + margin.Right;
			double num2 = margin.Top + margin.Bottom;
			if (useLayoutRounding && (this is ScrollContentPresenter || !FrameworkAppContextSwitches.DoNotApplyLayoutRoundingToMarginsAndBorderThickness))
			{
				num = UIElement.RoundLayoutValue(num, dpi.DpiScaleX);
				num2 = UIElement.RoundLayoutValue(num2, dpi.DpiScaleY);
			}
			Size size = new Size(Math.Max(availableSize.Width - num, 0.0), Math.Max(availableSize.Height - num2, 0.0));
			FrameworkElement.MinMax minMax = new FrameworkElement.MinMax(this);
			if (useLayoutRounding && !FrameworkAppContextSwitches.DoNotApplyLayoutRoundingToMarginsAndBorderThickness)
			{
				minMax.maxHeight = UIElement.RoundLayoutValue(minMax.maxHeight, dpi.DpiScaleY);
				minMax.maxWidth = UIElement.RoundLayoutValue(minMax.maxWidth, dpi.DpiScaleX);
				minMax.minHeight = UIElement.RoundLayoutValue(minMax.minHeight, dpi.DpiScaleY);
				minMax.minWidth = UIElement.RoundLayoutValue(minMax.minWidth, dpi.DpiScaleX);
			}
			FrameworkElement.LayoutTransformData layoutTransformData = FrameworkElement.LayoutTransformDataField.GetValue(this);
			Transform layoutTransform = this.LayoutTransform;
			if (layoutTransform != null && !layoutTransform.IsIdentity)
			{
				if (layoutTransformData == null)
				{
					layoutTransformData = new FrameworkElement.LayoutTransformData();
					FrameworkElement.LayoutTransformDataField.SetValue(this, layoutTransformData);
				}
				layoutTransformData.CreateTransformSnapshot(layoutTransform);
				layoutTransformData.UntransformedDS = default(Size);
				if (useLayoutRounding)
				{
					layoutTransformData.TransformedUnroundedDS = default(Size);
				}
			}
			else if (layoutTransformData != null)
			{
				layoutTransformData = null;
				FrameworkElement.LayoutTransformDataField.ClearValue(this);
			}
			if (layoutTransformData != null)
			{
				size = this.FindMaximalAreaLocalSpaceRect(layoutTransformData.Transform, size);
			}
			size.Width = Math.Max(minMax.minWidth, Math.Min(size.Width, minMax.maxWidth));
			size.Height = Math.Max(minMax.minHeight, Math.Min(size.Height, minMax.maxHeight));
			if (useLayoutRounding)
			{
				size = UIElement.RoundLayoutSize(size, dpi.DpiScaleX, dpi.DpiScaleY);
			}
			Size size2 = this.MeasureOverride(size);
			size2 = new Size(Math.Max(size2.Width, minMax.minWidth), Math.Max(size2.Height, minMax.minHeight));
			Size size3 = size2;
			if (layoutTransformData != null)
			{
				layoutTransformData.UntransformedDS = size3;
				Rect rect = Rect.Transform(new Rect(0.0, 0.0, size3.Width, size3.Height), layoutTransformData.Transform.Value);
				size3.Width = rect.Width;
				size3.Height = rect.Height;
			}
			bool flag = false;
			if (size2.Width > minMax.maxWidth)
			{
				size2.Width = minMax.maxWidth;
				flag = true;
			}
			if (size2.Height > minMax.maxHeight)
			{
				size2.Height = minMax.maxHeight;
				flag = true;
			}
			if (layoutTransformData != null)
			{
				Rect rect2 = Rect.Transform(new Rect(0.0, 0.0, size2.Width, size2.Height), layoutTransformData.Transform.Value);
				size2.Width = rect2.Width;
				size2.Height = rect2.Height;
			}
			double num3 = size2.Width + num;
			double num4 = size2.Height + num2;
			if (num3 > availableSize.Width)
			{
				num3 = availableSize.Width;
				flag = true;
			}
			if (num4 > availableSize.Height)
			{
				num4 = availableSize.Height;
				flag = true;
			}
			if (layoutTransformData != null)
			{
				layoutTransformData.TransformedUnroundedDS = new Size(Math.Max(0.0, num3), Math.Max(0.0, num4));
			}
			if (useLayoutRounding)
			{
				num3 = UIElement.RoundLayoutValue(num3, dpi.DpiScaleX);
				num4 = UIElement.RoundLayoutValue(num4, dpi.DpiScaleY);
			}
			SizeBox sizeBox = FrameworkElement.UnclippedDesiredSizeField.GetValue(this);
			if (flag || num3 < 0.0 || num4 < 0.0)
			{
				if (sizeBox == null)
				{
					sizeBox = new SizeBox(size3);
					FrameworkElement.UnclippedDesiredSizeField.SetValue(this, sizeBox);
				}
				else
				{
					sizeBox.Width = size3.Width;
					sizeBox.Height = size3.Height;
				}
			}
			else if (sizeBox != null)
			{
				FrameworkElement.UnclippedDesiredSizeField.ClearValue(this);
			}
			return new Size(Math.Max(0.0, num3), Math.Max(0.0, num4));
		}

		/// <summary>Implements <see cref="M:System.Windows.UIElement.ArrangeCore(System.Windows.Rect)" /> (defined as virtual in <see cref="T:System.Windows.UIElement" />) and seals the implementation.</summary>
		/// <param name="finalRect">The final area within the parent that this element should use to arrange itself and its children.</param>
		// Token: 0x060005B4 RID: 1460 RVA: 0x000118C4 File Offset: 0x0000FAC4
		protected sealed override void ArrangeCore(Rect finalRect)
		{
			bool useLayoutRounding = this.UseLayoutRounding;
			DpiScale dpi = base.GetDpi();
			FrameworkElement.LayoutTransformData value = FrameworkElement.LayoutTransformDataField.GetValue(this);
			Size size = Size.Empty;
			if (useLayoutRounding && !base.CheckFlagsAnd(VisualFlags.UseLayoutRounding))
			{
				base.SetFlags(true, VisualFlags.UseLayoutRounding);
			}
			if (this.BypassLayoutPolicies)
			{
				Size renderSize = base.RenderSize;
				Size renderSize2 = this.ArrangeOverride(finalRect.Size);
				base.RenderSize = renderSize2;
				this.SetLayoutOffset(new Vector(finalRect.X, finalRect.Y), renderSize);
				return;
			}
			this.NeedsClipBounds = false;
			Size size2 = finalRect.Size;
			Thickness margin = this.Margin;
			double num = margin.Left + margin.Right;
			double num2 = margin.Top + margin.Bottom;
			if (useLayoutRounding && !FrameworkAppContextSwitches.DoNotApplyLayoutRoundingToMarginsAndBorderThickness)
			{
				num = UIElement.RoundLayoutValue(num, dpi.DpiScaleX);
				num2 = UIElement.RoundLayoutValue(num2, dpi.DpiScaleY);
			}
			size2.Width = Math.Max(0.0, size2.Width - num);
			size2.Height = Math.Max(0.0, size2.Height - num2);
			if (useLayoutRounding && value != null)
			{
				Size transformedUnroundedDS = value.TransformedUnroundedDS;
				size = value.TransformedUnroundedDS;
				size.Width = Math.Max(0.0, size.Width - num);
				size.Height = Math.Max(0.0, size.Height - num2);
			}
			SizeBox value2 = FrameworkElement.UnclippedDesiredSizeField.GetValue(this);
			Size untransformedDS;
			if (value2 == null)
			{
				untransformedDS = new Size(Math.Max(0.0, base.DesiredSize.Width - num), Math.Max(0.0, base.DesiredSize.Height - num2));
				if (size != Size.Empty)
				{
					untransformedDS.Width = Math.Max(size.Width, untransformedDS.Width);
					untransformedDS.Height = Math.Max(size.Height, untransformedDS.Height);
				}
			}
			else
			{
				untransformedDS = new Size(value2.Width, value2.Height);
			}
			if (DoubleUtil.LessThan(size2.Width, untransformedDS.Width))
			{
				this.NeedsClipBounds = true;
				size2.Width = untransformedDS.Width;
			}
			if (DoubleUtil.LessThan(size2.Height, untransformedDS.Height))
			{
				this.NeedsClipBounds = true;
				size2.Height = untransformedDS.Height;
			}
			if (this.HorizontalAlignment != HorizontalAlignment.Stretch)
			{
				size2.Width = untransformedDS.Width;
			}
			if (this.VerticalAlignment != VerticalAlignment.Stretch)
			{
				size2.Height = untransformedDS.Height;
			}
			if (value != null)
			{
				Size size3 = this.FindMaximalAreaLocalSpaceRect(value.Transform, size2);
				size2 = size3;
				untransformedDS = value.UntransformedDS;
				if (!DoubleUtil.IsZero(size3.Width) && !DoubleUtil.IsZero(size3.Height) && (LayoutDoubleUtil.LessThan(size3.Width, untransformedDS.Width) || LayoutDoubleUtil.LessThan(size3.Height, untransformedDS.Height)))
				{
					size2 = untransformedDS;
				}
				if (DoubleUtil.LessThan(size2.Width, untransformedDS.Width))
				{
					this.NeedsClipBounds = true;
					size2.Width = untransformedDS.Width;
				}
				if (DoubleUtil.LessThan(size2.Height, untransformedDS.Height))
				{
					this.NeedsClipBounds = true;
					size2.Height = untransformedDS.Height;
				}
			}
			FrameworkElement.MinMax minMax = new FrameworkElement.MinMax(this);
			if (useLayoutRounding && !FrameworkAppContextSwitches.DoNotApplyLayoutRoundingToMarginsAndBorderThickness)
			{
				minMax.maxHeight = UIElement.RoundLayoutValue(minMax.maxHeight, dpi.DpiScaleY);
				minMax.maxWidth = UIElement.RoundLayoutValue(minMax.maxWidth, dpi.DpiScaleX);
				minMax.minHeight = UIElement.RoundLayoutValue(minMax.minHeight, dpi.DpiScaleY);
				minMax.minWidth = UIElement.RoundLayoutValue(minMax.minWidth, dpi.DpiScaleX);
			}
			double num3 = Math.Max(untransformedDS.Width, minMax.maxWidth);
			if (DoubleUtil.LessThan(num3, size2.Width))
			{
				this.NeedsClipBounds = true;
				size2.Width = num3;
			}
			double num4 = Math.Max(untransformedDS.Height, minMax.maxHeight);
			if (DoubleUtil.LessThan(num4, size2.Height))
			{
				this.NeedsClipBounds = true;
				size2.Height = num4;
			}
			if (useLayoutRounding)
			{
				size2 = UIElement.RoundLayoutSize(size2, dpi.DpiScaleX, dpi.DpiScaleY);
			}
			Size renderSize3 = base.RenderSize;
			Size renderSize4 = this.ArrangeOverride(size2);
			base.RenderSize = renderSize4;
			if (useLayoutRounding)
			{
				base.RenderSize = UIElement.RoundLayoutSize(base.RenderSize, dpi.DpiScaleX, dpi.DpiScaleY);
			}
			Size size4 = new Size(Math.Min(renderSize4.Width, minMax.maxWidth), Math.Min(renderSize4.Height, minMax.maxHeight));
			if (useLayoutRounding)
			{
				size4 = UIElement.RoundLayoutSize(size4, dpi.DpiScaleX, dpi.DpiScaleY);
			}
			this.NeedsClipBounds |= (DoubleUtil.LessThan(size4.Width, renderSize4.Width) || DoubleUtil.LessThan(size4.Height, renderSize4.Height));
			if (value != null)
			{
				Rect rect = Rect.Transform(new Rect(0.0, 0.0, size4.Width, size4.Height), value.Transform.Value);
				size4.Width = rect.Width;
				size4.Height = rect.Height;
				if (useLayoutRounding)
				{
					size4 = UIElement.RoundLayoutSize(size4, dpi.DpiScaleX, dpi.DpiScaleY);
				}
			}
			Size size5 = new Size(Math.Max(0.0, finalRect.Width - num), Math.Max(0.0, finalRect.Height - num2));
			if (useLayoutRounding)
			{
				size5 = UIElement.RoundLayoutSize(size5, dpi.DpiScaleX, dpi.DpiScaleY);
			}
			this.NeedsClipBounds |= (DoubleUtil.LessThan(size5.Width, size4.Width) || DoubleUtil.LessThan(size5.Height, size4.Height));
			Vector offset = this.ComputeAlignmentOffset(size5, size4);
			offset.X += finalRect.X + margin.Left;
			offset.Y += finalRect.Y + margin.Top;
			if (useLayoutRounding)
			{
				offset.X = UIElement.RoundLayoutValue(offset.X, dpi.DpiScaleX);
				offset.Y = UIElement.RoundLayoutValue(offset.Y, dpi.DpiScaleY);
			}
			this.SetLayoutOffset(offset, renderSize3);
		}

		/// <summary>Raises the <see cref="E:System.Windows.FrameworkElement.SizeChanged" /> event, using the specified information as part of the eventual event data. </summary>
		/// <param name="sizeInfo">Details of the old and new size involved in the change.</param>
		// Token: 0x060005B5 RID: 1461 RVA: 0x00011F74 File Offset: 0x00010174
		protected internal override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
		{
			SizeChangedEventArgs sizeChangedEventArgs = new SizeChangedEventArgs(this, sizeInfo);
			sizeChangedEventArgs.RoutedEvent = FrameworkElement.SizeChangedEvent;
			if (sizeInfo.WidthChanged)
			{
				this.HasWidthEverChanged = true;
				base.NotifyPropertyChange(new DependencyPropertyChangedEventArgs(FrameworkElement.ActualWidthProperty, FrameworkElement._actualWidthMetadata, sizeInfo.PreviousSize.Width, sizeInfo.NewSize.Width));
			}
			if (sizeInfo.HeightChanged)
			{
				this.HasHeightEverChanged = true;
				base.NotifyPropertyChange(new DependencyPropertyChangedEventArgs(FrameworkElement.ActualHeightProperty, FrameworkElement._actualHeightMetadata, sizeInfo.PreviousSize.Height, sizeInfo.NewSize.Height));
			}
			base.RaiseEvent(sizeChangedEventArgs);
		}

		// Token: 0x060005B6 RID: 1462 RVA: 0x00012030 File Offset: 0x00010230
		private Vector ComputeAlignmentOffset(Size clientSize, Size inkSize)
		{
			Vector result = default(Vector);
			HorizontalAlignment horizontalAlignment = this.HorizontalAlignment;
			VerticalAlignment verticalAlignment = this.VerticalAlignment;
			if (horizontalAlignment == HorizontalAlignment.Stretch && inkSize.Width > clientSize.Width)
			{
				horizontalAlignment = HorizontalAlignment.Left;
			}
			if (verticalAlignment == VerticalAlignment.Stretch && inkSize.Height > clientSize.Height)
			{
				verticalAlignment = VerticalAlignment.Top;
			}
			if (horizontalAlignment == HorizontalAlignment.Center || horizontalAlignment == HorizontalAlignment.Stretch)
			{
				result.X = (clientSize.Width - inkSize.Width) * 0.5;
			}
			else if (horizontalAlignment == HorizontalAlignment.Right)
			{
				result.X = clientSize.Width - inkSize.Width;
			}
			else
			{
				result.X = 0.0;
			}
			if (verticalAlignment == VerticalAlignment.Center || verticalAlignment == VerticalAlignment.Stretch)
			{
				result.Y = (clientSize.Height - inkSize.Height) * 0.5;
			}
			else if (verticalAlignment == VerticalAlignment.Bottom)
			{
				result.Y = clientSize.Height - inkSize.Height;
			}
			else
			{
				result.Y = 0.0;
			}
			return result;
		}

		/// <summary>Returns a geometry for a clipping mask. The mask applies if the layout system attempts to arrange an element that is larger than the available display space.</summary>
		/// <param name="layoutSlotSize">The size of the part of the element that does visual presentation. </param>
		/// <returns>The clipping geometry.</returns>
		// Token: 0x060005B7 RID: 1463 RVA: 0x0001212C File Offset: 0x0001032C
		protected override Geometry GetLayoutClip(Size layoutSlotSize)
		{
			bool useLayoutRounding = this.UseLayoutRounding;
			DpiScale dpi = base.GetDpi();
			if (useLayoutRounding && !base.CheckFlagsAnd(VisualFlags.UseLayoutRounding))
			{
				base.SetFlags(true, VisualFlags.UseLayoutRounding);
			}
			if (!this.NeedsClipBounds && !base.ClipToBounds)
			{
				return base.GetLayoutClip(layoutSlotSize);
			}
			FrameworkElement.MinMax minMax = new FrameworkElement.MinMax(this);
			if (useLayoutRounding && !FrameworkAppContextSwitches.DoNotApplyLayoutRoundingToMarginsAndBorderThickness)
			{
				minMax.maxHeight = UIElement.RoundLayoutValue(minMax.maxHeight, dpi.DpiScaleY);
				minMax.maxWidth = UIElement.RoundLayoutValue(minMax.maxWidth, dpi.DpiScaleX);
				minMax.minHeight = UIElement.RoundLayoutValue(minMax.minHeight, dpi.DpiScaleY);
				minMax.minWidth = UIElement.RoundLayoutValue(minMax.minWidth, dpi.DpiScaleX);
			}
			Size renderSize = base.RenderSize;
			double num = double.IsPositiveInfinity(minMax.maxWidth) ? renderSize.Width : minMax.maxWidth;
			double num2 = double.IsPositiveInfinity(minMax.maxHeight) ? renderSize.Height : minMax.maxHeight;
			bool flag = base.ClipToBounds || DoubleUtil.LessThan(num, renderSize.Width) || DoubleUtil.LessThan(num2, renderSize.Height);
			renderSize.Width = Math.Min(renderSize.Width, minMax.maxWidth);
			renderSize.Height = Math.Min(renderSize.Height, minMax.maxHeight);
			FrameworkElement.LayoutTransformData value = FrameworkElement.LayoutTransformDataField.GetValue(this);
			Rect rect = default(Rect);
			if (value != null)
			{
				rect = Rect.Transform(new Rect(0.0, 0.0, renderSize.Width, renderSize.Height), value.Transform.Value);
				renderSize.Width = rect.Width;
				renderSize.Height = rect.Height;
			}
			Thickness margin = this.Margin;
			double num3 = margin.Left + margin.Right;
			double num4 = margin.Top + margin.Bottom;
			Size clientSize = new Size(Math.Max(0.0, layoutSlotSize.Width - num3), Math.Max(0.0, layoutSlotSize.Height - num4));
			bool flag2 = base.ClipToBounds || DoubleUtil.LessThan(clientSize.Width, renderSize.Width) || DoubleUtil.LessThan(clientSize.Height, renderSize.Height);
			Transform flowDirectionTransform = this.GetFlowDirectionTransform();
			if (flag && !flag2)
			{
				Rect rect2 = new Rect(0.0, 0.0, num, num2);
				if (useLayoutRounding)
				{
					rect2 = UIElement.RoundLayoutRect(rect2, dpi.DpiScaleX, dpi.DpiScaleY);
				}
				RectangleGeometry rectangleGeometry = new RectangleGeometry(rect2);
				if (flowDirectionTransform != null)
				{
					rectangleGeometry.Transform = flowDirectionTransform;
				}
				return rectangleGeometry;
			}
			if (!flag2)
			{
				return null;
			}
			Vector vector = this.ComputeAlignmentOffset(clientSize, renderSize);
			if (value == null)
			{
				Rect rect3 = new Rect(-vector.X + rect.X, -vector.Y + rect.Y, clientSize.Width, clientSize.Height);
				if (useLayoutRounding)
				{
					rect3 = UIElement.RoundLayoutRect(rect3, dpi.DpiScaleX, dpi.DpiScaleY);
				}
				if (flag)
				{
					Rect rect4 = new Rect(0.0, 0.0, num, num2);
					if (useLayoutRounding)
					{
						rect4 = UIElement.RoundLayoutRect(rect4, dpi.DpiScaleX, dpi.DpiScaleY);
					}
					rect3.Intersect(rect4);
				}
				RectangleGeometry rectangleGeometry2 = new RectangleGeometry(rect3);
				if (flowDirectionTransform != null)
				{
					rectangleGeometry2.Transform = flowDirectionTransform;
				}
				return rectangleGeometry2;
			}
			Rect rect5 = new Rect(-vector.X + rect.X, -vector.Y + rect.Y, clientSize.Width, clientSize.Height);
			if (useLayoutRounding)
			{
				rect5 = UIElement.RoundLayoutRect(rect5, dpi.DpiScaleX, dpi.DpiScaleY);
			}
			RectangleGeometry rectangleGeometry3 = new RectangleGeometry(rect5);
			Matrix value2 = value.Transform.Value;
			if (value2.HasInverse)
			{
				value2.Invert();
				rectangleGeometry3.Transform = new MatrixTransform(value2);
			}
			if (flag)
			{
				Rect rect6 = new Rect(0.0, 0.0, num, num2);
				if (useLayoutRounding)
				{
					rect6 = UIElement.RoundLayoutRect(rect6, dpi.DpiScaleX, dpi.DpiScaleY);
				}
				RectangleGeometry geometry = new RectangleGeometry(rect6);
				PathGeometry pathGeometry = Geometry.Combine(geometry, rectangleGeometry3, GeometryCombineMode.Intersect, null);
				if (flowDirectionTransform != null)
				{
					pathGeometry.Transform = flowDirectionTransform;
				}
				return pathGeometry;
			}
			if (flowDirectionTransform != null)
			{
				if (rectangleGeometry3.Transform != null)
				{
					rectangleGeometry3.Transform = new MatrixTransform(rectangleGeometry3.Transform.Value * flowDirectionTransform.Value);
				}
				else
				{
					rectangleGeometry3.Transform = flowDirectionTransform;
				}
			}
			return rectangleGeometry3;
		}

		// Token: 0x060005B8 RID: 1464 RVA: 0x000125E4 File Offset: 0x000107E4
		internal Geometry GetLayoutClipInternal()
		{
			if (base.IsMeasureValid && base.IsArrangeValid)
			{
				return this.GetLayoutClip(base.PreviousArrangeRect.Size);
			}
			return null;
		}

		/// <summary>When overridden in a derived class, measures the size in layout required for child elements and determines a size for the <see cref="T:System.Windows.FrameworkElement" />-derived class. </summary>
		/// <param name="availableSize">The available size that this element can give to child elements. Infinity can be specified as a value to indicate that the element will size to whatever content is available.</param>
		/// <returns>The size that this element determines it needs during layout, based on its calculations of child element sizes.</returns>
		// Token: 0x060005B9 RID: 1465 RVA: 0x00012617 File Offset: 0x00010817
		protected virtual Size MeasureOverride(Size availableSize)
		{
			return new Size(0.0, 0.0);
		}

		/// <summary>When overridden in a derived class, positions child elements and determines a size for a <see cref="T:System.Windows.FrameworkElement" /> derived class. </summary>
		/// <param name="finalSize">The final area within the parent that this element should use to arrange itself and its children.</param>
		/// <returns>The actual size used.</returns>
		// Token: 0x060005BA RID: 1466 RVA: 0x00012630 File Offset: 0x00010830
		protected virtual Size ArrangeOverride(Size finalSize)
		{
			return finalSize;
		}

		// Token: 0x060005BB RID: 1467 RVA: 0x00012634 File Offset: 0x00010834
		internal static void InternalSetLayoutTransform(UIElement element, Transform layoutTransform)
		{
			FrameworkElement frameworkElement = element as FrameworkElement;
			element.InternalSetOffsetWorkaround(default(Vector));
			Transform transform = (frameworkElement == null) ? null : frameworkElement.GetFlowDirectionTransform();
			Transform transform2 = element.RenderTransform;
			if (transform2 == Transform.Identity)
			{
				transform2 = null;
			}
			TransformCollection transformCollection = new TransformCollection();
			transformCollection.CanBeInheritanceContext = false;
			if (transform != null)
			{
				transformCollection.Add(transform);
			}
			if (transform2 != null)
			{
				transformCollection.Add(transform2);
			}
			transformCollection.Add(layoutTransform);
			element.InternalSetTransformWorkaround(new TransformGroup
			{
				Children = transformCollection
			});
		}

		// Token: 0x060005BC RID: 1468 RVA: 0x000126B8 File Offset: 0x000108B8
		private void SetLayoutOffset(Vector offset, Size oldRenderSize)
		{
			if (!base.AreTransformsClean || !DoubleUtil.AreClose(base.RenderSize, oldRenderSize))
			{
				Transform flowDirectionTransform = this.GetFlowDirectionTransform();
				Transform transform = base.RenderTransform;
				if (transform == Transform.Identity)
				{
					transform = null;
				}
				FrameworkElement.LayoutTransformData value = FrameworkElement.LayoutTransformDataField.GetValue(this);
				TransformGroup transformGroup = null;
				if (flowDirectionTransform != null || transform != null || value != null)
				{
					transformGroup = new TransformGroup();
					transformGroup.CanBeInheritanceContext = false;
					transformGroup.Children.CanBeInheritanceContext = false;
					if (flowDirectionTransform != null)
					{
						transformGroup.Children.Add(flowDirectionTransform);
					}
					if (value != null)
					{
						transformGroup.Children.Add(value.Transform);
						FrameworkElement.MinMax minMax = new FrameworkElement.MinMax(this);
						Size renderSize = base.RenderSize;
						double num = double.IsPositiveInfinity(minMax.maxWidth) ? renderSize.Width : minMax.maxWidth;
						double num2 = double.IsPositiveInfinity(minMax.maxHeight) ? renderSize.Height : minMax.maxHeight;
						renderSize.Width = Math.Min(renderSize.Width, minMax.maxWidth);
						renderSize.Height = Math.Min(renderSize.Height, minMax.maxHeight);
						Rect rect = Rect.Transform(new Rect(renderSize), value.Transform.Value);
						transformGroup.Children.Add(new TranslateTransform(-rect.X, -rect.Y));
					}
					if (transform != null)
					{
						Point renderTransformOrigin = this.GetRenderTransformOrigin();
						bool flag = renderTransformOrigin.X != 0.0 || renderTransformOrigin.Y != 0.0;
						if (flag)
						{
							TranslateTransform translateTransform = new TranslateTransform(-renderTransformOrigin.X, -renderTransformOrigin.Y);
							translateTransform.Freeze();
							transformGroup.Children.Add(translateTransform);
						}
						transformGroup.Children.Add(transform);
						if (flag)
						{
							TranslateTransform translateTransform2 = new TranslateTransform(renderTransformOrigin.X, renderTransformOrigin.Y);
							translateTransform2.Freeze();
							transformGroup.Children.Add(translateTransform2);
						}
					}
				}
				base.VisualTransform = transformGroup;
				base.AreTransformsClean = true;
			}
			Vector visualOffset = base.VisualOffset;
			if (!DoubleUtil.AreClose(visualOffset.X, offset.X) || !DoubleUtil.AreClose(visualOffset.Y, offset.Y))
			{
				base.VisualOffset = offset;
			}
		}

		// Token: 0x060005BD RID: 1469 RVA: 0x00012900 File Offset: 0x00010B00
		private Point GetRenderTransformOrigin()
		{
			Point renderTransformOrigin = base.RenderTransformOrigin;
			Size renderSize = base.RenderSize;
			return new Point(renderSize.Width * renderTransformOrigin.X, renderSize.Height * renderTransformOrigin.Y);
		}

		/// <summary>Moves the keyboard focus away from this element and to another element in a provided traversal direction. </summary>
		/// <param name="request">The direction that focus is to be moved, as a value of the enumeration.</param>
		/// <returns>Returns <see langword="true" /> if focus is moved successfully; <see langword="false" /> if the target element in direction as specified does not exist or could not be keyboard focused.</returns>
		// Token: 0x060005BE RID: 1470 RVA: 0x0000CA4C File Offset: 0x0000AC4C
		public sealed override bool MoveFocus(TraversalRequest request)
		{
			if (request == null)
			{
				throw new ArgumentNullException("request");
			}
			return KeyboardNavigation.Current.Navigate(this, request);
		}

		/// <summary>Determines the next element that would receive focus relative to this element for a provided focus movement direction, but does not actually move the focus.</summary>
		/// <param name="direction">The direction for which a prospective focus change should be determined.</param>
		/// <returns>The next element that focus would move to if focus were actually traversed. May return <see langword="null" /> if focus cannot be moved relative to this element for the provided direction.</returns>
		/// <exception cref="T:System.ComponentModel.InvalidEnumArgumentException">Specified one of the following directions in the <see cref="T:System.Windows.Input.TraversalRequest" />: <see cref="F:System.Windows.Input.FocusNavigationDirection.Next" />, <see cref="F:System.Windows.Input.FocusNavigationDirection.Previous" />, <see cref="F:System.Windows.Input.FocusNavigationDirection.First" />, <see cref="F:System.Windows.Input.FocusNavigationDirection.Last" />. These directions are not legal for <see cref="M:System.Windows.FrameworkElement.PredictFocus(System.Windows.Input.FocusNavigationDirection)" /> (but they are legal for <see cref="M:System.Windows.FrameworkElement.MoveFocus(System.Windows.Input.TraversalRequest)" />). </exception>
		// Token: 0x060005BF RID: 1471 RVA: 0x0000CA68 File Offset: 0x0000AC68
		public sealed override DependencyObject PredictFocus(FocusNavigationDirection direction)
		{
			return KeyboardNavigation.Current.PredictFocusedElement(this, direction);
		}

		// Token: 0x060005C0 RID: 1472 RVA: 0x00012940 File Offset: 0x00010B40
		private static void OnPreviewGotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
		{
			if (e.OriginalSource == sender)
			{
				FrameworkElement element = (FrameworkElement)sender;
				IInputElement focusedElement = FocusManager.GetFocusedElement(element, true);
				if (focusedElement != null && focusedElement != sender && Keyboard.IsFocusable(focusedElement as DependencyObject))
				{
					IInputElement focusedElement2 = Keyboard.FocusedElement;
					focusedElement.Focus();
					if (Keyboard.FocusedElement == focusedElement || Keyboard.FocusedElement != focusedElement2)
					{
						e.Handled = true;
						return;
					}
				}
			}
		}

		// Token: 0x060005C1 RID: 1473 RVA: 0x000129A0 File Offset: 0x00010BA0
		private static void OnGotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
		{
			if (sender == e.OriginalSource)
			{
				FrameworkElement frameworkElement = (FrameworkElement)sender;
				KeyboardNavigation.UpdateFocusedElement(frameworkElement);
				KeyboardNavigation keyboardNavigation = KeyboardNavigation.Current;
				KeyboardNavigation.ShowFocusVisual();
				keyboardNavigation.NotifyFocusChanged(frameworkElement, e);
				keyboardNavigation.UpdateActiveElement(frameworkElement);
			}
		}

		// Token: 0x060005C2 RID: 1474 RVA: 0x0000CAC5 File Offset: 0x0000ACC5
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

		/// <summary>Invoked whenever an unhandled <see cref="E:System.Windows.UIElement.GotFocus" /> event reaches this element in its route.</summary>
		/// <param name="e">The <see cref="T:System.Windows.RoutedEventArgs" /> that contains the event data.</param>
		// Token: 0x060005C3 RID: 1475 RVA: 0x000129DD File Offset: 0x00010BDD
		protected override void OnGotFocus(RoutedEventArgs e)
		{
			if (base.IsKeyboardFocused)
			{
				this.BringIntoView();
			}
			base.OnGotFocus(e);
		}

		/// <summary>Starts the initialization process for this element. </summary>
		// Token: 0x060005C4 RID: 1476 RVA: 0x000129F4 File Offset: 0x00010BF4
		public virtual void BeginInit()
		{
			if (this.ReadInternalFlag(InternalFlags.InitPending))
			{
				throw new InvalidOperationException(SR.Get("NestedBeginInitNotSupported"));
			}
			this.WriteInternalFlag(InternalFlags.InitPending, true);
		}

		/// <summary>Indicates that the initialization process for the element is complete. </summary>
		/// <exception cref="T:System.InvalidOperationException">
		///         <see cref="M:System.Windows.FrameworkElement.EndInit" /> was called without <see cref="M:System.Windows.FrameworkElement.BeginInit" /> having previously been called on the element.</exception>
		// Token: 0x060005C5 RID: 1477 RVA: 0x00012A1F File Offset: 0x00010C1F
		public virtual void EndInit()
		{
			if (!this.ReadInternalFlag(InternalFlags.InitPending))
			{
				throw new InvalidOperationException(SR.Get("EndInitWithoutBeginInitNotSupported"));
			}
			this.WriteInternalFlag(InternalFlags.InitPending, false);
			this.TryFireInitialized();
		}

		/// <summary>Gets a value that indicates whether this element has been initialized, either during processing by a XAML processor, or by explicitly having its <see cref="M:System.Windows.FrameworkElement.EndInit" /> method called. </summary>
		/// <returns>
		///     <see langword="true" /> if the element is initialized per the aforementioned XAML processing or method calls; otherwise, <see langword="false" />.</returns>
		// Token: 0x17000103 RID: 259
		// (get) Token: 0x060005C6 RID: 1478 RVA: 0x00012A50 File Offset: 0x00010C50
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public bool IsInitialized
		{
			get
			{
				return this.ReadInternalFlag(InternalFlags.IsInitialized);
			}
		}

		/// <summary>Occurs when this <see cref="T:System.Windows.FrameworkElement" /> is initialized. This event coincides with cases where the value of the <see cref="P:System.Windows.FrameworkElement.IsInitialized" /> property changes from <see langword="false" /> (or undefined) to <see langword="true" />. </summary>
		// Token: 0x14000020 RID: 32
		// (add) Token: 0x060005C7 RID: 1479 RVA: 0x00012A5D File Offset: 0x00010C5D
		// (remove) Token: 0x060005C8 RID: 1480 RVA: 0x00012A6B File Offset: 0x00010C6B
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

		/// <summary>Raises the <see cref="E:System.Windows.FrameworkElement.Initialized" /> event. This method is invoked whenever <see cref="P:System.Windows.FrameworkElement.IsInitialized" /> is set to <see langword="true " />internally. </summary>
		/// <param name="e">The <see cref="T:System.Windows.RoutedEventArgs" /> that contains the event data.</param>
		// Token: 0x060005C9 RID: 1481 RVA: 0x00012A79 File Offset: 0x00010C79
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

		// Token: 0x060005CA RID: 1482 RVA: 0x00012AA3 File Offset: 0x00010CA3
		private void TryFireInitialized()
		{
			if (!this.ReadInternalFlag(InternalFlags.InitPending) && !this.ReadInternalFlag(InternalFlags.IsInitialized))
			{
				this.WriteInternalFlag(InternalFlags.IsInitialized, true);
				this.PrivateInitialized();
				this.OnInitialized(EventArgs.Empty);
			}
		}

		// Token: 0x060005CB RID: 1483 RVA: 0x00012ADC File Offset: 0x00010CDC
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

		// Token: 0x060005CC RID: 1484 RVA: 0x00012B0B File Offset: 0x00010D0B
		private static void NumberSubstitutionChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
		{
			((FrameworkElement)o).HasNumberSubstitutionChanged = true;
		}

		// Token: 0x060005CD RID: 1485 RVA: 0x00012B1C File Offset: 0x00010D1C
		private static bool ShouldUseSystemFont(FrameworkElement fe, DependencyProperty dp)
		{
			bool flag;
			return (SystemResources.SystemResourcesAreChanging || (fe.ReadInternalFlag(InternalFlags.CreatingRoot) && SystemResources.SystemResourcesHaveChanged)) && fe._parent == null && VisualTreeHelper.GetParent(fe) == null && fe.GetValueSource(dp, null, out flag) == BaseValueSourceInternal.Default;
		}

		// Token: 0x060005CE RID: 1486 RVA: 0x00012B63 File Offset: 0x00010D63
		private static object CoerceFontFamily(DependencyObject o, object value)
		{
			if (FrameworkElement.ShouldUseSystemFont((FrameworkElement)o, TextElement.FontFamilyProperty))
			{
				return SystemFonts.MessageFontFamily;
			}
			return value;
		}

		// Token: 0x060005CF RID: 1487 RVA: 0x00012B7E File Offset: 0x00010D7E
		private static object CoerceFontSize(DependencyObject o, object value)
		{
			if (FrameworkElement.ShouldUseSystemFont((FrameworkElement)o, TextElement.FontSizeProperty))
			{
				return SystemFonts.MessageFontSize;
			}
			return value;
		}

		// Token: 0x060005D0 RID: 1488 RVA: 0x00012B9E File Offset: 0x00010D9E
		private static object CoerceFontStyle(DependencyObject o, object value)
		{
			if (FrameworkElement.ShouldUseSystemFont((FrameworkElement)o, TextElement.FontStyleProperty))
			{
				return SystemFonts.MessageFontStyle;
			}
			return value;
		}

		// Token: 0x060005D1 RID: 1489 RVA: 0x00012BBE File Offset: 0x00010DBE
		private static object CoerceFontWeight(DependencyObject o, object value)
		{
			if (FrameworkElement.ShouldUseSystemFont((FrameworkElement)o, TextElement.FontWeightProperty))
			{
				return SystemFonts.MessageFontWeight;
			}
			return value;
		}

		// Token: 0x060005D2 RID: 1490 RVA: 0x00012BE0 File Offset: 0x00010DE0
		internal sealed override void OnPresentationSourceChanged(bool attached)
		{
			base.OnPresentationSourceChanged(attached);
			if (attached)
			{
				this.FireLoadedOnDescendentsInternal();
				if (SystemResources.SystemResourcesHaveChanged)
				{
					this.WriteInternalFlag(InternalFlags.CreatingRoot, true);
					base.CoerceValue(TextElement.FontFamilyProperty);
					base.CoerceValue(TextElement.FontSizeProperty);
					base.CoerceValue(TextElement.FontStyleProperty);
					base.CoerceValue(TextElement.FontWeightProperty);
					this.WriteInternalFlag(InternalFlags.CreatingRoot, false);
					return;
				}
			}
			else
			{
				this.FireUnloadedOnDescendentsInternal();
			}
		}

		/// <summary>Gets a value that indicates whether this element has been loaded for presentation. </summary>
		/// <returns>
		///     <see langword="true" /> if the current element is attached to an element tree; <see langword="false" /> if the element has never been attached to a loaded element tree. </returns>
		// Token: 0x17000104 RID: 260
		// (get) Token: 0x060005D3 RID: 1491 RVA: 0x00012C50 File Offset: 0x00010E50
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

		/// <summary>Occurs when the element is laid out, rendered, and ready for interaction.</summary>
		// Token: 0x14000021 RID: 33
		// (add) Token: 0x060005D4 RID: 1492 RVA: 0x00012C8B File Offset: 0x00010E8B
		// (remove) Token: 0x060005D5 RID: 1493 RVA: 0x00012C9A File Offset: 0x00010E9A
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

		// Token: 0x060005D6 RID: 1494 RVA: 0x00012CA8 File Offset: 0x00010EA8
		internal override void OnAddHandler(RoutedEvent routedEvent, Delegate handler)
		{
			base.OnAddHandler(routedEvent, handler);
			if (routedEvent == FrameworkElement.LoadedEvent || routedEvent == FrameworkElement.UnloadedEvent)
			{
				BroadcastEventHelper.AddHasLoadedChangeHandlerFlagInAncestry(this);
			}
		}

		// Token: 0x060005D7 RID: 1495 RVA: 0x00012CC8 File Offset: 0x00010EC8
		internal override void OnRemoveHandler(RoutedEvent routedEvent, Delegate handler)
		{
			base.OnRemoveHandler(routedEvent, handler);
			if (routedEvent != FrameworkElement.LoadedEvent && routedEvent != FrameworkElement.UnloadedEvent)
			{
				return;
			}
			if (!this.ThisHasLoadedChangeEventHandler)
			{
				BroadcastEventHelper.RemoveHasLoadedChangeHandlerFlagInAncestry(this);
			}
		}

		// Token: 0x060005D8 RID: 1496 RVA: 0x00012CF1 File Offset: 0x00010EF1
		internal void OnLoaded(RoutedEventArgs args)
		{
			base.RaiseEvent(args);
		}

		/// <summary>Occurs when the element is removed from within an element tree of loaded elements.</summary>
		// Token: 0x14000022 RID: 34
		// (add) Token: 0x060005D9 RID: 1497 RVA: 0x00012CFA File Offset: 0x00010EFA
		// (remove) Token: 0x060005DA RID: 1498 RVA: 0x00012D09 File Offset: 0x00010F09
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

		// Token: 0x060005DB RID: 1499 RVA: 0x00012CF1 File Offset: 0x00010EF1
		internal void OnUnloaded(RoutedEventArgs args)
		{
			base.RaiseEvent(args);
		}

		// Token: 0x060005DC RID: 1500 RVA: 0x00012D18 File Offset: 0x00010F18
		internal override void AddSynchronizedInputPreOpportunityHandlerCore(EventRoute route, RoutedEventArgs args)
		{
			UIElement uielement = this._templatedParent as UIElement;
			if (uielement != null)
			{
				uielement.AddSynchronizedInputPreOpportunityHandler(route, args);
			}
		}

		// Token: 0x060005DD RID: 1501 RVA: 0x00012D3C File Offset: 0x00010F3C
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

		// Token: 0x17000105 RID: 261
		// (get) Token: 0x060005DE RID: 1502 RVA: 0x00012D6B File Offset: 0x00010F6B
		internal static PopupControlService PopupControlService
		{
			get
			{
				return FrameworkElement.EnsureFrameworkServices()._popupControlService;
			}
		}

		// Token: 0x17000106 RID: 262
		// (get) Token: 0x060005DF RID: 1503 RVA: 0x00012D77 File Offset: 0x00010F77
		internal static KeyboardNavigation KeyboardNavigation
		{
			get
			{
				return FrameworkElement.EnsureFrameworkServices()._keyboardNavigation;
			}
		}

		// Token: 0x060005E0 RID: 1504 RVA: 0x00012D83 File Offset: 0x00010F83
		private static FrameworkElement.FrameworkServices EnsureFrameworkServices()
		{
			if (FrameworkElement._frameworkServices == null)
			{
				FrameworkElement._frameworkServices = new FrameworkElement.FrameworkServices();
			}
			return FrameworkElement._frameworkServices;
		}

		/// <summary> Gets or sets the tool-tip object that is displayed for this element in the user interface (UI).</summary>
		/// <returns>The tooltip object. See Remarks below for details on why this parameter is not strongly typed.</returns>
		// Token: 0x17000107 RID: 263
		// (get) Token: 0x060005E1 RID: 1505 RVA: 0x0000D02F File Offset: 0x0000B22F
		// (set) Token: 0x060005E2 RID: 1506 RVA: 0x0000D037 File Offset: 0x0000B237
		[Bindable(true)]
		[Category("Appearance")]
		[Localizability(LocalizationCategory.ToolTip)]
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

		/// <summary> Gets or sets the context menu element that should appear whenever the context menu is requested through user interface (UI) from within this element.</summary>
		/// <returns>The context menu assigned to this element. </returns>
		// Token: 0x17000108 RID: 264
		// (get) Token: 0x060005E3 RID: 1507 RVA: 0x00012D9B File Offset: 0x00010F9B
		// (set) Token: 0x060005E4 RID: 1508 RVA: 0x00012DAD File Offset: 0x00010FAD
		public ContextMenu ContextMenu
		{
			get
			{
				return base.GetValue(FrameworkElement.ContextMenuProperty) as ContextMenu;
			}
			set
			{
				base.SetValue(FrameworkElement.ContextMenuProperty, value);
			}
		}

		/// <summary>Occurs when any tooltip on the element is opened. </summary>
		// Token: 0x14000023 RID: 35
		// (add) Token: 0x060005E5 RID: 1509 RVA: 0x00012DBB File Offset: 0x00010FBB
		// (remove) Token: 0x060005E6 RID: 1510 RVA: 0x00012DC9 File Offset: 0x00010FC9
		public event ToolTipEventHandler ToolTipOpening
		{
			add
			{
				base.AddHandler(FrameworkElement.ToolTipOpeningEvent, value);
			}
			remove
			{
				base.RemoveHandler(FrameworkElement.ToolTipOpeningEvent, value);
			}
		}

		// Token: 0x060005E7 RID: 1511 RVA: 0x00012DD7 File Offset: 0x00010FD7
		private static void OnToolTipOpeningThunk(object sender, ToolTipEventArgs e)
		{
			((FrameworkElement)sender).OnToolTipOpening(e);
		}

		/// <summary> Invoked whenever the <see cref="E:System.Windows.FrameworkElement.ToolTipOpening" /> routed event reaches this class in its route. Implement this method to add class handling for this event. </summary>
		/// <param name="e">Provides data about the event.</param>
		// Token: 0x060005E8 RID: 1512 RVA: 0x00002137 File Offset: 0x00000337
		protected virtual void OnToolTipOpening(ToolTipEventArgs e)
		{
		}

		/// <summary>Occurs just before any tooltip on the element is closed. </summary>
		// Token: 0x14000024 RID: 36
		// (add) Token: 0x060005E9 RID: 1513 RVA: 0x00012DE5 File Offset: 0x00010FE5
		// (remove) Token: 0x060005EA RID: 1514 RVA: 0x00012DF3 File Offset: 0x00010FF3
		public event ToolTipEventHandler ToolTipClosing
		{
			add
			{
				base.AddHandler(FrameworkElement.ToolTipClosingEvent, value);
			}
			remove
			{
				base.RemoveHandler(FrameworkElement.ToolTipClosingEvent, value);
			}
		}

		// Token: 0x060005EB RID: 1515 RVA: 0x00012E01 File Offset: 0x00011001
		private static void OnToolTipClosingThunk(object sender, ToolTipEventArgs e)
		{
			((FrameworkElement)sender).OnToolTipClosing(e);
		}

		/// <summary> Invoked whenever an unhandled <see cref="E:System.Windows.FrameworkElement.ToolTipClosing" /> routed event reaches this class in its route. Implement this method to add class handling for this event. </summary>
		/// <param name="e">Provides data about the event.</param>
		// Token: 0x060005EC RID: 1516 RVA: 0x00002137 File Offset: 0x00000337
		protected virtual void OnToolTipClosing(ToolTipEventArgs e)
		{
		}

		/// <summary>Occurs when any context menu on the element is opened. </summary>
		// Token: 0x14000025 RID: 37
		// (add) Token: 0x060005ED RID: 1517 RVA: 0x00012E0F File Offset: 0x0001100F
		// (remove) Token: 0x060005EE RID: 1518 RVA: 0x00012E1D File Offset: 0x0001101D
		public event ContextMenuEventHandler ContextMenuOpening
		{
			add
			{
				base.AddHandler(FrameworkElement.ContextMenuOpeningEvent, value);
			}
			remove
			{
				base.RemoveHandler(FrameworkElement.ContextMenuOpeningEvent, value);
			}
		}

		// Token: 0x060005EF RID: 1519 RVA: 0x00012E2B File Offset: 0x0001102B
		private static void OnContextMenuOpeningThunk(object sender, ContextMenuEventArgs e)
		{
			((FrameworkElement)sender).OnContextMenuOpening(e);
		}

		/// <summary> Invoked whenever an unhandled <see cref="E:System.Windows.FrameworkElement.ContextMenuOpening" /> routed event reaches this class in its route. Implement this method to add class handling for this event. </summary>
		/// <param name="e">The <see cref="T:System.Windows.RoutedEventArgs" /> that contains the event data.</param>
		// Token: 0x060005F0 RID: 1520 RVA: 0x00002137 File Offset: 0x00000337
		protected virtual void OnContextMenuOpening(ContextMenuEventArgs e)
		{
		}

		/// <summary>Occurs just before any context menu on the element is closed. </summary>
		// Token: 0x14000026 RID: 38
		// (add) Token: 0x060005F1 RID: 1521 RVA: 0x00012E39 File Offset: 0x00011039
		// (remove) Token: 0x060005F2 RID: 1522 RVA: 0x00012E47 File Offset: 0x00011047
		public event ContextMenuEventHandler ContextMenuClosing
		{
			add
			{
				base.AddHandler(FrameworkElement.ContextMenuClosingEvent, value);
			}
			remove
			{
				base.RemoveHandler(FrameworkElement.ContextMenuClosingEvent, value);
			}
		}

		// Token: 0x060005F3 RID: 1523 RVA: 0x00012E55 File Offset: 0x00011055
		private static void OnContextMenuClosingThunk(object sender, ContextMenuEventArgs e)
		{
			((FrameworkElement)sender).OnContextMenuClosing(e);
		}

		/// <summary> Invoked whenever an unhandled <see cref="E:System.Windows.FrameworkElement.ContextMenuClosing" /> routed event reaches this class in its route. Implement this method to add class handling for this event. </summary>
		/// <param name="e">Provides data about the event.</param>
		// Token: 0x060005F4 RID: 1524 RVA: 0x00002137 File Offset: 0x00000337
		protected virtual void OnContextMenuClosing(ContextMenuEventArgs e)
		{
		}

		// Token: 0x060005F5 RID: 1525 RVA: 0x00012E64 File Offset: 0x00011064
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

		// Token: 0x060005F6 RID: 1526 RVA: 0x00012E94 File Offset: 0x00011094
		internal static void AddIntermediateElementsToRoute(DependencyObject mergePoint, EventRoute route, RoutedEventArgs args, DependencyObject modelTreeNode)
		{
			while (modelTreeNode != null && modelTreeNode != mergePoint)
			{
				UIElement uielement = modelTreeNode as UIElement;
				ContentElement contentElement = modelTreeNode as ContentElement;
				UIElement3D uielement3D = modelTreeNode as UIElement3D;
				if (uielement != null)
				{
					uielement.AddToEventRoute(route, args);
					FrameworkElement frameworkElement = uielement as FrameworkElement;
					if (frameworkElement != null)
					{
						FrameworkElement.AddStyleHandlersToEventRoute(frameworkElement, null, route, args);
					}
				}
				else if (contentElement != null)
				{
					contentElement.AddToEventRoute(route, args);
					FrameworkContentElement frameworkContentElement = contentElement as FrameworkContentElement;
					if (frameworkContentElement != null)
					{
						FrameworkElement.AddStyleHandlersToEventRoute(null, frameworkContentElement, route, args);
					}
				}
				else if (uielement3D != null)
				{
					uielement3D.AddToEventRoute(route, args);
				}
				modelTreeNode = LogicalTreeHelper.GetParent(modelTreeNode);
			}
		}

		// Token: 0x060005F7 RID: 1527 RVA: 0x0000CCFF File Offset: 0x0000AEFF
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

		// Token: 0x060005F8 RID: 1528 RVA: 0x00012F15 File Offset: 0x00011115
		internal void EventHandlersStoreAdd(EventPrivateKey key, Delegate handler)
		{
			base.EnsureEventHandlersStore();
			base.EventHandlersStore.Add(key, handler);
		}

		// Token: 0x060005F9 RID: 1529 RVA: 0x00012F2C File Offset: 0x0001112C
		internal void EventHandlersStoreRemove(EventPrivateKey key, Delegate handler)
		{
			EventHandlersStore eventHandlersStore = base.EventHandlersStore;
			if (eventHandlersStore != null)
			{
				eventHandlersStore.Remove(key, handler);
			}
		}

		// Token: 0x17000109 RID: 265
		// (get) Token: 0x060005FA RID: 1530 RVA: 0x00012F4B File Offset: 0x0001114B
		// (set) Token: 0x060005FB RID: 1531 RVA: 0x00012F54 File Offset: 0x00011154
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

		// Token: 0x1700010A RID: 266
		// (get) Token: 0x060005FC RID: 1532 RVA: 0x00012F5E File Offset: 0x0001115E
		// (set) Token: 0x060005FD RID: 1533 RVA: 0x00012F6B File Offset: 0x0001116B
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

		// Token: 0x1700010B RID: 267
		// (get) Token: 0x060005FE RID: 1534 RVA: 0x00012F79 File Offset: 0x00011179
		// (set) Token: 0x060005FF RID: 1535 RVA: 0x00012F86 File Offset: 0x00011186
		internal bool InVisibilityCollapsedTree
		{
			get
			{
				return this.ReadInternalFlag(InternalFlags.InVisibilityCollapsedTree);
			}
			set
			{
				this.WriteInternalFlag(InternalFlags.InVisibilityCollapsedTree, value);
			}
		}

		// Token: 0x1700010C RID: 268
		// (get) Token: 0x06000600 RID: 1536 RVA: 0x00012F94 File Offset: 0x00011194
		// (set) Token: 0x06000601 RID: 1537 RVA: 0x00012FA1 File Offset: 0x000111A1
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

		// Token: 0x1700010D RID: 269
		// (get) Token: 0x06000602 RID: 1538 RVA: 0x00012FAF File Offset: 0x000111AF
		// (set) Token: 0x06000603 RID: 1539 RVA: 0x00012FBC File Offset: 0x000111BC
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

		// Token: 0x1700010E RID: 270
		// (get) Token: 0x06000604 RID: 1540 RVA: 0x00012FCA File Offset: 0x000111CA
		// (set) Token: 0x06000605 RID: 1541 RVA: 0x00012FD7 File Offset: 0x000111D7
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

		// Token: 0x1700010F RID: 271
		// (get) Token: 0x06000606 RID: 1542 RVA: 0x00012FE5 File Offset: 0x000111E5
		// (set) Token: 0x06000607 RID: 1543 RVA: 0x00012FF2 File Offset: 0x000111F2
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

		// Token: 0x17000110 RID: 272
		// (get) Token: 0x06000608 RID: 1544 RVA: 0x00013000 File Offset: 0x00011200
		// (set) Token: 0x06000609 RID: 1545 RVA: 0x0001300D File Offset: 0x0001120D
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

		// Token: 0x17000111 RID: 273
		// (get) Token: 0x0600060A RID: 1546 RVA: 0x0001301B File Offset: 0x0001121B
		// (set) Token: 0x0600060B RID: 1547 RVA: 0x00013028 File Offset: 0x00011228
		private bool NeedsClipBounds
		{
			get
			{
				return this.ReadInternalFlag(InternalFlags.NeedsClipBounds);
			}
			set
			{
				this.WriteInternalFlag(InternalFlags.NeedsClipBounds, value);
			}
		}

		// Token: 0x17000112 RID: 274
		// (get) Token: 0x0600060C RID: 1548 RVA: 0x00013036 File Offset: 0x00011236
		// (set) Token: 0x0600060D RID: 1549 RVA: 0x00013043 File Offset: 0x00011243
		private bool HasWidthEverChanged
		{
			get
			{
				return this.ReadInternalFlag(InternalFlags.HasWidthEverChanged);
			}
			set
			{
				this.WriteInternalFlag(InternalFlags.HasWidthEverChanged, value);
			}
		}

		// Token: 0x17000113 RID: 275
		// (get) Token: 0x0600060E RID: 1550 RVA: 0x00013051 File Offset: 0x00011251
		// (set) Token: 0x0600060F RID: 1551 RVA: 0x0001305E File Offset: 0x0001125E
		private bool HasHeightEverChanged
		{
			get
			{
				return this.ReadInternalFlag(InternalFlags.HasHeightEverChanged);
			}
			set
			{
				this.WriteInternalFlag(InternalFlags.HasHeightEverChanged, value);
			}
		}

		// Token: 0x17000114 RID: 276
		// (get) Token: 0x06000610 RID: 1552 RVA: 0x0001306C File Offset: 0x0001126C
		// (set) Token: 0x06000611 RID: 1553 RVA: 0x00013079 File Offset: 0x00011279
		internal bool IsRightToLeft
		{
			get
			{
				return this.ReadInternalFlag(InternalFlags.IsRightToLeft);
			}
			set
			{
				this.WriteInternalFlag(InternalFlags.IsRightToLeft, value);
			}
		}

		// Token: 0x17000115 RID: 277
		// (get) Token: 0x06000612 RID: 1554 RVA: 0x00013088 File Offset: 0x00011288
		// (set) Token: 0x06000613 RID: 1555 RVA: 0x000130B0 File Offset: 0x000112B0
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

		// Token: 0x17000116 RID: 278
		// (get) Token: 0x06000614 RID: 1556 RVA: 0x000130FF File Offset: 0x000112FF
		// (set) Token: 0x06000615 RID: 1557 RVA: 0x0001310C File Offset: 0x0001130C
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

		// Token: 0x17000117 RID: 279
		// (get) Token: 0x06000616 RID: 1558 RVA: 0x0001311A File Offset: 0x0001131A
		// (set) Token: 0x06000617 RID: 1559 RVA: 0x00013127 File Offset: 0x00011327
		internal bool BypassLayoutPolicies
		{
			get
			{
				return this.ReadInternalFlag2((InternalFlags2)2147483648U);
			}
			set
			{
				this.WriteInternalFlag2((InternalFlags2)2147483648U, value);
			}
		}

		// Token: 0x06000618 RID: 1560 RVA: 0x00013135 File Offset: 0x00011335
		internal bool ReadInternalFlag(InternalFlags reqFlag)
		{
			return (this._flags & reqFlag) > (InternalFlags)0U;
		}

		// Token: 0x06000619 RID: 1561 RVA: 0x00013142 File Offset: 0x00011342
		internal bool ReadInternalFlag2(InternalFlags2 reqFlag)
		{
			return (this._flags2 & reqFlag) > (InternalFlags2)0U;
		}

		// Token: 0x0600061A RID: 1562 RVA: 0x0001314F File Offset: 0x0001134F
		internal void WriteInternalFlag(InternalFlags reqFlag, bool set)
		{
			if (set)
			{
				this._flags |= reqFlag;
				return;
			}
			this._flags &= ~reqFlag;
		}

		// Token: 0x0600061B RID: 1563 RVA: 0x00013172 File Offset: 0x00011372
		internal void WriteInternalFlag2(InternalFlags2 reqFlag, bool set)
		{
			if (set)
			{
				this._flags2 |= reqFlag;
				return;
			}
			this._flags2 &= ~reqFlag;
		}

		// Token: 0x17000118 RID: 280
		// (get) Token: 0x0600061C RID: 1564 RVA: 0x00013195 File Offset: 0x00011395
		private static DependencyObjectType ControlDType
		{
			get
			{
				if (FrameworkElement._controlDType == null)
				{
					FrameworkElement._controlDType = DependencyObjectType.FromSystemTypeInternal(typeof(Control));
				}
				return FrameworkElement._controlDType;
			}
		}

		// Token: 0x17000119 RID: 281
		// (get) Token: 0x0600061D RID: 1565 RVA: 0x000131B7 File Offset: 0x000113B7
		private static DependencyObjectType ContentPresenterDType
		{
			get
			{
				if (FrameworkElement._contentPresenterDType == null)
				{
					FrameworkElement._contentPresenterDType = DependencyObjectType.FromSystemTypeInternal(typeof(ContentPresenter));
				}
				return FrameworkElement._contentPresenterDType;
			}
		}

		// Token: 0x1700011A RID: 282
		// (get) Token: 0x0600061E RID: 1566 RVA: 0x000131D9 File Offset: 0x000113D9
		private static DependencyObjectType PageDType
		{
			get
			{
				if (FrameworkElement._pageDType == null)
				{
					FrameworkElement._pageDType = DependencyObjectType.FromSystemTypeInternal(typeof(Page));
				}
				return FrameworkElement._pageDType;
			}
		}

		// Token: 0x1700011B RID: 283
		// (get) Token: 0x0600061F RID: 1567 RVA: 0x000131FB File Offset: 0x000113FB
		private static DependencyObjectType PageFunctionBaseDType
		{
			get
			{
				if (FrameworkElement._pageFunctionBaseDType == null)
				{
					FrameworkElement._pageFunctionBaseDType = DependencyObjectType.FromSystemTypeInternal(typeof(PageFunctionBase));
				}
				return FrameworkElement._pageFunctionBaseDType;
			}
		}

		// Token: 0x1700011C RID: 284
		// (get) Token: 0x06000620 RID: 1568 RVA: 0x0001321D File Offset: 0x0001141D
		internal override int EffectiveValuesInitialSize
		{
			get
			{
				return 7;
			}
		}

		/// <summary>Gets the logical parent  element of this element. </summary>
		/// <returns>This element's logical parent.</returns>
		// Token: 0x1700011D RID: 285
		// (get) Token: 0x06000621 RID: 1569 RVA: 0x00013220 File Offset: 0x00011420
		public DependencyObject Parent
		{
			get
			{
				return this.ContextVerifiedGetParent();
			}
		}

		/// <summary>Provides an accessor that simplifies access to the <see cref="T:System.Windows.NameScope" /> registration method.</summary>
		/// <param name="name">Name to use for the specified name-object mapping.</param>
		/// <param name="scopedElement">Object for the mapping.</param>
		// Token: 0x06000622 RID: 1570 RVA: 0x00013228 File Offset: 0x00011428
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
		// Token: 0x06000623 RID: 1571 RVA: 0x0001326C File Offset: 0x0001146C
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

		/// <summary>Finds an element that has the provided identifier name. </summary>
		/// <param name="name">The name of the requested element.</param>
		/// <returns>The requested element. This can be <see langword="null" /> if no matching element was found.</returns>
		// Token: 0x06000624 RID: 1572 RVA: 0x000132AC File Offset: 0x000114AC
		public object FindName(string name)
		{
			DependencyObject dependencyObject;
			return this.FindName(name, out dependencyObject);
		}

		// Token: 0x06000625 RID: 1573 RVA: 0x000132C4 File Offset: 0x000114C4
		internal object FindName(string name, out DependencyObject scopeOwner)
		{
			INameScope nameScope = FrameworkElement.FindScope(this, out scopeOwner);
			if (nameScope != null)
			{
				return nameScope.FindName(name);
			}
			return null;
		}

		/// <summary>Reapplies the default style to the current <see cref="T:System.Windows.FrameworkElement" />.</summary>
		// Token: 0x06000626 RID: 1574 RVA: 0x000132E5 File Offset: 0x000114E5
		public void UpdateDefaultStyle()
		{
			TreeWalkHelper.InvalidateOnResourcesChange(this, null, ResourcesChangeInfo.ThemeChangeInfo);
		}

		/// <summary> Gets an enumerator for logical child elements of this element. </summary>
		/// <returns>An enumerator for logical child elements of this element.</returns>
		// Token: 0x1700011E RID: 286
		// (get) Token: 0x06000627 RID: 1575 RVA: 0x0000C238 File Offset: 0x0000A438
		protected internal virtual IEnumerator LogicalChildren
		{
			get
			{
				return null;
			}
		}

		// Token: 0x06000628 RID: 1576 RVA: 0x000132F4 File Offset: 0x000114F4
		internal object FindResourceOnSelf(object resourceKey, bool allowDeferredResourceReference, bool mustReturnDeferredResourceReference)
		{
			ResourceDictionary value = FrameworkElement.ResourcesField.GetValue(this);
			if (value != null && value.Contains(resourceKey))
			{
				bool flag;
				return value.FetchResource(resourceKey, allowDeferredResourceReference, mustReturnDeferredResourceReference, out flag);
			}
			return DependencyProperty.UnsetValue;
		}

		// Token: 0x06000629 RID: 1577 RVA: 0x000105CB File Offset: 0x0000E7CB
		internal DependencyObject ContextVerifiedGetParent()
		{
			return this._parent;
		}

		/// <summary>Adds the provided object to the logical tree of this element. </summary>
		/// <param name="child">Child element to be added.</param>
		// Token: 0x0600062A RID: 1578 RVA: 0x0001332C File Offset: 0x0001152C
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

		/// <summary>Removes the provided object from this element's logical tree. <see cref="T:System.Windows.FrameworkElement" /> updates the affected logical tree parent pointers to keep in sync with this deletion.</summary>
		/// <param name="child">The element to remove.</param>
		// Token: 0x0600062B RID: 1579 RVA: 0x00013390 File Offset: 0x00011590
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

		// Token: 0x0600062C RID: 1580 RVA: 0x000133F8 File Offset: 0x000115F8
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
			DependencyObject parent2 = (newParent != null) ? newParent : parent;
			TreeWalkHelper.InvalidateOnTreeChange(this, null, parent2, newParent != null);
			this.TryFireInitialized();
		}

		// Token: 0x0600062D RID: 1581 RVA: 0x0001349C File Offset: 0x0001169C
		internal virtual void OnNewParent(DependencyObject newParent)
		{
			DependencyObject parent = this._parent;
			this._parent = newParent;
			if (this._parent != null && this._parent is ContentElement)
			{
				UIElement.SynchronizeForceInheritProperties(this, null, null, this._parent);
			}
			else if (parent is ContentElement)
			{
				UIElement.SynchronizeForceInheritProperties(this, null, null, parent);
			}
			base.SynchronizeReverseInheritPropertyFlags(parent, false);
		}

		// Token: 0x0600062E RID: 1582 RVA: 0x000134F8 File Offset: 0x000116F8
		[SecurityCritical]
		[SecurityTreatAsSafe]
		internal void OnAncestorChangedInternal(TreeChangeInfo parentTreeState)
		{
			bool isSelfInheritanceParent = base.IsSelfInheritanceParent;
			if (parentTreeState.Root != this)
			{
				this.HasStyleChanged = false;
				this.HasStyleInvalidated = false;
				this.HasTemplateChanged = false;
			}
			if (parentTreeState.IsAddOperation)
			{
				FrameworkObject frameworkObject = new FrameworkObject(this, null);
				frameworkObject.SetShouldLookupImplicitStyles();
			}
			if (this.HasResourceReference)
			{
				TreeWalkHelper.OnResourcesChanged(this, ResourcesChangeInfo.TreeChangeInfo, false);
			}
			FrugalObjectList<DependencyProperty> item = this.InvalidateTreeDependentProperties(parentTreeState, base.IsSelfInheritanceParent, isSelfInheritanceParent);
			parentTreeState.InheritablePropertiesStack.Push(item);
			this.OnAncestorChanged();
			if (this.PotentiallyHasMentees)
			{
				this.RaiseClrEvent(FrameworkElement.ResourcesChangedKey, EventArgs.Empty);
			}
		}

		// Token: 0x0600062F RID: 1583 RVA: 0x00013594 File Offset: 0x00011794
		internal FrugalObjectList<DependencyProperty> InvalidateTreeDependentProperties(TreeChangeInfo parentTreeState, bool isSelfInheritanceParent, bool wasSelfInheritanceParent)
		{
			this.AncestorChangeInProgress = true;
			this.InVisibilityCollapsedTree = false;
			if (parentTreeState.TopmostCollapsedParentNode == null)
			{
				if (base.Visibility == Visibility.Collapsed)
				{
					parentTreeState.TopmostCollapsedParentNode = this;
					this.InVisibilityCollapsedTree = true;
				}
			}
			else
			{
				this.InVisibilityCollapsedTree = true;
			}
			FrugalObjectList<DependencyProperty> result;
			try
			{
				if (this.IsInitialized && !this.HasLocalStyle && this != parentTreeState.Root)
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
				result = TreeWalkHelper.InvalidateTreeDependentProperties(parentTreeState, this, null, style, themeStyle, ref childRecord, isChildRecordValid, hasStyleChanged, isSelfInheritanceParent, wasSelfInheritanceParent);
			}
			finally
			{
				this.AncestorChangeInProgress = false;
				this.InVisibilityCollapsedTree = false;
			}
			return result;
		}

		// Token: 0x1700011F RID: 287
		// (get) Token: 0x06000630 RID: 1584 RVA: 0x000136D8 File Offset: 0x000118D8
		internal bool ThisHasLoadedChangeEventHandler
		{
			get
			{
				return (base.EventHandlersStore != null && (base.EventHandlersStore.Contains(FrameworkElement.LoadedEvent) || base.EventHandlersStore.Contains(FrameworkElement.UnloadedEvent))) || (this.Style != null && this.Style.HasLoadedChangeHandler) || (this.ThemeStyle != null && this.ThemeStyle.HasLoadedChangeHandler) || (this.TemplateInternal != null && this.TemplateInternal.HasLoadedChangeHandler) || this.HasFefLoadedChangeHandler;
			}
		}

		// Token: 0x17000120 RID: 288
		// (get) Token: 0x06000631 RID: 1585 RVA: 0x00013764 File Offset: 0x00011964
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

		// Token: 0x06000632 RID: 1586 RVA: 0x000137A4 File Offset: 0x000119A4
		internal void UpdateStyleProperty()
		{
			if (!this.HasStyleInvalidated)
			{
				if (!this.IsStyleUpdateInProgress)
				{
					this.IsStyleUpdateInProgress = true;
					try
					{
						base.InvalidateProperty(FrameworkElement.StyleProperty);
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

		// Token: 0x06000633 RID: 1587 RVA: 0x00013810 File Offset: 0x00011A10
		internal void UpdateThemeStyleProperty()
		{
			if (!this.IsThemeStyleUpdateInProgress)
			{
				this.IsThemeStyleUpdateInProgress = true;
				try
				{
					StyleHelper.GetThemeStyle(this, null);
					ContextMenu contextMenu = base.GetValueEntry(base.LookupEntry(FrameworkElement.ContextMenuProperty.GlobalIndex), FrameworkElement.ContextMenuProperty, null, RequestFlags.DeferredReferences).Value as ContextMenu;
					if (contextMenu != null)
					{
						TreeWalkHelper.InvalidateOnResourcesChange(contextMenu, null, ResourcesChangeInfo.ThemeChangeInfo);
					}
					DependencyObject dependencyObject = base.GetValueEntry(base.LookupEntry(FrameworkElement.ToolTipProperty.GlobalIndex), FrameworkElement.ToolTipProperty, null, RequestFlags.DeferredReferences).Value as DependencyObject;
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

		// Token: 0x06000634 RID: 1588 RVA: 0x00002137 File Offset: 0x00000337
		internal virtual void OnThemeChanged()
		{
		}

		// Token: 0x06000635 RID: 1589 RVA: 0x00013904 File Offset: 0x00011B04
		internal void FireLoadedOnDescendentsInternal()
		{
			if (this.LoadedPending == null)
			{
				DependencyObject parent = this.Parent;
				if (parent == null)
				{
					parent = VisualTreeHelper.GetParent(this);
				}
				object[] unloadedPending = this.UnloadedPending;
				if (unloadedPending == null || unloadedPending[2] != parent)
				{
					BroadcastEventHelper.AddLoadedCallback(this, parent);
					return;
				}
				BroadcastEventHelper.RemoveUnloadedCallback(this, unloadedPending);
			}
		}

		// Token: 0x06000636 RID: 1590 RVA: 0x0001394C File Offset: 0x00011B4C
		internal void FireUnloadedOnDescendentsInternal()
		{
			if (this.UnloadedPending == null)
			{
				DependencyObject parent = this.Parent;
				if (parent == null)
				{
					parent = VisualTreeHelper.GetParent(this);
				}
				object[] loadedPending = this.LoadedPending;
				if (loadedPending == null)
				{
					BroadcastEventHelper.AddUnloadedCallback(this, parent);
					return;
				}
				BroadcastEventHelper.RemoveLoadedCallback(this, loadedPending);
			}
		}

		// Token: 0x06000637 RID: 1591 RVA: 0x0001398C File Offset: 0x00011B8C
		internal override bool ShouldProvideInheritanceContext(DependencyObject target, DependencyProperty property)
		{
			FrameworkObject frameworkObject = new FrameworkObject(target);
			return !frameworkObject.IsValid;
		}

		// Token: 0x17000121 RID: 289
		// (get) Token: 0x06000638 RID: 1592 RVA: 0x000139AB File Offset: 0x00011BAB
		internal override DependencyObject InheritanceContext
		{
			get
			{
				return FrameworkElement.InheritanceContextField.GetValue(this);
			}
		}

		// Token: 0x06000639 RID: 1593 RVA: 0x000139B8 File Offset: 0x00011BB8
		internal override void AddInheritanceContext(DependencyObject context, DependencyProperty property)
		{
			base.AddInheritanceContext(context, property);
			this.TryFireInitialized();
			if ((property == VisualBrush.VisualProperty || property == BitmapCacheBrush.TargetProperty) && FrameworkElement.GetFrameworkParent(this) == null && !FrameworkObject.IsEffectiveAncestor(this, context))
			{
				if (!this.HasMultipleInheritanceContexts && this.InheritanceContext == null)
				{
					FrameworkElement.InheritanceContextField.SetValue(this, context);
					base.OnInheritanceContextChanged(EventArgs.Empty);
					return;
				}
				if (this.InheritanceContext != null)
				{
					FrameworkElement.InheritanceContextField.ClearValue(this);
					this.WriteInternalFlag2(InternalFlags2.HasMultipleInheritanceContexts, true);
					base.OnInheritanceContextChanged(EventArgs.Empty);
				}
			}
		}

		// Token: 0x0600063A RID: 1594 RVA: 0x00013A46 File Offset: 0x00011C46
		internal override void RemoveInheritanceContext(DependencyObject context, DependencyProperty property)
		{
			if (this.InheritanceContext == context)
			{
				FrameworkElement.InheritanceContextField.ClearValue(this);
				base.OnInheritanceContextChanged(EventArgs.Empty);
			}
			base.RemoveInheritanceContext(context, property);
		}

		// Token: 0x0600063B RID: 1595 RVA: 0x00013A6F File Offset: 0x00011C6F
		private void ClearInheritanceContext()
		{
			if (this.InheritanceContext != null)
			{
				FrameworkElement.InheritanceContextField.ClearValue(this);
				base.OnInheritanceContextChanged(EventArgs.Empty);
			}
		}

		// Token: 0x0600063C RID: 1596 RVA: 0x00013A90 File Offset: 0x00011C90
		internal override void OnInheritanceContextChangedCore(EventArgs args)
		{
			DependencyObject value = FrameworkElement.MentorField.GetValue(this);
			DependencyObject dependencyObject = Helper.FindMentor(this.InheritanceContext);
			if (value != dependencyObject)
			{
				FrameworkElement.MentorField.SetValue(this, dependencyObject);
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

		// Token: 0x0600063D RID: 1597 RVA: 0x00013ADC File Offset: 0x00011CDC
		private void ConnectMentor(DependencyObject mentor)
		{
			FrameworkObject frameworkObject = new FrameworkObject(mentor);
			frameworkObject.InheritedPropertyChanged += this.OnMentorInheritedPropertyChanged;
			frameworkObject.ResourcesChanged += this.OnMentorResourcesChanged;
			TreeWalkHelper.InvalidateOnTreeChange(this, null, frameworkObject.DO, true);
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

		// Token: 0x0600063E RID: 1598 RVA: 0x00013B48 File Offset: 0x00011D48
		private void DisconnectMentor(DependencyObject mentor)
		{
			FrameworkObject frameworkObject = new FrameworkObject(mentor);
			frameworkObject.InheritedPropertyChanged -= this.OnMentorInheritedPropertyChanged;
			frameworkObject.ResourcesChanged -= this.OnMentorResourcesChanged;
			TreeWalkHelper.InvalidateOnTreeChange(this, null, frameworkObject.DO, false);
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

		// Token: 0x0600063F RID: 1599 RVA: 0x00013BBC File Offset: 0x00011DBC
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

		// Token: 0x06000640 RID: 1600 RVA: 0x00013BF4 File Offset: 0x00011DF4
		private void OnMentorLoaded(object sender, RoutedEventArgs e)
		{
			FrameworkObject frameworkObject = new FrameworkObject((DependencyObject)sender);
			frameworkObject.Loaded -= this.OnMentorLoaded;
			frameworkObject.Unloaded += this.OnMentorUnloaded;
			BroadcastEventHelper.BroadcastLoadedSynchronously(this, this.IsLoaded);
		}

		// Token: 0x06000641 RID: 1601 RVA: 0x00013C40 File Offset: 0x00011E40
		private void OnMentorUnloaded(object sender, RoutedEventArgs e)
		{
			FrameworkObject frameworkObject = new FrameworkObject((DependencyObject)sender);
			frameworkObject.Unloaded -= this.OnMentorUnloaded;
			frameworkObject.Loaded += this.OnMentorLoaded;
			BroadcastEventHelper.BroadcastUnloadedSynchronously(this, this.IsLoaded);
		}

		// Token: 0x06000642 RID: 1602 RVA: 0x00013C8C File Offset: 0x00011E8C
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

		// Token: 0x06000643 RID: 1603 RVA: 0x00013CBE File Offset: 0x00011EBE
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

		// Token: 0x06000644 RID: 1604 RVA: 0x00013CF0 File Offset: 0x00011EF0
		private void OnMentorInheritedPropertyChanged(object sender, InheritedPropertyChangedEventArgs e)
		{
			TreeWalkHelper.InvalidateOnInheritablePropertyChange(this, null, e.Info, false);
		}

		// Token: 0x06000645 RID: 1605 RVA: 0x00013D00 File Offset: 0x00011F00
		private void OnMentorResourcesChanged(object sender, EventArgs e)
		{
			TreeWalkHelper.InvalidateOnResourcesChange(this, null, ResourcesChangeInfo.CatastrophicDictionaryChangeInfo);
		}

		// Token: 0x06000646 RID: 1606 RVA: 0x00013D10 File Offset: 0x00011F10
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

		// Token: 0x17000122 RID: 290
		// (get) Token: 0x06000647 RID: 1607 RVA: 0x00013D4A File Offset: 0x00011F4A
		// (set) Token: 0x06000648 RID: 1608 RVA: 0x00013D54 File Offset: 0x00011F54
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

		// Token: 0x17000123 RID: 291
		// (get) Token: 0x06000649 RID: 1609 RVA: 0x00013D5F File Offset: 0x00011F5F
		// (set) Token: 0x0600064A RID: 1610 RVA: 0x00013D6C File Offset: 0x00011F6C
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

		// Token: 0x17000124 RID: 292
		// (get) Token: 0x0600064B RID: 1611 RVA: 0x00013D7A File Offset: 0x00011F7A
		// (set) Token: 0x0600064C RID: 1612 RVA: 0x00013D87 File Offset: 0x00011F87
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

		// Token: 0x17000125 RID: 293
		// (get) Token: 0x0600064D RID: 1613 RVA: 0x00013D95 File Offset: 0x00011F95
		// (set) Token: 0x0600064E RID: 1614 RVA: 0x00013D9E File Offset: 0x00011F9E
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

		// Token: 0x17000126 RID: 294
		// (get) Token: 0x0600064F RID: 1615 RVA: 0x00013DA8 File Offset: 0x00011FA8
		// (set) Token: 0x06000650 RID: 1616 RVA: 0x00013DB5 File Offset: 0x00011FB5
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

		// Token: 0x17000127 RID: 295
		// (get) Token: 0x06000651 RID: 1617 RVA: 0x00013DC3 File Offset: 0x00011FC3
		// (set) Token: 0x06000652 RID: 1618 RVA: 0x00013DCC File Offset: 0x00011FCC
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

		// Token: 0x17000128 RID: 296
		// (get) Token: 0x06000653 RID: 1619 RVA: 0x00013DD6 File Offset: 0x00011FD6
		// (set) Token: 0x06000654 RID: 1620 RVA: 0x00013DE3 File Offset: 0x00011FE3
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

		// Token: 0x17000129 RID: 297
		// (get) Token: 0x06000655 RID: 1621 RVA: 0x00013DF1 File Offset: 0x00011FF1
		// (set) Token: 0x06000656 RID: 1622 RVA: 0x00013DFE File Offset: 0x00011FFE
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

		// Token: 0x1700012A RID: 298
		// (get) Token: 0x06000657 RID: 1623 RVA: 0x00013E0C File Offset: 0x0001200C
		// (set) Token: 0x06000658 RID: 1624 RVA: 0x00013E19 File Offset: 0x00012019
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

		// Token: 0x1700012B RID: 299
		// (get) Token: 0x06000659 RID: 1625 RVA: 0x00013E27 File Offset: 0x00012027
		// (set) Token: 0x0600065A RID: 1626 RVA: 0x00013E34 File Offset: 0x00012034
		internal bool HasTemplateChanged
		{
			get
			{
				return this.ReadInternalFlag2(InternalFlags2.HasTemplateChanged);
			}
			set
			{
				this.WriteInternalFlag2(InternalFlags2.HasTemplateChanged, value);
			}
		}

		// Token: 0x1700012C RID: 300
		// (get) Token: 0x0600065B RID: 1627 RVA: 0x00013E42 File Offset: 0x00012042
		// (set) Token: 0x0600065C RID: 1628 RVA: 0x00013E4F File Offset: 0x0001204F
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

		// Token: 0x1700012D RID: 301
		// (get) Token: 0x0600065D RID: 1629 RVA: 0x00013E5D File Offset: 0x0001205D
		// (set) Token: 0x0600065E RID: 1630 RVA: 0x00013E6A File Offset: 0x0001206A
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

		// Token: 0x1700012E RID: 302
		// (get) Token: 0x0600065F RID: 1631 RVA: 0x00013E78 File Offset: 0x00012078
		// (set) Token: 0x06000660 RID: 1632 RVA: 0x00013E85 File Offset: 0x00012085
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

		// Token: 0x1700012F RID: 303
		// (get) Token: 0x06000661 RID: 1633 RVA: 0x00013E93 File Offset: 0x00012093
		// (set) Token: 0x06000662 RID: 1634 RVA: 0x00013EA0 File Offset: 0x000120A0
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

		// Token: 0x17000130 RID: 304
		// (get) Token: 0x06000663 RID: 1635 RVA: 0x00013EAE File Offset: 0x000120AE
		// (set) Token: 0x06000664 RID: 1636 RVA: 0x00013EBB File Offset: 0x000120BB
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

		// Token: 0x17000131 RID: 305
		// (get) Token: 0x06000665 RID: 1637 RVA: 0x00013EC9 File Offset: 0x000120C9
		// (set) Token: 0x06000666 RID: 1638 RVA: 0x00013ED1 File Offset: 0x000120D1
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

		// Token: 0x17000132 RID: 306
		// (get) Token: 0x06000667 RID: 1639 RVA: 0x00013EDA File Offset: 0x000120DA
		internal object[] LoadedPending
		{
			get
			{
				return (object[])base.GetValue(FrameworkElement.LoadedPendingProperty);
			}
		}

		// Token: 0x17000133 RID: 307
		// (get) Token: 0x06000668 RID: 1640 RVA: 0x00013EEC File Offset: 0x000120EC
		internal object[] UnloadedPending
		{
			get
			{
				return (object[])base.GetValue(FrameworkElement.UnloadedPendingProperty);
			}
		}

		// Token: 0x17000134 RID: 308
		// (get) Token: 0x06000669 RID: 1641 RVA: 0x00013EFE File Offset: 0x000120FE
		internal override bool HasMultipleInheritanceContexts
		{
			get
			{
				return this.ReadInternalFlag2(InternalFlags2.HasMultipleInheritanceContexts);
			}
		}

		// Token: 0x17000135 RID: 309
		// (get) Token: 0x0600066A RID: 1642 RVA: 0x00013F0B File Offset: 0x0001210B
		// (set) Token: 0x0600066B RID: 1643 RVA: 0x00013F18 File Offset: 0x00012118
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

		// Token: 0x14000027 RID: 39
		// (add) Token: 0x0600066C RID: 1644 RVA: 0x00013F26 File Offset: 0x00012126
		// (remove) Token: 0x0600066D RID: 1645 RVA: 0x00013F3B File Offset: 0x0001213B
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

		// Token: 0x14000028 RID: 40
		// (add) Token: 0x0600066E RID: 1646 RVA: 0x00013F49 File Offset: 0x00012149
		// (remove) Token: 0x0600066F RID: 1647 RVA: 0x00013F5E File Offset: 0x0001215E
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

		// Token: 0x04000656 RID: 1622
		private static readonly Type _typeofThis = typeof(FrameworkElement);

		/// <summary>Identifies the <see cref="P:System.Windows.FrameworkElement.Style" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.FrameworkElement.Style" /> dependency property.</returns>
		// Token: 0x04000657 RID: 1623
		[CommonDependencyProperty]
		public static readonly DependencyProperty StyleProperty = DependencyProperty.Register("Style", typeof(Style), FrameworkElement._typeofThis, new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsMeasure, new PropertyChangedCallback(FrameworkElement.OnStyleChanged)));

		/// <summary>Identifies the <see cref="P:System.Windows.FrameworkElement.OverridesDefaultStyle" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.FrameworkElement.OverridesDefaultStyle" /> dependency property.</returns>
		// Token: 0x04000658 RID: 1624
		public static readonly DependencyProperty OverridesDefaultStyleProperty = DependencyProperty.Register("OverridesDefaultStyle", typeof(bool), FrameworkElement._typeofThis, new FrameworkPropertyMetadata(BooleanBoxes.FalseBox, FrameworkPropertyMetadataOptions.AffectsMeasure, new PropertyChangedCallback(FrameworkElement.OnThemeStyleKeyChanged)));

		/// <summary>Identifies the <see cref="P:System.Windows.FrameworkElement.UseLayoutRounding" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.FrameworkElement.UseLayoutRounding" /> dependency property.</returns>
		// Token: 0x04000659 RID: 1625
		public static readonly DependencyProperty UseLayoutRoundingProperty = DependencyProperty.Register("UseLayoutRounding", typeof(bool), typeof(FrameworkElement), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.Inherits, new PropertyChangedCallback(FrameworkElement.OnUseLayoutRoundingChanged)));

		/// <summary>Identifies the <see cref="P:System.Windows.FrameworkElement.DefaultStyleKey" /> dependency property. </summary>
		/// <returns>The <see cref="P:System.Windows.FrameworkElement.DefaultStyleKey" /> dependency property identifier.</returns>
		// Token: 0x0400065A RID: 1626
		protected internal static readonly DependencyProperty DefaultStyleKeyProperty = DependencyProperty.Register("DefaultStyleKey", typeof(object), FrameworkElement._typeofThis, new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsMeasure, new PropertyChangedCallback(FrameworkElement.OnThemeStyleKeyChanged)));

		// Token: 0x0400065B RID: 1627
		internal static readonly NumberSubstitution DefaultNumberSubstitution = new NumberSubstitution(NumberCultureSource.User, null, NumberSubstitutionMethod.AsCulture);

		/// <summary> Identifies the <see cref="P:System.Windows.FrameworkElement.DataContext" /> dependency property. </summary>
		/// <returns>The <see cref="P:System.Windows.FrameworkElement.DataContext" /> dependency property identifier.</returns>
		// Token: 0x0400065C RID: 1628
		public static readonly DependencyProperty DataContextProperty = DependencyProperty.Register("DataContext", typeof(object), FrameworkElement._typeofThis, new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.Inherits, new PropertyChangedCallback(FrameworkElement.OnDataContextChanged)));

		// Token: 0x0400065D RID: 1629
		internal static readonly EventPrivateKey DataContextChangedKey = new EventPrivateKey();

		/// <summary>Identifies the <see cref="P:System.Windows.FrameworkElement.BindingGroup" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.FrameworkElement.BindingGroup" /> dependency property.</returns>
		// Token: 0x0400065E RID: 1630
		public static readonly DependencyProperty BindingGroupProperty = DependencyProperty.Register("BindingGroup", typeof(BindingGroup), FrameworkElement._typeofThis, new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.Inherits));

		/// <summary>Identifies the <see cref="P:System.Windows.FrameworkElement.Language" /> dependency property. </summary>
		/// <returns>The <see cref="P:System.Windows.FrameworkElement.Language" /> dependency property identifier.</returns>
		// Token: 0x0400065F RID: 1631
		public static readonly DependencyProperty LanguageProperty = DependencyProperty.RegisterAttached("Language", typeof(XmlLanguage), FrameworkElement._typeofThis, new FrameworkPropertyMetadata(XmlLanguage.GetLanguage("en-US"), FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.Inherits));

		/// <summary>Identifies the <see cref="P:System.Windows.FrameworkElement.Name" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.FrameworkElement.Name" /> dependency property.</returns>
		// Token: 0x04000660 RID: 1632
		[CommonDependencyProperty]
		public static readonly DependencyProperty NameProperty = DependencyProperty.Register("Name", typeof(string), FrameworkElement._typeofThis, new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.None, null, null, true), new ValidateValueCallback(NameValidationHelper.NameValidationCallback));

		/// <summary>Identifies the <see cref="P:System.Windows.FrameworkElement.Tag" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.FrameworkElement.Tag" /> dependency property.</returns>
		// Token: 0x04000661 RID: 1633
		public static readonly DependencyProperty TagProperty = DependencyProperty.Register("Tag", typeof(object), FrameworkElement._typeofThis, new FrameworkPropertyMetadata(null));

		/// <summary>Identifies the <see cref="P:System.Windows.FrameworkElement.InputScope" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.FrameworkElement.InputScope" /> dependency property.</returns>
		// Token: 0x04000662 RID: 1634
		public static readonly DependencyProperty InputScopeProperty = InputMethod.InputScopeProperty.AddOwner(FrameworkElement._typeofThis, new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.Inherits));

		// Token: 0x04000665 RID: 1637
		private static PropertyMetadata _actualWidthMetadata;

		// Token: 0x04000666 RID: 1638
		private static readonly DependencyPropertyKey ActualWidthPropertyKey;

		/// <summary>Identifies the <see cref="P:System.Windows.FrameworkElement.ActualWidth" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.FrameworkElement.ActualWidth" /> dependency property.</returns>
		// Token: 0x04000667 RID: 1639
		public static readonly DependencyProperty ActualWidthProperty;

		// Token: 0x04000668 RID: 1640
		private static PropertyMetadata _actualHeightMetadata;

		// Token: 0x04000669 RID: 1641
		private static readonly DependencyPropertyKey ActualHeightPropertyKey;

		/// <summary> Identifies the <see cref="P:System.Windows.FrameworkElement.ActualHeight" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.FrameworkElement.ActualHeight" /> dependency property.</returns>
		// Token: 0x0400066A RID: 1642
		public static readonly DependencyProperty ActualHeightProperty;

		/// <summary>Identifies the <see cref="P:System.Windows.FrameworkElement.LayoutTransform" /> dependency property. </summary>
		/// <returns>The <see cref="P:System.Windows.FrameworkElement.LayoutTransform" /> dependency property identifier.</returns>
		// Token: 0x0400066B RID: 1643
		public static readonly DependencyProperty LayoutTransformProperty;

		/// <summary> Identifies the <see cref="P:System.Windows.FrameworkElement.Width" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.FrameworkElement.Width" /> dependency property.</returns>
		// Token: 0x0400066C RID: 1644
		[CommonDependencyProperty]
		public static readonly DependencyProperty WidthProperty;

		/// <summary>Identifies the <see cref="P:System.Windows.FrameworkElement.MinWidth" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.FrameworkElement.MinWidth" /> dependency property.</returns>
		// Token: 0x0400066D RID: 1645
		[CommonDependencyProperty]
		public static readonly DependencyProperty MinWidthProperty;

		/// <summary> Identifies the <see cref="P:System.Windows.FrameworkElement.MaxWidth" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.FrameworkElement.MaxWidth" /> dependency property.</returns>
		// Token: 0x0400066E RID: 1646
		[CommonDependencyProperty]
		public static readonly DependencyProperty MaxWidthProperty;

		/// <summary>Identifies the <see cref="P:System.Windows.FrameworkElement.Height" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.FrameworkElement.Height" /> dependency property.</returns>
		// Token: 0x0400066F RID: 1647
		[CommonDependencyProperty]
		public static readonly DependencyProperty HeightProperty;

		/// <summary>Identifies the <see cref="P:System.Windows.FrameworkElement.MinHeight" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.FrameworkElement.MinHeight" /> dependency property.</returns>
		// Token: 0x04000670 RID: 1648
		[CommonDependencyProperty]
		public static readonly DependencyProperty MinHeightProperty;

		/// <summary> Identifies the <see cref="P:System.Windows.FrameworkElement.MaxHeight" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.FrameworkElement.MaxHeight" /> dependency property.</returns>
		// Token: 0x04000671 RID: 1649
		[CommonDependencyProperty]
		public static readonly DependencyProperty MaxHeightProperty;

		/// <summary>Identifies the <see cref="P:System.Windows.FrameworkElement.FlowDirection" /> dependency property. </summary>
		/// <returns>The <see cref="P:System.Windows.FrameworkElement.FlowDirection" /> dependency property identifier.</returns>
		// Token: 0x04000672 RID: 1650
		[CommonDependencyProperty]
		public static readonly DependencyProperty FlowDirectionProperty;

		/// <summary> Identifies the <see cref="P:System.Windows.FrameworkElement.Margin" /> dependency property. </summary>
		/// <returns>The <see cref="P:System.Windows.FrameworkElement.Margin" /> dependency property identifier.</returns>
		// Token: 0x04000673 RID: 1651
		[CommonDependencyProperty]
		public static readonly DependencyProperty MarginProperty;

		/// <summary>Identifies the <see cref="P:System.Windows.FrameworkElement.HorizontalAlignment" /> dependency property. </summary>
		/// <returns>The <see cref="P:System.Windows.FrameworkElement.HorizontalAlignment" /> dependency property identifier.</returns>
		// Token: 0x04000674 RID: 1652
		[CommonDependencyProperty]
		public static readonly DependencyProperty HorizontalAlignmentProperty;

		/// <summary> Identifies the <see cref="P:System.Windows.FrameworkElement.VerticalAlignment" /> dependency property. </summary>
		/// <returns>The <see cref="P:System.Windows.FrameworkElement.VerticalAlignment" /> dependency property identifier.</returns>
		// Token: 0x04000675 RID: 1653
		[CommonDependencyProperty]
		public static readonly DependencyProperty VerticalAlignmentProperty;

		// Token: 0x04000676 RID: 1654
		private static Style _defaultFocusVisualStyle;

		/// <summary>Identifies the <see cref="P:System.Windows.FrameworkElement.FocusVisualStyle" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.FrameworkElement.FocusVisualStyle" /> dependency property.</returns>
		// Token: 0x04000677 RID: 1655
		public static readonly DependencyProperty FocusVisualStyleProperty;

		/// <summary>Identifies the <see cref="P:System.Windows.FrameworkElement.Cursor" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.FrameworkElement.Cursor" /> dependency property.</returns>
		// Token: 0x04000678 RID: 1656
		public static readonly DependencyProperty CursorProperty;

		/// <summary>Identifies the <see cref="P:System.Windows.FrameworkElement.ForceCursor" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.FrameworkElement.ForceCursor" /> dependency property.</returns>
		// Token: 0x04000679 RID: 1657
		public static readonly DependencyProperty ForceCursorProperty;

		// Token: 0x0400067A RID: 1658
		internal static readonly EventPrivateKey InitializedKey;

		// Token: 0x0400067B RID: 1659
		internal static readonly DependencyPropertyKey LoadedPendingPropertyKey;

		// Token: 0x0400067C RID: 1660
		internal static readonly DependencyProperty LoadedPendingProperty;

		// Token: 0x0400067D RID: 1661
		internal static readonly DependencyPropertyKey UnloadedPendingPropertyKey;

		// Token: 0x0400067E RID: 1662
		internal static readonly DependencyProperty UnloadedPendingProperty;

		/// <summary>Identifies the <see cref="P:System.Windows.FrameworkElement.ToolTip" /> dependency property. </summary>
		/// <returns>The <see cref="P:System.Windows.FrameworkElement.ToolTip" /> dependency property identifier.</returns>
		// Token: 0x04000681 RID: 1665
		public static readonly DependencyProperty ToolTipProperty;

		/// <summary> Identifies the <see cref="P:System.Windows.FrameworkElement.ContextMenu" /> dependency property. </summary>
		/// <returns>The <see cref="P:System.Windows.FrameworkElement.ContextMenu" /> dependency property identifier.</returns>
		// Token: 0x04000682 RID: 1666
		public static readonly DependencyProperty ContextMenuProperty;

		// Token: 0x04000687 RID: 1671
		private Style _themeStyleCache;

		// Token: 0x04000688 RID: 1672
		private static readonly UncommonField<SizeBox> UnclippedDesiredSizeField;

		// Token: 0x04000689 RID: 1673
		private static readonly UncommonField<FrameworkElement.LayoutTransformData> LayoutTransformDataField;

		// Token: 0x0400068A RID: 1674
		private Style _styleCache;

		// Token: 0x0400068B RID: 1675
		internal static readonly UncommonField<ResourceDictionary> ResourcesField;

		// Token: 0x0400068C RID: 1676
		internal DependencyObject _templatedParent;

		// Token: 0x0400068D RID: 1677
		private UIElement _templateChild;

		// Token: 0x0400068E RID: 1678
		private InternalFlags _flags;

		// Token: 0x0400068F RID: 1679
		private InternalFlags2 _flags2 = InternalFlags2.Default;

		// Token: 0x04000690 RID: 1680
		internal static DependencyObjectType UIElementDType;

		// Token: 0x04000691 RID: 1681
		private static DependencyObjectType _controlDType;

		// Token: 0x04000692 RID: 1682
		private static DependencyObjectType _contentPresenterDType;

		// Token: 0x04000693 RID: 1683
		private static DependencyObjectType _pageFunctionBaseDType;

		// Token: 0x04000694 RID: 1684
		private static DependencyObjectType _pageDType;

		// Token: 0x04000695 RID: 1685
		[ThreadStatic]
		private static FrameworkElement.FrameworkServices _frameworkServices;

		// Token: 0x04000696 RID: 1686
		internal static readonly EventPrivateKey ResourcesChangedKey;

		// Token: 0x04000697 RID: 1687
		internal static readonly EventPrivateKey InheritedPropertyChangedKey;

		// Token: 0x04000698 RID: 1688
		internal new static DependencyObjectType DType;

		// Token: 0x04000699 RID: 1689
		private new DependencyObject _parent;

		// Token: 0x0400069A RID: 1690
		private FrugalObjectList<DependencyProperty> _inheritableProperties;

		// Token: 0x0400069B RID: 1691
		private static readonly UncommonField<DependencyObject> InheritanceContextField;

		// Token: 0x0400069C RID: 1692
		private static readonly UncommonField<DependencyObject> MentorField;

		// Token: 0x02000816 RID: 2070
		private struct MinMax
		{
			// Token: 0x06007E3C RID: 32316 RVA: 0x002356D8 File Offset: 0x002338D8
			internal MinMax(FrameworkElement e)
			{
				this.maxHeight = e.MaxHeight;
				this.minHeight = e.MinHeight;
				double num = e.Height;
				double num2 = DoubleUtil.IsNaN(num) ? double.PositiveInfinity : num;
				this.maxHeight = Math.Max(Math.Min(num2, this.maxHeight), this.minHeight);
				num2 = (DoubleUtil.IsNaN(num) ? 0.0 : num);
				this.minHeight = Math.Max(Math.Min(this.maxHeight, num2), this.minHeight);
				this.maxWidth = e.MaxWidth;
				this.minWidth = e.MinWidth;
				num = e.Width;
				double num3 = DoubleUtil.IsNaN(num) ? double.PositiveInfinity : num;
				this.maxWidth = Math.Max(Math.Min(num3, this.maxWidth), this.minWidth);
				num3 = (DoubleUtil.IsNaN(num) ? 0.0 : num);
				this.minWidth = Math.Max(Math.Min(this.maxWidth, num3), this.minWidth);
			}

			// Token: 0x04003BB1 RID: 15281
			internal double minWidth;

			// Token: 0x04003BB2 RID: 15282
			internal double maxWidth;

			// Token: 0x04003BB3 RID: 15283
			internal double minHeight;

			// Token: 0x04003BB4 RID: 15284
			internal double maxHeight;
		}

		// Token: 0x02000817 RID: 2071
		private class LayoutTransformData
		{
			// Token: 0x06007E3D RID: 32317 RVA: 0x002357EB File Offset: 0x002339EB
			internal void CreateTransformSnapshot(Transform sourceTransform)
			{
				this._transform = new MatrixTransform(sourceTransform.Value);
				this._transform.Freeze();
			}

			// Token: 0x17001D4B RID: 7499
			// (get) Token: 0x06007E3E RID: 32318 RVA: 0x00235809 File Offset: 0x00233A09
			internal Transform Transform
			{
				get
				{
					return this._transform;
				}
			}

			// Token: 0x04003BB5 RID: 15285
			internal Size UntransformedDS;

			// Token: 0x04003BB6 RID: 15286
			internal Size TransformedUnroundedDS;

			// Token: 0x04003BB7 RID: 15287
			private Transform _transform;
		}

		// Token: 0x02000818 RID: 2072
		private class FrameworkServices
		{
			// Token: 0x06007E40 RID: 32320 RVA: 0x00235811 File Offset: 0x00233A11
			internal FrameworkServices()
			{
				this._keyboardNavigation = new KeyboardNavigation();
				this._popupControlService = new PopupControlService();
				if (!AccessibilitySwitches.UseLegacyToolTipDisplay)
				{
					this._keyboardNavigation.FocusChanged += this._popupControlService.FocusChangedEventHandler;
				}
			}

			// Token: 0x04003BB8 RID: 15288
			internal KeyboardNavigation _keyboardNavigation;

			// Token: 0x04003BB9 RID: 15289
			internal PopupControlService _popupControlService;
		}
	}
}
