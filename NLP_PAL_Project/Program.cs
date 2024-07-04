﻿using NLP_PAL_Project.Models;

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
                    Intro = "",
                    ExampleQuestion = "Q: There were nine computers in the server room. Five more computers were installed each day, from monday to thursday. How many computers are now in the server room?",
                    ExampleAnswer = "A: # solution in Python:\r\n\"\"\"There were nine computers in the server room. Five more computers were installed each day, from monday to thursday. How many computers are now in the server room?\"\"\"\r\ncomputers_initial = 9\r\ncomputers_per_day = 5\r\nnum_days = 4  # 4 days between monday and thursday\r\ncomputers_added = computers_per_day * num_days\r\ncomputers_total = computers_initial + computers_added\r\nresult = computers_total\r\nreturn result",
                    RealQuestion= "Q: Jason had 20 lollipops. He gave Denny some lollipops. Now Jason has 12 lollipops. How many lollipops did Jason give to Denny?",
                }
            };

            dynamic[] codeSnippets = await GptLogic.GeneratePalAnswers(questionObjs);

            // Execute code on compilers - Denis

            // Get answers from compilers and give scores to languages

            // Graphs, stats, etc.
        }
    }
}