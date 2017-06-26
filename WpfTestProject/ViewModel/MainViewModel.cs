using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Newtonsoft.Json;

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using GalaSoft.MvvmLight.Views;
using Microsoft.Practices.ServiceLocation;
using WpfTestProject.Models;

namespace WpfTestProject.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel()
        {
            ////if (IsInDesignMode)
            ////{
            ////    // Code runs in Blend --> create design time data.
            ////}
            ////else
            ////{
            ////    // Code runs "for real"
            ////}
            var jsonStr = Encoding.Default.GetString(File.ReadAllBytes("F:\\Tools\\citisjson.txt"));
            var listCitis = JsonConvert.DeserializeObject<List<Citis>>(jsonStr);
            var confirmCommand = new RelayCommand<Info>(Confirm);

            var list = listCitis.Select(p => new Info
            {
                Title = p.name,
                Tag = null,
                ConfirmCommand = confirmCommand
            }
            );

            Data = new ObservableCollection<Info>(list);
        }

        private void Confirm(Info obj)
        {
           MessageBox.Show($"—°‘Ò¡À{obj.Title}","HEHE");
            // ServiceLocator.Current.GetInstance<INavigationService>().NavigateTo("");
        }

        private ObservableCollection<Info> _data;

        public ObservableCollection<Info> Data
        {
            get { return _data; }
            set
            {
                _data = value;
                RaisePropertyChanged();
            }
        }
    }
}