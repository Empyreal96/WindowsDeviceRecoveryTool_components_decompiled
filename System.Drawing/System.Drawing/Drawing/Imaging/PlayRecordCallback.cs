using System;

namespace System.Drawing.Imaging
{
	/// <summary>This delegate is not used. For an example of enumerating the records of a metafile, see <see cref="M:System.Drawing.Graphics.EnumerateMetafile(System.Drawing.Imaging.Metafile,System.Drawing.Point,System.Drawing.Graphics.EnumerateMetafileProc)" />.</summary>
	/// <param name="recordType">Not used. </param>
	/// <param name="flags">Not used. </param>
	/// <param name="dataSize">Not used. </param>
	/// <param name="recordData">Not used. </param>
	// Token: 0x020000AE RID: 174
	// (Invoke) Token: 0x06000A08 RID: 2568
	public delegate void PlayRecordCallback(EmfPlusRecordType recordType, int flags, int dataSize, IntPtr recordData);
}
