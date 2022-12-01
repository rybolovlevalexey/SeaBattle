using System;
using System.Collections.Generic;
using System.Text;

namespace Морской_бой_2._0
{
    class OpponentBot
    {
        public const int mapSize = 10;

        public int[,] ships_coords = new int[10, 2];
        public List<int> shoots_on_person = new List<int>();
        public List<int> stack = new List<int>();
        public OpponentBot()
        {
            Random rnd = new Random();
            // одинарный корабль
            int x = rnd.Next(8, 10), y = rnd.Next(8, 10);
            if (rnd.Next(1, 3) == 1)
            {
                x = rnd.Next(0, 2);
                y = rnd.Next(0, 2);
            }
            ships_coords[0, 0] = x; ships_coords[0, 1] = y;
            // двойной корабль
            x = rnd.Next(0, 7); y = rnd.Next(8, 10);
            if (rnd.Next(1, 3) == 1)
            {
                x = rnd.Next(2, 8);
                y = rnd.Next(0, 2);
            }
            ships_coords[1, 0] = x; ships_coords[1, 1] = y;
            x += 1;
            ships_coords[2, 0] = x; ships_coords[2, 1] = y;
            // тройной корабль
            x = rnd.Next(1, 4); y = rnd.Next(3, 7);
            if (rnd.Next(1, 3) == 1)  // горизонтальность/вертикальность
            {
                ships_coords[3, 0] = x; ships_coords[3, 1] = y;
                x += 1;
                ships_coords[4, 0] = x; ships_coords[4, 1] = y;
                x -= 2;
                ships_coords[5, 0] = x; ships_coords[5, 1] = y;
            } else
            {
                ships_coords[3, 0] = x; ships_coords[3, 1] = y;
                y += 1;
                ships_coords[4, 0] = x; ships_coords[4, 1] = y;
                y -= 2;
                ships_coords[5, 0] = x; ships_coords[5, 1] = y;
            }
            // корабль из четырёх клеток
            x = rnd.Next(6, 9); y = rnd.Next(4, 7);
            ships_coords[6, 0] = x; ships_coords[6, 1] = y;
            y -= 1;
            ships_coords[7, 0] = x; ships_coords[7, 1] = y;
            y -= 1;
            ships_coords[8, 0] = x; ships_coords[8, 1] = y;
            y += 3;
            ships_coords[9, 0] = x; ships_coords[9, 1] = y;
        }

        public int[,] give_ships_coords()
        {
            return ships_coords;
        }

        public void append_to_stack(int i, int j)
        {
            static bool check(List<int> sp, int x)  // true элемент есть, false элемента нет в массиве
            {
                foreach (var el in sp)
                {
                    if (el == x)
                        return true;
                }
                return false;
            }
            
            while (stack.Count > 0)
            {
                stack.RemoveAt(0);
            }
            for (int di = -1; di < 2; di += 1)
            {
                for (int dj = -1; dj < 2; dj += 1) 
                {
                    if (i + di < 0 || j + dj < 0 || i + di >= mapSize || j + dj >= mapSize)
                        continue;
                    if (di == 0 || dj == 0)
                    {
                        int fire_num = (j + dj) + mapSize * (i + di);
                        if (!check(shoots_on_person, fire_num) && !check(stack, fire_num))
                            stack.Add(fire_num);
                    }
                }
            }
        }

        public int shoot_by_bot()
        {
            static bool check(List<int> sp, int x)  // true элемент есть, false элемента нет в массиве
            {
                foreach (var el in sp)
                {
                    if (el == x)
                        return true;
                }
                return false;
            }
            Random rnd = new Random();
            int number = 0;
            if (stack.Count == 0)
            {
                number = rnd.Next(0, 99);
                while (check(shoots_on_person, number))
                {
                    number = rnd.Next(0, 99);
                }
            } else
            {
                number = stack[0];
                stack.Remove(number);
            }
            shoots_on_person.Add(number);
            return number;
        }
    }
}
