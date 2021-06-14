using System;

namespace System.Windows.Markup
{
	// Token: 0x02000212 RID: 530
	internal class BamlWriterXamlTypeMapper : XamlTypeMapper
	{
		// Token: 0x06002107 RID: 8455 RVA: 0x00098067 File Offset: 0x00096267
		internal BamlWriterXamlTypeMapper(string[] assemblyNames, NamespaceMapEntry[] namespaceMaps) : base(assemblyNames, namespaceMaps)
		{
		}

		// Token: 0x06002108 RID: 8456 RVA: 0x00016748 File Offset: 0x00014948
		protected sealed override bool AllowInternalType(Type type)
		{
			return true;
		}
	}
}
