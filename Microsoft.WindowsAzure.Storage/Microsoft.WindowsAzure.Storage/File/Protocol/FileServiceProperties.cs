using System;
using System.IO;
using System.Xml.Linq;
using Microsoft.WindowsAzure.Storage.Shared.Protocol;

namespace Microsoft.WindowsAzure.Storage.File.Protocol
{
	// Token: 0x020000EA RID: 234
	public sealed class FileServiceProperties
	{
		// Token: 0x060011F6 RID: 4598 RVA: 0x0004299D File Offset: 0x00040B9D
		public FileServiceProperties()
		{
			this.serviceProperties = new ServiceProperties();
			this.serviceProperties.HourMetrics = null;
			this.serviceProperties.MinuteMetrics = null;
			this.serviceProperties.Logging = null;
		}

		// Token: 0x170002AB RID: 683
		// (get) Token: 0x060011F7 RID: 4599 RVA: 0x000429D4 File Offset: 0x00040BD4
		// (set) Token: 0x060011F8 RID: 4600 RVA: 0x000429E1 File Offset: 0x00040BE1
		public CorsProperties Cors
		{
			get
			{
				return this.serviceProperties.Cors;
			}
			set
			{
				this.serviceProperties.Cors = value;
			}
		}

		// Token: 0x060011F9 RID: 4601 RVA: 0x000429F0 File Offset: 0x00040BF0
		internal static FileServiceProperties FromServiceXml(XDocument servicePropertiesDocument)
		{
			XElement xelement = servicePropertiesDocument.Element("StorageServiceProperties");
			return new FileServiceProperties
			{
				Cors = ServiceProperties.ReadCorsPropertiesFromXml(xelement.Element("Cors"))
			};
		}

		// Token: 0x060011FA RID: 4602 RVA: 0x00042A32 File Offset: 0x00040C32
		internal XDocument ToServiceXml()
		{
			return this.serviceProperties.ToServiceXml();
		}

		// Token: 0x060011FB RID: 4603 RVA: 0x00042A3F File Offset: 0x00040C3F
		internal void WriteServiceProperties(Stream outputStream)
		{
			this.serviceProperties.WriteServiceProperties(outputStream);
		}

		// Token: 0x04000508 RID: 1288
		private ServiceProperties serviceProperties;
	}
}
