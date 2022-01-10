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
        public List<string> options { get; private set; }

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
            this.text = this.text.Remove(0, this.text.IndexOf('['));
            return text;
        }

        QuestionType IQuestion.GetType()
        {
            return QuestionType.Order;
        }
    }
}
