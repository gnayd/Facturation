﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.Sql;
using System.Data.SqlClient;

namespace Facturation
{
    static class Program
    {
        public static SqlConnection cnx =new SqlConnection("Data Source=ADIL-PC\\SQLEXPRESS;Initial Catalog=facturation;Integrated Security=True");

        /// <summary>
        /// Point d'entrée principal de l'application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Facturation());
        }
    }
}
