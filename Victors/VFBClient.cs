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
        public static void SGridFill(SourceGrid.Grid grid, DataTable dt, Dictionary fields, Dictionary filter)
        {
            //Columns filling
            grid.ColumnsCount = fields.isEmpty ? dt.Columns.Count : fields.Count;
            grid.FixedRows = 1;
            grid.Rows.Insert(0);
            for (int i = 0; i < (fields.isEmpty ? dt.Columns.Count : fields.Count); i++)
            {
                grid[0, i] = new SourceGrid.Cells.ColumnHeader(fields.isEmpty ? dt.Columns[i].Caption : fields.Names[i]);
            }
            //Data filling
            for (int r = 0; r < dt.Rows.Count; r++)
            {
                bool RowChecked = filter.isEmpty || fields.isEmpty;
                if (!RowChecked)
                {
                    RowChecked = true;
                    for (int i = 0; i < filter.Count || !RowChecked; i++)
                    {
                        //int index = fields.IndexOfCaption(filter.Items[i].Caption);
                        try
                        {
                            RowChecked = dt.Rows[r][fields[filter.Names[i]]] == filter[i];
                        }
                        catch { }
                    }
                }
                if (!RowChecked)
                {
                    continue;
                }
                    grid.Rows.Insert(r + 1);
                for (int i = 0; i < (fields.isEmpty ? dt.Columns.Count : fields.Count); i++)
                {
                    if (fields.isEmpty)
                    {
                        grid[r + 1, i] = new SourceGrid.Cells.Cell(dt.Rows[r][i]);
                    }
                    else
                    {
                        grid[r + 1, i] = new SourceGrid.Cells.Cell(dt.Rows[r][fields[i]]);
                    }
                }
            }

            grid.AutoSizeCells();
        }
        public static void SGridFill(SourceGrid.Grid grid, DataTable dt, Dictionary fields)
        {
            SGridFill(grid, dt, fields, null);
        }
        public static void SGridFill(SourceGrid.Grid grid, DataTable dt)
        {
            SGridFill(grid, dt, null, null);
        }
    }
}