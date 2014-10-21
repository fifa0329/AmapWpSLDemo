using System;
using System.Windows;
using Microsoft.Phone.Controls;
using Com.AMap.Api.Maps;
using Com.AMap.Api.Maps.Model;

namespace AMapAPIforWP8Demo.Samples.MapDemo
{
    public partial class GroundOverly : PhoneApplicationPage
    {
        AMap amap;
        public GroundOverly()
        {
            InitializeComponent();
            this.ContentPanel.Children.Add(amap = new AMap());
            this.amap.Loaded += amap_Loaded;
        }

        void amap_Loaded(object sender, RoutedEventArgs e)
        {

            amap.MoveCamera(CameraUpdateFactory.NewLatLngZoom(new LatLng((39.935029 + 39.939577)/2, (116.384377+116.388331)/2), 17));
            this.Dispatcher.BeginInvoke(() =>
                {
                    AMapGroundOverlay go = amap.AddGroundOverlay(new AMapGroundOverlayOptions()
                    {
                        ImageUri = new Uri("Images/2.png", UriKind.Relative),
                        Bounds = new LatLngBounds(new LatLng(39.935029, 116.384377), new LatLng(39.939577, 116.388331)),
                        Visible = true,
                    });
                });
        }
        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            amap.Destory();
            base.OnBackKeyPress(e);
        }
    }
}