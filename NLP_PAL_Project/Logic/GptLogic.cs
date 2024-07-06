using NLP_PAL_Project.Models;
using NLP_PAL_Project.Utils;
using NLP_PAL_Project.Utils.Utils;

namespace NLP_PAL_Project.Logic
{
    public class GptLogic : AILogic
    {
        public async Task<bool> ProcessCompletionRequest(QuestionLanguageObj questionObj)
        {
            ApiCompletionResponse ret = new ApiCompletionResponse();
            dynamic response = await GeneralUtils.PostRequest(Consts.BaseUrl, GptUtils.GenerateGptRequestBody(questionObj));
            ret.Id = response["id"];
            ret.Content = response["choices"][0]["message"]["content"];
            ret.FinishReason = response["choices"][0]["finish_reason"];
            return true;
        }

        public async Task<bool> GeneratePalAnswers(List<QuestionObj> questionObjs)
        {
            return true;
        }
    }
}
