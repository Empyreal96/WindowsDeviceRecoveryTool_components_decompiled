using System;

namespace Microsoft.Data.Edm.Csdl.Internal.Parsing.Common
{
	// Token: 0x0200015E RID: 350
	internal class XmlTextValue : XmlElementValue<string>
	{
		// Token: 0x060006E6 RID: 1766 RVA: 0x000118B6 File Offset: 0x0000FAB6
		internal XmlTextValue(CsdlLocation textLocation, string textValue) : base("<\"Text\">", textLocation, textValue)
		{
		}

		// Token: 0x170002DB RID: 731
		// (get) Token: 0x060006E7 RID: 1767 RVA: 0x000118C5 File Offset: 0x0000FAC5
		internal override bool IsText
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170002DC RID: 732
		// (get) Token: 0x060006E8 RID: 1768 RVA: 0x000118C8 File Offset: 0x0000FAC8
		internal override string TextValue
		{
			get
			{
				return base.Value;
			}
		}

		// Token: 0x04000390 RID: 912
		internal const string ElementName = "<\"Text\">";

		// Token: 0x04000391 RID: 913
		internal static readonly XmlTextValue Missing = new XmlTextValue(null, null);
	}
}
