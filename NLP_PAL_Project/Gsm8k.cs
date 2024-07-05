using System;
using System.Text.Json;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NLP_PAL_Project.Models;

namespace NLP_PAL_Project
{
    public class Gsm8k
    {
        public static async Task<List<QuestionObj>> ReadQuestionsFromJsonAsync(string filePath)
        {
            List<QuestionObj> questionObjs = new List<QuestionObj>();

            using (StreamReader reader = new StreamReader(filePath))
            {
                int id = 1;
                while (!reader.EndOfStream)
                {
                    string line = await reader.ReadLineAsync();
                    var data = JsonSerializer.Deserialize<Dictionary<string, string>>(line);

                    QuestionObj questionObj = new QuestionObj
                    {
                        Id = id,
                        RealQuestion = data["question"],
                        RealAnswer = data["answer"]
                    };

                    questionObjs.Add(questionObj);
                    id++;
                }
            }
            return questionObjs;
        }
    }
}

