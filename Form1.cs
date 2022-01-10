using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestTrainer
{
    public partial class Form1 : Form
    {
        IEnumerator<IQuestion> questions;
        List<CheckBox> checkBoxes;

        public Form1()
        {
            checkBoxes = new List<CheckBox>();
            InitializeComponent();
            TestParser parser = new TestParser("DataBase1.txt", 10);
            questions = parser.GetQuestions();
            questions.MoveNext();
            ShowQuestion(questions.Current);
        }

        private void Draw(IQuestion question)
        {
            switch (question.GetType())
            {
                case QuestionType.Mulitple:
                    {
                        var mult = (QuestionMultiple)question;
                        checkBoxes.Clear();
                        answerPanel.Controls.Clear();
                        for (int i = 0; i < mult.Options.Count; i++)
                        {
                            var checkBox = new CheckBox();
                            checkBox.Text = mult.Options[i];
                            var size = checkBox.Bounds.Size;
                            checkBox.Bounds = new Rectangle(new Point(10, i * size.Height + 5), size);
                            checkBoxes.Add(checkBox);
                            answerPanel.Controls.Add(checkBoxes[i]);
                        }
                    }
                    break;
            };
        }

        private void ShowQuestion(IQuestion question)
        {
            label1.Text = question.GetText();
            if (question is WriteAnswer)
            {
                ShowWriteAnswer(true);
                ShowSelectAnswer(false);
            }
            else if (question is QuestionMultiple)
            {
                ShowWriteAnswer(false);
                ShowSelectAnswer(true);
            }
            else if (question is Choise)
            {
                ShowWriteAnswer(false);
                ShowSelectAnswer(true);
            }
            else if (question is QuestionOrder)
            {
                ShowWriteAnswer(true);
                ShowSelectAnswer(false);
            }
            Draw(question);
        }

        private void ShowWriteAnswer(bool value)
        {
            btnWrite.Visible = value;
            btnWrite.Enabled = value;
            txtAnwser.Visible = value;
            txtAnwser.Enabled = value;
        }

        private void ShowSelectAnswer(bool value)
        {
            answerPanel.Visible = value;
            answerPanel.Enabled = value;
            btnSelect.Visible = value;
            btnSelect.Enabled = value;
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            var btn = (Button)sender;
            btn.Enabled = false;
            if (questions.MoveNext())
            {
                ShowQuestion(questions.Current);
            }
            else
            {
                label1.Text = "Всё епта";
                btnNext.Enabled = false;
                ShowWriteAnswer(false);
                ShowSelectAnswer(false);
            }
        }

        private void btnWrite_Click(object sender, EventArgs e)
        {
            btnNext.Enabled = true;
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            if (questions.Current.GetType() == QuestionType.Mulitple)
            {
                bool flag = false;

                foreach (var check in checkBoxes)
                    check.BackColor = Color.Transparent;
                var answers = new List<string>();
                foreach (var check in checkBoxes)
                {
                    if (check.Checked)
                    {
                        check.BackColor = Color.Red;
                        answers.Add(check.Text);
                    }
                }

                if (((QuestionMultiple)questions.Current).IsRight(answers))
                    flag = true;

                foreach (var check in checkBoxes)
                {
                    if (((QuestionMultiple)questions.Current).RightAnswers.Contains(check.Text))
                    {
                        if (check.Checked)
                            check.BackColor = Color.Green;
                        else if (!flag)
                            check.BackColor = Color.Yellow;
                    }
                }
            }



            btnNext.Enabled = true;
        }

    }
}
