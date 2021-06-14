using System;
using System.Globalization;
using System.Resources;
using System.Threading;

namespace System.Management
{
	// Token: 0x020000AA RID: 170
	internal sealed class SR
	{
		// Token: 0x0600047D RID: 1149 RVA: 0x00021E5E File Offset: 0x0002005E
		internal SR()
		{
			this.resources = new ResourceManager("System.Management", base.GetType().Assembly);
		}

		// Token: 0x0600047E RID: 1150 RVA: 0x00021E84 File Offset: 0x00020084
		private static SR GetLoader()
		{
			if (SR.loader == null)
			{
				SR value = new SR();
				Interlocked.CompareExchange<SR>(ref SR.loader, value, null);
			}
			return SR.loader;
		}

		// Token: 0x1700009F RID: 159
		// (get) Token: 0x0600047F RID: 1151 RVA: 0x00021EB0 File Offset: 0x000200B0
		private static CultureInfo Culture
		{
			get
			{
				return null;
			}
		}

		// Token: 0x170000A0 RID: 160
		// (get) Token: 0x06000480 RID: 1152 RVA: 0x00021EB3 File Offset: 0x000200B3
		public static ResourceManager Resources
		{
			get
			{
				return SR.GetLoader().resources;
			}
		}

		// Token: 0x06000481 RID: 1153 RVA: 0x00021EC0 File Offset: 0x000200C0
		public static string GetString(string name, params object[] args)
		{
			SR sr = SR.GetLoader();
			if (sr == null)
			{
				return null;
			}
			string @string = sr.resources.GetString(name, SR.Culture);
			if (args != null && args.Length != 0)
			{
				for (int i = 0; i < args.Length; i++)
				{
					string text = args[i] as string;
					if (text != null && text.Length > 1024)
					{
						args[i] = text.Substring(0, 1021) + "...";
					}
				}
				return string.Format(CultureInfo.CurrentCulture, @string, args);
			}
			return @string;
		}

		// Token: 0x06000482 RID: 1154 RVA: 0x00021F40 File Offset: 0x00020140
		public static string GetString(string name)
		{
			SR sr = SR.GetLoader();
			if (sr == null)
			{
				return null;
			}
			return sr.resources.GetString(name, SR.Culture);
		}

		// Token: 0x06000483 RID: 1155 RVA: 0x00021F69 File Offset: 0x00020169
		public static string GetString(string name, out bool usedFallback)
		{
			usedFallback = false;
			return SR.GetString(name);
		}

		// Token: 0x06000484 RID: 1156 RVA: 0x00021F74 File Offset: 0x00020174
		public static object GetObject(string name)
		{
			SR sr = SR.GetLoader();
			if (sr == null)
			{
				return null;
			}
			return sr.resources.GetObject(name, SR.Culture);
		}

		// Token: 0x04000499 RID: 1177
		internal const string ASSEMBLY_NOT_REGISTERED = "ASSEMBLY_NOT_REGISTERED";

		// Token: 0x0400049A RID: 1178
		internal const string FAILED_TO_BUILD_GENERATED_ASSEMBLY = "FAILED_TO_BUILD_GENERATED_ASSEMBLY";

		// Token: 0x0400049B RID: 1179
		internal const string COMMENT_SHOULDSERIALIZE = "COMMENT_SHOULDSERIALIZE";

		// Token: 0x0400049C RID: 1180
		internal const string COMMENT_ISPROPNULL = "COMMENT_ISPROPNULL";

		// Token: 0x0400049D RID: 1181
		internal const string COMMENT_RESETPROP = "COMMENT_RESETPROP";

		// Token: 0x0400049E RID: 1182
		internal const string COMMENT_DATECONVFUNC = "COMMENT_DATECONVFUNC";

		// Token: 0x0400049F RID: 1183
		internal const string COMMENT_TIMESPANCONVFUNC = "COMMENT_TIMESPANCONVFUNC";

		// Token: 0x040004A0 RID: 1184
		internal const string COMMENT_ATTRIBPROP = "COMMENT_ATTRIBPROP";

		// Token: 0x040004A1 RID: 1185
		internal const string COMMENT_GETINSTANCES = "COMMENT_GETINSTANCES";

		// Token: 0x040004A2 RID: 1186
		internal const string COMMENT_CLASSBEGIN = "COMMENT_CLASSBEGIN";

		// Token: 0x040004A3 RID: 1187
		internal const string COMMENT_PRIVAUTOCOMMIT = "COMMENT_PRIVAUTOCOMMIT";

		// Token: 0x040004A4 RID: 1188
		internal const string COMMENT_CONSTRUCTORS = "COMMENT_CONSTRUCTORS";

		// Token: 0x040004A5 RID: 1189
		internal const string COMMENT_ORIGNAMESPACE = "COMMENT_ORIGNAMESPACE";

		// Token: 0x040004A6 RID: 1190
		internal const string COMMENT_CLASSNAME = "COMMENT_CLASSNAME";

		// Token: 0x040004A7 RID: 1191
		internal const string COMMENT_SYSOBJECT = "COMMENT_SYSOBJECT";

		// Token: 0x040004A8 RID: 1192
		internal const string COMMENT_LATEBOUNDOBJ = "COMMENT_LATEBOUNDOBJ";

		// Token: 0x040004A9 RID: 1193
		internal const string COMMENT_MGMTSCOPE = "COMMENT_MGMTSCOPE";

		// Token: 0x040004AA RID: 1194
		internal const string COMMENT_AUTOCOMMITPROP = "COMMENT_AUTOCOMMITPROP";

		// Token: 0x040004AB RID: 1195
		internal const string COMMENT_MGMTPATH = "COMMENT_MGMTPATH";

		// Token: 0x040004AC RID: 1196
		internal const string COMMENT_PROPTYPECONVERTER = "COMMENT_PROPTYPECONVERTER";

		// Token: 0x040004AD RID: 1197
		internal const string COMMENT_SYSPROPCLASS = "COMMENT_SYSPROPCLASS";

		// Token: 0x040004AE RID: 1198
		internal const string COMMENT_ENUMIMPL = "COMMENT_ENUMIMPL";

		// Token: 0x040004AF RID: 1199
		internal const string COMMENT_LATEBOUNDPROP = "COMMENT_LATEBOUNDPROP";

		// Token: 0x040004B0 RID: 1200
		internal const string COMMENT_CREATEDCLASS = "COMMENT_CREATEDCLASS";

		// Token: 0x040004B1 RID: 1201
		internal const string COMMENT_CREATEDWMINAMESPACE = "COMMENT_CREATEDWMINAMESPACE";

		// Token: 0x040004B2 RID: 1202
		internal const string COMMENT_STATICMANAGEMENTSCOPE = "COMMENT_STATICMANAGEMENTSCOPE";

		// Token: 0x040004B3 RID: 1203
		internal const string COMMENT_STATICSCOPEPROPERTY = "COMMENT_STATICSCOPEPROPERTY";

		// Token: 0x040004B4 RID: 1204
		internal const string COMMENT_TODATETIME = "COMMENT_TODATETIME";

		// Token: 0x040004B5 RID: 1205
		internal const string COMMENT_TODMTFDATETIME = "COMMENT_TODMTFDATETIME";

		// Token: 0x040004B6 RID: 1206
		internal const string COMMENT_TODMTFTIMEINTERVAL = "COMMENT_TODMTFTIMEINTERVAL";

		// Token: 0x040004B7 RID: 1207
		internal const string COMMENT_TOTIMESPAN = "COMMENT_TOTIMESPAN";

		// Token: 0x040004B8 RID: 1208
		internal const string COMMENT_EMBEDDEDOBJ = "COMMENT_EMBEDDEDOBJ";

		// Token: 0x040004B9 RID: 1209
		internal const string COMMENT_CURRENTOBJ = "COMMENT_CURRENTOBJ";

		// Token: 0x040004BA RID: 1210
		internal const string COMMENT_FLAGFOREMBEDDED = "COMMENT_FLAGFOREMBEDDED";

		// Token: 0x040004BB RID: 1211
		internal const string EMBEDDED_COMMENT1 = "EMBEDDED_COMMENT1";

		// Token: 0x040004BC RID: 1212
		internal const string EMBEDDED_COMMENT2 = "EMBEDDED_COMMENT2";

		// Token: 0x040004BD RID: 1213
		internal const string EMBEDDED_COMMENT3 = "EMBEDDED_COMMENT3";

		// Token: 0x040004BE RID: 1214
		internal const string EMBEDDED_COMMENT4 = "EMBEDDED_COMMENT4";

		// Token: 0x040004BF RID: 1215
		internal const string EMBEDDED_COMMENT5 = "EMBEDDED_COMMENT5";

		// Token: 0x040004C0 RID: 1216
		internal const string EMBEDDED_COMMENT6 = "EMBEDDED_COMMENT6";

		// Token: 0x040004C1 RID: 1217
		internal const string EMBEDDED_COMMENT7 = "EMBEDDED_COMMENT7";

		// Token: 0x040004C2 RID: 1218
		internal const string EMBEDED_VB_CODESAMP4 = "EMBEDED_VB_CODESAMP4";

		// Token: 0x040004C3 RID: 1219
		internal const string EMBEDED_VB_CODESAMP5 = "EMBEDED_VB_CODESAMP5";

		// Token: 0x040004C4 RID: 1220
		internal const string EMBEDDED_COMMENT8 = "EMBEDDED_COMMENT8";

		// Token: 0x040004C5 RID: 1221
		internal const string EMBEDED_CS_CODESAMP4 = "EMBEDED_CS_CODESAMP4";

		// Token: 0x040004C6 RID: 1222
		internal const string EMBEDED_CS_CODESAMP5 = "EMBEDED_CS_CODESAMP5";

		// Token: 0x040004C7 RID: 1223
		internal const string CLASSNOT_FOUND_EXCEPT = "CLASSNOT_FOUND_EXCEPT";

		// Token: 0x040004C8 RID: 1224
		internal const string NULLFILEPATH_EXCEPT = "NULLFILEPATH_EXCEPT";

		// Token: 0x040004C9 RID: 1225
		internal const string EMPTY_FILEPATH_EXCEPT = "EMPTY_FILEPATH_EXCEPT";

		// Token: 0x040004CA RID: 1226
		internal const string NAMESPACE_NOTINIT_EXCEPT = "NAMESPACE_NOTINIT_EXCEPT";

		// Token: 0x040004CB RID: 1227
		internal const string CLASSNAME_NOTINIT_EXCEPT = "CLASSNAME_NOTINIT_EXCEPT";

		// Token: 0x040004CC RID: 1228
		internal const string UNABLE_TOCREATE_GEN_EXCEPT = "UNABLE_TOCREATE_GEN_EXCEPT";

		// Token: 0x040004CD RID: 1229
		internal const string FORCE_UPDATE = "FORCE_UPDATE";

		// Token: 0x040004CE RID: 1230
		internal const string FILETOWRITE_MOF = "FILETOWRITE_MOF";

		// Token: 0x040004CF RID: 1231
		internal const string WMISCHEMA_INSTALLATIONSTART = "WMISCHEMA_INSTALLATIONSTART";

		// Token: 0x040004D0 RID: 1232
		internal const string REGESTRING_ASSEMBLY = "REGESTRING_ASSEMBLY";

		// Token: 0x040004D1 RID: 1233
		internal const string WMISCHEMA_INSTALLATIONEND = "WMISCHEMA_INSTALLATIONEND";

		// Token: 0x040004D2 RID: 1234
		internal const string MOFFILE_GENERATING = "MOFFILE_GENERATING";

		// Token: 0x040004D3 RID: 1235
		internal const string UNSUPPORTEDMEMBER_EXCEPT = "UNSUPPORTEDMEMBER_EXCEPT";

		// Token: 0x040004D4 RID: 1236
		internal const string CLASSINST_EXCEPT = "CLASSINST_EXCEPT";

		// Token: 0x040004D5 RID: 1237
		internal const string MEMBERCONFLILCT_EXCEPT = "MEMBERCONFLILCT_EXCEPT";

		// Token: 0x040004D6 RID: 1238
		internal const string NAMESPACE_ENSURE = "NAMESPACE_ENSURE";

		// Token: 0x040004D7 RID: 1239
		internal const string CLASS_ENSURE = "CLASS_ENSURE";

		// Token: 0x040004D8 RID: 1240
		internal const string CLASS_ENSURECREATE = "CLASS_ENSURECREATE";

		// Token: 0x040004D9 RID: 1241
		internal const string CLASS_NOTREPLACED_EXCEPT = "CLASS_NOTREPLACED_EXCEPT";

		// Token: 0x040004DA RID: 1242
		internal const string NONCLS_COMPLIANT_EXCEPTION = "NONCLS_COMPLIANT_EXCEPTION";

		// Token: 0x040004DB RID: 1243
		internal const string INVALID_QUERY = "INVALID_QUERY";

		// Token: 0x040004DC RID: 1244
		internal const string INVALID_QUERY_DUP_TOKEN = "INVALID_QUERY_DUP_TOKEN";

		// Token: 0x040004DD RID: 1245
		internal const string INVALID_QUERY_NULL_TOKEN = "INVALID_QUERY_NULL_TOKEN";

		// Token: 0x040004DE RID: 1246
		internal const string WORKER_THREAD_WAKEUP_FAILED = "WORKER_THREAD_WAKEUP_FAILED";

		// Token: 0x040004DF RID: 1247
		private static SR loader;

		// Token: 0x040004E0 RID: 1248
		private ResourceManager resources;
	}
}
