using System;

namespace System.Windows
{
	/// <summary>Provides data about a change in value to a dependency property as reported by particular routed events, including the previous and current value of the property that changed. </summary>
	/// <typeparam name="T">The type of the dependency property that has changed.</typeparam>
	// Token: 0x020000F1 RID: 241
	public class RoutedPropertyChangedEventArgs<T> : RoutedEventArgs
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.RoutedPropertyChangedEventArgs`1" /> class, with provided old and new values.</summary>
		/// <param name="oldValue">Previous value of the property, prior to the event being raised.</param>
		/// <param name="newValue">Current value of the property at the time of the event.</param>
		// Token: 0x0600088A RID: 2186 RVA: 0x0001BCE0 File Offset: 0x00019EE0
		public RoutedPropertyChangedEventArgs(T oldValue, T newValue)
		{
			this._oldValue = oldValue;
			this._newValue = newValue;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.RoutedPropertyChangedEventArgs`1" /> class, with provided old and new values, and an event identifier.</summary>
		/// <param name="oldValue">Previous value of the property, prior to the event being raised.</param>
		/// <param name="newValue">Current value of the property at the time of the event.</param>
		/// <param name="routedEvent">Identifier of the routed event that this arguments class carries information for.</param>
		// Token: 0x0600088B RID: 2187 RVA: 0x0001BCF6 File Offset: 0x00019EF6
		public RoutedPropertyChangedEventArgs(T oldValue, T newValue, RoutedEvent routedEvent) : this(oldValue, newValue)
		{
			base.RoutedEvent = routedEvent;
		}

		/// <summary>Gets the previous value of the property as reported by a property changed event. </summary>
		/// <returns>The generic value. In a practical implementation of the <see cref="T:System.Windows.RoutedPropertyChangedEventArgs`1" />, the generic type of this property is replaced with the constrained type of the implementation.</returns>
		// Token: 0x170001C6 RID: 454
		// (get) Token: 0x0600088C RID: 2188 RVA: 0x0001BD07 File Offset: 0x00019F07
		public T OldValue
		{
			get
			{
				return this._oldValue;
			}
		}

		/// <summary>Gets the new value of a property as reported by a property changed event. </summary>
		/// <returns>The generic value. In a practical implementation of the <see cref="T:System.Windows.RoutedPropertyChangedEventArgs`1" />, the generic type of this property is replaced with the constrained type of the implementation.</returns>
		// Token: 0x170001C7 RID: 455
		// (get) Token: 0x0600088D RID: 2189 RVA: 0x0001BD0F File Offset: 0x00019F0F
		public T NewValue
		{
			get
			{
				return this._newValue;
			}
		}

		/// <summary>Invokes event handlers in a type-specific way, which can increase event system efficiency.</summary>
		/// <param name="genericHandler">The generic handler to call in a type-specific way.</param>
		/// <param name="genericTarget">The target to call the handler on.</param>
		// Token: 0x0600088E RID: 2190 RVA: 0x0001BD18 File Offset: 0x00019F18
		protected override void InvokeEventHandler(Delegate genericHandler, object genericTarget)
		{
			RoutedPropertyChangedEventHandler<T> routedPropertyChangedEventHandler = (RoutedPropertyChangedEventHandler<T>)genericHandler;
			routedPropertyChangedEventHandler(genericTarget, this);
		}

		// Token: 0x040007A7 RID: 1959
		private T _oldValue;

		// Token: 0x040007A8 RID: 1960
		private T _newValue;
	}
}
