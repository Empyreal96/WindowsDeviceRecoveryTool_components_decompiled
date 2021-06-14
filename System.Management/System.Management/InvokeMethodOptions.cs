using System;

namespace System.Management
{
	/// <summary>Specifies options for invoking a management method.          </summary>
	// Token: 0x02000031 RID: 49
	public class InvokeMethodOptions : ManagementOptions
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Management.InvokeMethodOptions" /> class for the <see cref="M:System.Management.ManagementObject.InvokeMethod(System.String,System.Object[])" /> operation, using default values. This is the default constructor.          </summary>
		// Token: 0x06000174 RID: 372 RVA: 0x000085FF File Offset: 0x000067FF
		public InvokeMethodOptions()
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Management.InvokeMethodOptions" /> class for an invoke operation using the specified values.          </summary>
		/// <param name="context">A provider-specific, named-value pairs object to be passed through to the provider.</param>
		/// <param name="timeout">The length of time to let the operation perform before it times out. The default value is <see cref="F:System.TimeSpan.MaxValue" />. Setting this parameter will invoke the operation semisynchronously.</param>
		// Token: 0x06000175 RID: 373 RVA: 0x00008607 File Offset: 0x00006807
		public InvokeMethodOptions(ManagementNamedValueCollection context, TimeSpan timeout) : base(context, timeout)
		{
		}

		/// <summary>Returns a copy of the object.          </summary>
		/// <returns>The cloned object.</returns>
		// Token: 0x06000176 RID: 374 RVA: 0x00008644 File Offset: 0x00006844
		public override object Clone()
		{
			ManagementNamedValueCollection context = null;
			if (base.Context != null)
			{
				context = base.Context.Clone();
			}
			return new InvokeMethodOptions(context, base.Timeout);
		}
	}
}
