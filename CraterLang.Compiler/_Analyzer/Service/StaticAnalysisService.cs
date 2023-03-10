using CraterLang.Compiler._Analyzer.Constants;
using CraterLang.Compiler._Analyzer.Exceptions;
using CraterLang.Compiler._Analyzer.Helpers;
using CraterLang.Compiler._Parser.Definitions;
using CraterLang.Compiler._Storage.Implementation;
using CraterLang.Compiler.Shared;
using CraterLang.Compiler.Shared.Constants;
using CraterLang.Compiler.Shared.Enums;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using TokenizerCore.Interfaces;

namespace CraterLang.Compiler._Analyzer.Service
{
    internal class StaticAnalysisService
    {
        private List<NextPassItem> _additionalPassItems = new();
        private class NextPassItem
        {
            public BaseDefinition Definition { get; private set; }

            public NextPassItem(BaseDefinition definition)
            {
                Definition = definition;
            }

            public virtual bool CanResolve(AnalyzerContext context) => true;
        }

        private class TypeReliantPassItem : NextPassItem
        {
            public TypeSymbol UnresolvedTypeSymbol { get; private set; }
            public TypeReliantPassItem(BaseDefinition definition, TypeSymbol unresolvedTypeSymbol) 
                : base(definition)
            {
                UnresolvedTypeSymbol = unresolvedTypeSymbol;
            }

            public override bool CanResolve(AnalyzerContext context)
            {
                return context.TypeProvider.CanResolve(UnresolvedTypeSymbol);
            }
        }

        public bool Analyze(IEnumerable<BaseDefinition> definitions, out List<CrateType> types, out List<CrateMethod> methods, out List<TypeSymbol> unresolvedTypes)
        {
            types = new List<CrateType>();
            methods = new List<CrateMethod>();
            unresolvedTypes = new List<TypeSymbol>();   
            var methodProvider = new MethodProvider();
            var typeProvider = new TypeProvider();
            ProvideNativeTypes(typeProvider);
            ProvideNativeMethods(methodProvider);
            LoadItems(definitions);

            var analyzerContext = new AnalyzerContext(methodProvider, typeProvider);
            var analyzer = new StaticAnalyzer(analyzerContext);

            var currentItem = GetNextPassItem(analyzerContext);
            while (currentItem != null)
            {
                analyzerContext.FlushEnvironment();
                analyzerContext.FlushRegisters();
                if (currentItem.Definition is TypeDefinition typeDef)
                {
                    try
                    {
                        var analyzedType = analyzer.Determine(typeDef);
                        typeProvider.ProvideType(analyzedType);
                        types.Add(analyzedType);
                    } catch(UnresolvedSymbolException e)
                    {
                        _additionalPassItems.Add(new TypeReliantPassItem(currentItem.Definition, e.Symbol));
                    }
                    
                }
                else if (currentItem.Definition is MethodDefinition methodDef)
                {
                    var analyzedMethod = analyzer.Determine(methodDef);
                    methods.Add(analyzedMethod);
                }
                else throw new Exception($"unsupported definition type {currentItem.Definition}");
                currentItem = GetNextPassItem(analyzerContext);
            }
            foreach(var passItem in _additionalPassItems)
            {
                if (passItem is TypeReliantPassItem item)
                {
                    if (!unresolvedTypes.Contains(item.UnresolvedTypeSymbol)) unresolvedTypes.Add(item.UnresolvedTypeSymbol);
                }
            }
            return !_additionalPassItems.Any();
        }

        private NextPassItem? GetNextPassItem(AnalyzerContext context)
        {
            var item = _additionalPassItems.FirstOrDefault(i => i.CanResolve(context));
            if (item != null) _additionalPassItems.Remove(item);
            return item;
        }

        private void LoadItems(IEnumerable<BaseDefinition> definitions)
        {
            _additionalPassItems.Clear();
            _additionalPassItems.AddRange(definitions.OrderByDescending(d => d is TypeDefinition).Select(def => new NextPassItem(def)));
        }

        private static void ProvideNativeTypes(TypeProvider typeProvider)
        {
            typeProvider.ProvideType(FullCTypes.bool_t);
            typeProvider.ProvideType(FullCTypes.null_t);
            typeProvider.ProvideType(FullCTypes.string_t);
            typeProvider.ProvideType(FullCTypes.char_t);
            typeProvider.ProvideType(FullCTypes.uint8_t);
            typeProvider.ProvideType(FullCTypes.uint16_t);
            typeProvider.ProvideType(FullCTypes.uint32_t);
            typeProvider.ProvideType(FullCTypes.uint64_t);
            typeProvider.ProvideType(FullCTypes.int8_t);
            typeProvider.ProvideType(FullCTypes.int16_t);
            typeProvider.ProvideType(FullCTypes.int32_t);
            typeProvider.ProvideType(FullCTypes.int64_t);
            typeProvider.ProvideType(FullCTypes.error_result_t);
        }

        private static void ProvideNativeMethods(MethodProvider methodProvider)
        {
            methodProvider.ProvideMethod(CNativeMethods.PrintLn);
        }
    }
}
