using System;
using System.Collections;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.TextFormatting;
using MS.Internal;
using MS.Internal.Documents;

namespace System.Windows.Documents
{
	// Token: 0x020003D4 RID: 980
	internal sealed class RubberbandSelector
	{
		// Token: 0x06003504 RID: 13572 RVA: 0x000EFEB8 File Offset: 0x000EE0B8
		internal void ClearSelection()
		{
			if (this.HasSelection)
			{
				FixedPage page = this._page;
				this._page = null;
				this.UpdateHighlightVisual(page);
			}
			this._selectionRect = Rect.Empty;
		}

		// Token: 0x06003505 RID: 13573 RVA: 0x000EFEF0 File Offset: 0x000EE0F0
		[SecurityCritical]
		[SecurityTreatAsSafe]
		internal void AttachRubberbandSelector(FrameworkElement scope)
		{
			if (scope == null)
			{
				throw new ArgumentNullException("scope");
			}
			this.ClearSelection();
			scope.MouseLeftButtonDown += this.OnLeftMouseDown;
			scope.MouseLeftButtonUp += this.OnLeftMouseUp;
			scope.MouseMove += this.OnMouseMove;
			scope.QueryCursor += this.OnQueryCursor;
			scope.Cursor = null;
			if (scope is DocumentGrid)
			{
				this._uiScope = ((DocumentGrid)scope).DocumentViewerOwner;
				Invariant.Assert(this._uiScope != null, "DocumentGrid's DocumentViewerOwner cannot be null.");
			}
			else
			{
				this._uiScope = scope;
			}
			CommandBinding commandBinding = new CommandBinding(ApplicationCommands.Copy);
			commandBinding.Executed += this.OnCopy;
			commandBinding.CanExecute += this.QueryCopy;
			this._uiScope.CommandBindings.Add(commandBinding);
			this._scope = scope;
		}

		// Token: 0x06003506 RID: 13574 RVA: 0x000EFFE0 File Offset: 0x000EE1E0
		internal void DetachRubberbandSelector()
		{
			this.ClearSelection();
			if (this._scope != null)
			{
				this._scope.MouseLeftButtonDown -= this.OnLeftMouseDown;
				this._scope.MouseLeftButtonUp -= this.OnLeftMouseUp;
				this._scope.MouseMove -= this.OnMouseMove;
				this._scope.QueryCursor -= this.OnQueryCursor;
				this._scope = null;
			}
			if (this._uiScope != null)
			{
				CommandBindingCollection commandBindings = this._uiScope.CommandBindings;
				foreach (object obj in commandBindings)
				{
					CommandBinding commandBinding = (CommandBinding)obj;
					if (commandBinding.Command == ApplicationCommands.Copy)
					{
						commandBinding.Executed -= this.OnCopy;
						commandBinding.CanExecute -= this.QueryCopy;
					}
				}
				this._uiScope = null;
			}
		}

		// Token: 0x06003507 RID: 13575 RVA: 0x000F00EC File Offset: 0x000EE2EC
		private void ExtendSelection(Point pt)
		{
			Size size = this._panel.ComputePageSize(this._page);
			if (pt.X < 0.0)
			{
				pt.X = 0.0;
			}
			else if (pt.X > size.Width)
			{
				pt.X = size.Width;
			}
			if (pt.Y < 0.0)
			{
				pt.Y = 0.0;
			}
			else if (pt.Y > size.Height)
			{
				pt.Y = size.Height;
			}
			this._selectionRect = new Rect(this._origin, pt);
			this.UpdateHighlightVisual(this._page);
		}

		// Token: 0x06003508 RID: 13576 RVA: 0x000F01B0 File Offset: 0x000EE3B0
		private void UpdateHighlightVisual(FixedPage page)
		{
			if (page != null)
			{
				HighlightVisual highlightVisual = HighlightVisual.GetHighlightVisual(page);
				if (highlightVisual != null)
				{
					highlightVisual.UpdateRubberbandSelection(this);
				}
			}
		}

		// Token: 0x06003509 RID: 13577 RVA: 0x000F01D4 File Offset: 0x000EE3D4
		[SecuritySafeCritical]
		private bool HasRubberBandCopyPermissions()
		{
			bool result;
			try
			{
				new SecurityPermission(SecurityPermissionFlag.UnmanagedCode | SecurityPermissionFlag.SerializationFormatter).Demand();
				CodeAccessPermission codeAccessPermission = SecurityHelper.CreateMediaAccessPermission(null);
				codeAccessPermission.Demand();
				result = true;
			}
			catch (SecurityException)
			{
				result = false;
			}
			return result;
		}

		// Token: 0x0600350A RID: 13578 RVA: 0x000F0218 File Offset: 0x000EE418
		[SecurityCritical]
		[SecurityTreatAsSafe]
		private void OnCopy(object sender, ExecutedRoutedEventArgs e)
		{
			if (this.HasSelection && this._selectionRect.Width > 0.0 && this._selectionRect.Height > 0.0)
			{
				string text = this.GetText();
				object obj = null;
				bool flag = false;
				if (this._scope is DocumentGrid && ((DocumentGrid)this._scope).DocumentViewerOwner is DocumentApplicationDocumentViewer)
				{
					if (!e.UserInitiated && !this.HasRubberBandCopyPermissions())
					{
						return;
					}
					flag = true;
				}
				else
				{
					flag = this.HasRubberBandCopyPermissions();
				}
				if (flag)
				{
					obj = SystemDrawingHelper.GetBitmapFromBitmapSource(this.GetImage());
				}
				new UIPermission(UIPermissionClipboard.AllClipboard).Assert();
				IDataObject dataObject;
				try
				{
					dataObject = new DataObject();
					dataObject.SetData(DataFormats.Text, text, true);
					dataObject.SetData(DataFormats.UnicodeText, text, true);
					if (obj != null)
					{
						dataObject.SetData(DataFormats.Bitmap, obj, true);
					}
				}
				finally
				{
					CodeAccessPermission.RevertAssert();
				}
				PermissionSet permissionSet = new PermissionSet(PermissionState.None);
				permissionSet.AddPermission(new SecurityPermission(SecurityPermissionFlag.SerializationFormatter));
				permissionSet.AddPermission(new UIPermission(UIPermissionClipboard.AllClipboard));
				permissionSet.AddPermission(new SecurityPermission(SecurityPermissionFlag.UnmanagedCode));
				if (flag)
				{
					CodeAccessPermission perm = SecurityHelper.CreateMediaAccessPermission(null);
					permissionSet.AddPermission(perm);
				}
				permissionSet.Assert();
				try
				{
					Clipboard.SetDataObject(dataObject, true);
				}
				catch (ExternalException)
				{
				}
				finally
				{
					CodeAccessPermission.RevertAssert();
				}
			}
		}

		// Token: 0x0600350B RID: 13579 RVA: 0x000F038C File Offset: 0x000EE58C
		private BitmapSource GetImage()
		{
			Visual visual = this.GetVisual(-this._selectionRect.Left, -this._selectionRect.Top);
			double num = 96.0;
			double num2 = num / 96.0;
			RenderTargetBitmap renderTargetBitmap = new RenderTargetBitmap((int)(num2 * this._selectionRect.Width), (int)(num2 * this._selectionRect.Height), num, num, PixelFormats.Pbgra32);
			renderTargetBitmap.Render(visual);
			return renderTargetBitmap;
		}

		// Token: 0x0600350C RID: 13580 RVA: 0x000F0400 File Offset: 0x000EE600
		private Visual GetVisual(double offsetX, double offsetY)
		{
			ContainerVisual containerVisual = new ContainerVisual();
			DrawingVisual drawingVisual = new DrawingVisual();
			containerVisual.Children.Add(drawingVisual);
			drawingVisual.Offset = new Vector(offsetX, offsetY);
			DrawingContext drawingContext = drawingVisual.RenderOpen();
			drawingContext.DrawDrawing(this._page.GetDrawing());
			drawingContext.Close();
			UIElementCollection children = this._page.Children;
			foreach (object obj in children)
			{
				UIElement old = (UIElement)obj;
				this.CloneVisualTree(drawingVisual, old);
			}
			return containerVisual;
		}

		// Token: 0x0600350D RID: 13581 RVA: 0x000F04B0 File Offset: 0x000EE6B0
		private void CloneVisualTree(ContainerVisual parent, Visual old)
		{
			DrawingVisual drawingVisual = new DrawingVisual();
			parent.Children.Add(drawingVisual);
			drawingVisual.Clip = VisualTreeHelper.GetClip(old);
			drawingVisual.Offset = VisualTreeHelper.GetOffset(old);
			drawingVisual.Transform = VisualTreeHelper.GetTransform(old);
			drawingVisual.Opacity = VisualTreeHelper.GetOpacity(old);
			drawingVisual.OpacityMask = VisualTreeHelper.GetOpacityMask(old);
			drawingVisual.BitmapEffectInput = VisualTreeHelper.GetBitmapEffectInput(old);
			drawingVisual.BitmapEffect = VisualTreeHelper.GetBitmapEffect(old);
			DrawingContext drawingContext = drawingVisual.RenderOpen();
			drawingContext.DrawDrawing(old.GetDrawing());
			drawingContext.Close();
			int childrenCount = VisualTreeHelper.GetChildrenCount(old);
			for (int i = 0; i < childrenCount; i++)
			{
				Visual old2 = old.InternalGetVisualChild(i);
				this.CloneVisualTree(drawingVisual, old2);
			}
		}

		// Token: 0x0600350E RID: 13582 RVA: 0x000F0564 File Offset: 0x000EE764
		private string GetText()
		{
			double top = this._selectionRect.Top;
			double bottom = this._selectionRect.Bottom;
			double left = this._selectionRect.Left;
			double right = this._selectionRect.Right;
			double num = 0.0;
			double num2 = 0.0;
			int count = this._page.Children.Count;
			ArrayList arrayList = new ArrayList();
			FixedNode[] array = this._panel.FixedContainer.FixedTextBuilder.GetFirstLine(this._pageIndex);
			while (array != null && array.Length != 0)
			{
				RubberbandSelector.TextPositionPair textPositionPair = null;
				foreach (FixedNode node in array)
				{
					Glyphs glyphsElement = this._page.GetGlyphsElement(node);
					if (glyphsElement != null)
					{
						int num3;
						int charIndex;
						bool flag;
						if (this.IntersectGlyphs(glyphsElement, top, left, bottom, right, out num3, out charIndex, out flag, out num, out num2))
						{
							if (textPositionPair == null || num3 > 0)
							{
								textPositionPair = new RubberbandSelector.TextPositionPair();
								textPositionPair.first = this._GetTextPosition(node, num3);
								arrayList.Add(textPositionPair);
							}
							textPositionPair.second = this._GetTextPosition(node, charIndex);
							if (!flag)
							{
								textPositionPair = null;
							}
						}
						else
						{
							textPositionPair = null;
						}
					}
				}
				int num4 = 1;
				array = this._panel.FixedContainer.FixedTextBuilder.GetNextLine(array[0], true, ref num4);
			}
			string text = "";
			foreach (object obj in arrayList)
			{
				RubberbandSelector.TextPositionPair textPositionPair2 = (RubberbandSelector.TextPositionPair)obj;
				text = text + TextRangeBase.GetTextInternal(textPositionPair2.first, textPositionPair2.second) + "\r\n";
			}
			return text;
		}

		// Token: 0x0600350F RID: 13583 RVA: 0x000F0754 File Offset: 0x000EE954
		private ITextPointer _GetTextPosition(FixedNode node, int charIndex)
		{
			FixedPosition fixedPosition = new FixedPosition(node, charIndex);
			FlowPosition flowPosition = this._panel.FixedContainer.FixedTextBuilder.CreateFlowPosition(fixedPosition);
			if (flowPosition != null)
			{
				return new FixedTextPointer(false, LogicalDirection.Forward, flowPosition);
			}
			return null;
		}

		// Token: 0x06003510 RID: 13584 RVA: 0x000F0790 File Offset: 0x000EE990
		private bool IntersectGlyphs(Glyphs g, double top, double left, double bottom, double right, out int begin, out int end, out bool includeEnd, out double baseline, out double height)
		{
			begin = 0;
			end = 0;
			includeEnd = false;
			GlyphRun glyphRun = g.ToGlyphRun();
			Rect rect = glyphRun.ComputeAlignmentBox();
			rect.Offset(glyphRun.BaselineOrigin.X, glyphRun.BaselineOrigin.Y);
			baseline = glyphRun.BaselineOrigin.Y;
			height = rect.Height;
			double y = rect.Y + 0.5 * rect.Height;
			GeneralTransform generalTransform = g.TransformToAncestor(this._page);
			Point point;
			generalTransform.TryTransform(new Point(rect.Left, y), out point);
			Point point2;
			generalTransform.TryTransform(new Point(rect.Right, y), out point2);
			bool flag = false;
			if (point.X < left)
			{
				if (point2.X < left)
				{
					return false;
				}
				flag = true;
			}
			else if (point.X > right)
			{
				if (point2.X > right)
				{
					return false;
				}
				flag = true;
			}
			else if (point2.X < left || point2.X > right)
			{
				flag = true;
			}
			double num3;
			double num4;
			if (flag)
			{
				double num = (left - point.X) / (point2.X - point.X);
				double num2 = (right - point.X) / (point2.X - point.X);
				if (num2 > num)
				{
					num3 = num;
					num4 = num2;
				}
				else
				{
					num3 = num2;
					num4 = num;
				}
			}
			else
			{
				num3 = 0.0;
				num4 = 1.0;
			}
			flag = false;
			if (point.Y < top)
			{
				if (point2.Y < top)
				{
					return false;
				}
				flag = true;
			}
			else if (point.Y > bottom)
			{
				if (point2.Y > bottom)
				{
					return false;
				}
				flag = true;
			}
			else if (point2.Y < top || point2.Y > bottom)
			{
				flag = true;
			}
			if (flag)
			{
				double num5 = (top - point.Y) / (point2.Y - point.Y);
				double num6 = (bottom - point.Y) / (point2.Y - point.Y);
				if (num6 > num5)
				{
					if (num5 > num3)
					{
						num3 = num5;
					}
					if (num6 < num4)
					{
						num4 = num6;
					}
				}
				else
				{
					if (num6 > num3)
					{
						num3 = num6;
					}
					if (num5 < num4)
					{
						num4 = num5;
					}
				}
			}
			num3 = rect.Left + rect.Width * num3;
			num4 = rect.Left + rect.Width * num4;
			bool ltr = (glyphRun.BidiLevel & 1) == 0;
			begin = this.GlyphRunHitTest(glyphRun, num3, ltr);
			end = this.GlyphRunHitTest(glyphRun, num4, ltr);
			if (begin > end)
			{
				int num7 = begin;
				begin = end;
				end = num7;
			}
			int num8 = (glyphRun.Characters == null) ? 0 : glyphRun.Characters.Count;
			includeEnd = (end == num8);
			return true;
		}

		// Token: 0x06003511 RID: 13585 RVA: 0x000F0A54 File Offset: 0x000EEC54
		private int GlyphRunHitTest(GlyphRun run, double xoffset, bool LTR)
		{
			double distance = LTR ? (xoffset - run.BaselineOrigin.X) : (run.BaselineOrigin.X - xoffset);
			bool flag;
			CharacterHit caretCharacterHitFromDistance = run.GetCaretCharacterHitFromDistance(distance, out flag);
			return caretCharacterHitFromDistance.FirstCharacterIndex + caretCharacterHitFromDistance.TrailingLength;
		}

		// Token: 0x06003512 RID: 13586 RVA: 0x000F0AA0 File Offset: 0x000EECA0
		private void QueryCopy(object sender, CanExecuteRoutedEventArgs e)
		{
			if (this.HasSelection)
			{
				e.CanExecute = true;
			}
		}

		// Token: 0x06003513 RID: 13587 RVA: 0x000F0AB4 File Offset: 0x000EECB4
		private void OnLeftMouseDown(object sender, MouseButtonEventArgs e)
		{
			e.Handled = true;
			FixedDocumentPage fixedPanelDocumentPage = this.GetFixedPanelDocumentPage(e.GetPosition(this._scope));
			if (fixedPanelDocumentPage != null)
			{
				this._uiScope.Focus();
				this._scope.CaptureMouse();
				this.ClearSelection();
				this._panel = fixedPanelDocumentPage.Owner;
				this._page = fixedPanelDocumentPage.FixedPage;
				this._isSelecting = true;
				this._origin = e.GetPosition(this._page);
				this._pageIndex = fixedPanelDocumentPage.PageIndex;
			}
		}

		// Token: 0x06003514 RID: 13588 RVA: 0x000F0B39 File Offset: 0x000EED39
		private void OnLeftMouseUp(object sender, MouseButtonEventArgs e)
		{
			e.Handled = true;
			this._scope.ReleaseMouseCapture();
			if (this._isSelecting)
			{
				this._isSelecting = false;
				if (this._page != null)
				{
					this.ExtendSelection(e.GetPosition(this._page));
				}
			}
		}

		// Token: 0x06003515 RID: 13589 RVA: 0x000F0B76 File Offset: 0x000EED76
		private void OnMouseMove(object sender, MouseEventArgs e)
		{
			e.Handled = true;
			if (e.LeftButton == MouseButtonState.Released)
			{
				this._isSelecting = false;
				return;
			}
			if (this._isSelecting && this._page != null)
			{
				this.ExtendSelection(e.GetPosition(this._page));
			}
		}

		// Token: 0x06003516 RID: 13590 RVA: 0x000F0BB1 File Offset: 0x000EEDB1
		private void OnQueryCursor(object sender, QueryCursorEventArgs e)
		{
			if (this._isSelecting || this.GetFixedPanelDocumentPage(e.GetPosition(this._scope)) != null)
			{
				e.Cursor = Cursors.Cross;
			}
			else
			{
				e.Cursor = Cursors.Arrow;
			}
			e.Handled = true;
		}

		// Token: 0x06003517 RID: 13591 RVA: 0x000F0BF0 File Offset: 0x000EEDF0
		private FixedDocumentPage GetFixedPanelDocumentPage(Point pt)
		{
			DocumentGrid documentGrid = this._scope as DocumentGrid;
			if (documentGrid != null)
			{
				DocumentPage documentPageFromPoint = documentGrid.GetDocumentPageFromPoint(pt);
				FixedDocumentPage fixedDocumentPage = documentPageFromPoint as FixedDocumentPage;
				if (fixedDocumentPage == null)
				{
					FixedDocumentSequenceDocumentPage fixedDocumentSequenceDocumentPage = documentPageFromPoint as FixedDocumentSequenceDocumentPage;
					if (fixedDocumentSequenceDocumentPage != null)
					{
						fixedDocumentPage = (fixedDocumentSequenceDocumentPage.ChildDocumentPage as FixedDocumentPage);
					}
				}
				return fixedDocumentPage;
			}
			return null;
		}

		// Token: 0x17000DA4 RID: 3492
		// (get) Token: 0x06003518 RID: 13592 RVA: 0x000F0C37 File Offset: 0x000EEE37
		internal FixedPage Page
		{
			get
			{
				return this._page;
			}
		}

		// Token: 0x17000DA5 RID: 3493
		// (get) Token: 0x06003519 RID: 13593 RVA: 0x000F0C3F File Offset: 0x000EEE3F
		internal Rect SelectionRect
		{
			get
			{
				return this._selectionRect;
			}
		}

		// Token: 0x17000DA6 RID: 3494
		// (get) Token: 0x0600351A RID: 13594 RVA: 0x000F0C47 File Offset: 0x000EEE47
		internal bool HasSelection
		{
			get
			{
				return this._page != null && this._panel != null && !this._selectionRect.IsEmpty;
			}
		}

		// Token: 0x040024F4 RID: 9460
		private FixedDocument _panel;

		// Token: 0x040024F5 RID: 9461
		private FixedPage _page;

		// Token: 0x040024F6 RID: 9462
		private Rect _selectionRect;

		// Token: 0x040024F7 RID: 9463
		private bool _isSelecting;

		// Token: 0x040024F8 RID: 9464
		private Point _origin;

		// Token: 0x040024F9 RID: 9465
		private UIElement _scope;

		// Token: 0x040024FA RID: 9466
		private FrameworkElement _uiScope;

		// Token: 0x040024FB RID: 9467
		private int _pageIndex;

		// Token: 0x020008DC RID: 2268
		private class TextPositionPair
		{
			// Token: 0x04004293 RID: 17043
			public ITextPointer first;

			// Token: 0x04004294 RID: 17044
			public ITextPointer second;
		}
	}
}
