using System;

namespace Newtonsoft.Json.Utilities
{
	// Token: 0x020000DD RID: 221
	internal struct DateTimeParser
	{
		// Token: 0x06000AA5 RID: 2725 RVA: 0x0002A50A File Offset: 0x0002870A
		public bool Parse(string text)
		{
			this._text = text;
			this._length = text.Length;
			return this.ParseDate(0) && this.ParseChar(DateTimeParser.Lzyyyy_MM_dd, 'T') && this.ParseTimeAndZoneAndWhitespace(DateTimeParser.Lzyyyy_MM_ddT);
		}

		// Token: 0x06000AA6 RID: 2726 RVA: 0x0002A548 File Offset: 0x00028748
		private bool ParseDate(int start)
		{
			return this.Parse4Digit(start, out this.Year) && 1 <= this.Year && this.ParseChar(start + DateTimeParser.Lzyyyy, '-') && this.Parse2Digit(start + DateTimeParser.Lzyyyy_, out this.Month) && 1 <= this.Month && this.Month <= 12 && this.ParseChar(start + DateTimeParser.Lzyyyy_MM, '-') && this.Parse2Digit(start + DateTimeParser.Lzyyyy_MM_, out this.Day) && 1 <= this.Day && this.Day <= DateTime.DaysInMonth(this.Year, this.Month);
		}

		// Token: 0x06000AA7 RID: 2727 RVA: 0x0002A5F9 File Offset: 0x000287F9
		private bool ParseTimeAndZoneAndWhitespace(int start)
		{
			return this.ParseTime(ref start) && this.ParseZone(start);
		}

		// Token: 0x06000AA8 RID: 2728 RVA: 0x0002A610 File Offset: 0x00028810
		private bool ParseTime(ref int start)
		{
			if (!this.Parse2Digit(start, out this.Hour) || this.Hour >= 24 || !this.ParseChar(start + DateTimeParser.LzHH, ':') || !this.Parse2Digit(start + DateTimeParser.LzHH_, out this.Minute) || this.Minute >= 60 || !this.ParseChar(start + DateTimeParser.LzHH_mm, ':') || !this.Parse2Digit(start + DateTimeParser.LzHH_mm_, out this.Second) || this.Second >= 60)
			{
				return false;
			}
			start += DateTimeParser.LzHH_mm_ss;
			if (this.ParseChar(start, '.'))
			{
				this.Fraction = 0;
				int num = 0;
				while (++start < this._length && num < 7)
				{
					int num2 = (int)(this._text[start] - '0');
					if (num2 < 0 || num2 > 9)
					{
						break;
					}
					this.Fraction = this.Fraction * 10 + num2;
					num++;
				}
				if (num < 7)
				{
					if (num == 0)
					{
						return false;
					}
					this.Fraction *= DateTimeParser.Power10[7 - num];
				}
			}
			return true;
		}

		// Token: 0x06000AA9 RID: 2729 RVA: 0x0002A724 File Offset: 0x00028924
		private bool ParseZone(int start)
		{
			if (start < this._length)
			{
				char c = this._text[start];
				if (c == 'Z' || c == 'z')
				{
					this.Zone = ParserTimeZone.Utc;
					start++;
				}
				else
				{
					if (start + 2 < this._length && this.Parse2Digit(start + DateTimeParser.Lz_, out this.ZoneHour) && this.ZoneHour <= 99)
					{
						switch (c)
						{
						case '+':
							this.Zone = ParserTimeZone.LocalEastOfUtc;
							start += DateTimeParser.Lz_zz;
							break;
						case '-':
							this.Zone = ParserTimeZone.LocalWestOfUtc;
							start += DateTimeParser.Lz_zz;
							break;
						}
					}
					if (start < this._length)
					{
						if (this.ParseChar(start, ':'))
						{
							start++;
							if (start + 1 < this._length && this.Parse2Digit(start, out this.ZoneMinute) && this.ZoneMinute <= 99)
							{
								start += 2;
							}
						}
						else if (start + 1 < this._length && this.Parse2Digit(start, out this.ZoneMinute) && this.ZoneMinute <= 99)
						{
							start += 2;
						}
					}
				}
			}
			return start == this._length;
		}

		// Token: 0x06000AAA RID: 2730 RVA: 0x0002A840 File Offset: 0x00028A40
		private bool Parse4Digit(int start, out int num)
		{
			if (start + 3 < this._length)
			{
				int num2 = (int)(this._text[start] - '0');
				int num3 = (int)(this._text[start + 1] - '0');
				int num4 = (int)(this._text[start + 2] - '0');
				int num5 = (int)(this._text[start + 3] - '0');
				if (0 <= num2 && num2 < 10 && 0 <= num3 && num3 < 10 && 0 <= num4 && num4 < 10 && 0 <= num5 && num5 < 10)
				{
					num = ((num2 * 10 + num3) * 10 + num4) * 10 + num5;
					return true;
				}
			}
			num = 0;
			return false;
		}

		// Token: 0x06000AAB RID: 2731 RVA: 0x0002A8DC File Offset: 0x00028ADC
		private bool Parse2Digit(int start, out int num)
		{
			if (start + 1 < this._length)
			{
				int num2 = (int)(this._text[start] - '0');
				int num3 = (int)(this._text[start + 1] - '0');
				if (0 <= num2 && num2 < 10 && 0 <= num3 && num3 < 10)
				{
					num = num2 * 10 + num3;
					return true;
				}
			}
			num = 0;
			return false;
		}

		// Token: 0x06000AAC RID: 2732 RVA: 0x0002A936 File Offset: 0x00028B36
		private bool ParseChar(int start, char ch)
		{
			return start < this._length && this._text[start] == ch;
		}

		// Token: 0x040003CE RID: 974
		private const short MaxFractionDigits = 7;

		// Token: 0x040003CF RID: 975
		public int Year;

		// Token: 0x040003D0 RID: 976
		public int Month;

		// Token: 0x040003D1 RID: 977
		public int Day;

		// Token: 0x040003D2 RID: 978
		public int Hour;

		// Token: 0x040003D3 RID: 979
		public int Minute;

		// Token: 0x040003D4 RID: 980
		public int Second;

		// Token: 0x040003D5 RID: 981
		public int Fraction;

		// Token: 0x040003D6 RID: 982
		public int ZoneHour;

		// Token: 0x040003D7 RID: 983
		public int ZoneMinute;

		// Token: 0x040003D8 RID: 984
		public ParserTimeZone Zone;

		// Token: 0x040003D9 RID: 985
		private string _text;

		// Token: 0x040003DA RID: 986
		private int _length;

		// Token: 0x040003DB RID: 987
		private static readonly int[] Power10 = new int[]
		{
			-1,
			10,
			100,
			1000,
			10000,
			100000,
			1000000
		};

		// Token: 0x040003DC RID: 988
		private static readonly int Lzyyyy = "yyyy".Length;

		// Token: 0x040003DD RID: 989
		private static readonly int Lzyyyy_ = "yyyy-".Length;

		// Token: 0x040003DE RID: 990
		private static readonly int Lzyyyy_MM = "yyyy-MM".Length;

		// Token: 0x040003DF RID: 991
		private static readonly int Lzyyyy_MM_ = "yyyy-MM-".Length;

		// Token: 0x040003E0 RID: 992
		private static readonly int Lzyyyy_MM_dd = "yyyy-MM-dd".Length;

		// Token: 0x040003E1 RID: 993
		private static readonly int Lzyyyy_MM_ddT = "yyyy-MM-ddT".Length;

		// Token: 0x040003E2 RID: 994
		private static readonly int LzHH = "HH".Length;

		// Token: 0x040003E3 RID: 995
		private static readonly int LzHH_ = "HH:".Length;

		// Token: 0x040003E4 RID: 996
		private static readonly int LzHH_mm = "HH:mm".Length;

		// Token: 0x040003E5 RID: 997
		private static readonly int LzHH_mm_ = "HH:mm:".Length;

		// Token: 0x040003E6 RID: 998
		private static readonly int LzHH_mm_ss = "HH:mm:ss".Length;

		// Token: 0x040003E7 RID: 999
		private static readonly int Lz_ = "-".Length;

		// Token: 0x040003E8 RID: 1000
		private static readonly int Lz_zz = "-zz".Length;
	}
}
