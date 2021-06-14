using System;
using Microsoft.WindowsDeviceRecoveryTool.Model.Enums;

namespace Microsoft.WindowsDeviceRecoveryTool.LogicCommon.Helpers
{
	// Token: 0x02000011 RID: 17
	public static class VersionComparer
	{
		// Token: 0x0600008E RID: 142 RVA: 0x00003348 File Offset: 0x00001548
		public static SwVersionComparisonResult CompareSoftwareVersions(string first, string second, params char[] splitChars)
		{
			SwVersionComparisonResult result;
			if (first == null || second == null)
			{
				result = SwVersionComparisonResult.UnableToCompare;
			}
			else
			{
				string[] array = first.Split(splitChars);
				string[] array2 = second.Split(splitChars);
				if (array.Length != array2.Length)
				{
					result = SwVersionComparisonResult.UnableToCompare;
				}
				else
				{
					for (int i = 0; i < array.Length; i++)
					{
						string s = array[i];
						string s2 = array2[i];
						int num;
						int num2;
						if (int.TryParse(s, out num) && int.TryParse(s2, out num2))
						{
							if (num2 > num)
							{
								return SwVersionComparisonResult.SecondIsGreater;
							}
							if (num2 < num)
							{
								return SwVersionComparisonResult.FirstIsGreater;
							}
						}
					}
					result = SwVersionComparisonResult.NumbersAreEqual;
				}
			}
			return result;
		}

		// Token: 0x0600008F RID: 143 RVA: 0x0000340C File Offset: 0x0000160C
		public static int CompareVersions(string a, string b)
		{
			int[] array = VersionComparer.ConvertVersionToTable(a);
			int[] array2 = VersionComparer.ConvertVersionToTable(b);
			for (int i = 0; i < Math.Min(array.Length, array2.Length); i++)
			{
				int num = array[i];
				int num2 = array2[i];
				if (num != num2)
				{
					return num - num2;
				}
			}
			return array.Length - array2.Length;
		}

		// Token: 0x06000090 RID: 144 RVA: 0x00003470 File Offset: 0x00001670
		private static int[] ConvertVersionToTable(string version)
		{
			string[] array = version.Split(new char[]
			{
				'.'
			});
			int[] array2 = new int[array.Length];
			for (int i = 0; i < array.Length; i++)
			{
				array2[i] = int.Parse(array[i]);
			}
			return array2;
		}

		// Token: 0x06000091 RID: 145 RVA: 0x000034C4 File Offset: 0x000016C4
		public static int Compare(string a, string b)
		{
			int[] array = VersionComparer.Convert(a);
			int[] array2 = VersionComparer.Convert(b);
			for (int i = 0; i < Math.Min(array.Length, array2.Length); i++)
			{
				int num = array[i];
				int num2 = array2[i];
				if (num != num2)
				{
					return num - num2;
				}
			}
			return array.Length - array2.Length;
		}

		// Token: 0x06000092 RID: 146 RVA: 0x00003528 File Offset: 0x00001728
		private static int[] Convert(string version)
		{
			string[] array = version.Split(new char[]
			{
				'.'
			});
			int[] array2 = new int[array.Length];
			for (int i = 0; i < array.Length; i++)
			{
				array2[i] = int.Parse(array[i]);
			}
			return array2;
		}
	}
}
