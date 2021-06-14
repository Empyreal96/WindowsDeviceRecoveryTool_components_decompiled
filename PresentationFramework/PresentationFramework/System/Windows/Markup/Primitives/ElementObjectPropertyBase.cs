using System;

namespace System.Windows.Markup.Primitives
{
	// Token: 0x0200027A RID: 634
	internal abstract class ElementObjectPropertyBase : ElementPropertyBase
	{
		// Token: 0x06002428 RID: 9256 RVA: 0x000B0589 File Offset: 0x000AE789
		protected ElementObjectPropertyBase(ElementMarkupObject obj) : base(obj.Manager)
		{
			this._object = obj;
		}

		// Token: 0x06002429 RID: 9257 RVA: 0x000B059E File Offset: 0x000AE79E
		protected override IValueSerializerContext GetItemContext()
		{
			return this._object.Context;
		}

		// Token: 0x0600242A RID: 9258 RVA: 0x000B05AB File Offset: 0x000AE7AB
		protected override Type GetObjectType()
		{
			return this._object.ObjectType;
		}

		// Token: 0x04001B26 RID: 6950
		protected readonly ElementMarkupObject _object;
	}
}
