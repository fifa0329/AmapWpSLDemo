using System;
using System.Windows;
using Microsoft.Phone.Controls;
using Com.AMap.Api.Maps;
using Com.AMap.Api.Maps.Model;
using System.Diagnostics;
using System.Windows.Media;

namespace AMap_WP8_Api_Demos_v2._2.Samples
{
    /// <summary>
    /// 定位功能
    /// </summary>
    public partial class MyLocation : PhoneApplicationPage
    {
        AMap amap;
        //标注点
        AMapMarker marker;
        //圆
        AMapCircle circle;
        AMapGeolocator mylocation;

        LatLng location;

        public MyLocation()
        {
            InitializeComponent();
            //添加地图控件
            this.ContentPanel.Children.Add(amap = new AMap());
            this.Loaded += MyLocation_Loaded;
            this.Unloaded += MyLocation_Unloaded;
        }

        void MyLocation_Loaded(object sender, RoutedEventArgs e)
        {
            mylocation = new AMapGeolocator();
            mylocation.Start();
            //触发位置改变事件
            mylocation.PositionChanged += mylocation_PositionChanged;
          
        }

        void MyLocation_Unloaded(object sender, RoutedEventArgs e)
        {
            if (mylocation!=null)
            {
                mylocation.PositionChanged -= mylocation_PositionChanged;
                mylocation.Stop();
            } 
         
        }


        void mylocation_PositionChanged(AMapGeolocator sender, AMapPositionChangedEventArgs args)
        {
            location = args.LngLat;
            //todo 是否应该给用户直接转向UI线程??类似amap_CameraChangeListener
            this.Dispatcher.BeginInvoke(() =>
            {
                //GeoSearch(args.LngLat);

                if (marker == null)
                {
                    //添加圆
                    circle = amap.AddCircle(new AMapCircleOptions()
                     {
                         Center = args.LngLat,//圆点位置
                         Radius = (float)args.Accuracy,//半径
                         FillColor = Color.FromArgb(80, 100, 150, 255),
                         StrokeWidth = 2,//边框粗细
                         StrokeColor = Color.FromArgb(80, 0, 0, 255),//边框颜色

                     });

                    //添加点标注，用于标注地图上的点
                    marker = amap.AddMarker(
                   new AMapMarkerOptions()
                   {
                       Position = args.LngLat,//图标的位置
                       Title = "我的位置",
                       Snippet=args.LngLat.ToString(),
                       IconUri = new Uri("Images/marker_gps_no_sharing.png", UriKind.Relative),//图标的URL
                       Anchor = new Point(0.5, 0.5),//图标中心点

                   });
                }
                else
                {
                    //点标注和圆的位置在当前经纬度
                    marker.Position = args.LngLat;
                    circle.Center = args.LngLat;

                    circle.Radius = (float)args.Accuracy;//圆半径
                }

                //设置当前地图的经纬度和缩放级别
                amap.MoveCamera(CameraUpdateFactory.NewLatLngZoom(args.LngLat, 15));
                Debug.WriteLine("定位精度：" + args.Accuracy + "米");
                Debug.WriteLine("定位经纬度：" + args.LngLat);
            });
        }

        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            amap.Destory();
            base.OnBackKeyPress(e);
        }

      
    }
}