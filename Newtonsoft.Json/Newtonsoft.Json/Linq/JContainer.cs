using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Threading;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Linq
{
	// Token: 0x02000061 RID: 97
	public abstract class JContainer : JToken, IList<JToken>, ICollection<JToken>, IEnumerable<JToken>, ITypedList, IBindingList, IList, ICollection, IEnumerable, INotifyCollectionChanged
	{
		// Token: 0x14000003 RID: 3
		// (add) Token: 0x06000509 RID: 1289 RVA: 0x00013F39 File Offset: 0x00012139
		// (remove) Token: 0x0600050A RID: 1290 RVA: 0x00013F52 File Offset: 0x00012152
		public event ListChangedEventHandler ListChanged
		{
			add
			{
				this._listChanged = (ListChangedEventHandler)Delegate.Combine(this._listChanged, value);
			}
			remove
			{
				this._listChanged = (ListChangedEventHandler)Delegate.Remove(this._listChanged, value);
			}
		}

		// Token: 0x14000004 RID: 4
		// (add) Token: 0x0600050B RID: 1291 RVA: 0x00013F6B File Offset: 0x0001216B
		// (remove) Token: 0x0600050C RID: 1292 RVA: 0x00013F84 File Offset: 0x00012184
		public event AddingNewEventHandler AddingNew
		{
			add
			{
				this._addingNew = (AddingNewEventHandler)Delegate.Combine(this._addingNew, value);
			}
			remove
			{
				this._addingNew = (AddingNewEventHandler)Delegate.Remove(this._addingNew, value);
			}
		}

		// Token: 0x14000005 RID: 5
		// (add) Token: 0x0600050D RID: 1293 RVA: 0x00013F9D File Offset: 0x0001219D
		// (remove) Token: 0x0600050E RID: 1294 RVA: 0x00013FB6 File Offset: 0x000121B6
		public event NotifyCollectionChangedEventHandler CollectionChanged
		{
			add
			{
				this._collectionChanged = (NotifyCollectionChangedEventHandler)Delegate.Combine(this._collectionChanged, value);
			}
			remove
			{
				this._collectionChanged = (NotifyCollectionChangedEventHandler)Delegate.Remove(this._collectionChanged, value);
			}
		}

		// Token: 0x17000112 RID: 274
		// (get) Token: 0x0600050F RID: 1295
		protected abstract IList<JToken> ChildrenTokens { get; }

		// Token: 0x06000510 RID: 1296 RVA: 0x00013FCF File Offset: 0x000121CF
		internal JContainer()
		{
		}

		// Token: 0x06000511 RID: 1297 RVA: 0x00013FD8 File Offset: 0x000121D8
		internal JContainer(JContainer other) : this()
		{
			ValidationUtils.ArgumentNotNull(other, "c");
			int num = 0;
			foreach (JToken content in ((IEnumerable<JToken>)other))
			{
				this.AddInternal(num, content, false);
				num++;
			}
		}

		// Token: 0x06000512 RID: 1298 RVA: 0x0001403C File Offset: 0x0001223C
		internal void CheckReentrancy()
		{
			if (this._busy)
			{
				throw new InvalidOperationException("Cannot change {0} during a collection change event.".FormatWith(CultureInfo.InvariantCulture, base.GetType()));
			}
		}

		// Token: 0x06000513 RID: 1299 RVA: 0x00014061 File Offset: 0x00012261
		internal virtual IList<JToken> CreateChildrenCollection()
		{
			return new List<JToken>();
		}

		// Token: 0x06000514 RID: 1300 RVA: 0x00014068 File Offset: 0x00012268
		protected virtual void OnAddingNew(AddingNewEventArgs e)
		{
			AddingNewEventHandler addingNew = this._addingNew;
			if (addingNew != null)
			{
				addingNew(this, e);
			}
		}

		// Token: 0x06000515 RID: 1301 RVA: 0x00014088 File Offset: 0x00012288
		protected virtual void OnListChanged(ListChangedEventArgs e)
		{
			ListChangedEventHandler listChanged = this._listChanged;
			if (listChanged != null)
			{
				this._busy = true;
				try
				{
					listChanged(this, e);
				}
				finally
				{
					this._busy = false;
				}
			}
		}

		// Token: 0x06000516 RID: 1302 RVA: 0x000140C8 File Offset: 0x000122C8
		protected virtual void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
		{
			NotifyCollectionChangedEventHandler collectionChanged = this._collectionChanged;
			if (collectionChanged != null)
			{
				this._busy = true;
				try
				{
					collectionChanged(this, e);
				}
				finally
				{
					this._busy = false;
				}
			}
		}

		// Token: 0x17000113 RID: 275
		// (get) Token: 0x06000517 RID: 1303 RVA: 0x00014108 File Offset: 0x00012308
		public override bool HasValues
		{
			get
			{
				return this.ChildrenTokens.Count > 0;
			}
		}

		// Token: 0x06000518 RID: 1304 RVA: 0x00014118 File Offset: 0x00012318
		internal bool ContentsEqual(JContainer container)
		{
			if (container == this)
			{
				return true;
			}
			IList<JToken> childrenTokens = this.ChildrenTokens;
			IList<JToken> childrenTokens2 = container.ChildrenTokens;
			if (childrenTokens.Count != childrenTokens2.Count)
			{
				return false;
			}
			for (int i = 0; i < childrenTokens.Count; i++)
			{
				if (!childrenTokens[i].DeepEquals(childrenTokens2[i]))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x17000114 RID: 276
		// (get) Token: 0x06000519 RID: 1305 RVA: 0x00014172 File Offset: 0x00012372
		public override JToken First
		{
			get
			{
				return this.ChildrenTokens.FirstOrDefault<JToken>();
			}
		}

		// Token: 0x17000115 RID: 277
		// (get) Token: 0x0600051A RID: 1306 RVA: 0x0001417F File Offset: 0x0001237F
		public override JToken Last
		{
			get
			{
				return this.ChildrenTokens.LastOrDefault<JToken>();
			}
		}

		// Token: 0x0600051B RID: 1307 RVA: 0x0001418C File Offset: 0x0001238C
		public override JEnumerable<JToken> Children()
		{
			return new JEnumerable<JToken>(this.ChildrenTokens);
		}

		// Token: 0x0600051C RID: 1308 RVA: 0x00014199 File Offset: 0x00012399
		public override IEnumerable<T> Values<T>()
		{
			return this.ChildrenTokens.Convert<JToken, T>();
		}

		// Token: 0x0600051D RID: 1309 RVA: 0x000141A6 File Offset: 0x000123A6
		public IEnumerable<JToken> Descendants()
		{
			return this.GetDescendants(false);
		}

		// Token: 0x0600051E RID: 1310 RVA: 0x000141AF File Offset: 0x000123AF
		public IEnumerable<JToken> DescendantsAndSelf()
		{
			return this.GetDescendants(true);
		}

		// Token: 0x0600051F RID: 1311 RVA: 0x00014464 File Offset: 0x00012664
		internal IEnumerable<JToken> GetDescendants(bool self)
		{
			if (self)
			{
				yield return this;
			}
			foreach (JToken o in this.ChildrenTokens)
			{
				yield return o;
				JContainer c = o as JContainer;
				if (c != null)
				{
					foreach (JToken d in c.Descendants())
					{
						yield return d;
					}
				}
			}
			yield break;
		}

		// Token: 0x06000520 RID: 1312 RVA: 0x00014488 File Offset: 0x00012688
		internal bool IsMultiContent(object content)
		{
			return content is IEnumerable && !(content is string) && !(content is JToken) && !(content is byte[]);
		}

		// Token: 0x06000521 RID: 1313 RVA: 0x000144B0 File Offset: 0x000126B0
		internal JToken EnsureParentToken(JToken item, bool skipParentCheck)
		{
			if (item == null)
			{
				return JValue.CreateNull();
			}
			if (skipParentCheck)
			{
				return item;
			}
			if (item.Parent != null || item == this || (item.HasValues && base.Root == item))
			{
				item = item.CloneToken();
			}
			return item;
		}

		// Token: 0x06000522 RID: 1314 RVA: 0x000144E6 File Offset: 0x000126E6
		internal int IndexOfItem(JToken item)
		{
			return this.ChildrenTokens.IndexOf(item, JContainer.JTokenReferenceEqualityComparer.Instance);
		}

		// Token: 0x06000523 RID: 1315 RVA: 0x000144FC File Offset: 0x000126FC
		internal virtual void InsertItem(int index, JToken item, bool skipParentCheck)
		{
			if (index > this.ChildrenTokens.Count)
			{
				throw new ArgumentOutOfRangeException("index", "Index must be within the bounds of the List.");
			}
			this.CheckReentrancy();
			item = this.EnsureParentToken(item, skipParentCheck);
			JToken jtoken = (index == 0) ? null : this.ChildrenTokens[index - 1];
			JToken jtoken2 = (index == this.ChildrenTokens.Count) ? null : this.ChildrenTokens[index];
			this.ValidateToken(item, null);
			item.Parent = this;
			item.Previous = jtoken;
			if (jtoken != null)
			{
				jtoken.Next = item;
			}
			item.Next = jtoken2;
			if (jtoken2 != null)
			{
				jtoken2.Previous = item;
			}
			this.ChildrenTokens.Insert(index, item);
			if (this._listChanged != null)
			{
				this.OnListChanged(new ListChangedEventArgs(ListChangedType.ItemAdded, index));
			}
			if (this._collectionChanged != null)
			{
				this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item, index));
			}
		}

		// Token: 0x06000524 RID: 1316 RVA: 0x000145D4 File Offset: 0x000127D4
		internal virtual void RemoveItemAt(int index)
		{
			if (index < 0)
			{
				throw new ArgumentOutOfRangeException("index", "Index is less than 0.");
			}
			if (index >= this.ChildrenTokens.Count)
			{
				throw new ArgumentOutOfRangeException("index", "Index is equal to or greater than Count.");
			}
			this.CheckReentrancy();
			JToken jtoken = this.ChildrenTokens[index];
			JToken jtoken2 = (index == 0) ? null : this.ChildrenTokens[index - 1];
			JToken jtoken3 = (index == this.ChildrenTokens.Count - 1) ? null : this.ChildrenTokens[index + 1];
			if (jtoken2 != null)
			{
				jtoken2.Next = jtoken3;
			}
			if (jtoken3 != null)
			{
				jtoken3.Previous = jtoken2;
			}
			jtoken.Parent = null;
			jtoken.Previous = null;
			jtoken.Next = null;
			this.ChildrenTokens.RemoveAt(index);
			if (this._listChanged != null)
			{
				this.OnListChanged(new ListChangedEventArgs(ListChangedType.ItemDeleted, index));
			}
			if (this._collectionChanged != null)
			{
				this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, jtoken, index));
			}
		}

		// Token: 0x06000525 RID: 1317 RVA: 0x000146C0 File Offset: 0x000128C0
		internal virtual bool RemoveItem(JToken item)
		{
			int num = this.IndexOfItem(item);
			if (num >= 0)
			{
				this.RemoveItemAt(num);
				return true;
			}
			return false;
		}

		// Token: 0x06000526 RID: 1318 RVA: 0x000146E3 File Offset: 0x000128E3
		internal virtual JToken GetItem(int index)
		{
			return this.ChildrenTokens[index];
		}

		// Token: 0x06000527 RID: 1319 RVA: 0x000146F4 File Offset: 0x000128F4
		internal virtual void SetItem(int index, JToken item)
		{
			if (index < 0)
			{
				throw new ArgumentOutOfRangeException("index", "Index is less than 0.");
			}
			if (index >= this.ChildrenTokens.Count)
			{
				throw new ArgumentOutOfRangeException("index", "Index is equal to or greater than Count.");
			}
			JToken jtoken = this.ChildrenTokens[index];
			if (JContainer.IsTokenUnchanged(jtoken, item))
			{
				return;
			}
			this.CheckReentrancy();
			item = this.EnsureParentToken(item, false);
			this.ValidateToken(item, jtoken);
			JToken jtoken2 = (index == 0) ? null : this.ChildrenTokens[index - 1];
			JToken jtoken3 = (index == this.ChildrenTokens.Count - 1) ? null : this.ChildrenTokens[index + 1];
			item.Parent = this;
			item.Previous = jtoken2;
			if (jtoken2 != null)
			{
				jtoken2.Next = item;
			}
			item.Next = jtoken3;
			if (jtoken3 != null)
			{
				jtoken3.Previous = item;
			}
			this.ChildrenTokens[index] = item;
			jtoken.Parent = null;
			jtoken.Previous = null;
			jtoken.Next = null;
			if (this._listChanged != null)
			{
				this.OnListChanged(new ListChangedEventArgs(ListChangedType.ItemChanged, index));
			}
			if (this._collectionChanged != null)
			{
				this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, item, jtoken, index));
			}
		}

		// Token: 0x06000528 RID: 1320 RVA: 0x00014810 File Offset: 0x00012A10
		internal virtual void ClearItems()
		{
			this.CheckReentrancy();
			foreach (JToken jtoken in this.ChildrenTokens)
			{
				jtoken.Parent = null;
				jtoken.Previous = null;
				jtoken.Next = null;
			}
			this.ChildrenTokens.Clear();
			if (this._listChanged != null)
			{
				this.OnListChanged(new ListChangedEventArgs(ListChangedType.Reset, -1));
			}
			if (this._collectionChanged != null)
			{
				this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
			}
		}

		// Token: 0x06000529 RID: 1321 RVA: 0x000148A8 File Offset: 0x00012AA8
		internal virtual void ReplaceItem(JToken existing, JToken replacement)
		{
			if (existing == null || existing.Parent != this)
			{
				return;
			}
			int index = this.IndexOfItem(existing);
			this.SetItem(index, replacement);
		}

		// Token: 0x0600052A RID: 1322 RVA: 0x000148D2 File Offset: 0x00012AD2
		internal virtual bool ContainsItem(JToken item)
		{
			return this.IndexOfItem(item) != -1;
		}

		// Token: 0x0600052B RID: 1323 RVA: 0x000148E4 File Offset: 0x00012AE4
		internal virtual void CopyItemsTo(Array array, int arrayIndex)
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
			if (this.Count > array.Length - arrayIndex)
			{
				throw new ArgumentException("The number of elements in the source JObject is greater than the available space from arrayIndex to the end of the destination array.");
			}
			int num = 0;
			foreach (JToken value in this.ChildrenTokens)
			{
				array.SetValue(value, arrayIndex + num);
				num++;
			}
		}

		// Token: 0x0600052C RID: 1324 RVA: 0x00014990 File Offset: 0x00012B90
		internal static bool IsTokenUnchanged(JToken currentValue, JToken newValue)
		{
			JValue jvalue = currentValue as JValue;
			return jvalue != null && ((jvalue.Type == JTokenType.Null && newValue == null) || jvalue.Equals(newValue));
		}

		// Token: 0x0600052D RID: 1325 RVA: 0x000149BF File Offset: 0x00012BBF
		internal virtual void ValidateToken(JToken o, JToken existing)
		{
			ValidationUtils.ArgumentNotNull(o, "o");
			if (o.Type == JTokenType.Property)
			{
				throw new ArgumentException("Can not add {0} to {1}.".FormatWith(CultureInfo.InvariantCulture, o.GetType(), base.GetType()));
			}
		}

		// Token: 0x0600052E RID: 1326 RVA: 0x000149F6 File Offset: 0x00012BF6
		public virtual void Add(object content)
		{
			this.AddInternal(this.ChildrenTokens.Count, content, false);
		}

		// Token: 0x0600052F RID: 1327 RVA: 0x00014A0B File Offset: 0x00012C0B
		internal void AddAndSkipParentCheck(JToken token)
		{
			this.AddInternal(this.ChildrenTokens.Count, token, true);
		}

		// Token: 0x06000530 RID: 1328 RVA: 0x00014A20 File Offset: 0x00012C20
		public void AddFirst(object content)
		{
			this.AddInternal(0, content, false);
		}

		// Token: 0x06000531 RID: 1329 RVA: 0x00014A2C File Offset: 0x00012C2C
		internal void AddInternal(int index, object content, bool skipParentCheck)
		{
			if (this.IsMultiContent(content))
			{
				IEnumerable enumerable = (IEnumerable)content;
				int num = index;
				using (IEnumerator enumerator = enumerable.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						object content2 = enumerator.Current;
						this.AddInternal(num, content2, skipParentCheck);
						num++;
					}
					return;
				}
			}
			JToken item = JContainer.CreateFromContent(content);
			this.InsertItem(index, item, skipParentCheck);
		}

		// Token: 0x06000532 RID: 1330 RVA: 0x00014AAC File Offset: 0x00012CAC
		internal static JToken CreateFromContent(object content)
		{
			if (content is JToken)
			{
				return (JToken)content;
			}
			return new JValue(content);
		}

		// Token: 0x06000533 RID: 1331 RVA: 0x00014AC3 File Offset: 0x00012CC3
		public JsonWriter CreateWriter()
		{
			return new JTokenWriter(this);
		}

		// Token: 0x06000534 RID: 1332 RVA: 0x00014ACB File Offset: 0x00012CCB
		public void ReplaceAll(object content)
		{
			this.ClearItems();
			this.Add(content);
		}

		// Token: 0x06000535 RID: 1333 RVA: 0x00014ADA File Offset: 0x00012CDA
		public void RemoveAll()
		{
			this.ClearItems();
		}

		// Token: 0x06000536 RID: 1334
		internal abstract void MergeItem(object content, JsonMergeSettings settings);

		// Token: 0x06000537 RID: 1335 RVA: 0x00014AE2 File Offset: 0x00012CE2
		public void Merge(object content)
		{
			this.MergeItem(content, new JsonMergeSettings());
		}

		// Token: 0x06000538 RID: 1336 RVA: 0x00014AF0 File Offset: 0x00012CF0
		public void Merge(object content, JsonMergeSettings settings)
		{
			this.MergeItem(content, settings);
		}

		// Token: 0x06000539 RID: 1337 RVA: 0x00014AFC File Offset: 0x00012CFC
		internal void ReadTokenFrom(JsonReader reader)
		{
			int depth = reader.Depth;
			if (!reader.Read())
			{
				throw JsonReaderException.Create(reader, "Error reading {0} from JsonReader.".FormatWith(CultureInfo.InvariantCulture, base.GetType().Name));
			}
			this.ReadContentFrom(reader);
			int depth2 = reader.Depth;
			if (depth2 > depth)
			{
				throw JsonReaderException.Create(reader, "Unexpected end of content while loading {0}.".FormatWith(CultureInfo.InvariantCulture, base.GetType().Name));
			}
		}

		// Token: 0x0600053A RID: 1338 RVA: 0x00014B6C File Offset: 0x00012D6C
		internal void ReadContentFrom(JsonReader r)
		{
			ValidationUtils.ArgumentNotNull(r, "r");
			IJsonLineInfo lineInfo = r as IJsonLineInfo;
			JContainer jcontainer = this;
			for (;;)
			{
				if (jcontainer is JProperty && ((JProperty)jcontainer).Value != null)
				{
					if (jcontainer == this)
					{
						break;
					}
					jcontainer = jcontainer.Parent;
				}
				switch (r.TokenType)
				{
				case JsonToken.None:
					goto IL_20F;
				case JsonToken.StartObject:
				{
					JObject jobject = new JObject();
					jobject.SetLineInfo(lineInfo);
					jcontainer.Add(jobject);
					jcontainer = jobject;
					goto IL_20F;
				}
				case JsonToken.StartArray:
				{
					JArray jarray = new JArray();
					jarray.SetLineInfo(lineInfo);
					jcontainer.Add(jarray);
					jcontainer = jarray;
					goto IL_20F;
				}
				case JsonToken.StartConstructor:
				{
					JConstructor jconstructor = new JConstructor(r.Value.ToString());
					jconstructor.SetLineInfo(lineInfo);
					jcontainer.Add(jconstructor);
					jcontainer = jconstructor;
					goto IL_20F;
				}
				case JsonToken.PropertyName:
				{
					string name = r.Value.ToString();
					JProperty jproperty = new JProperty(name);
					jproperty.SetLineInfo(lineInfo);
					JObject jobject2 = (JObject)jcontainer;
					JProperty jproperty2 = jobject2.Property(name);
					if (jproperty2 == null)
					{
						jcontainer.Add(jproperty);
					}
					else
					{
						jproperty2.Replace(jproperty);
					}
					jcontainer = jproperty;
					goto IL_20F;
				}
				case JsonToken.Comment:
				{
					JValue jvalue = JValue.CreateComment(r.Value.ToString());
					jvalue.SetLineInfo(lineInfo);
					jcontainer.Add(jvalue);
					goto IL_20F;
				}
				case JsonToken.Integer:
				case JsonToken.Float:
				case JsonToken.String:
				case JsonToken.Boolean:
				case JsonToken.Date:
				case JsonToken.Bytes:
				{
					JValue jvalue = new JValue(r.Value);
					jvalue.SetLineInfo(lineInfo);
					jcontainer.Add(jvalue);
					goto IL_20F;
				}
				case JsonToken.Null:
				{
					JValue jvalue = JValue.CreateNull();
					jvalue.SetLineInfo(lineInfo);
					jcontainer.Add(jvalue);
					goto IL_20F;
				}
				case JsonToken.Undefined:
				{
					JValue jvalue = JValue.CreateUndefined();
					jvalue.SetLineInfo(lineInfo);
					jcontainer.Add(jvalue);
					goto IL_20F;
				}
				case JsonToken.EndObject:
					if (jcontainer == this)
					{
						return;
					}
					jcontainer = jcontainer.Parent;
					goto IL_20F;
				case JsonToken.EndArray:
					if (jcontainer == this)
					{
						return;
					}
					jcontainer = jcontainer.Parent;
					goto IL_20F;
				case JsonToken.EndConstructor:
					if (jcontainer == this)
					{
						return;
					}
					jcontainer = jcontainer.Parent;
					goto IL_20F;
				}
				goto Block_4;
				IL_20F:
				if (!r.Read())
				{
					return;
				}
			}
			return;
			Block_4:
			throw new InvalidOperationException("The JsonReader should not be on a token of type {0}.".FormatWith(CultureInfo.InvariantCulture, r.TokenType));
		}

		// Token: 0x0600053B RID: 1339 RVA: 0x00014D94 File Offset: 0x00012F94
		internal int ContentsHashCode()
		{
			int num = 0;
			foreach (JToken jtoken in this.ChildrenTokens)
			{
				num ^= jtoken.GetDeepHashCode();
			}
			return num;
		}

		// Token: 0x0600053C RID: 1340 RVA: 0x00014DE8 File Offset: 0x00012FE8
		string ITypedList.GetListName(PropertyDescriptor[] listAccessors)
		{
			return string.Empty;
		}

		// Token: 0x0600053D RID: 1341 RVA: 0x00014DF0 File Offset: 0x00012FF0
		PropertyDescriptorCollection ITypedList.GetItemProperties(PropertyDescriptor[] listAccessors)
		{
			ICustomTypeDescriptor customTypeDescriptor = this.First as ICustomTypeDescriptor;
			if (customTypeDescriptor != null)
			{
				return customTypeDescriptor.GetProperties();
			}
			return null;
		}

		// Token: 0x0600053E RID: 1342 RVA: 0x00014E14 File Offset: 0x00013014
		int IList<JToken>.IndexOf(JToken item)
		{
			return this.IndexOfItem(item);
		}

		// Token: 0x0600053F RID: 1343 RVA: 0x00014E1D File Offset: 0x0001301D
		void IList<JToken>.Insert(int index, JToken item)
		{
			this.InsertItem(index, item, false);
		}

		// Token: 0x06000540 RID: 1344 RVA: 0x00014E28 File Offset: 0x00013028
		void IList<JToken>.RemoveAt(int index)
		{
			this.RemoveItemAt(index);
		}

		// Token: 0x17000116 RID: 278
		JToken IList<JToken>.this[int index]
		{
			get
			{
				return this.GetItem(index);
			}
			set
			{
				this.SetItem(index, value);
			}
		}

		// Token: 0x06000543 RID: 1347 RVA: 0x00014E44 File Offset: 0x00013044
		void ICollection<JToken>.Add(JToken item)
		{
			this.Add(item);
		}

		// Token: 0x06000544 RID: 1348 RVA: 0x00014E4D File Offset: 0x0001304D
		void ICollection<JToken>.Clear()
		{
			this.ClearItems();
		}

		// Token: 0x06000545 RID: 1349 RVA: 0x00014E55 File Offset: 0x00013055
		bool ICollection<JToken>.Contains(JToken item)
		{
			return this.ContainsItem(item);
		}

		// Token: 0x06000546 RID: 1350 RVA: 0x00014E5E File Offset: 0x0001305E
		void ICollection<JToken>.CopyTo(JToken[] array, int arrayIndex)
		{
			this.CopyItemsTo(array, arrayIndex);
		}

		// Token: 0x17000117 RID: 279
		// (get) Token: 0x06000547 RID: 1351 RVA: 0x00014E68 File Offset: 0x00013068
		bool ICollection<JToken>.IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06000548 RID: 1352 RVA: 0x00014E6B File Offset: 0x0001306B
		bool ICollection<JToken>.Remove(JToken item)
		{
			return this.RemoveItem(item);
		}

		// Token: 0x06000549 RID: 1353 RVA: 0x00014E74 File Offset: 0x00013074
		private JToken EnsureValue(object value)
		{
			if (value == null)
			{
				return null;
			}
			if (value is JToken)
			{
				return (JToken)value;
			}
			throw new ArgumentException("Argument is not a JToken.");
		}

		// Token: 0x0600054A RID: 1354 RVA: 0x00014E94 File Offset: 0x00013094
		int IList.Add(object value)
		{
			this.Add(this.EnsureValue(value));
			return this.Count - 1;
		}

		// Token: 0x0600054B RID: 1355 RVA: 0x00014EAB File Offset: 0x000130AB
		void IList.Clear()
		{
			this.ClearItems();
		}

		// Token: 0x0600054C RID: 1356 RVA: 0x00014EB3 File Offset: 0x000130B3
		bool IList.Contains(object value)
		{
			return this.ContainsItem(this.EnsureValue(value));
		}

		// Token: 0x0600054D RID: 1357 RVA: 0x00014EC2 File Offset: 0x000130C2
		int IList.IndexOf(object value)
		{
			return this.IndexOfItem(this.EnsureValue(value));
		}

		// Token: 0x0600054E RID: 1358 RVA: 0x00014ED1 File Offset: 0x000130D1
		void IList.Insert(int index, object value)
		{
			this.InsertItem(index, this.EnsureValue(value), false);
		}

		// Token: 0x17000118 RID: 280
		// (get) Token: 0x0600054F RID: 1359 RVA: 0x00014EE2 File Offset: 0x000130E2
		bool IList.IsFixedSize
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000119 RID: 281
		// (get) Token: 0x06000550 RID: 1360 RVA: 0x00014EE5 File Offset: 0x000130E5
		bool IList.IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06000551 RID: 1361 RVA: 0x00014EE8 File Offset: 0x000130E8
		void IList.Remove(object value)
		{
			this.RemoveItem(this.EnsureValue(value));
		}

		// Token: 0x06000552 RID: 1362 RVA: 0x00014EF8 File Offset: 0x000130F8
		void IList.RemoveAt(int index)
		{
			this.RemoveItemAt(index);
		}

		// Token: 0x1700011A RID: 282
		object IList.this[int index]
		{
			get
			{
				return this.GetItem(index);
			}
			set
			{
				this.SetItem(index, this.EnsureValue(value));
			}
		}

		// Token: 0x06000555 RID: 1365 RVA: 0x00014F1A File Offset: 0x0001311A
		void ICollection.CopyTo(Array array, int index)
		{
			this.CopyItemsTo(array, index);
		}

		// Token: 0x1700011B RID: 283
		// (get) Token: 0x06000556 RID: 1366 RVA: 0x00014F24 File Offset: 0x00013124
		public int Count
		{
			get
			{
				return this.ChildrenTokens.Count;
			}
		}

		// Token: 0x1700011C RID: 284
		// (get) Token: 0x06000557 RID: 1367 RVA: 0x00014F31 File Offset: 0x00013131
		bool ICollection.IsSynchronized
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700011D RID: 285
		// (get) Token: 0x06000558 RID: 1368 RVA: 0x00014F34 File Offset: 0x00013134
		object ICollection.SyncRoot
		{
			get
			{
				if (this._syncRoot == null)
				{
					Interlocked.CompareExchange(ref this._syncRoot, new object(), null);
				}
				return this._syncRoot;
			}
		}

		// Token: 0x06000559 RID: 1369 RVA: 0x00014F56 File Offset: 0x00013156
		void IBindingList.AddIndex(PropertyDescriptor property)
		{
		}

		// Token: 0x0600055A RID: 1370 RVA: 0x00014F58 File Offset: 0x00013158
		object IBindingList.AddNew()
		{
			AddingNewEventArgs addingNewEventArgs = new AddingNewEventArgs();
			this.OnAddingNew(addingNewEventArgs);
			if (addingNewEventArgs.NewObject == null)
			{
				throw new JsonException("Could not determine new value to add to '{0}'.".FormatWith(CultureInfo.InvariantCulture, base.GetType()));
			}
			if (!(addingNewEventArgs.NewObject is JToken))
			{
				throw new JsonException("New item to be added to collection must be compatible with {0}.".FormatWith(CultureInfo.InvariantCulture, typeof(JToken)));
			}
			JToken jtoken = (JToken)addingNewEventArgs.NewObject;
			this.Add(jtoken);
			return jtoken;
		}

		// Token: 0x1700011E RID: 286
		// (get) Token: 0x0600055B RID: 1371 RVA: 0x00014FD5 File Offset: 0x000131D5
		bool IBindingList.AllowEdit
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700011F RID: 287
		// (get) Token: 0x0600055C RID: 1372 RVA: 0x00014FD8 File Offset: 0x000131D8
		bool IBindingList.AllowNew
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000120 RID: 288
		// (get) Token: 0x0600055D RID: 1373 RVA: 0x00014FDB File Offset: 0x000131DB
		bool IBindingList.AllowRemove
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600055E RID: 1374 RVA: 0x00014FDE File Offset: 0x000131DE
		void IBindingList.ApplySort(PropertyDescriptor property, ListSortDirection direction)
		{
			throw new NotSupportedException();
		}

		// Token: 0x0600055F RID: 1375 RVA: 0x00014FE5 File Offset: 0x000131E5
		int IBindingList.Find(PropertyDescriptor property, object key)
		{
			throw new NotSupportedException();
		}

		// Token: 0x17000121 RID: 289
		// (get) Token: 0x06000560 RID: 1376 RVA: 0x00014FEC File Offset: 0x000131EC
		bool IBindingList.IsSorted
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06000561 RID: 1377 RVA: 0x00014FEF File Offset: 0x000131EF
		void IBindingList.RemoveIndex(PropertyDescriptor property)
		{
		}

		// Token: 0x06000562 RID: 1378 RVA: 0x00014FF1 File Offset: 0x000131F1
		void IBindingList.RemoveSort()
		{
			throw new NotSupportedException();
		}

		// Token: 0x17000122 RID: 290
		// (get) Token: 0x06000563 RID: 1379 RVA: 0x00014FF8 File Offset: 0x000131F8
		ListSortDirection IBindingList.SortDirection
		{
			get
			{
				return ListSortDirection.Ascending;
			}
		}

		// Token: 0x17000123 RID: 291
		// (get) Token: 0x06000564 RID: 1380 RVA: 0x00014FFB File Offset: 0x000131FB
		PropertyDescriptor IBindingList.SortProperty
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000124 RID: 292
		// (get) Token: 0x06000565 RID: 1381 RVA: 0x00014FFE File Offset: 0x000131FE
		bool IBindingList.SupportsChangeNotification
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000125 RID: 293
		// (get) Token: 0x06000566 RID: 1382 RVA: 0x00015001 File Offset: 0x00013201
		bool IBindingList.SupportsSearching
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000126 RID: 294
		// (get) Token: 0x06000567 RID: 1383 RVA: 0x00015004 File Offset: 0x00013204
		bool IBindingList.SupportsSorting
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06000568 RID: 1384 RVA: 0x00015008 File Offset: 0x00013208
		internal static void MergeEnumerableContent(JContainer target, IEnumerable content, JsonMergeSettings settings)
		{
			switch (settings.MergeArrayHandling)
			{
			case MergeArrayHandling.Concat:
				using (IEnumerator enumerator = content.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						object obj = enumerator.Current;
						JToken content2 = (JToken)obj;
						target.Add(content2);
					}
					return;
				}
				break;
			case MergeArrayHandling.Union:
				break;
			case MergeArrayHandling.Replace:
				goto IL_BB;
			case MergeArrayHandling.Merge:
				goto IL_102;
			default:
				goto IL_1A1;
			}
			HashSet<JToken> hashSet = new HashSet<JToken>(target, JToken.EqualityComparer);
			using (IEnumerator enumerator2 = content.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					object obj2 = enumerator2.Current;
					JToken jtoken = (JToken)obj2;
					if (hashSet.Add(jtoken))
					{
						target.Add(jtoken);
					}
				}
				return;
			}
			IL_BB:
			target.ClearItems();
			using (IEnumerator enumerator3 = content.GetEnumerator())
			{
				while (enumerator3.MoveNext())
				{
					object obj3 = enumerator3.Current;
					JToken content3 = (JToken)obj3;
					target.Add(content3);
				}
				return;
			}
			IL_102:
			int num = 0;
			using (IEnumerator enumerator4 = content.GetEnumerator())
			{
				while (enumerator4.MoveNext())
				{
					object obj4 = enumerator4.Current;
					if (num < target.Count)
					{
						JToken jtoken2 = target[num];
						JContainer jcontainer = jtoken2 as JContainer;
						if (jcontainer != null)
						{
							jcontainer.Merge(obj4, settings);
						}
						else if (obj4 != null)
						{
							JToken jtoken3 = JContainer.CreateFromContent(obj4);
							if (jtoken3.Type != JTokenType.Null)
							{
								target[num] = jtoken3;
							}
						}
					}
					else
					{
						target.Add(obj4);
					}
					num++;
				}
				return;
			}
			IL_1A1:
			throw new ArgumentOutOfRangeException("settings", "Unexpected merge array handling when merging JSON.");
		}

		// Token: 0x040001B1 RID: 433
		internal ListChangedEventHandler _listChanged;

		// Token: 0x040001B2 RID: 434
		internal AddingNewEventHandler _addingNew;

		// Token: 0x040001B3 RID: 435
		internal NotifyCollectionChangedEventHandler _collectionChanged;

		// Token: 0x040001B4 RID: 436
		private object _syncRoot;

		// Token: 0x040001B5 RID: 437
		private bool _busy;

		// Token: 0x02000062 RID: 98
		private class JTokenReferenceEqualityComparer : IEqualityComparer<JToken>
		{
			// Token: 0x06000569 RID: 1385 RVA: 0x000151FC File Offset: 0x000133FC
			public bool Equals(JToken x, JToken y)
			{
				return object.ReferenceEquals(x, y);
			}

			// Token: 0x0600056A RID: 1386 RVA: 0x00015205 File Offset: 0x00013405
			public int GetHashCode(JToken obj)
			{
				if (obj == null)
				{
					return 0;
				}
				return obj.GetHashCode();
			}

			// Token: 0x040001B6 RID: 438
			public static readonly JContainer.JTokenReferenceEqualityComparer Instance = new JContainer.JTokenReferenceEqualityComparer();
		}
	}
}
