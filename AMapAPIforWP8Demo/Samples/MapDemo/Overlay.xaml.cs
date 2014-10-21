using System;
using System.Collections.Generic;
using System.Windows;
using Microsoft.Phone.Controls;
using Com.AMap.Api.Maps;
using Com.AMap.Api.Maps.Model;
using System.Windows.Media;

namespace AMapAPIforWP8Demo.Samples.MapDemo
{
    public partial class Overlay : PhoneApplicationPage
    {
        AMap amap;
        AMapPolygon polygon;
        AMapCircle circle;

        public Overlay()
        {
            InitializeComponent();
            this.ContentPanel.Children.Add(amap = new AMap());
            this.amap.Loaded += amap_Loaded;
            //this.amap.Tap += amap_Tap;
            amap.MarkerClickListener += amap_MarkerClickListener;
            this.Loaded += Overlay_Loaded;
        }

        void Overlay_Loaded(object sender, RoutedEventArgs e)
        {
            
        }

        void amap_MarkerClickListener(AMapMarker sender, AMapEventArgs args)
        {
            if (circle.IsContains( sender.Position))
            {
                sender.ShowInfoWindow(new AInfoWindow()
                {
                    Title="点",
                     ContentText="在圆内",
                });
            }
           
           else if (polygon.IsContains(sender.Position))
            {
                sender.ShowInfoWindow(new AInfoWindow()
                {
                    Title = "点",
                    ContentText = "在多边形内",
                });
            }
            else
            {
                sender.ShowInfoWindow(new AInfoWindow()
                {
                    Title = "点",
                    ContentText = "在覆盖物外",
                });
            }
        }

        void amap_Loaded(object sender, RoutedEventArgs e)
        {
            amap.MoveCamera(CameraUpdateFactory.ZoomBy(2));
            this.Dispatcher.BeginInvoke(() =>
                {
                    LatLng lng = new LatLng(39.97803, 116.407525);
                    //绘多边形
                    List<LatLng> lnglats = new List<LatLng>();
                    lnglats.Add(new LatLng(lng.latitude + 0.02, lng.longitude + 0.03));
                    lnglats.Add(new LatLng(lng.latitude + 0.03, lng.longitude - 0.03));
                    lnglats.Add(new LatLng(lng.latitude - 0.026, lng.longitude - 0.03));
                    lnglats.Add(lng);
                    lnglats.Add(new LatLng(lng.latitude - 0.04, lng.longitude + 0.035));
                   polygon=  amap.AddPolygon(new AMapPolygonOptions()
                    {
                        FillColor = Color.FromArgb(30, 255, 0, 255),
                        Points = lnglats,
                        StrokeColor = Color.FromArgb(255, 102, 136, 255),
                        StrokeWidth = 2
                    });
                    //绘制多边形
                    circle= amap.AddCircle(new AMapCircleOptions()
                     {
                         Center = new LatLng(39.88600, 116.407525),//圆点位置
                         Radius = 4000,//半径
                         FillColor = Color.FromArgb(70, 100, 150, 255),
                         StrokeWidth = 3,
                         StrokeColor = Color.FromArgb(255, 0, 0, 255)
                     });

                    amap.AddMarker(new AMapMarkerOptions()
                    {

                        Position = new LatLng(39.917545, 116.354774),
                        IconUri = new Uri("Images/ROSE.png", UriKind.Relative),
                        //Anchor = new Point(1, 1),//图标中心点
                    });
                    amap.AddMarker(new AMapMarkerOptions()
                    {

                        Position = new LatLng(39.886349, 116.397011),
                        IconUri = new Uri("Images/ROSE.png", UriKind.Relative),
                        //Anchor = new Point(1, 1),//图标中心点
                    });
                    amap.AddMarker(new AMapMarkerOptions()
                    {

                        Position = new LatLng(39.954784, 116.409668),
                        IconUri = new Uri("Images/ROSE.png", UriKind.Relative),
                        //Anchor = new Point(1, 1),//图标中心点
                    });
                    amap.AddMarker(new AMapMarkerOptions()
                    {

                        Position = new LatLng(39.98867, 116.402168),
                        IconUri = new Uri("Images/ROSE.png", UriKind.Relative),
                        //Anchor = new Point(1, 1),//图标中心点
                    });
                });
        }

        //void amap_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        //{
        //    this.Dispatcher.BeginInvoke(() =>
        //        {
        //             amap.AddMarker(new AMapMarkerOptions()
        //            {
        //                Position = amap.GetProjection().FromScreenLocation(e.GetPosition(amap)),
        //                IconUri = new Uri("Images/AZURE.png", UriKind.Relative),
        //                Anchor = new Point(0.5, 1),//图标中心点
        //            });
        //        });
        //}

        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            amap.Destory();
            base.OnBackKeyPress(e);
        }

        
    }
}