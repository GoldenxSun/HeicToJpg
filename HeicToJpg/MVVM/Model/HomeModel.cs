using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HeicToJpg.Core;
using System.ComponentModel;
using System.Windows.Input;
using Microsoft.WindowsAPICodePack.Dialogs;
using System.IO;
using ImageMagick;
using System.Threading;

namespace HeicToJpg.MVVM.Model
{
    internal class HomeModel : ObservableObject
    {
        private bool cancel;

        private string _selectedSourceFolder;

        public string SelectedSourceFolder
        {
            get { return _selectedSourceFolder; }
            set
            {
                _selectedSourceFolder = value;
                OnPropertyChanged();
            }
        }

        private string _selectedDestinationFolder;

        public string SelectedDestinationFolder
        {
            get { return _selectedDestinationFolder; }
            set
            {
                _selectedDestinationFolder = value;
                OnPropertyChanged();
            }
        }

        private string _convertText;

        public string ConvertText
        {
            get { return _convertText; }
            set
            {
                _convertText = value;
                OnPropertyChanged();
            }
        }

        private string _stopText;

        public string StopText
        {
            get { return _stopText; }
            set
            {
                _stopText = value;
                OnPropertyChanged();
            }
        }

        private string _progressText;

        public string ProgressText
        {
            get { return _progressText; }
            set
            {
                _progressText = value;
                OnPropertyChanged();
            }
        }

        private double _progressBar;

        public double ProgressBar
        {
            get { return _progressBar; }
            set
            {
                _progressBar = value;
                OnPropertyChanged();
            }
        }

        public RelayCommand SelectSourceFolder { get; set; }
        public RelayCommand SelectDestinationFolder { get; set; }
        public RelayCommand Convert { get; set; }
        public RelayCommand Stop { get; set; }

        private string SelectFolder()
        {
            CommonOpenFileDialog openFileDialog = new CommonOpenFileDialog();
            openFileDialog.IsFolderPicker = true;

            if (openFileDialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                return openFileDialog.FileName;
            }

            return "Error";
        }

        private void ConvertImages()
        {
            if (Directory.Exists(SelectedDestinationFolder) && Directory.Exists(SelectedSourceFolder))
            {
                ConvertText = "Converting...";
                string[] allfiles = Directory.GetFiles(SelectedSourceFolder, "*.heic", SearchOption.AllDirectories);
                double procent = 0;
                double length = allfiles.Length;

                foreach (var file in allfiles)
                {
                    if (cancel) { break; }
                    FileInfo info = new FileInfo(file);
                    using (MagickImage image = new MagickImage(info.FullName))
                    {
                        image.Write(@$"{SelectedDestinationFolder}\\{info.Name.Split('.')[0]}.jpg");
                    }
                    int p = System.Convert.ToInt32(++procent / length * 100.0);
                    ProgressText = $"Progress {p}%";
                    ProgressBar = p * 4.1;
                }
            }
            Thread.Sleep(100);
            ConvertText = "Convert";
            ProgressText = cancel ? "Canceled" : "Done";
            ProgressBar = 0;
            cancel = false;
        }

        private void Default()
        {
            SelectedSourceFolder = "";
            SelectedDestinationFolder = "";
            ConvertText = "Convert";
            StopText = "Stop";
            ProgressText = "";
            ProgressBar = 0;
        }

        public HomeModel()
        {
            Default();

            SelectSourceFolder = new RelayCommand(o =>
            {
                SelectedSourceFolder = SelectFolder();
            });

            SelectDestinationFolder = new RelayCommand(o =>
            {
                SelectedDestinationFolder = SelectFolder();
            });

            Convert = new RelayCommand(o =>
            {
                new Thread(o => ConvertImages()).Start();
            });

            Stop = new RelayCommand(o =>
            {
                cancel = true;
            });
        }
    }
}