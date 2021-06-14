using System;

namespace MS.Internal.AppModel
{
	// Token: 0x02000798 RID: 1944
	[Serializable]
	internal struct ReturnEventSaverInfo
	{
		// Token: 0x060079DD RID: 31197 RVA: 0x002288DE File Offset: 0x00226ADE
		internal ReturnEventSaverInfo(string delegateTypeName, string targetTypeName, string delegateMethodName, bool fSamePf)
		{
			this._delegateTypeName = delegateTypeName;
			this._targetTypeName = targetTypeName;
			this._delegateMethodName = delegateMethodName;
			this._delegateInSamePF = fSamePf;
		}

		// Token: 0x040039A1 RID: 14753
		internal string _delegateTypeName;

		// Token: 0x040039A2 RID: 14754
		internal string _targetTypeName;

		// Token: 0x040039A3 RID: 14755
		internal string _delegateMethodName;

		// Token: 0x040039A4 RID: 14756
		internal bool _delegateInSamePF;
	}
}
