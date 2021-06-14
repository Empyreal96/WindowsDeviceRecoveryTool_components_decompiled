using System;

namespace System.Windows.Data
{
	/// <summary>Encapsulates arguments for data transfer events.</summary>
	// Token: 0x020001AD RID: 429
	public class DataTransferEventArgs : RoutedEventArgs
	{
		// Token: 0x06001B59 RID: 7001 RVA: 0x00080999 File Offset: 0x0007EB99
		internal DataTransferEventArgs(DependencyObject targetObject, DependencyProperty dp)
		{
			this._targetObject = targetObject;
			this._dp = dp;
		}

		/// <summary>Gets the binding target object of the binding that raised the event.</summary>
		/// <returns>The target object of the binding that raised the event.</returns>
		// Token: 0x1700065A RID: 1626
		// (get) Token: 0x06001B5A RID: 7002 RVA: 0x000809AF File Offset: 0x0007EBAF
		public DependencyObject TargetObject
		{
			get
			{
				return this._targetObject;
			}
		}

		/// <summary>Gets the specific binding target property that is involved in the data transfer event.</summary>
		/// <returns>The property that changed.</returns>
		// Token: 0x1700065B RID: 1627
		// (get) Token: 0x06001B5B RID: 7003 RVA: 0x000809B7 File Offset: 0x0007EBB7
		public DependencyProperty Property
		{
			get
			{
				return this._dp;
			}
		}

		/// <summary>Invokes the specified handler in a type-specific way on the specified object.</summary>
		/// <param name="genericHandler">The generic handler to call in a type-specific way.</param>
		/// <param name="genericTarget">The object to invoke the handler on.</param>
		// Token: 0x06001B5C RID: 7004 RVA: 0x000809C0 File Offset: 0x0007EBC0
		protected override void InvokeEventHandler(Delegate genericHandler, object genericTarget)
		{
			EventHandler<DataTransferEventArgs> eventHandler = (EventHandler<DataTransferEventArgs>)genericHandler;
			eventHandler(genericTarget, this);
		}

		// Token: 0x040013A1 RID: 5025
		private DependencyObject _targetObject;

		// Token: 0x040013A2 RID: 5026
		private DependencyProperty _dp;
	}
}
