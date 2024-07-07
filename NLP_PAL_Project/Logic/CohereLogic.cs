using Newtonsoft.Json;
using NLP_PAL_Project.Models;
using NLP_PAL_Project.Utils;
using System.Threading.Tasks;

namespace NLP_PAL_Project.Logic
{
    public class CohereLogic : AILogic
    {
        public async Task<bool> ProcessCompletionRequest(QuestionLanguageObj languageObj)
        {
            ApiCompletionResponse ret = new ApiCompletionResponse();
            dynamic response = await GeneralUtils.PostRequest(Consts.BaseUrl, CohereUtils.GenerateCohereRequestBody(languageObj));
            try
            {
                languageObj.GeneratedAnswer = response["text"];
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return true;
        }

        public async Task<bool> GeneratePalAnswers(List<QuestionObj> questionObjs)
        {
            // Send prompt to GPT and get code - Dan
            foreach (QuestionObj obj in questionObjs)
            {
                Console.WriteLine($"Generating code for question #{obj.Id}");
                List<Task<bool>> taskList = new List<Task<bool>>();
                foreach (KeyValuePair<Language, QuestionLanguageObj> languageObj in obj.LanguageObjects)
                {
                    Task<bool> task = Task.Run(() => ProcessCompletionRequest(languageObj.Value));
                    taskList.Add(task);
                }
                await Task.WhenAll(taskList);
                taskList = new List<Task<bool>>();
            }
            return true;
        }
    }
}
