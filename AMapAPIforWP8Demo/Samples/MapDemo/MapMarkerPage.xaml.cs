using System;
using System.Windows;
using Microsoft.Phone.Controls;
using Com.AMap.Api.Maps;
using Com.AMap.Api.Maps.Model;

namespace AMap_WP8_Api_Demos_v2._2.Samples
{
    public partial class MapMarkerPage : PhoneApplicationPage
    {
        AMap amap;
        AMapMarker marker;
        public MapMarkerPage()
        {
            InitializeComponent();
            this.ContentPanel.Children.Add(amap = new AMap());
            this.btnVisible.IsEnabled = false;
        }
        private void Button_drawMarker_Click(object sender, RoutedEventArgs e)
        {
            marker = amap.AddMarker(new AMapMarkerOptions()
            {
                Position=amap.Center,
                Title="Title",
                Snippet="Snippet",
                IconUri=new Uri ("Images/AZURE.png",UriKind.Relative),
            });
            this.btnVisible.IsEnabled = true;
        }

        private void Button_Destroy_Click(object sender, RoutedEventArgs e)
        {
            if (marker!=null)
            {
                marker.Destroy();
            }
            this.btnVisible.IsEnabled = false;
        }

        private void Button_Visible_Click(object sender, RoutedEventArgs e)
        {
            if (marker!=null)
            {
                marker.Visible = false;
            }
            this.btnVisible.IsEnabled = false;
        }

        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            amap.Destory();
            base.OnBackKeyPress(e);
        }

      
    }
}