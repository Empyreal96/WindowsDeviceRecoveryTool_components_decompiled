using System;
using System.IO;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Xml.Serialization;

namespace Microsoft.WindowsPhone.ImageUpdate.Tools.Common
{
	// Token: 0x02000051 RID: 81
	public class FileAcl : ResourceAcl
	{
		// Token: 0x060001CC RID: 460 RVA: 0x00009A66 File Offset: 0x00007C66
		public FileAcl()
		{
		}

		// Token: 0x060001CD RID: 461 RVA: 0x00009A70 File Offset: 0x00007C70
		public FileAcl(string file, string rootPath)
		{
			if (!LongPathFile.Exists(file))
			{
				throw new FileNotFoundException("Specified file cannot be found", file);
			}
			FileInfo fi = new FileInfo(file);
			this.Initialize(fi, rootPath);
		}

		// Token: 0x060001CE RID: 462 RVA: 0x00009AA6 File Offset: 0x00007CA6
		public FileAcl(FileInfo fi, string rootPath)
		{
			if (fi == null)
			{
				throw new ArgumentNullException("fi");
			}
			this.Initialize(fi, rootPath);
		}

		// Token: 0x17000039 RID: 57
		// (get) Token: 0x060001CF RID: 463 RVA: 0x00009AC4 File Offset: 0x00007CC4
		// (set) Token: 0x060001D0 RID: 464 RVA: 0x00009B17 File Offset: 0x00007D17
		[XmlAttribute("SACL")]
		public override string MandatoryIntegrityLabel
		{
			get
			{
				if (this.m_nos != null)
				{
					this.m_macLabel = SecurityUtils.GetFileSystemMandatoryLevel(this.m_fullPath);
					if (string.IsNullOrEmpty(this.m_macLabel))
					{
						this.m_macLabel = null;
					}
					else
					{
						this.m_macLabel = SddlNormalizer.FixAceSddl(this.m_macLabel);
					}
				}
				return this.m_macLabel;
			}
			set
			{
				this.m_macLabel = value;
			}
		}

		// Token: 0x1700003A RID: 58
		// (get) Token: 0x060001D1 RID: 465 RVA: 0x00009B20 File Offset: 0x00007D20
		public override NativeObjectSecurity ObjectSecurity
		{
			get
			{
				FileSecurity fileSecurity = null;
				if (this.m_nos != null)
				{
					fileSecurity = new FileSecurity();
					fileSecurity.SetSecurityDescriptorBinaryForm(this.m_nos.GetSecurityDescriptorBinaryForm());
				}
				return fileSecurity;
			}
		}

		// Token: 0x1700003B RID: 59
		// (get) Token: 0x060001D2 RID: 466 RVA: 0x00009B4F File Offset: 0x00007D4F
		protected override string TypeString
		{
			get
			{
				return "File";
			}
		}

		// Token: 0x060001D3 RID: 467 RVA: 0x00009B58 File Offset: 0x00007D58
		protected override string ComputeExplicitDACL()
		{
			FileSecurity accessControl = this.m_fi.GetAccessControl(AccessControlSections.All);
			AuthorizationRuleCollection accessRules = accessControl.GetAccessRules(true, false, typeof(NTAccount));
			int num = accessRules.Count;
			foreach (object obj in accessRules)
			{
				FileSystemAccessRule fileSystemAccessRule = (FileSystemAccessRule)obj;
				if (fileSystemAccessRule.IsInherited)
				{
					accessControl.RemoveAccessRule(fileSystemAccessRule);
					num--;
				}
			}
			if (base.DACLProtected && accessControl.AreAccessRulesCanonical)
			{
				accessControl.SetAccessRuleProtection(true, base.PreserveInheritance);
			}
			string text = null;
			if (base.DACLProtected || num > 0)
			{
				text = accessControl.GetSecurityDescriptorSddlForm(AccessControlSections.Access);
				if (!string.IsNullOrEmpty(text))
				{
					text = ResourceAcl.regexStripDacl.Replace(text, string.Empty);
				}
			}
			return SddlNormalizer.FixAceSddl(text);
		}

		// Token: 0x060001D4 RID: 468 RVA: 0x00009C40 File Offset: 0x00007E40
		private void Initialize(FileInfo fi, string rootPath)
		{
			if (fi == null)
			{
				throw new ArgumentNullException("fi");
			}
			this.m_fi = fi;
			this.m_nos = fi.GetAccessControl(AccessControlSections.All);
			this.m_fullPath = fi.FullName;
			this.m_path = System.IO.Path.Combine("\\", this.m_fullPath.Remove(0, rootPath.Length)).ToUpper();
		}

		// Token: 0x0400013F RID: 319
		private FileInfo m_fi;
	}
}
