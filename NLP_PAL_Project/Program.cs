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
            List<QuestionObj> questionObjs = await NLP_PAL_Project.Gsm8k.ReadQuestionsFromJsonAsync("test.jsonl");
         

            AILogic AI = new CohereLogic();
            dynamic[] codeSnippets = await AI.GeneratePalAnswers(questionObjs);

            // Execute code on compilers - Denis
            CodeExecutor codeExecutor= new CodeExecutor();
            string pythonCode = "print(\"this is python code\")";
            string JSCode = "console.log('this is java script code');";
            string RubyCode = "print \"This is ruby code\"";
            string pythonOutput = codeExecutor.ExecutePythonCode(pythonCode);
            string JSOutput = codeExecutor.ExecuteJavaScriptCode(JSCode);
            string RubyOutput = codeExecutor.ExecuteRubyCode(RubyCode);
            Console.WriteLine(JSOutput);
            Console.WriteLine(pythonOutput);
            Console.WriteLine(RubyOutput);
            // Get answers from compilers and give scores to languages

            // Graphs, stats, etc.






        }
    }
}
