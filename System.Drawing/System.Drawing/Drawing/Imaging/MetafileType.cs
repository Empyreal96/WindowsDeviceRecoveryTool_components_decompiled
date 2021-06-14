using System;

namespace System.Drawing.Imaging
{
	/// <summary>Specifies types of metafiles. The <see cref="P:System.Drawing.Imaging.MetafileHeader.Type" /> property returns a member of this enumeration.</summary>
	// Token: 0x020000AA RID: 170
	public enum MetafileType
	{
		/// <summary>Specifies a metafile format that is not recognized in GDI+.</summary>
		// Token: 0x04000923 RID: 2339
		Invalid,
		/// <summary>Specifies a WMF (Windows Metafile) file. Such a file contains only GDI records.</summary>
		// Token: 0x04000924 RID: 2340
		Wmf,
		/// <summary>Specifies a WMF (Windows Metafile) file that has a placeable metafile header in front of it.</summary>
		// Token: 0x04000925 RID: 2341
		WmfPlaceable,
		/// <summary>Specifies an Enhanced Metafile (EMF) file. Such a file contains only GDI records.</summary>
		// Token: 0x04000926 RID: 2342
		Emf,
		/// <summary>Specifies an EMF+ file. Such a file contains only GDI+ records and must be displayed by using GDI+. Displaying the records using GDI may cause unpredictable results.</summary>
		// Token: 0x04000927 RID: 2343
		EmfPlusOnly,
		/// <summary>Specifies an EMF+ Dual file. Such a file contains GDI+ records along with alternative GDI records and can be displayed by using either GDI or GDI+. Displaying the records using GDI may cause some quality degradation.</summary>
		// Token: 0x04000928 RID: 2344
		EmfPlusDual
	}
}
