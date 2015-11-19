using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Victors;

namespace Test
{
    public partial class TestForm : Form
    {
        public TestForm()
        {
            InitializeComponent();
        }

        private void TestForm_Load(object sender, EventArgs e)
        {
            FBClient client = new FBClient(@"character set = WIN1251; data source = localhost; initial catalog = D:\NASTROECHNAYA_2015.GDB; user id = SYSDBA; password = masterkey");
            DataTable dt = client.QueryRecordsList(
                @"
select 
  orderid, 
  orderno, 
  dateorder, 
  agreementdate
--coalesce(cast(agreementdate as datetimetype), '<null>') 
from orders
                ");
            List<dynamic> param = new List<dynamic>
            {
                "Найменування", "orderno",
                "Дата готовності", "dateorder",
                "Дата договору", "AgreementDate"
            };
            Dictionary dict = new Dictionary(param);
            SourceGridUtilities.Grid.Fill(grid1, dt, "orderid", dict);
        }
    }
}
