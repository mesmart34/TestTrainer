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
        private List<string> options;
        public string RightAnswer { get; private set; }

        public Choise(string text)
        {
            options = new List<string>();
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
                RightAnswer = Normalize(option);
            options.Add(Normalize(option));
        }

        public bool IsRight(string answer)
        {
            return RightAnswer == answer;
        }

        QuestionType IQuestion.GetType()
        {
            return QuestionType.Choise;
        }
    }
}
