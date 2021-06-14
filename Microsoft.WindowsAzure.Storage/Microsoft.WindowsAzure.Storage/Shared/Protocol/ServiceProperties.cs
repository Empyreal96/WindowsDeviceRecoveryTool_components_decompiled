using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using Microsoft.WindowsAzure.Storage.Core.Util;

namespace Microsoft.WindowsAzure.Storage.Shared.Protocol
{
	// Token: 0x02000166 RID: 358
	public sealed class ServiceProperties
	{
		// Token: 0x06001531 RID: 5425 RVA: 0x000505EE File Offset: 0x0004E7EE
		public ServiceProperties()
		{
			this.Logging = new LoggingProperties();
			this.HourMetrics = new MetricsProperties();
			this.MinuteMetrics = new MetricsProperties();
			this.Cors = new CorsProperties();
		}

		// Token: 0x17000368 RID: 872
		// (get) Token: 0x06001532 RID: 5426 RVA: 0x00050622 File Offset: 0x0004E822
		// (set) Token: 0x06001533 RID: 5427 RVA: 0x0005062A File Offset: 0x0004E82A
		public LoggingProperties Logging { get; set; }

		// Token: 0x17000369 RID: 873
		// (get) Token: 0x06001534 RID: 5428 RVA: 0x00050633 File Offset: 0x0004E833
		// (set) Token: 0x06001535 RID: 5429 RVA: 0x0005063B File Offset: 0x0004E83B
		[Obsolete("Metrics has been renamed to HourMetrics.")]
		public MetricsProperties Metrics
		{
			get
			{
				return this.HourMetrics;
			}
			set
			{
				this.HourMetrics = value;
			}
		}

		// Token: 0x1700036A RID: 874
		// (get) Token: 0x06001536 RID: 5430 RVA: 0x00050644 File Offset: 0x0004E844
		// (set) Token: 0x06001537 RID: 5431 RVA: 0x0005064C File Offset: 0x0004E84C
		public MetricsProperties HourMetrics { get; set; }

		// Token: 0x1700036B RID: 875
		// (get) Token: 0x06001538 RID: 5432 RVA: 0x00050655 File Offset: 0x0004E855
		// (set) Token: 0x06001539 RID: 5433 RVA: 0x0005065D File Offset: 0x0004E85D
		public CorsProperties Cors { get; set; }

		// Token: 0x1700036C RID: 876
		// (get) Token: 0x0600153A RID: 5434 RVA: 0x00050666 File Offset: 0x0004E866
		// (set) Token: 0x0600153B RID: 5435 RVA: 0x0005066E File Offset: 0x0004E86E
		public MetricsProperties MinuteMetrics { get; set; }

		// Token: 0x1700036D RID: 877
		// (get) Token: 0x0600153C RID: 5436 RVA: 0x00050677 File Offset: 0x0004E877
		// (set) Token: 0x0600153D RID: 5437 RVA: 0x0005067F File Offset: 0x0004E87F
		public string DefaultServiceVersion { get; set; }

		// Token: 0x0600153E RID: 5438 RVA: 0x00050688 File Offset: 0x0004E888
		internal static ServiceProperties FromServiceXml(XDocument servicePropertiesDocument)
		{
			XElement xelement = servicePropertiesDocument.Element("StorageServiceProperties");
			ServiceProperties serviceProperties = new ServiceProperties
			{
				Logging = ServiceProperties.ReadLoggingPropertiesFromXml(xelement.Element("Logging")),
				HourMetrics = ServiceProperties.ReadMetricsPropertiesFromXml(xelement.Element("HourMetrics")),
				MinuteMetrics = ServiceProperties.ReadMetricsPropertiesFromXml(xelement.Element("MinuteMetrics")),
				Cors = ServiceProperties.ReadCorsPropertiesFromXml(xelement.Element("Cors"))
			};
			XElement xelement2 = xelement.Element("DefaultServiceVersion");
			if (xelement2 != null)
			{
				serviceProperties.DefaultServiceVersion = xelement2.Value;
			}
			return serviceProperties;
		}

		// Token: 0x0600153F RID: 5439 RVA: 0x0005073C File Offset: 0x0004E93C
		internal XDocument ToServiceXml()
		{
			XElement xelement = new XElement("StorageServiceProperties");
			if (this.Logging != null)
			{
				xelement.Add(ServiceProperties.GenerateLoggingXml(this.Logging));
			}
			if (this.HourMetrics != null)
			{
				xelement.Add(ServiceProperties.GenerateMetricsXml(this.HourMetrics, "HourMetrics"));
			}
			if (this.MinuteMetrics != null)
			{
				xelement.Add(ServiceProperties.GenerateMetricsXml(this.MinuteMetrics, "MinuteMetrics"));
			}
			if (this.Cors != null)
			{
				xelement.Add(ServiceProperties.GenerateCorsXml(this.Cors));
			}
			if (this.DefaultServiceVersion != null)
			{
				xelement.Add(new XElement("DefaultServiceVersion", this.DefaultServiceVersion));
			}
			return new XDocument(new object[]
			{
				xelement
			});
		}

		// Token: 0x06001540 RID: 5440 RVA: 0x000507FC File Offset: 0x0004E9FC
		private static XElement GenerateRetentionPolicyXml(int? retentionDays)
		{
			bool flag = retentionDays != null;
			XElement xelement = new XElement("RetentionPolicy", new XElement("Enabled", flag));
			if (flag)
			{
				xelement.Add(new XElement("Days", retentionDays.Value));
			}
			return xelement;
		}

		// Token: 0x06001541 RID: 5441 RVA: 0x0005085C File Offset: 0x0004EA5C
		private static XElement GenerateMetricsXml(MetricsProperties metrics, string metricsName)
		{
			if (!Enum.IsDefined(typeof(MetricsLevel), metrics.MetricsLevel))
			{
				throw new InvalidOperationException("Invalid metrics level specified.");
			}
			if (string.IsNullOrEmpty(metrics.Version))
			{
				throw new InvalidOperationException("The metrics version is null or empty.");
			}
			bool flag = metrics.MetricsLevel != MetricsLevel.None;
			XElement xelement = new XElement(metricsName, new object[]
			{
				new XElement("Version", metrics.Version),
				new XElement("Enabled", flag),
				ServiceProperties.GenerateRetentionPolicyXml(metrics.RetentionDays)
			});
			if (flag)
			{
				xelement.Add(new XElement("IncludeAPIs", metrics.MetricsLevel == MetricsLevel.ServiceAndApi));
			}
			return xelement;
		}

		// Token: 0x06001542 RID: 5442 RVA: 0x00050930 File Offset: 0x0004EB30
		private static XElement GenerateLoggingXml(LoggingProperties logging)
		{
			if ((LoggingOperations.All & logging.LoggingOperations) != logging.LoggingOperations)
			{
				throw new InvalidOperationException("Invalid logging operations specified.");
			}
			if (string.IsNullOrEmpty(logging.Version))
			{
				throw new InvalidOperationException("The logging version is null or empty.");
			}
			return new XElement("Logging", new object[]
			{
				new XElement("Version", logging.Version),
				new XElement("Delete", (logging.LoggingOperations & LoggingOperations.Delete) != LoggingOperations.None),
				new XElement("Read", (logging.LoggingOperations & LoggingOperations.Read) != LoggingOperations.None),
				new XElement("Write", (logging.LoggingOperations & LoggingOperations.Write) != LoggingOperations.None),
				ServiceProperties.GenerateRetentionPolicyXml(logging.RetentionDays)
			});
		}

		// Token: 0x06001543 RID: 5443 RVA: 0x00050A1C File Offset: 0x0004EC1C
		private static XElement GenerateCorsXml(CorsProperties cors)
		{
			CommonUtility.AssertNotNull("cors", cors);
			IList<CorsRule> corsRules = cors.CorsRules;
			XElement xelement = new XElement("Cors");
			foreach (CorsRule corsRule in corsRules)
			{
				if (corsRule.AllowedOrigins.Count < 1 || corsRule.AllowedMethods == CorsHttpMethods.None || corsRule.MaxAgeInSeconds < 0)
				{
					throw new InvalidOperationException("A CORS rule must contain at least one allowed origin and allowed method, and MaxAgeInSeconds cannot have a value less than zero.");
				}
				XElement content = new XElement("CorsRule", new object[]
				{
					new XElement("AllowedOrigins", string.Join(",", corsRule.AllowedOrigins.ToArray<string>())),
					new XElement("AllowedMethods", corsRule.AllowedMethods.ToString().Replace(" ", string.Empty).ToUpperInvariant()),
					new XElement("ExposedHeaders", string.Join(",", corsRule.ExposedHeaders.ToArray<string>())),
					new XElement("AllowedHeaders", string.Join(",", corsRule.AllowedHeaders.ToArray<string>())),
					new XElement("MaxAgeInSeconds", corsRule.MaxAgeInSeconds)
				});
				xelement.Add(content);
			}
			return xelement;
		}

		// Token: 0x06001544 RID: 5444 RVA: 0x00050BB0 File Offset: 0x0004EDB0
		private static LoggingProperties ReadLoggingPropertiesFromXml(XElement element)
		{
			LoggingOperations loggingOperations = LoggingOperations.None;
			if (bool.Parse(element.Element("Delete").Value))
			{
				loggingOperations |= LoggingOperations.Delete;
			}
			if (bool.Parse(element.Element("Read").Value))
			{
				loggingOperations |= LoggingOperations.Read;
			}
			if (bool.Parse(element.Element("Write").Value))
			{
				loggingOperations |= LoggingOperations.Write;
			}
			return new LoggingProperties
			{
				Version = element.Element("Version").Value,
				LoggingOperations = loggingOperations,
				RetentionDays = ServiceProperties.ReadRetentionPolicyFromXml(element.Element("RetentionPolicy"))
			};
		}

		// Token: 0x06001545 RID: 5445 RVA: 0x00050C64 File Offset: 0x0004EE64
		private static MetricsProperties ReadMetricsPropertiesFromXml(XElement element)
		{
			MetricsLevel metricsLevel = MetricsLevel.None;
			if (bool.Parse(element.Element("Enabled").Value))
			{
				metricsLevel = MetricsLevel.Service;
				if (bool.Parse(element.Element("IncludeAPIs").Value))
				{
					metricsLevel = MetricsLevel.ServiceAndApi;
				}
			}
			return new MetricsProperties
			{
				Version = element.Element("Version").Value,
				MetricsLevel = metricsLevel,
				RetentionDays = ServiceProperties.ReadRetentionPolicyFromXml(element.Element("RetentionPolicy"))
			};
		}

		// Token: 0x06001546 RID: 5446 RVA: 0x00050DF8 File Offset: 0x0004EFF8
		internal static CorsProperties ReadCorsPropertiesFromXml(XElement element)
		{
			CorsProperties corsProperties = new CorsProperties();
			if (element == null)
			{
				return corsProperties;
			}
			IEnumerable<XElement> source = element.Descendants("CorsRule");
			corsProperties.CorsRules = (from rule in source
			select new CorsRule
			{
				AllowedOrigins = rule.Element("AllowedOrigins").Value.Split(new char[]
				{
					','
				}).ToList<string>(),
				AllowedMethods = (CorsHttpMethods)Enum.Parse(typeof(CorsHttpMethods), rule.Element("AllowedMethods").Value, true),
				AllowedHeaders = rule.Element("AllowedHeaders").Value.Split(new char[]
				{
					','
				}, StringSplitOptions.RemoveEmptyEntries).ToList<string>(),
				ExposedHeaders = rule.Element("ExposedHeaders").Value.Split(new char[]
				{
					','
				}, StringSplitOptions.RemoveEmptyEntries).ToList<string>(),
				MaxAgeInSeconds = int.Parse(rule.Element("MaxAgeInSeconds").Value, CultureInfo.InvariantCulture)
			}).ToList<CorsRule>();
			return corsProperties;
		}

		// Token: 0x06001547 RID: 5447 RVA: 0x00050E50 File Offset: 0x0004F050
		private static int? ReadRetentionPolicyFromXml(XElement element)
		{
			if (!bool.Parse(element.Element("Enabled").Value))
			{
				return null;
			}
			return new int?(int.Parse(element.Element("Days").Value, CultureInfo.InvariantCulture));
		}

		// Token: 0x06001548 RID: 5448 RVA: 0x00050EA8 File Offset: 0x0004F0A8
		internal void WriteServiceProperties(Stream outputStream)
		{
			XDocument xdocument = this.ToServiceXml();
			using (XmlWriter xmlWriter = XmlWriter.Create(outputStream, new XmlWriterSettings
			{
				Encoding = Encoding.UTF8,
				NewLineHandling = NewLineHandling.Entitize
			}))
			{
				xdocument.Save(xmlWriter);
			}
		}

		// Token: 0x040009AC RID: 2476
		internal const string StorageServicePropertiesName = "StorageServiceProperties";

		// Token: 0x040009AD RID: 2477
		internal const string LoggingName = "Logging";

		// Token: 0x040009AE RID: 2478
		internal const string HourMetricsName = "HourMetrics";

		// Token: 0x040009AF RID: 2479
		internal const string CorsName = "Cors";

		// Token: 0x040009B0 RID: 2480
		internal const string MinuteMetricsName = "MinuteMetrics";

		// Token: 0x040009B1 RID: 2481
		internal const string VersionName = "Version";

		// Token: 0x040009B2 RID: 2482
		internal const string DeleteName = "Delete";

		// Token: 0x040009B3 RID: 2483
		internal const string ReadName = "Read";

		// Token: 0x040009B4 RID: 2484
		internal const string WriteName = "Write";

		// Token: 0x040009B5 RID: 2485
		internal const string RetentionPolicyName = "RetentionPolicy";

		// Token: 0x040009B6 RID: 2486
		internal const string EnabledName = "Enabled";

		// Token: 0x040009B7 RID: 2487
		internal const string DaysName = "Days";

		// Token: 0x040009B8 RID: 2488
		internal const string IncludeApisName = "IncludeAPIs";

		// Token: 0x040009B9 RID: 2489
		internal const string DefaultServiceVersionName = "DefaultServiceVersion";

		// Token: 0x040009BA RID: 2490
		internal const string CorsRuleName = "CorsRule";

		// Token: 0x040009BB RID: 2491
		internal const string AllowedOriginsName = "AllowedOrigins";

		// Token: 0x040009BC RID: 2492
		internal const string AllowedMethodsName = "AllowedMethods";

		// Token: 0x040009BD RID: 2493
		internal const string MaxAgeInSecondsName = "MaxAgeInSeconds";

		// Token: 0x040009BE RID: 2494
		internal const string ExposedHeadersName = "ExposedHeaders";

		// Token: 0x040009BF RID: 2495
		internal const string AllowedHeadersName = "AllowedHeaders";
	}
}
