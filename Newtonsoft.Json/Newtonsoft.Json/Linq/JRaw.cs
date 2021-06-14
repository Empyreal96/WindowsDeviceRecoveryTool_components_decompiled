using System;
using System.Globalization;
using System.IO;

namespace Newtonsoft.Json.Linq
{
	// Token: 0x02000077 RID: 119
	public class JRaw : JValue
	{
		// Token: 0x060006A9 RID: 1705 RVA: 0x0001A1CF File Offset: 0x000183CF
		public JRaw(JRaw other) : base(other)
		{
		}

		// Token: 0x060006AA RID: 1706 RVA: 0x0001A1D8 File Offset: 0x000183D8
		public JRaw(object rawJson) : base(rawJson, JTokenType.Raw)
		{
		}

		// Token: 0x060006AB RID: 1707 RVA: 0x0001A1E4 File Offset: 0x000183E4
		public static JRaw Create(JsonReader reader)
		{
			JRaw result;
			using (StringWriter stringWriter = new StringWriter(CultureInfo.InvariantCulture))
			{
				using (JsonTextWriter jsonTextWriter = new JsonTextWriter(stringWriter))
				{
					jsonTextWriter.WriteToken(reader);
					result = new JRaw(stringWriter.ToString());
				}
			}
			return result;
		}

		// Token: 0x060006AC RID: 1708 RVA: 0x0001A24C File Offset: 0x0001844C
		internal override JToken CloneToken()
		{
			return new JRaw(this);
		}
	}
}
