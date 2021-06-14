using System;
using System.Globalization;
using System.IO;
using System.Numerics;

namespace Newtonsoft.Json.Serialization
{
	// Token: 0x020000CE RID: 206
	internal class TraceJsonWriter : JsonWriter
	{
		// Token: 0x06000A2A RID: 2602 RVA: 0x00028300 File Offset: 0x00026500
		public TraceJsonWriter(JsonWriter innerWriter)
		{
			this._innerWriter = innerWriter;
			this._sw = new StringWriter(CultureInfo.InvariantCulture);
			this._textWriter = new JsonTextWriter(this._sw);
			this._textWriter.Formatting = Formatting.Indented;
			this._textWriter.Culture = innerWriter.Culture;
			this._textWriter.DateFormatHandling = innerWriter.DateFormatHandling;
			this._textWriter.DateFormatString = innerWriter.DateFormatString;
			this._textWriter.DateTimeZoneHandling = innerWriter.DateTimeZoneHandling;
			this._textWriter.FloatFormatHandling = innerWriter.FloatFormatHandling;
		}

		// Token: 0x06000A2B RID: 2603 RVA: 0x0002839C File Offset: 0x0002659C
		public string GetJson()
		{
			return this._sw.ToString();
		}

		// Token: 0x06000A2C RID: 2604 RVA: 0x000283A9 File Offset: 0x000265A9
		public override void WriteValue(decimal value)
		{
			this._textWriter.WriteValue(value);
			this._innerWriter.WriteValue(value);
			base.WriteValue(value);
		}

		// Token: 0x06000A2D RID: 2605 RVA: 0x000283CA File Offset: 0x000265CA
		public override void WriteValue(bool value)
		{
			this._textWriter.WriteValue(value);
			this._innerWriter.WriteValue(value);
			base.WriteValue(value);
		}

		// Token: 0x06000A2E RID: 2606 RVA: 0x000283EB File Offset: 0x000265EB
		public override void WriteValue(byte value)
		{
			this._textWriter.WriteValue(value);
			this._innerWriter.WriteValue(value);
			base.WriteValue(value);
		}

		// Token: 0x06000A2F RID: 2607 RVA: 0x0002840C File Offset: 0x0002660C
		public override void WriteValue(byte? value)
		{
			this._textWriter.WriteValue(value);
			this._innerWriter.WriteValue(value);
			base.WriteValue(value);
		}

		// Token: 0x06000A30 RID: 2608 RVA: 0x0002842D File Offset: 0x0002662D
		public override void WriteValue(char value)
		{
			this._textWriter.WriteValue(value);
			this._innerWriter.WriteValue(value);
			base.WriteValue(value);
		}

		// Token: 0x06000A31 RID: 2609 RVA: 0x0002844E File Offset: 0x0002664E
		public override void WriteValue(byte[] value)
		{
			this._textWriter.WriteValue(value);
			this._innerWriter.WriteValue(value);
			base.WriteValue(value);
		}

		// Token: 0x06000A32 RID: 2610 RVA: 0x0002846F File Offset: 0x0002666F
		public override void WriteValue(DateTime value)
		{
			this._textWriter.WriteValue(value);
			this._innerWriter.WriteValue(value);
			base.WriteValue(value);
		}

		// Token: 0x06000A33 RID: 2611 RVA: 0x00028490 File Offset: 0x00026690
		public override void WriteValue(DateTimeOffset value)
		{
			this._textWriter.WriteValue(value);
			this._innerWriter.WriteValue(value);
			base.WriteValue(value);
		}

		// Token: 0x06000A34 RID: 2612 RVA: 0x000284B1 File Offset: 0x000266B1
		public override void WriteValue(double value)
		{
			this._textWriter.WriteValue(value);
			this._innerWriter.WriteValue(value);
			base.WriteValue(value);
		}

		// Token: 0x06000A35 RID: 2613 RVA: 0x000284D2 File Offset: 0x000266D2
		public override void WriteUndefined()
		{
			this._textWriter.WriteUndefined();
			this._innerWriter.WriteUndefined();
			base.WriteUndefined();
		}

		// Token: 0x06000A36 RID: 2614 RVA: 0x000284F0 File Offset: 0x000266F0
		public override void WriteNull()
		{
			this._textWriter.WriteNull();
			this._innerWriter.WriteNull();
			base.WriteUndefined();
		}

		// Token: 0x06000A37 RID: 2615 RVA: 0x0002850E File Offset: 0x0002670E
		public override void WriteValue(float value)
		{
			this._textWriter.WriteValue(value);
			this._innerWriter.WriteValue(value);
			base.WriteValue(value);
		}

		// Token: 0x06000A38 RID: 2616 RVA: 0x0002852F File Offset: 0x0002672F
		public override void WriteValue(Guid value)
		{
			this._textWriter.WriteValue(value);
			this._innerWriter.WriteValue(value);
			base.WriteValue(value);
		}

		// Token: 0x06000A39 RID: 2617 RVA: 0x00028550 File Offset: 0x00026750
		public override void WriteValue(int value)
		{
			this._textWriter.WriteValue(value);
			this._innerWriter.WriteValue(value);
			base.WriteValue(value);
		}

		// Token: 0x06000A3A RID: 2618 RVA: 0x00028571 File Offset: 0x00026771
		public override void WriteValue(long value)
		{
			this._textWriter.WriteValue(value);
			this._innerWriter.WriteValue(value);
			base.WriteValue(value);
		}

		// Token: 0x06000A3B RID: 2619 RVA: 0x00028594 File Offset: 0x00026794
		public override void WriteValue(object value)
		{
			if (value is BigInteger)
			{
				this._textWriter.WriteValue(value);
				this._innerWriter.WriteValue(value);
				base.InternalWriteValue(JsonToken.Integer);
				return;
			}
			this._textWriter.WriteValue(value);
			this._innerWriter.WriteValue(value);
			base.WriteValue(value);
		}

		// Token: 0x06000A3C RID: 2620 RVA: 0x000285E8 File Offset: 0x000267E8
		public override void WriteValue(sbyte value)
		{
			this._textWriter.WriteValue(value);
			this._innerWriter.WriteValue(value);
			base.WriteValue(value);
		}

		// Token: 0x06000A3D RID: 2621 RVA: 0x00028609 File Offset: 0x00026809
		public override void WriteValue(short value)
		{
			this._textWriter.WriteValue(value);
			this._innerWriter.WriteValue(value);
			base.WriteValue(value);
		}

		// Token: 0x06000A3E RID: 2622 RVA: 0x0002862A File Offset: 0x0002682A
		public override void WriteValue(string value)
		{
			this._textWriter.WriteValue(value);
			this._innerWriter.WriteValue(value);
			base.WriteValue(value);
		}

		// Token: 0x06000A3F RID: 2623 RVA: 0x0002864B File Offset: 0x0002684B
		public override void WriteValue(TimeSpan value)
		{
			this._textWriter.WriteValue(value);
			this._innerWriter.WriteValue(value);
			base.WriteValue(value);
		}

		// Token: 0x06000A40 RID: 2624 RVA: 0x0002866C File Offset: 0x0002686C
		public override void WriteValue(uint value)
		{
			this._textWriter.WriteValue(value);
			this._innerWriter.WriteValue(value);
			base.WriteValue(value);
		}

		// Token: 0x06000A41 RID: 2625 RVA: 0x0002868D File Offset: 0x0002688D
		public override void WriteValue(ulong value)
		{
			this._textWriter.WriteValue(value);
			this._innerWriter.WriteValue(value);
			base.WriteValue(value);
		}

		// Token: 0x06000A42 RID: 2626 RVA: 0x000286AE File Offset: 0x000268AE
		public override void WriteValue(Uri value)
		{
			this._textWriter.WriteValue(value);
			this._innerWriter.WriteValue(value);
			base.WriteValue(value);
		}

		// Token: 0x06000A43 RID: 2627 RVA: 0x000286CF File Offset: 0x000268CF
		public override void WriteValue(ushort value)
		{
			this._textWriter.WriteValue(value);
			this._innerWriter.WriteValue(value);
			base.WriteValue(value);
		}

		// Token: 0x06000A44 RID: 2628 RVA: 0x000286F0 File Offset: 0x000268F0
		public override void WriteWhitespace(string ws)
		{
			this._textWriter.WriteWhitespace(ws);
			this._innerWriter.WriteWhitespace(ws);
			base.WriteWhitespace(ws);
		}

		// Token: 0x06000A45 RID: 2629 RVA: 0x00028711 File Offset: 0x00026911
		public override void WriteComment(string text)
		{
			this._textWriter.WriteComment(text);
			this._innerWriter.WriteComment(text);
			base.WriteComment(text);
		}

		// Token: 0x06000A46 RID: 2630 RVA: 0x00028732 File Offset: 0x00026932
		public override void WriteStartArray()
		{
			this._textWriter.WriteStartArray();
			this._innerWriter.WriteStartArray();
			base.WriteStartArray();
		}

		// Token: 0x06000A47 RID: 2631 RVA: 0x00028750 File Offset: 0x00026950
		public override void WriteEndArray()
		{
			this._textWriter.WriteEndArray();
			this._innerWriter.WriteEndArray();
			base.WriteEndArray();
		}

		// Token: 0x06000A48 RID: 2632 RVA: 0x0002876E File Offset: 0x0002696E
		public override void WriteStartConstructor(string name)
		{
			this._textWriter.WriteStartConstructor(name);
			this._innerWriter.WriteStartConstructor(name);
			base.WriteStartConstructor(name);
		}

		// Token: 0x06000A49 RID: 2633 RVA: 0x0002878F File Offset: 0x0002698F
		public override void WriteEndConstructor()
		{
			this._textWriter.WriteEndConstructor();
			this._innerWriter.WriteEndConstructor();
			base.WriteEndConstructor();
		}

		// Token: 0x06000A4A RID: 2634 RVA: 0x000287AD File Offset: 0x000269AD
		public override void WritePropertyName(string name)
		{
			this._textWriter.WritePropertyName(name);
			this._innerWriter.WritePropertyName(name);
			base.WritePropertyName(name);
		}

		// Token: 0x06000A4B RID: 2635 RVA: 0x000287CE File Offset: 0x000269CE
		public override void WritePropertyName(string name, bool escape)
		{
			this._textWriter.WritePropertyName(name, escape);
			this._innerWriter.WritePropertyName(name, escape);
			base.WritePropertyName(name);
		}

		// Token: 0x06000A4C RID: 2636 RVA: 0x000287F1 File Offset: 0x000269F1
		public override void WriteStartObject()
		{
			this._textWriter.WriteStartObject();
			this._innerWriter.WriteStartObject();
			base.WriteStartObject();
		}

		// Token: 0x06000A4D RID: 2637 RVA: 0x0002880F File Offset: 0x00026A0F
		public override void WriteEndObject()
		{
			this._textWriter.WriteEndObject();
			this._innerWriter.WriteEndObject();
			base.WriteEndObject();
		}

		// Token: 0x06000A4E RID: 2638 RVA: 0x0002882D File Offset: 0x00026A2D
		public override void WriteRaw(string json)
		{
			this._textWriter.WriteRaw(json);
			this._innerWriter.WriteRaw(json);
			base.WriteRaw(json);
		}

		// Token: 0x06000A4F RID: 2639 RVA: 0x0002884E File Offset: 0x00026A4E
		public override void WriteRawValue(string json)
		{
			this._textWriter.WriteRawValue(json);
			this._innerWriter.WriteRawValue(json);
			base.WriteRawValue(json);
		}

		// Token: 0x06000A50 RID: 2640 RVA: 0x0002886F File Offset: 0x00026A6F
		public override void Close()
		{
			this._textWriter.Close();
			this._innerWriter.Close();
			base.Close();
		}

		// Token: 0x06000A51 RID: 2641 RVA: 0x0002888D File Offset: 0x00026A8D
		public override void Flush()
		{
			this._textWriter.Flush();
			this._innerWriter.Flush();
		}

		// Token: 0x04000373 RID: 883
		private readonly JsonWriter _innerWriter;

		// Token: 0x04000374 RID: 884
		private readonly JsonTextWriter _textWriter;

		// Token: 0x04000375 RID: 885
		private readonly StringWriter _sw;
	}
}
