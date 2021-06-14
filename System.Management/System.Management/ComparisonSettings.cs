using System;

namespace System.Management
{
	/// <summary>Describes the object comparison modes that can be used with <see cref="M:System.Management.ManagementBaseObject.CompareTo(System.Management.ManagementBaseObject,System.Management.ComparisonSettings)" />. Note that these values may be combined.          </summary>
	// Token: 0x02000007 RID: 7
	[Flags]
	public enum ComparisonSettings
	{
		/// <summary>A mode that compares all elements of the compared objects. Value: 0. </summary>
		// Token: 0x04000067 RID: 103
		IncludeAll = 0,
		/// <summary>A mode that compares the objects, ignoring qualifiers. Value: 1.</summary>
		// Token: 0x04000068 RID: 104
		IgnoreQualifiers = 1,
		/// <summary>A mode that ignores the source of the objects, namely the server and the namespace they came from, in comparison to other objects. Value: 2.</summary>
		// Token: 0x04000069 RID: 105
		IgnoreObjectSource = 2,
		/// <summary>A mode that ignores the default values of properties. This value is only meaningful when comparing classes. Value: 4.</summary>
		// Token: 0x0400006A RID: 106
		IgnoreDefaultValues = 4,
		/// <summary>A mode that assumes that the objects being compared are instances of                    the same class. Consequently, this value causes comparison of instance-related information only. Use this flag to optimize performance. If the objects are not of the same class, the results are undefined. Value: 8.</summary>
		// Token: 0x0400006B RID: 107
		IgnoreClass = 8,
		/// <summary>A mode that compares string values in a case-insensitive manner. This applies to strings and to qualifier values. Property and qualifier names are always compared in a case-insensitive manner whether this flag is specified or not. Value: 16.</summary>
		// Token: 0x0400006C RID: 108
		IgnoreCase = 16,
		/// <summary>A mode that ignores qualifier flavors. This flag still takes qualifier values into account, but ignores flavor distinctions such as propagation rules and override restrictions. Value: 32.</summary>
		// Token: 0x0400006D RID: 109
		IgnoreFlavor = 32
	}
}
