using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestTrainer
{
    internal class QuestionOrder : IQuestion
    {
        private string text;
        public List<string> RightAnswers { get; private set; }
        public List<string> Options { get; private set; }
        private Random rnd;

        public QuestionOrder(string text)
        {
            rnd = new Random();
            this.text = text;
            RightAnswers = new List<string>();
            Options = new List<string>();
           
        }

        public string GetRightAnswer()
        {
            var right = "";
            foreach (var item in Options)
            {
                var index = RightAnswers.IndexOf(item);
                if (index == -1)
                    return string.Empty;
                right += (index + 1).ToString();
            }
            return right;
        }

        public bool IsRight(string answers)
        {
            return answers == GetRightAnswer();
        }


        public void AddOption(string option)
        {
            RightAnswers.Add(option);
            Options.Add(option);
            Options = Options.OrderBy(x => rnd.Next()).ToList();
        }

        public string GetText()
        {
            return text.Remove(0, text.IndexOf('['));
        }

        QuestionType IQuestion.GetType()
        {
            return QuestionType.Order;
        }
    }
}
