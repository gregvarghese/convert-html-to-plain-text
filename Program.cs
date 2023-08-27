using ReadSharp;

Console.BackgroundColor = ConsoleColor.Blue;
Console.ForegroundColor = ConsoleColor.White;

WriteFullLine("HTML to Text Converter");
WriteFullLine("By: Gregory Varghese");
WriteFullLine("https://www.gregoryvarghese.com");
WriteFullLine("-------------");
WriteFullLine("This program will convert all HTML files in a folder to text files.");
WriteFullLine("It will also convert a single HTML file to a text file.");
WriteFullLine("It will also replace line endings with 2 line breaks.");



WriteFullLine("Enter folder path or file name:");
var sPath = Console.ReadLine();

var replaceLineEndings = "";
var bReplaceLineEndings = true;
var bAlwaysOverwrite = true;


do
{
    WriteFullLine("Replace Line Endings with 2 Line Breaks? (y/n)");
    replaceLineEndings = Console.ReadLine();

    switch (replaceLineEndings)
    {
        case "y":
            bReplaceLineEndings = true;
            break;
        case "n":
            bReplaceLineEndings = false;
            break;
        default:
            WriteFullLine("Invalid input. Please enter y or n.");
            replaceLineEndings = "";
            break;
    }
} while (replaceLineEndings == "");


if (File.Exists(sPath))
{
    WriteFile(sPath, bReplaceLineEndings);
    return;
}


if (!Directory.Exists(sPath))
{
    WriteFullLine("Folder path does not exist.");
    return;
}

var files = Directory.EnumerateFiles(sPath, "*.*", SearchOption.AllDirectories).Where(s => s.EndsWith(".html") || s.EndsWith(".htm"));

WriteFullLine($"Found {files.Count()} files.");

foreach (var file in files) WriteFile(file, bReplaceLineEndings);

void WriteFile(string filename, bool ReplaceLineEndings)
{
    var html = File.ReadAllText(filename);
    var cleanHtml = HtmlUtilities.ConvertToPlainText(html);
    var fileName = Path.GetFileNameWithoutExtension(filename);
    var newFilePath = Path.Combine(Path.GetDirectoryName(filename), fileName + ".txt");
    if (ReplaceLineEndings) cleanHtml = cleanHtml.Replace("\n", "\n\n");


    if (File.Exists(newFilePath) && !bAlwaysOverwrite)
    {
        WriteFullLine($"File {newFilePath} already exists. Overwrite? (y/n/a)");
        var sOverwrite = Console.ReadLine();
        switch (sOverwrite)
        {
            case "y":
                File.WriteAllText(newFilePath, cleanHtml);
                Console.WriteLine($"File {newFilePath} written.");
                break;
            case "n":
                Console.WriteLine($"File {newFilePath} not written.");
                break;
            case "a":
                bAlwaysOverwrite = true;
                File.WriteAllText(newFilePath, cleanHtml);
                Console.WriteLine($"File {newFilePath} written.");
                break;
            default:
                Console.WriteLine("Invalid input. Please enter y, n, or a.");
                sOverwrite = "";
                break;
        }
    }
    else
    {
        File.WriteAllText(newFilePath, cleanHtml);
        Console.WriteLine($"File {newFilePath} written.");
    }
}

static void WriteFullLine(string value)
{
    Console.WriteLine(value.PadRight(Console.WindowWidth - 1)); // <-- see note
}