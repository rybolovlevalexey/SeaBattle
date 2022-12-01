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
    public partial class Form1 : Form
    {
        public const int mapSize = 10;
        public int btnSize = 40;
        public string[] letters = { "А", "Б", "В", "Г", "Д", "Е", "Ж", "З", "И", "К" };

        public Form1()
        {
            this.Width = (mapSize + 1) * 2 * btnSize + 70;
            this.Height = (mapSize + 3) * btnSize;
            this.Text = "Морской бой";
            InitializeComponent();
            StartWindow();
        }

        public void StartWindow()
        {
            Button btn_new_start = new Button();
            btn_new_start.Location = new Point(this.Width / 5 * 2, this.Height / 5 - 10);
            btn_new_start.Size = new Size(this.Width / 5, this.Height / 5);
            btn_new_start.Text = "Начать новую\nигру";
            btn_new_start.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular,
                        System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            btn_new_start.Click += new EventHandler(StartNewGamePushed);
            this.Controls.Add(btn_new_start);

            Button btn_restart = new Button();
            btn_restart.Location = new Point(this.Width / 5 * 2, this.Height / 5 * 3 - 10);
            btn_restart.Size = new Size(this.Width / 5, this.Height / 5);
            btn_restart.Text = "Продолжить игру";
            btn_restart.Enabled = false;
            btn_restart.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular,
                        System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Controls.Add(btn_restart);
            
        }

        public void StartNewGamePushed(object sender, EventArgs e)
        {
            GameForm game_form = new GameForm();
            game_form.Show();
        }
    }
}
