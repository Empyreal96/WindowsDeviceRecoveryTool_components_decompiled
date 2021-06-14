using System;
using System.ComponentModel;
using System.IO;
using System.Media;
using System.Windows.Navigation;
using System.Windows.Threading;
using MS.Internal;

namespace System.Windows.Controls
{
	/// <summary>Represents a lightweight audio playback <see cref="T:System.Windows.TriggerAction" /> used to play .wav files.</summary>
	// Token: 0x02000533 RID: 1331
	public class SoundPlayerAction : TriggerAction, IDisposable
	{
		/// <summary>Releases the resources used by the <see cref="T:System.Windows.Controls.SoundPlayerAction" /> class.</summary>
		// Token: 0x06005651 RID: 22097 RVA: 0x0017E806 File Offset: 0x0017CA06
		public void Dispose()
		{
			if (this.m_player != null)
			{
				this.m_player.Dispose();
			}
		}

		/// <summary>Gets or sets the audio source location. </summary>
		/// <returns>The audio source location.</returns>
		// Token: 0x170014FC RID: 5372
		// (get) Token: 0x06005652 RID: 22098 RVA: 0x0017E81B File Offset: 0x0017CA1B
		// (set) Token: 0x06005653 RID: 22099 RVA: 0x0017E82D File Offset: 0x0017CA2D
		public Uri Source
		{
			get
			{
				return (Uri)base.GetValue(SoundPlayerAction.SourceProperty);
			}
			set
			{
				base.SetValue(SoundPlayerAction.SourceProperty, value);
			}
		}

		// Token: 0x06005654 RID: 22100 RVA: 0x0017E83C File Offset: 0x0017CA3C
		private static void OnSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			SoundPlayerAction soundPlayerAction = (SoundPlayerAction)d;
			soundPlayerAction.OnSourceChangedHelper((Uri)e.NewValue);
		}

		// Token: 0x06005655 RID: 22101 RVA: 0x0017E864 File Offset: 0x0017CA64
		private void OnSourceChangedHelper(Uri newValue)
		{
			if (newValue == null || newValue.IsAbsoluteUri)
			{
				this.m_lastRequestedAbsoluteUri = newValue;
			}
			else
			{
				this.m_lastRequestedAbsoluteUri = BaseUriHelper.GetResolvedUri(BaseUriHelper.BaseUri, newValue);
			}
			this.m_player = null;
			this.m_playRequested = false;
			if (this.m_streamLoadInProgress)
			{
				this.m_uriChangedWhileLoadingStream = true;
				return;
			}
			this.BeginLoadStream();
		}

		// Token: 0x06005656 RID: 22102 RVA: 0x0017E8C0 File Offset: 0x0017CAC0
		internal sealed override void Invoke(FrameworkElement el, FrameworkContentElement ctntEl, Style targetStyle, FrameworkTemplate targetTemplate, long layer)
		{
			this.PlayWhenLoaded();
		}

		// Token: 0x06005657 RID: 22103 RVA: 0x0017E8C0 File Offset: 0x0017CAC0
		internal sealed override void Invoke(FrameworkElement el)
		{
			this.PlayWhenLoaded();
		}

		// Token: 0x06005658 RID: 22104 RVA: 0x0017E8C8 File Offset: 0x0017CAC8
		private void PlayWhenLoaded()
		{
			if (this.m_streamLoadInProgress)
			{
				this.m_playRequested = true;
				return;
			}
			if (this.m_player != null)
			{
				this.m_player.Play();
			}
		}

		// Token: 0x06005659 RID: 22105 RVA: 0x0017E8F0 File Offset: 0x0017CAF0
		private void BeginLoadStream()
		{
			if (this.m_lastRequestedAbsoluteUri != null)
			{
				this.m_streamLoadInProgress = true;
				SoundPlayerAction.LoadStreamCaller loadStreamCaller = new SoundPlayerAction.LoadStreamCaller(this.LoadStreamAsync);
				IAsyncResult asyncResult = loadStreamCaller.BeginInvoke(this.m_lastRequestedAbsoluteUri, new AsyncCallback(this.LoadStreamCallback), loadStreamCaller);
			}
		}

		// Token: 0x0600565A RID: 22106 RVA: 0x0017E939 File Offset: 0x0017CB39
		private Stream LoadStreamAsync(Uri uri)
		{
			return WpfWebRequestHelper.CreateRequestAndGetResponseStream(uri);
		}

		// Token: 0x0600565B RID: 22107 RVA: 0x0017E944 File Offset: 0x0017CB44
		private void LoadStreamCallback(IAsyncResult asyncResult)
		{
			DispatcherOperationCallback method = new DispatcherOperationCallback(this.OnLoadStreamCompleted);
			base.Dispatcher.BeginInvoke(DispatcherPriority.Normal, method, asyncResult);
		}

		// Token: 0x0600565C RID: 22108 RVA: 0x0017E970 File Offset: 0x0017CB70
		private object OnLoadStreamCompleted(object asyncResultArg)
		{
			IAsyncResult asyncResult = (IAsyncResult)asyncResultArg;
			SoundPlayerAction.LoadStreamCaller loadStreamCaller = (SoundPlayerAction.LoadStreamCaller)asyncResult.AsyncState;
			Stream stream = loadStreamCaller.EndInvoke(asyncResult);
			if (this.m_uriChangedWhileLoadingStream)
			{
				this.m_uriChangedWhileLoadingStream = false;
				if (stream != null)
				{
					stream.Dispose();
				}
				this.BeginLoadStream();
			}
			else if (stream != null)
			{
				if (this.m_player == null)
				{
					this.m_player = new SoundPlayer(stream);
				}
				else
				{
					this.m_player.Stream = stream;
				}
				this.m_player.LoadCompleted += this.OnSoundPlayerLoadCompleted;
				this.m_player.LoadAsync();
			}
			return null;
		}

		// Token: 0x0600565D RID: 22109 RVA: 0x0017EA00 File Offset: 0x0017CC00
		private void OnSoundPlayerLoadCompleted(object sender, AsyncCompletedEventArgs e)
		{
			if (this.m_player == sender)
			{
				if (this.m_uriChangedWhileLoadingStream)
				{
					this.m_player = null;
					this.m_uriChangedWhileLoadingStream = false;
					this.BeginLoadStream();
					return;
				}
				this.m_streamLoadInProgress = false;
				if (this.m_playRequested)
				{
					this.m_playRequested = false;
					this.m_player.Play();
				}
			}
		}

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.SoundPlayerAction.Source" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.SoundPlayerAction.Source" /> dependency property.</returns>
		// Token: 0x04002E3E RID: 11838
		public static readonly DependencyProperty SourceProperty = DependencyProperty.Register("Source", typeof(Uri), typeof(SoundPlayerAction), new FrameworkPropertyMetadata(new PropertyChangedCallback(SoundPlayerAction.OnSourceChanged)));

		// Token: 0x04002E3F RID: 11839
		private SoundPlayer m_player;

		// Token: 0x04002E40 RID: 11840
		private Uri m_lastRequestedAbsoluteUri;

		// Token: 0x04002E41 RID: 11841
		private bool m_streamLoadInProgress;

		// Token: 0x04002E42 RID: 11842
		private bool m_playRequested;

		// Token: 0x04002E43 RID: 11843
		private bool m_uriChangedWhileLoadingStream;

		// Token: 0x020009BC RID: 2492
		// (Invoke) Token: 0x0600887A RID: 34938
		private delegate Stream LoadStreamCaller(Uri uri);
	}
}
