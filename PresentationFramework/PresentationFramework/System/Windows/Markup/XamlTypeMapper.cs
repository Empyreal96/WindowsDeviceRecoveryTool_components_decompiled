using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using System.Windows.Baml2006;
using System.Xaml;
using MS.Internal;
using MS.Internal.PresentationFramework;
using MS.Internal.WindowsBase;
using MS.Utility;

namespace System.Windows.Markup
{
	/// <summary>Maps a XAML element name to the appropriate CLR <see cref="T:System.Type" /> in assemblies.</summary>
	// Token: 0x0200021C RID: 540
	public class XamlTypeMapper
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Markup.XamlTypeMapper" /> class by specifying an array of assembly names that the <see cref="T:System.Windows.Markup.XamlTypeMapper" /> should use.</summary>
		/// <param name="assemblyNames">The array of assembly names the <see cref="T:System.Windows.Markup.XamlTypeMapper" /> should use.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="assemblyNames" /> is <see langword="null" />.</exception>
		// Token: 0x06002137 RID: 8503 RVA: 0x000A5120 File Offset: 0x000A3320
		public XamlTypeMapper(string[] assemblyNames)
		{
			if (assemblyNames == null)
			{
				throw new ArgumentNullException("assemblyNames");
			}
			this._assemblyNames = assemblyNames;
			this._namespaceMaps = null;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Markup.XamlTypeMapper" /> class, using the specified array of assembly names and the specified namespace maps.</summary>
		/// <param name="assemblyNames">The array of assembly names the <see cref="T:System.Windows.Markup.XamlTypeMapper" /> should use.</param>
		/// <param name="namespaceMaps">The array of namespace maps the <see cref="T:System.Windows.Markup.XamlTypeMapper" /> should use.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="assemblyNames" /> is <see langword="null" />.</exception>
		// Token: 0x06002138 RID: 8504 RVA: 0x000A5194 File Offset: 0x000A3394
		public XamlTypeMapper(string[] assemblyNames, NamespaceMapEntry[] namespaceMaps)
		{
			if (assemblyNames == null)
			{
				throw new ArgumentNullException("assemblyNames");
			}
			this._assemblyNames = assemblyNames;
			this._namespaceMaps = namespaceMaps;
		}

		/// <summary>Gets the CLR <see cref="T:System.Type" /> that a given XAML element is mapped to, using the specified XML namespace prefix and element name.</summary>
		/// <param name="xmlNamespace">The specified XML namespace prefix.</param>
		/// <param name="localName">The "local" name of the XAML element to obtain the mapped <see cref="T:System.Type" /> for. Local in this context means as mapped versus the provided <paramref name="xmlNamespace" />.</param>
		/// <returns>The <see cref="T:System.Type" /> for the object, or <see langword="null" /> if no mapping could be resolved.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="xmlNamespace" /> is <see langword="null" />-or-
		///         <paramref name="localName" /> is <see langword="null" />.</exception>
		// Token: 0x06002139 RID: 8505 RVA: 0x000A5208 File Offset: 0x000A3408
		public Type GetType(string xmlNamespace, string localName)
		{
			if (xmlNamespace == null)
			{
				throw new ArgumentNullException("xmlNamespace");
			}
			if (localName == null)
			{
				throw new ArgumentNullException("localName");
			}
			TypeAndSerializer typeOnly = this.GetTypeOnly(xmlNamespace, localName);
			if (typeOnly == null)
			{
				return null;
			}
			return typeOnly.ObjectType;
		}

		/// <summary>Defines a mapping between an XML namespace and CLR namespaces in assemblies, and adds these to the <see cref="T:System.Windows.Markup.XamlTypeMapper" /> information.</summary>
		/// <param name="xmlNamespace">The prefix for the XML namespace..</param>
		/// <param name="clrNamespace">The CLR  namespace that contains the types to map.</param>
		/// <param name="assemblyName">The assembly that contains the CLR  namespace.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="xmlNamespace" /> is <see langword="null" />-or-
		///         <paramref name="clrNamespace" /> is <see langword="null" />-or-
		///         <paramref name="assemblyName" /> is <see langword="null" />.</exception>
		// Token: 0x0600213A RID: 8506 RVA: 0x000A5248 File Offset: 0x000A3448
		public void AddMappingProcessingInstruction(string xmlNamespace, string clrNamespace, string assemblyName)
		{
			if (xmlNamespace == null)
			{
				throw new ArgumentNullException("xmlNamespace");
			}
			if (clrNamespace == null)
			{
				throw new ArgumentNullException("clrNamespace");
			}
			if (assemblyName == null)
			{
				throw new ArgumentNullException("assemblyName");
			}
			ClrNamespaceAssemblyPair clrNamespaceAssemblyPair = new ClrNamespaceAssemblyPair(clrNamespace, assemblyName);
			this.PITable[xmlNamespace] = clrNamespaceAssemblyPair;
			string str = assemblyName.ToUpper(TypeConverterHelper.InvariantEnglishUS);
			string key = clrNamespace + "#" + str;
			this._piReverseTable[key] = xmlNamespace;
			if (this._schemaContext != null)
			{
				this._schemaContext.SetMappingProcessingInstruction(xmlNamespace, clrNamespaceAssemblyPair);
			}
		}

		/// <summary>Specifies the path to use when loading an assembly.</summary>
		/// <param name="assemblyName">The short name of the assembly without an extension or path specified (equivalent to <see cref="P:System.Reflection.AssemblyName.Name" />).  </param>
		/// <param name="assemblyPath">The file path of the assembly. The assembly path must be a full file path containing a file extension.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="assemblyName" /> is <see langword="null" />-or-
		///         <paramref name="assemblyPath" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.Windows.Markup.XamlParseException">
		///         <paramref name="assemblyName" /> is <see cref="F:System.String.Empty" />-or-
		///         <paramref name="assemblyPath" /> is <see cref="F:System.String.Empty" />-or-
		///         <paramref name="assemblyPath" /> is not a full file path containing a file extension.</exception>
		// Token: 0x0600213B RID: 8507 RVA: 0x000A52D8 File Offset: 0x000A34D8
		public void SetAssemblyPath(string assemblyName, string assemblyPath)
		{
			if (assemblyName == null)
			{
				throw new ArgumentNullException("assemblyName");
			}
			if (assemblyPath == null)
			{
				throw new ArgumentNullException("assemblyPath");
			}
			if (assemblyPath == string.Empty)
			{
				this._lineNumber = 0;
				this.ThrowException("ParserBadAssemblyPath");
			}
			if (assemblyName == string.Empty)
			{
				this._lineNumber = 0;
				this.ThrowException("ParserBadAssemblyName");
			}
			string text = assemblyName.ToUpper(CultureInfo.InvariantCulture);
			HybridDictionary assemblyPathTable = this._assemblyPathTable;
			lock (assemblyPathTable)
			{
				this._assemblyPathTable[text] = assemblyPath;
			}
			Assembly alreadyLoadedAssembly = ReflectionHelper.GetAlreadyLoadedAssembly(text);
			if (alreadyLoadedAssembly != null && !alreadyLoadedAssembly.GlobalAssemblyCache)
			{
				ReflectionHelper.ResetCacheForAssembly(text);
				if (this._schemaContext != null)
				{
					this._schemaContext = null;
				}
			}
		}

		/// <summary>Gets an instance of the <see cref="T:System.Windows.Markup.XamlTypeMapper" /> to use if one has not been specified.</summary>
		/// <returns>The default type mapper.</returns>
		// Token: 0x17000802 RID: 2050
		// (get) Token: 0x0600213C RID: 8508 RVA: 0x000A53B4 File Offset: 0x000A35B4
		public static XamlTypeMapper DefaultMapper
		{
			get
			{
				return XmlParserDefaults.DefaultMapper;
			}
		}

		// Token: 0x0600213D RID: 8509 RVA: 0x000A53BC File Offset: 0x000A35BC
		internal void Initialize()
		{
			this._typeLookupFromXmlHashtable.Clear();
			this._namespaceMapHashList.Clear();
			this._piTable.Clear();
			this._piReverseTable.Clear();
			HybridDictionary assemblyPathTable = this._assemblyPathTable;
			lock (assemblyPathTable)
			{
				this._assemblyPathTable.Clear();
			}
			this._referenceAssembliesLoaded = false;
		}

		// Token: 0x0600213E RID: 8510 RVA: 0x000A5434 File Offset: 0x000A3634
		internal XamlTypeMapper Clone()
		{
			return new XamlTypeMapper(this._assemblyNames.Clone() as string[])
			{
				_mapTable = this._mapTable,
				_referenceAssembliesLoaded = this._referenceAssembliesLoaded,
				_lineNumber = this._lineNumber,
				_linePosition = this._linePosition,
				_namespaceMaps = (this._namespaceMaps.Clone() as NamespaceMapEntry[]),
				_typeLookupFromXmlHashtable = (this._typeLookupFromXmlHashtable.Clone() as Hashtable),
				_namespaceMapHashList = (this._namespaceMapHashList.Clone() as Hashtable),
				_typeInformationCache = this.CloneHybridDictionary(this._typeInformationCache),
				_piTable = this.CloneHybridDictionary(this._piTable),
				_piReverseTable = this.CloneStringDictionary(this._piReverseTable),
				_assemblyPathTable = this.CloneHybridDictionary(this._assemblyPathTable)
			};
		}

		// Token: 0x0600213F RID: 8511 RVA: 0x000A5514 File Offset: 0x000A3714
		private HybridDictionary CloneHybridDictionary(HybridDictionary dict)
		{
			HybridDictionary hybridDictionary = new HybridDictionary(dict.Count);
			foreach (object obj in dict)
			{
				DictionaryEntry dictionaryEntry = (DictionaryEntry)obj;
				hybridDictionary.Add(dictionaryEntry.Key, dictionaryEntry.Value);
			}
			return hybridDictionary;
		}

		// Token: 0x06002140 RID: 8512 RVA: 0x000A5584 File Offset: 0x000A3784
		private Dictionary<string, string> CloneStringDictionary(Dictionary<string, string> dict)
		{
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			foreach (KeyValuePair<string, string> keyValuePair in dict)
			{
				dictionary.Add(keyValuePair.Key, keyValuePair.Value);
			}
			return dictionary;
		}

		// Token: 0x06002141 RID: 8513 RVA: 0x000A55E8 File Offset: 0x000A37E8
		internal string AssemblyPathFor(string assemblyName)
		{
			string result = null;
			if (assemblyName != null)
			{
				HybridDictionary assemblyPathTable = this._assemblyPathTable;
				lock (assemblyPathTable)
				{
					result = (this._assemblyPathTable[assemblyName.ToUpper(CultureInfo.InvariantCulture)] as string);
				}
			}
			return result;
		}

		// Token: 0x06002142 RID: 8514 RVA: 0x000A5644 File Offset: 0x000A3844
		private bool LoadReferenceAssemblies()
		{
			if (!this._referenceAssembliesLoaded)
			{
				this._referenceAssembliesLoaded = true;
				foreach (object obj in this._assemblyPathTable)
				{
					DictionaryEntry dictionaryEntry = (DictionaryEntry)obj;
					ReflectionHelper.LoadAssembly(dictionaryEntry.Key as string, dictionaryEntry.Value as string);
				}
				return true;
			}
			return false;
		}

		// Token: 0x06002143 RID: 8515 RVA: 0x000A56C8 File Offset: 0x000A38C8
		internal RoutedEvent GetRoutedEvent(Type owner, string xmlNamespace, string localName)
		{
			Type type = null;
			string text = null;
			if (localName == null)
			{
				throw new ArgumentNullException("localName");
			}
			if (xmlNamespace == null)
			{
				throw new ArgumentNullException("xmlNamespace");
			}
			if (owner != null && !ReflectionHelper.IsPublicType(owner))
			{
				this._lineNumber = 0;
				this.ThrowException("ParserOwnerEventMustBePublic", owner.FullName);
			}
			return this.GetDependencyObject(true, owner, xmlNamespace, localName, ref type, ref text) as RoutedEvent;
		}

		// Token: 0x06002144 RID: 8516 RVA: 0x000A5734 File Offset: 0x000A3934
		internal object ParseProperty(object targetObject, Type propType, string propName, object dpOrPiOrFi, ITypeDescriptorContext typeContext, ParserContext parserContext, string value, short converterTypeId)
		{
			this._lineNumber = ((parserContext != null) ? parserContext.LineNumber : 0);
			this._linePosition = ((parserContext != null) ? parserContext.LinePosition : 0);
			if (converterTypeId < 0 && -converterTypeId == 615)
			{
				if (propType == typeof(object) || propType == typeof(string))
				{
					return value;
				}
				string message = SR.Get("ParserCannotConvertPropertyValueString", new object[]
				{
					value,
					propName,
					propType.FullName
				});
				XamlParseException.ThrowException(parserContext, this._lineNumber, this._linePosition, message, null);
			}
			object obj = null;
			TypeConverter typeConverter;
			if (converterTypeId != 0)
			{
				typeConverter = parserContext.MapTable.GetConverterFromId(converterTypeId, propType, parserContext);
			}
			else
			{
				typeConverter = this.GetPropertyConverter(propType, dpOrPiOrFi);
			}
			try
			{
				obj = typeConverter.ConvertFromString(typeContext, TypeConverterHelper.InvariantEnglishUS, value);
				if (TraceMarkup.IsEnabled)
				{
					TraceMarkup.TraceActivityItem(TraceMarkup.TypeConvert, typeConverter, value, obj);
				}
			}
			catch (Exception ex)
			{
				if (CriticalExceptions.IsCriticalException(ex) || ex is XamlParseException)
				{
					throw;
				}
				IProvidePropertyFallback providePropertyFallback = targetObject as IProvidePropertyFallback;
				if (providePropertyFallback != null && providePropertyFallback.CanProvidePropertyFallback(propName))
				{
					obj = providePropertyFallback.ProvidePropertyFallback(propName, ex);
					if (TraceMarkup.IsEnabled)
					{
						TraceMarkup.TraceActivityItem(TraceMarkup.TypeConvertFallback, typeConverter, value, obj);
					}
				}
				else if (typeConverter.GetType() == typeof(TypeConverter))
				{
					string message2;
					if (propName != string.Empty)
					{
						message2 = SR.Get("ParserDefaultConverterProperty", new object[]
						{
							propType.FullName,
							propName,
							value
						});
					}
					else
					{
						message2 = SR.Get("ParserDefaultConverterElement", new object[]
						{
							propType.FullName,
							value
						});
					}
					XamlParseException.ThrowException(parserContext, this._lineNumber, this._linePosition, message2, null);
				}
				else
				{
					string message3 = this.TypeConverterFailure(value, propName, propType.FullName);
					XamlParseException.ThrowException(parserContext, this._lineNumber, this._linePosition, message3, ex);
				}
			}
			if (obj != null && !propType.IsAssignableFrom(obj.GetType()))
			{
				string message4 = this.TypeConverterFailure(value, propName, propType.FullName);
				XamlParseException.ThrowException(parserContext, this._lineNumber, this._linePosition, message4, null);
			}
			return obj;
		}

		// Token: 0x06002145 RID: 8517 RVA: 0x000A596C File Offset: 0x000A3B6C
		private string TypeConverterFailure(string value, string propName, string propType)
		{
			string result;
			if (propName != string.Empty)
			{
				result = SR.Get("ParserCannotConvertPropertyValueString", new object[]
				{
					value,
					propName,
					propType
				});
			}
			else
			{
				result = SR.Get("ParserCannotConvertInitializationText", new object[]
				{
					value,
					propType
				});
			}
			return result;
		}

		// Token: 0x06002146 RID: 8518 RVA: 0x000A59C0 File Offset: 0x000A3BC0
		internal void ValidateNames(string value, int lineNumber, int linePosition)
		{
			this._lineNumber = lineNumber;
			this._linePosition = linePosition;
			if (value == string.Empty)
			{
				this.ThrowException("ParserBadName", value);
			}
			if (MarkupExtensionParser.LooksLikeAMarkupExtension(value))
			{
				string text = SR.Get("ParserBadUidOrNameME", new object[]
				{
					value
				});
				text += " ";
				text += SR.Get("ParserLineAndOffset", new object[]
				{
					lineNumber.ToString(CultureInfo.CurrentCulture),
					linePosition.ToString(CultureInfo.CurrentCulture)
				});
				XamlParseException ex = new XamlParseException(text, lineNumber, linePosition);
				throw ex;
			}
			if (!NameValidationHelper.IsValidIdentifierName(value))
			{
				this.ThrowException("ParserBadName", value);
			}
		}

		// Token: 0x06002147 RID: 8519 RVA: 0x000A5A74 File Offset: 0x000A3C74
		internal void ValidateEnums(string propName, Type propType, string attribValue)
		{
			if (propType.IsEnum && attribValue != string.Empty)
			{
				bool flag = false;
				for (int i = 0; i < attribValue.Length; i++)
				{
					if (!char.IsWhiteSpace(attribValue[i]))
					{
						if (flag)
						{
							if (attribValue[i] == ',')
							{
								flag = false;
							}
						}
						else if (char.IsDigit(attribValue[i]))
						{
							this.ThrowException("ParserNoDigitEnums", propName, attribValue);
						}
						else
						{
							flag = true;
						}
					}
				}
			}
		}

		// Token: 0x06002148 RID: 8520 RVA: 0x000A5AE8 File Offset: 0x000A3CE8
		private MemberInfo GetCachedMemberInfo(Type owner, string propName, bool onlyPropInfo, out BamlAttributeInfoRecord infoRecord)
		{
			infoRecord = null;
			if (this.MapTable != null)
			{
				string ownerTypeName = owner.IsGenericType ? (owner.Namespace + "." + owner.Name) : owner.FullName;
				object attributeInfoKey = this.MapTable.GetAttributeInfoKey(ownerTypeName, propName);
				infoRecord = (this.MapTable.GetHashTableData(attributeInfoKey) as BamlAttributeInfoRecord);
				if (infoRecord != null)
				{
					return infoRecord.GetPropertyMember(onlyPropInfo) as MemberInfo;
				}
			}
			return null;
		}

		// Token: 0x06002149 RID: 8521 RVA: 0x000A5B60 File Offset: 0x000A3D60
		private void AddCachedAttributeInfo(Type ownerType, BamlAttributeInfoRecord infoRecord)
		{
			if (this.MapTable != null)
			{
				object attributeInfoKey = this.MapTable.GetAttributeInfoKey(ownerType.FullName, infoRecord.Name);
				this.MapTable.AddHashTableData(attributeInfoKey, infoRecord);
			}
		}

		// Token: 0x0600214A RID: 8522 RVA: 0x000A5B9C File Offset: 0x000A3D9C
		internal void UpdateClrPropertyInfo(Type currentParentType, BamlAttributeInfoRecord attribInfo)
		{
			bool isInternal = false;
			string name = attribInfo.Name;
			BamlAttributeInfoRecord bamlAttributeInfoRecord;
			attribInfo.PropInfo = (this.GetCachedMemberInfo(currentParentType, name, true, out bamlAttributeInfoRecord) as PropertyInfo);
			if (attribInfo.PropInfo == null)
			{
				attribInfo.PropInfo = this.PropertyInfoFromName(name, currentParentType, !ReflectionHelper.IsPublicType(currentParentType), false, out isInternal);
				attribInfo.IsInternal = isInternal;
				if (attribInfo.PropInfo != null)
				{
					if (bamlAttributeInfoRecord != null)
					{
						bamlAttributeInfoRecord.SetPropertyMember(attribInfo.PropInfo);
						bamlAttributeInfoRecord.IsInternal = attribInfo.IsInternal;
						return;
					}
					this.AddCachedAttributeInfo(currentParentType, attribInfo);
					return;
				}
			}
			else
			{
				attribInfo.IsInternal = bamlAttributeInfoRecord.IsInternal;
			}
		}

		// Token: 0x0600214B RID: 8523 RVA: 0x000A5C38 File Offset: 0x000A3E38
		private void UpdateAttachedPropertyMethdodInfo(BamlAttributeInfoRecord attributeInfo, bool isSetter)
		{
			MethodInfo methodInfo = null;
			Type ownerType = attributeInfo.OwnerType;
			bool flag = !ReflectionHelper.IsPublicType(ownerType);
			string name = (isSetter ? "Set" : "Get") + attributeInfo.Name;
			BindingFlags bindingFlags = BindingFlags.Static | BindingFlags.Public | BindingFlags.FlattenHierarchy;
			try
			{
				if (!flag)
				{
					methodInfo = ownerType.GetMethod(name, bindingFlags);
				}
				if (methodInfo == null)
				{
					methodInfo = ownerType.GetMethod(name, bindingFlags | BindingFlags.NonPublic);
				}
			}
			catch (AmbiguousMatchException)
			{
			}
			int num = isSetter ? 2 : 1;
			if (methodInfo != null && methodInfo.GetParameters().Length == num)
			{
				if (isSetter)
				{
					attributeInfo.AttachedPropertySetter = methodInfo;
					return;
				}
				attributeInfo.AttachedPropertyGetter = methodInfo;
			}
		}

		// Token: 0x0600214C RID: 8524 RVA: 0x000A5CE0 File Offset: 0x000A3EE0
		internal void UpdateAttachedPropertySetter(BamlAttributeInfoRecord attributeInfo)
		{
			if (attributeInfo.AttachedPropertySetter == null)
			{
				this.UpdateAttachedPropertyMethdodInfo(attributeInfo, true);
			}
		}

		// Token: 0x0600214D RID: 8525 RVA: 0x000A5CF8 File Offset: 0x000A3EF8
		internal void UpdateAttachedPropertyGetter(BamlAttributeInfoRecord attributeInfo)
		{
			if (attributeInfo.AttachedPropertyGetter == null)
			{
				this.UpdateAttachedPropertyMethdodInfo(attributeInfo, false);
			}
		}

		// Token: 0x0600214E RID: 8526 RVA: 0x000A5D10 File Offset: 0x000A3F10
		internal MemberInfo GetClrInfo(bool isEvent, Type owner, string xmlNamespace, string localName, ref string propName)
		{
			string globalClassName = null;
			int num = localName.LastIndexOf('.');
			if (-1 != num)
			{
				globalClassName = localName.Substring(0, num);
				localName = localName.Substring(num + 1);
			}
			return this.GetClrInfoForClass(isEvent, owner, xmlNamespace, localName, globalClassName, ref propName);
		}

		// Token: 0x0600214F RID: 8527 RVA: 0x000A5D54 File Offset: 0x000A3F54
		internal bool IsAllowedPropertySet(PropertyInfo pi)
		{
			MethodInfo setMethod = pi.GetSetMethod(true);
			return setMethod != null && setMethod.IsPublic;
		}

		// Token: 0x06002150 RID: 8528 RVA: 0x000A5D7C File Offset: 0x000A3F7C
		internal bool IsAllowedPropertyGet(PropertyInfo pi)
		{
			MethodInfo getMethod = pi.GetGetMethod(true);
			return getMethod != null && getMethod.IsPublic;
		}

		// Token: 0x06002151 RID: 8529 RVA: 0x000A5DA4 File Offset: 0x000A3FA4
		internal static bool IsAllowedPropertySet(PropertyInfo pi, bool allowProtected, out bool isPublic)
		{
			MethodInfo setMethod = pi.GetSetMethod(true);
			bool flag = allowProtected && setMethod != null && setMethod.IsFamily;
			isPublic = (setMethod != null && setMethod.IsPublic && ReflectionHelper.IsPublicType(setMethod.DeclaringType));
			return setMethod != null && (setMethod.IsPublic || setMethod.IsAssembly || setMethod.IsFamilyOrAssembly || flag);
		}

		// Token: 0x06002152 RID: 8530 RVA: 0x000A5E18 File Offset: 0x000A4018
		private static bool IsAllowedPropertyGet(PropertyInfo pi, bool allowProtected, out bool isPublic)
		{
			MethodInfo getMethod = pi.GetGetMethod(true);
			bool flag = allowProtected && getMethod != null && getMethod.IsFamily;
			isPublic = (getMethod != null && getMethod.IsPublic && ReflectionHelper.IsPublicType(getMethod.DeclaringType));
			return getMethod != null && (getMethod.IsPublic || getMethod.IsAssembly || getMethod.IsFamilyOrAssembly || flag);
		}

		// Token: 0x06002153 RID: 8531 RVA: 0x000A5E8C File Offset: 0x000A408C
		private static bool IsAllowedEvent(EventInfo ei, bool allowProtected, out bool isPublic)
		{
			MethodInfo addMethod = ei.GetAddMethod(true);
			bool flag = allowProtected && addMethod != null && addMethod.IsFamily;
			isPublic = (addMethod != null && addMethod.IsPublic && ReflectionHelper.IsPublicType(addMethod.DeclaringType));
			return addMethod != null && (addMethod.IsPublic || addMethod.IsAssembly || addMethod.IsFamilyOrAssembly || flag);
		}

		// Token: 0x06002154 RID: 8532 RVA: 0x000A5F00 File Offset: 0x000A4100
		private static bool IsPublicEvent(EventInfo ei)
		{
			MethodInfo addMethod = ei.GetAddMethod(true);
			return addMethod != null && addMethod.IsPublic;
		}

		/// <summary>Requests permission for a <see cref="T:System.Windows.Markup.XamlTypeMapper" /> derived type that is called under full trust to access a specific internal type.</summary>
		/// <param name="type">The type to access.</param>
		/// <returns>
		///     <see langword="true" /> if the internal type can be accessed; otherwise, <see langword="false" />.</returns>
		// Token: 0x06002155 RID: 8533 RVA: 0x0000B02A File Offset: 0x0000922A
		protected virtual bool AllowInternalType(Type type)
		{
			return false;
		}

		// Token: 0x06002156 RID: 8534 RVA: 0x000A5F28 File Offset: 0x000A4128
		private bool IsInternalTypeAllowedInFullTrust(Type type)
		{
			bool result = false;
			if (ReflectionHelper.IsInternalType(type) && SecurityHelper.IsFullTrustCaller())
			{
				result = this.AllowInternalType(type);
			}
			return result;
		}

		// Token: 0x06002157 RID: 8535 RVA: 0x000A5F50 File Offset: 0x000A4150
		internal MemberInfo GetClrInfoForClass(bool isEvent, Type owner, string xmlNamespace, string localName, string globalClassName, ref string propName)
		{
			return this.GetClrInfoForClass(isEvent, owner, xmlNamespace, localName, globalClassName, false, ref propName);
		}

		// Token: 0x06002158 RID: 8536 RVA: 0x000A5F74 File Offset: 0x000A4174
		private MemberInfo GetClrInfoForClass(bool isEvent, Type owner, string xmlNamespace, string localName, string globalClassName, bool tryInternal, ref string propName)
		{
			bool flag = false;
			MemberInfo memberInfo = null;
			BindingFlags bindingFlags = BindingFlags.Public;
			propName = null;
			if (globalClassName != null)
			{
				TypeAndSerializer typeOnly = this.GetTypeOnly(xmlNamespace, globalClassName);
				if (typeOnly != null && typeOnly.ObjectType != null)
				{
					Type objectType = typeOnly.ObjectType;
					BamlAttributeInfoRecord bamlAttributeInfoRecord;
					memberInfo = this.GetCachedMemberInfo(objectType, localName, false, out bamlAttributeInfoRecord);
					if (memberInfo == null)
					{
						if (isEvent)
						{
							memberInfo = objectType.GetMethod("Add" + localName + "Handler", bindingFlags | BindingFlags.Static | BindingFlags.FlattenHierarchy);
							if (memberInfo != null)
							{
								MethodInfo methodInfo = memberInfo as MethodInfo;
								if (methodInfo != null)
								{
									ParameterInfo[] parameters = methodInfo.GetParameters();
									Type type = KnownTypes.Types[135];
									if (parameters == null || parameters.Length != 2 || !type.IsAssignableFrom(parameters[0].ParameterType))
									{
										memberInfo = null;
									}
								}
							}
							if (memberInfo == null)
							{
								memberInfo = objectType.GetEvent(localName, bindingFlags | BindingFlags.Instance | BindingFlags.FlattenHierarchy);
								if (memberInfo != null)
								{
									EventInfo eventInfo = memberInfo as EventInfo;
									if (!ReflectionHelper.IsPublicType(eventInfo.EventHandlerType))
									{
										this.ThrowException("ParserEventDelegateTypeNotAccessible", eventInfo.EventHandlerType.FullName, objectType.Name + "." + localName);
									}
									if (!XamlTypeMapper.IsPublicEvent(eventInfo))
									{
										this.ThrowException("ParserCantSetAttribute", "event", objectType.Name + "." + localName, "add");
									}
								}
							}
						}
						else
						{
							memberInfo = objectType.GetMethod("Set" + localName, bindingFlags | BindingFlags.Static | BindingFlags.FlattenHierarchy);
							if (memberInfo != null && ((MethodInfo)memberInfo).GetParameters().Length != 2)
							{
								memberInfo = null;
							}
							if (memberInfo == null)
							{
								memberInfo = objectType.GetMethod("Get" + localName, bindingFlags | BindingFlags.Static | BindingFlags.FlattenHierarchy);
								if (memberInfo != null && ((MethodInfo)memberInfo).GetParameters().Length != 1)
								{
									memberInfo = null;
								}
							}
							if (memberInfo == null)
							{
								memberInfo = this.PropertyInfoFromName(localName, objectType, tryInternal, true, out flag);
								if (memberInfo != null && owner != null && !objectType.IsAssignableFrom(owner))
								{
									this.ThrowException("ParserAttachedPropInheritError", string.Format(CultureInfo.CurrentCulture, "{0}.{1}", new object[]
									{
										objectType.Name,
										localName
									}), owner.Name);
								}
							}
							if (null != memberInfo && bamlAttributeInfoRecord != null)
							{
								if (bamlAttributeInfoRecord.DP == null)
								{
									bamlAttributeInfoRecord.DP = this.MapTable.GetDependencyProperty(bamlAttributeInfoRecord);
								}
								bamlAttributeInfoRecord.SetPropertyMember(memberInfo);
							}
						}
					}
				}
			}
			else if (null != owner && null != owner)
			{
				BamlAttributeInfoRecord bamlAttributeInfoRecord2;
				memberInfo = this.GetCachedMemberInfo(owner, localName, false, out bamlAttributeInfoRecord2);
				if (memberInfo == null)
				{
					if (isEvent)
					{
						memberInfo = owner.GetMethod("Add" + localName + "Handler", bindingFlags | BindingFlags.Static | BindingFlags.FlattenHierarchy);
						if (memberInfo != null)
						{
							MethodInfo methodInfo2 = memberInfo as MethodInfo;
							if (methodInfo2 != null)
							{
								ParameterInfo[] parameters = methodInfo2.GetParameters();
								Type type2 = KnownTypes.Types[135];
								if (parameters == null || parameters.Length != 2 || !type2.IsAssignableFrom(parameters[0].ParameterType))
								{
									memberInfo = null;
								}
							}
						}
						if (memberInfo == null)
						{
							memberInfo = owner.GetEvent(localName, BindingFlags.Instance | BindingFlags.FlattenHierarchy | bindingFlags);
							if (memberInfo != null)
							{
								EventInfo eventInfo2 = memberInfo as EventInfo;
								if (!ReflectionHelper.IsPublicType(eventInfo2.EventHandlerType))
								{
									this.ThrowException("ParserEventDelegateTypeNotAccessible", eventInfo2.EventHandlerType.FullName, owner.Name + "." + localName);
								}
								if (!XamlTypeMapper.IsPublicEvent(eventInfo2))
								{
									this.ThrowException("ParserCantSetAttribute", "event", owner.Name + "." + localName, "add");
								}
							}
						}
					}
					else
					{
						memberInfo = owner.GetMethod("Set" + localName, bindingFlags | BindingFlags.Static | BindingFlags.FlattenHierarchy);
						if (memberInfo != null && ((MethodInfo)memberInfo).GetParameters().Length != 2)
						{
							memberInfo = null;
						}
						if (memberInfo == null)
						{
							memberInfo = owner.GetMethod("Get" + localName, bindingFlags | BindingFlags.Static | BindingFlags.FlattenHierarchy);
							if (memberInfo != null && ((MethodInfo)memberInfo).GetParameters().Length != 1)
							{
								memberInfo = null;
							}
						}
						if (memberInfo == null)
						{
							memberInfo = this.PropertyInfoFromName(localName, owner, tryInternal, true, out flag);
						}
						if (null != memberInfo && bamlAttributeInfoRecord2 != null)
						{
							if (bamlAttributeInfoRecord2.DP == null)
							{
								bamlAttributeInfoRecord2.DP = this.MapTable.GetDependencyProperty(bamlAttributeInfoRecord2);
							}
							bamlAttributeInfoRecord2.SetPropertyMember(memberInfo);
						}
					}
				}
			}
			if (null != memberInfo)
			{
				propName = localName;
			}
			return memberInfo;
		}

		// Token: 0x06002159 RID: 8537 RVA: 0x000A6424 File Offset: 0x000A4624
		internal EventInfo GetClrEventInfo(Type owner, string eventName)
		{
			EventInfo eventInfo = null;
			while (owner != null)
			{
				eventInfo = owner.GetEvent(eventName, BindingFlags.Instance | BindingFlags.Public);
				if (eventInfo != null)
				{
					break;
				}
				owner = this.GetCachedBaseType(owner);
			}
			return eventInfo;
		}

		// Token: 0x0600215A RID: 8538 RVA: 0x000A645C File Offset: 0x000A465C
		internal object GetDependencyObject(bool isEvent, Type owner, string xmlNamespace, string localName, ref Type baseType, ref string dynamicObjectName)
		{
			object obj = null;
			string text = null;
			dynamicObjectName = null;
			int num = localName.LastIndexOf('.');
			if (-1 != num)
			{
				text = localName.Substring(0, num);
				localName = localName.Substring(num + 1);
			}
			if (text != null)
			{
				TypeAndSerializer typeOnly = this.GetTypeOnly(xmlNamespace, text);
				if (typeOnly != null && typeOnly.ObjectType != null)
				{
					baseType = typeOnly.ObjectType;
					if (isEvent)
					{
						obj = this.RoutedEventFromName(localName, baseType);
					}
					else
					{
						obj = DependencyProperty.FromName(localName, baseType);
					}
					if (obj != null)
					{
						dynamicObjectName = localName;
					}
				}
			}
			else
			{
				NamespaceMapEntry[] namespaceMapEntries = this.GetNamespaceMapEntries(xmlNamespace);
				if (namespaceMapEntries == null)
				{
					return null;
				}
				baseType = owner;
				while (null != baseType)
				{
					bool flag = false;
					int num2 = 0;
					while (num2 < namespaceMapEntries.Length && !flag)
					{
						NamespaceMapEntry namespaceMapEntry = namespaceMapEntries[num2];
						if (namespaceMapEntry.ClrNamespace == this.GetCachedNamespace(baseType))
						{
							flag = true;
						}
						num2++;
					}
					if (flag)
					{
						if (isEvent)
						{
							obj = this.RoutedEventFromName(localName, baseType);
						}
						else
						{
							obj = DependencyProperty.FromName(localName, baseType);
						}
					}
					if (obj != null || isEvent)
					{
						dynamicObjectName = localName;
						break;
					}
					baseType = this.GetCachedBaseType(baseType);
				}
			}
			return obj;
		}

		// Token: 0x0600215B RID: 8539 RVA: 0x000A6588 File Offset: 0x000A4788
		internal DependencyProperty DependencyPropertyFromName(string localName, string xmlNamespace, ref Type ownerType)
		{
			int num = localName.LastIndexOf('.');
			if (-1 != num)
			{
				string text = localName.Substring(0, num);
				localName = localName.Substring(num + 1);
				TypeAndSerializer typeOnly = this.GetTypeOnly(xmlNamespace, text);
				if (typeOnly == null || typeOnly.ObjectType == null)
				{
					this.ThrowException("ParserNoType", text);
				}
				ownerType = typeOnly.ObjectType;
			}
			if (null == ownerType)
			{
				throw new ArgumentNullException("ownerType");
			}
			return DependencyProperty.FromName(localName, ownerType);
		}

		// Token: 0x0600215C RID: 8540 RVA: 0x000A6604 File Offset: 0x000A4804
		internal PropertyInfo GetXmlLangProperty(string xmlNamespace, string localName)
		{
			TypeAndSerializer typeOnly = this.GetTypeOnly(xmlNamespace, localName);
			if (typeOnly == null || typeOnly.ObjectType == null)
			{
				return null;
			}
			if (typeOnly.XmlLangProperty == null)
			{
				BamlAssemblyInfoRecord assemblyInfoFromId = this.MapTable.GetAssemblyInfoFromId(-1);
				if (typeOnly.ObjectType.Assembly == assemblyInfoFromId.Assembly)
				{
					if (KnownTypes.Types[226].IsAssignableFrom(typeOnly.ObjectType) || KnownTypes.Types[225].IsAssignableFrom(typeOnly.ObjectType))
					{
						typeOnly.XmlLangProperty = KnownTypes.Types[226].GetProperty("Language", BindingFlags.Instance | BindingFlags.Public | BindingFlags.FlattenHierarchy);
					}
				}
				else
				{
					string text = null;
					bool flag = false;
					AttributeCollection attributes = TypeDescriptor.GetAttributes(typeOnly.ObjectType);
					if (attributes != null)
					{
						XmlLangPropertyAttribute xmlLangPropertyAttribute = attributes[typeof(XmlLangPropertyAttribute)] as XmlLangPropertyAttribute;
						if (xmlLangPropertyAttribute != null)
						{
							flag = true;
							text = xmlLangPropertyAttribute.Name;
						}
					}
					if (flag)
					{
						if (text != null && text.Length > 0)
						{
							typeOnly.XmlLangProperty = typeOnly.ObjectType.GetProperty(text, BindingFlags.Instance | BindingFlags.Public | BindingFlags.FlattenHierarchy);
						}
						if (typeOnly.XmlLangProperty == null)
						{
							this.ThrowException("ParserXmlLangPropertyValueInvalid");
						}
					}
				}
			}
			return typeOnly.XmlLangProperty;
		}

		// Token: 0x0600215D RID: 8541 RVA: 0x000A6740 File Offset: 0x000A4940
		private PropertyInfo PropertyInfoFromName(string localName, Type ownerType, bool tryInternal, bool tryPublicOnly, out bool isInternal)
		{
			PropertyInfo propertyInfo = null;
			isInternal = false;
			XamlTypeMapper.TypeInformationCacheData cachedInformationForType = this.GetCachedInformationForType(ownerType);
			XamlTypeMapper.PropertyAndType propertyAndType = cachedInformationForType.GetPropertyAndType(localName);
			if (propertyAndType == null || !propertyAndType.PropInfoSet)
			{
				try
				{
					BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.FlattenHierarchy;
					if (!tryInternal)
					{
						propertyInfo = ownerType.GetProperty(localName, bindingFlags);
					}
					if (propertyInfo == null && !tryPublicOnly)
					{
						propertyInfo = ownerType.GetProperty(localName, bindingFlags | BindingFlags.NonPublic);
						if (propertyInfo != null)
						{
							isInternal = true;
						}
					}
				}
				catch (AmbiguousMatchException)
				{
					PropertyInfo[] properties = ownerType.GetProperties(BindingFlags.Instance | BindingFlags.Public);
					for (int i = 0; i < properties.Length; i++)
					{
						if (properties[i].Name == localName)
						{
							propertyInfo = properties[i];
							break;
						}
					}
				}
				cachedInformationForType.SetPropertyAndType(localName, propertyInfo, ownerType, isInternal);
			}
			else
			{
				propertyInfo = propertyAndType.PropInfo;
				isInternal = propertyAndType.IsInternal;
			}
			return propertyInfo;
		}

		// Token: 0x0600215E RID: 8542 RVA: 0x000A6814 File Offset: 0x000A4A14
		internal RoutedEvent RoutedEventFromName(string localName, Type ownerType)
		{
			Type type = ownerType;
			while (null != type)
			{
				SecurityHelper.RunClassConstructor(type);
				type = this.GetCachedBaseType(type);
			}
			return EventManager.GetRoutedEventFromName(localName, ownerType);
		}

		// Token: 0x0600215F RID: 8543 RVA: 0x000A6848 File Offset: 0x000A4A48
		internal static Type GetPropertyType(object propertyMember)
		{
			Type result;
			bool flag;
			XamlTypeMapper.GetPropertyType(propertyMember, out result, out flag);
			return result;
		}

		// Token: 0x06002160 RID: 8544 RVA: 0x000A6860 File Offset: 0x000A4A60
		internal static void GetPropertyType(object propertyMember, out Type propertyType, out bool propertyCanWrite)
		{
			DependencyProperty dependencyProperty = propertyMember as DependencyProperty;
			if (dependencyProperty != null)
			{
				propertyType = dependencyProperty.PropertyType;
				propertyCanWrite = !dependencyProperty.ReadOnly;
				return;
			}
			PropertyInfo propertyInfo = propertyMember as PropertyInfo;
			if (propertyInfo != null)
			{
				propertyType = propertyInfo.PropertyType;
				propertyCanWrite = propertyInfo.CanWrite;
				return;
			}
			MethodInfo methodInfo = propertyMember as MethodInfo;
			if (methodInfo != null)
			{
				ParameterInfo[] parameters = methodInfo.GetParameters();
				propertyType = ((parameters.Length == 1) ? methodInfo.ReturnType : parameters[1].ParameterType);
				propertyCanWrite = (parameters.Length != 1);
				return;
			}
			propertyType = typeof(object);
			propertyCanWrite = false;
		}

		// Token: 0x06002161 RID: 8545 RVA: 0x000A68F8 File Offset: 0x000A4AF8
		internal static string GetPropertyName(object propertyMember)
		{
			DependencyProperty dependencyProperty = propertyMember as DependencyProperty;
			if (dependencyProperty != null)
			{
				return dependencyProperty.Name;
			}
			PropertyInfo propertyInfo = propertyMember as PropertyInfo;
			if (propertyInfo != null)
			{
				return propertyInfo.Name;
			}
			MethodInfo methodInfo = propertyMember as MethodInfo;
			if (methodInfo != null)
			{
				return methodInfo.Name.Substring("Get".Length);
			}
			return null;
		}

		// Token: 0x06002162 RID: 8546 RVA: 0x000A6954 File Offset: 0x000A4B54
		internal static Type GetDeclaringType(object propertyMember)
		{
			MemberInfo memberInfo = propertyMember as MemberInfo;
			Type result;
			if (memberInfo != null)
			{
				result = memberInfo.DeclaringType;
			}
			else
			{
				result = ((DependencyProperty)propertyMember).OwnerType;
			}
			return result;
		}

		// Token: 0x06002163 RID: 8547 RVA: 0x000A698C File Offset: 0x000A4B8C
		internal static Type GetTypeFromName(string typeName, DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			if (typeName == null)
			{
				throw new ArgumentNullException("typeName");
			}
			int num = typeName.IndexOf(':');
			string text = string.Empty;
			if (num > 0)
			{
				text = typeName.Substring(0, num);
				typeName = typeName.Substring(num + 1, typeName.Length - num - 1);
			}
			XmlnsDictionary xmlnsDictionary = element.GetValue(XmlAttributeProperties.XmlnsDictionaryProperty) as XmlnsDictionary;
			object obj = (xmlnsDictionary != null) ? xmlnsDictionary[text] : null;
			Hashtable hashtable = element.GetValue(XmlAttributeProperties.XmlNamespaceMapsProperty) as Hashtable;
			NamespaceMapEntry[] array = (hashtable != null && obj != null) ? (hashtable[obj] as NamespaceMapEntry[]) : null;
			if (array == null)
			{
				if (text == string.Empty)
				{
					List<ClrNamespaceAssemblyPair> clrNamespacePairFromCache = XamlTypeMapper.GetClrNamespacePairFromCache("http://schemas.microsoft.com/winfx/2006/xaml/presentation");
					foreach (ClrNamespaceAssemblyPair clrNamespaceAssemblyPair in clrNamespacePairFromCache)
					{
						if (clrNamespaceAssemblyPair.AssemblyName != null)
						{
							Assembly assembly = ReflectionHelper.LoadAssembly(clrNamespaceAssemblyPair.AssemblyName, null);
							if (assembly != null)
							{
								string name = string.Format(TypeConverterHelper.InvariantEnglishUS, "{0}.{1}", new object[]
								{
									clrNamespaceAssemblyPair.ClrNamespace,
									typeName
								});
								Type type = assembly.GetType(name);
								if (type != null)
								{
									return type;
								}
							}
						}
					}
				}
				return null;
			}
			for (int i = 0; i < array.Length; i++)
			{
				Assembly assembly2 = array[i].Assembly;
				if (assembly2 != null)
				{
					string name2 = string.Format(TypeConverterHelper.InvariantEnglishUS, "{0}.{1}", new object[]
					{
						array[i].ClrNamespace,
						typeName
					});
					Type type2 = assembly2.GetType(name2);
					if (type2 != null)
					{
						return type2;
					}
				}
			}
			return null;
		}

		// Token: 0x06002164 RID: 8548 RVA: 0x000A6B64 File Offset: 0x000A4D64
		internal Type GetTargetTypeAndMember(string valueParam, ParserContext context, bool isTypeExpected, out string memberName)
		{
			string text = valueParam;
			string text2 = string.Empty;
			int num = text.IndexOf(':');
			if (num >= 0)
			{
				text2 = text.Substring(0, num);
				text = text.Substring(num + 1);
			}
			memberName = null;
			Type type = null;
			num = text.LastIndexOf('.');
			if (num >= 0)
			{
				memberName = text.Substring(num + 1);
				text = text.Substring(0, num);
				string xmlNamespace = context.XmlnsDictionary[text2];
				TypeAndSerializer typeOnly = this.GetTypeOnly(xmlNamespace, text);
				if (typeOnly != null)
				{
					type = typeOnly.ObjectType;
				}
				if (type == null)
				{
					this.ThrowException("ParserNoType", text);
				}
			}
			else if (!isTypeExpected && text2.Length == 0)
			{
				memberName = text;
			}
			else
			{
				this.ThrowException("ParserBadMemberReference", valueParam);
			}
			return type;
		}

		// Token: 0x06002165 RID: 8549 RVA: 0x000A6C1C File Offset: 0x000A4E1C
		internal Type GetDependencyPropertyOwnerAndName(string memberValue, ParserContext context, Type defaultTargetType, out string memberName)
		{
			Type type = this.GetTargetTypeAndMember(memberValue, context, false, out memberName);
			if (type == null)
			{
				type = defaultTargetType;
				if (type == null)
				{
					this.ThrowException("ParserBadMemberReference", memberValue);
				}
			}
			string memberName2 = memberName + "Property";
			MemberInfo staticMemberInfo = this.GetStaticMemberInfo(type, memberName2, true);
			if (staticMemberInfo.DeclaringType != type)
			{
				type = staticMemberInfo.DeclaringType;
			}
			return type;
		}

		// Token: 0x06002166 RID: 8550 RVA: 0x000A6C84 File Offset: 0x000A4E84
		internal MemberInfo GetStaticMemberInfo(Type targetType, string memberName, bool fieldInfoOnly)
		{
			MemberInfo staticMemberInfo = this.GetStaticMemberInfo(targetType, memberName, fieldInfoOnly, false);
			if (staticMemberInfo == null)
			{
				this.ThrowException("ParserInvalidStaticMember", memberName, targetType.Name);
			}
			return staticMemberInfo;
		}

		// Token: 0x06002167 RID: 8551 RVA: 0x000A6CB8 File Offset: 0x000A4EB8
		private MemberInfo GetStaticMemberInfo(Type targetType, string memberName, bool fieldInfoOnly, bool tryInternal)
		{
			MemberInfo memberInfo = null;
			BindingFlags bindingFlags = BindingFlags.Static | BindingFlags.Public | BindingFlags.FlattenHierarchy;
			if (tryInternal)
			{
				bindingFlags |= BindingFlags.NonPublic;
			}
			if (!fieldInfoOnly)
			{
				memberInfo = targetType.GetProperty(memberName, bindingFlags);
			}
			if (memberInfo == null)
			{
				memberInfo = targetType.GetField(memberName, bindingFlags);
			}
			return memberInfo;
		}

		// Token: 0x06002168 RID: 8552 RVA: 0x000A6CF4 File Offset: 0x000A4EF4
		internal TypeAndSerializer GetTypeOnly(string xmlNamespace, string localName)
		{
			string key = xmlNamespace + ":" + localName;
			TypeAndSerializer typeAndSerializer = this._typeLookupFromXmlHashtable[key] as TypeAndSerializer;
			if (typeAndSerializer == null && !this._typeLookupFromXmlHashtable.Contains(key))
			{
				typeAndSerializer = this.CreateTypeAndSerializer(xmlNamespace, localName);
				this._typeLookupFromXmlHashtable[key] = typeAndSerializer;
			}
			return typeAndSerializer;
		}

		// Token: 0x06002169 RID: 8553 RVA: 0x000A6D48 File Offset: 0x000A4F48
		internal TypeAndSerializer GetTypeAndSerializer(string xmlNamespace, string localName, object dpOrPiorMi)
		{
			string key = xmlNamespace + ":" + localName;
			TypeAndSerializer typeAndSerializer = this._typeLookupFromXmlHashtable[key] as TypeAndSerializer;
			if (typeAndSerializer == null && !this._typeLookupFromXmlHashtable.Contains(key))
			{
				typeAndSerializer = this.CreateTypeAndSerializer(xmlNamespace, localName);
				this._typeLookupFromXmlHashtable[key] = typeAndSerializer;
			}
			if (typeAndSerializer != null && !typeAndSerializer.IsSerializerTypeSet)
			{
				typeAndSerializer.SerializerType = this.GetXamlSerializerForType(typeAndSerializer.ObjectType);
				typeAndSerializer.IsSerializerTypeSet = true;
			}
			return typeAndSerializer;
		}

		// Token: 0x0600216A RID: 8554 RVA: 0x000A6DC0 File Offset: 0x000A4FC0
		private TypeAndSerializer CreateTypeAndSerializer(string xmlNamespace, string localName)
		{
			TypeAndSerializer typeAndSerializer = null;
			NamespaceMapEntry[] namespaceMapEntries = this.GetNamespaceMapEntries(xmlNamespace);
			if (namespaceMapEntries != null)
			{
				bool flag = true;
				int i = 0;
				while (i < namespaceMapEntries.Length)
				{
					NamespaceMapEntry namespaceMapEntry = namespaceMapEntries[i];
					if (namespaceMapEntry != null)
					{
						Type objectType = this.GetObjectType(namespaceMapEntry, localName, flag);
						if (null != objectType)
						{
							if (!ReflectionHelper.IsPublicType(objectType) && !this.IsInternalTypeAllowedInFullTrust(objectType))
							{
								this.ThrowException("ParserPublicType", objectType.Name);
							}
							typeAndSerializer = new TypeAndSerializer();
							typeAndSerializer.ObjectType = objectType;
							break;
						}
					}
					i++;
					if (flag && i == namespaceMapEntries.Length)
					{
						flag = false;
						i = 0;
					}
				}
			}
			return typeAndSerializer;
		}

		// Token: 0x0600216B RID: 8555 RVA: 0x000A6E4C File Offset: 0x000A504C
		private Type GetObjectType(NamespaceMapEntry namespaceMap, string localName, bool knownTypesOnly)
		{
			Type result = null;
			if (knownTypesOnly)
			{
				short knownTypeIdFromName = BamlMapTable.GetKnownTypeIdFromName(namespaceMap.AssemblyName, namespaceMap.ClrNamespace, localName);
				if (knownTypeIdFromName != 0)
				{
					result = BamlMapTable.GetKnownTypeFromId(knownTypeIdFromName);
				}
			}
			else
			{
				Assembly assembly = namespaceMap.Assembly;
				if (null != assembly)
				{
					string name = namespaceMap.ClrNamespace + "." + localName;
					try
					{
						result = assembly.GetType(name);
					}
					catch (Exception ex)
					{
						if (CriticalExceptions.IsCriticalException(ex))
						{
							throw;
						}
						if (this.LoadReferenceAssemblies())
						{
							try
							{
								result = assembly.GetType(name);
							}
							catch (ArgumentException)
							{
								result = null;
							}
						}
					}
				}
			}
			return result;
		}

		// Token: 0x0600216C RID: 8556 RVA: 0x000A6EEC File Offset: 0x000A50EC
		internal int GetCustomBamlSerializerIdForType(Type objectType)
		{
			if (objectType == KnownTypes.Types[52])
			{
				return 744;
			}
			if (objectType == KnownTypes.Types[237] || objectType == KnownTypes.Types[609])
			{
				return 746;
			}
			if (objectType == KnownTypes.Types[461])
			{
				return 747;
			}
			if (objectType == KnownTypes.Types[713])
			{
				return 752;
			}
			if (objectType == KnownTypes.Types[472])
			{
				return 748;
			}
			if (objectType == KnownTypes.Types[313])
			{
				return 745;
			}
			return 0;
		}

		// Token: 0x0600216D RID: 8557 RVA: 0x000A6FBC File Offset: 0x000A51BC
		internal Type GetXamlSerializerForType(Type objectType)
		{
			if (objectType == KnownTypes.Types[620])
			{
				return typeof(XamlStyleSerializer);
			}
			if (KnownTypes.Types[231].IsAssignableFrom(objectType))
			{
				return typeof(XamlTemplateSerializer);
			}
			return null;
		}

		// Token: 0x0600216E RID: 8558 RVA: 0x000A7010 File Offset: 0x000A5210
		internal static Type GetInternalTypeHelperTypeFromAssembly(ParserContext pc)
		{
			Assembly streamCreatedAssembly = pc.StreamCreatedAssembly;
			if (streamCreatedAssembly == null)
			{
				return null;
			}
			Type type = streamCreatedAssembly.GetType("XamlGeneratedNamespace.GeneratedInternalTypeHelper");
			if (type == null)
			{
				RootNamespaceAttribute rootNamespaceAttribute = (RootNamespaceAttribute)Attribute.GetCustomAttribute(streamCreatedAssembly, typeof(RootNamespaceAttribute));
				if (rootNamespaceAttribute != null)
				{
					string @namespace = rootNamespaceAttribute.Namespace;
					type = streamCreatedAssembly.GetType(@namespace + ".XamlGeneratedNamespace.GeneratedInternalTypeHelper");
				}
			}
			return type;
		}

		// Token: 0x0600216F RID: 8559 RVA: 0x000A7078 File Offset: 0x000A5278
		private static InternalTypeHelper GetInternalTypeHelperFromAssembly(ParserContext pc)
		{
			InternalTypeHelper result = null;
			Type internalTypeHelperTypeFromAssembly = XamlTypeMapper.GetInternalTypeHelperTypeFromAssembly(pc);
			if (internalTypeHelperTypeFromAssembly != null)
			{
				result = (InternalTypeHelper)Activator.CreateInstance(internalTypeHelperTypeFromAssembly);
			}
			return result;
		}

		// Token: 0x06002170 RID: 8560 RVA: 0x000A70A4 File Offset: 0x000A52A4
		internal static object CreateInternalInstance(ParserContext pc, Type type)
		{
			object result = null;
			if (SecurityHelper.CallerHasMemberAccessReflectionPermission())
			{
				result = Activator.CreateInstance(type, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.CreateInstance, null, null, TypeConverterHelper.InvariantEnglishUS);
			}
			else
			{
				InternalTypeHelper internalTypeHelperFromAssembly = XamlTypeMapper.GetInternalTypeHelperFromAssembly(pc);
				if (internalTypeHelperFromAssembly != null)
				{
					result = internalTypeHelperFromAssembly.CreateInstance(type, TypeConverterHelper.InvariantEnglishUS);
				}
			}
			return result;
		}

		// Token: 0x06002171 RID: 8561 RVA: 0x000A70E8 File Offset: 0x000A52E8
		internal static object GetInternalPropertyValue(ParserContext pc, object rootElement, PropertyInfo pi, object target)
		{
			object result = null;
			bool flag = false;
			bool allowProtected = rootElement is IComponentConnector && rootElement == target;
			bool flag2 = XamlTypeMapper.IsAllowedPropertyGet(pi, allowProtected, out flag);
			if (flag2)
			{
				if (flag || SecurityHelper.CallerHasMemberAccessReflectionPermission())
				{
					result = pi.GetValue(target, BindingFlags.Default, null, null, TypeConverterHelper.InvariantEnglishUS);
				}
				else
				{
					InternalTypeHelper internalTypeHelperFromAssembly = XamlTypeMapper.GetInternalTypeHelperFromAssembly(pc);
					if (internalTypeHelperFromAssembly != null)
					{
						result = internalTypeHelperFromAssembly.GetPropertyValue(pi, target, TypeConverterHelper.InvariantEnglishUS);
					}
				}
			}
			return result;
		}

		// Token: 0x06002172 RID: 8562 RVA: 0x000A7150 File Offset: 0x000A5350
		internal static bool SetInternalPropertyValue(ParserContext pc, object rootElement, PropertyInfo pi, object target, object value)
		{
			bool flag = false;
			bool allowProtected = rootElement is IComponentConnector && rootElement == target;
			bool flag2 = XamlTypeMapper.IsAllowedPropertySet(pi, allowProtected, out flag);
			if (flag2)
			{
				if (flag || SecurityHelper.CallerHasMemberAccessReflectionPermission())
				{
					pi.SetValue(target, value, BindingFlags.Default, null, null, TypeConverterHelper.InvariantEnglishUS);
					return true;
				}
				InternalTypeHelper internalTypeHelperFromAssembly = XamlTypeMapper.GetInternalTypeHelperFromAssembly(pc);
				if (internalTypeHelperFromAssembly != null)
				{
					internalTypeHelperFromAssembly.SetPropertyValue(pi, target, value, TypeConverterHelper.InvariantEnglishUS);
					return true;
				}
			}
			return false;
		}

		// Token: 0x06002173 RID: 8563 RVA: 0x000A71B8 File Offset: 0x000A53B8
		internal static Delegate CreateDelegate(ParserContext pc, Type delegateType, object target, string handler)
		{
			Delegate result = null;
			bool flag = ReflectionHelper.IsPublicType(delegateType) || ReflectionHelper.IsInternalType(delegateType);
			if (flag)
			{
				if (SecurityHelper.CallerHasMemberAccessReflectionPermission())
				{
					result = Delegate.CreateDelegate(delegateType, target, handler);
				}
				else if (target.GetType().Assembly == pc.StreamCreatedAssembly)
				{
					InternalTypeHelper internalTypeHelperFromAssembly = XamlTypeMapper.GetInternalTypeHelperFromAssembly(pc);
					if (internalTypeHelperFromAssembly != null)
					{
						result = internalTypeHelperFromAssembly.CreateDelegate(delegateType, target, handler);
					}
				}
			}
			return result;
		}

		// Token: 0x06002174 RID: 8564 RVA: 0x000A721C File Offset: 0x000A541C
		internal static bool AddInternalEventHandler(ParserContext pc, object rootElement, EventInfo eventInfo, object target, Delegate handler)
		{
			bool flag = false;
			bool allowProtected = rootElement == target;
			bool flag2 = XamlTypeMapper.IsAllowedEvent(eventInfo, allowProtected, out flag);
			if (flag2)
			{
				if (flag || SecurityHelper.CallerHasMemberAccessReflectionPermission())
				{
					eventInfo.AddEventHandler(target, handler);
					return true;
				}
				InternalTypeHelper internalTypeHelperFromAssembly = XamlTypeMapper.GetInternalTypeHelperFromAssembly(pc);
				if (internalTypeHelperFromAssembly != null)
				{
					internalTypeHelperFromAssembly.AddEventHandler(eventInfo, target, handler);
					return true;
				}
			}
			return false;
		}

		// Token: 0x06002175 RID: 8565 RVA: 0x000A726C File Offset: 0x000A546C
		internal bool IsLocalAssembly(string namespaceUri)
		{
			return false;
		}

		// Token: 0x06002176 RID: 8566 RVA: 0x000A727C File Offset: 0x000A547C
		internal Type GetTypeFromBaseString(string typeString, ParserContext context, bool throwOnError)
		{
			string text = string.Empty;
			Type type = null;
			int num = typeString.IndexOf(':');
			if (num == -1)
			{
				text = context.XmlnsDictionary[string.Empty];
				if (text == null)
				{
					this.ThrowException("ParserUndeclaredNS", string.Empty);
				}
			}
			else
			{
				string text2 = typeString.Substring(0, num);
				text = context.XmlnsDictionary[text2];
				if (text == null)
				{
					this.ThrowException("ParserUndeclaredNS", text2);
				}
				else
				{
					typeString = typeString.Substring(num + 1);
				}
			}
			if (string.CompareOrdinal(text, "http://schemas.microsoft.com/winfx/2006/xaml/presentation") == 0)
			{
				if (!(typeString == "SystemParameters"))
				{
					if (!(typeString == "SystemColors"))
					{
						if (typeString == "SystemFonts")
						{
							type = typeof(SystemFonts);
						}
					}
					else
					{
						type = typeof(SystemColors);
					}
				}
				else
				{
					type = typeof(SystemParameters);
				}
			}
			if (type == null)
			{
				type = this.GetType(text, typeString);
			}
			if (type == null && throwOnError && !this.IsLocalAssembly(text))
			{
				this._lineNumber = ((context != null) ? context.LineNumber : 0);
				this._linePosition = ((context != null) ? context.LinePosition : 0);
				this.ThrowException("ParserResourceKeyType", typeString);
			}
			return type;
		}

		// Token: 0x06002177 RID: 8567 RVA: 0x000A73A8 File Offset: 0x000A55A8
		private XamlTypeMapper.TypeInformationCacheData GetCachedInformationForType(Type type)
		{
			XamlTypeMapper.TypeInformationCacheData typeInformationCacheData = this._typeInformationCache[type] as XamlTypeMapper.TypeInformationCacheData;
			if (typeInformationCacheData == null)
			{
				typeInformationCacheData = new XamlTypeMapper.TypeInformationCacheData(type.BaseType);
				typeInformationCacheData.ClrNamespace = type.Namespace;
				this._typeInformationCache[type] = typeInformationCacheData;
			}
			return typeInformationCacheData;
		}

		// Token: 0x06002178 RID: 8568 RVA: 0x000A73F0 File Offset: 0x000A55F0
		private Type GetCachedBaseType(Type t)
		{
			XamlTypeMapper.TypeInformationCacheData cachedInformationForType = this.GetCachedInformationForType(t);
			return cachedInformationForType.BaseType;
		}

		// Token: 0x06002179 RID: 8569 RVA: 0x000A740C File Offset: 0x000A560C
		internal static string ProcessNameString(ParserContext parserContext, ref string nameString)
		{
			int num = nameString.IndexOf(':');
			string text = string.Empty;
			if (num != -1)
			{
				text = nameString.Substring(0, num);
				nameString = nameString.Substring(num + 1);
			}
			string text2 = parserContext.XmlnsDictionary[text];
			if (text2 == null)
			{
				parserContext.XamlTypeMapper.ThrowException("ParserPrefixNSProperty", text, nameString);
			}
			return text2;
		}

		// Token: 0x0600217A RID: 8570 RVA: 0x000A7468 File Offset: 0x000A5668
		internal static DependencyProperty ParsePropertyName(ParserContext parserContext, string propertyName, ref Type ownerType)
		{
			string xmlNamespace = XamlTypeMapper.ProcessNameString(parserContext, ref propertyName);
			return parserContext.XamlTypeMapper.DependencyPropertyFromName(propertyName, xmlNamespace, ref ownerType);
		}

		// Token: 0x0600217B RID: 8571 RVA: 0x000A7490 File Offset: 0x000A5690
		internal static RoutedEvent ParseEventName(ParserContext parserContext, string eventName, Type ownerType)
		{
			string xmlNamespace = XamlTypeMapper.ProcessNameString(parserContext, ref eventName);
			return parserContext.XamlTypeMapper.GetRoutedEvent(ownerType, xmlNamespace, eventName);
		}

		// Token: 0x0600217C RID: 8572 RVA: 0x000A74B8 File Offset: 0x000A56B8
		internal object CreateInstance(Type t)
		{
			short knownTypeIdFromType = BamlMapTable.GetKnownTypeIdFromType(t);
			object result;
			if (knownTypeIdFromType < 0)
			{
				result = this.MapTable.CreateKnownTypeFromId(knownTypeIdFromType);
			}
			else
			{
				result = Activator.CreateInstance(t, BindingFlags.Instance | BindingFlags.Public | BindingFlags.CreateInstance, null, null, TypeConverterHelper.InvariantEnglishUS);
			}
			return result;
		}

		// Token: 0x0600217D RID: 8573 RVA: 0x000A74F8 File Offset: 0x000A56F8
		internal bool IsXmlNamespaceKnown(string xmlNamespace, out string newXmlNamespace)
		{
			bool result;
			if (string.IsNullOrEmpty(xmlNamespace))
			{
				result = false;
				newXmlNamespace = null;
			}
			else
			{
				NamespaceMapEntry[] namespaceMapEntries = this.GetNamespaceMapEntries(xmlNamespace);
				if (XamlTypeMapper._xmlnsCache == null)
				{
					XamlTypeMapper._xmlnsCache = new XmlnsCache();
				}
				newXmlNamespace = XamlTypeMapper._xmlnsCache.GetNewXmlnamespace(xmlNamespace);
				result = ((namespaceMapEntries != null && namespaceMapEntries.Length != 0) || !string.IsNullOrEmpty(newXmlNamespace));
			}
			return result;
		}

		// Token: 0x0600217E RID: 8574 RVA: 0x000A7550 File Offset: 0x000A5750
		internal void SetUriToAssemblyNameMapping(string xmlNamespace, short[] assemblyIds)
		{
			if (xmlNamespace.StartsWith("clr-namespace:", StringComparison.Ordinal))
			{
				return;
			}
			if (XamlTypeMapper._xmlnsCache == null)
			{
				XamlTypeMapper._xmlnsCache = new XmlnsCache();
			}
			string[] array = null;
			if (assemblyIds != null && assemblyIds.Length != 0)
			{
				array = new string[assemblyIds.Length];
				for (int i = 0; i < assemblyIds.Length; i++)
				{
					BamlAssemblyInfoRecord assemblyInfoFromId = this.MapTable.GetAssemblyInfoFromId(assemblyIds[i]);
					array[i] = assemblyInfoFromId.AssemblyFullName;
				}
			}
			XamlTypeMapper._xmlnsCache.SetUriToAssemblyNameMapping(xmlNamespace, array);
		}

		// Token: 0x0600217F RID: 8575 RVA: 0x000A75C4 File Offset: 0x000A57C4
		internal NamespaceMapEntry[] GetNamespaceMapEntries(string xmlNamespace)
		{
			NamespaceMapEntry[] array = this._namespaceMapHashList[xmlNamespace] as NamespaceMapEntry[];
			if (array == null)
			{
				ArrayList arrayList = new ArrayList(6);
				if (this._namespaceMaps != null)
				{
					for (int i = 0; i < this._namespaceMaps.Length; i++)
					{
						NamespaceMapEntry namespaceMapEntry = this._namespaceMaps[i];
						if (namespaceMapEntry.XmlNamespace == xmlNamespace)
						{
							arrayList.Add(namespaceMapEntry);
						}
					}
				}
				List<ClrNamespaceAssemblyPair> list;
				if (this.PITable.Contains(xmlNamespace))
				{
					list = new List<ClrNamespaceAssemblyPair>(1);
					list.Add((ClrNamespaceAssemblyPair)this.PITable[xmlNamespace]);
				}
				else
				{
					list = XamlTypeMapper.GetClrNamespacePairFromCache(xmlNamespace);
				}
				if (list != null)
				{
					for (int j = 0; j < list.Count; j++)
					{
						ClrNamespaceAssemblyPair clrNamespaceAssemblyPair = list[j];
						string text = null;
						string assemblyPath = this.AssemblyPathFor(clrNamespaceAssemblyPair.AssemblyName);
						if (!string.IsNullOrEmpty(clrNamespaceAssemblyPair.AssemblyName) && !string.IsNullOrEmpty(clrNamespaceAssemblyPair.ClrNamespace))
						{
							text = clrNamespaceAssemblyPair.AssemblyName;
							NamespaceMapEntry value = new NamespaceMapEntry(xmlNamespace, text, clrNamespaceAssemblyPair.ClrNamespace, assemblyPath);
							arrayList.Add(value);
						}
						if (!string.IsNullOrEmpty(clrNamespaceAssemblyPair.ClrNamespace))
						{
							for (int k = 0; k < this._assemblyNames.Length; k++)
							{
								if (text == null)
								{
									arrayList.Add(new NamespaceMapEntry(xmlNamespace, this._assemblyNames[k], clrNamespaceAssemblyPair.ClrNamespace, assemblyPath));
								}
								else
								{
									int num = this._assemblyNames[k].LastIndexOf('\\');
									if (num > 0 && this._assemblyNames[k].Substring(num + 1) == text)
									{
										arrayList.Add(new NamespaceMapEntry(xmlNamespace, this._assemblyNames[k], clrNamespaceAssemblyPair.ClrNamespace, assemblyPath));
									}
								}
							}
						}
					}
				}
				array = (NamespaceMapEntry[])arrayList.ToArray(typeof(NamespaceMapEntry));
				if (array != null)
				{
					this._namespaceMapHashList.Add(xmlNamespace, array);
				}
			}
			return array;
		}

		// Token: 0x06002180 RID: 8576 RVA: 0x000A77AC File Offset: 0x000A59AC
		internal string GetXmlNamespace(string clrNamespaceFullName, string assemblyFullName)
		{
			string str = assemblyFullName.ToUpper(TypeConverterHelper.InvariantEnglishUS);
			string key = clrNamespaceFullName + "#" + str;
			string text;
			if (this._piReverseTable.TryGetValue(key, out text) && text != null)
			{
				return text;
			}
			return string.Empty;
		}

		// Token: 0x06002181 RID: 8577 RVA: 0x000A77EC File Offset: 0x000A59EC
		private string GetCachedNamespace(Type t)
		{
			XamlTypeMapper.TypeInformationCacheData cachedInformationForType = this.GetCachedInformationForType(t);
			return cachedInformationForType.ClrNamespace;
		}

		// Token: 0x06002182 RID: 8578 RVA: 0x000A7808 File Offset: 0x000A5A08
		internal static List<ClrNamespaceAssemblyPair> GetClrNamespacePairFromCache(string namespaceUri)
		{
			if (XamlTypeMapper._xmlnsCache == null)
			{
				XamlTypeMapper._xmlnsCache = new XmlnsCache();
			}
			return XamlTypeMapper._xmlnsCache.GetMappingArray(namespaceUri);
		}

		// Token: 0x06002183 RID: 8579 RVA: 0x000A7838 File Offset: 0x000A5A38
		internal Type GetTypeConverterType(Type type)
		{
			XamlTypeMapper.TypeInformationCacheData cachedInformationForType = this.GetCachedInformationForType(type);
			if (null != cachedInformationForType.TypeConverterType)
			{
				return cachedInformationForType.TypeConverterType;
			}
			Type type2 = this.MapTable.GetKnownConverterTypeFromType(type);
			if (type2 == null)
			{
				type2 = TypeConverterHelper.GetConverterType(type);
				if (type2 == null)
				{
					type2 = TypeConverterHelper.GetCoreConverterTypeFromCustomType(type);
				}
			}
			cachedInformationForType.TypeConverterType = type2;
			return type2;
		}

		// Token: 0x06002184 RID: 8580 RVA: 0x000A789C File Offset: 0x000A5A9C
		internal TypeConverter GetTypeConverter(Type type)
		{
			XamlTypeMapper.TypeInformationCacheData cachedInformationForType = this.GetCachedInformationForType(type);
			if (cachedInformationForType.Converter != null)
			{
				return cachedInformationForType.Converter;
			}
			TypeConverter typeConverter = this.MapTable.GetKnownConverterFromType(type);
			if (typeConverter == null)
			{
				Type converterType = TypeConverterHelper.GetConverterType(type);
				if (converterType == null)
				{
					typeConverter = TypeConverterHelper.GetCoreConverterFromCustomType(type);
				}
				else
				{
					typeConverter = (this.CreateInstance(converterType) as TypeConverter);
				}
			}
			cachedInformationForType.Converter = typeConverter;
			if (typeConverter == null)
			{
				this.ThrowException("ParserNoTypeConv", type.Name);
			}
			return typeConverter;
		}

		// Token: 0x06002185 RID: 8581 RVA: 0x000A7918 File Offset: 0x000A5B18
		internal Type GetPropertyConverterType(Type propType, object dpOrPiOrMi)
		{
			Type result = null;
			if (dpOrPiOrMi != null)
			{
				MemberInfo memberInfoForPropertyConverter = TypeConverterHelper.GetMemberInfoForPropertyConverter(dpOrPiOrMi);
				if (memberInfoForPropertyConverter != null)
				{
					result = TypeConverterHelper.GetConverterType(memberInfoForPropertyConverter);
				}
			}
			return result;
		}

		// Token: 0x06002186 RID: 8582 RVA: 0x000A7944 File Offset: 0x000A5B44
		internal TypeConverter GetPropertyConverter(Type propType, object dpOrPiOrMi)
		{
			TypeConverter typeConverter = null;
			XamlTypeMapper.TypeInformationCacheData cachedInformationForType = this.GetCachedInformationForType(propType);
			if (dpOrPiOrMi != null)
			{
				object obj = cachedInformationForType.PropertyConverters[dpOrPiOrMi];
				if (obj != null)
				{
					return (TypeConverter)obj;
				}
				MemberInfo memberInfoForPropertyConverter = TypeConverterHelper.GetMemberInfoForPropertyConverter(dpOrPiOrMi);
				if (memberInfoForPropertyConverter != null)
				{
					Type converterType = TypeConverterHelper.GetConverterType(memberInfoForPropertyConverter);
					if (converterType != null)
					{
						typeConverter = (this.CreateInstance(converterType) as TypeConverter);
					}
				}
			}
			if (typeConverter == null)
			{
				typeConverter = this.GetTypeConverter(propType);
			}
			if (dpOrPiOrMi != null)
			{
				cachedInformationForType.SetPropertyConverter(dpOrPiOrMi, typeConverter);
			}
			return typeConverter;
		}

		// Token: 0x06002187 RID: 8583 RVA: 0x000A79BE File Offset: 0x000A5BBE
		internal object GetDictionaryKey(string keyString, ParserContext context)
		{
			if (keyString.Length > 0 && (char.IsWhiteSpace(keyString[0]) || char.IsWhiteSpace(keyString[keyString.Length - 1])))
			{
				keyString = keyString.Trim();
			}
			return keyString;
		}

		// Token: 0x06002188 RID: 8584 RVA: 0x000A79F8 File Offset: 0x000A5BF8
		internal XamlTypeMapper.ConstructorData GetConstructors(Type type)
		{
			if (this._constructorInformationCache == null)
			{
				this._constructorInformationCache = new HybridDictionary(3);
			}
			if (!this._constructorInformationCache.Contains(type))
			{
				this._constructorInformationCache[type] = new XamlTypeMapper.ConstructorData(type.GetConstructors(BindingFlags.Instance | BindingFlags.Public));
			}
			return (XamlTypeMapper.ConstructorData)this._constructorInformationCache[type];
		}

		// Token: 0x06002189 RID: 8585 RVA: 0x000A7A54 File Offset: 0x000A5C54
		internal bool GetCachedTrimSurroundingWhitespace(Type t)
		{
			XamlTypeMapper.TypeInformationCacheData cachedInformationForType = this.GetCachedInformationForType(t);
			if (!cachedInformationForType.TrimSurroundingWhitespaceSet)
			{
				cachedInformationForType.TrimSurroundingWhitespace = this.GetTrimSurroundingWhitespace(t);
				cachedInformationForType.TrimSurroundingWhitespaceSet = true;
			}
			return cachedInformationForType.TrimSurroundingWhitespace;
		}

		// Token: 0x0600218A RID: 8586 RVA: 0x000A7A8C File Offset: 0x000A5C8C
		private bool GetTrimSurroundingWhitespace(Type type)
		{
			if (null != type)
			{
				TrimSurroundingWhitespaceAttribute[] array = type.GetCustomAttributes(typeof(TrimSurroundingWhitespaceAttribute), true) as TrimSurroundingWhitespaceAttribute[];
				if (array.Length != 0)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600218B RID: 8587 RVA: 0x000A7AC0 File Offset: 0x000A5CC0
		private void ThrowException(string id)
		{
			this.ThrowExceptionWithLine(SR.Get(id), null);
		}

		// Token: 0x0600218C RID: 8588 RVA: 0x000A7ACF File Offset: 0x000A5CCF
		internal void ThrowException(string id, string parameter)
		{
			this.ThrowExceptionWithLine(SR.Get(id, new object[]
			{
				parameter
			}), null);
		}

		// Token: 0x0600218D RID: 8589 RVA: 0x000A7AE8 File Offset: 0x000A5CE8
		private void ThrowException(string id, string parameter1, string parameter2)
		{
			this.ThrowExceptionWithLine(SR.Get(id, new object[]
			{
				parameter1,
				parameter2
			}), null);
		}

		// Token: 0x0600218E RID: 8590 RVA: 0x000A7B05 File Offset: 0x000A5D05
		private void ThrowException(string id, string parameter1, string parameter2, string parameter3)
		{
			this.ThrowExceptionWithLine(SR.Get(id, new object[]
			{
				parameter1,
				parameter2,
				parameter3
			}), null);
		}

		// Token: 0x0600218F RID: 8591 RVA: 0x000A7B27 File Offset: 0x000A5D27
		internal void ThrowExceptionWithLine(string message, Exception innerException)
		{
			XamlParseException.ThrowException(message, innerException, this._lineNumber, this._linePosition);
		}

		// Token: 0x17000803 RID: 2051
		// (get) Token: 0x06002190 RID: 8592 RVA: 0x000A7B3C File Offset: 0x000A5D3C
		internal HybridDictionary PITable
		{
			get
			{
				return this._piTable;
			}
		}

		// Token: 0x17000804 RID: 2052
		// (get) Token: 0x06002191 RID: 8593 RVA: 0x000A7B44 File Offset: 0x000A5D44
		// (set) Token: 0x06002192 RID: 8594 RVA: 0x000A7B4C File Offset: 0x000A5D4C
		internal BamlMapTable MapTable
		{
			get
			{
				return this._mapTable;
			}
			set
			{
				this._mapTable = value;
			}
		}

		// Token: 0x17000805 RID: 2053
		// (set) Token: 0x06002193 RID: 8595 RVA: 0x000A7B55 File Offset: 0x000A5D55
		internal int LineNumber
		{
			set
			{
				this._lineNumber = value;
			}
		}

		// Token: 0x17000806 RID: 2054
		// (set) Token: 0x06002194 RID: 8596 RVA: 0x000A7B5E File Offset: 0x000A5D5E
		internal int LinePosition
		{
			set
			{
				this._linePosition = value;
			}
		}

		// Token: 0x17000807 RID: 2055
		// (get) Token: 0x06002195 RID: 8597 RVA: 0x000A7B67 File Offset: 0x000A5D67
		internal Hashtable NamespaceMapHashList
		{
			get
			{
				return this._namespaceMapHashList;
			}
		}

		// Token: 0x17000808 RID: 2056
		// (get) Token: 0x06002196 RID: 8598 RVA: 0x000A7B6F File Offset: 0x000A5D6F
		internal XamlSchemaContext SchemaContext
		{
			get
			{
				if (this._schemaContext == null)
				{
					this._schemaContext = new XamlTypeMapper.XamlTypeMapperSchemaContext(this);
				}
				return this._schemaContext;
			}
		}

		// Token: 0x0400198B RID: 6539
		internal const string MarkupExtensionTypeString = "Type ";

		// Token: 0x0400198C RID: 6540
		internal const string MarkupExtensionStaticString = "Static ";

		// Token: 0x0400198D RID: 6541
		internal const string MarkupExtensionDynamicResourceString = "DynamicResource ";

		// Token: 0x0400198E RID: 6542
		internal const string PresentationFrameworkDllName = "PresentationFramework";

		// Token: 0x0400198F RID: 6543
		internal const string GeneratedNamespace = "XamlGeneratedNamespace";

		// Token: 0x04001990 RID: 6544
		internal const string GeneratedInternalTypeHelperClassName = "GeneratedInternalTypeHelper";

		// Token: 0x04001991 RID: 6545
		internal const string MarkupExtensionTemplateBindingString = "TemplateBinding ";

		// Token: 0x04001992 RID: 6546
		private BamlMapTable _mapTable;

		// Token: 0x04001993 RID: 6547
		private string[] _assemblyNames;

		// Token: 0x04001994 RID: 6548
		private NamespaceMapEntry[] _namespaceMaps;

		// Token: 0x04001995 RID: 6549
		private Hashtable _typeLookupFromXmlHashtable = new Hashtable();

		// Token: 0x04001996 RID: 6550
		private Hashtable _namespaceMapHashList = new Hashtable();

		// Token: 0x04001997 RID: 6551
		private HybridDictionary _typeInformationCache = new HybridDictionary();

		// Token: 0x04001998 RID: 6552
		private HybridDictionary _constructorInformationCache;

		// Token: 0x04001999 RID: 6553
		private XamlTypeMapper.XamlTypeMapperSchemaContext _schemaContext;

		// Token: 0x0400199A RID: 6554
		private HybridDictionary _piTable = new HybridDictionary();

		// Token: 0x0400199B RID: 6555
		private Dictionary<string, string> _piReverseTable = new Dictionary<string, string>();

		// Token: 0x0400199C RID: 6556
		private HybridDictionary _assemblyPathTable = new HybridDictionary();

		// Token: 0x0400199D RID: 6557
		private bool _referenceAssembliesLoaded;

		// Token: 0x0400199E RID: 6558
		private int _lineNumber;

		// Token: 0x0400199F RID: 6559
		private int _linePosition;

		// Token: 0x040019A0 RID: 6560
		private static XmlnsCache _xmlnsCache;

		// Token: 0x02000890 RID: 2192
		internal class ConstructorData
		{
			// Token: 0x0600836F RID: 33647 RVA: 0x0024519F File Offset: 0x0024339F
			internal ConstructorData(ConstructorInfo[] constructors)
			{
				this._constructors = constructors;
			}

			// Token: 0x06008370 RID: 33648 RVA: 0x002451B0 File Offset: 0x002433B0
			internal ParameterInfo[] GetParameters(int constructorIndex)
			{
				if (this._parameters == null)
				{
					this._parameters = new ParameterInfo[this._constructors.Length][];
				}
				if (this._parameters[constructorIndex] == null)
				{
					this._parameters[constructorIndex] = this._constructors[constructorIndex].GetParameters();
				}
				return this._parameters[constructorIndex];
			}

			// Token: 0x17001DCD RID: 7629
			// (get) Token: 0x06008371 RID: 33649 RVA: 0x002451FF File Offset: 0x002433FF
			internal ConstructorInfo[] Constructors
			{
				get
				{
					return this._constructors;
				}
			}

			// Token: 0x0400417B RID: 16763
			private ConstructorInfo[] _constructors;

			// Token: 0x0400417C RID: 16764
			private ParameterInfo[][] _parameters;
		}

		// Token: 0x02000891 RID: 2193
		internal class TypeInformationCacheData
		{
			// Token: 0x06008372 RID: 33650 RVA: 0x00245207 File Offset: 0x00243407
			internal TypeInformationCacheData(Type baseType)
			{
				this._baseType = baseType;
			}

			// Token: 0x17001DCE RID: 7630
			// (get) Token: 0x06008373 RID: 33651 RVA: 0x00245221 File Offset: 0x00243421
			// (set) Token: 0x06008374 RID: 33652 RVA: 0x00245229 File Offset: 0x00243429
			internal string ClrNamespace
			{
				get
				{
					return this._clrNamespace;
				}
				set
				{
					this._clrNamespace = value;
				}
			}

			// Token: 0x17001DCF RID: 7631
			// (get) Token: 0x06008375 RID: 33653 RVA: 0x00245232 File Offset: 0x00243432
			internal Type BaseType
			{
				get
				{
					return this._baseType;
				}
			}

			// Token: 0x17001DD0 RID: 7632
			// (get) Token: 0x06008376 RID: 33654 RVA: 0x0024523A File Offset: 0x0024343A
			// (set) Token: 0x06008377 RID: 33655 RVA: 0x00245242 File Offset: 0x00243442
			internal TypeConverter Converter
			{
				get
				{
					return this._typeConverter;
				}
				set
				{
					this._typeConverter = value;
				}
			}

			// Token: 0x17001DD1 RID: 7633
			// (get) Token: 0x06008378 RID: 33656 RVA: 0x0024524B File Offset: 0x0024344B
			// (set) Token: 0x06008379 RID: 33657 RVA: 0x00245253 File Offset: 0x00243453
			internal Type TypeConverterType
			{
				get
				{
					return this._typeConverterType;
				}
				set
				{
					this._typeConverterType = value;
				}
			}

			// Token: 0x17001DD2 RID: 7634
			// (get) Token: 0x0600837A RID: 33658 RVA: 0x0024525C File Offset: 0x0024345C
			// (set) Token: 0x0600837B RID: 33659 RVA: 0x00245264 File Offset: 0x00243464
			internal bool TrimSurroundingWhitespace
			{
				get
				{
					return this._trimSurroundingWhitespace;
				}
				set
				{
					this._trimSurroundingWhitespace = value;
				}
			}

			// Token: 0x17001DD3 RID: 7635
			// (get) Token: 0x0600837C RID: 33660 RVA: 0x0024526D File Offset: 0x0024346D
			// (set) Token: 0x0600837D RID: 33661 RVA: 0x00245275 File Offset: 0x00243475
			internal bool TrimSurroundingWhitespaceSet
			{
				get
				{
					return this._trimSurroundingWhitespaceSet;
				}
				set
				{
					this._trimSurroundingWhitespaceSet = value;
				}
			}

			// Token: 0x0600837E RID: 33662 RVA: 0x0024527E File Offset: 0x0024347E
			internal XamlTypeMapper.PropertyAndType GetPropertyAndType(string dpName)
			{
				if (this._dpLookupHashtable == null)
				{
					this._dpLookupHashtable = new Hashtable();
					return null;
				}
				return this._dpLookupHashtable[dpName] as XamlTypeMapper.PropertyAndType;
			}

			// Token: 0x0600837F RID: 33663 RVA: 0x002452A8 File Offset: 0x002434A8
			internal void SetPropertyAndType(string dpName, PropertyInfo dpInfo, Type ownerType, bool isInternal)
			{
				XamlTypeMapper.PropertyAndType propertyAndType = this._dpLookupHashtable[dpName] as XamlTypeMapper.PropertyAndType;
				if (propertyAndType == null)
				{
					this._dpLookupHashtable[dpName] = new XamlTypeMapper.PropertyAndType(null, dpInfo, false, true, ownerType, isInternal);
					return;
				}
				propertyAndType.PropInfo = dpInfo;
				propertyAndType.PropInfoSet = true;
				propertyAndType.IsInternal = isInternal;
			}

			// Token: 0x17001DD4 RID: 7636
			// (get) Token: 0x06008380 RID: 33664 RVA: 0x002452F9 File Offset: 0x002434F9
			internal HybridDictionary PropertyConverters
			{
				get
				{
					if (this._propertyConverters == null)
					{
						this._propertyConverters = new HybridDictionary();
					}
					return this._propertyConverters;
				}
			}

			// Token: 0x06008381 RID: 33665 RVA: 0x00245314 File Offset: 0x00243514
			internal void SetPropertyConverter(object dpOrPi, TypeConverter converter)
			{
				this._propertyConverters[dpOrPi] = converter;
			}

			// Token: 0x0400417D RID: 16765
			private string _clrNamespace;

			// Token: 0x0400417E RID: 16766
			private Type _baseType;

			// Token: 0x0400417F RID: 16767
			private bool _trimSurroundingWhitespace;

			// Token: 0x04004180 RID: 16768
			private Hashtable _dpLookupHashtable;

			// Token: 0x04004181 RID: 16769
			private HybridDictionary _propertyConverters = new HybridDictionary();

			// Token: 0x04004182 RID: 16770
			private bool _trimSurroundingWhitespaceSet;

			// Token: 0x04004183 RID: 16771
			private TypeConverter _typeConverter;

			// Token: 0x04004184 RID: 16772
			private Type _typeConverterType;
		}

		// Token: 0x02000892 RID: 2194
		internal class PropertyAndType
		{
			// Token: 0x06008382 RID: 33666 RVA: 0x00245323 File Offset: 0x00243523
			public PropertyAndType(MethodInfo dpSetter, PropertyInfo dpInfo, bool setterSet, bool propInfoSet, Type ot, bool isInternal)
			{
				this.Setter = dpSetter;
				this.PropInfo = dpInfo;
				this.OwnerType = ot;
				this.SetterSet = setterSet;
				this.PropInfoSet = propInfoSet;
				this.IsInternal = isInternal;
			}

			// Token: 0x04004185 RID: 16773
			public PropertyInfo PropInfo;

			// Token: 0x04004186 RID: 16774
			public MethodInfo Setter;

			// Token: 0x04004187 RID: 16775
			public Type OwnerType;

			// Token: 0x04004188 RID: 16776
			public bool PropInfoSet;

			// Token: 0x04004189 RID: 16777
			public bool SetterSet;

			// Token: 0x0400418A RID: 16778
			public bool IsInternal;
		}

		// Token: 0x02000893 RID: 2195
		internal class XamlTypeMapperSchemaContext : XamlSchemaContext
		{
			// Token: 0x06008383 RID: 33667 RVA: 0x00245358 File Offset: 0x00243558
			internal XamlTypeMapperSchemaContext(XamlTypeMapper typeMapper)
			{
				this._typeMapper = typeMapper;
				this._sharedSchemaContext = (WpfSharedXamlSchemaContext)XamlReader.GetWpfSchemaContext();
				if (typeMapper._namespaceMaps != null)
				{
					this._nsDefinitions = new Dictionary<string, FrugalObjectList<string>>();
					foreach (NamespaceMapEntry namespaceMapEntry in this._typeMapper._namespaceMaps)
					{
						FrugalObjectList<string> frugalObjectList;
						if (!this._nsDefinitions.TryGetValue(namespaceMapEntry.XmlNamespace, out frugalObjectList))
						{
							frugalObjectList = new FrugalObjectList<string>(1);
							this._nsDefinitions.Add(namespaceMapEntry.XmlNamespace, frugalObjectList);
						}
						string clrNsUri = XamlTypeMapper.XamlTypeMapperSchemaContext.GetClrNsUri(namespaceMapEntry.ClrNamespace, namespaceMapEntry.AssemblyName);
						frugalObjectList.Add(clrNsUri);
					}
				}
				if (typeMapper.PITable.Count > 0)
				{
					this._piNamespaces = new Dictionary<string, string>(typeMapper.PITable.Count);
					foreach (object obj in typeMapper.PITable)
					{
						DictionaryEntry dictionaryEntry = (DictionaryEntry)obj;
						ClrNamespaceAssemblyPair clrNamespaceAssemblyPair = (ClrNamespaceAssemblyPair)dictionaryEntry.Value;
						string clrNsUri2 = XamlTypeMapper.XamlTypeMapperSchemaContext.GetClrNsUri(clrNamespaceAssemblyPair.ClrNamespace, clrNamespaceAssemblyPair.AssemblyName);
						this._piNamespaces.Add((string)dictionaryEntry.Key, clrNsUri2);
					}
				}
				this._clrNamespaces = new HashSet<string>();
			}

			// Token: 0x06008384 RID: 33668 RVA: 0x002454C4 File Offset: 0x002436C4
			public override IEnumerable<string> GetAllXamlNamespaces()
			{
				IEnumerable<string> enumerable = this._allXamlNamespaces;
				if (enumerable == null)
				{
					object obj = this.syncObject;
					lock (obj)
					{
						if (this._nsDefinitions != null || this._piNamespaces != null)
						{
							List<string> list = new List<string>(this._sharedSchemaContext.GetAllXamlNamespaces());
							this.AddKnownNamespaces(list);
							enumerable = list.AsReadOnly();
						}
						else
						{
							enumerable = this._sharedSchemaContext.GetAllXamlNamespaces();
						}
						this._allXamlNamespaces = enumerable;
					}
				}
				return enumerable;
			}

			// Token: 0x06008385 RID: 33669 RVA: 0x00245550 File Offset: 0x00243750
			public override XamlType GetXamlType(Type type)
			{
				if (ReflectionHelper.IsPublicType(type))
				{
					return this._sharedSchemaContext.GetXamlType(type);
				}
				return this.GetInternalType(type, null);
			}

			// Token: 0x06008386 RID: 33670 RVA: 0x00245570 File Offset: 0x00243770
			public override bool TryGetCompatibleXamlNamespace(string xamlNamespace, out string compatibleNamespace)
			{
				if (this._sharedSchemaContext.TryGetCompatibleXamlNamespace(xamlNamespace, out compatibleNamespace))
				{
					return true;
				}
				if ((this._nsDefinitions != null && this._nsDefinitions.ContainsKey(xamlNamespace)) || (this._piNamespaces != null && this.SyncContainsKey<string, string>(this._piNamespaces, xamlNamespace)))
				{
					compatibleNamespace = xamlNamespace;
					return true;
				}
				return false;
			}

			// Token: 0x06008387 RID: 33671 RVA: 0x002455C4 File Offset: 0x002437C4
			internal Hashtable GetNamespaceMapHashList()
			{
				Hashtable hashtable = new Hashtable();
				if (this._typeMapper._namespaceMaps != null)
				{
					foreach (NamespaceMapEntry namespaceMapEntry in this._typeMapper._namespaceMaps)
					{
						NamespaceMapEntry value = new NamespaceMapEntry
						{
							XmlNamespace = namespaceMapEntry.XmlNamespace,
							ClrNamespace = namespaceMapEntry.ClrNamespace,
							AssemblyName = namespaceMapEntry.AssemblyName,
							AssemblyPath = namespaceMapEntry.AssemblyPath
						};
						XamlTypeMapper.XamlTypeMapperSchemaContext.AddToMultiHashtable<string, NamespaceMapEntry>(hashtable, namespaceMapEntry.XmlNamespace, value);
					}
				}
				foreach (object obj in this._typeMapper.PITable)
				{
					DictionaryEntry dictionaryEntry = (DictionaryEntry)obj;
					ClrNamespaceAssemblyPair clrNamespaceAssemblyPair = (ClrNamespaceAssemblyPair)dictionaryEntry.Value;
					NamespaceMapEntry namespaceMapEntry2 = new NamespaceMapEntry
					{
						XmlNamespace = (string)dictionaryEntry.Key,
						ClrNamespace = clrNamespaceAssemblyPair.ClrNamespace,
						AssemblyName = clrNamespaceAssemblyPair.AssemblyName,
						AssemblyPath = this._typeMapper.AssemblyPathFor(clrNamespaceAssemblyPair.AssemblyName)
					};
					XamlTypeMapper.XamlTypeMapperSchemaContext.AddToMultiHashtable<string, NamespaceMapEntry>(hashtable, namespaceMapEntry2.XmlNamespace, namespaceMapEntry2);
				}
				object obj2 = this.syncObject;
				lock (obj2)
				{
					foreach (string xmlNamespace in this._clrNamespaces)
					{
						string clrNamespace;
						string text;
						XamlTypeMapper.XamlTypeMapperSchemaContext.SplitClrNsUri(xmlNamespace, out clrNamespace, out text);
						if (!string.IsNullOrEmpty(text))
						{
							string text2 = this._typeMapper.AssemblyPathFor(text);
							if (!string.IsNullOrEmpty(text2))
							{
								NamespaceMapEntry namespaceMapEntry3 = new NamespaceMapEntry
								{
									XmlNamespace = xmlNamespace,
									ClrNamespace = clrNamespace,
									AssemblyName = text,
									AssemblyPath = text2
								};
								XamlTypeMapper.XamlTypeMapperSchemaContext.AddToMultiHashtable<string, NamespaceMapEntry>(hashtable, namespaceMapEntry3.XmlNamespace, namespaceMapEntry3);
							}
						}
					}
				}
				object[] array = new object[hashtable.Count];
				hashtable.Keys.CopyTo(array, 0);
				foreach (object key in array)
				{
					List<NamespaceMapEntry> list = (List<NamespaceMapEntry>)hashtable[key];
					hashtable[key] = list.ToArray();
				}
				return hashtable;
			}

			// Token: 0x06008388 RID: 33672 RVA: 0x00245828 File Offset: 0x00243A28
			internal void SetMappingProcessingInstruction(string xamlNamespace, ClrNamespaceAssemblyPair pair)
			{
				string clrNsUri = XamlTypeMapper.XamlTypeMapperSchemaContext.GetClrNsUri(pair.ClrNamespace, pair.AssemblyName);
				object obj = this.syncObject;
				lock (obj)
				{
					if (this._piNamespaces == null)
					{
						this._piNamespaces = new Dictionary<string, string>();
					}
					this._piNamespaces[xamlNamespace] = clrNsUri;
					this._allXamlNamespaces = null;
				}
			}

			// Token: 0x06008389 RID: 33673 RVA: 0x002458A0 File Offset: 0x00243AA0
			protected override XamlType GetXamlType(string xamlNamespace, string name, params XamlType[] typeArguments)
			{
				XamlType result;
				try
				{
					result = this.LookupXamlType(xamlNamespace, name, typeArguments);
				}
				catch (Exception ex)
				{
					if (CriticalExceptions.IsCriticalException(ex))
					{
						throw;
					}
					if (!this._typeMapper.LoadReferenceAssemblies())
					{
						throw;
					}
					result = this.LookupXamlType(xamlNamespace, name, typeArguments);
				}
				return result;
			}

			// Token: 0x0600838A RID: 33674 RVA: 0x002458F0 File Offset: 0x00243AF0
			protected override Assembly OnAssemblyResolve(string assemblyName)
			{
				string text = this._typeMapper.AssemblyPathFor(assemblyName);
				if (!string.IsNullOrEmpty(text))
				{
					return ReflectionHelper.LoadAssembly(assemblyName, text);
				}
				return base.OnAssemblyResolve(assemblyName);
			}

			// Token: 0x0600838B RID: 33675 RVA: 0x00245921 File Offset: 0x00243B21
			private static string GetClrNsUri(string clrNamespace, string assembly)
			{
				return "clr-namespace:" + clrNamespace + ";assembly=" + assembly;
			}

			// Token: 0x0600838C RID: 33676 RVA: 0x00245934 File Offset: 0x00243B34
			private static void SplitClrNsUri(string xmlNamespace, out string clrNamespace, out string assembly)
			{
				clrNamespace = null;
				assembly = null;
				int num = xmlNamespace.IndexOf("clr-namespace:", StringComparison.Ordinal);
				if (num < 0)
				{
					return;
				}
				num += "clr-namespace:".Length;
				if (num <= xmlNamespace.Length)
				{
					return;
				}
				int num2 = xmlNamespace.IndexOf(";assembly=", StringComparison.Ordinal);
				if (num2 < num)
				{
					clrNamespace = xmlNamespace.Substring(num);
					return;
				}
				clrNamespace = xmlNamespace.Substring(num, num2 - num);
				num2 += ";assembly=".Length;
				if (num2 <= xmlNamespace.Length)
				{
					return;
				}
				assembly = xmlNamespace.Substring(num2);
			}

			// Token: 0x0600838D RID: 33677 RVA: 0x002459B8 File Offset: 0x00243BB8
			private void AddKnownNamespaces(List<string> nsList)
			{
				if (this._nsDefinitions != null)
				{
					foreach (string item in this._nsDefinitions.Keys)
					{
						if (!nsList.Contains(item))
						{
							nsList.Add(item);
						}
					}
				}
				if (this._piNamespaces != null)
				{
					foreach (string item2 in this._piNamespaces.Keys)
					{
						if (!nsList.Contains(item2))
						{
							nsList.Add(item2);
						}
					}
				}
			}

			// Token: 0x0600838E RID: 33678 RVA: 0x00245A7C File Offset: 0x00243C7C
			private XamlType GetInternalType(Type type, XamlType sharedSchemaXamlType)
			{
				object obj = this.syncObject;
				XamlType result;
				lock (obj)
				{
					if (this._allowedInternalTypes == null)
					{
						this._allowedInternalTypes = new Dictionary<Type, XamlType>();
					}
					XamlType xamlType;
					if (!this._allowedInternalTypes.TryGetValue(type, out xamlType))
					{
						WpfSharedXamlSchemaContext.RequireRuntimeType(type);
						if (this._typeMapper.IsInternalTypeAllowedInFullTrust(type))
						{
							xamlType = new XamlTypeMapper.VisibilityMaskingXamlType(type, this._sharedSchemaContext);
						}
						else
						{
							xamlType = (sharedSchemaXamlType ?? this._sharedSchemaContext.GetXamlType(type));
						}
						this._allowedInternalTypes.Add(type, xamlType);
					}
					result = xamlType;
				}
				return result;
			}

			// Token: 0x0600838F RID: 33679 RVA: 0x00245B20 File Offset: 0x00243D20
			private XamlType LookupXamlType(string xamlNamespace, string name, XamlType[] typeArguments)
			{
				FrugalObjectList<string> frugalObjectList;
				XamlType xamlType;
				if (this._nsDefinitions != null && this._nsDefinitions.TryGetValue(xamlNamespace, out frugalObjectList))
				{
					for (int i = 0; i < frugalObjectList.Count; i++)
					{
						xamlType = base.GetXamlType(frugalObjectList[i], name, typeArguments);
						if (xamlType != null)
						{
							return xamlType;
						}
					}
				}
				string xamlNamespace2;
				if (this._piNamespaces != null && this.SyncTryGetValue(this._piNamespaces, xamlNamespace, out xamlNamespace2))
				{
					return base.GetXamlType(xamlNamespace2, name, typeArguments);
				}
				if (xamlNamespace.StartsWith("clr-namespace:", StringComparison.Ordinal))
				{
					object obj = this.syncObject;
					lock (obj)
					{
						if (!this._clrNamespaces.Contains(xamlNamespace))
						{
							this._clrNamespaces.Add(xamlNamespace);
						}
					}
					return base.GetXamlType(xamlNamespace, name, typeArguments);
				}
				xamlType = this._sharedSchemaContext.GetXamlTypeInternal(xamlNamespace, name, typeArguments);
				if (!(xamlType == null) && !xamlType.IsPublic)
				{
					return this.GetInternalType(xamlType.UnderlyingType, xamlType);
				}
				return xamlType;
			}

			// Token: 0x06008390 RID: 33680 RVA: 0x00245C28 File Offset: 0x00243E28
			private bool SyncContainsKey<K, V>(IDictionary<K, V> dict, K key)
			{
				object obj = this.syncObject;
				bool result;
				lock (obj)
				{
					result = dict.ContainsKey(key);
				}
				return result;
			}

			// Token: 0x06008391 RID: 33681 RVA: 0x00245C6C File Offset: 0x00243E6C
			private bool SyncTryGetValue(IDictionary<string, string> dict, string key, out string value)
			{
				object obj = this.syncObject;
				bool result;
				lock (obj)
				{
					result = dict.TryGetValue(key, out value);
				}
				return result;
			}

			// Token: 0x06008392 RID: 33682 RVA: 0x00245CB0 File Offset: 0x00243EB0
			private static void AddToMultiHashtable<K, V>(Hashtable hashtable, K key, V value)
			{
				List<V> list = (List<V>)hashtable[key];
				if (list == null)
				{
					list = new List<V>();
					hashtable.Add(key, list);
				}
				list.Add(value);
			}

			// Token: 0x0400418B RID: 16779
			private Dictionary<string, FrugalObjectList<string>> _nsDefinitions;

			// Token: 0x0400418C RID: 16780
			private XamlTypeMapper _typeMapper;

			// Token: 0x0400418D RID: 16781
			private WpfSharedXamlSchemaContext _sharedSchemaContext;

			// Token: 0x0400418E RID: 16782
			private object syncObject = new object();

			// Token: 0x0400418F RID: 16783
			private Dictionary<string, string> _piNamespaces;

			// Token: 0x04004190 RID: 16784
			private IEnumerable<string> _allXamlNamespaces;

			// Token: 0x04004191 RID: 16785
			private Dictionary<Type, XamlType> _allowedInternalTypes;

			// Token: 0x04004192 RID: 16786
			private HashSet<string> _clrNamespaces;
		}

		// Token: 0x02000894 RID: 2196
		private class VisibilityMaskingXamlType : XamlType
		{
			// Token: 0x06008393 RID: 33683 RVA: 0x00245CEC File Offset: 0x00243EEC
			public VisibilityMaskingXamlType(Type underlyingType, XamlSchemaContext schemaContext) : base(underlyingType, schemaContext)
			{
			}

			// Token: 0x06008394 RID: 33684 RVA: 0x00016748 File Offset: 0x00014948
			protected override bool LookupIsPublic()
			{
				return true;
			}
		}
	}
}
