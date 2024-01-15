﻿
using Opc.Ua.Client.Controls;
using Opc.Ua;
using OpcUaHelper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace OpcUaTest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (OpcUaHelper.FormBrowseServer fbs = new OpcUaHelper.FormBrowseServer())
            {
                fbs.ShowDialog();
            }
        }




        private OpcUaHelper.OpcUaClient opcUaClient;

        private void Form1_Load(object sender, EventArgs e)
        {
            short a = 1;
            int b = a == 0 ? 1 : 0;


            opcUaClient = new OpcUaHelper.OpcUaClient();
            ////opcUaClient.ConnectServer("http://117.48.203.204:62547/DataAccessServer");

            //opcUaClient.ConnectServer("opc.tcp://192.168.250.1:4840");

        }

    
        private async void button2_Click(object sender, EventArgs e)
        {
            Int32[] data = await opcUaClient.ReadNodeAsync<Int32[]>("ns=4;s=intPLC_CCD1");
            ;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            opcUaClient.Disconnect();
        }

        // ns=2;s=Robots/RobotA/RobotMode
        // ns=2;s=Robots/RobotA/UserFloat

        private void button3_Click(object sender, EventArgs e)
        {
            //Opc.Ua.DataValue value = opcUaClient.ReadNode("ns=2;s=Robots/RobotA/RobotMode");

            Opc.Ua.DataValue value = opcUaClient.ReadNode("ns=4;s=intPLC_CCD1");

            // 一个数据的类型是不是数组由 value.WrappedValue.TypeInfo.ValueRank 来决定的
            // -1 说明是一个数值
            // 1  说明是一维数组，如果类型BuiltInType是Int32，那么实际是int[]
            // 2  说明是二维数组，如果类型BuiltInType是Int32，那么实际是int[,]
            if (value.WrappedValue.TypeInfo.BuiltInType == Opc.Ua.BuiltInType.Int32)
            {
                if (value.WrappedValue.TypeInfo.ValueRank == -1)
                {
                    int temp = (int)value.WrappedValue.Value;               // 最终值
                }
                else if (value.WrappedValue.TypeInfo.ValueRank == 1)
                {
                    int[] temp = (int[])value.WrappedValue.Value;           // 最终值
                }
                else if (value.WrappedValue.TypeInfo.ValueRank == 2)
                {
                    int[,] temp = (int[,])value.WrappedValue.Value;         // 最终值
                }
            }
            else if (value.WrappedValue.TypeInfo.BuiltInType == Opc.Ua.BuiltInType.UInt32)
            {
                uint[] temp = (uint[])value.WrappedValue.Value;                 // 数组的情况参照上面的例子
            }
            else if (value.WrappedValue.TypeInfo.BuiltInType == Opc.Ua.BuiltInType.Float)
            {
                float temp = (float)value.WrappedValue.Value;               // 数组的情况参照上面的例子
            }
            else if (value.WrappedValue.TypeInfo.BuiltInType == Opc.Ua.BuiltInType.String)
            {
                string temp = (string)value.WrappedValue.Value;             // 数组的情况参照上面的例子
            }
            else if (value.WrappedValue.TypeInfo.BuiltInType == Opc.Ua.BuiltInType.DateTime)
            {
                DateTime temp = (DateTime)value.WrappedValue.Value;         // 数组的情况参照上面的例子
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string[] nodes = new string[]
            {
                "ns=4;s=intPLC_CCD1",
                "ns=4;s=intPLC_CCD"
            };

            // 因为不能保证读取的节点类型一致，所以只提供统一的DataValue读取，每个节点需要单独解析
            foreach (Opc.Ua.DataValue value in opcUaClient.ReadNodes(nodes))
            {
                // 获取到了值，具体的每个变量的解析参照上面类型不确定的解析
                object data = value.WrappedValue.Value;
                // 下面写你自己的操作

            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            // 批量写入的代码
            string[] nodes = new string[]
            {
                "ns=4;s=intPLC_CCD1",
            };
            object[] data = new object[]
            {
                new Int32[500],
            };

            // 都成功返回True，否则返回False
            bool result = opcUaClient.WriteNodes(nodes, data);
            ;
        }

        private void button6_Click( object sender, EventArgs e )
        {
            // 获取本机已经注册的服务器地址
            string endpointUrl = new Opc.Ua.Client.Controls.DiscoverServerDlg().ShowDialog(opcUaClient.AppConfig, null);
            // 获取其他服务器注册的地址，注意，需要该IP的安全策略配置正确
            // string endpointUrl = new Opc.Ua.Client.Controls.DiscoverServerDlg( ).ShowDialog( opcUaClient.AppConfig, "192.168.0.100" );

            if (!string.IsNullOrEmpty(endpointUrl))
            {
                // 获取到的需要操作的服务器地址
            }
        }
    }
}
