using System;
using System.Collections.Generic;

namespace Microsoft.Data.Edm.Library
{
	// Token: 0x020001D1 RID: 465
	public abstract class EdmFunctionBase : EdmNamedElement, IEdmFunctionBase, IEdmNamedElement, IEdmVocabularyAnnotatable, IEdmElement
	{
		// Token: 0x06000B09 RID: 2825 RVA: 0x000204CC File Offset: 0x0001E6CC
		protected EdmFunctionBase(string name, IEdmTypeReference returnType) : base(name)
		{
			this.returnType = returnType;
		}

		// Token: 0x17000433 RID: 1075
		// (get) Token: 0x06000B0A RID: 2826 RVA: 0x000204E7 File Offset: 0x0001E6E7
		public IEdmTypeReference ReturnType
		{
			get
			{
				return this.returnType;
			}
		}

		// Token: 0x17000434 RID: 1076
		// (get) Token: 0x06000B0B RID: 2827 RVA: 0x000204EF File Offset: 0x0001E6EF
		public IEnumerable<IEdmFunctionParameter> Parameters
		{
			get
			{
				return this.parameters;
			}
		}

		// Token: 0x06000B0C RID: 2828 RVA: 0x000204F8 File Offset: 0x0001E6F8
		public IEdmFunctionParameter FindParameter(string name)
		{
			foreach (IEdmFunctionParameter edmFunctionParameter in this.parameters)
			{
				if (edmFunctionParameter.Name == name)
				{
					return edmFunctionParameter;
				}
			}
			return null;
		}

		// Token: 0x06000B0D RID: 2829 RVA: 0x0002055C File Offset: 0x0001E75C
		public EdmFunctionParameter AddParameter(string name, IEdmTypeReference type)
		{
			EdmFunctionParameter edmFunctionParameter = new EdmFunctionParameter(this, name, type);
			this.parameters.Add(edmFunctionParameter);
			return edmFunctionParameter;
		}

		// Token: 0x06000B0E RID: 2830 RVA: 0x00020580 File Offset: 0x0001E780
		public EdmFunctionParameter AddParameter(string name, IEdmTypeReference type, EdmFunctionParameterMode mode)
		{
			EdmFunctionParameter edmFunctionParameter = new EdmFunctionParameter(this, name, type, mode);
			this.parameters.Add(edmFunctionParameter);
			return edmFunctionParameter;
		}

		// Token: 0x06000B0F RID: 2831 RVA: 0x000205A4 File Offset: 0x0001E7A4
		public void AddParameter(IEdmFunctionParameter parameter)
		{
			EdmUtil.CheckArgumentNull<IEdmFunctionParameter>(parameter, "parameter");
			this.parameters.Add(parameter);
		}

		// Token: 0x04000530 RID: 1328
		private readonly List<IEdmFunctionParameter> parameters = new List<IEdmFunctionParameter>();

		// Token: 0x04000531 RID: 1329
		private IEdmTypeReference returnType;
	}
}
