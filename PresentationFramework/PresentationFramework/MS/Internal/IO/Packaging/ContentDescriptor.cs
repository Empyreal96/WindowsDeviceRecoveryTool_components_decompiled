using System;

namespace MS.Internal.IO.Packaging
{
	// Token: 0x0200065E RID: 1630
	internal class ContentDescriptor
	{
		// Token: 0x06006C22 RID: 27682 RVA: 0x001F1DC6 File Offset: 0x001EFFC6
		internal ContentDescriptor(bool hasIndexableContent, bool isInline, string contentProp, string titleProp)
		{
			this.HasIndexableContent = hasIndexableContent;
			this.IsInline = isInline;
			this.ContentProp = contentProp;
			this.TitleProp = titleProp;
		}

		// Token: 0x06006C23 RID: 27683 RVA: 0x001F1DEB File Offset: 0x001EFFEB
		internal ContentDescriptor(bool hasIndexableContent)
		{
			this.HasIndexableContent = hasIndexableContent;
			this.IsInline = false;
			this.ContentProp = null;
			this.TitleProp = null;
		}

		// Token: 0x170019DB RID: 6619
		// (get) Token: 0x06006C24 RID: 27684 RVA: 0x001F1E0F File Offset: 0x001F000F
		// (set) Token: 0x06006C25 RID: 27685 RVA: 0x001F1E17 File Offset: 0x001F0017
		internal bool HasIndexableContent
		{
			get
			{
				return this._hasIndexableContent;
			}
			set
			{
				this._hasIndexableContent = value;
			}
		}

		// Token: 0x170019DC RID: 6620
		// (get) Token: 0x06006C26 RID: 27686 RVA: 0x001F1E20 File Offset: 0x001F0020
		// (set) Token: 0x06006C27 RID: 27687 RVA: 0x001F1E28 File Offset: 0x001F0028
		internal bool IsInline
		{
			get
			{
				return this._isInline;
			}
			set
			{
				this._isInline = value;
			}
		}

		// Token: 0x170019DD RID: 6621
		// (get) Token: 0x06006C28 RID: 27688 RVA: 0x001F1E31 File Offset: 0x001F0031
		// (set) Token: 0x06006C29 RID: 27689 RVA: 0x001F1E39 File Offset: 0x001F0039
		internal string ContentProp
		{
			get
			{
				return this._contentProp;
			}
			set
			{
				this._contentProp = value;
			}
		}

		// Token: 0x170019DE RID: 6622
		// (get) Token: 0x06006C2A RID: 27690 RVA: 0x001F1E42 File Offset: 0x001F0042
		// (set) Token: 0x06006C2B RID: 27691 RVA: 0x001F1E4A File Offset: 0x001F004A
		internal string TitleProp
		{
			get
			{
				return this._titleProp;
			}
			set
			{
				this._titleProp = value;
			}
		}

		// Token: 0x04003511 RID: 13585
		internal const string ResourceKeyName = "Dictionary";

		// Token: 0x04003512 RID: 13586
		internal const string ResourceName = "ElementTable";

		// Token: 0x04003513 RID: 13587
		private bool _hasIndexableContent;

		// Token: 0x04003514 RID: 13588
		private bool _isInline;

		// Token: 0x04003515 RID: 13589
		private string _contentProp;

		// Token: 0x04003516 RID: 13590
		private string _titleProp;
	}
}
