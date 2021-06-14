using System;

namespace Microsoft.WindowsDeviceRecoveryTool.LogicCommon.Reporting
{
	// Token: 0x0200002F RID: 47
	public sealed class ReportUpdateStatus4Parameters
	{
		// Token: 0x170000A7 RID: 167
		// (get) Token: 0x0600021F RID: 543 RVA: 0x00009D04 File Offset: 0x00007F04
		// (set) Token: 0x06000220 RID: 544 RVA: 0x00009D1B File Offset: 0x00007F1B
		public string OmsiModuleSessionId { get; set; }

		// Token: 0x170000A8 RID: 168
		// (get) Token: 0x06000221 RID: 545 RVA: 0x00009D24 File Offset: 0x00007F24
		// (set) Token: 0x06000222 RID: 546 RVA: 0x00009D3B File Offset: 0x00007F3B
		public string UserSiteLanguageId { get; set; }

		// Token: 0x170000A9 RID: 169
		// (get) Token: 0x06000223 RID: 547 RVA: 0x00009D44 File Offset: 0x00007F44
		// (set) Token: 0x06000224 RID: 548 RVA: 0x00009D5B File Offset: 0x00007F5B
		public long TimeStamp { get; set; }

		// Token: 0x170000AA RID: 170
		// (get) Token: 0x06000225 RID: 549 RVA: 0x00009D64 File Offset: 0x00007F64
		// (set) Token: 0x06000226 RID: 550 RVA: 0x00009D7B File Offset: 0x00007F7B
		public string IP { get; set; }

		// Token: 0x170000AB RID: 171
		// (get) Token: 0x06000227 RID: 551 RVA: 0x00009D84 File Offset: 0x00007F84
		// (set) Token: 0x06000228 RID: 552 RVA: 0x00009D9B File Offset: 0x00007F9B
		public string ApplicationName { get; set; }

		// Token: 0x170000AC RID: 172
		// (get) Token: 0x06000229 RID: 553 RVA: 0x00009DA4 File Offset: 0x00007FA4
		// (set) Token: 0x0600022A RID: 554 RVA: 0x00009DBB File Offset: 0x00007FBB
		public string ApplicationVersion { get; set; }

		// Token: 0x170000AD RID: 173
		// (get) Token: 0x0600022B RID: 555 RVA: 0x00009DC4 File Offset: 0x00007FC4
		// (set) Token: 0x0600022C RID: 556 RVA: 0x00009DDB File Offset: 0x00007FDB
		public string ApplicationVendorName { get; set; }

		// Token: 0x170000AE RID: 174
		// (get) Token: 0x0600022D RID: 557 RVA: 0x00009DE4 File Offset: 0x00007FE4
		// (set) Token: 0x0600022E RID: 558 RVA: 0x00009DFB File Offset: 0x00007FFB
		public string ProductType { get; set; }

		// Token: 0x170000AF RID: 175
		// (get) Token: 0x0600022F RID: 559 RVA: 0x00009E04 File Offset: 0x00008004
		// (set) Token: 0x06000230 RID: 560 RVA: 0x00009E1B File Offset: 0x0000801B
		public string ProductCode { get; set; }

		// Token: 0x170000B0 RID: 176
		// (get) Token: 0x06000231 RID: 561 RVA: 0x00009E24 File Offset: 0x00008024
		// (set) Token: 0x06000232 RID: 562 RVA: 0x00009E3B File Offset: 0x0000803B
		public string BasicProductCode { get; set; }

		// Token: 0x170000B1 RID: 177
		// (get) Token: 0x06000233 RID: 563 RVA: 0x00009E44 File Offset: 0x00008044
		// (set) Token: 0x06000234 RID: 564 RVA: 0x00009E5B File Offset: 0x0000805B
		public string HwId { get; set; }

		// Token: 0x170000B2 RID: 178
		// (get) Token: 0x06000235 RID: 565 RVA: 0x00009E64 File Offset: 0x00008064
		// (set) Token: 0x06000236 RID: 566 RVA: 0x00009E7B File Offset: 0x0000807B
		public string Imei { get; set; }

		// Token: 0x170000B3 RID: 179
		// (get) Token: 0x06000237 RID: 567 RVA: 0x00009E84 File Offset: 0x00008084
		// (set) Token: 0x06000238 RID: 568 RVA: 0x00009E9B File Offset: 0x0000809B
		public string Imsi { get; set; }

		// Token: 0x170000B4 RID: 180
		// (get) Token: 0x06000239 RID: 569 RVA: 0x00009EA4 File Offset: 0x000080A4
		// (set) Token: 0x0600023A RID: 570 RVA: 0x00009EBB File Offset: 0x000080BB
		public string ActionDescription { get; set; }

		// Token: 0x170000B5 RID: 181
		// (get) Token: 0x0600023B RID: 571 RVA: 0x00009EC4 File Offset: 0x000080C4
		// (set) Token: 0x0600023C RID: 572 RVA: 0x00009EDB File Offset: 0x000080DB
		public string FlashDevice { get; set; }

		// Token: 0x170000B6 RID: 182
		// (get) Token: 0x0600023D RID: 573 RVA: 0x00009EE4 File Offset: 0x000080E4
		// (set) Token: 0x0600023E RID: 574 RVA: 0x00009EFB File Offset: 0x000080FB
		public long Duration { get; set; }

		// Token: 0x170000B7 RID: 183
		// (get) Token: 0x0600023F RID: 575 RVA: 0x00009F04 File Offset: 0x00008104
		// (set) Token: 0x06000240 RID: 576 RVA: 0x00009F1B File Offset: 0x0000811B
		public long DownloadDuration { get; set; }

		// Token: 0x170000B8 RID: 184
		// (get) Token: 0x06000241 RID: 577 RVA: 0x00009F24 File Offset: 0x00008124
		// (set) Token: 0x06000242 RID: 578 RVA: 0x00009F3B File Offset: 0x0000813B
		public long UpdateDuration { get; set; }

		// Token: 0x170000B9 RID: 185
		// (get) Token: 0x06000243 RID: 579 RVA: 0x00009F44 File Offset: 0x00008144
		// (set) Token: 0x06000244 RID: 580 RVA: 0x00009F5B File Offset: 0x0000815B
		public string FirmwareVersionOld { get; set; }

		// Token: 0x170000BA RID: 186
		// (get) Token: 0x06000245 RID: 581 RVA: 0x00009F64 File Offset: 0x00008164
		// (set) Token: 0x06000246 RID: 582 RVA: 0x00009F7B File Offset: 0x0000817B
		public string FirmwareVersionNew { get; set; }

		// Token: 0x170000BB RID: 187
		// (get) Token: 0x06000247 RID: 583 RVA: 0x00009F84 File Offset: 0x00008184
		// (set) Token: 0x06000248 RID: 584 RVA: 0x00009F9B File Offset: 0x0000819B
		public string FwGrading { get; set; }

		// Token: 0x170000BC RID: 188
		// (get) Token: 0x06000249 RID: 585 RVA: 0x00009FA4 File Offset: 0x000081A4
		// (set) Token: 0x0600024A RID: 586 RVA: 0x00009FBB File Offset: 0x000081BB
		public string RdInfo { get; set; }

		// Token: 0x170000BD RID: 189
		// (get) Token: 0x0600024B RID: 587 RVA: 0x00009FC4 File Offset: 0x000081C4
		// (set) Token: 0x0600024C RID: 588 RVA: 0x00009FDB File Offset: 0x000081DB
		public string CurrentMode { get; set; }

		// Token: 0x170000BE RID: 190
		// (get) Token: 0x0600024D RID: 589 RVA: 0x00009FE4 File Offset: 0x000081E4
		// (set) Token: 0x0600024E RID: 590 RVA: 0x00009FFB File Offset: 0x000081FB
		public string TargetMode { get; set; }

		// Token: 0x170000BF RID: 191
		// (get) Token: 0x0600024F RID: 591 RVA: 0x0000A004 File Offset: 0x00008204
		// (set) Token: 0x06000250 RID: 592 RVA: 0x0000A01B File Offset: 0x0000821B
		public string LanguagePackageOld { get; set; }

		// Token: 0x170000C0 RID: 192
		// (get) Token: 0x06000251 RID: 593 RVA: 0x0000A024 File Offset: 0x00008224
		// (set) Token: 0x06000252 RID: 594 RVA: 0x0000A03B File Offset: 0x0000823B
		public string LanguagePackageNew { get; set; }

		// Token: 0x170000C1 RID: 193
		// (get) Token: 0x06000253 RID: 595 RVA: 0x0000A044 File Offset: 0x00008244
		// (set) Token: 0x06000254 RID: 596 RVA: 0x0000A05B File Offset: 0x0000825B
		public long Uri { get; set; }

		// Token: 0x170000C2 RID: 194
		// (get) Token: 0x06000255 RID: 597 RVA: 0x0000A064 File Offset: 0x00008264
		// (set) Token: 0x06000256 RID: 598 RVA: 0x0000A07B File Offset: 0x0000827B
		public string UriDescription { get; set; }

		// Token: 0x170000C3 RID: 195
		// (get) Token: 0x06000257 RID: 599 RVA: 0x0000A084 File Offset: 0x00008284
		// (set) Token: 0x06000258 RID: 600 RVA: 0x0000A09B File Offset: 0x0000829B
		public string ApiError { get; set; }

		// Token: 0x170000C4 RID: 196
		// (get) Token: 0x06000259 RID: 601 RVA: 0x0000A0A4 File Offset: 0x000082A4
		// (set) Token: 0x0600025A RID: 602 RVA: 0x0000A0BB File Offset: 0x000082BB
		public string ApiErrorText { get; set; }

		// Token: 0x170000C5 RID: 197
		// (get) Token: 0x0600025B RID: 603 RVA: 0x0000A0C4 File Offset: 0x000082C4
		// (set) Token: 0x0600025C RID: 604 RVA: 0x0000A0DB File Offset: 0x000082DB
		public string DebugField { get; set; }

		// Token: 0x170000C6 RID: 198
		// (get) Token: 0x0600025D RID: 605 RVA: 0x0000A0E4 File Offset: 0x000082E4
		// (set) Token: 0x0600025E RID: 606 RVA: 0x0000A0FB File Offset: 0x000082FB
		public string Ext1 { get; set; }

		// Token: 0x170000C7 RID: 199
		// (get) Token: 0x0600025F RID: 607 RVA: 0x0000A104 File Offset: 0x00008304
		// (set) Token: 0x06000260 RID: 608 RVA: 0x0000A11B File Offset: 0x0000831B
		public string Ext2 { get; set; }

		// Token: 0x170000C8 RID: 200
		// (get) Token: 0x06000261 RID: 609 RVA: 0x0000A124 File Offset: 0x00008324
		// (set) Token: 0x06000262 RID: 610 RVA: 0x0000A13B File Offset: 0x0000833B
		public string Ext3 { get; set; }

		// Token: 0x170000C9 RID: 201
		// (get) Token: 0x06000263 RID: 611 RVA: 0x0000A144 File Offset: 0x00008344
		// (set) Token: 0x06000264 RID: 612 RVA: 0x0000A15B File Offset: 0x0000835B
		public string Ext4 { get; set; }

		// Token: 0x170000CA RID: 202
		// (get) Token: 0x06000265 RID: 613 RVA: 0x0000A164 File Offset: 0x00008364
		// (set) Token: 0x06000266 RID: 614 RVA: 0x0000A17B File Offset: 0x0000837B
		public string Ext5 { get; set; }

		// Token: 0x170000CB RID: 203
		// (get) Token: 0x06000267 RID: 615 RVA: 0x0000A184 File Offset: 0x00008384
		// (set) Token: 0x06000268 RID: 616 RVA: 0x0000A19B File Offset: 0x0000839B
		public string Ext6 { get; set; }

		// Token: 0x170000CC RID: 204
		// (get) Token: 0x06000269 RID: 617 RVA: 0x0000A1A4 File Offset: 0x000083A4
		// (set) Token: 0x0600026A RID: 618 RVA: 0x0000A1BB File Offset: 0x000083BB
		public string Ext7 { get; set; }

		// Token: 0x170000CD RID: 205
		// (get) Token: 0x0600026B RID: 619 RVA: 0x0000A1C4 File Offset: 0x000083C4
		// (set) Token: 0x0600026C RID: 620 RVA: 0x0000A1DB File Offset: 0x000083DB
		public string Ext8 { get; set; }

		// Token: 0x170000CE RID: 206
		// (get) Token: 0x0600026D RID: 621 RVA: 0x0000A1E4 File Offset: 0x000083E4
		// (set) Token: 0x0600026E RID: 622 RVA: 0x0000A1FB File Offset: 0x000083FB
		public string Ext9 { get; set; }

		// Token: 0x170000CF RID: 207
		// (get) Token: 0x0600026F RID: 623 RVA: 0x0000A204 File Offset: 0x00008404
		// (set) Token: 0x06000270 RID: 624 RVA: 0x0000A21B File Offset: 0x0000841B
		public string ServiceLayerInfo { get; set; }

		// Token: 0x170000D0 RID: 208
		// (get) Token: 0x06000271 RID: 625 RVA: 0x0000A224 File Offset: 0x00008424
		// (set) Token: 0x06000272 RID: 626 RVA: 0x0000A23B File Offset: 0x0000843B
		public string SystemInfo { get; set; }

		// Token: 0x170000D1 RID: 209
		// (get) Token: 0x06000273 RID: 627 RVA: 0x0000A244 File Offset: 0x00008444
		// (set) Token: 0x06000274 RID: 628 RVA: 0x0000A25B File Offset: 0x0000845B
		public string FlashDeviceInfo { get; set; }
	}
}
