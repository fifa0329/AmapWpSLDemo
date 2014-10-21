using System;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Com.AMap.Api.Maps;

namespace AMapAPIforWP8Demo.Samples.MapDemo
{
    public partial class Screen : PhoneApplicationPage
    {
        AMap amap;
        private ProgressIndicator _progressIndicator = new ProgressIndicator();
        public Screen()
        {
            InitializeComponent();
            this.ContentPanel.Children.Add(amap = new AMap());

        }

        private async void ApplicationBarIconButton_Click(object sender, EventArgs e)
        {

            _progressIndicator.Text = "正在截图并保存....";
            _progressIndicator.IsIndeterminate = true;
            _progressIndicator.IsVisible = true;
            var bitmap = await amap.GetScreenAsync();
            var stream = new System.IO.MemoryStream();
            System.Windows.Media.Imaging.Extensions.SaveJpeg(bitmap, stream, bitmap.PixelWidth, bitmap.PixelHeight, 0, 100);
            stream.Position = 0;
            var mediaLib = new Microsoft.Xna.Framework.Media.MediaLibrary();
            var datatime = System.DateTime.Now;
            var fileName = string.Format("{0}", datatime.ToString("yyddHHmmss"));

            mediaLib.SavePicture(fileName, stream);
            SystemTray.SetProgressIndicator(this, _progressIndicator);
            SystemTray.ProgressIndicator.IsVisible = false;
        }

        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            amap.Destory();
            base.OnBackKeyPress(e);
        }

    }
}