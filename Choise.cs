using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestTrainer
{
    internal class Choise : IQuestion
    {
        private string text;
        public List<string> Options { get; private set; }
        private string _rightAnswer;

        public Choise(string text)
        {
            Options = new List<string>();
            this.text = text;
        }

        public string GetText()
        {
            return text.Remove(0, text.IndexOf('['));
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
                _rightAnswer = Normalize(option);
            Options.Add(Normalize(option));
        }

        public bool IsRight(string answer)
        {
            return _rightAnswer == answer;
        }

        QuestionType IQuestion.GetType()
        {
            return QuestionType.Choise;
        }
    }
}
