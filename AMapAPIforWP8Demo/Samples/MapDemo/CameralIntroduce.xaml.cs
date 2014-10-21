using System.Windows;
using Microsoft.Phone.Controls;
using Com.AMap.Api.Maps;
using Com.AMap.Api.Maps.Model;

namespace AMap_WP8_Api_Demos_v2._2.Samples
{
    public partial class CameralIntroduce : PhoneApplicationPage
    {
        AMap amap;
        public CameralIntroduce()
        {
            InitializeComponent();
            //添加地图控件
            this.ContentPanel.Children.Add(amap = new AMap());

            amap.Loaded += amap_Loaded;
            amap.CameraChangeListener += amap_CameraChangeListener;
            amap.Tap += amap_Tap;
        }

        void amap_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            this.Dispatcher.BeginInvoke(() =>
                {
                    //点击地图获取点击点的经纬度， 屏幕坐标转换为地图坐标
                    LatLng lats = amap.GetProjection().FromScreenLocation(e.GetPosition(amap));
                    cameraloutput1.Text = "target：lat/lng:(" + lats.latitude + "," + lats.longitude + ")";
                });

        }

        private void amap_Loaded(object sender, RoutedEventArgs e)
        {
            //设置地图默认的经纬度和缩放级别
            ;// amap.MoveCamera(CameraUpdateFactory.NewLatLngZoom(new LatLng(39.90923, 116.397428)));
        }

        private void amap_CameraChangeListener(object sender, AMapEventArgs e)
        {
            this.Dispatcher.BeginInvoke(() =>
            {
                //显示信息
                cameraloutput1.Text = "target：" + e.CameraPosition.target.ToString();
                cameraloutput2.Text = "tilt：" + e.CameraPosition.tilt.ToString();
                cameraloutput3.Text = "bearing：" + e.CameraPosition.bearing.ToString();
                cameraloutput4.Text = "zoom:" + e.CameraPosition.zoom.ToString();
            });

        }

        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            amap.Destory();
            base.OnBackKeyPress(e);
        }

    }
}