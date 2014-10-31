using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using System.Xml.Linq;

namespace AMapAPIforWP8Demo
{
    public partial class MainPage : PhoneApplicationPage
    {

        // 构造函数
        public MainPage()
        {
            InitializeComponent();

            this.Loaded += MainPage_Loaded;
        }
        void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            if (listBox.ItemsSource != null)
                return;
            if (listBoxMap.ItemsSource != null)
            {
                return;
            }

            XElement root = XElement.Load("NaviMap.xml");

            var mapItem = LoadData(root);
            listBoxMap.ItemsSource = mapItem;
            listBoxMap.UpdateLayout();


            root = XElement.Load("NaviSearch.xml");
            var items = LoadData(root);
            listBox.ItemsSource = items;
            listBox.UpdateLayout();

        }

        private List<NavigationModel> LoadData(XElement root)
        {
            if (root == null)
                return null;

            var items = from n in root.Elements("node")
                        select new NavigationModel
                        {
                            Title = (string)n.Attribute("title"),
                            ChildTitle = n.Attribute("childTitle") == null ? null : (string)n.Attribute("childTitle"),
                            Address = n.Attribute("address") == null ? null : new Uri((string)n.Attribute("address"), UriKind.Relative),
                            Children = LoadData(n)
                        };

            return items.ToList();
        }

        private void Grid_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri((sender as Grid).Tag.ToString(), UriKind.Relative));
        }


        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
        }

        private void GridMap_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri((sender as Grid).Tag.ToString(), UriKind.Relative));
        }

    }

    public class NavigationModel
    {
        public string Title { get; set; }
        public string ChildTitle { get; set; }
        public Uri Address { get; set; }
        public List<NavigationModel> Children { get; set; }
    }
}