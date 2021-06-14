using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.WindowsDeviceRecoveryTool.Common.Tracing;

namespace Microsoft.WindowsDeviceRecoveryTool.Model
{
	// Token: 0x0200004D RID: 77
	public class RegionAndLanguage
	{
		// Token: 0x06000268 RID: 616
		[DllImport("kernel32.dll", CallingConvention = CallingConvention.StdCall, ExactSpelling = true, SetLastError = true)]
		private static extern int GetUserGeoID(RegionAndLanguage.GeoClass geoClass);

		// Token: 0x06000269 RID: 617
		[DllImport("kernel32.dll")]
		private static extern int GetUserDefaultLCID();

		// Token: 0x0600026A RID: 618
		[DllImport("kernel32.dll")]
		private static extern int GetGeoInfo(int geoid, int geoType, StringBuilder lpGeoData, int cchData, int langid);

		// Token: 0x170000EE RID: 238
		// (get) Token: 0x0600026B RID: 619 RVA: 0x00006F48 File Offset: 0x00005148
		public static string CurrentLocation
		{
			get
			{
				return RegionAndLanguage.currentLocation;
			}
		}

		// Token: 0x0600026C RID: 620 RVA: 0x00006F60 File Offset: 0x00005160
		private static string GetMachineCurrentLocation()
		{
			RegionInfo regionInfo = RegionAndLanguage.SafelyGetRegionInfo(CultureInfo.CurrentCulture.LCID);
			string result = string.Empty;
			string text = string.Empty;
			try
			{
				int userGeoID = RegionAndLanguage.GetUserGeoID(RegionAndLanguage.GeoClass.Nation);
				int userDefaultLCID = RegionAndLanguage.GetUserDefaultLCID();
				StringBuilder stringBuilder = new StringBuilder(100);
				RegionAndLanguage.GetGeoInfo(userGeoID, 8, stringBuilder, stringBuilder.Capacity, userDefaultLCID);
				Tracer<RegionAndLanguage>.WriteInformation("GetGEoInfo returned location: {0}", new object[]
				{
					stringBuilder
				});
				Collection<Location> collection = RegionAndLanguage.CreateLocationList();
				foreach (Location location in collection)
				{
					if (location.CountryEnglishName == stringBuilder.ToString() && location.GeoId == userGeoID)
					{
						text = location.IetfLanguageTag;
					}
				}
				if (!string.IsNullOrWhiteSpace(text))
				{
					regionInfo = RegionAndLanguage.SafelyGetRegionInfo(text);
				}
				else
				{
					Tracer<RegionAndLanguage>.WriteInformation("Culture not found for: {0}. Trying read region info from location id: {1}", new object[]
					{
						stringBuilder,
						userDefaultLCID
					});
					regionInfo = RegionAndLanguage.SafelyGetRegionInfo(userDefaultLCID);
				}
				if (regionInfo != null)
				{
					result = regionInfo.TwoLetterISORegionName;
				}
				else
				{
					text = ((!string.IsNullOrWhiteSpace(text)) ? text : CultureInfo.CurrentCulture.IetfLanguageTag);
					Tracer<RegionAndLanguage>.WriteInformation("Region not found. Extracting country code from language tag: {0}", new object[]
					{
						text
					});
					result = RegionAndLanguage.ExtractCountryCodeFromLanguageTag(text);
				}
			}
			catch (Exception error)
			{
				Tracer<RegionAndLanguage>.WriteWarning(error, "Could not read culture location info: {0}", new object[]
				{
					text
				});
			}
			return result;
		}

		// Token: 0x0600026D RID: 621 RVA: 0x00007140 File Offset: 0x00005340
		private static RegionInfo SafelyGetRegionInfo(int lcid)
		{
			try
			{
				return new RegionInfo(lcid);
			}
			catch (Exception error)
			{
				Tracer<RegionAndLanguage>.WriteWarning(error, "Could not read region info: {0}", new object[]
				{
					lcid
				});
			}
			return null;
		}

		// Token: 0x0600026E RID: 622 RVA: 0x00007194 File Offset: 0x00005394
		private static RegionInfo SafelyGetRegionInfo(string languageTag)
		{
			try
			{
				return new RegionInfo(languageTag);
			}
			catch (Exception error)
			{
				Tracer<RegionAndLanguage>.WriteWarning(error, "Could not read region info: {0}", new object[]
				{
					languageTag
				});
			}
			return null;
		}

		// Token: 0x0600026F RID: 623 RVA: 0x000071E0 File Offset: 0x000053E0
		private static string ExtractCountryCodeFromLanguageTag(string ietfFlag)
		{
			Regex regex = new Regex(".*-([A-Za-z][A-Za-z])");
			Match match = regex.Match(ietfFlag);
			string result;
			if (match.Success)
			{
				result = match.Groups[1].Value;
			}
			else
			{
				result = string.Empty;
			}
			return result;
		}

		// Token: 0x06000270 RID: 624 RVA: 0x000072CC File Offset: 0x000054CC
		private static Collection<Location> CreateLocationList()
		{
			Collection<Location> collection = new Collection<Location>();
			CultureInfo[] cultures = CultureInfo.GetCultures(CultureTypes.SpecificCultures);
			collection.Add(RegionAndLanguage.CreateCountryLocationFix("en-GB", false, "", "", ""));
			collection.Add(RegionAndLanguage.CreateCountryLocationFix("ru-RU", false, "", "", ""));
			collection.Add(RegionAndLanguage.CreateCountryLocationFix("en-CA", false, "", "", ""));
			collection.Add(RegionAndLanguage.CreateCountryLocationFix("es-CL", false, "", "", ""));
			collection.Add(RegionAndLanguage.CreateCountryLocationFix("es-GT", false, "", "", ""));
			collection.Add(RegionAndLanguage.CreateCountryLocationFix("en-PH", false, "", "", ""));
			collection.Add(RegionAndLanguage.CreateCountryLocationFix("af-ZA", false, "", "", ""));
			collection.Add(RegionAndLanguage.CreateCountryLocationFix("es-EC", false, "", "", ""));
			collection.Add(RegionAndLanguage.CreateCountryLocationFix("ms-MY", false, "", "", ""));
			collection.Add(RegionAndLanguage.CreateCountryLocationFix("ko-KR", false, "한국", "", ""));
			collection.Add(RegionAndLanguage.CreateCountryLocationFix("th-TH", false, "ประเทศไทย", "", ""));
			collection.Add(RegionAndLanguage.CreateCountryLocationFix("ur-PK", true, "پاکستان", "", ""));
			collection.Add(RegionAndLanguage.CreateCountryLocationFix("tk-TM", false, "Туркменистан", "", ""));
			collection.Add(RegionAndLanguage.CreateCountryLocationFix("cs-CZ", false, "Česko", "", ""));
			collection.Add(RegionAndLanguage.CreateCountryLocationFix("sk-SK", false, "Slovensko", "", ""));
			collection.Add(RegionAndLanguage.CreateCountryLocationFix("ar-SY", true, "سورية", "", ""));
			collection.Add(RegionAndLanguage.CreateCountryLocationFix("si-LK", false, "ශ්රී ලංකා", "", ""));
			collection.Add(RegionAndLanguage.CreateCountryLocationFix("dv-MV", true, "ދިވެހި", "", ""));
			collection.Add(RegionAndLanguage.CreateCountryLocationFix("es-BO", false, "Bolivia", "", ""));
			collection.Add(RegionAndLanguage.CreateCountryLocationFix("ga-IE", false, "Ireland", "", ""));
			collection.Add(RegionAndLanguage.CreateCountryLocationFix("ms-BN", true, "بروني دارالسلام", "", ""));
			collection.Add(RegionAndLanguage.CreateCountryLocationFix("ar-DZ", true, "الجزائر", "", ""));
			collection.Add(RegionAndLanguage.CreateCountryLocationFix("ar-MA", true, "المغرب", "", ""));
			collection.Add(RegionAndLanguage.CreateCountryLocationFix("en-TT", false, "Trinidad and Tobago", "", ""));
			collection.Add(RegionAndLanguage.CreateCountryLocationFix("sr-Latn-ME", false, "Црна Гора", "", "sr-ME"));
			collection.Add(RegionAndLanguage.CreateCountryLocationFix("zh-HK", false, "香港", "Hong Kong", ""));
			collection.Add(RegionAndLanguage.CreateCountryLocationFix("es-PE", false, "Perú", "Peru", ""));
			collection.Add(RegionAndLanguage.CreateCountryLocationFix("zh-MO", false, "澳門", "Macau", ""));
			collection.Add(RegionAndLanguage.CreateCountryLocationFix("fr-MC", false, "Monaco", "Monaco", ""));
			collection.Add(RegionAndLanguage.CreateCountryLocationFix("es-VE", false, "Venezuela", "Venezuela", ""));
			collection.Add(RegionAndLanguage.CreateCountryLocationFix("sr-Latn-CS", false, "Srbija i Crna Gora", "Serbia and Montenegro", "sr-CS"));
			collection.Add(RegionAndLanguage.CreateCountryLocationFix("bo-CN", false, "中国", "China", ""));
			collection.Add(RegionAndLanguage.CreateCountryLocationFix("lo-LA", false, "ສາທາລະນະລັດ ປະຊາທິປະໄຕ ປະຊາຊົນລາວ", "Laos", ""));
			collection.Add(RegionAndLanguage.CreateCountryLocationFix("mk-MK", false, "", "Macedonia", ""));
			collection.Add(RegionAndLanguage.CreateCountryLocationFix("uz-Latn-UZ", false, "Ўзбекистон", "", "uz-UZ"));
			collection.Add(RegionAndLanguage.CreateCountryLocationFix("sr-Latn-RS", false, "", "", "sr-RS"));
			collection.Add(RegionAndLanguage.CreateCountryLocationFix("tg-Cyrl-TJ", false, "", "", "tg-TJ"));
			collection.Add(RegionAndLanguage.CreateCountryLocationFix("az-Latn-AZ", false, "", "", "az-AZ"));
			collection.Add(RegionAndLanguage.CreateCountryLocationFix("ha-Latn-NG", false, "", "", "ha-NG"));
			for (;;)
			{
				int index;
				if ((index = collection.ToList<Location>().FindIndex((Location x) => x == null)) == -1)
				{
					break;
				}
				collection.RemoveAt(index);
			}
			CultureInfo[] array = cultures;
			for (int i = 0; i < array.Length; i++)
			{
				CultureInfo cul = array[i];
				try
				{
					CultureInfo cultureInfo = new CultureInfo(cul.Name, false);
					RegionInfo country = new RegionInfo(cultureInfo.LCID);
					if (collection.Count((Location p) => p.IetfLanguageTag.Substring(p.IetfLanguageTag.LastIndexOf('-') + 1) == cul.IetfLanguageTag.Substring(cul.IetfLanguageTag.LastIndexOf('-') + 1) || p.GeoId == country.GeoId) == 0)
					{
						collection.Add(new Location(country.EnglishName, cul.IetfLanguageTag, country.GeoId));
					}
				}
				catch (Exception ex)
				{
				}
			}
			return collection;
		}

		// Token: 0x06000271 RID: 625 RVA: 0x00007914 File Offset: 0x00005B14
		private static Location CreateCountryLocationFix(string ietfLanguageTag, bool isRightToLeft, string nativeName = "", string englishName = "", string customIetfLanguageTag = "")
		{
			Location result;
			try
			{
				CultureInfo cultureInfo = new CultureInfo(ietfLanguageTag);
				RegionInfo regionInfo = new RegionInfo(new CultureInfo(cultureInfo.Name, false).LCID);
				nativeName = ((nativeName == string.Empty) ? regionInfo.NativeName : nativeName);
				englishName = ((englishName == string.Empty) ? regionInfo.EnglishName : englishName);
				ietfLanguageTag = ((customIetfLanguageTag == string.Empty) ? cultureInfo.IetfLanguageTag : customIetfLanguageTag);
				result = new Location(englishName, ietfLanguageTag, regionInfo.GeoId);
			}
			catch (Exception ex)
			{
				result = null;
			}
			return result;
		}

		// Token: 0x04000223 RID: 547
		private const int GEO_FRIENDLYNAME = 8;

		// Token: 0x04000224 RID: 548
		private static readonly string currentLocation = RegionAndLanguage.GetMachineCurrentLocation();

		// Token: 0x0200004E RID: 78
		private enum GeoClass
		{
			// Token: 0x04000227 RID: 551
			Nation = 16,
			// Token: 0x04000228 RID: 552
			Region = 14
		}
	}
}
