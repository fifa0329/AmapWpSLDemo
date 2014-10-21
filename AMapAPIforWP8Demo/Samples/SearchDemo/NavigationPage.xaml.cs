using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Com.AMap.Api.Services;
using Com.AMap.Api.Services.Results;
using Microsoft.Phone.Controls;
using Com.AMap.Api.Maps.Model;
using Com.AMap.Api.Maps;
using System.Diagnostics;
using System.Windows.Media;

namespace AMapAPIforWP8Demo.Samples.SearchDemo
{
    public partial class NavigationPage : PhoneApplicationPage
    {
        AMap amap;
        LatLng startLatLng;
        LatLng endLatLng;

        public NavigationPage()
        {
           
            InitializeComponent();
            this.ContentPanel.Children.Add(amap = new AMap());
            amap.Tap += amap_Tap;
        }

        private async void GetNavigationDriving(LatLng start, LatLng end)
        {
            //116.481028, 39.989643, 116.465302, 40.004717
            AMapRouteResults rts = await AMapNavigationSearch.DrivingNavigation(start.longitude, start.latitude, end.longitude, end.latitude, Extensions.All);
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

                    //绘制驾车路线
                    List<AMapStep> steps = item.Steps.ToList();
                    foreach (AMapStep st in steps)
                    {
                        Debug.WriteLine(st.Instruction);
                        //Debug.WriteLine(st.Road);
                        // Debug.WriteLine(st.Assistant_action);

                        amap.AddMarker(new AMapMarkerOptions()
                        {
                            Position = latLagsFromString(st.Polyline).FirstOrDefault(),
                            Title = "",
                            Snippet = "Snippet",
                            IconUri = new Uri("Images/car.png", UriKind.Relative),
                        });

                        lnglats = latLagsFromString(st.Polyline);
                        amap.AddPolyline(new AMapPolylineOptions()
                        {
                            Points = latLagsFromString(st.Polyline),
                            Color = Colors.Green,
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

        private async void GetNavigationBus(LatLng start, LatLng end, string city)
        {
            //116.481499, 39.990475, 116.465063, 39.999538, Coordsys.autonavi, 0, false, "北京"
            //116.299934,39.999233&destination=116.460602,39.805386

            //AMapRouteResults rts = await AMapNavigationSearch.BusNavigation(116.299934, 39.999233, 116.460602, 39.805386, 0, false, city);
            AMapRouteResults rts = await AMapNavigationSearch.BusNavigation(start.longitude, start.latitude, end.longitude, end.latitude, 0, false, city);
            if (rts.Erro == null)
            {
                if (rts.Count == 0)
                {
                    MessageBox.Show("无查询结果");
                    return;
                }

                AMapRoute route = rts.Route;

                List<AMapTransit> transits = route.Transits.ToList();
                List<AMapSegment> segments = transits.FirstOrDefault(p => p.Segments != null).Segments.ToList();



                foreach (var item in segments)
                {
                    int b = 0;
                    amap.AddMarker(new AMapMarkerOptions()
                    {
                        Position = new LatLng(item.Walking.Destination.Lat, item.Walking.Destination.Lon),
                        Title = "Title",
                        Snippet = "Snippet",
                        IconUri = new Uri("Images/man.png", UriKind.Relative),
                    });
                    amap.AddMarker(new AMapMarkerOptions()
                    {
                        Position = new LatLng(item.Walking.Origin.Lat, item.Walking.Origin.Lon),
                        Title = "Title",
                        Snippet = "Snippet",
                        IconUri = new Uri("Images/man.png", UriKind.Relative),
                    });

                    //绘制步行路径
                    foreach (var sp in item.Walking.Steps)
                    {

                       amap.AddPolyline(new AMapPolylineOptions()
                        {
                            Points = latLagsFromString(sp.Polyline),
                            Color = Colors.Red,
                            Width = 4,
                        });
                       // Debug.WriteLine(line.Points.Count);
                        Debug.WriteLine("Walking.Steps:" + sp.Polyline);
                    }
                    //绘制公交路径
                    foreach (AMapBusLine buslines in item.BusLine)
                    {
                        b++;

                        amap.AddPolyline(new AMapPolylineOptions()
                        {
                            Points = latLagsFromString(buslines.Polyline),
                            Color = Colors.Red,
                            Width = 4,
                        });
                        Debug.WriteLine("BusLine:" + buslines.Polyline);
                        foreach (AMapBusStop busstop in buslines.Bus_stops)
                        {
                            amap.AddMarker(new AMapMarkerOptions()
                            {
                                Position = new LatLng(busstop.Location.Lat, busstop.Location.Lon),
                                Title = busstop.Name,
                                Snippet = busstop.Name,
                                IconUri = new Uri("Images/bus.png", UriKind.Relative),
                            });
                        }

                    }


                    Debug.WriteLine("BusLine返回结果数:" + b);

                }

            }
            else
            {
                MessageBox.Show(rts.Erro.Message);
            }
        }

        private async void GetNavigationWalking(LatLng start, LatLng end)
        {
            //116.481028, 39.989643, 116.465302, 40.004717
            AMapRouteResults rts = await AMapNavigationSearch.WalkingNavigation(start.longitude, start.latitude, end.longitude, end.latitude, 0);
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
                            Color = Colors.Blue,
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


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //amap.Clear();

            this.Dispatcher.BeginInvoke(() =>
            {
                if (startLatLng != null && endLatLng != null)
                {
                    GetNavigationDriving(startLatLng, endLatLng);
                    GetNavigationWalking(startLatLng, endLatLng);
                    GetNavigationBus(startLatLng, endLatLng,"北京");
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
                    IconUri = new Uri("Images/AZURE.png", UriKind.Relative),
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
                    IconUri = new Uri("Images/RED.png", UriKind.Relative),
                });
            }
        }

        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            amap.Destory();
            base.OnBackKeyPress(e);
        }

    }
}