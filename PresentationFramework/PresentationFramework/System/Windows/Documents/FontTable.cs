using System;
using System.Collections;
using System.Globalization;
using Microsoft.Win32;

namespace System.Windows.Documents
{
	// Token: 0x020003C5 RID: 965
	internal class FontTable : ArrayList
	{
		// Token: 0x060033F1 RID: 13297 RVA: 0x000E75DE File Offset: 0x000E57DE
		internal FontTable() : base(20)
		{
			this._fontMappings = null;
		}

		// Token: 0x060033F2 RID: 13298 RVA: 0x000E75F0 File Offset: 0x000E57F0
		internal FontTableEntry DefineEntry(int index)
		{
			FontTableEntry fontTableEntry = this.FindEntryByIndex(index);
			if (fontTableEntry != null)
			{
				fontTableEntry.IsPending = true;
				fontTableEntry.Name = null;
				return fontTableEntry;
			}
			fontTableEntry = new FontTableEntry();
			fontTableEntry.Index = index;
			this.Add(fontTableEntry);
			return fontTableEntry;
		}

		// Token: 0x060033F3 RID: 13299 RVA: 0x000E7630 File Offset: 0x000E5830
		internal FontTableEntry FindEntryByIndex(int index)
		{
			for (int i = 0; i < this.Count; i++)
			{
				FontTableEntry fontTableEntry = this.EntryAt(i);
				if (fontTableEntry.Index == index)
				{
					return fontTableEntry;
				}
			}
			return null;
		}

		// Token: 0x060033F4 RID: 13300 RVA: 0x000E7664 File Offset: 0x000E5864
		internal FontTableEntry FindEntryByName(string name)
		{
			for (int i = 0; i < this.Count; i++)
			{
				FontTableEntry fontTableEntry = this.EntryAt(i);
				if (name.Equals(fontTableEntry.Name))
				{
					return fontTableEntry;
				}
			}
			return null;
		}

		// Token: 0x060033F5 RID: 13301 RVA: 0x000E769B File Offset: 0x000E589B
		internal FontTableEntry EntryAt(int index)
		{
			return (FontTableEntry)this[index];
		}

		// Token: 0x060033F6 RID: 13302 RVA: 0x000E76AC File Offset: 0x000E58AC
		internal int DefineEntryByName(string name)
		{
			int num = -1;
			for (int i = 0; i < this.Count; i++)
			{
				FontTableEntry fontTableEntry = this.EntryAt(i);
				if (name.Equals(fontTableEntry.Name))
				{
					return fontTableEntry.Index;
				}
				if (fontTableEntry.Index > num)
				{
					num = fontTableEntry.Index;
				}
			}
			FontTableEntry fontTableEntry2 = new FontTableEntry();
			fontTableEntry2.Index = num + 1;
			this.Add(fontTableEntry2);
			fontTableEntry2.Name = name;
			return num + 1;
		}

		// Token: 0x060033F7 RID: 13303 RVA: 0x000E771C File Offset: 0x000E591C
		internal void MapFonts()
		{
			Hashtable fontMappings = this.FontMappings;
			for (int i = 0; i < this.Count; i++)
			{
				FontTableEntry fontTableEntry = this.EntryAt(i);
				if (fontTableEntry.Name != null)
				{
					string text = (string)fontMappings[fontTableEntry.Name.ToLower(CultureInfo.InvariantCulture)];
					if (text != null)
					{
						fontTableEntry.Name = text;
					}
					else
					{
						int num = fontTableEntry.Name.IndexOf('(');
						if (num >= 0)
						{
							while (num > 0 && fontTableEntry.Name[num - 1] == ' ')
							{
								num--;
							}
							fontTableEntry.Name = fontTableEntry.Name.Substring(0, num);
						}
					}
				}
			}
		}

		// Token: 0x17000D54 RID: 3412
		// (get) Token: 0x060033F8 RID: 13304 RVA: 0x000E77C8 File Offset: 0x000E59C8
		internal FontTableEntry CurrentEntry
		{
			get
			{
				if (this.Count == 0)
				{
					return null;
				}
				for (int i = this.Count - 1; i >= 0; i--)
				{
					FontTableEntry fontTableEntry = this.EntryAt(i);
					if (fontTableEntry.IsPending)
					{
						return fontTableEntry;
					}
				}
				return this.EntryAt(this.Count - 1);
			}
		}

		// Token: 0x17000D55 RID: 3413
		// (get) Token: 0x060033F9 RID: 13305 RVA: 0x000E7814 File Offset: 0x000E5A14
		internal Hashtable FontMappings
		{
			get
			{
				if (this._fontMappings == null)
				{
					this._fontMappings = new Hashtable();
					RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("Software\\Microsoft\\Windows NT\\CurrentVersion\\FontSubstitutes");
					if (registryKey != null)
					{
						string[] valueNames = registryKey.GetValueNames();
						foreach (string text in valueNames)
						{
							string text2 = (string)registryKey.GetValue(text);
							if (text.Length > 0 && text2.Length > 0)
							{
								string text3 = text;
								string text4 = string.Empty;
								string text5 = text2;
								string text6 = string.Empty;
								int num = text.IndexOf(',');
								if (num >= 0)
								{
									text3 = text.Substring(0, num);
									text4 = text.Substring(num + 1, text.Length - num - 1);
								}
								num = text2.IndexOf(',');
								if (num >= 0)
								{
									text5 = text2.Substring(0, num);
									text6 = text2.Substring(num + 1, text2.Length - num - 1);
								}
								if (text3.Length > 0 && text5.Length > 0)
								{
									bool flag = false;
									if (text4.Length > 0 && text6.Length > 0)
									{
										if (string.Compare(text4, text6, StringComparison.OrdinalIgnoreCase) == 0)
										{
											flag = true;
										}
									}
									else if (text4.Length == 0 && text6.Length == 0)
									{
										if (text3.Length > text5.Length)
										{
											string strA = text3.Substring(0, text5.Length);
											if (string.Compare(strA, text5, StringComparison.OrdinalIgnoreCase) == 0)
											{
												flag = true;
											}
										}
									}
									else if (text4.Length > 0 && text6.Length == 0)
									{
										flag = true;
									}
									if (flag)
									{
										string key = text3.ToLower(CultureInfo.InvariantCulture);
										if (this._fontMappings[key] == null)
										{
											this._fontMappings.Add(key, text5);
										}
									}
								}
							}
						}
					}
				}
				return this._fontMappings;
			}
		}

		// Token: 0x040024B7 RID: 9399
		private Hashtable _fontMappings;
	}
}
