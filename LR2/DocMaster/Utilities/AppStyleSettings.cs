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

        private AppStyleSettings()
        {
            // Устанавливаем дефолтные цвета
            BackgroundColor = ConsoleColor.Black;
            ApplyColors(); // Применяем сразу
        }

        public void ChangeColors(ConsoleColor background)
        {
            BackgroundColor = background;
            ApplyColors();
        }

        private void ApplyColors()
        {
            Console.BackgroundColor = BackgroundColor;
            Console.Clear(); // Важно: очищаем консоль для применения фона
        }
        
    }
}
