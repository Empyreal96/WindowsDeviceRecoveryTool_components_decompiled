using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Dynamic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Linq
{
	// Token: 0x02000066 RID: 102
	public class JObject : JContainer, IDictionary<string, JToken>, ICollection<KeyValuePair<string, JToken>>, IEnumerable<KeyValuePair<string, JToken>>, IEnumerable, INotifyPropertyChanged, ICustomTypeDescriptor, INotifyPropertyChanging
	{
		// Token: 0x17000131 RID: 305
		// (get) Token: 0x060005A3 RID: 1443 RVA: 0x00015891 File Offset: 0x00013A91
		protected override IList<JToken> ChildrenTokens
		{
			get
			{
				return this._properties;
			}
		}

		// Token: 0x14000006 RID: 6
		// (add) Token: 0x060005A4 RID: 1444 RVA: 0x0001589C File Offset: 0x00013A9C
		// (remove) Token: 0x060005A5 RID: 1445 RVA: 0x000158D4 File Offset: 0x00013AD4
		public event PropertyChangedEventHandler PropertyChanged;

		// Token: 0x14000007 RID: 7
		// (add) Token: 0x060005A6 RID: 1446 RVA: 0x0001590C File Offset: 0x00013B0C
		// (remove) Token: 0x060005A7 RID: 1447 RVA: 0x00015944 File Offset: 0x00013B44
		public event PropertyChangingEventHandler PropertyChanging;

		// Token: 0x060005A8 RID: 1448 RVA: 0x00015979 File Offset: 0x00013B79
		public JObject()
		{
		}

		// Token: 0x060005A9 RID: 1449 RVA: 0x0001598C File Offset: 0x00013B8C
		public JObject(JObject other) : base(other)
		{
		}

		// Token: 0x060005AA RID: 1450 RVA: 0x000159A0 File Offset: 0x00013BA0
		public JObject(params object[] content) : this(content)
		{
		}

		// Token: 0x060005AB RID: 1451 RVA: 0x000159A9 File Offset: 0x00013BA9
		public JObject(object content)
		{
			this.Add(content);
		}

		// Token: 0x060005AC RID: 1452 RVA: 0x000159C4 File Offset: 0x00013BC4
		internal override bool DeepEquals(JToken node)
		{
			JObject jobject = node as JObject;
			return jobject != null && this._properties.Compare(jobject._properties);
		}

		// Token: 0x060005AD RID: 1453 RVA: 0x000159EE File Offset: 0x00013BEE
		internal override void InsertItem(int index, JToken item, bool skipParentCheck)
		{
			if (item != null && item.Type == JTokenType.Comment)
			{
				return;
			}
			base.InsertItem(index, item, skipParentCheck);
		}

		// Token: 0x060005AE RID: 1454 RVA: 0x00015A08 File Offset: 0x00013C08
		internal override void ValidateToken(JToken o, JToken existing)
		{
			ValidationUtils.ArgumentNotNull(o, "o");
			if (o.Type != JTokenType.Property)
			{
				throw new ArgumentException("Can not add {0} to {1}.".FormatWith(CultureInfo.InvariantCulture, o.GetType(), base.GetType()));
			}
			JProperty jproperty = (JProperty)o;
			if (existing != null)
			{
				JProperty jproperty2 = (JProperty)existing;
				if (jproperty.Name == jproperty2.Name)
				{
					return;
				}
			}
			if (this._properties.TryGetValue(jproperty.Name, out existing))
			{
				throw new ArgumentException("Can not add property {0} to {1}. Property with the same name already exists on object.".FormatWith(CultureInfo.InvariantCulture, jproperty.Name, base.GetType()));
			}
		}

		// Token: 0x060005AF RID: 1455 RVA: 0x00015AA8 File Offset: 0x00013CA8
		internal override void MergeItem(object content, JsonMergeSettings settings)
		{
			JObject jobject = content as JObject;
			if (jobject == null)
			{
				return;
			}
			foreach (KeyValuePair<string, JToken> keyValuePair in jobject)
			{
				JProperty jproperty = this.Property(keyValuePair.Key);
				if (jproperty == null)
				{
					this.Add(keyValuePair.Key, keyValuePair.Value);
				}
				else if (keyValuePair.Value != null)
				{
					JContainer jcontainer = jproperty.Value as JContainer;
					if (jcontainer == null)
					{
						if (keyValuePair.Value.Type != JTokenType.Null)
						{
							jproperty.Value = keyValuePair.Value;
						}
					}
					else if (jcontainer.Type != keyValuePair.Value.Type)
					{
						jproperty.Value = keyValuePair.Value;
					}
					else
					{
						jcontainer.Merge(keyValuePair.Value, settings);
					}
				}
			}
		}

		// Token: 0x060005B0 RID: 1456 RVA: 0x00015B90 File Offset: 0x00013D90
		internal void InternalPropertyChanged(JProperty childProperty)
		{
			this.OnPropertyChanged(childProperty.Name);
			if (this._listChanged != null)
			{
				this.OnListChanged(new ListChangedEventArgs(ListChangedType.ItemChanged, base.IndexOfItem(childProperty)));
			}
			if (this._collectionChanged != null)
			{
				this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, childProperty, childProperty, base.IndexOfItem(childProperty)));
			}
		}

		// Token: 0x060005B1 RID: 1457 RVA: 0x00015BE1 File Offset: 0x00013DE1
		internal void InternalPropertyChanging(JProperty childProperty)
		{
			this.OnPropertyChanging(childProperty.Name);
		}

		// Token: 0x060005B2 RID: 1458 RVA: 0x00015BEF File Offset: 0x00013DEF
		internal override JToken CloneToken()
		{
			return new JObject(this);
		}

		// Token: 0x17000132 RID: 306
		// (get) Token: 0x060005B3 RID: 1459 RVA: 0x00015BF7 File Offset: 0x00013DF7
		public override JTokenType Type
		{
			get
			{
				return JTokenType.Object;
			}
		}

		// Token: 0x060005B4 RID: 1460 RVA: 0x00015BFA File Offset: 0x00013DFA
		public IEnumerable<JProperty> Properties()
		{
			return this._properties.Cast<JProperty>();
		}

		// Token: 0x060005B5 RID: 1461 RVA: 0x00015C08 File Offset: 0x00013E08
		public JProperty Property(string name)
		{
			if (name == null)
			{
				return null;
			}
			JToken jtoken;
			this._properties.TryGetValue(name, out jtoken);
			return (JProperty)jtoken;
		}

		// Token: 0x060005B6 RID: 1462 RVA: 0x00015C37 File Offset: 0x00013E37
		public JEnumerable<JToken> PropertyValues()
		{
			return new JEnumerable<JToken>(from p in this.Properties()
			select p.Value);
		}

		// Token: 0x17000133 RID: 307
		public override JToken this[object key]
		{
			get
			{
				ValidationUtils.ArgumentNotNull(key, "o");
				string text = key as string;
				if (text == null)
				{
					throw new ArgumentException("Accessed JObject values with invalid key value: {0}. Object property name expected.".FormatWith(CultureInfo.InvariantCulture, MiscellaneousUtils.ToString(key)));
				}
				return this[text];
			}
			set
			{
				ValidationUtils.ArgumentNotNull(key, "o");
				string text = key as string;
				if (text == null)
				{
					throw new ArgumentException("Set JObject values with invalid key value: {0}. Object property name expected.".FormatWith(CultureInfo.InvariantCulture, MiscellaneousUtils.ToString(key)));
				}
				this[text] = value;
			}
		}

		// Token: 0x17000134 RID: 308
		public JToken this[string propertyName]
		{
			get
			{
				ValidationUtils.ArgumentNotNull(propertyName, "propertyName");
				JProperty jproperty = this.Property(propertyName);
				if (jproperty == null)
				{
					return null;
				}
				return jproperty.Value;
			}
			set
			{
				JProperty jproperty = this.Property(propertyName);
				if (jproperty != null)
				{
					jproperty.Value = value;
					return;
				}
				this.OnPropertyChanging(propertyName);
				this.Add(new JProperty(propertyName, value));
				this.OnPropertyChanged(propertyName);
			}
		}

		// Token: 0x060005BB RID: 1467 RVA: 0x00015D5C File Offset: 0x00013F5C
		public new static JObject Load(JsonReader reader)
		{
			ValidationUtils.ArgumentNotNull(reader, "reader");
			if (reader.TokenType == JsonToken.None && !reader.Read())
			{
				throw JsonReaderException.Create(reader, "Error reading JObject from JsonReader.");
			}
			while (reader.TokenType == JsonToken.Comment)
			{
				reader.Read();
			}
			if (reader.TokenType != JsonToken.StartObject)
			{
				throw JsonReaderException.Create(reader, "Error reading JObject from JsonReader. Current JsonReader item is not an object: {0}".FormatWith(CultureInfo.InvariantCulture, reader.TokenType));
			}
			JObject jobject = new JObject();
			jobject.SetLineInfo(reader as IJsonLineInfo);
			jobject.ReadTokenFrom(reader);
			return jobject;
		}

		// Token: 0x060005BC RID: 1468 RVA: 0x00015DE4 File Offset: 0x00013FE4
		public new static JObject Parse(string json)
		{
			JObject result;
			using (JsonReader jsonReader = new JsonTextReader(new StringReader(json)))
			{
				JObject jobject = JObject.Load(jsonReader);
				if (jsonReader.Read() && jsonReader.TokenType != JsonToken.Comment)
				{
					throw JsonReaderException.Create(jsonReader, "Additional text found in JSON string after parsing content.");
				}
				result = jobject;
			}
			return result;
		}

		// Token: 0x060005BD RID: 1469 RVA: 0x00015E40 File Offset: 0x00014040
		public new static JObject FromObject(object o)
		{
			return JObject.FromObject(o, JsonSerializer.CreateDefault());
		}

		// Token: 0x060005BE RID: 1470 RVA: 0x00015E50 File Offset: 0x00014050
		public new static JObject FromObject(object o, JsonSerializer jsonSerializer)
		{
			JToken jtoken = JToken.FromObjectInternal(o, jsonSerializer);
			if (jtoken != null && jtoken.Type != JTokenType.Object)
			{
				throw new ArgumentException("Object serialized to {0}. JObject instance expected.".FormatWith(CultureInfo.InvariantCulture, jtoken.Type));
			}
			return (JObject)jtoken;
		}

		// Token: 0x060005BF RID: 1471 RVA: 0x00015E98 File Offset: 0x00014098
		public override void WriteTo(JsonWriter writer, params JsonConverter[] converters)
		{
			writer.WriteStartObject();
			for (int i = 0; i < this._properties.Count; i++)
			{
				this._properties[i].WriteTo(writer, converters);
			}
			writer.WriteEndObject();
		}

		// Token: 0x060005C0 RID: 1472 RVA: 0x00015EDA File Offset: 0x000140DA
		public JToken GetValue(string propertyName)
		{
			return this.GetValue(propertyName, StringComparison.Ordinal);
		}

		// Token: 0x060005C1 RID: 1473 RVA: 0x00015EE4 File Offset: 0x000140E4
		public JToken GetValue(string propertyName, StringComparison comparison)
		{
			if (propertyName == null)
			{
				return null;
			}
			JProperty jproperty = this.Property(propertyName);
			if (jproperty != null)
			{
				return jproperty.Value;
			}
			if (comparison != StringComparison.Ordinal)
			{
				foreach (JToken jtoken in this._properties)
				{
					JProperty jproperty2 = (JProperty)jtoken;
					if (string.Equals(jproperty2.Name, propertyName, comparison))
					{
						return jproperty2.Value;
					}
				}
			}
			return null;
		}

		// Token: 0x060005C2 RID: 1474 RVA: 0x00015F68 File Offset: 0x00014168
		public bool TryGetValue(string propertyName, StringComparison comparison, out JToken value)
		{
			value = this.GetValue(propertyName, comparison);
			return value != null;
		}

		// Token: 0x060005C3 RID: 1475 RVA: 0x00015F7C File Offset: 0x0001417C
		public void Add(string propertyName, JToken value)
		{
			this.Add(new JProperty(propertyName, value));
		}

		// Token: 0x060005C4 RID: 1476 RVA: 0x00015F8B File Offset: 0x0001418B
		bool IDictionary<string, JToken>.ContainsKey(string key)
		{
			return this._properties.Contains(key);
		}

		// Token: 0x17000135 RID: 309
		// (get) Token: 0x060005C5 RID: 1477 RVA: 0x00015F99 File Offset: 0x00014199
		ICollection<string> IDictionary<string, JToken>.Keys
		{
			get
			{
				return this._properties.Keys;
			}
		}

		// Token: 0x060005C6 RID: 1478 RVA: 0x00015FA8 File Offset: 0x000141A8
		public bool Remove(string propertyName)
		{
			JProperty jproperty = this.Property(propertyName);
			if (jproperty == null)
			{
				return false;
			}
			jproperty.Remove();
			return true;
		}

		// Token: 0x060005C7 RID: 1479 RVA: 0x00015FCC File Offset: 0x000141CC
		public bool TryGetValue(string propertyName, out JToken value)
		{
			JProperty jproperty = this.Property(propertyName);
			if (jproperty == null)
			{
				value = null;
				return false;
			}
			value = jproperty.Value;
			return true;
		}

		// Token: 0x17000136 RID: 310
		// (get) Token: 0x060005C8 RID: 1480 RVA: 0x00015FF2 File Offset: 0x000141F2
		ICollection<JToken> IDictionary<string, JToken>.Values
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		// Token: 0x060005C9 RID: 1481 RVA: 0x00015FF9 File Offset: 0x000141F9
		void ICollection<KeyValuePair<string, JToken>>.Add(KeyValuePair<string, JToken> item)
		{
			this.Add(new JProperty(item.Key, item.Value));
		}

		// Token: 0x060005CA RID: 1482 RVA: 0x00016014 File Offset: 0x00014214
		void ICollection<KeyValuePair<string, JToken>>.Clear()
		{
			base.RemoveAll();
		}

		// Token: 0x060005CB RID: 1483 RVA: 0x0001601C File Offset: 0x0001421C
		bool ICollection<KeyValuePair<string, JToken>>.Contains(KeyValuePair<string, JToken> item)
		{
			JProperty jproperty = this.Property(item.Key);
			return jproperty != null && jproperty.Value == item.Value;
		}

		// Token: 0x060005CC RID: 1484 RVA: 0x0001604C File Offset: 0x0001424C
		void ICollection<KeyValuePair<string, JToken>>.CopyTo(KeyValuePair<string, JToken>[] array, int arrayIndex)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			if (arrayIndex < 0)
			{
				throw new ArgumentOutOfRangeException("arrayIndex", "arrayIndex is less than 0.");
			}
			if (arrayIndex >= array.Length && arrayIndex != 0)
			{
				throw new ArgumentException("arrayIndex is equal to or greater than the length of array.");
			}
			if (base.Count > array.Length - arrayIndex)
			{
				throw new ArgumentException("The number of elements in the source JObject is greater than the available space from arrayIndex to the end of the destination array.");
			}
			int num = 0;
			foreach (JToken jtoken in this._properties)
			{
				JProperty jproperty = (JProperty)jtoken;
				array[arrayIndex + num] = new KeyValuePair<string, JToken>(jproperty.Name, jproperty.Value);
				num++;
			}
		}

		// Token: 0x17000137 RID: 311
		// (get) Token: 0x060005CD RID: 1485 RVA: 0x0001610C File Offset: 0x0001430C
		bool ICollection<KeyValuePair<string, JToken>>.IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060005CE RID: 1486 RVA: 0x0001610F File Offset: 0x0001430F
		bool ICollection<KeyValuePair<string, JToken>>.Remove(KeyValuePair<string, JToken> item)
		{
			if (!((ICollection<KeyValuePair<string, JToken>>)this).Contains(item))
			{
				return false;
			}
			((IDictionary<string, JToken>)this).Remove(item.Key);
			return true;
		}

		// Token: 0x060005CF RID: 1487 RVA: 0x0001612B File Offset: 0x0001432B
		internal override int GetDeepHashCode()
		{
			return base.ContentsHashCode();
		}

		// Token: 0x060005D0 RID: 1488 RVA: 0x00016288 File Offset: 0x00014488
		public IEnumerator<KeyValuePair<string, JToken>> GetEnumerator()
		{
			foreach (JToken jtoken in this._properties)
			{
				JProperty property = (JProperty)jtoken;
				yield return new KeyValuePair<string, JToken>(property.Name, property.Value);
			}
			yield break;
		}

		// Token: 0x060005D1 RID: 1489 RVA: 0x000162A4 File Offset: 0x000144A4
		protected virtual void OnPropertyChanged(string propertyName)
		{
			if (this.PropertyChanged != null)
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		// Token: 0x060005D2 RID: 1490 RVA: 0x000162C0 File Offset: 0x000144C0
		protected virtual void OnPropertyChanging(string propertyName)
		{
			if (this.PropertyChanging != null)
			{
				this.PropertyChanging(this, new PropertyChangingEventArgs(propertyName));
			}
		}

		// Token: 0x060005D3 RID: 1491 RVA: 0x000162DC File Offset: 0x000144DC
		PropertyDescriptorCollection ICustomTypeDescriptor.GetProperties()
		{
			return ((ICustomTypeDescriptor)this).GetProperties(null);
		}

		// Token: 0x060005D4 RID: 1492 RVA: 0x000162E8 File Offset: 0x000144E8
		PropertyDescriptorCollection ICustomTypeDescriptor.GetProperties(Attribute[] attributes)
		{
			PropertyDescriptorCollection propertyDescriptorCollection = new PropertyDescriptorCollection(null);
			foreach (KeyValuePair<string, JToken> keyValuePair in this)
			{
				propertyDescriptorCollection.Add(new JPropertyDescriptor(keyValuePair.Key));
			}
			return propertyDescriptorCollection;
		}

		// Token: 0x060005D5 RID: 1493 RVA: 0x00016344 File Offset: 0x00014544
		AttributeCollection ICustomTypeDescriptor.GetAttributes()
		{
			return AttributeCollection.Empty;
		}

		// Token: 0x060005D6 RID: 1494 RVA: 0x0001634B File Offset: 0x0001454B
		string ICustomTypeDescriptor.GetClassName()
		{
			return null;
		}

		// Token: 0x060005D7 RID: 1495 RVA: 0x0001634E File Offset: 0x0001454E
		string ICustomTypeDescriptor.GetComponentName()
		{
			return null;
		}

		// Token: 0x060005D8 RID: 1496 RVA: 0x00016351 File Offset: 0x00014551
		TypeConverter ICustomTypeDescriptor.GetConverter()
		{
			return new TypeConverter();
		}

		// Token: 0x060005D9 RID: 1497 RVA: 0x00016358 File Offset: 0x00014558
		EventDescriptor ICustomTypeDescriptor.GetDefaultEvent()
		{
			return null;
		}

		// Token: 0x060005DA RID: 1498 RVA: 0x0001635B File Offset: 0x0001455B
		PropertyDescriptor ICustomTypeDescriptor.GetDefaultProperty()
		{
			return null;
		}

		// Token: 0x060005DB RID: 1499 RVA: 0x0001635E File Offset: 0x0001455E
		object ICustomTypeDescriptor.GetEditor(Type editorBaseType)
		{
			return null;
		}

		// Token: 0x060005DC RID: 1500 RVA: 0x00016361 File Offset: 0x00014561
		EventDescriptorCollection ICustomTypeDescriptor.GetEvents(Attribute[] attributes)
		{
			return EventDescriptorCollection.Empty;
		}

		// Token: 0x060005DD RID: 1501 RVA: 0x00016368 File Offset: 0x00014568
		EventDescriptorCollection ICustomTypeDescriptor.GetEvents()
		{
			return EventDescriptorCollection.Empty;
		}

		// Token: 0x060005DE RID: 1502 RVA: 0x0001636F File Offset: 0x0001456F
		object ICustomTypeDescriptor.GetPropertyOwner(PropertyDescriptor pd)
		{
			return null;
		}

		// Token: 0x060005DF RID: 1503 RVA: 0x00016372 File Offset: 0x00014572
		protected override DynamicMetaObject GetMetaObject(Expression parameter)
		{
			return new DynamicProxyMetaObject<JObject>(parameter, this, new JObject.JObjectDynamicProxy(), true);
		}

		// Token: 0x040001BC RID: 444
		private readonly JPropertyKeyedCollection _properties = new JPropertyKeyedCollection();

		// Token: 0x02000068 RID: 104
		private class JObjectDynamicProxy : DynamicProxy<JObject>
		{
			// Token: 0x060005EF RID: 1519 RVA: 0x000163D2 File Offset: 0x000145D2
			public override bool TryGetMember(JObject instance, GetMemberBinder binder, out object result)
			{
				result = instance[binder.Name];
				return true;
			}

			// Token: 0x060005F0 RID: 1520 RVA: 0x000163E4 File Offset: 0x000145E4
			public override bool TrySetMember(JObject instance, SetMemberBinder binder, object value)
			{
				JToken jtoken = value as JToken;
				if (jtoken == null)
				{
					jtoken = new JValue(value);
				}
				instance[binder.Name] = jtoken;
				return true;
			}

			// Token: 0x060005F1 RID: 1521 RVA: 0x00016418 File Offset: 0x00014618
			public override IEnumerable<string> GetDynamicMemberNames(JObject instance)
			{
				return from p in instance.Properties()
				select p.Name;
			}
		}
	}
}
