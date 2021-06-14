using System;
using System.Globalization;

namespace System.Spatial
{
	// Token: 0x0200003D RID: 61
	public class GeometryPosition : IEquatable<GeometryPosition>
	{
		// Token: 0x06000190 RID: 400 RVA: 0x00004AA1 File Offset: 0x00002CA1
		public GeometryPosition(double x, double y, double? z, double? m)
		{
			this.x = x;
			this.y = y;
			this.z = z;
			this.m = m;
		}

		// Token: 0x06000191 RID: 401 RVA: 0x00004AC8 File Offset: 0x00002CC8
		public GeometryPosition(double x, double y) : this(x, y, null, null)
		{
		}

		// Token: 0x17000036 RID: 54
		// (get) Token: 0x06000192 RID: 402 RVA: 0x00004AEF File Offset: 0x00002CEF
		public double? M
		{
			get
			{
				return this.m;
			}
		}

		// Token: 0x17000037 RID: 55
		// (get) Token: 0x06000193 RID: 403 RVA: 0x00004AF7 File Offset: 0x00002CF7
		public double X
		{
			get
			{
				return this.x;
			}
		}

		// Token: 0x17000038 RID: 56
		// (get) Token: 0x06000194 RID: 404 RVA: 0x00004AFF File Offset: 0x00002CFF
		public double Y
		{
			get
			{
				return this.y;
			}
		}

		// Token: 0x17000039 RID: 57
		// (get) Token: 0x06000195 RID: 405 RVA: 0x00004B07 File Offset: 0x00002D07
		public double? Z
		{
			get
			{
				return this.z;
			}
		}

		// Token: 0x06000196 RID: 406 RVA: 0x00004B0F File Offset: 0x00002D0F
		public static bool operator ==(GeometryPosition left, GeometryPosition right)
		{
			if (object.ReferenceEquals(left, null))
			{
				return object.ReferenceEquals(right, null);
			}
			return !object.ReferenceEquals(right, null) && left.Equals(right);
		}

		// Token: 0x06000197 RID: 407 RVA: 0x00004B34 File Offset: 0x00002D34
		public static bool operator !=(GeometryPosition left, GeometryPosition right)
		{
			return !(left == right);
		}

		// Token: 0x06000198 RID: 408 RVA: 0x00004B40 File Offset: 0x00002D40
		public override bool Equals(object obj)
		{
			return !object.ReferenceEquals(null, obj) && !(obj.GetType() != typeof(GeometryPosition)) && this.Equals((GeometryPosition)obj);
		}

		// Token: 0x06000199 RID: 409 RVA: 0x00004B74 File Offset: 0x00002D74
		public bool Equals(GeometryPosition other)
		{
			return other != null && other.x.Equals(this.x) && other.y.Equals(this.y) && other.z.Equals(this.z) && other.m.Equals(this.m);
		}

		// Token: 0x0600019A RID: 410 RVA: 0x00004BF8 File Offset: 0x00002DF8
		public override int GetHashCode()
		{
			int num = this.x.GetHashCode();
			num = (num * 397 ^ this.y.GetHashCode());
			num = (num * 397 ^ ((this.z != null) ? this.z.Value.GetHashCode() : 0));
			return num * 397 ^ ((this.m != null) ? this.m.Value.GetHashCode() : 0);
		}

		// Token: 0x0600019B RID: 411 RVA: 0x00004C98 File Offset: 0x00002E98
		public override string ToString()
		{
			return string.Format(CultureInfo.InvariantCulture, "GeometryPosition({0}, {1}, {2}, {3})", new object[]
			{
				this.x,
				this.y,
				(this.z != null) ? this.z.ToString() : "null",
				(this.m != null) ? this.m.ToString() : "null"
			});
		}

		// Token: 0x04000038 RID: 56
		private readonly double? m;

		// Token: 0x04000039 RID: 57
		private readonly double x;

		// Token: 0x0400003A RID: 58
		private readonly double y;

		// Token: 0x0400003B RID: 59
		private readonly double? z;
	}
}
