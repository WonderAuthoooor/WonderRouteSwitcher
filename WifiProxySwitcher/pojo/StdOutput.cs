using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WifiProxySwitcher.pojo
{
    public static class StdOutput
    {

        private  const int msgLength = 400;

        private static TextBox txtDebugOutput;


        public static void Init(TextBox output)
        {
            txtDebugOutput = output;
        }

        public static void debugOutput(string msg)
        {
            string curText = DateTime.Now.ToString() + ":" + msg;
            string preText = txtDebugOutput.Text;
            string resultText = curText + "\r\n" + preText;
            int textLength = resultText.Length > msgLength ? msgLength : resultText.Length;
            string output = resultText.Substring(0, textLength);
            txtDebugOutput.Text = output;
        }
    }
}
