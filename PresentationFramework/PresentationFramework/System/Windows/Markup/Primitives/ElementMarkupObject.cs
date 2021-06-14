using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Reflection;
using System.Security;
using MS.Internal;

namespace System.Windows.Markup.Primitives
{
	// Token: 0x02000278 RID: 632
	internal class ElementMarkupObject : MarkupObject
	{
		// Token: 0x0600240A RID: 9226 RVA: 0x000AFD02 File Offset: 0x000ADF02
		internal ElementMarkupObject(object instance, XamlDesignerSerializationManager manager)
		{
			this._instance = instance;
			this._context = new ElementMarkupObject.ElementObjectContext(this, null);
			this._manager = manager;
		}

		// Token: 0x170008B4 RID: 2228
		// (get) Token: 0x0600240B RID: 9227 RVA: 0x000AFD25 File Offset: 0x000ADF25
		public override Type ObjectType
		{
			get
			{
				return this._instance.GetType();
			}
		}

		// Token: 0x170008B5 RID: 2229
		// (get) Token: 0x0600240C RID: 9228 RVA: 0x000AFD32 File Offset: 0x000ADF32
		public override object Instance
		{
			get
			{
				return this._instance;
			}
		}

		// Token: 0x0600240D RID: 9229 RVA: 0x000AFD3A File Offset: 0x000ADF3A
		internal override IEnumerable<MarkupProperty> GetProperties(bool mapToConstructorArgs)
		{
			ValueSerializer serializerFor = ValueSerializer.GetSerializerFor(this.ObjectType, this.Context);
			if (serializerFor != null && serializerFor.CanConvertToString(this._instance, this.Context))
			{
				yield return new ElementStringValueProperty(this);
				if (this._key != null)
				{
					yield return this._key;
				}
			}
			else
			{
				Dictionary<string, string> constructorArguments = null;
				IEnumerator enumerator;
				if (mapToConstructorArgs && this._instance is MarkupExtension)
				{
					ParameterInfo[] parameters;
					ICollection collection;
					if (this.TryGetConstructorInfoArguments(this._instance, out parameters, out collection))
					{
						int i = 0;
						foreach (object obj in collection)
						{
							if (constructorArguments == null)
							{
								constructorArguments = new Dictionary<string, string>();
							}
							constructorArguments.Add(parameters[i].Name, parameters[i].Name);
							object value = obj;
							ParameterInfo[] array = parameters;
							int num = i;
							i = num + 1;
							yield return new ElementConstructorArgument(value, array[num].ParameterType, this);
						}
						enumerator = null;
					}
					parameters = null;
				}
				foreach (object obj2 in TypeDescriptor.GetProperties(this._instance))
				{
					PropertyDescriptor propertyDescriptor = (PropertyDescriptor)obj2;
					DesignerSerializationVisibility serializationVisibility = propertyDescriptor.SerializationVisibility;
					if (serializationVisibility != DesignerSerializationVisibility.Hidden && (!propertyDescriptor.IsReadOnly || serializationVisibility == DesignerSerializationVisibility.Content) && ElementMarkupObject.ShouldSerialize(propertyDescriptor, this._instance, this._manager))
					{
						if (serializationVisibility == DesignerSerializationVisibility.Content)
						{
							object value2 = propertyDescriptor.GetValue(this._instance);
							if (value2 == null)
							{
								continue;
							}
							ICollection collection2 = value2 as ICollection;
							if (collection2 != null && collection2.Count < 1)
							{
								continue;
							}
							IEnumerable enumerable = value2 as IEnumerable;
							if (enumerable != null && !enumerable.GetEnumerator().MoveNext())
							{
								continue;
							}
						}
						if (constructorArguments != null)
						{
							ConstructorArgumentAttribute constructorArgumentAttribute = propertyDescriptor.Attributes[typeof(ConstructorArgumentAttribute)] as ConstructorArgumentAttribute;
							if (constructorArgumentAttribute != null && constructorArguments.ContainsKey(constructorArgumentAttribute.ArgumentName))
							{
								continue;
							}
						}
						yield return new ElementProperty(this, propertyDescriptor);
					}
				}
				enumerator = null;
				IDictionary dictionary = this._instance as IDictionary;
				if (dictionary != null)
				{
					yield return new ElementDictionaryItemsPseudoProperty(dictionary, typeof(IDictionary), this);
				}
				else
				{
					IEnumerable enumerable2 = this._instance as IEnumerable;
					if (enumerable2 != null && enumerable2.GetEnumerator().MoveNext())
					{
						yield return new ElementItemsPseudoProperty(enumerable2, typeof(IEnumerable), this);
					}
				}
				if (this._key != null)
				{
					yield return this._key;
				}
				constructorArguments = null;
			}
			yield break;
			yield break;
		}

		// Token: 0x170008B6 RID: 2230
		// (get) Token: 0x0600240E RID: 9230 RVA: 0x000AFD51 File Offset: 0x000ADF51
		public override AttributeCollection Attributes
		{
			get
			{
				return TypeDescriptor.GetAttributes(this.ObjectType);
			}
		}

		// Token: 0x0600240F RID: 9231 RVA: 0x000AFD5E File Offset: 0x000ADF5E
		public override void AssignRootContext(IValueSerializerContext context)
		{
			this._context = new ElementMarkupObject.ElementObjectContext(this, context);
		}

		// Token: 0x170008B7 RID: 2231
		// (get) Token: 0x06002410 RID: 9232 RVA: 0x000AFD6D File Offset: 0x000ADF6D
		internal IValueSerializerContext Context
		{
			get
			{
				return this._context;
			}
		}

		// Token: 0x170008B8 RID: 2232
		// (get) Token: 0x06002411 RID: 9233 RVA: 0x000AFD75 File Offset: 0x000ADF75
		internal XamlDesignerSerializationManager Manager
		{
			get
			{
				return this._manager;
			}
		}

		// Token: 0x06002412 RID: 9234 RVA: 0x000AFD7D File Offset: 0x000ADF7D
		internal void SetKey(ElementKey key)
		{
			this._key = key;
		}

		// Token: 0x06002413 RID: 9235 RVA: 0x000AFD88 File Offset: 0x000ADF88
		private static bool ShouldSerialize(PropertyDescriptor pd, object instance, XamlDesignerSerializationManager manager)
		{
			object obj = instance;
			DependencyPropertyDescriptor dependencyPropertyDescriptor = DependencyPropertyDescriptor.FromProperty(pd);
			MethodInfo methodInfo;
			if (dependencyPropertyDescriptor != null && dependencyPropertyDescriptor.IsAttached)
			{
				Type ownerType = dependencyPropertyDescriptor.DependencyProperty.OwnerType;
				string name = dependencyPropertyDescriptor.DependencyProperty.Name;
				string propertyName = name + "!";
				if (!ElementMarkupObject.TryGetShouldSerializeMethod(new ElementMarkupObject.ShouldSerializeKey(ownerType, propertyName), out methodInfo))
				{
					string name2 = "ShouldSerialize" + name;
					methodInfo = ownerType.GetMethod(name2, BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic, null, ElementMarkupObject._shouldSerializeArgsObject, null);
					if (methodInfo == null)
					{
						methodInfo = ownerType.GetMethod(name2, BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic, null, ElementMarkupObject._shouldSerializeArgsManager, null);
					}
					if (methodInfo == null)
					{
						methodInfo = ownerType.GetMethod(name2, BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic, null, ElementMarkupObject._shouldSerializeArgsMode, null);
					}
					if (methodInfo == null)
					{
						methodInfo = ownerType.GetMethod(name2, BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic, null, ElementMarkupObject._shouldSerializeArgsObjectManager, null);
					}
					if (methodInfo != null && methodInfo.ReturnType != typeof(bool))
					{
						methodInfo = null;
					}
					ElementMarkupObject.CacheShouldSerializeMethod(new ElementMarkupObject.ShouldSerializeKey(ownerType, propertyName), methodInfo);
				}
				obj = null;
			}
			else if (!ElementMarkupObject.TryGetShouldSerializeMethod(new ElementMarkupObject.ShouldSerializeKey(instance.GetType(), pd.Name), out methodInfo))
			{
				Type type = instance.GetType();
				string name3 = "ShouldSerialize" + pd.Name;
				methodInfo = type.GetMethod(name3, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic, null, ElementMarkupObject._shouldSerializeArgsObject, null);
				if (methodInfo == null)
				{
					methodInfo = type.GetMethod(name3, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic, null, ElementMarkupObject._shouldSerializeArgsManager, null);
				}
				if (methodInfo == null)
				{
					methodInfo = type.GetMethod(name3, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic, null, ElementMarkupObject._shouldSerializeArgsMode, null);
				}
				if (methodInfo == null)
				{
					methodInfo = type.GetMethod(name3, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic, null, ElementMarkupObject._shouldSerializeArgsObjectManager, null);
				}
				if (methodInfo != null && methodInfo.ReturnType != typeof(bool))
				{
					methodInfo = null;
				}
				ElementMarkupObject.CacheShouldSerializeMethod(new ElementMarkupObject.ShouldSerializeKey(type, pd.Name), methodInfo);
			}
			if (methodInfo != null)
			{
				ParameterInfo[] parameters = methodInfo.GetParameters();
				if (parameters != null)
				{
					object[] parameters2;
					if (parameters.Length == 1)
					{
						if (parameters[0].ParameterType == typeof(DependencyObject))
						{
							parameters2 = new object[]
							{
								instance as DependencyObject
							};
						}
						else if (parameters[0].ParameterType == typeof(XamlWriterMode))
						{
							parameters2 = new object[]
							{
								manager.XamlWriterMode
							};
						}
						else
						{
							parameters2 = new object[]
							{
								manager
							};
						}
					}
					else
					{
						parameters2 = new object[]
						{
							instance as DependencyObject,
							manager
						};
					}
					return (bool)methodInfo.Invoke(obj, parameters2);
				}
			}
			return pd.ShouldSerializeValue(instance);
		}

		// Token: 0x06002414 RID: 9236 RVA: 0x000B0020 File Offset: 0x000AE220
		private static bool TryGetShouldSerializeMethod(ElementMarkupObject.ShouldSerializeKey key, out MethodInfo methodInfo)
		{
			object obj = ElementMarkupObject._shouldSerializeCache[key];
			if (obj == null || obj == ElementMarkupObject._shouldSerializeCacheLock)
			{
				methodInfo = null;
				return obj != null;
			}
			methodInfo = (obj as MethodInfo);
			return true;
		}

		// Token: 0x06002415 RID: 9237 RVA: 0x000B005C File Offset: 0x000AE25C
		private static void CacheShouldSerializeMethod(ElementMarkupObject.ShouldSerializeKey key, MethodInfo methodInfo)
		{
			object value = (methodInfo == null) ? ElementMarkupObject._shouldSerializeCacheLock : methodInfo;
			object shouldSerializeCacheLock = ElementMarkupObject._shouldSerializeCacheLock;
			lock (shouldSerializeCacheLock)
			{
				ElementMarkupObject._shouldSerializeCache[key] = value;
			}
		}

		// Token: 0x06002416 RID: 9238 RVA: 0x000B00B8 File Offset: 0x000AE2B8
		[SecuritySafeCritical]
		private bool TryGetConstructorInfoArguments(object instance, out ParameterInfo[] parameters, out ICollection arguments)
		{
			SecurityHelper.DemandUnmanagedCode();
			TypeConverter converter = TypeDescriptor.GetConverter(instance);
			if (converter != null && converter.CanConvertTo(this.Context, typeof(InstanceDescriptor)))
			{
				InstanceDescriptor instanceDescriptor;
				try
				{
					instanceDescriptor = (converter.ConvertTo(this._context, TypeConverterHelper.InvariantEnglishUS, instance, typeof(InstanceDescriptor)) as InstanceDescriptor);
				}
				catch (InvalidOperationException)
				{
					instanceDescriptor = null;
				}
				catch (NotSupportedException)
				{
					instanceDescriptor = null;
				}
				if (instanceDescriptor != null)
				{
					ConstructorInfo constructorInfo = instanceDescriptor.MemberInfo as ConstructorInfo;
					if (constructorInfo != null)
					{
						ParameterInfo[] parameters2 = constructorInfo.GetParameters();
						if (parameters2 != null && parameters2.Length == instanceDescriptor.Arguments.Count)
						{
							parameters = parameters2;
							arguments = instanceDescriptor.Arguments;
							return true;
						}
					}
				}
			}
			parameters = null;
			arguments = null;
			return false;
		}

		// Token: 0x04001B15 RID: 6933
		private static object _shouldSerializeCacheLock = new object();

		// Token: 0x04001B16 RID: 6934
		private static Hashtable _shouldSerializeCache = new Hashtable();

		// Token: 0x04001B17 RID: 6935
		private static Type[] _shouldSerializeArgsObject = new Type[]
		{
			typeof(DependencyObject)
		};

		// Token: 0x04001B18 RID: 6936
		private static Type[] _shouldSerializeArgsManager = new Type[]
		{
			typeof(XamlDesignerSerializationManager)
		};

		// Token: 0x04001B19 RID: 6937
		private static Type[] _shouldSerializeArgsMode = new Type[]
		{
			typeof(XamlWriterMode)
		};

		// Token: 0x04001B1A RID: 6938
		private static Type[] _shouldSerializeArgsObjectManager = new Type[]
		{
			typeof(DependencyObject),
			typeof(XamlDesignerSerializationManager)
		};

		// Token: 0x04001B1B RID: 6939
		private static Attribute[] _propertyAttributes = new Attribute[]
		{
			new PropertyFilterAttribute(PropertyFilterOptions.SetValues)
		};

		// Token: 0x04001B1C RID: 6940
		private object _instance;

		// Token: 0x04001B1D RID: 6941
		private IValueSerializerContext _context;

		// Token: 0x04001B1E RID: 6942
		private ElementKey _key;

		// Token: 0x04001B1F RID: 6943
		private XamlDesignerSerializationManager _manager;

		// Token: 0x020008A2 RID: 2210
		private sealed class ElementObjectContext : ValueSerializerContextWrapper, IValueSerializerContext, ITypeDescriptorContext, IServiceProvider
		{
			// Token: 0x060083B8 RID: 33720 RVA: 0x002460BF File Offset: 0x002442BF
			public ElementObjectContext(ElementMarkupObject obj, IValueSerializerContext baseContext) : base(baseContext)
			{
				this._object = obj;
			}

			// Token: 0x17001DDB RID: 7643
			// (get) Token: 0x060083B9 RID: 33721 RVA: 0x002460CF File Offset: 0x002442CF
			object ITypeDescriptorContext.Instance
			{
				get
				{
					return this._object.Instance;
				}
			}

			// Token: 0x040041B9 RID: 16825
			private ElementMarkupObject _object;
		}

		// Token: 0x020008A3 RID: 2211
		private struct ShouldSerializeKey
		{
			// Token: 0x060083BA RID: 33722 RVA: 0x002460DC File Offset: 0x002442DC
			public ShouldSerializeKey(Type type, string propertyName)
			{
				this._type = type;
				this._propertyName = propertyName;
			}

			// Token: 0x060083BB RID: 33723 RVA: 0x002460EC File Offset: 0x002442EC
			public override bool Equals(object obj)
			{
				if (!(obj is ElementMarkupObject.ShouldSerializeKey))
				{
					return false;
				}
				ElementMarkupObject.ShouldSerializeKey shouldSerializeKey = (ElementMarkupObject.ShouldSerializeKey)obj;
				return shouldSerializeKey._type == this._type && shouldSerializeKey._propertyName == this._propertyName;
			}

			// Token: 0x060083BC RID: 33724 RVA: 0x00246130 File Offset: 0x00244330
			public static bool operator ==(ElementMarkupObject.ShouldSerializeKey key1, ElementMarkupObject.ShouldSerializeKey key2)
			{
				return key1.Equals(key2);
			}

			// Token: 0x060083BD RID: 33725 RVA: 0x00246145 File Offset: 0x00244345
			public static bool operator !=(ElementMarkupObject.ShouldSerializeKey key1, ElementMarkupObject.ShouldSerializeKey key2)
			{
				return !key1.Equals(key2);
			}

			// Token: 0x060083BE RID: 33726 RVA: 0x0024615D File Offset: 0x0024435D
			public override int GetHashCode()
			{
				return this._type.GetHashCode() ^ this._propertyName.GetHashCode();
			}

			// Token: 0x040041BA RID: 16826
			private Type _type;

			// Token: 0x040041BB RID: 16827
			private string _propertyName;
		}
	}
}
