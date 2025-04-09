using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocMaster.Utilities
{
    public sealed class AppStyleSettings
    {
        private static readonly Lazy<AppStyleSettings> _instance =
            new Lazy<AppStyleSettings>(() => new AppStyleSettings());

        public static AppStyleSettings Instance => _instance.Value;

        // Добавляем фоновый цвет
        public ConsoleColor BackgroundColor { get; private set; }
        public ConsoleColor ForegroundColor { get; private set; }

        private AppStyleSettings()
        {
            // Устанавливаем дефолтные цвета
            BackgroundColor = ConsoleColor.Black;
            ForegroundColor = ConsoleColor.White;
            ApplyColors(); // Применяем сразу
        }

        public void ChangeColors(ConsoleColor background, ConsoleColor foreground)
        {
            BackgroundColor = background;
            ForegroundColor = foreground;
            ApplyColors();
        }

        private void ApplyColors()
        {
            Console.BackgroundColor = BackgroundColor;
            Console.ForegroundColor = ForegroundColor;
            Console.Clear(); // Важно: очищаем консоль для применения фона
        }
    }
}
