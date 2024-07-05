using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLP_PAL_Project.Models
{
    public class QuestionObj
    {
        public int Id { get; set; }
        public string? RealQuestion { get; set; }
        public string? RealAnswer { get; set; }
        public Dictionary<Language, QuestionLanguageObj> LanguageObjects {get; set; }
        
   public override string ToString()
        {
            return $"Id: {Id}\nIntro: {Intro}\nExampleQuestion: {ExampleQuestion}\nExampleAnswer: {ExampleAnswer}\nRealQuestion: {RealQuestion}\n";
        }
        
     public QuestionObj(int id, string realQuestion, string realAnswer) 
        {
            Id = id;
            RealQuestion = realQuestion;
            RealAnswer = realAnswer;
            LanguageObjects = new Dictionary<Language, QuestionLanguageObj>
            {
                {
                    Language.Python,
                    new QuestionLanguageObj
                    {
                        ExampleQuestion = Consts.ExampleQuestion,
                        ExampleAnswer = Consts.PythonExampleAnswer,
                        RealQuestion = this.RealQuestion,
                    }
                },
                {
                    Language.JS,
                    new QuestionLanguageObj
                    {
                        ExampleQuestion = Consts.ExampleQuestion,
                        ExampleAnswer = Consts.JSExampleAnswer,
                        RealQuestion = this.RealQuestion,
                    }
                },
                {
                    Language.Ruby,
                    new QuestionLanguageObj
                    {
                        ExampleQuestion = Consts.ExampleQuestion,
                        ExampleAnswer = Consts.RubyExampleAnswer,
                        RealQuestion = this.RealQuestion,
                    }
                }
            };
        }
    }
}
