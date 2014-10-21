using System;
using System.Collections.Generic;
using System.Windows;
using Microsoft.Phone.Controls;
using Com.AMap.Api.Maps;
using Com.AMap.Api.Maps.Model;
using System.Windows.Media;

namespace AMapAPIforWP8Demo.Samples.MapDemo
{
    /// <summary>
    /// 绘制大地曲线
    /// </summary>
    public partial class MapGeodesic : PhoneApplicationPage
    {
        AMap amap;
        LatLng startLatLng;
        LatLng endLatLng;
        int flag = 0;
        public MapGeodesic()
        {
            InitializeComponent();
            this.ContentPanel.Children.Add(amap = new AMap());
            this.amap.Loaded += amap_Loaded;
            amap.Tap += amap_Tap;
        }

        void amap_Loaded(object sender, RoutedEventArgs e)
        {
           
            this.Dispatcher.BeginInvoke(() =>
                {
                    List<LatLng> lnglats = new List<LatLng>();
                    lnglats.Add(new LatLng(38.4064, 75.5687));
                    lnglats.Add(new LatLng(39.8273, 116.8392));

                    amap.AddPolyline(new AMapPolylineOptions()
                    {
                        IsGeodesic = true,
                        Color = Colors.Blue,
                        Points = lnglats,
                        Width = 4,
                    });
                   
                });
            this.amap.MoveCamera(CameraUpdateFactory.NewLatLngZoom(amap.Center,3));
        }

        private void amap_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            LatLng latLng = amap.GetProjection().FromScreenLocation(e.GetPosition(amap));
            //if (startLatLng == null)
            //{
            flag++;
          
            if (flag%2==1)
            {
                startLatLng = latLng;
                //txtOrigin.Text = latLng.latitude + "/" + latLng.longitude;
                amap.AddMarker(new AMapMarkerOptions()
                {
                    Position = startLatLng,
                    Title = "Title",
                    Snippet = "Snippet",
                    IconUri = new Uri("Images/bus_start_pic.png", UriKind.Relative),
                });
            }
            else if(flag%2==0)
            {
                endLatLng = latLng;
                //txtDestination.Text = latLng.latitude + "/" + latLng.longitude;
                amap.AddMarker(new AMapMarkerOptions()
                {
                    Position = endLatLng,
                    Title = "Title",
                    Snippet = "Snippet",
                    IconUri = new Uri("Images/bus_end_pic.png", UriKind.Relative),
                });
                List<LatLng> lnglats = new List<LatLng>();
                lnglats.Add(startLatLng);
                lnglats.Add(endLatLng);

                amap.AddPolyline(new AMapPolylineOptions()
                {
                    IsGeodesic = true,
                    Color = Colors.Blue,
                    Points = lnglats,
                    Width = 4,
                });
                amap.AddPolyline(new AMapPolylineOptions()
                {
                    
                    Color = Colors.Red,
                    Points = lnglats,
                    Width = 4,
                });
            }
               
            //}
            //else if (endLatLng == null)
            //{
              
            //}
        }

        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            amap.Destory();
            base.OnBackKeyPress(e);
        }

    }
}