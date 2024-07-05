using System.Diagnostics;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;

namespace NLP_PAL_Project.Logic
{
    internal class CodeExecutor
    {
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
        public async Task<string> ExecuteCSharpCode(string sourceCode)
        {
            try
            {
                ScriptOptions scriptOptions = ScriptOptions.Default.WithReferences(AppDomain.CurrentDomain.GetAssemblies())
                .WithImports("System", "System.Linq", "System.Collections.Generic");
                string result = await CSharpScript.EvaluateAsync<string>(sourceCode, scriptOptions);
                return result;
            }
            catch (CompilationErrorException e)
            {
                return "Compilation error: " + e.Message;
            }
            catch (Exception e)
            {
                return "Execution error: " + e.Message;
            }
        }
    }

}
