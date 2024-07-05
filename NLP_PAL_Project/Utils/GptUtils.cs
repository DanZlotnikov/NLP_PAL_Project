using NLP_PAL_Project.Models;
using System.Data;
using System.Text;

namespace NLP_PAL_Project.Utils.Utils
{
    public class GptUtils
    {
        public static StringContent GenerateGptRequestBody(QuestionLanguageObj param)
        {
            StringContent stringContent = new(
                System.Text.Json.JsonSerializer.Serialize(new
                {
                    model = "gpt-3.5-turbo",
                    messages = CreateMessageList(param)
                }),
                Encoding.UTF8,
                "application/json"
            );
            return stringContent;
        }

        public static List<GptRequestMessageObject> CreateMessageList(QuestionLanguageObj questionObj)
        {
            List<GptRequestMessageObject> messages;
            messages = new List<GptRequestMessageObject> {
               new GptRequestMessageObject {
                   role = Consts.GptUserRole,
                   content = string.Format($"{questionObj.ExampleQuestion} \n\n {questionObj.ExampleAnswer} \n\n ")
               }
            };

            return messages;
        }
    }
}
