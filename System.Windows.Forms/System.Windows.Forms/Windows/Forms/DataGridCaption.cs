using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;

namespace System.Windows.Forms
{
	// Token: 0x0200016D RID: 365
	internal class DataGridCaption
	{
		// Token: 0x060012B6 RID: 4790 RVA: 0x00047600 File Offset: 0x00045800
		internal DataGridCaption(DataGrid dataGrid)
		{
			this.dataGrid = dataGrid;
			this.downButtonVisible = dataGrid.ParentRowsVisible;
			DataGridCaption.colorMap[0].OldColor = Color.White;
			DataGridCaption.colorMap[0].NewColor = this.ForeColor;
			this.OnGridFontChanged();
		}

		// Token: 0x060012B7 RID: 4791 RVA: 0x0004767C File Offset: 0x0004587C
		internal void OnGridFontChanged()
		{
			if (this.dataGridFont == null || !this.dataGridFont.Equals(this.dataGrid.Font))
			{
				try
				{
					this.dataGridFont = new Font(this.dataGrid.Font, FontStyle.Bold);
				}
				catch
				{
				}
			}
		}

		// Token: 0x1700048D RID: 1165
		// (get) Token: 0x060012B8 RID: 4792 RVA: 0x000476D8 File Offset: 0x000458D8
		// (set) Token: 0x060012B9 RID: 4793 RVA: 0x000476E0 File Offset: 0x000458E0
		internal bool BackButtonActive
		{
			get
			{
				return this.backActive;
			}
			set
			{
				if (this.backActive != value)
				{
					this.backActive = value;
					this.InvalidateCaptionRect(this.backButtonRect);
				}
			}
		}

		// Token: 0x1700048E RID: 1166
		// (get) Token: 0x060012BA RID: 4794 RVA: 0x000476FE File Offset: 0x000458FE
		// (set) Token: 0x060012BB RID: 4795 RVA: 0x00047706 File Offset: 0x00045906
		internal bool DownButtonActive
		{
			get
			{
				return this.downActive;
			}
			set
			{
				if (this.downActive != value)
				{
					this.downActive = value;
					this.InvalidateCaptionRect(this.downButtonRect);
				}
			}
		}

		// Token: 0x1700048F RID: 1167
		// (get) Token: 0x060012BC RID: 4796 RVA: 0x0003C476 File Offset: 0x0003A676
		internal static SolidBrush DefaultBackBrush
		{
			get
			{
				return (SolidBrush)SystemBrushes.ActiveCaption;
			}
		}

		// Token: 0x17000490 RID: 1168
		// (get) Token: 0x060012BD RID: 4797 RVA: 0x00047724 File Offset: 0x00045924
		internal static Pen DefaultTextBorderPen
		{
			get
			{
				return new Pen(SystemColors.ActiveCaptionText);
			}
		}

		// Token: 0x17000491 RID: 1169
		// (get) Token: 0x060012BE RID: 4798 RVA: 0x0003C482 File Offset: 0x0003A682
		internal static SolidBrush DefaultForeBrush
		{
			get
			{
				return (SolidBrush)SystemBrushes.ActiveCaptionText;
			}
		}

		// Token: 0x17000492 RID: 1170
		// (get) Token: 0x060012BF RID: 4799 RVA: 0x00047730 File Offset: 0x00045930
		// (set) Token: 0x060012C0 RID: 4800 RVA: 0x00047740 File Offset: 0x00045940
		internal Color BackColor
		{
			get
			{
				return this.backBrush.Color;
			}
			set
			{
				if (!this.backBrush.Color.Equals(value))
				{
					if (value.IsEmpty)
					{
						throw new ArgumentException(SR.GetString("DataGridEmptyColor", new object[]
						{
							"Caption BackColor"
						}));
					}
					this.backBrush = new SolidBrush(value);
					this.Invalidate();
				}
			}
		}

		// Token: 0x17000493 RID: 1171
		// (get) Token: 0x060012C1 RID: 4801 RVA: 0x000477A7 File Offset: 0x000459A7
		internal EventHandlerList Events
		{
			get
			{
				if (this.events == null)
				{
					this.events = new EventHandlerList();
				}
				return this.events;
			}
		}

		// Token: 0x17000494 RID: 1172
		// (get) Token: 0x060012C2 RID: 4802 RVA: 0x000477C2 File Offset: 0x000459C2
		// (set) Token: 0x060012C3 RID: 4803 RVA: 0x000477DC File Offset: 0x000459DC
		internal Font Font
		{
			get
			{
				if (this.textFont == null)
				{
					return this.dataGridFont;
				}
				return this.textFont;
			}
			set
			{
				if (this.textFont == null || !this.textFont.Equals(value))
				{
					this.textFont = value;
					if (this.dataGrid.Caption != null)
					{
						this.dataGrid.RecalculateFonts();
						this.dataGrid.PerformLayout();
						this.dataGrid.Invalidate();
					}
				}
			}
		}

		// Token: 0x060012C4 RID: 4804 RVA: 0x00047834 File Offset: 0x00045A34
		internal bool ShouldSerializeFont()
		{
			return this.textFont != null && !this.textFont.Equals(this.dataGridFont);
		}

		// Token: 0x060012C5 RID: 4805 RVA: 0x00047854 File Offset: 0x00045A54
		internal bool ShouldSerializeBackColor()
		{
			return !this.backBrush.Equals(DataGridCaption.DefaultBackBrush);
		}

		// Token: 0x060012C6 RID: 4806 RVA: 0x00047869 File Offset: 0x00045A69
		internal void ResetBackColor()
		{
			if (this.ShouldSerializeBackColor())
			{
				this.backBrush = DataGridCaption.DefaultBackBrush;
				this.Invalidate();
			}
		}

		// Token: 0x060012C7 RID: 4807 RVA: 0x00047884 File Offset: 0x00045A84
		internal void ResetForeColor()
		{
			if (this.ShouldSerializeForeColor())
			{
				this.foreBrush = DataGridCaption.DefaultForeBrush;
				this.Invalidate();
			}
		}

		// Token: 0x060012C8 RID: 4808 RVA: 0x0004789F File Offset: 0x00045A9F
		internal bool ShouldSerializeForeColor()
		{
			return !this.foreBrush.Equals(DataGridCaption.DefaultForeBrush);
		}

		// Token: 0x060012C9 RID: 4809 RVA: 0x000478B4 File Offset: 0x00045AB4
		internal void ResetFont()
		{
			this.textFont = null;
			this.Invalidate();
		}

		// Token: 0x17000495 RID: 1173
		// (get) Token: 0x060012CA RID: 4810 RVA: 0x000478C3 File Offset: 0x00045AC3
		// (set) Token: 0x060012CB RID: 4811 RVA: 0x000478CB File Offset: 0x00045ACB
		internal string Text
		{
			get
			{
				return this.text;
			}
			set
			{
				if (value == null)
				{
					this.text = "";
				}
				else
				{
					this.text = value;
				}
				this.Invalidate();
			}
		}

		// Token: 0x17000496 RID: 1174
		// (get) Token: 0x060012CC RID: 4812 RVA: 0x000478EA File Offset: 0x00045AEA
		// (set) Token: 0x060012CD RID: 4813 RVA: 0x000478F2 File Offset: 0x00045AF2
		internal bool TextBorderVisible
		{
			get
			{
				return this.textBorderVisible;
			}
			set
			{
				this.textBorderVisible = value;
				this.Invalidate();
			}
		}

		// Token: 0x17000497 RID: 1175
		// (get) Token: 0x060012CE RID: 4814 RVA: 0x00047901 File Offset: 0x00045B01
		// (set) Token: 0x060012CF RID: 4815 RVA: 0x00047910 File Offset: 0x00045B10
		internal Color ForeColor
		{
			get
			{
				return this.foreBrush.Color;
			}
			set
			{
				if (value.IsEmpty)
				{
					throw new ArgumentException(SR.GetString("DataGridEmptyColor", new object[]
					{
						"Caption ForeColor"
					}));
				}
				this.foreBrush = new SolidBrush(value);
				DataGridCaption.colorMap[0].NewColor = this.ForeColor;
				this.Invalidate();
			}
		}

		// Token: 0x17000498 RID: 1176
		// (get) Token: 0x060012D0 RID: 4816 RVA: 0x00047968 File Offset: 0x00045B68
		internal Point MinimumBounds
		{
			get
			{
				return DataGridCaption.minimumBounds;
			}
		}

		// Token: 0x17000499 RID: 1177
		// (get) Token: 0x060012D1 RID: 4817 RVA: 0x0004796F File Offset: 0x00045B6F
		// (set) Token: 0x060012D2 RID: 4818 RVA: 0x00047977 File Offset: 0x00045B77
		internal bool BackButtonVisible
		{
			get
			{
				return this.backButtonVisible;
			}
			set
			{
				if (this.backButtonVisible != value)
				{
					this.backButtonVisible = value;
					this.Invalidate();
				}
			}
		}

		// Token: 0x1700049A RID: 1178
		// (get) Token: 0x060012D3 RID: 4819 RVA: 0x0004798F File Offset: 0x00045B8F
		// (set) Token: 0x060012D4 RID: 4820 RVA: 0x00047997 File Offset: 0x00045B97
		internal bool DownButtonVisible
		{
			get
			{
				return this.downButtonVisible;
			}
			set
			{
				if (this.downButtonVisible != value)
				{
					this.downButtonVisible = value;
					this.Invalidate();
				}
			}
		}

		// Token: 0x060012D5 RID: 4821 RVA: 0x000479B0 File Offset: 0x00045BB0
		protected virtual void AddEventHandler(object key, Delegate handler)
		{
			lock (this)
			{
				if (handler != null)
				{
					for (DataGridCaption.EventEntry next = this.eventList; next != null; next = next.next)
					{
						if (next.key == key)
						{
							next.handler = Delegate.Combine(next.handler, handler);
							return;
						}
					}
					this.eventList = new DataGridCaption.EventEntry(this.eventList, key, handler);
				}
			}
		}

		// Token: 0x140000C9 RID: 201
		// (add) Token: 0x060012D6 RID: 4822 RVA: 0x00047A30 File Offset: 0x00045C30
		// (remove) Token: 0x060012D7 RID: 4823 RVA: 0x00047A43 File Offset: 0x00045C43
		internal event EventHandler BackwardClicked
		{
			add
			{
				this.Events.AddHandler(DataGridCaption.EVENT_BACKWARDCLICKED, value);
			}
			remove
			{
				this.Events.RemoveHandler(DataGridCaption.EVENT_BACKWARDCLICKED, value);
			}
		}

		// Token: 0x140000CA RID: 202
		// (add) Token: 0x060012D8 RID: 4824 RVA: 0x00047A56 File Offset: 0x00045C56
		// (remove) Token: 0x060012D9 RID: 4825 RVA: 0x00047A69 File Offset: 0x00045C69
		internal event EventHandler CaptionClicked
		{
			add
			{
				this.Events.AddHandler(DataGridCaption.EVENT_CAPTIONCLICKED, value);
			}
			remove
			{
				this.Events.RemoveHandler(DataGridCaption.EVENT_CAPTIONCLICKED, value);
			}
		}

		// Token: 0x140000CB RID: 203
		// (add) Token: 0x060012DA RID: 4826 RVA: 0x00047A7C File Offset: 0x00045C7C
		// (remove) Token: 0x060012DB RID: 4827 RVA: 0x00047A8F File Offset: 0x00045C8F
		internal event EventHandler DownClicked
		{
			add
			{
				this.Events.AddHandler(DataGridCaption.EVENT_DOWNCLICKED, value);
			}
			remove
			{
				this.Events.RemoveHandler(DataGridCaption.EVENT_DOWNCLICKED, value);
			}
		}

		// Token: 0x060012DC RID: 4828 RVA: 0x00047AA2 File Offset: 0x00045CA2
		private void Invalidate()
		{
			if (this.dataGrid != null)
			{
				this.dataGrid.InvalidateCaption();
			}
		}

		// Token: 0x060012DD RID: 4829 RVA: 0x00047AB7 File Offset: 0x00045CB7
		private void InvalidateCaptionRect(Rectangle r)
		{
			if (this.dataGrid != null)
			{
				this.dataGrid.InvalidateCaptionRect(r);
			}
		}

		// Token: 0x060012DE RID: 4830 RVA: 0x00047AD0 File Offset: 0x00045CD0
		private void InvalidateLocation(DataGridCaption.CaptionLocation loc)
		{
			Rectangle r;
			if (loc == DataGridCaption.CaptionLocation.BackButton)
			{
				r = this.backButtonRect;
				r.Inflate(1, 1);
				this.InvalidateCaptionRect(r);
				return;
			}
			if (loc != DataGridCaption.CaptionLocation.DownButton)
			{
				return;
			}
			r = this.downButtonRect;
			r.Inflate(1, 1);
			this.InvalidateCaptionRect(r);
		}

		// Token: 0x060012DF RID: 4831 RVA: 0x00047B18 File Offset: 0x00045D18
		protected void OnBackwardClicked(EventArgs e)
		{
			if (this.backActive)
			{
				EventHandler eventHandler = (EventHandler)this.Events[DataGridCaption.EVENT_BACKWARDCLICKED];
				if (eventHandler != null)
				{
					eventHandler(this, e);
				}
			}
		}

		// Token: 0x060012E0 RID: 4832 RVA: 0x00047B50 File Offset: 0x00045D50
		protected void OnCaptionClicked(EventArgs e)
		{
			EventHandler eventHandler = (EventHandler)this.Events[DataGridCaption.EVENT_CAPTIONCLICKED];
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		// Token: 0x060012E1 RID: 4833 RVA: 0x00047B80 File Offset: 0x00045D80
		protected void OnDownClicked(EventArgs e)
		{
			if (this.downActive && this.downButtonVisible)
			{
				EventHandler eventHandler = (EventHandler)this.Events[DataGridCaption.EVENT_DOWNCLICKED];
				if (eventHandler != null)
				{
					eventHandler(this, e);
				}
			}
		}

		// Token: 0x060012E2 RID: 4834 RVA: 0x00047BC0 File Offset: 0x00045DC0
		private Bitmap GetBitmap(string bitmapName)
		{
			Bitmap bitmap = null;
			try
			{
				bitmap = new Bitmap(typeof(DataGridCaption), bitmapName);
				bitmap.MakeTransparent();
			}
			catch (Exception ex)
			{
			}
			return bitmap;
		}

		// Token: 0x060012E3 RID: 4835 RVA: 0x00047BFC File Offset: 0x00045DFC
		private Bitmap GetBackButtonBmp(bool alignRight)
		{
			if (alignRight)
			{
				if (DataGridCaption.leftButtonBitmap_bidi == null)
				{
					DataGridCaption.leftButtonBitmap_bidi = this.GetBitmap("DataGridCaption.backarrow_bidi.bmp");
				}
				return DataGridCaption.leftButtonBitmap_bidi;
			}
			if (DataGridCaption.leftButtonBitmap == null)
			{
				DataGridCaption.leftButtonBitmap = this.GetBitmap("DataGridCaption.backarrow.bmp");
			}
			return DataGridCaption.leftButtonBitmap;
		}

		// Token: 0x060012E4 RID: 4836 RVA: 0x00047C3A File Offset: 0x00045E3A
		private Bitmap GetDetailsBmp()
		{
			if (DataGridCaption.magnifyingGlassBitmap == null)
			{
				DataGridCaption.magnifyingGlassBitmap = this.GetBitmap("DataGridCaption.Details.bmp");
			}
			return DataGridCaption.magnifyingGlassBitmap;
		}

		// Token: 0x060012E5 RID: 4837 RVA: 0x00047C58 File Offset: 0x00045E58
		protected virtual Delegate GetEventHandler(object key)
		{
			Delegate result;
			lock (this)
			{
				for (DataGridCaption.EventEntry next = this.eventList; next != null; next = next.next)
				{
					if (next.key == key)
					{
						return next.handler;
					}
				}
				result = null;
			}
			return result;
		}

		// Token: 0x060012E6 RID: 4838 RVA: 0x00047CB8 File Offset: 0x00045EB8
		internal Rectangle GetBackButtonRect(Rectangle bounds, bool alignRight, int downButtonWidth)
		{
			Bitmap backButtonBmp = this.GetBackButtonBmp(false);
			Bitmap obj = backButtonBmp;
			Size size;
			lock (obj)
			{
				size = backButtonBmp.Size;
			}
			return new Rectangle(bounds.Right - 12 - downButtonWidth - size.Width, bounds.Y + 1 + 2, size.Width, size.Height);
		}

		// Token: 0x060012E7 RID: 4839 RVA: 0x00047D30 File Offset: 0x00045F30
		internal int GetDetailsButtonWidth()
		{
			int result = 0;
			Bitmap detailsBmp = this.GetDetailsBmp();
			Bitmap obj = detailsBmp;
			lock (obj)
			{
				result = detailsBmp.Size.Width;
			}
			return result;
		}

		// Token: 0x060012E8 RID: 4840 RVA: 0x00047D80 File Offset: 0x00045F80
		internal Rectangle GetDetailsButtonRect(Rectangle bounds, bool alignRight)
		{
			Bitmap detailsBmp = this.GetDetailsBmp();
			Bitmap obj = detailsBmp;
			Size size;
			lock (obj)
			{
				size = detailsBmp.Size;
			}
			int width = size.Width;
			return new Rectangle(bounds.Right - 6 - width, bounds.Y + 1 + 2, width, size.Height);
		}

		// Token: 0x060012E9 RID: 4841 RVA: 0x00047DF0 File Offset: 0x00045FF0
		internal void Paint(Graphics g, Rectangle bounds, bool alignRight)
		{
			Size size = new Size((int)g.MeasureString(this.text, this.Font).Width + 2, this.Font.Height + 2);
			this.downButtonRect = this.GetDetailsButtonRect(bounds, alignRight);
			int detailsButtonWidth = this.GetDetailsButtonWidth();
			this.backButtonRect = this.GetBackButtonRect(bounds, alignRight, detailsButtonWidth);
			int num = this.backButtonVisible ? (this.backButtonRect.Width + 3 + 4) : 0;
			int num2 = (this.downButtonVisible && !this.dataGrid.ParentRowsIsEmpty()) ? (detailsButtonWidth + 3 + 4) : 0;
			int val = bounds.Width - 3 - num - num2;
			this.textRect = new Rectangle(bounds.X, bounds.Y + 1, Math.Min(val, 4 + size.Width), 4 + size.Height);
			if (alignRight)
			{
				this.textRect.X = bounds.Right - this.textRect.Width;
				this.backButtonRect.X = bounds.X + 12 + detailsButtonWidth;
				this.downButtonRect.X = bounds.X + 6;
			}
			g.FillRectangle(this.backBrush, bounds);
			if (this.backButtonVisible)
			{
				this.PaintBackButton(g, this.backButtonRect, alignRight);
				if (this.backActive && this.lastMouseLocation == DataGridCaption.CaptionLocation.BackButton)
				{
					this.backButtonRect.Inflate(1, 1);
					ControlPaint.DrawBorder3D(g, this.backButtonRect, this.backPressed ? Border3DStyle.SunkenInner : Border3DStyle.RaisedInner);
				}
			}
			this.PaintText(g, this.textRect, alignRight);
			if (this.downButtonVisible && !this.dataGrid.ParentRowsIsEmpty())
			{
				this.PaintDownButton(g, this.downButtonRect);
				if (this.lastMouseLocation == DataGridCaption.CaptionLocation.DownButton)
				{
					this.downButtonRect.Inflate(1, 1);
					ControlPaint.DrawBorder3D(g, this.downButtonRect, this.downPressed ? Border3DStyle.SunkenInner : Border3DStyle.RaisedInner);
				}
			}
		}

		// Token: 0x060012EA RID: 4842 RVA: 0x00047FD4 File Offset: 0x000461D4
		private void PaintIcon(Graphics g, Rectangle bounds, Bitmap b)
		{
			ImageAttributes imageAttributes = new ImageAttributes();
			imageAttributes.SetRemapTable(DataGridCaption.colorMap, ColorAdjustType.Bitmap);
			g.DrawImage(b, bounds, 0, 0, bounds.Width, bounds.Height, GraphicsUnit.Pixel, imageAttributes);
			imageAttributes.Dispose();
		}

		// Token: 0x060012EB RID: 4843 RVA: 0x00048014 File Offset: 0x00046214
		private void PaintBackButton(Graphics g, Rectangle bounds, bool alignRight)
		{
			Bitmap backButtonBmp = this.GetBackButtonBmp(alignRight);
			Bitmap obj = backButtonBmp;
			lock (obj)
			{
				this.PaintIcon(g, bounds, backButtonBmp);
			}
		}

		// Token: 0x060012EC RID: 4844 RVA: 0x0004805C File Offset: 0x0004625C
		private void PaintDownButton(Graphics g, Rectangle bounds)
		{
			Bitmap detailsBmp = this.GetDetailsBmp();
			Bitmap obj = detailsBmp;
			lock (obj)
			{
				this.PaintIcon(g, bounds, detailsBmp);
			}
		}

		// Token: 0x060012ED RID: 4845 RVA: 0x000480A4 File Offset: 0x000462A4
		private void PaintText(Graphics g, Rectangle bounds, bool alignToRight)
		{
			Rectangle rectangle = bounds;
			if (rectangle.Width <= 0 || rectangle.Height <= 0)
			{
				return;
			}
			if (this.textBorderVisible)
			{
				g.DrawRectangle(this.textBorderPen, rectangle.X, rectangle.Y, rectangle.Width - 1, rectangle.Height - 1);
				rectangle.Inflate(-1, -1);
			}
			Rectangle rect = rectangle;
			rect.Height = 2;
			g.FillRectangle(this.backBrush, rect);
			rect.Y = rectangle.Bottom - 2;
			g.FillRectangle(this.backBrush, rect);
			rect = new Rectangle(rectangle.X, rectangle.Y + 2, 2, rectangle.Height - 4);
			g.FillRectangle(this.backBrush, rect);
			rect.X = rectangle.Right - 2;
			g.FillRectangle(this.backBrush, rect);
			rectangle.Inflate(-2, -2);
			g.FillRectangle(this.backBrush, rectangle);
			StringFormat stringFormat = new StringFormat();
			if (alignToRight)
			{
				stringFormat.FormatFlags |= StringFormatFlags.DirectionRightToLeft;
				stringFormat.Alignment = StringAlignment.Far;
			}
			g.DrawString(this.text, this.Font, this.foreBrush, rectangle, stringFormat);
			stringFormat.Dispose();
		}

		// Token: 0x060012EE RID: 4846 RVA: 0x000481E0 File Offset: 0x000463E0
		private DataGridCaption.CaptionLocation FindLocation(int x, int y)
		{
			if (!this.backButtonRect.IsEmpty && this.backButtonRect.Contains(x, y))
			{
				return DataGridCaption.CaptionLocation.BackButton;
			}
			if (!this.downButtonRect.IsEmpty && this.downButtonRect.Contains(x, y))
			{
				return DataGridCaption.CaptionLocation.DownButton;
			}
			if (!this.textRect.IsEmpty && this.textRect.Contains(x, y))
			{
				return DataGridCaption.CaptionLocation.Text;
			}
			return DataGridCaption.CaptionLocation.Nowhere;
		}

		// Token: 0x1700049B RID: 1179
		// (get) Token: 0x060012EF RID: 4847 RVA: 0x00048248 File Offset: 0x00046448
		// (set) Token: 0x060012F0 RID: 4848 RVA: 0x00048250 File Offset: 0x00046450
		private bool DownButtonDown
		{
			get
			{
				return this.downButtonDown;
			}
			set
			{
				if (this.downButtonDown != value)
				{
					this.downButtonDown = value;
					this.InvalidateLocation(DataGridCaption.CaptionLocation.DownButton);
				}
			}
		}

		// Token: 0x060012F1 RID: 4849 RVA: 0x00048269 File Offset: 0x00046469
		internal bool GetDownButtonDirection()
		{
			return this.DownButtonDown;
		}

		// Token: 0x060012F2 RID: 4850 RVA: 0x00048274 File Offset: 0x00046474
		internal void MouseDown(int x, int y)
		{
			DataGridCaption.CaptionLocation loc = this.FindLocation(x, y);
			switch (loc)
			{
			case DataGridCaption.CaptionLocation.BackButton:
				this.backPressed = true;
				this.InvalidateLocation(loc);
				return;
			case DataGridCaption.CaptionLocation.DownButton:
				this.downPressed = true;
				this.InvalidateLocation(loc);
				return;
			case DataGridCaption.CaptionLocation.Text:
				this.OnCaptionClicked(EventArgs.Empty);
				return;
			default:
				return;
			}
		}

		// Token: 0x060012F3 RID: 4851 RVA: 0x000482C8 File Offset: 0x000464C8
		internal void MouseUp(int x, int y)
		{
			DataGridCaption.CaptionLocation captionLocation = this.FindLocation(x, y);
			if (captionLocation != DataGridCaption.CaptionLocation.BackButton)
			{
				if (captionLocation == DataGridCaption.CaptionLocation.DownButton && this.downPressed)
				{
					this.downPressed = false;
					this.OnDownClicked(EventArgs.Empty);
					return;
				}
			}
			else if (this.backPressed)
			{
				this.backPressed = false;
				this.OnBackwardClicked(EventArgs.Empty);
			}
		}

		// Token: 0x060012F4 RID: 4852 RVA: 0x0004831C File Offset: 0x0004651C
		internal void MouseLeft()
		{
			DataGridCaption.CaptionLocation loc = this.lastMouseLocation;
			this.lastMouseLocation = DataGridCaption.CaptionLocation.Nowhere;
			this.InvalidateLocation(loc);
		}

		// Token: 0x060012F5 RID: 4853 RVA: 0x00048340 File Offset: 0x00046540
		internal void MouseOver(int x, int y)
		{
			DataGridCaption.CaptionLocation loc = this.FindLocation(x, y);
			this.InvalidateLocation(this.lastMouseLocation);
			this.InvalidateLocation(loc);
			this.lastMouseLocation = loc;
		}

		// Token: 0x060012F6 RID: 4854 RVA: 0x00048370 File Offset: 0x00046570
		protected virtual void RaiseEvent(object key, EventArgs e)
		{
			Delegate eventHandler = this.GetEventHandler(key);
			if (eventHandler != null)
			{
				((EventHandler)eventHandler)(this, e);
			}
		}

		// Token: 0x060012F7 RID: 4855 RVA: 0x00048398 File Offset: 0x00046598
		protected virtual void RemoveEventHandler(object key, Delegate handler)
		{
			lock (this)
			{
				if (handler != null)
				{
					DataGridCaption.EventEntry next = this.eventList;
					DataGridCaption.EventEntry eventEntry = null;
					while (next != null)
					{
						if (next.key == key)
						{
							next.handler = Delegate.Remove(next.handler, handler);
							if (next.handler == null)
							{
								if (eventEntry == null)
								{
									this.eventList = next.next;
								}
								else
								{
									eventEntry.next = next.next;
								}
							}
							break;
						}
						eventEntry = next;
						next = next.next;
					}
				}
			}
		}

		// Token: 0x060012F8 RID: 4856 RVA: 0x0004842C File Offset: 0x0004662C
		protected virtual void RemoveEventHandlers()
		{
			this.eventList = null;
		}

		// Token: 0x060012F9 RID: 4857 RVA: 0x00048435 File Offset: 0x00046635
		internal void SetDownButtonDirection(bool pointDown)
		{
			this.DownButtonDown = pointDown;
		}

		// Token: 0x060012FA RID: 4858 RVA: 0x0004843E File Offset: 0x0004663E
		internal bool ToggleDownButtonDirection()
		{
			this.DownButtonDown = !this.DownButtonDown;
			return this.DownButtonDown;
		}

		// Token: 0x04000963 RID: 2403
		internal EventHandlerList events;

		// Token: 0x04000964 RID: 2404
		private const int xOffset = 3;

		// Token: 0x04000965 RID: 2405
		private const int yOffset = 1;

		// Token: 0x04000966 RID: 2406
		private const int textPadding = 2;

		// Token: 0x04000967 RID: 2407
		private const int buttonToText = 4;

		// Token: 0x04000968 RID: 2408
		private static ColorMap[] colorMap = new ColorMap[]
		{
			new ColorMap()
		};

		// Token: 0x04000969 RID: 2409
		private static readonly Point minimumBounds = new Point(50, 30);

		// Token: 0x0400096A RID: 2410
		private DataGrid dataGrid;

		// Token: 0x0400096B RID: 2411
		private bool backButtonVisible;

		// Token: 0x0400096C RID: 2412
		private bool downButtonVisible;

		// Token: 0x0400096D RID: 2413
		private SolidBrush backBrush = DataGridCaption.DefaultBackBrush;

		// Token: 0x0400096E RID: 2414
		private SolidBrush foreBrush = DataGridCaption.DefaultForeBrush;

		// Token: 0x0400096F RID: 2415
		private Pen textBorderPen = DataGridCaption.DefaultTextBorderPen;

		// Token: 0x04000970 RID: 2416
		private string text = "";

		// Token: 0x04000971 RID: 2417
		private bool textBorderVisible;

		// Token: 0x04000972 RID: 2418
		private Font textFont;

		// Token: 0x04000973 RID: 2419
		private Font dataGridFont;

		// Token: 0x04000974 RID: 2420
		private bool backActive;

		// Token: 0x04000975 RID: 2421
		private bool downActive;

		// Token: 0x04000976 RID: 2422
		private bool backPressed;

		// Token: 0x04000977 RID: 2423
		private bool downPressed;

		// Token: 0x04000978 RID: 2424
		private bool downButtonDown;

		// Token: 0x04000979 RID: 2425
		private static Bitmap leftButtonBitmap;

		// Token: 0x0400097A RID: 2426
		private static Bitmap leftButtonBitmap_bidi;

		// Token: 0x0400097B RID: 2427
		private static Bitmap magnifyingGlassBitmap;

		// Token: 0x0400097C RID: 2428
		private Rectangle backButtonRect;

		// Token: 0x0400097D RID: 2429
		private Rectangle downButtonRect;

		// Token: 0x0400097E RID: 2430
		private Rectangle textRect;

		// Token: 0x0400097F RID: 2431
		private DataGridCaption.CaptionLocation lastMouseLocation;

		// Token: 0x04000980 RID: 2432
		private DataGridCaption.EventEntry eventList;

		// Token: 0x04000981 RID: 2433
		private static readonly object EVENT_BACKWARDCLICKED = new object();

		// Token: 0x04000982 RID: 2434
		private static readonly object EVENT_DOWNCLICKED = new object();

		// Token: 0x04000983 RID: 2435
		private static readonly object EVENT_CAPTIONCLICKED = new object();

		// Token: 0x0200058F RID: 1423
		internal enum CaptionLocation
		{
			// Token: 0x04003898 RID: 14488
			Nowhere,
			// Token: 0x04003899 RID: 14489
			BackButton,
			// Token: 0x0400389A RID: 14490
			DownButton,
			// Token: 0x0400389B RID: 14491
			Text
		}

		// Token: 0x02000590 RID: 1424
		private sealed class EventEntry
		{
			// Token: 0x060057F7 RID: 22519 RVA: 0x00172494 File Offset: 0x00170694
			internal EventEntry(DataGridCaption.EventEntry next, object key, Delegate handler)
			{
				this.next = next;
				this.key = key;
				this.handler = handler;
			}

			// Token: 0x0400389C RID: 14492
			internal DataGridCaption.EventEntry next;

			// Token: 0x0400389D RID: 14493
			internal object key;

			// Token: 0x0400389E RID: 14494
			internal Delegate handler;
		}
	}
}
