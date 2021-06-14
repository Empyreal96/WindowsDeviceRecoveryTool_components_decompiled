using System;
using System.Collections.Generic;
using Microsoft.Data.Edm;

namespace Microsoft.Data.OData.Query.SemanticAst
{
	// Token: 0x0200004B RID: 75
	public class ODataExpandPath : ODataPath
	{
		// Token: 0x060001F6 RID: 502 RVA: 0x00007C29 File Offset: 0x00005E29
		public ODataExpandPath(IEnumerable<ODataPathSegment> segments) : base(segments)
		{
			this.ValidatePath();
		}

		// Token: 0x060001F7 RID: 503 RVA: 0x00007C38 File Offset: 0x00005E38
		public ODataExpandPath(params ODataPathSegment[] segments) : base(segments)
		{
			this.ValidatePath();
		}

		// Token: 0x060001F8 RID: 504 RVA: 0x00007C47 File Offset: 0x00005E47
		internal IEdmNavigationProperty GetNavigationProperty()
		{
			return ((NavigationPropertySegment)base.LastSegment).NavigationProperty;
		}

		// Token: 0x060001F9 RID: 505 RVA: 0x00007C5C File Offset: 0x00005E5C
		private void ValidatePath()
		{
			int num = 0;
			bool flag = false;
			foreach (ODataPathSegment odataPathSegment in this)
			{
				if (odataPathSegment is TypeSegment)
				{
					if (num == base.Count - 1)
					{
						throw new ODataException(Strings.ODataExpandPath_OnlyLastSegmentMustBeNavigationProperty);
					}
				}
				else
				{
					if (!(odataPathSegment is NavigationPropertySegment))
					{
						throw new ODataException(Strings.ODataExpandPath_InvalidExpandPathSegment(odataPathSegment.GetType().Name));
					}
					if (num < base.Count - 1 || flag)
					{
						throw new ODataException(Strings.ODataExpandPath_OnlyLastSegmentMustBeNavigationProperty);
					}
					flag = true;
				}
				num++;
			}
		}
	}
}
