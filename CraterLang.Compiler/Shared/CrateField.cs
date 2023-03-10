using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CraterLang.Compiler.Shared
{
    internal class CrateField
    {
        public CrateType CrateType { get; private set; }
        public string FieldName { get; private set; }
        public CrateField(CrateType crateType, string fieldName)
        {
            CrateType = crateType;
            FieldName = fieldName;
        }

        public override bool Equals(object? obj)
        {
            if (obj is CrateField field)
            {
                return FieldName == field.FieldName && CrateType == field.CrateType;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(FieldName, CrateType.GetHashCode());
        }
    }
}
