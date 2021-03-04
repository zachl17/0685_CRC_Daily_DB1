using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _0685_CRC_Daily_DB1
{
    public partial class Form1 : Form
    {
        //sql connection
        static string testDataSource = "AMCDB04-T";
        static string productionDataSource = "AMCDB03";
        static string testInitialCatalog = "DevelopmentTesting";
        static string productionInitialCatalog = "";
        static string integratedSecurity = "True";
        static string multipleActiveResultSets = "True";
        SqlConnection cnSQL;
        SqlCommand selectCMD, insertCMD, deleteCMD, updateCMD;
        SqlDataReader dr;
        string selectSQL, insertSQL, deleteSQL, updateSQL;

        //sql tables
        string dataTable = "[dbo].[AllWorkers.CRCDaily_Data]";
        string doneDataTable = "[dbo].[AllWorkers.CRCDaily_Done]";
        string sqlConnStr = $"Data Source={testDataSource};Initial Catalog={testInitialCatalog};Integrated Security={integratedSecurity};MultipleActiveResultSets={multipleActiveResultSets};";

        Int64 accountID;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //get remaining number of accounts in the workers table
            GetRemainingTotals();
            //populate the form
            PopulateForm();
        }
        //Fills the form with data from the worker's table
        void PopulateForm()
        {
            selectSQL = $@"SELECT TOP 1 * FROM {dataTable} WHERE (CURRENT_TIMESTAMP > DATEADD(MINUTE, 15, WorkerTimestamp) OR CurrentUser = @CurrentUser)";

            using (cnSQL = new SqlConnection(sqlConnStr))
            {
                cnSQL.Open();
                using (selectCMD = new SqlCommand(selectSQL, cnSQL))
                {
                    selectCMD.Parameters.Add("@CurrentUser", SqlDbType.VarChar).Value = Environment.UserName.ToString().ToUpper();
                    dr = selectCMD.ExecuteReader();
                    //if there are results
                    if (dr.HasRows)
                    {
                        //keep reading until the end
                        while (dr.Read())
                        {
                            txtDebtor.Text = dr["DebtorNumber"].ToString();
                            txtActionDate.Text = Convert.ToDateTime(dr["ActionDate"]).ToString("MM-dd-yyyy");
                            accountID = (long)dr["ID"];
                        }
                    }
                    selectCMD.Parameters.Clear();
                }
            }
            cnSQL.Close();
        }

        void GetRemainingTotals()
        {
            selectSQL = $@"SELECT COUNT(*) AS 'COUNT' FROM {dataTable}";

            using (cnSQL = new SqlConnection(sqlConnStr))
            {
                cnSQL.Open();
                using (selectCMD = new SqlCommand(selectSQL, cnSQL))
                {
                    dr = selectCMD.ExecuteReader();
                    //if there are results
                    if (dr.HasRows)
                    {
                        //keep reading until the end
                        while (dr.Read())
                        {
                            lblRemainingTotalCount.Text = dr["COUNT"].ToString();
                        }
                    }
                    selectCMD.Parameters.Clear();
                }
            }
            cnSQL.Close();
        }
        void UpdateCurrentAccount()
        {
            updateSQL = $@"UPDATE {dataTable} 
            SET COUNT(*) AS 'COUNT' FROM {dataTable}";




        }

        void StoreToDoneTable()
        {

        }


        public void DeleteAccount(string Table, Int64 id)
        {
            //delete current row ID from the database
            deleteSQL = $"DELETE FROM {Table} WHERE ID = @ID";

            using (cnSQL = new SqlConnection(sqlConnStr))
            {
                cnSQL.Open();
                using (deleteCMD = new SqlCommand(deleteSQL, cnSQL))
                {
                    deleteCMD.Parameters.Add("@ID", SqlDbType.BigInt).Value = id;
                    deleteCMD.ExecuteNonQuery();
                }
            }
            cnSQL.Close();
        }

        private void btnDone_Click(object sender, EventArgs e)
        {
            //delete the current account from the database
            DeleteAccount(dataTable, accountID);
            //get the next account to be worked
            PopulateForm();
        }
    }
}
