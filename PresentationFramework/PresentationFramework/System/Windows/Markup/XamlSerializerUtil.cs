using System;

namespace System.Windows.Markup
{
	// Token: 0x02000267 RID: 615
	internal static class XamlSerializerUtil
	{
		// Token: 0x0600233B RID: 9019 RVA: 0x000ACE74 File Offset: 0x000AB074
		internal static void ThrowIfNonWhiteSpaceInAddText(string s, object parent)
		{
			if (s != null)
			{
				for (int i = 0; i < s.Length; i++)
				{
					if (!char.IsWhiteSpace(s[i]))
					{
						throw new ArgumentException(SR.Get("NonWhiteSpaceInAddText", new object[]
						{
							s
						}));
					}
				}
			}
		}
	}
}
