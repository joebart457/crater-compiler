using CraterLang.Compiler.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CraterLang.Compiler._Compiler.Models
{
    internal class CompiledSource
    {
        private readonly HashSet<string> _headers = new HashSet<string>() { "\"crater_runtime.h\""};
        private List<CompiledType> _types = new List<CompiledType>();
        private List<CompiledMethod> _methods = new List<CompiledMethod>();
        public void ProvideHeader(string header)
        {
            _headers.Add(header);
        }

        public void ProvideType(CrateType type, string source)
        {
            _types.Add(new CompiledType(type, source));
        }

        public void ProvideMethod(CrateMethod method, string source)
        {
            _methods.Add(new CompiledMethod(method, source));
        }

        public void Output(Stream stream)
        {
            foreach (var header in _headers)
                Write(stream, $"#include {header}\n");
            foreach (var type in _types)
                Write(stream, type.GeneratedSource);
            foreach (var method in _methods)
                Write(stream, method.GeneratedSource);
        }

        private static void Write(Stream stream, string data)
        {
            if (!stream.CanWrite) throw new Exception("unable to write to output stream");

            byte[] info = new UTF8Encoding(true).GetBytes(data);
            stream.Write(info, 0, info.Length);
        }
    }
}
