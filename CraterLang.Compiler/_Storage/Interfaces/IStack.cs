using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CraterLang.Compiler._Storage.Interfaces
{
    internal interface IStack<TyKey, TyValue> : IScope<TyKey, TyValue>
    {
        public IStack<TyKey, TyValue>? Enclosing { get; set; }
        public new IStack<TyKey, TyValue> ShallowCopy();
    }
}
