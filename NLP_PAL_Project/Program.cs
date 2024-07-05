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
            // Read all prompt data - Bar
            //List<QuestionObj> questionObjs = await Gsm8k.ReadQuestionsFromJsonAsync("test.jsonl");
            List<QuestionObj> questionObjs = new List<QuestionObj>()
            {
                new QuestionObj(1, "Jean has 30 lollipops. Jean eats 2 of the lollipops. With the remaining lollipops, Jean wants to package 2 lollipops in one bag. How many bags can Jean fill?", "14")
            };
            await new CohereLogic().GeneratePalAnswers(questionObjs);

            // Execute code on compilers - Denis
            foreach (QuestionObj questionObj in questionObjs)
            {
                CodeExecutor codeExecutor= new CodeExecutor();
                string PythonOutput = codeExecutor.ExecutePythonCode("print(1)");
                string RubyOutput = codeExecutor.ExecuteRubyCode(questionObj.LanguageObjects[Language.Ruby].GeneratedAnswer);
                string JSOutput = codeExecutor.ExecuteJavaScriptCode(questionObj.LanguageObjects[Language.JS].GeneratedAnswer);
                Console.WriteLine($"python: {PythonOutput} \nruby: {RubyOutput}, \njs: {JSOutput}");
            }
            Console.ReadLine();
            // Get answers from compilers and give scores to languages

            // Graphs, stats, etc.






        }
    }
}
