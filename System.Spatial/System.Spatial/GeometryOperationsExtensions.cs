using System;
using System.Linq;
using Microsoft.Data.Spatial;

namespace System.Spatial
{
	// Token: 0x02000040 RID: 64
	public static class GeometryOperationsExtensions
	{
		// Token: 0x060001A2 RID: 418 RVA: 0x00004ED4 File Offset: 0x000030D4
		public static double? Distance(this Geometry operand1, Geometry operand2)
		{
			return GeometryOperationsExtensions.OperationsFor(new Geometry[]
			{
				operand1,
				operand2
			}).IfValidReturningNullable((SpatialOperations ops) => ops.Distance(operand1, operand2));
		}

		// Token: 0x060001A3 RID: 419 RVA: 0x00004F3C File Offset: 0x0000313C
		public static double? Length(this Geometry operand)
		{
			return GeometryOperationsExtensions.OperationsFor(new Geometry[]
			{
				operand
			}).IfValidReturningNullable((SpatialOperations ops) => ops.Length(operand));
		}

		// Token: 0x060001A4 RID: 420 RVA: 0x00004F9C File Offset: 0x0000319C
		public static bool? Intersects(this Geometry operand1, Geometry operand2)
		{
			return GeometryOperationsExtensions.OperationsFor(new Geometry[]
			{
				operand1,
				operand2
			}).IfValidReturningNullable((SpatialOperations ops) => ops.Intersects(operand1, operand2));
		}

		// Token: 0x060001A5 RID: 421 RVA: 0x00004FF3 File Offset: 0x000031F3
		private static SpatialOperations OperationsFor(params Geometry[] operands)
		{
			if (operands.Any((Geometry operand) => operand == null))
			{
				return null;
			}
			return operands[0].Creator.VerifyAndGetNonNullOperations();
		}
	}
}
