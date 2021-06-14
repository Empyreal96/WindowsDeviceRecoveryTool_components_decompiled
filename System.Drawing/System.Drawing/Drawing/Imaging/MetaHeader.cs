using System;
using System.Runtime.InteropServices;

namespace System.Drawing.Imaging
{
	/// <summary>Contains information about a windows-format (WMF) metafile.</summary>
	// Token: 0x020000AB RID: 171
	[StructLayout(LayoutKind.Sequential, Pack = 2)]
	public sealed class MetaHeader
	{
		/// <summary>Gets or sets the type of the associated <see cref="T:System.Drawing.Imaging.Metafile" /> object.</summary>
		/// <returns>The type of the associated <see cref="T:System.Drawing.Imaging.Metafile" /> object.</returns>
		// Token: 0x17000387 RID: 903
		// (get) Token: 0x060009F8 RID: 2552 RVA: 0x00025694 File Offset: 0x00023894
		// (set) Token: 0x060009F9 RID: 2553 RVA: 0x0002569C File Offset: 0x0002389C
		public short Type
		{
			get
			{
				return this.type;
			}
			set
			{
				this.type = value;
			}
		}

		/// <summary>Gets or sets the size, in bytes, of the header file.</summary>
		/// <returns>The size, in bytes, of the header file.</returns>
		// Token: 0x17000388 RID: 904
		// (get) Token: 0x060009FA RID: 2554 RVA: 0x000256A5 File Offset: 0x000238A5
		// (set) Token: 0x060009FB RID: 2555 RVA: 0x000256AD File Offset: 0x000238AD
		public short HeaderSize
		{
			get
			{
				return this.headerSize;
			}
			set
			{
				this.headerSize = value;
			}
		}

		/// <summary>Gets or sets the version number of the header format.</summary>
		/// <returns>The version number of the header format.</returns>
		// Token: 0x17000389 RID: 905
		// (get) Token: 0x060009FC RID: 2556 RVA: 0x000256B6 File Offset: 0x000238B6
		// (set) Token: 0x060009FD RID: 2557 RVA: 0x000256BE File Offset: 0x000238BE
		public short Version
		{
			get
			{
				return this.version;
			}
			set
			{
				this.version = value;
			}
		}

		/// <summary>Gets or sets the size, in bytes, of the associated <see cref="T:System.Drawing.Imaging.Metafile" /> object.</summary>
		/// <returns>The size, in bytes, of the associated <see cref="T:System.Drawing.Imaging.Metafile" /> object.</returns>
		// Token: 0x1700038A RID: 906
		// (get) Token: 0x060009FE RID: 2558 RVA: 0x000256C7 File Offset: 0x000238C7
		// (set) Token: 0x060009FF RID: 2559 RVA: 0x000256CF File Offset: 0x000238CF
		public int Size
		{
			get
			{
				return this.size;
			}
			set
			{
				this.size = value;
			}
		}

		/// <summary>Gets or sets the maximum number of objects that exist in the <see cref="T:System.Drawing.Imaging.Metafile" /> object at the same time.</summary>
		/// <returns>The maximum number of objects that exist in the <see cref="T:System.Drawing.Imaging.Metafile" /> object at the same time.</returns>
		// Token: 0x1700038B RID: 907
		// (get) Token: 0x06000A00 RID: 2560 RVA: 0x000256D8 File Offset: 0x000238D8
		// (set) Token: 0x06000A01 RID: 2561 RVA: 0x000256E0 File Offset: 0x000238E0
		public short NoObjects
		{
			get
			{
				return this.noObjects;
			}
			set
			{
				this.noObjects = value;
			}
		}

		/// <summary>Gets or sets the size, in bytes, of the largest record in the associated <see cref="T:System.Drawing.Imaging.Metafile" /> object.</summary>
		/// <returns>The size, in bytes, of the largest record in the associated <see cref="T:System.Drawing.Imaging.Metafile" /> object.</returns>
		// Token: 0x1700038C RID: 908
		// (get) Token: 0x06000A02 RID: 2562 RVA: 0x000256E9 File Offset: 0x000238E9
		// (set) Token: 0x06000A03 RID: 2563 RVA: 0x000256F1 File Offset: 0x000238F1
		public int MaxRecord
		{
			get
			{
				return this.maxRecord;
			}
			set
			{
				this.maxRecord = value;
			}
		}

		/// <summary>Not used. Always returns 0.</summary>
		/// <returns>Always 0.</returns>
		// Token: 0x1700038D RID: 909
		// (get) Token: 0x06000A04 RID: 2564 RVA: 0x000256FA File Offset: 0x000238FA
		// (set) Token: 0x06000A05 RID: 2565 RVA: 0x00025702 File Offset: 0x00023902
		public short NoParameters
		{
			get
			{
				return this.noParameters;
			}
			set
			{
				this.noParameters = value;
			}
		}

		// Token: 0x04000929 RID: 2345
		private short type;

		// Token: 0x0400092A RID: 2346
		private short headerSize;

		// Token: 0x0400092B RID: 2347
		private short version;

		// Token: 0x0400092C RID: 2348
		private int size;

		// Token: 0x0400092D RID: 2349
		private short noObjects;

		// Token: 0x0400092E RID: 2350
		private int maxRecord;

		// Token: 0x0400092F RID: 2351
		private short noParameters;
	}
}
