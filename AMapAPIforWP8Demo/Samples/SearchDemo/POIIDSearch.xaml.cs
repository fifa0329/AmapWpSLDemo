using System;
using System.Windows;
using Com.AMap.Api.Services;
using Com.AMap.Api.Services.Results;
using Microsoft.Phone.Controls;
using Com.AMap.Api.Maps;
using Com.AMap.Api.Maps.Model;
using System.Diagnostics;

namespace AMapAPIforWP8Demo.Samples.SearchDemo
{
    public partial class POIIDSearch : PhoneApplicationPage
    {
        AMap amap;
        AMapMarker marker;
        public POIIDSearch()
        {
            InitializeComponent();
            this.ContentPanel.Children.Add(amap = new AMap());
        }

        private async void GetPOIID(string id)
        {
            AMapPOIResults poir = await AMapPOISearch.POIID(id);
            this.Dispatcher.BeginInvoke(() =>
            {
                if (poir.Erro==null&&poir.POIList!=null)
                {
                    if (poir.POIList.Count==0)
                    {
                        MessageBox.Show("无查询结果");
                        return;
                    }
                    int i = 0;
                    foreach (AMapPOI item in poir.POIList)
                    {
                        i++;
                       marker= amap.AddMarker(new AMapMarkerOptions()
                        {
                            Position = new LatLng(item.Location.Lat, item.Location.Lon),//amap.Center,//
                            IconUri = new Uri("Images/AZURE.png", UriKind.Relative),
                            Anchor = new Point(0.5, 0.5),
                        });
                       // amap.MoveCamera(CameraUpdateFactory.NewLatLngZoom(new LatLng(item.Location.Lat, item.Location.Lon), 13));
                      
                        marker.ShowInfoWindow(
                            new AInfoWindow() 
                            { 
                              Title=item.Name,
                              ContentText=item.Address,
                            });
                    }
                    Debug.WriteLine(i);
                }
                else
                {
                    MessageBox.Show(poir.Erro.Message);
                }
            });


        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            amap.Clear();
            GetPOIID(txtID.Text);
        }

        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            amap.Destory();
            base.OnBackKeyPress(e);
        }

    }
}