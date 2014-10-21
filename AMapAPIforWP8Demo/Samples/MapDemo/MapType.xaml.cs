using System.Windows;
using Microsoft.Phone.Controls;
using Com.AMap.Api.Maps;
using Com.AMap.Api.Maps.Model;

namespace AMap_WP8_Api_Demos_v2._2.Samples
{
    /// <summary>
    /// 地图类型
    /// </summary>
    public partial class MapType : PhoneApplicationPage
    {
        AMap amap;
        public MapType()
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



        private void SatelliteShow(object sender, RoutedEventArgs e)
        {
            //地图类型：卫星地图
            amap.MapType = AMap.AMapType.Aerial;
        }

        private void SatelliteHide(object sender, RoutedEventArgs e)
        {
            //地图类型：道路地图
            amap.MapType = AMap.AMapType.Road;
        }

        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            amap.Destory();
            base.OnBackKeyPress(e);
        }

    }
}