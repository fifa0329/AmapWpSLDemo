using System;
using System.Windows;
using Microsoft.Phone.Controls;
using Com.AMap.Api.Maps;
using Com.AMap.Api.Maps.Model;

namespace AMapAPIforWP8Demo.Samples
{
    /// <summary>
    /// 展示地图
    /// </summary>
    public partial class DisplayMap : PhoneApplicationPage
    {
        AMap amap=null;
        public DisplayMap()
        {
            InitializeComponent();
            //添加地图控件
            this.ContentPanel.Children.Add(amap = new AMap());
            //触发地图加载事件
            amap.Loaded += amap_Loaded;
            this.Unloaded += DisplayMap_Unloaded;
        }

        void DisplayMap_Unloaded(object sender, RoutedEventArgs e)
        {
            amap = null;
        }

        /// <summary>
        /// 地图加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void amap_Loaded(object sender, RoutedEventArgs e)
        {
            //设置地图默认的经纬度和缩放级别
            amap.MoveCamera(CameraUpdateFactory.NewLatLngZoom(new LatLng(39.90923, 116.397428), 13));
        
        }
        private void ApplicationBarIconButton_Click(object sender, EventArgs e)
        {
            string msg = "";
            msg = "当前地图版本：" + amap.APIVersion;
            msg += Environment.NewLine;
            msg += "当前地图中心：" + amap.Center.ToString();
            msg += Environment.NewLine;
            msg += "当前地图缩放级别：" + amap.Zoom.ToString("0.00");
            msg += Environment.NewLine;
            msg += "地图缩放最小级别：" + amap.MinZoomLevel;
            msg += Environment.NewLine;
            msg += "地图缩放最大级别：" + amap.MaxZoomLevel;
            msg += Environment.NewLine;
            msg += "地图每个像素对应长度：" + amap.GetScalePerPixel().ToString("0.00")+"m";
            MessageBox.Show(msg, "地图信息", MessageBoxButton.OK);
        }

        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            amap.Destory();
            base.OnBackKeyPress(e);
        }

    }
}