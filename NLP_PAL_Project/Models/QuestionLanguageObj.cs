namespace NLP_PAL_Project.Models
{
    public class QuestionLanguageObj
    {
        public string ExampleQuestion = "There were nine computers in the server room. Five more computers were installed each day, from monday to thursday. How many computers are now in the server room?";
        public string ExampleAnswer = "# solution in Python: \n\n computers_initial = 9\r\ncomputers_per_day = 5\r\nnum_days = 4  # 4 days between monday and thursday\r\ncomputers_added = computers_per_day * num_days\r\ncomputers_total = computers_initial + computers_added\r\nresult = computers_total\r\nprint(result)";
        public string RealQuestion { get; set; }
        public string RealAnswer { get; set; }

    }
}
