using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;

class Program
{
    static void Main(string[] args)
    {
       
        Console.OutputEncoding = Encoding.UTF8;

        string filePath = "..\\..\\..\\book.txt"; 
        

        string text = File.ReadAllText(filePath);

        Console.WriteLine("Starting Non-Threaded Version...");
        Stopwatch stopwatch = Stopwatch.StartNew();

        RunNonThreadedVersion(text);

        stopwatch.Stop();
        Console.WriteLine($"Non-Threaded Version Execution Time: {stopwatch.ElapsedMilliseconds} ms\n");

        Console.WriteLine("Starting Multi-Threaded Version...");
        stopwatch.Restart();

        RunMultiThreadedVersion(text);

        stopwatch.Stop();
        Console.WriteLine($"Multi-Threaded Version Execution Time: {stopwatch.ElapsedMilliseconds} ms");
    }

   
    static void RunNonThreadedVersion(string text)
    {
        List<string> words = ExtractWords(text);

        int wordCount = words.Count;
        string shortestWord = FindShortestWord(words);
        string longestWord = FindLongestWord(words);
        double averageLength = CalculateAverageWordLength(words);
        var mostCommonWords = FindMostCommonWords(words, 5);
        var leastCommonWords = FindLeastCommonWords(words, 5);

        PrintResults(wordCount, shortestWord, longestWord, averageLength, mostCommonWords, leastCommonWords);
    }

    
    static void RunMultiThreadedVersion(string text)
    {
        List<string> words = ExtractWords(text);

        int wordCount = 0;
        string shortestWord = "";
        string longestWord = "";
        double averageLength = 0;
        Dictionary<string, int> mostCommonWords = null;
        Dictionary<string, int> leastCommonWords = null;

        Thread t1 = new Thread(() => wordCount = words.Count);
        Thread t2 = new Thread(() => shortestWord = FindShortestWord(words));
        Thread t3 = new Thread(() => longestWord = FindLongestWord(words));
        Thread t4 = new Thread(() => averageLength = CalculateAverageWordLength(words));
        Thread t5 = new Thread(() => mostCommonWords = FindMostCommonWords(words, 5));
        Thread t6 = new Thread(() => leastCommonWords = FindLeastCommonWords(words, 5));

        t1.Start();
        t2.Start();
        t3.Start();
        t4.Start();
        t5.Start();
        t6.Start();

        t1.Join();
        t2.Join();
        t3.Join();
        t4.Join();
        t5.Join();
        t6.Join();

        PrintResults(wordCount, shortestWord, longestWord, averageLength, mostCommonWords, leastCommonWords);
    }

    
    static List<string> ExtractWords(string text)
    {
        var words = new List<string>();
        string currentWord = "";
        foreach (char c in text)
        {
            
            if (char.IsLetter(c))
            {
                currentWord += c;
            }
            else
            {
                if (currentWord.Length >= 3) words.Add(currentWord.ToLower());
                currentWord = "";
            }
        }
        if (currentWord.Length >= 3) words.Add(currentWord.ToLower());
        return words;
    }

    static string FindShortestWord(List<string> words)
    {
        string shortest = words[0];
        foreach (var word in words)
        {
            if (word.Length < shortest.Length) { 
                shortest = word; 
            }
        }
        return shortest;
    }

    static string FindLongestWord(List<string> words)
    {
        string longest = words[0];
        foreach (var word in words)
        {
            if (word.Length > longest.Length) {
                longest = word;
            }
        }
        return longest;
    }

    static double CalculateAverageWordLength(List<string> words)
    {
        double totalLength = 0;
        foreach (var word in words) totalLength += word.Length;
        return totalLength / words.Count;
    }

    static Dictionary<string, int> FindMostCommonWords(List<string> words, int count)
    {
        var wordFrequency = new Dictionary<string, int>();
        foreach (var word in words)
        {
            if (wordFrequency.ContainsKey(word))
            {
                wordFrequency[word]++;
            }
            else
            {
                wordFrequency[word] = 1;
            }
        }

        var sortedWords = new List<KeyValuePair<string, int>>(wordFrequency);
        sortedWords.Sort((x, y) => y.Value.CompareTo(x.Value));
        return sortedWords.GetRange(0, count).ToDictionary(k => k.Key, v => v.Value);
    }

    static Dictionary<string, int> FindLeastCommonWords(List<string> words, int count)
    {
        var wordFrequency = new Dictionary<string, int>();
        foreach (var word in words)
        {
            if (wordFrequency.ContainsKey(word))
            {
                wordFrequency[word]++;
            }
            else
            {
                wordFrequency[word] = 1;
            }
        }

        var sortedWords = new List<KeyValuePair<string, int>>(wordFrequency);
        sortedWords.Sort((x, y) => x.Value.CompareTo(y.Value));
        return sortedWords.GetRange(0, count).ToDictionary(k => k.Key, v => v.Value);
    }

    static void PrintResults(int wordCount, string shortestWord, string longestWord, double averageLength,
                             Dictionary<string, int> mostCommonWords, Dictionary<string, int> leastCommonWords)
    {
        Console.WriteLine($"Number of words: {wordCount}");
        Console.WriteLine($"Shortest word: {shortestWord}");
        Console.WriteLine($"Longest word: {longestWord}");
        Console.WriteLine($"Average word length: {averageLength:F2}");
        Console.WriteLine("Five most common words:");
        foreach (var word in mostCommonWords) Console.WriteLine($"{word.Key}: {word.Value}");
        Console.WriteLine("Five least common words:");
        foreach (var word in leastCommonWords) Console.WriteLine($"{word.Key}: {word.Value}");
    }
}
