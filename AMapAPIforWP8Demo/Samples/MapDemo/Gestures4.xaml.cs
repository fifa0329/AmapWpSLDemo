﻿using System.Windows;
using Microsoft.Phone.Controls;
using Com.AMap.Api.Maps;
using Com.AMap.Api.Maps.Model;

namespace AMap_WP8_Api_Demos_v2._2.Samples
{
    /// <summary>
    /// 手势操作
    /// </summary>
    public partial class Gestures4  : PhoneApplicationPage
    {
        AMap amap;
        UiSettings uiset;
        public Gestures4()
        {
            InitializeComponent();
            this.ContentPanel.Children.Add(amap = new AMap());
            amap.Loaded += amap_Loaded;
            uiset = amap.GetUiSettings();
            uiset.TiltGesturesEnabled = false;

        }

        private void amap_Loaded(object sender, RoutedEventArgs e)
        {
            //设置默认的地图经纬度和缩放级别
            amap.MoveCamera(CameraUpdateFactory.NewCameraPosition(new LatLng(39.90923, 116.397428), 13, 45, 30));
        }

      
        private void tiltEnable(object sender, RoutedEventArgs e)
        {
            //允许倾斜
            uiset.TiltGesturesEnabled = true;

        }

      
        private void untiltEnable(object sender, RoutedEventArgs e)
        {
            //不允许倾斜
            uiset.TiltGesturesEnabled = false;
        }

     
        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            amap.Destory();
            base.OnBackKeyPress(e);
        }

    }
}