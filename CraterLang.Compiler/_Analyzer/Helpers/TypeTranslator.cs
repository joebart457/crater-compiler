using CraterLang.Compiler.Shared;
using CraterLang.Compiler.Shared.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CraterLang.Compiler._Analyzer.Helpers
{
    internal static class TypeTranslator
    {
        public static CrateType Translate(Type type)
        {
            //TODO update with FullCType
            if (type == typeof(void)) return new CrateType(CTypes.null_t);
            if (type == typeof(string)) return new CrateType(CTypes.string_t);
            if (type == typeof(char)) return new CrateType(CTypes.char_t);

            if (type == typeof(UInt16)) return new CrateType(CTypes.uint16_t);
            if (type == typeof(UInt32)) return new CrateType(CTypes.uint32_t);
            if (type == typeof(UInt64)) return new CrateType(CTypes.uint64_t);

            if (type == typeof(Int16)) return new CrateType(CTypes.int16_t);
            if (type == typeof(Int32)) return new CrateType(CTypes.int32_t);
            if (type == typeof(Int64)) return new CrateType(CTypes.int64_t);
            if (type == typeof(bool)) return new CrateType(CTypes.bool_t);

            throw new Exception($"unable to translate type {type.Name} to valid ctype");
        }
    }
}
