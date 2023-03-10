using CraterLang.Compiler._Storage.Implementation;
using CraterLang.Compiler._Storage.Interfaces;
using CraterLang.Compiler.Shared;
using CraterLang.Compiler.Shared.Enums;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CraterLang.Compiler._Compiler
{
    internal class CompilerContext
    {
        private uint _error_result_index = 0;
        private uint _result_index = 0;
        private uint _operator_result_index = 0;
        public string EnclosingResultIdentifier { get; private set; } = "default_result";
        public string EnclosingErrorResultIdentifier { get; private set; } = "default_error_result";
        private IScope<RegisterType, string> _registers = new Scope<RegisterType, string>();

        public (string resultIdentifier, string errorIdentifier) GenerateEnclosingMethodIdentifiers(CrateMethod method)
        {
            var resultId = GenerateEnclosingResultIdentifier(method);
            var errId = GenerateEnclosingErrorResultIdentifier(method);
            return (resultId, errId);
        }

        public string GenerateEnclosingErrorResultIdentifier(CrateMethod method)
        {
            var newId = $"_error_result_{method.MethodName}_{_error_result_index}";
            _error_result_index++;
            UpdateRegister(RegisterType.Err, newId);
            return newId;
        }

        public string GenerateEnclosingResultIdentifier(CrateMethod method)
        {
            var newId = $"_result_{method.MethodName}_{_result_index}";
            _result_index++;
            EnclosingResultIdentifier = newId;
            return newId;
        }
        public string GenerateErrorResultIdentifier(CrateMethod method)
        {
            var newId = $"_error_result_{method.MethodName}_{_error_result_index}";
            _error_result_index++;
            EnclosingErrorResultIdentifier = newId;
            return newId;
        }

        public string GenerateResultIdentifier(CrateType resultType)
        {
            var newId = $"_result_{resultType.CType}_{_result_index}";
            _result_index++;
            UpdateRegister(RegisterType.Result, newId);
            return newId;
        }

        public string GenerateResultIdentifier(CrateMethod method)
        {
            var newId = $"_result_{method.MethodName}_{_result_index}";
            _result_index++;
            UpdateRegister(RegisterType.Result, newId);
            return newId;
        }

        public (string resultIdentifier, string errorIdentifier) GenerateCallIdentifiers(CrateMethod method)
        {
            var resultId = GenerateResultIdentifier(method);
            var errId = GenerateErrorResultIdentifier(method);
            return (resultId, errId);
        }

        public string GetRegister(RegisterType register)
        {
            if (!_registers.Exists(register)) throw new Exception($"register {register} is not defined in the current context");
            return _registers.Get(register);
        }

        public void UpdateRegister(RegisterType register, string value)
        {
            if (_registers.Exists(register)) _registers.Update(register, value);
            else _registers.Define(register, value);
        }

        public string GenerateOperatorResultIdentifier(OperatorType operatorType)
        {
            var newIdentifier = $"_{operatorType}_result_{_operator_result_index}";
            _operator_result_index++;
            UpdateRegister(RegisterType.OperatorResult, newIdentifier);
            return newIdentifier;
        }

        public void FlushRegisters()
        {
            _registers = new Scope<RegisterType, string>();
            _error_result_index = 0;
            _result_index = 0;
            _operator_result_index = 0;
        }
    }
}
