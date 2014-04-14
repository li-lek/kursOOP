using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Threading;
using System.Data.SqlClient;
using System.Data.SqlServerCe;

namespace kursOOP
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // TODO: данная строка кода позволяет загрузить данные в таблицу "kursDataSet.Tovar". При необходимости она может быть перемещена или удалена.
            this.tovarTableAdapter.Fill(this.kursDataSet.Tovar);

        }

        private void del_Click(object sender, EventArgs e)
        {
            //пометка на удаление:
            tovarBindingSource.RemoveCurrent();
            //сохранение изменений:
            tovarBindingSource.EndEdit();
            //выгрузка в DataGridView обновленных данных:
            tovarTableAdapter.Update(this.kursDataSet.Tovar); 
        }

        private void add_Click(object sender, EventArgs e)
        {
            Add add = new Add();
            tovarBindingSource.AddNew();
            add.tovarBindingSource.DataSource = tovarBindingSource;
            add.tovarBindingSource.Position = tovarBindingSource.Position;
            if (add.ShowDialog()==System.Windows.Forms.DialogResult.OK)
                tovarTableAdapter.Update(this.kursDataSet.Tovar);
        }

        private void red_Click(object sender, EventArgs e)
        {
            Add red = new Add();
            red.tovarBindingSource.DataSource = tovarBindingSource;
            red.tovarBindingSource.Position = tovarBindingSource.Position;
            if (red.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                tovarTableAdapter.Update(this.kursDataSet.Tovar);
        }

        private void b4_Click(object sender, EventArgs e)
        {
            string str=""; string ctr="";
            for (int i = 0; i < dataGridView1.Rows.Count;i++ )
            {
                if ((DateTime)dataGridView1[2, i].Value < DateTime.Now.Date)
                    str += dataGridView1[1, i].Value.ToString()+ Environment.NewLine;
                if (Convert.ToInt32(dataGridView1[3, i].Value) == 0)
                    ctr += dataGridView1[1, i].Value.ToString() + Environment.NewLine;
               }
            if (str.Count()>1) MessageBox.Show("Имеются просроченные товары!" + Environment.NewLine + "наименование:" + Environment.NewLine + str);
            if (ctr.Count()>1) MessageBox.Show("Закончились товары!" + Environment.NewLine + "наименование:" + Environment.NewLine + ctr);
            this.Close();
        }

        private void price_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
            FileStream f = new FileStream(openFileDialog1.FileName, FileMode.Open);
            StreamReader rd = new StreamReader(f);
            string[] str;
            string inpstr;
            List<Tovar> tov = new List<Tovar>();
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                tov.Add(new Tovar(Convert.ToInt32(dataGridView1[0, i].Value), Convert.ToString(dataGridView1[1, i].Value), Convert.ToDateTime(dataGridView1[2, i].Value), Convert.ToInt32(dataGridView1[3, i].Value), 0));
            }
            while ((inpstr = rd.ReadLine()) != null)
            {
                str = inpstr.Split(' ');
                Tovar t = new Tovar(Convert.ToInt32(str[0]), str[1], Convert.ToDateTime(str[2]), Convert.ToInt32(str[3]), Convert.ToDecimal(str[4]));
                using (SqlCeConnection conn = new SqlCeConnection(@"Data Source=D:\kurs.sdf; Persist Security Info=False;"))
                {
                    conn.Open();
                    using (SqlCeCommand c = new SqlCeCommand(@"insert into tovar(name, srok, kolvo, price) values('" + t.name + "','"+t.srok.Date+"',"  + t.kolvo + "," + t.prise.ToString().Replace(',','.')+")"))
                    {
                        c.Connection = conn;
                        c.ExecuteNonQuery();
                    }
                }

            }
            rd.Close();
            this.tovarTableAdapter.Fill(this.kursDataSet.Tovar);
            tovarTableAdapter.Update(this.kursDataSet.Tovar);
        }

    }
}
