using System;
using System.Runtime.InteropServices;

namespace Interop.SirepClient
{
	// Token: 0x0200000D RID: 13
	public class SirepDiscovery : IDisposable
	{
		// Token: 0x06000089 RID: 137 RVA: 0x00002918 File Offset: 0x00000B18
		~SirepDiscovery()
		{
			this.Dispose(false);
		}

		// Token: 0x0600008A RID: 138 RVA: 0x00002948 File Offset: 0x00000B48
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x0600008B RID: 139 RVA: 0x00002958 File Offset: 0x00000B58
		protected virtual void Dispose(bool disposing)
		{
			if (this.sirepEnum != IntPtr.Zero)
			{
				NativeMethods.EndSirepEnum(this.sirepEnum);
				this.sirepEnum = IntPtr.Zero;
			}
			if (this.gch.IsAllocated)
			{
				this.gch.Free();
			}
		}

		// Token: 0x14000002 RID: 2
		// (add) Token: 0x0600008C RID: 140 RVA: 0x000029A8 File Offset: 0x00000BA8
		// (remove) Token: 0x0600008D RID: 141 RVA: 0x000029E0 File Offset: 0x00000BE0
		public event EventHandler<SirepDiscovery.DiscoveredEventArgs> Discovered;

		// Token: 0x0600008E RID: 142 RVA: 0x00002A18 File Offset: 0x00000C18
		public void Start(Guid soughtGuid = default(Guid))
		{
			if (this.sirepEnum == IntPtr.Zero)
			{
				this.gch = GCHandle.Alloc(this);
				this.enumCallback = new NativeMethods.SirepEnumCallback(SirepDiscovery.EnumCallback);
				this.sirepEnum = NativeMethods.BeginSirepEnum(0U, GCHandle.ToIntPtr(this.gch), soughtGuid.Equals(Guid.Empty) ? "" : soughtGuid.ToString("B"), this.enumCallback);
			}
		}

		// Token: 0x0600008F RID: 143 RVA: 0x00002A93 File Offset: 0x00000C93
		public void Stop()
		{
			this.Dispose();
		}

		// Token: 0x06000090 RID: 144 RVA: 0x00002A9C File Offset: 0x00000C9C
		private static void EnumCallback(IntPtr context, uint source, ref Guid guid, string name, string location, string address, string architecture, string osversion, ushort sirepPort, ushort sshPort)
		{
			SirepDiscovery sirepDiscovery = (SirepDiscovery)GCHandle.FromIntPtr(context).Target;
			EventHandler<SirepDiscovery.DiscoveredEventArgs> discovered = sirepDiscovery.Discovered;
			if (discovered != null)
			{
				sirepDiscovery.Discovered(sirepDiscovery, new SirepDiscovery.DiscoveredEventArgs(source, guid, name, location, address, architecture, osversion, sirepPort, sshPort));
			}
		}

		// Token: 0x0400002A RID: 42
		private IntPtr sirepEnum;

		// Token: 0x0400002B RID: 43
		private NativeMethods.SirepEnumCallback enumCallback;

		// Token: 0x0400002C RID: 44
		private GCHandle gch;

		// Token: 0x0200000E RID: 14
		public class DiscoveredEventArgs : EventArgs
		{
			// Token: 0x17000006 RID: 6
			// (get) Token: 0x06000091 RID: 145 RVA: 0x00002AEC File Offset: 0x00000CEC
			// (set) Token: 0x06000092 RID: 146 RVA: 0x00002AF4 File Offset: 0x00000CF4
			public uint ConnectionType { get; private set; }

			// Token: 0x17000007 RID: 7
			// (get) Token: 0x06000093 RID: 147 RVA: 0x00002AFD File Offset: 0x00000CFD
			// (set) Token: 0x06000094 RID: 148 RVA: 0x00002B05 File Offset: 0x00000D05
			public Guid Guid { get; private set; }

			// Token: 0x17000008 RID: 8
			// (get) Token: 0x06000095 RID: 149 RVA: 0x00002B0E File Offset: 0x00000D0E
			// (set) Token: 0x06000096 RID: 150 RVA: 0x00002B16 File Offset: 0x00000D16
			public string Name { get; private set; }

			// Token: 0x17000009 RID: 9
			// (get) Token: 0x06000097 RID: 151 RVA: 0x00002B1F File Offset: 0x00000D1F
			// (set) Token: 0x06000098 RID: 152 RVA: 0x00002B27 File Offset: 0x00000D27
			public string Location { get; private set; }

			// Token: 0x1700000A RID: 10
			// (get) Token: 0x06000099 RID: 153 RVA: 0x00002B30 File Offset: 0x00000D30
			// (set) Token: 0x0600009A RID: 154 RVA: 0x00002B38 File Offset: 0x00000D38
			public string Address { get; private set; }

			// Token: 0x1700000B RID: 11
			// (get) Token: 0x0600009B RID: 155 RVA: 0x00002B41 File Offset: 0x00000D41
			// (set) Token: 0x0600009C RID: 156 RVA: 0x00002B49 File Offset: 0x00000D49
			public string Architecture { get; private set; }

			// Token: 0x1700000C RID: 12
			// (get) Token: 0x0600009D RID: 157 RVA: 0x00002B52 File Offset: 0x00000D52
			// (set) Token: 0x0600009E RID: 158 RVA: 0x00002B5A File Offset: 0x00000D5A
			public string OSVersion { get; private set; }

			// Token: 0x1700000D RID: 13
			// (get) Token: 0x0600009F RID: 159 RVA: 0x00002B63 File Offset: 0x00000D63
			// (set) Token: 0x060000A0 RID: 160 RVA: 0x00002B6B File Offset: 0x00000D6B
			public ushort SirepPort { get; private set; }

			// Token: 0x1700000E RID: 14
			// (get) Token: 0x060000A1 RID: 161 RVA: 0x00002B74 File Offset: 0x00000D74
			// (set) Token: 0x060000A2 RID: 162 RVA: 0x00002B7C File Offset: 0x00000D7C
			public ushort SshPort { get; private set; }

			// Token: 0x060000A3 RID: 163 RVA: 0x00002B88 File Offset: 0x00000D88
			internal DiscoveredEventArgs(uint source, Guid guid, string name, string location, string address, string architecture, string osversion, ushort sirepPort, ushort sshPort)
			{
				this.ConnectionType = source;
				this.Guid = guid;
				this.Name = name;
				this.Location = location;
				this.Address = address;
				this.Architecture = architecture;
				this.OSVersion = osversion;
				this.SirepPort = sirepPort;
				this.SshPort = sshPort;
			}
		}
	}
}
