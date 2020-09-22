using System;
using System.Collections.Generic;
using System.Linq;

namespace BullsAndCows
{
    static class Program
    {
        static readonly Random RandomGenerator = new Random();

        /// <summary>
        ///     Генерирует и возвращает указанное количество неповторяющихся цифр
        /// </summary>
        /// <param name="length">Количество цифр. Не может превышать 9</param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     Выбрасывается если невозможно сгенерировать требуемое количество неповторяющихся цифр
        /// </exception>
        static int[] GetRandomDigits(int length)
        {
            var availableDigits = new List<int> {0, 1, 2, 3, 4, 5, 6, 7, 8, 9};

            if (length < 0 || length > availableDigits.Count)
            {
                throw new ArgumentOutOfRangeException(nameof(length));
            }

            var result = new int[length];
            for (var i = 0; i < length; i++)
            {
                var digitIndex = RandomGenerator.Next(0, availableDigits.Count);
                result[i] = availableDigits[digitIndex];

                // Remove chosen digit from pool, so it won't be selected again:
                availableDigits.RemoveAt(digitIndex);
            }

            return result;
        }

        /// <summary>
        /// Выделяет из переданной строки требуемое количество цифр.
        /// </summary>
        /// <returns>
        /// Возвращает ровно <paramref name="length"/> выделенных из строки цифр,
        /// но только если строка не содержит ничего кроме необходимого числа цифр.
        /// Иначе возвращает null: если в строке есть лишние симфолы или их не хватает
        /// </returns>
        static int[] SplitNumber(string str, int length)
        {
            if (str.Length != length)
            {
                return null;
            }

            var result = new int[length];
            for (var i = 0; i < length; i++)
            {
                var character = str[i].ToString();
                if (int.TryParse(character, out var digit))
                {
                    result[i] = digit;
                }
                else
                {
                    return null;
                }
            }

            return result;
        }

        /// <summary>
        /// Запрашивает у пользователя требуемое количество цифр до тех пор пока он не введет корректное число.
        /// </summary>
        static int[] AskUser(int length)
        {
            while (true)
            {
                Console.Write("Ваша догадка: ");
                var input = Console.ReadLine();

                // Documentation says that Console.ReadLine() may return null.
                if (input != null)
                {
                    // Trim whitespaces. User should not worry about few invisible characters.
                    input = input.Trim();

                    var digits = SplitNumber(input, length);
                    if (digits != null)
                    {
                        return digits;
                    }
                }

                Console.WriteLine("    Введите корректное число.");
            }
        }

        /// <summary>
        /// Играет ровно один раунд с пользователем до тех пор, пока он не победит.
        /// </summary>
        /// <param name="length">Длина загаданного числа</param>
        static void PlaySingleGame(int length)
        {
            var number = GetRandomDigits(length);
            while (true)
            {
                var guess = AskUser(length);

                var correct = 0;
                var misplaced = 0;
                for (var i = 0; i < length; i++)
                {
                    var digit = guess[i];
                    if (digit == number[i])
                    {
                        correct += 1;
                    }
                    else if (number.Contains(digit))
                    {
                        misplaced += 1;
                    }
                }

                if (correct != length)
                {
                    Console.WriteLine($"Быков: {correct}");
                    Console.WriteLine($"Коров: {misplaced}");
                }
                else
                {
                    Console.WriteLine("Все быки на месте!");
                    return;
                }
            }
        }

        static void Main()
        {
            const int length = 4;
            string choice;
            do
            {
                PlaySingleGame(length);

                Console.Write("Введите 'y' чтобы сыграть ещё раз");
                choice = Console.ReadLine();
            } while (choice == "y");
        }
    }
}