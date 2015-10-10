using System;
using FirebirdSql.Data.FirebirdClient;
using System.Data;
using System.Windows.Forms;
using System.Collections.Generic;

namespace Victors
{
    static public class VFBClient
    {
        public static string ConnectionStr { get; set; } = @"character set=WIN1251;data source=localhost;initial catalog=D:\NASTROECHNAYA_2015.GDB ;user id=SYSDBA;password=masterkey";
        public static DataTable QueryRecordsList(string SQL)
        {
            using (FbConnection fbc = new FbConnection(ConnectionStr))
            {
                try
                {
                    fbc.Open();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message +
                        System.IO.Path.GetFileName(
                        System.Reflection.Assembly.GetExecutingAssembly().Location));
                    return null;
                }

                // Транзакция
                FbTransactionOptions fbto = new FbTransactionOptions();
                fbto.TransactionBehavior = FbTransactionBehavior.NoWait |
                     FbTransactionBehavior.ReadCommitted |
                     FbTransactionBehavior.RecVersion;
                FbTransaction fbt = fbc.BeginTransaction(fbto);

                // Создаем простой запрос
                FbCommand fbcom = new FbCommand(SQL, fbc, fbt);
                /*
                fbcom.Parameters.Clear();
                fbcom.Parameters.AddWithValue("orderno", "362");
                */
                // Создаем адаптер данных
                FbDataAdapter fbda = new FbDataAdapter(fbcom);

                DataSet ds = new DataSet();

                try
                {
                    fbda.Fill(ds);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message +
                        System.IO.Path.GetFileName(
                        System.Reflection.Assembly.GetExecutingAssembly().Location));
                    return null;
                }

                finally
                {
                    fbt.Rollback();
                    fbc.Close();
                    fbcom.Dispose();
                    fbt.Dispose();
                    fbc.Dispose();
                    fbda.Dispose();
                }

                return ds.Tables[0];

            }
        }
        public static void SGridFill(SourceGrid.Grid grid, DataTable dt, Fields fields)
        {
            grid.ColumnsCount = fields.Items.Count==0 ? dt.Columns.Count : fields.Items.Count;
            grid.FixedRows = 1;
            grid.Rows.Insert(0);
            for (int i = 0; i < (fields.Items.Count == 0 ? dt.Columns.Count : fields.Items.Count); i++)
            {
                grid[0, i] = new SourceGrid.Cells.ColumnHeader(fields.Items.Count == 0 ? dt.Columns[i].Caption : fields.Items[i].Caption);
            }

            for (int r = 0; r < dt.Rows.Count; r++)
            {
                grid.Rows.Insert(r + 1);
                for (int i = 0; i < (fields.Items.Count == 0 ? dt.Columns.Count : fields.Items.Count); i++)
                {
                    if (fields.Items.Count == 0) {
                        grid[r + 1, i] = new SourceGrid.Cells.Cell(dt.Rows[r][i]);
                    }
                    else
                    {
                        grid[r + 1, i] = new SourceGrid.Cells.Cell(dt.Rows[r][fields.Items[i].Field]);
                    }
                }
            }

            grid.AutoSizeCells();
        }
        public static void SGridFill(SourceGrid.Grid grid, DataTable dt)
        {
            SGridFill(grid, dt, new Fields());
        }
        public class Fields
        {
            public List<FieldItem> Items { get; set; }
            public string Key { get; set; } //The FieldName of KeyValue
            public Fields(string _key)
            {
                Key = _key;
                Items = new List<FieldItem> { };
            }
            public Fields(List<FieldItem> _items, string _key) : this(_key)
            {
                Items = _items;
            }
            public Fields() : this("")
            {

            }
            public Fields(List<FieldItem> _items) : this(_items.Count > 0 ? _items[0].Field : "", _items)
            {

            }
            public Fields(string _key, List<FieldItem> _items) : this(_items, _key)
            {
                
            }
        }
        public class FieldItem
        {
            public string Caption { get; set; }
            public string Field { get; set; }
        } 

    }
}