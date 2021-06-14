using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Data.Edm.Annotations;
using Microsoft.Data.Edm.Csdl.Internal.Parsing.Ast;
using Microsoft.Data.Edm.Internal;
using Microsoft.Data.Edm.Library;
using Microsoft.Data.Edm.Library.Internal;
using Microsoft.Data.Edm.Validation;

namespace Microsoft.Data.Edm.Csdl.Internal.CsdlSemantics
{
	// Token: 0x0200009D RID: 157
	internal abstract class CsdlSemanticsVocabularyAnnotation : CsdlSemanticsElement, IEdmVocabularyAnnotation, IEdmElement, IEdmCheckable
	{
		// Token: 0x06000289 RID: 649 RVA: 0x00006384 File Offset: 0x00004584
		protected CsdlSemanticsVocabularyAnnotation(CsdlSemanticsSchema schema, IEdmVocabularyAnnotatable targetContext, CsdlSemanticsAnnotations annotationsContext, CsdlVocabularyAnnotationBase annotation, string qualifier) : base(annotation)
		{
			this.schema = schema;
			this.Annotation = annotation;
			this.qualifier = (qualifier ?? annotation.Qualifier);
			this.targetContext = targetContext;
			this.annotationsContext = annotationsContext;
		}

		// Token: 0x17000161 RID: 353
		// (get) Token: 0x0600028A RID: 650 RVA: 0x000063DF File Offset: 0x000045DF
		public CsdlSemanticsSchema Schema
		{
			get
			{
				return this.schema;
			}
		}

		// Token: 0x17000162 RID: 354
		// (get) Token: 0x0600028B RID: 651 RVA: 0x000063E7 File Offset: 0x000045E7
		public override CsdlElement Element
		{
			get
			{
				return this.Annotation;
			}
		}

		// Token: 0x17000163 RID: 355
		// (get) Token: 0x0600028C RID: 652 RVA: 0x000063EF File Offset: 0x000045EF
		public string Qualifier
		{
			get
			{
				return this.qualifier;
			}
		}

		// Token: 0x17000164 RID: 356
		// (get) Token: 0x0600028D RID: 653 RVA: 0x000063F7 File Offset: 0x000045F7
		public override CsdlSemanticsModel Model
		{
			get
			{
				return this.schema.Model;
			}
		}

		// Token: 0x17000165 RID: 357
		// (get) Token: 0x0600028E RID: 654 RVA: 0x00006404 File Offset: 0x00004604
		public IEdmTerm Term
		{
			get
			{
				return this.termCache.GetValue(this, CsdlSemanticsVocabularyAnnotation.ComputeTermFunc, null);
			}
		}

		// Token: 0x17000166 RID: 358
		// (get) Token: 0x0600028F RID: 655 RVA: 0x00006418 File Offset: 0x00004618
		public IEdmVocabularyAnnotatable Target
		{
			get
			{
				return this.targetCache.GetValue(this, CsdlSemanticsVocabularyAnnotation.ComputeTargetFunc, null);
			}
		}

		// Token: 0x17000167 RID: 359
		// (get) Token: 0x06000290 RID: 656 RVA: 0x0000642C File Offset: 0x0000462C
		public IEnumerable<EdmError> Errors
		{
			get
			{
				if (this.Term is IUnresolvedElement)
				{
					return this.Term.Errors();
				}
				if (this.Target is IUnresolvedElement)
				{
					return this.Target.Errors();
				}
				return Enumerable.Empty<EdmError>();
			}
		}

		// Token: 0x17000168 RID: 360
		// (get) Token: 0x06000291 RID: 657 RVA: 0x00006468 File Offset: 0x00004668
		public IEdmEntityType TargetBindingContext
		{
			get
			{
				IEdmVocabularyAnnotatable target = this.Target;
				IEdmEntityType edmEntityType = target as IEdmEntityType;
				if (edmEntityType == null)
				{
					IEdmEntitySet edmEntitySet = target as IEdmEntitySet;
					if (edmEntitySet != null)
					{
						edmEntityType = edmEntitySet.ElementType;
					}
				}
				return edmEntityType;
			}
		}

		// Token: 0x06000292 RID: 658
		protected abstract IEdmTerm ComputeTerm();

		// Token: 0x06000293 RID: 659 RVA: 0x00006498 File Offset: 0x00004698
		private IEdmVocabularyAnnotatable ComputeTarget()
		{
			if (this.targetContext != null)
			{
				return this.targetContext;
			}
			string target = this.annotationsContext.Annotations.Target;
			string[] array = target.Split(new char[]
			{
				'/'
			});
			int num = array.Count<string>();
			if (num == 1)
			{
				string text = array[0];
				IEdmSchemaType edmSchemaType = this.schema.FindType(text);
				if (edmSchemaType != null)
				{
					return edmSchemaType;
				}
				IEdmValueTerm edmValueTerm = this.schema.FindValueTerm(text);
				if (edmValueTerm != null)
				{
					return edmValueTerm;
				}
				IEdmFunction edmFunction = this.FindParameterizedFunction<IEdmFunction>(text, new Func<string, IEnumerable<IEdmFunction>>(this.Schema.FindFunctions), new Func<IEnumerable<IEdmFunction>, IEdmFunction>(this.CreateAmbiguousFunction));
				if (edmFunction != null)
				{
					return edmFunction;
				}
				IEdmEntityContainer edmEntityContainer = this.schema.FindEntityContainer(text);
				if (edmEntityContainer != null)
				{
					return edmEntityContainer;
				}
				return new UnresolvedType(this.Schema.UnresolvedName(array[0]), base.Location);
			}
			else if (num == 2)
			{
				IEdmEntityContainer edmEntityContainer = this.schema.FindEntityContainer(array[0]);
				if (edmEntityContainer != null)
				{
					IEdmEntityContainerElement edmEntityContainerElement = edmEntityContainer.FindEntitySet(array[1]);
					if (edmEntityContainerElement != null)
					{
						return edmEntityContainerElement;
					}
					IEdmFunctionImport edmFunctionImport = this.FindParameterizedFunction<IEdmFunctionImport>(array[1], new Func<string, IEnumerable<IEdmFunctionImport>>(edmEntityContainer.FindFunctionImports), new Func<IEnumerable<IEdmFunctionImport>, IEdmFunctionImport>(this.CreateAmbiguousFunctionImport));
					if (edmFunctionImport != null)
					{
						return edmFunctionImport;
					}
					return new UnresolvedEntitySet(array[1], edmEntityContainer, base.Location);
				}
				else
				{
					IEdmStructuredType edmStructuredType = this.schema.FindType(array[0]) as IEdmStructuredType;
					if (edmStructuredType != null)
					{
						IEdmProperty edmProperty = edmStructuredType.FindProperty(array[1]);
						if (edmProperty != null)
						{
							return edmProperty;
						}
						return new UnresolvedProperty(edmStructuredType, array[1], base.Location);
					}
					else
					{
						IEdmFunction edmFunction2 = this.FindParameterizedFunction<IEdmFunction>(array[0], new Func<string, IEnumerable<IEdmFunction>>(this.Schema.FindFunctions), new Func<IEnumerable<IEdmFunction>, IEdmFunction>(this.CreateAmbiguousFunction));
						if (edmFunction2 == null)
						{
							return new UnresolvedProperty(new UnresolvedEntityType(this.Schema.UnresolvedName(array[0]), base.Location), array[1], base.Location);
						}
						IEdmFunctionParameter edmFunctionParameter = edmFunction2.FindParameter(array[1]);
						if (edmFunctionParameter != null)
						{
							return edmFunctionParameter;
						}
						return new UnresolvedParameter(edmFunction2, array[1], base.Location);
					}
				}
			}
			else
			{
				if (num == 3)
				{
					string text2 = array[0];
					string text3 = array[1];
					string name = array[2];
					IEdmEntityContainer edmEntityContainer = this.Model.FindEntityContainer(text2);
					if (edmEntityContainer != null)
					{
						IEdmFunctionImport edmFunctionImport2 = this.FindParameterizedFunction<IEdmFunctionImport>(text3, new Func<string, IEnumerable<IEdmFunctionImport>>(edmEntityContainer.FindFunctionImports), new Func<IEnumerable<IEdmFunctionImport>, IEdmFunctionImport>(this.CreateAmbiguousFunctionImport));
						if (edmFunctionImport2 != null)
						{
							IEdmFunctionParameter edmFunctionParameter2 = edmFunctionImport2.FindParameter(name);
							if (edmFunctionParameter2 != null)
							{
								return edmFunctionParameter2;
							}
							return new UnresolvedParameter(edmFunctionImport2, name, base.Location);
						}
					}
					string text4 = text2 + "/" + text3;
					UnresolvedFunction declaringFunction = new UnresolvedFunction(text4, Strings.Bad_UnresolvedFunction(text4), base.Location);
					return new UnresolvedParameter(declaringFunction, name, base.Location);
				}
				return new BadElement(new EdmError[]
				{
					new EdmError(base.Location, EdmErrorCode.ImpossibleAnnotationsTarget, Strings.CsdlSemantics_ImpossibleAnnotationsTarget(target))
				});
			}
		}

		// Token: 0x06000294 RID: 660 RVA: 0x00006760 File Offset: 0x00004960
		private T FindParameterizedFunction<T>(string parameterizedName, Func<string, IEnumerable<T>> findFunctions, Func<IEnumerable<T>, T> ambiguityCreator) where T : class, IEdmFunctionBase
		{
			int num = parameterizedName.IndexOf('(');
			int num2 = parameterizedName.LastIndexOf(')');
			if (num < 0)
			{
				return default(T);
			}
			string arg = parameterizedName.Substring(0, num);
			string[] parameters = parameterizedName.Substring(num + 1, num2 - (num + 1)).Split(new string[]
			{
				", "
			}, StringSplitOptions.RemoveEmptyEntries);
			IEnumerable<T> enumerable = this.FindParameterizedFunctionFromList(findFunctions(arg).Cast<IEdmFunctionBase>(), parameters).Cast<T>();
			if (enumerable.Count<T>() == 0)
			{
				return default(T);
			}
			if (enumerable.Count<T>() == 1)
			{
				return enumerable.First<T>();
			}
			return ambiguityCreator(enumerable);
		}

		// Token: 0x06000295 RID: 661 RVA: 0x0000680C File Offset: 0x00004A0C
		private IEdmFunctionImport CreateAmbiguousFunctionImport(IEnumerable<IEdmFunctionImport> functions)
		{
			IEnumerator<IEdmFunctionImport> enumerator = functions.GetEnumerator();
			enumerator.MoveNext();
			IEdmFunctionImport first = enumerator.Current;
			enumerator.MoveNext();
			IEdmFunctionImport second = enumerator.Current;
			AmbiguousFunctionImportBinding ambiguousFunctionImportBinding = new AmbiguousFunctionImportBinding(first, second);
			while (enumerator.MoveNext())
			{
				IEdmFunctionImport binding = enumerator.Current;
				ambiguousFunctionImportBinding.AddBinding(binding);
			}
			return ambiguousFunctionImportBinding;
		}

		// Token: 0x06000296 RID: 662 RVA: 0x0000685C File Offset: 0x00004A5C
		private IEdmFunction CreateAmbiguousFunction(IEnumerable<IEdmFunction> functions)
		{
			IEnumerator<IEdmFunction> enumerator = functions.GetEnumerator();
			enumerator.MoveNext();
			IEdmFunction first = enumerator.Current;
			enumerator.MoveNext();
			IEdmFunction second = enumerator.Current;
			AmbiguousFunctionBinding ambiguousFunctionBinding = new AmbiguousFunctionBinding(first, second);
			while (enumerator.MoveNext())
			{
				IEdmFunction binding = enumerator.Current;
				ambiguousFunctionBinding.AddBinding(binding);
			}
			return ambiguousFunctionBinding;
		}

		// Token: 0x06000297 RID: 663 RVA: 0x000068AC File Offset: 0x00004AAC
		private IEnumerable<IEdmFunctionBase> FindParameterizedFunctionFromList(IEnumerable<IEdmFunctionBase> functions, string[] parameters)
		{
			List<IEdmFunctionBase> list = new List<IEdmFunctionBase>();
			foreach (IEdmFunctionBase edmFunctionBase in functions)
			{
				if (edmFunctionBase.Parameters.Count<IEdmFunctionParameter>() == parameters.Count<string>())
				{
					bool flag = true;
					IEnumerator<string> enumerator2 = ((IEnumerable<string>)parameters).GetEnumerator();
					foreach (IEdmFunctionParameter edmFunctionParameter in edmFunctionBase.Parameters)
					{
						enumerator2.MoveNext();
						string[] array = enumerator2.Current.Split(new char[]
						{
							'(',
							')'
						});
						string a;
						if ((a = array[0]) == null)
						{
							goto IL_128;
						}
						if (!(a == "Collection"))
						{
							if (!(a == "Ref"))
							{
								goto IL_128;
							}
							flag = (edmFunctionParameter.Type.IsEntityReference() && this.Schema.FindType(array[1]).IsEquivalentTo(edmFunctionParameter.Type.AsEntityReference().EntityType()));
						}
						else
						{
							flag = (edmFunctionParameter.Type.IsCollection() && this.Schema.FindType(array[1]).IsEquivalentTo(edmFunctionParameter.Type.AsCollection().ElementType().Definition));
						}
						IL_171:
						if (flag)
						{
							continue;
						}
						break;
						IL_128:
						flag = (EdmCoreModel.Instance.FindDeclaredType(enumerator2.Current).IsEquivalentTo(edmFunctionParameter.Type.Definition) || this.Schema.FindType(enumerator2.Current).IsEquivalentTo(edmFunctionParameter.Type.Definition));
						goto IL_171;
					}
					if (flag)
					{
						list.Add(edmFunctionBase);
					}
				}
			}
			return list;
		}

		// Token: 0x04000124 RID: 292
		protected readonly CsdlVocabularyAnnotationBase Annotation;

		// Token: 0x04000125 RID: 293
		private readonly CsdlSemanticsSchema schema;

		// Token: 0x04000126 RID: 294
		private readonly string qualifier;

		// Token: 0x04000127 RID: 295
		private readonly IEdmVocabularyAnnotatable targetContext;

		// Token: 0x04000128 RID: 296
		private readonly CsdlSemanticsAnnotations annotationsContext;

		// Token: 0x04000129 RID: 297
		private readonly Cache<CsdlSemanticsVocabularyAnnotation, IEdmTerm> termCache = new Cache<CsdlSemanticsVocabularyAnnotation, IEdmTerm>();

		// Token: 0x0400012A RID: 298
		private static readonly Func<CsdlSemanticsVocabularyAnnotation, IEdmTerm> ComputeTermFunc = (CsdlSemanticsVocabularyAnnotation me) => me.ComputeTerm();

		// Token: 0x0400012B RID: 299
		private readonly Cache<CsdlSemanticsVocabularyAnnotation, IEdmVocabularyAnnotatable> targetCache = new Cache<CsdlSemanticsVocabularyAnnotation, IEdmVocabularyAnnotatable>();

		// Token: 0x0400012C RID: 300
		private static readonly Func<CsdlSemanticsVocabularyAnnotation, IEdmVocabularyAnnotatable> ComputeTargetFunc = (CsdlSemanticsVocabularyAnnotation me) => me.ComputeTarget();
	}
}
