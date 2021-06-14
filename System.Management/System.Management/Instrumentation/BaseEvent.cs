using System;

namespace System.Management.Instrumentation
{
	/// <summary>Represents classes derived from <see cref="T:System.Management.Instrumentation.BaseEvent" /> that are known to be management event classes. These derived classes inherit an implementation of <see cref="T:System.Management.Instrumentation.IEvent" /> that allows events to be fired through the <see cref="M:System.Management.Instrumentation.BaseEvent.Fire" /> method.Note: the WMI .NET libraries are now considered in final state, and no further development, enhancements, or updates will be available for non-security related issues affecting these libraries. The MI APIs should be used for all new development.</summary>
	// Token: 0x020000BB RID: 187
	[InstrumentationClass(InstrumentationType.Event)]
	public abstract class BaseEvent : IEvent
	{
		// Token: 0x170000AE RID: 174
		// (get) Token: 0x06000505 RID: 1285 RVA: 0x00023F62 File Offset: 0x00022162
		private ProvisionFunction FireFunction
		{
			get
			{
				if (this.fireFunction == null)
				{
					this.fireFunction = Instrumentation.GetFireFunction(base.GetType());
				}
				return this.fireFunction;
			}
		}

		/// <summary>Raises a management event.</summary>
		// Token: 0x06000506 RID: 1286 RVA: 0x00023F83 File Offset: 0x00022183
		public void Fire()
		{
			this.FireFunction(this);
		}

		// Token: 0x0400050F RID: 1295
		private ProvisionFunction fireFunction;
	}
}
