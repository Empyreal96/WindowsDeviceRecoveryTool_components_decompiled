using System;
using System.Collections.Generic;

namespace System.Windows.Markup.Primitives
{
	// Token: 0x02000285 RID: 645
	internal class ExtensionSimplifierMarkupObject : MarkupObjectWrapper
	{
		// Token: 0x06002473 RID: 9331 RVA: 0x000B0BE2 File Offset: 0x000AEDE2
		public ExtensionSimplifierMarkupObject(MarkupObject baseObject, IValueSerializerContext context) : base(baseObject)
		{
			this._context = context;
		}

		// Token: 0x06002474 RID: 9332 RVA: 0x000B0BF2 File Offset: 0x000AEDF2
		private IEnumerable<MarkupProperty> GetBaseProperties(bool mapToConstructorArgs)
		{
			return base.GetProperties(mapToConstructorArgs);
		}

		// Token: 0x06002475 RID: 9333 RVA: 0x000B0BFB File Offset: 0x000AEDFB
		internal override IEnumerable<MarkupProperty> GetProperties(bool mapToConstructorArgs)
		{
			foreach (MarkupProperty baseProperty in this.GetBaseProperties(mapToConstructorArgs))
			{
				yield return new ExtensionSimplifierProperty(baseProperty, this._context);
			}
			IEnumerator<MarkupProperty> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x06002476 RID: 9334 RVA: 0x000B0C12 File Offset: 0x000AEE12
		public override void AssignRootContext(IValueSerializerContext context)
		{
			this._context = context;
			base.AssignRootContext(context);
		}

		// Token: 0x04001B33 RID: 6963
		private IValueSerializerContext _context;
	}
}
