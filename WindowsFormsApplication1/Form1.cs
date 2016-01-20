using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
   

    public partial class Form1 : Form
    {
        public List<Drugs> data;
        public List<string> incorrect;
        public HashSet<string> correct;

        public int num;
        public bool newCat;
        public int choice;
        public int choice2;
        public string[] lines;
        public Random random;
        public int index, index2;
        public string answer;
        public bool flag;
        public char[] delim = { '/', ',' };

        public Form1()
        {
            InitializeComponent();
        }

        public class Drugs
        {
            public int num;
            public int corr;
            public List<Drug> list;
        }
        public class Drug
        {
            public int num;
            public string descr;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Height = 242;
            incorrect = new List<string>();
            correct = new HashSet<string>();

            newCat = true;
            num = 0;
            flag = true;
            answer = "____";
            random = new Random();
            textBox1.Text = "Press enter here to start";

            choice = 1;
            choice2 = 1;
            index = 0;
            data = new List<Drugs>();

            // Read the file and display it line by line.
            lines = System.IO.File.ReadAllLines("data.txt");

            comboBox1.Items.Add("All drugs");
            comboBox1.Items.Add("Drugs by numbers");
            comboBox1.Items.Add("Drugs #1-50");
            comboBox1.Items.Add("Drugs #51-100");
            comboBox1.Items.Add("Drugs #101-150");
            comboBox1.Items.Add("Drugs #151-200");

            foreach (string line in lines)
            {
                if (line.Length != 0)
                {
                    string[] temp = line.Split('\t');
                    if (newCat)
                    {
                        Drugs x = new Drugs();
                        x.num = 0;
                        x.corr = 0;
                        x.list = new List<Drug>();
                        data.Add(x);
                        newCat = false;
                        comboBox1.Items.Add(temp[3]);
                    }

                    Drug y = new Drug();
                    y.num = Int32.Parse(temp[4]);
                    y.descr = line;
                    data[data.Count - 1].list.Add(y);
                    data[data.Count - 1].num++;
                }
                else
                    newCat = true;
            }

            comboBox1.SelectedIndex = 0;
            comboBox1.SelectedIndexChanged += comboBox1_SelectedIndexChanged;
            index = random.Next(0, data.Count);
            index2 = random.Next(0, data[index].list.Count);

            string[] str = data[index].list[index2].descr.Split('\t');
            label7.Text = "____";
            label8.Text = str[1];
            label9.Text = str[2];
            label10.Text = str[3];
            textBox1.Text = "";
            label6.Text = "____";
            answer = str[0];
            
           
            textBox1.Focus();
        }
      
        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (flag)
                    checkAnswer();
                else
                    chooseQuestion();
            //    e.Handled = true;
            //    e.SuppressKeyPress = true;
            }
        }

        private void checkAnswer()
        {
            if (String.IsNullOrEmpty(textBox1.Text))
                return;
            flag = false;
       /*     int total = 0;
            for (int i = 0; i < data.Count(); i++)
                total += data[i].list.Count();
            if (total == 0)
            {
                label6.Text = "Quiz completed.";
                return;
            }
            if (comboBox1.SelectedIndex != 0 && data[index].list.Count <= 0)
            {
                label6.Text = "Category completed.";
                return;
            }
            */
            string[] str = data[index].list[index2].descr.Split('\t');

            label7.Text = str[0];
            label8.Text = str[1];
            label9.Text = str[2];
            label10.Text = str[3];

            answer = System.Text.RegularExpressions.Regex.Replace(answer, @" ?\(.*?\)", string.Empty);
            string[] splitAnswer = answer.ToLower().Split(delim);
            string response = textBox1.Text.Trim().ToLower();

            string log = data[index].list[index2].descr + "\t --->   " + response;


            if (String.Compare(answer.ToLower(), response.ToLower()) == 0)
            {
                label6.Text = "Correct: " + answer;
                correct.Add(data[index].list[index2].descr);
                data[index].corr++;
                data[index].list.RemoveAt(index2);
                listBox1.Items.Insert(0, "Correct:\t\t" + log);
                return;
            }

            label6.Text = "Incorrect: " + answer;
            foreach (string part in splitAnswer)
                if (String.Compare(response, part.Trim()) == 0)
                {
                    label6.Text = "Correct: " + answer;
                    correct.Add(data[index].list[index2].descr);
                    data[index].corr++;
                    data[index].list.RemoveAt(index2);

                    listBox1.Items.Insert(0, "Correct:\t\t" + log);
                    return;
                }

            listBox1.Items.Insert(0, "Incorrect:\t\t" + log);
            incorrect.Add(log);
        }
        
        private void chooseQuestion()
        {
            panel1.Visible = false;
            int total = 0;
            for (int i = 0; i < data.Count; i++)
                total += data[i].list.Count;
            if (total == 0)
            {
                label6.Text = "Quiz completed.";
                label2.Text = "200/200";
                return;
            }

            if (comboBox1.SelectedIndex == 0)
            {
                label2.Text = correct.Count.ToString() + "/200";

                radioButton4.Enabled = true;
                do
                {
                    index = random.Next(0, data.Count);
                } while (data[index].list.Count == 0);

                index2 = random.Next(0, data[index].list.Count);
            }
            else if (comboBox1.SelectedIndex > 0 && comboBox1.SelectedIndex < 6)
            {
                radioButton4.Enabled = true;
                List<int> ind = new List<int>();
                List<int> ind2 = new List<int>();
                int rangeStart, rangeEnd;

                if (comboBox1.SelectedIndex == 1)
                {
                    panel1.Visible = true;
                    rangeStart = (int) numericUpDown1.Value;
                    rangeEnd = (int)numericUpDown2.Value;
                }
                else
                {
                    rangeStart = (comboBox1.SelectedIndex - 2) * 50 + 1;
                    rangeEnd = rangeStart + 49;
                }

                for (int i = 0; i < data.Count; i++)
                {
                    for (int j = 0; j < data[i].list.Count; j++)
                    {
                        if (data[i].list[j].num >= rangeStart
                            && data[i].list[j].num <= rangeEnd)
                        {
                            ind.Add(i);
                            ind2.Add(j);
                        }
                    }
                }
                
                label2.Text = (rangeEnd - rangeStart - ind.Count + 1).ToString() 
                    + "/" + (rangeEnd - rangeStart + 1).ToString();
                
                if (ind.Count == 0)
                {
                    label6.Text = "Category completed.";
                    return;
                }
                int temp = random.Next(0, ind.Count);
                index = ind[temp];
                index2 = ind2[temp];
            }
            else if(comboBox1.SelectedIndex >= 6)
            {
                label2.Text = data[index].corr.ToString() + "/" + data[index].num.ToString();

                if (data[index].list.Count <= 0)
                {
                    label6.Text = "Category completed.";
                    return;
                }

                if(radioButton4.Checked)
                {
                    int check = random.Next(0, 3);
                    if (check == 0)
                        radioButton1.Checked = true;
                    else if (check == 1)
                        radioButton2.Checked = true;
                    else
                        radioButton3.Checked = true;
                }
                radioButton4.Enabled = false;

                index2 = random.Next(0, data[index].list.Count);
            }

            flag = true;

            string[] str = data[index].list[index2].descr.Split('\t');
            label7.Text = str[0];
            label8.Text = str[1];
            label9.Text = str[2];
            label10.Text = str[3];
            textBox1.Text = "";
            label6.Text = "____";

            choice2 = choice;
            if (choice == 5)
                if (comboBox1.SelectedIndex < 6)
                    choice2 = random.Next(1, 5);
                else
                    choice2 = random.Next(1, 4);
            switch (choice2)
            {
                case 1:
                    radioButton1.ForeColor = Color.Red;
                    radioButton2.ForeColor = Color.Black;
                    radioButton3.ForeColor = Color.Black;
                    radioButton4.ForeColor = Color.Black;
                    answer = label7.Text;
                    label7.Text = "____";
                    break;
                case 2:
                    radioButton1.ForeColor = Color.Black;
                    radioButton2.ForeColor = Color.Red;
                    radioButton3.ForeColor = Color.Black;
                    radioButton4.ForeColor = Color.Black;
                    answer = label8.Text;
                    label8.Text = "____";
                    break;
                case 3:
                    radioButton1.ForeColor = Color.Black;
                    radioButton2.ForeColor = Color.Black;
                    radioButton3.ForeColor = Color.Red;
                    radioButton4.ForeColor = Color.Black;
                    answer = label9.Text;
                    label9.Text = "____";
                    break;
                case 4:
                    radioButton1.ForeColor = Color.Black;
                    radioButton2.ForeColor = Color.Black;
                    radioButton3.ForeColor = Color.Black;
                    radioButton4.ForeColor = Color.Red;
                    answer = label10.Text;
                    label10.Text = "____";
                    break;
                default:
                    break;
            }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            choice = 1;
            textBox1.Focus();
            System.Windows.Forms.SendKeys.Send("{ENTER}");
            flag = false;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            choice = 2;
            textBox1.Focus();
            System.Windows.Forms.SendKeys.Send("{ENTER}");
            flag = false;
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            choice = 3;
            textBox1.Focus();
            System.Windows.Forms.SendKeys.Send("{ENTER}");
            flag = false;
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            choice = 4;
            textBox1.Focus();
            System.Windows.Forms.SendKeys.Send("{ENTER}");
            flag = false;
        }

        private void radioButton5_CheckedChanged(object sender, EventArgs e)
        {
            choice = 5;
            textBox1.Focus();
            System.Windows.Forms.SendKeys.Send("{ENTER}");
            flag = false;
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            //     if ((int)numericUpDown1.Value < 1)
            //       numericUpDown1.Value = 1;
       //     if (numericUpDown1.Value == numericUpDown2.Value)
         //       return;
            if (numericUpDown1.Value > numericUpDown2.Value)
                numericUpDown2.Value = numericUpDown1.Value;
            textBox1.Focus();
            System.Windows.Forms.SendKeys.Send("{ENTER}");
            flag = false;
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
           // if (numericUpDown1.Value == numericUpDown2.Value)
             //   return;
            if (numericUpDown2.Value < numericUpDown1.Value)// || (int)numericUpDown2.Value > 200)
                numericUpDown1.Value = numericUpDown2.Value;
            textBox1.Focus();
            System.Windows.Forms.SendKeys.Send("{ENTER}");
            flag = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Height = 600;
            button1.Enabled = false;
            button2.Enabled = true;
            textBox1.Focus();
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            string [] lines = incorrect.ToArray();

            using (System.IO.StreamWriter file =
            new System.IO.StreamWriter("incorrect.txt"))
            {
                file.WriteLine(incorrect.Count() + " incorrect answers.");
                file.WriteLine("\t");
                foreach (string line in lines)
                    file.WriteLine(line);
            }

            lines = correct.ToArray();
            using (System.IO.StreamWriter file =
            new System.IO.StreamWriter("correct.txt"))
            {
                file.WriteLine(correct.Count() + " correct answers.");
                file.WriteLine("\t");
                foreach (string line in lines)
                    file.WriteLine(line);
            }
            if(checkBox1.Checked)
                System.Diagnostics.Process.Start("correct.txt");
            if (checkBox2.Checked)
                System.Diagnostics.Process.Start("incorrect.txt"); 
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Height = 242;
            button2.Enabled = false;
            button1.Enabled = true;
            textBox1.Focus();
        }

        private void button3_Click(object sender, EventArgs e)
        {
      //      numericUpDown1.Value = 1;
      //      numericUpDown2.Value = 10;
      //      radioButton1.Select();
      //      this.Height = 242;
      //      button2.Enabled = false;
      //      button1.Enabled = true;
            incorrect = new List<string>();
            correct = new HashSet<string>();

            newCat = true;
            num = 0;
            flag = true;
     //       answer = "____";
            random = new Random();
            textBox1.Text = "";

     //       choice = 1;
       //     choice2 = 1;
         //   index = 0;
            data = new List<Drugs>();

            // Read the file and display it line by line.
            lines = System.IO.File.ReadAllLines("data.txt");
/*
            comboBox1.Items.Clear();
            comboBox1.Items.Add("All drugs");
            comboBox1.Items.Add("Drugs by number");
            comboBox1.Items.Add("Drugs 1-50");
            comboBox1.Items.Add("Drugs 51-100");
            comboBox1.Items.Add("Drugs 101-150");
            comboBox1.Items.Add("Drugs 151-200");
*/
            foreach (string line in lines)
            {
                if (line.Length != 0)
                {
                    string[] temp = line.Split('\t');
                    if (newCat)
                    {
                        Drugs x = new Drugs();
                        x.num = 0;
                        x.corr = 0;
                        x.list = new List<Drug>();
                        data.Add(x);
                        newCat = false;
      //                  comboBox1.Items.Add(temp[3]);
                    }

                    Drug y = new Drug();
                    y.num = Int32.Parse(temp[4]);
                    y.descr = line;
                    data[data.Count - 1].list.Add(y);
                    data[data.Count - 1].num++;
                }
                else
                    newCat = true;
            }

     //       comboBox1.SelectedIndex = 0;
     //       comboBox1.SelectedIndexChanged += comboBox1_SelectedIndexChanged;
    //        index = random.Next(0, data.Count);
      //      index2 = random.Next(0, data[index].list.Count);

     /*       string[] str = data[index].list[index2].descr.Split('\t');
            label7.Text = "____";
            label8.Text = str[1];
            label9.Text = str[2];
            label10.Text = str[3];
            textBox1.Text = "";
            label6.Text = "____";
            answer = str[0];
*/
            listBox1.Items.Clear();
            chooseQuestion();
            textBox1.Focus();
            System.Windows.Forms.SendKeys.Send("{ENTER}");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            AboutBox1 a = new AboutBox1();
            a.Show();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            index = comboBox1.SelectedIndex - 6;
            if (index < 0)
                index = random.Next(0, data.Count);
            index2 = random.Next(0, data[index].list.Count);
            textBox1.Focus();
            System.Windows.Forms.SendKeys.Send("{ENTER}");
            flag = false;
        }

        
    }
}
