using System;
using System.Collections.ObjectModel;
using System.Data.Services.Client;
using System.Linq;

namespace System.Data.Services.Common
{
	// Token: 0x020000F8 RID: 248
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
	public sealed class DataServiceKeyAttribute : Attribute
	{
		// Token: 0x0600082F RID: 2095 RVA: 0x00022EA8 File Offset: 0x000210A8
		public DataServiceKeyAttribute(string keyName)
		{
			Util.CheckArgumentNull<string>(keyName, "keyName");
			Util.CheckArgumentNullAndEmpty(keyName, "KeyName");
			this.keyNames = new ReadOnlyCollection<string>(new string[]
			{
				keyName
			});
		}

		// Token: 0x06000830 RID: 2096 RVA: 0x00022EFC File Offset: 0x000210FC
		public DataServiceKeyAttribute(params string[] keyNames)
		{
			Util.CheckArgumentNull<string[]>(keyNames, "keyNames");
			if (keyNames.Length != 0)
			{
				if (!keyNames.Any((string f) => f == null || f.Length == 0))
				{
					this.keyNames = new ReadOnlyCollection<string>(keyNames);
					return;
				}
			}
			throw Error.Argument(Strings.DSKAttribute_MustSpecifyAtleastOnePropertyName, "keyNames");
		}

		// Token: 0x170001DB RID: 475
		// (get) Token: 0x06000831 RID: 2097 RVA: 0x00022F61 File Offset: 0x00021161
		public ReadOnlyCollection<string> KeyNames
		{
			get
			{
				return this.keyNames;
			}
		}

		// Token: 0x040004E1 RID: 1249
		private readonly ReadOnlyCollection<string> keyNames;
	}
}
