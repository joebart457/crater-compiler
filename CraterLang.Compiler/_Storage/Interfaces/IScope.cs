using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CraterLang.Compiler._Storage.Interfaces
{
    internal interface IScope<TyKey, TyResult>
    {
        public void Define(TyKey key, TyResult value);
        public TyResult Get(TyKey key);
        public void Update(TyKey key, TyResult value);
        public void Delete(TyKey key);
        public bool Exists(TyKey key);
        public IScope<TyKey, TyResult> ShallowCopy();
    }
}
