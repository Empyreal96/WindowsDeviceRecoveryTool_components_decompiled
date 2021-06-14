using System;
using System.Linq;
using Microsoft.Data.Edm;
using Microsoft.Data.Edm.Annotations;
using Microsoft.Data.Edm.Expressions;

namespace Microsoft.Data.OData.Evaluation
{
	// Token: 0x02000009 RID: 9
	internal sealed class UrlConvention
	{
		// Token: 0x06000023 RID: 35 RVA: 0x00002705 File Offset: 0x00000905
		private UrlConvention(bool generateKeyAsSegment)
		{
			this.generateKeyAsSegment = generateKeyAsSegment;
		}

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000024 RID: 36 RVA: 0x00002714 File Offset: 0x00000914
		internal bool GenerateKeyAsSegment
		{
			get
			{
				return this.generateKeyAsSegment;
			}
		}

		// Token: 0x06000025 RID: 37 RVA: 0x0000271C File Offset: 0x0000091C
		internal static UrlConvention CreateWithExplicitValue(bool generateKeyAsSegment)
		{
			return new UrlConvention(generateKeyAsSegment);
		}

		// Token: 0x06000026 RID: 38 RVA: 0x00002724 File Offset: 0x00000924
		internal static UrlConvention ForEntityContainer(IEdmModel model, IEdmEntityContainer container)
		{
			return UrlConvention.CreateWithExplicitValue(model.FindVocabularyAnnotations(container).OfType<IEdmValueAnnotation>().Any(new Func<IEdmValueAnnotation, bool>(UrlConvention.IsKeyAsSegmentUrlConventionAnnotation)));
		}

		// Token: 0x06000027 RID: 39 RVA: 0x00002748 File Offset: 0x00000948
		internal static UrlConvention ForUserSettingAndTypeContext(bool? keyAsSegment, IODataFeedAndEntryTypeContext typeContext)
		{
			if (keyAsSegment != null)
			{
				return UrlConvention.CreateWithExplicitValue(keyAsSegment.Value);
			}
			return typeContext.UrlConvention;
		}

		// Token: 0x06000028 RID: 40 RVA: 0x00002766 File Offset: 0x00000966
		private static bool IsKeyAsSegmentUrlConventionAnnotation(IEdmValueAnnotation annotation)
		{
			return annotation != null && UrlConvention.IsUrlConventionTerm(annotation.Term) && UrlConvention.IsKeyAsSegment(annotation.Value);
		}

		// Token: 0x06000029 RID: 41 RVA: 0x00002785 File Offset: 0x00000985
		private static bool IsKeyAsSegment(IEdmExpression value)
		{
			return value != null && value.ExpressionKind == EdmExpressionKind.StringConstant && ((IEdmStringConstantExpression)value).Value == "KeyAsSegment";
		}

		// Token: 0x0600002A RID: 42 RVA: 0x000027AB File Offset: 0x000009AB
		private static bool IsUrlConventionTerm(IEdmTerm term)
		{
			return term != null && term.Name == "UrlConventions" && term.Namespace == "Com.Microsoft.Data.Services.Conventions.V1";
		}

		// Token: 0x04000008 RID: 8
		private const string ConventionTermNamespace = "Com.Microsoft.Data.Services.Conventions.V1";

		// Token: 0x04000009 RID: 9
		private const string ConventionTermName = "UrlConventions";

		// Token: 0x0400000A RID: 10
		private const string KeyAsSegmentConventionName = "KeyAsSegment";

		// Token: 0x0400000B RID: 11
		private const string UrlConventionHeaderName = "DataServiceUrlConventions";

		// Token: 0x0400000C RID: 12
		private readonly bool generateKeyAsSegment;
	}
}
