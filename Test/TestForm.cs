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
            DataTable dt = client.QueryRecordsList(qryOrders);
            Dictionary dict = new Dictionary(new List<dynamic>
            {
                "Номер замовлення", "orderno",
                "Дата готовності", "dateorder",
                "Стан", "VORDERSTATENAME",
                "Клієнт", "VCUSTOMERNAME"
            });
            Dictionary filter = new Dictionary(new List<dynamic>
            {
                "Дата готовності", "18.01"
            });
            SourceGridUtilities.Grid.Fill(grid1, dt, "orderid", dict);
            SourceGridUtilities.Grid.Fill(grid1, dt, "orderid", dict, filter);
        }
        #region Скрипти SQL
        private string qryOrders { get; set; } =
    @"
select * from vtorders
where deleted = 0
            ";
        #endregion
    }
}
