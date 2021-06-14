using System;
using System.Collections;
using System.Security;
using System.Windows.Documents;
using MS.Internal;
using MS.Internal.Documents;

namespace System.Windows.Controls
{
	// Token: 0x02000510 RID: 1296
	internal sealed class PasswordTextContainer : ITextContainer
	{
		// Token: 0x06005369 RID: 21353 RVA: 0x0017302C File Offset: 0x0017122C
		internal PasswordTextContainer(PasswordBox passwordBox)
		{
			this._passwordBox = passwordBox;
			this._password = new SecureString();
		}

		// Token: 0x0600536A RID: 21354 RVA: 0x00173048 File Offset: 0x00171248
		internal void InsertText(ITextPointer position, string textData)
		{
			this.BeginChange();
			try
			{
				int offset = ((PasswordTextPointer)position).Offset;
				for (int i = 0; i < textData.Length; i++)
				{
					this._password.InsertAt(offset + i, textData[i]);
				}
				this.OnPasswordChange(offset, textData.Length);
			}
			finally
			{
				this.EndChange();
			}
		}

		// Token: 0x0600536B RID: 21355 RVA: 0x001730B4 File Offset: 0x001712B4
		internal void DeleteContent(ITextPointer startPosition, ITextPointer endPosition)
		{
			this.BeginChange();
			try
			{
				int offset = ((PasswordTextPointer)startPosition).Offset;
				int offset2 = ((PasswordTextPointer)endPosition).Offset;
				for (int i = 0; i < offset2 - offset; i++)
				{
					this._password.RemoveAt(offset);
				}
				this.OnPasswordChange(offset, offset - offset2);
			}
			finally
			{
				this.EndChange();
			}
		}

		// Token: 0x0600536C RID: 21356 RVA: 0x0017311C File Offset: 0x0017131C
		internal void BeginChange()
		{
			this._changeBlockLevel++;
		}

		// Token: 0x0600536D RID: 21357 RVA: 0x0017312C File Offset: 0x0017132C
		internal void EndChange()
		{
			this.EndChange(false);
		}

		// Token: 0x0600536E RID: 21358 RVA: 0x00173138 File Offset: 0x00171338
		internal void EndChange(bool skipEvents)
		{
			Invariant.Assert(this._changeBlockLevel > 0, "Unmatched EndChange call!");
			this._changeBlockLevel--;
			if (this._changeBlockLevel == 0 && this._changes != null)
			{
				TextContainerChangedEventArgs changes = this._changes;
				this._changes = null;
				if (this.Changed != null && !skipEvents)
				{
					this.Changed(this, changes);
				}
			}
		}

		// Token: 0x0600536F RID: 21359 RVA: 0x0017319C File Offset: 0x0017139C
		void ITextContainer.BeginChange()
		{
			this.BeginChange();
		}

		// Token: 0x06005370 RID: 21360 RVA: 0x000C744F File Offset: 0x000C564F
		void ITextContainer.BeginChangeNoUndo()
		{
			((ITextContainer)this).BeginChange();
		}

		// Token: 0x06005371 RID: 21361 RVA: 0x0017312C File Offset: 0x0017132C
		void ITextContainer.EndChange()
		{
			this.EndChange(false);
		}

		// Token: 0x06005372 RID: 21362 RVA: 0x001731A4 File Offset: 0x001713A4
		void ITextContainer.EndChange(bool skipEvents)
		{
			this.EndChange(skipEvents);
		}

		// Token: 0x06005373 RID: 21363 RVA: 0x001731AD File Offset: 0x001713AD
		ITextPointer ITextContainer.CreatePointerAtOffset(int offset, LogicalDirection direction)
		{
			return new PasswordTextPointer(this, direction, offset);
		}

		// Token: 0x06005374 RID: 21364 RVA: 0x001731B7 File Offset: 0x001713B7
		ITextPointer ITextContainer.CreatePointerAtCharOffset(int charOffset, LogicalDirection direction)
		{
			return ((ITextContainer)this).CreatePointerAtOffset(charOffset, direction);
		}

		// Token: 0x06005375 RID: 21365 RVA: 0x000C74D3 File Offset: 0x000C56D3
		ITextPointer ITextContainer.CreateDynamicTextPointer(StaticTextPointer position, LogicalDirection direction)
		{
			return ((ITextPointer)position.Handle0).CreatePointer(direction);
		}

		// Token: 0x06005376 RID: 21366 RVA: 0x000C74E7 File Offset: 0x000C56E7
		StaticTextPointer ITextContainer.CreateStaticPointerAtOffset(int offset)
		{
			return new StaticTextPointer(this, ((ITextContainer)this).CreatePointerAtOffset(offset, LogicalDirection.Forward));
		}

		// Token: 0x06005377 RID: 21367 RVA: 0x000C74F7 File Offset: 0x000C56F7
		TextPointerContext ITextContainer.GetPointerContext(StaticTextPointer pointer, LogicalDirection direction)
		{
			return ((ITextPointer)pointer.Handle0).GetPointerContext(direction);
		}

		// Token: 0x06005378 RID: 21368 RVA: 0x000C750B File Offset: 0x000C570B
		int ITextContainer.GetOffsetToPosition(StaticTextPointer position1, StaticTextPointer position2)
		{
			return ((ITextPointer)position1.Handle0).GetOffsetToPosition((ITextPointer)position2.Handle0);
		}

		// Token: 0x06005379 RID: 21369 RVA: 0x000C752A File Offset: 0x000C572A
		int ITextContainer.GetTextInRun(StaticTextPointer position, LogicalDirection direction, char[] textBuffer, int startIndex, int count)
		{
			return ((ITextPointer)position.Handle0).GetTextInRun(direction, textBuffer, startIndex, count);
		}

		// Token: 0x0600537A RID: 21370 RVA: 0x000C7543 File Offset: 0x000C5743
		object ITextContainer.GetAdjacentElement(StaticTextPointer position, LogicalDirection direction)
		{
			return ((ITextPointer)position.Handle0).GetAdjacentElement(direction);
		}

		// Token: 0x0600537B RID: 21371 RVA: 0x0000C238 File Offset: 0x0000A438
		DependencyObject ITextContainer.GetParent(StaticTextPointer position)
		{
			return null;
		}

		// Token: 0x0600537C RID: 21372 RVA: 0x000C7557 File Offset: 0x000C5757
		StaticTextPointer ITextContainer.CreatePointer(StaticTextPointer position, int offset)
		{
			return new StaticTextPointer(this, ((ITextPointer)position.Handle0).CreatePointer(offset));
		}

		// Token: 0x0600537D RID: 21373 RVA: 0x000C7571 File Offset: 0x000C5771
		StaticTextPointer ITextContainer.GetNextContextPosition(StaticTextPointer position, LogicalDirection direction)
		{
			return new StaticTextPointer(this, ((ITextPointer)position.Handle0).GetNextContextPosition(direction));
		}

		// Token: 0x0600537E RID: 21374 RVA: 0x000C758B File Offset: 0x000C578B
		int ITextContainer.CompareTo(StaticTextPointer position1, StaticTextPointer position2)
		{
			return ((ITextPointer)position1.Handle0).CompareTo((ITextPointer)position2.Handle0);
		}

		// Token: 0x0600537F RID: 21375 RVA: 0x000C75AA File Offset: 0x000C57AA
		int ITextContainer.CompareTo(StaticTextPointer position1, ITextPointer position2)
		{
			return ((ITextPointer)position1.Handle0).CompareTo(position2);
		}

		// Token: 0x06005380 RID: 21376 RVA: 0x000C75BE File Offset: 0x000C57BE
		object ITextContainer.GetValue(StaticTextPointer position, DependencyProperty formattingProperty)
		{
			return ((ITextPointer)position.Handle0).GetValue(formattingProperty);
		}

		// Token: 0x06005381 RID: 21377 RVA: 0x001731C4 File Offset: 0x001713C4
		internal void AddPosition(PasswordTextPointer position)
		{
			this.RemoveUnreferencedPositions();
			if (this._positionList == null)
			{
				this._positionList = new ArrayList();
			}
			int index = this.FindIndex(position.Offset, position.LogicalDirection);
			this._positionList.Insert(index, new WeakReference(position));
			this.DebugAssertPositionList();
		}

		// Token: 0x06005382 RID: 21378 RVA: 0x00173218 File Offset: 0x00171418
		internal void RemovePosition(PasswordTextPointer searchPosition)
		{
			Invariant.Assert(this._positionList != null);
			int i;
			for (i = 0; i < this._positionList.Count; i++)
			{
				PasswordTextPointer pointerAtIndex = this.GetPointerAtIndex(i);
				if (pointerAtIndex == searchPosition)
				{
					this._positionList.RemoveAt(i);
					i = -1;
					break;
				}
			}
			Invariant.Assert(i == -1, "Couldn't find position to remove!");
		}

		// Token: 0x17001441 RID: 5185
		// (get) Token: 0x06005383 RID: 21379 RVA: 0x0000B02A File Offset: 0x0000922A
		bool ITextContainer.IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17001442 RID: 5186
		// (get) Token: 0x06005384 RID: 21380 RVA: 0x00173273 File Offset: 0x00171473
		ITextPointer ITextContainer.Start
		{
			get
			{
				return this.Start;
			}
		}

		// Token: 0x17001443 RID: 5187
		// (get) Token: 0x06005385 RID: 21381 RVA: 0x0017327B File Offset: 0x0017147B
		ITextPointer ITextContainer.End
		{
			get
			{
				return this.End;
			}
		}

		// Token: 0x17001444 RID: 5188
		// (get) Token: 0x06005386 RID: 21382 RVA: 0x0000B02A File Offset: 0x0000922A
		uint ITextContainer.Generation
		{
			get
			{
				return 0U;
			}
		}

		// Token: 0x17001445 RID: 5189
		// (get) Token: 0x06005387 RID: 21383 RVA: 0x00173283 File Offset: 0x00171483
		Highlights ITextContainer.Highlights
		{
			get
			{
				if (this._highlights == null)
				{
					this._highlights = new Highlights(this);
				}
				return this._highlights;
			}
		}

		// Token: 0x17001446 RID: 5190
		// (get) Token: 0x06005388 RID: 21384 RVA: 0x0017329F File Offset: 0x0017149F
		DependencyObject ITextContainer.Parent
		{
			get
			{
				return this._passwordBox;
			}
		}

		// Token: 0x17001447 RID: 5191
		// (get) Token: 0x06005389 RID: 21385 RVA: 0x001732A7 File Offset: 0x001714A7
		// (set) Token: 0x0600538A RID: 21386 RVA: 0x001732AF File Offset: 0x001714AF
		ITextSelection ITextContainer.TextSelection
		{
			get
			{
				return this._textSelection;
			}
			set
			{
				this._textSelection = value;
			}
		}

		// Token: 0x17001448 RID: 5192
		// (get) Token: 0x0600538B RID: 21387 RVA: 0x0000C238 File Offset: 0x0000A438
		UndoManager ITextContainer.UndoManager
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17001449 RID: 5193
		// (get) Token: 0x0600538C RID: 21388 RVA: 0x001732B8 File Offset: 0x001714B8
		// (set) Token: 0x0600538D RID: 21389 RVA: 0x001732C0 File Offset: 0x001714C0
		ITextView ITextContainer.TextView
		{
			get
			{
				return this.TextView;
			}
			set
			{
				this.TextView = value;
			}
		}

		// Token: 0x1700144A RID: 5194
		// (get) Token: 0x0600538E RID: 21390 RVA: 0x001732C9 File Offset: 0x001714C9
		// (set) Token: 0x0600538F RID: 21391 RVA: 0x001732D1 File Offset: 0x001714D1
		internal ITextView TextView
		{
			get
			{
				return this._textview;
			}
			set
			{
				this._textview = value;
			}
		}

		// Token: 0x1700144B RID: 5195
		// (get) Token: 0x06005390 RID: 21392 RVA: 0x001732DA File Offset: 0x001714DA
		int ITextContainer.SymbolCount
		{
			get
			{
				return this.SymbolCount;
			}
		}

		// Token: 0x1700144C RID: 5196
		// (get) Token: 0x06005391 RID: 21393 RVA: 0x001732DA File Offset: 0x001714DA
		int ITextContainer.IMECharCount
		{
			get
			{
				return this.SymbolCount;
			}
		}

		// Token: 0x1700144D RID: 5197
		// (get) Token: 0x06005392 RID: 21394 RVA: 0x001732E2 File Offset: 0x001714E2
		internal ITextPointer Start
		{
			get
			{
				return new PasswordTextPointer(this, LogicalDirection.Backward, 0);
			}
		}

		// Token: 0x1700144E RID: 5198
		// (get) Token: 0x06005393 RID: 21395 RVA: 0x001732EC File Offset: 0x001714EC
		internal ITextPointer End
		{
			get
			{
				return new PasswordTextPointer(this, LogicalDirection.Forward, this.SymbolCount);
			}
		}

		// Token: 0x06005394 RID: 21396 RVA: 0x001732FB File Offset: 0x001714FB
		internal SecureString GetPasswordCopy()
		{
			return this._password.Copy();
		}

		// Token: 0x06005395 RID: 21397 RVA: 0x00173308 File Offset: 0x00171508
		internal void SetPassword(SecureString value)
		{
			int symbolCount = this.SymbolCount;
			this._password.Clear();
			this.OnPasswordChange(0, -symbolCount);
			this._password = ((value == null) ? new SecureString() : value.Copy());
			this.OnPasswordChange(0, this.SymbolCount);
		}

		// Token: 0x1700144F RID: 5199
		// (get) Token: 0x06005396 RID: 21398 RVA: 0x00173353 File Offset: 0x00171553
		internal int SymbolCount
		{
			get
			{
				return this._password.Length;
			}
		}

		// Token: 0x17001450 RID: 5200
		// (get) Token: 0x06005397 RID: 21399 RVA: 0x00173360 File Offset: 0x00171560
		internal char PasswordChar
		{
			get
			{
				return this.PasswordBox.PasswordChar;
			}
		}

		// Token: 0x17001451 RID: 5201
		// (get) Token: 0x06005398 RID: 21400 RVA: 0x0017329F File Offset: 0x0017149F
		internal PasswordBox PasswordBox
		{
			get
			{
				return this._passwordBox;
			}
		}

		// Token: 0x14000104 RID: 260
		// (add) Token: 0x06005399 RID: 21401 RVA: 0x0017336D File Offset: 0x0017156D
		// (remove) Token: 0x0600539A RID: 21402 RVA: 0x00173386 File Offset: 0x00171586
		event EventHandler Changing;

		// Token: 0x14000105 RID: 261
		// (add) Token: 0x0600539B RID: 21403 RVA: 0x0017339F File Offset: 0x0017159F
		// (remove) Token: 0x0600539C RID: 21404 RVA: 0x001733B8 File Offset: 0x001715B8
		event TextContainerChangeEventHandler Change;

		// Token: 0x14000106 RID: 262
		// (add) Token: 0x0600539D RID: 21405 RVA: 0x001733D1 File Offset: 0x001715D1
		// (remove) Token: 0x0600539E RID: 21406 RVA: 0x001733EA File Offset: 0x001715EA
		event TextContainerChangedEventHandler Changed;

		// Token: 0x0600539F RID: 21407 RVA: 0x00173404 File Offset: 0x00171604
		private void AddChange(ITextPointer startPosition, int symbolCount, PrecursorTextChangeType precursorTextChange)
		{
			Invariant.Assert(this._changeBlockLevel > 0, "All public APIs must call BeginChange!");
			Invariant.Assert(!this._isReadOnly, "Illegal to modify PasswordTextContainer inside Change event scope!");
			if (this.Changing != null)
			{
				this.Changing(this, EventArgs.Empty);
			}
			if (this._changes == null)
			{
				this._changes = new TextContainerChangedEventArgs();
			}
			this._changes.AddChange(precursorTextChange, startPosition.Offset, symbolCount, false);
			if (this.Change != null)
			{
				Invariant.Assert(precursorTextChange == PrecursorTextChangeType.ContentAdded || precursorTextChange == PrecursorTextChangeType.ContentRemoved);
				TextChangeType textChange = (precursorTextChange == PrecursorTextChangeType.ContentAdded) ? TextChangeType.ContentAdded : TextChangeType.ContentRemoved;
				this._isReadOnly = true;
				try
				{
					this.Change(this, new TextContainerChangeEventArgs(startPosition, symbolCount, symbolCount, textChange));
				}
				finally
				{
					this._isReadOnly = false;
				}
			}
		}

		// Token: 0x060053A0 RID: 21408 RVA: 0x001734CC File Offset: 0x001716CC
		private void OnPasswordChange(int offset, int delta)
		{
			if (delta != 0)
			{
				this.UpdatePositionList(offset, delta);
				PasswordTextPointer startPosition = new PasswordTextPointer(this, LogicalDirection.Forward, offset);
				int symbolCount;
				PrecursorTextChangeType precursorTextChange;
				if (delta > 0)
				{
					symbolCount = delta;
					precursorTextChange = PrecursorTextChangeType.ContentAdded;
				}
				else
				{
					symbolCount = -delta;
					precursorTextChange = PrecursorTextChangeType.ContentRemoved;
				}
				this.AddChange(startPosition, symbolCount, precursorTextChange);
			}
		}

		// Token: 0x060053A1 RID: 21409 RVA: 0x00173508 File Offset: 0x00171708
		private void UpdatePositionList(int offset, int delta)
		{
			if (this._positionList == null)
			{
				return;
			}
			this.RemoveUnreferencedPositions();
			int i = this.FindIndex(offset, LogicalDirection.Forward);
			if (delta < 0)
			{
				int num = -1;
				while (i < this._positionList.Count)
				{
					PasswordTextPointer pointerAtIndex = this.GetPointerAtIndex(i);
					if (pointerAtIndex != null)
					{
						if (pointerAtIndex.Offset > offset + -delta)
						{
							break;
						}
						pointerAtIndex.Offset = offset;
						if (pointerAtIndex.LogicalDirection == LogicalDirection.Backward)
						{
							if (num >= 0)
							{
								WeakReference value = (WeakReference)this._positionList[num];
								this._positionList[num] = this._positionList[i];
								this._positionList[i] = value;
								num++;
							}
						}
						else if (num == -1)
						{
							num = i;
						}
					}
					i++;
				}
			}
			while (i < this._positionList.Count)
			{
				PasswordTextPointer pointerAtIndex = this.GetPointerAtIndex(i);
				if (pointerAtIndex != null)
				{
					pointerAtIndex.Offset += delta;
				}
				i++;
			}
			this.DebugAssertPositionList();
		}

		// Token: 0x060053A2 RID: 21410 RVA: 0x001735F0 File Offset: 0x001717F0
		private void RemoveUnreferencedPositions()
		{
			if (this._positionList == null)
			{
				return;
			}
			for (int i = this._positionList.Count - 1; i >= 0; i--)
			{
				if (this.GetPointerAtIndex(i) == null)
				{
					this._positionList.RemoveAt(i);
				}
			}
		}

		// Token: 0x060053A3 RID: 21411 RVA: 0x00173638 File Offset: 0x00171838
		private int FindIndex(int offset, LogicalDirection gravity)
		{
			Invariant.Assert(this._positionList != null);
			int i;
			for (i = 0; i < this._positionList.Count; i++)
			{
				PasswordTextPointer pointerAtIndex = this.GetPointerAtIndex(i);
				if (pointerAtIndex != null && ((pointerAtIndex.Offset == offset && (pointerAtIndex.LogicalDirection == gravity || gravity == LogicalDirection.Backward)) || pointerAtIndex.Offset > offset))
				{
					break;
				}
			}
			return i;
		}

		// Token: 0x060053A4 RID: 21412 RVA: 0x00173694 File Offset: 0x00171894
		private void DebugAssertPositionList()
		{
			if (Invariant.Strict)
			{
				int num = -1;
				LogicalDirection logicalDirection = LogicalDirection.Backward;
				for (int i = 0; i < this._positionList.Count; i++)
				{
					PasswordTextPointer pointerAtIndex = this.GetPointerAtIndex(i);
					if (pointerAtIndex != null)
					{
						Invariant.Assert(pointerAtIndex.Offset >= 0 && pointerAtIndex.Offset <= this._password.Length);
						Invariant.Assert(num <= pointerAtIndex.Offset);
						if (i > 0 && pointerAtIndex.LogicalDirection == LogicalDirection.Backward && num == pointerAtIndex.Offset)
						{
							Invariant.Assert(logicalDirection != LogicalDirection.Forward);
						}
						num = pointerAtIndex.Offset;
						logicalDirection = pointerAtIndex.LogicalDirection;
					}
				}
			}
		}

		// Token: 0x060053A5 RID: 21413 RVA: 0x0017373C File Offset: 0x0017193C
		private PasswordTextPointer GetPointerAtIndex(int index)
		{
			Invariant.Assert(this._positionList != null);
			WeakReference weakReference = (WeakReference)this._positionList[index];
			Invariant.Assert(weakReference != null);
			object target = weakReference.Target;
			if (target != null && !(target is PasswordTextPointer))
			{
				Invariant.Assert(false, "Unexpected type: " + target.GetType());
			}
			return (PasswordTextPointer)target;
		}

		// Token: 0x04002CFE RID: 11518
		private readonly PasswordBox _passwordBox;

		// Token: 0x04002CFF RID: 11519
		private SecureString _password;

		// Token: 0x04002D00 RID: 11520
		private ArrayList _positionList;

		// Token: 0x04002D01 RID: 11521
		private Highlights _highlights;

		// Token: 0x04002D02 RID: 11522
		private int _changeBlockLevel;

		// Token: 0x04002D03 RID: 11523
		private TextContainerChangedEventArgs _changes;

		// Token: 0x04002D04 RID: 11524
		private ITextView _textview;

		// Token: 0x04002D05 RID: 11525
		private bool _isReadOnly;

		// Token: 0x04002D06 RID: 11526
		private EventHandler Changing;

		// Token: 0x04002D07 RID: 11527
		private TextContainerChangeEventHandler Change;

		// Token: 0x04002D08 RID: 11528
		private TextContainerChangedEventHandler Changed;

		// Token: 0x04002D09 RID: 11529
		private ITextSelection _textSelection;
	}
}
