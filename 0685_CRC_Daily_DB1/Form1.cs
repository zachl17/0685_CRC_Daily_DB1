using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
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

        //sql tables
        string dataTable = "[dbo].[AllWorkers.CRCDaily_Data]";
        string doneDataTable = "[dbo].[AllWorkers.CRCDaily_Done]";
        string sqlConnStr = $"Data Source={testDataSource};Initial Catalog={testInitialCatalog};Integrated Security={integratedSecurity};MultipleActiveResultSets={multipleActiveResultSets};";

        public Form1()
        {
            InitializeComponent();
        }

        //Fills the form with data from the worker's table
        void PopulateForm()
        {

        }
    }
}
