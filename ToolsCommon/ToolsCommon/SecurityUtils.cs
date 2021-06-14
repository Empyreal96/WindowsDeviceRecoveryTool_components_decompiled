using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

namespace Microsoft.WindowsPhone.ImageUpdate.Tools.Common
{
	// Token: 0x02000022 RID: 34
	public class SecurityUtils
	{
		// Token: 0x0600011D RID: 285 RVA: 0x00007834 File Offset: 0x00005A34
		public static string GetFileSystemMandatoryLevel(string resourcePath)
		{
			string result = string.Empty;
			byte[] securityDescriptor = SecurityUtils.GetSecurityDescriptor(resourcePath, SecurityInformationFlags.MANDATORY_ACCESS_LABEL);
			string text = SecurityUtils.ConvertSDToStringSD(securityDescriptor, SecurityInformationFlags.MANDATORY_ACCESS_LABEL);
			if (!string.IsNullOrEmpty(text))
			{
				string text2 = text;
				char[] trimChars = new char[1];
				text = text2.TrimEnd(trimChars);
				Match match = SecurityUtils.regexExtractMIL.Match(text);
				if (match.Success)
				{
					Group group = match.Groups["MIL"];
					result = group.Value;
				}
			}
			return result;
		}

		// Token: 0x0600011E RID: 286 RVA: 0x000078A4 File Offset: 0x00005AA4
		[CLSCompliant(false)]
		public static byte[] GetSecurityDescriptor(string resourcePath, SecurityInformationFlags flags)
		{
			byte[] array = null;
			int num = 0;
			bool fileSecurity = NativeSecurityMethods.GetFileSecurity(resourcePath, flags, IntPtr.Zero, 0, ref num);
			int lastWin32Error = Marshal.GetLastWin32Error();
			if (lastWin32Error != 122)
			{
				Console.WriteLine("Error {0} Calling GetFileSecurity() on {1}", lastWin32Error, resourcePath);
				throw new Win32Exception(lastWin32Error);
			}
			int num2 = num;
			IntPtr intPtr = Marshal.AllocHGlobal(num2);
			try
			{
				if (!NativeSecurityMethods.GetFileSecurity(resourcePath, flags, intPtr, num2, ref num))
				{
					throw new Win32Exception(Marshal.GetLastWin32Error());
				}
				array = new byte[num];
				Marshal.Copy(intPtr, array, 0, num);
			}
			finally
			{
				Marshal.FreeHGlobal(intPtr);
			}
			return array;
		}

		// Token: 0x0600011F RID: 287 RVA: 0x00007940 File Offset: 0x00005B40
		[CLSCompliant(false)]
		public static string ConvertSDToStringSD(byte[] securityDescriptor, SecurityInformationFlags flags)
		{
			string result = string.Empty;
			IntPtr zero;
			int len;
			bool flag = NativeSecurityMethods.ConvertSecurityDescriptorToStringSecurityDescriptor(securityDescriptor, 1, flags, out zero, out len);
			try
			{
				if (!flag)
				{
					throw new Win32Exception(Marshal.GetLastWin32Error());
				}
				result = Marshal.PtrToStringUni(zero, len);
			}
			finally
			{
				if (zero != IntPtr.Zero)
				{
					Marshal.FreeHGlobal(zero);
				}
				zero = IntPtr.Zero;
			}
			return result;
		}

		// Token: 0x06000120 RID: 288 RVA: 0x000079A4 File Offset: 0x00005BA4
		public static AclCollection GetFileSystemACLs(string rootDir)
		{
			if (rootDir == null)
			{
				throw new ArgumentNullException("rootDir");
			}
			if (!LongPathDirectory.Exists(rootDir))
			{
				throw new ArgumentException(string.Format("Directory {0} does not exist", rootDir));
			}
			AclCollection aclCollection = new AclCollection();
			DirectoryInfo directoryInfo = new DirectoryInfo(rootDir);
			DirectoryAcl directoryAcl = new DirectoryAcl(directoryInfo, rootDir);
			if (!directoryAcl.IsEmpty)
			{
				aclCollection.Add(directoryAcl);
			}
			SecurityUtils.GetFileSystemACLsRecursive(directoryInfo, rootDir, aclCollection);
			return aclCollection;
		}

		// Token: 0x06000121 RID: 289 RVA: 0x00007A08 File Offset: 0x00005C08
		public static AclCollection GetRegistryACLs(string hiveRoot)
		{
			if (hiveRoot == null)
			{
				throw new ArgumentNullException("hiveRoot");
			}
			if (!LongPathDirectory.Exists(hiveRoot))
			{
				throw new ArgumentException(string.Format("Directory {0} does not exist", hiveRoot));
			}
			AclCollection aclCollection = new AclCollection();
			foreach (object obj in Enum.GetValues(typeof(SystemRegistryHiveFiles)))
			{
				SystemRegistryHiveFiles systemRegistryHiveFiles = (SystemRegistryHiveFiles)obj;
				string hivefile = Path.Combine(hiveRoot, Enum.GetName(typeof(SystemRegistryHiveFiles), systemRegistryHiveFiles));
				string prefix = RegistryUtils.MapHiveToMountPoint(systemRegistryHiveFiles);
				using (ORRegistryKey orregistryKey = ORRegistryKey.OpenHive(hivefile, prefix))
				{
					SecurityUtils.GetRegistryACLsRecursive(orregistryKey, aclCollection);
				}
			}
			return aclCollection;
		}

		// Token: 0x06000122 RID: 290 RVA: 0x00007AE8 File Offset: 0x00005CE8
		private static void GetFileSystemACLsRecursive(DirectoryInfo rootdi, string rootDir, AclCollection accesslist)
		{
			foreach (DirectoryInfo directoryInfo in rootdi.GetDirectories())
			{
				SecurityUtils.GetFileSystemACLsRecursive(directoryInfo, rootDir, accesslist);
				DirectoryAcl directoryAcl = new DirectoryAcl(directoryInfo, rootDir);
				if (!directoryAcl.IsEmpty)
				{
					accesslist.Add(directoryAcl);
				}
			}
			foreach (FileInfo fi in rootdi.GetFiles())
			{
				FileAcl fileAcl = new FileAcl(fi, rootDir);
				if (!fileAcl.IsEmpty)
				{
					accesslist.Add(fileAcl);
				}
			}
		}

		// Token: 0x06000123 RID: 291 RVA: 0x00007B70 File Offset: 0x00005D70
		public static void GetRegistryACLsRecursive(ORRegistryKey parent, AclCollection accesslist)
		{
			new RegistryAcl(parent);
			string[] subKeys = parent.SubKeys;
			foreach (string subkeyname in subKeys)
			{
				using (ORRegistryKey orregistryKey = parent.OpenSubKey(subkeyname))
				{
					SecurityUtils.GetRegistryACLsRecursive(orregistryKey, accesslist);
					RegistryAcl registryAcl = new RegistryAcl(orregistryKey);
					if (!registryAcl.IsEmpty)
					{
						accesslist.Add(registryAcl);
					}
				}
			}
		}

		// Token: 0x04000072 RID: 114
		private const int ERROR_INSUFFICIENT_BUFFER = 122;

		// Token: 0x04000073 RID: 115
		private static readonly Regex regexExtractMIL = new Regex("(?<MIL>\\(ML[^\\)]*\\))", RegexOptions.Compiled);
	}
}
