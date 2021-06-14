using System;

namespace Nokia.Lucid.DeviceDetection
{
	// Token: 0x02000002 RID: 2
	public class PortChecker
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		public PortChecker(string[] parentDeviceLocationPaths)
		{
			if (parentDeviceLocationPaths == null)
			{
				throw new ArgumentNullException("parentDeviceLocationPaths");
			}
			this.PortIds = parentDeviceLocationPaths;
		}

		// Token: 0x06000002 RID: 2 RVA: 0x00002070 File Offset: 0x00000270
		public PortChecker(string parentDeviceLocationPath)
		{
			if (parentDeviceLocationPath == null)
			{
				throw new ArgumentNullException("parentDeviceLocationPath");
			}
			this.PortIds = new string[]
			{
				parentDeviceLocationPath
			};
		}

		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000003 RID: 3 RVA: 0x000020A3 File Offset: 0x000002A3
		// (set) Token: 0x06000004 RID: 4 RVA: 0x000020AB File Offset: 0x000002AB
		public string[] PortIds { get; private set; }

		// Token: 0x06000005 RID: 5 RVA: 0x000020B4 File Offset: 0x000002B4
		public int Check(string locationPath)
		{
			if (locationPath == null)
			{
				throw new ArgumentNullException("locationPath");
			}
			int num = 0;
			foreach (string value in this.PortIds)
			{
				if (locationPath.Contains(value))
				{
					return num;
				}
				num++;
			}
			return -1;
		}
	}
}
