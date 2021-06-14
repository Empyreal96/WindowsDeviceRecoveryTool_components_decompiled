using System;
using System.Globalization;
using System.Linq;
using Microsoft.WindowsAzure.Storage.Core.Util;
using Microsoft.WindowsAzure.Storage.Table;

namespace Microsoft.WindowsAzure.Storage.Analytics
{
	// Token: 0x02000009 RID: 9
	public class MetricsEntity : TableEntity
	{
		// Token: 0x17000042 RID: 66
		// (get) Token: 0x060000C7 RID: 199 RVA: 0x0000555C File Offset: 0x0000375C
		public DateTimeOffset Time
		{
			get
			{
				return DateTimeOffset.ParseExact(base.PartitionKey, "yyyyMMdd'T'HHmm", CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal | DateTimeStyles.AssumeUniversal);
			}
		}

		// Token: 0x17000043 RID: 67
		// (get) Token: 0x060000C8 RID: 200 RVA: 0x00005578 File Offset: 0x00003778
		public string AccessType
		{
			get
			{
				CommonUtility.AssertNotNullOrEmpty("RowKey", base.RowKey);
				string text = base.RowKey.Split(new char[]
				{
					';'
				}).ElementAtOrDefault(0);
				CommonUtility.AssertNotNullOrEmpty("AccessType", text);
				return text;
			}
		}

		// Token: 0x17000044 RID: 68
		// (get) Token: 0x060000C9 RID: 201 RVA: 0x000055C0 File Offset: 0x000037C0
		public string TransactionType
		{
			get
			{
				CommonUtility.AssertNotNullOrEmpty("RowKey", base.RowKey);
				string text = base.RowKey.Split(new char[]
				{
					';'
				}).ElementAtOrDefault(1);
				CommonUtility.AssertNotNullOrEmpty("TransactionType", text);
				return text;
			}
		}

		// Token: 0x17000045 RID: 69
		// (get) Token: 0x060000CA RID: 202 RVA: 0x00005608 File Offset: 0x00003808
		// (set) Token: 0x060000CB RID: 203 RVA: 0x00005610 File Offset: 0x00003810
		public long TotalIngress { get; set; }

		// Token: 0x17000046 RID: 70
		// (get) Token: 0x060000CC RID: 204 RVA: 0x00005619 File Offset: 0x00003819
		// (set) Token: 0x060000CD RID: 205 RVA: 0x00005621 File Offset: 0x00003821
		public long TotalEgress { get; set; }

		// Token: 0x17000047 RID: 71
		// (get) Token: 0x060000CE RID: 206 RVA: 0x0000562A File Offset: 0x0000382A
		// (set) Token: 0x060000CF RID: 207 RVA: 0x00005632 File Offset: 0x00003832
		public long TotalRequests { get; set; }

		// Token: 0x17000048 RID: 72
		// (get) Token: 0x060000D0 RID: 208 RVA: 0x0000563B File Offset: 0x0000383B
		// (set) Token: 0x060000D1 RID: 209 RVA: 0x00005643 File Offset: 0x00003843
		public long TotalBillableRequests { get; set; }

		// Token: 0x17000049 RID: 73
		// (get) Token: 0x060000D2 RID: 210 RVA: 0x0000564C File Offset: 0x0000384C
		// (set) Token: 0x060000D3 RID: 211 RVA: 0x00005654 File Offset: 0x00003854
		public double Availability { get; set; }

		// Token: 0x1700004A RID: 74
		// (get) Token: 0x060000D4 RID: 212 RVA: 0x0000565D File Offset: 0x0000385D
		// (set) Token: 0x060000D5 RID: 213 RVA: 0x00005665 File Offset: 0x00003865
		public double AverageE2ELatency { get; set; }

		// Token: 0x1700004B RID: 75
		// (get) Token: 0x060000D6 RID: 214 RVA: 0x0000566E File Offset: 0x0000386E
		// (set) Token: 0x060000D7 RID: 215 RVA: 0x00005676 File Offset: 0x00003876
		public double AverageServerLatency { get; set; }

		// Token: 0x1700004C RID: 76
		// (get) Token: 0x060000D8 RID: 216 RVA: 0x0000567F File Offset: 0x0000387F
		// (set) Token: 0x060000D9 RID: 217 RVA: 0x00005687 File Offset: 0x00003887
		public double PercentSuccess { get; set; }

		// Token: 0x1700004D RID: 77
		// (get) Token: 0x060000DA RID: 218 RVA: 0x00005690 File Offset: 0x00003890
		// (set) Token: 0x060000DB RID: 219 RVA: 0x00005698 File Offset: 0x00003898
		public double PercentThrottlingError { get; set; }

		// Token: 0x1700004E RID: 78
		// (get) Token: 0x060000DC RID: 220 RVA: 0x000056A1 File Offset: 0x000038A1
		// (set) Token: 0x060000DD RID: 221 RVA: 0x000056A9 File Offset: 0x000038A9
		public double PercentTimeoutError { get; set; }

		// Token: 0x1700004F RID: 79
		// (get) Token: 0x060000DE RID: 222 RVA: 0x000056B2 File Offset: 0x000038B2
		// (set) Token: 0x060000DF RID: 223 RVA: 0x000056BA File Offset: 0x000038BA
		public double PercentServerOtherError { get; set; }

		// Token: 0x17000050 RID: 80
		// (get) Token: 0x060000E0 RID: 224 RVA: 0x000056C3 File Offset: 0x000038C3
		// (set) Token: 0x060000E1 RID: 225 RVA: 0x000056CB File Offset: 0x000038CB
		public double PercentClientOtherError { get; set; }

		// Token: 0x17000051 RID: 81
		// (get) Token: 0x060000E2 RID: 226 RVA: 0x000056D4 File Offset: 0x000038D4
		// (set) Token: 0x060000E3 RID: 227 RVA: 0x000056DC File Offset: 0x000038DC
		public double PercentAuthorizationError { get; set; }

		// Token: 0x17000052 RID: 82
		// (get) Token: 0x060000E4 RID: 228 RVA: 0x000056E5 File Offset: 0x000038E5
		// (set) Token: 0x060000E5 RID: 229 RVA: 0x000056ED File Offset: 0x000038ED
		public double PercentNetworkError { get; set; }

		// Token: 0x17000053 RID: 83
		// (get) Token: 0x060000E6 RID: 230 RVA: 0x000056F6 File Offset: 0x000038F6
		// (set) Token: 0x060000E7 RID: 231 RVA: 0x000056FE File Offset: 0x000038FE
		public long Success { get; set; }

		// Token: 0x17000054 RID: 84
		// (get) Token: 0x060000E8 RID: 232 RVA: 0x00005707 File Offset: 0x00003907
		// (set) Token: 0x060000E9 RID: 233 RVA: 0x0000570F File Offset: 0x0000390F
		public long AnonymousSuccess { get; set; }

		// Token: 0x17000055 RID: 85
		// (get) Token: 0x060000EA RID: 234 RVA: 0x00005718 File Offset: 0x00003918
		// (set) Token: 0x060000EB RID: 235 RVA: 0x00005720 File Offset: 0x00003920
		public long SASSuccess { get; set; }

		// Token: 0x17000056 RID: 86
		// (get) Token: 0x060000EC RID: 236 RVA: 0x00005729 File Offset: 0x00003929
		// (set) Token: 0x060000ED RID: 237 RVA: 0x00005731 File Offset: 0x00003931
		public long ThrottlingError { get; set; }

		// Token: 0x17000057 RID: 87
		// (get) Token: 0x060000EE RID: 238 RVA: 0x0000573A File Offset: 0x0000393A
		// (set) Token: 0x060000EF RID: 239 RVA: 0x00005742 File Offset: 0x00003942
		public long AnonymousThrottlingError { get; set; }

		// Token: 0x17000058 RID: 88
		// (get) Token: 0x060000F0 RID: 240 RVA: 0x0000574B File Offset: 0x0000394B
		// (set) Token: 0x060000F1 RID: 241 RVA: 0x00005753 File Offset: 0x00003953
		public long SASThrottlingError { get; set; }

		// Token: 0x17000059 RID: 89
		// (get) Token: 0x060000F2 RID: 242 RVA: 0x0000575C File Offset: 0x0000395C
		// (set) Token: 0x060000F3 RID: 243 RVA: 0x00005764 File Offset: 0x00003964
		public long ClientTimeoutError { get; set; }

		// Token: 0x1700005A RID: 90
		// (get) Token: 0x060000F4 RID: 244 RVA: 0x0000576D File Offset: 0x0000396D
		// (set) Token: 0x060000F5 RID: 245 RVA: 0x00005775 File Offset: 0x00003975
		public long AnonymousClientTimeoutError { get; set; }

		// Token: 0x1700005B RID: 91
		// (get) Token: 0x060000F6 RID: 246 RVA: 0x0000577E File Offset: 0x0000397E
		// (set) Token: 0x060000F7 RID: 247 RVA: 0x00005786 File Offset: 0x00003986
		public long SASClientTimeoutError { get; set; }

		// Token: 0x1700005C RID: 92
		// (get) Token: 0x060000F8 RID: 248 RVA: 0x0000578F File Offset: 0x0000398F
		// (set) Token: 0x060000F9 RID: 249 RVA: 0x00005797 File Offset: 0x00003997
		public long ServerTimeoutError { get; set; }

		// Token: 0x1700005D RID: 93
		// (get) Token: 0x060000FA RID: 250 RVA: 0x000057A0 File Offset: 0x000039A0
		// (set) Token: 0x060000FB RID: 251 RVA: 0x000057A8 File Offset: 0x000039A8
		public long AnonymousServerTimeoutError { get; set; }

		// Token: 0x1700005E RID: 94
		// (get) Token: 0x060000FC RID: 252 RVA: 0x000057B1 File Offset: 0x000039B1
		// (set) Token: 0x060000FD RID: 253 RVA: 0x000057B9 File Offset: 0x000039B9
		public long SASServerTimeoutError { get; set; }

		// Token: 0x1700005F RID: 95
		// (get) Token: 0x060000FE RID: 254 RVA: 0x000057C2 File Offset: 0x000039C2
		// (set) Token: 0x060000FF RID: 255 RVA: 0x000057CA File Offset: 0x000039CA
		public long ClientOtherError { get; set; }

		// Token: 0x17000060 RID: 96
		// (get) Token: 0x06000100 RID: 256 RVA: 0x000057D3 File Offset: 0x000039D3
		// (set) Token: 0x06000101 RID: 257 RVA: 0x000057DB File Offset: 0x000039DB
		public long SASClientOtherError { get; set; }

		// Token: 0x17000061 RID: 97
		// (get) Token: 0x06000102 RID: 258 RVA: 0x000057E4 File Offset: 0x000039E4
		// (set) Token: 0x06000103 RID: 259 RVA: 0x000057EC File Offset: 0x000039EC
		public long AnonymousClientOtherError { get; set; }

		// Token: 0x17000062 RID: 98
		// (get) Token: 0x06000104 RID: 260 RVA: 0x000057F5 File Offset: 0x000039F5
		// (set) Token: 0x06000105 RID: 261 RVA: 0x000057FD File Offset: 0x000039FD
		public long ServerOtherError { get; set; }

		// Token: 0x17000063 RID: 99
		// (get) Token: 0x06000106 RID: 262 RVA: 0x00005806 File Offset: 0x00003A06
		// (set) Token: 0x06000107 RID: 263 RVA: 0x0000580E File Offset: 0x00003A0E
		public long AnonymousServerOtherError { get; set; }

		// Token: 0x17000064 RID: 100
		// (get) Token: 0x06000108 RID: 264 RVA: 0x00005817 File Offset: 0x00003A17
		// (set) Token: 0x06000109 RID: 265 RVA: 0x0000581F File Offset: 0x00003A1F
		public long SASServerOtherError { get; set; }

		// Token: 0x17000065 RID: 101
		// (get) Token: 0x0600010A RID: 266 RVA: 0x00005828 File Offset: 0x00003A28
		// (set) Token: 0x0600010B RID: 267 RVA: 0x00005830 File Offset: 0x00003A30
		public long AuthorizationError { get; set; }

		// Token: 0x17000066 RID: 102
		// (get) Token: 0x0600010C RID: 268 RVA: 0x00005839 File Offset: 0x00003A39
		// (set) Token: 0x0600010D RID: 269 RVA: 0x00005841 File Offset: 0x00003A41
		public long AnonymousAuthorizationError { get; set; }

		// Token: 0x17000067 RID: 103
		// (get) Token: 0x0600010E RID: 270 RVA: 0x0000584A File Offset: 0x00003A4A
		// (set) Token: 0x0600010F RID: 271 RVA: 0x00005852 File Offset: 0x00003A52
		public long SASAuthorizationError { get; set; }

		// Token: 0x17000068 RID: 104
		// (get) Token: 0x06000110 RID: 272 RVA: 0x0000585B File Offset: 0x00003A5B
		// (set) Token: 0x06000111 RID: 273 RVA: 0x00005863 File Offset: 0x00003A63
		public long NetworkError { get; set; }

		// Token: 0x17000069 RID: 105
		// (get) Token: 0x06000112 RID: 274 RVA: 0x0000586C File Offset: 0x00003A6C
		// (set) Token: 0x06000113 RID: 275 RVA: 0x00005874 File Offset: 0x00003A74
		public long AnonymousNetworkError { get; set; }

		// Token: 0x1700006A RID: 106
		// (get) Token: 0x06000114 RID: 276 RVA: 0x0000587D File Offset: 0x00003A7D
		// (set) Token: 0x06000115 RID: 277 RVA: 0x00005885 File Offset: 0x00003A85
		public long SASNetworkError { get; set; }
	}
}
