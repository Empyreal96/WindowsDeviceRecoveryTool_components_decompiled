using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using Microsoft.WindowsDeviceRecoveryTool.Model.Exceptions;

namespace Microsoft.WindowsDeviceRecoveryTool.States.Error
{
	// Token: 0x020000BD RID: 189
	public class ErrorTemplateSelector : DataTemplateSelector
	{
		// Token: 0x17000142 RID: 322
		// (get) Token: 0x060005AB RID: 1451 RVA: 0x0001D8EC File Offset: 0x0001BAEC
		// (set) Token: 0x060005AC RID: 1452 RVA: 0x0001D903 File Offset: 0x0001BB03
		public DataTemplate DefaultError { get; set; }

		// Token: 0x17000143 RID: 323
		// (get) Token: 0x060005AD RID: 1453 RVA: 0x0001D90C File Offset: 0x0001BB0C
		// (set) Token: 0x060005AE RID: 1454 RVA: 0x0001D923 File Offset: 0x0001BB23
		public DataTemplate TryAgainError { get; set; }

		// Token: 0x17000144 RID: 324
		// (get) Token: 0x060005AF RID: 1455 RVA: 0x0001D92C File Offset: 0x0001BB2C
		// (set) Token: 0x060005B0 RID: 1456 RVA: 0x0001D943 File Offset: 0x0001BB43
		public DataTemplate AutoUpdateError { get; set; }

		// Token: 0x060005B1 RID: 1457 RVA: 0x0001D94C File Offset: 0x0001BB4C
		public override DataTemplate SelectTemplate(object item, DependencyObject container)
		{
			DataTemplate dataTemplate = this.SelectWorkflowExceptionTemplate(item);
			DataTemplate result;
			if (dataTemplate != null)
			{
				result = dataTemplate;
			}
			else if (item is Exception)
			{
				result = this.DefaultError;
			}
			else
			{
				result = null;
			}
			return result;
		}

		// Token: 0x060005B2 RID: 1458 RVA: 0x0001D990 File Offset: 0x0001BB90
		public bool IsImplemented(object exception)
		{
			return this.SelectTemplate(exception, null) != null;
		}

		// Token: 0x060005B3 RID: 1459 RVA: 0x0001D9B0 File Offset: 0x0001BBB0
		public bool IsWorkflowException(Exception ex)
		{
			return this.SelectWorkflowExceptionTemplate(ex) != null;
		}

		// Token: 0x060005B4 RID: 1460 RVA: 0x0001D9D0 File Offset: 0x0001BBD0
		private DataTemplate SelectWorkflowExceptionTemplate(object item)
		{
			DataTemplate result;
			if (item is DeviceNotFoundException || item is DownloadPackageException || item is CannotAccessDirectoryException || item is WebException || item is NoInternetConnectionException || item is NotEnoughSpaceException || item is PackageNotFoundException || item is FirmwareFileNotFoundException)
			{
				result = this.TryAgainError;
			}
			else if (item is AutoUpdateException || item is PlannedServiceBreakException || item is AutoUpdateNotEnoughSpaceException)
			{
				result = this.AutoUpdateError;
			}
			else
			{
				result = null;
			}
			return result;
		}
	}
}
