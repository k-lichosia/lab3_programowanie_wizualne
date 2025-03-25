using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace lab3_wizualne
{
    public partial class Form2 : Form
    {
        public Form1 _form1;

        public Form2(Form1 form1)
        {
            InitializeComponent();
            _form1 = form1;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(textBox3.Text, out int wiek))
            {
                MessageBox.Show("Wprowadź poprawny wiek!");
                return;
            }

            _form1.Imie = textBox1.Text;
            _form1.Nazwisko = textBox2.Text;
            _form1.Wiek = wiek;
            _form1.Stanowisko = comboBox1.SelectedItem?.ToString() ?? "";

            if (string.IsNullOrWhiteSpace(_form1.Imie) ||
                string.IsNullOrWhiteSpace(_form1.Nazwisko) ||
                string.IsNullOrWhiteSpace(_form1.Stanowisko) ||
                _form1.Wiek <= 0)
            {
                MessageBox.Show("Wszystkie pola muszą być poprawnie wypełnione.");
                return;
            }
            _form1.Aktualizuj();

            this.DialogResult = DialogResult.OK;
            this.Close();
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }


}

