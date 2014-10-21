using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Com.AMap.Api.Services;
using Com.AMap.Api.Services.Results;
using Microsoft.Phone.Controls;
using Com.AMap.Api.Maps;
using Com.AMap.Api.Maps.Model;
using System.Diagnostics;
using System.Windows.Media;

namespace AMapAPIforWP8Demo.Samples.SearchDemo
{
    public partial class WalkingNavigation : PhoneApplicationPage
    {
        AMap amap;
        LatLng startLatLng;
        LatLng endLatLng;

        public WalkingNavigation()
        {
            InitializeComponent();
            this.ContentPanel.Children.Add(amap = new AMap());
            amap.Tap += amap_Tap;
            amap.Loaded += amap_Loaded;
        }

        private async void GetNavigationWalking(LatLng start, LatLng end)
        {
            //116.481028, 39.989643, 116.465302, 40.004717
            AMapRouteResults rts = await AMapNavigationSearch.WalkingNavigation(start.longitude, start.latitude, end.longitude, end.latitude,0);
            if (rts.Erro == null)
            {
                if (rts.Count == 0)
                {
                    MessageBox.Show("无查询结果");
                    return;
                }

                AMapRoute route = rts.Route;
                List<AMapPath> paths = route.Paths.ToList();

                List<LatLng> lnglats = new List<LatLng>();
                foreach (AMapPath item in paths)
                {
                    Debug.WriteLine("起点终点距离:" + item.Distance);
                    Debug.WriteLine("预计耗时:" + item.Duration / 60 + "分钟");
                    Debug.WriteLine("导航策略:" + item.Strategy);

                    //画路线
                    List<AMapStep> steps = item.Steps.ToList();
                    foreach (AMapStep st in steps)
                    {

                        amap.AddMarker(new AMapMarkerOptions()
                        {
                            Position = latLagsFromString(st.Polyline).FirstOrDefault(),
                            Title = "Title",
                            Snippet = "Snippet",
                            IconUri = new Uri("Images/man.png", UriKind.Relative),
                        });
                        Debug.WriteLine(st.Instruction);
                        //Debug.WriteLine(st.Road);
                        // Debug.WriteLine(st.Assistant_action);
                        lnglats = latLagsFromString(st.Polyline);
                        amap.AddPolyline(new AMapPolylineOptions()
                        {
                            Points = latLagsFromString(st.Polyline),
                            Color = Color.FromArgb(255, 0, 0, 255),
                            Width = 4,
                        });
                    }

                }
            }
            else
            {
                MessageBox.Show(rts.Erro.Message);
            }

        }

        void amap_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            LatLng latLng = amap.GetProjection().FromScreenLocation(e.GetPosition(amap));
            if (startLatLng == null)
            {
                startLatLng = latLng;
                txtOrigin.Text = latLng.latitude + "/" + latLng.longitude;
                amap.AddMarker(new AMapMarkerOptions()
                {
                    Position = startLatLng,
                    Title = "Title",
                    Snippet = "Snippet",
                    IconUri = new Uri("Images/bus_start_pic.png", UriKind.Relative),
                });
            }
            else if (endLatLng == null)
            {
                endLatLng = latLng;
                txtDestination.Text = latLng.latitude + "/" + latLng.longitude;
                amap.AddMarker(new AMapMarkerOptions()
                {
                    Position = endLatLng,
                    Title = "Title",
                    Snippet = "Snippet",
                    IconUri = new Uri("Images/bus_end_pic.png", UriKind.Relative),
                });
            }
        }

        void amap_Loaded(object sender, RoutedEventArgs e)
        {
            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Dispatcher.BeginInvoke(() =>
            {
                if (startLatLng != null && endLatLng != null)
                {
                    GetNavigationWalking(startLatLng, endLatLng);
                }
            });
        }

        private List<LatLng> latLagsFromString(string polyline)
        {
            List<LatLng> latlng = new List<LatLng>();

            string[] arrystring = polyline.Split(new char[] { ';' });
            foreach (String str in arrystring)
            {
                String[] lnglatds = str.Split(new char[] { ',' });
                latlng.Add(new LatLng(Double.Parse(lnglatds[1]), Double.Parse(lnglatds[0])));
            }
            return latlng;

        }

        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            amap.Destory();
            base.OnBackKeyPress(e);
        }

    }
}