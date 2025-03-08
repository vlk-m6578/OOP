using System.Text.Json.Serialization;

namespace PAINT.Models
{
    public abstract class Shape
    {
        protected string name;
        protected int x;
        protected int y;
        protected char symbol;
        protected char symbolBackground;
}
