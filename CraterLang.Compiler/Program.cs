

using CliParser;
using CraterLang.Compiler._Startup;

#if DEBUG
args = new string[] { "compile", "C:\\Code\\CraterLang\\Demo\\test.txt", "C:\\Code\\CraterLang\\Demo\\test.c" };
#endif

args.Resolve(new StartupService());