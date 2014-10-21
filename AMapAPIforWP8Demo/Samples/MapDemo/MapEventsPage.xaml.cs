using System;
using System.Windows;
using Microsoft.Phone.Controls;
using Com.AMap.Api.Maps;
using Com.AMap.Api.Maps.Model;

namespace AMapAPIforWP8Demo.Samples.MapDemo
{
    public partial class MapEventsPage : PhoneApplicationPage
    {
        AMap amap;
        AMapMarker marker;
        AInfoWindow infoWindow;

        public MapEventsPage()
        {
            InitializeComponent();
            this.ContentPanel.Children.Add(amap = new AMap());
            amap.Loaded += amap_Loaded;
            amap.Hold += amap_Hold;
            amap.Tap += amap_Tap;
            amap.MarkerClickListener += amap_MarkerClickListener;
            
            
        }

        void amap_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            marker = amap.AddMarker(new AMapMarkerOptions()
            {
                Position = amap.GetProjection().FromScreenLocation(e.GetPosition(amap)),
                Title = "Tap事件",
                Snippet = "Test",
                IconUri = new Uri("Images/ORANGE.png", UriKind.Relative),
                
            });
        }

        void amap_MarkerClickListener(AMapMarker sender, AMapEventArgs args)
        {
            
            sender.ShowInfoWindow(infoWindow=new AInfoWindow()
            {
                Title = sender.Title,
                ContentText = sender.Snippet,
            });
            infoWindow.Tap += infoWindow_Tap;
        }

        void infoWindow_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        { 
            AInfoWindow aInfo=sender as AInfoWindow;
            MessageBox.Show(aInfo.Title,"自定义窗体",MessageBoxButton.OK);
        }

        void amap_Hold(object sender, System.Windows.Input.GestureEventArgs e)
        {
            marker = amap.AddMarker(new AMapMarkerOptions()
            {
                Position =amap.GetProjection().FromScreenLocation(e.GetPosition(amap)),
                Title = "Hold事件",
                Snippet="Test",
                IconUri = new Uri("Images/AZURE.png", UriKind.Relative),
                
            });
        }

        void amap_Loaded(object sender, RoutedEventArgs e)
        {
            this.Dispatcher.BeginInvoke(() =>
                {
                    amap.AddMarker(new AMapMarkerOptions()
                     {
                         Position = amap.Center,
                         Title = "地图加载",
                         Snippet = "Test",
                         IconUri = new Uri("Images/RED.png", UriKind.Relative),

                     });
                });
            //infoWindow = new AInfoWindow();
            //infoWindow.Title = "这是自定义信息窗口";
            //infoWindow.ContentText = "marker的窗口";
            //marker.ShowInfoWindow(infoWindow, new Point(0, 0));
           // Debug.WriteLine("加载完成");
        }

        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            amap.Destory();
            base.OnBackKeyPress(e);
        }

    }
}