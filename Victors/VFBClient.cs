using System;
using FirebirdSql.Data.FirebirdClient;
using System.Data;
using System.Windows.Forms;

namespace Victors
{
    public class FBClient
    {
        public string ConnectionStr { get; set; } = @"character set=WIN1251;data source=localhost;initial catalog=D:\NASTROECHNAYA_2015.GDB ;user id=SYSDBA;password=masterkey";
        public DataTable QueryRecordsList(string SQL)
        {
            using (FbConnection fbc = new FbConnection(ConnectionStr))
            {
                try
                {
                    fbc.Open();
                }
                catch (Exception ex)
                {
                    throw new FBClientException(
                        ex.Message + "\r" + 
                        System.IO.Path.GetFileName(System.Reflection.Assembly.GetExecutingAssembly().Location));
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
                catch (Exception e)
                {
                    //return null;
                    throw new FBClientException(
                        e.Message + "\r" +
                        System.IO.Path.GetFileName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\r" +
                        SQL
                    );
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
        public dynamic QueryValue(string SQL)
        {
            DataTable dt = QueryRecordsList(SQL);
            if (dt != null && dt.Columns.Count > 0) {
                return dt.Rows[0][0];
            }
            else
            {
                return null;
            }
        }
        private void Constructor(string connectionStr)
        {
            ConnectionStr = connectionStr;
        }
        public FBClient()
        {
            Constructor("");
        }
        public FBClient(string connectionStr)
        {
            Constructor(connectionStr);
        }
        
        
    }
    class FBClientException : Exception
    {
        public FBClientException(string S) : base(S)
        {
        }
    }
}