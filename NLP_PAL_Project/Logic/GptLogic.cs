using NLP_PAL_Project.Models;
using NLP_PAL_Project.Utils;

namespace NLP_PAL_Project.Logic
{
    public class GptLogic : AILogic
    {
        public async Task<dynamic> ProcessCompletionRequest(QuestionObj questionObj)
        {
            ApiCompletionResponse ret = new ApiCompletionResponse();
            dynamic response = await GeneralUtils.PostRequest(Consts.BaseUrl, CohereUtils.GenerateCohereRequestBody(questionObj));
            ret.Id = response["id"];
            ret.Content = response["choices"][0]["message"]["content"];
            ret.FinishReason = response["choices"][0]["finish_reason"];
            ret.OriginalRequest = questionObj;
            return ret;
        }

        public async Task<dynamic> GeneratePalAnswers(List<QuestionObj> questionObjs)
        {
            List<Task<dynamic>> taskList = new List<Task<dynamic>>();
            // Send prompt to GPT and get code - Dan
            foreach (QuestionObj obj in questionObjs)
            {
                Task<dynamic> task = ProcessCompletionRequest(obj);
                taskList.Add(task);
            }
            dynamic[] results = await Task.WhenAll(taskList.ToArray());
            return results;
        }
    }
}
