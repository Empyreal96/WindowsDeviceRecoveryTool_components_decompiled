using System;
using System.Text.RegularExpressions;

namespace Microsoft.WindowsDeviceRecoveryTool.Core
{
	// Token: 0x0200000E RID: 14
	public sealed class VidPidPair : IEquatable<VidPidPair>
	{
		// Token: 0x0600002C RID: 44 RVA: 0x00002512 File Offset: 0x00000712
		public VidPidPair(string vid, string pid)
		{
			this.Vid = vid;
			this.Pid = pid;
		}

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x0600002D RID: 45 RVA: 0x00002528 File Offset: 0x00000728
		// (set) Token: 0x0600002E RID: 46 RVA: 0x00002530 File Offset: 0x00000730
		public string Vid { get; private set; }

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x0600002F RID: 47 RVA: 0x00002539 File Offset: 0x00000739
		// (set) Token: 0x06000030 RID: 48 RVA: 0x00002541 File Offset: 0x00000741
		public string Pid { get; private set; }

		// Token: 0x06000031 RID: 49 RVA: 0x0000254C File Offset: 0x0000074C
		public static VidPidPair Parse(string devicePath)
		{
			VidPidPair result;
			if (!VidPidPair.TryParse(devicePath, out result))
			{
				throw new FormatException();
			}
			return result;
		}

		// Token: 0x06000032 RID: 50 RVA: 0x0000256C File Offset: 0x0000076C
		public bool Equals(VidPidPair other)
		{
			if (other == null)
			{
				return false;
			}
			VidPidPairEqualityComparer vidPidPairEqualityComparer = new VidPidPairEqualityComparer();
			return vidPidPairEqualityComparer.Equals(this, other);
		}

		// Token: 0x06000033 RID: 51 RVA: 0x00002594 File Offset: 0x00000794
		public override bool Equals(object obj)
		{
			VidPidPair vidPidPair = obj as VidPidPair;
			return !(vidPidPair == null) && this.Equals(vidPidPair);
		}

		// Token: 0x06000034 RID: 52 RVA: 0x000025BC File Offset: 0x000007BC
		public override int GetHashCode()
		{
			VidPidPairEqualityComparer vidPidPairEqualityComparer = new VidPidPairEqualityComparer();
			return vidPidPairEqualityComparer.GetHashCode(this);
		}

		// Token: 0x06000035 RID: 53 RVA: 0x000025D6 File Offset: 0x000007D6
		public static bool operator ==(VidPidPair a, VidPidPair b)
		{
			return object.ReferenceEquals(a, b) || (a != null && b != null && a.Equals(b));
		}

		// Token: 0x06000036 RID: 54 RVA: 0x000025F2 File Offset: 0x000007F2
		public static bool operator !=(VidPidPair a, VidPidPair b)
		{
			return !(a == b);
		}

		// Token: 0x06000037 RID: 55 RVA: 0x00002600 File Offset: 0x00000800
		public static bool TryParse(string value, out VidPidPair result)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			string vid;
			string pid;
			if (!VidPidPair.TryParse(value, out vid, out pid))
			{
				result = null;
				return false;
			}
			result = new VidPidPair(vid, pid);
			return true;
		}

		// Token: 0x06000038 RID: 56 RVA: 0x00002638 File Offset: 0x00000838
		private static bool TryParse(string value, out string vid, out string pid)
		{
			Match match = VidPidPair.Pattern.Match(value);
			if (!match.Success)
			{
				vid = null;
				pid = null;
				return false;
			}
			vid = match.Groups["Vid"].Value;
			pid = match.Groups["Pid"].Value;
			return true;
		}

		// Token: 0x04000010 RID: 16
		private static readonly Regex Pattern = new Regex("\\bVID_(?<Vid>[0-9A-Z]{4})&PID_(?<Pid>[0-9A-Z]{4})(?:&MI_(?<Mi>\\d{2}))?.*(?<Guid>{[A-F0-9]{8}(?:-[A-F0-9]{4}){3}-[A-F0-9]{12}})", RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.CultureInvariant);
	}
}
