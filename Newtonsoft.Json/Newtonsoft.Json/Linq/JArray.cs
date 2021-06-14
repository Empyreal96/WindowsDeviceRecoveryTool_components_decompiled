using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Linq
{
	// Token: 0x02000063 RID: 99
	public class JArray : JContainer, IList<JToken>, ICollection<JToken>, IEnumerable<JToken>, IEnumerable
	{
		// Token: 0x17000127 RID: 295
		// (get) Token: 0x0600056D RID: 1389 RVA: 0x00015226 File Offset: 0x00013426
		protected override IList<JToken> ChildrenTokens
		{
			get
			{
				return this._values;
			}
		}

		// Token: 0x17000128 RID: 296
		// (get) Token: 0x0600056E RID: 1390 RVA: 0x0001522E File Offset: 0x0001342E
		public override JTokenType Type
		{
			get
			{
				return JTokenType.Array;
			}
		}

		// Token: 0x0600056F RID: 1391 RVA: 0x00015231 File Offset: 0x00013431
		public JArray()
		{
		}

		// Token: 0x06000570 RID: 1392 RVA: 0x00015244 File Offset: 0x00013444
		public JArray(JArray other) : base(other)
		{
		}

		// Token: 0x06000571 RID: 1393 RVA: 0x00015258 File Offset: 0x00013458
		public JArray(params object[] content) : this(content)
		{
		}

		// Token: 0x06000572 RID: 1394 RVA: 0x00015261 File Offset: 0x00013461
		public JArray(object content)
		{
			this.Add(content);
		}

		// Token: 0x06000573 RID: 1395 RVA: 0x0001527C File Offset: 0x0001347C
		internal override bool DeepEquals(JToken node)
		{
			JArray jarray = node as JArray;
			return jarray != null && base.ContentsEqual(jarray);
		}

		// Token: 0x06000574 RID: 1396 RVA: 0x0001529C File Offset: 0x0001349C
		internal override JToken CloneToken()
		{
			return new JArray(this);
		}

		// Token: 0x06000575 RID: 1397 RVA: 0x000152A4 File Offset: 0x000134A4
		public new static JArray Load(JsonReader reader)
		{
			if (reader.TokenType == JsonToken.None && !reader.Read())
			{
				throw JsonReaderException.Create(reader, "Error reading JArray from JsonReader.");
			}
			while (reader.TokenType == JsonToken.Comment)
			{
				reader.Read();
			}
			if (reader.TokenType != JsonToken.StartArray)
			{
				throw JsonReaderException.Create(reader, "Error reading JArray from JsonReader. Current JsonReader item is not an array: {0}".FormatWith(CultureInfo.InvariantCulture, reader.TokenType));
			}
			JArray jarray = new JArray();
			jarray.SetLineInfo(reader as IJsonLineInfo);
			jarray.ReadTokenFrom(reader);
			return jarray;
		}

		// Token: 0x06000576 RID: 1398 RVA: 0x00015324 File Offset: 0x00013524
		public new static JArray Parse(string json)
		{
			JArray result;
			using (JsonReader jsonReader = new JsonTextReader(new StringReader(json)))
			{
				JArray jarray = JArray.Load(jsonReader);
				if (jsonReader.Read() && jsonReader.TokenType != JsonToken.Comment)
				{
					throw JsonReaderException.Create(jsonReader, "Additional text found in JSON string after parsing content.");
				}
				result = jarray;
			}
			return result;
		}

		// Token: 0x06000577 RID: 1399 RVA: 0x00015380 File Offset: 0x00013580
		public new static JArray FromObject(object o)
		{
			return JArray.FromObject(o, JsonSerializer.CreateDefault());
		}

		// Token: 0x06000578 RID: 1400 RVA: 0x00015390 File Offset: 0x00013590
		public new static JArray FromObject(object o, JsonSerializer jsonSerializer)
		{
			JToken jtoken = JToken.FromObjectInternal(o, jsonSerializer);
			if (jtoken.Type != JTokenType.Array)
			{
				throw new ArgumentException("Object serialized to {0}. JArray instance expected.".FormatWith(CultureInfo.InvariantCulture, jtoken.Type));
			}
			return (JArray)jtoken;
		}

		// Token: 0x06000579 RID: 1401 RVA: 0x000153D4 File Offset: 0x000135D4
		public override void WriteTo(JsonWriter writer, params JsonConverter[] converters)
		{
			writer.WriteStartArray();
			for (int i = 0; i < this._values.Count; i++)
			{
				this._values[i].WriteTo(writer, converters);
			}
			writer.WriteEndArray();
		}

		// Token: 0x17000129 RID: 297
		public override JToken this[object key]
		{
			get
			{
				ValidationUtils.ArgumentNotNull(key, "o");
				if (!(key is int))
				{
					throw new ArgumentException("Accessed JArray values with invalid key value: {0}. Array position index expected.".FormatWith(CultureInfo.InvariantCulture, MiscellaneousUtils.ToString(key)));
				}
				return this.GetItem((int)key);
			}
			set
			{
				ValidationUtils.ArgumentNotNull(key, "o");
				if (!(key is int))
				{
					throw new ArgumentException("Set JArray values with invalid key value: {0}. Array position index expected.".FormatWith(CultureInfo.InvariantCulture, MiscellaneousUtils.ToString(key)));
				}
				this.SetItem((int)key, value);
			}
		}

		// Token: 0x1700012A RID: 298
		public JToken this[int index]
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

		// Token: 0x0600057E RID: 1406 RVA: 0x000154A4 File Offset: 0x000136A4
		internal override void MergeItem(object content, JsonMergeSettings settings)
		{
			IEnumerable enumerable = (base.IsMultiContent(content) || content is JArray) ? ((IEnumerable)content) : null;
			if (enumerable == null)
			{
				return;
			}
			JContainer.MergeEnumerableContent(this, enumerable, settings);
		}

		// Token: 0x0600057F RID: 1407 RVA: 0x000154D8 File Offset: 0x000136D8
		public int IndexOf(JToken item)
		{
			return base.IndexOfItem(item);
		}

		// Token: 0x06000580 RID: 1408 RVA: 0x000154E1 File Offset: 0x000136E1
		public void Insert(int index, JToken item)
		{
			this.InsertItem(index, item, false);
		}

		// Token: 0x06000581 RID: 1409 RVA: 0x000154EC File Offset: 0x000136EC
		public void RemoveAt(int index)
		{
			this.RemoveItemAt(index);
		}

		// Token: 0x06000582 RID: 1410 RVA: 0x000154F8 File Offset: 0x000136F8
		public IEnumerator<JToken> GetEnumerator()
		{
			return this.Children().GetEnumerator();
		}

		// Token: 0x06000583 RID: 1411 RVA: 0x00015513 File Offset: 0x00013713
		public void Add(JToken item)
		{
			this.Add(item);
		}

		// Token: 0x06000584 RID: 1412 RVA: 0x0001551C File Offset: 0x0001371C
		public void Clear()
		{
			this.ClearItems();
		}

		// Token: 0x06000585 RID: 1413 RVA: 0x00015524 File Offset: 0x00013724
		public bool Contains(JToken item)
		{
			return this.ContainsItem(item);
		}

		// Token: 0x06000586 RID: 1414 RVA: 0x0001552D File Offset: 0x0001372D
		public void CopyTo(JToken[] array, int arrayIndex)
		{
			this.CopyItemsTo(array, arrayIndex);
		}

		// Token: 0x1700012B RID: 299
		// (get) Token: 0x06000587 RID: 1415 RVA: 0x00015537 File Offset: 0x00013737
		public bool IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06000588 RID: 1416 RVA: 0x0001553A File Offset: 0x0001373A
		public bool Remove(JToken item)
		{
			return this.RemoveItem(item);
		}

		// Token: 0x06000589 RID: 1417 RVA: 0x00015543 File Offset: 0x00013743
		internal override int GetDeepHashCode()
		{
			return base.ContentsHashCode();
		}

		// Token: 0x040001B7 RID: 439
		private readonly List<JToken> _values = new List<JToken>();
	}
}
