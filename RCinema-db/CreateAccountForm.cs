﻿using System;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace RCinema_db
{
    public partial class CreateAccountForm : Form
    {
        public CreateAccountForm()
        {
            CustomizeDesign();
        }

        private void CustomizeDesign()
        {
            this.Text = "Create Account";
            this.ClientSize = new Size(600, 400);
            this.BackColor = Color.Black;

            Label titleLabel = new Label
            {
                Text = "CREATE ACCOUNT",
                Font = new Font("Courier New", 24, FontStyle.Bold),
                ForeColor = Color.Lime,
                AutoSize = true,
                Location = new Point(200, 30)
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
                Font = new Font("Courier New", 14),
                BackColor = Color.Black,
                ForeColor = Color.Lime,
                BorderStyle = BorderStyle.FixedSingle,
                Location = new Point(50, 130),
                Width = 500
            };
            this.Controls.Add(usernameTextBox);

            Label emailLabel = new Label
            {
                Text = "EMAIL:",
                Font = new Font("Courier New", 14, FontStyle.Bold),
                ForeColor = Color.Lime,
                AutoSize = true,
                Location = new Point(50, 180)
            };
            this.Controls.Add(emailLabel);

            TextBox emailTextBox = new TextBox
            {
                Font = new Font("Courier New", 14),
                BackColor = Color.Black,
                ForeColor = Color.Lime,
                BorderStyle = BorderStyle.FixedSingle,
                Location = new Point(50, 210),
                Width = 500
            };
            this.Controls.Add(emailTextBox);

            Label passwordLabel = new Label
            {
                Text = "PASSWORD:",
                Font = new Font("Courier New", 14, FontStyle.Bold),
                ForeColor = Color.Lime,
                AutoSize = true,
                Location = new Point(50, 260)
            };
            this.Controls.Add(passwordLabel);

            TextBox passwordTextBox = new TextBox
            {
                Font = new Font("Courier New", 14),
                BackColor = Color.Black,
                ForeColor = Color.Lime,
                BorderStyle = BorderStyle.FixedSingle,
                PasswordChar = '*',
                Location = new Point(50, 290),
                Width = 500
            };
            this.Controls.Add(passwordTextBox);

            Button createButton = new Button
            {
                Text = "CREATE ACCOUNT",
                Font = new Font("Courier New", 14),
                BackColor = Color.Lime,
                ForeColor = Color.Black,
                FlatStyle = FlatStyle.Flat,
                Location = new Point(50, 340),
                Width = 200
            };
            createButton.Click += (sender, e) =>
            {
                string username = usernameTextBox.Text.Trim();
                string email = emailTextBox.Text.Trim();
                string password = passwordTextBox.Text.Trim();

                if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
                {
                    MessageBox.Show("All fields must be filled.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                try
                {
                    using (SqlConnection conn = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\ripmy\source\repos\RCinema-db\RCinema-db\Database.mdf;Integrated Security=True"))
                    {
                        conn.Open();

                        string query = "INSERT INTO Users (Username, Email, PasswordHash, Role) VALUES (@Username, @Email, @PasswordHash, @Role)";
                        using (SqlCommand cmd = new SqlCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@Username", username);
                            cmd.Parameters.AddWithValue("@Email", email);
                            cmd.Parameters.AddWithValue("@PasswordHash", password); 
                            cmd.Parameters.AddWithValue("@Role", "User");

                            cmd.ExecuteNonQuery();
                        }
                    }

                    MessageBox.Show("Account created successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error saving account: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };
            this.Controls.Add(createButton);
        }
    }
}