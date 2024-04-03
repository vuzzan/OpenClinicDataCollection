using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OpenClinicDataCollection
{
    public partial class frmPreview : Form
    {
        public frmPreview()
        {
            InitializeComponent();
        }

        public void ShowDataGridView(object data)
        {
            try {
                var jsonDataResult = JsonConvert.SerializeObject(data);
                var array = JsonConvert.DeserializeObject<object>(jsonDataResult);

                //foreach (object element in array)
                //{
                //    etc...
                //}

                DataGridView dataGridView1 = new DataGridView();
                dataGridView1.Dock = DockStyle.Fill;
                Controls.Add(dataGridView1);

                dataGridView1.Columns.Add("CS", "Chỉ số");
                dataGridView1.Columns.Add("KQ", "Kết quả");

                //for (int i = 0; i < data.GetLength(0); i++)
                //{
                //    dataGridView1.Rows.Add(new string[] { data[i, 0], data[i, 1], data[i, 2] });
                //}
            }catch (Exception ex)
            {
                throw ex;
            }


        }
        public class Data
        {
            public string BN { get; set; }
            public string DATA { get; set; }
        }
    }
}
