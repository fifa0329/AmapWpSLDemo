using System;
using System.Collections.Generic;
using System.Windows;
using Microsoft.Phone.Controls;
using Com.AMap.Api.Maps;
using Com.AMap.Api.Maps.Model;
using System.Windows.Media;


namespace AMap_WP8_Api_Demos_v2._2.Samples
{
    public partial class ZIndexPage : PhoneApplicationPage
    {
        AMap amap;
        AMapCircle circle;
        AMapPolyline polyline;
        AMapLayer mapLayer;
        AMapMarker marker1;
        AMapMarker marker2;
        bool falg = true;
        public ZIndexPage()
        {
            InitializeComponent();
            this.ContentPanel.Children.Add(amap = new AMap());
            amap.Loaded += amap_Loaded;
            amap.AddAMapLayer(mapLayer= new AMapLayer());
        }

        void amap_Loaded(object sender, RoutedEventArgs e)
        {
            
            this.Dispatcher.BeginInvoke(() => 
            {
                amap.MoveCamera(CameraUpdateFactory.NewLatLngZoom(amap.Center, 12));
                List<LatLng> lnglats = new List<LatLng>();
                lnglats.Add(new LatLng(amap.Center.latitude + 0.03, amap.Center.longitude + 0.04));
                lnglats.Add(new LatLng(amap.Center.latitude - 0.03, amap.Center.longitude - 0.04));

                //绘圆
                circle = mapLayer.AddCircle(new AMapCircleOptions()
                {
                    Center = amap.Center,
                    Radius = 2000,
                    FillColor = Color.FromArgb(80, 0, 0, 255),
                    ZIndex = 5,
                });

                //绘线
                polyline = mapLayer.AddPolyline(new AMapPolylineOptions()
                {
                    Points = lnglats,
                    Width = 5,
                    Color = Color.FromArgb(255, 255, 0, 0),
                    ZIndex = 8,
                });

                marker1 = mapLayer.AddMarker(new AMapMarkerOptions()
                {
                    Position = amap.Center,
                    Title = "Title",
                    Snippet = "Snippet",
                    IconUri = new Uri("Images/AZURE.png", UriKind.Relative),
                    
                });
                marker2 = mapLayer.AddMarker(new AMapMarkerOptions()
                {
                    Position = amap.Center,
                    Title = "Title",
                    Snippet = "Snippet",
                    IconUri = new Uri("Images/RED.png", UriKind.Relative),
                    Anchor=new Point (1,1),
                    
                });
            });
           
        }

       
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (falg)
            {
                //改变顺序
                circle.ZIndex = 3;
                polyline.ZIndex = 15;
                marker1.ZIndex = 20;
                marker2.ZIndex = 8;
                falg = false;
            }
            else
            {
                //改变顺序
                circle.ZIndex = 15;
                polyline.ZIndex = 3;
                marker1.ZIndex = 5;
                marker2.ZIndex = 20;
                falg = true;
            }
            
        }

        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            amap.Destory();
            base.OnBackKeyPress(e);
        }


    }
}