using System;
using System.ComponentModel;
using System.Windows.Markup;

namespace System.Windows
{
	/// <summary>Represents an event setter in a style. Event setters invoke the specified event handlers in response to events.</summary>
	// Token: 0x020000B8 RID: 184
	public class EventSetter : SetterBase
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.EventSetter" /> class. </summary>
		// Token: 0x060003DC RID: 988 RVA: 0x0000B185 File Offset: 0x00009385
		public EventSetter()
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.EventSetter" /> class, using the provided event and handler parameters. </summary>
		/// <param name="routedEvent">The particular routed event that the <see cref="T:System.Windows.EventSetter" /> responds to.</param>
		/// <param name="handler">The handler to assign in this setter.</param>
		// Token: 0x060003DD RID: 989 RVA: 0x0000B18D File Offset: 0x0000938D
		public EventSetter(RoutedEvent routedEvent, Delegate handler)
		{
			if (routedEvent == null)
			{
				throw new ArgumentNullException("routedEvent");
			}
			if (handler == null)
			{
				throw new ArgumentNullException("handler");
			}
			this._event = routedEvent;
			this._handler = handler;
		}

		/// <summary>Gets or sets the particular routed event that this <see cref="T:System.Windows.EventSetter" /> responds to.</summary>
		/// <returns>The identifier field of the routed event.</returns>
		/// <exception cref="T:System.InvalidOperationException">Attempted to set this property on a sealed <see cref="T:System.Windows.EventSetter" /> .</exception>
		// Token: 0x17000094 RID: 148
		// (get) Token: 0x060003DE RID: 990 RVA: 0x0000B1BF File Offset: 0x000093BF
		// (set) Token: 0x060003DF RID: 991 RVA: 0x0000B1C7 File Offset: 0x000093C7
		public RoutedEvent Event
		{
			get
			{
				return this._event;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				base.CheckSealed();
				this._event = value;
			}
		}

		/// <summary>Gets or sets the reference to a handler for a routed event in the setter. </summary>
		/// <returns>Reference to the handler that is attached by this <see cref="T:System.Windows.EventSetter" />.</returns>
		// Token: 0x17000095 RID: 149
		// (get) Token: 0x060003E0 RID: 992 RVA: 0x0000B1E4 File Offset: 0x000093E4
		// (set) Token: 0x060003E1 RID: 993 RVA: 0x0000B1EC File Offset: 0x000093EC
		[TypeConverter(typeof(EventSetterHandlerConverter))]
		public Delegate Handler
		{
			get
			{
				return this._handler;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				base.CheckSealed();
				this._handler = value;
			}
		}

		/// <summary>Gets or sets a value that determines whether the handler assigned to the setter should still be invoked, even if the event is marked handled in its event data. </summary>
		/// <returns>
		///     <see langword="true" /> if the handler should still be invoked; otherwise, <see langword="false" />.</returns>
		// Token: 0x17000096 RID: 150
		// (get) Token: 0x060003E2 RID: 994 RVA: 0x0000B209 File Offset: 0x00009409
		// (set) Token: 0x060003E3 RID: 995 RVA: 0x0000B211 File Offset: 0x00009411
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool HandledEventsToo
		{
			get
			{
				return this._handledEventsToo;
			}
			set
			{
				base.CheckSealed();
				this._handledEventsToo = value;
			}
		}

		// Token: 0x060003E4 RID: 996 RVA: 0x0000B220 File Offset: 0x00009420
		internal override void Seal()
		{
			if (this._event == null)
			{
				throw new ArgumentException(SR.Get("NullPropertyIllegal", new object[]
				{
					"EventSetter.Event"
				}));
			}
			if (this._handler == null)
			{
				throw new ArgumentException(SR.Get("NullPropertyIllegal", new object[]
				{
					"EventSetter.Handler"
				}));
			}
			if (this._handler.GetType() != this._event.HandlerType)
			{
				throw new ArgumentException(SR.Get("HandlerTypeIllegal"));
			}
			base.Seal();
		}

		// Token: 0x04000616 RID: 1558
		private RoutedEvent _event;

		// Token: 0x04000617 RID: 1559
		private Delegate _handler;

		// Token: 0x04000618 RID: 1560
		private bool _handledEventsToo;
	}
}
