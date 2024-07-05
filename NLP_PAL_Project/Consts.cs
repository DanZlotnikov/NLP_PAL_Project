using Newtonsoft.Json;

namespace NLP_PAL_Project
{
    public class Consts
    {
        public static dynamic? config;
        public static string? BaseUrl;
        public static string? MediaType;
        public static string? GptUserRole;
        public static string? GptAccessKey;

        public static readonly string ExampleQuestion = "There were nine computers in the server room. Five more computers were installed each day, from monday to thursday. How many computers are now in the server room?";

        #region example answers
        public static readonly string PythonExampleAnswer = "# Initial number of computers in the server room\ninitial_computers = 9\n\n# Number of computers added each day\ncomputers_added_per_day = 5\n\n# Number of days computers were added (from Monday to Thursday)\ndays_added = 4\n\n# Total number of computers added\ntotal_computers_added = computers_added_per_day * days_added\n\n# Total number of computers in the server room now\ntotal_computers_now = initial_computers + total_computers_added\nprint(ntotal_computers_now)";
        public static readonly string JSExampleAnswer = "// Initial number of computers in the server room\nlet initialComputers = 9;\n\n// Number of computers added each day\nlet computersAddedPerDay = 5;\n\n// Number of days computers were added (from Monday to Thursday)\nlet daysAdded = 4;\n\n// Total number of computers added\nlet totalComputersAdded = computersAddedPerDay * daysAdded;\n\n// Total number of computers in the server room now\nlet totalComputersNow = initialComputers + totalComputersAdded;\n\nconsole.log(totalComputersNow);";
        public static readonly string RubyExampleAnswer = "# Initial number of computers in the server room\ninitial_computers = 9\n\n# Number of computers added each day\ncomputers_added_per_day = 5\n\n# Number of days computers were added (from Monday to Thursday)\ndays_added = 4\n\n# Total number of computers added\ntotal_computers_added = computers_added_per_day * days_added\n\n# Total number of computers in the server room now\ntotal_computers_now = initial_computers + total_computers_added\n\nputs total_computers_now";
        #endregion

        public static void Init()
        {
            config = JsonConvert.DeserializeObject<dynamic>(File.ReadAllText(@"../../../config.json"));
            BaseUrl = config["GPT"]["BaseUrl"];
            MediaType = config["GPT"]["MediaType"];
            GptUserRole = config["GPT"]["Roles"]["User"];
            GptAccessKey = config["GPT"]["AccessKey"];
        }
    }

    public enum Language
    {
        Python,
        JS,
        Ruby
    }
}
