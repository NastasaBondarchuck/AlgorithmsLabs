namespace lab2
{
    public class queueNode
    {
        public Cell Cell;
        public int Distance;

        public queueNode( Cell cell, int distance)
        {
            Cell = cell;
            Distance = distance;
        }
    }
}