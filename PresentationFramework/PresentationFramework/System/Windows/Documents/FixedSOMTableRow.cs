using System;

namespace System.Windows.Documents
{
	// Token: 0x02000366 RID: 870
	internal sealed class FixedSOMTableRow : FixedSOMContainer
	{
		// Token: 0x06002E33 RID: 11827 RVA: 0x000CE54C File Offset: 0x000CC74C
		public void AddCell(FixedSOMTableCell cell)
		{
			base.Add(cell);
		}

		// Token: 0x17000B8C RID: 2956
		// (get) Token: 0x06002E34 RID: 11828 RVA: 0x000D0D59 File Offset: 0x000CEF59
		internal override FixedElement.ElementType[] ElementTypes
		{
			get
			{
				return new FixedElement.ElementType[]
				{
					FixedElement.ElementType.TableRow
				};
			}
		}

		// Token: 0x17000B8D RID: 2957
		// (get) Token: 0x06002E35 RID: 11829 RVA: 0x000D0D68 File Offset: 0x000CEF68
		internal bool IsEmpty
		{
			get
			{
				foreach (FixedSOMSemanticBox fixedSOMSemanticBox in base.SemanticBoxes)
				{
					FixedSOMTableCell fixedSOMTableCell = (FixedSOMTableCell)fixedSOMSemanticBox;
					if (!fixedSOMTableCell.IsEmpty)
					{
						return false;
					}
				}
				return true;
			}
		}
	}
}
