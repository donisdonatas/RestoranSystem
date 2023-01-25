namespace RestoranSystem.Model
{
    public class Table
    {
        public int TableID;
        public int Seats;
        public bool isReserved;
        public bool isOrderAccepted;

        public Table(int seats)
        {
            Seats = seats;
            isReserved = false;
            isOrderAccepted = false;
        }
    }
}
