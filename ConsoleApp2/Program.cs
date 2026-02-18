using System;
using System.Collections.Generic;
using System.Diagnostics;
using static System.Console;

namespace Snake
{
    class Program
    {
        static void Main()
        {
            WindowHeight = 16;
            WindowWidth = 32;

            var rand = new Random();
            const int InitialLength = 5;
            int score = InitialLength;

            var head = new Pixel(WindowWidth / 2, WindowHeight / 2, ConsoleColor.Red);
            var berry = new Pixel(rand.Next(1, WindowWidth - 2), rand.Next(1, WindowHeight - 2), ConsoleColor.Cyan);
            var body = new List<Pixel>();

            Direction currentMovement = Direction.Right;
            bool gameover = false;

            CursorVisible = false;

            while (!gameover)
            {
                Clear();
                DrawBorder();

                // 1. KONTROLA KOLIZE SE ZDÍ
                if (head.XPos == WindowWidth - 1 || head.XPos == 0 ||
                    head.YPos == WindowHeight - 1 || head.YPos == 0)
                {
                    gameover = true;
                }

                // 2. KONTROLA SEŽRÁNÍ BOBULE
                if (berry.XPos == head.XPos && berry.YPos == head.YPos)
                {
                    score++;
                    berry = new Pixel(rand.Next(1, WindowWidth - 2), rand.Next(1, WindowHeight - 2), ConsoleColor.Cyan);
                }

                // 3. VYKRESLENÍ TĚLA A KONTROLA KOLIZE SE SEBOU
                for (int i = 0; i < body.Count; i++)
                {
                    DrawPixel(body[i]);
                    if (body[i].XPos == head.XPos && body[i].YPos == head.YPos)
                    {
                        gameover = true;
                    }
                }

                DrawPixel(head);
                DrawPixel(berry);

                // 4. INPUT DELAY (RYCHLOST HRY)
                var sw = Stopwatch.StartNew();
                while (sw.ElapsedMilliseconds <= 150)
                {
                    currentMovement = ReadMovement(currentMovement);
                }

                // 5. LOGIKA POHYBU (Tohle v tvém kódu chybělo!)
                body.Add(new Pixel(head.XPos, head.YPos, ConsoleColor.Green));

                switch (currentMovement)
                {
                    case Direction.Up:    head.YPos--; break;
                    case Direction.Down:  head.YPos++; break;
                    case Direction.Left:  head.XPos--; break;
                    case Direction.Right: head.XPos++; break;
                }

                if (body.Count > score)
                {
                    body.RemoveAt(0);
                }
            }

            SetCursorPosition(WindowWidth / 5, WindowHeight / 2);
            ForegroundColor = ConsoleColor.White;
            WriteLine($"Game over, Score: {score - InitialLength}");
            SetCursorPosition(WindowWidth / 5, WindowHeight / 2 + 1);
            ReadKey();
        }

        static Direction ReadMovement(Direction movement)
        {
            if (KeyAvailable)
            {
                var key = ReadKey(true).Key;
                if (key == ConsoleKey.UpArrow && movement != Direction.Down)    movement = Direction.Up;
                else if (key == ConsoleKey.DownArrow && movement != Direction.Up)    movement = Direction.Down;
                else if (key == ConsoleKey.LeftArrow && movement != Direction.Right)  movement = Direction.Left;
                else if (key == ConsoleKey.RightArrow && movement != Direction.Left)  movement = Direction.Right;
            }
            return movement;
        }

        static void DrawPixel(Pixel pixel)
        {
            SetCursorPosition(pixel.XPos, pixel.YPos);
            ForegroundColor = pixel.ScreenColor;
            Write("■");
            SetCursorPosition(0, 0);
        }

        static void DrawBorder()
        {
            ForegroundColor = ConsoleColor.White;
            for (int i = 0; i < WindowWidth; i++)
            {
                SetCursorPosition(i, 0);
                Write("■");
                SetCursorPosition(i, WindowHeight - 1);
                Write("■");
            }
            for (int i = 0; i < WindowHeight; i++)
            {
                SetCursorPosition(0, i);
                Write("■");
                SetCursorPosition(WindowWidth - 1, i);
                Write("■");
            }
        }

        struct Pixel
        {
            public Pixel(int xPos, int yPos, ConsoleColor color)
            {
                XPos = xPos;
                YPos = yPos;
                ScreenColor = color;
            }
            public int XPos { get; set; }
            public int YPos { get; set; }
            public ConsoleColor ScreenColor { get; set; }
        }

        enum Direction { Up, Down, Right, Left }
    }
}