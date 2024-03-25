using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Imaging;










IPAddress ip = IPAddress.Parse("192.168.0.103");
int port = 27001;

IPEndPoint endPoint=new IPEndPoint(ip, port);
IPEndPoint remoteEP = new IPEndPoint(IPAddress.Any, 0);

UdpClient listener = new UdpClient(endPoint);

while (true)
{
    UdpReceiveResult result =await listener.ReceiveAsync();
    _ = Task.Run(async () =>
    {
        remoteEP = result.RemoteEndPoint;
        while (true)
        {
            Image screen = await TakeScreanShotAsync();
            byte[] ImageBytes = await ImageToByte(screen);
            IEnumerable<byte[]> chunks=ImageBytes.Chunk(ushort.MaxValue - 29);
            foreach (byte[] chunk in chunks)
            {
                await listener.SendAsync(chunk, chunk.Length, remoteEP);

            }
        }
    });

}

//Ekranin scren edilmesi
async Task<Image> TakeScreanShotAsync() 
{
    
    var with = 2020;
    var heght = 1080;
    Bitmap bitmap = new Bitmap(with,heght);
    using Graphics graphics = Graphics.FromImage(bitmap);
    graphics.CopyFromScreen(0,0,0,0,bitmap.Size);
    return bitmap;
}

async Task<byte[]> ImageToByte(Image image) 
{
    using MemoryStream stream = new MemoryStream();
    image.Save(stream, ImageFormat.Jpeg);

    return stream.ToArray();
}

