using System;
using System.Diagnostics;

namespace System.ComponentModel
{
	// Token: 0x020000F6 RID: 246
	internal static class CompModSwitches
	{
		// Token: 0x1700013E RID: 318
		// (get) Token: 0x060003BF RID: 959 RVA: 0x0000BE87 File Offset: 0x0000A087
		public static TraceSwitch ActiveX
		{
			get
			{
				if (CompModSwitches.activeX == null)
				{
					CompModSwitches.activeX = new TraceSwitch("ActiveX", "Debug ActiveX sourcing");
				}
				return CompModSwitches.activeX;
			}
		}

		// Token: 0x1700013F RID: 319
		// (get) Token: 0x060003C0 RID: 960 RVA: 0x0000BEA9 File Offset: 0x0000A0A9
		public static TraceSwitch DataCursor
		{
			get
			{
				if (CompModSwitches.dataCursor == null)
				{
					CompModSwitches.dataCursor = new TraceSwitch("Microsoft.WFC.Data.DataCursor", "DataCursor");
				}
				return CompModSwitches.dataCursor;
			}
		}

		// Token: 0x17000140 RID: 320
		// (get) Token: 0x060003C1 RID: 961 RVA: 0x0000BECB File Offset: 0x0000A0CB
		public static TraceSwitch DataGridCursor
		{
			get
			{
				if (CompModSwitches.dataGridCursor == null)
				{
					CompModSwitches.dataGridCursor = new TraceSwitch("DataGridCursor", "DataGrid cursor tracing");
				}
				return CompModSwitches.dataGridCursor;
			}
		}

		// Token: 0x17000141 RID: 321
		// (get) Token: 0x060003C2 RID: 962 RVA: 0x0000BEED File Offset: 0x0000A0ED
		public static TraceSwitch DataGridEditing
		{
			get
			{
				if (CompModSwitches.dataGridEditing == null)
				{
					CompModSwitches.dataGridEditing = new TraceSwitch("DataGridEditing", "DataGrid edit related tracing");
				}
				return CompModSwitches.dataGridEditing;
			}
		}

		// Token: 0x17000142 RID: 322
		// (get) Token: 0x060003C3 RID: 963 RVA: 0x0000BF0F File Offset: 0x0000A10F
		public static TraceSwitch DataGridKeys
		{
			get
			{
				if (CompModSwitches.dataGridKeys == null)
				{
					CompModSwitches.dataGridKeys = new TraceSwitch("DataGridKeys", "DataGrid keystroke management tracing");
				}
				return CompModSwitches.dataGridKeys;
			}
		}

		// Token: 0x17000143 RID: 323
		// (get) Token: 0x060003C4 RID: 964 RVA: 0x0000BF31 File Offset: 0x0000A131
		public static TraceSwitch DataGridLayout
		{
			get
			{
				if (CompModSwitches.dataGridLayout == null)
				{
					CompModSwitches.dataGridLayout = new TraceSwitch("DataGridLayout", "DataGrid layout tracing");
				}
				return CompModSwitches.dataGridLayout;
			}
		}

		// Token: 0x17000144 RID: 324
		// (get) Token: 0x060003C5 RID: 965 RVA: 0x0000BF53 File Offset: 0x0000A153
		public static TraceSwitch DataGridPainting
		{
			get
			{
				if (CompModSwitches.dataGridPainting == null)
				{
					CompModSwitches.dataGridPainting = new TraceSwitch("DataGridPainting", "DataGrid Painting related tracing");
				}
				return CompModSwitches.dataGridPainting;
			}
		}

		// Token: 0x17000145 RID: 325
		// (get) Token: 0x060003C6 RID: 966 RVA: 0x0000BF75 File Offset: 0x0000A175
		public static TraceSwitch DataGridParents
		{
			get
			{
				if (CompModSwitches.dataGridParents == null)
				{
					CompModSwitches.dataGridParents = new TraceSwitch("DataGridParents", "DataGrid parent rows");
				}
				return CompModSwitches.dataGridParents;
			}
		}

		// Token: 0x17000146 RID: 326
		// (get) Token: 0x060003C7 RID: 967 RVA: 0x0000BF97 File Offset: 0x0000A197
		public static TraceSwitch DataGridScrolling
		{
			get
			{
				if (CompModSwitches.dataGridScrolling == null)
				{
					CompModSwitches.dataGridScrolling = new TraceSwitch("DataGridScrolling", "DataGrid scrolling");
				}
				return CompModSwitches.dataGridScrolling;
			}
		}

		// Token: 0x17000147 RID: 327
		// (get) Token: 0x060003C8 RID: 968 RVA: 0x0000BFB9 File Offset: 0x0000A1B9
		public static TraceSwitch DataGridSelection
		{
			get
			{
				if (CompModSwitches.dataGridSelection == null)
				{
					CompModSwitches.dataGridSelection = new TraceSwitch("DataGridSelection", "DataGrid selection management tracing");
				}
				return CompModSwitches.dataGridSelection;
			}
		}

		// Token: 0x17000148 RID: 328
		// (get) Token: 0x060003C9 RID: 969 RVA: 0x0000BFDB File Offset: 0x0000A1DB
		public static TraceSwitch DataObject
		{
			get
			{
				if (CompModSwitches.dataObject == null)
				{
					CompModSwitches.dataObject = new TraceSwitch("DataObject", "Enable tracing for the DataObject class.");
				}
				return CompModSwitches.dataObject;
			}
		}

		// Token: 0x17000149 RID: 329
		// (get) Token: 0x060003CA RID: 970 RVA: 0x0000BFFD File Offset: 0x0000A1FD
		public static TraceSwitch DataView
		{
			get
			{
				if (CompModSwitches.dataView == null)
				{
					CompModSwitches.dataView = new TraceSwitch("DataView", "DataView");
				}
				return CompModSwitches.dataView;
			}
		}

		// Token: 0x1700014A RID: 330
		// (get) Token: 0x060003CB RID: 971 RVA: 0x0000C01F File Offset: 0x0000A21F
		public static TraceSwitch DebugGridView
		{
			get
			{
				if (CompModSwitches.debugGridView == null)
				{
					CompModSwitches.debugGridView = new TraceSwitch("PSDEBUGGRIDVIEW", "Debug PropertyGridView");
				}
				return CompModSwitches.debugGridView;
			}
		}

		// Token: 0x1700014B RID: 331
		// (get) Token: 0x060003CC RID: 972 RVA: 0x0000C041 File Offset: 0x0000A241
		public static TraceSwitch DGCaptionPaint
		{
			get
			{
				if (CompModSwitches.dgCaptionPaint == null)
				{
					CompModSwitches.dgCaptionPaint = new TraceSwitch("DGCaptionPaint", "DataGridCaption");
				}
				return CompModSwitches.dgCaptionPaint;
			}
		}

		// Token: 0x1700014C RID: 332
		// (get) Token: 0x060003CD RID: 973 RVA: 0x0000C063 File Offset: 0x0000A263
		public static TraceSwitch DGEditColumnEditing
		{
			get
			{
				if (CompModSwitches.dgEditColumnEditing == null)
				{
					CompModSwitches.dgEditColumnEditing = new TraceSwitch("DGEditColumnEditing", "Editing related tracing");
				}
				return CompModSwitches.dgEditColumnEditing;
			}
		}

		// Token: 0x1700014D RID: 333
		// (get) Token: 0x060003CE RID: 974 RVA: 0x0000C085 File Offset: 0x0000A285
		public static TraceSwitch DGRelationShpRowLayout
		{
			get
			{
				if (CompModSwitches.dgRelationShpRowLayout == null)
				{
					CompModSwitches.dgRelationShpRowLayout = new TraceSwitch("DGRelationShpRowLayout", "Relationship row layout");
				}
				return CompModSwitches.dgRelationShpRowLayout;
			}
		}

		// Token: 0x1700014E RID: 334
		// (get) Token: 0x060003CF RID: 975 RVA: 0x0000C0A7 File Offset: 0x0000A2A7
		public static TraceSwitch DGRelationShpRowPaint
		{
			get
			{
				if (CompModSwitches.dgRelationShpRowPaint == null)
				{
					CompModSwitches.dgRelationShpRowPaint = new TraceSwitch("DGRelationShpRowPaint", "Relationship row painting");
				}
				return CompModSwitches.dgRelationShpRowPaint;
			}
		}

		// Token: 0x1700014F RID: 335
		// (get) Token: 0x060003D0 RID: 976 RVA: 0x0000C0C9 File Offset: 0x0000A2C9
		public static TraceSwitch DGRowPaint
		{
			get
			{
				if (CompModSwitches.dgRowPaint == null)
				{
					CompModSwitches.dgRowPaint = new TraceSwitch("DGRowPaint", "DataGrid Simple Row painting stuff");
				}
				return CompModSwitches.dgRowPaint;
			}
		}

		// Token: 0x17000150 RID: 336
		// (get) Token: 0x060003D1 RID: 977 RVA: 0x0000C0EB File Offset: 0x0000A2EB
		public static TraceSwitch DragDrop
		{
			get
			{
				if (CompModSwitches.dragDrop == null)
				{
					CompModSwitches.dragDrop = new TraceSwitch("DragDrop", "Debug OLEDragDrop support in Controls");
				}
				return CompModSwitches.dragDrop;
			}
		}

		// Token: 0x17000151 RID: 337
		// (get) Token: 0x060003D2 RID: 978 RVA: 0x0000C10D File Offset: 0x0000A30D
		public static TraceSwitch FlowLayout
		{
			get
			{
				if (CompModSwitches.flowLayout == null)
				{
					CompModSwitches.flowLayout = new TraceSwitch("FlowLayout", "Debug flow layout");
				}
				return CompModSwitches.flowLayout;
			}
		}

		// Token: 0x17000152 RID: 338
		// (get) Token: 0x060003D3 RID: 979 RVA: 0x0000C12F File Offset: 0x0000A32F
		public static TraceSwitch ImeMode
		{
			get
			{
				if (CompModSwitches.imeMode == null)
				{
					CompModSwitches.imeMode = new TraceSwitch("ImeMode", "Debug IME Mode");
				}
				return CompModSwitches.imeMode;
			}
		}

		// Token: 0x17000153 RID: 339
		// (get) Token: 0x060003D4 RID: 980 RVA: 0x0000C151 File Offset: 0x0000A351
		public static TraceSwitch LayoutPerformance
		{
			get
			{
				if (CompModSwitches.layoutPerformance == null)
				{
					CompModSwitches.layoutPerformance = new TraceSwitch("LayoutPerformance", "Tracks layout events which impact performance.");
				}
				return CompModSwitches.layoutPerformance;
			}
		}

		// Token: 0x17000154 RID: 340
		// (get) Token: 0x060003D5 RID: 981 RVA: 0x0000C173 File Offset: 0x0000A373
		public static TraceSwitch LayoutSuspendResume
		{
			get
			{
				if (CompModSwitches.layoutSuspendResume == null)
				{
					CompModSwitches.layoutSuspendResume = new TraceSwitch("LayoutSuspendResume", "Tracks SuspendLayout/ResumeLayout.");
				}
				return CompModSwitches.layoutSuspendResume;
			}
		}

		// Token: 0x17000155 RID: 341
		// (get) Token: 0x060003D6 RID: 982 RVA: 0x0000C195 File Offset: 0x0000A395
		public static BooleanSwitch LifetimeTracing
		{
			get
			{
				if (CompModSwitches.lifetimeTracing == null)
				{
					CompModSwitches.lifetimeTracing = new BooleanSwitch("LifetimeTracing", "Track lifetime events. This will cause objects to track the stack at creation and dispose.");
				}
				return CompModSwitches.lifetimeTracing;
			}
		}

		// Token: 0x17000156 RID: 342
		// (get) Token: 0x060003D7 RID: 983 RVA: 0x0000C1B7 File Offset: 0x0000A3B7
		public static TraceSwitch MSAA
		{
			get
			{
				if (CompModSwitches.msaa == null)
				{
					CompModSwitches.msaa = new TraceSwitch("MSAA", "Debug Microsoft Active Accessibility");
				}
				return CompModSwitches.msaa;
			}
		}

		// Token: 0x17000157 RID: 343
		// (get) Token: 0x060003D8 RID: 984 RVA: 0x0000C1D9 File Offset: 0x0000A3D9
		public static TraceSwitch MSOComponentManager
		{
			get
			{
				if (CompModSwitches.msoComponentManager == null)
				{
					CompModSwitches.msoComponentManager = new TraceSwitch("MSOComponentManager", "Debug MSO Component Manager support");
				}
				return CompModSwitches.msoComponentManager;
			}
		}

		// Token: 0x17000158 RID: 344
		// (get) Token: 0x060003D9 RID: 985 RVA: 0x0000C1FB File Offset: 0x0000A3FB
		public static TraceSwitch RichLayout
		{
			get
			{
				if (CompModSwitches.richLayout == null)
				{
					CompModSwitches.richLayout = new TraceSwitch("RichLayout", "Debug layout in RichControls");
				}
				return CompModSwitches.richLayout;
			}
		}

		// Token: 0x17000159 RID: 345
		// (get) Token: 0x060003DA RID: 986 RVA: 0x0000C21D File Offset: 0x0000A41D
		public static TraceSwitch SetBounds
		{
			get
			{
				if (CompModSwitches.setBounds == null)
				{
					CompModSwitches.setBounds = new TraceSwitch("SetBounds", "Trace changes to control size/position.");
				}
				return CompModSwitches.setBounds;
			}
		}

		// Token: 0x1700015A RID: 346
		// (get) Token: 0x060003DB RID: 987 RVA: 0x0000C23F File Offset: 0x0000A43F
		public static TraceSwitch HandleLeak
		{
			get
			{
				if (CompModSwitches.handleLeak == null)
				{
					CompModSwitches.handleLeak = new TraceSwitch("HANDLELEAK", "HandleCollector: Track Win32 Handle Leaks");
				}
				return CompModSwitches.handleLeak;
			}
		}

		// Token: 0x1700015B RID: 347
		// (get) Token: 0x060003DC RID: 988 RVA: 0x0000C261 File Offset: 0x0000A461
		public static BooleanSwitch TraceCollect
		{
			get
			{
				if (CompModSwitches.traceCollect == null)
				{
					CompModSwitches.traceCollect = new BooleanSwitch("TRACECOLLECT", "HandleCollector: Trace HandleCollector operations");
				}
				return CompModSwitches.traceCollect;
			}
		}

		// Token: 0x04000403 RID: 1027
		private static TraceSwitch activeX;

		// Token: 0x04000404 RID: 1028
		private static TraceSwitch flowLayout;

		// Token: 0x04000405 RID: 1029
		private static TraceSwitch dataCursor;

		// Token: 0x04000406 RID: 1030
		private static TraceSwitch dataGridCursor;

		// Token: 0x04000407 RID: 1031
		private static TraceSwitch dataGridEditing;

		// Token: 0x04000408 RID: 1032
		private static TraceSwitch dataGridKeys;

		// Token: 0x04000409 RID: 1033
		private static TraceSwitch dataGridLayout;

		// Token: 0x0400040A RID: 1034
		private static TraceSwitch dataGridPainting;

		// Token: 0x0400040B RID: 1035
		private static TraceSwitch dataGridParents;

		// Token: 0x0400040C RID: 1036
		private static TraceSwitch dataGridScrolling;

		// Token: 0x0400040D RID: 1037
		private static TraceSwitch dataGridSelection;

		// Token: 0x0400040E RID: 1038
		private static TraceSwitch dataObject;

		// Token: 0x0400040F RID: 1039
		private static TraceSwitch dataView;

		// Token: 0x04000410 RID: 1040
		private static TraceSwitch debugGridView;

		// Token: 0x04000411 RID: 1041
		private static TraceSwitch dgCaptionPaint;

		// Token: 0x04000412 RID: 1042
		private static TraceSwitch dgEditColumnEditing;

		// Token: 0x04000413 RID: 1043
		private static TraceSwitch dgRelationShpRowLayout;

		// Token: 0x04000414 RID: 1044
		private static TraceSwitch dgRelationShpRowPaint;

		// Token: 0x04000415 RID: 1045
		private static TraceSwitch dgRowPaint;

		// Token: 0x04000416 RID: 1046
		private static TraceSwitch dragDrop;

		// Token: 0x04000417 RID: 1047
		private static TraceSwitch imeMode;

		// Token: 0x04000418 RID: 1048
		private static TraceSwitch msaa;

		// Token: 0x04000419 RID: 1049
		private static TraceSwitch msoComponentManager;

		// Token: 0x0400041A RID: 1050
		private static TraceSwitch layoutPerformance;

		// Token: 0x0400041B RID: 1051
		private static TraceSwitch layoutSuspendResume;

		// Token: 0x0400041C RID: 1052
		private static TraceSwitch richLayout;

		// Token: 0x0400041D RID: 1053
		private static TraceSwitch setBounds;

		// Token: 0x0400041E RID: 1054
		private static BooleanSwitch lifetimeTracing;

		// Token: 0x0400041F RID: 1055
		private static TraceSwitch handleLeak;

		// Token: 0x04000420 RID: 1056
		private static BooleanSwitch traceCollect;
	}
}
