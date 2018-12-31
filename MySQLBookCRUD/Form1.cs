using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace MySQLBookCRUD
{
    public partial class Form1 : Form
    {
        string connectionString = @"Server=127.0.0.1;Database=bookdb;Uid=root;Pwd=123qwe;";
        int bookID;

        public Form1()
        {
            InitializeComponent();
        }

        private void butSave_Click(object sender, EventArgs e)
        {
            using (MySqlConnection mysqlCon = new MySqlConnection(connectionString))
            {
                mysqlCon.Open();
                MySqlCommand mySqlCmd = new MySqlCommand("BookAddOrEdit",mysqlCon);
                mySqlCmd.CommandType = CommandType.StoredProcedure;
                mySqlCmd.Parameters.AddWithValue("_BookID",bookID);
                mySqlCmd.Parameters.AddWithValue("_BookName", txtBookName.Text.Trim());
                mySqlCmd.Parameters.AddWithValue("_Author", txtAuthor.Text.Trim());
                mySqlCmd.Parameters.AddWithValue("_Description", txtDescription.Text.Trim());
                mySqlCmd.ExecuteNonQuery();
                MessageBox.Show("Submited Successfully");
                Clear();
                GridFill();
            }
        }

        void GridFill()
        {
            using (MySqlConnection mysqlCon = new MySqlConnection(connectionString))
            {
                mysqlCon.Open();
                MySqlDataAdapter sqlDa = new MySqlDataAdapter("BookViewAll",mysqlCon);
                sqlDa.SelectCommand.CommandType = CommandType.StoredProcedure;
                DataTable dtblBook = new DataTable();
                sqlDa.Fill(dtblBook);
                dgvBook.DataSource = dtblBook;
                dgvBook.Columns[0].Visible = false;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Clear();
            GridFill();
        }

        void Clear()
        {
            txtBookName.Text = txtAuthor.Text = txtDescription.Text = txtSearch.Text = "";
            bookID = 0;
            butSave.Text = "Save";
            butDelete.Enabled = false;
        }

        private void dgvBook_DoubleClick(object sender, EventArgs e)
        {
            if(dgvBook.CurrentRow.Index != -1)
            {
                txtBookName.Text = dgvBook.CurrentRow.Cells[1].Value.ToString();
                txtAuthor.Text = dgvBook.CurrentRow.Cells[2].Value.ToString();
                txtDescription.Text = dgvBook.CurrentRow.Cells[3].Value.ToString();
                bookID = Convert.ToInt32(dgvBook.CurrentRow.Cells[0].Value.ToString());
                butSave.Text = "Update";
                butDelete.Enabled = Enabled;
            }
        }

        private void butSearch_Click(object sender, EventArgs e)
        {
            using (MySqlConnection mysqlCon = new MySqlConnection(connectionString))
            {
                mysqlCon.Open();
                MySqlDataAdapter sqlDa = new MySqlDataAdapter("BookSearchByValue", mysqlCon);
                sqlDa.SelectCommand.CommandType = CommandType.StoredProcedure;
                sqlDa.SelectCommand.Parameters.AddWithValue("_SearchValue", txtSearch.Text);
                DataTable dtblBook = new DataTable();
                sqlDa.Fill(dtblBook);
                dgvBook.DataSource = dtblBook;
                dgvBook.Columns[0].Visible = false;
            }
        }

        private void butCancel_Click(object sender, EventArgs e)
        {
            Clear();
        }

        private void butDelete_Click(object sender, EventArgs e)
        {
            using (MySqlConnection mysqlCon = new MySqlConnection(connectionString))
            {
                mysqlCon.Open();
                MySqlCommand mySqlCmd = new MySqlCommand("BookDeleteByID", mysqlCon);
                mySqlCmd.CommandType = CommandType.StoredProcedure;
                mySqlCmd.Parameters.AddWithValue("_BookID", bookID);
                mySqlCmd.ExecuteNonQuery();
                MessageBox.Show("Deleted Successfully");
                Clear();
                GridFill();
            }
        }
    }
}
