using System;
using System.Data.Services.Common;

namespace System.Data.Services.Client
{
	// Token: 0x02000087 RID: 135
	internal class ResponseInfo
	{
		// Token: 0x060004C3 RID: 1219 RVA: 0x00013BB2 File Offset: 0x00011DB2
		internal ResponseInfo(RequestInfo requestInfo, MergeOption mergeOption)
		{
			this.requestInfo = requestInfo;
			this.mergeOption = mergeOption;
			this.ReadHelper = new ODataMessageReadingHelper(this);
		}

		// Token: 0x1700013B RID: 315
		// (get) Token: 0x060004C4 RID: 1220 RVA: 0x00013BD4 File Offset: 0x00011DD4
		// (set) Token: 0x060004C5 RID: 1221 RVA: 0x00013BDC File Offset: 0x00011DDC
		public ODataMessageReadingHelper ReadHelper { get; private set; }

		// Token: 0x1700013C RID: 316
		// (get) Token: 0x060004C6 RID: 1222 RVA: 0x00013BE5 File Offset: 0x00011DE5
		internal bool IsContinuation
		{
			get
			{
				return this.requestInfo.IsContinuation;
			}
		}

		// Token: 0x1700013D RID: 317
		// (get) Token: 0x060004C7 RID: 1223 RVA: 0x00013BF2 File Offset: 0x00011DF2
		internal Uri TypeScheme
		{
			get
			{
				return this.Context.TypeScheme;
			}
		}

		// Token: 0x1700013E RID: 318
		// (get) Token: 0x060004C8 RID: 1224 RVA: 0x00013BFF File Offset: 0x00011DFF
		internal string DataNamespace
		{
			get
			{
				return this.Context.DataNamespace;
			}
		}

		// Token: 0x1700013F RID: 319
		// (get) Token: 0x060004C9 RID: 1225 RVA: 0x00013C0C File Offset: 0x00011E0C
		internal MergeOption MergeOption
		{
			get
			{
				return this.mergeOption;
			}
		}

		// Token: 0x17000140 RID: 320
		// (get) Token: 0x060004CA RID: 1226 RVA: 0x00013C14 File Offset: 0x00011E14
		internal bool IgnoreMissingProperties
		{
			get
			{
				return this.Context.IgnoreMissingProperties;
			}
		}

		// Token: 0x17000141 RID: 321
		// (get) Token: 0x060004CB RID: 1227 RVA: 0x00013C21 File Offset: 0x00011E21
		internal EntityTracker EntityTracker
		{
			get
			{
				return this.Context.EntityTracker;
			}
		}

		// Token: 0x17000142 RID: 322
		// (get) Token: 0x060004CC RID: 1228 RVA: 0x00013C2E File Offset: 0x00011E2E
		// (set) Token: 0x060004CD RID: 1229 RVA: 0x00013C3B File Offset: 0x00011E3B
		internal bool ApplyingChanges
		{
			get
			{
				return this.Context.ApplyingChanges;
			}
			set
			{
				this.Context.ApplyingChanges = value;
			}
		}

		// Token: 0x17000143 RID: 323
		// (get) Token: 0x060004CE RID: 1230 RVA: 0x00013C49 File Offset: 0x00011E49
		internal TypeResolver TypeResolver
		{
			get
			{
				return this.requestInfo.TypeResolver;
			}
		}

		// Token: 0x17000144 RID: 324
		// (get) Token: 0x060004CF RID: 1231 RVA: 0x00013C56 File Offset: 0x00011E56
		internal UriResolver BaseUriResolver
		{
			get
			{
				return this.requestInfo.BaseUriResolver;
			}
		}

		// Token: 0x17000145 RID: 325
		// (get) Token: 0x060004D0 RID: 1232 RVA: 0x00013C63 File Offset: 0x00011E63
		internal DataServiceProtocolVersion MaxProtocolVersion
		{
			get
			{
				return this.Context.MaxProtocolVersion;
			}
		}

		// Token: 0x17000146 RID: 326
		// (get) Token: 0x060004D1 RID: 1233 RVA: 0x00013C70 File Offset: 0x00011E70
		internal ClientEdmModel Model
		{
			get
			{
				return this.requestInfo.Model;
			}
		}

		// Token: 0x17000147 RID: 327
		// (get) Token: 0x060004D2 RID: 1234 RVA: 0x00013C7D File Offset: 0x00011E7D
		internal DataServiceContext Context
		{
			get
			{
				return this.requestInfo.Context;
			}
		}

		// Token: 0x17000148 RID: 328
		// (get) Token: 0x060004D3 RID: 1235 RVA: 0x00013C8A File Offset: 0x00011E8A
		internal DataServiceClientResponsePipelineConfiguration ResponsePipeline
		{
			get
			{
				return this.requestInfo.Configurations.ResponsePipeline;
			}
		}

		// Token: 0x040002EE RID: 750
		private readonly RequestInfo requestInfo;

		// Token: 0x040002EF RID: 751
		private readonly MergeOption mergeOption;
	}
}
