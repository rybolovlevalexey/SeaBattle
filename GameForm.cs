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
    class GameForm : Form
    {
        public const int mapSize = 10;
        public int btnSize = 40;
        public string[] letters = {" ", "А", "Б", "В", "Г", "Д", "Е", "Ж", "З", "И", "К" };
        FieldButton[,] person_map = new FieldButton[mapSize, mapSize]; // поле игрока
        FieldButton[,] bot_map = new FieldButton[mapSize, mapSize];  // поле бота
        bool is_game_started = false;
        OpponentBot bot = new OpponentBot(); // инициализация бота, играющего против пользователя

        public GameForm()
        {
            this.Width = (mapSize + 1) * 2 * btnSize + 70;
            this.Height = (mapSize + 3) * btnSize;
            this.Text = "Морской бой";
            InitializeComponent();
        }

        // метод дающий начало игре - первый ход делает пользователь
        private void person_ready_pushed(object sender, EventArgs e)
        {
            Label inf_lbl = new Label();
            inf_lbl.Location = new Point(300, btnSize * (mapSize + 1));
            inf_lbl.Text = "Проверка";
            inf_lbl.ForeColor = Color.ForestGreen;
            inf_lbl.Size = new Size(btnSize * 100, btnSize);
            inf_lbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular,
                        System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Controls.Add(inf_lbl);

            string is_correct = check_person_ships_toCorrect();  // вызов функции проверки корректности расстановки кораблей пользователем
            MessageBox.Show(is_correct);
            if (is_correct == "OK"){
                is_game_started = true;
                int[,] bot_ships = bot.give_ships_coords();
                for (int i = 0; i < 10; i += 1)
                {
                    int x = bot_ships[i, 0], y = bot_ships[i, 1];
                    bot_map[x, y].change_ship();
                }
                inf_lbl.Location = new Point(500, btnSize * (mapSize + 1));
                inf_lbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 26F, System.Drawing.FontStyle.Regular,
                        System.Drawing.GraphicsUnit.Point, ((byte)(204)));
                inf_lbl.Text = "Удачной игры";
                inf_lbl.ForeColor = Color.ForestGreen;
            } else
            {
                inf_lbl.Text = is_correct;
                inf_lbl.ForeColor = Color.Red;
            }
            
        }
        private void InitializeComponent()
        {
            // создание поля пользователя
            for (int i = 0; i <= mapSize; i += 1)
            {
                BorderButton btn = new BorderButton(letters[i]);
                btn.Location = new Point(btnSize * i, 0);
                this.Controls.Add(btn);
            }
            for (int i = 0; i < mapSize; i += 1)
            {
                for (int j = 0; j <= mapSize; j += 1)
                {
                    if (j == 0)
                    {
                        BorderButton btn = new BorderButton((i + 1).ToString());
                        btn.Location = new Point(btnSize * j, btnSize * (i + 1));
                        this.Controls.Add(btn);
                    } else
                    {
                        FieldButton btn = new FieldButton("P");
                        btn.Location = new Point(btnSize * j, btnSize * (i + 1));
                        person_map[i, j - 1] = btn;
                        btn.Click += new EventHandler(person_btn_pushed);
                        btn.Name = $"{i};{j - 1}";
                        this.Controls.Add(btn);
                    }
                }
            }

            // поле бота-соперника
            for (int i = 0; i <= mapSize; i += 1)
            {
                BorderButton btn = new BorderButton(letters[i]);
                btn.Location = new Point(btnSize * i + btnSize * (2 + mapSize) + 15, 0);
                this.Controls.Add(btn);
            }
            for (int i = 0; i < mapSize; i += 1)
            {
                for (int j = 0; j <= mapSize; j += 1)
                {
                    if (j == 0)
                    {
                        BorderButton btn = new BorderButton((i + 1).ToString());
                        btn.Location = new Point(btnSize * j + btnSize * (2 + mapSize) + 15, btnSize * (i + 1));
                        this.Controls.Add(btn);
                    }
                    else
                    {
                        FieldButton btn = new FieldButton("B");
                        btn.Location = new Point(btnSize * j + btnSize * (2 + mapSize) + 15, btnSize * (i + 1));
                        this.Controls.Add(btn);
                        bot_map[i, j - 1] = btn;
                        btn.Name = $"{i};{j - 1}";
                        btn.Click += new EventHandler(bot_btn_pushed);
                    }
                }
            }

            // кнопка дающая начало игре - пользователь расставил свои корабли
            Button ready_btn = new Button();
            ready_btn.Location = new Point(175, btnSize * (mapSize + 1));
            ready_btn.Size = new Size(btnSize * 3, btnSize);
            ready_btn.Text = "Готов";
            ready_btn.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular,
                        System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            ready_btn.Click += new EventHandler(person_ready_pushed);
            this.Controls.Add(ready_btn);
        }

        // пользователь стреляет по полю противника
        private void bot_btn_pushed(object sender, EventArgs e)
        {
            FieldButton push_btn = sender as FieldButton;

            if (is_game_started)
            {
                int x = Convert.ToInt32(push_btn.Name.ToString().Split(";")[0]);
                int y = Convert.ToInt32(push_btn.Name.ToString().Split(";")[1]);
                bool result = bot_map[x, y].shoot_from_person();

                // выстрел бота в ответ
                int num = bot.shoot_by_bot();
                x = num / mapSize; y = num - x * mapSize;
                bool res_bot_fire = person_map[x, y].shoot_from_bot();
                if (res_bot_fire)
                {
                    bot.append_to_stack(x, y);
                }
                load_maps_in_file(); // загрузка карты в файл
                // проверка на победу
                string win_checker = this.check_winner();
                if (win_checker == "person")
                {
                    for (int i = 0; i < mapSize; i += 1)
                    {
                        for (int j = 0; j < mapSize; j += 1)
                        {
                            person_map[i, j].BackColor = Color.Green;
                            bot_map[i, j].BackColor = Color.Coral;
                        }
                    }
                    MessageBox.Show("Победа");
                }
                if (win_checker == "bot")
                {
                    for (int i = 0; i < mapSize; i += 1)
                    {
                        for (int j = 0; j < mapSize; j += 1)
                        {
                            person_map[i, j].BackColor = Color.Coral;
                            bot_map[i, j].BackColor = Color.Green;
                        }
                    }
                    MessageBox.Show("Поражение");
                }
            }
        }

        // обработчик нажатия по кнопке пользователя, пока не начата игра - установка корабля
        private void person_btn_pushed(object sender, EventArgs e)
        {
            FieldButton push_btn = sender as FieldButton;

            if (!is_game_started)
            {
                int x = Convert.ToInt32(push_btn.Name.ToString().Split(";")[0]);
                int y = Convert.ToInt32(push_btn.Name.ToString().Split(";")[1]);
                person_map[x, y].change_ship();
            }
        }

        // проверка на победу одного из участников
        public string check_winner()
        {
            // проверка пользователя на победу
            int cnt1 = 0, cnt2 = 0;
            for (int i = 0; i < mapSize; i += 1)
            {
                for (int j = 0; j < mapSize; j += 1)
                {
                    if (bot_map[i, j].BackColor == Color.Orange)
                    {
                        cnt1 += 1;
                    }
                }
            }
            // проверка на победу бота
            for (int i = 0; i < mapSize; i += 1)
            {
                for (int j = 0; j < mapSize; j += 1)
                {
                    if (person_map[i, j].BackColor == Color.Orange)
                    {
                        cnt2 += 1;
                    }
                }
            }
            if (cnt1 == 10)
            {
                return "person";
            } else if (cnt2 == 10)
            {
                return "bot";
            }
            return "draw now";
        }

        // проверка на корректность расстановки пользователя
        public string check_person_ships_toCorrect()
        {
            
            // проверка трёхпалубных кораблей
            //.....
            // все проверки пройдены
            return "OK";
        }

        // функция загрузки полей в файл
        public void load_maps_in_file()
        {
            try
            {
                using (FileStream stream = new FileStream(@"C:\Visual Studio Projects\Морской бой 2.0\HistoryFile.txt", FileMode.Create))
                {
                    for (int i = 0; i < mapSize; i += 1)
                    {
                        for (int j = 0; j < mapSize; j += 1)
                        {
                            // is_ship;color;is_person;btn_text
                            string text = "";
                            FieldButton btn = person_map[i, j];
                            if (btn.is_ship)
                                text += "true;";
                            else
                                text += "false;";
                            text += (Convert.ToString(btn.BackColor) + ";");
                            if (btn.is_person)
                                text += "true;";
                            else
                                text += "false;";
                            text += btn.Text;
                            text += "\n";

                            byte[] array = System.Text.Encoding.Default.GetBytes(text);
                            stream.Write(array);
                        }
                    }
                    for (int i = 0; i < mapSize; i += 1)
                    {
                        for (int j = 0; j < mapSize; j += 1)
                        {
                            // is_ship;color;is_person;btn_text
                            string text1 = "";
                            FieldButton btn = bot_map[i, j];
                            if (btn.is_ship)
                                text1 += "true;";
                            else
                                text1 += "false;";
                            text1 += (Convert.ToString(btn.BackColor) + ";");
                            if (btn.is_person)
                                text1 += "true;";
                            else
                                text1 += "false;";
                            text1 += btn.Text;
                            text1 += "\n";

                            byte[] array = System.Text.Encoding.Default.GetBytes(text1);
                            stream.Write(array);
                        }
                    }
                }
            } catch(FileNotFoundException ex)
            {
                MessageBox.Show("Файл не найден");
            }
        }
    }
}
