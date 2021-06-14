using System;
using System.Collections;
using System.Globalization;

namespace MS.Internal.Utility
{
	// Token: 0x020007EF RID: 2031
	internal class TraceLog
	{
		// Token: 0x06007D40 RID: 32064 RVA: 0x00233357 File Offset: 0x00231557
		internal TraceLog() : this(int.MaxValue)
		{
		}

		// Token: 0x06007D41 RID: 32065 RVA: 0x00233364 File Offset: 0x00231564
		internal TraceLog(int size)
		{
			this._size = size;
			this._log = new ArrayList();
		}

		// Token: 0x06007D42 RID: 32066 RVA: 0x00233380 File Offset: 0x00231580
		internal void Add(string message, params object[] args)
		{
			string value = DateTime.Now.Ticks.ToString(CultureInfo.InvariantCulture) + " " + string.Format(CultureInfo.InvariantCulture, message, args);
			if (this._log.Count == this._size)
			{
				this._log.RemoveAt(0);
			}
			this._log.Add(value);
		}

		// Token: 0x06007D43 RID: 32067 RVA: 0x002333EC File Offset: 0x002315EC
		internal void WriteLog()
		{
			for (int i = 0; i < this._log.Count; i++)
			{
				Console.WriteLine(this._log[i]);
			}
		}

		// Token: 0x06007D44 RID: 32068 RVA: 0x00233420 File Offset: 0x00231620
		internal static string IdFor(object o)
		{
			if (o == null)
			{
				return "NULL";
			}
			return string.Format(CultureInfo.InvariantCulture, "{0}.{1}", new object[]
			{
				o.GetType().Name,
				o.GetHashCode()
			});
		}

		// Token: 0x04003AEE RID: 15086
		private ArrayList _log;

		// Token: 0x04003AEF RID: 15087
		private int _size;
	}
}
