using System.Windows;
using Microsoft.Phone.Controls;
using Com.AMap.Api.Maps;
using Com.AMap.Api.Maps.Model;

namespace AMap_WP8_Api_Demos_v2._2.Samples
{
     /// <summary>
     /// 实时交通
     /// </summary>
    public partial class Traffic : PhoneApplicationPage
    {
        AMap amap;
        public Traffic()
        {
            InitializeComponent();
            //添加地图控件
            this.ContentPanel.Children.Add(amap = new AMap());
          
            amap.Loaded += amap_Loaded;
        }

        private void amap_Loaded(object sender, RoutedEventArgs e)
        {
            //设置地图默认的经纬度和缩放级别
            amap.MoveCamera(CameraUpdateFactory.NewLatLngZoom(new LatLng(39.90923, 116.397428), 13));
        }

  

        private void showTraffic(object sender, RoutedEventArgs e)
        {
            //显示实时交通
            amap.TrafficEnabled = true;
        }

        private void hideTraffic(object sender, RoutedEventArgs e)
        {
            //隐藏实时交通
            amap.TrafficEnabled = false;
        }

        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            amap.Destory();
            base.OnBackKeyPress(e);
        }

    }
}