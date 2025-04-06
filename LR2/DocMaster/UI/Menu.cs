
namespace DocMaster.UI
{
    public class Menu
    {
        public void ShowMainMenu(UserRole role)
        {
            //Console.Clear();
            Console.WriteLine("\n\n=== DocMaster ===");

            if(role == UserRole.Viewer)
            {
                Console.WriteLine("1. View document");
                Console.WriteLine("2. Exit");
            }

            if (role == UserRole.Editor)
            {
                Console.WriteLine("1. Create document");
                Console.WriteLine("2. Open document");
                Console.WriteLine("3. Delete document");
                Console.WriteLine("4. Edit document");
                Console.WriteLine("5. Save document");
                Console.WriteLine("6. Text formatting");
                Console.WriteLine("7. Exit");
            }

            if (role == UserRole.Admin)
            {
                Console.WriteLine("1. Create document");
                Console.WriteLine("2. Open document");
                Console.WriteLine("3. Delete document");
                Console.WriteLine("4. Edit document");
                Console.WriteLine("5. Save document");
                Console.WriteLine("6. Text formatting");
                Console.WriteLine("7. User management");
                Console.WriteLine("8. System settings");
                Console.WriteLine("9. Exit");
            }

            Console.Write("Choice: ");
        }
        public void ShowAdminMenu()
        {
            Console.WriteLine("\n=== Admin Dashboard ===");
            Console.WriteLine("1. Change user role");
            Console.WriteLine("2. View all users");
            Console.WriteLine("3. Block document");
            Console.Write("Choice: ");
        }
        public void ShowSaveMenu()
        {
            Console.WriteLine("\n=== Save Dashboard ===");
            Console.WriteLine("1. Save in current format");
            Console.WriteLine("2. Convert and save in another format");
            Console.WriteLine("3. Select storage location");
            Console.Write("Choice: ");
        }
    }
}
