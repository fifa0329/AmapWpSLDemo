using System;
using System.Collections.Generic;
using System.Windows;
using Com.AMap.Api.Services;
using Com.AMap.Api.Services.Results;
using Microsoft.Phone.Controls;
using Com.AMap.Api.Maps;
using Com.AMap.Api.Maps.Model;
using System.Windows.Media;
using System.Diagnostics;

namespace AMapAPIforWP8Demo.Samples.SearchDemo
{
    /// <summary>
    /// 周边多边形搜索
    /// </summary>
    public partial class POIPolygonSearch : PhoneApplicationPage
    {
        AMap amap;
        AMapMarker marker;
        List<AMapMarker> markers;
        Com.AMap.Api.Maps.Model.AMapPolygon polygon;
        List<LatLng> lnglats1;
        public POIPolygonSearch()
        {
            InitializeComponent();
            this.ContentPanel.Children.Add(amap = new AMap());
            amap.Loaded += amap_Loaded;
            amap.Tap += amap_Tap;
            amap.MarkerClickListener += amap_MarkerClickListener;
        }

     

        void amap_Loaded(object sender, RoutedEventArgs e)
        {
            this.Dispatcher.BeginInvoke(() =>
                {
                    amap.MoveCamera(CameraUpdateFactory.ZoomTo(12));
                });
        }

        private async void GetPOIPolygonSearch(string keywords, string types, bool groupbuy, bool discount, string city, uint offset)
        {
            Com.AMap.Api.Services.Results.AMapPolygon polygon = new Com.AMap.Api.Services.Results.AMapPolygon();
            polygon._Type = Com.AMap.Api.Services.Results.AMapPolygon.Types.Type.POLYGON;
           
            markers = new List<AMapMarker>();
            //多边形
            polygon.Points = new List<AMapLocation>();
            polygon.Points.Add(new AMapLocation() { Lon = lnglats1[0].longitude, Lat = lnglats1[0].latitude });
            polygon.Points.Add(new AMapLocation() { Lon = lnglats1[1].longitude, Lat = lnglats1[1].latitude });
            polygon.Points.Add(new AMapLocation() { Lon = lnglats1[2].longitude, Lat = lnglats1[2].latitude });
            polygon.Points.Add(new AMapLocation() { Lon = lnglats1[3].longitude, Lat = lnglats1[3].latitude });
            polygon.Points.Add(new AMapLocation() { Lon = lnglats1[4].longitude, Lat = lnglats1[4].latitude });

            AMapPOIResults poir = await AMapPOISearch.POIPolygon(keywords, types, polygon, null, 0, offset, 1, Extensions.All, city);
            this.Dispatcher.BeginInvoke(() =>
                {
                    if (poir.Erro == null && poir.POIList != null)
                    {
                        if (poir.POIList.Count == 0)
                        {
                            MessageBox.Show("无查询结果");
                            return;
                        }
                        IEnumerable<AMapPOI> pois = poir.POIList;
                        int i = 0;
                        foreach (AMapPOI poi in pois)
                        {
                            i++;
                            marker = amap.AddMarker(new AMapMarkerOptions()
                            {
                                Position = new LatLng(poi.Location.Lat, poi.Location.Lon),
                                Title = poi.Name,
                                Snippet = poi.Address,
                                IconUri = new Uri("Images/AZURE.png", UriKind.Relative),

                            });
                            markers.Add(marker);
                        }
                        Debug.WriteLine(i);
                    }
                    else
                    {
                        MessageBox.Show(poir.Erro.Message);
                    }
                });
        }

        void amap_MarkerClickListener(AMapMarker sender, AMapEventArgs args)
        {
            sender.ShowInfoWindow(new AInfoWindow()
            {
                    Title=sender.Title,
                    ContentText=sender.Snippet,
            });
        }

        void amap_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            this.Dispatcher.BeginInvoke(() =>
            {
                //绘多边形
                if (lnglats1 == null)
                {

                    lnglats1 = new List<LatLng>();
                    lnglats1.Add(new LatLng(amap.Center.latitude + 0.02, amap.Center.longitude + 0.03));
                    lnglats1.Add(new LatLng(amap.Center.latitude + 0.03, amap.Center.longitude - 0.03));
                    lnglats1.Add(new LatLng(amap.Center.latitude - 0.026, amap.Center.longitude - 0.03));
                    //lnglats1.Add(amap.Center);
                    lnglats1.Add(new LatLng(amap.Center.latitude - 0.04, amap.Center.longitude + 0.035));
                    lnglats1.Add(new LatLng(amap.Center.latitude - 0.046, amap.Center.longitude + 0.045));
                    polygon = amap.AddPolygon(new AMapPolygonOptions()
                    {
                        FillColor = Color.FromArgb(30, 255, 0, 255),
                        Points = lnglats1,
                        StrokeColor = Color.FromArgb(255, 102, 136, 255),
                        StrokeWidth = 2
                    });


                }

                foreach (LatLng latlng in lnglats1)
                {
                    amap.AddMarker(new AMapMarkerOptions()
                        {
                            Position = latlng,
                            Snippet = latlng.ToString(),
                            IconUri = new Uri("Images/AZURE.png", UriKind.Relative),
                        });
                }
            });
          

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
           // amap.Clear();
            if (markers != null)
            {
                foreach (var item in markers)
                {
                    item.Destroy();
                }

            }
            if (lnglats1 == null)
            {
                //MessageBox.Show("Tap地图");
                return;
            }

            GetPOIPolygonSearch(txtKeyWords.Text, txtTypes.Text, (bool)chkBoxGroupbuy.IsChecked, (bool)chkBoxDiscount.IsChecked, txtCity.Text, 50);
           
           
        }

        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            amap.Destory();
            base.OnBackKeyPress(e);
        }


    }
}