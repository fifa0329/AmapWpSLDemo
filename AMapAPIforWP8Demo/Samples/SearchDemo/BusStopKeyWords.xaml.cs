using System;
using System.Collections.Generic;
using System.Windows;
using Com.AMap.Api.Services;
using Com.AMap.Api.Services.Results;
using Microsoft.Phone.Controls;
using Com.AMap.Api.Maps;
using Com.AMap.Api.Maps.Model;
using System.Diagnostics;

namespace AMapAPIforWP8Demo.Samples.SearchDemo
{
    /// <summary>
    /// 公交站查询
    /// </summary>
    public partial class BusStopKeyWords : PhoneApplicationPage
    {
        AMap amap;
        IEnumerable<AMapBusStop> busStops;
        public BusStopKeyWords()
        {
            InitializeComponent();
            this.ContentPanel.Children.Add(amap = new AMap());
        }

        public async void GetBusStopKeyWords(string keywords,string city, uint offset,uint page )
        {
            AMapBusStopResults busStoprs = await AMapBusSearch.BusStopKeyWords(keywords, offset, page, city);
            if (busStoprs.Erro == null && busStoprs.BusStopList != null)
            {
                if (busStoprs.BusStopList.Count==0)
                {
                    MessageBox.Show("无查询结果");
                    return;
                }
                busStops = busStoprs.BusStopList;
                List<LatLng> latLngs = new List<LatLng>();

                int i = 0;
                foreach (AMapBusStop bs in busStops)
                {
                    i++;
                    Debug.WriteLine(bs.Id);
                    Debug.WriteLine(bs.Name);
                    Debug.WriteLine(bs.Location.Lat);
                    Debug.WriteLine(bs.Location.Lon);

                    latLngs.Add(new LatLng(bs.Location.Lat, bs.Location.Lon));
                 
                }
                Debug.WriteLine("公交站总数："+i);
                //绘制公交站
                foreach (LatLng latlng in latLngs)
                {
                    amap.AddMarker(new AMapMarkerOptions()
                    {
                        Position = latlng,
                        Title = "Title",
                        Snippet = "Snippet",
                        IconUri = new Uri("Images/marker_gps_no_sharing.png", UriKind.Relative),
                    });
                }

                //amap.MoveCamera(CameraUpdateFactory.NewLatLngZoom(new LatLng(busStoprs.BusStopList[0].Location.Lat, busStoprs.BusStopList[0].Location.Lon), 13));
            }
            else
            {
                MessageBox.Show(busStoprs.Erro.Message);
            }

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Dispatcher.BeginInvoke(() =>
                {
                    amap.Clear();
                    
                        GetBusStopKeyWords(txtBusStop.Text, txtCity.Text, 20, 1);
                    
                });
        }

        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            amap.Destory();
            base.OnBackKeyPress(e);
        }



    }
}