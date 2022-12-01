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
    class BorderButton : Button
    {
        public int btnSize = 40;

        public BorderButton(string st)
        {
            this.Enabled = false;
            this.Size = new Size(btnSize, btnSize);
            this.Text = st;
            this.BackColor = Color.Gray;
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular,
                        System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        }
    }
}
