using Microsoft.CodeAnalysis.Scripting.Hosting;
using NLP_PAL_Project.Logic;
using NLP_PAL_Project.Models;
namespace NLP_PAL_Project
{
    class Program
    {
        public static async Task Main(string[] args)
        {
            Consts.Init();

            (int correct, int incorrect, int error, int timeout) pythonStats = (0, 0, 0, 0);
            (int correct, int incorrect, int error, int timeout) javaScriptStats = (0, 0, 0, 0);
            (int correct, int incorrect, int error, int timeout) rubyStats = (0,0, 0, 0);
            int total = 0;

            List<QuestionObj> questionObjs = await Gsm8k.ReadQuestionsFromJsonAsync($"../../../{Consts.config["GSM"]["FilePath"].ToString()}");
            await new CohereLogic().GeneratePalAnswers(questionObjs);

            // Execute code on compilers - Denis
            foreach (QuestionObj questionObj in questionObjs)
            {
                CodeExecutor codeExecutor = new CodeExecutor();
                (double answer, Boolean error, Boolean timeout) PythonOutput = await codeExecutor.ExecutePythonCode(questionObj.LanguageObjects[Language.Python].GeneratedAnswer);
                (double answer, Boolean error, Boolean timeout) RubyOutput = await codeExecutor.ExecuteRubyCode(questionObj.LanguageObjects[Language.Ruby].GeneratedAnswer);
                (double answer, Boolean error, Boolean timeout) JSOutput = await codeExecutor.ExecuteJavaScriptCode(questionObj.LanguageObjects[Language.JS].GeneratedAnswer);

                pythonStats = PythonOutput.timeout ? (pythonStats.correct, pythonStats.incorrect, pythonStats.error + 1,pythonStats.timeout)
                    : (PythonOutput.error ? (pythonStats.correct, pythonStats.incorrect, pythonStats.error + 1,pythonStats.timeout)
                    : PythonOutput.Item1 == questionObj.RealAnswer ? (pythonStats.correct + 1, pythonStats.incorrect, pythonStats.error,pythonStats.timeout)
                    : (pythonStats.correct, pythonStats.incorrect + 1, pythonStats.error,pythonStats.timeout));
                rubyStats = RubyOutput.timeout ? (rubyStats.correct, rubyStats.incorrect, rubyStats.error + 1,rubyStats.timeout)
                    : (RubyOutput.error ? (rubyStats.correct, rubyStats.incorrect, rubyStats.error + 1,rubyStats.timeout)
                    : RubyOutput.Item1 == questionObj.RealAnswer ? (rubyStats.correct + 1, rubyStats.incorrect, rubyStats.error,rubyStats.timeout)
                    : (rubyStats.correct, rubyStats.incorrect + 1, rubyStats.error,rubyStats.timeout));
                javaScriptStats = JSOutput.timeout ? (javaScriptStats.correct, javaScriptStats.incorrect, javaScriptStats.error + 1,javaScriptStats.timeout)
                    : (JSOutput.error ? (javaScriptStats.correct, javaScriptStats.incorrect, javaScriptStats.error + 1,javaScriptStats.timeout)
                    : JSOutput.Item1 == questionObj.RealAnswer ? (javaScriptStats.correct + 1, javaScriptStats.incorrect, javaScriptStats.error,javaScriptStats.timeout)
                    : (javaScriptStats.correct, javaScriptStats.incorrect + 1, javaScriptStats.error,javaScriptStats.timeout));
                total++;
            }
            Console.WriteLine($"python: {pythonStats}; js: {javaScriptStats}; ruby: {rubyStats}");
            Console.ReadLine();
            // Get answers from compilers and give scores to languages

            // Graphs, stats, etc.






        }
    }
}
