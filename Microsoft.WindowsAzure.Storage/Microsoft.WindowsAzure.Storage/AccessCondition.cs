using System;

namespace Microsoft.WindowsAzure.Storage
{
	// Token: 0x0200006E RID: 110
	public sealed class AccessCondition
	{
		// Token: 0x1700017E RID: 382
		// (get) Token: 0x06000E0D RID: 3597 RVA: 0x000368D1 File Offset: 0x00034AD1
		// (set) Token: 0x06000E0E RID: 3598 RVA: 0x000368D9 File Offset: 0x00034AD9
		public string IfMatchETag { get; set; }

		// Token: 0x1700017F RID: 383
		// (get) Token: 0x06000E0F RID: 3599 RVA: 0x000368E2 File Offset: 0x00034AE2
		// (set) Token: 0x06000E10 RID: 3600 RVA: 0x000368EA File Offset: 0x00034AEA
		public string IfNoneMatchETag { get; set; }

		// Token: 0x17000180 RID: 384
		// (get) Token: 0x06000E11 RID: 3601 RVA: 0x000368F3 File Offset: 0x00034AF3
		// (set) Token: 0x06000E12 RID: 3602 RVA: 0x000368FC File Offset: 0x00034AFC
		public DateTimeOffset? IfModifiedSinceTime
		{
			get
			{
				return this.ifModifiedSinceDateTime;
			}
			set
			{
				this.ifModifiedSinceDateTime = ((value != null) ? new DateTimeOffset?(value.Value.ToUniversalTime()) : value);
			}
		}

		// Token: 0x17000181 RID: 385
		// (get) Token: 0x06000E13 RID: 3603 RVA: 0x0003692F File Offset: 0x00034B2F
		// (set) Token: 0x06000E14 RID: 3604 RVA: 0x00036938 File Offset: 0x00034B38
		public DateTimeOffset? IfNotModifiedSinceTime
		{
			get
			{
				return this.ifNotModifiedSinceDateTime;
			}
			set
			{
				this.ifNotModifiedSinceDateTime = ((value != null) ? new DateTimeOffset?(value.Value.ToUniversalTime()) : value);
			}
		}

		// Token: 0x17000182 RID: 386
		// (get) Token: 0x06000E15 RID: 3605 RVA: 0x0003696B File Offset: 0x00034B6B
		// (set) Token: 0x06000E16 RID: 3606 RVA: 0x00036973 File Offset: 0x00034B73
		public long? IfMaxSizeLessThanOrEqual { get; set; }

		// Token: 0x17000183 RID: 387
		// (get) Token: 0x06000E17 RID: 3607 RVA: 0x0003697C File Offset: 0x00034B7C
		// (set) Token: 0x06000E18 RID: 3608 RVA: 0x00036984 File Offset: 0x00034B84
		public long? IfAppendPositionEqual { get; set; }

		// Token: 0x17000184 RID: 388
		// (get) Token: 0x06000E19 RID: 3609 RVA: 0x0003698D File Offset: 0x00034B8D
		// (set) Token: 0x06000E1A RID: 3610 RVA: 0x00036995 File Offset: 0x00034B95
		public long? IfSequenceNumberLessThanOrEqual { get; set; }

		// Token: 0x17000185 RID: 389
		// (get) Token: 0x06000E1B RID: 3611 RVA: 0x0003699E File Offset: 0x00034B9E
		// (set) Token: 0x06000E1C RID: 3612 RVA: 0x000369A6 File Offset: 0x00034BA6
		public long? IfSequenceNumberLessThan { get; set; }

		// Token: 0x17000186 RID: 390
		// (get) Token: 0x06000E1D RID: 3613 RVA: 0x000369AF File Offset: 0x00034BAF
		// (set) Token: 0x06000E1E RID: 3614 RVA: 0x000369B7 File Offset: 0x00034BB7
		public long? IfSequenceNumberEqual { get; set; }

		// Token: 0x17000187 RID: 391
		// (get) Token: 0x06000E1F RID: 3615 RVA: 0x000369C0 File Offset: 0x00034BC0
		// (set) Token: 0x06000E20 RID: 3616 RVA: 0x000369C8 File Offset: 0x00034BC8
		public string LeaseId { get; set; }

		// Token: 0x17000188 RID: 392
		// (get) Token: 0x06000E21 RID: 3617 RVA: 0x000369D4 File Offset: 0x00034BD4
		internal bool IsConditional
		{
			get
			{
				return !string.IsNullOrEmpty(this.IfMatchETag) || !string.IsNullOrEmpty(this.IfNoneMatchETag) || this.IfModifiedSinceTime != null || this.IfNotModifiedSinceTime != null;
			}
		}

		// Token: 0x06000E22 RID: 3618 RVA: 0x00036A1B File Offset: 0x00034C1B
		public static AccessCondition GenerateEmptyCondition()
		{
			return new AccessCondition();
		}

		// Token: 0x06000E23 RID: 3619 RVA: 0x00036A24 File Offset: 0x00034C24
		public static AccessCondition GenerateIfNotExistsCondition()
		{
			return new AccessCondition
			{
				IfNoneMatchETag = "*"
			};
		}

		// Token: 0x06000E24 RID: 3620 RVA: 0x00036A44 File Offset: 0x00034C44
		public static AccessCondition GenerateIfExistsCondition()
		{
			return new AccessCondition
			{
				IfMatchETag = "*"
			};
		}

		// Token: 0x06000E25 RID: 3621 RVA: 0x00036A64 File Offset: 0x00034C64
		public static AccessCondition GenerateIfMatchCondition(string etag)
		{
			return new AccessCondition
			{
				IfMatchETag = etag
			};
		}

		// Token: 0x06000E26 RID: 3622 RVA: 0x00036A80 File Offset: 0x00034C80
		public static AccessCondition GenerateIfModifiedSinceCondition(DateTimeOffset modifiedTime)
		{
			return new AccessCondition
			{
				IfModifiedSinceTime = new DateTimeOffset?(modifiedTime)
			};
		}

		// Token: 0x06000E27 RID: 3623 RVA: 0x00036AA0 File Offset: 0x00034CA0
		public static AccessCondition GenerateIfNoneMatchCondition(string etag)
		{
			return new AccessCondition
			{
				IfNoneMatchETag = etag
			};
		}

		// Token: 0x06000E28 RID: 3624 RVA: 0x00036ABC File Offset: 0x00034CBC
		public static AccessCondition GenerateIfNotModifiedSinceCondition(DateTimeOffset modifiedTime)
		{
			return new AccessCondition
			{
				IfNotModifiedSinceTime = new DateTimeOffset?(modifiedTime)
			};
		}

		// Token: 0x06000E29 RID: 3625 RVA: 0x00036ADC File Offset: 0x00034CDC
		public static AccessCondition GenerateIfMaxSizeLessThanOrEqualCondition(long maxSize)
		{
			return new AccessCondition
			{
				IfMaxSizeLessThanOrEqual = new long?(maxSize)
			};
		}

		// Token: 0x06000E2A RID: 3626 RVA: 0x00036AFC File Offset: 0x00034CFC
		public static AccessCondition GenerateIfAppendPositionEqualCondition(long appendPosition)
		{
			return new AccessCondition
			{
				IfAppendPositionEqual = new long?(appendPosition)
			};
		}

		// Token: 0x06000E2B RID: 3627 RVA: 0x00036B1C File Offset: 0x00034D1C
		public static AccessCondition GenerateIfSequenceNumberLessThanOrEqualCondition(long sequenceNumber)
		{
			return new AccessCondition
			{
				IfSequenceNumberLessThanOrEqual = new long?(sequenceNumber)
			};
		}

		// Token: 0x06000E2C RID: 3628 RVA: 0x00036B3C File Offset: 0x00034D3C
		public static AccessCondition GenerateIfSequenceNumberLessThanCondition(long sequenceNumber)
		{
			return new AccessCondition
			{
				IfSequenceNumberLessThan = new long?(sequenceNumber)
			};
		}

		// Token: 0x06000E2D RID: 3629 RVA: 0x00036B5C File Offset: 0x00034D5C
		public static AccessCondition GenerateIfSequenceNumberEqualCondition(long sequenceNumber)
		{
			return new AccessCondition
			{
				IfSequenceNumberEqual = new long?(sequenceNumber)
			};
		}

		// Token: 0x06000E2E RID: 3630 RVA: 0x00036B7C File Offset: 0x00034D7C
		public static AccessCondition GenerateLeaseCondition(string leaseId)
		{
			return new AccessCondition
			{
				LeaseId = leaseId
			};
		}

		// Token: 0x06000E2F RID: 3631 RVA: 0x00036B98 File Offset: 0x00034D98
		internal static AccessCondition CloneConditionWithETag(AccessCondition accessCondition, string etag)
		{
			return new AccessCondition
			{
				IfMatchETag = etag,
				LeaseId = ((accessCondition != null) ? accessCondition.LeaseId : null)
			};
		}

		// Token: 0x040001F8 RID: 504
		private DateTimeOffset? ifModifiedSinceDateTime;

		// Token: 0x040001F9 RID: 505
		private DateTimeOffset? ifNotModifiedSinceDateTime;
	}
}
