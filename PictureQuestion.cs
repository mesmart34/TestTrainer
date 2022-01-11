using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestTrainer
{
    internal class PictureQuestion : IQuestion
    {
        private string text;
        public List<string> Options { get; private set; }
        private string _rightAnswer;
        private bool tried = false;
        public Image Image { get; private set; }


        public PictureQuestion(string text)
        {
            Options = new List<string>();
            this.text = text;
            try
            {
                Image = Image.FromFile("src/" + text + ".png");
            } catch (Exception ex)
            {
            }
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
            tried = true;
            return _rightAnswer == answer;
        }

        QuestionType IQuestion.GetType()
        {
            return QuestionType.Picture;
        }

        public bool IsTried() => tried;
    }
}
