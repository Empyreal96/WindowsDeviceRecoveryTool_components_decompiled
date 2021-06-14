using System;

namespace System.Drawing.Imaging
{
	/// <summary>Provides properties that get the frame dimensions of an image. Not inheritable.</summary>
	// Token: 0x0200009D RID: 157
	public sealed class FrameDimension
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Drawing.Imaging.FrameDimension" /> class using the specified <see langword="Guid" /> structure.</summary>
		/// <param name="guid">A <see langword="Guid" /> structure that contains a GUID for this <see cref="T:System.Drawing.Imaging.FrameDimension" /> object. </param>
		// Token: 0x0600094C RID: 2380 RVA: 0x00023BA6 File Offset: 0x00021DA6
		public FrameDimension(Guid guid)
		{
			this.guid = guid;
		}

		/// <summary>Gets a globally unique identifier (GUID) that represents this <see cref="T:System.Drawing.Imaging.FrameDimension" /> object.</summary>
		/// <returns>A <see langword="Guid" /> structure that contains a GUID that represents this <see cref="T:System.Drawing.Imaging.FrameDimension" /> object.</returns>
		// Token: 0x17000363 RID: 867
		// (get) Token: 0x0600094D RID: 2381 RVA: 0x00023BB5 File Offset: 0x00021DB5
		public Guid Guid
		{
			get
			{
				return this.guid;
			}
		}

		/// <summary>Gets the time dimension.</summary>
		/// <returns>The time dimension.</returns>
		// Token: 0x17000364 RID: 868
		// (get) Token: 0x0600094E RID: 2382 RVA: 0x00023BBD File Offset: 0x00021DBD
		public static FrameDimension Time
		{
			get
			{
				return FrameDimension.time;
			}
		}

		/// <summary>Gets the resolution dimension.</summary>
		/// <returns>The resolution dimension.</returns>
		// Token: 0x17000365 RID: 869
		// (get) Token: 0x0600094F RID: 2383 RVA: 0x00023BC4 File Offset: 0x00021DC4
		public static FrameDimension Resolution
		{
			get
			{
				return FrameDimension.resolution;
			}
		}

		/// <summary>Gets the page dimension.</summary>
		/// <returns>The page dimension.</returns>
		// Token: 0x17000366 RID: 870
		// (get) Token: 0x06000950 RID: 2384 RVA: 0x00023BCB File Offset: 0x00021DCB
		public static FrameDimension Page
		{
			get
			{
				return FrameDimension.page;
			}
		}

		/// <summary>Returns a value that indicates whether the specified object is a <see cref="T:System.Drawing.Imaging.FrameDimension" /> equivalent to this <see cref="T:System.Drawing.Imaging.FrameDimension" /> object.</summary>
		/// <param name="o">The object to test. </param>
		/// <returns>Returns <see langword="true" /> if <paramref name="o" /> is a <see cref="T:System.Drawing.Imaging.FrameDimension" /> equivalent to this <see cref="T:System.Drawing.Imaging.FrameDimension" /> object; otherwise, <see langword="false" />.</returns>
		// Token: 0x06000951 RID: 2385 RVA: 0x00023BD4 File Offset: 0x00021DD4
		public override bool Equals(object o)
		{
			FrameDimension frameDimension = o as FrameDimension;
			return frameDimension != null && this.guid == frameDimension.guid;
		}

		/// <summary>Returns a hash code for this <see cref="T:System.Drawing.Imaging.FrameDimension" /> object.</summary>
		/// <returns>Returns an <see langword="int" /> value that is the hash code of this <see cref="T:System.Drawing.Imaging.FrameDimension" /> object.</returns>
		// Token: 0x06000952 RID: 2386 RVA: 0x00023BFE File Offset: 0x00021DFE
		public override int GetHashCode()
		{
			return this.guid.GetHashCode();
		}

		/// <summary>Converts this <see cref="T:System.Drawing.Imaging.FrameDimension" /> object to a human-readable string.</summary>
		/// <returns>A string that represents this <see cref="T:System.Drawing.Imaging.FrameDimension" /> object.</returns>
		// Token: 0x06000953 RID: 2387 RVA: 0x00023C14 File Offset: 0x00021E14
		public override string ToString()
		{
			if (this == FrameDimension.time)
			{
				return "Time";
			}
			if (this == FrameDimension.resolution)
			{
				return "Resolution";
			}
			if (this == FrameDimension.page)
			{
				return "Page";
			}
			return "[FrameDimension: " + this.guid + "]";
		}

		// Token: 0x040008A5 RID: 2213
		private static FrameDimension time = new FrameDimension(new Guid("{6aedbd6d-3fb5-418a-83a6-7f45229dc872}"));

		// Token: 0x040008A6 RID: 2214
		private static FrameDimension resolution = new FrameDimension(new Guid("{84236f7b-3bd3-428f-8dab-4ea1439ca315}"));

		// Token: 0x040008A7 RID: 2215
		private static FrameDimension page = new FrameDimension(new Guid("{7462dc86-6180-4c7e-8e3f-ee7333a7a483}"));

		// Token: 0x040008A8 RID: 2216
		private Guid guid;
	}
}
