using Newtonsoft.Json;
using NLP_PAL_Project.Models;
using NLP_PAL_Project.Utils;

namespace NLP_PAL_Project.Logic
{
    public class CohereLogic : AILogic
    {
        public async Task<bool> ProcessCompletionRequest(QuestionLanguageObj languageObj)
        {
            ApiCompletionResponse ret = new ApiCompletionResponse();
            dynamic response = await GeneralUtils.PostRequest(Consts.BaseUrl, CohereUtils.GenerateCohereRequestBody(languageObj));
            languageObj.GeneratedAnswer = response["text"];
            return true;
        }

        public async Task<bool> GeneratePalAnswers(List<QuestionObj> questionObjs)
        {
            List<Task<bool>> taskList = new List<Task<bool>>();
            // Send prompt to GPT and get code - Dan
            foreach (QuestionObj obj in questionObjs)
            {
                foreach (KeyValuePair<Language, QuestionLanguageObj> languageObj in obj.LanguageObjects)
                {
                    Task<bool> task = ProcessCompletionRequest(languageObj.Value);
                    taskList.Add(task);
                }
            }
            List<List<Task<bool>>> splitTaskLists = GeneralUtils.SplitTasks(taskList, 60);
            foreach (List<Task<bool>> tasks in splitTaskLists)
            {
                await Task.WhenAll(tasks);
                Thread.Sleep(60000);
            }
            return true;
        }
    }
}
