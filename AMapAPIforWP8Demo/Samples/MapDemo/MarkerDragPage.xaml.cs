using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Microsoft.Phone.Controls;
using Com.AMap.Api.Maps;
using Com.AMap.Api.Maps.Model;
using System.Diagnostics;
using AMapAPIforWP8Demo;

namespace AMap_WP8_Api_Demos_v2._2.Samples.MapDemo
{
    public partial class MarkerDragPage : PhoneApplicationPage
    {
        AMap amap;

        AMapMarker aMapMarker1;

        AMapMarker aMapMarker2;
        AMapMarker aMapMarker3;
        AMapMarker aMapMarker4;
        AMapMarker aMapMarker5;

        public MarkerDragPage()
        {
            InitializeComponent();
            this.ContentPanel.Children.Add(amap = new AMap());
            amap.Loaded += amap_Loaded;
            amap.MarkerDragLister += amap_MarkerDragLister;//监听拖拽事件
            this.Unloaded += MarkerDragPage_Unloaded;
            amap.MarkerClickListener += amap_MarkerClickListener;
        }

      

        void MarkerDragPage_Unloaded(object sender, RoutedEventArgs e)
        {
            ////请确保Marker被销毁
            //if (amap == null)
            //{
            //    return;
            //}
            //foreach (var item in amap.GetMapMarkers())
            //{
            //    item.Destroy();
            //}
        }

        void amap_MarkerDragLister(AMapMarker sender, AMapMarkerEventArgs args)
        {
            this.Dispatcher.BeginInvoke(() =>
                {
                    txtMsg.Text = args.Status+"拖拽:" + args.Position.ToString();
                });
        }

        void amap_Loaded(object sender, RoutedEventArgs e)
        {
            this.amap.MoveCamera(CameraUpdateFactory.NewLatLngZoom(new LatLng(34.7466, 113.625367), 5));
            this.Dispatcher.BeginInvoke(() =>
                {
                    aMapMarker1 = amap.AddMarker(new AMapMarkerOptions()
                    {
                        Position = new LatLng(39.90403, 116.407525),//图标的位置
                        Title = "可拖拽点",
                        IconUri = new Uri("Images/AZURE.png", UriKind.Relative),//图标的URL
                        Anchor = new Point(0.5, 1),//图标中心点
                         IsDragable = true,//是否允许拖拽
                    });
                    aMapMarker2 = amap.AddMarker(new AMapMarkerOptions()
                    {
                        Position = new LatLng(34.7466, 113.625367),//图标的位置
                        IconUri = new Uri("Images/GREEN.png", UriKind.Relative),//图标的URL
                        Anchor = new Point(0.5, 1),//图标中心点
                         Title="点",
                        RotateAngle = 90,//旋转角度
                    });
                    aMapMarker3 = amap.AddMarker(new AMapMarkerOptions()
                    {
                        Position = new LatLng(31.238068, 121.501654),//图标的位置
                        Title = "上海",
                        Snippet = "31.238068, 121.501654",
                        IconUri = new Uri("Images/RED.png", UriKind.Relative),//图标的URL
                        Anchor = new Point(0.5, 1),//图标中心点
                    });
                    
                    //动画播放多图Marker
                    aMapMarker4 = amap.AddMarker(new AMapMarkerOptions()
                    {
                        Position = new LatLng(30.679879, 104.064855),
                        Anchor = new Point(0.5, 1),//图标中心点
                        Title="成都",
                        Snippet="30.679879, 104.064855",
                        IconUris = new List<Uri> { 
                        new Uri("Images/AZURE.png", UriKind.Relative),
                        new Uri("Images/RED.png", UriKind.Relative), 
                        new Uri("Images/ROSE.png", UriKind.Relative), 
                        new Uri("Images/BLUE.png", UriKind.Relative), 
                        new Uri("Images/CYAN.png", UriKind.Relative), 
                        new Uri("Images/GREEN.png", UriKind.Relative), 
                        new Uri("Images/MAGENTAV.png", UriKind.Relative), 
                        new Uri("Images/ORANGE.png", UriKind.Relative), 
                        new Uri("Images/VIOLET.png", UriKind.Relative), 
                        new Uri("Images/YELLOW.png", UriKind.Relative), 
                    },
                        Periods = 500,//刷新周期，单位毫秒
                    });
                    aMapMarker5 = amap.AddMarker(new AMapMarkerOptions()
                    {
                        Position = new LatLng(34.341568, 108.940174),//图标的位置
                        Title = "西安",
                        Snippet="34.341568, 108.940174",
                        IconUri = new Uri("Images/RED.png", UriKind.Relative),//图标的URL
                        Anchor = new Point(0.5, 1),//图标中心点
                    });
                  
                });
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("当前屏幕Markers1:"+amap.GetMapScreenMarkers().Count());
            LatLngBounds.Builder builder = new LatLngBounds.Builder();
            //builder.Include(aMapMarker1.Position);
            //builder.Include(aMapMarker2.Position);
            //builder.Include(aMapMarker3.Position);
            //builder.Include(aMapMarker4.Position);
            List<AMapMarker> markers = amap.GetMapMarkers();
            foreach (AMapMarker marker in markers)
            {
                builder.Include(marker.Position);
            }

            //this.amap.MoveCamera(CameraUpdateFactory.NewLatLngBoundsWithSize(builder.Build(), 800,800,markers.Count()));
            this.amap.MoveCamera(CameraUpdateFactory.NewLatLngBounds(builder.Build(), markers.Count()));

        }

        private void amap_MarkerClickListener(AMapMarker sender, AMapEventArgs args)
        {
            if (string.IsNullOrWhiteSpace(sender.Snippet)&&string.IsNullOrWhiteSpace(sender.Title))
                return;
            sender.ShowInfoWindow(new AInfoWindow() 
            {
                Title = sender.Title,
                ContentText = sender.Snippet,
            });
        }

        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            amap.Destory();
            base.OnBackKeyPress(e);
        }

    }
}