using System;
using System.Collections.Generic;
using System.Windows;
using Microsoft.Phone.Controls;
using Com.AMap.Api.Maps;
using Com.AMap.Api.Maps.Model;
//using Com.AMap.Api.Maps.Com.AMap.Api.Maps;
using System.Windows.Media;

namespace AMap_WP8_Api_Demos_v2._2.Samples.MapDemo
{
    public partial class MapCalculatePage : PhoneApplicationPage
    {
        AMap amap;
        AMapLayer MapLayer = new AMapLayer();
        AMapMarker markerA1;
        AMapMarker markerA2;
        AMapMarker markerB1;
        AMapMarker markerB2;
        AMapMarker markerB3;
        AMapMarker markerB4;
        public MapCalculatePage()
        {
            InitializeComponent();
            this.ContentPanel.Children.Add(amap = new AMap());
            amap.AddAMapLayer(MapLayer);
            this.amap.Loaded += amap_Loaded;

        }

        void amap_Loaded(object sender, RoutedEventArgs e)
        {
            amap.MoveCamera(CameraUpdateFactory.NewLatLngZoom(new LatLng(39.963728, 116.453712), 13));
            this.Dispatcher.BeginInvoke(() =>
            {
                markerA1 = MapLayer.AddMarker(new AMapMarkerOptions()
                {
                    Position = new LatLng(39.987326, 116.48236),
                    Title = "这是一个markerA1",
                    IconUri = new Uri("Images/AZURE.png", UriKind.Relative),
                    Anchor = new Point(0.5, 1),//图标中心点
                });

                markerA2 = MapLayer.AddMarker(new AMapMarkerOptions()
                {
                    Position = new LatLng(39.93041, 116.450712),
                    Title = "这是一个markerA2",
                    IconUri = new Uri("Images/AZURE.png", UriKind.Relative),
                    Anchor = new Point(0.5, 1),
                });
              

                markerB1 = MapLayer.AddMarker(new AMapMarkerOptions()
                {
                    Position = new LatLng(39.963728, 116.438764),
                    Title = "这是一个markerB1",
                    IconUri = new Uri("Images/marker_gps_no_sharing.png", UriKind.Relative),
                    Anchor = new Point(0.5, 0.5),//图标中心点
                });

                markerB2 = MapLayer.AddMarker(new AMapMarkerOptions()
                {
                    Position = new LatLng(39.96041, 116.453712),
                    Title = "这是一个markerB2",
                    IconUri = new Uri("Images/marker_gps_no_sharing.png", UriKind.Relative),
                    Anchor = new Point(0.5, 0.5),//图标中心点
                });
                markerB3 = MapLayer.AddMarker(new AMapMarkerOptions()
                {
                    Position = new LatLng(39.963728, 116.453712),
                    Title = "这是一个markerB3",
                    IconUri = new Uri("Images/marker_gps_no_sharing.png", UriKind.Relative),
                    Anchor = new Point(0.5, 0.5),//图标中心点
                });

                markerB4 = MapLayer.AddMarker(new AMapMarkerOptions()
                {
                    Position = new LatLng(39.96041, 116.438764),
                    Title = "这是一个markerB4",
                    IconUri = new Uri("Images/marker_gps_no_sharing.png", UriKind.Relative),
                    Anchor = new Point(0.5, 0.5),//图标中心点
                });
                this.amap.AddAMapLayer(MapLayer);
               
            });
        }

        private void btnLineDistance_Click(object sender, RoutedEventArgs e)
        {
            this.Dispatcher.BeginInvoke(() =>
                {
                    List<LatLng> lineLatLng = new List<LatLng>();
                    lineLatLng.Add(markerA1.Position);
                    lineLatLng.Add(markerA2.Position);

                    AMapPolylineOptions opt = new AMapPolylineOptions()
                    {
                        Color = Colors.Red,
                        Points = lineLatLng,
                        Width = 5,
                    };
                    MapLayer.AddPolyline(opt);

                    float distance = AMapUtils.CalculateLineDistance(markerA1.Position, markerA2.Position);
                    txtMsg.Text = string.Format("距离：{0}米", distance);
                });
        }

        private void btnArea_Click(object sender, RoutedEventArgs e)
        {
            this.Dispatcher.BeginInvoke(() =>
                {
                    float distance = AMapUtils.CalculateArea(markerB1.Position, markerB2.Position);
                    txtMsg.Text = string.Format("面积：{0}平方米", distance);

                    List<LatLng> latlngs = new List<LatLng>();
                    latlngs.Add(markerB1.Position);
                   
                    latlngs.Add(markerB3.Position);
                    latlngs.Add(markerB2.Position);
                    latlngs.Add(markerB4.Position);

                    MapLayer.AddPolygon(new AMapPolygonOptions()
                    {
                        Points = latlngs,
                        StrokeWidth = 3,
                        StrokeColor = Colors.Orange,
                    });
                });
        }

        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            amap.Destory();
            base.OnBackKeyPress(e);
        }

      
    }
}