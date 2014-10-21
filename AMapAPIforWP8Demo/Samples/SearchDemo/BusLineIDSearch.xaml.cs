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
    public partial class BusLineIDSearch : PhoneApplicationPage
    {
        AMap amap;
        public BusLineIDSearch()
        {
            InitializeComponent();
            this.ContentPanel.Children.Add(amap = new AMap());
        }

        private async void GetBusLineIDSearch(string id)
        {
            AMapBusLineResults busLines = await AMapBusSearch.BusLineIDSearch(id);
            if (busLines.Erro==null)
            {
                if (busLines.BusLineList.Count==0)
                {
                    MessageBox.Show("无查询结果");
                    return;
                }
                List<AMapBusLine> busLine = busLines.BusLineList.ToList();

                foreach (AMapBusLine bl in busLine)
                {
                    Debug.WriteLine(bl.Name);
                    List<LatLng> latlng = latLagsFromString(bl.Polyline );
                    //绘制公交路线
                    amap.AddPolyline(new AMapPolylineOptions()
                    {
                        Points =latlng ,
                        Color = Color.FromArgb(255, 0, 0, 255),
                        Width = 2,
                    });
                    //起始站
                    amap.AddMarker(new AMapMarkerOptions()
                    {
                        Position = latlng.FirstOrDefault(),
                        Title = "Title",
                        Snippet = "Snippet",
                        IconUri = new Uri("Images/bus_start_pic.png", UriKind.Relative),
                    });
                    //终点站
                    amap.AddMarker(new AMapMarkerOptions()
                    {
                        Position =latlng.LastOrDefault(),
                        Title = "Title",
                        Snippet = "Snippet",
                        IconUri = new Uri("Images/bus_end_pic.png", UriKind.Relative),
                    });
                }
              txtBusLine.Text= busLine[0].Name;
            }
            else
            {
                MessageBox.Show(busLines.Erro.Message);
            }

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Dispatcher.BeginInvoke(() =>
                {
                    amap.Clear();
                    GetBusLineIDSearch(txtBusLineID.Text);
                });
        }

        /// <summary>
        /// 经纬度字符串转换
        /// </summary>
        /// <param name="polyline"></param>
        /// <returns></returns>
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