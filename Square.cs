using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Windows.Forms;

namespace QE
{
    public partial class Square : Form
    {
        Autorization aut;
        string text = "";
        string name;

        public Square(Autorization aut, string name)
        {
            InitializeComponent();
            this.aut = aut;
            this.name = name;

            using (StreamWriter writer = new StreamWriter("Log_" + name + ".txt", File.Exists("Log_" + name + ".txt")))
            {
                writer.WriteLine("User " + name + " logged in, at " + DateTime.Now.ToString());
                writer.WriteLine();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Length > 0 && textBox2.Text.Length > 0 && textBox3.Text.Length > 0)
            {
                textBox4.Text = " ";
                textBox5.Text = " ";

                try
                {
                    double a = double.Parse(textBox1.Text);
                    if (a == 0)
                    {
                        MessageBox.Show("a = 0, при вычислении будет деление на 0", "Ошибка");
                        return;
                    }
                    double b = double.Parse(textBox2.Text);
                    double c = double.Parse(textBox3.Text);

                    text = "a=" + a + ", b=" + b + ", c=" + c + '\n';

                    double D = Math.Pow(b, 2) - (4 * a * c);
                    label8.Text = D.ToString();
                    if (D == 0)
                    {
                        double x1 = -b / (2 * a);
                        x1 = Math.Round(x1, 2);
                        textBox4.Text = x1.ToString();
                        label11.Text = "Действительные";
                        text += "Тип корней: Действительные\n" + "Корни уравнения: x1 = " + x1 + "\n\n";
                    }
                    else if (D > 0)
                    {
                        double x1 = (-b + Math.Sqrt(D)) / (2 * a);
                        double x2 = (-b - Math.Sqrt(D)) / (2 * a);
                        x1 = Math.Round(x1, 2);
                        x2 = Math.Round(x2, 2);
                        textBox4.Text = x1.ToString();
                        textBox5.Text = x2.ToString();
                        label11.Text = "Действительные";
                        text += "Тип корней: Действительные\n" + "Корни уравнения: x1 = " + x1 + ", x2 = " + x2 + "\n\n";
                    }
                    else if (D < 0)
                    {
                        Complex x1 = new Complex(0, Math.Sqrt(Math.Abs(D)));
                        Complex x2 = new Complex(0, -1 * Math.Sqrt(Math.Abs(D)));
                        x1 = (x1 - b) / (2 * a);
                        x2 = (x2 - b) / (2 * a);
                        textBox4.Text = x1.Real.ToString() + "+i*" + x1.Imaginary.ToString();
                        textBox5.Text = x2.Real.ToString() + "+i*" + x2.Imaginary.ToString();
                        label11.Text = "Комплексные";
                        text += "Тип корней: Комплексные\n" + "Корни уравнения: x1 = " + x1.Real.ToString() + "+i*" + x1.Imaginary.ToString() +
                            ", x2 = " + x2.Real.ToString() + "+i*" + x2.Imaginary.ToString() + "\n\n";
                    }

                    using (StreamWriter writer = new StreamWriter("Log_" + name + ".txt", File.Exists("Log_" + name + ".txt")))
                    {
                        writer.WriteLine(text);
                    }
                }
                catch
                {
                    MessageBox.Show("Ошибка ввода данных", "Ошибка");
                    return;
                }
            }
            else
            {
                MessageBox.Show("Не все поля заполнены", "Ошибка");
                return;
            }
        }

        private void Square_FormClosed(object sender, FormClosedEventArgs e)
        {
            aut.Close();
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == ',')
            {
                if ((textBox1.Text.Contains('-') && textBox1.Text.Length == 1) || textBox1.Text.Contains(',') || textBox1.Text.Length == 0) e.KeyChar = '\0';
                else return;
            }
            else if (e.KeyChar == '-')
            {
                if (textBox1.Text.Length == 0) return;
                else e.KeyChar = '\0';
            }
            else if (e.KeyChar == (int)Keys.Back && textBox1.Text.Length != 0) return;
            else if (!Char.IsNumber(e.KeyChar)) e.KeyChar = '\0';
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == ',')
            {
                if ((textBox2.Text.Contains('-') && textBox2.Text.Length == 1) || textBox2.Text.Contains(',') || textBox2.Text.Length == 0) e.KeyChar = '\0';
                else return;
            }
            else if (e.KeyChar == '-')
            {
                if (textBox2.Text.Length == 0) return;
                else e.KeyChar = '\0';
            }
            else if (e.KeyChar == (int)Keys.Back && textBox2.Text.Length != 0) return;
            else if (!Char.IsNumber(e.KeyChar)) e.KeyChar = '\0';
        }

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == ',')
            {
                if ((textBox3.Text.Contains('-') && textBox3.Text.Length == 1) || textBox3.Text.Contains(',') || textBox3.Text.Length == 0) e.KeyChar = '\0';
                else return;
            }
            else if (e.KeyChar == '-')
            {
                if (textBox3.Text.Length == 0) return;
                else e.KeyChar = '\0';
            }
            else if (e.KeyChar == (int)Keys.Back && textBox3.Text.Length != 0) return;
            else if (!Char.IsNumber(e.KeyChar)) e.KeyChar = '\0';
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            if (File.Exists("Справка.ref")) Process.Start("Справка.ref");
            else
            {
                MessageBox.Show("Файл со справкой отсутствует", "Ошибка");
                return;
            }
        }
    }
}
