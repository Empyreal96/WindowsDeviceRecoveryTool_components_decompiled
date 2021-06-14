using System;
using System.Collections.Generic;

namespace Microsoft.Data.OData
{
	// Token: 0x02000134 RID: 308
	public sealed class ODataPreferenceHeader
	{
		// Token: 0x06000819 RID: 2073 RVA: 0x0001A9D7 File Offset: 0x00018BD7
		internal ODataPreferenceHeader(IODataRequestMessage requestMessage)
		{
			this.message = new ODataRequestMessage(requestMessage, true, false, -1L);
			this.preferenceHeaderName = "Prefer";
		}

		// Token: 0x0600081A RID: 2074 RVA: 0x0001A9FA File Offset: 0x00018BFA
		internal ODataPreferenceHeader(IODataResponseMessage responseMessage)
		{
			this.message = new ODataResponseMessage(responseMessage, true, false, -1L);
			this.preferenceHeaderName = "Preference-Applied";
		}

		// Token: 0x17000206 RID: 518
		// (get) Token: 0x0600081B RID: 2075 RVA: 0x0001AA20 File Offset: 0x00018C20
		// (set) Token: 0x0600081C RID: 2076 RVA: 0x0001AA60 File Offset: 0x00018C60
		public bool? ReturnContent
		{
			get
			{
				if (this.PreferenceExists("return-content"))
				{
					return new bool?(true);
				}
				if (this.PreferenceExists("return-no-content"))
				{
					return new bool?(false);
				}
				return null;
			}
			set
			{
				this.Clear("return-content");
				this.Clear("return-no-content");
				if (value == true)
				{
					this.Set(ODataPreferenceHeader.ReturnContentPreference);
				}
				if (value == false)
				{
					this.Set(ODataPreferenceHeader.ReturnNoContentPreference);
				}
			}
		}

		// Token: 0x17000207 RID: 519
		// (get) Token: 0x0600081D RID: 2077 RVA: 0x0001AAC8 File Offset: 0x00018CC8
		// (set) Token: 0x0600081E RID: 2078 RVA: 0x0001AAFE File Offset: 0x00018CFE
		public string AnnotationFilter
		{
			get
			{
				HttpHeaderValueElement httpHeaderValueElement = this.Get("odata.include-annotations");
				if (httpHeaderValueElement != null)
				{
					return httpHeaderValueElement.Value.Trim(new char[]
					{
						'"'
					});
				}
				return null;
			}
			set
			{
				ExceptionUtils.CheckArgumentStringNotEmpty(value, "AnnotationFilter");
				if (value == null)
				{
					this.Clear("odata.include-annotations");
					return;
				}
				this.Set(new HttpHeaderValueElement("odata.include-annotations", ODataPreferenceHeader.AddQuotes(value), ODataPreferenceHeader.EmptyParameters));
			}
		}

		// Token: 0x17000208 RID: 520
		// (get) Token: 0x0600081F RID: 2079 RVA: 0x0001AB38 File Offset: 0x00018D38
		private HttpHeaderValue Preferences
		{
			get
			{
				HttpHeaderValue result;
				if ((result = this.preferences) == null)
				{
					result = (this.preferences = this.ParsePreferences());
				}
				return result;
			}
		}

		// Token: 0x06000820 RID: 2080 RVA: 0x0001AB5E File Offset: 0x00018D5E
		private static string AddQuotes(string text)
		{
			return "\"" + text + "\"";
		}

		// Token: 0x06000821 RID: 2081 RVA: 0x0001AB70 File Offset: 0x00018D70
		private bool PreferenceExists(string preference)
		{
			return this.Get(preference) != null;
		}

		// Token: 0x06000822 RID: 2082 RVA: 0x0001AB7F File Offset: 0x00018D7F
		private void Clear(string preference)
		{
			if (this.Preferences.Remove(preference))
			{
				this.SetPreferencesToMessageHeader();
			}
		}

		// Token: 0x06000823 RID: 2083 RVA: 0x0001AB95 File Offset: 0x00018D95
		private void Set(HttpHeaderValueElement preference)
		{
			this.Preferences[preference.Name] = preference;
			this.SetPreferencesToMessageHeader();
		}

		// Token: 0x06000824 RID: 2084 RVA: 0x0001ABB0 File Offset: 0x00018DB0
		private HttpHeaderValueElement Get(string preferenceName)
		{
			HttpHeaderValueElement result;
			if (!this.Preferences.TryGetValue(preferenceName, out result))
			{
				return null;
			}
			return result;
		}

		// Token: 0x06000825 RID: 2085 RVA: 0x0001ABD0 File Offset: 0x00018DD0
		private HttpHeaderValue ParsePreferences()
		{
			string header = this.message.GetHeader(this.preferenceHeaderName);
			HttpHeaderValueLexer httpHeaderValueLexer = HttpHeaderValueLexer.Create(this.preferenceHeaderName, header);
			return httpHeaderValueLexer.ToHttpHeaderValue();
		}

		// Token: 0x06000826 RID: 2086 RVA: 0x0001AC02 File Offset: 0x00018E02
		private void SetPreferencesToMessageHeader()
		{
			this.message.SetHeader(this.preferenceHeaderName, this.Preferences.ToString());
		}

		// Token: 0x04000313 RID: 787
		private const string ReturnNoContentPreferenceToken = "return-no-content";

		// Token: 0x04000314 RID: 788
		private const string ReturnContentPreferenceToken = "return-content";

		// Token: 0x04000315 RID: 789
		private const string ODataAnnotationPreferenceToken = "odata.include-annotations";

		// Token: 0x04000316 RID: 790
		private const string PreferHeaderName = "Prefer";

		// Token: 0x04000317 RID: 791
		private const string PreferenceAppliedHeaderName = "Preference-Applied";

		// Token: 0x04000318 RID: 792
		private static readonly KeyValuePair<string, string>[] EmptyParameters = new KeyValuePair<string, string>[0];

		// Token: 0x04000319 RID: 793
		private static readonly HttpHeaderValueElement ReturnNoContentPreference = new HttpHeaderValueElement("return-no-content", null, ODataPreferenceHeader.EmptyParameters);

		// Token: 0x0400031A RID: 794
		private static readonly HttpHeaderValueElement ReturnContentPreference = new HttpHeaderValueElement("return-content", null, ODataPreferenceHeader.EmptyParameters);

		// Token: 0x0400031B RID: 795
		private readonly ODataMessage message;

		// Token: 0x0400031C RID: 796
		private readonly string preferenceHeaderName;

		// Token: 0x0400031D RID: 797
		private HttpHeaderValue preferences;
	}
}
