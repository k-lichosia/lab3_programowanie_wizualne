using System;
using System.Data;
using System.Windows.Forms;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace lab3_wizualne
{
    public partial class Form1 : Form
    {
        public string Imie = "";
        public string Nazwisko = "";
        public int Wiek = 0;
        public string Stanowisko = "";
        public int currentIndex = 3;

        private BindingSource bindingSource1 = new BindingSource();
        private DataGridView dataGridView1 = new DataGridView();
        private DataTable dataTable = new DataTable();
        public Form1()
        {
            InitializeComponent();

            dataTable.Columns.Add("ID", typeof(int));
            dataTable.Columns.Add("Imie", typeof(string));
            dataTable.Columns.Add("Nazwisko", typeof(string));
            dataTable.Columns.Add("Wiek", typeof(int));
            dataTable.Columns.Add("Stanowisko", typeof(string));
            dataTable.Rows.Add(1, "Jan", "Nowak", 26, "Pracownik");
            dataTable.Rows.Add(2, "Joanna", "Kowalska", 45, "Kierownik");
            bindingSource1.DataSource = dataTable;
            dataGridView1.Dock = DockStyle.Fill;
            dataGridView1.DataSource = bindingSource1;
            Controls.Add(dataGridView1);
        }
        public void Aktualizuj()
        {
            int lastIndex = 1;
            if (dataTable.Rows.Count > 0)
            {
                DataRow lastRow = dataTable.Rows[dataTable.Rows.Count - 1];
                string lastIndexString = lastRow["ID"].ToString();
                lastIndex = int.Parse(lastIndexString);
            }
            dataTable.Rows.Add(lastIndex + 1, Imie, Nazwisko, Wiek, Stanowisko);
        }

        private void ExportToCSV(DataGridView dataGridView, string filePath)
        {
            string csvContent = "Id,Imie,Nazwisko,Wiek,Stanowisko" + Environment.NewLine;
            foreach (DataGridViewRow row in dataGridView.Rows)
            {
                if (!row.IsNewRow)
                {
                    csvContent += string.Join(",", Array.ConvertAll(row.Cells.Cast<DataGridViewCell>()
                    .ToArray(), c => c.Value)) + Environment.NewLine;
                }
            }
            File.WriteAllText(filePath, csvContent);
        }

        private void LoadCSVToDataGridView(string filePath)
        {
            if (!File.Exists(filePath))
            {
                MessageBox.Show("Plik CSV nie istnieje.", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            string[] lines = File.ReadAllLines(filePath);
            dataTable = new DataTable();
            string[] headers = lines[0].Split(',');
            foreach (string header in headers)
            {
                dataTable.Columns.Add(header);
            }
            for (int i = 1; i < lines.Length; i++)
            {
                string[] values = lines[i].Split(',');
                dataTable.Rows.Add(values);
            }
            dataGridView1.DataSource = dataTable;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form2 secondForm = new Form2(this);
            secondForm.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                int index = dataGridView1.SelectedRows[0].Index;
                dataTable.Rows.RemoveAt(index);
            }
            else
            {
                MessageBox.Show("Proszę zaznaczyć wiersz do usunięcia.");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "Pliki CSV (*.csv)|*.csv|Wszystkie pliki (*.*)|*.*";
            saveFileDialog1.Title = "Wybierz lokalizację zapisu pliku CSV";
            saveFileDialog1.ShowDialog();
            if (saveFileDialog1.FileName != "")
            {
                ExportToCSV(dataGridView1, saveFileDialog1.FileName);
            }

        }

        private void button4_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "Pliki CSV (*.csv)|*.csv|Wszystkie pliki (*.*)|*.*";
            openFileDialog1.Title = "Wybierz plik CSV do wczytania";
            openFileDialog1.ShowDialog();
            if (openFileDialog1.FileName != "")
            {
                LoadCSVToDataGridView(openFileDialog1.FileName);
            }

        }

        private void button5_Click(object sender, EventArgs e)
        {
            List<Osoba> listaOsob = new List<Osoba>();

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (!row.IsNewRow)
                {
                    int id = int.Parse(row.Cells["ID"].Value.ToString());
                    string imie = row.Cells["Imie"].Value.ToString();
                    string nazwisko = row.Cells["Nazwisko"].Value.ToString();
                    int wiek = int.Parse(row.Cells["Wiek"].Value.ToString());
                    string stanowisko = row.Cells["Stanowisko"].Value.ToString();

                    listaOsob.Add(new Osoba(id, imie, nazwisko, wiek, stanowisko));
                }
            }

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Pliki JSON (*.json)|*.json|Wszystkie pliki (*.*)|*.*";
            saveFileDialog.Title = "Zapisz dane do pliku JSON";

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                string fileName = saveFileDialog.FileName;
                var options = new JsonSerializerOptions { WriteIndented = true };
                string jsonString = JsonSerializer.Serialize(listaOsob, options);
                File.WriteAllText(fileName, jsonString);

                MessageBox.Show("Dane zostały zapisane do pliku JSON.");
            }
        }
    }
}
