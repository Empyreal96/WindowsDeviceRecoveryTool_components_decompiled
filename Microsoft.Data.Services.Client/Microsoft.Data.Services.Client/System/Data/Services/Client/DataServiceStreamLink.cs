using System;
using System.ComponentModel;

namespace System.Data.Services.Client
{
	// Token: 0x0200005E RID: 94
	public sealed class DataServiceStreamLink : INotifyPropertyChanged
	{
		// Token: 0x06000314 RID: 788 RVA: 0x0000DF51 File Offset: 0x0000C151
		internal DataServiceStreamLink(string name)
		{
			this.name = name;
		}

		// Token: 0x14000002 RID: 2
		// (add) Token: 0x06000315 RID: 789 RVA: 0x0000DF60 File Offset: 0x0000C160
		// (remove) Token: 0x06000316 RID: 790 RVA: 0x0000DF98 File Offset: 0x0000C198
		public event PropertyChangedEventHandler PropertyChanged;

		// Token: 0x170000CB RID: 203
		// (get) Token: 0x06000317 RID: 791 RVA: 0x0000DFCD File Offset: 0x0000C1CD
		public string Name
		{
			get
			{
				return this.name;
			}
		}

		// Token: 0x170000CC RID: 204
		// (get) Token: 0x06000318 RID: 792 RVA: 0x0000DFD5 File Offset: 0x0000C1D5
		// (set) Token: 0x06000319 RID: 793 RVA: 0x0000DFDD File Offset: 0x0000C1DD
		public Uri SelfLink
		{
			get
			{
				return this.selfLink;
			}
			internal set
			{
				this.selfLink = value;
				this.OnPropertyChanged("SelfLink");
			}
		}

		// Token: 0x170000CD RID: 205
		// (get) Token: 0x0600031A RID: 794 RVA: 0x0000DFF1 File Offset: 0x0000C1F1
		// (set) Token: 0x0600031B RID: 795 RVA: 0x0000DFF9 File Offset: 0x0000C1F9
		public Uri EditLink
		{
			get
			{
				return this.editLink;
			}
			internal set
			{
				this.editLink = value;
				this.OnPropertyChanged("EditLink");
			}
		}

		// Token: 0x170000CE RID: 206
		// (get) Token: 0x0600031C RID: 796 RVA: 0x0000E00D File Offset: 0x0000C20D
		// (set) Token: 0x0600031D RID: 797 RVA: 0x0000E015 File Offset: 0x0000C215
		public string ContentType
		{
			get
			{
				return this.contentType;
			}
			internal set
			{
				this.contentType = value;
				this.OnPropertyChanged("ContentType");
			}
		}

		// Token: 0x170000CF RID: 207
		// (get) Token: 0x0600031E RID: 798 RVA: 0x0000E029 File Offset: 0x0000C229
		// (set) Token: 0x0600031F RID: 799 RVA: 0x0000E031 File Offset: 0x0000C231
		public string ETag
		{
			get
			{
				return this.etag;
			}
			internal set
			{
				this.etag = value;
				this.OnPropertyChanged("ETag");
			}
		}

		// Token: 0x06000320 RID: 800 RVA: 0x0000E045 File Offset: 0x0000C245
		private void OnPropertyChanged(string propertyName)
		{
			if (this.PropertyChanged != null)
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		// Token: 0x04000281 RID: 641
		private readonly string name;

		// Token: 0x04000282 RID: 642
		private Uri selfLink;

		// Token: 0x04000283 RID: 643
		private Uri editLink;

		// Token: 0x04000284 RID: 644
		private string contentType;

		// Token: 0x04000285 RID: 645
		private string etag;
	}
}
