using System;

namespace ClassLib
{
    public class TicketSale
    {
        public int id;
        public decimal grandprizeamt;
        public DateTime startts;
        public DateTime endts;
        public long level0;
        public long level1;
        public long level2;
        public long level3;
        public long level4;
        public long level5;
        public long level6;
        public long level7;
        public long level8;
        public long level9;

        public TicketSale(int id, decimal grandprizeamt, DateTime startts, DateTime endts, long level0, long level1, long level2, long level3, long level4, long level5, long level6, long level7, long level8, long level9)
        {
            this.id = id;
            this.grandprizeamt = grandprizeamt;
            this.startts = startts;
            this.endts = endts;
            this.level0 = level0;
            this.level1 = level1;
            this.level2 = level2;
            this.level3 = level3;
            this.level4 = level4;
            this.level5 = level5;
            this.level6 = level6;
            this.level7 = level7;
            this.level8 = level8;
            this.level9 = level9;
        }

        public override bool Equals(object obj)
        {
            return obj is TicketSale other &&
                   id == other.id &&
                   grandprizeamt == other.grandprizeamt &&
                   startts == other.startts &&
                   endts == other.endts &&
                   level0 == other.level0 &&
                   level1 == other.level1 &&
                   level2 == other.level2 &&
                   level3 == other.level3 &&
                   level4 == other.level4 &&
                   level5 == other.level5 &&
                   level6 == other.level6 &&
                   level7 == other.level7 &&
                   level8 == other.level8 &&
                   level9 == other.level9;
        }

        public override int GetHashCode()
        {
            HashCode hash = new HashCode();
            hash.Add(id);
            hash.Add(grandprizeamt);
            hash.Add(startts);
            hash.Add(endts);
            hash.Add(level0);
            hash.Add(level1);
            hash.Add(level2);
            hash.Add(level3);
            hash.Add(level4);
            hash.Add(level5);
            hash.Add(level6);
            hash.Add(level7);
            hash.Add(level8);
            hash.Add(level9);
            return hash.ToHashCode();
        }

        public void Deconstruct(out int id, out decimal grandprizeamt, out DateTime startts, out DateTime endts, out long level0, out long level1, out long level2, out long level3, out long level4, out long level5, out long level6, out long level7, out long level8, out long level9)
        {
            id = this.id;
            grandprizeamt = this.grandprizeamt;
            startts = this.startts;
            endts = this.endts;
            level0 = this.level0;
            level1 = this.level1;
            level2 = this.level2;
            level3 = this.level3;
            level4 = this.level4;
            level5 = this.level5;
            level6 = this.level6;
            level7 = this.level7;
            level8 = this.level8;
            level9 = this.level9;
        }

        public static implicit operator (int id, decimal grandprizeamt, DateTime startts, DateTime endts, long level0, long level1, long level2, long level3, long level4, long level5, long level6, long level7, long level8, long level9)(TicketSale value)
        {
            return (value.id, value.grandprizeamt, value.startts, value.endts, value.level0, value.level1, value.level2, value.level3, value.level4, value.level5, value.level6, value.level7, value.level8, value.level9);
        }

        public static implicit operator TicketSale((int id, decimal grandprizeamt, DateTime startts, DateTime endts, long level0, long level1, long level2, long level3, long level4, long level5, long level6, long level7, long level8, long level9) value)
        {
            return new TicketSale(value.id, value.grandprizeamt, value.startts, value.endts, value.level0, value.level1, value.level2, value.level3, value.level4, value.level5, value.level6, value.level7, value.level8, value.level9);
        }
    }
}
