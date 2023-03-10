using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CraterLang.Compiler.Shared
{
    internal class CrateType
    {
        public string CType { get; private set; }
        public HashSet<CrateField> Fields { get; private set; } = new();
        public CrateType(string cType)
        {
            CType = cType;
        }

        public CrateField GetField(string name)
        {
            var field = Fields.FirstOrDefault(f => f.FieldName == name);
            if (field == null) throw new Exception($"type {this} does not contain definition for field {name}");
            return field;
        }


        public override bool Equals(object? obj)
        {
            if (obj is CrateType type) 
            {
                return CType == type.CType && Fields.SetEquals(type.Fields);
            }
            return false;
        }

        public override int GetHashCode()
        {
            return CType.GetHashCode();
        }

        public override string ToString()
        {
            return CType;
        }

        public static bool operator==(CrateType a, CrateType b)
        {
            return a.Equals((object)b);
        }

        public static bool operator !=(CrateType a, CrateType b)
        {
            return !a.Equals((object)b);
        }
    }
}
