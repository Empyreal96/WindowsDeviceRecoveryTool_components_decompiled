using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.WindowsDeviceRecoveryTool.LumiaAdaptation.Services
{
	// Token: 0x02000009 RID: 9
	public class SalesNames
	{
		// Token: 0x06000085 RID: 133 RVA: 0x00005D98 File Offset: 0x00003F98
		public static string NameForPackageName(string packageName)
		{
			return string.IsNullOrEmpty(packageName) ? string.Empty : SalesNames.NameForProductType(packageName.Split(new char[]
			{
				' '
			}).First<string>());
		}

		// Token: 0x06000086 RID: 134 RVA: 0x00005E08 File Offset: 0x00004008
		public static string NameForProductType(string productType)
		{
			string value = SalesNames.SalesNamesDictionary.FirstOrDefault((KeyValuePair<string, string> item) => item.Key == productType.ToUpper()).Value;
			return string.IsNullOrEmpty(value) ? productType : value;
		}

		// Token: 0x04000039 RID: 57
		private static readonly Dictionary<string, string> SalesNamesDictionary = new Dictionary<string, string>
		{
			{
				"RM-820",
				"Lumia 920"
			},
			{
				"RM-821",
				"Lumia 920"
			},
			{
				"RM-824",
				"Lumia 820"
			},
			{
				"RM-825",
				"Lumia 820"
			},
			{
				"RM-846",
				"Lumia 620"
			},
			{
				"RM-860",
				"Lumia 928"
			},
			{
				"RM-867",
				"Lumia 920T"
			},
			{
				"RM-875",
				"Lumia 1020"
			},
			{
				"RM-876",
				"Lumia 1020"
			},
			{
				"RM-877",
				"Lumia 1020"
			},
			{
				"RM-885",
				"Lumia 720"
			},
			{
				"RM-887",
				"Lumia 720T"
			},
			{
				"RM-892",
				"Lumia 925"
			},
			{
				"RM-910",
				"Lumia 925"
			},
			{
				"RM-913",
				"Lumia 520T"
			},
			{
				"RM-914",
				"Lumia 520"
			},
			{
				"RM-915",
				"Lumia 520"
			},
			{
				"RM-917",
				"Lumia 521"
			},
			{
				"RM-927",
				"Lumia Icon"
			},
			{
				"RM-937",
				"Lumia 1520"
			},
			{
				"RM-938",
				"Lumia 1520"
			},
			{
				"RM-939",
				"Lumia 1520"
			},
			{
				"RM-940",
				"Lumia 1520"
			},
			{
				"RM-941",
				"Lumia 625"
			},
			{
				"RM-942",
				"Lumia 625"
			},
			{
				"RM-955",
				"Lumia 925T"
			},
			{
				"RM-974",
				"Lumia 635"
			},
			{
				"RM-975",
				"Lumia 635"
			},
			{
				"RM-976",
				"Lumia 630"
			},
			{
				"RM-977",
				"Lumia 630"
			},
			{
				"RM-978",
				"Lumia 630 Dual SIM"
			},
			{
				"RM-979",
				"Lumia 630 Dual SIM"
			},
			{
				"RM-983",
				"Lumia 830"
			},
			{
				"RM-984",
				"Lumia 830"
			},
			{
				"RM-985",
				"Lumia 830"
			},
			{
				"RM-994",
				"Lumia 1320"
			},
			{
				"RM-995",
				"Lumia 1320"
			},
			{
				"RM-996",
				"Lumia 1320"
			},
			{
				"RM-997",
				"Lumia 526"
			},
			{
				"RM-998",
				"Lumia 525"
			},
			{
				"RM-1010",
				"Lumia 638"
			},
			{
				"RM-1017",
				"Lumia 530"
			},
			{
				"RM-1018",
				"Lumia 530"
			},
			{
				"RM-1019",
				"Lumia 530 Dual SIM"
			},
			{
				"RM-1020",
				"Lumia 530 Dual SIM"
			},
			{
				"RM-1027",
				"Lumia 636"
			},
			{
				"RM-1031",
				"Lumia 532 Dual SIM"
			},
			{
				"RM-1032",
				"Lumia 532 Dual SIM"
			},
			{
				"RM-1034",
				"Lumia 532"
			},
			{
				"RM-1038",
				"Lumia 735"
			},
			{
				"RM-1039",
				"Lumia 735"
			},
			{
				"RM-1040",
				"Lumia 730 Dual SIM"
			},
			{
				"RM-1045",
				"Lumia 930"
			},
			{
				"RM-1049",
				"Lumia 830"
			},
			{
				"RM-1062",
				"Lumia 640 XL LTE"
			},
			{
				"RM-1064",
				"Lumia 640 XL LTE"
			},
			{
				"RM-1066",
				"Lumia 640 XL"
			},
			{
				"RM-1067",
				"Lumia 640 XL Dual SIM"
			},
			{
				"RM-1068",
				"Lumia 435 Dual SIM"
			},
			{
				"RM-1069",
				"Lumia 435 Dual SIM"
			},
			{
				"RM-1070",
				"Lumia 435"
			},
			{
				"RM-1071",
				"Lumia 435"
			},
			{
				"RM-1072",
				"Lumia 640 LTE"
			},
			{
				"RM-1073",
				"Lumia 640 LTE"
			},
			{
				"RM-1075",
				"Lumia 640 LTE Dual SIM"
			},
			{
				"RM-1077",
				"Lumia 640 Dual SIM"
			},
			{
				"RM-1078",
				"Lumia 635"
			},
			{
				"RM-1087",
				"Lumia 930"
			},
			{
				"RM-1089",
				"Lumia 535"
			},
			{
				"RM-1090",
				"Lumia 535 Dual SIM"
			},
			{
				"RM-1091",
				"Lumia 535"
			},
			{
				"RM-1092",
				"Lumia 535 Dual SIM"
			},
			{
				"RM-1109",
				"Lumia 640 Dual SIM DTV"
			},
			{
				"RM-1113",
				"Lumia 640 LTE Dual SIM"
			},
			{
				"RM-1114",
				"Lumia 435 Dual SIM DTV"
			},
			{
				"RM-1115",
				"Lumia 532 Dual SIM DTV"
			}
		};
	}
}
