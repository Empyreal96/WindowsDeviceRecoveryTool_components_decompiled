using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows;

namespace MS.Internal.Documents
{
	// Token: 0x020006FD RID: 1789
	internal class UndoManager
	{
		// Token: 0x06007316 RID: 29462 RVA: 0x00210308 File Offset: 0x0020E508
		internal UndoManager()
		{
			this._scope = null;
			this._state = UndoState.Normal;
			this._isEnabled = false;
			this._undoStack = new List<IUndoUnit>(4);
			this._redoStack = new Stack(2);
			this._topUndoIndex = -1;
			this._bottomUndoIndex = 0;
			this._undoLimit = 100;
		}

		// Token: 0x06007317 RID: 29463 RVA: 0x00210360 File Offset: 0x0020E560
		internal static void AttachUndoManager(DependencyObject scope, UndoManager undoManager)
		{
			if (scope == null)
			{
				throw new ArgumentNullException("scope");
			}
			if (undoManager == null)
			{
				throw new ArgumentNullException("undoManager");
			}
			if (undoManager != null && undoManager._scope != null)
			{
				throw new InvalidOperationException(SR.Get("UndoManagerAlreadyAttached"));
			}
			UndoManager.DetachUndoManager(scope);
			scope.SetValue(UndoManager.UndoManagerInstanceProperty, undoManager);
			if (undoManager != null)
			{
				undoManager._scope = scope;
			}
			undoManager.IsEnabled = true;
		}

		// Token: 0x06007318 RID: 29464 RVA: 0x002103C8 File Offset: 0x0020E5C8
		internal static void DetachUndoManager(DependencyObject scope)
		{
			if (scope == null)
			{
				throw new ArgumentNullException("scope");
			}
			UndoManager undoManager = scope.ReadLocalValue(UndoManager.UndoManagerInstanceProperty) as UndoManager;
			if (undoManager != null)
			{
				undoManager.IsEnabled = false;
				scope.ClearValue(UndoManager.UndoManagerInstanceProperty);
				if (undoManager != null)
				{
					undoManager._scope = null;
				}
			}
		}

		// Token: 0x06007319 RID: 29465 RVA: 0x00210413 File Offset: 0x0020E613
		internal static UndoManager GetUndoManager(DependencyObject target)
		{
			if (target == null)
			{
				return null;
			}
			return target.GetValue(UndoManager.UndoManagerInstanceProperty) as UndoManager;
		}

		// Token: 0x0600731A RID: 29466 RVA: 0x0021042C File Offset: 0x0020E62C
		internal void Open(IParentUndoUnit unit)
		{
			if (!this.IsEnabled)
			{
				throw new InvalidOperationException(SR.Get("UndoServiceDisabled"));
			}
			if (unit == null)
			{
				throw new ArgumentNullException("unit");
			}
			IParentUndoUnit deepestOpenUnit = this.DeepestOpenUnit;
			if (deepestOpenUnit == unit)
			{
				throw new InvalidOperationException(SR.Get("UndoUnitCantBeOpenedTwice"));
			}
			if (deepestOpenUnit == null)
			{
				if (unit != this.LastUnit)
				{
					this.Add(unit);
					this.SetLastUnit(unit);
				}
				this.SetOpenedUnit(unit);
				unit.Container = this;
				return;
			}
			unit.Container = deepestOpenUnit;
			deepestOpenUnit.Open(unit);
		}

		// Token: 0x0600731B RID: 29467 RVA: 0x002104B4 File Offset: 0x0020E6B4
		internal void Reopen(IParentUndoUnit unit)
		{
			if (!this.IsEnabled)
			{
				throw new InvalidOperationException(SR.Get("UndoServiceDisabled"));
			}
			if (unit == null)
			{
				throw new ArgumentNullException("unit");
			}
			if (this.OpenedUnit != null)
			{
				throw new InvalidOperationException(SR.Get("UndoUnitAlreadyOpen"));
			}
			switch (this.State)
			{
			case UndoState.Normal:
			case UndoState.Redo:
				if (this.UndoCount == 0 || this.PeekUndoStack() != unit)
				{
					throw new InvalidOperationException(SR.Get("UndoUnitNotOnTopOfStack"));
				}
				break;
			case UndoState.Undo:
				if (this.RedoStack.Count == 0 || (IParentUndoUnit)this.RedoStack.Peek() != unit)
				{
					throw new InvalidOperationException(SR.Get("UndoUnitNotOnTopOfStack"));
				}
				break;
			}
			if (unit.Locked)
			{
				throw new InvalidOperationException(SR.Get("UndoUnitLocked"));
			}
			this.Open(unit);
			this._lastReopenedUnit = unit;
		}

		// Token: 0x0600731C RID: 29468 RVA: 0x00210595 File Offset: 0x0020E795
		internal void Close(UndoCloseAction closeAction)
		{
			this.Close(this.OpenedUnit, closeAction);
		}

		// Token: 0x0600731D RID: 29469 RVA: 0x002105A4 File Offset: 0x0020E7A4
		internal void Close(IParentUndoUnit unit, UndoCloseAction closeAction)
		{
			if (!this.IsEnabled)
			{
				throw new InvalidOperationException(SR.Get("UndoServiceDisabled"));
			}
			if (unit == null)
			{
				throw new ArgumentNullException("unit");
			}
			if (this.OpenedUnit == null)
			{
				throw new InvalidOperationException(SR.Get("UndoNoOpenUnit"));
			}
			if (this.OpenedUnit != unit)
			{
				IParentUndoUnit openedUnit = this.OpenedUnit;
				while (openedUnit.OpenedUnit != null && openedUnit.OpenedUnit != unit)
				{
					openedUnit = openedUnit.OpenedUnit;
				}
				if (openedUnit.OpenedUnit == null)
				{
					throw new ArgumentException(SR.Get("UndoUnitNotFound"), "unit");
				}
				openedUnit.Close(closeAction);
				return;
			}
			else
			{
				if (closeAction != UndoCloseAction.Commit)
				{
					this.SetState(UndoState.Rollback);
					if (unit.OpenedUnit != null)
					{
						unit.Close(closeAction);
					}
					if (closeAction == UndoCloseAction.Rollback)
					{
						unit.Do();
					}
					this.PopUndoStack();
					this.SetOpenedUnit(null);
					this.OnNextDiscard();
					this.SetLastUnit((this._topUndoIndex == -1) ? null : this.PeekUndoStack());
					this.SetState(UndoState.Normal);
					return;
				}
				if (unit.OpenedUnit != null)
				{
					unit.Close(UndoCloseAction.Commit);
				}
				if (this.State != UndoState.Redo && this.State != UndoState.Undo && this.RedoStack.Count > 0)
				{
					this.RedoStack.Clear();
				}
				this.SetOpenedUnit(null);
				return;
			}
		}

		// Token: 0x0600731E RID: 29470 RVA: 0x002106D8 File Offset: 0x0020E8D8
		internal void Add(IUndoUnit unit)
		{
			if (!this.IsEnabled)
			{
				throw new InvalidOperationException(SR.Get("UndoServiceDisabled"));
			}
			if (unit == null)
			{
				throw new ArgumentNullException("unit");
			}
			IParentUndoUnit deepestOpenUnit = this.DeepestOpenUnit;
			if (deepestOpenUnit != null)
			{
				deepestOpenUnit.Add(unit);
				return;
			}
			if (!(unit is IParentUndoUnit))
			{
				throw new InvalidOperationException(SR.Get("UndoNoOpenParentUnit"));
			}
			((IParentUndoUnit)unit).Container = this;
			if (this.LastUnit is IParentUndoUnit)
			{
				((IParentUndoUnit)this.LastUnit).OnNextAdd();
			}
			this.SetLastUnit(unit);
			if (this.State == UndoState.Normal || this.State == UndoState.Redo)
			{
				int num = this._topUndoIndex + 1;
				this._topUndoIndex = num;
				if (num == this.UndoLimit)
				{
					this._topUndoIndex = 0;
				}
				if ((this._topUndoIndex >= this.UndoStack.Count || this.PeekUndoStack() != null) && (this.UndoLimit == -1 || this.UndoStack.Count < this.UndoLimit))
				{
					this.UndoStack.Add(unit);
					return;
				}
				if (this.PeekUndoStack() != null)
				{
					num = this._bottomUndoIndex + 1;
					this._bottomUndoIndex = num;
					if (num == this.UndoLimit)
					{
						this._bottomUndoIndex = 0;
					}
				}
				this.UndoStack[this._topUndoIndex] = unit;
				return;
			}
			else
			{
				if (this.State == UndoState.Undo)
				{
					this.RedoStack.Push(unit);
					return;
				}
				UndoState state = this.State;
				return;
			}
		}

		// Token: 0x0600731F RID: 29471 RVA: 0x0021083A File Offset: 0x0020EA3A
		internal void Clear()
		{
			if (!this.IsEnabled)
			{
				throw new InvalidOperationException(SR.Get("UndoServiceDisabled"));
			}
			if (!this._imeSupportModeEnabled)
			{
				this.DoClear();
			}
		}

		// Token: 0x06007320 RID: 29472 RVA: 0x00210864 File Offset: 0x0020EA64
		internal void Undo(int count)
		{
			if (!this.IsEnabled)
			{
				throw new InvalidOperationException(SR.Get("UndoServiceDisabled"));
			}
			if (count > this.UndoCount || count <= 0)
			{
				throw new ArgumentOutOfRangeException("count");
			}
			if (this.State != UndoState.Normal)
			{
				throw new InvalidOperationException(SR.Get("UndoNotInNormalState"));
			}
			if (this.OpenedUnit != null)
			{
				throw new InvalidOperationException(SR.Get("UndoUnitOpen"));
			}
			Invariant.Assert(this.UndoCount > this._minUndoStackCount);
			this.SetState(UndoState.Undo);
			bool flag = true;
			try
			{
				while (count > 0)
				{
					IUndoUnit undoUnit = this.PopUndoStack();
					undoUnit.Do();
					count--;
				}
				flag = false;
			}
			finally
			{
				if (flag)
				{
					this.Clear();
				}
			}
			this.SetState(UndoState.Normal);
		}

		// Token: 0x06007321 RID: 29473 RVA: 0x0021092C File Offset: 0x0020EB2C
		internal void Redo(int count)
		{
			if (!this.IsEnabled)
			{
				throw new InvalidOperationException(SR.Get("UndoServiceDisabled"));
			}
			if (count > this.RedoStack.Count || count <= 0)
			{
				throw new ArgumentOutOfRangeException("count");
			}
			if (this.State != UndoState.Normal)
			{
				throw new InvalidOperationException(SR.Get("UndoNotInNormalState"));
			}
			if (this.OpenedUnit != null)
			{
				throw new InvalidOperationException(SR.Get("UndoUnitOpen"));
			}
			this.SetState(UndoState.Redo);
			bool flag = true;
			try
			{
				while (count > 0)
				{
					IUndoUnit undoUnit = (IUndoUnit)this.RedoStack.Pop();
					undoUnit.Do();
					count--;
				}
				flag = false;
			}
			finally
			{
				if (flag)
				{
					this.Clear();
				}
			}
			this.SetState(UndoState.Normal);
		}

		// Token: 0x06007322 RID: 29474 RVA: 0x002109F0 File Offset: 0x0020EBF0
		internal virtual void OnNextDiscard()
		{
			if (this.UndoCount > 0)
			{
				IParentUndoUnit parentUndoUnit = (IParentUndoUnit)this.PeekUndoStack();
				parentUndoUnit.OnNextDiscard();
			}
		}

		// Token: 0x06007323 RID: 29475 RVA: 0x00210A18 File Offset: 0x0020EC18
		internal IUndoUnit PeekUndoStack()
		{
			if (this._topUndoIndex < 0 || this._topUndoIndex == this.UndoStack.Count)
			{
				return null;
			}
			return this.UndoStack[this._topUndoIndex];
		}

		// Token: 0x06007324 RID: 29476 RVA: 0x00210A4C File Offset: 0x0020EC4C
		internal Stack SetRedoStack(Stack value)
		{
			Stack redoStack = this._redoStack;
			if (value == null)
			{
				value = new Stack(2);
			}
			this._redoStack = value;
			return redoStack;
		}

		// Token: 0x17001B4B RID: 6987
		// (get) Token: 0x06007325 RID: 29477 RVA: 0x00210A73 File Offset: 0x0020EC73
		// (set) Token: 0x06007326 RID: 29478 RVA: 0x00210A7C File Offset: 0x0020EC7C
		internal bool IsImeSupportModeEnabled
		{
			get
			{
				return this._imeSupportModeEnabled;
			}
			set
			{
				if (value != this._imeSupportModeEnabled)
				{
					if (value)
					{
						if (this._bottomUndoIndex != 0 && this._topUndoIndex >= 0)
						{
							List<IUndoUnit> list = new List<IUndoUnit>(this.UndoCount);
							if (this._bottomUndoIndex > this._topUndoIndex)
							{
								for (int i = this._bottomUndoIndex; i < this.UndoLimit; i++)
								{
									list.Add(this._undoStack[i]);
								}
								this._bottomUndoIndex = 0;
							}
							for (int i = this._bottomUndoIndex; i <= this._topUndoIndex; i++)
							{
								list.Add(this._undoStack[i]);
							}
							this._undoStack = list;
							this._bottomUndoIndex = 0;
							this._topUndoIndex = list.Count - 1;
						}
						this._imeSupportModeEnabled = value;
						return;
					}
					this._imeSupportModeEnabled = value;
					if (!this.IsEnabled)
					{
						this.DoClear();
						return;
					}
					if (this.UndoLimit >= 0 && this._topUndoIndex >= this.UndoLimit)
					{
						List<IUndoUnit> list2 = new List<IUndoUnit>(this.UndoLimit);
						for (int j = this._topUndoIndex + 1 - this.UndoLimit; j <= this._topUndoIndex; j++)
						{
							list2.Add(this._undoStack[j]);
						}
						this._undoStack = list2;
						this._bottomUndoIndex = 0;
						this._topUndoIndex = this.UndoLimit - 1;
					}
				}
			}
		}

		// Token: 0x17001B4C RID: 6988
		// (get) Token: 0x06007327 RID: 29479 RVA: 0x00210BCD File Offset: 0x0020EDCD
		// (set) Token: 0x06007328 RID: 29480 RVA: 0x00210BDF File Offset: 0x0020EDDF
		internal int UndoLimit
		{
			get
			{
				if (!this._imeSupportModeEnabled)
				{
					return this._undoLimit;
				}
				return -1;
			}
			set
			{
				this._undoLimit = value;
				if (!this._imeSupportModeEnabled)
				{
					this.DoClear();
				}
			}
		}

		// Token: 0x17001B4D RID: 6989
		// (get) Token: 0x06007329 RID: 29481 RVA: 0x00210BF6 File Offset: 0x0020EDF6
		internal UndoState State
		{
			get
			{
				return this._state;
			}
		}

		// Token: 0x17001B4E RID: 6990
		// (get) Token: 0x0600732A RID: 29482 RVA: 0x00210BFE File Offset: 0x0020EDFE
		// (set) Token: 0x0600732B RID: 29483 RVA: 0x00210C1D File Offset: 0x0020EE1D
		internal bool IsEnabled
		{
			get
			{
				return this._imeSupportModeEnabled || (this._isEnabled && this._undoLimit != 0);
			}
			set
			{
				this._isEnabled = value;
			}
		}

		// Token: 0x17001B4F RID: 6991
		// (get) Token: 0x0600732C RID: 29484 RVA: 0x00210C26 File Offset: 0x0020EE26
		internal IParentUndoUnit OpenedUnit
		{
			get
			{
				return this._openedUnit;
			}
		}

		// Token: 0x17001B50 RID: 6992
		// (get) Token: 0x0600732D RID: 29485 RVA: 0x00210C2E File Offset: 0x0020EE2E
		internal IUndoUnit LastUnit
		{
			get
			{
				return this._lastUnit;
			}
		}

		// Token: 0x17001B51 RID: 6993
		// (get) Token: 0x0600732E RID: 29486 RVA: 0x00210C36 File Offset: 0x0020EE36
		internal IParentUndoUnit LastReopenedUnit
		{
			get
			{
				return this._lastReopenedUnit;
			}
		}

		// Token: 0x17001B52 RID: 6994
		// (get) Token: 0x0600732F RID: 29487 RVA: 0x00210C40 File Offset: 0x0020EE40
		internal int UndoCount
		{
			get
			{
				int result;
				if (this.UndoStack.Count == 0 || this._topUndoIndex < 0)
				{
					result = 0;
				}
				else if (this._topUndoIndex == this._bottomUndoIndex - 1 && this.PeekUndoStack() == null)
				{
					result = 0;
				}
				else if (this._topUndoIndex >= this._bottomUndoIndex)
				{
					result = this._topUndoIndex - this._bottomUndoIndex + 1;
				}
				else
				{
					result = this._topUndoIndex + (this.UndoLimit - this._bottomUndoIndex) + 1;
				}
				return result;
			}
		}

		// Token: 0x17001B53 RID: 6995
		// (get) Token: 0x06007330 RID: 29488 RVA: 0x00210CBB File Offset: 0x0020EEBB
		internal int RedoCount
		{
			get
			{
				return this.RedoStack.Count;
			}
		}

		// Token: 0x17001B54 RID: 6996
		// (get) Token: 0x06007331 RID: 29489 RVA: 0x00210CC8 File Offset: 0x0020EEC8
		internal static int UndoLimitDefaultValue
		{
			get
			{
				return 100;
			}
		}

		// Token: 0x06007332 RID: 29490 RVA: 0x00210CCC File Offset: 0x0020EECC
		internal IUndoUnit GetUndoUnit(int index)
		{
			Invariant.Assert(index < this.UndoCount);
			Invariant.Assert(index >= 0);
			Invariant.Assert(this._bottomUndoIndex == 0);
			Invariant.Assert(this._imeSupportModeEnabled);
			return this._undoStack[index];
		}

		// Token: 0x06007333 RID: 29491 RVA: 0x00210D18 File Offset: 0x0020EF18
		internal void RemoveUndoRange(int index, int count)
		{
			Invariant.Assert(index >= 0);
			Invariant.Assert(count >= 0);
			Invariant.Assert(count + index <= this.UndoCount);
			Invariant.Assert(this._bottomUndoIndex == 0);
			Invariant.Assert(this._imeSupportModeEnabled);
			for (int i = index + count; i <= this._topUndoIndex; i++)
			{
				this._undoStack[i - count] = this._undoStack[i];
			}
			for (int i = this._topUndoIndex - (count - 1); i <= this._topUndoIndex; i++)
			{
				this._undoStack[i] = null;
			}
			this._topUndoIndex -= count;
		}

		// Token: 0x17001B55 RID: 6997
		// (get) Token: 0x06007334 RID: 29492 RVA: 0x00210DCB File Offset: 0x0020EFCB
		// (set) Token: 0x06007335 RID: 29493 RVA: 0x00210DD3 File Offset: 0x0020EFD3
		internal int MinUndoStackCount
		{
			get
			{
				return this._minUndoStackCount;
			}
			set
			{
				this._minUndoStackCount = value;
			}
		}

		// Token: 0x06007336 RID: 29494 RVA: 0x00210DDC File Offset: 0x0020EFDC
		protected void SetState(UndoState value)
		{
			this._state = value;
		}

		// Token: 0x06007337 RID: 29495 RVA: 0x00210DE5 File Offset: 0x0020EFE5
		protected void SetOpenedUnit(IParentUndoUnit value)
		{
			this._openedUnit = value;
		}

		// Token: 0x06007338 RID: 29496 RVA: 0x00210DEE File Offset: 0x0020EFEE
		protected void SetLastUnit(IUndoUnit value)
		{
			this._lastUnit = value;
		}

		// Token: 0x17001B56 RID: 6998
		// (get) Token: 0x06007339 RID: 29497 RVA: 0x00210DF8 File Offset: 0x0020EFF8
		protected IParentUndoUnit DeepestOpenUnit
		{
			get
			{
				IParentUndoUnit openedUnit = this.OpenedUnit;
				if (openedUnit != null)
				{
					while (openedUnit.OpenedUnit != null)
					{
						openedUnit = openedUnit.OpenedUnit;
					}
				}
				return openedUnit;
			}
		}

		// Token: 0x17001B57 RID: 6999
		// (get) Token: 0x0600733A RID: 29498 RVA: 0x00210E21 File Offset: 0x0020F021
		protected List<IUndoUnit> UndoStack
		{
			get
			{
				return this._undoStack;
			}
		}

		// Token: 0x17001B58 RID: 7000
		// (get) Token: 0x0600733B RID: 29499 RVA: 0x00210E29 File Offset: 0x0020F029
		protected Stack RedoStack
		{
			get
			{
				return this._redoStack;
			}
		}

		// Token: 0x0600733C RID: 29500 RVA: 0x00210E34 File Offset: 0x0020F034
		private void DoClear()
		{
			Invariant.Assert(!this._imeSupportModeEnabled);
			if (this.UndoStack.Count > 0)
			{
				this.UndoStack.Clear();
				this.UndoStack.TrimExcess();
			}
			if (this.RedoStack.Count > 0)
			{
				this.RedoStack.Clear();
			}
			this.SetLastUnit(null);
			this.SetOpenedUnit(null);
			this._topUndoIndex = -1;
			this._bottomUndoIndex = 0;
		}

		// Token: 0x0600733D RID: 29501 RVA: 0x00210EA8 File Offset: 0x0020F0A8
		private IUndoUnit PopUndoStack()
		{
			int num = this.UndoCount - 1;
			IUndoUnit result = this.UndoStack[this._topUndoIndex];
			List<IUndoUnit> undoStack = this.UndoStack;
			int topUndoIndex = this._topUndoIndex;
			this._topUndoIndex = topUndoIndex - 1;
			undoStack[topUndoIndex] = null;
			if (this._topUndoIndex < 0 && num > 0)
			{
				Invariant.Assert(this.UndoLimit > 0);
				this._topUndoIndex = this.UndoLimit - 1;
			}
			return result;
		}

		// Token: 0x0400377B RID: 14203
		private static readonly DependencyProperty UndoManagerInstanceProperty = DependencyProperty.RegisterAttached("UndoManagerInstance", typeof(UndoManager), typeof(UndoManager), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.Inherits));

		// Token: 0x0400377C RID: 14204
		private DependencyObject _scope;

		// Token: 0x0400377D RID: 14205
		private IParentUndoUnit _openedUnit;

		// Token: 0x0400377E RID: 14206
		private IUndoUnit _lastUnit;

		// Token: 0x0400377F RID: 14207
		private List<IUndoUnit> _undoStack;

		// Token: 0x04003780 RID: 14208
		private Stack _redoStack;

		// Token: 0x04003781 RID: 14209
		private UndoState _state;

		// Token: 0x04003782 RID: 14210
		private bool _isEnabled;

		// Token: 0x04003783 RID: 14211
		private IParentUndoUnit _lastReopenedUnit;

		// Token: 0x04003784 RID: 14212
		private int _topUndoIndex;

		// Token: 0x04003785 RID: 14213
		private int _bottomUndoIndex;

		// Token: 0x04003786 RID: 14214
		private int _undoLimit;

		// Token: 0x04003787 RID: 14215
		private int _minUndoStackCount;

		// Token: 0x04003788 RID: 14216
		private bool _imeSupportModeEnabled;

		// Token: 0x04003789 RID: 14217
		private const int _undoLimitDefaultValue = 100;
	}
}
