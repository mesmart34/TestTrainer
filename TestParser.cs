using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestTrainer
{
    public class TestParser
    {
        enum LineType
        {
            Question, Answer
        }

        private List<IQuestion> questions;
        private int questionToGenerate;

        public TestParser(string pathToDataBase, int n)
        {
            questionToGenerate = n;
            questions = new List<IQuestion>();
            var lines = LoadFileAsLines(pathToDataBase).Where(x => x.Length > 0).ToList();
            IQuestion question = null;
            for (var lineIndex = 0; lineIndex < lines.Count; lineIndex++)
            {
                var line = lines[lineIndex];
                if (string.IsNullOrEmpty(line))
                    continue;
                if (line.StartsWith("V"))
                    continue;
                if (GetLineType(line) == LineType.Question)
                {
                    var questionType = GetQuestionType(line);
                    question = MakeQuestion(line, questionType);
                    if (question != null) 
                        questions.Add(question);
                }
                else
                {
                    if (question == null)
                        continue;
                    switch (question.GetType())
                    {
                        case QuestionType.Order:
                            {
                                var order = (QuestionOrder)question;
                                order.AddOption(line);
                            }
                            break;
                        case QuestionType.Choise:
                            {
                                var choise = (Choise)question;
                                choise.AddOption(line);
                            }
                            break;
                        case QuestionType.Mulitple:
                            {
                                var mult = (QuestionMultiple)question;
                                mult.AddOption(line);
                            }
                            break;
                        case QuestionType.Write:
                            {
                                var choise = (WriteAnswer)question;

                                //choise.AddOption(line);
                            }
                            break;
                        case QuestionType.Correlate:
                            {
                                var correlate = (Correlate)question;
                                correlate.AddOption(line);

                            }
                            break;
                        case QuestionType.Picture:
                            {
                                var correlate = (PictureQuestion)question;
                                correlate.AddOption(line);

                            }
                            break;
                    }
                }
            }
        }

        public int GetQuestionsNumber()
        {
            if (questionToGenerate > questions.Count)
                questionToGenerate = questions.Count;
            return questionToGenerate;
        }

        private LineType GetLineType(string text)
        {
            return text.Contains('[') ? LineType.Question : LineType.Answer;
        }

        private IQuestion MakeQuestion(string text, QuestionType type)
        {
            switch (type)
            {
                case QuestionType.Order:
                    {
                        return new QuestionOrder(text);
                    };
                case QuestionType.Choise:
                    {
                        return new Choise(text);
                    }
                case QuestionType.Write:
                    {
                        return new WriteAnswer(text);
                    };
                case QuestionType.Mulitple:
                    {
                        return new QuestionMultiple(text);
                    };
                case QuestionType.Correlate:
                    {
                        return new Correlate(text);
                    };
                case QuestionType.Picture:
                    {
                        return new PictureQuestion(text);
                    };
            }
            return null;
        }

        private QuestionType GetQuestionType(string line)
        {
            if (line.Contains("(У)"))
                return QuestionType.Order;
            if (line.Contains("(ВО1)"))
                return QuestionType.Choise;
            if (line.Contains("(О)"))
                return QuestionType.Write;
            if (line.Contains("(ВОМ)"))
                return QuestionType.Mulitple;
            if (line.Contains("(С)"))
                return QuestionType.Correlate;
            if (line.Contains("(ВО1К)"))
                return QuestionType.Picture;
            return QuestionType.None;
        }

        private string[] LoadFileAsLines(string path)
        {
            return File.ReadAllLines(path);
        }

        public IEnumerator<IQuestion> GetQuestions()
        {
            var rnd = new Random();
            foreach (var question in questions.OrderBy(item => rnd.Next()).Take(GetQuestionsNumber()))
                yield return question;
        }
    }
}
