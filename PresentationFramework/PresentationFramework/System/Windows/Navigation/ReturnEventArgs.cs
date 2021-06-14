using System;

namespace System.Windows.Navigation
{
	/// <summary>Provides data for the <see cref="E:System.Windows.Navigation.PageFunction`1.Return" /> event. </summary>
	/// <typeparam name="T">The type of the return value.</typeparam>
	// Token: 0x02000326 RID: 806
	public class ReturnEventArgs<T> : EventArgs
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Navigation.ReturnEventArgs`1" /> class. </summary>
		// Token: 0x06002A87 RID: 10887 RVA: 0x000C2904 File Offset: 0x000C0B04
		public ReturnEventArgs()
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Navigation.ReturnEventArgs`1" /> class with the return value.</summary>
		/// <param name="result">The value to be returned.</param>
		// Token: 0x06002A88 RID: 10888 RVA: 0x000C290C File Offset: 0x000C0B0C
		public ReturnEventArgs(T result)
		{
			this._result = result;
		}

		/// <summary>Gets or sets the value that is returned by the page function.</summary>
		/// <returns>The value that is returned by the page function.</returns>
		// Token: 0x17000A47 RID: 2631
		// (get) Token: 0x06002A89 RID: 10889 RVA: 0x000C291B File Offset: 0x000C0B1B
		// (set) Token: 0x06002A8A RID: 10890 RVA: 0x000C2923 File Offset: 0x000C0B23
		public T Result
		{
			get
			{
				return this._result;
			}
			set
			{
				this._result = value;
			}
		}

		// Token: 0x04001C44 RID: 7236
		private T _result;
	}
}
