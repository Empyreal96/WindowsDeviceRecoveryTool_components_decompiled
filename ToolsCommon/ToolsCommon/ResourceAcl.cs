using System;
using System.Globalization;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

namespace Microsoft.WindowsPhone.ImageUpdate.Tools.Common
{
	// Token: 0x0200004E RID: 78
	public abstract class ResourceAcl
	{
		// Token: 0x17000026 RID: 38
		// (get) Token: 0x060001A6 RID: 422 RVA: 0x0000940C File Offset: 0x0000760C
		// (set) Token: 0x060001A7 RID: 423 RVA: 0x00009428 File Offset: 0x00007628
		[XmlAttribute("DACL")]
		public string ExplicitDACL
		{
			get
			{
				if (this.m_nos != null)
				{
					this.m_explicitDacl = this.ComputeExplicitDACL();
				}
				return this.m_explicitDacl;
			}
			set
			{
				this.m_explicitDacl = value;
			}
		}

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x060001A8 RID: 424
		// (set) Token: 0x060001A9 RID: 425
		[XmlAttribute("SACL")]
		public abstract string MandatoryIntegrityLabel { get; set; }

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x060001AA RID: 426 RVA: 0x00009431 File Offset: 0x00007631
		// (set) Token: 0x060001AB RID: 427 RVA: 0x00009465 File Offset: 0x00007665
		[XmlAttribute("Owner")]
		public string Owner
		{
			get
			{
				if (this.m_nos != null)
				{
					this.m_owner = this.m_nos.GetSecurityDescriptorSddlForm(AccessControlSections.Owner | AccessControlSections.Group);
					this.m_owner = SddlNormalizer.FixOwnerSddl(this.m_owner);
				}
				return this.m_owner;
			}
			set
			{
				this.m_owner = value;
			}
		}

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x060001AC RID: 428 RVA: 0x00009470 File Offset: 0x00007670
		// (set) Token: 0x060001AD RID: 429 RVA: 0x000094DB File Offset: 0x000076DB
		[XmlAttribute]
		public string ElementID
		{
			get
			{
				if (!string.IsNullOrEmpty(this.m_path))
				{
					StringBuilder stringBuilder = new StringBuilder();
					stringBuilder.Append(this.TypeString);
					stringBuilder.Append(this.m_path.ToUpper(new CultureInfo("en-US", false)));
					this.m_elementId = CommonUtils.GetSha256Hash(Encoding.Unicode.GetBytes(stringBuilder.ToString()));
				}
				return this.m_elementId;
			}
			set
			{
				this.m_elementId = value;
			}
		}

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x060001AE RID: 430 RVA: 0x000094E4 File Offset: 0x000076E4
		// (set) Token: 0x060001AF RID: 431 RVA: 0x000095A4 File Offset: 0x000077A4
		[XmlAttribute]
		public virtual string AttributeHash
		{
			get
			{
				if (string.IsNullOrEmpty(this.m_attributeHash))
				{
					StringBuilder stringBuilder = new StringBuilder();
					stringBuilder.Append(this.TypeString);
					stringBuilder.Append(this.m_path.ToUpper(new CultureInfo("en-US", false)));
					stringBuilder.Append(this.Protected);
					string owner = this.Owner;
					if (!string.IsNullOrEmpty(owner))
					{
						stringBuilder.Append(owner);
					}
					string explicitDACL = this.ExplicitDACL;
					if (!string.IsNullOrEmpty(explicitDACL))
					{
						stringBuilder.Append(explicitDACL);
					}
					string mandatoryIntegrityLabel = this.MandatoryIntegrityLabel;
					if (!string.IsNullOrEmpty(mandatoryIntegrityLabel))
					{
						stringBuilder.Append(mandatoryIntegrityLabel);
					}
					this.m_attributeHash = CommonUtils.GetSha256Hash(Encoding.Unicode.GetBytes(stringBuilder.ToString()));
				}
				return this.m_attributeHash;
			}
			set
			{
				this.m_attributeHash = value;
			}
		}

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x060001B0 RID: 432 RVA: 0x000095AD File Offset: 0x000077AD
		// (set) Token: 0x060001B1 RID: 433 RVA: 0x000095B5 File Offset: 0x000077B5
		[XmlAttribute]
		public string Path
		{
			get
			{
				return this.m_path;
			}
			set
			{
				this.m_path = value;
			}
		}

		// Token: 0x1700002C RID: 44
		// (get) Token: 0x060001B2 RID: 434 RVA: 0x000095BE File Offset: 0x000077BE
		// (set) Token: 0x060001B3 RID: 435 RVA: 0x000095EC File Offset: 0x000077EC
		[XmlIgnore]
		public string Protected
		{
			get
			{
				if (this.m_nos != null)
				{
					this.m_isProtected = this.m_nos.AreAccessRulesProtected;
				}
				if (!this.m_isProtected)
				{
					return "No";
				}
				return "Yes";
			}
			set
			{
				this.m_isProtected = value.Equals("Yes", StringComparison.OrdinalIgnoreCase);
			}
		}

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x060001B4 RID: 436 RVA: 0x00009608 File Offset: 0x00007808
		public bool IsEmpty
		{
			get
			{
				return string.IsNullOrEmpty(this.ExplicitDACL) && string.IsNullOrEmpty(this.MandatoryIntegrityLabel) && !this.DACLProtected;
			}
		}

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x060001B5 RID: 437 RVA: 0x00009640 File Offset: 0x00007840
		public string DACL
		{
			get
			{
				string text = string.Empty;
				if (this.m_nos != null)
				{
					text = this.m_nos.GetSecurityDescriptorSddlForm(AccessControlSections.Access);
					if (!string.IsNullOrEmpty(text))
					{
						text = ResourceAcl.regexStripDacl.Replace(text, string.Empty);
					}
				}
				return SddlNormalizer.FixAceSddl(text);
			}
		}

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x060001B6 RID: 438 RVA: 0x00009688 File Offset: 0x00007888
		public string FullACL
		{
			get
			{
				string result = string.Empty;
				if (this.m_nos != null)
				{
					result = this.m_nos.GetSecurityDescriptorSddlForm(AccessControlSections.All);
				}
				return result;
			}
		}

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x060001B7 RID: 439 RVA: 0x000096B2 File Offset: 0x000078B2
		public static ResourceAclComparer Comparer
		{
			get
			{
				return ResourceAcl.ResourceAclComparer;
			}
		}

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x060001B8 RID: 440
		public abstract NativeObjectSecurity ObjectSecurity { get; }

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x060001B9 RID: 441
		protected abstract string TypeString { get; }

		// Token: 0x17000033 RID: 51
		// (get) Token: 0x060001BA RID: 442 RVA: 0x000096B9 File Offset: 0x000078B9
		protected AuthorizationRuleCollection AccessRules
		{
			get
			{
				if (this.m_accessRules == null && this.m_nos != null)
				{
					this.m_accessRules = this.m_nos.GetAccessRules(true, false, typeof(NTAccount));
				}
				return this.m_accessRules;
			}
		}

		// Token: 0x17000034 RID: 52
		// (get) Token: 0x060001BB RID: 443 RVA: 0x000096EE File Offset: 0x000078EE
		public bool PreserveInheritance
		{
			get
			{
				return this.m_nos != null && this.m_nos.GetAccessRules(false, true, typeof(NTAccount)).Count > 0;
			}
		}

		// Token: 0x17000035 RID: 53
		// (get) Token: 0x060001BC RID: 444 RVA: 0x00009719 File Offset: 0x00007919
		public bool DACLProtected
		{
			get
			{
				return this.m_nos != null && this.m_nos.AreAccessRulesProtected;
			}
		}

		// Token: 0x060001BD RID: 445
		protected abstract string ComputeExplicitDACL();

		// Token: 0x0400012F RID: 303
		protected string m_explicitDacl;

		// Token: 0x04000130 RID: 304
		protected string m_macLabel;

		// Token: 0x04000131 RID: 305
		protected string m_owner;

		// Token: 0x04000132 RID: 306
		protected string m_elementId;

		// Token: 0x04000133 RID: 307
		protected string m_attributeHash;

		// Token: 0x04000134 RID: 308
		protected string m_path;

		// Token: 0x04000135 RID: 309
		protected bool m_isProtected;

		// Token: 0x04000136 RID: 310
		private static readonly ResourceAclComparer ResourceAclComparer = new ResourceAclComparer();

		// Token: 0x04000137 RID: 311
		[CLSCompliant(false)]
		protected NativeObjectSecurity m_nos;

		// Token: 0x04000138 RID: 312
		[CLSCompliant(false)]
		protected AuthorizationRuleCollection m_accessRules;

		// Token: 0x04000139 RID: 313
		protected string m_fullPath = string.Empty;

		// Token: 0x0400013A RID: 314
		protected static readonly Regex regexExtractMIL = new Regex("(?<MIL>\\(ML[^\\)]*\\))", RegexOptions.IgnoreCase | RegexOptions.Compiled);

		// Token: 0x0400013B RID: 315
		protected static readonly Regex regexStripDacl = new Regex("^D:", RegexOptions.IgnoreCase | RegexOptions.Compiled);

		// Token: 0x0400013C RID: 316
		protected static readonly Regex regexStripDriveLetter = new Regex("^[A-Z]:", RegexOptions.IgnoreCase | RegexOptions.Compiled);
	}
}
