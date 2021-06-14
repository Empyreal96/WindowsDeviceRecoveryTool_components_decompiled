using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace Microsoft.WindowsPhone.ImageUpdate.Tools.Common
{
	// Token: 0x0200001D RID: 29
	public static class RegValidator
	{
		// Token: 0x060000FE RID: 254
		[DllImport("UpdateDLL.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
		public static extern int ValidateRegistryHive(string RegHive);

		// Token: 0x060000FF RID: 255
		[DllImport("UpdateDLL.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
		private static extern int ValidateRegFiles(string[] rgRegFiles, int cRegFiles, string[] rgaFiles, int cRgaFiles);

		// Token: 0x06000100 RID: 256 RVA: 0x00007720 File Offset: 0x00005920
		public static void Validate(IEnumerable<string> regFiles, IEnumerable<string> rgaFiles)
		{
			string[] array = (regFiles != null) ? regFiles.ToArray<string>() : new string[0];
			string[] array2 = (rgaFiles != null) ? rgaFiles.ToArray<string>() : new string[0];
			if (array.Length == 0 && array2.Length == 0)
			{
				return;
			}
			int num = RegValidator.ValidateRegFiles(array, array.Length, array2, array2.Length);
			if (num != 0)
			{
				throw new IUException("Registry validation failed, check output log for detailed failure information, err '0x{0:X8}'", new object[]
				{
					num
				});
			}
		}
	}
}
