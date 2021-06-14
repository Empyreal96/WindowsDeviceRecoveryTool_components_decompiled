using System;

namespace Nokia.Lucid.DeviceInformation
{
	// Token: 0x02000017 RID: 23
	public struct PropertyKey : IEquatable<PropertyKey>
	{
		// Token: 0x0600009C RID: 156 RVA: 0x000055C0 File Offset: 0x000037C0
		public PropertyKey(int a, short b, short c, byte d, byte e, byte f, byte g, byte h, byte i, byte j, byte k, int propertyId)
		{
			this.category = new Guid(a, b, c, d, e, f, g, h, i, j, k);
			this.propertyId = propertyId;
		}

		// Token: 0x0600009D RID: 157 RVA: 0x000055F4 File Offset: 0x000037F4
		[CLSCompliant(false)]
		public PropertyKey(uint a, ushort b, ushort c, byte d, byte e, byte f, byte g, byte h, byte i, byte j, byte k, int propertyId)
		{
			this.category = new Guid(a, b, c, d, e, f, g, h, i, j, k);
			this.propertyId = propertyId;
		}

		// Token: 0x0600009E RID: 158 RVA: 0x00005627 File Offset: 0x00003827
		public PropertyKey(Guid category, int propertyId)
		{
			this.category = category;
			this.propertyId = propertyId;
		}

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x0600009F RID: 159 RVA: 0x00005637 File Offset: 0x00003837
		public Guid Category
		{
			get
			{
				return this.category;
			}
		}

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x060000A0 RID: 160 RVA: 0x0000563F File Offset: 0x0000383F
		public int PropertyId
		{
			get
			{
				return this.propertyId;
			}
		}

		// Token: 0x060000A1 RID: 161 RVA: 0x00005647 File Offset: 0x00003847
		public static bool operator ==(PropertyKey left, PropertyKey right)
		{
			return object.Equals(left, right);
		}

		// Token: 0x060000A2 RID: 162 RVA: 0x0000565A File Offset: 0x0000385A
		public static bool operator !=(PropertyKey left, PropertyKey right)
		{
			return !object.Equals(left, right);
		}

		// Token: 0x060000A3 RID: 163 RVA: 0x00005670 File Offset: 0x00003870
		public override int GetHashCode()
		{
			return this.category.GetHashCode() ^ this.propertyId.GetHashCode();
		}

		// Token: 0x060000A4 RID: 164 RVA: 0x000056A0 File Offset: 0x000038A0
		public override bool Equals(object obj)
		{
			return obj is PropertyKey && this.Equals((PropertyKey)obj);
		}

		// Token: 0x060000A5 RID: 165 RVA: 0x000056B8 File Offset: 0x000038B8
		public bool Equals(PropertyKey other)
		{
			return this.category == other.category && this.propertyId == other.propertyId;
		}

		// Token: 0x060000A6 RID: 166 RVA: 0x000056DF File Offset: 0x000038DF
		public override string ToString()
		{
			return string.Format("{{{0}}}[{1}]", this.category, this.propertyId);
		}

		// Token: 0x0400005F RID: 95
		private readonly Guid category;

		// Token: 0x04000060 RID: 96
		private readonly int propertyId;
	}
}
