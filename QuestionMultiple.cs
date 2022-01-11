using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestTrainer
{
    internal class QuestionMultiple : IQuestion
    {
        private string text;
        public List<string> Options { get; private set; }
        public List<string> RightAnswers { get; private set; }
        private bool tried = false;

        public QuestionMultiple(string text)
        {
            Options = new List<string>();
            RightAnswers = new List<string>();
            this.text = text;
        }

        public string GetText()
        {
            this.text = this.text.Remove(0, this.text.IndexOf('['));
            return text;
        }

        public string Normalize(string option)
        {
            if (option.Contains("&"))
                option = option.Replace("&", "");
            if (option[1] == ')')
                option = option.Remove(0, 2);
            return option.Trim();
        }

        public void AddOption(string option)
        {
            if (option.Contains("&"))
                RightAnswers.Add(Normalize(option));
            Options.Add(Normalize(option));
        }

        public bool IsRight(List<string> answers)
        {
            tried = true;
            if (answers.Count != RightAnswers.Count)
                return false;
            foreach(var item in answers)
            {
                if (!RightAnswers.Contains(item))
                    return false;
            }
            return true;
        }

        QuestionType IQuestion.GetType()
        {
            return QuestionType.Mulitple;
        }

        public bool IsTried() => tried;
    }
}
