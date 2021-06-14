using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.WindowsPhone.ImageUpdate.Tools.Common
{
	// Token: 0x0200005F RID: 95
	public class RgaBuilder
	{
		// Token: 0x17000045 RID: 69
		// (get) Token: 0x06000257 RID: 599 RVA: 0x0000B5C4 File Offset: 0x000097C4
		public bool HasContent
		{
			get
			{
				return this._rgaValues.Count > 0;
			}
		}

		// Token: 0x06000258 RID: 600 RVA: 0x0000B5D4 File Offset: 0x000097D4
		public void AddRgaValue(string keyName, string valueName, params string[] values)
		{
			KeyValuePair<string, string> key = new KeyValuePair<string, string>(keyName, valueName);
			List<string> list = null;
			if (!this._rgaValues.TryGetValue(key, out list))
			{
				list = new List<string>();
				this._rgaValues.Add(key, list);
			}
			list.AddRange(values);
		}

		// Token: 0x06000259 RID: 601 RVA: 0x0000B634 File Offset: 0x00009834
		public void Save(string outputFile)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("Windows Registry Editor Version 5.00");
			foreach (IGrouping<string, KeyValuePair<KeyValuePair<string, string>, List<string>>> grouping in from x in this._rgaValues
			group x by x.Key.Key)
			{
				stringBuilder.AppendFormat("[{0}]", grouping.Key);
				stringBuilder.AppendLine();
				foreach (KeyValuePair<KeyValuePair<string, string>, List<string>> keyValuePair in grouping)
				{
					RegUtil.RegOutput(stringBuilder, keyValuePair.Key.Value, keyValuePair.Value);
				}
				stringBuilder.AppendLine();
			}
			LongPathFile.WriteAllText(outputFile, stringBuilder.ToString(), Encoding.Unicode);
		}

		// Token: 0x04000172 RID: 370
		private Dictionary<KeyValuePair<string, string>, List<string>> _rgaValues = new Dictionary<KeyValuePair<string, string>, List<string>>(new RgaBuilder.KeyValuePairComparer());

		// Token: 0x02000060 RID: 96
		private class KeyValuePairComparer : IEqualityComparer<KeyValuePair<string, string>>
		{
			// Token: 0x0600025C RID: 604 RVA: 0x0000B74C File Offset: 0x0000994C
			public bool Equals(KeyValuePair<string, string> x, KeyValuePair<string, string> y)
			{
				return x.Key.Equals(y.Key, StringComparison.InvariantCultureIgnoreCase) && x.Value.Equals(y.Value, StringComparison.InvariantCultureIgnoreCase);
			}

			// Token: 0x0600025D RID: 605 RVA: 0x0000B77A File Offset: 0x0000997A
			public int GetHashCode(KeyValuePair<string, string> obj)
			{
				return obj.Key.GetHashCode() ^ obj.Value.GetHashCode();
			}
		}
	}
}
