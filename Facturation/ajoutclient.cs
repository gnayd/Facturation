using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Data;


namespace Facturation
{
    public partial class ajoutclient : Form
    {
        
        public ajoutclient()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            foreach (TextBox item in Controls.OfType<TextBox>())
            {
                item.Clear();
            }
            ajoutclient_Load(sender, e);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (Program.cnx.State != ConnectionState.Open)
                    Program.cnx.Open();

                SqlCommand cmd = new SqlCommand("AjouterClient", Program.cnx);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Codecl", textBox1.Text);
                cmd.Parameters.AddWithValue("@nomcl", textBox2.Text);
                cmd.Parameters.AddWithValue("@adresscl", textBox3.Text);
                cmd.Parameters.AddWithValue("@ville", textBox4.Text);
                cmd.Parameters.AddWithValue("@solde", textBox5.Text);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Client Bien ajouté");
                foreach (TextBox item in Controls.OfType<TextBox>())
                {
                    item.Clear();
                }
                ajoutclient_Load(sender, e);

            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                if (Program.cnx.State != ConnectionState.Closed)
                    Program.cnx.Close();
            }
        }

        private void ajoutclient_Load(object sender, EventArgs e)
        {
            try
            {
                if (Program.cnx.State != ConnectionState.Open)
                    Program.cnx.Open();

                SqlCommand cmd = new SqlCommand("AfficheMaxCodeClient", Program.cnx);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataReader dr = cmd.ExecuteReader();
                dr.Read();
                int max = int.Parse(dr[0].ToString());
                // MessageBox.Show(max.ToString());
                textBox1.Text = (max + 1).ToString();
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                if (Program.cnx.State != ConnectionState.Closed)
                    Program.cnx.Close();
            }
        }
       
    }
}
