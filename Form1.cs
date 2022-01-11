using Microsoft.VisualBasic;
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
        int n = 0;
        int rights = 0;


        public Form1()
        {
            InitializeComponent();
            FormBorderStyle = FormBorderStyle.FixedDialog;
            string input = Interaction.InputBox("Кол-во вопросов?", "Тренер Рома просит кол-во вопросов", "60");
            if (string.IsNullOrEmpty(input))
                Environment.Exit(1);
            n = int.Parse(input);
            checkBoxes = new List<CheckBox>();
            radioButtons = new List<RadioButton>();
            var parser = new TestParser("DataBase.txt", n);
            n = parser.GetQuestionsNumber();
            questions = parser.GetQuestions();
            questions.MoveNext();
            ShowQuestion(questions.Current);
        }

        private void Draw(IQuestion question)
        {
            pictureBox1.Image = null;
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
                        foreach (var item in order.Shuffled)
                        {
                            var index = item.IndexOf(')');
                            if (index == -1)
                                continue;
                            counter++;
                            label1.Text += '\n' + counter.ToString() + ". " + item.Remove(0, index + 1).Trim(' ');
                        }
                    }
                    break;
                case QuestionType.Correlate:
                    {
                        var correlate = (Correlate)question;
                        var sb = new StringBuilder();
                        foreach (var item in correlate.Left)
                        {
                            sb.AppendLine(item);
                        }
                        leftLabel.Text = sb.ToString();
                        sb.Clear();
                        int counter = 0;
                        foreach (var item in correlate.RightShuffled)
                        {
                            sb.AppendLine((char)('А' + counter++) + item.Substring(1, item.Length - 1));
                        }
                        rightLabel.Text = sb.ToString();
                    }
                    break;
                case QuestionType.Picture:
                    {
                        var pic = (PictureQuestion)question;
                        label1.Text = pic.GetText();
                        radioButtons.Clear();
                        answerPanel.Controls.Clear();
                        pictureBox1.Image = pic.Image;
                        for (int i = 0; i < pic.Options.Count; i++)
                        {
                            var radio = new RadioButton();
                            radio.Text = pic.Options[i];
                            var size = radio.Bounds.Size;
                            size.Width = 1000;
                            radio.AutoSize = true;
                            radio.Bounds = new Rectangle(new Point(10, i * size.Height + 5), size);
                            radioButtons.Add(radio);
                            answerPanel.Controls.Add(radioButtons[i]);
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
                ShowCorrelateLabels(false);
                ShowSelectAnswer(false);
            }
            else if (question is QuestionMultiple)
            {
                ShowWriteAnswer(false);
                ShowCorrelateLabels(false);
                ShowSelectAnswer(true);
            }
            else if (question is Choise || question is PictureQuestion)
            {
                ShowWriteAnswer(false);
                ShowCorrelateLabels(false);
                ShowSelectAnswer(true);
            }
            else if (question is QuestionOrder)
            {
                ShowWriteAnswer(true);
                ShowCorrelateLabels(false);
                ShowSelectAnswer(false);
            }
            else if (question is Correlate)
            {
                ShowWriteAnswer(true);
                ShowSelectAnswer(false);
                ShowCorrelateLabels(true);
            }
            Draw(question);
        }

        private void ShowCorrelateLabels(bool value)
        {
            leftPanel.Visible = value;
            leftPanel.Enabled = value;
            rightPanel.Visible = value;
            rightPanel.Enabled = value;
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
                End();
            }
        }

        private void End()
        {
            label1.Text = "Всё епта";
            btnNext.Enabled = false;
            ShowWriteAnswer(false);
            ShowSelectAnswer(false);
            var sb = new StringBuilder();
            int success = (int)((rights / (double)n) * 100.0);
            sb.Append("Тренировка с Ромой выполнена на: " + success.ToString() + "%");
            if (success >= 60)
                sb.Append("\nРома вами доволен!");
            else
                sb.Append("\nВас ждет неприятный разговор с Ромой!");
            Interaction.MsgBox(sb.ToString(), MsgBoxStyle.OkOnly, "Статистика");
        }


        private void ProcessMultiple()
        {
            bool flag = false;
            var multiple = ((QuestionMultiple)questions.Current);
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

            if (!multiple.IsTried())
            {
                if (multiple.IsRight(answers))
                {
                    flag = true;
                    rights++;
                }
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
            if (!write.IsTried())
            {
                if (write.IsRight(answers))
                {
                    txtAnwser.BackColor = Color.Green;
                    rights++;
                }
                else
                {
                    txtAnwser.Text = write.RightAnswer;
                    txtAnwser.BackColor = Color.Red;
                }
            }
        }

        private void ProcessChoise()
        {
            var choise = ((Choise)questions.Current);
            if (!choise.IsTried())
            {
                foreach (var radio in radioButtons)
                {
                    radio.BackColor = Color.Transparent;
                    if (radio.Checked)
                    {
                        radio.BackColor = Color.Red;
                        {
                            if (choise.IsRight(radio.Text))
                            {
                                radio.BackColor = Color.Green;
                                rights++;
                            }
                        }
                    }
                    else
                    {
                        if (((Choise)questions.Current).IsRight(radio.Text))
                            radio.BackColor = Color.Yellow;
                    }
                }
            }
        }

        private void ProcessOrder()
        {
            var order = ((QuestionOrder)questions.Current);
            if (!order.IsTried())
            {
                if (order.IsRight(txtAnwser.Text))
                {
                    txtAnwser.BackColor = Color.Green;
                    rights++;
                }
                else
                {
                    txtAnwser.BackColor = Color.Red;
                    txtAnwser.Text = order.GetRightAnswer();
                }
            }
        }

        private void ProcessCorrelate()
        {
            var correlate = ((Correlate)questions.Current);
            if (!correlate.IsTried())
            {
                if (correlate.IsRight(txtAnwser.Text))
                {
                    txtAnwser.BackColor = Color.Green;
                    rights++;
                }
                else
                {
                    txtAnwser.BackColor = Color.Red;
                    txtAnwser.Text = correlate.GetRightAnswer();
                }
            }
        }

        private void ProcessPicture()
        {
            var choise = ((PictureQuestion)questions.Current);
            if (!choise.IsTried())
            {
                foreach (var radio in radioButtons)
                {
                    radio.BackColor = Color.Transparent;
                    if (radio.Checked)
                    {
                        radio.BackColor = Color.Red;
                        {
                            if (choise.IsRight(radio.Text))
                            {
                                radio.BackColor = Color.Green;
                                rights++;
                            }
                        }
                    }
                    else
                    {
                        if (((PictureQuestion)questions.Current).IsRight(radio.Text))
                            radio.BackColor = Color.Yellow;
                    }
                }
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
            }
            else if (questions.Current.GetType() == QuestionType.Order)
            {
                ProcessOrder();
            }
            else if (questions.Current.GetType() == QuestionType.Correlate)
            {
                ProcessCorrelate();
            }
            else if (questions.Current.GetType() == QuestionType.Correlate)
            {
                ProcessCorrelate();
            }
            else if (questions.Current.GetType() == QuestionType.Picture)
            {
                ProcessPicture();
            }
            btnNext.Enabled = true;
        }
    }
}
