using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace QE
{
    public partial class AdminPanel : Form
    {
        Autorization aut;

        List<string> login = new List<string>();
        List<string> password = new List<string>();

        public AdminPanel(Autorization aut)
        {
            InitializeComponent();

            this.aut = aut;

            dataGridView1.Columns.Add("login", "Логин");
            dataGridView1.Columns.Add("password", "Пароль");

            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            try
            {
                using (StreamReader reader = new StreamReader("Account.acc"))
                {
                    while (!reader.EndOfStream)
                    {
                        if (!reader.EndOfStream) login.Add(reader.ReadLine());
                        if (!reader.EndOfStream) password.Add(reader.ReadLine());
                    }
                }

                if (login.Count != password.Count)
                {
                    MessageBox.Show("Файл поврежден или изменен не верно", "Ошибка");
                }
                else
                {
                    for (int i = 0; i < login.Count; i++)
                    {
                        dataGridView1.Rows.Add(login[i].ToString(), password[i].ToString());
                    }
                }
            }
            catch
            {
                MessageBox.Show("Произошла ошибка при открытии файла Account.acc", "Ошибка");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SaveDataTable();
        }

        private void SaveDataTable()
        {
            login.Clear();
            password.Clear();

            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                if (dataGridView1.Rows[i].Cells[0].Value.ToString() != String.Empty)
                {
                    login.Add(dataGridView1.Rows[i].Cells[0].Value.ToString());
                }
                else
                {
                    MessageBox.Show("Строка номер " + (i + 1) + " имеет пустой логин", "Ошибка");
                }

                if (dataGridView1.Rows[i].Cells[1].Value.ToString() != String.Empty)
                {
                    password.Add(dataGridView1.Rows[i].Cells[1].Value.ToString());
                }
                else
                {
                    MessageBox.Show("Строка номер " + (i + 1) + " имеет пустой логин", "Ошибка");
                }
            }

            using (StreamWriter writer = new StreamWriter("Account.acc"))
            {
                for (int i = 0; i < login.Count; i++)
                {
                    writer.WriteLine(login[i]);
                    writer.WriteLine(password[i]);
                }
            }
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (int)Keys.Back) return;
            else if (!Char.IsNumber(e.KeyChar) && !Char.IsLetter(e.KeyChar)) e.KeyChar = '\0';
        }

        private void AdminPanel_FormClosed(object sender, FormClosedEventArgs e)
        {
            aut.Show();
        }

        private void dataGridView1_RowStateChanged(object sender, DataGridViewRowStateChangedEventArgs e)
        {
            e.Row.HeaderCell.Value = (e.Row.Index + 1).ToString();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if ((int)numericUpDown1.Value <= dataGridView1.Rows.Count)
            {
                textBox1.Text = dataGridView1.Rows[(int)numericUpDown1.Value - 1].Cells[0].Value.ToString();
                textBox2.Text = dataGridView1.Rows[(int)numericUpDown1.Value - 1].Cells[1].Value.ToString();
            }
            else
            {
                MessageBox.Show("Такой строки нет", "Ошибка");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Length > 0 && textBox2.Text.Length > 0)
            {
                if (textBox1.Text != "Admin")
                {
                    for (int i = 0; i < dataGridView1.Rows.Count; i++)
                    {
                        if (dataGridView1.Rows[i].Cells[0].Value.ToString() == textBox1.Text)
                        {
                            using (StreamWriter writer = new StreamWriter("ProgramLog.prog", File.Exists("ProgramLog.prog")))
                            {
                                writer.WriteLine("An attempt was made to add an account with a login - " + textBox1.Text + ", and password - " + textBox2.Text + ", at " + DateTime.Now.ToString());
                                writer.WriteLine("Error access = Such an account already exists");
                            }

                            MessageBox.Show("Такой логин уже есть", "Ошибка");
                            return;
                        }
                    }
                }
                else
                {
                    using (StreamWriter writer = new StreamWriter("ProgramLog.prog", File.Exists("ProgramLog.prog")))
                    {
                        writer.WriteLine("An attempt was made to add an account with a login - " + textBox1.Text + ", and password - " + textBox2.Text + ", at " + DateTime.Now.ToString());
                        writer.WriteLine("Error access = Cannot create an account with the login Admin");
                    }

                    MessageBox.Show("Нельзя вводить поле Admin", "Ошибка");
                    return;
                }

                dataGridView1.Rows.Add(textBox1.Text, textBox2.Text);
            }
            else MessageBox.Show("Не все поля заполнены", "Ошибка");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Length > 0 && textBox2.Text.Length > 0)
            {
                if (textBox1.Text != "Admin")
                {
                    if ((int)numericUpDown1.Value <= dataGridView1.Rows.Count)
                    {
                        for (int i = 0; i < dataGridView1.Rows.Count; i++)
                        {
                            if (dataGridView1.Rows[i].Cells[0].Value.ToString() == textBox1.Text && (int)numericUpDown1.Value != (i + 1))
                            {
                                using (StreamWriter writer = new StreamWriter("ProgramLog.prog", File.Exists("ProgramLog.prog")))
                                {
                                    writer.WriteLine("An attempt was made to change the account data with a login - " + dataGridView1.Rows[(int)numericUpDown1.Value - 1].Cells[0].Value + ", and password - " + dataGridView1.Rows[(int)numericUpDown1.Value - 1].Cells[1].Value + 
                                        ", to login - " + textBox1.Text + ", and password - " + textBox2.Text + " at " + DateTime.Now.ToString());
                                    writer.WriteLine("Error access = An account with this username has already been created");
                                }

                                MessageBox.Show("Такой логин уже есть", "Ошибка");
                                return;
                            }
                        }

                        dataGridView1.Rows[(int)numericUpDown1.Value - 1].Cells[0].Value = textBox1.Text;
                        dataGridView1.Rows[(int)numericUpDown1.Value - 1].Cells[1].Value = textBox2.Text;

                        using (StreamWriter writer = new StreamWriter("ProgramLog.prog", File.Exists("ProgramLog.prog")))
                        {
                            writer.WriteLine("The account was changed from login - " + dataGridView1.Rows[(int)numericUpDown1.Value - 1].Cells[0].Value + ", and password - " + dataGridView1.Rows[(int)numericUpDown1.Value - 1].Cells[1].Value +
                                ", to login - " + textBox1.Text + ", and password - " + textBox2.Text + " at " + DateTime.Now.ToString());
                        }
                    }
                    else MessageBox.Show("Такой строки нет", "Ошибка");
                }
                else MessageBox.Show("Нельзя вводить поле Admin", "Ошибка");
            }
            else MessageBox.Show("Не все поля заполнены", "Ошибка");
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if ((int)numericUpDown1.Value <= dataGridView1.Rows.Count) dataGridView1.Rows.RemoveAt((int)numericUpDown1.Value - 1);
            else MessageBox.Show("Такой строки нет", "Ошибка");
        }

        private void AdminPanel_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Сохранить таблицу с паролями?", "Exit", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                SaveDataTable();
            }
        }
    }
}
