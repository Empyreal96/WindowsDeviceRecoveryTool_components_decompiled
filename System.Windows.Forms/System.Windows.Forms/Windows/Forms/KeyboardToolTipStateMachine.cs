using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;

namespace System.Windows.Forms
{
	// Token: 0x020002A1 RID: 673
	internal sealed class KeyboardToolTipStateMachine
	{
		// Token: 0x17000972 RID: 2418
		// (get) Token: 0x06002708 RID: 9992 RVA: 0x000B7746 File Offset: 0x000B5946
		public static KeyboardToolTipStateMachine Instance
		{
			get
			{
				if (KeyboardToolTipStateMachine.instance == null)
				{
					KeyboardToolTipStateMachine.instance = new KeyboardToolTipStateMachine();
				}
				return KeyboardToolTipStateMachine.instance;
			}
		}

		// Token: 0x06002709 RID: 9993 RVA: 0x000B7760 File Offset: 0x000B5960
		private KeyboardToolTipStateMachine()
		{
			Dictionary<KeyboardToolTipStateMachine.SmTransition, Func<IKeyboardToolTip, ToolTip, KeyboardToolTipStateMachine.SmState>> dictionary = new Dictionary<KeyboardToolTipStateMachine.SmTransition, Func<IKeyboardToolTip, ToolTip, KeyboardToolTipStateMachine.SmState>>();
			KeyboardToolTipStateMachine.SmTransition key = new KeyboardToolTipStateMachine.SmTransition(KeyboardToolTipStateMachine.SmState.Hidden, KeyboardToolTipStateMachine.SmEvent.FocusedTool);
			dictionary[key] = new Func<IKeyboardToolTip, ToolTip, KeyboardToolTipStateMachine.SmState>(this.SetupInitShowTimer);
			KeyboardToolTipStateMachine.SmTransition key2 = new KeyboardToolTipStateMachine.SmTransition(KeyboardToolTipStateMachine.SmState.Hidden, KeyboardToolTipStateMachine.SmEvent.LeftTool);
			dictionary[key2] = new Func<IKeyboardToolTip, ToolTip, KeyboardToolTipStateMachine.SmState>(this.DoNothing);
			KeyboardToolTipStateMachine.SmTransition key3 = new KeyboardToolTipStateMachine.SmTransition(KeyboardToolTipStateMachine.SmState.ReadyForInitShow, KeyboardToolTipStateMachine.SmEvent.FocusedTool);
			dictionary[key3] = new Func<IKeyboardToolTip, ToolTip, KeyboardToolTipStateMachine.SmState>(this.DoNothing);
			KeyboardToolTipStateMachine.SmTransition key4 = new KeyboardToolTipStateMachine.SmTransition(KeyboardToolTipStateMachine.SmState.ReadyForInitShow, KeyboardToolTipStateMachine.SmEvent.LeftTool);
			dictionary[key4] = new Func<IKeyboardToolTip, ToolTip, KeyboardToolTipStateMachine.SmState>(this.ResetFsmToHidden);
			KeyboardToolTipStateMachine.SmTransition key5 = new KeyboardToolTipStateMachine.SmTransition(KeyboardToolTipStateMachine.SmState.ReadyForInitShow, KeyboardToolTipStateMachine.SmEvent.InitialDelayTimerExpired);
			dictionary[key5] = new Func<IKeyboardToolTip, ToolTip, KeyboardToolTipStateMachine.SmState>(this.ShowToolTip);
			KeyboardToolTipStateMachine.SmTransition key6 = new KeyboardToolTipStateMachine.SmTransition(KeyboardToolTipStateMachine.SmState.Shown, KeyboardToolTipStateMachine.SmEvent.FocusedTool);
			dictionary[key6] = new Func<IKeyboardToolTip, ToolTip, KeyboardToolTipStateMachine.SmState>(this.DoNothing);
			KeyboardToolTipStateMachine.SmTransition key7 = new KeyboardToolTipStateMachine.SmTransition(KeyboardToolTipStateMachine.SmState.Shown, KeyboardToolTipStateMachine.SmEvent.LeftTool);
			dictionary[key7] = new Func<IKeyboardToolTip, ToolTip, KeyboardToolTipStateMachine.SmState>(this.HideAndStartWaitingForRefocus);
			KeyboardToolTipStateMachine.SmTransition key8 = new KeyboardToolTipStateMachine.SmTransition(KeyboardToolTipStateMachine.SmState.Shown, KeyboardToolTipStateMachine.SmEvent.AutoPopupDelayTimerExpired);
			dictionary[key8] = new Func<IKeyboardToolTip, ToolTip, KeyboardToolTipStateMachine.SmState>(this.ResetFsmToHidden);
			KeyboardToolTipStateMachine.SmTransition key9 = new KeyboardToolTipStateMachine.SmTransition(KeyboardToolTipStateMachine.SmState.WaitForRefocus, KeyboardToolTipStateMachine.SmEvent.FocusedTool);
			dictionary[key9] = new Func<IKeyboardToolTip, ToolTip, KeyboardToolTipStateMachine.SmState>(this.SetupReshowTimer);
			KeyboardToolTipStateMachine.SmTransition key10 = new KeyboardToolTipStateMachine.SmTransition(KeyboardToolTipStateMachine.SmState.WaitForRefocus, KeyboardToolTipStateMachine.SmEvent.LeftTool);
			dictionary[key10] = new Func<IKeyboardToolTip, ToolTip, KeyboardToolTipStateMachine.SmState>(this.DoNothing);
			KeyboardToolTipStateMachine.SmTransition key11 = new KeyboardToolTipStateMachine.SmTransition(KeyboardToolTipStateMachine.SmState.WaitForRefocus, KeyboardToolTipStateMachine.SmEvent.RefocusWaitDelayExpired);
			dictionary[key11] = new Func<IKeyboardToolTip, ToolTip, KeyboardToolTipStateMachine.SmState>(this.ResetFsmToHidden);
			KeyboardToolTipStateMachine.SmTransition key12 = new KeyboardToolTipStateMachine.SmTransition(KeyboardToolTipStateMachine.SmState.ReadyForReshow, KeyboardToolTipStateMachine.SmEvent.FocusedTool);
			dictionary[key12] = new Func<IKeyboardToolTip, ToolTip, KeyboardToolTipStateMachine.SmState>(this.DoNothing);
			KeyboardToolTipStateMachine.SmTransition key13 = new KeyboardToolTipStateMachine.SmTransition(KeyboardToolTipStateMachine.SmState.ReadyForReshow, KeyboardToolTipStateMachine.SmEvent.LeftTool);
			dictionary[key13] = new Func<IKeyboardToolTip, ToolTip, KeyboardToolTipStateMachine.SmState>(this.StartWaitingForRefocus);
			KeyboardToolTipStateMachine.SmTransition key14 = new KeyboardToolTipStateMachine.SmTransition(KeyboardToolTipStateMachine.SmState.ReadyForReshow, KeyboardToolTipStateMachine.SmEvent.ReshowDelayTimerExpired);
			dictionary[key14] = new Func<IKeyboardToolTip, ToolTip, KeyboardToolTipStateMachine.SmState>(this.ShowToolTip);
			this.transitions = dictionary;
		}

		// Token: 0x0600270A RID: 9994 RVA: 0x000B7932 File Offset: 0x000B5B32
		public void ResetStateMachine(ToolTip toolTip)
		{
			this.Reset(toolTip);
		}

		// Token: 0x0600270B RID: 9995 RVA: 0x000B793B File Offset: 0x000B5B3B
		public void Hook(IKeyboardToolTip tool, ToolTip toolTip)
		{
			if (tool.AllowsToolTip())
			{
				this.StartTracking(tool, toolTip);
				tool.OnHooked(toolTip);
			}
		}

		// Token: 0x0600270C RID: 9996 RVA: 0x000B7954 File Offset: 0x000B5B54
		public void NotifyAboutMouseEnter(IKeyboardToolTip sender)
		{
			if (this.IsToolTracked(sender) && sender.ShowsOwnToolTip())
			{
				this.Reset(null);
			}
		}

		// Token: 0x0600270D RID: 9997 RVA: 0x000B796E File Offset: 0x000B5B6E
		private bool IsToolTracked(IKeyboardToolTip sender)
		{
			return this.toolToTip[sender] != null;
		}

		// Token: 0x0600270E RID: 9998 RVA: 0x000B797F File Offset: 0x000B5B7F
		public void NotifyAboutLostFocus(IKeyboardToolTip sender)
		{
			if (this.IsToolTracked(sender) && sender.ShowsOwnToolTip())
			{
				this.Transit(KeyboardToolTipStateMachine.SmEvent.LeftTool, sender);
				if (this.currentTool == null)
				{
					this.lastFocusedTool.SetTarget(null);
				}
			}
		}

		// Token: 0x0600270F RID: 9999 RVA: 0x000B79AE File Offset: 0x000B5BAE
		public void NotifyAboutGotFocus(IKeyboardToolTip sender)
		{
			if (this.IsToolTracked(sender) && sender.ShowsOwnToolTip() && sender.IsBeingTabbedTo())
			{
				this.Transit(KeyboardToolTipStateMachine.SmEvent.FocusedTool, sender);
				if (this.currentTool == sender)
				{
					this.lastFocusedTool.SetTarget(sender);
				}
			}
		}

		// Token: 0x06002710 RID: 10000 RVA: 0x000B79E6 File Offset: 0x000B5BE6
		public void Unhook(IKeyboardToolTip tool, ToolTip toolTip)
		{
			if (tool.AllowsToolTip())
			{
				this.StopTracking(tool, toolTip);
				tool.OnUnhooked(toolTip);
			}
		}

		// Token: 0x06002711 RID: 10001 RVA: 0x000B79FF File Offset: 0x000B5BFF
		public void NotifyAboutFormDeactivation(ToolTip sender)
		{
			this.OnFormDeactivation(sender);
		}

		// Token: 0x17000973 RID: 2419
		// (get) Token: 0x06002712 RID: 10002 RVA: 0x000B7A08 File Offset: 0x000B5C08
		internal IKeyboardToolTip LastFocusedTool
		{
			get
			{
				IKeyboardToolTip result;
				if (this.lastFocusedTool.TryGetTarget(out result))
				{
					return result;
				}
				return Control.FromHandleInternal(UnsafeNativeMethods.GetFocus());
			}
		}

		// Token: 0x06002713 RID: 10003 RVA: 0x000B7A30 File Offset: 0x000B5C30
		private KeyboardToolTipStateMachine.SmState HideAndStartWaitingForRefocus(IKeyboardToolTip tool, ToolTip toolTip)
		{
			toolTip.HideToolTip(this.currentTool);
			return this.StartWaitingForRefocus(tool, toolTip);
		}

		// Token: 0x06002714 RID: 10004 RVA: 0x000B7A48 File Offset: 0x000B5C48
		private KeyboardToolTipStateMachine.SmState StartWaitingForRefocus(IKeyboardToolTip tool, ToolTip toolTip)
		{
			this.ResetTimer();
			this.currentTool = null;
			SendOrPostCallback expirationCallback = null;
			this.refocusDelayExpirationCallback = (expirationCallback = delegate(object toolObject)
			{
				if (this.currentState == KeyboardToolTipStateMachine.SmState.WaitForRefocus && this.refocusDelayExpirationCallback == expirationCallback)
				{
					this.Transit(KeyboardToolTipStateMachine.SmEvent.RefocusWaitDelayExpired, (IKeyboardToolTip)toolObject);
				}
			});
			SynchronizationContext.Current.Post(expirationCallback, tool);
			return KeyboardToolTipStateMachine.SmState.WaitForRefocus;
		}

		// Token: 0x06002715 RID: 10005 RVA: 0x000B7AA4 File Offset: 0x000B5CA4
		private KeyboardToolTipStateMachine.SmState SetupReshowTimer(IKeyboardToolTip tool, ToolTip toolTip)
		{
			this.currentTool = tool;
			this.ResetTimer();
			this.StartTimer(toolTip.GetDelayTime(1), this.GetOneRunTickHandler(delegate(Timer sender)
			{
				this.Transit(KeyboardToolTipStateMachine.SmEvent.ReshowDelayTimerExpired, tool);
			}));
			return KeyboardToolTipStateMachine.SmState.ReadyForReshow;
		}

		// Token: 0x06002716 RID: 10006 RVA: 0x000B7AF8 File Offset: 0x000B5CF8
		private KeyboardToolTipStateMachine.SmState ShowToolTip(IKeyboardToolTip tool, ToolTip toolTip)
		{
			string captionForTool = tool.GetCaptionForTool(toolTip);
			int delayTime = toolTip.GetDelayTime(2);
			if (!this.currentTool.IsHoveredWithMouse())
			{
				toolTip.ShowKeyboardToolTip(captionForTool, this.currentTool, delayTime);
			}
			this.StartTimer(delayTime, this.GetOneRunTickHandler(delegate(Timer sender)
			{
				this.Transit(KeyboardToolTipStateMachine.SmEvent.AutoPopupDelayTimerExpired, this.currentTool);
			}));
			return KeyboardToolTipStateMachine.SmState.Shown;
		}

		// Token: 0x06002717 RID: 10007 RVA: 0x000B7B4A File Offset: 0x000B5D4A
		private KeyboardToolTipStateMachine.SmState ResetFsmToHidden(IKeyboardToolTip tool, ToolTip toolTip)
		{
			return this.FullFsmReset();
		}

		// Token: 0x06002718 RID: 10008 RVA: 0x000B7B52 File Offset: 0x000B5D52
		private KeyboardToolTipStateMachine.SmState DoNothing(IKeyboardToolTip tool, ToolTip toolTip)
		{
			return this.currentState;
		}

		// Token: 0x06002719 RID: 10009 RVA: 0x000B7B5A File Offset: 0x000B5D5A
		private KeyboardToolTipStateMachine.SmState SetupInitShowTimer(IKeyboardToolTip tool, ToolTip toolTip)
		{
			this.currentTool = tool;
			this.ResetTimer();
			this.StartTimer(toolTip.GetDelayTime(3), this.GetOneRunTickHandler(delegate(Timer sender)
			{
				this.Transit(KeyboardToolTipStateMachine.SmEvent.InitialDelayTimerExpired, this.currentTool);
			}));
			return KeyboardToolTipStateMachine.SmState.ReadyForInitShow;
		}

		// Token: 0x0600271A RID: 10010 RVA: 0x000B7B89 File Offset: 0x000B5D89
		private void StartTimer(int interval, EventHandler eventHandler)
		{
			this.timer.Interval = interval;
			this.timer.Tick += eventHandler;
			this.timer.Start();
		}

		// Token: 0x0600271B RID: 10011 RVA: 0x000B7BB0 File Offset: 0x000B5DB0
		private EventHandler GetOneRunTickHandler(Action<Timer> handler)
		{
			EventHandler wrapper = null;
			wrapper = delegate(object sender, EventArgs eventArgs)
			{
				this.timer.Stop();
				this.timer.Tick -= wrapper;
				handler(this.timer);
			};
			return wrapper;
		}

		// Token: 0x0600271C RID: 10012 RVA: 0x000B7BF0 File Offset: 0x000B5DF0
		private void Transit(KeyboardToolTipStateMachine.SmEvent @event, IKeyboardToolTip source)
		{
			bool flag = false;
			try
			{
				ToolTip toolTip = this.toolToTip[source];
				if ((this.currentTool == null || this.currentTool.CanShowToolTipsNow()) && toolTip != null)
				{
					Func<IKeyboardToolTip, ToolTip, KeyboardToolTipStateMachine.SmState> func = this.transitions[new KeyboardToolTipStateMachine.SmTransition(this.currentState, @event)];
					this.currentState = func(source, toolTip);
				}
				else
				{
					flag = true;
				}
			}
			catch
			{
				flag = true;
				throw;
			}
			finally
			{
				if (flag)
				{
					this.FullFsmReset();
				}
			}
		}

		// Token: 0x0600271D RID: 10013 RVA: 0x000B7C7C File Offset: 0x000B5E7C
		private KeyboardToolTipStateMachine.SmState FullFsmReset()
		{
			if (this.currentState == KeyboardToolTipStateMachine.SmState.Shown && this.currentTool != null)
			{
				ToolTip toolTip = this.toolToTip[this.currentTool];
				if (toolTip != null)
				{
					toolTip.HideToolTip(this.currentTool);
				}
			}
			this.ResetTimer();
			this.currentTool = null;
			return this.currentState = KeyboardToolTipStateMachine.SmState.Hidden;
		}

		// Token: 0x0600271E RID: 10014 RVA: 0x000B7CD2 File Offset: 0x000B5ED2
		private void ResetTimer()
		{
			this.timer.ClearTimerTickHandlers();
			this.timer.Stop();
		}

		// Token: 0x0600271F RID: 10015 RVA: 0x000B7CEA File Offset: 0x000B5EEA
		private void Reset(ToolTip toolTipToReset)
		{
			if (toolTipToReset == null || (this.currentTool != null && this.toolToTip[this.currentTool] == toolTipToReset))
			{
				this.FullFsmReset();
			}
		}

		// Token: 0x06002720 RID: 10016 RVA: 0x000B7D12 File Offset: 0x000B5F12
		private void StartTracking(IKeyboardToolTip tool, ToolTip toolTip)
		{
			this.toolToTip[tool] = toolTip;
		}

		// Token: 0x06002721 RID: 10017 RVA: 0x000B7D21 File Offset: 0x000B5F21
		private void StopTracking(IKeyboardToolTip tool, ToolTip toolTip)
		{
			this.toolToTip.Remove(tool, toolTip);
		}

		// Token: 0x06002722 RID: 10018 RVA: 0x000B7D30 File Offset: 0x000B5F30
		private void OnFormDeactivation(ToolTip sender)
		{
			if (this.currentTool != null && this.toolToTip[this.currentTool] == sender)
			{
				this.FullFsmReset();
			}
		}

		// Token: 0x04001078 RID: 4216
		[ThreadStatic]
		private static KeyboardToolTipStateMachine instance;

		// Token: 0x04001079 RID: 4217
		private readonly Dictionary<KeyboardToolTipStateMachine.SmTransition, Func<IKeyboardToolTip, ToolTip, KeyboardToolTipStateMachine.SmState>> transitions;

		// Token: 0x0400107A RID: 4218
		private readonly KeyboardToolTipStateMachine.ToolToTipDictionary toolToTip = new KeyboardToolTipStateMachine.ToolToTipDictionary();

		// Token: 0x0400107B RID: 4219
		private KeyboardToolTipStateMachine.SmState currentState;

		// Token: 0x0400107C RID: 4220
		private IKeyboardToolTip currentTool;

		// Token: 0x0400107D RID: 4221
		private readonly KeyboardToolTipStateMachine.InternalStateMachineTimer timer = new KeyboardToolTipStateMachine.InternalStateMachineTimer();

		// Token: 0x0400107E RID: 4222
		private SendOrPostCallback refocusDelayExpirationCallback;

		// Token: 0x0400107F RID: 4223
		private readonly WeakReference<IKeyboardToolTip> lastFocusedTool = new WeakReference<IKeyboardToolTip>(null);

		// Token: 0x020005F5 RID: 1525
		private enum SmEvent : byte
		{
			// Token: 0x040039CB RID: 14795
			FocusedTool,
			// Token: 0x040039CC RID: 14796
			LeftTool,
			// Token: 0x040039CD RID: 14797
			InitialDelayTimerExpired,
			// Token: 0x040039CE RID: 14798
			ReshowDelayTimerExpired,
			// Token: 0x040039CF RID: 14799
			AutoPopupDelayTimerExpired,
			// Token: 0x040039D0 RID: 14800
			RefocusWaitDelayExpired
		}

		// Token: 0x020005F6 RID: 1526
		private enum SmState : byte
		{
			// Token: 0x040039D2 RID: 14802
			Hidden,
			// Token: 0x040039D3 RID: 14803
			ReadyForInitShow,
			// Token: 0x040039D4 RID: 14804
			Shown,
			// Token: 0x040039D5 RID: 14805
			ReadyForReshow,
			// Token: 0x040039D6 RID: 14806
			WaitForRefocus
		}

		// Token: 0x020005F7 RID: 1527
		private struct SmTransition : IEquatable<KeyboardToolTipStateMachine.SmTransition>
		{
			// Token: 0x06005BD0 RID: 23504 RVA: 0x0017FBBE File Offset: 0x0017DDBE
			public SmTransition(KeyboardToolTipStateMachine.SmState currentState, KeyboardToolTipStateMachine.SmEvent @event)
			{
				this.currentState = currentState;
				this.@event = @event;
			}

			// Token: 0x06005BD1 RID: 23505 RVA: 0x0017FBCE File Offset: 0x0017DDCE
			public bool Equals(KeyboardToolTipStateMachine.SmTransition other)
			{
				return this.currentState == other.currentState && this.@event == other.@event;
			}

			// Token: 0x06005BD2 RID: 23506 RVA: 0x0017FBEE File Offset: 0x0017DDEE
			public override bool Equals(object obj)
			{
				return obj is KeyboardToolTipStateMachine.SmTransition && this.Equals((KeyboardToolTipStateMachine.SmTransition)obj);
			}

			// Token: 0x06005BD3 RID: 23507 RVA: 0x0017FC06 File Offset: 0x0017DE06
			public override int GetHashCode()
			{
				return (int)((int)this.currentState << 16 | (KeyboardToolTipStateMachine.SmState)this.@event);
			}

			// Token: 0x040039D7 RID: 14807
			private readonly KeyboardToolTipStateMachine.SmState currentState;

			// Token: 0x040039D8 RID: 14808
			private readonly KeyboardToolTipStateMachine.SmEvent @event;
		}

		// Token: 0x020005F8 RID: 1528
		private sealed class InternalStateMachineTimer : Timer
		{
			// Token: 0x06005BD4 RID: 23508 RVA: 0x0017FC18 File Offset: 0x0017DE18
			public void ClearTimerTickHandlers()
			{
				this.onTimer = null;
			}
		}

		// Token: 0x020005F9 RID: 1529
		private sealed class ToolToTipDictionary
		{
			// Token: 0x170015FE RID: 5630
			public ToolTip this[IKeyboardToolTip tool]
			{
				get
				{
					ToolTip result = null;
					WeakReference<ToolTip> weakReference;
					if (this.table.TryGetValue(tool, out weakReference) && !weakReference.TryGetTarget(out result))
					{
						this.table.Remove(tool);
					}
					return result;
				}
				set
				{
					WeakReference<ToolTip> weakReference;
					if (this.table.TryGetValue(tool, out weakReference))
					{
						weakReference.SetTarget(value);
						return;
					}
					this.table.Add(tool, new WeakReference<ToolTip>(value));
				}
			}

			// Token: 0x06005BD8 RID: 23512 RVA: 0x0017FC9C File Offset: 0x0017DE9C
			public void Remove(IKeyboardToolTip tool, ToolTip toolTip)
			{
				WeakReference<ToolTip> weakReference;
				if (this.table.TryGetValue(tool, out weakReference))
				{
					ToolTip toolTip2;
					if (weakReference.TryGetTarget(out toolTip2))
					{
						if (toolTip2 == toolTip)
						{
							this.table.Remove(tool);
							return;
						}
					}
					else
					{
						this.table.Remove(tool);
					}
				}
			}

			// Token: 0x040039D9 RID: 14809
			private ConditionalWeakTable<IKeyboardToolTip, WeakReference<ToolTip>> table = new ConditionalWeakTable<IKeyboardToolTip, WeakReference<ToolTip>>();
		}
	}
}
