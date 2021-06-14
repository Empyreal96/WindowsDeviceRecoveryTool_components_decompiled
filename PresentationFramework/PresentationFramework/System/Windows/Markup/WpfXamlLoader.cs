using System;
using System.Collections;
using System.Windows.Baml2006;
using System.Windows.Data;
using System.Windows.Diagnostics;
using System.Xaml;
using System.Xaml.Permissions;
using MS.Internal;
using MS.Internal.Xaml.Context;
using MS.Utility;

namespace System.Windows.Markup
{
	// Token: 0x02000232 RID: 562
	internal class WpfXamlLoader
	{
		// Token: 0x06002267 RID: 8807 RVA: 0x000AAE64 File Offset: 0x000A9064
		public static object Load(XamlReader xamlReader, bool skipJournaledProperties, Uri baseUri)
		{
			XamlObjectWriterSettings settings = XamlReader.CreateObjectWriterSettings();
			object obj = WpfXamlLoader.Load(xamlReader, null, skipJournaledProperties, null, settings, baseUri);
			WpfXamlLoader.EnsureXmlNamespaceMaps(obj, xamlReader.SchemaContext);
			return obj;
		}

		// Token: 0x06002268 RID: 8808 RVA: 0x000AAE90 File Offset: 0x000A9090
		public static object LoadDeferredContent(XamlReader xamlReader, IXamlObjectWriterFactory writerFactory, bool skipJournaledProperties, object rootObject, XamlObjectWriterSettings parentSettings, Uri baseUri)
		{
			XamlObjectWriterSettings settings = XamlReader.CreateObjectWriterSettings(parentSettings);
			return WpfXamlLoader.Load(xamlReader, writerFactory, skipJournaledProperties, rootObject, settings, baseUri);
		}

		// Token: 0x06002269 RID: 8809 RVA: 0x000AAEB4 File Offset: 0x000A90B4
		public static object LoadBaml(XamlReader xamlReader, bool skipJournaledProperties, object rootObject, XamlAccessLevel accessLevel, Uri baseUri)
		{
			XamlObjectWriterSettings xamlObjectWriterSettings = XamlReader.CreateObjectWriterSettingsForBaml();
			xamlObjectWriterSettings.RootObjectInstance = rootObject;
			xamlObjectWriterSettings.AccessLevel = accessLevel;
			object obj = WpfXamlLoader.Load(xamlReader, null, skipJournaledProperties, rootObject, xamlObjectWriterSettings, baseUri);
			WpfXamlLoader.EnsureXmlNamespaceMaps(obj, xamlReader.SchemaContext);
			return obj;
		}

		// Token: 0x0600226A RID: 8810 RVA: 0x000AAEF0 File Offset: 0x000A90F0
		internal static void EnsureXmlNamespaceMaps(object rootObject, XamlSchemaContext schemaContext)
		{
			DependencyObject dependencyObject = rootObject as DependencyObject;
			if (dependencyObject == null)
			{
				return;
			}
			XamlTypeMapper.XamlTypeMapperSchemaContext xamlTypeMapperSchemaContext = schemaContext as XamlTypeMapper.XamlTypeMapperSchemaContext;
			Hashtable value;
			if (xamlTypeMapperSchemaContext == null)
			{
				value = new Hashtable();
			}
			else
			{
				value = xamlTypeMapperSchemaContext.GetNamespaceMapHashList();
			}
			dependencyObject.SetValue(XmlAttributeProperties.XmlNamespaceMapsProperty, value);
		}

		// Token: 0x0600226B RID: 8811 RVA: 0x000AAF30 File Offset: 0x000A9130
		private static object Load(XamlReader xamlReader, IXamlObjectWriterFactory writerFactory, bool skipJournaledProperties, object rootObject, XamlObjectWriterSettings settings, Uri baseUri)
		{
			XamlContextStack<WpfXamlFrame> stack = new XamlContextStack<WpfXamlFrame>(() => new WpfXamlFrame());
			int persistId = 1;
			settings.AfterBeginInitHandler = delegate(object sender, XamlObjectEventArgs args)
			{
				if (EventTrace.IsEnabled(EventTrace.Keyword.KeywordPerf | EventTrace.Keyword.KeywordXamlBaml, EventTrace.Level.Verbose))
				{
					IXamlLineInfo xamlLineInfo2 = xamlReader as IXamlLineInfo;
					int num = -1;
					int num2 = -1;
					if (xamlLineInfo2 != null && xamlLineInfo2.HasLineInfo)
					{
						num = xamlLineInfo2.LineNumber;
						num2 = xamlLineInfo2.LinePosition;
					}
					EventTrace.EventProvider.TraceEvent(EventTrace.Event.WClientParseXamlBamlInfo, EventTrace.Keyword.KeywordPerf | EventTrace.Keyword.KeywordXamlBaml, EventTrace.Level.Verbose, new object[]
					{
						(args.Instance == null) ? 0L : PerfService.GetPerfElementID(args.Instance),
						num,
						num2
					});
				}
				UIElement uielement = args.Instance as UIElement;
				if (uielement != null)
				{
					UIElement uielement2 = uielement;
					int persistId = persistId;
					persistId++;
					uielement2.SetPersistId(persistId);
				}
				XamlSourceInfoHelper.SetXamlSourceInfo(args.Instance, args, baseUri);
				DependencyObject dependencyObject = args.Instance as DependencyObject;
				if (dependencyObject != null && stack.CurrentFrame.XmlnsDictionary != null)
				{
					XmlnsDictionary xmlnsDictionary = stack.CurrentFrame.XmlnsDictionary;
					xmlnsDictionary.Seal();
					XmlAttributeProperties.SetXmlnsDictionary(dependencyObject, xmlnsDictionary);
				}
				stack.CurrentFrame.Instance = args.Instance;
			};
			XamlObjectWriter xamlObjectWriter;
			if (writerFactory != null)
			{
				xamlObjectWriter = writerFactory.GetXamlObjectWriter(settings);
			}
			else
			{
				xamlObjectWriter = new XamlObjectWriter(xamlReader.SchemaContext, settings);
			}
			IXamlLineInfo xamlLineInfo = null;
			object result;
			try
			{
				xamlLineInfo = (xamlReader as IXamlLineInfo);
				IXamlLineInfoConsumer xamlLineInfoConsumer = xamlObjectWriter;
				bool shouldPassLineNumberInfo = false;
				if (xamlLineInfo != null && xamlLineInfo.HasLineInfo && xamlLineInfoConsumer != null && xamlLineInfoConsumer.ShouldProvideLineInfo)
				{
					shouldPassLineNumberInfo = true;
				}
				IStyleConnector styleConnector = rootObject as IStyleConnector;
				WpfXamlLoader.TransformNodes(xamlReader, xamlObjectWriter, false, skipJournaledProperties, shouldPassLineNumberInfo, xamlLineInfo, xamlLineInfoConsumer, stack, styleConnector);
				xamlObjectWriter.Close();
				result = xamlObjectWriter.Result;
			}
			catch (Exception ex)
			{
				if (CriticalExceptions.IsCriticalException(ex) || !XamlReader.ShouldReWrapException(ex, baseUri))
				{
					throw;
				}
				XamlReader.RewrapException(ex, xamlLineInfo, baseUri);
				result = null;
			}
			return result;
		}

		// Token: 0x0600226C RID: 8812 RVA: 0x000AB058 File Offset: 0x000A9258
		internal static void TransformNodes(XamlReader xamlReader, XamlObjectWriter xamlWriter, bool onlyLoadOneNode, bool skipJournaledProperties, bool shouldPassLineNumberInfo, IXamlLineInfo xamlLineInfo, IXamlLineInfoConsumer xamlLineInfoConsumer, XamlContextStack<WpfXamlFrame> stack, IStyleConnector styleConnector)
		{
			while (xamlReader.Read())
			{
				if (shouldPassLineNumberInfo && xamlLineInfo.LineNumber != 0)
				{
					xamlLineInfoConsumer.SetLineInfo(xamlLineInfo.LineNumber, xamlLineInfo.LinePosition);
				}
				switch (xamlReader.NodeType)
				{
				case XamlNodeType.StartObject:
					WpfXamlLoader.WriteStartObject(xamlReader, xamlWriter, stack);
					break;
				case XamlNodeType.GetObject:
					xamlWriter.WriteNode(xamlReader);
					if (stack.CurrentFrame.Type != null)
					{
						stack.PushScope();
					}
					stack.CurrentFrame.Type = stack.PreviousFrame.Property.Type;
					break;
				case XamlNodeType.EndObject:
				{
					xamlWriter.WriteNode(xamlReader);
					if (stack.CurrentFrame.FreezeFreezable)
					{
						Freezable freezable = xamlWriter.Result as Freezable;
						if (freezable != null && freezable.CanFreeze)
						{
							freezable.Freeze();
						}
					}
					DependencyObject dependencyObject = xamlWriter.Result as DependencyObject;
					if (dependencyObject != null && stack.CurrentFrame.XmlSpace != null)
					{
						XmlAttributeProperties.SetXmlSpace(dependencyObject, stack.CurrentFrame.XmlSpace.Value ? "default" : "preserve");
					}
					stack.PopScope();
					break;
				}
				case XamlNodeType.StartMember:
					if ((!xamlReader.Member.IsDirective || !(xamlReader.Member == XamlReaderHelper.Freeze)) && xamlReader.Member != WpfXamlLoader.XmlSpace.Value && xamlReader.Member != XamlLanguage.Space)
					{
						xamlWriter.WriteNode(xamlReader);
					}
					stack.CurrentFrame.Property = xamlReader.Member;
					if (skipJournaledProperties && !stack.CurrentFrame.Property.IsDirective)
					{
						WpfXamlMember wpfXamlMember = stack.CurrentFrame.Property as WpfXamlMember;
						if (wpfXamlMember != null)
						{
							DependencyProperty dependencyProperty = wpfXamlMember.DependencyProperty;
							if (dependencyProperty != null)
							{
								FrameworkPropertyMetadata frameworkPropertyMetadata = dependencyProperty.GetMetadata(stack.CurrentFrame.Type.UnderlyingType) as FrameworkPropertyMetadata;
								if (frameworkPropertyMetadata != null && frameworkPropertyMetadata.Journal)
								{
									int num = 1;
									while (xamlReader.Read())
									{
										switch (xamlReader.NodeType)
										{
										case XamlNodeType.StartObject:
										{
											XamlType type = xamlReader.Type;
											XamlType xamlType = type.SchemaContext.GetXamlType(typeof(BindingBase));
											XamlType xamlType2 = type.SchemaContext.GetXamlType(typeof(DynamicResourceExtension));
											if (num == 1 && (type.CanAssignTo(xamlType) || type.CanAssignTo(xamlType2)))
											{
												num = 0;
												WpfXamlLoader.WriteStartObject(xamlReader, xamlWriter, stack);
											}
											break;
										}
										case XamlNodeType.StartMember:
											num++;
											break;
										case XamlNodeType.EndMember:
											num--;
											if (num == 0)
											{
												xamlWriter.WriteNode(xamlReader);
												stack.CurrentFrame.Property = null;
											}
											break;
										case XamlNodeType.Value:
										{
											DynamicResourceExtension dynamicResourceExtension = xamlReader.Value as DynamicResourceExtension;
											if (dynamicResourceExtension != null)
											{
												WpfXamlLoader.WriteValue(xamlReader, xamlWriter, stack, styleConnector);
											}
											break;
										}
										}
										if (num == 0)
										{
											break;
										}
									}
								}
							}
						}
					}
					break;
				case XamlNodeType.EndMember:
				{
					WpfXamlFrame currentFrame = stack.CurrentFrame;
					XamlMember property = currentFrame.Property;
					if ((!property.IsDirective || !(property == XamlReaderHelper.Freeze)) && property != WpfXamlLoader.XmlSpace.Value && property != XamlLanguage.Space)
					{
						xamlWriter.WriteNode(xamlReader);
					}
					currentFrame.Property = null;
					break;
				}
				case XamlNodeType.Value:
					WpfXamlLoader.WriteValue(xamlReader, xamlWriter, stack, styleConnector);
					break;
				case XamlNodeType.NamespaceDeclaration:
					xamlWriter.WriteNode(xamlReader);
					if (stack.Depth == 0 || stack.CurrentFrame.Type != null)
					{
						stack.PushScope();
						for (WpfXamlFrame wpfXamlFrame = stack.CurrentFrame; wpfXamlFrame != null; wpfXamlFrame = (WpfXamlFrame)wpfXamlFrame.Previous)
						{
							if (wpfXamlFrame.XmlnsDictionary != null)
							{
								stack.CurrentFrame.XmlnsDictionary = new XmlnsDictionary(wpfXamlFrame.XmlnsDictionary);
								break;
							}
						}
						if (stack.CurrentFrame.XmlnsDictionary == null)
						{
							stack.CurrentFrame.XmlnsDictionary = new XmlnsDictionary();
						}
					}
					stack.CurrentFrame.XmlnsDictionary.Add(xamlReader.Namespace.Prefix, xamlReader.Namespace.Namespace);
					break;
				default:
					xamlWriter.WriteNode(xamlReader);
					break;
				}
				if (onlyLoadOneNode)
				{
					return;
				}
			}
		}

		// Token: 0x0600226D RID: 8813 RVA: 0x000AB498 File Offset: 0x000A9698
		private static void WriteValue(XamlReader xamlReader, XamlObjectWriter xamlWriter, XamlContextStack<WpfXamlFrame> stack, IStyleConnector styleConnector)
		{
			bool flag;
			if (stack.CurrentFrame.Property.IsDirective && stack.CurrentFrame.Property == XamlLanguage.Shared && bool.TryParse(xamlReader.Value as string, out flag) && !flag && !(xamlReader is Baml2006Reader))
			{
				throw new XamlParseException(SR.Get("SharedAttributeInLooseXaml"));
			}
			if (stack.CurrentFrame.Property.IsDirective && stack.CurrentFrame.Property == XamlReaderHelper.Freeze)
			{
				bool flag2 = Convert.ToBoolean(xamlReader.Value, TypeConverterHelper.InvariantEnglishUS);
				stack.CurrentFrame.FreezeFreezable = flag2;
				Baml2006Reader baml2006Reader = xamlReader as Baml2006Reader;
				if (baml2006Reader != null)
				{
					baml2006Reader.FreezeFreezables = flag2;
					return;
				}
			}
			else if (stack.CurrentFrame.Property == WpfXamlLoader.XmlSpace.Value || stack.CurrentFrame.Property == XamlLanguage.Space)
			{
				if (typeof(DependencyObject).IsAssignableFrom(stack.CurrentFrame.Type.UnderlyingType))
				{
					stack.CurrentFrame.XmlSpace = new bool?((string)xamlReader.Value == "default");
					return;
				}
			}
			else
			{
				if (styleConnector != null && stack.CurrentFrame.Instance != null && stack.CurrentFrame.Property == XamlLanguage.ConnectionId && typeof(Style).IsAssignableFrom(stack.CurrentFrame.Type.UnderlyingType))
				{
					styleConnector.Connect((int)xamlReader.Value, stack.CurrentFrame.Instance);
				}
				xamlWriter.WriteNode(xamlReader);
			}
		}

		// Token: 0x0600226E RID: 8814 RVA: 0x000AB640 File Offset: 0x000A9840
		private static void WriteStartObject(XamlReader xamlReader, XamlObjectWriter xamlWriter, XamlContextStack<WpfXamlFrame> stack)
		{
			xamlWriter.WriteNode(xamlReader);
			if (stack.Depth != 0 && stack.CurrentFrame.Type == null)
			{
				stack.CurrentFrame.Type = xamlReader.Type;
				return;
			}
			stack.PushScope();
			stack.CurrentFrame.Type = xamlReader.Type;
			if (stack.PreviousFrame.FreezeFreezable)
			{
				stack.CurrentFrame.FreezeFreezable = true;
			}
		}

		// Token: 0x040019F0 RID: 6640
		private static Lazy<XamlMember> XmlSpace = new Lazy<XamlMember>(() => new WpfXamlMember(XmlAttributeProperties.XmlSpaceProperty, true));
	}
}
