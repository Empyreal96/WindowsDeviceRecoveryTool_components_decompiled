using System;
using System.Globalization;

namespace System.Windows.Forms.PropertyGridInternal
{
	// Token: 0x0200047A RID: 1146
	internal class ArrayElementGridEntry : GridEntry
	{
		// Token: 0x06004D25 RID: 19749 RVA: 0x0013C7A1 File Offset: 0x0013A9A1
		public ArrayElementGridEntry(PropertyGrid ownerGrid, GridEntry peParent, int index) : base(ownerGrid, peParent)
		{
			this.index = index;
			this.SetFlag(256, (peParent.Flags & 256) != 0 || peParent.ForceReadOnly);
		}

		// Token: 0x17001305 RID: 4869
		// (get) Token: 0x06004D26 RID: 19750 RVA: 0x0000E211 File Offset: 0x0000C411
		public override GridItemType GridItemType
		{
			get
			{
				return GridItemType.ArrayValue;
			}
		}

		// Token: 0x17001306 RID: 4870
		// (get) Token: 0x06004D27 RID: 19751 RVA: 0x0013C7D4 File Offset: 0x0013A9D4
		public override bool IsValueEditable
		{
			get
			{
				return this.ParentGridEntry.IsValueEditable;
			}
		}

		// Token: 0x17001307 RID: 4871
		// (get) Token: 0x06004D28 RID: 19752 RVA: 0x0013C7E1 File Offset: 0x0013A9E1
		public override string PropertyLabel
		{
			get
			{
				return "[" + this.index.ToString(CultureInfo.CurrentCulture) + "]";
			}
		}

		// Token: 0x17001308 RID: 4872
		// (get) Token: 0x06004D29 RID: 19753 RVA: 0x0013C802 File Offset: 0x0013AA02
		public override Type PropertyType
		{
			get
			{
				return this.parentPE.PropertyType.GetElementType();
			}
		}

		// Token: 0x17001309 RID: 4873
		// (get) Token: 0x06004D2A RID: 19754 RVA: 0x0013C814 File Offset: 0x0013AA14
		// (set) Token: 0x06004D2B RID: 19755 RVA: 0x0013C83C File Offset: 0x0013AA3C
		public override object PropertyValue
		{
			get
			{
				object valueOwner = this.GetValueOwner();
				return ((Array)valueOwner).GetValue(this.index);
			}
			set
			{
				object valueOwner = this.GetValueOwner();
				((Array)valueOwner).SetValue(value, this.index);
			}
		}

		// Token: 0x1700130A RID: 4874
		// (get) Token: 0x06004D2C RID: 19756 RVA: 0x0013C862 File Offset: 0x0013AA62
		public override bool ShouldRenderReadOnly
		{
			get
			{
				return this.ParentGridEntry.ShouldRenderReadOnly;
			}
		}

		// Token: 0x040032E3 RID: 13027
		protected int index;
	}
}
