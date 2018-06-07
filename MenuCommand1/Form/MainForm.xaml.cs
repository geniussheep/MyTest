using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using MessageBox = System.Windows.MessageBox;

namespace MenuCommand.Form
{
    public partial class MainForm : Window
    {

        #region 加载列表
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            
        }
        #endregion

        #region 确认提交事件
        /// <summary>
        /// 确认提交事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SubmitEvent(object sender, RoutedEventArgs e)
        {
            try
            {
                
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion

        #region 全选
        private void HadAddSelectAll_ClickEvent(object sender, RoutedEventArgs e)
        {
            
        }

        private void NoAddSelectAll_ClickEvent(object sender, RoutedEventArgs e)
        {
            
        }

        private void NoExistSelectAll_ClickEvent(object sender, RoutedEventArgs e)
        {

        }

        private List<T> FindVisualChild<T>(DependencyObject obj) where T : DependencyObject
        {
            List<T> list = new List<T>();
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                var child = VisualTreeHelper.GetChild(obj, i);
                var item = child as T;
                if (item != null)
                {
                    list.Add(item);
                }
                else
                {
                    var childOfChildren = FindVisualChild<T>(child);
                    if (childOfChildren != null)
                    {
                        list.AddRange(childOfChildren);
                    }
                }
            }
            return list;
        }
        #endregion

        #region CheckBox选中事件
        private void TemplateHadAddCheckBox_ClickEvent(object sender, RoutedEventArgs e)
        {

        }

        private void TemplateNoAddCheckBox_ClickEvent(object sender, RoutedEventArgs e)
        {

        }

        private void TemplateNoExistCheckBox_ClickEvent(object sender, RoutedEventArgs e)
        {

        }
        #endregion
    }

    public class ListViewItem
    {
        public string Name { get; set; }
    }
}
