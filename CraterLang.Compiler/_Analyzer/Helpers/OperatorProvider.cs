using CraterLang.Compiler.Shared;
using CraterLang.Compiler.Shared.Constants;
using CraterLang.Compiler.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CraterLang.Compiler._Analyzer.Helpers
{
    internal static class OperatorProvider
    {
        public static bool CanOperate(OperatorType operatorType, CrateType rhs, out CrateType resultType)
        {
            switch (operatorType)
            {
                case OperatorType.Not:
                    {
                        resultType = new CrateType(CTypes.bool_t);
                        return rhs.CType == CTypes.bool_t;
                    }
                default:
                    throw new Exception($"unary operator type {operatorType} is not supported");
            }
        }

        public static bool CanOperate(OperatorType operatorType, CrateType lhs, CrateType rhs, out CrateType resultType)
        {
            switch (operatorType)
            {
                case OperatorType.Addition:
                    return CanOperate_Addition(lhs, rhs, out resultType);
                case OperatorType.Subtraction:
                    return CanOperate_Subtractraction(lhs, rhs, out resultType);
                case OperatorType.Multiplication:
                    return CanOperate_Multiplication(lhs, rhs, out resultType);
                case OperatorType.Division:
                    return CanOperate_Division(lhs, rhs, out resultType);
                case OperatorType.And:
                    return CanOperate_And(lhs, rhs, out resultType);
                case OperatorType.Or:
                    return CanOperate_Or(lhs, rhs, out resultType);
                case OperatorType.Equal:
                    return CanOperate_Equal(lhs, rhs, out resultType);
                case OperatorType.NotEqual:
                    return CanOperate_NotEqual(lhs, rhs, out resultType);
                case OperatorType.LessThan:
                    return CanOperate_LessThan(lhs, rhs, out resultType);
                case OperatorType.GreaterThan:
                    return CanOperate_GreaterThan(lhs, rhs, out resultType);
                case OperatorType.LessThanEqual:
                    return CanOperate_LessThanEqual(lhs, rhs, out resultType);
                case OperatorType.GreaterThanEqual:
                    return CanOperate_GreaterThanEqual(lhs, rhs, out resultType);
                default:
                    throw new Exception($"binary operator type {operatorType} is not supported");
            }
        }

        private static bool CanOperate_Addition(CrateType lhs, CrateType rhs, out CrateType resultType)
        {
            resultType = lhs;
            if (lhs.CType == CTypes.string_t) return rhs.CType == CTypes.string_t;
            return IsNumericType(lhs) && IsNumericType(rhs);
        }

        private static bool CanOperate_Subtractraction(CrateType lhs, CrateType rhs, out CrateType resultType)
        {
            resultType = lhs;
            return IsNumericType(lhs) && IsNumericType(rhs);
        }

        private static bool CanOperate_Multiplication(CrateType lhs, CrateType rhs, out CrateType resultType)
        {
            resultType = lhs;
            return IsNumericType(lhs) && IsNumericType(rhs);
        }

        private static bool CanOperate_Division(CrateType lhs, CrateType rhs, out CrateType resultType)
        {
            resultType = lhs;
            return IsNumericType(lhs) && IsNumericType(rhs);
        }

        private static bool CanOperate_And(CrateType lhs, CrateType rhs, out CrateType resultType)
        {
            resultType = new CrateType(CTypes.bool_t);
            return lhs.CType == CTypes.bool_t && rhs.CType == CTypes.bool_t;
        }

        private static bool CanOperate_Or(CrateType lhs, CrateType rhs, out CrateType resultType)
        {
            resultType = new CrateType(CTypes.bool_t);
            return lhs.CType == CTypes.bool_t && rhs.CType == CTypes.bool_t;
        }

        private static bool CanOperate_Equal(CrateType lhs, CrateType rhs, out CrateType resultType)
        {
            resultType = new CrateType(CTypes.bool_t);
            return IsEquatableTo(lhs, rhs);
        }

        private static bool CanOperate_NotEqual(CrateType lhs, CrateType rhs, out CrateType resultType)
        {
            resultType = new CrateType(CTypes.bool_t);
            return IsEquatableTo(lhs, rhs);
        }

        private static bool CanOperate_LessThan(CrateType lhs, CrateType rhs, out CrateType resultType)
        {
            resultType = new CrateType(CTypes.bool_t);
            return IsNumericType(lhs) && IsNumericType(rhs);
        }

        private static bool CanOperate_GreaterThan(CrateType lhs, CrateType rhs, out CrateType resultType)
        {
            resultType = new CrateType(CTypes.bool_t);
            return IsNumericType(lhs) && IsNumericType(rhs);
        }

        private static bool CanOperate_LessThanEqual(CrateType lhs, CrateType rhs, out CrateType resultType)
        {
            resultType = new CrateType(CTypes.bool_t);
            return IsNumericType(lhs) && IsNumericType(rhs);
        }

        private static bool CanOperate_GreaterThanEqual(CrateType lhs, CrateType rhs, out CrateType resultType)
        {
            resultType = new CrateType(CTypes.bool_t);
            return IsNumericType(lhs) && IsNumericType(rhs);
        }

        private static bool IsNumericType(CrateType type)
        {
            return _numericTypes.Contains(type.CType);
        }

        private static bool IsEquatableTo(CrateType lhs, CrateType rhs)
        {
            if (lhs == rhs) return true;
            foreach(var field in lhs.Fields)
            {
                if (!rhs.Fields.Contains(field)) return false;
            }
            return true;
        }

        private static readonly List<string> _numericTypes = new List<string>()
        {
            CTypes.int16_t,
            CTypes.int32_t,
            CTypes.int64_t,
            CTypes.uint16_t,
            CTypes.uint32_t,
            CTypes.uint64_t,
        };
    }
}
