using System;
using System.Collections.Generic;
using System.Windows;
using Com.AMap.Api.Services;
using Com.AMap.Api.Services.Results;
using Microsoft.Phone.Controls;
using Com.AMap.Api.Maps;
using System.Threading.Tasks;
using System.Diagnostics;
using Com.AMap.Api.Maps.Model;


namespace AMapAPIforWP8Demo.Samples.SearchDemo
{
    /// <summary>
    /// POI关键字搜索
    /// </summary>
    public partial class POIkeywordSearch : PhoneApplicationPage
    {
        AMap amap;
        AInfoWindow aInfoWindows;

        public POIkeywordSearch()
        {
            InitializeComponent();
            this.ContentPanel.Children.Add(amap = new AMap());
            amap.MarkerClickListener += amap_MarkerClickListener;

        }

        private async Task GetPOISearch(string city, string keywords, string types, bool groupbuy, bool discount)
        {
            AMapFilterOption aMapFilterOption = new AMapFilterOption();
            aMapFilterOption.Groupbuy = groupbuy;
            aMapFilterOption.Discount = discount;
            AMapPOIResults poirs = await AMapPOISearch.POIKeyWords(keywords, types, aMapFilterOption, 0, 20, 1, Extensions.All, city);
            this.Dispatcher.BeginInvoke(() =>
                {
                    if (poirs.Erro == null && poirs.POIList != null)
                    {
                        if (poirs.POIList.Count == 0)
                        {
                            MessageBox.Show("无查询结果");
                            return;
                        }
                        IEnumerable<AMapPOI> pois = poirs.POIList;
                        int i = 0;
                        foreach (AMapPOI poi in pois)
                        {
                            i++;

                            amap.AddMarker(new AMapMarkerOptions()
                            {
                                Position = new LatLng(poi.Location.Lat, poi.Location.Lon),
                                Title = poi.Name,
                                Snippet = poi.Address,
                                IconUri = new Uri("Images/AZURE.png", UriKind.Relative),

                            });

                        }

                        Debug.WriteLine("POI总数:" + i);

                    }
                    else
                    {
                        MessageBox.Show(poirs.Erro.Message);
                    }


                });
        }

        private void amap_MarkerClickListener(AMapMarker sender, AMapEventArgs args)
        {
            sender.ShowInfoWindow(aInfoWindows = new AInfoWindow()
            {
                Title = sender.Title,
                ContentText = sender.Snippet,
            });
            aInfoWindows.Tap += aInfoWindows_Tap;
        }

        void aInfoWindows_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            string msg = ((AInfoWindow)sender).Title;
            MessageBox.Show(msg);
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {

            amap.Clear();
            await GetPOISearch(txtCity.Text, txtKeyWords.Text, txtTypes.Text, (bool)chkBoxGroupbuy.IsChecked, (bool)chkBoxDiscount.IsChecked);

        }

        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            amap.Destory();
            base.OnBackKeyPress(e);
        }

    }
}