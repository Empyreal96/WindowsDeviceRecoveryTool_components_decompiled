using System;
using System.Windows;
using System.Windows.Markup;

namespace MS.Internal.Globalization
{
	// Token: 0x020006A1 RID: 1697
	internal sealed class BamlStartDocumentNode : BamlTreeNode, ILocalizabilityInheritable
	{
		// Token: 0x06006E54 RID: 28244 RVA: 0x001FC09D File Offset: 0x001FA29D
		internal BamlStartDocumentNode() : base(BamlNodeType.StartDocument)
		{
		}

		// Token: 0x06006E55 RID: 28245 RVA: 0x001FC0A6 File Offset: 0x001FA2A6
		internal override void Serialize(BamlWriter writer)
		{
			writer.WriteStartDocument();
		}

		// Token: 0x06006E56 RID: 28246 RVA: 0x001FC0AE File Offset: 0x001FA2AE
		internal override BamlTreeNode Copy()
		{
			return new BamlStartDocumentNode();
		}

		// Token: 0x17001A33 RID: 6707
		// (get) Token: 0x06006E57 RID: 28247 RVA: 0x0000C238 File Offset: 0x0000A438
		public ILocalizabilityInheritable LocalizabilityAncestor
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17001A34 RID: 6708
		// (get) Token: 0x06006E58 RID: 28248 RVA: 0x001FC0B8 File Offset: 0x001FA2B8
		// (set) Token: 0x06006E59 RID: 28249 RVA: 0x00002137 File Offset: 0x00000337
		public LocalizabilityAttribute InheritableAttribute
		{
			get
			{
				return new LocalizabilityAttribute(LocalizationCategory.None)
				{
					Readability = Readability.Readable,
					Modifiability = Modifiability.Modifiable
				};
			}
			set
			{
			}
		}

		// Token: 0x17001A35 RID: 6709
		// (get) Token: 0x06006E5A RID: 28250 RVA: 0x0000B02A File Offset: 0x0000922A
		// (set) Token: 0x06006E5B RID: 28251 RVA: 0x00002137 File Offset: 0x00000337
		public bool IsIgnored
		{
			get
			{
				return false;
			}
			set
			{
			}
		}
	}
}
