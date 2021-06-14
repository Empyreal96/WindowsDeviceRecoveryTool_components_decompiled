using System;

namespace System.Data.Services.Client
{
	// Token: 0x0200001F RID: 31
	public abstract class Descriptor
	{
		// Token: 0x060000AC RID: 172 RVA: 0x00004C43 File Offset: 0x00002E43
		internal Descriptor(EntityStates state)
		{
			this.state = state;
		}

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x060000AD RID: 173 RVA: 0x00004C59 File Offset: 0x00002E59
		// (set) Token: 0x060000AE RID: 174 RVA: 0x00004C61 File Offset: 0x00002E61
		public EntityStates State
		{
			get
			{
				return this.state;
			}
			internal set
			{
				this.state = value;
			}
		}

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x060000AF RID: 175
		internal abstract DescriptorKind DescriptorKind { get; }

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x060000B0 RID: 176 RVA: 0x00004C6A File Offset: 0x00002E6A
		// (set) Token: 0x060000B1 RID: 177 RVA: 0x00004C72 File Offset: 0x00002E72
		internal uint ChangeOrder
		{
			get
			{
				return this.changeOrder;
			}
			set
			{
				this.changeOrder = value;
			}
		}

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x060000B2 RID: 178 RVA: 0x00004C7B File Offset: 0x00002E7B
		// (set) Token: 0x060000B3 RID: 179 RVA: 0x00004C83 File Offset: 0x00002E83
		internal bool ContentGeneratedForSave
		{
			get
			{
				return this.saveContentGenerated;
			}
			set
			{
				this.saveContentGenerated = value;
			}
		}

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x060000B4 RID: 180 RVA: 0x00004C8C File Offset: 0x00002E8C
		// (set) Token: 0x060000B5 RID: 181 RVA: 0x00004C94 File Offset: 0x00002E94
		internal EntityStates SaveResultWasProcessed
		{
			get
			{
				return this.saveResultProcessed;
			}
			set
			{
				this.saveResultProcessed = value;
			}
		}

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x060000B6 RID: 182 RVA: 0x00004C9D File Offset: 0x00002E9D
		// (set) Token: 0x060000B7 RID: 183 RVA: 0x00004CA5 File Offset: 0x00002EA5
		internal Exception SaveError
		{
			get
			{
				return this.saveError;
			}
			set
			{
				this.saveError = value;
			}
		}

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x060000B8 RID: 184 RVA: 0x00004CAE File Offset: 0x00002EAE
		internal virtual bool IsModified
		{
			get
			{
				return EntityStates.Unchanged != this.state;
			}
		}

		// Token: 0x060000B9 RID: 185
		internal abstract void ClearChanges();

		// Token: 0x0400019B RID: 411
		private uint changeOrder = uint.MaxValue;

		// Token: 0x0400019C RID: 412
		private bool saveContentGenerated;

		// Token: 0x0400019D RID: 413
		private EntityStates saveResultProcessed;

		// Token: 0x0400019E RID: 414
		private Exception saveError;

		// Token: 0x0400019F RID: 415
		private EntityStates state;
	}
}
