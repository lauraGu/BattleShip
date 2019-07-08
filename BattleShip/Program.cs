using System;
using System.Collections.Generic;
using System.Linq;


namespace BattleShip
{
    class Program
    {
        const int Destroyer = 4;
        const int Battleship = 5;
        const int mapSize = 10;

        static Ship[,] PlaceShips(List<int> ships)
        {
            Ship[,] map = new Ship[mapSize,mapSize];

            Random rand = new Random();
            int column = 0;
            int row = 0;
            int direction = 0;
            bool success;

            for (int i = 0; i < ships.Count(); i++)
            {
                success = false;
                while (success == false)
                {
                    column = rand.Next(0, mapSize);
                    row = rand.Next(0, mapSize);
                    direction = rand.Next(0, 2);
                    if (direction == 0) //horizontal
                    {
                        if (column > mapSize - ships[i])
                        {
                            column = column - ships[i] + 1;
                        }
                    }
                    else
                    {
                        if (row > mapSize - ships[i])
                        {
                            row = row - ships[i] + 1;
                        }                
                    }
                    success = ValidateShip(map, ships[i], column, row, direction);
                }
                for (int j = 0; j < ships[i]; j++)
                {
                    map[column, row] = new Ship
                    {
                        id = i,
                        size = ships[i],
                        isHit = false
                    };
                    if (direction==0) //horizontal
                    {                                               
                        column++;
                    }
                    else
                    {
                        row++;
                    }
                }
            }
            return map;

        }

        static bool ValidateShip(Ship[,] map, int length, int column, int row, int direction)
        {
            for(int i = 0; i < length; i++)
            {
                if (map[column, row] != null)
                {
                    return false;
                }
                if (direction == 0) //horizontal
                {
                    column++;
                }
                else
                {
                    row++;
                }
            }            
            return true;
        }

        static bool CoordinatesValidate (string coordinates, out int column, out int row)
        {
            column = -1;
            row = -1;
            if(!String.IsNullOrEmpty(coordinates) && coordinates.Length > 1 && coordinates.Length < 4)
            {
                column = (int)(coordinates[0] - 'A');
                bool parsed = Int32.TryParse(coordinates.Substring(1), out row);
                if (parsed)
                {
                    row = row - 1;
                }
                if (column < 0 || column >= mapSize || parsed == false || row < 0 || row >= mapSize)
                {
                    return false;
                }
                return true;
            }
            else
            {
                return false;
            }
            
        }

        static bool IsShipHit (Ship[,] map, int column, int row)
        {
            if (map[column,row] != null)
            {
                map[column, row].isHit = true;
                return true;
            }
            else
            {
                return false;
            }            
        }

        static bool IsShipSunk (Ship[,] map, int column, int row)
        {
            int size = map[column, row].size;
            int id = map[column, row].id;
            int hits = 1;
            for(int i = 1; i < size; i++)
            {
                if (hits < size)
                {
                    if (column + i < mapSize && map[column + i, row] != null && map[column + i, row].id == id && map[column + i, row].isHit == true)
                    {    
                        hits++;                        
                    }
                   
                    if (column - i >= 0 && map[column - i, row] != null && map[column - i, row].id == id && map[column - i, row].isHit == true)
                    {
                        hits++;
                    }

                    if (row + i < mapSize && map[column, row + i] != null && map[column, row + i].id == id && map[column, row + i].isHit == true)
                    {
                        hits++;                         
                    }
                        
                    if (row - i >= 0 && map[column, row - i] != null && map[column, row - i].id == id && map[column, row - i].isHit == true)
                    {
                        hits++;                        
                    }
                    
                }
                else
                {
                    return true;
                }
            }
            if (hits == size)
            {
                return true;
            }
            else
            {
                return false;
            }            
        }

        static void Main(string[] args)
        {
            List<int> ships = new List<int> { Destroyer, Destroyer, Battleship };

            Ship[,] map = PlaceShips(ships);
            string[,] hitsMap = new string[mapSize, mapSize];
            for (int i = 0; i < mapSize; i++)
            {
                for(int j = 0; j < mapSize; j++)
                {
                    hitsMap[i,j] = "-";
                }
            }
            
            bool validated = false;

            int allShipsSunk = 0;
            while (allShipsSunk < ships.Count())
            {
                Console.WriteLine("  ABCDEFGHIJ");
                for (int i = 0; i < mapSize; i++)
                {
                    if (i + 1 < 10)
                        Console.Write(" ");
                    Console.Write(i+1);
                    
                    for (int j = 0; j < mapSize; j++)
                    {
                        Console.Write(hitsMap[j, i]);
                    }
                    Console.Write("\n");
                }
                Console.WriteLine("Please enter coordinates [A-J][1-10] (e.g. A1): ");
                string coordinates = Console.ReadLine();
                validated = CoordinatesValidate(coordinates, out int column, out int row);
                if(validated == false)
                {
                    Console.WriteLine("Wrong coordinates. Please enter again. ");
                    continue;
                }
                if(IsShipHit(map, column, row))
                {
                    if(IsShipSunk(map, column, row))
                    {
                        Console.WriteLine("Sunk!");
                        allShipsSunk++;
                    }
                    else
                    {
                        Console.WriteLine("Hit!");
                    }
                    hitsMap[column, row] = "X";
                }
                else
                {
                    Console.WriteLine("Missed!");
                    hitsMap[column, row] = "*";
                }
                

            }
            Console.WriteLine("\nAll Ships are sunk! Click ENTER to close the game.");
            Console.ReadLine();
        }
    }

    class Ship
    {
        public int size;
        public int id;
        public bool isHit;
    }
}
