using System;
using System.IO;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Xml.Serialization;

namespace Microsoft.WindowsPhone.ImageUpdate.Tools.Common
{
	// Token: 0x02000050 RID: 80
	public class DirectoryAcl : ResourceAcl
	{
		// Token: 0x060001C3 RID: 451 RVA: 0x000097F5 File Offset: 0x000079F5
		public DirectoryAcl()
		{
		}

		// Token: 0x060001C4 RID: 452 RVA: 0x00009800 File Offset: 0x00007A00
		public DirectoryAcl(string directory, string rootPath)
		{
			if (!LongPathDirectory.Exists(directory))
			{
				throw new DirectoryNotFoundException(string.Format("Folder {0} cannot be found", directory));
			}
			DirectoryInfo di = new DirectoryInfo(directory);
			this.Initialize(di, rootPath);
		}

		// Token: 0x060001C5 RID: 453 RVA: 0x0000983B File Offset: 0x00007A3B
		public DirectoryAcl(DirectoryInfo di, string rootPath)
		{
			if (di == null)
			{
				throw new ArgumentNullException("di");
			}
			this.Initialize(di, rootPath);
		}

		// Token: 0x17000036 RID: 54
		// (get) Token: 0x060001C6 RID: 454 RVA: 0x0000985C File Offset: 0x00007A5C
		// (set) Token: 0x060001C7 RID: 455 RVA: 0x000098AF File Offset: 0x00007AAF
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

		// Token: 0x17000037 RID: 55
		// (get) Token: 0x060001C8 RID: 456 RVA: 0x000098B8 File Offset: 0x00007AB8
		public override NativeObjectSecurity ObjectSecurity
		{
			get
			{
				DirectorySecurity directorySecurity = null;
				if (this.m_nos != null)
				{
					directorySecurity = new DirectorySecurity();
					directorySecurity.SetSecurityDescriptorBinaryForm(this.m_nos.GetSecurityDescriptorBinaryForm());
				}
				return directorySecurity;
			}
		}

		// Token: 0x17000038 RID: 56
		// (get) Token: 0x060001C9 RID: 457 RVA: 0x000098E7 File Offset: 0x00007AE7
		protected override string TypeString
		{
			get
			{
				return "Directory";
			}
		}

		// Token: 0x060001CA RID: 458 RVA: 0x000098F0 File Offset: 0x00007AF0
		protected override string ComputeExplicitDACL()
		{
			string text = null;
			if (this.m_isRoot)
			{
				text = this.m_nos.GetSecurityDescriptorSddlForm(AccessControlSections.Access);
			}
			else
			{
				DirectorySecurity accessControl = this.m_di.GetAccessControl(AccessControlSections.All);
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
				if (base.DACLProtected || num > 0)
				{
					text = accessControl.GetSecurityDescriptorSddlForm(AccessControlSections.Access);
				}
			}
			if (!string.IsNullOrEmpty(text))
			{
				text = ResourceAcl.regexStripDacl.Replace(text, string.Empty);
			}
			return SddlNormalizer.FixAceSddl(text);
		}

		// Token: 0x060001CB RID: 459 RVA: 0x000099F0 File Offset: 0x00007BF0
		private void Initialize(DirectoryInfo di, string rootPath)
		{
			if (di == null)
			{
				throw new ArgumentNullException("di");
			}
			this.m_di = di;
			this.m_nos = di.GetAccessControl(AccessControlSections.All);
			this.m_fullPath = di.FullName;
			this.m_isRoot = string.Equals(di.FullName, rootPath, StringComparison.OrdinalIgnoreCase);
			this.m_path = System.IO.Path.Combine("\\", di.FullName.Remove(0, rootPath.Length)).ToUpper();
		}

		// Token: 0x0400013D RID: 317
		private bool m_isRoot;

		// Token: 0x0400013E RID: 318
		private DirectoryInfo m_di;
	}
}
