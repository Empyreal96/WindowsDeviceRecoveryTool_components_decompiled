using System;
using System.ComponentModel;
using System.Xml.Serialization;
using Microsoft.WindowsPhone.FeatureAPI;
using Microsoft.WindowsPhone.ImageUpdate.PkgCommon;

namespace Microsoft.WindowsPhone.Imaging
{
	// Token: 0x02000028 RID: 40
	public class FeatureIdentifierPackage
	{
		// Token: 0x1700008B RID: 139
		// (get) Token: 0x060001D7 RID: 471 RVA: 0x0000B45C File Offset: 0x0000965C
		[XmlIgnore]
		public string FeatureIDWithFMID
		{
			get
			{
				return FeatureManifest.GetFeatureIDWithFMID(this.FeatureID, this.FMID);
			}
		}

		// Token: 0x060001D8 RID: 472 RVA: 0x0000B46F File Offset: 0x0000966F
		public FeatureIdentifierPackage()
		{
		}

		// Token: 0x060001D9 RID: 473 RVA: 0x0000B48C File Offset: 0x0000968C
		public FeatureIdentifierPackage(PublishingPackageInfo pkg)
		{
			this.ID = pkg.ID;
			this.Partition = pkg.Partition;
			this.ownerType = pkg.OwnerType;
			this.FeatureID = pkg.FeatureID;
			this.FMID = pkg.FMID;
		}

		// Token: 0x060001DA RID: 474 RVA: 0x0000B4F0 File Offset: 0x000096F0
		public override string ToString()
		{
			string text = string.Concat(new string[]
			{
				this.FeatureIDWithFMID,
				" : ",
				this.ID,
				":",
				this.Partition
			});
			switch (this.FixUpAction)
			{
			case FeatureIdentifierPackage.FixUpActions.Ignore:
				text = text + " (" + this.FixUpAction.ToString() + ")";
				break;
			case FeatureIdentifierPackage.FixUpActions.MoveToAnotherFeature:
			case FeatureIdentifierPackage.FixUpActions.AndFeature:
			{
				string text2 = text;
				text = string.Concat(new string[]
				{
					text2,
					" (",
					this.FixUpAction.ToString(),
					" = ",
					this.FixUpActionValue,
					")"
				});
				break;
			}
			}
			return text;
		}

		// Token: 0x04000127 RID: 295
		[XmlAttribute]
		public string FeatureID;

		// Token: 0x04000128 RID: 296
		[XmlAttribute]
		public string FMID;

		// Token: 0x04000129 RID: 297
		[XmlAttribute]
		public string ID;

		// Token: 0x0400012A RID: 298
		[XmlAttribute]
		[DefaultValue("MainOS")]
		public string Partition = "MainOS";

		// Token: 0x0400012B RID: 299
		[XmlAttribute("OwnerType")]
		[DefaultValue(1)]
		public OwnerType ownerType = 1;

		// Token: 0x0400012C RID: 300
		[DefaultValue(FeatureIdentifierPackage.FixUpActions.None)]
		[XmlAttribute]
		public FeatureIdentifierPackage.FixUpActions FixUpAction;

		// Token: 0x0400012D RID: 301
		[XmlAttribute]
		public string FixUpActionValue;

		// Token: 0x02000029 RID: 41
		public enum FixUpActions
		{
			// Token: 0x0400012F RID: 303
			None,
			// Token: 0x04000130 RID: 304
			Ignore,
			// Token: 0x04000131 RID: 305
			MoveToAnotherFeature,
			// Token: 0x04000132 RID: 306
			AndFeature
		}
	}
}
