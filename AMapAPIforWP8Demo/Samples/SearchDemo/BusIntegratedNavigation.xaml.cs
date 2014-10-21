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
    /// <summary>
    /// 公交换乘导航
    /// </summary>
    public partial class BusIntegratedNavigation : PhoneApplicationPage
    {
        AMap amap;
        LatLng startLatLng;
        LatLng endLatLng;
        AMapPolyline line;
        public BusIntegratedNavigation()
        {
            InitializeComponent();
            this.ContentPanel.Children.Add(amap = new AMap());
            amap.Tap += amap_Tap;
            amap.MarkerClickListener += amap_MarkerClickListener;
        }

        void amap_MarkerClickListener(AMapMarker sender, AMapEventArgs args)
        {
            sender.ShowInfoWindow(new AInfoWindow()
            {
                Title = sender.Title,
                ContentText = sender.Snippet,
            });
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
                    Title = "起点",
                  //  Snippet = "Snippet",
                    IconUri = new Uri("Images/bus_start_pic.png", UriKind.Relative),
                });
                Debug.WriteLine("起点：" + startLatLng.ToString());
            }
            else if (endLatLng == null)
            {
                endLatLng = latLng;
                txtDestination.Text = latLng.latitude + "/" + latLng.longitude;
                amap.AddMarker(new AMapMarkerOptions()
                {
                    Position = endLatLng,
                    Title = "终点",
                    //Snippet = "Snippet",
                    IconUri = new Uri("Images/bus_end_pic.png", UriKind.Relative),
                });
                Debug.WriteLine("终点：" + endLatLng.ToString());
            }

        }

        private async void GetIntegratedNavigation(LatLng start, LatLng end, string city)
        {
            
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
                        Position =new LatLng( item.Walking.Destination.Lat,item.Walking.Destination.Lon),
                        Title = "步行",
                        //Snippet = "Snippet",
                        IconUri = new Uri("Images/man.png", UriKind.Relative),
                    });
                    amap.AddMarker(new AMapMarkerOptions()
                    {
                        Position = new LatLng(item.Walking.Origin.Lat, item.Walking.Origin.Lon),
                        Title = "步行",
                        //Snippet = "Snippet",
                        IconUri = new Uri("Images/man.png", UriKind.Relative),
                    });

                  //绘制步行路径
                    foreach (var sp in item.Walking.Steps)
                    {
                        
                        line = amap.AddPolyline(new AMapPolylineOptions()
                        {
                            Points = latLagsFromString(sp.Polyline),
                            Color = Color.FromArgb(255, 0, 0, 255),
                            Width = 4,
                        });
                        //Debug.WriteLine(line.Points.Count);
                        //Debug.WriteLine("Walking.Steps:" + sp.Polyline);
                    }
                    //绘制公交路径
                    foreach (AMapBusLine buslines in item.BusLine)
                    {
                        b++;

                        amap.AddPolyline(new AMapPolylineOptions()
                        {
                            Points = latLagsFromString(buslines.Polyline),
                            Color = Color.FromArgb(255, 120, 43, 255),
                            Width = 4,
                        });
                        //Debug.WriteLine("BusLine:" + buslines.Polyline);
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


        /// <summary>
        /// 经纬度字符串转换
        /// </summary>
        /// <param name="polyline"></param>
        /// <returns></returns>
        private List<LatLng> latLagsFromString(string polyline)
        {
            List<LatLng> latlng = new List<LatLng>();

            string[] arrystring = polyline.Split(new char[] { ';' });
            foreach (String str in arrystring)
            {
                String[] lnglatds = str.Split(new char[] { ',' });
                latlng.Add(new LatLng(Double.Parse(lnglatds[1]), Double.Parse(lnglatds[0])));
                //Debug.WriteLine("latLagsFromString:" + new LatLng(Double.Parse(lnglatds[1]), Double.Parse(lnglatds[0])).ToString());
            }
            return latlng;

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Dispatcher.BeginInvoke(() =>
               {
                  
                   if (startLatLng != null && endLatLng != null)
                   {
                       GetIntegratedNavigation(startLatLng, endLatLng, txtCity.Text);
                   }
               });

        }

        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            amap.Destory();
            base.OnBackKeyPress(e);
        }

    }
}