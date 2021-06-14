using System;
using System.ComponentModel;
using System.Windows.Markup;

namespace System.Windows
{
	/// <summary>Represents a trigger that applies a set of actions in response to an event.</summary>
	// Token: 0x020000B9 RID: 185
	[ContentProperty("Actions")]
	public class EventTrigger : TriggerBase, IAddChild
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.EventTrigger" /> class. </summary>
		// Token: 0x060003E5 RID: 997 RVA: 0x0000B2AC File Offset: 0x000094AC
		public EventTrigger()
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.EventTrigger" /> class with the specified event.</summary>
		/// <param name="routedEvent">The <see cref="T:System.Windows.RoutedEvent" /> that activates this trigger.</param>
		// Token: 0x060003E6 RID: 998 RVA: 0x0000B2B4 File Offset: 0x000094B4
		public EventTrigger(RoutedEvent routedEvent)
		{
			this.RoutedEvent = routedEvent;
		}

		/// <summary>Adds a child object. </summary>
		/// <param name="value">The child object to add.</param>
		// Token: 0x060003E7 RID: 999 RVA: 0x0000B2C3 File Offset: 0x000094C3
		void IAddChild.AddChild(object value)
		{
			this.AddChild(value);
		}

		/// <summary>Adds the specified object to the <see cref="P:System.Windows.EventTrigger.Actions" /> collection of the current event trigger.</summary>
		/// <param name="value">A <see cref="T:System.Windows.TriggerAction" /> object to add to the <see cref="P:System.Windows.EventTrigger.Actions" /> collection of this trigger.</param>
		// Token: 0x060003E8 RID: 1000 RVA: 0x0000B2CC File Offset: 0x000094CC
		protected virtual void AddChild(object value)
		{
			TriggerAction triggerAction = value as TriggerAction;
			if (triggerAction == null)
			{
				throw new ArgumentException(SR.Get("EventTriggerBadAction", new object[]
				{
					value.GetType().Name
				}));
			}
			this.Actions.Add(triggerAction);
		}

		/// <summary>Adds the text content of a node to the object. </summary>
		/// <param name="text">The text to add to the object.</param>
		// Token: 0x060003E9 RID: 1001 RVA: 0x0000B313 File Offset: 0x00009513
		void IAddChild.AddText(string text)
		{
			this.AddText(text);
		}

		/// <summary>This method is not supported and results in an exception.</summary>
		/// <param name="text">This parameter is not used.</param>
		// Token: 0x060003EA RID: 1002 RVA: 0x0000B31C File Offset: 0x0000951C
		protected virtual void AddText(string text)
		{
			XamlSerializerUtil.ThrowIfNonWhiteSpaceInAddText(text, this);
		}

		/// <summary>Gets or sets the <see cref="T:System.Windows.RoutedEvent" /> that will activate this trigger.</summary>
		/// <returns>The default value is <see langword="null" />.</returns>
		/// <exception cref="T:System.ArgumentNullException">The <see cref="P:System.Windows.EventTrigger.RoutedEvent" /> property cannot be null.</exception>
		// Token: 0x17000097 RID: 151
		// (get) Token: 0x060003EB RID: 1003 RVA: 0x0000B325 File Offset: 0x00009525
		// (set) Token: 0x060003EC RID: 1004 RVA: 0x0000B330 File Offset: 0x00009530
		public RoutedEvent RoutedEvent
		{
			get
			{
				return this._routedEvent;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				if (base.IsSealed)
				{
					throw new InvalidOperationException(SR.Get("CannotChangeAfterSealed", new object[]
					{
						"EventTrigger"
					}));
				}
				if (this._routedEventHandler != null)
				{
					throw new InvalidOperationException(SR.Get("CannotChangeAfterSealed", new object[]
					{
						"EventTrigger"
					}));
				}
				this._routedEvent = value;
			}
		}

		/// <summary>Gets or sets the name of the object with the event that activates this trigger. This is only used by element triggers or template triggers.</summary>
		/// <returns>The default value is <see langword="null" />. If this property value is <see langword="null" />, then the element being monitored for the raising of the event is the templated parent or the logical tree root.</returns>
		/// <exception cref="T:System.InvalidOperationException">After an <see cref="T:System.Windows.EventTrigger" /> is in use, it cannot be modified.</exception>
		// Token: 0x17000098 RID: 152
		// (get) Token: 0x060003ED RID: 1005 RVA: 0x0000B39E File Offset: 0x0000959E
		// (set) Token: 0x060003EE RID: 1006 RVA: 0x0000B3A6 File Offset: 0x000095A6
		[DefaultValue(null)]
		public string SourceName
		{
			get
			{
				return this._sourceName;
			}
			set
			{
				if (base.IsSealed)
				{
					throw new InvalidOperationException(SR.Get("CannotChangeAfterSealed", new object[]
					{
						"EventTrigger"
					}));
				}
				this._sourceName = value;
			}
		}

		// Token: 0x17000099 RID: 153
		// (get) Token: 0x060003EF RID: 1007 RVA: 0x0000B3D5 File Offset: 0x000095D5
		// (set) Token: 0x060003F0 RID: 1008 RVA: 0x0000B3DD File Offset: 0x000095DD
		internal int TriggerChildIndex
		{
			get
			{
				return this._childId;
			}
			set
			{
				this._childId = value;
			}
		}

		/// <summary>Gets the collection of actions to apply when the event occurs.</summary>
		/// <returns>The default is an empty collection.</returns>
		// Token: 0x1700009A RID: 154
		// (get) Token: 0x060003F1 RID: 1009 RVA: 0x0000B3E6 File Offset: 0x000095E6
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public TriggerActionCollection Actions
		{
			get
			{
				if (this._actions == null)
				{
					this._actions = new TriggerActionCollection();
					this._actions.Owner = this;
				}
				return this._actions;
			}
		}

		// Token: 0x060003F2 RID: 1010 RVA: 0x0000B410 File Offset: 0x00009610
		internal override void OnInheritanceContextChangedCore(EventArgs args)
		{
			base.OnInheritanceContextChangedCore(args);
			if (this._actions == null)
			{
				return;
			}
			for (int i = 0; i < this._actions.Count; i++)
			{
				DependencyObject dependencyObject = this._actions[i];
				if (dependencyObject != null && dependencyObject.InheritanceContext == this)
				{
					dependencyObject.OnInheritanceContextChanged(args);
				}
			}
		}

		/// <summary>Returns whether serialization processes should serialize the effective value of the <see cref="P:System.Windows.EventTrigger.Actions" /> property on instances of this class.</summary>
		/// <returns>Returns <see langword="true " />if the <see cref="P:System.Windows.EventTrigger.Actions" /> property value should be serialized; otherwise, <see langword="false" />.</returns>
		// Token: 0x060003F3 RID: 1011 RVA: 0x0000B463 File Offset: 0x00009663
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool ShouldSerializeActions()
		{
			return this._actions != null && this._actions.Count > 0;
		}

		// Token: 0x060003F4 RID: 1012 RVA: 0x0000B480 File Offset: 0x00009680
		internal sealed override void Seal()
		{
			if (this.PropertyValues.Count > 0)
			{
				throw new InvalidOperationException(SR.Get("EventTriggerDoNotSetProperties"));
			}
			if (base.HasEnterActions || base.HasExitActions)
			{
				throw new InvalidOperationException(SR.Get("EventTriggerDoesNotEnterExit"));
			}
			if (this._routedEvent != null && this._actions != null && this._actions.Count > 0)
			{
				this._actions.Seal(this);
			}
			base.Seal();
		}

		// Token: 0x060003F5 RID: 1013 RVA: 0x0000B4FC File Offset: 0x000096FC
		internal static void ProcessTriggerCollection(FrameworkElement triggersHost)
		{
			TriggerCollection value = EventTrigger.TriggerCollectionField.GetValue(triggersHost);
			if (value != null)
			{
				for (int i = 0; i < value.Count; i++)
				{
					EventTrigger.ProcessOneTrigger(triggersHost, value[i]);
				}
			}
		}

		// Token: 0x060003F6 RID: 1014 RVA: 0x0000B538 File Offset: 0x00009738
		internal static void ProcessOneTrigger(FrameworkElement triggersHost, TriggerBase triggerBase)
		{
			EventTrigger eventTrigger = triggerBase as EventTrigger;
			if (eventTrigger != null)
			{
				eventTrigger._source = FrameworkElement.FindNamedFrameworkElement(triggersHost, eventTrigger.SourceName);
				EventTrigger.EventTriggerSourceListener @object = new EventTrigger.EventTriggerSourceListener(eventTrigger, triggersHost);
				eventTrigger._routedEventHandler = new RoutedEventHandler(@object.Handler);
				eventTrigger._source.AddHandler(eventTrigger.RoutedEvent, eventTrigger._routedEventHandler, false);
				return;
			}
			throw new InvalidOperationException(SR.Get("TriggersSupportsEventTriggersOnly"));
		}

		// Token: 0x060003F7 RID: 1015 RVA: 0x0000B5A4 File Offset: 0x000097A4
		internal static void DisconnectAllTriggers(FrameworkElement triggersHost)
		{
			TriggerCollection value = EventTrigger.TriggerCollectionField.GetValue(triggersHost);
			if (value != null)
			{
				for (int i = 0; i < value.Count; i++)
				{
					EventTrigger.DisconnectOneTrigger(triggersHost, value[i]);
				}
			}
		}

		// Token: 0x060003F8 RID: 1016 RVA: 0x0000B5E0 File Offset: 0x000097E0
		internal static void DisconnectOneTrigger(FrameworkElement triggersHost, TriggerBase triggerBase)
		{
			EventTrigger eventTrigger = triggerBase as EventTrigger;
			if (eventTrigger != null)
			{
				eventTrigger._source.RemoveHandler(eventTrigger.RoutedEvent, eventTrigger._routedEventHandler);
				eventTrigger._routedEventHandler = null;
				return;
			}
			throw new InvalidOperationException(SR.Get("TriggersSupportsEventTriggersOnly"));
		}

		// Token: 0x04000619 RID: 1561
		private RoutedEvent _routedEvent;

		// Token: 0x0400061A RID: 1562
		private string _sourceName;

		// Token: 0x0400061B RID: 1563
		private int _childId;

		// Token: 0x0400061C RID: 1564
		private TriggerActionCollection _actions;

		// Token: 0x0400061D RID: 1565
		internal static readonly UncommonField<TriggerCollection> TriggerCollectionField = new UncommonField<TriggerCollection>(null);

		// Token: 0x0400061E RID: 1566
		private RoutedEventHandler _routedEventHandler;

		// Token: 0x0400061F RID: 1567
		private FrameworkElement _source;

		// Token: 0x02000814 RID: 2068
		internal class EventTriggerSourceListener
		{
			// Token: 0x06007E3A RID: 32314 RVA: 0x00235684 File Offset: 0x00233884
			internal EventTriggerSourceListener(EventTrigger trigger, FrameworkElement host)
			{
				this._owningTrigger = trigger;
				this._owningTriggerHost = host;
			}

			// Token: 0x06007E3B RID: 32315 RVA: 0x0023569C File Offset: 0x0023389C
			internal void Handler(object sender, RoutedEventArgs e)
			{
				TriggerActionCollection actions = this._owningTrigger.Actions;
				for (int i = 0; i < actions.Count; i++)
				{
					actions[i].Invoke(this._owningTriggerHost);
				}
			}

			// Token: 0x04003BAD RID: 15277
			private EventTrigger _owningTrigger;

			// Token: 0x04003BAE RID: 15278
			private FrameworkElement _owningTriggerHost;
		}
	}
}
