using System;
using System.Globalization;
using System.Text;

namespace System.Windows.Documents
{
	// Token: 0x020003C0 RID: 960
	internal class CellFormat
	{
		// Token: 0x06003386 RID: 13190 RVA: 0x000E65AC File Offset: 0x000E47AC
		internal CellFormat()
		{
			this.BorderLeft = new BorderFormat();
			this.BorderRight = new BorderFormat();
			this.BorderBottom = new BorderFormat();
			this.BorderTop = new BorderFormat();
			this.Width = new CellWidth();
			this.SetDefaults();
			this.IsPending = true;
		}

		// Token: 0x06003387 RID: 13191 RVA: 0x000E6604 File Offset: 0x000E4804
		internal CellFormat(CellFormat cf)
		{
			this.CellX = cf.CellX;
			this.IsCellXSet = cf.IsCellXSet;
			this.Width = new CellWidth(cf.Width);
			this.CB = cf.CB;
			this.CF = cf.CF;
			this.Shading = cf.Shading;
			this.PaddingTop = cf.PaddingTop;
			this.PaddingBottom = cf.PaddingBottom;
			this.PaddingRight = cf.PaddingRight;
			this.PaddingLeft = cf.PaddingLeft;
			this.BorderLeft = new BorderFormat(cf.BorderLeft);
			this.BorderRight = new BorderFormat(cf.BorderRight);
			this.BorderBottom = new BorderFormat(cf.BorderBottom);
			this.BorderTop = new BorderFormat(cf.BorderTop);
			this.SpacingTop = cf.SpacingTop;
			this.SpacingBottom = cf.SpacingBottom;
			this.SpacingRight = cf.SpacingRight;
			this.SpacingLeft = cf.SpacingLeft;
			this.VAlign = VAlign.AlignTop;
			this.IsPending = true;
			this.IsHMerge = cf.IsHMerge;
			this.IsHMergeFirst = cf.IsHMergeFirst;
			this.IsVMerge = cf.IsVMerge;
			this.IsVMergeFirst = cf.IsVMergeFirst;
		}

		// Token: 0x17000D23 RID: 3363
		// (get) Token: 0x06003388 RID: 13192 RVA: 0x000E6746 File Offset: 0x000E4946
		// (set) Token: 0x06003389 RID: 13193 RVA: 0x000E674E File Offset: 0x000E494E
		internal long CB
		{
			get
			{
				return this._cb;
			}
			set
			{
				this._cb = value;
			}
		}

		// Token: 0x17000D24 RID: 3364
		// (get) Token: 0x0600338A RID: 13194 RVA: 0x000E6757 File Offset: 0x000E4957
		// (set) Token: 0x0600338B RID: 13195 RVA: 0x000E675F File Offset: 0x000E495F
		internal long CF
		{
			get
			{
				return this._cf;
			}
			set
			{
				this._cf = value;
			}
		}

		// Token: 0x17000D25 RID: 3365
		// (get) Token: 0x0600338C RID: 13196 RVA: 0x000E6768 File Offset: 0x000E4968
		// (set) Token: 0x0600338D RID: 13197 RVA: 0x000E6770 File Offset: 0x000E4970
		internal long Shading
		{
			get
			{
				return this._nShading;
			}
			set
			{
				this._nShading = Validators.MakeValidShading(value);
			}
		}

		// Token: 0x17000D26 RID: 3366
		// (get) Token: 0x0600338E RID: 13198 RVA: 0x000E677E File Offset: 0x000E497E
		// (set) Token: 0x0600338F RID: 13199 RVA: 0x000E6786 File Offset: 0x000E4986
		internal long PaddingLeft
		{
			get
			{
				return this._padL;
			}
			set
			{
				this._padL = value;
			}
		}

		// Token: 0x17000D27 RID: 3367
		// (get) Token: 0x06003390 RID: 13200 RVA: 0x000E678F File Offset: 0x000E498F
		// (set) Token: 0x06003391 RID: 13201 RVA: 0x000E6797 File Offset: 0x000E4997
		internal long PaddingRight
		{
			get
			{
				return this._padR;
			}
			set
			{
				this._padR = value;
			}
		}

		// Token: 0x17000D28 RID: 3368
		// (get) Token: 0x06003392 RID: 13202 RVA: 0x000E67A0 File Offset: 0x000E49A0
		// (set) Token: 0x06003393 RID: 13203 RVA: 0x000E67A8 File Offset: 0x000E49A8
		internal long PaddingTop
		{
			get
			{
				return this._padT;
			}
			set
			{
				this._padT = value;
			}
		}

		// Token: 0x17000D29 RID: 3369
		// (get) Token: 0x06003394 RID: 13204 RVA: 0x000E67B1 File Offset: 0x000E49B1
		// (set) Token: 0x06003395 RID: 13205 RVA: 0x000E67B9 File Offset: 0x000E49B9
		internal long PaddingBottom
		{
			get
			{
				return this._padB;
			}
			set
			{
				this._padB = value;
			}
		}

		// Token: 0x17000D2A RID: 3370
		// (get) Token: 0x06003396 RID: 13206 RVA: 0x000E67C2 File Offset: 0x000E49C2
		// (set) Token: 0x06003397 RID: 13207 RVA: 0x000E67CA File Offset: 0x000E49CA
		internal BorderFormat BorderTop
		{
			get
			{
				return this._brdT;
			}
			set
			{
				this._brdT = value;
			}
		}

		// Token: 0x17000D2B RID: 3371
		// (get) Token: 0x06003398 RID: 13208 RVA: 0x000E67D3 File Offset: 0x000E49D3
		// (set) Token: 0x06003399 RID: 13209 RVA: 0x000E67DB File Offset: 0x000E49DB
		internal BorderFormat BorderBottom
		{
			get
			{
				return this._brdB;
			}
			set
			{
				this._brdB = value;
			}
		}

		// Token: 0x17000D2C RID: 3372
		// (get) Token: 0x0600339A RID: 13210 RVA: 0x000E67E4 File Offset: 0x000E49E4
		// (set) Token: 0x0600339B RID: 13211 RVA: 0x000E67EC File Offset: 0x000E49EC
		internal BorderFormat BorderLeft
		{
			get
			{
				return this._brdL;
			}
			set
			{
				this._brdL = value;
			}
		}

		// Token: 0x17000D2D RID: 3373
		// (get) Token: 0x0600339C RID: 13212 RVA: 0x000E67F5 File Offset: 0x000E49F5
		// (set) Token: 0x0600339D RID: 13213 RVA: 0x000E67FD File Offset: 0x000E49FD
		internal BorderFormat BorderRight
		{
			get
			{
				return this._brdR;
			}
			set
			{
				this._brdR = value;
			}
		}

		// Token: 0x17000D2E RID: 3374
		// (get) Token: 0x0600339E RID: 13214 RVA: 0x000E6806 File Offset: 0x000E4A06
		// (set) Token: 0x0600339F RID: 13215 RVA: 0x000E680E File Offset: 0x000E4A0E
		internal CellWidth Width
		{
			get
			{
				return this._width;
			}
			set
			{
				this._width = value;
			}
		}

		// Token: 0x17000D2F RID: 3375
		// (get) Token: 0x060033A0 RID: 13216 RVA: 0x000E6817 File Offset: 0x000E4A17
		// (set) Token: 0x060033A1 RID: 13217 RVA: 0x000E681F File Offset: 0x000E4A1F
		internal long CellX
		{
			get
			{
				return this._nCellX;
			}
			set
			{
				this._nCellX = value;
				this._fCellXSet = true;
			}
		}

		// Token: 0x17000D30 RID: 3376
		// (get) Token: 0x060033A2 RID: 13218 RVA: 0x000E682F File Offset: 0x000E4A2F
		// (set) Token: 0x060033A3 RID: 13219 RVA: 0x000E6837 File Offset: 0x000E4A37
		internal bool IsCellXSet
		{
			get
			{
				return this._fCellXSet;
			}
			set
			{
				this._fCellXSet = value;
			}
		}

		// Token: 0x17000D31 RID: 3377
		// (set) Token: 0x060033A4 RID: 13220 RVA: 0x000E6840 File Offset: 0x000E4A40
		internal VAlign VAlign
		{
			set
			{
				this._valign = value;
			}
		}

		// Token: 0x17000D32 RID: 3378
		// (get) Token: 0x060033A5 RID: 13221 RVA: 0x000E6849 File Offset: 0x000E4A49
		// (set) Token: 0x060033A6 RID: 13222 RVA: 0x000E6851 File Offset: 0x000E4A51
		internal long SpacingTop
		{
			get
			{
				return this._spaceT;
			}
			set
			{
				this._spaceT = value;
			}
		}

		// Token: 0x17000D33 RID: 3379
		// (get) Token: 0x060033A7 RID: 13223 RVA: 0x000E685A File Offset: 0x000E4A5A
		// (set) Token: 0x060033A8 RID: 13224 RVA: 0x000E6862 File Offset: 0x000E4A62
		internal long SpacingLeft
		{
			get
			{
				return this._spaceL;
			}
			set
			{
				this._spaceL = value;
			}
		}

		// Token: 0x17000D34 RID: 3380
		// (get) Token: 0x060033A9 RID: 13225 RVA: 0x000E686B File Offset: 0x000E4A6B
		// (set) Token: 0x060033AA RID: 13226 RVA: 0x000E6873 File Offset: 0x000E4A73
		internal long SpacingBottom
		{
			get
			{
				return this._spaceB;
			}
			set
			{
				this._spaceB = value;
			}
		}

		// Token: 0x17000D35 RID: 3381
		// (get) Token: 0x060033AB RID: 13227 RVA: 0x000E687C File Offset: 0x000E4A7C
		// (set) Token: 0x060033AC RID: 13228 RVA: 0x000E6884 File Offset: 0x000E4A84
		internal long SpacingRight
		{
			get
			{
				return this._spaceR;
			}
			set
			{
				this._spaceR = value;
			}
		}

		// Token: 0x17000D36 RID: 3382
		// (get) Token: 0x060033AD RID: 13229 RVA: 0x000E688D File Offset: 0x000E4A8D
		// (set) Token: 0x060033AE RID: 13230 RVA: 0x000E6895 File Offset: 0x000E4A95
		internal bool IsPending
		{
			get
			{
				return this._fPending;
			}
			set
			{
				this._fPending = value;
			}
		}

		// Token: 0x17000D37 RID: 3383
		// (get) Token: 0x060033AF RID: 13231 RVA: 0x000E689E File Offset: 0x000E4A9E
		// (set) Token: 0x060033B0 RID: 13232 RVA: 0x000E68A6 File Offset: 0x000E4AA6
		internal bool IsHMerge
		{
			get
			{
				return this._fHMerge;
			}
			set
			{
				this._fHMerge = value;
			}
		}

		// Token: 0x17000D38 RID: 3384
		// (get) Token: 0x060033B1 RID: 13233 RVA: 0x000E68AF File Offset: 0x000E4AAF
		// (set) Token: 0x060033B2 RID: 13234 RVA: 0x000E68B7 File Offset: 0x000E4AB7
		internal bool IsHMergeFirst
		{
			get
			{
				return this._fHMergeFirst;
			}
			set
			{
				this._fHMergeFirst = value;
			}
		}

		// Token: 0x17000D39 RID: 3385
		// (get) Token: 0x060033B3 RID: 13235 RVA: 0x000E68C0 File Offset: 0x000E4AC0
		// (set) Token: 0x060033B4 RID: 13236 RVA: 0x000E68C8 File Offset: 0x000E4AC8
		internal bool IsVMerge
		{
			get
			{
				return this._fVMerge;
			}
			set
			{
				this._fVMerge = value;
			}
		}

		// Token: 0x17000D3A RID: 3386
		// (get) Token: 0x060033B5 RID: 13237 RVA: 0x000E68D1 File Offset: 0x000E4AD1
		// (set) Token: 0x060033B6 RID: 13238 RVA: 0x000E68D9 File Offset: 0x000E4AD9
		internal bool IsVMergeFirst
		{
			get
			{
				return this._fVMergeFirst;
			}
			set
			{
				this._fVMergeFirst = value;
			}
		}

		// Token: 0x17000D3B RID: 3387
		// (get) Token: 0x060033B7 RID: 13239 RVA: 0x000E68E2 File Offset: 0x000E4AE2
		internal bool HasBorder
		{
			get
			{
				return this.BorderLeft.EffectiveWidth > 0L || this.BorderRight.EffectiveWidth > 0L || this.BorderTop.EffectiveWidth > 0L || this.BorderBottom.EffectiveWidth > 0L;
			}
		}

		// Token: 0x17000D3C RID: 3388
		// (get) Token: 0x060033B8 RID: 13240 RVA: 0x000E6924 File Offset: 0x000E4B24
		internal string RTFEncodingForWidth
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append("\\clftsWidth");
				stringBuilder.Append(((int)this.Width.Type).ToString(CultureInfo.InvariantCulture));
				stringBuilder.Append("\\clwWidth");
				stringBuilder.Append(this.Width.Value.ToString(CultureInfo.InvariantCulture));
				stringBuilder.Append("\\cellx");
				stringBuilder.Append(this.CellX.ToString(CultureInfo.InvariantCulture));
				return stringBuilder.ToString();
			}
		}

		// Token: 0x060033B9 RID: 13241 RVA: 0x000E69BC File Offset: 0x000E4BBC
		internal void SetDefaults()
		{
			this.CellX = -1L;
			this.IsCellXSet = false;
			this.Width.SetDefaults();
			this.CB = -1L;
			this.CF = -1L;
			this.Shading = -1L;
			this.PaddingTop = 0L;
			this.PaddingBottom = 0L;
			this.PaddingRight = 0L;
			this.PaddingLeft = 0L;
			this.BorderLeft.SetDefaults();
			this.BorderRight.SetDefaults();
			this.BorderBottom.SetDefaults();
			this.BorderTop.SetDefaults();
			this.SpacingTop = 0L;
			this.SpacingBottom = 0L;
			this.SpacingRight = 0L;
			this.SpacingLeft = 0L;
			this.VAlign = VAlign.AlignTop;
			this.IsHMerge = false;
			this.IsHMergeFirst = false;
			this.IsVMerge = false;
			this.IsVMergeFirst = false;
		}

		// Token: 0x060033BA RID: 13242 RVA: 0x000E6A8C File Offset: 0x000E4C8C
		internal string GetBorderAttributeString(ConverterState converterState)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(" BorderThickness=\"");
			stringBuilder.Append(Converters.TwipToPositiveVisiblePxString((double)this.BorderLeft.EffectiveWidth));
			stringBuilder.Append(",");
			stringBuilder.Append(Converters.TwipToPositiveVisiblePxString((double)this.BorderTop.EffectiveWidth));
			stringBuilder.Append(",");
			stringBuilder.Append(Converters.TwipToPositiveVisiblePxString((double)this.BorderRight.EffectiveWidth));
			stringBuilder.Append(",");
			stringBuilder.Append(Converters.TwipToPositiveVisiblePxString((double)this.BorderBottom.EffectiveWidth));
			stringBuilder.Append("\"");
			ColorTableEntry colorTableEntry = null;
			if (this.BorderLeft.CF >= 0L)
			{
				colorTableEntry = converterState.ColorTable.EntryAt((int)this.BorderLeft.CF);
			}
			if (colorTableEntry != null)
			{
				stringBuilder.Append(" BorderBrush=\"");
				stringBuilder.Append(colorTableEntry.Color.ToString(CultureInfo.InvariantCulture));
				stringBuilder.Append("\"");
			}
			else
			{
				stringBuilder.Append(" BorderBrush=\"#FF000000\"");
			}
			return stringBuilder.ToString();
		}

		// Token: 0x060033BB RID: 13243 RVA: 0x000E6BB0 File Offset: 0x000E4DB0
		internal string GetPaddingAttributeString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(" Padding=\"");
			stringBuilder.Append(Converters.TwipToPositivePxString((double)this.PaddingLeft));
			stringBuilder.Append(",");
			stringBuilder.Append(Converters.TwipToPositivePxString((double)this.PaddingTop));
			stringBuilder.Append(",");
			stringBuilder.Append(Converters.TwipToPositivePxString((double)this.PaddingRight));
			stringBuilder.Append(",");
			stringBuilder.Append(Converters.TwipToPositivePxString((double)this.PaddingBottom));
			stringBuilder.Append("\"");
			return stringBuilder.ToString();
		}

		// Token: 0x0400248C RID: 9356
		private long _cb;

		// Token: 0x0400248D RID: 9357
		private long _cf;

		// Token: 0x0400248E RID: 9358
		private long _nShading;

		// Token: 0x0400248F RID: 9359
		private long _padT;

		// Token: 0x04002490 RID: 9360
		private long _padB;

		// Token: 0x04002491 RID: 9361
		private long _padR;

		// Token: 0x04002492 RID: 9362
		private long _padL;

		// Token: 0x04002493 RID: 9363
		private long _spaceT;

		// Token: 0x04002494 RID: 9364
		private long _spaceB;

		// Token: 0x04002495 RID: 9365
		private long _spaceR;

		// Token: 0x04002496 RID: 9366
		private long _spaceL;

		// Token: 0x04002497 RID: 9367
		private long _nCellX;

		// Token: 0x04002498 RID: 9368
		private CellWidth _width;

		// Token: 0x04002499 RID: 9369
		private VAlign _valign;

		// Token: 0x0400249A RID: 9370
		private BorderFormat _brdL;

		// Token: 0x0400249B RID: 9371
		private BorderFormat _brdR;

		// Token: 0x0400249C RID: 9372
		private BorderFormat _brdT;

		// Token: 0x0400249D RID: 9373
		private BorderFormat _brdB;

		// Token: 0x0400249E RID: 9374
		private bool _fPending;

		// Token: 0x0400249F RID: 9375
		private bool _fHMerge;

		// Token: 0x040024A0 RID: 9376
		private bool _fHMergeFirst;

		// Token: 0x040024A1 RID: 9377
		private bool _fVMerge;

		// Token: 0x040024A2 RID: 9378
		private bool _fVMergeFirst;

		// Token: 0x040024A3 RID: 9379
		private bool _fCellXSet;
	}
}
