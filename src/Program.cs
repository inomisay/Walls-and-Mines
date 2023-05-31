using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Project_3_Version_7
{
    internal class Program
    {
        struct Enemies
        {
            public int X;
            public int Y;
            public int Direction;
        }
        static void Main(string[] args)
        {
            // Necessary variables
            Random rand = new Random();
            Enemies[] enemyX = new Enemies[1000];
            Enemies[] enemyY = new Enemies[1000];
            int ind = 0;
            int ind2 = 0;
            //Enemies enemyX, enemyY;

            // walls
            bool WallsAreCorrect = false;
            char[,] wallCombination = new char[4, 4];
            int[] existanceOfWalls = { 0, 0, 0, 0 };  //Upperwall = array[0]; LeftsideWall = array[1]; LowerWall = array[2]; RightsideWall = array[3]
            int[,,] everyWallInGameField = new int[4, 10, 4];
            int emptyWallCounter = 0;
            Console.CursorVisible = false;

            // Status
            bool alive = true;
            double time = 0;

            int playerEnergy = 200; // Energy of P is 200 at the beginning.
            int score = 0;

            int mine = 150;
            int minePosition = 0; // Counter for the position of the mines

            // Keys
            ConsoleKeyInfo cki;               // required for readkey

            /*---------------------------------------------------------------------------*/
            Console.ForegroundColor = System.ConsoleColor.DarkGreen;
            Console.WriteLine("\n         ⍙ Welcome To The Walls and Mines Game ⍙");
            Console.ResetColor();

            // Game Field
            char[,] gameField = new char[23, 53];

            GenerateWall(); // Generate the wall
            Status(); // Showing the status
            /*---------------------------------------------------------------------------*/
            // Controling the availability of the player and the enemies
            int cursorx = rand.Next(4, 56), cursory = rand.Next(4, 26);   // position of cursor - Human player P is located randomly.

            // Availability of P
            do
            {
                // position of cursor - Human player P is located randomly.
                cursorx = rand.Next(4, 56);
                cursory = rand.Next(4, 26);
            }
            while (gameField[cursory - 3, cursorx - 3] == '#' || gameField[cursory - 3, cursorx - 3] == 'X' || gameField[cursory - 3, cursorx - 3] == 'Y');


            /*---------------------------------------------------------------------------*/
            // Calling the Functions 
            Game();

            Console.ReadKey();
            /*****************************************************************************/
            // Functions

            // Generate wall of the game
            void GenerateWall()
            {
                Console.SetCursorPosition(3, 3);
                // Assigning borders to the gameField array
                for (int i = 0; i < gameField.GetLength(0); i++)
                {
                    for (int j = 0; j < gameField.GetLength(1); j++)
                    {
                        if (i == 0 || i == 22 || j == 0 || j == 52)
                            gameField[i, j] = '#';
                        else
                            gameField[i, j] = ' ';
                    }
                }
                for (int i = 0; i < 4; i++)
                {
                    for (int j = 0; j < 10; j++)
                    {
                        while (WallsAreCorrect == false)
                        {
                            aBlockOfWall();
                            if (emptyWallCounter != 0 && emptyWallCounter != 4)
                            {
                                WallsAreCorrect = true;
                            }
                        }
                        WallsAreCorrect = false;
                        //Filling in the 3D array;
                        for (int s = 0; s < 4; s++)
                        {
                            everyWallInGameField[i, j, s] = existanceOfWalls[s];
                        }
                        ////
                        for (int t = 0; t < wallCombination.GetLength(0); t++)
                        {
                            for (int k = 0; k < wallCombination.GetLength(1); k++)
                            {
                                gameField[2 + i * 5 + t, 2 + j * 5 + k] = wallCombination[t, k];
                            }
                        }
                    }
                }
                Console.SetCursorPosition(3, 3);
                for (int i = 0; i < gameField.GetLength(0); i++)
                {
                    for (int j = 0; j < gameField.GetLength(1); j++)
                    {
                        Console.Write(gameField[i, j]);
                    }
                    Console.WriteLine();
                    // Position of the inner walls inside the outer walls
                    Console.SetCursorPosition(3, 3 + i + 1);
                }
            }

            // Block of wall
            void aBlockOfWall()
            {
                int UpperWall = rand.Next(0, 2);
                int LeftsideWall = rand.Next(0, 2);
                int LowerWall = rand.Next(0, 2);
                int RightsideWall = rand.Next(0, 2);
                emptyWallCounter = 0;
                for (int i = 0; i < wallCombination.GetLength(0); i++)
                {
                    for (int j = 0; j < wallCombination.GetLength(1); j++)
                    {
                        wallCombination[i, j] = ' ';
                    }
                }
                for (int i = 0; i < existanceOfWalls.Length; i++)
                {
                    existanceOfWalls[i] = 0;
                }
                if (UpperWall == 1)
                {
                    wallCombination[0, 0] = '#';
                    wallCombination[0, 1] = '#';
                    wallCombination[0, 2] = '#';
                    wallCombination[0, 3] = '#';
                    existanceOfWalls[0] = 1;
                }
                if (LowerWall == 1)
                {
                    wallCombination[3, 0] = '#';
                    wallCombination[3, 1] = '#';
                    wallCombination[3, 2] = '#';
                    wallCombination[3, 3] = '#';
                    existanceOfWalls[2] = 2;
                }
                if (RightsideWall == 1)
                {
                    wallCombination[0, 0] = '#';
                    wallCombination[1, 0] = '#';
                    wallCombination[2, 0] = '#';
                    wallCombination[3, 0] = '#';
                    existanceOfWalls[1] = 3;
                }
                if (LeftsideWall == 1)
                {
                    wallCombination[0, 3] = '#';
                    wallCombination[1, 3] = '#';
                    wallCombination[2, 3] = '#';
                    wallCombination[3, 3] = '#';
                    existanceOfWalls[3] = 4;
                }
                for (int k = 0; k < existanceOfWalls.Length; k++)
                {
                    if (existanceOfWalls[k] == 0)
                        emptyWallCounter++;
                }
            }

            // Change of an inner wall
            void ChangeWall()
            {

                /////////Change of an inner wall.///////////
                int emptyWallCounter1 = 0;
                //Out of 40 wall blocks, a random one is chosen.
                int randomI = rand.Next(0, 4);
                int randomJ = rand.Next(0, 10);
                //Counting the empty walls in a randomly chosen block of walls:
                for (int i = 0; i < 4; i++)
                {
                    if (everyWallInGameField[randomI, randomJ, i] == 0)
                    {
                        emptyWallCounter1++;
                    }
                }

                //Only on wall in a block wall:
                if (emptyWallCounter1 == 3) //If there are three empty walls in the chosen wall block, the program has to add one wall.
                {
                    int randomEmptyWall = rand.Next(0, 4);
                    bool flag = false;
                    while (flag == false)
                    {
                        randomEmptyWall = rand.Next(0, 4);
                        if (existanceOfWalls[randomEmptyWall] == 0) //A random empty wall is chosen.
                            flag = true;
                    }

                    //Upperwall = array[0]; LeftsideWall = array[1]; LowerWall = array[2]; RightsideWall = array[3]
                    if (randomEmptyWall % 4 == 0) //Uppper
                    {
                        gameField[2 + randomI * 5 + 0, 2 + randomJ * 5 + 0] = '#';
                        gameField[2 + randomI * 5 + 0, 2 + randomJ * 5 + 1] = '#';
                        gameField[2 + randomI * 5 + 0, 2 + randomJ * 5 + 2] = '#';
                        gameField[2 + randomI * 5 + 0, 2 + randomJ * 5 + 3] = '#';
                        everyWallInGameField[randomI, randomJ, randomEmptyWall] = 1;
                    }
                    else if (randomEmptyWall % 4 == 1) // Left
                    {
                        gameField[2 + randomI * 5 + 0, 2 + randomJ * 5 + 3] = '#';
                        gameField[2 + randomI * 5 + 1, 2 + randomJ * 5 + 3] = '#';
                        gameField[2 + randomI * 5 + 2, 2 + randomJ * 5 + 3] = '#';
                        gameField[2 + randomI * 5 + 3, 2 + randomJ * 5 + 3] = '#';
                        everyWallInGameField[randomI, randomJ, randomEmptyWall] = 3;
                    }
                    else if (randomEmptyWall % 4 == 2) // Lower
                    {
                        gameField[2 + randomI * 5 + 3, 2 + randomJ * 5 + 0] = '#';
                        gameField[2 + randomI * 5 + 3, 2 + randomJ * 5 + 1] = '#';
                        gameField[2 + randomI * 5 + 3, 2 + randomJ * 5 + 2] = '#';
                        gameField[2 + randomI * 5 + 3, 2 + randomJ * 5 + 3] = '#';
                        everyWallInGameField[randomI, randomJ, randomEmptyWall] = 2;
                    }
                    else if (randomEmptyWall % 4 == 3) // Right
                    {
                        gameField[2 + randomI * 5 + 0, 2 + randomJ * 5 + 0] = '#';
                        gameField[2 + randomI * 5 + 1, 2 + randomJ * 5 + 0] = '#';
                        gameField[2 + randomI * 5 + 2, 2 + randomJ * 5 + 0] = '#';
                        gameField[2 + randomI * 5 + 3, 2 + randomJ * 5 + 0] = '#';
                        everyWallInGameField[randomI, randomJ, randomEmptyWall] = 4;
                    }

                }
                //Three walls in a block of wall:
                if (emptyWallCounter1 == 1) //If there is only one empty wall in the chosen wall block, the program has to delete one wall.
                {
                    int randomFilledWall = rand.Next(0, 4);
                    bool flag = false;
                    while (flag == false)
                    {
                        randomFilledWall = rand.Next(0, 4);
                        if (existanceOfWalls[randomFilledWall] != 0)
                            flag = true;
                    }

                    //Upperwall = array[0]; LeftsideWall = array[1]; LowerWall = array[2]; RightsideWall = array[3]
                    if (randomFilledWall % 4 == 0) //Uppper
                    {
                        if (gameField[2 + randomI * 5 + 1, 2 + randomJ * 5 + 0] == ' ' && gameField[2 + randomI * 5 + 1, 2 + randomJ * 5 + 3] == ' ')
                        {
                            gameField[2 + randomI * 5 + 0, 2 + randomJ * 5 + 0] = ' ';
                            gameField[2 + randomI * 5 + 0, 2 + randomJ * 5 + 1] = ' ';
                            gameField[2 + randomI * 5 + 0, 2 + randomJ * 5 + 2] = ' ';
                            gameField[2 + randomI * 5 + 0, 2 + randomJ * 5 + 3] = ' ';
                        }
                        if (gameField[2 + randomI * 5 + 1, 2 + randomJ * 5 + 0] == '#' && gameField[2 + randomI * 5 + 1, 2 + randomJ * 5 + 3] == '#')
                        {
                            gameField[2 + randomI * 5 + 0, 2 + randomJ * 5 + 0] = '#';
                            gameField[2 + randomI * 5 + 0, 2 + randomJ * 5 + 1] = ' ';
                            gameField[2 + randomI * 5 + 0, 2 + randomJ * 5 + 2] = ' ';
                            gameField[2 + randomI * 5 + 0, 2 + randomJ * 5 + 3] = '#';
                        }
                        if (gameField[2 + randomI * 5 + 1, 2 + randomJ * 5 + 0] == '#' && gameField[2 + randomI * 5 + 1, 2 + randomJ * 5 + 3] == ' ')
                        {
                            gameField[2 + randomI * 5 + 0, 2 + randomJ * 5 + 0] = '#';
                            gameField[2 + randomI * 5 + 0, 2 + randomJ * 5 + 1] = ' ';
                            gameField[2 + randomI * 5 + 0, 2 + randomJ * 5 + 2] = ' ';
                            gameField[2 + randomI * 5 + 0, 2 + randomJ * 5 + 3] = ' ';
                        }
                        if (gameField[2 + randomI * 5 + 1, 2 + randomJ * 5 + 0] == ' ' && gameField[2 + randomI * 5 + 1, 2 + randomJ * 5 + 3] == '#')
                        {
                            gameField[2 + randomI * 5 + 0, 2 + randomJ * 5 + 0] = ' ';
                            gameField[2 + randomI * 5 + 0, 2 + randomJ * 5 + 1] = ' ';
                            gameField[2 + randomI * 5 + 0, 2 + randomJ * 5 + 2] = ' ';
                            gameField[2 + randomI * 5 + 0, 2 + randomJ * 5 + 3] = '#';
                        }

                    }
                    else if (randomFilledWall % 4 == 1) // Left
                    {
                        if (gameField[2 + randomI * 5 + 0, 2 + randomJ * 5 + 2] == ' ' && gameField[2 + randomI * 5 + 3, 2 + randomJ * 5 + 2] == ' ')
                        {
                            gameField[2 + randomI * 5 + 0, 2 + randomJ * 5 + 3] = ' ';
                            gameField[2 + randomI * 5 + 1, 2 + randomJ * 5 + 3] = ' ';
                            gameField[2 + randomI * 5 + 2, 2 + randomJ * 5 + 3] = ' ';
                            gameField[2 + randomI * 5 + 3, 2 + randomJ * 5 + 3] = ' ';
                        }
                        if (gameField[2 + randomI * 5 + 0, 2 + randomJ * 5 + 2] == '#' && gameField[2 + randomI * 5 + 3, 2 + randomJ * 5 + 2] == '#')
                        {
                            gameField[2 + randomI * 5 + 0, 2 + randomJ * 5 + 3] = '#';
                            gameField[2 + randomI * 5 + 1, 2 + randomJ * 5 + 3] = ' ';
                            gameField[2 + randomI * 5 + 2, 2 + randomJ * 5 + 3] = ' ';
                            gameField[2 + randomI * 5 + 3, 2 + randomJ * 5 + 3] = '#';
                        }
                        if (gameField[2 + randomI * 5 + 0, 2 + randomJ * 5 + 2] == '#' && gameField[2 + randomI * 5 + 3, 2 + randomJ * 5 + 2] == ' ')
                        {
                            gameField[2 + randomI * 5 + 0, 2 + randomJ * 5 + 3] = '#';
                            gameField[2 + randomI * 5 + 1, 2 + randomJ * 5 + 3] = ' ';
                            gameField[2 + randomI * 5 + 2, 2 + randomJ * 5 + 3] = ' ';
                            gameField[2 + randomI * 5 + 3, 2 + randomJ * 5 + 3] = ' ';
                        }
                        else if (gameField[2 + randomI * 5 + 0, 2 + randomJ * 5 + 2] == ' ' && gameField[2 + randomI * 5 + 3, 2 + randomJ * 5 + 2] == '#')
                        {
                            gameField[2 + randomI * 5 + 0, 2 + randomJ * 5 + 3] = ' ';
                            gameField[2 + randomI * 5 + 1, 2 + randomJ * 5 + 3] = ' ';
                            gameField[2 + randomI * 5 + 2, 2 + randomJ * 5 + 3] = ' ';
                            gameField[2 + randomI * 5 + 3, 2 + randomJ * 5 + 3] = '#';
                        }
                    }
                    else if (randomFilledWall % 4 == 2) // Lower
                    {
                        if (gameField[2 + randomI * 5 + 2, 2 + randomJ * 5 + 0] == ' ' && gameField[2 + randomI * 5 + 2, 2 + randomJ * 5 + 3] == ' ')
                        {
                            gameField[2 + randomI * 5 + 3, 2 + randomJ * 5 + 0] = ' ';
                            gameField[2 + randomI * 5 + 3, 2 + randomJ * 5 + 1] = ' ';
                            gameField[2 + randomI * 5 + 3, 2 + randomJ * 5 + 2] = ' ';
                            gameField[2 + randomI * 5 + 3, 2 + randomJ * 5 + 3] = ' ';
                        }
                        if (gameField[2 + randomI * 5 + 2, 2 + randomJ * 5 + 0] == '#' && gameField[2 + randomI * 5 + 2, 2 + randomJ * 5 + 3] == '#')
                        {
                            gameField[2 + randomI * 5 + 3, 2 + randomJ * 5 + 0] = '#';
                            gameField[2 + randomI * 5 + 3, 2 + randomJ * 5 + 1] = ' ';
                            gameField[2 + randomI * 5 + 3, 2 + randomJ * 5 + 2] = ' ';
                            gameField[2 + randomI * 5 + 3, 2 + randomJ * 5 + 3] = '#';
                        }
                        if (gameField[2 + randomI * 5 + 2, 2 + randomJ * 5 + 0] == '#' && gameField[2 + randomI * 5 + 2, 2 + randomJ * 5 + 3] == ' ')
                        {
                            gameField[2 + randomI * 5 + 3, 2 + randomJ * 5 + 0] = '#';
                            gameField[2 + randomI * 5 + 3, 2 + randomJ * 5 + 1] = ' ';
                            gameField[2 + randomI * 5 + 3, 2 + randomJ * 5 + 2] = ' ';
                            gameField[2 + randomI * 5 + 3, 2 + randomJ * 5 + 3] = ' ';
                        }
                        else if (gameField[2 + randomI * 5 + 2, 2 + randomJ * 5 + 0] == ' ' && gameField[2 + randomI * 5 + 2, 2 + randomJ * 5 + 3] == '#')
                        {
                            gameField[2 + randomI * 5 + 3, 2 + randomJ * 5 + 0] = ' ';
                            gameField[2 + randomI * 5 + 3, 2 + randomJ * 5 + 1] = ' ';
                            gameField[2 + randomI * 5 + 3, 2 + randomJ * 5 + 2] = ' ';
                            gameField[2 + randomI * 5 + 3, 2 + randomJ * 5 + 3] = '#';
                        }
                    }
                    else if (randomFilledWall % 4 == 3) // Right
                    {
                        if (gameField[2 + randomI * 5 + 0, 2 + randomJ * 5 + 1] == ' ' && gameField[2 + randomI * 5 + 3, 2 + randomJ * 5 + 1] == ' ')
                        {
                            gameField[2 + randomI * 5 + 0, 2 + randomJ * 5 + 0] = ' ';
                            gameField[2 + randomI * 5 + 1, 2 + randomJ * 5 + 0] = ' ';
                            gameField[2 + randomI * 5 + 2, 2 + randomJ * 5 + 0] = ' ';
                            gameField[2 + randomI * 5 + 3, 2 + randomJ * 5 + 0] = ' ';
                        }
                        if (gameField[2 + randomI * 5 + 0, 2 + randomJ * 5 + 1] == '#' && gameField[2 + randomI * 5 + 3, 2 + randomJ * 5 + 1] == '#')
                        {
                            gameField[2 + randomI * 5 + 0, 2 + randomJ * 5 + 0] = '#';
                            gameField[2 + randomI * 5 + 1, 2 + randomJ * 5 + 0] = ' ';
                            gameField[2 + randomI * 5 + 2, 2 + randomJ * 5 + 0] = ' ';
                            gameField[2 + randomI * 5 + 3, 2 + randomJ * 5 + 0] = '#';
                        }
                        if (gameField[2 + randomI * 5 + 0, 2 + randomJ * 5 + 1] == '#' && gameField[2 + randomI * 5 + 3, 2 + randomJ * 5 + 1] == ' ')
                        {
                            gameField[2 + randomI * 5 + 0, 2 + randomJ * 5 + 0] = '#';
                            gameField[2 + randomI * 5 + 1, 2 + randomJ * 5 + 0] = ' ';
                            gameField[2 + randomI * 5 + 2, 2 + randomJ * 5 + 0] = ' ';
                            gameField[2 + randomI * 5 + 3, 2 + randomJ * 5 + 0] = ' ';
                        }
                        if (gameField[2 + randomI * 5 + 0, 2 + randomJ * 5 + 1] == ' ' && gameField[2 + randomI * 5 + 3, 2 + randomJ * 5 + 1] == '#')
                        {
                            gameField[2 + randomI * 5 + 0, 2 + randomJ * 5 + 0] = ' ';
                            gameField[2 + randomI * 5 + 1, 2 + randomJ * 5 + 0] = ' ';
                            gameField[2 + randomI * 5 + 2, 2 + randomJ * 5 + 0] = ' ';
                            gameField[2 + randomI * 5 + 3, 2 + randomJ * 5 + 0] = '#';
                        }
                    }
                    everyWallInGameField[randomI, randomJ, randomFilledWall] = 0;
                }
                //Two walls in a block of wall;
                if (emptyWallCounter1 == 2)  //If there are two walls, the program will randomly choose to add or delete a wall.
                {
                    int addOrDeleteWall = rand.Next(0, 2); // 0-> Delete; 1-> Add;
                    if (addOrDeleteWall == 0)
                    {
                        int[] indexesOfFilledWalls = new int[2];
                        int j = 0;
                        for (int t = 0; t < everyWallInGameField.GetLength(2); t++)
                        {
                            if (everyWallInGameField[randomI, randomJ, t] != 0)
                            {
                                indexesOfFilledWalls[j] = t;
                                j++;
                            }
                        }

                        int randomIndex = rand.Next(0, 2);
                        int i = indexesOfFilledWalls[randomIndex];
                        if (i % 4 == 0) //Uppper
                        {
                            if (gameField[2 + randomI * 5 + 1, 2 + randomJ * 5 + 0] == ' ' && gameField[2 + randomI * 5 + 1, 2 + randomJ * 5 + 3] == ' ')
                            {
                                gameField[2 + randomI * 5 + 0, 2 + randomJ * 5 + 0] = ' ';
                                gameField[2 + randomI * 5 + 0, 2 + randomJ * 5 + 1] = ' ';
                                gameField[2 + randomI * 5 + 0, 2 + randomJ * 5 + 2] = ' ';
                                gameField[2 + randomI * 5 + 0, 2 + randomJ * 5 + 3] = ' ';
                            }
                            if (gameField[2 + randomI * 5 + 1, 2 + randomJ * 5 + 0] == '#' && gameField[2 + randomI * 5 + 1, 2 + randomJ * 5 + 3] == '#')
                            {
                                gameField[2 + randomI * 5 + 0, 2 + randomJ * 5 + 0] = '#';
                                gameField[2 + randomI * 5 + 0, 2 + randomJ * 5 + 1] = ' ';
                                gameField[2 + randomI * 5 + 0, 2 + randomJ * 5 + 2] = ' ';
                                gameField[2 + randomI * 5 + 0, 2 + randomJ * 5 + 3] = '#';
                            }
                            if (gameField[2 + randomI * 5 + 1, 2 + randomJ * 5 + 0] == '#' && gameField[2 + randomI * 5 + 1, 2 + randomJ * 5 + 3] == ' ')
                            {
                                gameField[2 + randomI * 5 + 0, 2 + randomJ * 5 + 0] = '#';
                                gameField[2 + randomI * 5 + 0, 2 + randomJ * 5 + 1] = ' ';
                                gameField[2 + randomI * 5 + 0, 2 + randomJ * 5 + 2] = ' ';
                                gameField[2 + randomI * 5 + 0, 2 + randomJ * 5 + 3] = ' ';
                            }
                            if (gameField[2 + randomI * 5 + 1, 2 + randomJ * 5 + 0] == ' ' && gameField[2 + randomI * 5 + 1, 2 + randomJ * 5 + 3] == '#')
                            {
                                gameField[2 + randomI * 5 + 0, 2 + randomJ * 5 + 0] = ' ';
                                gameField[2 + randomI * 5 + 0, 2 + randomJ * 5 + 1] = ' ';
                                gameField[2 + randomI * 5 + 0, 2 + randomJ * 5 + 2] = ' ';
                                gameField[2 + randomI * 5 + 0, 2 + randomJ * 5 + 3] = '#';
                            }
                        }
                        else if (i % 4 == 1) // Left
                        {
                            if (gameField[2 + randomI * 5 + 0, 2 + randomJ * 5 + 2] == ' ' && gameField[2 + randomI * 5 + 3, 2 + randomJ * 5 + 2] == ' ')
                            {
                                gameField[2 + randomI * 5 + 0, 2 + randomJ * 5 + 3] = ' ';
                                gameField[2 + randomI * 5 + 1, 2 + randomJ * 5 + 3] = ' ';
                                gameField[2 + randomI * 5 + 2, 2 + randomJ * 5 + 3] = ' ';
                                gameField[2 + randomI * 5 + 3, 2 + randomJ * 5 + 3] = ' ';
                            }
                            if (gameField[2 + randomI * 5 + 0, 2 + randomJ * 5 + 2] == '#' && gameField[2 + randomI * 5 + 3, 2 + randomJ * 5 + 2] == '#')
                            {
                                gameField[2 + randomI * 5 + 0, 2 + randomJ * 5 + 3] = '#';
                                gameField[2 + randomI * 5 + 1, 2 + randomJ * 5 + 3] = ' ';
                                gameField[2 + randomI * 5 + 2, 2 + randomJ * 5 + 3] = ' ';
                                gameField[2 + randomI * 5 + 3, 2 + randomJ * 5 + 3] = '#';
                            }
                            if (gameField[2 + randomI * 5 + 0, 2 + randomJ * 5 + 2] == '#' && gameField[2 + randomI * 5 + 3, 2 + randomJ * 5 + 2] == ' ')
                            {
                                gameField[2 + randomI * 5 + 0, 2 + randomJ * 5 + 3] = '#';
                                gameField[2 + randomI * 5 + 1, 2 + randomJ * 5 + 3] = ' ';
                                gameField[2 + randomI * 5 + 2, 2 + randomJ * 5 + 3] = ' ';
                                gameField[2 + randomI * 5 + 3, 2 + randomJ * 5 + 3] = ' ';
                            }
                            else if (gameField[2 + randomI * 5 + 0, 2 + randomJ * 5 + 2] == ' ' && gameField[2 + randomI * 5 + 3, 2 + randomJ * 5 + 2] == '#')
                            {
                                gameField[2 + randomI * 5 + 0, 2 + randomJ * 5 + 3] = ' ';
                                gameField[2 + randomI * 5 + 1, 2 + randomJ * 5 + 3] = ' ';
                                gameField[2 + randomI * 5 + 2, 2 + randomJ * 5 + 3] = ' ';
                                gameField[2 + randomI * 5 + 3, 2 + randomJ * 5 + 3] = '#';
                            }
                        }
                        else if (i % 4 == 2) // Lower
                        {
                            if (gameField[2 + randomI * 5 + 2, 2 + randomJ * 5 + 0] == ' ' && gameField[2 + randomI * 5 + 2, 2 + randomJ * 5 + 3] == ' ')
                            {
                                gameField[2 + randomI * 5 + 3, 2 + randomJ * 5 + 0] = ' ';
                                gameField[2 + randomI * 5 + 3, 2 + randomJ * 5 + 1] = ' ';
                                gameField[2 + randomI * 5 + 3, 2 + randomJ * 5 + 2] = ' ';
                                gameField[2 + randomI * 5 + 3, 2 + randomJ * 5 + 3] = ' ';
                            }
                            if (gameField[2 + randomI * 5 + 2, 2 + randomJ * 5 + 0] == '#' && gameField[2 + randomI * 5 + 2, 2 + randomJ * 5 + 3] == '#')
                            {
                                gameField[2 + randomI * 5 + 3, 2 + randomJ * 5 + 0] = '#';
                                gameField[2 + randomI * 5 + 3, 2 + randomJ * 5 + 1] = ' ';
                                gameField[2 + randomI * 5 + 3, 2 + randomJ * 5 + 2] = ' ';
                                gameField[2 + randomI * 5 + 3, 2 + randomJ * 5 + 3] = '#';
                            }
                            if (gameField[2 + randomI * 5 + 2, 2 + randomJ * 5 + 0] == '#' && gameField[2 + randomI * 5 + 2, 2 + randomJ * 5 + 3] == ' ')
                            {
                                gameField[2 + randomI * 5 + 3, 2 + randomJ * 5 + 0] = '#';
                                gameField[2 + randomI * 5 + 3, 2 + randomJ * 5 + 1] = ' ';
                                gameField[2 + randomI * 5 + 3, 2 + randomJ * 5 + 2] = ' ';
                                gameField[2 + randomI * 5 + 3, 2 + randomJ * 5 + 3] = ' ';
                            }
                            else if (gameField[2 + randomI * 5 + 2, 2 + randomJ * 5 + 0] == ' ' && gameField[2 + randomI * 5 + 2, 2 + randomJ * 5 + 3] == '#')
                            {
                                gameField[2 + randomI * 5 + 3, 2 + randomJ * 5 + 0] = ' ';
                                gameField[2 + randomI * 5 + 3, 2 + randomJ * 5 + 1] = ' ';
                                gameField[2 + randomI * 5 + 3, 2 + randomJ * 5 + 2] = ' ';
                                gameField[2 + randomI * 5 + 3, 2 + randomJ * 5 + 3] = '#';
                            }
                        }
                        else if (i % 4 == 3) // Right
                        {
                            if (gameField[2 + randomI * 5 + 0, 2 + randomJ * 5 + 1] == ' ' && gameField[2 + randomI * 5 + 3, 2 + randomJ * 5 + 1] == ' ')
                            {
                                gameField[2 + randomI * 5 + 0, 2 + randomJ * 5 + 0] = ' ';
                                gameField[2 + randomI * 5 + 1, 2 + randomJ * 5 + 0] = ' ';
                                gameField[2 + randomI * 5 + 2, 2 + randomJ * 5 + 0] = ' ';
                                gameField[2 + randomI * 5 + 3, 2 + randomJ * 5 + 0] = ' ';
                            }
                            if (gameField[2 + randomI * 5 + 0, 2 + randomJ * 5 + 1] == '#' && gameField[2 + randomI * 5 + 3, 2 + randomJ * 5 + 1] == '#')
                            {
                                gameField[2 + randomI * 5 + 0, 2 + randomJ * 5 + 0] = '#';
                                gameField[2 + randomI * 5 + 1, 2 + randomJ * 5 + 0] = ' ';
                                gameField[2 + randomI * 5 + 2, 2 + randomJ * 5 + 0] = ' ';
                                gameField[2 + randomI * 5 + 3, 2 + randomJ * 5 + 0] = '#';
                            }
                            if (gameField[2 + randomI * 5 + 0, 2 + randomJ * 5 + 1] == '#' && gameField[2 + randomI * 5 + 3, 2 + randomJ * 5 + 1] == ' ')
                            {
                                gameField[2 + randomI * 5 + 0, 2 + randomJ * 5 + 0] = '#';
                                gameField[2 + randomI * 5 + 1, 2 + randomJ * 5 + 0] = ' ';
                                gameField[2 + randomI * 5 + 2, 2 + randomJ * 5 + 0] = ' ';
                                gameField[2 + randomI * 5 + 3, 2 + randomJ * 5 + 0] = ' ';
                            }
                            if (gameField[2 + randomI * 5 + 0, 2 + randomJ * 5 + 1] == ' ' && gameField[2 + randomI * 5 + 3, 2 + randomJ * 5 + 1] == '#')
                            {
                                gameField[2 + randomI * 5 + 0, 2 + randomJ * 5 + 0] = ' ';
                                gameField[2 + randomI * 5 + 1, 2 + randomJ * 5 + 0] = ' ';
                                gameField[2 + randomI * 5 + 2, 2 + randomJ * 5 + 0] = ' ';
                                gameField[2 + randomI * 5 + 3, 2 + randomJ * 5 + 0] = '#';
                            }
                        }
                        everyWallInGameField[randomI, randomJ, i] = 0;
                    }
                    //////
                    if (addOrDeleteWall == 1)
                    {
                        int[] indexesOfEmptyWalls = new int[2];
                        int j = 0;
                        for (int t = 0; t < everyWallInGameField.GetLength(2); t++)
                        {
                            if (everyWallInGameField[randomI, randomJ, t] == 0)
                            {
                                indexesOfEmptyWalls[j] = t;
                                j++;
                            }
                        }
                        j = 0;
                        int randomIndex = rand.Next(0, 2);
                        int i = indexesOfEmptyWalls[randomIndex];
                        if (i % 4 == 0) //Uppper
                        {
                            gameField[2 + randomI * 5 + 0, 2 + randomJ * 5 + 0] = '#';
                            gameField[2 + randomI * 5 + 0, 2 + randomJ * 5 + 1] = '#';
                            gameField[2 + randomI * 5 + 0, 2 + randomJ * 5 + 2] = '#';
                            gameField[2 + randomI * 5 + 0, 2 + randomJ * 5 + 3] = '#';
                            everyWallInGameField[randomI, randomJ, i] = 1;
                        }
                        else if (i % 4 == 1) // Left
                        {
                            gameField[2 + randomI * 5 + 0, 2 + randomJ * 5 + 3] = '#';
                            gameField[2 + randomI * 5 + 1, 2 + randomJ * 5 + 3] = '#';
                            gameField[2 + randomI * 5 + 2, 2 + randomJ * 5 + 3] = '#';
                            gameField[2 + randomI * 5 + 3, 2 + randomJ * 5 + 3] = '#';
                            everyWallInGameField[randomI, randomJ, i] = 3;
                        }
                        else if (i % 4 == 2) // Lower
                        {
                            gameField[2 + randomI * 5 + 3, 2 + randomJ * 5 + 0] = '#';
                            gameField[2 + randomI * 5 + 3, 2 + randomJ * 5 + 1] = '#';
                            gameField[2 + randomI * 5 + 3, 2 + randomJ * 5 + 2] = '#';
                            gameField[2 + randomI * 5 + 3, 2 + randomJ * 5 + 3] = '#';
                            everyWallInGameField[randomI, randomJ, i] = 2;
                        }
                        else if (i % 4 == 3) // Right
                        {
                            gameField[2 + randomI * 5 + 0, 2 + randomJ * 5 + 0] = '#';
                            gameField[2 + randomI * 5 + 1, 2 + randomJ * 5 + 0] = '#';
                            gameField[2 + randomI * 5 + 2, 2 + randomJ * 5 + 0] = '#';
                            gameField[2 + randomI * 5 + 3, 2 + randomJ * 5 + 0] = '#';
                            everyWallInGameField[randomI, randomJ, i] = 4;
                        }
                    }
                }
            }

            // Game Status
            void Status()
            {
                // Status
                Console.SetCursorPosition(60, 3);
                Console.WriteLine("Time     :  " + (int)time);
                Console.SetCursorPosition(60, 5);
                Console.WriteLine("Score    :  " + score);
                Console.SetCursorPosition(60, 7);
                Console.WriteLine("Energy   :  " + playerEnergy + " ");
                Console.SetCursorPosition(60, 9);
                Console.WriteLine("Mine     :  " + mine);
            }

            // 1,2,3 Numbers
           void Gifts(int x_gifts, int y_gifts, int possibility, char[,] game_area)
           {

                if (possibility < 6 && (game_area[y_gifts, x_gifts] == ' '))
                {
                    game_area[y_gifts, x_gifts] = '1';
                }
                else if ((possibility > 5) && (possibility < 9) && game_area[y_gifts, x_gifts] == ' ')
                {
                    game_area[y_gifts, x_gifts] = '2';
                }
                else if ((possibility == 9) && game_area[y_gifts, x_gifts] == ' ')
                {
                    game_area[y_gifts, x_gifts] = '3';
                }
           }


            void SpawnEnemies()
            {
                if (time % 150 == 0)
                {
                    do
                    {
                        enemyX[ind].X = rand.Next(4, 55);
                        enemyX[ind].Y = rand.Next(4, 25);
                    } while (gameField[enemyX[ind].Y - 3, enemyX[ind].X - 3] == '#' || gameField[enemyX[ind].Y - 3, enemyX[ind].X - 3] == 'X' || gameField[enemyX[ind].Y - 3, enemyX[ind].X - 3] == 'Y' || gameField[enemyX[ind].Y - 3, enemyX[ind].X - 3] == 'P');

                    enemyX[ind].Direction = 1;
                    ind++;
                }

                for (int indexOfEnemyX = 0; indexOfEnemyX < ind; indexOfEnemyX++)
                {
                    

                    if (enemyX[indexOfEnemyX].Direction!=0)
                    {
                        gameField[enemyX[indexOfEnemyX].Y - 3, enemyX[indexOfEnemyX].X - 3] = 'X'; 

                        if (enemyX[indexOfEnemyX].X != cursorx)
                        {
                            if (enemyX[indexOfEnemyX].Direction == 1 && enemyX[indexOfEnemyX].X > cursorx) enemyX[indexOfEnemyX].Direction = -1;
                            if (enemyX[indexOfEnemyX].Direction == -1 && enemyX[indexOfEnemyX].X < cursorx) enemyX[indexOfEnemyX].Direction = 1;
                            if (enemyX[indexOfEnemyX].Direction == 1 && gameField[enemyX[indexOfEnemyX].Y - 3, enemyX[indexOfEnemyX].X - 3 + 1] != '#')
                            {
                                Console.SetCursorPosition(enemyX[indexOfEnemyX].X, enemyX[indexOfEnemyX].Y);    // delete old enemy
                                Console.WriteLine(" ");
                                gameField[enemyX[indexOfEnemyX].Y - 3, enemyX[indexOfEnemyX].X - 3] = ' ';
                                enemyX[indexOfEnemyX].X = enemyX[indexOfEnemyX].X + enemyX[indexOfEnemyX].Direction;
                                Console.SetCursorPosition(enemyX[indexOfEnemyX].X, enemyX[indexOfEnemyX].Y);    // refresh enemy (current position)
                                Console.ForegroundColor = System.ConsoleColor.Cyan;
                                Console.WriteLine("X");
                                gameField[enemyX[indexOfEnemyX].Y - 3, enemyX[indexOfEnemyX].X - 3] = 'X';
                                Console.ResetColor();
                            }
                            if (enemyX[indexOfEnemyX].Direction == -1 && gameField[enemyX[indexOfEnemyX].Y - 3, enemyX[indexOfEnemyX].X - 3 - 1] != '#')
                            {
                                Console.SetCursorPosition(enemyX[indexOfEnemyX].X, enemyX[indexOfEnemyX].Y);    // delete old enemy
                                Console.WriteLine(" ");
                                gameField[enemyX[indexOfEnemyX].Y - 3, enemyX[indexOfEnemyX].X - 3] = ' ';
                                enemyX[indexOfEnemyX].X = enemyX[indexOfEnemyX].X + enemyX[indexOfEnemyX].Direction;
                                Console.SetCursorPosition(enemyX[indexOfEnemyX].X, enemyX[indexOfEnemyX].Y);    // refresh enemy (current position)
                                Console.ForegroundColor = System.ConsoleColor.Cyan;
                                Console.WriteLine("X");
                                gameField[enemyX[indexOfEnemyX].Y - 3, enemyX[indexOfEnemyX].X - 3] = 'X';
                                Console.ResetColor();
                            }
                        }
                        if (enemyX[indexOfEnemyX].X == cursorx)
                        {
                            if (enemyX[indexOfEnemyX].Direction == 1 && enemyX[indexOfEnemyX].Y > cursory) enemyX[indexOfEnemyX].Direction = -1;
                            if (enemyX[indexOfEnemyX].Direction == -1 && enemyX[indexOfEnemyX].Y < cursory) enemyX[indexOfEnemyX].Direction = 1;
                            if (enemyX[indexOfEnemyX].Direction == 1 && gameField[enemyX[indexOfEnemyX].Y - 3 + 1, enemyX[indexOfEnemyX].X - 3] != '#')
                            {
                                Console.SetCursorPosition(enemyX[indexOfEnemyX].X, enemyX[indexOfEnemyX].Y);    // delete old enemy
                                Console.WriteLine(" ");
                                gameField[enemyX[indexOfEnemyX].Y - 3, enemyX[indexOfEnemyX].X - 3] = ' ';
                                enemyX[indexOfEnemyX].Y = enemyX[indexOfEnemyX].Y + enemyX[indexOfEnemyX].Direction;
                                Console.SetCursorPosition(enemyX[indexOfEnemyX].X, enemyX[indexOfEnemyX].Y);    // refresh enemy (current position)
                                Console.ForegroundColor = System.ConsoleColor.Cyan;
                                Console.WriteLine("X");
                                gameField[enemyX[indexOfEnemyX].Y - 3, enemyX[indexOfEnemyX].X - 3] = 'X';
                                Console.ResetColor();
                            }
                            if (enemyX[indexOfEnemyX].Direction == -1 && gameField[enemyX[indexOfEnemyX].Y - 3 - 1, enemyX[indexOfEnemyX].X - 3] != '#')
                            {
                                Console.SetCursorPosition(enemyX[indexOfEnemyX].X, enemyX[indexOfEnemyX].Y);    // delete old enemy
                                Console.WriteLine(" ");
                                gameField[enemyX[indexOfEnemyX].Y - 3, enemyX[indexOfEnemyX].X - 3] = ' ';
                                enemyX[indexOfEnemyX].Y = enemyX[indexOfEnemyX].Y + enemyX[indexOfEnemyX].Direction;
                                Console.SetCursorPosition(enemyX[indexOfEnemyX].X, enemyX[indexOfEnemyX].Y);    // refresh enemy (current position)
                                Console.ForegroundColor = System.ConsoleColor.Cyan;
                                Console.WriteLine("X");
                                gameField[enemyX[indexOfEnemyX].Y - 3, enemyX[indexOfEnemyX].X - 3] = 'X';
                                Console.ResetColor();
                            }
                        }
                        if (gameField[cursory - 3, cursorx - 3] == 'X' || gameField[cursory - 3, cursorx - 3] == 'Y')
                        {
                            Console.ForegroundColor = System.ConsoleColor.DarkRed;
                            Console.SetCursorPosition(cursorx, cursory);
                            Console.Write("GAME OVER!!!");
                            Console.ResetColor();
                            alive = false;
                            Console.ReadKey();
                            break;
                        }
                        if (gameField[enemyX[indexOfEnemyX].Y - 3, enemyX[indexOfEnemyX].X - 3] == '+')
                        {
                            enemyX[indexOfEnemyX].Direction = 0;
                        }
                    }
                }



                //if (time % 150 == 0)
                //{
                //    do
                //    {
                //        enemyY[ind2].X = rand.Next(4, 55);
                //        enemyY[ind2].Y = rand.Next(4, 25);
                //    } while (gameField[enemyY[ind2].Y - 3, enemyY[ind2].X - 3] == '#' || gameField[enemyY[ind2].Y - 3, enemyY[ind2].X - 3] == 'X' || gameField[enemyY[ind2].Y - 3, enemyY[ind2].X - 3] == 'Y' || gameField[enemyY[ind2].Y - 3, enemyY[ind2].X - 3] == 'P');

                //    enemyY[ind2].Direction = 1;
                //    ind2++;
                //}


                //for (int indexOfEnemyY = 0; indexOfEnemyY < ind2; indexOfEnemyY++)
                //{
                //    gameField[enemyY[indexOfEnemyY].Y - 3, enemyY[indexOfEnemyY].X - 3] = 'Y';

                //    if (enemyY[indexOfEnemyY].Y != cursory)
                //    {
                //        if (enemyY[indexOfEnemyY].Direction == 1 && enemyY[indexOfEnemyY].Y > cursory) enemyY[indexOfEnemyY].Direction = -1;
                //        if (enemyY[indexOfEnemyY].Direction == -1 && enemyY[indexOfEnemyY].Y < cursory) enemyY[indexOfEnemyY].Direction = 1;
                //        if (enemyY[indexOfEnemyY].Direction == 1 && gameField[enemyY[indexOfEnemyY].Y - 3 + 1, enemyY[indexOfEnemyY].X - 3] != '#')
                //        {
                //            Console.SetCursorPosition(enemyY[indexOfEnemyY].X, enemyY[indexOfEnemyY].Y);    // delete old enemy
                //            Console.WriteLine(" ");
                //            gameField[enemyY[indexOfEnemyY].Y - 3, enemyY[indexOfEnemyY].X - 3] = ' ';
                //            enemyY[indexOfEnemyY].Y = enemyY[indexOfEnemyY].Y + enemyY[indexOfEnemyY].Direction;
                //            Console.SetCursorPosition(enemyY[indexOfEnemyY].X, enemyY[indexOfEnemyY].Y);    // refresh enemy (current position)
                //            Console.ForegroundColor = System.ConsoleColor.Cyan;
                //            Console.WriteLine("Y");
                //            gameField[enemyY[indexOfEnemyY].Y - 3, enemyY[indexOfEnemyY].X - 3] = 'Y';
                //            Console.ResetColor();
                //        }
                //        if (enemyY[indexOfEnemyY].Direction == -1 && gameField[enemyY[indexOfEnemyY].Y - 3 - 1, enemyY[indexOfEnemyY].X - 3] != '#')
                //        {
                //            Console.SetCursorPosition(enemyY[indexOfEnemyY].X, enemyY[indexOfEnemyY].Y);    // delete old enemy
                //            Console.WriteLine(" ");
                //            gameField[enemyY[indexOfEnemyY].Y - 3, enemyY[indexOfEnemyY].X - 3] = ' ';
                //            enemyY[indexOfEnemyY].Y = enemyY[indexOfEnemyY].Y + enemyY[indexOfEnemyY].Direction;
                //            Console.SetCursorPosition(enemyY[indexOfEnemyY].X, enemyY[indexOfEnemyY].Y);    // refresh enemy (current position)
                //            Console.ForegroundColor = System.ConsoleColor.Cyan;
                //            Console.WriteLine("Y");
                //            gameField[enemyY[indexOfEnemyY].Y - 3, enemyY[indexOfEnemyY].X - 3] = 'Y';
                //            Console.ResetColor();
                //        }
                //    }
                //    if (enemyY[indexOfEnemyY].Y == cursory)
                //    {
                //        if (enemyY[indexOfEnemyY].Direction == 1 && enemyY[indexOfEnemyY].X > cursorx) enemyY[indexOfEnemyY].Direction = -1;
                //        if (enemyY[indexOfEnemyY].Direction == -1 && enemyY[indexOfEnemyY].X < cursorx) enemyY[indexOfEnemyY].Direction = 1;
                //        if (enemyY[indexOfEnemyY].Direction == 1 && gameField[enemyY[indexOfEnemyY].Y - 3, enemyY[indexOfEnemyY].X - 3 + 1] != '#')
                //        {
                //            Console.SetCursorPosition(enemyY[indexOfEnemyY].X, enemyY[indexOfEnemyY].Y);    // delete old enemy
                //            Console.WriteLine(" ");
                //            gameField[enemyY[indexOfEnemyY].Y - 3, enemyY[indexOfEnemyY].X - 3] = ' ';
                //            enemyY[indexOfEnemyY].X = enemyY[indexOfEnemyY].X + enemyY[indexOfEnemyY].Direction;
                //            Console.SetCursorPosition(enemyY[indexOfEnemyY].X, enemyY[indexOfEnemyY].Y);    // refresh enemy (current position)
                //            Console.ForegroundColor = System.ConsoleColor.Cyan;
                //            Console.WriteLine("Y");
                //            gameField[enemyY[indexOfEnemyY].Y - 3, enemyY[indexOfEnemyY].X - 3] = 'Y';
                //            Console.ResetColor();
                //        }
                //        if (enemyY[indexOfEnemyY].Direction == -1 && gameField[enemyY[indexOfEnemyY].Y - 3, enemyY[indexOfEnemyY].X - 3 - 1] != '#')
                //        {
                //            Console.SetCursorPosition(enemyY[indexOfEnemyY].X, enemyY[indexOfEnemyY].Y);    // delete old enemy
                //            Console.WriteLine(" ");
                //            gameField[enemyY[indexOfEnemyY].Y - 3, enemyY[indexOfEnemyY].X - 3] = ' ';
                //            enemyY[indexOfEnemyY].X = enemyY[indexOfEnemyY].X + enemyY[indexOfEnemyY].Direction;
                //            Console.SetCursorPosition(enemyY[indexOfEnemyY].X, enemyY[indexOfEnemyY].Y);    // refresh enemy (current position)
                //            Console.ForegroundColor = System.ConsoleColor.Cyan;
                //            Console.WriteLine("Y");
                //            gameField[enemyY[indexOfEnemyY].Y - 3, enemyY[indexOfEnemyY].X - 3] = 'Y';
                //            Console.ResetColor();
                //        }
                //    }
                //    if (gameField[cursory - 3, cursorx - 3] == 'X' || gameField[cursory - 3, cursorx - 3] == 'Y')
                //    {
                //        Console.ForegroundColor = System.ConsoleColor.DarkRed;
                //        Console.SetCursorPosition(cursorx, cursory);
                //        Console.Write("GAME OVER!!");
                //        Console.ResetColor();
                //        alive = false;
                //        Console.ReadKey();
                //        break;
                //    }
                //    if (gameField[enemyX[indexOfEnemyY].Y - 3, enemyX[indexOfEnemyY].X - 3] == '+')
                //    {
                //        enemyX[indexOfEnemyY].Direction = 0;

                //    }
                //}
            }
            // --- Main game loop
            void Game()
            {
                int x_gifts;
                int y_gifts;
                int possibility;

                for (int i = 0; i < 19; i++)    //generating 20 number for start of the game
                {
                    while (true)
                    {
                        Random rnd = new Random();
                        x_gifts = rnd.Next(1, 52);
                        y_gifts = rnd.Next(1, 22);
                        possibility = rnd.Next(0, 10);
                        if (gameField[y_gifts, x_gifts] == ' ')
                        {
                            break;
                        }
                    }
                    Gifts(x_gifts, y_gifts, possibility, gameField);
                }
                // --- Main game loop
                while (alive)
                {
                    if (playerEnergy == 0 && time % 2 == 0)
                    {
                    }
                    if (Console.KeyAvailable)
                    {   // true: there is a key in keyboard buffer
                        cki = Console.ReadKey(true);       // true: do not write character
                        while (Console.KeyAvailable)
                            Console.ReadKey();
                        /*---------------------------------------------------------------------------*/
                        if (cki.Key == ConsoleKey.RightArrow && cursorx <= 53 && gameField[cursory - 3, cursorx - 2] != '#')
                        {   // key and boundary control
                            Console.SetCursorPosition(cursorx, cursory); // delete enemies (old position)
                            Console.WriteLine(" ");
                            gameField[cursory - 3, cursorx - 3] = ' ';
                            cursorx++;
                            if (playerEnergy > 0)
                                playerEnergy--;
                            minePosition = 1;
                        }
                        if (cki.Key == ConsoleKey.LeftArrow && cursorx > 4 && gameField[cursory - 3, cursorx - 4] != '#')
                        {
                            Console.SetCursorPosition(cursorx, cursory);
                            Console.WriteLine(" ");
                            gameField[cursory - 3, cursorx - 3] = ' ';
                            cursorx--;
                            if (playerEnergy > 0)
                                playerEnergy--;
                            minePosition = 2;
                        }
                        if (cki.Key == ConsoleKey.UpArrow && cursory > 4 && gameField[cursory - 4, cursorx - 3] != '#')
                        {
                            Console.SetCursorPosition(cursorx, cursory);
                            Console.WriteLine(" ");
                            gameField[cursory - 3, cursorx - 3] = ' ';
                            cursory--;
                            if (playerEnergy > 0)
                                playerEnergy--;
                            minePosition = 3;
                        }
                        if (cki.Key == ConsoleKey.DownArrow && cursory < 24 && gameField[cursory - 2, cursorx - 3] != '#')
                        {
                            Console.SetCursorPosition(cursorx, cursory);
                            Console.WriteLine(" ");
                            gameField[cursory - 3, cursorx - 3] = ' ';
                            cursory++;
                            if (playerEnergy > 0)
                                playerEnergy--;
                            minePosition = 4;
                        }

                        // mine
                        if (cki.Key == ConsoleKey.Spacebar && mine != 0)
                        {
                            if (minePosition == 1 && gameField[cursory - 3, cursorx - 3 + 1] == ' ')
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.SetCursorPosition(cursorx - 1, cursory);
                                gameField[cursory - 3, cursorx - 1 - 3] = '+';
                                Console.WriteLine("+");
                                Console.ResetColor();
                                mine--;
                            }
                            else if (minePosition == 2 && gameField[cursory - 3, cursorx - 3 - 1] == ' ')
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.SetCursorPosition(cursorx + 1, cursory);
                                gameField[cursory - 3, cursorx + 1 - 3] = '+';
                                Console.WriteLine("+");
                                Console.ResetColor();
                                mine--;
                            }
                            else if (minePosition == 3 && gameField[cursory - 3 - 1, cursorx - 3] == ' ')
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.SetCursorPosition(cursorx, cursory + 1);
                                gameField[cursory - 3 + 1, cursorx - 3] = '+';
                                Console.WriteLine("+");
                                Console.ResetColor();
                                mine--;
                            }
                            else if (minePosition == 4 && gameField[cursory - 3 + 1, cursorx - 3] == ' ')
                            {

                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.SetCursorPosition(cursorx, cursory - 1);
                                gameField[cursory - 1 - 3, cursorx - 3] = '+';
                                Console.WriteLine("+");
                                Console.ResetColor();
                                mine--;
                            }
                        }

                        // Mine Exposure & Enemy Exposure
                        if (gameField[cursory - 3, cursorx - 3] == '+' || gameField[cursory - 3, cursorx - 3] == 'X' || gameField[cursory - 3, cursorx - 3] == 'Y')
                        {
                            Console.ForegroundColor = System.ConsoleColor.DarkRed;
                            Console.SetCursorPosition(cursorx, cursory);
                            Console.Write("GAME OVER!!!");
                            Console.ResetColor();
                            alive = false;
                            Console.ReadKey();
                            break;
                        }

                        // Collecting Gifts
                        if (gameField[cursory - 3, cursorx - 3] == '1')
                        {
                            Console.SetCursorPosition(cursorx, cursory);
                            Console.Write(' ');
                            gameField[cursory - 3, cursorx - 3] = ' ';
                            score += 10;
                        }
                        if (gameField[cursory - 3, cursorx - 3] == '2')
                        {
                            Console.SetCursorPosition(cursorx, cursory);
                            Console.Write(' ');
                            gameField[cursory - 3, cursorx - 3] = ' ';
                            score += 30;
                            playerEnergy += 50;
                        }
                        if (gameField[cursory - 3, cursorx - 3] == '3')
                        {
                            Console.SetCursorPosition(cursorx, cursory);
                            Console.Write(' ');
                            gameField[cursory - 3, cursorx - 3] = ' ';
                            score += 90;
                            playerEnergy += 200;
                            mine++;
                        }

                        if (cki.KeyChar >= 97 && cki.KeyChar <= 102)
                        {       // keys: a-f
                            Console.SetCursorPosition(3, 30);
                            Console.WriteLine("Pressed Key: " + cki.KeyChar);
                        }
                        if (cki.Key == ConsoleKey.Escape) break;
                    }
                    /*---------------------------------------------------------------------------*/
                    // Calling the functions
                    // strating a game with pressing a key

                    Status(); // Updating the status

                    if (time % 10 == 0)//generating 1 number in every 10 time
                    {
                        while (true)
                        {
                            Random rnd = new Random();
                            x_gifts = rnd.Next(1, 52);
                            y_gifts = rnd.Next(1, 22);
                            possibility = rnd.Next(0, 10);
                            if (gameField[y_gifts, x_gifts] == ' ')
                            {
                                break;
                            }
                        }
                        Gifts(x_gifts, y_gifts, possibility, gameField);
                    }
                    /*---------------------------------------------------------------------------*/
                    SpawnEnemies();
                    SpawnEnemies();

                    /*---------------------------------------------------------------------------*/
                    // if the player has no energy his/her movment speed is reduced by half
                    if (playerEnergy > 0)
                    {
                        Console.SetCursorPosition(cursorx, cursory);    // refresh P (current position)            
                        Console.ForegroundColor = System.ConsoleColor.Yellow;
                        Console.WriteLine("P");
                        gameField[cursory - 3, cursorx - 3] = 'P';
                        Console.ResetColor();
                    }
                    else if (playerEnergy == 0)
                    {
                        Console.SetCursorPosition(cursorx, cursory);    // refresh P (current position)            
                        Console.ForegroundColor = System.ConsoleColor.Yellow;
                        Console.WriteLine("P");
                        gameField[cursory - 3, cursorx - 3] = 'P';
                        Console.ResetColor();
                        Thread.Sleep(250);     // sleep 250 ms
                    }
                    /*---------------------------------------------------------------------------*/
                    // Time Unit of the Game
                    Thread.Sleep(200);     // sleep 200 ms
                    if (time % 1 == 0)
                    {
                        ChangeWall();
                    }

                    // Timing of the game
                    time = time + 0.5;
                    Console.SetCursorPosition(3, 3);
                    for (int i = 0; i < gameField.GetLength(0); i++)
                    {
                        for (int j = 0; j < gameField.GetLength(1); j++)
                        {
                            if (gameField[i, j] == 'X' || gameField[i, j] == 'Y')
                            {
                                Console.ForegroundColor = System.ConsoleColor.Cyan;
                                Console.Write(gameField[i, j]);
                                Console.ResetColor();
                            }
                            else if (gameField[i, j] == '+')
                            {
                                Console.ForegroundColor = System.ConsoleColor.DarkRed;
                                Console.Write(gameField[i, j]);
                                Console.ResetColor();
                            }
                            else if (gameField[i, j] == '1' || gameField[i, j] == '2' || gameField[i, j] == '3')
                            {
                                Console.ForegroundColor = System.ConsoleColor.DarkMagenta;
                                Console.Write(gameField[i, j]);
                                Console.ResetColor();
                            }
                            else
                                Console.Write(gameField[i, j]);
                        }
                        Console.WriteLine();
                        // Position of the inner walls inside the outer walls
                        Console.SetCursorPosition(3, 3 + i + 1);
                    }
                }
            }


        }
    }
}
