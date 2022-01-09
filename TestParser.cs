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

        public TestParser(string pathToDataBase)
        {
            questions = new List<IQuestion>();
            var lines = LoadFileAsLines(pathToDataBase).Where(x => x.Length > 0).ToList();
            //var questionType = QuestionType.None;
            IQuestion question = null;
            for (var lineIndex = 0; lineIndex < lines.Count; lineIndex++)
            {
                var line = lines[lineIndex];
                if (string.IsNullOrEmpty(line))
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
                    }
                }
            }
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
            }
            return null;
        }

        private QuestionType GetQuestionType(string line)
        {
            if (line.Contains("(У)"))
                return QuestionType.Order;

            return QuestionType.None;
        }

        private string[] LoadFileAsLines(string path)
        {
            return File.ReadAllLines(path);
        }

        public IEnumerable<IQuestion> GetQuestions()
        {
            foreach (var question in questions)
                yield return question;
        }
    }
}
