using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Com.AMap.Api.Services;
using Com.AMap.Api.Services.Results;
using Microsoft.Phone.Controls;
using Com.AMap.Api.Maps;
using Com.AMap.Api.Maps.Model;
using System.Diagnostics;
using System.Windows.Media;

namespace AMapAPIforWP8Demo.Samples.SearchDemo
{
    /// <summary>
    /// 公交线路查询
    /// </summary>
    public partial class BusSearchofLine : PhoneApplicationPage
    {
        AMap amap;
        public BusSearchofLine()
        {
            InitializeComponent();
            this.ContentPanel.Children.Add(amap = new AMap());

        }

        private async void GetBusLineKeyWords(string keywords, string city, uint offset, uint page)
        {
                AMapBusLineResults busLines = await AMapBusSearch.BusLineKeyWords(keywords, offset, page, city, Extensions.All);

               
               this.Dispatcher.BeginInvoke(() =>
                     {
                         if (busLines.Erro == null && busLines.BusLineList != null)
                         {
                             if (busLines.BusLineList.Count == 0)
                             {
                                 MessageBox.Show("无查询结果");
                                 return;
                             }

                             IEnumerable<AMapBusLine> bLines = busLines.BusLineList;
                             //this.Dispatcher.BeginInvoke(() =>
                             //{

                             amap.AddMarker(new AMapMarkerOptions()
                             {
                                 Position = new LatLng(bLines.FirstOrDefault().Bus_stops[0].Location.Lat, bLines.FirstOrDefault().Bus_stops[0].Location.Lon),
                                 Title = "Title",
                                    Snippet = "Snippet",
                                 IconUri = new Uri("Images/bus_start_pic.png", UriKind.Relative),
                             });
                             //终点站
                             amap.AddMarker(new AMapMarkerOptions()
                             {
                                 Position = new LatLng(bLines.FirstOrDefault().Bus_stops.Last().Location.Lat, bLines.FirstOrDefault().Bus_stops.Last().Location.Lon),
                                 Title = "Title",
                                 Snippet = "Snippet",
                                 IconUri = new Uri("Images/bus_end_pic.png", UriKind.Relative),
                             });

                             //});
                             //显示第一条公交路线

                             //起始站

                             //公交路线
                             List<LatLng> lnglats = new List<LatLng>();
                             lnglats = latLagsFromString(bLines.FirstOrDefault().Polyline);
                             //绘制公交路线
                             //this.Dispatcher.BeginInvoke(() =>
                             //    {
                             amap.AddPolyline(new AMapPolylineOptions()
                             {
                                 Points = lnglats,
                                 Color = Color.FromArgb(255, 0, 0, 255),
                                 Width = 2,
                             });
                             //});
                             List<LatLng> latlngs = new List<LatLng>();
                             //添加途径公交站
                             int j = 0;
                             foreach (AMapBusStop bs in bLines.FirstOrDefault().Bus_stops)
                             {
                                 j++;
                                 latlngs.Add(new LatLng(bs.Location.Lat, bs.Location.Lon));

                             }
                             Debug.WriteLine("途径公交站总数" + j);
                             //去除起始站和终点站
                             latlngs.RemoveAt(0);
                             latlngs.RemoveAt(latlngs.Count - 1);

                             //this.Dispatcher.BeginInvoke(() =>
                             //    {
                             foreach (LatLng latlng in latlngs)
                             {
                                 amap.AddMarker(new AMapMarkerOptions()
                                 {
                                     Position = latlng,
                                     IconUri = new Uri("Images/bus.png", UriKind.Relative),
                                 });
                             }

                             amap.MoveCamera(CameraUpdateFactory.NewLatLngZoom(latlngs.FirstOrDefault(), 12));
                             //});
                         }
                         else
                         {
                             MessageBox.Show(busLines.Erro.Message);
                         }
                         // this.Dispatcher.BeginInvoke(,);
                     });
                
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            amap.Clear();//清除地图上的覆盖物
            GetBusLineKeyWords(txtBusLine.Text, txtCity.Text, 10, 1);
            
         

        }


        private List<LatLng> latLagsFromString(string polyline)
        {
            List<LatLng> latlng = new List<LatLng>();

            string[] arrystring = polyline.Split(new char[] { ';' });
            foreach (String str in arrystring)
            {
                String[] lnglatds = str.Split(new char[] { ',' });
                latlng.Add(new LatLng(Double.Parse(lnglatds[1]), Double.Parse(lnglatds[0])));
            }
            return latlng;

        }

        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            amap.Destory();
            base.OnBackKeyPress(e);
        }

    }
}