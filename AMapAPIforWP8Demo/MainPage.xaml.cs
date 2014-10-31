using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Xml.Linq;
using Microsoft.Phone.Controls;
using GestureEventArgs = System.Windows.Input.GestureEventArgs;

namespace AMapAPIforWP8Demo
{
    public partial class MainPage : PhoneApplicationPage
    {
        // 构造函数
        public MainPage()
        {
            InitializeComponent();

            Loaded += MainPage_Loaded;
        }

        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            if (listBox.ItemsSource != null)
                return;
            if (listBoxMap.ItemsSource != null)
            {
                return;
            }

            XElement root = XElement.Load("NaviMap.xml");

            List<NavigationModel> mapItems = LoadData(root);
            listBoxMap.ItemsSource = mapItems;
            listBoxMap.UpdateLayout();


            root = XElement.Load("NaviSearch.xml");
            List<NavigationModel> searchItems = LoadData(root);
            listBox.ItemsSource = searchItems;
            listBox.UpdateLayout();
        }

        private List<NavigationModel> LoadData(XElement root)
        {
            if (root == null)
                return null;

            IEnumerable<NavigationModel> items = from n in root.Elements("node")
                select new NavigationModel
                {
                    Title = (string) n.Attribute("title"),
                    ChildTitle = n.Attribute("childTitle") == null ? null : (string) n.Attribute("childTitle"),
                    Address =
                        n.Attribute("address") == null
                            ? null
                            : new Uri((string) n.Attribute("address"), UriKind.Relative),
                    Children = LoadData(n)
                };

            return items.ToList();
        }

        private void GridSearch_Tap(object sender, GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri((sender as Grid).Tag.ToString(), UriKind.Relative));
        }


        private void GridMap_Tap(object sender, GestureEventArgs e)
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