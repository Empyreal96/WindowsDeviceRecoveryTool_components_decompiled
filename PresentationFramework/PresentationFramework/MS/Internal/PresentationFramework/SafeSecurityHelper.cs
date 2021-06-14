using System;
using System.Reflection;
using System.Security;
using System.Security.Permissions;
using System.Windows;
using System.Windows.Media;
using Microsoft.Win32;

namespace MS.Internal.PresentationFramework
{
	// Token: 0x020007FE RID: 2046
	internal static class SafeSecurityHelper
	{
		// Token: 0x06007D96 RID: 32150 RVA: 0x002344F0 File Offset: 0x002326F0
		internal static string GetAssemblyPartialName(Assembly assembly)
		{
			AssemblyName assemblyName = new AssemblyName(assembly.FullName);
			string name = assemblyName.Name;
			if (name == null)
			{
				return string.Empty;
			}
			return name;
		}

		// Token: 0x06007D97 RID: 32151 RVA: 0x0023451C File Offset: 0x0023271C
		internal static string GetFullAssemblyNameFromPartialName(Assembly protoAssembly, string partialName)
		{
			return new AssemblyName(protoAssembly.FullName)
			{
				Name = partialName
			}.FullName;
		}

		// Token: 0x06007D98 RID: 32152 RVA: 0x00234544 File Offset: 0x00232744
		[SecurityCritical]
		[SecurityTreatAsSafe]
		internal static Point ClientToScreen(UIElement relativeTo, Point point)
		{
			PresentationSource presentationSource = PresentationSource.CriticalFromVisual(relativeTo);
			if (presentationSource == null)
			{
				return new Point(double.NaN, double.NaN);
			}
			GeneralTransform generalTransform = relativeTo.TransformToAncestor(presentationSource.RootVisual);
			Point point2;
			generalTransform.TryTransform(point, out point2);
			Point pointClient = PointUtil.RootToClient(point2, presentationSource);
			return PointUtil.ClientToScreen(pointClient, presentationSource);
		}

		// Token: 0x06007D99 RID: 32153 RVA: 0x002345A0 File Offset: 0x002327A0
		internal static bool IsSameKeyToken(byte[] reqKeyToken, byte[] curKeyToken)
		{
			bool result = false;
			if (reqKeyToken == null && curKeyToken == null)
			{
				result = true;
			}
			else if (reqKeyToken != null && curKeyToken != null && reqKeyToken.Length == curKeyToken.Length)
			{
				result = true;
				for (int i = 0; i < reqKeyToken.Length; i++)
				{
					if (reqKeyToken[i] != curKeyToken[i])
					{
						result = false;
						break;
					}
				}
			}
			return result;
		}

		// Token: 0x06007D9A RID: 32154 RVA: 0x002345E4 File Offset: 0x002327E4
		[SecurityCritical]
		[SecurityTreatAsSafe]
		internal static bool IsFeatureDisabled(SafeSecurityHelper.KeyToRead key)
		{
			bool flag = false;
			switch (key)
			{
			case SafeSecurityHelper.KeyToRead.WebBrowserDisable:
			{
				string name = "WebBrowserDisallow";
				goto IL_76;
			}
			case SafeSecurityHelper.KeyToRead.MediaAudioDisable:
			{
				string name = "MediaAudioDisallow";
				goto IL_76;
			}
			case (SafeSecurityHelper.KeyToRead)3:
			case (SafeSecurityHelper.KeyToRead)5:
			case (SafeSecurityHelper.KeyToRead)7:
				break;
			case SafeSecurityHelper.KeyToRead.MediaVideoDisable:
			{
				string name = "MediaVideoDisallow";
				goto IL_76;
			}
			case SafeSecurityHelper.KeyToRead.MediaAudioOrVideoDisable:
			{
				string name = "MediaAudioDisallow";
				goto IL_76;
			}
			case SafeSecurityHelper.KeyToRead.MediaImageDisable:
			{
				string name = "MediaImageDisallow";
				goto IL_76;
			}
			default:
				if (key == SafeSecurityHelper.KeyToRead.ScriptInteropDisable)
				{
					string name = "ScriptInteropDisallow";
					goto IL_76;
				}
				break;
			}
			throw new ArgumentException(key.ToString());
			IL_76:
			RegistryPermission registryPermission = new RegistryPermission(RegistryPermissionAccess.Read, "HKEY_LOCAL_MACHINE\\Software\\Microsoft\\.NETFramework\\Windows Presentation Foundation\\Features");
			registryPermission.Assert();
			try
			{
				RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("Software\\Microsoft\\.NETFramework\\Windows Presentation Foundation\\Features");
				if (registryKey != null)
				{
					string name;
					object value = registryKey.GetValue(name);
					bool flag2 = value is int && (int)value == 1;
					if (flag2)
					{
						flag = true;
					}
					if (!flag && key == SafeSecurityHelper.KeyToRead.MediaAudioOrVideoDisable)
					{
						name = "MediaVideoDisallow";
						value = registryKey.GetValue(name);
						flag2 = (value is int && (int)value == 1);
						if (flag2)
						{
							flag = true;
						}
					}
				}
			}
			finally
			{
				CodeAccessPermission.RevertAssert();
			}
			return flag;
		}

		// Token: 0x06007D9B RID: 32155 RVA: 0x00234708 File Offset: 0x00232908
		[SecurityCritical]
		[SecurityTreatAsSafe]
		internal static bool IsConnectedToPresentationSource(Visual visual)
		{
			return PresentationSource.CriticalFromVisual(visual) != null;
		}

		// Token: 0x04003B44 RID: 15172
		internal const string IMAGE = "image";

		// Token: 0x02000B8C RID: 2956
		internal enum KeyToRead
		{
			// Token: 0x04004B9E RID: 19358
			WebBrowserDisable = 1,
			// Token: 0x04004B9F RID: 19359
			MediaAudioDisable,
			// Token: 0x04004BA0 RID: 19360
			MediaVideoDisable = 4,
			// Token: 0x04004BA1 RID: 19361
			MediaImageDisable = 8,
			// Token: 0x04004BA2 RID: 19362
			MediaAudioOrVideoDisable = 6,
			// Token: 0x04004BA3 RID: 19363
			ScriptInteropDisable = 16
		}
	}
}
