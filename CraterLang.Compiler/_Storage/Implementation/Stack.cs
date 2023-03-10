using CraterLang.Compiler._Storage.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CraterLang.Compiler._Storage.Implementation
{
    internal class Stack<TyKey, TyValue> : Scope<TyKey, TyValue>, IStack<TyKey, TyValue> where TyKey : notnull
    {
        public IStack<TyKey, TyValue>? Enclosing { get; set; }

        public Stack(IEqualityComparer<TyKey>? comparer, IStack<TyKey, TyValue>? enclosing = null)
            : base(comparer)
        {
            Enclosing = enclosing;
        }

        public new void Define(TyKey key, TyValue value)
        {
            base.Define(key, value);
        }

        public new void Delete(TyKey key)
        {
            base.Delete(key);
        }

        public new bool Exists(TyKey key)
        {
            if (base.Exists(key)) return true;
            return Enclosing?.Exists(key) == true;
        }

        public new TyValue Get(TyKey key)
        {
            if (base.Exists(key) || Enclosing == null) return base.Get(key);
            return Enclosing.Get(key);
        }

        public new void Update(TyKey key, TyValue value)
        {
            if (base.Exists(key) || Enclosing == null) base.Update(key, value);
            else Enclosing.Update(key, value);
        }

        public new IStack<TyKey, TyValue> ShallowCopy()
        {
            var copy = new Stack<TyKey, TyValue>(_comparer, Enclosing?.ShallowCopy());
            foreach ((var key, var value) in _lookup)
            {
                copy.Define(key, value);
            }
            return copy;
        }
    }
}
