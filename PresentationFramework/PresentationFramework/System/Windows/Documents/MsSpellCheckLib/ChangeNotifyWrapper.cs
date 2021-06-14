using System;
using System.ComponentModel;

namespace System.Windows.Documents.MsSpellCheckLib
{
	// Token: 0x0200045E RID: 1118
	internal class ChangeNotifyWrapper<T> : IChangeNotifyWrapper<T>, IChangeNotifyWrapper, INotifyPropertyChanged
	{
		// Token: 0x060040A4 RID: 16548 RVA: 0x00127472 File Offset: 0x00125672
		internal ChangeNotifyWrapper(T value = default(T), bool shouldThrowInvalidCastException = false)
		{
			this.Value = value;
			this._shouldThrowInvalidCastException = shouldThrowInvalidCastException;
		}

		// Token: 0x17000FE5 RID: 4069
		// (get) Token: 0x060040A5 RID: 16549 RVA: 0x00127488 File Offset: 0x00125688
		// (set) Token: 0x060040A6 RID: 16550 RVA: 0x00127490 File Offset: 0x00125690
		public T Value
		{
			get
			{
				return this._value;
			}
			set
			{
				this._value = value;
				PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
				if (propertyChanged == null)
				{
					return;
				}
				propertyChanged(this, new PropertyChangedEventArgs("Value"));
			}
		}

		// Token: 0x17000FE6 RID: 4070
		// (get) Token: 0x060040A7 RID: 16551 RVA: 0x001274B4 File Offset: 0x001256B4
		// (set) Token: 0x060040A8 RID: 16552 RVA: 0x001274C4 File Offset: 0x001256C4
		object IChangeNotifyWrapper.Value
		{
			get
			{
				return this.Value;
			}
			set
			{
				T value2 = default(T);
				try
				{
					value2 = (T)((object)value);
				}
				catch (InvalidCastException obj) when (!this._shouldThrowInvalidCastException)
				{
					return;
				}
				this.Value = value2;
			}
		}

		// Token: 0x14000099 RID: 153
		// (add) Token: 0x060040A9 RID: 16553 RVA: 0x00127518 File Offset: 0x00125718
		// (remove) Token: 0x060040AA RID: 16554 RVA: 0x00127550 File Offset: 0x00125750
		public event PropertyChangedEventHandler PropertyChanged;

		// Token: 0x04002771 RID: 10097
		private T _value;

		// Token: 0x04002772 RID: 10098
		private bool _shouldThrowInvalidCastException;
	}
}
