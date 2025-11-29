using Client.Models.File;
using Client.Services.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;

namespace Client.ViewModels.File
{
    public partial class FileListViewModel : ObservableObject
    {
        private readonly IFileService _fileService;
        private readonly IServiceProvider _sp;

        public ObservableCollection<FileItemViewModel> Files { get; } = new ObservableCollection<FileItemViewModel>();

        public FileListViewModel(IFileService fileService, IServiceProvider sp)
        {
            _fileService = fileService;
            _sp = sp;
        }

        [RelayCommand]
        public async Task LoadFilesAsync()
        {
            var list = await _fileService.GetFilesAsync();
            Files.Clear();
            foreach (var f in list)
            {
                // создаём FileItemViewModel через DI-резолвер, чтобы он мог получить IFileService
                Files.Add(new FileItemViewModel(f, _fileService));
            }
        }

        [RelayCommand]
        public async Task UploadAsync()
        {
            var dlg = new Microsoft.Win32.OpenFileDialog();
            if (dlg.ShowDialog() != true) return;

            var success = await _fileService.UploadFileAsync(dlg.FileName);
            if (!success) MessageBox.Show("Ошибка при загрузке");
            else
            {
                MessageBox.Show("Файл загружен");
                await LoadFilesAsync();
            }
        }
    }
}


