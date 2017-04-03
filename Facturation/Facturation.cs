using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.Sql;
using System.Data.SqlClient;

namespace Facturation
{
    
    public partial class Facturation : Form
    {
        struct article
        {
            public string refe;
            public string designiation;
            public int pu;
            public int qut_st;
        }
        struct client
        {
            public string code;
            public string nom;
            public string adr;
            public string ville;
            public float solde;
        }
        List <client> Tcc = new List<client>();
        List<article> Tc = new List<article>();
        SqlCommand cmd;
        public DataTable dtc;
        public DataTable dtm;
        public DataTable la;
        SqlDataAdapter da;
        SqlDataReader dr;

        public Facturation()
        {
            InitializeComponent();
        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            fillcba();
            fillcbc();
            makefanumber();
        }

        private void button6_Click(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            ajoutclient ac = new ajoutclient();
            ac.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int cellnumber=-1;
            if (qtevendus.Text != string.Empty && int.Parse(qtevendus.Text)<=Tc[cbarticle.SelectedIndex].qut_st)
            {
                //Tc[cbarticle.SelectedIndex].qut_st. = Tc[cbarticle.SelectedIndex].qut_st - int.Parse(qtevendus.Text);
                //calcul du quantité commandé * quantité du stock

                int dgcount = dataGridView1.Rows.Count - 1;
                //// MessageBox.Show(dgcount.ToString());
                if (dgcount == 0)
                {
                    double somme = (Convert.ToDouble(prix.Text) * Convert.ToDouble(qtevendus.Text));
                    double ds = 0;
                    dataGridView1.Rows.Add(Tc[cbarticle.SelectedIndex].refe, Tc[cbarticle.SelectedIndex].designiation, Tc[cbarticle.SelectedIndex].pu.ToString(), qtevendus.Text, somme.ToString());
                    for (int j = 0; j < dataGridView1.Rows.Count; j++)
                    {
                        ds += Convert.ToDouble(dataGridView1.Rows[j].Cells[4].Value);
                    }

                }
                else if (dgcount != 0)
                {
                   // String er="false";
                    for (int i = 0; i < dataGridView1.Rows.Count-1; i++)
                    {

                        if ( dataGridView1.Rows[i].Cells[0].Value.ToString()==(Tc[cbarticle.SelectedIndex].refe))
                        
                        {
                            cellnumber = i;
                            break;
                        }
                    } 
                    
                    if (cellnumber!=-1)
                    {
                        //  MessageBox.Show(comboBox2.Text.ToString());
                        Double pr = Convert.ToDouble(dataGridView1.Rows[cellnumber].Cells[2].Value);
                        int qt = Convert.ToInt32(dataGridView1.Rows[cellnumber].Cells[3].Value);
                        Double p = pr; //+ (Convert.ToDouble(Tc[cbarticle.SelectedIndex].pu));
                        int q = qt + (int.Parse(qtevendus.Text));
                        Double s = p * q;
                        dataGridView1.Rows[cellnumber].Cells[2].Value = p;
                        dataGridView1.Rows[cellnumber].Cells[3].Value = q;
                        dataGridView1.Rows[cellnumber].Cells[4].Value = s;
                        double ds = 0;
                        for (int j = 0; j < dataGridView1.Rows.Count; j++)
                        {
                            ds += Convert.ToDouble(dataGridView1.Rows[j].Cells[4].Value);
                        }
                        t2.Text = ds.ToString();
                        
                    }
                    else 
                    {
                        //  MessageBox.Show(comboBox2.Text.ToString());
                        Double pp = Convert.ToDouble(qtevendus.Text);
                        double qq = (Convert.ToDouble(Tc[cbarticle.SelectedIndex].pu));
                        Double s = pp * qq;
                        //dataGridView1.Rows.Add(comboBox2.Text, textBox10.Text, textBox9.Text, textBox8.Text, s);
                        dataGridView1.Rows.Add(Tc[cbarticle.SelectedIndex].refe, Tc[cbarticle.SelectedIndex].designiation, Tc[cbarticle.SelectedIndex].pu.ToString(), qtevendus.Text, s.ToString());
                        double ds = 0;
                        for (int j = 0; j < dataGridView1.Rows.Count; j++)
                        {
                            ds += Convert.ToDouble(dataGridView1.Rows[j].Cells[4].Value);
                        }
                        t2.Text = ds.ToString();
                    }

                }
            }
        }

        private void cbarticle_SelectedIndexChanged(object sender, EventArgs e)
        {
            design.Text = Tc[cbarticle.SelectedIndex].designiation;
            prix.Text = Tc[cbarticle.SelectedIndex].pu.ToString();
            qtestock.Text = Tc[cbarticle.SelectedIndex].qut_st.ToString();
            design.ReadOnly = true;
            prix.ReadOnly = true;
            qtestock.ReadOnly = true;
        }
        #region mes fontion
        public void fillcba()
        {
            Tc.Clear();
            dtm = new DataTable();
            date.Text = DateTime.Now.ToShortDateString();
            date.ReadOnly = true;
            cmd = new SqlCommand("select * from marchandise", Program.cnx);
            Program.cnx.Open();
            da = new SqlDataAdapter(cmd);
            da.Fill(dtm);
            for (int i = 0; i < dtm.Rows.Count; i++)
            {

                article a=new article();

                a.refe = dtm.Rows[i][0].ToString();
                a.designiation = dtm.Rows[i][1].ToString();
                a.pu = int.Parse(dtm.Rows[i][2].ToString());
                a.qut_st = int.Parse(dtm.Rows[i][3].ToString());
                Tc.Add(a);
                
                cbarticle.Items.Add(dtm.Rows[i][0].ToString() + " " + dtm.Rows[i][1].ToString());
            }
            Program.cnx.Close();
        }
        public void fillcbc()
        {
            Tcc.Clear();
            Program.cnx.Open();
            dtc = new DataTable();
            cmd = new SqlCommand("select * from Client", Program.cnx);
            if (Program.cnx.State == ConnectionState.Closed) Program.cnx.Open(); ;
            da = new SqlDataAdapter(cmd);
            da.Fill(dtc);
            for (int i = 0; i < dtc.Rows.Count; i++)
            {
                client c = new client();
                c.code = dtc.Rows[i][0].ToString();
                c.nom = dtc.Rows[i][1].ToString();
                c.adr = dtc.Rows[i][2].ToString();
                c.ville = (dtc.Rows[i][3].ToString());
                Tcc.Add(c);
            //    Tcc[i].solde = float.Parse(dtc.Rows[i][3].ToString());
                cbclient.Items.Add(dtc.Rows[i][0].ToString() + " " + dtc.Rows[i][1].ToString());

            }


            if (Program.cnx.State == ConnectionState.Open) Program.cnx.Close(); ;

        }
        public void makefanumber()
        {
            if (Program.cnx.State == ConnectionState.Closed) Program.cnx.Open(); 
            la = new DataTable();
            cmd = new SqlCommand("select count(*)+1 as [nombre de facture] from facture", Program.cnx);
            if (Program.cnx.State == ConnectionState.Closed) Program.cnx.Open(); ;
            da = new SqlDataAdapter(cmd);
            da.Fill(la);
            NumVente.ReadOnly = true;
            NumVente.Text = la.Rows[0][0].ToString();


        }

        #endregion

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void indexchange(object sender, EventArgs e)
        {
            nom.Text = Tcc[cbclient.SelectedIndex].nom;
            nom.ReadOnly = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {

            double ss = 0;
            if (dataGridView1.SelectedRows.Count > 0)
            {
                ss = Convert.ToDouble(dataGridView1.Rows[dataGridView1.SelectedRows[0].Index].Cells[4].Value);
                dataGridView1.Rows.RemoveAt(dataGridView1.SelectedRows[0].Index);
                t2.Text = (Convert.ToDouble(t2.Text) - ss).ToString();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if(dataGridView1.Rows.Count!=0)
            {
                if (Program.cnx.State == ConnectionState.Closed) Program.cnx.Open();
                //string x = "@1,@2";
                cmd = new SqlCommand($"insert into facture values(@a1,@a2,@a3,@a4,@a5)",Program.cnx);
                cmd.Parameters.Add("@a1", SqlDbType.Int).Value = int.Parse(NumVente.Text);
                cmd.Parameters.Add("@a2", SqlDbType.Date).Value =DateTime.Now.ToShortDateString();
                decimal a= Convert.ToDecimal(t2.Text);
                cmd.Parameters.Add("@a3", SqlDbType.Money).Value =   a;

                cmd.Parameters.Add("@a4", SqlDbType.Money).Value =a;
                cmd.Parameters.Add("@a5", SqlDbType.Int).Value = Tcc[cbclient.SelectedIndex].code.ToString();
                cmd.ExecuteNonQuery();
                if (Program.cnx.State == ConnectionState.Open) Program.cnx.Close();
                for (int i = 0; i < dataGridView1.Rows.Count-1; i++)
                {
                    if (Program.cnx.State == ConnectionState.Closed) Program.cnx.Open();
                    SqlCommand cmd2 = new SqlCommand($@"insert into Contient values(@b1,@b2,@b3)",Program.cnx);
                    cmd2.Parameters.Add("@b1",SqlDbType.Int).Value=int.Parse(dataGridView1.Rows[i].Cells[3].Value.ToString());
                    cmd2.Parameters.Add("@b2",SqlDbType.VarChar).Value= dataGridView1.Rows[i].Cells[0].Value.ToString();
                    cmd2.Parameters.Add("@b3", SqlDbType.Int).Value = int.Parse(NumVente.Text);
                    cmd2.ExecuteNonQuery();
                    if (Program.cnx.State == ConnectionState.Open) Program.cnx.Close();
                    if (Program.cnx.State == ConnectionState.Closed) Program.cnx.Open();
                    SqlCommand cmd3 = new SqlCommand($"update marchandise set qut_st=qut_st-{dataGridView1.Rows[i].Cells[3].Value.ToString()} where ref='{dataGridView1.Rows[i].Cells[0].Value.ToString()}'",Program.cnx);
                    cmd3.ExecuteNonQuery();
                    if (Program.cnx.State == ConnectionState.Open) Program.cnx.Close();
                    makefanumber();
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Print p = new Print();
            p.Show();
            Hide();
        }
    }
}
