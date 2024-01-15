using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Net;
using System.Net.Sockets;


namespace 通讯接口测试_JGP__20200415
{
    class BailCilent
    {
        private System.Net.Sockets.TcpClient client=new TcpClient();
        public System.Net.Sockets.NetworkStream baliStream;
        public string SN;
       

        /* Open Connection to Bali Server
            Description:
                Opens TCP socket, connects to Bali server, and initializes instance variables
                TcpClient client and NetworkStream baliStream

            Input(s):
                - (string) serverAddr   - IPv4 Address of Bali Server (10.0.0.2 by default)
                - (Int32) port          - Bali server TCP port (1111 by default)
            Output(s):
                none
            Exception(s):
                BaliClientException   - Thrown if issue opening TCP socket or 
                                        connecting to Bali server
        */
        public void openServerConnection(string serverAddr, Int32 port,string localip)
        {
            try
            {
                // Connect to Server
                IPAddress ipLocal = IPAddress.Parse(localip);
                IPAddress ipSever = IPAddress.Parse(serverAddr);
                try
                {
                    client.Client.Bind(new IPEndPoint(ipLocal, 0));
                    client.Client.Connect(new IPEndPoint(ipSever, port));
                    //client = new System.Net.Sockets.TcpClient(serverAddr, port);
                }
                catch
                {
                }
          //      client = new System.Net.Sockets.TcpClient(serverAddr, port);
                
                // Get NetworkStream
                baliStream = client.GetStream();
            }
            catch (ArgumentNullException e)
            {
                throw (new BaliClientException("Null Hostname", e));
            }
            catch (ArgumentOutOfRangeException e)
            {
                throw (new BaliClientException("Port out of range", e));
            }
            catch (System.Net.Sockets.SocketException e)
            {
                throw (new BaliClientException("Unable to create socket", e));
            }
            catch (InvalidOperationException e)
            {
                throw (new BaliClientException("Client is not connected to Bali server", e));
            }
        }
        public bool isconnected()
        {
            if (client != null)
            {
                if (client.Connected)
                {
                    return true;


                }

                else
                {
                    return false;

                }
            }
            else
            {
                return false;
            }

        }





        /* Create 'start' Message
            Message Description:
                MUST BE THE FIRST MESSAGE SENT
                Signals to Bali server that we will be sending data for this DUT
            Message Template(s):
                "<SN>@start\n"

            Input(s):
                - (string) serialNumber - DUT Serial Number
            Output(s):
                - (string) msg          - 'start' message to send to Bali server
            Exception(s):
                N/A
        */
        public string checkSfcUnit(string serialNumber)
        {
            SN = serialNumber;      // 检查是否有上一站中板二维码信息
            return String.Format("{0}@sfc_unit_check@DEVELOPMENT10\n", SN);
        }
        public string createStartMessage(string serialNumber)
        {
            SN = serialNumber;      // Set SN instance variable
            return String.Format("{0}@start\n", SN);
        }

        public string createStartAuditMessage(string serialNumber)
        {
            SN = serialNumber;      // Set SN instance variable
            return String.Format("{0}@start@audit\n", SN);
        }
        /* Create 'dut_pos' Message
            Message Description:
                Specifies station/fixture ID and nest/jig/head ID
            Message Template(s):
                "<SN>@dut_pos@<fixtureID>@<headID>\n"

            Input(s):
                - (string) fixtureID    - [16 CHAR MAX] ID of the station/fixture testing DUT
                - (string) headID       - [8 CHAR MAX] ID of the specific nest/jig/head testing DUT
            Output(s):
                - (string) msg          - 'dut_pos' message to send to Bali server
            Exception(s):
                BaliClientException - Thrown if 'start' message has not been sent yet
        */
        public string createPriority(string id)
        {
            if (SN == null)
            {
                throw (new BaliClientException("MUST SEND START MESSAGE BEFORE DUT_POS MESSAGE"));
            }
            return String.Format("{0}@priority@{1}\n", SN, id);
        }

        public string createTestfail(string station_name)
        {
            if (SN == null)
            {
                throw (new BaliClientException("MUST SEND START MESSAGE BEFORE DUT_POS MESSAGE"));
            }
            return String.Format("{0}@test_fail@{1}@ NG part\n", SN, station_name);
        }

        public string createDUT_POSMessage(string fixtureID, string headID)
        {
            if (SN == null)
            {
                throw (new BaliClientException("MUST SEND START MESSAGE BEFORE DUT_POS MESSAGE"));
            }
            return String.Format("{0}@dut_pos@{1}@{2}\n", SN, fixtureID, headID);
        }


        /* Create 'pdata' Message
            Message Description:
                DUT data from a single parameter
            Message Template(s):
                "<SN>@pdata@<TestName>@<TestValue>\n"
                "<SN>@pdata@<TestName>@<TestValue>@<LowerLimit>@<UpperLimit>\n"
                "<SN>@pdata@<TestName>@<TestValue>@<LowerLimit>@<UpperLimit>@<MeasurementUnit>\n"

            Input(s):
                - (string) TestName         - Name of parameter
                - (float) TestValue         - Measured parameter value
                - (float) LowerLimit        - [OPTIONAL] Parameter's lower limit
                - (float) UpperLimit        - [OPTIONAL] Parameter's upper limit
                - (string) MeasurementUnit  - [OPTIONAL] Parameter unit of measurement (i.e. mm, um, V, ...)
            Output(s):
                - (string) msg      - 'pdata' message to send to Bali server
            Exception(s):
                BaliClientException - Thrown if 'start' message has not been sent yet
        */
        public string createPDataMessage(string testName, int testValue)
        {
            if (SN == null)
            {
                throw (new BaliClientException("MUST SEND START MESSAGE BEFORE PDATA MESSAGE"));
            }
            return String.Format("{0}@pdata@{1}@{2}\n", SN, testName, testValue);
        }
        public string createPDataMessage(string testName, float testValue, float lowerLimit, float upperLimit)
        {
            if (SN == null)
            {
                throw (new BaliClientException("MUST SEND START MESSAGE BEFORE PDATA MESSAGE"));
            }
            return String.Format("{0}@pdata@{1}@{2}@{3}@{4}\n", SN, testName, testValue, lowerLimit, upperLimit);
        }
        public string createPDataMessage(string testName, int  testValue, string  lowerLimit, string  upperLimit, string measurementUnit)
        {
            if (SN == null)
            {
                throw (new BaliClientException("MUST SEND START MESSAGE BEFORE PDATA MESSAGE"));
            }
            return String.Format("{0}@pdata@{1}@{2}@{3}@{4}@{5}\n", SN, testName, testValue, lowerLimit, upperLimit, measurementUnit);
        }
        public string createPDataMessage(string testName, string  testValue, string lowerLimit, string upperLimit, string measurementUnit)
        {
            if (SN == null)
            {
                throw (new BaliClientException("MUST SEND START MESSAGE BEFORE PDATA MESSAGE"));
            }
            return String.Format("{0}@pdata@{1}@{2}@{3}@{4}@{5}\n", SN, testName, testValue, lowerLimit, upperLimit, measurementUnit);
        }
        /* Create 'submit' Message
            Message Description:
                MUST BE THE LAST MESSAGE SENT
                Signals to Bali server that we have finished sending data for this DUT
            Message Template(s):
                "<SN>@submit@<SWVersion>\n"

            Input(s):
                - (string) SWVersion- Version of AOI Station SW (the SW which sends messages to Bali)
            Output(s):
                - (string) msg      - 'submit' message to send to Bali server
            Exception(s):
                BaliClientException - Thrown if 'start' message has not been sent yet
        */
        public string createStartTimeMessage(DateTime dt)
        {
            if (SN == null)
            {
                throw (new BaliClientException("MUST SEND START MESSAGE BEFORE PDATA MESSAGE"));
            }
            return String.Format("{0}@start_time@{1}\n", SN, dt.ToString("yyyy-MM-dd HH:mm:ss"));
        }
        public string createStopTimeMessage(DateTime dt)
        {
            if (SN == null)
            {
                throw (new BaliClientException("MUST SEND START MESSAGE BEFORE PDATA MESSAGE"));
            }
            return String.Format("{0}@stop_time@{1}\n", SN, dt.ToString("yyyy-MM-dd HH:mm:ss"));
        }

        /// <summary>
        /// Weld_time
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>

        public string createWeldStartTimeMessage(DateTime dt)
        {
            if (SN == null)
            {
                throw (new BaliClientException("MUST SEND START MESSAGE BEFORE PDATA MESSAGE"));
            }
            return String.Format("{0}@weld_start_time@{1}\n", SN, dt.ToString("yyyy-MM-dd HH:mm:ss"));
        }
        public string createWeldStopTimeMessage(DateTime dt)
        {
            if (SN == null)
            {
                throw (new BaliClientException("MUST SEND START MESSAGE BEFORE PDATA MESSAGE"));
            }
            return String.Format("{0}@weld_stop_time@{1}\n", SN, dt.ToString("yyyy-MM-dd HH:mm:ss"));
        }

        public string createAttrMessage(string attributeName, string attribute)
        {
            if (SN == null)
            {
                throw (new BaliClientException("MUST SEND START MESSAGE BEFORE PDATA MESSAGE"));
            }
            return String.Format("{0}@attr@{1}@{2}\n", SN, attributeName, attribute);
        }
        public string createjpAttrMessage(string SN, string attributeName, string attribute)
        {
            if (SN == null)
            {
                throw (new BaliClientException("MUST SEND START MESSAGE BEFORE PDATA MESSAGE"));
            }
            return String.Format("{0}@attr@{1}@{2}\n", SN, attributeName, attribute);
        }
        public string createCancelMessage()
        {
            if (SN == null)
            {
                throw (new BaliClientException("MUST SEND START MESSAGE BEFORE PDATA MESSAGE"));
            }
            return String.Format("{0}@cancel\n",SN);
        }


        public string createSubmitMessage(string SWVersion)
        {
            if (SN == null)
            {
                throw (new BaliClientException("MUST SEND START MESSAGE BEFORE SUBMIT MESSAGE"));
            }
            string serialNum = SN;
            SN = null;                  // Reset SN for next DUT
            return String.Format("{0}@submit@{1}\n", serialNum, SWVersion);
        }
        /* Send Message to Bali Server
            Input(s):
                - (string) msg      - Data message to send to Bali server 
            Output(s):
                none
            Exception(s):
                BaliClientException - Thrown on all socket write exceptions
        */
        public void sendMessage(string msg)
        {
            try
            {
                // Convert Message to Bytes
                Byte[] data = System.Text.Encoding.ASCII.GetBytes(msg);
                // Send Message to Bali Server
                baliStream.Write(data, 0, data.Length);
                Console.WriteLine("->SentMsg:\n\t<{0}>", Regex.Replace(msg, @"\n", "\\n"));
            }
            catch (ArgumentNullException e)
            {
                throw (new BaliClientException("Error with Message to Send to Bali", e));
            }
            catch (ArgumentOutOfRangeException e)
            {
                throw (new BaliClientException("Error with size of Message to Send to Bali", e));
            }
            catch (System.IO.IOException e)
            {
                throw (new BaliClientException("Unable to write Message to socket", e));
            }
            catch (ObjectDisposedException e)
            {
                throw (new BaliClientException("No TcpClient - Run openServerConnection() to open connection to Bali Server", e));
            }
        }

        /* Bali Reply Description
            Bali replies fall into 3 categories (OK,Err,Bad)
            All Bali replies are terminated by termination string ("}@\n")
            Bali replies contain a string message (msg) of length zero or greater

            Bali Reply Types:
                "ok@{<msg>}@\n"     -> Message to Bali server successful
                                        -> Can proceed with sending next command or starting next DUT
                "err@{<msg>}@\n"    -> Panda Message Error - Could indicate:
                                        - Bad Parametric Value
                                        - Bali server unable to reach Panda
                                        - Bali server not properly Groundhogged
                "bad@{<msg>}@\n"    -> Bali Message Error
                                        -> The message to Bali was not in the correct format
                                        -> You won't get this response if you use the message creation
                                            methods in this class.
        */
        /* Get Reply Message from Bali Server
            Input(s):
                none
            Output(s):
                - (string) reply    - Bali Server's response to prior command
            Exception(s):
                BaliClientException - Thrown on all socket read exceptions
        */
        public string getReply()
        {
            // Initialize
            string reply = String.Empty;
            int BUFSIZE = 32;
            Byte[] buf;
            bool done = false;

            // Get Reply by reading BUFSIZE-byte chunks from the TCP socket until
            // we get the reply termination string - "}@\n"
            try
            {
                while (!done)
                {
                    // Reinitialize our buffer
                    buf = new Byte[BUFSIZE];
                    // Read BUFSIZE bytes from TCP socket
                    int bytesRead=0;
                    baliStream.ReadTimeout = 2000;
                    bytesRead = baliStream.Read(buf, 0, buf.Length);
                    // Check if any bytes were read
                    if (bytesRead == 0)
                    {
                       // throw (new BaliClientException("TCP Connection shutdown by server before finishing reply"));
                    }
                    // Convert buf from bytes to string
                    String tmpReply = System.Text.Encoding.ASCII.GetString(buf);
                    // Concatenate this portion of the reply to the full reply
                    reply = String.Concat(reply, tmpReply);
                    // Check if this portion of the reply contains termination string "}@\n"
                    if (reply.Contains("}@\n"))
                    {
                        // Cut off all characters after termination string
                        int lengthOfValidString = reply.LastIndexOf("}@\n") + 3;
                        reply = reply.Substring(0, lengthOfValidString);
                        // Signal that we're done
                        done = true;
                    }
                }
            }
            catch (ArgumentNullException e)
            {
                throw (new BaliClientException("Invalid buffer passed to baliStream.read()", e));
            }
            catch (ArgumentOutOfRangeException e)
            {
                throw (new BaliClientException("Error with size of buffer passed to baliStream.Read()", e));
            }
            catch (System.IO.IOException e)
            {
                throw (new BaliClientException("Unable to read reply from socket", e));
            }
            catch (ObjectDisposedException e)
            {
                throw (new BaliClientException("No TcpClient - Run openServerConnection() to open connection to Bali Server", e));
            }

            // Return
            Console.WriteLine("<-RecvMsg:\n\t<{0}>", Regex.Replace(reply, @"\n", "\\n"));
            return reply;
        }
        /* Parse Bali Reply
            Input(s):
                - (string) reply    - Bali reply from getReply() method
            Output(s):
                - (string) msg      - Message included in Bali Reply
                                      NOTE: May be the empty string
            Exception(s):
                BaliBadReplyException   - Thrown on 'BAD' reply from Bali (see 'Bali Reply Description')
                BaliErrReplyException   - Thrown on 'ERR' reply from Bali (see 'Bali Reply Description')
                BaliClientException     - Thrown when unable to parse Bali reply
        */
        public string parseBaliReply(string reply)
        {
            // Initialize
            string replyType = String.Empty;
            string replyMsg = String.Empty;
            // Split Bali Reply by @
            string[] replyComponents = reply.Split('@');
            // Verify that we have 3 entries ([<replyType>,<replyMsg>,"\n"])
            if (replyComponents.Length != 3)
            {
                throw (new BaliClientException(String.Format("Error parsing Bali reply <{0}>", reply)));
            }

            // Capture replyType and replyMsg
            replyType = replyComponents[0];
            replyMsg = replyComponents[1];
            //Console.WriteLine(String.Format("ParseReply: <{0}> : <{1}>", replyType, replyMsg));

            // Check replyType for Bad or Err
            if (replyType.ToLower().Equals("bad"))
            {
                throw (new BaliBadReplyException(replyMsg));
            }
            else if (replyType.ToLower().Equals("err"))
            {
                throw (new BaliErrReplyException(replyMsg));
            }

            // If we get down to here, the Bali reply was 'ok',
            // so we can just return the replyMsg

            // Return
            return replyMsg;
        }

        /* Close Connection to Bali Server
            Input(s):
                none
            Output(s):
                none
            Exception(s):
                BaliClientException   - Thrown if baliStream or TCP socket are not open
        */
        public void closeServerConnection()
        {
            try
            {
                // Close NetworkStream
                baliStream.Close();
                // Close TcpClient
                client.Close();
             
            }
            catch (ObjectDisposedException e)
            {
                throw (new BaliClientException("ObjectDisposedException", e));
            }
        }


    }

    /* Custom Exception Class
    Desc:
        Thrown for any exception encountered in BaliClient methods
*/
    public class BaliClientException : Exception
    {
        public BaliClientException(string message)
            : base(message)
        {
        }
        public BaliClientException(Exception e)
            : base(e.ToString())
        {
        }
        public BaliClientException(string msg, Exception e)
            : base(String.Format("{0}\n{1}", msg, e))
        {
        }
    }

    /* Custom Exception Class for Bali Server 'Err' Reply
        Desc:
            Thrown when Bali server replies with 'err' message
            (see 'Bali Reply Description' comments)
    */
    public class BaliErrReplyException : BaliClientException
    {
        public BaliErrReplyException(string message)
            : base(message)
        {
        }
    }
    /* Custom Exception Class for Bali Server 'Bad' Reply
        Desc:
            Thrown when Bali server replies with 'bad' message
            (see 'Bali Reply Description' comments)
    */
    public class BaliBadReplyException : BaliClientException
    {
        public BaliBadReplyException(string message)
            : base(message)
        {
        }
    }







}
