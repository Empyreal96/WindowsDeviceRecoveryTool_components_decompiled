using System;
using System.ComponentModel;
using System.Xml.Serialization;
using Microsoft.WindowsPhone.ImageUpdate.PkgCommon;

namespace Microsoft.WindowsPhone.Imaging
{
	// Token: 0x02000027 RID: 39
	public class FMCollectionItem
	{
		// Token: 0x1700008A RID: 138
		// (get) Token: 0x060001D3 RID: 467 RVA: 0x0000B403 File Offset: 0x00009603
		// (set) Token: 0x060001D4 RID: 468 RVA: 0x0000B420 File Offset: 0x00009620
		[XmlAttribute("OwnerName")]
		[DefaultValue(null)]
		public string Owner
		{
			get
			{
				if (this.ownerType == 1)
				{
					return 1.ToString();
				}
				return this._owner;
			}
			set
			{
				if (this.ownerType == 1)
				{
					this._owner = null;
					return;
				}
				this._owner = value;
			}
		}

		// Token: 0x060001D5 RID: 469 RVA: 0x0000B43A File Offset: 0x0000963A
		public override string ToString()
		{
			return this.Path;
		}

		// Token: 0x0400011C RID: 284
		[XmlAttribute]
		public string Path;

		// Token: 0x0400011D RID: 285
		[XmlAttribute("ReleaseType")]
		public ReleaseType releaseType;

		// Token: 0x0400011E RID: 286
		[XmlAttribute]
		public bool UserInstallable;

		// Token: 0x0400011F RID: 287
		[XmlAttribute("OwnerType")]
		public OwnerType ownerType = 2;

		// Token: 0x04000120 RID: 288
		private string _owner;

		// Token: 0x04000121 RID: 289
		[XmlAttribute]
		public CpuId CPUType;

		// Token: 0x04000122 RID: 290
		[XmlAttribute]
		public bool SkipForPublishing;

		// Token: 0x04000123 RID: 291
		[XmlAttribute]
		public bool ValidateAsMicrosoftPhoneFM;

		// Token: 0x04000124 RID: 292
		[XmlAttribute]
		public bool Critical;

		// Token: 0x04000125 RID: 293
		[XmlAttribute]
		public string ID;

		// Token: 0x04000126 RID: 294
		[XmlAttribute]
		public Guid MicrosoftFMGUID = Guid.Empty;
	}
}
