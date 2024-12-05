﻿using System.Data.SqlClient;
using System.Text;

namespace RCinema_db
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
            CreateDatabaseIfNotExist();
            CustomizeDesign();
        }

        private void CreateDatabaseIfNotExist()
        {
            DatabaseCreateClass db = new DatabaseCreateClass();
            db.CreateDatabase();
        }

        private void CustomizeDesign()
        {
            this.Text = "Pixel Login";
            this.ClientSize = new Size(600, 400);
            this.FormBorderStyle = FormBorderStyle.Sizable;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.Black;

            Label titleLabel = new Label
            {
                Text = "LOGIN",
                Font = new Font("Courier New", 32, FontStyle.Bold),
                ForeColor = Color.Lime,
                AutoSize = true,
                Location = new Point(this.ClientSize.Width / 2 - 80, 30),
                TextAlign = ContentAlignment.MiddleCenter,
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            };
            this.Controls.Add(titleLabel);

            Label usernameLabel = new Label
            {
                Text = "USERNAME:",
                Font = new Font("Courier New", 14, FontStyle.Bold),
                ForeColor = Color.Lime,
                AutoSize = true,
                Location = new Point(50, 100)
            };
            this.Controls.Add(usernameLabel);

            TextBox usernameTextBox = new TextBox
            {
                Font = new Font("Courier New", 14, FontStyle.Bold),
                BackColor = Color.Black,
                ForeColor = Color.Lime,
                BorderStyle = BorderStyle.FixedSingle,
                Location = new Point(50, 130),
                Width = this.ClientSize.Width - 100,
                Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top
            };
            this.Controls.Add(usernameTextBox);

            Label passwordLabel = new Label
            {
                Text = "PASSWORD:",
                Font = new Font("Courier New", 14, FontStyle.Bold),
                ForeColor = Color.Lime,
                AutoSize = true,
                Location = new Point(50, 180)
            };
            this.Controls.Add(passwordLabel);

            TextBox passwordTextBox = new TextBox
            {
                Font = new Font("Courier New", 14, FontStyle.Bold),
                BackColor = Color.Black,
                ForeColor = Color.Lime,
                BorderStyle = BorderStyle.FixedSingle,
                PasswordChar = '*',
                Location = new Point(50, 210),
                Width = this.ClientSize.Width - 100,
                Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top
            };
            this.Controls.Add(passwordTextBox);

            Button decorativeLine = new Button
            {
                Text = "Create account",
                Font = new Font("Courier New", 12),
                ForeColor = Color.Lime,
                AutoSize = true,
                Location = new Point(50, 260),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            };
            decorativeLine.Click += (sender, e) =>
            {
                CreateAccountForm createAccountForm = new CreateAccountForm();
                createAccountForm.ShowDialog();
            };
            this.Controls.Add(decorativeLine);

            Button loginButton = new Button
            {
                Text = "LOGIN",
                Font = new Font("Courier New", 14, FontStyle.Bold),
                BackColor = Color.Lime,
                ForeColor = Color.Black,
                FlatStyle = FlatStyle.Flat,
                Location = new Point(50, 300),
                Width = 200,
                Height = 50,
                Anchor = AnchorStyles.Left | AnchorStyles.Bottom
            };
            loginButton.FlatAppearance.BorderSize = 1;
            loginButton.Click += (sender, e) =>
            {
                string username = usernameTextBox.Text.Trim();
                string password = passwordTextBox.Text.Trim();

                if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                {
                    MessageBox.Show("Please enter your username and password.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                try
                {
                    using (SqlConnection conn = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\ripmy\source\repos\RCinema-db\RCinema-db\Database.mdf;Integrated Security=True"))
                    {
                        conn.Open();

                        // Заменен параметр на Password вместо PasswordHash
                        string query = "SELECT Role FROM Users WHERE Username = @Username AND Password = @Password";
                        using (SqlCommand cmd = new SqlCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@Username", username);
                            cmd.Parameters.AddWithValue("@Password", password);

                            object result = cmd.ExecuteScalar();

                            if (result != null)
                            {
                                string role = result.ToString();
                                if (role == "Admin")
                                {
                                    MessageBox.Show("Welcome, Admin!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    AdminForm adminForm = new AdminForm();
                                    adminForm.Show();
                                }
                                else
                                {
                                    MessageBox.Show("Welcome, User!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    MainForm mainForm = new MainForm();
                                    mainForm.Show();
                                }
                            }
                            else
                            {
                                MessageBox.Show("Invalid username or password.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error logging in: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };

            this.Controls.Add(loginButton);

            Button cancelButton = new Button
            {
                Text = "CANCEL",
                Font = new Font("Courier New", 14, FontStyle.Bold),
                BackColor = Color.Red,
                ForeColor = Color.Black,
                FlatStyle = FlatStyle.Flat,
                Location = new Point(this.ClientSize.Width - 250, 300),
                Width = 200,
                Height = 50,
                Anchor = AnchorStyles.Right | AnchorStyles.Bottom
            };
            cancelButton.FlatAppearance.BorderSize = 1;
            cancelButton.Click += (sender, e) =>
            {
                this.Close();
            };
            this.Controls.Add(cancelButton);

            Panel gridPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackgroundImage = GeneratePixelGrid(this.ClientSize.Width, this.ClientSize.Height),
                BackgroundImageLayout = ImageLayout.Tile
            };
            this.Controls.Add(gridPanel);
            gridPanel.SendToBack();
        }

        private Bitmap GeneratePixelGrid(int width, int height)
        {
            Bitmap bmp = new Bitmap(width, height);
            Random random = new Random();
            Color[] palette = { Color.Black, Color.Gray, Color.Yellow, Color.Orange };

            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.Clear(Color.Black);

                int pixelSize = 10;
                for (int x = 0; x < width; x += pixelSize)
                {
                    for (int y = 0; y < height; y += pixelSize)
                    {
                        Color randomColor = palette[random.Next(palette.Length)];
                        using (Brush brush = new SolidBrush(randomColor))
                        {
                            g.FillRectangle(brush, x, y, pixelSize, pixelSize);
                        }
                    }
                }
            }
            return bmp;
        }

        public void CreateUser(string username, string password, string role)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\ripmy\source\repos\RCinema-db\RCinema-db\Database.mdf;Integrated Security=True"))
                {
                    conn.Open();

                    string insertQuery = "INSERT INTO Users (Username, Password, Role) VALUES (@Username, @Password, @Role)";
                    using (SqlCommand cmd = new SqlCommand(insertQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@Username", username);
                        cmd.Parameters.AddWithValue("@Password", password); 
                        cmd.Parameters.AddWithValue("@Role", role);

                        cmd.ExecuteNonQuery();
                        MessageBox.Show($"Пользователь {username} создан!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при создании пользователя: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}