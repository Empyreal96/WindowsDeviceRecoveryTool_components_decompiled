using System;

namespace System.Drawing.Drawing2D
{
	/// <summary>Encapsulates the data that makes up a <see cref="T:System.Drawing.Region" /> object. This class cannot be inherited.</summary>
	// Token: 0x020000D3 RID: 211
	public sealed class RegionData
	{
		// Token: 0x06000B67 RID: 2919 RVA: 0x00029D15 File Offset: 0x00027F15
		internal RegionData(byte[] data)
		{
			this.data = data;
		}

		/// <summary>Gets or sets an array of bytes that specify the <see cref="T:System.Drawing.Region" /> object.</summary>
		/// <returns>An array of bytes that specify the <see cref="T:System.Drawing.Region" /> object.</returns>
		// Token: 0x170003C8 RID: 968
		// (get) Token: 0x06000B68 RID: 2920 RVA: 0x00029D24 File Offset: 0x00027F24
		// (set) Token: 0x06000B69 RID: 2921 RVA: 0x00029D2C File Offset: 0x00027F2C
		public byte[] Data
		{
			get
			{
				return this.data;
			}
			set
			{
				this.data = value;
			}
		}

		// Token: 0x04000A13 RID: 2579
		private byte[] data;
	}
}
