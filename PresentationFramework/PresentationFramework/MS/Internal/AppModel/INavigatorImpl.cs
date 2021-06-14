using System;
using System.Windows.Media;

namespace MS.Internal.AppModel
{
	// Token: 0x02000782 RID: 1922
	internal interface INavigatorImpl
	{
		// Token: 0x06007944 RID: 31044
		void OnSourceUpdatedFromNavService(bool journalOrCancel);

		// Token: 0x06007945 RID: 31045
		Visual FindRootViewer();
	}
}
