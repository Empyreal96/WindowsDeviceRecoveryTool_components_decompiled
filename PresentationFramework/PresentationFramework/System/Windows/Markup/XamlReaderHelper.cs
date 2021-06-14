using System;
using System.Xaml;

namespace System.Windows.Markup
{
	// Token: 0x0200026D RID: 621
	internal class XamlReaderHelper
	{
		// Token: 0x1700089E RID: 2206
		// (get) Token: 0x06002380 RID: 9088 RVA: 0x000AD780 File Offset: 0x000AB980
		internal static XamlDirective Freeze
		{
			get
			{
				if (XamlReaderHelper._freezeDirective == null)
				{
					XamlReaderHelper._freezeDirective = new XamlDirective("http://schemas.microsoft.com/winfx/2006/xaml/presentation/options", "Freeze");
				}
				return XamlReaderHelper._freezeDirective;
			}
		}

		// Token: 0x04001ACF RID: 6863
		internal const string DefinitionNamespaceURI = "http://schemas.microsoft.com/winfx/2006/xaml";

		// Token: 0x04001AD0 RID: 6864
		internal const string DefinitionUid = "Uid";

		// Token: 0x04001AD1 RID: 6865
		internal const string DefinitionType = "Type";

		// Token: 0x04001AD2 RID: 6866
		internal const string DefinitionTypeArgs = "TypeArguments";

		// Token: 0x04001AD3 RID: 6867
		internal const string DefinitionName = "Key";

		// Token: 0x04001AD4 RID: 6868
		internal const string DefinitionRuntimeName = "Name";

		// Token: 0x04001AD5 RID: 6869
		internal const string DefinitionShared = "Shared";

		// Token: 0x04001AD6 RID: 6870
		internal const string DefinitionSynchronousMode = "SynchronousMode";

		// Token: 0x04001AD7 RID: 6871
		internal const string DefinitionAsyncRecords = "AsyncRecords";

		// Token: 0x04001AD8 RID: 6872
		internal const string DefinitionContent = "Content";

		// Token: 0x04001AD9 RID: 6873
		internal const string DefinitionClass = "Class";

		// Token: 0x04001ADA RID: 6874
		internal const string DefinitionSubclass = "Subclass";

		// Token: 0x04001ADB RID: 6875
		internal const string DefinitionClassModifier = "ClassModifier";

		// Token: 0x04001ADC RID: 6876
		internal const string DefinitionFieldModifier = "FieldModifier";

		// Token: 0x04001ADD RID: 6877
		internal const string DefinitionCodeTag = "Code";

		// Token: 0x04001ADE RID: 6878
		internal const string DefinitionXDataTag = "XData";

		// Token: 0x04001ADF RID: 6879
		internal const string MappingProtocol = "clr-namespace:";

		// Token: 0x04001AE0 RID: 6880
		internal const string MappingAssembly = ";assembly=";

		// Token: 0x04001AE1 RID: 6881
		internal const string PresentationOptionsFreeze = "Freeze";

		// Token: 0x04001AE2 RID: 6882
		internal const string DefaultNamespaceURI = "http://schemas.microsoft.com/winfx/2006/xaml/presentation";

		// Token: 0x04001AE3 RID: 6883
		internal const string DefinitionMetroNamespaceURI = "http://schemas.microsoft.com/xps/2005/06/resourcedictionary-key";

		// Token: 0x04001AE4 RID: 6884
		internal const string PresentationOptionsNamespaceURI = "http://schemas.microsoft.com/winfx/2006/xaml/presentation/options";

		// Token: 0x04001AE5 RID: 6885
		private static XamlDirective _freezeDirective;
	}
}
