using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Windows.Forms;
using System.Xml;

namespace Laba1
{
    public partial class Form1 : Form
    {
        public static Dictionary<string, string> dictionary = new Dictionary<string, string>();
        private static Form1 instance;

        public static Form1 GetInstance()
        {
            if (instance == null)
                instance = new Form1();
            return instance;
        }

        public void Fooo(string value) {
            MessageBox.Show(value);
        }

        public string CellNameToCellValue(string name)
        {
            return dictionary[name];
        }
        public bool ContainsKey(string key)
        {
            string outist;
            return dictionary.TryGetValue(key, out outist);
        }

        public Form1()
        {
            InitializeComponent();
        }

        private void DataGridInit(int rows, int columns)
        {
            for (int i = 0; i < columns; ++i)
            {
                DataGridViewTextBoxColumn A = new DataGridViewTextBoxColumn();
                A.HeaderText = (Convert.ToChar(65 + i).ToString());
                A.Name = A.HeaderText;
                dataGridView1.Columns.Add(A);
            }
        }

        private void ExportDgvToXML()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "MyExcelFiles|*.txt";
            saveFileDialog.Title = "Save an *txt file";
            if (saveFileDialog.ShowDialog() != DialogResult.OK)
                return;
            FileStream fs = (FileStream)saveFileDialog.OpenFile();
            StreamWriter sw = new StreamWriter(fs);
            sw.WriteLine(dataGridView1.ColumnCount);
            sw.WriteLine(dataGridView1.RowCount);

            foreach (DataGridViewRow dr in dataGridView1.Rows)
            {
                foreach (DataGridViewCell dc in dr.Cells)
                {
                    sw.WriteLine(dc.Value);
                }
            }

            sw.Close();
            fs.Close();
            MessageBox.Show("Проізошла вигрузка");
        }

        private void ImportDgvFromXML()
        {
            OpenFileDialog oFD = new OpenFileDialog();
            oFD.Filter = "MyExcelFiles|*.txt";
            oFD.Title = "Open an *.txt file";
            if (oFD.ShowDialog() != DialogResult.OK) 
                return;
            StreamReader sw = new StreamReader(oFD.FileName);
            dataGridView1.Columns.Clear();
            dataGridView1.Rows.Clear();
            int columnNum = int.Parse(sw.ReadLine());
            int rowNum = int.Parse(sw.ReadLine());
            DataGridInit(rowNum, columnNum);
            for (int i = 0; i < rowNum; ++i)
            {
                DataGridViewRow dr = new DataGridViewRow();

                for (int j = 0; j < columnNum; ++j)
                {
                    var value = sw.ReadLine();
                    dr.Cells.Add(new DataGridViewTextBoxCell { Value = value });
                    dictionary[dataGridView1.Columns[j].Name + i.ToString()] = value;
                }
                dataGridView1.Rows.Add(dr);
                dataGridView1.Rows[dataGridView1.RowCount - 2].HeaderCell.Value = (dataGridView1.RowCount - 1).ToString();
            }
            sw.Close();
        }

    private void Form1_Load(object sender, EventArgs e)
    {
        for (char i = 'A'; i <= 'F'; i++)
        {
            dataGridView1.Columns.Add(Convert.ToString(i), Convert.ToString(i));
        }

        for (int i = 0; i < 5; i++)
        {
            dataGridView1.Rows.Add();
            dataGridView1.Rows[dataGridView1.RowCount - 2].HeaderCell.Value = (i + 1).ToString();
        }
    }


        private void button1_Click_1(object sender, EventArgs e)
        {
            dataGridView1.Rows.Add();
            dataGridView1.Rows[dataGridView1.RowCount - 2].HeaderCell.Value = (dataGridView1.RowCount - 1).ToString();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (dataGridView1.RowCount - 2 <= 0)
            {
                MessageBox.Show("Та вже нікуди ті рядочки видаляти!");
                return;
            }
             dataGridView1.Rows.RemoveAt(dataGridView1.RowCount - 2);
        }

        private void button3_Click(object sender, EventArgs e)
        {
           if (dataGridView1.Columns[dataGridView1.ColumnCount - 1].Name[0] >= 'Z')
            {
                MessageBox.Show("МАКСИМУМ КОЛОНОЧОК!!!");
                return;
            }

            dataGridView1.Columns.Add(Convert.ToString((char)(dataGridView1.Columns[dataGridView1.ColumnCount - 1].Name[0] + 1)),
                Convert.ToString((char)(dataGridView1.Columns[dataGridView1.ColumnCount - 1].Name[0] + 1)));
        }


        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                foreach(DataGridViewCell cell in dataGridView1.SelectedCells)
                {
                    cell.Value = DBNull.Value;
                }
            }
            catch
            {
                MessageBox.Show("НЕМОЖЛИВО ВИДАЛИТИ КОЛОН ОЧКУ!!!");
            }
        }

        private void Button6_Click(object sender, EventArgs e)
        {
            if (dataGridView1.ColumnCount - 1 <= 0)
            {
                MessageBox.Show("Стовпчики закінчились :с");
                return;
            }
            
            dataGridView1.Columns.RemoveAt(dataGridView1.ColumnCount - 1);
        }

        private string _expressionValue = string.Empty;

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            _expressionValue = textBox1.Text;    
        }

        private void button7_Click(object sender, EventArgs e)
        {
            try
            {
                if (textBox1.Text == string.Empty)
                {
                    MessageBox.Show("Введіть вираз, буласка");
                    return;
                }

                foreach (DataGridViewCell cell in dataGridView1.SelectedCells)
                {
                    cell.Value = Calculator.Evaluate(textBox1.Text);
                    string cellName = dataGridView1.Columns[cell.ColumnIndex].Name[0].ToString();
                    cellName += cell.RowIndex + 1;
                    dictionary[cellName] = cell.Value.ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.CurrentCell.Value == null)
                return;

            string s = dataGridView1.Columns[dataGridView1.CurrentCell.ColumnIndex].Name[0].ToString();
            s += (dataGridView1.CurrentCell.RowIndex + 1);

            dictionary[s] = dataGridView1.CurrentCell.Value.ToString();
        }


        private void button7_Click_1(object sender, EventArgs e)
        {
            ExportDgvToXML();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            ImportDgvFromXML();
        }
    }
}

