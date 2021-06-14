using System;
using System.Windows.Media;
using MS.Internal;

namespace System.Windows.Shell
{
	/// <summary>Represents information about how the taskbar thumbnail is displayed.</summary>
	// Token: 0x0200014C RID: 332
	public sealed class TaskbarItemInfo : Freezable
	{
		// Token: 0x06000E90 RID: 3728 RVA: 0x000380F5 File Offset: 0x000362F5
		protected override Freezable CreateInstanceCore()
		{
			return new TaskbarItemInfo();
		}

		/// <summary>Gets or sets a value that indicates how the progress indicator is displayed in the taskbar button.</summary>
		/// <returns>An enumeration value that indicates how the progress indicator is displayed in the taskbar button. The default is <see cref="F:System.Windows.Shell.TaskbarItemProgressState.None" />.</returns>
		// Token: 0x1700046C RID: 1132
		// (get) Token: 0x06000E91 RID: 3729 RVA: 0x000380FC File Offset: 0x000362FC
		// (set) Token: 0x06000E92 RID: 3730 RVA: 0x0003810E File Offset: 0x0003630E
		public TaskbarItemProgressState ProgressState
		{
			get
			{
				return (TaskbarItemProgressState)base.GetValue(TaskbarItemInfo.ProgressStateProperty);
			}
			set
			{
				base.SetValue(TaskbarItemInfo.ProgressStateProperty, value);
			}
		}

		// Token: 0x06000E93 RID: 3731 RVA: 0x00038121 File Offset: 0x00036321
		private TaskbarItemProgressState CoerceProgressState(TaskbarItemProgressState value)
		{
			if (value > TaskbarItemProgressState.Paused)
			{
				value = TaskbarItemProgressState.None;
			}
			return value;
		}

		/// <summary>Gets or sets a value that indicates the fullness of the progress indicator in the taskbar button.</summary>
		/// <returns>A value that indicates the fullness of the progress indicator in the taskbar button. The default is 0.</returns>
		// Token: 0x1700046D RID: 1133
		// (get) Token: 0x06000E94 RID: 3732 RVA: 0x0003812B File Offset: 0x0003632B
		// (set) Token: 0x06000E95 RID: 3733 RVA: 0x0003813D File Offset: 0x0003633D
		public double ProgressValue
		{
			get
			{
				return (double)base.GetValue(TaskbarItemInfo.ProgressValueProperty);
			}
			set
			{
				base.SetValue(TaskbarItemInfo.ProgressValueProperty, value);
			}
		}

		// Token: 0x06000E96 RID: 3734 RVA: 0x00038150 File Offset: 0x00036350
		private static double CoerceProgressValue(double progressValue)
		{
			if (double.IsNaN(progressValue))
			{
				progressValue = 0.0;
			}
			else
			{
				progressValue = Math.Max(progressValue, 0.0);
				progressValue = Math.Min(1.0, progressValue);
			}
			return progressValue;
		}

		/// <summary>Gets or sets the image that is displayed over the program icon in the taskbar button.</summary>
		/// <returns>The image that is displayed over the program icon in the taskbar button. The default is <see langword="null" />.</returns>
		// Token: 0x1700046E RID: 1134
		// (get) Token: 0x06000E97 RID: 3735 RVA: 0x0003818A File Offset: 0x0003638A
		// (set) Token: 0x06000E98 RID: 3736 RVA: 0x0003819C File Offset: 0x0003639C
		public ImageSource Overlay
		{
			get
			{
				return (ImageSource)base.GetValue(TaskbarItemInfo.OverlayProperty);
			}
			set
			{
				base.SetValue(TaskbarItemInfo.OverlayProperty, value);
			}
		}

		/// <summary>Gets or sets the text for the taskbar item tooltip.</summary>
		/// <returns>The text for the taskbar item tooltip. The default is an empty string.</returns>
		// Token: 0x1700046F RID: 1135
		// (get) Token: 0x06000E99 RID: 3737 RVA: 0x000381AA File Offset: 0x000363AA
		// (set) Token: 0x06000E9A RID: 3738 RVA: 0x000381BC File Offset: 0x000363BC
		public string Description
		{
			get
			{
				return (string)base.GetValue(TaskbarItemInfo.DescriptionProperty);
			}
			set
			{
				base.SetValue(TaskbarItemInfo.DescriptionProperty, value);
			}
		}

		/// <summary>Gets or sets a value that specifies the part of the application window's client area that is displayed in the taskbar thumbnail.</summary>
		/// <returns>A value that specifies the part of the application window's client area that is displayed in the taskbar thumbnail. The default is an empty <see cref="T:System.Windows.Thickness" />.</returns>
		// Token: 0x17000470 RID: 1136
		// (get) Token: 0x06000E9B RID: 3739 RVA: 0x000381CA File Offset: 0x000363CA
		// (set) Token: 0x06000E9C RID: 3740 RVA: 0x000381DC File Offset: 0x000363DC
		public Thickness ThumbnailClipMargin
		{
			get
			{
				return (Thickness)base.GetValue(TaskbarItemInfo.ThumbnailClipMarginProperty);
			}
			set
			{
				base.SetValue(TaskbarItemInfo.ThumbnailClipMarginProperty, value);
			}
		}

		// Token: 0x06000E9D RID: 3741 RVA: 0x000381F0 File Offset: 0x000363F0
		private Thickness CoerceThumbnailClipMargin(Thickness margin)
		{
			if (!margin.IsValid(false, false, false, false))
			{
				return default(Thickness);
			}
			return margin;
		}

		/// <summary>Gets or sets the collection of <see cref="T:System.Windows.Shell.ThumbButtonInfo" /> objects that are associated with the <see cref="T:System.Windows.Window" />.</summary>
		/// <returns>The collection of <see cref="T:System.Windows.Shell.ThumbButtonInfo" /> objects that are associated with the <see cref="T:System.Windows.Window" />. The default is an empty collection.</returns>
		// Token: 0x17000471 RID: 1137
		// (get) Token: 0x06000E9E RID: 3742 RVA: 0x00038215 File Offset: 0x00036415
		// (set) Token: 0x06000E9F RID: 3743 RVA: 0x00038227 File Offset: 0x00036427
		public ThumbButtonInfoCollection ThumbButtonInfos
		{
			get
			{
				return (ThumbButtonInfoCollection)base.GetValue(TaskbarItemInfo.ThumbButtonInfosProperty);
			}
			set
			{
				base.SetValue(TaskbarItemInfo.ThumbButtonInfosProperty, value);
			}
		}

		// Token: 0x06000EA0 RID: 3744 RVA: 0x00038238 File Offset: 0x00036438
		private void NotifyDependencyPropertyChanged(DependencyPropertyChangedEventArgs e)
		{
			DependencyPropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			if (propertyChanged != null)
			{
				propertyChanged(this, e);
			}
		}

		// Token: 0x1400003B RID: 59
		// (add) Token: 0x06000EA1 RID: 3745 RVA: 0x00038258 File Offset: 0x00036458
		// (remove) Token: 0x06000EA2 RID: 3746 RVA: 0x00038290 File Offset: 0x00036490
		internal event DependencyPropertyChangedEventHandler PropertyChanged;

		/// <summary>Identifies the <see cref="P:System.Windows.Shell.TaskbarItemInfo.ProgressState" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Shell.TaskbarItemInfo.ProgressState" /> dependency property.</returns>
		// Token: 0x0400113D RID: 4413
		public static readonly DependencyProperty ProgressStateProperty = DependencyProperty.Register("ProgressState", typeof(TaskbarItemProgressState), typeof(TaskbarItemInfo), new PropertyMetadata(TaskbarItemProgressState.None, delegate(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((TaskbarItemInfo)d).NotifyDependencyPropertyChanged(e);
		}, (DependencyObject d, object baseValue) => ((TaskbarItemInfo)d).CoerceProgressState((TaskbarItemProgressState)baseValue)));

		/// <summary>Identifies the <see cref="P:System.Windows.Shell.TaskbarItemInfo.ProgressValue" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Shell.TaskbarItemInfo.ProgressValue" /> dependency property.</returns>
		// Token: 0x0400113E RID: 4414
		public static readonly DependencyProperty ProgressValueProperty = DependencyProperty.Register("ProgressValue", typeof(double), typeof(TaskbarItemInfo), new PropertyMetadata(0.0, delegate(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((TaskbarItemInfo)d).NotifyDependencyPropertyChanged(e);
		}, (DependencyObject d, object baseValue) => TaskbarItemInfo.CoerceProgressValue((double)baseValue)));

		/// <summary>Identifies the <see cref="P:System.Windows.Shell.TaskbarItemInfo.Overlay" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Shell.TaskbarItemInfo.Overlay" /> dependency property.</returns>
		// Token: 0x0400113F RID: 4415
		public static readonly DependencyProperty OverlayProperty = DependencyProperty.Register("Overlay", typeof(ImageSource), typeof(TaskbarItemInfo), new PropertyMetadata(null, delegate(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((TaskbarItemInfo)d).NotifyDependencyPropertyChanged(e);
		}));

		/// <summary>Identifies the <see cref="P:System.Windows.Shell.TaskbarItemInfo.Description" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Shell.TaskbarItemInfo.Description" /> dependency property.</returns>
		// Token: 0x04001140 RID: 4416
		public static readonly DependencyProperty DescriptionProperty = DependencyProperty.Register("Description", typeof(string), typeof(TaskbarItemInfo), new PropertyMetadata(string.Empty, delegate(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((TaskbarItemInfo)d).NotifyDependencyPropertyChanged(e);
		}));

		/// <summary>Identifies the <see cref="P:System.Windows.Shell.TaskbarItemInfo.ThumbnailClipMargin" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Shell.TaskbarItemInfo.ThumbnailClipMargin" /> dependency property.</returns>
		// Token: 0x04001141 RID: 4417
		public static readonly DependencyProperty ThumbnailClipMarginProperty = DependencyProperty.Register("ThumbnailClipMargin", typeof(Thickness), typeof(TaskbarItemInfo), new PropertyMetadata(default(Thickness), delegate(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((TaskbarItemInfo)d).NotifyDependencyPropertyChanged(e);
		}, (DependencyObject d, object baseValue) => ((TaskbarItemInfo)d).CoerceThumbnailClipMargin((Thickness)baseValue)));

		/// <summary>Identifies the <see cref="P:System.Windows.Shell.TaskbarItemInfo.ThumbButtonInfos" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Shell.TaskbarItemInfo.ThumbButtonInfos" /> dependency property.</returns>
		// Token: 0x04001142 RID: 4418
		public static readonly DependencyProperty ThumbButtonInfosProperty = DependencyProperty.Register("ThumbButtonInfos", typeof(ThumbButtonInfoCollection), typeof(TaskbarItemInfo), new PropertyMetadata(new FreezableDefaultValueFactory(ThumbButtonInfoCollection.Empty), delegate(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((TaskbarItemInfo)d).NotifyDependencyPropertyChanged(e);
		}));
	}
}
