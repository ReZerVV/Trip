namespace Trip.ConsoleApp
{
    public static class ConsoleWriter
    {
        public static void Info(string message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("info\t");
            Console.ResetColor();
            for (var messageIndex = 0; messageIndex < message.Length; messageIndex++)
            {
                if ((messageIndex + 8) % Console.WindowWidth == 0)
                {
                    Console.Write("\n\t");
                }
                Console.Write(message[messageIndex]);
            }
            Console.Write("\n");
        }

        public static void Warn(string message)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("warn\t");
            Console.ResetColor();
            for (var messageIndex = 0; messageIndex < message.Length; messageIndex++)
            {
                if ((messageIndex + 8) % Console.WindowWidth == 0)
                {
                    Console.Write("\n\t");
                }
                Console.Write(message[messageIndex]);
            }
            Console.Write("\n");
        }

        public static void Error(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("error\t");
            Console.ResetColor();
            for (var messageIndex = 0; messageIndex < message.Length; messageIndex++) 
            {
                if ((messageIndex + 8) % Console.WindowWidth == 0) 
                {
                    Console.Write("\n\t");
                }
                Console.Write(message[messageIndex]);
            }
            Console.Write("\n");
        }

        public static void Table(string[] headers, string[,] data)
        {
            int numRows = data.GetLength(0);
            int numCols = headers.Length;

            // Обчислюємо ширину кожного стовпця на основі найбільшої довжини даних і заголовків
            int[] colWidths = new int[numCols];
            for (int col = 0; col < numCols; col++)
            {
                colWidths[col] = headers[col].Length;
                for (int row = 0; row < numRows; row++)
                {
                    int dataLength = data[row, col].Length;
                    if (dataLength > colWidths[col])
                    {
                        colWidths[col] = dataLength;
                    }
                }
            }

            // Виводимо верхню лінію таблиці
            Console.Write("+");
            for (int col = 0; col < numCols; col++)
            {
                Console.Write(new string('-', colWidths[col] + 2) + "+");
            }
            Console.WriteLine();

            // Виводимо заголовки
            Console.Write("|");
            for (int col = 0; col < numCols; col++)
            {
                Console.Write($" {headers[col].PadRight(colWidths[col])} |");
            }
            Console.WriteLine();

            // Виводимо роздільну лінію між заголовками та даними
            Console.Write("+");
            for (int col = 0; col < numCols; col++)
            {
                Console.Write(new string('-', colWidths[col] + 2) + "+");
            }
            Console.WriteLine();

            // Виводимо дані
            for (int row = 0; row < numRows; row++)
            {
                Console.Write("|");
                for (int col = 0; col < numCols; col++)
                {
                    Console.Write($" {data[row, col].PadRight(colWidths[col])} |");
                }
                Console.WriteLine();
            }

            // Виводимо нижню лінію таблиці
            Console.Write("+");
            for (int col = 0; col < numCols; col++)
            {
                Console.Write(new string('-', colWidths[col] + 2) + "+");
            }
            Console.WriteLine();
        }

        public static void Table(string[] headers, string[][] data)
        {
            int numRows = data.GetLength(0);
            int numCols = headers.Length;

            // Обчислюємо ширину кожного стовпця на основі найбільшої довжини даних і заголовків
            int[] colWidths = new int[numCols];
            for (int col = 0; col < numCols; col++)
            {
                colWidths[col] = headers[col].Length;
                for (int row = 0; row < numRows; row++)
                {
                    int dataLength = data[row][col].Length;
                    if (dataLength > colWidths[col])
                    {
                        colWidths[col] = dataLength;
                    }
                }
            }

            // Виводимо верхню лінію таблиці
            Console.Write("+");
            for (int col = 0; col < numCols; col++)
            {
                Console.Write(new string('-', colWidths[col] + 2) + "+");
            }
            Console.WriteLine();

            // Виводимо заголовки
            Console.Write("|");
            for (int col = 0; col < numCols; col++)
            {
                Console.Write($" {headers[col].PadRight(colWidths[col])} |");
            }
            Console.WriteLine();

            // Виводимо роздільну лінію між заголовками та даними
            Console.Write("+");
            for (int col = 0; col < numCols; col++)
            {
                Console.Write(new string('-', colWidths[col] + 2) + "+");
            }
            Console.WriteLine();

            // Виводимо дані
            for (int row = 0; row < numRows; row++)
            {
                Console.Write("|");
                for (int col = 0; col < numCols; col++)
                {
                    Console.Write($" {data[row][col].PadRight(colWidths[col])} |");
                }
                Console.WriteLine();
            }

            // Виводимо нижню лінію таблиці
            Console.Write("+");
            for (int col = 0; col < numCols; col++)
            {
                Console.Write(new string('-', colWidths[col] + 2) + "+");
            }
            Console.WriteLine();
        }
    }
}
