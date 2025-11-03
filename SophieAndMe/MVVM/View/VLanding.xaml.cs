﻿
using System.IO;
using System.Reflection;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Web.WebView2.Core;
using SophieAndMe.MVVM.Model;
using SophieAndMe.MVVM.ViewModel;

namespace SophieAndMe.MVVM.View
{
    public partial class VLanding : UserControl
    {
        public VLanding(MainViewModel mainVm)
        {
            InitializeComponent();
            
            WeakReferenceMessenger.Default.Register<MediatorLanding.JsCallMessage>(this, (r, m) =>
            {
                WebViewAll.CoreWebView2.PostWebMessageAsJson(m.Value);
            });
            Loaded += async (s, e) =>
            {
                await WebViewAll.EnsureCoreWebView2Async();
                WebViewAll.CoreWebView2.Settings.AreDefaultContextMenusEnabled = false;
                WebViewAll.CoreWebView2.WebMessageReceived += OnWebMessageReceived;
                WebViewAll.DefaultBackgroundColor = System.Drawing.Color.Transparent;
                // CoreWebView2.SetVirtualHostNameToFolderMapping();
                // WebViewAll.CoreWebView2.OpenDevToolsWindow();
                string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\HTML_Const\Landing\Landing2.html");
                var uri = new Uri(Path.GetFullPath(path));
                WebViewAll.Source = uri;
                WebViewAll.CoreWebView2.NavigationCompleted += (sender, args) => { 
                    var vm = new VLandingModel(mainVm);
                    this.DataContext = vm;};
            };
            
            Unloaded += (s, e) =>
            {
                WeakReferenceMessenger.Default.Unregister<MediatorLanding.JsCallMessage>(this);
            };
        }
        private void OnWebMessageReceived(object? sender, CoreWebView2WebMessageReceivedEventArgs e)
        {
            try
            {
                var msgSoR = JsonSerializer.Deserialize<MediatorLanding.WebJsMessage>(e.WebMessageAsJson);
                if (msgSoR != null)
                {
                    WeakReferenceMessenger.Default.Send(new MediatorLanding.JstoAppMessage(msgSoR.action, msgSoR.matier, msgSoR.name, msgSoR.question, msgSoR.imgQuestion, msgSoR.rep, msgSoR.imgRep));
                }
            
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur JS: " + ex.Message);
            }
        }
    }
}