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
        List<RadioButton> radioButtons;
        int n = 60;

        public Form1()
        {
            InitializeComponent();
            checkBoxes = new List<CheckBox>();
            radioButtons = new List<RadioButton>();
            var parser = new TestParser("DataBase.txt", n);
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
                            size.Width = 1000;
                            checkBox.Bounds = new Rectangle(new Point(10, i * size.Height + 5), size);
                            checkBox.AutoSize = true;
                            checkBoxes.Add(checkBox);
                            answerPanel.Controls.Add(checkBoxes[i]);
                        }
                    }
                    break;
                case QuestionType.Choise:
                    {
                        var choice = (Choise)question;
                        radioButtons.Clear();
                        answerPanel.Controls.Clear();
                        for (int i = 0; i < choice.Options.Count; i++)
                        {
                            var radio = new RadioButton();
                            radio.Text = choice.Options[i];
                            var size = radio.Bounds.Size;
                            size.Width = 1000;
                            radio.AutoSize = true;
                            radio.Bounds = new Rectangle(new Point(10, i * size.Height + 5), size);
                            radioButtons.Add(radio);
                            answerPanel.Controls.Add(radioButtons[i]);
                        }
                    }
                    break;
                case QuestionType.Order:
                    {
                        var order = (QuestionOrder)question;
                        int counter = 0;
                        foreach (var item in order.Options)
                        {
                            var index = item.IndexOf(')');
                            if(index == -1)
                                continue;
                            counter++;
                            label1.Text += '\n' + counter.ToString() + ". " + item.Remove(0, index + 1).Trim(' ');
                        }
                    }
                    break;
            };
        }

        private void ShowQuestion(IQuestion question)
        {
            label1.ForeColor = Color.Black;
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
            txtAnwser.BackColor = Color.White;
            if (questions.MoveNext())
            {
                txtAnwser.Text = "";
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


        private void ProcessMultiple()
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
            {
                flag = true;
                
            }

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

        private void ProcessWrite()
        {
            var answers = txtAnwser.Text;
            var write = ((WriteAnswer)questions.Current);
            if (write.IsRight(answers))
            {
                txtAnwser.ForeColor = Color.Green;
            }
            else
            {
                txtAnwser.Text = write.RightAnswer;
                txtAnwser.ForeColor = Color.Red;
            }
        }

        private void ProcessChoise()
        {
            foreach(var radio in radioButtons)
            {
                radio.BackColor = Color.Transparent;
                if(radio.Checked)
                {
                    radio.BackColor = Color.Red;
                    if (((Choise)questions.Current).IsRight(radio.Text))
                        radio.BackColor = Color.Green;
                }
                else
                {
                    if (((Choise)questions.Current).IsRight(radio.Text))
                        radio.BackColor = Color.Yellow;
                }
            }
        }

        private void ProcessOrder()
        {
            var order = ((QuestionOrder)questions.Current);
            if (order.IsRight(txtAnwser.Text))
                txtAnwser.BackColor = Color.Green;
            else
            {
                txtAnwser.BackColor = Color.Red;
                txtAnwser.Text = order.GetRightAnswer();
            }
        }

        private void AnswerButton(object sender, EventArgs e)
        {
            if (questions.Current.GetType() == QuestionType.Mulitple)
            {
                ProcessMultiple();
            }
            else if (questions.Current.GetType() == QuestionType.Write)
            {
                ProcessWrite();
            }
            else if (questions.Current.GetType() == QuestionType.Choise)
            {
                ProcessChoise();
            }else if (questions.Current.GetType() == QuestionType.Order)
            {
                ProcessOrder();
            }
            btnNext.Enabled = true;
        }
    }
}
