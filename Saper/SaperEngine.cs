using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saper
{
    class SaperEngine
    {
        public List<List<int>> BombField { get; set; }
        public int boardX, boardY;
        Random rnd;


        public int CountBombs(int fieldY,int fieldX)
        {
            if (BombField[fieldY][fieldX] == 9)
                return 9;

            int count = 0;


            for (int j = -1; j < 2; j++)
                for (int i = -1; i < 2; i++)
                {
                    if ((fieldY + j) < 0 || (fieldX + i) < 0) continue;
                    if ((fieldY + j) >= BombField.Count() || (fieldX + i) >= BombField[0].Count()) continue;

                    if (BombField[fieldY + j][fieldX + i] == 9)
                        count++;
                }

            return count;
        }

        private void setAllFields()
        {
            for (int j = 0; j < BombField.Count(); j++)
                for (int i = 0; i < BombField[0].Count(); i++)
                    BombField[j][i] = CountBombs(j, i);
        }

        private void CreateBombs(int Bombs)
        {
            int x;
            int y;

            while (0 < Bombs)
            {
                y = rnd.Next(0, BombField.Count());
                x = rnd.Next(0, BombField[0].Count());

                if (BombField[y][x] != 9)
                {
                    BombField[y][x] = 9;
                    Bombs--;
                }
            }
        }

        public void SetBoard(int bX, int bY,int Bombs)
        {
            rnd = new Random();

            boardX = bX;
            boardY = bY;

            BombField = new List<List<int>>();

            for (int i = 0; i < boardY; i++)
            {
                BombField.Add(new List<int>());
                for (int j = 0; j < boardX; j++)
                    BombField[i].Add(0);
            }
            CreateBombs(Bombs);
            setAllFields();
        }      

    }
}
