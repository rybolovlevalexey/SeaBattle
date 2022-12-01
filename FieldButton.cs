using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Морской_бой_2._0
{
    class FieldButton : Button
    {
        public int btnSize = 40;
        public bool is_ship = false;
        public bool is_person = true;
        public FieldButton(string st)  // создание кнопки при первом её упоминании в программе
        {
            bool is_ship = false;
            this.BackColor = Color.SteelBlue;
            this.Size = new Size(btnSize, btnSize);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular,
                        System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            if (st == "P")
                is_person = true;
            else
                is_person = false;
        }
        
        public void change_ship()
        {
            if (is_person)
            {
                if (is_ship)
                {
                    is_ship = false;
                    this.BackColor = Color.SteelBlue;
                }
                else
                {
                    is_ship = true;
                    this.BackColor = Color.Brown;
                }
            } else
            {
                is_ship = true;
            }
        }

        // true - попал, false - не попал
        public bool shoot_from_person()
        {
            if (!is_person)
            {
                if (is_ship)
                {
                    this.BackColor = Color.Orange;
                    this.Text = "X";
                    return true;
                } else
                {
                    this.BackColor = Color.DarkBlue;
                    return false;
                }
            }
            return false;
        }

        public bool shoot_from_bot()
        {
            if (is_person)
            {
                if (is_ship)
                {
                    this.BackColor = Color.Orange;
                    this.Text = "X";
                    return true;
                }
                else
                {
                    this.BackColor = Color.DarkBlue;
                    return false;
                }
            }
            return false;
        }
    }
}
