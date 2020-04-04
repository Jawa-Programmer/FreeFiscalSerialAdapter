using System;
using System.Drawing;
using System.IO.Ports;
using System.Media;
using System.Threading;
using System.Windows.Forms;

namespace FreeFiscal
{
    public partial class Form1 : Form
    {
        const int ANS_BTS = 0x01, ANS_STR = 0x02;
        byte CMD_ABOUT = 0x01, CMD_REQUEST = 0x02, CMD_AUTORIZE = 0x03;

        byte[] buffer = null;
        byte sended_command;

        public Form1()
        {
            InitializeComponent();
            PortListBox.Items.AddRange(SerialPort.GetPortNames());
        }

        private void button2_Click(object sender, System.EventArgs e)
        {
            PortListBox.Items.Clear();
            PortListBox.Items.AddRange(SerialPort.GetPortNames());
        }
        SerialPort ser;
        bool Autorized = false;

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (ser != null)
            {
                ser.Close();
                ser.Dispose();
                ser = null;
            }
        }

        public void dataRecived(object sender, SerialDataReceivedEventArgs e)
        {

            if (!Autorized)
            {
                byte[] data = new byte[ser.BytesToRead];
                ser.Read(data, 0, ser.BytesToRead);
                if (data.Length == 4 && data[0] == 0xff && data[1] == 0x00 && data[2] == 0xf0 && data[3] == 0x0f)
                {
                    Autorized = true;
                    this.BeginInvoke((Action)(() =>
                    {
                        TextOut.AppendText("[" + DateTime.Now.ToLongTimeString() + "]: ");
                        TextOut.AppendText("Соеденинение установлено\n");
                        ser.Write(new byte[] { CMD_ABOUT }, 0, 1);
                        Thread.Sleep(1000);
                        sendComand(0x33);
                    }));
                }
                else
                {
                    this.BeginInvoke((Action)(() =>
                    {
                        TextOut.AppendText("[" + DateTime.Now.ToLongTimeString() + "]: ");
                        TextOut.AppendText("Соеденинение установить не удалось\n");
                        TextOut.ScrollToCaret();
                    }));
                }
            }
            else
            {
                int code = ser.ReadByte();
                switch (code)
                {
                    case ANS_STR:
                        string text = ser.ReadLine();
                        this.BeginInvoke((Action)(() =>
                        {
                            TextOut.AppendText("[" + DateTime.Now.ToLongTimeString() + "] ФН: ");
                            TextOut.AppendText(text);
                            TextOut.ScrollToCaret();
                        }));
                        break;
                    case ANS_BTS:
                        while (ser.BytesToRead == 0) ;
                        int len = ser.ReadByte();
                        buffer = new byte[len];
                        for (int i = 0; i < len; i++)
                        {
                            while (ser.BytesToRead == 0) ;
                            buffer[i] = (byte)ser.ReadByte();
                        }
                        this.BeginInvoke((Action)(() =>
                        {
                            TextOut.AppendText("[" + DateTime.Now.ToLongTimeString() + "]: длинна полученого сообщения: " + len.ToString() + "\n");
                            TextOut.AppendText("".PadLeft(120, '-') + "\n");
                        }));
                        handleBufferData();
                        break;
                }
            }

        }
        string incorrectMessege(bool inc = true)
        {
            string txt = "";
            if (inc) txt += "Неверный формат ответа. Полный код ответа:\n";
            else txt += "Данный код ответа еще не добавлен в функционал FreeFiscal. Полный код ответа:\n";
            txt += "\n[" + DateTime.Now.ToLongTimeString() + "]:";
            for (int i = 0; i < buffer.Length; i++) txt += "0x" + buffer[i].ToString("X2") + " ";
            txt += "\n";
            return txt;
        }
        string btToString(byte[] bf, int start, int len)
        {
            string ret = "";
            for (int i = 0; i < len; i++)
                if (buffer[i + start] > 0x20 && buffer[i + start] < 0x7F)
                    ret += (char)bf[i + start];
                else ret += '_';
            return ret;
        }
        void handleBufferData()
        {

            if (buffer != null && buffer.Length > 5 && buffer[3] == 0)
            {
                string txt = "";
                switch (sended_command)
                {
                    case 0x33:
                        if (buffer.Length != 23)
                        {
                            txt = incorrectMessege();
                        }
                        else
                        {
                            txt = "Версия ФН: ";
                            txt += btToString(buffer, 4, 16);
                            if (buffer[20] == 0) txt += "\n[" + DateTime.Now.ToLongTimeString() + "]: " + "Это версия для отладки\n";
                            else txt += "\n[" + DateTime.Now.ToLongTimeString() + "]: " + "Это серийная модель\n";
                        }
                        break;

                    case 0x30:
                        if (buffer.Length != 36)
                            txt = incorrectMessege();
                        else
                        {
                            txt = "Состояние фазы жизни: " + Convert.ToString(buffer[4], 2).PadLeft(4, '0');
                            txt += "\n[" + DateTime.Now.ToLongTimeString() + "]: Текущий документ: 0x" + buffer[5].ToString("X2");
                            txt += "\n[" + DateTime.Now.ToLongTimeString() + "]: Данные документа: " + buffer[6];
                            txt += "\n[" + DateTime.Now.ToLongTimeString() + "]: Состояние смены: " + Convert.ToString(buffer[7], 2).PadLeft(4, '0');
                            txt += "\n[" + DateTime.Now.ToLongTimeString() + "]: Флаги предупреждения: " + Convert.ToString(buffer[8], 2).PadLeft(4, '0'); ;
                            txt += "\n[" + DateTime.Now.ToLongTimeString() + "]: Время последнего документа: ";
                            txt += buffer[11] + "/" + buffer[10] + "/" + buffer[9] + " " + buffer[12] + ":" + buffer[13];
                            txt += "\n[" + DateTime.Now.ToLongTimeString() + "]: Номер ФН: " + btToString(buffer, 14, 16);
                            txt += "\n[" + DateTime.Now.ToLongTimeString() + "]: Номер последнего ФД: 0x" + buffer[30].ToString("X2") + buffer[31].ToString("X2") + buffer[32].ToString("X2") + buffer[33].ToString("X2");
                            txt += "\n";
                        }
                        break;
                    case 0x31:
                        if (buffer.Length != 22)
                        {
                            txt = incorrectMessege();
                        }
                        else
                        {
                            txt += "Номер ФН: " + btToString(buffer, 4, 16);
                            txt += "\n";
                        }
                        break;
                    case 0x40:
                        txt = incorrectMessege();
                        break;
                    case 0x43:
                        if (buffer.Length != 69)
                        {
                            txt = incorrectMessege();
                        }
                        else
                        {
                            txt = "Дата: " + buffer[6] + "/" + buffer[5] + "/" + buffer[4] + " " + buffer[7] + ":" + buffer[8];
                            txt += "\n[" + DateTime.Now.ToLongTimeString() + "]: ИНН" + btToString(buffer, 9, 12);
                            txt += "\n[" + DateTime.Now.ToLongTimeString() + "]: Номер КТТ" + btToString(buffer, 21, 20);
                            txt += "\n[" + DateTime.Now.ToLongTimeString() + "]: Номер ФН" + btToString(buffer, 41, 16);
                            txt += "\n[" + DateTime.Now.ToLongTimeString() + "]: Код налогообложения" + buffer[57].ToString();
                            txt += "\n[" + DateTime.Now.ToLongTimeString() + "]: Режим работы" + buffer[58].ToString();
                            txt += "\n[" + DateTime.Now.ToLongTimeString() + "]: Номер ФД" + (((ulong)buffer[59] << 24) | ((ulong)buffer[60] << 16) | ((ulong)buffer[61] << 8) | ((ulong)buffer[62])).ToString();
                            txt += "\n[" + DateTime.Now.ToLongTimeString() + "]: Фискальный Признак" + (((ulong)buffer[63] << 24) | ((ulong)buffer[64] << 16) | ((ulong)buffer[65] << 8) | ((ulong)buffer[66])).ToString() + "\n";
                        }
                        break;
                    default: txt = incorrectMessege(); break;
                }
                txt += "[" + DateTime.Now.ToLongTimeString() + "]: " + "CRC16: 0x" + buffer[buffer.Length - 2].ToString("X2") + buffer[buffer.Length - 1].ToString("X2") + "\n";


                this.BeginInvoke((Action)(() =>
            {
                TextOut.AppendText("[" + DateTime.Now.ToLongTimeString() + "]: ");
                TextOut.AppendText(txt);
                TextOut.AppendText("".PadLeft(120, '-') + "\n");
                TextOut.ScrollToCaret();
            }));
            }
            else if (buffer != null && buffer[3] != 0)
            {
                this.BeginInvoke((Action)(() =>
                {
                    TextOut.ForeColor = Color.Red;
                    TextOut.AppendText("[" + DateTime.Now.ToLongTimeString() + "]: ");
                    TextOut.AppendText("Фискальный накопитель вернул следующий код ошибки: 0x" + buffer[3].ToString("X") + "\n");
                    TextOut.AppendText("[" + DateTime.Now.ToLongTimeString() + "]: ");
                    TextOut.AppendText("Полное сообщение: ");
                    for (int i = 0; i < buffer.Length; i++) TextOut.AppendText("0x" + buffer[i].ToString("X") + " ");
                    TextOut.AppendText("\n");
                    TextOut.ScrollToCaret();
                    TextOut.ForeColor = Color.Black;
                }));
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (Autorized)
                sendComand(0x30);
        }

        private void Send_Click(object sender, EventArgs e)
        {
            if (Autorized)
            {
                string cmd = ComandToSend.Text;
                this.BeginInvoke((Action)(() =>
                {
                    ComandToSend.Text = "";
                }));
                string[] cmds = cmd.Split(' ');
                if (cmds.Length < 1) return;
                byte comand = Convert.ToByte(cmds[0], 16);
                byte[] cmdsb = new byte[cmds.Length - 1];
                for (int i = 1; i < cmds.Length; i++)
                    cmdsb[i] = Convert.ToByte(cmds[i], 16);
                sendComand(comand,cmdsb);
            }
        }

        void sendComand(byte comand, byte[] args = null)
        {
            if (Autorized)
            {
                sended_command = comand;
                //ser.Write(CMD_REQUEST, 0, 1);
                ser.Write(new byte[] { CMD_REQUEST, comand }, 0, 2);
                if (args != null)
                {
                    ser.Write(new byte[] { (byte)args.Length }, 0, 1);
                    ser.Write(args, 0, args.Length);
                }
                else
                {
                    ser.Write(new byte[] { 0x00 }, 0, 1);
                }
            }
        }
        private void button1_Click(object sender, System.EventArgs e)
        {
            if (PortListBox.SelectedItems.Count == 0 || ser != null)
            {
                SystemSounds.Exclamation.Play();
            }
            else
            {
                ser = new SerialPort((string)PortListBox.SelectedItem, 19200);
                ser.DataReceived += new SerialDataReceivedEventHandler(dataRecived);
                ser.Open();
                ser.Write(new byte[] { CMD_AUTORIZE }, 0, 1);

            }
        }
        //909522736
        private void button3_Click(object sender, System.EventArgs e)
        {
            if (ser == null || !ser.IsOpen)
            {
                SystemSounds.Exclamation.Play();
                this.BeginInvoke((Action)(() =>
                {
                    TextOut.AppendText("[" + DateTime.Now.ToLongTimeString() + "]: ");
                    TextOut.AppendText("Соеденинение нельзя разорвать т.к. оно ещё не было установлено\n");
                    TextOut.ScrollToCaret();
                }));
            }
            else
            {
                Autorized = false;
                ser.Close();
                ser.Dispose();
                ser = null;
                this.BeginInvoke((Action)(() =>
                {
                    TextOut.AppendText("[" + DateTime.Now.ToLongTimeString() + "]: ");
                    TextOut.AppendText("Соеденинение разорвано.\n");
                    TextOut.ScrollToCaret();
                }));
            }
        }
    }
}
