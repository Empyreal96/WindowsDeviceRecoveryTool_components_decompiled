using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Threading;
using System.Windows.Baml2006;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media.Media3D;
using MS.Internal;
using MS.Utility;

namespace System.Windows
{
	/// <summary>Supports the creation of templates.</summary>
	// Token: 0x020000C7 RID: 199
	[Localizability(LocalizationCategory.NeverLocalize)]
	public class FrameworkElementFactory
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.FrameworkElementFactory" /> class.</summary>
		// Token: 0x06000672 RID: 1650 RVA: 0x00013FAF File Offset: 0x000121AF
		public FrameworkElementFactory() : this(null, null)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.FrameworkElementFactory" /> class with the specified <see cref="T:System.Type" />.</summary>
		/// <param name="type">The type of instance to create.</param>
		// Token: 0x06000673 RID: 1651 RVA: 0x00013FB9 File Offset: 0x000121B9
		public FrameworkElementFactory(Type type) : this(type, null)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.FrameworkElementFactory" /> class with the specified text to produce.</summary>
		/// <param name="text">The text string to produce.</param>
		// Token: 0x06000674 RID: 1652 RVA: 0x00013FC3 File Offset: 0x000121C3
		public FrameworkElementFactory(string text) : this(null, null)
		{
			this.Text = text;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.FrameworkElementFactory" /> class with the specified <see cref="T:System.Type" /> and name.</summary>
		/// <param name="type">The type of instance to create.</param>
		/// <param name="name">The style identifier.</param>
		// Token: 0x06000675 RID: 1653 RVA: 0x00013FD4 File Offset: 0x000121D4
		public FrameworkElementFactory(Type type, string name)
		{
			this.Type = type;
			this.Name = name;
		}

		/// <summary>Gets or sets the type of the objects this factory produces.</summary>
		/// <returns>The type of the objects this factory produces.</returns>
		// Token: 0x17000136 RID: 310
		// (get) Token: 0x06000676 RID: 1654 RVA: 0x00013FFC File Offset: 0x000121FC
		// (set) Token: 0x06000677 RID: 1655 RVA: 0x00014004 File Offset: 0x00012204
		public Type Type
		{
			get
			{
				return this._type;
			}
			set
			{
				if (this._sealed)
				{
					throw new InvalidOperationException(SR.Get("CannotChangeAfterSealed", new object[]
					{
						"FrameworkElementFactory"
					}));
				}
				if (this._text != null)
				{
					throw new InvalidOperationException(SR.Get("FrameworkElementFactoryCannotAddText"));
				}
				if (value != null && !typeof(FrameworkElement).IsAssignableFrom(value) && !typeof(FrameworkContentElement).IsAssignableFrom(value) && !typeof(Visual3D).IsAssignableFrom(value))
				{
					throw new ArgumentException(SR.Get("MustBeFrameworkOr3DDerived", new object[]
					{
						value.Name
					}));
				}
				this._type = value;
				WpfKnownType wpfKnownType = null;
				if (this._type != null)
				{
					wpfKnownType = (XamlReader.BamlSharedSchemaContext.GetKnownXamlType(this._type) as WpfKnownType);
				}
				this._knownTypeFactory = ((wpfKnownType != null) ? wpfKnownType.DefaultConstructor : null);
			}
		}

		/// <summary>Gets or sets the text string to produce.</summary>
		/// <returns>The text string to produce.</returns>
		// Token: 0x17000137 RID: 311
		// (get) Token: 0x06000678 RID: 1656 RVA: 0x000140F2 File Offset: 0x000122F2
		// (set) Token: 0x06000679 RID: 1657 RVA: 0x000140FC File Offset: 0x000122FC
		public string Text
		{
			get
			{
				return this._text;
			}
			set
			{
				if (this._sealed)
				{
					throw new InvalidOperationException(SR.Get("CannotChangeAfterSealed", new object[]
					{
						"FrameworkElementFactory"
					}));
				}
				if (this._firstChild != null)
				{
					throw new InvalidOperationException(SR.Get("FrameworkElementFactoryCannotAddText"));
				}
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				this._text = value;
			}
		}

		/// <summary>Gets or sets the name of a template item.</summary>
		/// <returns>A string that is the template identifier.</returns>
		// Token: 0x17000138 RID: 312
		// (get) Token: 0x0600067A RID: 1658 RVA: 0x0001415C File Offset: 0x0001235C
		// (set) Token: 0x0600067B RID: 1659 RVA: 0x00014164 File Offset: 0x00012364
		public string Name
		{
			get
			{
				return this._childName;
			}
			set
			{
				if (this._sealed)
				{
					throw new InvalidOperationException(SR.Get("CannotChangeAfterSealed", new object[]
					{
						"FrameworkElementFactory"
					}));
				}
				if (value == string.Empty)
				{
					throw new ArgumentException(SR.Get("NameNotEmptyString"));
				}
				this._childName = value;
			}
		}

		/// <summary>Adds a child factory to this factory.</summary>
		/// <param name="child">The <see cref="T:System.Windows.FrameworkElementFactory" /> object to add as a child.</param>
		// Token: 0x0600067C RID: 1660 RVA: 0x000141BC File Offset: 0x000123BC
		public void AppendChild(FrameworkElementFactory child)
		{
			if (this._sealed)
			{
				throw new InvalidOperationException(SR.Get("CannotChangeAfterSealed", new object[]
				{
					"FrameworkElementFactory"
				}));
			}
			if (child == null)
			{
				throw new ArgumentNullException("child");
			}
			if (child._parent != null)
			{
				throw new ArgumentException(SR.Get("FrameworkElementFactoryAlreadyParented"));
			}
			if (this._text != null)
			{
				throw new InvalidOperationException(SR.Get("FrameworkElementFactoryCannotAddText"));
			}
			if (this._firstChild == null)
			{
				this._firstChild = child;
				this._lastChild = child;
			}
			else
			{
				this._lastChild._nextSibling = child;
				this._lastChild = child;
			}
			child._parent = this;
		}

		/// <summary>Sets the value of a dependency property.</summary>
		/// <param name="dp">The dependency property identifier of the property to set.</param>
		/// <param name="value">The new value.</param>
		// Token: 0x0600067D RID: 1661 RVA: 0x00014260 File Offset: 0x00012460
		public void SetValue(DependencyProperty dp, object value)
		{
			if (this._sealed)
			{
				throw new InvalidOperationException(SR.Get("CannotChangeAfterSealed", new object[]
				{
					"FrameworkElementFactory"
				}));
			}
			if (dp == null)
			{
				throw new ArgumentNullException("dp");
			}
			if (!dp.IsValidValue(value) && !(value is MarkupExtension) && !(value is DeferredReference))
			{
				throw new ArgumentException(SR.Get("InvalidPropertyValue", new object[]
				{
					value,
					dp.Name
				}));
			}
			if (StyleHelper.IsStylingLogicalTree(dp, value))
			{
				throw new NotSupportedException(SR.Get("ModifyingLogicalTreeViaStylesNotImplemented", new object[]
				{
					value,
					"FrameworkElementFactory.SetValue"
				}));
			}
			if (dp.ReadOnly)
			{
				throw new ArgumentException(SR.Get("ReadOnlyPropertyNotAllowed", new object[]
				{
					dp.Name,
					base.GetType().Name
				}));
			}
			ResourceReferenceExpression resourceReferenceExpression = value as ResourceReferenceExpression;
			DynamicResourceExtension dynamicResourceExtension = value as DynamicResourceExtension;
			object obj = null;
			if (resourceReferenceExpression != null)
			{
				obj = resourceReferenceExpression.ResourceKey;
			}
			else if (dynamicResourceExtension != null)
			{
				obj = dynamicResourceExtension.ResourceKey;
			}
			if (obj != null)
			{
				this.UpdatePropertyValueList(dp, PropertyValueType.Resource, obj);
				return;
			}
			TemplateBindingExtension templateBindingExtension = value as TemplateBindingExtension;
			if (templateBindingExtension == null)
			{
				this.UpdatePropertyValueList(dp, PropertyValueType.Set, value);
				return;
			}
			this.UpdatePropertyValueList(dp, PropertyValueType.TemplateBinding, templateBindingExtension);
		}

		/// <summary>Sets up data binding on a property.</summary>
		/// <param name="dp">Identifies the property where the binding should be established.</param>
		/// <param name="binding">Description of the binding.</param>
		// Token: 0x0600067E RID: 1662 RVA: 0x0001438D File Offset: 0x0001258D
		public void SetBinding(DependencyProperty dp, BindingBase binding)
		{
			this.SetValue(dp, binding);
		}

		/// <summary>Set up a dynamic resource reference on a child property.</summary>
		/// <param name="dp">The property to which the resource is bound.</param>
		/// <param name="name">The name of the resource.</param>
		// Token: 0x0600067F RID: 1663 RVA: 0x00014397 File Offset: 0x00012597
		public void SetResourceReference(DependencyProperty dp, object name)
		{
			if (this._sealed)
			{
				throw new InvalidOperationException(SR.Get("CannotChangeAfterSealed", new object[]
				{
					"FrameworkElementFactory"
				}));
			}
			if (dp == null)
			{
				throw new ArgumentNullException("dp");
			}
			this.UpdatePropertyValueList(dp, PropertyValueType.Resource, name);
		}

		/// <summary>Adds an event handler for the given routed event to the instances created by this factory.</summary>
		/// <param name="routedEvent">Identifier object for the routed event being handled.</param>
		/// <param name="handler">A reference to the handler implementation.</param>
		// Token: 0x06000680 RID: 1664 RVA: 0x000143D6 File Offset: 0x000125D6
		public void AddHandler(RoutedEvent routedEvent, Delegate handler)
		{
			this.AddHandler(routedEvent, handler, false);
		}

		/// <summary>Adds an event handler for the given routed event to the instances created by this factory, with the option of having the provided handler be invoked even in cases of routed events that had already been marked as handled by another element along the route.</summary>
		/// <param name="routedEvent">Identifier object for the routed event being handled.</param>
		/// <param name="handler">A reference to the handler implementation.</param>
		/// <param name="handledEventsToo">Whether to invoke the handler in cases where the routed event has already been marked as handled in its arguments object. <see langword="true" /> to invoke the handler even when the routed event is marked handled; otherwise, <see langword="false" />. The default is <see langword="false" />. Asking to handle already-handled routed events is not common.</param>
		// Token: 0x06000681 RID: 1665 RVA: 0x000143E4 File Offset: 0x000125E4
		public void AddHandler(RoutedEvent routedEvent, Delegate handler, bool handledEventsToo)
		{
			if (this._sealed)
			{
				throw new InvalidOperationException(SR.Get("CannotChangeAfterSealed", new object[]
				{
					"FrameworkElementFactory"
				}));
			}
			if (routedEvent == null)
			{
				throw new ArgumentNullException("routedEvent");
			}
			if (handler == null)
			{
				throw new ArgumentNullException("handler");
			}
			if (handler.GetType() != routedEvent.HandlerType)
			{
				throw new ArgumentException(SR.Get("HandlerTypeIllegal"));
			}
			if (this._eventHandlersStore == null)
			{
				this._eventHandlersStore = new EventHandlersStore();
			}
			this._eventHandlersStore.AddRoutedEventHandler(routedEvent, handler, handledEventsToo);
			if (routedEvent == FrameworkElement.LoadedEvent || routedEvent == FrameworkElement.UnloadedEvent)
			{
				this.HasLoadedChangeHandler = true;
			}
		}

		/// <summary>Removes an event handler from the given routed event. This applies to the instances created by this factory.</summary>
		/// <param name="routedEvent">Identifier object for the routed event.</param>
		/// <param name="handler">The handler to remove.</param>
		// Token: 0x06000682 RID: 1666 RVA: 0x00014490 File Offset: 0x00012690
		public void RemoveHandler(RoutedEvent routedEvent, Delegate handler)
		{
			if (this._sealed)
			{
				throw new InvalidOperationException(SR.Get("CannotChangeAfterSealed", new object[]
				{
					"FrameworkElementFactory"
				}));
			}
			if (routedEvent == null)
			{
				throw new ArgumentNullException("routedEvent");
			}
			if (handler == null)
			{
				throw new ArgumentNullException("handler");
			}
			if (handler.GetType() != routedEvent.HandlerType)
			{
				throw new ArgumentException(SR.Get("HandlerTypeIllegal"));
			}
			if (this._eventHandlersStore != null)
			{
				this._eventHandlersStore.RemoveRoutedEventHandler(routedEvent, handler);
				if ((routedEvent == FrameworkElement.LoadedEvent || routedEvent == FrameworkElement.UnloadedEvent) && !this._eventHandlersStore.Contains(FrameworkElement.LoadedEvent) && !this._eventHandlersStore.Contains(FrameworkElement.UnloadedEvent))
				{
					this.HasLoadedChangeHandler = false;
				}
			}
		}

		// Token: 0x17000139 RID: 313
		// (get) Token: 0x06000683 RID: 1667 RVA: 0x00014552 File Offset: 0x00012752
		// (set) Token: 0x06000684 RID: 1668 RVA: 0x0001455A File Offset: 0x0001275A
		internal EventHandlersStore EventHandlersStore
		{
			get
			{
				return this._eventHandlersStore;
			}
			set
			{
				this._eventHandlersStore = value;
			}
		}

		// Token: 0x1700013A RID: 314
		// (get) Token: 0x06000685 RID: 1669 RVA: 0x00014563 File Offset: 0x00012763
		// (set) Token: 0x06000686 RID: 1670 RVA: 0x0001456B File Offset: 0x0001276B
		internal bool HasLoadedChangeHandler
		{
			get
			{
				return this._hasLoadedChangeHandler;
			}
			set
			{
				this._hasLoadedChangeHandler = value;
			}
		}

		// Token: 0x06000687 RID: 1671 RVA: 0x00014574 File Offset: 0x00012774
		private void UpdatePropertyValueList(DependencyProperty dp, PropertyValueType valueType, object value)
		{
			int num = -1;
			for (int i = 0; i < this.PropertyValues.Count; i++)
			{
				if (this.PropertyValues[i].Property == dp)
				{
					num = i;
					break;
				}
			}
			if (num >= 0)
			{
				object synchronized = this._synchronized;
				lock (synchronized)
				{
					PropertyValue value2 = this.PropertyValues[num];
					value2.ValueType = valueType;
					value2.ValueInternal = value;
					this.PropertyValues[num] = value2;
					return;
				}
			}
			PropertyValue value3 = default(PropertyValue);
			value3.ValueType = valueType;
			value3.ChildName = null;
			value3.Property = dp;
			value3.ValueInternal = value;
			object synchronized2 = this._synchronized;
			lock (synchronized2)
			{
				this.PropertyValues.Add(value3);
			}
		}

		// Token: 0x06000688 RID: 1672 RVA: 0x00014674 File Offset: 0x00012874
		private DependencyObject CreateDependencyObject()
		{
			if (this._knownTypeFactory != null)
			{
				return this._knownTypeFactory() as DependencyObject;
			}
			return (DependencyObject)Activator.CreateInstance(this._type);
		}

		/// <summary>Gets a value that indicates whether this object is in an immutable state.</summary>
		/// <returns>
		///     <see langword="true" /> if this object is in an immutable state; otherwise, <see langword="false" />.</returns>
		// Token: 0x1700013B RID: 315
		// (get) Token: 0x06000689 RID: 1673 RVA: 0x0001469F File Offset: 0x0001289F
		public bool IsSealed
		{
			get
			{
				return this._sealed;
			}
		}

		/// <summary>Gets the parent <see cref="T:System.Windows.FrameworkElementFactory" />.</summary>
		/// <returns>A <see cref="T:System.Windows.FrameworkElementFactory" /> that is the parent factory.</returns>
		// Token: 0x1700013C RID: 316
		// (get) Token: 0x0600068A RID: 1674 RVA: 0x000146A7 File Offset: 0x000128A7
		public FrameworkElementFactory Parent
		{
			get
			{
				return this._parent;
			}
		}

		/// <summary>Gets the first child factory.</summary>
		/// <returns>A <see cref="T:System.Windows.FrameworkElementFactory" /> the first child factory.</returns>
		// Token: 0x1700013D RID: 317
		// (get) Token: 0x0600068B RID: 1675 RVA: 0x000146AF File Offset: 0x000128AF
		public FrameworkElementFactory FirstChild
		{
			get
			{
				return this._firstChild;
			}
		}

		/// <summary>Gets the next sibling factory.</summary>
		/// <returns>A <see cref="T:System.Windows.FrameworkElementFactory" /> that is the next sibling factory.</returns>
		// Token: 0x1700013E RID: 318
		// (get) Token: 0x0600068C RID: 1676 RVA: 0x000146B7 File Offset: 0x000128B7
		public FrameworkElementFactory NextSibling
		{
			get
			{
				return this._nextSibling;
			}
		}

		// Token: 0x1700013F RID: 319
		// (get) Token: 0x0600068D RID: 1677 RVA: 0x000146BF File Offset: 0x000128BF
		internal FrameworkTemplate FrameworkTemplate
		{
			get
			{
				return this._frameworkTemplate;
			}
		}

		// Token: 0x0600068E RID: 1678 RVA: 0x000146C8 File Offset: 0x000128C8
		internal object GetValue(DependencyProperty dp)
		{
			for (int i = 0; i < this.PropertyValues.Count; i++)
			{
				if (this.PropertyValues[i].ValueType == PropertyValueType.Set && this.PropertyValues[i].Property == dp)
				{
					return this.PropertyValues[i].ValueInternal;
				}
			}
			return DependencyProperty.UnsetValue;
		}

		// Token: 0x0600068F RID: 1679 RVA: 0x00014729 File Offset: 0x00012929
		internal void Seal(FrameworkTemplate ownerTemplate)
		{
			if (this._sealed)
			{
				return;
			}
			this._frameworkTemplate = ownerTemplate;
			this.Seal();
		}

		// Token: 0x06000690 RID: 1680 RVA: 0x00014744 File Offset: 0x00012944
		private void Seal()
		{
			if (this._type == null && this._text == null)
			{
				throw new InvalidOperationException(SR.Get("NullTypeIllegal"));
			}
			if (this._firstChild != null && !typeof(IAddChild).IsAssignableFrom(this._type))
			{
				throw new InvalidOperationException(SR.Get("TypeMustImplementIAddChild", new object[]
				{
					this._type.Name
				}));
			}
			this.ApplyAutoAliasRules();
			if (this._childName != null && this._childName != string.Empty)
			{
				if (!this.IsChildNameValid(this._childName))
				{
					throw new InvalidOperationException(SR.Get("ChildNameNamePatternReserved", new object[]
					{
						this._childName
					}));
				}
				this._childName = string.Intern(this._childName);
			}
			else
			{
				this._childName = this.GenerateChildName();
			}
			object synchronized = this._synchronized;
			lock (synchronized)
			{
				for (int i = 0; i < this.PropertyValues.Count; i++)
				{
					PropertyValue propertyValue = this.PropertyValues[i];
					propertyValue.ChildName = this._childName;
					StyleHelper.SealIfSealable(propertyValue.ValueInternal);
					this.PropertyValues[i] = propertyValue;
				}
			}
			this._sealed = true;
			if (this._childName != null && this._childName != string.Empty && this._frameworkTemplate != null)
			{
				this._childIndex = StyleHelper.CreateChildIndexFromChildName(this._childName, this._frameworkTemplate);
			}
			for (FrameworkElementFactory frameworkElementFactory = this._firstChild; frameworkElementFactory != null; frameworkElementFactory = frameworkElementFactory._nextSibling)
			{
				if (this._frameworkTemplate != null)
				{
					frameworkElementFactory.Seal(this._frameworkTemplate);
				}
			}
		}

		// Token: 0x06000691 RID: 1681 RVA: 0x00014908 File Offset: 0x00012B08
		internal DependencyObject InstantiateTree(UncommonField<HybridDictionary[]> dataField, DependencyObject container, DependencyObject parent, List<DependencyObject> affectedChildren, ref List<DependencyObject> noChildIndexChildren, ref FrugalStructList<ChildPropertyDependent> resourceDependents)
		{
			EventTrace.EasyTraceEvent(EventTrace.Keyword.KeywordXamlBaml, EventTrace.Level.Verbose, EventTrace.Event.WClientParseFefCrInstBegin);
			FrameworkElement frameworkElement = container as FrameworkElement;
			bool flag = frameworkElement != null;
			DependencyObject dependencyObject = null;
			if (this._text != null)
			{
				IAddChild addChild = parent as IAddChild;
				if (addChild == null)
				{
					throw new InvalidOperationException(SR.Get("TypeMustImplementIAddChild", new object[]
					{
						parent.GetType().Name
					}));
				}
				addChild.AddText(this._text);
			}
			else
			{
				dependencyObject = this.CreateDependencyObject();
				EventTrace.EasyTraceEvent(EventTrace.Keyword.KeywordXamlBaml, EventTrace.Level.Verbose, EventTrace.Event.WClientParseFefCrInstEnd);
				FrameworkObject frameworkObject = new FrameworkObject(dependencyObject);
				Visual3D visual3D = null;
				bool flag2 = false;
				if (!frameworkObject.IsValid)
				{
					visual3D = (dependencyObject as Visual3D);
					if (visual3D != null)
					{
						flag2 = true;
					}
				}
				bool isFE = frameworkObject.IsFE;
				if (!flag2)
				{
					FrameworkElementFactory.NewNodeBeginInit(isFE, frameworkObject.FE, frameworkObject.FCE);
					if (StyleHelper.HasResourceDependentsForChild(this._childIndex, ref resourceDependents))
					{
						frameworkObject.HasResourceReference = true;
					}
					FrameworkElementFactory.UpdateChildChains(this._childName, this._childIndex, isFE, frameworkObject.FE, frameworkObject.FCE, affectedChildren, ref noChildIndexChildren);
					FrameworkElementFactory.NewNodeStyledParentProperty(container, flag, isFE, frameworkObject.FE, frameworkObject.FCE);
					if (this._childIndex != -1)
					{
						StyleHelper.CreateInstanceDataForChild(dataField, container, dependencyObject, this._childIndex, this._frameworkTemplate.HasInstanceValues, ref this._frameworkTemplate.ChildRecordFromChildIndex);
					}
					if (this.HasLoadedChangeHandler)
					{
						BroadcastEventHelper.AddHasLoadedChangeHandlerFlagInAncestry(dependencyObject);
					}
				}
				else if (this._childName != null)
				{
					affectedChildren.Add(dependencyObject);
				}
				else
				{
					if (noChildIndexChildren == null)
					{
						noChildIndexChildren = new List<DependencyObject>(4);
					}
					noChildIndexChildren.Add(dependencyObject);
				}
				if (container == parent)
				{
					TemplateNameScope value = new TemplateNameScope(container);
					NameScope.SetNameScope(dependencyObject, value);
					if (flag)
					{
						frameworkElement.TemplateChild = frameworkObject.FE;
					}
					else
					{
						FrameworkElementFactory.AddNodeToLogicalTree((FrameworkContentElement)parent, this._type, isFE, frameworkObject.FE, frameworkObject.FCE);
					}
				}
				else
				{
					this.AddNodeToParent(parent, frameworkObject);
				}
				if (!flag2)
				{
					StyleHelper.InvalidatePropertiesOnTemplateNode(container, frameworkObject, this._childIndex, ref this._frameworkTemplate.ChildRecordFromChildIndex, false, this);
				}
				else
				{
					for (int i = 0; i < this.PropertyValues.Count; i++)
					{
						if (this.PropertyValues[i].ValueType != PropertyValueType.Set)
						{
							throw new NotSupportedException(SR.Get("Template3DValueOnly", new object[]
							{
								this.PropertyValues[i].Property
							}));
						}
						object obj = this.PropertyValues[i].ValueInternal;
						Freezable freezable = obj as Freezable;
						if (freezable != null && !freezable.CanFreeze)
						{
							obj = freezable.Clone();
						}
						MarkupExtension markupExtension = obj as MarkupExtension;
						if (markupExtension != null)
						{
							ProvideValueServiceProvider provideValueServiceProvider = new ProvideValueServiceProvider();
							provideValueServiceProvider.SetData(visual3D, this.PropertyValues[i].Property);
							obj = markupExtension.ProvideValue(provideValueServiceProvider);
						}
						visual3D.SetValue(this.PropertyValues[i].Property, obj);
					}
				}
				for (FrameworkElementFactory frameworkElementFactory = this._firstChild; frameworkElementFactory != null; frameworkElementFactory = frameworkElementFactory._nextSibling)
				{
					frameworkElementFactory.InstantiateTree(dataField, container, dependencyObject, affectedChildren, ref noChildIndexChildren, ref resourceDependents);
				}
				if (!flag2)
				{
					FrameworkElementFactory.NewNodeEndInit(isFE, frameworkObject.FE, frameworkObject.FCE);
				}
			}
			return dependencyObject;
		}

		// Token: 0x06000692 RID: 1682 RVA: 0x00014C34 File Offset: 0x00012E34
		private void AddNodeToParent(DependencyObject parent, FrameworkObject childFrameworkObject)
		{
			RowDefinition rowDefinition = null;
			Grid grid;
			ColumnDefinition columnDefinition;
			if (childFrameworkObject.IsFCE && (grid = (parent as Grid)) != null && ((columnDefinition = (childFrameworkObject.FCE as ColumnDefinition)) != null || (rowDefinition = (childFrameworkObject.FCE as RowDefinition)) != null))
			{
				if (columnDefinition != null)
				{
					grid.ColumnDefinitions.Add(columnDefinition);
					return;
				}
				if (rowDefinition != null)
				{
					grid.RowDefinitions.Add(rowDefinition);
					return;
				}
			}
			else
			{
				if (!(parent is IAddChild))
				{
					throw new InvalidOperationException(SR.Get("TypeMustImplementIAddChild", new object[]
					{
						parent.GetType().Name
					}));
				}
				((IAddChild)parent).AddChild(childFrameworkObject.DO);
			}
		}

		// Token: 0x06000693 RID: 1683 RVA: 0x00014CD4 File Offset: 0x00012ED4
		internal FrameworkObject InstantiateUnoptimizedTree()
		{
			if (!this._sealed)
			{
				throw new InvalidOperationException(SR.Get("FrameworkElementFactoryMustBeSealed"));
			}
			FrameworkObject result = new FrameworkObject(this.CreateDependencyObject());
			result.BeginInit();
			ProvideValueServiceProvider provideValueServiceProvider = null;
			FrameworkTemplate.SetTemplateParentValues(this.Name, result.DO, this._frameworkTemplate, ref provideValueServiceProvider);
			FrameworkElementFactory frameworkElementFactory = this._firstChild;
			IAddChild addChild = null;
			if (frameworkElementFactory != null)
			{
				addChild = (result.DO as IAddChild);
				if (addChild == null)
				{
					throw new InvalidOperationException(SR.Get("TypeMustImplementIAddChild", new object[]
					{
						result.DO.GetType().Name
					}));
				}
			}
			while (frameworkElementFactory != null)
			{
				if (frameworkElementFactory._text != null)
				{
					addChild.AddText(frameworkElementFactory._text);
				}
				else
				{
					FrameworkObject childFrameworkObject = frameworkElementFactory.InstantiateUnoptimizedTree();
					this.AddNodeToParent(result.DO, childFrameworkObject);
				}
				frameworkElementFactory = frameworkElementFactory._nextSibling;
			}
			result.EndInit();
			return result;
		}

		// Token: 0x06000694 RID: 1684 RVA: 0x00014DB0 File Offset: 0x00012FB0
		private static void UpdateChildChains(string childID, int childIndex, bool treeNodeIsFE, FrameworkElement treeNodeFE, FrameworkContentElement treeNodeFCE, List<DependencyObject> affectedChildren, ref List<DependencyObject> noChildIndexChildren)
		{
			if (childID != null)
			{
				if (treeNodeIsFE)
				{
					treeNodeFE.TemplateChildIndex = childIndex;
				}
				else
				{
					treeNodeFCE.TemplateChildIndex = childIndex;
				}
				affectedChildren.Add(treeNodeIsFE ? treeNodeFE : treeNodeFCE);
				return;
			}
			if (noChildIndexChildren == null)
			{
				noChildIndexChildren = new List<DependencyObject>(4);
			}
			noChildIndexChildren.Add(treeNodeIsFE ? treeNodeFE : treeNodeFCE);
		}

		// Token: 0x06000695 RID: 1685 RVA: 0x00014E02 File Offset: 0x00013002
		internal static void NewNodeBeginInit(bool treeNodeIsFE, FrameworkElement treeNodeFE, FrameworkContentElement treeNodeFCE)
		{
			if (treeNodeIsFE)
			{
				treeNodeFE.BeginInit();
				return;
			}
			treeNodeFCE.BeginInit();
		}

		// Token: 0x06000696 RID: 1686 RVA: 0x00014E14 File Offset: 0x00013014
		private static void NewNodeEndInit(bool treeNodeIsFE, FrameworkElement treeNodeFE, FrameworkContentElement treeNodeFCE)
		{
			if (treeNodeIsFE)
			{
				treeNodeFE.EndInit();
				return;
			}
			treeNodeFCE.EndInit();
		}

		// Token: 0x06000697 RID: 1687 RVA: 0x00014E26 File Offset: 0x00013026
		private static void NewNodeStyledParentProperty(DependencyObject container, bool isContainerAnFE, bool treeNodeIsFE, FrameworkElement treeNodeFE, FrameworkContentElement treeNodeFCE)
		{
			if (treeNodeIsFE)
			{
				treeNodeFE._templatedParent = container;
				treeNodeFE.IsTemplatedParentAnFE = isContainerAnFE;
				return;
			}
			treeNodeFCE._templatedParent = container;
			treeNodeFCE.IsTemplatedParentAnFE = isContainerAnFE;
		}

		// Token: 0x06000698 RID: 1688 RVA: 0x00014E4C File Offset: 0x0001304C
		internal static void AddNodeToLogicalTree(DependencyObject parent, Type type, bool treeNodeIsFE, FrameworkElement treeNodeFE, FrameworkContentElement treeNodeFCE)
		{
			FrameworkContentElement frameworkContentElement = parent as FrameworkContentElement;
			if (frameworkContentElement != null)
			{
				IEnumerator logicalChildren = frameworkContentElement.LogicalChildren;
				if (logicalChildren != null && logicalChildren.MoveNext())
				{
					throw new InvalidOperationException(SR.Get("AlreadyHasLogicalChildren", new object[]
					{
						parent.GetType().Name
					}));
				}
			}
			IAddChild addChild = parent as IAddChild;
			if (addChild == null)
			{
				throw new InvalidOperationException(SR.Get("CannotHookupFCERoot", new object[]
				{
					type.Name
				}));
			}
			if (treeNodeFE != null)
			{
				addChild.AddChild(treeNodeFE);
				return;
			}
			addChild.AddChild(treeNodeFCE);
		}

		// Token: 0x06000699 RID: 1689 RVA: 0x00014ED5 File Offset: 0x000130D5
		internal bool IsChildNameValid(string childName)
		{
			return !childName.StartsWith(FrameworkElementFactory.AutoGenChildNamePrefix, StringComparison.Ordinal);
		}

		// Token: 0x0600069A RID: 1690 RVA: 0x00014EE8 File Offset: 0x000130E8
		private string GenerateChildName()
		{
			string result = FrameworkElementFactory.AutoGenChildNamePrefix + FrameworkElementFactory.AutoGenChildNamePostfix.ToString(CultureInfo.InvariantCulture);
			Interlocked.Increment(ref FrameworkElementFactory.AutoGenChildNamePostfix);
			return result;
		}

		// Token: 0x0600069B RID: 1691 RVA: 0x00014F1C File Offset: 0x0001311C
		private void ApplyAutoAliasRules()
		{
			if (typeof(ContentPresenter).IsAssignableFrom(this._type))
			{
				object value = this.GetValue(ContentPresenter.ContentSourceProperty);
				string text = (value == DependencyProperty.UnsetValue) ? "Content" : ((string)value);
				if (!string.IsNullOrEmpty(text) && !this.IsValueDefined(ContentPresenter.ContentProperty))
				{
					Type targetTypeInternal = this._frameworkTemplate.TargetTypeInternal;
					DependencyProperty dependencyProperty = DependencyProperty.FromName(text, targetTypeInternal);
					DependencyProperty dependencyProperty2 = DependencyProperty.FromName(text + "Template", targetTypeInternal);
					DependencyProperty dependencyProperty3 = DependencyProperty.FromName(text + "TemplateSelector", targetTypeInternal);
					DependencyProperty dependencyProperty4 = DependencyProperty.FromName(text + "StringFormat", targetTypeInternal);
					if (dependencyProperty == null && value != DependencyProperty.UnsetValue)
					{
						throw new InvalidOperationException(SR.Get("MissingContentSource", new object[]
						{
							text,
							targetTypeInternal
						}));
					}
					if (dependencyProperty != null)
					{
						this.SetValue(ContentPresenter.ContentProperty, new TemplateBindingExtension(dependencyProperty));
					}
					if (!this.IsValueDefined(ContentPresenter.ContentTemplateProperty) && !this.IsValueDefined(ContentPresenter.ContentTemplateSelectorProperty) && !this.IsValueDefined(ContentPresenter.ContentStringFormatProperty))
					{
						if (dependencyProperty2 != null)
						{
							this.SetValue(ContentPresenter.ContentTemplateProperty, new TemplateBindingExtension(dependencyProperty2));
						}
						if (dependencyProperty3 != null)
						{
							this.SetValue(ContentPresenter.ContentTemplateSelectorProperty, new TemplateBindingExtension(dependencyProperty3));
						}
						if (dependencyProperty4 != null)
						{
							this.SetValue(ContentPresenter.ContentStringFormatProperty, new TemplateBindingExtension(dependencyProperty4));
							return;
						}
					}
				}
			}
			else if (typeof(GridViewRowPresenter).IsAssignableFrom(this._type))
			{
				if (!this.IsValueDefined(GridViewRowPresenter.ContentProperty))
				{
					Type targetTypeInternal2 = this._frameworkTemplate.TargetTypeInternal;
					DependencyProperty dependencyProperty5 = DependencyProperty.FromName("Content", targetTypeInternal2);
					if (dependencyProperty5 != null)
					{
						this.SetValue(GridViewRowPresenter.ContentProperty, new TemplateBindingExtension(dependencyProperty5));
					}
				}
				if (!this.IsValueDefined(GridViewRowPresenterBase.ColumnsProperty))
				{
					this.SetValue(GridViewRowPresenterBase.ColumnsProperty, new TemplateBindingExtension(GridView.ColumnCollectionProperty));
				}
			}
		}

		// Token: 0x0600069C RID: 1692 RVA: 0x000150F8 File Offset: 0x000132F8
		private bool IsValueDefined(DependencyProperty dp)
		{
			for (int i = 0; i < this.PropertyValues.Count; i++)
			{
				if (this.PropertyValues[i].Property == dp && (this.PropertyValues[i].ValueType == PropertyValueType.Set || this.PropertyValues[i].ValueType == PropertyValueType.Resource || this.PropertyValues[i].ValueType == PropertyValueType.TemplateBinding))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x040006D7 RID: 1751
		private bool _sealed;

		// Token: 0x040006D8 RID: 1752
		internal FrugalStructList<PropertyValue> PropertyValues;

		// Token: 0x040006D9 RID: 1753
		private EventHandlersStore _eventHandlersStore;

		// Token: 0x040006DA RID: 1754
		internal bool _hasLoadedChangeHandler;

		// Token: 0x040006DB RID: 1755
		private Type _type;

		// Token: 0x040006DC RID: 1756
		private string _text;

		// Token: 0x040006DD RID: 1757
		private Func<object> _knownTypeFactory;

		// Token: 0x040006DE RID: 1758
		private string _childName;

		// Token: 0x040006DF RID: 1759
		internal int _childIndex = -1;

		// Token: 0x040006E0 RID: 1760
		private FrameworkTemplate _frameworkTemplate;

		// Token: 0x040006E1 RID: 1761
		private static int AutoGenChildNamePostfix = 1;

		// Token: 0x040006E2 RID: 1762
		private static string AutoGenChildNamePrefix = "~ChildID";

		// Token: 0x040006E3 RID: 1763
		private FrameworkElementFactory _parent;

		// Token: 0x040006E4 RID: 1764
		private FrameworkElementFactory _firstChild;

		// Token: 0x040006E5 RID: 1765
		private FrameworkElementFactory _lastChild;

		// Token: 0x040006E6 RID: 1766
		private FrameworkElementFactory _nextSibling;

		// Token: 0x040006E7 RID: 1767
		private object _synchronized = new object();
	}
}
