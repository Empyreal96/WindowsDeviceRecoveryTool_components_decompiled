using System;
using System.Reflection;
using System.Security;
using System.Security.Permissions;

namespace System.Windows.Forms
{
	// Token: 0x02000105 RID: 261
	internal static class SecurityUtils
	{
		// Token: 0x1700016E RID: 366
		// (get) Token: 0x0600044F RID: 1103 RVA: 0x0000D98C File Offset: 0x0000BB8C
		private static ReflectionPermission MemberAccessPermission
		{
			get
			{
				if (SecurityUtils.memberAccessPermission == null)
				{
					SecurityUtils.memberAccessPermission = new ReflectionPermission(ReflectionPermissionFlag.MemberAccess);
				}
				return SecurityUtils.memberAccessPermission;
			}
		}

		// Token: 0x1700016F RID: 367
		// (get) Token: 0x06000450 RID: 1104 RVA: 0x0000D9AB File Offset: 0x0000BBAB
		private static ReflectionPermission RestrictedMemberAccessPermission
		{
			get
			{
				if (SecurityUtils.restrictedMemberAccessPermission == null)
				{
					SecurityUtils.restrictedMemberAccessPermission = new ReflectionPermission(ReflectionPermissionFlag.RestrictedMemberAccess);
				}
				return SecurityUtils.restrictedMemberAccessPermission;
			}
		}

		// Token: 0x06000451 RID: 1105 RVA: 0x0000D9CC File Offset: 0x0000BBCC
		private static void DemandReflectionAccess(Type type)
		{
			try
			{
				SecurityUtils.MemberAccessPermission.Demand();
			}
			catch (SecurityException)
			{
				SecurityUtils.DemandGrantSet(type.Assembly);
			}
		}

		// Token: 0x06000452 RID: 1106 RVA: 0x0000DA04 File Offset: 0x0000BC04
		[SecuritySafeCritical]
		private static void DemandGrantSet(Assembly assembly)
		{
			PermissionSet permissionSet = assembly.PermissionSet;
			permissionSet.AddPermission(SecurityUtils.RestrictedMemberAccessPermission);
			permissionSet.Demand();
		}

		// Token: 0x06000453 RID: 1107 RVA: 0x0000DA2C File Offset: 0x0000BC2C
		private static bool HasReflectionPermission(Type type)
		{
			try
			{
				SecurityUtils.DemandReflectionAccess(type);
				return true;
			}
			catch (SecurityException)
			{
			}
			return false;
		}

		// Token: 0x06000454 RID: 1108 RVA: 0x0000DA5C File Offset: 0x0000BC5C
		internal static object SecureCreateInstance(Type type)
		{
			return SecurityUtils.SecureCreateInstance(type, null, false);
		}

		// Token: 0x06000455 RID: 1109 RVA: 0x0000DA68 File Offset: 0x0000BC68
		internal static object SecureCreateInstance(Type type, object[] args, bool allowNonPublic)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.CreateInstance;
			if (!type.IsVisible)
			{
				SecurityUtils.DemandReflectionAccess(type);
			}
			else if (allowNonPublic && !SecurityUtils.HasReflectionPermission(type))
			{
				allowNonPublic = false;
			}
			if (allowNonPublic)
			{
				bindingFlags |= BindingFlags.NonPublic;
			}
			return Activator.CreateInstance(type, bindingFlags, null, args, null);
		}

		// Token: 0x0400044F RID: 1103
		private static volatile ReflectionPermission memberAccessPermission;

		// Token: 0x04000450 RID: 1104
		private static volatile ReflectionPermission restrictedMemberAccessPermission;
	}
}
