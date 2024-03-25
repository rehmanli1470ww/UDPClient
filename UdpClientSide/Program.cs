using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Channels;

IPAddress ip = IPAddress.Parse("192.168.0.103");
int port = 27001;

IPEndPoint remoteEp =new IPEndPoint(ip, port);  

UdpClient client = new UdpClient();

while (true)
{
    Console.Write("Messaji daxil edin : ");
    string msg = Console.ReadLine();
    byte[] bytes=Encoding.UTF8.GetBytes(msg);
    client.Send(bytes,bytes.Length,remoteEp);
}