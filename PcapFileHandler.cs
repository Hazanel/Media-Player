using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using SharpPcap;
using SharpPcap.LibPcap;
using PacketDotNet;
using System.Windows.Forms;


namespace DSPMediaPlayer
{

    public class Single_Traffic_Path
    {
        public string packetSourceAdress;
        public string packetDestAdress;
        public string PortSourceAddress;
        public string PortDestAddress;
        public CaptureFileWriterDevice FileWriter;
        public string pcapFileName;
        public long   FileSize;
        public string FileSizeStr;
    };
    
    public class PcapFileHandler
    {
        private static PcapFileHandler PcapFileHandlerObj = null;

        public PcapFileHandler(string aPcapDir)
        {
            PcapDir = aPcapDir;
        }
        
       
        public  static List<Single_Traffic_Path> Traffic_Path_List = new List<Single_Traffic_Path>();
        public static ICaptureDevice device;
        private static int packetIndex = 0;
        private static string PcapDir;



        public static void PcapFileHandlerProcess(string aPcapDir, string aPcapFile)
        {
            string ver = SharpPcap.Version.VersionString;

            if (null == PcapFileHandlerObj)
            {
                PcapFileHandlerObj = new PcapFileHandler(aPcapDir);
            }

            try
            {
                // Get an offline device
                device = new CaptureFileReaderDevice(aPcapFile);

                // Open the device
                device.Open();
            }
            catch (Exception e)
            {
                MessageBox.Show("Caught exception when opening file" + e.ToString());
                return;
            }

            // Register our handler function to the 'packet arrival' event
            device.OnPacketArrival +=
                new PacketArrivalEventHandler(device_OnPacketArrival);

           
            // Start capture 'INFINTE' number of packets
            // This method will return when EOF reached.
            device.Capture();

            // Close the pcap device
            device.Close();

        }


        /// <summary>
        /// Prints the source and dest MAC addresses of each received Ethernet frame
        /// </summary>
        private  static void device_OnPacketArrival(object sender, CaptureEventArgs e)
        {
            //if (e.Packet.LinkLayerType == PacketDotNet.LinkLayers.Ethernet)//EladH
            //{
            try
            {
                var packet = PacketDotNet.Packet.ParsePacket(e.Packet.LinkLayerType, e.Packet.Data);

                //var ethernetPacket = (PacketDotNet.EthernetPacket)packet;


                var UDPPacket = PacketDotNet.UdpPacket.GetEncapsulated(packet);
                var TCPPacket = PacketDotNet.TcpPacket.GetEncapsulated(null);//just preparing - no need at the moment
                if (UDPPacket != null)
                {
                    Single_Traffic_Path Path_To_List = new Single_Traffic_Path();
                    var ipPacket = (UDPPacket != null) ? (PacketDotNet.IpPacket)UDPPacket.ParentPacket : (PacketDotNet.IpPacket)TCPPacket.ParentPacket;
                    System.Net.IPAddress srcIp = ipPacket.SourceAddress;
                    Path_To_List.packetSourceAdress = srcIp.ToString();
                    if (!Char.IsNumber(Path_To_List.packetSourceAdress, 0))
                        return;

                    System.Net.IPAddress dstIp = ipPacket.DestinationAddress;
                    Path_To_List.packetDestAdress = dstIp.ToString();
                    int srcPort = (UDPPacket != null) ? UDPPacket.SourcePort : TCPPacket.SourcePort;
                    Path_To_List.PortSourceAddress = srcPort.ToString();
                    int dstPort = (UDPPacket != null) ? UDPPacket.DestinationPort : TCPPacket.DestinationPort;
                    Path_To_List.PortDestAddress = dstPort.ToString();

                    //Console.WriteLine("{0} At: {1}:{2}: MAC:{3} -> MAC:{4}",
                    //            packetIndex,
                    //            e.Packet.Timeval.Date.ToString(),
                    //            e.Packet.Timeval.Date.Millisecond,
                    //            ethernetPacket.SourceHwAddress,
                    //            ethernetPacket.DestinationHwAddress);

                    bool IsItemInList = false;
                    if (Traffic_Path_List.Count.Equals(0))
                    {
                        string capFile = Path_To_List.packetSourceAdress + "_" + Path_To_List.PortSourceAddress + "_" + Path_To_List.packetDestAdress + "_" + Path_To_List.PortDestAddress + ".pcap";
                        Path_To_List.FileWriter = new CaptureFileWriterDevice(PcapDir + '\\' + capFile);
                        Path_To_List.pcapFileName = capFile;
                        Path_To_List.FileWriter.Write(e.Packet);
                        Traffic_Path_List.Add(Path_To_List);


                    }
                    else
                    {

                        foreach (Single_Traffic_Path Display_Cell in Traffic_Path_List)
                        {

                            if (
                                Path_To_List.packetSourceAdress.Equals(Display_Cell.packetSourceAdress)
                                &&
                                Path_To_List.packetDestAdress.Equals(Display_Cell.packetDestAdress)
                                &&
                                Path_To_List.PortSourceAddress.Equals(Display_Cell.PortSourceAddress)
                                &&
                                Path_To_List.PortDestAddress.Equals(Display_Cell.PortDestAddress)
                                )
                            {
                                IsItemInList = true;
                                Display_Cell.FileWriter.Write(e.Packet);
                                break;
                            }
                        }

                        if (IsItemInList.Equals(false))
                        {
                            string capFile = Path_To_List.packetSourceAdress + "_" + Path_To_List.PortSourceAddress + "_" + Path_To_List.packetDestAdress + "_" + Path_To_List.PortDestAddress + ".pcap";
                            Path_To_List.FileWriter = new CaptureFileWriterDevice(PcapDir + '\\' + capFile);
                            Path_To_List.pcapFileName = capFile;
                            Path_To_List.FileWriter.Write(e.Packet);
                            Traffic_Path_List.Add(Path_To_List);
                        }
                    }



                }


                packetIndex++;
                //}
            }
            catch
            {

            }

               
        }


    }
}
