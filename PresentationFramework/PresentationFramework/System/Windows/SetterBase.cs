using System;

namespace System.Windows
{
	/// <summary>Represents the base class for value setters. </summary>
	// Token: 0x020000F3 RID: 243
	[Localizability(LocalizationCategory.Ignore)]
	public abstract class SetterBase
	{
		// Token: 0x060008A0 RID: 2208 RVA: 0x0000326D File Offset: 0x0000146D
		internal SetterBase()
		{
		}

		/// <summary>Gets a value that indicates whether this object is in an immutable state.</summary>
		/// <returns>
		///     <see langword="true" /> if this object is in an immutable state; otherwise, <see langword="false" />.</returns>
		// Token: 0x170001CC RID: 460
		// (get) Token: 0x060008A1 RID: 2209 RVA: 0x0001C1C0 File Offset: 0x0001A3C0
		public bool IsSealed
		{
			get
			{
				return this._sealed;
			}
		}

		// Token: 0x060008A2 RID: 2210 RVA: 0x0001C1C8 File Offset: 0x0001A3C8
		internal virtual void Seal()
		{
			this._sealed = true;
		}

		/// <summary>Checks whether this object is read-only and cannot be changed.</summary>
		// Token: 0x060008A3 RID: 2211 RVA: 0x0001C1D1 File Offset: 0x0001A3D1
		protected void CheckSealed()
		{
			if (this._sealed)
			{
				throw new InvalidOperationException(SR.Get("CannotChangeAfterSealed", new object[]
				{
					"SetterBase"
				}));
			}
		}

		// Token: 0x040007B0 RID: 1968
		private bool _sealed;
	}
}
