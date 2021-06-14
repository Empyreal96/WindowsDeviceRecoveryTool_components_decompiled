using System;

namespace MS.Internal.PresentationFramework
{
	// Token: 0x020007FC RID: 2044
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum | AttributeTargets.Constructor | AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Interface | AttributeTargets.Delegate, AllowMultiple = false, Inherited = true)]
	internal sealed class FriendAccessAllowedAttribute : Attribute
	{
	}
}
