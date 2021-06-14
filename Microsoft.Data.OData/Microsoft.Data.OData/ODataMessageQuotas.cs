using System;

namespace Microsoft.Data.OData
{
	// Token: 0x020001B9 RID: 441
	public sealed class ODataMessageQuotas
	{
		// Token: 0x06000DB8 RID: 3512 RVA: 0x0002FD60 File Offset: 0x0002DF60
		public ODataMessageQuotas()
		{
			this.maxPartsPerBatch = 100;
			this.maxOperationsPerChangeset = 1000;
			this.maxNestingDepth = 100;
			this.maxReceivedMessageSize = 1048576L;
			this.maxEntityPropertyMappingsPerType = 100;
		}

		// Token: 0x06000DB9 RID: 3513 RVA: 0x0002FD98 File Offset: 0x0002DF98
		public ODataMessageQuotas(ODataMessageQuotas other)
		{
			ExceptionUtils.CheckArgumentNotNull<ODataMessageQuotas>(other, "other");
			this.maxPartsPerBatch = other.maxPartsPerBatch;
			this.maxOperationsPerChangeset = other.maxOperationsPerChangeset;
			this.maxNestingDepth = other.maxNestingDepth;
			this.maxReceivedMessageSize = other.maxReceivedMessageSize;
			this.maxEntityPropertyMappingsPerType = other.maxEntityPropertyMappingsPerType;
		}

		// Token: 0x170002F1 RID: 753
		// (get) Token: 0x06000DBA RID: 3514 RVA: 0x0002FDF2 File Offset: 0x0002DFF2
		// (set) Token: 0x06000DBB RID: 3515 RVA: 0x0002FDFA File Offset: 0x0002DFFA
		public int MaxPartsPerBatch
		{
			get
			{
				return this.maxPartsPerBatch;
			}
			set
			{
				ExceptionUtils.CheckIntegerNotNegative(value, "MaxPartsPerBatch");
				this.maxPartsPerBatch = value;
			}
		}

		// Token: 0x170002F2 RID: 754
		// (get) Token: 0x06000DBC RID: 3516 RVA: 0x0002FE0E File Offset: 0x0002E00E
		// (set) Token: 0x06000DBD RID: 3517 RVA: 0x0002FE16 File Offset: 0x0002E016
		public int MaxOperationsPerChangeset
		{
			get
			{
				return this.maxOperationsPerChangeset;
			}
			set
			{
				ExceptionUtils.CheckIntegerNotNegative(value, "MaxOperationsPerChangeset");
				this.maxOperationsPerChangeset = value;
			}
		}

		// Token: 0x170002F3 RID: 755
		// (get) Token: 0x06000DBE RID: 3518 RVA: 0x0002FE2A File Offset: 0x0002E02A
		// (set) Token: 0x06000DBF RID: 3519 RVA: 0x0002FE32 File Offset: 0x0002E032
		public int MaxNestingDepth
		{
			get
			{
				return this.maxNestingDepth;
			}
			set
			{
				ExceptionUtils.CheckIntegerPositive(value, "MaxNestingDepth");
				this.maxNestingDepth = value;
			}
		}

		// Token: 0x170002F4 RID: 756
		// (get) Token: 0x06000DC0 RID: 3520 RVA: 0x0002FE46 File Offset: 0x0002E046
		// (set) Token: 0x06000DC1 RID: 3521 RVA: 0x0002FE4E File Offset: 0x0002E04E
		public long MaxReceivedMessageSize
		{
			get
			{
				return this.maxReceivedMessageSize;
			}
			set
			{
				ExceptionUtils.CheckLongPositive(value, "MaxReceivedMessageSize");
				this.maxReceivedMessageSize = value;
			}
		}

		// Token: 0x170002F5 RID: 757
		// (get) Token: 0x06000DC2 RID: 3522 RVA: 0x0002FE62 File Offset: 0x0002E062
		// (set) Token: 0x06000DC3 RID: 3523 RVA: 0x0002FE6A File Offset: 0x0002E06A
		public int MaxEntityPropertyMappingsPerType
		{
			get
			{
				return this.maxEntityPropertyMappingsPerType;
			}
			set
			{
				ExceptionUtils.CheckIntegerNotNegative(value, "MaxEntityPropertyMappingsPerType");
				this.maxEntityPropertyMappingsPerType = value;
			}
		}

		// Token: 0x04000492 RID: 1170
		private int maxPartsPerBatch;

		// Token: 0x04000493 RID: 1171
		private int maxOperationsPerChangeset;

		// Token: 0x04000494 RID: 1172
		private int maxNestingDepth;

		// Token: 0x04000495 RID: 1173
		private long maxReceivedMessageSize;

		// Token: 0x04000496 RID: 1174
		private int maxEntityPropertyMappingsPerType;
	}
}
