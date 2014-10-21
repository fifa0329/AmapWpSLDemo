using System.Windows;
using Microsoft.Phone.Controls;
using Com.AMap.Api.Maps;
using Com.AMap.Api.Maps.Model;

namespace AMap_WP8_Api_Demos_v2._2.Samples
{
    public partial class MapCirclePage : PhoneApplicationPage
    {
        AMap amap;
        AMapCircle circle;
        public MapCirclePage()
        {
            InitializeComponent();
            this.ContentPanel.Children.Add(amap = new AMap());

            this.btnVisible.IsEnabled = false;
        }


        private void Button_DrawCircle_Click(object sender, RoutedEventArgs e)
        {
            circle = amap.AddCircle(new AMapCircleOptions()
            {
                 Center=amap.Center,
                 Radius=2000,
            });
            this.btnVisible.IsEnabled = true;
        }

       
        private void Button_Destroy_Click(object sender, RoutedEventArgs e)
        {
            if (circle != null)
            {
                circle.Destroy();
            }
            this.btnVisible.IsEnabled = false;
        }

        private void Button_Visible_Click(object sender, RoutedEventArgs e)
        {
            if (circle!= null)
            {
                circle.Visible = false;
            }
            this.btnVisible.IsEnabled = false;
        }

        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            amap.Destory();
            base.OnBackKeyPress(e);
        }

    }
}