using System;

namespace System.Windows.Forms
{
	// Token: 0x02000263 RID: 611
	internal class HelpInfo
	{
		// Token: 0x060024C0 RID: 9408 RVA: 0x000B24FC File Offset: 0x000B06FC
		public HelpInfo(string helpfilepath)
		{
			this.helpFilePath = helpfilepath;
			this.keyword = "";
			this.navigator = HelpNavigator.TableOfContents;
			this.param = null;
			this.option = 1;
		}

		// Token: 0x060024C1 RID: 9409 RVA: 0x000B252F File Offset: 0x000B072F
		public HelpInfo(string helpfilepath, string keyword)
		{
			this.helpFilePath = helpfilepath;
			this.keyword = keyword;
			this.navigator = HelpNavigator.TableOfContents;
			this.param = null;
			this.option = 2;
		}

		// Token: 0x060024C2 RID: 9410 RVA: 0x000B255E File Offset: 0x000B075E
		public HelpInfo(string helpfilepath, HelpNavigator navigator)
		{
			this.helpFilePath = helpfilepath;
			this.keyword = "";
			this.navigator = navigator;
			this.param = null;
			this.option = 3;
		}

		// Token: 0x060024C3 RID: 9411 RVA: 0x000B258D File Offset: 0x000B078D
		public HelpInfo(string helpfilepath, HelpNavigator navigator, object param)
		{
			this.helpFilePath = helpfilepath;
			this.keyword = "";
			this.navigator = navigator;
			this.param = param;
			this.option = 4;
		}

		// Token: 0x170008D1 RID: 2257
		// (get) Token: 0x060024C4 RID: 9412 RVA: 0x000B25BC File Offset: 0x000B07BC
		public int Option
		{
			get
			{
				return this.option;
			}
		}

		// Token: 0x170008D2 RID: 2258
		// (get) Token: 0x060024C5 RID: 9413 RVA: 0x000B25C4 File Offset: 0x000B07C4
		public string HelpFilePath
		{
			get
			{
				return this.helpFilePath;
			}
		}

		// Token: 0x170008D3 RID: 2259
		// (get) Token: 0x060024C6 RID: 9414 RVA: 0x000B25CC File Offset: 0x000B07CC
		public string Keyword
		{
			get
			{
				return this.keyword;
			}
		}

		// Token: 0x170008D4 RID: 2260
		// (get) Token: 0x060024C7 RID: 9415 RVA: 0x000B25D4 File Offset: 0x000B07D4
		public HelpNavigator Navigator
		{
			get
			{
				return this.navigator;
			}
		}

		// Token: 0x170008D5 RID: 2261
		// (get) Token: 0x060024C8 RID: 9416 RVA: 0x000B25DC File Offset: 0x000B07DC
		public object Param
		{
			get
			{
				return this.param;
			}
		}

		// Token: 0x060024C9 RID: 9417 RVA: 0x000B25E4 File Offset: 0x000B07E4
		public override string ToString()
		{
			return string.Concat(new string[]
			{
				"{HelpFilePath=",
				this.helpFilePath,
				", keyword =",
				this.keyword,
				", navigator=",
				this.navigator.ToString(),
				"}"
			});
		}

		// Token: 0x04000FD3 RID: 4051
		private string helpFilePath;

		// Token: 0x04000FD4 RID: 4052
		private string keyword;

		// Token: 0x04000FD5 RID: 4053
		private HelpNavigator navigator;

		// Token: 0x04000FD6 RID: 4054
		private object param;

		// Token: 0x04000FD7 RID: 4055
		private int option;
	}
}
