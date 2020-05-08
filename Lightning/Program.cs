using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Colorful;
using Lightning_Data;
using Console = Colorful.Console;

namespace Lightning
{
    class Program
    {

        static string environmentpath;
        static string os;
        static void Main(string[] args)
        {

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                os = "Microsoft Windows";
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                os = "OSX";
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                os = "Linux";
            else
                throw new Exception("Not running on a recognized operating system!");

            if (args.Length > 0)
            {
                string envpath = args[0];
                if (Directory.Exists(envpath))
                    environmentpath = envpath;
                else
                    throw new Exception("Not a valid path!");
            }
            else
            {
                environmentpath = Environment.CurrentDirectory;
            }
            Lightning(environmentpath);
        }

        public static void Lightning(string environmentpath)
        {

            string[] HLP = 
            {
                "help                                  Display help.",
                "ld                                    Display all items contained inside the current folder.",
                "ld ? <DirectoryName>                  Display all items contained inside the folder specified.",
                "clear                                 Clears the current terminal.",
                "echo:<Message>                        Prints the message on the screen.",
                "cd <DirectoryName>                    Changes the current directory to the specified one. '.' is the same directory and '..' is parent directory.",
                "colorf <Microsoft .NET Color Name>    Changes the foreground to the entered Microsoft .NET color name.",
                "colorb <Microsoft .NET Color Name>    Changes the background to the entered Microsoft .NET color name.",
                "resetcolor                            Resets the terminal's color.",
                "new                                   Resets the Lightning Terminal.",
                "exit                                  Exits the current Lightning Terminal.",
                "in keyboard(:<Message> Optional)      Inputs the default variable IN to the input entered by the user. You can also define it by using (:).",
                "in key(:<Key> Optional)               Inputs the default variable IN to the key pressed by the user. You can also define it by using (:).",
                "If the command entered is not defined, it will search to execute a file named so. To execute a file in the current directory, use .\\ at start."
            };

            Console.ResetColor();
            Console.Clear();
            Console.ForegroundColor = Color.DeepSkyBlue;
            Console.WriteLine(@"  _      _       _     _         _             ");
            Console.WriteLine(@" | |    (_)     | |   | |       (_)            ");
            Console.WriteLine(@" | |     _  __ _| |__ | |_ _ __  _ _ __   __ _ ");
            Console.WriteLine(@" | |    | |/ _` | '_ \| __| '_ \| | '_ \ / _` |");
            Console.WriteLine(@" | |____| | (_| | | | | |_| | | | | | | | (_| |");
            Console.WriteLine(@" |______|_|\__, |_| |_|\__|_| |_|_|_| |_|\__, |");
            Console.WriteLine(@"            __/ |                         __/ |");
            Console.WriteLine(@"           |___/                         |___/ ");
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine(@"                --------------");
            Console.WriteLine(@"                |Version 0.01|");
            Console.WriteLine(@"                --------------");
            Console.ResetColor();
            Console.WriteLine("Welcome to the Lightning Terminal!\n");
            Console.WriteLine($"Running on {os}, Path:'{@environmentpath}'");
            Console.ResetColor();
            Color OLDforeground = Data.INPUT_COLOR;
            Color OLDbackground = Console.BackgroundColor;
            Color newforeground = Data.INPUT_COLOR;
            Color newbackground = Console.BackgroundColor;

            object in_ = null;
            while (true)
            {
                Console.ResetColor();
                Console.BackgroundColor = newbackground;
                Console.Title = @environmentpath + "$";
                Console.ForegroundColor = Data.PATH_COLOR;
                Console.Write($"[{@environmentpath}]");
                Console.ForegroundColor = Color.Yellow;
                Console.Write("$ ");
                Console.ForegroundColor = newforeground;
                string input = Console.ReadLine();
                input = input.TrimStart();
                input.ToLower();
                Console.Title += input;
                Console.ResetColor();
                if (input.StartsWith("clear"))
                {
                    Console.Clear();
                }
                else if (input.StartsWith("exit"))
                {
                    return;
                }
                else if (input.StartsWith("help"))
                {
                    Color previous = Console.ForegroundColor;
                    Console.WriteLine("Help for Lightning:");
                    Console.ForegroundColor = Color.Yellow;
                    foreach (string help in HLP)
                    {
                        Console.WriteLine();
                        Console.WriteLine(help);
                    }
                    Console.ForegroundColor = previous;
                }
                else if (input.StartsWith("echo"))
                {
                    if (input.Contains(':'))
                    {
                        int pointer = input.IndexOf(':') + 1;
                        string toWrite = input.Substring(pointer);
                        Console.WriteLine(toWrite);
                    }
                    else if (input.Contains("user"))
                    {
                        Console.WriteLine(in_);
                    }
                    else
                    {
                        Color previous = Console.ForegroundColor;
                        Console.ForegroundColor = Data.ERROR_COLOR;
                        Console.WriteLine($"Incorrect usage for echo.\n(Full String):'{input}'");
                        Console.ForegroundColor = previous;
                    }
                }
                else if (input.StartsWith("scvg-dumpbin"))
                {
                    int pointer = input.IndexOf('n') + 2;
                    int pointer2 = input.IndexOf(',');
                    string filename = input.Substring(pointer, pointer2 - pointer);
                    if (File.Exists(filename))
                    {
                        byte[] file = File.ReadAllBytes(filename);
                        string dumpFile = input.Substring(pointer2 + 2);
                        File.WriteAllBytes(dumpFile, file);
                    }
                    else
                        Console.WriteLine("Scavenger: No file found!");
                }
                else if (input.StartsWith("scvg-dumpfile"))
                {
                    int pointer = input.IndexOf('e') + 2;
                    int pointer2 = input.IndexOf(',');
                    string filename = input.Substring(pointer, pointer2 - pointer);
                    if (File.Exists(filename))
                    {
                        byte[] file = File.ReadAllBytes(filename);
                        string dumpFile = input.Substring(pointer2 + 2);
                        List<string> output = new List<string>();
                        foreach (byte b in file)
                        {
                            output.Add(Convert.ToString(b));
                        }
                        File.WriteAllLines(dumpFile, output.ToArray());
                    }
                    else
                        Console.WriteLine("Scavenger: No file found!");
                }
                else if (input.StartsWith("scvg"))
                {
                    int pointer = input.IndexOf('g') + 2;
                    string filename = input.Substring(pointer);
                    if (File.Exists(filename))
                    {
                        byte[] file = File.ReadAllBytes(filename);
                        foreach (byte b in file)
                        {
                            Console.Write(b + "\t");
                        }
                        Console.WriteLine();
                    }
                    else
                        Console.WriteLine("Scavenger: No file found!");
                }
                else if (input.StartsWith("in"))
                {
                    int pointer = input.IndexOf('n') + 2;
                    string item = input.Substring(pointer);
                    item.TrimStart();
                    item.TrimEnd();
                    item.ToLower();
                    if (item.StartsWith("keyboard"))
                    {
                        if (item.Contains(":"))
                        {
                            int pointer2 = item.IndexOf(':') + 1;
                            in_ = item.Substring(pointer2);
                        }
                        else
                            in_ = Console.ReadLine();
                    }
                    else if (item.StartsWith("key"))
                    {
                        if (item.Contains(":"))
                        {
                            int pointer2 = item.IndexOf(':') + 1;
                            in_ = item[pointer2];
                        }
                        else
                            in_ = Console.ReadKey().KeyChar;
                    }
                }
                else if (input.StartsWith("cd") && !input.Contains("cdir"))
                {
                    int pointer = input.IndexOf('d') + 2;
                    try
                    {
                        string dir = @input.Substring(pointer) + @"\";

                        if (dir == ".." || dir == @"..\")
                        {
                            try
                            {
                                environmentpath = Directory.GetParent(environmentpath).FullName;
                            }
                            catch
                            {
                                Console.WriteLine($"No parent directory for directory '{environmentpath}'!");
                            }

                        }
                        else if (dir == "." || dir == @".\")
                        {
                        }
                        else if (dir == "home")
                        {
                            environmentpath = Environment.CurrentDirectory;
                        }
                        else
                        {
                            if (Directory.Exists(dir))
                                environmentpath = dir;
                            else if (Directory.Exists(environmentpath + @"\" + dir))
                            {
                                if (dir.EndsWith('\\'))
                                {
                                    environmentpath += @$"\{dir}";
                                }
                                else
                                {
                                    environmentpath += $@"\{dir}\";
                                }
                            }
                            else
                            {
                                Color previous = Console.ForegroundColor;
                                Console.ForegroundColor = Data.ERROR_COLOR;
                                Console.WriteLine($"Incorrect usage for cd. (Full command):'{input}'");
                                Console.ForegroundColor = previous;
                            }
                        }
                    }
                    catch
                    {

                    }
                    
                }
                else if (input.StartsWith("cfile"))
                {
                    int pointer = input.IndexOf('e') + 2;
                    string filepath = @environmentpath + @input.Substring(pointer);
                    if (File.Exists(@filepath))
                    {
                        Console.WriteLine("File already existing. Press enter to overwrite.");
                        if (Console.ReadKey().Key == ConsoleKey.Enter)
                        {
                            
                        }
                        else
                        {
                            Console.WriteLine("File creation aborted.");
                            continue;
                        }
                    }
                    File.Create(@filepath);
                    Console.WriteLine($"Created file '{@filepath}'.");
                }
                else if (input.StartsWith("dfile"))
                {
                    int pointer = input.IndexOf('e') + 2;
                    string filepath = @environmentpath + @input.Substring(pointer);
                    if (File.Exists(@filepath))
                    {
                        File.Delete(@filepath);
                        Console.WriteLine($"Deleted file '{@filepath}'");
                    }
                    else
                        Console.WriteLine("File not existing!");
                }
                else if (input.StartsWith("cdir"))
                {
                    int pointer = input.IndexOf('r') + 2;
                    string filepath = @environmentpath + @"\" + @input.Substring(pointer);
                    if (Directory.Exists(@filepath))
                    {
                        Console.WriteLine("Directory already existing. Press enter to overwrite.");
                        if (Console.ReadKey().Key == ConsoleKey.Enter)
                        {

                        }
                        else
                        {
                            Console.WriteLine("Directory creation aborted.");
                            continue;
                        }
                    }
                    Directory.CreateDirectory(@filepath);
                    Console.WriteLine($"Created directory '{@filepath}'.");
                }
                else if (input.StartsWith("ddir"))
                {
                    int pointer = input.IndexOf('r') + 2;
                    string filepath = @environmentpath + @input.Substring(pointer);
                    if (Directory.Exists(@filepath))
                    {
                        Directory.Delete(@filepath);
                        Console.WriteLine($"Deleted Directory '{@filepath}'");
                    }
                    else
                        Console.WriteLine("Directory not existing!");
                }
                else if (input.StartsWith("colorf"))
                {
                    int pointer = input.IndexOf('f') + 2;
                    string color = input.Substring(pointer);
                    Color foreground = Color.FromName(color);
                    newforeground = foreground;
                }
                else if (input.StartsWith("colorb"))
                {
                    int pointer = input.IndexOf('b') + 2;
                    string color = input.Substring(pointer);
                    Color background = Color.FromName(color);
                    newbackground = background;
                }
                else if (input.StartsWith("resetcolor"))
                {
                    newforeground = OLDforeground;
                    newbackground = OLDbackground;
                }
                else if (input.StartsWith("ld"))
                {
                    try
                    {
                        if (input.Contains('?'))
                        {
                            int pointer = input.IndexOf('?') + 2;
                            string path = input.Substring(pointer);
                            Console.WriteLine($"Listing the Directory '{path}':");
                            Color previous = Console.ForegroundColor;
                            Console.ForegroundColor = Color.DarkTurquoise;
                            int items = 0;
                            foreach (string file in Directory.EnumerateFiles(path))
                            {
                                Console.WriteLine(file + " [File]");
                                items++;
                            }
                            foreach (string dir in Directory.EnumerateDirectories(path))
                            {
                                Console.WriteLine(dir + " [Directory]");
                                items++;
                            }
                            Console.Write($"Folder length:{items}");
                            Console.WriteLine();
                            Console.ForegroundColor = previous;
                        }
                        else
                        {
                            Console.WriteLine($"Listing the Directory '{environmentpath}':");
                            Color previous = Console.ForegroundColor;
                            Console.ForegroundColor = Color.DarkTurquoise;
                            int items = 0;
                            foreach (string file in Directory.EnumerateFiles(environmentpath))
                            {
                                Console.WriteLine(file + " [File]");
                                items++;
                            }
                            foreach (string dir in Directory.EnumerateDirectories(environmentpath))
                            {
                                Console.WriteLine(dir + " [Directory]");
                                items++;
                            }
                            Console.Write($"This folder contains {items} items.");
                            Console.WriteLine();
                            Console.ForegroundColor = previous;
                        }
                    }
                    catch
                    {
                        Color previous = Console.ForegroundColor;
                        Console.ForegroundColor = Color.Red;
                        Console.WriteLine("Cannot find directory!");
                        Console.ForegroundColor = previous;
                    }
                    
                }
                else if (input.StartsWith("new"))
                {
                    Lightning(environmentpath);
                    break;
                }
                else if (string.IsNullOrWhiteSpace(input))
                {

                }
                else
                {
                    try
                    {
                        if (input.Contains("-"))
                        {
                            int pointer = input.IndexOf('-') + 1;
                            string args = @input.Substring(pointer);
                            string appname = @input.Substring(0, pointer - 2);
                            //Process.Start(appname, args);
                            Process.Start(appname, args);
                            Console.WriteLine($"Started '{appname}' with arguments '{args}'.");
                            Console.WriteLine("Press any key to end...");
                            Console.ReadKey();
                        }
                        else
                        {
                            Process.Start(@input);
                            Console.WriteLine($"Started '{input}'.");
                            Console.WriteLine("Press any key to end...");
                            Console.ReadKey();
                        }
                    }
                    catch
                    {
                        Color previous = Console.ForegroundColor;
                        Console.ForegroundColor = Data.ERROR_COLOR;
                        Console.WriteLine($"'{input}' is not recognized as a command, script file, applet or runnable application.");
                        Console.ForegroundColor = previous;
                    }
                }
            }
        }
    }
}
