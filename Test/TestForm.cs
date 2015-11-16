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
            FBClient client = new FBClient();
            DataTable dt = client.QueryRecordsList(@"select * from orders");
            List<dynamic> param = new List<dynamic>
            {
                "Найменування", "orderno",
                "Дата готовності", "dateorder"
            };
            Dictionary dict = new Dictionary(param);
            SourceGridUtilities.Grid.Fill(grid1, dt, "orderid", dict);
        }
    }
}
