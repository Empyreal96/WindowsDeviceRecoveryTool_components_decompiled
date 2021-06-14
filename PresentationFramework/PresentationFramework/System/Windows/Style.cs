using System;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows.Markup;
using System.Windows.Threading;
using MS.Utility;

namespace System.Windows
{
	/// <summary>Enables the sharing of properties, resources, and event handlers between instances of a type.</summary>
	// Token: 0x020000FA RID: 250
	[Localizability(LocalizationCategory.Ignore)]
	[DictionaryKeyProperty("TargetType")]
	[ContentProperty("Setters")]
	public class Style : DispatcherObject, INameScope, IAddChild, ISealable, IHaveResources, IQueryAmbient
	{
		// Token: 0x060008C1 RID: 2241 RVA: 0x0001C488 File Offset: 0x0001A688
		static Style()
		{
			StyleHelper.RegisterAlternateExpressionStorage();
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Style" /> class. </summary>
		// Token: 0x060008C2 RID: 2242 RVA: 0x0001C4AE File Offset: 0x0001A6AE
		public Style()
		{
			this.GetUniqueGlobalIndex();
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Style" /> class to use on the specified <see cref="T:System.Type" />. </summary>
		/// <param name="targetType">The type to which the style will apply.</param>
		// Token: 0x060008C3 RID: 2243 RVA: 0x0001C4DE File Offset: 0x0001A6DE
		public Style(Type targetType)
		{
			this.TargetType = targetType;
			this.GetUniqueGlobalIndex();
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Style" /> class to use on the specified <see cref="T:System.Type" /> and based on the specified <see cref="T:System.Windows.Style" />. </summary>
		/// <param name="targetType">The type to which the style will apply.</param>
		/// <param name="basedOn">The style to base this style on.</param>
		// Token: 0x060008C4 RID: 2244 RVA: 0x0001C515 File Offset: 0x0001A715
		public Style(Type targetType, Style basedOn)
		{
			this.TargetType = targetType;
			this.BasedOn = basedOn;
			this.GetUniqueGlobalIndex();
		}

		/// <summary>Registers a new name-object pair in the current namescope.</summary>
		/// <param name="name">The name to register.</param>
		/// <param name="scopedElement">The object to map to the specified <paramref name="name" />.</param>
		// Token: 0x060008C5 RID: 2245 RVA: 0x0001C553 File Offset: 0x0001A753
		public void RegisterName(string name, object scopedElement)
		{
			base.VerifyAccess();
			this._nameScope.RegisterName(name, scopedElement);
		}

		/// <summary>Removes a name-object mapping from the namescope.</summary>
		/// <param name="name">The name of the mapping to remove.</param>
		// Token: 0x060008C6 RID: 2246 RVA: 0x0001C568 File Offset: 0x0001A768
		public void UnregisterName(string name)
		{
			base.VerifyAccess();
			this._nameScope.UnregisterName(name);
		}

		/// <summary>Returns an object that has the provided identifying name. </summary>
		/// <param name="name">The name identifier for the object being requested.</param>
		/// <returns>The object, if found. Returns <see langword="null" /> if no object of that name was found.</returns>
		// Token: 0x060008C7 RID: 2247 RVA: 0x0001C57C File Offset: 0x0001A77C
		object INameScope.FindName(string name)
		{
			base.VerifyAccess();
			return this._nameScope.FindName(name);
		}

		// Token: 0x060008C8 RID: 2248 RVA: 0x0001C590 File Offset: 0x0001A790
		private void GetUniqueGlobalIndex()
		{
			object synchronized = Style.Synchronized;
			lock (synchronized)
			{
				Style.StyleInstanceCount++;
				this.GlobalIndex = Style.StyleInstanceCount;
			}
		}

		/// <summary>Gets a value that indicates whether the style is read-only and cannot be changed.</summary>
		/// <returns>
		///     <see langword="true" /> if the style is sealed; otherwise <see langword="false" />.</returns>
		// Token: 0x170001D5 RID: 469
		// (get) Token: 0x060008C9 RID: 2249 RVA: 0x0001C5E0 File Offset: 0x0001A7E0
		public bool IsSealed
		{
			get
			{
				base.VerifyAccess();
				return this._sealed;
			}
		}

		/// <summary>Gets or sets the type for which this style is intended.</summary>
		/// <returns>The target type for this style.</returns>
		// Token: 0x170001D6 RID: 470
		// (get) Token: 0x060008CA RID: 2250 RVA: 0x0001C5EE File Offset: 0x0001A7EE
		// (set) Token: 0x060008CB RID: 2251 RVA: 0x0001C5FC File Offset: 0x0001A7FC
		[Ambient]
		[Localizability(LocalizationCategory.NeverLocalize)]
		public Type TargetType
		{
			get
			{
				base.VerifyAccess();
				return this._targetType;
			}
			set
			{
				base.VerifyAccess();
				if (this._sealed)
				{
					throw new InvalidOperationException(SR.Get("CannotChangeAfterSealed", new object[]
					{
						"Style"
					}));
				}
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				if (!typeof(FrameworkElement).IsAssignableFrom(value) && !typeof(FrameworkContentElement).IsAssignableFrom(value) && !(Style.DefaultTargetType == value))
				{
					throw new ArgumentException(SR.Get("MustBeFrameworkDerived", new object[]
					{
						value.Name
					}));
				}
				this._targetType = value;
				this.SetModified(1);
			}
		}

		/// <summary>Gets or sets a defined style that is the basis of the current style.</summary>
		/// <returns>A defined style that is the basis of the current style. The default value is <see langword="null" />.</returns>
		// Token: 0x170001D7 RID: 471
		// (get) Token: 0x060008CC RID: 2252 RVA: 0x0001C6A7 File Offset: 0x0001A8A7
		// (set) Token: 0x060008CD RID: 2253 RVA: 0x0001C6B8 File Offset: 0x0001A8B8
		[DefaultValue(null)]
		[Ambient]
		public Style BasedOn
		{
			get
			{
				base.VerifyAccess();
				return this._basedOn;
			}
			set
			{
				base.VerifyAccess();
				if (this._sealed)
				{
					throw new InvalidOperationException(SR.Get("CannotChangeAfterSealed", new object[]
					{
						"Style"
					}));
				}
				if (value == this)
				{
					throw new ArgumentException(SR.Get("StyleCannotBeBasedOnSelf"));
				}
				this._basedOn = value;
				this.SetModified(2);
			}
		}

		/// <summary>Gets a collection of <see cref="T:System.Windows.TriggerBase" /> objects that apply property values based on specified conditions.</summary>
		/// <returns>A collection of <see cref="T:System.Windows.TriggerBase" /> objects. The default is an empty collection.</returns>
		// Token: 0x170001D8 RID: 472
		// (get) Token: 0x060008CE RID: 2254 RVA: 0x0001C713 File Offset: 0x0001A913
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public TriggerCollection Triggers
		{
			get
			{
				base.VerifyAccess();
				if (this._visualTriggers == null)
				{
					this._visualTriggers = new TriggerCollection();
					if (this._sealed)
					{
						this._visualTriggers.Seal();
					}
				}
				return this._visualTriggers;
			}
		}

		/// <summary>Gets a collection of <see cref="T:System.Windows.Setter" /> and <see cref="T:System.Windows.EventSetter" /> objects.</summary>
		/// <returns>A collection of <see cref="T:System.Windows.Setter" /> and <see cref="T:System.Windows.EventSetter" /> objects. The default is an empty collection.</returns>
		// Token: 0x170001D9 RID: 473
		// (get) Token: 0x060008CF RID: 2255 RVA: 0x0001C747 File Offset: 0x0001A947
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public SetterBaseCollection Setters
		{
			get
			{
				base.VerifyAccess();
				if (this._setters == null)
				{
					this._setters = new SetterBaseCollection();
					if (this._sealed)
					{
						this._setters.Seal();
					}
				}
				return this._setters;
			}
		}

		/// <summary>Gets or sets the collection of resources that can be used within the scope of this style.</summary>
		/// <returns>The resources that can be used within the scope of this style.</returns>
		// Token: 0x170001DA RID: 474
		// (get) Token: 0x060008D0 RID: 2256 RVA: 0x0001C77C File Offset: 0x0001A97C
		// (set) Token: 0x060008D1 RID: 2257 RVA: 0x0001C7C8 File Offset: 0x0001A9C8
		[Ambient]
		public ResourceDictionary Resources
		{
			get
			{
				base.VerifyAccess();
				if (this._resources == null)
				{
					this._resources = new ResourceDictionary();
					this._resources.CanBeAccessedAcrossThreads = true;
					if (this._sealed)
					{
						this._resources.IsReadOnly = true;
					}
				}
				return this._resources;
			}
			set
			{
				base.VerifyAccess();
				if (this._sealed)
				{
					throw new InvalidOperationException(SR.Get("CannotChangeAfterSealed", new object[]
					{
						"Style"
					}));
				}
				this._resources = value;
				if (this._resources != null)
				{
					this._resources.CanBeAccessedAcrossThreads = true;
				}
			}
		}

		// Token: 0x170001DB RID: 475
		// (get) Token: 0x060008D2 RID: 2258 RVA: 0x0001C81C File Offset: 0x0001AA1C
		// (set) Token: 0x060008D3 RID: 2259 RVA: 0x0001C824 File Offset: 0x0001AA24
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

		// Token: 0x060008D4 RID: 2260 RVA: 0x0001C830 File Offset: 0x0001AA30
		internal object FindResource(object resourceKey, bool allowDeferredResourceReference, bool mustReturnDeferredResourceReference)
		{
			if (this._resources != null && this._resources.Contains(resourceKey))
			{
				bool flag;
				return this._resources.FetchResource(resourceKey, allowDeferredResourceReference, mustReturnDeferredResourceReference, out flag);
			}
			if (this._basedOn != null)
			{
				return this._basedOn.FindResource(resourceKey, allowDeferredResourceReference, mustReturnDeferredResourceReference);
			}
			return DependencyProperty.UnsetValue;
		}

		// Token: 0x060008D5 RID: 2261 RVA: 0x0001C880 File Offset: 0x0001AA80
		internal ResourceDictionary FindResourceDictionary(object resourceKey)
		{
			if (this._resources != null && this._resources.Contains(resourceKey))
			{
				return this._resources;
			}
			if (this._basedOn != null)
			{
				return this._basedOn.FindResourceDictionary(resourceKey);
			}
			return null;
		}

		/// <summary>Queries whether a specified ambient property is available in the current scope.</summary>
		/// <param name="propertyName">The name of the requested ambient property.</param>
		/// <returns>
		///     <see langword="true" /> if the requested ambient property is available; otherwise, <see langword="false" />.</returns>
		// Token: 0x060008D6 RID: 2262 RVA: 0x0001C8B5 File Offset: 0x0001AAB5
		bool IQueryAmbient.IsAmbientPropertyAvailable(string propertyName)
		{
			if (!(propertyName == "Resources"))
			{
				if (propertyName == "BasedOn")
				{
					if (this._basedOn == null)
					{
						return false;
					}
				}
			}
			else if (this._resources == null)
			{
				return false;
			}
			return true;
		}

		/// <summary>Adds a child object. </summary>
		/// <param name="value">The child object to add.</param>
		// Token: 0x060008D7 RID: 2263 RVA: 0x0001C8E8 File Offset: 0x0001AAE8
		void IAddChild.AddChild(object value)
		{
			base.VerifyAccess();
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			SetterBase setterBase = value as SetterBase;
			if (setterBase == null)
			{
				throw new ArgumentException(SR.Get("UnexpectedParameterType", new object[]
				{
					value.GetType(),
					typeof(SetterBase)
				}), "value");
			}
			this.Setters.Add(setterBase);
		}

		/// <summary>Adds the text content of a node to the object. </summary>
		/// <param name="text">The text to add to the object.</param>
		// Token: 0x060008D8 RID: 2264 RVA: 0x0000A446 File Offset: 0x00008646
		void IAddChild.AddText(string text)
		{
			base.VerifyAccess();
			XamlSerializerUtil.ThrowIfNonWhiteSpaceInAddText(text, this);
		}

		// Token: 0x060008D9 RID: 2265 RVA: 0x0001C950 File Offset: 0x0001AB50
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
				PropertyValue value2 = this.PropertyValues[num];
				value2.ValueType = valueType;
				value2.ValueInternal = value;
				this.PropertyValues[num] = value2;
				return;
			}
			this.PropertyValues.Add(new PropertyValue
			{
				ValueType = valueType,
				ChildName = "~Self",
				Property = dp,
				ValueInternal = value
			});
		}

		// Token: 0x060008DA RID: 2266 RVA: 0x0001C9F8 File Offset: 0x0001ABF8
		internal void CheckTargetType(object element)
		{
			if (Style.DefaultTargetType == this.TargetType)
			{
				return;
			}
			Type type = element.GetType();
			if (!this.TargetType.IsAssignableFrom(type))
			{
				throw new InvalidOperationException(SR.Get("StyleTargetTypeMismatchWithElement", new object[]
				{
					this.TargetType.Name,
					type.Name
				}));
			}
		}

		/// <summary>Locks this style and all factories and triggers so they cannot be changed.</summary>
		// Token: 0x060008DB RID: 2267 RVA: 0x0001CA5C File Offset: 0x0001AC5C
		public void Seal()
		{
			base.VerifyAccess();
			if (this._sealed)
			{
				return;
			}
			if (this._targetType == null)
			{
				throw new InvalidOperationException(SR.Get("NullPropertyIllegal", new object[]
				{
					"TargetType"
				}));
			}
			if (this._basedOn != null && Style.DefaultTargetType != this._basedOn.TargetType && !this._basedOn.TargetType.IsAssignableFrom(this._targetType))
			{
				throw new InvalidOperationException(SR.Get("MustBaseOnStyleOfABaseType", new object[]
				{
					this._targetType.Name
				}));
			}
			if (this._setters != null)
			{
				this._setters.Seal();
			}
			if (this._visualTriggers != null)
			{
				this._visualTriggers.Seal();
			}
			this.CheckForCircularBasedOnReferences();
			if (this._basedOn != null)
			{
				this._basedOn.Seal();
			}
			if (this._resources != null)
			{
				this._resources.IsReadOnly = true;
			}
			this.ProcessSetters(this);
			StyleHelper.AddEventDependent(0, this.EventHandlersStore, ref this.EventDependents);
			this.ProcessSelfStyles(this);
			this.ProcessVisualTriggers(this);
			StyleHelper.SortResourceDependents(ref this.ResourceDependents);
			this._sealed = true;
			base.DetachFromDispatcher();
		}

		// Token: 0x060008DC RID: 2268 RVA: 0x0001CB94 File Offset: 0x0001AD94
		private void CheckForCircularBasedOnReferences()
		{
			Stack stack = new Stack(10);
			for (Style style = this; style != null; style = style.BasedOn)
			{
				if (stack.Contains(style))
				{
					throw new InvalidOperationException(SR.Get("StyleBasedOnHasLoop"));
				}
				stack.Push(style);
			}
		}

		// Token: 0x060008DD RID: 2269 RVA: 0x0001CBD8 File Offset: 0x0001ADD8
		private void ProcessSetters(Style style)
		{
			if (style == null)
			{
				return;
			}
			style.Setters.Seal();
			if (this.PropertyValues.Count == 0)
			{
				this.PropertyValues = new FrugalStructList<PropertyValue>(style.Setters.Count);
			}
			for (int i = 0; i < style.Setters.Count; i++)
			{
				SetterBase setterBase = style.Setters[i];
				Setter setter = setterBase as Setter;
				if (setter != null)
				{
					if (setter.TargetName != null)
					{
						throw new InvalidOperationException(SR.Get("SetterOnStyleNotAllowedToHaveTarget", new object[]
						{
							setter.TargetName
						}));
					}
					if (style == this)
					{
						DynamicResourceExtension dynamicResourceExtension = setter.ValueInternal as DynamicResourceExtension;
						if (dynamicResourceExtension == null)
						{
							this.UpdatePropertyValueList(setter.Property, PropertyValueType.Set, setter.ValueInternal);
						}
						else
						{
							this.UpdatePropertyValueList(setter.Property, PropertyValueType.Resource, dynamicResourceExtension.ResourceKey);
						}
					}
				}
				else
				{
					EventSetter eventSetter = (EventSetter)setterBase;
					if (this._eventHandlersStore == null)
					{
						this._eventHandlersStore = new EventHandlersStore();
					}
					this._eventHandlersStore.AddRoutedEventHandler(eventSetter.Event, eventSetter.Handler, eventSetter.HandledEventsToo);
					this.SetModified(16);
					if (eventSetter.Event == FrameworkElement.LoadedEvent || eventSetter.Event == FrameworkElement.UnloadedEvent)
					{
						this._hasLoadedChangeHandler = true;
					}
				}
			}
			this.ProcessSetters(style._basedOn);
		}

		// Token: 0x060008DE RID: 2270 RVA: 0x0001CD24 File Offset: 0x0001AF24
		private void ProcessSelfStyles(Style style)
		{
			if (style == null)
			{
				return;
			}
			this.ProcessSelfStyles(style._basedOn);
			for (int i = 0; i < style.PropertyValues.Count; i++)
			{
				PropertyValue propertyValue = style.PropertyValues[i];
				StyleHelper.UpdateTables(ref propertyValue, ref this.ChildRecordFromChildIndex, ref this.TriggerSourceRecordFromChildIndex, ref this.ResourceDependents, ref this._dataTriggerRecordFromBinding, null, ref this._hasInstanceValues);
				StyleHelper.AddContainerDependent(propertyValue.Property, false, ref this.ContainerDependents);
			}
		}

		// Token: 0x060008DF RID: 2271 RVA: 0x0001CD9C File Offset: 0x0001AF9C
		private void ProcessVisualTriggers(Style style)
		{
			if (style == null)
			{
				return;
			}
			this.ProcessVisualTriggers(style._basedOn);
			if (style._visualTriggers != null)
			{
				int count = style._visualTriggers.Count;
				for (int i = 0; i < count; i++)
				{
					TriggerBase triggerBase = style._visualTriggers[i];
					for (int j = 0; j < triggerBase.PropertyValues.Count; j++)
					{
						PropertyValue propertyValue = triggerBase.PropertyValues[j];
						if (propertyValue.ChildName != "~Self")
						{
							throw new InvalidOperationException(SR.Get("StyleTriggersCannotTargetTheTemplate"));
						}
						TriggerCondition[] conditions = propertyValue.Conditions;
						for (int k = 0; k < conditions.Length; k++)
						{
							if (conditions[k].SourceName != "~Self")
							{
								throw new InvalidOperationException(SR.Get("TriggerOnStyleNotAllowedToHaveSource", new object[]
								{
									conditions[k].SourceName
								}));
							}
						}
						StyleHelper.AddContainerDependent(propertyValue.Property, true, ref this.ContainerDependents);
						StyleHelper.UpdateTables(ref propertyValue, ref this.ChildRecordFromChildIndex, ref this.TriggerSourceRecordFromChildIndex, ref this.ResourceDependents, ref this._dataTriggerRecordFromBinding, null, ref this._hasInstanceValues);
					}
					if (triggerBase.HasEnterActions || triggerBase.HasExitActions)
					{
						if (triggerBase is Trigger)
						{
							StyleHelper.AddPropertyTriggerWithAction(triggerBase, ((Trigger)triggerBase).Property, ref this.PropertyTriggersWithActions);
						}
						else if (triggerBase is MultiTrigger)
						{
							MultiTrigger multiTrigger = (MultiTrigger)triggerBase;
							for (int l = 0; l < multiTrigger.Conditions.Count; l++)
							{
								Condition condition = multiTrigger.Conditions[l];
								StyleHelper.AddPropertyTriggerWithAction(triggerBase, condition.Property, ref this.PropertyTriggersWithActions);
							}
						}
						else if (triggerBase is DataTrigger)
						{
							StyleHelper.AddDataTriggerWithAction(triggerBase, ((DataTrigger)triggerBase).Binding, ref this.DataTriggersWithActions);
						}
						else
						{
							if (!(triggerBase is MultiDataTrigger))
							{
								throw new InvalidOperationException(SR.Get("UnsupportedTriggerInStyle", new object[]
								{
									triggerBase.GetType().Name
								}));
							}
							MultiDataTrigger multiDataTrigger = (MultiDataTrigger)triggerBase;
							for (int m = 0; m < multiDataTrigger.Conditions.Count; m++)
							{
								Condition condition2 = multiDataTrigger.Conditions[m];
								StyleHelper.AddDataTriggerWithAction(triggerBase, condition2.Binding, ref this.DataTriggersWithActions);
							}
						}
					}
					EventTrigger eventTrigger = triggerBase as EventTrigger;
					if (eventTrigger != null)
					{
						if (eventTrigger.SourceName != null && eventTrigger.SourceName.Length > 0)
						{
							throw new InvalidOperationException(SR.Get("EventTriggerOnStyleNotAllowedToHaveTarget", new object[]
							{
								eventTrigger.SourceName
							}));
						}
						StyleHelper.ProcessEventTrigger(eventTrigger, null, ref this._triggerActions, ref this.EventDependents, null, null, ref this._eventHandlersStore, ref this._hasLoadedChangeHandler);
					}
				}
			}
		}

		/// <summary>Returns the hash code for this <see cref="T:System.Windows.Style" />.    </summary>
		/// <returns>The hash code for this <see cref="T:System.Windows.Style" />.   </returns>
		// Token: 0x060008E0 RID: 2272 RVA: 0x0001D056 File Offset: 0x0001B256
		public override int GetHashCode()
		{
			base.VerifyAccess();
			return this.GlobalIndex;
		}

		// Token: 0x170001DC RID: 476
		// (get) Token: 0x060008E1 RID: 2273 RVA: 0x00016748 File Offset: 0x00014948
		bool ISealable.CanSeal
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170001DD RID: 477
		// (get) Token: 0x060008E2 RID: 2274 RVA: 0x0001D064 File Offset: 0x0001B264
		bool ISealable.IsSealed
		{
			get
			{
				return this.IsSealed;
			}
		}

		// Token: 0x060008E3 RID: 2275 RVA: 0x0001D06C File Offset: 0x0001B26C
		void ISealable.Seal()
		{
			this.Seal();
		}

		// Token: 0x170001DE RID: 478
		// (get) Token: 0x060008E4 RID: 2276 RVA: 0x0001D074 File Offset: 0x0001B274
		internal bool HasResourceReferences
		{
			get
			{
				return this.ResourceDependents.Count > 0;
			}
		}

		// Token: 0x170001DF RID: 479
		// (get) Token: 0x060008E5 RID: 2277 RVA: 0x0001D084 File Offset: 0x0001B284
		internal EventHandlersStore EventHandlersStore
		{
			get
			{
				return this._eventHandlersStore;
			}
		}

		// Token: 0x170001E0 RID: 480
		// (get) Token: 0x060008E6 RID: 2278 RVA: 0x0001D08C File Offset: 0x0001B28C
		internal bool HasEventDependents
		{
			get
			{
				return this.EventDependents.Count > 0;
			}
		}

		// Token: 0x170001E1 RID: 481
		// (get) Token: 0x060008E7 RID: 2279 RVA: 0x0001D09C File Offset: 0x0001B29C
		internal bool HasEventSetters
		{
			get
			{
				return this.IsModified(16);
			}
		}

		// Token: 0x170001E2 RID: 482
		// (get) Token: 0x060008E8 RID: 2280 RVA: 0x0001D0A6 File Offset: 0x0001B2A6
		internal bool HasInstanceValues
		{
			get
			{
				return this._hasInstanceValues;
			}
		}

		// Token: 0x170001E3 RID: 483
		// (get) Token: 0x060008E9 RID: 2281 RVA: 0x0001D0AE File Offset: 0x0001B2AE
		// (set) Token: 0x060008EA RID: 2282 RVA: 0x0001D0B6 File Offset: 0x0001B2B6
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

		// Token: 0x060008EB RID: 2283 RVA: 0x0001D0BF File Offset: 0x0001B2BF
		private static bool IsEqual(object a, object b)
		{
			if (a == null)
			{
				return b == null;
			}
			return a.Equals(b);
		}

		// Token: 0x170001E4 RID: 484
		// (get) Token: 0x060008EC RID: 2284 RVA: 0x0001D0D0 File Offset: 0x0001B2D0
		internal bool IsBasedOnModified
		{
			get
			{
				return this.IsModified(2);
			}
		}

		// Token: 0x060008ED RID: 2285 RVA: 0x0001D0D9 File Offset: 0x0001B2D9
		private void SetModified(int id)
		{
			this._modified |= id;
		}

		// Token: 0x060008EE RID: 2286 RVA: 0x0001D0E9 File Offset: 0x0001B2E9
		internal bool IsModified(int id)
		{
			return (id & this._modified) != 0;
		}

		// Token: 0x040007BD RID: 1981
		private NameScope _nameScope = new NameScope();

		// Token: 0x040007BE RID: 1982
		private EventHandlersStore _eventHandlersStore;

		// Token: 0x040007BF RID: 1983
		private bool _sealed;

		// Token: 0x040007C0 RID: 1984
		private bool _hasInstanceValues;

		// Token: 0x040007C1 RID: 1985
		internal static readonly Type DefaultTargetType = typeof(IFrameworkInputElement);

		// Token: 0x040007C2 RID: 1986
		private bool _hasLoadedChangeHandler;

		// Token: 0x040007C3 RID: 1987
		private Type _targetType = Style.DefaultTargetType;

		// Token: 0x040007C4 RID: 1988
		private Style _basedOn;

		// Token: 0x040007C5 RID: 1989
		private TriggerCollection _visualTriggers;

		// Token: 0x040007C6 RID: 1990
		private SetterBaseCollection _setters;

		// Token: 0x040007C7 RID: 1991
		internal ResourceDictionary _resources;

		// Token: 0x040007C8 RID: 1992
		internal int GlobalIndex;

		// Token: 0x040007C9 RID: 1993
		internal FrugalStructList<ChildRecord> ChildRecordFromChildIndex;

		// Token: 0x040007CA RID: 1994
		internal FrugalStructList<ItemStructMap<TriggerSourceRecord>> TriggerSourceRecordFromChildIndex;

		// Token: 0x040007CB RID: 1995
		internal FrugalMap PropertyTriggersWithActions;

		// Token: 0x040007CC RID: 1996
		internal FrugalStructList<PropertyValue> PropertyValues;

		// Token: 0x040007CD RID: 1997
		internal FrugalStructList<ContainerDependent> ContainerDependents;

		// Token: 0x040007CE RID: 1998
		internal FrugalStructList<ChildPropertyDependent> ResourceDependents;

		// Token: 0x040007CF RID: 1999
		internal ItemStructList<ChildEventDependent> EventDependents = new ItemStructList<ChildEventDependent>(1);

		// Token: 0x040007D0 RID: 2000
		internal HybridDictionary _triggerActions;

		// Token: 0x040007D1 RID: 2001
		internal HybridDictionary _dataTriggerRecordFromBinding;

		// Token: 0x040007D2 RID: 2002
		internal HybridDictionary DataTriggersWithActions;

		// Token: 0x040007D3 RID: 2003
		private static int StyleInstanceCount = 0;

		// Token: 0x040007D4 RID: 2004
		internal static object Synchronized = new object();

		// Token: 0x040007D5 RID: 2005
		private const int TargetTypeID = 1;

		// Token: 0x040007D6 RID: 2006
		internal const int BasedOnID = 2;

		// Token: 0x040007D7 RID: 2007
		private const int HasEventSetter = 16;

		// Token: 0x040007D8 RID: 2008
		private int _modified;
	}
}
