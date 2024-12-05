using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using ChatMeister.Data;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ChatMeister
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.InitializeComponent();
            using (ChatMeisterDbContext db = new ChatMeisterDbContext())
            {
                db.Database.EnsureCreated();
                chatListView.ItemsSource = db.Chats.OrderBy(c => c.Id);
            }
        }

        private void chatListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            Chat selectedChat = (Chat)e.ClickedItem;
            ChatViewWindow chatViewWindow = new ChatViewWindow(selectedChat.Id);
            chatViewWindow.Activate();
        }   
    }
}
