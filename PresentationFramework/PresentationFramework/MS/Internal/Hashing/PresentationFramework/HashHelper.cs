using System;
using System.Windows;
using System.Windows.Markup.Localizer;
using MS.Internal.Hashing.PresentationCore;

namespace MS.Internal.Hashing.PresentationFramework
{
	// Token: 0x020007F8 RID: 2040
	internal static class HashHelper
	{
		// Token: 0x06007D7D RID: 32125 RVA: 0x002341D8 File Offset: 0x002323D8
		static HashHelper()
		{
			HashHelper.Initialize();
			Type[] types = new Type[]
			{
				typeof(BamlLocalizableResource),
				typeof(ComponentResourceKey)
			};
			BaseHashHelper.RegisterTypes(typeof(HashHelper).Assembly, types);
			HashHelper.Initialize();
		}

		// Token: 0x06007D7E RID: 32126 RVA: 0x00234225 File Offset: 0x00232425
		internal static bool HasReliableHashCode(object item)
		{
			return BaseHashHelper.HasReliableHashCode(item);
		}

		// Token: 0x06007D7F RID: 32127 RVA: 0x00002137 File Offset: 0x00000337
		internal static void Initialize()
		{
		}
	}
}
