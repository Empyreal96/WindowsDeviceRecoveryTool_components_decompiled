using System;

namespace Newtonsoft.Json.Converters
{
	// Token: 0x0200001D RID: 29
	public abstract class DateTimeConverterBase : JsonConverter
	{
		// Token: 0x06000159 RID: 345 RVA: 0x00006A90 File Offset: 0x00004C90
		public override bool CanConvert(Type objectType)
		{
			return objectType == typeof(DateTime) || objectType == typeof(DateTime?) || (objectType == typeof(DateTimeOffset) || objectType == typeof(DateTimeOffset?));
		}
	}
}
