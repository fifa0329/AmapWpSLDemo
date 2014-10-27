using System.ComponentModel;
using System.Windows;
using Com.AMap.Api.Maps;
using Com.AMap.Api.Maps.Model;
using Microsoft.Phone.Controls;
using GestureEventArgs = System.Windows.Input.GestureEventArgs;

namespace AMap_WP8_Api_Demos_v2._2.Samples
{
    public partial class CameralIntroduce : PhoneApplicationPage
    {
        private readonly AMap amap;

        public CameralIntroduce()
        {
            InitializeComponent();
            //添加地图控件
            ContentPanel.Children.Add(amap = new AMap());

            amap.Loaded += amap_Loaded;
            amap.CameraChangeListener += amap_CameraChangeListener;
            amap.Tap += amap_Tap;
        }

        private void amap_Tap(object sender, GestureEventArgs e)
        {
            //点击地图获取点击点的经纬度， 屏幕坐标转换为地图坐标
            LatLng lats = amap.GetProjection().FromScreenLocation(e.GetPosition(amap));
            cameraloutput1.Text = "target：lat/lng:(" + lats.latitude + "," + lats.longitude + ")";
        }

        private void amap_Loaded(object sender, RoutedEventArgs e)
        {
            //设置地图默认的经纬度和缩放级别
            ; // amap.MoveCamera(CameraUpdateFactory.NewLatLngZoom(new LatLng(39.90923, 116.397428)));
        }

        private void amap_CameraChangeListener(object sender, AMapEventArgs e)
        {
            //todo 应该直接给开发者返回main thread
            Dispatcher.BeginInvoke(() =>
            {
                //显示信息
                cameraloutput1.Text = "target：" + e.CameraPosition.target;
                cameraloutput2.Text = "tilt：" + e.CameraPosition.tilt;
                cameraloutput3.Text = "bearing：" + e.CameraPosition.bearing;
                cameraloutput4.Text = "zoom:" + e.CameraPosition.zoom;
            });
        }

        protected override void OnBackKeyPress(CancelEventArgs e)
        {
            amap.Destory();
            base.OnBackKeyPress(e);
        }
    }
}