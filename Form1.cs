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
        public Form1()
        {
            InitializeComponent();
            TestParser parser = new TestParser("DataBase.txt");
            var questions = parser.GetQuestions();
        }
    }
}
