// See https://aka.ms/new-console-template for more information

using Dojo_Playfair;

var cipher = new Cipher("TURINGSMELABCDFHKOP VWXYZ");

Console.WriteLine("Enter string to be decoded:");
var encodedString = Console.ReadLine();

Console.WriteLine(cipher.Decode(encodedString));