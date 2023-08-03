using System.Text;

namespace Dojo_Playfair;

public class Cipher
{
    private string[] CipherArray { get; } 
    
    public Cipher (string cipherText)
    {
        if (cipherText.Length != 25)
        {
            throw new ArgumentException("Invalid Cipher");
        }

        CipherArray = new string[5];

        for (var i = 0; i < CipherArray.Length; i++)
        {
            CipherArray[i] = cipherText.Substring(5 * i, 5);
        }
    }

    public string Decode(string encodedString)
    {
        var sb = new StringBuilder("");
        
        for (var i = 0; i < encodedString.Length; i+=2)
        {
            if (i + 1 >= encodedString.Length)
            {
                continue;
            }
            
            var decodedPair = DecodePair(encodedString[i], encodedString[i + 1]);
            sb.Append(decodedPair);
        }

        var decodedString = sb.ToString();

        return CleanUpX(decodedString);
    }

    private string DecodePair(char letter1, char letter2)
    {
        var encodedLetter1 = LocateLetter(letter1);
        var encodedLetter2 = LocateLetter(letter2);

        char decodedLetter1;
        char decodedLetter2;

        if (encodedLetter1.Y == encodedLetter2.Y) // Same Row
        {
            if (encodedLetter1.X == 0)
                encodedLetter1.X = CipherArray.Length;
            if (encodedLetter2.X == 0)
                encodedLetter2.X = CipherArray.Length;
            
            decodedLetter1 = CipherArray[encodedLetter2.Y][encodedLetter2.X - 1];
            decodedLetter2 = CipherArray[encodedLetter1.Y][encodedLetter1.X - 1];
        }
        else if (encodedLetter1.X == encodedLetter2.X) // Same Column
        {
            if (encodedLetter1.Y == CipherArray.Length - 1)
                encodedLetter1.Y = -1;
            if (encodedLetter2.Y == CipherArray.Length - 1)
                encodedLetter2.Y = -1;
            
            decodedLetter1 = CipherArray[encodedLetter1.Y + 1][encodedLetter1.X];
            decodedLetter2 = CipherArray[encodedLetter2.Y + 1][encodedLetter2.X];
        }
        else // Rectangle
        {
            var decodedCorner1 = (encodedLetter1.X, encodedLetter2.Y);
            var decodedCorner2 = (encodedLetter2.X, encodedLetter1.Y);
        
            decodedLetter1 = CipherArray[decodedCorner1.Y][decodedCorner1.X];
            decodedLetter2 = CipherArray[decodedCorner2.Y][decodedCorner2.X];

        }
            
        return char.ToString(decodedLetter2) + char.ToString(decodedLetter1);
    }

    private (int X, int Y) LocateLetter(char letter)
    {
        for (var y = 0; y < CipherArray.Length; y++)
        {
            var row = CipherArray[y];
            
            for (var x = 0; x < row.Length; x++)
            {
                var character = row[x];
                if (character == letter)
                {
                    return (x, y);
                }
            }
        }

        return (-1, -1);
    }

    private string CleanUpX(string decodedStringRaw)
    {
        var indicesToRemove = new List<int>();
        for (int i = 0; i < decodedStringRaw.Length; i++)
        {
            if (i - 1 <= 0 || i + 1 >= decodedStringRaw.Length)
                continue;

            if (decodedStringRaw[i] == 'X' &&
                decodedStringRaw[i - 1] == decodedStringRaw[i + 1])
            {
                indicesToRemove.Add(i);
            }
        }
        
        if(decodedStringRaw[^1] == 'X')
            indicesToRemove.Add(decodedStringRaw.Length - 1);

        for (var i = 0; i < indicesToRemove.Count; i++)
        {
            var indexToRemove = indicesToRemove[i] - i;
            decodedStringRaw = decodedStringRaw.Remove(indexToRemove, 1);
        }

        return decodedStringRaw;
    }
}