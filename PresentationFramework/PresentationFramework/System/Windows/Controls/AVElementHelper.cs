using System;
using System.IO.Packaging;
using System.Security;
using System.Windows.Media;
using MS.Internal;

namespace System.Windows.Controls
{
	// Token: 0x0200046F RID: 1135
	internal class AVElementHelper
	{
		// Token: 0x06004235 RID: 16949 RVA: 0x0012EE00 File Offset: 0x0012D000
		internal AVElementHelper(MediaElement element)
		{
			this._element = element;
			this._position = new SettableState<TimeSpan>(new TimeSpan(0L));
			this._mediaState = new SettableState<MediaState>(MediaState.Close);
			this._source = new SettableState<Uri>(null);
			this._clock = new SettableState<MediaClock>(null);
			this._speedRatio = new SettableState<double>(1.0);
			this._volume = new SettableState<double>(0.5);
			this._isMuted = new SettableState<bool>(false);
			this._balance = new SettableState<double>(0.0);
			this._isScrubbingEnabled = new SettableState<bool>(false);
			this._mediaPlayer = new MediaPlayer();
			this.HookEvents();
		}

		// Token: 0x06004236 RID: 16950 RVA: 0x0012EECC File Offset: 0x0012D0CC
		internal static AVElementHelper GetHelper(DependencyObject d)
		{
			MediaElement mediaElement = d as MediaElement;
			if (mediaElement != null)
			{
				return mediaElement.Helper;
			}
			throw new ArgumentException(SR.Get("AudioVideo_InvalidDependencyObject"));
		}

		// Token: 0x17001049 RID: 4169
		// (get) Token: 0x06004237 RID: 16951 RVA: 0x0012EEF9 File Offset: 0x0012D0F9
		internal MediaPlayer Player
		{
			get
			{
				return this._mediaPlayer;
			}
		}

		// Token: 0x1700104A RID: 4170
		// (get) Token: 0x06004238 RID: 16952 RVA: 0x0012EF01 File Offset: 0x0012D101
		// (set) Token: 0x06004239 RID: 16953 RVA: 0x0012EF09 File Offset: 0x0012D109
		internal Uri BaseUri
		{
			get
			{
				return this._baseUri;
			}
			set
			{
				if (value.Scheme != PackUriHelper.UriSchemePack)
				{
					this._baseUri = value;
					return;
				}
				this._baseUri = null;
			}
		}

		// Token: 0x0600423A RID: 16954 RVA: 0x0012EF2C File Offset: 0x0012D12C
		internal void SetUnloadedBehavior(MediaState unloadedBehavior)
		{
			this._unloadedBehavior = unloadedBehavior;
			this.HandleStateChange();
		}

		// Token: 0x0600423B RID: 16955 RVA: 0x0012EF3B File Offset: 0x0012D13B
		internal void SetLoadedBehavior(MediaState loadedBehavior)
		{
			this._loadedBehavior = loadedBehavior;
			this.HandleStateChange();
		}

		// Token: 0x1700104B RID: 4171
		// (get) Token: 0x0600423C RID: 16956 RVA: 0x0012EF4A File Offset: 0x0012D14A
		internal TimeSpan Position
		{
			get
			{
				if (this._currentState == MediaState.Close)
				{
					return this._position._value;
				}
				return this._mediaPlayer.Position;
			}
		}

		// Token: 0x0600423D RID: 16957 RVA: 0x0012EF6C File Offset: 0x0012D16C
		internal void SetPosition(TimeSpan position)
		{
			this._position._isSet = true;
			this._position._value = position;
			this.HandleStateChange();
		}

		// Token: 0x1700104C RID: 4172
		// (get) Token: 0x0600423E RID: 16958 RVA: 0x0012EF8C File Offset: 0x0012D18C
		internal MediaClock Clock
		{
			get
			{
				return this._clock._value;
			}
		}

		// Token: 0x0600423F RID: 16959 RVA: 0x0012EF99 File Offset: 0x0012D199
		internal void SetClock(MediaClock clock)
		{
			this._clock._value = clock;
			this._clock._isSet = true;
			this.HandleStateChange();
		}

		// Token: 0x1700104D RID: 4173
		// (get) Token: 0x06004240 RID: 16960 RVA: 0x0012EFB9 File Offset: 0x0012D1B9
		internal double SpeedRatio
		{
			get
			{
				return this._speedRatio._value;
			}
		}

		// Token: 0x06004241 RID: 16961 RVA: 0x0012EFC8 File Offset: 0x0012D1C8
		internal void SetSpeedRatio(double speedRatio)
		{
			this._speedRatio._wasSet = (this._speedRatio._isSet = true);
			this._speedRatio._value = speedRatio;
			this.HandleStateChange();
		}

		// Token: 0x06004242 RID: 16962 RVA: 0x0012F001 File Offset: 0x0012D201
		internal void SetState(MediaState mediaState)
		{
			if (this._loadedBehavior != MediaState.Manual && this._unloadedBehavior != MediaState.Manual)
			{
				throw new NotSupportedException(SR.Get("AudioVideo_CannotControlMedia"));
			}
			this._mediaState._value = mediaState;
			this._mediaState._isSet = true;
			this.HandleStateChange();
		}

		// Token: 0x06004243 RID: 16963 RVA: 0x0012F044 File Offset: 0x0012D244
		internal void SetVolume(double volume)
		{
			this._volume._wasSet = (this._volume._isSet = true);
			this._volume._value = volume;
			this.HandleStateChange();
		}

		// Token: 0x06004244 RID: 16964 RVA: 0x0012F080 File Offset: 0x0012D280
		internal void SetBalance(double balance)
		{
			this._balance._wasSet = (this._balance._isSet = true);
			this._balance._value = balance;
			this.HandleStateChange();
		}

		// Token: 0x06004245 RID: 16965 RVA: 0x0012F0BC File Offset: 0x0012D2BC
		internal void SetIsMuted(bool isMuted)
		{
			this._isMuted._wasSet = (this._isMuted._isSet = true);
			this._isMuted._value = isMuted;
			this.HandleStateChange();
		}

		// Token: 0x06004246 RID: 16966 RVA: 0x0012F0F8 File Offset: 0x0012D2F8
		internal void SetScrubbingEnabled(bool isScrubbingEnabled)
		{
			this._isScrubbingEnabled._wasSet = (this._isScrubbingEnabled._isSet = true);
			this._isScrubbingEnabled._value = isScrubbingEnabled;
			this.HandleStateChange();
		}

		// Token: 0x06004247 RID: 16967 RVA: 0x0012F134 File Offset: 0x0012D334
		private void HookEvents()
		{
			this._mediaPlayer.MediaOpened += this.OnMediaOpened;
			this._mediaPlayer.MediaFailed += this.OnMediaFailed;
			this._mediaPlayer.BufferingStarted += this.OnBufferingStarted;
			this._mediaPlayer.BufferingEnded += this.OnBufferingEnded;
			this._mediaPlayer.MediaEnded += this.OnMediaEnded;
			this._mediaPlayer.ScriptCommand += this.OnScriptCommand;
			this._element.Loaded += this.OnLoaded;
			this._element.Unloaded += this.OnUnloaded;
		}

		// Token: 0x06004248 RID: 16968 RVA: 0x0012F1FC File Offset: 0x0012D3FC
		private void HandleStateChange()
		{
			MediaState mediaState = this._mediaState._value;
			bool flag = false;
			bool flag2 = false;
			if (this._isLoaded)
			{
				if (this._clock._value != null)
				{
					mediaState = MediaState.Manual;
					flag = true;
				}
				else if (this._loadedBehavior != MediaState.Manual)
				{
					mediaState = this._loadedBehavior;
				}
				else if (this._source._wasSet)
				{
					if (this._loadedBehavior != MediaState.Manual)
					{
						mediaState = MediaState.Play;
					}
					else
					{
						flag2 = true;
					}
				}
			}
			else if (this._unloadedBehavior != MediaState.Manual)
			{
				mediaState = this._unloadedBehavior;
			}
			else
			{
				Invariant.Assert(this._unloadedBehavior == MediaState.Manual);
				if (this._clock._value != null)
				{
					mediaState = MediaState.Manual;
					flag = true;
				}
				else
				{
					flag2 = true;
				}
			}
			bool flag3 = false;
			if (mediaState != MediaState.Close && mediaState != MediaState.Manual)
			{
				Invariant.Assert(!flag);
				if (this._mediaPlayer.Clock != null)
				{
					this._mediaPlayer.Clock = null;
				}
				if (this._currentState == MediaState.Close || this._source._isSet)
				{
					if (this._isScrubbingEnabled._wasSet)
					{
						this._mediaPlayer.ScrubbingEnabled = this._isScrubbingEnabled._value;
						this._isScrubbingEnabled._isSet = false;
					}
					if (this._clock._value == null)
					{
						this._mediaPlayer.Open(this.UriFromSourceUri(this._source._value));
					}
					flag3 = true;
				}
			}
			else if (flag)
			{
				if (this._currentState == MediaState.Close || this._clock._isSet)
				{
					if (this._isScrubbingEnabled._wasSet)
					{
						this._mediaPlayer.ScrubbingEnabled = this._isScrubbingEnabled._value;
						this._isScrubbingEnabled._isSet = false;
					}
					this._mediaPlayer.Clock = this._clock._value;
					this._clock._isSet = false;
					flag3 = true;
				}
			}
			else if (mediaState == MediaState.Close && this._currentState != MediaState.Close)
			{
				this._mediaPlayer.Clock = null;
				this._mediaPlayer.Close();
				this._currentState = MediaState.Close;
			}
			if (this._currentState != MediaState.Close || flag3)
			{
				if (this._position._isSet)
				{
					this._mediaPlayer.Position = this._position._value;
					this._position._isSet = false;
				}
				if (this._volume._isSet || (flag3 && this._volume._wasSet))
				{
					this._mediaPlayer.Volume = this._volume._value;
					this._volume._isSet = false;
				}
				if (this._balance._isSet || (flag3 && this._balance._wasSet))
				{
					this._mediaPlayer.Balance = this._balance._value;
					this._balance._isSet = false;
				}
				if (this._isMuted._isSet || (flag3 && this._isMuted._wasSet))
				{
					this._mediaPlayer.IsMuted = this._isMuted._value;
					this._isMuted._isSet = false;
				}
				if (this._isScrubbingEnabled._isSet)
				{
					this._mediaPlayer.ScrubbingEnabled = this._isScrubbingEnabled._value;
					this._isScrubbingEnabled._isSet = false;
				}
				if (mediaState == MediaState.Play && this._source._isSet)
				{
					this._mediaPlayer.Play();
					if (!this._speedRatio._wasSet)
					{
						this._mediaPlayer.SpeedRatio = 1.0;
					}
					this._source._isSet = false;
					this._mediaState._isSet = false;
				}
				else if (this._currentState != mediaState || (flag2 && this._mediaState._isSet))
				{
					switch (mediaState)
					{
					case MediaState.Manual:
						goto IL_3BE;
					case MediaState.Play:
						this._mediaPlayer.Play();
						goto IL_3BE;
					case MediaState.Pause:
						this._mediaPlayer.Pause();
						goto IL_3BE;
					case MediaState.Stop:
						this._mediaPlayer.Stop();
						goto IL_3BE;
					}
					Invariant.Assert(false, "Unexpected state request.");
					IL_3BE:
					if (flag2)
					{
						this._mediaState._isSet = false;
					}
				}
				this._currentState = mediaState;
				if (this._speedRatio._isSet || (flag3 && this._speedRatio._wasSet))
				{
					this._mediaPlayer.SpeedRatio = this._speedRatio._value;
					this._speedRatio._isSet = false;
				}
			}
		}

		// Token: 0x06004249 RID: 16969 RVA: 0x0012F61C File Offset: 0x0012D81C
		private Uri UriFromSourceUri(Uri sourceUri)
		{
			if (sourceUri != null)
			{
				if (sourceUri.IsAbsoluteUri)
				{
					return sourceUri;
				}
				if (this.BaseUri != null)
				{
					return new Uri(this.BaseUri, sourceUri);
				}
			}
			return sourceUri;
		}

		// Token: 0x0600424A RID: 16970 RVA: 0x0012F650 File Offset: 0x0012D850
		[SecurityCritical]
		[SecurityTreatAsSafe]
		internal static void OnSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			if (e.IsASubPropertyChange)
			{
				return;
			}
			AVElementHelper helper = AVElementHelper.GetHelper(d);
			helper.MemberOnInvalidateSource(e);
		}

		// Token: 0x0600424B RID: 16971 RVA: 0x0012F678 File Offset: 0x0012D878
		private void MemberOnInvalidateSource(DependencyPropertyChangedEventArgs e)
		{
			if (this._clock._value != null)
			{
				throw new InvalidOperationException(SR.Get("MediaElement_CannotSetSourceOnMediaElementDrivenByClock"));
			}
			this._source._value = (Uri)e.NewValue;
			this._source._wasSet = (this._source._isSet = true);
			this.HandleStateChange();
		}

		// Token: 0x0600424C RID: 16972 RVA: 0x0012F6D9 File Offset: 0x0012D8D9
		private void OnMediaFailed(object sender, ExceptionEventArgs args)
		{
			this._element.OnMediaFailed(sender, args);
		}

		// Token: 0x0600424D RID: 16973 RVA: 0x0012F6E8 File Offset: 0x0012D8E8
		private void OnMediaOpened(object sender, EventArgs args)
		{
			this._element.InvalidateMeasure();
			this._element.OnMediaOpened(sender, args);
		}

		// Token: 0x0600424E RID: 16974 RVA: 0x0012F702 File Offset: 0x0012D902
		private void OnBufferingStarted(object sender, EventArgs args)
		{
			this._element.OnBufferingStarted(sender, args);
		}

		// Token: 0x0600424F RID: 16975 RVA: 0x0012F711 File Offset: 0x0012D911
		private void OnBufferingEnded(object sender, EventArgs args)
		{
			this._element.OnBufferingEnded(sender, args);
		}

		// Token: 0x06004250 RID: 16976 RVA: 0x0012F720 File Offset: 0x0012D920
		private void OnMediaEnded(object sender, EventArgs args)
		{
			this._element.OnMediaEnded(sender, args);
		}

		// Token: 0x06004251 RID: 16977 RVA: 0x0012F72F File Offset: 0x0012D92F
		private void OnScriptCommand(object sender, MediaScriptCommandEventArgs args)
		{
			this._element.OnScriptCommand(sender, args);
		}

		// Token: 0x06004252 RID: 16978 RVA: 0x0012F73E File Offset: 0x0012D93E
		private void OnLoaded(object sender, RoutedEventArgs args)
		{
			this._isLoaded = true;
			this.HandleStateChange();
		}

		// Token: 0x06004253 RID: 16979 RVA: 0x0012F74D File Offset: 0x0012D94D
		private void OnUnloaded(object sender, RoutedEventArgs args)
		{
			this._isLoaded = false;
			this.HandleStateChange();
		}

		// Token: 0x040027DC RID: 10204
		private MediaPlayer _mediaPlayer;

		// Token: 0x040027DD RID: 10205
		private MediaElement _element;

		// Token: 0x040027DE RID: 10206
		private Uri _baseUri;

		// Token: 0x040027DF RID: 10207
		private MediaState _unloadedBehavior = MediaState.Close;

		// Token: 0x040027E0 RID: 10208
		private MediaState _loadedBehavior = MediaState.Play;

		// Token: 0x040027E1 RID: 10209
		private MediaState _currentState = MediaState.Close;

		// Token: 0x040027E2 RID: 10210
		private bool _isLoaded;

		// Token: 0x040027E3 RID: 10211
		private SettableState<TimeSpan> _position;

		// Token: 0x040027E4 RID: 10212
		private SettableState<MediaState> _mediaState;

		// Token: 0x040027E5 RID: 10213
		private SettableState<Uri> _source;

		// Token: 0x040027E6 RID: 10214
		private SettableState<MediaClock> _clock;

		// Token: 0x040027E7 RID: 10215
		private SettableState<double> _speedRatio;

		// Token: 0x040027E8 RID: 10216
		private SettableState<double> _volume;

		// Token: 0x040027E9 RID: 10217
		private SettableState<bool> _isMuted;

		// Token: 0x040027EA RID: 10218
		private SettableState<double> _balance;

		// Token: 0x040027EB RID: 10219
		private SettableState<bool> _isScrubbingEnabled;
	}
}
