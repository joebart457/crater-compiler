using CraterLang.Compiler.Shared.Constants;
using CraterLang.Compiler.Shared.Enums;
using CraterLang.Compiler.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CraterLang.Compiler._Compiler.Helpers
{
    internal static class UnaryOperatorSourceGenerator
    {
        public static string GenerateOperator(OperatorType operatorType,  CrateType rhs)
        {
            /* At this point we are assuming operator type is compatible with the operand type */
            switch (operatorType)
            {
                case OperatorType.Not:
                    return GenerateOperator_Not(rhs);
                
                default:
                    throw new Exception($"unary operator type {operatorType} is not supported");
            }
        }

        private static string GenerateOperator_Not( CrateType rhs)
        {
            return "!{0}";
        }
    }
}
