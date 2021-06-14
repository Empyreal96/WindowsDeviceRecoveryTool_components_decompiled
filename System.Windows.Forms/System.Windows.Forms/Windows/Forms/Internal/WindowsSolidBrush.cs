using System;
using System.Drawing;
using System.Globalization;

namespace System.Windows.Forms.Internal
{
	// Token: 0x02000500 RID: 1280
	internal sealed class WindowsSolidBrush : WindowsBrush
	{
		// Token: 0x06005419 RID: 21529 RVA: 0x0015F248 File Offset: 0x0015D448
		protected override void CreateBrush()
		{
			IntPtr intPtr = IntSafeNativeMethods.CreateSolidBrush(ColorTranslator.ToWin32(base.Color));
			intPtr == IntPtr.Zero;
			base.NativeHandle = intPtr;
		}

		// Token: 0x0600541A RID: 21530 RVA: 0x0015F279 File Offset: 0x0015D479
		public WindowsSolidBrush(DeviceContext dc) : base(dc)
		{
		}

		// Token: 0x0600541B RID: 21531 RVA: 0x0015F282 File Offset: 0x0015D482
		public WindowsSolidBrush(DeviceContext dc, Color color) : base(dc, color)
		{
		}

		// Token: 0x0600541C RID: 21532 RVA: 0x0015F28C File Offset: 0x0015D48C
		public override object Clone()
		{
			return new WindowsSolidBrush(base.DC, base.Color);
		}

		// Token: 0x0600541D RID: 21533 RVA: 0x0015F29F File Offset: 0x0015D49F
		public override string ToString()
		{
			return string.Format(CultureInfo.InvariantCulture, "{0}: Color={1}", new object[]
			{
				base.GetType().Name,
				base.Color
			});
		}
	}
}
