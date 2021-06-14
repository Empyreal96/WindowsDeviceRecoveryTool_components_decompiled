using System;
using MS.Internal.Interop;

namespace MS.Internal.IO.Packaging
{
	// Token: 0x02000666 RID: 1638
	internal interface IManagedFilter
	{
		// Token: 0x06006C79 RID: 27769
		IFILTER_FLAGS Init(IFILTER_INIT grfFlags, ManagedFullPropSpec[] aAttributes);

		// Token: 0x06006C7A RID: 27770
		ManagedChunk GetChunk();

		// Token: 0x06006C7B RID: 27771
		string GetText(int bufferCharacterCount);

		// Token: 0x06006C7C RID: 27772
		object GetValue();
	}
}
