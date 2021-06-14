using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace Microsoft.WindowsPhone.ImageUpdate.Tools.Common
{
	// Token: 0x0200004B RID: 75
	[XmlRoot("PhoneSecurityPolicy")]
	public class WPSecurityPolicy
	{
		// Token: 0x0600018B RID: 395 RVA: 0x00008E48 File Offset: 0x00007048
		public WPSecurityPolicy()
		{
		}

		// Token: 0x0600018C RID: 396 RVA: 0x00008EA8 File Offset: 0x000070A8
		public WPSecurityPolicy(string packageName)
		{
			this.m_packageId = packageName;
		}

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x0600018D RID: 397 RVA: 0x00008F0F File Offset: 0x0000710F
		// (set) Token: 0x0600018E RID: 398 RVA: 0x00008F17 File Offset: 0x00007117
		[XmlAttribute]
		public string Description
		{
			get
			{
				return this.m_descr;
			}
			set
			{
				this.m_descr = value;
			}
		}

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x0600018F RID: 399 RVA: 0x00008F20 File Offset: 0x00007120
		// (set) Token: 0x06000190 RID: 400 RVA: 0x00008F28 File Offset: 0x00007128
		[XmlAttribute]
		public string Vendor
		{
			get
			{
				return this.m_vendor;
			}
			set
			{
				this.m_vendor = value;
			}
		}

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x06000191 RID: 401 RVA: 0x00008F31 File Offset: 0x00007131
		// (set) Token: 0x06000192 RID: 402 RVA: 0x00008F39 File Offset: 0x00007139
		[XmlAttribute]
		public string RequiredOSVersion
		{
			get
			{
				return this.m_OSVersion;
			}
			set
			{
				this.m_OSVersion = value;
			}
		}

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x06000193 RID: 403 RVA: 0x00008F42 File Offset: 0x00007142
		// (set) Token: 0x06000194 RID: 404 RVA: 0x00008F4A File Offset: 0x0000714A
		[XmlAttribute]
		public string FileVersion
		{
			get
			{
				return this.m_fileVersion;
			}
			set
			{
				this.m_fileVersion = value;
			}
		}

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x06000195 RID: 405 RVA: 0x00008F53 File Offset: 0x00007153
		// (set) Token: 0x06000196 RID: 406 RVA: 0x00008F5B File Offset: 0x0000715B
		[XmlAttribute]
		public string HashType
		{
			get
			{
				return this.m_hashType;
			}
			set
			{
				this.m_hashType = value;
			}
		}

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x06000197 RID: 407 RVA: 0x00008F64 File Offset: 0x00007164
		// (set) Token: 0x06000198 RID: 408 RVA: 0x00008F6C File Offset: 0x0000716C
		[XmlAttribute]
		public string PackageID
		{
			get
			{
				return this.m_packageId;
			}
			set
			{
				this.m_packageId = value;
			}
		}

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x06000199 RID: 409 RVA: 0x00008F75 File Offset: 0x00007175
		// (set) Token: 0x0600019A RID: 410 RVA: 0x00008F82 File Offset: 0x00007182
		[XmlArrayItem(typeof(RegistryAcl), ElementName = "RegKey")]
		[XmlArrayItem(typeof(FileAcl), ElementName = "File")]
		[XmlArrayItem(typeof(RegistryStoredAcl), ElementName = "SDRegValue")]
		[XmlArrayItem(typeof(DirectoryAcl), ElementName = "Directory")]
		[XmlArrayItem(typeof(RegAclWithFullAcl), ElementName = "RegKeyFullACL")]
		public ResourceAcl[] Rules
		{
			get
			{
				return this.m_aclCollection.ToArray<ResourceAcl>();
			}
			set
			{
				this.m_aclCollection.Clear();
				this.m_aclCollection.UnionWith(value);
			}
		}

		// Token: 0x0600019B RID: 411 RVA: 0x00008F9B File Offset: 0x0000719B
		public void Add(IEnumerable<ResourceAcl> acls)
		{
			this.m_aclCollection.UnionWith(acls);
		}

		// Token: 0x0600019C RID: 412 RVA: 0x00008FAC File Offset: 0x000071AC
		public void SaveToXml(string policyFile)
		{
			XmlSerializer xmlSerializer = new XmlSerializer(typeof(WPSecurityPolicy), "urn:Microsoft.WindowsPhone/PhoneSecurityPolicyInternal.v8.00");
			using (TextWriter textWriter = new StreamWriter(LongPathFile.OpenWrite(policyFile)))
			{
				xmlSerializer.Serialize(textWriter, this);
			}
		}

		// Token: 0x0600019D RID: 413 RVA: 0x00009000 File Offset: 0x00007200
		public static WPSecurityPolicy LoadFromXml(string policyFile)
		{
			if (!LongPathFile.Exists(policyFile))
			{
				throw new FileNotFoundException(string.Format("Policy file {0} does not exist, or cannot be read", policyFile), policyFile);
			}
			XmlSerializer xmlSerializer = new XmlSerializer(typeof(WPSecurityPolicy), "urn:Microsoft.WindowsPhone/PhoneSecurityPolicyInternal.v8.00");
			WPSecurityPolicy result = null;
			using (TextReader textReader = new StreamReader(LongPathFile.OpenRead(policyFile)))
			{
				result = (WPSecurityPolicy)xmlSerializer.Deserialize(textReader);
			}
			return result;
		}

		// Token: 0x04000123 RID: 291
		private const string WP8PolicyNamespace = "urn:Microsoft.WindowsPhone/PhoneSecurityPolicyInternal.v8.00";

		// Token: 0x04000124 RID: 292
		private string m_descr = "Mobile Core Policy";

		// Token: 0x04000125 RID: 293
		private string m_vendor = "Microsoft";

		// Token: 0x04000126 RID: 294
		private string m_OSVersion = "8.00";

		// Token: 0x04000127 RID: 295
		private string m_fileVersion = "8.00";

		// Token: 0x04000128 RID: 296
		private string m_hashType = "Sha2";

		// Token: 0x04000129 RID: 297
		private string m_packageId = "";

		// Token: 0x0400012A RID: 298
		private AclCollection m_aclCollection = new AclCollection();
	}
}
