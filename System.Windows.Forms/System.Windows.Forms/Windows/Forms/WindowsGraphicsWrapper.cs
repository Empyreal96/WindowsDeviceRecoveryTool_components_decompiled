using System;
using System.Drawing;
using System.Windows.Forms.Internal;

namespace System.Windows.Forms
{
	// Token: 0x02000442 RID: 1090
	internal sealed class WindowsGraphicsWrapper : IDisposable
	{
		// Token: 0x06004C9E RID: 19614 RVA: 0x0013A88C File Offset: 0x00138A8C
		public WindowsGraphicsWrapper(IDeviceContext idc, TextFormatFlags flags)
		{
			if (idc is Graphics)
			{
				ApplyGraphicsProperties applyGraphicsProperties = ApplyGraphicsProperties.None;
				if ((flags & TextFormatFlags.PreserveGraphicsClipping) != TextFormatFlags.Default)
				{
					applyGraphicsProperties |= ApplyGraphicsProperties.Clipping;
				}
				if ((flags & TextFormatFlags.PreserveGraphicsTranslateTransform) != TextFormatFlags.Default)
				{
					applyGraphicsProperties |= ApplyGraphicsProperties.TranslateTransform;
				}
				if (applyGraphicsProperties != ApplyGraphicsProperties.None)
				{
					this.wg = WindowsGraphics.FromGraphics(idc as Graphics, applyGraphicsProperties);
				}
			}
			else
			{
				this.wg = (idc as WindowsGraphics);
				if (this.wg != null)
				{
					this.idc = idc;
				}
			}
			if (this.wg == null)
			{
				this.idc = idc;
				this.wg = WindowsGraphics.FromHdc(idc.GetHdc());
			}
			if ((flags & TextFormatFlags.LeftAndRightPadding) != TextFormatFlags.Default)
			{
				this.wg.TextPadding = TextPaddingOptions.LeftAndRightPadding;
				return;
			}
			if ((flags & TextFormatFlags.NoPadding) != TextFormatFlags.Default)
			{
				this.wg.TextPadding = TextPaddingOptions.NoPadding;
			}
		}

		// Token: 0x170012D3 RID: 4819
		// (get) Token: 0x06004C9F RID: 19615 RVA: 0x0013A940 File Offset: 0x00138B40
		public WindowsGraphics WindowsGraphics
		{
			get
			{
				return this.wg;
			}
		}

		// Token: 0x06004CA0 RID: 19616 RVA: 0x0013A948 File Offset: 0x00138B48
		~WindowsGraphicsWrapper()
		{
			this.Dispose(false);
		}

		// Token: 0x06004CA1 RID: 19617 RVA: 0x0013A978 File Offset: 0x00138B78
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06004CA2 RID: 19618 RVA: 0x0013A988 File Offset: 0x00138B88
		public void Dispose(bool disposing)
		{
			if (this.wg != null)
			{
				if (this.wg != this.idc)
				{
					this.wg.Dispose();
					if (this.idc != null)
					{
						this.idc.ReleaseHdc();
					}
				}
				this.idc = null;
				this.wg = null;
			}
		}

		// Token: 0x040027EB RID: 10219
		private IDeviceContext idc;

		// Token: 0x040027EC RID: 10220
		private WindowsGraphics wg;
	}
}
