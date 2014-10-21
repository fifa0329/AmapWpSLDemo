using System;
using System.Collections.Generic;
using System.Windows;
using Microsoft.Phone.Controls;
using Com.AMap.Api.Maps;
using Com.AMap.Api.Maps.Model;
using System.Windows.Media;
using System.Diagnostics;

namespace AMap_WP8_Api_Demos_v2._2.Samples
{
    /// <summary>
    /// 地图覆盖物
    /// </summary>
    public partial class AddOverlays : PhoneApplicationPage
    {
        AMap amap;
        AMapCircle circle;
        AMapPolyline polyline;
        AMapPolygon polylgon;
        public AddOverlays()
        {
            InitializeComponent();

            //添加地图控件
            this.ContentPanel.Children.Add(amap = new AMap());

            amap.Loaded += amap_Loaded;

        }

        void amap_Loaded(object sender, RoutedEventArgs e)
        {
            //设置地图默认的经纬度和缩放级别
            amap.MoveCamera(CameraUpdateFactory.NewLatLngZoom(new LatLng(39.987326, 116.48236), 13));

        }

        private void Button_AddMarker_Click(object sender, RoutedEventArgs e)
        {
            //绘点
            amap.AddMarker(new AMapMarkerOptions()
            {
                Position = amap.Center,
                IconUri = new Uri("Images/AZURE.png", UriKind.Relative)
            });
        }

        private void Button_DrawLine_Click(object sender, RoutedEventArgs e)
        {
            //绘线
            List<LatLng> lnglats = new List<LatLng>();
            lnglats.Add(new LatLng(amap.Center.latitude + 0.02, amap.Center.longitude + 0.03));
            lnglats.Add(amap.Center);
            lnglats.Add(new LatLng(amap.Center.latitude + 0.05, amap.Center.longitude - 0.06));

            polyline = amap.AddPolyline(new AMapPolylineOptions()
             {
                 Points = lnglats,
                 Color = Color.FromArgb(255, 255, 0, 0),
                 Width = 4
             });
           
        }

        private void Button_DrawPolygon_Click(object sender, RoutedEventArgs e)
        {
            //绘多边形
            List<LatLng> lnglats = new List<LatLng>();
            lnglats.Add(new LatLng(amap.Center.latitude + 0.02, amap.Center.longitude + 0.03));
            lnglats.Add(new LatLng(amap.Center.latitude + 0.03, amap.Center.longitude - 0.03));
            lnglats.Add(new LatLng(amap.Center.latitude - 0.026, amap.Center.longitude - 0.03));
            lnglats.Add(amap.Center);
            lnglats.Add(new LatLng(amap.Center.latitude - 0.04, amap.Center.longitude + 0.035));
            polylgon = amap.AddPolygon(new AMapPolygonOptions()
            {
                FillColor = Color.FromArgb(30, 255, 0, 255),
                Points = lnglats,
                StrokeColor = Color.FromArgb(255, 102, 136, 255),
                StrokeWidth = 2
            });
        }

        private void Button_DrawCircle_Click(object sender, RoutedEventArgs e)
        {
            //绘圆
            circle = amap.AddCircle(new AMapCircleOptions()
             {
                 Center = amap.Center,//圆点位置
                 Radius = 1000,//半径
                 FillColor = Color.FromArgb(70, 100, 150, 255),
                 StrokeWidth = 3,
                 StrokeColor = Color.FromArgb(255, 0, 0, 255)
             });
        }



        private void Button_RemoveOverlays_Click(object sender, RoutedEventArgs e)
        {
            //清除覆盖物
            amap.Clear();
        }

        private void Slider_ValueChanged_1(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            //调整边框粗细
            Debug.WriteLine("边框:" + e.NewValue);
            if (circle != null)
            {
                circle.StrokeWidth = (float)e.NewValue;

            }
            if (polyline != null)
            {
                polyline.Width = (float)e.NewValue;
            }
            if (polylgon != null)
            {
                polylgon.StrokeWidth = (float)e.NewValue;
            }
        }

        private void Slider_ValueChanged_2(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            //调整填充颜色RGB
            Debug.WriteLine("RGB：" + e.NewValue);
            if (circle != null)
            {
                circle.FillColor = Color.FromArgb(255, Convert.ToByte(e.NewValue), Convert.ToByte(e.NewValue), Convert.ToByte(e.NewValue));
            }
            if (polyline != null)
            {
                polyline.Color = Color.FromArgb(255, Convert.ToByte(e.NewValue), Convert.ToByte(e.NewValue), Convert.ToByte(e.NewValue));
            }
            if (polylgon != null)
            {
                polylgon.FillColor = Color.FromArgb(255, Convert.ToByte(e.NewValue), Convert.ToByte(e.NewValue), 255);
            }
        }

        private void Slider_ValueChanged_3(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            //调整透明度
            Debug.WriteLine("透明度：" + e.NewValue);
            if (circle != null)
            {
                circle.FillColor = Color.FromArgb(Convert.ToByte(e.NewValue), 0, 0, 255);
            }
            if (polyline != null)
            {
                polyline.Color = Color.FromArgb(Convert.ToByte(e.NewValue), 0, 0, 255);
            }
            if (polylgon != null)
            {
                polylgon.FillColor = Color.FromArgb(Convert.ToByte(e.NewValue), 255, 0, 0);
            }
        }

        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            amap.Destory();
            base.OnBackKeyPress(e);
        }

    }
}