using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace System.Windows.Controls
{
	/// <summary>Provides data for the <see cref="E:System.Windows.Controls.Primitives.TextBoxBase.TextChanged" /> event.</summary>
	// Token: 0x02000541 RID: 1345
	public class TextChangedEventArgs : RoutedEventArgs
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Controls.TextChangedEventArgs" /> class, using the specified event ID, undo action, and text changes. </summary>
		/// <param name="id">The event identifier (ID).</param>
		/// <param name="action">The <see cref="P:System.Windows.Controls.TextChangedEventArgs.UndoAction" /> caused by the text change.</param>
		/// <param name="changes">The changes that occurred during this event. For more information, see <see cref="P:System.Windows.Controls.TextChangedEventArgs.Changes" />.</param>
		// Token: 0x060057ED RID: 22509 RVA: 0x00185C34 File Offset: 0x00183E34
		public TextChangedEventArgs(RoutedEvent id, UndoAction action, ICollection<TextChange> changes)
		{
			if (id == null)
			{
				throw new ArgumentNullException("id");
			}
			if (action < UndoAction.None || action > UndoAction.Create)
			{
				throw new InvalidEnumArgumentException("action", (int)action, typeof(UndoAction));
			}
			base.RoutedEvent = id;
			this._undoAction = action;
			this._changes = changes;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Controls.TextChangedEventArgs" /> class, using the specified event ID and undo action.     </summary>
		/// <param name="id">The event identifier (ID).</param>
		/// <param name="action">The <see cref="P:System.Windows.Controls.TextChangedEventArgs.UndoAction" /> caused by the text change.</param>
		// Token: 0x060057EE RID: 22510 RVA: 0x00185C88 File Offset: 0x00183E88
		public TextChangedEventArgs(RoutedEvent id, UndoAction action) : this(id, action, new ReadOnlyCollection<TextChange>(new List<TextChange>()))
		{
		}

		/// <summary>Gets how the undo stack is caused or affected by this text change </summary>
		/// <returns>The <see cref="T:System.Windows.Controls.UndoAction" /> appropriate for this text change.</returns>
		// Token: 0x1700156B RID: 5483
		// (get) Token: 0x060057EF RID: 22511 RVA: 0x00185C9C File Offset: 0x00183E9C
		public UndoAction UndoAction
		{
			get
			{
				return this._undoAction;
			}
		}

		/// <summary>Gets a collection of objects that contains information about the changes that occurred.</summary>
		/// <returns>A collection of objects that contains information about the changes that occurred.</returns>
		// Token: 0x1700156C RID: 5484
		// (get) Token: 0x060057F0 RID: 22512 RVA: 0x00185CA4 File Offset: 0x00183EA4
		public ICollection<TextChange> Changes
		{
			get
			{
				return this._changes;
			}
		}

		/// <summary>Performs the proper type casting to call the type-safe <see cref="T:System.Windows.Controls.TextChangedEventHandler" />  delegate for the <see cref="E:System.Windows.Controls.Primitives.TextBoxBase.TextChanged" /> event.</summary>
		/// <param name="genericHandler">The handler to invoke.</param>
		/// <param name="genericTarget">The current object along the event's route.</param>
		// Token: 0x060057F1 RID: 22513 RVA: 0x00185CAC File Offset: 0x00183EAC
		protected override void InvokeEventHandler(Delegate genericHandler, object genericTarget)
		{
			TextChangedEventHandler textChangedEventHandler = (TextChangedEventHandler)genericHandler;
			textChangedEventHandler(genericTarget, this);
		}

		// Token: 0x04002E9A RID: 11930
		private UndoAction _undoAction;

		// Token: 0x04002E9B RID: 11931
		private readonly ICollection<TextChange> _changes;
	}
}
