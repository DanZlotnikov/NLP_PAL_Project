using NLP_PAL_Project.Models;

namespace NLP_PAL_Project.Logic
{
    public interface AILogic
    {
        public abstract Task<bool> ProcessCompletionRequest(QuestionLanguageObj questionObj);
        public abstract Task<bool> GeneratePalAnswers(List<QuestionObj> questionObjs);
    }
}
