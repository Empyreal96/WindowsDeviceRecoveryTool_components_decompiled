using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using System.Text;
using System.Windows.Baml2006;
using System.Windows.Markup;
using System.Windows.Threading;
using MS.Internal;
using MS.Internal.Data;
using MS.Utility;

namespace System.Windows
{
	/// <summary>Implements a data structure for describing a property as a path below another property, or below an owning type. Property paths are used in data binding to objects, and in storyboards and timelines for animations.</summary>
	// Token: 0x020000E2 RID: 226
	[TypeConverter(typeof(PropertyPathConverter))]
	public sealed class PropertyPath
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.PropertyPath" /> class, with the provided pathing token string and parameters.</summary>
		/// <param name="path">A string that specifies the <see cref="P:System.Windows.PropertyPath.Path" />, in a tokenized format.</param>
		/// <param name="pathParameters">An array of objects that sets the <see cref="P:System.Windows.PropertyPath.PathParameters" />.  </param>
		// Token: 0x0600079E RID: 1950 RVA: 0x00017C34 File Offset: 0x00015E34
		public PropertyPath(string path, params object[] pathParameters)
		{
			if (Dispatcher.CurrentDispatcher == null)
			{
				throw new InvalidOperationException();
			}
			this._path = path;
			if (pathParameters != null && pathParameters.Length != 0)
			{
				PropertyPath.PathParameterCollection pathParameterCollection = new PropertyPath.PathParameterCollection(pathParameters);
				this.SetPathParameterCollection(pathParameterCollection);
			}
			this.PrepareSourceValueInfo(null);
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.PropertyPath" /> class.</summary>
		/// <param name="parameter">A property path that either describes a path to a common language runtime (CLR) property, or a single dependency property. </param>
		// Token: 0x0600079F RID: 1951 RVA: 0x00017C8D File Offset: 0x00015E8D
		public PropertyPath(object parameter) : this("(0)", new object[]
		{
			parameter
		})
		{
		}

		// Token: 0x060007A0 RID: 1952 RVA: 0x00017CA4 File Offset: 0x00015EA4
		internal PropertyPath(string path, ITypeDescriptorContext typeDescriptorContext)
		{
			this._path = path;
			this.PrepareSourceValueInfo(typeDescriptorContext);
			this.NormalizePath();
		}

		/// <summary>Gets or sets the string that describes the path. </summary>
		/// <returns>The string that describes the path.</returns>
		// Token: 0x1700018E RID: 398
		// (get) Token: 0x060007A1 RID: 1953 RVA: 0x00017CD6 File Offset: 0x00015ED6
		// (set) Token: 0x060007A2 RID: 1954 RVA: 0x00017CDE File Offset: 0x00015EDE
		public string Path
		{
			get
			{
				return this._path;
			}
			set
			{
				this._path = value;
				this.PrepareSourceValueInfo(null);
			}
		}

		/// <summary>Gets the list of parameters to use when the path refers to indexed parameters.</summary>
		/// <returns>The parameter list.</returns>
		// Token: 0x1700018F RID: 399
		// (get) Token: 0x060007A3 RID: 1955 RVA: 0x00017CEE File Offset: 0x00015EEE
		public Collection<object> PathParameters
		{
			get
			{
				if (this._parameters == null)
				{
					this.SetPathParameterCollection(new PropertyPath.PathParameterCollection());
				}
				return this._parameters;
			}
		}

		// Token: 0x17000190 RID: 400
		// (get) Token: 0x060007A4 RID: 1956 RVA: 0x00017D09 File Offset: 0x00015F09
		internal int Length
		{
			get
			{
				return this._arySVI.Length;
			}
		}

		// Token: 0x17000191 RID: 401
		// (get) Token: 0x060007A5 RID: 1957 RVA: 0x00017D13 File Offset: 0x00015F13
		internal PropertyPathStatus Status
		{
			get
			{
				return this.SingleWorker.Status;
			}
		}

		// Token: 0x17000192 RID: 402
		// (get) Token: 0x060007A6 RID: 1958 RVA: 0x00017D20 File Offset: 0x00015F20
		internal string LastError
		{
			get
			{
				return this._lastError;
			}
		}

		// Token: 0x17000193 RID: 403
		// (get) Token: 0x060007A7 RID: 1959 RVA: 0x00017D28 File Offset: 0x00015F28
		internal object LastItem
		{
			get
			{
				return this.GetItem(this.Length - 1);
			}
		}

		// Token: 0x17000194 RID: 404
		// (get) Token: 0x060007A8 RID: 1960 RVA: 0x00017D38 File Offset: 0x00015F38
		internal object LastAccessor
		{
			get
			{
				return this.GetAccessor(this.Length - 1);
			}
		}

		// Token: 0x17000195 RID: 405
		// (get) Token: 0x060007A9 RID: 1961 RVA: 0x00017D48 File Offset: 0x00015F48
		internal object[] LastIndexerArguments
		{
			get
			{
				return this.GetIndexerArguments(this.Length - 1);
			}
		}

		// Token: 0x17000196 RID: 406
		// (get) Token: 0x060007AA RID: 1962 RVA: 0x00017D58 File Offset: 0x00015F58
		internal bool StartsWithStaticProperty
		{
			get
			{
				return this.Length > 0 && PropertyPath.IsStaticProperty(this._earlyBoundPathParts[0]);
			}
		}

		// Token: 0x060007AB RID: 1963 RVA: 0x00017D74 File Offset: 0x00015F74
		internal static bool IsStaticProperty(object accessor)
		{
			DependencyProperty dependencyProperty;
			PropertyInfo propertyInfo;
			PropertyDescriptor propertyDescriptor;
			DynamicObjectAccessor dynamicObjectAccessor;
			PropertyPath.DowncastAccessor(accessor, out dependencyProperty, out propertyInfo, out propertyDescriptor, out dynamicObjectAccessor);
			if (propertyInfo != null)
			{
				MethodInfo getMethod = propertyInfo.GetGetMethod();
				return getMethod != null && getMethod.IsStatic;
			}
			return false;
		}

		// Token: 0x060007AC RID: 1964 RVA: 0x00017DB4 File Offset: 0x00015FB4
		internal static void DowncastAccessor(object accessor, out DependencyProperty dp, out PropertyInfo pi, out PropertyDescriptor pd, out DynamicObjectAccessor doa)
		{
			DependencyProperty dependencyProperty;
			dp = (dependencyProperty = (accessor as DependencyProperty));
			if (dependencyProperty != null)
			{
				pd = null;
				pi = null;
				doa = null;
				return;
			}
			PropertyInfo left;
			pi = (left = (accessor as PropertyInfo));
			if (left != null)
			{
				pd = null;
				doa = null;
				return;
			}
			PropertyDescriptor propertyDescriptor;
			pd = (propertyDescriptor = (accessor as PropertyDescriptor));
			if (propertyDescriptor != null)
			{
				doa = null;
				return;
			}
			doa = (accessor as DynamicObjectAccessor);
		}

		// Token: 0x060007AD RID: 1965 RVA: 0x00017E0F File Offset: 0x0001600F
		internal IDisposable SetContext(object rootItem)
		{
			return this.SingleWorker.SetContext(rootItem);
		}

		// Token: 0x060007AE RID: 1966 RVA: 0x00017E1D File Offset: 0x0001601D
		internal object GetItem(int k)
		{
			return this.SingleWorker.GetItem(k);
		}

		// Token: 0x060007AF RID: 1967 RVA: 0x00017E2C File Offset: 0x0001602C
		internal object GetAccessor(int k)
		{
			object obj = this._earlyBoundPathParts[k];
			if (obj == null)
			{
				obj = this.SingleWorker.GetAccessor(k);
			}
			return obj;
		}

		// Token: 0x060007B0 RID: 1968 RVA: 0x00017E53 File Offset: 0x00016053
		internal object[] GetIndexerArguments(int k)
		{
			return this.SingleWorker.GetIndexerArguments(k);
		}

		// Token: 0x060007B1 RID: 1969 RVA: 0x00017E61 File Offset: 0x00016061
		internal object GetValue()
		{
			return this.SingleWorker.RawValue();
		}

		// Token: 0x060007B2 RID: 1970 RVA: 0x00017E70 File Offset: 0x00016070
		internal int ComputeUnresolvedAttachedPropertiesInPath()
		{
			int num = 0;
			for (int i = this.Length - 1; i >= 0; i--)
			{
				if (this._earlyBoundPathParts[i] == null)
				{
					string name = this._arySVI[i].name;
					if (PropertyPath.IsPropertyReference(name) && name.IndexOf('.') >= 0)
					{
						num++;
					}
				}
			}
			return num;
		}

		// Token: 0x17000197 RID: 407
		// (get) Token: 0x060007B3 RID: 1971 RVA: 0x00017EC6 File Offset: 0x000160C6
		internal SourceValueInfo[] SVI
		{
			get
			{
				return this._arySVI;
			}
		}

		// Token: 0x060007B4 RID: 1972 RVA: 0x00017ED0 File Offset: 0x000160D0
		internal object ResolvePropertyName(int level, object item, Type ownerType, object context)
		{
			object obj = this._earlyBoundPathParts[level];
			if (obj == null)
			{
				obj = this.ResolvePropertyName(this._arySVI[level].name, item, ownerType, context, false);
			}
			return obj;
		}

		// Token: 0x060007B5 RID: 1973 RVA: 0x00017F08 File Offset: 0x00016108
		internal IndexerParameterInfo[] ResolveIndexerParams(int level, object context)
		{
			IndexerParameterInfo[] array = this._earlyBoundPathParts[level] as IndexerParameterInfo[];
			if (array == null)
			{
				array = this.ResolveIndexerParams(this._arySVI[level].paramList, context, false);
			}
			return array;
		}

		// Token: 0x060007B6 RID: 1974 RVA: 0x00017F44 File Offset: 0x00016144
		internal void ReplaceIndexerByProperty(int level, string name)
		{
			this._arySVI[level].name = name;
			this._arySVI[level].propertyName = name;
			this._arySVI[level].type = SourceValueType.Property;
			this._earlyBoundPathParts[level] = null;
		}

		// Token: 0x17000198 RID: 408
		// (get) Token: 0x060007B7 RID: 1975 RVA: 0x00017F90 File Offset: 0x00016190
		private PropertyPathWorker SingleWorker
		{
			get
			{
				if (this._singleWorker == null)
				{
					this._singleWorker = new PropertyPathWorker(this);
				}
				return this._singleWorker;
			}
		}

		// Token: 0x060007B8 RID: 1976 RVA: 0x00017FAC File Offset: 0x000161AC
		private void PrepareSourceValueInfo(ITypeDescriptorContext typeDescriptorContext)
		{
			PathParser pathParser = DataBindEngine.CurrentDataBindEngine.PathParser;
			this._arySVI = pathParser.Parse(this.Path);
			if (this._arySVI.Length == 0)
			{
				string text = pathParser.Error;
				if (text == null)
				{
					text = this.Path;
				}
				throw new InvalidOperationException(SR.Get("PropertyPathSyntaxError", new object[]
				{
					text
				}));
			}
			this.ResolvePathParts(typeDescriptorContext);
		}

		// Token: 0x060007B9 RID: 1977 RVA: 0x00018014 File Offset: 0x00016214
		private void NormalizePath()
		{
			StringBuilder stringBuilder = new StringBuilder();
			PropertyPath.PathParameterCollection pathParameterCollection = new PropertyPath.PathParameterCollection();
			for (int i = 0; i < this._arySVI.Length; i++)
			{
				switch (this._arySVI[i].drillIn)
				{
				case DrillIn.Never:
					if (this._arySVI[i].type == SourceValueType.Property)
					{
						stringBuilder.Append('.');
					}
					break;
				case DrillIn.Always:
					stringBuilder.Append('/');
					break;
				}
				switch (this._arySVI[i].type)
				{
				case SourceValueType.Property:
					if (this._earlyBoundPathParts[i] != null)
					{
						stringBuilder.Append('(');
						stringBuilder.Append(pathParameterCollection.Count.ToString(TypeConverterHelper.InvariantEnglishUS.NumberFormat));
						stringBuilder.Append(')');
						pathParameterCollection.Add(this._earlyBoundPathParts[i]);
					}
					else
					{
						stringBuilder.Append(this._arySVI[i].name);
					}
					break;
				case SourceValueType.Indexer:
					stringBuilder.Append('[');
					if (this._earlyBoundPathParts[i] != null)
					{
						IndexerParameterInfo[] array = (IndexerParameterInfo[])this._earlyBoundPathParts[i];
						int num = 0;
						for (;;)
						{
							IndexerParameterInfo indexerParameterInfo = array[num];
							if (indexerParameterInfo.type != null)
							{
								stringBuilder.Append('(');
								stringBuilder.Append(pathParameterCollection.Count.ToString(TypeConverterHelper.InvariantEnglishUS.NumberFormat));
								stringBuilder.Append(')');
								pathParameterCollection.Add(indexerParameterInfo.value);
							}
							else
							{
								stringBuilder.Append(indexerParameterInfo.value);
							}
							num++;
							if (num >= array.Length)
							{
								break;
							}
							stringBuilder.Append(',');
						}
					}
					else
					{
						stringBuilder.Append(this._arySVI[i].name);
					}
					stringBuilder.Append(']');
					break;
				}
			}
			if (pathParameterCollection.Count > 0)
			{
				this._path = stringBuilder.ToString();
				this.SetPathParameterCollection(pathParameterCollection);
			}
		}

		// Token: 0x060007BA RID: 1978 RVA: 0x00018214 File Offset: 0x00016414
		private void SetPathParameterCollection(PropertyPath.PathParameterCollection parameters)
		{
			if (this._parameters != null)
			{
				this._parameters.CollectionChanged -= this.ParameterCollectionChanged;
			}
			this._parameters = parameters;
			if (this._parameters != null)
			{
				this._parameters.CollectionChanged += this.ParameterCollectionChanged;
			}
		}

		// Token: 0x060007BB RID: 1979 RVA: 0x00018266 File Offset: 0x00016466
		private void ParameterCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			this.PrepareSourceValueInfo(null);
		}

		// Token: 0x060007BC RID: 1980 RVA: 0x00018270 File Offset: 0x00016470
		private void ResolvePathParts(ITypeDescriptorContext typeDescriptorContext)
		{
			bool throwOnError = typeDescriptorContext != null;
			object obj = null;
			TypeConvertContext typeConvertContext = typeDescriptorContext as TypeConvertContext;
			if (typeConvertContext != null)
			{
				obj = typeConvertContext.ParserContext;
			}
			if (obj == null)
			{
				obj = typeDescriptorContext;
			}
			this._earlyBoundPathParts = new object[this.Length];
			for (int i = this.Length - 1; i >= 0; i--)
			{
				if (this._arySVI[i].type == SourceValueType.Property)
				{
					string name = this._arySVI[i].name;
					if (PropertyPath.IsPropertyReference(name))
					{
						object obj2 = this.ResolvePropertyName(name, null, null, obj, throwOnError);
						this._earlyBoundPathParts[i] = obj2;
						if (obj2 != null)
						{
							this._arySVI[i].propertyName = PropertyPath.GetPropertyName(obj2);
						}
					}
					else
					{
						this._arySVI[i].propertyName = name;
					}
				}
				else if (this._arySVI[i].type == SourceValueType.Indexer)
				{
					IndexerParameterInfo[] array = this.ResolveIndexerParams(this._arySVI[i].paramList, obj, throwOnError);
					this._earlyBoundPathParts[i] = array;
					this._arySVI[i].propertyName = "Item[]";
				}
			}
		}

		// Token: 0x060007BD RID: 1981 RVA: 0x00018390 File Offset: 0x00016590
		private object ResolvePropertyName(string name, object item, Type ownerType, object context, bool throwOnError)
		{
			string text = name;
			int num;
			if (PropertyPath.IsParameterIndex(name, out num))
			{
				if (0 <= num && num < this.PathParameters.Count)
				{
					object obj = this.PathParameters[num];
					if (!PropertyPath.IsValidAccessor(obj))
					{
						throw new InvalidOperationException(SR.Get("PropertyPathInvalidAccessor", new object[]
						{
							(obj != null) ? obj.GetType().FullName : "null"
						}));
					}
					return obj;
				}
				else
				{
					if (throwOnError)
					{
						throw new InvalidOperationException(SR.Get("PathParametersIndexOutOfRange", new object[]
						{
							num,
							this.PathParameters.Count
						}));
					}
					return null;
				}
			}
			else
			{
				if (PropertyPath.IsPropertyReference(name))
				{
					name = name.Substring(1, name.Length - 2);
					int num2 = name.LastIndexOf('.');
					if (num2 >= 0)
					{
						text = name.Substring(num2 + 1).Trim();
						string text2 = name.Substring(0, num2).Trim();
						ownerType = this.GetTypeFromName(text2, context);
						if (ownerType == null && throwOnError)
						{
							throw new InvalidOperationException(SR.Get("PropertyPathNoOwnerType", new object[]
							{
								text2
							}));
						}
					}
					else
					{
						text = name;
					}
				}
				if (!(ownerType != null))
				{
					return null;
				}
				object obj2 = DependencyProperty.FromName(text, ownerType);
				if (obj2 == null && item is ICustomTypeDescriptor)
				{
					obj2 = TypeDescriptor.GetProperties(item)[text];
				}
				if (obj2 == null && (item is INotifyPropertyChanged || item is DependencyObject))
				{
					obj2 = this.GetPropertyHelper(ownerType, text);
				}
				if (obj2 == null && item != null)
				{
					obj2 = TypeDescriptor.GetProperties(item)[text];
				}
				if (obj2 == null)
				{
					obj2 = this.GetPropertyHelper(ownerType, text);
				}
				if (obj2 == null && SystemCoreHelper.IsIDynamicMetaObjectProvider(item))
				{
					obj2 = SystemCoreHelper.NewDynamicPropertyAccessor(item.GetType(), text);
				}
				if (obj2 == null && throwOnError)
				{
					throw new InvalidOperationException(SR.Get("PropertyPathNoProperty", new object[]
					{
						ownerType.Name,
						text
					}));
				}
				return obj2;
			}
		}

		// Token: 0x060007BE RID: 1982 RVA: 0x00018570 File Offset: 0x00016770
		private PropertyInfo GetPropertyHelper(Type ownerType, string propertyName)
		{
			PropertyInfo propertyInfo = null;
			bool flag = false;
			bool flag2 = false;
			try
			{
				propertyInfo = ownerType.GetProperty(propertyName, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.FlattenHierarchy);
			}
			catch (AmbiguousMatchException)
			{
				flag = true;
			}
			if (flag)
			{
				try
				{
					propertyInfo = null;
					while (propertyInfo == null && ownerType != null)
					{
						propertyInfo = ownerType.GetProperty(propertyName, BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public);
						ownerType = ownerType.BaseType;
					}
				}
				catch (AmbiguousMatchException)
				{
					flag2 = true;
				}
			}
			if (PropertyPathWorker.IsIndexedProperty(propertyInfo))
			{
				flag2 = true;
			}
			if (flag2)
			{
				propertyInfo = IndexerPropertyInfo.Instance;
			}
			return propertyInfo;
		}

		// Token: 0x060007BF RID: 1983 RVA: 0x000185F8 File Offset: 0x000167F8
		private IndexerParameterInfo[] ResolveIndexerParams(FrugalObjectList<IndexerParamInfo> paramList, object context, bool throwOnError)
		{
			IndexerParameterInfo[] array = new IndexerParameterInfo[paramList.Count];
			for (int i = 0; i < array.Length; i++)
			{
				if (string.IsNullOrEmpty(paramList[i].parenString))
				{
					array[i].value = paramList[i].valueString;
				}
				else if (string.IsNullOrEmpty(paramList[i].valueString))
				{
					int num;
					if (int.TryParse(paramList[i].parenString.Trim(), NumberStyles.Integer, TypeConverterHelper.InvariantEnglishUS.NumberFormat, out num))
					{
						if (0 <= num && num < this.PathParameters.Count)
						{
							object obj = this.PathParameters[num];
							if (obj != null)
							{
								array[i].value = obj;
								array[i].type = obj.GetType();
							}
							else if (throwOnError)
							{
								throw new InvalidOperationException(SR.Get("PathParameterIsNull", new object[]
								{
									num
								}));
							}
						}
						else if (throwOnError)
						{
							throw new InvalidOperationException(SR.Get("PathParametersIndexOutOfRange", new object[]
							{
								num,
								this.PathParameters.Count
							}));
						}
					}
					else
					{
						array[i].value = "(" + paramList[i].parenString + ")";
					}
				}
				else
				{
					array[i].type = this.GetTypeFromName(paramList[i].parenString, context);
					if (array[i].type != null)
					{
						object typedParamValue = this.GetTypedParamValue(paramList[i].valueString.Trim(), array[i].type, throwOnError);
						if (typedParamValue != null)
						{
							array[i].value = typedParamValue;
						}
						else
						{
							if (throwOnError)
							{
								throw new InvalidOperationException(SR.Get("PropertyPathIndexWrongType", new object[]
								{
									paramList[i].parenString,
									paramList[i].valueString
								}));
							}
							array[i].type = null;
						}
					}
					else
					{
						array[i].value = "(" + paramList[i].parenString + ")" + paramList[i].valueString;
					}
				}
			}
			return array;
		}

		// Token: 0x060007C0 RID: 1984 RVA: 0x0001884C File Offset: 0x00016A4C
		private object GetTypedParamValue(string param, Type type, bool throwOnError)
		{
			object obj = null;
			if (type == typeof(string))
			{
				return param;
			}
			TypeConverter converter = TypeDescriptor.GetConverter(type);
			if (converter != null && converter.CanConvertFrom(typeof(string)))
			{
				try
				{
					obj = converter.ConvertFromString(null, CultureInfo.InvariantCulture, param);
				}
				catch (Exception ex)
				{
					if (CriticalExceptions.IsCriticalApplicationException(ex) || throwOnError)
					{
						throw;
					}
				}
				catch
				{
					if (throwOnError)
					{
						throw;
					}
				}
			}
			if (obj == null && type.IsAssignableFrom(typeof(string)))
			{
				obj = param;
			}
			return obj;
		}

		// Token: 0x060007C1 RID: 1985 RVA: 0x000188E8 File Offset: 0x00016AE8
		private Type GetTypeFromName(string name, object context)
		{
			ParserContext parserContext = context as ParserContext;
			if (parserContext == null)
			{
				if (context is IServiceProvider)
				{
					IXamlTypeResolver xamlTypeResolver = (context as IServiceProvider).GetService(typeof(IXamlTypeResolver)) as IXamlTypeResolver;
					if (xamlTypeResolver != null)
					{
						return xamlTypeResolver.Resolve(name);
					}
				}
				IValueSerializerContext valueSerializerContext = context as IValueSerializerContext;
				if (valueSerializerContext != null)
				{
					ValueSerializer serializerFor = ValueSerializer.GetSerializerFor(typeof(Type), valueSerializerContext);
					if (serializerFor != null)
					{
						return serializerFor.ConvertFromString(name, valueSerializerContext) as Type;
					}
				}
				DependencyObject dependencyObject = context as DependencyObject;
				if (dependencyObject == null)
				{
					if (FrameworkCompatibilityPreferences.TargetsDesktop_V4_0)
					{
						return null;
					}
					dependencyObject = new DependencyObject();
				}
				WpfSharedBamlSchemaContext bamlSharedSchemaContext = XamlReader.BamlSharedSchemaContext;
				return bamlSharedSchemaContext.ResolvePrefixedNameWithAdditionalWpfSemantics(name, dependencyObject);
			}
			int num = name.IndexOf(':');
			string text;
			if (num == -1)
			{
				text = string.Empty;
			}
			else
			{
				text = name.Substring(0, num).TrimEnd(new char[0]);
				name = name.Substring(num + 1).TrimStart(new char[0]);
			}
			string text2 = parserContext.XmlnsDictionary[text];
			if (text2 == null)
			{
				throw new ArgumentException(SR.Get("ParserPrefixNSProperty", new object[]
				{
					text,
					name
				}));
			}
			TypeAndSerializer typeOnly = parserContext.XamlTypeMapper.GetTypeOnly(text2, name);
			if (typeOnly == null)
			{
				return null;
			}
			return typeOnly.ObjectType;
		}

		// Token: 0x060007C2 RID: 1986 RVA: 0x00018A24 File Offset: 0x00016C24
		internal static bool IsPropertyReference(string name)
		{
			return name != null && name.Length > 1 && name[0] == '(' && name[name.Length - 1] == ')';
		}

		// Token: 0x060007C3 RID: 1987 RVA: 0x00018A51 File Offset: 0x00016C51
		internal static bool IsParameterIndex(string name, out int index)
		{
			if (PropertyPath.IsPropertyReference(name))
			{
				name = name.Substring(1, name.Length - 2);
				return int.TryParse(name, NumberStyles.Integer, TypeConverterHelper.InvariantEnglishUS.NumberFormat, out index);
			}
			index = -1;
			return false;
		}

		// Token: 0x060007C4 RID: 1988 RVA: 0x00018A85 File Offset: 0x00016C85
		private static bool IsValidAccessor(object accessor)
		{
			return accessor is DependencyProperty || accessor is PropertyInfo || accessor is PropertyDescriptor || accessor is DynamicObjectAccessor;
		}

		// Token: 0x060007C5 RID: 1989 RVA: 0x00018AAC File Offset: 0x00016CAC
		private static string GetPropertyName(object accessor)
		{
			DependencyProperty dependencyProperty;
			if ((dependencyProperty = (accessor as DependencyProperty)) != null)
			{
				return dependencyProperty.Name;
			}
			PropertyInfo propertyInfo;
			if ((propertyInfo = (accessor as PropertyInfo)) != null)
			{
				return propertyInfo.Name;
			}
			PropertyDescriptor propertyDescriptor;
			if ((propertyDescriptor = (accessor as PropertyDescriptor)) != null)
			{
				return propertyDescriptor.Name;
			}
			DynamicObjectAccessor dynamicObjectAccessor;
			if ((dynamicObjectAccessor = (accessor as DynamicObjectAccessor)) != null)
			{
				return dynamicObjectAccessor.PropertyName;
			}
			Invariant.Assert(false, "Unknown accessor type");
			return null;
		}

		// Token: 0x0400076C RID: 1900
		private const string SingleStepPath = "(0)";

		// Token: 0x0400076D RID: 1901
		private static readonly char[] s_comma = new char[]
		{
			','
		};

		// Token: 0x0400076E RID: 1902
		private string _path = string.Empty;

		// Token: 0x0400076F RID: 1903
		private PropertyPath.PathParameterCollection _parameters;

		// Token: 0x04000770 RID: 1904
		private SourceValueInfo[] _arySVI;

		// Token: 0x04000771 RID: 1905
		private string _lastError = string.Empty;

		// Token: 0x04000772 RID: 1906
		private object[] _earlyBoundPathParts;

		// Token: 0x04000773 RID: 1907
		private PropertyPathWorker _singleWorker;

		// Token: 0x0200081F RID: 2079
		private class PathParameterCollection : ObservableCollection<object>
		{
			// Token: 0x06007E56 RID: 32342 RVA: 0x002359AB File Offset: 0x00233BAB
			public PathParameterCollection()
			{
			}

			// Token: 0x06007E57 RID: 32343 RVA: 0x002359B4 File Offset: 0x00233BB4
			public PathParameterCollection(object[] parameters)
			{
				IList<object> items = base.Items;
				foreach (object item in parameters)
				{
					items.Add(item);
				}
			}
		}
	}
}
