using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Com.AMap.Api.Services;
using Com.AMap.Api.Services.Results;
using Microsoft.Phone.Controls;
using Com.AMap.Api.Maps;
using System.Diagnostics;
using Com.AMap.Api.Maps.Model;
using System.Threading.Tasks;

namespace AMapAPIforWP8Demo.Samples
{
    public partial class SearchReGeoCode : PhoneApplicationPage
    {
        AMap amap;
        AMapMarker marker;
        LatLng latLng;
        public SearchReGeoCode()
        {
            InitializeComponent();
            this.ContentPanel.Children.Add(amap = new AMap());
            amap.Tap += amap_Tap;
            amap.MarkerClickListener += amap_MarkerClickListener;

        }

        void amap_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            latLng = amap.GetProjection().FromScreenLocation(e.GetPosition(amap));
            this.txtLat.Text = latLng.latitude.ToString();
            this.txtLon.Text = latLng.longitude.ToString();

        }



        private async Task GeoCodeToAddress(double lon, double lat)
        {
            AMapReGeoCodeResult rcc = await AMapReGeoCodeSearch.GeoCodeToAddress(lon, lat, 500, "", Extensions.All);
            // AMapReGeoCodeResult rcc = await AMapReGeoCodeSearch.GeoCodeToAddress(121.38927, 31.17057);

            
            this.Dispatcher.BeginInvoke(() =>
                {
                    if (rcc.Erro == null && rcc.ReGeoCode != null)
                    {
                        AMapReGeoCode regeocode = rcc.ReGeoCode;
                        Debug.WriteLine(regeocode.Address_component);
                        Debug.WriteLine(regeocode.Formatted_address);
                        Debug.WriteLine(regeocode.Pois);

                        List<AMapPOI> pois = regeocode.Pois.ToList();
                        //POI信息点
                        foreach (AMapPOI poi in pois)
                        {
                            marker = amap.AddMarker(new AMapMarkerOptions()
                            {
                                Position = new LatLng(poi.Location.Lat, poi.Location.Lon),
                                Title = poi.Name,
                                Snippet = poi.Address,
                                IconUri = new Uri("Images/RED.png", UriKind.Relative),

                            });
                        }

                        Debug.WriteLine(regeocode.Roadinters);
                        Debug.WriteLine(regeocode.Roadslist);
                        AMapAddressComponent addressComponent = regeocode.Address_component;
                        Debug.WriteLine(addressComponent.Building);
                        Debug.WriteLine(addressComponent.City);
                        Debug.WriteLine(addressComponent.District);
                        Debug.WriteLine(addressComponent.Neighborhood);
                        Debug.WriteLine(addressComponent.Province);
                        Debug.WriteLine(addressComponent.Stree_number);
                        Debug.WriteLine(addressComponent.Township);
                        AMapStreetNumber streetNumber = addressComponent.Stree_number;
                        Debug.WriteLine(streetNumber.Direction);
                        Debug.WriteLine(streetNumber.Distance);
                        Debug.WriteLine(streetNumber.Location.Lat);
                        Debug.WriteLine(streetNumber.Location.Lon);
                        Debug.WriteLine(streetNumber.Number);
                        Debug.WriteLine(streetNumber.Street);


                        marker = amap.AddMarker(new AMapMarkerOptions()
                          {
                              Position = new LatLng(streetNumber.Location.Lat, streetNumber.Location.Lon),//amap.Center,//
                              Title = addressComponent.Province,
                              Snippet = regeocode.Formatted_address,
                              IconUri = new Uri("Images/AZURE.png", UriKind.Relative),

                          });


                        //显示化弹出信息
                        AInfoWindow info = new AInfoWindow();
                        info.Title = addressComponent.Province;
                        info.ContentText = regeocode.Formatted_address;
                        marker.ShowInfoWindow(info);

                        amap.MoveCamera(CameraUpdateFactory.NewLatLngZoom(new LatLng(Convert.ToDouble(txtLon.Text), Convert.ToDouble(txtLat.Text)), 12));
                    }
                    else
                    {
                        MessageBox.Show(rcc.Erro.Message);
                    }

                });

        }

        void amap_MarkerClickListener(AMapMarker sender, AMapEventArgs args)
        {
            sender.ShowInfoWindow(new AInfoWindow()
            {
                Title = sender.Title,
                ContentText = sender.Snippet,
            });
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Dispatcher.BeginInvoke(async () =>
                {
                    int time = Environment.TickCount;
                    amap.Clear();
                    if (string.IsNullOrWhiteSpace(txtLat.Text) && string.IsNullOrWhiteSpace(txtLon.Text))
                    {
                        return;
                    }
                    await GeoCodeToAddress(Convert.ToDouble(txtLon.Text), Convert.ToDouble(txtLat.Text));
                    Debug.WriteLine("查询时间：================================================="+(Environment.TickCount - time));
                });
        }

        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            amap.Destory();
            base.OnBackKeyPress(e);
        }

    }
}