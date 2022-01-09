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
        private List<string> options;

        public QuestionOrder(string text)
        {
            this.text = text;
            options = new List<string>();
        }

        public void AddOption(string option)
        {
            options.Add(option);
        }

        public string GetText()
        {
            return text;
        }

        QuestionType IQuestion.GetType()
        {
            return QuestionType.Order;
        }
    }
}
