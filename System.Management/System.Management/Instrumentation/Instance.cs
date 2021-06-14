using System;

namespace System.Management.Instrumentation
{
	/// <summary>Represents derived classes known to be management instrumentation instance classes. These derived classes inherit an implementation of <see cref="T:System.Management.Instrumentation.IInstance" /> that allows instances to be published through the <see cref="P:System.Management.Instrumentation.IInstance.Published" /> property.          Note: the WMI .NET libraries are now considered in final state, and no further development, enhancements, or updates will be available for non-security related issues affecting these libraries. The MI APIs should be used for all new development.</summary>
	// Token: 0x020000BD RID: 189
	[InstrumentationClass(InstrumentationType.Instance)]
	public abstract class Instance : IInstance
	{
		// Token: 0x170000B0 RID: 176
		// (get) Token: 0x0600050A RID: 1290 RVA: 0x00023F91 File Offset: 0x00022191
		private ProvisionFunction PublishFunction
		{
			get
			{
				if (this.publishFunction == null)
				{
					this.publishFunction = Instrumentation.GetPublishFunction(base.GetType());
				}
				return this.publishFunction;
			}
		}

		// Token: 0x170000B1 RID: 177
		// (get) Token: 0x0600050B RID: 1291 RVA: 0x00023FB2 File Offset: 0x000221B2
		private ProvisionFunction RevokeFunction
		{
			get
			{
				if (this.revokeFunction == null)
				{
					this.revokeFunction = Instrumentation.GetRevokeFunction(base.GetType());
				}
				return this.revokeFunction;
			}
		}

		/// <summary>Gets or sets a value indicating whether instances of classes that implement this interface are visible through management instrumentation.          </summary>
		/// <returns>Returns a <see cref="T:System.Boolean" /> value indicating whether instances of classes that implement this interface are visible through management instrumentation.</returns>
		// Token: 0x170000B2 RID: 178
		// (get) Token: 0x0600050C RID: 1292 RVA: 0x00023FD3 File Offset: 0x000221D3
		// (set) Token: 0x0600050D RID: 1293 RVA: 0x00023FDC File Offset: 0x000221DC
		[IgnoreMember]
		public bool Published
		{
			get
			{
				return this.published;
			}
			set
			{
				if (this.published && !value)
				{
					this.RevokeFunction(this);
					this.published = false;
					return;
				}
				if (!this.published && value)
				{
					this.PublishFunction(this);
					this.published = true;
				}
			}
		}

		// Token: 0x04000510 RID: 1296
		private ProvisionFunction publishFunction;

		// Token: 0x04000511 RID: 1297
		private ProvisionFunction revokeFunction;

		// Token: 0x04000512 RID: 1298
		private bool published;
	}
}
