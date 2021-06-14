using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace SoftwareRepository.Streaming
{
	// Token: 0x02000013 RID: 19
	[DataContract]
	public class UrlResult
	{
		// Token: 0x17000016 RID: 22
		// (get) Token: 0x06000064 RID: 100 RVA: 0x0000318D File Offset: 0x0000138D
		// (set) Token: 0x06000065 RID: 101 RVA: 0x00003195 File Offset: 0x00001395
		[SuppressMessage("Microsoft.Design", "CA1056:UriPropertiesShouldNotBeStrings")]
		[DataMember(Name = "fileUrl")]
		public string FileUrl { get; set; }

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x06000066 RID: 102 RVA: 0x0000319E File Offset: 0x0000139E
		// (set) Token: 0x06000067 RID: 103 RVA: 0x000031A6 File Offset: 0x000013A6
		[DataMember(Name = "isSelected")]
		public bool IsSelected { get; set; }

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x06000068 RID: 104 RVA: 0x000031AF File Offset: 0x000013AF
		// (set) Token: 0x06000069 RID: 105 RVA: 0x000031B7 File Offset: 0x000013B7
		[DataMember(Name = "testSpeed")]
		public double TestSpeed { get; set; }

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x0600006A RID: 106 RVA: 0x000031C0 File Offset: 0x000013C0
		// (set) Token: 0x0600006B RID: 107 RVA: 0x00002050 File Offset: 0x00000250
		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
		[SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "value")]
		[DataMember(Name = "displayTestSpeed")]
		public string DisplayTestSpeed
		{
			get
			{
				return this.TestSpeed.ToSpeedFormat();
			}
			private set
			{
			}
		}

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x0600006C RID: 108 RVA: 0x000031CD File Offset: 0x000013CD
		// (set) Token: 0x0600006D RID: 109 RVA: 0x000031D5 File Offset: 0x000013D5
		[DataMember(Name = "error")]
		public string Error { get; set; }

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x0600006E RID: 110 RVA: 0x000031DE File Offset: 0x000013DE
		// (set) Token: 0x0600006F RID: 111 RVA: 0x000031E6 File Offset: 0x000013E6
		[DataMember(Name = "bytesRead")]
		public long BytesRead { get; set; }
	}
}
