namespace NLP_PAL_Project.Models
{
    public class ApiCompletionResponse
    {
        public string Id { get; set; }
        public string Content { get; set; }
        public string FinishReason { get; set; }
        public QuestionLanguageObj OriginalRequest { get; set; }
        public int TotalTokens { get; set; }
    }
}
