using RestoranSystem.Model;
using RestoranSystem.Reports;
using RestoranSystem.Struct;
using RestoranSystem.Utilities;
using System;
using System.Collections.Generic;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace RestoranSystem.Services
{
    public class CustomerCheckoutServices
    {

        private int _tableId = 0;
        private bool _isCheckNeed = false;
        private MailAddress? _clientEmail;
        private List<ReservedTable>? _reservedTables;
        private int _accountingId;
        private int _clientId;

        public int TableId
        {
            get { return _tableId; }
            set { _tableId = value; }
        }

        public int ClientId
        {
            get { return _clientId; }
            set { _clientId = value; }
        }

        public bool IsCheckNeed
        {
            get { return _isCheckNeed; }
            set { _isCheckNeed = value; }
        }

        public MailAddress ClientEmail
        {
            get { return _clientEmail; }
            set { _clientEmail = value; }
        }

        public void InitializeCheckout()
        {
            SystemMenu Menu = new SystemMenu();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Atsiskaitymo priėmimas");
            Console.WriteLine("Pasirinkite staliuką užsakymo atsiskaitymui:");
            TableId = SelectOrderedTable();
            if (TableId != 0)
            {
                IsCheckNeed = AskCheckNeed();
                //ShowTotalPaidValue();
                CompleteCheckout();
            }
            Menu.BackToMainMenu();
        }

        protected int SelectOrderedTable()
        {
            List<AvailableTable> OrderedTables = SQLiteServices.GetOrderedTableList();
            if (OrderedTables.Any())
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine("-----------------");
                int KeyboardKey = 0;
                foreach (AvailableTable table in OrderedTables)
                {
                    ++KeyboardKey;
                    Console.WriteLine($"[{KeyboardKey}] Staliukas Nr. {table.TableID}. Laukia {table.OccupiedSeats} žmonės.");
                }
                Console.WriteLine("-----------------");
                InputValidation.ValidateInput(KeyboardKey);
                _accountingId = OrderedTables[KeyboardKey - 1].AccountingID;
                return OrderedTables[KeyboardKey - 1].TableID;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine("Nėra staliukų laukiančių atsiskaitymo");
                return 0;
            }
        }

        protected bool AskCheckNeed()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Ar reikia atsiųsti sąskaitos išrašą?");
            Console.WriteLine("[1] Taip");
            Console.WriteLine("[2] Ne");
            int KeyboardKey = InputValidation.ValidateInput(2);
            return OpenMenuByChoice(KeyboardKey);
        }

        protected bool OpenMenuByChoice(int key)
        {
            switch(key)
            {
                case 1: 
                    AskMailInfo();
                    return true;
                default:
                    return false;
            }
        }

        protected void AskMailInfo()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Įveskite email:");
            //MailAddress? ClientEmail;
            bool isValidEmail = false;
            Exception? err = null;
            while(!isValidEmail)
            {
                try
                {
                    Console.ForegroundColor = ConsoleColor.White;
                    ClientEmail = new MailAddress(Console.ReadLine());
                }
                catch (Exception ex)
                {
                    err = ex;
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Neteisingai įvestas email adresas. Pakartokite.");
                }
                finally
                {
                    if (err == null)
                    {
                        isValidEmail = true;
                        WriteEmailToDB(ClientEmail);// Čia reikėtų įrašyti email į duomenų bazę??
                        SendBillByMail();
                    }
                }
            }
        }

        protected decimal ShowTotalPaidValue()
        {
            List<MenuLine> Order = GetOrderBill();
            decimal TotalPaid = 0.00m;
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("-----------------");
            foreach (MenuLine line in Order)
            {
                Console.WriteLine($"{line.MealName} - {line.MealPrice}");
                TotalPaid += line.MealPrice;
            }
            Console.WriteLine("--------");
            Console.WriteLine($"Viso mokėti: {TotalPaid}Eur");
            Console.WriteLine("-----------------");
            return TotalPaid;
        }

        private List<MenuLine> GetOrderBill()
        {
            List<MenuLine> OrderBill = SQLiteServices.GetOrderBillList(TableId);
            return OrderBill;
        }

        private int GetAccountingID()
        {
            int AccountingID = SQLiteServices.GetOrderAccountingID(TableId);
            return AccountingID;
        }

        private void WriteEmailToDB(MailAddress email)
        {
            string WriteString = $"INSERT INTO ClientsData (Mail) VALUES ('{email}');";
            SQLiteServices.WriteToSQLiteDB(WriteString);
        }

        private int GetClientId()
        {
            int ClientId = 0;
            if(IsCheckNeed)
            {
                ClientId = SQLiteServices.GetClientIdByEmail(ClientEmail);
            }
            return ClientId;
        }

        protected void CompleteCheckout()
        {
            //Console.WriteLine($"UPDATE Orders SET isPaid=1 WHERE TableID={TableId} AND isPaid=0;");
            //Console.WriteLine($"UPDATE Tables SET isReserved=0, OccupiedSeats=0, isOrderAccepted=0 WHERE TableID={TableId};");
            //Console.WriteLine($"UPDATE Accounting SET Time='{DateTime.Now:HH:mm}', Value={Converter.ConvertDecimalToReal(ShowTotalPaidValue())}, SendRaport={IsCheckNeed}, ClientID={GetClientId()} WHERE AccountingID={GetAccountingID()};");
            string OrdersUpdateString = $"UPDATE Orders SET isPaid=1 WHERE TableID={TableId} AND isPaid=0;";
            string TablesUpdateString = $"UPDATE Tables SET isReserved=0, OccupiedSeats=0, isOrderAccepted=0 WHERE TableID={TableId};";
            string AccountingUpdateString = $"UPDATE Accounting SET Time='{DateTime.Now:HH:mm}', Value={Converter.ConvertDecimalToReal(ShowTotalPaidValue())}, SendRaport={IsCheckNeed}, ClientID={GetClientId()} WHERE AccountingID={GetAccountingID()};";
            SQLiteServices.UpdateSQLTable(OrdersUpdateString);
            SQLiteServices.UpdateSQLTable(TablesUpdateString);
            SQLiteServices.UpdateSQLTable(AccountingUpdateString);

            // ++Dar reikia peržvelti kur padaryti db atnaujinimą, Orders lentelėje ir Table lentelėje.
            // ++Oreders reikia pažymėti, kad isPaid=1
            // ++Table lentelėje reikia updeitinti isReserved=0, OccupiedSeats=0, isOrderAccepted=0
            // Accouting reikia įrašyti Value=TotalPaid, beje dar SendReport, ir ClientID
            // Jei SendReport=true, tada dar į ClientsInfo įrašyti ClientMail ir galiausiai padaryti HTML reportą ir nusiųsti šiuo adresu.
        }

        protected void SendBillByMail()
        {
            HtmlBill GenerateHtmlBill = new HtmlBill();
            EmailService SendMail = new EmailService();
            SendMail.SendEmail(GenerateHtmlBill.generateHTMLraport(GetOrderBill()), ClientEmail);
        }
    }
}
