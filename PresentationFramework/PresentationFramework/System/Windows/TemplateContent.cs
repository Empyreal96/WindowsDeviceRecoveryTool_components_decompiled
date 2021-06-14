using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Globalization;
using System.Security;
using System.Windows.Baml2006;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Diagnostics;
using System.Windows.Markup;
using System.Xaml;
using System.Xaml.Permissions;
using System.Xaml.Schema;
using MS.Internal.Xaml.Context;
using MS.Utility;

namespace System.Windows
{
	/// <summary>Implements the record and playback logic that templates use for deferring content when they interact with XAML readers and writers.</summary>
	// Token: 0x0200011F RID: 287
	[XamlDeferLoad(typeof(TemplateContentLoader), typeof(FrameworkElement))]
	public class TemplateContent
	{
		// Token: 0x06000BD7 RID: 3031 RVA: 0x0002B400 File Offset: 0x00029600
		[SecurityCritical]
		[SecurityTreatAsSafe]
		internal TemplateContent(System.Xaml.XamlReader xamlReader, IXamlObjectWriterFactory factory, IServiceProvider context)
		{
			this.TemplateLoadData = new TemplateLoadData();
			this.ObjectWriterFactory = factory;
			this.SchemaContext = xamlReader.SchemaContext;
			this.ObjectWriterParentSettings = factory.GetParentSettings();
			XamlAccessLevel accessLevel = this.ObjectWriterParentSettings.AccessLevel;
			if (accessLevel != null)
			{
				XamlLoadPermission xamlLoadPermission = new XamlLoadPermission(accessLevel);
				xamlLoadPermission.Demand();
				this.LoadPermission = xamlLoadPermission;
			}
			this.TemplateLoadData.Reader = xamlReader;
			this.Initialize(context);
		}

		// Token: 0x06000BD8 RID: 3032 RVA: 0x0002B474 File Offset: 0x00029674
		private void Initialize(IServiceProvider context)
		{
			XamlObjectWriterSettings xamlObjectWriterSettings = System.Windows.Markup.XamlReader.CreateObjectWriterSettings(this.ObjectWriterParentSettings);
			xamlObjectWriterSettings.AfterBeginInitHandler = delegate(object sender, XamlObjectEventArgs args)
			{
				if (this.Stack != null && this.Stack.Depth > 0)
				{
					this.Stack.CurrentFrame.Instance = args.Instance;
				}
			};
			xamlObjectWriterSettings.SkipProvideValueOnRoot = true;
			this.TemplateLoadData.ObjectWriter = this.ObjectWriterFactory.GetXamlObjectWriter(xamlObjectWriterSettings);
			this.TemplateLoadData.ServiceProviderWrapper = new TemplateContent.ServiceProviderWrapper(context, this.SchemaContext);
			IRootObjectProvider rootObjectProvider = context.GetService(typeof(IRootObjectProvider)) as IRootObjectProvider;
			if (rootObjectProvider != null)
			{
				this.TemplateLoadData.RootObject = rootObjectProvider.RootObject;
			}
		}

		// Token: 0x06000BD9 RID: 3033 RVA: 0x0002B500 File Offset: 0x00029700
		internal void ParseXaml()
		{
			TemplateContent.StackOfFrames stackOfFrames = new TemplateContent.StackOfFrames();
			this.TemplateLoadData.ServiceProviderWrapper.Frames = stackOfFrames;
			this.OwnerTemplate.StyleConnector = (this.TemplateLoadData.RootObject as IStyleConnector);
			this.TemplateLoadData.RootObject = null;
			List<PropertyValue> list = new List<PropertyValue>();
			int num = 1;
			this.ParseTree(stackOfFrames, list, ref num);
			if (this.OwnerTemplate is ItemsPanelTemplate)
			{
				list.Add(new PropertyValue
				{
					ValueType = PropertyValueType.Set,
					ChildName = this.TemplateLoadData.RootName,
					ValueInternal = true,
					Property = Panel.IsItemsHostProperty
				});
			}
			for (int i = 0; i < list.Count; i++)
			{
				PropertyValue propertyValue = list[i];
				if (propertyValue.ValueInternal is TemplateBindingExtension)
				{
					propertyValue.ValueType = PropertyValueType.TemplateBinding;
				}
				else if (propertyValue.ValueInternal is DynamicResourceExtension)
				{
					DynamicResourceExtension dynamicResourceExtension = propertyValue.Value as DynamicResourceExtension;
					propertyValue.ValueType = PropertyValueType.Resource;
					propertyValue.ValueInternal = dynamicResourceExtension.ResourceKey;
				}
				else
				{
					StyleHelper.SealIfSealable(propertyValue.ValueInternal);
				}
				StyleHelper.UpdateTables(ref propertyValue, ref this.OwnerTemplate.ChildRecordFromChildIndex, ref this.OwnerTemplate.TriggerSourceRecordFromChildIndex, ref this.OwnerTemplate.ResourceDependents, ref this.OwnerTemplate._dataTriggerRecordFromBinding, this.OwnerTemplate.ChildIndexFromChildName, ref this.OwnerTemplate._hasInstanceValues);
			}
			this.TemplateLoadData.ObjectWriter = null;
		}

		// Token: 0x06000BDA RID: 3034 RVA: 0x0002B67E File Offset: 0x0002987E
		[SecurityCritical]
		[SecurityTreatAsSafe]
		internal System.Xaml.XamlReader PlayXaml()
		{
			return this._xamlNodeList.GetReader();
		}

		// Token: 0x170003D5 RID: 981
		// (get) Token: 0x06000BDB RID: 3035 RVA: 0x0002B68B File Offset: 0x0002988B
		// (set) Token: 0x06000BDC RID: 3036 RVA: 0x0002B693 File Offset: 0x00029893
		internal XamlLoadPermission LoadPermission { [SecurityCritical] get; [SecurityCritical] set; }

		// Token: 0x06000BDD RID: 3037 RVA: 0x0002B69C File Offset: 0x0002989C
		[SecurityCritical]
		[SecurityTreatAsSafe]
		internal void ResetTemplateLoadData()
		{
			this.TemplateLoadData = null;
		}

		// Token: 0x06000BDE RID: 3038 RVA: 0x0002B6A8 File Offset: 0x000298A8
		private void UpdateSharedPropertyNames(string name, List<PropertyValue> sharedProperties, XamlType type)
		{
			int key = StyleHelper.CreateChildIndexFromChildName(name, this.OwnerTemplate);
			this.OwnerTemplate.ChildNames.Add(name);
			this.OwnerTemplate.ChildTypeFromChildIndex.Add(key, type.UnderlyingType);
			for (int i = sharedProperties.Count - 1; i >= 0; i--)
			{
				PropertyValue propertyValue = sharedProperties[i];
				if (propertyValue.ChildName != null)
				{
					break;
				}
				propertyValue.ChildName = name;
				sharedProperties[i] = propertyValue;
			}
		}

		// Token: 0x06000BDF RID: 3039 RVA: 0x0002B720 File Offset: 0x00029920
		[SecurityCritical]
		[SecurityTreatAsSafe]
		private void ParseTree(TemplateContent.StackOfFrames stack, List<PropertyValue> sharedProperties, ref int nameNumber)
		{
			if (this.LoadPermission != null)
			{
				this.LoadPermission.Assert();
				try
				{
					this.ParseNodes(stack, sharedProperties, ref nameNumber);
					return;
				}
				finally
				{
					CodeAccessPermission.RevertAssert();
				}
			}
			this.ParseNodes(stack, sharedProperties, ref nameNumber);
		}

		// Token: 0x06000BE0 RID: 3040 RVA: 0x0002B76C File Offset: 0x0002996C
		[SecurityCritical]
		[SecurityTreatAsSafe]
		private void ParseNodes(TemplateContent.StackOfFrames stack, List<PropertyValue> sharedProperties, ref int nameNumber)
		{
			this._xamlNodeList = new XamlNodeList(this.SchemaContext);
			System.Xaml.XamlWriter writer = this._xamlNodeList.Writer;
			System.Xaml.XamlReader reader = this.TemplateLoadData.Reader;
			IXamlLineInfoConsumer xamlLineInfoConsumer = null;
			IXamlLineInfo xamlLineInfo = null;
			if (XamlSourceInfoHelper.IsXamlSourceInfoEnabled)
			{
				xamlLineInfo = (reader as IXamlLineInfo);
				if (xamlLineInfo != null)
				{
					xamlLineInfoConsumer = (writer as IXamlLineInfoConsumer);
				}
			}
			while (reader.Read())
			{
				if (xamlLineInfoConsumer != null)
				{
					xamlLineInfoConsumer.SetLineInfo(xamlLineInfo.LineNumber, xamlLineInfo.LinePosition);
				}
				object obj;
				bool flag = this.ParseNode(reader, stack, sharedProperties, ref nameNumber, out obj);
				if (flag)
				{
					if (obj == DependencyProperty.UnsetValue)
					{
						writer.WriteNode(reader);
					}
					else
					{
						writer.WriteValue(obj);
					}
				}
			}
			writer.Close();
			this.TemplateLoadData.Reader = null;
		}

		// Token: 0x06000BE1 RID: 3041 RVA: 0x0002B81C File Offset: 0x00029A1C
		private bool ParseNode(System.Xaml.XamlReader xamlReader, TemplateContent.StackOfFrames stack, List<PropertyValue> sharedProperties, ref int nameNumber, out object newValue)
		{
			newValue = DependencyProperty.UnsetValue;
			switch (xamlReader.NodeType)
			{
			case System.Xaml.XamlNodeType.StartObject:
				if (xamlReader.Type.UnderlyingType == typeof(StaticResourceExtension))
				{
					XamlObjectWriter objectWriter = this.TemplateLoadData.ObjectWriter;
					objectWriter.Clear();
					this.WriteNamespaces(objectWriter, stack.InScopeNamespaces, null);
					newValue = this.LoadTimeBindUnshareableStaticResource(xamlReader, objectWriter);
					return true;
				}
				if (stack.Depth > 0 && !stack.CurrentFrame.NameSet && stack.CurrentFrame.Type != null && !stack.CurrentFrame.IsInNameScope && !stack.CurrentFrame.IsInStyleOrTemplate)
				{
					if (typeof(FrameworkElement).IsAssignableFrom(stack.CurrentFrame.Type.UnderlyingType) || typeof(FrameworkContentElement).IsAssignableFrom(stack.CurrentFrame.Type.UnderlyingType))
					{
						int num = nameNumber;
						nameNumber = num + 1;
						string name = num.ToString(CultureInfo.InvariantCulture) + "_T";
						this.UpdateSharedPropertyNames(name, sharedProperties, stack.CurrentFrame.Type);
						stack.CurrentFrame.Name = name;
					}
					stack.CurrentFrame.NameSet = true;
				}
				if (this.RootType == null)
				{
					this.RootType = xamlReader.Type;
				}
				stack.Push(xamlReader.Type, null);
				break;
			case System.Xaml.XamlNodeType.GetObject:
			{
				if (stack.Depth > 0 && !stack.CurrentFrame.NameSet && stack.CurrentFrame.Type != null && !stack.CurrentFrame.IsInNameScope && !stack.CurrentFrame.IsInStyleOrTemplate)
				{
					if (typeof(FrameworkElement).IsAssignableFrom(stack.CurrentFrame.Type.UnderlyingType) || typeof(FrameworkContentElement).IsAssignableFrom(stack.CurrentFrame.Type.UnderlyingType))
					{
						int num = nameNumber;
						nameNumber = num + 1;
						string name2 = num.ToString(CultureInfo.InvariantCulture) + "_T";
						this.UpdateSharedPropertyNames(name2, sharedProperties, stack.CurrentFrame.Type);
						stack.CurrentFrame.Name = name2;
					}
					stack.CurrentFrame.NameSet = true;
				}
				XamlType type = stack.CurrentFrame.Property.Type;
				if (this.RootType == null)
				{
					this.RootType = type;
				}
				stack.Push(type, null);
				break;
			}
			case System.Xaml.XamlNodeType.EndObject:
				if (!stack.CurrentFrame.IsInStyleOrTemplate)
				{
					if (!stack.CurrentFrame.NameSet && !stack.CurrentFrame.IsInNameScope)
					{
						if (typeof(FrameworkElement).IsAssignableFrom(stack.CurrentFrame.Type.UnderlyingType) || typeof(FrameworkContentElement).IsAssignableFrom(stack.CurrentFrame.Type.UnderlyingType))
						{
							int num = nameNumber;
							nameNumber = num + 1;
							string name3 = num.ToString(CultureInfo.InvariantCulture) + "_T";
							this.UpdateSharedPropertyNames(name3, sharedProperties, stack.CurrentFrame.Type);
							stack.CurrentFrame.Name = name3;
						}
						stack.CurrentFrame.NameSet = true;
					}
					if (this.TemplateLoadData.RootName == null && stack.Depth == 1)
					{
						this.TemplateLoadData.RootName = stack.CurrentFrame.Name;
					}
					if (typeof(ContentPresenter).IsAssignableFrom(stack.CurrentFrame.Type.UnderlyingType))
					{
						TemplateContent.AutoAliasContentPresenter(this.OwnerTemplate.TargetTypeInternal, stack.CurrentFrame.ContentSource, stack.CurrentFrame.Name, ref this.OwnerTemplate.ChildRecordFromChildIndex, ref this.OwnerTemplate.TriggerSourceRecordFromChildIndex, ref this.OwnerTemplate.ResourceDependents, ref this.OwnerTemplate._dataTriggerRecordFromBinding, ref this.OwnerTemplate._hasInstanceValues, this.OwnerTemplate.ChildIndexFromChildName, stack.CurrentFrame.ContentSet, stack.CurrentFrame.ContentSourceSet, stack.CurrentFrame.ContentTemplateSet, stack.CurrentFrame.ContentTemplateSelectorSet, stack.CurrentFrame.ContentStringFormatSet);
					}
					if (typeof(GridViewRowPresenter).IsAssignableFrom(stack.CurrentFrame.Type.UnderlyingType))
					{
						TemplateContent.AutoAliasGridViewRowPresenter(this.OwnerTemplate.TargetTypeInternal, stack.CurrentFrame.ContentSource, stack.CurrentFrame.Name, ref this.OwnerTemplate.ChildRecordFromChildIndex, ref this.OwnerTemplate.TriggerSourceRecordFromChildIndex, ref this.OwnerTemplate.ResourceDependents, ref this.OwnerTemplate._dataTriggerRecordFromBinding, ref this.OwnerTemplate._hasInstanceValues, this.OwnerTemplate.ChildIndexFromChildName, stack.CurrentFrame.ContentSet, stack.CurrentFrame.ColumnsSet);
					}
				}
				stack.PopScope();
				break;
			case System.Xaml.XamlNodeType.StartMember:
				stack.CurrentFrame.Property = xamlReader.Member;
				if (!stack.CurrentFrame.IsInStyleOrTemplate)
				{
					if (typeof(GridViewRowPresenter).IsAssignableFrom(stack.CurrentFrame.Type.UnderlyingType))
					{
						if (xamlReader.Member.Name == "Content")
						{
							stack.CurrentFrame.ContentSet = true;
						}
						else if (xamlReader.Member.Name == "Columns")
						{
							stack.CurrentFrame.ColumnsSet = true;
						}
					}
					else if (typeof(ContentPresenter).IsAssignableFrom(stack.CurrentFrame.Type.UnderlyingType))
					{
						if (xamlReader.Member.Name == "Content")
						{
							stack.CurrentFrame.ContentSet = true;
						}
						else if (xamlReader.Member.Name == "ContentTemplate")
						{
							stack.CurrentFrame.ContentTemplateSet = true;
						}
						else if (xamlReader.Member.Name == "ContentTemplateSelector")
						{
							stack.CurrentFrame.ContentTemplateSelectorSet = true;
						}
						else if (xamlReader.Member.Name == "ContentStringFormat")
						{
							stack.CurrentFrame.ContentStringFormatSet = true;
						}
						else if (xamlReader.Member.Name == "ContentSource")
						{
							stack.CurrentFrame.ContentSourceSet = true;
						}
					}
					if (!stack.CurrentFrame.IsInNameScope && !xamlReader.Member.IsDirective)
					{
						IXamlIndexingReader xamlIndexingReader = xamlReader as IXamlIndexingReader;
						bool flag = false;
						int currentIndex = xamlIndexingReader.CurrentIndex;
						PropertyValue? propertyValue;
						try
						{
							flag = this.TrySharingProperty(xamlReader, stack.CurrentFrame.Type, stack.CurrentFrame.Name, stack.InScopeNamespaces, out propertyValue);
						}
						catch
						{
							flag = false;
							propertyValue = null;
						}
						if (flag)
						{
							sharedProperties.Add(propertyValue.Value);
							if ((typeof(GridViewRowPresenter).IsAssignableFrom(stack.CurrentFrame.Type.UnderlyingType) || typeof(ContentPresenter).IsAssignableFrom(stack.CurrentFrame.Type.UnderlyingType)) && propertyValue.Value.Property.Name == "ContentSource")
							{
								stack.CurrentFrame.ContentSource = (propertyValue.Value.ValueInternal as string);
								if (!(propertyValue.Value.ValueInternal is string) && propertyValue.Value.ValueInternal != null)
								{
									stack.CurrentFrame.ContentSourceSet = false;
								}
							}
							return false;
						}
						xamlIndexingReader.CurrentIndex = currentIndex;
					}
				}
				break;
			case System.Xaml.XamlNodeType.EndMember:
				stack.CurrentFrame.Property = null;
				break;
			case System.Xaml.XamlNodeType.Value:
			{
				if (!stack.CurrentFrame.IsInStyleOrTemplate)
				{
					if (FrameworkTemplate.IsNameProperty(stack.CurrentFrame.Property, stack.CurrentFrame.Type))
					{
						string text = xamlReader.Value as string;
						stack.CurrentFrame.Name = text;
						stack.CurrentFrame.NameSet = true;
						if (this.TemplateLoadData.RootName == null)
						{
							this.TemplateLoadData.RootName = text;
						}
						if (!stack.CurrentFrame.IsInNameScope && (typeof(FrameworkElement).IsAssignableFrom(stack.CurrentFrame.Type.UnderlyingType) || typeof(FrameworkContentElement).IsAssignableFrom(stack.CurrentFrame.Type.UnderlyingType)))
						{
							this.TemplateLoadData.NamedTypes.Add(text, stack.CurrentFrame.Type);
							this.UpdateSharedPropertyNames(text, sharedProperties, stack.CurrentFrame.Type);
						}
					}
					if (typeof(ContentPresenter).IsAssignableFrom(stack.CurrentFrame.Type.UnderlyingType) && stack.CurrentFrame.Property.Name == "ContentSource")
					{
						stack.CurrentFrame.ContentSource = (xamlReader.Value as string);
					}
				}
				object value = xamlReader.Value;
				StaticResourceExtension staticResourceExtension = value as StaticResourceExtension;
				if (staticResourceExtension != null)
				{
					object obj = null;
					if (staticResourceExtension.GetType() == typeof(StaticResourceExtension))
					{
						obj = staticResourceExtension.TryProvideValueInternal(this.TemplateLoadData.ServiceProviderWrapper, true, true);
					}
					else if (staticResourceExtension.GetType() == typeof(StaticResourceHolder))
					{
						obj = staticResourceExtension.FindResourceInDeferredContent(this.TemplateLoadData.ServiceProviderWrapper, true, false);
						if (obj == DependencyProperty.UnsetValue)
						{
							obj = null;
						}
					}
					if (obj != null)
					{
						DeferredResourceReference prefetchedValue = obj as DeferredResourceReference;
						newValue = new StaticResourceHolder(staticResourceExtension.ResourceKey, prefetchedValue);
					}
				}
				break;
			}
			case System.Xaml.XamlNodeType.NamespaceDeclaration:
				if (!stack.CurrentFrame.IsInStyleOrTemplate && stack.Depth > 0 && !stack.CurrentFrame.NameSet && stack.CurrentFrame.Type != null && !stack.CurrentFrame.IsInNameScope)
				{
					if (typeof(FrameworkElement).IsAssignableFrom(stack.CurrentFrame.Type.UnderlyingType) || typeof(FrameworkContentElement).IsAssignableFrom(stack.CurrentFrame.Type.UnderlyingType))
					{
						int num = nameNumber;
						nameNumber = num + 1;
						string name4 = num.ToString(CultureInfo.InvariantCulture) + "_T";
						this.UpdateSharedPropertyNames(name4, sharedProperties, stack.CurrentFrame.Type);
						stack.CurrentFrame.Name = name4;
					}
					stack.CurrentFrame.NameSet = true;
				}
				stack.AddNamespace(xamlReader.Namespace);
				break;
			}
			return true;
		}

		// Token: 0x06000BE2 RID: 3042 RVA: 0x0002C2D4 File Offset: 0x0002A4D4
		private StaticResourceExtension LoadTimeBindUnshareableStaticResource(System.Xaml.XamlReader xamlReader, XamlObjectWriter writer)
		{
			int num = 0;
			do
			{
				writer.WriteNode(xamlReader);
				System.Xaml.XamlNodeType nodeType = xamlReader.NodeType;
				if (nodeType - System.Xaml.XamlNodeType.StartObject > 1)
				{
					if (nodeType == System.Xaml.XamlNodeType.EndObject)
					{
						num--;
					}
				}
				else
				{
					num++;
				}
			}
			while (num > 0 && xamlReader.Read());
			StaticResourceExtension staticResourceExtension = writer.Result as StaticResourceExtension;
			DeferredResourceReference prefetchedValue = (DeferredResourceReference)staticResourceExtension.TryProvideValueInternal(this.TemplateLoadData.ServiceProviderWrapper, true, true);
			return new StaticResourceHolder(staticResourceExtension.ResourceKey, prefetchedValue);
		}

		// Token: 0x06000BE3 RID: 3043 RVA: 0x0002C344 File Offset: 0x0002A544
		private bool TrySharingProperty(System.Xaml.XamlReader xamlReader, XamlType parentType, string parentName, FrugalObjectList<NamespaceDeclaration> previousNamespaces, out PropertyValue? sharedValue)
		{
			WpfXamlMember wpfXamlMember = xamlReader.Member as WpfXamlMember;
			if (wpfXamlMember == null)
			{
				sharedValue = null;
				return false;
			}
			DependencyProperty dependencyProperty = wpfXamlMember.DependencyProperty;
			if (dependencyProperty == null)
			{
				sharedValue = null;
				return false;
			}
			if (xamlReader.Member == parentType.GetAliasedProperty(XamlLanguage.Name))
			{
				sharedValue = null;
				return false;
			}
			if (!typeof(FrameworkElement).IsAssignableFrom(parentType.UnderlyingType) && !typeof(FrameworkContentElement).IsAssignableFrom(parentType.UnderlyingType))
			{
				sharedValue = null;
				return false;
			}
			xamlReader.Read();
			if (xamlReader.NodeType == System.Xaml.XamlNodeType.Value)
			{
				if (xamlReader.Value == null)
				{
					sharedValue = null;
					return false;
				}
				Type type = xamlReader.Value.GetType();
				if (!TemplateContent.CheckSpecialCasesShareable(type, dependencyProperty))
				{
					sharedValue = null;
					return false;
				}
				if (!(xamlReader.Value is string))
				{
					return this.TrySharingValue(dependencyProperty, xamlReader.Value, parentName, xamlReader, true, out sharedValue);
				}
				object value = xamlReader.Value;
				TypeConverter typeConverter = null;
				if (wpfXamlMember.TypeConverter != null)
				{
					typeConverter = wpfXamlMember.TypeConverter.ConverterInstance;
				}
				else if (wpfXamlMember.Type.TypeConverter != null)
				{
					typeConverter = wpfXamlMember.Type.TypeConverter.ConverterInstance;
				}
				if (typeConverter != null)
				{
					value = typeConverter.ConvertFrom(this.TemplateLoadData.ServiceProviderWrapper, CultureInfo.InvariantCulture, value);
				}
				return this.TrySharingValue(dependencyProperty, value, parentName, xamlReader, true, out sharedValue);
			}
			else
			{
				if (xamlReader.NodeType == System.Xaml.XamlNodeType.StartObject || xamlReader.NodeType == System.Xaml.XamlNodeType.NamespaceDeclaration)
				{
					FrugalObjectList<NamespaceDeclaration> frugalObjectList = null;
					if (xamlReader.NodeType == System.Xaml.XamlNodeType.NamespaceDeclaration)
					{
						frugalObjectList = new FrugalObjectList<NamespaceDeclaration>();
						while (xamlReader.NodeType == System.Xaml.XamlNodeType.NamespaceDeclaration)
						{
							frugalObjectList.Add(xamlReader.Namespace);
							xamlReader.Read();
						}
					}
					Type underlyingType = xamlReader.Type.UnderlyingType;
					if (!TemplateContent.CheckSpecialCasesShareable(underlyingType, dependencyProperty))
					{
						sharedValue = null;
						return false;
					}
					if (!this.IsTypeShareable(xamlReader.Type.UnderlyingType))
					{
						sharedValue = null;
						return false;
					}
					TemplateContent.StackOfFrames stackOfFrames = new TemplateContent.StackOfFrames();
					stackOfFrames.Push(xamlReader.Type, null);
					bool flag = false;
					bool flag2 = false;
					if (typeof(FrameworkTemplate).IsAssignableFrom(xamlReader.Type.UnderlyingType))
					{
						flag = true;
						this.Stack = stackOfFrames;
					}
					else if (typeof(Style).IsAssignableFrom(xamlReader.Type.UnderlyingType))
					{
						flag2 = true;
						this.Stack = stackOfFrames;
					}
					try
					{
						XamlObjectWriter objectWriter = this.TemplateLoadData.ObjectWriter;
						objectWriter.Clear();
						this.WriteNamespaces(objectWriter, previousNamespaces, frugalObjectList);
						objectWriter.WriteNode(xamlReader);
						bool flag3 = false;
						while (!flag3 && xamlReader.Read())
						{
							TemplateContent.SkipFreeze(xamlReader);
							objectWriter.WriteNode(xamlReader);
							switch (xamlReader.NodeType)
							{
							case System.Xaml.XamlNodeType.StartObject:
								if (typeof(StaticResourceExtension).IsAssignableFrom(xamlReader.Type.UnderlyingType))
								{
									sharedValue = null;
									return false;
								}
								stackOfFrames.Push(xamlReader.Type, null);
								break;
							case System.Xaml.XamlNodeType.GetObject:
							{
								XamlType type2 = stackOfFrames.CurrentFrame.Property.Type;
								stackOfFrames.Push(type2, null);
								break;
							}
							case System.Xaml.XamlNodeType.EndObject:
								if (stackOfFrames.Depth == 1)
								{
									return this.TrySharingValue(dependencyProperty, objectWriter.Result, parentName, xamlReader, true, out sharedValue);
								}
								stackOfFrames.PopScope();
								break;
							case System.Xaml.XamlNodeType.StartMember:
								if (!flag2 && !flag && FrameworkTemplate.IsNameProperty(xamlReader.Member, stackOfFrames.CurrentFrame.Type))
								{
									flag3 = true;
								}
								else
								{
									stackOfFrames.CurrentFrame.Property = xamlReader.Member;
								}
								break;
							case System.Xaml.XamlNodeType.Value:
								if (xamlReader.Value != null && typeof(StaticResourceExtension).IsAssignableFrom(xamlReader.Value.GetType()))
								{
									sharedValue = null;
									return false;
								}
								if (!flag && stackOfFrames.CurrentFrame.Property == XamlLanguage.ConnectionId && this.OwnerTemplate.StyleConnector != null)
								{
									this.OwnerTemplate.StyleConnector.Connect((int)xamlReader.Value, stackOfFrames.CurrentFrame.Instance);
								}
								break;
							}
						}
						sharedValue = null;
						return false;
					}
					finally
					{
						this.Stack = null;
					}
				}
				if (xamlReader.NodeType == System.Xaml.XamlNodeType.GetObject)
				{
					sharedValue = null;
					return false;
				}
				throw new System.Windows.Markup.XamlParseException(SR.Get("ParserUnexpectedEndEle"));
			}
		}

		// Token: 0x06000BE4 RID: 3044 RVA: 0x0002C7D0 File Offset: 0x0002A9D0
		private static bool CheckSpecialCasesShareable(Type typeofValue, DependencyProperty property)
		{
			if (typeofValue != typeof(DynamicResourceExtension) && typeofValue != typeof(TemplateBindingExtension) && typeofValue != typeof(TypeExtension) && typeofValue != typeof(StaticExtension))
			{
				if (typeof(IList).IsAssignableFrom(property.PropertyType))
				{
					return false;
				}
				if (property.PropertyType.IsArray)
				{
					return false;
				}
				if (typeof(IDictionary).IsAssignableFrom(property.PropertyType))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06000BE5 RID: 3045 RVA: 0x0002C868 File Offset: 0x0002AA68
		private static bool IsFreezableDirective(System.Xaml.XamlReader reader)
		{
			System.Xaml.XamlNodeType nodeType = reader.NodeType;
			if (nodeType == System.Xaml.XamlNodeType.StartMember)
			{
				XamlMember member = reader.Member;
				return member.IsUnknown && member.IsDirective && member.Name == "Freeze";
			}
			return false;
		}

		// Token: 0x06000BE6 RID: 3046 RVA: 0x0002C8AB File Offset: 0x0002AAAB
		private static void SkipFreeze(System.Xaml.XamlReader reader)
		{
			if (TemplateContent.IsFreezableDirective(reader))
			{
				reader.Read();
				reader.Read();
				reader.Read();
			}
		}

		// Token: 0x06000BE7 RID: 3047 RVA: 0x0002C8CC File Offset: 0x0002AACC
		private bool TrySharingValue(DependencyProperty property, object value, string parentName, System.Xaml.XamlReader xamlReader, bool allowRecursive, out PropertyValue? sharedValue)
		{
			sharedValue = null;
			if (value != null && !this.IsTypeShareable(value.GetType()))
			{
				return false;
			}
			bool flag = true;
			if (value is Freezable)
			{
				Freezable freezable = value as Freezable;
				if (freezable != null)
				{
					if (freezable.CanFreeze)
					{
						freezable.Freeze();
					}
					else
					{
						flag = false;
					}
				}
			}
			else if (value is CollectionViewSource)
			{
				CollectionViewSource collectionViewSource = value as CollectionViewSource;
				if (collectionViewSource != null)
				{
					flag = collectionViewSource.IsShareableInTemplate();
				}
			}
			else if (value is MarkupExtension)
			{
				if (value is BindingBase || value is TemplateBindingExtension || value is DynamicResourceExtension)
				{
					flag = true;
				}
				else if (value is StaticResourceExtension || value is StaticResourceHolder)
				{
					flag = false;
				}
				else
				{
					this.TemplateLoadData.ServiceProviderWrapper.SetData(TemplateContent._sharedDpInstance, property);
					value = (value as MarkupExtension).ProvideValue(this.TemplateLoadData.ServiceProviderWrapper);
					this.TemplateLoadData.ServiceProviderWrapper.Clear();
					if (allowRecursive)
					{
						return this.TrySharingValue(property, value, parentName, xamlReader, false, out sharedValue);
					}
					flag = true;
				}
			}
			if (flag)
			{
				sharedValue = new PropertyValue?(new PropertyValue
				{
					Property = property,
					ChildName = parentName,
					ValueInternal = value,
					ValueType = PropertyValueType.Set
				});
				xamlReader.Read();
			}
			return flag;
		}

		// Token: 0x06000BE8 RID: 3048 RVA: 0x0002CA18 File Offset: 0x0002AC18
		private bool IsTypeShareable(Type type)
		{
			return typeof(Freezable).IsAssignableFrom(type) || type == typeof(string) || type == typeof(Uri) || type == typeof(Type) || (typeof(MarkupExtension).IsAssignableFrom(type) && !typeof(StaticResourceExtension).IsAssignableFrom(type)) || typeof(Style).IsAssignableFrom(type) || typeof(FrameworkTemplate).IsAssignableFrom(type) || typeof(CollectionViewSource).IsAssignableFrom(type) || (type != null && type.IsValueType);
		}

		// Token: 0x06000BE9 RID: 3049 RVA: 0x0002CAE4 File Offset: 0x0002ACE4
		private void WriteNamespaces(System.Xaml.XamlWriter writer, FrugalObjectList<NamespaceDeclaration> previousNamespaces, FrugalObjectList<NamespaceDeclaration> localNamespaces)
		{
			if (previousNamespaces != null)
			{
				for (int i = 0; i < previousNamespaces.Count; i++)
				{
					writer.WriteNamespace(previousNamespaces[i]);
				}
			}
			if (localNamespaces != null)
			{
				for (int j = 0; j < localNamespaces.Count; j++)
				{
					writer.WriteNamespace(localNamespaces[j]);
				}
			}
		}

		// Token: 0x06000BEA RID: 3050 RVA: 0x0002CB34 File Offset: 0x0002AD34
		private static void AutoAliasContentPresenter(Type targetType, string contentSource, string templateChildName, ref FrugalStructList<ChildRecord> childRecordFromChildIndex, ref FrugalStructList<ItemStructMap<TriggerSourceRecord>> triggerSourceRecordFromChildIndex, ref FrugalStructList<ChildPropertyDependent> resourceDependents, ref HybridDictionary dataTriggerRecordFromBinding, ref bool hasInstanceValues, HybridDictionary childIndexFromChildName, bool isContentPropertyDefined, bool isContentSourceSet, bool isContentTemplatePropertyDefined, bool isContentTemplateSelectorPropertyDefined, bool isContentStringFormatPropertyDefined)
		{
			if (string.IsNullOrEmpty(contentSource) && !isContentSourceSet)
			{
				contentSource = "Content";
			}
			if (!string.IsNullOrEmpty(contentSource) && !isContentPropertyDefined)
			{
				DependencyProperty dependencyProperty = DependencyProperty.FromName(contentSource, targetType);
				DependencyProperty dependencyProperty2 = DependencyProperty.FromName(contentSource + "Template", targetType);
				DependencyProperty dependencyProperty3 = DependencyProperty.FromName(contentSource + "TemplateSelector", targetType);
				DependencyProperty dependencyProperty4 = DependencyProperty.FromName(contentSource + "StringFormat", targetType);
				if (dependencyProperty == null && isContentSourceSet)
				{
					throw new InvalidOperationException(SR.Get("MissingContentSource", new object[]
					{
						contentSource,
						targetType
					}));
				}
				if (dependencyProperty != null)
				{
					PropertyValue propertyValue = default(PropertyValue);
					propertyValue.ValueType = PropertyValueType.TemplateBinding;
					propertyValue.ChildName = templateChildName;
					propertyValue.ValueInternal = new TemplateBindingExtension(dependencyProperty);
					propertyValue.Property = ContentPresenter.ContentProperty;
					StyleHelper.UpdateTables(ref propertyValue, ref childRecordFromChildIndex, ref triggerSourceRecordFromChildIndex, ref resourceDependents, ref dataTriggerRecordFromBinding, childIndexFromChildName, ref hasInstanceValues);
				}
				if (!isContentTemplatePropertyDefined && !isContentTemplateSelectorPropertyDefined && !isContentStringFormatPropertyDefined)
				{
					if (dependencyProperty2 != null)
					{
						PropertyValue propertyValue2 = default(PropertyValue);
						propertyValue2.ValueType = PropertyValueType.TemplateBinding;
						propertyValue2.ChildName = templateChildName;
						propertyValue2.ValueInternal = new TemplateBindingExtension(dependencyProperty2);
						propertyValue2.Property = ContentPresenter.ContentTemplateProperty;
						StyleHelper.UpdateTables(ref propertyValue2, ref childRecordFromChildIndex, ref triggerSourceRecordFromChildIndex, ref resourceDependents, ref dataTriggerRecordFromBinding, childIndexFromChildName, ref hasInstanceValues);
					}
					if (dependencyProperty3 != null)
					{
						PropertyValue propertyValue3 = default(PropertyValue);
						propertyValue3.ValueType = PropertyValueType.TemplateBinding;
						propertyValue3.ChildName = templateChildName;
						propertyValue3.ValueInternal = new TemplateBindingExtension(dependencyProperty3);
						propertyValue3.Property = ContentPresenter.ContentTemplateSelectorProperty;
						StyleHelper.UpdateTables(ref propertyValue3, ref childRecordFromChildIndex, ref triggerSourceRecordFromChildIndex, ref resourceDependents, ref dataTriggerRecordFromBinding, childIndexFromChildName, ref hasInstanceValues);
					}
					if (dependencyProperty4 != null)
					{
						PropertyValue propertyValue4 = default(PropertyValue);
						propertyValue4.ValueType = PropertyValueType.TemplateBinding;
						propertyValue4.ChildName = templateChildName;
						propertyValue4.ValueInternal = new TemplateBindingExtension(dependencyProperty4);
						propertyValue4.Property = ContentPresenter.ContentStringFormatProperty;
						StyleHelper.UpdateTables(ref propertyValue4, ref childRecordFromChildIndex, ref triggerSourceRecordFromChildIndex, ref resourceDependents, ref dataTriggerRecordFromBinding, childIndexFromChildName, ref hasInstanceValues);
					}
				}
			}
		}

		// Token: 0x06000BEB RID: 3051 RVA: 0x0002CCF8 File Offset: 0x0002AEF8
		private static void AutoAliasGridViewRowPresenter(Type targetType, string contentSource, string childName, ref FrugalStructList<ChildRecord> childRecordFromChildIndex, ref FrugalStructList<ItemStructMap<TriggerSourceRecord>> triggerSourceRecordFromChildIndex, ref FrugalStructList<ChildPropertyDependent> resourceDependents, ref HybridDictionary dataTriggerRecordFromBinding, ref bool hasInstanceValues, HybridDictionary childIndexFromChildID, bool isContentPropertyDefined, bool isColumnsPropertyDefined)
		{
			if (!isContentPropertyDefined)
			{
				DependencyProperty dependencyProperty = DependencyProperty.FromName("Content", targetType);
				if (dependencyProperty != null)
				{
					PropertyValue propertyValue = default(PropertyValue);
					propertyValue.ValueType = PropertyValueType.TemplateBinding;
					propertyValue.ChildName = childName;
					propertyValue.ValueInternal = new TemplateBindingExtension(dependencyProperty);
					propertyValue.Property = GridViewRowPresenter.ContentProperty;
					StyleHelper.UpdateTables(ref propertyValue, ref childRecordFromChildIndex, ref triggerSourceRecordFromChildIndex, ref resourceDependents, ref dataTriggerRecordFromBinding, childIndexFromChildID, ref hasInstanceValues);
				}
			}
			if (!isColumnsPropertyDefined)
			{
				PropertyValue propertyValue2 = default(PropertyValue);
				propertyValue2.ValueType = PropertyValueType.TemplateBinding;
				propertyValue2.ChildName = childName;
				propertyValue2.ValueInternal = new TemplateBindingExtension(GridView.ColumnCollectionProperty);
				propertyValue2.Property = GridViewRowPresenterBase.ColumnsProperty;
				StyleHelper.UpdateTables(ref propertyValue2, ref childRecordFromChildIndex, ref triggerSourceRecordFromChildIndex, ref resourceDependents, ref dataTriggerRecordFromBinding, childIndexFromChildID, ref hasInstanceValues);
			}
		}

		// Token: 0x170003D6 RID: 982
		// (get) Token: 0x06000BEC RID: 3052 RVA: 0x0002CDA6 File Offset: 0x0002AFA6
		// (set) Token: 0x06000BED RID: 3053 RVA: 0x0002CDAE File Offset: 0x0002AFAE
		internal XamlType RootType { get; private set; }

		// Token: 0x06000BEE RID: 3054 RVA: 0x0002CDB7 File Offset: 0x0002AFB7
		internal XamlType GetTypeForName(string name)
		{
			return this.TemplateLoadData.NamedTypes[name];
		}

		// Token: 0x170003D7 RID: 983
		// (get) Token: 0x06000BEF RID: 3055 RVA: 0x0002CDCA File Offset: 0x0002AFCA
		// (set) Token: 0x06000BF0 RID: 3056 RVA: 0x0002CDD2 File Offset: 0x0002AFD2
		internal FrameworkTemplate OwnerTemplate { get; set; }

		// Token: 0x170003D8 RID: 984
		// (get) Token: 0x06000BF1 RID: 3057 RVA: 0x0002CDDB File Offset: 0x0002AFDB
		// (set) Token: 0x06000BF2 RID: 3058 RVA: 0x0002CDE3 File Offset: 0x0002AFE3
		internal IXamlObjectWriterFactory ObjectWriterFactory { get; private set; }

		// Token: 0x170003D9 RID: 985
		// (get) Token: 0x06000BF3 RID: 3059 RVA: 0x0002CDEC File Offset: 0x0002AFEC
		// (set) Token: 0x06000BF4 RID: 3060 RVA: 0x0002CDF4 File Offset: 0x0002AFF4
		internal XamlObjectWriterSettings ObjectWriterParentSettings { get; private set; }

		// Token: 0x170003DA RID: 986
		// (get) Token: 0x06000BF5 RID: 3061 RVA: 0x0002CDFD File Offset: 0x0002AFFD
		// (set) Token: 0x06000BF6 RID: 3062 RVA: 0x0002CE05 File Offset: 0x0002B005
		internal XamlSchemaContext SchemaContext { get; private set; }

		// Token: 0x170003DB RID: 987
		// (get) Token: 0x06000BF7 RID: 3063 RVA: 0x0002CE0E File Offset: 0x0002B00E
		// (set) Token: 0x06000BF8 RID: 3064 RVA: 0x0002CE1B File Offset: 0x0002B01B
		private TemplateContent.StackOfFrames Stack
		{
			get
			{
				return this.TemplateLoadData.Stack;
			}
			set
			{
				this.TemplateLoadData.Stack = value;
			}
		}

		// Token: 0x170003DC RID: 988
		// (get) Token: 0x06000BF9 RID: 3065 RVA: 0x0002CE29 File Offset: 0x0002B029
		// (set) Token: 0x06000BFA RID: 3066 RVA: 0x0002CE31 File Offset: 0x0002B031
		internal TemplateLoadData TemplateLoadData { get; [SecurityCritical] set; }

		// Token: 0x04000AC2 RID: 2754
		[SecurityCritical]
		internal XamlNodeList _xamlNodeList;

		// Token: 0x04000AC3 RID: 2755
		private static SharedDp _sharedDpInstance = new SharedDp(null, null, null);

		// Token: 0x0200082D RID: 2093
		internal class Frame : XamlFrame
		{
			// Token: 0x17001D5E RID: 7518
			// (get) Token: 0x06007E82 RID: 32386 RVA: 0x00236313 File Offset: 0x00234513
			// (set) Token: 0x06007E83 RID: 32387 RVA: 0x0023631B File Offset: 0x0023451B
			public XamlType Type
			{
				get
				{
					return this._xamlType;
				}
				set
				{
					this._xamlType = value;
				}
			}

			// Token: 0x17001D5F RID: 7519
			// (get) Token: 0x06007E84 RID: 32388 RVA: 0x00236324 File Offset: 0x00234524
			// (set) Token: 0x06007E85 RID: 32389 RVA: 0x0023632C File Offset: 0x0023452C
			public XamlMember Property { get; set; }

			// Token: 0x17001D60 RID: 7520
			// (get) Token: 0x06007E86 RID: 32390 RVA: 0x00236335 File Offset: 0x00234535
			// (set) Token: 0x06007E87 RID: 32391 RVA: 0x0023633D File Offset: 0x0023453D
			public string Name { get; set; }

			// Token: 0x17001D61 RID: 7521
			// (get) Token: 0x06007E88 RID: 32392 RVA: 0x00236346 File Offset: 0x00234546
			// (set) Token: 0x06007E89 RID: 32393 RVA: 0x0023634E File Offset: 0x0023454E
			public bool NameSet { get; set; }

			// Token: 0x17001D62 RID: 7522
			// (get) Token: 0x06007E8A RID: 32394 RVA: 0x00236357 File Offset: 0x00234557
			// (set) Token: 0x06007E8B RID: 32395 RVA: 0x0023635F File Offset: 0x0023455F
			public bool IsInNameScope { get; set; }

			// Token: 0x17001D63 RID: 7523
			// (get) Token: 0x06007E8C RID: 32396 RVA: 0x00236368 File Offset: 0x00234568
			// (set) Token: 0x06007E8D RID: 32397 RVA: 0x00236370 File Offset: 0x00234570
			public bool IsInStyleOrTemplate { get; set; }

			// Token: 0x17001D64 RID: 7524
			// (get) Token: 0x06007E8E RID: 32398 RVA: 0x00236379 File Offset: 0x00234579
			// (set) Token: 0x06007E8F RID: 32399 RVA: 0x00236381 File Offset: 0x00234581
			public object Instance { get; set; }

			// Token: 0x17001D65 RID: 7525
			// (get) Token: 0x06007E90 RID: 32400 RVA: 0x0023638A File Offset: 0x0023458A
			// (set) Token: 0x06007E91 RID: 32401 RVA: 0x00236392 File Offset: 0x00234592
			public bool ContentSet { get; set; }

			// Token: 0x17001D66 RID: 7526
			// (get) Token: 0x06007E92 RID: 32402 RVA: 0x0023639B File Offset: 0x0023459B
			// (set) Token: 0x06007E93 RID: 32403 RVA: 0x002363A3 File Offset: 0x002345A3
			public bool ContentSourceSet { get; set; }

			// Token: 0x17001D67 RID: 7527
			// (get) Token: 0x06007E94 RID: 32404 RVA: 0x002363AC File Offset: 0x002345AC
			// (set) Token: 0x06007E95 RID: 32405 RVA: 0x002363B4 File Offset: 0x002345B4
			public string ContentSource { get; set; }

			// Token: 0x17001D68 RID: 7528
			// (get) Token: 0x06007E96 RID: 32406 RVA: 0x002363BD File Offset: 0x002345BD
			// (set) Token: 0x06007E97 RID: 32407 RVA: 0x002363C5 File Offset: 0x002345C5
			public bool ContentTemplateSet { get; set; }

			// Token: 0x17001D69 RID: 7529
			// (get) Token: 0x06007E98 RID: 32408 RVA: 0x002363CE File Offset: 0x002345CE
			// (set) Token: 0x06007E99 RID: 32409 RVA: 0x002363D6 File Offset: 0x002345D6
			public bool ContentTemplateSelectorSet { get; set; }

			// Token: 0x17001D6A RID: 7530
			// (get) Token: 0x06007E9A RID: 32410 RVA: 0x002363DF File Offset: 0x002345DF
			// (set) Token: 0x06007E9B RID: 32411 RVA: 0x002363E7 File Offset: 0x002345E7
			public bool ContentStringFormatSet { get; set; }

			// Token: 0x17001D6B RID: 7531
			// (get) Token: 0x06007E9C RID: 32412 RVA: 0x002363F0 File Offset: 0x002345F0
			// (set) Token: 0x06007E9D RID: 32413 RVA: 0x002363F8 File Offset: 0x002345F8
			public bool ColumnsSet { get; set; }

			// Token: 0x06007E9E RID: 32414 RVA: 0x00236404 File Offset: 0x00234604
			public override void Reset()
			{
				this._xamlType = null;
				this.Property = null;
				this.Name = null;
				this.NameSet = false;
				this.IsInNameScope = false;
				this.Instance = null;
				this.ContentSet = false;
				this.ContentSourceSet = false;
				this.ContentSource = null;
				this.ContentTemplateSet = false;
				this.ContentTemplateSelectorSet = false;
				this.ContentStringFormatSet = false;
				this.IsInNameScope = false;
				if (this.HasNamespaces)
				{
					this._namespaces = null;
				}
			}

			// Token: 0x17001D6C RID: 7532
			// (get) Token: 0x06007E9F RID: 32415 RVA: 0x0023647B File Offset: 0x0023467B
			public FrugalObjectList<NamespaceDeclaration> Namespaces
			{
				get
				{
					if (this._namespaces == null)
					{
						this._namespaces = new FrugalObjectList<NamespaceDeclaration>();
					}
					return this._namespaces;
				}
			}

			// Token: 0x17001D6D RID: 7533
			// (get) Token: 0x06007EA0 RID: 32416 RVA: 0x00236496 File Offset: 0x00234696
			public bool HasNamespaces
			{
				get
				{
					return this._namespaces != null && this._namespaces.Count > 0;
				}
			}

			// Token: 0x06007EA1 RID: 32417 RVA: 0x002364B0 File Offset: 0x002346B0
			public override string ToString()
			{
				string text = (this.Type == null) ? string.Empty : this.Type.Name;
				string text2 = (this.Property == null) ? "-" : this.Property.Name;
				string text3 = (this.Instance == null) ? "-" : "*";
				return string.Format(CultureInfo.InvariantCulture, "{0}.{1} inst={2}", new object[]
				{
					text,
					text2,
					text3
				});
			}

			// Token: 0x04003C9E RID: 15518
			private FrugalObjectList<NamespaceDeclaration> _namespaces;

			// Token: 0x04003C9F RID: 15519
			private XamlType _xamlType;
		}

		// Token: 0x0200082E RID: 2094
		internal class StackOfFrames : XamlContextStack<TemplateContent.Frame>
		{
			// Token: 0x06007EA2 RID: 32418 RVA: 0x00236537 File Offset: 0x00234737
			public StackOfFrames() : base(() => new TemplateContent.Frame())
			{
			}

			// Token: 0x06007EA3 RID: 32419 RVA: 0x00236560 File Offset: 0x00234760
			public void Push(XamlType xamlType, string name)
			{
				bool isInNameScope = false;
				bool isInStyleOrTemplate = false;
				if (base.Depth > 0)
				{
					isInNameScope = (base.CurrentFrame.IsInNameScope || (base.CurrentFrame.Type != null && FrameworkTemplate.IsNameScope(base.CurrentFrame.Type)));
					isInStyleOrTemplate = (base.CurrentFrame.IsInStyleOrTemplate || (base.CurrentFrame.Type != null && (typeof(FrameworkTemplate).IsAssignableFrom(base.CurrentFrame.Type.UnderlyingType) || typeof(Style).IsAssignableFrom(base.CurrentFrame.Type.UnderlyingType))));
				}
				if (base.Depth == 0 || base.CurrentFrame.Type != null)
				{
					base.PushScope();
				}
				base.CurrentFrame.Type = xamlType;
				base.CurrentFrame.Name = name;
				base.CurrentFrame.IsInNameScope = isInNameScope;
				base.CurrentFrame.IsInStyleOrTemplate = isInStyleOrTemplate;
			}

			// Token: 0x06007EA4 RID: 32420 RVA: 0x00236670 File Offset: 0x00234870
			public void AddNamespace(NamespaceDeclaration nsd)
			{
				bool isInNameScope = false;
				bool isInStyleOrTemplate = false;
				if (base.Depth > 0)
				{
					isInNameScope = (base.CurrentFrame.IsInNameScope || (base.CurrentFrame.Type != null && FrameworkTemplate.IsNameScope(base.CurrentFrame.Type)));
					isInStyleOrTemplate = (base.CurrentFrame.IsInStyleOrTemplate || (base.CurrentFrame.Type != null && (typeof(FrameworkTemplate).IsAssignableFrom(base.CurrentFrame.Type.UnderlyingType) || typeof(Style).IsAssignableFrom(base.CurrentFrame.Type.UnderlyingType))));
				}
				if (base.Depth == 0 || base.CurrentFrame.Type != null)
				{
					base.PushScope();
				}
				base.CurrentFrame.Namespaces.Add(nsd);
				base.CurrentFrame.IsInNameScope = isInNameScope;
				base.CurrentFrame.IsInStyleOrTemplate = isInStyleOrTemplate;
			}

			// Token: 0x17001D6E RID: 7534
			// (get) Token: 0x06007EA5 RID: 32421 RVA: 0x0023677C File Offset: 0x0023497C
			public FrugalObjectList<NamespaceDeclaration> InScopeNamespaces
			{
				get
				{
					FrugalObjectList<NamespaceDeclaration> frugalObjectList = null;
					for (TemplateContent.Frame frame = base.CurrentFrame; frame != null; frame = (TemplateContent.Frame)frame.Previous)
					{
						if (frame.HasNamespaces)
						{
							if (frugalObjectList == null)
							{
								frugalObjectList = new FrugalObjectList<NamespaceDeclaration>();
							}
							for (int i = 0; i < frame.Namespaces.Count; i++)
							{
								frugalObjectList.Add(frame.Namespaces[i]);
							}
						}
					}
					return frugalObjectList;
				}
			}
		}

		// Token: 0x0200082F RID: 2095
		internal class ServiceProviderWrapper : ITypeDescriptorContext, IServiceProvider, IXamlTypeResolver, IXamlNamespaceResolver, IProvideValueTarget
		{
			// Token: 0x17001D6F RID: 7535
			// (get) Token: 0x06007EA6 RID: 32422 RVA: 0x002367DE File Offset: 0x002349DE
			// (set) Token: 0x06007EA7 RID: 32423 RVA: 0x002367E6 File Offset: 0x002349E6
			internal TemplateContent.StackOfFrames Frames { get; set; }

			// Token: 0x06007EA8 RID: 32424 RVA: 0x002367EF File Offset: 0x002349EF
			public ServiceProviderWrapper(IServiceProvider services, XamlSchemaContext schemaContext)
			{
				this._services = services;
				this._schemaContext = schemaContext;
			}

			// Token: 0x06007EA9 RID: 32425 RVA: 0x00236805 File Offset: 0x00234A05
			object IServiceProvider.GetService(Type serviceType)
			{
				if (serviceType == typeof(IXamlTypeResolver))
				{
					return this;
				}
				if (serviceType == typeof(IProvideValueTarget))
				{
					return this;
				}
				return this._services.GetService(serviceType);
			}

			// Token: 0x06007EAA RID: 32426 RVA: 0x0023683B File Offset: 0x00234A3B
			Type IXamlTypeResolver.Resolve(string qualifiedTypeName)
			{
				return this._schemaContext.GetXamlType(XamlTypeName.Parse(qualifiedTypeName, this)).UnderlyingType;
			}

			// Token: 0x06007EAB RID: 32427 RVA: 0x00236854 File Offset: 0x00234A54
			string IXamlNamespaceResolver.GetNamespace(string prefix)
			{
				FrugalObjectList<NamespaceDeclaration> inScopeNamespaces = this.Frames.InScopeNamespaces;
				if (inScopeNamespaces != null)
				{
					for (int i = 0; i < inScopeNamespaces.Count; i++)
					{
						if (inScopeNamespaces[i].Prefix == prefix)
						{
							return inScopeNamespaces[i].Namespace;
						}
					}
				}
				return ((IXamlNamespaceResolver)this._services.GetService(typeof(IXamlNamespaceResolver))).GetNamespace(prefix);
			}

			// Token: 0x06007EAC RID: 32428 RVA: 0x0003E384 File Offset: 0x0003C584
			IEnumerable<NamespaceDeclaration> IXamlNamespaceResolver.GetNamespacePrefixes()
			{
				throw new NotImplementedException();
			}

			// Token: 0x17001D70 RID: 7536
			// (get) Token: 0x06007EAD RID: 32429 RVA: 0x0000C238 File Offset: 0x0000A438
			IContainer ITypeDescriptorContext.Container
			{
				get
				{
					return null;
				}
			}

			// Token: 0x17001D71 RID: 7537
			// (get) Token: 0x06007EAE RID: 32430 RVA: 0x0000C238 File Offset: 0x0000A438
			object ITypeDescriptorContext.Instance
			{
				get
				{
					return null;
				}
			}

			// Token: 0x06007EAF RID: 32431 RVA: 0x00002137 File Offset: 0x00000337
			void ITypeDescriptorContext.OnComponentChanged()
			{
			}

			// Token: 0x06007EB0 RID: 32432 RVA: 0x0000B02A File Offset: 0x0000922A
			bool ITypeDescriptorContext.OnComponentChanging()
			{
				return false;
			}

			// Token: 0x17001D72 RID: 7538
			// (get) Token: 0x06007EB1 RID: 32433 RVA: 0x0000C238 File Offset: 0x0000A438
			PropertyDescriptor ITypeDescriptorContext.PropertyDescriptor
			{
				get
				{
					return null;
				}
			}

			// Token: 0x06007EB2 RID: 32434 RVA: 0x002368C2 File Offset: 0x00234AC2
			public void SetData(object targetObject, object targetProperty)
			{
				this._targetObject = targetObject;
				this._targetProperty = targetProperty;
			}

			// Token: 0x06007EB3 RID: 32435 RVA: 0x002368D2 File Offset: 0x00234AD2
			public void Clear()
			{
				this._targetObject = null;
				this._targetProperty = null;
			}

			// Token: 0x17001D73 RID: 7539
			// (get) Token: 0x06007EB4 RID: 32436 RVA: 0x002368E2 File Offset: 0x00234AE2
			object IProvideValueTarget.TargetObject
			{
				get
				{
					return this._targetObject;
				}
			}

			// Token: 0x17001D74 RID: 7540
			// (get) Token: 0x06007EB5 RID: 32437 RVA: 0x002368EA File Offset: 0x00234AEA
			object IProvideValueTarget.TargetProperty
			{
				get
				{
					return this._targetProperty;
				}
			}

			// Token: 0x04003CAD RID: 15533
			private IServiceProvider _services;

			// Token: 0x04003CAF RID: 15535
			private XamlSchemaContext _schemaContext;

			// Token: 0x04003CB0 RID: 15536
			private object _targetObject;

			// Token: 0x04003CB1 RID: 15537
			private object _targetProperty;
		}
	}
}
