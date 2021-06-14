using System;

namespace System.Management
{
	/// <summary>Describes the possible effects of saving an object to WMI when using <see cref="M:System.Management.ManagementObject.Put" />.          </summary>
	// Token: 0x0200002A RID: 42
	public enum PutType
	{
		/// <summary>No change.</summary>
		// Token: 0x04000131 RID: 305
		None,
		/// <summary>Updates an existing object only; does not create a new object.</summary>
		// Token: 0x04000132 RID: 306
		UpdateOnly,
		/// <summary>Creates an object only; does not update an existing object.</summary>
		// Token: 0x04000133 RID: 307
		CreateOnly,
		/// <summary>Saves the object, whether updating an existing object or creating a new object.</summary>
		// Token: 0x04000134 RID: 308
		UpdateOrCreate
	}
}
