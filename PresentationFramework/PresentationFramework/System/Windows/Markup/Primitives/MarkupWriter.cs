using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace System.Windows.Markup.Primitives
{
	/// <summary>Provides methods to write an object to XAML format.</summary>
	// Token: 0x0200028B RID: 651
	public sealed class MarkupWriter : IDisposable
	{
		/// <summary>Creates an instance of a <see cref="T:System.Windows.Markup.Primitives.MarkupObject" /> from the specified object.</summary>
		/// <param name="instance">An object that will be the root of the serialized tree.</param>
		/// <returns>A markup object that enables navigating through the tree of objects.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="instance" /> is <see langword="null" />.</exception>
		// Token: 0x060024A5 RID: 9381 RVA: 0x000B12D0 File Offset: 0x000AF4D0
		public static MarkupObject GetMarkupObjectFor(object instance)
		{
			if (instance == null)
			{
				throw new ArgumentNullException("instance");
			}
			return new ElementMarkupObject(instance, new XamlDesignerSerializationManager(null)
			{
				XamlWriterMode = XamlWriterMode.Expression
			});
		}

		/// <summary>Creates an instance of a <see cref="T:System.Windows.Markup.Primitives.MarkupObject" /> from the specified object and the specified serialization manager.</summary>
		/// <param name="instance">An object that will be the root of the serialized tree.</param>
		/// <param name="manager">The serialization manager.</param>
		/// <returns>A markup object that enables navigating through the tree of objects.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="instance" /> or <paramref name="manager" /> is <see langword="null" />.</exception>
		// Token: 0x060024A6 RID: 9382 RVA: 0x000B1300 File Offset: 0x000AF500
		public static MarkupObject GetMarkupObjectFor(object instance, XamlDesignerSerializationManager manager)
		{
			if (instance == null)
			{
				throw new ArgumentNullException("instance");
			}
			if (manager == null)
			{
				throw new ArgumentNullException("manager");
			}
			return new ElementMarkupObject(instance, manager);
		}

		// Token: 0x060024A7 RID: 9383 RVA: 0x000B1325 File Offset: 0x000AF525
		internal static void SaveAsXml(XmlWriter writer, object instance)
		{
			if (writer == null)
			{
				throw new ArgumentNullException("writer");
			}
			MarkupWriter.SaveAsXml(writer, MarkupWriter.GetMarkupObjectFor(instance));
		}

		// Token: 0x060024A8 RID: 9384 RVA: 0x000B1341 File Offset: 0x000AF541
		internal static void SaveAsXml(XmlWriter writer, object instance, XamlDesignerSerializationManager manager)
		{
			if (writer == null)
			{
				throw new ArgumentNullException("writer");
			}
			if (manager == null)
			{
				throw new ArgumentNullException("manager");
			}
			manager.ClearXmlWriter();
			MarkupWriter.SaveAsXml(writer, MarkupWriter.GetMarkupObjectFor(instance, manager));
		}

		// Token: 0x060024A9 RID: 9385 RVA: 0x000B1374 File Offset: 0x000AF574
		internal static void SaveAsXml(XmlWriter writer, MarkupObject item)
		{
			if (writer == null)
			{
				throw new ArgumentNullException("writer");
			}
			if (item == null)
			{
				throw new ArgumentNullException("item");
			}
			try
			{
				using (MarkupWriter markupWriter = new MarkupWriter(writer))
				{
					markupWriter.WriteItem(item);
				}
			}
			finally
			{
				writer.Flush();
			}
		}

		// Token: 0x060024AA RID: 9386 RVA: 0x000B13DC File Offset: 0x000AF5DC
		internal static void VerifyTypeIsSerializable(Type type)
		{
			if (type.IsNestedPublic)
			{
				throw new InvalidOperationException(SR.Get("MarkupWriter_CannotSerializeNestedPublictype", new object[]
				{
					type.ToString()
				}));
			}
			if (!type.IsPublic)
			{
				throw new InvalidOperationException(SR.Get("MarkupWriter_CannotSerializeNonPublictype", new object[]
				{
					type.ToString()
				}));
			}
			if (type.IsGenericType)
			{
				throw new InvalidOperationException(SR.Get("MarkupWriter_CannotSerializeGenerictype", new object[]
				{
					type.ToString()
				}));
			}
		}

		// Token: 0x060024AB RID: 9387 RVA: 0x000B145E File Offset: 0x000AF65E
		internal MarkupWriter(XmlWriter writer)
		{
			this._writer = writer;
			this._xmlTextWriter = (writer as XmlTextWriter);
		}

		/// <summary>Releases the resources used by the <see cref="T:System.Windows.Markup.Primitives.MarkupWriter" />.</summary>
		// Token: 0x060024AC RID: 9388 RVA: 0x000B1479 File Offset: 0x000AF679
		public void Dispose()
		{
			GC.SuppressFinalize(this);
		}

		// Token: 0x060024AD RID: 9389 RVA: 0x000B1484 File Offset: 0x000AF684
		private bool RecordNamespaces(MarkupWriter.Scope scope, MarkupObject item, IValueSerializerContext context, bool lastWasString)
		{
			bool result = true;
			if (lastWasString || item.ObjectType != typeof(string) || this.HasNonValueProperties(item))
			{
				scope.MakeAddressable(item.ObjectType);
				result = false;
			}
			item.AssignRootContext(context);
			lastWasString = false;
			foreach (MarkupProperty markupProperty in item.Properties)
			{
				if (markupProperty.IsComposite)
				{
					bool flag = this.IsCollectionType(markupProperty.PropertyType);
					using (IEnumerator<MarkupObject> enumerator2 = markupProperty.Items.GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							MarkupObject item2 = enumerator2.Current;
							lastWasString = this.RecordNamespaces(scope, item2, context, lastWasString || flag);
						}
						goto IL_B7;
					}
					goto IL_AB;
				}
				goto IL_AB;
				IL_B7:
				if (markupProperty.DependencyProperty != null)
				{
					scope.MakeAddressable(markupProperty.DependencyProperty.OwnerType);
				}
				if (markupProperty.IsKey)
				{
					scope.MakeAddressable(MarkupWriter.NamespaceCache.XamlNamespace);
					continue;
				}
				continue;
				IL_AB:
				scope.MakeAddressable(markupProperty.TypeReferences);
				goto IL_B7;
			}
			return result;
		}

		// Token: 0x060024AE RID: 9390 RVA: 0x000B15AC File Offset: 0x000AF7AC
		internal void WriteItem(MarkupObject item)
		{
			MarkupWriter.VerifyTypeIsSerializable(item.ObjectType);
			MarkupWriter.Scope scope = new MarkupWriter.Scope(null);
			scope.RecordMapping("", MarkupWriter.NamespaceCache.GetNamespaceUriFor(item.ObjectType));
			this.RecordNamespaces(scope, item, new MarkupWriter.MarkupWriterContext(scope), false);
			item = new ExtensionSimplifierMarkupObject(item, null);
			this.WriteItem(item, scope);
			this._writer = null;
		}

		// Token: 0x060024AF RID: 9391 RVA: 0x000B160C File Offset: 0x000AF80C
		private void WriteItem(MarkupObject item, MarkupWriter.Scope scope)
		{
			MarkupWriter.VerifyTypeIsSerializable(item.ObjectType);
			MarkupWriter.MarkupWriterContext context = new MarkupWriter.MarkupWriterContext(scope);
			item.AssignRootContext(context);
			string text = scope.MakeAddressable(item.ObjectType);
			string prefixOf = scope.GetPrefixOf(text);
			string text2 = item.ObjectType.Name;
			if (typeof(MarkupExtension).IsAssignableFrom(item.ObjectType) && text2.EndsWith("Extension", StringComparison.Ordinal))
			{
				text2 = text2.Substring(0, text2.Length - 9);
			}
			this._writer.WriteStartElement(prefixOf, text2, text);
			ContentPropertyAttribute cpa = item.Attributes[typeof(ContentPropertyAttribute)] as ContentPropertyAttribute;
			XmlLangPropertyAttribute xmlLangPropertyAttribute = item.Attributes[typeof(XmlLangPropertyAttribute)] as XmlLangPropertyAttribute;
			UidPropertyAttribute uidPropertyAttribute = item.Attributes[typeof(UidPropertyAttribute)] as UidPropertyAttribute;
			MarkupProperty markupProperty = null;
			int num = 0;
			List<int> list = null;
			List<MarkupProperty> list2 = null;
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			MarkupWriter.PartiallyOrderedList<string, MarkupProperty> partiallyOrderedList = null;
			Formatting formatting = (this._xmlTextWriter != null) ? this._xmlTextWriter.Formatting : Formatting.None;
			foreach (MarkupProperty markupProperty2 in item.GetProperties(false))
			{
				if (markupProperty2.IsConstructorArgument)
				{
					throw new InvalidOperationException(SR.Get("UnserializableKeyValue"));
				}
				if (!this.IsContentProperty(markupProperty2, cpa, ref markupProperty))
				{
					if (!this.IsDeferredProperty(markupProperty2, dictionary, ref partiallyOrderedList))
					{
						if (!markupProperty2.IsComposite)
						{
							if (markupProperty2.IsAttached || markupProperty2.PropertyDescriptor == null)
							{
								if (markupProperty2.IsValueAsString)
								{
									markupProperty = markupProperty2;
								}
								else if (markupProperty2.IsKey)
								{
									scope.MakeAddressable(MarkupWriter.NamespaceCache.XamlNamespace);
									this._writer.WriteAttributeString(scope.GetPrefixOf(MarkupWriter.NamespaceCache.XamlNamespace), "Key", MarkupWriter.NamespaceCache.XamlNamespace, markupProperty2.StringValue);
								}
								else
								{
									DependencyProperty dependencyProperty = markupProperty2.DependencyProperty;
									string text3 = scope.MakeAddressable(dependencyProperty.OwnerType);
									scope.MakeAddressable(markupProperty2.TypeReferences);
									if (markupProperty2.Attributes[typeof(DesignerSerializationOptionsAttribute)] != null)
									{
										DesignerSerializationOptionsAttribute designerSerializationOptionsAttribute = markupProperty2.Attributes[typeof(DesignerSerializationOptionsAttribute)] as DesignerSerializationOptionsAttribute;
										if (designerSerializationOptionsAttribute.DesignerSerializationOptions == DesignerSerializationOptions.SerializeAsAttribute)
										{
											if (dependencyProperty == UIElement.UidProperty)
											{
												string text4 = scope.MakeAddressable(typeof(TypeExtension));
												this._writer.WriteAttributeString(scope.GetPrefixOf(text4), dependencyProperty.Name, text4, markupProperty2.StringValue);
												continue;
											}
											continue;
										}
									}
									markupProperty2.VerifyOnlySerializableTypes();
									string prefixOf2 = scope.GetPrefixOf(text3);
									string localName = dependencyProperty.OwnerType.Name + "." + dependencyProperty.Name;
									if (string.IsNullOrEmpty(prefixOf2))
									{
										this._writer.WriteAttributeString(localName, markupProperty2.StringValue);
									}
									else
									{
										this._writer.WriteAttributeString(prefixOf2, localName, text3, markupProperty2.StringValue);
									}
								}
							}
							else
							{
								markupProperty2.VerifyOnlySerializableTypes();
								if (xmlLangPropertyAttribute != null && xmlLangPropertyAttribute.Name == markupProperty2.PropertyDescriptor.Name)
								{
									this._writer.WriteAttributeString("xml", "lang", MarkupWriter.NamespaceCache.XmlNamespace, markupProperty2.StringValue);
								}
								else if (uidPropertyAttribute != null && uidPropertyAttribute.Name == markupProperty2.PropertyDescriptor.Name)
								{
									string text5 = scope.MakeAddressable(MarkupWriter.NamespaceCache.XamlNamespace);
									this._writer.WriteAttributeString(scope.GetPrefixOf(text5), markupProperty2.PropertyDescriptor.Name, text5, markupProperty2.StringValue);
								}
								else
								{
									this._writer.WriteAttributeString(markupProperty2.PropertyDescriptor.Name, markupProperty2.StringValue);
								}
								dictionary[markupProperty2.Name] = markupProperty2.Name;
							}
						}
						else
						{
							if (markupProperty2.DependencyProperty != null)
							{
								scope.MakeAddressable(markupProperty2.DependencyProperty.OwnerType);
							}
							if (markupProperty2.IsKey)
							{
								scope.MakeAddressable(MarkupWriter.NamespaceCache.XamlNamespace);
							}
							else if (markupProperty2.IsConstructorArgument)
							{
								scope.MakeAddressable(MarkupWriter.NamespaceCache.XamlNamespace);
								if (list == null)
								{
									list = new List<int>();
								}
								list.Add(++num);
							}
							if (list2 == null)
							{
								list2 = new List<MarkupProperty>();
							}
							list2.Add(markupProperty2);
						}
					}
				}
			}
			foreach (MarkupWriter.Mapping mapping in scope.EnumerateLocalMappings)
			{
				this._writer.WriteAttributeString("xmlns", mapping.Prefix, MarkupWriter.NamespaceCache.XmlnsNamespace, mapping.Uri);
			}
			if (!scope.XmlnsSpacePreserve && markupProperty != null && !this.HasOnlyNormalizationNeutralStrings(markupProperty, false, false))
			{
				this._writer.WriteAttributeString("xml", "space", MarkupWriter.NamespaceCache.XmlNamespace, "preserve");
				scope.XmlnsSpacePreserve = true;
				this._writer.WriteString(string.Empty);
				if (scope.IsTopOfSpacePreservationScope && this._xmlTextWriter != null)
				{
					this._xmlTextWriter.Formatting = Formatting.None;
				}
			}
			if (list2 != null)
			{
				foreach (MarkupProperty markupProperty3 in list2)
				{
					bool flag = false;
					bool flag2 = false;
					foreach (MarkupObject markupObject in markupProperty3.Items)
					{
						if (!flag)
						{
							flag = true;
							if (markupProperty3.IsAttached || markupProperty3.PropertyDescriptor == null)
							{
								if (markupProperty3.IsKey)
								{
									throw new InvalidOperationException(SR.Get("UnserializableKeyValue", new object[]
									{
										markupProperty3.Value.GetType().FullName
									}));
								}
								string uri = scope.MakeAddressable(markupProperty3.DependencyProperty.OwnerType);
								this.WritePropertyStart(scope.GetPrefixOf(uri), markupProperty3.DependencyProperty.OwnerType.Name + "." + markupProperty3.DependencyProperty.Name, uri);
							}
							else
							{
								this.WritePropertyStart(prefixOf, item.ObjectType.Name + "." + markupProperty3.PropertyDescriptor.Name, text);
								dictionary[markupProperty3.Name] = markupProperty3.Name;
							}
							flag2 = this.NeedToWriteExplicitTag(markupProperty3, markupObject);
							if (flag2)
							{
								this.WriteExplicitTagStart(markupProperty3, scope);
							}
						}
						this.WriteItem(markupObject, new MarkupWriter.Scope(scope));
					}
					if (flag)
					{
						if (flag2)
						{
							this.WriteExplicitTagEnd();
						}
						this.WritePropertyEnd();
					}
				}
			}
			if (markupProperty != null)
			{
				if (markupProperty.IsComposite)
				{
					IXmlSerializable xmlSerializable = markupProperty.Value as IXmlSerializable;
					if (xmlSerializable != null)
					{
						this.WriteXmlIsland(xmlSerializable, scope);
						goto IL_8B5;
					}
					bool flag3 = false;
					List<Type> wrapperTypes = this.GetWrapperTypes(markupProperty.PropertyType);
					if (wrapperTypes == null)
					{
						using (IEnumerator<MarkupObject> enumerator5 = markupProperty.Items.GetEnumerator())
						{
							while (enumerator5.MoveNext())
							{
								MarkupObject markupObject2 = enumerator5.Current;
								if (!flag3 && markupObject2.ObjectType == typeof(string) && !this.IsCollectionType(markupProperty.PropertyType) && !this.HasNonValueProperties(markupObject2))
								{
									this._writer.WriteString(this.TextValue(markupObject2));
									flag3 = true;
								}
								else
								{
									this.WriteItem(markupObject2, new MarkupWriter.Scope(scope));
									flag3 = false;
								}
							}
							goto IL_8B5;
						}
					}
					using (IEnumerator<MarkupObject> enumerator6 = markupProperty.Items.GetEnumerator())
					{
						while (enumerator6.MoveNext())
						{
							MarkupObject markupObject3 = enumerator6.Current;
							MarkupProperty wrappedProperty = this.GetWrappedProperty(wrapperTypes, markupObject3);
							if (wrappedProperty == null)
							{
								this.WriteItem(markupObject3, new MarkupWriter.Scope(scope));
								flag3 = false;
							}
							else
							{
								if (wrappedProperty.IsComposite)
								{
									using (IEnumerator<MarkupObject> enumerator7 = wrappedProperty.Items.GetEnumerator())
									{
										while (enumerator7.MoveNext())
										{
											MarkupObject item2 = enumerator7.Current;
											if (!flag3 && markupObject3.ObjectType == typeof(string) && !this.HasNonValueProperties(markupObject3))
											{
												this._writer.WriteString(this.TextValue(item2));
												flag3 = true;
											}
											else
											{
												this.WriteItem(item2, new MarkupWriter.Scope(scope));
												flag3 = false;
											}
										}
										continue;
									}
								}
								if (!flag3)
								{
									this._writer.WriteString(wrappedProperty.StringValue);
									flag3 = true;
								}
								else
								{
									this.WriteItem(markupObject3, new MarkupWriter.Scope(scope));
									flag3 = false;
								}
							}
						}
						goto IL_8B5;
					}
				}
				string text6 = markupProperty.Value as string;
				if (text6 == null)
				{
					text6 = markupProperty.StringValue;
				}
				this._writer.WriteString(text6);
				IL_8B5:
				dictionary[markupProperty.Name] = markupProperty.Name;
			}
			if (partiallyOrderedList != null)
			{
				foreach (MarkupProperty markupProperty4 in partiallyOrderedList)
				{
					if (!dictionary.ContainsKey(markupProperty4.Name))
					{
						dictionary[markupProperty4.Name] = markupProperty4.Name;
						this._writer.WriteStartElement(prefixOf, item.ObjectType.Name + "." + markupProperty4.PropertyDescriptor.Name, text);
						if (markupProperty4.IsComposite || markupProperty4.StringValue.IndexOf("{", StringComparison.Ordinal) == 0)
						{
							using (IEnumerator<MarkupObject> enumerator9 = markupProperty4.Items.GetEnumerator())
							{
								while (enumerator9.MoveNext())
								{
									MarkupObject item3 = enumerator9.Current;
									this.WriteItem(item3, new MarkupWriter.Scope(scope));
								}
								goto IL_9AB;
							}
							goto IL_999;
						}
						goto IL_999;
						IL_9AB:
						this._writer.WriteEndElement();
						continue;
						IL_999:
						this._writer.WriteString(markupProperty4.StringValue);
						goto IL_9AB;
					}
				}
			}
			this._writer.WriteEndElement();
			if (scope.IsTopOfSpacePreservationScope && this._xmlTextWriter != null && this._xmlTextWriter.Formatting != formatting)
			{
				this._xmlTextWriter.Formatting = formatting;
			}
		}

		// Token: 0x060024B0 RID: 9392 RVA: 0x000B20FC File Offset: 0x000B02FC
		private bool IsContentProperty(MarkupProperty property, ContentPropertyAttribute cpa, ref MarkupProperty contentProperty)
		{
			bool flag = property.IsContent;
			if (!flag)
			{
				PropertyDescriptor propertyDescriptor = property.PropertyDescriptor;
				if ((propertyDescriptor != null && typeof(FrameworkTemplate).IsAssignableFrom(propertyDescriptor.ComponentType) && property.Name == "Template") || property.Name == "VisualTree")
				{
					flag = true;
				}
				if (cpa != null && contentProperty == null && propertyDescriptor != null && propertyDescriptor.Name == cpa.Name)
				{
					if (property.IsComposite)
					{
						if (propertyDescriptor == null || propertyDescriptor.IsReadOnly || !typeof(IList).IsAssignableFrom(propertyDescriptor.PropertyType))
						{
							flag = true;
						}
					}
					else if (property.Value != null && !(property.Value is MarkupExtension) && property.PropertyType.IsAssignableFrom(typeof(string)))
					{
						flag = true;
					}
				}
			}
			if (flag)
			{
				contentProperty = property;
			}
			return flag;
		}

		// Token: 0x060024B1 RID: 9393 RVA: 0x000B21E0 File Offset: 0x000B03E0
		private bool IsDeferredProperty(MarkupProperty property, Dictionary<string, string> writtenAttributes, ref MarkupWriter.PartiallyOrderedList<string, MarkupProperty> deferredProperties)
		{
			bool flag = false;
			if (property.PropertyDescriptor != null)
			{
				foreach (object obj in property.Attributes)
				{
					Attribute attribute = (Attribute)obj;
					DependsOnAttribute dependsOnAttribute = attribute as DependsOnAttribute;
					if (dependsOnAttribute != null && !writtenAttributes.ContainsKey(dependsOnAttribute.Name))
					{
						if (deferredProperties == null)
						{
							deferredProperties = new MarkupWriter.PartiallyOrderedList<string, MarkupProperty>();
						}
						deferredProperties.SetOrder(dependsOnAttribute.Name, property.Name);
						flag = true;
					}
				}
				if (flag)
				{
					deferredProperties.Add(property.Name, property);
				}
			}
			return flag;
		}

		// Token: 0x060024B2 RID: 9394 RVA: 0x000B228C File Offset: 0x000B048C
		private bool NeedToWriteExplicitTag(MarkupProperty property, MarkupObject firstItem)
		{
			bool result = false;
			if (property.IsCollectionProperty)
			{
				if (MarkupWriter._nullDefaultValueAttribute == null)
				{
					MarkupWriter._nullDefaultValueAttribute = new DefaultValueAttribute(null);
				}
				if (property.Attributes.Contains(MarkupWriter._nullDefaultValueAttribute))
				{
					result = true;
					object instance = firstItem.Instance;
					if (instance is MarkupExtension)
					{
						if (instance is NullExtension)
						{
							result = false;
						}
						else if (property.PropertyType.IsArray)
						{
							ArrayExtension arrayExtension = instance as ArrayExtension;
							if (property.PropertyType.IsAssignableFrom(arrayExtension.Type.MakeArrayType()))
							{
								result = false;
							}
						}
					}
					else if (property.PropertyType.IsAssignableFrom(firstItem.ObjectType))
					{
						result = false;
					}
				}
			}
			return result;
		}

		// Token: 0x060024B3 RID: 9395 RVA: 0x000B2330 File Offset: 0x000B0530
		private void WriteExplicitTagStart(MarkupProperty property, MarkupWriter.Scope scope)
		{
			Type type = property.Value.GetType();
			string text = scope.MakeAddressable(type);
			string prefixOf = scope.GetPrefixOf(text);
			string text2 = type.Name;
			if (typeof(MarkupExtension).IsAssignableFrom(type) && text2.EndsWith("Extension", StringComparison.Ordinal))
			{
				text2 = text2.Substring(0, text2.Length - 9);
			}
			this._writer.WriteStartElement(prefixOf, text2, text);
		}

		// Token: 0x060024B4 RID: 9396 RVA: 0x000B239F File Offset: 0x000B059F
		private void WriteExplicitTagEnd()
		{
			this._writer.WriteEndElement();
		}

		// Token: 0x060024B5 RID: 9397 RVA: 0x000B23AC File Offset: 0x000B05AC
		private void WritePropertyStart(string prefix, string propertyName, string uri)
		{
			this._writer.WriteStartElement(prefix, propertyName, uri);
		}

		// Token: 0x060024B6 RID: 9398 RVA: 0x000B239F File Offset: 0x000B059F
		private void WritePropertyEnd()
		{
			this._writer.WriteEndElement();
		}

		// Token: 0x060024B7 RID: 9399 RVA: 0x000B23BC File Offset: 0x000B05BC
		private void WriteXmlIsland(IXmlSerializable xmlSerializable, MarkupWriter.Scope scope)
		{
			scope.MakeAddressable(MarkupWriter.NamespaceCache.XamlNamespace);
			this._writer.WriteStartElement(scope.GetPrefixOf(MarkupWriter.NamespaceCache.XamlNamespace), "XData", MarkupWriter.NamespaceCache.XamlNamespace);
			xmlSerializable.WriteXml(this._writer);
			this._writer.WriteEndElement();
		}

		// Token: 0x060024B8 RID: 9400 RVA: 0x000B240C File Offset: 0x000B060C
		private List<Type> GetWrapperTypes(Type type)
		{
			AttributeCollection attributes = TypeDescriptor.GetAttributes(type);
			if (attributes[typeof(ContentWrapperAttribute)] == null)
			{
				return null;
			}
			List<Type> list = new List<Type>();
			foreach (object obj in attributes)
			{
				Attribute attribute = (Attribute)obj;
				ContentWrapperAttribute contentWrapperAttribute = attribute as ContentWrapperAttribute;
				if (contentWrapperAttribute != null)
				{
					list.Add(contentWrapperAttribute.ContentWrapper);
				}
			}
			return list;
		}

		// Token: 0x060024B9 RID: 9401 RVA: 0x000B2498 File Offset: 0x000B0698
		private MarkupProperty GetWrappedProperty(List<Type> wrapperTypes, MarkupObject item)
		{
			if (!this.IsInTypes(item.ObjectType, wrapperTypes))
			{
				return null;
			}
			ContentPropertyAttribute contentPropertyAttribute = item.Attributes[typeof(ContentPropertyAttribute)] as ContentPropertyAttribute;
			MarkupProperty result = null;
			foreach (MarkupProperty markupProperty in item.Properties)
			{
				if (!markupProperty.IsContent && (contentPropertyAttribute == null || markupProperty.PropertyDescriptor == null || !(markupProperty.PropertyDescriptor.Name == contentPropertyAttribute.Name)))
				{
					result = null;
					break;
				}
				result = markupProperty;
			}
			return result;
		}

		// Token: 0x060024BA RID: 9402 RVA: 0x000B2540 File Offset: 0x000B0740
		private bool IsInTypes(Type type, List<Type> types)
		{
			foreach (Type left in types)
			{
				if (left == type)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060024BB RID: 9403 RVA: 0x000B2598 File Offset: 0x000B0798
		private string TextValue(MarkupObject item)
		{
			using (IEnumerator<MarkupProperty> enumerator = item.Properties.GetEnumerator())
			{
				if (enumerator.MoveNext())
				{
					MarkupProperty markupProperty = enumerator.Current;
					if (markupProperty.IsValueAsString)
					{
						return markupProperty.StringValue;
					}
				}
			}
			return null;
		}

		// Token: 0x060024BC RID: 9404 RVA: 0x000B25F4 File Offset: 0x000B07F4
		private bool HasNonValueProperties(MarkupObject item)
		{
			foreach (MarkupProperty markupProperty in item.Properties)
			{
				if (!markupProperty.IsValueAsString)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060024BD RID: 9405 RVA: 0x000B264C File Offset: 0x000B084C
		private bool IsCollectionType(Type type)
		{
			return typeof(IEnumerable).IsAssignableFrom(type) || typeof(Array).IsAssignableFrom(type);
		}

		// Token: 0x060024BE RID: 9406 RVA: 0x000B2674 File Offset: 0x000B0874
		private bool HasOnlyNormalizationNeutralStrings(MarkupProperty contentProperty, bool keepLeadingSpace, bool keepTrailingSpace)
		{
			if (!contentProperty.IsComposite)
			{
				return this.IsNormalizationNeutralString(contentProperty.StringValue, keepLeadingSpace, keepTrailingSpace);
			}
			bool flag = true;
			bool flag2 = !keepLeadingSpace;
			bool flag3 = !keepLeadingSpace;
			string text = null;
			MarkupProperty markupProperty = null;
			List<Type> wrapperTypes = this.GetWrapperTypes(contentProperty.PropertyType);
			foreach (MarkupObject markupObject in contentProperty.Items)
			{
				flag3 = flag2;
				flag2 = this.ShouldTrimSurroundingWhitespace(markupObject);
				if (text != null)
				{
					flag = this.IsNormalizationNeutralString(text, !flag3, !flag2);
					text = null;
					if (!flag)
					{
						return false;
					}
				}
				if (markupProperty != null)
				{
					flag = this.HasOnlyNormalizationNeutralStrings(markupProperty, !flag3, !flag2);
					markupProperty = null;
					if (!flag)
					{
						return false;
					}
				}
				if (markupObject.ObjectType == typeof(string))
				{
					text = this.TextValue(markupObject);
					if (text != null)
					{
						continue;
					}
				}
				if (wrapperTypes != null)
				{
					MarkupProperty wrappedProperty = this.GetWrappedProperty(wrapperTypes, markupObject);
					if (wrappedProperty != null)
					{
						markupProperty = wrappedProperty;
					}
				}
			}
			if (text != null)
			{
				flag = this.IsNormalizationNeutralString(text, !flag3, keepTrailingSpace);
			}
			else if (markupProperty != null)
			{
				flag = this.HasOnlyNormalizationNeutralStrings(markupProperty, !flag3, keepTrailingSpace);
			}
			return flag;
		}

		// Token: 0x060024BF RID: 9407 RVA: 0x000B27AC File Offset: 0x000B09AC
		private bool ShouldTrimSurroundingWhitespace(MarkupObject item)
		{
			TrimSurroundingWhitespaceAttribute trimSurroundingWhitespaceAttribute = item.Attributes[typeof(TrimSurroundingWhitespaceAttribute)] as TrimSurroundingWhitespaceAttribute;
			return trimSurroundingWhitespaceAttribute != null;
		}

		// Token: 0x060024C0 RID: 9408 RVA: 0x000B27D8 File Offset: 0x000B09D8
		private bool IsNormalizationNeutralString(string value, bool keepLeadingSpace, bool keepTrailingSpace)
		{
			bool flag = !keepLeadingSpace;
			int i = 0;
			while (i < value.Length)
			{
				char c = value[i];
				switch (c)
				{
				case '\t':
				case '\n':
				case '\f':
				case '\r':
					return false;
				case '\v':
					goto IL_3E;
				default:
					if (c != ' ')
					{
						goto IL_3E;
					}
					if (flag)
					{
						return false;
					}
					flag = true;
					break;
				}
				IL_40:
				i++;
				continue;
				IL_3E:
				flag = false;
				goto IL_40;
			}
			return !flag || keepTrailingSpace;
		}

		// Token: 0x04001B41 RID: 6977
		private const string clrUriPrefix = "clr-namespace:";

		// Token: 0x04001B42 RID: 6978
		private const int EXTENSIONLENGTH = 9;

		// Token: 0x04001B43 RID: 6979
		private XmlWriter _writer;

		// Token: 0x04001B44 RID: 6980
		private XmlTextWriter _xmlTextWriter;

		// Token: 0x04001B45 RID: 6981
		private static DefaultValueAttribute _nullDefaultValueAttribute;

		// Token: 0x020008AE RID: 2222
		private class PartiallyOrderedList<TKey, TValue> : IEnumerable<TValue>, IEnumerable where TValue : class
		{
			// Token: 0x0600840C RID: 33804 RVA: 0x00247638 File Offset: 0x00245838
			public void Add(TKey key, TValue value)
			{
				MarkupWriter.PartiallyOrderedList<TKey, TValue>.Entry entry = new MarkupWriter.PartiallyOrderedList<TKey, TValue>.Entry(key, value);
				int num = this._entries.IndexOf(entry);
				if (num >= 0)
				{
					entry.Predecessors = this._entries[num].Predecessors;
					this._entries[num] = entry;
					return;
				}
				this._entries.Add(entry);
			}

			// Token: 0x0600840D RID: 33805 RVA: 0x00247690 File Offset: 0x00245890
			private int GetEntryIndex(TKey key)
			{
				MarkupWriter.PartiallyOrderedList<TKey, TValue>.Entry item = new MarkupWriter.PartiallyOrderedList<TKey, TValue>.Entry(key, default(TValue));
				int num = this._entries.IndexOf(item);
				if (num < 0)
				{
					num = this._entries.Count;
					this._entries.Add(item);
				}
				return num;
			}

			// Token: 0x0600840E RID: 33806 RVA: 0x002476D8 File Offset: 0x002458D8
			public void SetOrder(TKey predecessor, TKey key)
			{
				int entryIndex = this.GetEntryIndex(predecessor);
				MarkupWriter.PartiallyOrderedList<TKey, TValue>.Entry entry = this._entries[entryIndex];
				int entryIndex2 = this.GetEntryIndex(key);
				MarkupWriter.PartiallyOrderedList<TKey, TValue>.Entry entry2 = this._entries[entryIndex2];
				if (entry2.Predecessors == null)
				{
					entry2.Predecessors = new List<int>();
				}
				entry2.Predecessors.Add(entryIndex);
				this._firstIndex = -1;
			}

			// Token: 0x0600840F RID: 33807 RVA: 0x00247738 File Offset: 0x00245938
			private void TopologicalSort()
			{
				this._firstIndex = -1;
				this._lastIndex = -1;
				for (int i = 0; i < this._entries.Count; i++)
				{
					this._entries[i].Link = -1;
				}
				for (int j = 0; j < this._entries.Count; j++)
				{
					this.DepthFirstSearch(j);
				}
			}

			// Token: 0x06008410 RID: 33808 RVA: 0x00247798 File Offset: 0x00245998
			private void DepthFirstSearch(int index)
			{
				if (this._entries[index].Link == -1)
				{
					this._entries[index].Link = -2;
					if (this._entries[index].Predecessors != null)
					{
						foreach (int index2 in this._entries[index].Predecessors)
						{
							this.DepthFirstSearch(index2);
						}
					}
					if (this._lastIndex == -1)
					{
						this._firstIndex = index;
					}
					else
					{
						this._entries[this._lastIndex].Link = index;
					}
					this._lastIndex = index;
				}
			}

			// Token: 0x06008411 RID: 33809 RVA: 0x00247864 File Offset: 0x00245A64
			public IEnumerator<TValue> GetEnumerator()
			{
				if (this._firstIndex < 0)
				{
					this.TopologicalSort();
				}
				int i = this._firstIndex;
				while (i >= 0)
				{
					MarkupWriter.PartiallyOrderedList<TKey, TValue>.Entry entry = this._entries[i];
					if (entry.Value != null)
					{
						yield return entry.Value;
					}
					i = entry.Link;
					entry = null;
				}
				yield break;
			}

			// Token: 0x06008412 RID: 33810 RVA: 0x00247873 File Offset: 0x00245A73
			IEnumerator IEnumerable.GetEnumerator()
			{
				foreach (TValue tvalue in this)
				{
					yield return tvalue;
				}
				IEnumerator<TValue> enumerator = null;
				yield break;
				yield break;
			}

			// Token: 0x040041F4 RID: 16884
			private List<MarkupWriter.PartiallyOrderedList<TKey, TValue>.Entry> _entries = new List<MarkupWriter.PartiallyOrderedList<TKey, TValue>.Entry>();

			// Token: 0x040041F5 RID: 16885
			private int _firstIndex = -1;

			// Token: 0x040041F6 RID: 16886
			private int _lastIndex;

			// Token: 0x02000BA0 RID: 2976
			private class Entry
			{
				// Token: 0x06009187 RID: 37255 RVA: 0x0025EA8A File Offset: 0x0025CC8A
				public Entry(TKey key, TValue value)
				{
					this.Key = key;
					this.Value = value;
					this.Predecessors = null;
					this.Link = 0;
				}

				// Token: 0x06009188 RID: 37256 RVA: 0x0025EAB0 File Offset: 0x0025CCB0
				public override bool Equals(object obj)
				{
					MarkupWriter.PartiallyOrderedList<TKey, TValue>.Entry entry = obj as MarkupWriter.PartiallyOrderedList<TKey, TValue>.Entry;
					if (entry != null)
					{
						TKey key = entry.Key;
						return key.Equals(this.Key);
					}
					return false;
				}

				// Token: 0x06009189 RID: 37257 RVA: 0x0025EAE8 File Offset: 0x0025CCE8
				public override int GetHashCode()
				{
					TKey key = this.Key;
					return key.GetHashCode();
				}

				// Token: 0x04004EA4 RID: 20132
				public readonly TKey Key;

				// Token: 0x04004EA5 RID: 20133
				public readonly TValue Value;

				// Token: 0x04004EA6 RID: 20134
				public List<int> Predecessors;

				// Token: 0x04004EA7 RID: 20135
				public int Link;

				// Token: 0x04004EA8 RID: 20136
				public const int UNSEEN = -1;

				// Token: 0x04004EA9 RID: 20137
				public const int INDFS = -2;
			}
		}

		// Token: 0x020008AF RID: 2223
		private class Mapping
		{
			// Token: 0x06008414 RID: 33812 RVA: 0x0024789C File Offset: 0x00245A9C
			public Mapping(string uri, string prefix)
			{
				this.Uri = uri;
				this.Prefix = prefix;
			}

			// Token: 0x06008415 RID: 33813 RVA: 0x002478B4 File Offset: 0x00245AB4
			public override bool Equals(object obj)
			{
				MarkupWriter.Mapping mapping = obj as MarkupWriter.Mapping;
				return mapping != null && this.Uri.Equals(mapping.Uri) && this.Prefix.Equals(mapping.Prefix);
			}

			// Token: 0x06008416 RID: 33814 RVA: 0x002478F1 File Offset: 0x00245AF1
			public override int GetHashCode()
			{
				return this.Uri.GetHashCode() + this.Prefix.GetHashCode();
			}

			// Token: 0x040041F7 RID: 16887
			public readonly string Uri;

			// Token: 0x040041F8 RID: 16888
			public readonly string Prefix;
		}

		// Token: 0x020008B0 RID: 2224
		private class Scope
		{
			// Token: 0x06008417 RID: 33815 RVA: 0x0024790A File Offset: 0x00245B0A
			public Scope(MarkupWriter.Scope containingScope)
			{
				this._containingScope = containingScope;
			}

			// Token: 0x17001DED RID: 7661
			// (get) Token: 0x06008418 RID: 33816 RVA: 0x00247919 File Offset: 0x00245B19
			// (set) Token: 0x06008419 RID: 33817 RVA: 0x00247949 File Offset: 0x00245B49
			public bool XmlnsSpacePreserve
			{
				get
				{
					if (this._xmlnsSpacePreserve != null)
					{
						return this._xmlnsSpacePreserve.Value;
					}
					return this._containingScope != null && this._containingScope.XmlnsSpacePreserve;
				}
				set
				{
					this._xmlnsSpacePreserve = new bool?(value);
				}
			}

			// Token: 0x17001DEE RID: 7662
			// (get) Token: 0x0600841A RID: 33818 RVA: 0x00247957 File Offset: 0x00245B57
			public bool IsTopOfSpacePreservationScope
			{
				get
				{
					return this._containingScope == null || (this._xmlnsSpacePreserve != null && this._xmlnsSpacePreserve.Value != this._containingScope.XmlnsSpacePreserve);
				}
			}

			// Token: 0x0600841B RID: 33819 RVA: 0x00247990 File Offset: 0x00245B90
			public string GetPrefixOf(string uri)
			{
				string result;
				if (this._uriToPrefix != null && this._uriToPrefix.TryGetValue(uri, out result))
				{
					return result;
				}
				if (this._containingScope != null)
				{
					return this._containingScope.GetPrefixOf(uri);
				}
				return null;
			}

			// Token: 0x0600841C RID: 33820 RVA: 0x002479D0 File Offset: 0x00245BD0
			public string GetUriOf(string prefix)
			{
				string result;
				if (this._prefixToUri != null && this._prefixToUri.TryGetValue(prefix, out result))
				{
					return result;
				}
				if (this._containingScope != null)
				{
					return this._containingScope.GetUriOf(prefix);
				}
				return null;
			}

			// Token: 0x0600841D RID: 33821 RVA: 0x00247A10 File Offset: 0x00245C10
			public void RecordMapping(string prefix, string uri)
			{
				if (this._uriToPrefix == null)
				{
					this._uriToPrefix = new Dictionary<string, string>();
				}
				if (this._prefixToUri == null)
				{
					this._prefixToUri = new Dictionary<string, string>();
				}
				this._uriToPrefix[uri] = prefix;
				this._prefixToUri[prefix] = uri;
			}

			// Token: 0x0600841E RID: 33822 RVA: 0x00247A60 File Offset: 0x00245C60
			public void MakeAddressable(IEnumerable<Type> types)
			{
				if (types != null)
				{
					foreach (Type type in types)
					{
						this.MakeAddressable(type);
					}
				}
			}

			// Token: 0x0600841F RID: 33823 RVA: 0x00247AAC File Offset: 0x00245CAC
			public string MakeAddressable(Type type)
			{
				return this.MakeAddressable(MarkupWriter.NamespaceCache.GetNamespaceUriFor(type));
			}

			// Token: 0x06008420 RID: 33824 RVA: 0x00247ABC File Offset: 0x00245CBC
			public string MakeAddressable(string uri)
			{
				if (this.GetPrefixOf(uri) == null)
				{
					string defaultPrefixFor = MarkupWriter.NamespaceCache.GetDefaultPrefixFor(uri);
					string prefix = defaultPrefixFor;
					int num = 0;
					while (this.GetUriOf(prefix) != null)
					{
						prefix = defaultPrefixFor + num++;
					}
					this.RecordMapping(prefix, uri);
				}
				return uri;
			}

			// Token: 0x17001DEF RID: 7663
			// (get) Token: 0x06008421 RID: 33825 RVA: 0x00247B04 File Offset: 0x00245D04
			public IEnumerable<MarkupWriter.Mapping> EnumerateLocalMappings
			{
				get
				{
					if (this._uriToPrefix != null)
					{
						foreach (KeyValuePair<string, string> keyValuePair in this._uriToPrefix)
						{
							yield return new MarkupWriter.Mapping(keyValuePair.Key, keyValuePair.Value);
						}
						Dictionary<string, string>.Enumerator enumerator = default(Dictionary<string, string>.Enumerator);
					}
					yield break;
					yield break;
				}
			}

			// Token: 0x17001DF0 RID: 7664
			// (get) Token: 0x06008422 RID: 33826 RVA: 0x00247B24 File Offset: 0x00245D24
			public IEnumerable<MarkupWriter.Mapping> EnumerateAllMappings
			{
				get
				{
					IEnumerator<MarkupWriter.Mapping> enumerator;
					if (this._containingScope != null)
					{
						foreach (MarkupWriter.Mapping mapping in this._containingScope.EnumerateAllMappings)
						{
							yield return mapping;
						}
						enumerator = null;
					}
					foreach (MarkupWriter.Mapping mapping2 in this.EnumerateLocalMappings)
					{
						yield return mapping2;
					}
					enumerator = null;
					yield break;
					yield break;
				}
			}

			// Token: 0x040041F9 RID: 16889
			private MarkupWriter.Scope _containingScope;

			// Token: 0x040041FA RID: 16890
			private bool? _xmlnsSpacePreserve;

			// Token: 0x040041FB RID: 16891
			private Dictionary<string, string> _uriToPrefix;

			// Token: 0x040041FC RID: 16892
			private Dictionary<string, string> _prefixToUri;
		}

		// Token: 0x020008B1 RID: 2225
		private class MarkupWriterContext : IValueSerializerContext, ITypeDescriptorContext, IServiceProvider
		{
			// Token: 0x06008423 RID: 33827 RVA: 0x00247B41 File Offset: 0x00245D41
			internal MarkupWriterContext(MarkupWriter.Scope scope)
			{
				this._scope = scope;
			}

			// Token: 0x06008424 RID: 33828 RVA: 0x00247B50 File Offset: 0x00245D50
			public ValueSerializer GetValueSerializerFor(PropertyDescriptor descriptor)
			{
				if (descriptor.PropertyType == typeof(Type))
				{
					return new MarkupWriter.TypeValueSerializer(this._scope);
				}
				return ValueSerializer.GetSerializerFor(descriptor);
			}

			// Token: 0x06008425 RID: 33829 RVA: 0x00247B7B File Offset: 0x00245D7B
			public ValueSerializer GetValueSerializerFor(Type type)
			{
				if (type == typeof(Type))
				{
					return new MarkupWriter.TypeValueSerializer(this._scope);
				}
				return ValueSerializer.GetSerializerFor(type);
			}

			// Token: 0x17001DF1 RID: 7665
			// (get) Token: 0x06008426 RID: 33830 RVA: 0x0000C238 File Offset: 0x0000A438
			public IContainer Container
			{
				get
				{
					return null;
				}
			}

			// Token: 0x17001DF2 RID: 7666
			// (get) Token: 0x06008427 RID: 33831 RVA: 0x0000C238 File Offset: 0x0000A438
			public object Instance
			{
				get
				{
					return null;
				}
			}

			// Token: 0x06008428 RID: 33832 RVA: 0x00002137 File Offset: 0x00000337
			public void OnComponentChanged()
			{
			}

			// Token: 0x06008429 RID: 33833 RVA: 0x00016748 File Offset: 0x00014948
			public bool OnComponentChanging()
			{
				return true;
			}

			// Token: 0x17001DF3 RID: 7667
			// (get) Token: 0x0600842A RID: 33834 RVA: 0x0000C238 File Offset: 0x0000A438
			public PropertyDescriptor PropertyDescriptor
			{
				get
				{
					return null;
				}
			}

			// Token: 0x0600842B RID: 33835 RVA: 0x0000C238 File Offset: 0x0000A438
			public object GetService(Type serviceType)
			{
				return null;
			}

			// Token: 0x040041FD RID: 16893
			private MarkupWriter.Scope _scope;
		}

		// Token: 0x020008B2 RID: 2226
		private class TypeValueSerializer : ValueSerializer
		{
			// Token: 0x0600842C RID: 33836 RVA: 0x00247BA1 File Offset: 0x00245DA1
			public TypeValueSerializer(MarkupWriter.Scope scope)
			{
				this._scope = scope;
			}

			// Token: 0x0600842D RID: 33837 RVA: 0x00016748 File Offset: 0x00014948
			public override bool CanConvertToString(object value, IValueSerializerContext context)
			{
				return true;
			}

			// Token: 0x0600842E RID: 33838 RVA: 0x00247BB0 File Offset: 0x00245DB0
			public override string ConvertToString(object value, IValueSerializerContext context)
			{
				Type type = value as Type;
				if (type == null)
				{
					throw new InvalidOperationException();
				}
				string uri = this._scope.MakeAddressable(type);
				string prefixOf = this._scope.GetPrefixOf(uri);
				if (prefixOf == null || prefixOf == "")
				{
					return type.Name;
				}
				return prefixOf + ":" + type.Name;
			}

			// Token: 0x0600842F RID: 33839 RVA: 0x00247C18 File Offset: 0x00245E18
			public override IEnumerable<Type> TypeReferences(object value, IValueSerializerContext context)
			{
				Type type = value as Type;
				if (type != null)
				{
					return new Type[]
					{
						type
					};
				}
				return base.TypeReferences(value, context);
			}

			// Token: 0x040041FE RID: 16894
			private MarkupWriter.Scope _scope;
		}

		// Token: 0x020008B3 RID: 2227
		private static class NamespaceCache
		{
			// Token: 0x06008430 RID: 33840 RVA: 0x00247C48 File Offset: 0x00245E48
			private static Dictionary<string, string> GetMappingsFor(Assembly assembly)
			{
				object syncObject = MarkupWriter.NamespaceCache.SyncObject;
				Dictionary<string, string> dictionary;
				lock (syncObject)
				{
					if (!MarkupWriter.NamespaceCache.XmlnsDefinitions.TryGetValue(assembly, out dictionary))
					{
						foreach (XmlnsPrefixAttribute xmlnsPrefixAttribute in assembly.GetCustomAttributes(typeof(XmlnsPrefixAttribute), true))
						{
							MarkupWriter.NamespaceCache.DefaultPrefixes[xmlnsPrefixAttribute.XmlNamespace] = xmlnsPrefixAttribute.Prefix;
						}
						dictionary = new Dictionary<string, string>();
						MarkupWriter.NamespaceCache.XmlnsDefinitions[assembly] = dictionary;
						object[] customAttributes2 = assembly.GetCustomAttributes(typeof(XmlnsDefinitionAttribute), true);
						foreach (XmlnsDefinitionAttribute xmlnsDefinitionAttribute in customAttributes2)
						{
							if (xmlnsDefinitionAttribute.AssemblyName == null)
							{
								string text = null;
								string text2 = null;
								string text3 = null;
								if (dictionary.TryGetValue(xmlnsDefinitionAttribute.ClrNamespace, out text) && MarkupWriter.NamespaceCache.DefaultPrefixes.TryGetValue(text, out text2))
								{
									MarkupWriter.NamespaceCache.DefaultPrefixes.TryGetValue(xmlnsDefinitionAttribute.XmlNamespace, out text3);
								}
								if (text == null || text2 == null || (text3 != null && text2.Length > text3.Length))
								{
									dictionary[xmlnsDefinitionAttribute.ClrNamespace] = xmlnsDefinitionAttribute.XmlNamespace;
								}
							}
							else
							{
								Assembly assembly2 = Assembly.Load(new AssemblyName(xmlnsDefinitionAttribute.AssemblyName));
								if (assembly2 != null)
								{
									Dictionary<string, string> mappingsFor = MarkupWriter.NamespaceCache.GetMappingsFor(assembly2);
									mappingsFor[xmlnsDefinitionAttribute.ClrNamespace] = xmlnsDefinitionAttribute.XmlNamespace;
								}
							}
						}
					}
				}
				return dictionary;
			}

			// Token: 0x06008431 RID: 33841 RVA: 0x00247DEC File Offset: 0x00245FEC
			public static string GetNamespaceUriFor(Type type)
			{
				object syncObject = MarkupWriter.NamespaceCache.SyncObject;
				string result;
				lock (syncObject)
				{
					if (type.Namespace == null)
					{
						result = string.Format(CultureInfo.InvariantCulture, "clr-namespace:;assembly={0}", new object[]
						{
							type.Assembly.GetName().Name
						});
					}
					else
					{
						Dictionary<string, string> mappingsFor = MarkupWriter.NamespaceCache.GetMappingsFor(type.Assembly);
						if (!mappingsFor.TryGetValue(type.Namespace, out result))
						{
							result = string.Format(CultureInfo.InvariantCulture, "clr-namespace:{0};assembly={1}", new object[]
							{
								type.Namespace,
								type.Assembly.GetName().Name
							});
						}
					}
				}
				return result;
			}

			// Token: 0x06008432 RID: 33842 RVA: 0x00247EA8 File Offset: 0x002460A8
			public static string GetDefaultPrefixFor(string uri)
			{
				object syncObject = MarkupWriter.NamespaceCache.SyncObject;
				string text;
				lock (syncObject)
				{
					MarkupWriter.NamespaceCache.DefaultPrefixes.TryGetValue(uri, out text);
					if (text == null)
					{
						text = "assembly";
						if (uri.StartsWith("clr-namespace:", StringComparison.Ordinal))
						{
							string text2 = uri.Substring("clr-namespace:".Length, uri.IndexOf(";", StringComparison.Ordinal) - "clr-namespace:".Length);
							StringBuilder stringBuilder = new StringBuilder();
							foreach (char c in text2)
							{
								if (c >= 'A' && c <= 'Z')
								{
									stringBuilder.Append(c.ToString().ToLower(CultureInfo.InvariantCulture));
								}
							}
							if (stringBuilder.Length > 0)
							{
								text = stringBuilder.ToString();
							}
						}
					}
				}
				return text;
			}

			// Token: 0x040041FF RID: 16895
			private static Dictionary<Assembly, Dictionary<string, string>> XmlnsDefinitions = new Dictionary<Assembly, Dictionary<string, string>>();

			// Token: 0x04004200 RID: 16896
			private static Dictionary<string, string> DefaultPrefixes = new Dictionary<string, string>();

			// Token: 0x04004201 RID: 16897
			private static object SyncObject = new object();

			// Token: 0x04004202 RID: 16898
			public static string XamlNamespace = "http://schemas.microsoft.com/winfx/2006/xaml";

			// Token: 0x04004203 RID: 16899
			public static string XmlNamespace = "http://www.w3.org/XML/1998/namespace";

			// Token: 0x04004204 RID: 16900
			public static string XmlnsNamespace = "http://www.w3.org/2000/xmlns/";
		}
	}
}
