using System;
using FirebirdSql.Data.FirebirdClient;
using System.Data;
using System.Windows.Forms;

namespace Victors
{
    public class FBClient
    {
        public string ConnectionStr { get; set; } = @"character set=WIN1251;data source=localhost;initial catalog=D:\NASTROECHNAYA_2015.GDB ;user id=SYSDBA;password=masterkey";
        public void ExecuteSQLCommit(string SQL)
        {
            using (FbConnection fbConnection = new FbConnection(ConnectionStr))
            {
                try
                {
                    fbConnection.Open();
                }
                catch (Exception e)
                {
                    throw new FBClientException(
                        e.Message + "\r" +
                        System.IO.Path.GetFileName(System.Reflection.Assembly.GetExecutingAssembly().Location));
                }

                // Транзакция
                FbTransactionOptions fbTransactionOptions = new FbTransactionOptions();
                fbTransactionOptions.TransactionBehavior =
                    FbTransactionBehavior.NoWait |
                    FbTransactionBehavior.Write |
                    FbTransactionBehavior.Autocommit;
                FbTransaction fbTransaction = fbConnection.BeginTransaction(fbTransactionOptions);

                // Создаем простой запрос
                FbCommand fbCommand = new FbCommand(SQL, fbConnection, fbTransaction);
                /*
                fbcom.Parameters.Clear();
                fbcom.Parameters.AddWithValue("orderno", "362");
                */
                try
                {
                    fbCommand.ExecuteNonQuery();
                    fbTransaction.Commit();
                }
                catch (Exception e)
                {
                    fbTransaction.Rollback();
                    throw new FBClientException(
                        e.Message + "\r" +
                        System.IO.Path.GetFileName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\r" +
                        SQL.Replace("\n", "")
                    );
                }
                finally
                {
                    fbConnection.Close();
                    fbCommand.Dispose();
                    fbTransaction.Dispose();
                    fbConnection.Dispose();
                }
            }
        }

        public DataTable QueryRecordsList(string SQL)
        {
            using (FbConnection fbConnection = new FbConnection(ConnectionStr))
            {
                try
                {
                    fbConnection.Open();
                }
                catch (Exception e)
                {
                    throw new FBClientException(
                        e.Message + "\r" + 
                        System.IO.Path.GetFileName(System.Reflection.Assembly.GetExecutingAssembly().Location));
                }

                // Транзакция
                FbTransactionOptions fbTransactionOptions = new FbTransactionOptions();
                fbTransactionOptions.TransactionBehavior = 
                    FbTransactionBehavior.NoWait |
                    FbTransactionBehavior.ReadCommitted |
                    FbTransactionBehavior.RecVersion;
                FbTransaction fbTransaction = fbConnection.BeginTransaction(fbTransactionOptions);

                // Создаем простой запрос
                FbCommand fbCommand = new FbCommand(SQL, fbConnection, fbTransaction);
                /*
                fbcom.Parameters.Clear();
                fbcom.Parameters.AddWithValue("orderno", "362");
                */
                // Создаем адаптер данных
                FbDataAdapter fbDataAdapter = new FbDataAdapter(fbCommand);

                DataSet dataset = new DataSet();

                try
                {
                    fbDataAdapter.Fill(dataset);
                    fbTransaction.Commit();
                }
                catch (Exception e)
                {
                    //return null;
                    fbTransaction.Rollback();
                    throw new FBClientException(
                        e.Message + "\r" +
                        System.IO.Path.GetFileName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\r" +
                        SQL.Replace("\n", "")
                    );
                }
                finally
                {
                    fbConnection.Close();
                    fbCommand.Dispose();
                    fbTransaction.Dispose();
                    fbConnection.Dispose();
                    fbDataAdapter.Dispose();
                }
                return dataset.Tables[0];
            }
        }
        public dynamic QueryValue(string SQL)
        {
            try
            {
                DataTable dt = QueryRecordsList(SQL);
                if (dt.Columns.Count > 0) {
                    return dt.Rows[0][0];
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                return null;
                throw new FBClientException(e.Message);
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