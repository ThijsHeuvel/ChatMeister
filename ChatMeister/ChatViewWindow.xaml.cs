using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using ChatMeister.Data;
using Microsoft.EntityFrameworkCore;
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
    public sealed partial class ChatViewWindow : Window
    {
        private Chat SelectedChat { get; set; }
        private List<Message> Messages { get; set; }
        
        private List<User> ParticipatingUsers { get; set; }
        public ChatViewWindow(int chatId)
        {
            this.InitializeComponent();
            using (ChatMeisterDbContext db = new ChatMeisterDbContext())
            {
                SelectedChat = db.Chats.Where(c => c.Id == chatId).First();
            }
            GetMessages(chatId);
            ChatRoomTextBlock.Text = SelectedChat.Name;
            MessageListView.ItemsSource = Messages;
            UserListView.ItemsSource = ParticipatingUsers;
        }

        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            AddNewMessage(NewMessageBox.Text);
        }

        private void GetMessages(int chatId)
        {
            using (ChatMeisterDbContext db = new ChatMeisterDbContext())
            {
                Messages = db.Messages.Where(m => m.ChatId == chatId).OrderBy(m => m.SentAt).Include(m => m.User).ToList();

                ParticipatingUsers = db.Messages
                .Where(m => m.ChatId == chatId) // Select ALL messages that are from this chat
                .Include(m => m.User) // get the user that sent the message
                .Select(m => m.User)  // ONLY return the user
                .Distinct()           // make sure there are no duplicates
                .ToList();
            }
        }

        private void NewMessageBox_KeyUp(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                AddNewMessage(NewMessageBox.Text);
            }
        }

        private void AddNewMessage(string msg)
        {
            if (msg == "")
            {
                return;
            }
            Message message = new Message
            {
                ChatId = SelectedChat.Id,
                UserId = 3,
                Content = msg,
                SentAt = DateTime.Now
            };
            using (ChatMeisterDbContext db = new ChatMeisterDbContext())
            {
                db.Add(message);
                db.SaveChanges();
            }
            NewMessageBox.Text = "";

            GetMessages(SelectedChat.Id); // re-query database to get users
            MessageListView.ItemsSource = null; // Manually refreshing ListView to update messages
            MessageListView.ItemsSource = Messages;
        }
    }
}
