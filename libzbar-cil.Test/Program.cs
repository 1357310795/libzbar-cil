using System.Drawing;
using System.Drawing.Imaging;

namespace libzbar_cil.Test
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");
            DecodeByZbar(new Bitmap("testimage.png"));
        }

        private static void DecodeByZbar(Bitmap img)
        {
            Bitmap pImg = MakeGrayscale3(img);
            using (ZBar.ImageScanner scanner = new ZBar.ImageScanner())
            {
                //scanner.SetConfiguration(ZBar.SymbolType.None, ZBar.Config.Enable, 0);
                //scanner.SetConfiguration(ZBar.SymbolType.CODE39, ZBar.Config.Enable, 1);
                //scanner.SetConfiguration(ZBar.SymbolType.CODE128, ZBar.Config.Enable, 1);
                //scanner.SetConfiguration(ZBar.SymbolType.QRCODE, ZBar.Config.Enable, 1);

                List<ZBar.Symbol> symbols = new List<ZBar.Symbol>();
                symbols = scanner.Scan(pImg);
                pImg.Dispose();

                if (symbols != null && symbols.Count > 0)
                {
                    foreach(var symbol in symbols)
                    {
                        Console.WriteLine($"Data: {symbol.Data}");
                        Console.WriteLine($"Quality: {symbol.Quality}");
                        Console.WriteLine($"Type: {symbol.Type}");
                        Console.Write($"Location: ");
                        PrintPointsCStyle(symbol.Location);
                    }
                        
                }
                else
                {
                    Console.WriteLine("No code detected!");
                }
            }
        }

        public static Bitmap MakeGrayscale3(Bitmap original)
        {
            //create a blank bitmap the same size as original
            Bitmap newBitmap = new Bitmap(original.Width, original.Height);

            //get a graphics object from the new image
            Graphics g = Graphics.FromImage(newBitmap);

            //create the grayscale ColorMatrix
            System.Drawing.Imaging.ColorMatrix colorMatrix = new System.Drawing.Imaging.ColorMatrix(
              new float[][]
              {
                 new float[] {.3f, .3f, .3f, 0, 0},
                 new float[] {.59f, .59f, .59f, 0, 0},
                 new float[] {.11f, .11f, .11f, 0, 0},
                 new float[] {0, 0, 0, 1, 0},
                 new float[] {0, 0, 0, 0, 1}
              });

            //create some image attributes
            ImageAttributes attributes = new ImageAttributes();

            //set the color matrix attribute
            attributes.SetColorMatrix(colorMatrix);

            //draw the original image on the new image
            //using the grayscale color matrix
            g.DrawImage(original, new Rectangle(0, 0, original.Width, original.Height),
               0, 0, original.Width, original.Height, GraphicsUnit.Pixel, attributes);

            //dispose the Graphics object
            g.Dispose();
            original.Dispose();
            return newBitmap;
        }

        public static void PrintPointsCStyle(List<Point> points)
        {
            Console.WriteLine("{");
            foreach (Point point in points)
            {
                Console.WriteLine($" {{ {point.X}, {point.Y} }},");
            }
            Console.WriteLine("}");
        }
    }
}