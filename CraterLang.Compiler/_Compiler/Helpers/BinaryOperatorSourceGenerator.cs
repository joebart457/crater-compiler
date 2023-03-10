using CraterLang.Compiler.Shared.Constants;
using CraterLang.Compiler.Shared.Enums;
using CraterLang.Compiler.Shared;

namespace CraterLang.Compiler._Compiler.Helpers
{
    internal static class BinaryOperatorGenerator
    {
        public static string GenerateOperator(OperatorType operatorType, CrateType lhs, CrateType rhs)
        {
            /* At this point we are assuming operator type is compatible with the operand types */
            switch (operatorType)
            {
                case OperatorType.Addition:
                    return GenerateOperator_Addition(lhs, rhs);
                case OperatorType.Subtraction:
                    return GenerateOperator_Subtractraction(lhs, rhs);
                case OperatorType.Multiplication:
                    return GenerateOperator_Multiplication(lhs, rhs);
                case OperatorType.Division:
                    return GenerateOperator_Division(lhs, rhs);
                case OperatorType.And:
                    return GenerateOperator_And(lhs, rhs);
                case OperatorType.Or:
                    return GenerateOperator_Or(lhs, rhs);
                case OperatorType.Equal:
                    return GenerateOperator_Equal(lhs, rhs);
                case OperatorType.NotEqual:
                    return GenerateOperator_NotEqual(lhs, rhs);
                case OperatorType.LessThan:
                    return GenerateOperator_LessThan(lhs, rhs);
                case OperatorType.GreaterThan:
                    return GenerateOperator_GreaterThan(lhs, rhs);
                case OperatorType.LessThanEqual:
                    return GenerateOperator_LessThanEqual(lhs, rhs);
                case OperatorType.GreaterThanEqual:
                    return GenerateOperator_GreaterThanEqual(lhs, rhs);
                default:
                    throw new Exception($"binary operator type {operatorType} is not supported");
            }
        }

        private static string GenerateOperator_Addition(CrateType lhs, CrateType rhs)
        {
            if (lhs.CType == CTypes.string_t) return "strcat({0}, {1})"; //TODO can this be translated to fn call before we get here?
            return "{0} + {1}";
        }

        private static string GenerateOperator_Subtractraction(CrateType lhs, CrateType rhs)
        {
            return "{0} - {1}";
        }

        private static string GenerateOperator_Multiplication(CrateType lhs, CrateType rhs)
        {
            return "{0} * {1}";
        }

        private static string GenerateOperator_Division(CrateType lhs, CrateType rhs)
        {
            return "{0} / {1}";
        }

        private static string GenerateOperator_And(CrateType lhs, CrateType rhs)
        {
            return "{0} && {1}";
        }

        private static string GenerateOperator_Or(CrateType lhs, CrateType rhs)
        {
            return "{0} || {1}";
        }

        private static string GenerateOperator_Equal(CrateType lhs, CrateType rhs)
        {
            if (lhs.CType == CTypes.string_t) return "strcmp({0}, {1}) == 0";
            if (IsValueType(lhs)) return "{0} == {1}";
            var fieldComparisons = new List<string>();
            foreach (var field in lhs.Fields)
                fieldComparisons.Add(string.Format(GenerateOperator_Equal(field.CrateType, field.CrateType), $"{{0}}.{field.FieldName}", $"{{1}}.{field.FieldName}"));
            return string.Join(" && ", fieldComparisons);
        }

        private static string GenerateOperator_NotEqual(CrateType lhs, CrateType rhs)
        {
            if (lhs.CType == CTypes.string_t) return "strcmp({0}, {1}) != 0";
            if (IsValueType(lhs)) return "{0} != {1}";
            var fieldComparisons = new List<string>();
            foreach (var field in lhs.Fields)
                fieldComparisons.Append(string.Format(GenerateOperator_NotEqual(field.CrateType, field.CrateType), $"{{0}}.{field.FieldName}", $"{{1}}.{field.FieldName}"));
            return string.Join(" || ", fieldComparisons);
        }

        private static string GenerateOperator_LessThan(CrateType lhs, CrateType rhs)
        {
            return "{0} < {1}";
        }

        private static string GenerateOperator_GreaterThan(CrateType lhs, CrateType rhs)
        {
            return "{0} > {1}";
        }

        private static string GenerateOperator_LessThanEqual(CrateType lhs, CrateType rhs)
        {
            return "{0} <= {1}";
        }

        private static string GenerateOperator_GreaterThanEqual(CrateType lhs, CrateType rhs)
        {
            return "{0} >= {1}";
        }

        private static bool IsValueType(CrateType type)
        {
            if (type.CType == CTypes.int16_t) return true;
            if (type.CType == CTypes.int32_t) return true;
            if (type.CType == CTypes.int64_t) return true;
            if (type.CType == CTypes.uint16_t) return true;
            if (type.CType == CTypes.uint32_t) return true;
            if (type.CType == CTypes.uint64_t) return true;
            if (type.CType == CTypes.bool_t) return true;
            if (type.CType == CTypes.char_t) return true;
            return false;
        }
    }
}
