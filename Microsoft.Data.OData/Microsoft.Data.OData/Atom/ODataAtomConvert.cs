using System;
using System.Globalization;
using System.Xml;

namespace Microsoft.Data.OData.Atom
{
	// Token: 0x02000282 RID: 642
	internal static class ODataAtomConvert
	{
		// Token: 0x0600154F RID: 5455 RVA: 0x0004DDF3 File Offset: 0x0004BFF3
		internal static string ToString(bool b)
		{
			if (!b)
			{
				return "false";
			}
			return "true";
		}

		// Token: 0x06001550 RID: 5456 RVA: 0x0004DE03 File Offset: 0x0004C003
		internal static string ToString(byte b)
		{
			return XmlConvert.ToString(b);
		}

		// Token: 0x06001551 RID: 5457 RVA: 0x0004DE0B File Offset: 0x0004C00B
		internal static string ToString(decimal d)
		{
			return XmlConvert.ToString(d);
		}

		// Token: 0x06001552 RID: 5458 RVA: 0x0004DE13 File Offset: 0x0004C013
		internal static string ToString(this DateTime dt)
		{
			return PlatformHelper.ConvertDateTimeToString(dt);
		}

		// Token: 0x06001553 RID: 5459 RVA: 0x0004DE1B File Offset: 0x0004C01B
		internal static string ToString(DateTimeOffset dateTime)
		{
			return XmlConvert.ToString(dateTime);
		}

		// Token: 0x06001554 RID: 5460 RVA: 0x0004DE24 File Offset: 0x0004C024
		internal static string ToAtomString(DateTimeOffset dateTime)
		{
			if (dateTime.Offset == ODataAtomConvert.zeroOffset)
			{
				return dateTime.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ssZ", CultureInfo.InvariantCulture);
			}
			return dateTime.ToString("yyyy-MM-ddTHH:mm:sszzz", CultureInfo.InvariantCulture);
		}

		// Token: 0x06001555 RID: 5461 RVA: 0x0004DE6F File Offset: 0x0004C06F
		internal static string ToString(this TimeSpan ts)
		{
			return XmlConvert.ToString(ts);
		}

		// Token: 0x06001556 RID: 5462 RVA: 0x0004DE77 File Offset: 0x0004C077
		internal static string ToString(this double d)
		{
			return XmlConvert.ToString(d);
		}

		// Token: 0x06001557 RID: 5463 RVA: 0x0004DE7F File Offset: 0x0004C07F
		internal static string ToString(this short i)
		{
			return XmlConvert.ToString(i);
		}

		// Token: 0x06001558 RID: 5464 RVA: 0x0004DE87 File Offset: 0x0004C087
		internal static string ToString(this int i)
		{
			return XmlConvert.ToString(i);
		}

		// Token: 0x06001559 RID: 5465 RVA: 0x0004DE8F File Offset: 0x0004C08F
		internal static string ToString(this long i)
		{
			return XmlConvert.ToString(i);
		}

		// Token: 0x0600155A RID: 5466 RVA: 0x0004DE97 File Offset: 0x0004C097
		internal static string ToString(this sbyte sb)
		{
			return XmlConvert.ToString(sb);
		}

		// Token: 0x0600155B RID: 5467 RVA: 0x0004DE9F File Offset: 0x0004C09F
		internal static string ToString(this byte[] bytes)
		{
			return Convert.ToBase64String(bytes);
		}

		// Token: 0x0600155C RID: 5468 RVA: 0x0004DEA7 File Offset: 0x0004C0A7
		internal static string ToString(this float s)
		{
			return XmlConvert.ToString(s);
		}

		// Token: 0x0600155D RID: 5469 RVA: 0x0004DEAF File Offset: 0x0004C0AF
		internal static string ToString(this Guid guid)
		{
			return XmlConvert.ToString(guid);
		}

		// Token: 0x040007CA RID: 1994
		private static readonly TimeSpan zeroOffset = new TimeSpan(0, 0, 0);
	}
}
