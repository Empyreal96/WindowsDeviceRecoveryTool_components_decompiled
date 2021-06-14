using System;
using System.Globalization;
using Microsoft.WindowsAzure.Storage.Core.Util;

namespace Microsoft.WindowsAzure.Storage.Analytics
{
	// Token: 0x02000007 RID: 7
	[Serializable]
	public class LogRecord
	{
		// Token: 0x06000071 RID: 113 RVA: 0x00004C16 File Offset: 0x00002E16
		internal LogRecord()
		{
		}

		// Token: 0x06000072 RID: 114 RVA: 0x00004C20 File Offset: 0x00002E20
		internal LogRecord(LogRecordStreamReader reader)
		{
			CommonUtility.AssertNotNull("reader", reader);
			this.VersionNumber = reader.ReadString();
			CommonUtility.AssertNotNullOrEmpty("VersionNumber", this.VersionNumber);
			if (string.Equals("1.0", this.VersionNumber, StringComparison.Ordinal))
			{
				this.PopulateVersion1Log(reader);
				return;
			}
			throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "A storage log version of {0} is unsupported.", new object[]
			{
				this.VersionNumber
			}));
		}

		// Token: 0x06000073 RID: 115 RVA: 0x00004C9C File Offset: 0x00002E9C
		private void PopulateVersion1Log(LogRecordStreamReader reader)
		{
			this.RequestStartTime = reader.ReadDateTimeOffset("o");
			this.OperationType = reader.ReadString();
			this.RequestStatus = reader.ReadString();
			this.HttpStatusCode = reader.ReadString();
			this.EndToEndLatency = reader.ReadTimeSpanInMS();
			this.ServerLatency = reader.ReadTimeSpanInMS();
			this.AuthenticationType = reader.ReadString();
			this.RequesterAccountName = reader.ReadString();
			this.OwnerAccountName = reader.ReadString();
			this.ServiceType = reader.ReadString();
			this.RequestUrl = reader.ReadUri();
			this.RequestedObjectKey = reader.ReadQuotedString();
			this.RequestIdHeader = reader.ReadGuid();
			this.OperationCount = reader.ReadInt();
			this.RequesterIPAddress = reader.ReadString();
			this.RequestVersionHeader = reader.ReadString();
			this.RequestHeaderSize = reader.ReadLong();
			this.RequestPacketSize = reader.ReadLong();
			this.ResponseHeaderSize = reader.ReadLong();
			this.ResponsePacketSize = reader.ReadLong();
			this.RequestContentLength = reader.ReadLong();
			this.RequestMD5 = reader.ReadQuotedString();
			this.ServerMD5 = reader.ReadQuotedString();
			this.ETagIdentifier = reader.ReadQuotedString();
			this.LastModifiedTime = reader.ReadDateTimeOffset("dddd, dd-MMM-yy HH:mm:ss 'GMT'");
			this.ConditionsUsed = reader.ReadQuotedString();
			this.UserAgentHeader = reader.ReadQuotedString();
			this.ReferrerHeader = reader.ReadQuotedString();
			this.ClientRequestId = reader.ReadQuotedString();
			reader.EndCurrentRecord();
		}

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x06000074 RID: 116 RVA: 0x00004E15 File Offset: 0x00003015
		// (set) Token: 0x06000075 RID: 117 RVA: 0x00004E1D File Offset: 0x0000301D
		public string VersionNumber { get; internal set; }

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x06000076 RID: 118 RVA: 0x00004E26 File Offset: 0x00003026
		// (set) Token: 0x06000077 RID: 119 RVA: 0x00004E2E File Offset: 0x0000302E
		public DateTimeOffset? RequestStartTime { get; internal set; }

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x06000078 RID: 120 RVA: 0x00004E37 File Offset: 0x00003037
		// (set) Token: 0x06000079 RID: 121 RVA: 0x00004E3F File Offset: 0x0000303F
		public string OperationType { get; internal set; }

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x0600007A RID: 122 RVA: 0x00004E48 File Offset: 0x00003048
		// (set) Token: 0x0600007B RID: 123 RVA: 0x00004E50 File Offset: 0x00003050
		public string RequestStatus { get; internal set; }

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x0600007C RID: 124 RVA: 0x00004E59 File Offset: 0x00003059
		// (set) Token: 0x0600007D RID: 125 RVA: 0x00004E61 File Offset: 0x00003061
		public string HttpStatusCode { get; internal set; }

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x0600007E RID: 126 RVA: 0x00004E6A File Offset: 0x0000306A
		// (set) Token: 0x0600007F RID: 127 RVA: 0x00004E72 File Offset: 0x00003072
		public TimeSpan? EndToEndLatency { get; internal set; }

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x06000080 RID: 128 RVA: 0x00004E7B File Offset: 0x0000307B
		// (set) Token: 0x06000081 RID: 129 RVA: 0x00004E83 File Offset: 0x00003083
		public TimeSpan? ServerLatency { get; internal set; }

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x06000082 RID: 130 RVA: 0x00004E8C File Offset: 0x0000308C
		// (set) Token: 0x06000083 RID: 131 RVA: 0x00004E94 File Offset: 0x00003094
		public string AuthenticationType { get; internal set; }

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x06000084 RID: 132 RVA: 0x00004E9D File Offset: 0x0000309D
		// (set) Token: 0x06000085 RID: 133 RVA: 0x00004EA5 File Offset: 0x000030A5
		public string RequesterAccountName { get; internal set; }

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x06000086 RID: 134 RVA: 0x00004EAE File Offset: 0x000030AE
		// (set) Token: 0x06000087 RID: 135 RVA: 0x00004EB6 File Offset: 0x000030B6
		public string OwnerAccountName { get; internal set; }

		// Token: 0x1700002C RID: 44
		// (get) Token: 0x06000088 RID: 136 RVA: 0x00004EBF File Offset: 0x000030BF
		// (set) Token: 0x06000089 RID: 137 RVA: 0x00004EC7 File Offset: 0x000030C7
		public string ServiceType { get; internal set; }

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x0600008A RID: 138 RVA: 0x00004ED0 File Offset: 0x000030D0
		// (set) Token: 0x0600008B RID: 139 RVA: 0x00004ED8 File Offset: 0x000030D8
		public Uri RequestUrl { get; internal set; }

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x0600008C RID: 140 RVA: 0x00004EE1 File Offset: 0x000030E1
		// (set) Token: 0x0600008D RID: 141 RVA: 0x00004EE9 File Offset: 0x000030E9
		public string RequestedObjectKey { get; internal set; }

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x0600008E RID: 142 RVA: 0x00004EF2 File Offset: 0x000030F2
		// (set) Token: 0x0600008F RID: 143 RVA: 0x00004EFA File Offset: 0x000030FA
		public Guid? RequestIdHeader { get; internal set; }

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x06000090 RID: 144 RVA: 0x00004F03 File Offset: 0x00003103
		// (set) Token: 0x06000091 RID: 145 RVA: 0x00004F0B File Offset: 0x0000310B
		public int? OperationCount { get; internal set; }

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x06000092 RID: 146 RVA: 0x00004F14 File Offset: 0x00003114
		// (set) Token: 0x06000093 RID: 147 RVA: 0x00004F1C File Offset: 0x0000311C
		public string RequesterIPAddress { get; internal set; }

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x06000094 RID: 148 RVA: 0x00004F25 File Offset: 0x00003125
		// (set) Token: 0x06000095 RID: 149 RVA: 0x00004F2D File Offset: 0x0000312D
		public string RequestVersionHeader { get; internal set; }

		// Token: 0x17000033 RID: 51
		// (get) Token: 0x06000096 RID: 150 RVA: 0x00004F36 File Offset: 0x00003136
		// (set) Token: 0x06000097 RID: 151 RVA: 0x00004F3E File Offset: 0x0000313E
		public long? RequestHeaderSize { get; internal set; }

		// Token: 0x17000034 RID: 52
		// (get) Token: 0x06000098 RID: 152 RVA: 0x00004F47 File Offset: 0x00003147
		// (set) Token: 0x06000099 RID: 153 RVA: 0x00004F4F File Offset: 0x0000314F
		public long? RequestPacketSize { get; internal set; }

		// Token: 0x17000035 RID: 53
		// (get) Token: 0x0600009A RID: 154 RVA: 0x00004F58 File Offset: 0x00003158
		// (set) Token: 0x0600009B RID: 155 RVA: 0x00004F60 File Offset: 0x00003160
		public long? ResponseHeaderSize { get; internal set; }

		// Token: 0x17000036 RID: 54
		// (get) Token: 0x0600009C RID: 156 RVA: 0x00004F69 File Offset: 0x00003169
		// (set) Token: 0x0600009D RID: 157 RVA: 0x00004F71 File Offset: 0x00003171
		public long? ResponsePacketSize { get; internal set; }

		// Token: 0x17000037 RID: 55
		// (get) Token: 0x0600009E RID: 158 RVA: 0x00004F7A File Offset: 0x0000317A
		// (set) Token: 0x0600009F RID: 159 RVA: 0x00004F82 File Offset: 0x00003182
		public long? RequestContentLength { get; internal set; }

		// Token: 0x17000038 RID: 56
		// (get) Token: 0x060000A0 RID: 160 RVA: 0x00004F8B File Offset: 0x0000318B
		// (set) Token: 0x060000A1 RID: 161 RVA: 0x00004F93 File Offset: 0x00003193
		public string RequestMD5 { get; internal set; }

		// Token: 0x17000039 RID: 57
		// (get) Token: 0x060000A2 RID: 162 RVA: 0x00004F9C File Offset: 0x0000319C
		// (set) Token: 0x060000A3 RID: 163 RVA: 0x00004FA4 File Offset: 0x000031A4
		public string ServerMD5 { get; internal set; }

		// Token: 0x1700003A RID: 58
		// (get) Token: 0x060000A4 RID: 164 RVA: 0x00004FAD File Offset: 0x000031AD
		// (set) Token: 0x060000A5 RID: 165 RVA: 0x00004FB5 File Offset: 0x000031B5
		public string ETagIdentifier { get; internal set; }

		// Token: 0x1700003B RID: 59
		// (get) Token: 0x060000A6 RID: 166 RVA: 0x00004FBE File Offset: 0x000031BE
		// (set) Token: 0x060000A7 RID: 167 RVA: 0x00004FC6 File Offset: 0x000031C6
		public DateTimeOffset? LastModifiedTime { get; internal set; }

		// Token: 0x1700003C RID: 60
		// (get) Token: 0x060000A8 RID: 168 RVA: 0x00004FCF File Offset: 0x000031CF
		// (set) Token: 0x060000A9 RID: 169 RVA: 0x00004FD7 File Offset: 0x000031D7
		public string ConditionsUsed { get; internal set; }

		// Token: 0x1700003D RID: 61
		// (get) Token: 0x060000AA RID: 170 RVA: 0x00004FE0 File Offset: 0x000031E0
		// (set) Token: 0x060000AB RID: 171 RVA: 0x00004FE8 File Offset: 0x000031E8
		public string UserAgentHeader { get; internal set; }

		// Token: 0x1700003E RID: 62
		// (get) Token: 0x060000AC RID: 172 RVA: 0x00004FF1 File Offset: 0x000031F1
		// (set) Token: 0x060000AD RID: 173 RVA: 0x00004FF9 File Offset: 0x000031F9
		public string ReferrerHeader { get; internal set; }

		// Token: 0x1700003F RID: 63
		// (get) Token: 0x060000AE RID: 174 RVA: 0x00005002 File Offset: 0x00003202
		// (set) Token: 0x060000AF RID: 175 RVA: 0x0000500A File Offset: 0x0000320A
		public string ClientRequestId { get; internal set; }
	}
}
