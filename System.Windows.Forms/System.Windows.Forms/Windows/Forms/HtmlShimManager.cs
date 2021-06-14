using System;
using System.Collections.Generic;

namespace System.Windows.Forms
{
	// Token: 0x02000274 RID: 628
	internal sealed class HtmlShimManager : IDisposable
	{
		// Token: 0x060025E7 RID: 9703 RVA: 0x000027DB File Offset: 0x000009DB
		internal HtmlShimManager()
		{
		}

		// Token: 0x060025E8 RID: 9704 RVA: 0x000B4D2C File Offset: 0x000B2F2C
		public void AddDocumentShim(HtmlDocument doc)
		{
			HtmlDocument.HtmlDocumentShim htmlDocumentShim = null;
			if (this.htmlDocumentShims == null)
			{
				this.htmlDocumentShims = new Dictionary<HtmlDocument, HtmlDocument.HtmlDocumentShim>();
				htmlDocumentShim = new HtmlDocument.HtmlDocumentShim(doc);
				this.htmlDocumentShims[doc] = htmlDocumentShim;
			}
			else if (!this.htmlDocumentShims.ContainsKey(doc))
			{
				htmlDocumentShim = new HtmlDocument.HtmlDocumentShim(doc);
				this.htmlDocumentShims[doc] = htmlDocumentShim;
			}
			if (htmlDocumentShim != null)
			{
				this.OnShimAdded(htmlDocumentShim);
			}
		}

		// Token: 0x060025E9 RID: 9705 RVA: 0x000B4D90 File Offset: 0x000B2F90
		public void AddWindowShim(HtmlWindow window)
		{
			HtmlWindow.HtmlWindowShim htmlWindowShim = null;
			if (this.htmlWindowShims == null)
			{
				this.htmlWindowShims = new Dictionary<HtmlWindow, HtmlWindow.HtmlWindowShim>();
				htmlWindowShim = new HtmlWindow.HtmlWindowShim(window);
				this.htmlWindowShims[window] = htmlWindowShim;
			}
			else if (!this.htmlWindowShims.ContainsKey(window))
			{
				htmlWindowShim = new HtmlWindow.HtmlWindowShim(window);
				this.htmlWindowShims[window] = htmlWindowShim;
			}
			if (htmlWindowShim != null)
			{
				this.OnShimAdded(htmlWindowShim);
			}
		}

		// Token: 0x060025EA RID: 9706 RVA: 0x000B4DF4 File Offset: 0x000B2FF4
		public void AddElementShim(HtmlElement element)
		{
			HtmlElement.HtmlElementShim htmlElementShim = null;
			if (this.htmlElementShims == null)
			{
				this.htmlElementShims = new Dictionary<HtmlElement, HtmlElement.HtmlElementShim>();
				htmlElementShim = new HtmlElement.HtmlElementShim(element);
				this.htmlElementShims[element] = htmlElementShim;
			}
			else if (!this.htmlElementShims.ContainsKey(element))
			{
				htmlElementShim = new HtmlElement.HtmlElementShim(element);
				this.htmlElementShims[element] = htmlElementShim;
			}
			if (htmlElementShim != null)
			{
				this.OnShimAdded(htmlElementShim);
			}
		}

		// Token: 0x060025EB RID: 9707 RVA: 0x000B4E58 File Offset: 0x000B3058
		internal HtmlDocument.HtmlDocumentShim GetDocumentShim(HtmlDocument document)
		{
			if (this.htmlDocumentShims == null)
			{
				return null;
			}
			if (this.htmlDocumentShims.ContainsKey(document))
			{
				return this.htmlDocumentShims[document];
			}
			return null;
		}

		// Token: 0x060025EC RID: 9708 RVA: 0x000B4E80 File Offset: 0x000B3080
		internal HtmlElement.HtmlElementShim GetElementShim(HtmlElement element)
		{
			if (this.htmlElementShims == null)
			{
				return null;
			}
			if (this.htmlElementShims.ContainsKey(element))
			{
				return this.htmlElementShims[element];
			}
			return null;
		}

		// Token: 0x060025ED RID: 9709 RVA: 0x000B4EA8 File Offset: 0x000B30A8
		internal HtmlWindow.HtmlWindowShim GetWindowShim(HtmlWindow window)
		{
			if (this.htmlWindowShims == null)
			{
				return null;
			}
			if (this.htmlWindowShims.ContainsKey(window))
			{
				return this.htmlWindowShims[window];
			}
			return null;
		}

		// Token: 0x060025EE RID: 9710 RVA: 0x000B4ED0 File Offset: 0x000B30D0
		private void OnShimAdded(HtmlShim addedShim)
		{
			if (addedShim != null && !(addedShim is HtmlWindow.HtmlWindowShim))
			{
				this.AddWindowShim(new HtmlWindow(this, addedShim.AssociatedWindow));
			}
		}

		// Token: 0x060025EF RID: 9711 RVA: 0x000B4EF0 File Offset: 0x000B30F0
		internal void OnWindowUnloaded(HtmlWindow unloadedWindow)
		{
			if (unloadedWindow != null)
			{
				if (this.htmlDocumentShims != null)
				{
					HtmlDocument.HtmlDocumentShim[] array = new HtmlDocument.HtmlDocumentShim[this.htmlDocumentShims.Count];
					this.htmlDocumentShims.Values.CopyTo(array, 0);
					foreach (HtmlDocument.HtmlDocumentShim htmlDocumentShim in array)
					{
						if (htmlDocumentShim.AssociatedWindow == unloadedWindow.NativeHtmlWindow)
						{
							this.htmlDocumentShims.Remove(htmlDocumentShim.Document);
							htmlDocumentShim.Dispose();
						}
					}
				}
				if (this.htmlElementShims != null)
				{
					HtmlElement.HtmlElementShim[] array3 = new HtmlElement.HtmlElementShim[this.htmlElementShims.Count];
					this.htmlElementShims.Values.CopyTo(array3, 0);
					foreach (HtmlElement.HtmlElementShim htmlElementShim in array3)
					{
						if (htmlElementShim.AssociatedWindow == unloadedWindow.NativeHtmlWindow)
						{
							this.htmlElementShims.Remove(htmlElementShim.Element);
							htmlElementShim.Dispose();
						}
					}
				}
				if (this.htmlWindowShims != null && this.htmlWindowShims.ContainsKey(unloadedWindow))
				{
					HtmlWindow.HtmlWindowShim htmlWindowShim = this.htmlWindowShims[unloadedWindow];
					this.htmlWindowShims.Remove(unloadedWindow);
					htmlWindowShim.Dispose();
				}
			}
		}

		// Token: 0x060025F0 RID: 9712 RVA: 0x000B501A File Offset: 0x000B321A
		public void Dispose()
		{
			this.Dispose(true);
		}

		// Token: 0x060025F1 RID: 9713 RVA: 0x000B5024 File Offset: 0x000B3224
		private void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (this.htmlElementShims != null)
				{
					foreach (HtmlElement.HtmlElementShim htmlElementShim in this.htmlElementShims.Values)
					{
						htmlElementShim.Dispose();
					}
				}
				if (this.htmlDocumentShims != null)
				{
					foreach (HtmlDocument.HtmlDocumentShim htmlDocumentShim in this.htmlDocumentShims.Values)
					{
						htmlDocumentShim.Dispose();
					}
				}
				if (this.htmlWindowShims != null)
				{
					foreach (HtmlWindow.HtmlWindowShim htmlWindowShim in this.htmlWindowShims.Values)
					{
						htmlWindowShim.Dispose();
					}
				}
				this.htmlWindowShims = null;
				this.htmlDocumentShims = null;
				this.htmlWindowShims = null;
			}
		}

		// Token: 0x060025F2 RID: 9714 RVA: 0x000B5140 File Offset: 0x000B3340
		~HtmlShimManager()
		{
			this.Dispose(false);
		}

		// Token: 0x04001025 RID: 4133
		private Dictionary<HtmlWindow, HtmlWindow.HtmlWindowShim> htmlWindowShims;

		// Token: 0x04001026 RID: 4134
		private Dictionary<HtmlElement, HtmlElement.HtmlElementShim> htmlElementShims;

		// Token: 0x04001027 RID: 4135
		private Dictionary<HtmlDocument, HtmlDocument.HtmlDocumentShim> htmlDocumentShims;
	}
}
