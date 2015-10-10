using System;
using FirebirdSql.Data.FirebirdClient;
using System.Data;
using System.Windows.Forms;
using System.Collections.Generic;
using Victors.VFBClientClasses;

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
        public static void SGridFill(SourceGrid.Grid grid, DataTable dt, Fields fields, Filter filter)
        {
            bool EmptyFields = true;
            try
            {
                EmptyFields = fields.Items.Count == 0;
            }
            catch { }

            bool EmptyFilter = true;
            try
            {
                EmptyFilter = filter.Items.Count == 0;
            }
            catch { }
            //Columns filling
            grid.ColumnsCount = EmptyFields ? dt.Columns.Count : fields.Items.Count;
            grid.FixedRows = 1;
            grid.Rows.Insert(0);
            for (int i = 0; i < (EmptyFields ? dt.Columns.Count : fields.Items.Count); i++)
            {
                grid[0, i] = new SourceGrid.Cells.ColumnHeader(EmptyFields ? dt.Columns[i].Caption : fields.Items[i].Caption);
            }
            //Data filling
            for (int r = 0; r < dt.Rows.Count; r++)
            {
                bool RowChecked = EmptyFilter || EmptyFields;
                if (!RowChecked)
                {
                    RowChecked = true;
                    for (int i = 0; i < filter.Items.Count || !RowChecked; i++)
                    {
                        int index = fields.IndexOfCaption(filter.Items[i].Caption);
                        RowChecked = index==-1 || dt.Rows[r][fields.Items[index].Field]==filter.Items[i].Value;
                    }
                }
                if (!RowChecked)
                {
                    continue;
                }
                    grid.Rows.Insert(r + 1);
                for (int i = 0; i < (EmptyFields ? dt.Columns.Count : fields.Items.Count); i++)
                {
                    if (EmptyFields)
                    {
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
            SGridFill(grid, dt, null, null);
        }
        public static void SGridFill(SourceGrid.Grid grid, DataTable dt, Fields fields)
        {
            SGridFill(grid, dt, fields, null);
        }
    }
}