using System;

namespace System.Data.Services.Client
{
	// Token: 0x02000020 RID: 32
	public abstract class OperationDescriptor : Descriptor
	{
		// Token: 0x060000BA RID: 186 RVA: 0x00004CBC File Offset: 0x00002EBC
		internal OperationDescriptor() : base(EntityStates.Unchanged)
		{
		}

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x060000BB RID: 187 RVA: 0x00004CC5 File Offset: 0x00002EC5
		// (set) Token: 0x060000BC RID: 188 RVA: 0x00004CCD File Offset: 0x00002ECD
		public string Title
		{
			get
			{
				return this.title;
			}
			internal set
			{
				this.title = value;
			}
		}

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x060000BD RID: 189 RVA: 0x00004CD6 File Offset: 0x00002ED6
		// (set) Token: 0x060000BE RID: 190 RVA: 0x00004CDE File Offset: 0x00002EDE
		public Uri Metadata
		{
			get
			{
				return this.metadata;
			}
			internal set
			{
				this.metadata = value;
			}
		}

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x060000BF RID: 191 RVA: 0x00004CE7 File Offset: 0x00002EE7
		// (set) Token: 0x060000C0 RID: 192 RVA: 0x00004CEF File Offset: 0x00002EEF
		public Uri Target
		{
			get
			{
				return this.target;
			}
			internal set
			{
				this.target = value;
			}
		}

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x060000C1 RID: 193 RVA: 0x00004CF8 File Offset: 0x00002EF8
		internal override DescriptorKind DescriptorKind
		{
			get
			{
				return DescriptorKind.OperationDescriptor;
			}
		}

		// Token: 0x060000C2 RID: 194 RVA: 0x00004CFB File Offset: 0x00002EFB
		internal override void ClearChanges()
		{
		}

		// Token: 0x040001A0 RID: 416
		private string title;

		// Token: 0x040001A1 RID: 417
		private Uri metadata;

		// Token: 0x040001A2 RID: 418
		private Uri target;
	}
}
