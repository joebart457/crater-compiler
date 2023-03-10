using CraterLang.Compiler._Storage.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CraterLang.Compiler._Storage.Implementation
{
    internal class Scope<TyKey, TyResult> : IScope<TyKey, TyResult> where TyKey : notnull
    {
        protected Dictionary<TyKey, TyResult> _lookup;
        protected readonly IEqualityComparer<TyKey>? _comparer;
        public Scope()
        {
            _lookup = new Dictionary<TyKey, TyResult>();
        }

        public Scope(IEqualityComparer<TyKey>? comparer)
        {
            _comparer = comparer;
            _lookup = new Dictionary<TyKey, TyResult>(comparer);
        }

        public void Define(TyKey key, TyResult value)
        {
            if (_lookup.ContainsKey(key))
            {
                throw new AmbiguousMatchException($"identifier {key} already exists");
            }
            _lookup[key] = value;
        }

        public TyResult Get(TyKey key)
        {
            if (_lookup.TryGetValue(key, out var value) && value != null)
            {
                return value;
            }
            throw new KeyNotFoundException($"{key} is not defined in current scope");
        }

        public void Update(TyKey key, TyResult value)
        {
            if (!_lookup.ContainsKey(key))
            {
                throw new KeyNotFoundException($"{key} is not defined in current scope");
            }
            _lookup[key] = value;
        }

        public void Delete(TyKey key)
        {
            _lookup.Remove(key);
        }
        public bool Exists(TyKey key)
        {
            return _lookup.ContainsKey(key);
        }

        public IScope<TyKey, TyResult> ShallowCopy()
        {
            var copy = new Scope<TyKey, TyResult>(_comparer);
            foreach ((var key, var value) in _lookup)
            {
                copy.Define(key, value);
            }
            return copy;
        }

    }
}
