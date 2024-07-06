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
            
            (int correct, int incorrect, int error) pythonStats = (0, 0, 0);
            (int correct, int incorrect, int error) javaScriptStats = (0, 0, 0);
            (int correct, int incorrect, int error) rubyStats = (0, 0, 0);
            int total = 0;

            List<QuestionObj> questionObjs = await Gsm8k.ReadQuestionsFromJsonAsync($"../../../{Consts.config["GSM"]["FilePath"].ToString()}");
            await new CohereLogic().GeneratePalAnswers(questionObjs);

            // Execute code on compilers - Denis
            foreach (QuestionObj questionObj in questionObjs)
            {
                CodeExecutor codeExecutor = new CodeExecutor();
                (double answer,Boolean error) PythonOutput = await codeExecutor.ExecutePythonCode(questionObj.LanguageObjects[Language.Python].GeneratedAnswer);
                (double answer, Boolean error) RubyOutput = await codeExecutor.ExecuteRubyCode(questionObj.LanguageObjects[Language.Ruby].GeneratedAnswer);
                (double answer, Boolean error) JSOutput = await codeExecutor.ExecuteJavaScriptCode(questionObj.LanguageObjects[Language.JS].GeneratedAnswer);

                pythonStats = (PythonOutput.error ? (pythonStats.correct, pythonStats.incorrect, pythonStats.error + 1)
                    : PythonOutput.Item1 == questionObj.RealAnswer ? (pythonStats.correct + 1, pythonStats.incorrect, pythonStats.error)
                    : (pythonStats.correct, pythonStats.incorrect + 1, pythonStats.error));
                rubyStats = (RubyOutput.error ? (rubyStats.correct, rubyStats.incorrect, rubyStats.error + 1)
                    : RubyOutput.Item1 == questionObj.RealAnswer ? (rubyStats.correct + 1, rubyStats.incorrect, rubyStats.error)
                    : (rubyStats.correct, rubyStats.incorrect + 1, rubyStats.error));
                javaScriptStats = (JSOutput.error ? (javaScriptStats.correct, javaScriptStats.incorrect, javaScriptStats.error + 1)
                    : JSOutput.Item1 == questionObj.RealAnswer ? (javaScriptStats.correct + 1, javaScriptStats.incorrect, javaScriptStats.error)
                    : (javaScriptStats.correct, javaScriptStats.incorrect + 1, javaScriptStats.error));
                total++;

            }
            Console.ReadLine();
            // Get answers from compilers and give scores to languages

            // Graphs, stats, etc.






        }
    }
}
