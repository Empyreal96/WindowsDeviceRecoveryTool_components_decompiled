using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Linq
{
	// Token: 0x02000071 RID: 113
	public class JProperty : JContainer
	{
		// Token: 0x17000141 RID: 321
		// (get) Token: 0x06000626 RID: 1574 RVA: 0x000187A8 File Offset: 0x000169A8
		protected override IList<JToken> ChildrenTokens
		{
			get
			{
				return this._content;
			}
		}

		// Token: 0x17000142 RID: 322
		// (get) Token: 0x06000627 RID: 1575 RVA: 0x000187B0 File Offset: 0x000169B0
		public string Name
		{
			[DebuggerStepThrough]
			get
			{
				return this._name;
			}
		}

		// Token: 0x17000143 RID: 323
		// (get) Token: 0x06000628 RID: 1576 RVA: 0x000187B8 File Offset: 0x000169B8
		// (set) Token: 0x06000629 RID: 1577 RVA: 0x000187C8 File Offset: 0x000169C8
		public new JToken Value
		{
			[DebuggerStepThrough]
			get
			{
				return this._content._token;
			}
			set
			{
				base.CheckReentrancy();
				JToken item = value ?? JValue.CreateNull();
				if (this._content._token == null)
				{
					this.InsertItem(0, item, false);
					return;
				}
				this.SetItem(0, item);
			}
		}

		// Token: 0x0600062A RID: 1578 RVA: 0x00018805 File Offset: 0x00016A05
		public JProperty(JProperty other) : base(other)
		{
			this._name = other.Name;
		}

		// Token: 0x0600062B RID: 1579 RVA: 0x00018825 File Offset: 0x00016A25
		internal override JToken GetItem(int index)
		{
			if (index != 0)
			{
				throw new ArgumentOutOfRangeException();
			}
			return this.Value;
		}

		// Token: 0x0600062C RID: 1580 RVA: 0x00018838 File Offset: 0x00016A38
		internal override void SetItem(int index, JToken item)
		{
			if (index != 0)
			{
				throw new ArgumentOutOfRangeException();
			}
			if (JContainer.IsTokenUnchanged(this.Value, item))
			{
				return;
			}
			if (base.Parent != null)
			{
				((JObject)base.Parent).InternalPropertyChanging(this);
			}
			base.SetItem(0, item);
			if (base.Parent != null)
			{
				((JObject)base.Parent).InternalPropertyChanged(this);
			}
		}

		// Token: 0x0600062D RID: 1581 RVA: 0x00018897 File Offset: 0x00016A97
		internal override bool RemoveItem(JToken item)
		{
			throw new JsonException("Cannot add or remove items from {0}.".FormatWith(CultureInfo.InvariantCulture, typeof(JProperty)));
		}

		// Token: 0x0600062E RID: 1582 RVA: 0x000188B7 File Offset: 0x00016AB7
		internal override void RemoveItemAt(int index)
		{
			throw new JsonException("Cannot add or remove items from {0}.".FormatWith(CultureInfo.InvariantCulture, typeof(JProperty)));
		}

		// Token: 0x0600062F RID: 1583 RVA: 0x000188D7 File Offset: 0x00016AD7
		internal override void InsertItem(int index, JToken item, bool skipParentCheck)
		{
			if (item != null && item.Type == JTokenType.Comment)
			{
				return;
			}
			if (this.Value != null)
			{
				throw new JsonException("{0} cannot have multiple values.".FormatWith(CultureInfo.InvariantCulture, typeof(JProperty)));
			}
			base.InsertItem(0, item, false);
		}

		// Token: 0x06000630 RID: 1584 RVA: 0x00018916 File Offset: 0x00016B16
		internal override bool ContainsItem(JToken item)
		{
			return this.Value == item;
		}

		// Token: 0x06000631 RID: 1585 RVA: 0x00018924 File Offset: 0x00016B24
		internal override void MergeItem(object content, JsonMergeSettings settings)
		{
			JProperty jproperty = content as JProperty;
			if (jproperty == null)
			{
				return;
			}
			if (jproperty.Value != null && jproperty.Value.Type != JTokenType.Null)
			{
				this.Value = jproperty.Value;
			}
		}

		// Token: 0x06000632 RID: 1586 RVA: 0x0001895F File Offset: 0x00016B5F
		internal override void ClearItems()
		{
			throw new JsonException("Cannot add or remove items from {0}.".FormatWith(CultureInfo.InvariantCulture, typeof(JProperty)));
		}

		// Token: 0x06000633 RID: 1587 RVA: 0x00018980 File Offset: 0x00016B80
		internal override bool DeepEquals(JToken node)
		{
			JProperty jproperty = node as JProperty;
			return jproperty != null && this._name == jproperty.Name && base.ContentsEqual(jproperty);
		}

		// Token: 0x06000634 RID: 1588 RVA: 0x000189B3 File Offset: 0x00016BB3
		internal override JToken CloneToken()
		{
			return new JProperty(this);
		}

		// Token: 0x17000144 RID: 324
		// (get) Token: 0x06000635 RID: 1589 RVA: 0x000189BB File Offset: 0x00016BBB
		public override JTokenType Type
		{
			[DebuggerStepThrough]
			get
			{
				return JTokenType.Property;
			}
		}

		// Token: 0x06000636 RID: 1590 RVA: 0x000189BE File Offset: 0x00016BBE
		internal JProperty(string name)
		{
			ValidationUtils.ArgumentNotNull(name, "name");
			this._name = name;
		}

		// Token: 0x06000637 RID: 1591 RVA: 0x000189E3 File Offset: 0x00016BE3
		public JProperty(string name, params object[] content) : this(name, content)
		{
		}

		// Token: 0x06000638 RID: 1592 RVA: 0x000189F0 File Offset: 0x00016BF0
		public JProperty(string name, object content)
		{
			ValidationUtils.ArgumentNotNull(name, "name");
			this._name = name;
			this.Value = (base.IsMultiContent(content) ? new JArray(content) : JContainer.CreateFromContent(content));
		}

		// Token: 0x06000639 RID: 1593 RVA: 0x00018A40 File Offset: 0x00016C40
		public override void WriteTo(JsonWriter writer, params JsonConverter[] converters)
		{
			writer.WritePropertyName(this._name);
			JToken value = this.Value;
			if (value != null)
			{
				value.WriteTo(writer, converters);
				return;
			}
			writer.WriteNull();
		}

		// Token: 0x0600063A RID: 1594 RVA: 0x00018A72 File Offset: 0x00016C72
		internal override int GetDeepHashCode()
		{
			return this._name.GetHashCode() ^ ((this.Value != null) ? this.Value.GetDeepHashCode() : 0);
		}

		// Token: 0x0600063B RID: 1595 RVA: 0x00018A98 File Offset: 0x00016C98
		public new static JProperty Load(JsonReader reader)
		{
			if (reader.TokenType == JsonToken.None && !reader.Read())
			{
				throw JsonReaderException.Create(reader, "Error reading JProperty from JsonReader.");
			}
			while (reader.TokenType == JsonToken.Comment)
			{
				reader.Read();
			}
			if (reader.TokenType != JsonToken.PropertyName)
			{
				throw JsonReaderException.Create(reader, "Error reading JProperty from JsonReader. Current JsonReader item is not a property: {0}".FormatWith(CultureInfo.InvariantCulture, reader.TokenType));
			}
			JProperty jproperty = new JProperty((string)reader.Value);
			jproperty.SetLineInfo(reader as IJsonLineInfo);
			jproperty.ReadTokenFrom(reader);
			return jproperty;
		}

		// Token: 0x040001CD RID: 461
		private readonly JProperty.JPropertyList _content = new JProperty.JPropertyList();

		// Token: 0x040001CE RID: 462
		private readonly string _name;

		// Token: 0x02000072 RID: 114
		private class JPropertyList : IList<JToken>, ICollection<JToken>, IEnumerable<JToken>, IEnumerable
		{
			// Token: 0x0600063C RID: 1596 RVA: 0x00018BA4 File Offset: 0x00016DA4
			public IEnumerator<JToken> GetEnumerator()
			{
				if (this._token != null)
				{
					yield return this._token;
				}
				yield break;
			}

			// Token: 0x0600063D RID: 1597 RVA: 0x00018BC0 File Offset: 0x00016DC0
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.GetEnumerator();
			}

			// Token: 0x0600063E RID: 1598 RVA: 0x00018BC8 File Offset: 0x00016DC8
			public void Add(JToken item)
			{
				this._token = item;
			}

			// Token: 0x0600063F RID: 1599 RVA: 0x00018BD1 File Offset: 0x00016DD1
			public void Clear()
			{
				this._token = null;
			}

			// Token: 0x06000640 RID: 1600 RVA: 0x00018BDA File Offset: 0x00016DDA
			public bool Contains(JToken item)
			{
				return this._token == item;
			}

			// Token: 0x06000641 RID: 1601 RVA: 0x00018BE5 File Offset: 0x00016DE5
			public void CopyTo(JToken[] array, int arrayIndex)
			{
				if (this._token != null)
				{
					array[arrayIndex] = this._token;
				}
			}

			// Token: 0x06000642 RID: 1602 RVA: 0x00018BF8 File Offset: 0x00016DF8
			public bool Remove(JToken item)
			{
				if (this._token == item)
				{
					this._token = null;
					return true;
				}
				return false;
			}

			// Token: 0x17000145 RID: 325
			// (get) Token: 0x06000643 RID: 1603 RVA: 0x00018C0D File Offset: 0x00016E0D
			public int Count
			{
				get
				{
					if (this._token == null)
					{
						return 0;
					}
					return 1;
				}
			}

			// Token: 0x17000146 RID: 326
			// (get) Token: 0x06000644 RID: 1604 RVA: 0x00018C1A File Offset: 0x00016E1A
			public bool IsReadOnly
			{
				get
				{
					return false;
				}
			}

			// Token: 0x06000645 RID: 1605 RVA: 0x00018C1D File Offset: 0x00016E1D
			public int IndexOf(JToken item)
			{
				if (this._token != item)
				{
					return -1;
				}
				return 0;
			}

			// Token: 0x06000646 RID: 1606 RVA: 0x00018C2B File Offset: 0x00016E2B
			public void Insert(int index, JToken item)
			{
				if (index == 0)
				{
					this._token = item;
				}
			}

			// Token: 0x06000647 RID: 1607 RVA: 0x00018C37 File Offset: 0x00016E37
			public void RemoveAt(int index)
			{
				if (index == 0)
				{
					this._token = null;
				}
			}

			// Token: 0x17000147 RID: 327
			public JToken this[int index]
			{
				get
				{
					if (index != 0)
					{
						return null;
					}
					return this._token;
				}
				set
				{
					if (index == 0)
					{
						this._token = value;
					}
				}
			}

			// Token: 0x040001CF RID: 463
			internal JToken _token;
		}
	}
}
