using System;
using System.Security.AccessControl;
using System.Xml.Serialization;

namespace Microsoft.WindowsPhone.ImageUpdate.Tools.Common
{
	// Token: 0x02000053 RID: 83
	public class RegAclWithFullAcl : RegistryAcl
	{
		// Token: 0x060001DC RID: 476 RVA: 0x00009E98 File Offset: 0x00008098
		public RegAclWithFullAcl()
		{
		}

		// Token: 0x060001DD RID: 477 RVA: 0x00009EA0 File Offset: 0x000080A0
		public RegAclWithFullAcl(NativeObjectSecurity nos)
		{
			this.m_nos = nos;
		}

		// Token: 0x1700003F RID: 63
		// (get) Token: 0x060001DE RID: 478 RVA: 0x00009EAF File Offset: 0x000080AF
		// (set) Token: 0x060001DF RID: 479 RVA: 0x00009EB7 File Offset: 0x000080B7
		[XmlAttribute("FullACL")]
		public string FullRegACL
		{
			get
			{
				return base.FullACL;
			}
			set
			{
			}
		}
	}
}
