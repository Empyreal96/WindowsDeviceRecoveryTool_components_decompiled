using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Threading;

namespace System.Drawing
{
	/// <summary>Animates an image that has time-based frames.</summary>
	// Token: 0x02000021 RID: 33
	public sealed class ImageAnimator
	{
		// Token: 0x06000359 RID: 857 RVA: 0x00003800 File Offset: 0x00001A00
		private ImageAnimator()
		{
		}

		/// <summary>Advances the frame in the specified image. The new frame is drawn the next time the image is rendered. This method applies only to images with time-based frames.</summary>
		/// <param name="image">The <see cref="T:System.Drawing.Image" /> object for which to update frames. </param>
		// Token: 0x0600035A RID: 858 RVA: 0x0000FC0C File Offset: 0x0000DE0C
		public static void UpdateFrames(Image image)
		{
			if (!ImageAnimator.anyFrameDirty || image == null || ImageAnimator.imageInfoList == null)
			{
				return;
			}
			if (ImageAnimator.threadWriterLockWaitCount > 0)
			{
				return;
			}
			ImageAnimator.rwImgListLock.AcquireReaderLock(-1);
			try
			{
				bool flag = false;
				bool flag2 = false;
				foreach (ImageAnimator.ImageInfo imageInfo in ImageAnimator.imageInfoList)
				{
					if (imageInfo.Image == image)
					{
						if (imageInfo.FrameDirty)
						{
							Image image2 = imageInfo.Image;
							lock (image2)
							{
								imageInfo.UpdateFrame();
							}
						}
						flag2 = true;
					}
					if (imageInfo.FrameDirty)
					{
						flag = true;
					}
					if (flag && flag2)
					{
						break;
					}
				}
				ImageAnimator.anyFrameDirty = flag;
			}
			finally
			{
				ImageAnimator.rwImgListLock.ReleaseReaderLock();
			}
		}

		/// <summary>Advances the frame in all images currently being animated. The new frame is drawn the next time the image is rendered.</summary>
		// Token: 0x0600035B RID: 859 RVA: 0x0000FCF8 File Offset: 0x0000DEF8
		public static void UpdateFrames()
		{
			if (!ImageAnimator.anyFrameDirty || ImageAnimator.imageInfoList == null)
			{
				return;
			}
			if (ImageAnimator.threadWriterLockWaitCount > 0)
			{
				return;
			}
			ImageAnimator.rwImgListLock.AcquireReaderLock(-1);
			try
			{
				foreach (ImageAnimator.ImageInfo imageInfo in ImageAnimator.imageInfoList)
				{
					Image image = imageInfo.Image;
					lock (image)
					{
						imageInfo.UpdateFrame();
					}
				}
				ImageAnimator.anyFrameDirty = false;
			}
			finally
			{
				ImageAnimator.rwImgListLock.ReleaseReaderLock();
			}
		}

		/// <summary>Displays a multiple-frame image as an animation.</summary>
		/// <param name="image">The <see cref="T:System.Drawing.Image" /> object to animate. </param>
		/// <param name="onFrameChangedHandler">An <see langword="EventHandler" /> object that specifies the method that is called when the animation frame changes. </param>
		// Token: 0x0600035C RID: 860 RVA: 0x0000FDB4 File Offset: 0x0000DFB4
		public static void Animate(Image image, EventHandler onFrameChangedHandler)
		{
			if (image == null)
			{
				return;
			}
			ImageAnimator.ImageInfo imageInfo = null;
			lock (image)
			{
				imageInfo = new ImageAnimator.ImageInfo(image);
			}
			ImageAnimator.StopAnimate(image, onFrameChangedHandler);
			bool isReaderLockHeld = ImageAnimator.rwImgListLock.IsReaderLockHeld;
			LockCookie lockCookie = default(LockCookie);
			ImageAnimator.threadWriterLockWaitCount++;
			try
			{
				if (isReaderLockHeld)
				{
					lockCookie = ImageAnimator.rwImgListLock.UpgradeToWriterLock(-1);
				}
				else
				{
					ImageAnimator.rwImgListLock.AcquireWriterLock(-1);
				}
			}
			finally
			{
				ImageAnimator.threadWriterLockWaitCount--;
			}
			try
			{
				if (imageInfo.Animated)
				{
					if (ImageAnimator.imageInfoList == null)
					{
						ImageAnimator.imageInfoList = new List<ImageAnimator.ImageInfo>();
					}
					imageInfo.FrameChangedHandler = onFrameChangedHandler;
					ImageAnimator.imageInfoList.Add(imageInfo);
					if (ImageAnimator.animationThread == null)
					{
						ImageAnimator.animationThread = new Thread(new ThreadStart(ImageAnimator.AnimateImages50ms));
						ImageAnimator.animationThread.Name = typeof(ImageAnimator).Name;
						ImageAnimator.animationThread.IsBackground = true;
						ImageAnimator.animationThread.Start();
					}
				}
			}
			finally
			{
				if (isReaderLockHeld)
				{
					ImageAnimator.rwImgListLock.DowngradeFromWriterLock(ref lockCookie);
				}
				else
				{
					ImageAnimator.rwImgListLock.ReleaseWriterLock();
				}
			}
		}

		/// <summary>Returns a Boolean value indicating whether the specified image contains time-based frames.</summary>
		/// <param name="image">The <see cref="T:System.Drawing.Image" /> object to test. </param>
		/// <returns>This method returns <see langword="true" /> if the specified image contains time-based frames; otherwise, <see langword="false" />.</returns>
		// Token: 0x0600035D RID: 861 RVA: 0x0000FEF8 File Offset: 0x0000E0F8
		public static bool CanAnimate(Image image)
		{
			if (image == null)
			{
				return false;
			}
			lock (image)
			{
				Guid[] frameDimensionsList = image.FrameDimensionsList;
				foreach (Guid guid in frameDimensionsList)
				{
					FrameDimension frameDimension = new FrameDimension(guid);
					if (frameDimension.Equals(FrameDimension.Time))
					{
						return image.GetFrameCount(FrameDimension.Time) > 1;
					}
				}
			}
			return false;
		}

		/// <summary>Terminates a running animation.</summary>
		/// <param name="image">The <see cref="T:System.Drawing.Image" /> object to stop animating. </param>
		/// <param name="onFrameChangedHandler">An <see langword="EventHandler" /> object that specifies the method that is called when the animation frame changes. </param>
		// Token: 0x0600035E RID: 862 RVA: 0x0000FF84 File Offset: 0x0000E184
		public static void StopAnimate(Image image, EventHandler onFrameChangedHandler)
		{
			if (image == null || ImageAnimator.imageInfoList == null)
			{
				return;
			}
			bool isReaderLockHeld = ImageAnimator.rwImgListLock.IsReaderLockHeld;
			LockCookie lockCookie = default(LockCookie);
			ImageAnimator.threadWriterLockWaitCount++;
			try
			{
				if (isReaderLockHeld)
				{
					lockCookie = ImageAnimator.rwImgListLock.UpgradeToWriterLock(-1);
				}
				else
				{
					ImageAnimator.rwImgListLock.AcquireWriterLock(-1);
				}
			}
			finally
			{
				ImageAnimator.threadWriterLockWaitCount--;
			}
			try
			{
				int i = 0;
				while (i < ImageAnimator.imageInfoList.Count)
				{
					ImageAnimator.ImageInfo imageInfo = ImageAnimator.imageInfoList[i];
					if (image == imageInfo.Image)
					{
						if (onFrameChangedHandler == imageInfo.FrameChangedHandler || (onFrameChangedHandler != null && onFrameChangedHandler.Equals(imageInfo.FrameChangedHandler)))
						{
							ImageAnimator.imageInfoList.Remove(imageInfo);
							break;
						}
						break;
					}
					else
					{
						i++;
					}
				}
			}
			finally
			{
				if (isReaderLockHeld)
				{
					ImageAnimator.rwImgListLock.DowngradeFromWriterLock(ref lockCookie);
				}
				else
				{
					ImageAnimator.rwImgListLock.ReleaseWriterLock();
				}
			}
		}

		// Token: 0x0600035F RID: 863 RVA: 0x00010078 File Offset: 0x0000E278
		private static void AnimateImages50ms()
		{
			for (;;)
			{
				ImageAnimator.rwImgListLock.AcquireReaderLock(-1);
				try
				{
					for (int i = 0; i < ImageAnimator.imageInfoList.Count; i++)
					{
						ImageAnimator.ImageInfo imageInfo = ImageAnimator.imageInfoList[i];
						imageInfo.FrameTimer += 5;
						if (imageInfo.FrameTimer >= imageInfo.FrameDelay(imageInfo.Frame))
						{
							imageInfo.FrameTimer = 0;
							if (imageInfo.Frame + 1 < imageInfo.FrameCount)
							{
								ImageAnimator.ImageInfo imageInfo2 = imageInfo;
								int frame = imageInfo2.Frame;
								imageInfo2.Frame = frame + 1;
							}
							else
							{
								imageInfo.Frame = 0;
							}
							if (imageInfo.FrameDirty)
							{
								ImageAnimator.anyFrameDirty = true;
							}
						}
					}
				}
				finally
				{
					ImageAnimator.rwImgListLock.ReleaseReaderLock();
				}
				Thread.Sleep(50);
			}
		}

		// Token: 0x04000191 RID: 401
		private static List<ImageAnimator.ImageInfo> imageInfoList;

		// Token: 0x04000192 RID: 402
		private static bool anyFrameDirty;

		// Token: 0x04000193 RID: 403
		private static Thread animationThread;

		// Token: 0x04000194 RID: 404
		private static ReaderWriterLock rwImgListLock = new ReaderWriterLock();

		// Token: 0x04000195 RID: 405
		[ThreadStatic]
		private static int threadWriterLockWaitCount;

		// Token: 0x020000FC RID: 252
		private class ImageInfo
		{
			// Token: 0x06000CAB RID: 3243 RVA: 0x0002C6FC File Offset: 0x0002A8FC
			public ImageInfo(Image image)
			{
				this.image = image;
				this.animated = ImageAnimator.CanAnimate(image);
				if (this.animated)
				{
					this.frameCount = image.GetFrameCount(FrameDimension.Time);
					PropertyItem propertyItem = image.GetPropertyItem(20736);
					if (propertyItem != null)
					{
						byte[] value = propertyItem.Value;
						this.frameDelay = new int[this.FrameCount];
						for (int i = 0; i < this.FrameCount; i++)
						{
							this.frameDelay[i] = (int)value[i * 4] + 256 * (int)value[i * 4 + 1] + 65536 * (int)value[i * 4 + 2] + 16777216 * (int)value[i * 4 + 3];
						}
					}
				}
				else
				{
					this.frameCount = 1;
				}
				if (this.frameDelay == null)
				{
					this.frameDelay = new int[this.FrameCount];
				}
			}

			// Token: 0x170003DD RID: 989
			// (get) Token: 0x06000CAC RID: 3244 RVA: 0x0002C7CF File Offset: 0x0002A9CF
			public bool Animated
			{
				get
				{
					return this.animated;
				}
			}

			// Token: 0x170003DE RID: 990
			// (get) Token: 0x06000CAD RID: 3245 RVA: 0x0002C7D7 File Offset: 0x0002A9D7
			// (set) Token: 0x06000CAE RID: 3246 RVA: 0x0002C7E0 File Offset: 0x0002A9E0
			public int Frame
			{
				get
				{
					return this.frame;
				}
				set
				{
					if (this.frame != value)
					{
						if (value < 0 || value >= this.FrameCount)
						{
							throw new ArgumentException(SR.GetString("InvalidFrame"), "value");
						}
						if (this.Animated)
						{
							this.frame = value;
							this.frameDirty = true;
							this.OnFrameChanged(EventArgs.Empty);
						}
					}
				}
			}

			// Token: 0x170003DF RID: 991
			// (get) Token: 0x06000CAF RID: 3247 RVA: 0x0002C839 File Offset: 0x0002AA39
			public bool FrameDirty
			{
				get
				{
					return this.frameDirty;
				}
			}

			// Token: 0x170003E0 RID: 992
			// (get) Token: 0x06000CB0 RID: 3248 RVA: 0x0002C841 File Offset: 0x0002AA41
			// (set) Token: 0x06000CB1 RID: 3249 RVA: 0x0002C849 File Offset: 0x0002AA49
			public EventHandler FrameChangedHandler
			{
				get
				{
					return this.onFrameChangedHandler;
				}
				set
				{
					this.onFrameChangedHandler = value;
				}
			}

			// Token: 0x170003E1 RID: 993
			// (get) Token: 0x06000CB2 RID: 3250 RVA: 0x0002C852 File Offset: 0x0002AA52
			public int FrameCount
			{
				get
				{
					return this.frameCount;
				}
			}

			// Token: 0x06000CB3 RID: 3251 RVA: 0x0002C85A File Offset: 0x0002AA5A
			public int FrameDelay(int frame)
			{
				return this.frameDelay[frame];
			}

			// Token: 0x170003E2 RID: 994
			// (get) Token: 0x06000CB4 RID: 3252 RVA: 0x0002C864 File Offset: 0x0002AA64
			// (set) Token: 0x06000CB5 RID: 3253 RVA: 0x0002C86C File Offset: 0x0002AA6C
			internal int FrameTimer
			{
				get
				{
					return this.frameTimer;
				}
				set
				{
					this.frameTimer = value;
				}
			}

			// Token: 0x170003E3 RID: 995
			// (get) Token: 0x06000CB6 RID: 3254 RVA: 0x0002C875 File Offset: 0x0002AA75
			internal Image Image
			{
				get
				{
					return this.image;
				}
			}

			// Token: 0x06000CB7 RID: 3255 RVA: 0x0002C87D File Offset: 0x0002AA7D
			internal void UpdateFrame()
			{
				if (this.frameDirty)
				{
					this.image.SelectActiveFrame(FrameDimension.Time, this.Frame);
					this.frameDirty = false;
				}
			}

			// Token: 0x06000CB8 RID: 3256 RVA: 0x0002C8A5 File Offset: 0x0002AAA5
			protected void OnFrameChanged(EventArgs e)
			{
				if (this.onFrameChangedHandler != null)
				{
					this.onFrameChangedHandler(this.image, e);
				}
			}

			// Token: 0x04000AEF RID: 2799
			private const int PropertyTagFrameDelay = 20736;

			// Token: 0x04000AF0 RID: 2800
			private Image image;

			// Token: 0x04000AF1 RID: 2801
			private int frame;

			// Token: 0x04000AF2 RID: 2802
			private int frameCount;

			// Token: 0x04000AF3 RID: 2803
			private bool frameDirty;

			// Token: 0x04000AF4 RID: 2804
			private bool animated;

			// Token: 0x04000AF5 RID: 2805
			private EventHandler onFrameChangedHandler;

			// Token: 0x04000AF6 RID: 2806
			private int[] frameDelay;

			// Token: 0x04000AF7 RID: 2807
			private int frameTimer;
		}
	}
}
