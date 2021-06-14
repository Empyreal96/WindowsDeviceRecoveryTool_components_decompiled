using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Windows;

namespace MS.Internal.IO.Packaging
{
	// Token: 0x02000663 RID: 1635
	internal class ManagedPropSpec
	{
		// Token: 0x170019E2 RID: 6626
		// (get) Token: 0x06006C61 RID: 27745 RVA: 0x001F3A3F File Offset: 0x001F1C3F
		internal PropSpecType PropType
		{
			get
			{
				return this._propType;
			}
		}

		// Token: 0x170019E3 RID: 6627
		// (get) Token: 0x06006C62 RID: 27746 RVA: 0x001F3A47 File Offset: 0x001F1C47
		// (set) Token: 0x06006C63 RID: 27747 RVA: 0x001F3A4F File Offset: 0x001F1C4F
		internal string PropName
		{
			get
			{
				return this._name;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				this._name = value;
				this._id = 0U;
				this._propType = PropSpecType.Name;
			}
		}

		// Token: 0x170019E4 RID: 6628
		// (get) Token: 0x06006C64 RID: 27748 RVA: 0x001F3A74 File Offset: 0x001F1C74
		// (set) Token: 0x06006C65 RID: 27749 RVA: 0x001F3A7C File Offset: 0x001F1C7C
		internal uint PropId
		{
			get
			{
				return this._id;
			}
			set
			{
				this._id = value;
				this._name = null;
				this._propType = PropSpecType.Id;
			}
		}

		// Token: 0x06006C66 RID: 27750 RVA: 0x001F3A93 File Offset: 0x001F1C93
		internal ManagedPropSpec(uint id)
		{
			this.PropId = id;
		}

		// Token: 0x06006C67 RID: 27751 RVA: 0x001F3AA4 File Offset: 0x001F1CA4
		[SecurityCritical]
		[SecurityTreatAsSafe]
		internal ManagedPropSpec(PROPSPEC propSpec)
		{
			SecurityHelper.DemandUnmanagedCode();
			PropSpecType propType = (PropSpecType)propSpec.propType;
			if (propType == PropSpecType.Name)
			{
				this.PropName = Marshal.PtrToStringUni(propSpec.union.name);
				return;
			}
			if (propType == PropSpecType.Id)
			{
				this.PropId = propSpec.union.propId;
				return;
			}
			throw new ArgumentException(SR.Get("FilterPropSpecUnknownUnionSelector"), "propSpec");
		}

		// Token: 0x0400353A RID: 13626
		private PropSpecType _propType;

		// Token: 0x0400353B RID: 13627
		private uint _id;

		// Token: 0x0400353C RID: 13628
		private string _name;
	}
}
