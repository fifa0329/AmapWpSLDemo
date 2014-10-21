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
    public partial class SearchGeoCode : PhoneApplicationPage
    {
        AMap amap;
        AMapMarker marker;
        public SearchGeoCode()
        {
            InitializeComponent();
            this.ContentPanel.Children.Add(amap = new AMap());
            amap.MarkerClickListener += amap_MarkerClickListener;
          
        }

        private async Task AddressToGeoCode(string address)
        {

            AMapGeoCodeResult cr = await AMapGeoCodeSearch.AddressToGeoCode(address);

            this.Dispatcher.BeginInvoke(() =>
            {
                if (cr.Erro == null && cr.GeoCodeList != null)
                {
                    if (cr.GeoCodeList.Count==0)
                    {
                        MessageBox.Show("无查询结果");
                        return;
                    }
                    IEnumerable<AMapGeoCode> geocode = cr.GeoCodeList;
                    int i = 0;
                    foreach (AMapGeoCode gcs in geocode)
                    {
                        i++;
                        Debug.WriteLine(gcs.Adcode);
                        Debug.WriteLine(gcs.Building);
                        Debug.WriteLine(gcs.City);
                        Debug.WriteLine(gcs.District);
                        Debug.WriteLine(gcs.FormattedAddress);
                        Debug.WriteLine(gcs.Province);
                        Debug.WriteLine(gcs.Township);
                        Debug.WriteLine(gcs.Location.Lon);
                        Debug.WriteLine(gcs.Location.Lat);
                        Debug.WriteLine(gcs.LevelList[0]);

                        marker= amap.AddMarker(new AMapMarkerOptions()
                        {
                            Position = new LatLng(gcs.Location.Lat, gcs.Location.Lon),
                            Title = gcs.FormattedAddress,
                            Snippet = gcs.District,
                            IconUri = new Uri("Images/AZURE.png", UriKind.Relative),

                        });
                    
                        //amap.MoveCamera(CameraUpdateFactory.NewLatLngZoom(new LatLng(gcs.Location.Lat, gcs.Location.Lon),14));

                    }
                    //如果返回的geocode数大于1个，调整视图
                    if (geocode.Count() > 1)
                    {
                        LatLngBounds.Builder builder = new LatLngBounds.Builder();
                        List<AMapMarker> markers = amap.GetMapMarkers();
                        foreach (AMapMarker marker in markers)
                        {
                            builder.Include(marker.Position);
                        }
                        this.amap.MoveCamera(CameraUpdateFactory.NewLatLngBounds(builder.Build(), markers.Count()));
                    }
                    else
                    {
                        amap.MoveCamera(CameraUpdateFactory.NewLatLngZoom(new LatLng(geocode.FirstOrDefault().Location.Lat, geocode.FirstOrDefault().Location.Lon), 14));
                    }

                    Debug.WriteLine(i);
                
                    
                }
                else
                {
                    MessageBox.Show(cr.Erro.Message);
                }

            });
        }

        void amap_MarkerClickListener(AMapMarker sender, AMapEventArgs args)
        {
            sender.ShowInfoWindow(new AInfoWindow()
            {
                Title = sender.Title,
                ContentText =sender.Snippet,
            });
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            amap.Clear();
            if (!string.IsNullOrWhiteSpace(txtAddress.Text))
            {
                await AddressToGeoCode(txtAddress.Text);
            }
            
        }

        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            amap.Destory();
            base.OnBackKeyPress(e);
        }

    }
}