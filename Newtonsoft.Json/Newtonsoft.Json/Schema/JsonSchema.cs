using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Schema
{
	// Token: 0x0200008C RID: 140
	public class JsonSchema
	{
		// Token: 0x1700015E RID: 350
		// (get) Token: 0x06000708 RID: 1800 RVA: 0x0001B494 File Offset: 0x00019694
		// (set) Token: 0x06000709 RID: 1801 RVA: 0x0001B49C File Offset: 0x0001969C
		public string Id { get; set; }

		// Token: 0x1700015F RID: 351
		// (get) Token: 0x0600070A RID: 1802 RVA: 0x0001B4A5 File Offset: 0x000196A5
		// (set) Token: 0x0600070B RID: 1803 RVA: 0x0001B4AD File Offset: 0x000196AD
		public string Title { get; set; }

		// Token: 0x17000160 RID: 352
		// (get) Token: 0x0600070C RID: 1804 RVA: 0x0001B4B6 File Offset: 0x000196B6
		// (set) Token: 0x0600070D RID: 1805 RVA: 0x0001B4BE File Offset: 0x000196BE
		public bool? Required { get; set; }

		// Token: 0x17000161 RID: 353
		// (get) Token: 0x0600070E RID: 1806 RVA: 0x0001B4C7 File Offset: 0x000196C7
		// (set) Token: 0x0600070F RID: 1807 RVA: 0x0001B4CF File Offset: 0x000196CF
		public bool? ReadOnly { get; set; }

		// Token: 0x17000162 RID: 354
		// (get) Token: 0x06000710 RID: 1808 RVA: 0x0001B4D8 File Offset: 0x000196D8
		// (set) Token: 0x06000711 RID: 1809 RVA: 0x0001B4E0 File Offset: 0x000196E0
		public bool? Hidden { get; set; }

		// Token: 0x17000163 RID: 355
		// (get) Token: 0x06000712 RID: 1810 RVA: 0x0001B4E9 File Offset: 0x000196E9
		// (set) Token: 0x06000713 RID: 1811 RVA: 0x0001B4F1 File Offset: 0x000196F1
		public bool? Transient { get; set; }

		// Token: 0x17000164 RID: 356
		// (get) Token: 0x06000714 RID: 1812 RVA: 0x0001B4FA File Offset: 0x000196FA
		// (set) Token: 0x06000715 RID: 1813 RVA: 0x0001B502 File Offset: 0x00019702
		public string Description { get; set; }

		// Token: 0x17000165 RID: 357
		// (get) Token: 0x06000716 RID: 1814 RVA: 0x0001B50B File Offset: 0x0001970B
		// (set) Token: 0x06000717 RID: 1815 RVA: 0x0001B513 File Offset: 0x00019713
		public JsonSchemaType? Type { get; set; }

		// Token: 0x17000166 RID: 358
		// (get) Token: 0x06000718 RID: 1816 RVA: 0x0001B51C File Offset: 0x0001971C
		// (set) Token: 0x06000719 RID: 1817 RVA: 0x0001B524 File Offset: 0x00019724
		public string Pattern { get; set; }

		// Token: 0x17000167 RID: 359
		// (get) Token: 0x0600071A RID: 1818 RVA: 0x0001B52D File Offset: 0x0001972D
		// (set) Token: 0x0600071B RID: 1819 RVA: 0x0001B535 File Offset: 0x00019735
		public int? MinimumLength { get; set; }

		// Token: 0x17000168 RID: 360
		// (get) Token: 0x0600071C RID: 1820 RVA: 0x0001B53E File Offset: 0x0001973E
		// (set) Token: 0x0600071D RID: 1821 RVA: 0x0001B546 File Offset: 0x00019746
		public int? MaximumLength { get; set; }

		// Token: 0x17000169 RID: 361
		// (get) Token: 0x0600071E RID: 1822 RVA: 0x0001B54F File Offset: 0x0001974F
		// (set) Token: 0x0600071F RID: 1823 RVA: 0x0001B557 File Offset: 0x00019757
		public double? DivisibleBy { get; set; }

		// Token: 0x1700016A RID: 362
		// (get) Token: 0x06000720 RID: 1824 RVA: 0x0001B560 File Offset: 0x00019760
		// (set) Token: 0x06000721 RID: 1825 RVA: 0x0001B568 File Offset: 0x00019768
		public double? Minimum { get; set; }

		// Token: 0x1700016B RID: 363
		// (get) Token: 0x06000722 RID: 1826 RVA: 0x0001B571 File Offset: 0x00019771
		// (set) Token: 0x06000723 RID: 1827 RVA: 0x0001B579 File Offset: 0x00019779
		public double? Maximum { get; set; }

		// Token: 0x1700016C RID: 364
		// (get) Token: 0x06000724 RID: 1828 RVA: 0x0001B582 File Offset: 0x00019782
		// (set) Token: 0x06000725 RID: 1829 RVA: 0x0001B58A File Offset: 0x0001978A
		public bool? ExclusiveMinimum { get; set; }

		// Token: 0x1700016D RID: 365
		// (get) Token: 0x06000726 RID: 1830 RVA: 0x0001B593 File Offset: 0x00019793
		// (set) Token: 0x06000727 RID: 1831 RVA: 0x0001B59B File Offset: 0x0001979B
		public bool? ExclusiveMaximum { get; set; }

		// Token: 0x1700016E RID: 366
		// (get) Token: 0x06000728 RID: 1832 RVA: 0x0001B5A4 File Offset: 0x000197A4
		// (set) Token: 0x06000729 RID: 1833 RVA: 0x0001B5AC File Offset: 0x000197AC
		public int? MinimumItems { get; set; }

		// Token: 0x1700016F RID: 367
		// (get) Token: 0x0600072A RID: 1834 RVA: 0x0001B5B5 File Offset: 0x000197B5
		// (set) Token: 0x0600072B RID: 1835 RVA: 0x0001B5BD File Offset: 0x000197BD
		public int? MaximumItems { get; set; }

		// Token: 0x17000170 RID: 368
		// (get) Token: 0x0600072C RID: 1836 RVA: 0x0001B5C6 File Offset: 0x000197C6
		// (set) Token: 0x0600072D RID: 1837 RVA: 0x0001B5CE File Offset: 0x000197CE
		public IList<JsonSchema> Items { get; set; }

		// Token: 0x17000171 RID: 369
		// (get) Token: 0x0600072E RID: 1838 RVA: 0x0001B5D7 File Offset: 0x000197D7
		// (set) Token: 0x0600072F RID: 1839 RVA: 0x0001B5DF File Offset: 0x000197DF
		public bool PositionalItemsValidation { get; set; }

		// Token: 0x17000172 RID: 370
		// (get) Token: 0x06000730 RID: 1840 RVA: 0x0001B5E8 File Offset: 0x000197E8
		// (set) Token: 0x06000731 RID: 1841 RVA: 0x0001B5F0 File Offset: 0x000197F0
		public JsonSchema AdditionalItems { get; set; }

		// Token: 0x17000173 RID: 371
		// (get) Token: 0x06000732 RID: 1842 RVA: 0x0001B5F9 File Offset: 0x000197F9
		// (set) Token: 0x06000733 RID: 1843 RVA: 0x0001B601 File Offset: 0x00019801
		public bool AllowAdditionalItems { get; set; }

		// Token: 0x17000174 RID: 372
		// (get) Token: 0x06000734 RID: 1844 RVA: 0x0001B60A File Offset: 0x0001980A
		// (set) Token: 0x06000735 RID: 1845 RVA: 0x0001B612 File Offset: 0x00019812
		public bool UniqueItems { get; set; }

		// Token: 0x17000175 RID: 373
		// (get) Token: 0x06000736 RID: 1846 RVA: 0x0001B61B File Offset: 0x0001981B
		// (set) Token: 0x06000737 RID: 1847 RVA: 0x0001B623 File Offset: 0x00019823
		public IDictionary<string, JsonSchema> Properties { get; set; }

		// Token: 0x17000176 RID: 374
		// (get) Token: 0x06000738 RID: 1848 RVA: 0x0001B62C File Offset: 0x0001982C
		// (set) Token: 0x06000739 RID: 1849 RVA: 0x0001B634 File Offset: 0x00019834
		public JsonSchema AdditionalProperties { get; set; }

		// Token: 0x17000177 RID: 375
		// (get) Token: 0x0600073A RID: 1850 RVA: 0x0001B63D File Offset: 0x0001983D
		// (set) Token: 0x0600073B RID: 1851 RVA: 0x0001B645 File Offset: 0x00019845
		public IDictionary<string, JsonSchema> PatternProperties { get; set; }

		// Token: 0x17000178 RID: 376
		// (get) Token: 0x0600073C RID: 1852 RVA: 0x0001B64E File Offset: 0x0001984E
		// (set) Token: 0x0600073D RID: 1853 RVA: 0x0001B656 File Offset: 0x00019856
		public bool AllowAdditionalProperties { get; set; }

		// Token: 0x17000179 RID: 377
		// (get) Token: 0x0600073E RID: 1854 RVA: 0x0001B65F File Offset: 0x0001985F
		// (set) Token: 0x0600073F RID: 1855 RVA: 0x0001B667 File Offset: 0x00019867
		public string Requires { get; set; }

		// Token: 0x1700017A RID: 378
		// (get) Token: 0x06000740 RID: 1856 RVA: 0x0001B670 File Offset: 0x00019870
		// (set) Token: 0x06000741 RID: 1857 RVA: 0x0001B678 File Offset: 0x00019878
		public IList<JToken> Enum { get; set; }

		// Token: 0x1700017B RID: 379
		// (get) Token: 0x06000742 RID: 1858 RVA: 0x0001B681 File Offset: 0x00019881
		// (set) Token: 0x06000743 RID: 1859 RVA: 0x0001B689 File Offset: 0x00019889
		public JsonSchemaType? Disallow { get; set; }

		// Token: 0x1700017C RID: 380
		// (get) Token: 0x06000744 RID: 1860 RVA: 0x0001B692 File Offset: 0x00019892
		// (set) Token: 0x06000745 RID: 1861 RVA: 0x0001B69A File Offset: 0x0001989A
		public JToken Default { get; set; }

		// Token: 0x1700017D RID: 381
		// (get) Token: 0x06000746 RID: 1862 RVA: 0x0001B6A3 File Offset: 0x000198A3
		// (set) Token: 0x06000747 RID: 1863 RVA: 0x0001B6AB File Offset: 0x000198AB
		public IList<JsonSchema> Extends { get; set; }

		// Token: 0x1700017E RID: 382
		// (get) Token: 0x06000748 RID: 1864 RVA: 0x0001B6B4 File Offset: 0x000198B4
		// (set) Token: 0x06000749 RID: 1865 RVA: 0x0001B6BC File Offset: 0x000198BC
		public string Format { get; set; }

		// Token: 0x1700017F RID: 383
		// (get) Token: 0x0600074A RID: 1866 RVA: 0x0001B6C5 File Offset: 0x000198C5
		// (set) Token: 0x0600074B RID: 1867 RVA: 0x0001B6CD File Offset: 0x000198CD
		internal string Location { get; set; }

		// Token: 0x17000180 RID: 384
		// (get) Token: 0x0600074C RID: 1868 RVA: 0x0001B6D6 File Offset: 0x000198D6
		internal string InternalId
		{
			get
			{
				return this._internalId;
			}
		}

		// Token: 0x17000181 RID: 385
		// (get) Token: 0x0600074D RID: 1869 RVA: 0x0001B6DE File Offset: 0x000198DE
		// (set) Token: 0x0600074E RID: 1870 RVA: 0x0001B6E6 File Offset: 0x000198E6
		internal string DeferredReference { get; set; }

		// Token: 0x17000182 RID: 386
		// (get) Token: 0x0600074F RID: 1871 RVA: 0x0001B6EF File Offset: 0x000198EF
		// (set) Token: 0x06000750 RID: 1872 RVA: 0x0001B6F7 File Offset: 0x000198F7
		internal bool ReferencesResolved { get; set; }

		// Token: 0x06000751 RID: 1873 RVA: 0x0001B700 File Offset: 0x00019900
		public JsonSchema()
		{
			this.AllowAdditionalProperties = true;
			this.AllowAdditionalItems = true;
		}

		// Token: 0x06000752 RID: 1874 RVA: 0x0001B739 File Offset: 0x00019939
		public static JsonSchema Read(JsonReader reader)
		{
			return JsonSchema.Read(reader, new JsonSchemaResolver());
		}

		// Token: 0x06000753 RID: 1875 RVA: 0x0001B748 File Offset: 0x00019948
		public static JsonSchema Read(JsonReader reader, JsonSchemaResolver resolver)
		{
			ValidationUtils.ArgumentNotNull(reader, "reader");
			ValidationUtils.ArgumentNotNull(resolver, "resolver");
			JsonSchemaBuilder jsonSchemaBuilder = new JsonSchemaBuilder(resolver);
			return jsonSchemaBuilder.Read(reader);
		}

		// Token: 0x06000754 RID: 1876 RVA: 0x0001B779 File Offset: 0x00019979
		public static JsonSchema Parse(string json)
		{
			return JsonSchema.Parse(json, new JsonSchemaResolver());
		}

		// Token: 0x06000755 RID: 1877 RVA: 0x0001B788 File Offset: 0x00019988
		public static JsonSchema Parse(string json, JsonSchemaResolver resolver)
		{
			ValidationUtils.ArgumentNotNull(json, "json");
			JsonSchema result;
			using (JsonReader jsonReader = new JsonTextReader(new StringReader(json)))
			{
				result = JsonSchema.Read(jsonReader, resolver);
			}
			return result;
		}

		// Token: 0x06000756 RID: 1878 RVA: 0x0001B7D4 File Offset: 0x000199D4
		public void WriteTo(JsonWriter writer)
		{
			this.WriteTo(writer, new JsonSchemaResolver());
		}

		// Token: 0x06000757 RID: 1879 RVA: 0x0001B7E4 File Offset: 0x000199E4
		public void WriteTo(JsonWriter writer, JsonSchemaResolver resolver)
		{
			ValidationUtils.ArgumentNotNull(writer, "writer");
			ValidationUtils.ArgumentNotNull(resolver, "resolver");
			JsonSchemaWriter jsonSchemaWriter = new JsonSchemaWriter(writer, resolver);
			jsonSchemaWriter.WriteSchema(this);
		}

		// Token: 0x06000758 RID: 1880 RVA: 0x0001B818 File Offset: 0x00019A18
		public override string ToString()
		{
			StringWriter stringWriter = new StringWriter(CultureInfo.InvariantCulture);
			this.WriteTo(new JsonTextWriter(stringWriter)
			{
				Formatting = Formatting.Indented
			});
			return stringWriter.ToString();
		}

		// Token: 0x04000224 RID: 548
		private readonly string _internalId = Guid.NewGuid().ToString("N");
	}
}
