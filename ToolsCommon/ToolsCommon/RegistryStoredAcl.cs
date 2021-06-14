using System;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

namespace Microsoft.WindowsPhone.ImageUpdate.Tools.Common
{
	// Token: 0x02000054 RID: 84
	public class RegistryStoredAcl : ResourceAcl
	{
		// Token: 0x060001E0 RID: 480 RVA: 0x00009EB9 File Offset: 0x000080B9
		public RegistryStoredAcl()
		{
		}

		// Token: 0x060001E1 RID: 481 RVA: 0x00009ECC File Offset: 0x000080CC
		public RegistryStoredAcl(string typeName, string path, byte[] rawSecurityDescriptor)
		{
			if (rawSecurityDescriptor == null || string.IsNullOrEmpty(path) || string.IsNullOrEmpty(typeName))
			{
				throw new ArgumentNullException("SDRegValue");
			}
			RegistrySecurity registrySecurity = new RegistrySecurity();
			registrySecurity.SetSecurityDescriptorBinaryForm(rawSecurityDescriptor);
			this.m_rawsd = rawSecurityDescriptor;
			this.m_nos = registrySecurity;
			this.m_path = path;
			this.m_fullPath = path;
			this.m_typeName = typeName;
		}

		// Token: 0x17000040 RID: 64
		// (get) Token: 0x060001E2 RID: 482 RVA: 0x00009F38 File Offset: 0x00008138
		// (set) Token: 0x060001E3 RID: 483 RVA: 0x00009F40 File Offset: 0x00008140
		[XmlAttribute("Type")]
		public string SDRegValueTypeName
		{
			get
			{
				return this.m_typeName;
			}
			set
			{
				this.m_typeName = value;
			}
		}

		// Token: 0x17000041 RID: 65
		// (get) Token: 0x060001E4 RID: 484 RVA: 0x00009F4C File Offset: 0x0000814C
		// (set) Token: 0x060001E5 RID: 485 RVA: 0x00009FDA File Offset: 0x000081DA
		[XmlAttribute("SACL")]
		public override string MandatoryIntegrityLabel
		{
			get
			{
				this.m_macLabel = null;
				string text = SecurityUtils.ConvertSDToStringSD(this.m_rawsd, SecurityInformationFlags.MANDATORY_ACCESS_LABEL);
				if (this.SDRegValueTypeName == "COM" && text == "S:")
				{
					return string.Empty;
				}
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
				return this.m_macLabel;
			}
			set
			{
				this.m_macLabel = value;
			}
		}

		// Token: 0x17000042 RID: 66
		// (get) Token: 0x060001E6 RID: 486 RVA: 0x00009FE4 File Offset: 0x000081E4
		// (set) Token: 0x060001E7 RID: 487 RVA: 0x0000A0A1 File Offset: 0x000082A1
		[XmlAttribute]
		public override string AttributeHash
		{
			get
			{
				if (string.IsNullOrEmpty(this.m_attributeHash))
				{
					StringBuilder stringBuilder = new StringBuilder();
					stringBuilder.Append(this.TypeString);
					stringBuilder.Append(this.m_path);
					stringBuilder.Append(base.Protected);
					string owner = base.Owner;
					if (!string.IsNullOrEmpty(owner))
					{
						stringBuilder.Append(owner);
					}
					string explicitDACL = base.ExplicitDACL;
					if (!string.IsNullOrEmpty(explicitDACL))
					{
						stringBuilder.Append(explicitDACL);
					}
					string mandatoryIntegrityLabel = this.MandatoryIntegrityLabel;
					if (!string.IsNullOrEmpty(mandatoryIntegrityLabel))
					{
						stringBuilder.Append(mandatoryIntegrityLabel);
					}
					stringBuilder.Append(this.SDRegValueTypeName);
					this.m_attributeHash = CommonUtils.GetSha256Hash(Encoding.Unicode.GetBytes(stringBuilder.ToString()));
				}
				return this.m_attributeHash;
			}
			set
			{
				this.m_attributeHash = value;
			}
		}

		// Token: 0x17000043 RID: 67
		// (get) Token: 0x060001E8 RID: 488 RVA: 0x0000A0AC File Offset: 0x000082AC
		public override NativeObjectSecurity ObjectSecurity
		{
			get
			{
				RegistrySecurity registrySecurity = new RegistrySecurity();
				registrySecurity.SetSecurityDescriptorBinaryForm(this.m_rawsd);
				return registrySecurity;
			}
		}

		// Token: 0x17000044 RID: 68
		// (get) Token: 0x060001E9 RID: 489 RVA: 0x0000A0CC File Offset: 0x000082CC
		protected override string TypeString
		{
			get
			{
				return "SDRegValue";
			}
		}

		// Token: 0x060001EA RID: 490 RVA: 0x0000A0D4 File Offset: 0x000082D4
		protected override string ComputeExplicitDACL()
		{
			RegistrySecurity registrySecurity = new RegistrySecurity();
			registrySecurity.SetSecurityDescriptorBinaryForm(this.m_rawsd);
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
					text = SddlNormalizer.FixAceSddl(text);
				}
			}
			return text;
		}

		// Token: 0x04000141 RID: 321
		protected string m_typeName = "Unknown";

		// Token: 0x04000142 RID: 322
		protected byte[] m_rawsd;
	}
}
