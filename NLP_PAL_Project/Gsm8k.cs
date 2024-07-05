﻿using System.Text.Json;
using Newtonsoft.Json;
using NLP_PAL_Project.Models;
using System.Text.Json;
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
                    var data = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, string>>(line);
                    questionObjs.Add(new QuestionObj(id++, data["question"], data["answer"]));
                }
            }
            return questionObjs;
        }
    }
}
