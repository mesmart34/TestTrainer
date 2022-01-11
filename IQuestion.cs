using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestTrainer
{
    public enum QuestionType
    {
        None, Order, Choise, Write, Mulitple, Correlate, Picture
    }

    public interface IQuestion
    {
        //void AddAnswer(IAnswer answer);
        string GetText();
        QuestionType GetType();

        bool IsTried();
    }
}
