
using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using GalaSoft.MvvmLight.Command;

namespace WpfTestProject.UserControls
{
    /// <summary>
    /// InfoMatrix.xaml 的交互逻辑
    /// </summary>
    public partial class InfoMatrix : UserControl, INotifyPropertyChanged
    {
        public InfoMatrix()
        {
            InitializeComponent();
            //if (DesignerProperties.GetIsInDesignMode(this))
            //{
            //    Resources.Source = new Uri("pack://application:,,,component/Theme/default.xaml");
            //}

            PropertyChanged += OnPropertyChanged;
            NextPageCommand = new RelayCommand(() => Page++, () => Page + 1 < PageCount);
            PrevPageCommand = new RelayCommand(() => Page--, () => Page > 0);
            ButtonPrev.Command = PrevPageCommand;
            ButtonNext.Command = NextPageCommand;
        }

        private RelayCommand NextPageCommand { get; }
        private RelayCommand PrevPageCommand { get; }

        public Visibility PageControlVisible
        {
            get { return (Visibility)GetValue(PageControlVisibleProperty); }
            set { SetValue(PageControlVisibleProperty, value); }
        }

        /// <summary>
        ///     项目数量
        /// </summary>
        public int Count => ItemsSource.OfType<object>().Count();

        /// <summary>
        ///     每页项目数量
        /// </summary>
        public int PageSize => ColumnCount * RowCount;

        /// <summary>
        ///     页码 0-Indexed
        /// </summary>
        public int Page
        {
            get { return (int)GetValue(PageProperty); }
            set { SetValue(PageProperty, value); }
        }

        /// <summary>
        ///     列数
        /// </summary>
        public int ColumnCount
        {
            get { return (int)GetValue(ColumnCountProperty); }
            set { SetValue(ColumnCountProperty, value); }
        }

        /// <summary>
        ///     行数
        /// </summary>
        public int RowCount
        {
            get { return (int)GetValue(RowCountProperty); }
            set { SetValue(RowCountProperty, value); }
        }

        /// <summary>
        ///     页数
        /// </summary>
        public int PageCount
        {
            get
            {
                if (ItemsSource == null)
                    return 0;
                var pageSize = PageSize;
                if (pageSize == 0)
                    return 0;
                var count = Count;
                return count / pageSize + (count % pageSize > 0 ? 1 : 0);
            }
        }

        public IEnumerable ItemsSource
        {
            get { return (IEnumerable)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        /// <summary>
        ///     分页后项目
        /// </summary>
        public IEnumerable PagedItemSource
        {
            get { return ListBox.ItemsSource; }
            set { ListBox.ItemsSource = value; }
        }

        /// <summary>
        ///     项目模板选择器
        /// </summary>
        public DataTemplateSelector ItemTemplateSelector
        {
            get { return (DataTemplateSelector)GetValue(DataTemplateSelectorProperty); }
            set { SetValue(DataTemplateSelectorProperty, value); }
        }

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            NextPageCommand.RaiseCanExecuteChanged();
            PrevPageCommand.RaiseCanExecuteChanged();

            ButtonNext.Visibility = NextPageCommand.CanExecute(null) ? Visibility.Visible : Visibility.Hidden;
            ButtonPrev.Visibility = PrevPageCommand.CanExecute(null) ? Visibility.Visible : Visibility.Hidden;

            LabelPage.Content = PageCount == 1 ? null : $"{Page + 1}/{PageCount}";

            if (ItemsSource == null)
                return;
            var pageSize = PageSize;
            PagedItemSource =
                new ObservableCollection<object>(
                    ItemsSource.OfType<object>().Skip(Page * pageSize).Take(pageSize).ToList());
            if (e.PropertyName == nameof(ItemsSource))
                Page = 0;
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion INotifyPropertyChanged

        #region DependencyProperty

        public static readonly DependencyProperty PageControlVisibleProperty = DependencyProperty.Register(
            nameof(PageControlVisible), typeof(Visibility), typeof(InfoMatrix),
            new FrameworkPropertyMetadata(Visibility.Visible, FrameworkPropertyMetadataOptions.None,
                (d, a) => (d as InfoMatrix).PageControl.Visibility = (Visibility)a.NewValue));

        public static readonly DependencyProperty DataTemplateSelectorProperty = DependencyProperty.Register(
            nameof(ItemTemplateSelector), typeof(DataTemplateSelector), typeof(InfoMatrix)
        );

        public static readonly DependencyProperty ColumnCountProperty = DependencyProperty.Register(
            nameof(ColumnCount), typeof(int), typeof(InfoMatrix),
            new FrameworkPropertyMetadata(4, FrameworkPropertyMetadataOptions.None,
                (d, a) => (d as InfoMatrix).OnPropertyChanged(nameof(ColumnCount)),
                (d, o) =>
                {
                    var i = (int)o;
                    return i <= 0 ? 1 : o;
                }));

        public static readonly DependencyProperty RowCountProperty = DependencyProperty.Register(
            nameof(RowCount), typeof(int), typeof(InfoMatrix),
            new FrameworkPropertyMetadata(4, FrameworkPropertyMetadataOptions.None,
                (d, a) => (d as InfoMatrix).OnPropertyChanged(nameof(RowCount)), (d, o) =>
                {
                    var i = (int)o;
                    return i <= 0 ? 1 : o;
                }));

        public static readonly DependencyProperty PageProperty = DependencyProperty.Register(
            nameof(Page), typeof(int), typeof(InfoMatrix),
            new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.None,
                (d, a) => (d as InfoMatrix).OnPropertyChanged(nameof(Page)), (d, o) =>
                {
                    var i = (int)o;
                    if (i < 0)
                        return 0;
                    var m = d as InfoMatrix;
                    if (i > m.PageCount)
                        return m.PageCount;
                    return o;
                }));

        public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register(
            nameof(ItemsSource), typeof(IEnumerable), typeof(InfoMatrix),
            new FrameworkPropertyMetadata(null, (d, e) => (d as InfoMatrix).OnPropertyChanged(nameof(ItemsSource))));

        #endregion DependencyProperty
    }
}