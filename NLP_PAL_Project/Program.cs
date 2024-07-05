﻿using System;
using NLP_PAL_Project.Logic;
using NLP_PAL_Project.Models;

namespace NLP_PAL_Project
{
    class Program
    {
        public static async Task Main(string[] args)
        {
            Consts.Init();
            // Read all prompt data - Bar
            List<QuestionObj> questionObjs = new List<QuestionObj> {
                new QuestionObj
                {
                    Id = 1,
                    RealQuestion= "Jason had 20 lollipops. He gave Denny some lollipops. Now Jason has 12 lollipops. How many lollipops did Jason give to Denny?",
                }
            };

            AILogic AI = new CohereLogic();
            dynamic[] codeSnippets = await AI.GeneratePalAnswers(questionObjs);

            // Execute code on compilers - Denis
            CodeExecutor codeExecutor= new CodeExecutor();
            string CSharpOutput = await codeExecutor.ExecuteCSharpCode("");
            string pythonOutput= codeExecutor.ExecutePythonCode(codeSnippets[0].Content);
            string JSOutput = codeExecutor.ExecuteJavaScriptCode("");
            // Get answers from compilers and give scores to languages

            // Graphs, stats, etc.






        }
    }
}