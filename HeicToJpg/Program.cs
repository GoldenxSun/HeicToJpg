using ImageMagick;

Console.Write("Source Path: ");

string? path = Console.ReadLine();

Console.Write("Destination Path: ");

string? destination = Console.ReadLine();

if (path != null)
{
    try
    {
        Console.Clear();
        string[] allfiles = Directory.GetFiles(path, "*.heic", SearchOption.AllDirectories);
        double procent = 0;
        double length = allfiles.Length;

        foreach (var file in allfiles)
        {
            FileInfo info = new FileInfo(file);
            using (MagickImage image = new MagickImage(info.FullName))
            {
                image.Write(@$"{destination}\\{info.Name.Split('.')[0]}.jpg");
            }

            int pro = Convert.ToInt32(++procent / length * 100.0);
            Console.SetCursorPosition(0, 0);
            Console.WriteLine(new string('■', pro/5));
            Console.WriteLine(pro + " %");
        }
        Console.WriteLine("Convert Finished");
        Console.ReadKey();
    }
    catch (Exception e)
    {
        Console.WriteLine(e.Message);
    }
}