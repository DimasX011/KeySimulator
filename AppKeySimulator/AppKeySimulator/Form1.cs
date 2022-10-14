using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using System.Threading;

namespace GtaAppKeySimulator
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            Test();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        public void Test()
        {
            while (true)
            {
                Thread.Sleep(5000);
                NativeMethods.AppClick("Калькулятор", "1");
            }
           
        }
    }
}
