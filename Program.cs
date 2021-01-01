using System;
using System.ComponentModel;
using System.IO;
using FolderPreservationCut.Classes;

namespace FolderPreservationCut
{
    class Program
    {
        public static void Main(string[] args)  // two arguments - first is start location, second is end location
        {
            Fpc fpc = new Fpc(new FileProvider());
            try
            {
                fpc.Cut(args[0], args[1]);
            }
            catch (IndexOutOfRangeException)
            {
                Console.WriteLine($"+++ Not all arguments provided - quitting... +++");
            }
            catch (FileNotFoundException error)
            {
                Console.WriteLine($"+++ {error.Message} - quitting... +++");
            }
            catch (Exception error)     // generic catch all
            {
                Console.WriteLine($"+++ Unknown Error: {error.Message} - quitting... +++");
            }
            
        }
        
    }
}
