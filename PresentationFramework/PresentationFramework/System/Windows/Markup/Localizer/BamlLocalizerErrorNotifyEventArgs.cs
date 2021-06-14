using System;

namespace System.Windows.Markup.Localizer
{
	/// <summary>Provides required event data for the <see cref="E:System.Windows.Markup.Localizer.BamlLocalizer.ErrorNotify" /> event.</summary>
	// Token: 0x02000295 RID: 661
	public class BamlLocalizerErrorNotifyEventArgs : EventArgs
	{
		// Token: 0x06002518 RID: 9496 RVA: 0x000B3078 File Offset: 0x000B1278
		internal BamlLocalizerErrorNotifyEventArgs(BamlLocalizableResourceKey key, BamlLocalizerError error)
		{
			this._key = key;
			this._error = error;
		}

		/// <summary>Gets the key associated with the resource that generated the error condition.</summary>
		/// <returns>The key associated with the resource that generated the error condition.</returns>
		// Token: 0x1700092E RID: 2350
		// (get) Token: 0x06002519 RID: 9497 RVA: 0x000B308E File Offset: 0x000B128E
		public BamlLocalizableResourceKey Key
		{
			get
			{
				return this._key;
			}
		}

		/// <summary>Gets the specific error condition encountered by <see cref="T:System.Windows.Markup.Localizer.BamlLocalizer" />.</summary>
		/// <returns>The error condition encountered by <see cref="T:System.Windows.Markup.Localizer.BamlLocalizer" />, as a value of the enumeration.</returns>
		// Token: 0x1700092F RID: 2351
		// (get) Token: 0x0600251A RID: 9498 RVA: 0x000B3096 File Offset: 0x000B1296
		public BamlLocalizerError Error
		{
			get
			{
				return this._error;
			}
		}

		// Token: 0x04001B62 RID: 7010
		private BamlLocalizableResourceKey _key;

		// Token: 0x04001B63 RID: 7011
		private BamlLocalizerError _error;
	}
}
