using System;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

namespace System.Management.Instrumentation
{
	// Token: 0x020000C6 RID: 198
	internal class MetaDataInfo : IDisposable
	{
		// Token: 0x06000578 RID: 1400 RVA: 0x00026B97 File Offset: 0x00024D97
		public MetaDataInfo(Assembly assembly) : this(assembly.Location)
		{
		}

		// Token: 0x06000579 RID: 1401 RVA: 0x00026BA8 File Offset: 0x00024DA8
		public MetaDataInfo(string assemblyName)
		{
			Guid guid = new Guid(((GuidAttribute)Attribute.GetCustomAttribute(typeof(IMetaDataImportInternalOnly), typeof(GuidAttribute), false)).Value);
			IMetaDataDispenser metaDataDispenser = (IMetaDataDispenser)new CorMetaDataDispenser();
			this.importInterface = (IMetaDataImportInternalOnly)metaDataDispenser.OpenScope(assemblyName, 0U, ref guid);
			Marshal.ReleaseComObject(metaDataDispenser);
		}

		// Token: 0x0600057A RID: 1402 RVA: 0x00026C10 File Offset: 0x00024E10
		private void InitNameAndMvid()
		{
			if (this.name == null)
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Capacity = 0;
				uint capacity;
				this.importInterface.GetScopeProps(stringBuilder, (uint)stringBuilder.Capacity, out capacity, out this.mvid);
				stringBuilder.Capacity = (int)capacity;
				this.importInterface.GetScopeProps(stringBuilder, (uint)stringBuilder.Capacity, out capacity, out this.mvid);
				this.name = stringBuilder.ToString();
			}
		}

		// Token: 0x170000DC RID: 220
		// (get) Token: 0x0600057B RID: 1403 RVA: 0x00026C79 File Offset: 0x00024E79
		public string Name
		{
			get
			{
				this.InitNameAndMvid();
				return this.name;
			}
		}

		// Token: 0x170000DD RID: 221
		// (get) Token: 0x0600057C RID: 1404 RVA: 0x00026C87 File Offset: 0x00024E87
		public Guid Mvid
		{
			get
			{
				this.InitNameAndMvid();
				return this.mvid;
			}
		}

		// Token: 0x0600057D RID: 1405 RVA: 0x00026C95 File Offset: 0x00024E95
		public void Dispose()
		{
			if (this.importInterface == null)
			{
				Marshal.ReleaseComObject(this.importInterface);
			}
			this.importInterface = null;
			GC.SuppressFinalize(this);
		}

		// Token: 0x0600057E RID: 1406 RVA: 0x00026CB8 File Offset: 0x00024EB8
		~MetaDataInfo()
		{
			this.Dispose();
		}

		// Token: 0x0600057F RID: 1407 RVA: 0x00026CE4 File Offset: 0x00024EE4
		public static Guid GetMvid(Assembly assembly)
		{
			Guid result;
			using (MetaDataInfo metaDataInfo = new MetaDataInfo(assembly))
			{
				result = metaDataInfo.Mvid;
			}
			return result;
		}

		// Token: 0x04000538 RID: 1336
		private IMetaDataImportInternalOnly importInterface;

		// Token: 0x04000539 RID: 1337
		private string name;

		// Token: 0x0400053A RID: 1338
		private Guid mvid;
	}
}
