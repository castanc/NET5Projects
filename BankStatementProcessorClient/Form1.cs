using BSPFacade;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Utils;

namespace BankStatementProcessorClient
{
    public partial class Form1 : Form
    {
        public Service service = new Service();
        public Form1()
        {
            InitializeComponent();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            string path = @"C:\Users\cesar\OneDrive\Documents\Bancos\Santander";
            string sourceFile = @"C:\Users\cesar\OneDrive\Documents\Bancos\Santander\Santander_2020_08.csv";
            var result = await service.ConvertToTSV(sourceFile, Encoding.UTF7);

            Clipboard.SetText(result);
        }

        private async void btnExcel_Click(object sender, EventArgs e)
        {
            string sourceFile = @"C:\Users\cesar\OneDrive\Documents\Bancos\Santander\Santander_2020_08.csv";
            sourceFile = @"C:\Users\cesar\OneDrive\Documents\Bancos\BROU\BROU-CCY-20180801-20181231.xls";
            sourceFile = @"C:\Users\cesar\OneDrive\Documents\Bancos\Santander\Santander_2020_08.csv";
            var result = await service.ConvertExcelToTSV(sourceFile);
            Clipboard.SetText(result);
        }
    }
}
