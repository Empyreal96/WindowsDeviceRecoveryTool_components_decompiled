using System;

namespace System.Management
{
	/// <summary>Specifies options for deleting a management object.          </summary>
	// Token: 0x02000030 RID: 48
	public class DeleteOptions : ManagementOptions
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Management.DeleteOptions" /> class for the delete operation, using default values. This is the default constructor.          </summary>
		// Token: 0x06000171 RID: 369 RVA: 0x000085FF File Offset: 0x000067FF
		public DeleteOptions()
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Management.DeleteOptions" /> class for a delete operation, using the specified values.          </summary>
		/// <param name="context">A provider-specific, named-value pairs object to be passed through to the provider. </param>
		/// <param name="timeout">The length of time to let the operation perform before it times out. The default value is <see cref="F:System.TimeSpan.MaxValue" />. Setting this parameter will invoke the operation semisynchronously.</param>
		// Token: 0x06000172 RID: 370 RVA: 0x00008607 File Offset: 0x00006807
		public DeleteOptions(ManagementNamedValueCollection context, TimeSpan timeout) : base(context, timeout)
		{
		}

		/// <summary>Returns a copy of the object.          </summary>
		/// <returns>A cloned object.</returns>
		// Token: 0x06000173 RID: 371 RVA: 0x00008614 File Offset: 0x00006814
		public override object Clone()
		{
			ManagementNamedValueCollection context = null;
			if (base.Context != null)
			{
				context = base.Context.Clone();
			}
			return new DeleteOptions(context, base.Timeout);
		}
	}
}
