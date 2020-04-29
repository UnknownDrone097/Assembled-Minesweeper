using System;

namespace Minesweeper
{
    class Game
    {
        public bool[,] Mines { get; set; }
        public int[,] Board { get; set; }
        public bool[,] KnownBoard { get; set; }
        public bool[,] Flags { get; set; }
        public int[,] PublicBoard { get; set; }
        int Length { get; set; }
        int Height { get; set; }
        public Game(int h, int l)
        {
            Board = new int[h, l];
            Mines = new bool[h, l];
            KnownBoard = new bool[h, l];
            PublicBoard = new int[h, l];
            Flags = new bool[h, l];
            //0 is unknown
            for (int i = 0; i < h; i++) { for (int ii = 0; ii < l; ii++) { KnownBoard[i, ii] = false; Flags[i, ii] = false; } }
            Length = l;
            Height = h;
        }
        public Game Initialize(double difficulty)
        {
            if (difficulty > 1 || difficulty < 0) { throw new Exception("Difficulty must be between 0 and 1"); }
            var r = new Random();

            //Create bombs
            for (int i = 0; i < Height; i++)
            {
                for (int ii = 0; ii < Length; ii++)
                {
                    //Generates a new bomb with a difficulty% chance
                    //-1 is the bomb state
                    Mines[i, ii] = r.NextDouble() < difficulty ? true : false;
                }
            }
            //Ensure at least one bomb is present
            for (int i = 0; i < Height; i++)
            {
                for (int ii = 0; ii < Length; ii++)
                {
                    if (Mines[i, ii]) { break; }
                }
                if (i == Height) { Mines[r.Next(0, Height), r.Next(0, Length)] = true; }
            }
            //Count bombs
            Board = Count().Board;

            return this;
        }
        public Game Count()
        {
            //Foreach cell
            for (int i = 0; i < Height; i++)
            {
                for (int ii = 0; ii < Length; ii++)
                {
                    if (Mines[i, ii] == true) { Board[i, ii] = -1; }
                    //Foreach cell and its neighbors
                    for (int x = -1; x < 2; x++)
                    {
                        for (int y = -1; y < 2; y++)
                        {
                            if (i + x < 0 || i + x > Height - 1 || ii + y < 0 || ii + y > Length - 1) { continue; }
                            //Increment bomb count per each bomb found
                            if (Mines[i + x, ii + y] == true) { Board[i, ii]++; }
                        }
                    }
                }
            }
            return this;
        }
        public void Reveal(int x, int y)
        {
            //Can't click a flag
            if (Flags[x, y] == true) { return; }
            //Reveal all on bomb click
            if (Mines[x, y] == true)
            {
                for (int i = 0; i < Height; i++)
                { for (int ii = 0; ii < Length; ii++) { KnownBoard[i, ii] = true; } }
                return;
            }
            //Update known tile otherwise
            KnownBoard[x, y] = true;
            Flags[x, y] = false;
            Cascade(x, y);
        }
        public void Flag(int x, int y)
        {
            if (KnownBoard[x, y]) { return; }
            Flags[x, y] = !Flags[x, y];
            //else { throw new Exception("Flag must be planted on an unknown tile"); }
        }
        public bool Victory()
        {
            for (int i = 0; i < Height; i++)
            {
                for (int ii = 0; ii < Length; ii++)
                {
                    if (!KnownBoard[i, ii] && !Flags[i, ii]) { return false; }
                }
            }
            return true;
        }
        public Game Update()
        {
            for (int i = 0; i < Height; i++)
            {
                for (int ii = 0; ii < Length; ii++)
                {
                    if (KnownBoard[i, ii] == true) { PublicBoard[i, ii] = Board[i, ii]; }
                }
            }
            return this;
        }
        public void Cascade(int x, int y)
        {
            for (int xi = -1; xi < 2; xi++)
            {
                if (x + xi < 0 || x + xi > Height - 1) { continue; }
                if (KnownBoard[x + xi, y] == true) { continue; }
                //Cascade if the adjascent cell is also 0
                if (Mines[x, y] == true) { continue; }
                if (Board[x, y] == 0) { KnownBoard[x + xi, y] = true; }
                if (Board[x + xi, y] == 0) { Cascade(x + xi, y); }
            }
            for (int yi = -1; yi < 2; yi++)
            {
                if (y + yi < 0 || y + yi > Length - 1) { continue; }
                if (KnownBoard[x, y + yi] == true) { continue; }
                //Cascade if the adjascent cell is also 0
                if (Mines[x, y] == true) { continue; }
                if (Board[x, y] == 0) { KnownBoard[x, y + yi] = true; }
                if (Board[x, y + yi] == 0) { Cascade(x, y + yi); }
            }
        }
    }
}