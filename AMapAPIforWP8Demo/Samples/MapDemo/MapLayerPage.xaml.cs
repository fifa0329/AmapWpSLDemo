using System;
using System.Windows;
using Microsoft.Phone.Controls;
using Com.AMap.Api.Maps;
using Com.AMap.Api.Maps.Model;

namespace AMap_WP8_Api_Demos_v2._2.Samples
{
    public partial class MapLayerPage : PhoneApplicationPage
    {
        AMap amap;
        AMapLayer mapLayer = new AMapLayer();
        public MapLayerPage()
        {
            InitializeComponent();
            this.ContentPanel.Children.Add(amap = new AMap());
            this.amap.Tap += amap_Tap;
          
        }

        void amap_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            //添加Marker
            AMapMarker marker1;
            marker1 = amap.AddMarker(new AMapMarkerOptions()
            {

                Position = amap.GetProjection().FromScreenLocation(e.GetPosition(amap)),
                IconUri = new Uri("Images/ROSE.png", UriKind.Relative),
                //Anchor = new Point(1, 1),//图标中心点
            });

            amap.AddAMapLayer(mapLayer);
            mapLayer.AddMarker(new AMapMarkerOptions()
            {
                Position = amap.GetProjection().FromScreenLocation(e.GetPosition(amap)),
                IconUri = new Uri("Images/AZURE.png", UriKind.Relative),
                Anchor=new Point(0.5,0.5),
            });
          
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //清除AMapLayer
            if (amap != null)
            {
                amap.RemoveAMapLayer(mapLayer);
            }
            
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            //AMap Clear
            if (amap != null)
            {
                amap.Clear();
            }
        }

        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            amap.Destory();
            base.OnBackKeyPress(e);
        }


    }
}