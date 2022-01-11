using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestTrainer
{
    internal class Correlate : IQuestion
    {
        private string text;
        public List<string> Left;
        public List<string> Right;
        public List<string> RightShuffled { get; private set; }
        private Random rnd;
        private bool tried = false;

        public Correlate(string text)
        {
            rnd = new Random();
            this.text = text;
            Left = new List<string>();
            Right = new List<string>();
            RightShuffled = new List<string>();
        }

        public string GetText()
        {
            return text;
        }

        public bool IsTried()
        {
            return tried;
        }

        public bool IsRight(string answers)
        {
            tried = true;
            return answers == GetRightAnswer();
        }

        public void AddOption(string option)
        {
            if(char.IsDigit(option[0]))
            {
                Left.Add(option);
            }
            else
            {
                Right.Add(option);
                RightShuffled = Right.OrderBy(x => rnd.Next()).ToList();
            }
        }

        public string GetRightAnswer()
        {

            var right = "";
            for (var i = 0; i < Left.Count; i++)
            {
                var r = RightShuffled.IndexOf(Right[i]);
                right += Right[r][0].ToString();
            }
            return right;
        }

        QuestionType IQuestion.GetType()
        {
            return QuestionType.Correlate;
        }
    }
}
