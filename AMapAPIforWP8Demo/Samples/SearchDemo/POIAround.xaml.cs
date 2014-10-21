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
    /// poi周边搜索
    /// </summary>
    public partial class POIAround : PhoneApplicationPage
    {
        AMap amap;
        LatLng latLng;
     
        public POIAround()
        {
            InitializeComponent();
            this.ContentPanel.Children.Add(amap = new AMap());
            this.amap.Tap += amap_Tap;
            amap.MarkerClickListener += amap_MarkerClickListener;
        }

    

        private async void GetPOIAround(double centerX, double centerY, string keywords, bool groupbuy, bool discount, string types, uint radius, string city)
        {

            AMapPOIResults poir = await AMapPOISearch.POIAround(centerX, centerY, keywords, types, null, radius, 0, 20, 1, Extensions.All, city);
           
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
                           
                            amap.AddMarker(new AMapMarkerOptions()
                            {
                                Position = new LatLng(poi.Location.Lat, poi.Location.Lon),//amap.Center,//
                                Title = poi.Name,
                                Snippet =poi.Address,
                                IconUri = new Uri("Images/AZURE.png", UriKind.Relative),

                            });
                        }
                        Debug.WriteLine("POI点总数：" + i);
                       // amap.MoveCamera(CameraUpdateFactory.NewLatLngZoom(latLng, 14));
                    }
                    else
                    {
                        MessageBox.Show(poir.Erro.Message);
                    }
                });
        }


        void amap_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            this.Dispatcher.BeginInvoke(() =>
                {
                    //amap.Clear();
                    latLng = amap.GetProjection().FromScreenLocation(e.GetPosition(amap));
                    txtLat.Text = "lon:" + latLng.longitude + " lat:" + latLng.latitude;
                    //点击点标注
                    //marker = amap.AddMarker(new AMapMarkerOptions()
                    // {
                    //     Position = latLng,
                    //     Title = "Title",
                    //     Snippet = "Snippet",
                    //     IconUri = new Uri("Images/AZURE.png", UriKind.Relative),

                    // });
                });

        }

        void amap_MarkerClickListener(AMapMarker sender, AMapEventArgs args)
        {
            sender.ShowInfoWindow(new AInfoWindow() 
            {
                Title=sender.Title,
                ContentText=sender.Snippet,
            }
                );

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (latLng != null)
            {
                amap.Clear();
                GetPOIAround(latLng.longitude, latLng.latitude, txtKeyWords.Text, (bool)chkBoxGroupbuy.IsChecked, (bool)chkBoxDiscount.IsChecked, txtTypes.Text, 3000, txtCity.Text);
                
            }

        }


        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            amap.Destory();
            base.OnBackKeyPress(e);
        }



    }
}