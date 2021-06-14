using System;
using System.ComponentModel;

namespace System.Windows.Documents.MsSpellCheckLib
{
	// Token: 0x0200045D RID: 1117
	internal interface IChangeNotifyWrapper<T> : IChangeNotifyWrapper, INotifyPropertyChanged
	{
		// Token: 0x17000FE4 RID: 4068
		// (get) Token: 0x060040A2 RID: 16546
		// (set) Token: 0x060040A3 RID: 16547
		T Value { get; set; }
	}
}
