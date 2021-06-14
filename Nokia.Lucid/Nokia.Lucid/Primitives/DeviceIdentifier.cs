using System;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace Nokia.Lucid.Primitives
{
	// Token: 0x02000035 RID: 53
	public sealed class DeviceIdentifier
	{
		// Token: 0x0600015F RID: 351 RVA: 0x0000B42A File Offset: 0x0000962A
		private DeviceIdentifier(string value, string vid, string pid, int? mi, Guid guid)
		{
			this.value = value;
			this.vid = vid;
			this.pid = pid;
			this.mi = mi;
			this.guid = guid;
		}

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x06000160 RID: 352 RVA: 0x0000B457 File Offset: 0x00009657
		public string Value
		{
			get
			{
				return this.value;
			}
		}

		// Token: 0x06000161 RID: 353 RVA: 0x0000B460 File Offset: 0x00009660
		public static DeviceIdentifier Parse(string value)
		{
			DeviceIdentifier result;
			if (!DeviceIdentifier.TryParse(value, out result))
			{
				throw new FormatException();
			}
			return result;
		}

		// Token: 0x06000162 RID: 354 RVA: 0x0000B480 File Offset: 0x00009680
		public static bool TryParse(string value, out DeviceIdentifier result)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			string text;
			string text2;
			int? num;
			Guid guid;
			if (!DeviceIdentifier.TryParse(value, out text, out text2, out num, out guid))
			{
				result = null;
				return false;
			}
			result = new DeviceIdentifier(value, text, text2, num, guid);
			return true;
		}

		// Token: 0x06000163 RID: 355 RVA: 0x0000B4BD File Offset: 0x000096BD
		public bool Vid(string value)
		{
			return string.Equals(this.vid, value, StringComparison.OrdinalIgnoreCase);
		}

		// Token: 0x06000164 RID: 356 RVA: 0x0000B4DB File Offset: 0x000096DB
		public bool Vid(params string[] values)
		{
			return values == null || values.Length == 0 || values.Any((string s) => string.Equals(this.vid, s, StringComparison.OrdinalIgnoreCase));
		}

		// Token: 0x06000165 RID: 357 RVA: 0x0000B4F9 File Offset: 0x000096F9
		public bool Pid(string value)
		{
			return string.Equals(this.pid, value, StringComparison.OrdinalIgnoreCase);
		}

		// Token: 0x06000166 RID: 358 RVA: 0x0000B517 File Offset: 0x00009717
		public bool Pid(params string[] values)
		{
			return values == null || values.Length == 0 || values.Any((string s) => string.Equals(this.pid, s, StringComparison.OrdinalIgnoreCase));
		}

		// Token: 0x06000167 RID: 359 RVA: 0x0000B538 File Offset: 0x00009738
		public bool MI(int value)
		{
			return this.mi == null || this.mi.Value == value;
		}

		// Token: 0x06000168 RID: 360 RVA: 0x0000B588 File Offset: 0x00009788
		public bool MI(params int[] values)
		{
			return this.mi == null || values == null || values.Length == 0 || values.Any((int n) => this.mi.Value == n);
		}

		// Token: 0x06000169 RID: 361 RVA: 0x0000B5C4 File Offset: 0x000097C4
		public bool Guid(Guid value)
		{
			return this.guid.Equals(value);
		}

		// Token: 0x0600016A RID: 362 RVA: 0x0000B5FC File Offset: 0x000097FC
		public bool Guid(params Guid[] values)
		{
			return values == null || values.Length == 0 || values.Any((Guid s) => this.guid.Equals(s));
		}

		// Token: 0x0600016B RID: 363 RVA: 0x0000B61A File Offset: 0x0000981A
		public bool Matches(string vid, string pid, Guid guid, int mi)
		{
			return this.Vid(vid) && this.Pid(pid) && this.Guid(guid) && this.MI(mi);
		}

		// Token: 0x0600016C RID: 364 RVA: 0x0000B641 File Offset: 0x00009841
		public bool Matches(string vid, string pid, Guid guid, params int[] mi)
		{
			return this.Vid(vid) && this.Pid(pid) && this.Guid(guid) && this.MI(mi);
		}

		// Token: 0x0600016D RID: 365 RVA: 0x0000B668 File Offset: 0x00009868
		public bool Matches(string identifier)
		{
			string text;
			string text2;
			int? num;
			Guid guid;
			return DeviceIdentifier.TryParse(identifier, out text, out text2, out num, out guid) && (this.Vid(text) && this.Pid(text2) && (num == null || this.MI(num.Value))) && this.Guid(guid);
		}

		// Token: 0x0600016E RID: 366 RVA: 0x0000B6BA File Offset: 0x000098BA
		public override string ToString()
		{
			return this.value;
		}

		// Token: 0x0600016F RID: 367 RVA: 0x0000B6C4 File Offset: 0x000098C4
		private static bool TryParse(string value, out string vid, out string pid, out int? mi, out Guid guid)
		{
			Match match = DeviceIdentifier.IdentifierPattern.Match(value);
			if (!match.Success)
			{
				vid = null;
				pid = null;
				mi = null;
				guid = System.Guid.Empty;
				return false;
			}
			vid = match.Groups["Vid"].Value;
			pid = match.Groups["Pid"].Value;
			guid = new Guid(match.Groups["Guid"].Value);
			Group group = match.Groups["Mi"];
			mi = (group.Success ? new int?(int.Parse(group.Value, NumberStyles.None, NumberFormatInfo.InvariantInfo)) : null);
			return true;
		}

		// Token: 0x040000C8 RID: 200
		private static readonly Regex IdentifierPattern = new Regex("\\bVID_(?<Vid>[0-9A-Z]{4})&PID_(?<Pid>[0-9A-Z]{4})(?:&MI_(?<Mi>\\d{2}))?.*(?<Guid>{[A-F0-9]{8}(?:-[A-F0-9]{4}){3}-[A-F0-9]{12}})", RegexOptions.IgnoreCase | RegexOptions.Compiled);

		// Token: 0x040000C9 RID: 201
		private readonly string value;

		// Token: 0x040000CA RID: 202
		private readonly string vid;

		// Token: 0x040000CB RID: 203
		private readonly string pid;

		// Token: 0x040000CC RID: 204
		private readonly int? mi;

		// Token: 0x040000CD RID: 205
		private readonly Guid guid;
	}
}
