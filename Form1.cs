using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
namespace File_manager
{
    public partial class Form1 : Form
    {
        private string filePath = "D:";
        private bool isFile = false;
        private string currentlySelectedItemName = "";
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            filePathTextBox.Text = filePath;
            loadFilesAndDirectories();
        }

        public void loadFilesAndDirectories()
        {
            DirectoryInfo fileList;
            string tempfilePath = "";
            FileAttributes fileAttr;
            try
            {
                if (isFile)
                {
                    tempfilePath = filePath + "/" + currentlySelectedItemName;
                    FileInfo fileDetail = new FileInfo(tempfilePath);
                    filenameLabel.Text = fileDetail.Name;
                    filetypeLabel.Text = fileDetail.Extension;
                    fileAttr = File.GetAttributes(tempfilePath);
                    Process.Start(tempfilePath);

                }
                else
                {
                    fileAttr = File.GetAttributes(filePath);
                    
                }
                if ((fileAttr & FileAttributes.Directory) == FileAttributes.Directory)
                {
                    fileList = new DirectoryInfo(filePath);
                    FileInfo[] file = fileList.GetFiles(); //Get all the file
                    DirectoryInfo[] dirs = fileList.GetDirectories(); //Get all the directory
                    string fileExtension = "";
                    listView1.Items.Clear();

                    for (int i = 0; i < file.Length; i++)
                    {
                        fileExtension = file[i].Extension.ToUpper();
                        switch (fileExtension)
                        {
                            
                            case ".ZIP":
                                listView1.Items.Add(file[i].Name, 11);
                                break;
                            case ".MP3":
                            case ".MP2":
                            
                                listView1.Items.Add(file[i].Name, 2);
                                break;
                            
                            case ".PY":
                                listView1.Items.Add(file[i].Name, 6);
                                break;
                            case ".CS":
                                listView1.Items.Add(file[i].Name, 5);
                                break;
                            case ".EXE":
                            case ".COM":
                                listView1.Items.Add(file[i].Name, 3);
                                break;
                            case ".MP4":
                            case ".MKV":
                            case ".AVI":
                                listView1.Items.Add(file[i].Name, 0);
                                break;
                            case ".PDF":
                                listView1.Items.Add(file[i].Name, 9);
                                break;
                            default:
                                listView1.Items.Add(file[i].Name, 12);
                                break;

                        }
                        
                    }

                    for (int i = 0; i < dirs.Length; i++)
                    {
                        listView1.Items.Add(dirs[i].Name, 8);
                    }
                }
                else
                {
                    filenameLabel.Text = this.currentlySelectedItemName;
                }

            }
            catch (Exception e) 
            { 
            
            }
        }

        public void loadButtonAction()
        {
            removeBackSlash();
            filePath = filePathTextBox.Text;
            loadFilesAndDirectories();
            isFile = false;
        }
        public void removeBackSlash()
        {
            string path = filePathTextBox.Text;
            if(path.LastIndexOf("/") == path.Length -1)
            {
                filePathTextBox.Text =path.Substring(0, path.Length - 1);
            }
        }
        public void goBack()
        {
            try
            {
                removeBackSlash();
                string path = filePathTextBox.Text;
                path = path.Substring(0, path.LastIndexOf("/"));
                this.isFile = false;
                filePathTextBox.Text = path;
                removeBackSlash();
            }
            catch (Exception e)
            {

                
            }
        }
        private void openButton_Click(object sender, EventArgs e)
        {
            loadButtonAction();
        }

        private void listView1_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            currentlySelectedItemName = e.Item.Text;
            FileAttributes fileAttr = File.GetAttributes(filePath + "/"+ currentlySelectedItemName);
            if((fileAttr & FileAttributes.Directory) == FileAttributes.Directory)
            {
                isFile = false;
                filePathTextBox.Text = filePath+"/"+currentlySelectedItemName;
            }
            else
            {
                isFile= true;
            }
        }

        private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            loadButtonAction();
        }

        private void filePathTextBox_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void backButton_Click(object sender, EventArgs e)
        {
            goBack();
            loadButtonAction();
        }

        private void listView1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (listView1.FocusedItem.Bounds.Contains(e.Location) == true)
                {
                    contextMenuStrip1.Show(listView1, e.Location);
                }
            }
        }

        private void contextMenuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            switch (e.ClickedItem.Text)
            {
                case "Open":
                    
                    loadButtonAction();
                    break;
                case "Delete":
                    // Lấy item được chọn
                    var selectedItem = listView1.SelectedItems[0];

                    // Lấy đường dẫn tới file 
                    string tempfilePath = filePath + "/" + currentlySelectedItemName;

                    // Xóa item khỏi ListView
                    listView1.Items.Remove(selectedItem);

                    // Xóa file trong máy tính
                    if (File.Exists(tempfilePath))
                    {
                        File.Delete(tempfilePath);
                    }
                    break;
                case "Rename":
                    
                    // Lấy item được chọn
                    selectedItem = listView1.SelectedItems[0];
                    
                    
                    break;
                case "Copy":
                    // Thêm mã xử lý cho lựa chọn "Copy" tại đây
                    break;
                case "Cut(Move)":
                    // Thêm mã xử lý cho lựa chọn "Cut(Move)" tại đây
                    break;
            }
        }
    }
}
