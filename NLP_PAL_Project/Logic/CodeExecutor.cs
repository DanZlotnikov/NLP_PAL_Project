using System.Diagnostics;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using static Microsoft.CodeAnalysis.CSharp.SyntaxTokenParser;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Reflection;
using Microsoft.CodeAnalysis;

namespace NLP_PAL_Project.Logic
{
    internal class CodeExecutor
    {
        public Dictionary<string, Tuple<int, int>> sucessRatio;

        public CodeExecutor()
        {
            sucessRatio= new Dictionary<string, Tuple<int, int>>(); //first: correct answers count, second: wrong answers count
            sucessRatio.Add("Java Script", new Tuple<int, int>(0, 0));
            sucessRatio.Add("Python", new Tuple<int, int>(0, 0));
            sucessRatio.Add("Ruby", new Tuple<int, int>(0, 0));
        }
        public string ExecutePythonCode(string sourceCode)
        {
            string codePath = Path.Combine(Path.GetTempPath(), "TempPythonScript.py");
            File.WriteAllText(codePath, sourceCode);
            Process pythonProcess = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "python",
                    Arguments = codePath,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                }
            };

            pythonProcess.Start();
            string runOutput = pythonProcess.StandardOutput.ReadToEnd();
            string runError = pythonProcess.StandardError.ReadToEnd();
            pythonProcess.WaitForExit();

            if (pythonProcess.ExitCode != 0)
            {
                // Execution failed
                Console.WriteLine(runError);
                return "Execution failed:\n" + runError;
            }
            File.Delete(codePath);
            return runOutput;
        }
        public string ExecuteJavaScriptCode(string sourceCode)
        {
            string codePath = Path.Combine(Path.GetTempPath(), "TempJavaScript.js");
            File.WriteAllText(codePath, sourceCode);
            Process JSProcess = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "node",
                    Arguments = codePath,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                }
            };

            JSProcess.Start();
            string runOutput = JSProcess.StandardOutput.ReadToEnd();
            string runError = JSProcess.StandardError.ReadToEnd();
            JSProcess.WaitForExit();

            if (JSProcess.ExitCode != 0)
            {
                // Execution failed
                Console.WriteLine(runError);
                return "Execution failed:\n" + runError;
            }
            File.Delete(codePath);
            return runOutput;
        }
        public string ExecuteRubyCode(string sourceCode)
        {
            string codePath = Path.Combine(Path.GetTempPath(), "TempRubyScript.rb");
            File.WriteAllText(codePath, sourceCode);
            Process rubyProcess = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "ruby",
                    Arguments = codePath,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                }
            };
            rubyProcess.Start();
            string runOutput = rubyProcess.StandardOutput.ReadToEnd();
            string runError = rubyProcess.StandardError.ReadToEnd();
            rubyProcess.WaitForExit();

            if (rubyProcess.ExitCode != 0)
            {
                // Execution failed
                Console.WriteLine(runError);
                return "Execution failed:\n" + runError;
            }
            File.Delete(codePath);
            return runOutput;
        }
        public async Task<string> ExecuteCSharpCodeWithRoslyn(string sourceCode)
        {
           //Doesn't work, can't redirect STDOUT so can't anlyze code answer
           //Any workaround way too complex and bug prone
            try
            {
                var assemblies = AppDomain.CurrentDomain.GetAssemblies()
                  .Where(assembly => !string.IsNullOrEmpty(assembly.Location))
                  .Select(assembly => MetadataReference.CreateFromFile(assembly.Location));

                ScriptOptions scriptOptions = ScriptOptions.Default
                    .WithReferences(assemblies)
                    .WithImports("System");

                // Execute the initial script
                 var script = CSharpScript.Create(sourceCode, scriptOptions);
                string result = await CSharpScript.EvaluateAsync<string>(sourceCode, scriptOptions);

                // Check if Main method needs to be called explicitly
                if (result == null)
                {
                    result = await CSharpScript.EvaluateAsync<string>(sourceCode + "\nProgram.Main();", scriptOptions);
                }

                return result;
            }
            catch (CompilationErrorException e)
            {
                return "Compilation error";
            }
            catch (Exception e)
            {
                return "Execution error";
            }

        }
    }

}
