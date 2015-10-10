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
        public static void SGridFill(SourceGrid.Grid grid1, DataTable dt)
        {
            grid1.ColumnsCount = dt.Columns.Count;
            grid1.FixedRows = 1;
            grid1.Rows.Insert(0);
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                grid1[0, i] = new SourceGrid.Cells.ColumnHeader(dt.Columns[i].Caption);
            }

            for (int r = 0; r < dt.Rows.Count; r++)
            {
                grid1.Rows.Insert(r + 1);
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    grid1[r + 1, i] = new SourceGrid.Cells.Cell(dt.Rows[r][i]);
                }
            }

            grid1.AutoSizeCells();
        }
        protected class FieldItem
        {
            public string Caption { get; set; }
            public string Field { get; set; }
        } 
        private class Fields
        {
            public List<FieldItem> Items { get; set; }
            public string Key { get; set; } //The FieldName of KeyValue
            public Fields(string _key)
            {
                Key = _key;
                Items = new List<FieldItem> { };
            }
            public Fields(string _key, List<FieldItem> _items) : this(_key)
            {
                Items = _items;
            }
            public Fields(List<FieldItem> _items, string _key) : this(_key)
            {
                Items = _items;
            }
        }
    }
}