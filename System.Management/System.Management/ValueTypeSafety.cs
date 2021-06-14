using System;
using System.Runtime.CompilerServices;

namespace System.Management
{
	// Token: 0x02000047 RID: 71
	internal class ValueTypeSafety
	{
		// Token: 0x0600028C RID: 652 RVA: 0x0000DBD9 File Offset: 0x0000BDD9
		public static object GetSafeObject(object theValue)
		{
			if (theValue == null)
			{
				return null;
			}
			if (theValue.GetType().IsPrimitive)
			{
				return ((IConvertible)theValue).ToType(typeof(object), null);
			}
			return RuntimeHelpers.GetObjectValue(theValue);
		}
	}
}
