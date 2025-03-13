
using PAINT.UI;
using System.Runtime.InteropServices;

public static class Program
{
    [DllImport("D:\\PAINT\\bin\\Debug\\net8.0\\clear.dll", CallingConvention = CallingConvention.Cdecl)]
    public static extern void clear();
    public static void Main(string[] args)
    {
        PaintApp app = new PaintApp();
        app.Run();
    }
}
