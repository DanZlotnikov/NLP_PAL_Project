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
            List<Task<bool>> taskList = new List<Task<bool>>();
            // Send prompt to GPT and get code - Dan
            foreach (QuestionObj obj in questionObjs)
            {
                if (taskList.Count > 10 * (Consts.CohereAccessKeys.Count - 1))
                {
                    await Task.WhenAll(taskList);
                    taskList = new List<Task<bool>>();
                    Thread.Sleep(60000);
                }
                foreach (KeyValuePair<Language, QuestionLanguageObj> languageObj in obj.LanguageObjects)
                {
                    Task<bool> task = ProcessCompletionRequest(languageObj.Value);
                    taskList.Add(task);
                }
            }
            Thread.Sleep(60000);
            await Task.WhenAll(taskList);
            return true;
        }
    }
}
