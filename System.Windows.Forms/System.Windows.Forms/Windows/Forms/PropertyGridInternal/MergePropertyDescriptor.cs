using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace System.Windows.Forms.PropertyGridInternal
{
	// Token: 0x0200048E RID: 1166
	internal class MergePropertyDescriptor : PropertyDescriptor
	{
		// Token: 0x06004E69 RID: 20073 RVA: 0x00141B54 File Offset: 0x0013FD54
		public MergePropertyDescriptor(PropertyDescriptor[] descriptors) : base(descriptors[0].Name, null)
		{
			this.descriptors = descriptors;
		}

		// Token: 0x1700136B RID: 4971
		// (get) Token: 0x06004E6A RID: 20074 RVA: 0x00141B6C File Offset: 0x0013FD6C
		public override Type ComponentType
		{
			get
			{
				return this.descriptors[0].ComponentType;
			}
		}

		// Token: 0x1700136C RID: 4972
		// (get) Token: 0x06004E6B RID: 20075 RVA: 0x00141B7B File Offset: 0x0013FD7B
		public override TypeConverter Converter
		{
			get
			{
				return this.descriptors[0].Converter;
			}
		}

		// Token: 0x1700136D RID: 4973
		// (get) Token: 0x06004E6C RID: 20076 RVA: 0x00141B8A File Offset: 0x0013FD8A
		public override string DisplayName
		{
			get
			{
				return this.descriptors[0].DisplayName;
			}
		}

		// Token: 0x1700136E RID: 4974
		// (get) Token: 0x06004E6D RID: 20077 RVA: 0x00141B9C File Offset: 0x0013FD9C
		public override bool IsLocalizable
		{
			get
			{
				if (this.localizable == MergePropertyDescriptor.TriState.Unknown)
				{
					this.localizable = MergePropertyDescriptor.TriState.Yes;
					foreach (PropertyDescriptor propertyDescriptor in this.descriptors)
					{
						if (!propertyDescriptor.IsLocalizable)
						{
							this.localizable = MergePropertyDescriptor.TriState.No;
							break;
						}
					}
				}
				return this.localizable == MergePropertyDescriptor.TriState.Yes;
			}
		}

		// Token: 0x1700136F RID: 4975
		// (get) Token: 0x06004E6E RID: 20078 RVA: 0x00141BEC File Offset: 0x0013FDEC
		public override bool IsReadOnly
		{
			get
			{
				if (this.readOnly == MergePropertyDescriptor.TriState.Unknown)
				{
					this.readOnly = MergePropertyDescriptor.TriState.No;
					foreach (PropertyDescriptor propertyDescriptor in this.descriptors)
					{
						if (propertyDescriptor.IsReadOnly)
						{
							this.readOnly = MergePropertyDescriptor.TriState.Yes;
							break;
						}
					}
				}
				return this.readOnly == MergePropertyDescriptor.TriState.Yes;
			}
		}

		// Token: 0x17001370 RID: 4976
		// (get) Token: 0x06004E6F RID: 20079 RVA: 0x00141C3B File Offset: 0x0013FE3B
		public override Type PropertyType
		{
			get
			{
				return this.descriptors[0].PropertyType;
			}
		}

		// Token: 0x17001371 RID: 4977
		public PropertyDescriptor this[int index]
		{
			get
			{
				return this.descriptors[index];
			}
		}

		// Token: 0x06004E71 RID: 20081 RVA: 0x00141C54 File Offset: 0x0013FE54
		public override bool CanResetValue(object component)
		{
			if (this.canReset == MergePropertyDescriptor.TriState.Unknown)
			{
				this.canReset = MergePropertyDescriptor.TriState.Yes;
				Array a = (Array)component;
				for (int i = 0; i < this.descriptors.Length; i++)
				{
					if (!this.descriptors[i].CanResetValue(this.GetPropertyOwnerForComponent(a, i)))
					{
						this.canReset = MergePropertyDescriptor.TriState.No;
						break;
					}
				}
			}
			return this.canReset == MergePropertyDescriptor.TriState.Yes;
		}

		// Token: 0x06004E72 RID: 20082 RVA: 0x00141CB4 File Offset: 0x0013FEB4
		private object CopyValue(object value)
		{
			if (value == null)
			{
				return value;
			}
			Type type = value.GetType();
			if (type.IsValueType)
			{
				return value;
			}
			object obj = null;
			ICloneable cloneable = value as ICloneable;
			if (cloneable != null)
			{
				obj = cloneable.Clone();
			}
			if (obj == null)
			{
				TypeConverter converter = TypeDescriptor.GetConverter(value);
				if (converter.CanConvertTo(typeof(InstanceDescriptor)))
				{
					InstanceDescriptor instanceDescriptor = (InstanceDescriptor)converter.ConvertTo(null, CultureInfo.InvariantCulture, value, typeof(InstanceDescriptor));
					if (instanceDescriptor != null && instanceDescriptor.IsComplete)
					{
						obj = instanceDescriptor.Invoke();
					}
				}
				if (obj == null && converter.CanConvertTo(typeof(string)) && converter.CanConvertFrom(typeof(string)))
				{
					object obj2 = converter.ConvertToInvariantString(value);
					obj = converter.ConvertFromInvariantString((string)obj2);
				}
			}
			if (obj == null && type.IsSerializable)
			{
				BinaryFormatter binaryFormatter = new BinaryFormatter();
				MemoryStream memoryStream = new MemoryStream();
				binaryFormatter.Serialize(memoryStream, value);
				memoryStream.Position = 0L;
				obj = binaryFormatter.Deserialize(memoryStream);
			}
			if (obj != null)
			{
				return obj;
			}
			return value;
		}

		// Token: 0x06004E73 RID: 20083 RVA: 0x00141DB6 File Offset: 0x0013FFB6
		protected override AttributeCollection CreateAttributeCollection()
		{
			return new MergePropertyDescriptor.MergedAttributeCollection(this);
		}

		// Token: 0x06004E74 RID: 20084 RVA: 0x00141DC0 File Offset: 0x0013FFC0
		private object GetPropertyOwnerForComponent(Array a, int i)
		{
			object obj = a.GetValue(i);
			if (obj is ICustomTypeDescriptor)
			{
				obj = ((ICustomTypeDescriptor)obj).GetPropertyOwner(this.descriptors[i]);
			}
			return obj;
		}

		// Token: 0x06004E75 RID: 20085 RVA: 0x00141DF2 File Offset: 0x0013FFF2
		public override object GetEditor(Type editorBaseType)
		{
			return this.descriptors[0].GetEditor(editorBaseType);
		}

		// Token: 0x06004E76 RID: 20086 RVA: 0x00141E04 File Offset: 0x00140004
		public override object GetValue(object component)
		{
			bool flag;
			return this.GetValue((Array)component, out flag);
		}

		// Token: 0x06004E77 RID: 20087 RVA: 0x00141E20 File Offset: 0x00140020
		public object GetValue(Array components, out bool allEqual)
		{
			allEqual = true;
			object value = this.descriptors[0].GetValue(this.GetPropertyOwnerForComponent(components, 0));
			if (value is ICollection)
			{
				if (this.collection == null)
				{
					this.collection = new MergePropertyDescriptor.MultiMergeCollection((ICollection)value);
				}
				else
				{
					if (this.collection.Locked)
					{
						return this.collection;
					}
					this.collection.SetItems((ICollection)value);
				}
			}
			for (int i = 1; i < this.descriptors.Length; i++)
			{
				object value2 = this.descriptors[i].GetValue(this.GetPropertyOwnerForComponent(components, i));
				if (this.collection != null)
				{
					if (!this.collection.MergeCollection((ICollection)value2))
					{
						allEqual = false;
						return null;
					}
				}
				else if ((value != null || value2 != null) && (value == null || !value.Equals(value2)))
				{
					allEqual = false;
					return null;
				}
			}
			if (allEqual && this.collection != null && this.collection.Count == 0)
			{
				return null;
			}
			if (this.collection == null)
			{
				return value;
			}
			return this.collection;
		}

		// Token: 0x06004E78 RID: 20088 RVA: 0x00141F1C File Offset: 0x0014011C
		internal object[] GetValues(Array components)
		{
			object[] array = new object[components.Length];
			for (int i = 0; i < components.Length; i++)
			{
				array[i] = this.descriptors[i].GetValue(this.GetPropertyOwnerForComponent(components, i));
			}
			return array;
		}

		// Token: 0x06004E79 RID: 20089 RVA: 0x00141F60 File Offset: 0x00140160
		public override void ResetValue(object component)
		{
			Array a = (Array)component;
			for (int i = 0; i < this.descriptors.Length; i++)
			{
				this.descriptors[i].ResetValue(this.GetPropertyOwnerForComponent(a, i));
			}
		}

		// Token: 0x06004E7A RID: 20090 RVA: 0x00141F9C File Offset: 0x0014019C
		private void SetCollectionValues(Array a, IList listValue)
		{
			try
			{
				if (this.collection != null)
				{
					this.collection.Locked = true;
				}
				object[] array = new object[listValue.Count];
				listValue.CopyTo(array, 0);
				for (int i = 0; i < this.descriptors.Length; i++)
				{
					IList list = this.descriptors[i].GetValue(this.GetPropertyOwnerForComponent(a, i)) as IList;
					if (list != null)
					{
						list.Clear();
						foreach (object value in array)
						{
							list.Add(value);
						}
					}
				}
			}
			finally
			{
				if (this.collection != null)
				{
					this.collection.Locked = false;
				}
			}
		}

		// Token: 0x06004E7B RID: 20091 RVA: 0x00142054 File Offset: 0x00140254
		public override void SetValue(object component, object value)
		{
			Array a = (Array)component;
			if (value is IList && typeof(IList).IsAssignableFrom(this.PropertyType))
			{
				this.SetCollectionValues(a, (IList)value);
				return;
			}
			for (int i = 0; i < this.descriptors.Length; i++)
			{
				object value2 = this.CopyValue(value);
				this.descriptors[i].SetValue(this.GetPropertyOwnerForComponent(a, i), value2);
			}
		}

		// Token: 0x06004E7C RID: 20092 RVA: 0x001420C8 File Offset: 0x001402C8
		public override bool ShouldSerializeValue(object component)
		{
			Array a = (Array)component;
			for (int i = 0; i < this.descriptors.Length; i++)
			{
				if (!this.descriptors[i].ShouldSerializeValue(this.GetPropertyOwnerForComponent(a, i)))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0400334E RID: 13134
		private PropertyDescriptor[] descriptors;

		// Token: 0x0400334F RID: 13135
		private MergePropertyDescriptor.TriState localizable;

		// Token: 0x04003350 RID: 13136
		private MergePropertyDescriptor.TriState readOnly;

		// Token: 0x04003351 RID: 13137
		private MergePropertyDescriptor.TriState canReset;

		// Token: 0x04003352 RID: 13138
		private MergePropertyDescriptor.MultiMergeCollection collection;

		// Token: 0x0200082B RID: 2091
		private enum TriState
		{
			// Token: 0x0400427F RID: 17023
			Unknown,
			// Token: 0x04004280 RID: 17024
			Yes,
			// Token: 0x04004281 RID: 17025
			No
		}

		// Token: 0x0200082C RID: 2092
		private class MultiMergeCollection : ICollection, IEnumerable
		{
			// Token: 0x06006E99 RID: 28313 RVA: 0x00194AB3 File Offset: 0x00192CB3
			public MultiMergeCollection(ICollection original)
			{
				this.SetItems(original);
			}

			// Token: 0x170017E7 RID: 6119
			// (get) Token: 0x06006E9A RID: 28314 RVA: 0x00194AC2 File Offset: 0x00192CC2
			public int Count
			{
				get
				{
					if (this.items != null)
					{
						return this.items.Length;
					}
					return 0;
				}
			}

			// Token: 0x170017E8 RID: 6120
			// (get) Token: 0x06006E9B RID: 28315 RVA: 0x00194AD6 File Offset: 0x00192CD6
			// (set) Token: 0x06006E9C RID: 28316 RVA: 0x00194ADE File Offset: 0x00192CDE
			public bool Locked
			{
				get
				{
					return this.locked;
				}
				set
				{
					this.locked = value;
				}
			}

			// Token: 0x170017E9 RID: 6121
			// (get) Token: 0x06006E9D RID: 28317 RVA: 0x000069BD File Offset: 0x00004BBD
			object ICollection.SyncRoot
			{
				get
				{
					return this;
				}
			}

			// Token: 0x170017EA RID: 6122
			// (get) Token: 0x06006E9E RID: 28318 RVA: 0x0000E0A4 File Offset: 0x0000C2A4
			bool ICollection.IsSynchronized
			{
				get
				{
					return false;
				}
			}

			// Token: 0x06006E9F RID: 28319 RVA: 0x00194AE7 File Offset: 0x00192CE7
			public void CopyTo(Array array, int index)
			{
				if (this.items == null)
				{
					return;
				}
				Array.Copy(this.items, 0, array, index, this.items.Length);
			}

			// Token: 0x06006EA0 RID: 28320 RVA: 0x00194B08 File Offset: 0x00192D08
			public IEnumerator GetEnumerator()
			{
				if (this.items != null)
				{
					return this.items.GetEnumerator();
				}
				return new object[0].GetEnumerator();
			}

			// Token: 0x06006EA1 RID: 28321 RVA: 0x00194B2C File Offset: 0x00192D2C
			public bool MergeCollection(ICollection newCollection)
			{
				if (this.locked)
				{
					return true;
				}
				if (this.items.Length != newCollection.Count)
				{
					this.items = new object[0];
					return false;
				}
				object[] array = new object[newCollection.Count];
				newCollection.CopyTo(array, 0);
				for (int i = 0; i < array.Length; i++)
				{
					if (array[i] == null != (this.items[i] == null) || (this.items[i] != null && !this.items[i].Equals(array[i])))
					{
						this.items = new object[0];
						return false;
					}
				}
				return true;
			}

			// Token: 0x06006EA2 RID: 28322 RVA: 0x00194BC1 File Offset: 0x00192DC1
			public void SetItems(ICollection collection)
			{
				if (this.locked)
				{
					return;
				}
				this.items = new object[collection.Count];
				collection.CopyTo(this.items, 0);
			}

			// Token: 0x04004282 RID: 17026
			private object[] items;

			// Token: 0x04004283 RID: 17027
			private bool locked;
		}

		// Token: 0x0200082D RID: 2093
		private class MergedAttributeCollection : AttributeCollection
		{
			// Token: 0x06006EA3 RID: 28323 RVA: 0x00194BEA File Offset: 0x00192DEA
			public MergedAttributeCollection(MergePropertyDescriptor owner) : base(null)
			{
				this.owner = owner;
			}

			// Token: 0x170017EB RID: 6123
			public override Attribute this[Type attributeType]
			{
				get
				{
					return this.GetCommonAttribute(attributeType);
				}
			}

			// Token: 0x06006EA5 RID: 28325 RVA: 0x00194C04 File Offset: 0x00192E04
			private Attribute GetCommonAttribute(Type attributeType)
			{
				if (this.attributeCollections == null)
				{
					this.attributeCollections = new AttributeCollection[this.owner.descriptors.Length];
					for (int i = 0; i < this.owner.descriptors.Length; i++)
					{
						this.attributeCollections[i] = this.owner.descriptors[i].Attributes;
					}
				}
				if (this.attributeCollections.Length == 0)
				{
					return base.GetDefaultAttribute(attributeType);
				}
				Attribute attribute;
				if (this.foundAttributes != null)
				{
					attribute = (this.foundAttributes[attributeType] as Attribute);
					if (attribute != null)
					{
						return attribute;
					}
				}
				attribute = this.attributeCollections[0][attributeType];
				if (attribute == null)
				{
					return null;
				}
				for (int j = 1; j < this.attributeCollections.Length; j++)
				{
					Attribute obj = this.attributeCollections[j][attributeType];
					if (!attribute.Equals(obj))
					{
						attribute = base.GetDefaultAttribute(attributeType);
						break;
					}
				}
				if (this.foundAttributes == null)
				{
					this.foundAttributes = new Hashtable();
				}
				this.foundAttributes[attributeType] = attribute;
				return attribute;
			}

			// Token: 0x04004284 RID: 17028
			private MergePropertyDescriptor owner;

			// Token: 0x04004285 RID: 17029
			private AttributeCollection[] attributeCollections;

			// Token: 0x04004286 RID: 17030
			private IDictionary foundAttributes;
		}
	}
}
