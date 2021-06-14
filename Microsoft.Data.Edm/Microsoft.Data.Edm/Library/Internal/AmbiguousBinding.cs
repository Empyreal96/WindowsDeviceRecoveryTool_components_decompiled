using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Data.Edm.Validation;

namespace Microsoft.Data.Edm.Library.Internal
{
	// Token: 0x020000D9 RID: 217
	internal class AmbiguousBinding<TElement> : BadElement where TElement : class, IEdmNamedElement
	{
		// Token: 0x0600045C RID: 1116 RVA: 0x0000BE68 File Offset: 0x0000A068
		public AmbiguousBinding(TElement first, TElement second) : base(new EdmError[]
		{
			new EdmError(null, EdmErrorCode.BadAmbiguousElementBinding, Strings.Bad_AmbiguousElementBinding(first.Name))
		})
		{
			this.AddBinding(first);
			this.AddBinding(second);
		}

		// Token: 0x170001C9 RID: 457
		// (get) Token: 0x0600045D RID: 1117 RVA: 0x0000BEBC File Offset: 0x0000A0BC
		public IEnumerable<TElement> Bindings
		{
			get
			{
				return this.bindings;
			}
		}

		// Token: 0x170001CA RID: 458
		// (get) Token: 0x0600045E RID: 1118 RVA: 0x0000BEC4 File Offset: 0x0000A0C4
		public string Name
		{
			get
			{
				TElement telement = this.bindings.First<TElement>();
				return telement.Name ?? string.Empty;
			}
		}

		// Token: 0x0600045F RID: 1119 RVA: 0x0000BEF3 File Offset: 0x0000A0F3
		public void AddBinding(TElement binding)
		{
			if (!this.bindings.Contains(binding))
			{
				this.bindings.Add(binding);
			}
		}

		// Token: 0x040001AB RID: 427
		private readonly List<TElement> bindings = new List<TElement>();
	}
}
