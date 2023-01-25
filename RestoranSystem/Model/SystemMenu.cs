using RestoranSystem.Services;
using RestoranSystem.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestoranSystem.Model
{
    public class SystemMenu
    {
        public void GetPrimaryMenu()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Sveiki, tai Restorano valdymo sistema.");
            Console.WriteLine("Galimi veiksmai:");

            Console.WriteLine("[1] Klientų priėmimas / staliuko rezervavimas");
            Console.WriteLine("[2] Užsakymo priėmimas");
            Console.WriteLine("[3] Atsiskaitymo priėmimas");
            Console.WriteLine("[0] Išeiti iš programos");

            int MenuChoice = InputValidation.ValidateInput(4);
            OpenMenuByPrimaryChoice(MenuChoice);
        }

        public void OpenMenuByPrimaryChoice(int choise)
        {
            while (true)
            {
                Console.Clear();
                switch (choise)
                {
                    case 1:
                        CustomerReceptionServices OrderTable = new CustomerReceptionServices();
                        OrderTable.InitializeReception();
                        break;
                    case 2:
                        CustomerOrderServices Order = new CustomerOrderServices();
                        Order.InitializeOrder();
                        break;
                    case 3:
                        CustomerCheckoutServices Checkout = new CustomerCheckoutServices();
                        Checkout.InitializeCheckout();
                        break;
                    //case 4:
                    //    Console.ForegroundColor = ConsoleColor.Green;
                    //    Console.WriteLine("Ataskaitų menu");
                    //    break;
                    default:
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.WriteLine("Viso gero.");
                        Environment.Exit(0);
                        break;
                }
            }
        }

        public void BackToMainMenu()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Grįžti į pradinį meniu.");
            Console.ReadLine();
            GetPrimaryMenu();
        }
    }
}
