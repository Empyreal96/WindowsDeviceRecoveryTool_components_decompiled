using System;
using System.Windows;
using System.Windows.Controls.Primitives;
using MS.Internal.KnownBoxes;

namespace MS.Internal.Documents
{
	// Token: 0x020006D1 RID: 1745
	internal class ReaderTwoPageViewer : ReaderPageViewer
	{
		// Token: 0x060070DF RID: 28895 RVA: 0x00204CF3 File Offset: 0x00202EF3
		protected override void OnPreviousPageCommand()
		{
			base.GoToPage(Math.Max(1, this.MasterPageNumber - 2));
		}

		// Token: 0x060070E0 RID: 28896 RVA: 0x00204D09 File Offset: 0x00202F09
		protected override void OnNextPageCommand()
		{
			base.GoToPage(Math.Min(base.PageCount, this.MasterPageNumber + 2));
		}

		// Token: 0x060070E1 RID: 28897 RVA: 0x00204D24 File Offset: 0x00202F24
		protected override void OnLastPageCommand()
		{
			base.GoToPage(base.PageCount);
		}

		// Token: 0x060070E2 RID: 28898 RVA: 0x00204D32 File Offset: 0x00202F32
		protected override void OnGoToPageCommand(int pageNumber)
		{
			base.OnGoToPageCommand((pageNumber - 1) / 2 * 2 + 1);
		}

		// Token: 0x060070E3 RID: 28899 RVA: 0x00204D44 File Offset: 0x00202F44
		protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
		{
			base.OnPropertyChanged(e);
			if (e.Property == DocumentViewerBase.MasterPageNumberProperty)
			{
				int num = (int)e.NewValue;
				num = (num - 1) / 2 * 2 + 1;
				if (num != (int)e.NewValue)
				{
					base.GoToPage(num);
				}
			}
		}

		// Token: 0x060070E4 RID: 28900 RVA: 0x00204D93 File Offset: 0x00202F93
		static ReaderTwoPageViewer()
		{
			DocumentViewerBase.CanGoToNextPagePropertyKey.OverrideMetadata(typeof(ReaderTwoPageViewer), new FrameworkPropertyMetadata(BooleanBoxes.FalseBox, null, new CoerceValueCallback(ReaderTwoPageViewer.CoerceCanGoToNextPage)));
		}

		// Token: 0x060070E5 RID: 28901 RVA: 0x00204DC0 File Offset: 0x00202FC0
		private static object CoerceCanGoToNextPage(DependencyObject d, object value)
		{
			Invariant.Assert(d != null && d is ReaderTwoPageViewer);
			ReaderTwoPageViewer readerTwoPageViewer = (ReaderTwoPageViewer)d;
			return readerTwoPageViewer.MasterPageNumber < readerTwoPageViewer.PageCount - 1;
		}
	}
}
