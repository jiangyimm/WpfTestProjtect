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
using WpfTestProject.View;

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


            Applications = new ObservableCollection<ApplicationTile>();
            Applications.Add(new ApplicationTile() { Name = "news", Color = "#FF4DAEB5", View = new TestView(), Header = "RSS Feeds", Icon = "/Images/RSS feeds2.png" });
            Applications.Add(new ApplicationTile() { Name = "weather", Color = "#FF36377C", Icon = "/Images/Cloud Sun.png", Header = "71 Sunny", Description = "Wednesday, 60 Cloudy Thursday, 55 Sunny Friday, 75 Cloudy", View = new TestView() });
            Applications.Add(new ApplicationTile() { Name = "stock", Color = "#FFD68513", Icon = "/Images/Stock Index Up.png", View = new TestView(), Description = "3 Dow stocks that never discovered.", Header = "Dow Chemical." });
            Applications.Add(new ApplicationTile() { Name = "twitter", Color = "#FF555BBE", Icon = "/Images/Twitter.png", Header = "metro studio", Description = "Response to Metro Studio is more than imagined", View = new TestView() });
            Applications.Add(new ApplicationTile() { Name = "pictures", Color = "green", Icon = "/Images/photo.jpg", View = new TestView(), Header = "michael angela", Description = "April 15, 2012 Boston, London", SlideImage = "/Images/Interview4.jpg", CanSlide = true });
            Applications.Add(new ApplicationTile() { Name = "internet explorer", Color = "#FF02478A", Icon = "/Images/IE.png", View = new TestView() });
            Applications.Add(new ApplicationTile() { Name = "my computer", Color = "#FF9AB534", Icon = "/Images/Monitor.png", View = new TestView() });
            Applications.Add(new ApplicationTile() { Name = "store", Color = "#FF7D35B2", Icon = "/Images/Market.png", Header = "market place", View = new TestView() });
            Applications.Add(new ApplicationTile() { Name = "videos", Color = "#FF781768", View = new TestView(), SlideImage = "/Images/Chad.png", CanSlide = true, Icon = "/Images/Film.png", Description = "Syncfusion employees discuss the success of Metro Studio.", Header = "metro studio" });
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



        private ObservableCollection<ApplicationTile> apps;

        public ObservableCollection<ApplicationTile> Applications
        {
            get { return apps; }
            set
            {
                apps = value;
                RaisePropertyChanged();
            }
        }
    }
}