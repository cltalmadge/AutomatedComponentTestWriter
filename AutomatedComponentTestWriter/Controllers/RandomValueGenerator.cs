using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AutomatedComponentTestWriter.Controllers
{
    public class RandomValueGenerator
    {
        Random random = new Random();
        public string CreateRandomDecimal(int valueLength)
        {
            double randomValue = random.NextDouble();

            // A list of allowed characters.
            string allowed = "0123456789";

            // We want an integer of a specific "length."
            if (valueLength == 0)
            {
                valueLength = random.Next(0, 29); // If no value length specified, just create a random integer.
            }

            if (valueLength > 29)
            {
                valueLength = 29; // To avoid creating integers larger than int32 can store.
            }

            var stringChars = new char[valueLength];

            for (int i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = allowed[random.Next(allowed.Length)];
            }

            // Pick a random spot in the array of characters to replace with a period/dot.
            int randomIndex = random.Next(stringChars.Length - 1);

            // Don't let it pick the last index of the decimal for dot placement to prevent it from generating buggy code.
            if (randomIndex == stringChars.Length)
                randomIndex = stringChars.Length - 1;

            // Just picks a random index of the decimal number and replaces it with a period.
            stringChars[randomIndex] = '.';

            string decimalString = new string(stringChars);
            return decimalString;
        }

        public string CreateRandomString(int valueLength)
        {
            // A list of allowed characters.
            if (valueLength == 0)
            {
                // Just an arbitrary placeholder to deal with empty value length fields.
                valueLength = random.Next(0, 30);
            }

            string allowed = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var stringChars = new char[valueLength];


            for (int i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = allowed[random.Next(allowed.Length)];
            }

            return new string(stringChars);
        }

        public int CreateRandomIntegerOfLength(int valueLength)
        {
            // A list of allowed characters.
            string allowed = "0123456789";

            // We want an integer of a specific "length."
            if (valueLength == 0)
            {
                return random.Next(); // If no value length specified, just create a random integer.
            }

            if (valueLength > 9)
            {
                valueLength = 9; // To avoid creating integers larger than int32 can store.
            }

            var stringChars = new char[valueLength];

            for (int i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = allowed[random.Next(allowed.Length)];
            }

            return int.Parse(new string(stringChars));
        }

        public string CreateRandomDate()
        {
            DateTime start = new DateTime(1995, 1, 1);

            int range = (DateTime.Today - start).Days;

            return start.AddDays(random.Next(range)).ToString();
        }
    }
}