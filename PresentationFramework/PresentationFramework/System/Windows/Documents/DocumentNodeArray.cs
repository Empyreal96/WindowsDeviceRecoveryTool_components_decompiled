using System;
using System.Collections;
using System.Text;
using MS.Internal;

namespace System.Windows.Documents
{
	// Token: 0x020003D1 RID: 977
	internal class DocumentNodeArray : ArrayList
	{
		// Token: 0x0600348B RID: 13451 RVA: 0x000E99DE File Offset: 0x000E7BDE
		internal DocumentNodeArray() : base(100)
		{
			this._fMain = false;
			this._dnaOpen = null;
		}

		// Token: 0x0600348C RID: 13452 RVA: 0x000E99F6 File Offset: 0x000E7BF6
		internal DocumentNode EntryAt(int nAt)
		{
			return (DocumentNode)this[nAt];
		}

		// Token: 0x0600348D RID: 13453 RVA: 0x000E9A04 File Offset: 0x000E7C04
		internal void Push(DocumentNode documentNode)
		{
			this.InsertNode(this.Count, documentNode);
		}

		// Token: 0x0600348E RID: 13454 RVA: 0x000E9A14 File Offset: 0x000E7C14
		internal DocumentNode Pop()
		{
			DocumentNode top = this.Top;
			if (this.Count > 0)
			{
				this.Excise(this.Count - 1, 1);
			}
			return top;
		}

		// Token: 0x0600348F RID: 13455 RVA: 0x000E9A44 File Offset: 0x000E7C44
		internal DocumentNode TopPending()
		{
			for (int i = this.Count - 1; i >= 0; i--)
			{
				DocumentNode documentNode = this.EntryAt(i);
				if (documentNode.IsPending)
				{
					return documentNode;
				}
			}
			return null;
		}

		// Token: 0x06003490 RID: 13456 RVA: 0x000E9A77 File Offset: 0x000E7C77
		internal bool TestTop(DocumentNodeType documentNodeType)
		{
			return this.Count > 0 && this.EntryAt(this.Count - 1).Type == documentNodeType;
		}

		// Token: 0x06003491 RID: 13457 RVA: 0x000E9A9C File Offset: 0x000E7C9C
		internal void PreCoalesceChildren(ConverterState converterState, int nStart, bool bChild)
		{
			DocumentNodeArray documentNodeArray = new DocumentNodeArray();
			bool flag = false;
			DocumentNode documentNode = this.EntryAt(nStart);
			int num = documentNode.ChildCount;
			if (nStart + num >= this.Count)
			{
				num = this.Count - nStart - 1;
			}
			int num2 = nStart + num;
			if (bChild)
			{
				nStart++;
			}
			for (int i = nStart; i <= num2; i++)
			{
				DocumentNode documentNode2 = this.EntryAt(i);
				if (documentNode2.IsInline && documentNode2.RequiresXamlDir && documentNode2.ClosedParent != null)
				{
					int j;
					for (j = i + 1; j <= num2; j++)
					{
						DocumentNode documentNode3 = this.EntryAt(j);
						if (!documentNode3.IsInline || documentNode3.Type == DocumentNodeType.dnHyperlink || documentNode3.FormatState.DirChar != documentNode2.FormatState.DirChar || documentNode3.ClosedParent != documentNode2.ClosedParent)
						{
							break;
						}
					}
					int num3 = j - i;
					if (num3 > 1)
					{
						DocumentNode documentNode4 = new DocumentNode(DocumentNodeType.dnInline);
						documentNode4.FormatState = new FormatState(documentNode2.Parent.FormatState);
						documentNode4.FormatState.DirChar = documentNode2.FormatState.DirChar;
						this.InsertChildAt(documentNode2.ClosedParent, documentNode4, i, num3);
						num2++;
					}
				}
				else if (documentNode2.Type == DocumentNodeType.dnListItem)
				{
					this.PreCoalesceListItem(documentNode2);
				}
				else if (documentNode2.Type == DocumentNodeType.dnList)
				{
					this.PreCoalesceList(documentNode2);
				}
				else if (documentNode2.Type == DocumentNodeType.dnTable)
				{
					documentNodeArray.Add(documentNode2);
					num2 += this.PreCoalesceTable(documentNode2);
				}
				else if (documentNode2.Type == DocumentNodeType.dnRow)
				{
					this.PreCoalesceRow(documentNode2, ref flag);
				}
			}
			if (flag)
			{
				this.ProcessTableRowSpan(documentNodeArray);
			}
		}

		// Token: 0x06003492 RID: 13458 RVA: 0x000E9C54 File Offset: 0x000E7E54
		internal void CoalesceChildren(ConverterState converterState, int nStart)
		{
			if (nStart >= this.Count || nStart < 0)
			{
				return;
			}
			this.PreCoalesceChildren(converterState, nStart, false);
			DocumentNode documentNode = this.EntryAt(nStart);
			int num = documentNode.ChildCount;
			if (nStart + num >= this.Count)
			{
				num = this.Count - nStart - 1;
			}
			int num2 = nStart + num;
			for (int i = num2; i >= nStart; i--)
			{
				DocumentNode documentNode2 = this.EntryAt(i);
				if (documentNode2.ChildCount == 0)
				{
					documentNode2.Terminate(converterState);
				}
				else
				{
					documentNode2.AppendXamlPrefix(converterState);
					StringBuilder stringBuilder = new StringBuilder(documentNode2.Xaml);
					int childCount = documentNode2.ChildCount;
					int num3 = i + childCount;
					for (int j = i + 1; j <= num3; j++)
					{
						DocumentNode documentNode3 = this.EntryAt(j);
						stringBuilder.Append(documentNode3.Xaml);
					}
					documentNode2.Xaml = stringBuilder.ToString();
					documentNode2.AppendXamlPostfix(converterState);
					documentNode2.IsTerminated = true;
					this.Excise(i + 1, childCount);
					num2 -= childCount;
					this.AssertTreeInvariants();
				}
				if (documentNode2.ColSpan == 0)
				{
					documentNode2.Xaml = string.Empty;
				}
			}
		}

		// Token: 0x06003493 RID: 13459 RVA: 0x000E9D6C File Offset: 0x000E7F6C
		internal void CoalesceOnlyChildren(ConverterState converterState, int nStart)
		{
			if (nStart >= this.Count || nStart < 0)
			{
				return;
			}
			this.PreCoalesceChildren(converterState, nStart, true);
			DocumentNode documentNode = this.EntryAt(nStart);
			int num = documentNode.ChildCount;
			if (nStart + num >= this.Count)
			{
				num = this.Count - nStart - 1;
			}
			int num2 = nStart + num;
			for (int i = num2; i >= nStart; i--)
			{
				DocumentNode documentNode2 = this.EntryAt(i);
				if (documentNode2.ChildCount == 0 && i != nStart)
				{
					documentNode2.Terminate(converterState);
				}
				else if (documentNode2.ChildCount > 0)
				{
					if (i != nStart)
					{
						documentNode2.AppendXamlPrefix(converterState);
					}
					StringBuilder stringBuilder = new StringBuilder(documentNode2.Xaml);
					int childCount = documentNode2.ChildCount;
					int num3 = i + childCount;
					for (int j = i + 1; j <= num3; j++)
					{
						DocumentNode documentNode3 = this.EntryAt(j);
						stringBuilder.Append(documentNode3.Xaml);
					}
					documentNode2.Xaml = stringBuilder.ToString();
					if (i != nStart)
					{
						documentNode2.AppendXamlPostfix(converterState);
						documentNode2.IsTerminated = true;
					}
					this.Excise(i + 1, childCount);
					num2 -= childCount;
				}
			}
		}

		// Token: 0x06003494 RID: 13460 RVA: 0x000E9E84 File Offset: 0x000E8084
		internal void CoalesceAll(ConverterState converterState)
		{
			for (int i = 0; i < this.Count; i++)
			{
				this.CoalesceChildren(converterState, i);
			}
		}

		// Token: 0x06003495 RID: 13461 RVA: 0x000E9EAC File Offset: 0x000E80AC
		internal void CloseAtHelper(int index, int nChildCount)
		{
			if (index >= this.Count || index < 0 || index + nChildCount >= this.Count)
			{
				return;
			}
			DocumentNode documentNode = this.EntryAt(index);
			if (!documentNode.IsPending)
			{
				return;
			}
			documentNode.IsPending = false;
			documentNode.ChildCount = nChildCount;
			int i = index + 1;
			int num = index + documentNode.ChildCount;
			while (i <= num)
			{
				DocumentNode documentNode2 = this.EntryAt(i);
				documentNode2.Parent = documentNode;
				i += documentNode2.ChildCount + 1;
			}
		}

		// Token: 0x06003496 RID: 13462 RVA: 0x000E9F20 File Offset: 0x000E8120
		internal void CloseAt(int index)
		{
			if (index >= this.Count || index < 0)
			{
				return;
			}
			DocumentNode documentNode = this.EntryAt(index);
			if (!documentNode.IsPending)
			{
				return;
			}
			this.AssertTreeInvariants();
			this.AssertTreeSemanticInvariants();
			for (int i = this.Count - 1; i > index; i--)
			{
				DocumentNode documentNode2 = this.EntryAt(i);
				if (documentNode2.IsPending)
				{
					this.CloseAt(i);
				}
			}
			this.CloseAtHelper(index, this.Count - index - 1);
			this.AssertTreeInvariants();
			this.AssertTreeSemanticInvariants();
		}

		// Token: 0x06003497 RID: 13463 RVA: 0x000E9FA0 File Offset: 0x000E81A0
		internal void AssertTreeInvariants()
		{
			if (Invariant.Strict)
			{
				for (int i = 0; i < this.Count; i++)
				{
					DocumentNode documentNode = this.EntryAt(i);
					for (int j = i + 1; j <= documentNode.LastChildIndex; j++)
					{
					}
					for (DocumentNode parent = documentNode.Parent; parent != null; parent = parent.Parent)
					{
					}
				}
			}
		}

		// Token: 0x06003498 RID: 13464 RVA: 0x000E9FF4 File Offset: 0x000E81F4
		internal void AssertTreeSemanticInvariants()
		{
			if (Invariant.Strict)
			{
				int i = 0;
				while (i < this.Count)
				{
					DocumentNode documentNode = this.EntryAt(i);
					DocumentNode parent = documentNode.Parent;
					switch (documentNode.Type)
					{
					default:
						i++;
						break;
					}
				}
			}
		}

		// Token: 0x06003499 RID: 13465 RVA: 0x000EA04C File Offset: 0x000E824C
		internal void CloseAll()
		{
			for (int i = 0; i < this.Count; i++)
			{
				if (this.EntryAt(i).IsPending)
				{
					this.CloseAt(i);
					return;
				}
			}
		}

		// Token: 0x0600349A RID: 13466 RVA: 0x000EA080 File Offset: 0x000E8280
		internal int CountOpenNodes(DocumentNodeType documentNodeType)
		{
			int num = 0;
			if (this._dnaOpen != null)
			{
				this._dnaOpen.CullOpen();
				for (int i = this._dnaOpen.Count - 1; i >= 0; i--)
				{
					DocumentNode documentNode = this._dnaOpen.EntryAt(i);
					if (documentNode.IsPending)
					{
						if (documentNode.Type == documentNodeType)
						{
							num++;
						}
						else if (documentNode.Type == DocumentNodeType.dnShape)
						{
							break;
						}
					}
				}
			}
			return num;
		}

		// Token: 0x0600349B RID: 13467 RVA: 0x000EA0E9 File Offset: 0x000E82E9
		internal int CountOpenCells()
		{
			return this.CountOpenNodes(DocumentNodeType.dnCell);
		}

		// Token: 0x0600349C RID: 13468 RVA: 0x000EA0F4 File Offset: 0x000E82F4
		internal DocumentNode GetOpenParentWhileParsing(DocumentNode dn)
		{
			if (this._dnaOpen != null)
			{
				this._dnaOpen.CullOpen();
				for (int i = this._dnaOpen.Count - 1; i >= 0; i--)
				{
					DocumentNode documentNode = this._dnaOpen.EntryAt(i);
					if (documentNode.IsPending && documentNode.Index < dn.Index)
					{
						return documentNode;
					}
				}
			}
			return null;
		}

		// Token: 0x0600349D RID: 13469 RVA: 0x000EA154 File Offset: 0x000E8354
		internal DocumentNodeType GetTableScope()
		{
			if (this._dnaOpen != null)
			{
				this._dnaOpen.CullOpen();
				for (int i = this._dnaOpen.Count - 1; i >= 0; i--)
				{
					DocumentNode documentNode = this._dnaOpen.EntryAt(i);
					if (documentNode.IsPending)
					{
						if (documentNode.Type == DocumentNodeType.dnTable || documentNode.Type == DocumentNodeType.dnTableBody || documentNode.Type == DocumentNodeType.dnRow || documentNode.Type == DocumentNodeType.dnCell)
						{
							return documentNode.Type;
						}
						if (documentNode.Type == DocumentNodeType.dnShape)
						{
							return DocumentNodeType.dnParagraph;
						}
					}
				}
			}
			return DocumentNodeType.dnParagraph;
		}

		// Token: 0x0600349E RID: 13470 RVA: 0x000EA1E0 File Offset: 0x000E83E0
		internal MarkerList GetOpenMarkerStyles()
		{
			MarkerList markerList = new MarkerList();
			if (this._dnaOpen != null)
			{
				this._dnaOpen.CullOpen();
				int num = 0;
				for (int i = 0; i < this._dnaOpen.Count; i++)
				{
					DocumentNode documentNode = this._dnaOpen.EntryAt(i);
					if (documentNode.IsPending && documentNode.Type == DocumentNodeType.dnShape)
					{
						num = i + 1;
					}
				}
				for (int j = num; j < this._dnaOpen.Count; j++)
				{
					DocumentNode documentNode2 = this._dnaOpen.EntryAt(j);
					if (documentNode2.IsPending && documentNode2.Type == DocumentNodeType.dnList)
					{
						markerList.AddEntry(documentNode2.FormatState.Marker, documentNode2.FormatState.ILS, documentNode2.FormatState.StartIndex, documentNode2.FormatState.StartIndexDefault, documentNode2.VirtualListLevel);
					}
				}
			}
			return markerList;
		}

		// Token: 0x0600349F RID: 13471 RVA: 0x000EA2C0 File Offset: 0x000E84C0
		internal MarkerList GetLastMarkerStyles(MarkerList mlHave, MarkerList mlWant)
		{
			MarkerList markerList = new MarkerList();
			if (mlHave.Count > 0 || mlWant.Count == 0)
			{
				return markerList;
			}
			bool flag = true;
			for (int i = this.Count - 1; i >= 0; i--)
			{
				DocumentNode documentNode = this.EntryAt(i);
				if (documentNode.Type == DocumentNodeType.dnCell || documentNode.Type == DocumentNodeType.dnTable)
				{
					break;
				}
				if (documentNode.Type == DocumentNodeType.dnListItem)
				{
					DocumentNode parentOfType = documentNode.GetParentOfType(DocumentNodeType.dnCell);
					if (parentOfType != null && !parentOfType.IsPending)
					{
						break;
					}
					DocumentNode parentOfType2 = documentNode.GetParentOfType(DocumentNodeType.dnShape);
					if (parentOfType2 == null || parentOfType2.IsPending)
					{
						for (DocumentNode parent = documentNode.Parent; parent != null; parent = parent.Parent)
						{
							if (parent.Type == DocumentNodeType.dnList)
							{
								MarkerListEntry markerListEntry = new MarkerListEntry();
								markerListEntry.Marker = parent.FormatState.Marker;
								markerListEntry.StartIndexOverride = parent.FormatState.StartIndex;
								markerListEntry.StartIndexDefault = parent.FormatState.StartIndexDefault;
								markerListEntry.VirtualListLevel = parent.VirtualListLevel;
								markerListEntry.ILS = parent.FormatState.ILS;
								markerList.Insert(0, markerListEntry);
								if (markerListEntry.Marker != MarkerStyle.MarkerBullet)
								{
									flag = false;
								}
							}
						}
						break;
					}
				}
			}
			if (markerList.Count == 1 && flag)
			{
				markerList.RemoveRange(0, 1);
			}
			return markerList;
		}

		// Token: 0x060034A0 RID: 13472 RVA: 0x000EA41C File Offset: 0x000E861C
		internal void OpenLastList()
		{
			for (int i = this.Count - 1; i >= 0; i--)
			{
				DocumentNode documentNode = this.EntryAt(i);
				if (documentNode.Type == DocumentNodeType.dnListItem)
				{
					DocumentNode parentOfType = documentNode.GetParentOfType(DocumentNodeType.dnShape);
					if (parentOfType == null || parentOfType.IsPending)
					{
						for (DocumentNode documentNode2 = documentNode; documentNode2 != null; documentNode2 = documentNode2.Parent)
						{
							if (documentNode2.Type == DocumentNodeType.dnList || documentNode2.Type == DocumentNodeType.dnListItem)
							{
								documentNode2.IsPending = true;
								this._dnaOpen.InsertOpenNode(documentNode2);
							}
						}
						return;
					}
				}
			}
		}

		// Token: 0x060034A1 RID: 13473 RVA: 0x000EA498 File Offset: 0x000E8698
		internal void OpenLastCell()
		{
			for (int i = this._dnaOpen.Count - 1; i >= 0; i--)
			{
				DocumentNode documentNode = this._dnaOpen.EntryAt(i);
				if (documentNode.IsPending)
				{
					if (documentNode.Type == DocumentNodeType.dnCell)
					{
						return;
					}
					if (documentNode.Type == DocumentNodeType.dnTable || documentNode.Type == DocumentNodeType.dnTableBody || documentNode.Type == DocumentNodeType.dnRow)
					{
						for (int j = this.Count - 1; j >= 0; j--)
						{
							DocumentNode documentNode2 = this.EntryAt(j);
							if (documentNode2 == documentNode)
							{
								return;
							}
							if (documentNode2.Type == DocumentNodeType.dnCell && documentNode2.GetParentOfType(documentNode.Type) == documentNode)
							{
								DocumentNode documentNode3 = documentNode2;
								while (documentNode3 != null && documentNode3 != documentNode)
								{
									documentNode3.IsPending = true;
									this._dnaOpen.InsertOpenNode(documentNode3);
									documentNode3 = documentNode3.Parent;
								}
								return;
							}
						}
					}
				}
			}
		}

		// Token: 0x060034A2 RID: 13474 RVA: 0x000EA56C File Offset: 0x000E876C
		internal int FindPendingFrom(DocumentNodeType documentNodeType, int nStart, int nLow)
		{
			if (this._dnaOpen != null)
			{
				this._dnaOpen.CullOpen();
				for (int i = this._dnaOpen.Count - 1; i >= 0; i--)
				{
					DocumentNode documentNode = this._dnaOpen.EntryAt(i);
					if (documentNode.Index <= nStart)
					{
						if (documentNode.Index <= nLow)
						{
							break;
						}
						if (documentNode.IsPending)
						{
							if (documentNode.Type == documentNodeType)
							{
								return documentNode.Index;
							}
							if (documentNode.Type == DocumentNodeType.dnShape)
							{
								break;
							}
						}
					}
				}
			}
			return -1;
		}

		// Token: 0x060034A3 RID: 13475 RVA: 0x000EA5E6 File Offset: 0x000E87E6
		internal int FindPending(DocumentNodeType documentNodeType, int nLow)
		{
			return this.FindPendingFrom(documentNodeType, this.Count - 1, nLow);
		}

		// Token: 0x060034A4 RID: 13476 RVA: 0x000EA5F8 File Offset: 0x000E87F8
		internal int FindPending(DocumentNodeType documentNodeType)
		{
			return this.FindPending(documentNodeType, -1);
		}

		// Token: 0x060034A5 RID: 13477 RVA: 0x000EA604 File Offset: 0x000E8804
		internal int FindUnmatched(DocumentNodeType dnType)
		{
			if (this._dnaOpen != null)
			{
				for (int i = this._dnaOpen.Count - 1; i >= 0; i--)
				{
					DocumentNode documentNode = this._dnaOpen.EntryAt(i);
					if (documentNode.Type == dnType && !documentNode.IsMatched)
					{
						return documentNode.Index;
					}
				}
			}
			return -1;
		}

		// Token: 0x060034A6 RID: 13478 RVA: 0x000EA658 File Offset: 0x000E8858
		internal void EstablishTreeRelationships()
		{
			for (int i = 0; i < this.Count; i++)
			{
				this.EntryAt(i).Index = i;
			}
			for (int i = 1; i < this.Count; i++)
			{
				DocumentNode documentNode = this.EntryAt(i);
				DocumentNode documentNode2 = this.EntryAt(i - 1);
				if (documentNode2.ChildCount == 0)
				{
					documentNode2 = documentNode2.Parent;
					while (documentNode2 != null && !documentNode2.IsAncestorOf(documentNode))
					{
						documentNode2 = documentNode2.Parent;
					}
				}
				documentNode.Parent = documentNode2;
			}
		}

		// Token: 0x060034A7 RID: 13479 RVA: 0x000EA6D4 File Offset: 0x000E88D4
		internal void CullOpen()
		{
			int i;
			for (i = this.Count - 1; i >= 0; i--)
			{
				DocumentNode documentNode = this.EntryAt(i);
				if (documentNode.Index >= 0 && documentNode.IsTrackedAsOpen)
				{
					break;
				}
			}
			int num = this.Count - (i + 1);
			if (num > 0)
			{
				this.RemoveRange(i + 1, num);
			}
		}

		// Token: 0x060034A8 RID: 13480 RVA: 0x000EA728 File Offset: 0x000E8928
		internal void InsertOpenNode(DocumentNode dn)
		{
			this.CullOpen();
			int num = this.Count;
			while (num > 0 && dn.Index <= this.EntryAt(num - 1).Index)
			{
				num--;
			}
			this.Insert(num, dn);
		}

		// Token: 0x060034A9 RID: 13481 RVA: 0x000EA76C File Offset: 0x000E896C
		internal void InsertNode(int nAt, DocumentNode dn)
		{
			this.Insert(nAt, dn);
			if (this._fMain)
			{
				dn.Index = nAt;
				dn.DNA = this;
				for (nAt++; nAt < this.Count; nAt++)
				{
					this.EntryAt(nAt).Index = nAt;
				}
				if (dn.IsTrackedAsOpen)
				{
					if (this._dnaOpen == null)
					{
						this._dnaOpen = new DocumentNodeArray();
					}
					this._dnaOpen.InsertOpenNode(dn);
				}
			}
		}

		// Token: 0x060034AA RID: 13482 RVA: 0x000EA7E0 File Offset: 0x000E89E0
		internal void InsertChildAt(DocumentNode dnParent, DocumentNode dnNew, int nInsertAt, int nChild)
		{
			this.InsertNode(nInsertAt, dnNew);
			this.CloseAtHelper(nInsertAt, nChild);
			if (dnParent != null && dnParent.Parent == dnNew)
			{
				Invariant.Assert(false, "Parent's Parent node shouldn't be the child node!");
			}
			dnNew.Parent = dnParent;
			while (dnParent != null)
			{
				dnParent.ChildCount++;
				dnParent = dnParent.ClosedParent;
			}
			this.AssertTreeInvariants();
		}

		// Token: 0x060034AB RID: 13483 RVA: 0x000EA840 File Offset: 0x000E8A40
		internal void Excise(int nAt, int nExcise)
		{
			DocumentNode documentNode = this.EntryAt(nAt);
			if (this._fMain)
			{
				int num = nAt + nExcise;
				for (int i = nAt; i < num; i++)
				{
					DocumentNode documentNode2 = this.EntryAt(i);
					documentNode2.Index = -1;
					documentNode2.DNA = null;
				}
			}
			this.RemoveRange(nAt, nExcise);
			if (this._fMain)
			{
				for (DocumentNode parent = documentNode.Parent; parent != null; parent = parent.Parent)
				{
					if (!parent.IsPending)
					{
						parent.ChildCount -= nExcise;
					}
				}
				while (nAt < this.Count)
				{
					this.EntryAt(nAt).Index = nAt;
					nAt++;
				}
				this.AssertTreeInvariants();
			}
		}

		// Token: 0x17000D90 RID: 3472
		// (get) Token: 0x060034AC RID: 13484 RVA: 0x000EA8E6 File Offset: 0x000E8AE6
		internal DocumentNode Top
		{
			get
			{
				if (this.Count <= 0)
				{
					return null;
				}
				return this.EntryAt(this.Count - 1);
			}
		}

		// Token: 0x17000D91 RID: 3473
		// (set) Token: 0x060034AD RID: 13485 RVA: 0x000EA901 File Offset: 0x000E8B01
		internal bool IsMain
		{
			set
			{
				this._fMain = value;
			}
		}

		// Token: 0x060034AE RID: 13486 RVA: 0x000EA90C File Offset: 0x000E8B0C
		private void PreCoalesceListItem(DocumentNode dn)
		{
			int index = dn.Index;
			long num = -1L;
			int num2 = index + dn.ChildCount;
			for (int i = index + 1; i <= num2; i++)
			{
				DocumentNode documentNode = this.EntryAt(i);
				if (documentNode.Type == DocumentNodeType.dnParagraph)
				{
					if (num == -1L)
					{
						num = documentNode.NearMargin;
					}
					else if (documentNode.NearMargin < num && documentNode.IsNonEmpty)
					{
						num = documentNode.NearMargin;
					}
				}
			}
			dn.NearMargin = num;
			for (int j = index; j <= num2; j++)
			{
				DocumentNode documentNode2 = this.EntryAt(j);
				if (documentNode2.Type == DocumentNodeType.dnParagraph)
				{
					documentNode2.NearMargin -= num;
				}
			}
		}

		// Token: 0x060034AF RID: 13487 RVA: 0x000EA9B4 File Offset: 0x000E8BB4
		private void PreCoalesceList(DocumentNode dn)
		{
			int index = dn.Index;
			bool flag = false;
			DirState dirState = DirState.DirDefault;
			int num = index + dn.ChildCount;
			int num2 = index + 1;
			while (!flag && num2 <= num)
			{
				DocumentNode documentNode = this.EntryAt(num2);
				if (documentNode.Type == DocumentNodeType.dnParagraph && documentNode.IsNonEmpty)
				{
					if (dirState == DirState.DirDefault)
					{
						dirState = documentNode.FormatState.DirPara;
					}
					else if (dirState != documentNode.FormatState.DirPara)
					{
						flag = true;
					}
				}
				num2++;
			}
			if (!flag && dirState != DirState.DirDefault)
			{
				for (int i = index; i <= num; i++)
				{
					DocumentNode documentNode2 = this.EntryAt(i);
					if (documentNode2.Type == DocumentNodeType.dnList || documentNode2.Type == DocumentNodeType.dnListItem)
					{
						documentNode2.FormatState.DirPara = dirState;
					}
				}
			}
		}

		// Token: 0x060034B0 RID: 13488 RVA: 0x000EAA70 File Offset: 0x000E8C70
		private int PreCoalesceTable(DocumentNode dn)
		{
			int result = 0;
			int index = dn.Index;
			ColumnStateArray columnStateArray = dn.ComputeColumns();
			int minUnfilledRowIndex = columnStateArray.GetMinUnfilledRowIndex();
			if (minUnfilledRowIndex > 0)
			{
				DocumentNode documentNode = new DocumentNode(DocumentNodeType.dnTable);
				DocumentNode documentNode2 = new DocumentNode(DocumentNodeType.dnTableBody);
				documentNode.FormatState = new FormatState(dn.FormatState);
				documentNode.FormatState.RowFormat = this.EntryAt(minUnfilledRowIndex).FormatState.RowFormat;
				int num = minUnfilledRowIndex - dn.Index - 1;
				int num2 = dn.ChildCount - num;
				dn.ChildCount = num;
				this.EntryAt(index + 1).ChildCount = num - 1;
				this.InsertNode(minUnfilledRowIndex, documentNode2);
				this.CloseAtHelper(minUnfilledRowIndex, num2);
				this.InsertNode(minUnfilledRowIndex, documentNode);
				this.CloseAtHelper(minUnfilledRowIndex, num2 + 1);
				documentNode2.Parent = documentNode;
				documentNode.Parent = dn.ClosedParent;
				for (DocumentNode closedParent = documentNode.ClosedParent; closedParent != null; closedParent = closedParent.ClosedParent)
				{
					closedParent.ChildCount += 2;
				}
				result = 2;
				dn.ColumnStateArray = dn.ComputeColumns();
			}
			else
			{
				dn.ColumnStateArray = columnStateArray;
			}
			return result;
		}

		// Token: 0x060034B1 RID: 13489 RVA: 0x000EAB8C File Offset: 0x000E8D8C
		private void PreCoalesceRow(DocumentNode dn, ref bool fVMerged)
		{
			DocumentNodeArray rowsCells = dn.GetRowsCells();
			RowFormat rowFormat = dn.FormatState.RowFormat;
			DocumentNode parentOfType = dn.GetParentOfType(DocumentNodeType.dnTable);
			ColumnStateArray columnStateArray = (parentOfType != null) ? parentOfType.ColumnStateArray : null;
			int num = (rowsCells.Count < rowFormat.CellCount) ? rowsCells.Count : rowFormat.CellCount;
			int i = 0;
			int j = 0;
			while (j < num)
			{
				DocumentNode documentNode = rowsCells.EntryAt(j);
				CellFormat cellFormat = rowFormat.NthCellFormat(j);
				long cellX = cellFormat.CellX;
				if (cellFormat.IsVMerge)
				{
					fVMerged = true;
				}
				if (cellFormat.IsHMergeFirst)
				{
					for (j++; j < num; j++)
					{
						cellFormat = rowFormat.NthCellFormat(j);
						if (cellFormat.IsVMerge)
						{
							fVMerged = true;
						}
						if (cellFormat.IsHMerge)
						{
							rowsCells.EntryAt(j).ColSpan = 0;
						}
					}
				}
				else
				{
					j++;
				}
				if (columnStateArray != null)
				{
					int num2 = i;
					while (i < columnStateArray.Count)
					{
						ColumnState columnState = columnStateArray.EntryAt(i);
						i++;
						if (columnState.CellX == cellX || columnState.CellX > cellX)
						{
							break;
						}
					}
					if (i - num2 > documentNode.ColSpan)
					{
						documentNode.ColSpan = i - num2;
					}
				}
			}
		}

		// Token: 0x060034B2 RID: 13490 RVA: 0x000EACC4 File Offset: 0x000E8EC4
		private void ProcessTableRowSpan(DocumentNodeArray dnaTables)
		{
			for (int i = 0; i < dnaTables.Count; i++)
			{
				DocumentNode documentNode = dnaTables.EntryAt(i);
				ColumnStateArray columnStateArray = documentNode.ColumnStateArray;
				if (columnStateArray != null && columnStateArray.Count != 0)
				{
					int count = columnStateArray.Count;
					DocumentNodeArray tableRows = documentNode.GetTableRows();
					DocumentNodeArray documentNodeArray = new DocumentNodeArray();
					for (int j = 0; j < count; j++)
					{
						documentNodeArray.Add(null);
					}
					for (int k = 0; k < tableRows.Count; k++)
					{
						DocumentNode documentNode2 = tableRows.EntryAt(k);
						RowFormat rowFormat = documentNode2.FormatState.RowFormat;
						DocumentNodeArray rowsCells = documentNode2.GetRowsCells();
						int num = count;
						if (rowFormat.CellCount < num)
						{
							num = rowFormat.CellCount;
						}
						if (rowsCells.Count < num)
						{
							num = rowsCells.Count;
						}
						int num2 = 0;
						int num3 = 0;
						while (num3 < num && num2 < documentNodeArray.Count)
						{
							DocumentNode documentNode3 = rowsCells.EntryAt(num3);
							CellFormat cellFormat = rowFormat.NthCellFormat(num3);
							if (cellFormat.IsVMerge)
							{
								DocumentNode documentNode4 = documentNodeArray.EntryAt(num2);
								if (documentNode4 != null)
								{
									documentNode4.RowSpan++;
								}
								num2 += documentNode3.ColSpan;
								documentNode3.ColSpan = 0;
							}
							else
							{
								if (cellFormat.IsVMergeFirst)
								{
									documentNode3.RowSpan = 1;
									documentNodeArray[num2] = documentNode3;
								}
								else
								{
									documentNodeArray[num2] = null;
								}
								for (int l = num2 + 1; l < num2 + documentNode3.ColSpan; l++)
								{
									documentNodeArray[l] = null;
								}
								num2 += documentNode3.ColSpan;
							}
							num3++;
						}
					}
				}
			}
		}

		// Token: 0x040024DD RID: 9437
		private bool _fMain;

		// Token: 0x040024DE RID: 9438
		private DocumentNodeArray _dnaOpen;
	}
}
