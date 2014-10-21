using System;
using System.Windows;
using Microsoft.Phone.Controls;
using Com.AMap.Api.Maps;
using Com.AMap.Api.Maps.Model;

namespace AMapAPIforWP8Demo.Samples.MapDemo
{
    public partial class Animate : PhoneApplicationPage
    {
        AMap amap;
        AMapMarker marker1, marker2;
        public Animate()
        {
            InitializeComponent();
            this.ContentPanel.Children.Add(amap = new AMap());
            this.amap.Loaded += amap_Loaded;
        }

        void amap_Loaded(object sender, RoutedEventArgs e)
        {
            this.Dispatcher.BeginInvoke(() =>
         {
             //添加点
             marker1 = amap.AddMarker(new AMapMarkerOptions()
                    {
                        Position = new LatLng(39.90403, 116.407525),
                        Title = "这是一个marker",

                        Anchor = new Point(0.5, 1),//图标中心点
                        IconUri = new Uri("Images/AZURE.png", UriKind.Relative),
                    });
             marker2 = amap.AddMarker(new AMapMarkerOptions()
              {
                  Position = new LatLng(30.679879, 104.064855),
                  Title = "这是一个marker",

                  Anchor = new Point(0.5, 1),//图标中心点
                  IconUri = new Uri("Images/RED.png", UriKind.Relative),
              });
         });
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            amap.AnimateCamera(CameraUpdateFactory.NewLatLngZoom(marker2.Position, 20), 1);

            //LatLngBounds bounds=new LatLngBounds(new LatLng(45,120),new LatLng(46,121) );
            //todo 0721曹光健的问题 出现点
           //amap.AnimateCamera(CameraUpdateFactory.NewLatLngBounds(bounds,0), 1);


        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            amap.StopAnimation();
        }

        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            amap.Destory();
            base.OnBackKeyPress(e);
        }

    }
}