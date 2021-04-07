/**
 * DNS Test Tool
 * 
 *  Copyright (C) 2021 Kema All Rights Reserved.
 * 
 * 
 * How it work?
 *  change content size and replace bytes.
 *  last period to '02', else '04' 
 *  * I don't know the details either
 * 
 * Reference
 *  https://www.atmarkit.co.jp/ait/articles/1601/29/news014_2.html
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            int fs = Environment.TickCount;
            byte[] header = new byte[12];
            header[0] = 00; // ID
            header[1] = 00;
            header[2] = 01; // Query
            header[3] = 00;
            header[4] = 00; // QDCount
            header[5] = 01;
            header[6] = 00; // ANCount
            header[7] = 00;
            header[8] = 00; // NSCount
            header[9] = 00;
            header[10] = 00; // ARCount
            header[11] = 00;

            byte[] content = new byte[12];
            content[0] = 03; //offset
            content[1] = (byte)'w';
            content[2] = (byte)'w';
            content[3] = (byte)'w';
            content[4] = 02; //.
            content[5] = (byte)'j';
            content[6] = (byte)'p';
            content[7] = 00; //offset
            content[8] = 00; // A
            content[9] = 01;
            content[10] = 00; // IN
            content[11] = 01;

            byte[] data = new byte[header.Length + content.Length];
            Array.Copy(header, 0, data, 0, header.Length);
            Array.Copy(content, 0, data, header.Length, content.Length);

            IPAddress ip = IPAddress.Parse("8.8.8.8");
            IPEndPoint remote = new IPEndPoint(ip, 53);

            UdpClient client = new UdpClient();

            int et = Environment.TickCount;
            Console.WriteLine("Sending Packet.");
            client.Send(data, data.Length, remote);

            IPEndPoint remoteIP = null;
            byte[] buffer = client.Receive(ref remoteIP);

            int le = Environment.TickCount;

            Console.WriteLine("Elapsed {0}ms", le - fs);
            Console.WriteLine("Elapsed creating data {0}ms", et - fs);
            Console.WriteLine(BitConverter.ToString(data));

            byte[] answer = new byte[buffer.Length - data.Length];
            Array.Copy(buffer, data.Length, answer, 0, answer.Length);

            Console.WriteLine(BitConverter.ToString(answer));

            // offset
            //answer[0], answer[1]
            // A
            //answer[2], answer[3]
            // IN
            //answer[4], answer[5]

            int offset = answer.Length - 4;
            int i0 = answer[offset + 0];
            int i1 = answer[offset + 1];
            int i2 = answer[offset + 2];
            int i3 = answer[offset + 3];

            Console.WriteLine("{0}.{1}.{2}.{3}", i0, i1, i2, i3);

            Console.ReadLine();

        }
    }
}
