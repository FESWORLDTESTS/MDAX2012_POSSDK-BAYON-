using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Microsoft.Dynamics.Retail.Pos.SalesOrder
{
    class DAC
    {
        SqlConnection conn;
        public DAC(SqlConnection _conn)
        {
            conn = _conn;
        }

        public void SaveContactDataOV(string Transactionid, string terminalid, string storeid, string Name, string Email, string phone,string salesperson,string salespersonid)
        {
            try
            {
                string cmdText = "Exec xsp_insertContactDataOV @STOREID, @TERMINALID, @TRANSACTIONID, @NAME, @EMAIL,@PHONE,@SALESPERSON,@SALESPERSONID";
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }
                using (SqlCommand command = new SqlCommand(cmdText, conn))
                {
                    command.Parameters.Add("@STOREID", SqlDbType.NVarChar, 10).Value = storeid;
                    command.Parameters.Add("@TERMINALID", SqlDbType.NVarChar, 10).Value = terminalid;
                    command.Parameters.Add("@TRANSACTIONID", SqlDbType.NVarChar, 44).Value = Transactionid;
                    command.Parameters.Add("@NAME", SqlDbType.NVarChar, 60).Value = Name;
                    command.Parameters.Add("@EMAIL", SqlDbType.NVarChar, 60).Value = Email==null?"":Email;
                    command.Parameters.Add("@PHONE", SqlDbType.NVarChar, 15).Value = phone;
                    command.Parameters.Add("@SALESPERSON", SqlDbType.NVarChar, 60).Value = salesperson==null?"":salesperson;
                    command.Parameters.Add("@SALESPERSONID", SqlDbType.NVarChar, 20).Value = salespersonid;
                    using (SqlDataReader reader = command.ExecuteReader(CommandBehavior.SingleRow))
                    {
                        if (reader.HasRows)
                        {
                            reader.Read();
                        }
                    }
                }
            }
            finally
            {
                if (conn.State != ConnectionState.Closed)
                {
                    conn.Close();
                }
            }
        }

        public void SaveLineDataOV(string Transactionid, string terminalid, string storeid, string StoreNumber, string shipmode,string ItemLineId,string isPickup)
        {
            try
            {
                string cmdText = "Exec xsp_insertLineDataOV @STOREID, @TERMINALID, @TRANSACTIONID, @STORENUMBER, @SHIPMODE,@ITEMLINEID,@ISPICKUP ";
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }
                using (SqlCommand command = new SqlCommand(cmdText, conn))
                {
                    command.Parameters.Add("@STOREID", SqlDbType.NVarChar, 10).Value = storeid;
                    command.Parameters.Add("@TERMINALID", SqlDbType.NVarChar, 10).Value = terminalid;
                    command.Parameters.Add("@TRANSACTIONID", SqlDbType.NVarChar, 44).Value = Transactionid;
                    command.Parameters.Add("@STORENUMBER", SqlDbType.NVarChar, 10).Value = StoreNumber==null?"":StoreNumber ;
                    command.Parameters.Add("@SHIPMODE", SqlDbType.NVarChar, 60).Value = shipmode == null?"":shipmode;
                    command.Parameters.Add("@ITEMLINEID", SqlDbType.NVarChar, 15).Value = ItemLineId==null?"":ItemLineId;
                    command.Parameters.Add("@ISPICKUP", SqlDbType.NVarChar, 1).Value = isPickup==null?"":isPickup;
                    using (SqlDataReader reader = command.ExecuteReader(CommandBehavior.SingleRow))
                    {
                        if (reader.HasRows)
                        {
                            reader.Read();
                        }
                    }
                }
            }
            finally
            {
                if (conn.State != ConnectionState.Closed)
                {
                    conn.Close();
                }
            }
        }

        public DataTable GetLineDataOV(string Transactionid, string terminalid, string storeid,  string ItemLineId)
        {
            DataTable dt = new DataTable();
            try
            {
                string cmdText = "Exec xspr_LineDataOV @STOREID, @TERMINALID, @TRANSACTIONID,@ITEMLINEID ";
                //if (conn.State != ConnectionState.Open)
                //{
                //    conn.Open();
                //}
                using (SqlCommand command = new SqlCommand(cmdText, conn))
                {
                    command.Parameters.Add("@STOREID", SqlDbType.NVarChar, 10).Value = storeid;
                    command.Parameters.Add("@TERMINALID", SqlDbType.NVarChar, 10).Value = terminalid;
                    command.Parameters.Add("@TRANSACTIONID", SqlDbType.NVarChar, 44).Value = Transactionid;
                    command.Parameters.Add("@ITEMLINEID", SqlDbType.NVarChar, 15).Value = ItemLineId;
                    

                    SqlDataAdapter adapt = new SqlDataAdapter(command);
                    adapt.Fill(dt);

                }
            }
            catch (Exception Ex)
            {
            }
            return dt;
        }

        public DataTable GetContactData(string Transactionid, string terminalid, string storeid)
        {
            DataTable dt = new DataTable();
            try
            {
                string cmdText = "Exec xspr_ContacteDataOV @STOREID, @TERMINALID, @TRANSACTIONID";
                //if (conn.State != ConnectionState.Open)
                //{
                //    conn.Open();
                //}
                using (SqlCommand command = new SqlCommand(cmdText, conn))
                {
                    command.Parameters.Add("@STOREID", SqlDbType.NVarChar, 10).Value = storeid;
                    command.Parameters.Add("@TERMINALID", SqlDbType.NVarChar, 10).Value = terminalid;
                    command.Parameters.Add("@TRANSACTIONID", SqlDbType.NVarChar, 44).Value = Transactionid;
                    
                    SqlDataAdapter adapt = new SqlDataAdapter(command);
                    adapt.Fill(dt);

                }
            }
            catch (Exception Ex)
            {
            }
            return dt;
        }

        public string getCardType(string Transactionid, string Terminal, string Store, int LineId,string TenderId)
        {
            DataTable data = new DataTable();
            string retval ="";
            try
            {
                SqlCommand comm = new SqlCommand("xspr_getCardType @Transactionid,@terminal,@store,@LineId,@TenderId", conn);
                comm.Parameters.Add("@Transactionid", SqlDbType.NVarChar,44);
                comm.Parameters.Add("@terminal", SqlDbType.NVarChar, 10);
                comm.Parameters.Add("@store", SqlDbType.NVarChar, 10);
                comm.Parameters.Add("@LineId", SqlDbType.Int);
                comm.Parameters.Add("@TenderId", SqlDbType.NVarChar, 10);
                comm.Parameters["@Transactionid"].Value = Transactionid;
                comm.Parameters["@terminal"].Value = Terminal;
                comm.Parameters["@store"].Value = Store;
                comm.Parameters["@LineId"].Value = LineId;
                comm.Parameters["@TenderId"].Value = TenderId;
                SqlDataAdapter adapt = new SqlDataAdapter(comm);

                adapt.Fill(data);

                if (data != null)
                {
                    if (data.Rows.Count > 0)
                    {
                        retval = data.Rows[0]["CARDTYPEID"].ToString().Trim();
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return retval;
        }

        //GRW Guardar Información de items recogidos
        public bool GRW_INSERTPICKUPITEM(string storeid, string terminal, string dataareaid, string transactionid, string salesOrderId, decimal lineNum, decimal quantity)
        {
            DataTable DtResults = new DataTable();
            bool retval = true;
            try
            {
                string cmdText = "Exec xsp_GRW_INSERTPICKUPITEM @DATAAREAID, @STOREID, @TERMINAL, @TRANSACTIONID, @SALESORDERID, @QTY, @LINENUM";
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }
                using (SqlCommand command = new SqlCommand(cmdText, conn))
                {
                    command.Parameters.Add("@STOREID", SqlDbType.NVarChar, 10).Value = storeid;
                    command.Parameters.Add("@TERMINAL", SqlDbType.NVarChar, 10).Value = terminal;
                    command.Parameters.Add("@DATAAREAID", SqlDbType.NVarChar, 4).Value = dataareaid;
                    command.Parameters.Add("@TRANSACTIONID", SqlDbType.NVarChar, 44).Value = transactionid;
                    command.Parameters.Add("@SALESORDERID", SqlDbType.NVarChar, 20).Value = salesOrderId;
                    command.Parameters.Add("@QTY", SqlDbType.Decimal).Value = quantity;
                    command.Parameters.Add("@LINENUM", SqlDbType.Decimal).Value = lineNum;
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            DtResults.Load(reader);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                retval = false;
                //Application.Services.Dialog.ShowMessage(ex.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (conn.State != ConnectionState.Closed)
                {
                    conn.Close();
                }
            }
            return retval;
        }
        //GRW

        //GRW Guardar Información de Monto de Depósito de Orden de Venta sobreescrito
        public bool GRW_INSERTLASTPICKUP(string storeid, string terminal, string dataareaid, string transactionid, string salesOrderId, int lastPickUp)
        {
            DataTable DtResults = new DataTable();
            bool retval = true;
            try
            {
                string cmdText = "Exec xsp_GRW_INSERTLASTPICKUP @DATAAREAID, @STOREID, @TERMINAL, @TRANSACTIONID, @SALESORDERID, @LASTPICKUP ";
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }
                using (SqlCommand command = new SqlCommand(cmdText, conn))
                {
                    command.Parameters.Add("@STOREID", SqlDbType.NVarChar, 10).Value = storeid;
                    command.Parameters.Add("@TERMINAL", SqlDbType.NVarChar, 10).Value = terminal;
                    command.Parameters.Add("@DATAAREAID", SqlDbType.NVarChar, 4).Value = dataareaid;
                    command.Parameters.Add("@TRANSACTIONID", SqlDbType.NVarChar, 44).Value = transactionid;
                    command.Parameters.Add("@SALESORDERID", SqlDbType.NVarChar, 20).Value = salesOrderId;
                    command.Parameters.Add("@LASTPICKUP", SqlDbType.Int).Value = lastPickUp;
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            DtResults.Load(reader);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                retval = false;
                SalesOrder.InternalApplication.Services.Dialog.ShowMessage(ex.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (conn.State != ConnectionState.Closed)
                {
                    conn.Close();
                }
            }
            return retval;
        }
        //GRW
    }
}
