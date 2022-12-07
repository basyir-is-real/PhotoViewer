// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Imaging;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage.Pickers;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace PhotoViewer
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        ImagesRepository ImagesRepository { get; } = new();

        public MainWindow()
        {
            this.InitializeComponent();
            string folderPath = "C:\\Users\\Administrator.SHEERSSOLUTIONS\\source\\repos\\PhotoViewer\\Photos\\Resized";
            LoadImages(folderPath);
        }

        private void LoadImages(string folderPath)
        {
            ImagesRepository.GetImages(folderPath);
            var numImages = ImagesRepository.Images.Count();
            ImageInfoBar.Message = $"{numImages} have loaded";
            ImageInfoBar.IsOpen = true;
        }

        private async void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            FolderPicker folderPicker = new FolderPicker();
            folderPicker.FileTypeFilter.Add("*");

            var hwnd = WinRT.Interop.WindowNative.GetWindowHandle(this);
            WinRT.Interop.InitializeWithWindow.Initialize(folderPicker, hwnd);

            var folder = await folderPicker.PickSingleFolderAsync();

            if (folder != null)
            {
                LoadImages(folder.Path);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var imageInfo = ((sender as Button)?.DataContext as ImageInfo);

            if(imageInfo != null)
            {
                Image image = new();
                image.Source = new BitmapImage(new Uri(imageInfo.FullName, UriKind.Relative));
            }

        }
    }

    public class ImageInfo
    {
        public ImageInfo (string name, string fullName)
        {
            Name = name;
            FullName = fullName;

        }

        public string Name { get; }
        public string FullName { get; }


    }

    public class ImagesRepository
    {
        public ObservableCollection<ImageInfo> Images { get; } = new();

        public void GetImages(string folderPath)
        {
            Images.Clear();

            var di = new DirectoryInfo(folderPath);
            var files = di.GetFiles("*.jpg");
            foreach(var file in files)
            {
                Images.Add(new ImageInfo(file.Name, file.FullName));

            }

        }
    }
}
