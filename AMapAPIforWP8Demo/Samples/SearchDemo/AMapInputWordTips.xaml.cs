using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Com.AMap.Api.Services;
using Com.AMap.Api.Services.Results;
using Microsoft.Phone.Controls;
using System.Diagnostics;

namespace AMapAPIforWP8Demo.Samples.SearchDemo
{
    /// <summary>
    /// 输入提示
    /// </summary>
    public partial class AMapInputWordTips : PhoneApplicationPage
    {
       
        public AMapInputWordTips()
        {
            InitializeComponent();
            this.txtWords.TextChanged += Search_Changed;
            this.txtCity.TextChanged += Search_Changed;
            this.txtTypes.TextChanged += Search_Changed;
           // this.ContentPanel.Children.Add(amap = new AMap());
          
        }

        private void Search_Changed(object sender, TextChangedEventArgs e)
        {
            if (txtWords.Text.Equals(""))
            {
                return;
            }
            GetAmapInputTips(txtWords.Text, txtCity.Text, txtTypes.Text);
        }
        public async void GetAmapInputTips( string words,string city,string types)
        {
            AMapTipResults tipResults = await AMapInputTips.InputTips(words, city, types);
            //this.Dispatcher.BeginInvoke(() =>
            //    {
                    if (tipResults.Erro == null && tipResults.TipList != null)
                    {
                        if (tipResults.TipList.Count == 0)
                        {
                            MessageBox.Show("无查询结果");
                            return;
                        }
                        int i = 0;
                        foreach (AMapTip tip in tipResults.TipList)
                        {

                            i++;
                            Debug.WriteLine(tip.Adcode);
                            Debug.WriteLine(tip.Name);
                            Debug.WriteLine(tip.District);

                        }
                        Debug.WriteLine(i);
                        //绑定列表数据
                        listBox.ItemsSource = tipResults.TipList.ToList();
                        
                    }
                    else
                    {
                        MessageBox.Show(tipResults.Erro.Message);
                    }
                //});
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (txtWords.Text.Equals(""))
            {
                return;
            }
            GetAmapInputTips(txtWords.Text,txtCity.Text,txtTypes.Text);

        }



    }
}