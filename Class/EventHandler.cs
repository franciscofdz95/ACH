using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Nmc.Ach.Dal;

namespace AchSystem
{
    public class EventHandler
    {
        public static void Delete_KeyDown(object sender, KeyEventArgs e)
        {
            ComboBox cbo = (ComboBox)sender;
            if (e.KeyCode == Keys.Delete)
                cbo.SelectedIndex = -1;
        }

        public static void NumericOnly_KeyPress(object sender, KeyPressEventArgs e)
        {
            //Ctrl + C
            if ((Control.ModifierKeys & Keys.Control) == Keys.Control && (int) e.KeyChar == 3 ) 
            {
                return;
            }

            //Ctrl + X
            if ((Control.ModifierKeys & Keys.Control) == Keys.Control && (int) e.KeyChar == 24)
            {
                return;
            }

            //Ctrl + V
            if ((Control.ModifierKeys & Keys.Control) == Keys.Control && (int)e.KeyChar == 22)
            {
                if (!DataLayer.IsNumeric(Clipboard.GetData(DataFormats.Text).ToString()))
                    e.Handled = true;

                return;
            }

            switch ((int)e.KeyChar)
            {
                case 8:
                case 48:
                case 49:
                case 50:
                case 51:
                case 52:
                case 53:
                case 54:
                case 55:
                case 56:
                case 57:
                    break;
                default:
                    e.Handled = true;
                    break;
            }
        }



        public static void CurrencyOnly_KeyPress(object sender, KeyPressEventArgs e)
        {
            //Ctrl + C
            if ((Control.ModifierKeys & Keys.Control) == Keys.Control && (int)e.KeyChar == 3)
            {
                return;
            }

            //Ctrl + X
            if ((Control.ModifierKeys & Keys.Control) == Keys.Control && (int)e.KeyChar == 24)
            {
                return;
            }


            //Ctrl + V
            if ((Control.ModifierKeys & Keys.Control) == Keys.Control && (int)e.KeyChar == 22)
            {
                if (!DataLayer.IsNumeric(Clipboard.GetData(DataFormats.Text).ToString()))
                    e.Handled = true;

                return;
            }

            switch ((int)e.KeyChar)
            {
                case 8:  //backspace 
                case 45: //minus
                case 46: //perioud
                case 48:
                case 49:
                case 50:
                case 51:
                case 52:
                case 53:
                case 54:
                case 55:
                case 56:
                case 57:
                    break;
                default:
                    e.Handled = true;
                    break;
            }
        }
    }
}
