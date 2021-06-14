using System;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

namespace Microsoft.WindowsPhone.ImageUpdate.Tools.Common
{
	// Token: 0x02000052 RID: 82
	public class RegistryAcl : ResourceAcl
	{
		// Token: 0x060001D5 RID: 469 RVA: 0x00009CA3 File Offset: 0x00007EA3
		public RegistryAcl()
		{
		}

		// Token: 0x060001D6 RID: 470 RVA: 0x00009CAC File Offset: 0x00007EAC
		public RegistryAcl(ORRegistryKey key)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key");
			}
			this.m_key = key;
			this.m_nos = key.RegistrySecurity;
			this.m_path = key.FullName;
			this.m_fullPath = key.FullName;
		}

		// Token: 0x1700003C RID: 60
		// (get) Token: 0x060001D7 RID: 471 RVA: 0x00009CF8 File Offset: 0x00007EF8
		// (set) Token: 0x060001D8 RID: 472 RVA: 0x00009D6E File Offset: 0x00007F6E
		[XmlAttribute("SACL")]
		public override string MandatoryIntegrityLabel
		{
			get
			{
				if (this.m_nos != null)
				{
					this.m_macLabel = null;
					string text = SecurityUtils.ConvertSDToStringSD(this.m_nos.GetSecurityDescriptorBinaryForm(), (SecurityInformationFlags)24U);
					if (!string.IsNullOrEmpty(text))
					{
						Match match = ResourceAcl.regexExtractMIL.Match(text);
						if (match.Success)
						{
							Group group = match.Groups["MIL"];
							if (group != null)
							{
								this.m_macLabel = SddlNormalizer.FixAceSddl(group.Value);
							}
						}
					}
				}
				return this.m_macLabel;
			}
			set
			{
				this.m_macLabel = value;
			}
		}

		// Token: 0x1700003D RID: 61
		// (get) Token: 0x060001D9 RID: 473 RVA: 0x00009D78 File Offset: 0x00007F78
		public override NativeObjectSecurity ObjectSecurity
		{
			get
			{
				RegistrySecurity registrySecurity = null;
				if (this.m_nos != null)
				{
					registrySecurity = new RegistrySecurity();
					registrySecurity.SetSecurityDescriptorBinaryForm(this.m_nos.GetSecurityDescriptorBinaryForm());
				}
				return registrySecurity;
			}
		}

		// Token: 0x1700003E RID: 62
		// (get) Token: 0x060001DA RID: 474 RVA: 0x00009DA7 File Offset: 0x00007FA7
		protected override string TypeString
		{
			get
			{
				return "RegKey";
			}
		}

		// Token: 0x060001DB RID: 475 RVA: 0x00009DB0 File Offset: 0x00007FB0
		protected override string ComputeExplicitDACL()
		{
			RegistrySecurity registrySecurity = this.m_key.RegistrySecurity;
			AuthorizationRuleCollection accessRules = registrySecurity.GetAccessRules(true, false, typeof(NTAccount));
			int num = accessRules.Count;
			foreach (object obj in accessRules)
			{
				RegistryAccessRule registryAccessRule = (RegistryAccessRule)obj;
				if (registryAccessRule.IsInherited)
				{
					registrySecurity.RemoveAccessRule(registryAccessRule);
					num--;
				}
			}
			if (base.DACLProtected && registrySecurity.AreAccessRulesCanonical)
			{
				registrySecurity.SetAccessRuleProtection(true, base.PreserveInheritance);
			}
			string text = null;
			if (base.DACLProtected || num > 0)
			{
				text = registrySecurity.GetSecurityDescriptorSddlForm(AccessControlSections.Access);
				if (!string.IsNullOrEmpty(text))
				{
					text = ResourceAcl.regexStripDacl.Replace(text, string.Empty);
				}
			}
			return SddlNormalizer.FixAceSddl(text);
		}

		// Token: 0x04000140 RID: 320
		private ORRegistryKey m_key;
	}
}
