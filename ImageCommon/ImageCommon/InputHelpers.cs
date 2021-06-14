using System;
using System.Globalization;

namespace Microsoft.WindowsPhone.Imaging
{
	// Token: 0x0200000D RID: 13
	internal class InputHelpers
	{
		// Token: 0x060000B5 RID: 181 RVA: 0x00004B94 File Offset: 0x00002D94
		public static bool StringToUint(string valueAsString, out uint value)
		{
			bool result = true;
			if (valueAsString.StartsWith("0x"))
			{
				if (!uint.TryParse(valueAsString.Substring(2, valueAsString.Length - 2), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out value))
				{
					result = false;
				}
			}
			else if (!uint.TryParse(valueAsString, NumberStyles.Integer, CultureInfo.InvariantCulture, out value))
			{
				result = false;
			}
			return result;
		}

		// Token: 0x060000B6 RID: 182 RVA: 0x00004BE7 File Offset: 0x00002DE7
		public static bool IsPowerOfTwo(uint value)
		{
			return (value & value - 1U) == 0U;
		}
	}
}
