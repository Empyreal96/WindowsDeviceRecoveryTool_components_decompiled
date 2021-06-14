using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Windows.Markup;
using System.Windows.Media.Animation;

namespace System.Windows
{
	/// <summary>Contains mutually exclusive <see cref="T:System.Windows.VisualState" /> objects and <see cref="T:System.Windows.VisualTransition" /> objects that are used to move from one state to another.</summary>
	// Token: 0x02000138 RID: 312
	[ContentProperty("States")]
	[RuntimeNameProperty("Name")]
	public class VisualStateGroup : DependencyObject
	{
		/// <summary>Gets or sets the name of the <see cref="T:System.Windows.VisualStateGroup" />.</summary>
		/// <returns>The name of the <see cref="T:System.Windows.VisualStateGroup" />.</returns>
		// Token: 0x1700041A RID: 1050
		// (get) Token: 0x06000CE1 RID: 3297 RVA: 0x0002FCB9 File Offset: 0x0002DEB9
		// (set) Token: 0x06000CE2 RID: 3298 RVA: 0x0002FCC1 File Offset: 0x0002DEC1
		public string Name { get; set; }

		/// <summary>Gets the collection of mutually exclusive <see cref="T:System.Windows.VisualState" /> objects.</summary>
		/// <returns>The collection of mutually exclusive <see cref="T:System.Windows.VisualState" /> objects.</returns>
		// Token: 0x1700041B RID: 1051
		// (get) Token: 0x06000CE3 RID: 3299 RVA: 0x0002FCCA File Offset: 0x0002DECA
		public IList States
		{
			get
			{
				if (this._states == null)
				{
					this._states = new FreezableCollection<VisualState>();
				}
				return this._states;
			}
		}

		/// <summary>Gets the collection of <see cref="T:System.Windows.VisualTransition" /> objects.</summary>
		/// <returns>The collection of <see cref="T:System.Windows.VisualTransition" /> objects.</returns>
		// Token: 0x1700041C RID: 1052
		// (get) Token: 0x06000CE4 RID: 3300 RVA: 0x0002FCE5 File Offset: 0x0002DEE5
		public IList Transitions
		{
			get
			{
				if (this._transitions == null)
				{
					this._transitions = new FreezableCollection<VisualTransition>();
				}
				return this._transitions;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.VisualState" /> that is currently applied to the control.</summary>
		/// <returns>The <see cref="T:System.Windows.VisualState" /> that is currently applied to the control.</returns>
		// Token: 0x1700041D RID: 1053
		// (get) Token: 0x06000CE5 RID: 3301 RVA: 0x0002FD00 File Offset: 0x0002DF00
		// (set) Token: 0x06000CE6 RID: 3302 RVA: 0x0002FD08 File Offset: 0x0002DF08
		public VisualState CurrentState { get; internal set; }

		// Token: 0x06000CE7 RID: 3303 RVA: 0x0002FD14 File Offset: 0x0002DF14
		internal VisualState GetState(string stateName)
		{
			for (int i = 0; i < this.States.Count; i++)
			{
				VisualState visualState = (VisualState)this.States[i];
				if (visualState.Name == stateName)
				{
					return visualState;
				}
			}
			return null;
		}

		// Token: 0x1700041E RID: 1054
		// (get) Token: 0x06000CE8 RID: 3304 RVA: 0x0002FD5A File Offset: 0x0002DF5A
		internal Collection<Storyboard> CurrentStoryboards
		{
			get
			{
				if (this._currentStoryboards == null)
				{
					this._currentStoryboards = new Collection<Storyboard>();
				}
				return this._currentStoryboards;
			}
		}

		// Token: 0x06000CE9 RID: 3305 RVA: 0x0002FD78 File Offset: 0x0002DF78
		internal void StartNewThenStopOld(FrameworkElement element, params Storyboard[] newStoryboards)
		{
			for (int i = 0; i < this.CurrentStoryboards.Count; i++)
			{
				if (this.CurrentStoryboards[i] != null)
				{
					this.CurrentStoryboards[i].Remove(element);
				}
			}
			this.CurrentStoryboards.Clear();
			for (int j = 0; j < newStoryboards.Length; j++)
			{
				if (newStoryboards[j] != null)
				{
					newStoryboards[j].Begin(element, HandoffBehavior.SnapshotAndReplace, true);
					this.CurrentStoryboards.Add(newStoryboards[j]);
				}
			}
		}

		// Token: 0x06000CEA RID: 3306 RVA: 0x0002FDF2 File Offset: 0x0002DFF2
		internal void RaiseCurrentStateChanging(FrameworkElement stateGroupsRoot, VisualState oldState, VisualState newState, FrameworkElement control)
		{
			if (this.CurrentStateChanging != null)
			{
				this.CurrentStateChanging(stateGroupsRoot, new VisualStateChangedEventArgs(oldState, newState, control, stateGroupsRoot));
			}
		}

		// Token: 0x06000CEB RID: 3307 RVA: 0x0002FE12 File Offset: 0x0002E012
		internal void RaiseCurrentStateChanged(FrameworkElement stateGroupsRoot, VisualState oldState, VisualState newState, FrameworkElement control)
		{
			if (this.CurrentStateChanged != null)
			{
				this.CurrentStateChanged(stateGroupsRoot, new VisualStateChangedEventArgs(oldState, newState, control, stateGroupsRoot));
			}
		}

		/// <summary>Occurs after a control transitions to a different state.</summary>
		// Token: 0x1400002D RID: 45
		// (add) Token: 0x06000CEC RID: 3308 RVA: 0x0002FE34 File Offset: 0x0002E034
		// (remove) Token: 0x06000CED RID: 3309 RVA: 0x0002FE6C File Offset: 0x0002E06C
		public event EventHandler<VisualStateChangedEventArgs> CurrentStateChanged;

		/// <summary>Occurs when a control starts transitioning to a different state.</summary>
		// Token: 0x1400002E RID: 46
		// (add) Token: 0x06000CEE RID: 3310 RVA: 0x0002FEA4 File Offset: 0x0002E0A4
		// (remove) Token: 0x06000CEF RID: 3311 RVA: 0x0002FEDC File Offset: 0x0002E0DC
		public event EventHandler<VisualStateChangedEventArgs> CurrentStateChanging;

		// Token: 0x04000B2E RID: 2862
		private Collection<Storyboard> _currentStoryboards;

		// Token: 0x04000B2F RID: 2863
		private FreezableCollection<VisualState> _states;

		// Token: 0x04000B30 RID: 2864
		private FreezableCollection<VisualTransition> _transitions;
	}
}
