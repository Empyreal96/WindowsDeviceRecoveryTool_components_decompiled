using System;
using System.Collections.Generic;
using System.Globalization;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Linq
{
	// Token: 0x02000064 RID: 100
	public class JConstructor : JContainer
	{
		// Token: 0x1700012C RID: 300
		// (get) Token: 0x0600058A RID: 1418 RVA: 0x0001554B File Offset: 0x0001374B
		protected override IList<JToken> ChildrenTokens
		{
			get
			{
				return this._values;
			}
		}

		// Token: 0x0600058B RID: 1419 RVA: 0x00015554 File Offset: 0x00013754
		internal override void MergeItem(object content, JsonMergeSettings settings)
		{
			JConstructor jconstructor = content as JConstructor;
			if (jconstructor == null)
			{
				return;
			}
			if (jconstructor.Name != null)
			{
				this.Name = jconstructor.Name;
			}
			JContainer.MergeEnumerableContent(this, jconstructor, settings);
		}

		// Token: 0x1700012D RID: 301
		// (get) Token: 0x0600058C RID: 1420 RVA: 0x00015588 File Offset: 0x00013788
		// (set) Token: 0x0600058D RID: 1421 RVA: 0x00015590 File Offset: 0x00013790
		public string Name
		{
			get
			{
				return this._name;
			}
			set
			{
				this._name = value;
			}
		}

		// Token: 0x1700012E RID: 302
		// (get) Token: 0x0600058E RID: 1422 RVA: 0x00015599 File Offset: 0x00013799
		public override JTokenType Type
		{
			get
			{
				return JTokenType.Constructor;
			}
		}

		// Token: 0x0600058F RID: 1423 RVA: 0x0001559C File Offset: 0x0001379C
		public JConstructor()
		{
		}

		// Token: 0x06000590 RID: 1424 RVA: 0x000155AF File Offset: 0x000137AF
		public JConstructor(JConstructor other) : base(other)
		{
			this._name = other.Name;
		}

		// Token: 0x06000591 RID: 1425 RVA: 0x000155CF File Offset: 0x000137CF
		public JConstructor(string name, params object[] content) : this(name, content)
		{
		}

		// Token: 0x06000592 RID: 1426 RVA: 0x000155D9 File Offset: 0x000137D9
		public JConstructor(string name, object content) : this(name)
		{
			this.Add(content);
		}

		// Token: 0x06000593 RID: 1427 RVA: 0x000155E9 File Offset: 0x000137E9
		public JConstructor(string name)
		{
			ValidationUtils.ArgumentNotNullOrEmpty(name, "name");
			this._name = name;
		}

		// Token: 0x06000594 RID: 1428 RVA: 0x00015610 File Offset: 0x00013810
		internal override bool DeepEquals(JToken node)
		{
			JConstructor jconstructor = node as JConstructor;
			return jconstructor != null && this._name == jconstructor.Name && base.ContentsEqual(jconstructor);
		}

		// Token: 0x06000595 RID: 1429 RVA: 0x00015643 File Offset: 0x00013843
		internal override JToken CloneToken()
		{
			return new JConstructor(this);
		}

		// Token: 0x06000596 RID: 1430 RVA: 0x0001564C File Offset: 0x0001384C
		public override void WriteTo(JsonWriter writer, params JsonConverter[] converters)
		{
			writer.WriteStartConstructor(this._name);
			foreach (JToken jtoken in this.Children())
			{
				jtoken.WriteTo(writer, converters);
			}
			writer.WriteEndConstructor();
		}

		// Token: 0x1700012F RID: 303
		public override JToken this[object key]
		{
			get
			{
				ValidationUtils.ArgumentNotNull(key, "o");
				if (!(key is int))
				{
					throw new ArgumentException("Accessed JConstructor values with invalid key value: {0}. Argument position index expected.".FormatWith(CultureInfo.InvariantCulture, MiscellaneousUtils.ToString(key)));
				}
				return this.GetItem((int)key);
			}
			set
			{
				ValidationUtils.ArgumentNotNull(key, "o");
				if (!(key is int))
				{
					throw new ArgumentException("Set JConstructor values with invalid key value: {0}. Argument position index expected.".FormatWith(CultureInfo.InvariantCulture, MiscellaneousUtils.ToString(key)));
				}
				this.SetItem((int)key, value);
			}
		}

		// Token: 0x06000599 RID: 1433 RVA: 0x00015729 File Offset: 0x00013929
		internal override int GetDeepHashCode()
		{
			return this._name.GetHashCode() ^ base.ContentsHashCode();
		}

		// Token: 0x0600059A RID: 1434 RVA: 0x00015740 File Offset: 0x00013940
		public new static JConstructor Load(JsonReader reader)
		{
			if (reader.TokenType == JsonToken.None && !reader.Read())
			{
				throw JsonReaderException.Create(reader, "Error reading JConstructor from JsonReader.");
			}
			while (reader.TokenType == JsonToken.Comment)
			{
				reader.Read();
			}
			if (reader.TokenType != JsonToken.StartConstructor)
			{
				throw JsonReaderException.Create(reader, "Error reading JConstructor from JsonReader. Current JsonReader item is not a constructor: {0}".FormatWith(CultureInfo.InvariantCulture, reader.TokenType));
			}
			JConstructor jconstructor = new JConstructor((string)reader.Value);
			jconstructor.SetLineInfo(reader as IJsonLineInfo);
			jconstructor.ReadTokenFrom(reader);
			return jconstructor;
		}

		// Token: 0x040001B8 RID: 440
		private string _name;

		// Token: 0x040001B9 RID: 441
		private readonly List<JToken> _values = new List<JToken>();
	}
}
