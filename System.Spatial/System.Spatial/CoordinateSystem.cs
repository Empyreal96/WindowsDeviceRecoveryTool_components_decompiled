using System;
using System.Collections.Generic;
using System.Globalization;

namespace System.Spatial
{
	// Token: 0x02000037 RID: 55
	public class CoordinateSystem
	{
		// Token: 0x0600016B RID: 363 RVA: 0x000044C8 File Offset: 0x000026C8
		static CoordinateSystem()
		{
			CoordinateSystem.References = new Dictionary<CompositeKey<int, CoordinateSystem.Topology>, CoordinateSystem>(EqualityComparer<CompositeKey<int, CoordinateSystem.Topology>>.Default);
			CoordinateSystem.AddRef(CoordinateSystem.DefaultGeometry);
			CoordinateSystem.AddRef(CoordinateSystem.DefaultGeography);
		}

		// Token: 0x0600016C RID: 364 RVA: 0x00004528 File Offset: 0x00002728
		internal CoordinateSystem(int epsgId, string name, CoordinateSystem.Topology topology)
		{
			this.topology = topology;
			this.EpsgId = new int?(epsgId);
			this.Name = name;
		}

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x0600016D RID: 365 RVA: 0x0000454A File Offset: 0x0000274A
		// (set) Token: 0x0600016E RID: 366 RVA: 0x00004552 File Offset: 0x00002752
		public int? EpsgId { get; internal set; }

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x0600016F RID: 367 RVA: 0x0000455C File Offset: 0x0000275C
		public string Id
		{
			get
			{
				return this.EpsgId.Value.ToString(CultureInfo.InvariantCulture);
			}
		}

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x06000170 RID: 368 RVA: 0x00004584 File Offset: 0x00002784
		// (set) Token: 0x06000171 RID: 369 RVA: 0x0000458C File Offset: 0x0000278C
		public string Name { get; internal set; }

		// Token: 0x06000172 RID: 370 RVA: 0x00004595 File Offset: 0x00002795
		public static CoordinateSystem Geography(int? epsgId)
		{
			if (epsgId == null)
			{
				return CoordinateSystem.DefaultGeography;
			}
			return CoordinateSystem.GetOrCreate(epsgId.Value, CoordinateSystem.Topology.Geography);
		}

		// Token: 0x06000173 RID: 371 RVA: 0x000045B3 File Offset: 0x000027B3
		public static CoordinateSystem Geometry(int? epsgId)
		{
			if (epsgId == null)
			{
				return CoordinateSystem.DefaultGeometry;
			}
			return CoordinateSystem.GetOrCreate(epsgId.Value, CoordinateSystem.Topology.Geometry);
		}

		// Token: 0x06000174 RID: 372 RVA: 0x000045D4 File Offset: 0x000027D4
		public override string ToString()
		{
			return string.Format(CultureInfo.InvariantCulture, "{0}CoordinateSystem(EpsgId={1})", new object[]
			{
				this.topology,
				this.EpsgId
			});
		}

		// Token: 0x06000175 RID: 373 RVA: 0x00004614 File Offset: 0x00002814
		public string ToWktId()
		{
			return "SRID=" + this.EpsgId + ";";
		}

		// Token: 0x06000176 RID: 374 RVA: 0x00004630 File Offset: 0x00002830
		public override bool Equals(object obj)
		{
			return this.Equals(obj as CoordinateSystem);
		}

		// Token: 0x06000177 RID: 375 RVA: 0x00004640 File Offset: 0x00002840
		public bool Equals(CoordinateSystem other)
		{
			return !object.ReferenceEquals(null, other) && (object.ReferenceEquals(this, other) || (object.Equals(other.topology, this.topology) && other.EpsgId.Equals(this.EpsgId)));
		}

		// Token: 0x06000178 RID: 376 RVA: 0x000046A4 File Offset: 0x000028A4
		public override int GetHashCode()
		{
			return this.topology.GetHashCode() * 397 ^ ((this.EpsgId != null) ? this.EpsgId.Value : 0);
		}

		// Token: 0x06000179 RID: 377 RVA: 0x000046E9 File Offset: 0x000028E9
		internal bool TopologyIs(CoordinateSystem.Topology expected)
		{
			return this.topology == expected;
		}

		// Token: 0x0600017A RID: 378 RVA: 0x000046F4 File Offset: 0x000028F4
		private static CoordinateSystem GetOrCreate(int epsgId, CoordinateSystem.Topology topology)
		{
			CoordinateSystem coordinateSystem;
			lock (CoordinateSystem.referencesLock)
			{
				if (CoordinateSystem.References.TryGetValue(CoordinateSystem.KeyFor(epsgId, topology), out coordinateSystem))
				{
					return coordinateSystem;
				}
				coordinateSystem = new CoordinateSystem(epsgId, "ID " + epsgId, topology);
				CoordinateSystem.AddRef(coordinateSystem);
			}
			return coordinateSystem;
		}

		// Token: 0x0600017B RID: 379 RVA: 0x00004768 File Offset: 0x00002968
		private static void AddRef(CoordinateSystem coords)
		{
			CoordinateSystem.References.Add(CoordinateSystem.KeyFor(coords.EpsgId.Value, coords.topology), coords);
		}

		// Token: 0x0600017C RID: 380 RVA: 0x00004799 File Offset: 0x00002999
		private static CompositeKey<int, CoordinateSystem.Topology> KeyFor(int epsgId, CoordinateSystem.Topology topology)
		{
			return new CompositeKey<int, CoordinateSystem.Topology>(epsgId, topology);
		}

		// Token: 0x04000029 RID: 41
		public static readonly CoordinateSystem DefaultGeometry = new CoordinateSystem(0, "Unitless Plane", CoordinateSystem.Topology.Geometry);

		// Token: 0x0400002A RID: 42
		public static readonly CoordinateSystem DefaultGeography = new CoordinateSystem(4326, "WGS84", CoordinateSystem.Topology.Geography);

		// Token: 0x0400002B RID: 43
		private static readonly Dictionary<CompositeKey<int, CoordinateSystem.Topology>, CoordinateSystem> References;

		// Token: 0x0400002C RID: 44
		private static readonly object referencesLock = new object();

		// Token: 0x0400002D RID: 45
		private CoordinateSystem.Topology topology;

		// Token: 0x02000038 RID: 56
		internal enum Topology
		{
			// Token: 0x04000031 RID: 49
			Geography,
			// Token: 0x04000032 RID: 50
			Geometry
		}
	}
}
