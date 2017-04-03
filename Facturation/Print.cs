using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Facturation
{
    public partial class Print : Form
    {
        public Print()
        {
            InitializeComponent();
        }
        struct clientx
        {
            public string nom;
            public int code;
        }
        List<clientx> walid = new List<clientx>();
        private void Print_Load(object sender, EventArgs e)
        {
            data1 ds = new data1();
            SqlDataAdapter da = new SqlDataAdapter("select * from x", Program.cnx);
            CrystalReport1 cr = new CrystalReport1();
            da.Fill(ds.x);
            cr.SetDataSource(ds);
            crystalReportViewer1.ReportSource = cr;
            crystalReportViewer1.Refresh();
            SqlDataAdapter sa = new SqlDataAdapter("select * from client", Program.cnx);
            DataTable dt = new DataTable();
            sa.Fill(dt);
            foreach (DataRow item in dt.Rows)
            {
                clientx x = new clientx();
                x.code = int.Parse(item[0].ToString());
                x.nom = item[1].ToString();
                comboBox1.Items.Add($"{x.code} {x.nom}");
                walid.Add(x);
            }
            /*
              Ds_stagiaire ods = new Ds_stagiaire();
            Cr_1stagiaire cr = new Cr_1stagiaire();
            da.Fill(ods, "Stagiaire");  
            cr.SetDataSource(ods);
            CRV_stagiaire.ReportSource = cr;
            CRV_stagiaire.Refresh();
             */
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            data1 ds = new data1();
            SqlDataAdapter da = new SqlDataAdapter($"select * from x where Nom='{walid[comboBox1.SelectedIndex].nom}'", Program.cnx);
            CrystalReport1 cr = new CrystalReport1();
            da.Fill(ds.x);
            cr.SetDataSource(ds);
            crystalReportViewer1.ReportSource = cr;
            crystalReportViewer1.Refresh();
        }
    }
}
