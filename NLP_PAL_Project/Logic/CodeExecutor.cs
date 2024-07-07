using System.Diagnostics;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using static Microsoft.CodeAnalysis.CSharp.SyntaxTokenParser;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Reflection;
using Microsoft.CodeAnalysis;
using ScottPlot.Colormaps;
using NLP_PAL_Project.Utils;

namespace NLP_PAL_Project.Logic
{
    internal class CodeExecutor
    {

        public CodeExecutor()
        {
           
        }
        public async Task<(double, Boolean, Boolean)> ExecutePythonCode(string sourceCode)
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
            string runOutput = "", errorOutput = "";
            var runTask = Task.Run(() => pythonProcess.StandardOutput.ReadToEnd());
            var errorTask = Task.Run(() => pythonProcess.StandardOutput.ReadToEnd());
            if (runTask.Wait(TimeSpan.FromSeconds(2)) && errorTask.Wait(TimeSpan.FromSeconds(2)))
            {
                runOutput = runTask.Result;
                errorOutput = errorTask.Result;
                pythonProcess.WaitForExit();
                if (pythonProcess.ExitCode != 0)
                {
                    return (-1, true, false);
                }
            }
            else
            {
                pythonProcess.Kill();
                Console.WriteLine("Timed out");
                return (-1, true, true);
            }
            File.Delete(codePath);
            double numberOutput = 0;
            GeneralUtils.TryCleanAndConvertToDouble(runOutput, out numberOutput);
            return (numberOutput, false, false);
        }
        public async Task<(double, Boolean, Boolean)> ExecuteJavaScriptCode(string sourceCode)
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
            string runOutput = "", errorOutput = "";
            double numberOutput = 0;
            var runTask = Task.Run(() => JSProcess.StandardOutput.ReadToEnd());
            var errorTask = Task.Run(() => JSProcess.StandardOutput.ReadToEnd());
            if (runTask.Wait(TimeSpan.FromSeconds(2)) && errorTask.Wait(TimeSpan.FromSeconds(2))) 
            {
                runOutput = runTask.Result;
                errorOutput = errorTask.Result;
                JSProcess.WaitForExit();
                if (JSProcess.ExitCode != 0)
                {
                    return (-1, true, false);
                }
            }
            else
            {
                JSProcess.Kill();
                Console.WriteLine("Timed out");
                return (-1, true, true);

            }
            File.Delete(codePath);
            GeneralUtils.TryCleanAndConvertToDouble(runOutput, out numberOutput);
            return (numberOutput, false, false);
        }
        public async Task<(double, Boolean, Boolean)> ExecuteRubyCode(string sourceCode)
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
            string runOutput = "", errorOutput = "";
            var runTask = Task.Run(() => rubyProcess.StandardOutput.ReadToEnd());
            var errorTask = Task.Run(() => rubyProcess.StandardOutput.ReadToEnd());
            if (runTask.Wait(TimeSpan.FromSeconds(2)) && errorTask.Wait(TimeSpan.FromSeconds(2)))
            {
                runOutput = runTask.Result;
                errorOutput = errorTask.Result;
                rubyProcess.WaitForExit();
                if (rubyProcess.ExitCode != 0)
                {
                    return (-1, true, false);
                }
            }
            else
            {
                rubyProcess.Kill();
                Console.WriteLine("Timed out");
                return (-1, true, true);
            }

            File.Delete(codePath);
            double numberOutput = 0;
            GeneralUtils.TryCleanAndConvertToDouble(runOutput, out numberOutput);
            return (numberOutput, false, false);
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
