using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Security;
using System.Windows.Baml2006;
using System.Windows.Data;
using System.Windows.Diagnostics;
using System.Windows.Markup;
using System.Windows.Navigation;
using System.Windows.Threading;
using System.Xaml;
using MS.Internal;
using MS.Internal.Xaml.Context;
using MS.Utility;

namespace System.Windows
{
	/// <summary>Enables the instantiation of a tree of <see cref="T:System.Windows.FrameworkElement" /> and/or <see cref="T:System.Windows.FrameworkContentElement" /> objects.</summary>
	// Token: 0x020000CB RID: 203
	[ContentProperty("VisualTree")]
	[Localizability(LocalizationCategory.NeverLocalize)]
	public abstract class FrameworkTemplate : DispatcherObject, INameScope, ISealable, IHaveResources, IQueryAmbient
	{
		/// <summary>When overridden in a derived class, supplies rules for the element this template is applied to.</summary>
		/// <param name="templatedParent">The element this template is applied to.</param>
		// Token: 0x060006CF RID: 1743 RVA: 0x00002137 File Offset: 0x00000337
		protected virtual void ValidateTemplatedParent(FrameworkElement templatedParent)
		{
		}

		/// <summary>Gets a value that indicates whether this object is in an immutable state so it cannot be changed.</summary>
		/// <returns>
		///     <see langword="true " />if this object is in an immutable state; otherwise, <see langword="false" />.</returns>
		// Token: 0x1700014F RID: 335
		// (get) Token: 0x060006D0 RID: 1744 RVA: 0x000158D5 File Offset: 0x00013AD5
		public bool IsSealed
		{
			get
			{
				base.VerifyAccess();
				return this._sealed;
			}
		}

		/// <summary>Gets or sets the root node of the template.</summary>
		/// <returns>The root node of the template.</returns>
		// Token: 0x17000150 RID: 336
		// (get) Token: 0x060006D1 RID: 1745 RVA: 0x000158E3 File Offset: 0x00013AE3
		// (set) Token: 0x060006D2 RID: 1746 RVA: 0x000158F1 File Offset: 0x00013AF1
		public FrameworkElementFactory VisualTree
		{
			get
			{
				base.VerifyAccess();
				return this._templateRoot;
			}
			set
			{
				base.VerifyAccess();
				this.CheckSealed();
				this.ValidateVisualTree(value);
				this._templateRoot = value;
			}
		}

		/// <summary>Returns a value that indicates whether serialization processes should serialize the value of the <see cref="P:System.Windows.FrameworkTemplate.VisualTree" /> property on instances of this class.</summary>
		/// <returns>
		///     <see langword="true" /> if the <see cref="P:System.Windows.FrameworkTemplate.VisualTree" /> property value should be serialized; otherwise, <see langword="false" />.</returns>
		// Token: 0x060006D3 RID: 1747 RVA: 0x0001590D File Offset: 0x00013B0D
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool ShouldSerializeVisualTree()
		{
			base.VerifyAccess();
			return this.HasContent || this.VisualTree != null;
		}

		/// <summary>Gets or sets a reference to the object that records or plays the XAML nodes for the template when the template is defined or applied by a writer.</summary>
		/// <returns>A reference to the object that records or plays the XAML nodes for the template.</returns>
		// Token: 0x17000151 RID: 337
		// (get) Token: 0x060006D4 RID: 1748 RVA: 0x00015928 File Offset: 0x00013B28
		// (set) Token: 0x060006D5 RID: 1749 RVA: 0x00015930 File Offset: 0x00013B30
		[Ambient]
		[DefaultValue(null)]
		public TemplateContent Template
		{
			get
			{
				return this._templateHolder;
			}
			set
			{
				this.CheckSealed();
				if (!this._hasXamlNodeContent)
				{
					value.OwnerTemplate = this;
					value.ParseXaml();
					this._templateHolder = value;
					this._hasXamlNodeContent = true;
					return;
				}
				throw new System.Windows.Markup.XamlParseException(SR.Get("TemplateContentSetTwice"));
			}
		}

		/// <summary>Gets or sets the collection of resources that can be used within the scope of this template.</summary>
		/// <returns>The resources that can be used within the scope of this template.</returns>
		// Token: 0x17000152 RID: 338
		// (get) Token: 0x060006D6 RID: 1750 RVA: 0x0001596C File Offset: 0x00013B6C
		// (set) Token: 0x060006D7 RID: 1751 RVA: 0x000159B8 File Offset: 0x00013BB8
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
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
				}
				if (this.IsSealed)
				{
					this._resources.IsReadOnly = true;
				}
				return this._resources;
			}
			set
			{
				base.VerifyAccess();
				if (this.IsSealed)
				{
					throw new InvalidOperationException(SR.Get("CannotChangeAfterSealed", new object[]
					{
						"Template"
					}));
				}
				this._resources = value;
				if (this._resources != null)
				{
					this._resources.CanBeAccessedAcrossThreads = true;
				}
			}
		}

		// Token: 0x17000153 RID: 339
		// (get) Token: 0x060006D8 RID: 1752 RVA: 0x00015A0C File Offset: 0x00013C0C
		// (set) Token: 0x060006D9 RID: 1753 RVA: 0x00015A14 File Offset: 0x00013C14
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

		// Token: 0x060006DA RID: 1754 RVA: 0x00015A20 File Offset: 0x00013C20
		internal object FindResource(object resourceKey, bool allowDeferredResourceReference, bool mustReturnDeferredResourceReference)
		{
			if (this._resources != null && this._resources.Contains(resourceKey))
			{
				bool flag;
				return this._resources.FetchResource(resourceKey, allowDeferredResourceReference, mustReturnDeferredResourceReference, out flag);
			}
			return DependencyProperty.UnsetValue;
		}

		/// <summary>Queries whether a specified ambient property is available in the current scope.</summary>
		/// <param name="propertyName">The name of the requested ambient property.</param>
		/// <returns>
		///     <see langword="true" /> if the requested ambient property is available; otherwise, <see langword="false" />.</returns>
		// Token: 0x060006DB RID: 1755 RVA: 0x00015A59 File Offset: 0x00013C59
		bool IQueryAmbient.IsAmbientPropertyAvailable(string propertyName)
		{
			return (!(propertyName == "Resources") || this._resources != null) && (!(propertyName == "Template") || this._hasXamlNodeContent);
		}

		/// <summary>Finds the element associated with the specified name defined within this template.</summary>
		/// <param name="name">The string name.</param>
		/// <param name="templatedParent">The context of the <see cref="T:System.Windows.FrameworkElement" /> where this template is applied.</param>
		/// <returns>The element associated with the specified name.</returns>
		// Token: 0x060006DC RID: 1756 RVA: 0x00015A8A File Offset: 0x00013C8A
		public object FindName(string name, FrameworkElement templatedParent)
		{
			base.VerifyAccess();
			if (templatedParent == null)
			{
				throw new ArgumentNullException("templatedParent");
			}
			if (this != templatedParent.TemplateInternal)
			{
				throw new InvalidOperationException(SR.Get("TemplateFindNameInInvalidElement"));
			}
			return StyleHelper.FindNameInTemplateContent(templatedParent, name, this);
		}

		/// <summary>Registers a new name/object pair into the current name scope.</summary>
		/// <param name="name">The name to register.</param>
		/// <param name="scopedElement">The object to be mapped to the provided name.</param>
		// Token: 0x060006DD RID: 1757 RVA: 0x00015AC1 File Offset: 0x00013CC1
		public void RegisterName(string name, object scopedElement)
		{
			base.VerifyAccess();
			this._nameScope.RegisterName(name, scopedElement);
		}

		/// <summary>Removes a name/object mapping from the XAML namescope.</summary>
		/// <param name="name">The name of the mapping to remove.</param>
		// Token: 0x060006DE RID: 1758 RVA: 0x00015AD6 File Offset: 0x00013CD6
		public void UnregisterName(string name)
		{
			base.VerifyAccess();
			this._nameScope.UnregisterName(name);
		}

		/// <summary>Returns an object that has the provided identifying name. </summary>
		/// <param name="name">The name identifier for the object being requested.</param>
		/// <returns>The object, if found. Returns <see langword="null" /> if no object of that name was found.</returns>
		// Token: 0x060006DF RID: 1759 RVA: 0x00015AEA File Offset: 0x00013CEA
		object INameScope.FindName(string name)
		{
			base.VerifyAccess();
			return this._nameScope.FindName(name);
		}

		// Token: 0x060006E0 RID: 1760 RVA: 0x00015B00 File Offset: 0x00013D00
		private void ValidateVisualTree(FrameworkElementFactory templateRoot)
		{
			if (templateRoot != null && typeof(FrameworkContentElement).IsAssignableFrom(templateRoot.Type))
			{
				throw new ArgumentException(SR.Get("VisualTreeRootIsFrameworkElement", new object[]
				{
					typeof(FrameworkElement).Name,
					templateRoot.Type.Name
				}));
			}
		}

		// Token: 0x060006E1 RID: 1761 RVA: 0x00002137 File Offset: 0x00000337
		internal virtual void ProcessTemplateBeforeSeal()
		{
		}

		/// <summary>Locks the template so it cannot be changed.</summary>
		// Token: 0x060006E2 RID: 1762 RVA: 0x00015B60 File Offset: 0x00013D60
		public void Seal()
		{
			base.VerifyAccess();
			StyleHelper.SealTemplate(this, ref this._sealed, this._templateRoot, this.TriggersInternal, this._resources, this.ChildIndexFromChildName, ref this.ChildRecordFromChildIndex, ref this.TriggerSourceRecordFromChildIndex, ref this.ContainerDependents, ref this.ResourceDependents, ref this.EventDependents, ref this._triggerActions, ref this._dataTriggerRecordFromBinding, ref this._hasInstanceValues, ref this._eventHandlersStore);
			if (this._templateHolder != null)
			{
				this._templateHolder.ResetTemplateLoadData();
			}
		}

		// Token: 0x060006E3 RID: 1763 RVA: 0x00015BE0 File Offset: 0x00013DE0
		internal void CheckSealed()
		{
			if (this._sealed)
			{
				throw new InvalidOperationException(SR.Get("CannotChangeAfterSealed", new object[]
				{
					"Template"
				}));
			}
		}

		// Token: 0x060006E4 RID: 1764 RVA: 0x00015C08 File Offset: 0x00013E08
		internal void SetResourceReferenceState()
		{
			StyleHelper.SortResourceDependents(ref this.ResourceDependents);
			for (int i = 0; i < this.ResourceDependents.Count; i++)
			{
				if (this.ResourceDependents[i].ChildIndex == 0)
				{
					this.WriteInternalFlag(FrameworkTemplate.InternalFlags.HasContainerResourceReferences, true);
				}
				else
				{
					this.WriteInternalFlag(FrameworkTemplate.InternalFlags.HasChildResourceReferences, true);
				}
			}
		}

		// Token: 0x060006E5 RID: 1765 RVA: 0x00015C60 File Offset: 0x00013E60
		internal bool ApplyTemplateContent(UncommonField<HybridDictionary[]> templateDataField, FrameworkElement container)
		{
			if (TraceDependencyProperty.IsEnabled)
			{
				TraceDependencyProperty.Trace(TraceEventType.Start, TraceDependencyProperty.ApplyTemplateContent, container, this);
			}
			this.ValidateTemplatedParent(container);
			bool result = StyleHelper.ApplyTemplateContent(templateDataField, container, this._templateRoot, this._lastChildIndex, this.ChildIndexFromChildName, this);
			if (TraceDependencyProperty.IsEnabled)
			{
				TraceDependencyProperty.Trace(TraceEventType.Stop, TraceDependencyProperty.ApplyTemplateContent, container, this);
			}
			return result;
		}

		/// <summary>Loads the content of the template as an instance of an object and returns the root element of the content.</summary>
		/// <returns>The root element of the content. Calling this multiple times returns separate instances.</returns>
		// Token: 0x060006E6 RID: 1766 RVA: 0x00015CC0 File Offset: 0x00013EC0
		public DependencyObject LoadContent()
		{
			base.VerifyAccess();
			if (this.VisualTree != null)
			{
				return this.VisualTree.InstantiateUnoptimizedTree().DO;
			}
			return this.LoadContent(null, null);
		}

		// Token: 0x060006E7 RID: 1767 RVA: 0x00015CF8 File Offset: 0x00013EF8
		internal DependencyObject LoadContent(DependencyObject container, List<DependencyObject> affectedChildren)
		{
			if (!this.HasContent)
			{
				return null;
			}
			object themeDictionaryLock = SystemResources.ThemeDictionaryLock;
			DependencyObject result;
			lock (themeDictionaryLock)
			{
				result = this.LoadOptimizedTemplateContent(container, null, this._styleConnector, affectedChildren, null);
			}
			return result;
		}

		// Token: 0x060006E8 RID: 1768 RVA: 0x00015D50 File Offset: 0x00013F50
		internal static bool IsNameScope(XamlType type)
		{
			return typeof(ResourceDictionary).IsAssignableFrom(type.UnderlyingType) || type.IsNameScope;
		}

		/// <summary>Gets a value that indicates whether this template has optimized content.</summary>
		/// <returns>
		///     <see langword="true" /> if this template has optimized content; otherwise, <see langword="false" />.</returns>
		// Token: 0x17000154 RID: 340
		// (get) Token: 0x060006E9 RID: 1769 RVA: 0x00015D71 File Offset: 0x00013F71
		public bool HasContent
		{
			get
			{
				base.VerifyAccess();
				return this._hasXamlNodeContent;
			}
		}

		// Token: 0x060006EA RID: 1770 RVA: 0x0000B02A File Offset: 0x0000922A
		internal virtual bool BuildVisualTree(FrameworkElement container)
		{
			return false;
		}

		// Token: 0x17000155 RID: 341
		// (get) Token: 0x060006EB RID: 1771 RVA: 0x00015D7F File Offset: 0x00013F7F
		// (set) Token: 0x060006EC RID: 1772 RVA: 0x00015D88 File Offset: 0x00013F88
		internal bool CanBuildVisualTree
		{
			get
			{
				return this.ReadInternalFlag(FrameworkTemplate.InternalFlags.CanBuildVisualTree);
			}
			set
			{
				this.WriteInternalFlag(FrameworkTemplate.InternalFlags.CanBuildVisualTree, value);
			}
		}

		/// <summary>Returns a value that indicates whether serialization processes should serialize the value of the <see cref="P:System.Windows.FrameworkTemplate.Resources" /> property on instances of this class.</summary>
		/// <param name="manager">The <see cref="T:System.Windows.Markup.XamlDesignerSerializationManager" />.</param>
		/// <returns>
		///     <see langword="true" /> if the <see cref="P:System.Windows.FrameworkTemplate.Resources" /> property value should be serialized; otherwise, <see langword="false" />.</returns>
		// Token: 0x060006ED RID: 1773 RVA: 0x00015D94 File Offset: 0x00013F94
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool ShouldSerializeResources(XamlDesignerSerializationManager manager)
		{
			base.VerifyAccess();
			bool result = true;
			if (manager != null)
			{
				result = (manager.XmlWriter == null);
			}
			return result;
		}

		// Token: 0x060006EE RID: 1774 RVA: 0x00015DB7 File Offset: 0x00013FB7
		private bool ReadInternalFlag(FrameworkTemplate.InternalFlags reqFlag)
		{
			return (this._flags & reqFlag) > (FrameworkTemplate.InternalFlags)0U;
		}

		// Token: 0x060006EF RID: 1775 RVA: 0x00015DC4 File Offset: 0x00013FC4
		private void WriteInternalFlag(FrameworkTemplate.InternalFlags reqFlag, bool set)
		{
			if (set)
			{
				this._flags |= reqFlag;
				return;
			}
			this._flags &= ~reqFlag;
		}

		// Token: 0x060006F0 RID: 1776 RVA: 0x00015DE8 File Offset: 0x00013FE8
		private bool ReceivePropertySet(object targetObject, XamlMember member, object value, DependencyObject templatedParent)
		{
			DependencyObject dependencyObject = targetObject as DependencyObject;
			WpfXamlMember wpfXamlMember = member as WpfXamlMember;
			if (!(wpfXamlMember != null))
			{
				return false;
			}
			DependencyProperty dependencyProperty = wpfXamlMember.DependencyProperty;
			if (dependencyProperty == null || dependencyObject == null)
			{
				return false;
			}
			FrameworkObject frameworkObject = new FrameworkObject(dependencyObject);
			if (frameworkObject.TemplatedParent == null || templatedParent == null)
			{
				return false;
			}
			if (dependencyProperty == BaseUriHelper.BaseUriProperty)
			{
				if (!frameworkObject.IsInitialized)
				{
					return true;
				}
			}
			else if (dependencyProperty == UIElement.UidProperty)
			{
				return true;
			}
			HybridDictionary hybridDictionary;
			if (!frameworkObject.StoresParentTemplateValues)
			{
				hybridDictionary = new HybridDictionary();
				StyleHelper.ParentTemplateValuesField.SetValue(dependencyObject, hybridDictionary);
				frameworkObject.StoresParentTemplateValues = true;
			}
			else
			{
				hybridDictionary = StyleHelper.ParentTemplateValuesField.GetValue(dependencyObject);
			}
			int templateChildIndex = frameworkObject.TemplateChildIndex;
			Expression expression;
			if ((expression = (value as Expression)) != null)
			{
				BindingExpressionBase bindingExpressionBase;
				TemplateBindingExpression templateBindingExpression;
				if ((bindingExpressionBase = (expression as BindingExpressionBase)) != null)
				{
					HybridDictionary instanceValues = StyleHelper.EnsureInstanceData(StyleHelper.TemplateDataField, templatedParent, InstanceStyleData.InstanceValues);
					StyleHelper.ProcessInstanceValue(dependencyObject, templateChildIndex, instanceValues, dependencyProperty, -1, true);
					value = bindingExpressionBase.ParentBindingBase;
				}
				else if ((templateBindingExpression = (expression as TemplateBindingExpression)) != null)
				{
					TemplateBindingExtension templateBindingExtension = templateBindingExpression.TemplateBindingExtension;
					HybridDictionary instanceValues2 = StyleHelper.EnsureInstanceData(StyleHelper.TemplateDataField, templatedParent, InstanceStyleData.InstanceValues);
					StyleHelper.ProcessInstanceValue(dependencyObject, templateChildIndex, instanceValues2, dependencyProperty, -1, true);
					value = new Binding
					{
						Mode = BindingMode.OneWay,
						RelativeSource = RelativeSource.TemplatedParent,
						Path = new PropertyPath(templateBindingExtension.Property),
						Converter = templateBindingExtension.Converter,
						ConverterParameter = templateBindingExtension.ConverterParameter
					};
				}
			}
			bool flag = value is MarkupExtension;
			if (!dependencyProperty.IsValidValue(value) && !flag && !(value is DeferredReference))
			{
				throw new ArgumentException(SR.Get("InvalidPropertyValue", new object[]
				{
					value,
					dependencyProperty.Name
				}));
			}
			hybridDictionary[dependencyProperty] = value;
			dependencyObject.ProvideSelfAsInheritanceContext(value, dependencyProperty);
			EffectiveValueEntry effectiveValueEntry = new EffectiveValueEntry(dependencyProperty);
			effectiveValueEntry.BaseValueSourceInternal = BaseValueSourceInternal.ParentTemplate;
			effectiveValueEntry.Value = value;
			if (flag)
			{
				StyleHelper.GetInstanceValue(StyleHelper.TemplateDataField, templatedParent, frameworkObject.FE, frameworkObject.FCE, templateChildIndex, dependencyProperty, -1, ref effectiveValueEntry);
			}
			dependencyObject.UpdateEffectiveValue(dependencyObject.LookupEntry(dependencyProperty.GlobalIndex), dependencyProperty, dependencyProperty.GetMetadata(dependencyObject.DependencyObjectType), default(EffectiveValueEntry), ref effectiveValueEntry, false, false, OperationType.Unknown);
			return true;
		}

		// Token: 0x060006F1 RID: 1777 RVA: 0x00016014 File Offset: 0x00014214
		private DependencyObject LoadOptimizedTemplateContent(DependencyObject container, IComponentConnector componentConnector, IStyleConnector styleConnector, List<DependencyObject> affectedChildren, UncommonField<Hashtable> templatedNonFeChildrenField)
		{
			if (this.Names == null)
			{
				this.Names = new XamlContextStack<FrameworkTemplate.Frame>(() => new FrameworkTemplate.Frame());
			}
			DependencyObject rootObject = null;
			if (TraceMarkup.IsEnabled)
			{
				TraceMarkup.Trace(TraceEventType.Start, TraceMarkup.Load);
			}
			FrameworkElement feContainer = container as FrameworkElement;
			bool flag = feContainer != null;
			TemplateNameScope nameScope = new TemplateNameScope(container, affectedChildren, this);
			XamlObjectWriterSettings xamlObjectWriterSettings = System.Windows.Markup.XamlReader.CreateObjectWriterSettings(this._templateHolder.ObjectWriterParentSettings);
			xamlObjectWriterSettings.ExternalNameScope = nameScope;
			xamlObjectWriterSettings.RegisterNamesOnExternalNamescope = true;
			IEnumerator<string> nameEnumerator = this.ChildNames.GetEnumerator();
			xamlObjectWriterSettings.AfterBeginInitHandler = delegate(object sender, XamlObjectEventArgs args)
			{
				this.HandleAfterBeginInit(args.Instance, ref rootObject, container, feContainer, nameScope, nameEnumerator);
				if (XamlSourceInfoHelper.IsXamlSourceInfoEnabled)
				{
					XamlSourceInfoHelper.SetXamlSourceInfo(args.Instance, args, null);
				}
			};
			xamlObjectWriterSettings.BeforePropertiesHandler = delegate(object sender, XamlObjectEventArgs args)
			{
				this.HandleBeforeProperties(args.Instance, ref rootObject, container, feContainer, nameScope);
			};
			xamlObjectWriterSettings.XamlSetValueHandler = delegate(object sender, XamlSetValueEventArgs setArgs)
			{
				setArgs.Handled = this.ReceivePropertySet(sender, setArgs.Member, setArgs.Value, container);
			};
			XamlObjectWriter xamlObjectWriter = this._templateHolder.ObjectWriterFactory.GetXamlObjectWriter(xamlObjectWriterSettings);
			try
			{
				this.LoadTemplateXaml(xamlObjectWriter);
			}
			finally
			{
				if (TraceMarkup.IsEnabled)
				{
					TraceMarkup.Trace(TraceEventType.Stop, TraceMarkup.Load, rootObject);
				}
			}
			return rootObject;
		}

		// Token: 0x060006F2 RID: 1778 RVA: 0x00016174 File Offset: 0x00014374
		[SecurityCritical]
		[SecurityTreatAsSafe]
		private void LoadTemplateXaml(XamlObjectWriter objectWriter)
		{
			System.Xaml.XamlReader templateReader = this._templateHolder.PlayXaml();
			if (this._templateHolder.LoadPermission != null)
			{
				this._templateHolder.LoadPermission.Assert();
				try
				{
					this.LoadTemplateXaml(templateReader, objectWriter);
					return;
				}
				finally
				{
					CodeAccessPermission.RevertAssert();
				}
			}
			this.LoadTemplateXaml(templateReader, objectWriter);
		}

		// Token: 0x060006F3 RID: 1779 RVA: 0x000161D4 File Offset: 0x000143D4
		private void LoadTemplateXaml(System.Xaml.XamlReader templateReader, XamlObjectWriter currentWriter)
		{
			try
			{
				int num = 0;
				IXamlLineInfoConsumer xamlLineInfoConsumer = null;
				IXamlLineInfo xamlLineInfo = null;
				if (XamlSourceInfoHelper.IsXamlSourceInfoEnabled)
				{
					xamlLineInfo = (templateReader as IXamlLineInfo);
					if (xamlLineInfo != null)
					{
						xamlLineInfoConsumer = currentWriter;
					}
				}
				while (templateReader.Read())
				{
					if (xamlLineInfoConsumer != null)
					{
						xamlLineInfoConsumer.SetLineInfo(xamlLineInfo.LineNumber, xamlLineInfo.LinePosition);
					}
					currentWriter.WriteNode(templateReader);
					switch (templateReader.NodeType)
					{
					case System.Xaml.XamlNodeType.StartObject:
					{
						bool flag = this.Names.Depth > 0 && (FrameworkTemplate.IsNameScope(this.Names.CurrentFrame.Type) || this.Names.CurrentFrame.InsideNameScope);
						this.Names.PushScope();
						this.Names.CurrentFrame.Type = templateReader.Type;
						if (flag)
						{
							this.Names.CurrentFrame.InsideNameScope = true;
						}
						break;
					}
					case System.Xaml.XamlNodeType.GetObject:
					{
						bool flag2 = FrameworkTemplate.IsNameScope(this.Names.CurrentFrame.Type) || this.Names.CurrentFrame.InsideNameScope;
						this.Names.PushScope();
						this.Names.CurrentFrame.Type = this.Names.PreviousFrame.Property.Type;
						if (flag2)
						{
							this.Names.CurrentFrame.InsideNameScope = true;
						}
						break;
					}
					case System.Xaml.XamlNodeType.EndObject:
						this.Names.PopScope();
						break;
					case System.Xaml.XamlNodeType.StartMember:
						this.Names.CurrentFrame.Property = templateReader.Member;
						if (templateReader.Member.DeferringLoader != null)
						{
							num++;
						}
						break;
					case System.Xaml.XamlNodeType.EndMember:
						if (this.Names.CurrentFrame.Property.DeferringLoader != null)
						{
							num--;
						}
						this.Names.CurrentFrame.Property = null;
						break;
					case System.Xaml.XamlNodeType.Value:
						if (num == 0 && this.Names.CurrentFrame.Property == XamlLanguage.ConnectionId && this._styleConnector != null)
						{
							this._styleConnector.Connect((int)templateReader.Value, this.Names.CurrentFrame.Instance);
						}
						break;
					}
				}
			}
			catch (Exception ex)
			{
				if (CriticalExceptions.IsCriticalException(ex) || ex is System.Windows.Markup.XamlParseException)
				{
					throw;
				}
				System.Windows.Markup.XamlReader.RewrapException(ex, null);
			}
		}

		// Token: 0x060006F4 RID: 1780 RVA: 0x00016454 File Offset: 0x00014654
		internal static bool IsNameProperty(XamlMember member, XamlType owner)
		{
			return member == owner.GetAliasedProperty(XamlLanguage.Name) || XamlLanguage.Name == member;
		}

		// Token: 0x060006F5 RID: 1781 RVA: 0x0001647C File Offset: 0x0001467C
		private void HandleAfterBeginInit(object createdObject, ref DependencyObject rootObject, DependencyObject container, FrameworkElement feContainer, TemplateNameScope nameScope, IEnumerator<string> nameEnumerator)
		{
			if (!this.Names.CurrentFrame.InsideNameScope && (createdObject is FrameworkElement || createdObject is FrameworkContentElement))
			{
				nameEnumerator.MoveNext();
				nameScope.RegisterNameInternal(nameEnumerator.Current, createdObject);
			}
			this.Names.CurrentFrame.Instance = createdObject;
		}

		// Token: 0x060006F6 RID: 1782 RVA: 0x000164D3 File Offset: 0x000146D3
		private void HandleBeforeProperties(object createdObject, ref DependencyObject rootObject, DependencyObject container, FrameworkElement feContainer, INameScope nameScope)
		{
			if (createdObject is FrameworkElement || createdObject is FrameworkContentElement)
			{
				if (rootObject == null)
				{
					rootObject = FrameworkTemplate.WireRootObjectToParent(createdObject, rootObject, container, feContainer, nameScope);
				}
				this.InvalidatePropertiesOnTemplate(container, createdObject);
			}
		}

		// Token: 0x060006F7 RID: 1783 RVA: 0x00016500 File Offset: 0x00014700
		private static DependencyObject WireRootObjectToParent(object createdObject, DependencyObject rootObject, DependencyObject container, FrameworkElement feContainer, INameScope nameScope)
		{
			rootObject = (createdObject as DependencyObject);
			if (rootObject != null)
			{
				if (feContainer != null)
				{
					UIElement uielement = rootObject as UIElement;
					if (uielement == null)
					{
						throw new InvalidOperationException(SR.Get("TemplateMustBeFE", new object[]
						{
							rootObject.GetType().FullName
						}));
					}
					feContainer.TemplateChild = uielement;
				}
				else if (container != null)
				{
					FrameworkElement frameworkElement;
					FrameworkContentElement treeNodeFCE;
					Helper.DowncastToFEorFCE(rootObject, out frameworkElement, out treeNodeFCE, true);
					FrameworkElementFactory.AddNodeToLogicalTree((FrameworkContentElement)container, rootObject.GetType(), frameworkElement != null, frameworkElement, treeNodeFCE);
				}
				if (NameScope.GetNameScope(rootObject) == null)
				{
					NameScope.SetNameScope(rootObject, nameScope);
				}
			}
			return rootObject;
		}

		// Token: 0x060006F8 RID: 1784 RVA: 0x00016588 File Offset: 0x00014788
		private void InvalidatePropertiesOnTemplate(DependencyObject container, object currentObject)
		{
			if (container != null)
			{
				DependencyObject dependencyObject = currentObject as DependencyObject;
				if (dependencyObject != null)
				{
					FrameworkObject child = new FrameworkObject(dependencyObject);
					if (child.IsValid)
					{
						int templateChildIndex = child.TemplateChildIndex;
						if (StyleHelper.HasResourceDependentsForChild(templateChildIndex, ref this.ResourceDependents))
						{
							child.HasResourceReference = true;
						}
						StyleHelper.InvalidatePropertiesOnTemplateNode(container, child, templateChildIndex, ref this.ChildRecordFromChildIndex, false, this.VisualTree);
					}
				}
			}
		}

		// Token: 0x060006F9 RID: 1785 RVA: 0x000165E8 File Offset: 0x000147E8
		internal static void SetTemplateParentValues(string name, object element, FrameworkTemplate frameworkTemplate, ref ProvideValueServiceProvider provideValueServiceProvider)
		{
			if (!frameworkTemplate.IsSealed)
			{
				frameworkTemplate.Seal();
			}
			HybridDictionary childIndexFromChildName = frameworkTemplate.ChildIndexFromChildName;
			FrugalStructList<ChildRecord> childRecordFromChildIndex = frameworkTemplate.ChildRecordFromChildIndex;
			int num = StyleHelper.QueryChildIndexFromChildName(name, childIndexFromChildName);
			if (num < childRecordFromChildIndex.Count)
			{
				ChildRecord childRecord = childRecordFromChildIndex[num];
				for (int i = 0; i < childRecord.ValueLookupListFromProperty.Count; i++)
				{
					for (int j = 0; j < childRecord.ValueLookupListFromProperty.Entries[i].Value.Count; j++)
					{
						ChildValueLookup childValueLookup = childRecord.ValueLookupListFromProperty.Entries[i].Value.List[j];
						if (childValueLookup.LookupType == ValueLookupType.Simple || childValueLookup.LookupType == ValueLookupType.Resource || childValueLookup.LookupType == ValueLookupType.TemplateBinding)
						{
							object obj = childValueLookup.Value;
							if (childValueLookup.LookupType == ValueLookupType.TemplateBinding)
							{
								obj = new TemplateBindingExpression(obj as TemplateBindingExtension);
							}
							else if (childValueLookup.LookupType == ValueLookupType.Resource)
							{
								obj = new ResourceReferenceExpression(obj);
							}
							MarkupExtension markupExtension = obj as MarkupExtension;
							if (markupExtension != null)
							{
								if (provideValueServiceProvider == null)
								{
									provideValueServiceProvider = new ProvideValueServiceProvider();
								}
								provideValueServiceProvider.SetData(element, childValueLookup.Property);
								obj = markupExtension.ProvideValue(provideValueServiceProvider);
								provideValueServiceProvider.ClearData();
							}
							(element as DependencyObject).SetValue(childValueLookup.Property, obj);
						}
					}
				}
			}
		}

		// Token: 0x17000156 RID: 342
		// (get) Token: 0x060006FA RID: 1786 RVA: 0x0000C238 File Offset: 0x0000A438
		internal virtual Type TargetTypeInternal
		{
			get
			{
				return null;
			}
		}

		// Token: 0x060006FB RID: 1787
		internal abstract void SetTargetTypeInternal(Type targetType);

		// Token: 0x17000157 RID: 343
		// (get) Token: 0x060006FC RID: 1788 RVA: 0x0000C238 File Offset: 0x0000A438
		internal virtual object DataTypeInternal
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000158 RID: 344
		// (get) Token: 0x060006FD RID: 1789 RVA: 0x00016748 File Offset: 0x00014948
		bool ISealable.CanSeal
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000159 RID: 345
		// (get) Token: 0x060006FE RID: 1790 RVA: 0x0001674B File Offset: 0x0001494B
		bool ISealable.IsSealed
		{
			get
			{
				return this.IsSealed;
			}
		}

		// Token: 0x060006FF RID: 1791 RVA: 0x00016753 File Offset: 0x00014953
		void ISealable.Seal()
		{
			this.Seal();
		}

		// Token: 0x1700015A RID: 346
		// (get) Token: 0x06000700 RID: 1792 RVA: 0x0000C238 File Offset: 0x0000A438
		internal virtual TriggerCollection TriggersInternal
		{
			get
			{
				return null;
			}
		}

		// Token: 0x1700015B RID: 347
		// (get) Token: 0x06000701 RID: 1793 RVA: 0x0001675B File Offset: 0x0001495B
		internal bool HasResourceReferences
		{
			get
			{
				return this.ResourceDependents.Count > 0;
			}
		}

		// Token: 0x1700015C RID: 348
		// (get) Token: 0x06000702 RID: 1794 RVA: 0x0001676B File Offset: 0x0001496B
		internal bool HasContainerResourceReferences
		{
			get
			{
				return this.ReadInternalFlag(FrameworkTemplate.InternalFlags.HasContainerResourceReferences);
			}
		}

		// Token: 0x1700015D RID: 349
		// (get) Token: 0x06000703 RID: 1795 RVA: 0x00016775 File Offset: 0x00014975
		internal bool HasChildResourceReferences
		{
			get
			{
				return this.ReadInternalFlag(FrameworkTemplate.InternalFlags.HasChildResourceReferences);
			}
		}

		// Token: 0x1700015E RID: 350
		// (get) Token: 0x06000704 RID: 1796 RVA: 0x0001677F File Offset: 0x0001497F
		internal bool HasEventDependents
		{
			get
			{
				return this.EventDependents.Count > 0;
			}
		}

		// Token: 0x1700015F RID: 351
		// (get) Token: 0x06000705 RID: 1797 RVA: 0x0001678F File Offset: 0x0001498F
		internal bool HasInstanceValues
		{
			get
			{
				return this._hasInstanceValues;
			}
		}

		// Token: 0x17000160 RID: 352
		// (get) Token: 0x06000706 RID: 1798 RVA: 0x00016797 File Offset: 0x00014997
		// (set) Token: 0x06000707 RID: 1799 RVA: 0x000167A0 File Offset: 0x000149A0
		internal bool HasLoadedChangeHandler
		{
			get
			{
				return this.ReadInternalFlag(FrameworkTemplate.InternalFlags.HasLoadedChangeHandler);
			}
			set
			{
				this.WriteInternalFlag(FrameworkTemplate.InternalFlags.HasLoadedChangeHandler, value);
			}
		}

		// Token: 0x06000708 RID: 1800 RVA: 0x000167AA File Offset: 0x000149AA
		internal void CopyParserContext(ParserContext parserContext)
		{
			this._parserContext = parserContext.ScopedCopy(false);
			this._parserContext.SkipJournaledProperties = false;
		}

		// Token: 0x17000161 RID: 353
		// (get) Token: 0x06000709 RID: 1801 RVA: 0x000167C5 File Offset: 0x000149C5
		internal ParserContext ParserContext
		{
			get
			{
				return this._parserContext;
			}
		}

		// Token: 0x17000162 RID: 354
		// (get) Token: 0x0600070A RID: 1802 RVA: 0x000167CD File Offset: 0x000149CD
		internal EventHandlersStore EventHandlersStore
		{
			get
			{
				return this._eventHandlersStore;
			}
		}

		// Token: 0x17000163 RID: 355
		// (get) Token: 0x0600070B RID: 1803 RVA: 0x000167D5 File Offset: 0x000149D5
		// (set) Token: 0x0600070C RID: 1804 RVA: 0x000167DD File Offset: 0x000149DD
		internal IStyleConnector StyleConnector
		{
			get
			{
				return this._styleConnector;
			}
			set
			{
				this._styleConnector = value;
			}
		}

		// Token: 0x17000164 RID: 356
		// (get) Token: 0x0600070D RID: 1805 RVA: 0x000167E6 File Offset: 0x000149E6
		// (set) Token: 0x0600070E RID: 1806 RVA: 0x000167EE File Offset: 0x000149EE
		internal IComponentConnector ComponentConnector
		{
			get
			{
				return this._componentConnector;
			}
			set
			{
				this._componentConnector = value;
			}
		}

		// Token: 0x17000165 RID: 357
		// (get) Token: 0x0600070F RID: 1807 RVA: 0x000167F7 File Offset: 0x000149F7
		// (set) Token: 0x06000710 RID: 1808 RVA: 0x000167FF File Offset: 0x000149FF
		internal object[] StaticResourceValues
		{
			get
			{
				return this._staticResourceValues;
			}
			set
			{
				this._staticResourceValues = value;
			}
		}

		// Token: 0x17000166 RID: 358
		// (get) Token: 0x06000711 RID: 1809 RVA: 0x00016808 File Offset: 0x00014A08
		internal bool HasXamlNodeContent
		{
			get
			{
				return this._hasXamlNodeContent;
			}
		}

		// Token: 0x17000167 RID: 359
		// (get) Token: 0x06000712 RID: 1810 RVA: 0x00016810 File Offset: 0x00014A10
		internal HybridDictionary ChildIndexFromChildName
		{
			get
			{
				return this._childIndexFromChildName;
			}
		}

		// Token: 0x17000168 RID: 360
		// (get) Token: 0x06000713 RID: 1811 RVA: 0x00016818 File Offset: 0x00014A18
		internal Dictionary<int, Type> ChildTypeFromChildIndex
		{
			get
			{
				return this._childTypeFromChildIndex;
			}
		}

		// Token: 0x17000169 RID: 361
		// (get) Token: 0x06000714 RID: 1812 RVA: 0x00016820 File Offset: 0x00014A20
		// (set) Token: 0x06000715 RID: 1813 RVA: 0x00016828 File Offset: 0x00014A28
		internal int LastChildIndex
		{
			get
			{
				return this._lastChildIndex;
			}
			set
			{
				this._lastChildIndex = value;
			}
		}

		// Token: 0x1700016A RID: 362
		// (get) Token: 0x06000716 RID: 1814 RVA: 0x00016831 File Offset: 0x00014A31
		internal List<string> ChildNames
		{
			get
			{
				return this._childNames;
			}
		}

		// Token: 0x040006F6 RID: 1782
		private NameScope _nameScope = new NameScope();

		// Token: 0x040006F7 RID: 1783
		private XamlContextStack<FrameworkTemplate.Frame> Names;

		// Token: 0x040006F8 RID: 1784
		private FrameworkTemplate.InternalFlags _flags;

		// Token: 0x040006F9 RID: 1785
		private bool _sealed;

		// Token: 0x040006FA RID: 1786
		internal bool _hasInstanceValues;

		// Token: 0x040006FB RID: 1787
		private ParserContext _parserContext;

		// Token: 0x040006FC RID: 1788
		private IStyleConnector _styleConnector;

		// Token: 0x040006FD RID: 1789
		private IComponentConnector _componentConnector;

		// Token: 0x040006FE RID: 1790
		private FrameworkElementFactory _templateRoot;

		// Token: 0x040006FF RID: 1791
		private TemplateContent _templateHolder;

		// Token: 0x04000700 RID: 1792
		private bool _hasXamlNodeContent;

		// Token: 0x04000701 RID: 1793
		private HybridDictionary _childIndexFromChildName = new HybridDictionary();

		// Token: 0x04000702 RID: 1794
		private Dictionary<int, Type> _childTypeFromChildIndex = new Dictionary<int, Type>();

		// Token: 0x04000703 RID: 1795
		private int _lastChildIndex = 1;

		// Token: 0x04000704 RID: 1796
		private List<string> _childNames = new List<string>();

		// Token: 0x04000705 RID: 1797
		internal ResourceDictionary _resources;

		// Token: 0x04000706 RID: 1798
		internal HybridDictionary _triggerActions;

		// Token: 0x04000707 RID: 1799
		internal FrugalStructList<ChildRecord> ChildRecordFromChildIndex;

		// Token: 0x04000708 RID: 1800
		internal FrugalStructList<ItemStructMap<TriggerSourceRecord>> TriggerSourceRecordFromChildIndex;

		// Token: 0x04000709 RID: 1801
		internal FrugalMap PropertyTriggersWithActions;

		// Token: 0x0400070A RID: 1802
		internal FrugalStructList<ContainerDependent> ContainerDependents;

		// Token: 0x0400070B RID: 1803
		internal FrugalStructList<ChildPropertyDependent> ResourceDependents;

		// Token: 0x0400070C RID: 1804
		internal HybridDictionary _dataTriggerRecordFromBinding;

		// Token: 0x0400070D RID: 1805
		internal HybridDictionary DataTriggersWithActions;

		// Token: 0x0400070E RID: 1806
		internal ConditionalWeakTable<DependencyObject, List<DeferredAction>> DeferredActions;

		// Token: 0x0400070F RID: 1807
		internal HybridDictionary _TemplateChildLoadedDictionary = new HybridDictionary();

		// Token: 0x04000710 RID: 1808
		internal ItemStructList<ChildEventDependent> EventDependents = new ItemStructList<ChildEventDependent>(1);

		// Token: 0x04000711 RID: 1809
		private EventHandlersStore _eventHandlersStore;

		// Token: 0x04000712 RID: 1810
		private object[] _staticResourceValues;

		// Token: 0x02000819 RID: 2073
		private class Frame : XamlFrame
		{
			// Token: 0x17001D4C RID: 7500
			// (get) Token: 0x06007E42 RID: 32322 RVA: 0x0023584C File Offset: 0x00233A4C
			// (set) Token: 0x06007E43 RID: 32323 RVA: 0x00235854 File Offset: 0x00233A54
			public XamlType Type { get; set; }

			// Token: 0x17001D4D RID: 7501
			// (get) Token: 0x06007E44 RID: 32324 RVA: 0x0023585D File Offset: 0x00233A5D
			// (set) Token: 0x06007E45 RID: 32325 RVA: 0x00235865 File Offset: 0x00233A65
			public XamlMember Property { get; set; }

			// Token: 0x17001D4E RID: 7502
			// (get) Token: 0x06007E46 RID: 32326 RVA: 0x0023586E File Offset: 0x00233A6E
			// (set) Token: 0x06007E47 RID: 32327 RVA: 0x00235876 File Offset: 0x00233A76
			public bool InsideNameScope { get; set; }

			// Token: 0x17001D4F RID: 7503
			// (get) Token: 0x06007E48 RID: 32328 RVA: 0x0023587F File Offset: 0x00233A7F
			// (set) Token: 0x06007E49 RID: 32329 RVA: 0x00235887 File Offset: 0x00233A87
			public object Instance { get; set; }

			// Token: 0x06007E4A RID: 32330 RVA: 0x00235890 File Offset: 0x00233A90
			public override void Reset()
			{
				this.Type = null;
				this.Property = null;
				this.Instance = null;
				if (this.InsideNameScope)
				{
					this.InsideNameScope = false;
				}
			}
		}

		// Token: 0x0200081A RID: 2074
		internal class TemplateChildLoadedFlags
		{
			// Token: 0x04003BBE RID: 15294
			public bool HasLoadedChangedHandler;

			// Token: 0x04003BBF RID: 15295
			public bool HasUnloadedChangedHandler;
		}

		// Token: 0x0200081B RID: 2075
		[Flags]
		private enum InternalFlags : uint
		{
			// Token: 0x04003BC1 RID: 15297
			CanBuildVisualTree = 4U,
			// Token: 0x04003BC2 RID: 15298
			HasLoadedChangeHandler = 8U,
			// Token: 0x04003BC3 RID: 15299
			HasContainerResourceReferences = 16U,
			// Token: 0x04003BC4 RID: 15300
			HasChildResourceReferences = 32U
		}
	}
}
