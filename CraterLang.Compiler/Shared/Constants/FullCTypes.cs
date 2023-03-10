using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CraterLang.Compiler.Shared.Constants
{
    internal static class FullCTypes
    {
        public static CrateType null_t => new CrateType(CTypes.null_t);
        public static CrateType string_t => new CrateType(CTypes.string_t);
        public static CrateType char_t => new CrateType(CTypes.char_t);
        public static CrateType uint8_t => new CrateType(CTypes.uint8_t);
        public static CrateType uint16_t => new CrateType(CTypes.uint16_t);
        public static CrateType uint32_t => new CrateType(CTypes.uint32_t);
        public static CrateType uint64_t => new CrateType(CTypes.uint64_t);
        public static CrateType int8_t => new CrateType(CTypes.int8_t);
        public static CrateType int16_t => new CrateType(CTypes.int16_t);
        public static CrateType int32_t => new CrateType(CTypes.int32_t);
        public static CrateType int64_t => new CrateType(CTypes.int64_t);
        public static CrateType bool_t => new CrateType(CTypes.bool_t);
        public static CrateType error_result_t => CreateErrorResultType();

        private static CrateType CreateErrorResultType()
        {
            var type = new CrateType(CTypes.error_result);
            type.Fields.Add(new CrateField(FullCTypes.bool_t, "had_error"));
            type.Fields.Add(new CrateField(FullCTypes.string_t, "error_message"));
            return type;
        }
            
    }
}
