using CliParser;
using CraterLang.Compiler._Analyzer.Service;
using CraterLang.Compiler._Compiler;
using CraterLang.Compiler._Parser;
using Logger;

namespace CraterLang.Compiler._Startup
{
    [Entry("CraterLang.Compile.exe")]
    internal class StartupService
    {
        [Command("compile")]
        public int RunFile(string path, string? outputPath = null)
        {
            var parser = new Parser();
            var staticAnalysisService = new StaticAnalysisService();
            var compiler = new StaticCompiler();
            try
            {
                var definitions = parser.ParseFile(path);
                if (!staticAnalysisService.Analyze(definitions, out var types, out var methods, out var unresolvedTypes))
                {
                    throw new Exception($"one or more unresolved symbols {string.Join(", ", unresolvedTypes.Select(t => t.Token.ToString()))}");
                }
                var compiledSource = compiler.Compile(types, methods);
                if (string.IsNullOrWhiteSpace(outputPath))
                {
                    compiledSource.Output(Console.OpenStandardOutput());
                }
                else
                {
                    using (var fs = File.Create(outputPath))
                    {
                        compiledSource.Output(fs);
                    }
                }

            }catch(Exception ex)
            {
                CliLogger.LogError(ex.Message);
                return -1;
            }

            return 0;
        }
    }
}
