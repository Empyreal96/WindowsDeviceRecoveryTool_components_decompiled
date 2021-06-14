using System;
using System.Windows;

namespace MS.Internal.Data
{
	// Token: 0x02000714 RID: 1812
	internal interface IDataBindEngineClient
	{
		// Token: 0x060074A4 RID: 29860
		void TransferValue();

		// Token: 0x060074A5 RID: 29861
		void UpdateValue();

		// Token: 0x060074A6 RID: 29862
		bool AttachToContext(bool lastChance);

		// Token: 0x060074A7 RID: 29863
		void VerifySourceReference(bool lastChance);

		// Token: 0x060074A8 RID: 29864
		void OnTargetUpdated();

		// Token: 0x17001BC0 RID: 7104
		// (get) Token: 0x060074A9 RID: 29865
		DependencyObject TargetElement { get; }
	}
}
