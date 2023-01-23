using RestoranSystem.ENum;
using RestoranSystem.Model;
using RestoranSystem.Struct;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace RestoranSystem.Services
{
    public static class SQLiteServices
    {

        public static SQLiteConnection CreateConnection()
        {
            SQLiteConnection SQLiteConn = new SQLiteConnection("Data Source = restaurant.db; Version = 3; New = True; Compress = True;");
            try
            {
                SQLiteConn.Open();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"There was error connecting to database. Error code: {ex.Message}");
            }
            return SQLiteConn;
        }

        public static List<AvailableTable> GetAvailableTableList()
        {
            using SQLiteConnection ConnectionToDatabase = CreateConnection();
            using SQLiteCommand SQLCommand = ConnectionToDatabase.CreateCommand();

            List<AvailableTable> FreeTableSeats = new List<AvailableTable>();
            SQLiteDataReader SQLiteReader;
            SQLCommand.CommandText = $"SELECT TableID, Seats FROM Tables WHERE isReserved=0;";
            SQLiteReader = SQLCommand.ExecuteReader();

            while (SQLiteReader.Read())
            {
                AvailableTable availableTable = new AvailableTable();
                availableTable.TableID = Convert.ToInt32(SQLiteReader[0]);
                availableTable.Seats = Convert.ToInt32(SQLiteReader[1]);
                FreeTableSeats.Add(availableTable);
            }

            return FreeTableSeats;
        }

        public static List<AvailableTable> GetReservedTableList()
        {
            using SQLiteConnection ConnectionToDatabase = CreateConnection();
            using SQLiteCommand SQLCommand = ConnectionToDatabase.CreateCommand();

            List<AvailableTable> ReservedSeats = new List<AvailableTable>();
            SQLiteDataReader SQLiteReader;
            SQLCommand.CommandText = $"SELECT TableID, OccupiedSeats FROM Tables WHERE isReserved=1 AND isOrderAccepted=0;";
            SQLiteReader = SQLCommand.ExecuteReader();

            while (SQLiteReader.Read())
            {
                AvailableTable availableTable = new AvailableTable();
                availableTable.TableID = Convert.ToInt32(SQLiteReader[0]);
                availableTable.OccupiedSeats = Convert.ToInt32(SQLiteReader[1]);
                ReservedSeats.Add(availableTable);
            }

            return ReservedSeats;
        }

        public static List<ReservedTable> GetReservedTables()
        {
            using SQLiteConnection ConnectionToDatabase = CreateConnection();
            using SQLiteCommand SQLCommand = ConnectionToDatabase.CreateCommand();

            List<ReservedTable> ReservedTables = new List<ReservedTable>();
            SQLiteDataReader SQLiteReader;
            SQLCommand.CommandText = $"SELECT TableID FROM Tables WHERE isReserved=1 AND isOrderAccepted=0;";
            SQLiteReader = SQLCommand.ExecuteReader();
            if(SQLiteReader.StepCount != 0)
            {
                while (SQLiteReader.Read())
                {
                    ReservedTable Table = new ReservedTable();
                    Table.TableID = Convert.ToInt32(SQLiteReader[0]);
                    ReservedTables.Add(Table);
                }
            }
            return ReservedTables;
        }

        public static void UpdateSQLTable(string sqlString)
        {
            using SQLiteConnection ConnectionToDatabase = CreateConnection();
            using SQLiteCommand SQLCommand = ConnectionToDatabase.CreateCommand();

            SQLCommand.CommandText = sqlString;
            SQLCommand.ExecuteNonQuery();
        }

        //public static void ReserveTable(int tableID, int customers)
        //{
        //    using SQLiteConnection ConnectionToDatabase = CreateConnection();
        //    using SQLiteCommand SQLCommand = ConnectionToDatabase.CreateCommand();

        //    SQLCommand.CommandText = $"UPDATE Tables SET isReserved=1, OccupiedSeats={customers} WHERE TableID={tableID};";
        //    SQLCommand.ExecuteNonQuery();
        //}

        public static List<MenuLine> GetRestaurantMenu()
        {
            using SQLiteConnection ConnectionToDatabase = CreateConnection();
            using SQLiteCommand SQLCommand = ConnectionToDatabase.CreateCommand();

            List<MenuLine> Menu = new List<MenuLine>();
            SQLiteDataReader SQLiteReader;
            SQLCommand.CommandText = $"SELECT MealType, MealName, MealPrice FROM MealTypes JOIN Menu ON Menu.MealTypeID = MealTypes.MealTypeID;";
            SQLiteReader = SQLCommand.ExecuteReader();

            while (SQLiteReader.Read())
            {
                MenuLine MenuRow = new MenuLine();
                MenuRow.MealType = Convert.ToString(SQLiteReader[0]);
                MenuRow.MealName = Convert.ToString(SQLiteReader[1]);
                MenuRow.MealPrice = Convert.ToDecimal(SQLiteReader[2]);
                Menu.Add(MenuRow);
            }

            return Menu;
        }

        public static List<AvailableTable> GetOrderedTableList()
        {
            using SQLiteConnection ConnectionToDatabase = CreateConnection();
            using SQLiteCommand SQLCommand = ConnectionToDatabase.CreateCommand();

            List<AvailableTable> ReservedSeats = new List<AvailableTable>();
            SQLiteDataReader SQLiteReader;
            SQLCommand.CommandText = $"SELECT DISTINCT Tables.TableID, OccupiedSeats, AccountingID FROM Tables JOIN Orders ON Orders.TableID=Tables.TableID WHERE isOrderAccepted=1 AND isPaid=0;";
            SQLiteReader = SQLCommand.ExecuteReader();

            while (SQLiteReader.Read())
            {
                AvailableTable availableTable = new AvailableTable();
                availableTable.TableID = Convert.ToInt32(SQLiteReader[0]);
                availableTable.OccupiedSeats = Convert.ToInt32(SQLiteReader[1]);
                availableTable.AccountingID = Convert.ToInt32(SQLiteReader[2]);
                ReservedSeats.Add(availableTable);
            }

            return ReservedSeats;
        }

        public static void GenerateNewAccountingID()
        {
            using SQLiteConnection ConnectionToDatabase = CreateConnection();
            using SQLiteCommand SQLCommand = ConnectionToDatabase.CreateCommand();
            SQLCommand.CommandText = $"INSERT INTO Accounting (Date) VALUES ('{DateTime.Today:yyyy-MM-dd}');";
            SQLCommand.ExecuteNonQuery();
        }

        //public static int SelectLastAccountingID()
        //{
        //    using SQLiteConnection ConnectionToDatabase = CreateConnection();
        //    using SQLiteCommand SQLCommand = ConnectionToDatabase.CreateCommand();
        //    SQLiteDataReader SQLiteReader;
        //    SQLCommand.CommandText = $"SELECT MAX(AccountingID) FROM Accounting;";
        //    SQLiteReader = SQLCommand.ExecuteReader();
        //    int AccountingID = 0;
        //    while (SQLiteReader.Read())
        //    {
        //        AccountingID = Convert.ToInt32(SQLiteReader[0]);
        //    }
        //    return AccountingID;
        //}

        public static void WriteOrderToDB(int tableId, List<int> order)
        {
            using SQLiteConnection ConnectionToDatabase = CreateConnection();
            using SQLiteCommand SQLCommand = ConnectionToDatabase.CreateCommand();
            foreach(int o in order)
            {
                SQLCommand.CommandText = $"INSERT INTO Orders (AccountingID, TableID, MealID) VALUES ((SELECT MAX(AccountingID) FROM Accounting), {tableId}, {o});";
                SQLCommand.ExecuteNonQuery();
            }
        }

        public static List<MenuLine> GetOrderBillList(int tableId)
        {
            using SQLiteConnection ConnectionToDatabase = CreateConnection();
            using SQLiteCommand SQLCommand = ConnectionToDatabase.CreateCommand();
            List<MenuLine> OrderedItems = new List<MenuLine>();
            SQLiteDataReader SQLiteReader;
            SQLCommand.CommandText = $"SELECT Orders.MealId, MealName, MealPrice FROM Orders JOIN Menu ON Orders.MealID=Menu.MealID WHERE TableID={tableId} AND isPaid=0;";
            SQLiteReader = SQLCommand.ExecuteReader();
            while (SQLiteReader.Read())
            {
                MenuLine OrderedLine = new MenuLine();
                OrderedLine.MealId = Convert.ToInt32(SQLiteReader[0]);
                OrderedLine.MealName = Convert.ToString(SQLiteReader[1]);
                OrderedLine.MealPrice = Convert.ToDecimal(SQLiteReader[2]);
                OrderedItems.Add(OrderedLine);
            }
            return OrderedItems;
        }

        public static int GetOrderAccountingID(int tableId)
        {
            using SQLiteConnection ConnectionToDatabase = CreateConnection();
            using SQLiteCommand SQLCommand = ConnectionToDatabase.CreateCommand();
            int AccountingID = 0;
            SQLiteDataReader SQLiteReader;
            SQLCommand.CommandText = $"SELECT DISTINCT AccountingID FROM Orders WHERE TableID={tableId} AND isPaid=0;";
            SQLiteReader = SQLCommand.ExecuteReader();

            while (SQLiteReader.Read())
            {
                AccountingID = Convert.ToInt32(SQLiteReader[0]);
            }

            return AccountingID;
        }

        public static int GetClientIdByEmail(MailAddress clientEmail)
        {
            using SQLiteConnection ConnectionToDatabase = CreateConnection();
            using SQLiteCommand SQLCommand = ConnectionToDatabase.CreateCommand();
            int ClientID = 0;
            SQLiteDataReader SQLiteReader;
            SQLCommand.CommandText = $"SELECT ClientID FROM ClientsData WHERE Mail='{clientEmail}' ;";
            SQLiteReader = SQLCommand.ExecuteReader();

            while (SQLiteReader.Read())
            {
                ClientID = Convert.ToInt32(SQLiteReader[0]);
            }

            return ClientID;
        }

        public static void WriteToSQLDB(string commandString)
        {

        }
    }
}
