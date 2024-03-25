using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfClient_UdpClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public UdpClient client;
        public IPAddress remoteIp;
        public int remotePort;
        public IPEndPoint remoteEp;
        public bool isCheck=false;
        public bool Stop= false;
        public bool Reguest= true;
        public MainWindow()
        {
            InitializeComponent();
            remoteIp = IPAddress.Parse("192.168.0.103");
            remotePort = 27001;
            remoteEp=new IPEndPoint(remoteIp, remotePort);  
            client=new UdpClient(); 

        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!isCheck)
            {
                
                isCheck = true;
                byte[] buffer = new byte[ushort.MaxValue - 29];
                
                List<byte> list =new List<byte>();
                int maxLen=buffer.Length;
                int len = 0;
                if (Reguest)
                {
                    await client.SendAsync(buffer, buffer.Length, remoteEp);
                    Reguest = false;
                }
                while (true)
                {
                    if (Stop)
                    {
                        break;
                    }
                    do
                    {
                        try
                        {
                            var resault= await client.ReceiveAsync();
                            buffer=resault.Buffer;
                            len=buffer.Length;
                            list.AddRange(buffer);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                            
                        }

                    } while (len==maxLen);
                    
                    var image=await ByteToImageAsync(list.ToArray());
                    if (image is not null)
                    {
                        ScrenImage.Source= image;   
                    }
                    list.Clear();

                }
                Stop = false;  

            }

        }
        public async Task<BitmapImage> ByteToImageAsync(byte[] bytes)
        {
            var image = new BitmapImage();
            image.BeginInit();
            image.StreamSource = new MemoryStream(bytes);
            image.CacheOption = BitmapCacheOption.OnLoad;
            image.EndInit();
            return image;
        }

        private async void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (isCheck)
            {
                isCheck = false;
            }
            Stop = true;    
        }
    }

    
}

