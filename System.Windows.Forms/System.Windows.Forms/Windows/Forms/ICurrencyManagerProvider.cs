using System;

namespace System.Windows.Forms
{
	/// <summary>Provides custom binding management for components.</summary>
	// Token: 0x0200027B RID: 635
	[SRDescription("ICurrencyManagerProviderDescr")]
	public interface ICurrencyManagerProvider
	{
		/// <summary>Gets the <see cref="T:System.Windows.Forms.CurrencyManager" /> associated with this <see cref="T:System.Windows.Forms.ICurrencyManagerProvider" />. </summary>
		/// <returns>The <see cref="T:System.Windows.Forms.CurrencyManager" /> associated with this <see cref="T:System.Windows.Forms.ICurrencyManagerProvider" />.</returns>
		// Token: 0x1700094A RID: 2378
		// (get) Token: 0x06002642 RID: 9794
		CurrencyManager CurrencyManager { get; }

		/// <summary>Gets the <see cref="T:System.Windows.Forms.CurrencyManager" /> for this <see cref="T:System.Windows.Forms.ICurrencyManagerProvider" /> and the specified data member.</summary>
		/// <param name="dataMember">The name of the column or list, within the data source, to obtain the <see cref="T:System.Windows.Forms.CurrencyManager" /> for.</param>
		/// <returns>The related <see cref="T:System.Windows.Forms.CurrencyManager" /> obtained from this <see cref="T:System.Windows.Forms.ICurrencyManagerProvider" /> and the specified data member.</returns>
		// Token: 0x06002643 RID: 9795
		CurrencyManager GetRelatedCurrencyManager(string dataMember);
	}
}
