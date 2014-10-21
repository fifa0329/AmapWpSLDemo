using System.Windows;
using Microsoft.Phone.Controls;
using Com.AMap.Api.Maps;

namespace AMapAPIforWP8Demo.Samples.MapDemo
{
    public partial class ShowBuilding : PhoneApplicationPage
    {
        AMap amap;

        public ShowBuilding()
        {
            InitializeComponent();
            this.ContentPanel.Children.Add(amap = new AMap());
            amap.Loaded += amap_Loaded;
        }

        void amap_Loaded(object sender, RoutedEventArgs e)
        {

            amap.MoveCamera(CameraUpdateFactory.ZoomTo(18));

        }

        private void cheIsShow_Checked(object sender, RoutedEventArgs e)
        {
            if (amap == null)
            {
                return;
            }
            amap.IsShowBuliding = true;
        }

        private void cheIsShow_Unchecked(object sender, RoutedEventArgs e)
        {
            if (amap == null)
            {
                return;
            }
            amap.IsShowBuliding = false;
        }

        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            amap.Destory();
            base.OnBackKeyPress(e);
        }

    }
}