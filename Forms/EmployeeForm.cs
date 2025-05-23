using EmployeeSchedulePlanner.Data;
using EmployeeSchedulePlanner.Models;
using EmployeeSchedulePlanner.Utils;
using Guna.UI2.WinForms;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace EmployeeSchedulePlanner
{
// Parādās user logs
    public partial class EmployeeForm : Form
    {
        Guna2TextBox txtName, txtSurname, txtUsername, txtPassword, txtEmail;
        Guna2ComboBox cmbRole;
        Guna2DataGridView dgvEmployees;
        int? selectedUserId = null;

        public EmployeeForm()
        {
            this.FormBorderStyle = FormBorderStyle.None;
            this.Size = new Size(1250, 650);
            this.BackColor = Color.FromArgb(30, 30, 30);

            InitCustomUI();
// Šeit tiek iegūts lietotāju saraksts no SQL un parādīts sarakstā
            this.Load += (s, e) => LoadUsersFromDatabase();
        }

        private void InitCustomUI()
        {
            Font font = new Font("Segoe UI", 10);
            int leftX = 50;
            int topY = 100;
            int spacingY = 70;
            int boxWidth = 320;

            var title = new Label
            {
                Text = "Darbinieku pārvaldība",
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 22, FontStyle.Bold),
                Location = new Point(leftX, 30)
            };
            this.Controls.Add(title);

            txtName = CreateTextBox("Vārds", leftX, topY, boxWidth, font); topY += spacingY;
            txtSurname = CreateTextBox("Uzvārds", leftX, topY, boxWidth, font); topY += spacingY;
            txtUsername = CreateTextBox("Lietotājvārds", leftX, topY, boxWidth, font); txtUsername.ReadOnly = true; topY += spacingY;
            txtPassword = CreateTextBox("Parole", leftX, topY, boxWidth, font); txtPassword.UseSystemPasswordChar = true; topY += spacingY;
            txtEmail = CreateTextBox("E-pasts", leftX, topY, boxWidth, font); topY += spacingY;

            cmbRole = new Guna2ComboBox
            {
                Location = new Point(leftX, topY),
                Size = new Size(boxWidth, 40),
                BorderRadius = 10,
                FillColor = Color.FromArgb(45, 45, 45),
                ForeColor = Color.White,
                Font = font,
                DrawMode = DrawMode.OwnerDrawFixed,
                DropDownStyle = ComboBoxStyle.DropDownList,
                BackColor = Color.Transparent
            };
            cmbRole.Items.AddRange(new object[]
            {
                "Projektu vadītājs",
                "Programmētājs",
                "Sistēmas administrators",
                "Analītiķis"
            });
            this.Controls.Add(cmbRole);
            topY += spacingY + 10;

            var btnAdd = new Guna2Button
            {
                Text = "Pievienot",
                Location = new Point(leftX, topY),
                Size = new Size(100, 40),
                FillColor = Color.FromArgb(0, 120, 215),
                ForeColor = Color.White,
                BorderRadius = 8
            };
            btnAdd.Click += BtnAdd_Click;
            this.Controls.Add(btnAdd);

            var btnEdit = new Guna2Button
            {
                Text = "Rediģēt",
                Location = new Point(leftX + 110, topY),
                Size = new Size(100, 40),
                FillColor = Color.FromArgb(255, 140, 0),
                ForeColor = Color.White,
                BorderRadius = 8
            };
            btnEdit.Click += BtnEdit_Click;
            this.Controls.Add(btnEdit);

            var btnDelete = new Guna2Button
            {
                Text = "Dzēst",
                Location = new Point(leftX + 220, topY),
                Size = new Size(100, 40),
                FillColor = Color.FromArgb(200, 40, 40),
                ForeColor = Color.White,
                BorderRadius = 8
            };
            btnDelete.Click += BtnDelete_Click;
            this.Controls.Add(btnDelete);

            dgvEmployees = new Guna2DataGridView
            {
                Location = new Point(420, 100),
                Size = new Size(780, 450),
                BackgroundColor = Color.FromArgb(30, 30, 30),
                AlternatingRowsDefaultCellStyle = new DataGridViewCellStyle { BackColor = Color.FromArgb(50, 50, 50), ForeColor = Color.White },
                ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
                {
                    BackColor = Color.FromArgb(0, 120, 215),
                    ForeColor = Color.White,
                    Font = new Font("Segoe UI", 10, FontStyle.Bold)
                },
                EnableHeadersVisualStyles = false,
                GridColor = Color.Gray,
                ColumnCount = 5
            };

            dgvEmployees.Columns[0].Name = "Vārds";
            dgvEmployees.Columns[1].Name = "Uzvārds";
            dgvEmployees.Columns[2].Name = "Lietotājvārds";
            dgvEmployees.Columns[3].Name = "E-pasts";
            dgvEmployees.Columns[4].Name = "Amats";

            dgvEmployees.CellClick += DgvEmployees_CellClick;

            this.Controls.Add(dgvEmployees);

// Kad tiek ievadīts uzvārds, automātiski izveido lietotājvārdu
            txtSurname.Leave += (s, e) =>
            {
                string name = txtName.Text.Trim().ToLower();
                string surname = txtSurname.Text.Trim().ToLower();
                if (!string.IsNullOrWhiteSpace(name) && !string.IsNullOrWhiteSpace(surname))
                {
                    txtUsername.Text = $"{name}.{surname}";
                }
            };
        }

        private Guna2TextBox CreateTextBox(string placeholder, int x, int y, int width, Font font)
        {
            var tb = new Guna2TextBox
            {
                PlaceholderText = placeholder,
                Location = new Point(x, y),
                Size = new Size(width, 40),
                BorderRadius = 10,
                FillColor = Color.FromArgb(45, 45, 45),
                ForeColor = Color.White,
                PlaceholderForeColor = Color.Gray,
                Font = font
            };
            this.Controls.Add(tb);
            return tb;
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            string name = txtName.Text.Trim();
            string surname = txtSurname.Text.Trim();
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text.Trim();
            string email = txtEmail.Text.Trim();
            string role = cmbRole.SelectedItem?.ToString() ?? "";

            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(surname) ||
                string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password) ||
                string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(role))
            {
                MessageBox.Show("Lūdzu, aizpildi visus laukus!", "Kļūda", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (var context = new ScheduleDbContext())
            {
// Šifrē parole pirms saglabāšanas
                var hashed = PasswordHasher.Hash(password);
                var user = new User
                {
                    Name = name,
                    Surname = surname,
                    Username = username,
                    Email = email,
                    Role = role,
                    PasswordHash = hashed
                };
// Šī koda daļa atbild par jauna lietotāja izveidi
                context.Users.Add(user);
// Saglabā izmaiņas datubāzē
                context.SaveChanges();
            }

// Šeit tiek iegūts lietotāju saraksts no SQL un parādīts sarakstā
            LoadUsersFromDatabase();
// Notīra ievades laukus formā
            ClearFields();
        }

        private void BtnEdit_Click(object sender, EventArgs e)
        {
            if (selectedUserId == null)
            {
                MessageBox.Show("Lūdzu, izvēlies lietotāju rediģēšanai.");
                return;
            }

            using (var context = new ScheduleDbContext())
            {
// Šeit tiek atlasīts konkrēts lietotājs datubāzē
                var user = context.Users.FirstOrDefault(u => u.Id == selectedUserId);
                if (user != null)
                {
                    user.Name = txtName.Text.Trim();
                    user.Surname = txtSurname.Text.Trim();
                    user.Email = txtEmail.Text.Trim();
                    user.Role = cmbRole.SelectedItem?.ToString() ?? user.Role;

                    if (!string.IsNullOrWhiteSpace(txtPassword.Text))
                    {
// Šifrē parole pirms saglabāšanas
                        user.PasswordHash = PasswordHasher.Hash(txtPassword.Text);
                    }

// Saglabā izmaiņas datubāzē
                    context.SaveChanges();
                }
            }

// Šeit tiek iegūts lietotāju saraksts no SQL un parādīts sarakstā
            LoadUsersFromDatabase();
// Notīra ievades laukus formā
            ClearFields();
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (selectedUserId == null)
            {
                MessageBox.Show("Lūdzu, izvēlies lietotāju dzēšanai.");
                return;
            }

            var confirmResult = MessageBox.Show("Vai tiešām vēlies dzēst šo lietotāju?", "Apstiprināt dzēšanu", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (confirmResult != DialogResult.Yes)
                return;

            using (var context = new ScheduleDbContext())
            {
// Šeit tiek atlasīts konkrēts lietotājs datubāzē
                var user = context.Users.FirstOrDefault(u => u.Id == selectedUserId);
                if (user != null)
                {
// Šī koda daļa atbild par lietotāja dzēšanu
                    context.Users.Remove(user);
// Saglabā izmaiņas datubāzē
                    context.SaveChanges();
                }
            }

// Šeit tiek iegūts lietotāju saraksts no SQL un parādīts sarakstā
            LoadUsersFromDatabase();
// Notīra ievades laukus formā
            ClearFields();
        }

        private void DgvEmployees_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < dgvEmployees.Rows.Count)
            {
                var row = dgvEmployees.Rows[e.RowIndex];
                txtName.Text = row.Cells[0].Value?.ToString();
                txtSurname.Text = row.Cells[1].Value?.ToString();
                txtUsername.Text = row.Cells[2].Value?.ToString();
                txtEmail.Text = row.Cells[3].Value?.ToString();
                cmbRole.SelectedItem = row.Cells[4].Value?.ToString();

                using (var context = new ScheduleDbContext())
                {
                    var username = txtUsername.Text;
// Šeit tiek atlasīts konkrēts lietotājs datubāzē
                    var user = context.Users.FirstOrDefault(u => u.Username == username);
                    if (user != null)
                    {
                        selectedUserId = user.Id;
                    }
                }
            }
        }

// Šeit tiek iegūts lietotāju saraksts no SQL un parādīts sarakstā
        private void LoadUsersFromDatabase()
        {
            using (var context = new ScheduleDbContext())
            {
                var users = context.Users.ToList();
                dgvEmployees.Rows.Clear();
// Cikls, kas iet cauri datu sarakstam un izpilda darbības
                foreach (var user in users)
                {
                    dgvEmployees.Rows.Add(user.Name, user.Surname, user.Username, user.Email, user.Role);
                }
            }
        }

// Notīra ievades laukus formā
        private void ClearFields()
        {
            txtName.Clear();
            txtSurname.Clear();
            txtUsername.Clear();
            txtPassword.Clear();
            txtEmail.Clear();
            cmbRole.SelectedIndex = -1;
            selectedUserId = null;
        }
    }
}