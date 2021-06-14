using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using MS.Internal.PtsHost.UnsafeNativeMethods;

namespace MS.Internal.PtsHost
{
	// Token: 0x02000647 RID: 1607
	internal sealed class StructuralCache
	{
		// Token: 0x06006A5A RID: 27226 RVA: 0x001E45D4 File Offset: 0x001E27D4
		internal StructuralCache(FlowDocument owner, TextContainer textContainer)
		{
			Invariant.Assert(owner != null);
			Invariant.Assert(textContainer != null);
			Invariant.Assert(textContainer.Parent != null);
			this._owner = owner;
			this._textContainer = textContainer;
			this._backgroundFormatInfo = new BackgroundFormatInfo(this);
		}

		// Token: 0x06006A5B RID: 27227 RVA: 0x001E4624 File Offset: 0x001E2824
		~StructuralCache()
		{
			if (this._ptsContext != null)
			{
				PtsCache.ReleaseContext(this._ptsContext);
			}
		}

		// Token: 0x06006A5C RID: 27228 RVA: 0x001E4660 File Offset: 0x001E2860
		internal IDisposable SetDocumentFormatContext(FlowDocumentPage currentPage)
		{
			if (!this.CheckFlags(StructuralCache.Flags.FormattedOnce))
			{
				this.SetFlags(true, StructuralCache.Flags.FormattedOnce);
				this._owner.InitializeForFirstFormatting();
			}
			return new StructuralCache.DocumentFormatContext(this, currentPage);
		}

		// Token: 0x06006A5D RID: 27229 RVA: 0x001E4685 File Offset: 0x001E2885
		internal IDisposable SetDocumentArrangeContext(FlowDocumentPage currentPage)
		{
			return new StructuralCache.DocumentArrangeContext(this, currentPage);
		}

		// Token: 0x06006A5E RID: 27230 RVA: 0x001E468E File Offset: 0x001E288E
		internal IDisposable SetDocumentVisualValidationContext(FlowDocumentPage currentPage)
		{
			return new StructuralCache.DocumentVisualValidationContext(this, currentPage);
		}

		// Token: 0x06006A5F RID: 27231 RVA: 0x001E4697 File Offset: 0x001E2897
		internal void DetectInvalidOperation()
		{
			if (this._illegalTreeChangeDetected)
			{
				throw new InvalidOperationException(SR.Get("IllegalTreeChangeDetectedPostAction"));
			}
		}

		// Token: 0x06006A60 RID: 27232 RVA: 0x001E46B1 File Offset: 0x001E28B1
		internal void OnInvalidOperationDetected()
		{
			if (this._currentPage != null)
			{
				this._illegalTreeChangeDetected = true;
			}
		}

		// Token: 0x06006A61 RID: 27233 RVA: 0x001E46C2 File Offset: 0x001E28C2
		internal void InvalidateFormatCache(bool destroyStructure)
		{
			if (this._section != null)
			{
				this._section.InvalidateFormatCache();
				this._destroyStructure = (this._destroyStructure || destroyStructure);
				this._forceReformat = true;
			}
		}

		// Token: 0x06006A62 RID: 27234 RVA: 0x001E46EC File Offset: 0x001E28EC
		internal void AddDirtyTextRange(DirtyTextRange dtr)
		{
			if (this._dtrs == null)
			{
				this._dtrs = new DtrList();
			}
			this._dtrs.Merge(dtr);
		}

		// Token: 0x06006A63 RID: 27235 RVA: 0x001E470D File Offset: 0x001E290D
		internal DtrList DtrsFromRange(int dcpNew, int cchOld)
		{
			if (this._dtrs == null)
			{
				return null;
			}
			return this._dtrs.DtrsFromRange(dcpNew, cchOld);
		}

		// Token: 0x06006A64 RID: 27236 RVA: 0x001E4728 File Offset: 0x001E2928
		internal void ClearUpdateInfo(bool destroyStructureCache)
		{
			this._dtrs = null;
			this._forceReformat = false;
			this._destroyStructure = false;
			if (this._section != null && !this._ptsContext.Disposed)
			{
				if (destroyStructureCache)
				{
					this._section.DestroyStructure();
				}
				this._section.ClearUpdateInfo();
			}
		}

		// Token: 0x06006A65 RID: 27237 RVA: 0x001E4778 File Offset: 0x001E2978
		internal void ThrottleBackgroundFormatting()
		{
			this._backgroundFormatInfo.ThrottleBackgroundFormatting();
		}

		// Token: 0x06006A66 RID: 27238 RVA: 0x001E4785 File Offset: 0x001E2985
		internal bool HasPtsContext()
		{
			return this._ptsContext != null;
		}

		// Token: 0x1700198E RID: 6542
		// (get) Token: 0x06006A67 RID: 27239 RVA: 0x001E4790 File Offset: 0x001E2990
		internal DependencyObject PropertyOwner
		{
			get
			{
				return this._textContainer.Parent;
			}
		}

		// Token: 0x1700198F RID: 6543
		// (get) Token: 0x06006A68 RID: 27240 RVA: 0x001E479D File Offset: 0x001E299D
		internal FlowDocument FormattingOwner
		{
			get
			{
				return this._owner;
			}
		}

		// Token: 0x17001990 RID: 6544
		// (get) Token: 0x06006A69 RID: 27241 RVA: 0x001E47A5 File Offset: 0x001E29A5
		internal Section Section
		{
			get
			{
				this.EnsurePtsContext();
				return this._section;
			}
		}

		// Token: 0x17001991 RID: 6545
		// (get) Token: 0x06006A6A RID: 27242 RVA: 0x001E47B3 File Offset: 0x001E29B3
		internal NaturalLanguageHyphenator Hyphenator
		{
			get
			{
				this.EnsureHyphenator();
				return this._hyphenator;
			}
		}

		// Token: 0x17001992 RID: 6546
		// (get) Token: 0x06006A6B RID: 27243 RVA: 0x001E47C1 File Offset: 0x001E29C1
		internal PtsContext PtsContext
		{
			get
			{
				this.EnsurePtsContext();
				return this._ptsContext;
			}
		}

		// Token: 0x17001993 RID: 6547
		// (get) Token: 0x06006A6C RID: 27244 RVA: 0x001E47CF File Offset: 0x001E29CF
		internal StructuralCache.DocumentFormatContext CurrentFormatContext
		{
			get
			{
				return this._documentFormatContext;
			}
		}

		// Token: 0x17001994 RID: 6548
		// (get) Token: 0x06006A6D RID: 27245 RVA: 0x001E47D7 File Offset: 0x001E29D7
		internal StructuralCache.DocumentArrangeContext CurrentArrangeContext
		{
			get
			{
				return this._documentArrangeContext;
			}
		}

		// Token: 0x17001995 RID: 6549
		// (get) Token: 0x06006A6E RID: 27246 RVA: 0x001E47DF File Offset: 0x001E29DF
		internal TextFormatterHost TextFormatterHost
		{
			get
			{
				this.EnsurePtsContext();
				return this._textFormatterHost;
			}
		}

		// Token: 0x17001996 RID: 6550
		// (get) Token: 0x06006A6F RID: 27247 RVA: 0x001E47ED File Offset: 0x001E29ED
		internal TextContainer TextContainer
		{
			get
			{
				return this._textContainer;
			}
		}

		// Token: 0x17001997 RID: 6551
		// (get) Token: 0x06006A70 RID: 27248 RVA: 0x001E47F5 File Offset: 0x001E29F5
		// (set) Token: 0x06006A71 RID: 27249 RVA: 0x001E47FD File Offset: 0x001E29FD
		internal FlowDirection PageFlowDirection
		{
			get
			{
				return this._pageFlowDirection;
			}
			set
			{
				this._pageFlowDirection = value;
			}
		}

		// Token: 0x17001998 RID: 6552
		// (get) Token: 0x06006A72 RID: 27250 RVA: 0x001E4806 File Offset: 0x001E2A06
		// (set) Token: 0x06006A73 RID: 27251 RVA: 0x001E480E File Offset: 0x001E2A0E
		internal bool ForceReformat
		{
			get
			{
				return this._forceReformat;
			}
			set
			{
				this._forceReformat = value;
			}
		}

		// Token: 0x17001999 RID: 6553
		// (get) Token: 0x06006A74 RID: 27252 RVA: 0x001E4817 File Offset: 0x001E2A17
		internal bool DestroyStructure
		{
			get
			{
				return this._destroyStructure;
			}
		}

		// Token: 0x1700199A RID: 6554
		// (get) Token: 0x06006A75 RID: 27253 RVA: 0x001E481F File Offset: 0x001E2A1F
		internal DtrList DtrList
		{
			get
			{
				return this._dtrs;
			}
		}

		// Token: 0x1700199B RID: 6555
		// (get) Token: 0x06006A76 RID: 27254 RVA: 0x001E4827 File Offset: 0x001E2A27
		internal bool IsDeferredVisualCreationSupported
		{
			get
			{
				return this._currentPage != null && !this._currentPage.FinitePage;
			}
		}

		// Token: 0x1700199C RID: 6556
		// (get) Token: 0x06006A77 RID: 27255 RVA: 0x001E4841 File Offset: 0x001E2A41
		internal BackgroundFormatInfo BackgroundFormatInfo
		{
			get
			{
				return this._backgroundFormatInfo;
			}
		}

		// Token: 0x1700199D RID: 6557
		// (get) Token: 0x06006A78 RID: 27256 RVA: 0x001E4849 File Offset: 0x001E2A49
		internal bool IsOptimalParagraphEnabled
		{
			get
			{
				return this.PtsContext.IsOptimalParagraphEnabled && (bool)this.PropertyOwner.GetValue(FlowDocument.IsOptimalParagraphEnabledProperty);
			}
		}

		// Token: 0x1700199E RID: 6558
		// (get) Token: 0x06006A79 RID: 27257 RVA: 0x001E486F File Offset: 0x001E2A6F
		// (set) Token: 0x06006A7A RID: 27258 RVA: 0x001E4878 File Offset: 0x001E2A78
		internal bool IsFormattingInProgress
		{
			get
			{
				return this.CheckFlags(StructuralCache.Flags.FormattingInProgress);
			}
			set
			{
				this.SetFlags(value, StructuralCache.Flags.FormattingInProgress);
			}
		}

		// Token: 0x1700199F RID: 6559
		// (get) Token: 0x06006A7B RID: 27259 RVA: 0x001E4882 File Offset: 0x001E2A82
		// (set) Token: 0x06006A7C RID: 27260 RVA: 0x001E488B File Offset: 0x001E2A8B
		internal bool IsContentChangeInProgress
		{
			get
			{
				return this.CheckFlags(StructuralCache.Flags.ContentChangeInProgress);
			}
			set
			{
				this.SetFlags(value, StructuralCache.Flags.ContentChangeInProgress);
			}
		}

		// Token: 0x170019A0 RID: 6560
		// (get) Token: 0x06006A7D RID: 27261 RVA: 0x001E4895 File Offset: 0x001E2A95
		// (set) Token: 0x06006A7E RID: 27262 RVA: 0x001E489E File Offset: 0x001E2A9E
		internal bool IsFormattedOnce
		{
			get
			{
				return this.CheckFlags(StructuralCache.Flags.FormattedOnce);
			}
			set
			{
				this.SetFlags(value, StructuralCache.Flags.FormattedOnce);
			}
		}

		// Token: 0x06006A7F RID: 27263 RVA: 0x001E48A8 File Offset: 0x001E2AA8
		private void EnsureHyphenator()
		{
			if (this._hyphenator == null)
			{
				this._hyphenator = new NaturalLanguageHyphenator();
			}
		}

		// Token: 0x06006A80 RID: 27264 RVA: 0x001E48C0 File Offset: 0x001E2AC0
		private void EnsurePtsContext()
		{
			if (this._ptsContext == null)
			{
				TextFormattingMode textFormattingMode = TextOptions.GetTextFormattingMode(this.PropertyOwner);
				this._ptsContext = new PtsContext(true, textFormattingMode);
				this._textFormatterHost = new TextFormatterHost(this._ptsContext.TextFormatter, textFormattingMode, this._owner.PixelsPerDip);
				this._section = new Section(this);
			}
		}

		// Token: 0x06006A81 RID: 27265 RVA: 0x001E491C File Offset: 0x001E2B1C
		private void SetFlags(bool value, StructuralCache.Flags flags)
		{
			this._flags = (value ? (this._flags | flags) : (this._flags & ~flags));
		}

		// Token: 0x06006A82 RID: 27266 RVA: 0x001E493A File Offset: 0x001E2B3A
		private bool CheckFlags(StructuralCache.Flags flags)
		{
			return (this._flags & flags) == flags;
		}

		// Token: 0x04003438 RID: 13368
		private readonly FlowDocument _owner;

		// Token: 0x04003439 RID: 13369
		private PtsContext _ptsContext;

		// Token: 0x0400343A RID: 13370
		private Section _section;

		// Token: 0x0400343B RID: 13371
		private TextContainer _textContainer;

		// Token: 0x0400343C RID: 13372
		private TextFormatterHost _textFormatterHost;

		// Token: 0x0400343D RID: 13373
		private FlowDocumentPage _currentPage;

		// Token: 0x0400343E RID: 13374
		private StructuralCache.DocumentFormatContext _documentFormatContext;

		// Token: 0x0400343F RID: 13375
		private StructuralCache.DocumentArrangeContext _documentArrangeContext;

		// Token: 0x04003440 RID: 13376
		private DtrList _dtrs;

		// Token: 0x04003441 RID: 13377
		private bool _illegalTreeChangeDetected;

		// Token: 0x04003442 RID: 13378
		private bool _forceReformat;

		// Token: 0x04003443 RID: 13379
		private bool _destroyStructure;

		// Token: 0x04003444 RID: 13380
		private BackgroundFormatInfo _backgroundFormatInfo;

		// Token: 0x04003445 RID: 13381
		private FlowDirection _pageFlowDirection;

		// Token: 0x04003446 RID: 13382
		private NaturalLanguageHyphenator _hyphenator;

		// Token: 0x04003447 RID: 13383
		private StructuralCache.Flags _flags;

		// Token: 0x02000A23 RID: 2595
		[Flags]
		private enum Flags
		{
			// Token: 0x04004713 RID: 18195
			FormattedOnce = 1,
			// Token: 0x04004714 RID: 18196
			ContentChangeInProgress = 2,
			// Token: 0x04004715 RID: 18197
			FormattingInProgress = 8
		}

		// Token: 0x02000A24 RID: 2596
		internal abstract class DocumentOperationContext
		{
			// Token: 0x06008ABB RID: 35515 RVA: 0x002579FC File Offset: 0x00255BFC
			internal DocumentOperationContext(StructuralCache owner, FlowDocumentPage page)
			{
				Invariant.Assert(owner != null, "Invalid owner object.");
				Invariant.Assert(page != null, "Invalid page object.");
				Invariant.Assert(owner._currentPage == null, "Page formatting reentrancy detected. Trying to create second _DocumentPageContext for the same StructuralCache.");
				this._owner = owner;
				this._owner._currentPage = page;
				this._owner._illegalTreeChangeDetected = false;
				owner.PtsContext.Enter();
			}

			// Token: 0x06008ABC RID: 35516 RVA: 0x00257A68 File Offset: 0x00255C68
			protected void Dispose()
			{
				Invariant.Assert(this._owner._currentPage != null, "DocumentPageContext is already disposed.");
				try
				{
					this._owner.PtsContext.Leave();
				}
				finally
				{
					this._owner._currentPage = null;
				}
			}

			// Token: 0x17001F59 RID: 8025
			// (get) Token: 0x06008ABD RID: 35517 RVA: 0x00257ABC File Offset: 0x00255CBC
			internal Size DocumentPageSize
			{
				get
				{
					return this._owner._currentPage.Size;
				}
			}

			// Token: 0x17001F5A RID: 8026
			// (get) Token: 0x06008ABE RID: 35518 RVA: 0x00257ACE File Offset: 0x00255CCE
			internal Thickness DocumentPageMargin
			{
				get
				{
					return this._owner._currentPage.Margin;
				}
			}

			// Token: 0x04004716 RID: 18198
			protected readonly StructuralCache _owner;
		}

		// Token: 0x02000A25 RID: 2597
		internal class DocumentFormatContext : StructuralCache.DocumentOperationContext, IDisposable
		{
			// Token: 0x06008ABF RID: 35519 RVA: 0x00257AE0 File Offset: 0x00255CE0
			internal DocumentFormatContext(StructuralCache owner, FlowDocumentPage page) : base(owner, page)
			{
				this._owner._documentFormatContext = this;
			}

			// Token: 0x06008AC0 RID: 35520 RVA: 0x00257B01 File Offset: 0x00255D01
			void IDisposable.Dispose()
			{
				this._owner._documentFormatContext = null;
				base.Dispose();
				GC.SuppressFinalize(this);
			}

			// Token: 0x06008AC1 RID: 35521 RVA: 0x00257B1B File Offset: 0x00255D1B
			internal void OnFormatLine()
			{
				this._owner._currentPage.OnFormatLine();
			}

			// Token: 0x06008AC2 RID: 35522 RVA: 0x00257B30 File Offset: 0x00255D30
			internal void PushNewPageData(Size pageSize, Thickness pageMargin, bool incrementalUpdate, bool finitePage)
			{
				this._documentFormatInfoStack.Push(this._currentFormatInfo);
				this._currentFormatInfo.PageSize = pageSize;
				this._currentFormatInfo.PageMargin = pageMargin;
				this._currentFormatInfo.IncrementalUpdate = incrementalUpdate;
				this._currentFormatInfo.FinitePage = finitePage;
			}

			// Token: 0x06008AC3 RID: 35523 RVA: 0x00257B7F File Offset: 0x00255D7F
			internal void PopPageData()
			{
				this._currentFormatInfo = this._documentFormatInfoStack.Pop();
			}

			// Token: 0x17001F5B RID: 8027
			// (get) Token: 0x06008AC4 RID: 35524 RVA: 0x00257B92 File Offset: 0x00255D92
			internal double PageHeight
			{
				get
				{
					return this._currentFormatInfo.PageSize.Height;
				}
			}

			// Token: 0x17001F5C RID: 8028
			// (get) Token: 0x06008AC5 RID: 35525 RVA: 0x00257BA4 File Offset: 0x00255DA4
			internal double PageWidth
			{
				get
				{
					return this._currentFormatInfo.PageSize.Width;
				}
			}

			// Token: 0x17001F5D RID: 8029
			// (get) Token: 0x06008AC6 RID: 35526 RVA: 0x00257BB6 File Offset: 0x00255DB6
			internal Size PageSize
			{
				get
				{
					return this._currentFormatInfo.PageSize;
				}
			}

			// Token: 0x17001F5E RID: 8030
			// (get) Token: 0x06008AC7 RID: 35527 RVA: 0x00257BC3 File Offset: 0x00255DC3
			internal Thickness PageMargin
			{
				get
				{
					return this._currentFormatInfo.PageMargin;
				}
			}

			// Token: 0x17001F5F RID: 8031
			// (get) Token: 0x06008AC8 RID: 35528 RVA: 0x00257BD0 File Offset: 0x00255DD0
			internal bool IncrementalUpdate
			{
				get
				{
					return this._currentFormatInfo.IncrementalUpdate;
				}
			}

			// Token: 0x17001F60 RID: 8032
			// (get) Token: 0x06008AC9 RID: 35529 RVA: 0x00257BDD File Offset: 0x00255DDD
			internal bool FinitePage
			{
				get
				{
					return this._currentFormatInfo.FinitePage;
				}
			}

			// Token: 0x17001F61 RID: 8033
			// (get) Token: 0x06008ACA RID: 35530 RVA: 0x00257BEA File Offset: 0x00255DEA
			internal PTS.FSRECT PageRect
			{
				get
				{
					return new PTS.FSRECT(new Rect(0.0, 0.0, this.PageWidth, this.PageHeight));
				}
			}

			// Token: 0x17001F62 RID: 8034
			// (get) Token: 0x06008ACB RID: 35531 RVA: 0x00257C14 File Offset: 0x00255E14
			internal PTS.FSRECT PageMarginRect
			{
				get
				{
					return new PTS.FSRECT(new Rect(this.PageMargin.Left, this.PageMargin.Top, this.PageSize.Width - this.PageMargin.Left - this.PageMargin.Right, this.PageSize.Height - this.PageMargin.Top - this.PageMargin.Bottom));
				}
			}

			// Token: 0x17001F63 RID: 8035
			// (set) Token: 0x06008ACC RID: 35532 RVA: 0x00257C9F File Offset: 0x00255E9F
			internal TextPointer DependentMax
			{
				set
				{
					this._owner._currentPage.DependentMax = value;
				}
			}

			// Token: 0x04004717 RID: 18199
			private StructuralCache.DocumentFormatContext.DocumentFormatInfo _currentFormatInfo;

			// Token: 0x04004718 RID: 18200
			private Stack<StructuralCache.DocumentFormatContext.DocumentFormatInfo> _documentFormatInfoStack = new Stack<StructuralCache.DocumentFormatContext.DocumentFormatInfo>();

			// Token: 0x02000BB6 RID: 2998
			private struct DocumentFormatInfo
			{
				// Token: 0x04004EF3 RID: 20211
				internal Size PageSize;

				// Token: 0x04004EF4 RID: 20212
				internal Thickness PageMargin;

				// Token: 0x04004EF5 RID: 20213
				internal bool IncrementalUpdate;

				// Token: 0x04004EF6 RID: 20214
				internal bool FinitePage;
			}
		}

		// Token: 0x02000A26 RID: 2598
		internal class DocumentArrangeContext : StructuralCache.DocumentOperationContext, IDisposable
		{
			// Token: 0x06008ACD RID: 35533 RVA: 0x00257CB2 File Offset: 0x00255EB2
			internal DocumentArrangeContext(StructuralCache owner, FlowDocumentPage page) : base(owner, page)
			{
				this._owner._documentArrangeContext = this;
			}

			// Token: 0x06008ACE RID: 35534 RVA: 0x00257CD3 File Offset: 0x00255ED3
			internal void PushNewPageData(PageContext pageContext, PTS.FSRECT columnRect, bool finitePage)
			{
				this._documentArrangeInfoStack.Push(this._currentArrangeInfo);
				this._currentArrangeInfo.PageContext = pageContext;
				this._currentArrangeInfo.ColumnRect = columnRect;
				this._currentArrangeInfo.FinitePage = finitePage;
			}

			// Token: 0x06008ACF RID: 35535 RVA: 0x00257D0A File Offset: 0x00255F0A
			internal void PopPageData()
			{
				this._currentArrangeInfo = this._documentArrangeInfoStack.Pop();
			}

			// Token: 0x06008AD0 RID: 35536 RVA: 0x00257D1D File Offset: 0x00255F1D
			void IDisposable.Dispose()
			{
				GC.SuppressFinalize(this);
				this._owner._documentArrangeContext = null;
				base.Dispose();
			}

			// Token: 0x17001F64 RID: 8036
			// (get) Token: 0x06008AD1 RID: 35537 RVA: 0x00257D37 File Offset: 0x00255F37
			internal PageContext PageContext
			{
				get
				{
					return this._currentArrangeInfo.PageContext;
				}
			}

			// Token: 0x17001F65 RID: 8037
			// (get) Token: 0x06008AD2 RID: 35538 RVA: 0x00257D44 File Offset: 0x00255F44
			internal PTS.FSRECT ColumnRect
			{
				get
				{
					return this._currentArrangeInfo.ColumnRect;
				}
			}

			// Token: 0x17001F66 RID: 8038
			// (get) Token: 0x06008AD3 RID: 35539 RVA: 0x00257D51 File Offset: 0x00255F51
			internal bool FinitePage
			{
				get
				{
					return this._currentArrangeInfo.FinitePage;
				}
			}

			// Token: 0x04004719 RID: 18201
			private StructuralCache.DocumentArrangeContext.DocumentArrangeInfo _currentArrangeInfo;

			// Token: 0x0400471A RID: 18202
			private Stack<StructuralCache.DocumentArrangeContext.DocumentArrangeInfo> _documentArrangeInfoStack = new Stack<StructuralCache.DocumentArrangeContext.DocumentArrangeInfo>();

			// Token: 0x02000BB7 RID: 2999
			private struct DocumentArrangeInfo
			{
				// Token: 0x04004EF7 RID: 20215
				internal PageContext PageContext;

				// Token: 0x04004EF8 RID: 20216
				internal PTS.FSRECT ColumnRect;

				// Token: 0x04004EF9 RID: 20217
				internal bool FinitePage;
			}
		}

		// Token: 0x02000A27 RID: 2599
		internal class DocumentVisualValidationContext : StructuralCache.DocumentOperationContext, IDisposable
		{
			// Token: 0x06008AD4 RID: 35540 RVA: 0x00257D5E File Offset: 0x00255F5E
			internal DocumentVisualValidationContext(StructuralCache owner, FlowDocumentPage page) : base(owner, page)
			{
			}

			// Token: 0x06008AD5 RID: 35541 RVA: 0x00257D68 File Offset: 0x00255F68
			void IDisposable.Dispose()
			{
				GC.SuppressFinalize(this);
				base.Dispose();
			}
		}
	}
}
