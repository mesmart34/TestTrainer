using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestTrainer
{
    internal class WriteAnswer : IQuestion
    {
        private string text;
        public string RightAnswer { get; private set; }


        public WriteAnswer(string text)
        {
            this.text = text;
            FindRightAnswer(text);
        }

        private bool FindRightAnswer(string text)
        {
            if(text.Contains("&"))
            {
                var splitted = text.Split(' ');
                RightAnswer = splitted.Select(x => x).Where(x => x.StartsWith("&")).First();
                this.text = text.Replace(RightAnswer, "_____");
                RightAnswer = RightAnswer.Replace("&", "");
                RightAnswer = RightAnswer.Replace(",", "");
                return true;
            }

            var splited = text.Split(' ');
            var found = true;
            foreach (var item in splited)
            {
                if (string.IsNullOrEmpty(item))
                    continue;
                found = true;
                foreach (var s in item)
                {
                    if (!char.IsUpper(s))
                    {
                        found = false;
                        break;
                    }
                }
                if (found)
                {
                    RightAnswer = item;
                    this.text = this.text.Replace(RightAnswer, "_______");
                    break;
                }
            }
            return false;
        }

        public bool IsRight(string answer)
        {
            return RightAnswer.ToLower().Replace(" ", "") == answer.ToLower().Replace(" ", "");
        }

        public string GetText()
        {
            this.text = this.text.Remove(0, this.text.IndexOf('['));
            return text;
        }

        QuestionType IQuestion.GetType()
        {
            return QuestionType.Write;
        }
    }
}
