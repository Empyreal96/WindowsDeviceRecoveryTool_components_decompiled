using System;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.InteropServices;

namespace System.Windows.Forms
{
	/// <summary>Implements a timer that raises an event at user-defined intervals. This timer is optimized for use in Windows Forms applications and must be used in a window.</summary>
	// Token: 0x02000397 RID: 919
	[DefaultProperty("Interval")]
	[DefaultEvent("Tick")]
	[ToolboxItemFilter("System.Windows.Forms")]
	[SRDescription("DescriptionTimer")]
	public class Timer : Component
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.Timer" /> class.</summary>
		// Token: 0x06003A16 RID: 14870 RVA: 0x001026B8 File Offset: 0x001008B8
		public Timer()
		{
			this.interval = 100;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.Timer" /> class together with the specified container.</summary>
		/// <param name="container">An <see cref="T:System.ComponentModel.IContainer" /> that represents the container for the timer. </param>
		// Token: 0x06003A17 RID: 14871 RVA: 0x001026D3 File Offset: 0x001008D3
		public Timer(IContainer container) : this()
		{
			if (container == null)
			{
				throw new ArgumentNullException("container");
			}
			container.Add(this);
		}

		/// <summary>Gets or sets an arbitrary string representing some type of user state.</summary>
		/// <returns>An arbitrary string representing some type of user state.</returns>
		// Token: 0x17000E7F RID: 3711
		// (get) Token: 0x06003A18 RID: 14872 RVA: 0x001026F0 File Offset: 0x001008F0
		// (set) Token: 0x06003A19 RID: 14873 RVA: 0x001026F8 File Offset: 0x001008F8
		[SRCategory("CatData")]
		[Localizable(false)]
		[Bindable(true)]
		[SRDescription("ControlTagDescr")]
		[DefaultValue(null)]
		[TypeConverter(typeof(StringConverter))]
		public object Tag
		{
			get
			{
				return this.userData;
			}
			set
			{
				this.userData = value;
			}
		}

		/// <summary>Occurs when the specified timer interval has elapsed and the timer is enabled.</summary>
		// Token: 0x140002D9 RID: 729
		// (add) Token: 0x06003A1A RID: 14874 RVA: 0x00102701 File Offset: 0x00100901
		// (remove) Token: 0x06003A1B RID: 14875 RVA: 0x0010271A File Offset: 0x0010091A
		[SRCategory("CatBehavior")]
		[SRDescription("TimerTimerDescr")]
		public event EventHandler Tick
		{
			add
			{
				this.onTimer = (EventHandler)Delegate.Combine(this.onTimer, value);
			}
			remove
			{
				this.onTimer = (EventHandler)Delegate.Remove(this.onTimer, value);
			}
		}

		/// <summary>Disposes of the resources, other than memory, used by the timer.</summary>
		/// <param name="disposing">
		///       <see langword="true" /> to release both managed and unmanaged resources. <see langword="false" /> to release only the unmanaged resources.</param>
		// Token: 0x06003A1C RID: 14876 RVA: 0x00102733 File Offset: 0x00100933
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (this.timerWindow != null)
				{
					this.timerWindow.StopTimer();
				}
				this.Enabled = false;
			}
			this.timerWindow = null;
			base.Dispose(disposing);
		}

		/// <summary>Gets or sets whether the timer is running.</summary>
		/// <returns>
		///     <see langword="true" /> if the timer is currently enabled; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
		// Token: 0x17000E80 RID: 3712
		// (get) Token: 0x06003A1D RID: 14877 RVA: 0x00102760 File Offset: 0x00100960
		// (set) Token: 0x06003A1E RID: 14878 RVA: 0x0010277C File Offset: 0x0010097C
		[SRCategory("CatBehavior")]
		[DefaultValue(false)]
		[SRDescription("TimerEnabledDescr")]
		public virtual bool Enabled
		{
			get
			{
				if (this.timerWindow == null)
				{
					return this.enabled;
				}
				return this.timerWindow.IsTimerRunning;
			}
			set
			{
				object obj = this.syncObj;
				lock (obj)
				{
					if (this.enabled != value)
					{
						this.enabled = value;
						if (!base.DesignMode)
						{
							if (value)
							{
								if (this.timerWindow == null)
								{
									this.timerWindow = new Timer.TimerNativeWindow(this);
								}
								this.timerRoot = GCHandle.Alloc(this);
								this.timerWindow.StartTimer(this.interval);
							}
							else
							{
								if (this.timerWindow != null)
								{
									this.timerWindow.StopTimer();
								}
								if (this.timerRoot.IsAllocated)
								{
									this.timerRoot.Free();
								}
							}
						}
					}
				}
			}
		}

		/// <summary>Gets or sets the time, in milliseconds, before the <see cref="E:System.Windows.Forms.Timer.Tick" /> event is raised relative to the last occurrence of the <see cref="E:System.Windows.Forms.Timer.Tick" /> event.</summary>
		/// <returns>An <see cref="T:System.Int32" /> specifying the number of milliseconds before the <see cref="E:System.Windows.Forms.Timer.Tick" /> event is raised relative to the last occurrence of the <see cref="E:System.Windows.Forms.Timer.Tick" /> event. The value cannot be less than one.</returns>
		// Token: 0x17000E81 RID: 3713
		// (get) Token: 0x06003A1F RID: 14879 RVA: 0x00102830 File Offset: 0x00100A30
		// (set) Token: 0x06003A20 RID: 14880 RVA: 0x00102838 File Offset: 0x00100A38
		[SRCategory("CatBehavior")]
		[DefaultValue(100)]
		[SRDescription("TimerIntervalDescr")]
		public int Interval
		{
			get
			{
				return this.interval;
			}
			set
			{
				object obj = this.syncObj;
				lock (obj)
				{
					if (value < 1)
					{
						throw new ArgumentOutOfRangeException("Interval", SR.GetString("TimerInvalidInterval", new object[]
						{
							value,
							0.ToString(CultureInfo.CurrentCulture)
						}));
					}
					if (this.interval != value)
					{
						this.interval = value;
						if (this.Enabled && !base.DesignMode && this.timerWindow != null)
						{
							this.timerWindow.RestartTimer(value);
						}
					}
				}
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Timer.Tick" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. This is always <see cref="F:System.EventArgs.Empty" />. </param>
		// Token: 0x06003A21 RID: 14881 RVA: 0x001028E0 File Offset: 0x00100AE0
		protected virtual void OnTick(EventArgs e)
		{
			if (this.onTimer != null)
			{
				this.onTimer(this, e);
			}
		}

		/// <summary>Starts the timer.</summary>
		// Token: 0x06003A22 RID: 14882 RVA: 0x001028F7 File Offset: 0x00100AF7
		public void Start()
		{
			this.Enabled = true;
		}

		/// <summary>Stops the timer.</summary>
		// Token: 0x06003A23 RID: 14883 RVA: 0x00102900 File Offset: 0x00100B00
		public void Stop()
		{
			this.Enabled = false;
		}

		/// <summary>Returns a string that represents the <see cref="T:System.Windows.Forms.Timer" />.</summary>
		/// <returns>A string that represents the current <see cref="T:System.Windows.Forms.Timer" />. </returns>
		// Token: 0x06003A24 RID: 14884 RVA: 0x0010290C File Offset: 0x00100B0C
		public override string ToString()
		{
			string str = base.ToString();
			return str + ", Interval: " + this.Interval.ToString(CultureInfo.CurrentCulture);
		}

		// Token: 0x040022F3 RID: 8947
		private int interval;

		// Token: 0x040022F4 RID: 8948
		private bool enabled;

		// Token: 0x040022F5 RID: 8949
		internal EventHandler onTimer;

		// Token: 0x040022F6 RID: 8950
		private GCHandle timerRoot;

		// Token: 0x040022F7 RID: 8951
		private Timer.TimerNativeWindow timerWindow;

		// Token: 0x040022F8 RID: 8952
		private object userData;

		// Token: 0x040022F9 RID: 8953
		private object syncObj = new object();

		// Token: 0x02000728 RID: 1832
		private class TimerNativeWindow : NativeWindow
		{
			// Token: 0x0600609F RID: 24735 RVA: 0x0018BBD5 File Offset: 0x00189DD5
			internal TimerNativeWindow(Timer owner)
			{
				this._owner = owner;
			}

			// Token: 0x060060A0 RID: 24736 RVA: 0x0018BBE4 File Offset: 0x00189DE4
			~TimerNativeWindow()
			{
				this.StopTimer();
			}

			// Token: 0x17001711 RID: 5905
			// (get) Token: 0x060060A1 RID: 24737 RVA: 0x0018BC10 File Offset: 0x00189E10
			public bool IsTimerRunning
			{
				get
				{
					return this._timerID != 0 && base.Handle != IntPtr.Zero;
				}
			}

			// Token: 0x060060A2 RID: 24738 RVA: 0x0018BC2C File Offset: 0x00189E2C
			private bool EnsureHandle()
			{
				if (base.Handle == IntPtr.Zero)
				{
					CreateParams createParams = new CreateParams();
					createParams.Style = 0;
					createParams.ExStyle = 0;
					createParams.ClassStyle = 0;
					createParams.Caption = base.GetType().Name;
					if (Environment.OSVersion.Platform == PlatformID.Win32NT)
					{
						createParams.Parent = (IntPtr)NativeMethods.HWND_MESSAGE;
					}
					this.CreateHandle(createParams);
				}
				return base.Handle != IntPtr.Zero;
			}

			// Token: 0x060060A3 RID: 24739 RVA: 0x0018BCAC File Offset: 0x00189EAC
			private bool GetInvokeRequired(IntPtr hWnd)
			{
				if (hWnd != IntPtr.Zero)
				{
					int num;
					int windowThreadProcessId = SafeNativeMethods.GetWindowThreadProcessId(new HandleRef(this, hWnd), out num);
					int currentThreadId = SafeNativeMethods.GetCurrentThreadId();
					return windowThreadProcessId != currentThreadId;
				}
				return false;
			}

			// Token: 0x060060A4 RID: 24740 RVA: 0x0018BCE4 File Offset: 0x00189EE4
			public void RestartTimer(int newInterval)
			{
				this.StopTimer(false, IntPtr.Zero);
				this.StartTimer(newInterval);
			}

			// Token: 0x060060A5 RID: 24741 RVA: 0x0018BCFC File Offset: 0x00189EFC
			public void StartTimer(int interval)
			{
				if (this._timerID == 0 && !this._stoppingTimer && this.EnsureHandle())
				{
					this._timerID = (int)SafeNativeMethods.SetTimer(new HandleRef(this, base.Handle), Timer.TimerNativeWindow.TimerID++, interval, IntPtr.Zero);
				}
			}

			// Token: 0x060060A6 RID: 24742 RVA: 0x0018BD50 File Offset: 0x00189F50
			public void StopTimer()
			{
				this.StopTimer(true, IntPtr.Zero);
			}

			// Token: 0x060060A7 RID: 24743 RVA: 0x0018BD60 File Offset: 0x00189F60
			public void StopTimer(bool destroyHwnd, IntPtr hWnd)
			{
				if (hWnd == IntPtr.Zero)
				{
					hWnd = base.Handle;
				}
				if (this.GetInvokeRequired(hWnd))
				{
					UnsafeNativeMethods.PostMessage(new HandleRef(this, hWnd), 16, 0, 0);
					return;
				}
				lock (this)
				{
					if (!this._stoppingTimer && !(hWnd == IntPtr.Zero) && UnsafeNativeMethods.IsWindow(new HandleRef(this, hWnd)))
					{
						if (this._timerID != 0)
						{
							try
							{
								this._stoppingTimer = true;
								SafeNativeMethods.KillTimer(new HandleRef(this, hWnd), this._timerID);
							}
							finally
							{
								this._timerID = 0;
								this._stoppingTimer = false;
							}
						}
						if (destroyHwnd)
						{
							base.DestroyHandle();
						}
					}
				}
			}

			// Token: 0x060060A8 RID: 24744 RVA: 0x0018BE34 File Offset: 0x0018A034
			public override void DestroyHandle()
			{
				this.StopTimer(false, IntPtr.Zero);
				base.DestroyHandle();
			}

			// Token: 0x060060A9 RID: 24745 RVA: 0x000337A1 File Offset: 0x000319A1
			protected override void OnThreadException(Exception e)
			{
				Application.OnThreadException(e);
			}

			// Token: 0x060060AA RID: 24746 RVA: 0x0018BE48 File Offset: 0x0018A048
			public override void ReleaseHandle()
			{
				this.StopTimer(false, IntPtr.Zero);
				base.ReleaseHandle();
			}

			// Token: 0x060060AB RID: 24747 RVA: 0x0018BE5C File Offset: 0x0018A05C
			protected override void WndProc(ref Message m)
			{
				if (m.Msg == 275)
				{
					if ((int)((long)m.WParam) == this._timerID)
					{
						this._owner.OnTick(EventArgs.Empty);
						return;
					}
				}
				else if (m.Msg == 16)
				{
					this.StopTimer(true, m.HWnd);
					return;
				}
				base.WndProc(ref m);
			}

			// Token: 0x0400415E RID: 16734
			private Timer _owner;

			// Token: 0x0400415F RID: 16735
			private int _timerID;

			// Token: 0x04004160 RID: 16736
			private static int TimerID = 1;

			// Token: 0x04004161 RID: 16737
			private bool _stoppingTimer;
		}
	}
}
