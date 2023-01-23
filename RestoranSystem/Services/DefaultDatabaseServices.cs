using RestoranSystem.ENum;
using RestoranSystem.Model;
using RestoranSystem.Utilities;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace RestoranSystem.Services
{
    public class DefaultDatabaseServices
    {
        internal List<Table> tables = new List<Table>() {new Table(2),
                                                         new Table(2),
                                                         new Table(4),
                                                         new Table(4),
                                                         new Table(4),
                                                         new Table(6),
                                                         new Table(8)};

        internal List<Meal> Menu = new List<Meal>() {new Meal(MealTypes.Užkandžiai, "Rinkinys prie vyno", 9.50m),
                                                     new Meal(MealTypes.Užkandžiai, "Silkės tartaras su baravykais", 6.50m),
                                                     new Meal(MealTypes.Užkandžiai, "Kepta duona su sūrio padažu", 4.50m),
                                                     new Meal(MealTypes.Salotos, "Cezario salotos su vištiena", 8.50m),
                                                     new Meal(MealTypes.Salotos, "Cezario salotos su krevetėmis", 9.50m),
                                                     new Meal(MealTypes.Karšti, "Dienos sriuba", 3.00m),
                                                     new Meal(MealTypes.Karšti, "Šaltibaršciai su bulvemis", 4.00m),
                                                     new Meal(MealTypes.Karšti, "Bulviniai blynai su sūdyta lašiša", 7.50m),
                                                     new Meal(MealTypes.Karšti, "Mėsainis su plėšyta kiauliena", 8.00m),
                                                     new Meal(MealTypes.Karšti, "Mėsainis su vištiena", 7.00m),
                                                     new Meal(MealTypes.Karšti, "Kiaulienos išpjovos kepsneliai", 9.50m),
                                                     new Meal(MealTypes.Karšti, "Vištienos kepsneliai", 8.50m),
                                                     new Meal(MealTypes.Karšti, "Starkio užkepėlė", 10.50m),
                                                     new Meal(MealTypes.Desertai, "Karštas obuolių pyragas su ledais", 4.00m),
                                                     new Meal(MealTypes.Desertai, "Vaniliniai ledai su padažu", 4.00m),
                                                     new Meal(MealTypes.Gėrimai, "Spanguolių arbata", 2.50m),
                                                     new Meal(MealTypes.Gėrimai, "Kapučinas", 2.50m),
                                                     new Meal(MealTypes.Gėrimai, "Kava su likeriu", 4.80m),
                                                     new Meal(MealTypes.Gėrimai, "Sultys", 2.80m),
                                                     new Meal(MealTypes.Gėrimai, "Alus", 4.50m),
                                                     new Meal(MealTypes.Gėrimai, "Vynas", 2.80m)};


        internal void CreateTablesTable()
        {
            using SQLiteConnection ConnectionToDatabase = SQLiteServices.CreateConnection();
            using SQLiteCommand SQLCommand = ConnectionToDatabase.CreateCommand();
            SQLCommand.CommandText = $"SELECT COUNT(name) FROM sqlite_master WHERE type='table' AND name='Tables';";
            bool isTableExist = Convert.ToBoolean(SQLCommand.ExecuteScalar());
            if (!isTableExist)
            {
                SQLCommand.CommandText = $"CREATE TABLE Tables (" +
                                                        $"TableID INTEGER PRIMARY KEY AUTOINCREMENT, " +
                                                        $"Seats INTEGER, " +
                                                        $"isReserved INTEGER DEFAULT 0, " +
                                                        $"OccupiedSeats INTEGER DEFAULT 0, " +
                                                        $"isOrderAccepted INTEGER DEFAULT 0);";
                SQLCommand.ExecuteNonQuery();

                foreach (Table table in tables)
                {
                    //SQLCommand.CommandText = $"INSERT INTO Tables (Seats, isReserved) VALUES ({table.Seats}, {table.isReserved});";
                    SQLCommand.CommandText = $"INSERT INTO Tables (Seats) VALUES ({table.Seats});";
                    SQLCommand.ExecuteNonQuery();
                }
            }
        }

        internal void CreateMenuTable()
        {
            using SQLiteConnection ConnectionToDatabase = SQLiteServices.CreateConnection();
            using SQLiteCommand SQLCommand = ConnectionToDatabase.CreateCommand();
            SQLCommand.CommandText = $"SELECT COUNT(name) FROM sqlite_master WHERE type='table' AND name='Menu';";
            bool isTableExist = Convert.ToBoolean(SQLCommand.ExecuteScalar());
            if (!isTableExist)
            {
                SQLCommand.CommandText = $"CREATE TABLE Menu (" +
                                                        $"MealID INTEGER PRIMARY KEY AUTOINCREMENT, " +
                                                        $"MealTypeID INTEGER," +
                                                        $"MealName TEXT(100)," +
                                                        $"MealPrice REAL);";
                SQLCommand.ExecuteNonQuery();

                foreach (Meal meal in Menu)
                {
                    string RealString = Converter.ConvertDecimalToReal(meal.MealPrice);
                    SQLCommand.CommandText = $"INSERT INTO Menu (MealTypeID, MealName, MealPrice) VALUES ({Convert.ToInt32(meal.MealType)}, '{meal.MealName}', {RealString});";
                    SQLCommand.ExecuteNonQuery();
                }
            }
        }

        internal void CreateMealTypesTable()
        {
            using SQLiteConnection ConnectionToDatabase = SQLiteServices.CreateConnection();
            using SQLiteCommand SQLCommand = ConnectionToDatabase.CreateCommand();
            SQLCommand.CommandText = $"SELECT COUNT(name) FROM sqlite_master WHERE type='table' AND name='MealTypes';";
            bool isTableExist = Convert.ToBoolean(SQLCommand.ExecuteScalar());
            if (!isTableExist)
            {
                SQLCommand.CommandText = $"CREATE TABLE MealTypes (" +
                                                        $"MealTypeID INTEGER PRIMARY KEY, " +
                                                        $"MealType TEXT(20));";
                SQLCommand.ExecuteNonQuery();

                foreach (int i in Enum.GetValues(typeof(MealTypes)))
                {
                    SQLCommand.CommandText = $"INSERT INTO MealTypes (MealTypeID, MealType) VALUES ({i}, '{Enum.GetName(typeof(MealTypes), i)}');";
                    SQLCommand.ExecuteNonQuery();
                }
            }
        }

        //internal void CreateOrderGroupsTable()
        //{
        //    using SQLiteConnection ConnectionToDatabase = SQLiteServices.CreateConnection();
        //    using SQLiteCommand SQLCommand = ConnectionToDatabase.CreateCommand();
        //    SQLCommand.CommandText = $"SELECT COUNT(name) FROM sqlite_master WHERE type='table' AND name='OrderGroups';";
        //    bool isTableExist = Convert.ToBoolean(SQLCommand.ExecuteScalar());
        //    if (!isTableExist)
        //    {
        //        SQLCommand.CommandText = $"CREATE TABLE OrderGroups (" +
        //                                                $"OrderGroupID INTEGER PRIMARY KEY AUTOINCREMENT, " +
        //                                                $"TableID INTEGER," +
        //                                                $"AccountingID INTEGER);";
        //        SQLCommand.ExecuteNonQuery();
        //    }
        //}

        internal void CreateAccountingTable()
        {
            using SQLiteConnection ConnectionToDatabase = SQLiteServices.CreateConnection();
            using SQLiteCommand SQLCommand = ConnectionToDatabase.CreateCommand();
            SQLCommand.CommandText = $"SELECT COUNT(name) FROM sqlite_master WHERE type='table' AND name='Accounting';";
            bool isTableExist = Convert.ToBoolean(SQLCommand.ExecuteScalar());
            if (!isTableExist)
            {
                SQLCommand.CommandText = $"CREATE TABLE Accounting (" +
                                                        $"AccountingID INTEGER PRIMARY KEY AUTOINCREMENT, " +
                                                        $"Date TEXT, " +
                                                        $"Time TEXT, " +
                                                        $"Value REAL, " +
                                                        $"SendRaport INTEGER, " +
                                                        $"ClientID INTEGER);";
                SQLCommand.ExecuteNonQuery();
            }
        }

        internal void CreateClientsDataTable()
        {
            using SQLiteConnection ConnectionToDatabase = SQLiteServices.CreateConnection();
            using SQLiteCommand SQLCommand = ConnectionToDatabase.CreateCommand();
            SQLCommand.CommandText = $"SELECT COUNT(name) FROM sqlite_master WHERE type='table' AND name='ClientsData';";
            bool isTableExist = Convert.ToBoolean(SQLCommand.ExecuteScalar());
            if (!isTableExist)
            {
                SQLCommand.CommandText = $"CREATE TABLE ClientsData (" +
                                                        $"ClientID INTEGER PRIMARY KEY AUTOINCREMENT, " +
                                                        $"Mail TEXT);";
                SQLCommand.ExecuteNonQuery();
            }
        }

        internal void CreateOrdersTable()
        {
            using SQLiteConnection ConnectionToDatabase = SQLiteServices.CreateConnection();
            using SQLiteCommand SQLCommand = ConnectionToDatabase.CreateCommand();
            SQLCommand.CommandText = $"SELECT COUNT(name) FROM sqlite_master WHERE type='table' AND name='Orders';";
            bool isTableExist = Convert.ToBoolean(SQLCommand.ExecuteScalar());
            if (!isTableExist)
            {
                SQLCommand.CommandText = $"CREATE TABLE Orders (" +
                                                        $"OrderID INTEGER PRIMARY KEY AUTOINCREMENT, " +
                                                        $"AccountingID INTEGER, " +
                                                        $"TableID INTEGER, " +
                                                        $"MealID INTEGER, " +
                                                        $"isPaid INTEGER);";
                SQLCommand.ExecuteNonQuery();
            }
        }

        public void CreateAllDatabaseTables()
        {
            CreateTablesTable();
            CreateMenuTable();
            CreateMealTypesTable();
            //CreateOrderGroupsTable();
            CreateAccountingTable();
            CreateClientsDataTable();
            CreateOrdersTable();
        }
    }
}
