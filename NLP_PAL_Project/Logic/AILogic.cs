using NLP_PAL_Project.Models;

namespace NLP_PAL_Project.Logic
{
    public interface AILogic
    {
        public abstract Task<dynamic> ProcessCompletionRequest(QuestionObj questionObj);
        public abstract Task<dynamic> GeneratePalAnswers(List<QuestionObj> questionObjs);
    }
}
