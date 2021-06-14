using System;
using System.Collections.ObjectModel;
using System.Management;

namespace Microsoft.WindowsPhone.ImageUpdate.Tools.Common
{
	// Token: 0x02000039 RID: 57
	public class ImagePartitionCollection : Collection<ImagePartition>
	{
		// Token: 0x0600017F RID: 383 RVA: 0x00008BA8 File Offset: 0x00006DA8
		public void PopulateFromPhysicalDeviceId(string deviceId)
		{
			RelatedObjectQuery query = new RelatedObjectQuery(string.Format("\\\\.\\root\\cimv2:Win32_DiskDrive.DeviceID='{0}'", deviceId), "Win32_DiskPartition");
			ManagementObjectSearcher managementObjectSearcher = new ManagementObjectSearcher(query);
			foreach (ManagementBaseObject managementBaseObject in managementObjectSearcher.Get())
			{
				ManagementObject managementObject = (ManagementObject)managementBaseObject;
				base.Add(new VHDImagePartition(deviceId, managementObject.GetPropertyValue("Name").ToString()));
			}
		}

		// Token: 0x040000E3 RID: 227
		private const string WMI_GETPARTITIONS_QUERY = "\\\\.\\root\\cimv2:Win32_DiskDrive.DeviceID='{0}'";

		// Token: 0x040000E4 RID: 228
		private const string WMI_DISKPARTITION_CLASS = "Win32_DiskPartition";

		// Token: 0x040000E5 RID: 229
		private const string STR_NAME = "Name";
	}
}
