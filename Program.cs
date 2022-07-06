using System.Collections.Generic;
using System;

namespace pathFinding
{
    class Program
    {
        public Queue<Tile> neighbor = new Queue<Tile>();
        public Queue<Tile> back = new Queue<Tile>();

        public class Tile
        {
            public int value = 0;
            public string symbol = "■";
            public bool explored = false;
            public int x, y;
            public bool connected = false;
        }

        public static Tile[,] setMap(Tile[,] map, int maxY, int maxX)
        {
            maxY = map.GetLength(0);
            maxX = map.GetLength(1);
            int[,] nrMap = new int[6, 7]{
                {0,0,1,1,1,1,0},
                {0,0,1,0,0,1,0},
                {0,3,1,0,1,1,0},
                {0,1,0,0,1,0,0},
                {1,1,1,1,1,0,0},
                {2,0,0,0,0,0,0}

            };
            for (int i = 0; i < maxY; i++)
            {
                for (int j = 0; j < maxX; j++)
                {
                    map[i, j].value = nrMap[i, j];
                    if (nrMap[i, j] == 2)
                    {
                        map[i, j].symbol = "*";
                        map[i, j].connected = true;
                    }
                    if (nrMap[i, j] == 3)
                    {
                        map[i, j].symbol = "+";
                        map[i, j].connected = true;
                    }
                    if (nrMap[i, j] == 1)
                    {
                        map[i, j].symbol = "1";
                    }
                }
            }
            return map;
        }

        public void getNeighbors(Tile[,] map, Tile now)
        {
            try
            {
                if (map[now.y, now.x - 1].value != 0 && map[now.y, now.x - 1].explored == false)
                {
                    neighbor.Enqueue(map[now.y, now.x - 1]);
                    // Console.WriteLine("add " + map[now.y,now.x-1].x + " " + map[now.y,now.x-1].y);
                    map[now.y, now.x - 1].explored = true;
                }
            }
            catch (IndexOutOfRangeException) { }
            finally
            {

                try
                {
                    if (map[now.y, now.x + 1].value != 0 && map[now.y, now.x + 1].explored == false)
                    {
                        neighbor.Enqueue(map[now.y, now.x + 1]);
                        // Console.WriteLine("add " + map[now.y,now.x+1].x + " " + map[now.y,now.x+1].y);
                        map[now.y, now.x + 1].explored = true;
                    }
                }
                catch (IndexOutOfRangeException) { }
                finally
                {
                    try
                    {
                        if (map[now.y - 1, now.x].value != 0 && map[now.y - 1, now.x].explored == false)
                        {
                            neighbor.Enqueue(map[now.y - 1, now.x]);
                            // Console.WriteLine("add " + map[now.y-1,now.x].x + " " + map[now.y-1,now.x].y);
                            map[now.y - 1, now.x].explored = true;
                        }
                    }
                    catch (IndexOutOfRangeException) { }
                    finally
                    {
                        try
                        {
                            if (map[now.y + 1, now.x].value != 0 && map[now.y + 1, now.x].explored == false)
                            {
                                neighbor.Enqueue(map[now.y + 1, now.x]);
                                // Console.WriteLine("add " + map[now.y+1,now.x].x + " " + map[now.y+1,now.x].y);
                                map[now.y + 1, now.x].explored = true;
                            }
                        }
                        catch (IndexOutOfRangeException) { }
                        finally { }
                    }
                }
            }
        }

        public Tile[,] solve(Tile[,] map, Tile now)
        {
            if (now.value == 3)
            {
                now.symbol = "3";
                return map;
            }
            else
            {

                if (now.value == 0)
                {
                    Console.WriteLine("This is a wall");
                    return map;
                }
                if (now.value == 2 || now.value == 1)
                {
                    if (now.value == 2 && now.explored == false)
                    {
                        neighbor.Enqueue(now);
                    }
                    else
                    {
                        back.Enqueue(now);
                    }
                    now.symbol = " ";
                    now.explored = true;
                    getNeighbors(map, now);
                    now = neighbor.Dequeue();
                    return solve(map, now);
                }
                else
                {
                    return map;
                }
            }
        }

        public Tile[,] clearMap(Tile[,] map)
        {
            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    map[i, j].explored = false;
                    if (map[i, j].symbol == "1")
                    {
                        map[i, j].symbol = "■";
                        map[i, j].value = 0;

                    }
                }
            }
            return map;
        }

        public Tile[,] findShortestPath(Tile[,] map)
        {
            if (back.Count == 0)
            {
                return map;
            }
            else
            {
                while (back.Count > 0)
                {
                    Tile tile = back.Dequeue();
                    if (tile.value == 1 && tile.symbol == " ")
                    {
                        neighbor.Clear();
                        getNeighbors(map, tile);
                        if (neighbor.Count < 2)
                        {
                            tile.value = 0;
                            tile.symbol = "■";
                            map = clearMap(map);
                        }
                    }
                    else
                    {
                        return findShortestPath(clearMap(map));
                    }
                }
                return map;
            }
        }

        public Tile[,] displayShortestPath(Tile[,] map)
        {
            foreach (Tile tile in map)
            {
                if (tile.value == 1 && tile.symbol == " ")
                {
                    tile.symbol = "*";
                }
            }
            return map;
        }

        public static void printMap(Tile[,] map)
        {
            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    if (map[i, j].value == 2)
                    {
                        map[i, j].symbol = "*";
                    }
                    Console.Write(map[i, j].symbol + " ");
                }
                Console.WriteLine();
            }
        }

        public static void Main(string[] args)
        {
            Program program = new Program();
            int startX = 0;
            int startY = 0;
            Tile[,] map = new Tile[6, 7];
            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    map[i, j] = new Tile();
                    map[i, j].x = j;
                    map[i, j].y = i;
                }
            }
            map = setMap(map, map.GetLength(0), map.GetLength(1));
            printMap(map);
            Console.WriteLine("\n\n");

            foreach (Tile tile in map)
            {
                if (tile.value == 2)
                {
                    startX = tile.x;
                    startY = tile.y;
                }
            }

            map = program.solve(map, map[startY, startX]);
            printMap(map);
            Console.WriteLine("\n\n");

            program.clearMap(map);
            printMap(map);
            Console.WriteLine("\n\n");

            map = program.findShortestPath(map);
            printMap(map);
            Console.WriteLine("\n\n");

            map = program.displayShortestPath(map);
            printMap(map);
            Console.WriteLine("\n\n");

            Console.ReadLine();
        }
    }
}