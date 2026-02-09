using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace Apartman_Yonetim_Sistemi
{
    public class sqlbaglantisi
    {
        public SqlConnection baglanti()
        {
            

            SqlConnection baglan = new SqlConnection("Data Source=.\\SQLEXPRESS;Initial Catalog=Apartman_Yonetim;Integrated Security=True;Encrypt=False;TrustServerCertificate=True");
            baglan.Open();
            return baglan;
        }

        
        public SqlConnection baglan()
        {
            return baglanti();
        }
    }
}