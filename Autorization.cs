using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace QE
{
    public partial class Autorization : Form
    {
        Square square;
        Registration registration;
        AdminPanel admin;

        List<string> login = new List<string>();
        List<string> password = new List<string>();

        public Autorization()
        {
            InitializeComponent();

            using (StreamWriter writer = new StreamWriter("ProgramLog.prog", File.Exists("ProgramLog.prog")))
            {
                writer.WriteLine("App launch, at " + DateTime.Now.ToString());
            }
        }

        private void Autorization_Shown(object sender, EventArgs e)
        {
            if (!ReaderAccount()) this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "Admin" && textBox2.Text == "admin")
            {
                admin = new AdminPanel(this);
                admin.Show();
                this.Hide();

                using (StreamWriter writer = new StreamWriter("ProgramLog.prog", File.Exists("ProgramLog.prog")))
                {
                    writer.WriteLine("\nAdmin logged in \'Admin panel\', at " + DateTime.Now.ToString());
                }

                return;
            }

            if (!ReaderAccount()) this.Hide();
            else
            {
                for (int i = 0; i < login.Count; i++)
                {
                    if (login[i] == textBox1.Text)
                    {
                        if (password[i] == textBox2.Text)
                        {
                            square = new Square(this, textBox1.Text);
                            square.Show();
                            this.Hide();

                            using (StreamWriter writer = new StreamWriter("ProgramLog.prog", File.Exists("ProgramLog.prog")))
                            {
                                writer.WriteLine("User " + textBox1.Text + " logged in, at " + DateTime.Now.ToString());
                            }

                            return;
                        }
                        else
                        {
                            using (StreamWriter writer = new StreamWriter("ProgramLog.prog", File.Exists("ProgramLog.prog")))
                            {
                                writer.WriteLine("An attempt was made to login to the account, with login " + textBox1.Text + ", at " + DateTime.Now.ToString());
                                writer.WriteLine("Error access = Wrong password");
                            }

                            MessageBox.Show("Неправильный аккаунт или пароль", "Ошибка");
                            return;
                        }
                    }
                }

                using (StreamWriter writer = new StreamWriter("ProgramLog.prog", File.Exists("ProgramLog.prog")))
                {
                    writer.WriteLine("An attempt was made to login to the account, with login - " + textBox1.Text + ", at " + DateTime.Now.ToString());
                    writer.WriteLine("Error access = No current account");
                }

                MessageBox.Show("Неправильный аккаунт или пароль", "Ошибка");
                return;
            }

            textBox1.Text = "";
            textBox2.Text = "";
            textBox1.Text = "";
            textBox2.Text = "";
        }

        private bool ReaderAccount()
        {
            try
            {
                login.Clear();
                password.Clear();

                using (StreamReader reader = new StreamReader("Account.acc"))
                {
                    while (!reader.EndOfStream)
                    {
                        if (!reader.EndOfStream) login.Add(reader.ReadLine());
                        if (!reader.EndOfStream) password.Add(reader.ReadLine());
                    }
                }

                if (login.Count != 0 || password.Count != 0)
                {
                    if (login.Count != password.Count)
                    {
                        DialogResult dialogResult = MessageBox.Show("Файл с аккаунтами поврежден или изменен\nПересоздать файл и зарегистрироваться?", "Ошибка", MessageBoxButtons.YesNo);
                        if (dialogResult == DialogResult.Yes)
                        {
                            login.Clear();
                            password.Clear();

                            registration = new Registration(true, this);
                            registration.Show();
                            return false;
                        }
                        else if (dialogResult == DialogResult.No)
                        {
                            login.Clear();
                            password.Clear();

                            return true;
                        }
                    }
                    else
                    {
                        for (int i = 0; i < login.Count; i++)
                        {
                            if (login.IndexOf(login[i]) != login.LastIndexOf(login[i]))
                            {
                                DialogResult dialogResult = MessageBox.Show("В файле с аккаунтами есть одинаковые логины\nПересоздать файл и зарегистрироваться?", "Ошибка", MessageBoxButtons.YesNo);
                                if (dialogResult == DialogResult.Yes)
                                {
                                    login.Clear();
                                    password.Clear();

                                    registration = new Registration(true, this);
                                    registration.Show();
                                    return false;
                                }
                                else if (dialogResult == DialogResult.No)
                                {
                                    login.Clear();
                                    password.Clear();

                                    return true;
                                }
                            }
                        }
                        return true;
                    }
                }
                else
                {
                    DialogResult dialogResult = MessageBox.Show("Файл с аккаунтами пуст\nХотите зарегистрироваться?", "Ошибка", MessageBoxButtons.YesNo);
                    if (dialogResult == DialogResult.Yes)
                    {
                        registration = new Registration(true, this);
                        registration.Show();
                        return false;
                    }
                    else if (dialogResult == DialogResult.No) return true;
                }
            }
            catch
            {
                DialogResult dialogResult = MessageBox.Show("Файл с аккаунтами отсутствует\nХотите зарегистрироваться?", "Ошибка", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    registration = new Registration(true, this);
                    registration.Show();
                    return false;
                }
                else if (dialogResult == DialogResult.No) return true;
            }
            return true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            registration = new Registration(false, this);
            registration.Show();
            this.Hide();

            textBox1.Text = "";
            textBox2.Text = "";
            textBox1.Text = "";
            textBox2.Text = "";
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (int)Keys.Back) return;
            else if (!Char.IsNumber(e.KeyChar) && !Char.IsLetter(e.KeyChar)) e.KeyChar = '\0';
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            if (ReaderAccount())
            {
                try
                {
                    login.Clear();
                    password.Clear();

                    using (StreamReader reader = new StreamReader("Account.acc"))
                    {
                        while (!reader.EndOfStream)
                        {
                            if (!reader.EndOfStream) login.Add(reader.ReadLine());
                            if (!reader.EndOfStream) password.Add(reader.ReadLine());
                        }
                    }

                    int index = 0;

                    for (int i = 0; i < login.Count; i++)
                    {
                        if (login[i] == textBox1.Text)
                        {
                            if (password[i] == textBox2.Text)
                            {
                                index = i;
                                break;
                            }

                            using (StreamWriter writer = new StreamWriter("ProgramLog.prog", File.Exists("ProgramLog.prog")))
                            {
                                writer.WriteLine("An attempt was made to delete an account with a login - " + textBox1.Text + ", at " + DateTime.Now.ToString());
                                writer.WriteLine("Error access = Wrong password");
                            }

                            MessageBox.Show("Неверный пароль от аккаунта", "Ошибка");
                            return;
                        }

                        if (i == login.Count - 1)
                        {
                            using (StreamWriter writer = new StreamWriter("ProgramLog.prog", File.Exists("ProgramLog.prog")))
                            {
                                writer.WriteLine("An attempt was made to delete an account with a login - " + textBox1.Text + ", at " + DateTime.Now.ToString());
                                writer.WriteLine("Error access = No current account");
                            }

                            MessageBox.Show("Такого аккаунта нет", "Ошибка");
                            return;
                        }
                    }

                    DialogResult dialogResult = MessageBox.Show("Удалить аккаунт:\nlogin: " + textBox1.Text + "\npassword: " + textBox2.Text, "Ошибка", MessageBoxButtons.YesNo);
                    if (dialogResult == DialogResult.Yes)
                    {
                        try
                        {
                            login.RemoveAt(index);
                            password.RemoveAt(index);

                            using (StreamWriter writer = new StreamWriter("Account.acc"))
                            {
                                for (int i = 0; i < login.Count; i++)
                                {
                                    writer.WriteLine(login[i]);
                                    writer.WriteLine(password[i]);
                                }
                            }
                        }
                        catch
                        {
                            using (StreamWriter writer = new StreamWriter("ProgramLog.prog", File.Exists("ProgramLog.prog")))
                            {
                                writer.WriteLine("An error occurred while trying to delete the account, at " + DateTime.Now.ToString());
                            }

                            MessageBox.Show("Ошибка при удалении аккаунта", "Ошибка");
                            return;
                        }

                        using (StreamWriter writer = new StreamWriter("ProgramLog.prog", File.Exists("ProgramLog.prog")))
                        {
                            writer.WriteLine("An account with a login - " + textBox1.Text + " was deleted, at " + DateTime.Now.ToString());
                        }
                    }
                    else if (dialogResult == DialogResult.No) return;
                }
                catch
                {
                    MessageBox.Show("Ошибка при открытия файла Account.acc", "Ошибка");
                    return;
                }

                textBox1.Clear();
                textBox2.Clear();
            }
        }
    }
}
