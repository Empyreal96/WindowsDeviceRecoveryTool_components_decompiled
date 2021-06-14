using System;
using System.ComponentModel;

namespace System.Windows.Documents.MsSpellCheckLib
{
	// Token: 0x0200045C RID: 1116
	internal interface IChangeNotifyWrapper : INotifyPropertyChanged
	{
		// Token: 0x17000FE3 RID: 4067
		// (get) Token: 0x060040A0 RID: 16544
		// (set) Token: 0x060040A1 RID: 16545
		object Value { get; set; }
	}
}
