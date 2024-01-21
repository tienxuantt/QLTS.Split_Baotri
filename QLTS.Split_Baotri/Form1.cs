using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace QLTS.Split_Baotri
{
    public partial class Form1 : Form
    {
        private List<string> datas = new List<string>();
        private int MaxLength = 28000;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void btnStartSplit_Click(object sender, EventArgs e)
        {
            var sourcePath = txtSourcePath.Text;
            var destPath = txtOutputPath.Text;
            var maxlengthStr = txtMaxLenth.Text;

            datas = new List<string>();

            try
            {
                if (string.IsNullOrEmpty(sourcePath) || string.IsNullOrEmpty(destPath) || string.IsNullOrEmpty(maxlengthStr))
                {
                    MessageBox.Show("Thông tin không hợp lệ");
                }
                else
                {
                    MaxLength = int.Parse(maxlengthStr);

                    if (!Directory.Exists(destPath))
                    {
                        Directory.CreateDirectory(destPath);
                    }

                    var filesDelete = new DirectoryInfo(destPath).GetFiles();

                    foreach (var file in filesDelete)
                    {
                        file.Delete();
                    }

                    LoadFileData(sourcePath);
                    SaveFileOutput(destPath);

                    MessageBox.Show("Split thành công", "Thông báo");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Có lỗi xảy ra");
            }
        }

        private void LoadFileData(string inputFileName)
        {
            Thread.Sleep(500);

            string data = "";

            List<string> lines = File.ReadLines(inputFileName).ToList();

            foreach (var item in lines)
            {
                data = data + item + "\n";

                if (data.Length > MaxLength)
                {
                    datas.Add(data);
                    data = "";
                }
            }

            if (!string.IsNullOrEmpty(data))
            {
                datas.Add(data);
            }
        }

        private void SaveFileOutput(string outputFileName)
        {
            try
            {
                int index = 1;

                foreach (var item in datas)
                {
                    string fileName = string.Format("{0}/FileOutputSplit_{1}.txt", outputFileName, index++);

                    using (StreamWriter sw = File.CreateText(fileName))
                    {
                        sw.Write(item);
                    }
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message, "Có lỗi xảy ra");
            }
        }
    }
}
