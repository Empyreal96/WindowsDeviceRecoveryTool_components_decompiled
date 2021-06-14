using System;
using System.Globalization;
using System.IO;
using System.Numerics;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Bson
{
	// Token: 0x02000015 RID: 21
	public class BsonWriter : JsonWriter
	{
		// Token: 0x17000030 RID: 48
		// (get) Token: 0x0600010A RID: 266 RVA: 0x00005C29 File Offset: 0x00003E29
		// (set) Token: 0x0600010B RID: 267 RVA: 0x00005C36 File Offset: 0x00003E36
		public DateTimeKind DateTimeKindHandling
		{
			get
			{
				return this._writer.DateTimeKindHandling;
			}
			set
			{
				this._writer.DateTimeKindHandling = value;
			}
		}

		// Token: 0x0600010C RID: 268 RVA: 0x00005C44 File Offset: 0x00003E44
		public BsonWriter(Stream stream)
		{
			ValidationUtils.ArgumentNotNull(stream, "stream");
			this._writer = new BsonBinaryWriter(new BinaryWriter(stream));
		}

		// Token: 0x0600010D RID: 269 RVA: 0x00005C68 File Offset: 0x00003E68
		public BsonWriter(BinaryWriter writer)
		{
			ValidationUtils.ArgumentNotNull(writer, "writer");
			this._writer = new BsonBinaryWriter(writer);
		}

		// Token: 0x0600010E RID: 270 RVA: 0x00005C87 File Offset: 0x00003E87
		public override void Flush()
		{
			this._writer.Flush();
		}

		// Token: 0x0600010F RID: 271 RVA: 0x00005C94 File Offset: 0x00003E94
		protected override void WriteEnd(JsonToken token)
		{
			base.WriteEnd(token);
			this.RemoveParent();
			if (base.Top == 0)
			{
				this._writer.WriteToken(this._root);
			}
		}

		// Token: 0x06000110 RID: 272 RVA: 0x00005CBC File Offset: 0x00003EBC
		public override void WriteComment(string text)
		{
			throw JsonWriterException.Create(this, "Cannot write JSON comment as BSON.", null);
		}

		// Token: 0x06000111 RID: 273 RVA: 0x00005CCA File Offset: 0x00003ECA
		public override void WriteStartConstructor(string name)
		{
			throw JsonWriterException.Create(this, "Cannot write JSON constructor as BSON.", null);
		}

		// Token: 0x06000112 RID: 274 RVA: 0x00005CD8 File Offset: 0x00003ED8
		public override void WriteRaw(string json)
		{
			throw JsonWriterException.Create(this, "Cannot write raw JSON as BSON.", null);
		}

		// Token: 0x06000113 RID: 275 RVA: 0x00005CE6 File Offset: 0x00003EE6
		public override void WriteRawValue(string json)
		{
			throw JsonWriterException.Create(this, "Cannot write raw JSON as BSON.", null);
		}

		// Token: 0x06000114 RID: 276 RVA: 0x00005CF4 File Offset: 0x00003EF4
		public override void WriteStartArray()
		{
			base.WriteStartArray();
			this.AddParent(new BsonArray());
		}

		// Token: 0x06000115 RID: 277 RVA: 0x00005D07 File Offset: 0x00003F07
		public override void WriteStartObject()
		{
			base.WriteStartObject();
			this.AddParent(new BsonObject());
		}

		// Token: 0x06000116 RID: 278 RVA: 0x00005D1A File Offset: 0x00003F1A
		public override void WritePropertyName(string name)
		{
			base.WritePropertyName(name);
			this._propertyName = name;
		}

		// Token: 0x06000117 RID: 279 RVA: 0x00005D2A File Offset: 0x00003F2A
		public override void Close()
		{
			base.Close();
			if (base.CloseOutput && this._writer != null)
			{
				this._writer.Close();
			}
		}

		// Token: 0x06000118 RID: 280 RVA: 0x00005D4D File Offset: 0x00003F4D
		private void AddParent(BsonToken container)
		{
			this.AddToken(container);
			this._parent = container;
		}

		// Token: 0x06000119 RID: 281 RVA: 0x00005D5D File Offset: 0x00003F5D
		private void RemoveParent()
		{
			this._parent = this._parent.Parent;
		}

		// Token: 0x0600011A RID: 282 RVA: 0x00005D70 File Offset: 0x00003F70
		private void AddValue(object value, BsonType type)
		{
			this.AddToken(new BsonValue(value, type));
		}

		// Token: 0x0600011B RID: 283 RVA: 0x00005D80 File Offset: 0x00003F80
		internal void AddToken(BsonToken token)
		{
			if (this._parent != null)
			{
				if (this._parent is BsonObject)
				{
					((BsonObject)this._parent).Add(this._propertyName, token);
					this._propertyName = null;
					return;
				}
				((BsonArray)this._parent).Add(token);
				return;
			}
			else
			{
				if (token.Type != BsonType.Object && token.Type != BsonType.Array)
				{
					throw JsonWriterException.Create(this, "Error writing {0} value. BSON must start with an Object or Array.".FormatWith(CultureInfo.InvariantCulture, token.Type), null);
				}
				this._parent = token;
				this._root = token;
				return;
			}
		}

		// Token: 0x0600011C RID: 284 RVA: 0x00005E18 File Offset: 0x00004018
		public override void WriteValue(object value)
		{
			if (value is BigInteger)
			{
				base.InternalWriteValue(JsonToken.Integer);
				this.AddToken(new BsonBinary(((BigInteger)value).ToByteArray(), BsonBinaryType.Binary));
				return;
			}
			base.WriteValue(value);
		}

		// Token: 0x0600011D RID: 285 RVA: 0x00005E56 File Offset: 0x00004056
		public override void WriteNull()
		{
			base.WriteNull();
			this.AddValue(null, BsonType.Null);
		}

		// Token: 0x0600011E RID: 286 RVA: 0x00005E67 File Offset: 0x00004067
		public override void WriteUndefined()
		{
			base.WriteUndefined();
			this.AddValue(null, BsonType.Undefined);
		}

		// Token: 0x0600011F RID: 287 RVA: 0x00005E77 File Offset: 0x00004077
		public override void WriteValue(string value)
		{
			base.WriteValue(value);
			if (value == null)
			{
				this.AddValue(null, BsonType.Null);
				return;
			}
			this.AddToken(new BsonString(value, true));
		}

		// Token: 0x06000120 RID: 288 RVA: 0x00005E9A File Offset: 0x0000409A
		public override void WriteValue(int value)
		{
			base.WriteValue(value);
			this.AddValue(value, BsonType.Integer);
		}

		// Token: 0x06000121 RID: 289 RVA: 0x00005EB1 File Offset: 0x000040B1
		[CLSCompliant(false)]
		public override void WriteValue(uint value)
		{
			if (value > 2147483647U)
			{
				throw JsonWriterException.Create(this, "Value is too large to fit in a signed 32 bit integer. BSON does not support unsigned values.", null);
			}
			base.WriteValue(value);
			this.AddValue(value, BsonType.Integer);
		}

		// Token: 0x06000122 RID: 290 RVA: 0x00005EDD File Offset: 0x000040DD
		public override void WriteValue(long value)
		{
			base.WriteValue(value);
			this.AddValue(value, BsonType.Long);
		}

		// Token: 0x06000123 RID: 291 RVA: 0x00005EF4 File Offset: 0x000040F4
		[CLSCompliant(false)]
		public override void WriteValue(ulong value)
		{
			if (value > 9223372036854775807UL)
			{
				throw JsonWriterException.Create(this, "Value is too large to fit in a signed 64 bit integer. BSON does not support unsigned values.", null);
			}
			base.WriteValue(value);
			this.AddValue(value, BsonType.Long);
		}

		// Token: 0x06000124 RID: 292 RVA: 0x00005F24 File Offset: 0x00004124
		public override void WriteValue(float value)
		{
			base.WriteValue(value);
			this.AddValue(value, BsonType.Number);
		}

		// Token: 0x06000125 RID: 293 RVA: 0x00005F3A File Offset: 0x0000413A
		public override void WriteValue(double value)
		{
			base.WriteValue(value);
			this.AddValue(value, BsonType.Number);
		}

		// Token: 0x06000126 RID: 294 RVA: 0x00005F50 File Offset: 0x00004150
		public override void WriteValue(bool value)
		{
			base.WriteValue(value);
			this.AddValue(value, BsonType.Boolean);
		}

		// Token: 0x06000127 RID: 295 RVA: 0x00005F66 File Offset: 0x00004166
		public override void WriteValue(short value)
		{
			base.WriteValue(value);
			this.AddValue(value, BsonType.Integer);
		}

		// Token: 0x06000128 RID: 296 RVA: 0x00005F7D File Offset: 0x0000417D
		[CLSCompliant(false)]
		public override void WriteValue(ushort value)
		{
			base.WriteValue(value);
			this.AddValue(value, BsonType.Integer);
		}

		// Token: 0x06000129 RID: 297 RVA: 0x00005F94 File Offset: 0x00004194
		public override void WriteValue(char value)
		{
			base.WriteValue(value);
			string value2 = value.ToString(CultureInfo.InvariantCulture);
			this.AddToken(new BsonString(value2, true));
		}

		// Token: 0x0600012A RID: 298 RVA: 0x00005FC4 File Offset: 0x000041C4
		public override void WriteValue(byte value)
		{
			base.WriteValue(value);
			this.AddValue(value, BsonType.Integer);
		}

		// Token: 0x0600012B RID: 299 RVA: 0x00005FDB File Offset: 0x000041DB
		[CLSCompliant(false)]
		public override void WriteValue(sbyte value)
		{
			base.WriteValue(value);
			this.AddValue(value, BsonType.Integer);
		}

		// Token: 0x0600012C RID: 300 RVA: 0x00005FF2 File Offset: 0x000041F2
		public override void WriteValue(decimal value)
		{
			base.WriteValue(value);
			this.AddValue(value, BsonType.Number);
		}

		// Token: 0x0600012D RID: 301 RVA: 0x00006008 File Offset: 0x00004208
		public override void WriteValue(DateTime value)
		{
			base.WriteValue(value);
			value = DateTimeUtils.EnsureDateTime(value, base.DateTimeZoneHandling);
			this.AddValue(value, BsonType.Date);
		}

		// Token: 0x0600012E RID: 302 RVA: 0x0000602D File Offset: 0x0000422D
		public override void WriteValue(DateTimeOffset value)
		{
			base.WriteValue(value);
			this.AddValue(value, BsonType.Date);
		}

		// Token: 0x0600012F RID: 303 RVA: 0x00006044 File Offset: 0x00004244
		public override void WriteValue(byte[] value)
		{
			base.WriteValue(value);
			this.AddToken(new BsonBinary(value, BsonBinaryType.Binary));
		}

		// Token: 0x06000130 RID: 304 RVA: 0x0000605A File Offset: 0x0000425A
		public override void WriteValue(Guid value)
		{
			base.WriteValue(value);
			this.AddToken(new BsonBinary(value.ToByteArray(), BsonBinaryType.Uuid));
		}

		// Token: 0x06000131 RID: 305 RVA: 0x00006076 File Offset: 0x00004276
		public override void WriteValue(TimeSpan value)
		{
			base.WriteValue(value);
			this.AddToken(new BsonString(value.ToString(), true));
		}

		// Token: 0x06000132 RID: 306 RVA: 0x00006098 File Offset: 0x00004298
		public override void WriteValue(Uri value)
		{
			base.WriteValue(value);
			this.AddToken(new BsonString(value.ToString(), true));
		}

		// Token: 0x06000133 RID: 307 RVA: 0x000060B3 File Offset: 0x000042B3
		public void WriteObjectId(byte[] value)
		{
			ValidationUtils.ArgumentNotNull(value, "value");
			if (value.Length != 12)
			{
				throw JsonWriterException.Create(this, "An object id must be 12 bytes", null);
			}
			base.UpdateScopeWithFinishedValue();
			base.AutoComplete(JsonToken.Undefined);
			this.AddValue(value, BsonType.Oid);
		}

		// Token: 0x06000134 RID: 308 RVA: 0x000060EA File Offset: 0x000042EA
		public void WriteRegex(string pattern, string options)
		{
			ValidationUtils.ArgumentNotNull(pattern, "pattern");
			base.UpdateScopeWithFinishedValue();
			base.AutoComplete(JsonToken.Undefined);
			this.AddToken(new BsonRegex(pattern, options));
		}

		// Token: 0x04000082 RID: 130
		private readonly BsonBinaryWriter _writer;

		// Token: 0x04000083 RID: 131
		private BsonToken _root;

		// Token: 0x04000084 RID: 132
		private BsonToken _parent;

		// Token: 0x04000085 RID: 133
		private string _propertyName;
	}
}
