using RestoranSystem.Model;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestoranSystem.Services
{
    public class SQLiteDBCheck
    {
        internal void CheckTablesTableColumnTypes()
        {
            using SQLiteConnection ConnectionToDatabase = SQLiteServices.CreateConnection();
            using SQLiteCommand SQLCommand = ConnectionToDatabase.CreateCommand();

            SQLiteDataReader SQLiteReader;
            SQLCommand.CommandText = $"SELECT TableID, typeof(TableID), Seats, typeof(Seats), isReserved, typeof(isReserved) FROM Tables;";
            SQLiteReader = SQLCommand.ExecuteReader();

            Console.WriteLine($"TableID | TableIDType | Seats | SeatsType | isReserved | isReservedType");
            while (SQLiteReader.Read())
            {
                int TableID = Convert.ToInt32(SQLiteReader[0]);
                string? TableIDType = Convert.ToString(SQLiteReader[1]);
                int Seats = Convert.ToInt32(SQLiteReader[2]);
                string? SeatsType = Convert.ToString(SQLiteReader[3]);
                bool isReserved = Convert.ToBoolean(SQLiteReader[4]);
                string? isReservedType = Convert.ToString(SQLiteReader[5]);
                Console.WriteLine($"{TableID} | {TableIDType} | {Seats} | {SeatsType} | {isReserved} | {isReservedType}");
            }
        }

        internal void CheckMenuTableColumnTypes()
        {
            using SQLiteConnection ConnectionToDatabase = SQLiteServices.CreateConnection();
            using SQLiteCommand SQLCommand = ConnectionToDatabase.CreateCommand();

            SQLiteDataReader SQLiteReader;
            SQLCommand.CommandText = $"SELECT MealID, typeof(MealID), MealTypeID, typeof(MealTypeID), MealName, typeof(MealName), MealPrice, typeof(MealPrice) FROM Menu;";
            SQLiteReader = SQLCommand.ExecuteReader();

            Console.WriteLine($"MealID | MealIDType | MealTypeID | MealTypeIDType | MealName | MealNameType | MealPrice | MealPriceType");
            while (SQLiteReader.Read())
            {
                int MealID = Convert.ToInt32(SQLiteReader[0]);
                string? MealIDType = Convert.ToString(SQLiteReader[1]);
                int MealTypeID = Convert.ToInt32(SQLiteReader[2]);
                string? MealTypeIDType = Convert.ToString(SQLiteReader[3]);
                string? MealName = Convert.ToString(SQLiteReader[4]);
                string? MealNameType = Convert.ToString(SQLiteReader[5]);
                decimal MealPrice = Convert.ToDecimal(SQLiteReader[6]);
                string? MealPriceType = Convert.ToString(SQLiteReader[7]);
                Console.WriteLine($"{MealID} | {MealIDType} | {MealTypeID} | {MealTypeIDType} | {MealName} | {MealNameType} | {MealPrice} | {MealPriceType}");
            }
        }
    }
}
