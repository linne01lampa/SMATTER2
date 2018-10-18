using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading.Tasks;
using System.IO;


namespace GridGame
{

    class Program
    {


        static void Main(string[] args)
        {



            Game myGame = new Game(15, 6);
            Level level = new Level();

            while (true)
            {
                myGame.UpdateBoard();
                myGame.DrawBoard();
            }

        }
    }

    class Game
    {
        int trys = 7;

        List<GameObject> GameObjects = new List<GameObject>();

        List<LevelBase> Levels = new List<LevelBase>();


        public Game(int xSize, int ySize)
        {

            for (int i = 0; i < ySize + 2; i++)
            {
                for (int j = 0; j < xSize + 2; j++)
                {
                    if (j == 0 || i == 0 || i == ySize + 1 || j == xSize + 1)
                    {
                        GameObjects.Add(new Wall(j, i));
                    }
                }
            }
            GameObjects.Add(new Player(1, 1));
            Levels.Add(new Level());
        }

        public void DrawBoard()
        {

            if (trys == 7)
            {
                //yumymg


                foreach (LevelBase i in Levels)
                {
                    Console.ForegroundColor = ConsoleColor.White;
                    i.Start(0, 0);
                }
                trys = 0;
            }
            foreach (GameObject gameObject in GameObjects)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                gameObject.Draw(5, 3);
                Console.ForegroundColor = ConsoleColor.White;
            }

            trys++;

        }

        public void UpdateBoard()
        {
            foreach (GameObject gameObject in GameObjects)
            {
                gameObject.Update();
            }

        }
    }

    abstract class GameObject
    {
        public int XPosition;
        public int YPosition;
        public abstract void Draw(int xBoxSize, int yBoxSize);
        public abstract void Update();
    }

    class Wall : GameObject
    {
        public Wall(int xPosition, int yPosition)
        {
            XPosition = xPosition;
            YPosition = yPosition;
        }

        public override void Draw(int xBoxSize, int yBoxSize)
        {
            int startX = XPosition * xBoxSize;
            int startY = YPosition * yBoxSize;
            //Console.SetCursorPosition(startX, startY);
            //Console.Write("█");
            //Console.SetCursorPosition(startX, startY + 1);
            //Console.Write(" ██ ");
            //Console.SetCursorPosition(startX, startY + 2);
            //Console.Write("█  █");
        }

        public override void Update()
        {

        }
    }

    class Player : GameObject
    {
        int lastX;
        int lastY;

        public Player(int xPos, int yPos)
        {
            XPosition = xPos;
            YPosition = yPos;
        }

        public override void Draw(int xBoxSize, int yBoxSize)
        {
            int curX = XPosition;
            int curY = YPosition;
            Console.SetCursorPosition(curX, curY);
            Console.Write("█");

            lastY = curY;
            lastX = curX;
        }

        public override void Update()
        {
            ConsoleKeyInfo keyInfo = Console.ReadKey(true);
            Console.CursorVisible = false;

            Erase();

            if (keyInfo.Key == ConsoleKey.W)
            {
                YPosition--;
            }
            else if (keyInfo.Key == ConsoleKey.S)
            {
                YPosition++;
            }
            else if (keyInfo.Key == ConsoleKey.D)
            {
                XPosition++;
            }
            else if (keyInfo.Key == ConsoleKey.A)
            {
                XPosition--;
            }
        }

        public void Erase()
        {
            Console.SetCursorPosition(lastX, lastY);
            Console.Write(" ");
        }
    }

    abstract class LevelBase
    {
        public string fileText;

        public abstract void Start(int x, int y);
    }

    class Level : LevelBase
    {
        string ranLevel = File.ReadAllText(Path.Combine(Environment.CurrentDirectory, "LevelOne.txt"));
        char[] lines;

        Random rnd;

        public Level()
        {

        }



        public override void Start(int x, int y)
        {
            int curY = x;
            int curX = y;
            int rndNum = 0;
            int offset = 1;

            bool write = false;
            bool vertical = false;

            rnd = new Random();

            List<Block> blocks = new List<Block>();

            lines = ranLevel.ToCharArray(0, ranLevel.Length);

            for (int i = 0; i < lines.Length; i++)
            {
                rndNum = rnd.Next(2);

                char curChar = lines[i];

                if (curChar.ToString() == "1")
                {
                    write = true;
                    curX += offset;
                }
                if (curChar.ToString() == "0")
                {
                    write = false;
                    vertical = false;
                    curX += offset;
                }
                if (curChar.ToString() == "2")
                {
                    write = false;
                    vertical = false;
                    curX = x;
                    curY += offset;
                }
                if (curChar.ToString() == "3")
                {
                    if (rndNum == 1)
                    {
                        write = true;
                    }
                    else
                    {
                        write = false;
                    }

                    vertical = true;
                    curX += offset;
                }

                blocks.Add(new Block(curX, curY, write, vertical));
                blocks[i].Draw(curX, curY);
            }
        }
    }

    class Block : GameObject
    {
        int xPosition;
        int yPosition;

        bool write;

        bool vert;

        public Block(int xPos, int yPos, bool ifWrite, bool vertical)
        {
            xPosition = xPos;
            yPosition = yPos;
            write = ifWrite;
            vert = vertical;
        }


        public override void Update()
        {

        }

        public override void Draw(int x, int y)
        {

            if (write)
            {
                if (vert)
                {
                    Console.SetCursorPosition(x, y);
                    Console.Write("█");
                    //Console.SetCursorPosition(x, y + 1);
                    //Console.Write("██");

                }
                else if (!vert)
                {
                    Console.SetCursorPosition(x, y);
                    Console.Write("█");
                }
            }
            else
            {
                Console.Write(" ");
            }
        }
    }

}

