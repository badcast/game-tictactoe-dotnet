using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace AnimationEditor
{
    static class Program
    {
        public class MessageProc : IMessageFilter
        {
            public bool PreFilterMessage(ref Message m)
            {
                return WndProc(m.HWnd, m.Msg, m.WParam, m.LParam);
            }
        }
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Time.InitTime();

            Configs.ConfigInitialize();


            Application.EnableVisualStyles();
            Application.AddMessageFilter(new MessageProc());
            Application.SetCompatibleTextRenderingDefault(false);

            Main mainForm = new Main();

            System.Windows.Forms.Timer timer = new Timer()
            {
                Interval = 1,
            };

            timer.Tick += (o, e) =>
            {
                Time.Calculate();
            };
            timer.Start();



            Application.Run(mainForm);
            //Save all configs
            Configs.Save();
        }

        static bool WndProc(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam)
        {
            if(msg == 0x000F)
            {
                Time.CalculateFps();
            }

            return false;
        }

        
    }
}
