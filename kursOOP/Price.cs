using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace kursOOP
{
    public class Tovar
    {
        public int id { get; set; }
        public string name { get; set; }
        public DateTime srok { get; set; }
        public int kolvo { get; set; }
        public decimal prise { get; set; }
        public Tovar(int i, string n, DateTime s, int k, decimal p)
        {
            id = i;
            name = n; srok = s; kolvo = k; prise = p;
        }
    }
}
