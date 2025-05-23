    using EmployeeSchedulePlanner.Data;
    using EmployeeSchedulePlanner.Models;
    using EmployeeSchedulePlanner.Utils;
    using Guna.UI2.WinForms;
    using System;
    using System.Data;
    using System.Drawing;
    using System.Linq;
    using System.Windows.Forms;

    namespace EmployeeSchedulePlanner.Forms
    {
        public partial class EmployeeForm : Form
        {
            private Guna2TextBox txtName, txtSurname, txtUsername, txtPassword, txtEmail;
            private Guna2ComboBox cmbRole;
            private Guna2DataGridView dgvEmployees;
            private Guna2Button btnAdd, btnEdit, btnDelete;
            private int? selectedUserId = null;
            private TableLayoutPanel layout;

            public EmployeeForm()
            {
                InitializeComponent();
                Load += (s, e) => LoadUsersFromDatabase();
            }

            private void InitializeComponent()
            {
                this.Text = "Darbinieku pārvaldība";
                this.Size = new Size(1000, 600);
                this.BackColor = Color.FromArgb(30, 30, 30);
                this.MinimumSize = new Size(800, 500);
                this.FormBorderStyle = FormBorderStyle.Sizable;

                layout = new TableLayoutPanel
                {
                    Dock = DockStyle.Fill,
                    ColumnCount = 2,
                    RowCount = 2,
                    BackColor = Color.FromArgb(30, 30, 30),
                    Padding = new Padding(10)
                };
                layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40));
                layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 60));
                layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 300));
                layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));

                InitializeFormFields();
                InitializeDataGrid();

                this.Controls.Add(layout);
            }

            private void InitializeFormFields()
            {
                var panel = new FlowLayoutPanel
                {
                    Dock = DockStyle.Fill,
                    AutoScroll = true,
                    FlowDirection = FlowDirection.TopDown,
                    WrapContents = false
                };

                txtName = CreateTextBox("Vārds");
                txtSurname = CreateTextBox("Uzvārds");
                txtUsername = CreateTextBox("Lietotājvārds"); txtUsername.ReadOnly = true;
                txtPassword = CreateTextBox("Parole"); txtPassword.UseSystemPasswordChar = true;
                txtEmail = CreateTextBox("E-pasts");

                cmbRole = new Guna2ComboBox
                {
                    Size = new Size(300, 40),
                    BorderRadius = 8,
                    DropDownStyle = ComboBoxStyle.DropDownList,
                    DrawMode = DrawMode.OwnerDrawFixed,
                    FillColor = Color.FromArgb(45, 45, 45),
                    ForeColor = Color.White,
                    Font = new Font("Segoe UI", 10),
                    BackColor = Color.Transparent
                };
                cmbRole.Items.AddRange(new object[]
                {
                    "Projektu vadītājs",
                    "Programmētājs",
                    "Sistēmas administrators",
                    "Analītiķis"
                });

                btnAdd = CreateButton("Pievienot", Color.FromArgb(0, 120, 215), BtnAdd_Click);
                btnEdit = CreateButton("Rediģēt", Color.FromArgb(255, 140, 0), BtnEdit_Click);
                btnDelete = CreateButton("Dzēst", Color.FromArgb(200, 40, 40), BtnDelete_Click);

                panel.Controls.AddRange(new Control[] {
                    txtName, txtSurname, txtUsername, txtPassword, txtEmail, cmbRole,
                    btnAdd, btnEdit, btnDelete
                });

                txtSurname.Leave += (s, e) =>
                {
                    string name = txtName.Text.Trim().ToLower();
                    string surname = txtSurname.Text.Trim().ToLower();
                    if (!string.IsNullOrWhiteSpace(name) && !string.IsNullOrWhiteSpace(surname))
                        txtUsername.Text = $"{name}.{surname}";
                };

                layout.Controls.Add(panel, 0, 0);
                layout.SetRowSpan(panel, 2);
            }

            private void InitializeDataGrid()
            {
                dgvEmployees = new Guna2DataGridView
                {
                    Dock = DockStyle.Fill,
                    BackgroundColor = Color.FromArgb(30, 30, 30),
                    AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                    ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
                    {
                        BackColor = Color.FromArgb(0, 120, 215),
                        ForeColor = Color.White,
                        Font = new Font("Segoe UI", 10, FontStyle.Bold)
                    },
                    EnableHeadersVisualStyles = false,
                    GridColor = Color.Gray,
                    AllowUserToAddRows = false,
                    ReadOnly = true
                };

                dgvEmployees.Columns.Add("Name", "Vārds");
                dgvEmployees.Columns.Add("Surname", "Uzvārds");
                dgvEmployees.Columns.Add("Username", "Lietotājvārds");
                dgvEmployees.Columns.Add("Email", "E-pasts");
                dgvEmployees.Columns.Add("Role", "Amats");

                dgvEmployees.CellClick += DgvEmployees_CellClick;

                layout.Controls.Add(dgvEmployees, 1, 1);
            }

            private Guna2TextBox CreateTextBox(string placeholder)
            {
                return new Guna2TextBox
                {
                    PlaceholderText = placeholder,
                    Size = new Size(300, 40),
                    BorderRadius = 8,
                    FillColor = Color.FromArgb(45, 45, 45),
                    ForeColor = Color.White,
                    PlaceholderForeColor = Color.Gray,
                    Font = new Font("Segoe UI", 10),
                    Margin = new Padding(5)
                };
            }

            private Guna2Button CreateButton(string text, Color color, EventHandler onClick)
            {
                var button = new Guna2Button
                {
                    Text = text,
                    Size = new Size(300, 40),
                    FillColor = color,
                    ForeColor = Color.White,
                    BorderRadius = 8,
                    Font = new Font("Segoe UI", 10),
                    Margin = new Padding(5)
                };
                button.Click += onClick;
                return button;
            }

            private void LoadUsersFromDatabase()
            {
                using var context = new ScheduleDbContext();
                var users = context.Users.ToList();

                dgvEmployees.Rows.Clear();
                foreach (var user in users)
                {
                    dgvEmployees.Rows.Add(user.Name, user.Surname, user.Username, user.Email, user.Role);
                }
            }

            private void BtnAdd_Click(object sender, EventArgs e)
            {
                using var context = new ScheduleDbContext();
                if (string.IsNullOrWhiteSpace(txtName.Text) || string.IsNullOrWhiteSpace(txtSurname.Text) ||
                    string.IsNullOrWhiteSpace(txtUsername.Text) || string.IsNullOrWhiteSpace(txtPassword.Text) ||
                    string.IsNullOrWhiteSpace(txtEmail.Text) || cmbRole.SelectedItem == null)
                {
                    MessageBox.Show("Lūdzu aizpildiet visus laukus.", "Kļūda", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var hashed = PasswordHasher.Hash(txtPassword.Text);
                context.Users.Add(new User
                {
                    Name = txtName.Text.Trim(),
                    Surname = txtSurname.Text.Trim(),
                    Username = txtUsername.Text.Trim(),
                    Email = txtEmail.Text.Trim(),
                    Role = cmbRole.SelectedItem.ToString(),
                    PasswordHash = hashed
                });
                context.SaveChanges();
                LoadUsersFromDatabase();
                ClearFields();
            }

            private void BtnEdit_Click(object sender, EventArgs e)
            {
                if (selectedUserId == null)
                {
                    MessageBox.Show("Izvēlies lietotāju rediģēšanai.");
                    return;
                }

                using var context = new ScheduleDbContext();
                var user = context.Users.FirstOrDefault(u => u.Id == selectedUserId);
                if (user != null)
                {
                    user.Name = txtName.Text.Trim();
                    user.Surname = txtSurname.Text.Trim();
                    user.Email = txtEmail.Text.Trim();
                    user.Role = cmbRole.SelectedItem.ToString();
                    if (!string.IsNullOrWhiteSpace(txtPassword.Text))
                        user.PasswordHash = PasswordHasher.Hash(txtPassword.Text);
                    context.SaveChanges();
                    LoadUsersFromDatabase();
                    ClearFields();
                }
            }

            private void BtnDelete_Click(object sender, EventArgs e)
            {
                if (selectedUserId == null)
                {
                    MessageBox.Show("Izvēlies lietotāju dzēšanai.");
                    return;
                }

                var confirm = MessageBox.Show("Vai tiešām dzēst šo lietotāju?", "Apstiprināt", MessageBoxButtons.YesNo);
                if (confirm != DialogResult.Yes) return;

                using var context = new ScheduleDbContext();
                var user = context.Users.FirstOrDefault(u => u.Id == selectedUserId);
                if (user != null)
                {
                    context.Users.Remove(user);
                    context.SaveChanges();
                    LoadUsersFromDatabase();
                    ClearFields();
                }
            }

            private void DgvEmployees_CellClick(object sender, DataGridViewCellEventArgs e)
            {
                if (e.RowIndex >= 0)
                {
                    var row = dgvEmployees.Rows[e.RowIndex];
                    txtName.Text = row.Cells[0].Value?.ToString();
                    txtSurname.Text = row.Cells[1].Value?.ToString();
                    txtUsername.Text = row.Cells[2].Value?.ToString();
                    txtEmail.Text = row.Cells[3].Value?.ToString();
                    cmbRole.SelectedItem = row.Cells[4].Value?.ToString();

                    using var context = new ScheduleDbContext();
                    var user = context.Users.FirstOrDefault(u => u.Username == txtUsername.Text);
                    if (user != null)
                        selectedUserId = user.Id;
                }
            }

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
