using System;

namespace System.Windows.Controls
{
	/// <summary>Specifies the states that can be applied to a <see cref="T:System.Windows.Controls.MediaElement" /> for the <see cref="P:System.Windows.Controls.MediaElement.LoadedBehavior" /> and <see cref="P:System.Windows.Controls.MediaElement.UnloadedBehavior" /> properties.</summary>
	// Token: 0x020004FF RID: 1279
	public enum MediaState
	{
		/// <summary>The state used to control a <see cref="T:System.Windows.Controls.MediaElement" /> manually. Interactive methods like <see cref="M:System.Windows.Controls.MediaElement.Play" /> and <see cref="M:System.Windows.Controls.MediaElement.Pause" /> can be used. Media will preroll but not play when the <see cref="T:System.Windows.Controls.MediaElement" /> is assigned valid media source.</summary>
		// Token: 0x04002C6A RID: 11370
		Manual,
		/// <summary>The state used to play the media. . Media will preroll automatically being playback when the <see cref="T:System.Windows.Controls.MediaElement" /> is assigned valid media source.</summary>
		// Token: 0x04002C6B RID: 11371
		Play,
		/// <summary>The state used to close the media. All media resources are released (including video memory).</summary>
		// Token: 0x04002C6C RID: 11372
		Close,
		/// <summary>The state used to pause the media. Media will preroll but remains paused when the <see cref="T:System.Windows.Controls.MediaElement" /> is assigned valid media source.</summary>
		// Token: 0x04002C6D RID: 11373
		Pause,
		/// <summary>The state used to stop the media. Media will preroll but not play when the <see cref="T:System.Windows.Controls.MediaElement" /> is assigned valid media source. Media resources are not released.</summary>
		// Token: 0x04002C6E RID: 11374
		Stop
	}
}
