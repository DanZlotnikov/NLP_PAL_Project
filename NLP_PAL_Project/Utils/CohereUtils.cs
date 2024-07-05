﻿using NLP_PAL_Project.Models;
using System.Data;
using System.Text;

namespace NLP_PAL_Project.Utils
{
    public class CohereUtils
    {
        public static StringContent GenerateCohereRequestBody(QuestionObj questionObj)
        {
            StringContent stringContent = new(
                System.Text.Json.JsonSerializer.Serialize(new
                {
                    chat_history = CreateHistoryList(questionObj),
                    message = questionObj.RealQuestion
                }),
                Encoding.UTF8,
                "application/json"
            );
            return stringContent;
        }

        public static List<CohereRequestMessageObject> CreateHistoryList(QuestionObj questionObj)
        {
            List<CohereRequestMessageObject> history;
            history = new List<CohereRequestMessageObject> {
               new CohereRequestMessageObject {
                   role = "USER",
                   message = string.Format($"{questionObj.ExampleQuestion} \n\n")
               },
               new CohereRequestMessageObject {
                   role = "CHATBOT",
                   message = string.Format($"{questionObj.ExampleAnswer} \n\n")
               },
            };

            return history;
        }
    }
}
